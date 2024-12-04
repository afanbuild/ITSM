/****************************************************************************
 * 
 * description:通用表单计划维护
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-10-16
 * *************************************************************************/
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using EpowerGlobal;
using EpowerCom;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.Common
{
    public partial class frmCommom_PlanDetailEdit : BasePage
    {
        #region PlanID
        /// <summary>
        /// 
        /// </summary>
        protected string PlanID
        {
            get
            {
                if (ViewState["PlanID"] == null)
                {
                    if (this.Master.MainID != string.Empty)
                        ViewState["PlanID"] = this.Master.MainID;
                    else
                        ViewState["PlanID"] = EPGlobal.GetNextID("EA_PlanDetailID").ToString();
                }
                return ViewState["PlanID"].ToString();
            }
            set
            {
                ViewState["PlanID"] = value;
            }
        }
        #endregion

        #region PlanType
        /// <summary>
        /// 
        /// </summary>
        protected string PlanType
        {
            get
            {
                if (Request["PlanType"] == null)
                {
                    if (ViewState["PlanType"] == null)
                        ViewState["PlanType"] = "0";
                }
                return ViewState["PlanType"].ToString();
            }
            set
            {
                ViewState["PlanType"] = value;
            }
        }
        #endregion

        #region PlanXml
        /// <summary>
        /// 计划时间串
        /// </summary>
        protected string PlanXml
        {
            get
            {
                if (this.Master.MainID != string.Empty)
                {
                    EA_PlanDetailDP ee = new EA_PlanDetailDP();
                    ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                    return ee.PlanXml.ToSetString();
                }
                else
                    return string.Empty;
            }
        }
        #endregion

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.Equ_CommonPlan;
            this.Master.IsCheckRight = true;
            if (this.Request.QueryString["id"] != null)
            {
                this.Master.MainID = this.Request.QueryString["id"].ToString();
            }
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);
            this.Master.Master_Button_Save_Click += new Global_BtnClick(Master_Master_Button_Save_Click);
            this.Master.Master_Button_GoHistory_Click += new Global_BtnClick(Master_Master_Button_GoHistory_Click);
            if (string.IsNullOrEmpty(this.Master.MainID.ToString()))
                this.Master.ShowAddPageButton();
            else
                this.Master.ShowEditPageButton();
        }
        #endregion

        #region Master_Master_Button_New_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("frmCommom_PlanDetailEdit.aspx?PlanType=" + PlanType);
        }
        #endregion

        #region Master_Master_Button_Delete_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            if (!String.IsNullOrEmpty(this.Master.MainID.ToString()))
            {
                EA_PlanDetailDP ee = new EA_PlanDetailDP();
                ee.DeleteRecorded(long.Parse(this.Master.MainID.Trim()));
                Master_Master_Button_GoHistory_Click();
            }
        }
        #endregion

        #region Master_Master_Button_GoHistory_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_GoHistory_Click()
        {
            Response.Redirect("frmEqu_PlanDetailMain.aspx?PlanType=" + PlanType);
        }
        #endregion

        #region Page_Load
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            SetParentButtonEvent();
            if (!IsPostBack)
            {
                InitDropDown();
                //加载数据
                LoadData();

                //加载计划时间
                PlanDetailSet psd = new PlanDetailSet(this.hidPlanXml.Value.Trim());
                LoadPlanValues(psd);
                SetPlan();

                PlanType = Request["PlanType"] != null ? Request["PlanType"].ToString() : "0";
            }
        }
        #endregion

        #region LoadData
        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            if (!string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                EA_PlanDetailDP ee = new EA_PlanDetailDP();
                ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                ddltFlow.SelectedValue = ee.RefID.ToString();
                txtPlanName.Text = ee.PlanName.ToString();
                txtPlanDesc.Text = ee.PlanDesc.ToString();
                ddltPlanState.SelectedValue = ee.PlanState.ToString();
                UserPicker1.UserID = long.Parse(ee.PlanDutyUserID.ToString());
                UserPicker1.UserName = ee.PlanDutyUserName.ToString();

                this.hidPlanXml.Value = ee.PlanXml.ToSetString();
            }
        }
        #endregion

        #region InitObject
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ee"></param>
        private void InitObject(EA_PlanDetailDP ee)
        {
            ee.RefType = (int)ePlan_Type.Common;
            ee.RefID = Decimal.Parse(ddltFlow.SelectedValue.Trim());
            ee.PlanName = txtPlanName.Text.Trim();
            ee.PlanDesc = txtPlanDesc.Text.Trim();

            PlanDetailSet pPlanDetailSet;
            //计算计划时间
            this.hidPlanXml.Value = GetPlanString();
            if (this.hidPlanXml.Value.Trim() != string.Empty)
            {
                pPlanDetailSet = new PlanDetailSet(this.hidPlanXml.Value.Trim());
            }
            else
            {
                pPlanDetailSet = new PlanDetailSet();
            }
            ee.PlanXml = pPlanDetailSet;

            ee.PlanExpand = string.Empty;
            //ee.PlanState = Int32.Parse(ddltPlanState.SelectedValue.Trim());
            ee.PlanDutyUserID = UserPicker1.UserID;
            ee.PlanDutyUserName = UserPicker1.UserName.Trim();
        }
        #endregion

        #region Master_Master_Button_Save_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Save_Click()
        {
            if (rblType.SelectedValue.Trim() != "10")  //持续运行
            {
                if (int.Parse(ddlHours.SelectedItem.Text) < int.Parse(ddlHoursBeg.SelectedItem.Text) || int.Parse(ddlHours.SelectedItem.Text) > int.Parse(ddlHoursEnd.SelectedItem.Text))
                {
                    PageTool.MsgBox(this, "指定时间在运行时间范围外,请重新指定");
                    this.Master.IsSaveSuccess = false;
                    return;
                }

                if (int.Parse(ddlHours.SelectedItem.Text) == int.Parse(ddlHoursBeg.SelectedItem.Text) || int.Parse(ddlHours.SelectedItem.Text) == int.Parse(ddlHoursEnd.SelectedItem.Text))
                {
                    //判断分钟
                    if (int.Parse(ddlMinutes.SelectedItem.Text) < int.Parse(ddlMinutesBeg.SelectedItem.Text) || int.Parse(ddlMinutes.SelectedItem.Text) > int.Parse(ddlMinutesEnd.SelectedItem.Text))
                    {
                        PageTool.MsgBox(this, "指定时间在运行时间范围外,请重新指定");
                        this.Master.IsSaveSuccess = false;
                        return;
                    }
                }
            }

            //判断是否可以启动流程
            int intCanStart = FlowModel.CanUseFlowModel(UserPicker1.UserID, long.Parse(ddltFlow.SelectedValue.Trim()));
            if (intCanStart != 0)
            {
                if (intCanStart == -2)
                {
                    PageTool.MsgBox(this, UserPicker1.UserName + "没有权限执行此操作！请与系统管理人员联系");
                    this.Master.IsSaveSuccess = false;
                }
                else
                {
                    PageTool.MsgBox(this, UserPicker1.UserName + "所执行的流程模型暂时无效！请与系统管理人员联系");
                    this.Master.IsSaveSuccess = false;
                }
                return;
            }
            EA_PlanDetailDP ee = new EA_PlanDetailDP();
            if (string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                InitObject(ee);
                ee.ID = decimal.Parse(PlanID);
                ee.Deleted = (int)eRecord_Status.eNormal;
                ee.RegUserID = long.Parse(Session["UserID"].ToString());
                ee.RegUserName = Session["PersonName"].ToString();
                ee.RegTime = DateTime.Now;
                ee.RegDeptID = long.Parse(Session["UserDeptID"].ToString());
                ee.RegDeptName = Session["UserDeptName"].ToString();
                ee.UpdateUserID = long.Parse(Session["UserID"].ToString());
                ee.UpdateUserName = Session["PersonName"].ToString();
                ee.UpdateTime = DateTime.Now;
                ee.RunStatus = (int)ePlan_RunState.NoRun;  //未运行
                ee.PlanState = (int)ePlan_State.Inefficacy; //计划有效性，默认无效

                ee.InsertRecorded(ee);

                this.Master.MainID = ee.ID.ToString();
            }
            else
            {
                ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                InitObject(ee);
                ee.UpdateRecorded(ee);
                this.Master.MainID = ee.ID.ToString();
            }

            //计算下次运行时间
            if (ee.PlanState == (int)ePlan_State.Effect)
                ee.CalcNextTime(true);
        }
        #endregion

        #region  初始化流程列表
        /// <summary>
        /// 初始化流程列表
        /// </summary>
        private void InitDropDown()
        {
            DataTable dt = AppFieldConfigDP.GetFlowModelList();

            ddltFlow.Items.Clear();
            ddltFlow.DataSource = dt.DefaultView;
            ddltFlow.DataTextField = "FlowName";
            ddltFlow.DataValueField = "FlowModelID";
            ddltFlow.DataBind();

            ddltFlow.Items.Insert(0, new ListItem("--流程--", ""));
        }
        #endregion

        #region rblType_SelectedIndexChanged
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetPlan();
        }
        /// <summary>
        /// 
        /// </summary>
        private void SetPlan()
        {
            if (rblType.SelectedValue.Trim() == "10")  //持续运行
            {
                trPlanTime.Visible = false;
                trPlanDateWeek.Visible = false;
                trPlanDateMonth.Visible = false;
                trTime.Visible = true;
                trWeek.Visible = false;
            }
            else if (rblType.SelectedValue.Trim() == "20") //每日执行
            {
                trPlanTime.Visible = true;
                trPlanDateWeek.Visible = false;
                trPlanDateMonth.Visible = false;
                trTime.Visible = false;
                trWeek.Visible = false;
            }
            else if (rblType.SelectedValue.Trim() == "30")  //每周执行
            {
                trPlanTime.Visible = true;
                trPlanDateWeek.Visible = true;
                trPlanDateMonth.Visible = false;
                trTime.Visible = false;
                trWeek.Visible = true;
            }
            else if (rblType.SelectedValue.Trim() == "40")  //每月执行
            {
                trPlanTime.Visible = true;
                trPlanDateWeek.Visible = false;
                trPlanDateMonth.Visible = true;
                trTime.Visible = false;
                trWeek.Visible = false;
            }
        }
        #endregion

        #region 计划时间设置
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetPlanString()
        {
            PlanDetailSet pds = new PlanDetailSet();
            //类别
            pds.SetType = (ePlan_SetType)(int.Parse(rblType.SelectedValue));


            //运行间隔
            pds.Interval = int.Parse(txtInterval.Text) * 3600000;


            //指定运行时间
            pds.SpecfiedTime = ddlHours.SelectedItem.Text + ":" + ddlMinutes.SelectedItem.Text;

            //时间范围
            pds.BeginTime = ddlHoursBeg.SelectedItem.Text + ":" + ddlMinutesBeg.SelectedItem.Text;
            pds.EndTime = ddlHoursEnd.SelectedItem.Text + ":" + ddlMinutesEnd.SelectedItem.Text;

            //指定日期(每月执行)
            pds.Day = ddlDays.SelectedItem.Value;
            //指定日期(每周执行)
            pds.WeekDay = ddlWeeks.SelectedItem.Value;


            //星期值
            string sWeeks = "";
            if (cblWeeks.Items[0].Selected == true)
            {
                sWeeks += "0";
            }

            if (cblWeeks.Items[1].Selected == true)
            {
                sWeeks += "1";
            }

            if (cblWeeks.Items[2].Selected == true)
            {
                sWeeks += "2";
            }

            if (cblWeeks.Items[3].Selected == true)
            {
                sWeeks += "3";
            }

            if (cblWeeks.Items[4].Selected == true)
            {
                sWeeks += "4";
            }

            if (cblWeeks.Items[5].Selected == true)
            {
                sWeeks += "5";
            }

            if (cblWeeks.Items[6].Selected == true)
            {
                sWeeks += "6";
            }
            pds.Weeks = sWeeks;

            return pds.ToSetString();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pds"></param>
        private void LoadPlanValues(PlanDetailSet pds)
        {
            //类别
            rblType.SelectedValue = ((int)pds.SetType).ToString();

            //运行间隔
            decimal dm = pds.Interval / 3600000;
            txtInterval.Text = dm.ToString();

            //指定运行时间

            string[] specTime = pds.SpecfiedTime.Split(":".ToCharArray());
            for (int i = 0; i < 24; i++)
            {
                ddlHours.Items.Add(new ListItem(i.ToString(), i.ToString()));
                if (i == int.Parse(specTime[0]))
                {
                    ddlHours.SelectedIndex = i;
                }
            }
            for (int i = 0; i < 60; i++)
            {
                ddlMinutes.Items.Add(new ListItem(i.ToString(), i.ToString()));
                if (i == int.Parse(specTime[1]))
                {
                    ddlMinutes.SelectedIndex = i;
                }
            }

            //指定日期(每月执行)
            for (int i = 0; i < 31; i++)
            {
                ddlDays.Items.Add(new ListItem(i.ToString(), i.ToString()));
                if (i == int.Parse(pds.Day))
                {
                    ddlDays.SelectedIndex = i;
                }
            }

            //指定日期(每周执行)
            ddlWeeks.Items.Add(new ListItem("星期日", "0"));
            ddlWeeks.Items.Add(new ListItem("星期一", "1"));
            ddlWeeks.Items.Add(new ListItem("星期二", "2"));
            ddlWeeks.Items.Add(new ListItem("星期三", "3"));
            ddlWeeks.Items.Add(new ListItem("星期四", "4"));
            ddlWeeks.Items.Add(new ListItem("星期五", "5"));
            ddlWeeks.Items.Add(new ListItem("星期六", "6"));
            ddlWeeks.SelectedIndex = int.Parse(pds.WeekDay);

            //时间范围
            //构造开始时间
            string[] beginTime = pds.BeginTime.Split(":".ToCharArray());

            //构造结束时间
            string[] endTime = pds.EndTime.Split(":".ToCharArray());

            for (int i = 0; i < 24; i++)
            {
                ddlHoursBeg.Items.Add(new ListItem(i.ToString(), i.ToString()));
                if (i == int.Parse(beginTime[0]))
                {
                    ddlHoursBeg.SelectedIndex = i;
                }
            }
            for (int i = 0; i < 60; i++)
            {
                ddlMinutesBeg.Items.Add(new ListItem(i.ToString(), i.ToString()));
                if (i == int.Parse(beginTime[1]))
                {
                    ddlMinutesBeg.SelectedIndex = i;
                }
            }
            for (int i = 0; i < 24; i++)
            {
                ddlHoursEnd.Items.Add(new ListItem(i.ToString(), i.ToString()));
                if (i == int.Parse(endTime[0]))
                {
                    ddlHoursEnd.SelectedIndex = i;
                }
            }
            for (int i = 0; i < 60; i++)
            {
                ddlMinutesEnd.Items.Add(new ListItem(i.ToString(), i.ToString()));
                if (i == int.Parse(endTime[1]))
                {
                    ddlMinutesEnd.SelectedIndex = i;
                }
            }

            //星期值
            if (pds.Weeks.IndexOf("0") >= 0)
            {
                cblWeeks.Items[0].Selected = true;
            }
            else
            {
                cblWeeks.Items[0].Selected = false;
            }

            if (pds.Weeks.IndexOf("1") >= 0)
            {
                cblWeeks.Items[1].Selected = true;
            }
            else
            {
                cblWeeks.Items[1].Selected = false;
            }

            if (pds.Weeks.IndexOf("2") >= 0)
            {
                cblWeeks.Items[2].Selected = true;
            }
            else
            {
                cblWeeks.Items[2].Selected = false;
            }

            if (pds.Weeks.IndexOf("3") >= 0)
            {
                cblWeeks.Items[3].Selected = true;
            }
            else
            {
                cblWeeks.Items[3].Selected = false;
            }

            if (pds.Weeks.IndexOf("4") >= 0)
            {
                cblWeeks.Items[4].Selected = true;
            }
            else
            {
                cblWeeks.Items[4].Selected = false;
            }

            if (pds.Weeks.IndexOf("5") >= 0)
            {
                cblWeeks.Items[5].Selected = true;
            }
            else
            {
                cblWeeks.Items[5].Selected = false;
            }

            if (pds.Weeks.IndexOf("6") >= 0)
            {
                cblWeeks.Items[6].Selected = true;
            }
            else
            {
                cblWeeks.Items[6].Selected = false;
            }
        }
        #endregion
    }
}