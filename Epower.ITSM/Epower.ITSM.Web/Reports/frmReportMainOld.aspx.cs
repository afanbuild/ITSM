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

namespace Epower.ITSM.Web.Reports
{
    public partial class frmReportMainOld : BasePage
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

            //���ò���
            SetParam();
            rptViewer.ServerUrl = reportServer;
            rptViewer.ReportPath = pStrReportPath + pReportName;

            if (Request["Toolbar"] != null)
            {
                if (Request["Toolbar"].ToString().ToLower() == "true")
                    rptViewer.Toolbar = Microsoft.Samples.ReportingServices.ReportViewer.multiState.True;
                else
                    rptViewer.Toolbar = Microsoft.Samples.ReportingServices.ReportViewer.multiState.False;
            }
            if (Request["Parameters"] != null)
            {
                if (Request["Parameters"].ToString().ToLower() == "true")
                    rptViewer.Parameters = Microsoft.Samples.ReportingServices.ReportViewer.multiState.True;
                else
                    rptViewer.Parameters = Microsoft.Samples.ReportingServices.ReportViewer.multiState.False;
            }
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
                for (int i = 0; i < sArrParam.Length; i++)
                {
                    rptViewer.SetParameter(sArrParam[i].ToString().Trim(), sArrParamValue[i].ToString().Trim());
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
