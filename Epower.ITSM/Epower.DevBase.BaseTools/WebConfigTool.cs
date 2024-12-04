using System;

namespace Epower.DevBase.BaseTools
{
	/// <summary>
	/// WebConfigTool 的摘要说明。
	/// </summary>
	public class WebConfigTool
	{
		public WebConfigTool()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

        public static string GetValue(string strKey)
        {            
            try
            {
                return System.Configuration.ConfigurationSettings.AppSettings[strKey].ToString();
            }
            catch
            {
                throw new Exception("请设置Web.Config中的"+strKey+"属性！");
            }            
        }

        public static string GetValue(string strKey,string defaultValue)
        {
            if (System.Configuration.ConfigurationSettings.AppSettings[strKey] == null)
            {
                return defaultValue;
            }
            return System.Configuration.ConfigurationSettings.AppSettings[strKey].ToString();
        }
	}
}
