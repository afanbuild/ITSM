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
using Epower.DevBase.BaseTools;


namespace Epower.ITSM.Web.AppForms
{
    public partial class frmWeekSetting : BasePage
    {
        #region 是否查询
        /// <summary>
        /// 
        /// </summary>
        protected bool IsSelect
        {
            get
            {
                if (Request["IsSelect"] != null && Request["IsSelect"] == "true")
                    return true;
                else
                    return false;
            }
        }
        #endregion

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.SystemManager;
            this.Master.IsCheckRight = true;
            this.Master.Master_Button_Save_Click += new Global_BtnClick(Master_Master_Button_Save_Click);
            this.Master.ShowAddPageButton();
            this.Master.ShowNewButton(false);
            this.Master.ShowDeleteButton(false);
            this.Master.ShowSaveButton(true);
            this.Master.ShowBackUrlButton(false);
            Master.ShowHomeButton(false);
            if (Master.GetEditRight() == false)
            {
                this.Master.ShowSaveButton(false);
            }
        }
        #endregion

        //加载当前数据
        private void Init()
        {
            DataTable dt = Cst_ServiceLevelDP.GetWeekSetting();
            if (dt != null && dt.Rows.Count > 0)
            {
                CtrDateBegin.dateTime = DateTime.Parse(dt.Rows[0]["BeginTime"].ToString());
                CtrDateEnd.dateTime = DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["EndTime"].ToString());
                drbDay.SelectedIndex = drbDay.Items.IndexOf(drbDay.Items.FindByValue(dt.Rows[0]["wBegin"].ToString()));

                if (decimal.Parse(dt.Rows[0]["wBegin"].ToString()) > 1)
                    lblSetting.Text = "当前周区间:从本周星期" + ChangeWeek(int.Parse(dt.Rows[0]["wBegin"].ToString())) + "至下周星期" + ChangeWeek(int.Parse(dt.Rows[0]["wBegin"].ToString()) - 1);
                else
                    lblSetting.Text = "当前周区间:从本周星期" + ChangeWeek(int.Parse(dt.Rows[0]["wBegin"].ToString())) + "至本周星期" + ChangeWeek(int.Parse(dt.Rows[0]["wBegin"].ToString()) - 1);                
            }
        }

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
                Init();

        }
        #endregion

        #region Master_Master_Button_Save_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Save_Click()
        {
            DateTime btime = new DateTime(2010, 1, 1);
            DateTime Etime = new DateTime(2040, 12, 31);

            Cst_ServiceLevelDP.WeekSetting(btime, Etime, int.Parse(drbDay.SelectedItem.Value), int.Parse(CtrEnd.Value));
            Init();
        }
        #endregion

        /// <summary>
        /// 转换周显示
        /// </summary>
        /// <param name="week"></param>
        /// <returns></returns>
        private string ChangeWeek(int week)
        {
            string Weeks = "";

            switch (week)
            { 
                case 0:
                    Weeks = "天";
                    break;
                case 1:
                    Weeks = "一";
                    break;
                case 2:
                    Weeks = "二";
                    break;
                case 3:
                    Weeks = "三";
                    break;
                case 4:
                    Weeks = "四";
                    break;
                case 5:
                    Weeks = "五";
                    break;
                case 6:
                    Weeks = "六";
                    break;
                case 7:
                    Weeks = "天";
                    break;
                default:
                    Weeks = "";
                    break;
            }

            return Weeks;
        }
    }
}
