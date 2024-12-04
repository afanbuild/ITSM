using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Epower.DevBase.Organization;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;
using System.Text;

using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.MyDestop
{
    /// <summary>
    /// frmQuickLocateCust ��ժҪ˵����
    /// </summary>
    public partial class frmQuickLocateCustAjax : BasePage
    {
        #region �Ƿ��ѯIsSelect
        protected bool IsSelect
        {
            get
            {
                if (Request["IsSelect"] != null && Request["IsSelect"] == "true")
                    return true;
                else return false;
            }
        }
        #endregion

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.TableVisible = false;
        }
        #endregion

        #region �ж����ĸ�ҳ�洫�ݹ����� ����ǰ 2013-04-24
        /// <summary>
        /// �ж����ĸ�ҳ�洫�ݹ����� 1Ĭ���¼��� 2�¼����߼���ѯ 3�������
        /// </summary>
        public string RequestPageType
        {
            set { ViewState["RequestPageType"] = value; }
            get { return ViewState["RequestPageType"].ToString(); }
        }
        #endregion

        #region ��ô������Ĳ��������жϸ�����Ŀؼ�id�����Ƶ���ͬ����
        /// <summary>
        /// ��ô������Ĳ��������жϸ�����Ŀؼ�id�����Ƶ���ͬ����
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            SetParentButtonEvent();
            if (!IsPostBack)
            {
                SetHeaderText();

                RequestPageType = (Request["PageType"] == null) ? "1" : Request["PageType"].ToString();

                if (Request["Name"] != null)
                    BindData(Request["Name"].ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindData(string sCust)
        {
            string sSql = " and (E.ShortName like " + StringTool.SqlQ("%" + sCust + "%")
                       + " or E.FullName like " + StringTool.SqlQ("%" + sCust + "%")
                       + " or E.CustomCode like " + StringTool.SqlQ("%" + sCust + "%")
                       + " or E.LinkMan1 like " + StringTool.SqlQ("%" + sCust + "%")
                       + " or Email like " + StringTool.SqlQ("%" + sCust + "%")
                       + " or E.Tel1 like " + StringTool.SqlQ("%" + sCust + "%") + ")";
            Br_ECustomerDP ee = new Br_ECustomerDP();
            DataTable dt = ee.GetCustomerServic(sSql, string.Empty);          

            this.dgUserInfo.DataSource = dt.DefaultView;
            this.dgUserInfo.DataBind();
        }

        /// <summary>
        /// ������ͷ���� ����ǰ 2013-05-24
        /// </summary>
        void SetHeaderText()
        {
            dgUserInfo.Columns[1].HeaderText = PageDeal.GetLanguageValue("Custom_CustName");
            dgUserInfo.Columns[2].HeaderText = PageDeal.GetLanguageValue("Custom_CustAddress");
            dgUserInfo.Columns[3].HeaderText = PageDeal.GetLanguageValue("Custom_Contact");
            dgUserInfo.Columns[4].HeaderText = PageDeal.GetLanguageValue("Custom_CTel");
            dgUserInfo.Columns[5].HeaderText = PageDeal.GetLanguageValue("Custom_CustomCode");
            dgUserInfo.Columns[6].HeaderText = PageDeal.GetLanguageValue("Custom_CustEmail");
            dgUserInfo.Columns[7].HeaderText = PageDeal.GetLanguageValue("Custom_MastCustName");
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
            this.dgUserInfo.ItemCreated += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgUserInfo_ItemCreated);

        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgUserInfo_ItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    // (DataView)e.Item.NamingContainer;
                    if (i > 0 && i < 8)
                    {
                        int j = i - 1;   //ע��,��Ϊǰ����һ�����ɼ�����
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgUserInfo_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {

                string client_id = e.Item.Cells[0].Text.Replace("&nbsp;", "");
                string sWhere = " And id = " + client_id;
                Br_ECustomerDP ec = new Br_ECustomerDP();
                DataTable dt = ec.GetDataTableAjax(sWhere, "");
                Json json = new Json(dt);


                if (RequestPageType == "2")
                {
                    StringBuilder strText = new StringBuilder();
                    strText.Append("<script>");
                    strText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtCustName", dt.Rows[0]["shortname"].ToString());
                    strText.Append("window.close();");
                    strText.Append("</script>");
                    Page.RegisterStartupScript(DateTime.Now.Ticks.ToString(), strText.ToString());
                    return;
                }

                //�������ҳ���ȡ�ͻ���Ϣ
                if (RequestPageType == "3")
                {
                    string strjson = "{record:" + json.ToJson() + "}";

                    StringBuilder strText = new StringBuilder();
                    strText.Append("<script>");
                    strText.Append("var arr = " + strjson + ";");
                    strText.Append("if(arr != '')");
                    strText.Append(" {");
                    strText.Append(" var json = arr;");
                    strText.Append(" var record=json.record;");

                    strText.Append(" for(var i=0; i < record.length; i++)");
                    strText.Append("  {");
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "txtCustAddr').value = record[i].shortname; ");  //�ͻ�
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidCust').value = record[i].shortname;");
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "txtAddr').value = record[i].address;");
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidAddr').value = record[i].address;");
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "txtContact').value = record[i].linkman1;"); //��ϵ��
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidContact').value = record[i].linkman1;"); //��ϵ��
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "txtCTel').value = record[i].tel1;"); //��ϵ�˵绰
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidTel').value = record[i].tel1;"); //��ϵ�˵绰
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidCustID').value = record[i].id;"); //�ͻ�ID��
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "lblCustDeptName').innerHTML = record[i].custdeptname;"); //��������
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "lblEmail').innerHTML = record[i].email;"); //�����ʼ�
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "lblMastCust').innerHTML = record[i].mastcustname;"); //����λ
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidCustDeptName').value = record[i].custdeptname;"); //��������
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidCustEmail').value = record[i].email;"); //�����ʼ�
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidMastCust').value = record[i].mastcustname;"); //����λ
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "lbljob').innerHTML = record[i].job;"); //ְλ
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidjob').value = record[i].job;"); //ְλ

                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "txtEqu').value = record[i].equname;"); //�ʲ�����
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidEquName').value = record[i].equname;"); //�ʲ�����
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidEqu').value = record[i].equid;"); //�ʲ�id                    
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidListName').value = record[i].listname;"); //�ʲ�Ŀ¼
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidListID').value = record[i].listid;"); //�ʲ�Ŀ¼
                    strText.Append(" }");
                    strText.Append(" }");
                    strText.Append("window.close();");
                    strText.Append("</script>");
                    Page.RegisterStartupScript(DateTime.Now.Ticks.ToString(), strText.ToString());
                    return;
                }


                string jsonstr = "{record:" + json.ToJson() + "}";

                StringBuilder sbText = new StringBuilder();
                sbText.Append("<script>");
                sbText.Append("var arr = " + jsonstr + ";");
                //==========zxl========
               // sbText.Append("window.parent.returnValue = arr;");
                //===============================
               sbText.Append("if(arr != '')");
               sbText.Append(" {");
               sbText.Append(" var json = arr;");
               sbText.Append(" var record=json.record;");

                sbText.Append(" for(var i=0; i < record.length; i++)");
                sbText.Append("  {");
                sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"txtCustAddr').value = record[i].shortname; ");  //�ͻ�

                 sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidCust').value = record[i].shortname;");

                 sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"txtAddr').value = record[i].address;");
                 sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidAddr').value = record[i].address;");
                 sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"txtContact').value = record[i].linkman1;"); //��ϵ��
                 sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidContact').value = record[i].linkman1;"); //��ϵ��

                 sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"txtCTel').value = record[i].tel1;"); //��ϵ�˵绰

                 sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidTel').value = record[i].tel1;"); //��ϵ�˵绰

                 sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidCustID').value = record[i].id;"); //�ͻ�ID��
                

                        
                
                sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"lblCustDeptName').innerHTML = record[i].custdeptname;"); //��������
                 sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"lblEmail').innerHTML = record[i].email;"); //�����ʼ�
                sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"lblMastCust').innerHTML = record[i].mastcustname;"); //����λ
                sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidCustDeptName').value = record[i].custdeptname;"); //��������
                

               // sbText.Append("");
                 sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidCustEmail').value = record[i].email;"); //�����ʼ�
                 sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidMastCust').value = record[i].mastcustname;"); //����λ
                 sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"lbljob').innerHTML = record[i].job;"); //ְλ
                sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidjob').value = record[i].job;"); //ְλ
                 
                sbText.Append(" if (typeof(window.opener.document.getElementById('"+Opener_ClientId+"Table3'))!='undefined' )");
                sbText.Append("{");

                    sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"txtEqu').value = record[i].equname;"); //�ʲ�����

                    sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidEquName').value = record[i].equname;"); //�ʲ�����
                    sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidEqu').value = record[i].equid;"); //�ʲ�id
                    sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"txtListName').value = record[i].listname;"); //�ʲ�Ŀ¼
                    sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidListName').value = record[i].listname;"); //�ʲ�Ŀ¼
                    sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidListID').value = record[i].listid;"); //�ʲ�Ŀ¼
              
                                                 

                     sbText.Append("}");               
                    sbText.Append("}");
                sbText.Append("}");
                sbText.Append("else");
                sbText.Append("{");

                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidCustID').value ='';"); //�ͻ�ID��	

                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "txtCustAddr').value ='';"); //�ͻ�����

                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidCust').value ='';"); //�ͻ�����	

                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "txtAddr').value ='';"); //��ַ	

                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidAddr').value ='';"); //��ַ	

                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "txtContact').value ='';"); //��ϵ��
                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidContact').value ='';");

                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "txtCTel').value ='';"); //��ϵ�˵绰
                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidTel').value ='';");

                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "lblCustDeptName').innerHTML ='';"); //��������
                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "lblEmail').innerHTML ='';"); //�����ʼ�
                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "lblMastCust').innerHTML ='';"); //����λ


                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidCustDeptName').value ='';"); //��������
                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidCustEmail').value ='';"); //�����ʼ�
                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidMastCust').value ='';"); //����λ

                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "lbljob').innerHTML ='';"); //ְλ
                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidjob').value ='';"); //ְλ
             
                sbText.Append(" }");           
      

                // �رմ���
                sbText.Append("top.close();");
                sbText.Append("</script>");
                // ��ͻ��˷���
                Page.RegisterStartupScript(DateTime.Now.ToString(), sbText.ToString());
               // Response.Write(sbText.ToString());
            }
        }

        protected void dgUserInfo_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");

                if (IsSelect)
                {
                    string client_id = e.Item.Cells[0].Text.Replace("&nbsp;", "");
                    string sWhere = " And id = " + client_id;
                    Br_ECustomerDP ec = new Br_ECustomerDP();
                    DataTable dt = ec.GetDataTableAjax(sWhere, "");
                    Json json = new Json(dt);

                    string jsonstr = "{record:" + json.ToJson() + "}";

                    // ��ͻ��˷���
                    //e.Item.Attributes.Add("ondblclick", "ServerOndblclick(" + jsonstr + ");");
                    Button lnkselect = (Button)e.Item.FindControl("lnkselect");
                    e.Item.Attributes.Add("ondblclick", "OnClientClick('" + lnkselect.ClientID + "');");
                }
            }
        }
    }
}
