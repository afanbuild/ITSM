/*******************************************************************
 *
 * Description:系统系列号表维护
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
    public partial class frmTs_SequenceMain : BasePage
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
            Response.Redirect("frmTs_SequenceEdit.aspx");
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
            Ts_SequenceDP ee = new Ts_SequenceDP();
            foreach (DataGridItem itm in dgTs_Sequence.Items)
            {
                if (itm.ItemType == ListItemType.AlternatingItem ||
                    itm.ItemType == ListItemType.Item)
                {
                    string sID = itm.Cells[1].Text;
                    CheckBox chkdel = (CheckBox)itm.Cells[0].FindControl("chkDel");
                    if (chkdel.Checked)
                    {
                        ee.DeleteRecorded(sID);
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
            cpTs_Sequence.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(BindData);
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
            string sOrder = " order by Name";
            if (txtName.Text.Trim() != string.Empty)
            {
                sWhere += " And Name like " + StringTool.SqlQ("%" + txtName.Text.Trim() + "%");
            }
            Ts_SequenceDP ee = new Ts_SequenceDP();
            int iRowCount = 0;
            dt = ee.GetDataTable(sWhere, sOrder, this.cpTs_Sequence.PageSize, this.cpTs_Sequence.CurrentPage, ref iRowCount);
            dgTs_Sequence.DataSource = dt.DefaultView;
            dgTs_Sequence.DataBind();
            this.cpTs_Sequence.RecordCount = iRowCount;
            this.cpTs_Sequence.Bind();
        }
        #endregion

        #region  dgTs_Sequence_ItemCommand
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgTs_Sequence_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                Response.Redirect("frmTs_SequenceEdit.aspx?id=" + e.Item.Cells[1].Text.ToString());
            }
        }
        #endregion

        #region dgTs_Sequence_ItemCreated
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgTs_Sequence_ItemCreated(object sender, DataGridItemEventArgs e)
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
                        if (j == 1)
                            e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                        else
                            e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",1);");
                    }
                }
            }
        }
        #endregion

        #region dgTs_Sequence_ItemDataBound
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgTs_Sequence_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                string sID = DataBinder.Eval(e.Item.DataItem, "Name").ToString();
                e.Item.Attributes.Add("ondblclick", "window.open('frmTs_SequenceEdit.aspx?ID=" + sID + "&randomid='+GetRandom(),'MainFrame','scrollbars=yes,resizable=yes')");
            }
        }
        #endregion
    }
}


