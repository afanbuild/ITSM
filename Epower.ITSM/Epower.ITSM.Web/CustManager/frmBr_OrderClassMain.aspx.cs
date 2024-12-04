/*******************************************************************
 *
 * Description
 * 
 * 排班表明细查询
 * Create By  :yxq
 * Create Date:2011年8月30日 星期二
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

namespace Epower.ITSM.Web.CustManager
{
    public partial class frmBr_OrderClassMain : BasePage
    {
        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.CBr_OrderClass;
            this.Master.IsCheckRight = true;
            this.dgBr_OrderClass.Columns[this.dgBr_OrderClass.Columns.Count - 1].Visible = this.Master.GetEditRight();
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);
            this.Master.Master_Button_GoHistory_Click += new Global_BtnClick(Master_Master_Button_GoHistory_Click);
            this.Master.ShowQueryPageButton();
            this.Master.ShowBackUrlButton(true);
            this.Master.MainID = "1";
        }

        void Master_Master_Button_GoHistory_Click()
        {
            Response.Redirect("frmBr_OrderClassView.aspx");
        }
        #endregion

        #region Master_Master_Button_New_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("frmBr_OrderClassEdit.aspx");
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
            Br_OrderClassDP ee = new Br_OrderClassDP();
            foreach (DataGridItem itm in dgBr_OrderClass.Items)
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
            cpBr_OrderClass.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(BindData);
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
            string sWhere = " 1=1 And nvl(Deleted,0)=0 ";
            string sOrder = " order by id";

            if (UserPickerMult1.UserID.Trim() != "")
            {
                sWhere += " And StaffID in (" + UserPickerMult1.UserID.Trim(',') + ")";
            }

            if (ctrDutyTime.dateTimeString != "")
            {
                sWhere += " And to_char(DutyTime,'yyyy-MM-dd') = " + StringTool.SqlQ(ctrDutyTime.dateTime.ToString("yyyy-MM-dd"));
            }
            Br_OrderClassDP ee = new Br_OrderClassDP();
            int iRowCount = 0;
            dt = ee.GetDataTable(sWhere, sOrder, this.cpBr_OrderClass.PageSize, this.cpBr_OrderClass.CurrentPage, ref iRowCount);
            dgBr_OrderClass.DataSource = dt.DefaultView;
            dgBr_OrderClass.DataBind();
            this.cpBr_OrderClass.RecordCount = iRowCount;
            this.cpBr_OrderClass.Bind();
        }
        #endregion

        #region  dgBr_OrderClass_ItemCommand
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgBr_OrderClass_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                Response.Redirect("frmBr_OrderClassEdit.aspx?id=" + e.Item.Cells[1].Text.ToString());
            }
        }
        #endregion

        #region dgBr_OrderClass_ItemCreated
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgBr_OrderClass_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                int j = 0;
                for (int i = 0; i < e.Item.Cells.Count - 1; i++)
                {
                    if (i > 1)
                    {
                        j = i - 1;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
        #endregion

        #region dgBr_OrderClass_ItemDataBound
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgBr_OrderClass_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                string sID = DataBinder.Eval(e.Item.DataItem, "ID").ToString();
                e.Item.Attributes.Add("ondblclick", "window.open('frmBr_OrderClassEdit.aspx?ID=" + sID + "&randomid='+GetRandom(),'MainFrame','scrollbars=yes,resizable=yes')");
               // HtmlInputHidden hd = (HtmlInputHidden)e.Item.FindControl("hidAnalyID");
                //
                //hd.ClientID
            }
        }
        #endregion


        protected string GetCustUrl(decimal dID)
        {
            string sUrl = "";
            sUrl = "javascript:window.open('" + Constant.ApplicationPath + "/CustManager/frmCst_ServiceStaffShow.aspx?id=" + dID.ToString() + "&IsSelect=true','','scrollbars=yes,resizable=yes,top=100,left=200,height=500,width=450');event.returnValue = false;";
            return sUrl;
        }
    }
}


