/*******************************************************************
 *
 * Description:服务级别
 * 
 * 
 * Create By  :zhumc
 * Create Date:2008年4月23日
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

namespace Epower.ITSM.Web.AppForms
{
    public partial class frmCst_ServiceLevelMain : BasePage
    {
        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.ServerLevel;
            this.Master.IsCheckRight = true;
            dgCst_ServiceLevel.Columns[5].Visible = this.Master.GetEditRight();
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
            Response.Redirect("frmCst_ServiceLevelEdit.aspx");
        }
        #endregion

        #region Master_Master_Button_Query_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
           LoadData();
        }
        #endregion

        #region Master_Master_Button_Delete_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            Cst_ServiceLevelDP ee = new Cst_ServiceLevelDP();
            bool isDeleteRecord = false;
            foreach (DataGridItem itm in dgCst_ServiceLevel.Items)
            {
                if (itm.ItemType == ListItemType.AlternatingItem ||
                    itm.ItemType == ListItemType.Item)
                {
                    string sID = itm.Cells[1].Text;
                    CheckBox chkdel = (CheckBox)itm.Cells[0].FindControl("chkDel");
                    if (chkdel.Checked)
                    {
                        ee.DeleteRecorded(long.Parse(sID));
                        isDeleteRecord = true;
                    }
                }
            }
            if (isDeleteRecord)
            {
                //强制相关缓存失效 
                HttpRuntime.Cache.Insert("CommCacheValidCstServiceLevel", false);
            }
   
            LoadData();
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
            cpServiceLevel.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(LoadData);
            if (!IsPostBack)
            {
                LoadData();
            }
        }
        #endregion

        #region LoadData
        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            int iRowCount = 0;
            DataTable dt;
            string sWhere = string.Empty;
            string sOrder = " order by Id ";
            if (txtLevelName.Text.Trim() != string.Empty)
            {
                sWhere += " And LevelName like " + StringTool.SqlQ("%" + txtLevelName.Text.Trim() + "%");
            }
            if (txtDefinition.Text.Trim() != string.Empty)
            {
                sWhere += " And Definition like " + StringTool.SqlQ("%" + txtDefinition.Text.Trim() + "%");
            }
            if (txtBaseLevel.Text.Trim() != string.Empty)
            {
                sWhere += " And BaseLevel like " + StringTool.SqlQ("%" + txtBaseLevel.Text.Trim() + "%");
            }
            if (txtNotInclude.Text.Trim() != string.Empty)
            {
                sWhere += " And NotInclude like " + StringTool.SqlQ("%" + txtNotInclude.Text.Trim() + "%");
            }
            if (txtAvailability.Text.Trim() != string.Empty)
            {
                sWhere += " And Availability like " + StringTool.SqlQ("%" + txtAvailability.Text.Trim() + "%");
            }
            if (rdbtIsAvail.SelectedValue.Trim() != string.Empty)
            {
                sWhere += " And IsAvail=" + rdbtIsAvail.SelectedValue.Trim();
            }
            Cst_ServiceLevelDP ee = new Cst_ServiceLevelDP();
            dt = ee.GetDataTable(sWhere, sOrder, this.cpServiceLevel.PageSize, this.cpServiceLevel.CurrentPage, ref iRowCount);
            dgCst_ServiceLevel.DataSource = dt.DefaultView;
            dgCst_ServiceLevel.DataBind();

            this.cpServiceLevel.RecordCount = iRowCount;
            this.cpServiceLevel.Bind();
        }
        #endregion     

        #region  dgCst_ServiceLevel_ItemCommand
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgCst_ServiceLevel_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                Response.Redirect("frmCst_ServiceLevelEdit.aspx?id=" + e.Item.Cells[1].Text.ToString());
            }
        }
        #endregion

        #region dgCst_ServiceLevel_ItemCreated
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgCst_ServiceLevel_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                int j = 0;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i > 1 && i < e.Item.Cells.Count - 1)
                    {
                        j = i - 1;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
        #endregion

        #region dgCst_ServiceLevel_ItemDataBound
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgCst_ServiceLevel_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.Item.Cells[7].Text == "0")
                {
                    e.Item.Cells[7].Text = "有效";
                }
                else if (e.Item.Cells[7].Text == "1")
                {
                    e.Item.Cells[7].Text = "无效";
                }


                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                e.Item.Attributes.Add("ondblclick", "window.open('../AppForms/frmCst_ServiceLevelEdit.aspx?id=" + e.Item.Cells[1].Text.ToString() + "&randomid='+GetRandom(),'MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");

                ((Label)e.Item.FindControl("Lb_ServiceNo")).Attributes.Add("onmouseover", "ShowDetailsInfo(this," + DataBinder.Eval(e.Item.DataItem, "ID").ToString() + ",400);");
            }
        }
        #endregion 
    }
}
