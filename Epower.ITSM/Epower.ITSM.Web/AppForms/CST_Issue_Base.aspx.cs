/****************************************************************************
 * 
 * description:故障单登记流程应用表单
 * 
 * 
 * 
 * Create by: yxq
 * Create Date:2011-08-02
 * *************************************************************************/
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Drawing;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using EpowerCom;
using EpowerGlobal;
using Epower.DevBase.BaseTools;
using System.Xml;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using appDataProcess;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;
using System.Text;
using Epower.ITSM.SqlDAL.ES_TBLCS;
using System.Collections.Generic;
using Epower.DevBase.Organization;

namespace Epower.ITSM.Web.AppForms
{
    public partial class CST_Issue_Base : BasePage
    {
        #region 变量
        private objFlow oFlow;
        private FlowForms myFlowForms;
        private long lngRequestID = 0;   //2009-04-28 增加 表示 公共请求的 ID
        protected bool FlowIsClosed = false;  //是否流程结束
        protected bool IsEmailFeedBack = true; //是否显示邮件回访
        #endregion

        #region 属性
        /// <summary>
        /// 取得流程FlowID
        /// </summary>
        public string FlowID
        {
            get
            {
                if (ViewState["frm_Issue_Base_FlowID"] != null)
                {
                    return ViewState["frm_Issue_Base_FlowID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["frm_Issue_Base_FlowID"] = value;
            }
        }

        /// <summary>
        /// 应用id
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
        /// <summary>
        /// 流程模型id
        /// </summary>
        public string FlowModelID
        {
            get
            {
                if (ViewState["frm_Issue_Base_FlowModelID"] != null)
                {
                    return ViewState["frm_Issue_Base_FlowModelID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["frm_Issue_Base_FlowModelID"] = value;
            }
        }

        /// <summary>
        /// 相关问题单ID
        /// </summary>
        public string ProblemFlowID
        {
            get
            {
                if (ViewState["ProblemFlowID"] != null)
                {
                    return ViewState["ProblemFlowID"].ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                ViewState["ProblemFlowID"] = value;
            }
        }

        /// <summary>
        /// 相关变更单ID
        /// </summary>
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
            set
            {
                ViewState["ChangeFlowID"] = value;
            }
        }

        /// <summary>
        /// 相关重复事件单
        /// </summary>
        public string IssuesSub
        {
            get
            {
                if (ViewState["IssuesSub"] != null)
                {
                    return ViewState["IssuesSub"].ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                ViewState["IssuesSub"] = value;
            }
        }

        /// <summary>
        /// 合并重复事件于
        /// </summary>
        public string IssuesSubRel
        {
            get
            {
                if (ViewState["IssuesSubRel"] != null)
                {
                    return ViewState["IssuesSubRel"].ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                ViewState["IssuesSubRel"] = value;
            }
        }

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
        public string StrCustomVisble()
        {
            if (Table15.Visible == false && Table12.Visible == false)
            {
                return "bool";
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 取得CatelogID
        /// </summary>
        public string CatelogID
        {
            get
            {
                return ctrFCDServiceType.CatelogID.ToString();
            }
        }

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

        public string EngineerEquJsonString
        { set; get; }

        private List<EQU_deploy> mEquDeploy = new List<EQU_deploy>();
        public List<EQU_deploy> EquDeploy
        {
            get { return mEquDeploy; }
            set { mEquDeploy = value; }
        }
        #endregion

        #region 初始化脚本 InitClientScript
        /// <summary>
        /// 初始化脚本
        /// </summary>
        private void InitClientScript()
        {

        }
        #endregion

        #region 页面初始化 Page_Load
        /// <summary>
        /// 页面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.gvBillItem.ItemDataBound += new DataGridItemEventHandler(gvBillItem_ItemDataBound);

            myFlowForms = (FlowForms)this.Master;
            myFlowForms.mySetContentReadOnly += new FlowForms.DoContentActions(Master_mySetContentReadOnly);
            myFlowForms.myGetFormsValue += new FlowForms.GetFormsValue(Master_myGetFormsValue);
            myFlowForms.mySetFormsValue += new FlowForms.DoContentActions(Master_mySetFormsValue);
            myFlowForms.myPreClickCustomize += new FlowForms.DoContentSubmitValid(Master_myPreClickCustomize);
            myFlowForms.myPreSaveClickCustomize += new FlowForms.DoContentValid(Master_myPreSaveClickCustomize);

            string strThisMsg = "";
            strThisMsg = txtCustAddr.ClientID + ">" + "客户";
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), txtCustAddr.ClientID, "<script type=\"text/javascript\" >if(typeof(sarrvalidlist) == 'undefined'){ var sarrvalidlist = new Array(1);}  sarrvalidlist[0] = sarrvalidlist[0] + '|' + '" + strThisMsg + "';</script>");

            string strThisMsgServiceLevel = "";
            strThisMsgServiceLevel = txtServiceLevel.ClientID + ">" + PageDeal.GetLanguageValue("CST_ServiceLevel");
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), txtServiceLevel.ClientID, "<script type=\"text/javascript\" >if(typeof(sarrvalidlist) == 'undefined'){ var sarrvalidlist = new Array(1);}  sarrvalidlist[0] = sarrvalidlist[0] + '|' + '" + strThisMsgServiceLevel + "';</script>");

            EngineerEquJsonString = JsonUtil.ChangeToJson(new EngineerSourceServer().GetEqu_DeskTable()).ToString();

            if (Page.IsPostBack == false)
            {
                txtCustAddr.Text = hidCust.Value;
                InitClientScript();

                trEqu.Visible = true;

                if (Session["IssueShortCutReqPubID"] != null)
                {
                    if (Session["IssueShortCutReqPubID"].ToString() != "")
                    {
                        lngRequestID = long.Parse(Session["IssueShortCutReqPubID"].ToString());
                    }
                    Session.Remove("IssueShortCutReqPubID");
                }
                ViewState["IssueShortCutReqPubID"] = lngRequestID;

                PageDeal.SetLanguage(this.Controls[0].Controls[1]);
            }
            else
            {
                lngRequestID = long.Parse(ViewState["IssueShortCutReqPubID"].ToString());

                if (txtCustAddr.Text.Trim() == string.Empty)
                    txtCustAddr.Text = hidCust.Value.Trim();
                if (txtContact.Text.Trim() == string.Empty)
                    txtContact.Text = hidContact.Value.Trim();
                if (txtCTel.Text.Trim() == string.Empty)
                    txtCTel.Text = hidTel.Value.Trim();
                if (txtAddr.Text.Trim() == string.Empty)
                    txtAddr.Text = hidAddr.Value.Trim();
                if (txtEqu.Text.Trim() == String.Empty)
                    txtEqu.Text = hidEquName.Value.Trim();

                if (txtListName.Text == String.Empty)
                    txtListName.Text = hidListName.Value.Trim();

                if (hidCustDeptName.Value.Trim() != string.Empty || lblCustDeptName.Text.Trim() == string.Empty)
                    lblCustDeptName.Text = hidCustDeptName.Value.Trim();

                if (hidCustEmail.Value.Trim() != string.Empty || lblEmail.Text.Trim() == string.Empty)
                    lblEmail.Text = hidCustEmail.Value.Trim();

                if (hidMastCust.Value.Trim() != string.Empty || lblMastCust.Text.Trim() == string.Empty)
                    lblMastCust.Text = hidMastCust.Value.Trim();
                if (hidjob.Value.Trim() != string.Empty || lbljob.Text.Trim() == string.Empty)
                    lbljob.Text = hidjob.Value.Trim();

                //获取隐藏域中的值给服务级别文本框赋值，防止页面刷新情况下文本框的值丢失了
                if (hidServiceLevel.Value.Trim() != string.Empty || txtServiceLevel.Text.Trim() == string.Empty)
                {
                    txtServiceLevel.Text = hidServiceLevel.Value.Trim();
                    ShowServiceLevel();
                }

            }

            CtrDTFinishedTime.OnChangeScript = "CalcuteTotalHours();";
            CtrDTOutTime.OnChangeScript = "CalcuteTotalHours();";
            ctrFCDServiceType.OnChangeScript = "CreateTitle();";

            if (CommonDP.GetConfigValue("Other", "QuickNewCust").Trim() == "1")
            {
                RightEntity re = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.CustumManager];
                bthCreateCus.Visible = re.CanAdd;

                RightEntity reEqu = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.EquManager];
                bthAddEqu.Visible = reEqu.CanAdd;
            }
            else
            {
                AddCustTD.Visible = false;
            }

            if (this.hidAppID.Value != "0")
            {
                //Extension_DayCtrList1.EquCategoryID = long.Parse(this.hidAppID.Value.ToString());


                EpowerCom.objFlow oFlow2 = new objFlow(long.Parse(Session["UserID"].ToString()), long.Parse(FlowModelID), long.Parse(MessageID));
                Extension_DayCtrList1.NodeModelID = oFlow2.NodeModelID;

                Extension_DayCtrList1.EquID = long.Parse(this.hidFlowID.Value.ToString());
                Extension_DayCtrList1.EquCategoryID = oFlow2.FlowModelID;

                
            }

            #region 是否显示邮件回访按钮 余向前 2013-05-02
            if (CommonDP.GetConfigValue("Other", "IsEmailFeedBack") != null && CommonDP.GetConfigValue("Other", "IsEmailFeedBack") == "0")
            {
                if (CheckRight(Constant.feedbackright) == false)
                {
                    IsEmailFeedBack = false;
                }
            }
            #endregion
        }
        #endregion

        #region 暂存时前面执行事件 Master_myPreSaveClickCustomize
        /// <summary>
        /// 提交前执行事件
        /// </summary>
        /// <returns></returns>
        bool Master_myPreSaveClickCustomize()
        {
            try
            {
                return SaveDetailItem();
            }
            catch (Exception ex)
            {
                E8Logger.Error(ex);
                throw;
            }

        }
        #endregion

        #region  提交前执行事件Master_myPreClickCustomize
        /// <summary>
        /// 暂存时前面执行事件
        /// </summary>
        /// <returns></returns>
        bool Master_myPreClickCustomize(long lngActionID, string strActionName)
        {
            bool flag = true;

            if (SaveDetailItem())
            {
                if (CtrReportingTime.dateTime < CtrDTCustTime.dateTime)
                {
                    PageTool.MsgBox(this, "报告时间不能小于发生时间!");
                    flag = false;
                }
                if (CtrReportingTime.dateTime > DateTime.Now)
                {
                    PageTool.MsgBox(this, "报告时间不能大于当前时间!");
                    flag = false;
                }
                if (CtrDTFinishedTime.dateTimeString != "")
                {
                    if (CtrDTFinishedTime.dateTime < CtrReportingTime.dateTime)
                    {
                        PageTool.MsgBox(this, "处理完成时间不能小于报告时间!");
                        flag = false;
                    }
                }

                if (CtrDTFinishedTime.dateTimeString != "")
                {
                    if (CtrDTFinishedTime.dateTime < CtrDTOutTime.dateTime)
                    {
                        PageTool.MsgBox(this, "处理完成时间不能小于派出时间!");
                        flag = false;
                    }
                }
            }
            else
            {
                flag = false;
            }
            return flag;
        }
        #endregion

        #region 设置窗体值 Master_mySetFormsValue

        private string GetTimeUnit(string code)
        {
            string ret = "单位";
            if (code == "0")
                ret = "分钟";
            if (code == "1")
                ret = "小时";
            if (code == "2")
                ret = "天";
            if (code == "3")
                ret = "工分";
            if (code == "4")
                ret = "工时";

            return ret;
        }

        private DateTime GetLimitedTimeValue(DateTime dt, string sValue, string code)
        {
            DateTime ret = dt;
            if (code == "0")
                ret = dt.AddMinutes(double.Parse(sValue));
            if (code == "1")
                ret = dt.AddHours(double.Parse(sValue));
            if (code == "2")
                ret = dt.AddDays(double.Parse(sValue));

            return ret;
        }

        /// <summary>
        /// 通过模版获取初值
        /// </summary>
        /// <param name="lngTemplateID"></param>
        private void SetFromFlowValues(long lngTemplateID)
        {
            EA_ShortCutTemplateDP ee = new EA_ShortCutTemplateDP();
            ee = ee.GetReCorded(lngTemplateID);

            if (ee.TemplateXml.Length != 0)
            {
                #region 获取模版值

                hidIssTempID.Value = lngTemplateID.ToString();//模板ID
                FieldValues fv = new FieldValues(ee.TemplateXml);
                hidServiceLevelID.Value = (fv.GetFieldValue("ServiceLevelID").Value == "" ? "0" : fv.GetFieldValue("ServiceLevelID").Value);
                hidServiceLevel.Value = fv.GetFieldValue("ServiceLevel").Value;
                txtServiceLevel.Text = hidServiceLevel.Value;
                hidServiceLevelChange.Value = "true";

                #region 事件类别

                if (fv.GetFieldValue("ServiceRootTypeID") != null && fv.GetFieldValue("ServiceRootTypeID").Value != "0")
                {
                    //如果获取的根分类ID不为0，则更改事件类别的下拉根分类
                    ctrFCDServiceType.RootID = long.Parse(fv.GetFieldValue("ServiceRootTypeID").Value);

                    if (fv.GetFieldValue("ServiceTypeID") != null && fv.GetFieldValue("ServiceTypeID").Value != "0" && fv.GetFieldValue("ServiceTypeID").Value != "")
                    {
                        //如果有默认初值，则设置选定的默认值
                        ctrFCDServiceType.CatelogID = long.Parse(fv.GetFieldValue("ServiceTypeID").Value);
                        ctrFCDServiceType.CatelogValue = fv.GetFieldValue("ServiceType").Value;
                    }
                }
                #endregion

                #region 影响度

                if (fv.GetFieldValue("RootEffectID") != null && fv.GetFieldValue("RootEffectID").Value != "0")
                {
                    //如果获取的根分类ID不为0，则更改事件类别的下拉根分类
                    CtrFCDEffect.RootID = long.Parse(fv.GetFieldValue("RootEffectID").Value);

                    if (fv.GetFieldValue("EffectID") != null && fv.GetFieldValue("EffectID").Value != "0" && fv.GetFieldValue("EffectID").Value != "")
                    {
                        //如果有默认初值，则设置选定的默认值
                        CtrFCDEffect.CatelogID = long.Parse(fv.GetFieldValue("EffectID").Value);
                        CtrFCDEffect.CatelogValue = fv.GetFieldValue("EffectID").Value;
                    }
                }
                #endregion

                #region 紧急度

                if (fv.GetFieldValue("RootInstancyID") != null && fv.GetFieldValue("RootInstancyID").Value != "0")
                {
                    //如果获取的根分类ID不为0，则更改事件类别的下拉根分类
                    CtrFCDInstancy.RootID = long.Parse(fv.GetFieldValue("RootInstancyID").Value);

                    if (fv.GetFieldValue("InstancyID") != null && fv.GetFieldValue("InstancyID").Value != "0" && fv.GetFieldValue("InstancyID").Value != "")
                    {
                        //如果有默认初值，则设置选定的默认值
                        CtrFCDInstancy.CatelogID = long.Parse(fv.GetFieldValue("InstancyID").Value);
                        CtrFCDInstancy.CatelogValue = fv.GetFieldValue("InstancyID").Value;
                    }
                }
                #endregion

                #region 事件来源

                if (fv.GetFieldValue("RootFromID") != null && fv.GetFieldValue("RootFromID").Value != "0")
                {
                    //如果获取的根分类ID不为0，则更改事件类别的下拉根分类
                    CtrReSouse.RootID = long.Parse(fv.GetFieldValue("RootFromID").Value);

                    if (fv.GetFieldValue("FromID") != null && fv.GetFieldValue("FromID").Value != "0" && fv.GetFieldValue("FromID").Value != "")
                    {
                        //如果有默认初值，则设置选定的默认值
                        CtrReSouse.CatelogID = long.Parse(fv.GetFieldValue("FromID").Value);
                        CtrReSouse.CatelogValue = fv.GetFieldValue("FromID").Value;
                    }
                }
                #endregion

                #region 关闭理由

                if (fv.GetFieldValue("RootReasonID") != null && fv.GetFieldValue("RootReasonID").Value != "0")
                {
                    //如果获取的根分类ID不为0，则更改事件类别的下拉根分类
                    CtrCloseReason.RootID = long.Parse(fv.GetFieldValue("RootReasonID").Value);

                    if (fv.GetFieldValue("ReasonID") != null && fv.GetFieldValue("ReasonID").Value != "0" && fv.GetFieldValue("ReasonID").Value != "")
                    {
                        //如果有默认初值，则设置选定的默认值
                        CtrCloseReason.CatelogID = long.Parse(fv.GetFieldValue("ReasonID").Value);
                        CtrCloseReason.CatelogValue = fv.GetFieldValue("ReasonID").Value;
                    }
                }
                #endregion

                //资产目录
                if (fv.GetFieldValue("EquListName") != null && fv.GetFieldValue("EquListID") != null)
                {
                    hidListID.Value = fv.GetFieldValue("EquListID").Value;
                    hidListName.Value = fv.GetFieldValue("EquListName").Value;
                    txtListName.Text = hidListName.Value;

                }
                #endregion

                #region 设置快捷内容
                if (Session["IssueShortCutReqSubject"] != null)
                {
                    CtrFlowFTSubject.Value = Session["IssueShortCutReqSubject"].ToString();
                    Session.Remove("IssueShortCutReqSubject");
                }

                if (Session["IssueShortCutReqContext"] != null)
                {
                    txtContent.Value = Session["IssueShortCutReqContext"].ToString();
                    Session.Remove("IssueShortCutReqContext");
                }
                if (Session["IssueShortCutReqCustName"] != null)
                {
                    if (Session["IssueShortCutReqCustName"].ToString() != "")
                    {
                        //有可能 模版配置,但快捷选其它人的情况
                        txtCustAddr.Text = Session["IssueShortCutReqCustName"].ToString();

                    }
                    Session.Remove("IssueShortCutReqCustName");
                }
                else
                {
                    //只有联系电话的情况下，比如公众请求
                    if (Session["IssueShortCutReqCTel"] != null)
                    {
                        txtCTel.Text = Session["IssueShortCutReqCTel"].ToString();
                        labCTel.Text = txtCTel.Text;
                        Session.Remove("IssueShortCutReqCTel");
                    }
                }

                if (Session["IssueShortCutReqEquName"] != null)
                {
                    if (Session["IssueShortCutReqEquName"].ToString() != "")
                    {
                        txtEqu.Text = Session["IssueShortCutReqEquName"].ToString();
                    }
                    Session.Remove("IssueShortCutReqEquName");
                }

                if (Session["IssueShortCutXml"] != null)
                {
                    //获取快捷登单的信息
                    FieldValues fv_Tmp = new FieldValues();
                    fv_Tmp = new FieldValues(Session["IssueShortCutXml"].ToString());

                    //详细描述
                    if (fv_Tmp.GetFieldValue("ReqContext") != null && fv_Tmp.GetFieldValue("ReqContext").Value.ToString() != "")
                    {
                        txtContent.Value = fv_Tmp.GetFieldValue("ReqContext").Value.ToString();
                    }

                    //客户
                    if (fv_Tmp.GetFieldValue("ReqCustID") != null && fv_Tmp.GetFieldValue("ReqCustID").Value.ToString() != "" && fv_Tmp.GetFieldValue("ReqCustID").Value.ToString() != "0")
                    {
                        hidCustID.Value = fv_Tmp.GetFieldValue("ReqCustID").Value.ToString();
                        txtCustAddr.Text = fv_Tmp.GetFieldValue("ReqCustName").Value.ToString();
                    }

                    //资产
                    if (fv_Tmp.GetFieldValue("EquID") != null && fv_Tmp.GetFieldValue("EquID").Value.ToString() != "" && fv_Tmp.GetFieldValue("EquID").Value.ToString() != "0")
                    {
                        hidEqu.Value = fv_Tmp.GetFieldValue("EquID").Value.ToString();
                        txtEqu.Text = fv_Tmp.GetFieldValue("ReqEquName").Value.ToString();
                    }
                }

                #endregion

                #region 服务级别详细
                /*
                if (hidServiceLevelID.Value != "" && hidServiceLevelID.Value != "0")
                {
                    trServiceLevelDetail.Attributes.Add("style", "display:");

                    Cst_SLGuidDP ee1 = new Cst_SLGuidDP();
                    DataTable dtTmp = ee1.GetDataByLevelIDCache(long.Parse(hidServiceLevelID.Value));

                    string sLimit = "";
                    foreach (DataRow r in dtTmp.Rows)
                    {
                        bool blnWX = false;  //响应时间超时
                        bool blnWF = false;  //完成时间超时

                        if (blnWX == true || blnWF == true)
                        {
                            sLimit += "<font color=red>";
                        }

                        sLimit += r["guidname"].ToString().Trim() + ":" + r["TimeLimit"].ToString().Trim() + GetTimeUnit(r["TimeUnit"].ToString().Trim()) + ",";

                        if (blnWX == true || blnWF == true)
                        {
                            sLimit += "</font>";
                        }
                    }
                    if (sLimit.EndsWith(","))
                        sLimit = sLimit.Substring(0, sLimit.Length - 1);

                    divSLTimeLimt.InnerHtml = "<table class='listContent' width='100%'> <tr><td class='listTitle' width='15%'>时限要求</td><td class='list'>" + sLimit + "</td></tr></table>";

                    Cst_ServiceLevelDP sld = new Cst_ServiceLevelDP();
                    sld = sld.GetReCorded(long.Parse(hidServiceLevelID.Value));
                    divSLDefinition.InnerHtml = "<table class='listContent' width='100%'> <tr><td class='listTitle' width='15%'>级别定义</td><td class='list'>" + sld.Definition + "</td></tr></table>";
                    lnkServiceLevel.Text = "详情";

                }
                 * */
                #endregion
                //展示服务级别
                ShowServiceLevel();

                #region 获取客户相关信息
                if (Session["IssueShortCutReqCustID"] != null && Session["IssueShortCutReqCustID"].ToString() != "0")
                {
                    hidCustID.Value = Session["IssueShortCutReqCustID"].ToString();
                    Session.Remove("IssueShortCutReqCustID");
                }
                if (hidCustID.Value.Trim() != "0" && hidCustID.Value.Trim() != "")
                {
                    Br_ECustomerDP ec = new Br_ECustomerDP();
                    ec = ec.GetReCorded(long.Parse(hidCustID.Value));
                    txtCustAddr.Text = ec.ShortName;
                    lblCustDeptName.Text = ec.CustDeptName;
                    hidCustDeptName.Value = ec.CustDeptName;
                    hidCustEmail.Value = ec.Email;
                    lblEmail.Text = ec.Email;
                    lblMastCust.Text = ec.ShortName;
                    lbljob.Text = ec.Job;
                    hidjob.Value = ec.Job;

                    txtAddr.Text = ec.Address;
                    hidAddr.Value = ec.Address;
                    lblAddr.Text = ec.Address;
                    txtContact.Text = ec.LinkMan1;
                    labContact.Text = StringTool.ParseForHtml(SpecTransText(txtContact.Text));
                    txtCTel.Text = ec.Tel1;
                    labCTel.Text = ec.Tel1;

                    Br_MastCustomerDP mc = new Br_MastCustomerDP();
                    mc = mc.GetReCorded((long)ec.MastCustID);
                    lblMastCust.Text = mc.ShortName;
                }

                #endregion

                #region 获取资产相关信息

                if (Session["IssueShortCutReqEquID"] != null && Session["IssueShortCutReqEquID"].ToString() != "0")
                {
                    hidEqu.Value = Session["IssueShortCutReqEquID"].ToString();
                    Session.Remove("IssueShortCutReqEquID");
                }
                if (hidEqu.Value.Trim() != "0" && hidEqu.Value.Trim() != "" && hidEqu.Value.Trim() != "-1")
                {
                    Equ_DeskDP edp = new Equ_DeskDP();
                    edp = edp.GetReCorded(long.Parse(hidEqu.Value.Trim()));
                    txtEqu.Text = edp.Name;
                    lblEqu.Text = edp.Name;
                    hidEquName.Value = edp.Name;

                    //资产分类
                    txtListName.Text = edp.ListName.ToString();
                    lblListName.Text = edp.ListName.ToString();
                    hidListName.Value = edp.ListName.ToString();
                    hidListID.Value = edp.ListID.ToString();

                }
                #endregion

            }
        }

        /// <summary>
        /// 通过模板设置类别Root值
        /// </summary>
        /// <param name="lngTemplateID"></param>
        private void SetRootCatalogId(long lngTemplateID)
        {
            EA_ShortCutTemplateDP ee = new EA_ShortCutTemplateDP();
            ee = ee.GetReCorded(lngTemplateID);

            if (ee.TemplateXml.Length != 0)
            {
                #region 获取模版值

                hidIssTempID.Value = lngTemplateID.ToString();//模板ID
                FieldValues fv = new FieldValues(ee.TemplateXml);

                #region 事件类别

                if (fv.GetFieldValue("ServiceRootTypeID") != null && fv.GetFieldValue("ServiceRootTypeID").Value != "0")
                {
                    //如果获取的根分类ID不为0，则更改事件类别的下拉根分类
                    ctrFCDServiceType.RootID = long.Parse(fv.GetFieldValue("ServiceRootTypeID").Value);
                }
                #endregion

                #region 影响度

                if (fv.GetFieldValue("RootEffectID") != null && fv.GetFieldValue("RootEffectID").Value != "0")
                {
                    //如果获取的根分类ID不为0，则更改事件类别的下拉根分类
                    CtrFCDEffect.RootID = long.Parse(fv.GetFieldValue("RootEffectID").Value);
                }
                #endregion

                #region 紧急度

                if (fv.GetFieldValue("RootInstancyID") != null && fv.GetFieldValue("RootInstancyID").Value != "0")
                {
                    //如果获取的根分类ID不为0，则更改事件类别的下拉根分类
                    CtrFCDInstancy.RootID = long.Parse(fv.GetFieldValue("RootInstancyID").Value);
                }
                #endregion

                #region 事件来源

                if (fv.GetFieldValue("RootFromID") != null && fv.GetFieldValue("RootFromID").Value != "0")
                {
                    //如果获取的根分类ID不为0，则更改事件类别的下拉根分类
                    CtrReSouse.RootID = long.Parse(fv.GetFieldValue("RootFromID").Value);
                }
                #endregion

                #region 关闭理由

                if (fv.GetFieldValue("RootReasonID") != null && fv.GetFieldValue("RootReasonID").Value != "0")
                {
                    //如果获取的根分类ID不为0，则更改事件类别的下拉根分类
                    CtrCloseReason.RootID = long.Parse(fv.GetFieldValue("RootReasonID").Value);
                }
                #endregion

                #endregion
            }

        }
        /// <summary>
        /// 邮件报障
        /// </summary>
        void setEmailFromValue()
        {
            if (Session["IssueEmailCutReqSubject"] != null)
            {
                CtrFlowFTSubject.Value = Session["IssueEmailCutReqSubject"].ToString();
                Session.Remove("IssueEmailCutReqSubject");
            }
            if (Session["IssueEmailCutReqContext"] != null)
            {
                txtContent.Value = StringTool.ParseHtmlForString(Session["IssueEmailCutReqContext"].ToString());
                Session.Remove("IssueEmailCutReqContext");
            }

        }

        /// <summary> 
        /// 设置窗体值
        /// </summary>
        void Master_mySetFormsValue()
        {
            #region 设置表单值
            oFlow = myFlowForms.oFlow;            

            if (!IsPostBack)
            {
                if (oFlow.FlowID != 0)
                {
                    myFlowForms.CtrButtons1.Button1Visible = true;
                    myFlowForms.CtrButtons1.ButtonName1 = "案例分析";
                    myFlowForms.CtrButtons1.Button1Function = "DoItemQuestionAnalysis(" + oFlow.AppID.ToString() + "," + oFlow.FlowID.ToString() + ");";
                }
                if (oFlow.FlowStatus == e_FlowStatus.efsEnd)
                {
                    myFlowForms.CtrButtons1.Button3Visible = true;
                    myFlowForms.CtrButtons1.ButtonName3 = "知识归档";
                    myFlowForms.CtrButtons1.Button3Function = "DoKmAdd(" + oFlow.MessageID.ToString() + "," + oFlow.AppID.ToString() + "," + oFlow.FlowID.ToString() + ");";
                }
                //是否显示服务单邮件通知
                if (CommonDP.GetConfigValue("Other", "IsEmailService") != null && CommonDP.GetConfigValue("Other", "IsEmailService") == "0" && oFlow.FlowID != 0)
                {
                    myFlowForms.CtrButtons1.Button2Visible = true;
                    myFlowForms.CtrButtons1.ButtonName2 = "邮件通知";
                    myFlowForms.CtrButtons1.Button2Function = "SendMail(2);";
                }
            }

            myFlowForms.FormTitle = oFlow.FlowName;
            this.FlowID = myFlowForms.oFlow.FlowID.ToString();
            this.MessageID = myFlowForms.oFlow.MessageID.ToString();
            this.AppID = myFlowForms.oFlow.AppID.ToString();
            this.FlowModelID = myFlowForms.oFlow.FlowModelID.ToString();
            this.hidAppID.Value = this.AppID;

            this.hidFlowID.Value = this.FlowID;

            DataTable dt = null;
            if (oFlow.MessageID != 0)
            {
                ImplDataProcess dp = new ImplDataProcess(oFlow.AppID);
                DataSet ds = dp.GetFieldsDataSet(oFlow.FlowID, oFlow.OpID);
                dt = ds.Tables[0];
                if (dt != null)
                {
                    #region 重复事件单
                    //重复事件
                    DataTable dtIssueSub = ZHServiceDP.GetIssuesSub(myFlowForms.oFlow.FlowID);
                    dgIssueSub.DataSource = dtIssueSub.DefaultView;
                    dgIssueSub.DataBind();
                    if (dtIssueSub.Rows.Count > 0)
                    {
                        IssuesSub = "true";
                    }
                    //重复事件于
                    DataTable dtIssuesSubRel = ZHServiceDP.GetIssuesSubRel(myFlowForms.oFlow.FlowID);
                    dgIssueSubRel.DataSource = dtIssuesSubRel.DefaultView;
                    dgIssueSubRel.DataBind();
                    if (dtIssuesSubRel.Rows.Count > 0)
                    {
                        IssuesSubRel = "true";
                    }
                    #endregion
                }


            }
            else
            {
                if (Session["ExtendParameter"] != null && Session["ExtendParameter"].ToString() != string.Empty && Session["ExtendParameter"].ToString() != "0")
                {
                    string extendParameter = Session["ExtendParameter"].ToString();
                    if (extendParameter.IndexOf("t") != -1)
                    {
                        //判断是否由语音系统过来的
                        extendParameter = extendParameter.Replace("t", "");
                        Br_ECustomerDP ec = new Br_ECustomerDP();
                        string sWhere = " AND (Mobile=" + StringTool.SqlQ(extendParameter) + " OR TEL1=" + StringTool.SqlQ(extendParameter) + ") ";
                        DataTable dt1 = ec.GetDataTable(sWhere, "");
                        foreach (DataRow row in dt1.Rows)
                        {
                            hidCustID.Value = row["ID"].ToString();
                            hidCustDeptName.Value = row["CustDeptName"].ToString();
                            hidCust.Value = row["ShortName"].ToString();
                            txtCustAddr.Text = row["ShortName"].ToString();
                            lblCustDeptName.Text = row["CustDeptName"].ToString();
                            lblEmail.Text = row["Email"].ToString();
                            hidCustEmail.Value = row["Email"].ToString();
                            lblMastCust.Text = row["ShortName"].ToString();
                            lbljob.Text = row["job"].ToString();
                            hidjob.Value = row["job"].ToString();

                            txtAddr.Text = row["Address"].ToString();
                            hidAddr.Value = row["Address"].ToString();
                            lblAddr.Text = row["Address"].ToString();
                            txtContact.Text = row["LinkMan1"].ToString();
                            labContact.Text = StringTool.ParseForHtml(SpecTransText(txtContact.Text));
                            txtCTel.Text = row["Tel1"].ToString();
                            labCTel.Text = row["Tel1"].ToString();
                            lblMastCust.Text = row["MastCustName"].ToString();

                            //取得资产资料
                            Equ_DeskDP equ = new Equ_DeskDP();
                            equ = equ.GetEquByCustID(long.Parse(row["ID"].ToString() == "" ? "0" : row["ID"].ToString()));
                            txtEqu.Text = equ.Name;
                            lblEqu.Text = equ.Name;
                            hidEqu.Value = equ.ID.ToString();
                            hidEquName.Value = equ.Name;
                            //资产分类
                            txtListName.Text = equ.ListName.ToString();
                            lblListName.Text = equ.ListName.ToString();
                            hidListName.Value = equ.ListName.ToString();
                            hidListID.Value = equ.ListID.ToString();
                        }

                        if (dt1 == null || dt1.Rows.Count <= 0)
                        {
                            txtCTel.Text = extendParameter;
                            labCTel.Text = extendParameter;
                        }

                    } //合并重复事件
                    else if (extendParameter.StartsWith("issuemerge"))
                    {
                        Br_ECustomerDP ec = new Br_ECustomerDP();
                        string sWhere = " AND (Mobile=" + StringTool.SqlQ(extendParameter) + " OR TEL1=" + StringTool.SqlQ(extendParameter) + ") ";
                        DataTable dt1 = ec.GetDataTable(sWhere, "");
                        foreach (DataRow row in dt1.Rows)
                        {
                            hidCustID.Value = row["ID"].ToString();
                            hidCustDeptName.Value = row["CustDeptName"].ToString();
                            hidCust.Value = row["ShortName"].ToString();
                            txtCustAddr.Text = row["ShortName"].ToString();
                            lblCustDeptName.Text = row["CustDeptName"].ToString();
                            lblEmail.Text = row["Email"].ToString();
                            hidCustEmail.Value = row["Email"].ToString();
                            lblMastCust.Text = row["ShortName"].ToString();
                            lbljob.Text = row["job"].ToString();
                            hidjob.Value = row["job"].ToString();

                            txtAddr.Text = row["Address"].ToString();
                            hidAddr.Value = row["Address"].ToString();
                            lblAddr.Text = row["Address"].ToString();
                            txtContact.Text = row["LinkMan1"].ToString();
                            labContact.Text = StringTool.ParseForHtml(SpecTransText(txtContact.Text));
                            txtCTel.Text = row["Tel1"].ToString();
                            labCTel.Text = row["Tel1"].ToString();
                            lblMastCust.Text = row["MastCustName"].ToString();

                        }
                        string strFlowIDList = string.Empty;
                        if (extendParameter.Length > 12)
                        {
                            strFlowIDList = extendParameter.Substring("issuemerge".Length + 1, extendParameter.Length - "issuemerge".Length - 1);
                            DataTable dtIssueSub = ZHServiceDP.GetIssuesSubAdd(strFlowIDList);
                            dgIssueSub.DataSource = dtIssueSub.DefaultView;
                            dgIssueSub.DataBind();
                            if (dtIssueSub.Rows.Count > 0)
                            {
                                IssuesSub = "true";
                            }
                            string strSubject = string.Empty;
                            foreach (DataRow dr in dtIssueSub.Rows)
                            {
                                strSubject += dr["Content"] + ";";
                            }
                            txtContent.Value = strSubject;

                        }
                    }
                    else
                    {
                        //通过模版获取初值
                        long lngTemplateID = long.Parse(Session["ExtendParameter"].ToString());

                        SetFromFlowValues(lngTemplateID);

                    }
                }
                else
                {

                    CtrDTRegTime.dateTime = DateTime.Now;

                    if (CommonDP.GetConfigValue("Other", "ItsmCustomerMode") == "0")
                    {
                        //如果没有从客户信息中返回，则内部模式设置缺省的 客户资料；否则就用传回来的客户ID

                        txtCustAddr.Text = Session["UserDefaultCustomerName"].ToString();
                        hidCustID.Value = Session["UserDefaultCustomerID"].ToString();

                        if (hidCustID.Value != "0" && hidCustID.Value != "")
                        {
                            //取得客户资料
                            Br_ECustomerDP ec = new Br_ECustomerDP();
                            ec = ec.GetReCorded(long.Parse(hidCustID.Value));
                            hidCustID.Value = ec.ID.ToString();
                            hidCustDeptName.Value = ec.CustDeptName;
                            hidCust.Value = ec.ShortName;
                            txtCustAddr.Text = ec.ShortName;
                            lblCustDeptName.Text = ec.CustDeptName;
                            lblEmail.Text = ec.Email;
                            hidCustEmail.Value = ec.Email;
                            lblMastCust.Text = ec.ShortName;
                            lbljob.Text = ec.Job;
                            hidjob.Value = ec.Job;

                            txtAddr.Text = ec.Address;
                            hidAddr.Value = ec.Address;
                            lblAddr.Text = ec.Address;
                            txtContact.Text = ec.LinkMan1;
                            labContact.Text = StringTool.ParseForHtml(SpecTransText(txtContact.Text));
                            txtCTel.Text = ec.Tel1;
                            labCTel.Text = ec.Tel1;
                            lblMastCust.Text = ec.MastCustName;

                            //取得资产资料
                            Equ_DeskDP ee = new Equ_DeskDP();
                            ee = ee.GetEquByCustID(long.Parse(ec.ID.ToString()));
                            txtEqu.Text = ee.Name;
                            lblEqu.Text = ee.Name;
                            hidEqu.Value = ee.ID.ToString();
                            hidEquName.Value = ee.Name;
                            //资产分类
                            txtListName.Text = ee.ListName.ToString();
                            lblListName.Text = ee.ListName.ToString();
                            hidListName.Value = ee.ListName.ToString();
                            hidListID.Value = ee.ListID.ToString();
                        }
                    }


                }

                if (RegUser.UserID == null || RegUser.UserID == 0)
                {
                    RegUser.UserID = long.Parse(Session["UserID"].ToString());
                    RegUser.UserName = Session["PersonName"].ToString();
                }
                hidServiceID.Value = "0";




            }

            //邮件报障
            setEmailFromValue();
            //此范例说明如何实现子流程的处理过程            
            Sjwxr.FlowID = "0";
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    hidServiceID.Value = row["smsid"].ToString();
                    labServiceNo.Text = row["ServiceNo"].ToString();
                    CtrDTRegTime.dateTime = (DateTime)row["RegSysDate"];
                    CtrFlowFTSubject.Value = row["Subject"].ToString();
                    txtContent.Value = row["Content"].ToString();

                    RegUser.UserName = row["RegUserName"].ToString();
                    if (row["RegUserID"].ToString().Length > 0)
                    {
                        RegUser.UserID = long.Parse(row["RegUserID"].ToString());
                    }
                    ctrFCDServiceType.CatelogID = long.Parse(row["servicetypeid"].ToString());
                    hidServiceTypeID.Value = long.Parse(row["servicetypeid"].ToString()).ToString();
                    ctrFCDWTType.CatelogID = long.Parse(row["ServiceKindid"].ToString());

                    txtServiceLevel.Text = row["ServiceLevel"].ToString().Trim();
                    labServiceLevel.Text = txtServiceLevel.Text;
                    hidServiceLevel.Value = txtServiceLevel.Text;
                    hidServiceLevelID.Value = row["ServiceLevelid"].ToString().Trim();
                    Sjwxr.FlowID = row["FlowID"].ToString();


                    CtrFCDEffect.CatelogID = long.Parse(row["EffectID"].ToString());  //影响度
                    CtrFCDInstancy.CatelogID = long.Parse(row["InstancyID"].ToString()); //紧急度

                    //事件状态
                    if (row["dealstatusid"].ToString() != string.Empty)
                    {
                        hidDealStatusID.Value = row["dealstatusid"].ToString();
                        hidDealStatus.Value = row["dealstatus"].ToString();
                        lblDealStatus.Text = row["dealstatus"].ToString();
                    }


                    if (row["custtime"].ToString() != "")
                    {
                        CtrDTCustTime.dateTime = (DateTime)row["custtime"]; //发生时间
                    }

                    if (row["ReportingTime"].ToString() != "")
                    {
                        CtrReportingTime.dateTime = (DateTime)row["ReportingTime"]; //报告时间
                    }

                    //关闭状态理由
                    CtrCloseReason.CatelogID = long.Parse(row["CloseReasonID"].ToString() == "" ? "0" : row["CloseReasonID"].ToString());
                    //事件来源
                    CtrReSouse.CatelogID = long.Parse(row["ReSouseID"].ToString() == "" ? "0" : row["ReSouseID"].ToString());


                    hidCustID.Value = row["CustID"].ToString();
                    txtCustAddr.Text = row["CustName"].ToString();
                    labCustAddr.Text = StringTool.ParseForHtml(SpecTransText(txtCustAddr.Text));
                    txtAddr.Text = row["CustAddress"].ToString();
                    hidAddr.Value = row["CustAddress"].ToString();
                    lblAddr.Text = row["CustAddress"].ToString();
                    txtContact.Text = row["Contact"].ToString();
                    labContact.Text = StringTool.ParseForHtml(SpecTransText(txtContact.Text));
                    txtCTel.Text = row["ctel"].ToString();
                    labCTel.Text = StringTool.ParseForHtml(SpecTransText(txtCTel.Text));

                    hidMastCust.Value = row["MastCust"].ToString();
                    hidCustDeptName.Value = row["CustDeptName"].ToString();
                    lblCustDeptName.Text = StringTool.ParseForHtml(SpecTransText(hidCustDeptName.Value));
                    hidCustEmail.Value = row["Email"].ToString();
                    lblEmail.Text = StringTool.ParseForHtml(SpecTransText(hidCustEmail.Value));
                    lblMastCust.Text = StringTool.ParseForHtml(SpecTransText(row["MastCust"].ToString()));
                    lbljob.Text = StringTool.ParseForHtml(SpecTransText(row["job"].ToString()));
                    hidjob.Value = row["job"].ToString();

                    #region 资产信息
                    lblEqu.Text = row["EquipmentName"].ToString();     //设备
                    txtEqu.Text = row["EquipmentName"].ToString();
                    hidEqu.Value = row["Equipmentid"].ToString();
                    hidEquName.Value = row["EquipmentName"].ToString();

                    lblListName.Text = row["EquipmentCatalogName"].ToString(); //资产分类
                    txtListName.Text = row["EquipmentCatalogName"].ToString();
                    hidListName.Value = row["EquipmentCatalogName"].ToString();
                    hidListID.Value = row["EquipmentCatalogID"].ToString() == "" ? "0" : row["EquipmentCatalogID"].ToString();

                    #endregion

                    freeTextBox1.Text = row["dealcontent"].ToString();
                    labDealContent.Text = freeTextBox1.Text.Trim();
                    //派出时间
                    if (row["outtime"].ToString() != "")
                    {
                        CtrDTOutTime.dateTime = (DateTime)row["outtime"];
                        CtrDTOutTime.AllowNull = false;
                    }
                    //上门时间
                    if (row["servicetime"].ToString() != "")
                    {
                        CtrDTServiceTime.dateTime = (DateTime)row["servicetime"];
                        CtrDTServiceTime.AllowNull = false;
                    }
                    //处理完成时间
                    if (row["finishedtime"].ToString() != "")
                    {
                        CtrDTFinishedTime.dateTime = (DateTime)row["finishedtime"];
                        CtrDTFinishedTime.AllowNull = false;
                    }
                    Sjwxr.UserName = row["sjwxr"].ToString();
                    if (row["sjwxrID"].ToString() != string.Empty)
                    {
                        Sjwxr.UserID = row["sjwxrID"].ToString();
                    }
                    labTotalAmount.Text = row["totalamount"].ToString().Length > 0 ? double.Parse(row["totalamount"].ToString()).ToString("N") : "0";
                    labTotalHours.Text = StringTool.ParseForHtml(SpecTransText(row["totalhours"].ToString()));
                    lblBuildCode.Text = row["BuildCode"].ToString();

                    if (row["EmailState"].ToString() == string.Empty || row["EmailState"].ToString() == "0")
                    {
                        lblEmailBack.Text = "未邮件回访";
                    }
                    else if (row["EmailState"].ToString() == "1")
                    {
                        lblEmailBack.Text = "已邮件回访通知";
                    }
                    else if (row["EmailState"].ToString() == "2")
                    {
                        lblEmailBack.Text = "已邮件回访";
                    }

                    ProblemFlowID = row["ChangeProblemFlowID"].ToString();
                    ChangeFlowID = row["AssociateFlowID"].ToString();


                    #region 判断使用存在事件模板ID 以及是否已经使用
                    long IssTempID = long.Parse(row["IssTempID"].ToString() == "" ? "0" : row["IssTempID"].ToString());
                    hidIssTempID.Value = IssTempID.ToString();
                    if (IssTempID > 0)
                    {
                        SetRootCatalogId(IssTempID);//设备类别RootID
                        if (int.Parse(row["IsUseIssTempID"].ToString() == "" ? "0" : row["IsUseIssTempID"].ToString()) <= 0)
                        {
                            EA_ShortCutTemplateDP ect = new EA_ShortCutTemplateDP();
                            ect = ect.GetReCorded(long.Parse(IssTempID.ToString()));
                            FieldValues fv = new FieldValues(ect.TemplateXml);

                            #region 资产信息

                            if (fv.GetFieldValue("EquListName") != null && fv.GetFieldValue("EquListID") != null)
                            {
                                lblListName.Text = fv.GetFieldValue("EquListName").Value; //资产分类
                                txtListName.Text = fv.GetFieldValue("EquListName").Value;
                                hidListName.Value = fv.GetFieldValue("EquListName").Value;
                                hidListID.Value = fv.GetFieldValue("EquListID").Value;
                            }

                            #endregion

                            if (fv.GetFieldValue("InstancyID") != null && fv.GetFieldValue("InstancyID").Value != "0" && fv.GetFieldValue("InstancyID").Value != "")
                            {
                                CtrFCDInstancy.CatelogID = long.Parse(fv.GetFieldValue("InstancyID").Value == "" ? "0" : fv.GetFieldValue("InstancyID").Value); //紧急度
                            }
                            if (fv.GetFieldValue("EffectID") != null && fv.GetFieldValue("EffectID").Value != "0" && fv.GetFieldValue("EffectID").Value != "")
                            {
                                CtrFCDEffect.CatelogID = long.Parse(fv.GetFieldValue("EffectID").Value);  //影响度    
                            }
                            if (fv.GetFieldValue("ReasonID") != null && fv.GetFieldValue("ReasonID").Value != "0" && fv.GetFieldValue("ReasonID").Value != "")
                            {
                                //关闭状态理由
                                CtrCloseReason.CatelogID = long.Parse(fv.GetFieldValue("ReasonID").Value == "" ? "0" : fv.GetFieldValue("ReasonID").Value);
                            }
                            if (fv.GetFieldValue("FromID") != null && fv.GetFieldValue("FromID").Value != "0" && fv.GetFieldValue("FromID").Value != "")
                            {
                                //事件来源
                                CtrReSouse.CatelogID = long.Parse(fv.GetFieldValue("FromID").Value == "" ? "0" : fv.GetFieldValue("FromID").Value);
                            }
                            if (fv.GetFieldValue("ServiceTypeID") != null && fv.GetFieldValue("ServiceTypeID").Value != "0" && fv.GetFieldValue("ServiceTypeID").Value != "")
                            {
                                //事件类别
                                ctrFCDServiceType.CatelogID = long.Parse(fv.GetFieldValue("ServiceTypeID").Value);
                            }

                            if (fv.GetFieldValue("ServiceLevelID") != null && fv.GetFieldValue("ServiceLevel") != null)
                            {
                                hidServiceLevelID.Value = fv.GetFieldValue("ServiceLevelID").Value;
                                hidServiceLevel.Value = fv.GetFieldValue("ServiceLevel").Value;
                                txtServiceLevel.Text = hidServiceLevel.Value;
                                labServiceLevel.Text = txtServiceLevel.Text;
                            }
                        }
                    }
                    #endregion


                    #region 设置事件级别
                    if (hidServiceLevelID.Value != "" && hidServiceLevelID.Value != "0")
                    {
                        trServiceLevelDetail.Attributes.Add("style", "display:");

                        Cst_SLGuidDP ee = new Cst_SLGuidDP();
                        DataTable dtTmp = ee.GetDataByLevelIDCache(long.Parse(hidServiceLevelID.Value));
                        string sLimit = "";
                        foreach (DataRow r in dtTmp.Rows)
                        {
                            bool blnWX = false;  //响应时间超时
                            bool blnWF = false;  //完成时间超时
                            if (r["guidid"].ToString() == "10002")
                            {

                                sLimit += r["guidname"].ToString().Trim() + ":" + r["TimeLimit"].ToString().Trim() + GetTimeUnit(r["TimeUnit"].ToString().Trim()) + ",";

                                #region 增加显示 超过/还剩多少时间
                                DataTable dtLimit = ZHServiceDP.GetFlowBusLimitDetailByFlowID(decimal.Parse(row["FlowID"].ToString()), decimal.Parse(r["guidid"].ToString()));
                                DateTime outtime = row["outtime"].ToString() == "" ? DateTime.Now : DateTime.Parse(row["outtime"].ToString());
                                DateTime FinishedTime = row["finishedtime"].ToString() == "" ? DateTime.Now : DateTime.Parse(row["finishedtime"].ToString());

                                if (dtLimit != null && dtLimit.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dtLimit.Rows.Count; i++)
                                    {
                                        DateTime LimitTime = dtLimit.Rows[i]["LimitTime"].ToString() == "" ? DateTime.Now : DateTime.Parse(dtLimit.Rows[i]["LimitTime"].ToString());
                                        TimeSpan ts1 = LimitTime.Subtract(outtime);
                                        //TimeSpan ts2 = LimitTime.Subtract(outtime);

                                        string str1 = "";
                                        if (ts1.Days != 0)
                                            str1 += (ts1.Days > 0 ? ts1.Days : -ts1.Days) + "天";
                                        if (ts1.Hours != 0)
                                            str1 += (ts1.Hours > 0 ? ts1.Hours : -ts1.Hours) + "小时";
                                        if (ts1.Minutes != 0)
                                            str1 += (ts1.Minutes > 0 ? ts1.Minutes : -ts1.Minutes) + "分钟";

                                        if (ts1.Days < 0 || ts1.Hours < 0 || ts1.Minutes < 0)
                                        {
                                            sLimit += "<font color=red>";
                                            sLimit += "    响应时效 超" + str1;
                                            sLimit += "</font>";
                                        }
                                        else
                                            sLimit += "    响应时效 还剩" + str1;

                                    }
                                }
                                #endregion

                                sLimit = sLimit.Trim(',');

                                sLimit += "</br>";

                            }
                            if (r["guidid"].ToString() == "10001")
                            {
                                sLimit += r["guidname"].ToString().Trim() + ":" + r["TimeLimit"].ToString().Trim() + GetTimeUnit(r["TimeUnit"].ToString().Trim()) + ",";

                                #region 增加显示 超过/还剩多少时间
                                DataTable dtLimit = ZHServiceDP.GetFlowBusLimitDetailByFlowID(decimal.Parse(row["FlowID"].ToString()), decimal.Parse(r["guidid"].ToString()));
                                DateTime FinishedTime = row["finishedtime"].ToString() == "" ? DateTime.Now : DateTime.Parse(row["finishedtime"].ToString());

                                if (dtLimit != null && dtLimit.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dtLimit.Rows.Count; i++)
                                    {
                                        DateTime LimitTime = dtLimit.Rows[i]["LimitTime"].ToString() == "" ? DateTime.Now : DateTime.Parse(dtLimit.Rows[i]["LimitTime"].ToString());
                                        TimeSpan ts1 = LimitTime.Subtract(FinishedTime);
                                        if (dtLimit.Rows[i]["GuidID"].ToString() == "10001")
                                        {
                                            string str1 = "";
                                            if (ts1.Days != 0)
                                                str1 += (ts1.Days > 0 ? ts1.Days : -ts1.Days) + "天";
                                            if (ts1.Hours != 0)
                                                str1 += (ts1.Hours > 0 ? ts1.Hours : -ts1.Hours) + "小时";
                                            if (ts1.Minutes != 0)
                                                str1 += (ts1.Minutes > 0 ? ts1.Minutes : -ts1.Minutes) + "分钟";

                                            if (ts1.Days < 0 || ts1.Hours < 0 || ts1.Minutes < 0)
                                            {
                                                sLimit += "<font color=red>";
                                                sLimit += " 超" + str1;
                                                sLimit += "</font>";
                                            }
                                            else
                                                sLimit += " 还剩" + str1;
                                        }
                                    }
                                }
                                #endregion

                                sLimit = sLimit.Trim(',');

                                sLimit += "</br>";
                            }
                        }

                        //if (sLimit.EndsWith(","))
                        //    sLimit = sLimit.Substring(0, sLimit.Length - 1);

                        divSLTimeLimt.InnerHtml = "<table class='listContent' width='100%'> <tr><td class='listTitle' width='15%'>时限要求</td><td class='list'>" + sLimit + "</td></tr></table>";

                        Cst_ServiceLevelDP sld = new Cst_ServiceLevelDP();
                        sld = sld.GetReCorded(long.Parse(hidServiceLevelID.Value));
                        divSLDefinition.InnerHtml = "<table class='listContent' width='100%'> <tr><td class='listTitle' width='15%'>级别定义</td><td class='list'>" + sld.Definition + "</td></tr></table>";
                        lnkServiceLevel.Text = "详情";
                    }
                    #endregion
                }
            }
            else
            {
                CtrDTCustTime.dateTime = DateTime.Now;//发生时间
                CtrReportingTime.dateTime = DateTime.Now;//报告时间      

                CtrDTOutTime.dateTimeString = "";
                CtrDTServiceTime.dateTimeString = "";
            }



            BindGrid(long.Parse(hidServiceID.Value));   //费用明细
            #endregion

            #region 设置页面展示
            int iShowBase = 0;     //是否显示事件基本资料
            int iShowDeal = 0;   //是否显示处理信息
            setFieldCollection setFields = oFlow.setFields;
            foreach (setField sf in setFields)
            {
                if (sf.Editable == true && sf.Visibled == true)
                {
                    continue;
                }
                switch (sf.Name.ToLower())
                {
                    //页面标志（是服务请求还是生产故障）
                    case "flag":
                        if (sf.Visibled == false)
                        {
                            trCustTime.Visible = false;
                            trServiceKind.Visible = false;
                            trInstancyName.Visible = false;
                            iShowBase += 1;
                        }
                        break;
                    //基本信息
                    case "reguserid":
                        RegUser.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            RegUser.ContralState = eOA_FlowControlState.eHidden;
                            iShowBase += 1;
                        }
                        break;
                    case "servicetypeid":
                        ctrFCDServiceType.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            ctrFCDServiceType.ContralState = eOA_FlowControlState.eHidden;
                            iShowBase += 1;
                        }
                        break;
                    case "servicekindid":
                        ctrFCDWTType.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            ctrFCDWTType.ContralState = eOA_FlowControlState.eHidden;
                            iShowBase += 1;
                        }
                        break;
                    case "servicelevel":
                        //CtrServicLevel.ContralState = eOA_FlowControlState.eReadOnly;
                        cmdPopServiceLevel.Visible = false;
                        txtServiceLevel.Visible = false;
                        lblWaringServiceLevel.Visible = false;
                        labServiceLevel.Visible = true;
                        if (sf.Visibled == false)
                        {
                            //CtrServicLevel.ContralState = eOA_FlowControlState.eHidden;
                            trShowServiceLevel.Visible = false;
                            iShowBase += 1;
                        }
                        break;
                    case "effectid":
                        CtrFCDEffect.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrFCDEffect.ContralState = eOA_FlowControlState.eHidden;
                            iShowBase += 1;
                        }
                        break;
                    case "instancyid":
                        CtrFCDInstancy.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrFCDInstancy.ContralState = eOA_FlowControlState.eHidden;
                            iShowBase += 1;
                        }
                        break;
                    case "subject":
                        CtrFlowFTSubject.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrFlowFTSubject.ContralState = eOA_FlowControlState.eHidden;
                            iShowBase += 1;
                        }
                        break;
                    case "content":
                        txtContent.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            txtContent.ContralState = eOA_FlowControlState.eHidden;
                            iShowBase += 1;
                        }

                        break;
                    case "custtime":
                        CtrDTCustTime.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrDTCustTime.ContralState = eOA_FlowControlState.eHidden;
                            iShowBase += 1;
                        }
                        break;
                    case "reportingtime":  //报告时间
                        CtrReportingTime.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrReportingTime.ContralState = eOA_FlowControlState.eHidden;
                            iShowBase += 1;
                        }
                        break;
                    case "dealstatusid":
                        //CtrDealState.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            //CtrDealState.ContralState = eOA_FlowControlState.eHidden;
                            lblDealStatus.Visible = false;
                            iShowBase += 1;                       //事件状态
                        }
                        break;
                    case "closereason":                         //关闭状态理由
                        CtrCloseReason.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrCloseReason.ContralState = eOA_FlowControlState.eHidden;
                            iShowBase += 1;
                        }
                        break;
                    case "resouse":                             //事件来源
                        CtrReSouse.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrReSouse.ContralState = eOA_FlowControlState.eHidden;
                            iShowBase += 1;
                        }
                        break;
                    //客户信息
                    case "custinfo":
                        txtCustAddr.Visible = false;
                        rWarning.Visible = false;
                        cmdCust.Visible = false;
                        if (sf.Visibled == true)
                        {
                            labCustAddr.Visible = true;
                            if (labCustAddr.Text.Trim() == "")
                            {
                                lnkCustHistory.Visible = false;
                            }
                        }
                        else
                        {
                            lnkCustHistory.Visible = false;
                            lblMastCust.Visible = false;
                        }

                        txtAddr.Visible = false;
                        if (sf.Visibled == true)
                            lblAddr.Visible = true;

                        txtContact.Visible = false;
                        if (sf.Visibled == true)
                            labContact.Visible = true;

                        txtCTel.Visible = false;
                        if (sf.Visibled == true)
                            labCTel.Visible = true;


                        if (sf.Visibled == false)
                        {
                            Table12.Visible = false;
                            Table2.Visible = false;

                        }
                        break;

                    //资产名称
                    case "equinfo":
                        txtEqu.Visible = false;
                        cmdEqu.Visible = false;
                        trEqu.Visible = true;
                        if (sf.Visibled == true)
                        {
                            lblEqu.Visible = true;
                            if (lblEqu.Text.Trim() == "")
                            {
                                lnkEquHistory.Visible = false;
                            }
                        }
                        else
                        {
                            lnkEquHistory.Visible = false;
                        }
                        txtListName.Visible = false;
                        cmdListName.Visible = false;
                        if (sf.Visibled == true)
                        {
                            lblListName.Visible = true;
                        }

                        bthAddEqu.Visible = false;
                        if (sf.Visibled == false)
                        {
                            Table15.Visible = false;
                            Table3.Visible = false;
                        }
                        break;


                    //处理信息
                    case "dealcontent":
                        freeTextBox1.Visible = false;
                        if (sf.Visibled == true)
                            labDealContent.Visible = true;
                        else
                            iShowDeal += 1;
                        break;
                    case "outtime":
                        CtrDTOutTime.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrDTOutTime.ContralState = eOA_FlowControlState.eHidden;
                            iShowDeal += 1;
                        }
                        break;
                    case "servicetime":
                        CtrDTServiceTime.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrDTServiceTime.ContralState = eOA_FlowControlState.eHidden;
                            iShowDeal += 1;
                        }
                        break;
                    case "finishedtime":
                        CtrDTFinishedTime.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrDTFinishedTime.ContralState = eOA_FlowControlState.eHidden;
                            iShowDeal += 1;
                        }
                        break;
                    case "sjwxr":
                        Sjwxr.VisibleText = false;
                        if (sf.Visibled)
                        {
                            Sjwxr.VisibleLabel = true;
                        }
                        else
                        {
                            iShowDeal += 1;
                        }
                        break;
                    case "totalhours":
                        if (sf.Visibled == true)
                        {
                            labTotalHours.Visible = true;
                        }
                        else
                        {
                            labTotalHours.Visible = false;
                            iShowDeal += 1;
                        }
                        break;
                    case "totalamount":
                        if (sf.Visibled == true)
                        {
                            labTotalAmount.Visible = true;
                        }
                        else
                        {
                            labTotalAmount.Visible = false;
                            iShowDeal += 1;
                        }
                        break;
                    case "faredetail":
                        if (sf.Visibled == true)
                        {
                            SetFareDetailReadOnly();
                        }
                        else
                        {
                            gvBillItem.Visible = false;
                            iShowDeal += 1;   //8
                        }
                        break;
                    case "editcustequ":
                        AddCustTD.Visible = false;
                        //AddEquTD.Visible = false;
                        break;
                    case "showfeedback":
                        hidFeedBack.Value = "1";
                        CtrFeedBack2.ContralState = eOA_FlowControlState.eReadOnly;
                        if (!sf.Visibled)
                        {
                            this.ShowFeedBack1.Visible = false;
                            this.CtrFeedBack2.DealVisible = false;
                            CtrFeedBack2.ContralState = eOA_FlowControlState.eHidden;
                        }

                        iShowDeal += 1;
                        break;
                    case "expr_property":    // 扩展属性 - 2013-11-19 @孙绍棕
                        Extension_DayCtrList1.ReadOnly = true;
                        if (!sf.Visibled)
                        {
                            Extension_DayCtrList1.Visible = false;
                        }
                        break;
                    default:
                        break;

                }
            }
            #endregion

            #region 扩展项

            //EquDeploy = ZHServiceDP.getEQU_deployList(long.Parse(this.hidFlowID.Value.ToString()));


            Extension_DayCtrList1.EquID = long.Parse(this.hidFlowID.Value.ToString());

            if (this.hidAppID.Value.ToString() != "0")
            {
                //Extension_DayCtrList1.EquCategoryID = long.Parse(this.hidAppID.Value.ToString());
                Extension_DayCtrList1.NodeModelID = this.FlowMaster.oFlow.NodeModelID;
                Extension_DayCtrList1.EquCategoryID = this.FlowMaster.oFlow.FlowModelID;

            }


            #endregion

            #region 其它
            //超时提示
            this.labExpectedTime.Text = StringTool.ParseForHtml(oFlow.FlowExpectEnd + GetDiffLimit(oFlow.FlowExpectEnd, oFlow.FlowStatus));

            if (oFlow.FlowExpectEnd.Length > 0)
            {
                if (DateTime.Parse(oFlow.FlowExpectEnd) <= DateTime.Now && oFlow.FlowStatus != e_FlowStatus.efsEnd)
                {
                    labExpectedTime.ForeColor = Color.Red;
                    labExpectedTime.Font.Bold = true;
                }
            }
            else
            {
                this.ShowFlowLimit.Visible = false;
            }

            #region 修改放开督办回访 余向前 2013-05-02
            //设置回访是否可以录入,根据权限 并且非 会签状态 结束情况方可
            //if (oFlow.FlowStatus == e_FlowStatus.efsEnd)
            //{
            //    this.ShowFeedBack.Visible = true;
            //    //是否需要邮件回访
            //    if (CommonDP.GetConfigValue("Other", "IsEmailFeedBack") != null && CommonDP.GetConfigValue("Other", "IsEmailFeedBack") == "0")
            //    {
            //        if (CheckRight(Constant.feedbackright) == false)
            //        {
            //            btnEmailBack.Visible = false;
            //        }
            //        else
            //        {
            //            btnEmailBack.Visible = true;
            //        }
            //    }
            //}
            //else
            //{
            //    this.ShowFeedBack.Visible = false;
            //}
            this.ShowFeedBack.Visible = false;
            #endregion

            #region 修改事件回访显示 余向前 2013-05-02
            if (CheckRight(Constant.feedbackright) == false || oFlow.ActorClass == e_ActorClass.fmInfluxActor)
            {
                this.CtrFeedBack1.DealVisible = false;
            }
            else
            {
                myFlowForms.FormTitle = "事件单[回访]";
            }
            #endregion

            // myFlowForms.SubProcessFlowList = "10059|10024,10061|10025";  实验
            //InitFeedBack(oFlow.FlowID);//获取回访信息
            //设置回访控件属性
            CtrFeedBack2.FlowID = oFlow.FlowID;
            CtrFeedBack2.AppID = oFlow.AppID;
            CtrFeedBack1.FlowID = oFlow.FlowID;
            CtrFeedBack1.AppID = oFlow.AppID;
            //CtrFeedBack1.FeedBackCustomer = labContact.Text.Trim();

            //设置督办是否可以录入,根据权限 并且非 会签状态 结束情况方可
            if (oFlow.MessageID == 0)
            {
                trShowMonitor.Visible = false;   //设置督办为不可见
            }
            if (CheckRight(Constant.dubanyijian) == false || oFlow.ActorClass == e_ActorClass.fmInfluxActor
                || oFlow.FlowStatus == e_FlowStatus.efsEnd || oFlow.FlowStatus == e_FlowStatus.efsStop || oFlow.FlowID == 0)
            {
                this.CtrMonitor1.DealVisible = false;
            }

            //设置督办属性
            CtrMonitor1.FlowID = oFlow.FlowID;
            CtrMonitor1.AppID = oFlow.AppID;

            #endregion

            #region 流程是否结束  余向前 2013-05-02
            if (myFlowForms.oFlow != null)
            {
                FlowIsClosed = myFlowForms.oFlow.FlowStatus == e_FlowStatus.efsEnd;
            }

            if (FlowIsClosed && IsEmailFeedBack)
            {
                hidFeedBack.Value = "0";
                this.ShowFeedBack1.Visible = true;
                this.CtrFeedBack2.Visible = true;
                this.CtrFeedBack2.DealVisible = true;
                this.CtrFeedBack2.ContralState = eOA_FlowControlState.eNormal;
            }
            else
            {
                this.CtrFeedBack2.DealVisible = false;
            }
            #endregion

        }
        #endregion

        #region 填空白值 SpecTransText
        /// <summary>
        /// 填空白值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string SpecTransText(string str)
        {
            string strR = str;
            if (str == "")
            {
                strR = "--";
            }
            if (str == "--")
            {
                strR = "";
            }
            return strR;
        }
        #endregion

        #region 取得页面值生成XML Master_myGetFormsValue
        /// <summary>
        /// 取得页面值生成XML
        /// </summary>
        /// <returns></returns>
        XmlDocument Master_myGetFormsValue(long lngActionID, string strActionName)
        {
            ///注意：当前应用没有附属表，当有附属表的情况，附属表的信息项值的XML存到 Tables节点里
            #region Master_myGetFormsValue

            FieldValues fv = new FieldValues();

            fv.Add("ServiceID", hidServiceID.Value.Trim());
            fv.Add("Subject", CtrFlowFTSubject.Value.Trim().Replace(System.Web.HttpUtility.HtmlDecode("&#160;"), "").Replace(System.Web.HttpUtility.HtmlDecode("&nbsp;"), ""));
            fv.Add("Content", txtContent.Value.Trim());
            fv.Add("RegUserID", RegUser.UserID.ToString().Trim());
            fv.Add("RegUserName", RegUser.UserName.Trim());
            fv.Add("ServiceLevelID", (hidServiceLevelID.Value.Trim() == "" ? "0" : hidServiceLevelID.Value.Trim()));
            fv.Add("ServiceLevel", hidServiceLevel.Value.Trim());
            fv.Add("ServiceLevelChange", hidServiceLevelChange.Value.Trim());


            fv.Add("ServiceTypeID", ctrFCDServiceType.CatelogID.ToString().Trim());
            fv.Add("ServiceType", ctrFCDServiceType.CatelogValue.Trim());
            fv.Add("ServiceKindID", ctrFCDWTType.CatelogID.ToString().Trim());
            fv.Add("ServiceKind", ctrFCDWTType.CatelogValue.Trim());

            fv.Add("EffectID", CtrFCDEffect.CatelogID.ToString().Trim());
            fv.Add("EffectName", CtrFCDEffect.CatelogValue.Trim());
            fv.Add("InstancyID", CtrFCDInstancy.CatelogID.ToString().Trim());
            fv.Add("InstancyName", CtrFCDInstancy.CatelogValue.Trim());
            fv.Add("DealStatusID", hidDealStatusID.Value == "" ? "0" : hidDealStatusID.Value);
            fv.Add("DealStatus", hidDealStatus.Value);

            fv.Add("CustTime", CtrDTCustTime.dateTimeString); //发生时间
            fv.Add("ReportingTime", CtrReportingTime.dateTimeString); //报告时间

            fv.Add("CustID", this.hidCustID.Value.Trim() == string.Empty ? "0" : this.hidCustID.Value.Trim());
            fv.Add("CustName", txtCustAddr.Text.Trim());
            fv.Add("CustAddress", txtAddr.Text.Trim());
            fv.Add("Contact", txtContact.Text.Trim());
            fv.Add("CTel", txtCTel.Text.Trim());
            fv.Add("CustDeptName", lblCustDeptName.Text.Trim());
            fv.Add("Job", lbljob.Text.Trim());
            fv.Add("Email", hidCustEmail.Value.Trim());
            fv.Add("MastCust", lblMastCust.Text.Trim());

            #region 资产信息

            fv.Add("EquipmentID", this.hidEqu.Value.Trim() == string.Empty ? "0" : this.hidEqu.Value.Trim());
            fv.Add("EquipmentName", this.txtEqu.Text.Trim());
            fv.Add("EquipmentCatalogName", hidListName.Value.Trim()); //资产目录
            fv.Add("EquipmentCatalogID", hidListID.Value.Trim() == string.Empty ? "0" : hidListID.Value.Trim());

            fv.Add("EquPositions", "");
            fv.Add("EquCode", "");
            fv.Add("EquSN", "");
            fv.Add("EquModel", "");
            fv.Add("EquBreed", "");

            #endregion

            fv.Add("DealContent", freeTextBox1.Text.Trim());
            fv.Add("Outtime", CtrDTOutTime.dateTimeString);
            fv.Add("ServiceTime", CtrDTServiceTime.dateTimeString);
            fv.Add("FinishedTime", CtrDTFinishedTime.dateTimeString);
            fv.Add("SjwxrID", Sjwxr.UserID.ToString().Trim());
            fv.Add("Sjwxr", Sjwxr.UserName.Trim());

            fv.Add("TotalAmount", labTotalAmount.Text.Trim() == string.Empty ? "0" : labTotalAmount.Text.Trim());
            fv.Add("OrgID", Session["UserOrgID"].ToString());

            fv.Add("RegSysDate", DateTime.Now.ToString());
            fv.Add("RegSysUserID", Session["UserID"].ToString());
            fv.Add("RegSysUser", Session["PersonName"].ToString());

            fv.Add("EmailNotify", myFlowForms.EmailValue.ToString());
            fv.Add("SMSNotify", myFlowForms.NotifyValue.ToString());

            //如果登单人不同，取相应的部门和部门名称
            if (Session["UserID"].ToString() == RegUser.UserID.ToString())
            {
                fv.Add("RegDeptID", Session["UserDeptID"].ToString());
                fv.Add("RegDeptName", Session["UserDeptName"].ToString());
            }
            else
            {
                Epower.DevBase.Organization.SqlDAL.UserEntity pUserEntity = new UserEntity(RegUser.UserID);
                fv.Add("RegDeptID", pUserEntity.DeptID.ToString());
                fv.Add("RegDeptName", pUserEntity.FullDeptName);
            }
            //取得服务单前缀，生成服务单号

            string sServiceNo = this.labServiceNo.Text.Trim();
            if (sServiceNo == string.Empty)
            {
                string sBuildCode = string.Empty;
                sServiceNo = RuleCodeDP.GetCodeBH2(10003, ref sBuildCode);
                fv.Add("serviceno", sServiceNo);
                fv.Add("buildCode", sBuildCode);
            }
            else
            {
                fv.Add("serviceno", sServiceNo);
                fv.Add("buildCode", lblBuildCode.Text.Trim());

            }


            #endregion

            fv.Add("Flag", "false");  //区分标志

            #region yxq 新增关闭理由和事件来源 2011-08-02
            fv.Add("CloseReasonID", CtrCloseReason.CatelogID.ToString());  //关闭理由ID
            fv.Add("CloseReasonName", CtrCloseReason.CatelogValue);        //关闭理由名称
            fv.Add("ReSouseID", CtrReSouse.CatelogID.ToString());          //事件来源ID
            fv.Add("ReSouseName", CtrReSouse.CatelogValue);                //事件来源名称
            #endregion

            fv.Add("pubRequestID", lngRequestID.ToString());   //2009-04-25 增加 公众请求ID


            //事件模板相关
            fv.Add("IssTempID", hidIssTempID.Value);
            fv.Add("IsUseIssTempID", "1");
            fv.Add("CustAreaID", "0");
            fv.Add("CustArea", "");
            fv.Add("ApplicationTime", "");
            fv.Add("ExpectedTime", "");
            fv.Add("Reason", "");



            #region  结束事件单号
            int iItemCount = 0;
            FieldValues fvitem = new FieldValues();
            foreach (DataGridItem item in dgIssueSub.Items)
            {
                if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
                {
                    CheckBox chkDel = (CheckBox)item.Cells[0].FindControl("chkDel");
                    string strChk = chkDel.Checked == true ? "1" : "0";
                    if (item.Cells[dgIssueSub.Columns.Count - 1].Text.Trim() == "2")
                    {
                        strChk = "2";
                    }
                    string sSubFlowID = item.Cells[dgIssueSub.Columns.Count - 3].Text;
                    fvitem.Add("strChk" + iItemCount, strChk);
                    fvitem.Add("sSubFlowID" + iItemCount, sSubFlowID);

                    iItemCount++;
                }
            }
            fv.Add("ItemCount", iItemCount.ToString());
            fv.Add("ItemXml", fvitem.GetXmlObject().InnerXml);

            string ExtensionDayList = SaveExtensionDayList();
            fv.Add("ExtensionDayList", ExtensionDayList);  //扩展项


            XmlDocument xmlDoc = fv.GetXmlObject();

            if (hidFeedBack.Value == "0")
            {
                E8Logger.Info("this.CtrFeedBack2.SaveFeedBack(null, null);");
                this.CtrFeedBack2.SaveFeedBack(null, null);
            }

            #endregion

            return fv.GetXmlObject();
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

        #region 设置页面为只读 Master_mySetContentReadOnly
        /// <summary>
        /// 设置页面为只读
        /// </summary>
        void Master_mySetContentReadOnly()
        {
            #region Master_mySetContentReadOnly
            if (CtrFlowFTSubject.ContralState != eOA_FlowControlState.eHidden)
                CtrFlowFTSubject.ContralState = eOA_FlowControlState.eReadOnly;

            if (RegUser.ContralState != eOA_FlowControlState.eHidden)
                RegUser.ContralState = eOA_FlowControlState.eReadOnly;

            if (ctrFCDServiceType.ContralState != eOA_FlowControlState.eHidden)
                ctrFCDServiceType.ContralState = eOA_FlowControlState.eReadOnly;

            if (ctrFCDWTType.ContralState != eOA_FlowControlState.eHidden)
                ctrFCDWTType.ContralState = eOA_FlowControlState.eReadOnly;

            cmdPopServiceLevel.Visible = false;
            txtServiceLevel.Visible = false;
            lblWaringServiceLevel.Visible = false;
            labServiceLevel.Visible = true;


            if (CtrFCDEffect.ContralState != eOA_FlowControlState.eHidden)
                CtrFCDEffect.ContralState = eOA_FlowControlState.eReadOnly;

            if (CtrFCDInstancy.ContralState != eOA_FlowControlState.eHidden)
                CtrFCDInstancy.ContralState = eOA_FlowControlState.eReadOnly;

            if (CtrDTOutTime.ContralState != eOA_FlowControlState.eHidden)
                CtrDTOutTime.ContralState = eOA_FlowControlState.eReadOnly;

            if (CtrReportingTime.ContralState != eOA_FlowControlState.eHidden)  //报告时间
                CtrReportingTime.ContralState = eOA_FlowControlState.eReadOnly;


            if (txtContent.ContralState != eOA_FlowControlState.eHidden)  //报告时间
                txtContent.ContralState = eOA_FlowControlState.eReadOnly;

            if (txtContact.Visible == true)
                labContact.Visible = true;
            txtContact.Visible = false;

            if (txtCTel.Visible == true)
                labCTel.Visible = true;
            txtCTel.Visible = false;

            if (txtCustAddr.Visible == true)
            {
                rWarning.Visible = false;
                labCustAddr.Visible = true;
                if (labCustAddr.Text.Trim() == "")
                {
                    lnkCustHistory.Visible = false;
                }
            }
            txtCustAddr.Visible = false;
            rWarning.Visible = false;
            cmdCust.Visible = false;

            if (txtAddr.Visible == true)
                lblAddr.Visible = true;
            txtAddr.Visible = false;

            //没有客户赋值时 不显示历史参考
            if (txtCustAddr.Text.Trim() == "" || txtCustAddr.Text.Trim() == "--")
                lnkCustHistory.Visible = false;

            if (txtEqu.Visible == true)      //设备
            {
                lblEqu.Visible = true;
                if (lblEqu.Text.Trim() == "")
                {
                    lnkEquHistory.Visible = false;
                }
            }
            txtEqu.Visible = false;
            cmdEqu.Visible = false;

            if (txtListName.Visible == true)
            {
                lblListName.Visible = true;
            }
            txtListName.Visible = false;
            cmdListName.Visible = false;

            //没有设备赋值时 不显示历史参考
            if (txtEqu.Text.Trim() == "" || txtEqu.Text.Trim() == "--")
                lnkEquHistory.Visible = false;

            if (freeTextBox1.Visible == true)
                labDealContent.Visible = true;
            freeTextBox1.Visible = false;

            if (CtrDTRegTime.ContralState != eOA_FlowControlState.eHidden)
                CtrDTRegTime.ContralState = eOA_FlowControlState.eReadOnly;
            if (CtrDTCustTime.ContralState != eOA_FlowControlState.eHidden)
                CtrDTCustTime.ContralState = eOA_FlowControlState.eReadOnly;
            if (CtrDTServiceTime.ContralState != eOA_FlowControlState.eHidden)
                CtrDTServiceTime.ContralState = eOA_FlowControlState.eReadOnly;
            if (CtrDTFinishedTime.ContralState != eOA_FlowControlState.eHidden)
                CtrDTFinishedTime.ContralState = eOA_FlowControlState.eReadOnly;

            if (Sjwxr.VisibleText)
                Sjwxr.VisibleLabel = true;
            Sjwxr.VisibleText = false;

            SetFareDetailReadOnly();                //设置明细为只读

            bthCreateCus.Visible = false;           //设置快速新增客户不可见
            bthAddEqu.Visible = false;              //设置快速新增资产不可见
            //if (hidFeedBackCount.Value == "1")
            //    this.ShowHFList.Visible = true;
            //this.ShowFeedBack1.Visible = false;
            //this.CtrFeedBack2.DealVisible = false;
            if (FlowIsClosed && IsEmailFeedBack)
            {
                this.ShowFeedBack1.Visible = true;
                this.CtrFeedBack2.Visible = true;
                this.CtrFeedBack2.DealVisible = true;
                this.CtrFeedBack2.ContralState = eOA_FlowControlState.eNormal;
            }
            else
            {
                this.CtrFeedBack2.ContralState = eOA_FlowControlState.eReadOnly;
            }

            #region 新增的 关闭状态理由和事件来源显示控制 yxq
            if (CtrCloseReason.ContralState != eOA_FlowControlState.eHidden)
                CtrCloseReason.ContralState = eOA_FlowControlState.eReadOnly;
            if (CtrReSouse.ContralState != eOA_FlowControlState.eHidden)
                CtrReSouse.ContralState = eOA_FlowControlState.eReadOnly;
            #endregion

            #endregion


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

        #region 获取时限描述信息 GetDiffLimit
        /// <summary>
        /// 获取时限描述信息
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        private string GetDiffLimit(string strDate, e_FlowStatus fs)
        {
            string ret = "";
            try
            {
                DateTime dt = System.DateTime.Parse(strDate);
                if (fs == e_FlowStatus.efsEnd)
                {
                    ret += " ";

                }
                else
                {

                    if (DateTime.Compare(dt, DateTime.Now) > 0)
                    {
                        TimeSpan span1 = dt - DateTime.Now;

                        ret += "    距离完成时限还有:" + ((span1.Days == 0 ? "" : (span1.Days.ToString() + "天"))) +
                            ((span1.Hours == 0 ? "" : (span1.Hours.ToString() + "小时"))) + span1.Minutes.ToString() + "分钟";
                    }
                    else
                    {
                        TimeSpan span2 = DateTime.Now - dt;

                        ret += "    已经超过完成时限:" + ((span2.Days == 0 ? "" : (span2.Days.ToString() + "天"))) +
                            ((span2.Hours == 0 ? "" : (span2.Hours.ToString() + "小时"))) + span2.Minutes.ToString() + "分钟";
                    }
                }
            }
            catch
            {
            }
            return ret;
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

        #region 费用明细操作
        #region 费用数据绑定 gvBillItem_ItemDataBound
        /// <summary>
        /// 数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvBillItem_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {

        }
        #endregion

        #region 保存费用数据 SaveDetailItem
        /// <summary>
        /// 保存费用数据
        /// </summary>
        /// <returns></returns>
        private bool SaveDetailItem()
        {
            long lngID = 0;
            try
            {
                DataTable dt = GetDetailItem(true);
                lngID = ZHServiceDP.SaveCLFareDetailItem(dt, long.Parse(hidServiceID.Value));
                if (lngID == 0)
                    return false;
                else
                    hidServiceID.Value = lngID.ToString(); //带回到处理类中
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        #endregion

        #region 取得费用总费用值 GetDetailTotalAmount
        /// <summary>
        /// 取得费用总费用值
        /// </summary>
        /// <returns></returns>
        private double GetDetailTotalAmount()
        {
            double dTotal = 0;

            foreach (DataGridItem row in gvBillItem.Items)
            {
                string sDTotalAmont = ((TextBox)row.FindControl("txtDTotalAmount")).Text;
                dTotal += sDTotalAmont == "" ? 0.0 : double.Parse(sDTotalAmont);
            }
            return dTotal;
        }
        #endregion

        #region 取得费用详细资料
        /// <summary>
        /// 取得费用详细资料 GetDetailItem
        /// </summary>
        /// <returns></returns>
        private DataTable GetDetailItem()
        {
            return GetDetailItem(false);
        }

        /// <summary>
        /// 取得费用详细资料
        /// </summary>
        /// <param name="isAll"></param>
        /// <returns></returns>
        private DataTable GetDetailItem(bool isAll)
        {
            DataTable dt = (DataTable)ViewState["ItemData"];
            dt.Rows.Clear();

            double dTotal = 0;
            int iCostID = 0;
            DataRow dr;

            foreach (DataGridItem row in gvBillItem.Controls[0].Controls)
            {
                iCostID++;
                if (row.ItemType == ListItemType.Footer)
                {
                    string sDFareName = ((TextBox)row.FindControl("txtAddDFareName")).Text; //((HtmlInputHidden)row.FindControl("hidAddDFareName")).Value;
                    string sDModelName = string.Empty; //((TextBox)row.FindControl("hidAddDModelName")).Text;//((HtmlInputHidden)row.FindControl("hidAddDModelName")).Value;
                    string sDFareCode = string.Empty; //((HtmlInputHidden)row.FindControl("hidAddDFareCode")).Value;
                    string sDQuantity = ((TextBox)row.FindControl("txtAddDQuantity")).Text;
                    string sDPrice = ((TextBox)row.FindControl("txtAddDPrice")).Text;//((HtmlInputHidden)row.FindControl("hidAddDPrice")).Value;
                    string sDFareAmount = ((TextBox)row.FindControl("txtAddDFareAmount")).Text;
                    string sDHumanAmount = ((TextBox)row.FindControl("txtAddDHumanAmount")).Text;
                    string sDRemark = ((TextBox)row.FindControl("txtAddDRemark")).Text;
                    string sDTotalAmont = ((TextBox)row.FindControl("txtAddDTotalAmount")).Text;

                    dr = dt.NewRow();
                    dr["CostID"] = iCostID;
                    dr["FareName"] = sDFareName;
                    dr["ModelName"] = sDModelName;
                    dr["FareCode"] = sDFareCode;
                    dr["Price"] = sDPrice == "" ? 0.0 : double.Parse(sDPrice);
                    dr["Quantity"] = sDQuantity == "" ? 0 : double.Parse(sDQuantity);
                    dr["FareAmount"] = (sDPrice == "" ? 0.0 : double.Parse(sDPrice)) * (sDQuantity == "" ? 0 : double.Parse(sDQuantity));
                    dr["HumanAmount"] = sDHumanAmount == "" ? 0.0 : double.Parse(sDHumanAmount);
                    dr["Remark"] = sDRemark;
                    dr["TotalAmount"] = (sDPrice == "" ? 0.0 : double.Parse(sDPrice)) * (sDQuantity == "" ? 0 : double.Parse(sDQuantity)) + (sDHumanAmount == "" ? 0.0 : double.Parse(sDHumanAmount));
                    if (((sDPrice == "" ? 0.0 : double.Parse(sDPrice)) * (sDQuantity == "" ? 0 : double.Parse(sDQuantity)) + (sDHumanAmount == "" ? 0.0 : double.Parse(sDHumanAmount))) != 0.0 && isAll == true)
                    {
                        //小计为0 的删除掉
                        dt.Rows.Add(dr);
                        // dt.AcceptChanges();
                        dTotal += (sDPrice == "" ? 0.0 : double.Parse(sDPrice)) * (sDQuantity == "" ? 0 : double.Parse(sDQuantity)) + (sDHumanAmount == "" ? 0.0 : double.Parse(sDHumanAmount));
                    }
                }
                else if (row.ItemType == ListItemType.Item || row.ItemType == ListItemType.AlternatingItem)
                {
                    string sDFareName = ((TextBox)row.FindControl("txtDFareName")).Text; //((HtmlInputHidden)row.FindControl("hidDFareName")).Value;
                    string sDModelName = string.Empty; //((HtmlInputHidden)row.FindControl("hidDModelName")).Value;
                    string sDFareCode = string.Empty; //((H/lInputHidden)row.FindControl("hidDFareCode")).Value;
                    string sDQuantity = ((TextBox)row.FindControl("txtDQuantity")).Text;
                    string sDPrice = ((TextBox)row.FindControl("txtDPrice")).Text;//((HtmlInputHidden)row.FindControl("hidDPrice")).Value;
                    string sDFareAmount = ((TextBox)row.FindControl("txtDFareAmount")).Text;
                    string sDHumanAmount = ((TextBox)row.FindControl("txtDHumanAmount")).Text;
                    string sDRemark = ((TextBox)row.FindControl("txtDRemark")).Text;
                    string sDTotalAmont = ((TextBox)row.FindControl("txtDTotalAmount")).Text;

                    dr = dt.NewRow();

                    if (sDFareName.Length > 0 || isAll == true)
                    {
                        dr["CostID"] = iCostID;
                        dr["FareName"] = sDFareName;
                        dr["ModelName"] = sDModelName;
                        dr["FareCode"] = sDFareCode;
                        dr["Price"] = sDPrice == "" ? 0.0 : double.Parse(sDPrice);
                        dr["Quantity"] = sDQuantity == "" ? 0 : double.Parse(sDQuantity);
                        dr["FareAmount"] = (sDPrice == "" ? 0.0 : double.Parse(sDPrice)) * (sDQuantity == "" ? 0 : double.Parse(sDQuantity));
                        dr["HumanAmount"] = sDHumanAmount == "" ? 0.0 : double.Parse(sDHumanAmount);
                        dr["Remark"] = sDRemark;
                        dr["TotalAmount"] = (sDPrice == "" ? 0.0 : double.Parse(sDPrice)) * (sDQuantity == "" ? 0 : double.Parse(sDQuantity)) + (sDHumanAmount == "" ? 0.0 : double.Parse(sDHumanAmount));
                        if (((sDPrice == "" ? 0.0 : double.Parse(sDPrice)) * (sDQuantity == "" ? 0 : double.Parse(sDQuantity)) + (sDHumanAmount == "" ? 0.0 : double.Parse(sDHumanAmount))) != 0.0)
                        {
                            //小计为0 的删除掉
                            dt.Rows.Add(dr);
                        }
                        dTotal += (sDPrice == "" ? 0.0 : double.Parse(sDPrice)) * (sDQuantity == "" ? 0 : double.Parse(sDQuantity)) + (sDHumanAmount == "" ? 0.0 : double.Parse(sDHumanAmount));
                    }
                }
            }
            ViewState["ItemData"] = dt;

            //注意这里同时　重新计算合计金额
            labTotalAmount.Text = dTotal.ToString();
            return dt;
        }
        #endregion

        #region 绑定费用明细资料 BindGrid
        /// <summary>
        /// 绑定费用明细资料
        /// </summary>
        /// <param name="id"></param>
        private void BindGrid(long id)
        {
            #region 获得明细资料

            DataTable dtItem = ZHServiceDP.GetCLFareItem(id);
            DataView dv = new DataView(dtItem);
            ViewState["ItemData"] = dtItem;
            gvBillItem.DataSource = dv;

            gvBillItem.DataBind();
            gvBillItem.Visible = true;

            #endregion
        }
        #endregion

        #region 设置费用明细资料为只读 SetFareDetailReadOnly
        /// <summary>
        /// 设置费用明细资料为只读
        /// </summary>
        private void SetFareDetailReadOnly()
        {
            foreach (DataGridItem row in gvBillItem.Items)
            {
                ((TextBox)row.FindControl("txtDFareName")).Visible = false;
                ((TextBox)row.FindControl("txtDModelName")).Visible = false;
                ((TextBox)row.FindControl("txtDQuantity")).Visible = false;
                ((TextBox)row.FindControl("txtDPrice")).Visible = false;
                ((TextBox)row.FindControl("txtDFareAmount")).Visible = false;
                ((TextBox)row.FindControl("txtDHumanAmount")).Visible = false;
                ((TextBox)row.FindControl("txtDTotalAmount")).Visible = false;
                ((TextBox)row.FindControl("txtDRemark")).Visible = false;
                ((HtmlInputButton)row.FindControl("cmdDFare")).Visible = false;

                ((Label)row.FindControl("lblDFareName")).Visible = true;
                ((Label)row.FindControl("lblDModelName")).Visible = true;
                ((Label)row.FindControl("lblDQuantity")).Visible = true;
                ((Label)row.FindControl("lblDPrice")).Visible = true;
                ((Label)row.FindControl("lblDFareAmount")).Visible = true;
                ((Label)row.FindControl("lblDHumanAmount")).Visible = true;
                ((Label)row.FindControl("lblDTotalAmount")).Visible = true;
                ((Label)row.FindControl("lblDRemark")).Visible = true;

                //gvBillItem.Columns[0].Visible = false;
                gvBillItem.Columns[8].Visible = false;
            }
            gvBillItem.ShowFooter = false;
        }
        #endregion

        #region 费用明细新增，删除事件 gvBillItem_ItemCommand
        /// <summary>
        /// 费用明细新增，删除事件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void gvBillItem_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            int iCostID = gvBillItem.Items.Count + 1;
            DataTable dt = GetDetailItem(false);
            if (e.CommandName == "Delete")
            {
                dt.Rows.RemoveAt(e.Item.ItemIndex);
            }
            else if (e.CommandName == "Add")
            {
                string sDFareName = ((TextBox)e.Item.FindControl("txtAddDFareName")).Text; //((HtmlInputHidden)row.FindControl("hidAddDFareName")).Value;
                string sDModelName = string.Empty; //((TextBox)row.FindControl("hidAddDModelName")).Text;//((HtmlInputHidden)row.FindControl("hidAddDModelName")).Value;
                string sDFareCode = string.Empty; //((HtmlInputHidden)row.FindControl("hidAddDFareCode")).Value;
                string sDQuantity = ((TextBox)e.Item.FindControl("txtAddDQuantity")).Text;
                string sDPrice = ((TextBox)e.Item.FindControl("txtAddDPrice")).Text;//((HtmlInputHidden)e.Item.FindControl("hidAddDPrice")).Value;
                string sDFareAmount = ((TextBox)e.Item.FindControl("txtAddDFareAmount")).Text;
                string sDHumanAmount = ((TextBox)e.Item.FindControl("txtAddDHumanAmount")).Text;
                string sDRemark = ((TextBox)e.Item.FindControl("txtAddDRemark")).Text;
                string sDTotalAmont = ((TextBox)e.Item.FindControl("txtAddDTotalAmount")).Text;

                DataRow dr = dt.NewRow();
                dr["CostID"] = iCostID;
                dr["FareName"] = sDFareName;
                dr["ModelName"] = sDModelName;
                dr["FareCode"] = sDFareCode;
                dr["Price"] = sDPrice == "" ? 0.0 : double.Parse(sDPrice);
                dr["Quantity"] = sDQuantity == "" ? 0 : double.Parse(sDQuantity);
                dr["FareAmount"] = (sDPrice == "" ? 0.0 : double.Parse(sDPrice)) * (sDQuantity == "" ? 0 : double.Parse(sDQuantity));
                dr["HumanAmount"] = sDHumanAmount == "" ? 0.0 : double.Parse(sDHumanAmount);
                dr["Remark"] = sDRemark;
                dr["TotalAmount"] = (sDPrice == "" ? 0.0 : double.Parse(sDPrice)) * (sDQuantity == "" ? 0 : double.Parse(sDQuantity)) + (sDHumanAmount == "" ? 0.0 : double.Parse(sDHumanAmount));
                if (((sDPrice == "" ? 0.0 : double.Parse(sDPrice)) * (sDQuantity == "" ? 0 : double.Parse(sDQuantity)) + (sDHumanAmount == "" ? 0.0 : double.Parse(sDHumanAmount))) != 0.0)
                {
                    //小计为0 的删除掉
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                }
                else
                {
                    PageTool.MsgBox(this, "没有保存的数据，请输入产生费用项的数据！");
                }
            }
            gvBillItem.DataSource = dt.DefaultView;
            gvBillItem.DataBind();
            //重新计算
            //注意这里同时　重新计算合计金额
            labTotalAmount.Text = GetDetailTotalAmount().ToString("N");
        }
        #endregion



        #endregion

        #region 显示页面地址
        /// <summary>
        /// 
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

        #region 回访情况展示 InitFeedBack
        /// <summary>
        /// 回访情况展示
        /// </summary>
        /// <param name="lngFlowID"></param>
        private void InitFeedBack(long lngFlowID)
        {
            DataTable dt = RiseDP.GetAllFeedBack(lngFlowID);
            int iCount = 0;
            long lngUserID = long.Parse(Session["UserID"].ToString());
            string sProcess = "";
            if (dt.Rows.Count == 0)
            {
                hidFeedBackCount.Value = "0";
                ShowHFList.Visible = false;//回访情况列表隐藏
                ltlHFList.Text = "";
            }
            else
            {
                hidFeedBackCount.Value = "1";
                if (ShowFeedBack1.Visible)
                    ShowHFList.Visible = false;//回访情况列表显示

                foreach (DataRow dr in dt.Rows)
                {
                    iCount++;
                    string sDate = dr["processsysdate"].ToString();
                    string sOpinion = dr["suggest"].ToString();
                    string sUserName = dr["dousername"].ToString();
                    string sMonID = dr["feedbackid"].ToString();
                    string sUser = dr["douser"].ToString();
                    string sFeedBack = dr["feedback"].ToString();
                    string sFeedType = dr["feedtype"].ToString();
                    string sFeedPerson = dr["feedPerson"].ToString();
                    string sCustName = dr["CustName"].ToString();
                    string sFBTime = dr["fbtime"].ToString();

                    switch (sFeedBack)
                    {
                        case "1":
                            sFeedBack = "满意";
                            break;
                        case "2":
                            sFeedBack = "基本满意";
                            break;
                        case "3":
                            sFeedBack = "不满意";
                            break;
                        default:
                            break;
                    }

                    switch (sFeedType)
                    {
                        case "1":
                            sFeedType = "电话";
                            break;
                        case "2":
                            sFeedType = "上门";
                            break;
                        case "3":
                            sFeedType = "其它";
                            break;
                        case "4":
                            sFeedType = "邮件";
                            break;
                        default:
                            break;
                    }
                    string sTR = "";
                    if (iCount == 1)
                    {
                        //添加标题情况

                        sTR += AddTD("回访时间&nbsp;", "noWrap") +
                            AddTD("登记人&nbsp;", "nowrap") +
                            AddTD("回访人&nbsp;", "nowrap") +
                            AddTD("被回访者&nbsp;", "nowrap") +
                            AddTD("回访方式&nbsp;", "nowrap") +
                             AddTD("满意程度&nbsp;", "nowrap") +
                            AddTD("回访内容", "width=50%");

                        sTR += AddTD("", "nowrap");


                        sTR = "<tr class='listTitle'>" + sTR + "</tr>";
                        sProcess += sTR;

                    }
                    //回访情况
                    sOpinion = sOpinion.Trim() == "" ? "" : "&nbsp;<font color=blue>" + StringTool.ParseForHtml(sOpinion) + "</font>";

                    sTR = "";
                    sTR += AddTD(sFBTime + "&nbsp;", "noWrap") +
                        AddTD(sUserName + "&nbsp;", "nowrap") +
                        AddTD(sFeedPerson + "&nbsp;", "nowrap") +
                        AddTD(sCustName + "&nbsp;", "nowrap") +
                         AddTD(sFeedType + "&nbsp;", "nowrap") +
                        AddTD(sFeedBack + "&nbsp;", "nowrap") +
                        AddTD(sOpinion, "width=100%");
                    sTR += AddTD("", "nowrap");
                    sTR = "<tr class='list'>" + sTR + "</tr>";
                    sProcess += sTR;
                }
                if (sProcess != "")
                {
                    sProcess = @"<TABLE id='Table1' cellSpacing='0' border='0' cellPadding='0' width='100%' >
					<TR vAlign='top'>
						<TD  class='listTitle' style='text-align:center; font-weight:bold'>&nbsp;&nbsp;回访情况</TD>
					</TR>
					<TR>
						<TD  class='list'><table cellspacing='0' cellpadding='0' rules='all' border='0' class='listContent' width='100%'  >" + sProcess;
                    ltlHFList.Text = sProcess + "</table></td></tr></table>";
                }
                else
                    ltlHFList.Text = SpecTransText(sProcess);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sText"></param>
        /// <param name="sAttrib"></param>
        /// <returns></returns>
        private string AddTD(string sText, string sAttrib)
        {
            string str = "<td " + sAttrib + " >" + sText + "</td>";
            return str;
        }

        #endregion

        #region 相关事件
        protected void dgIssue_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                e.Item.Attributes.Add("ondblclick", "window.open('../Forms/frmIssueView.aspx?NewWin=true&FlowID=" + DataBinder.Eval(e.Item.DataItem, "FlowID").ToString() + "','','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");

                string sFlowDealState = e.Item.Cells[dgIssueSub.Columns.Count - 1].Text.Trim();
                CheckBox chkDel = (CheckBox)e.Item.Cells[0].FindControl("chkDel");
                if (sFlowDealState == "0")  //待处理
                {
                    chkDel.Checked = false;
                    chkDel.Enabled = true;
                }
                else if (sFlowDealState == "1")
                {
                    chkDel.Checked = true;
                    chkDel.Enabled = true;
                }
                else
                {
                    chkDel.Checked = true;
                    chkDel.Enabled = false;
                }
            }
        }

        protected void dgIssueSubRel_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                e.Item.Attributes.Add("ondblclick", "window.open('../Forms/frmIssueView.aspx?NewWin=true&FlowID=" + DataBinder.Eval(e.Item.DataItem, "FlowID").ToString() + "','','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
            }
        }
        #endregion

        #region 展示服务级别 余向前 2013-04-08
        /// <summary>
        /// 展示服务级别
        /// </summary>
        public void ShowServiceLevel()
        {
            if (hidServiceLevelID.Value != "" && hidServiceLevelID.Value != "0")
            {
                trServiceLevelDetail.Attributes.Add("style", "display:");

                Cst_SLGuidDP ee1 = new Cst_SLGuidDP();
                DataTable dtTmp = ee1.GetDataByLevelIDCache(long.Parse(hidServiceLevelID.Value));

                string sLimit = "";
                foreach (DataRow r in dtTmp.Rows)
                {
                    bool blnWX = false;  //响应时间超时
                    bool blnWF = false;  //完成时间超时

                    if (blnWX == true || blnWF == true)
                    {
                        sLimit += "<font color=red>";
                    }

                    sLimit += r["guidname"].ToString().Trim() + ":" + r["TimeLimit"].ToString().Trim() + GetTimeUnit(r["TimeUnit"].ToString().Trim()) + ",";

                    if (blnWX == true || blnWF == true)
                    {
                        sLimit += "</font>";
                    }
                }
                if (sLimit.EndsWith(","))
                    sLimit = sLimit.Substring(0, sLimit.Length - 1);

                divSLTimeLimt.InnerHtml = "<table class='listContent' width='100%'> <tr><td class='listTitle' width='15%'>时限要求</td><td class='list'>" + sLimit + "</td></tr></table>";

                Cst_ServiceLevelDP sld = new Cst_ServiceLevelDP();
                sld = sld.GetReCorded(long.Parse(hidServiceLevelID.Value));
                divSLDefinition.InnerHtml = "<table class='listContent' width='100%'> <tr><td class='listTitle' width='15%'>级别定义</td><td class='list'>" + sld.Definition + "</td></tr></table>";
                lnkServiceLevel.Text = "详情";

            }
        }
        #endregion

        protected void btdsaas_Click(object sender, EventArgs e)
        {
            Console.WriteLine("22");
        }
    }
}
