/*******************************************************************
 * 版权所有： 深圳市非凡信息技术有限公司
 * 描述：设置桌面项展示

 * 
 * 
 * 创建人 ：zhumingchun
 * 创建日期：2007-11-29
 * 
 * 修改日志：
 * 修改时间：2010-01-12 修改人：zhumingchun 
 * 修改描述：
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

using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;

namespace Epower.ITSM.Web.mydestop
{
    /// <summary>
    /// 
    /// </summary>
    public partial class frmEa_DefineMainPageMain : BasePage
    {
        #region 变量区
        #endregion 

        #region 属性区
        #endregion 

        #region 方法区
        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.SystemManager;
            this.Master.IsCheckRight = true;
            dgEa_DefineMainPage.Columns[8].Visible = this.Master.GetEditRight();
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
            Response.Redirect("frmEa_DefineMainPageEdit.aspx");
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
			Ea_DefineMainPageDP ee = new Ea_DefineMainPageDP();
            foreach (DataGridItem itm in dgEa_DefineMainPage.Items)
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
			ControlPage1.On_PostBack+=new EventHandler(ControlPage1_On_PostBack);
			ControlPage1.DataGridToControl=dgEa_DefineMainPage;
			if(!IsPostBack)
			{
				DataTable dt = LoadData();
                dgEa_DefineMainPage.DataSource = dt.DefaultView;
                dgEa_DefineMainPage.DataBind();
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
			if(txtTitle.Text.Trim()!=string.Empty)
			{
				  sWhere +=" And Title like " + StringTool.SqlQ("%" + txtTitle.Text.Trim() + "%");
			}
			Ea_DefineMainPageDP ee = new Ea_DefineMainPageDP();
            dt = ee.GetDataTable(sWhere,sOrder); ;
            Session["Ea_DefineMainPage"] = dt;
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
            if (Session["Ea_DefineMainPage"] == null)
            {
                dt = LoadData();
            }
            else
            {
                dt = (DataTable)Session["Ea_DefineMainPage"];
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
                Response.Redirect("frmEa_DefineMainPageEdit.aspx?id=" + e.Item.Cells[1].Text.ToString());
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

        #region 数据绑定 dgEa_DefineMainPage_ItemDataBound
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgEa_DefineMainPage_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                string sID = DataBinder.Eval(e.Item.DataItem, "ID").ToString();
                e.Item.Attributes.Add("ondblclick", "window.open('frmEa_DefineMainPageEdit.aspx?id=" + sID + "&randomid='+GetRandom(),'MainFrame','scrollbars=yes,resizable=yes')");

                if (e.Item.Cells[3].Text.Trim() == "0")
                {
                    e.Item.Cells[3].Text = "左边";
                }
                else if (e.Item.Cells[3].Text.Trim() == "1")
                {
                    e.Item.Cells[3].Text = "右边";
                }
                if (e.Item.Cells[4].Text.Trim() == "0")
                {
                    e.Item.Cells[4].Text = "显示";
                }
                else if (e.Item.Cells[4].Text.Trim() == "1")
                {
                    e.Item.Cells[4].Text = "不显示";
                }
            }
        }
        #endregion 
        #endregion 
    }
}