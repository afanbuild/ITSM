/*******************************************************************
 *
 * Description:手机客户端数据
 * 
 * 
 * Create By  :谭雨
 * Create Date:2012年8月7日
 * *****************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Epower.ITSM.Base;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using EpowerGlobal;

namespace Epower.ITSM.SqlDAL.Mobile
{
    /// <summary>
    /// 手机登录平台DAL  的
    /// </summary>
    public class MobileDP
    {
        /// <summary>
        /// 
        /// </summary>
        public MobileDP()
        {

        }

        /// <summary>
        /// 用户登陆时，MobileloginHistory添加一条时间数据


        /// </summary>
        /// <param name="userId">登陆用户ID</param>
        public static void AddMobileloginHistory(long userId)
        {
            string strSQL = "";
            OracleConnection cn = ConfigTool.GetConnection();
            
            try
            {
                string strID = EpowerGlobal.EPGlobal.GetNextID("TS_MOBILELOGINHISTORYID").ToString();

                StringBuilder sbStrSQL = new StringBuilder();
                sbStrSQL.Append("INSERT INTO TS_MOBILELOGINHISTORY (ID,USERID,Cretime) VALUES(" + strID + ",");
                sbStrSQL.Append(userId);
                sbStrSQL.Append(",sysdate)");


                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, sbStrSQL.ToString());
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

        /// <summary>
        /// 用户登陆验证后，修改TS_MOBILELOGINHISTORY登录类型
        /// </summary>
        /// <param name="ID">TS_MOBILELOGINHISTORY_ID</param>
        /// <param name="userId">登陆用户ID</param>
        /// <param name="logintype">登陆类型(1登录成功\0 登录失败\2 访问被拒绝)</param>
        /// <param name="creby">密码</param>
        public static void UpdateMobileloginHistory(long id, long userId, string logintype, string creby)
        {
            StringBuilder sbStrSQL = new StringBuilder();
            sbStrSQL.Append("UPDATE TS_MOBILELOGINHISTORY SET logintype=");
            sbStrSQL.Append(StringTool.SqlQ(logintype.ToString()));
            sbStrSQL.Append(",creby=");
            sbStrSQL.Append(StringTool.SqlQ(creby.ToString()));
            sbStrSQL.Append(" where userid=");
            sbStrSQL.Append(userId);
            sbStrSQL.Append(" and id=");
            sbStrSQL.Append(id);

            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, sbStrSQL.ToString());
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

        /// <summary>
        /// 用户登陆时，Ts_Token添加一条数据


        /// </summary>
        /// <param name="userId">登陆用户ID</param>
        /// <param name="logintype">登陆类型(手机登录平台固定为M)</param>
        /// <param name="isValid">有效标志(1有效/0失效)</param>
        /// <param name="creby">建立者(手机登录平台固定为Mobile)</param>
        public static void AddToken(string guid, long userId, string logintype, string isValid, string creby)
        {
            StringBuilder sbStrSQL = new StringBuilder();
            sbStrSQL.Append("INSERT INTO Ts_Token (TOKENID,USERID,Logintype,Isvalid,Creby,Cretime) VALUES(");
            sbStrSQL.Append(StringTool.SqlQ(guid.ToString()));
            sbStrSQL.Append(",");
            sbStrSQL.Append(userId);
            sbStrSQL.Append(",");
            sbStrSQL.Append(StringTool.SqlQ(logintype.ToString()));
            sbStrSQL.Append(",");
            sbStrSQL.Append(isValid);
            sbStrSQL.Append(",");
            sbStrSQL.Append(StringTool.SqlQ(creby.ToString()));
            sbStrSQL.Append(",sysdate)");

            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, sbStrSQL.ToString());
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

        /// <summary>
        /// 用户登陆验证后，修改Ts_Token有效标志
        /// </summary>
        /// <param name="tokenId">Ts_Token_ID</param>
        /// <param name="userId">登陆用户ID</param>
        /// <param name="logintype">登陆用户ID</param>
        /// <param name="creby">登陆用户ID</param>
        public static void UpdateToken(string tokenId, long userId, string isvalId)
        {
            StringBuilder sbStrSQL = new StringBuilder();
            sbStrSQL.Append("UPDATE Ts_Token SET isvalid=");
            sbStrSQL.Append(StringTool.SqlQ(isvalId.ToString()));
            sbStrSQL.Append(" where userid=");
            sbStrSQL.Append(userId);
            sbStrSQL.Append(" and Tokenid=");
            sbStrSQL.Append(tokenId);

            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, sbStrSQL.ToString());
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

        /// <summary>
        /// 用户登陆时，验证是否时间段内超过5次登陆


        /// </summary>
        /// <param name="userId">登陆用户ID</param>
        /// <param name="beginDate">查询开始</param>
        /// <param name="endDate">查询结束</param>
        /// <returns>bool 返回false表示时间段内超过5次登陆</returns>
        public static bool IsUserLoginFrequency(long userId, string beginDate, string endDate)
        {
            bool frequency = true;

            StringBuilder sbStrSQL = new StringBuilder();
            sbStrSQL.Append("SELECT count(A.id) from TS_MOBILELOGINHISTORY A where A.Cretime < to_date(");
            sbStrSQL.Append(StringTool.SqlQ(beginDate.ToString()));
            sbStrSQL.Append(",'yyyy-mm-dd HH24:mi:ss') and A.Cretime > to_date(");
            sbStrSQL.Append(StringTool.SqlQ(endDate.ToString()));
            sbStrSQL.Append(", 'yyyy-mm-dd hh24:mi:ss') and A.LoginType=0 and A.userId=");
            sbStrSQL.Append(userId);

            string scn = ConfigTool.GetConnectString();
            using (OracleDataReader dr = OracleDbHelper.ExecuteReader(scn, CommandType.Text, sbStrSQL.ToString()))
            {
                if (dr.Read())
                {
                    int obj = dr.GetInt32(0);

                    if (obj >= 5)
                    {
                        frequency = false;
                    }
                }
            }

            return frequency;
        }

        /// <summary>
        /// 获取用户登陆TS_MOBILELOGINHISTORY表时间最近的ID
        /// </summary>
        /// <param name="userId">登陆用户ID</param>
        /// <returns>bool </returns>
        public static long GetMobileloginHistoryMaxID(long userId)
        {
            long mId = 0L;
            StringBuilder sbStrSQL = new StringBuilder();
            sbStrSQL.Append("SELECT max(A.id) as id from TS_MOBILELOGINHISTORY A where  A.USERID=");
            sbStrSQL.Append(userId);

            string scn = ConfigTool.GetConnectString();
            using (OracleDataReader dr = OracleDbHelper.ExecuteReader(scn, CommandType.Text, sbStrSQL.ToString()))
            {
                if (dr.Read())
                {
                    mId = long.Parse(dr["id"].ToString());
                }
            }

            return mId;
        }


        /// <summary>
        /// 获取用户登陆Ts_Token表时间最近的ID
        /// </summary>
        /// <param name="userId">登陆用户ID</param>
        /// <returns>bool </returns>
        public static string GetTokenMaxID(long userId)
        {
            string mId = string.Empty;
            StringBuilder sbStrSQL = new StringBuilder();
            sbStrSQL.Append("SELECT A.Tokenid as tokenid FROM Ts_Token A WHERE  A.USERID=");
            sbStrSQL.Append(userId);
            sbStrSQL.Append(" ORDER BY A.Cretime desc");

            string scn = ConfigTool.GetConnectString();
            using (OracleDataReader dr = OracleDbHelper.ExecuteReader(scn, CommandType.Text, sbStrSQL.ToString()))
            {
                if (dr.Read())
                {
                    mId = dr["Tokenid"].ToString();
                }
            }
            return mId;
        }


        /// <summary>
        /// 验证用户登录URL是否有效
        /// </summary>
        /// <param name="userId">登陆用户ID</param>
        /// <param name="tokenId">tokenID</param>
        /// <returns>bool </returns>
        public static bool IsValidUserLogin(long userId, string tokenId)
        {
            bool frequency = false;
            StringBuilder sbStrSQL = new StringBuilder();
            sbStrSQL.Append("SELECT count(A.Tokenid) from Ts_Token A where A.Tokenid=");
            sbStrSQL.Append(StringTool.SqlQ(tokenId.ToString()));
            sbStrSQL.Append(" and A.USERID=");
            sbStrSQL.Append(userId);
            sbStrSQL.Append("and A.isvalId=1");

            string scn = ConfigTool.GetConnectString();
            using (OracleDataReader dr = OracleDbHelper.ExecuteReader(scn, CommandType.Text, sbStrSQL.ToString()))
            {
                if (dr.Read())
                {
                    int obj = dr.GetInt32(0);

                    if (obj > 0)
                    {
                        frequency = true;
                    }
                }
            }

            return frequency;
        }

        /// <summary>
        /// 获取用户未处理的事件
        /// </summary>
        /// <param name="userId">登陆用户ID</param>
        /// <returns>DataTable</returns>
        public static DataTable GetUndoMessageList(long userId, eOA_TracePeriod tp)
        {
            DataTable dt = null;
            StringBuilder sbStrSQL = new StringBuilder();
            sbStrSQL.Append("SELECT b.flowmodelid,a.MessageID,a.FActors,a.ReceiveTime,b.Subject,b.Name as FlowName,c.AppID,c.AppName as AppName,b.FlowID ");
            sbStrSQL.Append("FROM Es_Message a,Es_Flow b,Es_App c ");
            sbStrSQL.Append("WHERE a.FlowID = b.FlowID AND b.AppID = c.AppID  AND a.ReceiverID=");
            sbStrSQL.Append(userId);
            sbStrSQL.Append(" AND a.Deleted =0 AND ( a.status = 20 OR a.prepausestatus = 20 ) AND a.actortype <> 1 ");
            //sbStrSQL.Append(" AND a.Deleted =0 AND a.actortype <> 1 ");

            GetStrSQL(ref sbStrSQL, tp);
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sbStrSQL.ToString());
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
            return dt;
        }

        /// <summary>
        /// 根据上次获取时间, 查询用户未处理的事件
        /// </summary>
        /// <param name="lngUserId">用户编号</param>
        /// <param name="lastUpdateTime">上次更新时间</param>
        /// <param name="tp">数据量</param>
        /// <returns>查询结果集</returns>
        public static DataTable GetUndoMessageList(long lngUserId,              
            DateTime lastUpdateTime,
            eOA_TracePeriod tp)
        {
            DataTable dt = null;
            StringBuilder sbStrSQL = new StringBuilder();
            sbStrSQL.Append("SELECT b.flowmodelid,a.MessageID,a.FActors,a.ReceiveTime,b.Subject,b.Name as FlowName,c.AppID,c.AppName as AppName,b.FlowID ");
            sbStrSQL.Append("FROM Es_Message a,Es_Flow b,Es_App c ");
            sbStrSQL.Append("WHERE a.FlowID = b.FlowID AND b.AppID = c.AppID  AND a.ReceiverID=");
            sbStrSQL.Append(lngUserId);
            sbStrSQL.Append(" AND a.Deleted =0 AND ( a.status = 20 OR a.prepausestatus = 20 ) AND a.actortype <> 1 ");
            //sbStrSQL.Append(" AND a.Deleted =0 AND a.actortype <> 1 ");

            // begin: 设定上次查询时间
            sbStrSQL.AppendFormat("AND RECEIVETIME >  to_date('{0}','YYYY-MM-DD HH24:MI:SS')",
                lastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            // end.

            GetStrSQL(ref sbStrSQL, tp);
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sbStrSQL.ToString());
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
            return dt;

            // draft: sunshaozong@gmail.com
        } 


        private static void GetStrSQL(ref StringBuilder sbStrSQL, eOA_TracePeriod tp)
        {
            switch (tp)
            {
                case eOA_TracePeriod.eMonth:
                    sbStrSQL.Append(" AND b.starttime >= to_date(to_char(DateAdd('month',-1,sysdate),'yyyy-MM-dd'),'yyyy-MM-dd') ");
                    break;
                case eOA_TracePeriod.eSeason:
                    sbStrSQL.Append(" AND b.starttime >= to_date(dateadd('month',-3,sysdate)) ");
                    break;
                case eOA_TracePeriod.eHalfYear:
                    sbStrSQL.Append(" AND b.starttime >= to_date(dateadd('month',-6,sysdate)) ");
                    break;
                case eOA_TracePeriod.eYear:
                    sbStrSQL.Append(" AND b.starttime >= to_date(dateadd('year',-1,sysdate)) ");
                    break;
                default:
                    break;
            }
            sbStrSQL.Append(" AND (b.AppID=1026  or b.AppID=420)  ORDER BY a.MessageID DESC ");
        }

        /// <summary>
        /// 获取用户未处理的事件总数
        /// </summary>
        /// <param name="userId">登陆用户ID</param>
        /// <returns>bool </returns>
        public static int GetUndoMessageCount(long userId, eOA_TracePeriod tp)
        {
            int count = 0;

            StringBuilder sbStrSQL = new StringBuilder();
            sbStrSQL.Append("SELECT count(*)  FROM Es_Message a,Es_Flow b,Es_App c ");
            sbStrSQL.Append("WHERE a.FlowID = b.FlowID AND b.AppID = c.AppID  AND a.ReceiverID=");
            sbStrSQL.Append(userId);
            sbStrSQL.Append(" AND a.Deleted = 0 AND ( a.status = 20 OR a.prepausestatus = 20 ) AND a.actortype <>  1 ");

            GetStrSQL(ref sbStrSQL, tp);
            string scn = ConfigTool.GetConnectString();
            using (OracleDataReader dr = OracleDbHelper.ExecuteReader(scn, CommandType.Text, sbStrSQL.ToString()))
            {
                if (dr.Read())
                {
                    count = dr.GetInt32(0);
                }
            }
            return count;
        }


        /// <summary>
        /// 获取用户未完成的流程实例列表
        /// </summary>
        /// <param name="userId">登陆用户ID</param>
        /// <returns>DataTable</returns>
        public static DataTable GetMyRegMessageUnFinished(long userId, eOA_TracePeriod tp)
        {
            DataTable dt = null;

            StringBuilder sbStrSQL = new StringBuilder();
            sbStrSQL.Append("SELECT b.flowid,b.flowmodelid,a.MessageID, b.subject,a.ReceiveTime,b.Name as FlowName, b.starttime FROM es_message a, es_flow b ");
            sbStrSQL.Append("WHERE a.senderid = 0 AND a.ReceiverID = ");
            sbStrSQL.Append(userId);
            sbStrSQL.Append(" AND b.status <> 50 AND b.status <> 30 and a.flowid = b.flowid AND a.Deleted = 0 ");

            GetStrSQL(ref sbStrSQL, tp);
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sbStrSQL.ToString());
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
            return dt;
        }

        /// <summary>
        /// 获取用户未完成的流程实例列表总数
        /// </summary>
        /// <param name="userId">登陆用户ID</param>
        /// <returns>int </returns>
        public static int GetMyRegMessageUnFinishedCount(long userId, eOA_TracePeriod tp)
        {
            int count = 0;

            StringBuilder sbStrSQL = new StringBuilder();
            sbStrSQL.Append("SELECT count(*)   FROM es_message a,es_flow b  WHERE a.senderid = 0  AND a.ReceiverID=");
            sbStrSQL.Append(userId);
            sbStrSQL.Append(" AND b.status <> 50 AND b.status <>  30 AND a.flowid =b.flowid  AND a.Deleted = 0 ");

            GetStrSQL(ref sbStrSQL, tp);
            string scn = ConfigTool.GetConnectString();
            using (OracleDataReader dr = OracleDbHelper.ExecuteReader(scn, CommandType.Text, sbStrSQL.ToString()))
            {
                if (dr.Read())
                {
                    count = dr.GetInt32(0);
                }
            }
            return count;
        }

        /// <summary>
        /// 获取用户未接收列表
        /// </summary>
        /// <param name="userId">登陆用户ID</param>
        /// <returns>DataTable</returns>
        public static DataTable GetReceiveMessageList(long lngUserId, 
            eOA_TracePeriod tp)
        {
            string strSQL = "";
            DataTable dt = null;

            StringBuilder sbStrSQL = new StringBuilder();
            sbStrSQL.Append("SELECT d.ID,b.flowmodelid,a.MessageID,a.FActors,a.ReceiveTime,b.Subject,b.Name as FlowName,c.AppID,c.AppName as AppName,b.FlowID  ");
            sbStrSQL.Append("FROM Es_Message a,Es_Flow b,Es_App c,Es_ReceiveList d  ");
            sbStrSQL.Append("WHERE d.MessageID = a.MessageID AND a.FlowID = b.FlowID AND b.AppID = c.AppID  AND d.ReceiveID =");
            sbStrSQL.Append(lngUserId);
            sbStrSQL.Append(" AND a.Deleted = 0 AND a.ReceiverID = 0 ");

            GetStrSQL(ref sbStrSQL, tp);
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sbStrSQL.ToString());
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
            return dt;
        }

        /// <summary>
        /// 根据上次获取时间, 查询用户未接收列表
        /// </summary>
        /// <param name="lngUserId">用户编号</param>
        /// <param name="lastUpdateTime">上次更新时间</param>
        /// <param name="tp">数据量</param>
        /// <returns>查询结果集</returns>
        public static DataTable GetReceiveMessageList(long lngUserId,
            DateTime lastUpdateTime,
            eOA_TracePeriod tp)
        {
            string strSQL = "";
            DataTable dt = null;

            StringBuilder sbStrSQL = new StringBuilder();
            sbStrSQL.Append("SELECT d.ID,b.flowmodelid,a.MessageID,a.FActors,a.ReceiveTime,b.Subject,b.Name as FlowName,c.AppID,c.AppName as AppName,b.FlowID  ");
            sbStrSQL.Append("FROM Es_Message a,Es_Flow b,Es_App c,Es_ReceiveList d  ");
            sbStrSQL.Append("WHERE d.MessageID = a.MessageID AND a.FlowID = b.FlowID AND b.AppID = c.AppID  AND d.ReceiveID =");
            sbStrSQL.Append(lngUserId);
            sbStrSQL.Append(" AND a.Deleted = 0 AND a.ReceiverID = 0 ");

            // begin: 设定上次查询时间
            sbStrSQL.AppendFormat("AND RECEIVETIME >  to_date('{0}','YYYY-MM-DD HH24:MI:SS')",
                lastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            // end.

            GetStrSQL(ref sbStrSQL, tp);
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sbStrSQL.ToString());
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
            return dt;
        }


        /// <summary>
        /// 获取用户未接收列表总数
        /// </summary>
        /// <param name="lngUserID">登陆用户ID</param>
        /// <returns>int </returns>
        public static int GetReceiveMessageListCount(long lngUserID, eOA_TracePeriod tp)
        {
            int count = 0;
            StringBuilder sbStrSQL = new StringBuilder();
            sbStrSQL.Append("SELECT count(*)  FROM Es_Message a,Es_Flow b,Es_App c,Es_ReceiveList d  ");
            sbStrSQL.Append("WHERE d.MessageID = a.MessageID AND a.FlowID = b.FlowID AND b.AppID = c.AppID  AND d.ReceiveID =");
            sbStrSQL.Append(lngUserID);
            sbStrSQL.Append(" AND a.Deleted =0 AND a.ReceiverID = 0 ");

            GetStrSQL(ref sbStrSQL, tp);
            string scn = ConfigTool.GetConnectString();
            using (OracleDataReader dr = OracleDbHelper.ExecuteReader(scn, CommandType.Text, sbStrSQL.ToString()))
            {
                if (dr.Read())
                {
                    count = dr.GetInt32(0);
                }
            }
            return count;
        }

        /// <summary>
        /// 获取未通过的待阅知事项
        /// </summary>
        /// <param name="lngUserID">登陆用户ID</param>
        /// <returns>DataTable</returns>
        public static DataTable GetUnReadMessage(long lngUserID, eOA_TracePeriod tp)
        {
            string strSQL = "";
            DataTable dt = null;

            StringBuilder sbStrSQL = new StringBuilder();
            sbStrSQL.Append("SELECT b.flowmodelid,a.MessageID,a.IsRead,a.Important,a.FActors,a.ReceiveTime,a.actortype,b.Subject,b.Attachment,");
            sbStrSQL.Append("b.Name as FlowName,c.AppID,c.AppName as AppName,datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,b.FlowID ");
            sbStrSQL.Append("FROM Es_Message a,Es_Flow b,Es_App c  WHERE a.FlowID = b.FlowID AND b.AppID = c.AppID  AND a.ReceiverID=");
            sbStrSQL.Append(lngUserID);
            sbStrSQL.Append(" AND a.Deleted = 0 AND ( a.status =  20 OR a.prepausestatus =  20 ) AND a.actortype =  1 ");

            GetStrSQL(ref sbStrSQL, tp);
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sbStrSQL.ToString());
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
            return dt;
        }

        /// <summary>
        /// 获取未通过的待阅知事项总数
        /// </summary>
        /// <param name="lngUserID">登陆用户ID</param>
        /// <returns>int </returns>
        public static int GetUnReadMessageCount(long lngUserID, eOA_TracePeriod tp)
        {
            int count = 0;

            StringBuilder sbStrSQL = new StringBuilder();
            sbStrSQL.Append("SELECT count(*)  FROM Es_Message a,Es_Flow b,Es_App c  WHERE a.FlowID = b.FlowID AND b.AppID = c.AppID  AND a.ReceiverID=");
            sbStrSQL.Append(lngUserID);
            sbStrSQL.Append(" AND a.Deleted = 0 AND ( a.status = 20 OR a.prepausestatus =  20) AND a.actortype = 1 ");

            GetStrSQL(ref sbStrSQL, tp);
            string scn = ConfigTool.GetConnectString();
            using (OracleDataReader dr = OracleDbHelper.ExecuteReader(scn, CommandType.Text, sbStrSQL.ToString()))
            {
                if (dr.Read())
                {
                    count = dr.GetInt32(0);
                }
            }
            return count;
        }


        /// <summary>
        /// 获取用户参与过的事项
        /// </summary>
        /// <param name="lngUserID">登陆用户ID</param>
        /// <returns>DataTable</returns>
        public static DataTable GetMyParticipateMatters(long lngUserID, eOA_TracePeriod tp)
        {
            string strSQL = "";
            DataTable dt = null;

            StringBuilder sbStrSQL = new StringBuilder();
            sbStrSQL.Append("select b.flowmodelid, a.MessageID, a.ReceiveTime, b.Subject, b.name as FlowName,c.AppName  as AppName,");
            sbStrSQL.Append("a.FlowID from Es_Message a,(select h.flowid, max(h.messageid) messageid from Es_Message h WHERE h.Deleted = 0 AND h.receiverid = ");
            sbStrSQL.Append(lngUserID);
            sbStrSQL.Append(" group by h.flowid) f,Es_Flow b,Es_App c where a.messageid = f.messageid AND a.FlowID = b.FlowID AND b.AppID = c.AppID ");

            GetStrSQL(ref sbStrSQL, tp);
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sbStrSQL.ToString());
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
            return dt;
        }


        /// <summary>
        /// 获取用户参与过的事项总数
        /// </summary>
        /// <param name="lngUserID">登陆用户ID</param>
        /// <returns>int </returns>
        public static int GetMyParticipateMattersCount(long lngUserID, eOA_TracePeriod tp)
        {
            int count = 0;

            StringBuilder sbStrSQL = new StringBuilder();
            sbStrSQL.Append("select count(a.messageid) from Es_Message a,(select h.flowid, max(h.messageid) messageid from Es_Message h ");
            sbStrSQL.Append("WHERE h.Deleted = 0 AND h.receiverid = ");
            sbStrSQL.Append(lngUserID);
            sbStrSQL.Append(" group by h.flowid) f, Es_Flow b, Es_App c where a.messageid = f.messageid AND a.FlowID = b.FlowID AND b.appid = c.appid ");

            GetStrSQL(ref sbStrSQL, tp);
            string scn = ConfigTool.GetConnectString();
            using (OracleDataReader dr = OracleDbHelper.ExecuteReader(scn, CommandType.Text, sbStrSQL.ToString()))
            {
                if (dr.Read())
                {
                    count = dr.GetInt32(0);
                }
            }
            return count;
        }

        /// <summary>
        /// 流程模型--环节
        /// </summary>
        /// <param name="flowModelID"></param>
        /// <param name="nodeModela"></param>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public static long GetLinkNodeID(long flowModelID, long nodeModela, long actionId)
        {
            long nodeModelB = 0;

            StringBuilder sbStrSQL = new StringBuilder();
            sbStrSQL.Append("SELECT a.nodemodelb  FROM Es_N_M_Link  a WHERE a.flowModelID =");
            sbStrSQL.Append(flowModelID);
            sbStrSQL.Append(" AND a.nodemodela =");
            sbStrSQL.Append(nodeModela);
            sbStrSQL.Append(" AND a.actionid =");
            sbStrSQL.Append(actionId);

            string scn = ConfigTool.GetConnectString();
            using (OracleDataReader dr = OracleDbHelper.ExecuteReader(scn, CommandType.Text, sbStrSQL.ToString()))
            {
                if (dr.Read())
                {
                    nodeModelB = long.Parse(dr.GetDecimal(0).ToString());
                }
            }
            return nodeModelB;
        }

        /// <summary>
        /// 环节模型类别
        /// </summary>
        /// <param name="flowId"></param>
        /// <returns></returns>
        public static long GetLngNodeModelid(long flowId)
        {
            long nodeTypeID = 0;

            StringBuilder sbStrSQL = new StringBuilder();
            sbStrSQL.Append("select a.nodemodelid from Es_Node a where a.flowid=");
            sbStrSQL.Append(flowId);
            sbStrSQL.Append(" order by a.nodeid desc");

            string scn = ConfigTool.GetConnectString();
            using (OracleDataReader dr = OracleDbHelper.ExecuteReader(scn, CommandType.Text, sbStrSQL.ToString()))
            {
                if (dr.Read())
                {
                    nodeTypeID = long.Parse(dr.GetDecimal(0).ToString());
                }

            }

            return nodeTypeID;
        }

        /// <summary>
        /// 环节类别
        /// </summary>
        /// <param name="flowModelID"></param>
        /// <param name="nodeModela"></param>
        /// <returns></returns>
        public static long GetLinkNodeModeType(long flowModelID, long nodeModela)
        {
            long typeID = 0;

            StringBuilder sbStrSQL = new StringBuilder();
            sbStrSQL.Append("select a.type from Es_NodeModel a WHERE flowModelID =");
            sbStrSQL.Append(flowModelID);
            sbStrSQL.Append(" AND NodeModelID =");
            sbStrSQL.Append(nodeModela);

            string scn = ConfigTool.GetConnectString();
            using (OracleDataReader dr = OracleDbHelper.ExecuteReader(scn, CommandType.Text, sbStrSQL.ToString()))
            {
                if (dr.Read())
                {
                    typeID = long.Parse(dr.GetDecimal(0).ToString());
                }
            }
            return typeID;
        }

        /// <summary>
        /// 获取最新messageid
        /// </summary>
        /// <param name="messageID">sID</param>
        /// <returns>long </returns>
        public static long GetNewMessageID(long messageID)
        {
            StringBuilder sbStrSQL = new StringBuilder();
            sbStrSQL.Append("select max(a.messageid ) from es_message a where a.flowid in (select b.flowid from es_message b where b.messageid= ");
            sbStrSQL.Append(messageID);
            sbStrSQL.Append(" ) order by a.messageid desc");

            string scn = ConfigTool.GetConnectString();
            using (OracleDataReader dr = OracleDbHelper.ExecuteReader(scn, CommandType.Text, sbStrSQL.ToString()))
            {
                if (dr.Read())
                {
                    messageID = long.Parse(dr.GetDecimal(0).ToString());
                }
            }
            return messageID;
        }

        /// <summary>
        ///  当用户参与时,获取最大的未处理的messageid
        ///  当没有最大未处理messageid 时获取最大 已经 messageid ,仅查看

        ///  当用户未参与时 获取最大 已经 messageid ,仅查看

        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <param name="lngUserID"></param>
        /// <returns></returns>
        public static long GetMessageID(long lngFlowID, long lngUserID)
        {
            long lngMessageID = 0;

            StringBuilder sbStrSQL = new StringBuilder();
            sbStrSQL.Append("SELECT max(messageid)  FROM es_message WHERE flowid = ");
            sbStrSQL.Append(lngFlowID);
            sbStrSQL.Append(" and receiverid = ");
            sbStrSQL.Append(lngUserID);
            sbStrSQL.Append(" and status = 20 ");
            sbStrSQL.Append(" union all ");
            sbStrSQL.Append(" SELECT max(messageid) FROM es_message WHERE flowid = ");
            sbStrSQL.Append(lngFlowID);
            sbStrSQL.Append(" AND status = 10 ");
            sbStrSQL.Append(" union all ");
            sbStrSQL.Append(" SELECT max(messageid)  FROM es_message WHERE flowid = ");
            sbStrSQL.Append(lngFlowID);

            string scn = ConfigTool.GetConnectString();
            using (OracleDataReader dr = OracleDbHelper.ExecuteReader(scn, CommandType.Text, sbStrSQL.ToString()))
            {
                while (dr.Read())
                {

                    if (dr.IsDBNull(0) == false)
                    {
                        lngMessageID = (long)dr.GetDecimal(0);

                        break;
                    }
                }

                return lngMessageID;
            }
        }
    }
}
