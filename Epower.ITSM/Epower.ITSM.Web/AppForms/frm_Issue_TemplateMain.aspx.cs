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

using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.AppForms
{
    public partial class frm_Issue_TemplateMain : BasePage
    {
        /// <summary>
        /// 设置父窗体按钮事件
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.IssueShortCutReqTemplate;
            this.Master.IsCheckRight = true;
            dgECustomer.Columns[4].Visible = this.Master.GetEditRight();
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.ShowQueryPageButton();
            //this.Master.ShowQueryButton(false);   //这里不需要显示查询按钮
            this.Master.MainID = "1";
        }

        void Master_Master_Button_Query_Click()
        {
            LoadData();
        }



        /// <summary>
        /// 新增事情
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("frm_Issue_Template.aspx");
        }


        /// <summary>
        /// 删除事件
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            foreach (DataGridItem itm in dgECustomer.Items)
            {
                if (itm.ItemType == ListItemType.AlternatingItem ||
                    itm.ItemType == ListItemType.Item)
                {
                    string sID = itm.Cells[1].Text;
                    CheckBox chkdel = (CheckBox)itm.Cells[0].FindControl("chkDel");
                    try
                    {
                        labMsg.Visible = false;
                        if (chkdel.Checked)
                        {
                            EA_ShortCutTemplateDP ee = new EA_ShortCutTemplateDP();
                            ee.DeleteRecorded(long.Parse(sID));

                        }
                    }
                    catch (Exception ee)
                    {
                        labMsg.Visible = true;
                        labMsg.Text = ee.Message.ToString();
                    }
                }
            }
            LoadData();
        }

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            SetParentButtonEvent();
            cpTemplate.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(LoadData);
            if (!IsPostBack)
            {
                //加载数据
                LoadData();
            }
        }



        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            string templatename = this.txtTemplateName.Text.Trim();
            string strWhere = " AppID = 1026 ";
            if (templatename != "")
            {
                strWhere += " and TemplateName like " + StringTool.SqlQ("%" + templatename + "%");
            }
           
            int iRowCount = 0;
            EA_ShortCutTemplateDP ee = new EA_ShortCutTemplateDP();            
            DataTable dt = ee.GetTemplateData(strWhere, this.cpTemplate.PageSize, this.cpTemplate.CurrentPage, ref iRowCount);
            DataView dv = new DataView(dt);
            dgECustomer.DataSource = dv;
            dgECustomer.DataBind();
            this.cpTemplate.RecordCount = iRowCount;
            this.cpTemplate.Bind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ControlPage1_On_PostBack(object sender, EventArgs e)
        {
            LoadData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgECustomer_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                Response.Redirect("frm_Issue_Template.aspx?id=" + e.Item.Cells[1].Text.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgECustomer_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i > 1 && i < 4)
                    {
                        int j = i - 1;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }

        protected void dgECustomer_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string Owner = e.Item.Cells[2].Text;
                if (Owner == "0")
                {
                    e.Item.Cells[2].Text = "公共";
                }
                else if (Owner == "3")
                {
                    e.Item.Cells[2].Text = "自助服务";
                }
                //else if (Owner == "5")
                //{
                //    e.Item.Cells[2].Text = "服务请求";
                //}
                else
                {
                    e.Item.Cells[2].Text = "公共";
                }

                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                e.Item.Attributes.Add("ondblclick", "window.open('../AppForms/frm_Issue_Template.aspx?id=" + e.Item.Cells[1].Text.ToString() + "&randomid='+GetRandom(),'MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
            }
        }


    }
}
