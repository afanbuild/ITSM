/****************************************************************************
 * 
 * description:�豸������ҳ�����
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
    /// frmSubjectedit ��ժҪ˵����
    /// </summary>
    public partial class frmSubjectedit : BasePage
    {
        #region ����
        string sSubjectId = string.Empty;               //�ʲ����ID
        #endregion

        #region  ����������
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
            dt.Columns.Add("IsSelect");//�Ƿ�����Ϊ��ѯ����
            return dt;
        }

        /// <summary>
        /// ��ȡ��grid �� datatabel
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

                    string OrderBy = ((TextBox)row.FindControl("TxtOrderBy")).Text; //����
                    bool isChack = ((CheckBox)row.FindControl("CheckIsTime")).Checked;//�Ƿ�ʱ��
                    bool IsSelect = ((CheckBox)row.FindControl("chkIsSelect")).Checked;//�Ƿ��ѯ����
                    dr = dt.NewRow();

                    if ((sID.Length > 0 && sCHName.Length > 0) || isAll == true)
                    {
                        dr["TypeName"] = sTypeName;
                        dr["ID"] = sID;
                        dr["CHName"] = sCHName;
                        dr["Group"] = sGroup;
                        dr["IsMust"] = isMustChecked == true ? "1" : "0";//�Ƿ����
                        dr["Default"] = (sTypeName == "��ֵ����" ? (textNumber == "" ? "0" : textNumber) : sTypeName == "��������" ? (blnTimeChecked == true ? "1" : "0") : sTypeName == "�û���Ϣ" ? (blnUserChecked == true ? "1" : "0") : sTypeName == "������Ϣ" ? (blnDeptChecked == true ? "1" : "0") : sTypeName == "����ѡ��" ? sDorpDownListDefault : sTypeName == "������Ϣ" ? stxtDefault : sTypeName == "��ע��Ϣ" ? stxtMDefault : (blnChecked == true ? "1" : "0"));
                        dr["OrderBy"] = OrderBy == "" ? "0" : OrderBy;//����
                        dr["isChack"] = isChack == true ? "1" : "0";//�Ƿ����
                        dr["IsSelect"] = IsSelect == true ? "1" : "0";//�Ƿ��ѯ����
                        dt.Rows.Add(dr);

                        if (sTypeName == "������Ϣ")
                        {
                            sLastBaseGroup = sGroup;
                        }
                        else if (sTypeName == "��ע��Ϣ")
                        {
                            sLastBaseMGroup = sGroup;
                        }
                        else if (sTypeName == "����ѡ��")
                        {
                            sDownLIstGrop = sGroup;
                        }
                        else if (sTypeName == "������Ϣ")
                        {
                            sDropGrop = sGroup;
                        }
                        else if (sTypeName == "�û���Ϣ")
                        {
                            sUserGrop = sGroup;
                        }
                        else if (sTypeName == "��������")
                        {
                            sTimeGrop = sGroup;
                        }
                        else if (sTypeName == "��ֵ����")
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
                xmlEle.SetAttribute("OrderBy", row["OrderBy"].ToString().Trim() == "" ? "0" : row["OrderBy"].ToString().Trim());//����
                xmlEle.SetAttribute("isChack", row["isChack"].ToString().Trim() == "" ? "0" : row["isChack"].ToString().Trim());//�Ƿ�ʱ��  
                xmlEle.SetAttribute("IsSelect", row["IsSelect"].ToString().Trim() == "" ? "0" : row["IsSelect"].ToString().Trim());//�Ƿ��ѯ����



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
                xmlEle.SetAttribute("IsSelect", row["IsSelect"].ToString().Trim() == "" ? "0" : row["IsSelect"].ToString().Trim());//�Ƿ��ѯ����
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
                xmlEle.SetAttribute("IsSelect", row["IsSelect"].ToString().Trim() == "" ? "0" : row["IsSelect"].ToString().Trim());//�Ƿ��ѯ����

                xmlGroup.AppendChild(xmlEle);
            }

            return xmlDoc.InnerXml;

        }

        /// <summary>
        /// ��ע��Ϣ
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
            string strTypeName = "��������";

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
                    else
                    {
                        values[5] = (object)"������Ϣ";
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
                if (list.SelectedItem.Text == "��������")
                {
                    CheckBox chkbox = ((CheckBox)e.Item.FindControl("chkIsMust"));
                    chkbox.Visible = false;
                    chkbox = ((CheckBox)e.Item.FindControl("chkIsSelect"));
                    chkbox.Visible = false;
                }
                else if (list.SelectedItem.Text == "����ѡ��")
                {
                    ctrFlowCataDropList DropList = ((ctrFlowCataDropList)e.Item.FindControl("ctrFlowCataDropDefault"));
                    string FieldID = DataBinder.Eval(e.Item.DataItem, "ID").ToString();
                    string Defaultvalue = e.Item.Cells[9].Text.ToString() == "" ? "0" : e.Item.Cells[9].Text.ToString();//��ʼĬ��ֵ

                    //��ȡ��Ӧ��rootid
                    Equ_SchemaItemsDP ee = new Equ_SchemaItemsDP();
                    ee = ee.GetReCorded(FieldID);
                    DropList.RootID = long.Parse(ee.CatalogID.ToString() == "" ? "1" : ee.CatalogID.ToString());
                    DropList.CatelogID = long.Parse(Defaultvalue);

                }
                else if (list.SelectedItem.Text == "��ע��Ϣ")
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
            string sDownListGrop = "";//����ѡ��
            string sNewFieldID = "";

            string sDropGrop = "";//����
            string sUserGrop = "";//�û�
            string sTimeGrop = "";//����
            string sNumberGrop = "";//��ֵ

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
                    dr["TypeName"] = es.itemType == 7 ? "��ֵ����" : es.itemType == 6 ? "��������" : es.itemType == 4 ? "������Ϣ" : es.itemType == 5 ? "�û���Ϣ" : es.itemType == 3 ? "����ѡ��" : es.itemType == 0 ? "������Ϣ" : es.itemType == 2 ? "��ע��Ϣ" : "��������";
                    dr["ID"] = es.FieldID.Trim();
                    dr["CHName"] = es.CHName.Trim();
                    dr["IsMust"] = "0";
                    dr["Group"] = es.itemType == 7 ? sNumberGrop : es.itemType == 6 ? sTimeGrop : es.itemType == 4 ? sDropGrop : es.itemType == 5 ? sUserGrop : es.itemType == 3 ? sDownListGrop : es.itemType == 0 ? sLastBaseGroup : es.itemType == 2 ? sLastBaseMGroup : sLastGroup;
                    dr["Default"] = (es.itemType == 6 ? "0" : es.itemType == 7 ? "0" : es.itemType == 4 ? "1" : es.itemType == 5 ? "1" : es.itemType == 3 ? "1" : es.itemType == 0 ? "" : es.itemType == 2 ? "" : "1");
                    dr["OrderBy"] = "0";
                    dr["isChack"] = "0";//�Ƿ�ʱ�� 
                    dr["IsSelect"] = "0";//�Ƿ�ʱ��                     
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
            // �ڴ˴������û������Գ�ʼ��ҳ��

            rdoSchema.Items[0].Attributes.Add("onclick", "rdoSchema_onchange(this);");

            if (!IsPostBack)
            {
                if (this.Request.QueryString["SubjectID"] != null)
                {
                    sSubjectId = this.Request.QueryString["SubjectID"].ToString();
                    // ��¼��ǰ����
                    Session["OldEQSubectID"] = long.Parse(sSubjectId);

                    hidCatalogID.Value = sSubjectId;
                    LoadData(StringTool.String2Long(sSubjectId));
                }


                ctrSetUserOtherRight2.OperateType = 30;             //Ȩ����𣬶�ӦOperateType��Ϣ��ʲ�Ȩ��ֵΪ30
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
                //    //�Ƴ����������Ϣ   �в���������
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
                if (hidPCatalogID.Value == "-1")	//(lngCatalogID == 1)//������
                {
                    txtPCatalogName.Text = "��";
                }
                if (hidPCatalogID.Value == "-1")
                {
                    //��һ�� ������ɾ��
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

                //����session
                hidSchemaXml.Value = strXml;
                string strSubjectID = Equ_SubjectDP.Save(StringTool.String2Long(hidCatalogID.Value), txtSubjectName.Text.Trim(), StringTool.String2Long(hidPCatalogID.Value), int.Parse(txtSortID.Text.Trim()), txtDesc.Text.Trim(), sOrgID, rdoSchema.SelectedIndex, hidSchemaXml.Value.Trim(), hidImage.Value.Trim());
                hidCatalogID.Value = strSubjectID;

                //ǿ�Ʒ�����ػ���ʧЧ 
                HttpRuntime.Cache.Insert("CommCacheValidEquCategory", false);

                hidModified.Value = "false";

                //����Ӳ�ѯ�������򿪣������رղ�ˢ�¸�����
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
            hidCatalogID.Value = string.Empty;//��շ����ʶ

            hidImage.Value = string.Empty;
            Image1.ImageUrl = string.Empty;

            //����Ĭ���ϼ����࣬Ĭ�����ܣ�Ĭ���쵼
            DataTable dt = Equ_SubjectDP.GetSubjectByID(StringTool.String2Long(sSubjectId));
            foreach (DataRow dr in dt.Rows)
            {
                hidPCatalogID.Value = sSubjectId == null ? "0" : sSubjectId.Trim();
                txtPCatalogName.Text = dr["CatalogName"].ToString();
                hidModified.Value = "false";
                //ȱʡ�̳��ϼ�������
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

                //ǿ�Ʒ�����ػ���ʧЧ 
                HttpRuntime.Cache.Insert("CommCacheValidEquCategory", false);

                //Session["OldEQSubectID"] = strSubjectID;

                //����Ӳ�ѯ�������򿪣������رղ�ˢ�¸�����
                if (Request["CloseAction"] != null && Request["CloseAction"].ToString().Equals("1"))
                    PageTool.AddJavaScript(this, "window.parent.returnValue = 'refresh';self.close();");
                else
                    PageTool.AddJavaScript(this, "window.parent.contents.location='frmEqu_Content.aspx?type=0&CurrSubjectID=1';window.location='about:blank'");
            }
            catch (Exception ee)
            {
                PageTool.MsgBox(this, "ɾ������ʱ���ִ��󣬴���Ϊ��" + ee.Message.ToString());
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
                //ȱʡ�̳��ϼ�������
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
