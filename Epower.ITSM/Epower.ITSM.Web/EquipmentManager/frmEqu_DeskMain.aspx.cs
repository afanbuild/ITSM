/****************************************************************************
 * 
 * description:�豸��������
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-09-19
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
using Epower.ITSM.SqlDAL.ES_TBLCS;
using System.Collections.Generic;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frmEqu_DeskMain : BasePage
    {
        #region ����

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

        Hashtable ht_sel = null;     //�ж��Ƿ���Ҫ���ض�̬��������
        XmlNodeList nodes = null;    //��Ž����ʼ��ʱ����̬���صĳ�������xml��
        StringBuilder sbText = new StringBuilder();
        /// <summary>
        /// �豸����ID
        /// </summary>
        protected string CatalogID
        {
            get
            {
                if (CtrEquCataDropList1.SelectedIndex != string.Empty)
                    return CtrEquCataDropList1.SelectedIndex.ToString();
                return CtrEquCataDropList1.CatelogID.ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected bool IsSelect
        {
            get { if (Request["IsSelect"] != null) return true; else return false; }
        }
        /// <summary>
        /// 
        /// </summary>
        protected string FlowID
        {
            get { if (Request["FlowID"] != null) return Request["FlowID"].ToString(); else return "0"; }
        }
        #endregion

        #region �Զ�����
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
                //����ɾ��
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
            if (IsSelect)
                this.Master.IsCheckRight = false;
            else
            {
                this.Master.IsCheckRight = true;
                dgEqu_Desk.Columns[10].Visible = this.Master.GetEditRight();
            }
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);
            this.Master.Master_Button_ExportExcel_Click += new Global_BtnClick(Master_Master_Button_ExportExcel_Click);
            this.Master.Master_Button_GoHistory_Click += new Global_BtnClick(Master_Master_Button_GoHistory_Click);
            this.Master.ShowQueryPageButton();
            this.Master.ShowExportExcelButton(true);
            this.Master.MainID = "1";

            dgEqu_Desk.Columns[14].Visible = false; //�¼���¼����

            if (IsSelect)  //���Ϊѡ��ʱ
            {
                this.Master.ShowNewButton(false);
                this.Master.ShowDeleteButton(false);
                this.Master.ShowExportExcelButton(false);
                this.Master.ShowBackUrlButton(true);
                dgEqu_Desk.Columns[0].Visible = false;
                dgEqu_Desk.Columns[11].Visible = false;
                dgEqu_Desk.Columns[12].Visible = false;
                dgEqu_Desk.Columns[13].Visible = false;
                dgEqu_Desk.Columns[14].Visible = false;
                dgEqu_Desk.Columns[15].Visible = false;

                dgEqu_Desk.Columns[10].HeaderText = "ѡ��";
            }
        }
        #endregion

        #region Master_Master_Button_GoHistory_Click
        /// <summary>
        /// Master_Master_Button_GoHistory_Click
        /// </summary>
        private void Master_Master_Button_GoHistory_Click()
        {
            Response.Write("<script>window.close();</script>");
        }
        #endregion

        #region ����EXCEL
        /// <summary>
        /// ����EXCEL�¼�
        /// </summary>
        void Master_Master_Button_ExportExcel_Click()
        {
            ViewState["DataTableSpec"] = "True";
            if (CtrEquCataDropList1.CatelogID.ToString().Trim() == "1")
            {
                PageTool.MsgBox(this, "��ѡ�������");
                return;
            }


            string xml = CommonDP.SelectEqu_CategoryconfigureSchema(CtrEquCataDropList1.CatelogID.ToString().Trim());
            //����xml�����datetableʵ��
            DataTable table = createTable(xml);

            DataTable dt = LoadData(CtrEquCataDropList1.CatelogID.ToString().Trim());
            ExportExcel(dt, table);
        }

        /// <summary>
        /// �����ľ���ʵ��
        /// </summary>
        /// <param name="DataTableBase"></param>
        /// <param name="DataTableXml"></param>
        void ExportExcel(DataTable DataTableBase, DataTable DataTableXml)
        {
            try
            {
                System.IO.StringWriter tw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);

                hw.WriteFullBeginTag("html");
                hw.WriteLine();
                hw.WriteFullBeginTag("head");
                hw.WriteLine();
                hw.WriteLine("<meta http-equiv=Content-Type Content=text/html; charset=utf-8>");
                hw.WriteEndTag("head");
                hw.WriteLine();
                hw.WriteFullBeginTag("body");
                hw.WriteLine();

                hw.WriteLine("<table><tr><td align=center><font size=\"3\"><B>�ʲ�����<B></font></td></tr>");
                hw.WriteLine("<tr><td>");
                TableXmlExcel(ref hw, DataTableBase, DataTableXml);
                hw.WriteLine("</td></tr>");
                hw.WriteLine("</table>");


                //this.grdTypeDirection.RenderControl(hw);

                hw.WriteEndTag("body");
                hw.WriteLine();
                hw.WriteEndTag("html");

                //�����.xls�ļ�
                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("�ʲ�����", System.Text.Encoding.UTF8) + ".xls");
                Response.Charset = "utf-8";
                Response.Write(tw.ToString());
                Response.End();
            }
            catch
            {
            }
        }

        /// <summary>
        /// �������� ��֯
        /// </summary>
        /// <param name="hw"></param>
        /// <param name="DataTableBase"></param>
        /// <param name="DataTableXml"></param>
        void TableXmlExcel(ref System.Web.UI.HtmlTextWriter hw, DataTable DataTableBase, DataTable DataTableXml)
        {


            Hashtable hashTB = new Hashtable();
            #region ����xml����datatable���ݽṹ
            foreach (DataRow xmlRow in DataTableXml.Rows)
            {
                DataTableBase.Columns.Add(xmlRow["CHName"].ToString());
                string[] str = new string[3];
                str[0] = xmlRow["ID"].ToString();
                str[1] = xmlRow["TypeName"].ToString();
                str[2] = xmlRow["Default"].ToString();
                hashTB.Add(xmlRow["CHName"].ToString(), str);
            }
            #endregion

            #region ȡ�����ñ���Ϣ���� ��ӵ�table����
            foreach (DataRow BaseRow in DataTableBase.Rows)
            {
                List<EQU_deploy> listline = EQU_deploy.getEQU_deployList(long.Parse(BaseRow["ID"].ToString()));

                foreach (DataColumn dc in DataTableBase.Columns)
                {
                    //�ж���������xml�д���                    
                    if (hashTB[dc.ColumnName] != null)
                    {
                        string[] str = (string[])hashTB[dc.ColumnName];
                        if (str[1].ToString() == "����ѡ��")
                        {
                            IEnumerator e = listline.GetEnumerator();
                            while (e.MoveNext())
                            {
                                EQU_deploy deploy = (EQU_deploy)e.Current;
                                if (deploy.FieldID.ToString() == str[0].Trim())
                                {
                                    if (deploy.Value.Trim() != "")
                                    {
                                        BaseRow[dc.ColumnName] = CatalogDP.GetCatalogNamebyID(long.Parse(deploy.Value), long.Parse(str[2].Trim()));
                                    }
                                    else
                                    {
                                        BaseRow[dc.ColumnName] = deploy.Value.Trim();
                                    }
                                    break;
                                }
                            }
                        }
                        else if (str[1].ToString() == "������Ϣ")
                        {
                            IEnumerator e = listline.GetEnumerator();
                            while (e.MoveNext())
                            {
                                EQU_deploy deploy = (EQU_deploy)e.Current;
                                if (deploy.FieldID.ToString() == str[0].Trim())
                                {
                                    if (deploy.Value.Trim() != "")
                                    {
                                        //��ò�������
                                        BaseRow[dc.ColumnName] = DeptDP.GetDeptName(long.Parse(deploy.Value));
                                    }
                                    else
                                    {
                                        BaseRow[dc.ColumnName] = deploy.Value.Trim();
                                    }
                                    break;
                                }
                            }
                        }
                        else if (str[1].ToString() == "�û���Ϣ")
                        {
                            IEnumerator e = listline.GetEnumerator();
                            while (e.MoveNext())
                            {
                                EQU_deploy deploy = (EQU_deploy)e.Current;
                                if (deploy.FieldID.ToString() == str[0].Trim())
                                {
                                    if (deploy.Value.Trim() != "")
                                    {
                                        //��ò�������
                                        BaseRow[dc.ColumnName] = UserDP.GetUserName(long.Parse(deploy.Value));
                                    }
                                    else
                                    {
                                        BaseRow[dc.ColumnName] = deploy.Value.Trim();
                                    }
                                    break;
                                }
                            }
                        }
                        else if (str[1].ToString() == "��������")
                        {
                            IEnumerator e = listline.GetEnumerator();
                            while (e.MoveNext())
                            {
                                EQU_deploy deploy = (EQU_deploy)e.Current;
                                if (deploy.FieldID.ToString() == str[0].Trim())
                                {
                                    if (deploy.Value.Trim() != "" && deploy.Value.Trim() == "1")
                                    {
                                        //��ò�������
                                        BaseRow[dc.ColumnName] = "��";
                                    }
                                    else
                                    {
                                        BaseRow[dc.ColumnName] = "��";
                                    }
                                    break;
                                }
                            }
                        }
                        else
                        {
                            IEnumerator e = listline.GetEnumerator();
                            while (e.MoveNext())
                            {
                                EQU_deploy deploy = (EQU_deploy)e.Current;
                                if (deploy.FieldID.ToString() == str[0].Trim())
                                {
                                    BaseRow[dc.ColumnName] = deploy.Value.Trim();
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            #endregion


            #region �����������ݵı�ͷ
            hw.WriteLine("<table width=100% border=1>");
            hw.WriteLine("<tr>");
            foreach (DataColumn dc in DataTableBase.Columns)
            {
                switch (dc.ColumnName.ToLower())
                {
                    case "name":
                        hw.WriteLine("<td style='background-color:#ccccff;font-family:����;font-size:10pt;' align=center><B>�ʲ�����</B></td>");
                        break;
                    case "code":
                        hw.WriteLine("<td style='background-color:#ccccff;font-family:����;font-size:10pt;' align=center><B>�ʲ����</B></td>");
                        break;
                    case "costomname":
                        hw.WriteLine("<td style='background-color:#ccccff;font-family:����;font-size:10pt;' align=center><B>�����ͻ�</B></td>");
                        break;
                    case "partbankname":
                        hw.WriteLine("<td style='background-color:#ccccff;font-family:����;font-size:10pt;' align=center><B>ά������</B></td>");
                        break;
                    case "partbranchname":
                        hw.WriteLine("<td style='background-color:#ccccff;font-family:����;font-size:10pt;' align=center><B>ά������</B></td>");
                        break;
                    case "equstatusname":
                        hw.WriteLine("<td style='background-color:#ccccff;font-family:����;font-size:10pt;' align=center><B>�ʲ�״̬</B></td>");
                        break;
                    case "servicebegintime":
                        hw.WriteLine("<td style='background-color:#ccccff;font-family:����;font-size:10pt;' align=center><B>���޿�ʼ����</B></td>");
                        break;
                    case "serviceendtime":
                        hw.WriteLine("<td style='background-color:#ccccff;font-family:����;font-size:10pt;' align=center><B>���޽�������</B></td>");
                        break;
                    case "mastcustname":
                        hw.WriteLine("<td style='background-color:#ccccff;font-family:����;font-size:10pt;' align=center><B>����λ</B></td>");
                        break;
                    default:
                        if (hashTB[dc.ColumnName] != null)
                        {
                            hw.WriteLine("<td style='background-color:#ccccff;font-family:����;font-size:10pt;' align=center><B>" + dc.ColumnName + "</B></td>");
                        }
                        break;
                }
            }
            hw.WriteLine("</tr>");
            #endregion

            #region ��������������
            foreach (DataRow BaseRow in DataTableBase.Rows)
            {
                hw.WriteLine("<tr>");
                foreach (DataColumn dc in DataTableBase.Columns)
                {
                    switch (dc.ColumnName.ToLower())
                    {
                        case "name":
                            hw.WriteLine("<td style='font-family:����;font-size:10pt;' align=center>" + BaseRow[dc.ColumnName].ToString().Trim() + "</td>");
                            break;
                        case "code":
                            hw.WriteLine("<td style='font-family:����;font-size:10pt;' align=center>" + BaseRow[dc.ColumnName].ToString().Trim() + "</td>");
                            break;
                        case "costomname":
                            hw.WriteLine("<td style='font-family:����;font-size:10pt;' align=center>" + BaseRow[dc.ColumnName].ToString().Trim() + "</td>");
                            break;
                        case "partbankname":
                            hw.WriteLine("<td style='font-family:����;font-size:10pt;' align=center>" + BaseRow[dc.ColumnName].ToString().Trim() + "</td>");
                            break;
                        case "partbranchname":
                            hw.WriteLine("<td style='font-family:����;font-size:10pt;' align=center>" + BaseRow[dc.ColumnName].ToString().Trim() + "</td>");
                            break;
                        case "equstatusname":
                            hw.WriteLine("<td style='font-family:����;font-size:10pt;' align=center>" + BaseRow[dc.ColumnName].ToString().Trim() + "</td>");
                            break;
                        case "servicebegintime":
                            hw.WriteLine("<td style='font-family:����;font-size:10pt;' align=center>" + BaseRow[dc.ColumnName].ToString().Trim() + "</td>");
                            break;
                        case "serviceendtime":
                            hw.WriteLine("<td style='font-family:����;font-size:10pt;' align=center>" + BaseRow[dc.ColumnName].ToString().Trim() + "</td>");
                            break;
                        case "mastcustname":
                            hw.WriteLine("<td style='font-family:����;font-size:10pt;' align=center>" + BaseRow[dc.ColumnName].ToString().Trim() + "</td>");
                            break;
                        default:
                            if (hashTB[dc.ColumnName] != null)
                            {
                                hw.WriteLine("<td style='font-family:����;font-size:10pt;' align=center>" + BaseRow[dc.ColumnName].ToString().Trim() + "</td>");
                            }
                            break;
                    }
                }
                hw.WriteLine("</tr>");
            }

            hw.WriteLine("</table>");
            #endregion

        }



        //����xml�� �������ת����datatable
        public DataTable createTable(string strXmlTemp)
        {
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
            return dt;
        }

        //����tableʵ��
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

        #endregion

        #region Master_Master_Button_New_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("frmEqu_DeskEdit.aspx?subjectid=" + CatalogID);
        }
        #endregion

        #region Master_Master_Button_Query_Click
        /// <summary>
        /// ��ѯ�¼�
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            //���ض�̬��������
            ht_sel = new Hashtable();
            ValueCollection(Table3, ref ht_sel);

            Bind("");
        }
        #endregion

        #region �ݹ��ȡ�ؼ���ֵ
        /// <summary>
        /// �ݹ��ȡ�ؼ���ֵ
        /// </summary>
        /// <param name="ctrRoot"></param>
        /// <param name="list"></param>
        private void ValueCollection(Control ctrRoot, ref Hashtable list)
        {
            string strType = string.Empty;              //�ؼ�����
            foreach (Control pControl in ctrRoot.Controls)
            {
                if (pControl.ID != null)
                {
                    if (pControl.ID.StartsWith("tDynamic"))
                    {
                        strType = pControl.GetType().Name;      //�ؼ�����
                        switch (strType)
                        {
                            case "HtmlTableRow":
                            case "HtmlTableCell":
                                ValueCollection(pControl, ref list);
                                break;
                            case "TextBox":
                                //�ı�����ֵ
                                list.Add(((TextBox)pControl).Attributes["Tag"], ((TextBox)pControl).Text);
                                break;
                            case "controls_ctrdateandtime_ascx":
                                //����
                                list.Add(((Epower.ITSM.Web.Controls.CtrDateAndTime)pControl).Attributes["Tag"], ((Epower.ITSM.Web.Controls.CtrDateAndTime)pControl).dateTimeString);
                                break;
                            case "controls_ctrflowcatadroplist_ascx":
                                //����
                                list.Add(((Epower.ITSM.Web.Controls.ctrFlowCataDropList)pControl).Attributes["Tag"], ((Epower.ITSM.Web.Controls.ctrFlowCataDropList)pControl).CatelogID);
                                break;
                            case "controls_deptpicker_ascx":
                                //����
                                list.Add(((Epower.ITSM.Web.Controls.DeptPicker)pControl).Attributes["Tag"], ((Epower.ITSM.Web.Controls.DeptPicker)pControl).DeptID);
                                break;
                            case "controls_userpicker_ascx":
                                //�û�
                                list.Add(((Epower.ITSM.Web.Controls.UserPicker)pControl).Attributes["Tag"], ((Epower.ITSM.Web.Controls.UserPicker)pControl).UserID);
                                break;
                            case "":
                                list.Add(((CheckBox)pControl).Attributes["Tag"], ((CheckBox)pControl).Checked == true ? "1" : "0");
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        #endregion

        #region Master_Master_Button_Delete_Click
        /// <summary>
        /// Master_Master_Button_Delete_Click
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            string strID = string.Empty;                //��������Ҫɾ�����ʲ�ID���
            Equ_DeskDP ee = new Equ_DeskDP();
            foreach (DataGridItem itm in dgEqu_Desk.Items)
            {
                if (itm.ItemType == ListItemType.AlternatingItem ||
                    itm.ItemType == ListItemType.Item)
                {
                    string sID = itm.Cells[1].Text;
                    string strName = itm.Cells[2].Text;         //�ʲ�����
                    CheckBox chkdel = (CheckBox)itm.Cells[0].FindControl("chkDel");
                    if (chkdel.Checked)
                    {
                        strID += sID + ",";
                    }
                }
            }

            strID = strID.Substring(0, strID.Length - 1);           //ȥ��ID����ַ������һ������

            ee.DeleteRecorded(strID);

            //ɾ���Զ�����
            DeleteItem();

            Bind("");
        }
        #endregion

        #region �õ�ɾ��ǰ����ʾ����
        /// <summary>
        /// �õ�ɾ��ǰ����ʾ���� 
        /// </summary>
        /// <returns></returns>
        public string GetTipsAndEquIDs()
        {
            string strTip = string.Empty;               //ɾ��ʱ����ʾ��Ϣ
            Equ_DeskDP ee = new Equ_DeskDP();
            foreach (DataGridItem itm in dgEqu_Desk.Items)
            {
                if (itm.ItemType == ListItemType.AlternatingItem ||
                    itm.ItemType == ListItemType.Item)
                {
                    string sID = itm.Cells[1].Text;
                    string strName = itm.Cells[2].Text;         //�ʲ�����
                    CheckBox chkdel = (CheckBox)itm.Cells[0].FindControl("chkDel");
                    if (chkdel.Checked)
                    {
                        string strEqus = Equ_DeskDP.GetEquNamesByEquID(sID);        //�����ʲ�ID�õ�����Ӱ����ʲ��������
                        strEqus = strEqus.Substring(0, strEqus.Length - 1);         //�����һ������ȥ��
                        if (strEqus != "")
                        {
                            //�����Ӱ����ʲ����������ʾ���ǣ���ɾ������ɾ
                            strTip += strName + "��ɾ����Ӱ�쵽:" + strEqus + ";";
                        }
                    }
                }
            }

            strTip += "��ȷ��Ҫɾ����Щ�ʲ���?";
            return strTip;
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
            cpfECustomerInfo.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(bindGrid);
            if (!IsPostBack)
            {
                if (Request["Opener_ClientId"] != null)
                {
                    Opener_ClientId = Request["Opener_ClientId"].ToString().Replace("hidClientId_ForOpenerPage", "");
                }

                //������ʾ
                PageDeal.SetLanguage(this.Controls[0]);
               
                SetHeaderText();

                InitDropDownList("-1");         //��ʼ�������

                //if (CommonDP.GetConfigValue("Other", "ItsmCustomerMode") != "0")
                //{
                //�󶨷���λ
                string sMastCust = "";
                if (Request["MastCust"] != null)
                    sMastCust = Request["MastCust"].ToString();
                InitMCDropDownList(sMastCust);

                if (Request["EquName"] != null)
                    txtName.Text = Request["EquName"].ToString();
                //if (Request["Cust"] != null)
                //    txtCust.Text = Request["Cust"].ToString();

                if (Request["EquipmentCatalogID"] != null)
                    hidEquipmentCatalogID.Value = Request["EquipmentCatalogID"].ToString() == "" ? "0" : Request["EquipmentCatalogID"].ToString();

                if (Request["subjectid"] != null && Request["subjectid"] != "-1" && Request["subjectid"] != "1")
                {
                    CtrEquCataDropList1.CatelogID = decimal.Parse(Request["subjectid"].ToString());
                    InitDropDownList(Request["subjectid"].ToString());
                    Bind(Request["subjectid"].ToString());
                }
                else
                {
                    InitDropDownList("1");          //��ʼ�������
                    Bind("");
                }
            }

            LoadHtmlContl();                //��̬���س�������
        }
        #endregion


        #region ����datagrid��ͷ��ʾ ����ǰ 2013-05-17
        /// <summary>
        /// ����datagrid��ͷ��ʾ
        /// </summary>
        private void SetHeaderText()
        {
            dgEqu_Desk.Columns[2].HeaderText = PageDeal.GetLanguageValue("Equ_DeskName");
            dgEqu_Desk.Columns[3].HeaderText = PageDeal.GetLanguageValue("Equ_Code");
            dgEqu_Desk.Columns[4].HeaderText = PageDeal.GetLanguageValue("Equ_CostomName");
            dgEqu_Desk.Columns[5].HeaderText = PageDeal.GetLanguageValue("Equ_CatalogName");
            dgEqu_Desk.Columns[6].HeaderText = PageDeal.GetLanguageValue("Equ_SerialNumber");
            dgEqu_Desk.Columns[7].HeaderText = PageDeal.GetLanguageValue("Equ_CatalogName");
            dgEqu_Desk.Columns[8].HeaderText = PageDeal.GetLanguageValue("Equ_Breed");
            dgEqu_Desk.Columns[9].HeaderText = PageDeal.GetLanguageValue("Equ_Model");
        }
        #endregion

        #region ��ҳ��dagagrid
        /// <summary>
        /// 
        /// </summary>
        public void bindGrid()
        {
            Bind("");
        }
        #endregion

        #region InitMCDropDownList
        /// <summary>
        /// �󶨷���λ
        /// </summary>
        /// <param name="sMastCust"></param>
        private void InitMCDropDownList(string sMastCust)
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
            if (sMastCust.Length > 0)
                ddltMastCustID.SelectedIndex = ddltMastCustID.Items.IndexOf(ddltMastCustID.Items.FindByText(sMastCust));
        }
        #endregion

        #region InitDropDownList
        /// <summary>
        /// ��������
        /// </summary>
        private void InitDropDownList(string strCatalogID)
        {
            #region ��ȡ������

            string strSchemaXml = string.Empty;                                             //������xml            
            DataTable dt = Equ_SubjectDP.GetSubjectByID(long.Parse(strCatalogID));          //�õ�������µ�������xml
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    strSchemaXml = dt.Rows[0]["ConfigureSchema"].ToString();
                }
            }

            Equ_SchemaItemsDP ee = new Equ_SchemaItemsDP();

            #endregion

            #region ����������
            string sWhere = " AND itemtype = 0 ";
            string sOrder = string.Empty;
            string strFieldID = "(0";                                                       //����ʲ�����¸�����������FieldID

            strFieldID = strFieldID + AnalySchema(strSchemaXml, "EquScheme/BaseItem/AttributeItem");           //��������������xml���õ���FieldID�ļ���
            sWhere += " AND FieldID in " + strFieldID + ")";                                                   //����ɸѡ����

            dt = ee.GetDataTable(sWhere, sOrder);
            ddlSchemaItemJB.DataSource = dt;
            ddlSchemaItemJB.DataTextField = "CHName";
            ddlSchemaItemJB.DataValueField = "FieldID";
            ddlSchemaItemJB.DataBind();
            ddlSchemaItemJB.Items.Insert(0, new ListItem("", ""));
            #endregion

            #region ����������
            sWhere = " AND itemtype = 1 ";
            sOrder = string.Empty;
            strFieldID = "(0";                                                                                 //����ʲ�����¸�����������FieldID     
            strFieldID = strFieldID + AnalySchema(strSchemaXml, "EquScheme/RelationConfig/AttributeItem");     //��������������xml���õ���FieldID�ļ���
            sWhere += " AND FieldID in " + strFieldID + ")";                                                   //����ɸѡ����

            dt = ee.GetDataTable(sWhere, sOrder);
            ddlSchemaItemGL.DataSource = dt;
            ddlSchemaItemGL.DataTextField = "CHName";
            ddlSchemaItemGL.DataValueField = "FieldID";
            ddlSchemaItemGL.DataBind();
            ddlSchemaItemGL.Items.Insert(0, new ListItem("", ""));
            #endregion

            #region ����������
            sWhere = " AND itemtype = 3 ";
            sOrder = string.Empty;
            strFieldID = "(0";                                                                                 //����ʲ�����¸�����������FieldID     
            strFieldID = strFieldID + AnalySchema(strSchemaXml, "EquScheme/BaseItem/AttributeItem");     //��������������xml���õ���FieldID�ļ���
            sWhere += " AND FieldID in " + strFieldID + ")";                                                   //����ɸѡ����

            dt = ee.GetDataTable(sWhere, sOrder);
            ddlPulldownItem.DataSource = dt;
            ddlPulldownItem.DataTextField = "CHName";
            ddlPulldownItem.DataValueField = "FieldID";
            ddlPulldownItem.DataBind();
            ddlPulldownItem.Items.Insert(0, new ListItem("", ""));

            #endregion

            #region ����������
            sWhere = " AND itemtype = 6 ";
            sOrder = string.Empty;
            strFieldID = "(0";                                                                                 //����ʲ�����¸�����������FieldID     
            strFieldID = strFieldID + AnalySchema(strSchemaXml, "EquScheme/BaseItem/AttributeItem");     //��������������xml���õ���FieldID�ļ���
            sWhere += " AND FieldID in " + strFieldID + ")";                                                   //����ɸѡ����

            dt = ee.GetDataTable(sWhere, sOrder);
            ddlSchemaItemDT.DataSource = dt;
            ddlSchemaItemDT.DataTextField = "CHName";
            ddlSchemaItemDT.DataValueField = "FieldID";
            ddlSchemaItemDT.DataBind();
            ddlSchemaItemDT.Items.Insert(0, new ListItem("", ""));

            #endregion

            #region ����������
            sWhere = " AND itemtype = 4 ";
            sOrder = string.Empty;
            strFieldID = "(0";                                                                                 //����ʲ�����¸�����������FieldID     
            strFieldID = strFieldID + AnalySchema(strSchemaXml, "EquScheme/BaseItem/AttributeItem");     //��������������xml���õ���FieldID�ļ���
            sWhere += " AND FieldID in " + strFieldID + ")";                                                   //����ɸѡ����

            dt = ee.GetDataTable(sWhere, sOrder);
            ddlSchemaItemDP.DataSource = dt;
            ddlSchemaItemDP.DataTextField = "CHName";
            ddlSchemaItemDP.DataValueField = "FieldID";
            ddlSchemaItemDP.DataBind();
            ddlSchemaItemDP.Items.Insert(0, new ListItem("", ""));

            #endregion

            #region �û�������
            sWhere = " AND itemtype = 5 ";
            sOrder = string.Empty;
            strFieldID = "(0";                                                                                 //����ʲ�����¸�����������FieldID     
            strFieldID = strFieldID + AnalySchema(strSchemaXml, "EquScheme/BaseItem/AttributeItem");     //��������������xml���õ���FieldID�ļ���
            sWhere += " AND FieldID in " + strFieldID + ")";                                                   //����ɸѡ����

            dt = ee.GetDataTable(sWhere, sOrder);
            ddlSchemaItemUS.DataSource = dt;
            ddlSchemaItemUS.DataTextField = "CHName";
            ddlSchemaItemUS.DataValueField = "FieldID";
            ddlSchemaItemUS.DataBind();
            ddlSchemaItemUS.Items.Insert(0, new ListItem("", ""));

            #endregion
        }
        #endregion

        #region ��ʼ����̬��ѯ����
        /// <summary>
        /// ��ʼ����̬��ѯ����
        /// </summary>
        private void LoadHtmlContl()
        {
            string strSchemaXml = string.Empty;                                             //������xml            
            DataTable dt = Equ_SubjectDP.GetSubjectByID(long.Parse(CatalogID));          //�õ�������µ�������xml
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    strSchemaXml = dt.Rows[0]["ConfigureSchema"].ToString();
                }
            }

            #region ��ʼ�����ò�ѯ����
            if (strSchemaXml != string.Empty && IsSelect == false)
            {
                //���û����չ���û�и߼���������������
                Table13.Visible = true;
                Table12.Visible = true;

                LoadHtmlControls(strSchemaXml);
            }
            else
            {
                Table13.Visible = false;
                Table12.Visible = false;
            }
            #endregion
        }
        #endregion

        #region ��̬���ز�ѯ����
        /// <summary>
        /// ��̬���ز�ѯ����
        /// </summary>
        /// <param name="strSchemaXml"></param>
        private void LoadHtmlControls(string strSchemaXml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(strSchemaXml);

            HtmlTableRow tr = new HtmlTableRow();
            HtmlTableCell tc = new HtmlTableCell();

            nodes = xmlDoc.SelectNodes("EquScheme/BaseItem/AttributeItem");     //����������
            RepeatCataItems(ref tr, ref tc, nodes);
        }
        #endregion

        #region ѭ����������������
        /// <summary>
        /// ѭ����������������
        /// </summary>
        private void RepeatCataItems(ref HtmlTableRow tr, ref HtmlTableCell tc, XmlNodeList nodes)
        {
            int iCount = 0;         //���㳣�������ĸ�������û�У���չʾ��������
            int iRow = 0;
            int iCell = 0;

            foreach (XmlNode node in nodes)
            {
                if (node.Attributes["IsSelect"].Value == "1")
                {
                    iCount++;
                    //���������Ĳ�ѯ����Ϊ�棬��ѭ����ӵ���ѯ������
                    if (iCell == 0)
                    {
                        //���iCellΪ1����Ҫ�������
                        iRow++;
                        tr = new HtmlTableRow();
                        tr.ID = "tDynamicRow" + iRow;
                    }

                    //���α�������㣬��Ӳ�ѯ����
                    tc = AddControlByNode(node, ref tr, ref tc, ref iRow, ref iCell);
                }
            }

            if (iCell == 1)
            {
                //���td����Ϊ1���������һ����ѯ������ֻ��һ������ʱ��Ҫ�����һ��td����ռ3����
                tc.Attributes.Add("colspan", "3");
                AddControl(Table3, tr);
            }

            if (iCount == 0)
            {
                //���û�г�����������չʾ������������
                Table13.Visible = false;
            }
            else
            {
                Table13.Visible = true;
            }
        }
        #endregion

        #region ���α�������㣬��Ӳ�ѯ����
        /// <summary>
        /// ���α�������㣬��Ӳ�ѯ����
        /// </summary>
        private HtmlTableCell AddControlByNode(XmlNode node, ref HtmlTableRow ctrl, ref HtmlTableCell _tc, ref int iRow, ref int iCell)
        {
            iCell++;                //td��������

            //��һ��td������
            HtmlTableCell tc = new HtmlTableCell();
            tc.ID = "tTitleDynamicCell_" + iRow.ToString() + "_" + iCell.ToString();
            tc.Attributes.Add("class", "listTitleRight");
            tc.Attributes.Add("style", "width:12%");
            tc.InnerText = node.Attributes["CHName"].Value;

            AddControl(ctrl, tc);           //��ӵ�һ��td��tr��

            //�ڶ���td������
            tc = new HtmlTableCell();
            tc.ID = "tDynamicCell_" + iRow.ToString() + "_" + iCell.ToString();
            tc.Attributes.Add("class", "list");
            if (iCell == 2)
            {
                //���ĸ�td,�����ÿ��
            }
            else
            {
                tc.Attributes.Add("style", "width:35%");
            }

            Control control = GetSettingControl(node);
            if (control != null)
            {
                AddControl(tc, control);
            }
            AddControl(ctrl, tc);           //��ӵڶ���td����һ��txt�ı�����tr��

            if (iCell == 2)
            {
                //���iCellΪ2������һ���Ѿ�ѭ����������Ҫ������
                iCell = 0;
                AddControl(Table3, ctrl);
            }

            return tc;
        }
        #endregion

        #region ��ȡ���õĿؼ�
        /// <summary>
        /// ��ȡ���õĿؼ�
        /// </summary>
        /// <param name="node"></param>
        /// <param name="strControlType"></param>
        /// <returns></returns>
        private Control GetSettingControl(XmlNode node)
        {
            string strContrType = node.Attributes["CtrlType"].Value;

            Control control;
            switch (strContrType)
            {
                case "MultiLine":
                //��ע
                case "TextBox":
                    //�ı���
                    control = new TextBox();
                    control.ID = "tDynamic" + "_txt_" + node.Attributes["ID"].Value;
                    ((TextBox)control).Attributes.Add("Tag", node.Attributes["ID"].Value);
                    break;
                case "Time":
                    //����
                    control = (Epower.ITSM.Web.Controls.CtrDateAndTime)LoadControl("~/Controls/CtrDateAndTimeV2.ascx");
                    control.ID = "tDynamic" + "_DateTime_" + node.Attributes["ID"].Value;
                    ((Epower.ITSM.Web.Controls.CtrDateAndTime)control).ShowTime = false;
                    ((Epower.ITSM.Web.Controls.CtrDateAndTime)control).Attributes.Add("Tag", node.Attributes["ID"].Value);
                    break;
                case "DropDownList":
                    //����
                    control = (Epower.ITSM.Web.Controls.ctrFlowCataDropList)LoadControl("~/Controls/ctrFlowCataDropList.ascx");
                    control.ID = "tDynamic" + "_cataddl_" + node.Attributes["ID"].Value;
                    ((Epower.ITSM.Web.Controls.ctrFlowCataDropList)control).RootID = long.Parse(node.Attributes["Default"].Value == "" ? "0" : node.Attributes["Default"].Value);
                    ((Epower.ITSM.Web.Controls.ctrFlowCataDropList)control).Attributes.Add("Tag", node.Attributes["ID"].Value);
                    break;
                case "Number":
                    //��ֵ
                    control = new TextBox();
                    control.ID = "tDynamic" + "_Num_" + node.Attributes["ID"].Value;
                    ((TextBox)control).Attributes.Add("Tag", node.Attributes["ID"].Value);
                    break;
                case "deptList":
                    //����
                    control = (Epower.ITSM.Web.Controls.DeptPicker)LoadControl("~/Controls/DeptPicker.ascx");
                    control.ID = "tDynamic" + "_Dept_" + node.Attributes["ID"].Value;
                    ((Epower.ITSM.Web.Controls.DeptPicker)control).Attributes.Add("Tag", node.Attributes["ID"].Value);
                    break;
                case "UserList":
                    //�û�
                    control = (Epower.ITSM.Web.Controls.UserPicker)LoadControl("~/Controls/UserPicker.ascx");
                    control.ID = "tDynamic" + "_User_" + node.Attributes["ID"].Value;
                    ((Epower.ITSM.Web.Controls.UserPicker)control).Attributes.Add("Tag", node.Attributes["ID"].Value);
                    break;
                case "":
                    //����
                    control = new CheckBox();
                    control.ID = "tDynamic" + "_Chb_" + node.Attributes["ID"].Value;
                    ((CheckBox)control).Attributes.Add("Tag", node.Attributes["ID"].Value);
                    break;
                default:
                    control = null;
                    break;
            }
            return control;
        }
        #endregion

        #region ��ӿؼ�td�Ϳؼ�
        /// <summary>
        /// ��ӿؼ�td�Ϳؼ�
        /// </summary>
        /// <param name="pParentControl">���ؼ�</param>
        /// <param name="pSubControl">�ӿؼ�</param>
        private void AddControl(Control pParentControl, Control pSubControl)
        {
            if (pParentControl != null && pSubControl != null)
            {
                bool bFound = false;
                foreach (Control contrl in pParentControl.Controls)
                {
                    if (contrl.ID == pSubControl.ID)
                    {
                        bFound = true;
                        break;
                    }
                }
                if (!bFound)
                {
                    //���û�ҵ�������� 
                    pParentControl.Controls.Add(pSubControl);
                }
            }
        }
        #endregion

        #region ����������xml
        /// <summary>
        /// ����������xml
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

        #region Bind
        /// <summary>
        /// 
        /// </summary>
        private void Bind(string Strsubjectid)
        {
            #region ������

            int iRowCount = 0;
            string sCalalogID = string.Empty;
            DataTable dt;
            string sWhere = " 1=1 ";
            string sOrder = " order by ID Desc";
            Hashtable ht = new Hashtable();

            bool blnSpec = false;

            #endregion

            #region ���

            if (Strsubjectid != "")
            {
                sCalalogID = Strsubjectid;
                CtrEquCataDropList1.SelectedIndex = sCalalogID;
            }
            else
            {
                sCalalogID = CtrEquCataDropList1.CatelogID.ToString();
            }

            #endregion

            #region ��̬��������

            if (ht_sel != null && nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    if (node.Attributes["IsSelect"].Value == "1")
                    {
                        //����ڲ�ѯ������
                        string strID = node.Attributes["ID"].Value;             //������ID
                        string strName = node.Attributes["CHName"].Value;       //����������
                        string strType = node.Attributes["CtrlType"].Value;     //����������
                        string strValue = string.Empty;                         //�������ֵ
                        strValue = ht_sel[strID].ToString().Trim();

                        if (strType == "Time")
                        {
                            ////���Ϊ�������ͣ����ַ���ת����������
                            if (strValue != "")
                                sWhere += " and ID in (select EquID from EQU_deploy where FieldID = " + StringTool.SqlQ(strID)
                                                                            + " and CHName = " + StringTool.SqlQ(strName) + " and Value = Convert(Datetime," + StringTool.SqlQ(strValue) + "))";
                        }
                        if (strType == "deptList" || strType == "UserList")
                        {
                            //���Ϊ���š��û�
                            if (strValue != "0" && strValue != "")
                                sWhere += " and ID in (select EquID from EQU_deploy where FieldID = " + StringTool.SqlQ(strID)
                                                                            + " and CHName = " + StringTool.SqlQ(strName) + " and Value = " + strValue + ")";
                        }
                        else if (strType == "DropDownList")
                        {
                            //���Ϊ����
                            string strRoot = node.Attributes["Default"].Value;          //������ID
                            if (strValue != strRoot && strValue != "0" && strValue != "")
                                sWhere += " and ID in (select EquID from EQU_deploy where FieldID = " + StringTool.SqlQ(strID)
                                                                            + " and CHName = " + StringTool.SqlQ(strName) + " and Value = " + strValue + ")";
                        }
                        else
                        {
                            if (strValue != "")
                                sWhere += " and ID in (select EquID from EQU_deploy where FieldID = " + StringTool.SqlQ(strID)
                                    + " and CHName = " + StringTool.SqlQ(strName) + " and Value like " + StringTool.SqlQ("%" + strValue + "%") + ")";
                        }
                    }
                }
            }

            #endregion

            #region �̶���������

            #region ��չ������

            //����������
            if (ddlSchemaItemJB.SelectedItem.Text.Trim() != string.Empty && txtItemValue.Text.Trim().Length > 0)
            {
                ht.Add(ddlSchemaItemJB.SelectedValue.Trim(), txtItemValue.Text.Trim());
            }

            //����������
            if (ddlSchemaItemGL.SelectedItem.Text.Trim() != string.Empty)
            {
                ht.Add(ddlSchemaItemGL.SelectedValue.Trim(), (chkItemValue.Checked == true ? "1" : "0"));
            }

            //����������
            if (ddlPulldownItem.SelectedItem.Text.Trim() != string.Empty && ddlPulldownValue.SelectedItem.Text.Trim() != string.Empty)
            {
                ht.Add(ddlPulldownItem.SelectedValue.Trim(), ddlPulldownValue.SelectedValue.Trim());
            }

            //����������
            if (ddlSchemaItemDT.SelectedItem.Text.Trim() != string.Empty && dtServiceTime.dateTimeString != "")
            {
                ht.Add(ddlSchemaItemDT.SelectedValue.Trim(), dtServiceTime.dateTime.ToShortDateString().Trim());
            }

            //����������
            if (ddlSchemaItemDP.SelectedItem.Text.Trim() != string.Empty && DeptPicker1.DeptID != 0)
            {
                ht.Add(ddlSchemaItemDP.SelectedValue.Trim(), DeptPicker1.DeptID.ToString());
            }

            //�û�������
            if (ddlSchemaItemUS.SelectedItem.Text.Trim() != string.Empty && UserPicker1.UserID != 0)
            {
                ht.Add(ddlSchemaItemUS.SelectedValue.Trim(), UserPicker1.UserID.ToString());
            }
            #endregion

            Equ_DeskDP ee = new Equ_DeskDP();

            //2008-05-12���������  1 �ǰ����ͻ���Ϣ �� ����λ ����һ�ֲ�ѯ,û�а����ͻ���Ϣ������һ�ֲ�ѯ,
            //if (ShowMastCust.Visible == true)
            //{
            if (ddltMastCustID.SelectedItem != null && ddltMastCustID.SelectedItem.Text.Trim() != string.Empty)
            {
                sWhere += " And MastCustID=" + ddltMastCustID.SelectedValue.Trim();
                blnSpec = true;
            }
            //}
            if (txtCust.Text.Trim() != string.Empty)
            {
                //Ӣ����
                sWhere += " And (nvl(FullName,'') like " + StringTool.SqlQ("%" + txtCust.Text.Trim() + "%");
                //������
                sWhere += " OR nvl(ShortName,'') like " + StringTool.SqlQ("%" + txtCust.Text.Trim() + "%");
                //����
                sWhere += " OR nvl(CustomCode,'') like " + StringTool.SqlQ("%" + txtCust.Text.Trim() + "%");
                //�绰
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
                //ά������
                if (DeptPickerbank2.DeptID > 0)
                    sWhere += " and partBankId=" + DeptPickerbank2.DeptID;


                if (decimal.Parse(sCalalogID) > 0)
                {
                    if (chkIncludeSub.Checked == true)
                    {
                        if (sCalalogID != "1")
                        {
                            string strFullID = Equ_SubjectDP.GetSubjectFullID(long.Parse(sCalalogID));
                            sWhere += " and CatalogID in (select CatalogID from Equ_Category where FullID like" + StringTool.SqlQ(strFullID + "%") + ")";
                        }
                    }
                    else
                    {
                        sWhere += " and CatalogID = " + sCalalogID;
                    }
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
                //ά������
                if (DeptPickerbank2.DeptID > 0)
                    sWhere += " and partBankId=" + DeptPickerbank2.DeptID;

                if (decimal.Parse(sCalalogID) > 0)
                {
                    if (chkIncludeSub.Checked == true)
                    {
                        if (sCalalogID != "1")
                        {
                            string strFullID = Equ_SubjectDP.GetSubjectFullID(long.Parse(sCalalogID));
                            sWhere += " and CatalogID in (select CatalogID from Equ_Category where FullID like" + StringTool.SqlQ(strFullID + "%") + ")";
                        }
                    }
                    else
                    {
                        sWhere += " and CatalogID = " + sCalalogID;
                    }
                }

                if (!string.IsNullOrEmpty(ctrCataEquStatus.CatelogValue.Trim()))
                    sWhere += " and EquStatusID= " + ctrCataEquStatus.CatelogID.ToString();

                dt = ee.GetDataTableSpec(sWhere, sOrder, ht, this.cpfECustomerInfo.PageSize, this.cpfECustomerInfo.CurrentPage, ref iRowCount);
            }

            #endregion

            dgEqu_Desk.DataSource = dt;
            dgEqu_Desk.DataBind();
            this.cpfECustomerInfo.RecordCount = iRowCount;
            this.cpfECustomerInfo.Bind();
        }
        #endregion

        #region LoadData
        /// <summary>
        /// 
        /// </summary>
        private DataTable LoadData(string Strsubjectid)
        {
            int iRowCount = 0;
            string sCalalogID = string.Empty;
            if (Strsubjectid != "")
            {
                sCalalogID = Strsubjectid;
            }
            else
            {
                sCalalogID = CtrEquCataDropList1.CatelogID.ToString();
            }
            DataTable dt;
            string sWhere = " 1=1 ";
            string sOrder = " order by ID Desc ";
            Hashtable ht = new Hashtable();

            bool blnSpec = false;

            #region ��̬��������

            if (ht_sel != null)
            {
                foreach (XmlNode node in nodes)
                {
                    string strID = node.Attributes["ID"].Value;             //������ID
                    string strName = node.Attributes["CHName"].Value;       //����������
                    string strType = node.Attributes["CtrlType"].Value;     //����������
                    string strValue = string.Empty;                         //�������ֵ
                    strValue = ht[strID].ToString();

                    if (strType == "Time")
                    {
                        ////���Ϊ�������ͣ����ַ���ת����������
                        sWhere += " and ID in (select EquID from EQU_deploy where FieldID = " + StringTool.SqlQ(strID)
                                                                    + " and CHName = " + StringTool.SqlQ(strName) + " and Value = Convert(Datetime," + StringTool.SqlQ(strValue) + "))";
                    }
                    if (strType == "deptList" || strType == "DropDownList" || strType == "UserList")
                    {
                        //���Ϊ���������š��û�
                        sWhere += " and ID in (select EquID from EQU_deploy where FieldID = " + StringTool.SqlQ(strID)
                                                                    + " and CHName = " + StringTool.SqlQ(strName) + " and Value = " + strValue + ")";
                    }
                    else
                    {
                        if (strValue != "")
                            sWhere += " and ID in (select EquID from EQU_deploy where FieldID = " + StringTool.SqlQ(strID)
                                + " and CHName = " + StringTool.SqlQ(strName) + " and Value like " + StringTool.SqlQ("%" + strValue + "%") + ")";
                    }
                }
            }

            #endregion

            #region ��չ������

            //����������
            if (ddlSchemaItemJB.SelectedItem.Text.Trim() != string.Empty && txtItemValue.Text.Trim().Length > 0)
            {
                ht.Add(ddlSchemaItemJB.SelectedValue.Trim(), txtItemValue.Text.Trim());
            }

            //����������
            if (ddlSchemaItemGL.SelectedItem.Text.Trim() != string.Empty)
            {
                ht.Add(ddlSchemaItemGL.SelectedValue.Trim(), (chkItemValue.Checked == true ? "1" : "0"));
            }

            //����������
            if (ddlPulldownItem.SelectedItem.Text.Trim() != string.Empty && ddlPulldownValue.SelectedItem.Text.Trim() != string.Empty)
            {
                ht.Add(ddlPulldownItem.SelectedValue.Trim(), ddlPulldownValue.SelectedValue.Trim());
            }

            //����������
            if (ddlSchemaItemDT.SelectedItem.Text.Trim() != string.Empty && dtServiceTime.dateTimeString != "")
            {
                ht.Add(ddlSchemaItemDT.SelectedValue.Trim(), dtServiceTime.dateTime.ToShortDateString().Trim());
            }

            //����������
            if (ddlSchemaItemDP.SelectedItem.Text.Trim() != string.Empty && DeptPicker1.DeptID != 0)
            {
                ht.Add(ddlSchemaItemDP.SelectedValue.Trim(), DeptPicker1.DeptID.ToString());
            }

            //�û�������
            if (ddlSchemaItemUS.SelectedItem.Text.Trim() != string.Empty && UserPicker1.UserID != 0)
            {
                ht.Add(ddlSchemaItemUS.SelectedValue.Trim(), UserPicker1.UserID.ToString());
            }
            #endregion

            Equ_DeskDP ee = new Equ_DeskDP();

            //2008-05-12���������  1 �ǰ����ͻ���Ϣ �� ����λ ����һ�ֲ�ѯ,û�а����ͻ���Ϣ������һ�ֲ�ѯ,
            //if (ShowMastCust.Visible == true)
            //{
            if (ddltMastCustID.SelectedItem != null && ddltMastCustID.SelectedItem.Text.Trim() != string.Empty)
            {
                sWhere += " And MastCustID=" + ddltMastCustID.SelectedValue.Trim();
                blnSpec = true;
            }
            //}

            //�ͻ���Ϣ
            if (txtCust.Text.Trim() != string.Empty)
            {
                //Ӣ����
                sWhere += " And (nvl(FullName,'') like " + StringTool.SqlQ("%" + txtCust.Text.Trim() + "%");
                //������
                sWhere += " OR nvl(ShortName,'') like " + StringTool.SqlQ("%" + txtCust.Text.Trim() + "%");
                //����
                sWhere += " OR nvl(CustomCode,'') like " + StringTool.SqlQ("%" + txtCust.Text.Trim() + "%");
                //�绰
                sWhere += " OR nvl(Tel1,'') like " + StringTool.SqlQ("%" + txtCust.Text.Trim() + "%");
                //EMAIL
                sWhere += " OR nvl(Email,'') like " + StringTool.SqlQ("%" + txtCust.Text.Trim() + "%");

                sWhere += ")";

                blnSpec = true;
            }

            if (blnSpec == true)
            {
                //�ʲ�����
                if (txtName.Text.Trim() != string.Empty)
                {
                    sWhere += " And Name like " + StringTool.SqlQ("%" + txtName.Text.Trim() + "%");
                }

                //�ʲ����
                if (txtCode.Text.Trim() != string.Empty)
                {
                    sWhere += " And Code like " + StringTool.SqlQ("%" + txtCode.Text.Trim() + "%");
                }

                string strFullID = Equ_SubjectDP.GetSubjectFullID(long.Parse(sCalalogID));
                if (sCalalogID == "1" || sCalalogID == "-1")
                { }
                else
                {
                    sWhere += " And FullID = " + StringTool.SqlQ(strFullID);
                }

                //�ʲ�״̬
                if (!string.IsNullOrEmpty(ctrCataEquStatus.CatelogValue.Trim()))
                    sWhere += " and EquStatusID= " + ctrCataEquStatus.CatelogID.ToString();

                if (ViewState["DataTableSpec"] != null && ViewState["DataTableSpec"].ToString() == "True")
                {
                    dt = ee.GetDataTableSpec(sWhere, sOrder, ht, 1000000000, 1, ref iRowCount);
                    ViewState["DataTableSpec"] = null;
                }
                else
                {
                    dt = ee.GetDataTableSpec(sWhere, sOrder, ht, this.cpfECustomerInfo.PageSize, this.cpfECustomerInfo.CurrentPage, ref iRowCount);
                }
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

                if (sCalalogID == "")
                {
                    sCalalogID = "-1";

                }
                string strFullID = Equ_SubjectDP.GetSubjectFullID(long.Parse(sCalalogID));
                if (sCalalogID == "1" || sCalalogID == "-1")
                { }
                else
                {
                    sWhere += " And FullID = " + StringTool.SqlQ(strFullID);
                }

                //�ʲ�״̬
                if (!string.IsNullOrEmpty(ctrCataEquStatus.CatelogValue.Trim()))
                    sWhere += " and EquStatusID= " + ctrCataEquStatus.CatelogID.ToString();

                if (ViewState["DataTableSpec"] != null && ViewState["DataTableSpec"].ToString() == "True")
                {
                    dt = ee.GetDataTableSpec(sWhere, sOrder, ht, 1000000000, 1, ref iRowCount);
                    ViewState["DataTableSpec"] = null;
                }
                else
                {
                    dt = ee.GetDataTableSpec(sWhere, sOrder, ht, this.cpfECustomerInfo.PageSize, this.cpfECustomerInfo.CurrentPage, ref iRowCount);
                }
            }
            return dt;
        }
        #endregion

        #region ControlPage1_On_PostBack
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ControlPage1_On_PostBack(object sender, EventArgs e)
        {
            Bind("");
        }
        #endregion

        #region  dgEqu_Desk_ItemCommand
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgEqu_Desk_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {            
                if (!IsSelect)
                    Response.Redirect("frmEqu_DeskEdit.aspx?id=" + e.Item.Cells[1].Text.ToString() + "&subjectid=" + CatalogID);
                else
                {
                    string id = e.Item.Cells[1].Text.Replace("&nbsp;", "");
                    string sWhere = " And id = " + id;

                    Equ_DeskDP ec = new Equ_DeskDP();
                    DataTable dt = ec.GetDataTable(sWhere, "");
                   
                     sbText.Append("<script>");
                     string requestType = "";
                     if (Request["TypeFrm"] != null) 
                     {
                         requestType = Request.QueryString["TypeFrm"].ToString();
                     }
                     else if (Request["TypeFrom"] != null) {
                         requestType = Request.QueryString["TypeFrom"].ToString();
                     }

                    switch (requestType)
                    {
                        case "frm_Change_AdvancedCondition":
                            AdvancedCondition(dt);                          
                            break;
                        case "frm_ChangeBase":
                            ChangeBase(dt);
                            break;
                        case "DemandBase":
                            DemandBase(dt);
                            break;
                    }
                    sbText.Append("window.close();");


                    //// �رմ���
                    //sbText.Append("top.close();");
                    sbText.Append("</script>");
                    // ��ͻ��˷���
                    Page.RegisterStartupScript(DateTime.Now.Ticks.ToString(), sbText.ToString());
                    //Response.Write(sbText.ToString());
                }
            }
            if (e.CommandName == "look")
            {
                if (IsSelect)
                {
                    Response.Redirect("frmEqu_DeskEdit.aspx?IsSelect=1&id=" + e.Item.Cells[1].Text.ToString() + "&subjectid=" + CatalogID + "&FlowID=" + FlowID);
                }
                else
                {
                    Response.Redirect("frmEqu_DeskEdit.aspx?id=" + e.Item.Cells[1].Text.ToString() + "&subjectid=" + CatalogID + "&FlowID=" + FlowID);
                }
            }
            if (e.CommandName == "link")
            {
                Response.Redirect("frmEquRelMain.aspx?ID=" + e.Item.Cells[1].Text.ToString() + "&subjectid=" + CatalogID);
            }
        }

        private void AdvancedCondition(DataTable dt)
        {
            sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtEquipmentName", dt.Rows[0]["name"].ToString());   //�豸����
            sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidEquName", dt.Rows[0]["name"].ToString());     //�豸����
            sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidEqu", dt.Rows[0]["id"].ToString()); 
        }

        private void ChangeBase(DataTable dt)
        {
            sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidAddID", dt.Rows[0]["id"].ToString());
            sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtAddEquName", dt.Rows[0]["name"].ToString());     //�豸����
            sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidAddEquName", dt.Rows[0]["name"].ToString());     //
            sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "lblAddCode", dt.Rows[0]["code"].ToString());     //
            sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidAddCode", dt.Rows[0]["code"].ToString());     //�ʲ�Ŀ¼����
            sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "lblDept", dt.Rows[0]["partbranchname"].ToString());     //
                    
        }       
        #endregion

        #region dgEqu_Desk_ItemDataBound
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgEqu_Desk_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Button lnkedit = (Button)e.Item.FindControl("lnkedit");
                LinkButton lnkEquName = (LinkButton)e.Item.FindControl("lnkEquName");
                Label lblEquName = (Label)e.Item.FindControl("lblEquName");
                if (IsSelect)
                {
                    lnkedit.Text = "ѡ��";
                    lnkEquName.Visible = false;
                    lblEquName.Visible = true;
                }

                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");

                if (!IsSelect)
                {
                    e.Item.Attributes.Add("ondblclick", "window.open('../EquipmentManager/frmEqu_DeskEdit.aspx?id=" + e.Item.Cells[1].Text.ToString() + "&subjectid=" + CatalogID + "&randomid='+GetRandom(),'subjectinfo','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
                }
                else
                {
                    string id = e.Item.Cells[1].Text.Replace("&nbsp;", "");
                    string sWhere = " And id = " + id;

                    Equ_DeskDP ec = new Equ_DeskDP();
                    DataTable dt = ec.GetDataTable(sWhere, "");
                    Json json = new Json(dt);

                    string jsonstr = "{record:" + json.ToJson() + "}";
                    //�����Ǹ��´���
                    string requestType="";
                    if (Request["TypeFrm"] != null)
                    {
                        requestType = Request.QueryString["TypeFrm"].ToString();
                    }

                    switch (requestType)
                    {
                        case "CST_Issue_Base":
                           // AdvancedCondition(dt);
                            e.Item.Attributes.Add("onclick", "ServerOndblclick_CST_Issue_Base("+jsonstr+")");
                            break;
                        case "frm_ChangeBase":
                            e.Item.Attributes.Add("onclick", "ServerOndblclickChangeBase("+jsonstr+")");
                            //ChangeBase(dt);
                            break;
                        case "frmProblemSolve":
                            e.Item.Attributes.Add("onclick", "ServerOndblclickfrmProblemSolve("+jsonstr+")");
                            break;
                        case "CST_Issue_Base_Fast":
                            lnkedit.Attributes.Add("onclick", "cst_issue_Base_fastServe(" + jsonstr + ")");
                           // e.Item.Attributes.Add("onclick","cst_issue_Base_fastServe("+jsonstr+")");
                            break;
                        case "frm_Change_AdvancedCondition":
                            lnkedit.Attributes.Add("onclick", "ServerAdvancedCondition(" + jsonstr + ")");
                            break;
                        case "ServerCst_Issue":
                            lnkedit.Attributes.Add("onclick","ServerCst_Issue_Serivce("+jsonstr+");");
                            break;
                        case "CST_Issue_Base_self":
                            lnkedit.Attributes.Add("onclick", "CTS_Issue_Base_Self("+jsonstr+")");
                            break;
                        case "DemandBase":
                            lnkedit.Attributes.Add("onclick", "DemandBase(" + jsonstr + ")");
                            break;
                        default:
                            // ��ͻ��˷���
                            e.Item.Attributes.Add("onclick", "ServerOndblclick(" + jsonstr + ");");
                            break;

                    }
                  
                   

                }

            }

        }
        #endregion

        #region ����ʲ�����ʱ�������ʲ��������
        /// <summary>
        /// ����ʲ�����ʱ�������ʲ��������
        /// </summary>
        /// <param name="decEquID"></param>
        protected string GetEquDetail(decimal decEquID)
        {
            string strUrl = string.Empty;

            strUrl = "javascript:openobj.open('" + "frmEqu_DeskEdit.aspx?IsTanChu=true&IsSelect=1&id=" + decEquID.ToString() + "&subjectid=" + CatalogID + "&FlowID=" + FlowID + "','','scrollbars=yes,resizable=yes');event.returnValue = false;";

            return strUrl;
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
                int j = 0;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i > 1 && i < 8)
                    {
                        if (IsSelect)
                            j = i - 2;
                        else
                            j = i - 1;
                        if (i == 7)
                        {
                            e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",1);");
                        }
                        else
                        {
                            e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                        }
                    }
                }
            }
        }
        #endregion

        #region �¼���¼
        /// <summary>
        /// �¼���¼
        /// </summary>
        /// <param name="lngNoticeID"></param>
        /// <returns></returns>
        protected string GetServiceUrl(decimal lngID)
        {
            //��ʱû�����ҳ
            string sUrl = "";
            sUrl = "javascript:window.open('../AppForms/frmIssueList.aspx?NewWin=true&ID=0&EquID=" + lngID.ToString() + "','newwindow','scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');";
            return sUrl;
        }
        #endregion

        #region Ѳ����ʷ
        /// <summary>
        /// Ѳ����ʷ
        /// </summary>
        /// <param name="lngNoticeID"></param>
        /// <returns></returns>
        protected string GetUrl(decimal lngID)
        {
            //��ʱû�����ҳ
            string sUrl = "";
            sUrl = "javascript:window.open('frm_Equ_PatrolList.aspx?NewWin=true&EquID=" + lngID.ToString() + "','newwindow','scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');";
            return sUrl;
        }
        #endregion

        #region �������������Ƹı�ʱ���ı��Ӧ�İ���������
        /// <summary>
        /// �������������Ƹı�ʱ���ı��Ӧ�İ���������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPulldownItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            //����������FieldID
            string strItemID = ddlPulldownItem.SelectedItem.Value;

            //�������ID��ȡ��Ӧ�İ�����
            DataTable dt = CommonDP.GetCatasByParentID(CatalogID, strItemID);
            ddlPulldownValue.DataSource = dt;
            ddlPulldownValue.DataTextField = "CatalogName";
            ddlPulldownValue.DataValueField = "CatalogID";
            ddlPulldownValue.DataBind();
            ddlPulldownValue.Items.Insert(0, new ListItem("", ""));
        }
        #endregion

        #region �������ҳ��ѡ���ʲ� ����ǰ 2013-04-27
        /// <summary>
        /// �������ҳ��ѡ���ʲ�
        /// </summary>
        /// <param name="dt"></param>
        private void DemandBase(DataTable dt)
        {
            sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtEqu", dt.Rows[0]["name"].ToString());   //�ʲ�����
            sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidEquName", dt.Rows[0]["name"].ToString());     //�ʲ�����
            sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidEqu", dt.Rows[0]["id"].ToString());    //�ʲ�ID

            sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidListName", dt.Rows[0]["listname"].ToString());     //�ʲ�Ŀ¼
            sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidListID", dt.Rows[0]["listid"].ToString());     //�ʲ�Ŀ¼ID
        }
        #endregion
    }
}

