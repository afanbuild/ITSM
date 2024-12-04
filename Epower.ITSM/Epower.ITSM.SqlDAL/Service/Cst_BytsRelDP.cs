/*******************************************************************
 *
 * Description:投诉单关联事件单
 * 
 * 
 * Create By  :zhumc
 * Create Date:2008年5月31日
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
    public class Cst_BytsRelDP
    {
        /// <summary>
        /// 
        /// </summary>
        public Cst_BytsRelDP()
        { }

        #region Property
        #region FlowID
        /// <summary>
        ///
        /// </summary>
        private Decimal mFlowID;
        public Decimal FlowID
        {
            get { return mFlowID; }
            set { mFlowID = value; }
        }
        #endregion

        #region RelFlowID
        /// <summary>
        ///
        /// </summary>
        private Decimal mRelFlowID;
        public Decimal RelFlowID
        {
            get { return mRelFlowID; }
            set { mRelFlowID = value; }
        }
        #endregion

        #region subject
        /// <summary>
        ///
        /// </summary>
        private String msubject = string.Empty;
        public String subject
        {
            get { return msubject; }
            set { msubject = value; }
        }
        #endregion

        #endregion

        #region GetReCorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>Cst_BytsRelDP</returns>
        public Cst_BytsRelDP GetReCorded(long lngID)
        {
            Cst_BytsRelDP ee = new Cst_BytsRelDP();
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM Cst_BytsRel WHERE ID = " + lngID.ToString();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            foreach (DataRow dr in dt.Rows)
            {
                ee.FlowID = Decimal.Parse(dr["FlowID"].ToString());
                ee.RelFlowID = Decimal.Parse(dr["RelFlowID"].ToString());
                ee.subject = dr["subject"].ToString();
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
            strSQL = "SELECT * FROM Cst_BytsRel Where 1=1 And Deleted=0 ";
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
        /// <param name=pCst_BytsRelDP></param>
        public void InsertRecorded(Cst_BytsRelDP pCst_BytsRelDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            string strID = "0";
            try
            {
                strID = EpowerGlobal.EPGlobal.GetNextID("Cst_BytsRelID").ToString();
                strSQL = @"INSERT INTO Cst_BytsRel(
									FlowID,
									RelFlowID,
									subject
					)
					VALUES( " +
                            strID.ToString() + "," +
                            pCst_BytsRelDP.RelFlowID.ToString() + "," +
                            StringTool.SqlQ(pCst_BytsRelDP.subject) +
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
        /// <param name=pCst_BytsRelDP></param>
        public void UpdateRecorded(Cst_BytsRelDP pCst_BytsRelDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE Cst_BytsRel Set " +
                                                        " RelFlowID = " + pCst_BytsRelDP.RelFlowID.ToString() + "," +
                            " subject = " + StringTool.SqlQ(pCst_BytsRelDP.subject) +
                                " WHERE FlowID = " + pCst_BytsRelDP.FlowID.ToString();

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
                string strSQL = "Update Cst_BytsRel Set Deleted=1  WHERE ID =" + lngID.ToString();
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

