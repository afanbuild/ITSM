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
using System.Drawing;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;
using System.Xml;

namespace Epower.ITSM.Web.AnalsysForms
{
    public partial class frmCheckPlan : BasePage
    {
        #region 设置父窗体按钮事件SetParentButtonEvent
        /// <summary>
        /// 设置父窗体按钮事件

        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_ExportExcel_Click += new Global_BtnClick(btnToExcel_Click);
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.ShowExportExcelButton(true);
            this.Master.ShowQueryButton(true);
        }
        #endregion

        #region 页面初始化 Page_Load
        /// <summary>
        /// 页面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            SetParentButtonEvent();
            cpfECustomerInfo.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(Bind);

            if (!IsPostBack)
            {
                //初始化服务单位
                SetMastCustomDboValue();

                //初始化基本配置项和关联配置项
                InitDropDownList();

                Bind();
            }
            else
            {
                if (Request.Form["__EVENTTARGET"] == "datarange")
                {
                    Bind();
                }
            }
        }

        #endregion
         
        #region 绑定配置项信息
        /// <summary>
        /// 绑定配置项
        /// </summary>
        private void InitDropDownList()
        {
            Equ_SchemaItemsDP ee = new Equ_SchemaItemsDP();
            string sWhere = " AND itemtype = 0 ";
            string sOrder = string.Empty;
            DataTable dt = ee.GetDataTable(sWhere, sOrder);


            //初始化基本配置项
            ddlSchemaItemJB.DataSource = dt;
            ddlSchemaItemJB.DataTextField = "CHName";
            ddlSchemaItemJB.DataValueField = "FieldID";
            ddlSchemaItemJB.DataBind();
            ddlSchemaItemJB.Items.Insert(0, new ListItem("", ""));

            //初始化关联配置项
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

        #region Bind
        /// <summary>
        /// 
        /// </summary>
        private void Bind()
        {
            int iRowCount = 0;
            string sCalalogID = ctrEquCataDropList1.CatelogID.ToString();
            DataTable dt;
            string sWhere = " 1=1 ";
            string sOrder = " order by ID Desc";
            Hashtable ht = new Hashtable();

            bool blnSpec = false;

            if (ddlSchemaItemJB.SelectedItem.Text.Trim() != string.Empty && txtItemValue.Text.Trim().Length > 0)
            {
                ht.Add(ddlSchemaItemJB.SelectedValue.Trim(), txtItemValue.Text.Trim());
            }

            if (ddlSchemaItemGL.SelectedItem.Text.Trim() != string.Empty)
            {
                ht.Add(ddlSchemaItemGL.SelectedValue.Trim(), (chkItemValue.Checked == true ? "1" : "0"));
            }

            Equ_DeskDP ee = new Equ_DeskDP();

            //2008-05-12分两种情况  1 是包含客户信息 或 服务单位 的用一种查询,没有包含客户信息的用另一种查询,
            if (dpdMastShortName.SelectedItem.Text.Trim() != string.Empty)
            {
                sWhere += " And mastcustid=" + dpdMastShortName.SelectedValue.Trim();
                blnSpec = true;
            }
            if (txtCust.Text.Trim() != string.Empty)
            {
                //英文名

                sWhere += " And (nvl(FullName,'') like " + StringTool.SqlQ("%" + txtCust.Text.Trim() + "%");
                //中文名

                sWhere += " OR nvl(ShortName,'') like " + StringTool.SqlQ("%" + txtCust.Text.Trim() + "%");
                //工号
                sWhere += " OR nvl(CustomCode,'') like " + StringTool.SqlQ("%" + txtCust.Text.Trim() + "%");
                //电话
                sWhere += " OR nvl(Tel1,'') like " + StringTool.SqlQ("%" + txtCust.Text.Trim() + "%");
                //EMAIL
                sWhere += " OR nvl(Email,'') like " + StringTool.SqlQ("%" + txtCust.Text.Trim() + "%");

                sWhere += ")";

                blnSpec = true;
            }

            if (blnSpec == true)
            {
                if (txtName.Text.Trim() != string.Empty)
                {
                    sWhere += " And Name like " + StringTool.SqlQ("%" + txtName.Text.Trim() + "%");
                }

                if (txtCode.Text.Trim() != string.Empty)
                {
                    sWhere += " And Code like " + StringTool.SqlQ("%" + txtCode.Text.Trim() + "%");
                }

                string strFullID = Equ_SubjectDP.GetSubjectFullID(long.Parse(sCalalogID));
                if ((sCalalogID == "1" || sCalalogID == "-1") && chkIncludeSub.Checked == true)
                {
                }
                else if (chkIncludeSub.Checked == true && strFullID.Length > 0)
                {
                    sWhere += " And SUBSTr(FullID,1," + strFullID.Length.ToString() + ") = " + StringTool.SqlQ(strFullID);
                }
                else
                {
                    sWhere += " And FullID = " + StringTool.SqlQ(strFullID);
                }

                if (!string.IsNullOrEmpty(ctrCataEquStatus.CatelogValue.Trim()))
                    sWhere += " and EquStatusID= " + ctrCataEquStatus.CatelogID.ToString();

                dt = ee.GetDataTableSpec(sWhere, sOrder, ht, this.cpfECustomerInfo.PageSize, this.cpfECustomerInfo.CurrentPage, ref iRowCount);
            }
            else
            {
                if (txtName.Text.Trim() != string.Empty)
                {
                    sWhere += " And Name like " + StringTool.SqlQ("%" + txtName.Text.Trim() + "%");
                }

                if (txtCode.Text.Trim() != string.Empty)
                {
                    sWhere += " And Code like " + StringTool.SqlQ("%" + txtCode.Text.Trim() + "%");
                }

                string strFullID = Equ_SubjectDP.GetSubjectFullID(long.Parse(sCalalogID));
                if ((sCalalogID == "1" || sCalalogID == "-1") && chkIncludeSub.Checked == true)
                {
                }
                else if (chkIncludeSub.Checked == true && strFullID.Length > 0)
                {
                    sWhere += " And SUBSTR(FullID,1," + strFullID.Length.ToString() + ") = " + StringTool.SqlQ(strFullID);
                }
                else
                {
                    sWhere += " And FullID = " + StringTool.SqlQ(strFullID);
                }

                if (!string.IsNullOrEmpty(ctrCataEquStatus.CatelogValue.Trim()))
                    sWhere += " and EquStatusID= " + ctrCataEquStatus.CatelogID.ToString();
                dt = ee.GetDataTableSpec(sWhere, sOrder, ht, this.cpfECustomerInfo.PageSize, this.cpfECustomerInfo.CurrentPage, ref iRowCount);
            }

            dgMaterialStat.DataSource = dt;
            dgMaterialStat.DataBind();

            this.cpfECustomerInfo.RecordCount = iRowCount;
            this.cpfECustomerInfo.Bind();

            //InitEquItems();  //初始化配置信息
        }
        #endregion 

        #region LoadData
        /// <summary>
        /// 
        /// </summary>
        private DataTable LoadData()
        {
            int iRowCount = 0;
            string sCalalogID = ctrEquCataDropList1.CatelogID.ToString();
            DataTable dt;
            string sWhere = " 1=1 ";
            string sOrder = " order by ID Desc";
            Hashtable ht = new Hashtable();

            bool blnSpec = false;

            if (ddlSchemaItemJB.SelectedItem.Text.Trim() != string.Empty && txtItemValue.Text.Trim().Length > 0)
            {
                ht.Add(ddlSchemaItemJB.SelectedValue.Trim(), txtItemValue.Text.Trim());
            }

            if (ddlSchemaItemGL.SelectedItem.Text.Trim() != string.Empty)
            {
                ht.Add(ddlSchemaItemGL.SelectedValue.Trim(), (chkItemValue.Checked == true ? "1" : "0"));
            }

            Equ_DeskDP ee = new Equ_DeskDP();

            //2008-05-12分两种情况  1 是包含客户信息 或 服务单位 的用一种查询,没有包含客户信息的用另一种查询,
            if (dpdMastShortName.SelectedItem.Text.Trim() != string.Empty)
            {
                sWhere += " And mastcustid=" + dpdMastShortName.SelectedValue.Trim();
                blnSpec = true;
            }
            if (txtCust.Text.Trim() != string.Empty)
            {
                //英文名

                sWhere += " And (nvl(FullName,'') like " + StringTool.SqlQ("%" + txtCust.Text.Trim() + "%");
                //中文名

                sWhere += " OR nvl(ShortName,'') like " + StringTool.SqlQ("%" + txtCust.Text.Trim() + "%");
                //工号
                sWhere += " OR nvl(CustomCode,'') like " + StringTool.SqlQ("%" + txtCust.Text.Trim() + "%");
                //电话
                sWhere += " OR nvl(Tel1,'') like " + StringTool.SqlQ("%" + txtCust.Text.Trim() + "%");
                //EMAIL
                sWhere += " OR nvl(Email,'') like " + StringTool.SqlQ("%" + txtCust.Text.Trim() + "%");

                sWhere += ")";

                blnSpec = true;
            }

            if (blnSpec == true)
            {
                if (txtName.Text.Trim() != string.Empty)
                {
                    sWhere += " And Name like " + StringTool.SqlQ("%" + txtName.Text.Trim() + "%");
                }

                if (txtCode.Text.Trim() != string.Empty)
                {
                    sWhere += " And Code like " + StringTool.SqlQ("%" + txtCode.Text.Trim() + "%");
                }

                string strFullID = Equ_SubjectDP.GetSubjectFullID(long.Parse(sCalalogID));
                if ((sCalalogID == "1" || sCalalogID == "-1") && chkIncludeSub.Checked == true)
                {
                }
                else if (chkIncludeSub.Checked == true && strFullID.Length > 0)
                {
                    sWhere += " And SUBSTR(FullID,1," + strFullID.Length.ToString() + ") = " + StringTool.SqlQ(strFullID);
                }
                else
                {
                    sWhere += " And FullID = " + StringTool.SqlQ(strFullID);
                }

                if (!string.IsNullOrEmpty(ctrCataEquStatus.CatelogValue.Trim()))
                    sWhere += " and EquStatusID= " + ctrCataEquStatus.CatelogID.ToString();

                dt = ee.GetDataTableSpec(sWhere, sOrder, ht, this.cpfECustomerInfo.PageSize, this.cpfECustomerInfo.CurrentPage, ref iRowCount);
            }
            else
            {
                if (txtName.Text.Trim() != string.Empty)
                {
                    sWhere += " And Name like " + StringTool.SqlQ("%" + txtName.Text.Trim() + "%");
                }

                if (txtCode.Text.Trim() != string.Empty)
                {
                    sWhere += " And Code like " + StringTool.SqlQ("%" + txtCode.Text.Trim() + "%");
                }

                string strFullID = Equ_SubjectDP.GetSubjectFullID(long.Parse(sCalalogID));
                if ((sCalalogID == "1" || sCalalogID == "-1") && chkIncludeSub.Checked == true)
                {
                }
                else if (chkIncludeSub.Checked == true && strFullID.Length > 0)
                {
                    sWhere += " And SUBSTR(FullID,1," + strFullID.Length.ToString() + ") = " + StringTool.SqlQ(strFullID);
                }
                else
                {
                    sWhere += " And FullID = " + StringTool.SqlQ(strFullID);
                }

                if (!string.IsNullOrEmpty(ctrCataEquStatus.CatelogValue.Trim()))
                    sWhere += " and EquStatusID= " + ctrCataEquStatus.CatelogID.ToString();
                if (ViewState["dtCheckPlan"]!=null&&ViewState["dtCheckPlan"].ToString() == "true")
                {
                    dt = ee.GetDataTableSpec(sWhere, sOrder, ht, 1000000000, 1, ref iRowCount);
                    ViewState["dtCheckPlan"] = null;
                }
                else
                {
                    dt = ee.GetDataTableSpec(sWhere, sOrder, ht, this.cpfECustomerInfo.PageSize, this.cpfECustomerInfo.CurrentPage, ref iRowCount);
                }
            }
            return dt;
        }
        #endregion 

        #region Master_Master_Button_Query_Click
        /// <summary>
        /// 
        /// </summary>
        protected void Master_Master_Button_Query_Click()
        {
            Bind();
        }
        #endregion 

        #region 导出Excel btnToExcel_Click
        /// <summary>
        /// 导出Excel
        /// </summary>
        protected void btnToExcel_Click()
        {
            ViewState["dtCheckPlan"] = "true";
             DataTable dt = LoadData();             
             Epower.ITSM.Web.Common.ExcelExport.ExportCheckPlanList(this,dt,Session["UserID"].ToString());
        }
        #endregion

        #region 初始化服务单位
        /// <summary>
        /// 初始化服务单位下拉列表的内容
        /// </summary>
        private void SetMastCustomDboValue()
        {
            DataTable dt = ZHServiceDP.GetMastCustomer();
            DataView dv = new DataView(dt);
            dv.Sort = "ID";
            dpdMastShortName.DataSource = dv;
            dpdMastShortName.DataTextField = "ShortName";
            dpdMastShortName.DataValueField = "ID";
            dpdMastShortName.DataBind();

            dpdMastShortName.Items.Insert(0, new ListItem("", "0"));

            dt.Dispose();
        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        private void InitEquItems()
        {
            Equ_SchemaItemsDP ee = new Equ_SchemaItemsDP();
            Hashtable _fields = ee.GetItemsAllFields(-1);   //获取最新的配置项情况
            XmlDocument xmldoc = new XmlDocument();
            XmlNodeList xmllist;
            string strXml = string.Empty;
            string fieldid = string.Empty;
            string fieldName = string.Empty;
            string fieldValue = string.Empty;
            string iType = string.Empty;
            string showBaseValue = string.Empty;
            string showRelValue = string.Empty;
            foreach (DataGridItem row in dgMaterialStat.Controls[0].Controls)
            {
                if (row.ItemType == ListItemType.Item || row.ItemType == ListItemType.AlternatingItem)
                {
                    showBaseValue = "基础配置：";
                    showRelValue = "关联配置：";
                    strXml = row.Cells[5].Text.Trim();
                    xmldoc.LoadXml(strXml);
                    xmllist = xmldoc.SelectNodes("Fields/Field");
                    foreach (XmlNode node in xmllist)
                    {
                        fieldid = node.Attributes["FieldName"].Value;
                        fieldValue = node.Attributes["Value"].Value;
                        if (_fields[fieldid] != null && fieldValue != string.Empty)
                        {
                            fieldName = _fields[fieldid].ToString().Substring(0, _fields[fieldid].ToString().Length - 1);
                            iType = _fields[fieldid].ToString().Substring(_fields[fieldid].ToString().Length - 1, 1);
                            if (iType == "0")
                            {
                                if (fieldValue.Trim() != string.Empty)
                                    showBaseValue += fieldName + ":" + fieldValue + " /";
                            }
                            else
                            {
                                if (fieldValue.Trim() == "1")
                                    showRelValue += fieldName + "/";
                            }
                        }
                    }
                    row.Cells[5].Text = showBaseValue + "    " + showRelValue;
                    if (row.Cells[5].Text == "基础配置：    关联配置：")
                        row.Cells[5].Text = " ";
                }
            }
        }

        protected void dgMaterialStat_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                e.Item.Attributes.Add("ondblclick", "window.open('../EquipmentManager/frmEqu_DeskEdit.aspx?IsSelect=1&id=" + e.Item.Cells[0].Text.ToString()+ "&FlowID=0&Soure=0&newWin=0', '', 'scrollbars=yes,status=yes ,resizable=yes,width=800,height=600');");
            }
        }

        protected void dgMaterialStat_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            { 
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count-1; i++)
                {
                    int j = 0;
                    if (i > 0)
                    {
                        j = i - 1;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
    }
}
