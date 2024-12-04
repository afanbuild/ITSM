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
    public partial class frm_frm_SchedulesArea : BasePage
    {
        private GS_SchedulesAreaDP areaDp = new GS_SchedulesAreaDP();

        private DataTable dtSchedulesArea = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindArea();

                //设置排班周期开始和结束时间
                initTextBoxTimeValue();
            }
        }

        private void initTextBoxTimeValue()
        {
            string strDateTime = GS_SchedulesAreaDP.GetMaxData();
            txtStartDate.dateTimeString = strDateTime;
            txtEndDate.dateTimeString = strDateTime;
        }

        protected void btnStart_Click(object sender, EventArgs e)
        {
            DateTime startDate = txtStartDate.dateTime;
            DateTime endTime = txtEndDate.dateTime;
            DateTime dateTime = DateTime.Parse(GS_SchedulesAreaDP.GetMaxData());
            TimeSpan ts = startDate - dateTime;
            TimeSpan ts2 = startDate - endTime;

            if (ts.Days < 0)
            {
                PageTool.MsgBox(this, "开始时间小于最大排班时间");
            }
            else if (ts2.Days > 0)
            {
                PageTool.MsgBox(this, "开始时间大于结束时间");
            }
            else
            {
                CreateWorkIssues();

                if (judgePreScheduleStatus())
                {
                    GS_CurSchedulesRuleDP curSchedulesRuleDp = new GS_CurSchedulesRuleDP();
                    curSchedulesRuleDp.CreateWorkIssues(areaDp.AREAID);

                    Response.Redirect("frm_frm_SchedulesArea_Set.aspx?Command=1&AreaId=" + areaDp.AREAID.ToString() + "&StartDate=" + areaDp.STARTDATE.ToShortDateString() + "&EndDate=" + areaDp.ENDDATE.ToShortDateString());
                }
                else
                {
                    initTextBoxTimeValue();
                    bindArea();
                }
              
            }
        }

        private bool judgePreScheduleStatus()
        {
            if (gridScheduleLog.Items.Count == 0)
            {
                return true;
            }
            int preStatus = int.Parse(((HiddenField)this.gridScheduleLog.Items[0].FindControl("hidAreaStatus")).Value);
            if (preStatus >1)
            {
                return true;
            }
            return false;

        }

        private void bindArea()
        {
            
            dtSchedulesArea = GS_SchedulesAreaDP.GetLast5Row();

            DataTable dtSchedulesAreaNew = null;
            dtSchedulesAreaNew = SetUpDataTable(dtSchedulesArea);

            this.gridScheduleLog.DataSource = dtSchedulesAreaNew;
            this.gridScheduleLog.DataBind();

            setLnkSchedureStatus();
        }

        /// <summary>
        /// 设置周期列表备注信息
        /// </summary>
        /// <param name="dtSchedulesArea"></param>
        /// <returns></returns>
        private static DataTable SetUpDataTable(DataTable dtSchedulesArea)
        {
            DataTable dtSchedulesAreaNew = new DataTable();
            dtSchedulesAreaNew.Columns.Add("remarks", typeof(System.String));
            dtSchedulesAreaNew.Columns.Add("StartDate", typeof(DateTime));
            dtSchedulesAreaNew.Columns.Add("EndDate", typeof(DateTime));
            dtSchedulesAreaNew.Columns.Add("AreaId", typeof(System.Int64));
            dtSchedulesAreaNew.Columns.Add("STATUS", typeof(System.Int32));
            dtSchedulesAreaNew.Columns.Add("NOISSUESCOUNT", typeof(System.Int32));

            foreach (DataRow dr in dtSchedulesArea.Rows)
            {
                DataRow row = dtSchedulesAreaNew.NewRow();
                int count = 0;
                long areaID = 0;
                areaID = long.Parse(dr["areaid"].ToString());

                if (dr["STATUS"].ToString() == "0")
                {
                    row["remarks"] = "还未开始排班。 ";
                }
                else
                {
                    DateTime endDate = DateTime.Parse(dr["EndDate"].ToString());
                    DateTime timeNew = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                    TimeSpan ts = timeNew - endDate;
                    if (ts.Days <= 0)
                    {
                        if (areaID > 0)
                        {
                            count = GS_SchedulesAreaDP.GetNoSchedulingEngineerCount(areaID);
                        }
                        if (count > 0)
                        {
                            row["remarks"] = "注意：还有" + count + "个工程师未排班。";
                        }
                        else
                        {
                            //row["remarks"] = "提示：工程师都已完成排班。 ";
                            row["remarks"] = " ";
                        }
                    }
                    else
                    {
                        row["remarks"] = "";
                    }
                    
                }

                row["StartDate"] = dr["StartDate"].ToString();
                row["EndDate"] = dr["EndDate"].ToString();
                row["STATUS"] = dr["STATUS"].ToString();
                row["AreaId"] = dr["AreaId"].ToString();
                row["NOISSUESCOUNT"] = count.ToString();

                dtSchedulesAreaNew.Rows.Add(row);
            }
            return dtSchedulesAreaNew;
        }

        private int insertAreaRecord()
        {
            return 0;
        }

        private bool isInsert()
        {
            return true;
        }

        protected void gridScheduleLog_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            GS_CurSchedulesRuleDP curSchedulesRuleDp = new GS_CurSchedulesRuleDP();

            int rowIndex =int.Parse(e.CommandArgument.ToString());

            long areaId=long.Parse(((HiddenField)gridScheduleLog.Items[rowIndex].FindControl("hidAreaId")).Value );
            DateTime startDate =DateTime.Parse(((HiddenField)gridScheduleLog.Items[rowIndex].FindControl("hidStartDate")).Value );
            DateTime endDate = DateTime.Parse(((HiddenField)gridScheduleLog.Items[rowIndex].FindControl("hidEndDate")).Value);
            long status =long.Parse(((HiddenField)gridScheduleLog.Items[rowIndex].FindControl("hidAreaStatus")).Value );

            switch (e.CommandName.ToString())
            {
                case "Delete":
                    if (rowIndex > 0)
                    {
                        PageTool.MsgBox(this, "请按照排班周期顺序删除数据。");
                        return;
                    }
                    areaDp.DeleteRecorded(areaId);
                    initTextBoxTimeValue();
                    bindArea();
                    break;
                case "View":
                    Response.Redirect("frm_SchedulesArea_Summary.aspx?Command=1&AreaId=" + areaId.ToString() + "&StartDate=" + startDate.ToShortDateString() + "&EndDate=" + endDate.ToShortDateString());
                    break;
                case "Do":
                    if (!curSchedulesRuleDp.HavedCreateWorkIssues(areaId))
                    {
                       curSchedulesRuleDp.CreateWorkIssues(areaId);
                    }
                    Response.Redirect("frm_frm_SchedulesArea_Set.aspx?Command=1&AreaId=" + areaId.ToString() + "&StartDate=" + startDate.ToShortDateString() + "&EndDate=" + endDate.ToShortDateString());
                    break;
                case "Supplement":
                    Response.Redirect("frm_frm_SchedulesArea_Set.aspx?Command=1&AreaId=" + areaId.ToString() + "&StartDate=" + startDate.ToShortDateString() + "&EndDate=" + endDate.ToShortDateString());
                    break;
                default:
                    throw new Exception("");
            }

        }

        /// <summary>
        /// 创建排班周期，跳转到排班详情
        /// </summary>
        private void CreateWorkIssues()
        {
            long areaID = 0;           

            try
            {
                areaID = long.Parse(EpowerGlobal.EPGlobal.GetNextID("GS_SCHEDULESAREAID").ToString());
                
                areaDp.AREAID = areaID;
                areaDp.STARTDATE = txtStartDate.dateTime;
                areaDp.ENDDATE = txtEndDate.dateTime;
                areaDp.STATUS = 0;
                areaDp.AREAFLAG = 0;
                areaDp.DELETED = 0;
                areaDp.CREBY = "";
                areaDp.CRETIME = DateTime.Now;
                areaDp.LATMDYBY = "";
                areaDp.LSTMDYTIME = DateTime.Now;
                areaDp.InsertRecorded(areaDp);

            }
            catch
            {
                throw;
            }
        }

        protected void gridScheduleLog_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            
        }

        private bool getDispSchedulesButtonStatus(DataTable dt, long curAreaId)
        {
            if (dt == null||dt.Rows.Count ==0)
            {
                return false;
            }
            foreach (DataRow dr in dt.Rows)
            {
                if (long.Parse(dr["AreaId"].ToString()) < curAreaId)
                {
                    return long.Parse(dr["STATUS"].ToString()) >= 2;
                }
            }
            return true;
        }

        protected void gridScheduleLog_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            HiddenField hidStauts = e.Item.FindControl("hidAreaStatus") as HiddenField;
            if (hidStauts == null)
            {
                return;
            }
            // <asp:HiddenField ID="hidAreaId" runat="server"  Value ='<%#Eval("AreaId") %>' />
            LinkButton lnkView = e.Item.FindControl("lnkView") as LinkButton;
            LinkButton lnkSchedules = e.Item.FindControl("lnkSchedules") as LinkButton;
            LinkButton lnkDelete = e.Item.FindControl("lnkDelete") as LinkButton;
            LinkButton lnkSupplement = e.Item.FindControl("lnkSupplement") as LinkButton;
            HiddenField hidAreaId = e.Item.FindControl("hidAreaId") as HiddenField;
            HiddenField hidNOISSUESCOUNT = e.Item.FindControl("hidNOISSUESCOUNT") as HiddenField;
            
            Label lblRemark = e.Item.FindControl("lblRemark") as Label;

            switch (hidStauts.Value)
            {
                case "0":
                    lnkView.Visible = false;
                    lnkSchedules.Enabled = getDispSchedulesButtonStatus(dtSchedulesArea, long.Parse(hidAreaId.Value ));
                    lnkDelete.Visible = true;
                    lnkSupplement.Visible = false;
                    break;
                case "1":
                    lnkView.Visible = false;
                    lnkSchedules.Enabled = getDispSchedulesButtonStatus(dtSchedulesArea, long.Parse(hidAreaId.Value));
                    lnkDelete.Visible = false;
                    lnkSupplement.Visible = false;
                    break;                
                case "2":            
                    lnkView.Visible = true;
                    lnkSchedules.Visible = false;
                    lnkDelete.Visible = false;
                    lnkSupplement.Visible = (hidNOISSUESCOUNT.Value == "0") ? false : true;
                    break;
                case "3":
                    lnkView.Visible = true;
                    lnkSchedules.Visible = false;
                    lnkDelete.Visible = false;
                    lnkSupplement.Visible = (hidNOISSUESCOUNT.Value == "0") ? false : true;
                    break;
                default:
                    throw new ArgumentNullException("hidStauts.Value is not range.");
            }          
        }

        

        private string  getRemarkAboutEmptyIssues()
        {
            DataTable noIssues = new Cst_ServiceStaffDP().GetNoSetWorkIssues() ;
            if (noIssues == null || noIssues.Rows.Count == 0)
            {
                return "";
            }
            System.Text.StringBuilder sbResult = new System.Text.StringBuilder();
            sbResult.Append("用户");
            foreach (DataRow dr in noIssues.Rows)
            {
                sbResult.AppendFormat(" <a href=aaa.aspx?id={0}>{1}</a>",dr["Id"].ToString(),dr["NAME"].ToString());                
            }
            sbResult.Append(" 未设置班次");
            return sbResult.ToString();
        }

        private bool isNoSetWorkIssues()
        {
            DataTable noIssues = new Cst_ServiceStaffDP().GetNoSetWorkIssues();
            if (noIssues == null || noIssues.Rows.Count == 0)
            {
                return false  ;
            }
            return true;
        }

        private void setLnkSchedureStatus()
        {
            int viewButtonStatus = 0;
            for (int i = gridScheduleLog.Items.Count-1; i >= 0 ; i--)
            {
                LinkButton lnkSchedules = gridScheduleLog.Items[i].FindControl("lnkSchedules") as LinkButton;
                LinkButton lnkView = gridScheduleLog.Items[i].FindControl("lnkView") as LinkButton;
                if (lnkView.Visible  )
                {
                    viewButtonStatus = 1;
                    continue;
                }
                if (viewButtonStatus == 1 && lnkSchedules.Visible )
                {
                    viewButtonStatus++;
                    continue;
                }
                if (viewButtonStatus > 1)
                {
                    lnkSchedules.Enabled = false;
                }
            }
        }


    }
}
