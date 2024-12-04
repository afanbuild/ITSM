/*******************************************************************
 * ��Ȩ���У�
 * Description���������ݴ�����
 * 
 * 
 * Create By  ��zhumingchun
 * Create Date��2007-08-28
 * *****************************************************************/
using System;
using System.Data;
using System.Data.OracleClient;
using System.Data.OleDb;
using System.Xml;
using System.IO;

using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;
using EpowerGlobal;
using EpowerCom;

namespace Epower.ITSM.SqlDAL
{
	/// <summary>
	/// MonitorDP ��ժҪ˵����
	/// </summary>
    public class MonitorDP
	{
		/// <summary>
		/// EMSDP
		/// </summary>
        public MonitorDP()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
        }

        /// <summary>
        /// ����FlowIDȡ�ö������ݼ�
        /// </summary>
        /// <param name="LngFlowID"></param>
        /// <returns></returns>
        public static DataTable GetMonitor(long lngFlowID)
        {
            string sSql = " SELECT *" +
                " FROM EA_Monitor " +
                " WHERE FlowID=" + lngFlowID.ToString() +
                " ORDER BY RegTime DESC";

            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sSql);                
                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }

        /// <summary>
        /// ��Ӷ���
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <param name="lngUserID"></param>
        /// <param name="strSuggest"></param>
        /// <param name="strUserName"></param>
        /// <param name="lngAppID"></param>
        public static void AdMonitor(long lngFlowID, long lngUserID, string strSuggest, string strUserName, long lngAppID)
        {
            string strSQL = string.Empty;
            //����IDΪ0 �˳�
            if (lngFlowID == 0)
                return;
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                string strID = EpowerGlobal.EPGlobal.GetNextID("EA_Monitor_SEQUENCE").ToString();

                strSQL = "INSERT INTO EA_Monitor(Monitorid,AppID,FlowID,RegTime,Suggest,RegUserID,RegUserName)" +
                  " VALUES(" + strID + ", " +
                  lngAppID.ToString() + "," +
                  lngFlowID.ToString() + "," +
                  "sysdate," +
                  StringTool.SqlQ(strSuggest) + "," +
                  lngUserID.ToString() + "," +
                  StringTool.SqlQ(strUserName) +
                  ")";
                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSQL);
            }
            finally { ConfigTool.CloseConnection(cn); }            
        }

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="lngID"></param>
        public static void DeleteMonitor(long lngID)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                strSQL = "DELETE EA_Monitor WHERE MonitorID =" + lngID.ToString();
                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSQL);
            }
            finally { ConfigTool.CloseConnection(cn); }            
        }
    }
}
