/********************************************************%
 * description:��ʾ����ҳ��
 * 
 * 
 * create by :zhumingchun
 * create date:2006-11-14
 * ******************************************************/
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
using Epower.ITSM.SqlDAL;

using Microsoft.Reporting.WebForms;

namespace Epower.ITSM.Web.Reports
{
    public partial class frmReportMain : BasePage
    {
        /// <summary>
        /// �ж��Ƿ��¼��־
        /// </summary>
        protected string logined = "false";

        /// <summary>
        ///���ʱ���ʽ 0 URLֱ�ӷ��ʷ�ʽ��1 �����¼���ʷ�ʽ
        /// </summary>
        public string VisitReportMode
        {
            get
            {
                return CommonDP.GetConfigValue("PrintMode", "VisitReportMode");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // �ڴ˴������û������Գ�ʼ��ҳ��
            if (!IsPostBack)
            {
                this.InitPage();
            }
        }

        /// <summary>
        /// ��ʼ��ҳ��
        /// </summary>
        protected void InitPage()
        {
            //���������
            string reportServer = CommonDP.GetConfigValue("PrintMode", "ReportServer");
            string pStrReportPath = CommonDP.GetConfigValue("PrintMode", "ReportPath");
            //��������
            string pReportName = Request["ReportName"].ToString();

            string reportuser = CommonDP.GetConfigValue("PrintMode", "ReportUser"); 
            string reportpwd = CommonDP.GetConfigValue("PrintMode", "ReportPwd");
            string domain = CommonDP.GetConfigValue("PrintMode", "ReportDomain");

            MyReportViewerCredential rvc = new MyReportViewerCredential(reportuser, reportpwd, domain);
            Uri uri = new Uri(reportServer);
            rptViewer.ServerReport.ReportServerCredentials = rvc;
            rptViewer.ServerReport.ReportServerUrl = uri;
            rptViewer.ServerReport.ReportPath = pStrReportPath + pReportName;

            //���ò���
            SetParam();

            if (Request["Toolbar"] != null)
            {
                if (Request["Toolbar"].ToString().ToLower() == "true")
                    rptViewer.ShowToolBar = true;
                else
                    rptViewer.ShowToolBar = false;
            }
            if (Request["Parameters"] != null)
            {
                if (Request["Parameters"].ToString().ToLower() == "true")
                    rptViewer.ShowParameterPrompts = true;
                else
                    rptViewer.ShowParameterPrompts = false;
            }
            rptViewer.ShowReportBody = true;
            rptViewer.Visible = true;
            //rptViewer.ServerReport.Refresh(); 
        }

        /// <summary>
        /// ���ñ������
        /// </summary>
        protected void SetParam()
        {
            if (Request["ReportParam"] != null)
            {
                string sReportParam = Request["ReportParam"].ToString();
                string sReportParamValue = Request["ReportParamValue"].ToString();
                string[] sArrParam = sReportParam.Split(',');
                string[] sArrParamValue = sReportParamValue.Split(',');
                ReportParameter[] parameters = new ReportParameter[1];
                ReportParameter parameter = new ReportParameter(); 
                for (int i = 0; i < sArrParam.Length; i++)
                {
                    parameter.Name = sArrParam[i].ToString().Trim();
                    parameter.Values.Add(sArrParamValue[i].ToString().Trim());
                    parameters[0] = parameter;
                    rptViewer.ServerReport.SetParameters(parameters);
                }
                
            }
        }

        /// <summary>
        /// ģ���¼
        /// </summary>
        protected void login()
        {
            this.RegisterClientScriptBlock("login", "<script>goLogin()</script>");

        }
    }
}
