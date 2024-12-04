/****************************************************************************
 * 
 * description:科目左边树页面
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

namespace Epower.ITSM.Web.InformationManager
{
    /// <summary>
    /// 
    /// </summary>
    public partial class frmInformationContent : BasePage
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
                if (sType == "0")          //知识库类型维护
                {
                    sReturn = "frmSubjectedit.aspx?1=1";
                }
                else if(sType =="1")  //知识库维护
                {
                    sReturn = "frmInf_InformationMain.aspx?1=1";
                }
                else if (sType == "2")  //知识库查看
                {
                    sReturn = "frmInf_InformationMain.aspx?IsGaoji=true";
                }
                else   //知识库查看
                {
                    sReturn = "frmInf_InformationMain.aspx?IsSelect=1&IsGaoji=true";
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
                if (sType == "0" || sType == "1")   //如果不为知识类别维护，知识维护
                    CtrSubjecttree1.InformationLimit = false;
                else
                    CtrSubjecttree1.InformationLimit = true;
                string SubjectID = "1";
                if (Request.QueryString["CurrSubjectID"] != null)
                {
                    SubjectID = Request.QueryString["CurrSubjectID"].ToString();
                }
                if (Session["OldSubjectID"] != null)
                {
                    SubjectID = Session["OldSubjectID"].ToString();
                }
                Response.Write("<SCRIPT>window.parent.subjectinfo.location='" + sUrl + "&subjectid=" + SubjectID + "';</SCRIPT>");
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
