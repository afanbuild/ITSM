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
using EpowerCom;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.AppForms
{
    public partial class frmCallCenter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hidBlockRoom.Value = Request["BlockRoom"].ToString(); //座室
                hidFlowModelId.Value = CommonDP.GetConfigValue("Other", "CallAppID"); //跳转的应用ID
                txtHost.Value = CommonDP.GetConfigValue("Other", "CallServerIP"); //跳转的应用ID
            }
        }
    }
}
