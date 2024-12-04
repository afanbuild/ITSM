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
    public partial class Frm_EsFilesEdit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                //绑定DropdownList 获得初值
                bindDropdownList();

                if (Request["ID"] != null)
                {
                    DataTable dt = ES_TBLFileds.select(long.Parse(Request["ID"].ToString()));
                    if (dt.Rows.Count > 0)
                    {
                        this.Master.MainID = dt.Rows[0]["ID"].ToString();
                        txt_TblFeildName.Value = dt.Rows[0]["filesName"].ToString();
                        Dop_tblName.SelectedIndex = Dop_tblName.Items.IndexOf(Dop_tblName.Items.FindByValue(dt.Rows[0]["tbl_id"].ToString()));
                        lab_tblName.Text = Dop_tblName.SelectedItem.Text;
                    }
                    Dop_tblName.Visible = false;
                    lab_tblName.Visible = true;                  
                }

            }

        }

        #region  绑定 DropDownList
        /// <summary>
        /// 绑定 DropDownList 
        /// </summary>
        public void bindDropdownList()
        {
            DataTable dt=ES_TBL.select();
            Dop_tblName.DataSource = dt;
            Dop_tblName.DataTextField = "tbl_Name";
            Dop_tblName.DataValueField = "ID";
            Dop_tblName.DataBind();
        }
        #endregion 

        protected void btn_Save_Click(object sender, EventArgs e)
        {           
            
            if (txt_TblFeildName.Value != "")
            {
                ES_TBLFileds TBLFiled = new ES_TBLFileds();
                TBLFiled.FilesName = txt_TblFeildName.Value.Trim();
                TBLFiled.TBL_ID = long.Parse(Dop_tblName.SelectedItem.Value);

                if (this.Master.MainID == null || this.Master.MainID == "")
                {
                    #region 插入保存                    
                    if (TBLFiled.InsertSave(ref TBLFiled))
                    {
                        PageTool.MsgBox(this.Page, "保存成功");
                        //保存不能修改配置项
                        lab_tblName.Text = Dop_tblName.SelectedItem.Text;
                        Dop_tblName.Visible = false; 
                        lab_tblName.Visible = true;
                        this.Master.MainID = TBLFiled.ID.ToString();
                    }
                    else
                    {
                        PageTool.MsgBox(this.Page, "保存失败！");
                    }
                    #endregion 
                }
                else
                {
                    //修改
                    TBLFiled.ID = long.Parse(this.Master.MainID.ToString());
                    if (TBLFiled.UpdateSave(ref TBLFiled))
                    {
                        PageTool.MsgBox(this.Page, "保存成功");
                        //保存不能修改配置项
                        lab_tblName.Text = Dop_tblName.SelectedItem.Text;
                        Dop_tblName.Visible = false;
                        lab_tblName.Visible = true;
                        this.Master.MainID = TBLFiled.ID.ToString();
                    }
                    else
                    {
                        PageTool.MsgBox(this.Page, "保存失败！");
                    }

                }
            }
            else
            {
                PageTool.MsgBox(this.Page, "配置项名称不能为空！");
            }
        }

        protected void Btn_return_Click(object sender, EventArgs e)
        {
            Response.Redirect("Frm_EsTBLFileslist.aspx");
        }
    }
}
