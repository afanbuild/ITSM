
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL.Flash;

namespace Epower.ITSM.Web.AnalsysForms
{
    public partial class Rpt_Event_DailySummary : BasePage
    {
        int nLastYear = 0;
        //上次查询时选择的年份,优化用,避免不必要的查询

        #region 设置父窗体按钮事件SetParentButtonEvent
        /// <summary>
        /// 设置父窗体按钮事件

        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_ExportExcel_Click += new Global_BtnClick(btnToExcel_Click);
            this.Master.ShowQueryButton(true);
            this.Master.ShowExportExcelButton(true);
            this.Master.MainID = "1";     //设置页面的ID编号，如果为查询页面，则设置为1
        }

        void Master_Master_Button_Query_Click()
        {
            LoadData();
        }


        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            SetParentButtonEvent();
            this.ctrFCDServiceType.mySelectedIndexChanged += new EventHandler(ctrFCDServiceType_mySelectedIndexChanged);
            if (!IsPostBack)
            {
                ///设置起始日期
                ctrDateTime.BeginTime = DateTime.Now.AddDays(-1).AddMonths(-1).ToString("yyyy-MM-dd");
                ctrDateTime.EndTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                SetManageOfficeDboValue();
                SetMastCustomDboValue();

                LoadData();
            }
            else
            {
                if (ViewState["LastYear"] != null)
                {
                    nLastYear = (int)ViewState["LastYear"];
                }
                if (Request.Form["__EVENTTARGET"] == "datarange")
                {
                    LoadData();
                }
            }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ctrFCDServiceType_mySelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dpdYear_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            LoadData();
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dpdManageOffice_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dpdMastShortName_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetManageOfficeDboValue()
        {
            DataTable dt = ZHServiceDP.GetManageOffices();
            DataView dv = new DataView(dt);
            dv.Sort = "deptid";

            dpdManageOffice.DataSource = dv;
            dpdManageOffice.DataTextField = "deptname";
            dpdManageOffice.DataValueField = "deptid";
            dpdManageOffice.DataBind();

            dpdManageOffice.Items.Insert(0, new ListItem("全部", "0"));

            dt.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetMastCustomDboValue()
        {
            DataTable dt = ZHServiceDP.GetMastCustomer();
            DataView dv = new DataView(dt);
            dv.Sort = "ID";
            dpdMastShortName.DataSource = dv;
            dpdMastShortName.DataTextField = "ShortName";
            dpdMastShortName.DataValueField = "ID";
            dpdMastShortName.DataBind();

            dpdMastShortName.Items.Insert(0, new ListItem("全部", "0"));

            dt.Dispose();
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


            int lngDeptID = int.Parse(ctrFCDServiceType.CatelogID.ToString());
            int custId = string.IsNullOrEmpty(dpdManageOffice.SelectedItem.Value) ? 0 : int.Parse(dpdManageOffice.SelectedItem.Value);
                                 
            DataTable dt;

            dt = ReportDP.GetDailyCount(startTime, endTime, lngDeptID, custId);
            
            #region 构造横向Table
            DataTable dts=new DataTable();
            dts.Columns.Add(new DataColumn("日期",System.Type.GetType("System.String")));
            foreach(DataRow dr in dt.Rows )
            {
                dts.Columns.Add(new DataColumn(dr[0].ToString(), System.Type.GetType("System.String")));
            }
            DataRow newDr = dts.NewRow();
            newDr[0] = "个数";
            for (int i = 0; i < dt.Rows.Count;i++ )
            {
                
                newDr[i+1] = dt.Rows[i].ItemArray[1].ToString();
            }
            dts.Rows.Add(newDr);
            
            
            #endregion

            ReportDiv.InnerHtml = FlashCS.MSPublicFlashUrl3D(dts, "日期", "服务量日报表", "日期", "日期", "../FlashReoport/Flash/MSLine.swf", "100%", "248", true, 0);
            //ReportDiv.InnerHtml = FlashCS.MSPublicFlashUrl3D(dt, "月份", "年度数据分析报表", "数量", "月份", "../FlashReoport/Flash/MSLine.swf", "100%", "248", true, 0);

            #region 给页面展示列表添加合计项 2013-5-24 温旭添加
            DataRow newDR = dt.NewRow();
            int count = 0;
            foreach(DataRow dr in dt.Rows)
            {
                count += Convert.ToInt32(dr["QTY"].ToString());
            }
            newDR["regsysdate"] = "合计";
            newDR["QTY"] = count;
            dt.Rows.Add(newDR);
            #endregion


            dgTypesCount.DataSource = dt.DefaultView;
            dgTypesCount.DataBind();


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

                hw.WriteLine("<table><tr><td><font size=\"3\">服务量日报表</font></td></tr></table>");
                this.dgTypesCount.RenderControl(hw);
                // this.tdResultChart.RenderControl(hw);


                hw.WriteEndTag("body");
                hw.WriteLine();
                hw.WriteEndTag("html");

                //输出到.xls文件
                Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("Content-Disposition","attachment; filename=服务事件趋势分析(事件类型).xls");  //会乱码
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("服务量日报表", System.Text.Encoding.UTF8) + ".xls");
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
    }
}

