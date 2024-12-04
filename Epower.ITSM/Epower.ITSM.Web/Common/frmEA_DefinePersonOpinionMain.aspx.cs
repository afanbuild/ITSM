/*******************************************************************
 *
 * Description
 * 
 * 
 * Create By  :zhumc
 * Create Date:2010年1月7日
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

namespace Epower.ITSM.Web.Common
{
    public partial class frmEA_DefinePersonOpinionMain : BasePage
    {
        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);
            this.Master.Master_Button_Save_Click += new Global_BtnClick(Master_Master_Button_Save_Click);
            this.Master.Master_Button_SaveFinish_Click += new Global_BtnClick(Master_Master_Button_SaveFinish_Click);
            this.Master.Master_Button_GoHistory_Click += new Global_BtnClick(Master_Master_Button_GoHistory_Click);
            this.Master.ShowEditPageButton();
            this.Master.ShowNewButton(false);
            this.Master.MainID = "1";
            if (Request["Type"] == null)
            {
                this.Master.ShowBackUrlButton(false);
            }
        }
        #endregion

        #region Master_Master_Button_GoHistory_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_GoHistory_Click()
        {
            Response.Write("<script>top.close();</script>");
        }
        #endregion

        #region Master_Master_Button_SaveFinish_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_SaveFinish_Click()
        {
            if (Request["Type"] == null)
            {
                this.Master.ShowBackUrlButton(false);
            }
            this.Master.ShowNewButton(false);
        }
        #endregion

        #region Master_Master_Button_Save_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Save_Click()
        {
            EA_DefinePersonOpinionDP ee = new EA_DefinePersonOpinionDP();
            ee.Name = CtrFlowName.Value.Trim();
            ee.UserID = long.Parse(Session["UserID"].ToString());
            ee.UserName = Session["PersonName"].ToString();
            ee.CreateTime = DateTime.Now;
            ee.InsertRecorded(ee);
            DataTable dt = LoadData();
            dgEA_DefinePersonOpinion.DataSource = dt.DefaultView;
            dgEA_DefinePersonOpinion.DataBind();
        }
        #endregion

        #region Master_Master_Button_Delete_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            EA_DefinePersonOpinionDP ee = new EA_DefinePersonOpinionDP();
            foreach (DataGridItem itm in dgEA_DefinePersonOpinion.Items)
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
            dgEA_DefinePersonOpinion.DataSource = dt.DefaultView;
            dgEA_DefinePersonOpinion.DataBind();
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
            if (!IsPostBack)
            {
                DataTable dt = LoadData();
                dgEA_DefinePersonOpinion.DataSource = dt.DefaultView;
                dgEA_DefinePersonOpinion.DataBind();
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
            sWhere += " And UserID=" + Session["UserID"].ToString();
            EA_DefinePersonOpinionDP ee = new EA_DefinePersonOpinionDP();
            dt = ee.GetDataTable(sWhere, sOrder); ;
            Session["EA_DefinePersonOpinion"] = dt;
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
            if (Session["EA_DefinePersonOpinion"] == null)
            {
                dt = LoadData();
            }
            else
            {
                dt = (DataTable)Session["EA_DefinePersonOpinion"];
            }
            dgEA_DefinePersonOpinion.DataSource = dt.DefaultView;
            dgEA_DefinePersonOpinion.DataBind();
        }
        #endregion

        #region dgEA_DefinePersonOpinion_ItemCreated
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgEA_DefinePersonOpinion_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;

                int j = 0, intColumnType = 0;    // intColumnType, 0 字符串, 1 数字, 2 日期
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i > 1 && i < e.Item.Cells.Count)
                    {
                        j = i - 1;

                        String strColumnName = dg.Columns[i].HeaderText;
                        if (strColumnName.Equals("创建时间"))
                        {
                            intColumnType = 2;
                        }
                        
                        String strSortTableFunc = String.Format("sortTable('{0}', {1}, {2});",
                            dg.ClientID, j, intColumnType);

                        e.Item.Cells[i].Attributes.Add("onclick", strSortTableFunc);
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgEA_DefinePersonOpinion_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //e.Item.Attributes.Add("ondblclick", "window.open('frmEa_DefineMainPageEdit.aspx?id=" + e.Item.Cells[1].Text + "&randomid='+GetRandom(),'MainFrame','scrollbars=yes,resizable=yes')");
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
            }
        }
    }
}