using System;
using System.Text;
using System.Web;

using System.Net;
using System.Collections.Specialized;

namespace Epower.ITSM.Log
{
	/// <summary>
	/// 操作日志类
	/// </summary>
    public class OperateLog
    {
        public static void PostOperateLog(string begintime, string action, string memo, string SourceID, string SourceTable)
		{
            if (CommonDP.GetConfigValue(Constant.LogConfigNodeName, "OperatetLog").Equals("1"))
			{
                EpowerOperateLogInfo eol = new EpowerOperateLogInfo();
                eol.AppID = CommonDP.GetConfigValue(Constant.LogConfigNodeName, "AppID");
                eol.UserID = Epower.ITSM.Log.Constant.UserID.ToString();
                eol.UserName = Epower.ITSM.Log.Constant.UserName;
                eol.DeptID = Epower.ITSM.Log.Constant.DeptID.ToString();
                eol.Dept = Epower.ITSM.Log.Constant.DeptName;
				eol.IPAddress = Utils.IPAddress;
				eol.Page = HttpContext.Current.Request.Url.ToString();
				eol.Action = action;
				eol.BeginTime = begintime;
				eol.EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                eol.Time = begintime;
				eol.Remark = memo;
                eol.SourceID = SourceID;
                eol.SourceTable = SourceTable;
				try
				{
                    eol.insertOperateLog();
				}
				catch
				{ }
			}
		}

    }
}
