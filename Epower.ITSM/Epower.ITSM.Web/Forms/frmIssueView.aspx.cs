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

namespace Epower.ITSM.Web.Forms
{
    /// <summary>
    /// frmIssueView 的摘要说明。
    /// </summary>
    public partial class frmIssueView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
		{
			long lngFlowID = 0;
            long lngMessageID=0;
			long lngUserID = (long)Session["UserID"];
			if(Request.QueryString["FlowID"]!= null)
			{
				lngFlowID = long.Parse(Request.QueryString["FlowID"]);
			}
            if(Request.QueryString["MessageID"]!= null)
			{
				lngMessageID = long.Parse(Request.QueryString["MessageID"]);
			}

            //如果是弹出新窗口,则工作流表单上的退出,统一执行关闭窗口
            if (Request.QueryString["NewWin"] != null)
            {
                if(Request.QueryString["NewWin"].ToLower() =="true")
                     Session["FromUrl"] = "close";
            }
            if(lngMessageID>0)
            {
                Response.Redirect(Epower.ITSM.SqlDAL.FlowDP.GetProcessORViewUrlV2(lngMessageID, lngUserID));
            }
            else
             {
			    Response.Redirect(Epower.ITSM.SqlDAL.FlowDP.GetProcessORViewUrl(lngFlowID,lngUserID));
             }


		}

        #region Web 窗体设计器生成的代码
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion
    }
}
