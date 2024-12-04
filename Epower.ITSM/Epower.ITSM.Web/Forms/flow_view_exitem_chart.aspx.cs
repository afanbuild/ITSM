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
using Epower.ITSM.SqlDAL.Customer;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// flow_View_Chart ��ժҪ˵����
	/// </summary>
    public partial class flow_view_exitem_chart : BasePage
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
            LoadChart();
		}

        /// <summary>
        /// ��������ͼģ��
        /// </summary>
        private void LoadChart()
        {
            long lngAppID = long.Parse(Request.QueryString["appid"]);    // Ӧ�ñ��
            long lngFlowModelID = long.Parse(Request.QueryString["FlowModelID"]);    // ����ģ�ͱ��               

            lngFlowModelID = FlowModel.GetLastVersionFlowModelID(lngFlowModelID);

            Page.RegisterHiddenField("hidAppID", lngAppID.ToString());
            Page.RegisterHiddenField("hidFlowModelID", lngFlowModelID.ToString());

            String strFlowModelName = Br_ExtensionDisplayWayDP.GetFlowModelName(lngFlowModelID);
            this.Page.Title = String.Format("����ģ��:��{0}�� - ��չ��ɼ��ɱ༭����", strFlowModelName);


            XmlDocument xmlDoc = new XmlDocument();
            String strFlowModelXML = FlowModel.GetFlowModelChartByFlowModel(lngFlowModelID);

            strFlowModelXML = strFlowModelXML.Replace("<TEXT", "<LINKTEXT");
            strFlowModelXML = strFlowModelXML.Replace("</TEXT>", "</LINKTEXT>");


            xmlDoc.LoadXml(strFlowModelXML);


            XPathNavigator nav = xmlDoc.DocumentElement.CreateNavigator();

            XslTransform xmlXsl = new XslTransform();

            xmlXsl.Load(Server.MapPath("../xslt/view_chart_exitem.xslt"));

            XsltArgumentList xslArg = new XsltArgumentList();

            xslArg.AddParam("FinishedNodeID", "", "");
            xslArg.AddParam("CurrNodeID", "", "");
            xslArg.AddParam("NodeList", "", "");

            xmlXsl.Transform(nav, xslArg, Response.OutputStream);
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
