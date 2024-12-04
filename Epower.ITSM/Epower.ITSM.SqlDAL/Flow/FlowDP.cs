using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;
using EpowerGlobal;
using MyComponent;
using EpowerCom;

namespace Epower.ITSM.SqlDAL
{
	/// <summary>
	/// FlowDP ��ժҪ˵����
	/// </summary>
	public class FlowDP
	{
		public FlowDP()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}


        /// <summary>
        /// ��ȡ��������б�
        /// </summary>
        /// <param name="lngFLowID"></param>
        /// <returns></returns>
        public static DataTable GetMsgProcess(long lngFLowID)
        {
            string strSQL = "SELECT  a.*,b.name,b.email,b.qq,nvl(NodeName,'') NodeName " +
                " FROM ts_user b,es_msgprocess a left outer join es_nodemodel c  ON nvl(a.flowmodelid,0)=c.flowmodelid and nvl(a.nodemodelid,0)=c.nodemodelid " +
                " WHERE a.userid = b.userid " +
                "       AND a.flowid = " + lngFLowID.ToString();
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }


		/// <summary>
		/// ɾ���������
		/// </summary>
		/// <param name="lngID"></param>
		public static void DeleteMsgProcess(long lngID)
		{
			string strSQL = "";

            

			OracleConnection cn = ConfigTool.GetConnection();

			
			strSQL="DELETE es_msgprocess WHERE mpid =" + lngID.ToString();

            try { OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSQL); }
            finally { ConfigTool.CloseConnection(cn); }					
		}


		/// <summary>
		/// �ж��û��Ƿ�Ϊ����ִ������˵Ļ��ڵĲ�����
		/// </summary>
		/// <param name="lngFlowID"></param>
		/// <param name="lngUserID"></param>
		public static bool IsFlowUser(long lngFlowID,long lngUserID)
		{
			bool ret = false;

			string strSQL = "";

		
			//����IDΪ0 �˳�
			if(lngFlowID == 0)
				return false;

			OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT messageid FROM es_message WHERE flowid = " + lngFlowID.ToString() +
                 " AND receiverid=" + lngUserID.ToString() +
                 " AND status =  " + ((int)e_MessageStatus.emsFinished).ToString();
                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                while (dr.Read())
                {
                    ret = true;
                    break;
                }
                dr.Close();
            }
            finally { ConfigTool.CloseConnection(cn); }					

			return ret;
		}


		/// <summary>
		/// ��Ӱ��������
		/// </summary>
		/// <param name="lngFlowID"></param>
		/// <param name="lngUserID"></param>
		/// <param name="strContent"></param>
		public static void AddMsgProcess(long lngFlowID,long lngUserID,string strContent)
		{
			string strSQL = "";

		
			//����IDΪ0 �˳�
			if(lngFlowID == 0)
				return;

			OracleConnection cn = ConfigTool.GetConnection();

            long lngNextID = EPGlobal.GetNextID("es_msgprocessID");
            strSQL = "INSERT INTO es_msgprocess (mpID,flowid,userid,mpcontent,mptime)" + 
				" VALUES( " +
                lngNextID.ToString() + "," +
				lngFlowID.ToString() + "," +
				lngUserID.ToString() +"," +
				StringTool.SqlQ(strContent) + "," +
				" sysdate" +
				")";

            try { OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSQL); }
            finally { ConfigTool.CloseConnection(cn); }					
		}

		/// <summary>
		/// ��ȡ�Ƿ����´�������
		/// </summary>
		/// <param name="lngID"></param>
		/// <param name="lngUserID"></param>
		/// <returns></returns>
		public static int CheckNewMessageItems(long lngID,long lngUserID)
		{
			string strSQL = @"SELECT  messageid mid FROM es_message 
								WHERE ROWNUM<=1 AND receiverid = " + lngUserID.ToString() + @" and deleted = 0 and status = 20
									and messageid > " + lngID.ToString() +
							@" UNION ALL
								SELECT a.messageid mid FROM es_receivelist a,es_message b
								WHERE ROWNUM<=1 AND a.messageid = b.messageid and  a.receiveid = " + lngUserID.ToString() + 
								@" and b.messageid  > " + lngID.ToString() + " and b.receiverid = 0";
			OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                int iRet = 0;
                while (dr.Read())
                {
                    iRet = 1;
                    break;
                }
                dr.Close();

                return iRet;
            }
            finally { ConfigTool.CloseConnection(cn); }			
		}


        /// <summary>
        /// ��ȡ�û����µ���Ϣ���ݱ�DataTable),������������
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static DataTable GetUndoMessageForNotice(long lngUserID, long lngID)
        {
            //���������졡Э�졡��ǩ ��֪
            string strSQL = "";
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT  * FROM (SELECT a.MessageID,a.FActors,a.ReceiveTime,a.actortype,b.Subject ,0 as isRec" +
                " FROM Es_Message a,Es_Flow b" +
                " WHERE ROWNUM<=1 AND a.FlowID = b.FlowID  " +
                " AND a.ReceiverID=" + lngUserID.ToString() + " AND a.Deleted =" + (int)e_Deleted.eNormal +
                " AND a.status = " + ((int)e_MessageStatus.emsHandle).ToString() +
                " AND a.messageid > " + lngID.ToString() +
                " ORDER BY a.messageid desc " +
                @" UNION ALL
						SELECT c.messageid ,d.FActors,d.ReceiveTime,d.actortype,e.Subject,1 as isRec  FROM es_receivelist c,es_message d,es_flow e
						WHERE ROWNUM<=1 AND c.messageid = d.messageid and d.flowid = e.flowid and  c.receiveid = " + lngUserID.ToString() +
                        @" and c.messageid  > " + lngID.ToString() + " and d.receiverid = 0 ORDER BY c.messageid desc ) f" +
                        " ORDER BY f.MessageID DESC";

            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);

            return dt;
        }

        #region ��ȡ���µĴ�������
        /// <summary>
        /// ��ȡ���µĴ�������
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="lngID"></param>
        /// <returns></returns>
        public static DataTable GetUndoMessage(long lngUserID, long lngID)
        {
            //���������졡Э�졡��ǩ ��֪
            string strSQL = "";
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT a.MessageID,a.FActors,a.ReceiveTime,a.actortype,b.Subject ,0 as isRec" +
                " FROM Es_Message a,Es_Flow b" +
                " WHERE ROWNUM<=1 AND a.FlowID = b.FlowID  " +
                " AND a.ReceiverID=" + lngUserID.ToString() + " AND a.Deleted =" + (int)e_Deleted.eNormal +
                " AND a.status = " + ((int)e_MessageStatus.emsHandle).ToString() +
                " AND a.messageid > " + lngID.ToString() +
                " ORDER BY a.messageid desc ";

            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);

            return dt;
        }
        /// <summary>
        /// ��ȡ���µĴ���������
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="lngID"></param>
        /// <returns></returns>
        public static DataTable GetUnRecMessage(long lngUserID, long lngID)
        {
            //���������졡Э�졡��ǩ ��֪
            string strSQL = "";
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = @"SELECT c.messageid ,d.FActors,d.ReceiveTime,d.actortype,e.Subject,1 as isRec  FROM es_receivelist c,es_message d,es_flow e
						WHERE ROWNUM<=1 AND c.messageid = d.messageid and d.flowid = e.flowid and  c.receiveid = " + lngUserID.ToString() +
                        @" and c.messageid  > " + lngID.ToString() + " and d.receiverid = 0 ORDER BY c.messageid desc";

            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);

            return dt;
        }
        #endregion 



        /// <summary>
		/// ɾ��������ص���Ϣ
		/// </summary>
		/// <param name="trans"></param>
		/// <param name="strFlowList">���̱�Ŵ��磺 1101,1203,1145 </param>
		internal static void DeleteFlowInfo(OracleTransaction trans, string strFlowList)
		{
			if(strFlowList.Trim() == "" )
			{
				return;
			}
			string strSQL = "";
			strSQL = "DELETE es_flow WHERE flowid in (" + strFlowList + ") ";
			OracleDbHelper.ExecuteNonQuery(trans,CommandType.Text,strSQL);

			strSQL = "DELETE es_message WHERE flowid in (" + strFlowList + ") ";
			OracleDbHelper.ExecuteNonQuery(trans,CommandType.Text,strSQL);

			strSQL = "DELETE es_node WHERE flowid in (" + strFlowList + ") ";
			OracleDbHelper.ExecuteNonQuery(trans,CommandType.Text,strSQL);

			strSQL = "DELETE es_messagefromto WHERE tmessageid IN " +
				" (SELECT messageid FROM es_message WHERE flowid in (" + strFlowList + "))";
			OracleDbHelper.ExecuteNonQuery(trans,CommandType.Text,strSQL);

			strSQL = "DELETE es_nodefromto WHERE fnodeid IN " +
				" (SELECT nodeid FROM es_node WHERE flowid in (" + strFlowList + "))";
			OracleDbHelper.ExecuteNonQuery(trans,CommandType.Text,strSQL);

			strSQL = "DELETE es_node_temp WHERE nodeid IN " +
				"(SELECT nodeid FROM es_node WHERE flowid in (" + strFlowList + "))";
			OracleDbHelper.ExecuteNonQuery(trans,CommandType.Text,strSQL);

		}

		/// <summary>
		/// ��ȡǰһ����ϢID
		/// </summary>
		/// <param name="lngMessageID"></param>
		/// <returns></returns>
		public static long GetPreMessageID(long lngMessageID)
		{
			string strSQL;

			long lngID=0;

			strSQL="SELECT fmessageid FROM es_messagefromto WHERE tmessageid = " + lngMessageID.ToString();

			OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);

                while (dr.Read())
                {
                    lngID = (long)dr.GetDecimal(0);
                    break;
                }
                dr.Close();                
            }
            finally { ConfigTool.CloseConnection(cn); }

			return lngID;

		}


		/// <summary>
		/// ��ȡ��Ϣ�ķ�����ID
		/// </summary>
		/// <param name="lngMessageID"></param>
		/// <returns></returns>
		public static long GetSenderID(long lngMessageID)
		{
			string strSQL;

			long lngID=0;

			strSQL="SELECT senderid FROM es_message WHERE messageid = " + lngMessageID.ToString();

			OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);

                while (dr.Read())
                {
                    lngID = (long)dr.GetDecimal(0);
                    break;
                }
                dr.Close();                
            }
            finally { ConfigTool.CloseConnection(cn); }

			return lngID;

		}


		/// <summary>
		/// ������̰���״�������ݼ�(״̬ͳ��)
		/// </summary>
		/// <param name="lngOID"></param>
		/// <param name="strStart"></param>
		/// <param name="strEnd"></param>
		/// <returns></returns>
		public static DataTable GetAnalysisDataForMessageStatus(long lngOID,string strStart,string strEnd)
		{
			string strSQL = "SELECT  case a.status when 10 then '���' when 20 then '���ڴ���' else '����' end ״̬,count(*) ���� "+
							" FROM es_message a,es_flow b,es_flowmodel c " +
							" WHERE a.flowid = b.flowid and b.flowmodelid = c.flowmodelid " +
				            "       AND c.oflowmodelid = " + lngOID.ToString();
			if(strStart.Trim().Length >0)
			{
                strSQL += " AND a.ReceiveTime >= to_date(" + StringTool.SqlQ(strStart) + ",'yyyy-MM-dd HH24:mi:ss')";
			}
			if(strEnd.Trim().Length >0)
			{
                strSQL += " AND a.ReceiveTime <= to_date(" + StringTool.SqlQ(strEnd) + ",'yyyy-MM-dd HH24:mi:ss')";
			}
			strSQL +=" group by a.status";

			OracleConnection cn = ConfigTool.GetConnection();
			DataTable dt = OracleDbHelper.ExecuteDataTable(cn,CommandType.Text,strSQL);
			ConfigTool.CloseConnection(cn);
			return dt;
		}

		/// <summary>
		/// �����ȹ���������
		/// </summary>
		/// <param name="nYear"></param>
		/// <returns></returns>
		public static DataTable GetAnalysisWorkQuantity(int nYear)
		{
			return GetAnalysisWorkQuantity(nYear,0);
		}

		public static DataTable GetAnalysisWorkQuantity(int nYear,long nOrgID)
		{
            string sSql = @"select '���" + nYear + @"' as nYear, 
                            sum(case  when a.months=1 then qty else 0 end) һ��,
							sum(case  when a.months=2 then qty else 0 end) ����,
							sum(case  when a.months=3 then qty else 0 end) ����,
							sum(case  when a.months=4 then qty else 0 end) ����,
							sum(case  when a.months=5 then qty else 0 end) ����,
							sum(case  when a.months=6 then qty else 0 end) ����,
							sum(case  when a.months=7 then qty else 0 end) ����,
							sum(case  when a.months=8 then qty else 0 end) ����,
							sum(case  when a.months=9 then qty else 0 end) ����,
							sum(case  when a.months=10 then qty else 0 end) ʮ��,
							sum(case  when a.months=11 then qty else 0 end) ʮһ��,
							sum(case  when a.months=12 then qty else 0 end) ʮ����
					from (
					select datepart('month',receivetime) as months,count(*) as qty
					from es_message
					where datepart('year',receivetime)=" + nYear.ToString()+@" #WHERE# 
					group by  datepart('month',receivetime)
					) a";

			if(nOrgID!=0)
				sSql=sSql.Replace("#WHERE#"," and recorgid="+nOrgID.ToString());
			else
				sSql=sSql.Replace("#WHERE#","");

			OracleConnection cn = ConfigTool.GetConnection();
			try
			{
				DataTable dt = OracleDbHelper.ExecuteDataTable(cn,CommandType.Text,sSql);
				return dt;
			}
			catch(Exception e)
			{
				throw e;
			}
			finally
			{
				ConfigTool.CloseConnection(cn);
			}
		}

		/// <summary>
		/// ������Ч������
		/// </summary>
		/// <param name="nYear"></param>
		/// <returns></returns>
		public static DataTable GetAnalysisWorkEfficiency(int nYear)
		{
			return GetAnalysisWorkEfficiency(nYear,0);
		}

		public static DataTable GetAnalysisWorkEfficiency(int nYear,long nOrgID)
		{
            string sSql = @"select (case status when 1 then '��ʱ' else '��ʱ' end) Status,
							sum(case  when a.months=1 then qty else 0 end) һ��,
							sum(case  when a.months=2 then qty else 0 end) ����,
							sum(case  when a.months=3 then qty else 0 end) ����,
							sum(case  when a.months=4 then qty else 0 end) ����,
							sum(case  when a.months=5 then qty else 0 end) ����,
							sum(case  when a.months=6 then qty else 0 end) ����,
							sum(case  when a.months=7 then qty else 0 end) ����,
							sum(case  when a.months=8 then qty else 0 end) ����,
							sum(case  when a.months=9 then qty else 0 end) ����,
							sum(case  when a.months=10 then qty else 0 end) ʮ��,
							sum(case  when a.months=11 then qty else 0 end) ʮһ��,
							sum(case  when a.months=12 then qty else 0 end) ʮ����
					from (
					select case when nvl(datediff('day',expected,recentprocesstime),0)<=0 
							then 1 else 0 end as status,
							datepart('month',receivetime) as months,count(*) as qty
					from es_message
					where status=10 and datepart('year',receivetime)=" + nYear.ToString() + @"  #WHERE# 
					group by case when nvl(datediff('day',expected,recentprocesstime),0)<=0 
							then 1 else 0 end ,
							datepart('month',receivetime)
					) a
					group by case status when 1 then '��ʱ' else '��ʱ' end";

            if (nOrgID != 0)
                sSql = sSql.Replace("#WHERE#", " and recorgid=" + nOrgID.ToString());
            else
                sSql = sSql.Replace("#WHERE#", "");

			OracleConnection cn = ConfigTool.GetConnection();
			try
			{
				DataTable dt = OracleDbHelper.ExecuteDataTable(cn,CommandType.Text,sSql);
				return dt;
			}
			catch(Exception e)
			{
				throw e;
			}
			finally
			{
				ConfigTool.CloseConnection(cn);
			}
		}

		/// <summary>
		/// �������ʹ�����
		/// </summary>
		/// <returns></returns>
		public static DataTable GetFlowYears()
		{
			string sSql="select distinct datepart('year',starttime) as years from es_flow ";

			OracleConnection cn = ConfigTool.GetConnection();
			try
			{
				DataTable dt = OracleDbHelper.ExecuteDataTable(cn,CommandType.Text,sSql);
				return dt;
			}
			catch(Exception e)
			{
				throw e;
			}
			finally
			{
				ConfigTool.CloseConnection(cn);
			}
		}
		/// <summary>
		/// ������̰���״�������ݼ�(�Ƿ�ʱ���ͳ��)
		/// </summary>
		/// <param name="lngOID"></param>
		/// <param name="strStart"></param>
		/// <param name="strEnd"></param>
		/// <returns></returns>
		public static DataTable GetAnalysisDataForMessageFinished(long lngOID,string strStart,string strEnd)
		{
			string strSQL = "SELECT  count(*) ����,case when datediff('minute',expected,recentprocesstime) > 0 then '��ʱ' else '��ʱ' end ��� "+
				" FROM es_message a,es_flow b,es_flowmodel c  " +
				" WHERE a.flowid = b.flowid and b.flowmodelid = c.flowmodelid and a.status = 10 " +
				"       AND c.oflowmodelid = " + lngOID.ToString();
			if(strStart.Trim().Length >0)
			{
                strSQL += " AND a.ReceiveTime >=to_date(" + StringTool.SqlQ(strStart) + ",'yyyy-MM-dd HH24:mi:ss')";
			}
			if(strEnd.Trim().Length >0)
			{
                strSQL += " AND a.ReceiveTime <=to_date(" + StringTool.SqlQ(strEnd) + ",'yyyy-MM-dd HH24:mi:ss')";
			}
			strSQL +=" group by case when datediff('minute',expected,recentprocesstime) > 0 then '��ʱ' else '��ʱ' end";

			OracleConnection cn = ConfigTool.GetConnection();
			DataTable dt = OracleDbHelper.ExecuteDataTable(cn,CommandType.Text,strSQL);
			ConfigTool.CloseConnection(cn);
			return dt;
		}


		/// <summary>
		/// ��ȡ�������̵��б��������̷���ʱ��ѡ������
		/// ԭʼ����ID����������
		/// </summary>
		/// <returns></returns>
		public static DataSet GetFlowListForSelect(long lngOrgID)
		{
			string strSQL = "SELECT a.OFlowModelID,a.FlowName FROM es_flowmodel a,es_flowmodel b WHERE a.oflowmodelid = b.flowmodelid AND a.Status = " + (int)e_fmStatus.fmFlowStarted + 
                           " AND b.orgid = " + lngOrgID.ToString() + " AND A.Deleted = " + (int)e_IsTrue.fmFalse;
			OracleConnection cn = ConfigTool.GetConnection();
			DataSet ds = OracleDbHelper.ExecuteDataset(cn,CommandType.Text,strSQL);
			ConfigTool.CloseConnection(cn);
			return ds;
		}

		public static long GetFlowIDByMessageId(long lngMessageID)
		{
			string sSql="Select FlowID from es_Message Where MessageID="+lngMessageID.ToString();
			string scn=ConfigTool.GetConnectString();
			using(OracleDataReader dr=OracleDbHelper.ExecuteReader(scn,CommandType.Text,sSql))
			{
				if(dr.Read())
				{
					object obj=dr.GetDecimal(0);
                    if (obj != null)
                    {
                        dr.Close();
                        return long.Parse(obj.ToString());
                    }
                    else
                    {
                        dr.Close();
                        return 0;
                    }
				}
				
				return 0;
			}
		}

		/// <summary>
		/// /���ݻ�ȡ���̵������ϢID
		///1������δ�����Ļ������������δ����ϢID
		///2���ۺϹ���Ա���������������Ϣid
		/// </summary>
		/// <param name="lngFlowID"></param>
		/// <param name="lngUserID"></param>
		/// <returns></returns>
		public static long GetMessageIDByFlowID(long lngFlowID,long lngUserID)
		{
			string strSQL;

			long lngMessageID=0;

			strSQL="select max(messageID) messageID,status"+
				" from es_message"+
				" where receiverid="+lngUserID.ToString()+
				" and flowid="+lngFlowID.ToString()+
				" and (status="+(int)e_MessageStatus.emsFinished+" or status="+(int)e_MessageStatus.emsHandle+")"+
				" group by status"+
				" order by status desc";

			OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);

                while (dr.Read())
                {
                    lngMessageID = (long)dr.GetDecimal(0);
                    break;
                }
                dr.Close();

                if (lngMessageID == 0)
                {
                    strSQL = "select max(messageID) messageID" +
                        " from es_message" +
                        " where flowid=" + lngFlowID.ToString() +
                        " and status=" + (int)e_MessageStatus.emsFinished;

                    dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);

                    while (dr.Read())
                    {
                        lngMessageID = (long)dr.GetDecimal(0);
                        break;
                    }
                    dr.Close();
                }
                
                return lngMessageID;
            }
            finally { ConfigTool.CloseConnection(cn); }
		}


        /// <summary>
        /// ��ȡ�鿴���������URL,upt by wxh
        /// </summary>        
        /// <param name="lngMessageID"></param>
        /// <returns></returns>
        public static string GetProcessORViewUrlV2(long lngMessageID, long lngUserID)
        {
            string sSql = "";            
            string strMessageID=string.Empty;
            string strUrl = "";

            sSql = @"SELECT max(b.messageid) 
                        FROM es_message a
                        join es_message b on a.flowid=b.flowid                      
                        where a.messageid=" + lngMessageID + @" and b.receiverid=" + lngUserID + " and b.status=20";
            string scn = ConfigTool.GetConnectString();
            strMessageID= OracleDbHelper.ExecuteScalar(scn, CommandType.Text, sSql).ToString();

            if (!string.IsNullOrEmpty(strMessageID))
            {
                //�в��봦��δ��������Ϣ
                strUrl = "flow_Normal.aspx?MessageID=" + strMessageID.ToString();
            }
            else
            {
                strUrl = "flow_Finished.aspx?MessageID=" + lngMessageID.ToString();
            }

            return strUrl;  
        }

		/// <summary>
		/// ��ȡ�鿴���������URL
		/// </summary>
		/// <param name="lngFlowID"></param>
		/// <param name="lngUserID"></param>
		/// <returns></returns>
		public static string GetProcessORViewUrl(long lngFlowID,long lngUserID)
		{
			// ���û�����ʱ,��ȡ����δ�����messageid
			//            ��û�����δ����messageid ʱ��ȡ��� �Ѿ� messageid ,���鿴
			// ���û�δ����ʱ ��ȡ��� �Ѿ� messageid ,���鿴

			string sSql="";
			long lngMessageID = 0;
			int iCount = 0;
			string strUrl = "";

			sSql = "SELECT max(messageid)  FROM es_message WHERE flowid = " + lngFlowID.ToString() +" and receiverid = " + lngUserID.ToString() + 
				" and status = 20 " + 
				" union all " +
				" SELECT max(messageid) FROM es_message WHERE flowid = " + lngFlowID.ToString() + " AND status = 10 " +
               " union all " +
				" SELECT max(messageid)  FROM es_message WHERE flowid = " + lngFlowID.ToString();

			string scn=ConfigTool.GetConnectString();
			using(OracleDataReader dr=OracleDbHelper.ExecuteReader(scn,CommandType.Text,sSql))
			{
				while(dr.Read())
				{
					
					if(dr.IsDBNull(0) == false)
					{
						lngMessageID = (long)dr.GetDecimal(0);
						
						break;
					}
					else
					{
						iCount++;
					}
				}
				dr.Close();

				if(iCount == 0)
				{
					//�в��봦��δ��������Ϣ
					strUrl = "flow_Normal.aspx?MessageID="+lngMessageID.ToString();
				}
				else
				{
					strUrl = "flow_Finished.aspx?MessageID="+lngMessageID.ToString();
				}

				return strUrl;
			}

		}


        /// <summary>
        /// ��ȡ�鿴���������URL
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <param name="lngUserID"></param>
        /// <returns></returns>
        public static long GetMessageId(long lngFlowID, long lngUserID)
        {
            // ���û�����ʱ,��ȡ����δ�����messageid
            //            ��û�����δ����messageid ʱ��ȡ��� �Ѿ� messageid ,���鿴
            // ���û�δ����ʱ ��ȡ��� �Ѿ� messageid ,���鿴

            string sSql = "";
            long lngMessageID = 0;            
            

            sSql = "SELECT max(messageid)  FROM es_message WHERE flowid = " + lngFlowID.ToString() + " and receiverid = " + lngUserID.ToString() +
                " and status = 20 " +
                " union all " +
                " SELECT max(messageid) FROM es_message WHERE flowid = " + lngFlowID.ToString() + " AND status = 10 " +
               " union all " +
                " SELECT max(messageid)  FROM es_message WHERE flowid = " + lngFlowID.ToString();

            string scn = ConfigTool.GetConnectString();
            using (OracleDataReader dr = OracleDbHelper.ExecuteReader(scn, CommandType.Text, sSql))
            {
                while (dr.Read())
                {

                    if (dr.IsDBNull(0) == false)
                    {
                        lngMessageID = (long)dr.GetDecimal(0);

                        break;
                    }                    
                }
                dr.Close();
                return lngMessageID;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flowModelID"></param>
        /// <returns></returns>
        public static long GetAppIDByFlowModelID(long flowModelID)
        {
            string sSql = "Select AppID from es_flowmodel Where rownum<=1 and flowmodelid=" + flowModelID.ToString();
            string scn = ConfigTool.GetConnectString();
            using (OracleDataReader dr = OracleDbHelper.ExecuteReader(scn, CommandType.Text, sSql))
            {
                if (dr.Read())
                {
                    object obj = dr.GetDecimal(0);
                    if (obj != null)
                        return long.Parse(obj.ToString());
                    else
                        return 0;
                }
                dr.Close();
                return 0;
            }
        }
        public static bool FlowIsFinish(long lngFlowID)
        {
            string sSql = "SELECT status FROM Es_Flow WHERE FlowID=" + lngFlowID.ToString();
            string scn = ConfigTool.GetConnectString();
            object obj = OracleDbHelper.ExecuteScalar(scn, CommandType.Text, sSql);
            if (obj != null)
            {
                if (obj.ToString() == "30")
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flowModelID"></param>
        /// <returns></returns>
        public static long GetOFlowModelID(long flowModelID)
        {
            string sSql = "Select OFlowModelID from Es_FlowModel Where rownum<=1 and flowmodelid=" + flowModelID.ToString();
            string scn = ConfigTool.GetConnectString();
            using (OracleDataReader dr = OracleDbHelper.ExecuteReader(scn, CommandType.Text, sSql))
            {
                if (dr.Read())
                {
                    object obj = dr.GetDecimal(0);
                    if (obj != null)
                        return long.Parse(obj.ToString());
                    else
                        return 0;
                }
                dr.Close();
                return 0;
            }
        }

        #region �ж���֪�Ƿ��Ѿ�����
        /// <summary>
        /// �ж���֪�Ƿ��Ѿ����ڣ�������֪�����ǰ����
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <param name="lngUserID"></param>
        /// <param name="lngNodeID"></param>
        /// <returns></returns>
        public static bool IsExistesrtTransmit(long lngFlowID, long lngUserID, long lngNodeID)
        {
            bool breturn = false;
            string sSql = "select MessageID from es_message where FlowID=" + lngFlowID + " and MessageType=50 and ReceiverID="
                + lngUserID + " and NodeID=" + lngNodeID;
            string scn = ConfigTool.GetConnectString();
            using (OracleDataReader dr = OracleDbHelper.ExecuteReader(scn, CommandType.Text, sSql))
            {
                if (dr.Read())
                {
                    object obj = dr.GetDecimal(0);
                    if (obj != null)
                        breturn = true;
                    else
                        breturn = false;
                }
                dr.Close();
                return breturn;
            }
        }
        #endregion

        #region �ж�Э���Ƿ��Ѿ�����
        /// <summary>
        /// �ж�Э���Ƿ��Ѿ����ڣ�����Э������
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <param name="lngUserID"></param>
        /// <param name="lngNodeID"></param>
        /// <returns></returns>
        public static int IsExistesrtAssist(long lngFlowID, long lngUserID, long lngNodeID)
        {
            int breturn = 0;   //������
            string sSql = "select MessageID,NodeID from es_message where FlowID=" + lngFlowID + " and MessageType in(20,60) and ReceiverID=" + lngUserID;
            DataTable dt = CommonDP.ExcuteSqlTable(sSql);
            if (dt.Rows.Count > 0)
            {
                DataRow[] drarr = dt.Select(" NodeID=" + lngNodeID);
                if (drarr.Length > 0)
                {
                    breturn = 1;     //���ڣ����ǵ�ǰ����
                }
                else
                {
                    breturn = 2;   //���ڣ������Ǳ�����
                }
            }
            return breturn;
        }
        #endregion 

        #region ȡ����ɾ�������̸���
        /// <summary>
        /// ȡ����ɾ�������̸���
        /// </summary>
        /// <param name="strFlowID"></param>
        /// <returns></returns>
        public static DataTable getDeleteAttchmentTBL(string strFlowID)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            try
            {
                string strSQL = @"select OA.*,U.name as UpuserName,OldOA.FileName as oldFiledName  from Es_Attachment OA
                left join ts_user  U on OA.upUserID=U.userid 
                left join Es_Attachment OldOA  on OA.requstFileId=to_char(OldOA.FileId)
                where nvl(OA.deleted,0)=1 and (OA.requstFileid='' or OA.requstFileid is null)  and OA.FlowID=" + strFlowID.ToString();

                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                ConfigTool.CloseConnection(cn);
                return dt;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }
        }
        #endregion

        #region �鿴�Ƿ������ݿ����Ѿ�����
        /// <summary>
        /// �鿴�Ƿ������ݿ����Ѿ�����
        /// </summary>
        /// <param name="FileId"></param>
        /// <returns></returns>
        public static DataTable getFileIsTrue(long FileId)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            try
            {
                string strSQL = "SELECT * FROM Es_Attachment where FileID=" + FileId.ToString();
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                ConfigTool.CloseConnection(cn);
                return dt;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }
        }
        #endregion

        #region ȡ�ø��¹��ĸ���
        /// <summary>
        /// ȡ�ø��¹��ĸ���
        /// </summary>
        /// <param name="lngKBID"></param>
        /// <param name="requstFileid"></param> 
        /// <returns></returns>
        public static DataTable getUpdateAttchmentTBL(string lngKBID, long requstFileid, bool IsDelete)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            try
            {
                string strSQL = string.Empty;
                if (IsDelete == false)
                {

                    strSQL = "SELECT a.deleteTime,a.flowID,a.FileID,a.FileName,a.SufName,a.Status,a.upTime,a.upUserID,b.Name as upUserName ,nvl(a.requstFileId,'') requstFileId " +
                    " FROM Es_Attachment a,Ts_User b " +
                    " WHERE a.upUserID = b.UserID " +
                    "		AND a.FlowID =" + lngKBID.ToString() + " AND a.requstFileId =" + requstFileid.ToString() + "  ORDER BY a.FileID";
                }
                else
                {
                    //����δ��������
                    strSQL = "SELECT a.deleteTime,a.flowID,a.FileID,a.FileName,a.SufName,a.Status,a.upTime,a.upUserID,b.Name as upUserName ,nvl(a.requstFileId,'') requstFileId " +
                    " FROM Es_Attachment a,Ts_User b " +
                    " WHERE a.upUserID = b.UserID " +
                    "		AND a.FlowID =" + lngKBID.ToString() + " AND a.FileID =" + requstFileid.ToString() + "  ORDER BY a.FileID";
                }

                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                ConfigTool.CloseConnection(cn);
                return dt;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }
        }
        #endregion

        #region ��ȡĳ����������
        public static string getBuidName(long lngBusId)
        {

            string strSQL = "select * from Es_BusAction where busId=" + lngBusId.ToString();
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            string BusIdName = string.Empty;
            if (dt.Rows.Count > 0)
            {
                BusIdName = dt.Rows[0]["BusName"].ToString();
            }
            return BusIdName;

        }
        #endregion

        #region ���̷���������
        /// <summary>
        /// �������������̣���ִ�����̽���ʱ�Ľӿ�
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngUserID"></param>
        /// <param name="lngMessageID"></param>
        /// <param name="strRemark"></param>
        /// <returns></returns>
        public static bool AutoEndFlow(OracleTransaction trans, long lngUserID, long lngMessageID, string strRemark)
        {
            string strMsg = string.Empty;

            string strSQL = "";
            OracleDataReader dr;

            long lngAppID = 0;
            long lngOpID = 0;
            long lngFlowModelID = 0;
            long lngNodeModelID = 0;

            long lngFlowID = 0;
            long lngNodeID = 0;
            long lngFolderID = 0;
            long lngSenderID = 0;
            long lngUserDeptID = 0;
            long lngUserOrgID = 0;

            long lngLinkNodeModelID = 0;

            long lngReceiverID = 0;   //��ǰ��Ϣ�Ľ�����,��Ҫ�ж��Ƿ�δ���յ����


            e_MessageStatus lngtmpStatus = 0;

            e_FlowStatus lngFlowStatus = e_FlowStatus.eEmpty;

            e_NodeWorkType lngNodeWorkType = e_NodeWorkType.enwtEnd;   //��������


            string strReceiversList = "";

            // *************   ����ʼ  *******************

            //	 Ȩ��У�� ��δ����


            //   �ж���Ϣ״̬����ֹͬʱ���������	
            //   ���µ�ǰ��Ϣ��״̬���ļ���ID�������ˡ�����ʱ�䡢��������
            //   ���µ�ǰ���ڵ�״̬

            lngSenderID = lngUserID;


            //   �ж���Ϣ״̬����ֹͬʱ���������

            //��ȡ��Ϣ�������Ϣ
            dr = ReadMessageInfo(trans," FlowID,NodeID,receiverid,Status ", lngMessageID);
            while (dr.Read())
            {
                lngFlowID = (long)dr.GetDecimal(0);
                lngNodeID = (long)dr.GetDecimal(1);
                lngReceiverID = (long)dr.GetDecimal(2);
                lngtmpStatus = (e_MessageStatus)dr.GetInt32(3);
                break;
            }
            dr.Close();


            //��һ�����ʱ���� try catch
            if (lngtmpStatus == e_MessageStatus.emsWaiting)
            {
                strMsg = @"���ڴ���������Ѿ�������ʱ���ܴ��� ���ܵ�ԭ���ǣ�Ƕ�׵���������û�д������";
                return false;
            }

            // ��ȡ����ģ��ID����Ϣ���ڵ�ģ�ͻ���ID��Ӧ��ID�Ͳ���ID
            strSQL = "SELECT b.flowmodelid,c.nodemodelid,b.status FROM es_node c,es_flow b " +
            " WHERE c.NodeID =" + lngNodeID.ToString() + " AND c.FlowID = b.FlowID ";
            dr = OracleDbHelper.ExecuteReader(trans, CommandType.Text, strSQL);
            while (dr.Read())
            {
                lngFlowModelID = (long)dr.GetDecimal(0);
                lngNodeModelID = (long)dr.GetDecimal(1);
                lngFlowStatus = (e_FlowStatus)dr.GetInt32(2);
                break;
            }
            dr.Close();

            if (lngFlowStatus != e_FlowStatus.efsHandle)
            {

                strMsg = @"��ǰ����ʵ���Ѿ���ͣ���������ֹ����ʱ���ܴ���";
                return false;
            }

            //��Ҫ���� �������ڵ�ģ��ID   lngLinkNodeModelID
            strSQL = "SELECT nodemodelid FROM es_nodemodel " +
                " WHERE flowmodelid =" + lngFlowModelID.ToString() + " AND type = " + ((int)e_FMNodeType.fmEnd).ToString();
            dr = OracleDbHelper.ExecuteReader(trans, CommandType.Text, strSQL);
            while (dr.Read())
            {
                lngLinkNodeModelID = (long)dr.GetDecimal(0);
                break;
            }
            dr.Close();

            long[] lngAppOP = new long[2];
            lngAppOP = GetAppandOpID(lngFlowModelID, lngNodeModelID, false);
            lngAppID = lngAppOP[0];
            lngOpID = lngAppOP[1];



            // ��ȡ�û�����������ID�ͻ���ID  2008-02-11 ��Ϊ֧�ֻ���ģʽ
            long[] lngDeptOrg = new long[2];
            lngDeptOrg = EPSystem.GetDeptandOrgID(lngUserID);
            lngUserDeptID = lngDeptOrg[0];
            lngUserOrgID = lngDeptOrg[1];

            try
            {
                // ������Ϣ��¼��״̬���ļ���ID�������ˡ�����ʱ�䡢��������
                //�°汾FolderID û�������ˣ���ʱȡ�� 2005-10-19
                //lngFolderID = Folder.GetFolderID(trans,lngSenderID,e_FolderType.epFileHandled);
                lngFolderID = 0;
                strReceiversList = "";
                strSQL = "UPDATE Es_Message SET " +
                    " FolderID = " + lngFolderID.ToString() + ",";
                if (lngReceiverID == 0)
                {
                    //δ����״��,�Զ�����
                    strSQL += " receiverid = " + lngUserID.ToString() + "," +
                              " recdeptid = " + lngUserDeptID.ToString() + "," +
                              " recorgid = " + lngUserOrgID.ToString() + ",";
                }
                strSQL += " TActors = " + MyGlobalString.SqlQ(strReceiversList) + "," +
                    " RecentProcessTime = sysdate," +
                    " ActionID = 0," +
                    " Status = " + (int)e_MessageStatus.emsFinished +
                    " WHERE MessageID=" + lngMessageID.ToString();
                // ����Ԥ���ֶ� �� Expected,WarnTime,Remark
                OracleDbHelper.ExecuteNonQuery(trans, strSQL);



                // ���»���ʵ����״̬
                strSQL = "UPDATE Es_Node SET " +
                    " WorkType=" + (int)lngNodeWorkType + "," +
                    " ActionID=0," +
                    " Status = " + (int)e_NodeStatus.ensFinished + "," +
                    " StatusTime = sysdate " +
                    " WHERE NodeID=" + lngNodeID.ToString();
                OracleDbHelper.ExecuteNonQuery(trans, strSQL);

                long lngNextID = EPGlobal.GetNextID("es_msgprocessID");
                strSQL = "INSERT INTO es_msgprocess (mpID,flowid,userid,mpcontent,isauto,mptime,flowmodelid,nodemodelid)" +
                                  " VALUES( " +
                                  lngNextID.ToString() + "," +
                                  lngFlowID.ToString() + "," +
                                  lngUserID.ToString() + "," +
                                  MyGlobalString.SqlQ("���з�����������" + strRemark + "��") + "," +
                                  "1, sysdate," +
                                  lngFlowModelID.ToString() + "," +
                                  lngNodeModelID.ToString() +
                                  ")";
                OracleDbHelper.ExecuteNonQuery(trans, strSQL);



                //�����������
                DealEnd(trans, lngUserID, lngAppID, lngFlowID, lngNodeID, lngFlowModelID, lngLinkNodeModelID, 0, false);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// ��ȡ��Ϣ����Ϣ
        /// </summary>
        /// <param name="strFieldList"></param>
        /// <param name="lngID"></param>
        /// <returns></returns>
        private static OracleDataReader ReadMessageInfo(OracleTransaction trans, string strFieldList, long lngID)
        {
            string strSQL = "";

            strSQL = "SELECT " + strFieldList +
                " FROM Es_Message" +
                " WHERE  MessageID =" + lngID.ToString();

            return OracleDbHelper.ExecuteReader(trans, CommandType.Text, strSQL);
        }

        /// <summary>
        /// ��ȡӦ��ID�Ͳ���ID
        /// </summary>
        /// <param name="lngFlowModelID"></param>
        /// <param name="lngNodeModelID"></param>
        /// <returns></returns>
        private static long[] GetAppandOpID(long lngFlowModelID, long lngNodeModelID, bool isAdd)
        {
            long[] lngAppOp = { 0, 0 };

            DataTable dt;
            if (isAdd == true)
            {
                //����ʵ����û�в���ʱ�� ��Ч�Ļ�����ȡֵ
                dt = EpSqlCacheHelper.GetDataTableFromCache("flowmodelnodes");
            }
            else
            {
                dt = EpSqlCacheHelper.GetDataTableFromCache("flowmodelnodesall");
            }
            if (dt != null)
            {
                DataRow[] drs = dt.Select("FlowModelID=" + lngFlowModelID.ToString()
                    + " AND NodeModelID=" + lngNodeModelID.ToString());

                if (drs.Length > 0)
                {
                    lngAppOp[1] = long.Parse(drs[0]["OpID"].ToString());
                    lngAppOp[0] = long.Parse(drs[0]["AppID"].ToString());
                }

                dt.Dispose();
            }
            return lngAppOp;
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngUserID"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngAppID">����Ӧ��IDʵ�ֲ�ͬӦ�õĹ鵵ϸ��</param>
        /// <param name="lngNodeID"></param>
        /// <param name="lngFlowModelID"></param>
        /// <param name="lngNodeModelID"></param>
        /// <param name="lngActionID"></param>
        /// <param name="blnAbort">�Ƿ�Ϊ��ֹ</param>
        private static void DealEnd(OracleTransaction trans, long lngUserID, long lngAppID, long lngFlowID, long lngNodeID, long lngFlowModelID, long lngNodeModelID, long lngActionID, bool blnAbort)
        {
            //			0��������������ʵ����˳��
            //			1��	������������������Ϣ������ʵ����״̬Ϊ����״̬,���������ʱ�䡣
            //			2��	����Ϣ���ļ��иĳ��Ѱ���ļ��У�ע����Щ��Ϣ�Ѿ�ת���Զ����ļ������ˣ�
            //          3  ɾ����������صĻ�����ʱ�洢��Ϣ
            //			3+ ��鲢����ǰ����Ϣ��״̬
            //			3++ ɾ�������յ���ʱ��Ϣ
            //			4��	���׳�һ���鵵���¼������¼�����Ϣ�����ģ������ַ�ʽ�����ϴ��ڷ��գ�������Ԥ���鵵�Ľӿڣ����ö��󹤳������ģʽ��ȥʵ�ֲ�ͬ�Ĺ鵵���룬ʵ�ַ�ʽͬ��Ϣ�洢����������

            string strSQL = "";
            long lngNextNodeID = 0;

            long lngPreMessageID = 0;
            e_FlowJoinType iFlowJoinType = e_FlowJoinType.efjtaNormal;

            strSQL = "SELECT premessageid,jointype " +
                " FROM es_flow WHERE flowid = " + lngFlowID.ToString();

            OracleDataReader dr = OracleDbHelper.ExecuteReader(trans,CommandType.Text, strSQL);
            while (dr.Read())
            {
                lngPreMessageID = (long)dr.GetDecimal(0);
                iFlowJoinType = (e_FlowJoinType)dr.GetInt32(1);

                break;
            }
            dr.Close();

            lngNextNodeID = EPGlobal.GetNextID("NODE_ID");
            strSQL = "INSERT INTO Es_Node (NodeID,FlowID,FlowModelID,NodeModelID,NodeModelType,WorkType,StartTime,ActionID,ExpectNumber,ArrivedNumber," +
                "Status,StatusTime) VALUES(" +
                lngNextNodeID.ToString() + "," +
                lngFlowID.ToString() + "," +
                lngFlowModelID.ToString() + "," +
                lngNodeModelID.ToString() + "," +
                (int)e_FMNodeType.fmEnd + "," +
                (int)e_NodeWorkType.enwtEnd + "," +
                "sysdate" + "," +
                lngActionID.ToString() + "," +
                "1" + "," +
                "1" + "," +
                (int)e_NodeStatus.ensFinished + "," +
                "sysdate" +
                ")";
            OracleDbHelper.ExecuteNonQuery(trans, strSQL);

            //��������˳���
            strSQL = "INSERT INTO Es_NodeFromTo (FNodeID,TNodeID) VALUES(" + lngNodeID.ToString() + "," + lngNextNodeID.ToString() + ")";
            OracleDbHelper.ExecuteNonQuery(trans, strSQL);

            strSQL = "UPDATE Es_Flow SET Status = " + (blnAbort == true ? (int)e_FlowStatus.efsAbort : (int)e_FlowStatus.efsEnd) + ",EndTime = sysdate "
                    + " WHERE FlowID=" + lngFlowID.ToString();

            OracleDbHelper.ExecuteNonQuery(trans, strSQL);


            strSQL = "UPDATE Es_Node SET Status = " + (int)e_NodeStatus.ensFinished
                    + " WHERE FlowID=" + lngFlowID.ToString(); ;

            OracleDbHelper.ExecuteNonQuery(trans, strSQL);

            strSQL = "UPDATE Es_Message SET Status = " + (int)e_MessageStatus.emsFinished
                    + " WHERE FlowID=" + lngFlowID.ToString(); ;

            OracleDbHelper.ExecuteNonQuery(trans, strSQL);

            // 2005- 08 -01 ���̽���ʱ��������Э��
            SetAllAssistFinishedForFlow(trans, lngFlowID);

            //ɾ����������ص���ʱ��¼
            strSQL = "DELETE Es_Node_Temp WHERE NodeID IN (SELECT a.NodeID FROM Es_Node a WHERE a.FlowID=" + lngFlowID.ToString() + ")";
            OracleDbHelper.ExecuteNonQuery(trans, strSQL);


            //�������ͬһ��Ϣ��Ƕ������״̬����������Ϣ״̬
            if (lngPreMessageID != 0 && iFlowJoinType == e_FlowJoinType.efjtNesting)
            {
                CheckPreMessageAndSetStatus(trans, lngPreMessageID);
            }

            // ɾ����Ϣ��ش�������Ϣ
            strSQL = @"DELETE Es_ReceiveList 
							WHERE MessageID IN (SELECT messageid FROM es_message WHERE flowid = " + lngFlowID.ToString() + ")";
            OracleDbHelper.ExecuteNonQuery(trans, strSQL);
        }

        /// <summary>
        ///  �������д�����δ��ɵ�Э����Ϣ��״̬Ϊ���,�����ļ���
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        private static void SetAllAssistFinishedForFlow(OracleTransaction trans, long lngFlowID)
        {
            //    string strSQL = "UPDATE es_message SET status = " + (int)e_MessageStatus.emsFinished + "," +
            //        " TActors = " + MyGlobalString.SqlQ("") + "," +
            //        " RecentProcessTime = sysdate" + "," +
            //        " ActionID = 0" + "," +
            //        "folderid = (SELECT min(folderid) FROM es_folder WHERE userid = a.receiverid AND type = " +
            //        (int)e_FolderType.epFileHandled + " and deleted = " + (int)e_Deleted.eNormal + ") FROM es_message a" +
            //        " WHERE a.flowid = " + lngFlowID.ToString() + " AND a.actortype = " + (int)e_ActorClass.fmAssistActor +
            //        " AND a.status <> " + (int)e_MessageStatus.emsFinished;

            // 2008-03-31 folderid �����û������ ����Ҫ�������� folderid 
            string strSQL = "UPDATE es_message SET status = " + (int)e_MessageStatus.emsFinished + "," +
                " TActors = " + MyGlobalString.SqlQ("") + "," +
                " RecentProcessTime = sysdate" + "," +
                " ActionID = 0 FROM es_message a" +
                " WHERE a.flowid = " + lngFlowID.ToString() + " AND a.actortype = " + (int)e_ActorClass.fmAssistActor +
                " AND a.status <> " + (int)e_MessageStatus.emsFinished;


            OracleDbHelper.ExecuteNonQuery(trans, strSQL);

        }

        /// <summary>
        /// �жϵ�ǰ��ϢǶ�׵�����δ��������Ƕ�������̵�״̬�������������״̬
        /// ע������δ��ɵ�����ʱ����Ϣ������״����������Ϊ������״̬
        /// ����������ǰ�᣺����ϢΪ�������Ϣ��****
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngMessageID"></param>
        private static void CheckPreMessageAndSetStatus(OracleTransaction trans, long lngMessageID)
        {
            string strSQL = "";

            bool blnAllFinished = false;
            e_MessageStatus lngCurrStatus = e_MessageStatus.emsWaiting;

            //��ȡ��Ϣ��ǰ״̬
            strSQL = "SELECT status FROM es_message WHERE messageid =" + lngMessageID.ToString();
            OracleDataReader dr = OracleDbHelper.ExecuteReader(trans,CommandType.Text, strSQL);
            while (dr.Read())
            {
                lngCurrStatus = (e_MessageStatus)dr.GetInt32(0);
            }
            dr.Close();

            //�жϵ�ǰ��ϢǶ�׵��������̵�״̬������������������ǰһ��Ϣ��״̬
            if (lngCurrStatus == e_MessageStatus.emsWaiting)
            {
                //                strSQL =  @"SELECT COUNT(flowid) FROM es_flow 
                //						WHERE premessageid = " + lngMessageID.ToString() + 
                //                    @" AND status = " + ((int)e_FlowStatus.efsHandle).ToString();
                //20090204 ���� �� ��ͣ״̬��ͳ�ƣ�ֻ��3��״̬�����Է�����������ͳ��
                //2010-04-29 ������ֹ������жϣ���ֹ��Ҳ�����
                strSQL = @"SELECT COUNT(flowid) FROM es_flow 
						WHERE premessageid = " + lngMessageID.ToString() +
                    @" AND status <> " + ((int)e_FlowStatus.efsAbort).ToString() +
                    @" AND status <> " + ((int)e_FlowStatus.efsEnd).ToString();
                dr = OracleDbHelper.ExecuteReader(trans,CommandType.Text, strSQL);
                while (dr.Read())
                {
                    if (dr.GetInt32(0) == 0)
                    {
                        blnAllFinished = true;
                    }
                }
                dr.Close();


                if (blnAllFinished == true)
                {
                    strSQL = "UPDATE es_message SET status = " + ((int)e_MessageStatus.emsHandle).ToString() +
                             " WHERE messageid = " + lngMessageID.ToString();
                    OracleDbHelper.ExecuteNonQuery(trans, strSQL);

                }
            }
        }
        #endregion 

    }
}
