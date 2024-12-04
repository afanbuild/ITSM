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
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.Common
{
    public partial class frmSetLicense : BasePage
    {
        string sCust = "";
        string sProd = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblServerCode.Text = SysRegEncry.WebLicenseDP.GetServerCode();
            }

        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            string s = "";
            bool blnHasValide = SysRegEncry.WebLicenseDP.ValidateLicense(txtCustCode.Text.Trim(), txtProdCode.Text.Trim(), txtLicense.Text.Trim(), lblServerCode.Text.Trim());
            if (blnHasValide == false)
            {
                PageTool.MsgBox(this, "请输入正确的注册码");
            }
            else
            {
                SysRegEncry.WebLicenseDP.SaveLicenseInfo(txtCustCode.Text.Trim(), txtProdCode.Text.Trim(), txtLicense.Text.Trim());
                PageTool.MsgBox(this, "注册成功！");
                //Response.Redirect("../NewOldMainPage/Index.aspx");
                Response.Write("<Script>window.top.location.href='../NewOldMainPage/Index.aspx';</Script>");
            }
        }
    }
}
