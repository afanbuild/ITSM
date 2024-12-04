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
    public delegate void CalendarChangedHandler(DateTime day);

    public partial class ShowWorkIssues : System.Web.UI.UserControl
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

        private GS_SchedulesAreaDP area = new GS_SchedulesAreaDP();

        public event CalendarChangedHandler CalendarChanged;

        protected void Page_Load(object sender, EventArgs e)
        {           
            if (!IsPostBack)
            {
               
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            initPage();
            bindWorkIssues();
            base.OnPreRender(e);
        }

        private void initPage()
        {
            StartTime = DateTime.Parse(Request["StartDate"].ToString());
            EndTime = DateTime.Parse(Request["EndDate"].ToString());
            AreaId = long.Parse(Request["AreaId"].ToString());
        }

        public  void bindWorkIssues()
        {
            area = area.GetReCorded(AreaId);
            List<GS_Engineer_SchedulesDP> engineerIssuesList = new GS_Engineer_SchedulesDP().GetWorkIssues(area.STARTDATE,area.ENDDATE);
            setCalendar(engineerIssuesList);
        }

        private void setCalendar(List<GS_Engineer_SchedulesDP> engineerIssuesList)
        {
            this.DataCalendar1.StartDate = area.STARTDATE;
            this.DataCalendar1.EndDate = area.ENDDATE;
            this.DataCalendar1.IssuesSource = changeToCalendarEntity(engineerIssuesList);
        }

        private List<PersonIssuesEntity> changeToCalendarEntity(List<GS_Engineer_SchedulesDP> engineerIssuesList)
        {
            if (engineerIssuesList == null || engineerIssuesList.Count == 0)
            {
                return null;
            }
            List<PersonIssuesEntity> result =new List<PersonIssuesEntity>();
            Array.ForEach<GS_Engineer_SchedulesDP>(engineerIssuesList.ToArray(), p => {
                result.Add(new PersonIssuesEntity() 
                    {  
                        Id=p.ESID , 
                        PersonId =p.ENGINEERID , 
                        PersonName=p.EngineerName ,
                        WorkDate=p.WORKDATE ,
                        WorkType=p.ScheduleName  
                    }); 
            });

            return result;
        }

        protected void DataCalendar1_SelectionChanged(object sender, EventArgs e)
        {
            if (CalendarChanged != null)
            {
                CalendarChanged(this.DataCalendar1.SelectedDate);
            }
        }
        
    }
}