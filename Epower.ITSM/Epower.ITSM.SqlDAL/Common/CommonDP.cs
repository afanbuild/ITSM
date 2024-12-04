/*******************************************************************
 *
 * Description:ֱ��ִ��SQL��
 * 
 * 
 * Create By  :
 * Create Date:2008��7��30��
 * *****************************************************************/
using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using EpowerGlobal;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;
using System.Text;
using System.Xml;

namespace Epower.ITSM.SqlDAL
{
    public class CommonDP
    {
        /// <summary>
        /// ִ��Sql
        /// </summary>
        /// <param name="sSql"></param>
        /// <returns></returns>
        public static void ExcuteSql(string sSql)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, sSql.ToString());
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


        /// <summary>
        /// ִ��SQL
        /// </summary>
        /// <param name="sSql"></param>
        public static DataTable ExcuteSqlTable(string sSql)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                return OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sSql.ToString());
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

        /// <summary>
        /// ִ��SQL
        /// </summary>
        /// <param name="pOracleTransaction"></param>
        /// <param name="sSql"></param>
        /// <returns></returns>
        public static DataTable ExcuteSqlTable(OracleTransaction pOracleTransaction, string sSql)
        {
            try
            {
                return OracleDbHelper.ExecuteDataset(pOracleTransaction, CommandType.Text, sSql.ToString()).Tables[0];
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
            }
        }

        /// <summary>
        /// ȡ��ϵͳ�������в���ֵ
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static string GetConfigValue(string NodesName, string Key)
        {
            string sValue = string.Empty;
            try
            {
                sValue = ConfigHelper.GetParameterValue("systemconfig", Key);
                return sValue;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }


        /// <summary>
        /// �����ʲ�״̬����

        /// </summary>
        /// <param name="strValue">����Id</param>        
        /// <returns></returns>
        public static bool AddEquStatueType(string strValue)
        {
            bool isTrue = false;

            string sqlSelect = @"select * from es_catalog where parentId='1018' and  catalogName=" + StringTool.SqlQ(strValue) + " and deleted=0";
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sqlSelect);
                if (dt.Rows.Count == 0)
                {
                    string Sqlstr = @"
                                declare @sequence nvarchar(20)
                                declare @sort nvarchar(20)
                                select @sequence=nvl(currentValue,0)+nvl(step,1)  from Ts_Sequence where name='Catalog_ID'
                                update Ts_Sequence set currentValue=@sequence where  name='Catalog_ID'
                                select @sort=max(SortID)+1 from es_catalog where parentId='1018' 

                                insert into es_catalog(FullID,OrgID,CatalogId,ParentID,CatalogName,sortId,Remark,UpdateTime,deleted) 
                                values('0010180'+@sequence,'1',@sequence,1018," + StringTool.SqlQ(strValue) + ",@sort,'',sysdate,0)";

                    int rtRows = OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, Sqlstr);


                    if (rtRows > 0)
                    {
                        isTrue = true;
                    }
                    else
                    {
                        isTrue = false;
                    }
                }
                else
                {
                    isTrue = true;
                }

                return isTrue;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static string SelectEquStatueType(string strValue)
        {
            string catalogID = "";
            string sqlSelect = @"select * from es_catalog where parentId='1018' and  catalogName=" + StringTool.SqlQ(strValue) + " and deleted=0";
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sqlSelect);
                if (dt.Rows.Count > 0)
                {
                    catalogID = dt.Rows[0]["catalogId"].ToString();
                }

                return catalogID;
            }
            finally { ConfigTool.CloseConnection(cn); }

        }

        #region ���ݵ�¼���ƻ�ȡ�û�ID
        /// <summary>
        /// ���ݵ�¼���ƻ�ȡ�û�ID
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string getUserId(string name)
        {
            string userID = "0";
            string sqlSelect = @"select UserId from ts_user where LoginName=" + StringTool.SqlQ(name) + " and deleted=0";
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sqlSelect);
                if (dt.Rows.Count > 0)
                {
                    userID = dt.Rows[0]["UserId"].ToString();
                }

                return userID;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion

        public static string getMastCustID(string strValue)
        {
            string sqlSelect = " select ID from Br_MastCustomer where shortname =" + StringTool.SqlQ(strValue);

            string RtnValue = "0";
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sqlSelect);
                if (dt.Rows.Count > 0)
                {
                    RtnValue = dt.Rows[0]["ID"].ToString();
                }

                return RtnValue;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }


        public static string getCustID(string strValue)
        {
            string sqlSelect = " select ID from Br_ECustomer where shortname =" + StringTool.SqlQ(strValue);

            string RtnValue = "0";
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sqlSelect);
                if (dt.Rows.Count > 0)
                {
                    RtnValue = dt.Rows[0]["ID"].ToString();
                }
                else
                {
                    Br_ECustomerDP ee = new Br_ECustomerDP();
                    ee.Deleted = (int)Epower.ITSM.Base.eRecord_Status.eNormal;
                    ee.RegTime = DateTime.Now;
                    ee.UpdateTime = DateTime.Now;
                    ee.ShortName = strValue;
                    ee.InsertRecorded(ee);
                    RtnValue = ee.ID.ToString();
                }
                return RtnValue;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }


        public static bool AddMastCustTomer(string strValue)
        {

            bool isTrue = false;
            string sqlSelect = " select ID from Br_MastCustomer where shortname =" + StringTool.SqlQ(strValue);

            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sqlSelect);
                if (dt.Rows.Count == 0)
                {

//                    string Sqlstr = @"  declare @maxId  int 
//                select @maxId=max(Currentvalue)+1 from Ts_Sequence where name='Br_MastCustomerID'
//                update Ts_Sequence set Currentvalue=@maxId where  name='Br_MastCustomerID'
                    //                insert into Br_MastCustomer (id,shortname)values (@maxId," + StringTool.SqlQ(strValue) + ")";

                    #region ����ʵ�ַ�ʽ yxq 2014-05-26
                    long lngID = EpowerGlobal.EPGlobal.GetNextID("Br_MastCustomerID");
                    string Sqlstr = @"  insert into Br_MastCustomer (id,shortname)values (" + lngID.ToString() + "," + StringTool.SqlQ(strValue) + ")";
                    #endregion

                    int rtRows = OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, Sqlstr);


                    if (rtRows > 0)
                    {
                        isTrue = true;
                    }
                    else
                    {
                        isTrue = false;
                    }
                }
                else
                {
                    isTrue = true;
                }

                return isTrue;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }


        #region �����������ƺ�rootID�ж�����ID�Ƿ����
        /// <summary>
        /// �����������ƺ�rootID�ж�����ID�Ƿ����
        /// </summary>
        /// <param name="strRootID"></param>
        /// <param name="strCatalogName"></param>
        /// <returns></returns>
        public static bool IsExistCataID(string strRootID, string strCatalogName)
        {
            bool strRet = false;
            string strSql = "select CatalogID from es_catalog where deleted = 0 and CatalogName = " + StringTool.SqlQ(strCatalogName) + " and ParentID = " + strRootID;

            DataTable dt = CommonDP.ExcuteSqlTable(strSql);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    strRet = true;
                }
            }

            return strRet;
        }
        #endregion

        #region �����������ƺ�rootID�õ�����ID
        /// <summary>
        /// �����������ƺ�rootID�õ�����ID
        /// </summary>
        /// <param name="strRootID"></param>
        /// <param name="strCatalogName"></param>
        /// <returns></returns>
        public static string GetCataIDByNameAndRoot(string strRootID, string strCatalogName)
        {
            string strRet = string.Empty;
            string strSql = "select CatalogID from es_catalog where deleted = 0 and CatalogName = " + StringTool.SqlQ(strCatalogName) + " and ParentID = " + strRootID;

            DataTable dt = CommonDP.ExcuteSqlTable(strSql);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    strRet = dt.Rows[0]["CatalogID"].ToString();
                }
            }

            return strRet;
        }
        #endregion

        /// <summary>
        /// �����ʲ����
        /// </summary>
        /// <param name="strValue">����Id</param>        
        /// <returns></returns>
        public static bool AddEqu_Category(string strValue)
        {
            bool isTrue = false;

            string sqlSelect = @"select * from Equ_Category where catalogName=" + StringTool.SqlQ(strValue) + " and deleted=0";
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sqlSelect);
                if (dt.Rows.Count == 0)
                {
                    isTrue = false;
                    //                string Sqlstr = @"
                    //                                declare @sequence nvarchar(20)
                    //                                declare @sort nvarchar(20)
                    //                                declare @configureSchema nvarchar(600)
                    //                                select @configureSchema=convert(nvarchar(600),configureSchema) from Equ_Category where catalogid=1
                    //                                select @sequence=nvl(currentValue,0)+nvl(step,1)  from Ts_Sequence where name='Equ_CategoryID'
                    //                                update Ts_Sequence set currentValue=@sequence where  name='Equ_CategoryID'
                    //                                select @sort=max(SortID)+1 from Equ_Category where parentId='1' 
                    //                                insert into Equ_Category(CatalogId,FullID,OrgID,ParentID,CatalogName,sortId,Remark,configureSchema,inheritschema,deleted,imageUrl) 
                    //                                values(@sequence,'0'+@sequence,'1',1," + StringTool.SqlQ(strValue) + ",@sort,'������Ϣ',@configureSchema,1,0,'')";

                    //                int rtRows = OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, Sqlstr);

                    //                //ǿ�ƻ���ʧЧ
                    //                System.Web.HttpRuntime.Cache.Insert("CommCacheValidEquCategory", false);

                    //                if (rtRows > 0)
                    //                {
                    //                    isTrue = true;
                    //                }
                    //                else
                    //                {
                    //                    isTrue = false;
                    //                }
                }
                else
                {
                    isTrue = true;
                }
                return isTrue;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }

        /// <summary>
        /// �����ʲ����
        /// </summary>
        /// <param name="strValue">����Id</param>        
        /// <returns></returns>
        public static bool AddEqu_CateLists(long catalogID, string strValue)
        {
            bool isTrue = false;

            string sqlSelect = @"select * from Equ_CateLists where catalogID =" + catalogID.ToString() + " and ListName =" + StringTool.SqlQ(strValue) + " and deleted=0";

            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sqlSelect);
                if (dt.Rows.Count == 0)
                {

                    Equ_CateListsDP CateLists = new Equ_CateListsDP();
                    CateLists.CatalogID = decimal.Parse(catalogID.ToString());
                    CateLists.CatalogName = Equ_SubjectDP.GetSubjectName(long.Parse(CateLists.CatalogID.ToString()));
                    CateLists.ListName = strValue;
                    CateLists.InsertRecorded(CateLists);
                    isTrue = true;
                }
                else
                {
                    isTrue = true;
                }

                return isTrue;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }

        /// <summary>
        /// ����ʲ�Ŀ¼��id
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static string getCateListsID(long catalogID, string strValue)
        {
            string sqlSelect = " select ID from Equ_CateLists where catalogID =" + catalogID.ToString() + " and ListName =" + StringTool.SqlQ(strValue) + " and deleted=0";

            string RtnValue = "0";
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sqlSelect);
                if (dt.Rows.Count > 0)
                {
                    RtnValue = dt.Rows[0]["ID"].ToString();
                }

                return RtnValue;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }

        /// <summary>
        /// �����ʲ�������
        /// </summary>
        /// <param name="strValue">����Id</param>        
        /// <returns></returns>
        public static bool AddEqu_SchemaItems(string strValue)
        {
            bool isTrue = false;

            string sqlSelect = @"  select * from Equ_SchemaItems where CHname=" + StringTool.SqlQ(strValue);
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sqlSelect);
                if (dt.Rows.Count == 0)
                {
                    /*
                     * guoch 2014
                     * �÷�����Sqlserver�е�д�������÷�������
                     */
//                    string Sqlstr = @"
//                                declare @sequence nvarchar(20)
//                                declare @maxFieldId nvarchar(20)
//                                select @maxFieldId=max(to_number(nvl(FieldID,0)))+1 from Equ_SchemaItems
//                                select @sequence=nvl(currentValue,0)+nvl(step,1)  from Ts_Sequence where name='Equ_SchemaItemsID'
//                                update Ts_Sequence set currentValue=@sequence where  name='Equ_SchemaItemsID'                                
//                                insert into Equ_SchemaItems(id,Fieldid,ChName,itemType,deleted) 
//                                values(@sequence,@maxFieldId," + StringTool.SqlQ(strValue) + ",0,0)";

                    string strSQL = "select (max(to_number(nvl(FieldID,0)))+1) as maxFieldId from Equ_SchemaItems";
                    DataTable dtMax = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                    int intMax = 0;
                    if (dtMax.Rows.Count > 0)
                    {
                        intMax = int.Parse(dtMax.Rows[0]["maxFieldId"].ToString());
                    }

                    strSQL = "select (nvl(currentValue,0)+nvl(step,1)) as sequence  from Ts_Sequence where name='Equ_SchemaItemsID'";
                    DataTable dtsequence = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                    int intsequence = 0;
                    if (dtsequence.Rows.Count > 0)
                    {
                        intsequence = int.Parse(dtsequence.Rows[0]["sequence"].ToString());
                    }

                    strSQL = "update Ts_Sequence set currentValue=" + intsequence + " where  name='Equ_SchemaItemsID'";
                    OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSQL);

                    strSQL = "insert into Equ_SchemaItems(id,Fieldid,ChName,itemType,deleted) values(" + intsequence + "," + intMax + "," + StringTool.SqlQ(strValue) + ",0,0)";

                    int rtRows = OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSQL);


                    if (rtRows > 0)
                    {
                        isTrue = true;
                    }
                    else
                    {
                        isTrue = false;
                    }
                }
                else
                {
                    isTrue = true;
                }

            }
            finally { ConfigTool.CloseConnection(cn); }

            return isTrue;
        }



        #region �����ʲ�������洢���̰桿 - 2014-03-13 @������

        /// <summary>
        /// �����ʲ�������洢���̰桿
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool AddEqu_SchemaItemsWithStoredProc(string strValue)
        {
            bool isTrue = false;

            string sqlSelect = @"  select * from Equ_SchemaItems where CHname=" + StringTool.SqlQ(strValue);
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sqlSelect);
                if (dt.Rows.Count == 0)
                {

                    int rtRows = OracleDbHelper.ExecuteNonQuery(cn, CommandType.StoredProcedure,
                        "proc_addEqu_schemaItems", new OracleParameter("p_key", strValue));


                    if (rtRows > 0)
                    {
                        isTrue = true;
                    }
                    else
                    {
                        isTrue = false;
                    }
                }
                else
                {
                    isTrue = true;
                }

            }
            finally { ConfigTool.CloseConnection(cn); }

            return isTrue;
        }

        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static string SelectEqu_SchemaItems(string strValue)
        {
            string ID = "";
            string sqlSelect = @"select * from Equ_SchemaItems where CHname=" + StringTool.SqlQ(strValue);
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sqlSelect);
                if (dt.Rows.Count > 0)
                {
                    ID = dt.Rows[0]["fieldId"].ToString();

                }

                return ID;
            }
            finally { ConfigTool.CloseConnection(cn); }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static string SelectEqu_SchemaItemsType(string strValue, ref string ItemType)
        {
            string ID = "";
            string sqlSelect = @"select * from Equ_SchemaItems where CHname=" + StringTool.SqlQ(strValue);
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sqlSelect);
                if (dt.Rows.Count > 0)
                {
                    ID = dt.Rows[0]["fieldId"].ToString();
                    ItemType = dt.Rows[0]["ItemType"].ToString();
                }

                return ID;
            }
            finally { ConfigTool.CloseConnection(cn); }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static string SelectProvide(string strValue)
        {
            string catalogID = "0";
            string sqlSelect = @"select * from Pro_ProvideManage where name =" + StringTool.SqlQ(strValue) + " and deleted=0";
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sqlSelect);
                if (dt.Rows.Count > 0)
                {
                    catalogID = dt.Rows[0]["ID"].ToString();

                }

                return catalogID;
            }
            finally { ConfigTool.CloseConnection(cn); }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static string SelectEqu_Category(string strValue)
        {
            string catalogID = "";
            string sqlSelect = @"select * from Equ_Category where catalogName=" + StringTool.SqlQ(strValue) + " and deleted=0";
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sqlSelect);
                if (dt.Rows.Count > 0)
                {
                    catalogID = dt.Rows[0]["catalogId"].ToString();

                }

                return catalogID;
            }
            finally { ConfigTool.CloseConnection(cn); }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string SelectEqu_CategoryconfigureSchema(string id)
        {
            string configXml = "";
            string sqlSelect = @"select * from Equ_Category where parentId='1' and  catalogid=" + StringTool.SqlQ(id) + " and deleted=0";
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sqlSelect);
                if (dt.Rows.Count > 0)
                {
                    configXml = dt.Rows[0]["configureSchema"].ToString();

                }

                return configXml;
            }
            finally { ConfigTool.CloseConnection(cn); }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="strValue"></param>
        public static void UpdateEqu_CategoryconfigureSchema(string id, string strValue)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            cn.Open();
            OracleTransaction trans = cn.BeginTransaction();
            try
            {
                string strSQL = "update Equ_Category set configureschema=:configureschema where catalogid=" + id + " and deleted=0";
                OracleCommand cmdCST = new OracleCommand(strSQL, trans.Connection, trans);
                cmdCST.Parameters.Add("configureschema", OracleType.Clob).Value = strValue;
                cmdCST.ExecuteNonQuery();

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }

        }

        #region ִ��sql����ѯ��ҳ��
        /// <summary>
        /// ִ��SQL����ѯ��ҳ��
        /// </summary>
        /// <param name="sTableName"></param>
        /// <param name="sFieldNames"></param>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="rowcount"></param>
        /// <returns></returns>
        public static DataTable ExcuteSqlTablePage(string sTableName, string sFieldNames, string sWhere, string sOrder, int pagesize, int pageindex, ref int rowcount)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                return OracleDbHelper.ExecuteDataTable(cn, sTableName, sFieldNames, sOrder, pagesize, pageindex, sWhere, ref rowcount);
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

        #region �������ID����չ��ID��ȡ��Ӧ�������б�����
        /// <summary>
        /// �������ID����չ��ID��ȡ��Ӧ�������б�����
        /// </summary>
        /// <param name="strCatalogID">�ʲ����ID</param>
        /// <param name="strItemID">��չ��ID</param>
        /// <returns></returns>
        public static DataTable GetCatasByParentID(string strCatalogID, string strItemID)
        {
            string strSchemaXml = string.Empty;         //��չ��xmlֵ
            string strItemValue = string.Empty;         //�����б�����ID
            string strSql = "select * from Equ_Category where deleted = 0 and CatalogID =" + strCatalogID;

            DataTable dt = CommonDP.ExcuteSqlTable(strSql);

            if (dt != null && dt.Rows.Count > 0)
            {
                strSchemaXml = dt.Rows[0]["ConfigureSchema"].ToString();
            }

            if (strSchemaXml != "")
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(strSchemaXml);

                XmlNodeList xmlNodes = xmlDoc.SelectNodes("EquScheme/BaseItem/AttributeItem");
                foreach (XmlNode node in xmlNodes)
                {
                    if (node.Attributes["ID"].Value == strItemID)
                    {
                        strItemValue = node.Attributes["Default"].Value;            //�����б�����IDֵ
                    }
                }
            }

            if (strItemValue != "" && strItemValue != "0")
            {
                strSql = "select * from es_catalog where deleted = 0 and ParentID = " + strItemValue;

                dt = CommonDP.ExcuteSqlTable(strSql);
            }
            else
            {
                strSql = "select * from es_catalog where 1 = 2";

                dt = CommonDP.ExcuteSqlTable(strSql);
            }

            return dt;
        }
        #endregion


        #region ����Ӧ��ID��ȡ��Ӧ������������������ģ�� ����ǰ 2013-03-21
        /// <summary>
        /// ����Ӧ��ID��ȡ��Ӧ������������������ģ��
        /// </summary>
        /// <param name="lngAppID">Ӧ��ID</param>
        /// <returns></returns>
        public static DataTable GetAllFlowModelByAppID(long lngAppID)
        {
            string strSQL = "select * from es_flowmodel where status=1 and AppID=" + lngAppID.ToString();
            DataTable dt = ExcuteSqlTable(strSQL);
            return dt;
        }
        #endregion

        #region ����AppID��OFlowModelID��ȡ���л�����Ϣ ����ǰ 2013-03-21
        /// <summary>
        /// ����AppID��OFlowModelID��ȡ���л�����Ϣ
        /// </summary>
        /// <param name="lngAppID">Ӧ��ID</param>
        /// <param name="lngOFlowModelID">����ģ��ID</param>
        /// <returns></returns>
        public static DataTable GetAllNodes(long lngAppID, long lngOFlowModelID)
        {
            DataTable dt = new DataTable();
            string strSql = "select * from es_nodemodel where FLowMOdelID = (select max(FlowModelID) from es_flowmodel where appid="
                + lngAppID + " and OFlowModelID =" + lngOFlowModelID + " and type<>10 and type<>60)";
            dt = ExcuteSqlTable(strSql);
            return dt;
        }
        #endregion

        #region ��ѯ�����ҵǼ����� ����ǰ 2013-04-19
        /// <summary>
        /// ��ѯ�����ҵǼ�����
        /// </summary>
        /// <param name="sWhere">where���� �� 1=1 and deleted=0</param>
        /// <param name="sOrder">�������� �� order by FlowID </param>
        /// <param name="pagesize">ÿҳ��ʾ��С</param>
        /// <param name="pageindex">��ǰҳ</param>
        /// <param name="rowcount">�ܼ�¼��</param>
        /// <returns></returns>
        public static DataTable GetRegEventData(string sWhere, string sOrder, int pagesize, int pageindex, ref int rowcount)
        {
            DataTable dt = CommonDP.ExcuteSqlTablePage("v_myRegEvent", "*", sWhere, sOrder, pagesize, pageindex, ref rowcount);
            return dt;
        }
        #endregion

        #region ��ȡ��ǰ�����û����MessageID��������������תʱ���ݲ��� ����ǰ 2013-04-22
        /// <summary>
        /// ��ȡ��ǰ�����û����MessageID��������������תʱ���ݲ���
        /// </summary>
        /// <param name="lngFlowID">����ID</param>
        /// <param name="lngUserID">�û�ID</param>
        /// <returns></returns>
        public static long GetMessageIDForSelf(long lngFlowID, long lngUserID)
        {

            // ���û�����ʱ,��ȡ����δ�����messageid
            //            ��û�����δ����messageid ʱ��ȡ��� �Ѿ� messageid ,���鿴
            // ���û�δ����ʱ ��ȡ��� �Ѿ� messageid ,���鿴


            string strSQL = "";
            long lngMessageID = 0;
            int iCount = 0;

            strSQL = "SELECT max(messageid)  FROM es_message WHERE flowid = " + lngFlowID.ToString() + " and receiverid = " + lngUserID.ToString() +
                " and status = 20 " +
                " union all " +
                " SELECT max(messageid) FROM es_message WHERE flowid = " + lngFlowID.ToString() + " AND status = 10 " +
               " union all " +
                " SELECT max(messageid)  FROM es_message WHERE flowid = " + lngFlowID.ToString();

            string scn = ConfigTool.GetConnectString();
            using (OracleDataReader dr = OracleDbHelper.ExecuteReader(scn, CommandType.Text, strSQL))
            {
                while (dr.Read())
                {

                    if (dr.IsDBNull(0) == false)
                    {
                        lngMessageID = (long)dr.GetDecimal(0);

                        break;
                    }
                    else
                    {
                        iCount++;
                    }
                }
                dr.Close();

                return lngMessageID;
            }

        }
        #endregion

        #region ��������ID�ͱ�����ѯ��Ӧ��������Ϣ ����ǰ 2013-06-03
        /// <summary>
        /// ��������ID�ͱ�����ѯ��Ӧ��������Ϣ
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <param name="strTableName"></param>
        /// <returns></returns>
        public static DataTable GetDataByFlowIDandTableName(long lngFlowID, string strTableName)
        {
            string strSQL = "select * from " + strTableName + " where FlowID=" + lngFlowID;
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            return dt;
        }
        #endregion



        #region ����ǩ�����Ƿ���뵽��ǩ���� - 2013-11-20 @������

        /// <summary>
        /// ����ǩ�����Ƿ���뵽��ǩ����
        /// </summary>
        /// <param name="lngFlowID">���̱��</param>
        /// <param name="lngNodeID">���ڱ��</param>
        /// <returns></returns>
        public static bool CheckInfuxSumTo(long lngFlowID, long lngNodeID)
        {
            String strSql = String.Format(@" select count(1) from es_message 
                            where flowid = {0} and nodeid = {1} and receivetype = 91", lngFlowID, lngNodeID);    // 91: ��ǩ����
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                Object objCount = OracleDbHelper.ExecuteScalar(cn, CommandType.Text, strSql);

                return Int32.Parse(objCount.ToString()) > 0;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }

        #endregion

        #region ȡδ�����ǩ��Ա������ - 2013-11-20 @������

        /// <summary>
        /// ȡδ�����ǩ��Ա������
        /// </summary>
        /// <param name="lngFlowID">���̱��</param>
        /// <param name="lngNodeID">���ڱ��</param>
        /// <returns></returns>
        public static Int32 GetUnInfuxActorCount(long lngFlowID, long lngNodeID)
        {
            String strSql = String.Format(@" select count(1) from es_message 
                                            where flowid = {0} and nodeid = {1} and receivetype <> 91 and status = 20", lngFlowID, lngNodeID);    // 91: ��ǩ����
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                Object objCount = OracleDbHelper.ExecuteScalar(cn, CommandType.Text, strSql);

                return Int32.Parse(objCount.ToString());
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }

        #endregion
    }
}
