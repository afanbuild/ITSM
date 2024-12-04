/****************************************************************************
 * 
 * description:联系人新增修改删除
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-10-24
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

namespace Epower.ITSM.Web.AppForms
{
    public partial class frmBr_ContactEdit : BasePage
    {
        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.ServiceDept;
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
            Response.Redirect("frmBr_ContactEdit.aspx?CustomID=" + this.hidServic.Value.ToString());
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
                Br_ContactDP ee = new Br_ContactDP();
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
            Response.Redirect("frmBr_ContactMain.aspx?CustomID=" + this.hidServic.Value.ToString());
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
            if (!IsPostBack)
            {
                if (Request["CustomID"] != null && Request["CustomID"].ToString()!=string.Empty)
                {
                    hidServic.Value = Request["CustomID"].ToString();
                    Br_MastCustomerDP ee = new Br_MastCustomerDP();
                    ee = ee.GetReCorded(long.Parse(hidServic.Value.Trim()));
                    hidServicName.Value = ee.ShortName;
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
                Br_ContactDP ee = new Br_ContactDP();
                ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                hidServic.Value = ee.CustomID.ToString(); 
                hidServicName.Value = ee.CustomName.ToString();
                txtContactName.Text = ee.ContactName.ToString();
                txtCPositionName.Text = ee.CPositionName.ToString();
                ddltSex.SelectedIndex = ddltSex.Items.IndexOf(ddltSex.Items.FindByValue(ee.Sex.ToString()));
                txtInterest.Text = ee.Interest.ToString();
                dtBirthday.dateTime = ee.Birthday;
                txtOfficeTel.Text = ee.OfficeTel.ToString();
                txtFamilyTel.Text = ee.FamilyTel.ToString();
                txtMobil.Text = ee.Mobil.ToString();
                txtEMail.Text = ee.EMail.ToString();
                txtFax.Text = ee.Fax.ToString();
                txtQQ.Text = ee.QQ.ToString();
                txtMsn.Text = ee.Msn.ToString();
                txtAddress.Text = ee.Address.ToString();
                txtPostNo.Text = ee.PostNo.ToString();
                txtRemark.Text = ee.Remark.ToString();
            }
        }
        #endregion

        #region InitObject
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ee"></param>
        private void InitObject(Br_ContactDP ee)
        {
            ee.CustomID = decimal.Parse(hidServic.Value);
            ee.CustomName = hidServicName.Value.Trim();
            ee.ContactName = txtContactName.Text.Trim();
            ee.CPositionName = txtCPositionName.Text.Trim();
            ee.Sex = Int32.Parse(ddltSex.SelectedValue);
            ee.Interest = txtInterest.Text.Trim();
            ee.Birthday = dtBirthday.dateTime;
            ee.OfficeTel = txtOfficeTel.Text.Trim();
            ee.FamilyTel = txtFamilyTel.Text.Trim();
            ee.Mobil = txtMobil.Text.Trim();
            ee.EMail = txtEMail.Text.Trim();
            ee.Fax = txtFax.Text.Trim();
            ee.QQ = txtQQ.Text.Trim();
            ee.Msn = txtMsn.Text.Trim();
            ee.Address = txtAddress.Text.Trim();
            ee.PostNo = txtPostNo.Text.Trim();
            ee.Remark = txtRemark.Text.Trim();
        }
        #endregion

        #region Master_Master_Button_Save_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Save_Click()
        {
            if (string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                Br_ContactDP ee = new Br_ContactDP();
                InitObject(ee);
                ee.Deleted = (int)eRecord_Status.eNormal;
                ee.RegUserID = long.Parse(Session["UserID"].ToString());
                ee.RegUserName = Session["PersonName"].ToString();
                ee.RegTime = DateTime.Now;
                //ee.RegDeptID = long.Parse(Session["UserDeptID"].ToString());
                //ee.RegDeptName = Session["UserDeptName"].ToString();
                ee.InsertRecorded(ee);

                this.Master.MainID = ee.ID.ToString();
            }
            else
            {
                Br_ContactDP ee = new Br_ContactDP();
                ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                InitObject(ee);
                ee.UpdateRecorded(ee);
                this.Master.MainID = ee.ID.ToString();
            }
        }
        #endregion
    }
}
