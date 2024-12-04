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
	/// frmCheckUser ��ժҪ˵����
	/// </summary>
	public partial class frmCheckUser : BasePage
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!this.IsPostBack)
			{
				if(this.Request.QueryString["LoginName"]!=null)
				{
					
					string sLoginName=this.Request.QueryString["LoginName"].ToString();
					UserEntity ue=new UserEntity();
					if(ue.CheckUser(sLoginName))
					{

						tbTips.Rows[0].Cells[0].Align="Left";
						lblTips.Text ="<font color=#ff0066>��½�ʺ��ѱ�������ռ�ã�</font><br><br>" +
							"�ʺţ�"+sLoginName+"<br>"+
							"������"+ ue.Name +"<br>"+
							"�Ա�"+ ue.Sex +"<br>"+
							"ְλ��"+ ue.Job +"<br>"+
							"�绰��"+ ue.TelNo +"<br>"+
							//"�ֻ���"+ue.Mobile +"<br>"+
							"Email��"+ ue.Email +"<br>"+
							"ѧ����"+ ue.EduLevel +"<br>"+
							"���ţ�"+ ue.FullDeptName +"<br>"+
							"״̬��"+ ((ue.Deleted==0)? "����":"����");
							
					}
					else
					{
						tbTips.Rows[0].Cells[0].Align="Center";
						lblTips.Text="<font color=green>��½�ʺſ��á�</font>";
					}				
				}
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
	}
}
