/*******************************************************************
 * ��Ȩ���У�
 * Description������������ؼ�
 * 
 * 
 * Create By  ��
 * Create Date��
 * *****************************************************************/
namespace Epower.ITSM.Web.Controls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Xml;
	using System.IO;
	using EpowerCom;
	using EpowerGlobal;

	/// <summary>
	///		FormSend ��ժҪ˵����
	/// </summary>
	public partial  class FormSend : System.Web.UI.UserControl
	{

		string _SendType = "New";
		public string SendType
		{
			get{return _SendType;}
			set{_SendType = value;}
		}


		private long lngMessageID=0;
        private long lngFlowID = 0;
		private long lngFlowModelID = 0;
		private long lngActionID=-1;
		private long lngAppID=-1;
		private long lngOpID=-1;
		private string strOpinionValue="";
        private long lngCurrNodeID = 2;

		private string strActionsXml="";
		//private string strOpinionsXml="";
		private string strFormXMLValue="";
		private string strAttachXml="";

		int intSpecRightType = (int)e_SpecRightType.esrtNormal;  //���⴦��Ȩ�޵ĳ�ֵ��һ�㴦��


		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			long lngID=0;
			e_SendType lngSendType;
			if (_SendType == "New")
			{
				//��������ʱ
				lngMessageID = 0;
                lngFlowID = 0;
				lngFlowModelID = (long)Session["FlowModelID"];
				
				lngActionID = 0;
				lngID = lngFlowModelID;
				lngSendType = e_SendType.eAddNew;
			}
			else
			{
				//��������ʱ���ؼ�������ҳ��������һ�� SendType ������
				lngFlowModelID =0;
				MessageEntity msgObject = (MessageEntity)Session["MessageObject"];
				lngMessageID = msgObject.MessageID;
                lngFlowID = msgObject.FlowID;
				lngActionID=0;
				lngID = lngMessageID;
				lngSendType = e_SendType.eDeal;
                lngCurrNodeID = msgObject.NodeID;
			}

			Message objMsg = new Message();
			InitValues(objMsg.GetDefaultValues(lngID,lngSendType));

		}


		
		private void InitValues(string strXML)
		{
			XmlElement xmlEle;
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(strXML);


			xmlEle = (XmlElement)xmlDoc.DocumentElement.SelectSingleNode("BaseValue");
			if (xmlEle != null)
			{
				lngActionID = long.Parse(xmlEle.GetAttribute("DefaultActionID"));
				lngAppID = long.Parse(xmlEle.GetAttribute("AppID"));
				lngOpID = long.Parse(xmlEle.GetAttribute("OpID"));
				strOpinionValue = xmlEle.GetAttribute("Opinion");
			}

			xmlEle = (XmlElement)xmlDoc.DocumentElement.SelectSingleNode("Actions");
			if (xmlEle != null)
			{
				strActionsXml = xmlEle.OuterXml;
				//strActionsXml = strXML;
			
			}

			xmlEle = (XmlElement)xmlDoc.DocumentElement.SelectSingleNode("Attachments");
			if (xmlEle != null)
			{
				strAttachXml = xmlEle.OuterXml;
			
			}


			xmlEle = (XmlElement)xmlDoc.DocumentElement.SelectSingleNode("FieldValues");
			if (xmlEle != null)
			{
				strFormXMLValue = xmlEle.OuterXml;
			
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
		
		///		�����֧������ķ��� - ��Ҫʹ��
		///		����༭���޸Ĵ˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{
		}
		#endregion

		protected override void Render(HtmlTextWriter writer)
		{
		//��������ֶ���Ϣ
			UIGlobal.AddHiddenField(writer,"Subject");
            UIGlobal.AddHiddenField(writer, "AppID", lngAppID);
			UIGlobal.AddHiddenField(writer,"MessageID",lngMessageID);
            UIGlobal.AddHiddenField(writer, "FlowID", lngFlowID);
			UIGlobal.AddHiddenField(writer,"FlowModelID",lngFlowModelID);
			UIGlobal.AddHiddenField(writer,"ActionID",lngActionID);
			UIGlobal.AddHiddenField(writer,"FormXMLValue",HttpUtility.UrlEncode(strFormXMLValue));
            UIGlobal.AddHiddenField(writer, "FormDefineValue", "");
			UIGlobal.AddHiddenField(writer,"ActionsXML",HttpUtility.UrlEncode(strActionsXml));
			//UIGlobal.AddHiddenField(writer,"ActionsXML",strActionsXml);
			//UIGlobal.AddHiddenField(writer,"OpinionsXML",HttpUtility.UrlEncode(strOpinionsXml));
			UIGlobal.AddHiddenField(writer,"OpinionValue",HttpUtility.UrlEncode(strOpinionValue));
			UIGlobal.AddHiddenField(writer,"Attachment",HttpUtility.UrlEncode(strAttachXml));
			
			UIGlobal.AddHiddenField(writer,"Importance","1");
			UIGlobal.AddHiddenField(writer,"Receivers");
			UIGlobal.AddHiddenField(writer,"SpecRightType",intSpecRightType.ToString());   //�������⴦��������ֶ�
			UIGlobal.AddHiddenField(writer,"JumpToNodeModel","0");   //������ת���Ļ���ģ��ID�������ֶ�
            UIGlobal.AddHiddenField(writer, "ReDoUserID", "0");   //��������ʱ������ID�������ֶΣ���ǰ������ˣ��������
            UIGlobal.AddHiddenField(writer, "ReDoNMType", "0");   //��������ʱ������ID�������ֶΣ���ǰ������ˣ��������

            UIGlobal.AddHiddenField(writer, "hidValidateList");   //2007-07-21�ӱ���ͳһ������֤�߼�
            UIGlobal.AddHiddenField(writer, "NodeID", lngCurrNodeID);
		}
	}
}
