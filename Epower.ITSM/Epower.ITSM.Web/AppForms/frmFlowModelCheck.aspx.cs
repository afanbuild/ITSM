/****************************************************************************
 * 
 * description:流程模型检查
 * 
 * 
 * 
 * Create by:
 * Create Date:2010-07-16
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

using EpowerCom;

namespace Epower.ITSM.Web.AppForms
{
    /// <summary>
    /// 
    /// </summary>
    public partial class frmFlowModelCheck : BasePage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.TableVisible = false;
            if (!IsPostBack)
            {
                DataTable dt = (new Message()).CheckAllFlowModel();

                dgFlowModelCheck.DataSource = dt.DefaultView;
                dgFlowModelCheck.DataBind();

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgFlowModelCheck_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    int j = i;
                    e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgFlowModelCheck_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.Item.Cells[3].Text.Trim() == "" || e.Item.Cells[3].Text.Trim() == "&nbsp;")
                {
                    e.Item.Cells[3].Text = "正常";
                }
                else
                {
                    e.Item.Cells[3].Text = "不正常";
                }
            }
        }
    }
}
