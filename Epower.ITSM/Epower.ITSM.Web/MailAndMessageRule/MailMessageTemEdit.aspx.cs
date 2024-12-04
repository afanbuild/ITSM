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
using System.Collections.Generic;

namespace Epower.ITSM.Web.AppForms
{
    /// <summary>
    /// MailMessageTemEdit ��ժҪ˵����
    /// </summary>
    public partial class MailMessageTemEdit : BasePage
    {
        #region SetParentButtonEvent
        /// <summary>
        /// ���ø����尴ť�¼�
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.SystemManager;
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
                Response.Redirect("MailMessageTemEdit.aspx?QuickNew=1");
            else
                Response.Redirect("MailMessageTemEdit.aspx");
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
                MailMessageTemManagerDP ee = new MailMessageTemManagerDP();
                ee.DeleteRecorded(long.Parse(this.Master.MainID.Trim()));
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
            string result = string.Empty;
            if (string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                MailMessageTemManagerDP ee = new MailMessageTemManagerDP();
                InitObject(ee);
                ee.RegUserId = long.Parse(Session["UserID"].ToString());
                ee.RegUserName = Session["PersonName"].ToString();
                ee.RegDeptId = long.Parse(Session["UserDeptID"].ToString());
                ee.RegDeptName = Session["UserDeptName"].ToString();
                ee.RegTime = DateTime.Now;
                ee.FlowModelID = cboFlowModel.SelectedValue;
                result = ee.InsertRecorded(ee);
                if (result == "Error")
                {
                    Master.IsSaveSuccess = false;
                    Epower.DevBase.BaseTools.PageTool.MsgBox(this.Page, "����ʧ���ʼ�ģ�����������ģ�����ݲ����ظ���");
                    return;
                }
                this.Master.MainID = ee.ID.ToString().Trim();
            }
            else
            {
                MailMessageTemManagerDP ee = new MailMessageTemManagerDP();
                ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                ee.FlowModelID = cboFlowModel.SelectedValue;

                InitObject(ee);
                result = ee.UpdateRecorded(ee);
                if (result == "Error")
                {
                    Master.IsSaveSuccess = false;
                    Epower.DevBase.BaseTools.PageTool.MsgBox(this.Page, "����ʧ���ʼ�ģ�����������ģ�����ݲ����ظ���");
                    return;
                }
                this.Master.MainID = ee.ID.ToString();
            }

        }

        #endregion

        #region Master_Master_Button_GoHistory_Click

        /// <summary>
        /// ����
        /// </summary>
        void Master_Master_Button_GoHistory_Click()
        {
            Response.Redirect("MailMessageTemManager.aspx");

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
                //labModeCoumList.Text = CommonDP.GetConfigValue("Other", "MailModeCoumList");

                ddlBind();
                //��������
                LoadData();
            }
        }

        #endregion

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

        }

        #region LoadData


        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            if (!string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                MailMessageTemManagerDP ee = new MailMessageTemManagerDP();
                ee = ee.GetReCorded(long.Parse(this.Master.MainID));
                LoadData(ee);
            }
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="ee"></param>
        private void LoadData(MailMessageTemManagerDP ee)
        {
            CtrTemplateName.Value = ee.TemplateName;//ģ������
            leaderMailContent.Text = ee.leadercontent;//�쵼ģ������
            FTBMailContent.Text = ee.MailContent;//�ʼ�ģ������
            FTBMailContent2.Text = ee.DealMainContent; //�������ʼ�ģ������
            txtModelContent.Text = ee.ModelContent;//����ģ������
            ddlStatus.SelectedValue = ee.Status.ToString();//�Ƿ���Ч
            txtRemark.Text = ee.Remark;//��ע
            cboApp.SelectedIndex = cboApp.Items.IndexOf(cboApp.Items.FindByValue(ee.SystemID.ToString()));


            getDesc(long.Parse(ee.SystemID.ToString()));//��ȡ����˵��

            #region �жϴ�ģ���Ƿ��Ѿ��ڹ������õ� yxq
            string tempName = ee.selectMailAndMessage(long.Parse(ee.ID.ToString()));
            if (tempName.Trim() != "")
                cboApp.Enabled = false;
            #endregion


            if (cboApp.SelectedValue == "199")
            {
                cboFlowModel.SelectedValue = ee.FlowModelID;
            }
        }

        #endregion

        #region InitObject

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ee"></param>
        private void InitObject(MailMessageTemManagerDP ee)
        {
            ee.TemplateName = CtrTemplateName.Value.Trim().ToString();//ģ������
            ee.leadercontent = leaderMailContent.Text.Trim().ToString();//�쵼ģ������
            ee.MailContent = FTBMailContent.Text.Trim().ToString();//�ʼ�ģ������
            ee.DealMainContent = FTBMailContent2.Text.Trim().ToString(); //�������ʼ�ģ������
            ee.ModelContent = txtModelContent.Text.Trim().ToString();//����ģ������
            ee.Status = Decimal.Parse(ddlStatus.SelectedItem.Value);//�Ƿ���Ч
            ee.Remark = txtRemark.Text.Trim().ToString();//��ע
            ee.SystemID = decimal.Parse(cboApp.SelectedItem.Value == "" ? "0" : cboApp.SelectedItem.Value);
        }

        #endregion


        #region ddlChanged

        protected void cboApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            trFlowModelID.Visible = false;
            getDesc(long.Parse(cboApp.SelectedItem.Value));
        }

        protected void cboFlowModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            String content = LoadNormalAppDisplayName();
            divDesc.InnerHtml = "�����ʼ����ÿɰ������¹������ã�<br> " + content;
        }

        protected void getDesc(long AppID)
        {

            string content = "";
            switch (AppID)
            {

                case 1028://��������
                    content += "[#?��Ϣ����?#] &nbsp;";
                    content += "[#?��Ϣ���?#] &nbsp;";
                    content += "[#?������?#] &nbsp;";
                    content += "[#?����ʱ��?#] &nbsp;";
                    content += "[#?��ֹʱ��?#] &nbsp;";
                    content += "[#?��������?#] &nbsp;";
                    break;
                case 201://�Զ��������
                    break;
                case 400://֪ʶ����                  
                    content += "[#?����?#] &nbsp;";
                    content += "[#?�ؼ���?#] &nbsp;";
                    content += "[#?ժҪ?#] &nbsp;";
                    content += "[#?֪ʶ���?#] &nbsp;";
                    content += "[#?֪ʶ����?#] &nbsp;";
                    break;
                case 199://ͨ������ 

                    LoadFlowModelID();
                    trFlowModelID.Visible = true;

                    content = LoadNormalAppDisplayName();

                    break;
                case 210://�������
                    content += "[#?���ⵥ��?#] &nbsp;";
                    content += "[#?�Ǽ���?#] &nbsp;";
                    content += "[#?�Ǽ��˲���?#] &nbsp;";
                    content += "[#?�Ǽ�ʱ��?#] &nbsp;";
                    content += "[#?Ӱ���?#] &nbsp;";
                    content += "[#?������?#] &nbsp;";
                    content += "[#?����?#] &nbsp;";
                    content += "[#?��������?#] &nbsp;";
                    content += "[#?����״̬?#] &nbsp;";
                    content += "[#?�������?#] &nbsp;";                    

                    break;
                case 410://�ʲ�Ѳ�� 
                    content += "[#?����?#] &nbsp;";
                    content += "[#?�Ǽ���?#] &nbsp;";
                    content += "[#?�Ǽǲ���?#] &nbsp;";
                    content += "[#?�Ǽ�ʱ��?#] &nbsp;";
                    content += "[#?��ע?#] &nbsp;";
                    break;
                case 420://�������
                    content += "[#?�������?#] &nbsp;";
                    content += "[#?�ͻ�����?#] &nbsp;";
                    content += "[#?�ͻ��绰?#] &nbsp;";
                    content += "[#?�������?#] &nbsp;";
                    content += "[#?���״̬?#] &nbsp;";
                    content += "[#?Ӱ���?#] &nbsp;";
                    content += "[#?ժҪ?#] &nbsp;";
                    content += "[#?��������?#] &nbsp;";
                    content += "[#?������?#] &nbsp;";                    
                    break;
                case 1026://�¼�����
                    content += "[#?�¼�����?#] &nbsp;";
                    content += "[#?�ͻ�����?#] &nbsp;";
                    content += "[#?�ͻ��ֻ�?#] &nbsp;";
                    content += "[#?�ͻ��ʼ�?#] &nbsp;";
                    content += "[#?�Ǽ�ʱ��?#] &nbsp;";
                    content += "[#?����ʱ��?#] &nbsp;";
                    content += "[#?����ʱ��?#] &nbsp;";
                    content += "[#?�¼����?#] &nbsp;";
                    content += "[#?���񼶱�?#] &nbsp;";
                    content += "[#?Ӱ�췶Χ?#] &nbsp;";
                    content += "[#?������?#] &nbsp;";
                    content += "[#?ժҪ?#] &nbsp;";
                    content += "[#?��ϸ����?#] &nbsp;";                    
                    break;
                default:
                    break;
            }

            divDesc.InnerHtml = "�����ʼ����ÿɰ������¹������ã�<br> " + content;
        }
        #endregion


        private void LoadFlowModelID()
        {
            trFlowModelID.Visible = true;

            string stWhere = cboApp.SelectedItem.Value == "-1" ? "" : " and AppID=" + cboApp.SelectedItem.Value;
            stWhere = stWhere + " and status=1 and deleted=0 ";
            cboFlowModel.DataSource = MailAndMessageRuleDP.getAllFlowModel(stWhere).DefaultView;
            cboFlowModel.DataTextField = "flowname";
            cboFlowModel.DataValueField = "oflowmodelid";
            cboFlowModel.DataBind();
        }

        private String LoadNormalAppDisplayName()
        {
            String content = "ͨ�ñ��̶��ֶΣ�[#?������?#], [#?�Ǽ���?#], [#?�Ǽ�����?#], [#?�Ǽǲ���?#] �Զ����ֶΣ�";

            long lngOFlowModelID = long.Parse(cboFlowModel.SelectedValue);
            List<KeyValuePair<String, String>> listField = AppFieldConfigDP.GetFieldNameAndDisplayName(lngOFlowModelID);

            foreach (KeyValuePair<String, String> field in listField)
            {
                content += String.Format("[#?{0}?#] &nbsp;", field.Value);
            }

            return content;
        }

    }
}
