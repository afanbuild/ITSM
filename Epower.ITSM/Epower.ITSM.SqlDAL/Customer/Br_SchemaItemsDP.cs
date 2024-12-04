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
    public class Br_SchemaItemsDP
   {
        /// <summary>
        /// 
        /// </summary>
        public Br_SchemaItemsDP()
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

        #endregion

        #region GetReCorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>Equ_SchemaItemsDP</returns>
        public Br_SchemaItemsDP GetReCorded(long lngID)
        {
            Br_SchemaItemsDP ee = new Br_SchemaItemsDP();

            DataTable dt;

               //2008-05-01 添加SQL缓存依赖的处理方式,减少数据库连接次数
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                dt = CommSqlCacheHelper.GetDataTableFromCache("brschemaitems");

                dt.DefaultView.RowFilter = " ID = " + lngID.ToString();

                foreach (DataRowView dvr in dt.DefaultView)
                {
                    ee.ID = Decimal.Parse(dvr.Row["id"].ToString());
                    ee.FieldID = dvr.Row["FieldID"].ToString();
                    ee.CHName = dvr.Row["CHName"].ToString();
                    ee.itemType = Int32.Parse(dvr.Row["itemType"].ToString());
                    ee.Deleted = Int32.Parse(dvr.Row["Deleted"].ToString());


                }
                return ee;

                
            }
            else
            {

               
                string strSQL = string.Empty;

                OracleConnection cn = ConfigTool.GetConnection();
                strSQL = "SELECT * FROM Br_SchemaItems WHERE ID = " + lngID.ToString();
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                ConfigTool.CloseConnection(cn);
                foreach (DataRow dr in dt.Rows)
                {
                    ee.ID = Decimal.Parse(dr["id"].ToString());
                    ee.FieldID = dr["FieldID"].ToString();
                    ee.CHName = dr["CHName"].ToString();
                    ee.itemType = Int32.Parse(dr["itemType"].ToString());
                    ee.Deleted = Int32.Parse(dr["Deleted"].ToString());
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

                dt = CommSqlCacheHelper.GetDataTableFromCache("brschemaitems");

                DataTable dtTemp = dt.Clone();
                //注意            *******      SWHERE 格式的合法性    ******
                dt.DefaultView.RowFilter = " deleted = 0 " + sWhere;

                dtTemp.Rows.Clear();
                if(sOrder != string.Empty)
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
                strSQL = "SELECT * FROM Br_SchemaItems Where 1=1 And Deleted=0 ";
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
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "Br_SchemaItems", "*", sOrder, pagesize, pageindex, strWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region InsertRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pEqu_SchemaItemsDP></param>
        public void InsertRecorded(Br_SchemaItemsDP pEqu_SchemaItemsDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            string strID = "0";
            try
            {
                strID = EpowerGlobal.EPGlobal.GetNextID("Br_SchemaItemsID").ToString();
                pEqu_SchemaItemsDP.ID = decimal.Parse(strID);
                strSQL = @"INSERT INTO Br_SchemaItems(
									id,
									FieldID,
									CHName,
									itemType,
									Deleted
					)
					VALUES( " +
                            strID.ToString() + "," +
                            StringTool.SqlQ(getMaxFiledID().ToString()) + "," +
                            StringTool.SqlQ(pEqu_SchemaItemsDP.CHName) + "," +
                            pEqu_SchemaItemsDP.itemType.ToString() + "," +
                            pEqu_SchemaItemsDP.Deleted.ToString() +
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
        public void UpdateRecorded(Br_SchemaItemsDP pEqu_SchemaItemsDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE Br_SchemaItems Set " +
                                                        " FieldID = " + StringTool.SqlQ(pEqu_SchemaItemsDP.FieldID) + "," +
                            " CHName = " + StringTool.SqlQ(pEqu_SchemaItemsDP.CHName) + "," +
                            " itemType = " + pEqu_SchemaItemsDP.itemType.ToString() + "," +
                            " Deleted = " + pEqu_SchemaItemsDP.Deleted.ToString() +
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

        /// <summary>
        /// 判断是否可以删除
        /// </summary>
        /// <param name="lngID"></param>
        public bool ReturnRecorded(long lngID, string strFiledId,ref string ChName)
        {
            bool isDelete =false;
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                if (strFiledId != "")
                {
                    string strSQL = "select * from  Br_SchemaItems WHERE ID =" + lngID.ToString() + " and FieldId in ("+strFiledId+")";
                    DataTable dt=OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                    if (dt.Rows.Count > 0)
                    {
                        isDelete = true;
                        ChName = dt.Rows[0]["CHName"].ToString();
                    }
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
            return isDelete;
        }
        #endregion

        #region CheckIsRepeat
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FieldID"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool CheckIsRepeat(string FieldID, decimal ID)
        {
            string scn = ConfigTool.GetConnectString();
            bool breturn = false;
            string strSQL = @"select ID from Br_SchemaItems
							 WHERE FieldID =" + StringTool.SqlQ(FieldID);
            if (ID!=0)
                strSQL += " And ID<>" + ID.ToString();

            try
            {
                object pobject = OracleDbHelper.ExecuteScalar(scn, CommandType.Text, strSQL);
                if (pobject != null)
                    breturn = true;
                return breturn;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion 

        #region GetReCorded by fieldid
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>Equ_SchemaItemsDP</returns>
        public Br_SchemaItemsDP GetReCorded(string sID)
        {
            Br_SchemaItemsDP ee = new Br_SchemaItemsDP();
            

            DataTable dt;

               //2008-05-01 添加SQL缓存依赖的处理方式,减少数据库连接次数
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                dt = CommSqlCacheHelper.GetDataTableFromCache("brschemaitems");

                dt.DefaultView.RowFilter = " FieldID = " + StringTool.SqlQ(sID);

                foreach (DataRowView dvr in dt.DefaultView)
                {
                    ee.ID = Decimal.Parse(dvr.Row["id"].ToString());
                    ee.FieldID = dvr.Row["FieldID"].ToString();
                    ee.CHName = dvr.Row["CHName"].ToString();
                    ee.itemType = Int32.Parse(dvr.Row["itemType"].ToString());
                    ee.Deleted = Int32.Parse(dvr.Row["Deleted"].ToString());


                }
                return ee;


            }
            else
            {

                string strSQL = string.Empty;
                OracleConnection cn = ConfigTool.GetConnection();
                strSQL = "SELECT * FROM Br_SchemaItems WHERE  FieldID = " + StringTool.SqlQ(sID);
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                ConfigTool.CloseConnection(cn);
                foreach (DataRow dr in dt.Rows)
                {
                    ee.ID = Decimal.Parse(dr["id"].ToString());
                    ee.FieldID = dr["FieldID"].ToString();
                    ee.CHName = dr["CHName"].ToString();
                    ee.itemType = Int32.Parse(dr["itemType"].ToString());
                    ee.Deleted = Int32.Parse(dr["Deleted"].ToString());
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

                dt = CommSqlCacheHelper.GetDataTableFromCache("brschemaitems");
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
                    strSQL = "SELECT * FROM Br_SchemaItems WHERE deleted = 0";
                }
                else
                {
                    strSQL = "SELECT * FROM Br_SchemaItems WHERE deleted = 0 AND itemType = " + iType.ToString();
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
        /// 
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

                dt = CommSqlCacheHelper.GetDataTableFromCache("brschemaitems");
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
                    strSQL = "SELECT * FROM Br_SchemaItems WHERE deleted = 0";
                }
                else
                {
                    strSQL = "SELECT * FROM Br_SchemaItems WHERE deleted = 0 AND itemType = " + iType.ToString();
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
        public Hashtable GetItemsByID(int iType,decimal id)
        {
            Hashtable ht = new Hashtable();

            DataTable dt;

            //2008-05-01 添加SQL缓存依赖的处理方式,减少数据库连接次数
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                dt = CommSqlCacheHelper.GetDataTableFromCache("brschemaitems");
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
                    strSQL = "SELECT * FROM Br_SchemaItems WHERE deleted = 0 and ID=" + id;
                }
                else
                {
                    strSQL = "SELECT * FROM Br_SchemaItems WHERE deleted = 0 AND itemType = " + iType.ToString() + "and ID=" + id;
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
            string sSql = "select max(to_number(FieldID))+1 as maxid from Br_SchemaItems";
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
            catch (Exception ex)
            {
                string s = ex.Message;     
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


