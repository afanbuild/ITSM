/****************************************************************************
 * 
 * description:
 * 
 * 工程师选择
 * 
 * Create by: yxq
 * Create Date:2011-08-30
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
using System.Text;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.AppForms
{
    public partial class frmCst_ServiceStaffSel : BasePage
    {
        #region 是否查询
        /// <summary>
        /// 
        /// </summary>
        protected bool IsSelect
        {
            get
            {
                if (Request["IsSelect"] != null && Request["IsSelect"] == "true")
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
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);            
            this.Master.MainID = "1";
            this.Master.ShowQueryButton(true);
        }
        #endregion

        #region Master_Master_Button_Query_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            LoadData();
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
            cpServiceStaff.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(LoadData);
            if (!IsPostBack)
            {               
                //绑定服务单位
                InitDropDownList();
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
            int iRowCount = 0;
            DataTable dt;
            string sWhere = string.Empty;
            string sOrder = " Order by OrderIndex";
            if (ddltMastCustID.SelectedItem.Text.Trim() != string.Empty)
            {
                sWhere += " And BlongDeptID=" + ddltMastCustID.SelectedValue.Trim();
            }

            Cst_ServiceStaffDP ee = new Cst_ServiceStaffDP();

            dt = ee.GetDataTable(sWhere, sOrder, this.cpServiceStaff.PageSize, this.cpServiceStaff.CurrentPage, ref iRowCount);

            dgCst_ServiceStaff.DataSource = dt.DefaultView;
            dgCst_ServiceStaff.DataBind();
            this.cpServiceStaff.RecordCount = iRowCount;
            this.cpServiceStaff.Bind();
        }
        #endregion        

        #region  dgCst_ServiceStaff_ItemCommand
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgCst_ServiceStaff_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                string id = e.Item.Cells[0].Text.Replace("&nbsp;", "");
                string sWhere = " And id = " + id;
                Cst_ServiceStaffDP ee = new Cst_ServiceStaffDP();
                DataTable dt = ee.GetDataTable(sWhere, "");
                Json json = new Json(dt);

                string jsonstr = "{record:" + json.ToJson() + "}";

                StringBuilder sbText = new StringBuilder();
                sbText.Append("<script>");
                sbText.Append("var arr = " + jsonstr + ";");
                sbText.Append("window.parent.returnValue = arr;");
                // 关闭窗口
                sbText.Append("top.close();");
                sbText.Append("</script>");
                // 向客户端发送
                Page.RegisterStartupScript(DateTime.Now.ToString(), sbText.ToString());
                Response.Write(sbText.ToString());
            }
        }
        #endregion

        #region dgCst_ServiceStaff_ItemCreated
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgCst_ServiceStaff_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                int j = 0;
                for (int i = 0; i < e.Item.Cells.Count-1; i++)
                {
                    if (i >= 1)
                    {
                        j = i;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
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
            ddltMastCustID.Items.Insert(0, new ListItem("", ""));
            if (ddltMastCustID.Items.Count > 1)
                ddltMastCustID.SelectedIndex = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddltMastCustID_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }      

        protected void dgCst_ServiceStaff_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");

                string id = e.Item.Cells[0].Text.Replace("&nbsp;", "");
                string sWhere = " And id = " + id;
                Cst_ServiceStaffDP ee = new Cst_ServiceStaffDP();
                DataTable dt = ee.GetDataTable(sWhere, "");
                Json json = new Json(dt);

                string jsonstr = "{record:" + json.ToJson() + "}";

                e.Item.Attributes.Add("ondblclick", "doubleSelect(" + jsonstr + ");");

            }
        }
    }
}