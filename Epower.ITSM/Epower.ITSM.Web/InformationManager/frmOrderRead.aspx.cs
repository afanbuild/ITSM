/****************************************************************************
 * 
 * description:阅读排名
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-10-22
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
using System.Text;

using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;

namespace Epower.ITSM.Web.InformationManager
{
    public partial class frmOrderRead : BasePage
    {
        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.TableVisible = false;
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
            ControlPage1.On_PostBack += new EventHandler(ControlPage1_On_PostBack);
            ControlPage1.DataGridToControl = dgInf_Information;

            dgInf_Information.PageSize = 100;
            if (!IsPostBack)
            {
                //设置显示
                PageDeal.SetLanguage(this.Controls[0]);

                SetHeaderText();
                
                DataTable dt;
                dt = LoadData();
                dgInf_Information.DataSource = dt.DefaultView;
                dgInf_Information.DataBind();
            }
        }
        #endregion

        #region 设置datagrid标头显示 余向前 2013-05-20
        /// <summary>
        /// 设置datagrid标头显示
        /// </summary>
        private void SetHeaderText()
        {
            dgInf_Information.Columns[2].HeaderText = PageDeal.GetLanguageValue("info_Title");
            dgInf_Information.Columns[3].HeaderText = PageDeal.GetLanguageValue("info_PKey");
            dgInf_Information.Columns[4].HeaderText = PageDeal.GetLanguageValue("info_TypeName");

        }
        #endregion

        #region LoadData
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sCalalogID"></param>
        /// <returns></returns>
        private DataTable LoadData()
        {
            DataTable dt;
            int iNum = 100;
            Inf_OrderDealDP ee = new Inf_OrderDealDP();
            dt = ee.GetReadOrderData(iNum); ;
            Session["Inf_InformationRead"] = dt;
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
            if (Session["Inf_InformationRead"] == null)
            {
                dt = LoadData();
            }
            else
            {
                dt = (DataTable)Session["Inf_InformationRead"];
            }
            dgInf_Information.DataSource = dt.DefaultView;
            dgInf_Information.DataBind();
        }
        #endregion

        #region ControlPage1_On_PostBack
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ControlPage1_On_PostBack(object sender, EventArgs e)
        {
            Bind();
        }
        #endregion
        #region dgInf_Information_ItemCreated
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgInf_Information_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                int j = 0;
                for (int i = 1; i < e.Item.Cells.Count; i++)
                {
                    j = i - 1;
                    if(i==5)
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",1);");
                    else
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                }
            }
        }
        #endregion 

        protected void dgInf_Information_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
            }
        }
    }
}
