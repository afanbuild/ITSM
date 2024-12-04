/****************************************************************************
 * 
 * description:��ȹ�����Ч����
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
	/// form_RptMenu ��ժҪ˵����
	/// </summary>
    public partial class form_RptMenu : BasePage
	{
        #region ���ø����尴ť�¼�SetParentButtonEvent
        /// <summary>
        /// ���ø����尴ť�¼�

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

            ReportDiv.InnerHtml = FlashCS.MSPublicFlashUrl3D(dt, "nYear", "��ȹ���������", "����", "�·�", "../FlashReoport/Flash/MSLine.swf", "100%", "248", true, 0);

			dt=FlowDP.GetAnalysisWorkEfficiency(nYear,nOrgID);

            ReportDiv2.InnerHtml = FlashCS.MSPublicFlashUrl3D(dt, "Status", "���Ч�ʷ���", "����", "�·�", "../FlashReoport/Flash/MSLine.swf", "100%", "248", true, 0);
           
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

            ReportDiv.InnerHtml = FlashCS.MSPublicFlashUrl3D(dt, "nYear", "��ȹ���������", "����", "�·�", "../FlashReoport/Flash/MSLine.swf", "100%", "248", true, 0);

            dt = FlowDP.GetAnalysisWorkEfficiency(nYear, nOrgID);

            ReportDiv2.InnerHtml = FlashCS.MSPublicFlashUrl3D(dt, "Status", "���Ч�ʷ���", "����", "�·�", "../FlashReoport/Flash/MSLine.swf", "100%", "248", true, 0);
           
           
			//LoadData();

        }
        #endregion 

        #region ����Excel btnToExcel_Click
        /// <summary>
        /// ����Excel
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

                hw.WriteLine("<table><tr><td><font size=\"3\">��ȹ�����Ч����(����ʱ��)</font></td></tr></table>");
                this.tdResultChart.RenderControl(hw);


                hw.WriteEndTag("body");
                hw.WriteLine();
                hw.WriteEndTag("html");

                //�����.xls�ļ�
                Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("Content-Disposition","attachment; filename=�����¼����Ʒ���(�¼�����).xls");  --������
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("��ȹ�����Ч����(����ʱ��)", System.Text.Encoding.UTF8) + ".xls");
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
