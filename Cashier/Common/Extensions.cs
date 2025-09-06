using System.Text;

namespace Cashier.Common
{
    public static class Extensions
    {
        /// <summary>
        /// 转为中文格式 "x天x小时x分钟x秒"
        /// </summary>
        public static string ToChineseString(this TimeSpan? timeSpan, bool skipZero = true)
        {
            if (timeSpan == null)
                return "0秒";
            var ts = timeSpan.Value;
            StringBuilder sb = new();

            if (!skipZero || ts.Days > 0)
                sb.Append(ts.Days).Append("天");

            if (!skipZero || ts.Hours > 0)
                sb.Append(ts.Hours).Append("小时");

            if (!skipZero || ts.Minutes > 0)
                sb.Append(ts.Minutes).Append("分钟");

            if (!skipZero || ts.Seconds > 0)
                sb.Append(ts.Seconds).Append("秒");

            // 如果全是 0
            if (sb.Length == 0)
                return "0秒";

            return sb.ToString();
        }
    }
}
