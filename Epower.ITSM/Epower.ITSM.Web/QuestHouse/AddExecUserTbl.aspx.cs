using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.AppForms
{
    public partial class AddExecUserTbl : BasePage
    {
        private static string chk = "";  //判断是否保存

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.TableVisible = false;
            if (!IsPostBack)
            {
                bindGrid();
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

        protected void ButSave_Click(object sender, EventArgs e)
        {
            if (IPurl.Text.Trim() != "" && userNO.Text.Trim() != "")
            {
                try
                {
                    if (chk == "true")
                    {
                        Flow_QuestHouse.AddExecUserTbl(long.Parse(Request["HouseID"].ToString() == "" ? "0" : Request["HouseID"].ToString()), IPurl.Text.Trim(), userNO.Text.Trim(), long.Parse(Request["OpObjId"].ToString()));
                    }
                    else
                    {
                        Flow_QuestHouse.AddExecUserTbl(long.Parse(Request["HouseID"].ToString() == "" ? "0" : Request["HouseID"].ToString()), IPurl.Text.Trim(), userNO.Text.Trim(), 0);
                    }

                    PageTool.MsgBox(this.Page, "保存成功");
                    bindGrid();
                    IPurl.Text = "";
                    userNO.Text = "";
                }
                catch
                {
                    PageTool.MsgBox(this.Page, "保存失败");
                }

            }
            else
            {
                PageTool.MsgBox(this.Page, "ip地址和用户号都不能为空");
            }
        }

        public void bindGrid()
        {//绑定ip
            chk = Request["chk"].ToString();

            DataTable dt3 = Flow_QuestHouse.getExecUserIPBill(long.Parse(Request["HouseID"].ToString() == "" ? "0" : Request["HouseID"].ToString()));
            ExecUserGrid.DataSource = dt3;
            ExecUserGrid.DataBind();
        }

        protected void dgProblem_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Del")
            {
                deleteds();
            }
        }

        public void deleteds()
        {
            try
            {
                foreach (DataGridItem items in ExecUserGrid.Items)
                {
                    if (((CheckBox)items.FindControl("chkDel")).Checked == true)
                    {
                        Flow_QuestHouse.DLTExecUserTbl(long.Parse(items.Cells[3].Text));
                    }

                }
                bindGrid();
            }
            catch (Exception)
            {
                PageTool.MsgBox(this.Page, "删除失败");
            }
        }
    }

     
}
