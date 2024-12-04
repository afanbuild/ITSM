
/*******************************************************************
 *
 * Description
 * 
 * 服务台管理页面
 * Create By  :yxq
 * Create Date:2011年5月21日 星期六
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
using Epower.ITSM.Base;

namespace Epower.ITSM.Web.AppForms
{
    public partial class frmEa_SetdeskMain : BasePage
    {
        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.CEa_Setdesk;
            this.Master.IsCheckRight = true;
            this.dgEa_Setdesk.Columns[this.dgEa_Setdesk.Columns.Count - 1].Visible = this.Master.GetEditRight();
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
            Response.Redirect("frmEa_SetdeskEdit.aspx");
        }
        #endregion

        #region Master_Master_Button_Query_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            BindData();
        }
        #endregion

        #region Master_Master_Button_Delete_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            Ea_SetdeskDP ee = new Ea_SetdeskDP();
            foreach (DataGridItem itm in dgEa_Setdesk.Items)
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
            BindData();
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
            cpEa_Setdesk.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(BindData);
            if (!IsPostBack)
            {
                BindData();
            }
        }
        #endregion

        #region BindData
        /// <summary>
        /// 
        /// </summary>
        private void BindData()
        {
            string sWhere = " 1=1 And Deleted=0 ";
            string sOrder = " order by id";

            if (UserPicker1.UserID>0)
            {
                sWhere += " And UserID = " + UserPicker1.UserID;
            }
            if (txtBlockRoom.Text != "")
            {
                sWhere += " And BlockRoom = " + StringTool.SqlQ(txtBlockRoom.Text);
            }

            Ea_SetdeskDP ee = new Ea_SetdeskDP();
            DataTable dt;
            int iRowCount = 0;
            dt = ee.GetDataTable(sWhere, sOrder, this.cpEa_Setdesk.PageSize, this.cpEa_Setdesk.CurrentPage, ref iRowCount);
            dgEa_Setdesk.DataSource = dt.DefaultView;
            dgEa_Setdesk.DataBind();
            this.cpEa_Setdesk.RecordCount = iRowCount;
            this.cpEa_Setdesk.Bind();
        }
        #endregion

        #region  dgEa_Setdesk_ItemCommand
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgEa_Setdesk_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                Response.Redirect("frmEa_SetdeskEdit.aspx?id=" + e.Item.Cells[1].Text.ToString());
            }
        }
        #endregion

        #region dgEa_Setdesk_ItemCreated
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgEa_Setdesk_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                int j = 0;
                for (int i = 0; i < e.Item.Cells.Count-1; i++)
                {
                    if (i > 0)
                    {
                        j = i - 2;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }

                }
            }
        }
        #endregion

        #region dgEa_Setdesk_ItemDataBound
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgEa_Setdesk_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                string sID = DataBinder.Eval(e.Item.DataItem, "ID").ToString();
                e.Item.Attributes.Add("ondblclick", "window.open('frmEa_SetdeskEdit.aspx?ID=" + sID + "&randomid='+GetRandom(),'MainFrame','scrollbars=yes,resizable=yes')");
            }
        }
        #endregion
    }
}