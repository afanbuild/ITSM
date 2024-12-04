/*******************************************************************
 *
 * Description
 * 
 * 
 * Create By  :zhumc
 * Create Date:2011Äê2ÔÂ14ÈÕ
 * *****************************************************************/
using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Log
{
    /// <summary>
    /// 
    /// </summary>
    public class es_flowdelelogDP
    {
        /// <summary>
        /// 
        /// </summary>
        public es_flowdelelogDP()
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

        #region AppID
        /// <summary>
        ///
        /// </summary>
        private Decimal mAppID;
        public Decimal AppID
        {
            get { return mAppID; }
            set { mAppID = value; }
        }
        #endregion

        #region Subject
        /// <summary>
        ///
        /// </summary>
        private String mSubject = string.Empty;
        public String Subject
        {
            get { return mSubject; }
            set { mSubject = value; }
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

        #region DeletedTime
        /// <summary>
        ///
        /// </summary>
        private DateTime mDeletedTime = DateTime.MinValue;
        public DateTime DeletedTime
        {
            get { return mDeletedTime; }
            set { mDeletedTime = value; }
        }
        #endregion

        #region DoUserID
        /// <summary>
        ///
        /// </summary>
        private Decimal mDoUserID;
        public Decimal DoUserID
        {
            get { return mDoUserID; }
            set { mDoUserID = value; }
        }
        #endregion

        #endregion

        #region GetReCorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>es_flowdelelogDP</returns>
        public es_flowdelelogDP GetReCorded(long lngID)
        {
            es_flowdelelogDP ee = new es_flowdelelogDP();
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM es_flowdelelog WHERE ID = " + lngID.ToString();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            foreach (DataRow dr in dt.Rows)
            {
                ee.FlowID = Decimal.Parse(dr["FlowID"].ToString());
                ee.AppID = Decimal.Parse(dr["AppID"].ToString());
                ee.Subject = dr["Subject"].ToString();
                ee.Remark = dr["Remark"].ToString();
                ee.DeletedTime = dr["DeletedTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["DeletedTime"].ToString());
                ee.DoUserID = Decimal.Parse(dr["DoUserID"].ToString());
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
            strSQL = "SELECT * FROM es_flowdelelog Where 1=1 And Deleted=0 ";
            strSQL += sWhere;
            strSQL += sOrder;
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region UpdateRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pes_flowdelelogDP></param>
        public void UpdateRecorded(es_flowdelelogDP pes_flowdelelogDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE es_flowdelelog Set " +
                                                        " AppID = " + pes_flowdelelogDP.AppID.ToString() + "," +
                            " Subject = " + StringTool.SqlQ(pes_flowdelelogDP.Subject) + "," +
                            " Remark = " + StringTool.SqlQ(pes_flowdelelogDP.Remark) + "," +
                            " DeletedTime = " + (pes_flowdelelogDP.DeletedTime == DateTime.MinValue ? " null " : StringTool.SqlQ(pes_flowdelelogDP.DeletedTime.ToString())) + "," +
                            " DoUserID = " + pes_flowdelelogDP.DoUserID.ToString() +
                                " WHERE FlowID = " + pes_flowdelelogDP.FlowID.ToString();

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
                string strSQL = "Update es_flowdelelog Set Deleted=1  WHERE ID =" + lngID.ToString();
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

        #region GetDataTable
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string sWhere, string sOrder, int pagesize, int pageindex, ref int rowcount)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "V_Es_flowdelelog", "*", sOrder, pagesize, pageindex, sWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region GetAllAppData
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllAppData()
        {
             string strSQL = "Select * from Es_App";
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion
    }
}

