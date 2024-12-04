using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
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

namespace Epower.ITSM.Web.AppForms
{
    public partial class CST_Issue_Base_Self : BasePage
    {
        #region 变量
        private objFlow oFlow;
        private FlowForms myFlowForms;
        private long lngRequestID = 0;   //表示 公共请求的 ID
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
        /// 指引
        /// </summary>
        public string KeyValue
        {
            get { if (ViewState["FastKeyValue"] != null) return ViewState["FastKeyValue"].ToString(); else return string.Empty; }
            set
            {
                ViewState["FastKeyValue"] = value;
                this.txtContent.Text = value;
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

        #region 判断是否自助模式查询页面进入的 余向前 2013-04-22
        /// <summary>
        /// 判断是否自助模式查询页面进入的
        /// </summary>
        public bool IsSelfMode
        {
            get
            {
                if (Request.QueryString["IsSelfMode"] != null && Request.QueryString["IsSelfMode"].ToLower() == "true")
                    return true;
                else
                    return false;
            }
        }
        #endregion

        #endregion

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            SetLitText();

            myFlowForms = (FlowForms)this.Master;
            myFlowForms.mySetContentReadOnly += new FlowForms.DoContentActions(Master_mySetContentReadOnly);
            myFlowForms.myGetFormsValue += new FlowForms.GetFormsValue(Master_myGetFormsValue);
            myFlowForms.mySetFormsValue += new FlowForms.DoContentActions(Master_mySetFormsValue);
            myFlowForms.myPreClickCustomize += new FlowForms.DoContentSubmitValid(Master_myPreClickCustomize);
            myFlowForms.myPreSaveClickCustomize += new FlowForms.DoContentValid(Master_myPreSaveClickCustomize);

            string strThisMsg = "";
            strThisMsg = txtContent.ClientID + ">" + "详细描述";
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), txtContent.ClientID, "<script type=\"text/javascript\" >if(typeof(sarrvalidlist) == 'undefined'){ var sarrvalidlist = new Array(1);}  sarrvalidlist[0] = sarrvalidlist[0] + '|' + '" + strThisMsg + "';</script>");


            if (this.hidAppID.Value != "0")
            {               
                EpowerCom.objFlow oFlow2 = new objFlow(long.Parse(Session["UserID"].ToString()), long.Parse(FlowModelID), long.Parse(MessageID));
                Extension_DayCtrList1.NodeModelID = oFlow2.NodeModelID;

                Extension_DayCtrList1.EquID = long.Parse(this.hidFlowID.Value.ToString());
                Extension_DayCtrList1.EquCategoryID = oFlow2.FlowModelID;


            }

        }

        /// <summary>
        /// 设置自定义名称
        /// 廖世进 2013-05-16 添加
        /// </summary>
        void SetLitText()
        {
            this.LitCustName.Text = PageDeal.GetLanguageValue("CST_CustName");
            this.LitCustAddress.Text = PageDeal.GetLanguageValue("CST_CustAddress");
            this.LitContact.Text = PageDeal.GetLanguageValue("CST_CustContract");
            this.LitCTel.Text = PageDeal.GetLanguageValue("CST_CustPhone");
            this.LitCustDeptName.Text = PageDeal.GetLanguageValue("CST_CustDeptName");
            this.litCustEmail.Text = PageDeal.GetLanguageValue("CST_CustEmail");
            this.LitMastShortName.Text = PageDeal.GetLanguageValue("CST_CustMastName");
            this.LitJob.Text = PageDeal.GetLanguageValue("CST_CustJob");
            this.LitEquipmentName.Text = PageDeal.GetLanguageValue("CST_EquName");
            this.LitContent.Text = PageDeal.GetLanguageValue("CST_Content");
            this.LitEquList.Text = PageDeal.GetLanguageValue("CST_EquList");
        }

        #endregion

        #region 设置页面为只读 Master_mySetContentReadOnly

        /// <summary>
        /// 设置页面为只读
        /// </summary>
        void Master_mySetContentReadOnly()
        {
            #region Master_mySetContentReadOnly

            if (txtContent.Visible == true)
            {
                labContent.Visible = true;
                txtContent.Visible = false;
                rWarning.Visible = false;
            }


            if (txtContact.Visible == true)
                labContact.Visible = true;
            txtContact.Visible = false;

            if (txtCTel.Visible == true)
                labCTel.Visible = true;
            txtCTel.Visible = false;

            if (txtCustAddr.Visible == true)
            {

                labCustAddr.Visible = true;
                if (labCustAddr.Text.Trim() == "")
                {
                    lnkCustHistory.Visible = false;
                }
            }

            txtCustAddr.Visible = false;
            cmdCust.Visible = false;

            if (txtAddr.Visible == true)
                lblAddr.Visible = true;
            txtAddr.Visible = false;


            //没有客户赋值时 不显示历史参考
            if (txtCustAddr.Text.Trim() == "" || txtCustAddr.Text.Trim() == "--")
            {
                lnkCustHistory.Visible = false;
            }
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
                lblListName.Visible = true;
            txtListName.Visible = false;
            cmdListName.Visible = false;

            //没有设备赋值时 不显示历史参考
            if (txtEqu.Text.Trim() == "" || txtEqu.Text.Trim() == "--")
                lnkEquHistory.Visible = false;


            #endregion

            #region 扩展属性 2014-05-12 yxq
            Extension_DayCtrList1.ReadOnly = true;

            if (this.hidAppID.Value != "0")
            {                
                Extension_DayCtrList1.NodeModelID = this.FlowMaster.oFlow.NodeModelID;

                Extension_DayCtrList1.EquID = long.Parse(this.hidFlowID.Value.ToString());
                Extension_DayCtrList1.EquCategoryID = this.FlowMaster.oFlow.FlowModelID;
            }
            #endregion
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
            fv.Add("Subject", "");
            fv.Add("Content", txtContent.Text.Trim());
            fv.Add("RegUserID", Session["UserID"].ToString());
            fv.Add("RegUserName", Session["PersonName"].ToString());
            fv.Add("ServiceLevelID", hidServiceLevelID.Value == "" ? "0" : hidServiceLevelID.Value);
            fv.Add("ServiceLevel", hidServiceLevel.Value);
            fv.Add("ServiceLevelChange", "false");
            fv.Add("ServiceTypeID", hidServiceTypeID.Value == "" ? "0" : hidServiceTypeID.Value);
            fv.Add("ServiceType", hidServiceType.Value);
            fv.Add("ServiceKindID", "0");
            fv.Add("ServiceKind", "");
            fv.Add("EffectID", hidEffectID.Value == "" ? "-1" : hidEffectID.Value);
            fv.Add("EffectName", hidEffectName.Value);
            fv.Add("InstancyID", hidInstancyID.Value == "" ? "-1" : hidInstancyID.Value);
            fv.Add("InstancyName", hidInstancyName.Value);
            fv.Add("DealStatusID", "10002");   //事件状态ID
            fv.Add("DealStatus", "事件派单");           //事件状态
            fv.Add("CustTime", ""); //发生时间
            fv.Add("ReportingTime", ""); //报告时间
            fv.Add("CustID", this.hidCustID.Value.Trim() == string.Empty ? "0" : this.hidCustID.Value.Trim());
            fv.Add("CustName", txtCustAddr.Text.Trim());
            fv.Add("CustAddress", txtAddr.Text.Trim());
            fv.Add("Contact", txtContact.Text.Trim());
            fv.Add("CTel", txtCTel.Text.Trim());
            fv.Add("CustDeptName", lblCustDeptName.Text.Trim());
            fv.Add("Job", lbljob.Text.Trim());
            fv.Add("Email", lblEmail.Text.Trim());
            fv.Add("MastCust", lblMastCust.Text.Trim());
            fv.Add("EquipmentID", this.hidEqu.Value.Trim() == string.Empty ? "0" : this.hidEqu.Value.Trim());
            fv.Add("EquipmentName", this.txtEqu.Text.Trim());
            fv.Add("EquipmentCatalogName", hidListName.Value.Trim()); //资产分类
            fv.Add("EquipmentCatalogID", hidListID.Value.Trim() == string.Empty ? "0" : hidListID.Value.Trim());
            fv.Add("EquPositions", "");
            fv.Add("EquCode", "");
            fv.Add("EquSN", "");
            fv.Add("EquModel", "");
            fv.Add("EquBreed", "");
            fv.Add("DealContent", "");
            fv.Add("Outtime", "");
            fv.Add("ServiceTime", "");
            fv.Add("FinishedTime", "");
            fv.Add("SjwxrID", "");
            fv.Add("Sjwxr", "");
            fv.Add("TotalAmount", "0");
            fv.Add("OrgID", Session["UserOrgID"].ToString());
            fv.Add("RegSysDate", DateTime.Now.ToString());
            fv.Add("RegSysUserID", Session["UserID"].ToString());
            fv.Add("RegSysUser", Session["PersonName"].ToString());
            fv.Add("EmailNotify", myFlowForms.EmailValue.ToString());
            fv.Add("SMSNotify", myFlowForms.NotifyValue.ToString());
            //如果登单人不同，取相应的部门和部门名称
            fv.Add("RegDeptID", Session["UserDeptID"].ToString());
            fv.Add("RegDeptName", Session["UserDeptName"].ToString());
            //取得服务单前缀，生成事件单号
            string sBuildCode = hidBuildCode.Value.Trim();
            string sServiceNo = hidServiceNo.Value.Trim();
            if (sServiceNo == string.Empty)
            {
                sBuildCode = string.Empty;
                sServiceNo = RuleCodeDP.GetCodeBH2(10003, ref sBuildCode);
            }
            fv.Add("serviceno", sServiceNo);
            fv.Add("buildCode", sBuildCode);
            fv.Add("Flag", "false");  //区分标志
            fv.Add("CloseReasonID", hidCloseReasonID.Value == "" ? "-1" : hidCloseReasonID.Value);  //关闭理由ID
            fv.Add("CloseReasonName", hidCloseReasonName.Value);        //关闭理由名称
            fv.Add("ReSouseID", hidReSouseID.Value == "" ? "-1" : hidReSouseID.Value);          //事件来源ID
            fv.Add("ReSouseName", hidReSouseName.Value);                //事件来源名称
            fv.Add("pubRequestID", lngRequestID.ToString());   //2009-04-25 增加 公众请求ID


            //事件模板相关
            fv.Add("IssTempID", hidIssTempID.Value);
            fv.Add("IsUseIssTempID", "0");

            fv.Add("CustAreaID", "0");
            fv.Add("CustArea", "");
            fv.Add("ApplicationTime", "");
            fv.Add("ExpectedTime", "");
            fv.Add("Reason", "");

            int iItemCount = 0;
            fv.Add("ItemCount", "0");
            fv.Add("ItemXml", "");
            string ExtensionDayList = SaveExtensionDayList();
            fv.Add("ExtensionDayList", ExtensionDayList);  //扩展项
            

            #endregion

            return fv.GetXmlObject();
        }
        #endregion

        /// <summary> 
        /// 设置窗体值
        /// </summary>
        void Master_mySetFormsValue()
        {
            #region 设置表单值

            oFlow = myFlowForms.oFlow;

            myFlowForms.FormTitle = oFlow.FlowName;
            this.FlowID = myFlowForms.oFlow.FlowID.ToString();
            this.MessageID = myFlowForms.oFlow.MessageID.ToString();
            this.AppID = myFlowForms.oFlow.AppID.ToString();
            this.FlowModelID = myFlowForms.oFlow.FlowModelID.ToString();
            this.hidAppID.Value = this.AppID;
            this.hidFlowID.Value = this.FlowID;

            #region 如果是从自助模式查询页面进入，需要设置页面只读 余向前 2013-04-22
            if (IsSelfMode)
            {
                myFlowForms.IsSelfMode = true;
            }
            #endregion

            DataTable dt = null;
            if (oFlow.MessageID != 0)
            {
                ImplDataProcess dp = new ImplDataProcess(oFlow.AppID);
                DataSet ds = dp.GetFieldsDataSet(oFlow.FlowID, oFlow.OpID);
                dt = ds.Tables[0];
            }
            else
            {
                if (Session["ExtendParameter"] != null && Session["ExtendParameter"].ToString() != string.Empty && Session["ExtendParameter"].ToString() != "0")
                {
                    string extendParameter = Session["ExtendParameter"].ToString();
                    EA_ServicesTemplateDP ee = new EA_ServicesTemplateDP();
                    ee = ee.GetReCorded(long.Parse(extendParameter));
                    KeyValue = ee.TemplateName + ":" + ee.Guide;
                    hidSubject.Value = ee.TemplateName + "--" + Session["PersonName"].ToString();
                    hidIssTempID.Value = ee.IssTempID.ToString() == "" ? "0" : ee.IssTempID.ToString();
                    txtContent.Text = KeyValue;

                    #region 根据模板ID获取模板设置的初始值
                    if (ee.IssTempID > 0)
                    {
                        EA_ShortCutTemplateDP ect = new EA_ShortCutTemplateDP();
                        ect = ect.GetReCorded(long.Parse(ee.IssTempID.ToString()));
                        FieldValues fv = new FieldValues(ect.TemplateXml);

                        if (fv.GetFieldValue("InstancyID") != null && fv.GetFieldValue("InstancyName") != null)
                        {
                            hidInstancyID.Value = fv.GetFieldValue("InstancyID").Value;
                            hidInstancyName.Value = fv.GetFieldValue("InstancyName").Value;
                        }
                        if (fv.GetFieldValue("EffectID") != null && fv.GetFieldValue("EffectName") != null)
                        {
                            hidEffectID.Value = fv.GetFieldValue("EffectID").Value;
                            hidEffectName.Value = fv.GetFieldValue("EffectName").Value;
                        }
                        if (fv.GetFieldValue("ReasonID") != null && fv.GetFieldValue("ReasonName") != null)
                        {
                            //关闭状态理由
                            hidCloseReasonID.Value = fv.GetFieldValue("ReasonID").Value;
                            hidCloseReasonName.Value = fv.GetFieldValue("ReasonName").Value;
                        }
                        if (fv.GetFieldValue("FromID") != null && fv.GetFieldValue("FromName") != null)
                        {
                            //事件来源
                            hidReSouseID.Value = fv.GetFieldValue("FromID").Value;
                            hidReSouseName.Value = fv.GetFieldValue("FromName").Value;
                        }
                        if (fv.GetFieldValue("ServiceTypeID") != null && fv.GetFieldValue("ServiceType") != null)
                        {
                            //事件类别
                            hidServiceTypeID.Value = fv.GetFieldValue("ServiceTypeID").Value;
                            hidServiceType.Value = fv.GetFieldValue("ServiceType").Value;
                        }

                        if (fv.GetFieldValue("ServiceLevelID") != null && fv.GetFieldValue("ServiceLevel") != null)
                        {
                            hidServiceLevelID.Value = fv.GetFieldValue("ServiceLevelID").Value;
                            hidServiceLevel.Value = fv.GetFieldValue("ServiceLevel").Value;
                        }
                    }
                    #endregion

                }
                else
                {
                    hidSubject.Value = oFlow.FlowName + "--" + Session["PersonName"].ToString();
                }

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
                    hidCustDeptName.Value = ec.CustDeptName;
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

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    hidServiceID.Value = row["smsid"].ToString();
                    hidBuildCode.Value = row["BuildCode"].ToString();
                    hidServiceNo.Value = row["ServiceNo"].ToString();


                    #region 详细描述

                    txtContent.Text = row["Content"].ToString();
                    labContent.Text = row["Content"].ToString();

                    #endregion

                    #region 客户信息

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

                    hidCustDeptName.Value = row["CustDeptName"].ToString();
                    lblCustDeptName.Text = StringTool.ParseForHtml(SpecTransText(hidCustDeptName.Value));
                    hidCustEmail.Value = row["Email"].ToString();
                    lblEmail.Text = StringTool.ParseForHtml(SpecTransText(hidCustEmail.Value));
                    lblMastCust.Text = StringTool.ParseForHtml(SpecTransText(row["MastCust"].ToString()));
                    lbljob.Text = StringTool.ParseForHtml(SpecTransText(row["job"].ToString()));
                    hidjob.Value = row["job"].ToString();

                    #endregion

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

                    #region 事件单模板设置初始值
                    hidInstancyID.Value = row["InstancyID"].ToString();
                    hidInstancyName.Value = row["InstancyName"].ToString();
                    hidEffectID.Value = row["EffectID"].ToString();
                    hidEffectName.Value = row["EffectName"].ToString();
                    //关闭状态理由
                    hidCloseReasonID.Value = row["CloseReasonID"].ToString();
                    hidCloseReasonName.Value = row["CloseReasonName"].ToString();
                    //事件来源
                    hidReSouseID.Value = row["ReSouseID"].ToString();
                    hidReSouseName.Value = row["ReSouseName"].ToString();
                    //事件类别
                    hidServiceTypeID.Value = row["ServiceTypeID"].ToString();
                    hidServiceType.Value = row["ServiceType"].ToString();
                    hidServiceLevelID.Value = row["ServiceLevelID"].ToString().Trim();
                    hidServiceLevel.Value = row["ServiceLevel"].ToString().Trim();
                    #endregion
                }
            }

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
                    //基本信息
                    case "reguserid":
                        if (sf.Visibled == false)
                        {
                            iShowBase += 1;
                        }
                        break;
                    case "servicetypeid":
                        if (sf.Visibled == false)
                        {
                            iShowBase += 1;
                        }
                        break;
                    case "servicekindid":
                        if (sf.Visibled == false)
                        {
                            iShowBase += 1;
                        }
                        break;
                    case "servicelevel":
                        if (sf.Visibled == false)
                        {
                            iShowBase += 1;
                        }
                        break;
                    case "effectid":
                        if (sf.Visibled == false)
                        {
                            iShowBase += 1;
                        }
                        break;
                    case "instancyid":
                        if (sf.Visibled == false)
                        {
                            iShowBase += 1;
                        }
                        break;
                    case "subject":
                        if (sf.Visibled == false)
                        {
                            iShowBase += 1;
                        }
                        break;
                    case "content":
                        txtContent.Visible = false;
                        rWarning.Visible = false;
                        if (sf.Visibled == true)
                            labContent.Visible = true;
                        else
                            iShowBase += 1;
                        break;
                    case "custtime":
                        if (sf.Visibled == false)
                        {
                            iShowBase += 1;
                        }
                        break;
                    case "reportingtime":  //报告时间
                        if (sf.Visibled == false)
                        {
                            iShowBase += 1;
                        }
                        break;
                    case "dealstatusid":
                        if (sf.Visibled == false)
                        {
                            iShowBase += 1;                       //事件状态

                        }
                        break;
                    case "closereason":                         //关闭状态理由

                        if (sf.Visibled == false)
                        {
                            iShowBase += 1;
                        }
                        break;
                    case "resouse":                             //事件来源
                        if (sf.Visibled == false)
                        {
                            iShowBase += 1;
                        }
                        break;
                    //客户信息
                    case "custinfo":
                        txtCustAddr.Visible = false;
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

                        //   bthCreateCus.Visible = false;
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
                            lblListName.Visible = true;

                        //      bthAddEqu.Visible = false;
                        if (sf.Visibled == false)
                        {
                            Table15.Visible = false;
                            Table3.Visible = false;
                        }
                        break;


                    //处理信息
                    case "dealcontent":
                        if (sf.Visibled == true)
                        { }
                        else
                            iShowDeal += 1;
                        break;
                    case "outtime":
                        if (sf.Visibled == false)
                        {
                            iShowDeal += 1;
                        }
                        break;
                    case "servicetime":
                        if (sf.Visibled == false)
                        {
                            iShowDeal += 1;
                        }
                        break;
                    case "finishedtime":
                        if (sf.Visibled == false)
                        {
                            iShowDeal += 1;
                        }
                        break;
                    case "sjwxr":
                        if (sf.Visibled)
                        {
                        }
                        else
                        {
                            iShowDeal += 1;
                        }
                        break;
                    case "totalhours":
                        if (sf.Visibled == true)
                        {
                        }
                        else
                        {
                            iShowDeal += 1;
                        }
                        break;
                    case "totalamount":
                        if (sf.Visibled == true)
                        {
                        }
                        else
                        {
                            iShowDeal += 1;
                        }
                        break;
                    case "faredetail":
                        if (sf.Visibled == true)
                        {
                        }
                        else
                        {
                            iShowDeal += 1;   //8
                        }
                        break;
                    case "expr_property":    // 扩展属性 - 2014-05-12 yxq
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

            #region 扩展项 2014-05-12 yxq

            Extension_DayCtrList1.EquID = long.Parse(this.hidFlowID.Value.ToString());

            if (this.hidAppID.Value.ToString() != "0")
            {                
                Extension_DayCtrList1.NodeModelID = this.FlowMaster.oFlow.NodeModelID;
                Extension_DayCtrList1.EquCategoryID = this.FlowMaster.oFlow.FlowModelID;

            }

            #endregion
        }

        #region  提交前执行事件Master_myPreClickCustomize

        /// <summary>
        /// 暂存时前面执行事件
        /// </summary>
        /// <returns></returns>
        bool Master_myPreClickCustomize(long lngActionID, string strActionName)
        {
            return true;
        }

        #endregion

        #region 暂存时前面执行事件 Master_myPreSaveClickCustomize

        /// <summary>
        /// 提交前执行事件
        /// </summary>
        /// <returns></returns>
        bool Master_myPreSaveClickCustomize()
        {
            return true;
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
    }
}
