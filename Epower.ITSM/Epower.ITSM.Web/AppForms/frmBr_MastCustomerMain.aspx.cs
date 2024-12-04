/*******************************************************************
 *
 * Description:服务单位管理
 * 
 * 
 * Create By  :zhumc
 * Create Date:2008年4月2日
 * *****************************************************************/
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
    /// frmBr_MastCustomerMain 的摘要说明。
	/// </summary>
    public partial class frmBr_MastCustomerMain : BasePage
	{

		RightEntity re = null;
		protected int iDisabled = 0;

        /// <summary>
        /// 设置父窗体按钮事件
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.ServiceDept;
            this.Master.IsCheckRight = true;
            dgECustomer.Columns[8].Visible = this.Master.GetEditRight();
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);
            this.Master.ShowQueryPageButton();
            this.Master.MainID = "1";
        }

        /// <summary>
        /// 新增事情
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("frmBr_MastCustomerEdit.aspx");
        }

        /// <summary>
        /// 查询事件
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            LoadData();
        }

        /// <summary>
        /// 删除事件
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            string custusName = string.Empty;
            int iValue = 0;
            foreach (DataGridItem itm in dgECustomer.Items)
            {
                if (itm.ItemType == ListItemType.AlternatingItem ||
                    itm.ItemType == ListItemType.Item)
                {
                    string sID = itm.Cells[1].Text;
                    CheckBox chkdel = (CheckBox)itm.Cells[0].FindControl("chkDel");
                    try
                    {
                        if (chkdel.Checked)
                        {
                            if (Br_ECustomerDP.GetmastCustId(sID) != true)
                            {
                                Br_MastCustomerDP ee = new Br_MastCustomerDP();
                                ee.DeleteRecorded(long.Parse(sID));
                            }
                            else
                            {
                                if (iValue == 0)
                                {
                                    custusName = itm.Cells[2].Text;
                                }
                                else
                                {
                                    custusName += "," + itm.Cells[2].Text;
                                }
                                iValue++;
                            }

                        }
                    }
                    catch (Exception ee)
                    {
                    }
                }
            }
       
            //强制相关缓存失效 
            HttpRuntime.Cache.Insert("CommCacheValidMastCustomer", false);
            LoadData();

            if (custusName != "")
            {
                Response.Write("<script>alert('服务名称[" + custusName + "]已在客户中用到，不能删除！')</script>");
            }
        }

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Page_Load(object sender, System.EventArgs e)
		{
            SetParentButtonEvent();
            cpServiceStaff.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(LoadData);
			if(!IsPostBack)
			{
				LoadData();
			}
		}

		#region Web 窗体设计器生成的代码
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sCallNO"></param>
        /// <returns></returns>
		protected string GetUrl(string sCallNO)
		{
			return "javascript:Edit("+sCallNO+")";	
		}

        /// <summary>
        /// 
        /// </summary>
		private void LoadData()
		{
            string sWhere = string.Empty;
            string sOrder = string.Empty;
            if (txtShortName.Text.Trim() != string.Empty)
            {
                sWhere += " And ShortName like " + StringTool.SqlQ("%" + txtShortName.Text.Trim() + "%");
            }
            if (txtFullName.Text.Trim() != string.Empty)
            {
                sWhere += " And FullName like " + StringTool.SqlQ("%" + txtFullName.Text.Trim() + "%");
            }
            if (ctrFCDServiceType.CatelogValue.ToString() != string.Empty )
            {
                sWhere += " And EnterpriseType=" + ctrFCDServiceType.CatelogID.ToString().Trim();
            }
            if (txtTel1.Text.Trim() != string.Empty)
            {
                sWhere += " And Tel1 like " + StringTool.SqlQ("%" + txtTel1.Text.Trim() + "%");
            }
            if (txtLinkMan1.Text.Trim() != string.Empty)
            {
                sWhere += " And LinkMan1 like " + StringTool.SqlQ("%" + txtLinkMan1.Text.Trim() + "%");
            }
            if (txtFax1.Text.Trim() != string.Empty)
            {
                sWhere += " And Fax1 like " + StringTool.SqlQ("%" + txtFax1.Text.Trim() + "%");
            }

            Br_MastCustomerDP ee = new Br_MastCustomerDP();
            int iRowCount = 0;
            DataTable dt = ee.GetDataTable(sWhere, this.cpServiceStaff.PageSize, this.cpServiceStaff.CurrentPage, ref iRowCount);
			dgECustomer.DataSource=dt.DefaultView;
			dgECustomer.DataBind();
            this.cpServiceStaff.RecordCount = iRowCount;
            this.cpServiceStaff.Bind();
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
                Response.Redirect("frmBr_MastCustomerEdit.aspx?id=" + e.Item.Cells[1].Text.ToString());
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
                for (int i = 1; i < e.Item.Cells.Count-1; i++)
                {
                    int j = i - 1;
                    e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                }
            }
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

                e.Item.Attributes.Add("ondblclick", "window.open('frmBr_MastCustomerEdit.aspx?id=" + e.Item.Cells[1].Text.ToString() + "','MainFrame','');");
            }
        }
	}
}
