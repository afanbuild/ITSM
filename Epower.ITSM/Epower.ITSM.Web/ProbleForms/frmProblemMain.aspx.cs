/****************************************************************************
 * 
 * description:问题管理主页面
 * 
 * 
 * 
 * Create by:zhumingchun
 * Create Date:2007-06-23
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
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;
using Epower.DevBase.BaseTools;
using System.Xml;

namespace Epower.ITSM.Web.ProbleForms
{
    /// <summary>
    /// frmProblemMain
    /// </summary>
    public partial class frmProblemMain : BasePage
    {
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

            RightEntity re = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.QuestionTrace];
            if (!re.CanAdd)
            {
                this.Master.ShowNewButton(false);
            }

        }
        #endregion

        #region Excel导出
        /// <summary>
        /// Excel导出
        /// </summary>
        void Master_Master_Button_ExportExcel_Click()
        {
            string[] key = null;
            string[] value = null;
            string[] arrField = { "ProblemNo", "EquName", "Problem_TypeName", "Problem_LevelName", "EffectName", "InstancyName", "Problem_Title", "Problem_Subject", "StateName", "Remark", "RegUserName" };
            string[] fileName = { "问题单号", "资产名称", "问题类别", "问题级别", "影响度", "紧急度", "摘要", "问题描述", "问题状态", "解决方案", "登单人" };
            string s = "问题单号";
            for (int i = 1; i < arrField.Length; i++)
            {
                s += "," + PageDeal.GetLanguageValue1(arrField[i], "问题单");
            }
            fileName = s.Split(',');
            if (!string.IsNullOrEmpty(this.HiddenColumn.Value))
            {
                string[] columnValues = this.HiddenColumn.Value.Split(',');
                if (columnValues.Length > 1)
                {
                    string k = "ProblemNo";
                    for (int i = 0; i < columnValues.Length - 1; i++)
                    {
                        k += "," + columnValues[i];
                    }
                    key = k.Split(',');
                    string v = "问题单号";
                    for (int i = 1; i < key.Length; i++)
                    {
                        v += "," + PageDeal.GetLanguageValue1(key[i], "问题单");
                    }
                    value = v.Split(',');
                }
            }
            long lngUserID = (long)Session["UserID"];
            long lngDeptID = (long)Session["UserDeptID"];
            long lngOrgID = (long)Session["UserOrgID"];
            //获得权限
            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.QuestionTrace];

            switch (TypeID)
            {
                case "0":
                    //高级查询
                    if (!string.IsNullOrEmpty(this.HiddenColumn.Value))
                    {
                        ProblemHighExcel(value, key);
                    }
                    else
                    {
                        ProblemHighExcel(fileName, arrField);
                    }
                    break;
                case "1":
                    //快速搜索
                    ProblemFastExcel(fileName, arrField);
                    break;
                case "10":
                    //默认进入的时候。查询出正在处理的，发生时间为近一个月的权限范围内的记录
                    ProblemdefaultExcel(fileName, arrField);
                    break;
                default:
                    ProblemdefaultExcel(fileName, arrField);
                    break;
            }
        }

        #endregion

        #region 高级查询导出excel
        /// <summary>
        /// 高级查询导出excel
        /// </summary>
        private void ProblemHighExcel()
        {
            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.QuestionTrace];
            int iRowCount = 0;
            //高级查询 
            XmlDocument xmlDoc = SqlWhereShow(hidSQLName.Value == "==选择收藏查询条件==" ? "Temp1" : hidSQLName.Value);
            DataTable dt = ProblemDealDP.GetProblemsForCond(xmlDoc.InnerXml, long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()),
                    long.Parse(Session["UserOrgID"].ToString()), reTrace);
            Epower.ITSM.Web.Common.ExcelExport.ExportProblemList(this, dt, Session["UserID"].ToString());
        }
        /// <summary>
        /// 高级查询导出excel
        /// </summary>
        private void ProblemHighExcel(string[] fileName, string[] arrFlide)
        {
            string strWhere = " where 1=1 ";
            string strOrder = " ORDER BY SMSID DESC ";
            long lngUserID = (long)Session["UserID"];
            long lngDeptID = (long)Session["UserDeptID"];
            long lngOrgID = (long)Session["UserOrgID"];
            int intRowCount = -1;

            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.QuestionTrace];
            int iRowCount = 0;
            //高级查询 
            //XmlDocument xmlDoc = SqlWhereShow(hidSQLName.Value == "==选择收藏查询条件==" ? "Temp1" : hidSQLName.Value);
            //DataTable dt = ProblemDealDP.GetProblemsForCond(xmlDoc.InnerXml, long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()),
            //        long.Parse(Session["UserOrgID"].ToString()), reTrace);

            //获得动态查询条件

            string strCondition = ctrCondition.strCondition;
            if (!string.IsNullOrEmpty(strCondition))
            {
                strWhere += " and " + strCondition;
            }

            //DataTable dt = DevRequestDP.GetDataTableExcel(strWhere, strOrder, lngUserID, lngDeptID, lngOrgID, reTrace);


            DataTable dt = ProblemDealDP.GetProblemsWithOutPage(strWhere,
                                                                    lngUserID,
                                                                    lngDeptID,
                                                                    lngOrgID,
                                                                    reTrace);


            Epower.ITSM.Web.Common.ExcelExport.ExportProblemList(this, dt, fileName, arrFlide, Session["UserID"].ToString());
        }
        #endregion

        #region 快速搜索导出excel
        /// <summary>
        /// 快速搜索导出excel
        /// </summary>
        private void ProblemFastExcel()
        {
            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.QuestionTrace];
            int iRowCount = 0;

            XmlDocument xmlDoc = GetXmlValue();
            DataTable dt = ProblemDealDP.GetProblemsForCond(xmlDoc.InnerXml, long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()),
                    long.Parse(Session["UserOrgID"].ToString()), reTrace);
            Epower.ITSM.Web.Common.ExcelExport.ExportProblemList(this, dt, Session["UserID"].ToString());
        }
        /// <summary>
        /// 快速搜索导出excel
        /// </summary>
        private void ProblemFastExcel(string[] fileName, string[] arrFlide)
        {
            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.QuestionTrace];
            int iRowCount = 0;

            XmlDocument xmlDoc = GetXmlValue();
            DataTable dt = ProblemDealDP.GetProblemsForCond(xmlDoc.InnerXml, long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()),
                    long.Parse(Session["UserOrgID"].ToString()), reTrace);
            Epower.ITSM.Web.Common.ExcelExport.ExportProblemList(this, dt, fileName, arrFlide, Session["UserID"].ToString());
        }
        #endregion

        #region 默认导出excel
        /// <summary>
        /// 默认导出excel
        /// </summary>
        private void ProblemdefaultExcel()
        {
            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.QuestionTrace];
            int iRowCount = 0;

            DataTable dt = ProblemDealDP.GetProbsForCondNew_Init("", long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()), long.Parse(Session["UserOrgID"].ToString()), reTrace);
            Epower.ITSM.Web.Common.ExcelExport.ExportProblemList(this, dt, Session["UserID"].ToString());
        }

        /// <summary>
        /// 默认导出excel
        /// </summary>
        private void ProblemdefaultExcel(string[] fileName, string[] arrFlide)
        {
            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.QuestionTrace];
            int iRowCount = 0;

            DataTable dt = ProblemDealDP.GetProbsForCondNew_Init("", long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()), long.Parse(Session["UserOrgID"].ToString()), reTrace);
            Epower.ITSM.Web.Common.ExcelExport.ExportProblemList(this, dt, fileName, arrFlide, Session["UserID"].ToString());
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

        #region  查询事件
        /// <summary>
        /// 查询事件
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            DropSQLwSave.SelectedIndex = 0;

            //点击查询按钮时，获取查询框中的内容进行查询
            TypeID = "1";

            LoadData();

            this.ctrCondition.SetDisplayMode = false;
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
                this.Master.KeyValue = "请输入问题单号";
                hidUserID.Value = ((long)(Session["UserID"])).ToString();

                //变更权限
                dgProblem.Columns[dgProblem.Columns.Count - 1].Visible = CheckRight(Constant.EquChangeQuery);
                InitDropSQLwSave();  //初始化查询条件

                re = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.QuestionTrace];


                #region 装载高级查询条件

                if (hidSQLName.Value != "" && hidSQLName.Value != "==选择收藏查询条件==")
                {
                    if (hidSQLName.Value != "Temp1")
                    {
                        DataTable dt = ProblemDealDP.getCST_ISSUE_Where("frmProblemMain", Session["UserName"].ToString(), hidSQLName.Value.ToString().Trim());

                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["SQLText"].ToString().Trim() == dt.Rows[1]["SQLText"].ToString().Trim())
                            {
                                #region 如果为原条件
                                if (dt.Rows[0]["Name"].ToString().Trim() != "Temp1")
                                {
                                    //更新访问次数
                                    ProblemDealDP.updateCST_ISSUE_WhereNums(Session["UserName"].ToString(), dt.Rows[0]["Name"].ToString().Trim());

                                    hidSQLName.Value = dt.Rows[0]["Name"].ToString().Trim();
                                    TypeID = "0";
                                }

                                if (dt.Rows[1]["Name"].ToString().Trim() != "Temp1")
                                {
                                    //更新访问次数
                                    ProblemDealDP.updateCST_ISSUE_WhereNums(Session["UserName"].ToString(), dt.Rows[1]["Name"].ToString().Trim());

                                    hidSQLName.Value = dt.Rows[1]["Name"].ToString().Trim();
                                    TypeID = "0";
                                }
                            }
                                #endregion
                            else
                            {
                                #region 如果不为原条件
                                hidSQLName.Value = "Temp1";
                                TypeID = "0";
                                #endregion
                            }
                        }
                    }
                    else
                    {
                        TypeID = "0";
                    }
                }
                else
                {
                    DropSQLwSave.SelectedIndex = 0;
                    TypeID = "10";                       //如果是第一次加载，则默认查询正在处理的、发生时间为近一个月的权限范围内的记录
                }

                #endregion

                LoadData();

                Session["FromUrl"] = "../ProbleForms/frmProblemMain.aspx";
                FromBackUrl = "../ProbleForms/frmProblemMain.aspx";
            }
            else
            {
                #region 装载高级查询条件

                if (hidIsGaoji.Value == "1")
                {
                    //当值为1时，表明从高级条件界面传过来的，此时应将hidSQLName的值传给DropSQLwSave

                    InitDropSQLwSave(hidSQLName.Value);

                    #region
                    if (hidSQLName.Value != "" && hidSQLName.Value != "==选择收藏查询条件==")
                    {
                        if (hidSQLName.Value != "Temp1")
                        {
                            //表明此时高级条件下拉列表选中的是自己定义的一个高级条件
                            DataTable dt = ProblemDealDP.getCST_ISSUE_Where("frmProblemMain", Session["UserName"].ToString(), hidSQLName.Value.ToString().Trim());

                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Rows[0]["SQLText"].ToString().Trim() == dt.Rows[1]["SQLText"].ToString().Trim())
                                {
                                    //如果完全相等，表明此时所用的查询条件，就是当前选中的高级条件名称；否则，是在当前选中的高级条件名称基础上新增了N条查询条件，且只是临时查询
                                    #region 如果为原条件
                                    if (dt.Rows[0]["Name"].ToString().Trim() != "Temp1")
                                    {
                                        //更新访问次数
                                        ProblemDealDP.updateCST_ISSUE_WhereNums(Session["UserName"].ToString(), dt.Rows[0]["Name"].ToString().Trim());

                                        hidSQLName.Value = dt.Rows[0]["Name"].ToString().Trim();
                                        TypeID = "0";
                                    }

                                    if (dt.Rows[1]["Name"].ToString().Trim() != "Temp1")
                                    {
                                        //更新访问次数
                                        ProblemDealDP.updateCST_ISSUE_WhereNums(Session["UserName"].ToString(), dt.Rows[1]["Name"].ToString().Trim());

                                        hidSQLName.Value = dt.Rows[1]["Name"].ToString().Trim();
                                        TypeID = "0";
                                    }
                                }
                                    #endregion
                                else
                                {
                                    #region 如果不为原条件；如果从高级条件传过来，则查询本次Temp1的临时xml串查询条件的记录；否则，查询下拉列表改变后的值
                                    if (hidIsGaoji.Value == "1")
                                    {
                                        //从高级条件传过来
                                        hidSQLName.Value = "Temp1";         //如果与原条件不同，则此时取Temp1的xml串
                                        DropSQLwSave.SelectedIndex = 0;     //此时，因为是查询一个临时的，所以下拉列表应设为根目录下
                                    }
                                    TypeID = "0";
                                    #endregion
                                }
                            }
                        }
                        else
                        {
                            TypeID = "0";
                        }
                    }
                    #endregion
                }
                else
                {
                    //否则，若为0，则说明是改变下拉列表的高级条件名称，此时将下拉列表改变后的值赋给hidSQLName
                    hidSQLName.Value = DropSQLwSave.SelectedItem.Text;
                }

                LoadData(hfTypeId.Value);
                #endregion
            }

            #region 动态查询: 设置动态查询参数 - 2013-04-01 @孙绍棕

            this.ctrCondition.TableName = "Pro_Problemdeal";
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

            this.Master.TxtKeyName.Value = "请输入问题单号";
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

            this.TypeID = __ADVANCED_SEARCH;
            LoadData(__ADVANCED_SEARCH);


            this.Master.TxtKeyName.Value = "请输入问题单号";
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
            int iRowCount = 0;
            if (Session["UserID"] == null || Session["UserDeptID"] == null || Session["UserOrgID"] == null)
            {
                return;
            }
            long lngUserID = (long)Session["UserID"];
            long lngDeptID = (long)Session["UserDeptID"];
            long lngOrgID = (long)Session["UserOrgID"];
            //获得权限
            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.QuestionTrace];
            if (reTrace == null)
            {
                return;
            }
            DataTable dt = null;
            XmlDocument xmlDoc = null;
            switch (strTypeId)
            {
                case "0":
                    //高级查询
                    hidIsGaoji.Value = "0";         //将值还原为0
                    //xmlDoc = SqlWhereShow(hidSQLName.Value);
                    //dt = ProblemDealDP.GetProblemsForCond(xmlDoc.InnerXml, lngUserID, lngDeptID, lngOrgID, reTrace, this.cpProblem.PageSize, this.cpProblem.CurrentPage, ref iRowCount);

                    Int32 intPageSize = this.cpProblem.PageSize;
                    Int32 intCurrentPage = this.cpProblem.CurrentPage;
                    Int32 intRowCount = iRowCount;
                    String strWhere = this.ctrCondition.strCondition;    // 动态生成的SQL语句

                    dt = ProblemDealDP.GetProblemsWithMoreParams(strWhere,
                        lngUserID,
                        lngDeptID,
                        lngOrgID,
                        reTrace,
                        intPageSize,
                        intCurrentPage,
                        ref iRowCount);

                    break;
                case "1":
                    //快速搜索
                    xmlDoc = GetXmlValue();
                    dt = ProblemDealDP.GetProblemsForCond(xmlDoc.InnerXml, lngUserID, lngDeptID, lngOrgID, reTrace, this.cpProblem.PageSize, this.cpProblem.CurrentPage, ref iRowCount);
                    break;
                case "10"://查询出正在处理的，发生时间为近一个月的权限范围内的记录
                    dt = ProblemDealDP.GetProbsForCondNew_Init("", long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()), long.Parse(Session["UserOrgID"].ToString()), reTrace, this.cpProblem.PageSize, this.cpProblem.CurrentPage, ref iRowCount);
                    break;
                default:
                    dt = new DataTable();
                    break;
            }


            dgProblem.DataSource = dt.DefaultView;
            dgProblem.DataBind();
            this.cpProblem.RecordCount = iRowCount;
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

            #region 问题单号
            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "ProblemNo");
            xmlEle.SetAttribute("Value", this.Master.TxtKeyName.Value.Trim().ToString() == "请输入问题单号" ? "" : this.Master.TxtKeyName.Value.Trim().ToString());
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
                    if (i == 6 || i == 7 || i == 8 || i == 9 || i == 10)
                    {
                        int j = i - 5;  //前面有6个隐藏的列

                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");

                        //e.Item.Cells[i].Attributes.Add("onclick", String.Format("sortTable('{0}',{1},{1});", dg.ClientID, j, 0));
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
                decimal sScale = StringTool.String2Decimal(e.Item.Cells[10].Text);
                decimal sEffect = StringTool.String2Decimal(e.Item.Cells[11].Text);
                decimal sStress = StringTool.String2Decimal(e.Item.Cells[12].Text);

                if (sScale > 50)
                    e.Item.Cells[10].ForeColor = System.Drawing.Color.Red;
                if (sEffect > 50)
                    e.Item.Cells[11].ForeColor = System.Drawing.Color.Red;
                if (sStress > 50)
                    e.Item.Cells[12].ForeColor = System.Drawing.Color.Red;

                decimal d = decimal.Parse(e.Item.Cells[e.Item.Cells.Count - 5].Text);
                if (d < 0)
                {
                    //超过流程规定的时间完成的
                    e.Item.Cells[6].ForeColor = System.Drawing.Color.Red;
                }

                if (DataBinder.Eval(e.Item.DataItem, "AssociateFlowID").ToString() == string.Empty || DataBinder.Eval(e.Item.DataItem, "AssociateFlowID").ToString() == "0")
                {
                    Button btnChange = (Button)e.Item.FindControl("btnAssociate");
                    Label lblChange = (Label)e.Item.FindControl("lblAssociate");
                    btnChange.Visible = true;
                    lblChange.Visible = false;
                }
                else
                {
                    Button btnChange = (Button)e.Item.FindControl("btnAssociate");
                    Label lblChange = (Label)e.Item.FindControl("lblAssociate");
                    btnChange.Visible = false;
                    lblChange.Visible = true;
                }

                //规范鼠标动作--ly
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                String sID = DataBinder.Eval(e.Item.DataItem, "FlowID").ToString();
                e.Item.Attributes.Add("ondblclick", "SetUrl();window.open('../Forms/frmIssueView.aspx?FlowID=" + sID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes')");

                ((Label)e.Item.FindControl("Lb_ProblemNo")).Attributes.Add("onmouseover", "ShowDetailsInfo(this," + DataBinder.Eval(e.Item.DataItem, "Problem_ID").ToString() + ",400);");
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
                Session["FromUrl"] = "../ProbleForms/frmProblemMain.aspx";

                string sUrl = "";
                long lngFlowID = long.Parse(e.Item.Cells[15].Text.Trim());
                sUrl = "../Forms/frmIssueView.aspx?FlowID=" + lngFlowID.ToString();
                Response.Redirect(sUrl);
            }
        }
        #endregion

        #region 高级查询相关

        #region DropSQLwSave_SelectedIndexChanged
        /// <summary>
        /// DropSQLwSave_SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DropSQLwSave_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Master.KeyValue = "请输入问题单号";          //将事件单查询条件清空
            string strTemp = string.Empty;              //临时存放改变后的下拉列表内容
            String strTypeId = String.Empty;

            if (DropSQLwSave.SelectedItem.Text != "==选择收藏查询条件==")
            {
                strTemp = DropSQLwSave.SelectedValue;           //将选择的高级条件名称存储起来
                TypeID = "0";
                //LoadData();
                strTypeId = "0";
                //更新访问次数
                ProblemDealDP.updateCST_ISSUE_WhereNums(Session["UserName"].ToString(), DropSQLwSave.SelectedItem.Text.ToString());
                InitDropSQLwSave();             //更新访问次数后，要重新绑定下拉列表内容，按访问次数排序

                DropSQLwSave.SelectedIndex = DropSQLwSave.Items.IndexOf(DropSQLwSave.Items.FindByValue(strTemp));
                hidSQLName.Value = DropSQLwSave.SelectedItem.Text.ToString();
            }
            else
            {
                hidSQLName.Value = "Temp1";
                strTypeId = "0";

            }

            LoadData(strTypeId);
        }
        #endregion

        #region 初始化高级查询条件
        /// <summary>
        /// 初始化高级查询条件
        /// </summary>
        private void InitDropSQLwSave()
        {
            DataTable dt = ProblemDealDP.getCST_ISSUE_LISTFASTQUERY("frmProblemMain", Session["UserName"].ToString(), string.Empty);
            DropSQLwSave.Items.Clear();
            DropSQLwSave.DataSource = dt.DefaultView;
            DropSQLwSave.DataTextField = "Name";
            DropSQLwSave.DataValueField = "ID";
            DropSQLwSave.DataBind();
            DropSQLwSave.Items.Insert(0, new ListItem("==选择收藏查询条件==", "0"));
        }

        private void InitDropSQLwSave(string SQLName)
        {
            DataTable dt = ProblemDealDP.getCST_ISSUE_LISTFASTQUERY("frmProblemMain", Session["UserName"].ToString(), string.Empty);

            //重新绑定高级查询条件
            DropSQLwSave.Items.Clear();
            DropSQLwSave.DataSource = dt.DefaultView;
            DropSQLwSave.DataTextField = "Name";
            DropSQLwSave.DataValueField = "ID";
            DropSQLwSave.DataBind();
            DropSQLwSave.Items.Insert(0, new ListItem("==选择收藏查询条件==", "0"));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Name"].ToString().Trim() == SQLName.Trim())
                {
                    DropSQLwSave.SelectedIndex = DropSQLwSave.Items.IndexOf(DropSQLwSave.Items.FindByValue(dt.Rows[i]["ID"].ToString().Trim()));
                }
            }
        }
        #endregion

        #region 确定高级条件时执行
        /// <summary>
        /// 确定高级条件时执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void HidButton_Click(object sender, EventArgs e)
        {
            this.Master.KeyValue = "请输入问题单号";

            LoadData();
        }
        #endregion

        #region 根据高级条件名称初始化查询参数
        /// <summary>
        /// 根据高级条件名称初始化查询参数
        /// </summary>
        /// <param name="SQLName"></param>
        protected XmlDocument SqlWhereShow(string SQLName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlRoot = xmlDoc.CreateElement("Fields");
            XmlElement xmlEle;

            DataTable dt = ProblemDealDP.getCST_ISSUE_LISTFASTQUERY("frmProblemMain", Session["UserName"].ToString(), SQLName);

            if (dt.Rows.Count > 0)
            {
                string[] SQLTextDetailList = dt.Rows[0]["SQLText"].ToString().Split(new string[] { "|@@?@$|" }, StringSplitOptions.None);
                this.HiddenColumn.Value = dt.Rows[0]["DISPLAYCOLUMN"].ToString();

                for (int i = 0; i < SQLTextDetailList.Length; i++)
                {
                    string[] SQLTextDetail = SQLTextDetailList[i].Trim().Split('=');

                    #region 问题单号
                    if (SQLTextDetail[0].Trim() == "txtProblemNo.Text")
                    {
                        xmlEle = xmlDoc.CreateElement("Field");
                        xmlEle.SetAttribute("FieldName", "ProblemNo");
                        xmlEle.SetAttribute("Value", SQLTextDetail[1].Trim());
                        xmlRoot.AppendChild(xmlEle);
                    }
                    #endregion

                    #region 资产名称
                    if (SQLTextDetail[0].Trim() == "txtEquName.Text")
                    {
                        xmlEle = xmlDoc.CreateElement("Field");
                        xmlEle.SetAttribute("FieldName", "EquName");
                        xmlEle.SetAttribute("Value", SQLTextDetail[1].Trim());
                        xmlRoot.AppendChild(xmlEle);
                    }
                    #endregion

                    #region 流程状态
                    if (SQLTextDetail[0].Trim() == "cboStatus.SelectedValue")
                    {
                        xmlEle = xmlDoc.CreateElement("Field");
                        xmlEle.SetAttribute("FieldName", "FlowStatus");
                        xmlEle.SetAttribute("Value", SQLTextDetail[1].Trim());
                        xmlRoot.AppendChild(xmlEle);
                    }
                    #endregion

                    #region 问题状态
                    if (SQLTextDetail[0].Trim() == "ddlDealStatus.SelectedValue")
                    {
                        xmlEle = xmlDoc.CreateElement("Field");
                        xmlEle.SetAttribute("FieldName", "Status");
                        xmlEle.SetAttribute("Value", SQLTextDetail[1].Trim());
                        xmlRoot.AppendChild(xmlEle);
                    }
                    #endregion

                    #region 登记时间开始
                    if (SQLTextDetail[0].Trim() == "ctrDateReSetTime.BeginTime")
                    {
                        xmlEle = xmlDoc.CreateElement("Field");
                        xmlEle.SetAttribute("FieldName", "MessageBegin");
                        xmlEle.SetAttribute("Value", SQLTextDetail[1].Trim());
                        xmlRoot.AppendChild(xmlEle);
                    }
                    #endregion

                    #region 登记时间结束
                    if (SQLTextDetail[0].Trim() == "ctrDateReSetTime.EndTime")
                    {
                        xmlEle = xmlDoc.CreateElement("Field");
                        xmlEle.SetAttribute("FieldName", "MessageEnd");
                        xmlEle.SetAttribute("Value", SQLTextDetail[1].Trim());
                        xmlRoot.AppendChild(xmlEle);
                    }
                    #endregion

                    #region 标题
                    if (SQLTextDetail[0].Trim() == "txtSubject.Text")
                    {
                        xmlEle = xmlDoc.CreateElement("Field");
                        xmlEle.SetAttribute("FieldName", "Subject");
                        xmlEle.SetAttribute("Value", SQLTextDetail[1].Trim());
                        xmlRoot.AppendChild(xmlEle);
                    }
                    #endregion

                    #region 问题类别
                    if (SQLTextDetail[0].Trim() == "CataProblemType.CatelogID")
                    {
                        xmlEle = xmlDoc.CreateElement("Field");
                        xmlEle.SetAttribute("FieldName", "CataProblemType");
                        xmlEle.SetAttribute("Value", SQLTextDetail[1].Trim());
                        xmlRoot.AppendChild(xmlEle);
                    }
                    #endregion

                    #region 问题级别
                    if (SQLTextDetail[0].Trim() == "CataProblemLevel.CatelogID")
                    {
                        xmlEle = xmlDoc.CreateElement("Field");
                        xmlEle.SetAttribute("FieldName", "CataProblemLevel");
                        xmlEle.SetAttribute("Value", SQLTextDetail[1].Trim());
                        xmlRoot.AppendChild(xmlEle);
                    }
                    #endregion

                    #region 影响度
                    if (SQLTextDetail[0].Trim() == "CtrFCDEffect.CatelogID")
                    {
                        xmlEle = xmlDoc.CreateElement("Field");
                        xmlEle.SetAttribute("FieldName", "CtrFCDEffect");
                        xmlEle.SetAttribute("Value", SQLTextDetail[1].Trim());
                        xmlRoot.AppendChild(xmlEle);
                    }
                    #endregion

                    #region 紧急度
                    if (SQLTextDetail[0].Trim() == "CtrFCDInstancy.CatelogID")
                    {
                        xmlEle = xmlDoc.CreateElement("Field");
                        xmlEle.SetAttribute("FieldName", "CtrFCDInstancy");
                        xmlEle.SetAttribute("Value", SQLTextDetail[1].Trim());
                        xmlRoot.AppendChild(xmlEle);
                    }
                    #endregion


                    #region 登记人
                    if (SQLTextDetail[0].Trim() == "txtRegUser.Text")
                    {
                        xmlEle = xmlDoc.CreateElement("Field");
                        xmlEle.SetAttribute("FieldName", "txtRegUser");
                        xmlEle.SetAttribute("Value", SQLTextDetail[1].Trim());
                        xmlRoot.AppendChild(xmlEle);
                    }
                    #endregion
                }
            }

            xmlDoc.AppendChild(xmlRoot);
            return xmlDoc;
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
