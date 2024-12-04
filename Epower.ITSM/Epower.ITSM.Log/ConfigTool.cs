using System;
using System.Data;
using System.Data.OracleClient;

namespace Epower.ITSM.Log
{
    /// <summary>
    /// ConfigTool ��ժҪ˵����
    /// </summary>
    internal class ConfigTool
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
        internal static void CloseConnection(OracleConnection cn)
        {
            if (cn.State == ConnectionState.Open)
            {
                cn.Close();
            }
        }

        internal static string GetConnectString()
        {
            return System.Configuration.ConfigurationSettings.AppSettings["SQLConnString"];
        }

        /// <summary>
        /// ����һ�����ӣ�ע������ڵ��ú���Ӧ�ó���ر�
        /// </summary>
        /// <returns></returns>
        internal static OracleConnection GetConnection()
        {
            OracleConnection cn = new OracleConnection(GetConnectString());
            return cn;
        }

    }
}
