using System;
using System.Collections.Generic;
using System.Text;
using Epower.DevBase.BaseTools;
using System.Data;
using System.Data.OracleClient;

namespace Epower.ITSM.SqlDAL
{
    public class Br_ConditionSaveDP
    {
        #region 属性


        #region ID
        /// <summary>
        /// 标示ID
        /// </summary>
        private long mID;
        public long ID
        {
            get { return mID; }
            set { mID = value; }
        }
        #endregion

        #region ConditionName
        /// <summary>
        /// 条件名称
        /// </summary>
        private string mConditionName = string.Empty;
        public string ConditionName
        {
            get { return mConditionName; }
            set { mConditionName = value; }
        }
        #endregion

        #region UserID
        /// <summary>
        /// 所属用户ID
        /// </summary>
        private long mUserID;
        public long UserID
        {
            get { return mUserID; }
            set { mUserID = value; }
        }
        #endregion

        #region TableName
        /// <summary>
        /// 所属表名 (按表名进行分组)
        /// </summary>
        private string mTableName = string.Empty;
        public string TableName
        {
            get { return mTableName; }
            set { mTableName = value; }
        }
        #endregion

        #region Condition
        /// <summary>
        /// 条件XML串

        /// </summary>
        private string mCondition = string.Empty;
        public string Condition
        {
            get { return mCondition; }
            set { mCondition = value; }
        }
        #endregion

        #endregion

        public Br_ConditionSaveDP()
        {

        }

        #region GetReCorded
        /// <summary>
        /// 得到某条查询记录
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="ConditionName">条件名称</param>
        /// <param name="TableName">所属表名</param>
        /// <returns></returns>
        public Br_ConditionSaveDP GetReCorded(long UserID, string ConditionName, string TableName)
        {
            string strSQL = "SELECT * FROM Br_ConditionSave WHERE UserID = " + UserID.ToString() + " and ConditionName=" + StringTool.SqlQ(ConditionName) + " and TableName=" + StringTool.SqlQ(TableName);
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);

            Br_ConditionSaveDP ee = new Br_ConditionSaveDP();

            foreach (DataRow dr in dt.Rows)
            {
                ee.ID = long.Parse(dr["ID"].ToString());
                ee.UserID = long.Parse(dr["UserID"].ToString());
                ee.ConditionName = dr["ConditionName"].ToString();
                ee.TableName = dr["TableName"].ToString();
                ee.Condition = dr["Condition"].ToString();
            }

            return ee;

        }
        /// <summary>
        /// 得到某条查询记录
        /// </summary>
        /// <param name="lngID">标示ID</param>
        /// <returns></returns>
        public Br_ConditionSaveDP GetReCorded(long lngID)
        {
            string strSQL = "SELECT * FROM Br_ConditionSave WHERE ID = " + lngID.ToString();
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);

            Br_ConditionSaveDP ee = new Br_ConditionSaveDP();

            foreach (DataRow dr in dt.Rows)
            {
                ee.ID = long.Parse(dr["ID"].ToString());
                ee.UserID = long.Parse(dr["UserID"].ToString());
                ee.ConditionName = dr["ConditionName"].ToString();
                ee.TableName = dr["TableName"].ToString();
                ee.Condition = dr["Condition"].ToString();
            }

            return ee;

        }
        #endregion

        #region GetDataTable
        /// <summary>
        /// 根据条件查询所有数据

        /// </summary>
        /// <param name="strWhere">where 条件 如( where 1=1 and TableName='XXX' )</param>
        /// <param name="strOrder">排序字段 如( order by ID)</param>
        /// <returns></returns>
        public static DataTable GetDataTable(string strWhere, string strOrder)
        {
            string strSQL = "select * from Br_ConditionSave ";
            strSQL = strSQL + strWhere + " " + strOrder;
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            return dt;
        }
        #endregion

        #region 获取高级查询条件列表
        /// <summary>
        /// 获取高级查询条件列表
        /// </summary>
        /// <param name="strWhere">where 条件 如( where 1=1 and TableName='XXX' )</param>
        /// <param name="strOrder">排序字段 如( order by ID)</param>
        /// <returns></returns>
        public static DataTable GetNames(string strWhere, string strOrder)
        {
            string strSQL = "select ID, ConditionName from Br_ConditionSave";
            strSQL = strSQL + strWhere + " " + strOrder;
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            return dt;
        }

        /// <summary>
        /// 使用编号查找高级查询条件的内容
        /// </summary>
        /// <param name="lngConditionId">高级查询条件编号</param>
        /// <returns></returns>
        public static String GetConditionContent(long lngConditionId)
        {
            string strSQL = String.Format("SELECT Condition FROM Br_ConditionSave WHERE ID = {0}", lngConditionId);
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);

            if (dt.Rows.Count <= 0) return String.Empty;
            return dt.Rows[0]["Condition"].ToString();
        }


        #endregion


        #region InsertRecorded
        /// <summary>
        ///  添加数据
        /// </summary>
        /// <param name=pCst_ServiceLevelDP></param>
        public void InsertRecorded(Br_ConditionSaveDP pBr_ConditionSaveDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            string strID = "0";
            try
            {
                strID = EpowerGlobal.EPGlobal.GetNextID("Br_ConditionSaveID").ToString();
                pBr_ConditionSaveDP.ID = long.Parse(strID);
                strSQL = @"INSERT INTO Br_ConditionSave(
									ID,
									ConditionName,
									UserID,
									TableName,
									Condition									
					)
					VALUES( " +
                            strID.ToString() + "," +
                            StringTool.SqlQ(pBr_ConditionSaveDP.ConditionName) + "," +
                            pBr_ConditionSaveDP.UserID + "," +
                            StringTool.SqlQ(pBr_ConditionSaveDP.TableName) + "," +
                            StringTool.SqlQ(pBr_ConditionSaveDP.Condition) +
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
        /// 修改记录
        /// </summary>
        /// <param name=pCst_ServiceLevelDP></param>
        public void UpdateRecorded(Br_ConditionSaveDP pBr_ConditionSaveDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE Br_ConditionSave Set " +
                            " ConditionName = " + StringTool.SqlQ(pBr_ConditionSaveDP.ConditionName) + "," +
                            " UserID = " + pBr_ConditionSaveDP.UserID + "," +
                            " TableName = " + StringTool.SqlQ(pBr_ConditionSaveDP.TableName) + "," +
                            " Condition = " + StringTool.SqlQ(pBr_ConditionSaveDP.Condition) +
                                " WHERE ID = " + pBr_ConditionSaveDP.ID.ToString();

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
        /// 删除记录
        /// </summary>
        /// <param name="lngID"></param>
        public void DeleteRecorded(long lngID)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                string strSQL = "delete from Br_ConditionSave WHERE ID =" + lngID.ToString();
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
        /// 删除记录
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="ConditionName">条件名称</param>
        /// <param name="TableName">所属表名</param>
        public void DeleteRecorded(long UserID, string ConditionName, string TableName)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                string strSQL = "delete from Br_ConditionSave WHERE UserID = " + UserID.ToString() + " and ConditionName=" + StringTool.SqlQ(ConditionName) + " and TableName=" + StringTool.SqlQ(TableName);
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
