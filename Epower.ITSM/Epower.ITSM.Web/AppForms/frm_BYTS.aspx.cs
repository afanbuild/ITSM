/****************************************************************************
 * 
 * description:Ͷ������Ӧ�ñ�
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-07-31
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
using System.Drawing;

using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using appDataProcess;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;
using EpowerCom;
using EpowerGlobal;
using Epower.ITSM.Web;


namespace Epower.ITSM.Web.AppForms
{
    /// <summary>
    /// 
    /// </summary>
    public partial class frm_BYTS : BasePage
    {
        private FlowForms myFlowForms;
        RightEntity reTrace = null;  //Ȩ��

        #region ������
        /// <summary>
        /// ȡ������FlowID
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
        /// Ӧ��id
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
        /// ����ģ��id
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

        #region ҳ�Ӽ��� Page_Load
        /// <summary>
        /// ҳ�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            myFlowForms = (FlowForms)this.Master;

            myFlowForms.mySetContentReadOnly += new FlowForms.DoContentActions(Master_mySetContentVisible);
            myFlowForms.myGetFormsValue += new FlowForms.GetFormsValue(Master_myGetFormsValue);
            myFlowForms.mySetFormsValue += new FlowForms.DoContentActions(Master_mySetFormsValue);
            myFlowForms.blnSMSNotify = false;   //���ζ���֪ͨ
            //myFlowForms.blnShowFlowOP = false;  //�������̿��ƿ�
            myFlowForms.blnEmail = false;

            InitPage();

            if (txtCustAddr.Text.Trim() ==string.Empty)  //�ͻ�
            {
                txtCustAddr.Text = hidCust.Value.Trim();
            }
            if (txtBY_PersonName.Text.Trim() == string.Empty)  //��ϵ��
            {
                txtBY_PersonName.Text = hidContact.Value.Trim();
            }
            if (txtBY_ContactAddress.Text.Trim() == string.Empty)  //��ַ
            {
                txtBY_ContactAddress.Text = hidaddress.Value.Trim();
            }
            if (txtBY_ContactPhone.Text.Trim() == string.Empty)  //��ϵ�绰
            {
                txtBY_ContactPhone.Text = hidTel.Value.Trim();
            }
            if (txtBY_Email.Text.Trim() == string.Empty)  //�����ʼ�
            {
                txtBY_Email.Text = hidBY_Email.Value.Trim();
            }

            if (!IsPostBack)
            {
                if (Session["FromUrl"] != null)
                {
                    hidFormFrom.Value = Session["FromUrl"].ToString();
                }
            }
        }
        #endregion 

        #region ��ʼ��ҳ�� InitPage
        /// <summary>
        /// ��ʼ��ҳ��
        /// </summary>
        private void InitPage()
        {
            if (!IsPostBack)
            {
                txtBY_Content.Attributes.Add("onblur", "javascript:MaxLength(this,500,'�������ݳ����޶����ȣ�');");
                //txtDealContent.Attributes.Add("onblur", "javascript:MaxLength(this,500,'�������ݳ����޶����ȣ�');");
                txtBY_PersonName.Attributes["onchange"] = "TransferValue();";
            }
        }
        #endregion 

        #region ����ҳ��ֵ Master_mySetFormsValue
        /// <summary>
        /// ����ҳ��ֵ
        /// </summary>
        private void Master_mySetFormsValue()
        {
            myFlowForms.CtrButtons1.Button2Visible = true;
            myFlowForms.CtrButtons1.ButtonName2 = "֪ʶ�ο�";
            myFlowForms.CtrButtons1.Button2Function = "FormDoKmRef();";
            if (myFlowForms.oFlow.FlowStatus == e_FlowStatus.efsEnd)
            {
                myFlowForms.CtrButtons1.Button3Visible = true;
                myFlowForms.CtrButtons1.ButtonName3 = "֪ʶ�鵵";
                myFlowForms.CtrButtons1.Button3Function = "DoKmAdd(" + myFlowForms.oFlow.MessageID.ToString() + "," + myFlowForms.oFlow.AppID.ToString() + "," + myFlowForms.oFlow.FlowID.ToString() + ");";
            }


            myFlowForms.FormTitle = myFlowForms.oFlow.FlowName;
            #region Master_mySetFormsValue

            #region ��ӡʱ�������
            //��ӡʱ��Ҫ����Ĳ���
            this.FlowID = myFlowForms.oFlow.FlowID.ToString();
            this.FlowModelID = myFlowForms.oFlow.FlowModelID.ToString();
            this.AppID = myFlowForms.oFlow.AppID.ToString();
            #endregion 

            ImplDataProcess dp = new ImplDataProcess(myFlowForms.oFlow.AppID);
            DataSet ds = dp.GetFieldsDataSet(myFlowForms.oFlow.FlowID, myFlowForms.oFlow.OpID);
            DataTable dt = ds.Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    labBY_PersonName.Text = row["BY_PersonName"].ToString();
                    txtBY_PersonName.Text = row["BY_PersonName"].ToString();
                    UserPicker1.UserName = row["BY_ProjectName"].ToString();
                    UserPicker1.UserID = long.Parse(row["BY_Project"].ToString());
                    CataSource.CatelogID = long.Parse(row["BY_Soure"].ToString());
                    CataType.CatelogID = long.Parse(row["BY_Type"].ToString());
                    CataKind.CatelogID = long.Parse(row["BY_Kind"].ToString());
                    Ctr_ReceiveTime.dateTime = Convert.ToDateTime(row["BY_ReceiveTime"].ToString());
                    labBY_Email.Text = row["BY_Email"].ToString();
                    txtBY_Email.Text = row["BY_Email"].ToString();
                    labBY_Mobile.Text = row["BY_Mobile"].ToString();
                    txtBY_Mobile.Text = row["BY_Mobile"].ToString();
                    labBY_ContactPhone.Text = row["BY_ContactPhone"].ToString();
                    txtBY_ContactPhone.Text = row["BY_ContactPhone"].ToString();
                    labBY_ContactAddress.Text = row["BY_ContactAddress"].ToString();
                    txtBY_ContactAddress.Text = row["BY_ContactAddress"].ToString();
                    labBY_InformNum.Text = row["BY_InformNum"].ToString();
                    txtBY_InformNum.Text = row["BY_InformNum"].ToString();
                    labBY_Content.Text = row["BY_Content"].ToString();
                    txtBY_Content.Text = row["BY_Content"].ToString();

                    hidContact.Value = row["BY_PersonName"].ToString();
                    txtCustAddr.Text = row["CustName"].ToString();
                    hidCust.Value = row["CustName"].ToString();
                    hidCustID.Value = row["CustID"].ToString();
                    hidaddress.Value = row["BY_ContactAddress"].ToString();
                    hidTel.Value = row["BY_ContactPhone"].ToString();
                    labCustAddr.Text = row["CustName"].ToString();

                    freeTextBox1.Text = row["DealContent"].ToString();
                    lblDealContent.Text = row["DealContent"].ToString();
                }
            }
            #endregion

            #region set visible
            int iShowBase = 0;     //�Ƿ���ʾ��������
            int iShowCust = 0;    //�Ƿ���ʾ�ͻ���Ϣ
            int iShowDeal = 0;   //�Ƿ���ʾ������Ϣ
            setFieldCollection setFields = myFlowForms.oFlow.setFields;

            foreach (setField sf in setFields)
            {
                if (sf.Editable == true && sf.Visibled == true)
                {
                    if (sf.Name.ToLower() == "relevent")
                    {
                        this.trRefEvent.Visible = true;
                        LoadData();   //���ع����¼�
                        gridUndoMsg.Columns[0].HeaderText = PageDeal.GetLanguageValue("CST_Subject");
                        gridUndoMsg.Columns[1].HeaderText = PageDeal.GetLanguageValue("CST_RegUserName");
                        gridUndoMsg.Columns[2].HeaderText = PageDeal.GetLanguageValue("CST_CustTime");
                        gridUndoMsg.Columns[3].HeaderText = PageDeal.GetLanguageValue("CST_CustName");
                        gridUndoMsg.Columns[4].HeaderText = PageDeal.GetLanguageValue("CST_CustContract");
                        gridUndoMsg.Columns[5].HeaderText = PageDeal.GetLanguageValue("CST_DealStatus");                      
                        
                    }
                    continue;
                }
                switch (sf.Name.ToLower())
                {
                    case "custname":
                        txtCustAddr.Visible = false;
                        labCustAddr.Visible = false;
                        cmdCust.Visible = false;
                        if (sf.Visibled == true)
                        {
                            labCustAddr.Visible = true;
                        }
                        else
                        {
                            lnkServiceHistory.Visible = false;
                            iShowCust += 1;
                        }
                        break;
                    case "by_personname":
                        txtBY_PersonName.Visible = false;
                        rBy_PersonName.Visible = false;
                        if (sf.Visibled == true)
                        {
                            labBY_PersonName.Visible = true;
                        }
                        else
                        {
                            iShowCust += 1;
                        }
                        break;
                    case "by_email":
                        txtBY_Email.Visible = false;
                        if (sf.Visibled == true)
                        {
                            labBY_Email.Visible = true;
                        }
                        else
                        {
                            iShowCust += 1;
                        }
                        break;
                    case "by_mobile":
                        txtBY_Mobile.Visible = false;
                        if (sf.Visibled == true)
                        {
                            labBY_Mobile.Visible = true;
                        }
                        else
                        {
                            iShowCust += 1;
                        }
                        break;
                    case "by_contactphone":
                        txtBY_ContactPhone.Visible = false;
                        if (sf.Visibled == true)
                        {
                            labBY_ContactPhone.Visible = true;
                        }
                        else
                        {
                            iShowCust += 1;
                        }
                        break;
                    case "by_contactaddress":
                        txtBY_ContactAddress.Visible = false;
                        if (sf.Visibled == true)
                        {
                            labBY_ContactAddress.Visible = true;
                        }
                        else
                        {
                            iShowCust += 1;       //6
                        }
                        break;
                    case "by_project":
                        UserPicker1.VisibleText = false;
                        if (sf.Visibled == true)
                        {
                            UserPicker1.VisibleLabel = true;
                        }
                        else
                        {
                            iShowBase += 1;
                        }
                        break;
                    case "by_soure":
                        CataSource.ContralState = eOA_FlowControlState.eHidden;
                        if (sf.Visibled == true)
                        {
                            CataSource.ContralState = eOA_FlowControlState.eReadOnly;
                        }
                        else
                        {
                            iShowBase += 1;
                        }
                        break;
                    case "by_type":
                        CataType.ContralState = eOA_FlowControlState.eHidden;
                        if (sf.Visibled == true)
                        {
                            CataType.ContralState = eOA_FlowControlState.eReadOnly;
                        }
                        else
                        {
                            iShowBase += 1;
                        }
                        break;
                    case "by_receivetime":
                        Ctr_ReceiveTime.ContralState = eOA_FlowControlState.eHidden;
                        if (sf.Visibled == true)
                        {
                            Ctr_ReceiveTime.ContralState = eOA_FlowControlState.eReadOnly;
                        }
                        else
                        {
                            iShowBase += 1;
                        }
                        break;
                    case "by_informnum":
                        txtBY_InformNum.Visible = false;
                        if (sf.Visibled == true)
                        {
                            labBY_InformNum.Visible = true;
                        }
                        else
                        {
                            iShowBase += 1;
                        }
                        break;
                    case "by_content":
                        txtBY_Content.Visible = false;
                        if (sf.Visibled == true)
                        {
                            labBY_Content.Visible = true;
                        }
                        else
                        {
                            iShowBase += 1;     //6
                        }
                        break;
                    case "by_kind":
                        CataKind.ContralState = eOA_FlowControlState.eHidden;
                        if (sf.Visibled == true)
                        {
                            CataKind.ContralState = eOA_FlowControlState.eReadOnly;
                        }
                        else
                        {
                            iShowDeal += 1;
                        }
                        break;
                    case "dealcontent":
                        freeTextBox1.Visible = false;
                        lblDealContent.Visible = false;
                        if (sf.Visibled == true)
                        {
                            lblDealContent.Visible = true;
                        }
                        else
                        {
                            iShowDeal += 1;   //2
                        }
                        break;
                    case "relevent":
                        if (sf.Visibled)
                        {
                            this.trRefEvent.Visible = true;
                            LoadData();   //���ع����¼�
                            this.gridUndoMsg.Columns[12].Visible = false;
                            gridUndoMsg.Columns[0].HeaderText = PageDeal.GetLanguageValue("CST_Subject");
                            gridUndoMsg.Columns[1].HeaderText = PageDeal.GetLanguageValue("CST_RegUserName");
                            gridUndoMsg.Columns[2].HeaderText = PageDeal.GetLanguageValue("CST_CustTime");
                            gridUndoMsg.Columns[3].HeaderText = PageDeal.GetLanguageValue("CST_CustName");
                            gridUndoMsg.Columns[4].HeaderText = PageDeal.GetLanguageValue("CST_CustContract");
                            gridUndoMsg.Columns[5].HeaderText = PageDeal.GetLanguageValue("CST_DealStatus");   
                        }
                        else
                        {
                            this.trRefEvent.Visible = false;
                            iShowDeal += 1;   //3
                        }
                        break;
                    default:
                        break;

                }
            }
            if (iShowBase == 6)
            {
                Table12.Visible = false;
                Table2.Visible = false;
            }
            if (iShowCust == 6)
            {
                Table11.Visible = false;
                Table1.Visible = false;
            }
            if (iShowDeal == 3)
            {
                Table13.Visible = false;
                Table3.Visible = false;
            }
            #endregion

            #region ���üӷö���Ȩ��
            if (myFlowForms.oFlow.FlowStatus == e_FlowStatus.efsEnd)
            {
                this.ShowFeedBack.Visible = true;
            }
            else
            {
                this.ShowFeedBack.Visible = false;
            }
            //���ûط��Ƿ����¼��,����Ȩ�� ���ҷ� ��ǩ״̬ �����������
            if (CheckRight(Constant.feedbackright) == false || myFlowForms.oFlow.ActorClass == e_ActorClass.fmInfluxActor
                || myFlowForms.oFlow.FlowStatus != e_FlowStatus.efsEnd)
            {
                this.CtrFeedBack1.DealVisible = false;
            }
            else
            {
                myFlowForms.FormTitle = "Ͷ�ߵ�[�ط�]";
            }
            //���ûطÿؼ�����
            CtrFeedBack1.FlowID = myFlowForms.oFlow.FlowID;
            CtrFeedBack1.AppID = myFlowForms.oFlow.AppID;
            CtrFeedBack1.FeedBackCustomer = Session["PersonName"].ToString();

            //���ö����Ƿ����¼��,����Ȩ�� ���ҷ� ��ǩ״̬ �����������
            if (myFlowForms.oFlow.MessageID == 0)
            {
                trShowMonitor.Visible = false;   //���ö���Ϊ���ɼ�
            }
            if (CheckRight(Constant.dubanyijian) == false || myFlowForms.oFlow.ActorClass == e_ActorClass.fmInfluxActor
                || myFlowForms.oFlow.FlowStatus == e_FlowStatus.efsEnd || myFlowForms.oFlow.FlowID == 0)
            {
                this.CtrMonitor1.DealVisible = false;
            }
            //���ö�������
            CtrMonitor1.FlowID = myFlowForms.oFlow.FlowID;
            CtrMonitor1.AppID = myFlowForms.oFlow.AppID;
            #endregion 
        }
        #endregion 

        #region ����ֻ�� Master_mySetContentVisible
        /// <summary>
        /// ����ֻ��
        /// </summary>
        private void Master_mySetContentVisible()
        {
            #region Master_mySetContentVisible
            if (txtBY_PersonName.Visible == true)
                labBY_PersonName.Visible = true;
            txtBY_PersonName.Visible = false;
            rBy_PersonName.Visible = false;
            if (UserPicker1.VisibleText == true)
                UserPicker1.VisibleLabel = true;
            UserPicker1.VisibleText = false;
            if (CataSource.ContralState != eOA_FlowControlState.eHidden)
                CataSource.ContralState = eOA_FlowControlState.eReadOnly;
            if (CataType.ContralState != eOA_FlowControlState.eHidden)
                CataType.ContralState = eOA_FlowControlState.eReadOnly;
            if (CataKind.ContralState != eOA_FlowControlState.eHidden)
                CataKind.ContralState = eOA_FlowControlState.eReadOnly;
            if (Ctr_ReceiveTime.ContralState != eOA_FlowControlState.eHidden)
                //���������������Ϊֻ��
                Ctr_ReceiveTime.ContralState = eOA_FlowControlState.eReadOnly;
            if (txtBY_Email.Visible == true)
                labBY_Email.Visible = true;
            txtBY_Email.Visible = false;
            if (txtBY_Mobile.Visible == true)
                labBY_Mobile.Visible = true;
            txtBY_Mobile.Visible = false;
            if (txtBY_ContactPhone.Visible == true)
                labBY_ContactPhone.Visible = true;
            txtBY_ContactPhone.Visible = false;
            if (txtBY_ContactAddress.Visible == true)
                labBY_ContactAddress.Visible = true;
            txtBY_ContactAddress.Visible = false;
            if (txtBY_InformNum.Visible == true)
                labBY_InformNum.Visible = true;
            txtBY_InformNum.Visible = false;
            if (txtBY_Content.Visible == true)
                labBY_Content.Visible = true;
            txtBY_Content.Visible = false;
            if (txtCustAddr.Visible == true)
                labCustAddr.Visible = true;
            txtCustAddr.Visible = false;
            cmdCust.Visible = false;
            if (freeTextBox1.Visible == true)
                lblDealContent.Visible = true;
            freeTextBox1.Visible = false;

            //û�пͻ���ֵʱ ����ʾ��ʷ�ο�
            if (txtCustAddr.Text.Trim() == "" || txtCustAddr.Text.Trim() == "--")
                lnkServiceHistory.Visible = false;

            this.gridUndoMsg.Columns[12].Visible = false;  //�����¼�ֻ��
            #endregion
        }
        #endregion 

        #region ȡҳ��ֵ Master_myGetFormsValue
        /// <summary>
        /// ȡҳ��ֵ
        /// </summary>
        /// <returns></returns>
        private XmlDocument Master_myGetFormsValue(long lngActionID, string strActionName)
        {
            #region Master_myGetFormsValue
            FieldValues fv = new FieldValues();
            fv.Add("BY_PersonName", txtBY_PersonName.Text.Trim());
            fv.Add("BY_Project", UserPicker1.UserID.ToString());
            fv.Add("BY_ProjectName", UserPicker1.UserName.Trim());
            fv.Add("BY_Soure",CataSource.CatelogID.ToString() );
            fv.Add("BY_SoureName", CataSource.CatelogValue.Trim());
            fv.Add("BY_Type", CataType.CatelogID.ToString());
            fv.Add("BY_TypeName", CataType.CatelogValue.Trim());
            fv.Add("BY_ReceiveTime", Ctr_ReceiveTime.dateTime.ToString().Trim());
            fv.Add("BY_Kind", CataKind.CatelogID.ToString());
            fv.Add("BY_KindName", CataKind.CatelogValue.Trim());
            fv.Add("BY_Email", txtBY_Email.Text.Trim());
            fv.Add("BY_Mobile", txtBY_Mobile.Text.Trim());
            fv.Add("BY_ContactPhone", txtBY_ContactPhone.Text.Trim());
            fv.Add("BY_ContactAddress", txtBY_ContactAddress.Text.Trim());
            fv.Add("BY_InformNum", txtBY_InformNum.Text.Trim() == string.Empty ? "0" : txtBY_InformNum.Text.Trim());
            fv.Add("BY_Content", txtBY_Content.Text.Trim());
            fv.Add("Deleted", ((int)e_Deleted.eNormal).ToString());
            fv.Add("RegUserID", Session["UserID"].ToString());
            fv.Add("RegUserName", Session["PersonName"].ToString());
            fv.Add("RegDeptID", Session["UserDeptID"].ToString());
            fv.Add("RegDeptName", Session["UserDeptName"].ToString());
            fv.Add("RegTime", DateTime.Now.ToString());
            fv.Add("orgID", Session["UserOrgID"].ToString());
            fv.Add("custid", hidCustID.Value.Trim());
            fv.Add("custname", txtCustAddr.Text.Trim());
            fv.Add("DealContent", freeTextBox1.Text.Trim());

            fv.Add("refevent",this.hidRefEvent.Value.Trim());
            if (this.hidRefEvent.Value.Trim().ToLower() == "true")
            {
                DataTable dt = GetDetailItem();
            }
            XmlDocument xmlDoc = fv.GetXmlObject();
            #endregion
            return xmlDoc;
        }
        #endregion 

        #region ���Ȩ�� CheckRight
        /// <summary>
        /// ���Ȩ��
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

        #region �����¼�
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngNoticeID"></param>
        /// <returns></returns>
        protected string GetUrl(decimal lngFlowID)
        {
            //��ʱû�����ҳ
            string sUrl = "";
            sUrl = "javascript:window.open('../Forms/frmIssueView.aspx?NewWin=true&FlowID=" + lngFlowID.ToString() + "','','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";

            return sUrl;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridUndoMsg_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            e_FlowStatus fs;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                //��������������Ԥ�ƴ���ʱ��δ����ģ������ʾ
                fs = (e_FlowStatus)int.Parse(e.Item.Cells[8].Text);

                if (int.Parse(e.Item.Cells[9].Text.Trim()) < 0 && fs != e_FlowStatus.efsEnd)
                {
                    for (int i = 0; i < e.Item.Cells.Count; i++)
                    {
                        e.Item.Cells[i].ForeColor = Color.Red;
                    }
                }
                if (e.Item.Cells[6].Text.Trim() == "20")
                {
                    e.Item.Cells[6].Text = "���ڴ���";
                }
                else if (e.Item.Cells[6].Text.Trim() == "30")
                {
                    e.Item.Cells[6].Text = "��������";
                }
                else if (e.Item.Cells[6].Text.Trim() == "40")
                {
                    e.Item.Cells[6].Text = "������ͣ";
                }

            }
        }
        protected void gridUndoMsg_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                DataTable dt = GetDetailItem();
                dt.Rows.RemoveAt(e.Item.ItemIndex);

                gridUndoMsg.DataSource = dt.DefaultView;
                gridUndoMsg.DataBind();

                string sRefID = string.Empty;
                CreateTotle(ref sRefID);
                this.hidCustArrIDold.Value = sRefID;

                this.hidRefEvent.Value = "true";
            }
        }
        #region ȡ����ϸ����GetDetailItem
        /// <summary>
        /// ȡ����ϸ����
        /// </summary>
        /// <param name="isAll"></param>
        /// <returns></returns>
        private DataTable GetDetailItem()
        {
            DataTable dt = (DataTable)Session["RelEventItemData"];
            dt.Rows.Clear();
            int iCostID = 0;
            DataRow dr;
            foreach (DataGridItem row in gridUndoMsg.Controls[0].Controls)
            {
                iCostID++;
                if (row.ItemType == ListItemType.Item || row.ItemType == ListItemType.AlternatingItem)
                {
                    dr = dt.NewRow();
                    dr["subject"] = row.Cells[0].Text.ToString();
                    dr["RegUserName"] = row.Cells[1].Text.ToString();
                    dr["CustTime"] = row.Cells[2].Text.ToString();
                    dr["custName"] = row.Cells[3].Text.ToString();
                    dr["contact"] = row.Cells[4].Text.ToString();
                    dr["dealstatus"] = row.Cells[10].Text.ToString();
                    dr["status"] = row.Cells[11].Text.ToString();
                    dr["flowid"] = row.Cells[8].Text.ToString();
                    dr["flowdiffminute"] = row.Cells[9].Text.ToString();
                    dt.Rows.Add(dr);
                }
            }
            Session["RelEventItemData"] = dt;
            return dt;
        }
        #endregion
        #region ���� btnAdd_Click
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            LoadProblemData();
            if (hidFormFrom.Value.Trim() != string.Empty)
            {
                Session["FromUrl"] = hidFormFrom.Value;
            }
        }
        #endregion 
        #region �����������ݣ�������ԭ������ LoadProblemData
        /// <summary>
        /// ��������
        /// </summary>
        private void LoadProblemData()
        {
            string sArr = string.Empty;
            string sArrold = this.hidCustArrIDold.Value.Trim();
            string[] arr = this.hidCustArrID.Value.Trim().Split(',');
            for (int i = 0; i < arr.Length - 1; i++)
            {
                if (sArrold.IndexOf(arr[i] + ",") == -1)
                {
                    sArr += arr[i] + ",";
                    sArrold += arr[i] + ",";
                }
            }
            this.hidCustArrIDold.Value = sArrold;

            if (!string.IsNullOrEmpty(sArr))
            {
                XmlDocument xmlDoc = GetXmlValue(sArr, "-1");
                reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.CustomerService];
                DataTable dtProblem = ZHServiceDP.GetIssuesForCond(xmlDoc.InnerXml, long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString())
               , long.Parse(Session["UserOrgID"].ToString()), reTrace);

                DataTable dt = GetDetailItem();
                dt.Merge(dtProblem);
                gridUndoMsg.DataSource = dt.DefaultView;
                gridUndoMsg.DataBind();

                string sRefID = string.Empty;
                CreateTotle(ref sRefID);
                this.hidCustArrIDold.Value = sRefID;
            }
        }
        #endregion 

        #region  ���ɲ�ѯXML�ַ��� GetXmlValue
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sArr"></param>
        /// <param name="sFlowID"></param>
        /// <returns></returns>
        private XmlDocument GetXmlValue(string sArr,string sFlowID)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlRoot = xmlDoc.CreateElement("Fields");
            XmlElement xmlEle;
            #region ����FLOWID
            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "RefFlowID");
            xmlEle.SetAttribute("Value", sFlowID);
            xmlRoot.AppendChild(xmlEle);
            #endregion
            #region ��ص�FLOWID
            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "ArrFlows");
            xmlEle.SetAttribute("Value", sArr);
            xmlRoot.AppendChild(xmlEle);
             #endregion
            xmlDoc.AppendChild(xmlRoot);

            return xmlDoc;
        }
        #endregion
        #region �������� LoadData
        /// <summary>
        /// ��������
        /// </summary>
        private void LoadData()
        {
            reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.CustomerService];
            XmlDocument xmlDoc = GetXmlValue(string.Empty, myFlowForms.oFlow.FlowID.ToString());
            DataTable dt = ZHServiceDP.GetIssuesForCond(xmlDoc.InnerXml, long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString())
           , long.Parse(Session["UserOrgID"].ToString()), reTrace);
            Session["RelEventItemData"] = dt;
            gridUndoMsg.DataSource = dt.DefaultView;
            gridUndoMsg.DataBind();

            string sRefID = string.Empty;
            CreateTotle(ref sRefID);
            this.hidCustArrIDold.Value = sRefID;
        }
        #endregion

        #region �����ܵ�REFID
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private void CreateTotle(ref string sArrRefID)
        {
            foreach (DataGridItem row in gridUndoMsg.Items)
            {
                sArrRefID += row.Cells[8].Text.Trim() + ",";
            }
        }
        #endregion
        #endregion 
    }
}
