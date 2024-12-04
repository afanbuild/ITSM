using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using Epower.ITSM.SqlDAL;
using Epower.ITSM.SqlDAL.Schedules;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.AppSchedules
{
    public partial class frm_frm_SchedulesArea_Set : BasePage
    {
        private GS_CurSchedulesRuleDP curSchedulesRuleDp = new GS_CurSchedulesRuleDP();

        /// <summary>
        /// 周期开始时间
        /// </summary>
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

        /// <summary>
        /// 周期结束时间
        /// </summary>
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

        /// <summary>
        /// 周期ID
        /// </summary>
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

        /// <summary>
        /// 操作类型（0添加/1修改）
        /// </summary>
        public int Command
        {
            set
            {
                ViewState["Command"] = value;
            }
            get
            {
                if (ViewState["Command"] == null)
                {
                    throw new ArgumentNullException("Command is null.");
                }
                return int.Parse(ViewState["Command"].ToString());
            }
        }

        /// <summary>
        /// 周期状态（0初始/1未生成排班/已生成排班）
        /// </summary>
        public long Status
        {
            set
            {
                ViewState["Status"] = value;
            }
            get
            {
                if (ViewState["Status"] == null)
                {
                    throw new ArgumentNullException("Status is null.");
                }
                return long.Parse(ViewState["Status"].ToString());
            }
        }

        /// <summary>
        /// 所有轮班班次详情
        /// </summary>
        public DataTable TurnRule
        {
            set
            {
                ViewState["TurnRule"] = value;
            }
            get
            {
                if (ViewState["TurnRule"] == null)
                {
                    throw new ArgumentNullException("DicTurnRuleDetl is null.");
                }
                return ViewState["TurnRule"] as DataTable;
            }
        }

        /// <summary>
        /// 所有轮班班次详情
        /// </summary>
        public Dictionary<long, DataTable> DicTurnRuleDetl
        {
            set
            {
                ViewState["DicTurnRuleDetl"] = value;
            }
            get
            {
                if (ViewState["DicTurnRuleDetl"] == null)
                {
                    throw new ArgumentNullException("DicTurnRuleDetl is null.");
                }
                return ViewState["DicTurnRuleDetl"] as Dictionary<long, DataTable>;
            }
        }

        /// <summary>
        /// 所有班次
        /// </summary>
        public DataTable DtAreaBase
        {
            set
            {
                ViewState["DtAreaBase"] = value;
            }
            get
            {
                if (ViewState["DtAreaBase"] == null)
                {
                    throw new ArgumentNullException("DtAreaBase is null.");
                }
                return ViewState["DtAreaBase"] as DataTable;
            }
        }

        private GS_SchedulesAreaDP schedulesArea
        {
            set
            {
                ViewState["schedulesArea"] = value;
            }
            get
            {
                if (ViewState["schedulesArea"] == null)
                {
                    throw new ArgumentNullException("DtAreaBase is null.");
                }
                return ViewState["schedulesArea"] as GS_SchedulesAreaDP;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                initPage();

                bindBudgetSchedule();
            }
        }

        private void initPage()
        {

            AreaId = long.Parse(Request["AreaId"].ToString());
            Command = int.Parse(Request["Command"].ToString());

            //取区间开始和结束时间
            schedulesArea = new GS_SchedulesAreaDP();
            schedulesArea = schedulesArea.GetReCorded(AreaId);
            StartTime = schedulesArea.STARTDATE;
            EndTime = schedulesArea.ENDDATE;
            Status = schedulesArea.STATUS;
            this.lblTimeArea.Text = StartTime.ToShortDateString() + " -- " + EndTime.ToShortDateString();

            this.btnGen.Visible = getGenButtonStatus();
            btnView.Visible = !btnGen.Visible;
            btnDelete.Visible = !btnGen.Visible;
            //btnDelete.Visible = false;
        }

        private bool getGenButtonStatus()
        {
            if (Command == 0)
            {
                return false;
            }
            return !new GS_Engineer_SchedulesDP().IsHavedWorkIssues(AreaId);
        }

        private void bindBudgetSchedule()
        {
            DtAreaBase = GS_Schedules_BaseDP.GetFullSchedulesAreaBase();
            DicTurnRuleDetl = GS_SchedulesAreaDP.GetTurnRuleDetl();
            TurnRule = GS_SchedulesAreaDP.GetTurnRule();
            Session["DicTurnRuleDetl"] = DicTurnRuleDetl;

            DataTable dtSchedule = curSchedulesRuleDp.GetScheduleTableByAreaId(AreaId);
            gridSchedule.DataSource = dtSchedule;
            gridSchedule.DataBind();

            bindNoShceduleingGrid();
           
        }

        private void bindNoShceduleingGrid()
        {
            DateTime timeNew = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            TimeSpan ts = timeNew - EndTime;
            if (ts.Days <= 0)
            {
                int count = 0;
                count = GS_SchedulesAreaDP.GetNoSchedulingEngineerCount(AreaId);
                if (count > 0)
                {
                    trNewGrid.Visible = true;
                    trNewTitle.Visible = true;

                    DataTable dtEngineer = null;
                    dtEngineer = GS_SchedulesAreaDP.GetNoSchedulingEngineer(AreaId);
                    gridScheduleNew.DataSource = dtEngineer;
                    gridScheduleNew.DataBind();
                }
                else
                {
                    trNewGrid.Visible = false;
                    trNewTitle.Visible = false;
                }
            }
            else
            {
                trNewGrid.Visible = false;
                trNewTitle.Visible = false;
            }
        }

        protected void btnGen_Click(object sender, EventArgs e)
        {
            List<string> SQLStringList = new List<string>();

            List<GS_TURN_RULEDP> turnList = new GS_TURN_RULEDP().GetAll();
            foreach (DataGridItem item in gridSchedule.Items)
            {
                HiddenField hidEngineerID = item.FindControl("hidEngineerID") as HiddenField; ;
                long EngineerID = long.Parse(hidEngineerID.Value);//gridSchedule.Items[0].Cells[1].Text;

                EngineerIssuesDP ei = new EngineerIssuesDP();
                ei.TurnList = turnList;
                ei.Load(EngineerID, AreaId);

                //获取排班类型控件(1固定班次/2轮班)
                DropDownList ddlTypeName = item.FindControl("ddlTypeName") as DropDownList;
                if (ddlTypeName.SelectedValue == "1")
                {
                    //班次
                    DropDownList ddlCurSchedulesName = item.FindControl("ddlCurSchedulesName") as DropDownList;

                    ei.Staff.WORKCATEID = Decimal.Parse(ddlTypeName.SelectedValue);
                    ei.Staff.SCHEDULESID = Decimal.Parse(ddlCurSchedulesName.SelectedValue);
                }
                else if (ddlTypeName.SelectedValue == "2")
                {
                    //班次
                    DropDownList ddlTurnDetl = item.FindControl("ddlTurnDetl") as DropDownList;
                    DropDownList ddlTurnName = item.FindControl("ddlTurnName") as DropDownList;

                    ei.Staff.WORKCATEID = Decimal.Parse(ddlTypeName.SelectedValue);
                    ei.Staff.SCHEDULESID = Decimal.Parse(ddlTurnDetl.SelectedValue);
                    ei.Staff.TRID = Decimal.Parse(ddlTurnName.SelectedValue);
                }

                List<string> curRowSqlList = ei.AcceptIssuesChange();
                if (curRowSqlList != null && curRowSqlList.Count > 0)
                {
                    foreach (string strSQL in curRowSqlList)
                    {
                        SQLStringList.Add(strSQL);
                    }
                }
            }
            if (EngineerIssuesDP.ExecuteSqlTran(SQLStringList))
            {
                curSchedulesRuleDp.CreateEveryOneWorkIssues(AreaId);
                redirectViewWorkIssues();
            }
            else
            {
                PageTool.MsgBox(this, "生成排班表失败！");
            }
        }

        

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (schedulesArea.STATUS == 3)
            {
                PageTool.MsgBox(this, "该期排班数据已经生效，系统禁止重新排班。");
                return;
            }

            bool nextIssuesFlag = schedulesArea.JudegePreAreaIssues(AreaId);
            if (!nextIssuesFlag)
            {
                PageTool.MsgBox(this, "下一个排班周期已经生成，系统不允许删除该期数据。");
                return;
            }

            curSchedulesRuleDp.DeleteRecordedByArea(this.AreaId);

            new GS_CurSchedulesRuleDP().CreateWorkIssues(AreaId);

            Response.Redirect("frm_frm_SchedulesArea_Set.aspx?Command=1&AreaId=" + AreaId.ToString() + "&StartDate=" + this.StartTime.ToShortDateString() + "&EndDate=" + this.EndTime.ToShortDateString());
                
            //bindBudgetSchedule();
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            redirectViewWorkIssues();
        }

        private void redirectViewWorkIssues()
        {
            string url = string.Format("frm_SchedulesArea_Summary.aspx?StartDate={0}&EndDate={1}&AreaId={2}",
                                                            StartTime.ToShortDateString(),
                                                            EndTime.ToShortDateString(),
                                                            AreaId);
            Response.Redirect(url);
        }

        protected void gridSchedule_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                SetDDLForTurnName(e);

                SetDDLForCurSchedulesName(e);             
            }
        }

        private void SetDDLForTurnName(DataGridItemEventArgs e)
        {
            if (TurnRule != null)
            {
                if (TurnRule.Rows.Count > 0)
                {
                    DropDownList ddlTurnName = e.Item.FindControl("ddlTurnName") as DropDownList;
                    HiddenField hidTurnID = e.Item.FindControl("hidTurnID") as HiddenField;

                    ddlTurnName.DataTextField = "turnname";
                    ddlTurnName.DataValueField = "trid";
                    ddlTurnName.DataSource = TurnRule;
                    if (!hidTurnID.Value.Equals("-1"))
                    {
                        ddlTurnName.SelectedValue = hidTurnID.Value;
                    }
                    ddlTurnName.DataBind();

                    SetDDLTurnRuleDetl(e, ddlTurnName, hidTurnID.Value);
                }
            }
        }

        private void SetDDLForCurSchedulesName(DataGridItemEventArgs e  )
        {
            if (DtAreaBase != null)
            {
                if (DtAreaBase.Rows.Count > 0)
                {
                    DropDownList ddlCurSchedulesName = e.Item.FindControl("ddlCurSchedulesName") as DropDownList;
                    HiddenField hidCurSchedulesID = e.Item.FindControl("hidCurSchedulesID") as HiddenField;
                    ddlCurSchedulesName.DataTextField = "fullname";
                    ddlCurSchedulesName.DataValueField = "schedulesid";
                    ddlCurSchedulesName.DataSource = DtAreaBase;
                    ddlCurSchedulesName.SelectedValue = hidCurSchedulesID.Value;
                    ddlCurSchedulesName.DataBind();
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("frm_frm_SchedulesArea.aspx");
        }

        protected void gridScheduleNew_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            long engineerID = long.Parse(e.CommandArgument.ToString());
            long workCateId = 0;
            long schedulesId = 0;
            long turnRuleId = 0;


            switch (e.CommandName.ToString())
            {
                case "Add":
                    DropDownList ddlTypeNameNew = e.Item.FindControl("ddlTypeNameNew") as DropDownList;
                    DropDownList ddlTurnNameNew = e.Item.FindControl("ddlTurnNameNew") as DropDownList;
                    DropDownList ddlCurSchedulesNameNew = e.Item.FindControl("ddlCurSchedulesNameNew") as DropDownList;
                    DropDownList ddlTurnDetlNew = e.Item.FindControl("ddlTurnDetlNew") as DropDownList;
                    //获取排班类型控件(1固定班次/2轮班)
                    if (ddlTypeNameNew.SelectedValue == "1")
                    {
                        workCateId = 1;
                        schedulesId = long.Parse(ddlCurSchedulesNameNew.SelectedValue);
                        turnRuleId = 0;
                    }
                    else if (ddlTypeNameNew.SelectedValue == "2")
                    {
                        workCateId = 2;
                        schedulesId = long.Parse(ddlTurnDetlNew.SelectedValue);
                        turnRuleId = long.Parse(ddlTurnNameNew.SelectedValue);
                    }

                    GS_SchedulesAreaDP.CreateEngineerWorkIssues(AreaId, engineerID, schedulesId, workCateId, turnRuleId);
                    bindBudgetSchedule();
                    break;
                default:
                    throw new Exception("");
            }
        }

        protected void gridScheduleNew_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (TurnRule != null)
                {
                    if (TurnRule.Rows.Count > 0)
                    {
                        DropDownList ddlTurnNameNew = e.Item.FindControl("ddlTurnNameNew") as DropDownList;

                        ddlTurnNameNew.DataTextField = "turnname";
                        ddlTurnNameNew.DataValueField = "trid";
                        ddlTurnNameNew.DataSource = TurnRule;
                        ddlTurnNameNew.DataBind();

                        SetDDLTurnRuleDetl2(e, ddlTurnNameNew);
                    }
                }

                if (DtAreaBase != null)
                {
                    if (DtAreaBase.Rows.Count > 0)
                    {
                        DropDownList ddlCurSchedulesNameNew = e.Item.FindControl("ddlCurSchedulesNameNew") as DropDownList;
                        ddlCurSchedulesNameNew.DataTextField = "fullname";
                        ddlCurSchedulesNameNew.DataValueField = "schedulesid";
                        ddlCurSchedulesNameNew.DataSource = DtAreaBase;
                        ddlCurSchedulesNameNew.DataBind();
                    }
                }
            }
        }

       
        /// <summary>
        /// 选择轮班初始化预选班次DropDownList
        /// </summary>
        /// <param name="e"></param>
        /// <param name="ddListTurn"></param>
        private void SetDDLTurnRuleDetl(DataGridItemEventArgs e, DropDownList ddListTurn, string hidTurnID)
        {
            long trID = long.Parse(ddListTurn.SelectedValue);
            if (trID > 0)
            {
                DataTable dtTurnRuleDetl = null;
                dtTurnRuleDetl = GetDTTurnRuleDetl(trID);

                if (dtTurnRuleDetl != null)
                {
                    if (dtTurnRuleDetl.Rows.Count > 0)
                    {
                        DropDownList ddlTurnDetl = e.Item.FindControl("ddlTurnDetl") as DropDownList;
                        HiddenField hidCurSchedulesID = e.Item.FindControl("hidCurSchedulesID") as HiddenField;
                        ddlTurnDetl.DataTextField = "fullname";
                        ddlTurnDetl.DataValueField = "schedulesid";
                        ddlTurnDetl.DataSource = dtTurnRuleDetl;
                        if (!hidTurnID.Equals("-1"))
                        {
                            ddlTurnDetl.SelectedValue = hidCurSchedulesID.Value;
                        }
                        ddlTurnDetl.DataBind();
                    }
                }
            }
        }

        /// <summary>
        /// 初始化工程师预选班次DropDownList
        /// </summary>
        /// <param name="e"></param>
        /// <param name="ddListTurn"></param>
        private void SetDDLTurnRuleDetl2(DataGridItemEventArgs e, DropDownList ddListTurn)
        {
            long trID = long.Parse(ddListTurn.SelectedValue);
            if (trID > 0)
            {
                DataTable dtTurnRuleDetl = null;
                dtTurnRuleDetl = GetDTTurnRuleDetl(trID);

                if (dtTurnRuleDetl != null)
                {
                    if (dtTurnRuleDetl.Rows.Count > 0)
                    {
                        DropDownList ddlTurnDetlNew = e.Item.FindControl("ddlTurnDetlNew") as DropDownList;
                        ddlTurnDetlNew.DataTextField = "fullname";
                        ddlTurnDetlNew.DataValueField = "schedulesid";
                        ddlTurnDetlNew.DataSource = dtTurnRuleDetl;
                        ddlTurnDetlNew.DataBind();
                    }
                }
            }
        }

        protected string GetDefaulControlState(string type)
        {
            string style = "display:none";
            if (type == "轮班")
            {
                style = "";
            }
            return style;
        }
        protected string GetDefaulControlState2(string type)
        {
            string style = "display:none";
            if (type == "固定班次")
            {
                style = "";
            }
            return style;
        }

        /// <summary>
        /// 获取轮班详细班次
        /// </summary>
        /// <param name="trID"></param>
        /// <returns></returns>
        private DataTable GetDTTurnRuleDetl(long trID)
        {
            DataTable dtTurnRuleDetl = null;
            if (DicTurnRuleDetl.Count == 0)
            {
                dtTurnRuleDetl = GS_SchedulesAreaDP.GetTurnRuleDetl(trID);
            }
            else
            {
                dtTurnRuleDetl = DicTurnRuleDetl[trID];
            }
            return dtTurnRuleDetl;
        }
    }
}
