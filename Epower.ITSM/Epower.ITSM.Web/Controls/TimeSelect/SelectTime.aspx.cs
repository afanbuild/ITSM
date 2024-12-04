using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace Epower.ITSM.Web.Controls.TimeSelect
{
    public partial class SelectTime : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            hidYear.Value = System.DateTime.Now.Year.ToString();
            HidMonth.Value = System.DateTime.Now.Month.ToString();

            hidYearEnd.Value = System.DateTime.Now.Year.ToString();
            HidMonthEnd.Value = System.DateTime.Now.Month.ToString();
        }
    }
}
