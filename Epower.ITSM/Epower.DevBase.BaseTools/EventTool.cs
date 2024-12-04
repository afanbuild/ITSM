using System;
using System.Diagnostics;
using System.Security;
using System.IO;
namespace Epower.DevBase.BaseTools
{
	/// <summary>
	/// ��������Ϣд�뵽������־��
    /// д�뵽��E8.NetӦ�ó�����־��
	/// </summary>
	public class EventTool
	{
        /// <summary>
        /// 
        /// </summary>
		public EventTool()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}

		/// <summary>
		/// ��������Ϣд�뵽������־��
		/// д�뵽��E8.NetӦ�ó�����־��
		/// </summary>
		/// <param name="Message">��¼�������Ϣ</param>
		/// <param name="Source">��¼����Դ</param>
		/// <param name="type">��ʾ��¼���ͣ���ö�����ͣ�������ѡ��:Error ����,Warning ����,Information ��Ϣ, SuccessAudit �ɹ����,
		///FailureAudit ʧ�����</param>
		
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
		/// ��������Ϣд�뵽������־��
        /// д�뵽��E8.NetӦ�ó�����־��
		/// </summary>
		/// <param name="Message">��¼�������Ϣ</param>
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
		/// ��catch��׽���Ĵ�����Ϣд�ɴ����ļ�
		/// </summary>
		/// <param name="errStr">ϵͳ��Ϣ����(Exception)</param>
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
