/*******************************************************************
 *
 * Description:资产目录
 * 
 * 
 * Create By  :ly
 * Create Date:2011年8月15日
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
    /// 资产目录
    /// </summary>
    public class Equ_CateListsDP
    {
        #region 构造方法

        /// <summary>
        /// 构造方法
        /// </summary>
        public Equ_CateListsDP()
        { }

        #endregion

        #region Property
        #region ID
        private Decimal mID;
        /// <summary>
        ///
        /// </summary>
        public Decimal ID
        {
            get { return mID; }
            set { mID = value; }
        }
        #endregion

        #region CatalogID
        private Decimal mCatalogID;
        /// <summary>
        ///
        /// </summary>
        public Decimal CatalogID
        {
            get { return mCatalogID; }
            set { mCatalogID = value; }
        }
        #endregion

        #region CatalogName
        private String mCatalogName = string.Empty;
        /// <summary>
        ///
        /// </summary>
        public String CatalogName
        {
            get { return mCatalogName; }
            set { mCatalogName = value; }
        }
        #endregion

        #region ListName
        private String mListName = string.Empty;
        /// <summary>
        ///
        /// </summary>
        public String ListName
        {
            get { return mListName; }
            set { mListName = value; }
        }
        #endregion

        #endregion

        #region GetReCorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>Equ_CateListsDP</returns>
        public Equ_CateListsDP GetReCorded(long lngID)
        {
            Equ_CateListsDP ee = new Equ_CateListsDP();
            string strSQL = string.Empty;
            strSQL = "SELECT * FROM Equ_CateLists WHERE ID = " + lngID.ToString();
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            foreach (DataRow dr in dt.Rows)
            {
                ee.ID = Decimal.Parse(dr["ID"].ToString());
                ee.CatalogID = Decimal.Parse(dr["CatalogID"].ToString());
                ee.CatalogName = dr["CatalogName"].ToString();
                ee.ListName = dr["ListName"].ToString();
            }
            return ee;
        }
        #endregion

        #region GetReCorded
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="lngID"></param>
        /// <param name="strCatalog"></param>
        /// <returns></returns>
        public Equ_CateListsDP GetReCorded(long lngID, string strCatalog)
        {
            Equ_CateListsDP ee = new Equ_CateListsDP();
            string strSQL = string.Empty;
            strSQL = "SELECT * FROM Equ_CateLists WHERE ID = " + lngID.ToString() + " and CatalogID = " + strCatalog + " and deleted = 0";
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            foreach (DataRow dr in dt.Rows)
            {
                ee.ID = Decimal.Parse(dr["ID"].ToString());
                ee.CatalogID = Decimal.Parse(dr["CatalogID"].ToString());
                ee.CatalogName = dr["CatalogName"].ToString();
                ee.ListName = dr["ListName"].ToString();
            }
            return ee;
        }
        #endregion

        #region GetReCorded

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="listName">资产目录名称</param>
        /// <param name="strCatalog">资产类别ID</param>
        /// <returns></returns>
        public Equ_CateListsDP GetReCorded(string listName, decimal strCatalog, ref int flag)
        {
            Equ_CateListsDP ee = new Equ_CateListsDP();
            string strSQL = string.Empty;
            strSQL = "SELECT * FROM Equ_CateLists WHERE ListName = " + StringTool.SqlQ(listName) + " and CatalogID = " + strCatalog + " and deleted = 0";
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            foreach (DataRow dr in dt.Rows)
            {
                ee.ID = Decimal.Parse(dr["ID"].ToString());
                ee.CatalogID = Decimal.Parse(dr["CatalogID"].ToString());
                ee.CatalogName = dr["CatalogName"].ToString();
                ee.ListName = dr["ListName"].ToString();
                flag = 1;
            }
            return ee;
        }

        #endregion

        #region CheckCateListChild--检查资产目录下是否存在资产

        /// <summary>
        /// 检查资产目录下是否存在资产
        /// </summary>
        /// <param name="lngID"></param>
        /// <param name="strCatalog"></param>
        /// <returns></returns>
        public bool CheckCateListChild(long lngID, string strCatalog)
        {
            OracleConnection conn = ConfigTool.GetConnection();
            conn.Open();
            string strSQL = string.Empty;
            bool flag = true;
            strSQL = "SELECT ID FROM EQU_DESK WHERE DELETED=0 AND CatalogID=" + strCatalog.ToString() + " AND ListID=" + lngID.ToString();
            try
            {
                OracleDataReader reader = OracleDbHelper.ExecuteReader(conn, CommandType.Text, strSQL);//执行语句
                if (reader.Read())
                {
                    flag = false;
                }
                reader.Close();
                return flag;
            }
            catch
            {
                conn.Close();
                throw;
            }
            finally
            {
                conn.Close();
            }
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
            DataTable dt = CommonDP.ExcuteSqlTablePage("Equ_CateLists", "*", sWhere, sOrder, pagesize, pageindex, ref rowcount);
            return dt;
        }

        /// <summary>
        /// 查询数据集
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sWhere)
        {
            OracleConnection conn = ConfigTool.GetConnection();
            conn.Open();
            string strSQL = string.Empty;
            strSQL = "SELECT * FROM Equ_CateLists WHERE 1=1 AND deleted=0 " + sWhere;
            DataTable dt = OracleDbHelper.ExecuteDataTable(conn, CommandType.Text, strSQL);
            return dt;
        }
        #endregion

        #region InsertRecorded
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name=pEqu_CateListsDP></param>
        public void InsertRecorded(Equ_CateListsDP pEqu_CateListsDP)
        {
            string strSQL = string.Empty;
            string strID = "0";
            try
            {
                strID = EpowerGlobal.EPGlobal.GetNextID("Equ_CateListsID").ToString();
                pEqu_CateListsDP.ID = decimal.Parse(strID);
                strSQL = @"INSERT INTO Equ_CateLists(
									ID,
									CatalogID,
									CatalogName,
									ListName,
                                    Deleted
					)
					VALUES( " +
                            strID.ToString() + "," +
                            pEqu_CateListsDP.CatalogID.ToString() + "," +
                            StringTool.SqlQ(pEqu_CateListsDP.CatalogName) + "," +
                            StringTool.SqlQ(pEqu_CateListsDP.ListName) + ",0" +
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
        /// 更新数据
        /// </summary>
        /// <param name=pEqu_CateListsDP></param>
        public void UpdateRecorded(Equ_CateListsDP pEqu_CateListsDP)
        {
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE Equ_CateLists Set " +
                            " ListName = " + StringTool.SqlQ(pEqu_CateListsDP.ListName) +
                                " WHERE ID = " + pEqu_CateListsDP.ID.ToString();

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
                string strSQL = "Update Equ_CateLists Set Deleted=1  WHERE ID =" + lngID.ToString();
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
