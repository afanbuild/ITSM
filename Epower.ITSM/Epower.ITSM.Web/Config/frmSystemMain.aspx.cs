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


namespace Epower.ITSM.Web.Config
{
    public partial class frmSystemMain : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["SelfCustName"] != null)
                    Response.Redirect("~/default.aspx");
            }
        }


        
    }
}
