/*******************************************************************
 * 版权所有：深圳市非凡信息技术有限公司
 * 描述："需求管理" - 登单页面
 * 
 * 
 * 创建人：孙绍棕
 * 创建日期：2013-04-26
 * 
 * 修改日志：
 * 修改时间：2013-04-26 修改人：孙绍棕
 * 修改描述：
 * *****************************************************************/

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Epower.ITSM.SqlDAL.ES_TBLCS;
using System.Collections.Generic;
using EpowerCom;
using Epower.ITSM.Base;
using EpowerGlobal;
using appDataProcess;
using Epower.ITSM.SqlDAL;
using System.Xml;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.Demand
{
    public partial class frm_REQ_DEMAND : BasePage
    {
        #region 变量
        private objFlow oFlow;
        private FlowForms myFlowForms;
        #endregion

        #region 属性

        /// <summary>
        /// 取得流程FlowID
        /// </summary>
        public string FlowID
        {
            get
            {
                if (ViewState["frm_REQ_DEMAND_FlowID"] != null)
                {
                    return ViewState["frm_REQ_DEMAND_FlowID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["frm_REQ_DEMAND_FlowID"] = value;
            }
        }

        /// <summary>
        /// 应用id
        /// </summary>
        public string AppID
        {
            get
            {
                if (ViewState["frm_REQ_DEMAND_AppID"] != null)
                {
                    return ViewState["frm_REQ_DEMAND_AppID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["frm_REQ_DEMAND_AppID"] = value;
            }
        }
        /// <summary>
        /// 流程模型id
        /// </summary>
        public string FlowModelID
        {
            get
            {
                if (ViewState["frm_REQ_DEMAND_FlowModelID"] != null)
                {
                    return ViewState["frm_REQ_DEMAND_FlowModelID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["frm_REQ_DEMAND_FlowModelID"] = value;
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

        #region 初始化脚本 InitClientScript
        /// <summary>
        /// 初始化脚本

        /// </summary>
        private void InitClientScript()
        {

        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 注册流程处理事件

            myFlowForms = (FlowForms)this.Master;
            myFlowForms.mySetContentReadOnly += new FlowForms.DoContentActions(Master_mySetContentVisible);
            myFlowForms.myGetFormsValue += new FlowForms.GetFormsValue(Master_myGetFormsValue);
            myFlowForms.mySetFormsValue += new FlowForms.DoContentActions(Master_mySetFormsValue);
            myFlowForms.myPreClickCustomize += new FlowForms.DoContentSubmitValid(Master_myPreClickCustomize);
            myFlowForms.myPreSaveClickCustomize += new FlowForms.DoContentValid(Master_myPreSaveClickCustomize);

            #endregion

            string strThisMsg = "";
            strThisMsg = txtCustAddr.ClientID + ">" + "客户";
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), txtCustAddr.ClientID, "<script type=\"text/javascript\" >if(typeof(sarrvalidlist) == 'undefined'){ var sarrvalidlist = new Array(1);}  sarrvalidlist[0] = sarrvalidlist[0] + '|' + '" + strThisMsg + "';</script>");


            #region 第一次进入时, 调用

            if (!IsPostBack)
            {
                InitClientScript();

                PageDeal.SetLanguage(this.Controls[0].Controls[1]);
            }

            #endregion

            #region 快速添加新客户和资产


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

            #endregion            
        }

        #region 提交前执行事件 Master_myPreSaveClickCustomize
        /// <summary>
        /// 提交前执行事件

        /// </summary>
        /// <returns></returns>
        bool Master_myPreSaveClickCustomize()
        {
            Int32 intSubjectLen = CtrFlowFTSubject.Value.Length;
            Int32 intContentLen = txtContent.Value.Length;

            if (intSubjectLen > 200)
            {
                PageTool.MsgBox(Page, "输入的需求主题，可支持的最大长度 200！");
                return false;
            }

            if (intContentLen > 500)
            {
                PageTool.MsgBox(Page, "输入的详细描述，可支持的最大长度 500！");
                return false;
            }

            return true;
        }
        #endregion

        #region 暂存时前面执行事件 Master_myPreClickCustomize
        /// <summary>
        /// 暂存时前面执行事件

        /// </summary>
        /// <returns></returns>
        bool Master_myPreClickCustomize(long lngActionID, string strActionName)
        {
            return true;
        }
        #endregion

        #region 设置表单控件的可见和可编辑.  Master_mySetContentVisible

        /// <summary>
        /// 设置表单控件的可见和可编辑.
        /// </summary>
        private void Master_mySetContentVisible()
        {
            if (CtrFlowFTSubject.ContralState != eOA_FlowControlState.eHidden)
                CtrFlowFTSubject.ContralState = eOA_FlowControlState.eReadOnly;

            if (RegUser.ContralState != eOA_FlowControlState.eHidden)
                RegUser.ContralState = eOA_FlowControlState.eReadOnly;

            if (ctrFCDServiceType.ContralState != eOA_FlowControlState.eHidden)
                ctrFCDServiceType.ContralState = eOA_FlowControlState.eReadOnly;


            if (txtContent.ContralState != eOA_FlowControlState.eHidden)
                txtContent.ContralState = eOA_FlowControlState.eReadOnly;

            if (CtrDealState.ContralState != eOA_FlowControlState.eHidden)
                CtrDealState.ContralState = eOA_FlowControlState.eReadOnly;

            Catalog_DymSchameCtrList1.ReadOnly = true;
            Catalog_DymSchameCtrList1.CatalogID = ctrFCDServiceType.CatelogID;


            bthCreateCus.Visible = false;
            txtCustAddr.Visible = false;
            rWarning.Visible = false;
            cmdCust.Visible = false;
            txtAddr.Visible = false;
            txtContact.Visible = false;
            txtCTel.Visible = false;

            labCustAddr.Visible = true;
            lblAddr.Visible = true;
            labContact.Visible = true;
            labCTel.Visible = true;
            lblCustDeptName.Visible = true;
            lblEmail.Visible = true;
            lblMastCust.Visible = true;
            lbljob.Visible = true;

            bthAddEqu.Visible = false;
            txtEqu.Visible = false;
            cmdEqu.Visible = false;

            lblEqu.Visible = true;

            if (CtrDTRegTime.ContralState != eOA_FlowControlState.eHidden)
                CtrDTRegTime.ContralState = eOA_FlowControlState.eReadOnly;



        }

        #endregion

        #region 设置表单内容.  Master_mySetFormsValue

        /// <summary>
        /// 设置表单内容
        /// </summary>
        private void Master_mySetFormsValue()
        {
            oFlow = myFlowForms.oFlow;

            if (myFlowForms.oFlow.FlowStatus == e_FlowStatus.efsEnd)
            {
                myFlowForms.CtrButtons1.Button3Visible = true;
                myFlowForms.CtrButtons1.ButtonName3 = "知识归档";
                myFlowForms.CtrButtons1.Button3Function = "DoKmAdd(" + myFlowForms.oFlow.MessageID.ToString() + "," + myFlowForms.oFlow.AppID.ToString() + "," + myFlowForms.oFlow.FlowID.ToString() + ");";
            }

            myFlowForms.FormTitle = myFlowForms.oFlow.FlowName;

            #region Master_mySetFormsValue

            hidFlowID.Value = myFlowForms.oFlow.FlowID.ToString();
            this.hidAppID.Value = myFlowForms.oFlow.AppID.ToString();

            #region 打印时传入参数

            //打印时需要传入的参数
            this.FlowID = myFlowForms.oFlow.FlowID.ToString();
            this.FlowModelID = myFlowForms.oFlow.FlowModelID.ToString();
            this.AppID = myFlowForms.oFlow.AppID.ToString();
            #endregion

            ImplDataProcess dp = new ImplDataProcess(myFlowForms.oFlow.AppID);
            DataSet ds = dp.GetFieldsDataSet(myFlowForms.oFlow.FlowID, myFlowForms.oFlow.OpID);

            DataTable dt = ds.Tables[0];
            if (dt != null)
            {
                Boolean isNewDraft = dt.Rows.Count <= 0;
                if (isNewDraft)
                {
                    #region 起草新的需求单

                    CtrDTRegTime.dateTime = DateTime.Now;

                    if (Session["ExtendParameter"] != null && Session["ExtendParameter"].ToString() != string.Empty && Session["ExtendParameter"].ToString() != "0")
                    {
                        string extendParameter = Session["ExtendParameter"].ToString();
                        if (extendParameter.IndexOf("Fast") != -1)//快速发起需求
                        {
                            long lngTemplateID = long.Parse(extendParameter.Replace("Fast", ""));//得到模板ID

                            SetFromFlowValues(lngTemplateID); //根据模板配置赋初值
                        }
                    }
                    else
                    {
                        #region 设置缺省客户和资产资料



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

                                hidListName.Value = ee.ListName.ToString();
                                hidListID.Value = ee.ListID.ToString();
                            }
                        }

                        #endregion
                    }


                    #region 设置登单人资料



                    if (RegUser.UserID == null || RegUser.UserID == 0)
                    {
                        RegUser.UserID = long.Parse(Session["UserID"].ToString());
                        RegUser.UserName = Session["PersonName"].ToString();
                    }

                    #endregion

                    #endregion
                }
                else
                {
                    #region 已添加的需求单

                    DataRow row = dt.Rows[0];

                    #region 客户资料

                    hidCustID.Value = row["CustUserID"].ToString();
                    txtCustAddr.Text = row["CustUserName"].ToString();
                    labCustAddr.Text = StringTool.ParseForHtml(SpecTransText(txtCustAddr.Text));

                    txtAddr.Text = row["CustAddress"].ToString();
                    hidAddr.Value = row["CustAddress"].ToString();
                    lblAddr.Text = row["CustAddress"].ToString();

                    txtContact.Text = row["CustContact"].ToString();
                    labContact.Text = StringTool.ParseForHtml(SpecTransText(txtContact.Text));
                    txtCTel.Text = row["CustTel"].ToString();
                    labCTel.Text = StringTool.ParseForHtml(SpecTransText(txtCTel.Text));

                    hidMastCust.Value = row["CustMastName"].ToString();
                    hidCustDeptName.Value = row["CustDeptName"].ToString();
                    lblCustDeptName.Text = StringTool.ParseForHtml(SpecTransText(hidCustDeptName.Value));
                    hidCustEmail.Value = row["CustEmail"].ToString();
                    lblEmail.Text = StringTool.ParseForHtml(SpecTransText(hidCustEmail.Value));
                    lblMastCust.Text = StringTool.ParseForHtml(SpecTransText(row["CustMastName"].ToString()));
                    lbljob.Text = StringTool.ParseForHtml(SpecTransText(row["CustJob"].ToString()));
                    hidjob.Value = row["CustJob"].ToString();

                    #endregion

                    #region 登单人资料



                    RegUser.UserName = row["RegUserName"].ToString();
                    if (row["RegUserID"].ToString().Length > 0)
                    {
                        RegUser.UserID = long.Parse(row["RegUserID"].ToString());
                    }

                    CtrDTRegTime.dateTime = (DateTime)row["RegTime"];

                    #endregion

                    #region 需求状态



                    if (row["DemandStatusID"].ToString() != string.Empty)
                        CtrDealState.CatelogID = long.Parse(row["DemandStatusID"].ToString());

                    #endregion

                    #region 需求单号, 主题，详细描述



                    lblBuildCode.Text = row["DemandNo"].ToString();
                    CtrFlowFTSubject.Value = row["DemandSubject"].ToString();
                    txtContent.Value = row["DemandContent"].ToString();

                    #endregion

                    #region 资产资料


                    hidListName.Value = row["EquipmentCatalogName"].ToString();
                    hidListID.Value = row["EquipmentCatalogID"].ToString() == "" ? "0" : row["EquipmentCatalogID"].ToString();

                    lblEqu.Text = row["EquipmentName"].ToString();  //资产信息
                    txtEqu.Text = row["EquipmentName"].ToString();
                    hidEquName.Value = row["EquipmentName"].ToString();
                    hidEqu.Value = row["Equipmentid"].ToString() == "" ? "0" : row["Equipmentid"].ToString();


                    //ChangeFlowID = row["AssociateFlowID"].ToString();

                    #endregion

                    #region 需求类别



                    //如果有默认初值，则设置选定的默认值


                    ctrFCDServiceType.CatelogID = long.Parse(row["DemandTypeID"].ToString());
                    ctrFCDServiceType.CatelogValue = row["DemandTypeID"].ToString();

                    #endregion

                    #region 需求模板相关
                    hidReqTempID.Value = row["ReqTempID"].ToString() == "" ? "0" : row["ReqTempID"].ToString();
                    #endregion

                    #endregion
                }
            }

            #endregion

            BR_ProgressBarDP pBr_ProgressBar = new BR_ProgressBarDP();
            pBr_ProgressBar = pBr_ProgressBar.GetRecorded(myFlowForms.oFlow.AppID, FlowDP.GetOFlowModelID(myFlowForms.oFlow.FlowModelID), myFlowForms.oFlow.NodeModelID);

            ShowImg.Src = pBr_ProgressBar.ImgURL;

            #region set visible
            int iShowBase = 0;     //是否显示需求基本资料

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
                    //基本信息
                    case "reguserid":
                        RegUser.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            RegUser.ContralState = eOA_FlowControlState.eHidden;
                            iShowBase += 1;
                        }
                        break;
                    case "demandtypeid":
                        ctrFCDServiceType.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            ctrFCDServiceType.ContralState = eOA_FlowControlState.eHidden;
                            iShowBase += 1;
                        }
                        break;
                    case "demandsubject":
                        CtrFlowFTSubject.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrFlowFTSubject.ContralState = eOA_FlowControlState.eHidden;
                            iShowBase += 1;
                        }
                        break;
                    case "demandcontent":
                        txtContent.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            txtContent.ContralState = eOA_FlowControlState.eHidden;
                            iShowBase += 1;
                        }

                        break;
                    case "demandstatusid":
                        CtrDealState.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrDealState.ContralState = eOA_FlowControlState.eHidden;
                            lblDealStatus.Visible = false;
                            iShowBase += 1;                       //事件状态

                        }
                        break;
                    //客户信息
                    case "custuserid":
                        bthCreateCus.Visible = false;
                        txtCustAddr.Visible = false;
                        rWarning.Visible = false;
                        cmdCust.Visible = false;
                        txtAddr.Visible = false;
                        txtContact.Visible = false;
                        txtCTel.Visible = false;

                        if (sf.Visibled == true)
                        {
                            labCustAddr.Visible = true;
                            lblAddr.Visible = true;
                            labContact.Visible = true;
                            labCTel.Visible = true;
                            lblCustDeptName.Visible = true;
                            lblEmail.Visible = true;
                            lblMastCust.Visible = true;
                            lbljob.Visible = true;
                        }
                        else
                        {
                            labCustAddr.Visible = false;
                            lblAddr.Visible = false;
                            labContact.Visible = false;
                            labCTel.Visible = false;
                            lblCustDeptName.Visible = false;
                            lblEmail.Visible = false;
                            lblMastCust.Visible = false;
                            lbljob.Visible = false;
                        }

                        break;

                    //资产名称
                    case "equipmentid":
                        bthAddEqu.Visible = false;
                        txtEqu.Visible = false;
                        cmdEqu.Visible = false;
                        if (sf.Visibled == true)
                        {
                            lblEqu.Visible = true;
                        }                  
                        break;
                    case "exta_schema":
                        Catalog_DymSchameCtrList1.ReadOnly = true;

                        if (!sf.Visibled)
                        {
                            trshema.Visible = false;
                        }

                        break;
                    case "demandno":
                        lblBuildCode.Visible = false;

                        if (sf.Visibled)
                        {
                            lblBuildCode.Visible = true;
                        }
                        break;

                    default:
                        break;

                }
            }

            #endregion

            #region 常用类别配置项


            Catalog_DymSchameCtrList1.SchemaRelateType = eSchemaRelateType.eFlowType;
            Catalog_DymSchameCtrList1.RelateID = long.Parse(hidFlowID.Value);
            Catalog_DymSchameCtrList1.CatalogID = ctrFCDServiceType.CatelogID;

            #endregion
        }

        #endregion

        #region 通过模版赋初值
        /// <summary>
        /// 通过模版赋初值
        /// </summary>
        /// <param name="lngTemplateID"></param>
        private void SetFromFlowValues(long lngTemplateID)
        {
            EA_ShortCutTemplateDP ee = new EA_ShortCutTemplateDP();
            ee = ee.GetReCorded(lngTemplateID);

            if (ee.TemplateXml.Length != 0)
            {
                #region 获取模板的值
                hidReqTempID.Value = lngTemplateID.ToString();
                FieldValues fv = new FieldValues(ee.TemplateXml);

                if (fv.GetFieldValue("DemandTypeID") != null && fv.GetFieldValue("DemandType") != null)
                {
                    //需求类别
                    ctrFCDServiceType.CatelogID = CTools.ToInt64(fv.GetFieldValue("DemandTypeID").Value == "" ? "0" : fv.GetFieldValue("DemandTypeID").Value);
                    ctrFCDServiceType.CatelogValue = fv.GetFieldValue("DemandType").Value;
                }

                #endregion

                #region 设置快捷内容 
                if (Session["ReqDemandShortXml"] != null)
                {
                    //获取快捷登单的信息
                    FieldValues fv_Tmp = new FieldValues();
                    fv_Tmp = new FieldValues(Session["ReqDemandShortXml"].ToString());

                    //详细描述
                    if (fv_Tmp.GetFieldValue("ReqContext") != null && fv_Tmp.GetFieldValue("ReqContext").Value.ToString() != "")
                    {
                        txtContent.Value = fv_Tmp.GetFieldValue("ReqContext").Value.ToString();
                    }

                    //客户
                    if (fv_Tmp.GetFieldValue("ReqCustID") != null && fv_Tmp.GetFieldValue("ReqCustID").Value.ToString() != "" && fv_Tmp.GetFieldValue("ReqCustID").Value.ToString() != "0")
                    {
                        hidCustID.Value = fv_Tmp.GetFieldValue("ReqCustID").Value.ToString();

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
                    }

                    //资产
                    if (fv_Tmp.GetFieldValue("EquID") != null && fv_Tmp.GetFieldValue("EquID").Value.ToString() != "" && fv_Tmp.GetFieldValue("EquID").Value.ToString() != "0")
                    {
                        hidEqu.Value = fv_Tmp.GetFieldValue("EquID").Value.ToString();                       

                        //取得资产资料
                        Equ_DeskDP equ = new Equ_DeskDP();
                        equ = equ.GetReCorded(long.Parse(hidEqu.Value == "" ? "0" : hidEqu.Value));
                        txtEqu.Text = equ.Name;
                        lblEqu.Text = equ.Name;
                        hidEqu.Value = equ.ID.ToString();
                        hidEquName.Value = equ.Name;
                        //资产目录                            
                        hidListName.Value = equ.ListName.ToString();
                        hidListID.Value = equ.ListID.ToString();
                    }

                }
                #endregion
            }
        }
        #endregion

        #region 取表单内容  Master_myGetFormsValue

        /// <summary>
        /// 取表单内容 
        /// </summary>
        /// <returns></returns>
        XmlDocument Master_myGetFormsValue(long lngActionID, string strActionName)
        {
            FieldValues fv = new FieldValues();

            #region 需求主题和详细描述

            fv.Add("DemandSubject", CtrFlowFTSubject.Value.Trim().Replace(System.Web.HttpUtility.HtmlDecode("&#160;"), "").Replace(System.Web.HttpUtility.HtmlDecode("&nbsp;"), ""));
            fv.Add("DemandContent", txtContent.Value.Trim());

            #endregion

            #region 需求类别


            fv.Add("DemandTypeID", ctrFCDServiceType.CatelogID.ToString().Trim());
            fv.Add("DemandTypeName", ctrFCDServiceType.CatelogValue.Trim());

            #endregion

            #region 需求状态


            fv.Add("DemandStatusID", CtrDealState.CatelogID.ToString());
            fv.Add("DemandStatus", CtrDealState.CatelogValue.ToString());

            #endregion

            #region 客户资料

            fv.Add("CustUserID", this.hidCustID.Value.Trim() == string.Empty ? "0" : this.hidCustID.Value.Trim());
            fv.Add("CustUserName", txtCustAddr.Text.Trim());
            fv.Add("CustAddress", txtAddr.Text.Trim());
            fv.Add("CustContact", txtContact.Text.Trim());
            fv.Add("CustTel", txtCTel.Text.Trim());
            fv.Add("CustDeptName", lblCustDeptName.Text.Trim());
            fv.Add("CustJob", lbljob.Text.Trim());
            fv.Add("CustJobID", "0");
            fv.Add("CustEmail", hidCustEmail.Value.Trim());
            fv.Add("CustMastName", lblMastCust.Text.Trim());

            #endregion

            #region 资产信息

            fv.Add("Equipmentid", this.hidEqu.Value.Trim() == string.Empty ? "0" : this.hidEqu.Value.Trim());
            fv.Add("EquipmentName", this.txtEqu.Text.Trim());
            fv.Add("EquipmentCatalogName", hidListName.Value.Trim()); //资产目录
            fv.Add("EquipmentCatalogID", hidListID.Value.Trim() == string.Empty ? "0" : hidListID.Value.Trim());

            #endregion

            #region 登单人资料


            fv.Add("RegUserID", RegUser.UserID.ToString().Trim());
            fv.Add("RegUserName", RegUser.UserName.Trim());
            fv.Add("RegOrgID", Session["UserOrgID"].ToString());
            fv.Add("RegTime", DateTime.Now.ToString());

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

            fv.Add("EmailNotify", myFlowForms.EmailValue.ToString());
            fv.Add("SMSNotify", myFlowForms.NotifyValue.ToString());

            #endregion

            #region 常用类别配置项


            string strCalalogSchema = GetCatalogSchema();
            fv.Add("CalalogSchema", strCalalogSchema);            
            fv.Add("CatalogSchemaRelateType", ((int)Catalog_DymSchameCtrList1.SchemaRelateType).ToString());

            #endregion

            #region 事件模板相关
            fv.Add("ReqTempID", hidReqTempID.Value == "" ? "0" : hidReqTempID.Value);
            fv.Add("IsUseReqTempID", "1");
            #endregion

            return fv.GetXmlObject();
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

        #region 取常用类别配置项的值
        /// <summary>
        /// 取常用类别配置项的值
        /// </summary>
        /// <returns></returns>
        private string GetCatalogSchema()
        {
            List<BR_Schema_Deploy> list = Catalog_DymSchameCtrList1.contorRtnValue;

            string str = "";           

            str= Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented);
            
            return str;
        }

        #endregion

        protected void btnShema_Click(object sender, EventArgs e)
        {
            Catalog_DymSchameCtrList1.CatalogID = ctrFCDServiceType.CatelogID;
            Catalog_DymSchameCtrList1.SetAddEquTrue();
        }
    }
}
