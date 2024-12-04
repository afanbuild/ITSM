



/*******************************************************************
 *
 * Description
 * 
 * 班次表操作类
 * Create By  :yxq
 * Create Date:2011年9月8日 星期四
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
    public class Br_OrderClassTypeDP
    {
        /// <summary>
        /// 
        /// </summary>
        public Br_OrderClassTypeDP()
        { }

        #region Property
        #region ID
        private Decimal mID;
        /// <summary>
        ///  班次ID
        /// </summary>
        public Decimal ID
        {
            get { return mID; }
            set { mID = value; }
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

        #region Remark
        private String mRemark = string.Empty;
        /// <summary>
        ///  班次说明
        /// </summary>
        public String Remark
        {
            get { return mRemark; }
            set { mRemark = value; }
        }
        #endregion

        #region RegUserID
        private Decimal mRegUserID;
        /// <summary>
        ///  添加人ID
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
        ///  添加人名称
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
        ///  添加时间
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

        #endregion

        #region GetReCorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>Br_OrderClassTypeDP</returns>
        public Br_OrderClassTypeDP GetReCorded(long lngID)
        {
            Br_OrderClassTypeDP ee = new Br_OrderClassTypeDP();
            string strSQL = string.Empty;
            strSQL = "SELECT * FROM Br_OrderClassType WHERE ID = " + lngID.ToString();
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            foreach (DataRow dr in dt.Rows)
            {
                ee.ID = Decimal.Parse(dr["ID"].ToString());
                ee.ClassTypeName = dr["ClassTypeName"].ToString();
                ee.Remark = dr["Remark"].ToString();
                ee.RegUserID = Decimal.Parse(dr["RegUserID"].ToString());
                ee.RegUserName = dr["RegUserName"].ToString();
                ee.RegTime = dr["RegTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["RegTime"].ToString());
                ee.Deleted = Int32.Parse(dr["Deleted"].ToString() == "" ? "0" : dr["Deleted"].ToString());
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
            DataTable dt = CommonDP.ExcuteSqlTablePage("Br_OrderClassType", "*", sWhere, sOrder, pagesize, pageindex, ref rowcount);
            return dt;
        }
        #endregion

        #region GetDataTable
        /// <summary>
        /// GetDataTable
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string sWhere, string sOrder)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM Br_OrderClassType Where 1=1 And Deleted=0 ";
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
        /// <param name=pBr_OrderClassTypeDP></param>
        public void InsertRecorded(Br_OrderClassTypeDP pBr_OrderClassTypeDP)
        {
            string strSQL = string.Empty;
            string strID = "0";
            try
            {
                strID = EpowerGlobal.EPGlobal.GetNextID("Br_OrderClassTypeID").ToString();
                pBr_OrderClassTypeDP.ID = decimal.Parse(strID);
                strSQL = @"INSERT INTO Br_OrderClassType(
									ID,
									ClassTypeName,
									Remark,
									RegUserID,
									RegUserName,
									RegTime,
									Deleted
					)
					VALUES( " +
                            strID.ToString() + "," +
                            StringTool.SqlQ(pBr_OrderClassTypeDP.ClassTypeName) + "," +
                            StringTool.SqlQ(pBr_OrderClassTypeDP.Remark) + "," +
                            pBr_OrderClassTypeDP.RegUserID.ToString() + "," +
                            StringTool.SqlQ(pBr_OrderClassTypeDP.RegUserName) + "," +
                            "to_date(" + StringTool.SqlQ(pBr_OrderClassTypeDP.RegTime.ToString()) + ",'yyyy-MM-dd HH24:mi:ss')," +
                            pBr_OrderClassTypeDP.Deleted.ToString() +
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
        /// <param name=pBr_OrderClassTypeDP></param>
        public void UpdateRecorded(Br_OrderClassTypeDP pBr_OrderClassTypeDP)
        {
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE Br_OrderClassType Set " +
                                                        " ClassTypeName = " + StringTool.SqlQ(pBr_OrderClassTypeDP.ClassTypeName) + "," +
                            " Remark = " + StringTool.SqlQ(pBr_OrderClassTypeDP.Remark) + "," +
                            " RegUserID = " + pBr_OrderClassTypeDP.RegUserID.ToString() + "," +
                            " RegUserName = " + StringTool.SqlQ(pBr_OrderClassTypeDP.RegUserName) + "," +
                            " RegTime = to_date(" + StringTool.SqlQ(pBr_OrderClassTypeDP.RegTime.ToString()) + ",'yyyy-MM-dd HH24:mi:ss')," +
                            " Deleted = " + pBr_OrderClassTypeDP.Deleted.ToString() +
                                " WHERE ID = " + pBr_OrderClassTypeDP.ID.ToString();

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
                string strSQL = "Update Br_OrderClassType Set Deleted=1  WHERE ID =" + lngID.ToString();
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

