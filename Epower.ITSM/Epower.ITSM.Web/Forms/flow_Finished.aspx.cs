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
using EpowerGlobal;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// flow_Finished ��ժҪ˵����
	/// </summary>
	public partial class flow_Finished : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			long lngMessageID = long.Parse(Request.QueryString["MessageID"]);
			//string strUrl = HttpUtility.UrlDecode(Request.QueryString["OUrl"]);

            //2007-09-15 ��¼ ���Ⱥ� ��ҳ��
            Session["AttemperBackUrl"] = "flow_Finished.aspx?MessageID=" + lngMessageID.ToString();



			MessageEntity msgObject = new MessageEntity(lngMessageID);

			Session["MessageObject"] = msgObject;

			if(msgObject.MessageID == 0)
			{
				UIGlobal.MsgBox(this,"����������ڣ�");

			}
			
			if(msgObject.ActorType == e_ActorClass.fmMasterActor)
			{
				Response.Redirect("OA_Finished.htm");
			}
			else
			{
				Response.Redirect("OA_Finished_Read.htm");
			}

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
