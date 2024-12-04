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

namespace Epower.ITSM.Web.AppForms
{
	/// <summary>
	/// frmSelectStaffRight ��ժҪ˵����
	/// </summary>
    public partial class Allfrmselectstaffright : BasePage
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
			dlUsers.DataSource=dt.DefaultView;
			dlUsers.DataBind();
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
