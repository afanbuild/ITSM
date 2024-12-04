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

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// frmCatalogContent 的摘要说明。
	/// </summary>
	public partial class frmCatalogContent : BasePage
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
            if (!this.IsPostBack)
            {
                string SubjectID = "1";
                if (Request.QueryString["CurrCatalogID"] != null)
                {
                    SubjectID = Request.QueryString["CurrCatalogID"].ToString();
                    Session["OldCatalogID"] = SubjectID;
                }
                if (Session["OldCatalogID"] != null)
                {
                    SubjectID = Session["OldCatalogID"].ToString();
                }
                Response.Write("<SCRIPT>window.parent.cataloginfo.location='frmcatalogedit.aspx?catalogid=" + SubjectID + "';</SCRIPT>");
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
