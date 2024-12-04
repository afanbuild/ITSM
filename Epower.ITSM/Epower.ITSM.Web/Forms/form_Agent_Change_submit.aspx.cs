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

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// form_Agent_Change_submit ��ժҪ˵����
	/// </summary>
	public partial class form_Agent_Change_submit : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{            

			// �ڴ˴������û������Գ�ʼ��ҳ��
			long lngUserID = (long)Session["UserID"];
			long lngAppID = long.Parse(Request.QueryString["AppID"]);
			long lngAgentID = long.Parse(Request.QueryString["AgentID"]);

			if(lngAppID != 0)
			{
				Miscellany.SetAgent(lngUserID,lngAppID,lngAgentID);
			}
			else
			{
				Miscellany.SetAgent(lngUserID,lngAgentID);
			}

			Response.Write("<script>window.parent.opener.location.reload();</script>");
			Response.Write("<script>window.parent.close();</script>");
			

		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN���õ����� ASP.NET Web ���������������ġ�
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
