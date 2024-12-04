/*******************************************************************
 *
 * Description:������ҳ��
 * 
 * 
 * Create By  :zhumc
 * Create Date:2008��4��5��
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
            sSql = @"select sum(case Status when 30 then 1 else 0 end) �����,
                           sum(case Status when 20 then 1 else 0 end) δ���,
                           sum(case when ExpectEndTime is null then 0 when Status=30 and EndTime>ExpectEndTime then 1 end) ��ʱ���,
                           sum(case when ExpectEndTime is null then 0 when Status=20 and sysdate>ExpectEndTime then 1 end) ��ʱδ���,
                           to_char(to_number(sum(case when ExpectEndTime is null then 0 when nvl(EndTime,sysdate)>ExpectEndTime then 1 end))/
	                    count(FlowID)*100,18,2)+'%' ��ʱ�¼���,
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
                    case "1026":   //����
                        lblServiceFinish.Text = dr["�����"].ToString();
                        lblServiceUnFinish.Text = dr["δ���"].ToString();
                        lblServiceOverFinish.Text = dr["��ʱ���"].ToString();
                        lblServiceOverUnFinish.Text = dr["��ʱδ���"].ToString();
                        lblServiceOver.Text = dr["��ʱ�¼���"].ToString();
                        lblServic.Text = (int.Parse(dr["�����"].ToString()) + int.Parse(dr["δ���"].ToString())).ToString();
                        break;
                    case "320":    //Ͷ�ߵ�
                        lblBytsFinish.Text = dr["�����"].ToString();
                        lblBytsUnFinish.Text = dr["δ���"].ToString();
                        lblBytsOverFinish.Text = dr["��ʱ���"].ToString();
                        lblBytsOverUnFinish.Text = dr["��ʱδ���"].ToString();
                        lblBytsOver.Text = dr["��ʱ�¼���"].ToString();
                        lblByts.Text = (int.Parse(dr["�����"].ToString()) + int.Parse(dr["δ���"].ToString())).ToString();
                        break;
                    case "210":    //���ⵥ
                        lblProblemFinish.Text = dr["�����"].ToString();
                        lblProblemUnFinish.Text = dr["δ���"].ToString();
                        lblProblemOverFinish.Text = dr["��ʱ���"].ToString();
                        lblProblemOverUnFinish.Text = dr["��ʱδ���"].ToString();
                        lblProblemOver.Text = dr["��ʱ�¼���"].ToString();
                        lblProblem.Text = (int.Parse(dr["�����"].ToString()) + int.Parse(dr["δ���"].ToString())).ToString();
                        break;
                    case "420":    //�����
                        lblChangeFinish.Text = dr["�����"].ToString();
                        lblChangeUnFinish.Text = dr["δ���"].ToString();
                        lblChangeOverFinish.Text = dr["��ʱ���"].ToString();
                        lblChangeOverUnFinish.Text = dr["��ʱδ���"].ToString();
                        lblChangeOver.Text = dr["��ʱ�¼���"].ToString();
                        lblChange.Text = (int.Parse(dr["�����"].ToString()) + int.Parse(dr["δ���"].ToString())).ToString();
                        break;
                    case "410":    //Ѳ�쵥
                        lblPatrolFinish.Text = dr["�����"].ToString();
                        lblPatrolUnFinish.Text = dr["δ���"].ToString();
                        lblPatrolOverFinish.Text = dr["��ʱ���"].ToString();
                        lblPatrolOverUnFinish.Text = dr["��ʱδ���"].ToString();
                        lblPatrolOver.Text = dr["��ʱ�¼���"].ToString();
                        lblPatrol.Text = (int.Parse(dr["�����"].ToString()) + int.Parse(dr["δ���"].ToString())).ToString();
                        break;
                    case "400":    //֪ʶ��
                        lblInforFinish.Text = dr["�����"].ToString();
                        lblInforUnFinish.Text = dr["δ���"].ToString();
                        lblInforOverFinish.Text = dr["��ʱ���"].ToString();
                        lblInforOverUnFinish.Text = dr["��ʱδ���"].ToString();
                        lblInforOver.Text = dr["��ʱ�¼���"].ToString();
                        lblInfor.Text = (int.Parse(dr["�����"].ToString()) + int.Parse(dr["δ���"].ToString())).ToString();
                        break;
                    default:
                        break;
                }
            }

            sSql = string.Empty;
            sSql = @"select to_char(to_number(sum(case FeedType when 1 then 1 else 0 end))/to_number(count(FeedBackID))*100,18,2)+'%' �����,
                        count(FeedBackID) ����,a.AppID   
                        from EA_Issues_FeedBack a,es_flow b where a.FlowID=b.FlowID";
            sSql += sWhere + " group by a.AppID";
            dt = Epower.ITSM.SqlDAL.CommonDP.ExcuteSqlTable(sSql);
            foreach (DataRow dr in dt.Rows)
            {
                string sAppID = dr["AppID"].ToString();
                switch (sAppID)
                {
                    case "1026":   //����
                        lblServicFeedBack.Text = dr["����"].ToString();
                        lblServicePlease.Text = dr["�����"].ToString();
                        break;
                    case "320":       //Ͷ�ߵ�
                        lblBytsFeedBack.Text = dr["����"].ToString();
                        lblBytsPlease.Text = dr["�����"].ToString();
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
