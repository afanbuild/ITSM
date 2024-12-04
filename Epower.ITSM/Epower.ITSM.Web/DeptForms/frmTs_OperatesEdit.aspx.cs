/*******************************************************************
 *
 * Description
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
    public partial class frmTs_OperatesEdit : BasePage
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
            Response.Redirect("frmTs_OperatesEdit.aspx");
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
                Ts_OperatesDP ee = new Ts_OperatesDP();
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
            Response.Redirect("frmTs_OperatesMain.aspx");
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
            Ts_OperatesDP ee = new Ts_OperatesDP();
            if (!string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                CtrFlowOperateID.Value = ee.OperateID.ToString();
                CtrFlowOpName.Value = ee.OpName.ToString();
                CtrFlowOpDesc.Value = ee.OpDesc.ToString();
                CtrFlowOpCatalog.Value = ee.OpCatalog.ToString();

                CtrFlowOperateID.ContralState = eOA_FlowControlState.eReadOnly;
            }
            else
            {
                CtrFlowOperateID.Value = ee.GetMaxID();
            }
        }
        #endregion

        #region InitObject
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ee"></param>
        private void InitObject(Ts_OperatesDP ee)
        {
            ee.OperateID = long.Parse(CtrFlowOperateID.Value.Trim());
            ee.OpName = CtrFlowOpName.Value.Trim();
            ee.OpDesc = CtrFlowOpDesc.Value.Trim();
            ee.OpCatalog = CtrFlowOpCatalog.Value.Trim();
        }
        #endregion

        #region Master_Master_Button_Save_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Save_Click()
        {
            Ts_OperatesDP ee = new Ts_OperatesDP();
            if (string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                try
                {
                    if (ee.CheckIsRepeat(long.Parse(CtrFlowOperateID.Value.Trim())))
                    {
                        Epower.DevBase.BaseTools.PageTool.MsgBox(this, "操作项不能重复，请重新输入！");
                        this.Master.IsSaveSuccess = false;
                        return;
                    }
                }
                catch (Exception e)
                { }

                InitObject(ee);
                ee.SysID = 101;
                ee.OpType = 30;
                ee.SqlStatement = string.Empty;
                ee.Paramaters = string.Empty;
                ee.ConnectSystem = string.Empty;
                ee.InsertRecorded(ee);

                this.Master.MainID = ee.OperateID.ToString();
            }
            else
            {
                ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                InitObject(ee);
                ee.UpdateRecorded(ee);
                this.Master.MainID = ee.OperateID.ToString();
            }
        }
        #endregion
    }
}
