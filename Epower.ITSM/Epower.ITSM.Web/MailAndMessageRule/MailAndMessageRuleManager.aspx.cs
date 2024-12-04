using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.SqlDAL;
using EpowerCom;
using Epower.ITSM.Base;

namespace Epower.ITSM.Web.MailAndMessageRule
{
    public partial class MailAndMessageRuleManager : BasePage
    {

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);
            this.Master.Master_Button_ExportExcel_Click += new Global_BtnClick(Master_Master_Button_ExportExcel_Click);

            this.Master.setButtonRigth(Constant.SystemManager, false);

            RightEntity re = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.SystemManager];
            if (re != null)
            {
                dgMailMessageTem.Columns[dgMailMessageTem.Columns.Count - 2].Visible = re.CanModify;
            }

            this.Master.ShowExportExcelButton(false);
            this.Master.ShowQueryButton(true);
            this.Master.MainID = "1";
        }
        #endregion

        #region Master_Master_Button_ExportExcel_Click 倒出EXCEL
        /// <summary>
        /// 倒出EXCEL事件
        /// </summary>
        void Master_Master_Button_ExportExcel_Click()
        {
            DataTable dt = Bind("");
            Epower.ITSM.Web.Common.ExcelExport.ExportMailMessageTemList(this, dt, Session["UserID"].ToString());
        }

        #endregion

        #region Master_Master_Button_New_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("MailMessageRuleEdit.aspx");
        }
        #endregion

        #region Master_Master_Button_Query_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            Bind("");
        }
        #endregion

        #region Master_Master_Button_Delete_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            MailAndMessageRuleDP ee = new MailAndMessageRuleDP();
            foreach (DataGridItem itm in dgMailMessageTem.Items)
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

            Bind("");
        }
        #endregion

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtonEvent();
            CtrcpfMailMessage.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(bindGrid);
            if (!IsPostBack)
            {
                ddlBind();
                Bind("");
            }
        }

        #endregion

        #region 翻页绑定dagagrid
        //翻页绑定dagagrid
        public void bindGrid()
        {
            Bind("");
        }
        #endregion

        private void ddlBind()
        {
            cboApp.DataSource = epApp.GetAllApps().DefaultView;
            cboApp.DataTextField = "AppName";
            cboApp.DataValueField = "AppID";
            cboApp.DataBind();

            //cboApp.Items.Remove(new ListItem("通用流程", "199"));
            cboApp.Items.Remove(new ListItem("进出操作间", "1027"));

            ListItem itm = new ListItem("", "-1");
            cboApp.Items.Insert(0, itm);
            cboApp.SelectedIndex = 0;

            cboFlowModel.DataSource = MailAndMessageRuleDP.getAllFlowModel("").DefaultView;
            cboFlowModel.DataTextField = "FlowName";
            cboFlowModel.DataValueField = "OFlowModelID";
            cboFlowModel.DataBind();

            ListItem itm1 = new ListItem("", "-1");
            cboFlowModel.Items.Insert(0, itm1);
            cboFlowModel.SelectedIndex = 0;
        }

        #region Bind
        /// <summary>
        /// 
        /// </summary>
        private DataTable Bind(string Strsubjectid)
        {
            int iRowCount = 0;
            DataTable dt;
            string sWhere = " 1=1 ";
            string sOrder = " order by ID Desc";

            #region 查询条件

            #region RuleName

            if (txtRuleName.Text.ToString() != string.Empty)
                sWhere += " and RuleName like " + StringTool.SqlQ("%" + txtRuleName.Text.ToString() + "%");

            #endregion

            #region Status
            if (ddlStatus.SelectedItem.Text.Trim() != "")
                sWhere += " and Status= " + ddlStatus.SelectedItem.Value.ToString();
            #endregion

            #region SystemName

            if (cboApp.SelectedItem.Value.ToString() != "-1")
                sWhere += " and SystemID = " + cboApp.SelectedItem.Value.ToString();
            #endregion

            #region ModelName
            if (cboFlowModel.SelectedItem.Value.ToString() != "-1")
                sWhere += " and ModelID = " + cboFlowModel.SelectedItem.Value.ToString();
            #endregion

            #endregion

            MailAndMessageRuleDP ee = new MailAndMessageRuleDP();
            dt = ee.GetDataTable(sWhere, sOrder, this.CtrcpfMailMessage.PageSize, this.CtrcpfMailMessage.CurrentPage, ref iRowCount);
            dgMailMessageTem.DataSource = dt;
            dgMailMessageTem.DataBind();
            this.CtrcpfMailMessage.RecordCount = iRowCount;
            this.CtrcpfMailMessage.Bind();
            return dt;
        }
        #endregion

        #region  dgMailMessageTem_ItemCommand
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgMailMessageTem_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                Response.Redirect("MailMessageRuleEdit.aspx?id=" + e.Item.Cells[1].Text.ToString());
            }
        }
        #endregion

        #region dgMailMessageTem_ItemDataBound
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgMailMessageTem_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                String sID = DataBinder.Eval(e.Item.DataItem, "ID").ToString();
                e.Item.Attributes.Add("ondblclick", "window.open('MailMessageRuleEdit.aspx?id=" + sID.ToString() + "&randomid='+GetRandom(),'MainFrame','scrollbars=yes,resizable=yes')");
                Label lbStatus = e.Item.FindControl("lbStatus") as Label;
                switch (lbStatus.Text)
                {
                    case "0": lbStatus.Text = "禁用"; break;
                    case "1": lbStatus.Text = "启用"; break;
                    default: lbStatus.Text = "其它"; break;
                }
                //((Label)e.Item.FindControl("lbTemplateName")).Attributes.Add("onmouseover", "ShowDetailsInfo(this," + DataBinder.Eval(e.Item.DataItem, "TemplateName").ToString() + ",400);");
            }
        }
        #endregion

        #region dgMailMessageTem_ItemCreated
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgMailMessageTem_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count - 2; i++)
                {
                    if (i > 1)
                    {
                        int j = i - 1;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
        #endregion

        protected void cboApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            string stWhere = cboApp.SelectedItem.Value == "-1" ? "" : " AND AppID=" + cboApp.SelectedItem.Value;
            stWhere = stWhere + " and status=1 and deleted=0 ";
            cboFlowModel.DataSource = MailAndMessageRuleDP.getAllFlowModel(stWhere).DefaultView;
            cboFlowModel.DataTextField = "flowname";
            cboFlowModel.DataValueField = "oflowmodelid";
            cboFlowModel.DataBind();

            ListItem itm1 = new ListItem("", "-1");
            cboFlowModel.Items.Insert(0, itm1);
            cboFlowModel.SelectedIndex = 0;
        }
    }
}
