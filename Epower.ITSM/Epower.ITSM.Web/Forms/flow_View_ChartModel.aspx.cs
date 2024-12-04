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
	public partial class flow_View_ChartModel :BasePage
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��

			long lngFlowModelID = long.Parse(Request.QueryString["FlowModelID"]);
            string strNodeList = "";  //���ڴ���Ļ���


            Page.RegisterHiddenField("hidFlowModelID", lngFlowModelID.ToString());

			XmlDocument xmlDoc = new XmlDocument();
			
			xmlDoc.LoadXml(FlowModel.GetFlowModelChartByFlowModel(lngFlowModelID));

            
		
			XPathNavigator nav = xmlDoc.DocumentElement.CreateNavigator();

	
			XslTransform xmlXsl = new XslTransform();
			
			xmlXsl.Load(Server.MapPath("../xslt/FlowImageNew.xslt"));

			XsltArgumentList xslArg = new XsltArgumentList();

            strNodeList = "-1,";
			xslArg.AddParam("FinishedNodeID","","");
			xslArg.AddParam("CurrNodeID","",-1);
            xslArg.AddParam("NodeList", "", strNodeList);

			xmlXsl.Transform(nav,xslArg,Response.OutputStream);
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
