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
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web
{
    public partial class login : System.Web.UI.Page
    {
        /// <summary>
        /// ¶¨Î»URL
        /// </summary>
        protected string sUrl
        {
            get 
            {
                if (CommonDP.GetConfigValue("Other", "E8Online") != null && CommonDP.GetConfigValue("Other", "E8Online") == "1")
                {
                    return CommonDP.GetConfigValue("Other", "E8OnlineLogin");
                }
                else
                {
                    return "Default.aspx";
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Themes"] == null)
            {
                Response.Write("<Script>window.top.location.href='" + sUrl + "';</Script>");
            }
        }
    }
}
