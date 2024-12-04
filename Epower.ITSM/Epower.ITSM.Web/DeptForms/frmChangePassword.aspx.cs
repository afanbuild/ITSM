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
	/// frmModUser ��ժҪ˵����
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

		protected void cmdSave_Click(object sender, System.EventArgs e)
		{
			if(!txtFistPwd.Text.Equals(txtLastPwd.Text ))
			{
				PageTool.MsgBox(this,"��������������벻һ�£�");
				return;
			}

			string UserName=Session["UserName"].ToString().Trim().ToLower();			
			if(System.Configuration.ConfigurationSettings.AppSettings["ChangeADPSW"].Trim().Equals("1") && 
			   !UserName.Equals("sa") &&
			   !UserName.Equals("admin"))
			{

				//�޸�AD����
				try
				{
					ADTool.ChangeUserPasswordByAccount(txtLoginName.Text.Trim() ,this.txtOldPwd.Text.Trim(),this.txtFistPwd.Text.Trim());  

					
				}
				catch(Exception ex)
				{
					if(ex.Message.Equals("���õ�Ŀ�귢�����쳣��"))
					{
						PageTool.MsgBox(this,System.Configuration.ConfigurationSettings.AppSettings["PSWPolicy"]);
					}
					else if(ex.Message.IndexOf("���õ������ʵ��") >= 0)
					{
                        PageTool.MsgBox(this, System.Configuration.ConfigurationSettings.AppSettings["PSWNotFindInAD"]);
					}
					else
                        PageTool.MsgBox(this, ex.Message);
					//PageTool.MsgBox(this,"�޸�AD�������,����ϵͳ����Ա��ϵ��");
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
				PageTool.MsgBox(this,"�޸ĳɹ���");
			}
			catch(Exception ex)
			{
				if(ex.Message.CompareTo("��¼�����Ѿ�����") == 0)
				{
                    PageTool.MsgBox(this, ex.Message);
				}
				else
				{
					PageTool.MsgBox(this,"�����û�����ʱ���ִ���\n����Ϊ:"+ex.Message.ToString());
				}
			}

		}

	}
}
