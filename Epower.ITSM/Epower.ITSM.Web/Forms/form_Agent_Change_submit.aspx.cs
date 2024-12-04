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
using EpowerCom;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// form_Agent_Change_submit 的摘要说明。
	/// </summary>
	public partial class form_Agent_Change_submit : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{            

			// 在此处放置用户代码以初始化页面
			long lngUserID = (long)Session["UserID"];
			long lngAppID = long.Parse(Request.QueryString["AppID"]);
			long lngAgentID = long.Parse(Request.QueryString["AgentID"]);

			if(lngAppID != 0)
			{
				Miscellany.SetAgent(lngUserID,lngAppID,lngAgentID);
			}
			else
			{
				Miscellany.SetAgent(lngUserID,lngAgentID);
			}

			Response.Write("<script>window.parent.opener.location.reload();</script>");
			Response.Write("<script>window.parent.close();</script>");
			

		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN：该调用是 ASP.NET Web 窗体设计器所必需的。
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
