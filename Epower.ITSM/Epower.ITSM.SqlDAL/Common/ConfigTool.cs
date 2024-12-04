using System;
using System.Data;
using System.Data.OracleClient;
//using System.Data.OracleClient;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// ConfigTool 的摘要说明。
    /// </summary>
    public class ConfigTool
    {
        internal ConfigTool()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 当连接未关闭时关闭连接
        /// </summary>
        /// <param name="cn"></param>
        public static void CloseConnection(OracleConnection cn)
        {
            if (cn.State == ConnectionState.Open)
            {
                cn.Close();
            }
        }

        public static string GetConnectString()
        {
            return System.Configuration.ConfigurationSettings.AppSettings["SQLConnString"];
        }

        /// <summary>
        /// 返回一个连接，注意事项，在调用后由应用程序关闭
        /// </summary>
        /// <returns></returns>
        public static OracleConnection GetConnection()
        {
            OracleConnection cn = new OracleConnection(GetConnectString());
            return cn;
        }

        #region Oracle数据库取法
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetConnectString(string sConnectName)
        {
            return System.Configuration.ConfigurationSettings.AppSettings[sConnectName];
        }

        /// <summary>
        /// 返回一个连接，注意事项，在调用后由应用程序关闭
        /// </summary>
        /// <returns></returns>
        public static OracleConnection GetConnection(string sConnectName)
        {
            OracleConnection cn = new OracleConnection(GetConnectString(sConnectName));
            return cn;
        }

        ///// <summary>
        ///// 当连接未关闭时关闭连接
        ///// </summary>
        ///// <param name="cn"></param>
        //internal static void CloseConnection(OracleConnection cn)
        //{
        //    if (cn.State == ConnectionState.Open)
        //    {
        //        cn.Close();
        //    }
        //}
        #endregion

    }
}
