using System;

namespace Epower.DevBase.BaseTools
{
	/// <summary>
	/// WebConfigTool ��ժҪ˵����
	/// </summary>
	public class WebConfigTool
	{
		public WebConfigTool()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
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
                throw new Exception("������Web.Config�е�"+strKey+"���ԣ�");
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
