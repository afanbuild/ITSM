/*******************************************************************
 *
 * Description:缓存处理类
 * 
 * 
 * Create By  :
 * Create Date:2008年7月30日
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
        /// CACHE数据读取
        /// CACHE数据依赖 数据相应表的变化而变化,
        /// 也可以根据应用程序对Cache["EpCacheValidFlowModel"](流程相关) Cache["EpCacheValidUser"](用户相关)
        /// Cache["EpCacheValidDept"](部门相关) Cache["EpCacheValidUserDept"](用户部门关系相关) 赋 false 值 强制取消CACHE
        /// </summary>
        /// <param name="CacheName"></param>
        /// <returns></returns>
        public static DataTable GetDataTableFromCache(string CacheName)
        {
            string Key = "CommCache_" + CacheName.ToLower();
            string strSQL = "";
            bool blnConstraint = false;     //为SQL缓存依赖添加 强制取消的功能
            if (sCacheSource == string.Empty)
            {
                sCacheSource = System.Configuration.ConfigurationSettings.AppSettings["SqlCacheDataSource"];
            }
            DataTable dt = null;
            SqlCacheDependency scd;


            //判断SQL缓存依赖添加 强制取消
            // 通过外部程序相关 cache 赋值的方式   根据 user dept app flowmodel..... 表的变化,分别判断
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
                    //判断catalog变化
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
                case "definepersonopinion":    //快速意见
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
                //取数据
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
                        case "servicelevelguid":   //服务级别设置缓存   依赖服务级别表
                            strSQL = "SELECT a.*,b.guidname FROM Cst_SLGuid a,Cst_GuidDefinition b WHERE a.guidid = b.guidid ";
                            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);

                            //scd = new SqlCacheDependency(sCacheSource, "cst_servicelevel");
                            HttpRuntime.Cache.Insert(Key, dt);
                            break;
                        case "definepersonopinion":   //快速意见
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
                    //如果有错误发生配置一下 SQL 缓存
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
                //从缓存中取值
                dt = (DataTable)HttpRuntime.Cache[Key];
            }
            return dt;
        }


        /// <summary>
        /// 直接缓存表数据[其它:目前已经使用 ]
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
                //取数据
                OracleConnection cn = ConfigTool.GetConnection();
                try
                {
                    string strSQL = "SELECT * FROM " + TableName;
                    DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);

                    //制定缓存策略
                    SqlCacheDependency scd = new SqlCacheDependency(sCacheSource, TableName.ToLower());
                    //插入缓存
                    HttpRuntime.Cache.Insert(Key, dt, scd);
                    return dt;
                }
                catch (Exception e)
                {
                    ConfigTool.CloseConnection(cn);
                    //如果发生错误配置一下缓存
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
                //从缓存中取值
                return (DataTable)HttpRuntime.Cache[Key];
            }
        }

    }
}
