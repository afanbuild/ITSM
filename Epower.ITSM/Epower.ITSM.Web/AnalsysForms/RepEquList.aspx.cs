/****************************************************************************
 * 
 * description:�ʲ���ϸ��
 * 
 * 
 * 
 * Create by:
 * Create Date:2009-02-11
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
using System.Drawing;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;
using System.Xml;

namespace Epower.ITSM.Web.AnalsysForms
{
    public partial class RepEquList : BasePage
    {
        #region ���ø����尴ť�¼�SetParentButtonEvent
        /// <summary>
        /// ���ø����尴ť�¼�

        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_ExportExcel_Click += new Global_BtnClick(btnToExcel_Click);
            this.Master.ShowExportExcelButton(true);
        }


        #endregion

        #region ҳ���ʼ�� Page_Load
        /// <summary>
        /// ҳ���ʼ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            SetParentButtonEvent();
            this.CtrFCDDealStatus.mySelectedIndexChanged += new EventHandler(CtrFCDDealStatus_mySelectedIndexChanged);
            cpCST_Issue.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(LoadData);
            if (!IsPostBack)
            {
                //��ȡdbo�ؼ���ȱʡֵ
                SetMastCustomDboValue();
                
                LoadData();
            }
            else
            {
                if (Request.Form["__EVENTTARGET"] == "datarange")
                {
                    LoadData();
                }
            }
        }

        #endregion

        #region ѡ�����״̬ʱ�����¼� CtrFCDDealStatus_mySelectedIndexChanged
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CtrFCDDealStatus_mySelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }
        #endregion 

        #region �������� LoadData
        /// <summary>
        /// ��������
        /// </summary>
        private void LoadData()
        {
            int iRowCount = 0;
            string sWhere = string.Empty;

            if (CtrFCDDealStatus.CatelogID != CtrFCDDealStatus.RootID && CtrFCDDealStatus.CatelogID > 0)
            {
                sWhere += " And EquStatusID=" + CtrFCDDealStatus.CatelogID.ToString();
            }
            if (dpdMastShortName.SelectedValue.Trim() != "0")
            {
                sWhere += " And nvl(Costom,0) In (select ID from Br_ECustomer where MastCustID=" + dpdMastShortName.SelectedValue.Trim() + ")";
            }
            Equ_DeskDP ee = new Equ_DeskDP();
            DataTable dt = ee.GetDataTableCust(sWhere, "order by Br_ECustomer.ID Desc", this.cpCST_Issue.PageSize, this.cpCST_Issue.CurrentPage, ref iRowCount);
            dgMaterialStat.DataSource = dt.DefaultView;
            dgMaterialStat.DataBind();

            this.cpCST_Issue.RecordCount = iRowCount;
            this.cpCST_Issue.Bind();

            //InitEquItems();  //��ʼ��������Ϣ
        }
        #endregion

        #region ����Excel btnToExcel_Click
        /// <summary>
        /// ����Excel
        /// </summary>
        protected void btnToExcel_Click()
        {
            ToExcel();
        }

        #region ��д����excel��������ģ�淽ʽ��
        /// <summary>
        /// ��д����excel��������ģ�淽ʽ��
        /// </summary>
        private void ToExcel()
        {
            int iRowCount = 0;
            string sWhere = string.Empty;

            if (!(CtrFCDDealStatus.CatelogID == CtrFCDDealStatus.RootID || CtrFCDDealStatus.CatelogID == 0))
            {
                sWhere += " And EquStatusID=" + CtrFCDDealStatus.CatelogID.ToString();
            }
            if (dpdMastShortName.SelectedValue.Trim() != "0")
            {
                sWhere += " And nvl(Costom,0) In (select ID from Br_ECustomer where MastCustID=" + dpdMastShortName.SelectedValue.Trim() + ")";
            }
            Equ_DeskDP ee = new Equ_DeskDP();
            DataTable dt = ee.GetDataTableEquDetails(sWhere, "order by Br_ECustomer.ID Desc", 10000000, 1, ref iRowCount);

            dt.Columns.Add("Props", typeof(string));                //������չ������

            //ѭ���ʲ�ID���õ���չ����
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string strEquID = row["ID"].ToString();           //�ʲ�ID

                    //��ȡ�ʲ�ID�µ���չ����
                    string strProps = Equ_DeskDP.GetPropsByEquID(strEquID);
                    row["Props"] = strProps;
                }
            }

            Epower.ITSM.Web.Common.ExcelExport.ExportEquDetailLists(this, dt, Session["UserID"].ToString());
        }
        #endregion

        public string getConfigureValue(string ConfigureValue)
        {
            Equ_SchemaItemsDP ee = new Equ_SchemaItemsDP();
            Hashtable _fields = ee.GetItemsAllFields(-1);   //��ȡ���µ����������
            XmlDocument xmldoc = new XmlDocument();
            XmlNodeList xmllist;
            string strXml = string.Empty;
            string fieldid = string.Empty;
            string fieldName = string.Empty;
            string fieldValue = string.Empty;
            string iType = string.Empty;
            string showBaseValue = string.Empty;
            string showRelValue = string.Empty;

            showBaseValue = "�������ã�";
            showRelValue = "�������ã�";
            strXml = ConfigureValue.Trim();
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
            ConfigureValue = showBaseValue + "    " + showRelValue;
            if (ConfigureValue == "�������ã�    �������ã�")
                ConfigureValue = " ";
            return ConfigureValue;
        }
        #endregion   

        #region ��������λ
        
        /// <summary>
        /// ��������λ
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

            dpdMastShortName.Items.Insert(0, new ListItem("ȫ��", "0"));

            dt.Dispose();
        }

        #endregion

        #region ����λ�ı���в�ѯ
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dpdMastShortName_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        #endregion

        #region ��ȡ���µ����������
        
        /// <summary>
        /// ��ȡ���µ����������
        /// </summary>
        private void InitEquItems()
        {
            Equ_SchemaItemsDP ee = new Equ_SchemaItemsDP();
            Hashtable _fields = ee.GetItemsAllFields(-1);   //��ȡ���µ����������
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
                    showBaseValue = "�������ã�";
                    showRelValue = "�������ã�";
                    strXml = row.Cells[5].Text.Trim();
                    xmldoc.LoadXml(strXml);
                    xmllist = xmldoc.SelectNodes("Fields/Field");
                    foreach (XmlNode node in xmllist)   
                    {
                        fieldid = node.Attributes["FieldName"].Value;
                        fieldValue = node.Attributes["Value"].Value;
                        if (_fields[fieldid] != null && fieldValue!=string.Empty)
                        {
                            fieldName = _fields[fieldid].ToString().Substring(0, _fields[fieldid].ToString().Length-1);
                            iType = _fields[fieldid].ToString().Substring(_fields[fieldid].ToString().Length - 1,1);
                            if (iType == "0")
                            {
                                if (fieldValue.Trim() != string.Empty)
                                    showBaseValue += fieldName + ":" + fieldValue + " /";
                            }
                            else
                            {
                                if (fieldValue.Trim()=="1")
                                    showRelValue += fieldName + "/";
                            }
                        }
                    }
                    row.Cells[5].Text = showBaseValue + "    " + showRelValue;
                    if (row.Cells[5].Text == "�������ã�    �������ã�")
                        row.Cells[5].Text = " ";
                }
            }
        }

        #endregion

        #region gv���ݰ�
        
        /// <summary>
        /// gv���ݰ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgMaterialStat_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                e.Item.Attributes.Add("ondblclick", "window.open('../EquipmentManager/frmEqu_DeskEdit.aspx?IsSelect=1&id=" + e.Item.Cells[0].Text.ToString() + "&FlowID=0&Soure=0&newWin=0', '', 'scrollbars=yes,status=yes ,resizable=yes,width=800,height=600');");
            }
        }

        #endregion

        #region gv���
        
        /// <summary>
        /// gv���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgMaterialStat_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    int j = 0;
                    if (i < 6)
                    {
                        j = i - 1;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }

        #endregion

    }
}
