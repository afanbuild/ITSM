
/*******************************************************************
 *
 * Description:导入配置
 * 
 * 
 * Create By  :zmc
 * Create Date:2008年7月16日
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
    public class EA_ExcelInportDP
    {
        /// <summary>
        /// 
        /// </summary>
        public EA_ExcelInportDP()
        { }

        #region Property
        #region CatalogID
        /// <summary>
        ///
        /// </summary>
        private Decimal mCatalogID;
        public Decimal CatalogID
        {
            get { return mCatalogID; }
            set { mCatalogID = value; }
        }
        #endregion

        #region CatalogName
        /// <summary>
        ///
        /// </summary>
        private String mCatalogName = string.Empty;
        public String CatalogName
        {
            get { return mCatalogName; }
            set { mCatalogName = value; }
        }
        #endregion

        #region TempTableName
        /// <summary>
        ///
        /// </summary>
        private String mTempTableName = string.Empty;
        public String TempTableName
        {
            get { return mTempTableName; }
            set { mTempTableName = value; }
        }
        #endregion

        #region CreateTabelScript
        /// <summary>
        ///
        /// </summary>
        private String mCreateTabelScript = string.Empty;
        public String CreateTabelScript
        {
            get { return mCreateTabelScript; }
            set { mCreateTabelScript = value; }
        }
        #endregion

        #region ExcScript
        /// <summary>
        ///
        /// </summary>
        private String mExcScript = string.Empty;
        public String ExcScript
        {
            get { return mExcScript; }
            set { mExcScript = value; }
        }
        #endregion

        #region UnInportScript
        /// <summary>
        ///
        /// </summary>
        private String mUnInportScript = string.Empty;
        public String UnInportScript
        {
            get { return mUnInportScript; }
            set { mUnInportScript = value; }
        }
        #endregion

        #region InportScript
        /// <summary>
        ///
        /// </summary>
        private String mInportScript = string.Empty;
        public String InportScript
        {
            get { return mInportScript; }
            set { mInportScript = value; }
        }
        #endregion

        #region CanInportScript
        /// <summary>
        ///
        /// </summary>
        private String mCanInportScript = string.Empty;
        public String CanInportScript
        {
            get { return mCanInportScript; }
            set { mCanInportScript = value; }
        }
        #endregion

        #region UnCanInportScript
        /// <summary>
        ///
        /// </summary>
        private String mUnCanInportScript = string.Empty;
        public String UnCanInportScript
        {
            get { return mUnCanInportScript; }
            set { mUnCanInportScript = value; }
        }
        #endregion

        #region Remark
        /// <summary>
        ///
        /// </summary>
        private String mRemark = string.Empty;
        public String Remark
        {
            get { return mRemark; }
            set { mRemark = value; }
        }
        #endregion

        #endregion

        #region GetReCorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>EA_ExcelInportDP</returns>
        public EA_ExcelInportDP GetReCorded(long lngID)
        {
            EA_ExcelInportDP ee = new EA_ExcelInportDP();
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT * FROM EA_ExcelInport WHERE CatalogID = " + lngID.ToString();
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                
                foreach (DataRow dr in dt.Rows)
                {
                    ee.CatalogID = Decimal.Parse(dr["CatalogID"].ToString());
                    ee.CatalogName = dr["CatalogName"].ToString();
                    ee.TempTableName = dr["TempTableName"].ToString();
                    ee.CreateTabelScript = dr["CreateTabelScript"].ToString();
                    ee.ExcScript = dr["ExcScript"].ToString();
                    ee.UnInportScript = dr["UnInportScript"].ToString();
                    ee.InportScript = dr["InportScript"].ToString();
                    ee.CanInportScript = dr["CanInportScript"].ToString();
                    ee.UnCanInportScript = dr["UnCanInportScript"].ToString();
                    ee.Remark = dr["Remark"].ToString();
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
            strSQL = "SELECT * FROM EA_ExcelInport Where 1=1 And Deleted=0 ";
            strSQL += sWhere;
            strSQL += sOrder;
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region InsertRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pEA_ExcelInportDP></param>
        public void InsertRecorded(EA_ExcelInportDP pEA_ExcelInportDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            try
            {
                strSQL = @"INSERT INTO EA_ExcelInport(
									CatalogID,
									CatalogName,
									TempTableName,
									CreateTabelScript,
									ExcScript,
									UnInportScript,
									InportScript,
									CanInportScript,
									UnCanInportScript,
									Remark,
                                    Deleted
					)
					VALUES( " +
                            pEA_ExcelInportDP.CatalogID.ToString() + "," +
                            StringTool.SqlQ(pEA_ExcelInportDP.CatalogName) + "," +
                            StringTool.SqlQ(pEA_ExcelInportDP.TempTableName) + "," +
                            StringTool.SqlQ(pEA_ExcelInportDP.CreateTabelScript) + "," +
                            StringTool.SqlQ(pEA_ExcelInportDP.ExcScript) + "," +
                            StringTool.SqlQ(pEA_ExcelInportDP.UnInportScript) + "," +
                            StringTool.SqlQ(pEA_ExcelInportDP.InportScript) + "," +
                            StringTool.SqlQ(pEA_ExcelInportDP.CanInportScript) + "," +
                            StringTool.SqlQ(pEA_ExcelInportDP.UnCanInportScript) + "," +
                            StringTool.SqlQ(pEA_ExcelInportDP.Remark) + "," +
                            "0" +
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
        /// <param name=pEA_ExcelInportDP></param>
        public void UpdateRecorded(EA_ExcelInportDP pEA_ExcelInportDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE EA_ExcelInport Set " +
                                                        " CatalogName = " + StringTool.SqlQ(pEA_ExcelInportDP.CatalogName) + "," +
                            " TempTableName = " + StringTool.SqlQ(pEA_ExcelInportDP.TempTableName) + "," +
                            " CreateTabelScript = " + StringTool.SqlQ(pEA_ExcelInportDP.CreateTabelScript) + "," +
                            " ExcScript = " + StringTool.SqlQ(pEA_ExcelInportDP.ExcScript) + "," +
                            " UnInportScript = " + StringTool.SqlQ(pEA_ExcelInportDP.UnInportScript) + "," +
                            " InportScript = " + StringTool.SqlQ(pEA_ExcelInportDP.InportScript) + "," +
                            " CanInportScript = " + StringTool.SqlQ(pEA_ExcelInportDP.CanInportScript) + "," +
                            " UnCanInportScript = " + StringTool.SqlQ(pEA_ExcelInportDP.UnCanInportScript) + "," +
                            " Remark = " + StringTool.SqlQ(pEA_ExcelInportDP.Remark) +
                                " WHERE CatalogID = " + pEA_ExcelInportDP.CatalogID.ToString();

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
                string strSQL = "Update EA_ExcelInport set Deleted=1 WHERE CatalogID =" + lngID.ToString();
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

        #region 判断重复值 CheckIsRepeat
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iCatalogID"></param>
        /// <param name="bAddOrEdit">true修改,False新增</param>
        /// <returns></returns>
        public bool CheckIsRepeat(decimal iCatalogID,bool bAddOrEdit)
        {
            bool sReturn = false;
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT CatalogID FROM EA_ExcelInport Where 1=1 And Deleted=0 And CatalogID=" + iCatalogID.ToString();
                if (bAddOrEdit)  //修改
                    strSQL += " and CatalogID<>" + iCatalogID.ToString();
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                if (dt.Rows.Count > 0)
                    sReturn = true;
                
                return sReturn;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion 
    }
}

