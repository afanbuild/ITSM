using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.AppSchedules
{
    public partial class frm_Schedules_Person_Edit : BasePage
    {
        public DateTime StartTime
        {
            set
            {
                ViewState["StartTime"] = value;
            }
            get
            {
                if (ViewState["StartTime"] == null)
                {
                    throw new ArgumentNullException("StartTime is null.");
                }
                return DateTime.Parse(ViewState["StartTime"].ToString());
            }
        }

        public DateTime EndTime
        {
            set
            {
                ViewState["EndTime"] = value;
            }
            get
            {
                if (ViewState["EndTime"] == null)
                {
                    throw new ArgumentNullException("EndTime is null.");
                }
                return DateTime.Parse(ViewState["EndTime"].ToString());
            }
        }

        public long AreaId
        {
            set
            {
                ViewState["AreaId"] = value;
            }
            get
            {
                if (ViewState["AreaId"] == null)
                {
                    throw new ArgumentNullException("AreaId is null.");
                }
                return long.Parse(ViewState["AreaId"].ToString());
            }
        }


        public GS_SchedulesAreaDP IssuesArea
        { set; get; }

        public List<GS_Schedules_BaseDP> ScheduleBaseList
        { set; get; }

        public List<GS_Engineer_SchedulesDP> EngineerIssuesList
        { set; get; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                initPage();
            }
            setUserCtrl();
        }

        private void initPage()
        {
            StartTime = DateTime.Parse(Request["StartDate"].ToString());
            EndTime = DateTime.Parse(Request["EndDate"].ToString());
            AreaId = long.Parse(Request["AreaId"].ToString());
            this.lblTimeArea.Text = StartTime.ToShortDateString() + " -- " + EndTime.ToShortDateString();

        }

        private void setUserCtrl()
        {
            IssuesArea = new GS_SchedulesAreaDP().GetReCorded(AreaId);
            ScheduleBaseList = new GS_Schedules_BaseDP().GetAllList();
            EngineerIssuesList = new GS_Engineer_SchedulesDP().GetWorkIssues(IssuesArea.STARTDATE, IssuesArea.ENDDATE);

            this.ShowWorkIssuesOfDay1.IssuesArea = IssuesArea;
            this.ShowWorkIssuesOfDay1.ScheduleBaseList = ScheduleBaseList;
            this.ShowWorkIssuesOfDay1.EngineerIssuesList = EngineerIssuesList;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("frm_SchedulesArea_Summary.aspx?StartDate=" + this.StartTime.ToShortDateString() + "&EndDate=" + this.EndTime.ToShortDateString() + "&AreaId=" + this.AreaId.ToString());
        }
    }
}
