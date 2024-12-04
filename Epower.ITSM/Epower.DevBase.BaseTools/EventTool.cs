using System;
using System.Diagnostics;
using System.Security;
using System.IO;
namespace Epower.DevBase.BaseTools
{
	/// <summary>
	/// 将错误信息写入到错误日志中
    /// 写入到《E8.Net应用程序日志》
	/// </summary>
	public class EventTool
	{
        /// <summary>
        /// 
        /// </summary>
		public EventTool()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

		/// <summary>
		/// 将错误信息写入到错误日志中
		/// 写入到《E8.Net应用程序日志》
		/// </summary>
		/// <param name="Message">记录的相关信息</param>
		/// <param name="Source">记录的来源</param>
		/// <param name="type">表示记录类型，是枚举类型，有以下选项:Error 错误,Warning 警告,Information 信息, SuccessAudit 成功审核,
		///FailureAudit 失败审核</param>
		
		public static void EventLog(string Message,string Source,EventLogEntryType type)
		{
            E8Logger.Error(string.Format("Message:{0}{3}  Source:{1} {3} type:{2}", Message, Source, type.ToString(),Environment.CommandLine));
            //string strDirLog = @"c:\EpowerLog\";
            //if (!System.IO.Directory.Exists(strDirLog))
            //{
            //    System.IO.Directory.CreateDirectory(strDirLog);
            //}

            //string strLogFileName = strDirLog + "EpowerErrMsg" + System.DateTime.Now.ToString("yyyyMMdd") + ".log";
            //FileStream fs = new FileStream(strLogFileName, FileMode.Append, FileAccess.Write);
            //StreamWriter sw = new StreamWriter(fs);
            //sw.WriteLine(System.DateTime.Now.ToString() + ((int)type).ToString());
            //sw.WriteLine(Message);
            //sw.Close();
            //fs.Close();	
			
		}
		
		/// <summary>
		/// 将错误信息写入到错误日志中
        /// 写入到《E8.Net应用程序日志》
		/// </summary>
		/// <param name="Message">记录的相关信息</param>
		public static void EventLog(string Message)
		{
            E8Logger.Error(Message);
            //string strDirLog = @"c:\EpowerLog\";
            //if (!System.IO.Directory.Exists(strDirLog))
            //{
            //    System.IO.Directory.CreateDirectory(strDirLog);
            //}

            //string strLogFileName = strDirLog + "EpowerErrMsg" + System.DateTime.Now.ToString("yyyyMMdd") + ".log";
            //FileStream fs = new FileStream(strLogFileName, FileMode.Append, FileAccess.Write);
            //StreamWriter sw = new StreamWriter(fs);
            //sw.WriteLine(System.DateTime.Now.ToString());
            //sw.WriteLine(Message);
            //sw.Close();
            //fs.Close();	
		}
		/// <summary>
		/// 将catch捕捉到的错误信息写成磁盘文件
		/// </summary>
		/// <param name="errStr">系统信息描述(Exception)</param>
		public static void WriteEventLog(Exception errStr)
		{
            E8Logger.Error(errStr);
            //string strDirLog = @"c:\EpowerLog\";
            //if(!System.IO.Directory.Exists(strDirLog))
            //{
            //    System.IO.Directory.CreateDirectory(strDirLog);
            //}
		
            //string strLogFileName = strDirLog + "EpowerErrMsg" + System.DateTime.Now.ToString("yyyyMMdd")+".log";
            //FileStream fs = new FileStream(strLogFileName,FileMode.Append,FileAccess.Write);
            //StreamWriter sw = new StreamWriter(fs);
            //sw.WriteLine(System.DateTime.Now.ToString());
            //sw.WriteLine(errStr.ToString());
            //sw.Close();
            //fs.Close();
		}
	}
}
