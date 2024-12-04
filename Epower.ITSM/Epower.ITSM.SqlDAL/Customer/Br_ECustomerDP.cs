using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using EpowerGlobal;
using Epower.ITSM.Base;
namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// Br_ECustomerDP 的摘要说明。
    /// </summary>
    public class Br_ECustomerDP
    {
        public Br_ECustomerDP()
        {

        }

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

        #region MastCustID
        /// <summary>
        ///
        /// </summary>
        private Decimal mMastCustID = 0;
        public Decimal MastCustID
        {
            get { return mMastCustID; }
            set { mMastCustID = value; }
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

        #region jobID
        /// <summary>
        ///
        /// </summary>
        private Decimal mJobID = 0;
        public Decimal jobID
        {
            get { return mJobID; }
            set { mJobID = value; }
        }
        #endregion

        #region job
        /// <summary>
        /// 
        /// </summary>
        private String mJob = string.Empty;
        public String Job
        {
            get { return mJob; }
            set { mJob = value; }
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

        #region Mobile
        /// <summary>
        ///  手机号码
        /// </summary>
        private String mMobile = string.Empty;
        public String Mobile
        {
            get { return mMobile; }
            set { mMobile = value; }
        }
        #endregion

        #region Email
        /// <summary>
        ///
        /// </summary>
        private String mEmail = string.Empty;
        public String Email
        {
            get { return mEmail; }
            set { mEmail = value; }
        }
        #endregion

        #region Rights
        /// <summary>
        ///
        /// </summary>
        private String mRights = string.Empty;
        public String Rights
        {
            get { return mRights; }
            set { mRights = value; }
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

        #region UserID
        /// <summary>
        ///
        /// </summary>
        private Decimal mUserID = 0;
        public Decimal UserID
        {
            get { return mUserID; }
            set { mUserID = value; }
        }
        #endregion

        #region CustDeptName
        /// <summary>
        ///
        /// </summary>
        private String mCustDeptName = string.Empty;
        public String CustDeptName
        {
            get { return mCustDeptName; }
            set { mCustDeptName = value; }
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

        #region MastCustName
        /// <summary>
        ///
        /// </summary>
        private String mMastCustName = string.Empty;
        public String MastCustName
        {
            get { return mMastCustName; }
            set { mMastCustName = value; }
        }
        #endregion

        #region UpdateTime
        /// <summary>
        ///
        /// </summary>
        private DateTime mUpdateTime = DateTime.MinValue;
        public DateTime UpdateTime
        {
            get { return mUpdateTime; }
            set { mUpdateTime = value; }
        }
        #endregion

        #region SchemaValue

        string mSchemaValue = string.Empty;
        public String SchemaValue
        {
            get { return mSchemaValue; }
            set { mSchemaValue = value; }
        }

        #endregion

        #region CustAreaID
        /// <summary>
        ///
        /// </summary>
        private Decimal mCustAreaID = 0;
        public Decimal CustAreaID
        {
            get { return mCustAreaID; }
            set { mCustAreaID = value; }
        }
        #endregion

        #region CustArea
        /// <summary>
        /// 
        /// </summary>
        private String mCustArea = string.Empty;
        public String CustArea
        {
            get { return mCustArea; }
            set { mCustArea = value; }
        }
        #endregion


        #region Changesource
        /// <summary>
        ///
        /// </summary>
        private String mchangesource = string.Empty;
        public String Changesource
        {
            get { return mchangesource; }
            set { mchangesource = value; }
        }
        #endregion

        #region Businesapprover
        /// <summary>
        ///
        /// </summary>
        private String mbusinesapprover = string.Empty;
        public String Businesapprover
        {
            get { return mbusinesapprover; }
            set { mbusinesapprover = value; }
        }
        #endregion

        #endregion

        #region GetReCorded
        /// <summary>
        /// GetReCorded
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>Br_ECustomerDP</returns>
        public Br_ECustomerDP GetReCorded(long lngID)
        {
            Br_ECustomerDP ee = new Br_ECustomerDP();
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = null;

            try
            {
                strSQL = "SELECT * FROM Br_ECustomer WHERE ID = " + lngID.ToString();
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);                
            }
            finally { ConfigTool.CloseConnection(cn); }

            foreach (DataRow dr in dt.Rows)
            {
                ee.ID = Decimal.Parse(dr["ID"].ToString());
                ee.MastCustID = Decimal.Parse(dr["MastCustID"].ToString());
                ee.ShortName = dr["ShortName"].ToString();
                ee.FullName = dr["FullName"].ToString();
                ee.Address = dr["Address"].ToString();
                ee.CustomerType = Decimal.Parse(dr["CustomerType"].ToString());
                ee.CustomerTypeName = dr["CustomerTypeName"].ToString();
                ee.jobID = Decimal.Parse(dr["jobID"].ToString() == "" ? "0" : dr["jobID"].ToString());
                ee.Job = dr["job"].ToString();
                ee.CustomCode = dr["CustomCode"].ToString();
                ee.LinkMan1 = dr["LinkMan1"].ToString();
                ee.Tel1 = dr["Tel1"].ToString();
                ee.Email = dr["Email"].ToString();
                ee.Rights = dr["Rights"].ToString();
                ee.Remark = dr["Remark"].ToString();
                if (dr["UserID"].ToString() == string.Empty)
                    ee.UserID = 0;
                else
                    ee.UserID = Decimal.Parse(dr["UserID"].ToString());
                ee.CustDeptName = dr["CustDeptName"].ToString();
                ee.Deleted = Int32.Parse(dr["Deleted"].ToString());
                ee.RegUserID = Decimal.Parse(dr["RegUserID"].ToString());
                ee.RegUserName = dr["RegUserName"].ToString();
                ee.RegTime = dr["RegTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["RegTime"].ToString());
                ee.MastCustName = dr["MastCustName"].ToString();
                ee.UpdateTime = dr["UpdateTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["UpdateTime"].ToString());
                ee.SchemaValue = dr["SchemaValue"].ToString();
                ee.CustDeptName = dr["CustDeptName"].ToString();
                ee.Mobile = dr["Mobile"].ToString();
                ee.CustAreaID = Decimal.Parse(dr["CustAreaID"].ToString() == "" ? "0" : dr["CustAreaID"].ToString());
                ee.CustArea = dr["CustArea"].ToString();

            }
            return ee;
        }
        #endregion

        #region 根据用户ID得到用户相对于的客户信息
        /// <summary>
        /// 根据用户ID得到用户相对于的客户信息
        /// </summary>
        /// <param name="strUserID"></param>
        /// <returns></returns>
        public Br_ECustomerDP GetReCordedByUserID(string strUserID)
        {
            Br_ECustomerDP ee = new Br_ECustomerDP();
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = null;

            try
            {
                strSQL = "SELECT * FROM Br_ECustomer WHERE deleted = 0 and UserID = " + strUserID.ToString();
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);                
            }
            finally { ConfigTool.CloseConnection(cn); }

            foreach (DataRow dr in dt.Rows)
            {
                ee.ID = Decimal.Parse(dr["ID"].ToString());
                ee.MastCustID = Decimal.Parse(dr["MastCustID"].ToString());
                ee.ShortName = dr["ShortName"].ToString();
                ee.FullName = dr["FullName"].ToString();
                ee.Address = dr["Address"].ToString();
                ee.CustomerType = Decimal.Parse(dr["CustomerType"].ToString());
                ee.CustomerTypeName = dr["CustomerTypeName"].ToString();
                ee.jobID = Decimal.Parse(dr["jobID"].ToString() == "" ? "0" : dr["jobID"].ToString());
                ee.Job = dr["job"].ToString();
                ee.CustomCode = dr["CustomCode"].ToString();
                ee.LinkMan1 = dr["LinkMan1"].ToString();
                ee.Tel1 = dr["Tel1"].ToString();
                ee.Email = dr["Email"].ToString();
                ee.Rights = dr["Rights"].ToString();
                ee.Remark = dr["Remark"].ToString();

                if (dr["UserID"].ToString() == string.Empty)
                    ee.UserID = 0;
                else
                    ee.UserID = Decimal.Parse(dr["UserID"].ToString());
                ee.CustDeptName = dr["CustDeptName"].ToString();
                ee.Deleted = Int32.Parse(dr["Deleted"].ToString());
                ee.RegUserID = Decimal.Parse(dr["RegUserID"].ToString());
                ee.RegUserName = dr["RegUserName"].ToString();
                ee.RegTime = dr["RegTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["RegTime"].ToString());
                ee.MastCustName = dr["MastCustName"].ToString();
                ee.UpdateTime = dr["UpdateTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["UpdateTime"].ToString());
                ee.SchemaValue = dr["SchemaValue"].ToString();
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
                strSQL = "SELECT * FROM Br_ECustomer Where 1=1 And Deleted=0 ";
                strSQL += sWhere;
                strSQL += sOrder;
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                
                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="rowcount"></param>
        /// <param name="sOrder"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sWhere, int pagesize, int pageindex, ref int rowcount, string sOrder)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "Br_ECustomer a,Br_MastCustomer b ", "a.*,b.ShortName MShortName", sOrder, pagesize, pageindex, sWhere, ref rowcount);                
                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion

        #region 获取客户Json对象
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <returns></returns>
        public static string GetJson(string sWhere, string sOrder)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT * FROM Br_ECustomer Where 1=1 And Deleted=0 ";
                strSQL += sWhere;
                strSQL += sOrder;
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);                

                Json json = new Json(dt);
                return "{record:" + json.ToJson() + "}";
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion

        #region GetDataTableAjax
        /// <summary>
        /// GetDataTableAjax
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTableAjax(string sWhere, string sOrder)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT * FROM V_Br_ECustomerAjax Where 1=1 And Deleted=0 ";
                strSQL += sWhere;
                strSQL += sOrder;
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                
                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取服务id是否在客户中已经用到
        /// </summary>   
        /// <returns></returns>
        public static bool GetmastCustId(string mastCustId)
        {
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                string strSql = "select mastCustId From Br_ECustomer where mastCustId =" + StringTool.SqlQ(mastCustId) + " and deleted =0";
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSql);
                
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            finally { ConfigTool.CloseConnection(cn); }

        }
        /// <summary>
        /// 判断用户名称是否存在
        /// </summary>
        /// <param name="custName"></param>
        /// <param name="sID"></param>
        /// <returns></returns>
        public static bool getCustName(string custName,string sID)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                string sWhere = string.Empty;
                if (sID != string.Empty)
                {
                    sWhere += " AND ID<>" + long.Parse(sID).ToString();
                }
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, "select * from Br_ECustomer where ShortName =" + StringTool.SqlQ(custName) + " and deleted =0 " + sWhere);
                
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            finally { ConfigTool.CloseConnection(cn); }
        }

        /// <summary>
        /// 检查用户代码是否重复
        /// </summary>
        /// <param name="CustomCode"></param>
        /// <returns></returns>
        public static bool CheckCustomCode(string CustomCode,string sID)
        {
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                string sWhere = string.Empty;
                if (sID != string.Empty)
                {
                    sWhere += " AND ID<>" + long.Parse(sID).ToString();
                }
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, "select * from Br_ECustomer where CustomCode =" + StringTool.SqlQ(CustomCode) + " and deleted =0 " + sWhere);
                
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            finally { ConfigTool.CloseConnection(cn); }
        }

        #endregion

        #region InsertRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pBr_ECustomerDP></param>
        public void InsertRecorded(Br_ECustomerDP pBr_ECustomerDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            string strID = "0";
            try
            {
                if (Br_ECustomerDP.CheckCustomCode(pBr_ECustomerDP.CustomCode, string.Empty) != true)
                {
                    strID = EpowerGlobal.EPGlobal.GetNextID("ECustomer_ID").ToString();
                    pBr_ECustomerDP.ID = decimal.Parse(strID);
                    strSQL = @"INSERT INTO Br_ECustomer(
									ID,
									MastCustID,
									ShortName,
									FullName,
									Address,
									CustomerType,
									CustomerTypeName,
                                    jobID,
                                    job,
									CustomCode,
									LinkMan1,
									Tel1,
                                    Email,
									Rights,
									Remark,
                                    UserID,
                                    CustDeptName,
									Deleted,
									RegUserID,
									RegUserName,
									RegTime,
                                    MastCustName,
                                    SchemaValue,
                                    Mobile,
                                    CustAreaID,
                                    CustArea,
                                    UpdateTime
					)
					VALUES( " +
                                strID.ToString() + "," +
                                pBr_ECustomerDP.MastCustID.ToString() + "," +
                                StringTool.SqlQ(pBr_ECustomerDP.ShortName) + "," +
                                StringTool.SqlQ(pBr_ECustomerDP.FullName) + "," +
                                StringTool.SqlQ(pBr_ECustomerDP.Address) + "," +
                                pBr_ECustomerDP.CustomerType.ToString() + "," +
                                StringTool.SqlQ(pBr_ECustomerDP.CustomerTypeName) + "," +
                                pBr_ECustomerDP.jobID.ToString() + "," +
                                StringTool.SqlQ(pBr_ECustomerDP.Job) + "," +
                                StringTool.SqlQ(pBr_ECustomerDP.CustomCode) + "," +
                                StringTool.SqlQ(pBr_ECustomerDP.LinkMan1) + "," +
                                StringTool.SqlQ(pBr_ECustomerDP.Tel1) + "," +
                                StringTool.SqlQ(pBr_ECustomerDP.Email) + "," +
                                StringTool.SqlQ(pBr_ECustomerDP.Rights) + "," +
                                StringTool.SqlQ(pBr_ECustomerDP.Remark) + "," +
                                pBr_ECustomerDP.UserID.ToString() + "," +
                                StringTool.SqlQ(pBr_ECustomerDP.CustDeptName) + "," +
                                pBr_ECustomerDP.Deleted.ToString() + "," +
                                pBr_ECustomerDP.RegUserID.ToString() + "," +
                                StringTool.SqlQ(pBr_ECustomerDP.RegUserName) + "," +
                                (pBr_ECustomerDP.RegTime == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(pBr_ECustomerDP.RegTime.ToString()) + ",'yyyy-MM-dd HH24:mi:ss')") + "," +
                                StringTool.SqlQ(pBr_ECustomerDP.MastCustName) + "," +
                                StringTool.SqlQ(pBr_ECustomerDP.SchemaValue) + "," +
                                StringTool.SqlQ(pBr_ECustomerDP.Mobile) + "," +
                                pBr_ECustomerDP.CustAreaID + "," +
                                StringTool.SqlQ(pBr_ECustomerDP.CustArea) + "," +
                                (pBr_ECustomerDP.UpdateTime == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(pBr_ECustomerDP.UpdateTime.ToString()) + ",'yyyy-MM-dd HH24:mi:ss')") +
                        ")";
                }
                else
                {
                    strSQL = @"update Br_ECustomer set							
									MastCustID=" + pBr_ECustomerDP.MastCustID.ToString() + "," +
                                    "FullName=" + StringTool.SqlQ(pBr_ECustomerDP.FullName) + "," +
                                    "Address=" + StringTool.SqlQ(pBr_ECustomerDP.Address) + "," +
                                    "CustomerType=" + pBr_ECustomerDP.CustomerType.ToString() + "," +
                                    "CustomerTypeName=" + StringTool.SqlQ(pBr_ECustomerDP.CustomerTypeName) + "," +
                                    "jobID=" + pBr_ECustomerDP.jobID.ToString() + "," +
                                    "job=" + StringTool.SqlQ(pBr_ECustomerDP.Job) + "," +
                                    "CustomCode=" + StringTool.SqlQ(pBr_ECustomerDP.CustomCode) + "," +
                                    "LinkMan1=" + StringTool.SqlQ(pBr_ECustomerDP.LinkMan1) + "," +
                                    "Tel1=" + StringTool.SqlQ(pBr_ECustomerDP.Tel1) + "," +
                                    "Email=" + StringTool.SqlQ(pBr_ECustomerDP.Email) + "," +
                                    "Rights=" + StringTool.SqlQ(pBr_ECustomerDP.Rights) + "," +
                                    "Remark=" + StringTool.SqlQ(pBr_ECustomerDP.Remark) + "," +
                                    "UserID=" + pBr_ECustomerDP.UserID.ToString() + "," +
                                    "CustDeptName=" + StringTool.SqlQ(pBr_ECustomerDP.CustDeptName) + "," +
                                    "Deleted=" + pBr_ECustomerDP.Deleted.ToString() + "," +
                                    "RegUserID=" + pBr_ECustomerDP.RegUserID.ToString() + "," +
                                    "RegUserName=" + StringTool.SqlQ(pBr_ECustomerDP.RegUserName) + "," +
                                    "RegTime=" + (pBr_ECustomerDP.RegTime == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(pBr_ECustomerDP.RegTime.ToString()) + ",'yyyy-MM-dd HH24:mi:ss')") + "," +
                                    "MastCustName=" + StringTool.SqlQ(pBr_ECustomerDP.MastCustName) + "," +
                                    "SchemaValue=" + StringTool.SqlQ(pBr_ECustomerDP.SchemaValue) + "," +
                                    "Mobile=" + StringTool.SqlQ(pBr_ECustomerDP.Mobile) + "," +
                                    "CustAreaID=" + pBr_ECustomerDP.CustAreaID + "," +
                                    "CustArea=" + StringTool.SqlQ(pBr_ECustomerDP.CustArea) + "," +
                                    "UpdateTime=" + (pBr_ECustomerDP.UpdateTime == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(pBr_ECustomerDP.UpdateTime.ToString()) + ",'yyyy-MM-dd HH24:mi:ss')") +
                               " where ShortName=" + StringTool.SqlQ(pBr_ECustomerDP.ShortName);
                }

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
        /// <param name=pBr_ECustomerDP></param>
        public void UpdateRecorded(Br_ECustomerDP pBr_ECustomerDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE Br_ECustomer Set " +
                                                        " MastCustID = " + pBr_ECustomerDP.MastCustID.ToString() + "," +
                            " ShortName = " + StringTool.SqlQ(pBr_ECustomerDP.ShortName) + "," +
                            " FullName = " + StringTool.SqlQ(pBr_ECustomerDP.FullName) + "," +
                            " Address = " + StringTool.SqlQ(pBr_ECustomerDP.Address) + "," +
                            " CustomerType = " + pBr_ECustomerDP.CustomerType.ToString() + "," +
                            " CustomerTypeName = " + StringTool.SqlQ(pBr_ECustomerDP.CustomerTypeName) + "," +
                            " jobID=" + pBr_ECustomerDP.jobID.ToString() + "," +
                            " job = " + StringTool.SqlQ(pBr_ECustomerDP.Job) + "," +
                            " CustomCode = " + StringTool.SqlQ(pBr_ECustomerDP.CustomCode) + "," +
                            " LinkMan1 = " + StringTool.SqlQ(pBr_ECustomerDP.LinkMan1) + "," +
                            " Tel1 = " + StringTool.SqlQ(pBr_ECustomerDP.Tel1) + "," +
                            " Email = " + StringTool.SqlQ(pBr_ECustomerDP.Email) + "," +
                            " Rights = " + StringTool.SqlQ(pBr_ECustomerDP.Rights) + "," +
                            " Remark = " + StringTool.SqlQ(pBr_ECustomerDP.Remark) + "," +
                            " UserID = " + pBr_ECustomerDP.UserID.ToString() + "," +
                            " CustDeptName = " + StringTool.SqlQ(pBr_ECustomerDP.CustDeptName) + "," +
                            " Deleted = " + pBr_ECustomerDP.Deleted.ToString() + "," +
                            " RegUserID = " + pBr_ECustomerDP.RegUserID.ToString() + "," +
                            " RegUserName = " + StringTool.SqlQ(pBr_ECustomerDP.RegUserName) + "," +
                            " RegTime = " + (pBr_ECustomerDP.RegTime == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(pBr_ECustomerDP.RegTime.ToString()) + ",'yyyy-MM-dd HH24:mi:ss')") + "," +
                            " MastCustName = " + StringTool.SqlQ(pBr_ECustomerDP.MastCustName) + "," +
                            " SchemaValue = " + StringTool.SqlQ(pBr_ECustomerDP.SchemaValue) + "," +
                            " Mobile=" + StringTool.SqlQ(pBr_ECustomerDP.Mobile) + "," +
                            " CustAreaID=" + pBr_ECustomerDP.CustAreaID + "," +
                            " CustArea=" + StringTool.SqlQ(pBr_ECustomerDP.CustArea) + "," +
                            " UpdateTime = " + (pBr_ECustomerDP.UpdateTime == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(pBr_ECustomerDP.UpdateTime.ToString()) + ",'yyyy-MM-dd HH24:mi:ss')") +
                                " WHERE ID = " + pBr_ECustomerDP.ID.ToString();

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
                string strSQL = "Update Br_ECustomer Set Deleted=1,UpdateTime=sysdate WHERE ID =" + lngID.ToString();
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

        #region 内部用户相关方法

        /// <summary>
        /// 获取用户对应的客户ID 和 名称
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <returns></returns>
        public string[] GetRefUserIDAndName(long lngUserID)
        {
            string[] ret = { "0", "" };
            string strSQL = "";
            OracleDataReader dr;
            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");
            try
            { //只取1个
                strSQL = "SELECT ID,ShortName FROM Br_ECustomer WHERE ROWNUM<=1 And Deleted=0 And UserID = " + lngUserID.ToString();
                dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                while (dr.Read())
                {
                    ret[0] = dr.GetDecimal(0).ToString();
                    ret[1] = dr.GetString(1);
                }
                dr.Close();            

                return ret;
            }
            finally { ConfigTool.CloseConnection(cn); }

        }

        #endregion        

        #region GetCustomerServic  取得客户资料和服务单位
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <returns>DataTable</returns>
        public DataTable GetCustomerServic(string sWhere, string sOrder)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT E.*,M.ShortName MName,M.ShortName MastCust FROM Br_ECustomer E,Br_MastCustomer M Where 1=1 And E.MastCustID=M.ID And E.Deleted=0 ";
            strSQL += sWhere;
            if (CommonDP.GetConfigValue("Other", "DataLimit") != null && CommonDP.GetConfigValue("Other", "DataLimit") == "1")  //是否限制
            {
                if (System.Web.HttpContext.Current.Session["MastLimitList"] != null)
                {
                    strSQL = strSQL + " and M.ID in (" + System.Web.HttpContext.Current.Session["MastLimitList"].ToString() + ")";
                }
            }
            strSQL += sOrder;
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);                
                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion
    }
}
