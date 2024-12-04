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
    /// MailMessageTemEdit 的摘要说明。
    /// </summary>
    public partial class PrintRuleEdit : BasePage
    {
        #region SetParentButtonEvent
        /// <summary>
        /// 设置父窗体按钮事件
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
        /// 新增
        /// </summary>
        void Master_Master_Button_New_Click()
        {

            Response.Redirect("PrintRuleEdit.aspx");
           
        }

        #endregion

        #region Master_Master_Button_Delete_Click

        /// <summary>
        /// 删除
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            if (!String.IsNullOrEmpty(this.Master.MainID.ToString()))
            {
                PRINTRULE Entity = new PRINTRULE();
                Entity.delete(long.Parse(this.Master.MainID.ToString()));
                //返回主页面
                Master_Master_Button_GoHistory_Click();
            }
        }

        #endregion

        #region Master_Master_Button_Save_Click

        /// <summary>
        /// 保存
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
                PageTool.MsgBox(this, " 请选择流程模型");
                this.Master.IsSaveSuccess=false;
                
            }

        }

        #endregion

        #region Master_Master_Button_GoHistory_Click

        /// <summary>
        /// 返回
        /// </summary>
        void Master_Master_Button_GoHistory_Click()
        {
            Response.Redirect("PrintRuleList.aspx");

        }

        #endregion

        #region Page_Load

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            //设置主页面
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

            cboApp.Items.Remove(new ListItem("通用流程", "199"));
            cboApp.Items.Remove(new ListItem("进出操作间", "1027"));


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

                case 1028://发布管理
                    content += "[#?版本名称?#] &nbsp;";
                    content += "[#?版本号?#] &nbsp;";
                    content += "[#?发布范围?#] &nbsp;";
                    content += "[#?联系人?#] &nbsp;";
                    content += "[#?联系电话?#] &nbsp;";
                    content += "[#?版本性质?#] &nbsp;";
                    content += "[#?版本类型?#] &nbsp;";
                    content += "[#?版本发布内容简介?#] &nbsp;";
                    break;
                case 1062://需求管理
                    content += "[#?客户名称?#] &nbsp;";
                    content += "[#?客户地址?#] &nbsp;";
                    content += "[#?联系人?#] &nbsp;";
                    content += "[#?联系电话?#] &nbsp;";
                    content += "[#?客户部门?#] &nbsp;";
                    content += "[#?电子邮件?#] &nbsp;";
                    content += "[#?服务单位?#] &nbsp;";
                    content += "[#?职位?#] &nbsp;";
                    content += "[#?资产名称?#] &nbsp;";
                    content += "[#?登单人?#] &nbsp;";
                    content += "[#?登记时间?#] &nbsp;";
                    content += "[#?需求单号?#] &nbsp;";
                    content += "[#?需求类别?#] &nbsp;";
                    content += "[#?需求状态?#] &nbsp;";
                    content += "[#?需求主题?#] &nbsp;";
                    content += "[#?详细描述?#] &nbsp;";
                    break;
                case 201://自定义表单流程
                    break;
                case 400://知识管理                  
                    content += "[#?主题?#] &nbsp;";
                    content += "[#?关键字?#] &nbsp;";
                    content += "[#?摘要?#] &nbsp;";
                    content += "[#?知识类别?#] &nbsp;";
                    content += "[#?知识内容?#] &nbsp;";
                    content += "[#?同意入库?#] &nbsp;";
                    break;
                case 199://通用流程       
                    break;
                case 210://问题管理                    
                    content += "[#?问题单号?#] &nbsp;";
                    content += "[#?登记人?#] &nbsp;";
                    content += "[#?登记人部门?#] &nbsp;";
                    content += "[#?登记时间?#] &nbsp;";
                    content += "[#?问题类别?#] &nbsp;";
                    content += "[#?问题级别?#] &nbsp;";
                    content += "[#?影响度?#] &nbsp;";
                    content += "[#?紧急度?#] &nbsp;";
                    content += "[#?资产名称?#] &nbsp;";
                    content += "[#?问题状态?#] &nbsp;";
                    content += "[#?摘要?#] &nbsp;";
                    content += "[#?问题描述?#] &nbsp;";                    
                    content += "[#?解决方案?#] &nbsp;";
                    break;
                case 410://资产巡检 
                    content += "[#?标题?#] &nbsp;";
                    content += "[#?登记人?#] &nbsp;";
                    content += "[#?登记部门?#] &nbsp;";
                    content += "[#?登记时间?#] &nbsp;";
                    content += "[#?备注?#] &nbsp;";
                    content += "[#?巡检信息?#] &nbsp;";
                    break;
                case 420://变更管理                   
                    content += "[#?客户名称?#] &nbsp;";
                    content += "[#?客户地址?#] &nbsp;";
                    content += "[#?联系人?#] &nbsp;";
                    content += "[#?联系电话?#] &nbsp;";
                    content += "[#?客户部门?#] &nbsp;";
                    content += "[#?电子邮件?#] &nbsp;";
                    content += "[#?服务单位?#] &nbsp;";
                    content += "[#?职位?#] &nbsp;";
                    content += "[#?资产信息?#] &nbsp;";
                    content += "[#?变更单号?#] &nbsp;";
                    content += "[#?登记时间?#] &nbsp;";
                    content += "[#?变更类别?#] &nbsp;";
                    content += "[#?变更时间?#] &nbsp;";
                    content += "[#?摘要?#] &nbsp;";
                    content += "[#?请求内容?#] &nbsp;";
                    content += "[#?影响度?#] &nbsp;";
                    content += "[#?紧急度?#] &nbsp;";
                    content += "[#?变更级别?#] &nbsp;";
                    content += "[#?变更状态?#] &nbsp;";
                    content += "[#?变更分析?#] &nbsp;";
                    content += "[#?分析结果?#] &nbsp;";
                    break;
                case 1026://事件管理                    
                    content += "[#?客户名称?#] &nbsp;";
                    content += "[#?客户地址?#] &nbsp;";
                    content += "[#?联系人?#] &nbsp;";
                    content += "[#?联系电话?#] &nbsp;";
                    content += "[#?客户部门?#] &nbsp;";
                    content += "[#?电子邮件?#] &nbsp;";
                    content += "[#?服务单位?#] &nbsp;";
                    content += "[#?职位?#] &nbsp;";
                    content += "[#?资产名称?#] &nbsp;";
                    content += "[#?登单人?#] &nbsp;";
                    content += "[#?登记时间?#] &nbsp;";
                    content += "[#?事件单号?#] &nbsp;";
                    content += "[#?事件类别?#] &nbsp;";
                    content += "[#?发生时间?#] &nbsp;";
                    content += "[#?报告时间?#] &nbsp;";
                    content += "[#?紧急度?#] &nbsp;";
                    content += "[#?影响度?#] &nbsp;";
                    content += "[#?摘要?#] &nbsp;";
                    content += "[#?需求描述?#] &nbsp;";
                    content += "[#?服务级别?#] &nbsp;";
                    content += "[#?关闭理由?#] &nbsp;";
                    content += "[#?事件来源?#] &nbsp;";
                    content += "[#?事件状态?#] &nbsp;";
                    content += "[#?完成时间?#] &nbsp;";
                    content += "[#?派出时间?#] &nbsp;";
                    content += "[#?上门时间?#] &nbsp;";
                    content += "[#?工程师?#] &nbsp;";
                    content += "[#?措施及结果?#] &nbsp;";
                    break;
                default:
                    break;
            }

            divDesc.InnerHtml = "打印配置可按照以下规则配置：<br> "+content;
        }

    }
}
