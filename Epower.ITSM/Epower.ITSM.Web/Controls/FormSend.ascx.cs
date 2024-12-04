/*******************************************************************
 * 版权所有：
 * Description：工作流处理控件
 * 
 * 
 * Create By  ：
 * Create Date：
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
	///		FormSend 的摘要说明。
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

		int intSpecRightType = (int)e_SpecRightType.esrtNormal;  //特殊处理权限的初值是一般处理


		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			long lngID=0;
			e_SendType lngSendType;
			if (_SendType == "New")
			{
				//新增流程时
				lngMessageID = 0;
                lngFlowID = 0;
				lngFlowModelID = (long)Session["FlowModelID"];
				
				lngActionID = 0;
				lngID = lngFlowModelID;
				lngSendType = e_SendType.eAddNew;
			}
			else
			{
				//处理流程时，控件必须在页面上设置一个 SendType 的属性
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
			// CODEGEN：该调用是 ASP.NET Web 窗体设计器所必需的。
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		///		设计器支持所需的方法 - 不要使用
		///		代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
		}
		#endregion

		protected override void Render(HtmlTextWriter writer)
		{
		//添加隐含字段信息
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
			UIGlobal.AddHiddenField(writer,"SpecRightType",intSpecRightType.ToString());   //保存特殊处理的隐含字段
			UIGlobal.AddHiddenField(writer,"JumpToNodeModel","0");   //保存跳转到的环节模型ID的隐含字段
            UIGlobal.AddHiddenField(writer, "ReDoUserID", "0");   //保存重审时处理人ID的隐含字段，提前计算好了，提高性能
            UIGlobal.AddHiddenField(writer, "ReDoNMType", "0");   //保存重审时处理人ID的隐含字段，提前计算好了，提高性能

            UIGlobal.AddHiddenField(writer, "hidValidateList");   //2007-07-21加便于统一公共验证逻辑
            UIGlobal.AddHiddenField(writer, "NodeID", lngCurrNodeID);
		}
	}
}
