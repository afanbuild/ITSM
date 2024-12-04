/*******************************************************************
 *
 * Description:关注事项
 * 
 * 
 * Create By  :
 * Create Date:2008年7月30日
 * *****************************************************************/
using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.SqlDAL
{
	/// <summary>
	/// AttentionDP 的摘要说明。
	/// </summary>
	public class AttentionDP
	{
		public AttentionDP()
		{
			
		}

		public static void AddAttention(long nFlowID,long nMessageID,long nInputUserID)
		{
            string strID = EpowerGlobal.EPGlobal.GetNextID("OA_AttentionID").ToString();
			string sSql="Insert into OA_Attention(ID,Flowid,MessageID,InputDate,InputUserID) values("+ strID + "," +
				nFlowID.ToString()+","+
				nMessageID.ToString()+",sysdate,"+
				nInputUserID.ToString()+")";
			string scn=ConfigTool.GetConnectString();
			try
			{
				OracleDbHelper.ExecuteNonQuery(scn,CommandType.Text,sSql);
			}
			catch(Exception e)
			{
				throw e;
			}
		}

		/// <summary>
		/// 判断是否已经加入了关注
		/// </summary>
		/// <param name="nMessageID"></param>
		/// <returns></returns>
		public static bool IsAdded(long nMessageID)
		{
			string sSql="Select MessageID From OA_Attention Where MessageID="+nMessageID.ToString();
			string scn=ConfigTool.GetConnectString();
			try
			{
				using(OracleDataReader dr=OracleDbHelper.ExecuteReader(scn,CommandType.Text,sSql))
				{
                    if (dr.Read())
                    {
                        dr.Close();
                        return true;
                    }
                    else
                    {
                        dr.Close();
                        return false;
                    }
                    
				}
			}
			catch(Exception e)
			{
                
				throw e;
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nAttentionID"></param>
		public static void DeleteAttention(long nAttentionID)
		{
			string sSql="Delete OA_Attention Where ID="+nAttentionID.ToString();
			string scn=ConfigTool.GetConnectString();
			try
			{
				OracleDbHelper.ExecuteNonQuery(scn,CommandType.Text,sSql);
			}
			catch(Exception e)
			{
				throw e;
			}

		}

		/// <summary>
        /// 显示关注事项
		/// </summary>
		/// <param name="nUserID"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public static DataTable GetMyAttention(long nUserID,int pageSize)
		{
            string sSql = @"select " + @" a.id,b.subject,max(c.messageid) as messageid,b.flowid,c.receiverid,e.name,b.status,g.nodename,b.AppID
						from oa_attention a
							join es_flow b on a.flowid=b.flowid
							join es_message c on b.flowid=c.flowid
							join (select flowid,max(messageid) as messageid from es_message group by flowid)h on h.messageid=c.messageid
							join ts_user e on c.receiverid=e.userid
							join es_node f on c.nodeid=f.nodeid
							join es_nodemodel g on f.nodemodelid=g.nodemodelid and g.flowmodelid=b.flowmodelid
						where ROWNUM<= " + pageSize.ToString() + " and a.inputuserid=" + nUserID + @"
						group by a.id,b.flowid,c.receiverid,e.name,b.status,b.subject,g.nodename,b.AppID";
            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sSql);
                ConfigTool.CloseConnection(cn);
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

        /// <summary>
        /// 显示关注事项
        /// </summary>
        /// <param name="nUserID"></param>
        /// <returns></returns>
        public static DataTable GetMyAttention(long nUserID)
        {
            string sSql = @"select a.id,b.subject,a.messageid as messageid,b.flowid,c.receiverid,e.name,b.status,g.nodename,b.appid
						from oa_attention a
							join es_flow b on a.flowid=b.flowid
							join es_message c on b.flowid=c.flowid
							join (select flowid,max(messageid) as messageid from es_message group by flowid)h on h.messageid=c.messageid
							join ts_user e on c.receiverid=e.userid
							join es_node f on c.nodeid=f.nodeid
							join es_nodemodel g on f.nodemodelid=g.nodemodelid and g.flowmodelid=b.flowmodelid
						where a.inputuserid=" + nUserID + @"
						--group by a.id,b.flowid,c.receiverid,e.name,b.status,b.subject,g.nodename,b.appid";
            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sSql);
                ConfigTool.CloseConnection(cn);
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

        /// <summary>
        /// 显示关注事项数量
        /// </summary>
        /// <param name="nUserID"></param>
        /// <returns></returns>
        public static int GetMyAttentionCount(long nUserID)
        {
            string sSql = @"select  a.id,b.subject,max(c.messageid) as messageid,b.flowid,c.receiverid,e.name,b.status,g.nodename
						from oa_attention a
							join es_flow b on a.flowid=b.flowid
							join es_message c on b.flowid=c.flowid
							join (select flowid,max(messageid) as messageid from es_message group by flowid)h on h.messageid=c.messageid
							join ts_user e on c.receiverid=e.userid
							join es_node f on c.nodeid=f.nodeid
							join es_nodemodel g on f.nodemodelid=g.nodemodelid and g.flowmodelid=b.flowmodelid
						where a.inputuserid=" + nUserID + @"
						group by a.id,b.flowid,c.receiverid,e.name,b.status,b.subject,g.nodename";
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sSql);
                ConfigTool.CloseConnection(cn);
                return dt.Rows.Count;
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
