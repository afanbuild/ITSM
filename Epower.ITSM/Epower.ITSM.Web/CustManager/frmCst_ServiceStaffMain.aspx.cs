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
using System.Text;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.AppForms
{
    public partial class frmCst_ServiceStaffMain : BasePage
    {

        public string Opener_ClientId
        {
            set
            {
                ViewState["Opener_ClientId"] = value;
            }
            get
            {
                return (ViewState["Opener_ClientId"] == null) ? "" : ViewState["Opener_ClientId"].ToString();
            }
        }
        public string TypeFrm
        {
            get
            {

                if (Request.QueryString["TypeFrm"] != null)
                {
                    return Request.QueryString["TypeFrm"];
                }
                else
                {
                    return "";
                }
            }
        }
        public string type {

            get {
                if (Request.QueryString["type"] != null)
                {
                    return Request.QueryString["type"];
                }
                else {

                    return "";
                }
            }

        }

        public string RequestType
        {
            set
            {
                ViewState["RequestType"] = value;
            }
            get
            {
                return (ViewState["RequestType"] == null) ? "" : ViewState["RequestType"].ToString();
            }
        }

        public string ObjID
        {
            get {
                return String.IsNullOrEmpty(Request.QueryString["objID"]) ? "" : Request.QueryString["objID"].ToString(); }
        }



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
            if (!IsSelect)
                this.Master.IsCheckRight = true;

            dgCst_ServiceStaff.Columns[dgCst_ServiceStaff.Columns.Count - 1].Visible = this.Master.GetEditRight();
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);
            this.Master.Master_Button_Print_Click += new Global_BtnClick(Master_Master_Button_Print_Click);
            this.Master.Master_Button_GoHistory_Click += new Global_BtnClick(Master_Master_Button_GoHistory_Click);
            this.Master.ShowQueryPageButton();
            this.Master.MainID = "1";
            this.Master.ShowQueryButton(true);
            // this.Master.Btn_print.Text = "批量添加";
            // this.Master.ShowPrintButton(true);
            this.Master.Btn_back.Text = "推荐工程师";
            this.Master.ShowBackUrlButton(true);
            if (IsSelect)  //如果是选择
            {
                trselect.Visible = true;
                this.Master.ShowQueryButton(true);
                this.Master.ShowNewButton(false);
                this.Master.ShowDeleteButton(false);
                this.Master.ShowPrintButton(false);
                this.Master.ShowBackUrlButton(false);
                this.dgCst_ServiceStaff.Columns[dgCst_ServiceStaff.Columns.Count - 1].Visible = false;
                //this.Master.TableVisible = false;
            }
            else
            {
                // this.Master.ShowNewButton(false);                
                trselect.Visible = false;
            }
        }
        #endregion

        #region 推荐工程师
        void Master_Master_Button_GoHistory_Click()
        {
            Response.Redirect("../RecommendRule/frmCst_RecommendRuleMain.aspx");
        }
        #endregion

        #region 批量添加工程师
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Print_Click()
        {
            string[] sarrUserID = hidUserID.Value.Trim().Split(',');
            Cst_ServiceStaffDP ee = new Cst_ServiceStaffDP();
            int j = 0;
            for (int i = 0; i < sarrUserID.Length - 1; i++)   //如果有值
            {
                if (!Cst_ServiceStaffDP.CheckIsRepeat(string.Empty, long.Parse(sarrUserID[i])))  //检查是否重复
                {
                    InitObject(ee, long.Parse(sarrUserID[i]));
                    ee.Deleted = (int)eRecord_Status.eNormal;
                    ee.RegUserID = long.Parse(Session["UserID"].ToString());
                    ee.RegUserName = Session["PersonName"].ToString();
                    ee.RegDeptID = long.Parse(Session["UserDeptID"].ToString());
                    ee.RegDeptName = Session["UserDeptName"].ToString();
                    ee.RegTime = DateTime.Now;
                    ee.InsertRecorded(ee);
                    j++;
                }
            }
            LoadData();
            PageTool.MsgBox(this, "批量添加成功，共添加工程师" + j.ToString() + "人！");
        }

        #region InitObject
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ee"></param>
        private void InitObject(Cst_ServiceStaffDP ee, long lngUserID)
        {
            UserEntity userentity = new UserEntity();
            long lngDeptID = UserDP.GetUserDeptID(lngUserID);
            string strName = UserDP.GetUserName(lngUserID);
            long DirectdeptId = DeptDP.GetDirectMustCust(lngDeptID);   //服务单位
            string DirectDeptname = DeptDP.GetDeptName(DirectdeptId);          //服务单位名称
            long lngMastID = long.Parse(userentity.getMastCost(DirectDeptname, DirectdeptId.ToString()).ToString());

            ee.Name = strName;
            ee.BlongDeptID = lngMastID;
            ee.BlongDeptName = DirectDeptname;
            ee.OrderIndex = 0;
            ee.Remark = string.Empty;
            ee.JoinDate = DateTime.MinValue;
            ee.Faculty = string.Empty;
            ee.UserID = lngUserID;
            ee.UserName = strName;
        }
        #endregion
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

        #region Master_Master_Button_Query_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            LoadData();
        }
        #endregion

        #region Master_Master_Button_Delete_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            Cst_ServiceStaffDP ee = new Cst_ServiceStaffDP();
            foreach (DataGridItem itm in dgCst_ServiceStaff.Items)
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
            HttpRuntime.Cache.Remove("ItEngineerSource");
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
                if (Request["Opener_ClientId"] != null)
                {
                    Opener_ClientId = Request["Opener_ClientId"].ToString().Replace("hidClientId_ForOpenerPage", "");
                }
                if (Request["RequestType"] != null)
                {
                    RequestType = Request["RequestType"].ToString();
                }
                
                if (Request.QueryString["flowid"] != null)
                {
                    if (Request.QueryString["flowid"].ToString() != "")
                    {
                        rbtnRU.Visible = IsSelect;
                        rbtnAll.Visible = IsSelect;
                    }
                    else
                    {
                        rbtnRU.Visible = false;
                        rbtnAll.Visible = false;
                        tblSelect.Visible = false;
                    }
                }
                else
                {
                    rbtnRU.Visible = false;
                    rbtnAll.Visible = false;
                    tblSelect.Visible = false;
                }
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

            string sOrder = " Order by regtime desc";

            if (CtrFlowName.Value.Trim() != "")
            {
                sWhere += " and Name like " + StringTool.SqlQ("%" + CtrFlowName.Value.Trim() + "%");
            }
            if (ddltMastCustID.SelectedItem.Text.Trim() != string.Empty)
            {
                sWhere += " And BlongDeptID=" + ddltMastCustID.SelectedValue.Trim();
            }
            if (RefUser.UserID > 0)
                sWhere += " and UserID=" + RefUser.UserID.ToString();

            if (IsSelect && rbtnRU.Checked && Request.QueryString["flowid"] != null)
            {
                if (Request.QueryString["flowid"].ToString() != "")
                {
                    string CustID = "0";
                    string EquName = "";
                    string ServiceTypeID = "0";
                    string ServiceLevlID = "0";
                    string sCustName = string.Empty;
                    string sMasterName = string.Empty;
                    if (Request.QueryString["CustID"] != null)
                    {
                        CustID = Request.QueryString["CustID"].ToString() == "" ? "0" : Request.QueryString["CustID"].ToString();
                    }
                    if (Request.QueryString["EquID"] != null)
                    {
                        EquName = Request.QueryString["EquID"].ToString();
                        lblEquName.Text = Request.QueryString["EquName"].ToString();
                    }
                    if (Request.QueryString["ServiceTypeID"] != null)
                    {
                        ServiceTypeID = Request.QueryString["ServiceTypeID"].ToString() == "" ? "0" : Request.QueryString["ServiceTypeID"].ToString();
                        lblTypeName.Text = Request.QueryString["TypeName"].ToString();
                    }
                    if (Request.QueryString["ServiceLevelID"] != null)
                    {
                        ServiceLevlID = Request.QueryString["ServiceLevelID"].ToString() == "" ? "0" : Request.QueryString["ServiceLevelID"].ToString();
                        lblLevelName.Text = Request.QueryString["LevelName"].ToString();
                    }
                    if (Request.QueryString["MastCustName"] != null)
                    {
                        ltlMastCust.Text = Request.QueryString["MastCustName"].ToString();
                        sMasterName = ltlMastCust.Text;
                    }
                    if (Request.QueryString["CustName"] != null)
                    {
                        sCustName = Request.QueryString["CustName"].ToString();
                        lblCustName.Text = Request.QueryString["CustName"].ToString();
                    }
                    Cst_RecommendRuleDP eeRU = new Cst_RecommendRuleDP();
                    sWhere += " AND Cst_RecommendRuleDetails.StaffID = Cst_ServiceStaff.ID AND Cst_RecommendRuleDetails.RuleID IN (" + eeRU.GetRUPersonList(long.Parse(CustID), EquName, long.Parse(ServiceTypeID), long.Parse(ServiceLevlID), sCustName, sMasterName) + ",0)";
                }
            }
            Cst_ServiceStaffDP ee = new Cst_ServiceStaffDP();
            if (IsSelect && rbtnRU.Checked && Request.QueryString["flowid"] != null)
            {
                if (Request.QueryString["flowid"].ToString() != "")
                {
                    dt = ee.GetDataTable_RU(sWhere, sOrder, this.cpServiceStaff.PageSize, this.cpServiceStaff.CurrentPage, ref iRowCount);
                }
                else
                {
                    dt = ee.GetDataTable(sWhere, sOrder, this.cpServiceStaff.PageSize, this.cpServiceStaff.CurrentPage, ref iRowCount);
                }
            }
            else
            {
                dt = ee.GetDataTable(sWhere, sOrder, this.cpServiceStaff.PageSize, this.cpServiceStaff.CurrentPage, ref iRowCount);
            }
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
                Response.Redirect("frmCst_ServiceStaffEdit.aspx?id=" + e.Item.Cells[1].Text.ToString() + "&IsSelect=" + IsSelect.ToString().ToLower());
            }
            else if (e.CommandName == "Select")
            {
                StringBuilder sbText = new StringBuilder();

                sbText.Append("<script>");

                sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtUser", e.Item.Cells[1].Text.Trim().Replace("&nbsp;", ""));     //维修人ID
                sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hid_txtUser", e.Item.Cells[1].Text.Trim().Replace("&nbsp;", ""));     //维修人ID

                sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidUserName", e.Item.Cells[2].Text.Trim().Replace("&nbsp;", ""));     //维修人名称

                sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidUser", e.Item.Cells[3].Text.Trim().Replace("&nbsp;", ""));     //维修人名称

                sbText.Append("window.close();");
                sbText.Append("</script>");

                Page.RegisterStartupScript(DateTime.Now.Ticks.ToString(), sbText.ToString());
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
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i > 1 && i < e.Item.Cells.Count - 1)
                    {
                        j = i - 1;
                        if (i != 3)
                            e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                        else
                            e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",1);");
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSelect_Click(object sender, EventArgs e)
        {
            string sarrid = string.Empty;
            string sarrname = string.Empty;
            string sarrDeptName = string.Empty;
            foreach (DataGridItem itm in dgCst_ServiceStaff.Items)
            {
                if (itm.ItemType == ListItemType.AlternatingItem ||
                    itm.ItemType == ListItemType.Item)
                {
                    CheckBox chkdel = (CheckBox)itm.Cells[0].FindControl("chkDel");
                    if (chkdel.Checked)
                    {
                        sarrid += itm.Cells[1].Text + ",";
                        sarrname += itm.Cells[2].Text + ",";
                        sarrDeptName += itm.Cells[3].Text + ",";
                    }
                }
            }
            if (sarrid == string.Empty)
            {
                Epower.DevBase.BaseTools.PageTool.MsgBox(this, "请选择人员！");
                return;
            }
            if (!string.IsNullOrEmpty(RequestType))
            {
                BR_RPT_EngineerDP engineer = new BR_RPT_EngineerDP();
                engineer.UserID = decimal.Parse(Session["UserID"].ToString());
                engineer.SysType = RequestType;
                engineer.EngineerIds = sarrid.Substring(0, sarrid.Length - 1);
                engineer.EngineerNames = sarrname.Substring(0, sarrname.Length - 1);

                engineer.CreBy = engineer.LastMdfBy = Session["UserName"].ToString();
                engineer.CreTime = engineer.LastMdfTime = DateTime.Now;
                engineer.Insert(engineer);

               
            }

            if (TypeFrm == "frmCst_RecommonRuleEdit")
            {
                StringBuilder sbText = new StringBuilder();
                sbText.Append("<script>");
                sbText.Append("var arr = new Array();");

                sbText.Append("window.opener.document.getElementById('" + ObjID.Replace("cmdAddUserName", "txtAddUserName") + "').value='" + sarrname.Substring(0, sarrname.Length - 1) + "';");
                sbText.Append("window.opener.document.getElementById('" + ObjID.Replace("cmdAddUserName", "hidAddStaffID") + "').value='" + sarrid.Substring(0, sarrid.Length - 1) + "';");
                sbText.Append("window.opener.document.getElementById('" + ObjID.Replace("cmdAddUserName", "lblAddBlongDeptName") + "').innerText='" + sarrDeptName.Substring(0, sarrDeptName.Length - 1) + "';");
                sbText.Append("window.opener.document.getElementById('" + ObjID.Replace("cmdAddUserName", "hidAddBlongDeptName") + "').value='" + sarrDeptName.Substring(0, sarrDeptName.Length - 1) + "';");
                sbText.Append("window.close();");
                sbText.Append("</script>");
                // 向客户端发送
                Page.RegisterStartupScript(DateTime.Now.ToString(), sbText.ToString());
                Response.Write(sbText.ToString());

            }
            else if (TypeFrm == "ServiceStaffMastCust") 
            {
                StringBuilder sbText = new StringBuilder();
                sbText.Append("<script>");
                sbText.Append("var arr = new Array();");

                sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtUser", sarrname.Substring(0, sarrname.Length - 1));//姓名
                sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidUserName", sarrname.Substring(0, sarrname.Length - 1));//姓名
                sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidUser", sarrid.Substring(0, sarrid.Length - 1));//ID

                sbText.Append("window.close();");
                sbText.Append("</script>");
                // 向客户端发送
                Page.RegisterStartupScript(DateTime.Now.ToString(), sbText.ToString());
                Response.Write(sbText.ToString());
                
            }            
        }

        protected void dgCst_ServiceStaff_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                string sarrid = e.Item.Cells[1].Text;
                string sarrname = e.Item.Cells[2].Text;
                string sarrDeptName = e.Item.Cells[3].Text;
                if (IsSelect == false)
                {
                    e.Item.Attributes.Add("ondblclick", "window.open('../CustManager/frmCst_ServiceStaffEdit.aspx?id=" + e.Item.Cells[1].Text.ToString() + "&IsSelect=" + IsSelect.ToString().ToLower() + "&randomid='+GetRandom(),'MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
                }
                else
                {
                    e.Item.Attributes.Add("ondblclick", "doubleSelect('" + sarrid.ToString() + "', '" + StringTool.HidenFieldQ(sarrname) + "','" + StringTool.HidenFieldQ(sarrDeptName) + "');");
                }
            }
        }

        protected void rbtnRU_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void rbtnAll_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}