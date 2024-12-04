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
    /// frmCatalogEdit ��ժҪ˵����
    /// </summary>
    public partial class frmCatalogEdit : BasePage
    {
        protected System.Web.UI.WebControls.TextBox txtPDeptName;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hidPDeptID;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            // �ڴ˴������û������Գ�ʼ��ҳ��
            CtrTitle.Title = "��������ά��";

            if (!IsPostBack)
            {
                if (this.Request.QueryString["CatalogID"] != null)
                {
                    string sCatalogId = this.Request.QueryString["CatalogID"].ToString();
                    // ��¼��ǰ����
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
            //�ж��Ƿ��г������������Ȩ��
            if (!CheckRight(Constant.CatalogSchema))
            {
                cbtnNew.Visible = false;
            }

            lblCatalogID.Text = hidCatalogID.Value.Trim();

            #region �Ƿ�չʾ��չ���� ����ǰ 2013-04-19
            if (CTools.ToInt(hidIsShowSchema.Value) == 0)
            {
                Table12.Visible = false;
                Table2.Visible = false;
            }
            #endregion

        }

        #region ��ʼ���������Ϣ
        /// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="lngCatalogID"></param>
        private void LoadData(long lngCatalogID)
        {
            CatalogEntity ce = new CatalogEntity(lngCatalogID);

            txtCatalogName.Text = ce.CatalogName;
            txtDesc.Value = ce.Remark;
            hidPCatalogID.Value = ce.ParentID.ToString();
            txtSortID.Text = ce.SortID.ToString();

            if (ce.ParentID == -1)	//(lngCatalogID == 1)//������
            {
                txtPCatalogName.Text = "��";
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

            //�Ƿ���ʾ��չ����
            hidIsShowSchema.Value = ce.IsShowSchema.ToString();
        }
        #endregion

        #region Web ������������ɵĴ���
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: �õ����� ASP.NET Web ���������������ġ�
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
        /// �˷��������ݡ�
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion

        #region ����
        /// <summary>
        /// ����
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
                labMsg.Text = "�������Ʋ���Ϊ��!";
                return;
            }
            else if (ce.ParentID == 0)
            {
                labMsg.Text = "��ѡ���ϼ�����!";
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

                //����session
                hidSchemaXml.Value = strXml;

                ce.ConfigureSchema = strXml;
                ce.IsShowSchema = CTools.ToInt(hidIsShowSchema.Value);


                ce.Save();
                hidCatalogID.Value = ce.CatalogID.ToString();

                //ǿ�Ʒ�����ػ���ʧЧ 
                HttpRuntime.Cache.Insert("EpCacheValidCataLog", false);

                //����Ӳ�ѯ�������򿪣������رղ�ˢ�¸�����
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

        #region ����
        /// <summary>
        /// ����
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
            hidCatalogID.Value = "";//��շ����ʶ
            lblCatalogID.Text = string.Empty;

            hidSchemaXml.Value = string.Empty;
            LoadItemData(0);


            //����Ĭ���ϼ����࣬Ĭ�����ܣ�Ĭ���쵼
            CatalogEntity ce = new CatalogEntity(StringTool.String2Long(sCatalogId));
            hidPCatalogID.Value = sCatalogId == null ? "0" : sCatalogId.Trim();
            txtPCatalogName.Text = ce.CatalogName;
            txtSortID.Text = "-1";

            hidIsShowSchema.Value = ce.IsShowSchema.ToString();
            if (ce.ParentID != -1)
            {
                //��һ��������������һ�� 
                cmdSave.Enabled = true;
            }
        }
        #endregion

        #region ɾ��
        /// <summary>
        /// ɾ��
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

                //ǿ�Ʒ�����ػ���ʧЧ 
                HttpRuntime.Cache.Insert("EpCacheValidCataLog", false);

                lblCatalogID.Text = string.Empty;
                //����Ӳ�ѯ�������򿪣������رղ�ˢ�¸�����
                if (Request["CloseAction"] != null && Request["CloseAction"].ToString().Equals("1"))
                    PageTool.AddJavaScript(this, "window.parent.returnValue = 'refresh';self.close();");
                else
                    PageTool.AddJavaScript(this, "window.parent.contents.location='frmCatalogContent.aspx?CurrCatalogID=1';window.location='about:blank'");
            }
            catch (Exception ee)
            {
                PageTool.MsgBox(this, "ɾ������ʱ���ִ��󣬴���Ϊ��" + ee.Message.ToString());
            }
        }
        #endregion

        #region ����������¼�

        #region �ж��Ƿ�ѡ��
        /// <summary>
        /// �ж��Ƿ�ѡ��
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

        #region ���������ֵ�����Ƿ���ʾ
        /// <summary>
        /// ���������ֵ�����Ƿ���ʾ
        /// </summary>
        /// <param name="type"></param>
        /// <param name="istxt"></param>
        /// <returns></returns>
        protected string GetDefaulControlState(string type, string istxt)
        {
            string style = "display:none";
            if (type == "������Ϣ" && istxt == "1")
            {
                style = "";
            }
            else if (type == "��ע��Ϣ" && istxt == "2")
            {
                style = "";
            }
            else if (type == "��������" && istxt == "0")
            {
                style = "";
            }
            else if (type == "����ѡ��" && istxt == "3")
            {
                style = "";
            }
            else if (type == "������Ϣ" && istxt == "4")
            {
                style = "";
            }
            else if (type == "�û���Ϣ" && istxt == "5")
            {
                style = "";
            }
            else if (type == "��������" && istxt == "6")
            {
                style = "";
            }
            else if (type == "��ֵ����" && istxt == "7")
            {
                style = "";
            }
            return style;
        }
        #endregion

        #region ����DataTable�ṹ
        /// <summary>
        /// ���� datatable�ṹ
        /// </summary>
        /// <returns></returns>
        private DataTable CreateNullTable()
        {
            DataTable dt = new DataTable("Schema");


            dt.Columns.Add("ID");
            dt.Columns.Add("CHName");
            dt.Columns.Add("Default");
            dt.Columns.Add("IsMust");//�Ƿ����
            dt.Columns.Add("Group");
            dt.Columns.Add("TypeName");
            dt.Columns.Add("OrderBy");//����
            dt.Columns.Add("isChack");//�Ƿ�ʱ��            
            return dt;
        }
        #endregion

        #region ��ȡ������grid�� Datatable
        /// <summary>
        /// ��ȡ������grid�� Datatable
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

                    string OrderBy = ((TextBox)row.FindControl("TxtOrderBy")).Text; //����
                    bool isChack = ((CheckBox)row.FindControl("CheckIsTime")).Checked;//�Ƿ�ʱ��                    
                    dr = dt.NewRow();

                    if ((sID.Length > 0 && sCHName.Length > 0) || isAll == true)
                    {
                        dr["TypeName"] = sTypeName;
                        dr["ID"] = sID;
                        dr["CHName"] = sCHName;
                        dr["Group"] = sGroup;
                        dr["IsMust"] = isMustChecked == true ? "1" : "0";//�Ƿ����
                        dr["Default"] = (sTypeName == "��ֵ����" ? (textNumber == "" ? "0" : textNumber) : sTypeName == "��������" ? (blnTimeChecked == true ? "1" : "0") : sTypeName == "�û���Ϣ" ? (blnUserChecked == true ? "1" : "0") : sTypeName == "������Ϣ" ? (blnDeptChecked == true ? "1" : "0") : sTypeName == "����ѡ��" ? sDorpDownListDefault : sTypeName == "������Ϣ" ? stxtDefault : sTypeName == "��ע��Ϣ" ? stxtMDefault : sTypeName == "��ѡ����" ? "" : (blnChecked == true ? "1" : "0"));
                        dr["OrderBy"] = OrderBy == "" ? "0" : OrderBy;//����
                        dr["isChack"] = isChack == true ? "1" : "0";//�Ƿ�ʱ��                        
                        dt.Rows.Add(dr);

                        strGroup = sGroup; //������Ϣ
                    }
                }
            }

            return dt;
        }
        #endregion

        #region ��������ť�����¼�
        /// <summary>
        /// ��������ť�����¼�
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
                    dr["TypeName"] = es.itemType == 7 ? "��ֵ����" : es.itemType == 6 ? "��������" : es.itemType == 4 ? "������Ϣ" : es.itemType == 5 ? "�û���Ϣ" : es.itemType == 3 ? "����ѡ��" : es.itemType == 0 ? "������Ϣ" : es.itemType == 2 ? "��ע��Ϣ" : es.itemType == 8 ? "��ѡ����" : "��������";
                    dr["ID"] = es.FieldID.Trim();
                    dr["CHName"] = es.CHName.Trim();
                    dr["IsMust"] = "0";
                    dr["Group"] = strGroup;
                    dr["Default"] = (es.itemType == 6 ? "0" : es.itemType == 7 ? "0" : es.itemType == 4 ? "1" : es.itemType == 5 ? "1" : es.itemType == 3 ? "1" : es.itemType == 0 ? "" : es.itemType == 2 ? "" : es.itemType == 8 ? "" : "1");
                    dr["OrderBy"] = "0";
                    dr["isChack"] = "0";//�Ƿ�ʱ��                                          
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
                // �رմ���
                sbText.Append("alert('������ " + StrAlert + " �Ѿ�����');");
                sbText.Append("</script>");
                Response.Write(sbText.ToString());
            }
        }
        #endregion

        #region ��ʼ������������Ϣ
        /// <summary>
        /// ��ʼ������������Ϣ
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

        #region ����dt�����������XML��
        /// <summary>
        /// ����dt�����������XML��
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

            //����������Ϣ����
            DataRow[] rows = dt.Select("TypeName not in ('��������','��ע��Ϣ') ", "Group,OrderBy");

            bool blnHasBase = false;
            string sLastBaseGroup = null;  //�п��������յ�����

            string sBaseGroup = "";
            int i = 0;
            foreach (DataRow row in rows)
            {
                sBaseGroup = row["Group"].ToString().Trim();
                if (sBaseGroup != sLastBaseGroup)
                {
                    //��������
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

                if (row["TypeName"].ToString() == "������Ϣ")
                {
                    xmlEle.SetAttribute("Default", row["Default"].ToString().Trim());
                    xmlEle.SetAttribute("CtrlType", "TextBox");
                }
                else if (row["TypeName"].ToString() == "����ѡ��")
                {

                    xmlEle.SetAttribute("Default", row["Default"].ToString().Trim() == "" ? "-1" : row["Default"].ToString().Trim());
                    xmlEle.SetAttribute("CtrlType", "DropDownList");
                }
                else if (row["TypeName"].ToString() == "������Ϣ")
                {
                    xmlEle.SetAttribute("Default", row["Default"].ToString().Trim() == "" ? "-1" : row["Default"].ToString().Trim());
                    xmlEle.SetAttribute("CtrlType", "deptList");
                }
                else if (row["TypeName"].ToString() == "�û���Ϣ")
                {
                    xmlEle.SetAttribute("Default", row["Default"].ToString().Trim() == "" ? "0" : row["Default"].ToString().Trim());
                    xmlEle.SetAttribute("CtrlType", "UserList");
                }
                else if (row["TypeName"].ToString() == "��������")
                {
                    xmlEle.SetAttribute("Default", row["Default"].ToString().Trim());
                    xmlEle.SetAttribute("CtrlType", "Time");
                }
                else if (row["TypeName"].ToString() == "��ֵ����")
                {
                    xmlEle.SetAttribute("Default", row["Default"].ToString().Trim() == "" ? "0" : row["Default"].ToString().Trim());
                    xmlEle.SetAttribute("CtrlType", "Number");
                }
                else if (row["TypeName"].ToString() == "��ѡ����")
                {
                    xmlEle.SetAttribute("Default", row["Default"].ToString().Trim());
                    xmlEle.SetAttribute("CtrlType", "CheckBox");
                }

                xmlEle.SetAttribute("OrderBy", row["OrderBy"].ToString().Trim() == "" ? "0" : row["OrderBy"].ToString().Trim());//����
                xmlEle.SetAttribute("isChack", row["isChack"].ToString().Trim() == "" ? "0" : row["isChack"].ToString().Trim());//�Ƿ�ʱ��                  



                xmlGroup.AppendChild(xmlEle);
            }

            //�����������ò���
            rows = dt.Select("TypeName='��������'", "Group,OrderBy");
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
                    //��������
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
                xmlEle.SetAttribute("isChack", row["isChack"].ToString().Trim() == "" ? "0" : row["isChack"].ToString().Trim());//�Ƿ�ʱ��                      
                xmlGroup.AppendChild(xmlEle);
            }

            rows = dt.Select("TypeName='��ע��Ϣ'", "Group,OrderBy");
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
                    //��������
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
                xmlEle.SetAttribute("isChack", row["isChack"].ToString().Trim() == "" ? "0" : row["isChack"].ToString().Trim());//�Ƿ�ʱ��                  

                xmlGroup.AppendChild(xmlEle);
            }

            return xmlDoc.InnerXml;

        }
        #endregion

        #region ��ӱ�ע�����Ϣ
        /// <summary>
        /// ��ӱ�ע�����Ϣ
        /// </summary>
        /// <param name="relnode"></param>
        /// <param name="dt"></param>
        private void AddRelRemarkDatas(XmlNode relnode, ref DataTable dt)
        {
            XmlNodeList ns = null;

            string strGroup = "";
            string strTypeName = "��ע��Ϣ";

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

        #region ��ӹ������������Ϣ
        /// <summary>
        /// ��ӹ������������Ϣ
        /// </summary>
        /// <param name="relnode"></param>
        /// <param name="dt"></param>
        private void AddRelSchemeDatas(XmlNode relnode, ref DataTable dt)
        {
            XmlNodeList ns = null;

            string strGroup = "";
            string strTypeName = "��������";

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

        #region ��ӻ��������Ϣ
        /// <summary>
        /// ��ӻ��������Ϣ
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
                        values[5] = (object)"������Ϣ";
                    }
                    else if (n.Attributes["CtrlType"].Value == "DropDownList")
                    {
                        values[5] = (object)"����ѡ��";
                    }
                    else if (n.Attributes["CtrlType"].Value == "deptList")
                    {
                        values[5] = (object)"������Ϣ";
                    }
                    else if (n.Attributes["CtrlType"].Value == "UserList")
                    {
                        values[5] = (object)"�û���Ϣ";
                    }
                    else if (n.Attributes["CtrlType"].Value == "Time")
                    {
                        values[5] = (object)"��������";
                    }
                    else if (n.Attributes["CtrlType"].Value == "Number")
                    {
                        values[5] = (object)"��ֵ����";
                    }
                    else if (n.Attributes["CtrlType"].Value == "CheckBox")
                    {
                        values[5] = (object)"��ѡ����";
                    }
                    else
                    {
                        values[5] = (object)"������Ϣ";
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
                if (list.SelectedItem.Text == "��������" || list.SelectedItem.Text == "��ѡ����")
                {
                    CheckBox chkbox = ((CheckBox)e.Item.FindControl("chkIsMust"));
                    chkbox.Visible = false;
                }
                else if (list.SelectedItem.Text == "����ѡ��")
                {
                    ctrFlowCataDropList DropList = ((ctrFlowCataDropList)e.Item.FindControl("ctrFlowCataDropDefault"));
                    string FieldID = DataBinder.Eval(e.Item.DataItem, "ID").ToString();
                    string Defaultvalue = e.Item.Cells[8].Text.ToString() == "" ? "0" : e.Item.Cells[8].Text.ToString();//��ʼĬ��ֵ

                    //��ȡ��Ӧ��rootid
                    BR_CatalogSchemaItemsDP ee = new BR_CatalogSchemaItemsDP();
                    ee = ee.GetReCorded(FieldID);
                    DropList.RootID = long.Parse(ee.CatalogID.ToString() == "" ? "1" : ee.CatalogID.ToString());
                    DropList.CatelogID = long.Parse(Defaultvalue);
                }

            }
        }
        #endregion

        #region ���Ȩ�� CheckRight
        /// <summary>
        /// ���Ȩ��
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
