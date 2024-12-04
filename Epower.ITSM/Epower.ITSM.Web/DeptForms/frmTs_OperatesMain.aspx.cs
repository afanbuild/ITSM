/*******************************************************************
 *
 * Description:操作项维护
 * 
 * 
 * Create By  :zhumc
 * Create Date:2011年5月19日
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
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;

namespace Epower.ITSM.Web.DeptForms
{
    public partial class frmTs_OperatesMain : BasePage
    {
        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Masster_Master_Button_Delete_Click);
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
            Response.Redirect("frmTs_OperatesEdit.aspx");
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
        void Masster_Master_Button_Delete_Click()
        {
            Ts_OperatesDP ee = new Ts_OperatesDP();
            foreach (DataGridItem itm in dgTs_Operates.Items)
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
            cpTs_Operates.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(BindData);
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
            DataTable dt;
            string sWhere = " 1=1 ";
            string sOrder = " order by OperateID";
            if (txtOpName.Text.Trim() != string.Empty)
            {
                sWhere += " And OpName like " + StringTool.SqlQ("%" + txtOpName.Text.Trim() + "%");
            }
            Ts_OperatesDP ee = new Ts_OperatesDP();
            int iRowCount = 0;
            dt = ee.GetDataTable(sWhere, sOrder, this.cpTs_Operates.PageSize, this.cpTs_Operates.CurrentPage, ref iRowCount);
            dgTs_Operates.DataSource = dt.DefaultView;
            dgTs_Operates.DataBind();
            this.cpTs_Operates.RecordCount = iRowCount;
            this.cpTs_Operates.Bind();
        }
        #endregion

        #region  dgTs_Operates_ItemCommand
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgTs_Operates_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                Response.Redirect("frmTs_OperatesEdit.aspx?id=" + e.Item.Cells[1].Text.ToString());
            }
        }
        #endregion

        #region dgTs_Operates_ItemCreated
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgTs_Operates_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                int j = 0;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i > 0 && i < e.Item.Cells.Count - 1)
                    {
                        j = i - 0;
                        if(j==1)
                            e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",1);");
                        else
                            e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
        #endregion

        #region dgTs_Operates_ItemDataBound
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgTs_Operates_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                string sID = DataBinder.Eval(e.Item.DataItem, "OperateID").ToString();
                e.Item.Attributes.Add("ondblclick", "window.open('frmTs_OperatesEdit.aspx?ID=" + sID + "&randomid='+GetRandom(),'MainFrame','scrollbars=yes,resizable=yes')");
            }
        }
        #endregion
    }
}
