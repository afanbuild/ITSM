using System;
using System.IO;
using System.Text;

namespace Epower.DevBase.BaseTools
{
	/// <summary>
	/// Log 的摘要说明。
	/// </summary>
	public class Log
	{
        /// <summary>
        /// 
        /// </summary>
		public Log()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strText"></param>
        public static void WriteLog(string strText)
        {
            E8Logger.Info("======ExchangeService start==========");
            E8Logger.Info(strText);
            E8Logger.Info("======ExchangeService end==========");
            //string fileName = System.Web.HttpContext.Current.Server.MapPath("ExchangeService\\Log")+"\\Log"+System.DateTime.Now.ToShortDateString()+".txt";
            
            //StreamWriter sw = new StreamWriter(fileName, true, Encoding.GetEncoding("GB2312"));            
            //sw.WriteLine(strText);
            //sw.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strText"></param>
        public static void WriteLogByQQ(string strText)
        {
            E8Logger.Info("======WriteLogByQQ start==========");
            E8Logger.Info(strText);
            E8Logger.Info("======WriteLogByQQ end==========");

            //string fileName = System.Web.HttpContext.Current.Server.MapPath("ExchangeService\\Log")+"\\QQLog"+System.DateTime.Now.ToShortDateString()+".txt";
            
            //StreamWriter sw = new StreamWriter(fileName, true, Encoding.GetEncoding("GB2312"));            
            //sw.WriteLine(strText);
            //sw.Close();
        }

	}
}
