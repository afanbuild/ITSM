



/*******************************************************************
 *
 * Description
 * 
 * 
 * Create By  :Administrator
 * Create Date:2011年8月30日 星期二
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
    public class Br_OrderClassDP
    {
        /// <summary>
        /// 
        /// </summary>
        public Br_OrderClassDP()
        { }

        #region Property
        #region ID
        private Decimal mID;
        /// <summary>
        /// 标示ID
        /// </summary>
        public Decimal ID
        {
            get { return mID; }
            set { mID = value; }
        }
        #endregion

        #region StaffID
        private Decimal mStaffID;
        /// <summary>
        ///  值班人ID
        /// </summary>
        public Decimal StaffID
        {
            get { return mStaffID; }
            set { mStaffID = value; }
        }
        #endregion

        #region StaffName
        private String mStaffName = string.Empty;
        /// <summary>
        ///  值班人名称
        /// </summary>
        public String StaffName
        {
            get { return mStaffName; }
            set { mStaffName = value; }
        }
        #endregion

        #region DutyTime
        private DateTime mDutyTime = DateTime.MinValue;
        /// <summary>
        /// 值班时间
        /// </summary>
        public DateTime DutyTime
        {
            get { return mDutyTime; }
            set { mDutyTime = value; }
        }
        #endregion

        #region RegUserID
        private Decimal mRegUserID;
        /// <summary>
        /// 创建人ID
        /// </summary>
        public Decimal RegUserID
        {
            get { return mRegUserID; }
            set { mRegUserID = value; }
        }
        #endregion

        #region RegUserName
        private String mRegUserName = string.Empty;
        /// <summary>
        ///  创建人
        /// </summary>
        public String RegUserName
        {
            get { return mRegUserName; }
            set { mRegUserName = value; }
        }
        #endregion

        #region RegTime
        private DateTime mRegTime = DateTime.MinValue;
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime RegTime
        {
            get { return mRegTime; }
            set { mRegTime = value; }
        }
        #endregion

        #region Deleted
        private Int32 mDeleted;
        /// <summary>
        ///  是否删除
        /// </summary>
        public Int32 Deleted
        {
            get { return mDeleted; }
            set { mDeleted = value; }
        }
        #endregion

        #region DeptID
        private Decimal mDeptID;
        /// <summary>
        /// 部门ID
        /// </summary>
        public Decimal DeptID
        {
            get { return mDeptID; }
            set { mDeptID = value; }
        }
        #endregion

        #region DeptName
        private String mDeptName = string.Empty;
        /// <summary>
        /// 部门名称
        /// </summary>
        public String DeptName
        {
            get { return mDeptName; }
            set { mDeptName = value; }
        }
        #endregion

        #region ClassTypeID
        private Decimal mClassTypeID;
        /// <summary>
        ///  班次ID
        /// </summary>
        public Decimal ClassTypeID
        {
            get { return mClassTypeID; }
            set { mClassTypeID = value; }
        }
        #endregion

        #region ClassTypeName
        private String mClassTypeName = string.Empty;
        /// <summary>
        /// 班次名称
        /// </summary>
        public String ClassTypeName
        {
            get { return mClassTypeName; }
            set { mClassTypeName = value; }
        }
        #endregion

        #endregion

        #region GetReCorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>Br_OrderClassDP</returns>
        public Br_OrderClassDP GetReCorded(long lngID)
        {
            Br_OrderClassDP ee = new Br_OrderClassDP();
            string strSQL = string.Empty;
            strSQL = "SELECT * FROM Br_OrderClass WHERE ID = " + lngID.ToString();
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            foreach (DataRow dr in dt.Rows)
            {
                ee.ID = Decimal.Parse(dr["ID"].ToString());
                ee.StaffID = Decimal.Parse(dr["StaffID"].ToString());
                ee.StaffName = dr["StaffName"].ToString();
                ee.DutyTime = dr["DutyTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["DutyTime"].ToString());
                ee.RegUserID = Decimal.Parse(dr["RegUserID"].ToString());
                ee.RegUserName = dr["RegUserName"].ToString();
                ee.RegTime = dr["RegTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["RegTime"].ToString());
                ee.Deleted = Int32.Parse(dr["Deleted"].ToString() == "" ? "0" : dr["Deleted"].ToString());
                ee.DeptID = Decimal.Parse(dr["DeptID"].ToString());
                ee.DeptName = dr["DeptName"].ToString();
                ee.ClassTypeID = Decimal.Parse(dr["ClassTypeID"].ToString());
                ee.ClassTypeName = dr["ClassTypeName"].ToString();
            }
            return ee;
        }
        #endregion

        #region GetDataTable
        /// <summary>
        /// GetDataTable
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="rowcount"></param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string sWhere, string sOrder, int pagesize, int pageindex, ref int rowcount)
        {
            DataTable dt = CommonDP.ExcuteSqlTablePage("Br_OrderClass", "*", sWhere, sOrder, pagesize, pageindex, ref rowcount);
            return dt;
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
            strSQL = "SELECT * FROM Br_OrderClass Where 1=1 And nvl(Deleted,0)=0 ";
            strSQL += sWhere;
            strSQL += sOrder;
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        #endregion

        public DataTable GetDataTable(OracleParameter[] parms)
        {
            OracleConnection cn = ConfigTool.GetConnection();

            DataTable dt = OracleDbHelper.ExecuteDataset(cn, CommandType.StoredProcedure, "proc_Br_OrderClass", parms).Tables[0];
            ConfigTool.CloseConnection(cn);

            return dt;
        }

        #region 判断是否存在
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public static bool IsExists(string sWhere)
        {
            bool result = false;

            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM Br_OrderClass Where 1=1 And nvl(Deleted,0)=0 ";
            strSQL += sWhere;
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);

            if (dt != null && dt.Rows.Count > 0)
                result = true;

            return result;
        }

        #endregion

        #region InsertRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pBr_OrderClassDP></param>
        public void InsertRecorded(Br_OrderClassDP pBr_OrderClassDP)
        {
            string strSQL = string.Empty;
            string strID = "0";
            try
            {
                strID = EpowerGlobal.EPGlobal.GetNextID("Br_OrderClassID").ToString();
                pBr_OrderClassDP.ID = decimal.Parse(strID);
                strSQL = @"INSERT INTO Br_OrderClass(
									ID,
									StaffID,
									StaffName,
									DutyTime,
									RegUserID,
									RegUserName,
									RegTime,
									DeptID,
									DeptName,
									ClassTypeID,
									ClassTypeName
					)
					VALUES( " +
                            strID.ToString() + "," +
                            pBr_OrderClassDP.StaffID.ToString() + "," +
                            StringTool.SqlQ(pBr_OrderClassDP.StaffName) + "," +
                            (pBr_OrderClassDP.DutyTime == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(pBr_OrderClassDP.DutyTime.ToString()) + ",'yyyy-MM-dd HH24:mi:ss')") + "," +
                            pBr_OrderClassDP.RegUserID.ToString() + "," +
                            StringTool.SqlQ(pBr_OrderClassDP.RegUserName) + "," +
                            (pBr_OrderClassDP.RegTime == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(pBr_OrderClassDP.RegTime.ToString()) + ",'yyyy-MM-dd HH24:mi:ss')") + "," +
                            pBr_OrderClassDP.DeptID.ToString() + "," +
                            StringTool.SqlQ(pBr_OrderClassDP.DeptName) + "," +
                            pBr_OrderClassDP.ClassTypeID.ToString() + "," +
                            StringTool.SqlQ(pBr_OrderClassDP.ClassTypeName) + 
                    ")";

                CommonDP.ExcuteSql(strSQL);
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region UpdateRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pBr_OrderClassDP></param>
        public void UpdateRecorded(Br_OrderClassDP pBr_OrderClassDP)
        {
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE Br_OrderClass Set " +
                                                        " StaffID = " + pBr_OrderClassDP.StaffID.ToString() + "," +
                            " StaffName = " + StringTool.SqlQ(pBr_OrderClassDP.StaffName) + "," +
                            " DutyTime = " + (pBr_OrderClassDP.DutyTime == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(pBr_OrderClassDP.DutyTime.ToString()) + ",'yyyy-MM-dd HH24:mi:ss')") + "," +
                            " DeptID = " + pBr_OrderClassDP.DeptID.ToString() + "," +
                            " DeptName = " + StringTool.SqlQ(pBr_OrderClassDP.DeptName) + "," +
                            " ClassTypeID = " + pBr_OrderClassDP.ClassTypeID.ToString() + "," +
                            " ClassTypeName = " + StringTool.SqlQ(pBr_OrderClassDP.ClassTypeName) + 
                                " WHERE ID = " + pBr_OrderClassDP.ID.ToString();

                CommonDP.ExcuteSql(strSQL);
            }
            catch
            {
                throw;
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
            try
            {
                string strSQL = "Update Br_OrderClass Set Deleted=1  WHERE ID =" + lngID.ToString();
                CommonDP.ExcuteSql(strSQL);
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}

