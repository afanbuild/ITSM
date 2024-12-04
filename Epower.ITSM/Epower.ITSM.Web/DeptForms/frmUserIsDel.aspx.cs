using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.DeptForms
{
    public partial class frmUserIsDel : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtonEvent();

            if (!IsPostBack)
            {
                BindData();
            }
        }

        /// <summary>
        /// 初始化界面按钮
        /// </summary>
        private void SetParentButtonEvent()
        {
            this.Master.ShowQueryButton(true);
            this.Master.ShowBackUrlButton(false);
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_GoHistory_Click += new Global_BtnClick(Master_Master_Button_GoHistory_Click);
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindData()
        {
            string strWhere = " and 1=1 ";

            if (txtLoginName.Text != string.Empty && txtLoginName.Text != "")
            {
                strWhere += " and LoginName like " + StringTool.SqlQ('%' + txtLoginName.Text.ToString() + "%");
            }

            DataTable dt = UserDP.GetUserIsDel(strWhere);
            dgUserIsDel.DataSource = dt.DefaultView;
            dgUserIsDel.DataBind();
        }

        /// <summary>
        /// 查询事件
        /// </summary>
        protected void Master_Master_Button_Query_Click()
        {
            BindData();
        }

        protected void dgUserIsDel_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Backuser")
            {
                string strUserID = e.Item.Cells[0].Text.ToString();
                UserDP.UpdUserByLoginName(strUserID);
            }
            BindData();
        }

        /// <summary>
        /// 返回事件
        /// </summary>
        protected void Master_Master_Button_GoHistory_Click()
        {
            Response.Write("<script>window.close();</script>");            
        }

        protected void dgUserIsDel_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#85B3FF'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");

                if (DataBinder.Eval(e.Item.DataItem, "deleted").ToString() == "0")
                {
                    ((Button)e.Item.FindControl("btnBackUser")).Enabled = false;
                }
                ((Button)e.Item.FindControl("btnBackUser")).Attributes.Add("onclick", "return confirm('您确认还原此用户吗?')");
            }
        }
    }
}
