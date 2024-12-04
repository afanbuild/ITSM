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
    public partial class flow_View_ChartModel_SVG : BasePage
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            // 在此处放置用户代码以初始化页面

            long lngFlowModelID = long.Parse(Request.QueryString["FlowModelID"]);
            string strNodeList = "";  //正在处理的环节


            Page.RegisterHiddenField("hidFlowModelID", lngFlowModelID.ToString());

            XmlDocument xmlDoc = new XmlDocument();

            string strFlow = FlowModel.GetFlowModelChartByFlowModel(lngFlowModelID);

            strFlow = strFlow.Replace("<TEXT", "<LINKTEXT");
            strFlow = strFlow.Replace("</TEXT>", "</LINKTEXT>");

            Epower.DevBase.BaseTools.E8Logger.Info(strFlow);

            xmlDoc.LoadXml(strFlow);



            XPathNavigator nav = xmlDoc.DocumentElement.CreateNavigator();


            XslTransform xmlXsl = new XslTransform();

            xmlXsl.Load(Server.MapPath("../xslt/FlowImageNew_SVG.xslt"));

            XsltArgumentList xslArg = new XsltArgumentList();

            strNodeList = "-1,";
            xslArg.AddParam("FinishedNodeID", "", "");
            xslArg.AddParam("CurrNodeID", "", -1);
            xslArg.AddParam("NodeList", "", strNodeList);
            
            xmlXsl.Transform(nav, xslArg, Response.OutputStream);
        }
    }
}
