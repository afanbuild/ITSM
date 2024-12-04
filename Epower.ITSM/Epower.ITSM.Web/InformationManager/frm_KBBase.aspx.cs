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
        #region ������


        long lngFromFlowID = 0; //�鵵��Դ������ʵ��ID

        /// <summary>
        /// ĸ��ҳ
        /// </summary>
        private FlowForms myFlowForms;
        #endregion

        #region ����

        /// <summary>
        /// ȡ������FlowID
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
        /// Ӧ��id
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
        /// ����ģ��id
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
        /// ȡ������MessageID
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
        /// ��ӡ��ʽ 0 IE��ʽ��1 Report Service��ʽ
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
        /// ҳ�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            myFlowForms = (FlowForms)this.Master;
            myFlowForms.mySetContentReadOnly += new FlowForms.DoContentActions(Master_mySetContentVisible);  //����ҳ��ֻ��
            myFlowForms.myGetFormsValue += new FlowForms.GetFormsValue(Master_myGetFormsValue);              //ȡ��ҳ��ֵ
            myFlowForms.mySetFormsValue += new FlowForms.DoContentActions(Master_mySetFormsValue);           //����ҳ��ֵ
            myFlowForms.myPreClickCustomize += new FlowForms.DoContentSubmitValid(Master_myPreClickCustomize);

            myFlowForms.blnSMSNotify = true;                                                                //���ζ���֪ͨ                                                              //�������̿��ƿ�
            myFlowForms.blnEmail = true;
            myFlowForms.blnShowFlowOP = true;
            InitPage();  //��ʼ��ҳ������

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

        #region ��ʼ��ҳ������ InitPage
        /// <summary>
        /// ��ʼ��ҳ������
        /// </summary>
        private void InitPage()
        {
            txtTitle.Attributes["onchange"] = "TransferValue();";
            txtTitle.Attributes.Add("onblur", "javascript:MaxLength(this,50,'�������ݳ����޶����ȣ�');");
            txtPKey.Attributes.Add("onblur", "javascript:MaxLength(this,50,'�������ݳ����޶����ȣ�');");            
        }
        #endregion

        #region ����ҳ��ֵ Master_mySetFormsValue

        private void SetFromFlowValues(objFlow of)
        {
            string strTable = "";
            //����Ӧ��ID �õ���Ӧ�ı���
            switch (of.AppID)
            { 
                case 1026:  //�¼�����
                    strTable = "cst_issues";
                    break;
                case 210:   //�������
                    strTable = "pro_problemdeal";
                    break;
                case 420:   //�������
                    strTable = "equ_changeservice";
                    break;
            }

            if (strTable.Trim() == string.Empty)
                return;
            //����Ӧ��ID��ѯ֪ʶת������������Ϣ
            DataTable dtTransfer = Inf_transfer_setDP.GetDataTable(of.AppID, " order by ID ");
            if (dtTransfer == null || dtTransfer.Rows.Count <= 0)
                return;

            //��������ID �͵õ��ı�����ѯ��Ӧ��������Ϣ
            DataTable dt = CommonDP.GetDataByFlowIDandTableName(of.FlowID, strTable);            

            //���ݵõ���������Ϣ��ҳ�渳��ֵ
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dtTransfer.Rows)
                {
                    string strValue = dt.Rows[0][dr["FLOWFIELD"].ToString()] == null ? "" : dt.Rows[0][dr["FLOWFIELD"].ToString()].ToString();

                    switch (dr["INFOFIELD"].ToString().ToLower())
                    { 
                        case "title":   //����                            
                            txtTitle.Text = txtTitle.Text + strValue + " ";
                            labTitle.Text = txtTitle.Text;
                            break;
                        case "pkey":    //�ؼ���
                            txtPKey.Text = txtPKey.Text + strValue + " ";
                            labPKey.Text = txtPKey.Text;
                            break;
                        case "tags":    //ժҪ
                            txtTags.Value = txtTags.Value + strValue + " ";
                            break;
                        case "content": //֪ʶ����
                            UEditor1.Content = UEditor1.Content + strValue + " ";
                            lblContent.Text = UEditor1.Content;
                            break;
                    }
                }
            }            
        }

        /// <summary>
        /// ����ҳ��ֵ
        /// </summary>
        private void Master_mySetFormsValue()
        {
            myFlowForms.FormTitle = "֪ʶ��������";
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

                    #region ����

                    labTitle.Text = row["Title"].ToString();
                    txtTitle.Text = row["Title"].ToString();

                    #endregion

                    #region �ؼ���

                    labPKey.Text = row["PKey"].ToString();
                    txtPKey.Text = row["PKey"].ToString();

                    #endregion

                    #region ֪ʶ���

                    CtrKBType.CatelogID = decimal.Parse(row["Type"].ToString());

                    #endregion

                    #region ����

                    lblContent.Text = row["Content"].ToString();
                    UEditor1.Content = row["Content"].ToString();

                    #endregion

                    #region ժҪ

                    txtTags.Value = row["Tags"].ToString();                    

                    #endregion

                    #region �ʲ�Ŀ¼

                    lblListName.Text = row["ListName"].ToString();//�ʲ�Ŀ¼����
                    txtListName.Text = row["ListName"].ToString();//�ʲ�Ŀ¼����
                    hidListName.Value = row["ListName"].ToString();//�ʲ�Ŀ¼����
                    hidListID.Value = row["ListID"].ToString();//�ʲ�Ŀ¼ID

                    #endregion

                    #region �ʲ�

                    lblEqu.Text = row["EquName"].ToString();//�ʲ�����
                    txtEqu.Text = row["EquName"].ToString();//�ʲ�����
                    hidEquName.Value = row["EquName"].ToString();//������
                    hidEqu.Value = row["EquID"].ToString();//�ʲ�ID

                    #endregion

                    #region �Ƿ����

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
                        //ͨ����ȡ
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

        #region ����ҳ��ֻ�� Master_mySetContentVisible
        /// <summary>
        /// ����ҳ��ֻ��
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

        #region ȡ��ҳ��ֵ Master_myGetFormsValue
        /// <summary>
        /// ȡ��ҳ��ֵ
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
        
        #region �ύʱǰ��ִ���¼� Master_myPreClickCustomize
        /// <summary>
        /// �ύʱǰ��ִ���¼�
        /// </summary>
        /// <returns></returns>
        bool Master_myPreClickCustomize(long lngActionID, string strActionName)
        {
            #region �ж�֪ʶ�����Ƿ�Ϊ��
            if (UEditor1.Content.Trim() == string.Empty)
            {
                PageTool.MsgBox(this, "֪ʶ���ݲ���Ϊ�գ�");
                return false;
            }
            #endregion

            return true;
        }
        #endregion
    }
}
