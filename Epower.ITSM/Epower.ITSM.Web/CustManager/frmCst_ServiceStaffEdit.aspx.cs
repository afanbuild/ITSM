/****************************************************************************
 * 
 * description:服务人员维护
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-11-12
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
    public partial class frmCst_ServiceStaffEdit : BasePage
    {
        #region 是否查询
        /// <summary>
        /// 
        /// </summary>
        protected bool IsSelect
        {
            get
            {
                if (Request["IsSelect"] != null && Request["IsSelect"]=="true")
                    return true;
                else
                    return false;
            }
        }
        #endregion 

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.ServiceStaff;
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
            Response.Redirect("frmCst_ServiceStaffEdit.aspx?IsSelect=" + IsSelect.ToString().ToLower());
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
                Cst_ServiceStaffDP ee = new Cst_ServiceStaffDP();
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
            Response.Redirect("frmCst_ServiceStaffMain.aspx?IsSelect=" + IsSelect.ToString().ToLower());
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
                //绑定服务单位
                InitDropDownList();
                InitRestType();
                LoadData();
                //Page.RegisterStartupScript("", "<script>OnSelectedChanged();</script>");
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
                Cst_ServiceStaffDP ee = new Cst_ServiceStaffDP();
                ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                CtrFlowtxtName.Value = ee.Name.ToString();
                ddltMastCustID.SelectedIndex = ddltMastCustID.Items.IndexOf(ddltMastCustID.Items.FindByValue(ee.BlongDeptID.ToString()));
                CtrNumOrder.Value = ee.OrderIndex.ToString();
                CtrRemark.Value = ee.Remark.ToString();
                CtrdateJoin.dateTime = ee.JoinDate;
                ftxtFaculty.Text = ee.Faculty.ToString();
                RefUser.UserID = long.Parse(ee.UserID.ToString());
                RefUser.UserName = ee.UserName.ToString();
                ddRestType.SelectedIndex = ddRestType.Items.IndexOf(ddRestType.Items.FindByValue(ee.RESTVALUE.ToString()));
            }
        }
        #endregion

        #region InitObject
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ee"></param>
        private void InitObject(Cst_ServiceStaffDP ee)
        {
            ee.Name = CtrFlowtxtName.Value.Trim();
            ee.BlongDeptID = decimal.Parse(ddltMastCustID.SelectedItem.Value.Trim());
            ee.BlongDeptName = ddltMastCustID.SelectedItem.Text.Trim();
            if (CtrNumOrder.Value.Trim() == string.Empty)
                CtrNumOrder.Value = "0";
            ee.OrderIndex = Int32.Parse(CtrNumOrder.Value.Trim());
            ee.Remark = CtrRemark.Value.Trim();
            ee.JoinDate = CtrdateJoin.dateTime;
            ee.Faculty = ftxtFaculty.Text.Trim();
            ee.UserID = RefUser.UserID;
            ee.UserName = RefUser.UserName.Trim();
            ee.RESTVALUE = ddRestType.SelectedValue;
            ee.RESTNAME = ddRestType.SelectedItem.Text;
        }
        #endregion

        #region Master_Master_Button_Save_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Save_Click()
        {
            //检查是否重复
            if (Cst_ServiceStaffDP.CheckIsRepeat(this.Master.MainID.Trim(),RefUser.UserID))
            {
                Epower.DevBase.BaseTools.PageTool.MsgBox(this, "对应用户不能重复，请重新选择！");
                this.Master.IsSaveSuccess = false;
                return;
            }

            if (string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                Cst_ServiceStaffDP ee = new Cst_ServiceStaffDP();
                InitObject(ee);
                ee.Deleted = (int)eRecord_Status.eNormal;
                ee.RegUserID = long.Parse(Session["UserID"].ToString());
                ee.RegUserName = Session["PersonName"].ToString();
                ee.RegDeptID = long.Parse(Session["UserDeptID"].ToString());
                ee.RegDeptName = Session["UserDeptName"].ToString();
                ee.RegTime = DateTime.Now;
                ee.RESTVALUE = ddRestType.SelectedValue;
                ee.RESTNAME = ddRestType.SelectedItem.Text;
                ee.InsertRecorded(ee);

                this.Master.MainID = ee.ID.ToString();
            }
            else
            {
                Cst_ServiceStaffDP ee = new Cst_ServiceStaffDP();
                ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                InitObject(ee);
                ee.UpdateRecorded(ee);
                this.Master.MainID = ee.ID.ToString();
            }
            HttpRuntime.Cache.Remove("ItEngineerSource");
        }
        #endregion

        /// <summary>
        /// 绑定服务单位
        /// </summary>
        private void InitDropDownList()
        {
            Br_MastCustomerDP ee = new Br_MastCustomerDP();
            string sWhere = string.Empty;
            string sOrder = string.Empty;
            DataTable dt = ee.GetDataTable(sWhere, sOrder);
            ddltMastCustID.DataSource = dt;
            ddltMastCustID.DataTextField = "ShortName";
            ddltMastCustID.DataValueField = "ID";
            ddltMastCustID.DataBind();
        }

        /// <summary>
        /// 绑定工程师休息类型
        /// </summary>
        private void InitRestType()
        {
            DataTable dt = GS_SchedulesAreaDP.GetRestType();
            ddRestType.DataSource = dt;
            ddRestType.DataTextField = "restname";
            ddRestType.DataValueField = "restvalue";
            ddRestType.DataBind();
        }
    }
}
