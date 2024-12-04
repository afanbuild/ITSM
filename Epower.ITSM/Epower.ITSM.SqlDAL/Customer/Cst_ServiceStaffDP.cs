/****************************************************************************
 * 
 * description:服务人员处理类
 * 
 * 
 * 
 * Create by:zhumingchun
 * Create Date:2007-11-12
 * *************************************************************************/
using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;
using System.Text;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// 
    /// </summary>
    public class Cst_ServiceStaffDP
    {
        /// <summary>
        /// 
        /// </summary>
        public Cst_ServiceStaffDP()
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
        private String mName;
        public String Name
        {
            get { return mName; }
            set { mName = value; }
        }
        #endregion

        #region BlongDeptID
        /// <summary>
        ///
        /// </summary>
        private Decimal mBlongDeptID;
        public Decimal BlongDeptID
        {
            get { return mBlongDeptID; }
            set { mBlongDeptID = value; }
        }
        #endregion

        #region BlongDeptName
        /// <summary>
        ///
        /// </summary>
        private String mBlongDeptName;
        public String BlongDeptName
        {
            get { return mBlongDeptName; }
            set { mBlongDeptName = value; }
        }
        #endregion

        #region OrderIndex
        /// <summary>
        ///
        /// </summary>
        private Int32 mOrderIndex;
        public Int32 OrderIndex
        {
            get { return mOrderIndex; }
            set { mOrderIndex = value; }
        }
        #endregion

        #region Remark
        /// <summary>
        ///
        /// </summary>
        private String mRemark;
        public String Remark
        {
            get { return mRemark; }
            set { mRemark = value; }
        }
        #endregion

        #region Deleted
        /// <summary>
        ///
        /// </summary>
        private Decimal mDeleted;
        public Decimal Deleted
        {
            get { return mDeleted; }
            set { mDeleted = value; }
        }
        #endregion

        #region RegUserID
        /// <summary>
        ///
        /// </summary>
        private Decimal mRegUserID;
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
        private String mRegUserName;
        public String RegUserName
        {
            get { return mRegUserName; }
            set { mRegUserName = value; }
        }
        #endregion

        #region RegDeptID
        /// <summary>
        ///
        /// </summary>
        private Decimal mRegDeptID;
        public Decimal RegDeptID
        {
            get { return mRegDeptID; }
            set { mRegDeptID = value; }
        }
        #endregion

        #region RegDeptName
        /// <summary>
        ///
        /// </summary>
        private String mRegDeptName;
        public String RegDeptName
        {
            get { return mRegDeptName; }
            set { mRegDeptName = value; }
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
        private String mUserName;
        public String UserName
        {
            get { return mUserName; }
            set { mUserName = value; }
        }
        #endregion

        #region Faculty
        /// <summary>
        ///
        /// </summary>
        private String mFaculty;
        public String Faculty
        {
            get { return mFaculty; }
            set { mFaculty = value; }
        }
        #endregion

        #region JoinDate
        /// <summary>
        ///
        /// </summary>
        private DateTime mJoinDate;
        public DateTime JoinDate
        {
            get { return mJoinDate; }
            set { mJoinDate = value; }
        }
        #endregion

        #region WORKCATEID
        /// <summary>
        ///
        /// </summary>
        private Decimal mWORKCATEID;
        public Decimal WORKCATEID
        {
            get { return mWORKCATEID; }
            set { mWORKCATEID = value; }
        }
        #endregion

        #region SCHEDULESID
        /// <summary>
        ///
        /// </summary>
        private Decimal mSCHEDULESID;
        public Decimal SCHEDULESID
        {
            get { return mSCHEDULESID; }
            set { mSCHEDULESID = value; }
        }
        #endregion

        #region TRID
        /// <summary>
        ///
        /// </summary>
        private Decimal mTRID;
        public Decimal TRID
        {
            get { return mTRID; }
            set { mTRID = value; }
        }
        #endregion

        #region FIRSTFLAG
        /// <summary>
        ///
        /// </summary>
        private Decimal mFIRSTFLAG;
        public Decimal FIRSTFLAG
        {
            get { return mFIRSTFLAG; }
            set { mFIRSTFLAG = value; }
        }
        #endregion

        #region RESTVALUE
        /// <summary>
        ///
        /// </summary>
        private string mRESTVALUE;
        public string RESTVALUE
        {
            get { return mRESTVALUE; }
            set { mRESTVALUE = value; }
        }
        #endregion

        #region RESTNAME
        /// <summary>
        ///
        /// </summary>
        private string mRESTNAME;
        public string RESTNAME
        {
            get { return mRESTNAME; }
            set { mRESTNAME = value; }
        }
        #endregion

        #region STATUS
        /// <summary>
        ///
        /// </summary>
        private Decimal mSTATUS;
        public Decimal STATUS
        {
            get { return mSTATUS; }
            set { mSTATUS = value; }
        }
        #endregion

        #endregion

        #region GetReCorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>Cst_ServiceStaffDP</returns>
        public Cst_ServiceStaffDP GetReCorded(long lngID)
        {
            Cst_ServiceStaffDP ee = new Cst_ServiceStaffDP();
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM Cst_ServiceStaff WHERE ID = " + lngID.ToString();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            foreach (DataRow dr in dt.Rows)
            {
                ee.ID = Decimal.Parse(dr["ID"].ToString());
                ee.Name = dr["Name"].ToString();
                ee.BlongDeptID = Decimal.Parse(dr["BlongDeptID"].ToString());
                ee.BlongDeptName = dr["BlongDeptName"].ToString();
                ee.OrderIndex = Int32.Parse(dr["OrderIndex"].ToString());
                ee.Remark = dr["Remark"].ToString();
                ee.Deleted = Decimal.Parse(dr["Deleted"].ToString());
                ee.RegUserID = Decimal.Parse(dr["RegUserID"].ToString());
                ee.RegUserName = dr["RegUserName"].ToString();
                ee.RegDeptID = Decimal.Parse(dr["RegDeptID"].ToString());
                ee.RegDeptName = dr["RegDeptName"].ToString();
                ee.RegTime = DateTime.Parse(dr["RegTime"].ToString());
                ee.UserID = Decimal.Parse(dr["UserID"].ToString());
                ee.UserName = dr["UserName"].ToString();
                ee.Faculty = dr["Faculty"].ToString();
                ee.JoinDate = dr["JoinDate"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["JoinDate"].ToString());
                if (!string.IsNullOrEmpty(dr["WORKCATEID"].ToString()))
                {
                    ee.WORKCATEID = Decimal.Parse(dr["WORKCATEID"].ToString());
                }
                if (!string.IsNullOrEmpty(dr["SCHEDULESID"].ToString()))
                {
                    ee.SCHEDULESID = Decimal.Parse(dr["SCHEDULESID"].ToString());
                }
                if (!string.IsNullOrEmpty(dr["TRID"].ToString()))
                {
                    ee.TRID = Decimal.Parse(dr["TRID"].ToString());
                }
                if (!string.IsNullOrEmpty(dr["FIRSTFLAG"].ToString()))
                {
                    ee.FIRSTFLAG = Decimal.Parse(dr["FIRSTFLAG"].ToString());
                }
                 if (!string.IsNullOrEmpty(dr["RESTVALUE"].ToString()))
                {
                    ee.RESTVALUE = dr["RESTVALUE"].ToString();
                }
                 if (!string.IsNullOrEmpty(dr["RESTNAME"].ToString()))
                {
                    ee.RESTNAME = dr["RESTNAME"].ToString();
                }
                 if (!string.IsNullOrEmpty(dr["STATUS"].ToString()))
                {
                    ee.STATUS = Decimal.Parse(dr["STATUS"].ToString());
                }
              
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
            strSQL = "SELECT * FROM Cst_ServiceStaff Where 1=1 And Deleted=0 ";
            strSQL += sWhere;
            strSQL += sOrder;
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
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
        public DataTable GetDataTable(string sWhere, string sOrder, int pagesize, int pageindex, ref int rowcount)
        {
            string strWhere = "1=1 And Deleted=0" + sWhere;
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "Cst_ServiceStaff", "*", sOrder, pagesize, pageindex, strWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        public DataTable GetDataTable_RU(string sWhere, string sOrder, int pagesize, int pageindex, ref int rowcount)
        {
            string strWhere = "1=1 And Cst_ServiceStaff.Deleted=0" + sWhere;
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "Cst_RecommendRuleDetails,Cst_ServiceStaff", "Cst_ServiceStaff.*", sOrder, pagesize, pageindex, strWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            dt = ShowDataViewSource(dt);
            return dt;
        }
        private DataTable ShowDataViewSource(DataTable dt)
        {
            DataView myDataView = dt.DefaultView;
            string[] strComuns = { "ID","Name","BlongDeptID" ,"BlongDeptName","OrderIndex" ,"Remark" ,"Deleted" ,"RegUserID" ,
	"RegUserName" ,	"RegDeptID" ,	"RegDeptName" ,	"RegTime" ,	"UserID" ,	"UserName" ,	"Faculty" ,	"JoinDate"  };
            return myDataView.ToTable(true, strComuns);

        }

        #endregion

        #region InsertRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pCst_ServiceStaffDP></param>
        public void InsertRecorded(Cst_ServiceStaffDP pCst_ServiceStaffDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            string strID = "0";
            try
            {
                strID = EpowerGlobal.EPGlobal.GetNextID("Cst_ServiceStaffID").ToString();
                pCst_ServiceStaffDP.ID = decimal.Parse(strID);
                strSQL = @"INSERT INTO Cst_ServiceStaff(
									ID,
									Name,
									BlongDeptID,
									BlongDeptName,
									OrderIndex,
									Remark,
									Deleted,
									RegUserID,
									RegUserName,
									RegDeptID,
									RegDeptName,
									RegTime,
                                    UserID,
                                    UserName,
                                    Faculty,
                                    RESTNAME,
                                    RESTVALUE,
                                    JoinDate
					)
					VALUES( " +
                            strID.ToString() + "," +
                            StringTool.SqlQ(pCst_ServiceStaffDP.Name) + "," +
                            pCst_ServiceStaffDP.BlongDeptID.ToString() + "," +
                            StringTool.SqlQ(pCst_ServiceStaffDP.BlongDeptName) + "," +
                            pCst_ServiceStaffDP.OrderIndex.ToString() + "," +
                            StringTool.SqlQ(pCst_ServiceStaffDP.Remark) + "," +
                            pCst_ServiceStaffDP.Deleted.ToString() + "," +
                            pCst_ServiceStaffDP.RegUserID.ToString() + "," +
                            StringTool.SqlQ(pCst_ServiceStaffDP.RegUserName) + "," +
                            pCst_ServiceStaffDP.RegDeptID.ToString() + "," +
                            StringTool.SqlQ(pCst_ServiceStaffDP.RegDeptName) + "," +
                            "to_date(" + StringTool.SqlQ(pCst_ServiceStaffDP.RegTime.ToString()) + ",'yyyy-MM-dd HH24:mi:ss')," +
                            pCst_ServiceStaffDP.UserID.ToString() + "," +
                            StringTool.SqlQ(pCst_ServiceStaffDP.UserName) + "," +
                            StringTool.SqlQ(pCst_ServiceStaffDP.Faculty) + "," +
                            StringTool.SqlQ(pCst_ServiceStaffDP.RESTNAME) + "," +
                            StringTool.SqlQ(pCst_ServiceStaffDP.RESTVALUE) + "," +
                            (pCst_ServiceStaffDP.JoinDate == DateTime.MinValue ? "null" : "to_date(" + StringTool.SqlQ(pCst_ServiceStaffDP.JoinDate.ToString()) + ",'yyyy-MM-dd HH24:mi:ss')") +
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
        /// <param name=pCst_ServiceStaffDP></param>
        public void UpdateRecorded(Cst_ServiceStaffDP pCst_ServiceStaffDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE Cst_ServiceStaff Set " +
                                                        " Name = " + StringTool.SqlQ(pCst_ServiceStaffDP.Name) + "," +
                            " BlongDeptID = " + pCst_ServiceStaffDP.BlongDeptID.ToString() + "," +
                            " BlongDeptName = " + StringTool.SqlQ(pCst_ServiceStaffDP.BlongDeptName) + "," +
                            " OrderIndex = " + pCst_ServiceStaffDP.OrderIndex.ToString() + "," +
                            " Remark = " + StringTool.SqlQ(pCst_ServiceStaffDP.Remark) + "," +
                            " Deleted = " + pCst_ServiceStaffDP.Deleted.ToString() + "," +
                            " UserID = " + pCst_ServiceStaffDP.UserID.ToString() + "," +
                            " UserName = " + StringTool.SqlQ(pCst_ServiceStaffDP.UserName) + "," +
                            " Faculty = " + StringTool.SqlQ(pCst_ServiceStaffDP.Faculty) + "," +
                            " RESTNAME = " + StringTool.SqlQ(pCst_ServiceStaffDP.RESTNAME) + "," +
                            " RESTVALUE = " + StringTool.SqlQ(pCst_ServiceStaffDP.RESTVALUE) + "," +
                            " JoinDate =" + (pCst_ServiceStaffDP.JoinDate == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(pCst_ServiceStaffDP.JoinDate.ToString()) + ",'yyyy-MM-dd HH24:mi:ss')") +
                                " WHERE ID = " + pCst_ServiceStaffDP.ID.ToString();

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

        /// <summary>
        /// 排班信息修改
        /// </summary>
        /// <param name=pCst_ServiceStaffDP></param>
        public string UpdateRecordedSchedules(Cst_ServiceStaffDP pCst_ServiceStaffDP)
        {
            StringBuilder sbStrSQL = new StringBuilder();
            sbStrSQL.Append("UPDATE Cst_ServiceStaff Set ");
            sbStrSQL.Append(" WORKCATEID = ");
            sbStrSQL.Append(pCst_ServiceStaffDP.WORKCATEID.ToString());
            sbStrSQL.Append(",SCHEDULESID = ");
            sbStrSQL.Append(pCst_ServiceStaffDP.SCHEDULESID.ToString());
            sbStrSQL.Append(",TRID = ");
            sbStrSQL.Append(pCst_ServiceStaffDP.TRID.ToString());
            sbStrSQL.Append(",FIRSTFLAG = ");
            sbStrSQL.Append(pCst_ServiceStaffDP.FIRSTFLAG.ToString());
            sbStrSQL.Append(" WHERE ID = ");
            sbStrSQL.Append(pCst_ServiceStaffDP.ID.ToString());
            return sbStrSQL.ToString();
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
                string strSQL = "Update Cst_ServiceStaff Set Deleted=1  WHERE ID =" + lngID.ToString();
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

        #region GetDataByDeptID根据部门ID，取得服务人员
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DeptID"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetDataByDeptID(long DeptID)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT Name,ID userid FROM Cst_ServiceStaff Where 1=1 And Deleted=0 And BlongDeptID=" + DeptID.ToString() + " Order by OrderIndex";
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region GetReCordedByUserID根据系统登录用户ID，取得相应的资料
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <returns>Cst_ServiceStaffDP</returns>
        public static Cst_ServiceStaffDP GetReCordedByUserID(long lngUserID)
        {
            Cst_ServiceStaffDP ee = new Cst_ServiceStaffDP();
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM Cst_ServiceStaff WHERE UserID = " + lngUserID.ToString();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            foreach (DataRow dr in dt.Rows)
            {
                ee.ID = Decimal.Parse(dr["ID"].ToString());
                ee.Name = dr["Name"].ToString();
                ee.BlongDeptID = Decimal.Parse(dr["BlongDeptID"].ToString());
                ee.BlongDeptName = dr["BlongDeptName"].ToString();
                ee.OrderIndex = Int32.Parse(dr["OrderIndex"].ToString());
                ee.Remark = dr["Remark"].ToString();
                ee.Deleted = Decimal.Parse(dr["Deleted"].ToString());
                ee.RegUserID = Decimal.Parse(dr["RegUserID"].ToString());
                ee.RegUserName = dr["RegUserName"].ToString();
                ee.RegDeptID = Decimal.Parse(dr["RegDeptID"].ToString());
                ee.RegDeptName = dr["RegDeptName"].ToString();
                ee.RegTime = DateTime.Parse(dr["RegTime"].ToString());
                ee.UserID = Decimal.Parse(dr["UserID"].ToString());
                ee.UserName = dr["UserName"].ToString();
                ee.Faculty = dr["Faculty"].ToString();
                ee.JoinDate = DateTime.Parse(dr["JoinDate"].ToString());
            }
            return ee;
        }
        #endregion

        #region 根据流程ID，找到相应的执行人
        /// <summary>
        /// lngFlowID
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        public static DataTable GetServiceStaffList(long lngFlowID)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM Cst_ServiceStaffList Where FlowID=" + lngFlowID.ToString();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region CheckIsRepeat判断用户不能重复
        /// <summary>
        /// 判断用户不能重复
        /// </summary>
        /// <returns></returns>
        public static bool CheckIsRepeat(string lngID, long lngUserID)
        {
            string strSQL = "SELECT * FROM Cst_ServiceStaff WHERE 1=1" +
                " and deleted = " + (int)eO_Deleted.eNormal;

            if (lngUserID != 0)
            {
                strSQL += " and UserID=" + lngUserID.ToString();
            }
            else
            {
                strSQL += " and 1<>1";
            }
            if (lngID != string.Empty)
            {
                strSQL += " and ID<>" + (long.Parse(lngID)).ToString();
            }
            DataTable dt;
            OracleConnection cn = ConfigTool.GetConnection();
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);

            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        #endregion

        public DataTable GetNoSetWorkIssues()
        {
            string sql = "select * from cst_servicestaff s where s.deleted =0 and nvl(s.schedulesid,0)=0";

            return CommonDP.ExcuteSqlTable(sql);

        }
    }
}

