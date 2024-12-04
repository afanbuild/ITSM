/*******************************************************************
 * 版权所有：深圳市非凡信息技术有限公司
 * 描述："需求管理" - 查询页面
 * 
 * 
 * 创建人：孙绍棕
 * 创建日期：2013-04-27
 * 
 * 修改日志：
 * 修改时间：2013-04-27 修改人：孙绍棕
 * 修改描述：
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
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;
using Epower.DevBase.BaseTools;
using System.Xml;
using Epower.ITSM.SqlDAL.Demand;
using EpowerCom;


namespace Epower.ITSM.Web.Demand
{
    public partial class frm_Req_DemandMain : BasePage
    {
        #region 委托: 取要导出到Excel的数据表 - 2013-04-27 @孙绍棕


        /// <summary>
        /// 取要导出到Excel的数据表
        /// </summary>
        /// <param name="arrDisplayName">显示字段名数组</param>
        /// <param name="arrFieldName">显示名数组</param>
        /// <returns>需求单表</returns>
        private delegate DataTable GetDataForExport(String[] arrDisplayName, String[] arrFieldName);

        #endregion

        #region 常量定义 - 2013-04-01 @孙绍棕


        /// <summary>
        /// 高级查询
        /// </summary>
        private const string __ADVANCED_SEARCH = "0";

        #endregion
        #region 变量定义

        RightEntity re = null;

        #region TypeID
        /// <summary>
        /// 
        /// </summary>
        protected string TypeID
        {
            get
            {
                if (ViewState["TypeID"] != null)
                    return ViewState["TypeID"].ToString();
                else
                    return "0";
            }
            set
            {
                ViewState["TypeID"] = value;
                hfTypeId.Value = value;
            }
        }
        #endregion

        #region FromBackUrl

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

        #region  SetParentButtonEvent
        /// <summary>
        /// 设置父窗体按钮事件
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.Master_Button_ExportExcel_Click += new Global_BtnClick(Master_Master_Button_ExportExcel_Click);
            this.Master.ShowQueryButton(true);
            this.Master.ShowNewButton(false);
            this.Master.ShowExportExcelButton(true);
            this.Master.TxtKeyName.Visible = true;

            RightEntity re = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.ReqDemandList];
            if (!re.CanAdd)
            {
                this.Master.ShowNewButton(false);
            }

            if (re.CanDelete)
            {
                this.Master.ShowDeleteButton(true);
                this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);
            }

        }

        #endregion

        #region Excel导出
        /// <summary>
        /// Excel导出
        /// </summary>
        void Master_Master_Button_ExportExcel_Click()
        {
            #region Step 1. 定义Excel默认导出的显示字段


            string[] key = null;
            string[] value = null;
            string[] arrField = {   "DemandNo", 
                                    "CustUserName",
                                    "CustTel",
                                    "CustEmail",
                                    "EquipmentName",
                                    "DemandTypeName", 
                                    "DemandSubject", 
                                    "DemandContent", 
                                    "DemandStatus", 
                                    "RegUserName",
                                    "RegTime"};

            string[] fileName = {   "需求单号", 
                                    "客户名称",
                                    "联系方式",
                                    "客户邮箱",
                                    "资产名称", 
                                    "需求类别", 
                                    "需求主题", 
                                    "详细描述", 
                                    "需求状态", 
                                    "登单人",
                                    "登记时间"};

            #endregion

            #region Setp 2. 获取 EA_DefinaLanguage 中设置的显示字段名


            string s = "需求单号";
            for (int i = 1; i < arrField.Length; i++)
            {
                s += "," + PageDeal.GetLanguageValue1(arrField[i], "需求管理");
            }
            fileName = s.Split(',');        

            #endregion

            #region Step 3. 定义基本变量

            long lngUserID = (long)Session["UserID"];
            long lngDeptID = (long)Session["UserDeptID"];
            long lngOrgID = (long)Session["UserOrgID"];
            //获得权限
            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.QuestionTrace];
            Int32 intRowCount = 0;
            Int32 intFirstPage = 1;
            Int32 intAllRecord = Int32.MaxValue;    // 注意: 若总记录数超过该最大数值, 则不能取出全部记录.

            ReqDemandDP reqDemandDP = new ReqDemandDP();
            GetDataForExport getDataForExport = null;

            #endregion

            switch (TypeID)
            {
                case "0":    // 高级查询

                    #region 高级查询

                    getDataForExport = new GetDataForExport(
                        delegate(String[] arrDisplayName,
                                 String[] arrFieldName)
                        {
                            String strCondition = ctrCondition.strCondition;

                            DataTable dt = reqDemandDP.FetchList(strCondition,
                                lngUserID,
                                lngDeptID,
                                lngOrgID,
                                reTrace,
                                intAllRecord,
                                intFirstPage,
                                ref intRowCount);

                            return dt;
                        });

                    ExportAsExcel(fileName, arrField, getDataForExport);

                    #endregion

                    break;
                case "1":    // 快速搜索                    

                    #region 快速搜索


                    getDataForExport = new GetDataForExport(
                        delegate(String[] arrDisplayName,
                                 String[] arrFieldName)
                        {
                            XmlDocument xmlDoc = GetXmlValue();

                            DataTable dt = reqDemandDP.FetchListByFastSearch(xmlDoc.InnerXml,
                                lngUserID,
                                lngDeptID,
                                lngOrgID,
                                reTrace,
                                intAllRecord,
                                intFirstPage,
                                ref intRowCount);

                            return dt;
                        });

                    ExportAsExcel(fileName, arrField, getDataForExport);

                    #endregion

                    break;
                case "10":
                default:    // 导出最近一个月的需求单

                    #region 导出最近一个月的需求单

                    getDataForExport = new GetDataForExport(
                        delegate(String[] arrDisplayName,
                                 String[] arrFieldName)
                        {
                            DataTable dt = reqDemandDP.FetchListForRecentMonth(lngUserID,
                                            lngDeptID,
                                            lngOrgID,
                                            reTrace,
                                            intAllRecord,
                                            intFirstPage,
                                            ref intRowCount);

                            return dt;
                        });

                    ExportAsExcel(fileName, arrField, getDataForExport);

                    #endregion

                    break;
            }
        }

        #endregion

        #region Excel. 导出 Excel

        /// <summary>
        /// 高级查询导出到Excel
        /// </summary>
        /// <param name="fileName">字段显示名数组</param>
        /// <param name="arrFlide">字段名数组</param>
        private void ExportAsExcel(
            string[] arrDisplayName,
            string[] arrFieldName,
            GetDataForExport getDataForExportFunc)
        {
            #region Step 1. 取数据源

            DataTable dt = getDataForExportFunc(arrDisplayName, arrFieldName);

            #endregion

            #region Step 2. 导出为Excel

            Epower.ITSM.Web.Common.ExcelExport.ExportReqDemandList(this,
                dt,
                arrDisplayName,
                arrFieldName,
                Session["UserID"].ToString());

            #endregion
        }

        #endregion

        #region 申请Master_Master_Button_New_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("~/Forms/form_all_flowmodel.aspx?appid=210");
        }
        #endregion

        #region 母板页中的删除按钮事件

        /// <summary>
        /// 母板页中的删除按钮事件
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            Message msg = new Message();

            String[] arrFlowID = this.Master.MainID.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            if (arrFlowID.Length <= 0)
            {
                PageTool.MsgBox(this, "请选择要删除的流程!");
                return;
            }

            foreach (String strFlowID in arrFlowID)
            {
                long lngFlowID = long.Parse(strFlowID);

                msg.AdminDeleteFlow(lngFlowID,
                    (long)Session["UserID"],
                    "批量删除流程");
            }

            LoadData();
            ctrCondition.Bind();
        }

        #endregion

        #region  查询事件
        /// <summary>
        /// 查询事件
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            //点击查询按钮时，获取查询框中的内容进行查询

            TypeID = "1";

            LoadData();
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

            cpProblem.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(LoadData);

            dgProblem.Columns[dgProblem.Columns.Count - 2].Visible = CheckRight(Constant.admindeleteflow);

            if (!IsPostBack)
            {
                this.Master.KeyValue = "请输入需求单号";
                hidUserID.Value = ((long)(Session["UserID"])).ToString();

                //设置显示
                PageDeal.SetLanguage(this.Controls[0]);

                //dgProblem.Columns[6].HeaderText = PageDeal.GetLanguageValue("litProblemNo");
                //dgProblem.Columns[7].HeaderText = PageDeal.GetLanguageValue("litProblemTitle");
                //dgProblem.Columns[8].HeaderText = PageDeal.GetLanguageValue("litProbleType");
                //dgProblem.Columns[9].HeaderText = PageDeal.GetLanguageValue("litProbleLevel");
                //dgProblem.Columns[10].HeaderText = PageDeal.GetLanguageValue("litProbleState");
                //变更权限
                dgProblem.Columns[dgProblem.Columns.Count - 1].Visible = CheckRight(Constant.ReqDemandList);

                re = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.ReqDemandList];

                //快速搜索

                if (Request["svalue"] != null)
                {
                    //txtTitle.Text = Request["svalue"].ToString().Trim();
                }

                #region 装载高级查询条件

                if (hidSQLName.Value != "" && hidSQLName.Value != "==选择收藏查询条件==")
                {
                    TypeID = "0";
                }
                else
                {
                    TypeID = "10";                       //如果是第一次加载，则默认查询正在处理的、发生时间为近一个月的权限范围内的记录

                }

                #endregion

                LoadData();

                Session["FromUrl"] = "../Demand/frm_Req_DemandMain.aspx";
                FromBackUrl = "../Demand/frm_Req_DemandMain.aspx";
            }
            else
            {
                #region 装载高级查询条件

                if (hidIsGaoji.Value == "1")
                {

                    TypeID = "0";
                }

                LoadData(hfTypeId.Value);
                #endregion
            }

            #region 动态查询: 设置动态查询参数 - 2013-04-01 @孙绍棕


            this.ctrCondition.TableName = "Req_Demand";
            this.ctrCondition.mybtnSelectOnClick += new EventHandler(ctrCondition1_mybtnSelectOnClick);
            this.ctrCondition.mySelectedIndexChanged += new EventHandler(ctrCondition1_mySelectedIndexChanged);

            #endregion
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
            this.TypeID = __ADVANCED_SEARCH;
            ctrCondition.ddlSelectChanged();

            LoadData(__ADVANCED_SEARCH);
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
            this.TypeID = __ADVANCED_SEARCH;
            LoadData(__ADVANCED_SEARCH);

            ctrCondition.Bind();
        }

        #endregion

        #region  LoadData
        private void LoadData()
        {
            LoadData(TypeID);
        }
        /// <summary>
        /// 数据加载
        /// </summary>
        private void LoadData(String strTypeId)
        {
            #region 验证是否登录

            if (Session["UserID"] == null || Session["UserDeptID"] == null || Session["UserOrgID"] == null)
            {
                return;
            }

            #endregion

            #region 验证权限

            //获得权限
            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.QuestionTrace];
            if (reTrace == null)
            {
                return;
            }

            #endregion


            #region 定义基本变量

            long lngUserID = (long)Session["UserID"];
            long lngDeptID = (long)Session["UserDeptID"];
            long lngOrgID = (long)Session["UserOrgID"];

            Int32 intPageSize = this.cpProblem.PageSize;
            Int32 intCurrentPage = this.cpProblem.CurrentPage;
            Int32 intRowCount = 0;

            #endregion

            DataTable dt = null;
            ReqDemandDP reqDemandDP = new ReqDemandDP();

            switch (strTypeId)
            {
                case "0":    // 高级查询

                    hidIsGaoji.Value = "0";         //将值还原为0
                    String strWhere = this.ctrCondition.strCondition;    // 动态生成的SQL语句

                    dt = reqDemandDP.FetchList(strWhere,
                        lngUserID,
                        lngDeptID,
                        lngOrgID,
                        reTrace,
                        intPageSize,
                        intCurrentPage,
                        ref intRowCount);

                    break;
                case "1":    // 快速搜索


                    XmlDocument xmlDoc = GetXmlValue();

                    dt = reqDemandDP.FetchListByFastSearch(xmlDoc.InnerXml,
                        lngUserID,
                        lngDeptID,
                        lngOrgID,
                        reTrace,
                        intPageSize,
                        intCurrentPage,
                        ref intRowCount);

                    break;
                case "10":    //查询出正在处理的，发生时间为近一个月的权限范围内的记录


                    dt = reqDemandDP.FetchListForRecentMonth(lngUserID,
                        lngDeptID,
                        lngOrgID,
                        reTrace,
                        intPageSize,
                        intCurrentPage,
                        ref intRowCount);

                    break;
                default:
                    dt = new DataTable();
                    break;
            }


            dgProblem.DataSource = dt.DefaultView;
            dgProblem.DataBind();

            this.cpProblem.RecordCount = intRowCount;
            this.cpProblem.Bind();

        }

        #endregion 获取快速搜索查询条件xml值


        #region 获取快速搜索查询条件xml值

        /// <summary>
        /// 获取快速搜索查询条件xml值

        /// </summary>
        /// <returns></returns>
        private XmlDocument GetXmlValue()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlRoot = xmlDoc.CreateElement("Fields");
            XmlElement xmlEle;

            #region 需求单号

            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "DemandNo");
            xmlEle.SetAttribute("Value", this.Master.TxtKeyName.Value.Trim().ToString() == "请输入需求单号" ? "" : this.Master.TxtKeyName.Value.Trim().ToString());
            xmlRoot.AppendChild(xmlEle);
            #endregion

            xmlDoc.AppendChild(xmlRoot);
            return xmlDoc;
        }
        #endregion

        #endregion

        #region ControlPage1_On_PostBack
        /// <summary>
        /// ControlPage1_On_PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ControlPage1_On_PostBack(object sender, EventArgs e)
        {
            LoadData();
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
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    // (DataView)e.Item.NamingContainer;
                    if (i == 5 || i == 6 || i == 7 || i == 8)
                    {
                        int j = i - 4;  //前面有6个隐藏的列


                        e.Item.Cells[i].Attributes.Add("onclick", String.Format("sortTable('{0}',{1},{2});",
                            dg.ClientID, j, 0));
                    }
                }
            }
        }
        #endregion

        #region  dgProblem_ItemDataBound
        /// <summary>
        /// dgProblem_ItemDataBound
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
                e.Item.Attributes.Add("ondblclick", "SetUrl();window.open('../Forms/frmIssueView.aspx?FlowID=" + sID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes')");

                ((Label)e.Item.FindControl("Lb_ProblemNo")).Attributes.Add("onmouseover", "ShowDetailsInfo(this,'" + DataBinder.Eval(e.Item.DataItem, "ID").ToString() + "',400);");
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

        #region dgProblem_ItemCommand
        /// <summary>
        /// dgProblem_ItemCommand
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgProblem_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "deal")  //详情
            {
                Session["FromUrl"] = "../Demand/frm_Req_DemandMain.aspx";

                string sUrl = "";
                long lngFlowID = long.Parse(e.Item.Cells[15].Text.Trim());
                sUrl = "../Forms/frmIssueView.aspx?FlowID=" + lngFlowID.ToString();
                Response.Redirect(sUrl);
            }
        }
        #endregion

        #region 高级查询相关

        #region 确定高级条件时执行

        /// <summary>
        /// 确定高级条件时执行

        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void HidButton_Click(object sender, EventArgs e)
        {
            this.Master.KeyValue = "请输入需求单号";

            LoadData();
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
            LoadData(hfTypeId.Value);
        }
    }
}
