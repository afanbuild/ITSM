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
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.DeptForms
{
    public partial class frmViewP : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (!Session["UserName"].ToString().Equals("sa"))
                {
                    Response.Redirect("../Default.aspx", true);
                }
            }
        }

        private void ShowPSW(string LoginName)
        {
            if (LoginName != "")
            {
                UserEntity ue = new UserEntity(LoginName);
                this.txtName.Text = ue.Name;
                this.txtPSW.Text = ue.Password;
            }
            else
            {
                this.txtName.Text = "";
                this.txtPSW.Text = "";
            }
        }


        protected void btnShow_Click(object sender, System.EventArgs e)
        {
            ShowPSW(LoginName.Text.Trim());
        }

    }
}
