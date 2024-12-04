using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.AppSchedules
{
    public partial class fem_SchedulesArea_Base : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindAreaBase();
            }
        }

        private void bindAreaBase()
        {
            this.dgSchedulesAreaBase.DataSource = GS_Schedules_BaseDP.GetFullSchedulesAreaBase();
            this.dgSchedulesAreaBase.DataBind();
        }

        protected void dgSchedulesAreaBase_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            long schedulesID = long.Parse(e.CommandArgument.ToString());
            switch (e.CommandName.ToString())
            {
                case "View":
                    Response.Redirect("frm_SchedulesArea_BaseSet.aspx?Command=1&SchedulesID=" + schedulesID.ToString());
                    break;
                case "Delete":
                    GS_Schedules_BaseDP gsScheduleBase = new GS_Schedules_BaseDP();
                    gsScheduleBase.DeleteRecorded(schedulesID);
                    bindAreaBase();
                    break;
                default:
                    throw new Exception("");
            }

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("frm_SchedulesArea_BaseSet.aspx?Command=0&SchedulesID=0");
        }
    }
}
