/*******************************************************************
 *
 * Description:短消息处理类
 * 
 * 
 * Create By  :
 * Create Date:2008年7月30日
 * *****************************************************************/
using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;

namespace Epower.ITSM.SqlDAL
{
	/// <summary>
	/// SMSDp 的摘要说明。
	/// </summary>
	public class SMSDp
	{
		public SMSDp()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}



		/// <summary>
        /// 获取用户的短消息
		/// </summary>
		/// <param name="lngUserID"></param>
		/// <param name="intNum"></param>
		/// <param name="sWhere"></param>
		/// <param name="iReadStatus"></param>
		/// <returns></returns>
        public static DataTable GetSMS(long lngUserID, int intNum, string sWhere, int iReadStatus, string dBeginDate, string dEndDate)
		{
            string strSQL = "SELECT a.smsid,a.Content,a.SendTime,b.Name as SenderName,a.SenderID,nvl(ReadStatus,'0') ReadStatus,case when nvl(ReadStatus,0)='0' then '未读' else '已读' end ReadStatusName" +
               " FROM OA_sms a,ts_user b" +
               " WHERE ROWNUM<= " + intNum.ToString() + " and a.ReceiverID = " + lngUserID.ToString() + " AND a.Deleted = 0 AND " +
               " a.SenderID = b.UserID ";
            strSQL += sWhere + " AND nvl(ReadStatus,'0')=" + iReadStatus.ToString();
            if (dBeginDate != string.Empty)
                strSQL += " And SendTime >= to_date(" + StringTool.SqlQ(dBeginDate) + ",'yyyy-mm-dd HH24:mi:ss')";
            if (dEndDate != string.Empty)
                strSQL += " And SendTime <to_date(" + StringTool.SqlQ(DateTime.Parse(dEndDate).AddDays(1).ToShortDateString()) + ",'yyyy-mm-dd HH24:mi:ss')";

            strSQL += " ORDER BY sendtime DESC";
			DataTable dt;
			OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);                

                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }
		}
	

		/// <summary>
		/// 保存短消息
		/// </summary>
		/// <param name="lngSMSID"></param>
		/// <param name="lngSenderID"></param>
		/// <param name="lngRecID"></param>
		/// <param name="strContent"></param>
		public static void SaveSMS(long lngSMSID,long lngSenderID,long lngRecID,string strContent)
		{
			if(lngSMSID != 0)
			{
				UpdateSMS(lngSMSID,strContent);
			}
			else
			{
				if(strContent !="" & lngSenderID !=0 & lngRecID != 0)
				{
					AddSMS(lngSenderID,lngRecID,strContent);
				}
			}
		}

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="lngSenderID"></param>
        /// <param name="lngRecID"></param>
        /// <param name="strContent"></param>
		private static void AddSMS(long lngSenderID,long lngRecID,string strContent)
		{
			string strSQL = "";
			OracleConnection cn = ConfigTool.GetConnection();
			try
			{
                string strID = EpowerGlobal.EPGlobal.GetNextID("OA_SMS_SEQUENCE").ToString();

                strSQL = "INSERT INTO OA_SMS (SMSID,SenderID,ReceiverID,Content,SendTime,Deleted)" +
                    " Values(" + strID + "," +
                    lngSenderID.ToString() + "," +
                    lngRecID.ToString() + "," +
                    StringTool.SqlQ(strContent) + ",sysdate,0" +
                    ")";

				OracleDbHelper.ExecuteNonQuery(cn,CommandType.Text,strSQL);

			}
			catch(Exception err)
			{
				throw new Exception(err.Message );
			}
			finally
			{
				ConfigTool.CloseConnection(cn);
			}

		}

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="lngSMSID"></param>
        /// <param name="strContent"></param>
		private static void UpdateSMS(long lngSMSID,string strContent)
		{
			string strSQL = "";
			OracleConnection cn = ConfigTool.GetConnection();
			try
			{
				strSQL = "UPDATE OA_sms SET " +
					" content = " +	StringTool.SqlQ(strContent) + 
					" WHERE smsid = " + lngSMSID.ToString();

				OracleDbHelper.ExecuteNonQuery(cn,CommandType.Text,strSQL);
			}
			catch(Exception err)
			{
				throw new Exception(err.Message );
			}
			finally
			{
				ConfigTool.CloseConnection(cn);
			}
		}


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="lngSMSID"></param>
		public static void DeleteSMS(long lngSMSID)
		{
			string strSQL = "";
			OracleConnection cn = ConfigTool.GetConnection();
			try
			{
				strSQL = "Update OA_sms  set deleted=1" +
					" WHERE smsid = " + lngSMSID.ToString();

				OracleDbHelper.ExecuteNonQuery(cn,CommandType.Text,strSQL);
			}
			catch(Exception err)
			{
				throw new Exception(err.Message );
			}
			finally
			{
				ConfigTool.CloseConnection(cn);
			}
		}

        /// <summary>
        /// 已读
        /// </summary>
        /// <param name="lngSMSID"></param>
        public static void ReadSMS(long lngSMSID)
        {
            string strSQL = "";
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                strSQL = "UPDATE OA_sms SET " +
                    " ReadStatus = 1" +
                    " WHERE smsid = " + lngSMSID.ToString();

                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSQL);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <returns></returns>
        public static string GetSMSReadCount(long lngUserID)
        {
            string strSQL = "SELECT count(smsid)" +
                " FROM OA_sms " +
                " WHERE ReceiverID = " + lngUserID.ToString() + " AND Deleted = 0 And nvl(ReadStatus,0)=0";
            DataTable dt;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                string inum = "0";
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                if (dt.Rows.Count > 0)
                    inum = dt.Rows[0][0].ToString();                

                return inum;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }

	}
}
