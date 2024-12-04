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
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// frmSetPassword 的摘要说明。
	/// </summary>
	public partial class frmSetPassword : BasePage
	{

		//long deptid=0;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			lblMsg.Attributes.Add("onclick","SetMsg()");

			//只有sa 和 admin才有权限重设密码
			string UserName=Session["UserName"].ToString().Trim().ToLower();			
			if(!UserName.Equals("sa") && !UserName.Equals("admin"))
			{
				Response.Write("<Script>alert('对不起，您的帐户没有足够的权限重设其他用户密码！');opener.top.document.location='../Default.aspx';window.close();</Script>");
			}

			if(!IsPostBack)
			{
				hidUserID.Value=Request.QueryString["userId"].ToString();
				LoadData();
			}
		}

		private void LoadData()
		{
			
			if(StringTool.String2Long(hidUserID.Value)!=0)
			{
				UserEntity ue=new UserEntity(StringTool.String2Long(hidUserID.Value));
				this.txtLoginName.Text = ue.LoginName.ToString();
				this.txtFistPwd.Text = ue.Password;//"*****";
				this.txtLastPwd.Text = ue.Password;//txtFistPwd.Text;
				this.txtName.Text = ue.Name;
				this.hidPassWord.Value =ue.Password;
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

		protected void cmdSave_Click(object sender, System.EventArgs e)
		{
			if(!txtFistPwd.Text.Equals(txtLastPwd.Text ))
			{
				PageTool.MsgBox(this,"两次输入的新密码不一致！");
				return;
			}

			UserEntity user=new UserEntity(StringTool.String2Long(hidUserID.Value));
			user.Password =this.txtFistPwd.Text.ToString();
			try
			{
				UserControls.AddUser(user);
				hidUserID.Value=user.UserID.ToString();
                //PageTool.MsgBox(this,"修改成功！");
                PageTool.AddJavaScript(this, "alert('修改成功！'); window.close();");
			}
			catch(Exception ex)
			{
				if(ex.Message.CompareTo("登录名称已经存在") == 0)
				{
                    PageTool.MsgBox(this, ex.Message);
				}
				else
				{
					PageTool.MsgBox(this,"保存用户资料时出现错误\n错误为:"+ex.Message.ToString());
				}
			}

		}

	}
}
