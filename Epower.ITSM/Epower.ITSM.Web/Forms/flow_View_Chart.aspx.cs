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
	public partial class flow_View_Chart : BasePage
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��

            long lngMessageID = long.Parse(Request.QueryString["MessageID"]);

            int intStatus = 0;
            string strFinishedList = "";
            long lngNMID = 0;
            long lngONMID = 0;
            int count = 0;

            long lngCurrNMID = 0;
            long lngFlowModelID = 0;
            long lngFlowID = 0;
            long lngNMMessageID =0;

            string strMsg = "";

            string strNodeList = "";  //���ڴ���Ļ���

            DataSet ds = Message.GetFlowInfo2009(lngMessageID);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                lngNMID = long.Parse(ds.Tables[0].Rows[i]["NodeModelID"].ToString());
                lngFlowModelID = long.Parse(ds.Tables[0].Rows[i]["FlowModelID"].ToString());
                lngFlowID = long.Parse(ds.Tables[0].Rows[i]["FlowID"].ToString());
                lngNMMessageID = long.Parse(ds.Tables[0].Rows[i]["MessageID"].ToString());
                if (lngNMID != lngONMID)
                {
                    if (count != 0)
                    {
                        if (intStatus == (int)e_NodeStatus.ensFinished)
                        {
                            strFinishedList = strFinishedList + lngONMID.ToString() + ",";
                            //������̬�ؼ����� MESSAGE
                            //Page.RegisterHiddenField("Msg" + lngONMID.ToString(), strMsg);
                        }
                    }
                    //strMsg = "�������Ĵ�����Ա��" + ds.Tables[0].Rows[i]["Name"].ToString() +
                    //            " ����" + GetNodeWorkTypeName(ds.Tables[0].Rows[i]["worktype"].ToString()) + "����" + ds.Tables[0].Rows[i]["TActors"].ToString();
                    intStatus = int.Parse(ds.Tables[0].Rows[i]["Status"].ToString());
                    lngONMID = lngNMID;
                    count++;
                }
                else
                {
                    //strMsg = "�������Ĵ�����Ա��" + ds.Tables[0].Rows[i]["Name"].ToString() +
                    //    " ����" + GetNodeWorkTypeName(ds.Tables[0].Rows[i]["worktype"].ToString()) + "����" + ds.Tables[0].Rows[i]["TActors"].ToString();
                    intStatus = int.Parse(ds.Tables[0].Rows[i]["Status"].ToString());
                }

                if (lngMessageID == lngNMMessageID)
                {
                    //ȡ��ǰ���ڵ�ģ��ID
                    lngCurrNMID = lngNMID;
                }

                if (intStatus == 20)   //��ǰ�������б�
                {
                    strNodeList += lngNMID.ToString() + ",";
                }

            }

            if (intStatus == (int)e_NodeStatus.ensFinished)
            {
                strFinishedList = strFinishedList + lngONMID.ToString() + ",";
                //Page.RegisterHiddenField("Msg" + lngONMID.ToString(), strMsg);

            }

            Page.RegisterHiddenField("hidFlowModelID", lngFlowModelID.ToString());
            Page.RegisterHiddenField("hidFlowID", lngFlowID.ToString());

            //ǰ��һ���ȡ��
            //lngCurrNMID = Message.GetCurrNodeModelID(lngMessageID);

            strFinishedList = strFinishedList.Replace(lngCurrNMID.ToString()+",", "");

            XmlDocument xmlDoc = new XmlDocument();

            //2008-02-10 �޸�ʵ��ʱ�鿴����ͼ Ϊ֧�ֻ���ģʽ�ķ���
            //xmlDoc.LoadXml(FlowModel.GetFlowModelChart(lngMessageID));
            xmlDoc.LoadXml(FlowModel.GetFlowModelChartByFlowModel(lngFlowModelID));
		

		
			XPathNavigator nav = xmlDoc.DocumentElement.CreateNavigator();

	
			XslTransform xmlXsl = new XslTransform();
			
			xmlXsl.Load(Server.MapPath("../xslt/FlowImageNew.xslt"));

			XsltArgumentList xslArg = new XsltArgumentList();

			xslArg.AddParam("FinishedNodeID","",strFinishedList);
			xslArg.AddParam("CurrNodeID","",lngCurrNMID);
            xslArg.AddParam("NodeList","", strNodeList);

			xmlXsl.Transform(nav,xslArg,Response.OutputStream);
		}


		private string GetNodeWorkTypeName(string strWorkType)
		{
			e_NodeWorkType nodeWorkType;
			if(strWorkType == "")
			{
				return "";
			}
			else
			{
				nodeWorkType = (e_NodeWorkType)int.Parse(strWorkType);
				string strRet = "";
				switch(nodeWorkType)
				{
					case e_NodeWorkType.enwtSkipTo:
						strRet = "ת";
						break;
					case e_NodeWorkType.enwtTakeOver:
						strRet = "����";
						break;
					case e_NodeWorkType.enwtSendBack:
						strRet = "�˻�";
						break;
					case e_NodeWorkType.enwtEnd:
						strRet = "����";
						break;
					default:
						strRet = "����";
						break;
				}
				return strRet;
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
