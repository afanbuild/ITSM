using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Collections.Generic;

namespace Epower.ITSM.Web.EquipmentManager
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class get_resource_state : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            // context.Response.ContentType = "text/plain";
            // context.Response.Write("Hello World");
            // var data = { action: 'get-resource-state', resource_id_list:  json_text};
            String str_action = context.Request.Form["action"];
            if (String.IsNullOrEmpty(str_action)) { response("error", "非法参数", null, context); return; }
            if (!str_action.Equals("get-resource-state")) { response("error", "你想干什么？", null, context); return; }

            if (String.IsNullOrEmpty(context.Request.Form["resource_id_list"])) { response("error", "非法参数", null, context); return; }

            List<String> id_list
                = Newtonsoft.Json.JsonConvert.DeserializeObject<List<String>>(context.Request.Form["resource_id_list"]);

            SqlDAL.ResourceMoniter.ResourceStateDP ee = new Epower.ITSM.SqlDAL.ResourceMoniter.ResourceStateDP(id_list);
            String str_json_text = ee.GetResourceStateResult();

            response("ok", "ok", str_json_text, context);
        }

        private void response(String str_status,
                    String str_message,
                    String str_data,
                    HttpContext context)
        {
            if (str_data == null)
                str_data = "' '";
            String str_response_text = "{" + String.Format("status:'{0}', message: '{1}', data: {2}",
                str_status, str_message.Replace("\n", "<br />"), str_data) + "}";

            context.Response.Write(str_response_text);
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
