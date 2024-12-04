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
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.ITSM.SqlDAL;
using System.Xml;
using System.Text;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frmEqu_DeskMainImply : BasePage
    {
        public string Opener_ClientId
        {

            get
            {
                return (Request["Opener_ClientId"] == null) ? "" : Request["Opener_ClientId"].ToString().Replace("hidClientId_ForOpenerPage", "");
            }
        }
        #region 属性
        /// <summary>
        /// 设备分类ID
        /// </summary>
        protected string EquIDs
        {
            get
            {
                if (Request["EquIDs"] != null && Request["EquIDs"] != "")
                    return Request["EquIDs"].ToString();
                else return "-1";
            }
        }
        #endregion

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.EquManager;

            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            //this.Master.ShowQueryPageButton();
            this.Master.ShowQueryButton(true);
            this.Master.ShowDeleteButton(false);
            this.Master.ShowExportExcelButton(false);
            this.Master.MainID = "1";

        }
        #endregion


        #region Master_Master_Button_Query_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            Bind();
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
            cpfECustomerInfo.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(bindGrid);
            if (!IsPostBack)
            {
                //设置显示
                PageDeal.SetLanguage(this.Controls[0]);

                dgEqu_Desk.Columns[1].HeaderText = PageDeal.GetLanguageValue("LitEquDeskName");
                dgEqu_Desk.Columns[2].HeaderText = PageDeal.GetLanguageValue("LitEquDeskCode");

                Bind();

            }
        }
        #endregion

        #region 翻页绑定dagagrid
        /// <summary>
        /// 
        /// </summary>
        public void bindGrid()
        {
            Bind();
        }
        #endregion

        #region Bind
        /// <summary>
        /// 
        /// </summary>
        private void Bind()
        {
            int iRowCount = 0;

            DataTable dt;
            string sWhere = " 1=1 ";
            string sOrder = " order by ID Desc";
            Hashtable ht = new Hashtable();

            Equ_DeskDP ee = new Equ_DeskDP();

            if (this.EquIDs != "-1")
            {
                //除去当前资产串
                sWhere += " AND id not in (" + this.EquIDs.ToString() + ")";
            }

            if (txtName.Text.Trim() != string.Empty)
            {
                sWhere += " And Name like " + StringTool.SqlQ("%" + txtName.Text.Trim() + "%");
            }

            if (txtCode.Text.Trim() != string.Empty)
            {
                sWhere += " And Code like " + StringTool.SqlQ("%" + txtCode.Text.Trim() + "%");
            }           

            dt = ee.GetDataTableSpec(sWhere, sOrder, ht, this.cpfECustomerInfo.PageSize, this.cpfECustomerInfo.CurrentPage, ref iRowCount);
            
            dgEqu_Desk.DataSource = dt;
            dgEqu_Desk.DataBind();
            this.cpfECustomerInfo.RecordCount = iRowCount;
            this.cpfECustomerInfo.Bind();
        }
        #endregion

        #region ControlPage1_On_PostBack
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ControlPage1_On_PostBack(object sender, EventArgs e)
        {
            Bind();
        }
        #endregion

        #region  dgEqu_Desk_ItemCommand
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgEqu_Desk_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Sel")
            {
                string id = e.Item.Cells[0].Text.Replace("&nbsp;", "");
                string sWhere = " And id = " + id;

                Equ_DeskDP ec = new Equ_DeskDP();
                DataTable dt = ec.GetDataTable(sWhere, "");
                Json json = new Json(dt);

                string jsonstr = "{record:" + json.ToJson() + "}";

                StringBuilder sbText = new StringBuilder();
                sbText.Append("<script>");
                sbText.Append("var arr = " + jsonstr + ";");
                sbText.Append("window.parent.returnValue = arr;");
                // 关闭窗口
                sbText.Append("top.close();");
                sbText.Append("</script>");
                // 向客户端发送
                Page.RegisterStartupScript(DateTime.Now.ToString(), sbText.ToString());
                Response.Write(sbText.ToString());
            }
        }
        #endregion

        #region dgEqu_Desk_ItemDataBound
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgEqu_Desk_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");

                string id = e.Item.Cells[0].Text.Replace("&nbsp;", "");
                string sWhere = " And id = " + id;

                Equ_DeskDP ec = new Equ_DeskDP();
                DataTable dt = ec.GetDataTable(sWhere, "");
                Json json = new Json(dt);

                string jsonstr = "{record:" + json.ToJson() + "}";

                // 向客户端发送
                Button lblEquName = (Button)e.Item.FindControl("btnSel");
                //==zxl ==
                lblEquName.Attributes.Add("onclick", "ServerOndblclick(" + jsonstr + ");");
                //btnSel
               // e.Item.Attributes.Add("ondblclick", "ServerOndblclick(" + jsonstr + ");");
            }
        }
        #endregion

        #region dgEqu_Desk_ItemCreated
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgEqu_Desk_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                int j = 0;
                for (int i = 0; i < e.Item.Cells.Count - 1; i++)
                {
                    if (i > 0)
                    {
                        j = i - 1;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",1);");
                    }
                }
            }
        }
        #endregion 
    }
}
