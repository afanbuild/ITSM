/*******************************************************************
 * 版权所有：
 * Description：事件单（查询）
 * Create By  ：SuperMan
 * Create Date：2011-08-17
 * *****************************************************************/
using System;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Xml;
using System.Drawing;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using EpowerCom;
using EpowerGlobal;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;
using System.Collections.Generic;
using System.Text;
using Epower.ITSM.SqlDAL.Excel;

namespace Epower.ITSM.Web.AppForms
{
    public partial class CST_Issue_List : BasePage
    {
        #region 变量定义

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

        #region 是否投诉单的选择事件单
        /// <summary>
        /// 
        /// </summary>
        protected bool IsSelect
        {
            get { if (Request["IsSelect"] != null) return true; else return false; }
        }
        #endregion

        #region 是否为首页自定义桌面项
        /// <summary>
        /// 
        /// </summary>
        protected bool IsShortCut
        {
            get { if (Request["IsShortCut"] != null) return true; else return false; }
        }
        #endregion

        bool blnDisplayDelay = false;
        bool blnDisplayFeedBack = false;
        RightEntity reTrace = null;         //服务跟踪权限 

        static string staCustInfo = "";
        static string staMsgDateBegion = "";

        #endregion

        #region 判断是否显示延时按钮

        /// <summary>
        /// 判断是否显示延时按钮
        /// </summary>
        /// <param name="status"></param>
        /// <param name="flowdiffMinute"></param>
        /// <returns></returns>
        protected string GetDelayVisible(int status, int flowdiffMinute)
        {
            string strRet = "display:none";

            if (status == (int)e_FlowStatus.efsHandle && blnDisplayDelay == true)
            {
                if (flowdiffMinute < 0)
                {
                    strRet = "";
                }
            }
            return strRet;

        }

        #endregion

        #region 获取按钮名称 详情或详情/评估

        /// <summary>
        /// 获取按钮名称 详情或详情/评估
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        protected string GetButtonValue(int status)
        {
            string strRet = "详情";
            if (status == (int)e_FlowStatus.efsEnd && blnDisplayFeedBack == true)
            {
                strRet = "详情/回访";

            }
            return strRet;
        }

        #endregion

        #region 获取连接页地址

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngNoticeID"></param>
        /// <returns></returns>
        protected string GetUrl(decimal lngFlowID)
        {
            //暂时没处理分页
            string sUrl = "";
            if (IsSelect)
                sUrl = "javascript:window.open('../Forms/frmIssueView.aspx?NewWin=true&FlowID=" + lngFlowID.ToString() + "','','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
            else
                sUrl = "javascript:window.open('../Forms/frmIssueView.aspx?FlowID=" + lngFlowID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
            return sUrl;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        protected string GetDelayUrl(decimal lngFlowID)
        {
            //暂时没处理分页
            string sUrl = "";
            sUrl = "javascript:window.open('../Forms/frmDelayFlow.aspx?flowid=" + lngFlowID.ToString() + "','blank','scrollbars=no,resizable=no,top=screen.availheight-210)/2,left=(screen.availwidth-337)/2,width=337,height=210');";
            return sUrl;
        }

        #endregion

        #region  生成查询XML字符串 GetXmlValueNew1
        /// <summary>
        /// GetXmlValueNew1
        /// </summary>
        /// <returns></returns>
        private XmlDocument GetXmlValueNew1()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlRoot = xmlDoc.CreateElement("Fields");
            XmlElement xmlEle;

            #region CustInfo    客户名称

            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "CustInfo");
            xmlEle.SetAttribute("Value", this.Master.TxtKeyName.Value.Trim().ToString() == "请输入事件单号，客户信息" ? "" : this.Master.TxtKeyName.Value.Trim().ToString());
            xmlRoot.AppendChild(xmlEle);

            #endregion

            #region SericeNo    事件单号

            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "SericeNo");
            xmlEle.SetAttribute("Value", this.Master.TxtKeyName.Value.Trim().ToString() == "请输入事件单号，客户信息" ? "" : this.Master.TxtKeyName.Value.Trim().ToString());
            xmlRoot.AppendChild(xmlEle);

            #endregion

            xmlDoc.AppendChild(xmlRoot);
            return xmlDoc;
        }
        #endregion

        #region 设置父窗体按钮事件SetParentButtonEvent
        /// <summary>
        /// 设置父窗体按钮事件
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_ExportExcel_Click += new Global_BtnClick(Master_Master_Button_ExportExcel_Click);
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.ShowQueryButton(true);
            this.Master.ShowExportExcelButton(true);
            this.Master.ShowNewButton(false);
            this.Master.TxtKeyName.Visible = true;

            this.Master.MainID = "1";     //设置页面的ID编号，如果为查询页面，则设置为1

        }

        #region 申请
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("~/Forms/form_all_flowmodel.aspx?appid=1026");
        }
        #endregion

        #endregion

        #region 导出EXCEL事件Master_Master_Button_ExportExcel_Click
        /// <summary>
        /// 导出EXCEL事件
        /// </summary>
        void Master_Master_Button_ExportExcel_Click()
        {
            string[] key = null;
            string[] value = null;
            string[] arrField = { "ServiceNo", "ServiceType", "MastCustName", "CustName", "contact", "CTel", "CustAddress", "EquipmentName", "CustTime", "ReportingTime", "FinishedTime", "ServiceLevel", "EffectName", "InstancyName", "CloseReasonName", "ReSouseName", "DealStatus", "subject", "Content", "status:GetFlowStatus", "Outtime", "ServiceTime", "Sjwxr", "DealContent", "RegUserName" };
            string[] fileName = { "事件单号", "事件类别", "所属部门", "用户名称", "联系人", "办公电话", "联系地址", "资产名称", "发生时间", "报告时间", "完成时间", "服务级别", "影响度", "紧急度", "关闭理由", "事件来源", "事件状态", "摘要", "详细描述", "流程状态", "派出时间", "上门时间", "工程师", "措施及结果", "登单人" };
            if (!string.IsNullOrEmpty(this.HiddenColumn.Value))
            {
                string[] columnValues = this.HiddenColumn.Value.Split('@');
                if (columnValues.Length > 1)
                {
                    string k = "ServiceNo," + columnValues[0];
                    key = k.Split(',');
                    string v = "事件单号," + columnValues[1];
                    value = v.Split(',');
                }
            }
            switch (TypeID)
            {
                case "0":
                    //高级查询
                    if (!string.IsNullOrEmpty(this.HiddenColumn.Value))
                    {
                        IssueHighExportExcel(value, key);
                    }
                    else
                    {
                        IssueHighExportExcel(fileName, arrField);
                    }

                    break;
                case "1":
                    //我登记事件
                    lkbMy_ExportExcel(fileName, arrField);
                    break;
                case "2":
                    //正在处理
                    lkbProccessing_ExportExcel(fileName, arrField);
                    break;
                case "4":
                    //正常结束
                    lkbProccessed_ExportExcel(fileName, arrField);
                    break;
                case "5":
                    //超时完成
                    lkbOverTimeF_ExportExcel(fileName, arrField);
                    break;
                case "6":
                    //超时未完成
                    lkbOverTimeU_ExportExcel(fileName, arrField);
                    break;
                case "8":
                    //快速搜索
                    IssueExportExcel(fileName, arrField);
                    break;
                case "10"://查询出正在处理的，发生时间为近一个月的权限范围内的记录
                    lkbDefault_ExportExcel(fileName, arrField);
                    break;
                default:
                    IssueExportExcel(fileName, arrField);
                    break;
            }

        }
        #endregion

        #region 查询事件Master_Master_Button_Query_Click
        /// <summary>
        /// 查询事件
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            DropSQLwSave.SelectedIndex = 0;

            //点击查询按钮时，获取查询框中的内容进行查询
            TypeID = "8";
            BindData();

            this.ctrCondition.SetDisplayMode = false;
        }
        #endregion


        #region 页面加载
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            SetParentButtonEvent();

            cpCST_Issue.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(BindData);


            //删除权限
            gridUndoMsg.Columns[gridUndoMsg.Columns.Count - 8].Visible = CheckRight(Constant.admindeleteflow);
            //升级问题单权限
            gridUndoMsg.Columns[gridUndoMsg.Columns.Count - 7].Visible = CheckRight(Constant.QuestionTrace);
            //变更权限
            gridUndoMsg.Columns[gridUndoMsg.Columns.Count - 6].Visible = CheckRight(Constant.EquChangeQuery);

            //获取评估(回访)权限
            blnDisplayFeedBack = CheckRight(Constant.feedbackright);
            //获得服务跟踪权限
            reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.CustomerService];
            Session["FromUrl"] = "../AppForms/CST_Issue_List.aspx";
            FromBackUrl = Session["FromUrl"].ToString();

            string strPara = "";
            if (Request.QueryString["BookMark"] != null)
            {
                strPara = Request.QueryString["BookMark"];
            }
            if (this.IsShortCut == true)
            {
                trShowCondi.Visible = false;
                this.Master.ShowQueryButton(false);
                this.Master.ShowExportExcelButton(false);
                this.Master.ShowNewButton(false);
                this.Master.TxtKeyName.Visible = false;
                trShowControlPage.Visible = false;
                if (Request["tblEmail"] != null && Request["tblEmail"].ToString() == "true")
                {
                    this.tblEmail.Visible = true;
                    this.trShowControlPre.Visible = false;
                    gridUndoMsg.Columns[0].Visible = true;

                }
                string sreturnurl = "../AppForms/CST_Issue_List.aspx";
                if (Session["Themes"].ToString() == "StandardThemes")  //标准风格
                {
                    sreturnurl = "../NewMainPage/NewmainDefine.aspx";
                }
                else if (Session["Themes"].ToString() == "TraditionThemes")                                    //传统风格
                {
                    sreturnurl = "../OldMainPage/NewmainDefine.aspx";
                }
                else if (Session["Themes"].ToString() == "Standard2Themes")                                    //清新风格
                {
                    sreturnurl = "../New2MainPage/NewmainDefine.aspx";
                }

                if (strPara != string.Empty)
                {
                    if (Request["tblEmail"] != null && Request["tblEmail"].ToString() == "true")
                    {
                        sreturnurl = "../AppForms/CST_Issue_List.aspx?BookMark=7&IsShortCut=0&tblEmail=true";
                    }
                    else
                    {
                        sreturnurl = "../AppForms/CST_Issue_List.aspx?BookMark=7&IsShortCut=0";
                    }
                }

                Session["FromUrl"] = sreturnurl;
                FromBackUrl = Session["FromUrl"].ToString();
            }

            if (!Page.IsPostBack)
            {
                //SetHeaderText();

                this.Master.KeyValue = "请输入事件单号，客户信息";
                hidUserID.Value = ((long)(Session["UserID"])).ToString();

                InitDropSQLwSave();  //初始化查询条件              

                //快速搜索
                if (Request["svalue"] != null)
                {
                    staCustInfo = Request["svalue"].ToString().Trim();
                }

                ////查询条件赋值
                //Control[] arrControl = { tbTest };
                //PageDeal.SetPageQueryParam(arrControl, cpCST_Issue, "CST_Issue_List");

                #region 装载高级查询条件

                if (hidSQLName.Value != "" && hidSQLName.Value != "==选择收藏查询条件==")
                {
                    if (hidSQLName.Value != "Temp1")
                    {
                        DataTable dt = ZHServiceDP.getCST_ISSUE_Where("CST_Issue_List", Session["UserName"].ToString(), hidSQLName.Value.ToString().Trim());

                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["SQLText"].ToString().Trim() == dt.Rows[1]["SQLText"].ToString().Trim())
                            {
                                #region 如果为原条件
                                if (dt.Rows[0]["Name"].ToString().Trim() != "Temp1")
                                {
                                    //更新访问次数
                                    ZHServiceDP.updateCST_ISSUE_WhereNums(Session["UserName"].ToString(), dt.Rows[0]["Name"].ToString().Trim());

                                    hidSQLName.Value = dt.Rows[0]["Name"].ToString().Trim();
                                    TypeID = "0";
                                }

                                if (dt.Rows[1]["Name"].ToString().Trim() != "Temp1")
                                {
                                    //更新访问次数
                                    ZHServiceDP.updateCST_ISSUE_WhereNums(Session["UserName"].ToString(), dt.Rows[1]["Name"].ToString().Trim());

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

                switch (strPara)
                {
                    case "":
                        BindData();
                        break;
                    case "1":
                        //由我登记
                        lkbMy_Click(this.lkbMy, System.EventArgs.Empty);
                        break;
                    case "2":
                        //正在处理
                        lkbProccessing_Click(this.lkbProccessing, System.EventArgs.Empty);
                        break;
                    case "4":
                        //正常结束
                        lkbProcessed_Click(this.lkbProcessed, System.EventArgs.Empty);
                        break;
                    case "5":
                        //超时完成
                        lkbOverTimeF_Click(this.lkbOverTimeF, System.EventArgs.Empty);
                        break;
                    case "6":
                        //超时未完成
                        lkbOverTimeU_Click(this.lkbOverTimeU, System.EventArgs.Empty);
                        break;
                    case "7":
                        //未回访
                        lkbUnFeedBack_Click(this.lkbProcessed, System.EventArgs.Empty);
                        break;
                    default:
                        BindData();
                        break;

                }

                if (Request["IsDesk"] != null)
                {
                    //是否从左面进来
                    gridUndoMsg.Columns[7].Visible = false;
                    gridUndoMsg.Columns[8].Visible = false;

                    gridUndoMsg.Columns[gridUndoMsg.Columns.Count - 6].Visible = false;
                    gridUndoMsg.Columns[gridUndoMsg.Columns.Count - 7].Visible = false;
                    gridUndoMsg.Columns[gridUndoMsg.Columns.Count - 8].Visible = false;
                }


                if (IsSelect)
                {
                    gridUndoMsg.Columns[gridUndoMsg.Columns.Count - 6].Visible = false;
                    gridUndoMsg.Columns[gridUndoMsg.Columns.Count - 7].Visible = false;
                    gridUndoMsg.Columns[gridUndoMsg.Columns.Count - 8].Visible = false;
                    gridUndoMsg.Columns[0].Visible = true;
                    TableSelect.Visible = true;
                }
            }
            else
            {
                #region 装载高级查询条件

                if (hidIsGaoji.Value == "1")
                {
                    //当值为1时，表明从高级条件界面传过来的，此时应将hidSQLName的值传给DropSQLwSave

                    InitDropSQLwSave1(hidSQLName.Value);

                    #region
                    if (hidSQLName.Value != "" && hidSQLName.Value != "==选择收藏查询条件==")
                    {
                        if (hidSQLName.Value != "Temp1")
                        {
                            //表明此时高级条件下拉列表选中的是自己定义的一个高级条件
                            DataTable dt = ZHServiceDP.getCST_ISSUE_Where("CST_Issue_List", Session["UserName"].ToString(), hidSQLName.Value.ToString().Trim());

                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Rows[0]["SQLText"].ToString().Trim() == dt.Rows[1]["SQLText"].ToString().Trim())
                                {
                                    //如果完全相等，表明此时所用的查询条件，就是当前选中的高级条件名称；否则，是在当前选中的高级条件名称基础上新增了N条查询条件，且只是临时查询
                                    #region 如果为原条件
                                    if (dt.Rows[0]["Name"].ToString().Trim() != "Temp1")
                                    {
                                        //更新访问次数
                                        ZHServiceDP.updateCST_ISSUE_WhereNums(Session["UserName"].ToString(), dt.Rows[0]["Name"].ToString().Trim());

                                        hidSQLName.Value = dt.Rows[0]["Name"].ToString().Trim();
                                        TypeID = "0";
                                    }

                                    if (dt.Rows[1]["Name"].ToString().Trim() != "Temp1")
                                    {
                                        //更新访问次数
                                        ZHServiceDP.updateCST_ISSUE_WhereNums(Session["UserName"].ToString(), dt.Rows[1]["Name"].ToString().Trim());

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

                #endregion



            }

            //保存查询条件
            Control[] arrControl1 = { tbTest };
            PageDeal.GetPageQueryParam(arrControl1, cpCST_Issue, "CST_Issue_List");

            #region 动态查询: 设置动态查询参数 - 2013-06-27 @孙绍棕

            this.ctrCondition.TableName = "cst_issue";
            this.ctrCondition.mybtnSelectOnClick += new EventHandler(ctrCondition1_mybtnSelectOnClick);
            this.ctrCondition.mySelectedIndexChanged += new EventHandler(ctrCondition1_mySelectedIndexChanged);


            if (IsPostBack)
            {
                if (this.ctrCondition.IsOpen)
                {
                    TypeID = "0";    // 高级查询
                    BindData();
                }
            }
            #endregion
        }


        #region 动态查询: 下拉框选择不同的查询条件组合时触发 - 2013-06-27 @孙绍棕

        /// <summary>
        /// 下拉框选择不同的查询条件组合时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ctrCondition1_mySelectedIndexChanged(object sender, EventArgs e)
        {
            ctrCondition.ddlSelectChanged();

            TypeID = "0";
            BindData();

            this.Master.TxtKeyName.Value = "请输入事件单号，客户信息";


            lkbProccessing.ForeColor = Color.Black;
            lkbMy.ForeColor = Color.Black;
            lkbProcessed.ForeColor = Color.Black;
            lkbOverTimeF.ForeColor = Color.Black;
            lkbOverTimeU.ForeColor = Color.Black;
        }

        #endregion

        #region 动态查询: 点击动态查询按钮时触发 - 2013-06-27 @孙绍棕

        /// <summary>
        /// 点击动态查询按钮时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ctrCondition1_mybtnSelectOnClick(object sender, EventArgs e)
        {
            ctrCondition.Bind();

            TypeID = "0";
            BindData();

            this.Master.TxtKeyName.Value = "请输入事件单号，客户信息";


            lkbProccessing.ForeColor = Color.Black;
            lkbMy.ForeColor = Color.Black;
            lkbProcessed.ForeColor = Color.Black;
            lkbOverTimeF.ForeColor = Color.Black;
            lkbOverTimeU.ForeColor = Color.Black;

            //this.Master.KeyValue = "请输入事件单号，客户信息";          //将事件单查询条件清空
            //string strTemp = string.Empty;              //临时存放改变后的下拉列表内容

            //if (DropSQLwSave.SelectedItem.Text != "==选择收藏查询条件==")
            //{
            //    strTemp = DropSQLwSave.SelectedValue;           //将选择的高级条件名称存储起来
            //    TypeID = "0";
            //    BindData();

            //    //更新访问次数
            //    ZHServiceDP.updateCST_ISSUE_WhereNums(Session["UserName"].ToString(), DropSQLwSave.SelectedItem.Text.ToString());
            //    InitDropSQLwSave();             //更新访问次数后，要重新绑定下拉列表内容，按访问次数排序

            //    DropSQLwSave.SelectedIndex = DropSQLwSave.Items.IndexOf(DropSQLwSave.Items.FindByValue(strTemp));
            //    hidSQLName.Value = DropSQLwSave.SelectedItem.Text.ToString();
            //}
            //else
            //{
            //    hidSQLName.Value = "Temp1";
            //    TypeID = "0";
            //    BindData();
            //}
        }

        #endregion

        /// <summary>
        /// 设置列头名称 廖世进 2013-05-16
        /// </summary>
        //void SetHeaderText()
        //{
        //    gridUndoMsg.Columns[1].HeaderText = PageDeal.GetLanguageValue("CST_ServiceNO");
        //    gridUndoMsg.Columns[2].HeaderText = PageDeal.GetLanguageValue("CST_CustName");
        //    gridUndoMsg.Columns[3].HeaderText = PageDeal.GetLanguageValue("CST_CustPhone");
        //    gridUndoMsg.Columns[4].HeaderText = PageDeal.GetLanguageValue("CST_ServiceLevel");
        //    gridUndoMsg.Columns[5].HeaderText = PageDeal.GetLanguageValue("CST_CustTime");
        //    gridUndoMsg.Columns[6].HeaderText = PageDeal.GetLanguageValue("CST_EquName");
        //    gridUndoMsg.Columns[7].HeaderText = PageDeal.GetLanguageValue("CST_Subject");
        //    gridUndoMsg.Columns[8].HeaderText = PageDeal.GetLanguageValue("CST_Content");
        //    gridUndoMsg.Columns[9].HeaderText = PageDeal.GetLanguageValue("CST_DealStatus");
        //    gridUndoMsg.Columns[10].HeaderText = PageDeal.GetLanguageValue("CST_ResponseTime");
        //    gridUndoMsg.Columns[11].HeaderText = PageDeal.GetLanguageValue("CST_OperaTime");
        //    gridUndoMsg.Columns[12].HeaderText = PageDeal.GetLanguageValue("CST_CurrentOpUser");

        //}
        #endregion

        #region 跟踪快速书签导出EXCEL代码段
        /// <summary>
        /// 我登记事件
        /// </summary>
        protected void lkbMy_ExportExcel(string[] fileName, string[] arrFlide)
        {
            eOA_TracePeriod tp = (eOA_TracePeriod)int.Parse(ddlPeriod.SelectedValue);
            DataTable dt = ZHServiceDP.GetIssuesForMy(long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString())
           , long.Parse(Session["UserOrgID"].ToString()), tp, reTrace);

            Epower.ITSM.Web.Common.ExcelExport.ExportIssuesList1(this, dt, fileName, arrFlide, Session["UserID"].ToString());
        }

        /// <summary>
        /// 正在处理
        /// </summary>
        protected void lkbProccessing_ExportExcel(string[] fileName, string[] arrFlide)
        {
            eOA_TracePeriod tp = (eOA_TracePeriod)int.Parse(ddlPeriod.SelectedValue);
            DataTable dt = ZHServiceDP.GetIssuesForHandle(long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString())
           , long.Parse(Session["UserOrgID"].ToString()), tp, reTrace);
            Epower.ITSM.Web.Common.ExcelExport.ExportIssuesList1(this, dt, fileName, arrFlide, Session["UserID"].ToString());
        }

        /// <summary>
        /// 默认进入时，查询正在处理的，一个月内的权限范围内的记录
        /// </summary>
        protected void lkbDefault_ExportExcel(string[] fileName, string[] arrFlide)
        {
            DataTable dt = ZHServiceDP.GetIssuesForCondNew_Init(string.Empty, long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()), long.Parse(Session["UserOrgID"].ToString()), reTrace);
            Epower.ITSM.Web.Common.ExcelExport.ExportIssuesList1(this, dt, fileName, arrFlide, Session["UserID"].ToString());
        }

        /// <summary>
        /// 超时完成
        /// </summary>
        protected void lkbOverTimeF_ExportExcel(string[] fileName, string[] arrFlide)
        {
            eOA_TracePeriod tp = (eOA_TracePeriod)int.Parse(ddlPeriod.SelectedValue);
            //dt = ee.GetIssuesForOverTime(long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()), long.Parse(Session["UserOrgID"].ToString()), tp, true, reTrace, this.cpCST_Issue.PageSize, this.cpCST_Issue.CurrentPage, ref iRowCount);

            int iRowCount = 0;

            ZHServiceDP ee = new ZHServiceDP();
            DataTable dt = ee.GetIssuesForOverTime(long.Parse(Session["UserID"].ToString()),
                long.Parse(Session["UserDeptID"].ToString()),
                long.Parse(Session["UserOrgID"].ToString()),
                tp,
                true,
                reTrace,
                int.MaxValue,
                1,
                ref iRowCount);

            Epower.ITSM.Web.Common.ExcelExport.ExportIssuesList1(this, dt, fileName, arrFlide, Session["UserID"].ToString());
        }

        /// <summary>
        /// 超时未完成
        /// </summary>
        protected void lkbOverTimeU_ExportExcel(string[] fileName, string[] arrFlide)
        {
            eOA_TracePeriod tp = (eOA_TracePeriod)int.Parse(ddlPeriod.SelectedValue);
            // DataTable dt = ZHServiceDP.GetIssuesForOverTime(long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString())
            //, long.Parse(Session["UserOrgID"].ToString()), tp, false, reTrace);

            int iRowCount = 0;

            ZHServiceDP ee = new ZHServiceDP();
            DataTable dt = ee.GetIssuesForOverTime(long.Parse(Session["UserID"].ToString()),
                long.Parse(Session["UserDeptID"].ToString()),
                long.Parse(Session["UserOrgID"].ToString()),
                tp,
                false,
                reTrace,
                int.MaxValue,
                1,
                ref iRowCount);            

            Epower.ITSM.Web.Common.ExcelExport.ExportIssuesList1(this, dt, fileName, arrFlide, Session["UserID"].ToString());
        }

        /// <summary>
        /// 正常结束
        /// </summary>
        protected void lkbProccessed_ExportExcel(string[] fileName, string[] arrFlide)
        {
            eOA_TracePeriod tp = (eOA_TracePeriod)int.Parse(ddlPeriod.SelectedValue);
            DataTable dt = ZHServiceDP.GetIssuesForEnd(long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString())
           , long.Parse(Session["UserOrgID"].ToString()), tp, reTrace);
            Epower.ITSM.Web.Common.ExcelExport.ExportIssuesList1(this, dt, fileName, arrFlide, Session["UserID"].ToString());
        }

        #endregion

        #region 跟踪快速书签代码段
        /// <summary>
        /// 由我登记
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbMy_Click(object sender, EventArgs e)
        {
            Session["FromUrl"] = "../AppForms/CST_Issue_List.aspx?BookMark=1";
            FromBackUrl = Session["FromUrl"].ToString();
            TypeID = "1";
            BindData();
            ChangeButtonFontColor(lkbMy, lkbProccessing, lkbProcessed, lkbOverTimeF, lkbOverTimeU);
            //==zxl

        }
        /// <summary>
        /// zxl 改变按钮点击颜色
        /// </summary>
        private void ChangeButtonFontColor(LinkButton linkOne, LinkButton lkbTwo, LinkButton lkbThree, LinkButton lkbFor, LinkButton lkbTu)
        {
            linkOne.ForeColor = Color.Red;
            lkbTwo.ForeColor = Color.Black;
            lkbThree.ForeColor = Color.Black;
            lkbFor.ForeColor = Color.Black;
            lkbTu.ForeColor = Color.Black;
        }

        /// <summary>
        /// 正在处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbProccessing_Click(object sender, EventArgs e)
        {
            Session["FromUrl"] = "../AppForms/CST_Issue_List.aspx?BookMark=2";
            FromBackUrl = Session["FromUrl"].ToString();
            TypeID = "2";
            BindData();
            ChangeButtonFontColor(lkbProccessing, lkbMy, lkbProcessed, lkbOverTimeF, lkbOverTimeU);

        }

        /// <summary>
        /// 正常结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbProcessed_Click(object sender, EventArgs e)
        {
            TypeID = "4";
            BindData();
            ChangeButtonFontColor(lkbProcessed, lkbProccessing, lkbMy, lkbOverTimeF, lkbOverTimeU);

        }

        /// <summary>
        /// 超时完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbOverTimeF_Click(object sender, EventArgs e)
        {
            Session["FromUrl"] = "../AppForms/CST_Issue_List.aspx?BookMark=5";
            FromBackUrl = Session["FromUrl"].ToString();
            TypeID = "5";
            BindData();
            ChangeButtonFontColor(lkbOverTimeF, lkbProccessing, lkbProcessed, lkbMy, lkbOverTimeU);
        }

        /// <summary>
        /// 超时未完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbOverTimeU_Click(object sender, EventArgs e)
        {
            //===zxl
            ChangeButtonFontColor(lkbOverTimeU, lkbProccessing, lkbProcessed, lkbOverTimeF, lkbMy);
            //===
            Session["FromUrl"] = "../AppForms/CST_Issue_List.aspx?BookMark=6";
            FromBackUrl = Session["FromUrl"].ToString();
            TypeID = "6";
            BindData();

        }

        /// <summary>
        /// 未回访
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbUnFeedBack_Click(object sender, EventArgs e)
        {
            TypeID = "7";
            BindData();
        }

        #endregion

        #region 检查权限 CheckRight
        /// <summary>
        /// 检查权限
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

        #region 删除流程gridUndoMsg_DeleteCommand
        /// <summary>
        /// 删除流程
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void gridUndoMsg_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            BindData();
        }
        #endregion

        #region 窗体按钮事件

        protected void gridUndoMsg_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            e_FlowStatus fs;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                //当超过整个流程预计处理时限未处理的，红低显示
                fs = (e_FlowStatus)int.Parse(e.Item.Cells[gridUndoMsg.Columns.Count - 5].Text);

                if (int.Parse(e.Item.Cells[gridUndoMsg.Columns.Count - 4].Text.Trim()) < 0 && fs != e_FlowStatus.efsEnd)
                {
                    for (int i = 0; i < e.Item.Cells.Count; i++)
                    {
                        e.Item.Cells[i].ForeColor = Color.Red;
                    }
                }

                if (DataBinder.Eval(e.Item.DataItem, "ChangeProblemFlowID").ToString() == "0" || DataBinder.Eval(e.Item.DataItem, "ChangeProblemFlowID").ToString() == string.Empty)   //已升级的问题单不能再升级
                {
                    Button btnChange = (Button)e.Item.FindControl("btnChange");
                    Label lblChange = (Label)e.Item.FindControl("lblChange");
                    btnChange.Visible = true;
                    lblChange.Visible = false;
                }
                else
                {
                    Button btnChange = (Button)e.Item.FindControl("btnChange");
                    Label lblChange = (Label)e.Item.FindControl("lblChange");
                    btnChange.Visible = false;
                    lblChange.Visible = true;
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
                if (e.Item.FindControl("Lb_ServiceNo") != null)
                    ((Label)e.Item.FindControl("Lb_ServiceNo")).Attributes.Add("onmouseover", "ShowDetailsInfo(this," + DataBinder.Eval(e.Item.DataItem, "SMSID").ToString() + ",400);");

                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                string sFlowID = DataBinder.Eval(e.Item.DataItem, "FlowID").ToString();
                e.Item.CssClass = "hand";
                e.Item.Attributes.Add("ondblclick", "SetUrl();window.open('../Forms/frmIssueView.aspx?FlowID=" + sFlowID.ToString() + "&randomid='+GetRandom(),'MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");

                #region 新增2个显示字段 时间差
                DataTable dt = ZHServiceDP.GetFlowBusLimitByFlowID(decimal.Parse(sFlowID == "" ? "0" : sFlowID));
                DateTime outtime = DataBinder.Eval(e.Item.DataItem, "Outtime").ToString() == "" ? DateTime.Now : DateTime.Parse(DataBinder.Eval(e.Item.DataItem, "Outtime").ToString());
                DateTime FinishedTime = DataBinder.Eval(e.Item.DataItem, "FinishedTime").ToString() == "" ? DateTime.Now : DateTime.Parse(DataBinder.Eval(e.Item.DataItem, "FinishedTime").ToString());

                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DateTime LimitTime = dt.Rows[i]["LimitTime"].ToString() == "" ? DateTime.Now : DateTime.Parse(dt.Rows[i]["LimitTime"].ToString());
                        TimeSpan ts1 = LimitTime.Subtract(FinishedTime);
                        TimeSpan ts2 = LimitTime.Subtract(outtime);
                        //ts2 = ts1;
                        if (dt.Rows[i]["GuidID"].ToString() == "10001")
                        {
                            string str1 = "";
                            if (ts1.Days != 0)
                                str1 += (ts1.Days > 0 ? ts1.Days : -ts1.Days) + "天";
                            if (ts1.Hours != 0)
                                str1 += (ts1.Hours > 0 ? ts1.Hours : -ts1.Hours) + "小时";
                            if (ts1.Minutes != 0)
                                str1 += (ts1.Minutes > 0 ? ts1.Minutes : -ts1.Minutes) + "分钟";
                            if (ts1.Seconds != 0)
                                str1 += (ts1.Seconds > 0 ? ts1.Seconds : -ts1.Seconds) + "秒";

                            if (ts1.Days < 0 || ts1.Hours < 0 || ts1.Minutes < 0 || ts1.Seconds < 0)
                            {
                                ((Label)e.Item.FindControl("txtNew1")).Text = "超" + str1;
                                ((Label)e.Item.FindControl("txtNew1")).ForeColor = Color.Red;
                            }
                            else
                            {
                                ((Label)e.Item.FindControl("txtNew1")).Text = "还剩" + str1;
                            }
                        }
                        if (dt.Rows[i]["GuidID"].ToString() == "10002")
                        {
                            string str2 = "";
                            if (ts2.Days != 0)
                                str2 += (ts2.Days > 0 ? ts2.Days : -ts2.Days) + "天";
                            if (ts2.Hours != 0)
                                str2 += (ts2.Hours > 0 ? ts2.Hours : -ts2.Hours) + "小时";
                            if (ts2.Minutes != 0)
                                str2 += (ts2.Minutes > 0 ? ts2.Minutes : -ts2.Minutes) + "分钟";
                            if (ts2.Seconds != 0)
                            {
                                str2 += (ts2.Seconds > 0 ? ts2.Seconds : -ts2.Seconds) + "秒";
                            }

                            if (ts2.Days < 0 || ts2.Hours < 0 || ts2.Minutes < 0 || ts2.Seconds < 0)
                            {
                                ((Label)e.Item.FindControl("txtNew2")).Text = "超" + str2;
                                ((Label)e.Item.FindControl("txtNew2")).ForeColor = Color.Red;
                            }
                            else
                            {
                                ((Label)e.Item.FindControl("txtNew2")).Text = "还剩" + str2;
                            }
                        }
                    }
                }
                else
                {
                    Label txtNew1 = ((Label)e.Item.FindControl("txtNew1")) as Label;
                    Label txtNew2 = ((Label)e.Item.FindControl("txtNew2")) as Label;
                    if (txtNew1 != null)
                    {
                        txtNew1.Text = "";
                    }

                    if (txtNew2 != null)
                    {
                        txtNew2.Text = "";
                    }
                }
                Label txtNew3 = ((Label)e.Item.FindControl("txtNew1")) as Label;
                Label txtNew4 = ((Label)e.Item.FindControl("txtNew2")) as Label;

                if (txtNew3 != null)
                {
                    txtNew3.Font.Bold = true;
                }

                if (txtNew4 != null)
                {
                    txtNew4.Font.Bold = true;
                }
                #endregion
            }
        }

        protected void gridUndoMsg_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (this.IsShortCut == false)
            {
                if (e.Item.ItemType == ListItemType.Header)
                {
                    DataGrid dg = (DataGrid)sender;
                    for (int i = 0; i < e.Item.Cells.Count; i++)
                    {
                        if (i > 0 && i < 11)
                        {
                            int j = i;
                            e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                        }
                    }
                }
            }
            else
            {
                if (Request["tblEmail"] != null && Request["tblEmail"].ToString() == "true")
                {
                    if (e.Item.ItemType == ListItemType.Header)
                    {
                        DataGrid dg = (DataGrid)sender;
                        for (int i = 0; i < e.Item.Cells.Count; i++)
                        {
                            if (i > 0 && i < 11)
                            {
                                int j = i;
                                e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                            }
                        }
                    }
                }
            }
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (DataGridItem itm in gridUndoMsg.Items)
            {
                if (itm.ItemType == ListItemType.AlternatingItem ||
                    itm.ItemType == ListItemType.Item)
                {
                    string sID = itm.Cells[gridUndoMsg.Columns.Count - 5].Text;
                    CheckBox chkdel = (CheckBox)itm.Cells[0].FindControl("chkDel");
                    if (chkdel.Checked)
                    {
                        sb.Append(sID + ",");
                    }
                }
            }
            System.Text.StringBuilder sbText = new System.Text.StringBuilder();
            sbText.Append("<script>");
            sbText.Append("var arr = new Array();");
            // ID
            sbText.Append("arr[0] ='" + sb.ToString() + "';");
            sbText.Append("window.parent.returnValue = arr;");
            // 关闭窗口
            sbText.Append("top.close();");
            sbText.Append("</script>");
            // 向客户端发送
            Page.RegisterStartupScript(DateTime.Now.ToString(), sbText.ToString());
            Response.Write(sbText.ToString());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClose_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sbText = new System.Text.StringBuilder();
            sbText.Append("<script>");
            // 关闭窗口
            sbText.Append("top.close();");
            sbText.Append("</script>");
            // 向客户端发送
            Page.RegisterStartupScript(DateTime.Now.ToString(), sbText.ToString());
            Response.Write(sbText.ToString());
        }

        #region 批量邮件回访
        /// 批量邮件回访
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSendMail_Click1(object sender, EventArgs e)
        {
            int i = 0;
            foreach (DataGridItem itm in gridUndoMsg.Items)
            {

                if (itm.ItemType == ListItemType.AlternatingItem ||
                    itm.ItemType == ListItemType.Item)
                {
                    long lngFlowID = long.Parse(itm.Cells[gridUndoMsg.Columns.Count - 5].Text);
                    int status = int.Parse(itm.Cells[gridUndoMsg.Columns.Count - 1].Text);
                    CheckBox chkdel = (CheckBox)itm.Cells[0].FindControl("chkDel");

                    if (chkdel.Checked && status == (int)e_FlowStatus.efsEnd)
                    {
                        //发送邮件

                        string sEmail = string.Empty;
                        string sCustName = string.Empty;
                        string sSubject = CommonDP.GetConfigValue("Other", "ServiceAutoFeedBackTitle");
                        string strSQL = "SELECT a.Subject,b.Email,b.ShortName FROM Cst_Issues a,Br_ECustomer b WHERE a.CustID=b.ID And a.FlowID=" + lngFlowID.ToString();
                        DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
                        foreach (DataRow dr in dt.Rows)
                        {
                            sEmail = dr["Email"].ToString();
                            sCustName = dr["ShortName"].ToString();
                            sSubject += dr["Subject"].ToString();
                            break;
                        }


                        if (sEmail != string.Empty)
                        {
                            MailSendDeal.EmailFeedBack(lngFlowID, sEmail, sCustName, sSubject);
                            ZHServiceDP.UpdateEmailState(lngFlowID);
                            i++;
                        }

                    }
                }
            }
            if (i > 0)
            {
                lkbUnFeedBack_Click(null, null);
                Epower.DevBase.BaseTools.PageTool.MsgBox(this, "批量邮件回访成功，共回访了" + i.ToString() + "个事件单！");
            }
            else
            {
                Epower.DevBase.BaseTools.PageTool.MsgBox(this, "未选择回访记录！");
            }
        }
        #endregion

        protected void DropSQLwSave_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Master.KeyValue = "请输入事件单号，客户信息";          //将事件单查询条件清空
            string strTemp = string.Empty;              //临时存放改变后的下拉列表内容

            if (DropSQLwSave.SelectedItem.Text != "==选择收藏查询条件==")
            {
                strTemp = DropSQLwSave.SelectedValue;           //将选择的高级条件名称存储起来
                TypeID = "0";
                BindData();

                //更新访问次数
                ZHServiceDP.updateCST_ISSUE_WhereNums(Session["UserName"].ToString(), DropSQLwSave.SelectedItem.Text.ToString());
                InitDropSQLwSave();             //更新访问次数后，要重新绑定下拉列表内容，按访问次数排序

                DropSQLwSave.SelectedIndex = DropSQLwSave.Items.IndexOf(DropSQLwSave.Items.FindByValue(strTemp));
                hidSQLName.Value = DropSQLwSave.SelectedItem.Text.ToString();
            }
            else
            {
                hidSQLName.Value = "Temp1";
                TypeID = "0";
                BindData();
            }
        }

        #endregion

        #region 自定义方法

        #region InitDropDown 初始化高级查询条件
        private void InitDropSQLwSave()
        {
            DataTable dt = ZHServiceDP.getCST_ISSUE_LISTFASTQUERY("CST_Issue_List", Session["UserName"].ToString(), string.Empty);
            DropSQLwSave.Items.Clear();
            DropSQLwSave.DataSource = dt.DefaultView;
            DropSQLwSave.DataTextField = "Name";
            DropSQLwSave.DataValueField = "ID";
            DropSQLwSave.DataBind();
            DropSQLwSave.Items.Insert(0, new ListItem("==选择收藏查询条件==", "0"));
        }

        private void InitDropSQLwSave1(string SQLName)
        {
            DataTable dt = ZHServiceDP.getCST_ISSUE_LISTFASTQUERY("CST_Issue_List", Session["UserName"].ToString(), string.Empty);

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

        #region 快速搜索导出

        /// <summary>
        /// 快速搜索导出
        /// </summary>
        private void IssueExportExcel(string[] fileName, string[] arrFlide)
        {
            XmlDocument xmlDoc = GetXmlValueNew1();
            DataTable dt = ZHServiceDP.GetIssuesForCondNew1(xmlDoc.InnerXml, long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()), long.Parse(Session["UserOrgID"].ToString()), reTrace);
            Epower.ITSM.Web.Common.ExcelExport.ExportIssuesList1(this, dt, fileName, arrFlide, Session["UserID"].ToString());
        }

        #endregion

        #region 高级查询导出

        private void IssueHighExportExcel(string[] fileName, string[] arrFlide)
        {
            DataTable dt = ZHServiceDP.GetIssuesForCond(this.ctrCondition.strCondition, long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()), long.Parse(Session["UserOrgID"].ToString()), reTrace);


            Epower.ITSM.Web.Common.ExcelExport.ExportIssuesList1(this, dt, fileName, arrFlide, Session["UserID"].ToString());
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
            this.Master.KeyValue = "请输入事件单号，客户信息";

            BindData();
        }
        #endregion

        #region 数据绑定

        /// <summary>
        /// 数据绑定
        /// </summary>
        private void BindData()
        {
            int iRowCount = 0;
            eOA_TracePeriod tp = (eOA_TracePeriod)int.Parse(ddlPeriod.SelectedValue);
            ZHServiceDP ee = new ZHServiceDP();
            DataTable dt = null;


            this.ctrCondition.SetDisplayMode = false;

            switch (TypeID)
            {
                case "1":
                    //我登记事件
                    dt = ee.GetIssuesForMy(long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()), long.Parse(Session["UserOrgID"].ToString()), tp, reTrace, this.cpCST_Issue.PageSize, this.cpCST_Issue.CurrentPage, ref iRowCount);
                    break;
                case "2":
                    //正在处理
                    dt = ee.GetIssuesForHandle(long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()), long.Parse(Session["UserOrgID"].ToString()), tp, reTrace, this.cpCST_Issue.PageSize, this.cpCST_Issue.CurrentPage, ref iRowCount);
                    break;
                case "4":
                    //正常结束
                    dt = ee.GetIssuesForEnd(long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()), long.Parse(Session["UserOrgID"].ToString()), tp, reTrace, this.cpCST_Issue.PageSize, this.cpCST_Issue.CurrentPage, ref iRowCount);
                    break;
                case "5":
                    //超时已完成
                    dt = ee.GetIssuesForOverTime(long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()), long.Parse(Session["UserOrgID"].ToString()), tp, true, reTrace, this.cpCST_Issue.PageSize, this.cpCST_Issue.CurrentPage, ref iRowCount);
                    break;
                case "6":
                    //超时未完成
                    dt = ee.GetIssuesForOverTime(long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()), long.Parse(Session["UserOrgID"].ToString()), tp, false, reTrace, this.cpCST_Issue.PageSize, this.cpCST_Issue.CurrentPage, ref iRowCount);
                    break;
                case "7":
                    if (this.IsShortCut == true)
                    {
                        dt = ee.GetIssuesForUnFeedBack(long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()), long.Parse(Session["UserOrgID"].ToString()), tp, reTrace, 100000000, 1, ref iRowCount, int.Parse(ddltEmail.SelectedValue));
                    }
                    else
                    {
                        Session["FromUrl"] = "../AppForms/CST_Issue_List.aspx?BookMark=7";
                        dt = ee.GetIssuesForUnFeedBack(long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()), long.Parse(Session["UserOrgID"].ToString()), tp, reTrace, this.cpCST_Issue.PageSize, this.cpCST_Issue.CurrentPage, ref iRowCount, int.Parse(ddltEmail.SelectedValue));
                    }
                    break;
                case "8":
                    Session["Issue_List_Excel"] = "";
                    XmlDocument xmlDocnew1 = GetXmlValueNew1();
                    dt = ee.GetIssuesForCondNew1(xmlDocnew1.InnerXml, long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()), long.Parse(Session["UserOrgID"].ToString()), reTrace, this.cpCST_Issue.PageSize, this.cpCST_Issue.CurrentPage, ref iRowCount);
                    break;
                case "10"://查询出正在处理的，发生时间为近一个月的权限范围内的记录
                    dt = ee.GetIssuesForCondNew_Init("", long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()), long.Parse(Session["UserOrgID"].ToString()), reTrace, this.cpCST_Issue.PageSize, this.cpCST_Issue.CurrentPage, ref iRowCount);
                    break;
                default://高级查询

                    this.ctrCondition.SetDisplayMode = true;
                    hidIsGaoji.Value = "0";       //将值还原回0
                    Session["Issue_List_Excel"] = "";

                    // sunshaozong
                    dt = ee.GetIssuesForCond(this.ctrCondition.strCondition, long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()), long.Parse(Session["UserOrgID"].ToString()), reTrace, this.cpCST_Issue.PageSize, this.cpCST_Issue.CurrentPage, ref iRowCount);
                    break;
            }
            
            gridUndoMsg.DataSource = dt;
            gridUndoMsg.DataBind();
            this.cpCST_Issue.RecordCount = iRowCount;
            this.cpCST_Issue.Bind();
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddltEmail_SelectedIndexChanged(object sender, EventArgs e)
        {
            lkbUnFeedBack_Click(null, null);
        }

        #region  设置显示字段

      

        protected void Button1_Click(object sender, EventArgs e)
        {
            string ConfigFile = HttpRuntime.AppDomainAppPath + "Config\\ExcelConfig.xml";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(ConfigFile);
            XmlNode noderoot = xmlDoc.DocumentElement.SelectSingleNode("Items[@name='Excel']");
            string sql = string.Empty;
            string title = string.Empty;
            string columnName = string.Empty;
            for (int i = 0; i < noderoot.ChildNodes.Count; i++)
            {
                XmlNode nodes = noderoot.ChildNodes[i];
                if (nodes.Attributes["Value"].Value == "true")
                {
                    XmlNodeList nodeList = nodes.ChildNodes;
                    if (nodeList.Count > 2)
                    {
                        sql = nodeList[0].InnerText.Replace("\r\n", "");
                        title = nodeList[1].InnerText;
                        columnName = nodeList[2].InnerText;
                    }
                }
            }
            if (!string.IsNullOrEmpty(sql) && !string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(columnName))
            {
                DataTable dt = ExcelSQl.GetExcel(sql);
                string[] arrField = columnName.Trim().Split(',');
                string[] fileName = title.Split(',');
                Epower.ITSM.Web.Common.ExcelExport.ExportIssuesList1(this, dt, fileName, arrField, Session["UserID"].ToString());
            }
            else
            {
                PageTool.MsgBox(Page, "导出Excel失败,请检查配置文件！");
            }
        }
        #endregion

        #region 根据FlowID获取当前处理人
        public string GetCurrName(object FlowId)
        {
            ZHServiceDP ee = new ZHServiceDP();
            return ee.GetCurrName(FlowId);
        }
        #endregion
        #region 截取详细描述的内容
        public string GetContent(string content)
        {
            return content.Length >= 10 ? content.Substring(0, 7) + "..." : content;
        }
        #endregion

        protected void hidd_btnDelete_Click(object sender, EventArgs e)
        {
            BindData();
        }
        #endregion

        #region  判断是否是督办事件
        /// <summary>
        /// 判断事件是否有督办内容
        /// </summary>
        /// <param name="flowid"></param>
        /// <returns></returns>
        public bool IsSuperIess(string flowid)
        {
            bool isSuper = false;
            ZHServiceDP zhdp = new ZHServiceDP();
            if (!string.IsNullOrEmpty(flowid.ToString()))
            {
                DataTable dt_Monitor = zhdp.getMonitor(flowid.ToString()); //调用查询函数，查询到事件有没有督办信息
                //如果有督办就返回true
                if (dt_Monitor != null && dt_Monitor.Rows.Count > 0)
                {
                    isSuper = true;
                }
            }

            return isSuper;
        }
        #endregion
    }
}
