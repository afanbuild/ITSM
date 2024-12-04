/****************************************************************************
 * 
 * description:变更管理主页面
 * 
 * 
 * 
 * Create by:zhumingchun
 * Create Date:2008-04-04
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
using System.Xml;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.AppForms
{
    /// <summary>
    /// frm_ChangeQuery
    /// </summary>
    public partial class frmFlow_QuestHouseQuery : BasePage
    {
        RightEntity re = null;

        #region SetParentButtonEvent
        /// <summary>
        /// 设置父窗体按钮事件
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.queyhouse;
            this.Master.IsCheckRight = true;
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.Master_Button_ExportExcel_Click += new Global_BtnClick(Master_Master_Button_ExportExcel_Click);
            this.Master.ShowQueryButton(true);
            this.Master.ShowNewButton(true);
            this.Master.ShowExportExcelButton(true);
        }
        #endregion

        #region Master_Master_Button_Query_Click
        /// <summary>
        /// 查询事件
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            LoadData();
        }
        #endregion

        #region 导出EXCEL事件Master_Master_Button_ExportExcel_Click
        /// <summary>
        /// 查询事件
        /// </summary>
        void Master_Master_Button_ExportExcel_Click()
        {
            LoadData();
            DataTable dt = ExportExcel();
            Epower.ITSM.Web.Common.ExcelExport.ExportQuesHouse(this, dt, Session["UserID"].ToString());
        }
        #endregion

        #region Master_Master_Button_New_Click申请
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("~/Forms/form_all_flowmodel.aspx?appid=1027");
        }
        #endregion

        #region Page_Load
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtonEvent();
            cpChange.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(LoadData);
            if (!IsPostBack)
            {
                //设置显示
                PageDeal.SetLanguage(this.Controls[0]);
                SetHeaderText();

                //快速搜索
                if (Request["svalue"] != null)
                {
                    txtCustInfo.Text = Request["svalue"].ToString().Trim();
                }

                re = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.EquChangeQuery];

                cboStatus.Items.Add(new ListItem("所有状态", "-1"));
                cboStatus.Items.Add(new ListItem("--正在处理", ((int)EpowerGlobal.e_FlowStatus.efsHandle).ToString()));
                cboStatus.Items.Add(new ListItem("--正常结束", ((int)EpowerGlobal.e_FlowStatus.efsEnd).ToString()));
                cboStatus.Items.Add(new ListItem("--流程暂停", ((int)EpowerGlobal.e_FlowStatus.efsStop).ToString()));
                cboStatus.Items.Add(new ListItem("--流程终止", ((int)EpowerGlobal.e_FlowStatus.efsAbort).ToString()));
                cboStatus.SelectedIndex = 1;
                //设置起始日期
                string sQueryBeginDate = string.Empty;
                sQueryBeginDate = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-01";
                ctrDateTime.BeginTime = DateTime.Parse(sQueryBeginDate).ToString("yyyy-MM-dd");
                ctrDateTime.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                LoadData();
                Session["FromUrl"] = "../QuestHouse/frmFlow_QuestHouseQuery.aspx";

                //应用管理员删除权限
                dgProblem.Columns[dgProblem.Columns.Count - 1].Visible = CheckRight(Constant.admindeleteflow);
            }
        }
        #endregion

        #region 设置datagrid标头显示 余向前 2013-05-17
        /// <summary>
        /// 设置datagrid标头显示
        /// </summary>
        private void SetHeaderText()
        {
            dgProblem.Columns[2].HeaderText = PageDeal.GetLanguageValue("Quest_ITILNO");
            dgProblem.Columns[3].HeaderText = PageDeal.GetLanguageValue("Quest_execbyno");
            dgProblem.Columns[4].HeaderText = PageDeal.GetLanguageValue("Quest_execbyname");
            dgProblem.Columns[5].HeaderText = PageDeal.GetLanguageValue("Quest_execbydeptname");
            dgProblem.Columns[6].HeaderText = PageDeal.GetLanguageValue("Quest_comeindate");
            dgProblem.Columns[7].HeaderText = PageDeal.GetLanguageValue("Quest_outdate");
        }
        #endregion

        #region LoadData
        /// <summary>
        /// 数据加载
        /// </summary>
        private void LoadData()
        {
            int iRowCount = 0;
            string sWhere = "";
            string strBeginDate = ctrDateTime.BeginTime;
            string strEndDate = ctrDateTime.EndTime;
            if (cboStatus.SelectedItem.Value.Trim() != "-1")
                sWhere += " and status= " + cboStatus.SelectedItem.Value.ToString();
            if (!string.IsNullOrEmpty(txtCustInfo.Text.Trim()))
            {
                sWhere += " and ITILNO like " + StringTool.SqlQ("%" + txtCustInfo.Text.Trim() + "%");
            }
            if (strBeginDate.Trim() != "")
                sWhere += " And createdate>=to_date(" + StringTool.SqlQ(strBeginDate) + ",'yyyy-MM-dd HH24:mi:ss')";
            if (strEndDate.Trim() != "")
                sWhere += " And createdate <=to_date(" + StringTool.SqlQ(DateTime.Parse(strEndDate).ToString("yyyy-MM-dd") + " 23:59:59") + ",'yyyy-MM-dd HH24:mi:ss')";
            if (ddlStatus.SelectedItem.Value != "0")
                sWhere += " and statusid=" + ddlStatus.SelectedItem.Value;

            long lngUserID = long.Parse(Session["UserID"].ToString().Trim());
            long lngDeptID = long.Parse(Session["UserDeptID"].ToString().Trim());
            long lngOrgID = long.Parse(Session["UserOrgID"].ToString().Trim());
            //获得权限
            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.queyhouse];
            DataTable dt;
            dt = Flow_QuestHouse.getQuestHouse(lngUserID, lngDeptID, lngOrgID, reTrace, sWhere, this.cpChange.PageSize, this.cpChange.CurrentPage, ref iRowCount);
            dgProblem.DataSource = dt.DefaultView;
            dgProblem.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");
            dgProblem.DataBind();
            this.cpChange.RecordCount = iRowCount;
            this.cpChange.Bind();
        }
        #endregion

        #region ExportExcel

        /// <summary>
        /// Excel导出
        /// </summary>
        /// <returns></returns>
        private DataTable ExportExcel()
        {
            DataTable dt = null;
            string sWhere = "";
            string strBeginDate = ctrDateTime.BeginTime;
            string strEndDate = ctrDateTime.EndTime;
            if (cboStatus.SelectedItem.Value.Trim() != "-1")
                sWhere += " and status= " + cboStatus.SelectedItem.Value.ToString();
            if (!string.IsNullOrEmpty(txtCustInfo.Text.Trim()))
            {
                sWhere += " and ITILNO like " + StringTool.SqlQ("%" + txtCustInfo.Text.Trim() + "%");
            }

            if (strBeginDate.Trim() != "")
                sWhere += " And createdate>= to_date(" + StringTool.SqlQ(strBeginDate) + ",'yyyy-MM-dd HH24:mi:ss')";
            if (strEndDate.Trim() != "")
                sWhere += " And createdate <= to_date(" + StringTool.SqlQ(DateTime.Parse(strEndDate).ToString("yyyy-MM-dd") + " 23:59:59") + ",'yyyy-MM-dd HH24:mi:ss')";
            if (ddlStatus.SelectedItem.Value != "0")
                sWhere += " and statusid=" + ddlStatus.SelectedItem.Value;

            long lngUserID = long.Parse(Session["UserID"].ToString().Trim());
            long lngDeptID = long.Parse(Session["UserDeptID"].ToString().Trim());
            long lngOrgID = long.Parse(Session["UserOrgID"].ToString().Trim());
            //获得权限
            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.queyhouse];
            dt = Flow_QuestHouse.getQuestHouse(lngUserID, lngDeptID, lngOrgID, reTrace, sWhere);
            return dt;
        }

        #endregion

        #region 显示页面地址
        /// <summary>
        /// 显示页面地址
        /// </summary>
        /// <param name="lngNoticeID"></param>
        /// <returns></returns>
        protected string GetUrl(decimal lngFlowID)
        {
            string sUrl = "";
            sUrl = "javascript:window.open('../Forms/frmIssueView.aspx?FlowID=" + lngFlowID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
            return sUrl;
        }

        #endregion

        #region dgProblem_ItemCreated
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgProblem_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count - 5; i++)
                {
                    int j = i - 2;  //前面有6个隐藏的列
                    e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                }
            }
        }
        #endregion

        #region 检查权限 CheckRight
        /// <summary>
        /// 
        /// </summary>
        /// <param name="OperatorID"></param>
        /// <returns></returns>
        private bool CheckRight(long OperatorID)
        {
            RightEntity re = (RightEntity)((Hashtable)Session["UserAllRights"])[OperatorID];
            if (re == null)
                return false;
            else
                return re.CanRead;
        }
        #endregion

        #region 删除流程dgProblem_DeleteCommand
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgProblem_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            LoadData();
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgProblem_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //规范鼠标动作--ly
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                String sID = DataBinder.Eval(e.Item.DataItem, "FlowID").ToString();
                e.Item.Attributes.Add("ondblclick", "window.open('../Forms/frmIssueView.aspx?FlowID=" + sID.ToString() + "&randomid='+GetRandom(),'MainFrame','scrollbars=yes,resizable=yes')");

            }
        }

        protected void hidd_btnDelete_Click(object sender, EventArgs e)
        {
            LoadData();
        }

    }
}
