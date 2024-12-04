/*******************************************************************
 *
 * Description
 * 
 * 排班表编辑
 * Create By  :yxq
 * Create Date:2011年8月30日 星期二
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
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.CustManager
{
    public partial class frmBr_OrderClassEdit : BasePage
    {
        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.CBr_OrderClass;
            this.Master.IsCheckRight = true;
            if (this.Request.QueryString["id"] != null)
            {
                this.Master.MainID = this.Request.QueryString["id"].ToString();
                this.UserPickerMult1.ContralState = eOA_FlowControlState.eReadOnly;
                ctrBeginDutyTime.MustInput = false;
                ctrEndDutyTime.MustInput = false;
                divTime.Visible = false;
            }
            else
            {
                ctrDutyTime.MustInput = false;
                ctrDutyTime.Visible = false;
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
            Response.Redirect("frmBr_OrderClassEdit.aspx");
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
                Br_OrderClassDP ee = new Br_OrderClassDP();
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
            Response.Redirect("frmBr_OrderClassMain.aspx");

            //Response.Write("<script>window.location.href='frmBr_OrderClassMain.aspx';</script>");
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
                Br_OrderClassDP ee = new Br_OrderClassDP();
                ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                UserPickerMult1.UserID = ee.StaffID.ToString();
                UserPickerMult1.UserName = ee.StaffName.ToString();
                HidClassTypeID.Value = ee.ClassTypeID.ToString();
                txtClassTypeName.Text = ee.ClassTypeName.ToString();
                ctrDutyTime.dateTime = DateTime.Parse(ee.DutyTime.ToString());                
            }
        }
        #endregion

        #region InitObject
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ee"></param>
        private void InitObject(Br_OrderClassDP ee)
        {
            //ee.DutyTime = ctrDutyTime.dateTime;
            ee.ClassTypeID = decimal.Parse(HidClassTypeID.Value.Trim());
            ee.ClassTypeName = txtClassTypeName.Text.Trim();
        }
        #endregion

        #region Master_Master_Button_Save_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Save_Click()
        {

            if (HidClassTypeID.Value == "" || decimal.Parse(HidClassTypeID.Value.ToString()) <= 0)
            {
                PageTool.MsgBox(Page, "请选择班次!");
                this.Master.IsSaveSuccess = false;
                return;
            }

            string[] users = null;

            if (UserPickerMult1.UserID.Trim(',').Contains(","))
                users = UserPickerMult1.UserID.Trim(',').Split(',');
            else
            {
                users = new string[1];
                users[0] = UserPickerMult1.UserID.Trim(',');
            }

            string strUserName = string.Empty;                      //存放已经排过某天班次的工程师名称组合


            if (string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                for (int i = 0; i < users.Length; i++)
                {
                    DateTime BeginTime = ctrBeginDutyTime.dateTime;
                    DateTime EndTime = ctrEndDutyTime.dateTime;

                    while (BeginTime <= EndTime)
                    {
                        string sWhere = " And id != " + (this.Master.MainID.Trim() == "" ? "0" : this.Master.MainID.Trim())
                                        + " and StaffID=" + users[i] + " and to_char(DutyTime,'yyyy-MM-dd')=" + StringTool.SqlQ(BeginTime.ToString("yyyy-MM-dd"));
                        string sUserName = UserDP.GetUserName(long.Parse(users[i] == "" ? "0" : users[i]));

                        if (Br_OrderClassDP.IsExists(sWhere))
                        {
                            strUserName += sUserName + "在" + BeginTime.ToString("yyyy-MM-dd") + "已经排过班了,";
                            BeginTime = BeginTime.AddDays(1);
                            continue;
                        }

                        Br_OrderClassDP ee = new Br_OrderClassDP();
                        InitObject(ee);
                        ee.DutyTime = BeginTime;
                        ee.StaffID = decimal.Parse(users[i].ToString());
                        ee.StaffName = UserDP.GetUserName(long.Parse(ee.StaffID.ToString() == "" ? "0" : ee.StaffID.ToString()));
                        ee.DeptID = UserDP.GetUserDeptID(long.Parse(ee.StaffID.ToString() == "" ? "0" : ee.StaffID.ToString()));
                        ee.DeptName = DeptDP.GetDeptName(long.Parse(ee.DeptID.ToString() == "" ? "0" : ee.DeptID.ToString()));

                        ee.Deleted = (int)eRecord_Status.eNormal;
                        ee.RegUserID = long.Parse(Session["UserID"].ToString());
                        ee.RegUserName = Session["PersonName"].ToString();
                        ee.RegTime = DateTime.Now;
                        ee.InsertRecorded(ee);

                        BeginTime = BeginTime.AddDays(1);
                    }
                }

                if (strUserName != string.Empty)
                {
                    PageTool.MsgBox(this, strUserName.Substring(0, strUserName.Length - 1));
                }
            }
            else
            {

                for (int i = 0; i < users.Length; i++)
                {
                    string sWhere = " And id != " + (this.Master.MainID.Trim() == "" ? "0" : this.Master.MainID.Trim())
                                    + " and StaffID=" + users[i] + " and to_char(DutyTime,'yyyy-MM-dd')=" + StringTool.SqlQ(ctrDutyTime.dateTime.ToString("yyyy-MM-dd"));
                    string sUserName = UserDP.GetUserName(long.Parse(users[i] == "" ? "0" : users[i]));

                    if (Br_OrderClassDP.IsExists(sWhere))
                    {
                        strUserName += sUserName + "在" + ctrDutyTime.dateTime.ToString("yyyy-MM-dd") + "已经排过班了,";
                    }
                }

                if (strUserName != string.Empty)
                {
                    PageTool.MsgBox(this, strUserName.Substring(0, strUserName.Length - 1));
                    this.Master.IsSaveSuccess = false;
                    return;
                }

                Br_OrderClassDP ee = new Br_OrderClassDP();
                ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                InitObject(ee);
                ee.DutyTime = ctrDutyTime.dateTime;
                ee.StaffID = decimal.Parse(users[0].ToString());
                ee.StaffName = UserDP.GetUserName(long.Parse(ee.StaffID.ToString() == "" ? "0" : ee.StaffID.ToString()));
                ee.DeptID = UserDP.GetUserDeptID(long.Parse(ee.StaffID.ToString() == "" ? "0" : ee.StaffID.ToString()));
                ee.DeptName = DeptDP.GetDeptName(long.Parse(ee.DeptID.ToString() == "" ? "0" : ee.DeptID.ToString()));
                ee.UpdateRecorded(ee);
            }


            Master_Master_Button_GoHistory_Click();
        }
        #endregion
    }
}