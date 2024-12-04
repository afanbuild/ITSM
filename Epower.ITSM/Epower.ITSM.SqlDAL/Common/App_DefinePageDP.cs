/*******************************************************************
 *
 * Description
 * 
 * 
 * Create By  :zhumc
 * Create Date:2010年8月17日 * *****************************************************************/
using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// 
    /// </summary>
    public class App_DefinePageDP
    {
        /// <summary>
        /// 
        /// </summary>
        public App_DefinePageDP()
        { }

        #region Property
        #region FlowModelID
        /// <summary>
        ///
        /// </summary>
        private Decimal mFlowModelID = 0;
        /// <summary>
        /// 
        /// </summary>
        public Decimal FlowModelID
        {
            get { return mFlowModelID; }
            set { mFlowModelID = value; }
        }
        #endregion

        #region PageName
        /// <summary>
        ///
        /// </summary>
        private String mPageName = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public String PageName
        {
            get { return mPageName; }
            set { mPageName = value; }
        }
        #endregion

        #region ContentXml
        /// <summary>
        ///
        /// </summary>
        private string mContentXml;
        /// <summary>
        /// 
        /// </summary>
        public string ContentXml
        {
            get { return mContentXml; }
            set { mContentXml = value; }
        }
        #endregion

        #region RegUserID
        /// <summary>
        ///
        /// </summary>
        private Decimal mRegUserID;
        /// <summary>
        /// 
        /// </summary>
        public Decimal RegUserID
        {
            get { return mRegUserID; }
            set { mRegUserID = value; }
        }
        #endregion

        #region RegUserName
        /// <summary>
        ///
        /// </summary>
        private String mRegUserName = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public String RegUserName
        {
            get { return mRegUserName; }
            set { mRegUserName = value; }
        }
        #endregion

        #region RegTime
        /// <summary>
        ///
        /// </summary>
        private DateTime mRegTime = DateTime.MinValue;
        /// <summary>
        /// 
        /// </summary>
        public DateTime RegTime
        {
            get { return mRegTime; }
            set { mRegTime = value; }
        }
        #endregion

        #endregion

        #region GetReCorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>App_DefinePageDP</returns>
        public App_DefinePageDP GetReCorded(long lngID)
        {
            App_DefinePageDP ee = new App_DefinePageDP();
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM App_DefinePage WHERE FlowModelID = (SELECT OFlowModelID FROM ES_FlowModel WHERE ROWNUM<=1 AND FLowModelID=" + lngID.ToString() + ")";
            DataTable dt = null;
            try { dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL); }
            finally { ConfigTool.CloseConnection(cn); }
            
            foreach (DataRow dr in dt.Rows)
            {
                ee.FlowModelID = Decimal.Parse(dr["FlowModelID"].ToString());
                ee.PageName = dr["PageName"].ToString();
                ee.ContentXml = dr["ContentXml"].ToString();
                ee.RegUserID = Decimal.Parse(dr["RegUserID"].ToString());
                ee.RegUserName = dr["RegUserName"].ToString();
                ee.RegTime = dr["RegTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["RegTime"].ToString());
            }
            return ee;
        }
        #endregion

        #region GetDataTable
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string sWhere, string sOrder)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                strSQL = "SELECT * FROM App_DefinePage Where 1=1 ";
                strSQL += sWhere;
                strSQL += sOrder;
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                
                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion

        #region InsertRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pApp_DefinePageDP></param>
        public void InsertRecorded(App_DefinePageDP pApp_DefinePageDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            try
            {
                strSQL = @"INSERT INTO App_DefinePage(
									FlowModelID,
									PageName,
									ContentXml,
									RegUserID,
									RegUserName,
									RegTime
					)
					VALUES( " +
                            pApp_DefinePageDP.FlowModelID.ToString() + "," +
                            StringTool.SqlQ(pApp_DefinePageDP.PageName) + "," +
                            StringTool.SqlQ(pApp_DefinePageDP.ContentXml) + "," +
                            pApp_DefinePageDP.RegUserID.ToString() + "," +
                            StringTool.SqlQ(pApp_DefinePageDP.RegUserName) + "," +
                            (pApp_DefinePageDP.RegTime == DateTime.MinValue ? " null " : StringTool.SqlQ(pApp_DefinePageDP.RegTime.ToString())) +
                    ")";

                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSQL);
            }
            catch
            {
                throw;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }
        #endregion

        #region UpdateRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pApp_DefinePageDP></param>
        public void UpdateRecorded(App_DefinePageDP pApp_DefinePageDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE App_DefinePage Set " +
                            " PageName = " + StringTool.SqlQ(pApp_DefinePageDP.PageName) + "," +
                            " ContentXml = " + StringTool.SqlQ(pApp_DefinePageDP.ContentXml) + "," +
                            " RegUserID = " + pApp_DefinePageDP.RegUserID.ToString() + "," +
                            " RegUserName = " + StringTool.SqlQ(pApp_DefinePageDP.RegUserName) + "," +
                            " RegTime = " + (pApp_DefinePageDP.RegTime == DateTime.MinValue ? " null " : StringTool.SqlQ(pApp_DefinePageDP.RegTime.ToString())) +
                                " WHERE FlowModelID = " + pApp_DefinePageDP.FlowModelID.ToString();

                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSQL);
            }
            catch
            {
                throw;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }
        #endregion

        #region DeleteRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        public void DeleteRecorded(long lngID)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                string strSQL = "Delete App_DefinePage WHERE FlowModelID =" + lngID.ToString();
                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSQL);
            }
            catch
            {
                throw;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }
        #endregion

        #region GetFlowModelList
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngAppID"></param>
        /// <returns></returns>
        public static DataTable GetFlowModelList(long lngAppID)
        {
            string sSql = "SELECT OFlowModelID,FlowModelID,FlowName FROM ES_FlowModel " +
                        "WHERE Deleted=0 and status=1 AND appid = " + lngAppID.ToString();
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
        #endregion 
    }
}

