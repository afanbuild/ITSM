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
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using System.Text;

namespace Epower.ITSM.Web.AppForms
{
    /// <summary>
    /// frmBr_ECustomerEdit ��ժҪ˵����
    /// </summary>
    public partial class frmBr_ECustomerEdit : BasePage
    {
        string sSchemaValue = "";

        long lngCatalogID = 0;

        RightEntity re = null;

        #region ����
        /// <summary>
        /// 
        /// </summary>
        protected string CustomID
        {
            get
            {
                return this.Master.MainID.ToString();
            }
        }
        #region ��������·��
        /// <summary>
        /// ��������·��
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
        /// �ͻ���չ����ģ��ID
        /// </summary>
        protected long CustSchemaID
        {
            get
            {
                if (ViewState["CustSchemaID"] != null)
                    return long.Parse(ViewState["CustSchemaID"].ToString());
                else
                    return 0;
            }
            set
            {
                ViewState["CustSchemaID"] = value;
            }
        }
        /// <summary>
        /// �������
        /// </summary>
        protected bool QuickNew
        {
            get
            {
                if (Request["QuickNew"] != null)
                    return true;
                else
                    return false;
            }
        }
        #endregion

        /// <summary>
        /// ���ø����尴ť�¼�
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.CustumManager;
            this.Master.IsCheckRight = true;
            if (this.Request.QueryString["id"] != null)
            {
                this.Master.MainID = this.Request.QueryString["id"].ToString();
            }

            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);
            this.Master.Master_Button_Save_Click += new Global_BtnClick(Master_Master_Button_Save_Click);
            this.Master.Master_Button_GoHistory_Click += new Global_BtnClick(Master_Master_Button_GoHistory_Click);
            if (string.IsNullOrEmpty(this.Master.MainID.ToString()))
                this.Master.ShowAddPageButton();
            else
                this.Master.ShowEditPageButton();
            if (Master.GetEditRight() == false && this.Request.QueryString["id"] != null)
            {
                this.Master.ShowNewButton(false);
                this.Master.ShowDeleteButton(false);
                this.Master.ShowSaveButton(false);
            }

            string strThisMsg = "";
            strThisMsg = ddltMastCustID.ClientID + ">" + "����λ" + ">" + "";
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), ddltMastCustID.ClientID, "<script type=\"text/javascript\" >if(typeof(sarrvalidlist) == 'undefined'){ var sarrvalidlist = new Array(1);}  sarrvalidlist[0] = sarrvalidlist[0] + '|' + '" + strThisMsg + "';</script>");

        }

        /// <summary>
        /// ����
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            if (QuickNew)
                Response.Redirect("frmBr_ECustomerEdit.aspx?QuickNew=1");
            else
                Response.Redirect("frmBr_ECustomerEdit.aspx");
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            if (!String.IsNullOrEmpty(this.Master.MainID.ToString()))
            {
                Br_ECustomerDP ee = new Br_ECustomerDP();
                ee.DeleteRecorded(long.Parse(this.Master.MainID.Trim()));
                //������ҳ��
                Master_Master_Button_GoHistory_Click();
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        void Master_Master_Button_Save_Click()
        {
            if (Br_ECustomerDP.CheckCustomCode(txtCustomCode.Value, this.Master.MainID.Trim()) == true)
            {
                PageTool.MsgBox(this, "�ͻ����벻���ظ���");
                this.Master.IsSaveSuccess = false;
                return;
            }
            if (Request["Page"] != "IssueBase")
            {
                if (string.IsNullOrEmpty(this.Master.MainID.Trim()))
                {
                    Br_ECustomerDP ee = new Br_ECustomerDP();
                    ee.Deleted = (int)Epower.ITSM.Base.eRecord_Status.eNormal;
                    ee.RegUserID = decimal.Parse(Session["UserID"].ToString());
                    ee.RegUserName = Session["PersonName"].ToString();
                    ee.RegTime = DateTime.Now;
                    InitObject(ee);
                    ee.UpdateTime = DateTime.Now;
                    ee.InsertRecorded(ee);
                    this.Master.MainID = ee.ID.ToString().Trim();
                }
                else
                {
                    Br_ECustomerDP ee = new Br_ECustomerDP();
                    ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                    InitObject(ee);
                    ee.UpdateTime = DateTime.Now;
                    ee.UpdateRecorded(ee);
                    this.Master.MainID = ee.ID.ToString();
                }
            }
            else if (Request["Page"] == "IssueBase")
            {
                //����Ǵ��¼����������ģ���ô����֮��Ҫ�Ѵ���Ϣ��ʾ���¼����Ǽǽ�����
                Br_ECustomerDP ee = new Br_ECustomerDP();
                ee.Deleted = (int)Epower.ITSM.Base.eRecord_Status.eNormal;
                ee.RegUserID = decimal.Parse(Session["UserID"].ToString());
                ee.RegUserName = Session["PersonName"].ToString();
                ee.RegTime = DateTime.Now;
                InitObject(ee);
                ee.UpdateTime = DateTime.Now;
                ee.InsertRecorded(ee);
                this.Master.MainID = ee.ID.ToString().Trim();

                string sWhere = " And id = " + ee.ID.ToString();
                DataTable dt = ee.GetDataTableAjax(sWhere, "");
                Json json = new Json(dt);

                string jsonstr = "{record:" + json.ToJson() + "}";


                StringBuilder sb = new StringBuilder();
                sb.Append("<script>");
                sb.Append("var arr = " + jsonstr + ";");
                //zxl==================
                if (Request["TypeFrm"] != null)
                {
                    string requestType = Request.QueryString["TypeFrm"].ToString();
                    //�����ж��ĸ�ҳ���������ġ�
                    if (requestType == "CTS_Issue_Service") 
                    {
                       sb.Append(" if (arr != null)");
                        sb.Append("{");
                           sb.Append(" var json = arr;");
                           sb.Append(" var record = json.record;");

                               sb.Append(" for (var i = 0; i < record.length; i++)");
                               sb.Append("{");
                               sb.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidCustID').value = record[i].id;" );  //�ͻ�ID
                               sb.Append("window.opener.document.getElementById('"+Opener_ClientId+"lblMastCust').innerHTML = record[i].mastcustname;");   //����λ
                               sb.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidMastCust').value = record[i].mastcustname;" );  //����λ

                               sb.Append("window.opener.document.getElementById('"+Opener_ClientId+"txtCustAddr').value = record[i].shortname;");   //�ͻ�����
                               sb.Append("window.opener.document.getElementById('"+Opener_ClientId+"txtAddr').value = record[i].address;");   //�û���ַ
                               sb.Append("window.opener.document.getElementById('"+Opener_ClientId+"txtCTel').value = record[i].tel1;");   //�칫�绰
                               sb.Append("window.opener.document.getElementById('"+Opener_ClientId+"lblCustDeptName').value = record[i].custdeptname;");   //�û�����

                               sb.Append("window.opener.document.getElementById('"+Opener_ClientId+"lblEmail').value = record[i].email;");   //�����ʼ�
                               sb.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidCust').value = record[i].shortname;");   //�ͻ�����
                               sb.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidAddr').value = record[i].address;");   //�û���ַ
                               sb.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidTel').value = record[i].tel1;");   //�칫�绰

                                sb.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidCustDeptName').value = record[i].custdeptname;");   //��������
                                sb.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidCustEmail').value = record[i].email;");   //�����ʼ�
                                sb.Append("window.opener.document.getElementById('"+Opener_ClientId+"txtCustMobile').value = record[i].mobile;");    //�ֻ�����
                                 sb.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidCustMobile').value = record[i].mobile;");    //�ֻ�����
                                 sb.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidCustAreaID').value = record[i].custareaid;");    //�ͻ�Ƭ��
                                 sb.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidCustArea').value = record[i].custarea;");    //�ͻ�Ƭ��
                                sb.Append("}");
                            sb.Append("}");  
                    }

                }
                else
                {


                    sb.Append(" if (arr != null)" +
                    "{ " +
                       " var json = arr; " +
                       " var record = json.record; " +

                       " for (var i = 0; i < record.length; i++) " +
                        "{ " +
                           " window.opener.document.getElementById('" + Opener_ClientId + "hidCustID').value = record[i].id;" +   //�ͻ�ID
                           " window.opener.document.getElementById('" + Opener_ClientId + "lblMastCust').innerHTML = record[i].mastcustname;" +   //����λ
                           " window.opener.document.getElementById('" + Opener_ClientId + "hidMastCust').value = record[i].mastcustname; " +   //����λ

                           " window.opener.document.getElementById('" + Opener_ClientId + "txtCustAddr').value = record[i].shortname;" +   //�ͻ�����
                           " window.opener.document.getElementById('" + Opener_ClientId + "txtAddr').value = record[i].address; " +  //�û���ַ
                           " window.opener.document.getElementById('" + Opener_ClientId + "txtContact').value = record[i].linkman1;" +   //��ϵ��
                           " window.opener.document.getElementById('" + Opener_ClientId + "txtCTel').value = record[i].tel1; " +  //��ϵ�绰
                           " window.opener.document.getElementById('" + Opener_ClientId + "lblCustDeptName').innerHTML = record[i].custdeptname;" +   //�û�����
                           " window.opener.document.getElementById('" + Opener_ClientId + "lblEmail').innerHTML = record[i].email; " +  //�����ʼ�

                           " window.opener.document.getElementById('" + Opener_ClientId + "hidCust').value = record[i].shortname;" +        //�ͻ�����
                           " window.opener.document.getElementById('" + Opener_ClientId + "hidAddr').value = record[i].address; " +       //�û���ַ
                           " window.opener.document.getElementById('" + Opener_ClientId + "hidContact').value = record[i].linkman1;" +     //��ϵ��
                           " window.opener.document.getElementById('" + Opener_ClientId + "hidTel').value = record[i].tel1; " +        //��ϵ�绰
                           " window.opener.document.getElementById('" + Opener_ClientId + "hidCustDeptName').value = record[i].custdeptname; " +   //��������
                           " window.opener.document.getElementById('" + Opener_ClientId + "hidCustEmail').value = record[i].email; " +  //�����ʼ�
                           " window.opener.document.getElementById('" + Opener_ClientId + "lbljob').innerHTML = record[i].job;" +   //ְλ
                           " window.opener.document.getElementById('" + Opener_ClientId + "hidjob').value = record[i].job; " +  //ְλ
                        "}" +
                   " }"
                   );

                    //  sb.Append("window.parent.returnValue=arr;");

                    //zxl====================
                }




                sb.Append("top.close();");
                sb.Append("</script>");

                //��ͻ��˷���
                Page.RegisterStartupScript(DateTime.Now.ToString(), sb.ToString());
                Response.Write(sb.ToString());

                //Response.Write("<script>window.parent.returnValue=" + this.Master.MainID + ";top.close();</script>");
            }

        }

        /// <summary>
        /// ����
        /// </summary>
        void Master_Master_Button_GoHistory_Click()
        {
            if (Request["Page"] == "IssueBase")
            { //���ص��¼����Ǽǽ���
                StringBuilder sb = new StringBuilder();
                sb.Append("<script>top.close();</script>");
                //��ͻ��˷���
                Page.RegisterStartupScript(DateTime.Now.ToString(), sb.ToString());
                Response.Write(sb.ToString());
            }
            else
            {
                if (QuickNew)
                    Response.Redirect("../Common/frmDRMUserSelect.aspx");
                else
                    Response.Redirect("frmBr_ECustomerMain.aspx");
            }

        }

        /// <summary>
        /// ҳ�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            //������ҳ��
            SetParentButtonEvent();
            if (!IsPostBack)
            {
                #region �ͻ���չģ��
                DataTable dt = new DataTable();
                dt = Br_SubjectDP.GetSubject();
                if (dt.Rows.Count > 0)
                {
                    //ģ��
                    CustSchemaID = long.Parse(Br_SubjectDP.GetSubject().Rows[0]["CatalogID"].ToString());
                }
                #endregion

                //�󶨷���λ
                InitDropDownList();

                //��������
                LoadData();

                if (Request["CustName"] != null)
                {
                    CtrFTShortName.Value = Request["CustName"].ToString();
                    CtrFTFullName.Value = Request["CustName"].ToString();
                }
                if (Request["CustAddress"] != null)
                {
                    txtAddress.Text = Request["CustAddress"].ToString();
                }
                if (Request["CustLinkMan"] != null)
                {
                    txtLinkMan1.Text = Request["CustLinkMan"].ToString();
                }
                if (Request["CustTel"] != null)
                {
                    txtTel1.Text = Request["CustTel"].ToString();
                }

                if (CommonDP.GetConfigValue("Other", "ItsmCustomerMode") != null && CommonDP.GetConfigValue("Other", "ItsmCustomerMode") == "0")  //����ģʽ
                {
                    tdRefUser.Visible = true;
                    tdRefUserTitle.Visible = true;
                    tdMast.ColSpan = 1;
                }
                else
                {
                    tdRefUser.Visible = false;
                    tdRefUserTitle.Visible = false;
                    tdMast.ColSpan = 3;
                }


                //��ȡ���¼����������Ŀͻ���Ϣ
                if (Request["Page"] == "IssueBase")
                {
                    if (Request["MastCust"] != null)
                    {
                        ddltMastCustID.SelectedIndex = ddltMastCustID.Items.IndexOf(ddltMastCustID.Items.FindByText(Request["MastCust"].ToString()));
                    }
                    if (Request["CustName"] != null)
                    {
                        CtrFTShortName.Value = Request["CustName"].ToString();
                    }
                    if (Request["Addr"] != null)
                    {
                        txtAddress.Text = Request["Addr"].ToString();
                    }
                    if (Request["Contact"] != null)
                    {
                        txtLinkMan1.Text = Request["Contact"].ToString();
                    }
                    if (Request["Tell"] != null)
                    {
                        txtTel1.Text = Request["Tell"].ToString();
                    }
                    if (Request["CustDeptName"] != null)
                    {
                        txtCustDeptName.Text = Request["CustDeptName"].ToString();
                    }
                    if (Request["Email"] != null)
                    {
                        txtEmail.Text = Request["Email"].ToString();
                    }
                    if (Request["job"] != null)
                    {
                        CtrFlowJob.Value = Request["job"].ToString();
                    }
                }

                //������ʾ
                PageDeal.SetLanguage(this.Controls[0]);



            }
            else
            {
                if (RefUser.UserID == 0)
                    RefUser.UserName = "";
                sSchemaValue = Session["Cust_DeskEdit_Schema"].ToString();
            }
            CustSchemeCtr1.ControlXmlValue = sSchemaValue;
            CustSchemeCtr1.BrCategoryID = CustSchemaID;
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
            if (ddltMastCustID.Items.Count > 1)
                ddltMastCustID.SelectedIndex = 1;
        }
        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            if (!string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                Br_ECustomerDP ee = new Br_ECustomerDP();
                ee = ee.GetReCorded(long.Parse(this.Master.MainID));
                ddltMastCustID.SelectedIndex = ddltMastCustID.Items.IndexOf(ddltMastCustID.Items.FindByValue(ee.MastCustID.ToString()));
                txtAddress.Text = ee.Address.ToString();
                ctrFCDWTType.CatelogID = long.Parse(ee.CustomerType.ToString());
                //ctrFCDWTType.CatelogValue = ee.CustomerTypeName.ToString();
                //CtrFlowJob.CatelogID = long.Parse(ee.jobID.ToString());
                CtrFlowJob.Value = ee.Job.ToString();
                txtCustomCode.Value = ee.CustomCode.ToString();
                CtrFTFullName.Value = ee.FullName.ToString();
                txtLinkMan1.Text = ee.LinkMan1.ToString();
                CtrFTShortName.Value = ee.ShortName.ToString();
                txtTel1.Text = ee.Tel1.ToString();
                txtEmail.Text = ee.Email.ToString();
                txtRights.Text = ee.Rights.ToString();
                txtRemark.Text = ee.Remark.ToString();
                txtCustDeptName.Text = ee.CustDeptName.ToString();
                RefUser.UserID = (long)ee.UserID;
                RefUser.UserName = EpowerCom.EPSystem.GetUserName((long)ee.UserID);   //�������еķ����л���
                if (ee.SchemaValue != string.Empty)
                {
                    sSchemaValue = ee.SchemaValue;
                }
                Session["Cust_DeskEdit_Schema"] = sSchemaValue;
            }
            else
            {
                CustSchemeCtr1.SetAddEquTrue();
            }
            Session["Cust_DeskEdit_Schema"] = sSchemaValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ee"></param>
        private void InitObject(Br_ECustomerDP ee)
        {
            ee.MastCustID = decimal.Parse(ddltMastCustID.SelectedValue.Trim());
            ee.MastCustName = ddltMastCustID.SelectedItem.Text.Trim();
            ee.Address = txtAddress.Text.Trim();
            ee.CustomerType = ctrFCDWTType.CatelogID;
            ee.CustomerTypeName = ctrFCDWTType.CatelogValue.Trim();
            ee.jobID = 0;
            ee.Job = CtrFlowJob.Value.Trim();
            ee.CustomCode = txtCustomCode.Value.Trim();
            ee.FullName = CtrFTFullName.Value.Trim();
            ee.LinkMan1 = txtLinkMan1.Text.Trim();

            ee.ShortName = CtrFTShortName.Value.Trim();
            ee.Tel1 = txtTel1.Text.Trim();
            ee.Email = txtEmail.Text.Trim();
            ee.Rights = txtRights.Text.Trim();
            ee.Remark = txtRemark.Text.Trim();
            ee.CustDeptName = txtCustDeptName.Text.Trim();
            ee.UserID = RefUser.UserID;
            ee.SchemaValue = CustSchemeCtr1.ControlXmlValue;

            Session["Cust_DeskEdit_Schema"] = ee.SchemaValue;
        }
    }
}
