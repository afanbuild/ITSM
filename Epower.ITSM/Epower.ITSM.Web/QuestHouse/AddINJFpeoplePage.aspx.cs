using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.AppForms
{
    public partial class AddINJFpeoplePage : BasePage
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
                dgProblem.Columns[dgProblem.Columns.Count - 1].Visible = Session["showdisplay1"].ToString().Trim() == string.Empty ? true : false;
            }
        }

        public void bindGrid()
        {
            //绑定公司内部人员
            DataTable dt = Flow_QuestHouse.getUser(long.Parse(Request["HouseID"].ToString() == "" ? "0" : Request["HouseID"].ToString()));
            dgProblem.DataSource = dt;
            dgProblem.DataBind();
        }

        protected void btnHidd_Click(object sender, EventArgs e)
        {
            bindGrid();
        }
    }
}
