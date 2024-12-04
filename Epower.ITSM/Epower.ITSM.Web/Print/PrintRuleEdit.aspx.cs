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
using Epower.ITSM.SqlDAL.Print;

namespace Epower.ITSM.Web.Print
{
    /// <summary>
    /// MailMessageTemEdit ��ժҪ˵����
    /// </summary>
    public partial class PrintRuleEdit : BasePage
    {
        #region SetParentButtonEvent
        /// <summary>
        /// ���ø����尴ť�¼�
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.IssueShortCutReqTemplate;
            this.Master.IsCheckRight = true;
            if (this.Request.QueryString["id"] != null)
            {
                this.Master.MainID = this.Request.QueryString["id"].ToString();
            }

            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);
            this.Master.Master_Button_Save_Click += new Global_BtnClick(Master_Master_Button_Save_Click);
            this.Master.Master_Button_GoHistory_Click += new Global_BtnClick(Master_Master_Button_GoHistory_Click);

            this.Master.setButtonRigth(Constant.IssueShortCutReqTemplate, true);
        }



        #endregion

        #region Master_Master_Button_New_Click

        /// <summary>
        /// ����
        /// </summary>
        void Master_Master_Button_New_Click()
        {

            Response.Redirect("PrintRuleEdit.aspx");
           
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
                PRINTRULE Entity = new PRINTRULE();
                Entity.delete(long.Parse(this.Master.MainID.ToString()));
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
                PRINTRULE Entity;
                if (ViewState["PRINTRULE"] != null)
                {
                    Entity = (PRINTRULE)ViewState["PRINTRULE"];
                }
                else
                {
                    Entity = new PRINTRULE();
                }

                if (string.IsNullOrEmpty(this.Master.MainID.Trim()))
                {
                    Entity.createByID = long.Parse(Session["UserID"].ToString());
                    Entity.createByName = Session["PersonName"].ToString();
                }
                else
                {
                    Entity.ID = long.Parse(Master.MainID.Trim());
                }

                Entity.PrintRuleName=CtrRuleName.Value.Trim();
                Entity.AppId = long.Parse(cboApp.SelectedItem.Value);
                Entity.AppNames = cboApp.SelectedItem.Text;
                Entity.FlowModelId = long.Parse(cboFlowModel.SelectedItem.Value);
                Entity.FlowModelName = cboFlowModel.SelectedItem.Text;
                Entity.IsOpen = int.Parse(ddlStatus.SelectedItem.Value);
                Entity.ModelContent = UEMailContent.Content;
                Entity.remark = txtRemark.Text;
                Entity.modifyByID = long.Parse(Session["UserID"].ToString());
                Entity.modifyByName = Session["PersonName"].ToString();
                Entity.IsProcess = Int32.Parse(drIsProcess.SelectedItem.Value);

                Entity = Entity.Save(Entity);
                ViewState["PRINTRULE"] = Entity;
                Master.MainID = Entity.ID.ToString();
                this.Master.IsSaveSuccess = true;
            }
            else
            {
                PageTool.MsgBox(this, " ��ѡ������ģ��");
                this.Master.IsSaveSuccess=false;
                
            }

        }

        #endregion

        #region Master_Master_Button_GoHistory_Click

        /// <summary>
        /// ����
        /// </summary>
        void Master_Master_Button_GoHistory_Click()
        {
            Response.Redirect("PrintRuleList.aspx");

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

                PRINTRULE Entity = new PRINTRULE();
                if (Master.MainID.ToString() != "")
                {
                    Entity=Entity.select(long.Parse(Master.MainID.ToString()));
                    if (Entity != null)
                    {
                        CtrRuleName.Value = Entity.PrintRuleName.ToString();
                        cboApp.SelectedIndex = cboApp.Items.IndexOf(cboApp.Items.FindByValue(Entity.AppId.ToString()));

                        if (cboApp.SelectedItem.Value != "-1")
                        {
                            string stWhere = cboApp.SelectedItem.Value == "-1" ? "" : " and AppID=" + cboApp.SelectedItem.Value;
                            stWhere = stWhere + " and status=1 and deleted=0 ";
                            cboFlowModel.DataSource = MailAndMessageRuleDP.getAllFlowModel(stWhere).DefaultView;
                            cboFlowModel.DataTextField = "flowname";
                            cboFlowModel.DataValueField = "oflowmodelid";
                            cboFlowModel.DataBind();


                        }
                        getDesc(long.Parse(cboApp.SelectedItem.Value));

                        cboFlowModel.SelectedIndex = cboFlowModel.Items.IndexOf(cboFlowModel.Items.FindByValue(Entity.FlowModelId.ToString()));
                        ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(Entity.IsOpen.ToString()));
                        drIsProcess.SelectedIndex = drIsProcess.Items.IndexOf(drIsProcess.Items.FindByValue(Entity.IsProcess.ToString()));
                        UEMailContent.Content = Entity.ModelContent;
                        txtRemark.Text = Entity.remark;
                        Master.MainID = Entity.ID.ToString();


                        ViewState["PRINTRULE"] = Entity;
                    }
                }
              
            }
        }

        #endregion



        #region dllBind

        private void ddlBind()
        {
            cboApp.DataSource = epApp.GetAllApps().DefaultView;
            cboApp.DataTextField = "AppName";
            cboApp.DataValueField = "AppID";
            cboApp.DataBind();

            cboApp.Items.Remove(new ListItem("ͨ������", "199"));
            cboApp.Items.Remove(new ListItem("����������", "1027"));


            ListItem itm = new ListItem("", "-1");
            cboApp.Items.Insert(0, itm);
            cboApp.SelectedIndex = 0;

            ListItem itm1 = new ListItem("", "-1");
            cboFlowModel.Items.Insert(0, itm1);
            cboFlowModel.SelectedIndex = 0;
        }

        #endregion



        #region ddlChanged

        protected void cboApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            string stWhere = cboApp.SelectedItem.Value == "-1" ? " and 1=2 " : " and AppID=" + cboApp.SelectedItem.Value;
            stWhere = stWhere + " and status=1 and deleted=0 ";
            cboFlowModel.DataSource = MailAndMessageRuleDP.getAllFlowModel(stWhere).DefaultView;
            cboFlowModel.DataTextField = "flowname";
            cboFlowModel.DataValueField = "oflowmodelid";
            cboFlowModel.DataBind();

            ListItem itm1 = new ListItem("", "-1");
            cboFlowModel.Items.Insert(0, itm1);
            cboFlowModel.SelectedIndex = 0;

            getDesc(long.Parse(cboApp.SelectedItem.Value));
        }

        #endregion


        protected void getDesc(long AppID)
        {

            string content = "";
            switch (AppID)
            {

                case 1028://��������
                    content += "[#?�汾����?#] &nbsp;";
                    content += "[#?�汾��?#] &nbsp;";
                    content += "[#?������Χ?#] &nbsp;";
                    content += "[#?��ϵ��?#] &nbsp;";
                    content += "[#?��ϵ�绰?#] &nbsp;";
                    content += "[#?�汾����?#] &nbsp;";
                    content += "[#?�汾����?#] &nbsp;";
                    content += "[#?�汾�������ݼ��?#] &nbsp;";
                    break;
                case 1062://�������
                    content += "[#?�ͻ�����?#] &nbsp;";
                    content += "[#?�ͻ���ַ?#] &nbsp;";
                    content += "[#?��ϵ��?#] &nbsp;";
                    content += "[#?��ϵ�绰?#] &nbsp;";
                    content += "[#?�ͻ�����?#] &nbsp;";
                    content += "[#?�����ʼ�?#] &nbsp;";
                    content += "[#?����λ?#] &nbsp;";
                    content += "[#?ְλ?#] &nbsp;";
                    content += "[#?�ʲ�����?#] &nbsp;";
                    content += "[#?�ǵ���?#] &nbsp;";
                    content += "[#?�Ǽ�ʱ��?#] &nbsp;";
                    content += "[#?���󵥺�?#] &nbsp;";
                    content += "[#?�������?#] &nbsp;";
                    content += "[#?����״̬?#] &nbsp;";
                    content += "[#?��������?#] &nbsp;";
                    content += "[#?��ϸ����?#] &nbsp;";
                    break;
                case 201://�Զ��������
                    break;
                case 400://֪ʶ����                  
                    content += "[#?����?#] &nbsp;";
                    content += "[#?�ؼ���?#] &nbsp;";
                    content += "[#?ժҪ?#] &nbsp;";
                    content += "[#?֪ʶ���?#] &nbsp;";
                    content += "[#?֪ʶ����?#] &nbsp;";
                    content += "[#?ͬ�����?#] &nbsp;";
                    break;
                case 199://ͨ������       
                    break;
                case 210://�������                    
                    content += "[#?���ⵥ��?#] &nbsp;";
                    content += "[#?�Ǽ���?#] &nbsp;";
                    content += "[#?�Ǽ��˲���?#] &nbsp;";
                    content += "[#?�Ǽ�ʱ��?#] &nbsp;";
                    content += "[#?�������?#] &nbsp;";
                    content += "[#?���⼶��?#] &nbsp;";
                    content += "[#?Ӱ���?#] &nbsp;";
                    content += "[#?������?#] &nbsp;";
                    content += "[#?�ʲ�����?#] &nbsp;";
                    content += "[#?����״̬?#] &nbsp;";
                    content += "[#?ժҪ?#] &nbsp;";
                    content += "[#?��������?#] &nbsp;";                    
                    content += "[#?�������?#] &nbsp;";
                    break;
                case 410://�ʲ�Ѳ�� 
                    content += "[#?����?#] &nbsp;";
                    content += "[#?�Ǽ���?#] &nbsp;";
                    content += "[#?�Ǽǲ���?#] &nbsp;";
                    content += "[#?�Ǽ�ʱ��?#] &nbsp;";
                    content += "[#?��ע?#] &nbsp;";
                    content += "[#?Ѳ����Ϣ?#] &nbsp;";
                    break;
                case 420://�������                   
                    content += "[#?�ͻ�����?#] &nbsp;";
                    content += "[#?�ͻ���ַ?#] &nbsp;";
                    content += "[#?��ϵ��?#] &nbsp;";
                    content += "[#?��ϵ�绰?#] &nbsp;";
                    content += "[#?�ͻ�����?#] &nbsp;";
                    content += "[#?�����ʼ�?#] &nbsp;";
                    content += "[#?����λ?#] &nbsp;";
                    content += "[#?ְλ?#] &nbsp;";
                    content += "[#?�ʲ���Ϣ?#] &nbsp;";
                    content += "[#?�������?#] &nbsp;";
                    content += "[#?�Ǽ�ʱ��?#] &nbsp;";
                    content += "[#?������?#] &nbsp;";
                    content += "[#?���ʱ��?#] &nbsp;";
                    content += "[#?ժҪ?#] &nbsp;";
                    content += "[#?��������?#] &nbsp;";
                    content += "[#?Ӱ���?#] &nbsp;";
                    content += "[#?������?#] &nbsp;";
                    content += "[#?�������?#] &nbsp;";
                    content += "[#?���״̬?#] &nbsp;";
                    content += "[#?�������?#] &nbsp;";
                    content += "[#?�������?#] &nbsp;";
                    break;
                case 1026://�¼�����                    
                    content += "[#?�ͻ�����?#] &nbsp;";
                    content += "[#?�ͻ���ַ?#] &nbsp;";
                    content += "[#?��ϵ��?#] &nbsp;";
                    content += "[#?��ϵ�绰?#] &nbsp;";
                    content += "[#?�ͻ�����?#] &nbsp;";
                    content += "[#?�����ʼ�?#] &nbsp;";
                    content += "[#?����λ?#] &nbsp;";
                    content += "[#?ְλ?#] &nbsp;";
                    content += "[#?�ʲ�����?#] &nbsp;";
                    content += "[#?�ǵ���?#] &nbsp;";
                    content += "[#?�Ǽ�ʱ��?#] &nbsp;";
                    content += "[#?�¼�����?#] &nbsp;";
                    content += "[#?�¼����?#] &nbsp;";
                    content += "[#?����ʱ��?#] &nbsp;";
                    content += "[#?����ʱ��?#] &nbsp;";
                    content += "[#?������?#] &nbsp;";
                    content += "[#?Ӱ���?#] &nbsp;";
                    content += "[#?ժҪ?#] &nbsp;";
                    content += "[#?��������?#] &nbsp;";
                    content += "[#?���񼶱�?#] &nbsp;";
                    content += "[#?�ر�����?#] &nbsp;";
                    content += "[#?�¼���Դ?#] &nbsp;";
                    content += "[#?�¼�״̬?#] &nbsp;";
                    content += "[#?���ʱ��?#] &nbsp;";
                    content += "[#?�ɳ�ʱ��?#] &nbsp;";
                    content += "[#?����ʱ��?#] &nbsp;";
                    content += "[#?����ʦ?#] &nbsp;";
                    content += "[#?��ʩ�����?#] &nbsp;";
                    break;
                default:
                    break;
            }

            divDesc.InnerHtml = "��ӡ���ÿɰ������¹������ã�<br> "+content;
        }

    }
}
