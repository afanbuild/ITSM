/****************************************************************************
 * 
 * description:知识关联
 * 
 * 
 * 
 * Create by:zhumingchun
 * Create Date:2007-09-26
 * *************************************************************************/
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

using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;
using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.InformationManager
{
    /// <summary>
    /// 
    /// </summary>
    public partial class frmRelMain : BasePage
    {
        #region KBID
        /// <summary>
        /// 
        /// </summary>
        protected string KBID
        {
            get { if (Request["KBID"] != null) return Request["KBID"].ToString(); else return "0"; }
        }
        #endregion 

        #region CatalogID
        /// <summary>
        /// 
        /// </summary>
        protected string CatalogID
        {
            get { if (Request["subjectid"] != null) return Request["subjectid"].ToString(); else return "0"; }
        }
        #endregion 

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_GoHistory_Click += new Global_BtnClick(Master_Master_Button_GoHistory_Click);
            this.Master.ShowBackUrlButton(true);
        }
        #endregion

        #region Master_Master_Button_GoHistory_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_GoHistory_Click()
        {
            Response.Redirect("frmInf_InformationMain.aspx?subjectid=" + CatalogID);
        }
        #endregion

        #region 页面加载 Page_Load
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtonEvent();
            if (!IsPostBack)
            {
                InitPage();
                LoadData();
            }
        }
        #endregion 

        #region 页面数据初始化 InitPage
        /// <summary>
        /// 
        /// </summary>
        private void InitPage()
        {
            Inf_InformationDP ee = new Inf_InformationDP();
            string sWhere = " and ID=" + KBID;
            DataTable dt = ee.GetDataTable(sWhere,string.Empty,false);
            foreach (DataRow dr in dt.Rows)
            {
                lblEventTitle.Text = dr["Title"].ToString();
            }
        }
        #endregion 

        #region 取得详细资料GetDetailItem
        /// <summary>
        /// 取得详细资料
        /// </summary>
        /// <param name="isAll"></param>
        /// <returns></returns>
        private DataTable GetDetailItem()
        {
            DataTable dt = (DataTable)Session["RelItemData"];
            dt.Rows.Clear();
            int iCostID = 0;
            DataRow dr;
            foreach (DataGridItem row in dgPro_ProblemAnalyse.Controls[0].Controls)
            {
                iCostID++;
                if (row.ItemType == ListItemType.Item || row.ItemType == ListItemType.AlternatingItem)
                {
                    dr = dt.NewRow();
                    dr["KBID"] = row.Cells[0].Text.ToString();
                    dr["RelID"] = row.Cells[1].Text.ToString();
                    dr["Title"] = row.Cells[2].Text.ToString(); ;
                    dt.Rows.Add(dr);
                }
            }
            Session["RelItemData"] = dt;
            return dt;
        }
        #endregion

        #region 明细删除事件 dgPro_ProblemAnalyse_ItemCommand
        /// <summary>
        /// 费用明细新增，删除事件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgPro_ProblemAnalyse_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            DataTable dt = GetDetailItem();
            if (e.CommandName == "Delete")
            {
                dt.Rows.RemoveAt(e.Item.ItemIndex);
            }
            dgPro_ProblemAnalyse.DataSource = dt.DefaultView;
            dgPro_ProblemAnalyse.DataBind();

            btnSave_Click(null, null);
        }
        #endregion 

        #region 保存数据 btnSave_Click
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dt = GetDetailItem();
            Inf_RelDP.SaveItem(dt, decimal.Parse(KBID));

            string sRefID = string.Empty;
            CreateTotle(ref sRefID);
            this.hidCustArrIDold.Value = sRefID;
        }
        #endregion 

        #region 新增 btnAdd_Click
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (hidFlag.Value.ToUpper() == "OK")
            {
                LoadProblemData();

                btnSave_Click(null, null);
            }
        }
        #endregion 

        #region 加载数据 LoadData
        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadData()
        {
            Inf_RelDP ee = new Inf_RelDP();
            string sWhere = " and KBID=" + KBID;
            DataTable dt = ee.GetDataTable(sWhere,string.Empty);
            Session["RelItemData"] = dt;
            dgPro_ProblemAnalyse.DataSource = dt.DefaultView;
            dgPro_ProblemAnalyse.DataBind();

            string sRefID = string.Empty;
            CreateTotle(ref sRefID);
            this.hidCustArrIDold.Value = sRefID;
        }
        #endregion 

        #region 生成总的REFID
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private void CreateTotle(ref string sArrRefID)
        {
            foreach (DataGridItem row in dgPro_ProblemAnalyse.Items)
            {
                sArrRefID += row.Cells[1].Text.Trim() + ",";
            }
        }
        #endregion

        #region 加载问题数据，并整合原有数据 LoadProblemData
        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadProblemData()
        {
            string sArr = string.Empty;
            string sArrold = this.hidCustArrIDold.Value.Trim();
            string[] arr = this.hidCustArrID.Value.Trim().Split(',');
            for (int i = 0; i < arr.Length - 1; i++)
            {
                if (sArrold.IndexOf(arr[i] + ",") == -1)
                {
                    sArr += arr[i] + ",";
                    sArrold += arr[i] + ",";
                }
            }
            this.hidCustArrIDold.Value = sArrold;

            if (!string.IsNullOrEmpty(sArr))
            {
                DataTable dtProblem = Inf_RelDP.GetProblemAnalsys(sArr, KBID);

                DataTable dt = GetDetailItem();
                dt.Merge(dtProblem);
                dgPro_ProblemAnalyse.DataSource = dt.DefaultView;
                dgPro_ProblemAnalyse.DataBind();

                string sRefID = string.Empty;
                CreateTotle(ref sRefID);
                this.hidCustArrIDold.Value = sRefID;
            }
        }
        #endregion 

        #region dgPro_ProblemAnalyse_ItemCreated
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgPro_ProblemAnalyse_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i > 2 && i < 4)
                    {
                        int j = i - 3;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
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
        protected void dgPro_ProblemAnalyse_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
            }
        }
    }
}
