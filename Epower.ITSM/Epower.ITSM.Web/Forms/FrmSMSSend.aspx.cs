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
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// FrmSMSSend ��ժҪ˵����
	/// </summary>
	public partial class FrmSMSSend : BasePage
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			CtrTitle1.Title="���Ͷ���Ϣ";
            if (!IsPostBack)
            {
                if (Request["UserID"] != null)
                {
                    UserPicker1.UserID = long.Parse(Request["UserID"].ToString());
                    UserPicker1.UserName = UserDP.GetUserName(long.Parse(UserPicker1.UserID.ToString().Trim()));
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

		protected void cmdSend_Click(object sender, System.EventArgs e)
		{
            if (UserPicker1.UserID.ToString() == "" || UserPicker1.UserID.ToString() == "0")
			{
                PageTool.MsgBox(Page, "��ѡ�������!");
				return;
			}

            if (txtContent.Text.Trim() == string.Empty)
            {
                PageTool.MsgBox(Page, "��������Ϣ����!");
                return;
            }

            SMSDp.SaveSMS(0, (long)Session["UserID"], UserPicker1.UserID, txtContent.Text.Trim());

			PageTool.SelfClose(this);
			
			
		}
	}
}
