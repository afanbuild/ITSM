using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using System.Text;
using EpowerCom;
using Epower.ITSM.SqlDAL.MailAndMessageRule;
using Epower.ITSM.Web.Controls;

namespace Epower.ITSM.Web.AppForms
{
    /// <summary>
    /// MailMessageTemEdit ��ժҪ˵����
    /// </summary>
    public partial class MailMessageRuleEdit : BasePage
    {
        HtmlInputHidden TriggerType = new HtmlInputHidden();
        HtmlInputHidden IntervalTime = new HtmlInputHidden();
        HtmlInputHidden hidRecipient_User = new HtmlInputHidden();
        HtmlInputHidden hidRecipient_UserID = new HtmlInputHidden();
        private FlowForms myFlowForms;
        
        #region SetParentButtonEvent
        /// <summary>
        /// ���ø����尴ť�¼�
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.CustumManager;
           
            this.Master.IsCheckRight = true;
            if (this.Request.QueryString["id"] != null)
            {
                this.Master.MainID = this.Request.QueryString["id"].ToString();
            }

            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);
            this.Master.Master_Button_Save_Click += new Global_BtnClick(Master_Master_Button_Save_Click);
            this.Master.Master_Button_GoHistory_Click += new Global_BtnClick(Master_Master_Button_GoHistory_Click);

            this.Master.setButtonRigth(Constant.SystemManager, true);
        }

        #endregion


       

        #region Master_Master_Button_New_Click

        /// <summary>
        /// ����
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            if (string.IsNullOrEmpty(this.Master.MainID.Trim()))
                Response.Redirect("MailMessageRuleEdit.aspx?QuickNew=1");
            else
                Response.Redirect("MailMessageRuleEdit.aspx");
        }

        #endregion

        #region Master_Master_Button_Delete_Click

        /// <summary>
        /// ɾ��
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            if (!String.IsNullOrEmpty(this.Master.MainID.ToString()))
            {
                //===========zxl==
                BR_MessageRulInstallDP br_messageRulDP = new BR_MessageRulInstallDP();
                br_messageRulDP.DeletedBRMessageTall(long.Parse(cboFlowModel.SelectedValue.ToString()));
                MailAndMessageRuleDP ee = new MailAndMessageRuleDP();
                ee.DeleteRecorded(long.Parse(this.Master.MainID.Trim()));                
                //=========zxl==
                //������ҳ��
                Master_Master_Button_GoHistory_Click();
            }
        }

        #endregion

        #region Master_Master_Button_Save_Click

        /// <summary>
        /// ����
        /// </summary>
        void Master_Master_Button_Save_Click()
        {
            if (cboFlowModel.SelectedItem.Text != "")
            {
                #region �ж�ĳ������ģ�����Ƿ��Ѿ����ڹ��� yxq 2011-09-05
                string swhere = " and id !=" + (this.Master.MainID.Trim() == "" ? "0" : this.Master.MainID.Trim()) + " and ModelID = " + cboFlowModel.SelectedItem.Value;
                if (MailAndMessageRuleDP.IsExistsByModel(swhere))
                {
                    PageTool.MsgBox(this, "������ģ�����Ѿ����ڶ����ʼ�����!");
                    this.Master.IsSaveSuccess = false;
                    return;
                }
                #endregion

                string result = string.Empty;
                if (string.IsNullOrEmpty(this.Master.MainID.Trim()))
                {
                    MailAndMessageRuleDP ee = new MailAndMessageRuleDP();
                    InitObject(ee);
                    ee.Deleted = (int)eRecord_Status.eNormal;
                    ee.RegUserID = long.Parse(Session["UserID"].ToString());
                    ee.RegUserName = Session["PersonName"].ToString();
                    ee.RegDeptID = long.Parse(Session["UserDeptID"].ToString());
                    ee.RegDeptName = Session["UserDeptName"].ToString();
                    ee.RegTime = DateTime.Now;
                    ee.InsertRecorded(ee);
                    this.Master.MainID = ee.ID.ToString().Trim();

                    #region �����ʼ�����
                    BR_MessageRulInstallDP ee2 = new BR_MessageRulInstallDP();
                    InitMessRulDP(ee2);
                    #endregion
                   
                }
                else
                {
                    MailAndMessageRuleDP ee = new MailAndMessageRuleDP();
                    ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                    InitObject(ee);
                    ee.UpdateRecorded(ee);
                    this.Master.MainID = ee.ID.ToString();

                    #region  �����ʼ�����
                    BR_MessageRulInstallDP ee2 = new BR_MessageRulInstallDP();
                    InitMessRulDP(ee2);
                    #endregion
                }
            }
            else
            {
                PageTool.MsgBox(this, " ��ѡ������ģ��");
            }

        }

        #endregion

        #region Master_Master_Button_GoHistory_Click

        /// <summary>
        /// ����
        /// </summary>
        void Master_Master_Button_GoHistory_Click()
        {
            Response.Redirect("MailAndMessageRuleManager.aspx");

        }

        #endregion

        #region Page_Load

        /// <summary>
        /// ҳ�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            //������ҳ��
            SetParentButtonEvent();
           
            if (!IsPostBack)
            {
                ddlBind();
                //��������
                LoadData();
            }

            #region �����Ѿ���ӣ����ñ��水ť��ֱ���ʼ�������������ť�󣬿���
            //string RuleName = CtrRuleName.Value.ToString();//��������
            //string TName = txtTName.Text.ToString();//����ģ��
            //if (RuleName != "" && TName != "")
            //{
            //    this.Master.Btn_save.Enabled = false;//����
            //}
            #endregion
        }

        #endregion

        #region LoadData


        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            if (!string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                MailAndMessageRuleDP ee = new MailAndMessageRuleDP();
                ee = ee.GetReCorded(long.Parse(this.Master.MainID));
                LoadData(ee);
            }
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="ee"></param>
        private void LoadData(MailAndMessageRuleDP ee)
        {
            CtrRuleName.Value = ee.RuleName;
            txtTName.Text = ee.TemplateName;//ģ������
            hidTNid.Value = ee.TemplateID.ToString();
            hidTNname.Value = ee.TemplateName.ToString();
            ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(ee.Status.ToString()));
            cboApp.SelectedIndex = cboApp.Items.IndexOf(cboApp.Items.FindByValue(ee.SystemID.ToString()));

            #region  ��������
            if (cboApp.SelectedItem.Value != "-1")
            {

                BindFlowModel();  //������ģ��


                ddlReceiversType.Items.Clear();


                if (cboApp.SelectedItem.Value == "1026" || cboApp.SelectedItem.Value == "420")
                {
                    ddlReceiversType.Items.Insert(0, new ListItem("", "0"));
                    ddlReceiversType.Items.Insert(1, new ListItem("�ͻ�", "1"));
                    ddlReceiversType.Items.Insert(2, new ListItem("������", "2"));
                    ddlReceiversType.Items.Insert(3, new ListItem("�ͻ��ʹ�����", "3"));
                }
                else
                {
                    ddlReceiversType.Items.Insert(0, new ListItem("", "0"));
                    ddlReceiversType.Items.Insert(1, new ListItem("������", "2"));
                }
            }
            #endregion

            cboFlowModel.SelectedIndex = cboFlowModel.Items.IndexOf(cboFlowModel.Items.FindByValue(ee.ModelID.ToString()));
            CtrFlowMailTitle.Value = ee.MailTitle;
            ddlReceiversType.SelectedIndex = ddlReceiversType.Items.IndexOf(ddlReceiversType.Items.FindByValue(ee.ReceiversTypeID.ToString()));
            ddlSenderType.SelectedIndex = ddlSenderType.Items.IndexOf(ddlSenderType.Items.FindByValue(ee.SenderTypeID.ToString()));
            UserPickerMult1.UserID = ee.RUserIDList;
            UserPickerMult1.UserName = ee.RUserNameList;
            CtrFlowTimeCount.Value = ee.TimeCount.ToString();
            txtRemark.Text = ee.Remark;//��ע


            #region �������� ������
            DataTable dt = new DataTable();

            long appid = long.Parse(cboApp.SelectedValue.ToString() == "" ? "0" : cboApp.SelectedValue.ToString());
            long modelid = long.Parse(cboFlowModel.SelectedValue.ToString() == "" ? "0" : cboFlowModel.SelectedValue.ToString());
            dt = ee.GetMessageRulInstall(appid, modelid);
            dgCondition.DataSource = dt.DefaultView;
            dgCondition.DataBind();
            #endregion
        }

        #endregion

        #region dllBind

        private void ddlBind()
        {
            cboApp.DataSource = epApp.GetAllApps().DefaultView;
            cboApp.DataTextField = "AppName";
            cboApp.DataValueField = "AppID";
            cboApp.DataBind();

            //cboApp.Items.Remove(new ListItem("ͨ������", "199"));
            cboApp.Items.Remove(new ListItem("����������", "1027"));

            ListItem itm = new ListItem("", "-1");
            cboApp.Items.Insert(0, itm);
            cboApp.SelectedIndex = 0;

            ListItem itm1 = new ListItem("", "-1");
            cboFlowModel.Items.Insert(0, itm1);
            cboFlowModel.SelectedIndex = 0;
        }

        #endregion

        #region InitObject

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ee"></param>
        private void InitObject(MailAndMessageRuleDP ee)
        {
            ee.RuleName = CtrRuleName.Value;
            ee.TemplateName = txtTName.Text.Trim().ToString();//ģ������
            ee.TemplateID = long.Parse(hidTNid.Value.ToString() == "" ? "0" : hidTNid.Value.ToString());
            ee.Status = int.Parse(ddlStatus.SelectedItem.Value);
            ee.SystemID = long.Parse(cboApp.SelectedItem.Value);
            ee.SystemName = cboApp.SelectedItem.Text;

            ee.ModelID = long.Parse(cboFlowModel.SelectedItem.Value);
            ee.ModelName = cboFlowModel.SelectedItem.Text;
            ee.MailTitle = CtrFlowMailTitle.Value;
            ee.ReceiversTypeID = long.Parse(ddlReceiversType.SelectedItem.Value);
            ee.ReceiversTypeName = ddlReceiversType.SelectedItem.Text;
            ee.SenderTypeID = long.Parse(ddlSenderType.SelectedItem.Value);
            ee.SenderTypeName = ddlSenderType.SelectedItem.Text;
            ee.RUserIDList = UserPickerMult1.UserID;
            ee.RUserNameList = UserPickerMult1.UserName;
            ee.TimeCount = int.Parse(CtrFlowTimeCount.Value == "" ? "0" : CtrFlowTimeCount.Value);
            ee.Remark = txtRemark.Text.Trim().ToString();//��ע
        }

        #endregion

        #region InitMessRulDP
        private void InitMessRulDP(BR_MessageRulInstallDP ee)
        {
            string str = "";

            //ɾ��������ģ�������е��ʼ�����
            BR_MessageRulInstallDP br_messageRulDP = new BR_MessageRulInstallDP();
            br_messageRulDP.DeletedBRMessageTall(long.Parse(cboFlowModel.SelectedValue.ToString()));

            //ѭ�������µ��ʼ�����
            bool isok=true;
            DataTable dt = GetDetailItem(true, 0, ref str);


                foreach (DataRow row in dt.Rows)
                {
                    ee.OFlowModelID = long.Parse(cboFlowModel.SelectedValue.ToString());
                    ee.NodeID = long.Parse(row[2].ToString());
                    ee.NodeName = row[3].ToString();
                    ee.NodeContent = row[4].ToString();
                    ee.FlowNameID = long.Parse(row[5].ToString());
                    ee.FlowName = row[6].ToString();
                    ee.ReceiverStypeName = row[7].ToString();
                    ee.Trigger_Type = row[8].ToString();
                    ee.Interval_time = row[9].ToString();
                    ee.Recipient_User = row[10].ToString();
                    ee.Recipient_UserID = row[11].ToString();

                    string swhere = " and OFlowModelId=" + ee.OFlowModelID + " and NodeId=" + ee.NodeID;
                    ee.InsertBRMessageTall(ee);
                }
         
        }
        #endregion

        #region ddlChanged

        protected void cboApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindFlowModel();

         

            ListItem itm1 = new ListItem("", "-1");
            cboFlowModel.Items.Insert(0, itm1);
            cboFlowModel.SelectedIndex = 0;
            ddlReceiversType.Items.Clear();
            if (cboApp.SelectedItem.Value == "1026" || cboApp.SelectedItem.Value == "420")
            {
                ddlReceiversType.Items.Insert(0, new ListItem("", "0"));
                ddlReceiversType.Items.Insert(1, new ListItem("�ͻ�", "1"));
                ddlReceiversType.Items.Insert(2, new ListItem("������", "2"));
                ddlReceiversType.Items.Insert(3, new ListItem("�ͻ��ʹ�����", "3"));
            }
            else
            {
                ddlReceiversType.Items.Insert(0, new ListItem("", "0"));
                ddlReceiversType.Items.Insert(1, new ListItem("������", "2"));
            }
        }

        #endregion

        #region  ������ģ��
        private void BindFlowModel()
        {
            string stWhere = cboApp.SelectedItem.Value == "-1" ? "" : " and AppID=" + cboApp.SelectedItem.Value;
            stWhere = stWhere + " and status=1 and deleted=0 ";
            cboFlowModel.DataSource = MailAndMessageRuleDP.getAllFlowModel(stWhere).DefaultView;
            cboFlowModel.DataTextField = "flowname";
            cboFlowModel.DataValueField = "oflowmodelid";
            cboFlowModel.DataBind();

        }
        #endregion

        #region cboFlowModel ����ģ��������ı��¼�
        protected void cboFlowModel_SelectedIndexChanged(object sender, EventArgs e)
        {

            long i = cboApp.SelectedIndex;
            MailAndMessageRuleDP mRulDP = new MailAndMessageRuleDP();
            DataTable dt = new DataTable();

            long appid = long.Parse(cboApp.SelectedValue.ToString() == "" ? "0" : cboApp.SelectedValue.ToString());
            long modelid = long.Parse(cboFlowModel.SelectedValue.ToString() == "" ? "0" : cboFlowModel.SelectedValue.ToString());
            dt = mRulDP.GetMessageRulInstall(appid, modelid);
            dgCondition.DataSource = dt.DefaultView;
            dgCondition.DataBind();

        }
        #endregion

        /// <summary>
        /// �������ã�������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgCondition_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            MailAndMessageRuleDP mRulDP = new MailAndMessageRuleDP();

            if (e.Item.ItemType == ListItemType.Footer)
            {
                //ȡ��������
                long appid = long.Parse(cboApp.SelectedValue.ToString());
                long flowmodeid = long.Parse(cboFlowModel.SelectedValue.ToString());
                DropDownList dlNodeName = (DropDownList)e.Item.FindControl("drpAddNodeName");

                DataTable dt = mRulDP.GetNodeName(appid, flowmodeid);
                DataView dv = dt.DefaultView;

                dlNodeName.DataSource = dv;
                dlNodeName.DataTextField = "nodename";
                dlNodeName.DataValueField = "NodeModelID";
                dlNodeName.DataBind();
                dlNodeName.Items.Insert(0, new ListItem("", ""));//add
                dlNodeName.Items.Insert(1, new ListItem("���л���", "-1"));

                HtmlInputHidden hdNodeID = (HtmlInputHidden)e.Item.FindControl("HidAddNodeId");
                dlNodeName.SelectedIndex = dlNodeName.Items.IndexOf(dlNodeName.Items.FindByValue(hdNodeID.Value));


                #region ȡ��������
                DropDownList dlAddReceiversType = (DropDownList)e.Item.FindControl("ddlAddReceiversType");
                if (cboApp.SelectedItem.Value != "-1")
                {
                    dlAddReceiversType.Items.Clear();

                    if (cboApp.SelectedItem.Value == "1026" || cboApp.SelectedItem.Value == "420")
                    {
                        dlAddReceiversType.Items.Insert(0, new ListItem("", "0"));
                        dlAddReceiversType.Items.Insert(1, new ListItem("�ͻ�", "1"));
                        dlAddReceiversType.Items.Insert(2, new ListItem("������", "2"));
                        dlAddReceiversType.Items.Insert(3, new ListItem("�ͻ��ʹ�����", "3"));
                    }
                    else
                    {
                        dlAddReceiversType.Items.Insert(0, new ListItem("", "0"));
                        dlAddReceiversType.Items.Insert(1, new ListItem("������", "2"));
                    }
                }
                #endregion

                HtmlInputHidden hdType = (HtmlInputHidden)e.Item.FindControl("HidAddReceiverType");
                //dlAddReceiversType.SelectedItem.Text = hdType.Value;
                dlAddReceiversType.SelectedIndex = dlAddReceiversType.Items.IndexOf(dlAddReceiversType.Items.FindByText(hdType.Value));

                HtmlInputHidden hdNameId = (HtmlInputHidden)e.Item.FindControl("hidAddTNname");
                TextBox textName = (TextBox)e.Item.FindControl("txtAddTName");
                textName.Text = hdNameId.Value;
            }
            else if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //ȡ��������
                long appid = long.Parse(cboApp.SelectedValue.ToString());
                long flowmodeid = long.Parse(cboFlowModel.SelectedValue.ToString());
                DropDownList dlNodeName = (DropDownList)e.Item.FindControl("drpNodeName");

                DataTable dt = mRulDP.GetNodeName(appid, flowmodeid);
                DataView dv = dt.DefaultView;

                dlNodeName.DataSource = dv;
                dlNodeName.DataTextField = "nodename";
                dlNodeName.DataValueField = "NodeModelID";
                dlNodeName.DataBind();
                dlNodeName.Items.Insert(0, new ListItem("���л���", "-1"));

                HtmlInputHidden hdNodeID = (HtmlInputHidden)e.Item.FindControl("hidNodeId");

                dlNodeName.SelectedIndex = dlNodeName.Items.IndexOf(dlNodeName.Items.FindByValue(hdNodeID.Value));


                #region
                DropDownList dlReceiversType = (DropDownList)e.Item.FindControl("ddlReceiversType");

                if (cboApp.SelectedItem.Value != "-1")
                {
                    dlReceiversType.Items.Clear();

                    if (cboApp.SelectedItem.Value == "1026" || cboApp.SelectedItem.Value == "420")
                    {
                        dlReceiversType.Items.Insert(0, new ListItem("", "0"));
                        dlReceiversType.Items.Insert(1, new ListItem("�ͻ�", "1"));
                        dlReceiversType.Items.Insert(2, new ListItem("������", "2"));
                        dlReceiversType.Items.Insert(3, new ListItem("�ͻ��ʹ�����", "3"));
                    }
                    else
                    {
                        dlReceiversType.Items.Insert(0, new ListItem("", "0"));
                        dlReceiversType.Items.Insert(1, new ListItem("������", "2"));
                    }
                }
                HtmlInputHidden hdType = (HtmlInputHidden)e.Item.FindControl("HidReceiverType");

                //dlReceiversType.SelectedItem.Text = hdType.Value;
                dlReceiversType.SelectedIndex = dlReceiversType.Items.IndexOf(dlReceiversType.Items.FindByText(hdType.Value));

                HtmlInputHidden hdNameId = (HtmlInputHidden)e.Item.FindControl("hidTNname");
                TextBox textName = (TextBox)e.Item.FindControl("txtTName");
                textName.Text = hdNameId.Value;

                TriggerType = (HtmlInputHidden)e.Item.FindControl("hiddropTriggerType");
                DropDownList dlTriggerType = (DropDownList)e.Item.FindControl("dropTriggerType");


                if (TriggerType != null && TriggerType.Value != "")
                {
                    switch (TriggerType.Value)
                    {
                        case "����":
                            dlTriggerType.SelectedIndex = 0;
                            break;
                        case "��Ӧ����Ԥ��":
                            dlTriggerType.SelectedIndex =1;
                            break;
                        case "��Ӧ����":
                            dlTriggerType.SelectedIndex = 2;
                            break;
                        case "�������Ԥ��":
                            dlTriggerType.SelectedIndex = 3;
                            break;
                        case "�������":
                            dlTriggerType.SelectedIndex = 4;
                            break;
                    }
                }

                //ʱ��
                IntervalTime=(HtmlInputHidden)e.Item.FindControl("hidtxt_Time");
                CtrFlowNumeric txt_time = (CtrFlowNumeric)e.Item.FindControl("txt_Time");
                txt_time.Value = "";
                if (IntervalTime != null && IntervalTime.Value != "")
                    txt_time.Value = IntervalTime.Value;
                

                //������
                hidRecipient_User = (HtmlInputHidden)e.Item.FindControl("hiduc_User");//����
                hidRecipient_UserID = (HtmlInputHidden)e.Item.FindControl("HidUserID");//ID
                UserPickerMult Recipient_User = (UserPickerMult)e.Item.FindControl("uc_user");
                
               
                Recipient_User.UserName = "";
                Recipient_User.UserID = "";

                if (hidRecipient_User != null && hidRecipient_User.Value != "")
                {
                    Recipient_User.UserName = hidRecipient_User.Value;
                }
                if (hidRecipient_UserID != null && hidRecipient_UserID.Value != "")
                {
                    Recipient_User.UserID = hidRecipient_UserID.Value;
                }
                #endregion
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgCondition_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            DataTable dt = new DataTable();
            if (e.CommandName == "Delete")
            {
                this.Master.Btn_save.Enabled = true;//���¼�¼�����ñ��水ť
                string hidId = "";
                
                dt = GetDetailItem(true, e.Item.ItemIndex, ref hidId);
                dt.Rows.RemoveAt(e.Item.ItemIndex);
                dgCondition.DataSource = dt.DefaultView;
                dgCondition.DataBind();
            }
            else if (e.CommandName == "Add")
            {
                this.Master.Btn_save.Enabled = true;//���¼�¼�����ñ��水ť

                string hidid = "";
               
                dt = GetDetailItem(false, e.Item.ItemIndex, ref hidid);

                
                    dgCondition.DataSource = dt.DefaultView;
                    dgCondition.DataBind();
                
            }
        }
        private void popupSelectWindow(DataGridCommandEventArgs e) 
        {
            HtmlInputHidden hidClientId_ForOpenerPage = (HtmlInputHidden)e.Item.FindControl("hidClientId_ForOpenerPage");
           // Button cmdAddTembutton = (Button)e.Item.FindControl("cmdTem");
            Random r = new Random();
            int getrandom = r.Next();
            if (cboApp.SelectedValue == "-1" || cboApp.SelectedValue == "") 
            {
                ClientScript.RegisterStartupScript(Page.GetType(),"","alert('��ѡ��Ӧ������!')",true);
            }
            HiddenField hidType = (HiddenField)e.Item.FindControl("typeValue");


            string url = "'../MailAndMessageRule/MailMessageTemManager.aspx?IsSelect=1& randomid=" + getrandom + "&systemID=" + cboApp.SelectedValue + "&Opener_ClientId=" + hidClientId_ForOpenerPage.ClientID + "&type=" + hidType.Value + "'";
            string flog = " var popupwindowFlag=1; if($.browser.safari) { popupwindowFlag=0; alert('����Safari�İ�ȫ���ƣ��������ݲ�֧��Safari�����!'); }";
            string strs = flog + "if (popupwindowFlag == 1) {  window.open(" + url + ", 'E8OpenWin', 'resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=520,height=340,left=150,top=50');}";
            
            ClientScript.RegisterStartupScript(Page.GetType(), "", strs, true);
        }


        public bool IsEmailModel(DataTable dt)
        {
            bool isok = true;
            long soFlowModelid = long.Parse(cboFlowModel.SelectedValue.ToString());
            MailAndMessageRuleDP mRuleDP = new MailAndMessageRuleDP();
            DataTable dt_emailMessage = mRuleDP.GetMessageRulInstall(soFlowModelid.ToString());

            foreach (DataRow newme in dt.Rows)
            {
                foreach (DataRow mesage in dt_emailMessage.Rows)
                {
                    string OFlowModelID = newme["OFlowModelID"].ToString();
                    string NodeId = newme["NodeId"].ToString();
                    string FlowNameID = newme["FlowNameID"].ToString();
                    string ReceiverStypeName = newme["ReceiverStypeName"].ToString();
                    string TRIGGER_TYPE = newme["TRIGGER_TYPE"].ToString();
                    string Recipient_User = newme["Recipient_User"].ToString();

                    string OFlowModelID_mesage = mesage["OFlowModelID"].ToString();//ģ��ID
                    string NodeId_mesage = mesage["NodeId"].ToString();//����
                    string FlowNameID_mesage = mesage["FlowNameID"].ToString();//�ʼ�ģ��
                    string ReceiverStypeName_mesage = mesage["ReceiverStypeName"].ToString();//��������
                    string TRIGGER_TYPE_mesage = mesage["TRIGGER_TYPE"].ToString();//��������
                    string Recipient_User_mesage = mesage["Recipient_User"].ToString();//�쵼
                   

                    if((OFlowModelID == OFlowModelID_mesage) && (NodeId == NodeId_mesage) &&(FlowNameID == FlowNameID_mesage && 
                        (ReceiverStypeName == ReceiverStypeName_mesage) && (TRIGGER_TYPE == TRIGGER_TYPE_mesage) &&
                        (Recipient_User == Recipient_User_mesage)))
                    {
                        isok = false;
                    }
                }
            }

            return isok;
        }

        private DataTable GetDetailItem(bool isAll, int indexs, ref string strHidAddValue)
        {
            MailAndMessageRuleDP mRuleDP = new MailAndMessageRuleDP();
            DataTable dt = new DataTable();

            DataRow dr;  //������
            int id = 0;

            #region ������
            dt.Columns.Add("ID", Type.GetType("System.Int64"));
            dt.Columns.Add("OFlowModelID", Type.GetType("System.Int64"));
            dt.Columns.Add("NodeId", Type.GetType("System.Int64"));
            dt.Columns.Add("NodeName", Type.GetType("System.String"));
            dt.Columns.Add("NodeContent", Type.GetType("System.String"));
            dt.Columns.Add("FlowNameID", Type.GetType("System.Int64"));
            dt.Columns.Add("FlowName", Type.GetType("System.String"));
            dt.Columns.Add("ReceiverStypeName", Type.GetType("System.String"));
            dt.Columns.Add("TRIGGER_TYPE",Type.GetType("System.String"));
            dt.Columns.Add("INTERVAL_TIME", Type.GetType("System.String"));
            dt.Columns.Add("Recipient_User", Type.GetType("System.String"));
            dt.Columns.Add("Recipient_UserID", Type.GetType("System.String"));
            #endregion


            #region  ����DataTable

            foreach (DataGridItem row in dgCondition.Controls[0].Controls)
            {
                id++;
                if (row.ItemType == ListItemType.Footer)
                {

                    long soFlowModelid = long.Parse(cboFlowModel.SelectedValue.ToString());

                    DropDownList drnode = (DropDownList)row.FindControl("drpAddNodeName");
                    DropDownList drtriggertype =(DropDownList)row.FindControl("dropAddTriggerType");
                    

                    long sNodeId = long.Parse(drnode.SelectedItem.Value == "" ? "0" : drnode.SelectedItem.Value);
                    string sNodeName = drnode.SelectedItem.Text;

                    string sNodeContent = ((CtrFlowFormText)row.FindControl("CtrFlowAddMailTitle")).Value;
                    HtmlInputHidden hdNodeID = (HtmlInputHidden)row.FindControl("hidAddTNid");

                    long sFlowNameId = long.Parse(hdNodeID.Value == "" ? "0" : hdNodeID.Value);

                    string sFlowName = ((TextBox)row.FindControl("txtAddTName")).Text;
                    DropDownList ddlReceiverStyleName = (DropDownList)row.FindControl("ddlAddReceiversType");
                    string sReceiverStypeName = ddlReceiverStyleName.SelectedItem.Text;

                    string triggerType = drtriggertype.SelectedItem.Text;
                    string intervaltime = ((CtrFlowNumeric)row.FindControl("txt_TimeAdd")).Value.ToString();//ʱ��
                    string Recipient_User = ((UserPickerMult)row.FindControl("uc_userAdd")).UserName.ToString();//������
                    string Recipient_UserID = ((UserPickerMult)row.FindControl("uc_userAdd")).UserID.ToString();//������ID

                    if (row.ItemIndex == indexs)
                    {
                        strHidAddValue = sNodeId.ToString();
                    }

                    if (sFlowNameId != 0 && sNodeId != 0)
                        {
                            bool isok = true;
                            foreach (DataRow drme in dt.Rows)
                            {
                                string OFlowModelID_mesage = drme["OFlowModelID"].ToString();//ģ��ID
                                string NodeId_mesage = drme["NodeId"].ToString();//����
                                string FlowNameID_mesage = drme["FlowNameID"].ToString();//�ʼ�ģ��
                                string ReceiverStypeName_mesage = drme["ReceiverStypeName"].ToString();//��������
                                string TRIGGER_TYPE_mesage = drme["TRIGGER_TYPE"].ToString();//��������
                                string Recipient_User_mesage = drme["Recipient_User"].ToString();//�쵼

                                if ((soFlowModelid.ToString() == OFlowModelID_mesage) && (sNodeId.ToString() == NodeId_mesage) && (sFlowNameId.ToString() == FlowNameID_mesage
                                 && (triggerType == TRIGGER_TYPE_mesage)))
                                {
                                    if ((sReceiverStypeName != "" && ReceiverStypeName_mesage != "") && (sReceiverStypeName==ReceiverStypeName_mesage) )
                                    {
                                        isok = false;
                                    }

                                    if ((Recipient_User_mesage != "" && Recipient_User != "") && (Recipient_User_mesage == Recipient_User))
                                    {
                                        isok = false;
                                    }
                                    
                                }
                            }

                            if (isok)
                            {
                                if (Recipient_UserID == "" && sReceiverStypeName == "")
                                {
                                    PageTool.MsgBox(this, "��ѡ�����Email���û�");
                                }
                                else
                                {
                                    dr = dt.NewRow();
                                    dr["ID"] = 0;
                                    dr["OFlowModelID"] = soFlowModelid;
                                    dr["NodeId"] = sNodeId;
                                    dr["NodeName"] = sNodeName;
                                    dr["NodeContent"] = sNodeContent;
                                    dr["FlowNameID"] = sFlowNameId;
                                    dr["FlowName"] = sFlowName;
                                    dr["ReceiverStypeName"] = sReceiverStypeName;
                                    dr["TRIGGER_TYPE"] = triggerType;
                                    dr["INTERVAL_TIME"] = intervaltime;
                                    dr["Recipient_User"] = Recipient_User;
                                    dr["Recipient_UserID"] = Recipient_UserID;
                                    dt.Rows.Add(dr);
                                }
                            }
                            else
                            {
                               
                                PageTool.MsgBox(this, "�˹����Ѿ����ڣ�����ʧ�ܣ�");
                            }

                        }
                        else
                        {
                            if (!isAll)
                            {
                                //PageTool.MsgBox(this, "���ڹ���ģ�岻��Ϊ�գ�");
                                PageTool.MsgBox(this, "��Ϣ���������� * �Ĳ���Ϊ��!");
                            }
                        }
                }
                else if (row.ItemType == ListItemType.Item || row.ItemType == ListItemType.AlternatingItem)
                {

                    long soFlowModelid = long.Parse(cboFlowModel.SelectedValue.ToString());
                    DropDownList drnode = (DropDownList)row.FindControl("drpNodeName");
                    DropDownList drtriggertype = (DropDownList)row.FindControl("dropTriggerType");

                    long sNodeId = long.Parse(drnode.SelectedItem.Value == "" ? "0" : drnode.SelectedItem.Value);
                    string sNodeName = drnode.SelectedItem.Text;

                    string sNodeContent = ((CtrFlowFormText)row.FindControl("CtrFlowMailTitle")).Value;

                    HtmlInputHidden hdNodeID = (HtmlInputHidden)row.FindControl("hidTNid");
                    long sFlowNameId = long.Parse(hdNodeID.Value);

                    string sFlowName = ((TextBox)row.FindControl("txtTName")).Text;
                    DropDownList ddlReceiverStypeName = (DropDownList)row.FindControl("ddlReceiversType");
                    string sReceiverStypeName = ddlReceiverStypeName.SelectedItem.Text;

                    string triggerType = drtriggertype.SelectedItem.Text;
                    string intervaltime = ((CtrFlowNumeric)row.FindControl("txt_Time")).Value.ToString();

                    string Recipient_User = ((UserPickerMult)row.FindControl("uc_user")).UserName.ToString();//������
                    string Recipient_UserID = ((UserPickerMult)row.FindControl("uc_user")).UserID.ToString();//������ID

                    if (row.ItemIndex == indexs)
                    {
                        strHidAddValue = sNodeId.ToString();
                    }

                    if (sFlowNameId != 0)
                    {
                        bool isok = true;
                        foreach (DataRow drm in dt.Rows)
                        {
                            string OFlowModelID_mesage = drm["OFlowModelID"].ToString();//ģ��ID
                            string NodeId_mesage = drm["NodeId"].ToString();//����
                            string FlowNameID_mesage = drm["FlowNameID"].ToString();//�ʼ�ģ��
                            string ReceiverStypeName_mesage = drm["ReceiverStypeName"].ToString();//��������
                            string TRIGGER_TYPE_mesage = drm["TRIGGER_TYPE"].ToString();//��������
                            string Recipient_User_mesage = drm["Recipient_User"].ToString();//�쵼

                            if ((soFlowModelid.ToString() == OFlowModelID_mesage) && (sNodeId.ToString() == NodeId_mesage) && (sFlowNameId.ToString() == FlowNameID_mesage
                             && (triggerType == TRIGGER_TYPE_mesage)))
                            {
                                if ((sReceiverStypeName != "" && ReceiverStypeName_mesage != "") && (sReceiverStypeName == ReceiverStypeName_mesage))
                                {
                                    isok = false;
                                }

                                if ((Recipient_User_mesage != "" && Recipient_User != "") && (Recipient_User_mesage == Recipient_User))
                                {
                                    isok = false;
                                }
                            }
                        }

                        if (isok)
                        {
                            if (Recipient_UserID == "" && sReceiverStypeName == "")
                            {
                                PageTool.MsgBox(this, "��ѡ�����Email���û�");
                            }
                            else
                            {
                                dr = dt.NewRow();
                                dr["ID"] = 0;
                                dr["OFlowModelID"] = soFlowModelid;
                                dr["NodeId"] = sNodeId;
                                dr["NodeName"] = sNodeName;
                                dr["NodeContent"] = sNodeContent;
                                dr["FlowNameID"] = sFlowNameId;
                                dr["FlowName"] = sFlowName;
                                dr["ReceiverStypeName"] = sReceiverStypeName;
                                dr["TRIGGER_TYPE"] = triggerType;
                                dr["INTERVAL_TIME"] = intervaltime;
                                dr["Recipient_User"] = Recipient_User;
                                dr["Recipient_UserID"] = Recipient_UserID;
                                dt.Rows.Add(dr);
                            }
                        }
                        else
                        {
                           
                            PageTool.MsgBox(this,"�˹����Ѿ����ڣ�����ʧ�ܣ�");
                        }
                    }
                }
            }

            #endregion


            return dt;
        }
    }
}