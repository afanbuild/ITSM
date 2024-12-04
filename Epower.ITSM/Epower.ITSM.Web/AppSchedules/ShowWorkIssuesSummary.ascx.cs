
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
    public partial class ShowWorkIssuesSummary : System.Web.UI.UserControl
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
        public event CalendarChangedHandler CalendarChanged;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                initPage();
                
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            bindWorkIssues();
            base.OnPreRender(e);
        }

        private void initPage()
        {
            StartTime = DateTime.Parse(Request["StartDate"].ToString());
            EndTime = DateTime.Parse(Request["EndDate"].ToString());
            AreaId = long.Parse(Request["AreaId"].ToString());   
        }

        public void bindWorkIssues()
        {            
            List<PersonIssuesEntity> summaryList = getIssuesSummary(EngineerIssuesList, ScheduleBaseList);
            setCalendar(summaryList);
        }

        private List<PersonIssuesEntity> getIssuesSummary(List<GS_Engineer_SchedulesDP> engineerIssuesList, List<GS_Schedules_BaseDP> scheduleBaseList )
        {
            if (scheduleBaseList == null || scheduleBaseList.Count == 0)
            {
                return null;
            }
            List<PersonIssuesEntity> result = new List<PersonIssuesEntity>();
            if (engineerIssuesList == null || engineerIssuesList.Count == 0)
            {
                Array.ForEach<GS_Schedules_BaseDP>(scheduleBaseList.ToArray(), p => {
                    result.Add(new PersonIssuesEntity() { 
                         PersonName =p.FULLNAME + "[0]"+ "(" + p.STARTDAYTIME + p.ENDDAYTIME +")", 
                         WorkTypeId =p.SCHEDULESID                       
                    });
                });
            }
            else
            {
                //DateTime startDate = area.STARTDATE;
                for (DateTime startDate = IssuesArea.STARTDATE; startDate <= IssuesArea.ENDDATE; startDate = startDate.AddDays(1))
                {
                    foreach (GS_Schedules_BaseDP scheduleBase in scheduleBaseList)
                    {
                        int count = engineerIssuesList.FindAll(p => { return p.SCHEDULESID == scheduleBase.SCHEDULESID && p.WORKDATE ==startDate ; }).Count;

                        result.Add(new PersonIssuesEntity()
                        {
                            //PersonName = scheduleBase.FULLNAME + "[" + count.ToString() + "]" + "(" + scheduleBase.STARTDAYTIME + scheduleBase.ENDDAYTIME + ")",
                            PersonName = scheduleBase.FULLNAME + "[" + count.ToString() + "]" ,
                            WorkDate=startDate, 
                            WorkTypeId = scheduleBase.SCHEDULESID 
                        });
                    }
                }
               
            
            }
            return result;
        }
        private void setCalendar(List<PersonIssuesEntity> summaryList)
        {
            this.DataCalendar1.StartDate = IssuesArea.STARTDATE;
            this.DataCalendar1.EndDate = IssuesArea.ENDDATE;
            this.DataCalendar1.IssuesSource = summaryList;
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
            bindWorkIssues();
            if (CalendarChanged != null)
            {
                CalendarChanged(this.DataCalendar1.SelectedDate);
            }
        }
        
    }
}