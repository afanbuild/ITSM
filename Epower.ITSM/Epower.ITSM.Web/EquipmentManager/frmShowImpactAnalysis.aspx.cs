/****************************************************************************
 * 
 * description:资产关联展示
 * 
 * 
 * 
 * Create by:
 * Create Date:2010-06-28
 * *************************************************************************/
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using System.Drawing;

using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using EpowerGlobal;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frmShowImpactAnalysis : BasePage
    {
        /// <summary>
        /// 资产ID
        /// </summary>
        protected string EquID
        {
            get
            {
                if (ViewState["EquID"] != null)
                {
                    return ViewState["EquID"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState["EquID"] = value;
            }

        }

        /// <summary>
        /// 关联类别
        /// </summary>
        protected string RelType
        {
            get
            {
                if (ViewState["RelType"] != null)
                {
                    return ViewState["RelType"].ToString();
                }
                else
                {
                    return "1";
                }
            }
            set
            {
                ViewState["RelType"] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            EquID = Request["EquID"].ToString();
            RelType = Request["Type"].ToString();
            switch (RelType)
            {
                case "4":  //影响资产
                    //BindEquRel();
                    BindEquRel2();
                    Table4.Visible = true;
                    break;
                case "5":  //资产关联于
                    BindEquRelIn();
                    Table5.Visible = true;
                    break;
            }
        }

        #region 关联资产描述
        /// <summary>
        /// 关联资产描述
        /// </summary>
        private void BindEquRel2()
        {
            Equ_RelDP ee = new Equ_RelDP();
            System.Text.StringBuilder sbText = new System.Text.StringBuilder();
            string sList = string.Empty;
            string strTemp = string.Empty;
            int iLev = 0;         //标记影响资产的层数
            ee.GetImpactAnalysisChart(ref sbText, EquID, RelType, ref sList, ref strTemp, ref iLev);
            ImplyDiv.InnerHtml = sbText.ToString();
        }
        #endregion

        #region 关联资产
        /// <summary>
        /// 加载数据
        /// </summary>
        private void BindEquRel()
        {
            Equ_RelDP ee = new Equ_RelDP();
            DataTable dtRet = ee.GetDataTable(" and 1<>1", string.Empty);
            string sList = string.Empty;
            string strTemp = string.Empty;
            DataTable dt = ee.GetImpactAnalysis(ref dtRet, EquID, RelType, ref sList, ref strTemp);
            dgPro_ProblemAnalyse.DataSource = dt.DefaultView;
            dgPro_ProblemAnalyse.DataBind();
        }
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

        protected void dgPro_ProblemAnalyse_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
            }
        }
        #endregion

        #region 关联资产于
        /// <summary>
        /// 加载数据
        /// </summary>
        private void BindEquRelIn()
        {
            Equ_RelDP ee = new Equ_RelDP();
            DataTable dtRet = ee.GetDataTable(" and 1<>1", string.Empty);
            string sList = string.Empty;
            string strTemp = string.Empty;
            DataTable dt = ee.GetImpactAnalysis(ref dtRet, EquID, RelType, ref sList, ref strTemp);
            dg_EquRelIn.DataSource = dt.DefaultView;
            dg_EquRelIn.DataBind();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dg_EquRelIn_ItemCreated(object sender, DataGridItemEventArgs e)
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

        protected void dg_EquRelIn_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
            }
        }
        #endregion

    }
}
