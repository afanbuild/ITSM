using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Epower.DevBase.BaseTools;
using System.Data.OracleClient;
using Epower.ITSM.SqlDAL.CommonUtil;
using System.Collections;

namespace Epower.ITSM.SqlDAL.Customer
{
    public class Br_ExtensionsItemsDP
    {
        /// <summary>
        /// 
        /// </summary>
        public Br_ExtensionsItemsDP() { }

        #region 属性

        #region Id
        private Decimal mId;
        public Decimal ID
        {
            get { return mId; }
            set { mId = value; }
        }
        #endregion

        #region chname 属性名称

        private String mChname;
        public String CHNAME
        {
            get { return mChname; }
            set { mChname = value; }
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

        #region ITEMTYPE 标识是基础知识还是其他的


        private Int32 mItemType;
        public Int32 ITEMTYPE
        {
            get { return mItemType; }
            set { mItemType = value; }
        }
        #endregion

        #region groupId 标识是事件单还是变更单还是问题单
        private Int32 mGroupId;
        public Int32 GROUPID
        {
            get { return mGroupId; }
            set { mGroupId = value; }
        }
        #endregion

        #region groupName
        private String mGroupNmae;
        public String GroupName
        {
            get { return mGroupNmae; }
            set { mGroupNmae = value; }
        }
        #endregion

        #region deleted
        private int mDeleted;
        public int DELETED
        {
            get { return mDeleted; }
            set { mDeleted = value; }
        }
        #endregion
        #endregion

        #region GetReCorded


        public Br_ExtensionsItemsDP GetReCorded(long lngID)
        {
            Br_ExtensionsItemsDP ee = new Br_ExtensionsItemsDP();

            DataTable dt;


            string strSQL = string.Empty;

            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM br_extensionsitems WHERE ID = " + lngID.ToString();
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            foreach (DataRow dr in dt.Rows)
            {
                ee.ID = Decimal.Parse(dr["id"].ToString());
                ee.FieldID = dr["FieldID"].ToString();
                ee.CHNAME = dr["CHName"].ToString();
                ee.ITEMTYPE = Int32.Parse(dr["itemType"].ToString());
                ee.DELETED = Int32.Parse(dr["Deleted"].ToString());
                ee.GROUPID = Int32.Parse(dr["groupID"].ToString());
            }
            return ee;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>Equ_SchemaItemsDP</returns>
        public Br_ExtensionsItemsDP GetReCorded(string sID)
        {
            Br_ExtensionsItemsDP ee = new Br_ExtensionsItemsDP();
            DataTable dt;
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM br_extensionsitems WHERE  FieldID = " + StringTool.SqlQ(sID);
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            foreach (DataRow dr in dt.Rows)
            {
                ee.ID = Decimal.Parse(dr["id"].ToString());
                ee.FieldID = dr["FieldID"].ToString();
                ee.CHNAME = dr["CHName"].ToString();
                ee.ITEMTYPE = Int32.Parse(dr["itemType"].ToString());
                ee.DELETED = Int32.Parse(dr["Deleted"].ToString());
                ee.GROUPID = Int32.Parse(dr["groupID"].ToString());
                ee.GroupName = dr["GroupName"].ToString();
            }
            return ee;

        }
        #endregion

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


        #region InsertRecorded
        public void InsertRecorded(Br_ExtensionsItemsDP Br_ExtensionsItem)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            string strID = "0";
            try
            {
                //strID = SerialNumber.GetNextval("Br_ExtensionsItemsID");
                strID = EpowerGlobal.EPGlobal.GetNextID("Br_ExtensionsItemsID").ToString();

                Br_ExtensionsItem.ID = decimal.Parse(strID);
                strSQL = @"insert into Br_ExtensionsItems(
                    Id,
                    FieldID,
                    chname,
                    itemtype,
                    groupId,
                    deleted,
                    groupName
                ) values(" +
                    strID.ToString() + "," +
                    Br_ExtensionsItem.FieldID.ToString() + "," +
                    StringTool.SqlQ(Br_ExtensionsItem.CHNAME) + "," +
                    Br_ExtensionsItem.ITEMTYPE.ToString() + "," +
                    Br_ExtensionsItem.GROUPID.ToString() + "," +
                    Br_ExtensionsItem.DELETED.ToString() + "," +
                    StringTool.SqlQ(Br_ExtensionsItem.GroupName) +
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
        public void UpdateRecorded(Br_ExtensionsItemsDP pEqu_SchemaItemsDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE Br_ExtensionsItems Set " +
                            " FieldID = " + pEqu_SchemaItemsDP.FieldID + "," +
                            " CHName = " + StringTool.SqlQ(pEqu_SchemaItemsDP.CHNAME) + "," +
                            " itemType = " + pEqu_SchemaItemsDP.ITEMTYPE.ToString() + "," +
                            " groupId=" + pEqu_SchemaItemsDP.GROUPID.ToString() + "," +
                            " groupName=" + StringTool.SqlQ(pEqu_SchemaItemsDP.GroupName) + "," +
                            " Deleted = " + pEqu_SchemaItemsDP.DELETED.ToString() +
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
        public bool ReturnRecorded(long lngID, string strFiledId, ref string ChName)
        {
            bool isDelete = false;
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                if (strFiledId != "")
                {
                    string strSQL = "select * from  Br_ExtensionsItems WHERE ID =" + lngID.ToString() + " and FieldId in (" + strFiledId + ")";
                    DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
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
                    if (!string.IsNullOrEmpty(dReader["maxid"].ToString()))
                    {
                        maxID = decimal.Parse(dReader["maxid"].ToString());
                    }

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
        public bool CheckIsRepeat(string FieldID, int GroupID, decimal ID)
        {
            string scn = ConfigTool.GetConnectString();
            bool breturn = false;
            string strSQL = @"select ID from Br_ExtensionsItems
							 WHERE FieldID =" + StringTool.SqlQ(FieldID) + " and GroupID=" + GroupID;
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
        /// <summary>
        /// 检查是否重复

        /// </summary>
        /// <param name="FieldID"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool CheckIsRepeatName(string FieldName, decimal ID, ref Br_ExtensionsItemsDP Schema)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            bool breturn = false;
            string strSQL = @"select ID from br_extensionsitems
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

        #region  根据GroupID找到GroupName
        public string GetGroupName(int strFlowID)
        {

            if (strFlowID == 0)
                return "";

            string Name = "";
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                string sSql = string.Format("Select APPName from es_app where APPID=" + strFlowID);
                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, sSql);
                while (dr.Read())
                {
                    Name = dr["APPName"].ToString();
                }
                dr.Close();
            }
            finally { ConfigTool.CloseConnection(cn); }

            return Name;
        }
        #endregion

        public Hashtable GetAllFieldsHistory()
        {
            return GetAllFieldsHistory(-1);
        }
        public Hashtable GetAllFields()
        {
            return GetAllFields(-1);
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


            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            if (iType == -1)
            {
                strSQL = "SELECT * FROM Br_Extensionsitems ";
            }
            else
            {
                strSQL = "SELECT * FROM Br_Extensionsitems WHERE  itemType = " + iType.ToString();
            }
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            foreach (DataRow dr in dt.Rows)
            {
                ht.Add(dr["FieldID"].ToString(), dr["CHName"].ToString());
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


            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            if (iType == -1)
            {
                strSQL = "SELECT * FROM Br_Extensionsitems WHERE deleted = 0";
            }
            else
            {
                strSQL = "SELECT * FROM Br_Extensionsitems WHERE deleted = 0 AND itemType = " + iType.ToString();
            }
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            foreach (DataRow dr in dt.Rows)
            {
                ht.Add(dr["FieldID"].ToString(), dr["CHName"].ToString());
            }

            return ht;
        }


        /// <summary>
        /// 删除指定流程模型下的扩展项列表
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowModelID"></param>
        public void DeleteExItemListByFlowModelID(OracleTransaction trans, long lngFlowModelID)
        {
            string strSQL = String.Format("UPDATE Br_ExtensionsItems SET Deleted = 1 WHERE GroupID = {0} ", lngFlowModelID);
            OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);
        }


        /// <summary>
        /// 保存扩展项信息
        /// </summary>
        /// <param name="trans">事务对象</param>
        /// <param name="lngAppID">应用编号</param>
        /// <param name="lngFlowModelID">流程模型编号</param>
        /// <param name="brExItem">扩展项对象</param>
        public void SaveExItemList(OracleTransaction trans, long lngAppID, long lngFlowModelID, Br_ExtensionsItemsDP brExItem)
        {
            string strSQL = string.Empty;
            string strID = "0";

            //strID = SerialNumber.GetNextval("Br_ExtensionsItemsID");
            strID = EpowerGlobal.EPGlobal.GetNextID("Br_ExtensionsItemsID").ToString();

            strSQL = @"insert into Br_ExtensionsItems(
                    Id,
                    FieldID,
                    chname,
                    itemtype,
                    groupId,
                    deleted,
                    groupName,
                    AppID
                ) values(" +
                strID.ToString() + "," +
                brExItem.FieldID.ToString() + "," +
                StringTool.SqlQ(brExItem.CHNAME) + "," +
                brExItem.ITEMTYPE.ToString() + "," +
                brExItem.GROUPID.ToString() + "," +
                brExItem.DELETED.ToString() + "," +
                StringTool.SqlQ(brExItem.GroupName) + "," +
                lngAppID +
            ")";
            OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);

        }


        /// <summary>
        /// 取有扩展项的流程模型列表
        /// </summary>
        /// <param name="strWhere">动态SQL查询脚本</param>
        /// <returns></returns>
        public DataTable GetExtFlowModelList(String strWhere)
        {

            OracleConnection conn = null;
            try
            {
                conn = ConfigTool.GetConnection();

                String strSql = String.Format(@"select appid, flowmodelid, appname, flowmodelname, extcount from v_ex_item_flowmodel
                                                WHERE 1=1 and deleted = 0 {0}", strWhere);

                return OracleDbHelper.ExecuteDataTable(conn, CommandType.Text, strSql);
            }
            finally
            {
                if (conn != null)
                    ConfigTool.CloseConnection(conn);
            }

        }

    }
}
