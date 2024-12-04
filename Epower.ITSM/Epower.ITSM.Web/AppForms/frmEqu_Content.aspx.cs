/****************************************************************************
 * 
 * description:设备左边树页面
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-08-22
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
using Microsoft.Web.UI.WebControls;
using Epower.DevBase.Organization;
using Epower.DevBase.Organization.Base ;

namespace Epower.ITSM.Web.AppForms
{
    /// <summary>
    /// 
    /// </summary>
    public partial class frmEqu_Content : BasePage
	{
        /// <summary>
        /// 取得页面展示的类别
        /// </summary>
        protected string sType
        {
            get
            {
                if (Request["Type"] != null)
                    return Request["Type"].ToString();
                else
                    return "0";
            }
        }

        /// <summary>
        /// 展示地址
        /// </summary>
        protected string sUrl
        {
            get
            {
                string sReturn = string.Empty;
                if (sType == "0")          //设备类型维护
                {
                    sReturn = "frm_Services_Template.aspx";
                }
                else if (sType == "1")     //设备维护
                {
                    sReturn = "frmEqu_DeskMain.aspx";
                }
                else if (sType == "2")     //资产目录管理
                {
                    sReturn = "frmEqu_DeskCateList.aspx";
                }
                return sReturn;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Page_Load(object sender, System.EventArgs e)
		{

            if (!this.IsPostBack)
            {
                string templateID = "1";
                if (Request.QueryString["CurrTemplateID"] != null)
                {
                    templateID = Request.QueryString["CurrTemplateID"].ToString();
                    Session["OldTemplateID"] = templateID;
                }
                if (Session["OldTemplateID"] != null)
                {
                    templateID = Session["OldTemplateID"].ToString();
                }
                Response.Write("<SCRIPT>window.parent.subjectinfo.location='" + sUrl + "?subjectid=" + templateID + "';</SCRIPT>");
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
