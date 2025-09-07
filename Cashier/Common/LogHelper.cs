namespace Cashier.Common
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public static class LogHelper
    {
        private static readonly BlockingCollection<(DateTime time, string level, string message)> _logQueue
            = new BlockingCollection<(DateTime, string, string)>(new ConcurrentQueue<(DateTime, string, string)>());

        private static readonly string _baseLogDir = Path.Combine(AppContext.BaseDirectory, "Log");

        private static readonly CancellationTokenSource _cts = new CancellationTokenSource();

        static LogHelper()
        {
            // 启动后台写日志线程
            Task.Factory.StartNew(ProcessQueue, TaskCreationOptions.LongRunning);
        }

        /// <summary>
        /// 投递日志（非阻塞，主线程不关心写入成功与否）
        /// </summary>
        public static void WriteLog(string message, string level = "INFO")
        {
            try
            {
                _logQueue.Add((DateTime.Now, level, message));
            }
            catch
            {
                // 队列满或者已关闭时，丢弃日志
            }
        }

        public static void Info(string message) => WriteLog(message, "INFO");
        public static void Warn(string message) => WriteLog(message, "WARN");
        public static void Error(string message) => WriteLog(message, "ERROR");

        private static void ProcessQueue()
        {
            foreach (var (time, level, message) in _logQueue.GetConsumingEnumerable(_cts.Token))
            {
                try
                {
                    string dayFolder = Path.Combine(_baseLogDir, time.ToString("yyyy-MM-dd"));
                    if (!Directory.Exists(dayFolder))
                    {
                        Directory.CreateDirectory(dayFolder);
                    }

                    string filePath = Path.Combine(dayFolder, $"{time:HH}.txt");

                    string logText = $"[{time:yyyy-MM-dd HH:mm:ss.fff}] [{level}] {message}{Environment.NewLine}";
                    File.AppendAllText(filePath, logText, Encoding.UTF8);
                }
                catch
                {
                    // 后台写入失败直接吞掉，不影响主流程
                }
            }
        }

        /// <summary>
        /// 优雅关闭日志系统（可在应用退出时调用）
        /// </summary>
        public static void Shutdown()
        {
            _cts.Cancel();
            _logQueue.CompleteAdding();
        }
    }

}
