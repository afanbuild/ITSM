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
//using Epower.DevBase.BaseTools;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// frmModUser 的摘要说明。
	/// </summary>
	public partial class frmModUser : BasePage
	{
		//protected System.Web.UI.WebControls.TextBox txtMobile;
		//protected System.Web.UI.WebControls.TextBox txtTelNo;

		//long deptid=0;

        /// <summary>
        /// 设置母版页面按钮
        /// </summary>
        protected void SetParentButtonEvent()
        {
            //this.Master.TableVisible = false;
        }

		protected void Page_Load(object sender, System.EventArgs e)
		{
            SetParentButtonEvent();
			Literal1.Text="";
			
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
				PageTool.MsgBox(this,"两次输入的密码不一致！");
				return;
			}

			int iSuccess=ChangePassword(hidUserID.Value,txtLoginName.Text.Trim(),txtName.Text.Trim(),"", txtFistPwd.Text,true);

			if(iSuccess==0)
			{
				

			}




		}



		private int ChangePassword(string UserID,string LoginName,string UserName,string OldPwd,string NewPwd,bool bTips)
		{
			int iSuccess=-1;  // -1 改密码失败 ； 0 改密码成功
			
			UserEntity user=new UserEntity(StringTool.String2Long(UserID));

			user.Password =NewPwd;

			try
			{
				UserControls.AddUser(user);
				hidUserID.Value=user.UserID.ToString();

                //PageTool.AddJavaScript(this,"window.close();window.opener.location.href=window.opener.location.href;window.opener.location.reload();");
				iSuccess=0;
                if (bTips) PageTool.MsgBox(this,"修改成功！");
			}
			catch(Exception ex)
			{
				if(ex.Message.CompareTo("登录名称已经存在") == 0)
				{
                    if (bTips) PageTool.MsgBox(this,ex.Message);
				}
				else
				{
                    if (bTips) PageTool.MsgBox(this,"保存用户资料时出现错误\n错误为:" + ex.Message.ToString());
				}
			}

			return iSuccess;

		}



	}
}
