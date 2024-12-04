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
    public partial class frm_frm_SchedulesArea_View : BasePage
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

        private GS_SchedulesAreaDP area;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                initPage();
               
            }
            loadAreaEntity();
            initButton();
        }

        protected override void OnPreRender(EventArgs e)
        {
        
            base.OnPreRender(e);
        }

        private void initPage()
        {
            StartTime = DateTime.Parse(Request["StartDate"].ToString());
            EndTime = DateTime.Parse(Request["EndDate"].ToString());
            AreaId = long.Parse(Request["AreaId"].ToString());
            this.lblTimeArea.Text = StartTime.ToShortDateString() + " -- " + EndTime.ToShortDateString();

        }

        private void initButton()
        {
            btnDelete.Enabled = (area.STATUS < 3) ? true : false;
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
                Response.Redirect("frm_frm_SchedulesArea_Set.aspx?Command=1&AreaId=" + area.AREAID.ToString()
                                    + "&StartDate=" + area.STARTDATE.ToShortDateString()
                                    + "&EndDate=" + area.ENDDATE.ToShortDateString());

            }
            else if (area.STATUS == 0)
            {
                Response.Redirect("frm_frm_SchedulesArea.aspx");
            }
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
        protected void btnGoToDetail_Click(object sender, EventArgs e)
         {
             Response.Redirect("frm_SchedulesArea_Summary.aspx?StartDate=" + this.StartTime.ToShortDateString() + "&EndDate=" + this.EndTime.ToShortDateString() + "&AreaId=" + this.AreaId.ToString() + "&Command=1");
         }
        
    }
}
