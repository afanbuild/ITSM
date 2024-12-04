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
using System.Text;
using System.Xml;

using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.Common
{
    public partial class frmGetPubRequestShow : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected override void Render(HtmlTextWriter writer)
        {
            long lngID = long.Parse(Request.QueryString["id"]);

            string sXml = cst_RequestDP.GetRequestShotValues(lngID);
            string sOutput = GetOutputForShot(sXml);

            Response.Clear();
            Response.Write(sOutput);
            Response.Flush();
            Response.End();
        }

        private string GetOutputForShot(string sXml)
        {
            if (sXml == "")
                return "";


            StringBuilder sb = new StringBuilder();

            bool blnHasHeader = false;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(sXml);

            XmlNodeList nodes = xmlDoc.DocumentElement.ChildNodes;
            string sName = "";
            string sValue = "";
            foreach (XmlNode n in nodes)
            {
                sName = n.Attributes["FieldName"].Value;
                sValue = n.Attributes["Value"].Value.Trim();
                if (sValue.Length > 0)
                {
                    if (blnHasHeader == false)
                    {
                        sb.Append("<table width='380px' class='listContent'>");
                        blnHasHeader = true;
                    }
                    sb.Append("<tr>");
                    sb.Append("<td class='listTitleNew_s' style='text-align:left;' width='17%'>");
                    sb.Append(sName + ":");
                    sb.Append("</td>");

                    sb.Append("<td  width='83%' class='listNew_s' style='text-align:left;'>");
                    sb.Append(sValue);
                    sb.Append("</td>");

                    sb.Append("</tr>");


                }
            }

            if (blnHasHeader == true)
            {
                sb.Append("</table>");
            }
            return sb.ToString();

        }

    }
}
