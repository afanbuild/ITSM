/********************************************************%
 * description:显示报表页面
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
        /// 判断是否登录标志
        /// </summary>
        protected string logined = "false";

        /// <summary>
        ///访问报表方式 0 URL直接访问方式，1 摸拟登录访问方式
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
            // 在此处放置用户代码以初始化页面
            if (!IsPostBack)
            {
                this.InitPage();
            }
        }

        /// <summary>
        /// 初始化页面
        /// </summary>
        protected void InitPage()
        {
            //报表服务器
            string reportServer = CommonDP.GetConfigValue("PrintMode", "ReportServer");
            string pStrReportPath = CommonDP.GetConfigValue("PrintMode", "ReportPath");
            //报表名称
            string pReportName = Request["ReportName"].ToString();

            //设置参数
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
        /// 设置报表参数
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
        /// 模拟登录
        /// </summary>
        protected void login()
        {
            this.RegisterClientScriptBlock("login", "<script>goLogin()</script>");

        }
    }
}
