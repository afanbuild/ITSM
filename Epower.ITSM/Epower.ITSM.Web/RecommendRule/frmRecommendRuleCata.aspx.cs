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
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;
using System.Text;


namespace Epower.ITSM.Web.RecommendRule
{
    public partial class frmRecommendRuleCata : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int id = int.Parse(Request.QueryString["id"]);

                switch ((e_ITSMRUItem)id)
                {
                    case e_ITSMRUItem.eitsmRUServiceTypeID: // 事件类别
                        NormalCata.Visible = true;
                        NormalCata.RootID = 1001;
                        break;
                    case e_ITSMRUItem.eitsmRUServiceLevelID: // 服务级别
                        ddlServiceLevel.Visible = true;
                        ddlBind();
                        break;
                    default:
                        break;
                }
            }

        }
        public string ObjID
        {
            get { return String.IsNullOrEmpty(Request["objID"]) ? "" : Request["objID"].ToString(); }
        }

        protected void ddlBind()
        {
            Cst_ServiceLevelDP ee = new Cst_ServiceLevelDP();
            DataTable dt = ee.GetDataTable("","");
            ddlServiceLevel.DataSource = dt;
            ddlServiceLevel.DataTextField = "LevelName";
            ddlServiceLevel.DataValueField = "ID";
            ddlServiceLevel.DataBind();
        }

        protected void cmdOK_Click(object sender, EventArgs e)
        {
            string strValues = "";
            if (ddlServiceLevel.Visible == true)
            {
                strValues = ddlServiceLevel.SelectedItem.Value + "@" + ddlServiceLevel.SelectedItem.Text.Trim();
            }
            else
            {
                strValues = NormalCata.CatelogID.ToString() + "@" + NormalCata.CatelogValue.Trim();
            }
            StringBuilder sbText = new StringBuilder();
            string[] arr= strValues.Split('@');

            sbText.Append("<script>");
            sbText.Append("window.opener.document.getElementById('" + ObjID.Replace("cmdPop", "HidTag") + "').value='" + arr[0] + "';");
            sbText.Append("window.opener.document.getElementById('" + ObjID.Replace("cmdPop", "txtValue") + "').value='" + arr[1] + "';");
            sbText.Append("window.opener.document.getElementById('" + ObjID.Replace("cmdPop", "hidValue") + "').value='" + arr[1] + "';");

            sbText.Append("top.close();");
            sbText.Append("</script>");

           
            Page.RegisterStartupScript(DateTime.Now.Ticks.ToString(), sbText.ToString());
           
        }
    }
}
