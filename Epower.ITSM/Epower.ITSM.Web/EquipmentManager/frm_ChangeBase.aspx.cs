/****************************************************************************
 * 
 * description:变更流程应用表单
 * 
 * 
 * 
 * Create by:
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

using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using appDataProcess;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;
using EpowerCom;
using EpowerGlobal;
using Epower.ITSM.Web.Controls;
using Epower.ITSM.SqlDAL.ES_TBLCS;
using System.Collections.Generic;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frm_ChangeBase : BasePage
    {
        #region 定义变量

        /// <summary>
        /// myFlowForms
        /// </summary>
        private FlowForms myFlowForms;
        long lngFromFlowID = 0; //归档来源的流程实例ID
        long lngProblemFlowID = 0;          //存放从问题单传过来的问题单FlowID
        private objFlow oFlow;

        #endregion

        #region 属性

        #region 流程ID

        /// <summary>
        /// 流程ID
        /// </summary>
        public string FlowID
        {
            get
            {
                if (ViewState["frm_ChangeBase_FlowID"] != null)
                {
                    return ViewState["frm_ChangeBase_FlowID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["frm_ChangeBase_FlowID"] = value;
            }
        }

        #endregion

        #region 业务动作ID

        /// <summary>
        /// 业务动作ID
        /// </summary>
        public string BusID
        {
            get
            {
                if (ViewState["frm_Change_Base_BusID"] != null)
                {
                    return ViewState["frm_Change_Base_BusID"].ToString();
                }
                else
                {
                    ViewState["frm_Change_Base_BusID"] = ZHServiceDP.GetBID(oFlow.FlowModelID).ToString();
                    return ViewState["frm_Change_Base_BusID"].ToString();
                }
            }
            set
            {
                ViewState["frm_Change_Base_BusID"] = value;
            }
        }

        #endregion

        #region 流程模型ID

        /// <summary>
        /// 流程模型ID
        /// </summary>
        public string FlowModelID
        {
            get
            {
                if (ViewState["frm_ChangeBase_FlowModelID"] != null)
                {
                    return ViewState["frm_ChangeBase_FlowModelID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["frm_ChangeBase_FlowModelID"] = value;
            }
        }

        #endregion

        #region 应用ID

        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppID
        {
            get
            {
                if (ViewState["frm_Issue_Base_AppID"] != null)
                {
                    return ViewState["frm_Issue_Base_AppID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["frm_Issue_Base_AppID"] = value;
            }
        }

        #endregion

        #region 流程环节ID

        /// <summary>
        /// 流程环节ID
        /// </summary>
        public string NodeModelID
        {
            get
            {
                if (ViewState["frm_ChangeBase_NodeModelID"] != null)
                {
                    return ViewState["frm_ChangeBase_NodeModelID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["frm_ChangeBase_NodeModelID"] = value;
            }
        }

        #endregion


        #region 消息编号

        /// <summary>
        /// 取得流程MessageID
        /// </summary>
        public string MessageID
        {
            get
            {
                if (ViewState["MessageID"] != null)
                {
                    return ViewState["MessageID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["MessageID"] = value;
            }
        }

        #endregion

        #region 临时存储XML文件路径

        /// <summary>
        /// 临时存储XML文件路径
        /// </summary>
        public string DeskChangeUrl
        {
            get
            {
                if (hidDeskChange.Value == "")
                {
                    hidDeskChange.Value = CommonDP.GetConfigValue("TempCataLog", "TempCataLog") + "DeskChange" + Session["UserID"].ToString() + CTools.GetRandom() + ".xml";
                    return hidDeskChange.Value;
                }
                return hidDeskChange.Value;
            }
        }

        #endregion

        #region 变更ID

        /// <summary>
        /// 变更ID
        /// </summary>
        protected string ChangeID
        {
            get { if (ViewState["ChangeID"] != null) return ViewState["ChangeID"].ToString(); else return "0"; }
            set { ViewState["ChangeID"] = value; }
        }

        #endregion

        #region 变更单是否有相关事件
        public string ChangeFlowID
        {
            get
            {
                if (ViewState["ChangeFlowID"] != null)
                {
                    return ViewState["ChangeFlowID"].ToString();
                }
                else
                {
                    return "";
                }
            }
        }
        #endregion

        #region 变更单是否有相关问题
        public string FromProblemFlowID
        {
            get
            {
                if (ViewState["FromProblemFlowID"] != null)
                {
                    return ViewState["FromProblemFlowID"].ToString();
                }
                else
                {
                    return "";
                }
            }
        }
        #endregion

        #region 打印模式

        /// <summary>
        /// 打印方式 0 IE方式，1 Report Service方式
        /// </summary>
        public string PrintMode
        {
            get
            {
                return CommonDP.GetConfigValue("PrintMode", "PrintMode").ToString();
            }
        }
        #endregion

        #endregion

        #region Page_Load

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            myFlowForms = (FlowForms)this.Master;

            myFlowForms.mySetContentReadOnly += new FlowForms.DoContentActions(Master_mySetContentVisible);
            myFlowForms.myGetFormsValue += new FlowForms.GetFormsValue(Master_myGetFormsValue);
            myFlowForms.mySetFormsValue += new FlowForms.DoContentActions(Master_mySetFormsValue);

            myFlowForms.myPreClickCustomize += new FlowForms.DoContentSubmitValid(myFlowForms_myPreClickCustomize);
            myFlowForms.myPreSaveClickCustomize += new FlowForms.DoContentValid(myFlowForms_myPreSaveClickCustomize);

            myFlowForms.blnSMSNotify = true;
            myFlowForms.blnEmail = true;

            if (IsPostBack == false)
            {
                PageDeal.SetLanguage(this.Controls[0].Controls[1]);
            }
            else
            {
                lngFromFlowID = long.Parse(ViewState["frm_Problem_FromFlowID"].ToString());
                lngProblemFlowID = long.Parse(ViewState["frm_FromProblemFlowID"].ToString());            //问题单传过来的FlowID

                if (txtCustAddr.Text.Trim() == string.Empty)
                    txtCustAddr.Text = hidCust.Value.Trim();
                if (txtContact.Text.Trim() == string.Empty)
                    txtContact.Text = hidContact.Value.Trim();
                if (txtCTel.Text.Trim() == string.Empty)
                    txtCTel.Text = hidTel.Value.Trim();
                if (txtAddr.Text.Trim() == string.Empty)
                    txtAddr.Text = hidAddr.Value.Trim();
                if (lblCustDeptName.Text.Trim() == string.Empty || hidCustDeptName.Value.Trim() != string.Empty)
                    lblCustDeptName.Text = hidCustDeptName.Value.Trim();
                if (lblEmail.Text.Trim() == string.Empty || hidCustEmail.Value.Trim() != string.Empty)
                    lblEmail.Text = hidCustEmail.Value.Trim();
                if (lblMastCust.Text.Trim() == string.Empty)
                    lblMastCust.Text = hidMastCust.Value.Trim();
                if (hidjob.Value.Trim() != string.Empty || lbljob.Text.Trim() == string.Empty)
                    lbljob.Text = hidjob.Value.Trim();
            }

            if (!IsPostBack)
            {
                if (this.chkChangePlace.SelectedItem == null)
                {
                    GetChangePlace();
                }
                string strThisMsg = "";
                strThisMsg = txtCustAddr.ClientID + ">" + "客户名称";
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), txtCustAddr.ClientID, "<script type=\"text/javascript\" >if(typeof(sarrvalidlist) == 'undefined'){ var sarrvalidlist = new Array(1);}  sarrvalidlist[0] = sarrvalidlist[0] + '|' + '" + strThisMsg + "';</script>");
            }

            if (hidEquIsHid.Value == "1")            //表明资产为只读
                SetFareDetailReadOnly();

            if (hidEquIsHid.Value == "2")            //表明资产为不可见
                gvBillItem.Visible = false;

            if (this.hidAppID.Value != "0")
            {
                //Extension_DayCtrList1.EquCategoryID = long.Parse(this.hidAppID.Value.ToString());
                EpowerCom.objFlow oFlow2 = new objFlow(long.Parse(Session["UserID"].ToString()), long.Parse(FlowModelID), long.Parse(MessageID));
                Extension_DayCtrList1.NodeModelID = oFlow2.NodeModelID;

                Extension_DayCtrList1.EquID = long.Parse(this.hidFlowID.Value.ToString());
                Extension_DayCtrList1.EquCategoryID = oFlow2.FlowModelID;
                
                
            }
        }

        #endregion

        #region 获取相关事件信息 BindRelItemData

        /// <summary>
        /// 获取相关事件信息
        /// </summary>
        /// <param name="lngFlowID"></param>
        private void BindRelItemData(long lngFlowID)
        {
            DataTable dt = ChangeDealDP.GetIssuesForChange(lngFlowID);
            gridAttention.DataSource = dt.DefaultView;
            gridAttention.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");

            gridAttention.DataBind();
        }

        #endregion

        #region 获取相关问题单
        /// <summary>
        /// 获取相关问题单
        /// </summary>
        /// <param name="lngFlowID"></param>
        private void BindRelProblemData(long lngFlowID)
        {
            DataTable dt = ChangeDealDP.GetProblemsByChange(lngFlowID);
            dgRelProblem.DataSource = dt.DefaultView;
            dgRelProblem.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");

            dgRelProblem.DataBind();
        }

        #endregion

        #region 获取 URL
        /// <summary>
        /// 获取URL
        /// </summary>
        /// <param name="lngNoticeID"></param>
        /// <returns></returns>
        protected string GetUrl(decimal lngFlowID)
        {
            //暂时没处理分页
            string sUrl = "";
            sUrl = "javascript:window.open('../Forms/frmIssueView.aspx?NewWin=true&FlowID=" + lngFlowID.ToString() + "','','scrollbars=yes,resizable=yes,top=0,left=0');";
            return sUrl;
        }

        #endregion

        #region 获取变更和资产的状态 GetEquStatus

        /// <summary>
        /// 获取变更和资产的状态
        /// </summary>
        /// <returns></returns>
        private bool GetEquStatus()
        {
            bool breturn = true;
            if (this.hidFlowID.Value.Trim() == "0" && lngFromFlowID != 0)
            {
                if (Session["ExtendParameter"] != null && Session["ExtendParameter"].ToString() != string.Empty && Session["ExtendParameter"].ToString() != "0")
                {
                    //检查是否已经转换为问题单
                    if (ProblemDealDP.CheckIsChangeProblem(lngFromFlowID))
                    {
                        Epower.DevBase.BaseTools.PageTool.MsgBox(this, "此事件已升级为问题，不能再升级！");
                        breturn = false;
                    }
                }
            }
            return breturn;
        }

        #endregion

        #region Master_mySetFormsValue

        /// <summary>
        /// 表单页面设值
        /// </summary>
        private void Master_mySetFormsValue()
        {
            #region 属性初始化

            oFlow = myFlowForms.oFlow;//流程对象
            myFlowForms.FormTitle = myFlowForms.oFlow.FlowName;//流程标题
            this.FlowID = myFlowForms.oFlow.FlowID.ToString();//流程ID
            this.FlowModelID = myFlowForms.oFlow.FlowModelID.ToString();//流程模型ID
            this.MessageID = myFlowForms.oFlow.MessageID.ToString();
            this.AppID = myFlowForms.oFlow.AppID.ToString();//应用ID
            this.NodeModelID = myFlowForms.oFlow.NodeModelID.ToString();//环节ID
            hidFlowID.Value = myFlowForms.oFlow.FlowID.ToString();//流程ID

            this.hidAppID.Value = this.AppID;

            if (oFlow.FlowStatus == e_FlowStatus.efsEnd)
            {
                myFlowForms.CtrButtons1.Button3Visible = true;
                myFlowForms.CtrButtons1.ButtonName3 = "知识归档";
                myFlowForms.CtrButtons1.Button3Function = "DoKmAdd(" + oFlow.MessageID.ToString() + "," + oFlow.AppID.ToString() + "," + oFlow.FlowID.ToString() + ");";
            }
            #endregion

            #region Master_mySetFormsValue

            DataTable dt = new DataTable();
            if (myFlowForms.oFlow.MessageID != 0)
            {
                #region 获取数据

                ImplDataProcess dp = new ImplDataProcess(myFlowForms.oFlow.AppID);//实现接口
                DataSet ds = dp.GetFieldsDataSet(myFlowForms.oFlow.FlowID, myFlowForms.oFlow.OpID);//获取数据
                dt = ds.Tables[0];

                #endregion
            }
            else
            {
                #region 内部模式设置缺省的 客户资料

                CtrDTCustTime.dateTime = DateTime.Now;//变更时间
                if (CommonDP.GetConfigValue("Other", "ItsmCustomerMode") == "0")
                {
                    //内部模式设置缺省的 客户资料
                    txtCustAddr.Text = Session["UserDefaultCustomerName"].ToString();
                    hidCustID.Value = Session["UserDefaultCustomerID"].ToString();
                    if (hidCustID.Value != "0" && hidCustID.Value != "")
                    {
                        //取得客户资料
                        Br_ECustomerDP ec = new Br_ECustomerDP();
                        ec = ec.GetReCorded(long.Parse(hidCustID.Value));
                        hidCustID.Value = ec.ID.ToString();
                        lblCustDeptName.Text = ec.CustDeptName;
                        hidCustDeptName.Value = ec.CustDeptName;
                        hidCust.Value = ec.ShortName;
                        txtCustAddr.Text = ec.ShortName;
                        lblEmail.Text = ec.Email;
                        hidCustEmail.Value = ec.Email;
                        lblMastCust.Text = ec.ShortName;
                        lbljob.Text = ec.Job;
                        hidjob.Value = ec.Job;

                        txtAddr.Text = ec.Address;
                        hidAddr.Value = ec.Address;
                        lblAddr.Text = ec.Address;
                        txtContact.Text = ec.LinkMan1;
                        labContact.Text = txtContact.Text;
                        txtCTel.Text = ec.Tel1;
                        labCTel.Text = ec.Tel1;
                        lblMastCust.Text = ec.MastCustName;

                    }
                }

                #endregion
            }

            #endregion

            #region 获取数据

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    #region 赋值页面数据

                    DataRow row = dt.Rows[0];

                    //登单人信息
                    UserPicker1.UserName = row["RegUserName"].ToString();
                    if (row["RegUserID"].ToString().Length > 0)
                    {
                        UserPicker1.UserID = long.Parse(row["RegUserID"].ToString());
                    }

                    labServiceNo.Text = row["ChangeNo"].ToString();//变更单号
                    ChangeID = row["id"].ToString();
                    hidChangeId.Value = ChangeID;
                    CtrFlowFTSubject.Value = row["Subject"].ToString();
                    CtrDTCustTime.dateTime = DateTime.Parse(row["ChangeTime"].ToString());
                    CtrFlowReContent.Value = row["Content"].ToString();
                    CtrFCDEffect.CatelogID = long.Parse(row["EffectID"].ToString());
                    CtrFCDInstancy.CatelogID = long.Parse(row["InstancyID"].ToString());
                    CtrFCDlevel.CatelogID = long.Parse(row["LevelID"].ToString());

                    hidCustID.Value = row["CustID"].ToString();    //客户
                    txtCustAddr.Text = row["CustName"].ToString();
                    labCustAddr.Text = StringTool.ParseForHtml(txtCustAddr.Text);
                    txtAddr.Text = row["CustAddress"].ToString();
                    hidAddr.Value = row["CustAddress"].ToString();
                    lblAddr.Text = row["CustAddress"].ToString();
                    txtContact.Text = row["Contact"].ToString();
                    labContact.Text = StringTool.ParseForHtml(txtContact.Text);
                    txtCTel.Text = row["ctel"].ToString();
                    labCTel.Text = StringTool.ParseForHtml(txtCTel.Text);

                    //获取客户相关信息
                    if (hidCustID.Value.Trim() != "0" && hidCustID.Value.Trim() != "")
                    {
                        Br_ECustomerDP ec = new Br_ECustomerDP();
                        ec = ec.GetReCorded(long.Parse(hidCustID.Value));
                        lblCustDeptName.Text = ec.CustDeptName;
                        hidCustDeptName.Value = ec.CustDeptName;
                        lbljob.Text = ec.Job;
                        hidjob.Value = ec.Job;
                        hidCustEmail.Value = ec.Email;
                        lblEmail.Text = ec.Email;
                        lblMastCust.Text = ec.ShortName;

                        Br_MastCustomerDP mc = new Br_MastCustomerDP();
                        mc = mc.GetReCorded((long)ec.MastCustID);
                        lblMastCust.Text = mc.ShortName;
                    }

                    #region 20160720 add
                    if (row["CHANGE_PLACE_ID"].ToString() != "")//变更场所
                    {
                        string[] cg_placeID = row["CHANGE_PLACE_ID"].ToString().Split(',');
                        string[] cg_place = row["CHANGE_PLACE_NAME"].ToString().Split(',');
                        if (cg_place != null)
                        {
                            for (int i = 0; i < cg_placeID.Length; i++)
                            {
                                if (cg_placeID[i] != "")
                                {
                                    for (int j = 0; j < this.chkChangePlace.Items.Count; j++)
                                    {
                                        if (cg_placeID[i] == this.chkChangePlace.Items[j].Value)
                                        {
                                            this.chkChangePlace.Items[j].Selected = true;
                                            break;
                                        }
                                    }
                                    this.labChangePlace.Text += cg_place[i] + "、";
                                }
                            }
                        }
                        else//一条数据
                        {
                            for (int j = 0; j < this.chkChangePlace.Items.Count; j++)
                            {
                                if (row["CHANGE_PLACE_ID"].ToString() == this.chkChangePlace.Items[j].Value)
                                {
                                    this.chkChangePlace.Items[j].Selected = true;
                                    break;
                                }
                            }
                            this.labChangePlace.Text = row["CHANGE_PLACE_NAME"].ToString();
                        }
                    }

                   
                    string Isplan = row["Isplan"].ToString();
                    if (Isplan != "无")
                    {
                        if (Isplan != "")
                        {
                            this.ddlIsplan.SelectedValue = "1";
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script>$('#div_isplan').show();</script>", false);
                        }
                        else
                        {
                            this.ddlIsplan.SelectedValue = "-1";
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script>$('#div_isplan').hide();</script>", false);
                        }
                    }
                    else
                    {
                        Isplan = "";
                        this.ddlIsplan.Visible = true;
                        this.ddlIsplan.SelectedValue = "0";
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script>$('#div_isplan').hide();</script>", false);
                    }
                    this.ctrChangeNeedPeople.UserID = Convert.ToInt64(row["ChangeNeedPeopleID"].ToString() == "" ? "0" : row["ChangeNeedPeopleID"]);//变更需求人
                    ctrIsplan.Value = Isplan;//应急/回退方案
                    CtrBusEffect.SelValue = row["IS_BUS_EFFECT"].ToString();
                    CtrBusEffect.TextValue = row["BUS_EFFECT"].ToString();
                    CtrDataEffect.SelValue = row["IS_DATA_EFFECT"].ToString();
                    CtrDataEffect.TextValue = row["DATA_EFFECT"].ToString();
                    CtrPlanStartTime.dateTimeString = row["PLAN_BEGIN_TIME"].ToString();
                    CtrPlanEndTime.dateTimeString = row["PLAN_END_TIME"].ToString();                 
                    labIsPlanChange.Text = row["IS_PLAN_CHANGE"].ToString() == "" || row["IS_PLAN_CHANGE"].ToString() == "0"? "否" : "是";
                    CtrStopServer.TextValue = row["STOP_SERVER_REMARK"].ToString();
                    CtrStopServer.SelValue = row["IS_STOP_SERVER"].ToString();
                    CtrChangeWindow.SelValue = row["CHANGE_WINDOW_ID"].ToString();
                    CtrChangeWindow.TextValue = row["CHANGE_WINDOW_REMARK"].ToString();
                    CtrRealStartTime.dateTimeString = row["REAL_BEGIN_TIME"].ToString();
                    //实际结束时间
                    CtrRealEndTime.dateTimeString = row["REAL_END_TIME"].ToString();
                    #endregion


                    #region 变更分析

                    CtrFlowReChangeAnalyses.Value = row["ChangeAnalyses"].ToString();

                    #endregion

                    #region 分析结果

                    CtrFlowReChangeAnalysesResult.Value = row["ChangeAnalysesResult"].ToString();

                    #endregion

                    #region 变更类别

                    CtrChangeType.CatelogID = Convert.ToInt32(row["ChangeTypeID"]);
                    CtrChangeType.CatelogValue = row["ChangeTypeName"].ToString();

                    #endregion

                    #region 登记时间

                    CtrDTRegTime.dateTime = DateTime.Parse(row["RegTime"].ToString());

                    #endregion

                    #region 变更状态

                    if (row["DealStatusID"].ToString() != string.Empty)
                        CtrFCDDealStatus.CatelogID = long.Parse(row["DealStatusID"].ToString());

                    #endregion

                    if (row["IssuesFlowID"].ToString() != string.Empty && row["IssuesFlowID"].ToString() != "0")
                    {
                        BindRelItemData(long.Parse(row["FlowID"].ToString()));
                        ViewState["ChangeFlowID"] = row["IssuesFlowID"].ToString();
                    }
                    else
                    {
                        ViewState["ChangeFlowID"] = "0";
                    }

                    if (row["ProblemFlowID"].ToString() != string.Empty && row["ProblemFlowID"].ToString() != "0")
                    {
                        BindRelProblemData(long.Parse(row["FlowID"].ToString()));
                        ViewState["FromProblemFlowID"] = row["ProblemFlowID"].ToString();
                    }
                    else
                    {
                        ViewState["FromProblemFlowID"] = "0";
                    }

                    #endregion
                }
                else
                {
                    #region 事件升级成变更

                    if (Session["ExtendParameter"] != null && Session["ExtendParameter"].ToString() != string.Empty && Session["ExtendParameter"].ToString() != "0")
                    {
                        //通过获取
                        long lngFlowID = long.Parse(Session["ExtendParameter"].ToString());
                        //检查是否已关联变更单
                        if (ChangeDealDP.CheckIsChangeProblem(lngFlowID))
                        {
                            Epower.DevBase.BaseTools.PageTool.MsgBox(this, "此事件关联变更单，不能再关联！");
                            Epower.DevBase.BaseTools.PageTool.AddJavaScript(this, "window.close();");
                        }

                        DataTable dtIssue = ZHServiceDP.GetDataByFlowID(lngFlowID);
                        if (dtIssue.Rows.Count > 0)
                        {
                            Br_ECustomerDP ec = new Br_ECustomerDP();
                            ec = ec.GetReCorded(long.Parse(dtIssue.Rows[0]["CustID"].ToString()));
                            hidCustID.Value = ec.ID.ToString();
                            hidCust.Value = ec.ShortName;
                            txtCustAddr.Text = ec.ShortName;
                            hidCustEmail.Value = ec.Email;
                            lblEmail.Text = ec.Email;
                            lblMastCust.Text = ec.ShortName;

                            lblCustDeptName.Text = ec.CustDeptName;
                            hidCustDeptName.Value = ec.CustDeptName;
                            lbljob.Text = ec.Job;
                            hidjob.Value = ec.Job;

                            txtAddr.Text = ec.Address;
                            hidAddr.Value = ec.Address;
                            lblAddr.Text = ec.Address;
                            txtContact.Text = ec.LinkMan1;
                            labContact.Text = txtContact.Text;
                            txtCTel.Text = ec.Tel1;
                            labCTel.Text = ec.Tel1;
                            lblMastCust.Text = ec.MastCustName;

                            DataRow dr = dtIssue.Rows[0];

                            if (dr["subject"] != null)
                                CtrFlowFTSubject.Value = dr["subject"].ToString();
                            if (dr["content"] != null)
                                CtrFlowReContent.Value = dr["content"].ToString();

                            #region 绑定事件传过来的资产
                            dt = ZHServiceDP.GetIssuesByFlowID(lngFlowID);
                            if (dt.Rows.Count > 0 && dt.Rows[0]["EQUID"] != null && dt.Rows[0]["EQUID"].ToString() != "" && dt.Rows[0]["EQUID"].ToString() != "0")
                            {

                                dt = CheckUnSaveDataAndLoadIt(dt);

                                gvBillItem.DataSource = dt.DefaultView;
                                gvBillItem.DataBind();
                            }
                            else
                            {
                                BindGrid(long.Parse(ChangeID));
                            }
                        }

                        lngFromFlowID = lngFlowID;

                        myFlowForms.TempFlowID = lngFlowID;

                    }
                    ViewState["ChangeFlowID"] = "0";
                            #endregion

                    #region 问题发起变更

                    if (Session["ProblemFlowID"] != null && Session["ProblemFlowID"].ToString() != string.Empty && Session["ProblemFlowID"].ToString() != "0")
                    {
                        //通过获取
                        long lngFlowID = long.Parse(Session["ProblemFlowID"].ToString());
                        //检查是否已关联变更单
                        if (ChangeDealDP.CheckIsChangeFromPro(lngFlowID))
                        {
                            Epower.DevBase.BaseTools.PageTool.MsgBox(this, "此问题已关联变更单，不能再关联！");
                            Epower.DevBase.BaseTools.PageTool.AddJavaScript(this, "window.close();");
                        }

                        DataTable dtPro = ProblemDealDP.GetDataByFlowID(lngFlowID);
                        if (dtPro.Rows.Count > 0)
                        {
                            #region 绑定问题传过来的用户信息
                            Br_ECustomerDP ec = new Br_ECustomerDP();
                            ec = ec.GetReCordedByUserID(dtPro.Rows[0]["RegUserID"].ToString());
                            hidCustID.Value = ec.ID.ToString();
                            hidCust.Value = ec.ShortName;
                            txtCustAddr.Text = ec.ShortName;
                            hidCustEmail.Value = ec.Email;
                            lblEmail.Text = ec.Email;
                            lblMastCust.Text = ec.ShortName;

                            lblCustDeptName.Text = ec.CustDeptName;
                            hidCustDeptName.Value = ec.CustDeptName;
                            lbljob.Text = ec.Job;
                            hidjob.Value = ec.Job;

                            txtAddr.Text = ec.Address;
                            hidAddr.Value = ec.Address;
                            lblAddr.Text = ec.Address;
                            txtContact.Text = ec.LinkMan1;
                            labContact.Text = txtContact.Text;
                            txtCTel.Text = ec.Tel1;
                            labCTel.Text = ec.Tel1;
                            lblMastCust.Text = ec.MastCustName;

                            DataRow dr = dtPro.Rows[0];
                            if (dr["problem_title"] != null)
                                CtrFlowFTSubject.Value = dr["problem_title"].ToString();
                            if (dr["problem_subject"] != null)
                                CtrFlowReContent.Value = dr["problem_subject"].ToString();
                            #endregion

                            #region 绑定问题传过来的资产
                            dt = ChangeDealDP.GetProblemsByFlowID(lngFlowID);
                            if (dt.Rows.Count > 0 && dt.Rows[0]["EQUID"] != null && dt.Rows[0]["EQUID"].ToString() != "" && long.Parse(dt.Rows[0]["EQUID"].ToString()) != -1)
                            {

                                dt = CheckUnSaveDataAndLoadIt(dt);

                                gvBillItem.DataSource = dt.DefaultView;
                                gvBillItem.DataBind();
                            }
                            else
                            {
                                BindGrid(long.Parse(ChangeID));
                            }
                            #endregion
                        }

                        lngProblemFlowID = lngFlowID;
                        myFlowForms.TempFlowID = lngFlowID;
                    }

                    ViewState["FromProblemFlowID"] = "0";
                    #endregion

                    #endregion
                }
            }

            if (UserPicker1.UserID == null || UserPicker1.UserID == 0)
            {
                UserPicker1.UserID = long.Parse(Session["UserID"].ToString());
                UserPicker1.UserName = Session["PersonName"].ToString();
            }

            if ((Session["ProblemFlowID"] == null || Session["ProblemFlowID"].ToString() == "") && (Session["ExtendParameter"] == null || Session["ExtendParameter"].ToString() == ""))
                BindGrid(long.Parse(ChangeID));



            #endregion

            #region set visible

            setFieldCollection setFields = myFlowForms.oFlow.setFields;
            foreach (setField sf in setFields)
            {
                if (sf.Editable == true && sf.Visibled == true)
                {
                    continue;
                }
                switch (sf.Name.ToLower())
                {
                    case "changetypeid":
                        CtrChangeType.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrChangeType.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    case "effectid":
                        CtrFCDEffect.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrFCDEffect.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    case "instancyid":
                        CtrFCDInstancy.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrFCDInstancy.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    case "levelid":
                        CtrFCDlevel.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrFCDlevel.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    case "subject":
                        CtrFlowFTSubject.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrFlowFTSubject.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    case "changetime":
                        CtrDTCustTime.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrDTCustTime.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    case "content":

                        CtrFlowReContent.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrFlowReContent.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    case "dealstatusid":
                        CtrFCDDealStatus.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrFCDDealStatus.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;

                    //客户信息
                    case "custinfo":
                        txtCustAddr.Visible = false;    // 客户名称
                        cmdCust.Visible = false;
                        txtAddr.Visible = false;    // 客户地址
                        txtContact.Visible = false;    // 联系人
                        txtCTel.Visible = false;    // 联系人电话
                        lblMustIn.Visible = false;
                        if (sf.Visibled == true)
                        {
                            labCustAddr.Visible = true;
                            lblAddr.Visible = true;
                            labContact.Visible = true;
                            labCTel.Visible = true;
                            lblCustDeptName.Visible = true;
                        }

                        if (sf.Visibled == false)
                        {
                            Table12.Visible = false;
                            Table2.Visible = false;

                        }
                        break;
                    //资产信息
                    case "equipmentname":
                        SetFareDetailvisible("equipmentname", sf.Visibled);
                        break;
                    case "impactanalysis":
                        SetFareDetailvisible("impactanalysis", sf.Visibled);
                        break;
                    //处理信息
                    case "changeanalyses":
                        CtrFlowReChangeAnalyses.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrFlowReChangeAnalyses.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    case "changeanalysesresult":
                        CtrFlowReChangeAnalysesResult.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrFlowReChangeAnalysesResult.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    case "equchange":
                        SetFareDetailvisible("equchange", sf.Visibled);
                        break;
                    case "equ_expr_property":    // 扩展属性 - 2013-11-19 @孙绍棕
                        Extension_DayCtrList1.ReadOnly = true;
                        if (!sf.Visibled)
                        {
                            Extension_DayCtrList1.Visible = false;
                        }
                        break;
                    #region 20160720 add                    
                    //变更场所
                    case "change_place_id":
                        //CtrChangePlace.ContralState = eOA_FlowControlState.eReadOnly;
                        //if (sf.Visibled == false)
                        //{
                        //    CtrChangePlace.ContralState = eOA_FlowControlState.eHidden;
                        //}
                        this.chkChangePlace.Visible = false;
                        this.labChangePlace.Visible = true;
                        spPlace.Visible = false;
                        if (sf.Visibled == false)
                        {
                            this.labChangePlace.Visible = false;
                        }
                        break;
                    //变更需求人
                    case "changeneedpeopleid":
                        ctrChangeNeedPeople.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            ctrChangeNeedPeople.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;                   
                    //应急/回退方案
                    case "isplan":
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script>$('#div_isplan').show();</script>", false);
                        this.ddlIsplan.Enabled = false;
                        ctrIsplan.ContralState = eOA_FlowControlState.eReadOnly;
                        spPlan.Visible = false;
                        if (sf.Visibled == false)
                        {
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script>$('#div_isplan').hide();</script>", false);
                            ctrIsplan.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    //是否业务影响
                    case "is_bus_effect":
                        CtrBusEffect.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrBusEffect.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    //是否数据影响
                    case "is_data_effect":
                        CtrDataEffect.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrDataEffect.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    //是否停用服务
                    case "is_stop_server":
                        CtrStopServer.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrStopServer.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    case "is_plan_change":
                        if (sf.Visibled == false)
                        {
                            this.labIsPlanChange.Visible = false;
                        }
                        break;
                    //变更窗口
                    case "change_window_id":
                        CtrChangeWindow.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrChangeWindow.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;

                    //计划开始时间
                    case "plan_begin_time":
                        CtrPlanStartTime.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrPlanStartTime.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    //计划结束时间
                    case "plan_end_time":
                        CtrPlanEndTime.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrPlanEndTime.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    //实际开始时间
                    case "real_begin_time":
                        CtrRealStartTime.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrRealStartTime.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    //实际结束时间
                    case "real_end_time":
                        CtrRealEndTime.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrRealEndTime.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    #endregion
                    default:
                        break;

                }
            }

            //无论任何情况,流程ID为0时 也不可见
            if (myFlowForms.oFlow.FlowID == 0)
            {
                trChange.Visible = false;
            }
            #endregion

            Extension_DayCtrList1.EquID = long.Parse(this.hidFlowID.Value.ToString());

            if (this.hidAppID.Value.ToString() != "0")
            {
                //Extension_DayCtrList1.EquCategoryID = long.Parse(this.hidAppID.Value.ToString());
                Extension_DayCtrList1.NodeModelID = this.FlowMaster.oFlow.NodeModelID;
                Extension_DayCtrList1.EquCategoryID = this.FlowMaster.oFlow.FlowModelID;


            }

            ViewState["frm_Problem_FromFlowID"] = lngFromFlowID;
            ViewState["frm_FromProblemFlowID"] = lngProblemFlowID;
        }
        #endregion

        #region Master_mySetContentVisible

        /// <summary>
        /// 页面隐藏
        /// </summary>
        private void Master_mySetContentVisible()
        {
            #region Master_mySetContentVisible

            #region 标题

            if (CtrFlowFTSubject.ContralState != eOA_FlowControlState.eHidden)
                CtrFlowFTSubject.ContralState = eOA_FlowControlState.eReadOnly;

            #endregion

            #region 变更时间

            if (CtrDTCustTime.ContralState != eOA_FlowControlState.eHidden)
                CtrDTCustTime.ContralState = eOA_FlowControlState.eReadOnly;

            #endregion

            #region 请求内容

            if (CtrFlowReContent.ContralState != eOA_FlowControlState.eHidden)
                CtrFlowReContent.ContralState = eOA_FlowControlState.eReadOnly;

            #endregion

            #region 请求内容

            if (CtrFCDDealStatus.ContralState != eOA_FlowControlState.eHidden)
                CtrFCDDealStatus.ContralState = eOA_FlowControlState.eReadOnly;

            #endregion

            #region 影响度

            if (CtrFCDEffect.ContralState != eOA_FlowControlState.eHidden)
                CtrFCDEffect.ContralState = eOA_FlowControlState.eReadOnly;

            #endregion

            #region 紧急度

            if (CtrFCDInstancy.ContralState != eOA_FlowControlState.eHidden)
                CtrFCDInstancy.ContralState = eOA_FlowControlState.eReadOnly;

            #endregion

            #region 变更级别

            if (CtrFCDlevel.ContralState != eOA_FlowControlState.eHidden)
                CtrFCDlevel.ContralState = eOA_FlowControlState.eReadOnly;

            #endregion

            #region 变更类别

            if (CtrChangeType.ContralState != eOA_FlowControlState.eHidden)
                CtrChangeType.ContralState = eOA_FlowControlState.eReadOnly;

            #endregion

            #region 联系人

            if (txtContact.Visible == true)
                labContact.Visible = true;
            txtContact.Visible = false;

            #endregion

            #region 联系电话


            if (txtCTel.Visible == true)
                labCTel.Visible = true;
            txtCTel.Visible = false;

            #endregion

            #region 联系地址

            if (txtCustAddr.Visible == true)
                labCustAddr.Visible = true;
            txtCustAddr.Visible = false;
            cmdCust.Visible = false;
            lblMustIn.Visible = false;

            if (txtAddr.Visible == true)
                lblAddr.Visible = true;
            txtAddr.Visible = false;

            #endregion

            #region 变更分析

            if (CtrFlowReChangeAnalyses.ContralState != eOA_FlowControlState.eHidden)
                CtrFlowReChangeAnalyses.ContralState = eOA_FlowControlState.eReadOnly;

            #endregion

            #region 分析结果

            if (CtrFlowReChangeAnalysesResult.ContralState != eOA_FlowControlState.eHidden)
                CtrFlowReChangeAnalysesResult.ContralState = eOA_FlowControlState.eReadOnly;

            #endregion

            trChange.Visible = false;

            SetFareDetailReadOnly();
            gvBillItem.Columns[gvBillItem.Columns.Count - 1].Visible = false;//资产变更
            gvBillItem.Columns[gvBillItem.Columns.Count - 3].Visible = false;//影响度分析
            #endregion

           
            #region 是否计划性变更
            if (this.labIsPlanChange.Visible == true)
            {
            }
            #endregion
            #region 变更场所            
            this.chkChangePlace.Visible = false;
            this.labChangePlace.Visible = true;
            spPlace.Visible = false;
            #endregion
            #region 变更需求人
            if (this.ctrChangeNeedPeople.ContralState != eOA_FlowControlState.eHidden)
            {
                this.ctrChangeNeedPeople.ContralState = eOA_FlowControlState.eReadOnly;
            }
            #endregion
            #region 应急/回退方案
            if (this.ctrIsplan.ContralState != eOA_FlowControlState.eHidden)
            {
                this.ddlIsplan.Enabled = false;
                spPlan.Visible = false;
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script>$('#div_isplan').show();</script>", false);
                this.ctrIsplan.ContralState = eOA_FlowControlState.eReadOnly;
            }
            #endregion
            #region 变更窗口
            if (CtrChangeWindow.ContralState != eOA_FlowControlState.eHidden)
                CtrChangeWindow.ContralState = eOA_FlowControlState.eReadOnly;
            #endregion
            #region 是否停用服务
            if (CtrStopServer.ContralState != eOA_FlowControlState.eHidden)
                CtrStopServer.ContralState = eOA_FlowControlState.eReadOnly;
            #endregion
            #region 是否业务影响
            if (CtrBusEffect.ContralState != eOA_FlowControlState.eHidden)
                CtrBusEffect.ContralState = eOA_FlowControlState.eReadOnly;
            #endregion
            #region 是否数据影响
            if (CtrDataEffect.ContralState != eOA_FlowControlState.eHidden)
                CtrDataEffect.ContralState = eOA_FlowControlState.eReadOnly;
            #endregion
            #region 计划开始时间
            if (CtrPlanStartTime.ContralState != eOA_FlowControlState.eHidden)
                CtrPlanStartTime.ContralState = eOA_FlowControlState.eReadOnly;
            #endregion
            #region 计划完成时间
            if (CtrPlanEndTime.ContralState != eOA_FlowControlState.eHidden)
                CtrPlanEndTime.ContralState = eOA_FlowControlState.eReadOnly;
            #endregion
            #region 实际开始时间
            if (CtrRealStartTime.ContralState != eOA_FlowControlState.eHidden)
                CtrRealStartTime.ContralState = eOA_FlowControlState.eReadOnly;
            #endregion
            //实际结束时间
            if (CtrRealEndTime.ContralState != eOA_FlowControlState.eHidden)
                CtrRealEndTime.ContralState = eOA_FlowControlState.eReadOnly;

            Extension_DayCtrList1.ReadOnly = true;    // 扩展属性 - 2013-11-19 @孙绍棕

            if (this.hidAppID.Value != "0")
            {
                //Extension_DayCtrList1.EquCategoryID = long.Parse(this.hidAppID.Value.ToString());
                Extension_DayCtrList1.NodeModelID = this.FlowMaster.oFlow.NodeModelID;

                Extension_DayCtrList1.EquID = long.Parse(this.hidFlowID.Value.ToString());
                Extension_DayCtrList1.EquCategoryID = this.FlowMaster.oFlow.FlowModelID;
                
            }            
        }

        #endregion

        #region Master_myGetFormsValue

        /// <summary>
        /// 数据赋值
        /// </summary>
        /// <param name="lngActionID"></param>
        /// <param name="strActionName"></param>
        /// <returns></returns>
        private XmlDocument Master_myGetFormsValue(long lngActionID, string strActionName)
        {
            #region Master_myGetFormsValue

            FieldValues fv = new FieldValues();
            fv.Add("EffectID", CtrFCDEffect.CatelogID.ToString().Trim());
            fv.Add("EffectName", CtrFCDEffect.CatelogValue.Trim());
            fv.Add("InstancyID", CtrFCDInstancy.CatelogID.ToString().Trim());
            fv.Add("InstancyName", CtrFCDInstancy.CatelogValue.Trim());
            fv.Add("LevelID", CtrFCDlevel.CatelogID.ToString().Trim());
            fv.Add("LevelName", CtrFCDlevel.CatelogValue.Trim());
            fv.Add("ChangeTypeID", CtrChangeType.CatelogID.ToString().Trim());
            fv.Add("ChangeTypeName", CtrChangeType.CatelogValue.ToString().Trim());
            string sServiceNo = this.labServiceNo.Text.Trim();
            if (sServiceNo == string.Empty)
            {
                sServiceNo = RuleCodeDP.GetCodeBH(10005);
            }
            fv.Add("ChangeNo", sServiceNo);
            fv.Add("Subject", CtrFlowFTSubject.Value.Trim());
            fv.Add("ChangeTime", CtrDTCustTime.dateTime.ToString("yyyy-MM-dd H:mm:ss"));
            fv.Add("Content", CtrFlowReContent.Value.Trim());
            fv.Add("Reason", String.Empty);
            fv.Add("CustID", this.hidCustID.Value.Trim() == string.Empty ? "0" : this.hidCustID.Value.Trim());
            fv.Add("CustName", txtCustAddr.Text.Trim());
            fv.Add("CustAddress", txtAddr.Text.Trim());
            fv.Add("Contact", txtContact.Text.Trim());
            fv.Add("CTel", txtCTel.Text.Trim());
            fv.Add("EquipmentID", "0");
            fv.Add("EquipmentName", "");
            fv.Add("ChangeAnalyses", CtrFlowReChangeAnalyses.Value.Trim());
            fv.Add("ChangeAnalysesResult", CtrFlowReChangeAnalysesResult.Value.Trim());
            fv.Add("Remark", string.Empty);
            fv.Add("DealStatusID", CtrFCDDealStatus.CatelogID.ToString());
            fv.Add("DealStatus", CtrFCDDealStatus.CatelogValue.Trim());
            fv.Add("RegUserID", Session["UserID"].ToString());
            fv.Add("RegUserName", Session["PersonName"].ToString());
            fv.Add("RegDeptID", Session["UserDeptID"].ToString());
            fv.Add("RegDeptName", Session["UserDeptName"].ToString());
            fv.Add("RegOrgID", Session["UserOrgID"].ToString());
            fv.Add("RegTime", DateTime.Now.ToString());
            fv.Add("FromFlowID", lngFromFlowID.ToString());
            fv.Add("EmailNotify", myFlowForms.EmailValue.ToString());
            fv.Add("SMSNotify", myFlowForms.NotifyValue.ToString());
            fv.Add("DeskChange", DeskChangeUrl);

            fv.Add("FromProblemFlowID", lngProblemFlowID.ToString());            //问题单传过来的问题单FlowID

            string ExtensionDayList = SaveExtensionDayList();
            fv.Add("ExtensionDayList", ExtensionDayList);  //扩展项

            #region 更改 增加提交时加入 客户邮箱参数 余向前 2013-04-11
            //如果配置了给客户发邮件 下面添加的代码才会发生作用
            fv.Add("Email", hidCustEmail.Value.ToString());
            #endregion

            #region 20160720 add
            string IsPlanChange = "";
            int week = Convert.ToInt32(DateTime.Today.DayOfWeek);//周几
            TimeSpan ts = this.CtrPlanStartTime.dateTime - DateTime.Now;
            int ts_day = Math.Abs(ts.Days);
            if (!IsInSameWeek(DateTime.Now, CtrPlanStartTime.dateTime))//超1周
            {
                IsPlanChange = "1";
            }
            else
            {
                IsPlanChange = "0";
            }
            fv.Add("IS_PLAN_CHANGE", IsPlanChange);
            fv.Add("ChangeNeedPeopleID", this.ctrChangeNeedPeople.UserID.ToString() == "" ? "0" : this.ctrChangeNeedPeople.UserID.ToString());//变更需求人ID
            fv.Add("ChangeNeedPeople", this.ctrChangeNeedPeople.UserName);//变更需求人
            string placeNameID = "";
            string placeName = "";
            for (int i = 0; i < this.chkChangePlace.Items.Count; i++)
            {
                if (this.chkChangePlace.Items[i].Selected)
                {
                    placeNameID += this.chkChangePlace.Items[i].Value + ",";
                    placeName += this.chkChangePlace.Items[i].Text + ",";
                }
            }
            fv.Add("Isplan", this.ddlIsplan.SelectedItem.Value == "1" ? this.ctrIsplan.Value.Trim() : this.ddlIsplan.SelectedItem.Text);//应急/回退方案
            fv.Add("CHANGE_PLACE_ID", placeNameID);
            fv.Add("CHANGE_PLACE_NAME", placeName);
            fv.Add("BUS_EFFECT", CtrBusEffect.TextValue);
            fv.Add("DATA_EFFECT", CtrDataEffect.TextValue);
            fv.Add("PLAN_BEGIN_TIME", CtrPlanStartTime.dateTimeString != "" ? CtrPlanStartTime.dateTime.ToString("yyyy-MM-dd HH:mm:ss") : "");
            fv.Add("PLAN_END_TIME", CtrPlanEndTime.dateTimeString != "" ? CtrPlanEndTime.dateTime.ToString("yyyy-MM-dd HH:mm:ss") : "");
            fv.Add("IS_BUS_EFFECT", CtrBusEffect.SelValue == "" ? "-1" : CtrBusEffect.SelValue);
            fv.Add("IS_DATA_EFFECT", CtrDataEffect.SelValue == "" ? "-1" : CtrDataEffect.SelValue);
            fv.Add("CHANGE_WINDOW_ID", CtrChangeWindow.SelValue == "" ? "0" : CtrChangeWindow.SelValue);
            fv.Add("CHANGE_WINDOW_NAME", CtrChangeWindow.SelText);
            fv.Add("CHANGE_WINDOW_REMARK", CtrChangeWindow.TextValue);
            fv.Add("IS_STOP_SERVER", CtrStopServer.SelValue == "" ? "-1" : CtrStopServer.SelValue);
            fv.Add("STOP_SERVER_REMARK", CtrStopServer.TextValue);
            fv.Add("REAL_BEGIN_TIME", CtrRealStartTime.dateTimeString != "" ? CtrRealStartTime.dateTime.ToString("yyyy-MM-dd HH:mm:ss") : "");
            //结束时间
            fv.Add("REAL_END_TIME", CtrRealEndTime.dateTimeString != "" ? CtrRealEndTime.dateTime.ToString("yyyy-MM-dd HH:mm:ss") : "");
            #endregion
            XmlDocument xmlDoc = fv.GetXmlObject();            

            #endregion


            return xmlDoc;
        }

        private string SaveExtensionDayList()
        {
            #region 扩展项

            List<EQU_deploy> list = Extension_DayCtrList1.contorRtnValue;

            string str = "";
            string strList = "";

            //保存配置信息
            foreach (EQU_deploy deploy in list)
            {
                string[] strArray = new string[1];
                strList = deploy.ID + "@" + deploy.EquID + "@" + deploy.CHName + "@" + deploy.FieldID
                   + "@" + deploy.Value;
                str += strList + "&";

            }

            #endregion
            return str;
        }
        #endregion

        #region myFlowForms_myPreSaveClickCustomize

        /// <summary>
        /// 暂存时保存数据
        /// </summary>
        /// <returns></returns>
        bool myFlowForms_myPreSaveClickCustomize()
        {
            bool blnRet = GetEquStatus();
            if (blnRet)
            {
                SaveDetailItem();
            }
            return blnRet;
        }
        #endregion

        #region 保存数据 SaveDetailItem

        /// <summary>
        /// 保存资产数据
        /// </summary>
        /// <returns></returns>
        private bool SaveDetailItem()
        {
            try
            {
                string equId = "";
                DataTable dt = GetDetailItem(true, 0, ref equId);
                gvBillItem.DataSource = dt.DefaultView;
                gvBillItem.DataBind();
                ChangeDealDP.SaveCLFareDetailItem(dt, DeskChangeUrl);
                Session["ProblemFlowID"] = null;
                Session["ExtendParameter"] = null;
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region 保存数据 myFlowForms_myPreClickCustomize

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="lngActionID"></param>
        /// <param name="strActionName"></param>
        /// <returns></returns>
        bool myFlowForms_myPreClickCustomize(long lngActionID, string strActionName)
        {
            string hidId = "";
            DataTable dt = GetDetailItem(true, 0, ref hidId);
            gvBillItem.DataSource = dt.DefaultView;
            gvBillItem.DataBind();
            bool blnRet = GetEquStatus();
            long lngEquID = 0;
            if (blnRet == true && lngActionID > 0 && lngEquID > 0)
            {
                //判断如果是应用变更的情况下,检查是否变更操作已经完成
                long lngBusID = MessageDep.GetBusinessActionID(long.Parse(this.FlowModelID), long.Parse(this.NodeModelID), lngActionID);
                if (lngBusID == 10000)
                {
                    Equ_DeskDP ee = new Equ_DeskDP();
                    blnRet = ee.GetTempChangeExist(long.Parse(this.FlowID));
                }
                if (blnRet == false)
                {
                    blnRet = true;
                }
            }

            //判断明细表中的分类不能为空
            bool IsFillTypeName = false;
            string atxtEquName = "";
            foreach (DataGridItem row in gvBillItem.Controls[0].Controls)
            {
                if (row.ItemType == ListItemType.Item || row.ItemType == ListItemType.AlternatingItem)
                {
                    atxtEquName = ((TextBox)row.FindControl("txtEquName")).Text; // 资产名称
                    if (atxtEquName.Trim() != string.Empty)
                    {
                        IsFillTypeName = true;
                    }
                }
            }

            if (!IsFillTypeName)
            {
                blnRet = false;
                PageTool.MsgBox(this, "资产信息不能为空!");
            }

            if (blnRet)
            {
                SaveDetailItem();
            }

            return blnRet;
        }

        #endregion

        #region 取得总金额 GetDetailTotalAmount
        /// <summary>
        /// 取得总金额
        /// </summary>
        /// <returns></returns>
        private double GetDetailTotalAmount()
        {
            double dTotal = 0;

            foreach (DataGridItem row in gvBillItem.Items)
            {
                string stxtSubTotal = ((TextBox)row.FindControl("txtSubTotal")).Text;
                dTotal += stxtSubTotal == "" ? 0.0 : double.Parse(stxtSubTotal);
            }
            return dTotal;
        }
        #endregion

        #region 取得详细资料

        /// <summary>
        ///  取得详细资料
        /// </summary>
        /// <returns></returns>
        private DataTable GetDetailItem()
        {
            string equId = "";
            return GetDetailItem(false, 0, ref equId);
        }

        /// <summary>
        /// 获取明细资料
        /// </summary>
        /// <param name="isAll"></param>
        /// <returns></returns>
        private DataTable GetDetailItem(bool isAll, int indexs, ref string strHidAddValue)
        {

            #region 定义变量

            Equ_DeskDP equDesk = new Equ_DeskDP();
            DataTable dt = new DataTable();
            int iCostID = 0;
            DataRow dr;//数据行

            #endregion

            #region 构建列

            dt.Columns.Add("DETAILSID", Type.GetType("System.Decimal"));
            dt.Columns.Add("EQUID", Type.GetType("System.Decimal"));
            dt.Columns.Add("EQUNAME", Type.GetType("System.String"));
            dt.Columns.Add("LISTID", Type.GetType("System.Decimal"));
            dt.Columns.Add("LISTNAME", Type.GetType("System.String"));
            dt.Columns.Add("EQUCODE", Type.GetType("System.String"));
            dt.Columns.Add("CHANGECONTENT", Type.GetType("System.String"));
            dt.Columns.Add("OLDVALUE", Type.GetType("System.String"));
            dt.Columns.Add("NEWVALUE", Type.GetType("System.String"));
            dt.Columns.Add("CHANGEDATE", Type.GetType("System.String"));
            dt.Columns.Add("CHANGEUSERID", Type.GetType("System.Decimal"));
            dt.Columns.Add("CHANGEUSERNAME", Type.GetType("System.String"));
            dt.Columns.Add("CHANGEDEPTID", Type.GetType("System.Decimal"));
            dt.Columns.Add("CHANGEDEPTNAME", Type.GetType("System.String"));
            dt.Columns.Add("CHANGESTATUS", Type.GetType("System.Decimal"));
            dt.Columns.Add("Remark", Type.GetType("System.String"));
            dt.Columns.Add("DEPT", Type.GetType("System.String"));

            #endregion

            #region 构建DataTable

            foreach (DataGridItem row in gvBillItem.Controls[0].Controls)
            {
                iCostID++;
                if (row.ItemType == ListItemType.Footer)
                {
                    string aEquID = ((HtmlInputHidden)row.FindControl("hidAddID")).Value; //资产ID
                    string aEquName = ((HtmlInputHidden)row.FindControl("hidAddEquName")).Value; //名称
                    string aCode = ((HtmlInputHidden)row.FindControl("hidAddCode")).Value; // 编号
                    string aListID = ((HtmlInputHidden)row.FindControl("hidAddListID")).Value; //目录ID
                    string aListName = ((HtmlInputHidden)row.FindControl("hidAddListName")).Value; ;//目录名称
                    string aChangeContent = ((CtrFlowFormText)row.FindControl("CtrAddChangeContent")).Value;//变更内容
                    string aChangeOld = ((CtrFlowFormText)row.FindControl("CtrAddOldValue")).Value; //变更原值
                    string aChangeNew = ((CtrFlowFormText)row.FindControl("CtrAddNewValue")).Value;   //变更后的值
                    string aRemark = ((CtrFlowFormText)row.FindControl("CtrAddNewValue")).Value;  //备注

                    aEquID = aEquID == "" ? "0" : aEquID;
                    aListID = aListID == "" ? "0" : aListID;
                    if (row.ItemIndex == indexs)
                    {
                        strHidAddValue = aEquID;
                    }
                    if (aEquID != "0")
                    {
                        equDesk = equDesk.GetReCorded(long.Parse(aEquID));
                        dr = dt.NewRow();
                        dr["DETAILSID"] = 0;
                        dr["EQUID"] = decimal.Parse(aEquID);
                        dr["EQUNAME"] = aEquName;
                        dr["LISTID"] = decimal.Parse(aListID);
                        dr["LISTNAME"] = aListName;
                        dr["EQUCODE"] = aCode;
                        dr["CHANGECONTENT"] = aChangeContent;
                        dr["OLDVALUE"] = aChangeOld;
                        dr["NEWVALUE"] = aChangeNew;
                        dr["CHANGEDATE"] = "";
                        dr["CHANGEUSERID"] = 0;
                        dr["CHANGEUSERNAME"] = "";
                        dr["CHANGEDEPTID"] = 0;
                        dr["CHANGEDEPTNAME"] = "";
                        dr["CHANGESTATUS"] = 0;
                        dr["Remark"] = aRemark;
                        dr["DEPT"] = equDesk.partBranchName;
                        dt.Rows.Add(dr);
                    }
                    else
                    {
                        if (!isAll)
                            PageTool.MsgBox(this, "资产信息不能为空！");
                    }
                }
                else if (row.ItemType == ListItemType.Item || row.ItemType == ListItemType.AlternatingItem)
                {
                    string EquID = ((HtmlInputHidden)row.FindControl("hidID")).Value; //产品ID
                    string EquName = ((HtmlInputHidden)row.FindControl("hidEquName")).Value; //名称
                    string Code = ((HtmlInputHidden)row.FindControl("hidCode")).Value; // 编号
                    string TypeID = ((HtmlInputHidden)row.FindControl("hidListID")).Value; //类别ID
                    string TypeName = ((HtmlInputHidden)row.FindControl("hidListName")).Value;// ((EquPicker)row.FindControl("epList")).EpuName;  //类别名称
                    string ChangeContent = ((CtrFlowFormText)row.FindControl("CtrChangeContent")).Value;//变更内容
                    string ChangeOld = ((CtrFlowFormText)row.FindControl("CtrOldValue")).Value; //变更原值
                    string ChangeNew = ((CtrFlowFormText)row.FindControl("CtrNewValue")).Value;   //变更后的值
                    string Remark = ((CtrFlowFormText)row.FindControl("CtrNewValue")).Value;  //备注


                    EquID = EquID == "" ? "0" : EquID;
                    TypeID = TypeID == "" ? "0" : TypeID;
                    if (row.ItemIndex == indexs)
                    {
                        strHidAddValue = EquID;
                    }
                    if (EquID != "0")
                    {
                        equDesk = equDesk.GetReCorded(long.Parse(EquID));
                        dr = dt.NewRow();
                        dr["DETAILSID"] = 0;
                        dr["EQUID"] = decimal.Parse(EquID);
                        dr["EQUNAME"] = EquName;
                        dr["LISTID"] = decimal.Parse(TypeID);
                        dr["LISTNAME"] = TypeName;
                        dr["EQUCODE"] = Code;
                        dr["CHANGECONTENT"] = ChangeContent;
                        dr["OLDVALUE"] = ChangeOld;
                        dr["NEWVALUE"] = ChangeNew;
                        dr["CHANGEDATE"] = "";
                        dr["CHANGEUSERID"] = 0;
                        dr["CHANGEUSERNAME"] = "";
                        dr["CHANGEDEPTID"] = 0;
                        dr["CHANGEDEPTNAME"] = "";
                        dr["CHANGESTATUS"] = 0;
                        dr["Remark"] = Remark;
                        dr["DEPT"] = equDesk.partBranchName;
                        dt.Rows.Add(dr);
                    }
                }
            }

            #endregion

            return dt;//返回数据
        }
        #endregion

        #region  绑定变更资产明细 BindGrid
        /// <summary>
        /// 绑定变更资产明细
        /// </summary>
        /// <param name="id"></param>
        private void BindGrid(long id)
        {
            #region 获取数据

            DataTable dtItem = ChangeDealDP.GetCLFareItem(id);

            dtItem = CheckUnSaveDataAndLoadIt(dtItem);

            gvBillItem.DataSource = dtItem;
            gvBillItem.DataBind();
            gvBillItem.Visible = true;

            #endregion
        }
        #endregion

        #region 设置资产为只读 SetFareDetailReadOnly
        /// <summary>
        /// 设置资产为只读
        /// </summary>
        private void SetFareDetailReadOnly()
        {
            foreach (DataGridItem row in gvBillItem.Items)
            {
                ((HtmlInputButton)row.FindControl("cmdEqu")).Visible = false;
                ((TextBox)row.FindControl("txtEquName")).Visible = false;
                ((TextBox)row.FindControl("txtEquName")).Attributes.Remove("onmousemove");
                ((Label)row.FindControl("lblEquName")).Visible = true;
                ((HtmlInputButton)row.FindControl("cmdListName")).Visible = false;
                ((TextBox)row.FindControl("txtListName")).Visible = false;
                ((Label)row.FindControl("lblListName")).Visible = true;
                ((CtrFlowFormText)row.FindControl("CtrChangeContent")).ContralState = eOA_FlowControlState.eReadOnly;
                ((CtrFlowFormText)row.FindControl("CtrOldValue")).ContralState = eOA_FlowControlState.eReadOnly;
                ((CtrFlowFormText)row.FindControl("CtrNewValue")).ContralState = eOA_FlowControlState.eReadOnly;

                gvBillItem.Columns[gvBillItem.Columns.Count - 2].Visible = false;
            }
            gvBillItem.ShowFooter = false;
        }

        private void SetFareDetailvisible(string text, bool flag)
        {
            foreach (DataGridItem row in gvBillItem.Items)
            {
                switch (text.ToLower())
                {
                    case "desktype":
                        ((HtmlInputButton)row.FindControl("cmdListName")).Visible = false;
                        ((TextBox)row.FindControl("txtListName")).Visible = false;
                        ((Label)row.FindControl("lblListName")).Visible = true;
                        if (flag == false)
                        {
                            ((Label)row.FindControl("lblListName")).Visible = false;
                        }
                        break;
                    case "equipmentname":

                        hidEquIsHid.Value = "1";            //表明资产为只读
                        SetFareDetailReadOnly();

                        if (flag == false)
                        {
                            hidEquIsHid.Value = "2";        //表明资产为不可见
                            //整个资产datagrid都隐藏
                            gvBillItem.Visible = false;
                        }
                        break;
                    case "oldvalue":
                        ((CtrFlowFormText)row.FindControl("CtrOldValue")).ContralState = eOA_FlowControlState.eReadOnly;
                        if (flag == false)
                        {
                            ((CtrFlowFormText)row.FindControl("CtrOldValue")).ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    case "newvalue":
                        ((CtrFlowFormText)row.FindControl("CtrNewValue")).ContralState = eOA_FlowControlState.eReadOnly;
                        if (flag == false)
                        {
                            ((CtrFlowFormText)row.FindControl("CtrNewValue")).ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    case "impactanalysis":
                        gvBillItem.Columns[gvBillItem.Columns.Count - 3].Visible = false;
                        break;
                    case "adddetails":
                        gvBillItem.Columns[gvBillItem.Columns.Count - 2].Visible = false;
                        gvBillItem.ShowFooter = false;
                        break;
                    default:
                        break;
                }
            }

            if (text == "equchange")
            {
                gvBillItem.Columns[gvBillItem.Columns.Count - 1].Visible = false;
                hidEquEdit.Value = "0";
            }
        }
        #endregion

        #region 明细新增，删除事件 gvBillItem_ItemCommand

        /// <summary>
        /// 费用明细新增，删除事件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void gvBillItem_ItemCommand(object source, DataGridCommandEventArgs e)
        {

            DataTable dt = new DataTable();
            if (e.CommandName == "Delete")
            {
                string hidId = "";
                dt = GetDetailItem(true, e.Item.ItemIndex, ref hidId);
                Equ_DeskDP.DeleteEquChange(long.Parse(FlowID), long.Parse(hidId), long.Parse(ChangeID.ToString()));
                dt.Rows.RemoveAt(e.Item.ItemIndex);

                /*     
                 * Date: 2013-07-04 10:22
                 * summary: 会话参数 Session["frm_changebase_aspx_cs_gvBillItem_unsaved_data"] 的说明见
                 * CheckUnSaveDataAndLoadIt 方法.
                 * 
                 * modified: sunshaozong@gmail.com     
                 */

                Session["frm_changebase_aspx_cs_gvBillItem_unsaved_data"] = dt;

                gvBillItem.DataSource = dt.DefaultView;
                gvBillItem.DataBind();

            }
            else if (e.CommandName == "Add")
            {
                string hidId = "";
                dt = GetDetailItem(false, e.Item.ItemIndex, ref hidId);

                Session["frm_changebase_aspx_cs_gvBillItem_unsaved_data"] = dt;

                gvBillItem.DataSource = dt.DefaultView;
                gvBillItem.DataBind();
            }

        }
        #endregion

        protected void gvBillItem_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer || e.Item.ItemType == ListItemType.Footer)
            {
                HiddenField hiddenfield = (HiddenField)e.Item.FindControl("hidClientId_ForOpenerPage");
                HtmlInputButton btn = (HtmlInputButton)e.Item.FindControl("cmdAddEqu");
                string url = "../EquipmentManager/frmEqu_DeskMain.aspx?IsSelect=true&EquipmentCatalogID=0&Opener_ClientId=" + hiddenfield.ClientID + "&TypeFrm=frm_ChangeBase";
                // e.Item.Attributes.Add("onclick", "window.open('" + url + "', '',  'resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=700,height=850px,left=150,top=50');");
                btn.Attributes.Add("onclick", "window.open('" + url + "', '',  'scrollbars=yes,resizable=yes,width=700px,height=600px,left=150px,top=50px');");

            }
        }
        #region GetUrl
        /// <summary>
        /// GetUrl
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        public string GetUrlEquId(decimal equID)
        {
            //暂时没处理分页

            string sUrl = "";
            Random ro = new Random();
            string random = ro.Next().ToString();
            sUrl = "javascript:window.open('frmEqu_ImpactAnalysis.aspx?EquId=" + equID + "&ChangeBillFlowID=" + FlowID + "&randomid=" + random + " ','','scrollbars=yes,status=yes ,resizable=yes,width=800,height=600'); ";
            //  sUrl = "javascript:window.open('../Forms/frmIssueView.aspx?NewWin=true&FlowID=" + lngFlowID.ToString() + "','','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
            return sUrl;
        }
        #endregion


        #region 检查未保存的资产信息并加载到 gvBillItem 中. - 2013-07-04 @孙绍棕
        /// <summary>
        /// 检查未保存的资产信息。若有，就加载到 gvBillItem 中。
        /// </summary>
        /// <param name="dt">资产信息表</param>
        /// <returns></returns>
        private DataTable CheckUnSaveDataAndLoadIt(DataTable dt)
        {
            /*     
             * Date: 2013-07-04 09:55
             * summary: 检查检查未保存的资产信息。若有，就加载到 gvBillItem 中。
             * 
             * 参数说明,
             * from_equ_deskedit: 表示是从 frmEqu_DeskEdit.aspx.cs 文件中的 Master_Master_Button_GoHistory_Click 方法跳转过来的。
             * from_equ_deskedit 合法值：yes
             * 
             * Session["frm_changebase_aspx_cs_gvBillItem_unsaved_data"]：未经保存的资产信息
             * Session["frm_changebase_aspx_cs_gvBillItem_unsaved_data"] 合法值：DataTable 类型, 其表结构和 gvBillItem 控件中绑定的数据源表结构一致。
             * 
             * 目标: 修复在变更单 > 资产信息 中新增资产, 然后点击变更配置项按钮跳转到资产资料编辑页面，再点返回按钮回去时，原先所选的资产不见
             * 了的问题。
             * 
             * modified: sunshaozong@gmail.com     
             */

            String strFromEquDeskEdit = Request.QueryString["from_equ_deskedit"];
            if (strFromEquDeskEdit == null)
            {
                Session["frm_changebase_aspx_cs_gvBillItem_unsaved_data"] = null;
                return dt;
            }

            strFromEquDeskEdit = strFromEquDeskEdit.Trim().ToLower();

            if (!strFromEquDeskEdit.Equals("yes")) return dt;

            DataTable dtUnsavedEquData = Session["frm_changebase_aspx_cs_gvBillItem_unsaved_data"] as DataTable;
            if (dtUnsavedEquData == null || dtUnsavedEquData.Rows.Count <= 0) return dt;

            Boolean isHave = dtUnsavedEquData.Rows.Count > dt.Rows.Count;

            if (!isHave) return dt;

            Equ_DeskDP ee = new Equ_DeskDP();

            for (int intStartIdx = dt.Rows.Count; intStartIdx < dtUnsavedEquData.Rows.Count; intStartIdx++)
            {
                DataRow row = dtUnsavedEquData.Rows[intStartIdx];

                DataTable dtEqu = ee.GetDataTable(String.Format(" and id = {0} ", row["EQUID"]), "");
                if (dtEqu.Rows.Count <= 0)
                {
                    continue;    // 若该资产在数据库中已被删除, 则跳过.
                }

                DataRow drUpdated = dtEqu.Rows[0];
                Object[] arrRow = null;

                if (dt.Columns.Contains("CHANGESTATUS"))
                {
                    arrRow = WrapChangeRow(row, drUpdated);
                }
                else
                {
                    arrRow = WrapProblemOrIssueRow(row, drUpdated);
                }


                dt.Rows.Add(arrRow);
            }

            return dt;
        }


        private Object[] WrapProblemOrIssueRow(DataRow row, DataRow rowUpdated)
        {
            List<Object> listRow = new List<object>();

            listRow.Add(row["LISTID"]);
            listRow.Add(row["LISTNAME"]);
            listRow.Add(row["EQUID"]);
            listRow.Add(rowUpdated["name"]);
            listRow.Add(rowUpdated["PARTBRANCHNAME"]);
            listRow.Add(rowUpdated["code"]);
            listRow.Add(row["CHANGECONTENT"]);
            listRow.Add(row["OLDVALUE"]);
            listRow.Add(row["NEWVALUE"]);
            listRow.Add(row["Remark"]);

            return listRow.ToArray();
        }

        private Object[] WrapChangeRow(DataRow row, DataRow rowUpdated)
        {
            List<Object> listRow = new List<object>();

            listRow.Add(row["DETAILSID"]);
            listRow.Add(row["EQUID"]);
            listRow.Add(rowUpdated["name"]);
            listRow.Add(rowUpdated["code"]);
            listRow.Add(row["CHANGECONTENT"]);
            listRow.Add(row["OLDVALUE"]);
            listRow.Add(row["NEWVALUE"]);
            listRow.Add(row["CHANGEDATE"]);
            listRow.Add(row["CHANGEUSERID"]);
            listRow.Add(row["CHANGEUSERNAME"]);

            listRow.Add(row["CHANGEDEPTID"]);
            listRow.Add(row["CHANGEDEPTNAME"]);
            listRow.Add(row["CHANGESTATUS"]);
            listRow.Add(-1);
            listRow.Add(row["Remark"]);
            listRow.Add(row["LISTID"]);
            listRow.Add(row["LISTNAME"]);


            listRow.Add(DateTime.Now);
            listRow.Add(DateTime.Now);
            listRow.Add(rowUpdated["PARTBRANCHNAME"]);

            return listRow.ToArray();
        }
        #endregion
        private void GetChangePlace()
        {
            DataTable dt = CatalogDP.GetNextCatalogs(1058);
            if (dt == null)
            {
                return;
            }
            this.chkChangePlace.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ListItem item = new ListItem();
                item.Text = dt.Rows[i]["CataLogName"].ToString();
                item.Value = dt.Rows[i]["catalogID"].ToString();
                this.chkChangePlace.Items.Add(item);
            }
        }
        /// <summary> 
        /// 判断两个日期是否在同一周 
        /// </summary> 
        /// <param name="dtmS">开始日期</param> 
        /// <param name="dtmE">结束日期</param>
        /// <returns></returns> 
        public static bool IsInSameWeek(DateTime dtmS, DateTime dtmE)
        {
            //获取两个时间差
            TimeSpan ts = dtmE - dtmS;
            //获取两个时间的相差天数
            double dbl = ts.TotalDays;
            //获取后面的结束时间的星期号并转成数字一到六为1-6，0为星期天
            int intDow = Convert.ToInt32(dtmE.DayOfWeek);
            if (intDow == 0) intDow = 7;
            //如果相差的天数大于6或相差的天数大于或等于后面的时间转成的数字就不是在同一周内
            if (dbl >= 7 || dbl >= intDow) return false;
            else return true;
        }
    }
}
