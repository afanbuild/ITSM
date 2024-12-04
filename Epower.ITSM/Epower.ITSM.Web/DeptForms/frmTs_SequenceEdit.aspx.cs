/*******************************************************************
 *
 * Description:系统系列号表维护
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
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.DeptForms
{
    public partial class frmTs_SequenceEdit : BasePage
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
            Response.Redirect("frmTs_SequenceEdit.aspx");
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
                Ts_SequenceDP ee = new Ts_SequenceDP();
                ee.DeleteRecorded(this.Master.MainID.Trim());
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
            Response.Redirect("frmTs_SequenceMain.aspx");
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
                Ts_SequenceDP ee = new Ts_SequenceDP();
                ee = ee.GetReCorded(this.Master.MainID.Trim());
                CtrFlowName.Value = ee.Name;
                CtrFlowMinValue.Value = ee.MinValue.ToString();
                CtrFlowMaxValue.Value = ee.MaxValue.ToString();
                CtrFlowCurrentValue.Value = ee.CurrentValue.ToString();
                CtrFlowStep.Value = ee.Step.ToString();
                CtrFlowRecycle.Value = ee.Recycle.ToString();

                CtrFlowName.ContralState = eOA_FlowControlState.eReadOnly;
            }
            else
            {
                CtrFlowMinValue.Value = "10000";
                CtrFlowMaxValue.Value = "9999999999";
                CtrFlowCurrentValue.Value = "10000";
                CtrFlowStep.Value = "1";
                CtrFlowRecycle.Value = "0";
            }
        }
        #endregion

        #region InitObject
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ee"></param>
        private void InitObject(Ts_SequenceDP ee)
        {
            ee.MinValue = Decimal.Parse(CtrFlowMinValue.Value.Trim());
            ee.MaxValue = Decimal.Parse(CtrFlowMaxValue.Value.Trim());
            ee.CurrentValue = Decimal.Parse(CtrFlowCurrentValue.Value.Trim());
            ee.Step = Int32.Parse(CtrFlowStep.Value.Trim());
            ee.Recycle = Int32.Parse(CtrFlowRecycle.Value.Trim());
        }
        #endregion

        #region Master_Master_Button_Save_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Save_Click()
        {
            #region 判断值的范围 yxq 2011-09-05
            if (decimal.Parse(CtrFlowMinValue.Value) > decimal.Parse(CtrFlowMaxValue.Value))
            {
                PageTool.MsgBox(this, "最小值不能大于最大值，请重新输入！");
                this.Master.IsSaveSuccess = false;
                return;
            }
            if (decimal.Parse(CtrFlowMinValue.Value) > decimal.Parse(CtrFlowCurrentValue.Value))
            {
                PageTool.MsgBox(this, "最小值不能大于当前值，请重新输入！");
                this.Master.IsSaveSuccess = false;
                return;
            }
            if (decimal.Parse(CtrFlowCurrentValue.Value) > decimal.Parse(CtrFlowMaxValue.Value))
            {
                PageTool.MsgBox(this, "当前值不能大于最大值，请重新输入！");
                this.Master.IsSaveSuccess = false;
                return;
            }

            #endregion

            if (string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                Ts_SequenceDP ee = new Ts_SequenceDP();
                if (ee.CheckIsRepeat(CtrFlowName.Value.Trim()))
                {
                    Epower.DevBase.BaseTools.PageTool.MsgBox(this, "系列号关键字不能重复，请重新输入！");
                    this.Master.IsSaveSuccess = false;
                    return;
                }


                
                InitObject(ee);
                ee.Name = CtrFlowName.Value.Trim();
                ee.InsertRecorded(ee);

                this.Master.MainID = ee.Name.ToString();
            }
            else
            {
                Ts_SequenceDP ee = new Ts_SequenceDP();
                ee = ee.GetReCorded(this.Master.MainID.Trim());
                InitObject(ee);
                ee.UpdateRecorded(ee);
                this.Master.MainID = ee.Name.ToString();
            }
        }
        #endregion
    }
}