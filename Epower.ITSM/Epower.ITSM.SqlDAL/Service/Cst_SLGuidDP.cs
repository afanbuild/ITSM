
/*******************************************************************
 *
 * Description:标准设置
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
    public class Cst_SLGuidDP
    {
        /// <summary>
        /// 
        /// </summary>
        public Cst_SLGuidDP()
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

        #region LevelID
        /// <summary>
        ///
        /// </summary>
        private Decimal mLevelID;
        public Decimal LevelID
        {
            get { return mLevelID; }
            set { mLevelID = value; }
        }
        #endregion

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

        #region TimeLimit
        /// <summary>
        ///
        /// </summary>
        private Int32 mTimeLimit;
        public Int32 TimeLimit
        {
            get { return mTimeLimit; }
            set { mTimeLimit = value; }
        }
        #endregion

        #region TimeUnit
        /// <summary>
        ///
        /// </summary>
        private Int32 mTimeUnit;
        public Int32 TimeUnit
        {
            get { return mTimeUnit; }
            set { mTimeUnit = value; }
        }
        #endregion

        #endregion

        #region GetReCorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>Cst_SLGuidDP</returns>
        public Cst_SLGuidDP GetReCorded(long lngID)
        {
            Cst_SLGuidDP ee = new Cst_SLGuidDP();
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM Cst_SLGuid WHERE ID = " + lngID.ToString();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            foreach (DataRow dr in dt.Rows)
            {
                ee.ID = Decimal.Parse(dr["ID"].ToString());
                ee.LevelID = Decimal.Parse(dr["LevelID"].ToString());
                ee.GuidID = Decimal.Parse(dr["GuidID"].ToString());
                ee.TimeLimit = Int32.Parse(dr["TimeLimit"].ToString());
                ee.TimeUnit = Int32.Parse(dr["TimeUnit"].ToString());
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
            strSQL = "SELECT * FROM Cst_SLGuid Where 1=1";
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
        /// <param name=pCst_SLGuidDP></param>
        public void InsertRecorded(Cst_SLGuidDP pCst_SLGuidDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            string strID = "0";
            try
            {
                strSQL = @"INSERT INTO Cst_SLGuid(
									LevelID,
									GuidID,
									TimeLimit,
									TimeUnit
					)
					VALUES( " +
                            pCst_SLGuidDP.LevelID.ToString() + "," +
                            pCst_SLGuidDP.GuidID.ToString() + "," +
                            pCst_SLGuidDP.TimeLimit.ToString() + "," +
                            pCst_SLGuidDP.TimeUnit.ToString() +
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
        /// <param name=pCst_SLGuidDP></param>
        public void UpdateRecorded(Cst_SLGuidDP pCst_SLGuidDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE Cst_SLGuid Set " +
                                                        " LevelID = " + pCst_SLGuidDP.LevelID.ToString() + "," +
                            " GuidID = " + pCst_SLGuidDP.GuidID.ToString() + "," +
                            " TimeLimit = " + pCst_SLGuidDP.TimeLimit.ToString() + "," +
                            " TimeUnit = " + pCst_SLGuidDP.TimeUnit.ToString() +
                                " WHERE ID = " + pCst_SLGuidDP.ID.ToString();

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
                string strSQL = "Delete Cst_SLGuid WHERE ID =" + lngID.ToString();
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

        #region GetDataByLevelID
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngSLevelID"></param>
        /// <returns>DataTable</returns>
        public DataTable GetDataByLevelID(long lngSLevelID)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT a.*,b.guidname,'1' as Saved FROM Cst_SLGuid a,Cst_GuidDefinition b WHERE a.guidid = b.guidid ";
            strSQL += " and a.LevelID=" + lngSLevelID.ToString();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }


        public DataTable GetDataByLevelIDCache(long lngSLevelID)
        {
             DataTable dt;
               //2008-05-01 添加SQL缓存依赖的处理方式,减少数据库连接次数
             //if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
             //{

             //    dt = CommSqlCacheHelper.GetDataTableFromCache("servicelevelguid");

             //    DataTable dtTemp = dt.Clone();
             //    //注意            *******      SWHERE 格式的合法性    ******
             //    dt.DefaultView.RowFilter = " LevelID=" + lngSLevelID.ToString();

             //    dtTemp.Rows.Clear();
             //    foreach (DataRowView dvr in dt.DefaultView)
             //    {
             //        dtTemp.Rows.Add(dvr.Row.ItemArray);

             //    }
             //    return dtTemp;
             //}
             //else
             //{
                 string strSQL = string.Empty;
                 OracleConnection cn = ConfigTool.GetConnection();

                 strSQL = "SELECT a.*,b.guidname FROM Cst_SLGuid a,Cst_GuidDefinition b WHERE a.guidid = b.guidid ";
                 strSQL += " and a.LevelID=" + lngSLevelID.ToString();
                 dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                 ConfigTool.CloseConnection(cn);
                 return dt;
             //}
        }

        #endregion

        #region 批量保存项 SaveDetailItem
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="lngLevelID"></param>
        public void SaveDetailItem(DataTable dt,long lngLevelID)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open) { cn.Open(); }
            OracleTransaction tran = cn.BeginTransaction();

            string strSQL = string.Empty;
            strSQL = "Delete Cst_SLGuid Where LevelID=" + lngLevelID.ToString();
            OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);
            string strID = "0";
            foreach (DataRow dr in dt.Rows)
            {
                strID = EpowerGlobal.EPGlobal.GetNextID("Cst_SLGuidID").ToString();
                strSQL = @" INSERT INTO Cst_SLGuid(id,
									LevelID,
									GuidID,
									TimeLimit,
									TimeUnit,
                                    Target,
                                    Remark
					)
					VALUES( " + strID + "," +
                           lngLevelID.ToString() + "," +
                           dr["GuidID"].ToString() + "," +
                           dr["TimeLimit"].ToString() + "," +
                           dr["TimeUnit"].ToString() + "," +
                           StringTool.SqlQ(dr["Target"].ToString()) + "," +
                           StringTool.SqlQ(dr["Remark"].ToString()) +
                   ")";
                OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);
            }
            try
            {
                //if (!string.IsNullOrEmpty(strSQL))
                //    OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);
                tran.Commit();
            }
            catch (Exception e)
            {
                tran.Rollback();
                throw e;
            }
            finally
            {
                tran.Dispose();
                ConfigTool.CloseConnection(cn);
            }
        }
        #endregion 
    }
}

