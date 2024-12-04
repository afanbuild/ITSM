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

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frmEqu_MyDesk : BasePage
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
                BindData();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindData()
        {
            string sSql = "select a.* from equ_desk a left join br_ecustomer b on a.costom = b.id where a.deleted = 0 and b.deleted = 0 and b.userid = " + Session["UserID"].ToString();
            
            DataTable dt = Epower.ITSM.SqlDAL.CommonDP.ExcuteSqlTable(sSql);

            dgUserInfo.Columns[1].HeaderText = PageDeal.GetLanguageValue("LitEquDeskName");
            dgUserInfo.Columns[2].HeaderText = PageDeal.GetLanguageValue("LitEquDeskType");
            dgUserInfo.Columns[3].HeaderText = PageDeal.GetLanguageValue("LitEquDeskSerialNumber");

            dgUserInfo.Columns[6].HeaderText = PageDeal.GetLanguageValue("LitEquDeskCode");
            dgUserInfo.Columns[4].HeaderText = PageDeal.GetLanguageValue("LitEquDeskBreed");
            dgUserInfo.Columns[5].HeaderText = PageDeal.GetLanguageValue("LitEquDeskModel");


            this.dgUserInfo.DataSource = dt.DefaultView;
            this.dgUserInfo.DataBind();


        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgUserInfo_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "look")
            {
                Response.Write(@"<script>window.open(""frmEqu_DeskEdit.aspx?newWin=true&id=" + e.Item.Cells[0].Text.ToString() + @"&subjectid=1&FlowID=-1"",""_blank"",""scrollbars=yes,status=yes ,resizable=yes,width=800,height=600"");</script>");
                
            }
        }

        protected void dgUserInfo_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    // (DataView)e.Item.NamingContainer;
                    if (i > 0 && i < 7)
                    {
                        int j = i - 1;   //注意,因为前面有一个不可见的列
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
    }
}
