/*******************************************************************
 *
 * Description:服务监管页面
 * 
 * 
 * Create By  :zhumc
 * Create Date:2008年4月5日
 * *****************************************************************/
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
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.AppForms
{
    /// <summary>
    /// frmServiceContrals
    /// </summary>
    public partial class frmServiceContrals : BasePage
    {
        string sSql = string.Empty;
        string sWhere = string.Empty;
        DataTable dt = new DataTable();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.TableVisible = false;
            InitPage();
            if (!IsPostBack)
            {
                CtrdBegin.dateTime = DateTime.Parse(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd"));
                CtrdEnd.dateTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                CtrdBegin.OnChangeScript = "ChangeDate();";
                CtrdEnd.OnChangeScript = "ChangeDate();";
                LoadServiceData();
            }
            else
            { 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadServiceData()
        {
            sSql = @"select sum(case Status when 30 then 1 else 0 end) 已完成,
                           sum(case Status when 20 then 1 else 0 end) 未完成,
                           sum(case when ExpectEndTime is null then 0 when Status=30 and EndTime>ExpectEndTime then 1 end) 超时完成,
                           sum(case when ExpectEndTime is null then 0 when Status=20 and sysdate>ExpectEndTime then 1 end) 超时未完成,
                           to_char(to_number(sum(case when ExpectEndTime is null then 0 when nvl(EndTime,sysdate)>ExpectEndTime then 1 end))/
	                    count(FlowID)*100,18,2)+'%' 超时事件率,
	                    AppID
                    from es_Flow Where AppID<>199";
            if (CtrdBegin.dateTime.ToString() != string.Empty)
            {
                sWhere += " and StartTime>= to_date(" + StringTool.SqlQ(CtrdBegin.dateTime.ToString("yyyy-MM-dd")) + ",'yyyy-MM-dd')";
            }
            if (CtrdEnd.dateTime.ToString() != string.Empty)
            {
                sWhere += " and StartTime<= to_date(" + StringTool.SqlQ(CtrdEnd.dateTime.ToString("yyyy-MM-dd") + " 23:59:59") + ",'yyyy-MM-dd HH24:mi:ss')";
            }
            sSql += sWhere + " Group By AppID";
            dt = Epower.ITSM.SqlDAL.CommonDP.ExcuteSqlTable(sSql);
            foreach (DataRow dr in dt.Rows)
            {
                string sAppID = dr["AppID"].ToString();
                switch (sAppID)
                {
                    case "1026":   //服务单
                        lblServiceFinish.Text = dr["已完成"].ToString();
                        lblServiceUnFinish.Text = dr["未完成"].ToString();
                        lblServiceOverFinish.Text = dr["超时完成"].ToString();
                        lblServiceOverUnFinish.Text = dr["超时未完成"].ToString();
                        lblServiceOver.Text = dr["超时事件率"].ToString();
                        lblServic.Text = (int.Parse(dr["已完成"].ToString()) + int.Parse(dr["未完成"].ToString())).ToString();
                        break;
                    case "320":    //投诉单
                        lblBytsFinish.Text = dr["已完成"].ToString();
                        lblBytsUnFinish.Text = dr["未完成"].ToString();
                        lblBytsOverFinish.Text = dr["超时完成"].ToString();
                        lblBytsOverUnFinish.Text = dr["超时未完成"].ToString();
                        lblBytsOver.Text = dr["超时事件率"].ToString();
                        lblByts.Text = (int.Parse(dr["已完成"].ToString()) + int.Parse(dr["未完成"].ToString())).ToString();
                        break;
                    case "210":    //问题单
                        lblProblemFinish.Text = dr["已完成"].ToString();
                        lblProblemUnFinish.Text = dr["未完成"].ToString();
                        lblProblemOverFinish.Text = dr["超时完成"].ToString();
                        lblProblemOverUnFinish.Text = dr["超时未完成"].ToString();
                        lblProblemOver.Text = dr["超时事件率"].ToString();
                        lblProblem.Text = (int.Parse(dr["已完成"].ToString()) + int.Parse(dr["未完成"].ToString())).ToString();
                        break;
                    case "420":    //变更单
                        lblChangeFinish.Text = dr["已完成"].ToString();
                        lblChangeUnFinish.Text = dr["未完成"].ToString();
                        lblChangeOverFinish.Text = dr["超时完成"].ToString();
                        lblChangeOverUnFinish.Text = dr["超时未完成"].ToString();
                        lblChangeOver.Text = dr["超时事件率"].ToString();
                        lblChange.Text = (int.Parse(dr["已完成"].ToString()) + int.Parse(dr["未完成"].ToString())).ToString();
                        break;
                    case "410":    //巡检单
                        lblPatrolFinish.Text = dr["已完成"].ToString();
                        lblPatrolUnFinish.Text = dr["未完成"].ToString();
                        lblPatrolOverFinish.Text = dr["超时完成"].ToString();
                        lblPatrolOverUnFinish.Text = dr["超时未完成"].ToString();
                        lblPatrolOver.Text = dr["超时事件率"].ToString();
                        lblPatrol.Text = (int.Parse(dr["已完成"].ToString()) + int.Parse(dr["未完成"].ToString())).ToString();
                        break;
                    case "400":    //知识单
                        lblInforFinish.Text = dr["已完成"].ToString();
                        lblInforUnFinish.Text = dr["未完成"].ToString();
                        lblInforOverFinish.Text = dr["超时完成"].ToString();
                        lblInforOverUnFinish.Text = dr["超时未完成"].ToString();
                        lblInforOver.Text = dr["超时事件率"].ToString();
                        lblInfor.Text = (int.Parse(dr["已完成"].ToString()) + int.Parse(dr["未完成"].ToString())).ToString();
                        break;
                    default:
                        break;
                }
            }

            sSql = string.Empty;
            sSql = @"select to_char(to_number(sum(case FeedType when 1 then 1 else 0 end))/to_number(count(FeedBackID))*100,18,2)+'%' 满意度,
                        count(FeedBackID) 总数,a.AppID   
                        from EA_Issues_FeedBack a,es_flow b where a.FlowID=b.FlowID";
            sSql += sWhere + " group by a.AppID";
            dt = Epower.ITSM.SqlDAL.CommonDP.ExcuteSqlTable(sSql);
            foreach (DataRow dr in dt.Rows)
            {
                string sAppID = dr["AppID"].ToString();
                switch (sAppID)
                {
                    case "1026":   //服务单
                        lblServicFeedBack.Text = dr["总数"].ToString();
                        lblServicePlease.Text = dr["满意度"].ToString();
                        break;
                    case "320":       //投诉单
                        lblBytsFeedBack.Text = dr["总数"].ToString();
                        lblBytsPlease.Text = dr["满意度"].ToString();
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitPage()
        {
            lblServiceFinish.Text = "0";
            lblServiceUnFinish.Text = "0";
            lblServiceOverFinish.Text = "0";
            lblServiceOverUnFinish.Text = "0";
            lblServiceOver.Text = "0.00%";
            lblServic.Text = "0";

            lblBytsFinish.Text = "0";
            lblBytsUnFinish.Text = "0";
            lblBytsOverFinish.Text = "0";
            lblBytsOverUnFinish.Text = "0";
            lblBytsOver.Text = "0.00%";
            lblByts.Text = "0";

            lblProblemFinish.Text = "0";
            lblProblemUnFinish.Text = "0";
            lblProblemOverFinish.Text = "0";
            lblProblemOverUnFinish.Text = "0";
            lblProblemOver.Text = "0.00%";
            lblProblem.Text = "0";

            lblChangeFinish.Text = "0";
            lblChangeUnFinish.Text = "0";
            lblChangeOverFinish.Text = "0";
            lblChangeOverUnFinish.Text = "0";
            lblChangeOver.Text = "0.00%";
            lblChange.Text = "0";

            lblPatrolFinish.Text = "0";
            lblPatrolUnFinish.Text = "0";
            lblPatrolOverFinish.Text = "0";
            lblPatrolOverUnFinish.Text = "0";
            lblPatrolOver.Text = "0.00%";
            lblPatrol.Text = "0";

            lblInforFinish.Text = "0";
            lblInforUnFinish.Text = "0";
            lblInforOverFinish.Text = "0";
            lblInforOverUnFinish.Text = "0";
            lblInforOver.Text = "0.00%";
            lblInfor.Text = "0";

            lblServicFeedBack.Text = "0";
            lblServicePlease.Text = "0.00%";
            lblBytsFeedBack.Text = "0";
            lblBytsPlease.Text = "0.00%";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOk_Click(object sender, EventArgs e)
        {
            LoadServiceData();
        }
    }
}
