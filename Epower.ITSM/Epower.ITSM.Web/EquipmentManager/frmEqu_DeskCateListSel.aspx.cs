/*******************************************************************
 *
 * Description 资产目录选择列表
 * 
 * 
 * Create By  :ly
 * Create Date:2011年8月15日
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
using System.Text;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frmEqu_DeskCateListSel : BasePage
    {
        #region 属性
        /// <summary>
        /// 设备分类ID
        /// </summary>
        protected string CatalogID
        {
            get
            {
                if (Request["subjectid"] != null && Request["subjectid"] != "")
                    return Request["subjectid"].ToString();
                else return "-1";
            }
        }
        #endregion

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.CEqu_CateLists;
            //this.Master.IsCheckRight = true;
            this.dgEqu_CateLists.Columns[this.dgEqu_CateLists.Columns.Count - 1].Visible = this.Master.GetEditRight();
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);
            this.Master.Master_Button_GoHistory_Click += new Global_BtnClick(Master_Master_Button_GoHistory_Click);
            this.Master.ShowQueryButton(true);
            this.Master.ShowBackUrlButton(true);
            this.Master.MainID = "1";
        }
        #endregion

        #region Master_Master_Button_GoHistory_Click
        /// <summary>
        /// Master_Master_Button_GoHistory_Click
        /// </summary>
        private void Master_Master_Button_GoHistory_Click()
        {
            Response.Write("<script>window.close();</script>");
        }
        #endregion

        #region Master_Master_Button_New_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("frmEqu_DeskCateListEdit.aspx?subjectid=" + CatalogID);
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
            Equ_CateListsDP ee = new Equ_CateListsDP();
            foreach (DataGridItem itm in dgEqu_CateLists.Items)
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
            cpEqu_CateLists.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(BindData);
            if (!IsPostBack)
            {
                
                if (Request["subjectid"] != null)
                { 
                    //如果传过来的类别不为空，则类别只能为只读；否则可以选
                    ctrFlowEquCategory1.CatelogID = long.Parse(Request["subjectid"].ToString() == "" ? "0" : Request["subjectid"].ToString());
                    ctrFlowEquCategory1.ContralState = eOA_FlowControlState.eReadOnly;
                }

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
            string sWhere = " 1=1 And Deleted=0 ";
            string sOrder = " order by id";

            if (ctrFlowEquCategory1.CatelogID > 1)
            {
                sWhere += " and CatalogID = " + ctrFlowEquCategory1.CatelogID;
            }

            if (txtListName.Text.Trim() != string.Empty)
            {
                sWhere += " And ListName like " + StringTool.SqlQ("%" + txtListName.Text.Trim() + "%");
            }
            Equ_CateListsDP ee = new Equ_CateListsDP();
            int iRowCount = 0;
            dt = ee.GetDataTable(sWhere, sOrder, this.cpEqu_CateLists.PageSize, this.cpEqu_CateLists.CurrentPage, ref iRowCount);
            dgEqu_CateLists.DataSource = dt.DefaultView;
            dgEqu_CateLists.DataBind();
            this.cpEqu_CateLists.RecordCount = iRowCount;
            this.cpEqu_CateLists.Bind();
        }
        #endregion

        #region  dgEqu_CateLists_ItemCommand
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgEqu_CateLists_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Sel")
            {
                StringBuilder sText = new StringBuilder();
                sText.Append("<script>");
                sText.Append("var arr = new Array();");
                sText.Append("arr[0] = '" + e.Item.Cells[0].Text + "';");       //目录ID
                sText.Append("arr[1] = '" + e.Item.Cells[1].Text + "';");       //目录名称
                sText.Append("window.parent.returnValue = arr;");
                sText.Append("top.close();");
                sText.Append("</script>");

                Page.RegisterStartupScript(DateTime.Now.ToString(), sText.ToString());
                Response.Write(sText.ToString());
            }
        }
        #endregion

        #region dgEqu_CateLists_ItemCreated
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgEqu_CateLists_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                int j = 0;
                for (int i = 0; i < e.Item.Cells.Count - 1; i++)
                {
                    j = i - 1;
                    e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                }
            }
        }
        #endregion

        #region dgEqu_CateLists_ItemDataBound
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgEqu_CateLists_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                string sID = DataBinder.Eval(e.Item.DataItem, "ID").ToString();

                string strValue1 = e.Item.Cells[0].Text.Replace("&nbsp;", "");      //目录ID
                string strValue2 = e.Item.Cells[1].Text.Replace("&nbsp;","");       //目录名称
                e.Item.Attributes.Add("ondblclick", "ServerOndblclick('"+ strValue1 + "','"+  strValue2 + "');");
            }
        }
        #endregion 
    }
}
