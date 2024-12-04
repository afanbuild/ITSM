using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Epower.ITSM.SqlDAL;
using MyCalendar.UI;

namespace Epower.ITSM.Web.AppSchedules
{
    public partial class ShowPersonIssues : System.Web.UI.UserControl
    {
        private DateTime mStartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

        private DateTime mEndDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1);

        public long EngineerId
        {
            set
            {
                ViewState["EngineerId"] = value;
            }
            get
            {
                if (ViewState["EngineerId"] == null)
                {
                    throw new ArgumentNullException("EngineerId is null.");
                }
                return long.Parse(ViewState["EngineerId"].ToString());
            }
        }

        private List<GS_Engineer_SchedulesDP> engineerIssuesList = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                initPage();
                
            }
        }

        private void initPage()
        {
         
        }

        protected override void OnPreRender(EventArgs e)
        {
            bindWorkIssues();
            base.OnPreRender(e);
        }

        public void bindWorkIssues()
        {
            engineerIssuesList = new GS_Engineer_SchedulesDP().GetWorkIssues(mStartDate, mEndDate).FindAll(p => { return p.ENGINEERID == EngineerId; });

            setCalendar(engineerIssuesList);
        }

        private void setCalendar(List<GS_Engineer_SchedulesDP> engineerIssuesList)
        {
            this.DataCalendar1.StartDate = mStartDate;
            this.DataCalendar1.EndDate = mEndDate;
            this.DataCalendar1.IssuesSource = changeToCalendarEntity(engineerIssuesList);
        }

        private List<PersonIssuesEntity> changeToCalendarEntity(List<GS_Engineer_SchedulesDP> engineerIssuesList)
        {
            if (engineerIssuesList == null || engineerIssuesList.Count == 0)
            {
                return null;
            }
            List<PersonIssuesEntity> result = new List<PersonIssuesEntity>();
            Array.ForEach<GS_Engineer_SchedulesDP>(engineerIssuesList.ToArray(), p =>
            {
                result.Add(new PersonIssuesEntity()
                {
                    Id = p.ESID,
                    PersonId = p.ENGINEERID,
                    PersonName = p.EngineerName,
                    WorkDate = p.WORKDATE,
                    WorkType = p.ScheduleName
                });
            });

            return result;
        }

        protected void DataCalendar1_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DateTime day in DataCalendar1.SelectedDates)
            {
                Response.Write(day.ToShortDateString());
            }
        }

        protected void DataCalendar1_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            //Response.Write(e.NewDate.ToShortDateString());
            mStartDate = e.NewDate;
            mEndDate = mStartDate.AddMonths(1).AddDays(-1);
        }

        
    }
}