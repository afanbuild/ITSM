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
using Epower.ITSM.SqlDAL.Print;
using EpowerGlobal;
using Epower.ITSM.SqlDAL.Service;

namespace Epower.ITSM.Web.Print
{
    public partial class frm_EmailIssue : BasePage
    {

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);
           
            this.Master.ShowQueryPageButton();
            this.Master.ShowDeleteButton(true);
            this.Master.ShowExportExcelButton(false);
            this.Master.ShowNewButton(false);
            this.Master.MainID = "1";
        }
        #endregion

      
     

        #region Master_Master_Button_Query_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            Bind();
        }
        #endregion

        #region Master_Master_Button_Delete_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            //PRINTRULE Entity = new PRINTRULE();
            foreach (DataGridItem itm in dgMailMessageTem.Items)
            {
                if (itm.ItemType == ListItemType.AlternatingItem ||
                    itm.ItemType == ListItemType.Item)
                {
                    string sID = itm.Cells[1].Text;
                    CheckBox chkdel = (CheckBox)itm.Cells[0].FindControl("chkDel");
                    if (chkdel.Checked)
                    {
                       EmialIssue.DeleteEmailIssue(long.Parse(sID));
                    }
                }
            }

            Bind();
        }
        #endregion

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtonEvent();
            CtrcpfMailMessage.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(bindGrid);
            if (!IsPostBack)
            {
                Bind();
            }
        }

        #endregion

        #region 翻页绑定dagagrid
        //翻页绑定dagagrid
        public void bindGrid()
        {
            Bind();
        }
        #endregion

        #region Bind
        /// <summary>
        /// 
        /// </summary>
        private DataTable Bind()
        {
            int iRowCount = 0;
            DataTable dt;
            string sWhere = " 1=1 ";
            string sOrder = " order by Datatime Desc";

            #region 查询条件

            if (txtFromEmail.Text.ToString() != string.Empty)
            {
                sWhere += " and FromEmail like " + StringTool.SqlQ("%" + txtFromEmail.Text.ToString() + "%");
            }
            if (txtEmailTitle.Text.ToString() != string.Empty)
            {
                sWhere += " and EmailTitle like " + StringTool.SqlQ("%" + txtEmailTitle.Text.ToString() + "%");
            }

            if (ddlStatus.SelectedItem.Text.Trim() != "")
            {
                sWhere += " and statue= " + ddlStatus.SelectedItem.Value.ToString();
            }
          
            #endregion
            
        

            
            
            dt = EmialIssue.GetDataTable(sWhere, sOrder, this.CtrcpfMailMessage.PageSize, this.CtrcpfMailMessage.CurrentPage, ref iRowCount);
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
                Response.Redirect("frm_EmailIssueEdit.aspx?id=" + e.Item.Cells[1].Text.ToString());
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
                e.Item.Attributes.Add("ondblclick", "window.open('frm_EmailIssueEdit.aspx?id=" + sID.ToString() + "&randomid='+GetRandom(),'MainFrame','scrollbars=yes,resizable=yes')");
                
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
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i > 1 && i < 10)
                    {
                        int j = i - 1;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
        #endregion

      
    }
}
