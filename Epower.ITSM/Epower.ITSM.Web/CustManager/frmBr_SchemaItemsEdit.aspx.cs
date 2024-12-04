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

using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;

namespace Epower.ITSM.Web.CustManager
{
    public partial class frmBr_SchemaItemsEdit : BasePage
    {


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
        #region 获得传过来的参数用来判断父窗体的控件id的名称的相同部分
        /// <summary>
        /// 获得传过来的参数用来判断父窗体的控件id的名称的相同部分
        /// </summary>
        public string Opener_ClientId
        {

            get
            {
                return (Request["Opener_ClientId"] == null) ? "" : Request["Opener_ClientId"].ToString().Replace("hidClientId_ForOpenerPage", "");
            }
        }
        #endregion


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
            Response.Redirect("frmBr_SchemaItemsEdit.aspx");
        }
        #endregion

        #region Master_Master_Button_Delete_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            if (!String.IsNullOrEmpty(this.Master.MainID.ToString()))
            {
                Equ_SchemaItemsDP ee = new Equ_SchemaItemsDP();
                ee.DeleteRecorded2(long.Parse(this.Master.MainID.Trim()));

                //强制相关缓存失效 
                HttpRuntime.Cache.Insert("CommCacheValidBrSchemaItem", false);

                Master_Master_Button_GoHistory_Click();
            }
        }
        #endregion

        #region Master_Master_Button_GoHistory_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_GoHistory_Click()
        {
            Response.Redirect("frmBr_SchemaItemsMain.aspx");
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
                Br_SchemaItemsDP ee = new Br_SchemaItemsDP();
                ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                txtFieldID.Text = ee.FieldID.ToString();
                //不能修改 字段ID 和类型
                txtFieldID.Enabled = false;
                ddltitemType.Enabled = false;
                CtrFTCHName.Value = ee.CHName.ToString();
                ddltitemType.SelectedValue = ee.itemType.ToString();
            }
            else
            {
                txtFieldID.Text = Br_SchemaItemsDP.getMaxFiledID().ToString();
            }
        }
        #endregion

        #region InitObject
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ee"></param>
        private void InitObject(Br_SchemaItemsDP ee)
        {
            ee.FieldID = txtFieldID.Text.Trim();
            ee.CHName = CtrFTCHName.Value.Trim();
            ee.itemType = Int32.Parse(ddltitemType.SelectedValue.Trim());
        }
        #endregion

        #region Master_Master_Button_Save_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Save_Click()
        {
            Br_SchemaItemsDP ee = new Br_SchemaItemsDP();
            if (string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                //检查是否重复
                if (ee.CheckIsRepeat(txtFieldID.Text.Trim(), 0))
                {
                    Epower.DevBase.BaseTools.PageTool.MsgBox(this, "属性代码重复，请重新输入！");
                    this.Master.IsSaveSuccess = false;
                    return;
                }
                InitObject(ee);
                ee.Deleted = (int)eRecord_Status.eNormal;
                ee.InsertRecorded(ee);

                this.Master.MainID = ee.ID.ToString();
            }
            else
            {
                //检查是否重复
                if (ee.CheckIsRepeat(txtFieldID.Text.Trim(), decimal.Parse(this.Master.MainID.Trim())))
                {
                    Epower.DevBase.BaseTools.PageTool.MsgBox(this, "属性代码重复，请重新输入！");
                    this.Master.IsSaveSuccess = false;
                    return;
                }
                ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                InitObject(ee);
                ee.UpdateRecorded(ee);
                this.Master.MainID = ee.ID.ToString();
            }

            //强制相关缓存失效 
            HttpRuntime.Cache.Insert("CommCacheValidBrSchemaItem", false);
            if (IsNew)
            {
                System.Text.StringBuilder sbText = new System.Text.StringBuilder();
                sbText.Append("<script>");
                sbText.Append("var value;");
                // FieldID
                sbText.Append("value ='" + ee.FieldID.Trim() + "';");
                sbText.Append("var objTemp = window.opener.document.getElementById('" + Opener_ClientId + "hidTempID');");
                sbText.Append(" var objBtn = window.opener.document.getElementById('" + Opener_ClientId + "btnAddNewItem'); ");
                sbText.Append("if (value != '') {");

                sbText.Append("objTemp.value = value;");
                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidTempID').value=value;");
                sbText.Append(" }else {");

              //  sbText.Append("  objTemp.value = '0'; ");

                sbText.Append("}");
                sbText.Append(" if (objTemp.value != '0' && objTemp.value != '') {  ");
                sbText.Append("objBtn.click(); ");
                sbText.Append("}");

                // 关闭窗口
                sbText.Append("window.close();");
                sbText.Append("</script>");
                // 向客户端发送
                Page.ClientScript.RegisterStartupScript(GetType(), DateTime.Now.ToString(), sbText.ToString());
            }

        }
        #endregion
    }
}
