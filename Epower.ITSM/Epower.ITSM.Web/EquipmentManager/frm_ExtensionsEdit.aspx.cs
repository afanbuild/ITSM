﻿using System;
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
using Epower.ITSM.SqlDAL.Customer;
using Epower.ITSM.Base;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frm_ExtensionsEdit : BasePage
    {
        public string Opener_ClientId
        {
            set
            {
                ViewState["Opener_ClientId"] = value;
            }
            get
            {
                return (ViewState["Opener_ClientId"] == null) ? "" : ViewState["Opener_ClientId"].ToString();
            }
        }

        public bool IsNew
        {
            get
            {
                if (Request["IsNew"] != null && Request["IsNew"] == "true")
                    return true;
                else
                    return false;
            }
        }


        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.SchemaItemsMain;
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

            if (IsNew)
            {
                Master.ShowNewButton(false);
                Master.ShowDeleteButton(false);
                Master.ShowBackUrlButton(false);
            }
            if (Master.GetEditRight() == false && this.Request.QueryString["id"] != null)
            {
                this.Master.ShowNewButton(false);
                this.Master.ShowDeleteButton(false);
                this.Master.ShowSaveButton(false);
            }
        }
        #endregion

        #region Master_Master_Button_New_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("frm_ExtensionsEdit.aspx");
        }
        #endregion

        #region Master_Master_Button_Delete_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {

        }
        #endregion

        #region Master_Master_Button_GoHistory_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_GoHistory_Click()
        {
            Response.Redirect("frm_Extensions.aspx");
        }
        #endregion

        #region Page_Load
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            SetParentButtonEvent();

            ddltitemType.Attributes.Add("onchange", "CheckSchemaValuesStatus();");

            if (!IsPostBack)
            {
                if (Request["Opener_ClientId"] != null)
                {
                    Opener_ClientId = Request["Opener_ClientId"].ToString().Replace("hidClientId_ForOpenerPage", "");
                }
                LoadData();
            }
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
                Br_ExtensionsItemsDP ee = new Br_ExtensionsItemsDP();
                ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                txtFieldID.Text = ee.FieldID.ToString();
                //不能修改 字段ID 和类型

                txtFieldID.Enabled = false;
                ddltitemType.Enabled = false;
                CtrFTCHName.Value = ee.CHNAME.ToString();
                ddltitemType.SelectedValue = ee.ITEMTYPE.ToString();
            }
            else
            {
                txtFieldID.Text = Br_ExtensionsItemsDP.getMaxFiledID().ToString();
            }
        }
        #endregion

        #region InitObject
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ee"></param>
        private void InitObject(Br_ExtensionsItemsDP ee)
        {
            ee.FieldID = txtFieldID.Text.Trim();
            ee.CHNAME = CtrFTCHName.Value.Trim();
            ee.ITEMTYPE = Int32.Parse(ddltitemType.SelectedValue.Trim());
            ee.DELETED = (int)eRecord_Status.eNormal;
            ee.GROUPID = ddlType.SelectedValue != null ? Convert.ToInt32(ddlType.SelectedValue) : 0;
            ee.GroupName = ddlType.SelectedItem.Text;
        }
        #endregion

        #region Master_Master_Button_Save_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Save_Click()
        {
            Br_ExtensionsItemsDP ee = new Br_ExtensionsItemsDP();
            if (string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                //检查是否重复
               
                if (ee.CheckIsRepeat(txtFieldID.Text.Trim(), 0))
                {
                    Epower.DevBase.BaseTools.PageTool.MsgBox(this, "属性代码重复，请重新输入！");
                    this.Master.IsSaveSuccess = false;
                    return;
                }
                if (ee.CheckIsRepeatName(CtrFTCHName.Value, 0, ref ee))
                {
                    Epower.DevBase.BaseTools.PageTool.MsgBox(this, "配置项名称已经存在！");
                    this.Master.IsSaveSuccess = false;
                    if (!IsNew)
                    {
                        return;
                    }
                }
                else {
                    InitObject(ee);
                    ee.InsertRecorded(ee);
                   
                }
                this.Master.MainID = ee.ID.ToString();
                
            }
            else
            {
                //检查是否重复

                int typeid = ddlType.SelectedValue != null ? Convert.ToInt32(ddlType.SelectedValue) : 0;
                if (ee.CheckIsRepeat(txtFieldID.Text.Trim(), typeid, decimal.Parse(this.Master.MainID.Trim())))
                {
                    Epower.DevBase.BaseTools.PageTool.MsgBox(this, "属性代码重复，请重新输入！");
                    this.Master.IsSaveSuccess = false;
                    return;
                }
                if (ee.CheckIsRepeatName(CtrFTCHName.Value, decimal.Parse(this.Master.MainID.Trim()), ref ee))
                {
                    Epower.DevBase.BaseTools.PageTool.MsgBox(this, "配置项名称已经存在！");
                    if (!IsNew)
                    {
                        return;
                    }
                }
                else {
                    ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                    InitObject(ee);
                    ee.UpdateRecorded(ee);
                    
                }
                
                this.Master.MainID = ee.ID.ToString();
            }
            //强制相关缓存失效 
            HttpRuntime.Cache.Insert("CommCacheValidBrExtensionsItems", false);
            if (IsNew)
            {
                System.Text.StringBuilder sbText = new System.Text.StringBuilder();
                sbText.Append("<script>");
                sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidTempID", ee.FieldID.Trim());
                sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidExtendsID", ee.ID);
                sbText.AppendFormat("window.opener.document.all.{0}.click();", Opener_ClientId + "btnAddNewItem");

               
                // 关闭窗口
                sbText.Append("top.close();");
                sbText.Append("</script>");
                // 向客户端发送
                Page.ClientScript.RegisterStartupScript(GetType(), DateTime.Now.ToString(), sbText.ToString());
                
            }

        }
        #endregion


    }
}
