/*******************************************************************
 * 版权所有：
 * Description：主框架
 * 
 * 
 * Create By  ：
 * Create Date：2010-04-10
 * *****************************************************************/
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


namespace Epower.ITSM.Web.NewOldMainPage
{
    public partial class Index : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null) { Response.Redirect("~/default.aspx"); }

            if (!IsPostBack)
            {
                if (Session["SelfCustName"] != null)
                    Response.Redirect("~/default.aspx");
            }
        }


        
    }
}
