using System;
using System.Data;
using System.Data.OracleClient;
//using System.Data.OracleClient;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// ConfigTool ��ժҪ˵����
    /// </summary>
    public class ConfigTool
    {
        internal ConfigTool()
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
        }

        /// <summary>
        /// ������δ�ر�ʱ�ر�����
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
        /// ����һ�����ӣ�ע������ڵ��ú���Ӧ�ó���ر�
        /// </summary>
        /// <returns></returns>
        public static OracleConnection GetConnection()
        {
            OracleConnection cn = new OracleConnection(GetConnectString());
            return cn;
        }

        #region Oracle���ݿ�ȡ��
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetConnectString(string sConnectName)
        {
            return System.Configuration.ConfigurationSettings.AppSettings[sConnectName];
        }

        /// <summary>
        /// ����һ�����ӣ�ע������ڵ��ú���Ӧ�ó���ر�
        /// </summary>
        /// <returns></returns>
        public static OracleConnection GetConnection(string sConnectName)
        {
            OracleConnection cn = new OracleConnection(GetConnectString(sConnectName));
            return cn;
        }

        ///// <summary>
        ///// ������δ�ر�ʱ�ر�����
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
