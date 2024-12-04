/*******************************************************************
 * ��Ȩ���У�
 * Description���¼����߼���ѯҳ��
 * Create By  ��SuperMan
 * Create Date��2011-08-22
 * *****************************************************************/
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
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using EpowerGlobal;
using System.Data.OracleClient;
using System.Text;
using System.Collections.Generic;


namespace Epower.ITSM.Web.Forms
{
    /// <summary>
    /// frmActorCondList ��ժҪ˵����
    /// </summary>
    public partial class CST_Issue_AdvancedCondition : BasePage
    {
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
        ///��� ·��
        /// </summary>
        public string Opener_ClientId
        {
           
            get
            {
                return (Request["Opener_ClientId"] == null) ? "" : Request["Opener_ClientId"].ToString().Replace("hidClientId_ForOpenerPage", "");
            }
        }

        /// <summary>
        /// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
        /// �˷��������ݡ�
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion
        #region ��������ͼ��
        private string tableName;
        private string viewName;
        #endregion
        #region �������

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                cboStatus.Items.Add(new ListItem("����״̬", "-1"));
                cboStatus.Items.Add(new ListItem("--���ڴ���", ((int)e_FlowStatus.efsHandle).ToString()));
                cboStatus.Items.Add(new ListItem("--��������", ((int)e_FlowStatus.efsEnd).ToString()));
                cboStatus.Items.Add(new ListItem("--������ͣ", ((int)e_FlowStatus.efsStop).ToString()));
                cboStatus.Items.Add(new ListItem("--������ֹ", ((int)e_FlowStatus.efsAbort).ToString()));
                cboStatus.SelectedIndex = 1;

                InitDropDown();  //��ʼ�����񼶱�

                //������ʼ����
                string sQueryBeginDate = "0";
                if (CommonDP.GetConfigValue("Other", "QueryBeginDate") != null)
                    sQueryBeginDate = CommonDP.GetConfigValue("Other", "QueryBeginDate").ToString();

                if (sQueryBeginDate == "0")
                {
                    ctrDateSelectTime1.BeginTime = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                }
                else
                {
                    ctrDateSelectTime1.BeginTime = DateTime.Parse(sQueryBeginDate).ToString("yyyy-MM-dd");
                }
                ctrDateSelectTime1.EndTime = DateTime.Now.ToString("yyyy-MM-dd");


                if (Request["SQLName"] != null)
                {
                    InitDropSQLwSave1(Request["SQLName"].ToString().Trim());  //��ʼ����ѯ����
                    SqlWhereShow(Request["SQLName"].ToString().Trim() == "" ? "Temp1" : Request["SQLName"].ToString().Trim());
                }

                if (DropSQLwSave.SelectedItem.Value == "0")
                {
                    btn_delete.Text = "���";
                }
                else
                {
                    btn_delete.Text = "ɾ��";
                }

                //������ʾ
                PageDeal.SetLanguage(this.Controls[1]);
                tableName = Request.QueryString["tableName"];
                viewName = Request.QueryString["viewName"];
                if (!string.IsNullOrEmpty(tableName) || !string.IsNullOrEmpty(viewName))
                    DataBing();
            }
            if (DropSQLwSave.SelectedItem.Value == "0")
            {
                btn_delete.Text = "���";
            }
            else
            {
                btn_delete.Text = "ɾ��";
            }
        }

        #endregion

        #region ���尴ť�¼�

        #region �����ѯ��������

        protected void chkSave_Click(object sender, EventArgs e)
        {
            if (this.txtSQLName.Text.Trim() == string.Empty)
            {
                PageTool.MsgBox(this.Page, "�������Ʋ���Ϊ�գ�");
                return;
            }

            if (this.txtSQLName.Text.Trim().ToLower() == "temp1")
            {
                PageTool.MsgBox(this.Page, "Temp1���Ʊ���ϵͳʹ�ã��������������ƣ�");
                return;
            }

            SqlWhereSave();
            InitDropSQLwSave();
        }

        #endregion

        #region ɾ����ѯ��������
        /// <summary>
        /// ɾ����ѯ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_delete_Click(object sender, EventArgs e)
        {
            if (this.txtSQLName.Text.Trim() == string.Empty && DropSQLwSave.SelectedItem.Text != "==ѡ���ղز�ѯ����==")
            {
                PageTool.MsgBox(this.Page, "�������Ʋ���Ϊ�գ�");
                return;
            }
            else
            {
                //���������Ʋ�Ϊ��ʱ������ѯ������ֵȫ�����
                txtCustName.Text = "";                  //�ͻ�����
                txtIssueNo.Text = "";                   //�¼�����
                ctrDateSelectTime1.BeginTime = "";      //����ʱ�俪ʼʱ��
                ctrDateSelectTime1.EndTime = "";        //����ʱ�����ʱ��
                cboStatus.SelectedIndex = 0;            //����״̬
                CtrFCDEffect.CatelogID = 0;             //Ӱ���
                ctrDealStatus.CatelogID = 0;         //�¼�״̬
                CtrFCDInstancy.CatelogID = 0;           //������
                ctrServiceType.CatelogID = 0;           //�¼����
                ctrServiceType.CatelogValue = "";
                ctrFCDWTType.CatelogID = 0;             //�¼�����
                UserPSjzxr.UserID = "0";                //����ʦ
                UserPSjzxr.UserName = "";
                txtSubject.Text = "";                   //����
                txtperson.Text = "";                    //�ǵ���
                ddltServiceLevel.SelectedIndex = 0;     //���񼶱�
                txtEqu.Text = "";                       //�ʲ���Ϣ
                lblEqu.Text = "";
                ddltEmailState.SelectedIndex = 0;       //�ʼ��ط�״̬
            }

            ZHServiceDP.deleteCST_ISSUE_Where(Session["UserName"].ToString(), DropSQLwSave.SelectedItem.Text.ToString());
            this.txtSQLName.Text = string.Empty;
            InitDropSQLwSave();
        }

        #endregion

        #region ȷ��
        /// <summary>
        /// ȷ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOK_Click(object sender, EventArgs e)
        {
            string SQLName = "";
            if (DropSQLwSave.SelectedItem.Text != "==ѡ���ղز�ѯ����==")
            {
                SQLName = DropSQLwSave.SelectedItem.Text.Trim();
            }
            else
            {
                SQLName = "Temp1";
            }

            //ȷ��ǰ�ȱ�����ʱ�����ݿ�
            SqlWhereSaveTemp1(SQLName);

            //�رմ���
            StringBuilder sbText = new StringBuilder();
            sbText.Append("<script>");
            sbText.Append("var arr = new Array();");
            // �ɹ�
            sbText.Append("arr[0] ='" + SQLName + "';");
            sbText.Append("arr[1] ='" + this.hidValue.Value + "';");
            // ����
            //==========zxl==
            sbText.Append(
             " var arrValue=arr;" +
            " if(arrValue != null)"+
	        "{" +
	        "window.opener.document.getElementById('"+Opener_ClientId+"hidIsGaoji').value = '1'; "+
	       " window.opener.document.getElementById('"+Opener_ClientId+"hidSQLName').value = arrValue[0];"+
	        " window.opener.document.getElementById('"+Opener_ClientId+"HiddenColumn').value = arrValue[1]; "+
	       " window.opener.document.getElementById('"+Opener_ClientId+"btnOk').click();" +
	    "}" );

            //=========zxl==
           // sbText.Append("window.parent.returnValue = arr;");
            // �رմ���
            sbText.Append("top.close();");
            sbText.Append("</script>");
            // ��ͻ��˷���
            Page.RegisterStartupScript(DateTime.Now.ToString(), sbText.ToString());
            Response.Write(sbText.ToString());
        }

        #endregion

        #region �ر�

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Write("<script>window.close();</script>");
        }

        #endregion

        #region �ղ������������¼�

        protected void DropSQLwSave_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strTemp = string.Empty;              //��ʱ��Ÿı��������б�����

            if (DropSQLwSave.SelectedItem.Text != "==ѡ���ղز�ѯ����==")
            {
                strTemp = DropSQLwSave.SelectedValue;           //��ѡ��ĸ߼��������ƴ洢����

                SqlWhereShow(DropSQLwSave.SelectedItem.Text.ToString());

                //���·��ʴ���
                ZHServiceDP.updateCST_ISSUE_WhereNums(Session["UserName"].ToString(), DropSQLwSave.SelectedItem.Text.ToString());
                DataTable dt = ZHServiceDP.getCST_ISSUE_LISTFASTQUERY("CST_Issue_List", Session["UserName"].ToString(), string.Empty);
                DropSQLwSave.Items.Clear();
                DropSQLwSave.DataSource = dt.DefaultView;
                DropSQLwSave.DataTextField = "Name";
                DropSQLwSave.DataValueField = "ID";
                DropSQLwSave.DataBind();
                DropSQLwSave.Items.Insert(0, new ListItem("==ѡ���ղز�ѯ����==", "0"));

                DropSQLwSave.SelectedIndex = DropSQLwSave.Items.IndexOf(DropSQLwSave.Items.FindByValue(strTemp));

                txtSQLName.Text = DropSQLwSave.SelectedItem.Text;
            }
            else
            {
                txtSQLName.Text = "";
            }
        }

        #endregion

        #endregion

        #region �Զ��巽��

        #region InitDropDown ��ʼ�����񼶱�
        /// <summary>
        /// 
        /// </summary>
        private void InitDropDown()
        {
            Cst_ServiceLevelDP ee = new Cst_ServiceLevelDP();
            DataTable dt = ee.GetDataTable("", "");
            ddltServiceLevel.Items.Clear();
            ddltServiceLevel.DataSource = dt.DefaultView;
            ddltServiceLevel.DataTextField = "LevelName";
            ddltServiceLevel.DataValueField = "ID";
            ddltServiceLevel.DataBind();
            ddltServiceLevel.Items.Insert(0, new ListItem("", "0"));
        }

        #endregion

        protected void Button1_Click(object sender, EventArgs e)
        {
            HidIS.Value = "true";
            Bing();
        }
        void ShowDataBing(Dictionary<string, string> dic)
        {
            this._TableColumnCheckBoxList.DataSource = dic;
            this._TableColumnCheckBoxList.DataTextField = "value";
            this._TableColumnCheckBoxList.DataValueField = "key";
            this._TableColumnCheckBoxList.DataBind();
        }
        public void Bing()
        {

            //string value = @"������,�����˵�ַ,�칫�绰,�����ʼ�,��ϸ��Ϣ,���񼶱�,�ɳ�ʱ��,����ʱ��,ִ��������,���ʱ��,��ʩ�����";//,�ϼƽ��,�ϼƹ�ʱ

            //string key = @"CustName,CustAddress,CTel,Email,Content,ServiceLevel,Outtime,ServiceTime,Sjwxr,FinishedTime,DealContent";//,TotalAmount,TotalHours


            string key2 = @"CustName,CustAddress,CTel,Email,Content,ServiceLevel,ReSouseName,DealStatus,Outtime,ServiceTime,Sjwxr,FinishedTime,DealContent";//IssueRootName,TotalAmount,TotalHours
            string value2 = @"�ͻ�����,�ͻ���ַ,�칫�绰,�����ʼ�,��ϸ��Ϣ,���񼶱�,�¼���Դ,���״̬,�ɳ�ʱ��,����ʱ��,ִ��������,���ʱ��,��ʩ�����";//,�ܼƽ��,�ϼƹ�ʱ,�¼���Դ

            string key = @"";
            string value= @"";
            if (this.DropDownList1.SelectedValue == "1")
            {

                this.ShowDataBing(Dic(value2, key2));
            }
            else
            {
                this.ShowDataBing(Dic(value, key));
            }
        }
        void DataBing()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("1", "�¼�");
            dic.Add("2", "��������");
            this.DropDownList1.DataSource = dic;
            this.DropDownList1.DataTextField = "value";
            this.DropDownList1.DataValueField = "key";
            this.DropDownList1.DataBind();
        }
        Dictionary<string, string> Dic(string value, string key)
        {
            string[] keys = key.Split(',');
            string[] values = value.Split(',');
            Dictionary<string, string> dic = new Dictionary<string, string>();
            for (int i = 0; i < keys.Length; i++)
            {
                dic.Add(keys[i], values[i]);
            }
            return dic;
        }
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bing();
        }
        #region �����ѯ��������

        protected void SqlWhereSave()
        {
            if (txtSQLName.Text.Trim() != string.Empty)
            {
                StringBuilder SQLText = new StringBuilder();

                //�ͻ�����
                SQLText.Append("txtCustName.Text=" + txtCustName.Text.ToString().Trim() + "|@@?@$|");

                //�¼�����
                SQLText.Append("txtIssueNo.Text=" + txtIssueNo.Text.ToString().Trim() + "|@@?@$|");

                //�¼�״̬
                SQLText.Append("ddlDealStatus.SelectedValue=" + ctrDealStatus.CatelogID.ToString().Trim() + "|@@?@$|");

                //����״̬
                SQLText.Append("cboStatus.SelectedValue=" + cboStatus.SelectedValue.ToString().Trim() + "|@@?@$|");

                //����ʱ�俪ʼ
                SQLText.Append("ctrDateSelectTime1.BeginTime=" + ctrDateSelectTime1.BeginTime.ToString().Trim() + "|@@?@$|");

                //����ʱ�����
                SQLText.Append("ctrDateSelectTime1.EndTime=" + ctrDateSelectTime1.EndTime.ToString().Trim() + "|@@?@$|");

                //�¼����
                SQLText.Append("ctrServiceType.CatelogID=" + ctrServiceType.CatelogID.ToString().Trim() + "|@@?@$|");

                //Ӱ���
                SQLText.Append("CtrFCDEffect.CatelogID=" + CtrFCDEffect.CatelogID.ToString().Trim() + "|@@?@$|");

                //�¼�����
                SQLText.Append("ctrFCDWTType.CatelogID=" + ctrFCDWTType.CatelogID.ToString().Trim() + "|@@?@$|");

                //������
                SQLText.Append("CtrFCDInstancy.CatelogID=" + CtrFCDInstancy.CatelogID.ToString().Trim() + "|@@?@$|");

                //����
                SQLText.Append("txtSubject.Text=" + txtSubject.Text.ToString().Trim() + "|@@?@$|");

                //ִ����
                SQLText.Append("UserPSjzxr.UserID=" + UserPSjzxr.UserID.ToString().Trim() + "|@@?@$|");

                //�ǵ���
                SQLText.Append("txtperson.Text=" + txtperson.Text.ToString().Trim() + "|@@?@$|");

                //���񼶱�
                SQLText.Append("ddltServiceLevel.SelectedValue=" + ddltServiceLevel.SelectedValue.ToString().Trim() + "|@@?@$|");

                //�ʲ���Ϣ
                SQLText.Append("txtEqu.Text=" + txtEqu.Text.ToString().Trim() + "|@@?@$|");

                //�ʼ��ط�״̬
                SQLText.Append("ddltEmailState.SelectedValue=" + ddltEmailState.SelectedValue.ToString().Trim() + "|@@?@$|");

                //����������
                SQLText.Append("txtSQLName.Text=" + txtSQLName.Text.ToString().Trim());

                string SQLstr = "";

                if (IsExistQuery(" and Name=" + StringTool.SqlQ(this.txtSQLName.Text.Trim()) + " and  nvl(SQLWhere,' ')!='Temp1'  and FORMID='CST_Issue_List'"))
                {
                    if (string.IsNullOrEmpty(this.hidValue.Value))
                    {
                        SQLstr = "update CST_ISSUE_QUERYSave set SQLText=" + StringTool.SqlQ(SQLText.ToString().Trim()) +
                                " where FORMID='CST_Issue_List' and  nvl(SQLWhere,' ')!='Temp1' and  LOGINNAME=" + StringTool.SqlQ(Session["UserName"].ToString()) + " and Name=" + StringTool.SqlQ(this.txtSQLName.Text.Trim());
                    }
                    else
                    {
                        SQLstr = "update CST_ISSUE_QUERYSave set SQLText=" + StringTool.SqlQ(SQLText.ToString().Trim()) + ",DISPLAYCOLUMN=" + StringTool.SqlQ(hidValue.Value.ToString().Trim()) +
                           " where FORMID='CST_Issue_List' and  nvl(SQLWhere,' ')!='Temp1' and  LOGINNAME=" + StringTool.SqlQ(Session["UserName"].ToString()) + " and Name=" + StringTool.SqlQ(this.txtSQLName.Text.Trim());
                    }
                }
                else
                {
                    string strID = EpowerGlobal.EPGlobal.GetNextID("CST_ISSUE_QUERYSaveID").ToString();

                    SQLstr = "insert into CST_ISSUE_QUERYSave(ID,Name,FORMID,SQLWhere,LOGINNAME,SQLText,DISPLAYCOLUMN)" +
                            " values(" + strID + "," + StringTool.SqlQ(this.txtSQLName.Text.Trim()) + ",'CST_Issue_List',''," +
                            StringTool.SqlQ(Session["UserName"].ToString()) + "," + StringTool.SqlQ(SQLText.ToString().Trim()) + "," + StringTool.SqlQ(this.hidValue.Value.ToString().Trim()) + ")";
                }
                OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationSettings.AppSettings["SQLConnString"]);
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, SQLstr);
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }

            }
        }

        protected void SqlWhereSaveTemp1(string SQLName)
        {
            StringBuilder SQLText = new StringBuilder();

            //�ͻ�����
            SQLText.Append("txtCustName.Text=" + txtCustName.Text.ToString().Trim() + "|@@?@$|");

            //�¼�����
            SQLText.Append("txtIssueNo.Text=" + txtIssueNo.Text.ToString().Trim() + "|@@?@$|");

            //�¼�״̬
            SQLText.Append("ddlDealStatus.SelectedValue=" + ctrDealStatus.CatelogID.ToString().Trim() + "|@@?@$|");

            //����״̬
            SQLText.Append("cboStatus.SelectedValue=" + cboStatus.SelectedValue.ToString().Trim() + "|@@?@$|");

            //����ʱ�俪ʼ
            SQLText.Append("ctrDateSelectTime1.BeginTime=" + ctrDateSelectTime1.BeginTime.ToString().Trim() + "|@@?@$|");

            //����ʱ�����
            SQLText.Append("ctrDateSelectTime1.EndTime=" + ctrDateSelectTime1.EndTime.ToString().Trim() + "|@@?@$|");

            //�¼����
            SQLText.Append("ctrServiceType.CatelogID=" + ctrServiceType.CatelogID.ToString().Trim() + "|@@?@$|");

            //Ӱ���
            SQLText.Append("CtrFCDEffect.CatelogID=" + CtrFCDEffect.CatelogID.ToString().Trim() + "|@@?@$|");

            //�¼�����
            SQLText.Append("ctrFCDWTType.CatelogID=" + ctrFCDWTType.CatelogID.ToString().Trim() + "|@@?@$|");

            //������
            SQLText.Append("CtrFCDInstancy.CatelogID=" + CtrFCDInstancy.CatelogID.ToString().Trim() + "|@@?@$|");

            //����
            SQLText.Append("txtSubject.Text=" + txtSubject.Text.ToString().Trim() + "|@@?@$|");

            //ִ����
            SQLText.Append("UserPSjzxr.UserID=" + UserPSjzxr.UserID.ToString().Trim() + "|@@?@$|");

            //�ǵ���
            SQLText.Append("txtperson.Text=" + txtperson.Text.ToString().Trim() + "|@@?@$|");

            //���񼶱�
            SQLText.Append("ddltServiceLevel.SelectedValue=" + ddltServiceLevel.SelectedValue.ToString().Trim() + "|@@?@$|");

            //�ʲ���Ϣ
            SQLText.Append("txtEqu.Text=" + txtEqu.Text.ToString().Trim() + "|@@?@$|");

            //�ʼ��ط�״̬
            SQLText.Append("ddltEmailState.SelectedValue=" + ddltEmailState.SelectedValue.ToString().Trim() + "|@@?@$|");

            //����������
            SQLText.Append("txtSQLName.Text=" + SQLName.Trim());

            string SQLstr = "";

            if (IsExistQuery(" and Name='Temp1' and FORMID='CST_Issue_List' and  SQLWhere='Temp1' and LOGINNAME=" + StringTool.SqlQ(Session["UserName"].ToString())))
            {
                 if (!string.IsNullOrEmpty(this.hidValue.Value))
                {
                    SQLstr = "update CST_ISSUE_QUERYSave set SQLText=" + StringTool.SqlQ(SQLText.ToString().Trim()) + ",DISPLAYCOLUMN=" + StringTool.SqlQ(this.hidValue.Value.ToString().Trim()) +
                        " where FORMID='CST_Issue_List' and  LOGINNAME=" + StringTool.SqlQ(Session["UserName"].ToString()) + " and Name='Temp1' and  SQLWhere='Temp1'";
                }
                 else
                 {
                     SQLstr = "update CST_ISSUE_QUERYSave set SQLText=" + StringTool.SqlQ(SQLText.ToString().Trim()) +
                             " where FORMID='CST_Issue_List' and  LOGINNAME=" + StringTool.SqlQ(Session["UserName"].ToString()) + " and Name='Temp1' and  SQLWhere='Temp1'";
                 }
            }
            else
            {
                string strID = EpowerGlobal.EPGlobal.GetNextID("CST_ISSUE_QUERYSaveID").ToString();

                SQLstr = "insert into CST_ISSUE_QUERYSave(ID,Name,SQLWhere,FORMID,LOGINNAME,SQLText,DISPLAYCOLUMN)" +
                        " values(" + strID + ",'Temp1','Temp1','CST_Issue_List'," +
                        StringTool.SqlQ(Session["UserName"].ToString()) + "," + StringTool.SqlQ(SQLText.ToString().Trim()) + "," + StringTool.SqlQ(this.hidValue.Value.ToString().Trim()) + ")";
            }
            OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationSettings.AppSettings["SQLConnString"]);
            try
            {
                int i = OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, SQLstr);                
            }
            finally { ConfigTool.CloseConnection(cn); }
        }

        #endregion

        #region �жϲ�ѯ�����Ƿ����
        /// <summary>
        /// �жϲ�ѯ�����Ƿ����
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public static bool IsExistQuery(string strWhere)
        {
            bool result = false;
            string strSql = "select * from CST_ISSUE_QUERYSave where rownum<=1 " + strWhere;
            DataTable dt = CommonDP.ExcuteSqlTable(strSql);
            if (dt != null && dt.Rows.Count > 0)
                result = true;

            return result;
        }
        #endregion

        #region  �ղ������������������
        private void InitDropSQLwSave()
        {
            DataTable dt = ZHServiceDP.getCST_ISSUE_LISTFASTQUERY("CST_Issue_List", Session["UserName"].ToString(), string.Empty);
            DropSQLwSave.Items.Clear();
            DropSQLwSave.DataSource = dt.DefaultView;
            DropSQLwSave.DataTextField = "Name";
            DropSQLwSave.DataValueField = "ID";
            DropSQLwSave.DataBind();
            DropSQLwSave.Items.Insert(0, new ListItem("==ѡ���ղز�ѯ����==", "0"));

            if (txtSQLName.Text.Trim() != string.Empty)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Name"].ToString().Trim() == txtSQLName.Text.Trim())
                    {
                        DropSQLwSave.SelectedIndex = DropSQLwSave.Items.IndexOf(DropSQLwSave.Items.FindByValue(dt.Rows[i]["ID"].ToString().Trim()));
                        txtSQLName.Text = DropSQLwSave.SelectedItem.Text == "==ѡ���ղز�ѯ����==" ? "" : DropSQLwSave.SelectedItem.Text;
                    }
                }
            }

            if (DropSQLwSave.SelectedItem.Value == "0")
            {
                btn_delete.Text = "���";
            }
            else
            {
                btn_delete.Text = "ɾ��";
            }
        }

        private void InitDropSQLwSave1(string SQLName)
        {
            DataTable dt = ZHServiceDP.getCST_ISSUE_LISTFASTQUERY("CST_Issue_List", Session["UserName"].ToString(), string.Empty);
            DropSQLwSave.Items.Clear();
            DropSQLwSave.DataSource = dt.DefaultView;
            DropSQLwSave.DataTextField = "Name";
            DropSQLwSave.DataValueField = "ID";
            DropSQLwSave.DataBind();
            DropSQLwSave.Items.Insert(0, new ListItem("==ѡ���ղز�ѯ����==", "0"));


            if (SQLName.Trim() != "Temp1")
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Name"].ToString().Trim() == SQLName.Trim())
                    {
                        DropSQLwSave.SelectedIndex = DropSQLwSave.Items.IndexOf(DropSQLwSave.Items.FindByValue(dt.Rows[i]["ID"].ToString().Trim()));
                        txtSQLName.Text = DropSQLwSave.SelectedItem.Text == "==ѡ���ղز�ѯ����==" ? "" : DropSQLwSave.SelectedItem.Text;
                    }
                }
            }
            else
            {
                DropSQLwSave.SelectedValue = "0";
            }

        }
        #endregion

        #region ����ѡ���������ؽ���ؼ�ֵ

        protected void SqlWhereShow(string SQLName)
        {
            DataTable dt = ZHServiceDP.getCST_ISSUE_LISTFASTQUERY("CST_Issue_List", Session["UserName"].ToString(), SQLName);

            if (dt.Rows.Count > 0)
            {
                string[] SQLTextDetailList = dt.Rows[0]["SQLText"].ToString().Split(new string[] { "|@@?@$|" }, StringSplitOptions.None);

                for (int i = 0; i < SQLTextDetailList.Length; i++)
                {
                    string[] SQLTextDetail = SQLTextDetailList[i].Trim().Split('=');

                    //�ͻ�����
                    if (SQLTextDetail[0].Trim() == "txtCustName.Text")
                    {
                        this.txtCustName.Text = SQLTextDetail[1].Trim();
                    }

                    //�¼�����
                    if (SQLTextDetail[0].Trim() == "txtIssueNo.Text")
                    {
                        this.txtIssueNo.Text = SQLTextDetail[1].Trim();
                    }

                    //����״̬
                    if (SQLTextDetail[0].Trim() == "cboStatus.SelectedValue")
                    {
                        this.cboStatus.SelectedValue = SQLTextDetail[1].Trim();
                    }

                    //����ʱ�俪ʼ
                    if (SQLTextDetail[0].Trim() == "ctrDateSelectTime1.BeginTime")
                    {
                        this.ctrDateSelectTime1.BeginTime = SQLTextDetail[1].Trim();
                    }

                    //����ʱ�����
                    if (SQLTextDetail[0].Trim() == "ctrDateSelectTime1.EndTime")
                    {
                        this.ctrDateSelectTime1.EndTime = SQLTextDetail[1].Trim();
                    }

                    //Ӱ���
                    if (SQLTextDetail[0].Trim() == "CtrFCDEffect.CatelogID")
                    {
                        this.CtrFCDEffect.CatelogID = Convert.ToInt64(SQLTextDetail[1].Trim());
                    }

                    //�¼�״̬
                    if (SQLTextDetail[0].Trim() == "ddlDealStatus.SelectedValue")
                    {
                        this.ctrDealStatus.CatelogID = Convert.ToInt64(SQLTextDetail[1].Trim());
                    }

                    //������
                    if (SQLTextDetail[0].Trim() == "CtrFCDInstancy.CatelogID")
                    {
                        this.CtrFCDInstancy.CatelogID = Convert.ToInt64(SQLTextDetail[1].Trim());
                    }

                    //�¼����
                    if (SQLTextDetail[0].Trim() == "ctrServiceType.CatelogID")
                    {
                        this.ctrServiceType.CatelogID = Convert.ToInt64(SQLTextDetail[1].Trim());
                        this.hidServiceTypeID.Value = SQLTextDetail[1].Trim();
                    }

                    //�¼�����
                    if (SQLTextDetail[0].Trim() == "ctrFCDWTType.CatelogID")
                    {
                        this.ctrFCDWTType.CatelogID = Convert.ToInt64(SQLTextDetail[1].Trim());
                    }

                    //ִ����
                    if (SQLTextDetail[0].Trim() == "UserPSjzxr.UserID")
                    {
                        this.UserPSjzxr.UserID = SQLTextDetail[1].Trim();

                        UserPSjzxr.UserName = ZHServiceDP.GetStaffNamesByStaffIDs(UserPSjzxr.UserID == "" ? "0" : UserPSjzxr.UserID);
                    }

                    //����
                    if (SQLTextDetail[0].Trim() == "txtSubject.Text")
                    {
                        this.txtSubject.Text = SQLTextDetail[1].Trim();
                    }

                    //�ǵ���
                    if (SQLTextDetail[0].Trim() == "txtperson.Text")
                    {
                        this.txtperson.Text = SQLTextDetail[1].Trim();
                    }

                    //���񼶱�
                    if (SQLTextDetail[0].Trim() == "ddltServiceLevel.SelectedValue")
                    {
                        this.ddltServiceLevel.SelectedIndex = ddltServiceLevel.Items.IndexOf(ddltServiceLevel.Items.FindByValue(SQLTextDetail[1].Trim()));
                        this.hidServiceLevelID.Value = (ddltServiceLevel.Items.IndexOf(ddltServiceLevel.Items.FindByValue(SQLTextDetail[1].Trim()))).ToString();

                    }

                    //�ʲ���Ϣ
                    if (SQLTextDetail[0].Trim() == "txtEqu.Text")
                    {
                        this.txtEqu.Text = SQLTextDetail[1].Trim();
                        this.lblEqu.Text = SQLTextDetail[1].Trim();
                    }

                    //�ʼ��ط�״̬
                    if (SQLTextDetail[0].Trim() == "ddltEmailState.SelectedValue")
                    {
                        ddltEmailState.SelectedIndex = ddltEmailState.Items.IndexOf(ddltEmailState.Items.FindByValue(SQLTextDetail[1].Trim()));
                    }
                }

            }
        }

        #endregion

        #endregion

    }
}
