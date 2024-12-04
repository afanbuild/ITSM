/*******************************************************************
 *
 * Description:直接执行SQL类
 * 
 * 
 * Create By  :
 * Create Date:2008年7月30日
 * *****************************************************************/
using System;
using System.Data;
using System.Data.OracleClient;
using System.Text;
using System.Xml;
using System.Web;

using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Log
{
    public class CommonDP
    {
        /// <summary>
        /// 执行Sql
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
        /// 执行SQL
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
        /// 执行SQL
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
        /// 取得系统基本运行参数值
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static string GetConfigValue(string NodesName, string Key)
        {
            string sConfigName = "SystemConfig";
            string sValue = string.Empty;
            try
            {
                string ConfigFile = HttpRuntime.AppDomainAppPath + "Config\\" + sConfigName + ".xml";

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.DocumentElement.SelectSingleNode("Items/Item[@Name=" +StringTool.SqlQ(Key) + " ]");

                sValue = node.Attributes["Value"].Value;
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

        #region 取某一会签环节未处理人员数 - 2013-11-20 @孙绍棕

        /// <summary>
        /// 取某一会签环节未处理人员数
        /// </summary>
        /// <param name="lngFlowID">流程编号</param>
        /// <param name="lngNodeID">环节编号</param>
        /// <returns></returns>
        public static Int32 CheckUnInfluxCount(long lngFlowID, long lngNodeID)
        {
            String strSql = String.Format(@"select count(status) from es_message 
                                            where flowid = {0} and nodeid = {1} and status = 20", lngFlowID, lngNodeID);
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
