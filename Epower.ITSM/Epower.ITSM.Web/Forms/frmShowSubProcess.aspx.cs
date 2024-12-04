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

namespace Epower.ITSM.Web.Forms
{
    public partial class frmShowSubProcess : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            long id = long.Parse(Request.QueryString["id"]);
            if (Page.IsPostBack == false)
            {
                string sList = EpowerCom.Flow.GetLinkedFlowListByMessageID(id);
                if (sList.Length > 0)
                    CtrDySubProcess1.SubFlowList = sList;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            //���HTML
            System.IO.StringWriter html = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter tw = new System.Web.UI.HtmlTextWriter(html);
            CtrDySubProcess1.RenderControl(tw);
            Response.Clear();
            //�����Ƿ������� 0 ��, 1 ��
            string sTmp = html.ToString();
            Response.Write(sTmp);
            Response.Flush();
            Response.End();
        }
    }
}
