using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Xml;
using System.IO;
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
	/// flow_Reader_menu ��ժҪ˵����
	/// </summary>
	public partial class OA_Reader_menu : System.Web.UI.Page
	{

		public long lngMessageID = 0;
		public string strOpinion ="";
        protected string sAttXml = "";
        protected string sActorType = "1";
        public string sApplicationUrl = Epower.ITSM.Base.Constant.ApplicationPath;     //����·��
        public long lngAppID = 0;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			MessageEntity msgObject = (MessageEntity)Session["MessageObject"];
			lngMessageID = msgObject.MessageID;
            lngAppID = FlowModel.GetMessageAppID(lngMessageID);

			string strXml = Message.GetMessageInfo(lngMessageID);
			
			XmlTextReader tr = new XmlTextReader(new StringReader(strXml));

			while(tr.Read())
			{
				if(tr.NodeType == XmlNodeType.Element && tr.Name == "DefaultValue")
				{
					strOpinion = tr.GetAttribute("OpinionValue");
                    sActorType = tr.GetAttribute("ActorType");
				}
                if (tr.NodeType == XmlNodeType.Element && tr.Name == "Attachments")
                {
                    sAttXml = HttpUtility.UrlEncode(tr.ReadOuterXml());
                    break;
                }
				//break;
			}
			tr.Close();
			

			

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