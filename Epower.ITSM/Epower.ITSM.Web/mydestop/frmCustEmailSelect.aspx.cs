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
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;
using Epower.DevBase.BaseTools;
 
namespace Epower.ITSM.Web.mydestop
{
    public partial class frmCustEmailSelect : BasePage
    {
        protected string strZHHiden = "";
        protected string strZHShow = "display:none;";

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.ShowQueryButton(true);
        }
        #endregion 

        /// <summary>
        /// 查询事件
        /// </summary>
        void Master_Master_Button_Query_Click()
        {

            LoadData();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtonEvent();
            ControlPage1.DataGridToControl = dgECustomer;
            ControlPage1.On_PostBack += new EventHandler(ControlPage1_On_PostBack);
            if (!IsPostBack)
            {
                //设置显示
                PageDeal.SetLanguage(this.Controls[0]);
                dgECustomer.Columns[2].HeaderText = PageDeal.GetLanguageValue("LitMastShortName");
                dgECustomer.Columns[3].HeaderText = PageDeal.GetLanguageValue("LitCustomerType");
                dgECustomer.Columns[4].HeaderText = PageDeal.GetLanguageValue("LitCustName");
                dgECustomer.Columns[5].HeaderText = PageDeal.GetLanguageValue("LitFullName");
                dgECustomer.Columns[6].HeaderText = PageDeal.GetLanguageValue("LitContact");
                dgECustomer.Columns[7].HeaderText = PageDeal.GetLanguageValue("LitCTel");

                //绑定服务单位
                InitDropDownList();

                //加载数据
                LoadData();
            }
        }

        /// <summary>
        /// 绑定服务单位
        /// </summary>
        private void InitDropDownList()
        {
            Br_MastCustomerDP ee = new Br_MastCustomerDP();
            string sWhere = string.Empty;
            string sOrder = string.Empty;
            DataTable dt = ee.GetDataTable(sWhere, sOrder);
            ddltMastCustID.DataSource = dt;
            ddltMastCustID.DataTextField = "ShortName";
            ddltMastCustID.DataValueField = "ID";
            ddltMastCustID.DataBind();
            ddltMastCustID.Items.Insert(0, new ListItem("", ""));
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            string sWhere = "";
            if (ddltMastCustID.SelectedItem.Text.Trim() != string.Empty)
            {
                sWhere += " And E.MastCustID=" + ddltMastCustID.SelectedValue.Trim();
            }
            if (txtShortName.Text.Trim() != string.Empty)
            {
                sWhere += " And E.ShortName like " + StringTool.SqlQ("%" + txtShortName.Text.Trim() + "%");
            }
            if (txtFullName.Text.Trim() != string.Empty)
            {
                sWhere += " And E.FullName like " + StringTool.SqlQ("%" + txtFullName.Text.Trim() + "%");
            }
            if (ctrFCDServiceType.CatelogValue.ToString() != string.Empty)
            {
                sWhere += " And E.CustomerType=" + ctrFCDServiceType.CatelogID.ToString().Trim();
            }
            if (txtLinkMan1.Text.Trim() != string.Empty)
            {
                sWhere += " And E.LinkMan1 like " + StringTool.SqlQ("%" + txtLinkMan1.Text.Trim() + "%");
            }
            if (txtTel1.Text.Trim() != string.Empty)
            {
                sWhere += " And E.Tel1 like " + StringTool.SqlQ("%" + txtTel1.Text.Trim() + "%");
            }

            if (txtCustomCode.Text.Trim() != string.Empty)
            {
                sWhere += " And E.CustomCode like " + StringTool.SqlQ("%" + txtCustomCode.Text.Trim() + "%");
            }



            Br_ECustomerDP ee = new Br_ECustomerDP();
            DataTable dt = ee.GetCustomerServic(sWhere, "");
            DataView dv = new DataView(dt);
            dgECustomer.DataSource = dv;
            dgECustomer.DataBind();
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgECustomer_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;

                int j = 0;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i >= 2 && i < e.Item.Cells.Count)
                    {
                        j = i - 1;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="FunctionName"></param>
        /// <param name="UserName"></param>
        /// <param name="Email"></param>
        /// <returns></returns>
        protected string GetEmailAction(string Title, string FunctionName, string UserName, string Email)
        {
            string sResult = "";

            if (Email.Trim().Length > 0)
                sResult = "<A href='#' title='点击[" + Title + "]将 " + UserName + " 加入" + Title + "列表' " +
                    "onclick=\"" + FunctionName + "('" + UserName + "','" + Email + "')\"" +
                    ">" + Title + "</A>";
            return sResult;
        }

    }
}
