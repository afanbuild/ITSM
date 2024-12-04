using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.AppForms
{
	/// <summary>
	/// frmECustomers 的摘要说明。
	/// </summary>
    public partial class frmBr_ECustomerMain : BasePage
	{
        /// <summary>
        /// 设置父窗体按钮事件
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.CustumManager;
            this.Master.IsCheckRight = true;
            dgECustomer.Columns[8].Visible = this.Master.GetEditRight();
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);
            this.Master.Master_Button_ExportExcel_Click += new Global_BtnClick(Master_Master_Button_ExportExcel_Click);
            this.Master.ShowQueryPageButton();
            this.Master.ShowExportExcelButton(true);
            this.Master.MainID = "1";
        }

       
        /// <summary>
        /// 查询事件
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            Bind();
        }
        
        /// <summary>
        /// 新增事情
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("frmBr_ECustomerEdit.aspx");
        }

        /// <summary>
        /// 倒出EXCEL事件
        /// </summary>
        void Master_Master_Button_ExportExcel_Click()
        {
            string sWhere = "";
            if (ddltMastCustID.SelectedItem.Text.Trim() != string.Empty)
            {
                sWhere += " And E.MastCustID=" + ddltMastCustID.SelectedValue.Trim();
            }
            if (txtShortName.Text.Trim() != string.Empty)
            {
                sWhere += " And E.ShortName like " + StringTool.SqlQ("%" + txtShortName.Text.Trim() + "%");
            }
            if (txtFullName.Text.Trim() != string.Empty)
            {
                sWhere += " And E.FullName like " + StringTool.SqlQ("%" + txtFullName.Text.Trim() + "%");
            }
            if (ctrFCDServiceType.CatelogValue.ToString() != string.Empty)
            {
                sWhere += " And E.CustomerType=" + ctrFCDServiceType.CatelogID.ToString().Trim();
            }
            if (txtLinkMan1.Text.Trim() != string.Empty)
            {
                sWhere += " And E.LinkMan1 like " + StringTool.SqlQ("%" + txtLinkMan1.Text.Trim() + "%");
            }
            if (txtTel1.Text.Trim() != string.Empty)
            {
                sWhere += " And E.Tel1 like " + StringTool.SqlQ("%" + txtTel1.Text.Trim() + "%");
            }

            if (txtCustomCode.Text.Trim() != string.Empty)
            {
                sWhere += " And E.CustomCode like " + StringTool.SqlQ("%" + txtCustomCode.Text.Trim() + "%");
            }



            Br_ECustomerDP ee = new Br_ECustomerDP();
            DataTable dt = ee.GetCustomerServic(sWhere, "");
            Epower.ITSM.Web.Common.ExcelExport.ExportCustList(this, dt, Session["UserID"].ToString());
        }

        /// <summary>
        /// 删除事件
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            foreach (DataGridItem itm in dgECustomer.Items)
            {
                if (itm.ItemType == ListItemType.AlternatingItem ||
                    itm.ItemType == ListItemType.Item)
                {
                    string sID = itm.Cells[1].Text;
                    CheckBox chkdel = (CheckBox)itm.Cells[0].FindControl("chkDel");
                    try
                    {
                        labMsg.Visible = false;
                        if (chkdel.Checked)
                        {
                            Br_ECustomerDP ee = new Br_ECustomerDP();
                            ee.DeleteRecorded(long.Parse(sID));

                        }
                    }
                    catch (Exception ee)
                    {
                        labMsg.Visible = true;
                        labMsg.Text = ee.Message.ToString();
                    }
                }
            }
            Bind();
        }

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Page_Load(object sender, System.EventArgs e)
		{
            
            SetParentButtonEvent();
            cpfECustomerInfo.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(Bind);
			if(!IsPostBack)
			{
                //设置显示
                PageDeal.SetLanguage(this.Controls[0]);
                SetHeaderText();

                //绑定服务单位
                InitDropDownList();

                //加载数据
				Bind();
			}
		}

        #region 设置datagrid标头显示 余向前 2013-05-17
        /// <summary>
        /// 设置datagrid标头显示
        /// </summary>
        private void SetHeaderText()
        {
            dgECustomer.Columns[2].HeaderText = PageDeal.GetLanguageValue("Custom_CustName");
            dgECustomer.Columns[3].HeaderText = PageDeal.GetLanguageValue("Custom_FullName");
            dgECustomer.Columns[4].HeaderText = PageDeal.GetLanguageValue("Custom_MastCustName");
            dgECustomer.Columns[5].HeaderText = PageDeal.GetLanguageValue("Custom_CustomerType");
            dgECustomer.Columns[6].HeaderText = PageDeal.GetLanguageValue("Custom_Contact");
            dgECustomer.Columns[7].HeaderText = PageDeal.GetLanguageValue("Custom_CTel");

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
            ddltMastCustID.Items.Insert(0, new ListItem("",""));
        }

        /// <summary>
        /// 
        /// </summary>
        private void Bind()
		{
            string sWhere = " 1=1 AND a.MastCustID=b.id And a.Deleted=0 ";
            if (ddltMastCustID.SelectedItem.Text.Trim() != string.Empty)
            {
                sWhere += " And a.MastCustID=" + ddltMastCustID.SelectedValue.Trim();
            }
            if (txtShortName.Text.Trim() != string.Empty)
            {
                sWhere += " And a.ShortName like " + StringTool.SqlQ("%" + txtShortName.Text.Trim() + "%");
            }
            if (txtFullName.Text.Trim() != string.Empty)
            {
                sWhere += " And a.FullName like " + StringTool.SqlQ("%" + txtFullName.Text.Trim() + "%");
            }
            if (ctrFCDServiceType.CatelogValue.ToString() != string.Empty)
            {
                sWhere += " And a.CustomerType=" + ctrFCDServiceType.CatelogID.ToString().Trim();
            }
            if (txtLinkMan1.Text.Trim() != string.Empty)
            {
                sWhere += " And a.LinkMan1 like " + StringTool.SqlQ("%" + txtLinkMan1.Text.Trim() + "%");
            }
            if (txtTel1.Text.Trim() != string.Empty)
            {
                sWhere += " And a.Tel1 like " + StringTool.SqlQ("%" + txtTel1.Text.Trim() + "%");
            }

            if (txtCustomCode.Text.Trim() != string.Empty)
            {
                sWhere += " And a.CustomCode like " + StringTool.SqlQ("%" + txtCustomCode.Text.Trim() + "%");
            }

            int iRowCount = 0;
            Br_ECustomerDP ee = new Br_ECustomerDP();
            DataTable dt = ee.GetDataTable(sWhere, this.cpfECustomerInfo.PageSize, this.cpfECustomerInfo.CurrentPage, ref iRowCount, " order by a.ID desc ");
			dgECustomer.DataSource= dt;
			dgECustomer.DataBind();
            this.cpfECustomerInfo.RecordCount = iRowCount;
            this.cpfECustomerInfo.Bind();
            ViewState["PageCount"] = cpfECustomerInfo.CurrentPage;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgECustomer_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                Response.Redirect("frmBr_ECustomerEdit.aspx?id=" + e.Item.Cells[1].Text.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgECustomer_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i > 1 && i < 8)
                    {
                        int j = i - 1;  
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }

        /// <summary>
        /// 服务
        /// </summary>
        /// <param name="lngNoticeID"></param>
        /// <returns></returns>
        protected string GetServiceUrl(decimal lngID)
        {
            //暂时没处理分页
            string sUrl = "";
            sUrl = "javascript:window.open('frmIssueList.aspx?NewWin=true&ID=" + lngID.ToString() + "','','scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');";
            return sUrl;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngNoticeID"></param>
        /// <returns></returns>
        protected string GetUrl(decimal lngID)
        {
            //暂时没处理分页
            string sUrl = "";
            sUrl = "javascript:window.open('frmBYTSList.aspx?NewWin=true&ID=" + lngID.ToString() + "','','scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');";
            return sUrl;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgECustomer_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                string sID = DataBinder.Eval(e.Item.DataItem, "ID").ToString();
                e.Item.Attributes.Add("ondblclick", "window.open('frmBr_ECustomerEdit.aspx?ID=" + sID + "&randomid='+GetRandom(),'MainFrame','scrollbars=yes,resizable=yes')");

                ((Label)e.Item.FindControl("ShortName")).Attributes.Add("onmouseover", "ShowDetailsInfo(this," + DataBinder.Eval(e.Item.DataItem, "ID").ToString() + ",400);");
            }
        }
	}
}
