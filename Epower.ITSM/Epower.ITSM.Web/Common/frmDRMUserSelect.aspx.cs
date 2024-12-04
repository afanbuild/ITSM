/****************************************************************************
 * 
 * description:Drmϵͳ�û�ѡ��
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-08-18
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
using System.Xml;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.Common
{
    /// <summary>
    /// 
    /// </summary>
    public partial class frmDRMUserSelect : BasePage
    {
        RightEntity reTrace = null;  //Ȩ��

        #region �Ƿ��ѯ
        /// <summary>
        /// 
        /// </summary>
        protected bool IsSelect
        {
            get
            {
                if (Request["IsSelect"] != null && Request["IsSelect"] == "true")
                    return true;
                else
                    return false;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        protected string FlowID
        {
            get { if (Request["FlowID"] != null) return Request["FlowID"].ToString(); else return "0"; }
        }
        #region �����·���ַ���
        /// <summary>
        /// �����·���ַ���
        /// </summary>
        public string Opener_ClientId
        {

            get
            {
                return (Request["Opener_ClientId"] == null) ? "" : Request["Opener_ClientId"].ToString().Replace("hidClientId_ForOpenerPage", "");
            }
        }
        #endregion
        /// <summary>
        /// �ͻ�����
        /// </summary>
        protected string CustName
        {
            get
            {
                if (ViewState["CustName"] != null)
                {
                    return ViewState["CustName"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState["CustName"] = value;
            }
        }
        /// <summary>
        /// ��ַ
        /// </summary>
        protected string CustAddress
        {
            get
            {
                if (ViewState["CustAddress"] != null)
                {
                    return ViewState["CustAddress"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState["CustAddress"] = value;
            }
        }
        /// <summary>
        /// ��ϵ��
        /// </summary>
        protected string CustLinkMan
        {
            get
            {
                if (ViewState["CustLinkMan"] != null)
                {
                    return ViewState["CustLinkMan"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState["CustLinkMan"] = value;
            }
        }
        /// <summary>
        /// ��ϵ�绰
        /// </summary>
        protected string CustTel
        {
            get
            {
                if (ViewState["CustTel"] != null)
                {
                    return ViewState["CustTel"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState["CustTel"] = value;
            }
        }

        #region ���ø����尴ť�¼�SetParentButtonEvent
        /// <summary>
        /// ���ø����尴ť�¼�
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.ShowQueryButton(true);

            this.Master.Master_Button_GoHistory_Click+=new Global_BtnClick(Master_Master_Button_GoHistory_Click);
            if (IsSelect)
            {
                this.Master.ShowBackUrlButton(true);
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtonEvent();
            cpfECustomerInfo.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(Bind);
            if (!IsPostBack)
            {
                //������ʾ
                PageDeal.SetLanguage(this.Controls[0]);
                dgdrmuser.Columns[1].HeaderText = PageDeal.GetLanguageValue("LitMastShortName");
                dgdrmuser.Columns[2].HeaderText = PageDeal.GetLanguageValue("LitCustomerType");
                dgdrmuser.Columns[3].HeaderText = PageDeal.GetLanguageValue("LitCustName");
                dgdrmuser.Columns[4].HeaderText = PageDeal.GetLanguageValue("LitContact");
                dgdrmuser.Columns[5].HeaderText = PageDeal.GetLanguageValue("LitCTel");
                dgdrmuser.Columns[6].HeaderText = PageDeal.GetLanguageValue("LitCustEmail");
                dgdrmuser.Columns[7].HeaderText = PageDeal.GetLanguageValue("LitCustomCode");
                dgdrmuser.Columns[8].HeaderText = PageDeal.GetLanguageValue("LitCustAddress");

                InitDropDownList();
                if (Request["CustName"] != null)
                {
                    txtName.Text = Request["CustName"].ToString();
                    CustName = Request["CustName"].ToString();
                }
                if (Request["CustAddress"] != null)
                {
                    CustAddress = Request["CustAddress"].ToString();
                }
                if (Request["CustLinkMan"] != null)
                {
                    CustLinkMan = Request["CustLinkMan"].ToString();
                }
                if (Request["CustTel"] != null)
                {
                    CustTel = Request["CustTel"].ToString();
                }

                Bind();
            }
        }

        /// <summary>
        /// �󶨷���λ
        /// </summary>
        private void InitDropDownList()
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
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Master_Master_Button_Query_Click()
        {
            Bind();
        }

        #region  ���ɲ�ѯXML�ַ��� GetXmlValue
        /// <summary>
        /// ���ɲ�ѯXML�ַ���

        /// </summary>
        /// <returns></returns>
        private XmlDocument GetXmlValue()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlRoot = xmlDoc.CreateElement("Fields");
            XmlElement xmlEle;
            #region MastCustID
            if (ddltMastCustID.SelectedItem.Text.Trim() != string.Empty)
            {
                xmlEle = xmlDoc.CreateElement("Field");
                xmlEle.SetAttribute("FieldName", "MastCustID");
                xmlEle.SetAttribute("Value", ddltMastCustID.SelectedValue.Trim());
                xmlRoot.AppendChild(xmlEle);
            }
            #endregion
            #region CustomerType
            if (ctrFCDServiceType.CatelogValue.ToString() != string.Empty)
            {
                xmlEle = xmlDoc.CreateElement("Field");
                xmlEle.SetAttribute("FieldName", "CustomerType");
                xmlEle.SetAttribute("Value", ctrFCDServiceType.CatelogID.ToString().Trim());
                xmlRoot.AppendChild(xmlEle);
            }
            #endregion
            #region client_name
            if (txtName.Text != "")
            {
                xmlEle = xmlDoc.CreateElement("Field");
                xmlEle.SetAttribute("FieldName", "client_name");
                xmlEle.SetAttribute("Value", txtName.Text.Replace("'", "''"));
                xmlRoot.AppendChild(xmlEle);
            }
            #endregion
            #region client_address
            if (txtaddress.Text != "")
            {
                xmlEle = xmlDoc.CreateElement("Field");
                xmlEle.SetAttribute("FieldName", "client_address");
                xmlEle.SetAttribute("Value", txtaddress.Text.Replace("'", "''"));
                xmlRoot.AppendChild(xmlEle);
            }
            #endregion
            #region client_contact
            if (txtcontract.Text != "")
            {
                xmlEle = xmlDoc.CreateElement("Field");
                xmlEle.SetAttribute("FieldName", "client_contact");
                xmlEle.SetAttribute("Value", txtcontract.Text.Replace("'", "''"));
                xmlRoot.AppendChild(xmlEle);
            }
            #endregion
            #region client_phone
            if (txttel.Text != "")
            {
                xmlEle = xmlDoc.CreateElement("Field");
                xmlEle.SetAttribute("FieldName", "client_phone");
                xmlEle.SetAttribute("Value", txttel.Text.Replace("'", "''"));
                xmlRoot.AppendChild(xmlEle);
            }
            #endregion
            #region txtCustomCode
            if (txtCustomCode.Text != "")
            {
                xmlEle = xmlDoc.CreateElement("Field");
                xmlEle.SetAttribute("FieldName", "customcode");
                xmlEle.SetAttribute("Value", txtCustomCode.Text.Replace("'", "''"));
                xmlRoot.AppendChild(xmlEle);
            }
            #endregion
            xmlDoc.AppendChild(xmlRoot);

            return xmlDoc;
        }
        #endregion

        #region ���ݼ��� Bind
        /// <summary>
        /// ���ݼ���
        /// </summary>
        private void Bind()
        {
            int iRowCount = 0;
            string sServiceCustom = "1";
            DataTable dt;
            XmlDocument xmlDoc = GetXmlValue();

            //writelog(DateTime.Now.ToString());

            DRMUserDP ee = new DRMUserDP();
            dt = ee.GetDRMUser(xmlDoc.InnerXml, long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString())
           , long.Parse(Session["UserOrgID"].ToString()), reTrace, sServiceCustom
           , this.cpfECustomerInfo.PageSize, this.cpfECustomerInfo.CurrentPage, ref iRowCount);

            dgdrmuser.DataSource = dt;
            dgdrmuser.DataBind();

            this.cpfECustomerInfo.RecordCount = iRowCount;
            this.cpfECustomerInfo.Bind();
        }
        #endregion

        /// <summary>
        /// ����
        /// </summary>
        void Master_Master_Button_GoHistory_Click()
        {
            Response.Write("<script>window.close();</script>");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgProduct_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                StringBuilder sbText = new StringBuilder();
                sbText.Append("<script>");
                sbText.Append("var arr = new Array();");

                // ���⣨���ģ�
                sbText.Append("arr[1] ='" + ((Label)e.Item.FindControl("client_name")).Text.Trim().Replace("&nbsp;", "") + "';");   //�û�����

                sbText.Append("arr[2] ='" + e.Item.Cells[4].Text.Trim().Replace("&nbsp;", "") + "';");   //��ϵ��

                sbText.Append("arr[3] ='" + e.Item.Cells[5].Text.Trim().Replace("&nbsp;", "") + "';");   //��ϵ�˵绰

                sbText.Append("arr[4] ='" + e.Item.Cells[0].Text.Trim().Replace("&nbsp;", "") + "';");   //id

                sbText.Append("arr[5] ='" + e.Item.Cells[1].Text.Trim().Replace("&nbsp;", "") + "';");   //����

                sbText.Append("arr[6] ='" + e.Item.Cells[2].Text.Trim().Replace("&nbsp;", "") + "';");   //�ͻ�����

                sbText.Append("arr[7] ='" + e.Item.Cells[8].Text.Trim().Replace("&nbsp;", "") + "';");   //��ַ

                sbText.Append("arr[13] ='" + e.Item.Cells[6].Text.Trim().Replace("&nbsp;", "") + "';");   //�����ʼ�

                sbText.Append("arr[14] ='" + e.Item.Cells[7].Text.Trim().Replace("&nbsp;", "") + "';");   //�ͻ�����

                sbText.Append("arr[15] ='" + e.Item.Cells[1].Text.Trim().Replace("&nbsp;", "") + "';");   //����λ

                //���ݿͻ�IDȡ���ʲ���Ϣ
                Equ_DeskDP ee = new Equ_DeskDP();
                ee = ee.GetEquByCustID(long.Parse(e.Item.Cells[0].Text.Trim().Replace("&nbsp;", "")));
                sbText.Append("arr[8] ='" + ee.ID.ToString() + "';");   // �ʲ�ID
                sbText.Append("arr[9] ='" + ee.Name.ToString() + "';");   // �豸����
                sbText.Append("arr[10] ='" + ee.Positions.ToString() + "';");   // �豸λ��
                sbText.Append("arr[11] ='" + ee.Code.ToString() + "';");   // �豸����
                sbText.Append("arr[12] ='" + ee.SerialNumber.ToString() + "';");   // �豸SN

                sbText.Append("arr[16] ='" + ee.ListID.ToString() + "';");       // �ʲ�Ŀ¼ID��ԭ�����豸Ʒ�ơ�
                sbText.Append("arr[17] ='" + ee.ListName.ToString() + "';");     // �ʲ�Ŀ¼���ơ�ԭ�����豸�ͺš�

                //=====zxl==
                sbText.Append(" var value=arr; if(value !=null){ if(value.length>1){ ");
                //sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtEquName", dt.Rows[0]["name"].ToString());  
                sbText.AppendFormat("window.opener.document.all.{0}.value={1};", Opener_ClientId + "txtCustAddr", "value[1]");
                sbText.AppendFormat("window.opener.document.all.{0}.value={1};", Opener_ClientId + "hidCust", "value[1]");
                sbText.AppendFormat("window.opener.document.all.{0}.value={1};", Opener_ClientId + "hidCustID", "value[4]");

                sbText.AppendFormat("window.opener.document.all.{0}.value={1};", Opener_ClientId + "txtEqu", "value[9]");
                sbText.AppendFormat("window.opener.document.all.{0}.value={1};", Opener_ClientId + "hidEquName", "value[9]");
                sbText.AppendFormat("window.opener.document.all.{0}.value={1};", Opener_ClientId + "hidEqu", "value[8]");

                sbText.Append("} }");


               // sbText.Append("window.parent.returnValue = arr;");

                //=====zxl===
                // �رմ���
                sbText.Append("top.close();");
                sbText.Append("</script>");
                // ��ͻ��˷���
                Page.RegisterStartupScript(DateTime.Now.ToString(), sbText.ToString());
                Response.Write(sbText.ToString());
            }
            else if (e.CommandName == "look")
            {
                Response.Redirect("frmcusShow.aspx?id=" + e.Item.Cells[0].Text.Trim() + "&FlowID=" + FlowID);
            }
            else if (e.CommandName == "edit")
            {
                Response.Redirect("../AppForms/frmBr_ECustomerEdit.aspx?QuickNew=1&id=" + e.Item.Cells[0].Text.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddltBuild_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind();
        }

        protected void dgdrmuser_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");

                string sID = e.Item.Cells[1].Text.Trim();
                //string id = DataBinder.Eval(e.Item.DataItem, "ID").ToString();  sID �� idָ����ͬһ��id����ȡ��ʽ��ͬ

                string name = DataBinder.Eval(e.Item.DataItem, "client_name").ToString();

                if (!IsSelect)
                {
                    e.Item.Attributes.Add("ondblclick", "window.open('../AppForms/frmBr_ECustomerEdit.aspx?QuickNew=1&id=" + e.Item.Cells[0].Text.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
                }
                else
                {
                    // �û�����
                    string value1 = ((Label)e.Item.FindControl("client_name")).Text.Trim().Replace("&nbsp;", "");
                    // �û���ַ
                    string value2 = e.Item.Cells[4].Text.Trim().Replace("&nbsp;", "");
                    // ��ϵ��
                    string value3 = e.Item.Cells[5].Text.Trim().Replace("&nbsp;", "");
                    // ��ϵ�绰
                    string value4 = e.Item.Cells[0].Text.Trim().Replace("&nbsp;", "");
                    // �û�����
                    string value5 = e.Item.Cells[1].Text.Trim().Replace("&nbsp;", "");
                    // �����ʼ�
                    string value6 = e.Item.Cells[2].Text.Trim().Replace("&nbsp;", "");
                    string value7 = e.Item.Cells[8].Text.Trim().Replace("&nbsp;", "");
                    string value8 = e.Item.Cells[6].Text.Trim().Replace("&nbsp;", "");
                    string value9 = e.Item.Cells[7].Text.Trim().Replace("&nbsp;", "");
                    string value10 = e.Item.Cells[1].Text.Trim().Replace("&nbsp;", "");

                    //���ݿͻ�IDȡ���ʲ���Ϣ
                    Equ_DeskDP ee = new Equ_DeskDP();
                    ee = ee.GetEquByCustID(long.Parse(e.Item.Cells[0].Text.Trim().Replace("&nbsp;", "")));

                    string value11 = ee.ID.ToString().Trim();                  // �ʲ�ID
                    string value12 = ee.Name.ToString().Trim();                // �豸����
                    string value13 = ee.Positions.ToString().Trim();           // �豸λ��
                    string value14 = ee.Code.ToString().Trim();                // �豸����
                    string value15 = ee.SerialNumber.ToString().Trim();        // �豸SN
                    string value16 = ee.ListID.ToString().Trim();              // �ʲ�Ŀ¼ID��ԭ�����豸Ʒ�ơ�
                    string value17 = ee.ListName.ToString().Trim();            // �ʲ�Ŀ¼���ơ�ԭ�����豸�ͺš�

                    // ��ͻ��˷���
                    e.Item.Attributes.Add("ondblclick", "ServerOndblclick('" + value1 + "','" + value2 + "','" + value3 + "','" + value4 + "','" + value5 + "','" + value6 + "','" + value7 + "','" + value8 + "','" + value9 + "','" + value10 + "','" + value11 + "','" + value12 + "','" + value13 + "','" + value14 + "','" + value15 + "','" + value16 + "','" + value17 + "');");

                }

                ((Label)e.Item.FindControl("client_name")).Attributes.Add("onmouseover", "ShowDetailsInfo(this," + DataBinder.Eval(e.Item.DataItem, "client_id").ToString() + ",400);");
            }
        }

        protected void dgdrmuser_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header) {
                DataGrid dg = (DataGrid)sender;
                int j = 0;
                for (int i = 0; i < e.Item.Cells.Count; i++) {
                    if (i > 0 && i < 9) {
                        j = i - 1;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
    
        
    }
}