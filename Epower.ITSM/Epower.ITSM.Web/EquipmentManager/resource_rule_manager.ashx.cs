using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Epower.ITSM.Log;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.EquipmentManager
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class resource_rule_manager : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            String str_action = context.Request.Form["action"];
            if (String.IsNullOrEmpty(str_action)) ReportError("缺少参数 [action]", context);

            switch (str_action)
            {
                case "add_new_rule":
                    AddNewRule(context);
                    break;
                case "update_rule":
                    UpdRule(context);
                    break;
                case "delete_rule":
                    DeleteRule(context);
                    break;
                default:
                    break;
            }
        }

        private void AddNewRule(HttpContext context)
        {
            String str_data = context.Request.Form["data"];
            if (String.IsNullOrEmpty(str_data)) ReportError("缺少参数 [data]", context);

            SqlDAL.ResourceMoniter.Rule rule = null;
            try
            {

                rule = Newtonsoft.Json.JsonConvert.DeserializeObject<SqlDAL.ResourceMoniter.Rule>(str_data);
                rule.CTIME = DateTime.Now;

                SqlDAL.ResourceMoniter.RuleDP ee
                    = new Epower.ITSM.SqlDAL.ResourceMoniter.RuleDP(rule);
                long lngRuleId = ee.AddNewRule();

                rule.RuleId = lngRuleId.ToString();

                str_data = Newtonsoft.Json.JsonConvert.SerializeObject(rule);
            }
            catch (Exception ex)
            {
                response("error", ex.Message, null, context);
            }

            response("ok", "添加规则成功", str_data, context);
        }

        private void UpdRule(HttpContext context)
        {
            String str_data = context.Request.Form["data"];
            if (String.IsNullOrEmpty(str_data)) ReportError("缺少参数 [data]", context);

            SqlDAL.ResourceMoniter.Rule rule = null;
            try
            {

                rule = Newtonsoft.Json.JsonConvert.DeserializeObject<SqlDAL.ResourceMoniter.Rule>(str_data);

                SqlDAL.ResourceMoniter.RuleDP ee
                    = new Epower.ITSM.SqlDAL.ResourceMoniter.RuleDP(rule);
                ee.UpdRule(rule);                
            }
            catch (Exception ex)
            {
                response("error", ex.Message, null, context);
            }

            response("ok", "修改规则成功", null, context);
        }

        private void DeleteRule(HttpContext context)
        {
            long lngRuleId;
            Boolean isOk = long.TryParse(context.Request.Form["rule_id"], out lngRuleId);
            if (!isOk) ReportError("参数 [rule_id] 非法", context);

            try
            {
                SqlDAL.ResourceMoniter.RuleDP ee
                    = new Epower.ITSM.SqlDAL.ResourceMoniter.RuleDP();
                ee.DelBy(lngRuleId);                
            }
            catch (Exception ex)
            {
                response("error", ex.Message, null, context);
            }

            response("ok", "删除规则成功", null, context);
        }

        private void ReportError(String str_message, HttpContext context)
        {
            response("error", str_message, null, context);
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
