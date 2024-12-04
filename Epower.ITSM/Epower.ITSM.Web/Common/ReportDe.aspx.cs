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
using Epower.ITSM.SqlDAL.Common;
using Epower.ITSM.SqlDAL;
using System.Drawing;

namespace Epower.ITSM.Web.Common
{
    public partial class ReportDe : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.cpIssue.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(LoadData1);
            if (!IsPostBack)
            {
                LoadData1();
            }
        }

        #region  LoadData1
        /// <summary>
        /// 数据加载
        /// </summary>
        private void LoadData1()
        {
            int rowcount = 0;
            this.gridIssue.DataSource = RefreshPage.GetRefreshPage(this.cpIssue.PageSize, this.cpIssue.CurrentPage, ref rowcount);
            this.gridIssue.DataBind();
            this.cpIssue.RecordCount = rowcount;
            this.cpIssue.Bind();
        }
        #endregion
        protected void gridIssue_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string sFlowID = DataBinder.Eval(e.Item.DataItem, "FlowID").ToString();
                DataTable dt = ZHServiceDP.GetFlowBusLimitByFlowID(decimal.Parse(sFlowID == "" ? "0" : sFlowID));
                DateTime FinishedTime = DataBinder.Eval(e.Item.DataItem, "FinishedTime").ToString() == "" ? DateTime.Now : DateTime.Parse(DataBinder.Eval(e.Item.DataItem, "FinishedTime").ToString());

                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DateTime LimitTime = dt.Rows[i]["LimitTime"].ToString() == "" ? DateTime.Now : DateTime.Parse(dt.Rows[i]["LimitTime"].ToString());
                        TimeSpan ts1 = LimitTime.Subtract(FinishedTime);
                        if (dt.Rows[i]["GuidID"].ToString() == "10001")
                        {
                            string str1 = "";
                            if (ts1.Days != 0)
                                str1 += (ts1.Days > 0 ? ts1.Days : -ts1.Days) + "天";
                            if (ts1.Hours != 0)
                                str1 += (ts1.Hours > 0 ? ts1.Hours : -ts1.Hours) + "小时";
                            if (ts1.Minutes != 0)
                                str1 += (ts1.Minutes > 0 ? ts1.Minutes : -ts1.Minutes) + "分钟";
                            if (ts1.Seconds != 0)
                                str1 += (ts1.Seconds > 0 ? ts1.Seconds : -ts1.Seconds) + "秒";

                            if (ts1.Days < 0 || ts1.Hours < 0 || ts1.Minutes < 0 || ts1.Seconds < 0)
                            {
                                e.Item.Cells[4].Text = "超" + str1;
                                e.Item.Cells[4].ForeColor = Color.Red;
                            }
                            else
                            {
                                e.Item.Cells[4].Text = "还剩" + str1;
                            }
                        }
                    }
                }
                else
                {
                    e.Item.Cells[4].Text = "";
                }
                e.Item.Cells[4].Font.Bold = true;

                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                // string sFlowID = DataBinder.Eval(e.Item.DataItem, "FlowID").ToString();
                e.Item.Attributes.Add("ondblclick", "SetUrl();window.open('../Forms/frmIssueView.aspx?FlowID=" + sFlowID.ToString() + "&randomid='+GetRandom(),'MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");

            }
        }
    }
}
