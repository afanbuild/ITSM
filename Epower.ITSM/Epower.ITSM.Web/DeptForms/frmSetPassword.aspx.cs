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
	/// frmSetPassword ��ժҪ˵����
	/// </summary>
	public partial class frmSetPassword : BasePage
	{

		//long deptid=0;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			lblMsg.Attributes.Add("onclick","SetMsg()");

			//ֻ��sa �� admin����Ȩ����������
			string UserName=Session["UserName"].ToString().Trim().ToLower();			
			if(!UserName.Equals("sa") && !UserName.Equals("admin"))
			{
				Response.Write("<Script>alert('�Բ��������ʻ�û���㹻��Ȩ�����������û����룡');opener.top.document.location='../Default.aspx';window.close();</Script>");
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

			UserEntity user=new UserEntity(StringTool.String2Long(hidUserID.Value));
			user.Password =this.txtFistPwd.Text.ToString();
			try
			{
				UserControls.AddUser(user);
				hidUserID.Value=user.UserID.ToString();
                //PageTool.MsgBox(this,"�޸ĳɹ���");
                PageTool.AddJavaScript(this, "alert('�޸ĳɹ���'); window.close();");
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
