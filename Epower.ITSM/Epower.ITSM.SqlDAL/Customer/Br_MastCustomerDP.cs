
/*******************************************************************
 *
 * Description:服务单位维护
 * 
 * 
 * Create By  :zhumc
 * Create Date:2008年4月2日
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
    public class Br_MastCustomerDP
    {
        /// <summary>
        /// 
        /// </summary>
        public Br_MastCustomerDP()
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

        #region ShortName
        /// <summary>
        ///
        /// </summary>
        private String mShortName = string.Empty;
        public String ShortName
        {
            get { return mShortName; }
            set { mShortName = value; }
        }
        #endregion

        #region FullName
        /// <summary>
        ///
        /// </summary>
        private String mFullName = string.Empty;
        public String FullName
        {
            get { return mFullName; }
            set { mFullName = value; }
        }
        #endregion

        #region Address
        /// <summary>
        ///
        /// </summary>
        private String mAddress = string.Empty;
        public String Address
        {
            get { return mAddress; }
            set { mAddress = value; }
        }
        #endregion

        #region EnterpriseType
        /// <summary>
        ///
        /// </summary>
        private Decimal mEnterpriseType = 0;
        public Decimal EnterpriseType
        {
            get { return mEnterpriseType; }
            set { mEnterpriseType = value; }
        }
        #endregion

        #region EnterpriseTypeName
        /// <summary>
        ///
        /// </summary>
        private String mEnterpriseTypeName = string.Empty;
        public String EnterpriseTypeName
        {
            get { return mEnterpriseTypeName; }
            set { mEnterpriseTypeName = value; }
        }
        #endregion

        #region CustomerType
        /// <summary>
        ///
        /// </summary>
        private Decimal mCustomerType = 0;
        public Decimal CustomerType
        {
            get { return mCustomerType; }
            set { mCustomerType = value; }
        }
        #endregion

        #region CustomerTypeName
        /// <summary>
        ///
        /// </summary>
        private String mCustomerTypeName = string.Empty;
        public String CustomerTypeName
        {
            get { return mCustomerTypeName; }
            set { mCustomerTypeName = value; }
        }
        #endregion

        #region CustomCode
        /// <summary>
        ///
        /// </summary>
        private String mCustomCode = string.Empty;
        public String CustomCode
        {
            get { return mCustomCode; }
            set { mCustomCode = value; }
        }
        #endregion

        #region Tel1
        /// <summary>
        ///
        /// </summary>
        private String mTel1 = string.Empty;
        public String Tel1
        {
            get { return mTel1; }
            set { mTel1 = value; }
        }
        #endregion

        #region LinkMan1
        /// <summary>
        ///
        /// </summary>
        private String mLinkMan1 = string.Empty;
        public String LinkMan1
        {
            get { return mLinkMan1; }
            set { mLinkMan1 = value; }
        }
        #endregion

        #region Fax1
        /// <summary>
        ///
        /// </summary>
        private String mFax1 = string.Empty;
        public String Fax1
        {
            get { return mFax1; }
            set { mFax1 = value; }
        }
        #endregion

        #region WebSite
        /// <summary>
        ///
        /// </summary>
        private String mWebSite = string.Empty;
        public String WebSite
        {
            get { return mWebSite; }
            set { mWebSite = value; }
        }
        #endregion

        #region ServiceProtocol
        /// <summary>
        ///
        /// </summary>
        private String mServiceProtocol;
        public String ServiceProtocol
        {
            get { return mServiceProtocol; }
            set { mServiceProtocol = value; }
        }
        #endregion

        #region Deleted
        /// <summary>
        ///
        /// </summary>
        private Int32 mDeleted;
        public Int32 Deleted
        {
            get { return mDeleted; }
            set { mDeleted = value; }
        }
        #endregion

        #region RegUserID
        /// <summary>
        ///
        /// </summary>
        private Decimal mRegUserID = 0;
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
        public DateTime RegTime
        {
            get { return mRegTime; }
            set { mRegTime = value; }
        }
        #endregion

        #endregion

        #region GetReCorded
        /// <summary>
        /// GetReCorded
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>Br_MastCustomerDP</returns>
        public Br_MastCustomerDP GetReCorded(long lngID)
        {
            Br_MastCustomerDP ee = new Br_MastCustomerDP();
            DataTable dt;
            //2008-05-02 添加SQL缓存依赖的处理方式,减少数据库连接次数
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                dt = CommSqlCacheHelper.GetDataTableFromCache("mastcustomer");

                dt.DefaultView.RowFilter = " ID = " + lngID.ToString();

                foreach (DataRowView dvr in dt.DefaultView)
                {
                    ee.ID = Decimal.Parse(dvr.Row["ID"].ToString());
                    ee.ShortName = dvr.Row["ShortName"].ToString();
                    ee.FullName = dvr.Row["FullName"].ToString();
                    ee.Address = dvr.Row["Address"].ToString();
                    ee.EnterpriseType = Decimal.Parse(dvr.Row["EnterpriseType"].ToString() == "" ? "0" : dvr.Row["EnterpriseType"].ToString());
                    ee.EnterpriseTypeName = dvr.Row["EnterpriseTypeName"].ToString();
                    ee.CustomerType = Decimal.Parse(dvr.Row["CustomerType"].ToString() == "" ? "0" : dvr.Row["CustomerType"].ToString());
                    ee.CustomerTypeName = dvr.Row["CustomerTypeName"].ToString();
                    ee.CustomCode = dvr.Row["CustomCode"].ToString();
                    ee.Tel1 = dvr.Row["Tel1"].ToString();
                    ee.LinkMan1 = dvr.Row["LinkMan1"].ToString();
                    ee.Fax1 = dvr.Row["Fax1"].ToString();
                    ee.WebSite = dvr.Row["WebSite"].ToString();
                    ee.ServiceProtocol = dvr.Row["ServiceProtocol"].ToString();
                    ee.Deleted = Int32.Parse(dvr.Row["Deleted"].ToString() == "" ? "0" : dvr.Row["Deleted"].ToString());
                    ee.RegUserID = Decimal.Parse(dvr.Row["RegUserID"].ToString() == "" ? "0" : dvr.Row["RegUserID"].ToString());
                    ee.RegUserName = dvr.Row["RegUserName"].ToString();
                    ee.RegTime = dvr.Row["RegTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dvr.Row["RegTime"].ToString());
                }
                return ee;


            }
            else
            {

                string strSQL = string.Empty;
                OracleConnection cn = ConfigTool.GetConnection();
                strSQL = "SELECT * FROM Br_MastCustomer WHERE ID = " + lngID.ToString();
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                ConfigTool.CloseConnection(cn);
                foreach (DataRow dr in dt.Rows)
                {
                    ee.ID = Decimal.Parse(dr["ID"].ToString());
                    ee.ShortName = dr["ShortName"].ToString();
                    ee.FullName = dr["FullName"].ToString();
                    ee.Address = dr["Address"].ToString();
                    ee.EnterpriseType = Decimal.Parse(dr["EnterpriseType"].ToString() == "" ? "0" : dr["EnterpriseType"].ToString());
                    ee.EnterpriseTypeName = dr["EnterpriseTypeName"].ToString();
                    ee.CustomerType = Decimal.Parse(dr["CustomerType"].ToString() == "" ? "0" : dr["CustomerType"].ToString());
                    ee.CustomerTypeName = dr["CustomerTypeName"].ToString();
                    ee.CustomCode = dr["CustomCode"].ToString();
                    ee.Tel1 = dr["Tel1"].ToString();
                    ee.LinkMan1 = dr["LinkMan1"].ToString();
                    ee.Fax1 = dr["Fax1"].ToString();
                    ee.WebSite = dr["WebSite"].ToString();
                    ee.ServiceProtocol = dr["ServiceProtocol"].ToString();
                    ee.Deleted = Int32.Parse(dr["Deleted"].ToString() == "" ? "0" : dr["Deleted"].ToString());
                    ee.RegUserID = Decimal.Parse(dr["RegUserID"].ToString() == "" ? "0" : dr["RegUserID"].ToString());
                    ee.RegUserName = dr["RegUserName"].ToString();
                    ee.RegTime = dr["RegTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["RegTime"].ToString());
                }
                return ee;
            }
        }
        /// <summary>
        /// GetReCorded
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>Br_MastCustomerDP</returns>
        public Br_MastCustomerDP GetReCorded(string shortName)
        {
            Br_MastCustomerDP ee = new Br_MastCustomerDP();
            DataTable dt;
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM Br_MastCustomer WHERE ShortName = " + StringTool.SqlQ(shortName);
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            foreach (DataRow dr in dt.Rows)
            {
                ee.ID = Decimal.Parse(dr["ID"].ToString());
                ee.ShortName = dr["ShortName"].ToString();
                ee.FullName = dr["FullName"].ToString();
                ee.Address = dr["Address"].ToString();
                ee.EnterpriseType = Decimal.Parse(dr["EnterpriseType"].ToString());
                ee.EnterpriseTypeName = dr["EnterpriseTypeName"].ToString();
                ee.CustomerType = Decimal.Parse(dr["CustomerType"].ToString());
                ee.CustomerTypeName = dr["CustomerTypeName"].ToString();
                ee.CustomCode = dr["CustomCode"].ToString();
                ee.Tel1 = dr["Tel1"].ToString();
                ee.LinkMan1 = dr["LinkMan1"].ToString();
                ee.Fax1 = dr["Fax1"].ToString();
                ee.WebSite = dr["WebSite"].ToString();
                ee.ServiceProtocol = dr["ServiceProtocol"].ToString();
                ee.Deleted = Int32.Parse(dr["Deleted"].ToString());
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
            DataTable dt;
            //2008-05-01 添加SQL缓存依赖的处理方式,减少数据库连接次数
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1"
                && (CommonDP.GetConfigValue("Other", "DataLimit") == null || CommonDP.GetConfigValue("Other", "DataLimit") == "0"))
            {
                dt = CommSqlCacheHelper.GetDataTableFromCache("mastcustomer");

                DataTable dtTemp = dt.Clone();
                //注意            *******      SWHERE 格式的合法性    ******
                dt.DefaultView.RowFilter = " deleted = 0 " + sWhere;

                dtTemp.Rows.Clear();
                if (sOrder != string.Empty)
                    dt.DefaultView.Sort = "sortid";
                foreach (DataRowView dvr in dt.DefaultView)
                {
                    dtTemp.Rows.Add(dvr.Row.ItemArray);

                }
                return dtTemp;
            }
            else
            {
                string strSQL = string.Empty;
                OracleConnection cn = ConfigTool.GetConnection();
                strSQL = "SELECT * FROM Br_MastCustomer Where 1=1 And Deleted=0 ";
                strSQL += sWhere;
                if (System.Web.HttpContext.Current.Session["MastLimitList"] != null)
                {
                    strSQL = strSQL + " and ID in (" + System.Web.HttpContext.Current.Session["MastLimitList"].ToString() + ")";
                }
                strSQL += sOrder;
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                ConfigTool.CloseConnection(cn);
                return dt;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="rowcount"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sWhere, int pagesize, int pageindex, ref int rowcount)
        {
            string strWhere = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strWhere += " Deleted=0 " + sWhere;
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "Br_MastCustomer", "*", " ORDER BY ID DESC", pagesize, pageindex, strWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="rowcount"></param>
        /// <returns></returns>
        public DataTable GetDataTableManager(string sWhere, string sOrder, int pagesize, int pageindex, ref int rowcount)
        {
            DataTable dt;
            //2008-05-01 添加SQL缓存依赖的处理方式,减少数据库连接次数
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                dt = CommSqlCacheHelper.GetDataTableFromCache("mastcustomer");

                DataTable dtTemp = dt.Clone();
                //注意            *******      SWHERE 格式的合法性    ******
                dt.DefaultView.RowFilter = " deleted = 0 " + sWhere;

                dtTemp.Rows.Clear();
                if (sOrder != string.Empty)
                    dt.DefaultView.Sort = "sortid";
                foreach (DataRowView dvr in dt.DefaultView)
                {
                    dtTemp.Rows.Add(dvr.Row.ItemArray);

                }
                return dtTemp;
            }
            else
            {
                dt = GetDataTable(sWhere, pagesize, pageindex, ref rowcount);
                return dt;
            }
        }
        #endregion

        #region 获取服务单位名称
        /// <summary>
        /// 获取服务单位名称
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns></returns>
        public static string GetMastCustName(long lngID)
        {
            string strSQL = "";
            string strName = "";
            //2008-06-09 添加SQL缓存依赖的处理方式,减少数据库连接次数
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                DataTable dt = CommSqlCacheHelper.GetDataTableFromCache("mastcustomer");
                if (dt != null)
                {
                    DataRow[] drs = dt.Select("ID = " + lngID.ToString());

                    if (drs.Length > 0)
                    {
                        strName = drs[0]["ShortName"].ToString();
                    }
                }
            }
            else
            {

                OracleDataReader dr;


                OracleConnection cn = ConfigTool.GetConnection();

                try
                {
                    strSQL = "SELECT ShortName FROM Br_MastCustomer Where ID = " + lngID.ToString();
                    dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                    while (dr.Read())
                    {
                        strName = dr.GetString(0);
                    }
                    dr.Close();                    
                }
                finally { ConfigTool.CloseConnection(cn); }
            }

            return strName;
        }

        #endregion

        #region InsertRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pBr_MastCustomerDP></param>
        public void InsertRecorded(Br_MastCustomerDP pBr_MastCustomerDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            string strID = "0";
            try
            {
                strID = EpowerGlobal.EPGlobal.GetNextID("Br_MastCustomerID").ToString();
                pBr_MastCustomerDP.ID = decimal.Parse(strID);
                strSQL = @"INSERT INTO Br_MastCustomer(
									ID,
									ShortName,
									FullName,
									Address,
									EnterpriseType,
									EnterpriseTypeName,
									CustomerType,
									CustomerTypeName,
									CustomCode,
									Tel1,
									LinkMan1,
									Fax1,
									WebSite,
									ServiceProtocol,
									Deleted,
									RegUserID,
									RegUserName,
									RegTime
					)
					VALUES( " +
                            strID.ToString() + "," +
                            StringTool.SqlQ(pBr_MastCustomerDP.ShortName) + "," +
                            StringTool.SqlQ(pBr_MastCustomerDP.FullName) + "," +
                            StringTool.SqlQ(pBr_MastCustomerDP.Address) + "," +
                            pBr_MastCustomerDP.EnterpriseType.ToString() + "," +
                            StringTool.SqlQ(pBr_MastCustomerDP.EnterpriseTypeName) + "," +
                            pBr_MastCustomerDP.CustomerType.ToString() + "," +
                            StringTool.SqlQ(pBr_MastCustomerDP.CustomerTypeName) + "," +
                            StringTool.SqlQ(pBr_MastCustomerDP.CustomCode) + "," +
                            StringTool.SqlQ(pBr_MastCustomerDP.Tel1) + "," +
                            StringTool.SqlQ(pBr_MastCustomerDP.LinkMan1) + "," +
                            StringTool.SqlQ(pBr_MastCustomerDP.Fax1) + "," +
                            StringTool.SqlQ(pBr_MastCustomerDP.WebSite) + "," +
                            StringTool.SqlQ(pBr_MastCustomerDP.ServiceProtocol) + "," +
                            pBr_MastCustomerDP.Deleted.ToString() + "," +
                            pBr_MastCustomerDP.RegUserID.ToString() + "," +
                            StringTool.SqlQ(pBr_MastCustomerDP.RegUserName) + "," +
                            (pBr_MastCustomerDP.RegTime == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(pBr_MastCustomerDP.RegTime.ToString()) + ",'yyyy-MM-dd HH24:mi:ss')") +
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
        /// <param name=pBr_MastCustomerDP></param>
        public void UpdateRecorded(Br_MastCustomerDP pBr_MastCustomerDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE Br_MastCustomer Set " +
                                                        " ShortName = " + StringTool.SqlQ(pBr_MastCustomerDP.ShortName) + "," +
                            " FullName = " + StringTool.SqlQ(pBr_MastCustomerDP.FullName) + "," +
                            " Address = " + StringTool.SqlQ(pBr_MastCustomerDP.Address) + "," +
                            " EnterpriseType = " + pBr_MastCustomerDP.EnterpriseType.ToString() + "," +
                            " EnterpriseTypeName = " + StringTool.SqlQ(pBr_MastCustomerDP.EnterpriseTypeName) + "," +
                            " CustomerType = " + pBr_MastCustomerDP.CustomerType.ToString() + "," +
                            " CustomerTypeName = " + StringTool.SqlQ(pBr_MastCustomerDP.CustomerTypeName) + "," +
                            " CustomCode = " + StringTool.SqlQ(pBr_MastCustomerDP.CustomCode) + "," +
                            " Tel1 = " + StringTool.SqlQ(pBr_MastCustomerDP.Tel1) + "," +
                            " LinkMan1 = " + StringTool.SqlQ(pBr_MastCustomerDP.LinkMan1) + "," +
                            " Fax1 = " + StringTool.SqlQ(pBr_MastCustomerDP.Fax1) + "," +
                            " WebSite = " + StringTool.SqlQ(pBr_MastCustomerDP.WebSite) + "," +
                            " ServiceProtocol = " + StringTool.SqlQ(pBr_MastCustomerDP.ServiceProtocol) + "," +
                            " Deleted = " + pBr_MastCustomerDP.Deleted.ToString() + "," +
                            " RegUserID = " + pBr_MastCustomerDP.RegUserID.ToString() + "," +
                            " RegUserName = " + StringTool.SqlQ(pBr_MastCustomerDP.RegUserName) + "," +
                            " RegTime = " + (pBr_MastCustomerDP.RegTime == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(pBr_MastCustomerDP.RegTime.ToString()) + ",'yyyy-MM-dd HH24:mi:ss')") +
                                " WHERE ID = " + pBr_MastCustomerDP.ID.ToString();

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
                string strSQL = "Update Br_MastCustomer Set Deleted=1  WHERE ID =" + lngID.ToString();
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

        #region 返回单位的所有服务

        /// <summary>
        /// 返回单位的所有服务
        /// </summary>
        /// <param name="MasterID"></param>
        /// <returns></returns>
        public static DataTable GetIssueByMaster(long MasterID)
        {
            DataTable dt = null;
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT Br_ECustomer.MastCustID,Br_MastCustOmer.ShortName as MasterName," +
                     " Br_MastCustOmer.FullName as MasterFullName,Br_ECustomer.ShortName AS CustName," +
                     " Cst_Issues.FlowID,Cst_Issues.CustID,Cst_Issues.SjwxrID " +
                     " FROM Cst_Issues inner join Br_ECustomer ON Cst_Issues.CustID = Br_ECustomer.ID  " +
                     " INNER JOIN Br_MastCustOmer ON Br_ECustomer.MastCustID = Br_MastCustOmer.ID " +
                     " WHERE Br_MastCustOmer.ID = " + MasterID.ToString();
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        public static DataTable GetIssueByMasterName(string MasterName)
        {
            DataTable dt = null;
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM Cst_Issues  WHERE MastCust = " + StringTool.SqlQ(MasterName.ToString());
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        #endregion

        #region 返回单位的所有服务

        /// <summary>
        /// 返回单位的所有服务
        /// </summary>
        /// <param name="CustID"></param>
        /// <returns></returns>
        public static DataTable GetIssueByCust(long CustID)
        {
            DataTable dt = null;
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT Br_ECustomer.MastCustID,Br_MastCustOmer.ShortName as MasterName," +
                     " Br_MastCustOmer.FullName as MasterFullName,Br_ECustomer.ShortName AS CustName," +
                     " Cst_Issues.FlowID,Cst_Issues.SjwxrID,Cst_Issues.CustID,Cst_Issues.SjwxrID" +
                     " FROM Cst_Issues inner join Br_ECustomer ON Cst_Issues.CustID = Br_ECustomer.ID  " +
                     " INNER JOIN Br_MastCustOmer ON Br_ECustomer.MastCustID = Br_MastCustOmer.ID " +
                     " WHERE Br_ECustomer.ID = " + CustID.ToString();
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        public static DataTable GetIssueByCustName(string CustName)
        {
            DataTable dt = null;
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM Cst_Issues  WHERE MastCust = " + StringTool.SqlQ(CustName.ToString());
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }


        #endregion
    }
}

