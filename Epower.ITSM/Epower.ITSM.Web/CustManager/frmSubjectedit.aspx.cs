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

namespace Epower.ITSM.Web.CustManager
{
    public partial class frmSubjectedit : BasePage
    {

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.IssueShortCutReqTemplate;
            this.Master.IsCheckRight = true;
            if (this.Request.QueryString["id"] != null)
            {
                this.Master.MainID = this.Request.QueryString["id"].ToString();
            }
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);
            this.Master.Master_Button_Save_Click += new Global_BtnClick(Master_Master_Button_Save_Click);
            this.Master.Master_Button_GoHistory_Click += new Global_BtnClick(Master_Master_Button_GoHistory_Click);
            this.Master.ShowAddPageButton();
            Master.ShowNewButton(false);
            Master.ShowDeleteButton(false);
            Master.ShowBackUrlButton(false);
        }
        #endregion


        #region Master_Master_Button_New_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("frmBr_SchemaItemsEdit.aspx");
        }
        #endregion

        #region Master_Master_Button_Delete_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            if (!String.IsNullOrEmpty(this.Master.MainID.ToString()))
            {
                Equ_SchemaItemsDP ee = new Equ_SchemaItemsDP();
                ee.DeleteRecorded(long.Parse(this.Master.MainID.Trim()));

                //强制相关缓存失效 
                HttpRuntime.Cache.Insert("CommCacheValidBrSchemaItem", false);

                Master_Master_Button_GoHistory_Click();
            }
        }
        #endregion

        #region Master_Master_Button_GoHistory_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_GoHistory_Click()
        {
            Response.Redirect("frmBr_SchemaItemsMain.aspx");
        }
        #endregion

        #region Master_Master_Button_Save_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Save_Click()
        {
            try
            {
                string sLastGroup = "";
                string sLastBaseGroup = "";
                string sLastBaseMGroup = "";
                DataTable dt = GetDetailItem(false, ref sLastBaseGroup, ref sLastGroup, ref sLastBaseMGroup);
                string strXml = GetSchemaXml(dt);

                //保存session
                hidSchemaXml.Value = strXml;
                string strSubjectID = Br_SubjectDP.Save(StringTool.String2Long(hidCatalogID.Value), hidSchemaXml.Value.Trim());
                hidCatalogID.Value = strSubjectID;

                //强制分类相关缓存失效 
                HttpRuntime.Cache.Insert("CommCacheValidBrCategory", false);
            }
            catch (Exception ee)
            {
                Master.IsSaveSuccess = false;
                PageTool.MsgBox(this, ee.Message.ToString());
            }
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
            dt.Columns.Add("Group");
            dt.Columns.Add("TypeName");

            return dt;
        }

        /// <summary>
        /// 获取表单grid 的 datatabel
        /// </summary>
        /// <param name="sLastBaseGroup"></param>
        /// <param name="sLastGroup"></param>
        /// <returns></returns>
        private DataTable GetDetailItem(bool isAll, ref string sLastBaseGroup, ref string sLastGroup, ref string sLastBaseMGroup)
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
                    bool blnChecked = ((CheckBox)row.FindControl("chkDefault")).Checked;
                    string sGroup = ((TextBox)row.FindControl("txtGroup")).Text;



                    dr = dt.NewRow();

                    if ((sID.Length > 0 && sCHName.Length > 0) || isAll == true)
                    {
                        dr["TypeName"] = sTypeName;
                        dr["ID"] = sID;
                        dr["CHName"] = sCHName;
                        dr["Group"] = sGroup;
                        dr["Default"] = (sTypeName == "基础信息" ? stxtDefault : sTypeName == "备注信息" ? stxtMDefault : (blnChecked == true ? "1" : "0"));
                        dt.Rows.Add(dr);

                        if (sTypeName == "基础信息")
                        {
                            sLastBaseGroup = sGroup;
                        }
                        else if (sTypeName == "备注信息")
                        {
                            sLastBaseMGroup = sGroup;
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
                strXmlTemp = Br_SubjectDP.GetCatalogSchema(lngCategoryID);
            }
            DataTable dt = CreateNullTable();

            if (strXmlTemp != "")
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(strXmlTemp);

                //if (xmldoc.DocumentElement.Attributes["Title"] != null)
                //    txtName.Text = xmldoc.DocumentElement.Attributes["Title"].Value;

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


            xmlDoc.DocumentElement.SetAttribute("Title", "客户信息扩展");

            //创建基础信息部分
            DataRow[] rows = dt.Select("TypeName='基础信息'", "Group");
            bool blnHasBase = false;
            string sLastBaseGroup = null;  //有可能遇到空的组名

            string sBaseGroup = "";
            int i = 0;
            foreach (DataRow row in rows)
            {
                i++;
                if (blnHasBase == false)
                {
                    //创建基础信息根

                    xmlRoot = xmlDoc.CreateElement("Base");

                    blnHasBase = true;
                }

                sBaseGroup = row["Group"].ToString().Trim();

                if (sBaseGroup != sLastBaseGroup)
                {
                    //创建组结点

                    xmlGroup = xmlDoc.CreateElement("BaseItem");
                    xmlGroup.SetAttribute("Title", sBaseGroup);
                    sLastBaseGroup = sBaseGroup;
                    xmlRoot.AppendChild(xmlGroup);
                }
                xmlEle = xmlDoc.CreateElement("AttributeItem");
                xmlEle.SetAttribute("ID", row["ID"].ToString().Trim());
                xmlEle.SetAttribute("Type", "varchar");
                xmlEle.SetAttribute("CHName", row["CHName"].ToString().Trim());
                xmlEle.SetAttribute("Default", row["Default"].ToString().Trim());
                xmlEle.SetAttribute("CtrlType", "TextBox");
                xmlGroup.AppendChild(xmlEle);
            }
            if (i > 0)
            {
                xmlDoc.DocumentElement.AppendChild(xmlRoot);
            }
            //创建关联配置部分
            rows = dt.Select("TypeName='关联配置'", "Group");
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
                xmlEle.SetAttribute("CHName", row["CHName"].ToString().Trim());
                xmlEle.SetAttribute("Default", row["Default"].ToString().Trim());

                xmlGroup.AppendChild(xmlEle);
            }
            rows = dt.Select("TypeName='备注信息'", "Group");
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
                xmlEle.SetAttribute("Default", row["Default"].ToString().Trim());
                xmlEle.SetAttribute("CtrlType", "MultiLine");
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
            object[] values = new object[5];
            foreach (XmlNode n in ns)
            {
                values[0] = (object)n.Attributes["ID"].Value;
                values[1] = (object)n.Attributes["CHName"].Value;
                values[2] = (object)n.Attributes["Default"].Value;
                values[3] = (object)strGroup;
                values[4] = (object)strTypeName;
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
            object[] values = new object[5];
            foreach (XmlNode n in ns)
            {
                values[0] = (object)n.Attributes["ID"].Value;
                values[1] = (object)n.Attributes["CHName"].Value;
                values[2] = (object)n.Attributes["Default"].Value;
                values[3] = (object)strGroup;
                values[4] = (object)strTypeName;
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
            XmlNodeList bnodes = xmldoc.SelectNodes("EquScheme/Base/BaseItem");
            string strGroup = "";
            string strTypeName = "基础信息";
            foreach (XmlNode node in bnodes)
            {
                strGroup = "";

                if (node.Attributes["Title"] != null)
                {
                    strGroup = node.Attributes["Title"].Value;
                }
                object[] values = new object[5];

                XmlNodeList ns = node.SelectNodes("AttributeItem");

                foreach (XmlNode n in ns)
                {
                    values[0] = (object)n.Attributes["ID"].Value;
                    values[1] = (object)n.Attributes["CHName"].Value;
                    values[2] = (object)n.Attributes["Default"].Value;
                    values[3] = (object)strGroup;
                    values[4] = (object)strTypeName;
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
            DataTable dt = GetDetailItem(true, ref s1, ref s2, ref s3);
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
                if (e.Item.Cells[1].Text.Trim() == "关联配置")
                {
                    if (e.Item.Cells[4].Text.Trim() == "1")
                    {
                        e.Item.Cells[4].Text = "配置";
                    }
                    else
                    {
                        e.Item.Cells[4].Text = "";
                    }

                    for (int i = 0; i < e.Item.Cells.Count; i++)
                    {

                        e.Item.Cells[i].BackColor = System.Drawing.Color.Honeydew;
                    }
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
            string sNewFieldID = "";


            string sArrValue=hidTempID.Value;
            string[] arrListValue=sArrValue.Split(',');

            string StrAlert = string.Empty;
            int i = 0;
            foreach (string returnVlue in arrListValue)
            {
                sNewFieldID = returnVlue;
                DataTable dt = GetDetailItem(false, ref sLastBaseGroup, ref sLastGroup, ref sLastBaseMGroup);
                DataRow[] rows = dt.Select("ID='" + sNewFieldID.Trim() + "'");
                Br_SchemaItemsDP es = new Br_SchemaItemsDP();
                es = es.GetReCorded(sNewFieldID);
                if (rows.Length == 0 && es.ID != 0)
                {
                    DataRow dr = dt.NewRow();

                    dr["TypeName"] = es.itemType == 0 ? "基础信息" : es.itemType == 2 ? "备注信息" : "关联配置";
                    dr["ID"] = es.FieldID.Trim();
                    dr["CHName"] = es.CHName.Trim();
                    dr["Group"] = es.itemType == 0 ? sLastBaseGroup : es.itemType == 2 ? sLastBaseMGroup : sLastGroup;
                    dr["Default"] = (es.itemType == 0 ? "" : es.itemType == 2 ? "" : "1");
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
                sbText.Append("alert('扩展项 " + StrAlert + " 已经存在');");
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
            SetParentButtonEvent();
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngCatalogID"></param>
        private void LoadData()
        {
            DataTable dt = Br_SubjectDP.GetSubject();
            if (dt.Rows.Count > 0)
            {
                hidSchemaXml.Value = dt.Rows[0]["ConfigureSchema"].ToString();
                LoadItemData(long.Parse(dt.Rows[0]["CatalogID"].ToString()));
                hidCatalogID.Value = dt.Rows[0]["CatalogID"].ToString();
            }
            else
            {
                hidSchemaXml.Value = "";
                LoadItemData(0);
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

        

    }
}
