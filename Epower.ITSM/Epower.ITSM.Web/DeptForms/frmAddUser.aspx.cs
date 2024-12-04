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
using Epower.DevBase.Organization;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.BaseTools;
namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// frmAddUser 的摘要说明。
	/// </summary>
	public partial class frmAddUser : BasePage
	{
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator4;
		protected System.Web.UI.WebControls.Button Button3;
		private long deptid ;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			deptid = Convert.ToInt32(Request.Params.Get("deptId"));
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

		protected void Button1_Click(object sender, System.EventArgs e)
		{
			UserEntity user = new UserEntity();
			user.DeptID = deptid;
			user.LoginName = this.txtLoginName.Text.Trim();
			user.Name = this.txtName.Text.Trim();
			user.Password = this.txtFistPwd.Text.Trim();
			user.Sex = this.dropSex.SelectedItem.Text;
			user.TelNo = this.txtTelNo.Text.Trim();
			user.Job = this.txtJob.Text.Trim();
			user.Email = this.txtEmail.Text.Trim();
			user.EduLevel = this.dropEdu.SelectedItem.Text;
			user.School = this.txtRole.Text.Trim();
			UserControls.AddUser(user);
            PageTool.AddJavaScript(this, "window.opener.location.href=window.opener.location.href;window.close();");
		}

	}
}
