using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Web.Controls;

namespace Epower.ITSM.Web.AppSchedules
{
    public partial class frm_SchedulesArea_BaseSet : BasePage
    {
        public long SchedulesID
        {
            set
            {
                ViewState["SchedulesID"] = value;
            }
            get
            {
                if (ViewState["SchedulesID"] == null)
                {
                    throw new ArgumentNullException("SchedulesID is null.");
                }
                return long.Parse(ViewState["SchedulesID"].ToString());
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                initPage();
                bindDetailed();
            }
        }

        private void initPage()
        {
            SchedulesID = long.Parse(Request["SchedulesID"].ToString());
            Command = int.Parse(Request["Command"].ToString());
        }

        private void bindDetailed()
        {
            if (Command != 0)
            {
                GS_Schedules_BaseDP gsScheduleBase = null;
                gsScheduleBase = GS_Schedules_BaseDP.GetSchedulesAreaBase(SchedulesID);

                txtFullName.Text = gsScheduleBase.FULLNAME;
                txtSimpleName.Text = gsScheduleBase.SIMPLENAME;
                CtrStartdaytime.dateTime = gsScheduleBase.STARTDAYTIME;
                CtrStartsegmenttime.dateTime = gsScheduleBase.STARTSEGMENTTIME;
                CtrEndsegmenttime.dateTime = gsScheduleBase.ENDSEGMENTTIME;
                CtrEnddaytime.dateTime = gsScheduleBase.ENDDAYTIME;
                txtFullName.Text = gsScheduleBase.FULLNAME;
                txtContent.Value = gsScheduleBase.CREBY;

                int time = CtrStartdaytime.Hour;
                if (gsScheduleBase.OVERDAYFLAG1 == 1)
                {
                    ddEndsegmenttime.SelectedValue = "1";
                }

                if (gsScheduleBase.OVERDAYFLAG2 == 1)
                {
                    ddStartsegmenttime.SelectedValue = "1";
                }

                if (gsScheduleBase.OVERDAYFLAG3 == 1)
                {
                    ddEnddaytime.SelectedValue = "1";
                }
            }
        }

        protected void BtnSub_ok_Click(object sender, System.EventArgs e)
        {
            int endsegment = int.Parse(ddEndsegmenttime.SelectedValue);
            int startsegment = int.Parse(ddStartsegmenttime.SelectedValue);
            int endday = int.Parse(ddEnddaytime.SelectedValue);

            if (txtFullName.Text.Trim().Length > 50
                || txtSimpleName.Text.Trim().Length > 10)
            {
                PageTool.MsgBox(this, "全称和简称都不能为空！");
                return;
            }

            string fullNmae = txtFullName.Text.Trim();
            string simpleName = txtSimpleName.Text.Trim();

            string strVerification = IsScheduleBaseForm(endsegment, startsegment, endday, fullNmae, simpleName);

            if (strVerification.Equals("Success"))
            {
                SetAddOrUpate(endsegment, startsegment, endday, fullNmae, simpleName);
            }
            else
            {
                PageTool.MsgBox(this, strVerification);
            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("fem_SchedulesArea_Base.aspx");
        }

        /// <summary>
        /// 添加新班次或修改班次
        /// </summary>
        /// <param name="endsegment"></param>
        /// <param name="startsegment"></param>
        /// <param name="endday"></param>
        /// <param name="fullNmae"></param>
        /// <param name="simpleName"></param>
        private void SetAddOrUpate(int endsegment, int startsegment, int endday, string fullNmae, string simpleName)
        {
            GS_Schedules_BaseDP gsScheduleBase = new GS_Schedules_BaseDP();

            decimal ABS_LENGTH1 = CalculateTime(CtrStartdaytime.Hour, CtrStartdaytime.Minute, CtrEndsegmenttime.Hour, CtrEndsegmenttime.Minute, endsegment);
            decimal ABS_LENGTH2 = CalculateTime(CtrStartdaytime.Hour, CtrStartdaytime.Minute, CtrStartsegmenttime.Hour, CtrStartsegmenttime.Minute, startsegment);
            decimal ABS_LENGTH3 = CalculateTime(CtrStartdaytime.Hour, CtrStartdaytime.Minute, CtrEnddaytime.Hour, CtrEnddaytime.Minute, endday);
            decimal ABS_LENGTH4 = CalculateTime(CtrStartsegmenttime.Hour, CtrStartsegmenttime.Minute, startsegment, CtrEnddaytime.Hour, CtrEnddaytime.Minute, endday);

            gsScheduleBase.FULLNAME = fullNmae;
            gsScheduleBase.SIMPLENAME = simpleName;
            gsScheduleBase.STARTDAYTIME = CtrStartdaytime.dateTime;
            gsScheduleBase.STARTSEGMENTTIME = CtrStartsegmenttime.dateTime;
            gsScheduleBase.ENDSEGMENTTIME = CtrEndsegmenttime.dateTime;
            gsScheduleBase.ENDDAYTIME = CtrEnddaytime.dateTime;
            gsScheduleBase.ABS_LENGTH1 = ABS_LENGTH1;
            gsScheduleBase.ABS_LENGTH2 = ABS_LENGTH2;
            gsScheduleBase.ABS_LENGTH3 = ABS_LENGTH3;
            gsScheduleBase.WORKHOUR = ABS_LENGTH1 + ABS_LENGTH4;

            if (endsegment == 0 && startsegment == 0 && endday == 0)
            {
                gsScheduleBase.OVERDAYFLAG = 0;
            }
            else
            {
                gsScheduleBase.OVERDAYFLAG = 1;
            }

            gsScheduleBase.OVERDAYFLAG1 = endsegment;
            gsScheduleBase.OVERDAYFLAG2 = startsegment;
            gsScheduleBase.OVERDAYFLAG3 = endday;

            gsScheduleBase.CREBY = txtContent.Value;

            try
            {
                if (Command != 0)
                {
                    gsScheduleBase.SCHEDULESID = SchedulesID;
                    gsScheduleBase.UpdateRecorded(gsScheduleBase);
                }
                else
                {
                    gsScheduleBase.DELETED = 0;
                    gsScheduleBase.CRETIME = DateTime.Now;
                    gsScheduleBase.LATMDYBY = "";
                    gsScheduleBase.LSTMDYTIME = DateTime.Now;
                    gsScheduleBase.InsertRecorded(gsScheduleBase);
                }
            }
            catch
            {
                throw;
            }

            Response.Redirect("fem_SchedulesArea_Base.aspx");
        }

        /// <summary>
        /// 验证页面表单信息
        /// </summary>
        /// <param name="endsegment"></param>
        /// <param name="startsegment"></param>
        /// <param name="endday"></param>
        /// <param name="fullNmae"></param>
        /// <param name="simpleName"></param>
        /// <returns></returns>
        private string IsScheduleBaseForm(int endsegment, int startsegment, int endday, string fullNmae, string simpleName)
        {
            string strVerification = string.Empty;

            if (fullNmae.Equals(string.Empty))
            {
                strVerification = "全称不能为空！";
            }

            if (simpleName.Equals(string.Empty))
            {
                strVerification = "简称不能为空！";
            }

            if (endsegment == 0 && CtrStartdaytime.Hour == CtrEndsegmenttime.Hour && CtrStartdaytime.Minute == CtrEndsegmenttime.Minute)
            {
                strVerification = "上班时间和休息时间相同！";
            }

            if (endsegment == 0 && startsegment == 0 && CtrEndsegmenttime.Hour == CtrStartsegmenttime.Hour && CtrEndsegmenttime.Minute == CtrStartsegmenttime.Minute)
            {
                strVerification = "休息时间和休息上班时间相同！";
            }

            if (endsegment == 1 && startsegment == 1 && CtrEndsegmenttime.Hour == CtrStartsegmenttime.Hour && CtrEndsegmenttime.Minute == CtrStartsegmenttime.Minute)
            {
                strVerification = "休息时间和休息上班时间相同！";
            }

            if (startsegment == 0 && endday == 0 && CtrStartsegmenttime.Hour == CtrEnddaytime.Hour && CtrStartsegmenttime.Minute == CtrEnddaytime.Minute)
            {
                strVerification = "休息上班时间和下班时间相同！";
            }

            if (startsegment == 1 && endday == 1 && CtrStartsegmenttime.Hour == CtrEnddaytime.Hour && CtrStartsegmenttime.Minute == CtrEnddaytime.Minute)
            {
                strVerification = "休息上班时间和下班时间相同！";
            }

            if (endsegment == 0 && CtrStartdaytime.Hour >= CtrEndsegmenttime.Hour && strVerification.Equals(string.Empty))
            {
                if (CtrStartdaytime.Hour != CtrEndsegmenttime.Hour)
                {
                    strVerification = "休息时间错误！";
                }
                else if (CtrStartdaytime.Hour == CtrEndsegmenttime.Hour && CtrStartdaytime.Minute >= CtrEndsegmenttime.Minute)
                {
                    strVerification = "休息时间错误！";
                }
            }

            if (endsegment == 0 && startsegment == 0 && CtrEndsegmenttime.Hour >= CtrStartsegmenttime.Hour && strVerification.Equals(string.Empty))
            {
                if (CtrEndsegmenttime.Hour != CtrStartsegmenttime.Hour)
                {
                    strVerification = "休息上班时间错误！";
                }
                else if (CtrEndsegmenttime.Hour == CtrStartsegmenttime.Hour && CtrEndsegmenttime.Minute >= CtrStartsegmenttime.Minute)
                {
                    strVerification = "休息上班时间错误！";
                }
                else
                {
                    strVerification = string.Empty;
                }

            }

            if (endsegment == 1 && startsegment == 1 && CtrEndsegmenttime.Hour >= CtrStartsegmenttime.Hour && strVerification.Equals(string.Empty))
            {
                if (CtrEndsegmenttime.Hour != CtrStartsegmenttime.Hour)
                {
                    strVerification = "休息上班时间错误！";
                }
                else if (CtrEndsegmenttime.Hour == CtrStartsegmenttime.Hour && CtrEndsegmenttime.Minute >= CtrStartsegmenttime.Minute)
                {
                    strVerification = "休息上班时间错误！";
                }
                else
                {
                    strVerification = string.Empty;
                }

            }

            if (startsegment == 0 && endday == 0 && CtrStartsegmenttime.Hour >= CtrEnddaytime.Hour && strVerification.Equals(string.Empty))
            {
                if (CtrStartsegmenttime.Hour != CtrEnddaytime.Hour)
                {
                    strVerification = "下班时间错误！";
                }
                else if (CtrStartsegmenttime.Hour == CtrEnddaytime.Hour && CtrStartsegmenttime.Minute >= CtrEnddaytime.Minute)
                {
                    strVerification = "下班时间错误！";
                }
                else
                {
                    strVerification = string.Empty;
                }
            }

            if (startsegment == 1 && endday == 1 && CtrStartsegmenttime.Hour >= CtrEnddaytime.Hour && strVerification.Equals(string.Empty))
            {
                if (CtrStartsegmenttime.Hour != CtrEnddaytime.Hour)
                {
                    strVerification = "下班时间错误！";
                }
                else if (CtrStartsegmenttime.Hour == CtrEnddaytime.Hour && CtrStartsegmenttime.Minute >= CtrEnddaytime.Minute)
                {
                    strVerification = "下班时间错误！";
                }
                else
                {
                    strVerification = string.Empty;
                }
            }
            if (strVerification.Equals(string.Empty))
            {
                strVerification = "Success";
            }
            return strVerification;
        }

        /// <summary>
        /// 计算工作时间1
        /// </summary>
        /// <param name="hour1"></param>
        /// <param name="minute1"></param>
        /// <param name="hour2"></param>
        /// <param name="minute2"></param>
        /// <param name="interDay"></param>
        /// <returns></returns>
        private decimal CalculateTime(int hour1, int minute1, int hour2, int minute2, int interDay)
        {
            decimal count = 0.00m;
            DateTime dTime = DateTime.Now;
            DateTime startDate = new DateTime(dTime.Year, dTime.Month, dTime.Day, hour1, minute1, 0);
            DateTime endDate = new DateTime(dTime.Year, dTime.Month, dTime.Day + interDay, hour2, minute2, 0);

            TimeSpan ts = endDate - startDate;
            decimal hour = decimal.Parse(ts.Hours.ToString());
            decimal minute = decimal.Parse(ts.Minutes.ToString());
            count = decimal.Parse((hour + (minute / 60)).ToString("0.00"));
            return count;
        }

        /// <summary>
        /// 计算工作时间2
        /// </summary>
        /// <param name="hour1"></param>
        /// <param name="minute1"></param>
        /// <param name="interDay1"></param>
        /// <param name="hour2"></param>
        /// <param name="minute2"></param>
        /// <param name="interDay2"></param>
        /// <returns></returns>
        private decimal CalculateTime(int hour1, int minute1, int interDay1, int hour2, int minute2, int interDay2)
        {
            decimal count = 0.00m;
            DateTime dTime = DateTime.Now;
            DateTime startDate = new DateTime(dTime.Year, dTime.Month, dTime.Day + interDay1, hour1, minute1, 0);
            DateTime endDate = new DateTime(dTime.Year, dTime.Month, dTime.Day + interDay2, hour2, minute2, 0);

            TimeSpan ts = endDate - startDate;
            decimal hour = decimal.Parse(ts.Hours.ToString());
            decimal minute = decimal.Parse(ts.Minutes.ToString());
            count = decimal.Parse((hour + (minute / 60)).ToString("0.00"));
            return count;
        }
    }
}
