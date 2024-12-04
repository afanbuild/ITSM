/*******************************************************************
 *
 * Description:标准定义
 * 
 * 
 * Create By  :zhumc
 * Create Date:2008年4月25日
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
    public class Cst_GuidDefinitionDP
    {
        /// <summary>
        /// 
        /// </summary>
        public Cst_GuidDefinitionDP()
        { }

        #region Property
        #region GuidID
        /// <summary>
        ///
        /// </summary>
        private Decimal mGuidID;
        public Decimal GuidID
        {
            get { return mGuidID; }
            set { mGuidID = value; }
        }
        #endregion

        #region GuidName
        /// <summary>
        ///
        /// </summary>
        private String mGuidName;
        public String GuidName
        {
            get { return mGuidName; }
            set { mGuidName = value; }
        }
        #endregion

        #endregion

        #region GetReCorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>Cst_GuidDefinitionDP</returns>
        public Cst_GuidDefinitionDP GetReCorded(long lngID)
        {
            Cst_GuidDefinitionDP ee = new Cst_GuidDefinitionDP();
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM Cst_GuidDefinition WHERE ID = " + lngID.ToString();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            foreach (DataRow dr in dt.Rows)
            {
                ee.GuidID = Decimal.Parse(dr["GuidID"].ToString());
                ee.GuidName = dr["GuidName"].ToString();
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
            strSQL = "SELECT * FROM Cst_GuidDefinition Where 1=1";
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
        /// <param name=pCst_GuidDefinitionDP></param>
        public void InsertRecorded(Cst_GuidDefinitionDP pCst_GuidDefinitionDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            string strID = "0";
            try
            {
                strSQL = @"INSERT INTO Cst_GuidDefinition(
									GuidName
					)
					VALUES( " +
                            StringTool.SqlQ(pCst_GuidDefinitionDP.GuidName) +
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
        /// <param name=pCst_GuidDefinitionDP></param>
        public void UpdateRecorded(Cst_GuidDefinitionDP pCst_GuidDefinitionDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE Cst_GuidDefinition Set " +
                                                        " GuidName = " + StringTool.SqlQ(pCst_GuidDefinitionDP.GuidName) +
                                " WHERE GuidID = " + pCst_GuidDefinitionDP.GuidID.ToString();

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
                string strSQL = "Delete Cst_GuidDefinition WHERE ID =" + lngID.ToString();
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
    }
}

