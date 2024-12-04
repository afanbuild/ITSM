using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Epower.ITSM.SqlDAL.ResourceMoniter
{
    /// <summary>
    /// 资源状态查询
    /// </summary>
    public class ResourceStateDP
    {
        private List<String> _resource_id_list;


        public ResourceStateDP() { _resource_id_list = new List<string>(); }

        public ResourceStateDP(String resource_id)
            : this()
        {
            _resource_id_list.Add(resource_id);
        }

        public ResourceStateDP(List<String> resourceid_list)
            : this()
        {
            _resource_id_list = resourceid_list;
        }

        /// <summary>
        /// 取资源状态集
        /// </summary>
        /// <returns></returns>
        public String GetResourceStateResult()
        {
            /*
             * 获取多个资产的监控状态, 以 JSON 格式返回
             * **/

            List<JSONOfResourceState> ret_data = new List<JSONOfResourceState>();

            foreach (String str_resource_id in _resource_id_list)
            {
                RuleDP ee_rule = new RuleDP();
                DataTable dt = ee_rule.FetchRuleListBy(Equ_DeskDP.GetEquCodeByID(str_resource_id));

                ResourceState resource_state = new ResourceState(str_resource_id);
                List<StandardObject.StandardData> data = resource_state.GetResourceStateList();

                if (data.Count <= 0) continue;

                JSONOfResourceState ret_data_item = new JSONOfResourceState();
                ret_data_item.ResourceId = EquCode2EquID(data[0].ResourceID)/**/;
                ret_data_item.URLAddress = data[0].ResourceURL;
                ret_data_item.MessageList = new List<JSONOfAlertMessage>();

                foreach (DataRow item in dt.Rows)
                {
                    String str_rule_json_text = item["Content"].ToString();
                    Rule rule = Newtonsoft.Json.JsonConvert.DeserializeObject<Rule>(str_rule_json_text);

                    JSONOfAlertMessage msg = new JSONOfAlertMessage();
                    msg.AlertImage = rule.AlertImage;
                    msg.Description = rule.Description;
                    msg.Priority = rule.Priority.ToString();
                    msg.RuleId = rule.RuleId;

                    msg.Messages = new List<JSONOfOPPairMessage>();

                    foreach (OPPair pair in rule.OPPairs)
                    {
                        StandardObject.StandardData standardData
                            = data.Find(delegate(StandardObject.StandardData arg1)
                        {
                            return arg1.Key == pair.Key;
                        });

                        if (standardData == null) continue;

                        EStandardData e_standardData = new EStandardData(pair, standardData);
                        String str_rule_message = e_standardData.ToString();
                        if (String.IsNullOrEmpty(str_rule_message)) continue;

                        JSONOfOPPairMessage oppair = new JSONOfOPPairMessage();
                        oppair.Key = standardData.Name;
                        oppair.NormalValue = standardData.NormalValue;
                        oppair.Operator = Utility.Operator2Symbol(pair.Operator);
                        oppair.Preset = pair.Value;
                        oppair.Value = standardData.Value;
                        oppair.Symbol = standardData.Symbol;

                        msg.Messages.Add(oppair);
                    }

                    if (rule.Way == LogicWay.And)
                    {
                        if (msg.Messages.Count != rule.OPPairs.Count)
                            msg.Messages.Clear();
                    }

                    if (msg.Messages.Count == 0) continue;
                    ret_data_item.MessageList.Add(msg);
                }

                ret_data_item.MessageList.Sort(new PriorityOfRuleComparer());

                ret_data.Add(ret_data_item);
            }

            String str_ret_json_text = Newtonsoft.Json.JsonConvert.SerializeObject(ret_data,
                Newtonsoft.Json.Formatting.Indented);

            return str_ret_json_text;
        }

        /// <summary>
        /// 资产支持的监控项集
        /// </summary>
        /// <param name="str_resource_id"></param>
        /// <returns></returns>
        public List<KeyValuePair<String, String>> GetSupportKeyBy(String str_resource_id)
        {
            ResourceState resource_state = new ResourceState(str_resource_id);
            List<KeyValuePair<String, String>> data = resource_state.GetSupportKeyList();
            return data;
        }

        private String EquCode2EquID(String str_equ_id)
        {
            DataTable dt = CommonDP.ExcuteSqlTable(String.Format("SELECT ID FROM Equ_Desk WHERE CODE = '{0}'", str_equ_id));
            return dt.Rows[0][0].ToString();
        }
    }
}
