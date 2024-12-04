using System;
using System.Web;
using System.Text.RegularExpressions;

using System.IO;

namespace Epower.ITSM.Log
{
	/// <summary>
	/// Util 的摘要说明。
	/// </summary>
	public class Utils
	{
		/// <summary> 
		/// 取得客户端真实IP。如果有代理则取第一个非内网地址 
		/// </summary> 
		public static string IPAddress
		{
			get
			{
				string result = String.Empty;

				result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

				if (result != null && result != String.Empty)
				{
					//可能有代理 
					if (result.IndexOf(".") == -1)    //没有“.”肯定是非IPv4格式 
						result = null;
					else
					{
						if (result.IndexOf(",") != -1)
						{
							//有“,”，估计多个代理。取第一个不是内网的IP。 
							result = result.Replace(" ", "").Replace("'", "");
							string[] temparyip = result.Split(",;".ToCharArray());
							for (int i = 0; i < temparyip.Length; i++)
							{
								if (IsIPAddress(temparyip[i])
									&& temparyip[i].Substring(0, 3) != "10."
									&& temparyip[i].Substring(0, 7) != "192.168"
									&& temparyip[i].Substring(0, 7) != "172.16.")
								{
									return temparyip[i];    //找到不是内网的地址 
								}
							}
						}
						else if (IsIPAddress(result)) //代理即是IP格式 
							return result;
						else
							result = null;    //代理中的内容 非IP，取IP 
					}

				}

				result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

				if (result == null || result == String.Empty)
					result = HttpContext.Current.Request.UserHostAddress;

				return result;
			}
		}

		#region bool IsIPAddress(str1) 判断是否是IP格式
		/// <summary>
		/// 判断是否是IP地址格式 0.0.0.0
		/// </summary>
		/// <param name="str1">待判断的IP地址</param>
		/// <returns>true or false</returns>
		public static bool IsIPAddress(string str1)
		{
			if (str1 == null || str1 == string.Empty || str1.Length < 7 || str1.Length > 15) return false;

			string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";

			Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
			return regex.IsMatch(str1);
		}
		#endregion


		//记录日志到文件
		public static void Log2File(string strLog)
		{
            Epower.DevBase.BaseTools.E8Logger.Info(strLog);
            //string logPath = HttpRuntime.AppDomainAppPath + "Log";

            ////确定文件是否存在。
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
