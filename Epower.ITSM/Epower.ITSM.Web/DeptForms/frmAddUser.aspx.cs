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
	/// frmAddUser ��ժҪ˵����
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

		#region Web ������������ɵĴ���
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: �õ����� ASP.NET Web ���������������ġ�
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
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
