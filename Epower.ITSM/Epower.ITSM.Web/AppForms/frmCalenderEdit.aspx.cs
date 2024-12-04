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
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;
using System.Collections.Generic;

namespace Epower.ITSM.Web.AppForms
{
    public partial class frmCalenderEdit : BasePage
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
            {
                this.Master.ShowEditPageButton();

            }
        }
        #endregion

        #region Master_Master_Button_New_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("frmCalenderEdit.aspx");
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
                CalenderDP ee = new CalenderDP();
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
            Response.Redirect("frmCalender.aspx");
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
                dpdObjectType.Attributes.Add("onchange", "selectChange(this,'" + this.dpdObjectType.ClientID + "')");
                CalenderDP cd = new CalenderDP();
                List<CalenderDP> list = cd.GetAllKind();
                foreach (CalenderDP var in list)
                {
                    ListItem lt = new ListItem(var.Deptname, var.Deptid.ToString());
                    dpdObjectName.Items.Add(lt);
                }
                if (this.Master.MainID != string.Empty)
                {
                    InitObject();
                }
            }
            if (hidObjectName.Value != string.Empty && hidObjectName.Value.Trim() != txtObjectName.Text.Trim())
            {
                txtObjectName.Text = hidObjectName.Value.Trim();
            }
        }
        #endregion

        #region InitObject
        /// <summary>
        /// 
        /// </summary>
        private void InitObject()
        {
            CalenderDP cd = new CalenderDP();
            cd = cd.GetReCorded(int.Parse(Master.MainID));
            dpdObjectType.SelectedIndex = dpdObjectType.Items.IndexOf(dpdObjectType.Items.FindByValue(cd.Caltype.ToString()));
            if (cd.Caltype == 1)  //机构
            {
                txtObjectName.Text = DeptDP.GetDeptName(long.Parse(cd.Compareid.ToString())).ToString();
                dpdObjectName.SelectedIndex = dpdObjectName.Items.IndexOf(dpdObjectName.Items.FindByValue(cd.Compareid.ToString()));
            }
            else if (cd.Caltype == 2)  //部门
            {
                txtObjectId.Value = cd.Compareid.ToString();
                txtObjectName.Text = DeptDP.GetDeptName(long.Parse(cd.Compareid.ToString())).ToString();
                hidObjectName.Value = txtObjectName.Text;
            }
            else if (cd.Caltype == 3)  //人员
            {
                txtObjectId.Value = cd.Compareid.ToString();
                txtObjectName.Text = UserDP.GetUserName(long.Parse(cd.Compareid.ToString())).ToString();
                hidObjectName.Value = txtObjectName.Text;
            }
            Ctrdateandtime1.dateTime = DateTime.Parse(cd.Caldate.ToString());
        }
        #endregion 

        #region Master_Master_Button_Save_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Save_Click()
        {
            CalenderDP cd = new CalenderDP();
            cd.Caldate = Ctrdateandtime1.dateTime.ToString("yyyy-MM-dd");
            cd.Caltype = int.Parse(dpdObjectType.SelectedValue);
            if (dpdObjectType.SelectedValue == "0")   //全局
                cd.Compareid = 0;
            else if (dpdObjectType.Text == "1")   //机构
                cd.Compareid = decimal.Parse(dpdObjectName.SelectedValue.Trim());
            else if (dpdObjectType.Text == "2")   //部门
                cd.Compareid = decimal.Parse(txtObjectId.Value.Trim());
            else if (dpdObjectType.Text == "3")   //人员
                cd.Compareid = decimal.Parse(txtObjectId.Value.Trim());

            if (!string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                if (CalenderDP.CheckIsRepeat(cd.Caltype, cd.Compareid, decimal.Parse(this.Master.MainID), cd.Caldate))
                {
                    Epower.DevBase.BaseTools.PageTool.MsgBox(this, "不能重复，请重新选择 ！");
                    this.Master.IsSaveSuccess = false;
                    return;
                }
                cd.Updateuser = decimal.Parse(Session["UserID"].ToString());
                cd.Updatetime = DateTime.Now;
                cd.Id = decimal.Parse(this.Master.MainID);
                cd.UpdateRecorded(cd);
                this.Master.MainID = cd.Id.ToString();
            }
            else
            {
                if (CalenderDP.CheckIsRepeat(cd.Caltype, cd.Compareid,0,cd.Caldate))
                {
                    Epower.DevBase.BaseTools.PageTool.MsgBox(this, "不能重复，请重新选择 ！");
                    this.Master.IsSaveSuccess = false;
                    return;
                }

                cd.Inputtime = DateTime.Now;
                cd.Inputuser = decimal.Parse(Session["UserID"].ToString());
                cd.InsertRecorded(cd);
                this.Master.MainID = cd.Id.ToString();
            }
        }
        #endregion
    }
}

