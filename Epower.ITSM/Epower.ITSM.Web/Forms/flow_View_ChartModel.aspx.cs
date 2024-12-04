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
	/// flow_View_Chart 的摘要说明。
	/// </summary>
	public partial class flow_View_ChartModel :BasePage
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面

			long lngFlowModelID = long.Parse(Request.QueryString["FlowModelID"]);
            string strNodeList = "";  //正在处理的环节


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


		

		#region Web 窗体设计器生成的代码
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion
	}
}
