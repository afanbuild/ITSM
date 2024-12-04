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
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// flow_Finished_submit ��ժҪ˵����
	/// </summary>
	public partial class flow_Finished_submit : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��

			long lngUserID;

			long lngMessageID =0;

			bool blnSuccess = true;



			string strMessage = "";

			lngUserID = (long)Session["UserID"];

			lngMessageID = long.Parse(Request.Form["MessageID"]);
		

			//��ֹ�û�ͨ��IE���˰�Ŧ�ظ��ύ
			Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);

			try
			{
				
				Message objMsg= new Message();

				objMsg.TakeBackFlow(lngUserID,lngMessageID);
			
			}
			catch(Exception eSend)
			{
				//strMessage = MyGlobalString.ParseForResponse(eSend.Message.Replace("\n",@"\n"));
				strMessage =eSend.Message;
				blnSuccess = false;
			}

			
			
			
			if(blnSuccess == true)
			{
				//UIGlobal.MsgBox(this,"�����Ѿ����գ�");
				//���պ���뵱ǰ��Ϣ�Ĵ���״̬
				//Response.Write("<script>window.opener.parent.location='flow_Normal.aspx?MessageID=" + lngMessageID.ToString() +"';</script>");
				Response.Redirect("flow_Normal.aspx?MessageID=" + lngMessageID.ToString());
				
			}
			else
			{
				if(strMessage != "")
				{
					//���ַ����쳣�����
					PageTool.MsgBox(this,strMessage.Replace("\r\n",","));
					Response.Write("<script>window.history.back();</script>");
				}
			}
			//Response.Write("<script>window.opener.parent.opener.location.reload();</script>");
			//Response.Write("<script>window.opener.parent.close();</script>");

			

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
