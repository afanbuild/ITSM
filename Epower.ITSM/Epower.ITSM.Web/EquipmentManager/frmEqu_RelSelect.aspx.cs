/****************************************************************************
 * 
 * description:资产关联选择
 * 
 * 
 * 
 * Create by:
 * Create Date:2008-09-26
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

using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;
using System.Xml;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frmEqu_RelSelect : BasePage
    {
        public string Opener_ClientId
        {
            set
            {
                ViewState["Opener_ClientId"] = value;
            }
            get
            {
                return (ViewState["Opener_ClientId"] == null) ? "" : ViewState["Opener_ClientId"].ToString();
            }
        }

        #region 设备分类ID
        /// <summary>
        /// 设备分类ID
        /// </summary>
        protected string CatalogID
        {
            get
            {
                if (Request["subjectid"] != null && Request["subjectid"] != "")
                    return Request["subjectid"].ToString();
                else return "-1";
            }
        }
        #endregion

        #region EquID
        /// <summary>
        /// EquID
        /// </summary>
        protected long EquID
        {
            get { if (Request["EquID"] != null) return long.Parse(Request["EquID"].ToString()); else return 0; }
        }
        #endregion

        #region EquIDs
        /// <summary>
        /// EquIDs
        /// </summary>
        protected string EquIDs
        {
            get { if (Request["EquIDs"] != null) return Request["EquIDs"].ToString(); else return "0"; }
        }
        #endregion

        #region 自定义项
        /// <summary>
        /// 
        /// </summary>
        protected void DeleteItem()
        {
            string sSQL = string.Empty;
            StringBuilder sb = new StringBuilder();
            sSQL = "update Equ_DeskDefineItem set Deleted=" + ((int)eRecord_Status.eDeleted).ToString() + " where 1=1 ";
            if (dgEqu_Desk.Items.Count > 0)
            {
                //批量删除
                for (int i = 0; i < dgEqu_Desk.Items.Count; i++)
                {
                    CheckBox chkDel = (CheckBox)dgEqu_Desk.Items[i].Cells[0].FindControl("chkDel");
                    if (chkDel != null && chkDel.Checked)
                    {
                        if (string.IsNullOrEmpty(sb.ToString()))
                            sb.Append(" ContractID=" + dgEqu_Desk.Items[i].Cells[1].Text.Trim());
                        else
                            sb.Append(" or ContractID=" + dgEqu_Desk.Items[i].Cells[1].Text.Trim());
                    }

                }
            }
            if (!string.IsNullOrEmpty(sb.ToString()))
            {
                sSQL += " and (" + sb.ToString() + ")";
                CommonDP.ExcuteSql(sSQL.ToString());
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
            this.Master.IsCheckRight = false;
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);
            this.Master.ShowQueryPageButton();
            this.Master.MainID = "1";
            this.Master.ShowNewButton(false);
            this.Master.ShowDeleteButton(false);
        }
        #endregion

        #region Master_Master_Button_New_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("frmEqu_DeskEdit.aspx");
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

        #region Master_Master_Button_Delete_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            Equ_DeskDP ee = new Equ_DeskDP();
            foreach (DataGridItem itm in dgEqu_Desk.Items)
            {
                if (itm.ItemType == ListItemType.AlternatingItem ||
                    itm.ItemType == ListItemType.Item)
                {
                    string sID = itm.Cells[1].Text;
                    CheckBox chkdel = (CheckBox)itm.Cells[0].FindControl("chkDel");
                    if (chkdel.Checked)
                    {
                        ee.DeleteRecorded(long.Parse(sID));
                    }
                }
            }
            //删除自定义项
            DeleteItem();

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
            cpfECustomerInfo.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(Bind);
            if (!IsPostBack)
            {
                String strRelKeyIndex = Request.QueryString["ddlrelkeyindex"];
                if (!String.IsNullOrEmpty(strRelKeyIndex))
                    ViewState["ddlrelkeyindex"] = strRelKeyIndex;

                if (Request["Opener_ClientId"] != null)
                {
                    Opener_ClientId = Request["Opener_ClientId"].ToString().Replace("hidClientId_ForOpenerPage", "");
                }

                PageDeal.SetLanguage(this.Controls[0]);

                dgEqu_Desk.Columns[2].HeaderText = PageDeal.GetLanguageValue("LitEquDeskName");
                dgEqu_Desk.Columns[3].HeaderText = PageDeal.GetLanguageValue("LitEquDeskCode");

                InitDropDownList();

                DataTable dt;
                if (Request["EquName"] != null)
                    txtName.Text = Request["EquName"].ToString();
                if (Request["Cust"] != null)
                    txtCust.Text = Request["Cust"].ToString();

                //资产类别初始化
                if (Request["subjectid"] != null)
                {
                    CtrEquCataDropList1.CatelogID = decimal.Parse(Request["subjectid"].ToString());

                    InitDropDownList(Request["subjectid"].ToString());
                }
                Bind();
            }
        }
        #endregion


        #region InitDropDownList

        /// <summary>
        /// 绑定配置项
        /// </summary>
        private void InitDropDownList()
        {
            Equ_SchemaItemsDP ee = new Equ_SchemaItemsDP();
            string sWhere = " AND itemtype = 0 ";
            string sOrder = string.Empty;
            DataTable dt = ee.GetDataTable(sWhere, sOrder);



            ddlSchemaItemJB.DataSource = dt;
            ddlSchemaItemJB.DataTextField = "CHName";
            ddlSchemaItemJB.DataValueField = "FieldID";
            ddlSchemaItemJB.DataBind();
            ddlSchemaItemJB.Items.Insert(0, new ListItem("", ""));


            sWhere = " AND itemtype = 1 ";
            sOrder = string.Empty;
            dt = ee.GetDataTable(sWhere, sOrder);
            ddlSchemaItemGL.DataSource = dt;
            ddlSchemaItemGL.DataTextField = "CHName";
            ddlSchemaItemGL.DataValueField = "FieldID";
            ddlSchemaItemGL.DataBind();
            ddlSchemaItemGL.Items.Insert(0, new ListItem("", ""));

        }


        #endregion

        #region LoadData
        /// <summary>
        /// 
        /// </summary>
        private void Bind()
        {
            int iRowCount = 0;
            string sCalalogID = CtrEquCataDropList1.CatelogID.ToString();
            DataTable dt;
            string sWhere = string.Empty;
            string sOrder = " order by id desc";
            Hashtable ht = new Hashtable();

            sWhere = " 1=1 AND deleted =0 ";
            if (this.EquID != 0)
            {
                //除去当前资产
                sWhere += " AND id <> " + this.EquID.ToString();
            }

            if (this.EquIDs != "0")
            {
                //除去当前资产串
                sWhere += " AND id not in (" + this.EquIDs.ToString() + ")";
            }

            if (txtName.Text.Trim() != string.Empty)
            {
                sWhere += " And Name like " + StringTool.SqlQ("%" + txtName.Text.Trim() + "%");
            }
            if (txtCust.Text.Trim() != string.Empty)
            {
                sWhere += " And nvl(CostomName,'') like " + StringTool.SqlQ("%" + txtCust.Text.Trim() + "%");
            }
            if (txtCode.Text.Trim() != string.Empty)
            {
                sWhere += " And Code like " + StringTool.SqlQ("%" + txtCode.Text.Trim() + "%");
            }
            string strFullID = Equ_SubjectDP.GetSubjectFullID(long.Parse(sCalalogID));

            if (sCalalogID == "1" || sCalalogID == "-1")
            {
            }
            else
            {
                sWhere += " And FullID = " + StringTool.SqlQ(strFullID);
            }

            if (!string.IsNullOrEmpty(ctrCataEquStatus.CatelogValue.Trim()))
                sWhere += " and EquStatusID= " + ctrCataEquStatus.CatelogID.ToString();


            if (ddlSchemaItemJB.SelectedItem.Text.Trim() != string.Empty && txtItemValue.Text.Trim().Length > 0)
            {
                ht.Add(ddlSchemaItemJB.SelectedValue.Trim(), txtItemValue.Text.Trim());
            }

            if (ddlSchemaItemGL.SelectedItem.Text.Trim() != string.Empty)
            {
                ht.Add(ddlSchemaItemGL.SelectedValue.Trim(), (chkItemValue.Checked == true ? "1" : "0"));
            }

            Equ_DeskDP ee = new Equ_DeskDP();
            dt = ee.GetDataTable(sWhere, sOrder, ht, this.cpfECustomerInfo.PageSize, this.cpfECustomerInfo.CurrentPage, ref iRowCount);
            dgEqu_Desk.DataSource = dt;
            dgEqu_Desk.DataBind();
            this.cpfECustomerInfo.RecordCount = iRowCount;
            this.cpfECustomerInfo.Bind();
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
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i > 1 && i < 7)
                    {
                        int j = i - 1;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataGridItem itm in dgEqu_Desk.Items)
            {
                if (itm.ItemType == ListItemType.AlternatingItem ||
                    itm.ItemType == ListItemType.Item)
                {
                    string sID = itm.Cells[1].Text;
                    CheckBox chkdel = (CheckBox)itm.Cells[0].FindControl("chkDel");
                    if (chkdel.Checked)
                    {
                        sb.Append(sID + ",");
                    }
                }
            }
            StringBuilder sbText = new StringBuilder();
            sbText.Append("<script>");
            sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidCustArrID", sb.ToString());
            sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidFlag", "OK");
            sbText.AppendFormat("window.opener.document.all.{0}.click();", Opener_ClientId + "btnAddHid");

            // 关闭窗口
            sbText.Append("top.close();");
            sbText.Append("</script>");
            // 向客户端发送
            Page.RegisterStartupScript(DateTime.Now.ToString(), sbText.ToString());
            Response.Write(sbText.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClose_Click(object sender, EventArgs e)
        {
            StringBuilder sbText = new StringBuilder();
            sbText.Append("<script>");
            // 关闭窗口
            sbText.Append("top.close();");
            sbText.Append("</script>");
            // 向客户端发送
            Page.RegisterStartupScript(DateTime.Now.ToString(), sbText.ToString());
            Response.Write(sbText.ToString());
        }

        protected void dgEqu_Desk_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");


                string value2 = e.Item.Cells[4].Text.Trim().Replace("&nbsp;", "");
                // 联系人
                string value3 = e.Item.Cells[5].Text.Trim().Replace("&nbsp;", "");
                // 联系电话
                //e.Item.Attributes.Add("ondblclick", "ServerOndblclick('" + e.Item.Cells[1].Text + ",');");
            }
        }

        #region 更改资产类别时，同时更改其绑定配置项内容
        /// <summary>
        /// 更改资产类别时，同时更改其绑定配置项内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnOK_Click(object sender, EventArgs e)
        {
            InitDropDownList(CtrEquCataDropList1.CatelogID.ToString());
        }
        #endregion

        #region InitDropDownList
        /// <summary>
        /// 绑定配置项
        /// </summary>
        private void InitDropDownList(string strCatalogID)
        {
            string strSchemaXml = string.Empty;                                             //配置项xml            
            DataTable dt = Equ_SubjectDP.GetSubjectByID(long.Parse(strCatalogID));          //得到此类别下的配置项xml
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    strSchemaXml = dt.Rows[0]["ConfigureSchema"].ToString();
                }
            }

            Equ_SchemaItemsDP ee = new Equ_SchemaItemsDP();

            #region 基本配置项
            string sWhere = " AND itemtype = 0 ";
            string sOrder = string.Empty;
            string strFieldID = "(0";                                                       //存放资产类别下各基本配置项FieldID

            strFieldID = strFieldID + AnalySchema(strSchemaXml, "EquScheme/BaseItem/AttributeItem");           //解析基本配置项xml，得到各FieldID的集合
            sWhere += " AND FieldID in " + strFieldID + ")";                                                   //加上筛选条件

            dt = ee.GetDataTable(sWhere, sOrder);
            ddlSchemaItemJB.DataSource = dt;
            ddlSchemaItemJB.DataTextField = "CHName";
            ddlSchemaItemJB.DataValueField = "FieldID";
            ddlSchemaItemJB.DataBind();
            ddlSchemaItemJB.Items.Insert(0, new ListItem("", ""));
            #endregion
        }
        #endregion

        #region 解析配置项xml
        /// <summary>
        /// 解析配置项xml
        /// </summary>
        private string AnalySchema(string strSchemaXml, string strLists)
        {
            string strFieldID = string.Empty;

            if (strSchemaXml != "")
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(strSchemaXml);

                XmlNodeList bnodes = xmlDoc.SelectNodes(strLists);
                foreach (XmlNode node in bnodes)
                {
                    strFieldID += "," + node.Attributes["ID"].Value;
                }
            }
            return strFieldID;
        }
        #endregion
    }
}
