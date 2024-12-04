using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Epower.ITSM.Web.Controls
{
    public partial class CtrDateAndTimeFactory : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (System.Web.HttpContext.Current.Request.Browser.Browser == "IE")
                {
                    this.Panel1.Controls.Add(base.LoadControl("CtrDateAndTime.ascx"));
                }
                else
                {
                    this.Panel1.Controls.Add(base.LoadControl("CtrDateAndTimeV2.ascx"));
                }
            }

        }
    }
}