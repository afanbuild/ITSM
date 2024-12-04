/****************************************************************************
 * 
 * description:供应商管理主表单
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-09-17
 * *************************************************************************/
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;

using Epower.ITSM.SqlDAL;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.Provide
{
    public partial class frmPro_ProvideManageMain : BasePage
    {
        #region IsSelect
        /// <summary>
        /// 
        /// </summary>
        protected bool IsSelect
        {
            get { if (Request["IsSelect"] != null) return true; else return false; }
        }
        #endregion 

     
       

   

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.PrivideManager;
            if (IsSelect)
                this.Master.IsCheckRight = false;
            else
            {
                this.Master.IsCheckRight = true;
                //dgPro_ProvideManage.Columns[6].Visible = this.Master.GetEditRight();

                #region  删除权限逻辑控制  yanghw  优先流程删除权限  然后再判断本操作项的权限
                //应用管理员删除权限                
                if (((RightEntity)((Hashtable)Session["UserAllRights"])[Constant.admindeleteflow]).CanDelete)
                {
                    dgPro_ProvideManage.Columns[6].Visible = true;
                }
                else
                {
                    RightEntity re = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.PrivideManager];
                    if (re != null)
                        dgPro_ProvideManage.Columns[6].Visible = re.CanModify;
                    else
                        dgPro_ProvideManage.Columns[6].Visible = false;

                }
                #endregion
            }


            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);
            this.Master.ShowQueryPageButton();
            this.Master.MainID = "1";

            if (IsSelect)  //如果为选择时
            {
                this.Master.ShowNewButton(false);
                this.Master.ShowDeleteButton(false);
                dgPro_ProvideManage.Columns[0].Visible = false;
            }
        }
        #endregion

        #region Master_Master_Button_New_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("frmPro_ProvideManageEdit.aspx");
        }
        #endregion

        #region Master_Master_Button_Query_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            LoadData();
        }
        #endregion

        #region Master_Master_Button_Delete_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            Pro_ProvideManageDP ee = new Pro_ProvideManageDP();
            int iValue = 0;
            string ValueName = string.Empty;
            foreach (DataGridItem itm in dgPro_ProvideManage.Items)
            {
                if (itm.ItemType == ListItemType.AlternatingItem ||
                    itm.ItemType == ListItemType.Item)
                {
                    string sID = itm.Cells[1].Text;
                    CheckBox chkdel = (CheckBox)itm.Cells[0].FindControl("chkDel");
                    if (chkdel.Checked)
                    {
                        if (Equ_DeskDP.getProvideName(sID.ToString()) != true)
                        {
                            ee.DeleteRecorded(long.Parse(sID));
                        }
                        else
                        {
                            if (iValue == 0)
                            {
                                ValueName = itm.Cells[6].Text.Trim();
                            }
                            else
                            {
                                ValueName ="," +itm.Cells[6].Text.Trim();
                            }
                            iValue++;
                        }
                    }
                }
            }
            LoadData();
            
            if (ValueName != "")
            {
                PageTool.MsgBox(this, "供应商名称[" + ValueName + "]已在资产中用到，不能删除！");                
            }
        }
        #endregion

        #region Page_Load
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            SetParentButtonEvent();
            cpServiceStaff.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(LoadData);
            if (!IsPostBack)
            {
                LoadData();
            }
        }
        #endregion

        #region LoadData
        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            DataTable dt;
            string sWhere = string.Empty;
            string sOrder = string.Empty;
            if (txtName.Text.Trim() != string.Empty)
            {
                sWhere += " And Name like" + StringTool.SqlQ("%" + txtName.Text.Trim() +"%");
            }
            if (txtCode.Text.Trim() != string.Empty)
            {
                sWhere += " And Code like" + StringTool.SqlQ("%" + txtCode.Text.Trim() + "%");
            }
            if (txtContract.Text.Trim() != string.Empty)
            {
                sWhere += " And Contract like" + StringTool.SqlQ("%" + txtContract.Text.Trim() + "%");
            }
            if (txtContractTel.Text.Trim() != string.Empty)
            {
                sWhere += " And ContractTel like" + StringTool.SqlQ("%" + txtContractTel.Text.Trim() + "%");
            }
            Pro_ProvideManageDP ee = new Pro_ProvideManageDP();
            int iRowCount = 0;
            dt = ee.GetDataTable(sWhere, this.cpServiceStaff.PageSize, this.cpServiceStaff.CurrentPage, ref iRowCount);
            dgPro_ProvideManage.DataSource = dt.DefaultView;
            dgPro_ProvideManage.DataBind();
            this.cpServiceStaff.RecordCount = iRowCount;
            this.cpServiceStaff.Bind();
        }
        #endregion

        #region  dgPro_ProvideManage_ItemCommand
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgPro_ProvideManage_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                if (!IsSelect)
                    Response.Redirect("frmPro_ProvideManageEdit.aspx?id=" + e.Item.Cells[1].Text.ToString());
                else 
                {
                    StringBuilder sbText = new StringBuilder();
                    sbText.Append("<script>");
                    sbText.Append("var arr = new Array();");
                    // ID
                    sbText.Append("arr[0] ='" + e.Item.Cells[1].Text + "';");
                    // 名称
                    sbText.Append("arr[1] ='" + e.Item.Cells[7].Text + "';");
                    sbText.Append("window.parent.returnValue = arr;");
                    // 关闭窗口
                    sbText.Append("top.close();");
                    sbText.Append("</script>");
                    // 向客户端发送
                    Page.RegisterStartupScript(DateTime.Now.ToString(), sbText.ToString());
                    Response.Write(sbText.ToString());
                }
            }
        }
        #endregion

        #region dgPro_ProvideManage_ItemDataBound
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgPro_ProvideManage_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Button lnkedit = (Button)e.Item.FindControl("lnkedit");
                if (IsSelect)
                {
                    lnkedit.Text = "选择";

                    string value1 = e.Item.Cells[1].Text.Trim().Replace("&nbsp;", "");               
                    string value2 = e.Item.Cells[7].Text.Trim().Replace("&nbsp;", "");
                    e.Item.Attributes.Add("ondblclick", "ServerOndblclick('" + value1 + "','" + value2 + "');"); 
                }
                else
                {
                    e.Item.Attributes.Add("ondblclick", "window.open('frmPro_ProvideManageEdit.aspx?id=" + e.Item.Cells[1].Text.ToString() + "','MainFrame','');");
                }

                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");

               
            }
        }
        #endregion 

        #region dgPro_ProvideManage_ItemCreated
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgPro_ProvideManage_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (IsSelect)
                    {
                        if (i > 1 && i < 6)
                        {
                            int j = i - 2;
                            e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                        }
                    }
                    else
                    {
                        if (i > 1 && i < 6)
                        {
                            int j = i - 1;
                            e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                        }
                    }
                }
            }
        }
        #endregion 
    }
}

