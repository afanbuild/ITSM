using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.AppSchedules
{
    public partial class frm_SchedulesArea_Summary : BasePage
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

        private GS_SchedulesAreaDP area;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.ShowWorkIssuesOfDay1.UpdatePage += new EventHandler(UpdateUserCtrl);
            this.ShowWorkIssuesSummary1.CalendarChanged += new CalendarChangedHandler(updateWorkIssues);
            if (!IsPostBack)
            {
                 initPage();
            }
            setUserCtrl();
            loadAreaEntity();
            initButton();
        }

        private void updateWorkIssues(DateTime queryDay)
        {
            ShowWorkIssuesOfDay1.QueryDate = queryDay;
            ShowWorkIssuesOfDay1.IssuesId = -1;
            ShowWorkIssuesOfDay1.binddayIssues();
        }

        protected void UpdateUserCtrl(object sender, EventArgs e)
        {
            setUserCtrl();
 
            ShowWorkIssuesSummary1.bindWorkIssues();
            ShowWorkIssuesOfDay1.binddayIssues();
        }

        private void initPage()
        {
            StartTime = DateTime.Parse(Request["StartDate"].ToString());
            EndTime = DateTime.Parse(Request["EndDate"].ToString());
            AreaId = long.Parse(Request["AreaId"].ToString());
            this.lblTimeArea.Text = StartTime.ToShortDateString() + " -- " + EndTime.ToShortDateString();           
        }

        private void loadAreaEntity()
        {
            area = new GS_SchedulesAreaDP().GetReCorded(AreaId);
            if (area == null)
            {
                throw new Exception("非法进入该页面。");
            }
            if (area.STATUS == 1)
            {
                Response.Redirect("frm_frm_SchedulesArea_Set.aspx?Command=0&AreaId=" + area.AREAID.ToString() 
                                    + "&StartDate=" + area.STARTDATE.ToShortDateString() 
                                    + "&EndDate=" + area.ENDDATE.ToShortDateString());
                    
            }
            else if (area.STATUS == 0)
            {
                Response.Redirect("frm_frm_SchedulesArea.aspx");
            }
        }

        private void initButton()
        {
            btnDelete.Enabled = (area.STATUS < 3) ? true : false;        
        }

        private void setUserCtrl()
        {
            IssuesArea = new GS_SchedulesAreaDP().GetReCorded(AreaId);
            ScheduleBaseList = new GS_Schedules_BaseDP().GetAllList();
            EngineerIssuesList = new GS_Engineer_SchedulesDP().GetWorkIssues(IssuesArea.STARTDATE, IssuesArea.ENDDATE);
            List<GS_Schedules_BaseDP> calaDispIssuesBaseList = getCurIssuesWorkType(ScheduleBaseList, EngineerIssuesList);

            this.ShowWorkIssuesSummary1.IssuesArea = IssuesArea;
            this.ShowWorkIssuesSummary1.ScheduleBaseList = calaDispIssuesBaseList;
            this.ShowWorkIssuesSummary1.EngineerIssuesList = EngineerIssuesList;

            this.ShowWorkIssuesOfDay1.IssuesArea = IssuesArea;
            this.ShowWorkIssuesOfDay1.ScheduleBaseList = ScheduleBaseList;
            this.ShowWorkIssuesOfDay1.EngineerIssuesList = EngineerIssuesList;        
        }

        private List<GS_Schedules_BaseDP>  getCurIssuesWorkType(List<GS_Schedules_BaseDP> scheduleBaseList, List<GS_Engineer_SchedulesDP> engineerIssuesList)
        {
            if (scheduleBaseList == null || scheduleBaseList.Count == 0 || engineerIssuesList == null || engineerIssuesList.Count == 0)
            {
                return null;
            }
            List<GS_Schedules_BaseDP> calaDispIssuesBaseList = new List<GS_Schedules_BaseDP>();
            Array.ForEach<GS_Schedules_BaseDP>(scheduleBaseList.ToArray(),p=>{
                GS_Engineer_SchedulesDP issues = engineerIssuesList.Find(t => t.SCHEDULESID == p.SCHEDULESID);
                if (issues != null)
                {
                    calaDispIssuesBaseList.Add(p);
                }
            });
            return calaDispIssuesBaseList;            
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (area.STATUS == 3)
            {
                PageTool.MsgBox(this, "该期排班数据已经生效，系统禁止重新排班。");
                return;
            }
            GS_SchedulesAreaDP preAreaDp = new GS_SchedulesAreaDP();
            bool flag = preAreaDp.JudegePreAreaIssues(AreaId);
            if (!flag)
            {
                PageTool.MsgBox(this, "下一个排班周期已经生成，系统不允许删除该期数据。");
                return;
            }
            
            
            new GS_CurSchedulesRuleDP().DeleteRecordedByArea(this.AreaId);

            new GS_CurSchedulesRuleDP().CreateWorkIssues(AreaId);

            Response.Redirect("frm_frm_SchedulesArea_Set.aspx?StartDate=" + this.StartTime.ToShortDateString() + "&EndDate=" + this.EndTime.ToShortDateString() + "&AreaId=" + this.AreaId.ToString() + "&Command=1");

        }

        protected void btnGoToSummary_Click(object sender, EventArgs e)
        {
            string commandName = ((Button)(sender)).CommandName;
            switch (commandName)
            { 
                case "ViewSchedure":
                    Response.Redirect("frm_frm_SchedulesArea_Set.aspx?StartDate=" + this.StartTime.ToShortDateString() + "&EndDate=" + this.EndTime.ToShortDateString() + "&AreaId=" + this.AreaId.ToString() + "&Command=1");
                    //Response.Redirect("frm_frm_SchedulesArea_Set.aspx?Command=1&AreaId=" + areaDp.AREAID.ToString() + "&StartDate=" + areaDp.STARTDATE.ToShortDateString() + "&EndDate=" + areaDp.ENDDATE.ToShortDateString());
               
                    break;

                case "ViewSummary":
                    Response.Redirect("frm_frm_SchedulesArea_View.aspx?StartDate=" + this.StartTime.ToShortDateString() + "&EndDate=" + this.EndTime.ToShortDateString() + "&AreaId=" + this.AreaId.ToString() + "&Command=0");
        
                    break;
            }
        }
        
    }
}
