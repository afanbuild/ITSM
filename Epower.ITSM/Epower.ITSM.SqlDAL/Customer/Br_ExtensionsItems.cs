using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Epower.DevBase.BaseTools;
using System.Data.OracleClient;
using Epower.ITSM.SqlDAL.CommonUtil;

namespace Epower.ITSM.SqlDAL.Customer
{
    public class Br_ExtensionsItems
    {
        /// <summary>
        /// 
        /// </summary>
        public Br_ExtensionsItems() { }

        #region 属性
        #region Id
        private decimal myId;
        public decimal Id {
            get { return myId;}
            set{myId=value;}
        }
        #endregion

        #region chname 属性名称
        private string chname;
        public string CHNAME {
            get { return chname; }
            set { chname = value; }
        }
        #endregion

        #region FieldID 标识是否在页面中使用，具体我不知道是代表什么...
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

        #region ITEMTYPE 标识是基础知识还是其他的
        private int itemtype;
        public int ITEMTYPE {
            get { return itemtype; }
            set { itemtype = value; }
        }
        #endregion

        #region typeId 标识是事件单还是变更单还是问题单
        private int typeId;
        public int TYPEID {
            get { return typeId; }
            set { typeId = value; }
        }
        #endregion

        #region deleted
        private int deleted;
        public int DELETED {
            get { return deleted; }
            set { deleted = value; }
        }
        #endregion
        #endregion

        #region GetReCorded
        public DataTable GetDataTable(string sWhere, string sOrder)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM br_ExtensionsItems Where 1=1 And Deleted=0 ";
            strSQL += sWhere;
            strSQL += sOrder;
            DataTable dt = new DataTable();
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        public DataTable GetDataTable(string sWhere, string sOrder, int pagesize, int pageindex, ref int rowcount)
        {
            string strWhere = "1=1 And Deleted=0" + sWhere;
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "br_ExtensionsItems", "*", sOrder, pagesize, pageindex, strWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region
        public void InsertRecorded(Br_ExtensionsItems Br_ExtensionsItem)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            string strID = "0";
            try {
                //strID = SerialNumber.GetNextval("Br_ExtensionsItemsID");
                strID = EpowerGlobal.EPGlobal.GetNextID("Br_ExtensionsItemsID").ToString(); 

                Br_ExtensionsItem.Id = decimal.Parse(strID);
                strSQL = @"insert into Br_ExtensionsItems(
                    Id,
                    FieldID,
                    chname,
                    itemtype,
                    typeId,
                    deleted
                ) values(" +
                    strID.ToString()+","+
                    getMaxFiledID().ToString() + "," +
                    StringTool.SqlQ(Br_ExtensionsItem.CHNAME)+","+
                    Br_ExtensionsItem.itemtype.ToString()+","+
                    Br_ExtensionsItem.typeId.ToString()+","+
                    Br_ExtensionsItem.deleted.ToString()+
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
        public void UpdateRecorded(Br_ExtensionsItems pEqu_SchemaItemsDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE Br_ExtensionsItems Set " +
                            " FieldID = " +pEqu_SchemaItemsDP.FieldID+ "," +
                            " CHName = " + StringTool.SqlQ(pEqu_SchemaItemsDP.chname) + "," +
                            " itemType = " + pEqu_SchemaItemsDP.itemtype.ToString() + "," +
                            " typeid=" + pEqu_SchemaItemsDP.itemtype.ToString()+","+
                            " Deleted = " + pEqu_SchemaItemsDP.deleted.ToString() +
                                " WHERE id = " + pEqu_SchemaItemsDP.Id.ToString();

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
                string strSQL = "Update Br_ExtensionsItems Set Deleted=1  WHERE ID =" + lngID.ToString();
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
        #region
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
                    string strSQL = "select * from  Br_ExtensionsItems WHERE ID =" + lngID.ToString() + " and FieldId in ("+strFiledId+")";
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
        #region 得到最大的编号ID

        /// <summary>
        /// 得到最大的编号ID
        /// </summary>
        /// <returns></returns>
        public static decimal getMaxFiledID()
        {
            decimal maxID = 1;
            string sSql = "select max(to_number(FieldID))+1 as maxid from Br_ExtensionsItems";
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
        #region CheckIsRepeat
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FieldID"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool CheckIsRepeat(string FieldID,int typeid, decimal ID)
        {
            string scn = ConfigTool.GetConnectString();
            bool breturn = false;
            string strSQL = @"select ID from Br_ExtensionsItems
							 WHERE FieldID =" + StringTool.SqlQ(FieldID)+" and TypeId="+typeId;
            if (ID != 0)
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
            string strSQL = @"select ID from Br_ExtensionsItems
							 WHERE FieldID =" + StringTool.SqlQ(FieldID);
            if (ID != 0)
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
        /// <returns>Br_ExtensionsItems</returns>
        public Br_ExtensionsItems GetReCorded(string sID)
        {
            Br_ExtensionsItems ee = new Br_ExtensionsItems();
            DataTable dt;

            //2008-05-01 添加SQL缓存依赖的处理方式,减少数据库连接次数

            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                dt = CommSqlCacheHelper.GetDataTableFromCache("brExtensionsItems");

                dt.DefaultView.RowFilter = " FieldID = " + StringTool.SqlQ(sID);

                foreach (DataRowView dvr in dt.DefaultView)
                {
                    ee.Id = Decimal.Parse(dvr.Row["id"].ToString());
                    ee.FieldID = dvr.Row["FieldID"].ToString();
                    ee.chname = dvr.Row["CHName"].ToString();
                    ee.itemtype = Int32.Parse(dvr.Row["itemType"].ToString());
                    ee.deleted = Int32.Parse(dvr.Row["Deleted"].ToString());
                    ee.TYPEID = Int32.Parse(dvr.Row["TypeID"].ToString());
                }
                return ee;
            }
            else
            {

                string strSQL = string.Empty;
                OracleConnection cn = ConfigTool.GetConnection();
                strSQL = "SELECT * FROM Br_ExtensionsItems WHERE  FieldID = " + StringTool.SqlQ(sID);
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                ConfigTool.CloseConnection(cn);
                foreach (DataRow dr in dt.Rows)
                {
                    ee.Id = Decimal.Parse(dr["id"].ToString());
                    ee.FieldID = dr["FieldID"].ToString();
                    ee.chname = dr["CHName"].ToString();
                    ee.itemtype = Int32.Parse(dr["itemType"].ToString());
                    ee.deleted = Int32.Parse(dr["Deleted"].ToString());
                    ee.TYPEID = Int32.Parse(dr["TypeID"].ToString());
                }
                return ee;
            }
        }
        #endregion
        #region GetReCorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>Br_ExtensionsItems</returns>
        public Br_ExtensionsItems GetReCorded(long lngID)
        {
            Br_ExtensionsItems ee = new Br_ExtensionsItems();

            DataTable dt;

            //2008-05-01 添加SQL缓存依赖的处理方式,减少数据库连接次数

            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                dt = CommSqlCacheHelper.GetDataTableFromCache("BrExtensionsItems");//这个dt没有值，会报错，不知道增加的代码在哪里

                dt.DefaultView.RowFilter = " ID = " + lngID.ToString();

                foreach (DataRowView dvr in dt.DefaultView)
                {
                    ee.Id = Decimal.Parse(dvr.Row["id"].ToString());
                    ee.FieldID = dvr.Row["FieldID"].ToString();
                    ee.chname = dvr.Row["CHName"].ToString();
                    ee.itemtype = Int32.Parse(dvr.Row["itemType"].ToString());
                    ee.deleted = Int32.Parse(dvr.Row["Deleted"].ToString());
                    ee.typeId = Int32.Parse(dvr.Row["TYPEId"].ToString());
                }
                return ee;
            }
            else
            {
                string strSQL = string.Empty;
                OracleConnection cn = ConfigTool.GetConnection();
                strSQL = "SELECT * FROM Br_ExtensionsItems WHERE ID = " + lngID.ToString();
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                ConfigTool.CloseConnection(cn);
                foreach (DataRow dr in dt.Rows)
                {
                    ee.Id = Decimal.Parse(dr["id"].ToString());
                    ee.FieldID = dr["FieldID"].ToString();
                    ee.chname = dr["CHName"].ToString();
                    ee.itemtype = Int32.Parse(dr["itemType"].ToString());
                    ee.deleted = Int32.Parse(dr["Deleted"].ToString());
                    ee.typeId = Int32.Parse(dr["TYPEId"].ToString());
                }
                return ee;
            }
        }
        #endregion
    }
}
