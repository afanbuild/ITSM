using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.AppForms
{
    public partial class AddComputerJRJF : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           this.Master.TableVisible = false;
            HouseID.Value = Request["HouseID"].ToString();
            if (!IsPostBack)
            {
                bindGrid();
            }
            if (Session["showdisplay2"] != null)
            {
                ExecUserGrid.Columns[ExecUserGrid.Columns.Count - 1].Visible = Session["showdisplay2"].ToString().Trim() == string.Empty ? true : false;
            }
        }
         public void bindGrid()
        {
            //绑定公司内部人

            DataTable dt3 = Flow_QuestHouse.getExecUserIP(long.Parse(Request["HouseID"].ToString() == "" ? "0" : Request["HouseID"].ToString()));
            ExecUserGrid.DataSource = dt3;
            ExecUserGrid.DataBind();
        }

         protected void ExecUserGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
         {
             if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
             {
                 string strIP = e.Item.Cells[1].Text;
                 e.Item.Cells[2].Text = Flow_QuestHouse.getExecUser(long.Parse(HouseID.Value=="" ? "0" : HouseID.Value), strIP);
             }
         }

         protected void BtnHidden_Click(object sender, EventArgs e)
         {
             bindGrid();
         }

      
    }
}
