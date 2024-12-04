/*******************************************************************
 *
 * Description:专业维护
 * 
 * 
 * Create By  :zhumc
 * Create Date:2011年5月19日
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

using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;

namespace Epower.ITSM.Web.DeptForms
{
    public partial class frmTs_ProfessionalEdit : BasePage
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
            Response.Redirect("frmTs_ProfessionalEdit.aspx");
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
                Ts_ProfessionalDP ee = new Ts_ProfessionalDP();
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
            Response.Redirect("frmTs_ProfessionalMain.aspx");
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
            Ts_ProfessionalDP ee = new Ts_ProfessionalDP();
            if (!string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                CtrFlowPID.Value = ee.PID.ToString();
                CtrFlowPName.Value = ee.PName.ToString();

                CtrFlowPID.ContralState = eOA_FlowControlState.eReadOnly;
            }
            else
            {
                CtrFlowPID.Value = ee.GetMaxID();
            }
        }
        #endregion

        #region InitObject
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ee"></param>
        private void InitObject(Ts_ProfessionalDP ee)
        {
            ee.PID = decimal.Parse(CtrFlowPID.Value.Trim());
            ee.PName = CtrFlowPName.Value.Trim();
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
                Ts_ProfessionalDP ee = new Ts_ProfessionalDP();

                if (ee.CheckIsRepeat(int.Parse(CtrFlowPID.Value.Trim())))
                {
                    Epower.DevBase.BaseTools.PageTool.MsgBox(this, "专业编号不能重复，请重新输入！");
                    this.Master.IsSaveSuccess = false;
                    return;
                }

                InitObject(ee);
                ee.InsertRecorded(ee);

                this.Master.MainID = ee.PID.ToString();
            }
            else
            {
                Ts_ProfessionalDP ee = new Ts_ProfessionalDP();
                ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                InitObject(ee);
                ee.UpdateRecorded(ee);
                this.Master.MainID = ee.PID.ToString();
            }
        }
        #endregion
    }
}
