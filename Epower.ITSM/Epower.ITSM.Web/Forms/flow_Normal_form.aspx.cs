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
using Epower.ITSM.Base;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// flow_Normal_form ��ժҪ˵����
	/// </summary>
	public partial class flow_Normal_form : System.Web.UI.Page
	{
        //2009��1�� �� ReadOnly FlowProcessType�Ȳ�����Ϊ��ĸ��ҳ�ϼ���

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��  ****  ������ϢID  δ�� ************


			MessageEntity msgObject = (MessageEntity)Session["MessageObject"];
			long lngMessageID = msgObject.MessageID;
			//long lngUserID = long.Parse(Session["UserID"].ToString());

			string strPage = epApp.GetStartWebFormByMessageP(lngMessageID);

            //eOA_FlowProcessType lngFPT = eOA_FlowProcessType.efptReadFinished;  //ȱʡֵ

            if (Session["g_FlowModelAutoPass"] != null)
            {
                strPage = strPage + "?FlowModelID=0&MessageID=" + lngMessageID.ToString() + "&autopass=" + Session["g_FlowModelAutoPass"].ToString();
                //Session ����һ��
                Session.Remove("g_FlowModelAutoPass");
            }
            else
            {
                strPage = strPage + "?FlowModelID=0&MessageID=" + lngMessageID.ToString();
            }
			Response.Redirect(strPage);
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
