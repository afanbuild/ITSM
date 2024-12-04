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

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// frmDeptContForUser ��ժҪ˵����
	/// </summary>
	public partial class frmDeptContForUser : BasePage
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			if(!this.IsPostBack)
			{
				string DeptID="0";
				if(Session["OldDeptID"] !=null)
					DeptID=Session["OldDeptID"].ToString();
				else
					DeptID=Session["RootDeptID"].ToString();

                if (Session["RootDeptID"] != null && Session["RootDeptID"].ToString() != "0")
                {
                    Response.Write("<SCRIPT>window.parent.userinfo.location='frmUserQuery.aspx?DeptID=" + DeptID + "';</SCRIPT>");
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