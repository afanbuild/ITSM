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
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// frmDelayFlow ��ժҪ˵����
	/// </summary>
	public partial class frmDelayFlow : System.Web.UI.Page
	{

		long lngFlowID = 0;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			lngFlowID = long.Parse(Request.QueryString["flowid"]);
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

		protected void cmdDelay_Click(object sender, System.EventArgs e)
		{
			EpowerCom.Flow.UpdateFlowExpectendTime(lngFlowID,int.Parse(txtMinute.Text));
			PageTool.SelfClose(this);

		}
	}
}
