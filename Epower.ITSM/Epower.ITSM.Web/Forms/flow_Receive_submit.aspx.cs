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
	/// flow_Receive_submit ��ժҪ˵����
	/// </summary>
	public partial class flow_Receive_submit : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			long lngUserID;

			long lngMessageID =0;
			

			bool blnSuccess = true;



			string strMessage = "";

			lngUserID = (long)Session["UserID"];

			lngMessageID = long.Parse(Request.Form["MessageID"]);

            long lngAppID = long.Parse(Request.Form["AppID"]);
			

			//��ֹ�û�ͨ��IE���˰�Ŧ�ظ��ύ
			Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);

			try
			{
                blnSuccess = ReceiveList.ReceiveMessage(lngMessageID, lngUserID, lngAppID);
			}
			catch(Exception eSend)
			{
				strMessage =eSend.Message;
				blnSuccess = false;
			}

			
			
			if(blnSuccess == true)
			{
				//���պ���봦��ҳ�桡
				Response.Redirect("flow_Normal.aspx?MessageID="  + lngMessageID.ToString());
			
			}
			else
			{
				if(strMessage != "")
				{
					PageTool.MsgBox(this,strMessage);
				}
				else
				{
					PageTool.MsgBox(this,"�������Ѿ��������û�����");
					Response.Write("<script>window.history.back();</script>");
					//Response.Redirect("frmContent.aspx");
					return;
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
