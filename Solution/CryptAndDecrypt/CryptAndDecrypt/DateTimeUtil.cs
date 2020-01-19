using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace EncryptAndDecrypt
{
    public class DateTimeUtil
    {
        /// <summary>
        /// 获取当前时间的UTC时间字符串
        /// </summary>
        /// <returns>2017-06-07T08:47:20.3066594Z</returns>
        public static string GetUTCString()
        {
            return DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture);
        }


        #region 根据UTC时间得到正常的当前时间
        /// <summary>
        /// 根据UTC时间得到正常的当前时间
        /// </summary>
        /// <param name="utcStr">Utc时间：2017-06-07T08:47:20.3066594Z</param>
        /// <returns>datetime型</returns>
        public static DateTime ConvertUTCStrToDatetime(string utcStr)
        {
            DateTime timestamp;
            DateTime.TryParseExact(utcStr, _timestampFormats, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out timestamp);
            return timestamp;
        }
        #endregion

        #region 判断目标时间和给定标准时间的大小关系
        /// <summary>
        /// 判断目标时间和给定标准时间的大小关系
        /// </summary>
        /// <param name="tagTime">目标时间</param>
        /// <param name="stardTime">标准时间</param>
        /// <returns>为真表示目标时间大于标准时间  否则为假</returns>
        public static bool IsVerify(DateTime tagTime, DateTime stardTime)
        {
            if ((tagTime - stardTime).Duration().TotalMilliseconds > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region 判断请求时间和当前时间是否在允许的时间差范围内
        /// <summary>
        /// 判断请求时间和当前时间是否在允许的时间差范围内
        /// </summary>
        /// <param name="requTime">请求时间</param>
        /// <returns>真表示在范围内   假表示不再范围内</returns>
        public static bool IsVerify(DateTime requTime)
        {
            if ((DateTime.UtcNow - requTime).Duration().TotalMilliseconds < int.Parse(System.Configuration.ConfigurationManager.AppSettings["timeout"]))
                return true;
            else
                return false;
        }
        #endregion




        private static readonly string[] _timestampFormats =
        {
            "o",
            "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffK",
            "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffffffK",
            "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffK",
            "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffffK",
            "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffK",
            "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffK",
            "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fK",
            "yyyy'-'MM'-'dd'T'HH':'mm':'ssK",
        };
    }
}
