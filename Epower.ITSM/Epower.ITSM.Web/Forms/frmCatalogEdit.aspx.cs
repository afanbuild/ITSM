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
using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;
using EpowerGlobal;
using System.IO;
using EpowerCom;
using Epower.ITSM.Base;
using System.Xml;
using Epower.ITSM.Web.Controls;
using System.Text;
using Epower.DevBase.Organization.SqlDAL;




namespace Epower.ITSM.Web.Forms
{
    /// <summary>
    /// frmCatalogEdit 的摘要说明。
    /// </summary>
    public partial class frmCatalogEdit : BasePage
    {
        protected System.Web.UI.WebControls.TextBox txtPDeptName;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hidPDeptID;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            // 在此处放置用户代码以初始化页面
            CtrTitle.Title = "分类资料维护";

            if (!IsPostBack)
            {
                if (this.Request.QueryString["CatalogID"] != null)
                {
                    string sCatalogId = this.Request.QueryString["CatalogID"].ToString();
                    // 记录当前分类
                    Session["OldCatalogID"] = long.Parse(sCatalogId);

                    hidCatalogID.Value = sCatalogId;
                    LoadData(StringTool.String2Long(sCatalogId));
                }

                hidSchemaXml.Value = string.Empty;
                if (hidCatalogID.Value.Trim() != string.Empty)
                {
                    LoadItemData(long.Parse(hidCatalogID.Value.Trim()));
                }
            }
            //判断是否有常用类别配置项权限
            if (!CheckRight(Constant.CatalogSchema))
            {
                cbtnNew.Visible = false;
            }

            lblCatalogID.Text = hidCatalogID.Value.Trim();

            #region 是否展示扩展功能 余向前 2013-04-19
            if (CTools.ToInt(hidIsShowSchema.Value) == 0)
            {
                Table12.Visible = false;
                Table2.Visible = false;
            }
            #endregion

        }

        #region 初始加载类别信息
        /// <summary>
        /// 初始加载类别信息
        /// </summary>
        /// <param name="lngCatalogID"></param>
        private void LoadData(long lngCatalogID)
        {
            CatalogEntity ce = new CatalogEntity(lngCatalogID);

            txtCatalogName.Text = ce.CatalogName;
            txtDesc.Value = ce.Remark;
            hidPCatalogID.Value = ce.ParentID.ToString();
            txtSortID.Text = ce.SortID.ToString();

            if (ce.ParentID == -1)	//(lngCatalogID == 1)//根分类
            {
                txtPCatalogName.Text = "无";
            }

            if (ce.ParentID == 1)
            {
                cmdSave.Enabled = false;
                cmdDelete.Enabled = false;
            }
            else if (ce.ParentID == -1)
            {
                cmdAdd.Enabled = false;
                cmdSave.Enabled = false;
                cmdDelete.Enabled = false;
            }
            txtPCatalogName.Text = CatalogDP.GetCatalogName(StringTool.String2Long(hidPCatalogID.Value));

            //是否显示扩展功能
            hidIsShowSchema.Value = ce.IsShowSchema.ToString();
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

        }
        #endregion

        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdSave_Click(object sender, System.EventArgs e)
        {
            CatalogEntity ce = new CatalogEntity(StringTool.String2Long(hidCatalogID.Value));

            ce.CatalogName = txtCatalogName.Text;
            ce.Remark = txtDesc.Value;

            ce.ParentID = StringTool.String2Long(hidPCatalogID.Value);
            ce.CatalogID = StringTool.String2Long(hidCatalogID.Value);
            ce.SortID = StringTool.String2Int(txtSortID.Text);

            if (ce.CatalogID <= 0)
                ce.CatalogID = EpowerGlobal.EPGlobal.GetNextID("Catalog_ID");

            if (Session["UserOrgID"] != null)
            {
                ce.OrgID = long.Parse(Session["UserOrgID"].ToString());
            }
            else
            {
                ce.OrgID = -1;
            }

            if (ce.CatalogName.Trim() == "")
            {
                labMsg.Text = "分类名称不能为空!";
                return;
            }
            else if (ce.ParentID == 0)
            {
                labMsg.Text = "请选择上级分类!";
                return;
            }
            else
            {
                labMsg.Text = "";
            }

            try
            {
                string strGroup = "";
                DataTable dt = GetDetailItem(false, ref strGroup);
                string strXml = GetSchemaXml(dt);

                //保存session
                hidSchemaXml.Value = strXml;

                ce.ConfigureSchema = strXml;
                ce.IsShowSchema = CTools.ToInt(hidIsShowSchema.Value);


                ce.Save();
                hidCatalogID.Value = ce.CatalogID.ToString();

                //强制分类相关缓存失效 
                HttpRuntime.Cache.Insert("EpCacheValidCataLog", false);

                //如过从查询分类界面打开，保存后关闭并刷新父窗体
                if (Request["CloseAction"] != null && Request["CloseAction"].ToString().Equals("1"))
                {
                    PageTool.AddJavaScript(this, "window.parent.returnValue = 'refresh';self.close();");
                }
                else
                {
                    PageTool.AddJavaScript(this, "window.parent.contents.location='frmCatalogContent.aspx'");
                    PageTool.AddJavaScript(this, "window.parent.cataloginfo.location='frmCatalogedit.aspx?Catalogid=" + ce.CatalogID.ToString() + "&CatalogText=" + txtCatalogName.Text + "'");
                }
            }
            catch (Exception ee)
            {
                PageTool.MsgBox(this, ee.Message.ToString());
            }

        }
        #endregion

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdAdd_Click(object sender, System.EventArgs e)
        {
            string sCatalogId = this.Request.QueryString["CatalogID"].ToString();
            //string sCatalogText=this.Request.QueryString["CatalogText"].ToString();				

            txtCatalogName.Text = "";
            txtDesc.Value = "";
            hidPCatalogID.Value = "";
            hidCatalogID.Value = "";//清空分类标识
            lblCatalogID.Text = string.Empty;

            hidSchemaXml.Value = string.Empty;
            LoadItemData(0);


            //加载默认上级分类，默认主管，默认领导
            CatalogEntity ce = new CatalogEntity(StringTool.String2Long(sCatalogId));
            hidPCatalogID.Value = sCatalogId == null ? "0" : sCatalogId.Trim();
            txtPCatalogName.Text = ce.CatalogName;
            txtSortID.Text = "-1";

            hidIsShowSchema.Value = ce.IsShowSchema.ToString();
            if (ce.ParentID != -1)
            {
                //第一级，可以新增下一级 
                cmdSave.Enabled = true;
            }
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdDelete_Click(object sender, System.EventArgs e)
        {
            CatalogEntity ce = new CatalogEntity();
            ce.CatalogID = StringTool.String2Long(hidCatalogID.Value);
            try
            {
                ce.Delete();

                //强制分类相关缓存失效 
                HttpRuntime.Cache.Insert("EpCacheValidCataLog", false);

                lblCatalogID.Text = string.Empty;
                //如过从查询分类界面打开，保存后关闭并刷新父窗体
                if (Request["CloseAction"] != null && Request["CloseAction"].ToString().Equals("1"))
                    PageTool.AddJavaScript(this, "window.parent.returnValue = 'refresh';self.close();");
                else
                    PageTool.AddJavaScript(this, "window.parent.contents.location='frmCatalogContent.aspx?CurrCatalogID=1';window.location='about:blank'");
            }
            catch (Exception ee)
            {
                PageTool.MsgBox(this, "删除分类时出现错误，错误为：" + ee.Message.ToString());
            }
        }
        #endregion

        #region 配置项相关事件

        #region 判断是否选择
        /// <summary>
        /// 判断是否选择
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
        #endregion

        #region 根据类别与值控制是否显示
        /// <summary>
        /// 根据类别与值控制是否显示
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
        #endregion

        #region 创建DataTable结构
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
            return dt;
        }
        #endregion

        #region 获取配置项grid的 Datatable
        /// <summary>
        /// 获取配置项grid的 Datatable
        /// </summary>
        /// <param name="isAll"></param>
        /// <param name="strGroup"></param>
        /// <returns></returns>
        private DataTable GetDetailItem(bool isAll, ref string strGroup)
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
                    dr = dt.NewRow();

                    if ((sID.Length > 0 && sCHName.Length > 0) || isAll == true)
                    {
                        dr["TypeName"] = sTypeName;
                        dr["ID"] = sID;
                        dr["CHName"] = sCHName;
                        dr["Group"] = sGroup;
                        dr["IsMust"] = isMustChecked == true ? "1" : "0";//是否必填
                        dr["Default"] = (sTypeName == "数值类型" ? (textNumber == "" ? "0" : textNumber) : sTypeName == "日期类型" ? (blnTimeChecked == true ? "1" : "0") : sTypeName == "用户信息" ? (blnUserChecked == true ? "1" : "0") : sTypeName == "部门信息" ? (blnDeptChecked == true ? "1" : "0") : sTypeName == "下拉选择" ? sDorpDownListDefault : sTypeName == "基础信息" ? stxtDefault : sTypeName == "备注信息" ? stxtMDefault : sTypeName == "复选类型" ? "" : (blnChecked == true ? "1" : "0"));
                        dr["OrderBy"] = OrderBy == "" ? "0" : OrderBy;//排序
                        dr["isChack"] = isChack == true ? "1" : "0";//是否时间                        
                        dt.Rows.Add(dr);

                        strGroup = sGroup; //分组信息
                    }
                }
            }

            return dt;
        }
        #endregion

        #region 添加配置项按钮单击事件
        /// <summary>
        /// 添加配置项按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddNewItem_Click(object sender, EventArgs e)
        {
            string strGroup = "";
            string sNewFieldID = "";

            string sArrValue = hidTempID.Value.Trim();
            string[] arrListValue = sArrValue.Split(',');

            string StrAlert = string.Empty;
            int i = 0;
            foreach (string returnVlue in arrListValue)
            {


                sNewFieldID = returnVlue;

                DataTable dt = GetDetailItem(false, ref strGroup);

                DataRow[] rows = dt.Select("ID='" + sNewFieldID.Trim() + "'");
                BR_CatalogSchemaItemsDP es = new BR_CatalogSchemaItemsDP();
                es = es.GetReCorded(sNewFieldID);

                if (rows.Length == 0 && es.ID != 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["TypeName"] = es.itemType == 7 ? "数值类型" : es.itemType == 6 ? "日期类型" : es.itemType == 4 ? "部门信息" : es.itemType == 5 ? "用户信息" : es.itemType == 3 ? "下拉选择" : es.itemType == 0 ? "基础信息" : es.itemType == 2 ? "备注信息" : es.itemType == 8 ? "复选类型" : "关联配置";
                    dr["ID"] = es.FieldID.Trim();
                    dr["CHName"] = es.CHName.Trim();
                    dr["IsMust"] = "0";
                    dr["Group"] = strGroup;
                    dr["Default"] = (es.itemType == 6 ? "0" : es.itemType == 7 ? "0" : es.itemType == 4 ? "1" : es.itemType == 5 ? "1" : es.itemType == 3 ? "1" : es.itemType == 0 ? "" : es.itemType == 2 ? "" : es.itemType == 8 ? "" : "1");
                    dr["OrderBy"] = "0";
                    dr["isChack"] = "0";//是否时间                                          
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
                sbText.Append("</script>");
                Response.Write(sbText.ToString());
            }
        }
        #endregion

        #region 初始加载配置项信息
        /// <summary>
        /// 初始加载配置项信息
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
                strXmlTemp = CatalogDP.GetCatalogSchema(lngCategoryID);
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
        #endregion

        #region 根据dt返回配置项的XML串
        /// <summary>
        /// 根据dt返回配置项的XML串
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


            xmlDoc.DocumentElement.SetAttribute("Title", txtCatalogName.Text.Trim());

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
                else if (row["TypeName"].ToString() == "复选类型")
                {
                    xmlEle.SetAttribute("Default", row["Default"].ToString().Trim());
                    xmlEle.SetAttribute("CtrlType", "CheckBox");
                }

                xmlEle.SetAttribute("OrderBy", row["OrderBy"].ToString().Trim() == "" ? "0" : row["OrderBy"].ToString().Trim());//排序
                xmlEle.SetAttribute("isChack", row["isChack"].ToString().Trim() == "" ? "0" : row["isChack"].ToString().Trim());//是否时间                  



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

                xmlGroup.AppendChild(xmlEle);
            }

            return xmlDoc.InnerXml;

        }
        #endregion

        #region 添加备注类别信息
        /// <summary>
        /// 添加备注类别信息
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
            object[] values = new object[8];
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
                dt.Rows.Add(values);

            }
        }
        #endregion

        #region 添加关联配置类别信息
        /// <summary>
        /// 添加关联配置类别信息
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
            object[] values = new object[8];
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
                dt.Rows.Add(values);

            }
        }
        #endregion

        #region 添加基础类别信息
        /// <summary>
        /// 添加基础类别信息
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
                object[] values = new object[8];

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
                    else if (n.Attributes["CtrlType"].Value == "CheckBox")
                    {
                        values[5] = (object)"复选类型";
                    }
                    else
                    {
                        values[5] = (object)"基础信息";
                    }
                    values[6] = (object)n.Attributes["OrderBy"].Value;
                    values[7] = (object)n.Attributes["isChack"].Value;
                    dt.Rows.Add(values);
                }
            }
        }
        #endregion

        #endregion

        #region  dgPro_ProvideManage_ItemCommand
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgPro_ProvideManage_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            string strGroup = "";
            DataTable dt = GetDetailItem(true, ref strGroup);
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
                if (list.SelectedItem.Text == "关联配置" || list.SelectedItem.Text == "复选类型")
                {
                    CheckBox chkbox = ((CheckBox)e.Item.FindControl("chkIsMust"));
                    chkbox.Visible = false;
                }
                else if (list.SelectedItem.Text == "下拉选择")
                {
                    ctrFlowCataDropList DropList = ((ctrFlowCataDropList)e.Item.FindControl("ctrFlowCataDropDefault"));
                    string FieldID = DataBinder.Eval(e.Item.DataItem, "ID").ToString();
                    string Defaultvalue = e.Item.Cells[8].Text.ToString() == "" ? "0" : e.Item.Cells[8].Text.ToString();//初始默认值

                    //获取对应的rootid
                    BR_CatalogSchemaItemsDP ee = new BR_CatalogSchemaItemsDP();
                    ee = ee.GetReCorded(FieldID);
                    DropList.RootID = long.Parse(ee.CatalogID.ToString() == "" ? "1" : ee.CatalogID.ToString());
                    DropList.CatelogID = long.Parse(Defaultvalue);
                }

            }
        }
        #endregion

        #region 检查权限 CheckRight
        /// <summary>
        /// 检查权限
        /// </summary>
        /// <param name="OperatorID"></param>
        /// <returns></returns>
        private bool CheckRight(long OperatorID)
        {
            RightEntity re = (RightEntity)((Hashtable)Session["UserAllRights"])[OperatorID];
            if (re == null)
                return false;
            else
                return re.CanAdd;
        }
        #endregion
    }
}
