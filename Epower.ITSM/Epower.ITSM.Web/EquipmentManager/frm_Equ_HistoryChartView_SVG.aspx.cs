



using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.IO;

using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frm_Equ_HistoryChartView_SVG : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            long lngID = 0;
            if (this.Request.QueryString["id"] != null)
                lngID = long.Parse(Request.QueryString["id"]);

            XmlDocument xmlDoc = new XmlDocument();

            Equ_DeskDP ee = new Equ_DeskDP();
            string sXml = ee.GetEquAllHistoryXml(lngID, 125, 500, 1500, 1000);

            xmlDoc.LoadXml(sXml);


            XPathNavigator nav = xmlDoc.DocumentElement.CreateNavigator();


            XslTransform xmlXsl = new XslTransform();

            xmlXsl.Load(Server.MapPath("../xslt/EquImageHistory_SVG.xslt"));

            XsltArgumentList xslArg = new XsltArgumentList();


            xmlXsl.Transform(nav, xslArg, Response.OutputStream);


        }
    }
}
