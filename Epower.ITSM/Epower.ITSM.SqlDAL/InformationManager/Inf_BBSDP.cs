/********************************************************
* Generated By:   zhumc

* Date Generated: 2007��9��25��
* ******************************************************/
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
    public class Inf_BBSDP
    {
        /// <summary>
        /// 
        /// </summary>
        public Inf_BBSDP()
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

        #region KBID
        /// <summary>
        ///
        /// </summary>
        private Decimal mKBID;
        public Decimal KBID
        {
            get { return mKBID; }
            set { mKBID = value; }
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

        #region Content
        /// <summary>
        ///
        /// </summary>
        private String mContent;
        public String Content
        {
            get { return mContent; }
            set { mContent = value; }
        }
        #endregion

        #region UserName
        /// <summary>
        ///
        /// </summary>
        private String mUserName;
        public String UserName
        {
            get { return mUserName; }
            set { mUserName = value; }
        }
        #endregion

        #region RegTime
        /// <summary>
        ///
        /// </summary>
        private DateTime mRegTime;
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
        /// <returns>Inf_BBSDP</returns>
        public Inf_BBSDP GetReCorded(long lngID)
        {
            Inf_BBSDP ee = new Inf_BBSDP();
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM Inf_BBS WHERE ID = " + lngID.ToString();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            foreach (DataRow dr in dt.Rows)
            {
                ee.ID = Decimal.Parse(dr["ID"].ToString());
                ee.KBID = Decimal.Parse(dr["KBID"].ToString());
                ee.UserID = Decimal.Parse(dr["UserID"].ToString());
                ee.Content = dr["Content"].ToString();
                ee.UserName = dr["UserName"].ToString();
                ee.RegTime = DateTime.Parse(dr["RegTime"].ToString());
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
            strSQL = "SELECT * FROM Inf_BBS Where 1=1";
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
        /// <param name=pInf_BBSDP></param>
        public void InsertRecorded(Inf_BBSDP pInf_BBSDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            try
            {
                string strNextID = EpowerGlobal.EPGlobal.GetNextID("Inf_BBSID").ToString();
                strSQL = @"INSERT INTO Inf_BBS(
                                    ID,
									KBID,
									UserID,
									Content,
									UserName,
									RegTime
					)
					VALUES( " +
                            strNextID + "," +
                            pInf_BBSDP.KBID.ToString() + "," +
                            pInf_BBSDP.UserID.ToString() + "," +
                            StringTool.SqlQ(pInf_BBSDP.Content) + "," +
                            StringTool.SqlQ(pInf_BBSDP.UserName) + "," +
                            "to_date(" + StringTool.SqlQ(pInf_BBSDP.RegTime.ToString()) + ",'yyyy-MM-dd HH24:mi:ss')" +
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
        /// <param name=pInf_BBSDP></param>
        public void UpdateRecorded(Inf_BBSDP pInf_BBSDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE Inf_BBS Set " +
                            " KBID = " + pInf_BBSDP.KBID.ToString() + "," +
                            " UserID = " + pInf_BBSDP.UserID.ToString() + "," +
                            " Content = " + StringTool.SqlQ(pInf_BBSDP.Content) + "," +
                            " UserName = " + StringTool.SqlQ(pInf_BBSDP.UserName) + "," +
                            " RegTime = " + StringTool.SqlQ(pInf_BBSDP.RegTime.ToString()) +
                                " WHERE ID = " + pInf_BBSDP.ID.ToString();

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
                string strSQL = "delete Inf_BBS WHERE ID =" + lngID.ToString();
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

