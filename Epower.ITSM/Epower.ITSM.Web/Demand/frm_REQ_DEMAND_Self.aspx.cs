/*******************************************************************
•	 * 版权所有：深圳市非凡信息技术有限公司
•	 * 描述：需求自助登单页面
•	
•	 * 
•	 * 
•	 * 创建人：余向前
•	 * 创建日期：2013-04-26
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
using EpowerCom;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;
using Epower.DevBase.BaseTools;
using appDataProcess;
using System.Xml;
using System.Collections.Generic;

namespace Epower.ITSM.Web.Demand
{
    public partial class frm_REQ_DEMAND_Self : BasePage
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
                if (ViewState["frm_Demand_Base_FlowID"] != null)
                {
                    return ViewState["frm_Demand_Base_FlowID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["frm_Demand_Base_FlowID"] = value;
            }
        }

        /// <summary>
        /// 指引
        /// </summary>
        public string KeyValue
        {
            get { if (ViewState["DemandFastKeyValue"] != null) return ViewState["DemandFastKeyValue"].ToString(); else return string.Empty; }
            set
            {
                ViewState["DemandFastKeyValue"] = value;
                this.txtContent.Value = value;
            }
        }

        /// <summary>
        /// 应用id
        /// </summary>
        public string AppID
        {
            get
            {
                if (ViewState["frm_Demand_Base_AppID"] != null)
                {
                    return ViewState["frm_Demand_Base_AppID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["frm_Demand_Base_AppID"] = value;
            }
        }

        /// <summary>
        /// 流程模型id
        /// </summary>
        public string FlowModelID
        {
            get
            {
                if (ViewState["frm_Demand_Base_FlowModelID"] != null)
                {
                    return ViewState["frm_Demand_Base_FlowModelID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["frm_Demand_Base_FlowModelID"] = value;
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
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            #region 常用类别配置项

            Catalog_DymSchameCtrList1.CatalogID = long.Parse(hidDemandTypeID.Value);

            #endregion
        }
        #endregion

        #region 提交前执行事件 Master_myPreSaveClickCustomize
        /// <summary>
        /// 提交前执行事件
        /// </summary>
        /// <returns></returns>
        bool Master_myPreSaveClickCustomize()
        {            
            Int32 intContentLen = txtContent.Value.Length;

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
            //客户
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


            //资产
            bthAddEqu.Visible = false;
            txtEqu.Visible = false;
            cmdEqu.Visible = false;

            lblEqu.Visible = true;

            //详细描述
            if (txtContent.ContralState != eOA_FlowControlState.eHidden)
                txtContent.ContralState = eOA_FlowControlState.eReadOnly;

            //扩展信息
            if (trshema.Visible != false)
            {
                Catalog_DymSchameCtrList1.ReadOnly = true;
                Catalog_DymSchameCtrList1.CatalogID = long.Parse(hidDemandTypeID.Value);
            }

        }

        #endregion

        #region 页面加载时初始化页面的值
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

            hidFlowID.Value = myFlowForms.oFlow.FlowID.ToString();

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
                #region 起草时获取需求请求模板数据赋初值
                if (Session["ExtendParameter"] != null && Session["ExtendParameter"].ToString() != string.Empty && Session["ExtendParameter"].ToString() != "0")
                {
                    string extendParameter = Session["ExtendParameter"].ToString();
                    EA_ServicesTemplateDP ee = new EA_ServicesTemplateDP();
                    ee = ee.GetReCorded(long.Parse(extendParameter));
                    KeyValue = ee.TemplateName + ":" + ee.Guide;
                    hidDemandSubject.Value = ee.TemplateName + "--" + Session["PersonName"].ToString();
                    hidReqTempID.Value = ee.IssTempID.ToString() == "" ? "0" : ee.IssTempID.ToString(); //关联的模板ID
                    txtContent.Value = KeyValue;

                    #region 根据模板ID获取模板设置的初始值

                    if (ee.IssTempID > 0)
                    {
                        EA_ShortCutTemplateDP ect = new EA_ShortCutTemplateDP();
                        ect = ect.GetReCorded(long.Parse(ee.IssTempID.ToString()));
                        FieldValues fv = new FieldValues(ect.TemplateXml);

                        if (fv.GetFieldValue("DemandTypeID") != null && fv.GetFieldValue("DemandType") != null)
                        {
                            //需求类别
                            hidDemandTypeID.Value = fv.GetFieldValue("DemandTypeID").Value == "" ? "0" : fv.GetFieldValue("DemandTypeID").Value;
                            hidDemandType.Value = fv.GetFieldValue("DemandType").Value;
                        }
                    }
                    #endregion

                }
                else
                {
                    hidDemandSubject.Value = oFlow.FlowName + "--" + Session["PersonName"].ToString();
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
                    hidListName.Value = ee.ListName.ToString();
                    hidListID.Value = ee.ListID.ToString();

                }
                #endregion
            }


            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    #region 客户资料

                    hidCustID.Value = row["CustUserID"].ToString() == "" ? "0" : row["CustUserID"].ToString();
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
                   
                    #region 资产资料

                    lblEqu.Text = row["EquipmentName"].ToString();  //资产信息
                    txtEqu.Text = row["EquipmentName"].ToString();
                    hidEquName.Value = row["EquipmentName"].ToString();
                    hidEqu.Value = row["Equipmentid"].ToString() == "" ? "0" : row["Equipmentid"].ToString();

                    hidListName.Value = row["EquipmentCatalogName"].ToString();
                    hidListID.Value = row["EquipmentCatalogID"].ToString() == "" ? "0" : row["EquipmentCatalogID"].ToString();

                    #endregion

                    #region 需求类别

                    //如果有默认初值，则设置选定的默认值
                    hidDemandTypeID.Value = row["DemandTypeID"].ToString() == "" ? "0" : row["DemandTypeID"].ToString();
                    hidDemandType.Value = row["DemandTypeName"].ToString();

                    #endregion

                    #region 需求状态
                    hidDemandStatusID.Value = row["DemandStatusID"].ToString() == "" ? "0" : row["DemandStatusID"].ToString();
                    hidDemandStatus.Value = row["DemandStatus"].ToString();
                    #endregion

                    #region 需求单号, 主题，详细描述

                    hidDemandNo.Value = row["DemandNo"].ToString();
                    hidDemandSubject.Value = row["DemandSubject"].ToString();
                    txtContent.Value = row["DemandContent"].ToString();

                    #endregion

                }
            }

            BR_ProgressBarDP pBr_ProgressBar = new BR_ProgressBarDP();
            pBr_ProgressBar = pBr_ProgressBar.GetRecorded(myFlowForms.oFlow.AppID, FlowDP.GetOFlowModelID(myFlowForms.oFlow.FlowModelID), myFlowForms.oFlow.NodeModelID);

            ShowImg.Src = pBr_ProgressBar.ImgURL;

            #endregion

            #region set visible
            setFieldCollection setFields = oFlow.setFields;
            foreach (setField sf in setFields)
            {
                if (sf.Editable == true && sf.Visibled == true)
                {
                    continue;
                }
                switch (sf.Name.ToLower())
                {
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
                    case "demandcontent":
                        txtContent.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            txtContent.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    case "exta_schema":
                        Catalog_DymSchameCtrList1.ReadOnly = true;                        
                        if (!sf.Visibled)
                        {
                            trshema.Visible = false;
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
            Catalog_DymSchameCtrList1.CatalogID = long.Parse(hidDemandTypeID.Value);

            #endregion
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

            fv.Add("RegUserID", Session["UserID"].ToString());
            fv.Add("RegUserName", Session["PersonName"].ToString());
            fv.Add("RegDeptID", Session["UserDeptID"].ToString());
            fv.Add("RegDeptName", Session["UserDeptName"].ToString());
            fv.Add("RegOrgID", Session["UserOrgID"].ToString());
            fv.Add("RegTime", DateTime.Now.ToString());

            #endregion

            #region 需求主题和详细描述

            fv.Add("DemandSubject", hidDemandSubject.Value);
            fv.Add("DemandContent", txtContent.Value.Trim());

            #endregion

            #region 需求类别

            fv.Add("DemandTypeID", hidDemandTypeID.Value == "" ? "0" : hidDemandTypeID.Value);
            fv.Add("DemandTypeName", hidDemandType.Value);

            #endregion

            #region 需求状态

            fv.Add("DemandStatusID", hidDemandStatusID.Value == "" ? "0" : hidDemandStatusID.Value);
            fv.Add("DemandStatus", hidDemandStatus.Value);

            #endregion           

            fv.Add("EmailNotify", myFlowForms.EmailValue.ToString());
            fv.Add("SMSNotify", myFlowForms.NotifyValue.ToString());

            #region 常用类别配置项

            string strCalalogSchema = GetCatalogSchema();
            fv.Add("CalalogSchema", strCalalogSchema);
            fv.Add("CalalogSchemaRelateID", Catalog_DymSchameCtrList1.RelateID.ToString());
            fv.Add("CatalogSchemaRelateType", ((int)Catalog_DymSchameCtrList1.SchemaRelateType).ToString());

            #endregion

            #region 需求模板相关
            fv.Add("ReqTempID", hidReqTempID.Value == "" ? "0" : hidReqTempID.Value);
            fv.Add("IsUseReqTempID", "0");
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
            str = Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented);

            return str;
        }

        #endregion
    }
}
