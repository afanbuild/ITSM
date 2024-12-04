/*******************************************************************
 *
 * Description:���洦����
 * 
 * 
 * Create By  :
 * Create Date:2008��7��30��
 * *****************************************************************/
using System;
using System.Data;
using System.Configuration;
using System.Web;

using System.Data.OracleClient;
using System.Web.Caching;
using System.Xml;

using Epower.DevBase.BaseTools;

namespace Epower.ITSM.SqlDAL
{
    public class CommSqlCacheHelper
    {

        private static string sCacheSource = string.Empty;

        /// <summary>
        /// CACHE���ݶ�ȡ
        /// CACHE�������� ������Ӧ��ı仯���仯,
        /// Ҳ���Ը���Ӧ�ó����Cache["EpCacheValidFlowModel"](�������) Cache["EpCacheValidUser"](�û����)
        /// Cache["EpCacheValidDept"](�������) Cache["EpCacheValidUserDept"](�û����Ź�ϵ���) �� false ֵ ǿ��ȡ��CACHE
        /// </summary>
        /// <param name="CacheName"></param>
        /// <returns></returns>
        public static DataTable GetDataTableFromCache(string CacheName)
        {
            string Key = "CommCache_" + CacheName.ToLower();
            string strSQL = "";
            bool blnConstraint = false;     //ΪSQL����������� ǿ��ȡ���Ĺ���
            if (sCacheSource == string.Empty)
            {
                sCacheSource = System.Configuration.ConfigurationSettings.AppSettings["SqlCacheDataSource"];
            }
            DataTable dt = null;
            SqlCacheDependency scd;


            //�ж�SQL����������� ǿ��ȡ��
            // ͨ���ⲿ������� cache ��ֵ�ķ�ʽ   ���� user dept app flowmodel..... ��ı仯,�ֱ��ж�
            switch (CacheName.ToLower())
            {
                case "equcategory":    
                    if (HttpRuntime.Cache["CommCacheValidEquCategory"] != null)
                    {
                        if ((bool)HttpRuntime.Cache["CommCacheValidEquCategory"] == false)
                        {
                            HttpRuntime.Cache.Remove(Key);
                            HttpRuntime.Cache.Remove("CommCacheValidEquCategory");
                            blnConstraint = true;
                        }
                    }
                    break;
                case "equschemaitems":    //
                    if (HttpRuntime.Cache["CommCacheValidEquSchemaItem"] != null)
                    {
                        if ((bool)HttpRuntime.Cache["CommCacheValidEquSchemaItem"] == false)
                        {
                            HttpRuntime.Cache.Remove(Key);
                            HttpRuntime.Cache.Remove("CommCacheValidEquSchemaItem");
                            blnConstraint = true;
                        }
                    }
                    break;
                case "brschemaitems":    //
                    if (HttpRuntime.Cache["CommCacheValidBrSchemaItem"] != null)
                    {
                        if ((bool)HttpRuntime.Cache["CommCacheValidBrSchemaItem"] == false)
                        {
                            HttpRuntime.Cache.Remove(Key);
                            HttpRuntime.Cache.Remove("CommCacheValidBrSchemaItem");
                            blnConstraint = true;
                        }
                    }
                    break;
                case "":
                    if (HttpRuntime.Cache["CommCacheValidBrExtensionsItems"] != null)
                    {
                        if ((bool)HttpRuntime.Cache["CommCacheValidBrExtensionsItems"] == false)
                        {
                            HttpRuntime.Cache.Remove(Key);
                            HttpRuntime.Cache.Remove("CommCacheValidBrExtensionsItems");
                            blnConstraint = true;
                        }
                    }
                    break;
                case "brcategory":    //
                    if (HttpRuntime.Cache["CommCacheValidBrCategory"] != null)
                    {
                        if ((bool)HttpRuntime.Cache["CommCacheValidBrCategory"] == false)
                        {
                            HttpRuntime.Cache.Remove(Key);
                            HttpRuntime.Cache.Remove("CommCacheValidBrCategory");
                            blnConstraint = true;
                        }
                    }
                    break;

                case "mastcustomer":    //
                    if (HttpRuntime.Cache["CommCacheValidMastCustomer"] != null)
                    {
                        if ((bool)HttpRuntime.Cache["CommCacheValidMastCustomer"] == false)
                        {
                            HttpRuntime.Cache.Remove(Key);
                            HttpRuntime.Cache.Remove("CommCacheValidMastCustomer");
                            blnConstraint = true;
                        }
                    }
                    break;

                case "servicelevelguid":
                case "cstservicelevel":    
                    //�ж�catalog�仯
                    if (HttpRuntime.Cache["CommCacheValidCstServiceLevel"] != null)
                    {
                        if ((bool)HttpRuntime.Cache["CommCacheValidCstServiceLevel"] == false)
                        {
                            HttpRuntime.Cache.Remove(Key);
                            HttpRuntime.Cache.Remove("CommCacheValidCstServiceLevel");
                            blnConstraint = true;
                        }
                    }
                    break;
                case "definepersonopinion":    //�������
                    if (HttpRuntime.Cache["CommCacheValiddefinePersonOpinion"] != null)
                    {
                        if ((bool)HttpRuntime.Cache["CommCacheValiddefinePersonOpinion"] == false)
                        {
                            HttpRuntime.Cache.Remove(Key);
                            HttpRuntime.Cache.Remove("CommCacheValiddefinePersonOpinion");
                            blnConstraint = true;
                        }
                    }
                    break;
                default:

                    break;
            }

            if (HttpRuntime.Cache[Key] == null)
            {
                //ȡ����
                OracleConnection cn = ConfigTool.GetConnection();
                try
                {
                    switch (CacheName.ToLower())
                    {
                        case "equcategory":
                            strSQL = "SELECT * FROM Equ_Category ";
                            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);

                            //scd = new SqlCacheDependency(sCacheSource, "equ_category");
                            HttpRuntime.Cache.Insert(Key, dt);
                            break;

                        case "equschemaitems":
                            strSQL = "SELECT * FROM Equ_SchemaItems ";
                            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);

                            //scd = new SqlCacheDependency(sCacheSource, "equ_schemaitems");
                            HttpRuntime.Cache.Insert(Key, dt);
                            break;
                        case "brschemaitems":
                            strSQL = "SELECT * FROM Br_SchemaItems ";
                            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);

                            //scd = new SqlCacheDependency(sCacheSource, "br_schemaitems");
                            HttpRuntime.Cache.Insert(Key, dt);
                            break;
                        case "brcategory":
                            strSQL = "SELECT * FROM Br_Category ";
                            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);

                            //scd = new SqlCacheDependency(sCacheSource, "br_category");
                            HttpRuntime.Cache.Insert(Key, dt);
                            break;

                        case "mastcustomer":
                            strSQL = "SELECT * FROM Br_MastCustomer ";
                            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);

                            //scd = new SqlCacheDependency(sCacheSource, "br_mastcustomer");
                            HttpRuntime.Cache.Insert(Key, dt);
                            break;
                        case "cstservicelevel":
                            strSQL = "SELECT * FROM Cst_ServiceLevel ";
                            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);

                            //scd = new SqlCacheDependency(sCacheSource, "cst_servicelevel");
                            HttpRuntime.Cache.Insert(Key, dt);
                            break;
                        case "servicelevelguid":   //���񼶱����û���   �������񼶱��
                            strSQL = "SELECT a.*,b.guidname FROM Cst_SLGuid a,Cst_GuidDefinition b WHERE a.guidid = b.guidid ";
                            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);

                            //scd = new SqlCacheDependency(sCacheSource, "cst_servicelevel");
                            HttpRuntime.Cache.Insert(Key, dt);
                            break;
                        case "definepersonopinion":   //�������
                            strSQL = "SELECT * FROM EA_DefinePersonOpinion ";
                            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);

                            //scd = new SqlCacheDependency(sCacheSource, "EA_DefinePersonOpinion");
                            HttpRuntime.Cache.Insert(Key, dt);
                            break;
                        default:
                            break;
                    }




                }
                catch (Exception e)
                {
                    

                    //dt = null;
                    //����д���������һ�� SQL ����
                    string ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["SQLConnString"];
                    SqlCacheDependencyAdmin.EnableNotifications(ConnectionString);
                    string[] tables = new string[] { "equ_category", "equ_schemaitems", "br_schemaitems", "br_category", "br_mastcustomer", "cst_servicelevel", "EA_DefinePersonOpinion" };

                    SqlCacheDependencyAdmin.EnableTableForNotifications(
                              ConnectionString,
                              tables);

                }
                finally { ConfigTool.CloseConnection(cn); }

            }
            else
            {
                //�ӻ�����ȡֵ
                dt = (DataTable)HttpRuntime.Cache[Key];
            }
            return dt;
        }


        /// <summary>
        /// ֱ�ӻ��������[����:Ŀǰ�Ѿ�ʹ�� ]
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public static DataTable GetDirectTableFromCache(string TableName)
        {
            string Key = "EpCacheTable_" + TableName.ToLower();

            if (sCacheSource == string.Empty)
            {
                sCacheSource = System.Configuration.ConfigurationSettings.AppSettings["SqlCacheDataSource"];
            }

            if (HttpRuntime.Cache[Key] == null)
            {
                //ȡ����
                OracleConnection cn = ConfigTool.GetConnection();
                try
                {
                    string strSQL = "SELECT * FROM " + TableName;
                    DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);

                    //�ƶ��������
                    SqlCacheDependency scd = new SqlCacheDependency(sCacheSource, TableName.ToLower());
                    //���뻺��
                    HttpRuntime.Cache.Insert(Key, dt, scd);
                    return dt;
                }
                catch (Exception e)
                {
                    ConfigTool.CloseConnection(cn);
                    //���������������һ�»���
                    string ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["SQLConnString"];
                    SqlCacheDependencyAdmin.EnableNotifications(ConnectionString);
                    SqlCacheDependencyAdmin.EnableTableForNotifications(
                              ConnectionString,
                              TableName.ToLower());
                    return null;
                }
                finally
                {
                    ConfigTool.CloseConnection(cn);
                }
            }
            else
            {
                //�ӻ�����ȡֵ
                return (DataTable)HttpRuntime.Cache[Key];
            }
        }

    }
}
