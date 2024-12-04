using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.AppForms
{
    /// <summary>
    /// AutoWebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    [System.Web.Script.Services.ScriptService]
    public class AutoWebService : System.Web.Services.WebService
    {
        //资产名称
        [WebMethod]
        public string[] GetDataList(string prefixText, int count)
        {
            string strSQL = string.Empty;
            OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationSettings.AppSettings["SQLConnString"]);
            List<string> items = new List<string>(count);
            try
            {
                strSQL = "SELECT DISTINCT name FROM Equ_Desk Where 1=1 And Deleted=0 And rownum<=" + count + " And name like " + StringTool.SqlQ(prefixText + "%");
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        items.Add(dt.Rows[i]["name"].ToString());
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cn.Close();
            }
            return items.ToArray();
        }        

       //客户名称
        [WebMethod]
        public string[] GetDataList2(string prefixText, int count)
        {
            string strSQL = string.Empty;
            OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationSettings.AppSettings["SQLConnString"]);
            strSQL = "SELECT ShortName FROM V_Br_ECustomerAjax Where 1=1 And Deleted=0 And rownum<="+count+" And ShortName like " + StringTool.SqlQ(prefixText + "%");
            List<string> items = new List<string>(count);
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        items.Add(dt.Rows[i]["ShortName"].ToString());
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cn.Close();
            }
            return items.ToArray();
        }      
    }
}
