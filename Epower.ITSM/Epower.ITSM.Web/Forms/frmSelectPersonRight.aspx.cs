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
using Epower.DevBase.Organization;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// frmSelectPersonRight 的摘要说明。
	/// </summary>
	public partial class frmSelectPersonRight : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				long deptId = Convert.ToInt32(Request.Params.Get("DeptID"));
				LoadData(deptId);
			}
		}

		private void LoadData(long lngDeptId)
		{
			DataTable dt=DeptControl.GetDeptUserList(lngDeptId);

			lsbStaff.DataSource=dt.DefaultView;
			lsbStaff.DataTextField ="Name";
			lsbStaff.DataValueField ="userid";
			lsbStaff.DataBind();
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
