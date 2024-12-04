using System;
using System.Text;
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

namespace Epower.ITSM.Web.Config
{
	/// <summary>
	/// Menu 的摘要说明。
	/// </summary>
    public partial class frmLeft : BasePage
	{
        private string sConfigName = "SystemConfig";

        /// <summary>
        /// 
        /// </summary>
        protected string iCount
        {
            get
            {
                if (ViewState["iCount"] != null)
                    return ViewState["iCount"].ToString();
                else
                    return "0";
            }
            set
            {
                ViewState["iCount"] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
            if (!Page.IsPostBack)
            {
                string ConfigFile = HttpRuntime.AppDomainAppPath + "Config\\" + sConfigName + ".xml";

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode noderoot = xmlDoc.ChildNodes[1];
                XmlNodeList nodes = noderoot.SelectNodes("Items");

                iCount = nodes.Count.ToString();
                InitPage(nodes);
            }
		}

        private void InitPage(XmlNodeList nodes)
        {
            string strTableTitle = @"<table width='180' border='0' cellspacing='0' cellpadding='0'>
                <tr>
                  <td height='3' align='center'></td>
                </tr>
                <tr>
                  <td background='images/lanmu-1.gif' align='center' width='180' height='28'><span class='tf'></span></td>
                </tr>";
            string strTableFoot = "</table>";
            string strTR = string.Empty;
            int i = 0;
            string sFirstName = string.Empty;
            foreach (XmlNode node in nodes)
            {
                string Name = node.Attributes["Name"].Value;
                string Title = node.Attributes["Title"].Value;
                string sImage = "images/lanmu-closed.gif";
                if (i == 0)
                {
                    sImage = "images/lanmu-open.gif";
                    sFirstName = Name;
                }
                strTR += "<tr><td height='1' align='center'></td></tr><tr>"
                      + "<td width='180' id='Td" + i.ToString() + "' onClick=\"ShowMain('frmSetSystemParams.aspx?NodesName=" + Name + "');chang_Class('Td'," + i.ToString() + ")\" style='cursor:hand' height='26'"
                     +" align='center' background='" + sImage + "' class='STYLE5'>" + Title+  "</td></tr>";
                i++;
            }
            LitData.Text = strTableTitle + strTR + strTableFoot;
            Response.Write("<SCRIPT>parent.frmSystemMain.location='frmSetSystemParams.aspx?NodesName=" + sFirstName + "';</SCRIPT>");
        }
	}
}
