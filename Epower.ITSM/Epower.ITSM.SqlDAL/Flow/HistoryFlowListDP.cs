using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.SqlDAL
{
	/// <summary>
	/// HistoryListDP 的摘要说明。
	/// </summary>
	public class HistoryFlowListDP
	{
		public HistoryFlowListDP()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

		/// <summary>
		/// 根据userid,flowmodelid,flowid,catalgvalue查找同类型的已经结束的流程。
		/// </summary>
		/// <param name="userID"></param>
		/// <param name="flowModelID"></param>
		/// <param name="flowID"></param>
		/// <param name="catalogName"></param>
		/// <param name="catalogValue"></param>
		/// <param name="tableName">表名：App_pub_SMS_Head　或　ris_issues 或其它．　有分类数的应用主表　</param>
		/// <returns></returns>
		public static DataTable GetHistoryFlowList(string userID, string flowModelID, string flowID, string catalogName, string catalogValue,string tableName)
		{
			// AND (ReceiverID <> " + userID +@")
			string sSql = @"SELECT  Es_Flow.Subject,Es_Flow.AppID, Es_Flow.Status,
                          (SELECT     MAX(MessageID) AS MessageID
                            FROM          Es_Message
                            WHERE      (FlowID = Es_Flow.FlowID) AND (Status = 10)) AS MessageID
						FROM     " + tableName + @" INNER JOIN
											Es_Flow ON " + tableName + @".FlowID = Es_Flow.FlowID
						WHERE ROWNUM<=20 AND     (" + tableName + @".FlowModelID = " + flowModelID + @") AND (" + tableName + @".FlowID <> " + flowID + @") AND (" + tableName + @"." + catalogName + @" = " + catalogValue + @")";
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sSql);
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
		}
	}
}
