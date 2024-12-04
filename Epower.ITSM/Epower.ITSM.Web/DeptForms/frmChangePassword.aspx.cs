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
	/// frmModUser 的摘要说明。
	/// </summary>
	public partial class frmChangePassword : System.Web.UI.Page
	{

		//long deptid=0;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			//deptid = Convert.ToInt32(Session["UserDeptID"].ToString());
			if(!IsPostBack)
			{
				hidUserID.Value=hidUserID.Value=Session["UserID"].ToString();
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

			string UserName=Session["UserName"].ToString().Trim().ToLower();			
			if(System.Configuration.ConfigurationSettings.AppSettings["ChangeADPSW"].Trim().Equals("1") && 
			   !UserName.Equals("sa") &&
			   !UserName.Equals("admin"))
			{

				//修改AD密码
				try
				{
					ADTool.ChangeUserPasswordByAccount(txtLoginName.Text.Trim() ,this.txtOldPwd.Text.Trim(),this.txtFistPwd.Text.Trim());  

					
				}
				catch(Exception ex)
				{
					if(ex.Message.Equals("调用的目标发生了异常。"))
					{
						PageTool.MsgBox(this,System.Configuration.ConfigurationSettings.AppSettings["PSWPolicy"]);
					}
					else if(ex.Message.IndexOf("设置到对象的实例") >= 0)
					{
                        PageTool.MsgBox(this, System.Configuration.ConfigurationSettings.AppSettings["PSWNotFindInAD"]);
					}
					else
                        PageTool.MsgBox(this, ex.Message);
					//PageTool.MsgBox(this,"修改AD密码出错,请与系统管理员联系。");
					return;
				}
			}


			UserEntity user=new UserEntity(StringTool.String2Long(hidUserID.Value));
			//user.TelNo = this.txtTelNo.Text.Trim();
			//user.Mobile = this.txtMobile.Text.Trim();
			user.Password =this.txtFistPwd.Text.ToString();
			try
			{
				UserControls.AddUser(user);
				hidUserID.Value=user.UserID.ToString();
                //PageTool.AddJavaScript(this,"window.close();window.opener.location.href=window.opener.location.href;window.opener.location.reload();");
				PageTool.MsgBox(this,"修改成功！");
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
