using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using System.Data;
using Epower.ITSM.SqlDAL.CommonUtil;
using Epower.DevBase.Organization.Base;

namespace Epower.ITSM.SqlDAL.Customer
{
    public class BR_ExtendsFieldsDP
    {
        public BR_ExtendsFieldsDP() { }

        #region 属性

        #region Id
        private Decimal mId;
        public Decimal ID
        {
            get { return mId; }
            set { mId = value; }
        }
        #endregion

        #region mKeyValue

        private String mKeyValue;
        public String KeyValue
        {
            get { return mKeyValue; }
            set { mKeyValue = value; }
        }
        #endregion

        #region mGroupID

        private decimal mGroupID;
        public decimal GroupID
        {
            get { return mGroupID; }
            set { mGroupID = value; }
        }
        #endregion

        #region mGroupName 标识是事件单还是变更单还是问题单
        private String mGroupName;
        public String GroupName
        {
            get { return mGroupName; }
            set { mGroupName = value; }
        }
        #endregion

        #region mDeleted
        private int mDeleted;
        public int DELETED
        {
            get { return mDeleted; }
            set { mDeleted = value; }
        }
        #endregion
        #endregion


        #region GetReCorded
        public BR_ExtendsFieldsDP GetReCorded(long lngID)
        {
            BR_ExtendsFieldsDP ee = new BR_ExtendsFieldsDP();
            DataTable dt;
            string strSQL = string.Empty;

            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT * FROM Br_Extendsfields WHERE GroupID = " + lngID.ToString();
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);

                foreach (DataRow dr in dt.Rows)
                {
                    ee.ID = Decimal.Parse(dr["id"].ToString());
                    ee.KeyValue = dr["KeyValue"].ToString();
                    ee.GroupName = dr["GroupName"].ToString();
                    ee.GroupID = decimal.Parse(dr["GroupID"].ToString());
                    ee.DELETED = Int32.Parse(dr["Deleted"].ToString());

                }
                return ee;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }

        #endregion

        #region GetDataTable
        public DataTable GetDataTable(decimal groupID)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = string.Format(@"SELECT * FROM BR_ExtendsFields Where  Deleted=0 And GroupID= ") + groupID;
                DataTable dt = new DataTable();
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);

                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }


        public DataTable GetDataTable(string sWhere, string sOrder)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT * FROM BR_ExtendsFields Where 1=1 And Deleted=0 ";
                strSQL += sWhere;
                strSQL += sOrder;
                DataTable dt = new DataTable();
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);

                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }

        public DataTable GetDataTable(string sWhere, string sOrder, int pagesize, int pageindex, ref int rowcount)
        {
            string strWhere = "1=1 And Deleted=0" + sWhere;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "BR_ExtendsFields", "*", sOrder, pagesize, pageindex, strWhere, ref rowcount);
                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }


        #endregion


        public static string GetKeyValue()
        {
            string strSQL;
            string strSchema = "";
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT KeyValue FROM br_extendsfields WHERE Deleted=0 ";
                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                if (dr.Read())
                {
                    strSchema = dr["KeyValue"].ToString().Trim();
                }
                dr.Close();

                return strSchema;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }

        #region InsertRecorded
        public void InsertRecorded(BR_ExtendsFieldsDP br_ExtendsFields)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            string strID = "0";
            try
            {
                //strID = SerialNumber.GetNextval("BR_ExtendsFieldsID");
                strID = EpowerGlobal.EPGlobal.GetNextID("BR_ExtendsFieldsID").ToString();

                br_ExtendsFields.ID = decimal.Parse(strID);
                strSQL = @"insert into BR_ExtendsFields(
                    Id,
                    KeyValue,
                    GroupName,
                    GroupID,
                    deleted
                ) values(" +
                    strID.ToString() + "," +
                    br_ExtendsFields.KeyValue.ToString() + "," +
                    br_ExtendsFields.GroupName.ToString() + "," +
                    br_ExtendsFields.GroupID + "," +
                    br_ExtendsFields.DELETED.ToString() +
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
        public void UpdateRecorded(BR_ExtendsFieldsDP br_ExtendsFields)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE BR_ExtendsFields Set " +
                            " KeyValue = " + br_ExtendsFields.KeyValue + "," +
                            " GroupName = " + br_ExtendsFields.GroupName.ToString() + "," +
                            " Deleted = " + br_ExtendsFields.DELETED.ToString() +
                                " WHERE id = " + br_ExtendsFields.ID.ToString();

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
                string strSQL = "Update BR_ExtendsFields Set Deleted=1  WHERE ID =" + lngID.ToString();
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
        public void DeleteRecorded()
        {
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                string strSQL = "Update BR_ExtendsFields Set Deleted=1";
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


        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="lngSubjectID"></param>
        /// <param name="sSubjectName"></param>
        /// <param name="lngParentID"></param>
        /// <param name="iSortID"></param>
        /// <param name="sRemark"></param>
        /// <param name="sOrgID"></param>
        /// <param name="inherit"></param>
        /// <param name="sSchema"></param>
        /// <param name="ImageUrl"></param>
        /// <returns></returns>
        public static string Save(string sSchema, string groupName, decimal groupID)
        {
            //更新或新增（ＩＤ　＝　０，ＮＡＭＥ　不等于　空的时候新增）
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            long lngNextID = 0;
            long groupids = 0;
            string strID = "";

            cn.Open();
            OracleTransaction trans = cn.BeginTransaction();
            try
            {
                strID = EpowerGlobal.EPGlobal.GetNextID("BR_ExtendsFieldsID").ToString();
                //strID = lngNextID.ToString();
                strSQL = "INSERT INTO Br_ExtendsFields (ID,KeyValue,GroupName,GroupID,Deleted)" +
                    " Values(" +
                    strID + "," +
                    StringTool.SqlQ(sSchema) + "," +
                    StringTool.SqlQ(groupName) + "," +
                    groupID + "," +
                    ((int)eO_Deleted.eNormal).ToString() +
                    ")";

                OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);


                trans.Commit();
                return strID;
            }
            catch
            {

                trans.Rollback();
                return strID;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);


            }
        }

        /// <summary>
        /// 删除指定流程模型下的扩展项
        /// </summary>
        /// <param name="trans">事务对象</param>
        /// <param name="lngFlowModelID">流程模型编号</param>
        public void DeleteExItemListByFlowModelID(OracleTransaction trans, long lngFlowModelID)
        {
            string strSQL = String.Format("UPDATE BR_ExtendsFields SET deleted = 1 WHERE GroupID = {0}", lngFlowModelID);
            OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);
        }


        /// <summary>
        /// 保存扩展项
        /// </summary>
        /// <param name="trans">事务对象</param>
        /// <param name="lngAppID">应用编号</param>
        /// <param name="lngFlowModelID">流程模型编号</param>
        /// <param name="strXML">XML配置串</param>        
        /// <returns></returns>
        public string SaveExItemList(OracleTransaction trans, long lngAppID, long lngFlowModelID, string strXML)
        {
            //更新或新增（ＩＤ　＝　０，ＮＡＭＥ　不等于　空的时候新增）
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            long lngNextID = 0;
            long groupids = 0;
            string strID = "";

            strID = EpowerGlobal.EPGlobal.GetNextID("BR_ExtendsFieldsID").ToString();
            //strID = lngNextID.ToString();
            strSQL = "INSERT INTO Br_ExtendsFields (ID,KeyValue,GroupName,GroupID,AppID,Deleted)" +
                " Values(" +
                strID + "," +
                StringTool.SqlQ(strXML) + ",''," +
                lngFlowModelID.ToString() + "," +
                lngAppID.ToString() + "," +
                ((int)eO_Deleted.eNormal).ToString() +
                ")";

            OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);

            return strID;
        }


    }
}
