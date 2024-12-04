using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using Epower.ITSM.SqlDAL.ResourceMoniter;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frmEqu_Monitoring_Rule_Resource_Details : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindRuleList();
                BindSupportKey();
                BindOperatorList();
            }
        }

        /// <summary>
        /// 显示该资源的规则列表
        /// </summary>
        private void BindRuleList()
        {
            String str_resource_id = Request.QueryString["resource_id"];
            if (String.IsNullOrEmpty(str_resource_id)) return;

            RuleDP rule = new RuleDP();
            DataTable dt = rule.FetchRuleListBy(Equ_DeskDP.GetEquCodeByID(str_resource_id));

            List<SqlDAL.ResourceMoniter.Rule> data = new List<Epower.ITSM.SqlDAL.ResourceMoniter.Rule>();
            foreach (DataRow item in dt.Rows)
            {
                String str_json_text = item["Content"] as String;
                SqlDAL.ResourceMoniter.Rule rule_item
                    = Newtonsoft.Json.JsonConvert.DeserializeObject<SqlDAL.ResourceMoniter.Rule>(str_json_text);
                rule_item.RuleId = item["ID"].ToString();
                data.Add(rule_item);
            }
            // dgRuleList            

            dgRuleList.DataSource = data;
            dgRuleList.DataBind();
        }

        /// <summary>
        /// 取该资源可用的监控项
        /// </summary>
        private void BindSupportKey()
        {
            String str_resource_id = Request.QueryString["resource_id"];
            if (String.IsNullOrEmpty(str_resource_id)) return;

            ResourceStateDP ee = new ResourceStateDP();
            List<KeyValuePair<string, string>> data = ee.GetSupportKeyBy(str_resource_id);

            if (data.Count == 0) {
                literal_alert_messages.Visible = true;
            }

            ddl_support_key_list.DataSource = data;
            ddl_support_key_list.DataTextField = "Value";
            ddl_support_key_list.DataValueField = "Key";
            ddl_support_key_list.DataBind();


        }



        /// <summary>
        /// 显示操作符列表
        /// </summary>
        private void BindOperatorList()
        {
            List<KeyValuePair<String, String>> data = new List<KeyValuePair<string, string>>();
            data.Add(new KeyValuePair<string, string>(((int)OP.Equal).ToString(), "相等"));
            data.Add(new KeyValuePair<string, string>(((int)OP.GreaterThan).ToString(), "大于"));
            data.Add(new KeyValuePair<string, string>(((int)OP.GreaterThanAndEqual).ToString(), "大于等于"));
            data.Add(new KeyValuePair<string, string>(((int)OP.In).ToString(), "包含"));
            data.Add(new KeyValuePair<string, string>(((int)OP.NotEqual).ToString(), "不等于"));
            data.Add(new KeyValuePair<string, string>(((int)OP.NotIn).ToString(), "不包含"));
            data.Add(new KeyValuePair<string, string>(((int)OP.SmallerThan).ToString(), "小于"));
            data.Add(new KeyValuePair<string, string>(((int)OP.SmallerThanAndEqual).ToString(), "小于等于"));

            ddl_operator_list.DataSource = data;
            ddl_operator_list.DataTextField = "Value";
            ddl_operator_list.DataValueField = "Key";
            ddl_operator_list.DataBind();
        }

        protected void dgRuleList_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.AlternatingItem && e.Item.ItemType != ListItemType.Item)
                return;

            SqlDAL.ResourceMoniter.Rule rule
                = e.Item.DataItem as SqlDAL.ResourceMoniter.Rule;

            // 显示图标
            e.Item.Cells[1].Text = String.Format(e.Item.Cells[1].Text, rule.AlertImage);

            // 存储 json 字串
            e.Item.Cells[6].Text = String.Format(e.Item.Cells[6].Text, rule.RuleId,
                Newtonsoft.Json.JsonConvert.SerializeObject(rule, Newtonsoft.Json.Formatting.Indented));
        }
    }
}
