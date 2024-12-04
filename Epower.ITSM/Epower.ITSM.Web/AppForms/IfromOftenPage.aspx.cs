using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Epower.ITSM.Web.NewMainPage
{
    public partial class IfromOftenPage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                Epower.ITSM.SqlDAL.UIMethod ui = new Epower.ITSM.SqlDAL.UIMethod();

                int rows = int.Parse(Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("PageRows", "OftenPageRow").ToString());
                DeskPageId.InnerHtml = ui.getMenuOften((long)Session["UserID"], rows);


            }
        }
    }
}
