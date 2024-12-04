
/*******************************************************************
 *
 * Description
 * 
 * 
 * Create By  :yxq
 * Create Date:2011年8月16日 星期二

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
    public class Ea_SetdeskDP
    {
        /// <summary>
        /// 
        /// </summary>
        public Ea_SetdeskDP()
        { }

        #region Property
        #region ID
        private Decimal mID;
        /// <summary>
        /// 主表ID
        /// </summary>
        public Decimal ID
        {
            get { return mID; }
            set { mID = value; }
        }
        #endregion

        #region UserID
        private Decimal mUserID;
        /// <summary>
        /// 用户ID
        /// </summary>
        public Decimal UserID
        {
            get { return mUserID; }
            set { mUserID = value; }
        }
        #endregion

        #region UserName
        private String mUserName = string.Empty;
        /// <summary>
        /// 用户名称
        /// </summary>
        public String UserName
        {
            get { return mUserName; }
            set { mUserName = value; }
        }
        #endregion

        #region BlockRoom
        private String mBlockRoom = string.Empty;
        /// <summary>
        /// 座室
        /// </summary>
        public String BlockRoom
        {
            get { return mBlockRoom; }
            set { mBlockRoom = value; }
        }
        #endregion

        #region Deleted
        private Int32 mDeleted;
        /// <summary>
        /// 删除标志
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
        /// <returns>Ea_SetdeskDP</returns>
        public Ea_SetdeskDP GetReCorded(long lngID)
        {
            Ea_SetdeskDP ee = new Ea_SetdeskDP();
            string strSQL = string.Empty;
            strSQL = "SELECT * FROM Ea_Setdesk WHERE ID = " + lngID.ToString();
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            foreach (DataRow dr in dt.Rows)
            {
                ee.ID = Decimal.Parse(dr["ID"].ToString());
                ee.UserID = Decimal.Parse(dr["UserID"].ToString());
                ee.UserName = dr["UserName"].ToString();
                ee.BlockRoom = dr["BlockRoom"].ToString();
                ee.Deleted = int.Parse(dr["Deleted"].ToString());
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
            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");
            strSQL = "SELECT * FROM Ea_setdesk Where 1=1 And Deleted=0 ";
            strSQL += sWhere;
            strSQL += sOrder;
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
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
            DataTable dt = CommonDP.ExcuteSqlTablePage("Ea_Setdesk", "*", sWhere, sOrder, pagesize, pageindex, ref rowcount);
            return dt;
        }
        #endregion

        #region InsertRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pEa_SetdeskDP></param>
        public void InsertRecorded(Ea_SetdeskDP pEa_SetdeskDP)
        {
            string strSQL = string.Empty;
            string strID = "0";
            try
            {
                strID = EpowerGlobal.EPGlobal.GetNextID("Ea_SetdeskID").ToString();
                pEa_SetdeskDP.ID = decimal.Parse(strID);
                strSQL = @"INSERT INTO Ea_Setdesk(
									ID,
									UserID,
									UserName,
									BlockRoom,
									Deleted
					)
					VALUES( " +
                            strID.ToString() + "," +
                            pEa_SetdeskDP.UserID.ToString() + "," +
                            StringTool.SqlQ(pEa_SetdeskDP.UserName) + "," +
                            StringTool.SqlQ(pEa_SetdeskDP.BlockRoom) + "," +
                            pEa_SetdeskDP.Deleted.ToString() +
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
        /// <param name=pEa_SetdeskDP></param>
        public void UpdateRecorded(Ea_SetdeskDP pEa_SetdeskDP)
        {
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE Ea_Setdesk Set " +
                                                        " UserID = " + pEa_SetdeskDP.UserID.ToString() + "," +
                            " UserName = " + StringTool.SqlQ(pEa_SetdeskDP.UserName) + "," +
                            " BlockRoom = " + StringTool.SqlQ(pEa_SetdeskDP.BlockRoom) + "," +
                            " Deleted = " + pEa_SetdeskDP.Deleted.ToString() +
                                " WHERE ID = " + pEa_SetdeskDP.ID.ToString();

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
                string strSQL = "Update Ea_Setdesk Set Deleted=1  WHERE ID =" + lngID.ToString();
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

