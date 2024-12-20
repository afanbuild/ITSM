/*******************************************************************
•	 * 版权所有：深圳市非凡信息技术有限公司
•	 * 描述：需求请求模板编辑页面
•	
•	 * 
•	 * 
•	 * 创建人：余向前
•	 * 创建日期：2013-04-26
 * *****************************************************************/

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

using EpowerCom;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.Demand
{
    public partial class frm_Demand_Template : BasePage
    {
        #region 属性定义

        public string sApplicationUrl = Epower.ITSM.Base.Constant.ApplicationPath;     //虚拟路径
        RightEntity re = null;

        /// <summary>
        /// 
        /// </summary>
        protected bool IsShow
        {
            get { if (Request["IsShow"] != null) return true; else return false; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string TemplateID
        {
            get
            {
                return this.Master.MainID.ToString();
            }
        }

        #endregion

        #region 设置父窗体按钮事件

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
            if (string.IsNullOrEmpty(this.Master.MainID.ToString()))
                this.Master.ShowAddPageButton();
            else
                this.Master.ShowEditPageButton();

            if (IsShow)
            {
                this.Master.ShowNewButton(false);
                this.Master.ShowDeleteButton(false);
                this.Master.ShowSaveButton(false);
                IsCanEdit();
            }
        }

        #endregion

        #region 新增

        /// <summary>
        /// 新增
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("frm_Demand_Template.aspx");
        }

        #endregion

        #region 删除

        /// <summary>
        /// 删除
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            if (!String.IsNullOrEmpty(this.Master.MainID.ToString()))
            {
                EA_ShortCutTemplateDP ee = new EA_ShortCutTemplateDP();
                ee.DeleteRecorded(long.Parse(this.Master.MainID.Trim()));

                //返回主页面
                Master_Master_Button_GoHistory_Click();
            }
        }

        #endregion

        #region 保存

        /// <summary>
        /// 保存
        /// </summary>
        void Master_Master_Button_Save_Click()
        {
            #region 判断模板名称是否存在 yxq 2011-09-27
            long id = this.Master.MainID.Trim() == "" ? 0 : long.Parse(this.Master.MainID.Trim());
            string tname = CtrFTTemplateName.Value.Trim();
            string sWhere = " And TemplateID<>" + id + " and TemplateName=" + StringTool.SqlQ(tname);
            if (EA_ShortCutTemplateDP.IsExist(sWhere))
            {
                PageTool.MsgBox(Page, "已经存在模板名为" + tname + "的模板,请重新输入!");
                this.Master.IsSaveSuccess = false;
                return;
            }
            #endregion

            #region 判断是否选择了流程模型
            if (hidFlowModel.Value.Trim() == string.Empty || hidFlowModel.Value.Trim() == "0")
            {
                PageTool.MsgBox(Page, "请选择流程模型!");
                this.Master.IsSaveSuccess = false;
                return;
            }
            #endregion


            if (string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                EA_ShortCutTemplateDP ee = new EA_ShortCutTemplateDP();

                InitObject(ee);
                ee.InsertRecorded(ee);
                this.Master.MainID = ee.TemplateID.ToString().Trim();
            }
            else
            {
                EA_ShortCutTemplateDP ee = new EA_ShortCutTemplateDP();
                ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                InitObject(ee);
                ee.UpdateRecorded(ee);
                this.Master.MainID = ee.TemplateID.ToString();
            }
            //LoadData();
        }

        #endregion

        #region 返回

        /// <summary>
        /// 返回
        /// </summary>
        void Master_Master_Button_GoHistory_Click()
        {
            if (!IsShow)
                Response.Redirect("frm_Demand_TemplateMain.aspx");
            else
                Response.Write("<script>window.close();</script>");
        }

        #endregion

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            //设置主页面
            SetParentButtonEvent();
            if (!IsPostBack)
            {
                //初始化需求类别根列表下拉内容
                InitServRootType();
                LoadData();
            }
            else
            {
                LoadFlowModels(rblTempOwner.SelectedValue); //重新绑定下拉框 防止回发更新了下拉框的绑定值
                ddlFlowModel.SelectedIndex = ddlFlowModel.Items.IndexOf(ddlFlowModel.Items.FindByValue(hidFlowModel.Value.ToString()));//赋值下拉列表                
            }
        }

        #endregion

        #region 初始化事件类别根列表下拉内容
        /// <summary>
        /// 初始化事件类别根列表下拉内容
        /// </summary>
        private void InitServRootType()
        {
            #region 需求类别
            DataTable dt = CatalogDP.GetCatasByRootID(1003);

            ddlDemandRootType.DataValueField = "CatalogID";
            ddlDemandRootType.DataTextField = "CatalogName";
            ddlDemandRootType.DataSource = dt.DefaultView;
            ddlDemandRootType.DataBind();

            ddlDemandRootType.Items.Insert(0, new ListItem("--顶级需求类别--", "1003"));

            #endregion           
        }
        #endregion

        #region 获取流程模型

        /// <summary>
        /// 获取流程模型
        /// </summary>
        /// <param name="strOwner"></param>
        private void LoadFlowModels(string strOwner)
        {
            long lngOwner = long.Parse(strOwner);
            ddlFlowModel.Items.Clear();
            DataTable dt = AppFieldConfigDP.GetFlowModelList(1062, (long)Session["UserID"], lngOwner);
            ddlFlowModel.DataSource = dt.DefaultView;
            ddlFlowModel.DataTextField = "FlowName";
            ddlFlowModel.DataValueField = "OFlowModelID";
            ddlFlowModel.DataBind();
            ddlFlowModel.Items.Insert(0, new ListItem("", ""));            
        }

        #endregion

        #region 加载数据

        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadData()
        {
            if (!string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                EA_ShortCutTemplateDP ee = new EA_ShortCutTemplateDP();
                ee = ee.GetReCorded(long.Parse(this.Master.MainID));
                CtrFTTemplateName.Value = ee.TemplateName;

                rblTempOwner.SelectedIndex = rblTempOwner.Items.IndexOf(rblTempOwner.Items.FindByValue(ee.Owner.ToString())); //赋值单选按钮

                LoadFlowModels(rblTempOwner.SelectedValue);

                SetXmlObjectValue(ee.TemplateXml);//根据XML对控件赋值

                ddlFlowModel.SelectedIndex = ddlFlowModel.Items.IndexOf(ddlFlowModel.Items.FindByValue(ee.OFlowModelID.ToString()));//赋值下拉列表                    
                hidFlowModel.Value = ee.OFlowModelID.ToString();
            }
            else
            {
                LoadFlowModels(rblTempOwner.SelectedValue);
            }
        }

        #endregion

        #region 根据XML对控件赋值

        /// <summary>
        /// 根据XML对控件赋值
        /// </summary>
        /// <param name="sXml"></param>
        private void SetXmlObjectValue(string sXml)
        {
            if (sXml.Length == 0)
                return;

            FieldValues fv = new FieldValues(sXml);           

            #region  事件类别
            if (fv.GetFieldValue("DemandRootTypeID") != null)
            {
                if (fv.GetFieldValue("DemandRootTypeID").Value != "0" && fv.GetFieldValue("DemandRootTypeID").Value != "")
                {
                    ddlDemandRootType.SelectedIndex = ddlDemandRootType.Items.IndexOf(ddlDemandRootType.Items.FindByValue(fv.GetFieldValue("DemandRootTypeID").Value));      //父
                    ctrFlowDemandType.Visible = true;

                    //判断选择的类别下是否有子类别，若有，则绑定；否则，不绑定             
                    DataTable dt = CatalogDP.GetCatasByRootID(long.Parse(ddlDemandRootType.SelectedValue));
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //绑定子类别的根
                        ctrFlowDemandType.RootID = long.Parse(ddlDemandRootType.SelectedItem.Value);
                    }

                    if (fv.GetFieldValue("DemandTypeID").Value != "0" && fv.GetFieldValue("DemandTypeID").Value != "")
                    {
                        //如果有默认初值，则设置选定的默认值
                        ctrFlowDemandType.CatelogID = long.Parse(fv.GetFieldValue("DemandTypeID").Value);
                        ctrFlowDemandType.CatelogValue = fv.GetFieldValue("DemandType").Value;
                    }
                }
            }
            #endregion
        }

        #endregion

        #region 数据初化数据

        /// <summary>
        /// 数据初化数据
        /// </summary>
        /// <param name="ee"></param>
        private void InitObject(EA_ShortCutTemplateDP ee)
        {
            //设置对应的应用ID
            ee.AppID = 1062; //不同模板页面对应的应用ID不同，需要修改

            ee.OFlowModelID = long.Parse(hidFlowModel.Value);
            ee.TemplateType = (int)e_ITSMShortCutReqType.eitsmscrtIssue;   //这里暂时是固定值
                       
            if (rblTempOwner.Items[0].Selected == true)
                ee.Owner = 0;
            else if (rblTempOwner.Items[1].Selected == true)
                ee.Owner = 3;           

            ee.TemplateName = CtrFTTemplateName.Value.Trim();
            ee.TemplateXml = GetFieldsXml();   //获取XML
        }
        #endregion

        #region 获取数据

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        private string GetFieldsXml()
        {
            FieldValues fv = new FieldValues();
           
            #region  事件类别

            //父
            fv.Add("DemandRootTypeID", ddlDemandRootType.SelectedItem.Value.ToString().Trim());
            fv.Add("DemandRootType", ddlDemandRootType.SelectedItem.Text.Trim());

            //子
            fv.Add("DemandTypeID", ctrFlowDemandType.CatelogID.ToString().Trim());
            fv.Add("DemandType", ctrFlowDemandType.CatelogValue.Trim());

            #endregion
           
            return fv.GetXmlObject().InnerXml;
        }

        #endregion

        #region 事件类别更改时
        /// <summary>
        /// 事件类别更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlDemandRootType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDemandRootType.SelectedValue != "0")
            {
                //判断选择的类别下是否有子类别，若有，则绑定；否则，不绑定             
                DataTable dt = CatalogDP.GetCatasByRootID(long.Parse(ddlDemandRootType.SelectedValue));
                if (dt != null && dt.Rows.Count > 0)
                {
                    ctrFlowDemandType.Visible = true;
                    ctrFlowDemandType.RootID = long.Parse(ddlDemandRootType.SelectedValue);
                }
                else
                {
                    ctrFlowDemandType.RootID = 1003;
                    ddlDemandRootType.SelectedIndex = ddlDemandRootType.Items.IndexOf(ddlDemandRootType.Items.FindByValue("1003"));
                    PageTool.MsgBox(Page, "此需求类别下没有子级,不能选择为父类!");
                }
            }
            else
            {
                ctrFlowDemandType.RootID = 1003;
            }

            ctrFlowDemandType.CatelogID = 0;
            ctrFlowDemandType.CatelogValue = "";
        }
        #endregion       

        #region 是否能编辑

        /// <summary>
        /// 是否能编辑
        /// </summary>
        private void IsCanEdit()
        {
            CtrFTTemplateName.ContralState = eOA_FlowControlState.eReadOnly;
            rblTempOwner.Enabled = false;
            ddlFlowModel.Enabled = false;
            ddlDemandRootType.Enabled = false;
            ctrFlowDemandType.ContralState = eOA_FlowControlState.eReadOnly;           
        }

        #endregion
    }
}
