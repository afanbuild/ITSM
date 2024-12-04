using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Epower.ITSM.SqlDAL.ResourceMoniter
{
    public class RuleDP
    {
        /// <summary>
        /// 未删除
        /// </summary>
        private static Int32 NOT_DELETED = 0;
        /// <summary>
        /// 删除
        /// </summary>
        private static Int32 DELETED = 1;
        /// <summary>
        /// 添加规则 SQL 脚本
        /// </summary>
        private static String _ADD_NEW_RULE_SQLTEXT
          = @"INSERT INTO EQU_RESOURCE_RULE(
              ID,  EQU_ID, RULE_DESC,  CONTENT, DELETED, CTIME) VALUES(
              {0}, '{1}',    '{2}',      '{3}',   {4},     to_date( '{5}', 'YYYY-MM-DD   HH24:MI:SS '))";
        /// <summary>
        /// 修改规则
        /// </summary>
        private static String _UPDATE_RULE_SQLTEXT
          = @"UPDATE EQU_RESOURCE_RULE SET RULE_DESC = '{0}', CONTENT = '{1}', DELETED = {2}
              WHERE ID = {3}";
        /// <summary>
        /// 设置规则为已删除状态
        /// </summary>
        private static String _DELETE_RULE_BY_RULEID
          = @"UPDATE EQU_RESOURCE_RULE SET DELETED = {0} WHERE ID = {1}";
        /// <summary>
        /// 取资产的规则列表
        /// </summary>
        private static String _QUERY_RULELIST_BY_RESOURCEID
          = @" SELECT ID,  EQU_ID, RULE_DESC,  CONTENT, DELETED, CTIME 
               FROM EQU_RESOURCE_RULE WHERE EQU_ID = '{0}' AND DELETED = 0 ORDER BY CTIME ASC";
        /// <summary>
        /// 规则对象
        /// </summary>
        private Rule _rule;

        public RuleDP(Rule rule)
        {
            this._rule = rule;
        }

        public RuleDP() { }

        public DataTable FetchRuleListBy(String str_resource_id)
        {
            String str_exec_sqltext = String.Format(_QUERY_RULELIST_BY_RESOURCEID,
                                str_resource_id);

            return CommonDP.ExcuteSqlTable(str_exec_sqltext);
        }

        public long AddNewRule()
        {
            long lngNextId = EpowerGlobal.EPGlobal.GetNextID("RESOURCE_MOINTORIN");

            _rule.ResourceId = Equ_DeskDP.GetEquCodeByID(_rule.ResourceId);

            String str_rule_json_text = Newtonsoft.Json.JsonConvert.SerializeObject(_rule,
                Newtonsoft.Json.Formatting.Indented);

            String str_exec_sqltext = String.Format(_ADD_NEW_RULE_SQLTEXT,
                lngNextId, _rule.ResourceId, _rule.Description, str_rule_json_text,
                NOT_DELETED, DateTime.Now);

            CommonDP.ExcuteSql(str_exec_sqltext);

            return lngNextId;
        }

        public void UpdRule(Rule rule)
        {
            String str_rule_json_text = Newtonsoft.Json.JsonConvert.SerializeObject(_rule,
                Newtonsoft.Json.Formatting.Indented);

            String str_exec_sqltext = String.Format(_UPDATE_RULE_SQLTEXT,
                rule.Description, str_rule_json_text, NOT_DELETED, rule.RuleId);

            CommonDP.ExcuteSql(str_exec_sqltext);
        }

        public void DelBy(long lngRuleId)
        {
            String str_exec_sqltext = String.Format(_DELETE_RULE_BY_RULEID,
                DELETED, lngRuleId);

            CommonDP.ExcuteSql(str_exec_sqltext);
        }
    }
}
