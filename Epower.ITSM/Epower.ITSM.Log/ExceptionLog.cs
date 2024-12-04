using System;
using System.Text;
using System.Web;

using System.Net;
using System.Collections.Specialized;

namespace Epower.ITSM.Log
{
	/// <summary>
	/// ¥ÌŒÛ»’÷æ¿‡
	/// </summary>
    public class ExceptionLog
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objErr"></param>
        /// <param name="memo"></param>
        /// <param name="level"></param>
		public static void PostException(Exception objErr, string memo, string level)
		{
            if (CommonDP.GetConfigValue(Constant.LogConfigNodeName, "ExceptionLog").Equals("1"))
			{
                EpowerExceptionLogInfo eel = new EpowerExceptionLogInfo();
				eel.Message = objErr.Message;
				eel.AppID = CommonDP.GetConfigValue(Constant.LogConfigNodeName, "AppID");
				eel.StackTrace = objErr.StackTrace;
				eel.TargetSite = objErr.TargetSite.ToString();
				eel.IPAddress = Utils.IPAddress;
				eel.PostURL = HttpContext.Current.Request.Url.ToString();
				eel.UserID = Constant.UserID.ToString();
				eel.UserName = Constant.UserName;
				eel.Level = level;
				eel.Remark = memo;
                eel.insertExceptionLog();
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objErr"></param>
        /// <param name="memo"></param>
        /// <param name="level"></param>
        public static void PostException(string ErrInfo)
        {
            if (CommonDP.GetConfigValue(Constant.LogConfigNodeName, "ExceptionLog").Equals("1"))
            {
                Utils.Log2File(Utils.IPAddress + " | " + HttpContext.Current.Request.Url.ToString() + " | " + Constant.UserID.ToString() + " | " + Constant.UserName + " | " + ErrInfo);
            }
        }

    }
}
