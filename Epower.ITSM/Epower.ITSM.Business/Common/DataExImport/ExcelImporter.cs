/*******************************************************************
 * 版权所有：深圳市非凡信息技术有限公司
 * 描述：配置管理-数据导入
 * 
 * 
 * 创建人：孙绍棕
 * 创建日期：2013-12-18
 * *****************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace Epower.ITSM.Business.Common.DataExImport
{
    /// <summary>
    /// Excel文件导入抽象类
    /// </summary>
    public abstract class ExcelImporter
    {
        /// <summary>
        /// 取Excel连接字符串
        /// </summary>
        /// <param name="strFileURL"></param>
        protected String GetConnStr(String strFileURL)
        {

            String strConnStr = String.Empty;
            strConnStr = String.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;
                            Extended Properties=Excel 8.0;
                            data source={0}", strFileURL);

            if (System.Configuration.ConfigurationSettings.AppSettings["Is64MachineExcel"] != null
          && System.Configuration.ConfigurationSettings.AppSettings["Is64MachineExcel"].ToLower() == "true")
            {
                strConnStr = String.Format(@" Provider = Microsoft.ACE.OLEDB.12.0;Data Source= {0};Extended Properties=Excel 12.0", strFileURL);
            }

            return strConnStr;
        }


        /// <summary>
        /// Excel内容导入
        /// </summary>
        /// <param name="strFileURL">Excel文件地址</param>
        /// <param name="sbResult">导入结果</param>
        /// <returns></returns>
        public abstract bool Exec(String strFileURL, ref StringBuilder sbResult);
    }
}
