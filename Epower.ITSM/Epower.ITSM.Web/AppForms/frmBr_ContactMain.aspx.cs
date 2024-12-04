/****************************************************************************
 * 
 * description:联系人管理
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-10-23
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

using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;

namespace Epower.ITSM.Web.AppForms
{
    public partial class frmBr_ContactMain : BasePage
    {
        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.ServiceDept;
            this.Master.IsCheckRight = true;
            dgBr_Contact.Columns[10].Visible = this.Master.GetEditRight();
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);
            this.Master.ShowQueryPageButton();
            this.Master.ShowQueryButton(false);
            this.Master.MainID = "1";
        }
        #endregion

        #region Master_Master_Button_New_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("frmBr_ContactEdit.aspx?CustomID=" + CustomID);
        }
        #endregion

        #region Master_Master_Button_Delete_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            Br_ContactDP ee = new Br_ContactDP();
            foreach (DataGridItem itm in dgBr_Contact.Items)
            {
                if (itm.ItemType == ListItemType.AlternatingItem ||
                    itm.ItemType == ListItemType.Item)
                {
                    string sID = itm.Cells[1].Text;
                    CheckBox chkdel = (CheckBox)itm.Cells[0].FindControl("chkDel");
                    if (chkdel.Checked)
                    {
                        ee.DeleteRecorded(long.Parse(sID));
                    }
                }
            }
            DataTable dt = LoadData();
            dgBr_Contact.DataSource = dt.DefaultView;
            dgBr_Contact.DataBind();
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
            ControlPage1.DataGridToControl = dgBr_Contact;
            if (!IsPostBack)
            {
                if (Request["CustomID"] != null)
                {
                    if (Request["CustomID"] != string.Empty)
                        CustomID = Request["CustomID"].ToString();
                }
                if(Request["CustomFlag"]!=null)
                {
                    Session["CustomFlag"] = Request["CustomFlag"].ToString();
                }
                if (Session["CustomFlag"].ToString() == "0")
                {
                    tbCustom.Visible = false;
                }
                DataTable dt = LoadData();
                dgBr_Contact.DataSource = dt.DefaultView;
                dgBr_Contact.DataBind();
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
            if (txtCustomName.Text.Trim() != string.Empty)
            {
                sWhere += " And CustomName like " + StringTool.SqlQ("%" + txtCustomName.Text.Trim() + "%");
            }
            if (txtContactName.Text.Trim() != string.Empty)
            {
                sWhere += " And ContactName like " + StringTool.SqlQ("%" + txtContactName.Text.Trim() + "%");
            }
            if (ddltSex.SelectedValue !="-1")
            {
                sWhere += " And Sex=" + ddltSex.SelectedValue.Trim();
            }  
            if (txtAddress.Text.Trim() != string.Empty)
            {
                sWhere += " And Address like " + StringTool.SqlQ("%" + txtAddress.Text.Trim() + "%");
            }
            if (Request["CustomID"]!=null)
            {
                if (Session["CustomFlag"].ToString() == "0")
                {
                    if (Request["CustomID"] != string.Empty)
                        sWhere += " And CustomID=" + CustomID;
                    else
                        sWhere += " And 1<>1 ";
                }
            }
            Br_ContactDP ee = new Br_ContactDP();
            dt = ee.GetDataTable(sWhere, sOrder); ;
            Session["Br_Contact"] = dt;
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
            if (Session["Br_Contact"] == null)
            {
                dt = LoadData();
            }
            else
            {
                dt = (DataTable)Session["Br_Contact"];
            }
            dgBr_Contact.DataSource = dt.DefaultView;
            dgBr_Contact.DataBind();
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

        #region  dgBr_Contact_ItemCommand
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgBr_Contact_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                Response.Redirect("frmBr_ContactEdit.aspx?id=" + e.Item.Cells[1].Text.ToString() + "&CustomID=" + CustomID);
            }
        }
        #endregion

        #region dgBr_Contact_ItemCreated
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgBr_Contact_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                int j = 0;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i > 1 && i < e.Item.Cells.Count - 1)
                    {
                        j = i - 2;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
        #endregion

        #region dgBr_Contact_ItemDataBound
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgBr_Contact_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.Item.Cells[5].Text.Trim() == "0")
                    e.Item.Cells[5].Text = "男";
                else if (e.Item.Cells[5].Text.Trim()=="1")
                    e.Item.Cells[5].Text = "女";

                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                e.Item.Attributes.Add("ondblclick", "window.open('frmBr_ContactEdit.aspx?ID=" + e.Item.Cells[1].Text.ToString() + "&randomid='+GetRandom(),'Contract','')");
            }
        }
        #endregion 

        #region CustomID
        /// <summary>
        /// 
        /// </summary>
        protected string CustomID
        {
            get
            {
                if (ViewState["CustomID"] != null)
                    return ViewState["CustomID"].ToString();
                else
                    return "0";
            }
            set
            {
                ViewState["CustomID"] = value;
            }
        }
        #endregion 
    }
}