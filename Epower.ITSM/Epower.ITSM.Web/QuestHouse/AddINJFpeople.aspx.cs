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
    public partial class AddINJFpeople : BasePage
    {
        private static string chk = "";  //判断是否保存

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.TableVisible = false;
            if (!IsPostBack)
            {
                bindGrid();
            }           
            if (HidPeople.Value.Trim() != "")
            {
                Lb_people.Text = HidPeople.Value;
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
            //绑定公司内部人员
            chk = Request["chk"].ToString();

            DataTable dt = Flow_QuestHouse.getUser(long.Parse(Request["HouseID"].ToString() == "" ? "0" : Request["HouseID"].ToString()));
            dgProblem.DataSource = dt;
            dgProblem.DataBind();
           

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
                foreach (DataGridItem items in dgProblem.Items)
                {
                    if (((CheckBox)items.FindControl("chkDel")).Checked == true)
                    {
                        Flow_QuestHouse.DLTINJFpeople(long.Parse(items.Cells[4].Text));
                    }

                }
                bindGrid();
            }
            catch (Exception)
            {
                PageTool.MsgBox(this.Page, "删除失败");
            }
        }

        protected void btn_server_Click(object sender, EventArgs e)
        {
            string rtnValues = UserValues.Value;
            if (rtnValues != "")
            {
                string[] listvalue = rtnValues.Split('|');
                foreach (string str in listvalue)
                {
                    string UserId = str.Split('@')[0].ToString();
                    string UserName = str.Split('@')[1].ToString();
                    string strdeptUser = Flow_QuestHouse.getExecDept(long.Parse(UserId));
                    string strDept = string.Empty;
                    string strUset = string.Empty;
                    if (strdeptUser != "")
                    {
                        strDept = strdeptUser.Split('|')[0].ToString();
                        strUset = strdeptUser.Split('|')[1].ToString();
                    }
                    if (chk == "true")
                    {
                        Flow_QuestHouse.AddINJFpeoples(long.Parse(Request["HouseID"].ToString() == "" ? "0" : Request["HouseID"].ToString()), UserName, strUset, strDept, long.Parse(Request["OpObjId"].ToString()));
                    }
                    else
                    {
                        Flow_QuestHouse.AddINJFpeoples(long.Parse(Request["HouseID"].ToString() == "" ? "0" : Request["HouseID"].ToString()), UserName, strUset, strDept, 0);
                    }

                }
                PageTool.MsgBox(this.Page, "保存成功");
                bindGrid();

            }
            else
            {
                PageTool.MsgBox(this.Page, "请选择人员");
            }
        }
    }
}
