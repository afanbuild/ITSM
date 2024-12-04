using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Epower.DevBase.Organization;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;
using System.Text;

namespace Epower.ITSM.Web.MyDestop
{
    /// <summary>
    /// frmQuickLocateEqu 的摘要说明。
    /// </summary>
    public partial class frmQuickLocateEqu : BasePage
    {
        #region 属性
        protected bool IsSelect
        {
            get
            {
                if (Request["IsSelect"] != null) return true;
                else return false;
            }
        }
        #endregion

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

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.TableVisible = false;
        }
        #endregion

        #region 页面加载

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            SetParentButtonEvent();
            if (!IsPostBack)
            {
                if (Request["Name"] != null)
                    BindData(Request["Name"].ToString());
            }
        }

        #endregion

        #region 数据绑定

        /// <summary>
        /// 数据绑定
        /// </summary>
        private void BindData(string sEqu)
        {
            string sSql = " and (Name like " + StringTool.SqlQ("%" + sEqu + "%")
                        + " or Code like " + StringTool.SqlQ("%" + sEqu + "%") + ")";

            if (Request["IsChange"] != null && Request["IsChange"].ToString() == "true")
            {
                //如果从批量更新资产列表中传过来时，则只需要判断一下一个参数
                if (Request["EquIDs"] != null && Request["EquIDs"].ToString() != "")
                {
                    //排除当前在列表中的资产
                    sSql += " and id not in (" + Request["EquIDs"].ToString() + ")";
                }
            }
            else
            {
                if (Request["EquCust"] != null && Request["EquCust"].ToString().Trim() != "")
                    sSql += "And nvl(CostomName,'') like " + StringTool.SqlQ("%" + Request["EquCust"].ToString().Trim() + "%");

                if (Request["EquipmentCatalogID"] != null && decimal.Parse(Request["EquipmentCatalogID"].ToString() == "" ? "0" : Request["EquipmentCatalogID"].ToString()) > 0)
                    sSql += " And listId=" + Request["EquipmentCatalogID"].ToString();
                //else
                //    sSql += " And 1=2 "; //没传资产分类ID 查询空
            }


            Epower.ITSM.SqlDAL.Equ_DeskDP mEqu_DeskDP = new Epower.ITSM.SqlDAL.Equ_DeskDP();
           //DataTable dt = mEqu_DeskDP.GetDataTable(sSql, string.Empty);
            DataTable dt = mEqu_DeskDP.GetDataTable(sSql,string.Empty);


            dgUserInfo.Columns[2].HeaderText = PageDeal.GetLanguageValue("LitEquDeskType");
            dgUserInfo.Columns[4].HeaderText = PageDeal.GetLanguageValue("LitEquDeskName");
            dgUserInfo.Columns[5].HeaderText = PageDeal.GetLanguageValue("LitEquDeskCode");
            dgUserInfo.Columns[6].HeaderText = PageDeal.GetLanguageValue("LitCustName");
            this.dgUserInfo.DataSource = dt.DefaultView;
            this.dgUserInfo.DataBind();
        }

        #endregion

        #region Web 窗体设计器生成的代码
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.dgUserInfo.ItemCreated += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgUserInfo_ItemCreated);

        }
        #endregion

        #region gv创建事件

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgUserInfo_ItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count - 1; i++)
                {
                    // (DataView)e.Item.NamingContainer;
                    if (i > 0)
                    {
                        int j = i - 2;   //注意,因为前面有一个不可见的列
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }

        #endregion

        #region gv绑定事件

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgUserInfo_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                StringBuilder sbText = new StringBuilder();
                sbText.Append("<script>");
                sbText.Append("var arr = new Array();");
                sbText.Append("arr[1] ='" + e.Item.Cells[0].Text.Trim().Replace("&nbsp;", "") + "';");   //编号
                sbText.Append("arr[2] ='" + e.Item.Cells[4].Text.Trim().Replace("&nbsp;", "") + "';");   //名称
                sbText.Append("arr[3] ='" + e.Item.Cells[3].Text.Trim().Replace("&nbsp;", "") + "';");   //资产目录
                sbText.Append("arr[4] ='" + e.Item.Cells[1].Text.Trim().Replace("&nbsp;", "") + "';");   //资产目录ID
                sbText.Append("window.parent.returnValue = arr;");

                string requestType = "";
                if (Request["TypeFrom"] != null)
                {
                    requestType = Request.QueryString["TypeFrom"].ToString();
                }

                if (requestType == "DemandBase")
                {
                    sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "txtEqu').value = arr[2]; ");  //资产名称
                    sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidEquName').value = arr[2]; ");  //资产名称
                    sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidEqu').value = arr[1]; ");  //id                    
                    sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidListName').value = arr[3]; ");  //资产目录名称
                    sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidListID').value = arr[4]; ");  //资产目录ID
                }
                else
                {
                    sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "txtEqu').value = arr[2]; ");  //资产名称
                    sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidEquName').value = arr[2]; ");  //资产名称
                    sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidEqu').value = arr[1]; ");  //id
                    sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "txtListName').value = arr[3]; ");  //资产目录名称
                    sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidListName').value = arr[3]; ");  //资产目录名称
                    sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidListID').value = arr[4]; ");  //资产目录ID
                }

                // 关闭窗口
                sbText.Append("top.close();");
                sbText.Append("</script>");
                // 向客户端发送
                Page.RegisterStartupScript(DateTime.Now.ToString(), sbText.ToString());
                //Response.Write(sbText.ToString());
            }
        }

        #endregion

        #region gv数据绑定

        protected void dgUserInfo_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");

                if (IsSelect)
                {
                    //string value1 = e.Item.Cells[0].Text.Trim().Replace("&nbsp;", "");
                    //string value2 = e.Item.Cells[4].Text.Trim().Replace("&nbsp;", "");
                    //string value3 = e.Item.Cells[3].Text.Trim().Replace("&nbsp;", "");
                    //string value4 = e.Item.Cells[1].Text.Trim().Replace("&nbsp;", "");

                    //e.Item.Attributes.Add("ondblclick", "ServerOndblclick('" + value1 + "','" + value2 + "','" + value3 + "','" + value4 + "');");

                    Button lnkselect = (Button)e.Item.FindControl("lnkselect");
                    e.Item.Attributes.Add("ondblclick", "OnClientClick('" + lnkselect.ClientID + "');");
                }
            }
        }

        #endregion

    }
}
