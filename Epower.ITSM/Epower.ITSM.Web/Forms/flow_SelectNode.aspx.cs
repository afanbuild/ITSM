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
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.IO;
using EpowerCom;
using EpowerGlobal;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// flow_View_Chart ��ժҪ˵����
	/// </summary>
	public partial class flow_SelectNode : BasePage
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��

			long lngMessageID = long.Parse(Request.QueryString["MessageID"]);

            int iType = 0;  //0��ʾ��ת  1 ����

            if (Request.QueryString["iType"] != null)
                iType = int.Parse(Request.QueryString["iType"]);

            if (iType == 1)
                Page.Title = "ѡ�񲵻صĻ���";

			int intStatus=0;
			string strFinishedList ="";
			long lngNMID=0;
			long lngONMID=0;
			int count = 0;

			long lngCurrNMID =0;
            long lngNMMessageID = 0;
            long lngFlowModelID = 0;
            long lngFlowID = 0;

            int lngCurrPathID = -1;   //���ڲ���ʱ·���ж�

			string strMsg="";

			


			
			//��ȡ����ɻ���ģ���б�
            DataSet ds = Message.GetFlowInfo2009(lngMessageID);

			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				lngNMID = long.Parse(ds.Tables[0].Rows[i]["NodeModelID"].ToString());
                lngNMMessageID = long.Parse(ds.Tables[0].Rows[i]["MessageID"].ToString());
                lngFlowModelID = long.Parse(ds.Tables[0].Rows[i]["FlowModelID"].ToString());
                lngFlowID = long.Parse(ds.Tables[0].Rows[i]["FlowID"].ToString());
				if(lngNMID != lngONMID)
				{
					if(count != 0)
					{
						if(intStatus == (int)e_NodeStatus.ensFinished)
						{
							strFinishedList = strFinishedList + lngONMID.ToString() + ",";
							//������̬�ؼ����� MESSAGE
							//Page.RegisterHiddenField("Msg" + lngONMID.ToString(),strMsg);
						}
					}

					intStatus=int.Parse(ds.Tables[0].Rows[i]["Status"].ToString());
					lngONMID = lngNMID;
					count++;
				}
				else
				{
					//strMsg = "�������Ĵ�����Ա��" + ds.Tables[0].Rows[i]["Name"].ToString() +
					//	" ���ҷ��͸���" + ds.Tables[0].Rows[i]["TActors"].ToString();
					intStatus=int.Parse(ds.Tables[0].Rows[i]["Status"].ToString());
				}

                if (lngMessageID == lngNMMessageID)
                {
                    //ȡ��ǰ���ڵ�ģ��ID
                    lngCurrNMID = lngNMID;
                }

				
			}

            


			if(intStatus == (int)e_NodeStatus.ensFinished)
			{
				strFinishedList = strFinishedList + lngONMID.ToString() + ",";
				//Page.RegisterHiddenField("Msg" + lngONMID.ToString(),strMsg);

			}

            //ǰ��һ���ȡ��
			//lngCurrNMID = Message.GetCurrNodeModelID(lngMessageID);

            strFinishedList = strFinishedList.Replace(lngCurrNMID.ToString() + ",", "");

            //��������ת���ڵ��б��������ֶ�
            string strCanJumpNList = "0";
            if (iType == 0)
            {
                strCanJumpNList = Message.GetCanJumpNodeList2009(lngFlowModelID, lngCurrNMID);
            }
            else
            {
                if (lngFlowModelID != 0 && lngCurrNMID != 0)
                {
                    ObjNodeModel onm = new ObjNodeModel(lngFlowModelID, lngCurrNMID);
                    lngCurrPathID = onm.PathID;
                }
                //���ﷵ�ص��� 1|2,2|2,...   [����ģ��ID]|[·��ID]������ת��ͬ
                strCanJumpNList = Message.GetCanTakeBackHasDoneNodeList2009(lngFlowModelID, lngCurrNMID,strFinishedList);
            }

            Page.RegisterHiddenField("hidJumpType", iType.ToString());
            Page.RegisterHiddenField("CanJumpNodeList", strCanJumpNList);
            Page.RegisterHiddenField("hidFlowModelID", lngFlowModelID.ToString());
            Page.RegisterHiddenField("hidFlowID", lngFlowID.ToString());

            Page.RegisterHiddenField("hidPahtID", lngCurrPathID.ToString());  //�����ڲ���

            Page.RegisterHiddenField("MsgTitle1", "ȷ��" + (iType == 0 ? "��ת" : "����") + "����");
            Page.RegisterHiddenField("MsgTitle2", "�����ڣ�");

			XmlDocument xmlDoc = new XmlDocument();
			
			xmlDoc.LoadXml(FlowModel.GetFlowModelChart(lngMessageID));

		
			XPathNavigator nav = xmlDoc.DocumentElement.CreateNavigator();

	
			XslTransform xmlXsl = new XslTransform();
            
            if (iType == 0)
            {
                tb001.Visible = false;
                xmlXsl.Load(Server.MapPath("../xslt/FlowNodeSelectNew.xslt"));
            }
            else
            {
                xmlXsl.Load(Server.MapPath("../xslt/FlowNodeSelectHasDone.xslt"));
            }
			XsltArgumentList xslArg = new XsltArgumentList();

			xslArg.AddParam("FinishedNodeID","",strFinishedList);
			xslArg.AddParam("CurrNodeID","",lngCurrNMID);

			xmlXsl.Transform(nav,xslArg,Response.OutputStream);
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
