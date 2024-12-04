using System;
using System.Data;
using MyComponent;
using System.Xml;
using System.IO;
using System.Collections;
using EpowerGlobal;
using Epower.DevBase.BaseTools;
using System.Data.OracleClient;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// ReceiveList 的摘要说明。
    /// </summary>
    public class FlowReceiveList
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FlowReceiveList()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 获取用户未接收列表
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <returns></returns>
        public static DataTable GetReceiveMessageList(long lngUserID)
        {
            string strSQL = "";
            strSQL = @"SELECT nvl(f.buildCode||f.ServiceNo,'') ServiceNo,f.*,d.ID,a.MessageID,a.flowid,b.Flowmodelid,e.nodemodelid,b.appid,
	                            a.IsRead,a.Important,a.FActors,a.ReceiveTime,a.actortype,b.Subject,b.Attachment,b.Name as FlowName,c.AppID,c.AppName as AppName,
                                datediff('Minute',sysdate,nvl(a.expected,sysdate)) as flowdiffminute,b.FlowID,
	                            b.status,b.endtime,f.CustName,f.ServiceKind,a.senderusername,a.sendernodename  
                        FROM V_ES_MESSAGE_NotReceiver a,Es_Flow b,Es_App c,Es_ReceiveList d,Es_Node e,Cst_Issues f
                        WHERE d.MessageID = a.MessageID AND a.FlowID = b.FlowID AND b.AppID = c.AppID And a.nodeid=e.nodeid And b.Flowid=f.flowid " +
                " AND d.ReceiveID = " + lngUserID.ToString() +
                " AND a.Deleted =" + (int)e_Deleted.eNormal +
                " AND a.ReceiverID = 0 " +
                " ORDER BY a.MessageID DESC";
            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        /// <summary>
        /// 获取用户未接收列表
        /// </summary>
        /// <param name="lngUserID">用户Id</param>
        /// <param name="strWhere">条件</param>
        /// <returns></returns>
        public static DataTable GetReceiveMessageList(long lngUserID, string strXmlcond)
        {
            string strvalue = string.Empty,
                strStatus = string.Empty,
                strSubject = string.Empty,
                strAppId = string.Empty,
                strMessageBegin = string.Empty,
                strMessageEnd = string.Empty,
                strProcessBegin = string.Empty,
                strProcessEnd = string.Empty,
                strWhere = string.Empty;
            long lngUserDeptID = 0L;
            XmlTextReader reader = new XmlTextReader(new StringReader(strXmlcond));
            while (reader.Read())
            {
                if ((reader.Name == "Field") && (reader.NodeType == XmlNodeType.Element))
                {
                    strvalue = reader.GetAttribute("Value").Trim();
                    switch (reader.GetAttribute("FieldName"))
                    {
                        case "Status":
                            strStatus = strvalue;
                            break;

                        case "Subject":
                            strSubject = strvalue;
                            break;

                        case "AppID":
                            strAppId = strvalue;
                            break;

                        case "MessageBegin":
                            strMessageBegin = strvalue;
                            break;

                        case "MessageEnd":
                            strMessageEnd = strvalue;
                            break;

                        case "ProcessBegin":
                            strProcessBegin = strvalue;
                            break;

                        case "ProcessEnd":
                            strProcessEnd = strvalue;
                            break;
                        case "UserDeptID":
                            lngUserDeptID = long.Parse(strvalue);
                            break;
                    }
                }
            }
            reader.Close();

            if ((strStatus != "-1") && (strStatus != "-2") && (strStatus != ""))
            {
                strWhere = strWhere + " AND b.status = " + strStatus;
            }

            if (strSubject.Length != 0)
            {
                strWhere = strWhere + " AND b.Subject like " + StringTool.SqlQ("%" + strSubject + "%");
            }

            if ((strAppId != "-1") && (strAppId != "-2") && (strAppId != ""))
            {
                strWhere = strWhere + " AND b.AppId =" + strAppId;
            }

            if (strMessageBegin != "")
            {
                strWhere = strWhere + " AND a.ReceiveTime >= " + StringTool.SqlQ(strMessageBegin);
            }

            if (strMessageEnd != "")
            {
                strWhere = strWhere + " AND a.ReceiveTime <= " + StringTool.SqlQ(strMessageEnd);
            }
            if (strProcessBegin != "")
            {
                strWhere = strWhere + " AND a.recentprocesstime >= " + StringTool.SqlQ(strProcessBegin);
            }
            if (strProcessEnd != "")
            {
                strWhere = strWhere + " AND a.recentprocesstime <= " + StringTool.SqlQ(strProcessEnd);
            }
            string strSQL = "";
            strSQL = "SELECT d.ID,a.MessageID,a.IsRead,a.Important,a.FActors,a.ReceiveTime,a.actortype,b.Subject,b.Attachment,b.Name as FlowName," +
                    "c.AppID,c.AppName as AppName,b.FlowID,datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,b.status " +
                   "FROM Es_Message a,Es_Flow b,Es_App c,Es_ReceiveList d  WHERE d.MessageID = a.MessageID AND a.FlowID = b.FlowID AND b.AppID = c.AppID  AND d.ReceiveID = " + lngUserID + " " +
                   "AND a.Deleted =0 AND a.ReceiverID =0 " + strWhere + " ORDER BY a.MessageID DESC ";
            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        /// <summary>
        /// 获取用户未接收列表
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static DataTable GetReceiveMessageList(long lngUserID, int pageSize)
        {
            string strSQL = "";
            strSQL = "SELECT d.ID,a.MessageID,a.flowid,b.Flowmodelid,e.nodemodelid,b.appid,a.IsRead,a.Important,a.FActors,a.ReceiveTime,a.actortype,b.Subject,b.Attachment,b.Name as FlowName,c.AppID,c.AppName as AppName,datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,b.FlowID " +
                " FROM Es_Message a,Es_Flow b,Es_App c,Es_ReceiveList d ,Es_Node e " +
                " WHERE ROWNUM<=" + pageSize.ToString() + " AND d.MessageID = a.MessageID AND a.FlowID = b.FlowID AND b.AppID = c.AppID And a.nodeid=e.nodeid  " +
                " AND d.ReceiveID = " + lngUserID.ToString() +
                " AND a.Deleted =" + (int)e_Deleted.eNormal +
                " AND a.ReceiverID = 0 " +
                " ORDER BY a.MessageID DESC";
            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;

        }


        /// <summary>
        /// 获取用户未接收数量
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static int GetReceiveMessageListCount(long lngUserID)
        {
            string strSQL = "";
            strSQL = "SELECT count(*) " +
                " FROM Es_Message a,Es_Flow b,Es_App c,Es_ReceiveList d " +
                " WHERE d.MessageID = a.MessageID AND a.FlowID = b.FlowID AND b.AppID = c.AppID " +
                " AND d.ReceiveID = " + lngUserID.ToString() +
                " AND a.Deleted =" + (int)e_Deleted.eNormal +
                " AND a.ReceiverID = 0 ";
            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");

            try
            {
                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                int iRet = 0;
                while (dr.Read())
                {
                    iRet = dr.GetInt32(0);
                    break;
                }
                dr.Close();

                return iRet;
            }
            finally { ConfigTool.CloseConnection(cn); }
            
        }
    }
}
