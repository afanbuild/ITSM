/*******************************************************************
 * 版权所有：
 * Description：变更单高级查询页面
 * Create By  ：SuperMan
 * Create Date：2011-08-22
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
using System.Xml;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;
using Epower.DevBase.BaseTools;
using System.Data.OracleClient;

namespace Epower.ITSM.Web.EquipmentManager
{
    /// <summary>
    /// frm_ChangeQuery
    /// </summary>
    public partial class frm_ChangeQuery : BasePage
    {
        #region 自定义变量

        RightEntity re = null;
        static string staMsgDateBegion = "";
        static string staCustInfo = "";
        static int icurrent = 0;   //1.高级查询 2.查询事件 3.页面第一次加载
        #endregion

        #region SetParentButtonEvent
        /// <summary>
        /// 设置父窗体按钮事件
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.Master_Button_ExportExcel_Click += new Global_BtnClick(Master_Master_Button_ExportExcel_Click);
            this.Master.ShowQueryButton(true);
            this.Master.TxtKeyName.Visible = true;
            this.Master.ShowNewButton(false);
            this.Master.ShowExportExcelButton(true);

        }
        #endregion

        #region Master_Master_Button_Query_Click
        /// <summary>
        /// 查询事件---
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            icurrent = 2;
            LoadData();
            DropSQLwSave.SelectedIndex = 0;

            this.ctrCondition.SetDisplayMode = false;
        }
        #endregion

        #region 导出EXCEL事件Master_Master_Button_ExportExcel_Click
        /// <summary>
        /// 查询事件
        /// </summary>
        void Master_Master_Button_ExportExcel_Click()
        {
            string[] key = null;
            string[] value = null;//, "InstancyName""紧急度",
            DataTable dt = GetDataTable();
            string[] arrField = { "ChangeNo", "ChangeTypeName", "RegTime", "MastCustName", "CustName", "CTel", "CustAddress", "Subject", "Content", "EffectName", "LevelName", "DealStatus", "ChangeAnalyses", "ChangeAnalysesResult", "ChangeTime", "DealStatus", "TPersonList",
                                "isplanchange", "ChangeNeedPeople", "Isplan","CHANGE_PLACE_NAME", "PLAN_BEGIN_TIME", "PLAN_END_TIME", "isbuseffect", "BUS_EFFECT", "isdataeffect", "DATA_EFFECT", "CHANGE_WINDOW_NAME", "isstopserver", "STOP_SERVER_REMARK","real_begin_time","real_end_time"
                                };
            string[] fileName = { "变更单号", "变更类别", "登记时间", "服务单位", "客户名称", "办公电话", "客户地址", "摘要", "请求内容", "影响度", "变更级别", "变更状态", "变更分析", "分析结果", "变更时间", "处理状态", "审批人",
                                "是否计划性变更","变更需求人","应急回退方案","变更场所","计划开始时间","计划完成时间","是否业务影响","业务影响说明","是否数据影响","数据影响说明","变更窗口","是否停用服务","停用服务说明","实际开始时间","实际结束时间"
                                };

            //string s = "变更单号";
            //for (int i = 1; i < arrField.Length; i++)
            //{
            //    s += "," + PageDeal.GetLanguageValue1(arrField[i], "变更单");
            //}
            //fileName = s.Split(',');
            //if (!string.IsNullOrEmpty(this.HiddenColumn.Value))
            //{
            //    string[] columnValues = this.HiddenColumn.Value.Split(',');
            //    if (columnValues.Length > 1)
            //    {
            //        string k = "ChangeNo";
            //        for (int i = 0; i < columnValues.Length - 1; i++)
            //        {
            //            k += "," + columnValues[i];
            //        }
            //        key = k.Split(',');
            //        string v = "变更单号";
            //        for (int i = 1; i < key.Length; i++)
            //        {
            //            v += "," + PageDeal.GetLanguageValue1(key[i], "变更单");
            //        }
            //        value = v.Split(',');
            //    }
            //}

            //取得审批人列表
            string strPersonList = string.Empty;
            DataColumn dtcl = new DataColumn("TPersonList");
            dt.Columns.Add(dtcl);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                strPersonList = ChangeDealDP.GetPersonList(long.Parse(dt.Rows[i]["FlowID"].ToString()));

                dt.Rows[i].SetModified();
                dt.Rows[i]["TPersonList"] = strPersonList;
            }
            if (!string.IsNullOrEmpty(this.HiddenColumn.Value))
            {
                Epower.ITSM.Web.Common.ExcelExport.ExportChangeList(this, dt, value, key, Session["UserID"].ToString());
            }
            else
            {
                Epower.ITSM.Web.Common.ExcelExport.ExportChangeList(this, dt, fileName, arrField, Session["UserID"].ToString());
            }

            //Epower.ITSM.Web.Common.ExcelExport.ExportChangeList(this, dt, Session["UserID"].ToString());
        }
        #endregion

        #region Master_Master_Button_New_Click申请
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("~/Forms/form_all_flowmodel.aspx?appid=420");
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

            //应用管理员删除权限
            dgProblem.Columns[dgProblem.Columns.Count - 1].Visible = CheckRight(Constant.admindeleteflow);

            if (!IsPostBack)
            {

                icurrent = 3;
                PageLoadQuery();

                SetHeaderText();
            }

            #region 动态查询: 设置动态查询参数 - 2013-04-01 @孙绍棕

            this.ctrCondition.TableName = "Equ_ChangeService";
            this.ctrCondition.mybtnSelectOnClick += new EventHandler(ctrCondition1_mybtnSelectOnClick);
            this.ctrCondition.mySelectedIndexChanged += new EventHandler(ctrCondition1_mySelectedIndexChanged);

            #endregion
        }
        #endregion

        #region 动态查询: 点击动态查询按钮时触发 - 2013-04-01 @孙绍棕

        /// <summary>
        /// 点击动态查询按钮时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ctrCondition1_mybtnSelectOnClick(object sender, EventArgs e)
        {
            ctrCondition.Bind();
            icurrent = 1;    // 高级查询
            LoadData();

            this.Master.TxtKeyName.Value = "请输入变更单号,客户信息";
        }

        #endregion

        #region 动态查询: 下拉框选择不同的查询条件组合时触发 - 2013-04-01 @孙绍棕

        /// <summary>
        /// 下拉框选择不同的查询条件组合时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ctrCondition1_mySelectedIndexChanged(object sender, EventArgs e)
        {
            //this.TypeID = __ADVANCED_SEARCH;
            ctrCondition.ddlSelectChanged();

            //LoadData(__ADVANCED_SEARCH);
            icurrent = 1;    // 高级查询
            LoadData();

            this.Master.TxtKeyName.Value = "请输入变更单号,客户信息";
        }

        #endregion

        #region 设置datagrid标头显示 余向前 2013-05-17
        /// <summary>
        /// 设置datagrid标头显示
        /// </summary>
        private void SetHeaderText()
        {
            dgProblem.Columns[2].HeaderText = PageDeal.GetLanguageValue("Change_ChangeNo");
            dgProblem.Columns[3].HeaderText = PageDeal.GetLanguageValue("Change_CustName");
            dgProblem.Columns[4].HeaderText = PageDeal.GetLanguageValue("Change_CustContract");
            dgProblem.Columns[5].HeaderText = PageDeal.GetLanguageValue("Change_InstancyName");
            dgProblem.Columns[6].HeaderText = PageDeal.GetLanguageValue("Change_EffectName");
            dgProblem.Columns[7].HeaderText = PageDeal.GetLanguageValue("Change_ChangeTime");
            dgProblem.Columns[8].HeaderText = PageDeal.GetLanguageValue("Change_DealStatus");
        }
        #endregion

        #region LoadData

        /// <summary>
        /// 取最近一个月未处理的流程单
        /// </summary>
        /// <param name="iRowCount">总行数</param>
        /// <returns></returns>
        private DataTable GetRecentlyMonthData(ref Int32 iRowCount)
        {
            long lngUserID = (long)Session["UserID"];
            long lngDeptID = (long)Session["UserDeptID"];
            long lngOrgID = (long)Session["UserOrgID"];
            //获得权限
            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.EquChangeQuery];

            String sWhere = String.Empty;
            sWhere += " and ChangeTime>= to_date(" + StringTool.SqlQ(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd")) + ",'yyyy-MM-dd HH24:mi:ss')";
            sWhere += " and ChangeTime<= to_date(" + StringTool.SqlQ(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59") + ",'yyyy-MM-dd HH24:mi:ss')";
            sWhere += " and status=20 ";

            DataTable dt = ChangeDealDP.GetChangeDealDataForCustInfo(lngUserID, lngDeptID, lngOrgID, reTrace, sWhere, this.cpChange.PageSize, this.cpChange.CurrentPage, ref iRowCount);

            return dt;
        }

        /// <summary>
        /// [导出Excel] 取最近一个月未处理的流程单
        /// </summary>
        /// <param name="iRowCount">总行数</param>
        /// <returns></returns>
        private DataTable GetRecentlyMonthDataForExportExcel()
        {
            long lngUserID = (long)Session["UserID"];
            long lngDeptID = (long)Session["UserDeptID"];
            long lngOrgID = (long)Session["UserOrgID"];
            //获得权限
            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.EquChangeQuery];

            String sWhere = String.Empty;
            sWhere += " and ChangeTime>= to_date(" + StringTool.SqlQ(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd")) + ",'yyyy-MM-dd HH24:mi:ss')";
            sWhere += " and ChangeTime<= to_date(" + StringTool.SqlQ(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59") + ",'yyyy-MM-dd HH24:mi:ss')";
            sWhere += " and status=20 ";

            DataTable dt = ChangeDealDP.GetChangeDealDataForCustInfo(lngUserID, lngDeptID, lngOrgID, reTrace, sWhere);

            return dt;
        }

        /// <summary>
        /// 取快速查询后的流程单
        /// </summary>
        /// <param name="iRowCount">总行数</param>
        /// <returns></returns>
        private DataTable GetQuicklySearchData(ref Int32 iRowCount)
        {
            long lngUserID = (long)Session["UserID"];
            long lngDeptID = (long)Session["UserDeptID"];
            long lngOrgID = (long)Session["UserOrgID"];
            //获得权限
            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.EquChangeQuery];

            String sWhere = "";
            String strCustInfo = this.Master.TxtKeyName.Value.Trim().ToString();

            if (strCustInfo != "请输入变更单号,客户信息")
            {
                string sSqlWhere1 = @"select ID from Br_ECustomer where 1=1";

                sSqlWhere1 += " And (";
                sSqlWhere1 += " CustName like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere1 += " OR Contact like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere1 += " OR customcode like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere1 += " OR CustAddress like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere1 += " OR mastCustname like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere1 += " OR Email like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere1 += " OR CTel like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere1 += " ) ";
                sWhere += " AND (custid IN (" + sSqlWhere1 + ")";

                sWhere += "  or  ChangeNo like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%") + ")";
            }

            DataTable dt = ChangeDealDP.GetChangeDealDataForCustInfo(lngUserID, lngDeptID, lngOrgID, reTrace, sWhere, this.cpChange.PageSize, this.cpChange.CurrentPage, ref iRowCount);

            return dt;
        }

        /// <summary>
        /// [导出Excel] 取快速查询后的流程单
        /// </summary>
        /// <param name="iRowCount">总行数</param>
        /// <returns></returns>
        private DataTable GetQuicklySearchDataForExportExcel()
        {
            long lngUserID = (long)Session["UserID"];
            long lngDeptID = (long)Session["UserDeptID"];
            long lngOrgID = (long)Session["UserOrgID"];
            //获得权限
            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.EquChangeQuery];

            String sWhere = "";
            String strCustInfo = this.Master.TxtKeyName.Value.Trim().ToString();

            if (strCustInfo != "请输入变更单号,客户信息")
            {
                string sSqlWhere1 = @"select ID from Br_ECustomer where 1=1";

                sSqlWhere1 += " And (";
                sSqlWhere1 += " CustName like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere1 += " OR Contact like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere1 += " OR customcode like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere1 += " OR CustAddress like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere1 += " OR mastCustname like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere1 += " OR Email like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere1 += " OR CTel like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere1 += " ) ";
                sWhere += " AND (custid IN (" + sSqlWhere1 + ")";

                sWhere += "  or  ChangeNo like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%") + ")";
            }

            DataTable dt = ChangeDealDP.GetChangeDealDataForCustInfo(lngUserID, lngDeptID, lngOrgID, reTrace, sWhere);

            return dt;
        }

        /// <summary>
        /// 取高级查询后的流程单
        /// </summary>
        /// <param name="iRowCount">总行数</param>
        /// <returns></returns>
        private DataTable GetAdvancedSearchData(ref Int32 iRowCount)
        {
            long lngUserID = (long)Session["UserID"];
            long lngDeptID = (long)Session["UserDeptID"];
            long lngOrgID = (long)Session["UserOrgID"];
            //获得权限
            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.EquChangeQuery];

            String sWhere = "";
            if (!String.IsNullOrEmpty(this.ctrCondition.strCondition.Trim()))
            {
                sWhere = " AND " + this.ctrCondition.strCondition;
            }


            DataTable dt = ChangeDealDP.GetChangeDealDataForCustInfo(lngUserID, lngDeptID, lngOrgID, reTrace, sWhere, this.cpChange.PageSize, this.cpChange.CurrentPage, ref iRowCount);

            return dt;
        }

        /// <summary>
        /// [导出Excel] 取高级查询后的流程单
        /// </summary>
        /// <param name="iRowCount">总行数</param>
        /// <returns></returns>
        private DataTable GetAdvancedSearchDataForExportExcel()
        {
            long lngUserID = (long)Session["UserID"];
            long lngDeptID = (long)Session["UserDeptID"];
            long lngOrgID = (long)Session["UserOrgID"];
            //获得权限
            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.EquChangeQuery];

            String sWhere = "";
            if (!String.IsNullOrEmpty(this.ctrCondition.strCondition.Trim()))
            {
                sWhere = " AND " + this.ctrCondition.strCondition;
            }


            DataTable dt = ChangeDealDP.GetChangeDealDataForCustInfo(lngUserID, lngDeptID, lngOrgID, reTrace, sWhere);

            return dt;
        }

        /// <summary>
        /// 加载显示流程单列表
        /// </summary>
        private void LoadData()
        {
            Int32 iRowCount = 0;

            DataTable dt = null;
            switch (icurrent)
            {
                case 1:    // 高级搜索
                    dt = GetAdvancedSearchData(ref iRowCount);
                    break;

                case 2:    // 快速搜索
                    dt = GetQuicklySearchData(ref iRowCount);
                    break;

                case 3:    // 最近一个月未处理的流程单
                    dt = GetRecentlyMonthData(ref iRowCount);
                    break;
            }

            dgProblem.DataSource = dt.DefaultView;
            dgProblem.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");
            dgProblem.DataBind();
            this.cpChange.RecordCount = iRowCount;
            this.cpChange.Bind();
        }

        /// <summary>
        /// 数据加载
        /// </summary>
        private void LoadData2()
        {
            int iRowCount = 0;
            string sWhere = "";

            sWhere = LoadDataPre();

            //初次加载时只加载一个月的数据
            if (icurrent == 3)
            {
                sWhere = "";
                sWhere += " and ChangeTime>= to_date(" + StringTool.SqlQ(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd")) + ",'yyyy-MM-dd HH24:mi:ss')";
                sWhere += " and ChangeTime<= to_date(" + StringTool.SqlQ(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59") + ",'yyyy-MM-dd HH24:mi:ss')";
                sWhere += " and status=20 ";

                hidSQLName.Value = "Temp1";
                SqlWhereShow("Temp1");
                this.DropSQLwSave.SelectedIndex = DropSQLwSave.Items.IndexOf(DropSQLwSave.Items.FindByValue("0"));
            }

            long lngUserID = (long)Session["UserID"];
            long lngDeptID = (long)Session["UserDeptID"];
            long lngOrgID = (long)Session["UserOrgID"];
            //获得权限
            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.EquChangeQuery];
            DataTable dt;
            dt = ChangeDealDP.GetChangeDealDataForCustInfo(lngUserID, lngDeptID, lngOrgID, reTrace, sWhere, this.cpChange.PageSize, this.cpChange.CurrentPage, ref iRowCount);

            //设置显示字段
            if (!string.IsNullOrEmpty(this.HiddenColumn.Value))
            {
                SetColumnDisplay(this.HiddenColumn.Value);
            }

            dgProblem.DataSource = dt.DefaultView;
            dgProblem.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");
            dgProblem.DataBind();
            this.cpChange.RecordCount = iRowCount;
            this.cpChange.Bind();
        }

        /// <summary>
        /// 获取页面数据
        /// </summary>
        /// <returns></returns>
        private DataTable GetDataTable()
        {
            DataTable dt = null;
            switch (icurrent)
            {
                case 1:    // 高级搜索
                    dt = GetAdvancedSearchDataForExportExcel();
                    break;

                case 2:    // 快速搜索
                    dt = GetQuicklySearchDataForExportExcel();
                    break;

                case 3:    // 最近一个月未处理的流程单
                    dt = GetRecentlyMonthDataForExportExcel();
                    break;
            }

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

        #region 窗体事件

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

                ((Label)e.Item.FindControl("lblChangeNo")).Attributes.Add("onmouseover", "ShowDetailsInfo(this," + DataBinder.Eval(e.Item.DataItem, "ID").ToString() + ",400);");
            }
        }

        protected void DropSQLwSave_SelectedIndexChanged(object sender, EventArgs e)
        {
            icurrent = 1;
            string xx = DropSQLwSave.SelectedItem.Text;

            if (DropSQLwSave.SelectedItem.Text != "==选择收藏查询条件==")
            {
                SqlWhereShow(DropSQLwSave.SelectedItem.Text.ToString());
                LoadData();
                hidSQLName.Value = DropSQLwSave.SelectedItem.Text.ToString();

                //更新访问次数
                ChangeDealDP.updateCST_ISSUE_WhereNums(Session["UserName"].ToString(), DropSQLwSave.SelectedItem.Text.ToString());

                hidSQLName.Value = DropSQLwSave.SelectedItem.Text.ToString();
            }
            else
            {
                SqlWhereShow("Temp1");
                hidSQLName.Value = "Temp1";
                LoadData();
            }
            this.Master.KeyValue = "请输入变更单号,客户信息";
        }

        protected void btn_addnew_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Forms/form_all_flowmodel.aspx?appid=420");
        }

        protected void btn_excel_Click(object sender, EventArgs e)
        {

            DataTable dt = GetDataTable();
            //取得审批人列表
            string strPersonList = string.Empty;
            DataColumn dtcl = new DataColumn("TPersonList");
            dt.Columns.Add(dtcl);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                strPersonList = ChangeDealDP.GetPersonList(long.Parse(dt.Rows[i]["FlowID"].ToString()));

                dt.Rows[i].SetModified();
                dt.Rows[i]["TPersonList"] = strPersonList;
            }

            Epower.ITSM.Web.Common.ExcelExport.ExportChangeList(this, dt, Session["UserID"].ToString());
        }

        protected void btn_query_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        #endregion

        #region 自定义方法
        private void InitDropSQLwSave()
        {
            DataTable dt = ChangeDealDP.getCST_ISSUE_LISTFASTQUERY("frm_ChangeQuery", Session["UserName"].ToString(), string.Empty);
            DropSQLwSave.Items.Clear();
            DropSQLwSave.DataSource = dt.DefaultView;
            DropSQLwSave.DataTextField = "Name";
            DropSQLwSave.DataValueField = "ID";
            DropSQLwSave.DataBind();
            DropSQLwSave.Items.Insert(0, new ListItem("==选择收藏查询条件==", "0"));
        }

        protected void SqlWhereShow(string SQLName)
        {
            DataTable dt = ChangeDealDP.getCST_ISSUE_LISTFASTQUERY("frm_ChangeQuery", Session["UserName"].ToString(), SQLName);

            if (dt.Rows.Count > 0)
            {
                string[] SQLTextDetailList = dt.Rows[0]["SQLText"].ToString().Split(new string[] { "|@@?@$|" }, StringSplitOptions.None);

                for (int i = 0; i < SQLTextDetailList.Length; i++)
                {
                    string[] SQLTextDetail = SQLTextDetailList[i].Trim().Split('=');

                    //用户信息
                    if (SQLTextDetail[0].Trim() == "txtCustInfo.Text")
                    {
                        this.txtCustInfo.Text = SQLTextDetail[1].Trim();
                    }

                    //流程状态
                    if (SQLTextDetail[0].Trim() == "cboStatus.SelectedValue")
                    {
                        this.cboStatus.SelectedValue = SQLTextDetail[1].Trim();
                    }

                    //变更状态
                    if (SQLTextDetail[0].Trim() == "CtrFlowChangeState.SelectedValue")
                    {
                        this.CtrDealState.CatelogID = Convert.ToInt64(SQLTextDetail[1].Trim());
                    }

                    //标题
                    if (SQLTextDetail[0].Trim() == "txtTitle.Text")
                    {
                        this.txtTitle.Text = SQLTextDetail[1].Trim();
                    }

                    //变更时间
                    if (SQLTextDetail[0].Trim() == "ctrDateSelectTime1.BeginTime")
                    {
                        this.txtRegTime.Text = SQLTextDetail[1].Trim();
                    }
                    if (SQLTextDetail[0].Trim() == "ctrDateSelectTime1.EndTime")
                    {
                        this.txtEndTime.Text = SQLTextDetail[1].Trim();
                    }

                    //资产目录
                    if (SQLTextDetail[0].Trim() == "txtEquipmentDir.Text")
                    {
                        this.txtEquipmentDir.Text = SQLTextDetail[1].Trim();
                    }

                    //资产名称
                    if (SQLTextDetail[0].Trim() == "txtEquipmentName.Text")
                    {
                        this.txtEquipmentName.Text = SQLTextDetail[1].Trim();
                    }

                    //变更级别
                    if (SQLTextDetail[0].Trim() == "CtrFCDlevel.CatelogID")
                    {
                        this.CtrFCDlevel.CatelogID = Convert.ToInt64(SQLTextDetail[1].Trim());
                    }

                    //影响度
                    if (SQLTextDetail[0].Trim() == "CtrFCDEffect.CatelogID")
                    {
                        this.CtrFCDEffect.CatelogID = Convert.ToInt64(SQLTextDetail[1].Trim());
                    }

                    //紧急度
                    if (SQLTextDetail[0].Trim() == "CtrFCDInstancy.CatelogID")
                    {
                        this.CtrFCDInstancy.CatelogID = Convert.ToInt64(SQLTextDetail[1].Trim());
                    }
                    //变更类别
                    if (SQLTextDetail[0].Trim() == "CtrChangeType.CatelogID")
                    {
                        this.CtrChangeType.CatelogID = Convert.ToInt64(SQLTextDetail[1].Trim());
                    }
                }

            }

        }

        private void InitDropSQLwSave1(string SQLName)
        {
            DataTable dt = ChangeDealDP.getCST_ISSUE_LISTFASTQUERY("frm_ChangeQuery", Session["UserName"].ToString(), string.Empty);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Name"].ToString().Trim() == SQLName.Trim())
                {
                    DropSQLwSave.SelectedValue = dt.Rows[i]["ID"].ToString().Trim();
                }
            }
        }

        private void setControlValue()
        {
            cboStatus.SelectedValue = "20";
            CtrDealState.CatelogID = 0;
            CtrFCDlevel.CatelogValue = string.Empty;
            CtrFCDEffect.CatelogValue = string.Empty;
            CtrFCDEffect.CatelogID = -1;
            CtrFCDInstancy.CatelogValue = string.Empty;
            CtrChangeType.CatelogValue = string.Empty;
            txtCustInfo.Text = staCustInfo;
            txtEquipmentName.Text = string.Empty;
            txtEquipmentDir.Text = string.Empty;
            txtTitle.Text = string.Empty;
            txtRegTime.Text = staMsgDateBegion;
            txtEndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 查询条件
        /// </summary>
        /// <returns></returns>
        private string LoadDataPre()
        {
            string sWhere = "";
            int iRowCount = 0;
            string strCustInfo = "";

            if (cboStatus.SelectedItem.Value.Trim() != "-1")
                sWhere += " and status= " + cboStatus.SelectedItem.Value.ToString();
            if (!string.IsNullOrEmpty(CtrDealState.CatelogValue.Trim()))
                sWhere += " and DealStatusID= " + CtrDealState.CatelogID.ToString();

            if (!string.IsNullOrEmpty(CtrFCDlevel.CatelogValue.Trim()))
                sWhere += " and LevelID= " + CtrFCDlevel.CatelogID.ToString();
            if (!string.IsNullOrEmpty(CtrFCDEffect.CatelogValue.Trim()))
                sWhere += " and EffectID= " + CtrFCDEffect.CatelogID.ToString();
            if (!string.IsNullOrEmpty(CtrFCDInstancy.CatelogValue.Trim()))
                sWhere += " and InstancyID= " + CtrFCDInstancy.CatelogID.ToString();
            if (!string.IsNullOrEmpty(CtrChangeType.CatelogValue.Trim()))
                sWhere += "and ChangeTypeID=" + CtrChangeType.CatelogID.ToString();
            if (!string.IsNullOrEmpty(txtCustInfo.Text.Trim()))
            {
                strCustInfo = txtCustInfo.Text.Trim();

                string sSqlWhere = @"select ID from Br_ECustomer where 1=1";

                sSqlWhere += " And (";
                sSqlWhere += " CustName like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere += " OR Contact like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere += " OR customcode like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere += " OR CustAddress like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere += " OR mastCustname like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere += " OR Email like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere += " OR CTel like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere += " ) ";
                sWhere += " AND custid IN(" + sSqlWhere + ")";

            }

            #region 筛选资产信息
            string sWhereEqu = "";
            if (!string.IsNullOrEmpty(txtEquipmentName.Text.Trim()))
            {
                sWhereEqu = "EquName like " + StringTool.SqlQ("%" + txtEquipmentName.Text.Trim() + "%");
            }

            if (!string.IsNullOrEmpty(txtEquipmentDir.Text.Trim()))
            {
                if (sWhereEqu == "")
                {
                    sWhereEqu = "a.ListName like " + StringTool.SqlQ("%" + txtEquipmentDir.Text.Trim() + "%");
                }
                else
                {
                    sWhereEqu += " and  a.ListName like " + StringTool.SqlQ("%" + txtEquipmentDir.Text.Trim() + "%");
                }
            }
            DataTable dt = ChangeDealDP.getEquipment(sWhereEqu);
            string changeids = "";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == (dt.Rows.Count - 1))
                    {
                        changeids += "'" + dt.Rows[i]["ChangeID"].ToString().Trim() + "'";
                    }
                    else
                    {
                        changeids += "'" + dt.Rows[i]["ChangeID"].ToString().Trim() + "',";
                    }
                }
            }
            else
            {
                changeids += "'0'";
            }

            if (changeids != "")
            {
                sWhere += " and ID in (" + changeids + ")";
            }

            #endregion

            if (!string.IsNullOrEmpty(txtTitle.Text.Trim()))
                sWhere += " and Subject like " + StringTool.SqlQ("%" + txtTitle.Text.Trim() + "%");
            if (!string.IsNullOrEmpty(txtRegTime.Text.Trim()))
                sWhere += " and ChangeTime>= to_date(" + StringTool.SqlQ(txtRegTime.Text.Trim()) + ",'yyyy-MM-dd HH24:mi:ss')";
            if (!string.IsNullOrEmpty(txtEndTime.Text.Trim()))
                sWhere += " and ChangeTime<= to_date(" + StringTool.SqlQ(txtEndTime.Text.Trim()) + ",'yyyy-MM-dd HH24:mi:ss')";

            if (icurrent == 2)
            {
                sWhere = "";
                strCustInfo = this.Master.TxtKeyName.Value.Trim().ToString();

                if (strCustInfo != "请输入变更单号,客户信息")
                {
                    string sSqlWhere1 = @"select ID from Br_ECustomer where 1=1";

                    sSqlWhere1 += " And (";
                    sSqlWhere1 += " CustName like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                    sSqlWhere1 += " OR Contact like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                    sSqlWhere1 += " OR customcode like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                    sSqlWhere1 += " OR CustAddress like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                    sSqlWhere1 += " OR mastCustname like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                    sSqlWhere1 += " OR Email like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                    sSqlWhere1 += " OR CTel like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                    sSqlWhere1 += " ) ";
                    sWhere += " AND (custid IN (" + sSqlWhere1 + ")";

                    sWhere += "  or  ChangeNo like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%") + ")";
                }
            }

            DataTable dt1 = ChangeDealDP.getCST_ISSUE_LISTFASTQUERY("frm_ChangeQuery", Session["UserName"].ToString(), DropSQLwSave.SelectedItem.Text.ToString());
            if (dt1.Rows.Count > 0)
                this.HiddenColumn.Value = dt1.Rows[0]["DISPLAYCOLUMN"].ToString();
            return sWhere;
        }

        private string LoadDataPre2()
        {
            string sWhere = "";
            int iRowCount = 0;
            string strCustInfo = "";

            if (cboStatus.SelectedItem.Value.Trim() != "-1")
                sWhere += " and status= " + cboStatus.SelectedItem.Value.ToString();
            if (!string.IsNullOrEmpty(CtrDealState.CatelogValue.Trim()))
                sWhere += " and DealStatusID= " + CtrDealState.CatelogID.ToString();

            if (!string.IsNullOrEmpty(CtrFCDlevel.CatelogValue.Trim()))
                sWhere += " and LevelID= " + CtrFCDlevel.CatelogID.ToString();
            if (!string.IsNullOrEmpty(CtrFCDEffect.CatelogValue.Trim()))
                sWhere += " and EffectID= " + CtrFCDEffect.CatelogID.ToString();
            if (!string.IsNullOrEmpty(CtrFCDInstancy.CatelogValue.Trim()))
                sWhere += " and InstancyID= " + CtrFCDInstancy.CatelogID.ToString();
            if (!string.IsNullOrEmpty(CtrChangeType.CatelogValue.Trim()))
                sWhere += "and ChangeTypeID=" + CtrChangeType.CatelogID.ToString();

            if (!string.IsNullOrEmpty(txtCustInfo.Text.Trim()))
            {
                strCustInfo = txtCustInfo.Text.Trim();

                string sSqlWhere = @"select ID from Br_ECustomer where 1=1";

                sSqlWhere += " And (";
                sSqlWhere += " CustName like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere += " OR Contact like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere += " OR customcode like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere += " OR CustAddress like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere += " OR mastCustname like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere += " OR Email like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere += " OR CTel like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere += " ) ";
                sWhere += " AND custid IN(" + sSqlWhere + ")";

            }

            #region 筛选资产信息
            string sWhereEqu = "";
            if (!string.IsNullOrEmpty(txtEquipmentName.Text.Trim()))
            {
                sWhereEqu = "EquName like " + StringTool.SqlQ("%" + txtEquipmentName.Text.Trim() + "%");
            }

            if (!string.IsNullOrEmpty(txtEquipmentDir.Text.Trim()))
            {
                if (sWhereEqu == "")
                {
                    sWhereEqu = "a.ListName like " + StringTool.SqlQ("%" + txtEquipmentDir.Text.Trim() + "%");
                }
                else
                {
                    sWhereEqu += " and  a.ListName like " + StringTool.SqlQ("%" + txtEquipmentDir.Text.Trim() + "%");
                }
            }
            DataTable dt = ChangeDealDP.getEquipment(sWhereEqu);
            string changeids = "";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == (dt.Rows.Count - 1))
                    {
                        changeids += "'" + dt.Rows[i]["ChangeID"].ToString().Trim() + "'";
                    }
                    else
                    {
                        changeids += "'" + dt.Rows[i]["ChangeID"].ToString().Trim() + "',";
                    }
                }
            }
            if (changeids != "")
            {
                sWhere += " and a.ID in (" + changeids + ")";
            }
            #endregion

            if (!string.IsNullOrEmpty(txtTitle.Text.Trim()))
                sWhere += " and Subject like " + StringTool.SqlQ("%" + txtTitle.Text.Trim() + "%");
            if (!string.IsNullOrEmpty(txtRegTime.Text.Trim()))
                sWhere += " and ChangeTime>= to_date(" + StringTool.SqlQ(txtRegTime.Text.Trim()) + ",'yyyy-MM-dd HH24:mi:ss')";
            if (!string.IsNullOrEmpty(txtEndTime.Text.Trim()))
                sWhere += " and ChangeTime<= to_date(" + StringTool.SqlQ(txtEndTime.Text.Trim() + " 23:59:59") + ",'yyyy-MM-dd HH24:mi:ss')";

            //查询按钮
            if (icurrent == 2)
            {
                sWhere = "";
                strCustInfo = this.Master.TxtKeyName.Value.Trim().ToString();

                if (strCustInfo != "请输入变更单号,客户信息")
                {
                    string sSqlWhere1 = @"select ID from Br_ECustomer where 1=1";

                    sSqlWhere1 += " And (";
                    sSqlWhere1 += " CustName like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                    sSqlWhere1 += " OR Contact like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                    sSqlWhere1 += " OR customcode like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                    sSqlWhere1 += " OR CustAddress like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                    sSqlWhere1 += " OR mastCustname like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                    sSqlWhere1 += " OR Email like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                    sSqlWhere1 += " OR CTel like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                    sSqlWhere1 += " ) ";
                    sWhere += " AND (custid IN (" + sSqlWhere1 + ")";

                    sWhere += "  or  ChangeNo like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%") + ")";
                }
            }

            //初次加载时只加载一个月的数据
            if (icurrent == 3)
            {
                sWhere = "";
                sWhere += " and ChangeTime>= to_date(" + StringTool.SqlQ(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd")) + ",'yyyy-MM-dd HH24:mi:ss')";
                sWhere += " and ChangeTime<= to_date(" + StringTool.SqlQ(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59") + ",'yyyy-MM-dd HH24:mi:ss')";
                sWhere += " and status=20 ";
            }

            return sWhere;
        }

        #endregion

        protected void HidButton_Click(object sender, EventArgs e)
        {
            icurrent = 1;
            PageLoadQuery();
        }

        private void PageLoadQuery()
        {
            SetParentButtonEvent();
            cpChange.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(LoadData);

            this.Master.KeyValue = "请输入变更单号,客户信息";
            hidUserID.Value = ((long)(Session["UserID"])).ToString();

            this.txtFastQuery.Attributes.Add("onclick", "txtFastQueryClear()");

            //快速搜索
            if (Request["svalue"] != null)
            {
                txtCustInfo.Text = Request["svalue"].ToString().Trim();
                staCustInfo = Request["svalue"].ToString().Trim();
            }

            re = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.EquChangeQuery];

            cboStatus.Items.Add(new ListItem("所有状态", "-1"));
            cboStatus.Items.Add(new ListItem("--正在处理", ((int)EpowerGlobal.e_FlowStatus.efsHandle).ToString()));
            cboStatus.Items.Add(new ListItem("--正常结束", ((int)EpowerGlobal.e_FlowStatus.efsEnd).ToString()));
            cboStatus.Items.Add(new ListItem("--流程暂停", ((int)EpowerGlobal.e_FlowStatus.efsStop).ToString()));
            cboStatus.Items.Add(new ListItem("--流程终止", ((int)EpowerGlobal.e_FlowStatus.efsAbort).ToString()));
            cboStatus.SelectedIndex = 1;

            InitDropSQLwSave();  //初始化查询条件

            string sQueryBeginDate = "0";
            if (CommonDP.GetConfigValue("Other", "QueryBeginDate") != null)
                sQueryBeginDate = CommonDP.GetConfigValue("Other", "QueryBeginDate").ToString();
            if (sQueryBeginDate == "0")
            {
                txtRegTime.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                staMsgDateBegion = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            }
            else
            {
                txtRegTime.Text = DateTime.Parse(sQueryBeginDate).ToString("yyyy-MM-dd");
                staMsgDateBegion = DateTime.Parse(sQueryBeginDate).ToString("yyyy-MM-dd");
            }
            txtEndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            ////查询条件赋值
            //Control[] arrControl = { Table12 };
            //PageDeal.SetPageQueryParam(arrControl, cpChange, "frm_ChangeQuery");

            #region 装载高级查询条件
            if (hidSQLName.Value != "")
            {
                if (hidSQLName.Value != "Temp1")
                {
                    DataTable dt = ChangeDealDP.getCST_ISSUE_Where("frm_ChangeQuery", Session["UserName"].ToString(), hidSQLName.Value.ToString().Trim());

                    if (dt.Rows[0]["SQLText"].ToString().Trim() == dt.Rows[1]["SQLText"].ToString().Trim())
                    {
                        #region 如果为原条件

                        if (dt.Rows[0]["Name"].ToString().Trim() != "Temp1")
                        {
                            //更新访问次数
                            ChangeDealDP.updateCST_ISSUE_WhereNums(Session["UserName"].ToString(), dt.Rows[0]["Name"].ToString().Trim());

                            hidSQLName.Value = dt.Rows[0]["Name"].ToString().Trim();
                            SqlWhereShow(dt.Rows[0]["Name"].ToString().Trim());
                            InitDropSQLwSave1(dt.Rows[0]["Name"].ToString().Trim());
                        }

                        if (dt.Rows[1]["Name"].ToString().Trim() != "Temp1")
                        {
                            //更新访问次数
                            ChangeDealDP.updateCST_ISSUE_WhereNums(Session["UserName"].ToString(), dt.Rows[1]["Name"].ToString().Trim());

                            hidSQLName.Value = dt.Rows[1]["Name"].ToString().Trim();
                            SqlWhereShow(dt.Rows[1]["Name"].ToString().Trim());
                            InitDropSQLwSave1(dt.Rows[1]["Name"].ToString().Trim());
                        }
                    }
                        #endregion
                    else
                    {
                        #region 如果不为原条件
                        hidSQLName.Value = "Temp1";
                        SqlWhereShow("Temp1");
                        this.DropSQLwSave.SelectedIndex = DropSQLwSave.Items.IndexOf(DropSQLwSave.Items.FindByValue("0"));
                        #endregion
                    }
                }
                else
                {
                    hidSQLName.Value = "Temp1";
                    SqlWhereShow("Temp1");
                    try
                    {
                        this.DropSQLwSave.SelectedIndex = DropSQLwSave.Items.IndexOf(DropSQLwSave.Items.FindByValue("0"));
                    }
                    catch
                    { }
                }
            }
            else
            {
                hidSQLName.Value = "Temp1";
            }

            #endregion

            LoadData();

            imgTBSJ.Attributes.Add("onclick", "fPopUpDlg('../Controls/Calendar/calendar.htm',document.all." + txtRegTime.ClientID + ", 'winpop', 234, 261);return false");
            imgEnd.Attributes.Add("onclick", "fPopUpDlg('../Controls/Calendar/calendar.htm',document.all." + txtEndTime.ClientID + ", 'winpop', 234, 261);return false");

            Session["FromUrl"] = "../EquipmentManager/frm_ChangeQuery.aspx";



            //保存查询条件
            Control[] arrControl1 = { Table12 };
            PageDeal.GetPageQueryParam(arrControl1, cpChange, "frm_ChangeQuery");
        }

        #region  设置显示字段

        public string SetColumnDisplay(string arr)
        {
            if (!string.IsNullOrEmpty(arr))
            {
                for (int i = 3; i <= this.dgProblem.Columns.Count; i++)
                {

                    if (this.dgProblem.Columns.Count < 9)
                        break;
                    else
                        this.dgProblem.Columns.RemoveAt(3);
                }
                DataBing(arr);

            }

            return "";
        }
        #region DataGrid 字段设置
        public void DataBing(string columns)
        {

            string[] columnValues = columns.Split(',');

            for (int i = columnValues.Length - 1; i > -1; i--)
            {
                if (columnValues[i] != "")
                {
                    BoundColumn bf = new BoundColumn();
                    bf.HeaderText = PageDeal.GetLanguageValue1(columnValues[i], "变更单");
                    bf.DataField = columnValues[i];
                    bf.SortExpression = columnValues[i];
                    bf.ItemStyle.Width = 150;
                    bf.HeaderStyle.Width = 150;
                    bf.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                    if (columnValues[i] == "RegSysDate")
                    {
                        bf.DataFormatString = "{0:g}";
                    }
                    this.dgProblem.Columns.AddAt(3, bf);
                }
            }
        }

        #endregion
        #endregion

        /// <summary>
        /// 删除记录后, 刷新数据.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void hidd_btnDelete_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
