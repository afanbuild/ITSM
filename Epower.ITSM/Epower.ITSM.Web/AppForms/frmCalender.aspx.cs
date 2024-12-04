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
using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;

using System.Collections.Generic;


namespace Epower.ITSM.Web.AppForms
{
    public partial class frmCalender : BasePage
    {
        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.SystemManager;
            this.Master.IsCheckRight = true;
            dgEa_DefineMainPage.Columns[dgEa_DefineMainPage.Columns.Count-1].Visible = this.Master.GetEditRight();
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);
            this.Master.ShowQueryPageButton();
            this.Master.MainID = "1";
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

        #region Master_Master_Button_Query_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            ControlPage1.DataGridToControl.CurrentPageIndex = 0;
            DataTable dt = LoadData();
            dgEa_DefineMainPage.DataSource = dt.DefaultView;
            dgEa_DefineMainPage.DataBind();
        }
        #endregion

        #region Master_Master_Button_Delete_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            CalenderDP cd = new CalenderDP(); 
            foreach (DataGridItem itm in dgEa_DefineMainPage.Items)
            {
                if (itm.ItemType == ListItemType.AlternatingItem ||
                    itm.ItemType == ListItemType.Item)
                {
                    string sID = itm.Cells[1].Text;
                    CheckBox chkdel = (CheckBox)itm.Cells[0].FindControl("chkDel");
                    if (chkdel.Checked)
                    {
                        cd.DeleteRecorded(long.Parse(sID));
                    }
                }
            }
            DataTable dt = LoadData();
            dgEa_DefineMainPage.DataSource = dt.DefaultView;
            dgEa_DefineMainPage.DataBind();
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
            ControlPage1.On_PostBack += new EventHandler(ControlPage1_On_PostBack);
            ControlPage1.DataGridToControl = dgEa_DefineMainPage;
            if (!IsPostBack)
            {
                CalenderDP cd = new CalenderDP();
                List<CalenderDP> list = cd.GetAllKind();
                ddltObjectID.Items.Add(new ListItem("--全部--","-1"));
                foreach (CalenderDP var in list)
                {
                     ListItem lt = new ListItem(var.Deptname, var.Deptid.ToString());
                    ddltObjectID.Items.Add(lt);
                }

                DataTable dt = LoadData();
                dgEa_DefineMainPage.DataSource = dt.DefaultView;
                dgEa_DefineMainPage.DataBind();
            }
            if (hidObjectName.Value.Trim() != txtObjectName.Text.Trim())
            {
                txtObjectName.Text = hidObjectName.Value.Trim();
            }
        }
        #endregion

        #region LoadData
        /// <summary>
        /// 
        /// </summary>
        private DataTable LoadData()
        {
            DataTable dt;
            string sWhere = string.Empty;
            string sOrder = string.Empty;
            switch (dpdObjectType.SelectedValue.Trim())
            {
                case "0":   //全局
                    sWhere += " And a.CalType=0 ";
                    break;
                case "1":   //机构
                    if (ddltObjectID.SelectedValue != "-1")
                    {
                        sWhere = " And a.CompareID=" + ddltObjectID.SelectedValue.Trim();
                    }
                    sWhere += " And a.CalType=1 ";
                    break;
                case "2":   //部门
                    if (hidObjectID.Value.Trim() != string.Empty)
                    {
                        sWhere = " And a.CompareID=" + hidObjectID.Value.Trim();
                    }
                    sWhere += " And a.CalType=2 ";
                    break;
                case "3":  //人员
                    if (hidObjectID.Value.Trim() != string.Empty)
                    {
                        sWhere = " And a.CompareID=" + hidObjectID.Value.Trim();
                    }
                    sWhere += " And a.CalType=3 ";
                    break;
                default:
                    sWhere = string.Empty;
                    break;
            }
            sOrder = " Order by CalType,CompareID,Caldate";
            CalenderDP cd = new CalenderDP(); 
            dt = cd.GetDataTable(sWhere, sOrder); ;
            Session["Ts_Calender"] = dt;
            return dt;
        }
        #endregion

        #region  Bind
        /// <summary>
        /// 
        /// </summary>
        private void Bind()
        {
            DataTable dt;
            if (Session["Ts_Calender"] == null)
            {
                dt = LoadData();
            }
            else
            {
                dt = (DataTable)Session["Ts_Calender"];
            }
            dgEa_DefineMainPage.DataSource = dt.DefaultView;
            dgEa_DefineMainPage.DataBind();
        }
        #endregion

        #region ControlPage1_On_PostBack
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ControlPage1_On_PostBack(object sender, EventArgs e)
        {
            Bind();
        }
        #endregion

        #region  dgEa_DefineMainPage_ItemCommand
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgEa_DefineMainPage_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                Response.Redirect("frmCalenderEdit.aspx?id=" + e.Item.Cells[1].Text.ToString());
            }
        }
        #endregion 

        #region dgEa_DefineMainPage_ItemCreated
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgEa_DefineMainPage_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                int j = 0;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i > 1 && i < e.Item.Cells.Count-1)
                    {
                        j = i - 1;
                        if(i==2 || i==7)
                        {
                            e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",1);");
                        }
                        else
                        {
                            e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                        } 
                    }
                }
            }
        }
        #endregion

        #region dpdObjectType_SelectedIndexChanged
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dpdObjectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (dpdObjectType.SelectedValue.Trim())
            { 
                case  "0":   //全局
                    lblTitle.Visible = false;
                    txtObjectName.Visible = false;
                    cmdPop.Visible = false;
                    ddltObjectID.Visible = false;
                    break;
                case "1":   //机构
                    lblTitle.Visible = true;
                    txtObjectName.Visible = false;
                    cmdPop.Visible = false;
                    ddltObjectID.Visible = true;
                    break;
                case "2":   //部门
                    lblTitle.Visible = true;
                    txtObjectName.Visible = true;
                    cmdPop.Visible = true;
                    ddltObjectID.Visible = false;
                    break;
                case "3":  //人员
                    lblTitle.Visible = true;
                    txtObjectName.Visible = true;
                    cmdPop.Visible = true;
                    ddltObjectID.Visible = false;
                    break;
                default :
                    lblTitle.Visible = false;
                    txtObjectName.Visible = false;
                    cmdPop.Visible = false;
                    ddltObjectID.Visible = false;
                    break;
            }
            txtObjectName.Text = string.Empty;
            hidObjectName.Value = string.Empty;
            hidObjectID.Value = string.Empty;
        }
        #endregion
    }
}