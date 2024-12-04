using System;
using System.Data;
using System.Data.OracleClient;

namespace appDataProcess
{
	/// <summary>
	/// ConfigTool 的摘要说明。
	/// </summary>
	public class ConfigToolApp
	{
		public ConfigToolApp()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

		/// <summary>
		/// 当连接未关闭时关闭连接
		/// </summary>
		/// <param name="cn"></param>
		internal static void CloseConnection(OracleConnection cn)
		{
			if(cn.State == ConnectionState.Open)
			{
				cn.Close();
			}
		}

		internal static string GetConnectString()
		{
			return System.Configuration.ConfigurationSettings.AppSettings["SQLConnString"];
		}

		/// <summary>
		/// 返回一个连接，注意事项，在调用后由应用程序关闭
		/// </summary>
		/// <returns></returns>
		internal static OracleConnection GetConnection()
		{
			OracleConnection cn = new OracleConnection(GetConnectString());
			return cn;
		}

	}
}
