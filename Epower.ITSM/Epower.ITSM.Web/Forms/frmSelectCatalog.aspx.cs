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
	/// frmSelectCatalog 的摘要说明。
	/// </summary>
	public partial class frmSelectCatalog : BasePage
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			if(Request.QueryString["LimitCurr"] != null)
			{
				CtrCatalogTree1.LimitCurr = bool.Parse(Request.QueryString["LimitCurr"]);
			}
			if(Request.QueryString["CurrCatalogID"] != null)
			{
				CtrCatalogTree1.CurrCatalogID = long.Parse(Request.QueryString["CurrCatalogID"]);
				// 记录当前部门
				Session["OldCatalogID"] = CatalogDP.GetCatalogParentID(CtrCatalogTree1.CurrCatalogID);
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
