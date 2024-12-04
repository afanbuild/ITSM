/*******************************************************************
 *
 * Description
 * 
 * 
 * Create By  :zhumc
 * Create Date:2010年1月5日
 * *****************************************************************/
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
    public class EA_DefinePersonOpinionDP
    {
        /// <summary>
        /// 
        /// </summary>
        public EA_DefinePersonOpinionDP()
        { }

        #region Property
        #region ID
        /// <summary>
        ///
        /// </summary>
        private Decimal mID;
        public Decimal ID
        {
            get { return mID; }
            set { mID = value; }
        }
        #endregion

        #region Name
        /// <summary>
        ///
        /// </summary>
        private String mName = string.Empty;
        public String Name
        {
            get { return mName; }
            set { mName = value; }
        }
        #endregion

        #region UserID
        /// <summary>
        ///
        /// </summary>
        private Decimal mUserID;
        public Decimal UserID
        {
            get { return mUserID; }
            set { mUserID = value; }
        }
        #endregion

        #region UserName
        /// <summary>
        ///
        /// </summary>
        private String mUserName = string.Empty;
        public String UserName
        {
            get { return mUserName; }
            set { mUserName = value; }
        }
        #endregion

        #region CreateTime
        /// <summary>
        ///
        /// </summary>
        private DateTime mCreateTime = DateTime.MinValue;
        public DateTime CreateTime
        {
            get { return mCreateTime; }
            set { mCreateTime = value; }
        }
        #endregion

        #endregion

        #region GetReCorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>EA_DefinePersonOpinionDP</returns>
        public EA_DefinePersonOpinionDP GetReCorded(long lngID)
        {
            EA_DefinePersonOpinionDP ee = new EA_DefinePersonOpinionDP();
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT * FROM EA_DefinePersonOpinion WHERE ID = " + lngID.ToString();
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                
                foreach (DataRow dr in dt.Rows)
                {
                    ee.ID = Decimal.Parse(dr["ID"].ToString());
                    ee.Name = dr["Name"].ToString();
                    ee.UserID = Decimal.Parse(dr["UserID"].ToString());
                    ee.UserName = dr["UserName"].ToString();
                    ee.CreateTime = dr["CreateTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["CreateTime"].ToString());
                }
                return ee;
            }
            finally { ConfigTool.CloseConnection(cn); }
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
                strSQL = "SELECT * FROM EA_DefinePersonOpinion Where 1=1 ";
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
        /// <param name=pEA_DefinePersonOpinionDP></param>
        public void InsertRecorded(EA_DefinePersonOpinionDP pEA_DefinePersonOpinionDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            string strID = "0";
            try
            {
                strID = EpowerGlobal.EPGlobal.GetNextID("EA_DefinePersonOpinionID").ToString();
                pEA_DefinePersonOpinionDP.ID = decimal.Parse(strID);
                strSQL = @"INSERT INTO EA_DefinePersonOpinion(
									ID,
									Name,
									UserID,
									UserName,
									CreateTime
					)
					VALUES( " +
                            strID.ToString() + "," +
                            StringTool.SqlQ(pEA_DefinePersonOpinionDP.Name) + "," +
                            pEA_DefinePersonOpinionDP.UserID.ToString() + "," +
                            StringTool.SqlQ(pEA_DefinePersonOpinionDP.UserName) + "," +
                            (pEA_DefinePersonOpinionDP.CreateTime == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(pEA_DefinePersonOpinionDP.CreateTime.ToString()) + ",'yyyy-MM-dd HH24:mi:ss')") +
                    ")";

                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSQL);
                System.Web.HttpRuntime.Cache.Insert("CommCacheValiddefinePersonOpinion",false);
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
        /// <param name=pEA_DefinePersonOpinionDP></param>
        public void UpdateRecorded(EA_DefinePersonOpinionDP pEA_DefinePersonOpinionDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE EA_DefinePersonOpinion Set " +
                                                        " Name = " + StringTool.SqlQ(pEA_DefinePersonOpinionDP.Name) + "," +
                            " UserID = " + pEA_DefinePersonOpinionDP.UserID.ToString() + "," +
                            " UserName = " + StringTool.SqlQ(pEA_DefinePersonOpinionDP.UserName) + 
                                " WHERE ID = " + pEA_DefinePersonOpinionDP.ID.ToString();

                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSQL);
                System.Web.HttpRuntime.Cache.Insert("CommCacheValiddefinePersonOpinion",false);
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
                string strSQL = "Delete EA_DefinePersonOpinion WHERE ID =" + lngID.ToString();
                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSQL);
                System.Web.HttpRuntime.Cache.Insert("CommCacheValiddefinePersonOpinion",false);
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

        #region GetDefinePersonOp 取得自定义意见
        /// <summary>
        /// 取得自定义意见
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDefinePersonOp(long lngUserID)
        {
            DataTable dt;
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                dt = CommSqlCacheHelper.GetDataTableFromCache("definepersonopinion");

                DataTable dtTemp = dt.Clone();
                dt.DefaultView.RowFilter = " UserID = " + lngUserID.ToString();

                dtTemp.Rows.Clear();
                foreach (DataRowView dvr in dt.DefaultView)
                {
                    dtTemp.Rows.Add(dvr.Row.ItemArray);

                }

                dt.Dispose();
                return dtTemp;
            }
            else
            {
                string strSQL = "SELECT * FROM EA_DefinePersonOpinion WHERE UserID =" + lngUserID.ToString();
                OracleConnection cn = ConfigTool.GetConnection();
                try { dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL); }
                finally { ConfigTool.CloseConnection(cn); }
                
                return dt;
            }
        }
        #endregion 
    }
}

