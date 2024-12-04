/*******************************************************************
 *
 * Description:配置项数据层管理
 * 
 * 
 * Create By  :zhumc
 * Create Date:2008年4月5日
 * *****************************************************************/
using System;
using System.Data;
using System.Collections;
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
    public class Equ_SchemaItemsDP
    {
        /// <summary>
        /// 
        /// </summary>
        public Equ_SchemaItemsDP()
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

        #region FieldID
        /// <summary>
        ///
        /// </summary>
        private String mFieldID;
        public String FieldID
        {
            get { return mFieldID; }
            set { mFieldID = value; }
        }
        #endregion

        #region CHName
        /// <summary>
        ///
        /// </summary>
        private String mCHName;
        public String CHName
        {
            get { return mCHName; }
            set { mCHName = value; }
        }
        #endregion

        #region itemType
        /// <summary>
        ///
        /// </summary>
        private Int32 mitemType;
        public Int32 itemType
        {
            get { return mitemType; }
            set { mitemType = value; }
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

        #region CatalogID
        /// <summary>
        ///
        /// </summary>
        private Decimal mCatalogID;
        public Decimal CatalogID
        {
            get { return mCatalogID; }
            set { mCatalogID = value; }
        }
        #endregion

        #endregion

        #region GetReCorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>Equ_SchemaItemsDP</returns>
        public Equ_SchemaItemsDP GetReCorded(long lngID)
        {
            Equ_SchemaItemsDP ee = new Equ_SchemaItemsDP();

            DataTable dt;

            //2008-05-01 添加SQL缓存依赖的处理方式,减少数据库连接次数
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                dt = CommSqlCacheHelper.GetDataTableFromCache("equschemaitems");

                dt.DefaultView.RowFilter = " ID = " + lngID.ToString();

                foreach (DataRowView dvr in dt.DefaultView)
                {
                    ee.ID = Decimal.Parse(dvr.Row["id"].ToString());
                    ee.FieldID = dvr.Row["FieldID"].ToString();
                    ee.CHName = dvr.Row["CHName"].ToString();
                    ee.itemType = Int32.Parse(dvr.Row["itemType"].ToString());
                    ee.Deleted = Int32.Parse(dvr.Row["Deleted"].ToString());
                    ee.CatalogID = Decimal.Parse(dvr.Row["CatalogID"].ToString() == "" ? "1" : dvr.Row["CatalogID"].ToString());

                }
                return ee;


            }
            else
            {


                string strSQL = string.Empty;

                OracleConnection cn = ConfigTool.GetConnection();
                strSQL = "SELECT * FROM Equ_SchemaItems WHERE ID = " + lngID.ToString();
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                ConfigTool.CloseConnection(cn);
                foreach (DataRow dr in dt.Rows)
                {
                    ee.ID = Decimal.Parse(dr["id"].ToString());
                    ee.FieldID = dr["FieldID"].ToString();
                    ee.CHName = dr["CHName"].ToString();
                    ee.itemType = Int32.Parse(dr["itemType"].ToString());
                    ee.Deleted = Int32.Parse(dr["Deleted"].ToString());
                    ee.CatalogID = Decimal.Parse(dr["CatalogID"].ToString() == "" ? "1" : dr["CatalogID"].ToString());
                }
                return ee;
            }
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
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                dt = CommSqlCacheHelper.GetDataTableFromCache("equschemaitems");

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
                strSQL = "SELECT * FROM Equ_SchemaItems Where 1=1 And Deleted=0 ";
                strSQL += sWhere;
                strSQL += sOrder;
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                ConfigTool.CloseConnection(cn);
                return dt;
            }
        }

        public DataTable GetDataTable(string sWhere, string sOrder, int pagesize, int pageindex, ref int rowcount)
        {
            string strWhere = "1=1 And Deleted=0" + sWhere;
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "Equ_SchemaItems", "*", sOrder, pagesize, pageindex, strWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region InsertRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pEqu_SchemaItemsDP></param>
        public void InsertRecorded(Equ_SchemaItemsDP pEqu_SchemaItemsDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            string strID = "0";
            try
            {
                strID = EpowerGlobal.EPGlobal.GetNextID("Equ_SchemaItemsID").ToString();
                pEqu_SchemaItemsDP.ID = decimal.Parse(strID);
                strSQL = @"INSERT INTO Equ_SchemaItems(
									id,
									FieldID,
									CHName,
									itemType,
									Deleted,
                                    CatalogID
					)
					VALUES( " +
                            strID.ToString() + "," +
                            StringTool.SqlQ(getMaxFiledID().ToString()) + "," +
                            StringTool.SqlQ(pEqu_SchemaItemsDP.CHName) + "," +
                            pEqu_SchemaItemsDP.itemType.ToString() + "," +
                            pEqu_SchemaItemsDP.Deleted.ToString() + "," +
                            pEqu_SchemaItemsDP.CatalogID.ToString() +
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
        /// <param name=pEqu_SchemaItemsDP></param>
        public void UpdateRecorded(Equ_SchemaItemsDP pEqu_SchemaItemsDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE Equ_SchemaItems Set " +
                                                        " FieldID = " + StringTool.SqlQ(pEqu_SchemaItemsDP.FieldID) + "," +
                            " CHName = " + StringTool.SqlQ(pEqu_SchemaItemsDP.CHName) + "," +
                            " itemType = " + pEqu_SchemaItemsDP.itemType.ToString() + "," +
                            " Deleted = " + pEqu_SchemaItemsDP.Deleted.ToString() + "," +
                            " CatalogID = " + pEqu_SchemaItemsDP.CatalogID.ToString() +
                                " WHERE id = " + pEqu_SchemaItemsDP.ID.ToString();

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
                string strSQL = "Update Equ_SchemaItems Set Deleted=1  WHERE ID =" + lngID.ToString();
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

        public bool isEquShiYong(long FeildID)
        {

            bool isTrue = false;
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                string strSQL = "select * from Equ_Category where deleted=0 and configureSchema like " + StringTool.SqlQ("%ID=\"" + FeildID.ToString() + "\"%");
                DataTable dt =  OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                if (dt.Rows.Count > 0)
                {
                    isTrue = true;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }

            return isTrue;
        }

        /// <param name="lngID"></param>
        public void DeleteRecorded2(long lngID)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                string strSQL = "Update Br_SchemaItems Set Deleted=1  WHERE ID =" + lngID.ToString();
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

        #region CheckIsRepeat
        /// <summary>
        /// 检查是否存在
        /// </summary>
        /// <param name="FieldID"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool CheckIsRepeat(string FieldID, decimal ID)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            bool breturn = false;
            string strSQL = @"select ID from Equ_SchemaItems
							 WHERE  FieldID =" + StringTool.SqlQ(FieldID);
            if (ID != 0)
                strSQL += " And ID<>" + ID.ToString();

            try
            {
                object pobject = OracleDbHelper.ExecuteScalar(cn, CommandType.Text, strSQL);
                if (pobject != null)
                    breturn = true;
                return breturn;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }


        }
        #endregion


        #region CheckIsRepeat
        /// <summary>
        /// 检查是否重复
        /// </summary>
        /// <param name="FieldID"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool CheckIsRepeatName(string FieldName, decimal ID, ref Equ_SchemaItemsDP Schema)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            bool breturn = false;
            string strSQL = @"select ID from Equ_SchemaItems
							 WHERE Deleted=0 AND CHName =" + StringTool.SqlQ(FieldName);
            if (ID != 0)
                strSQL += " And ID<>" + ID.ToString();

            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                if (dt.Rows.Count > 0)
                {
                    //获得该信息
                    Schema = GetReCorded(long.Parse(dt.Rows[0]["ID"].ToString()));
                    breturn = true;
                }
                return breturn;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }


        }
        #endregion


        #region GetReCorded by fieldid
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>Equ_SchemaItemsDP</returns>
        public Equ_SchemaItemsDP GetReCorded(string sID)
        {
            Equ_SchemaItemsDP ee = new Equ_SchemaItemsDP();


            DataTable dt;

            //2008-05-01 添加SQL缓存依赖的处理方式,减少数据库连接次数
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                dt = CommSqlCacheHelper.GetDataTableFromCache("equschemaitems");

                dt.DefaultView.RowFilter = " FieldID = " + StringTool.SqlQ(sID);

                foreach (DataRowView dvr in dt.DefaultView)
                {
                    ee.ID = Decimal.Parse(dvr.Row["id"].ToString());
                    ee.FieldID = dvr.Row["FieldID"].ToString();
                    ee.CHName = dvr.Row["CHName"].ToString();
                    ee.itemType = Int32.Parse(dvr.Row["itemType"].ToString());
                    ee.Deleted = Int32.Parse(dvr.Row["Deleted"].ToString());
                    ee.CatalogID = Decimal.Parse(dvr.Row["CatalogID"].ToString() == "" ? "1" : dvr.Row["CatalogID"].ToString());

                }
                return ee;


            }
            else
            {

                string strSQL = string.Empty;
                OracleConnection cn = ConfigTool.GetConnection();
                strSQL = "SELECT * FROM Equ_SchemaItems WHERE  FieldID = " + StringTool.SqlQ(sID);
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                ConfigTool.CloseConnection(cn);
                foreach (DataRow dr in dt.Rows)
                {
                    ee.ID = Decimal.Parse(dr["id"].ToString());
                    ee.FieldID = dr["FieldID"].ToString();
                    ee.CHName = dr["CHName"].ToString();
                    ee.itemType = Int32.Parse(dr["itemType"].ToString());
                    ee.Deleted = Int32.Parse(dr["Deleted"].ToString());
                    ee.CatalogID = Decimal.Parse(dr["CatalogID"].ToString() == "" ? "1" : dr["CatalogID"].ToString());
                }
                return ee;
            }
        }



        #endregion

        #region GetAllFields

        public Hashtable GetAllFields()
        {
            return GetAllFields(-1);
        }

        public Hashtable GetAllFieldsHistory()
        {
            return GetAllFieldsHistory(-1);
        }

        /// <summary>
        /// 获取所有字段名和ID的值对信息
        /// </summary>
        /// <param name="iType"></param>
        /// <returns></returns>
        public Hashtable GetAllFieldsHistory(int iType)
        {
            Hashtable ht = new Hashtable();

            DataTable dt;

            //2008-05-01 添加SQL缓存依赖的处理方式,减少数据库连接次数
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                dt = CommSqlCacheHelper.GetDataTableFromCache("equschemaitems");
                if (iType == -1)
                {
                    dt.DefaultView.RowFilter = " 1 = 1 ";
                }
                else
                {
                    dt.DefaultView.RowFilter = "  itemType = " + iType.ToString();
                }
                foreach (DataRowView dvr in dt.DefaultView)
                {
                    ht.Add(dvr.Row["FieldID"].ToString(), dvr.Row["CHName"].ToString());
                }

            }
            else
            {

                string strSQL = string.Empty;
                OracleConnection cn = ConfigTool.GetConnection();
                if (iType == -1)
                {
                    strSQL = "SELECT * FROM Equ_SchemaItems ";
                }
                else
                {
                    strSQL = "SELECT * FROM Equ_SchemaItems WHERE  itemType = " + iType.ToString();
                }
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                ConfigTool.CloseConnection(cn);
                foreach (DataRow dr in dt.Rows)
                {
                    ht.Add(dr["FieldID"].ToString(), dr["CHName"].ToString());
                }
            }
            return ht;
        }
        /// <summary>
        /// 获取所有字段名和ID的值对信息
        /// </summary>
        /// <param name="iType"></param>
        /// <returns></returns>
        public Hashtable GetAllFields(int iType)
        {
            Hashtable ht = new Hashtable();

            DataTable dt;

            //2008-05-01 添加SQL缓存依赖的处理方式,减少数据库连接次数
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                dt = CommSqlCacheHelper.GetDataTableFromCache("equschemaitems");
                if (iType == -1)
                {
                    dt.DefaultView.RowFilter = " deleted = 0 ";
                }
                else
                {
                    dt.DefaultView.RowFilter = " deleted = 0  AND itemType = " + iType.ToString();
                }
                foreach (DataRowView dvr in dt.DefaultView)
                {
                    ht.Add(dvr.Row["FieldID"].ToString(), dvr.Row["CHName"].ToString());
                }

            }
            else
            {

                string strSQL = string.Empty;
                OracleConnection cn = ConfigTool.GetConnection();
                if (iType == -1)
                {
                    strSQL = "SELECT * FROM Equ_SchemaItems WHERE deleted = 0";
                }
                else
                {
                    strSQL = "SELECT * FROM Equ_SchemaItems WHERE deleted = 0 AND itemType = " + iType.ToString();
                }
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                ConfigTool.CloseConnection(cn);
                foreach (DataRow dr in dt.Rows)
                {
                    ht.Add(dr["FieldID"].ToString(), dr["CHName"].ToString());
                }
            }
            return ht;
        }



        #endregion

        #region 配置信息
        /// <summary>
        /// 配置信息
        /// </summary>
        /// <param name="iType"></param>
        /// <returns></returns>
        public Hashtable GetItemsAllFields(int iType)
        {
            Hashtable ht = new Hashtable();

            DataTable dt;

            //2008-05-01 添加SQL缓存依赖的处理方式,减少数据库连接次数
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                dt = CommSqlCacheHelper.GetDataTableFromCache("equschemaitems");
                if (iType == -1)
                {
                    dt.DefaultView.RowFilter = " deleted = 0 ";
                }
                else
                {
                    dt.DefaultView.RowFilter = " deleted = 0  AND itemType = " + iType.ToString();
                }
                foreach (DataRowView dvr in dt.DefaultView)
                {
                    ht.Add(dvr.Row["FieldID"].ToString(), dvr.Row["CHName"].ToString() + dvr.Row["itemType"].ToString());
                }

            }
            else
            {

                string strSQL = string.Empty;
                OracleConnection cn = ConfigTool.GetConnection();
                if (iType == -1)
                {
                    strSQL = "SELECT * FROM Equ_SchemaItems WHERE deleted = 0";
                }
                else
                {
                    strSQL = "SELECT * FROM Equ_SchemaItems WHERE deleted = 0 AND itemType = " + iType.ToString();
                }
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                ConfigTool.CloseConnection(cn);
                foreach (DataRow dr in dt.Rows)
                {
                    ht.Add(dr["FieldID"].ToString(), dr["CHName"].ToString() + dr["itemType"].ToString());
                }
            }
            return ht;
        }
        #endregion

        //获取对应ID的配置信息
        #region 配置信息
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iType"></param>
        /// <returns></returns>
        public Hashtable GetItemsByID(int iType, decimal id)
        {
            Hashtable ht = new Hashtable();

            DataTable dt;

            //2008-05-01 添加SQL缓存依赖的处理方式,减少数据库连接次数
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                dt = CommSqlCacheHelper.GetDataTableFromCache("equschemaitems");
                if (iType == -1)
                {
                    dt.DefaultView.RowFilter = " deleted = 0 and  ID=" + id;
                }
                else
                {
                    dt.DefaultView.RowFilter = " deleted = 0  AND itemType = " + iType.ToString() + "and ID=" + id;
                }
                foreach (DataRowView dvr in dt.DefaultView)
                {
                    ht.Add(dvr.Row["FieldID"].ToString(), dvr.Row["CHName"].ToString() + dvr.Row["itemType"].ToString());
                }

            }
            else
            {

                string strSQL = string.Empty;
                OracleConnection cn = ConfigTool.GetConnection();
                if (iType == -1)
                {
                    strSQL = "SELECT * FROM Equ_SchemaItems WHERE deleted = 0 and ID=" + id;
                }
                else
                {
                    strSQL = "SELECT * FROM Equ_SchemaItems WHERE deleted = 0 AND itemType = " + iType.ToString() + "and ID=" + id;
                }
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                ConfigTool.CloseConnection(cn);
                foreach (DataRow dr in dt.Rows)
                {
                    ht.Add(dr["FieldID"].ToString(), dr["CHName"].ToString() + dr["itemType"].ToString());
                }
            }
            return ht;
        }
        #endregion

        #region 得到最大的编号ID

        /// <summary>
        /// 得到最大的编号ID
        /// </summary>
        /// <returns></returns>
        public static decimal getMaxFiledID()
        {
            decimal maxID = 1;
            string sSql = "select max(to_number(nvl(FieldID,0)))+1 as maxid from Equ_SchemaItems";
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                cn.Open();
                OracleDataReader dReader = OracleDbHelper.ExecuteReader(cn, CommandType.Text, sSql);
                while (dReader.Read())
                {
                    maxID = decimal.Parse(dReader.GetInt32(0).ToString());
                }
                dReader.Close();
            }
            catch (Exception)
            {
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
            return maxID;
        }

        #endregion
    }
}

