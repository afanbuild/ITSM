using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Epower.DevBase.BaseTools
{
    /// <summary>
    /// 工具类,方便取值,转换
    /// </summary>
    public static class CTools
    {
        private static Random randNums = new Random();

        /// <summary>
        /// 得到一个随机整数        /// </summary>
        /// <returns>返回随机整数</returns>
        public static int GetRandom()
        {
            return randNums.Next();
        }

        /// <summary>
        /// 得到一个随机整数        /// </summary>
        /// <param name="minValue">随机数下界,生成后的随机数会大于该值</param>
        /// <param name="maxValue">随机数上界,生成后的随机数会小于等于该值</param>
        /// <returns>返回随机整数</returns>
        public static int GetRandom(int minValue, int maxValue)
        {
            return randNums.Next(minValue, maxValue);
        }

        /// <summary>
        /// 得到一个随机字符数字        /// </summary>
        /// <returns>返回随机字符数字</returns>
        public static string GetRandom2()
        {
            return DateTime.Now.ToString("dHms");
        }

        /// <summary>
        /// 判断是否是数字        /// </summary>
        /// <param name="numbers">需要判断为整型的字符串</param>
        /// <returns>返回true:是数字, false:不是数字</returns>
        public static bool IsNumber(string numbers)
        {
            if (numbers != null && Regex.IsMatch(numbers, @"^\d*$"))
                return true;
            return false;
        }

        /// <summary>
        /// 字符串转换为整型
        /// </summary>
        /// <param name="numbers">需要转换的字符串</param>
        /// <returns>返回整型,出错失败则返回0</returns>
        public static int ToInt(string numbers)
        {
            return ToInt(numbers, 0);
        }

        /// <summary>
        /// 字符串转换为整形
        /// </summary>
        /// <param name="numbers">需要转换的字符串</param>
        /// <param name="ndefault">默认值</param>
        /// <returns>返回整型,出错失败则返回ndefault</returns>
        public static int ToInt(string numbers, int ndefault)
        {
            int result = 0;
            if (int.TryParse(numbers, out result))
                return result;
            return ndefault;
        }
        /// <summary>
        /// 转换成长整形
        /// </summary>
        /// <param name="numbers">需要转换的字符串</param>
        /// <returns>返回长整型,出错失败则返回0</returns>
        public static long ToInt64(string numbers)
        {
            return ToInt64(numbers, 0);
        }

        /// <summary>
        /// 转换成长整形
        /// </summary>
        /// <param name="numbers">需要转换的字符串</param>
        /// <param name="lDefault">默认值</param>
        /// <returns>返回长整型,出错失败则返回lDefault</returns>
        public static long ToInt64(string numbers, long lDefault)
        {
            long result = 0;
            if (long.TryParse(numbers, out result))
                return result;
            return lDefault;
        }

        public static decimal ToDecimal(string value)
        {
            return ToDecimal(value, 0);
        }

        public static decimal ToDecimal(string value, decimal defvalue)
        {
            decimal result = 0;
            if (decimal.TryParse(value, out result))
                return result;
            return defvalue;
        }

        /// <summary>
        /// 转换成长整形
        /// </summary>
        /// <param name="numbers">需要转换的double类型</param>
        /// <returns>返回长整型,出错失败则返回0</returns>
        public static long ToInt64(double numbers)
        {
            return ToInt64(numbers.ToString("0"), 0);
        }

        /// <summary>
        /// 转换成时间
        /// </summary>
        /// <param name="time">需要转换的字符串</param>
        /// <returns>返回时间,出错失败则返回DateTime.Now</returns>
        public static DateTime ToDateTime(string time)
        {
            return ToDateTime(time, DateTime.Now);
        }

        /// <summary>
        /// 转换成时间
        /// </summary>
        /// <param name="time">需要转换的字符串</param>
        /// <param name="datetime">默认时间</param>
        /// <returns>返回时间,出错失败则返回datetime</returns>
        public static DateTime ToDateTime(string time, DateTime datetime)
        {
            DateTime result = DateTime.Now;
            if (DateTime.TryParse(time, out result))
                return result;
            return datetime;
        }

        /// <summary>
        /// 按长度取字符串,起始位置为0
        /// </summary>
        /// <param name="content">字符串</param>
        /// <param name="length">长度</param>
        /// <returns>返回截取的字符串,没有达到length,则原样返回</returns>
        public static string Substring(string content, int length)
        {
            if (string.IsNullOrEmpty(content))
                return "";
            if (content.Length <= length)
                return content;
            return content.Substring(0, length);
        }

        /// <summary>
        /// 返回字符串真实长度, 1个汉字长度为2
        /// </summary>
        /// <returns>字符长度</returns>
        public static int Length(string str)
        {
            return Encoding.Default.GetBytes(str).Length;
        }

        public static string HtmlEncode(string value)
        {
            return HttpUtility.HtmlEncode(HttpUtility.UrlDecode(value));
        }


        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="context"></param>
        /// <param name="query"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static string getString(HttpContext context, string query, string def)
        {
            if (context.Request[query] == null)
            {
                return def;
            }
            else
            {
                return context.Request[query].ToString().Trim();
            }
        }

        public static string getString(HttpContext context, string query)
        {
            return getString(context, query, "");
        }

        /// <summary>
        /// 获取Int类型
        /// </summary>
        /// <param name="context"></param>
        /// <param name="query"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static int getInt(HttpContext context, string query, int def)
        {
            if (context.Request[query] == null)
            {
                return def;
            }
            else
            {
                return CTools.ToInt(context.Request[query].ToString(), def);
            }
        }

        /// <summary>
        /// 将图片转成byte[]类型
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static byte[] ReadFileBuffer(HttpPostedFile file)
        {
            ///处理上载的文件流信息。
            byte[] b = new byte[file.ContentLength];
            System.IO.Stream fs;            
            fs = (System.IO.Stream)file.InputStream;
            fs.Position = 0;
            fs.Read(b, 0, file.ContentLength);
            fs.Close();
            return b;
        }
    }
}
