using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;


using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.AppSchedules
{
    public partial class ShowWorkIssuesOfDay : System.Web.UI.UserControl
    {
        public DateTime QueryDate
        {
            set
            {
                ViewState["QueryDate"] = value;
            }
            get
            {
                if (ViewState["QueryDate"] == null)
                {
                    throw new ArgumentNullException("QueryDate is null.");
                }
                return DateTime.Parse(ViewState["QueryDate"].ToString());
            }
        }

        public long IssuesId
        {
            set
            {
                ViewState["IssuesId"] = value;
            }
            get
            {
                if (ViewState["IssuesId"] == null)
                {
                    throw new ArgumentNullException("IssuesId is null.");
                }
                return long.Parse(ViewState["IssuesId"].ToString());
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

        public event EventHandler UpdatePage;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["QueryDate"] == null)
                {
                    IssuesId = -1;
                    QueryDate = GetInitQueryDate();
                }
                else
                {
                    IssuesId = long.Parse(Request["IssuesId"].ToString());
                    QueryDate = DateTime.Parse(Request["QueryDate"].ToString());
                }

                             

                binddayIssues();
            }
        }

        private void InitPage()
        {
            
            this.lblRemark.Text = string.Format("{0}{1}", QueryDate.ToShortDateString(), getIssuesName());
        }

        private DateTime GetInitQueryDate()
        {

            if (System.DateTime.Today >= IssuesArea.STARTDATE && DateTime.Today <= IssuesArea.ENDDATE)
            {
                return DateTime.Today;
            }
            if (DateTime.Today > IssuesArea.ENDDATE)
            {
                return IssuesArea.ENDDATE;
            }
            else
            {
                return IssuesArea.STARTDATE;
            }
        
        }

        private string getIssuesName()
        {
            if (IssuesId == -1)
            {
                return "";
            }
            GS_Schedules_BaseDP issues =ScheduleBaseList.Find(p => { return p.SCHEDULESID == IssuesId; });
            return (issues == null) ? "" : " " + issues.FULLNAME + " ";
        }

        public  void binddayIssues()
        {
            InitPage();
            if (EngineerIssuesList == null || EngineerIssuesList.Count == 0)
            {
                return;
                //throw new ArgumentNullException("EngineerIssuesList is null.");
            }
            //List<GS_Engineer_SchedulesDP> dayIssList = new GS_Engineer_SchedulesDP().GetIssuesOfDay(QueryDate, IssuesId);
            List<GS_Engineer_SchedulesDP> dayIssList = null;
            if (IssuesId == -1)
            {
                dayIssList = EngineerIssuesList.FindAll(p => { return p.WORKDATE == QueryDate; });
            }
            else
            {
                dayIssList = EngineerIssuesList.FindAll(p => { return p.WORKDATE == QueryDate && p.SCHEDULESID == IssuesId; });            
            }
            this.gridIssuesOfDay.DataSource = dayIssList;
            this.gridIssuesOfDay.DataBind();
        }

   

        protected void gridIssuesOfDay_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DropDownList ddlSchedule = e.Item.FindControl("ddlSchedule") as DropDownList;

                Label lblSchedule = e.Item.FindControl("lblSchedule") as Label;
                LinkButton lnkChange = e.Item.FindControl("lnkChange") as LinkButton;
                HiddenField hidScheduleName = e.Item.FindControl("hidScheduleName") as HiddenField;
                
                TimeSpan diff = DateTime.Today - QueryDate;
                if (diff.Days > 0)
                {
                    ddlSchedule.Visible = false;
                    lblSchedule.Visible = true;
                    lnkChange.Enabled = false;
                }
                else
                {
                    ddlSchedule.Visible = true;
                    lblSchedule.Visible = false;
                    lnkChange.Enabled = true;

                    ddlSchedule.DataSource = ScheduleBaseList;
                    ddlSchedule.DataTextField = "FULLNAME";
                    ddlSchedule.DataValueField = "SCHEDULESID";
                    ddlSchedule.DataBind();

                    ddlSchedule.SelectedIndex = ddlSchedule.Items.IndexOf(ddlSchedule.Items.FindByText(hidScheduleName.Value));

                }
            }
           
        }

        protected void gridIssuesOfDay_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            int rowIndex = int.Parse(e.CommandArgument.ToString());
            long esId = long.Parse(((HiddenField)gridIssuesOfDay.Items[rowIndex].FindControl("hidESID")).Value);
            long issuesId = long.Parse(((DropDownList)gridIssuesOfDay.Items[rowIndex].FindControl("ddlSchedule")).SelectedValue);

            GS_Engineer_SchedulesDP issues = new GS_Engineer_SchedulesDP().GetReCorded(esId);

            GS_Schedules_BaseDP issuesBase = new GS_Schedules_BaseDP().GetAllList().Find(p => { return p.SCHEDULESID == issuesId && p.DELETED == 0; });

            issues.SCHEDULESID = issuesId;

            issues.STARTDAYTIME = DateTime.Parse(issues.WORKDATE .ToShortDateString() + " " + issuesBase.STARTDAYTIME);
            issues.ENDSEGMENTTIME = issues.STARTDAYTIME.AddHours((double)issuesBase.ABS_LENGTH1);
            issues.STARTSEGMENTTIME = issues.STARTDAYTIME.AddHours((double)issuesBase.ABS_LENGTH2);
            issues.ENDDAYTIME = issues.STARTDAYTIME.AddHours((double)issuesBase.ABS_LENGTH3);

            issues.LATMDYBY = "";
            issues.LSTMDYTIME = DateTime.Now;

            issues.UpdateRecorded(issues);

            if (UpdatePage != null)
            {
                UpdatePage(this, e);
            }
           
        }
    }
}