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


namespace Epower.ITSM.Web.InformationManager
{
    public partial class frm_KBBase : BasePage
    {
        #region 变量区


        long lngFromFlowID = 0; //归档来源的流程实例ID

        /// <summary>
        /// 母版页
        /// </summary>
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
                if (ViewState["frm_KBBase_FlowID"] != null)
                {
                    return ViewState["frm_KBBase_FlowID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["frm_KBBase_FlowID"] = value;
            }
        }

        /// <summary>
        /// 应用id
        /// </summary>
        public string AppID
        {
            get
            {
                if (ViewState["frm_KBBase_AppID"] != null)
                {
                    return ViewState["frm_KBBase_AppID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["frm_KBBase_AppID"] = value;
            }
        }
        /// <summary>
        /// 流程模型id
        /// </summary>
        public string FlowModelID
        {
            get
            {
                if (ViewState["frm_KBBase_FlowModelID"] != null)
                {
                    return ViewState["frm_KBBase_FlowModelID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["frm_KBBase_FlowModelID"] = value;
            }
        }
        /// <summary>
        /// 取得流程MessageID
        /// </summary>
        public string MessageID
        {
            get
            {
                if (ViewState["frm_KBBase_MessageID"] != null)
                {
                    return ViewState["frm_KBBase_MessageID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["frm_KBBase_MessageID"] = value;
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

        #region Page_Load

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            myFlowForms = (FlowForms)this.Master;
            myFlowForms.mySetContentReadOnly += new FlowForms.DoContentActions(Master_mySetContentVisible);  //设置页面只读
            myFlowForms.myGetFormsValue += new FlowForms.GetFormsValue(Master_myGetFormsValue);              //取得页面值
            myFlowForms.mySetFormsValue += new FlowForms.DoContentActions(Master_mySetFormsValue);           //设置页面值
            myFlowForms.myPreClickCustomize += new FlowForms.DoContentSubmitValid(Master_myPreClickCustomize);

            myFlowForms.blnSMSNotify = true;                                                                //屏蔽短信通知                                                              //屏蔽流程控制框
            myFlowForms.blnEmail = true;
            myFlowForms.blnShowFlowOP = true;
            InitPage();  //初始化页面数据

            if (Page.IsPostBack == true)
            {
                lngFromFlowID = long.Parse(ViewState["frm_KBBase_FromFlowID"].ToString());
            }
            else
            {
                PageDeal.SetLanguage(this.Controls[0].Controls[1]);
            }
        }

        #endregion

        #region 初始化页面数据 InitPage
        /// <summary>
        /// 初始化页面数据
        /// </summary>
        private void InitPage()
        {
            txtTitle.Attributes["onchange"] = "TransferValue();";
            txtTitle.Attributes.Add("onblur", "javascript:MaxLength(this,50,'输入内容超出限定长度：');");
            txtPKey.Attributes.Add("onblur", "javascript:MaxLength(this,50,'输入内容超出限定长度：');");            
        }
        #endregion

        #region 设置页面值 Master_mySetFormsValue

        private void SetFromFlowValues(objFlow of)
        {
            string strTable = "";
            //根据应用ID 得到对应的表名
            switch (of.AppID)
            { 
                case 1026:  //事件管理
                    strTable = "cst_issues";
                    break;
                case 210:   //问题管理
                    strTable = "pro_problemdeal";
                    break;
                case 420:   //变更管理
                    strTable = "equ_changeservice";
                    break;
            }

            if (strTable.Trim() == string.Empty)
                return;
            //根据应用ID查询知识转移配置数据信息
            DataTable dtTransfer = Inf_transfer_setDP.GetDataTable(of.AppID, " order by ID ");
            if (dtTransfer == null || dtTransfer.Rows.Count <= 0)
                return;

            //根据流程ID 和得到的表名查询对应的数据信息
            DataTable dt = CommonDP.GetDataByFlowIDandTableName(of.FlowID, strTable);            

            //根据得到的数据信息给页面赋初值
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dtTransfer.Rows)
                {
                    string strValue = dt.Rows[0][dr["FLOWFIELD"].ToString()] == null ? "" : dt.Rows[0][dr["FLOWFIELD"].ToString()].ToString();

                    switch (dr["INFOFIELD"].ToString().ToLower())
                    { 
                        case "title":   //主题                            
                            txtTitle.Text = txtTitle.Text + strValue + " ";
                            labTitle.Text = txtTitle.Text;
                            break;
                        case "pkey":    //关键字
                            txtPKey.Text = txtPKey.Text + strValue + " ";
                            labPKey.Text = txtPKey.Text;
                            break;
                        case "tags":    //摘要
                            txtTags.Value = txtTags.Value + strValue + " ";
                            break;
                        case "content": //知识内容
                            UEditor1.Content = UEditor1.Content + strValue + " ";
                            lblContent.Text = UEditor1.Content;
                            break;
                    }
                }
            }            
        }

        /// <summary>
        /// 设置页面值
        /// </summary>
        private void Master_mySetFormsValue()
        {
            myFlowForms.FormTitle = "知识审批管理";
            this.FlowID = myFlowForms.oFlow.FlowID.ToString();
            this.MessageID = myFlowForms.oFlow.MessageID.ToString();
            this.AppID = myFlowForms.oFlow.AppID.ToString();
            this.FlowModelID = myFlowForms.oFlow.FlowModelID.ToString();

            #region Master_mySetFormsValue
            ImplDataProcess dp = new ImplDataProcess(myFlowForms.oFlow.AppID);
            DataSet ds = dp.GetFieldsDataSet(myFlowForms.oFlow.FlowID, myFlowForms.oFlow.OpID);
            DataTable dt = ds.Tables[0];


            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    #region 标题

                    labTitle.Text = row["Title"].ToString();
                    txtTitle.Text = row["Title"].ToString();

                    #endregion

                    #region 关键字

                    labPKey.Text = row["PKey"].ToString();
                    txtPKey.Text = row["PKey"].ToString();

                    #endregion

                    #region 知识类别

                    CtrKBType.CatelogID = decimal.Parse(row["Type"].ToString());

                    #endregion

                    #region 内容

                    lblContent.Text = row["Content"].ToString();
                    UEditor1.Content = row["Content"].ToString();

                    #endregion

                    #region 摘要

                    txtTags.Value = row["Tags"].ToString();                    

                    #endregion

                    #region 资产目录

                    lblListName.Text = row["ListName"].ToString();//资产目录名称
                    txtListName.Text = row["ListName"].ToString();//资产目录名称
                    hidListName.Value = row["ListName"].ToString();//资产目录名称
                    hidListID.Value = row["ListID"].ToString();//资产目录ID

                    #endregion

                    #region 资产

                    lblEqu.Text = row["EquName"].ToString();//资产名称
                    txtEqu.Text = row["EquName"].ToString();//资产名称
                    hidEquName.Value = row["EquName"].ToString();//资名称
                    hidEqu.Value = row["EquID"].ToString();//资产ID

                    #endregion

                    #region 是否入库

                    if (row["isinkb"].ToString() == "0")
                    {
                        chkIsInKB.Checked = false;
                    }
                    else
                    {
                        chkIsInKB.Checked = true;
                    }

                    #endregion

                }
                else
                {
                    if (Session["ExtendParameter"] != null && Session["ExtendParameter"].ToString() != string.Empty && Session["ExtendParameter"].ToString() != "0")
                    {
                        //通过获取
                        long lngOriMessageID = long.Parse(Session["ExtendParameter"].ToString());
                        objFlow oldFlow = new objFlow((long)Session["UserID"], 0, lngOriMessageID);

                        SetFromFlowValues(oldFlow);

                        lngFromFlowID = oldFlow.FlowID;

                        myFlowForms.TempFlowID = lngFromFlowID;

                    }
                }

            }


            ViewState["frm_KBBase_FromFlowID"] = lngFromFlowID;

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
                    case "title":
                        txtTitle.Visible = false;
                        rTitle.Visible = false;
                        if (sf.Visibled == true)
                            labTitle.Visible = true;
                        break;
                    case "pkey":
                        txtPKey.Visible = false;
                        if (sf.Visibled == true)
                            labPKey.Visible = true;
                        break;
                    case "typename":
                        CtrKBType.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrKBType.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    case "content":
                        UEditor1.Visible = false;
                        lblContent.Visible = false;
                        rWarning.Visible = false;
                        if (sf.Visibled)
                            lblContent.Visible = true;
                        
                        break;
                    case "isinkb":
                        chkIsInKB.Visible = false;
                        if (sf.Visibled == true)
                        {
                            chkIsInKB.Enabled = false;
                            chkIsInKB.Visible = true;
                        }
                        break;
                    case "tags":
                        txtTags.ContralState = eOA_FlowControlState.eReadOnly;
                        if (!sf.Visibled)
                            txtTags.ContralState = eOA_FlowControlState.eHidden;
                        break;

                    case "listname":
                        txtListName.Visible = false;
                        cmdListName.Visible = false;
                        if (sf.Visibled == true)
                            lblListName.Visible = true;
                        break;
                    case "equname":
                        txtEqu.Visible = false;
                        cmdEqu.Visible = false;
                        if (sf.Visibled == true)
                            lblEqu.Visible = true;
                        break;
                    default:
                        break;

                }
            }
            #endregion
        }
        #endregion

        #region 设置页面只读 Master_mySetContentVisible
        /// <summary>
        /// 设置页面只读
        /// </summary>
        private void Master_mySetContentVisible()
        {
            #region Master_mySetContentVisible

            if (CtrKBType.ContralState != eOA_FlowControlState.eHidden)
                CtrKBType.ContralState = eOA_FlowControlState.eReadOnly;

            if (txtTitle.Visible == true)
                labTitle.Visible = true;
            txtTitle.Visible = false;
            rTitle.Visible = false;

            //UEditor1.UEditorReadOnly = true;
            

            if (UEditor1.Visible)
            {
                UEditor1.Visible = false;
                lblContent.Visible = true;
            }

            rWarning.Visible = false;            
            

            if (txtPKey.Visible == true)
                labPKey.Visible = true;
            txtPKey.Visible = false;

            if (txtTags.ContralState != eOA_FlowControlState.eHidden)
                txtTags.ContralState = eOA_FlowControlState.eReadOnly;

            if (txtListName.Visible == true)
                lblListName.Visible = true;
            txtListName.Visible = false;
            cmdListName.Visible = false;

            if (txtEqu.Visible == true)
                lblEqu.Visible = true;
            txtEqu.Visible = false;
            cmdEqu.Visible = false;

            chkIsInKB.Enabled = false;

            #endregion
        }
        #endregion

        #region 取得页面值 Master_myGetFormsValue
        /// <summary>
        /// 取得页面值
        /// </summary>
        /// <param name="lngActionID"></param>
        /// <param name="strActionName"></param>
        /// <returns></returns>
        private XmlDocument Master_myGetFormsValue(long lngActionID, string strActionName)
        {
            #region Master_myGetFormsValue

            FieldValues fv = new FieldValues();
            fv.Add("Title", txtTitle.Text.Trim());
            fv.Add("Pkey", txtPKey.Text.Trim());
            fv.Add("ListID", hidListID.Value.Trim());
            fv.Add("ListName", hidListName.Value.Trim());
            fv.Add("EquId", hidEqu.Value.Trim());
            fv.Add("EquName", hidEquName.Value.Trim());
            fv.Add("Type", CtrKBType.CatelogID.ToString());
            fv.Add("TypeName", CtrKBType.CatelogValue.Trim());
            fv.Add("Content", UEditor1.Content.Trim());
            fv.Add("isinkb", (chkIsInKB.Checked == true ? "1" : "0"));
            fv.Add("reguser", Session["UserID"].ToString());
            fv.Add("regusername", Session["PersonName"].ToString());
            fv.Add("preflowid", lngFromFlowID.ToString());
            fv.Add("RegTime", DateTime.Now.ToString());
            fv.Add("DeptID", Session["UserDeptID"].ToString());
            fv.Add("DeptName", Session["UserDeptName"].ToString());
            fv.Add("tags", txtTags.Value.Trim());
            fv.Add("EmailNotify", myFlowForms.EmailValue.ToString());
            fv.Add("SMSNotify", myFlowForms.NotifyValue.ToString());

            XmlDocument xmlDoc = fv.GetXmlObject();
            #endregion
            return xmlDoc;
        }
        #endregion
        
        #region 提交时前面执行事件 Master_myPreClickCustomize
        /// <summary>
        /// 提交时前面执行事件
        /// </summary>
        /// <returns></returns>
        bool Master_myPreClickCustomize(long lngActionID, string strActionName)
        {
            #region 判断知识内容是否为空
            if (UEditor1.Content.Trim() == string.Empty)
            {
                PageTool.MsgBox(this, "知识内容不能为空！");
                return false;
            }
            #endregion

            return true;
        }
        #endregion
    }
}
