/*******************************************************************
 *
 * Description:设置桌面项
 * 
 * 
 * Create By  :zhumc
 * Create Date:2007年11月29日
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

using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;

namespace Epower.ITSM.Web.mydestop
{
    public partial class frmEa_DefineMainPageEdit : BasePage
    {
        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            if (this.Request.QueryString["id"] != null)
            {
                this.Master.MainID = this.Request.QueryString["id"].ToString();
            }
            this.Master.OperatorID = Constant.SystemManager;
            this.Master.IsCheckRight = true;
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
            Response.Redirect("frmEa_DefineMainPageEdit.aspx");
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
                Ea_DefineMainPageDP ee = new Ea_DefineMainPageDP();
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
            Response.Redirect("frmEa_DefineMainPageMain.aspx");
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
                Ea_DefineMainPageDP ee = new Ea_DefineMainPageDP();
                ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                txtiOrder.Text = ee.iOrder.ToString();
                rdbtLeftOrRight.SelectedIndex = ee.LeftOrRight;
                txtTitle.Text = ee.Title.ToString();
                txtImageUrl.Text = ee.ImageUrl.ToString();
                txtMoreUrl.Text = ee.MoreUrl.ToString();
                txtUrl.Text = ee.Url.ToString();
                txtIframeHeight.Text = ee.IframeHeight.ToString();
                rdbtShow.SelectedIndex = ee.DefaultVisible;
            }
        }
        #endregion

        #region InitObject
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ee"></param>
        private void InitObject(Ea_DefineMainPageDP ee)
        {
            ee.iOrder = Int32.Parse(txtiOrder.Text.Trim());
            ee.LeftOrRight = rdbtLeftOrRight.SelectedIndex;
            ee.DefaultVisible = rdbtShow.SelectedIndex;
            ee.Title = txtTitle.Text.Trim();
            ee.ImageUrl = txtImageUrl.Text.Trim();
            if (txtMoreUrl.Text.Trim() != string.Empty)
                ee.MoreUrl = txtMoreUrl.Text.Trim();
            else
                ee.MoreUrl = "#";
            ee.Url = txtUrl.Text.Trim();
            ee.IframeHeight = txtIframeHeight.Text.Trim();
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
                Ea_DefineMainPageDP ee = new Ea_DefineMainPageDP();
                //检查是否重复了
                bool bRepeat = ee.CheckIsRepeat(int.Parse(this.txtiOrder.Text.Trim()), 0);
                if (bRepeat)
                {
                    Epower.DevBase.BaseTools.PageTool.MsgBox(this, "序号不能重复！");
                    this.Master.IsSaveSuccess = false;
                    return;
                }
                InitObject(ee);
                ee.RegTime = DateTime.Now;
                ee.RegUserID = decimal.Parse(Session["UserID"].ToString());
                ee.RegUserName = Session["PersonName"].ToString();
                ee.Deleted = (int)eRecord_Status.eNormal;
                ee.InsertRecorded(ee);

                this.Master.MainID = ee.ID.ToString();
            }
            else
            {
                Ea_DefineMainPageDP ee = new Ea_DefineMainPageDP();
                //检查是否重复了
                bool bRepeat = ee.CheckIsRepeat(int.Parse(this.txtiOrder.Text.Trim()), decimal.Parse(this.Master.MainID.Trim()));
                if (bRepeat)
                {
                    Epower.DevBase.BaseTools.PageTool.MsgBox(this, "序号不能重复！");
                    this.Master.IsSaveSuccess = false;
                    return;
                }
                ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                InitObject(ee);
                ee.UpdateRecorded(ee);
                this.Master.MainID = ee.ID.ToString();
            }
        }
        #endregion
    }
}
