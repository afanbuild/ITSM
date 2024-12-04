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

namespace Epower.ITSM.Web.AnalsysForms
{
    public partial class frm_Rpt_Event_YearCountSummary : BasePage
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
            LoadData();
        }

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

            BR_RPT_EngineerDP engineer = new BR_RPT_EngineerDP().GetByType(UserId, SysType);
            if (engineer != null)
            {
                Sjwxr.UserID = engineer.EngineerIds;
                Sjwxr.UserName = engineer.EngineerNames;
            }
        }
        
        private void LoadData()
        {

            if (string.IsNullOrEmpty(ctrDateTime.BeginTime) || string.IsNullOrEmpty(ctrDateTime.EndTime))
            {
                
                return;
            }
            DataTable dt;
            DateTime startTime = DateTime.Parse(ctrDateTime.BeginTime);
            DateTime endTime = DateTime.Parse(ctrDateTime.EndTime);
            string engineerIds = Sjwxr.UserID;               

            dt = ReportDP.GetCst_IssuesDayCount(startTime, endTime, engineerIds);
            DataTable dt3 = FlashCS.rtnTBL(dt, "Name", "RegSysDate", "Num");

            ReportDiv.InnerHtml = FlashCS.MSPublicFlashUrl3D(dt3, "Name", "按天统计工程师工作量", "数量", "天", "../FlashReoport/Flash/MSLine.swf", "100%", "248", true, 2);

            DataTable dt2 = FlashCS.rtnTBL(dt, "RegSysDate", "Name", "Num");

            string strdivGrid = "";
            if (dt2 != null && dt2.Rows.Count > 0)
            {
                strdivGrid += "<table cellspacing=\"1\" cellpadding=\"1\"  border=\"0\" class=\"listContent\" border=\"1\" id=\"tableGrid\" style=\"width:100%;\">";
                strdivGrid += "<tr class=\"listTitleNew_1\" align=\"center\">";

                ArrayList alist = new ArrayList();

                //拼表头 
                for (int i = 0; i < dt2.Columns.Count; i++)
                {
                    if (i == 0)
                    {
                        strdivGrid += "<td>日期</td>";

                    }
                    else
                    {
                        strdivGrid += "<td>" + dt2.Columns[i].ColumnName + "</td>";
                    }
                    

                    alist.Add(dt2.Columns[i].ColumnName);
                }
               
                strdivGrid += "</tr>";

                //拼内容

                foreach (DataRow dr in dt2.Rows)
                {
                    strdivGrid += "<tr class=\"listTitleNoAlign\" align=\"center\" style=\"background-color:White;height:25px;\">";
                    int num = 0;
                    for (int j = 0; j < alist.Count; j++)
                    {
                        if (j == 0)
                        { 
                            strdivGrid += "<td>" + dr[alist[j].ToString()] + "</td>";
                             
                        }
                        else
                        {
                            strdivGrid += "<td>" + (dr[alist[j].ToString()].ToString() == "" ? "0" : dr[alist[j].ToString()].ToString()) + "</td>";
                            num += dr[alist[j].ToString()].ToString() == "" ? 0 : Convert.ToInt32(dr[alist[j].ToString()]);
                        }
                    }
                   
                    strdivGrid += "</tr>";
                }
                strdivGrid += "<tr class=\"listTitleNew_1\" align=\"center\">";
                
                
                //拼合计

                for (int i = 0; i < dt2.Columns.Count; i++)
                {
                    long num = 0;
                    if (i == 0)
                    {
                        strdivGrid += "<td>合计</td>";

                    }
                    if (i != 0)
                    {
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            if (dt2.Columns[i].ColumnName.Contains(dt.Rows[j]["name"].ToString()))
                            {
                                if (dt.Rows[j]["num"] != null && !string.IsNullOrEmpty(dt.Rows[j]["num"].ToString()))
                                {
                                    num += Convert.ToInt64(dt.Rows[j]["num"].ToString());
                                }
                            }
                        }
                        strdivGrid += "<td>" + num.ToString() + "</td>";
                    }
                }
               
                strdivGrid += "</tr>";
                strdivGrid += "</table>";
            }

            divGrid.InnerHtml = strdivGrid;

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
        /// 
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

                hw.WriteLine("<table><tr><td><font size=\"3\">按天统计工程师工作量</font></td></tr></table>");
                hw.Write(divGrid.InnerHtml);
                hw.WriteEndTag("body");
                hw.WriteLine();
                hw.WriteEndTag("html");

                //输出到.xls文件
                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("按天统计工程师工作量", System.Text.Encoding.UTF8) + ".xls");
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
