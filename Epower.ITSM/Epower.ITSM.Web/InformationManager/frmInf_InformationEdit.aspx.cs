/****************************************************************************
 * 
 * description:
 * 
 * 
 * 
 * Create by:
 * Create Date:
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

using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.InformationManager
{
    public partial class frmInf_InformationEdit : BasePage
    {
        #region 属性
        /// <summary>
        /// 设备分类ID
        /// </summary>
        protected string CatalogID
        {
            get
            {
                return CtrKBCata1.CatelogID.ToString();
            }
        }
        #endregion 

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.InfManager;
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
        }
        #endregion

        #region Master_Master_Button_New_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("frmInf_InformationEdit.aspx?subjectid=" + CatalogID);
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
                Inf_InformationDP ee = new Inf_InformationDP();
                ee.DeleteRecorded(long.Parse(this.Master.MainID.Trim()));
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
            if (Request["IsTanChu"] != null && Request["IsTanChu"].ToString() == "true")
            {
                PageTool.AddJavaScript(this, "window.close();");
                return; 
            }
            else
            {
                Response.Redirect("frmInf_InformationMain.aspx?subjectid=" + CatalogID);
            }
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
            CtrKBCata1.RootID = 1;
            if (!IsPostBack)
            {
                //设置显示
                PageDeal.SetLanguage(this.Controls[0]);

                

                if (Request["subjectid"] != null)
                {
                    CtrKBCata1.CatelogID = decimal.Parse(Request["subjectid"].ToString());
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
                Inf_InformationDP ee = new Inf_InformationDP();
                ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                txtTitle.Text = ee.Title.ToString();
                txtPKey.Text = ee.PKey.ToString();
                UEditor1.Content = ee.Content.ToString();
                Ctrattachment1.AttachmentType = eOA_AttachmentType.eKB;
                Ctrattachment1.FlowID = (long)ee.ID;
                CtrKBCata1.CatelogID = ee.Type;
                txtTags.Text = ee.Tags.ToString();
                txtListName.Text = ee.ListName;
                lblListName.Text = ee.ListName;
                hidListID.Value = ee.ListID.ToString();
                hidEqu.Value = ee.EquID.ToString();//资产ID
                hidEquName.Value = ee.EquName;//资产名称
                txtEqu.Text = ee.EquName;
                lblEqu.Text = ee.EquName;
            }
        }
        #endregion

        #region InitObject
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ee"></param>
        private void InitObject(Inf_InformationDP ee)
        {
            ee.Title = txtTitle.Text.Trim();
            ee.PKey = txtPKey.Text.Trim();
            ee.Content = UEditor1.Content.Trim();
            ee.PlainContent = UEditorPlainContent.Value.Trim();    // 存储无格式的 RichText 内容.
            ee.Type = CtrKBCata1.CatelogID;
            ee.TypeName = CtrKBCata1.CatelogValue.Trim();
            ee.Tags = txtTags.Text.Trim();
            ee.ListID = decimal.Parse(hidListID.Value.Trim() == "" ? "0" : hidListID.Value.Trim());
            ee.ListName = hidListName.Value.Trim();
            ee.EquID = decimal.Parse(hidEqu.Value.Trim() == "" ? "0" : hidEqu.Value.Trim());
            ee.EquName = hidEquName.Value.Trim();            
        }
        #endregion

        #region Master_Master_Button_Save_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Save_Click()
        {            

            #region 判断主题是否为空
            if (txtTitle.Text.Trim() == string.Empty)
            {
                PageTool.MsgBox(this, "主题不能为空！");
                this.Master.IsSaveSuccess = false;
                return;
            }
            #endregion

            #region 判断知识内容是否为空
            if (UEditor1.Content.Trim() == string.Empty)
            {
                PageTool.MsgBox(this, "知识内容不能为空！");
                this.Master.IsSaveSuccess = false;
                return;
            }
            #endregion


            if (string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                Inf_InformationDP ee = new Inf_InformationDP();
                InitObject(ee);
                ee.Deleted = (int)eRecord_Status.eNormal;
                ee.RegUserID = long.Parse(Session["UserID"].ToString());
                ee.RegUserName = Session["PersonName"].ToString();
                ee.RegTime = DateTime.Now;
                ee.UpdateUserID = long.Parse(Session["UserID"].ToString());
                ee.UpdateUserName = Session["PersonName"].ToString();
                ee.UpdateTime = DateTime.Now;
                ee.AttachXml = Ctrattachment1.AttachXML;
                ee.InsertRecorded(ee);
                this.Master.MainID = ee.ID.ToString();
            }
            else
            {
                Inf_InformationDP ee = new Inf_InformationDP();
                ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                InitObject(ee);
                ee.UpdateUserID = long.Parse(Session["UserID"].ToString());
                ee.UpdateUserName = Session["PersonName"].ToString();
                ee.UpdateTime = DateTime.Now;
                ee.AttachXml = Ctrattachment1.AttachXML;
                ee.UpdateRecorded(ee);
                this.Master.MainID = ee.ID.ToString();
            }
        }
        #endregion
    }
}
