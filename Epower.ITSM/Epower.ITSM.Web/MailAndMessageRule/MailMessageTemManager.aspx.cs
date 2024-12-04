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
using System.Text;
using Epower.ITSM.Base;
using EpowerCom;

namespace Epower.ITSM.Web.MailAndMessageRule
{
    public partial class MailMessageTemManager : BasePage
    {
        protected bool IsSelect
        {
            get { if (Request["IsSelect"] != null) return true; else return false; }
        }

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.SystemManager;

            if (IsSelect)
                this.Master.IsCheckRight = false;
            else
            {
                this.Master.IsCheckRight = true;
                //dgMailMessageTem.Columns[10].Visible = this.Master.GetEditRight();
            }
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);
            this.Master.Master_Button_ExportExcel_Click += new Global_BtnClick(Master_Master_Button_ExportExcel_Click);

            this.Master.ShowExportExcelButton(true);
            this.Master.ShowQueryButton(true);
            this.Master.setButtonRigth(Constant.SystemManager, false);
            RightEntity re = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.SystemManager];
            if (re != null)
            {
                dgMailMessageTem.Columns[dgMailMessageTem.Columns.Count - 1].Visible = re.CanModify;
            }


            this.Master.MainID = "1";

            if (IsSelect)  //如果为选择时
            {
                this.Master.ShowNewButton(false);
                this.Master.ShowDeleteButton(false);
                this.Master.ShowExportExcelButton(false);
                dgMailMessageTem.Columns[0].Visible = false;
            }
        }
        #endregion
        #region  获得路径传过来的参数
        /// <summary>
        /// 获得路径传过来的参数
        /// </summary>
        public string Opener_ClientId
        {

            get
            {
                return (Request["Opener_ClientId"] == null) ? "" : Request["Opener_ClientId"].ToString().Replace("hidClientId_ForOpenerPage", "");
            }
        }
        public string Type 
        {
            get { return String.IsNullOrEmpty(Request["typeID"]) ? "" : Request["typeID"].ToString(); }
        }

        public string ObjID
        {
            get { return String.IsNullOrEmpty(Request["objID"]) ? "" : Request["objID"].ToString(); }
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
            Response.Redirect("MailMessageTemEdit.aspx");
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
            string strTempName = "";
            MailMessageTemManagerDP ee = new MailMessageTemManagerDP();
            foreach (DataGridItem itm in dgMailMessageTem.Items)
            {
                if (itm.ItemType == ListItemType.AlternatingItem ||
                    itm.ItemType == ListItemType.Item)
                {
                    string sID = itm.Cells[1].Text;
                    CheckBox chkdel = (CheckBox)itm.Cells[0].FindControl("chkDel");
                    if (chkdel.Checked)
                    {
                        string tempName = ee.selectMailAndMessage(long.Parse(sID));
                        if (tempName == "")
                        {
                            ee.DeleteRecorded(long.Parse(sID));
                        }
                        else
                        {
                            strTempName += tempName + ",";
                        }
                    }
                }
            }
            Bind("");
            if (strTempName != "")
            {
                PageTool.MsgBox(this.Page, "模板名称：" + strTempName.Trim(',') + " 已在短信邮件规则中用到，删除失败，请先删除与模板" + strTempName.Trim(',') + "的相关短信邮件规则！");
            }
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

                if (IsSelect && Request.QueryString["systemID"] != null)
                {
                    cboApp.SelectedIndex = cboApp.Items.IndexOf(cboApp.Items.FindByValue(Request.QueryString["systemID"].ToString()));
                    cboApp.Enabled = false;
                }

                Bind("");
            }
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

        }

        #region 翻页绑定dagagrid
        //翻页绑定dagagrid
        public void bindGrid()
        {
            Bind("");
        }
        #endregion

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

            #region TemplateName

            if (txtTemplateName.Text.ToString() != string.Empty)
                sWhere += " and TemplateName like " + StringTool.SqlQ("%" + txtTemplateName.Text.ToString() + "%");

            #endregion

            #region SystemID
            if (cboApp.SelectedItem.Value != "" && decimal.Parse(cboApp.SelectedItem.Value) > 0)
                sWhere += " and SystemID = " + cboApp.SelectedItem.Value;
            #endregion

            #region Status
            if (ddlStatus.SelectedItem.Text.Trim() != "")
                sWhere += " and Status= " + ddlStatus.SelectedItem.Value.ToString();
            #endregion

            #region MailContent
            if (txtMailContent.Text.Trim() != "")
                sWhere += " and MailContent like " + StringTool.SqlQ("%" + txtMailContent.Text.ToString() + "%");
            #endregion

            #region ModelContent
            if (txtModelContent.Text.Trim() != "")
                sWhere += " and ModelContent like " + StringTool.SqlQ("%" + txtModelContent.Text.ToString() + "%");
            #endregion

            #endregion

            MailMessageTemManagerDP ee = new MailMessageTemManagerDP();
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
                if (!IsSelect)
                    Response.Redirect("MailMessageTemEdit.aspx?id=" + e.Item.Cells[1].Text.ToString());
                else
                {
                    StringBuilder sbText = new StringBuilder();
                    sbText.Append("<script>");
                    sbText.Append("var arr = new Array();");
                    // ID
                    sbText.Append("arr[0] ='" + e.Item.Cells[1].Text + "';");
                    // 名称
                    sbText.Append("arr[1] ='" + ((Label)e.Item.FindControl("lbTemplateName")).Text + "';");


                    if (Type == "1")
                    {
                        sbText.Append("window.opener.document.getElementById('" + ObjID.Replace("cmdTem", "txtTName") + "').value = '" + ((Label)e.Item.FindControl("lbTemplateName")).Text + "';");
                        sbText.Append("window.opener.document.getElementById('" + ObjID.Replace("cmdTem", "hidTNname") + "').value = '" + ((Label)e.Item.FindControl("lbTemplateName")).Text + "';");
                        sbText.Append("window.opener.document.getElementById('" + ObjID.Replace("cmdTem", "hidTNid") + "').value = '" + e.Item.Cells[1].Text + "';");
                    }
                    else if (Type == "0")
                    {
                        sbText.Append("window.opener.document.getElementById('" + ObjID.Replace("cmdTem", "txtTName") + "').value = '" + ((Label)e.Item.FindControl("lbTemplateName")).Text + "';");
                        sbText.Append("window.opener.document.getElementById('" + ObjID.Replace("cmdTem", "hidTNname") + "').value = '" + ((Label)e.Item.FindControl("lbTemplateName")).Text + "';");
                        sbText.Append("window.opener.document.getElementById('" + ObjID.Replace("cmdTem", "hidTNid") + "').value = '" + e.Item.Cells[1].Text + "';");
                        
                    }
                    else
                    {
                        sbText.Append("window.opener.document.getElementById('" + ObjID.Replace("cmdAddTem", "txtAddTName") + "').value = '" + ((Label)e.Item.FindControl("lbTemplateName")).Text + "';");
                        sbText.Append("window.opener.document.getElementById('" + ObjID.Replace("cmdAddTem", "hidAddTNname") + "').value = '" + ((Label)e.Item.FindControl("lbTemplateName")).Text + "';");
                        sbText.Append("window.opener.document.getElementById('" + ObjID.Replace("cmdAddTem", "hidAddTNid") + "').value = '" + e.Item.Cells[1].Text + "';");
                    }

                  //  sbText.Append("window.parent.returnValue = arr;");
                    // 关闭窗口
                    sbText.Append("top.close();");
                    sbText.Append("</script>");
                    // 向客户端发送


                    Page.RegisterStartupScript(DateTime.Now.ToString(), sbText.ToString());
                    Response.Write(sbText.ToString());
                }

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
                Button lnkedit = (Button)e.Item.FindControl("lnkedit");
                if (IsSelect)
                {
                    lnkedit.Text = "选择";
                }

                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");

                if (e.Item.Cells[3].Text.Length > 40)
                    e.Item.Cells[3].Text = e.Item.Cells[3].Text.Substring(0, 40) + "... ...";

                String sID = DataBinder.Eval(e.Item.DataItem, "ID").ToString();

                Label lbStatus = e.Item.FindControl("lbStatus") as Label;
                switch (lbStatus.Text)
                {
                    case "0": lbStatus.Text = "禁用"; break;
                    case "1": lbStatus.Text = "启用"; break;
                    default: lbStatus.Text = "其它"; break;
                }
                if (!IsSelect)
                {
                    e.Item.Attributes.Add("ondblclick", "window.open('MailMessageTemEdit.aspx?id=" + sID.ToString() + "&randomid='+GetRandom(),'MainFrame','scrollbars=yes,resizable=yes')");
                }
                else
                {

                    //ID号


                    string value1 = e.Item.Cells[1].Text.Trim().Replace("&nbsp;", "");
                    // 名称
                    string value2 = ((Label)e.Item.FindControl("lbTemplateName")).Text.Replace("&nbsp;", "");
                    // 向客户端发送

                    e.Item.Attributes.Add("ondblclick", "OnClientClick('" + lnkedit.ClientID + "');");

                }

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
                for (int i = 0; i < e.Item.Cells.Count - 1; i++)
                {
                    if (i > 1)
                    {
                        int j = i - 4;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
        #endregion
        public string DisplayBySystemId(string systemId) 
        {
           return epApp.GetAppName(long.Parse(systemId)); 
        }
    }
}
