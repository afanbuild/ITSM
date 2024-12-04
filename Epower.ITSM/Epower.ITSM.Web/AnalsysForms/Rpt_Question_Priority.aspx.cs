using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Epower.ITSM.SqlDAL.Flash;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using System.Collections.Generic;
using System.Text;

namespace Epower.ITSM.Web.AnalsysForms
{
    public partial class Rpt_Question_Priority : BasePage
    {
        #region 设置父窗体按钮事件SetParentButtonEvent
        /// <summary>
        /// 设置父窗体按钮事件


        /// </summary>
        protected void SetParentButtonEvent()
        {
            //this.Master.OperatorID = Constant.EquChangeReport;
            //this.Master.IsCheckRight = true;
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_ExportExcel_Click += new Global_BtnClick(btnToExcel_Click);
            this.Master.ShowQueryButton(true);
            this.Master.ShowExportExcelButton(true);
            this.Master.MainID = "1";     //设置页面的ID编号，如果为查询页面，则设置为1
        }


        #endregion

        void Master_Master_Button_Query_Click()
        {
            getCondition();

            LoadData();
        }

        private string frmTitle = "按优先级统计问题单数";
        StringBuilder sbGrid = new StringBuilder();
        DataTable dtQuestion;
        public string SysType=RptRequestType.IssuesDayCount.ToString();
        public int UserId
        {
            set;
            get;
        }
     

        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtonEvent();
            UserId = int.Parse(Session["UserID"].ToString());
            if (!IsPostBack)
            {
                initPage();

                getCondition();

                LoadData();
            }            
        }

        private void initPage()
        {
            string sQueryBeginDate = "0";
            if (CommonDP.GetConfigValue("Other", "ReportBeginDate") != null)
                sQueryBeginDate = CommonDP.GetConfigValue("Other", "ReportBeginDate").ToString();

            #region 由当前年的第一个月的第一天，改成当前年的当前月的第一天

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            string begindate = year + "-" + month + "-" + "1";
            ctrDateTime.BeginTime = begindate;
            #endregion

            ctrDateTime.EndTime = DateTime.Now.ToString("yyyy-MM-dd");

            
        }

        private void getCondition()
        {
            
        }

        private void LoadData()
        {

            if (string.IsNullOrEmpty(ctrDateTime.BeginTime) || string.IsNullOrEmpty(ctrDateTime.EndTime))
            {
                Epower.DevBase.BaseTools.PageTool.MsgBox(this.Page, "请输入查询的日期.");
                return;
            }

            DateTime startTime = DateTime.Parse(ctrDateTime.BeginTime);
            DateTime endTime = DateTime.Parse(ctrDateTime.EndTime);
        


            dtQuestion = ReportDP.GetQuetByPriority(startTime, endTime);
            //DataTable dt3 = FlashCS.rtnTBL(dtQuestion, "Name", "RegSysDate", "Num");

            //ReportDiv.InnerHtml = FlashCS.MSPublicFlashUrl3D(dt3, "Name", "按天统计工程师工作量", "数量", "天", "../FlashReoport/Flash/MSLine.swf", "100%", "248", true, 2);

            ReportDiv.InnerHtml = FlashCS.PublicFlashUrl2D(
                                                       dtQuestion,
                                                       "DISPNAME", "QUESTIONCOUNT",
                                                       frmTitle, "数量", "优先级",
                                                       "../FlashReoport/Flash/Column2D.swf", "100%", "248", true, 2);

            //DataTable dt2 = FlashCS.rtnTBL(dtQuestion, "RegSysDate", "Name", "Num");


            bindTalbe();
        }

        private void bindTalbe()
        {
            if (dtQuestion == null || dtQuestion.Rows.Count == 0)
            {
                divGrid.InnerHtml = "";
                return;
            }
            sbGrid.Remove(0, sbGrid.Length);
            sbGrid.Append("<table cellspacing=\"1\" cellpadding=\"1\"  border=\"0\" class=\"listContent\" border=\"1\" id=\"tableGrid\" style=\"width:100%;\">");
            sbGrid.Append("<tr class=\"listTitleNew_1\" align=\"center\">");

            sbGrid.AppendFormat("<td>{0}</td>", "优先级");
            sbGrid.AppendFormat("<td>{0}</td>", "合计");

            foreach (DataRow dr in dtQuestion.Rows)
            {
                sbGrid.Append("<tr class=\"listTitleNoAlign\" align=\"center\" style=\"background-color:White;height:25px;\">");
                for (int i = 0; i < dtQuestion.Columns.Count; i++)
                {
                    sbGrid.AppendFormat("<td>{0}</td>", dr[i].ToString());
                }
                sbGrid.Append("</tr>");
            }
            sbGrid.Append("</table>");
            divGrid.InnerHtml = sbGrid.ToString();
        }     

        /// <summary>
        /// 
        /// </summary>
        protected void btnToExcel_Click()
        {
            try
            {
                LoadData(); ;
                ToExcel();
            }
            catch { }
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        private void ToExcel()
        {
            try
            {
                System.IO.StringWriter tw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);

                hw.WriteFullBeginTag("html");
                hw.WriteLine();
                hw.WriteFullBeginTag("head");
                hw.WriteLine();
                hw.WriteLine("<meta http-equiv=Content-Type Content=text/html; charset=utf-8>");
                hw.WriteEndTag("head");
                hw.WriteLine();
                hw.WriteFullBeginTag("body");
                hw.WriteLine();

                hw.WriteLine("<table><tr><td><font size=\"3\">" + frmTitle + "</font></td></tr></table>");
                hw.Write(divGrid.InnerHtml);
                hw.WriteEndTag("body");
                hw.WriteLine();
                hw.WriteEndTag("html");

                //输出到.xls文件
                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(frmTitle, System.Text.Encoding.UTF8) + ".xls");
                Response.Charset = "utf-8";
                Response.Write(tw.ToString());
                Response.End();
            }
            catch
            {
            }
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void dpdMastCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
