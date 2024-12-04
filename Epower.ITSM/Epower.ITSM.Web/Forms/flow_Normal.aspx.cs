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
	/// flow_Normal ��ժҪ˵����
	/// </summary>
	public partial class flow_Normal : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			long lngMessageID = long.Parse(Request.QueryString["MessageID"]);
            string strAutoPass = "false";

            //2007-09-15 ��¼ ���Ⱥ� ��ҳ��
            Session["AttemperBackUrl"] = "flow_Normal.aspx?MessageID=" + lngMessageID.ToString();

			string strUrl = "";
			
			if(Request.QueryString["OUrl"] != null)
			{
				strUrl = HttpUtility.UrlDecode(Request.QueryString["OUrl"]);
			}

            if (Request.QueryString["autopass"] != null)
            {
                strAutoPass = Request.QueryString["autopass"];
            }
            Session["g_FlowModelAutoPass"] = strAutoPass;

            if (Request.QueryString["OUrl"] != null)
            {
                strUrl = HttpUtility.UrlDecode(Request.QueryString["OUrl"]);
            }

			// ���⴦�����󡡣����Ӵ����������Ĵ����ء���������
			if(strUrl.IndexOf("FrmContent.aspx") == 0)
			{
				Session["BackToUndoList"] = true;
				
			}
			else
			{
				Session["BackToUndoList"] = false;
			}

			Session["BackToUrl"] = (Session["FromUrl"]==null)?strUrl:Session["FromUrl"];

			MessageEntity msgObject = new MessageEntity(lngMessageID);


			Session["MessageObject"] = msgObject;

			

			
			Message.SetMessageReadStatus(lngMessageID);



			if(msgObject.MessageID == 0)
			{
				UIGlobal.MsgBox(this,"����������ڣ�");

			}
			


			//if(lngActorType == e_ActorClass.fmMasterActor)
			if(msgObject.ActorType == e_ActorClass.fmMasterActor)
			{
				if(Session["NormalPortal"] == null)
				{
					Session["NormalPortal"] = true;
				}
				if((bool)Session["NormalPortal"] == true)
				{
					Session["NormalPortal"] = false;
					Response.Redirect("OA_Normal.htm");
					
				}
				else
				{
					Session["NormalPortal"] = true;
					Response.Redirect("OA_Normal11.htm");
					
				}
			}
			else
			{
				//��֪��Э�졡��ǩȫ�����������
				Response.Redirect("OA_Reader.htm");  
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
