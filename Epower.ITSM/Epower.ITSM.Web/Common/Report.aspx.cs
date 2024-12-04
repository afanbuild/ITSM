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
using Epower.ITSM.SqlDAL.Common;

namespace Epower.ITSM.Web.Common
{
    public partial class Report : BasePage
    {
        #region FromBackUrl
        public DataTable dt;
        private DataTable dt1;

        public string FromBackUrl
        {
            get
            {
                if (ViewState["FromBackUrl"] != null)
                    return ViewState["FromBackUrl"].ToString();
                else
                    return "";
            }
            set
            {
                ViewState["FromBackUrl"] = value;
            }
        }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {

            SetParentButtonEvent();


            Session["FromUrl"] = "../Common/FormRefreshPageMain.aspx";
            FromBackUrl = Session["FromUrl"].ToString();

            if (!IsPostBack)
            {
                LoadData();


            }


        }

        #region SetParentButtonEvent
        public void SetParentButtonEvent()
        {

            // this.Master.MainID = "1";
        }
        #endregion


        #region  LoadData
        /// <summary>
        /// 数据加载
        /// </summary>
        private void LoadData()
        {
            dt = RefreshPage.GetRefreshPageDate();
            this.refreshPageDataGrid.DataSource = dt;
            this.refreshPageDataGrid.DataBind();
            dt1 = RefreshPage.GetRefreshPageDate1();
            this._YearDataGrid.DataSource = dt1;
            this._YearDataGrid.DataBind();
            this.StatFootSum();

        }
        #endregion



        /// <summary>
        /// 
        /// </summary>
        private void StatFootSum()
        {
            double processing = 0;
            double iRespond = 0;
            double resolved = 0;
            double monthNum = 0;
            double dateNum = 0;
            foreach (DataGridItem row in this.refreshPageDataGrid.Controls[0].Controls)
            {
                if (row.ItemType == ListItemType.Footer)  //如果为合计的话
                {
                    ((Label)row.FindControl("ProcessingFoot")).Text = processing.ToString();
                    ((Label)row.FindControl("NoResponseFoot")).Text = iRespond.ToString();
                    ((Label)row.FindControl("resolvedFoot")).Text = resolved.ToString();
                    ((Label)row.FindControl("MonthNumFoot")).Text = monthNum.ToString();
                    ((Label)row.FindControl("DateNumFoot")).Text = dateNum.ToString();

                }
                else if (row.ItemType == ListItemType.Item || row.ItemType == ListItemType.AlternatingItem)
                {


                    processing += double.Parse(((Label)row.FindControl("lblProcessing")).Text);
                    iRespond += double.Parse(((Label)row.FindControl("lblNoResponse")).Text);
                    resolved += double.Parse(((Label)row.FindControl("lblresolved")).Text);
                    monthNum += double.Parse(((Label)row.FindControl("lblMonthNum")).Text);
                    dateNum += double.Parse(((Label)row.FindControl("lblDateNum")).Text);
                }
            }

            double yearNum = 0;
            double yearAchievements = 0;
            double quarter = 0;
            double quarterAchievements = 0;
            double monthNum1 = 0;
            double monthAchievements = 0;
            foreach (DataGridItem row in this._YearDataGrid.Controls[0].Controls)
            {
                if (row.ItemType == ListItemType.Footer)  //如果为合计的话
                {
                    ((Label)row.FindControl("lblYearNumFoot")).Text = yearNum.ToString();
                    ((Label)row.FindControl("lblYearAchievementsFoot")).Text = yearAchievements.ToString();
                    ((Label)row.FindControl("lblQuarterFoot")).Text = quarter.ToString();
                    ((Label)row.FindControl("lblQuarterAchievementsFoot")).Text = quarterAchievements.ToString();
                    ((Label)row.FindControl("lblMonthNumFoot")).Text = monthNum1.ToString();
                    ((Label)row.FindControl("lblMonthAchievementsFoot")).Text = monthAchievements.ToString();

                }
                else if (row.ItemType == ListItemType.Item || row.ItemType == ListItemType.AlternatingItem)
                {


                    yearNum += double.Parse(((Label)row.FindControl("lblYearNum")).Text);
                    yearAchievements += double.Parse(((Label)row.FindControl("lblYearAchievements")).Text);
                    quarter += double.Parse(((Label)row.FindControl("lblQuarter")).Text);
                    quarterAchievements += double.Parse(((Label)row.FindControl("lblQuarterAchievements")).Text);
                    monthNum1 += double.Parse(((Label)row.FindControl("lblMonthNum")).Text);
                    monthAchievements += double.Parse(((Label)row.FindControl("lblMonthAchievements")).Text);
                }
            }
        }

        public string DisplayValue(decimal num)
        {
            string str = string.Empty;
            if (num != 0)
                str = "(" + num + "%)";
            return str;
        }



    }
}
