using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using EpowerGlobal;

namespace Epower.ITSM.SqlDAL
{
	/// <summary>
	/// ProcessDp 的摘要说明。
	/// </summary>
	public class ProcessDP
	{
		public ProcessDP()
		{
			
		}

		/// <summary>
		/// 获取表示消息过程的一个数据表
		/// </summary>
		/// <param name="lngFlowModelID"></param>
		/// <param name="lngFlowID"></param>
		/// <returns></returns>
		public static DataTable GetProcessStatus(long lngFlowModelID,long lngFlowID)
		{
			/*
			string sSql=@"SELECT a.messageid,convert(varchar(16),a.receivetime,120)as receivetime,
								convert(varchar(16),a.recentprocesstime,120) as recentprocesstime,
								d.name as Factors,a.Tactors,a.opinion,c.nodename,a.status,
								case a.status when '10' then '完成' when '20' then '处理中' when '30' then '等待' end as statusname
							FROM es_message a,es_node b,es_nodemodel c,ts_user d
							WHERE a.flowid=b.flowid and a.nodeid=b.nodeid and a.receiverid=d.userid and d.deleted=0
								and b.nodemodelid=c.nodemodelid
								and b.flowid="+lngFlowID.ToString()+" and c.flowmodelid="+lngFlowModelID.ToString()+
								" and a.status="+(int)e_MessageStatus.emsFinished+
							" ORDER BY a.messageid";
							*/

            string sSql = @"SELECT a.messageid,TO_CHAR(a.receivetime)as receivetime,a.receiverid,a.senderid,a.actortype,a.receivetype,
								/*convert(varchar(16),a.recentprocesstime ,120) as dotime*/a.recentprocesstime as dotime,
								d.name as tactors,a.opinion,c.nodename,a.status,nvl(e.tmessageID,0) as tmessageID
							FROM es_message a 
								left join es_messagefromto e on a.messageid=e.fmessageid
								join es_node b on a.flowid=b.flowid and a.nodeid=b.nodeid
								join es_nodemodel c on  b.nodemodelid=c.nodemodelid	
								join ts_user d on a.receiverid=d.userid and d.deleted=0
							WHERE  b.flowid=#flowid# and c.flowmodelid=#flowmodelid#
							ORDER BY nvl(a.recentprocesstime,'9999-12-31')";//order a.receivetime
			
			sSql=sSql.Replace("#flowid#",lngFlowID.ToString()).Replace("#flowmodelid#",lngFlowModelID.ToString());
			
			OracleConnection cn=ConfigTool.GetConnection();
			DataTable dt=OracleDbHelper.ExecuteDataTable(cn,CommandType.Text,sSql);
			ConfigTool.CloseConnection(cn);
			return dt;
		}

	}
}
