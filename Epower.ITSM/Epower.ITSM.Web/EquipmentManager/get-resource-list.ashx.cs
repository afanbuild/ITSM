using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Epower.ITSM.SqlDAL;
using System.Web.UI.MobileControls;
using System.Collections.Generic;
using System.Web.Security;

namespace Epower.ITSM.Web.EquipmentManager
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class get_resource_list : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            SqlDAL.ResourceMoniter.ResourceState rs = new Epower.ITSM.SqlDAL.ResourceMoniter.ResourceState("");
            List<KeyValuePair<String, String>> data = rs.GetSupportKeyList();
            context.Response.Write(data.Count.ToString());

            context.Response.Write(DateTime.Now.Ticks.ToString());
            return;

            context.Response.ContentType = "text/plain";
            String auth_code = context.Request.QueryString["auth"];

            // verify is have "auth" parameter ?
            if (String.IsNullOrEmpty(auth_code))
            {
                context.Response.StatusCode = 404;
                context.Response.End();
            }

            // verify auth_code.
            String str_md5_pwd = FormsAuthentication.HashPasswordForStoringInConfigFile("sunshaozong",
                "md5");

            auth_code = auth_code.Trim();
            Boolean isEquals = auth_code.Equals(str_md5_pwd);

            if (!isEquals)
            {
                context.Response.StatusCode = 404;
                context.Response.End();
            }

            try
            {
                Equ_DeskDP ee = new Equ_DeskDP();
                DataTable dt = ee.GetDataTable(String.Empty, " ORDER BY ID ASC ");

                List<StandardObject.ResourceData> resource_list
                    = new List<StandardObject.ResourceData>();

                foreach (DataRow item in dt.Rows)
                {
                    StandardObject.ResourceData resource = new StandardObject.ResourceData();
                    resource.Id = long.Parse(item["ID"].ToString());
                    resource.Name = item["NAME"].ToString();
                    resource.Code = item["CODE"].ToString();
                    resource.CatalogName = item["CATALOGNAME"].ToString();

                    resource_list.Add(resource);
                }

                String str_json_text = Newtonsoft.Json.JsonConvert.SerializeObject(resource_list,
                    Newtonsoft.Json.Formatting.Indented);

                context.Response.Write(str_json_text);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = ex.Message;
                context.Response.End();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
