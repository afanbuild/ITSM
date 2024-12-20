/****************************************************************************
 * 
 * description:设备类别管理页面操作
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-09-22
 * *************************************************************************/
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.SqlDAL;
using System.Xml;
using System.Text;
using Epower.ITSM.Base;
using Epower.ITSM.Web.Controls;

namespace Epower.ITSM.Web.EquipmentManager
{
    /// <summary>
    /// frmSubjectedit 的摘要说明。
    /// </summary>
    public partial class frmSubjectedit : BasePage
    {
        #region 变量
        string sSubjectId = string.Empty;               //资产类别ID
        #endregion

        #region  配置项设置
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strvalue"></param>
        /// <returns></returns>
        protected bool GetDefaulCheckValue(string strvalue)
        {
            if (strvalue == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="istxt"></param>
        /// <returns></returns>
        protected string GetDefaulControlState(string type, string istxt)
        {
            string style = "display:none";
            if (type == "基础信息" && istxt == "1")
            {
                style = "";
            }
            else if (type == "备注信息" && istxt == "2")
            {
                style = "";
            }
            else if (type == "关联配置" && istxt == "0")
            {
                style = "";
            }
            else if (type == "下拉选择" && istxt == "3")
            {
                style = "";
            }
            else if (type == "部门信息" && istxt == "4")
            {
                style = "";
            }
            else if (type == "用户信息" && istxt == "5")
            {
                style = "";
            }
            else if (type == "日期类型" && istxt == "6")
            {
                style = "";
            }
            else if (type == "数值类型" && istxt == "7")
            {
                style = "";
            }
            return style;
        }
        /// <summary>
        /// 创建 datatable结构
        /// </summary>
        /// <returns></returns>
        private DataTable CreateNullTable()
        {
            DataTable dt = new DataTable("Schema");


            dt.Columns.Add("ID");
            dt.Columns.Add("CHName");
            dt.Columns.Add("Default");
            dt.Columns.Add("IsMust");//是否必填
            dt.Columns.Add("Group");
            dt.Columns.Add("TypeName");
            dt.Columns.Add("OrderBy");//排序
            dt.Columns.Add("isChack");//是否时间
            dt.Columns.Add("IsSelect");//是否设置为查询条件
            return dt;
        }

        /// <summary>
        /// 获取表单grid 的 datatabel
        /// </summary>
        /// <param name="sLastBaseGroup"></param>
        /// <param name="sLastGroup"></param>
        /// <returns></returns>
        private DataTable GetDetailItem(bool isAll, ref string sLastBaseGroup, ref string sLastGroup, ref string sLastBaseMGroup, ref string sDownLIstGrop, ref string sDropGrop, ref string sUserGrop, ref string sTimeGrop, ref string sNumberGrop)
        {
            DataTable dt = CreateNullTable();
            DataRow dr;

            foreach (DataGridItem row in dgSchema.Controls[0].Controls)
            {
                if (row.ItemType == ListItemType.Item || row.ItemType == ListItemType.AlternatingItem)
                {
                    string sTypeName = ((DropDownList)row.FindControl("ddlTypeName")).SelectedValue;
                    string sID = ((TextBox)row.FindControl("txtID")).Text;
                    string sCHName = ((TextBox)row.FindControl("txtCHName")).Text;
                    string stxtDefault = ((TextBox)row.FindControl("txtDefault")).Text;
                    string stxtMDefault = ((TextBox)row.FindControl("txtMDefault")).Text;
                    string sDorpDownListDefault = ((ctrFlowCataDropList)row.FindControl("ctrFlowCataDropDefault")).CatelogID.ToString();
                    bool blnChecked = ((CheckBox)row.FindControl("chkDefault")).Checked;

                    bool blnUserChecked = ((CheckBox)row.FindControl("CheckUser")).Checked;
                    bool blnDeptChecked = ((CheckBox)row.FindControl("CheckDept")).Checked;

                    bool blnTimeChecked = ((CheckBox)row.FindControl("CheckTime")).Checked;
                    string textNumber = ((CtrFlowNumeric)row.FindControl("TextNumber")).Value;


                    string sGroup = ((TextBox)row.FindControl("txtGroup")).Text;

                    bool isMustChecked = ((CheckBox)row.FindControl("chkIsMust")).Checked;

                    string OrderBy = ((TextBox)row.FindControl("TxtOrderBy")).Text; //排序
                    bool isChack = ((CheckBox)row.FindControl("CheckIsTime")).Checked;//是否时间
                    bool IsSelect = ((CheckBox)row.FindControl("chkIsSelect")).Checked;//是否查询条件
                    dr = dt.NewRow();

                    if ((sID.Length > 0 && sCHName.Length > 0) || isAll == true)
                    {
                        dr["TypeName"] = sTypeName;
                        dr["ID"] = sID;
                        dr["CHName"] = sCHName;
                        dr["Group"] = sGroup;
                        dr["IsMust"] = isMustChecked == true ? "1" : "0";//是否必填
                        dr["Default"] = (sTypeName == "数值类型" ? (textNumber == "" ? "0" : textNumber) : sTypeName == "日期类型" ? (blnTimeChecked == true ? "1" : "0") : sTypeName == "用户信息" ? (blnUserChecked == true ? "1" : "0") : sTypeName == "部门信息" ? (blnDeptChecked == true ? "1" : "0") : sTypeName == "下拉选择" ? sDorpDownListDefault : sTypeName == "基础信息" ? stxtDefault : sTypeName == "备注信息" ? stxtMDefault : (blnChecked == true ? "1" : "0"));
                        dr["OrderBy"] = OrderBy == "" ? "0" : OrderBy;//排序
                        dr["isChack"] = isChack == true ? "1" : "0";//是否必填
                        dr["IsSelect"] = IsSelect == true ? "1" : "0";//是否查询条件
                        dt.Rows.Add(dr);

                        if (sTypeName == "基础信息")
                        {
                            sLastBaseGroup = sGroup;
                        }
                        else if (sTypeName == "备注信息")
                        {
                            sLastBaseMGroup = sGroup;
                        }
                        else if (sTypeName == "下拉选择")
                        {
                            sDownLIstGrop = sGroup;
                        }
                        else if (sTypeName == "部门信息")
                        {
                            sDropGrop = sGroup;
                        }
                        else if (sTypeName == "用户信息")
                        {
                            sUserGrop = sGroup;
                        }
                        else if (sTypeName == "日期类型")
                        {
                            sTimeGrop = sGroup;
                        }
                        else if (sTypeName == "数值类型")
                        {
                            sNumberGrop = sGroup;
                        }
                        else
                        {
                            sLastGroup = sGroup;
                        }
                    }
                }
            }

            return dt;
        }

        #region LoadData
        /// <summary>
        /// 
        /// </summary>
        private void LoadItemData(long lngCategoryID)
        {
            string strXmlTemp = "";

            if (hidSchemaXml.Value.Trim() != string.Empty)
            {
                strXmlTemp = hidSchemaXml.Value.Trim();
            }
            else
            {
                strXmlTemp = Equ_SubjectDP.GetCatalogSchema(lngCategoryID);
            }
            DataTable dt = CreateNullTable();

            if (strXmlTemp != "")
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(strXmlTemp);

                AddBaseSchemeDatas(xmldoc, ref dt);

                XmlNodeList relnodes = xmldoc.SelectNodes("EquScheme/RelationConfig");

                foreach (XmlNode relnode in relnodes)
                {
                    AddRelSchemeDatas(relnode, ref dt);
                }

                relnodes = xmldoc.SelectNodes("EquScheme/Remark");

                foreach (XmlNode relnode in relnodes)
                {
                    AddRelRemarkDatas(relnode, ref dt);
                }

            }

            dgSchema.DataSource = dt.DefaultView;
            dgSchema.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string GetSchemaXml(DataTable dt)
        {
            if (dt.Rows.Count == 0)
                return "";

            XmlDocument xmlDoc = new XmlDocument();
            // xmlDoc.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8"" ?><EquScheme></EquScheme>");
            xmlDoc.LoadXml(@"<EquScheme></EquScheme>");
            XmlElement xmlEle = xmlDoc.CreateElement("temp");
            XmlElement xmlRoot = xmlDoc.CreateElement("temp");
            XmlElement xmlGroup = xmlDoc.CreateElement("temp");


            xmlDoc.DocumentElement.SetAttribute("Title", txtSubjectName.Text.Trim());

            //创建基础信息部分
            DataRow[] rows = dt.Select("TypeName not in ('关联配置','备注信息') ", "Group,OrderBy");

            bool blnHasBase = false;
            string sLastBaseGroup = null;  //有可能遇到空的组名

            string sBaseGroup = "";
            int i = 0;
            foreach (DataRow row in rows)
            {
                sBaseGroup = row["Group"].ToString().Trim();
                if (sBaseGroup != sLastBaseGroup)
                {
                    //创建组结点
                    xmlGroup = xmlDoc.CreateElement("BaseItem");
                    xmlGroup.SetAttribute("Title", sBaseGroup);
                    sLastBaseGroup = sBaseGroup;
                    xmlDoc.DocumentElement.AppendChild(xmlGroup);
                }

                xmlEle = xmlDoc.CreateElement("AttributeItem");
                xmlEle.SetAttribute("ID", row["ID"].ToString().Trim());
                xmlEle.SetAttribute("Type", "varchar");
                xmlEle.SetAttribute("CHName", row["CHName"].ToString().Trim());
                xmlEle.SetAttribute("IsMust", row["IsMust"].ToString().Trim());

                if (row["TypeName"].ToString() == "基础信息")
                {
                    xmlEle.SetAttribute("Default", row["Default"].ToString().Trim());
                    xmlEle.SetAttribute("CtrlType", "TextBox");
                }
                else if (row["TypeName"].ToString() == "下拉选择")
                {

                    xmlEle.SetAttribute("Default", row["Default"].ToString().Trim() == "" ? "-1" : row["Default"].ToString().Trim());
                    xmlEle.SetAttribute("CtrlType", "DropDownList");
                }
                else if (row["TypeName"].ToString() == "部门信息")
                {
                    xmlEle.SetAttribute("Default", row["Default"].ToString().Trim() == "" ? "-1" : row["Default"].ToString().Trim());
                    xmlEle.SetAttribute("CtrlType", "deptList");
                }
                else if (row["TypeName"].ToString() == "用户信息")
                {
                    xmlEle.SetAttribute("Default", row["Default"].ToString().Trim() == "" ? "0" : row["Default"].ToString().Trim());
                    xmlEle.SetAttribute("CtrlType", "UserList");
                }
                else if (row["TypeName"].ToString() == "日期类型")
                {
                    xmlEle.SetAttribute("Default", row["Default"].ToString().Trim());
                    xmlEle.SetAttribute("CtrlType", "Time");
                }
                else if (row["TypeName"].ToString() == "数值类型")
                {
                    xmlEle.SetAttribute("Default", row["Default"].ToString().Trim() == "" ? "0" : row["Default"].ToString().Trim());
                    xmlEle.SetAttribute("CtrlType", "Number");
                }
                xmlEle.SetAttribute("OrderBy", row["OrderBy"].ToString().Trim() == "" ? "0" : row["OrderBy"].ToString().Trim());//排序
                xmlEle.SetAttribute("isChack", row["isChack"].ToString().Trim() == "" ? "0" : row["isChack"].ToString().Trim());//是否时间  
                xmlEle.SetAttribute("IsSelect", row["IsSelect"].ToString().Trim() == "" ? "0" : row["IsSelect"].ToString().Trim());//是否查询条件



                xmlGroup.AppendChild(xmlEle);
            }

            //创建关联配置部分
            rows = dt.Select("TypeName='关联配置'", "Group,OrderBy");
            blnHasBase = false;
            sLastBaseGroup = null;
            sBaseGroup = "";
            i = 0;
            foreach (DataRow row in rows)
            {
                i++;
                sBaseGroup = row["Group"].ToString().Trim();
                if (sBaseGroup != sLastBaseGroup)
                {
                    //创建组结点
                    xmlGroup = xmlDoc.CreateElement("RelationConfig");
                    xmlGroup.SetAttribute("Title", sBaseGroup);
                    sLastBaseGroup = sBaseGroup;
                    xmlDoc.DocumentElement.AppendChild(xmlGroup);
                }
                xmlEle = xmlDoc.CreateElement("AttributeItem");
                xmlEle.SetAttribute("ID", row["ID"].ToString().Trim());
                xmlEle.SetAttribute("IsMust", row["IsMust"].ToString().Trim());
                xmlEle.SetAttribute("CHName", row["CHName"].ToString().Trim());
                xmlEle.SetAttribute("Default", row["Default"].ToString().Trim());
                xmlEle.SetAttribute("OrderBy", row["OrderBy"].ToString().Trim() == "" ? "0" : row["OrderBy"].ToString().Trim());
                xmlEle.SetAttribute("isChack", row["isChack"].ToString().Trim() == "" ? "0" : row["isChack"].ToString().Trim());//是否时间      
                xmlEle.SetAttribute("IsSelect", row["IsSelect"].ToString().Trim() == "" ? "0" : row["IsSelect"].ToString().Trim());//是否查询条件
                xmlGroup.AppendChild(xmlEle);
            }

            rows = dt.Select("TypeName='备注信息'", "Group,OrderBy");
            blnHasBase = false;
            sLastBaseGroup = null;
            sBaseGroup = "";
            i = 0;
            foreach (DataRow row in rows)
            {
                i++;
                sBaseGroup = row["Group"].ToString().Trim();
                if (sBaseGroup != sLastBaseGroup)
                {
                    //创建组结点
                    xmlGroup = xmlDoc.CreateElement("Remark");
                    xmlGroup.SetAttribute("Title", sBaseGroup);
                    sLastBaseGroup = sBaseGroup;
                    xmlDoc.DocumentElement.AppendChild(xmlGroup);
                }
                xmlEle = xmlDoc.CreateElement("AttributeItem");
                xmlEle.SetAttribute("ID", row["ID"].ToString().Trim());
                xmlEle.SetAttribute("CHName", row["CHName"].ToString().Trim());
                xmlEle.SetAttribute("IsMust", row["IsMust"].ToString().Trim());
                xmlEle.SetAttribute("Default", row["Default"].ToString().Trim());
                xmlEle.SetAttribute("CtrlType", "MultiLine");
                xmlEle.SetAttribute("OrderBy", row["OrderBy"].ToString().Trim() == "" ? "0" : row["OrderBy"].ToString().Trim());
                xmlEle.SetAttribute("isChack", row["isChack"].ToString().Trim() == "" ? "0" : row["isChack"].ToString().Trim());//是否时间  
                xmlEle.SetAttribute("IsSelect", row["IsSelect"].ToString().Trim() == "" ? "0" : row["IsSelect"].ToString().Trim());//是否查询条件

                xmlGroup.AppendChild(xmlEle);
            }

            return xmlDoc.InnerXml;

        }

        /// <summary>
        /// 备注信息
        /// </summary>
        /// <param name="relnode"></param>
        /// <param name="dt"></param>
        private void AddRelRemarkDatas(XmlNode relnode, ref DataTable dt)
        {
            XmlNodeList ns = null;

            string strGroup = "";
            string strTypeName = "备注信息";

            if (relnode.Attributes["Title"] != null)
            {
                strGroup = relnode.Attributes["Title"].Value;
            }
            ns = relnode.SelectNodes("AttributeItem");
            object[] values = new object[9];
            foreach (XmlNode n in ns)
            {
                values[0] = (object)n.Attributes["ID"].Value;
                values[1] = (object)n.Attributes["CHName"].Value;
                values[2] = (object)n.Attributes["Default"].Value;
                values[3] = (object)n.Attributes["IsMust"].Value;
                values[4] = (object)strGroup;
                values[5] = (object)strTypeName;
                values[6] = (object)n.Attributes["OrderBy"].Value;
                values[7] = (object)n.Attributes["isChack"].Value;
                values[8] = (object)n.Attributes["IsSelect"].Value;
                dt.Rows.Add(values);

            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="relnode"></param>
        /// <param name="dt"></param>
        private void AddRelSchemeDatas(XmlNode relnode, ref DataTable dt)
        {
            XmlNodeList ns = null;

            string strGroup = "";
            string strTypeName = "关联配置";

            if (relnode.Attributes["Title"] != null)
            {
                strGroup = relnode.Attributes["Title"].Value;
            }
            ns = relnode.SelectNodes("AttributeItem");
            object[] values = new object[9];
            foreach (XmlNode n in ns)
            {
                values[0] = (object)n.Attributes["ID"].Value;
                values[1] = (object)n.Attributes["CHName"].Value;
                values[2] = (object)n.Attributes["Default"].Value;
                values[3] = (object)n.Attributes["IsMust"].Value;
                values[4] = (object)strGroup;
                values[5] = (object)strTypeName;
                values[6] = (object)n.Attributes["OrderBy"].Value;
                values[7] = (object)n.Attributes["isChack"].Value;
                values[8] = (object)n.Attributes["IsSelect"].Value;
                dt.Rows.Add(values);

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <param name="dt"></param>
        private void AddBaseSchemeDatas(XmlDocument xmldoc, ref DataTable dt)
        {
            XmlNodeList bnodes = xmldoc.SelectNodes("EquScheme/BaseItem");
            string strGroup = "";
            foreach (XmlNode node in bnodes)
            {
                strGroup = "";

                if (node.Attributes["Title"] != null)
                {
                    strGroup = node.Attributes["Title"].Value;
                }
                object[] values = new object[9];

                XmlNodeList ns = node.SelectNodes("AttributeItem");

                foreach (XmlNode n in ns)
                {
                    values[0] = (object)n.Attributes["ID"].Value;
                    values[1] = (object)n.Attributes["CHName"].Value;
                    values[2] = (object)n.Attributes["Default"].Value;
                    values[3] = (object)n.Attributes["IsMust"].Value;
                    values[4] = (object)strGroup;
                    if (n.Attributes["CtrlType"].Value == "TextBox")
                    {
                        values[5] = (object)"基础信息";
                    }
                    else if (n.Attributes["CtrlType"].Value == "DropDownList")
                    {
                        values[5] = (object)"下拉选择";
                    }
                    else if (n.Attributes["CtrlType"].Value == "deptList")
                    {
                        values[5] = (object)"部门信息";
                    }
                    else if (n.Attributes["CtrlType"].Value == "UserList")
                    {
                        values[5] = (object)"用户信息";
                    }
                    else if (n.Attributes["CtrlType"].Value == "Time")
                    {
                        values[5] = (object)"日期类型";
                    }
                    else if (n.Attributes["CtrlType"].Value == "Number")
                    {
                        values[5] = (object)"数值类型";
                    }
                    else
                    {
                        values[5] = (object)"基础信息";
                    }
                    values[6] = (object)n.Attributes["OrderBy"].Value;
                    values[7] = (object)n.Attributes["isChack"].Value;
                    values[8] = (object)n.Attributes["IsSelect"].Value;
                    dt.Rows.Add(values);
                }
            }
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
            string s2 = "";
            string s1 = "";
            string s3 = "";
            string s4 = "";
            string s5 = "";
            string s6 = "";
            string s7 = "";
            string s8 = "";
            DataTable dt = GetDetailItem(true, ref s1, ref s2, ref s3, ref s4, ref s5, ref s6, ref s7, ref s8);
            bool hasDeleted = false;
            if (e.CommandName == "delete")
            {
                dt.Rows.RemoveAt(e.Item.ItemIndex);
                hasDeleted = true;
            }

            if (hasDeleted == true)
            {
                dgSchema.DataSource = dt.DefaultView;
                dgSchema.DataBind();
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

                DropDownList list = (DropDownList)e.Item.FindControl("ddlTypeName");
                if (list.SelectedItem.Text == "关联配置")
                {
                    CheckBox chkbox = ((CheckBox)e.Item.FindControl("chkIsMust"));
                    chkbox.Visible = false;
                    chkbox = ((CheckBox)e.Item.FindControl("chkIsSelect"));
                    chkbox.Visible = false;
                }
                else if (list.SelectedItem.Text == "下拉选择")
                {
                    ctrFlowCataDropList DropList = ((ctrFlowCataDropList)e.Item.FindControl("ctrFlowCataDropDefault"));
                    string FieldID = DataBinder.Eval(e.Item.DataItem, "ID").ToString();
                    string Defaultvalue = e.Item.Cells[9].Text.ToString() == "" ? "0" : e.Item.Cells[9].Text.ToString();//初始默认值

                    //获取对应的rootid
                    Equ_SchemaItemsDP ee = new Equ_SchemaItemsDP();
                    ee = ee.GetReCorded(FieldID);
                    DropList.RootID = long.Parse(ee.CatalogID.ToString() == "" ? "1" : ee.CatalogID.ToString());
                    DropList.CatelogID = long.Parse(Defaultvalue);

                }
                else if (list.SelectedItem.Text == "备注信息")
                {
                    CheckBox chkbox = ((CheckBox)e.Item.FindControl("chkIsSelect"));
                    chkbox.Visible = false;
                }
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
                for (int i = 0; i < e.Item.Cells.Count - 1; i++)
                {
                    if (0 < i && i < 5)
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
        protected void btnAddNewItem_Click(object sender, EventArgs e)
        {
            string sLastGroup = "";
            string sLastBaseGroup = "";
            string sLastBaseMGroup = "";
            string sDownListGrop = "";//下拉选择
            string sNewFieldID = "";

            string sDropGrop = "";//部门
            string sUserGrop = "";//用户
            string sTimeGrop = "";//日期
            string sNumberGrop = "";//数值

            string sArrValue = hidTempID.Value.Trim();
            string[] arrListValue = sArrValue.Split(',');

            string StrAlert = string.Empty;
            int i = 0;
            foreach (string returnVlue in arrListValue)
            {


                sNewFieldID = returnVlue;

                DataTable dt = GetDetailItem(false, ref sLastBaseGroup, ref sLastGroup, ref sLastBaseMGroup, ref sDownListGrop, ref sDropGrop, ref sUserGrop, ref sTimeGrop, ref sNumberGrop);

                DataRow[] rows = dt.Select("ID='" + sNewFieldID.Trim() + "'");
                Equ_SchemaItemsDP es = new Equ_SchemaItemsDP();
                es = es.GetReCorded(sNewFieldID);

                if (rows.Length == 0 && es.ID != 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["TypeName"] = es.itemType == 7 ? "数值类型" : es.itemType == 6 ? "日期类型" : es.itemType == 4 ? "部门信息" : es.itemType == 5 ? "用户信息" : es.itemType == 3 ? "下拉选择" : es.itemType == 0 ? "基础信息" : es.itemType == 2 ? "备注信息" : "关联配置";
                    dr["ID"] = es.FieldID.Trim();
                    dr["CHName"] = es.CHName.Trim();
                    dr["IsMust"] = "0";
                    dr["Group"] = es.itemType == 7 ? sNumberGrop : es.itemType == 6 ? sTimeGrop : es.itemType == 4 ? sDropGrop : es.itemType == 5 ? sUserGrop : es.itemType == 3 ? sDownListGrop : es.itemType == 0 ? sLastBaseGroup : es.itemType == 2 ? sLastBaseMGroup : sLastGroup;
                    dr["Default"] = (es.itemType == 6 ? "0" : es.itemType == 7 ? "0" : es.itemType == 4 ? "1" : es.itemType == 5 ? "1" : es.itemType == 3 ? "1" : es.itemType == 0 ? "" : es.itemType == 2 ? "" : "1");
                    dr["OrderBy"] = "0";
                    dr["isChack"] = "0";//是否时间 
                    dr["IsSelect"] = "0";//是否时间                     
                    dt.Rows.Add(dr);
                    dgSchema.DataSource = dt.DefaultView;
                    dgSchema.DataBind();
                }
                else
                {
                    if (i == 0)
                    {
                        StrAlert = es.CHName;
                    }
                    else
                    {
                        StrAlert += "," + es.CHName;
                    }
                    i++;
                }

            }
            if (StrAlert != string.Empty)
            {
                StringBuilder sbText = new StringBuilder();
                sbText.Append("<script>");

                // 关闭窗口
                sbText.Append("alert('配置项 " + StrAlert + " 已经存在');");
                //sbText.Append("alertExist();"); 
                sbText.Append("</script>");
                Response.Write(sbText.ToString());
            }
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            // 在此处放置用户代码以初始化页面

            rdoSchema.Items[0].Attributes.Add("onclick", "rdoSchema_onchange(this);");

            if (!IsPostBack)
            {
                if (this.Request.QueryString["SubjectID"] != null)
                {
                    sSubjectId = this.Request.QueryString["SubjectID"].ToString();
                    // 记录当前分类
                    Session["OldEQSubectID"] = long.Parse(sSubjectId);

                    hidCatalogID.Value = sSubjectId;
                    LoadData(StringTool.String2Long(sSubjectId));
                }


                ctrSetUserOtherRight2.OperateType = 30;             //权限类别，对应OperateType信息项；资产权限值为30
                if (string.IsNullOrEmpty(sSubjectId))
                {
                    ctrSetUserOtherRight2.OperateID = 0;
                }
                else
                {
                    ctrSetUserOtherRight2.OperateID = int.Parse(sSubjectId);
                }
                //if (Session["EquSchemaMainUnSaved"] != null)
                //{
                //    //移除保存过的信息   有不保存的情况
                //    Session.Remove("EquSchemaMainUnSaved");
                //}
                hidSchemaXml.Value = string.Empty;

                if (hidCatalogID.Value.Trim() != string.Empty)
                {
                    LoadItemData(long.Parse(hidCatalogID.Value.Trim()));
                }
                else
                {
                    LoadItemData(long.Parse(hidPCatalogID.Value.Trim()));

                }
                if (rdoSchema.SelectedValue == "0")
                {
                    dgSchema.Columns[8].Visible = false;
                    cbtnAdd.Visible = false;
                    cbtnNew.Visible = false;
                }
                else
                {
                    dgSchema.Columns[8].Visible = true;
                    RightEntity re = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.SchemaItemsMain];
                    cbtnAdd.Visible = re.CanAdd;
                    cbtnNew.Visible = re.CanAdd;
                }
                if (hidPCatalogID.Value == "-1")
                {
                    dgSchema.Columns[8].Visible = true;
                    RightEntity re = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.SchemaItemsMain];
                    cbtnAdd.Visible = re.CanAdd;
                    cbtnNew.Visible = re.CanAdd;
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngCatalogID"></param>
        private void LoadData(long lngSubjectID)
        {
            DataTable dt = Equ_SubjectDP.GetSubjectByID(lngSubjectID);

            if (dt.Rows.Count < 1)
            {
                cmdAdd.Enabled = false;
                cmdSave.Enabled = false;
                cmdDelete.Enabled = false;
            }

            foreach (DataRow dr in dt.Rows)
            {
                txtSubjectName.Text = dr["CatalogName"].ToString();
                txtDesc.Text = dr["Remark"].ToString();
                hidPCatalogID.Value = dr["ParentID"].ToString();
                txtSortID.Text = dr["SortID"].ToString();
                hidSchemaXml.Value = dr["ConfigureSchema"].ToString();
                hidImage.Value = dr["ImageUrl"].ToString();
                Image1.ImageUrl = dr["ImageUrl"].ToString();
                if (hidPCatalogID.Value == "-1")	//(lngCatalogID == 1)//根分类
                {
                    txtPCatalogName.Text = "无";
                }
                if (hidPCatalogID.Value == "-1")
                {
                    //第一级 不容许删除
                    cmdDelete.Enabled = false;

                    rdoSchema.Items[0].Enabled = false;
                    rdoSchema.Items[1].Selected = true;

                }
                else
                {
                    if (dr["InheritSchema"].ToString() != "")
                    {
                        if (dr["InheritSchema"].ToString() == "1")
                        {
                            rdoSchema.SelectedIndex = 1;
                        }
                        else
                        {
                            rdoSchema.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        rdoSchema.SelectedIndex = 0;
                    }
                }
                txtPCatalogName.Text = Equ_SubjectDP.GetSubjectName(StringTool.String2Long(hidPCatalogID.Value));
            }
        }


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

        }
        #endregion

        protected void cmdSave_Click(object sender, System.EventArgs e)
        {
            string sOrgID = string.Empty;
            if (Session["UserOrgID"] != null)
            {
                sOrgID = Session["UserOrgID"].ToString();
            }
            else
            {
                sOrgID = "-1";
            }

            try
            {
                string sLastGroup = "";
                string sLastBaseGroup = "";
                string sLastBaseMGroup = "";
                string sDownListGrop = "";
                string sDeptGrop = "";
                string sUserGrop = "";
                string sTimeGrop = "";
                string sNumberGrop = "";
                DataTable dt = GetDetailItem(false, ref sLastBaseGroup, ref sLastGroup, ref sLastBaseMGroup, ref sDownListGrop, ref sDeptGrop, ref sUserGrop, ref sTimeGrop, ref sNumberGrop);
                string strXml = GetSchemaXml(dt);

                //保存session
                hidSchemaXml.Value = strXml;
                string strSubjectID = Equ_SubjectDP.Save(StringTool.String2Long(hidCatalogID.Value), txtSubjectName.Text.Trim(), StringTool.String2Long(hidPCatalogID.Value), int.Parse(txtSortID.Text.Trim()), txtDesc.Text.Trim(), sOrgID, rdoSchema.SelectedIndex, hidSchemaXml.Value.Trim(), hidImage.Value.Trim());
                hidCatalogID.Value = strSubjectID;

                //强制分类相关缓存失效 
                HttpRuntime.Cache.Insert("CommCacheValidEquCategory", false);

                hidModified.Value = "false";

                //如过从查询分类界面打开，保存后关闭并刷新父窗体
                if (Request["CloseAction"] != null && Request["CloseAction"].ToString().Equals("1"))
                {
                    PageTool.AddJavaScript(this, "window.parent.returnValue = 'refresh';self.close();");
                }
                else
                {
                    PageTool.AddJavaScript(this, "window.parent.contents.location='frmEqu_Content.aspx?type=0'");
                    PageTool.AddJavaScript(this, "window.parent.subjectinfo.location='frmSubjectedit.aspx?Subjectid=" + hidCatalogID.Value + "'");
                }
            }
            catch (Exception ee)
            {
                PageTool.MsgBox(this, ee.Message.ToString());
            }

        }

        protected void cmdAdd_Click(object sender, System.EventArgs e)
        {
            string sSubjectId = this.Request.QueryString["SubjectID"].ToString();
            txtSubjectName.Text = string.Empty;
            txtDesc.Text = string.Empty;
            hidPCatalogID.Value = string.Empty;
            hidCatalogID.Value = string.Empty;//清空分类标识

            hidImage.Value = string.Empty;
            Image1.ImageUrl = string.Empty;

            //加载默认上级分类，默认主管，默认领导
            DataTable dt = Equ_SubjectDP.GetSubjectByID(StringTool.String2Long(sSubjectId));
            foreach (DataRow dr in dt.Rows)
            {
                hidPCatalogID.Value = sSubjectId == null ? "0" : sSubjectId.Trim();
                txtPCatalogName.Text = dr["CatalogName"].ToString();
                hidModified.Value = "false";
                //缺省继承上级的配置
                hidSchemaXml.Value = Equ_SubjectDP.GetCatalogSchema(sSubjectId == null ? 0 : long.Parse(sSubjectId.Trim()));
                rdoSchema.Items[0].Enabled = true;
                rdoSchema.SelectedIndex = 0;



            }
            txtSortID.Text = "-1";
            dgSchema.Columns[6].Visible = false;
        }

        protected void cmdDelete_Click(object sender, System.EventArgs e)
        {
            try
            {
                Equ_SubjectDP.Delete(long.Parse(hidCatalogID.Value.Trim()));

                //强制分类相关缓存失效 
                HttpRuntime.Cache.Insert("CommCacheValidEquCategory", false);

                //Session["OldEQSubectID"] = strSubjectID;

                //如过从查询分类界面打开，保存后关闭并刷新父窗体
                if (Request["CloseAction"] != null && Request["CloseAction"].ToString().Equals("1"))
                    PageTool.AddJavaScript(this, "window.parent.returnValue = 'refresh';self.close();");
                else
                    PageTool.AddJavaScript(this, "window.parent.contents.location='frmEqu_Content.aspx?type=0&CurrSubjectID=1';window.location='about:blank'");
            }
            catch (Exception ee)
            {
                PageTool.MsgBox(this, "删除分类时出现错误，错误为：" + ee.Message.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdoSchema_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdoSchema.SelectedValue == "0")
            {
                dgSchema.Columns[8].Visible = false;
                cbtnAdd.Visible = false;
                cbtnNew.Visible = false;

                string sSubjectId = this.Request.QueryString["SubjectID"].ToString();
                //缺省继承上级的配置
                hidSchemaXml.Value = Equ_SubjectDP.GetCatalogSchema(sSubjectId == null ? 0 : long.Parse(sSubjectId.Trim()));

            }
            else
            {
                dgSchema.Columns[8].Visible = true;
                RightEntity re = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.SchemaItemsMain];
                cbtnAdd.Visible = re.CanAdd;
                cbtnNew.Visible = re.CanAdd;

                hidSchemaXml.Value = string.Empty;                
            }

            LoadItemData(long.Parse(hidCatalogID.Value.Trim() == "" ? "0" : hidCatalogID.Value.Trim()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSelectImage_Click(object sender, EventArgs e)
        {
            Image1.ImageUrl = this.hidImage.Value.Trim();
        }


    }
}
