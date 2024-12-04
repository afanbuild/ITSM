using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.AppForms
{
    public partial class AddExecUserTblPage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
             this.Master.TableVisible = false;
            HouseID.Value = Request["HouseID"].ToString();
            if (!IsPostBack)
            {
                bindGrid();
            }
            if (Session["showdisplay1"] != null)
            {
                comperGrid.Columns[comperGrid.Columns.Count - 1].Visible = Session["showdisplay1"].ToString().Trim() == string.Empty ? true : false;
            }
        }
        
        public void bindGrid()
        {
            //绑定公司内部人

            DataTable dt2 = Flow_QuestHouse.getCompurUser(long.Parse(Request["HouseID"].ToString() == "" ? "0" : Request["HouseID"].ToString()));
            comperGrid.DataSource = dt2;
            comperGrid.DataBind();
        }

        protected void hiddButton_Click(object sender, EventArgs e)
        {
            bindGrid();
        }

         
    }
}
