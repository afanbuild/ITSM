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

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// frmSMSDelete ��ժҪ˵����
	/// </summary>
	public partial class frmSMSDelete : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			long lngSMSID = long.Parse(Request.QueryString["smsid"]);

			// ɾ����Ϣ

			SMSDp.DeleteSMS(lngSMSID);

			Response.Write("<script>window.parent.opener.location.reload();</script>");

			Response.Write("<script>window.close();</script>");

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
