/***************************************************************
 *   创建人：yanghw
 * 创建时间：2011-08-01
 *     说明：自定义配置项查询
 ***************************************************************/

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Epower.ITSM.SqlDAL.ES_TBLCS;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.FromES_TBl
{
    public partial class Frm_EsTBLlist : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {   
            this.Master.ShowNewButton(true);
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            if (!IsPostBack)
            {                
                BindGrid();            
            }
        }

        #region  绑定dataGrid
        /// <summary>
        /// 绑定dataGrid 
        /// </summary>
        public void BindGrid()
        {
            DataTable dt = ES_TBL.select();
            dgEs_TBL.DataSource = dt;
            dgEs_TBL.DataBind();
        }
        #endregion 

        protected void dgEs_TBL_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "look")
            {  //点击查看跳转界面
                Response.Redirect("Frm_EsTBL.aspx?ID=" + e.Item.Cells[0].Text);
            }
        }

        //新增
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("Frm_EsTBL.aspx");
        }
    }
}
