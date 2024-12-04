/****************************************************************************
 * 
 * description:配置信息设置主表单
 * 
 * 
 * 
 * Create by:
 * Create Date:2008-04-02
 * *************************************************************************/
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;

using Epower.ITSM.SqlDAL;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frmEquSchemaMain : BasePage
    {
       

       

       
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

        protected string GetDefaulControlState(string type, string istxt)
        {
            if (type == "基础信息")
            {
                if (istxt == "1")
                {
                    return "";
                }
                else
                {
                    return "display:none";
                }
            }
            else
            {
                if (istxt == "1")
                {
                    return "display:none";
                }
                else
                {
                    return "";
                }
            }
        }

       

       
        #region Page_Load
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            long lngCategoryID = 0;

            

            string bjcFather = "true";
            if (Request.QueryString["jcFather"] != null)
            {
                bjcFather = Request.QueryString["jcFather"];
            }

            if (Request.QueryString["CurrSubjectID"] != "" && Request.QueryString["CurrSubjectID"] != "0" && bjcFather!="true")
            {
                //存在当前ID,并且当前情况为非继承关系时,才取当前的ID
                lngCategoryID = long.Parse(Request.QueryString["CurrSubjectID"]);
            }
            else
            {
                //新增时自动取父级配置
                 if (Request.QueryString["PSubjectID"] != null)
                     lngCategoryID = long.Parse(Request.QueryString["PSubjectID"]);
            }
            if (bjcFather.ToLower() == "true")
            {
                cmdOK.Visible = false;
                btnAddNewItem.Visible = false;
                cbtnAdd.Visible = false;

                dgSchema.Columns[5].Visible = false;

            }

            // = 参数
            if (!IsPostBack)
            {
                string sHasChangeParent = "false";
                if (Request.QueryString["ChangeParent"] != null)
                {
                    sHasChangeParent = Request.QueryString["ChangeParent"];
                    if (sHasChangeParent == "true")
                    {
                        if (Session["EquSchemaMainUnSaved"] != null)
                        {
                            //移除保存过的信息
                            Session.Remove("EquSchemaMainUnSaved");
                        }
                    }
                }

               LoadData(lngCategoryID);
               
            }
        }
        #endregion

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
        private DataTable GetDetailItem(bool isAll, ref string sLastBaseGroup, ref string sLastGroup)
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
                    bool blnChecked = ((CheckBox)row.FindControl("chkDefault")).Checked;
                    string sGroup = ((TextBox)row.FindControl("txtGroup")).Text;

                    

                    dr = dt.NewRow();

                    if ((sID.Length > 0 && sCHName.Length > 0) || isAll == true)
                    {
                        dr["TypeName"] = sTypeName;
                        dr["ID"] = sID;
                        dr["CHName"] = sCHName;
                        dr["Group"] = sGroup;
                        dr["Default"] = (sTypeName == "基础信息" ? stxtDefault : (blnChecked==true?"1":"0"));
                        dt.Rows.Add(dr);

                        if (sTypeName == "基础信息")
                        {
                            sLastBaseGroup = sGroup;
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
        private void LoadData(long lngCategoryID)
        {
            string strXmlTemp = "";
            if (Session["EquSchemaMainUnSaved"] != null)
            {
                //之前编辑过的为主
                strXmlTemp = Session["EquSchemaMainUnSaved"].ToString();
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

                if(xmldoc.DocumentElement.Attributes["Title"] != null)
                    txtName.Text = xmldoc.DocumentElement.Attributes["Title"].Value;

                AddBaseSchemeDatas(xmldoc, ref dt);

                XmlNodeList relnodes = xmldoc.SelectNodes("EquScheme/RelationConfig");

                foreach (XmlNode relnode in relnodes)
                {
                    AddRelSchemeDatas(relnode, ref dt);
                }
            }

            dgSchema.DataSource = dt.DefaultView;
            dgSchema.DataBind();
        }

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

            xmlDoc.DocumentElement.SetAttribute("Title", txtName.Text.Trim());

            //创建基础信息部分
            DataRow[] rows = dt.Select("TypeName='基础信息'","Group");
            bool blnHasBase = false;
            string sLastBaseGroup = null;  //有可能遇到空的组名
            string sBaseGroup = "";
            int i = 0;
            foreach(DataRow row in rows)
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
       
            

            
            return xmlDoc.InnerXml;

        }

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

        private void AddBaseSchemeDatas(XmlDocument xmldoc,ref DataTable dt)
        {
            
            XmlNodeList bnodes = xmldoc.SelectNodes("EquScheme/Base/BaseItem");
            string strGroup = "";
            string strTypeName = "基础信息";
            foreach (XmlNode node in bnodes)
            {
                 strGroup = "";

                if (node.Attributes["Title"] != null)
                {
                    strGroup  =  node.Attributes["Title"].Value ;
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
            DataTable dt = GetDetailItem(true,ref s1,ref s2);
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
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if ( i < 5)
                    {
                        int j = i ;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
        #endregion

        protected void btnAddNewItem_Click(object sender, EventArgs e)
        {
            string sLastGroup = "";
            string sLastBaseGroup = "";
            string sNewFieldID = "";

            sNewFieldID = hidTempID.Value.Trim();

            DataTable dt = GetDetailItem(false, ref sLastBaseGroup, ref sLastGroup);

            DataRow[] rows = dt.Select("ID='" + sNewFieldID.Trim() + "'");
            Equ_SchemaItemsDP es = new Equ_SchemaItemsDP();
            es = es.GetReCorded(sNewFieldID);

            if (rows.Length == 0 && es.ID != 0)
            {
                

                DataRow dr = dt.NewRow();

                dr["TypeName"] = es.itemType == 0 ? "基础信息" : "关联配置";
                dr["ID"] = es.FieldID.Trim();
                dr["CHName"] = es.CHName.Trim();
                dr["Group"] = es.itemType == 0 ? sLastBaseGroup : sLastGroup;
                dr["Default"] = (es.itemType == 0 ? "" : "1");
                dt.Rows.Add(dr);

                dgSchema.DataSource = dt.DefaultView;
                dgSchema.DataBind();
            }
            else
            {
                StringBuilder sbText = new StringBuilder();
                sbText.Append("<script>");
              
                // 关闭窗口
                sbText.Append("alert('配置项已经存在');"); 
                //sbText.Append("alertExist();"); 
                sbText.Append("</script>");
                Response.Write(sbText.ToString());
            }
        }

        protected void cmdOK_Click(object sender, EventArgs e)
        {
            string sLastGroup = "";
            string sLastBaseGroup = "";
            DataTable dt = GetDetailItem(false,ref sLastBaseGroup,  ref sLastGroup);
            string strXml = GetSchemaXml(dt);

            //保存session
            Session["EquSchemaMainUnSaved"] = strXml;

            Response.Write("<script>window.returnValue ='" + strXml +"';window.close();</script>");
            //window.returnValue = document.all.hidCatalogID.value;
            //window.close();
        }

       

      
    }
}

