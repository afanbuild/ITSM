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
using Epower.ITSM.SqlDAL;
using System.Text;
using System.Collections.Generic;

namespace Epower.ITSM.Web.AppSchedules
{
    public partial class turnAjax : System.Web.UI.Page
    {
        /// <summary>
        /// 所有轮班班次详情
        /// </summary>
        public Dictionary<long, DataTable> DicTurnRuleDetl
        {
            get
            {
                if (Session["DicTurnRuleDetl"] == null)
                {
                    throw new ArgumentNullException("DicTurnRuleDetl is null.");
                }
                return Session["DicTurnRuleDetl"] as Dictionary<long, DataTable>;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {

            string strJson = string.Empty;

            long trID = long.Parse(Request["TRID"].ToString());
            if (trID > 0)
            {
                DataTable dtTurnRuleDetl = null;
                dtTurnRuleDetl = GetDTTurnRuleDetl(trID);

                if (dtTurnRuleDetl != null)
                {
                    if (dtTurnRuleDetl.Rows.Count > 0)
                    {
                        strJson=DataTableJson(dtTurnRuleDetl);
                    }
                }
            }
            Response.Write("{\"result\": " + strJson + "}");
        }

        /// <summary>
        /// 获取轮班详细班次
        /// </summary>
        /// <param name="trID"></param>
        /// <returns></returns>
        private DataTable GetDTTurnRuleDetl(long trID)
        {
            DataTable dtTurnRuleDetl = null;
            if (DicTurnRuleDetl.Count == 0)
            {
                dtTurnRuleDetl = GS_SchedulesAreaDP.GetTurnRuleDetl(trID);
            }
            else
            {
                dtTurnRuleDetl = DicTurnRuleDetl[trID];
            }
            return dtTurnRuleDetl;
        }


        public string DataTableJson(DataTable dt)
        {
            if (dt.Rows.Count == 0)
            {
                return "";
            }

            StringBuilder jsonBuilder = new StringBuilder();
            // jsonBuilder.Append("{");
            //jsonBuilder.Append(dt.TableName.ToString()); 
            jsonBuilder.Append("[");//转换成多个model的形式
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            //  jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }
    }
}
