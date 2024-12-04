/****************************************************************************
 * 
 * description:年度工作绩效分析
 * 
 * 
 * 
 * Create by:
 * Create Date:2008-03-12
 * *************************************************************************/
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.SqlDAL.Flash;
namespace Epower.ITSM.Web.AnalsysForms
{
	/// <summary>
	/// form_RptMenu 的摘要说明。
	/// </summary>
    public partial class form_RptMenu : BasePage
	{
        #region 设置父窗体按钮事件SetParentButtonEvent
        /// <summary>
        /// 设置父窗体按钮事件

        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_ExportExcel_Click += new Global_BtnClick(btnToExcel_Click);
            this.Master.ShowExportExcelButton(true);
        }


        #endregion

        #region Page_Load
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Page_Load(object sender, System.EventArgs e)
		{
            SetParentButtonEvent();
			if(!IsPostBack)
			{
				LoadData();
			}
        }
        #endregion 

        #region LoadData
        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
		{
			DataTable dt=FlowDP.GetFlowYears();
			DataView dv=new DataView(dt);
			dv.Sort="years";
          
			dpdYear.DataSource=dv;
			dpdYear.DataTextField="years";
			dpdYear.DataValueField="years";
			dpdYear.DataBind();

			int nYear=Epower.DevBase.BaseTools.StringTool.String2Int(dpdYear.SelectedValue);
			long nOrgID=long.Parse(Session["UserOrgID"].ToString());
			if(System.Configuration.ConfigurationSettings.AppSettings["SystemModel"] != "1")
				nOrgID=0;

			dt=FlowDP.GetAnalysisWorkQuantity(nYear,nOrgID);

            ReportDiv.InnerHtml = FlashCS.MSPublicFlashUrl3D(dt, "nYear", "年度工作量分析", "数量", "月份", "../FlashReoport/Flash/MSLine.swf", "100%", "248", true, 0);

			dt=FlowDP.GetAnalysisWorkEfficiency(nYear,nOrgID);

            ReportDiv2.InnerHtml = FlashCS.MSPublicFlashUrl3D(dt, "Status", "年度效率分析", "数量", "月份", "../FlashReoport/Flash/MSLine.swf", "100%", "248", true, 0);
           
        }
        #endregion 

        #region dpdYear_SelectedIndexChanged
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void dpdYear_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            //zxl===
            int nYear = Epower.DevBase.BaseTools.StringTool.String2Int(dpdYear.SelectedValue);
            long nOrgID = long.Parse(Session["UserOrgID"].ToString());
            if (System.Configuration.ConfigurationSettings.AppSettings["SystemModel"] != "1")
                nOrgID = 0;

            DataTable dt = FlowDP.GetAnalysisWorkQuantity(nYear, nOrgID);

            ReportDiv.InnerHtml = FlashCS.MSPublicFlashUrl3D(dt, "nYear", "年度工作量分析", "数量", "月份", "../FlashReoport/Flash/MSLine.swf", "100%", "248", true, 0);

            dt = FlowDP.GetAnalysisWorkEfficiency(nYear, nOrgID);

            ReportDiv2.InnerHtml = FlashCS.MSPublicFlashUrl3D(dt, "Status", "年度效率分析", "数量", "月份", "../FlashReoport/Flash/MSLine.swf", "100%", "248", true, 0);
           
           
			//LoadData();

        }
        #endregion 

        #region 导出Excel btnToExcel_Click
        /// <summary>
        /// 导出Excel
        /// </summary>
        protected void btnToExcel_Click()
        {
            try
            {
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

                hw.WriteLine("<table><tr><td><font size=\"3\">年度工作绩效分析(工作时间)</font></td></tr></table>");
                this.tdResultChart.RenderControl(hw);


                hw.WriteEndTag("body");
                hw.WriteLine();
                hw.WriteEndTag("html");

                //输出到.xls文件
                Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("Content-Disposition","attachment; filename=服务事件趋势分析(事件类型).xls");  --会乱码
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("年度工作绩效分析(工作时间)", System.Text.Encoding.UTF8) + ".xls");
                Response.Charset = "utf-8";
                Response.Write(tw.ToString());
                Response.End();
            }
            catch
            {
            }
        }
        #endregion
    }
}
