using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Epower.ITSM.SqlDAL;
using System.Data;
using Epower.DevBase.BaseTools;
namespace Epower.ITSM.Web.AppForms
{
    public partial class EiitcomeINJFpeople : BasePage
    {
        private static string chk = "";  //判断是否保存

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.TableVisible = false;
            if (!IsPostBack)
            {
                if (Request["HouseID"] != null)
                {
                    bindGrid();
                }
            }

        }
        #region 获得传过来的参数用来判断父窗体的控件id的名称的相同部分
        /// <summary>
        /// 获得传过来的参数用来判断父窗体的控件id的名称的相同部分
        /// </summary>
        public string Opener_ClientId
        {

            get
            {
                return (Request["Opener_ClientId"] == null) ? "" : Request["Opener_ClientId"].ToString().Replace("hidClientId_ForOpenerPage", "");
            }
        }
        #endregion

        public void bindGrid()
        {
            chk = Request["chk"].ToString();

            DataTable dt2 = Flow_QuestHouse.getCompurUser(long.Parse(Request["HouseID"].ToString() == "" ? "0" : Request["HouseID"].ToString()));
            comperGrid.DataSource = dt2;
            comperGrid.DataBind();
        }
      

        protected void comperGrid_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Del")
            {
                deleteds();
            }
        }
        public void deleteds()
        {
            foreach(DataGridItem items in comperGrid.Items)
            {
                if(((CheckBox)items.FindControl("chkDel")).Checked==true)
                {
                    Flow_QuestHouse.DLTcompurUser(long.Parse(items.Cells[5].Text));
                }

            }
             bindGrid();
        }

        protected void ButSave_Click(object sender, EventArgs e)
        {
            if (UserName.Text.Trim() != "")
            {
                if (chk == "true")
                {
                    Flow_QuestHouse.AddCompurUser(long.Parse(Request["HouseID"].ToString() == "" ? "0" : Request["HouseID"].ToString()), UserName.Text.Trim(), CardNO.Text.Trim(), phone.Text.Trim(), compurName.Text.Trim(), long.Parse(Request["OpObjId"].ToString()));
                }
                else
                {
                    Flow_QuestHouse.AddCompurUser(long.Parse(Request["HouseID"].ToString() == "" ? "0" : Request["HouseID"].ToString()), UserName.Text.Trim(), CardNO.Text.Trim(), phone.Text.Trim(), compurName.Text.Trim(), 0);
                }

                UserName.Text = "";
                CardNO.Text = "";
                phone.Text = "";
                compurName.Text = "";
                PageTool.MsgBox(this.Page, "添加成功!");
                bindGrid();
            }
            else
            {
                PageTool.MsgBox(this.Page,"请输入姓名!");
            }
        }

 
    

   
    }
}
