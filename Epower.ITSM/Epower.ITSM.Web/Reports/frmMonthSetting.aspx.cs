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


namespace Epower.ITSM.Web.Reports
{
    public partial class frmMonthSetting : BasePage
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
            DataTable dt = Cst_ServiceLevelDP.Getmonthsetting();
            if (dt != null && dt.Rows.Count > 0)
            {
                CtrDateBegin.dateTime = DateTime.Parse(dt.Rows[0]["BeginTime"].ToString());
                CtrDateEnd.dateTime = DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["EndTime"].ToString());
                drbDay.SelectedIndex = drbDay.Items.IndexOf(drbDay.Items.FindByValue(dt.Rows[0]["bMonth"].ToString()));
               
                //当前月区间：从本月1号的0点0分--下月最后一天的23点59分

                //lblSetting.Text = "当前月区间:" + dt.Rows[0]["BEGINTIME"].ToString() + "至" + dt.Rows[dt.Rows.Count - 1]["ENDTIME"].ToString() + " 月区间:从上月" + dt.Rows[0]["bMonth"].ToString() + "号至下月" + ((int.Parse(dt.Rows[0]["bMonth"].ToString()) - 1) == 0 ? "最后一天" : (int.Parse(dt.Rows[0]["bMonth"].ToString()) - 1) + "号");
                if (decimal.Parse(dt.Rows[0]["bMonth"].ToString()) > 1)
                    lblSetting.Text = "当前月区间:从本月" + dt.Rows[0]["bMonth"].ToString() + "号的0点0分--下月" + ((int.Parse(dt.Rows[0]["bMonth"].ToString()) - 1) == 0 ? "最后一天" : (int.Parse(dt.Rows[0]["bMonth"].ToString()) - 1) + "号") + "的23点59分";
                else
                    lblSetting.Text = "当前月区间:从本月" + dt.Rows[0]["bMonth"].ToString() + "号的0点0分--本月" + ((int.Parse(dt.Rows[0]["bMonth"].ToString()) - 1) == 0 ? "最后一天" : (int.Parse(dt.Rows[0]["bMonth"].ToString()) - 1) + "号") + "的23点59分";
            }
        }

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            SetParentButtonEvent();
            if (!IsPostBack)
                Init();
        }

        /// <summary>
        /// 保存
        /// </summary>
        void Master_Master_Button_Save_Click()
        {

            DateTime btime =new DateTime(2010,1,1);
            DateTime Etime = new DateTime(2040, 12, 31);

            Cst_ServiceLevelDP.MonthSetting(btime, Etime, int.Parse(drbDay.SelectedItem.Value), int.Parse(CtrEnd.Value));
            Init();
        }
    }
}
