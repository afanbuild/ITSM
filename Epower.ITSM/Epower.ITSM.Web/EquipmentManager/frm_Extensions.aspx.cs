using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Epower.ITSM.SqlDAL.Customer;
using Epower.ITSM.Base;
using Epower.DevBase.BaseTools;
using System.Text;
using Epower.ITSM.Web.Controls;
using Epower.ITSM.SqlDAL;
using System.Xml;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frm_Extensions : BasePage
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
        /// 是否选择状态
        /// </summary>
        protected bool IsSelect
        {
            get { if (Request["IsSelect"] != null) return true; else return false; }
        }
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
            dt.Columns.Add("GroupID");
            dt.Columns.Add("GroupName");  //所属分组
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

                    string GroupName = ((TextBox)row.FindControl("TxtGroupName")).Text; //所属分组
                    string GroupID = ((TextBox)row.FindControl("TxtGroupID")).Text;
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
                        dr["GroupID"] = GroupID;
                        dr["GroupName"] = GroupName;
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
        private void LoadItemData()
        {
            
            BR_ExtendsFieldsDP br_extendsfields=new BR_ExtendsFieldsDP();
            string strXmlTemp = "";
            DataTable dt_Extend = br_extendsfields.GetDataTable("","");
            DataTable dtContent = CreateNullTable();
            if (dt_Extend.Rows.Count > 0)
            {
                DataTable dt = CreateNullTable();
                foreach (DataRow dr in dt_Extend.Rows)
                {
                    if (hidSchemaXml.Value.Trim() != string.Empty)
                    {
                        strXmlTemp = hidSchemaXml.Value.Trim();
                    }
                    else {
                        strXmlTemp = dr["KeyValue"].ToString();

                    }
                    
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
                }
                foreach (DataRow dr in dt.Rows)
                {
                    dtContent.Rows.Add(dr.ItemArray);
                }
               

            }
            string sWhere = "1='1'";
            if (txtCHName.Text.Trim() != "")
            {
                 sWhere += " And CHName like '%" + txtCHName.Text.Trim() + "%'";
            }
            if (ddlType.SelectedItem.Value != "")
            {
                 sWhere += " And GroupID='" + ddlType.SelectedValue+"'";
            }
            DataRow [] drs = dtContent.Select(sWhere);
            DataTable dtSeletedSchema = dtContent.Clone();
            if (drs.Length > 0)
            {
                for (int i = 0; i < drs.Length; i++)
                {
                    dtSeletedSchema.ImportRow(drs[i]);
                }
            }

            dgSchema.DataSource = dtSeletedSchema.DefaultView;
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


            xmlDoc.DocumentElement.SetAttribute("Title", "用户自定义扩展项");

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
                xmlEle.SetAttribute("GroupName",row["GroupName"].ToString().Trim());
                xmlEle.SetAttribute("GroupID",row["GroupID"].ToString().Trim());

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
                xmlEle.SetAttribute("GroupName",row["GroupName"].ToString().Trim());
                xmlEle.SetAttribute("GroupID",row["GroupID"].ToString().Trim());
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
                xmlEle.SetAttribute("GroupName",row["GroupName"].ToString().Trim());
                xmlEle.SetAttribute("GroupID",row["GroupID"].ToString().Trim());

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
            object[] values = new object[11];
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
                values[9] = (object)n.Attributes["GroupID"].Value;
                values[10] = (object)n.Attributes["GroupName"].Value;
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
            object[] values = new object[11];
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
                values[9] = (object)n.Attributes["GroupID"].Value;
                values[10] = (object)n.Attributes["GroupName"].Value;
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
                object[] values = new object[11];

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
                    values[9] = (object)n.Attributes["GroupID"].Value;
                    values[10] = (object)n.Attributes["GroupName"].Value;
                    dt.Rows.Add(values);
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
                
                string FieldID = DataBinder.Eval(e.Item.DataItem, "ID").ToString();
                string Defaultvalue = e.Item.Cells[10].Text.ToString() == "" ? "0" : e.Item.Cells[10].Text.ToString();//初始默认值
                Br_ExtensionsItemsDP br_extensionitems = new Br_ExtensionsItemsDP();
                br_extensionitems.GetReCorded(FieldID);

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
                    
                    //获取对应的rootid
                   
                    //DropList.RootID = long.Parse(ee.CatalogID.ToString() == "" ? "1" : ee.CatalogID.ToString());
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
                Br_ExtensionsItemsDP es = new Br_ExtensionsItemsDP();
                
                es= es.GetReCorded(sNewFieldID);
              

                if (rows.Length == 0 && es.ID != 0)
                {
                    DataRow dr = dt.NewRow();

                    dr["TypeName"] = es.ITEMTYPE == 7 ? "数值类型" : es.ITEMTYPE == 6 ? "日期类型" : es.ITEMTYPE == 4 ? "部门信息" : es.ITEMTYPE == 5 ? "用户信息" : es.ITEMTYPE == 3 ? "下拉选择" : es.ITEMTYPE == 0 ? "基础信息" : es.ITEMTYPE == 2 ? "备注信息" : "关联配置";
                    dr["ID"] = es.FieldID.Trim();
                    dr["CHName"] = es.CHNAME.Trim();
                    dr["IsMust"] = "0";
                    dr["Group"] = es.ITEMTYPE == 7 ? sNumberGrop : es.ITEMTYPE == 6 ? sTimeGrop : es.ITEMTYPE == 4 ? sDropGrop : es.ITEMTYPE == 5 ? sUserGrop : es.ITEMTYPE == 3 ? sDownListGrop : es.ITEMTYPE == 0 ? sLastBaseGroup : es.ITEMTYPE == 2 ? sLastBaseMGroup : sLastGroup;
                    dr["Default"] = (es.ITEMTYPE == 6 ? "0" : es.ITEMTYPE == 7 ? "0" : es.ITEMTYPE == 4 ? "1" : es.ITEMTYPE == 5 ? "1" : es.ITEMTYPE == 3 ? "1" : es.ITEMTYPE == 0 ? "" : es.ITEMTYPE == 2 ? "" : "1");
                    dr["OrderBy"] = "0";
                    dr["isChack"] = "0";//是否时间 
                    dr["IsSelect"] = "0";//是否时间   
                    dr["GroupID"] = es.GROUPID.ToString();
                    dr["GroupName"] = es.GroupName; //所属分类
                    
                    dt.Rows.Add(dr);
                    dgSchema.DataSource = dt.DefaultView;
                    dgSchema.DataBind();
                }
                else
                {
                    if (i == 0)
                    {
                        StrAlert = es.CHNAME;
                    }
                    else
                    {
                        StrAlert += "," + es.CHNAME;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               LoadItemData();
            }

           
        }

       
       
        #region  Bind
       

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

                string strXml = "";
                //strXml = GetSchemaXml(dt);
                string strID = "";
                //保存之前先删除数据
                BR_ExtendsFieldsDP ee = new BR_ExtendsFieldsDP();
                ee.DeleteRecorded();
                foreach (DataRow dr in dt.Rows)
                {
                    DataTable dtxml = CreateNullTable();
                    dtxml.Rows.Add(dr.ItemArray);
                    strXml = GetSchemaXml(dtxml);
                    string GroupName = dr["GroupName"].ToString();
                    decimal GroupID = decimal.Parse(dr["GroupID"].ToString());
                    hidSchemaXml.Value = strXml;
                    strID = BR_ExtendsFieldsDP.Save(hidSchemaXml.Value.Trim(), GroupName, GroupID);

                }


                hidExtendsID.Value = strID;



                //如过从查询分类界面打开，保存后关闭并刷新父窗体
                //if (Request["CloseAction"] != null && Request["CloseAction"].ToString().Equals("1"))
                //{
                //    PageTool.AddJavaScript(this, "window.parent.returnValue = 'refresh';self.close();");
                //}
                //else
                //{
                //    PageTool.AddJavaScript(this, "window.history.go(0);");
                //}
            }
            catch (Exception ee)
            {
                PageTool.MsgBox(this, ee.Message.ToString());
            }
            finally {
                PageTool.MsgBox(this,"数据保存成功！");
            }

        }

        protected void cmdQuery_Click(object sender, System.EventArgs e)
        {
            LoadItemData();

        }

      
        #endregion

        
        protected void ddlType_SelectedIndexChanged1(object sender, EventArgs e)
        {
            LoadItemData();
        }
        

      
    }
}
