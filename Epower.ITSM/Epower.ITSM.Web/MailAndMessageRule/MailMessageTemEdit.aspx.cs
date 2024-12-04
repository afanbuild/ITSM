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
    /// MailMessageTemEdit 的摘要说明。
    /// </summary>
    public partial class MailMessageTemEdit : BasePage
    {
        #region SetParentButtonEvent
        /// <summary>
        /// 设置父窗体按钮事件
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
        /// 新增
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
        /// 删除
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            if (!String.IsNullOrEmpty(this.Master.MainID.ToString()))
            {
                MailMessageTemManagerDP ee = new MailMessageTemManagerDP();
                ee.DeleteRecorded(long.Parse(this.Master.MainID.Trim()));
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
                    Epower.DevBase.BaseTools.PageTool.MsgBox(this.Page, "插入失败邮件模板内容与短信模板内容不能重复！");
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
                    Epower.DevBase.BaseTools.PageTool.MsgBox(this.Page, "更新失败邮件模板内容与短信模板内容不能重复！");
                    return;
                }
                this.Master.MainID = ee.ID.ToString();
            }

        }

        #endregion

        #region Master_Master_Button_GoHistory_Click

        /// <summary>
        /// 返回
        /// </summary>
        void Master_Master_Button_GoHistory_Click()
        {
            Response.Redirect("MailMessageTemManager.aspx");

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
                //labModeCoumList.Text = CommonDP.GetConfigValue("Other", "MailModeCoumList");

                ddlBind();
                //加载数据
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

            //cboApp.Items.Remove(new ListItem("通用流程", "199"));
            cboApp.Items.Remove(new ListItem("进出操作间", "1027"));

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
        /// 获取数据
        /// </summary>
        /// <param name="ee"></param>
        private void LoadData(MailMessageTemManagerDP ee)
        {
            CtrTemplateName.Value = ee.TemplateName;//模块名称
            leaderMailContent.Text = ee.leadercontent;//领导模版内容
            FTBMailContent.Text = ee.MailContent;//邮件模板内容
            FTBMailContent2.Text = ee.DealMainContent; //处理人邮件模板内容
            txtModelContent.Text = ee.ModelContent;//短信模板内容
            ddlStatus.SelectedValue = ee.Status.ToString();//是否有效
            txtRemark.Text = ee.Remark;//备注
            cboApp.SelectedIndex = cboApp.Items.IndexOf(cboApp.Items.FindByValue(ee.SystemID.ToString()));


            getDesc(long.Parse(ee.SystemID.ToString()));//获取配置说明

            #region 判断此模板是否已经在规则里用到 yxq
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
            ee.TemplateName = CtrTemplateName.Value.Trim().ToString();//模块名称
            ee.leadercontent = leaderMailContent.Text.Trim().ToString();//领导模版内容
            ee.MailContent = FTBMailContent.Text.Trim().ToString();//邮件模板内容
            ee.DealMainContent = FTBMailContent2.Text.Trim().ToString(); //处理人邮件模板内容
            ee.ModelContent = txtModelContent.Text.Trim().ToString();//短信模板内容
            ee.Status = Decimal.Parse(ddlStatus.SelectedItem.Value);//是否有效
            ee.Remark = txtRemark.Text.Trim().ToString();//备注
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
            divDesc.InnerHtml = "短信邮件配置可按照以下规则配置：<br> " + content;
        }

        protected void getDesc(long AppID)
        {

            string content = "";
            switch (AppID)
            {

                case 1028://发布管理
                    content += "[#?信息标题?#] &nbsp;";
                    content += "[#?信息类别?#] &nbsp;";
                    content += "[#?发布人?#] &nbsp;";
                    content += "[#?发布时间?#] &nbsp;";
                    content += "[#?截止时间?#] &nbsp;";
                    content += "[#?具体内容?#] &nbsp;";
                    break;
                case 201://自定义表单流程
                    break;
                case 400://知识管理                  
                    content += "[#?主题?#] &nbsp;";
                    content += "[#?关键字?#] &nbsp;";
                    content += "[#?摘要?#] &nbsp;";
                    content += "[#?知识类别?#] &nbsp;";
                    content += "[#?知识内容?#] &nbsp;";
                    break;
                case 199://通用流程 

                    LoadFlowModelID();
                    trFlowModelID.Visible = true;

                    content = LoadNormalAppDisplayName();

                    break;
                case 210://问题管理
                    content += "[#?问题单号?#] &nbsp;";
                    content += "[#?登记人?#] &nbsp;";
                    content += "[#?登记人部门?#] &nbsp;";
                    content += "[#?登记时间?#] &nbsp;";
                    content += "[#?影响度?#] &nbsp;";
                    content += "[#?紧急度?#] &nbsp;";
                    content += "[#?标题?#] &nbsp;";
                    content += "[#?问题描述?#] &nbsp;";
                    content += "[#?问题状态?#] &nbsp;";
                    content += "[#?解决方案?#] &nbsp;";                    

                    break;
                case 410://资产巡检 
                    content += "[#?标题?#] &nbsp;";
                    content += "[#?登记人?#] &nbsp;";
                    content += "[#?登记部门?#] &nbsp;";
                    content += "[#?登记时间?#] &nbsp;";
                    content += "[#?备注?#] &nbsp;";
                    break;
                case 420://变更管理
                    content += "[#?变更单号?#] &nbsp;";
                    content += "[#?客户名称?#] &nbsp;";
                    content += "[#?客户电话?#] &nbsp;";
                    content += "[#?变更级别?#] &nbsp;";
                    content += "[#?变更状态?#] &nbsp;";
                    content += "[#?影响度?#] &nbsp;";
                    content += "[#?摘要?#] &nbsp;";
                    content += "[#?请求内容?#] &nbsp;";
                    content += "[#?紧急度?#] &nbsp;";                    
                    break;
                case 1026://事件管理
                    content += "[#?事件单号?#] &nbsp;";
                    content += "[#?客户名称?#] &nbsp;";
                    content += "[#?客户手机?#] &nbsp;";
                    content += "[#?客户邮件?#] &nbsp;";
                    content += "[#?登记时间?#] &nbsp;";
                    content += "[#?发生时间?#] &nbsp;";
                    content += "[#?报告时间?#] &nbsp;";
                    content += "[#?事件类别?#] &nbsp;";
                    content += "[#?服务级别?#] &nbsp;";
                    content += "[#?影响范围?#] &nbsp;";
                    content += "[#?紧急度?#] &nbsp;";
                    content += "[#?摘要?#] &nbsp;";
                    content += "[#?详细描述?#] &nbsp;";                    
                    break;
                default:
                    break;
            }

            divDesc.InnerHtml = "短信邮件配置可按照以下规则配置：<br> " + content;
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
            String content = "通用表单固定字段：[#?流程名?#], [#?登记人?#], [#?登记日期?#], [#?登记部门?#] 自定义字段：";

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
