namespace Cashier.Common
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;

    public class DelayedTaskScheduler : IDisposable
    {
        private readonly TimeSpan _delay;
        private readonly Func<string, Task> _execute;
        private readonly int _maxQps;
        private readonly TimeSpan _qpsWindow;

        private readonly CancellationTokenSource _cts = new();
        private readonly ConcurrentQueue<ScheduledTask> _queue = new();
        private readonly SemaphoreSlim _signal = new(0);
        private readonly ConcurrentQueue<DateTime> _scheduleTimes = new();

        private class ScheduledTask(string data = "", bool isOriginal = true)
        {
            public string Data { get; set; } = data;
            public bool IsOriginal { get; set; } = isOriginal;


        }

        public DelayedTaskScheduler(TimeSpan delay, int maxQps, TimeSpan qpsWindow, Func<string, Task> execute)
        {
            _delay = delay;
            _maxQps = maxQps;
            _qpsWindow = qpsWindow;
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));

            _ = Task.Run(ProcessLoopAsync);
        }

        public void Schedule(string data)
        {
            var task = new ScheduledTask(data);

            _queue.Enqueue(task);

            // 只有原始任务计入 QPS
            if (task.IsOriginal)
                _scheduleTimes.Enqueue(DateTime.UtcNow);

            _signal.Release();
        }

        private async Task ProcessLoopAsync()
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                try
                {
                    await _signal.WaitAsync(_cts.Token);

                    if (_queue.TryDequeue(out var task))
                    {
                        await Task.Delay(_delay, _cts.Token);

                        CleanupOldScheduleTimes();
                        var qps = _scheduleTimes.Count / _qpsWindow.TotalSeconds;

                        if (qps > _maxQps)
                        {
                            _queue.Enqueue(new ScheduledTask(task.Data, false));
                            _signal.Release();
                            continue;
                        }

                        _execute(task.Data);
                    }
                }
                catch (OperationCanceledException) { break; }
                catch (Exception ex)
                {
                    LogHelper.Error($"[DelayedTaskScheduler] Exception in ProcessLoopAsync  Error: {ex}");
                }
            }
        }

        private void CleanupOldScheduleTimes()
        {
            var now = DateTime.UtcNow;
            while (_scheduleTimes.TryPeek(out var t) && (now - t) > _qpsWindow)
            {
                _scheduleTimes.TryDequeue(out _);
            }
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
            _signal.Dispose();
        }
    }

}
