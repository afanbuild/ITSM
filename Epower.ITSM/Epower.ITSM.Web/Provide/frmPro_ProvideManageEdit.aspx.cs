/****************************************************************************
 * 
 * description:供应商管理主表单
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-09-17
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
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.Provide
{
    /// <summary>
    /// 
    /// </summary>
    public partial class frmPro_ProvideManageEdit : BasePage
    {

        /// <summary>
        /// 是否只读
        /// </summary>
        protected bool IsReadyOnly
        {
            get
            {
                if (Request["ReadyOnly"] != null)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// 是否打开新窗口,新窗口返回时关闭
        /// </summary>
        protected bool IsNewWin
        {
            get { if (Request["newWin"] != null) return true; else return false; }
        }

        #region
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
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

            if (IsReadyOnly)
            {
                this.Master.ShowNewButton(false);
                this.Master.ShowDeleteButton(false);
                this.Master.ShowSaveButton(false);
            }
            if (Master.GetEditRight() == false && this.Request.QueryString["id"] != null)
            {
                this.Master.ShowNewButton(false);
                this.Master.ShowDeleteButton(false);
                this.Master.ShowSaveButton(false);
            }

        }
        #endregion

        #region
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("frmPro_ProvideManageEdit.aspx");
        }
        #endregion

        #region
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            if (!String.IsNullOrEmpty(this.Master.MainID.ToString()))
            {
                if (Equ_DeskDP.getProvideName(this.Master.MainID.Trim()) != true)
                {
                    Pro_ProvideManageDP ee = new Pro_ProvideManageDP();
                    ee.DeleteRecorded(long.Parse(this.Master.MainID.Trim()));
                    Master_Master_Button_GoHistory_Click();
                }
                else
                {
                    PageTool.MsgBox(this, "供应商已经在资产中用到，不能删除！");
                }
            }
        }
        #endregion

        #region
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_GoHistory_Click()
        {
            if (IsNewWin) //关闭窗口的情况
            {
                PageTool.AddJavaScript(this, "window.close();");
                return;
            }

            Response.Redirect("frmPro_ProvideManageMain.aspx");
        }
        #endregion

        #region
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            SetParentButtonEvent();
            RightEntity re = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.PrivideManager];
            if (re != null)
                this.Master.ShowDeleteButton(re.CanDelete);
            else
                this.Master.ShowDeleteButton(false);

                if (!IsPostBack)
                {
                    LoadData();

                    if (IsReadyOnly)
                        SetFormReadOnly();
                }
        }
        #endregion

        #region
        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            if (!string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                Pro_ProvideManageDP ee = new Pro_ProvideManageDP();
                ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                txtName.Text = ee.Name.ToString();
                txtCode.Text = ee.Code.ToString();
                txtContract.Text = ee.Contract.ToString();
                txtContractTel.Text = ee.ContractTel.ToString();
                txtAddress.Text = ee.Address.ToString();
                txtRemark.Text = ee.Remark.ToString();
                txtContent.Text = ee.ContactLevel.ToString();

                lblName.Text = ee.Name.ToString();
                lblCode.Text = ee.Code.ToString();
                lblContract.Text = ee.Contract.ToString();
                lblContractTel.Text = ee.ContractTel.ToString();
                lblAddress.Text = ee.Address.ToString();
                lblRemark.Text = ee.Remark.ToString();
                lblContent.Text = ee.ContactLevel.ToString();
            }
        }
        #endregion

        #region  SetFormReadOnly
        /// <summary>
        /// 
        /// </summary>
        private void SetFormReadOnly()
        {
            lblWarning.Visible = false;
            txtName.Visible = false;
            txtCode.Visible = false;
            txtContract.Visible = false;
            txtContractTel.Visible = false;
            txtAddress.Visible = false;
            txtRemark.Visible = false;
            txtContent.Visible = false;

            lblName.Visible = true;
            lblCode.Visible = true;
            lblContract.Visible = true;
            lblContractTel.Visible = true;
            lblAddress.Visible = true;
            lblRemark.Visible = true;
            lblContent.Visible = true;

        }
        #endregion 


        #region
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ee"></param>
        private void InitObject(Pro_ProvideManageDP ee)
        {
            ee.Name = txtName.Text.Trim();
            ee.Code = txtCode.Text.Trim();
            ee.Contract = txtContract.Text.Trim();
            ee.ContractTel = txtContractTel.Text.Trim();
            ee.Address = txtAddress.Text.Trim();
            ee.Remark = txtRemark.Text.Trim();
            ee.ContactLevel = txtContent.Text.Trim();
        }
        #endregion

        #region
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Save_Click()
        {
            if (string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                Pro_ProvideManageDP ee = new Pro_ProvideManageDP();
                InitObject(ee);
                ee.Deleted = (int)eRecord_Status.eNormal;
                ee.RegUserID = long.Parse(Session["UserID"].ToString());
                ee.RegUserName = Session["PersonName"].ToString();
                ee.RegTime = DateTime.Now;
                ee.InsertRecorded(ee);

                this.Master.MainID = ee.ID.ToString();
            }
            else
            {
                Pro_ProvideManageDP ee = new Pro_ProvideManageDP();
                ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                InitObject(ee);
                ee.UpdateRecorded(ee);
                this.Master.MainID = ee.ID.ToString();
            }
        }
        #endregion
    }
}