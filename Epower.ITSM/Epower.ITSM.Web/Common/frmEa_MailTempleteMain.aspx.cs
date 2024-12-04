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

namespace Epower.ITSM.Web.Common
{
    public partial class frmEa_MailTempleteMain : BasePage
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
            Response.Redirect("frmEa_MailTempleteEdit.aspx");
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
            dgEa_MailTemplete.DataSource = dt.DefaultView;
            dgEa_MailTemplete.DataBind();
        }
        #endregion 

		#region Master_Master_Button_Delete_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
			Ea_MailTempleteDP ee = new Ea_MailTempleteDP();
            foreach (DataGridItem itm in dgEa_MailTemplete.Items)
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
            dgEa_MailTemplete.DataSource = dt.DefaultView;
            dgEa_MailTemplete.DataBind();
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
			ControlPage1.DataGridToControl=dgEa_MailTemplete;
			if(!IsPostBack)
			{
				DataTable dt = LoadData();
                dgEa_MailTemplete.DataSource = dt.DefaultView;
                dgEa_MailTemplete.DataBind();
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
			if(txtMailTitle.Text.Trim()!=string.Empty)
			{
				  sWhere +=" And MailTitle like " + StringTool.SqlQ("%" + txtMailTitle.Text.Trim() + "%");
			}
			
			Ea_MailTempleteDP ee = new Ea_MailTempleteDP();
            dt = ee.GetDataTable(sWhere,sOrder); ;
            Session["Ea_MailTemplete"] = dt;
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
            if (Session["Ea_MailTemplete"] == null)
            {
                dt = LoadData();
            }
            else
            {
                dt = (DataTable)Session["Ea_MailTemplete"];
            }
            dgEa_MailTemplete.DataSource = dt.DefaultView;
            dgEa_MailTemplete.DataBind();
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
		
		#region  dgEa_MailTemplete_ItemCommand
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgEa_MailTemplete_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                Response.Redirect("frmEa_MailTempleteEdit.aspx?id=" + e.Item.Cells[1].Text.ToString());
            }
            if (e.CommandName == "SendMail")
            {
                Response.Redirect("frmEa_MailTempleteSend.aspx?id=" + e.Item.Cells[1].Text.ToString());
            }
        }
        #endregion 
		
		#region dgEa_MailTemplete_ItemCreated
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgEa_MailTemplete_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                int j = 0;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i > 1 && i < e.Item.Cells.Count-2)
                    {
                        j = i - 1;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
        #endregion 
    }
}
