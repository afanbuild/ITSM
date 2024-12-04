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
using EpowerCom;
using Epower.ITSM.Base;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// Rise_FormX ��ժҪ˵����
	/// </summary>
	public partial class OA_FormX : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			
			// �ڴ˴������û������Գ�ʼ��ҳ��

			long lngFlowModelID = (long)Session["FlowModelID"];
			string strPage = epApp.GetStartWebFormByFlowModelP(lngFlowModelID);

            Response.Redirect(strPage + "?FlowModelID=" + lngFlowModelID.ToString() + "&MessageID=0");
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
