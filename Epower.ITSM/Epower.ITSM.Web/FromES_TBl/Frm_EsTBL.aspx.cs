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
using Epower.ITSM.Base;

namespace Epower.ITSM.Web.FromES_TBl
{
    public partial class Frm_EsTBL : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Request["ID"] != null)
                {
                    DataTable dt= ES_TBL.select(long.Parse(Request["ID"].ToString()));
                    if (dt.Rows.Count > 0)
                    {
                        txt_TblName.Value = dt.Rows[0]["tbl_Name"].ToString();                       
                    }
                
                    
                    txt_TblName.ContralState = eOA_FlowControlState.eReadOnly;
                    btn_Save.Visible = false;
                }

            }

        }

        protected void btn_Save_Click(object sender, EventArgs e)
        {
            bool isTrue=false;//判断是否重复的标志
            if (txt_TblName.Value != "")
            {
                //保存
                if (ES_TBL.Save(txt_TblName.Value.Trim(), ref isTrue))
                {
                    PageTool.MsgBox(this.Page, "保存成功");
                    btn_Save.Visible = false;

                }
                else
                {
                    if (isTrue == true)
                    {
                        PageTool.MsgBox(this.Page, "配置名称已经存在，请修改配置名称后再保存！");
                    }
                    else
                    {
                        PageTool.MsgBox(this.Page, "保存失败！");
                    }
                }
            }
            else
            {
                PageTool.MsgBox(this.Page, "配置名称不能为空！");
            }
        }

        protected void Btn_return_Click(object sender, EventArgs e)
        {
            Response.Redirect("Frm_EsTBLlist.aspx");
        }
    }
}
