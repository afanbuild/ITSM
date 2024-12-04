using System;
using System.Web;
using System.Text.RegularExpressions;

using System.IO;

namespace Epower.ITSM.Log
{
	/// <summary>
	/// Util ��ժҪ˵����
	/// </summary>
	public class Utils
	{
		/// <summary> 
		/// ȡ�ÿͻ�����ʵIP������д�����ȡ��һ����������ַ 
		/// </summary> 
		public static string IPAddress
		{
			get
			{
				string result = String.Empty;

				result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

				if (result != null && result != String.Empty)
				{
					//�����д��� 
					if (result.IndexOf(".") == -1)    //û�С�.���϶��Ƿ�IPv4��ʽ 
						result = null;
					else
					{
						if (result.IndexOf(",") != -1)
						{
							//�С�,�������ƶ������ȡ��һ������������IP�� 
							result = result.Replace(" ", "").Replace("'", "");
							string[] temparyip = result.Split(",;".ToCharArray());
							for (int i = 0; i < temparyip.Length; i++)
							{
								if (IsIPAddress(temparyip[i])
									&& temparyip[i].Substring(0, 3) != "10."
									&& temparyip[i].Substring(0, 7) != "192.168"
									&& temparyip[i].Substring(0, 7) != "172.16.")
								{
									return temparyip[i];    //�ҵ����������ĵ�ַ 
								}
							}
						}
						else if (IsIPAddress(result)) //������IP��ʽ 
							return result;
						else
							result = null;    //�����е����� ��IP��ȡIP 
					}

				}

				result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

				if (result == null || result == String.Empty)
					result = HttpContext.Current.Request.UserHostAddress;

				return result;
			}
		}

		#region bool IsIPAddress(str1) �ж��Ƿ���IP��ʽ
		/// <summary>
		/// �ж��Ƿ���IP��ַ��ʽ 0.0.0.0
		/// </summary>
		/// <param name="str1">���жϵ�IP��ַ</param>
		/// <returns>true or false</returns>
		public static bool IsIPAddress(string str1)
		{
			if (str1 == null || str1 == string.Empty || str1.Length < 7 || str1.Length > 15) return false;

			string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";

			Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
			return regex.IsMatch(str1);
		}
		#endregion


		//��¼��־���ļ�
		public static void Log2File(string strLog)
		{
            Epower.DevBase.BaseTools.E8Logger.Info(strLog);
            //string logPath = HttpRuntime.AppDomainAppPath + "Log";

            ////ȷ���ļ��Ƿ���ڡ�
            //if (!Directory.Exists(logPath))
            //{
            //    DirectoryInfo d = Directory.CreateDirectory(logPath);
            //}

            //string LogFile = logPath + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".log";

            //if (!File.Exists(LogFile))
            //{
            //    StreamWriter rw = File.CreateText(LogFile);
            //    rw.Close();
            //}

            //StreamWriter sw = new StreamWriter(LogFile, true, System.Text.Encoding.Default);
            //sw.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "	" + strLog + "\r\n");

            //sw.Flush();
            //sw.Close();
		}
	}
}
