/****************************************************************************
 * 
 * description:�������Ӧ�ñ�
 * 
 * 
 * 
 * Create by:
 * Create Date:2008-04-04
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
using System.Xml;

using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using appDataProcess;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;
using EpowerCom;
using EpowerGlobal;
using Epower.ITSM.Web.Controls;
using Epower.ITSM.SqlDAL.ES_TBLCS;
using System.Collections.Generic;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frm_ChangeBase : BasePage
    {
        #region �������

        /// <summary>
        /// myFlowForms
        /// </summary>
        private FlowForms myFlowForms;
        long lngFromFlowID = 0; //�鵵��Դ������ʵ��ID
        long lngProblemFlowID = 0;          //��Ŵ����ⵥ�����������ⵥFlowID
        private objFlow oFlow;

        #endregion

        #region ����

        #region ����ID

        /// <summary>
        /// ����ID
        /// </summary>
        public string FlowID
        {
            get
            {
                if (ViewState["frm_ChangeBase_FlowID"] != null)
                {
                    return ViewState["frm_ChangeBase_FlowID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["frm_ChangeBase_FlowID"] = value;
            }
        }

        #endregion

        #region ҵ����ID

        /// <summary>
        /// ҵ����ID
        /// </summary>
        public string BusID
        {
            get
            {
                if (ViewState["frm_Change_Base_BusID"] != null)
                {
                    return ViewState["frm_Change_Base_BusID"].ToString();
                }
                else
                {
                    ViewState["frm_Change_Base_BusID"] = ZHServiceDP.GetBID(oFlow.FlowModelID).ToString();
                    return ViewState["frm_Change_Base_BusID"].ToString();
                }
            }
            set
            {
                ViewState["frm_Change_Base_BusID"] = value;
            }
        }

        #endregion

        #region ����ģ��ID

        /// <summary>
        /// ����ģ��ID
        /// </summary>
        public string FlowModelID
        {
            get
            {
                if (ViewState["frm_ChangeBase_FlowModelID"] != null)
                {
                    return ViewState["frm_ChangeBase_FlowModelID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["frm_ChangeBase_FlowModelID"] = value;
            }
        }

        #endregion

        #region Ӧ��ID

        /// <summary>
        /// Ӧ��ID
        /// </summary>
        public string AppID
        {
            get
            {
                if (ViewState["frm_Issue_Base_AppID"] != null)
                {
                    return ViewState["frm_Issue_Base_AppID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["frm_Issue_Base_AppID"] = value;
            }
        }

        #endregion

        #region ���̻���ID

        /// <summary>
        /// ���̻���ID
        /// </summary>
        public string NodeModelID
        {
            get
            {
                if (ViewState["frm_ChangeBase_NodeModelID"] != null)
                {
                    return ViewState["frm_ChangeBase_NodeModelID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["frm_ChangeBase_NodeModelID"] = value;
            }
        }

        #endregion


        #region ��Ϣ���

        /// <summary>
        /// ȡ������MessageID
        /// </summary>
        public string MessageID
        {
            get
            {
                if (ViewState["MessageID"] != null)
                {
                    return ViewState["MessageID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["MessageID"] = value;
            }
        }

        #endregion

        #region ��ʱ�洢XML�ļ�·��

        /// <summary>
        /// ��ʱ�洢XML�ļ�·��
        /// </summary>
        public string DeskChangeUrl
        {
            get
            {
                if (hidDeskChange.Value == "")
                {
                    hidDeskChange.Value = CommonDP.GetConfigValue("TempCataLog", "TempCataLog") + "DeskChange" + Session["UserID"].ToString() + CTools.GetRandom() + ".xml";
                    return hidDeskChange.Value;
                }
                return hidDeskChange.Value;
            }
        }

        #endregion

        #region ���ID

        /// <summary>
        /// ���ID
        /// </summary>
        protected string ChangeID
        {
            get { if (ViewState["ChangeID"] != null) return ViewState["ChangeID"].ToString(); else return "0"; }
            set { ViewState["ChangeID"] = value; }
        }

        #endregion

        #region ������Ƿ�������¼�
        public string ChangeFlowID
        {
            get
            {
                if (ViewState["ChangeFlowID"] != null)
                {
                    return ViewState["ChangeFlowID"].ToString();
                }
                else
                {
                    return "";
                }
            }
        }
        #endregion

        #region ������Ƿ����������
        public string FromProblemFlowID
        {
            get
            {
                if (ViewState["FromProblemFlowID"] != null)
                {
                    return ViewState["FromProblemFlowID"].ToString();
                }
                else
                {
                    return "";
                }
            }
        }
        #endregion

        #region ��ӡģʽ

        /// <summary>
        /// ��ӡ��ʽ 0 IE��ʽ��1 Report Service��ʽ
        /// </summary>
        public string PrintMode
        {
            get
            {
                return CommonDP.GetConfigValue("PrintMode", "PrintMode").ToString();
            }
        }
        #endregion

        #endregion

        #region Page_Load

        /// <summary>
        /// ҳ�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            myFlowForms = (FlowForms)this.Master;

            myFlowForms.mySetContentReadOnly += new FlowForms.DoContentActions(Master_mySetContentVisible);
            myFlowForms.myGetFormsValue += new FlowForms.GetFormsValue(Master_myGetFormsValue);
            myFlowForms.mySetFormsValue += new FlowForms.DoContentActions(Master_mySetFormsValue);

            myFlowForms.myPreClickCustomize += new FlowForms.DoContentSubmitValid(myFlowForms_myPreClickCustomize);
            myFlowForms.myPreSaveClickCustomize += new FlowForms.DoContentValid(myFlowForms_myPreSaveClickCustomize);

            myFlowForms.blnSMSNotify = true;
            myFlowForms.blnEmail = true;

            if (IsPostBack == false)
            {
                PageDeal.SetLanguage(this.Controls[0].Controls[1]);
            }
            else
            {
                lngFromFlowID = long.Parse(ViewState["frm_Problem_FromFlowID"].ToString());
                lngProblemFlowID = long.Parse(ViewState["frm_FromProblemFlowID"].ToString());            //���ⵥ��������FlowID

                if (txtCustAddr.Text.Trim() == string.Empty)
                    txtCustAddr.Text = hidCust.Value.Trim();
                if (txtContact.Text.Trim() == string.Empty)
                    txtContact.Text = hidContact.Value.Trim();
                if (txtCTel.Text.Trim() == string.Empty)
                    txtCTel.Text = hidTel.Value.Trim();
                if (txtAddr.Text.Trim() == string.Empty)
                    txtAddr.Text = hidAddr.Value.Trim();
                if (lblCustDeptName.Text.Trim() == string.Empty || hidCustDeptName.Value.Trim() != string.Empty)
                    lblCustDeptName.Text = hidCustDeptName.Value.Trim();
                if (lblEmail.Text.Trim() == string.Empty || hidCustEmail.Value.Trim() != string.Empty)
                    lblEmail.Text = hidCustEmail.Value.Trim();
                if (lblMastCust.Text.Trim() == string.Empty)
                    lblMastCust.Text = hidMastCust.Value.Trim();
                if (hidjob.Value.Trim() != string.Empty || lbljob.Text.Trim() == string.Empty)
                    lbljob.Text = hidjob.Value.Trim();
            }

            if (!IsPostBack)
            {
                if (this.chkChangePlace.SelectedItem == null)
                {
                    GetChangePlace();
                }
                string strThisMsg = "";
                strThisMsg = txtCustAddr.ClientID + ">" + "�ͻ�����";
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), txtCustAddr.ClientID, "<script type=\"text/javascript\" >if(typeof(sarrvalidlist) == 'undefined'){ var sarrvalidlist = new Array(1);}  sarrvalidlist[0] = sarrvalidlist[0] + '|' + '" + strThisMsg + "';</script>");
            }

            if (hidEquIsHid.Value == "1")            //�����ʲ�Ϊֻ��
                SetFareDetailReadOnly();

            if (hidEquIsHid.Value == "2")            //�����ʲ�Ϊ���ɼ�
                gvBillItem.Visible = false;

            if (this.hidAppID.Value != "0")
            {
                //Extension_DayCtrList1.EquCategoryID = long.Parse(this.hidAppID.Value.ToString());
                EpowerCom.objFlow oFlow2 = new objFlow(long.Parse(Session["UserID"].ToString()), long.Parse(FlowModelID), long.Parse(MessageID));
                Extension_DayCtrList1.NodeModelID = oFlow2.NodeModelID;

                Extension_DayCtrList1.EquID = long.Parse(this.hidFlowID.Value.ToString());
                Extension_DayCtrList1.EquCategoryID = oFlow2.FlowModelID;
                
                
            }
        }

        #endregion

        #region ��ȡ����¼���Ϣ BindRelItemData

        /// <summary>
        /// ��ȡ����¼���Ϣ
        /// </summary>
        /// <param name="lngFlowID"></param>
        private void BindRelItemData(long lngFlowID)
        {
            DataTable dt = ChangeDealDP.GetIssuesForChange(lngFlowID);
            gridAttention.DataSource = dt.DefaultView;
            gridAttention.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");

            gridAttention.DataBind();
        }

        #endregion

        #region ��ȡ������ⵥ
        /// <summary>
        /// ��ȡ������ⵥ
        /// </summary>
        /// <param name="lngFlowID"></param>
        private void BindRelProblemData(long lngFlowID)
        {
            DataTable dt = ChangeDealDP.GetProblemsByChange(lngFlowID);
            dgRelProblem.DataSource = dt.DefaultView;
            dgRelProblem.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");

            dgRelProblem.DataBind();
        }

        #endregion

        #region ��ȡ URL
        /// <summary>
        /// ��ȡURL
        /// </summary>
        /// <param name="lngNoticeID"></param>
        /// <returns></returns>
        protected string GetUrl(decimal lngFlowID)
        {
            //��ʱû�����ҳ
            string sUrl = "";
            sUrl = "javascript:window.open('../Forms/frmIssueView.aspx?NewWin=true&FlowID=" + lngFlowID.ToString() + "','','scrollbars=yes,resizable=yes,top=0,left=0');";
            return sUrl;
        }

        #endregion

        #region ��ȡ������ʲ���״̬ GetEquStatus

        /// <summary>
        /// ��ȡ������ʲ���״̬
        /// </summary>
        /// <returns></returns>
        private bool GetEquStatus()
        {
            bool breturn = true;
            if (this.hidFlowID.Value.Trim() == "0" && lngFromFlowID != 0)
            {
                if (Session["ExtendParameter"] != null && Session["ExtendParameter"].ToString() != string.Empty && Session["ExtendParameter"].ToString() != "0")
                {
                    //����Ƿ��Ѿ�ת��Ϊ���ⵥ
                    if (ProblemDealDP.CheckIsChangeProblem(lngFromFlowID))
                    {
                        Epower.DevBase.BaseTools.PageTool.MsgBox(this, "���¼�������Ϊ���⣬������������");
                        breturn = false;
                    }
                }
            }
            return breturn;
        }

        #endregion

        #region Master_mySetFormsValue

        /// <summary>
        /// ��ҳ����ֵ
        /// </summary>
        private void Master_mySetFormsValue()
        {
            #region ���Գ�ʼ��

            oFlow = myFlowForms.oFlow;//���̶���
            myFlowForms.FormTitle = myFlowForms.oFlow.FlowName;//���̱���
            this.FlowID = myFlowForms.oFlow.FlowID.ToString();//����ID
            this.FlowModelID = myFlowForms.oFlow.FlowModelID.ToString();//����ģ��ID
            this.MessageID = myFlowForms.oFlow.MessageID.ToString();
            this.AppID = myFlowForms.oFlow.AppID.ToString();//Ӧ��ID
            this.NodeModelID = myFlowForms.oFlow.NodeModelID.ToString();//����ID
            hidFlowID.Value = myFlowForms.oFlow.FlowID.ToString();//����ID

            this.hidAppID.Value = this.AppID;

            if (oFlow.FlowStatus == e_FlowStatus.efsEnd)
            {
                myFlowForms.CtrButtons1.Button3Visible = true;
                myFlowForms.CtrButtons1.ButtonName3 = "֪ʶ�鵵";
                myFlowForms.CtrButtons1.Button3Function = "DoKmAdd(" + oFlow.MessageID.ToString() + "," + oFlow.AppID.ToString() + "," + oFlow.FlowID.ToString() + ");";
            }
            #endregion

            #region Master_mySetFormsValue

            DataTable dt = new DataTable();
            if (myFlowForms.oFlow.MessageID != 0)
            {
                #region ��ȡ����

                ImplDataProcess dp = new ImplDataProcess(myFlowForms.oFlow.AppID);//ʵ�ֽӿ�
                DataSet ds = dp.GetFieldsDataSet(myFlowForms.oFlow.FlowID, myFlowForms.oFlow.OpID);//��ȡ����
                dt = ds.Tables[0];

                #endregion
            }
            else
            {
                #region �ڲ�ģʽ����ȱʡ�� �ͻ�����

                CtrDTCustTime.dateTime = DateTime.Now;//���ʱ��
                if (CommonDP.GetConfigValue("Other", "ItsmCustomerMode") == "0")
                {
                    //�ڲ�ģʽ����ȱʡ�� �ͻ�����
                    txtCustAddr.Text = Session["UserDefaultCustomerName"].ToString();
                    hidCustID.Value = Session["UserDefaultCustomerID"].ToString();
                    if (hidCustID.Value != "0" && hidCustID.Value != "")
                    {
                        //ȡ�ÿͻ�����
                        Br_ECustomerDP ec = new Br_ECustomerDP();
                        ec = ec.GetReCorded(long.Parse(hidCustID.Value));
                        hidCustID.Value = ec.ID.ToString();
                        lblCustDeptName.Text = ec.CustDeptName;
                        hidCustDeptName.Value = ec.CustDeptName;
                        hidCust.Value = ec.ShortName;
                        txtCustAddr.Text = ec.ShortName;
                        lblEmail.Text = ec.Email;
                        hidCustEmail.Value = ec.Email;
                        lblMastCust.Text = ec.ShortName;
                        lbljob.Text = ec.Job;
                        hidjob.Value = ec.Job;

                        txtAddr.Text = ec.Address;
                        hidAddr.Value = ec.Address;
                        lblAddr.Text = ec.Address;
                        txtContact.Text = ec.LinkMan1;
                        labContact.Text = txtContact.Text;
                        txtCTel.Text = ec.Tel1;
                        labCTel.Text = ec.Tel1;
                        lblMastCust.Text = ec.MastCustName;

                    }
                }

                #endregion
            }

            #endregion

            #region ��ȡ����

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    #region ��ֵҳ������

                    DataRow row = dt.Rows[0];

                    //�ǵ�����Ϣ
                    UserPicker1.UserName = row["RegUserName"].ToString();
                    if (row["RegUserID"].ToString().Length > 0)
                    {
                        UserPicker1.UserID = long.Parse(row["RegUserID"].ToString());
                    }

                    labServiceNo.Text = row["ChangeNo"].ToString();//�������
                    ChangeID = row["id"].ToString();
                    hidChangeId.Value = ChangeID;
                    CtrFlowFTSubject.Value = row["Subject"].ToString();
                    CtrDTCustTime.dateTime = DateTime.Parse(row["ChangeTime"].ToString());
                    CtrFlowReContent.Value = row["Content"].ToString();
                    CtrFCDEffect.CatelogID = long.Parse(row["EffectID"].ToString());
                    CtrFCDInstancy.CatelogID = long.Parse(row["InstancyID"].ToString());
                    CtrFCDlevel.CatelogID = long.Parse(row["LevelID"].ToString());

                    hidCustID.Value = row["CustID"].ToString();    //�ͻ�
                    txtCustAddr.Text = row["CustName"].ToString();
                    labCustAddr.Text = StringTool.ParseForHtml(txtCustAddr.Text);
                    txtAddr.Text = row["CustAddress"].ToString();
                    hidAddr.Value = row["CustAddress"].ToString();
                    lblAddr.Text = row["CustAddress"].ToString();
                    txtContact.Text = row["Contact"].ToString();
                    labContact.Text = StringTool.ParseForHtml(txtContact.Text);
                    txtCTel.Text = row["ctel"].ToString();
                    labCTel.Text = StringTool.ParseForHtml(txtCTel.Text);

                    //��ȡ�ͻ������Ϣ
                    if (hidCustID.Value.Trim() != "0" && hidCustID.Value.Trim() != "")
                    {
                        Br_ECustomerDP ec = new Br_ECustomerDP();
                        ec = ec.GetReCorded(long.Parse(hidCustID.Value));
                        lblCustDeptName.Text = ec.CustDeptName;
                        hidCustDeptName.Value = ec.CustDeptName;
                        lbljob.Text = ec.Job;
                        hidjob.Value = ec.Job;
                        hidCustEmail.Value = ec.Email;
                        lblEmail.Text = ec.Email;
                        lblMastCust.Text = ec.ShortName;

                        Br_MastCustomerDP mc = new Br_MastCustomerDP();
                        mc = mc.GetReCorded((long)ec.MastCustID);
                        lblMastCust.Text = mc.ShortName;
                    }

                    #region 20160720 add
                    if (row["CHANGE_PLACE_ID"].ToString() != "")//�������
                    {
                        string[] cg_placeID = row["CHANGE_PLACE_ID"].ToString().Split(',');
                        string[] cg_place = row["CHANGE_PLACE_NAME"].ToString().Split(',');
                        if (cg_place != null)
                        {
                            for (int i = 0; i < cg_placeID.Length; i++)
                            {
                                if (cg_placeID[i] != "")
                                {
                                    for (int j = 0; j < this.chkChangePlace.Items.Count; j++)
                                    {
                                        if (cg_placeID[i] == this.chkChangePlace.Items[j].Value)
                                        {
                                            this.chkChangePlace.Items[j].Selected = true;
                                            break;
                                        }
                                    }
                                    this.labChangePlace.Text += cg_place[i] + "��";
                                }
                            }
                        }
                        else//һ������
                        {
                            for (int j = 0; j < this.chkChangePlace.Items.Count; j++)
                            {
                                if (row["CHANGE_PLACE_ID"].ToString() == this.chkChangePlace.Items[j].Value)
                                {
                                    this.chkChangePlace.Items[j].Selected = true;
                                    break;
                                }
                            }
                            this.labChangePlace.Text = row["CHANGE_PLACE_NAME"].ToString();
                        }
                    }

                   
                    string Isplan = row["Isplan"].ToString();
                    if (Isplan != "��")
                    {
                        if (Isplan != "")
                        {
                            this.ddlIsplan.SelectedValue = "1";
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script>$('#div_isplan').show();</script>", false);
                        }
                        else
                        {
                            this.ddlIsplan.SelectedValue = "-1";
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script>$('#div_isplan').hide();</script>", false);
                        }
                    }
                    else
                    {
                        Isplan = "";
                        this.ddlIsplan.Visible = true;
                        this.ddlIsplan.SelectedValue = "0";
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script>$('#div_isplan').hide();</script>", false);
                    }
                    this.ctrChangeNeedPeople.UserID = Convert.ToInt64(row["ChangeNeedPeopleID"].ToString() == "" ? "0" : row["ChangeNeedPeopleID"]);//���������
                    ctrIsplan.Value = Isplan;//Ӧ��/���˷���
                    CtrBusEffect.SelValue = row["IS_BUS_EFFECT"].ToString();
                    CtrBusEffect.TextValue = row["BUS_EFFECT"].ToString();
                    CtrDataEffect.SelValue = row["IS_DATA_EFFECT"].ToString();
                    CtrDataEffect.TextValue = row["DATA_EFFECT"].ToString();
                    CtrPlanStartTime.dateTimeString = row["PLAN_BEGIN_TIME"].ToString();
                    CtrPlanEndTime.dateTimeString = row["PLAN_END_TIME"].ToString();                 
                    labIsPlanChange.Text = row["IS_PLAN_CHANGE"].ToString() == "" || row["IS_PLAN_CHANGE"].ToString() == "0"? "��" : "��";
                    CtrStopServer.TextValue = row["STOP_SERVER_REMARK"].ToString();
                    CtrStopServer.SelValue = row["IS_STOP_SERVER"].ToString();
                    CtrChangeWindow.SelValue = row["CHANGE_WINDOW_ID"].ToString();
                    CtrChangeWindow.TextValue = row["CHANGE_WINDOW_REMARK"].ToString();
                    CtrRealStartTime.dateTimeString = row["REAL_BEGIN_TIME"].ToString();
                    //ʵ�ʽ���ʱ��
                    CtrRealEndTime.dateTimeString = row["REAL_END_TIME"].ToString();
                    #endregion


                    #region �������

                    CtrFlowReChangeAnalyses.Value = row["ChangeAnalyses"].ToString();

                    #endregion

                    #region �������

                    CtrFlowReChangeAnalysesResult.Value = row["ChangeAnalysesResult"].ToString();

                    #endregion

                    #region ������

                    CtrChangeType.CatelogID = Convert.ToInt32(row["ChangeTypeID"]);
                    CtrChangeType.CatelogValue = row["ChangeTypeName"].ToString();

                    #endregion

                    #region �Ǽ�ʱ��

                    CtrDTRegTime.dateTime = DateTime.Parse(row["RegTime"].ToString());

                    #endregion

                    #region ���״̬

                    if (row["DealStatusID"].ToString() != string.Empty)
                        CtrFCDDealStatus.CatelogID = long.Parse(row["DealStatusID"].ToString());

                    #endregion

                    if (row["IssuesFlowID"].ToString() != string.Empty && row["IssuesFlowID"].ToString() != "0")
                    {
                        BindRelItemData(long.Parse(row["FlowID"].ToString()));
                        ViewState["ChangeFlowID"] = row["IssuesFlowID"].ToString();
                    }
                    else
                    {
                        ViewState["ChangeFlowID"] = "0";
                    }

                    if (row["ProblemFlowID"].ToString() != string.Empty && row["ProblemFlowID"].ToString() != "0")
                    {
                        BindRelProblemData(long.Parse(row["FlowID"].ToString()));
                        ViewState["FromProblemFlowID"] = row["ProblemFlowID"].ToString();
                    }
                    else
                    {
                        ViewState["FromProblemFlowID"] = "0";
                    }

                    #endregion
                }
                else
                {
                    #region �¼������ɱ��

                    if (Session["ExtendParameter"] != null && Session["ExtendParameter"].ToString() != string.Empty && Session["ExtendParameter"].ToString() != "0")
                    {
                        //ͨ����ȡ
                        long lngFlowID = long.Parse(Session["ExtendParameter"].ToString());
                        //����Ƿ��ѹ��������
                        if (ChangeDealDP.CheckIsChangeProblem(lngFlowID))
                        {
                            Epower.DevBase.BaseTools.PageTool.MsgBox(this, "���¼�����������������ٹ�����");
                            Epower.DevBase.BaseTools.PageTool.AddJavaScript(this, "window.close();");
                        }

                        DataTable dtIssue = ZHServiceDP.GetDataByFlowID(lngFlowID);
                        if (dtIssue.Rows.Count > 0)
                        {
                            Br_ECustomerDP ec = new Br_ECustomerDP();
                            ec = ec.GetReCorded(long.Parse(dtIssue.Rows[0]["CustID"].ToString()));
                            hidCustID.Value = ec.ID.ToString();
                            hidCust.Value = ec.ShortName;
                            txtCustAddr.Text = ec.ShortName;
                            hidCustEmail.Value = ec.Email;
                            lblEmail.Text = ec.Email;
                            lblMastCust.Text = ec.ShortName;

                            lblCustDeptName.Text = ec.CustDeptName;
                            hidCustDeptName.Value = ec.CustDeptName;
                            lbljob.Text = ec.Job;
                            hidjob.Value = ec.Job;

                            txtAddr.Text = ec.Address;
                            hidAddr.Value = ec.Address;
                            lblAddr.Text = ec.Address;
                            txtContact.Text = ec.LinkMan1;
                            labContact.Text = txtContact.Text;
                            txtCTel.Text = ec.Tel1;
                            labCTel.Text = ec.Tel1;
                            lblMastCust.Text = ec.MastCustName;

                            DataRow dr = dtIssue.Rows[0];

                            if (dr["subject"] != null)
                                CtrFlowFTSubject.Value = dr["subject"].ToString();
                            if (dr["content"] != null)
                                CtrFlowReContent.Value = dr["content"].ToString();

                            #region ���¼����������ʲ�
                            dt = ZHServiceDP.GetIssuesByFlowID(lngFlowID);
                            if (dt.Rows.Count > 0 && dt.Rows[0]["EQUID"] != null && dt.Rows[0]["EQUID"].ToString() != "" && dt.Rows[0]["EQUID"].ToString() != "0")
                            {

                                dt = CheckUnSaveDataAndLoadIt(dt);

                                gvBillItem.DataSource = dt.DefaultView;
                                gvBillItem.DataBind();
                            }
                            else
                            {
                                BindGrid(long.Parse(ChangeID));
                            }
                        }

                        lngFromFlowID = lngFlowID;

                        myFlowForms.TempFlowID = lngFlowID;

                    }
                    ViewState["ChangeFlowID"] = "0";
                            #endregion

                    #region ���ⷢ����

                    if (Session["ProblemFlowID"] != null && Session["ProblemFlowID"].ToString() != string.Empty && Session["ProblemFlowID"].ToString() != "0")
                    {
                        //ͨ����ȡ
                        long lngFlowID = long.Parse(Session["ProblemFlowID"].ToString());
                        //����Ƿ��ѹ��������
                        if (ChangeDealDP.CheckIsChangeFromPro(lngFlowID))
                        {
                            Epower.DevBase.BaseTools.PageTool.MsgBox(this, "�������ѹ���������������ٹ�����");
                            Epower.DevBase.BaseTools.PageTool.AddJavaScript(this, "window.close();");
                        }

                        DataTable dtPro = ProblemDealDP.GetDataByFlowID(lngFlowID);
                        if (dtPro.Rows.Count > 0)
                        {
                            #region �����⴫�������û���Ϣ
                            Br_ECustomerDP ec = new Br_ECustomerDP();
                            ec = ec.GetReCordedByUserID(dtPro.Rows[0]["RegUserID"].ToString());
                            hidCustID.Value = ec.ID.ToString();
                            hidCust.Value = ec.ShortName;
                            txtCustAddr.Text = ec.ShortName;
                            hidCustEmail.Value = ec.Email;
                            lblEmail.Text = ec.Email;
                            lblMastCust.Text = ec.ShortName;

                            lblCustDeptName.Text = ec.CustDeptName;
                            hidCustDeptName.Value = ec.CustDeptName;
                            lbljob.Text = ec.Job;
                            hidjob.Value = ec.Job;

                            txtAddr.Text = ec.Address;
                            hidAddr.Value = ec.Address;
                            lblAddr.Text = ec.Address;
                            txtContact.Text = ec.LinkMan1;
                            labContact.Text = txtContact.Text;
                            txtCTel.Text = ec.Tel1;
                            labCTel.Text = ec.Tel1;
                            lblMastCust.Text = ec.MastCustName;

                            DataRow dr = dtPro.Rows[0];
                            if (dr["problem_title"] != null)
                                CtrFlowFTSubject.Value = dr["problem_title"].ToString();
                            if (dr["problem_subject"] != null)
                                CtrFlowReContent.Value = dr["problem_subject"].ToString();
                            #endregion

                            #region �����⴫�������ʲ�
                            dt = ChangeDealDP.GetProblemsByFlowID(lngFlowID);
                            if (dt.Rows.Count > 0 && dt.Rows[0]["EQUID"] != null && dt.Rows[0]["EQUID"].ToString() != "" && long.Parse(dt.Rows[0]["EQUID"].ToString()) != -1)
                            {

                                dt = CheckUnSaveDataAndLoadIt(dt);

                                gvBillItem.DataSource = dt.DefaultView;
                                gvBillItem.DataBind();
                            }
                            else
                            {
                                BindGrid(long.Parse(ChangeID));
                            }
                            #endregion
                        }

                        lngProblemFlowID = lngFlowID;
                        myFlowForms.TempFlowID = lngFlowID;
                    }

                    ViewState["FromProblemFlowID"] = "0";
                    #endregion

                    #endregion
                }
            }

            if (UserPicker1.UserID == null || UserPicker1.UserID == 0)
            {
                UserPicker1.UserID = long.Parse(Session["UserID"].ToString());
                UserPicker1.UserName = Session["PersonName"].ToString();
            }

            if ((Session["ProblemFlowID"] == null || Session["ProblemFlowID"].ToString() == "") && (Session["ExtendParameter"] == null || Session["ExtendParameter"].ToString() == ""))
                BindGrid(long.Parse(ChangeID));



            #endregion

            #region set visible

            setFieldCollection setFields = myFlowForms.oFlow.setFields;
            foreach (setField sf in setFields)
            {
                if (sf.Editable == true && sf.Visibled == true)
                {
                    continue;
                }
                switch (sf.Name.ToLower())
                {
                    case "changetypeid":
                        CtrChangeType.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrChangeType.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    case "effectid":
                        CtrFCDEffect.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrFCDEffect.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    case "instancyid":
                        CtrFCDInstancy.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrFCDInstancy.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    case "levelid":
                        CtrFCDlevel.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrFCDlevel.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    case "subject":
                        CtrFlowFTSubject.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrFlowFTSubject.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    case "changetime":
                        CtrDTCustTime.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrDTCustTime.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    case "content":

                        CtrFlowReContent.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrFlowReContent.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    case "dealstatusid":
                        CtrFCDDealStatus.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrFCDDealStatus.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;

                    //�ͻ���Ϣ
                    case "custinfo":
                        txtCustAddr.Visible = false;    // �ͻ�����
                        cmdCust.Visible = false;
                        txtAddr.Visible = false;    // �ͻ���ַ
                        txtContact.Visible = false;    // ��ϵ��
                        txtCTel.Visible = false;    // ��ϵ�˵绰
                        lblMustIn.Visible = false;
                        if (sf.Visibled == true)
                        {
                            labCustAddr.Visible = true;
                            lblAddr.Visible = true;
                            labContact.Visible = true;
                            labCTel.Visible = true;
                            lblCustDeptName.Visible = true;
                        }

                        if (sf.Visibled == false)
                        {
                            Table12.Visible = false;
                            Table2.Visible = false;

                        }
                        break;
                    //�ʲ���Ϣ
                    case "equipmentname":
                        SetFareDetailvisible("equipmentname", sf.Visibled);
                        break;
                    case "impactanalysis":
                        SetFareDetailvisible("impactanalysis", sf.Visibled);
                        break;
                    //������Ϣ
                    case "changeanalyses":
                        CtrFlowReChangeAnalyses.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrFlowReChangeAnalyses.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    case "changeanalysesresult":
                        CtrFlowReChangeAnalysesResult.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrFlowReChangeAnalysesResult.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    case "equchange":
                        SetFareDetailvisible("equchange", sf.Visibled);
                        break;
                    case "equ_expr_property":    // ��չ���� - 2013-11-19 @������
                        Extension_DayCtrList1.ReadOnly = true;
                        if (!sf.Visibled)
                        {
                            Extension_DayCtrList1.Visible = false;
                        }
                        break;
                    #region 20160720 add                    
                    //�������
                    case "change_place_id":
                        //CtrChangePlace.ContralState = eOA_FlowControlState.eReadOnly;
                        //if (sf.Visibled == false)
                        //{
                        //    CtrChangePlace.ContralState = eOA_FlowControlState.eHidden;
                        //}
                        this.chkChangePlace.Visible = false;
                        this.labChangePlace.Visible = true;
                        spPlace.Visible = false;
                        if (sf.Visibled == false)
                        {
                            this.labChangePlace.Visible = false;
                        }
                        break;
                    //���������
                    case "changeneedpeopleid":
                        ctrChangeNeedPeople.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            ctrChangeNeedPeople.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;                   
                    //Ӧ��/���˷���
                    case "isplan":
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script>$('#div_isplan').show();</script>", false);
                        this.ddlIsplan.Enabled = false;
                        ctrIsplan.ContralState = eOA_FlowControlState.eReadOnly;
                        spPlan.Visible = false;
                        if (sf.Visibled == false)
                        {
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script>$('#div_isplan').hide();</script>", false);
                            ctrIsplan.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    //�Ƿ�ҵ��Ӱ��
                    case "is_bus_effect":
                        CtrBusEffect.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrBusEffect.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    //�Ƿ�����Ӱ��
                    case "is_data_effect":
                        CtrDataEffect.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrDataEffect.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    //�Ƿ�ͣ�÷���
                    case "is_stop_server":
                        CtrStopServer.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrStopServer.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    case "is_plan_change":
                        if (sf.Visibled == false)
                        {
                            this.labIsPlanChange.Visible = false;
                        }
                        break;
                    //�������
                    case "change_window_id":
                        CtrChangeWindow.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrChangeWindow.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;

                    //�ƻ���ʼʱ��
                    case "plan_begin_time":
                        CtrPlanStartTime.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrPlanStartTime.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    //�ƻ�����ʱ��
                    case "plan_end_time":
                        CtrPlanEndTime.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrPlanEndTime.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    //ʵ�ʿ�ʼʱ��
                    case "real_begin_time":
                        CtrRealStartTime.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrRealStartTime.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    //ʵ�ʽ���ʱ��
                    case "real_end_time":
                        CtrRealEndTime.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrRealEndTime.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    #endregion
                    default:
                        break;

                }
            }

            //�����κ����,����IDΪ0ʱ Ҳ���ɼ�
            if (myFlowForms.oFlow.FlowID == 0)
            {
                trChange.Visible = false;
            }
            #endregion

            Extension_DayCtrList1.EquID = long.Parse(this.hidFlowID.Value.ToString());

            if (this.hidAppID.Value.ToString() != "0")
            {
                //Extension_DayCtrList1.EquCategoryID = long.Parse(this.hidAppID.Value.ToString());
                Extension_DayCtrList1.NodeModelID = this.FlowMaster.oFlow.NodeModelID;
                Extension_DayCtrList1.EquCategoryID = this.FlowMaster.oFlow.FlowModelID;


            }

            ViewState["frm_Problem_FromFlowID"] = lngFromFlowID;
            ViewState["frm_FromProblemFlowID"] = lngProblemFlowID;
        }
        #endregion

        #region Master_mySetContentVisible

        /// <summary>
        /// ҳ������
        /// </summary>
        private void Master_mySetContentVisible()
        {
            #region Master_mySetContentVisible

            #region ����

            if (CtrFlowFTSubject.ContralState != eOA_FlowControlState.eHidden)
                CtrFlowFTSubject.ContralState = eOA_FlowControlState.eReadOnly;

            #endregion

            #region ���ʱ��

            if (CtrDTCustTime.ContralState != eOA_FlowControlState.eHidden)
                CtrDTCustTime.ContralState = eOA_FlowControlState.eReadOnly;

            #endregion

            #region ��������

            if (CtrFlowReContent.ContralState != eOA_FlowControlState.eHidden)
                CtrFlowReContent.ContralState = eOA_FlowControlState.eReadOnly;

            #endregion

            #region ��������

            if (CtrFCDDealStatus.ContralState != eOA_FlowControlState.eHidden)
                CtrFCDDealStatus.ContralState = eOA_FlowControlState.eReadOnly;

            #endregion

            #region Ӱ���

            if (CtrFCDEffect.ContralState != eOA_FlowControlState.eHidden)
                CtrFCDEffect.ContralState = eOA_FlowControlState.eReadOnly;

            #endregion

            #region ������

            if (CtrFCDInstancy.ContralState != eOA_FlowControlState.eHidden)
                CtrFCDInstancy.ContralState = eOA_FlowControlState.eReadOnly;

            #endregion

            #region �������

            if (CtrFCDlevel.ContralState != eOA_FlowControlState.eHidden)
                CtrFCDlevel.ContralState = eOA_FlowControlState.eReadOnly;

            #endregion

            #region ������

            if (CtrChangeType.ContralState != eOA_FlowControlState.eHidden)
                CtrChangeType.ContralState = eOA_FlowControlState.eReadOnly;

            #endregion

            #region ��ϵ��

            if (txtContact.Visible == true)
                labContact.Visible = true;
            txtContact.Visible = false;

            #endregion

            #region ��ϵ�绰


            if (txtCTel.Visible == true)
                labCTel.Visible = true;
            txtCTel.Visible = false;

            #endregion

            #region ��ϵ��ַ

            if (txtCustAddr.Visible == true)
                labCustAddr.Visible = true;
            txtCustAddr.Visible = false;
            cmdCust.Visible = false;
            lblMustIn.Visible = false;

            if (txtAddr.Visible == true)
                lblAddr.Visible = true;
            txtAddr.Visible = false;

            #endregion

            #region �������

            if (CtrFlowReChangeAnalyses.ContralState != eOA_FlowControlState.eHidden)
                CtrFlowReChangeAnalyses.ContralState = eOA_FlowControlState.eReadOnly;

            #endregion

            #region �������

            if (CtrFlowReChangeAnalysesResult.ContralState != eOA_FlowControlState.eHidden)
                CtrFlowReChangeAnalysesResult.ContralState = eOA_FlowControlState.eReadOnly;

            #endregion

            trChange.Visible = false;

            SetFareDetailReadOnly();
            gvBillItem.Columns[gvBillItem.Columns.Count - 1].Visible = false;//�ʲ����
            gvBillItem.Columns[gvBillItem.Columns.Count - 3].Visible = false;//Ӱ��ȷ���
            #endregion

           
            #region �Ƿ�ƻ��Ա��
            if (this.labIsPlanChange.Visible == true)
            {
            }
            #endregion
            #region �������            
            this.chkChangePlace.Visible = false;
            this.labChangePlace.Visible = true;
            spPlace.Visible = false;
            #endregion
            #region ���������
            if (this.ctrChangeNeedPeople.ContralState != eOA_FlowControlState.eHidden)
            {
                this.ctrChangeNeedPeople.ContralState = eOA_FlowControlState.eReadOnly;
            }
            #endregion
            #region Ӧ��/���˷���
            if (this.ctrIsplan.ContralState != eOA_FlowControlState.eHidden)
            {
                this.ddlIsplan.Enabled = false;
                spPlan.Visible = false;
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script>$('#div_isplan').show();</script>", false);
                this.ctrIsplan.ContralState = eOA_FlowControlState.eReadOnly;
            }
            #endregion
            #region �������
            if (CtrChangeWindow.ContralState != eOA_FlowControlState.eHidden)
                CtrChangeWindow.ContralState = eOA_FlowControlState.eReadOnly;
            #endregion
            #region �Ƿ�ͣ�÷���
            if (CtrStopServer.ContralState != eOA_FlowControlState.eHidden)
                CtrStopServer.ContralState = eOA_FlowControlState.eReadOnly;
            #endregion
            #region �Ƿ�ҵ��Ӱ��
            if (CtrBusEffect.ContralState != eOA_FlowControlState.eHidden)
                CtrBusEffect.ContralState = eOA_FlowControlState.eReadOnly;
            #endregion
            #region �Ƿ�����Ӱ��
            if (CtrDataEffect.ContralState != eOA_FlowControlState.eHidden)
                CtrDataEffect.ContralState = eOA_FlowControlState.eReadOnly;
            #endregion
            #region �ƻ���ʼʱ��
            if (CtrPlanStartTime.ContralState != eOA_FlowControlState.eHidden)
                CtrPlanStartTime.ContralState = eOA_FlowControlState.eReadOnly;
            #endregion
            #region �ƻ����ʱ��
            if (CtrPlanEndTime.ContralState != eOA_FlowControlState.eHidden)
                CtrPlanEndTime.ContralState = eOA_FlowControlState.eReadOnly;
            #endregion
            #region ʵ�ʿ�ʼʱ��
            if (CtrRealStartTime.ContralState != eOA_FlowControlState.eHidden)
                CtrRealStartTime.ContralState = eOA_FlowControlState.eReadOnly;
            #endregion
            //ʵ�ʽ���ʱ��
            if (CtrRealEndTime.ContralState != eOA_FlowControlState.eHidden)
                CtrRealEndTime.ContralState = eOA_FlowControlState.eReadOnly;

            Extension_DayCtrList1.ReadOnly = true;    // ��չ���� - 2013-11-19 @������

            if (this.hidAppID.Value != "0")
            {
                //Extension_DayCtrList1.EquCategoryID = long.Parse(this.hidAppID.Value.ToString());
                Extension_DayCtrList1.NodeModelID = this.FlowMaster.oFlow.NodeModelID;

                Extension_DayCtrList1.EquID = long.Parse(this.hidFlowID.Value.ToString());
                Extension_DayCtrList1.EquCategoryID = this.FlowMaster.oFlow.FlowModelID;
                
            }            
        }

        #endregion

        #region Master_myGetFormsValue

        /// <summary>
        /// ���ݸ�ֵ
        /// </summary>
        /// <param name="lngActionID"></param>
        /// <param name="strActionName"></param>
        /// <returns></returns>
        private XmlDocument Master_myGetFormsValue(long lngActionID, string strActionName)
        {
            #region Master_myGetFormsValue

            FieldValues fv = new FieldValues();
            fv.Add("EffectID", CtrFCDEffect.CatelogID.ToString().Trim());
            fv.Add("EffectName", CtrFCDEffect.CatelogValue.Trim());
            fv.Add("InstancyID", CtrFCDInstancy.CatelogID.ToString().Trim());
            fv.Add("InstancyName", CtrFCDInstancy.CatelogValue.Trim());
            fv.Add("LevelID", CtrFCDlevel.CatelogID.ToString().Trim());
            fv.Add("LevelName", CtrFCDlevel.CatelogValue.Trim());
            fv.Add("ChangeTypeID", CtrChangeType.CatelogID.ToString().Trim());
            fv.Add("ChangeTypeName", CtrChangeType.CatelogValue.ToString().Trim());
            string sServiceNo = this.labServiceNo.Text.Trim();
            if (sServiceNo == string.Empty)
            {
                sServiceNo = RuleCodeDP.GetCodeBH(10005);
            }
            fv.Add("ChangeNo", sServiceNo);
            fv.Add("Subject", CtrFlowFTSubject.Value.Trim());
            fv.Add("ChangeTime", CtrDTCustTime.dateTime.ToString("yyyy-MM-dd H:mm:ss"));
            fv.Add("Content", CtrFlowReContent.Value.Trim());
            fv.Add("Reason", String.Empty);
            fv.Add("CustID", this.hidCustID.Value.Trim() == string.Empty ? "0" : this.hidCustID.Value.Trim());
            fv.Add("CustName", txtCustAddr.Text.Trim());
            fv.Add("CustAddress", txtAddr.Text.Trim());
            fv.Add("Contact", txtContact.Text.Trim());
            fv.Add("CTel", txtCTel.Text.Trim());
            fv.Add("EquipmentID", "0");
            fv.Add("EquipmentName", "");
            fv.Add("ChangeAnalyses", CtrFlowReChangeAnalyses.Value.Trim());
            fv.Add("ChangeAnalysesResult", CtrFlowReChangeAnalysesResult.Value.Trim());
            fv.Add("Remark", string.Empty);
            fv.Add("DealStatusID", CtrFCDDealStatus.CatelogID.ToString());
            fv.Add("DealStatus", CtrFCDDealStatus.CatelogValue.Trim());
            fv.Add("RegUserID", Session["UserID"].ToString());
            fv.Add("RegUserName", Session["PersonName"].ToString());
            fv.Add("RegDeptID", Session["UserDeptID"].ToString());
            fv.Add("RegDeptName", Session["UserDeptName"].ToString());
            fv.Add("RegOrgID", Session["UserOrgID"].ToString());
            fv.Add("RegTime", DateTime.Now.ToString());
            fv.Add("FromFlowID", lngFromFlowID.ToString());
            fv.Add("EmailNotify", myFlowForms.EmailValue.ToString());
            fv.Add("SMSNotify", myFlowForms.NotifyValue.ToString());
            fv.Add("DeskChange", DeskChangeUrl);

            fv.Add("FromProblemFlowID", lngProblemFlowID.ToString());            //���ⵥ�����������ⵥFlowID

            string ExtensionDayList = SaveExtensionDayList();
            fv.Add("ExtensionDayList", ExtensionDayList);  //��չ��

            #region ���� �����ύʱ���� �ͻ�������� ����ǰ 2013-04-11
            //��������˸��ͻ����ʼ� ������ӵĴ���Żᷢ������
            fv.Add("Email", hidCustEmail.Value.ToString());
            #endregion

            #region 20160720 add
            string IsPlanChange = "";
            int week = Convert.ToInt32(DateTime.Today.DayOfWeek);//�ܼ�
            TimeSpan ts = this.CtrPlanStartTime.dateTime - DateTime.Now;
            int ts_day = Math.Abs(ts.Days);
            if (!IsInSameWeek(DateTime.Now, CtrPlanStartTime.dateTime))//��1��
            {
                IsPlanChange = "1";
            }
            else
            {
                IsPlanChange = "0";
            }
            fv.Add("IS_PLAN_CHANGE", IsPlanChange);
            fv.Add("ChangeNeedPeopleID", this.ctrChangeNeedPeople.UserID.ToString() == "" ? "0" : this.ctrChangeNeedPeople.UserID.ToString());//���������ID
            fv.Add("ChangeNeedPeople", this.ctrChangeNeedPeople.UserName);//���������
            string placeNameID = "";
            string placeName = "";
            for (int i = 0; i < this.chkChangePlace.Items.Count; i++)
            {
                if (this.chkChangePlace.Items[i].Selected)
                {
                    placeNameID += this.chkChangePlace.Items[i].Value + ",";
                    placeName += this.chkChangePlace.Items[i].Text + ",";
                }
            }
            fv.Add("Isplan", this.ddlIsplan.SelectedItem.Value == "1" ? this.ctrIsplan.Value.Trim() : this.ddlIsplan.SelectedItem.Text);//Ӧ��/���˷���
            fv.Add("CHANGE_PLACE_ID", placeNameID);
            fv.Add("CHANGE_PLACE_NAME", placeName);
            fv.Add("BUS_EFFECT", CtrBusEffect.TextValue);
            fv.Add("DATA_EFFECT", CtrDataEffect.TextValue);
            fv.Add("PLAN_BEGIN_TIME", CtrPlanStartTime.dateTimeString != "" ? CtrPlanStartTime.dateTime.ToString("yyyy-MM-dd HH:mm:ss") : "");
            fv.Add("PLAN_END_TIME", CtrPlanEndTime.dateTimeString != "" ? CtrPlanEndTime.dateTime.ToString("yyyy-MM-dd HH:mm:ss") : "");
            fv.Add("IS_BUS_EFFECT", CtrBusEffect.SelValue == "" ? "-1" : CtrBusEffect.SelValue);
            fv.Add("IS_DATA_EFFECT", CtrDataEffect.SelValue == "" ? "-1" : CtrDataEffect.SelValue);
            fv.Add("CHANGE_WINDOW_ID", CtrChangeWindow.SelValue == "" ? "0" : CtrChangeWindow.SelValue);
            fv.Add("CHANGE_WINDOW_NAME", CtrChangeWindow.SelText);
            fv.Add("CHANGE_WINDOW_REMARK", CtrChangeWindow.TextValue);
            fv.Add("IS_STOP_SERVER", CtrStopServer.SelValue == "" ? "-1" : CtrStopServer.SelValue);
            fv.Add("STOP_SERVER_REMARK", CtrStopServer.TextValue);
            fv.Add("REAL_BEGIN_TIME", CtrRealStartTime.dateTimeString != "" ? CtrRealStartTime.dateTime.ToString("yyyy-MM-dd HH:mm:ss") : "");
            //����ʱ��
            fv.Add("REAL_END_TIME", CtrRealEndTime.dateTimeString != "" ? CtrRealEndTime.dateTime.ToString("yyyy-MM-dd HH:mm:ss") : "");
            #endregion
            XmlDocument xmlDoc = fv.GetXmlObject();            

            #endregion


            return xmlDoc;
        }

        private string SaveExtensionDayList()
        {
            #region ��չ��

            List<EQU_deploy> list = Extension_DayCtrList1.contorRtnValue;

            string str = "";
            string strList = "";

            //����������Ϣ
            foreach (EQU_deploy deploy in list)
            {
                string[] strArray = new string[1];
                strList = deploy.ID + "@" + deploy.EquID + "@" + deploy.CHName + "@" + deploy.FieldID
                   + "@" + deploy.Value;
                str += strList + "&";

            }

            #endregion
            return str;
        }
        #endregion

        #region myFlowForms_myPreSaveClickCustomize

        /// <summary>
        /// �ݴ�ʱ��������
        /// </summary>
        /// <returns></returns>
        bool myFlowForms_myPreSaveClickCustomize()
        {
            bool blnRet = GetEquStatus();
            if (blnRet)
            {
                SaveDetailItem();
            }
            return blnRet;
        }
        #endregion

        #region �������� SaveDetailItem

        /// <summary>
        /// �����ʲ�����
        /// </summary>
        /// <returns></returns>
        private bool SaveDetailItem()
        {
            try
            {
                string equId = "";
                DataTable dt = GetDetailItem(true, 0, ref equId);
                gvBillItem.DataSource = dt.DefaultView;
                gvBillItem.DataBind();
                ChangeDealDP.SaveCLFareDetailItem(dt, DeskChangeUrl);
                Session["ProblemFlowID"] = null;
                Session["ExtendParameter"] = null;
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region �������� myFlowForms_myPreClickCustomize

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="lngActionID"></param>
        /// <param name="strActionName"></param>
        /// <returns></returns>
        bool myFlowForms_myPreClickCustomize(long lngActionID, string strActionName)
        {
            string hidId = "";
            DataTable dt = GetDetailItem(true, 0, ref hidId);
            gvBillItem.DataSource = dt.DefaultView;
            gvBillItem.DataBind();
            bool blnRet = GetEquStatus();
            long lngEquID = 0;
            if (blnRet == true && lngActionID > 0 && lngEquID > 0)
            {
                //�ж������Ӧ�ñ���������,����Ƿ��������Ѿ����
                long lngBusID = MessageDep.GetBusinessActionID(long.Parse(this.FlowModelID), long.Parse(this.NodeModelID), lngActionID);
                if (lngBusID == 10000)
                {
                    Equ_DeskDP ee = new Equ_DeskDP();
                    blnRet = ee.GetTempChangeExist(long.Parse(this.FlowID));
                }
                if (blnRet == false)
                {
                    blnRet = true;
                }
            }

            //�ж���ϸ���еķ��಻��Ϊ��
            bool IsFillTypeName = false;
            string atxtEquName = "";
            foreach (DataGridItem row in gvBillItem.Controls[0].Controls)
            {
                if (row.ItemType == ListItemType.Item || row.ItemType == ListItemType.AlternatingItem)
                {
                    atxtEquName = ((TextBox)row.FindControl("txtEquName")).Text; // �ʲ�����
                    if (atxtEquName.Trim() != string.Empty)
                    {
                        IsFillTypeName = true;
                    }
                }
            }

            if (!IsFillTypeName)
            {
                blnRet = false;
                PageTool.MsgBox(this, "�ʲ���Ϣ����Ϊ��!");
            }

            if (blnRet)
            {
                SaveDetailItem();
            }

            return blnRet;
        }

        #endregion

        #region ȡ���ܽ�� GetDetailTotalAmount
        /// <summary>
        /// ȡ���ܽ��
        /// </summary>
        /// <returns></returns>
        private double GetDetailTotalAmount()
        {
            double dTotal = 0;

            foreach (DataGridItem row in gvBillItem.Items)
            {
                string stxtSubTotal = ((TextBox)row.FindControl("txtSubTotal")).Text;
                dTotal += stxtSubTotal == "" ? 0.0 : double.Parse(stxtSubTotal);
            }
            return dTotal;
        }
        #endregion

        #region ȡ����ϸ����

        /// <summary>
        ///  ȡ����ϸ����
        /// </summary>
        /// <returns></returns>
        private DataTable GetDetailItem()
        {
            string equId = "";
            return GetDetailItem(false, 0, ref equId);
        }

        /// <summary>
        /// ��ȡ��ϸ����
        /// </summary>
        /// <param name="isAll"></param>
        /// <returns></returns>
        private DataTable GetDetailItem(bool isAll, int indexs, ref string strHidAddValue)
        {

            #region �������

            Equ_DeskDP equDesk = new Equ_DeskDP();
            DataTable dt = new DataTable();
            int iCostID = 0;
            DataRow dr;//������

            #endregion

            #region ������

            dt.Columns.Add("DETAILSID", Type.GetType("System.Decimal"));
            dt.Columns.Add("EQUID", Type.GetType("System.Decimal"));
            dt.Columns.Add("EQUNAME", Type.GetType("System.String"));
            dt.Columns.Add("LISTID", Type.GetType("System.Decimal"));
            dt.Columns.Add("LISTNAME", Type.GetType("System.String"));
            dt.Columns.Add("EQUCODE", Type.GetType("System.String"));
            dt.Columns.Add("CHANGECONTENT", Type.GetType("System.String"));
            dt.Columns.Add("OLDVALUE", Type.GetType("System.String"));
            dt.Columns.Add("NEWVALUE", Type.GetType("System.String"));
            dt.Columns.Add("CHANGEDATE", Type.GetType("System.String"));
            dt.Columns.Add("CHANGEUSERID", Type.GetType("System.Decimal"));
            dt.Columns.Add("CHANGEUSERNAME", Type.GetType("System.String"));
            dt.Columns.Add("CHANGEDEPTID", Type.GetType("System.Decimal"));
            dt.Columns.Add("CHANGEDEPTNAME", Type.GetType("System.String"));
            dt.Columns.Add("CHANGESTATUS", Type.GetType("System.Decimal"));
            dt.Columns.Add("Remark", Type.GetType("System.String"));
            dt.Columns.Add("DEPT", Type.GetType("System.String"));

            #endregion

            #region ����DataTable

            foreach (DataGridItem row in gvBillItem.Controls[0].Controls)
            {
                iCostID++;
                if (row.ItemType == ListItemType.Footer)
                {
                    string aEquID = ((HtmlInputHidden)row.FindControl("hidAddID")).Value; //�ʲ�ID
                    string aEquName = ((HtmlInputHidden)row.FindControl("hidAddEquName")).Value; //����
                    string aCode = ((HtmlInputHidden)row.FindControl("hidAddCode")).Value; // ���
                    string aListID = ((HtmlInputHidden)row.FindControl("hidAddListID")).Value; //Ŀ¼ID
                    string aListName = ((HtmlInputHidden)row.FindControl("hidAddListName")).Value; ;//Ŀ¼����
                    string aChangeContent = ((CtrFlowFormText)row.FindControl("CtrAddChangeContent")).Value;//�������
                    string aChangeOld = ((CtrFlowFormText)row.FindControl("CtrAddOldValue")).Value; //���ԭֵ
                    string aChangeNew = ((CtrFlowFormText)row.FindControl("CtrAddNewValue")).Value;   //������ֵ
                    string aRemark = ((CtrFlowFormText)row.FindControl("CtrAddNewValue")).Value;  //��ע

                    aEquID = aEquID == "" ? "0" : aEquID;
                    aListID = aListID == "" ? "0" : aListID;
                    if (row.ItemIndex == indexs)
                    {
                        strHidAddValue = aEquID;
                    }
                    if (aEquID != "0")
                    {
                        equDesk = equDesk.GetReCorded(long.Parse(aEquID));
                        dr = dt.NewRow();
                        dr["DETAILSID"] = 0;
                        dr["EQUID"] = decimal.Parse(aEquID);
                        dr["EQUNAME"] = aEquName;
                        dr["LISTID"] = decimal.Parse(aListID);
                        dr["LISTNAME"] = aListName;
                        dr["EQUCODE"] = aCode;
                        dr["CHANGECONTENT"] = aChangeContent;
                        dr["OLDVALUE"] = aChangeOld;
                        dr["NEWVALUE"] = aChangeNew;
                        dr["CHANGEDATE"] = "";
                        dr["CHANGEUSERID"] = 0;
                        dr["CHANGEUSERNAME"] = "";
                        dr["CHANGEDEPTID"] = 0;
                        dr["CHANGEDEPTNAME"] = "";
                        dr["CHANGESTATUS"] = 0;
                        dr["Remark"] = aRemark;
                        dr["DEPT"] = equDesk.partBranchName;
                        dt.Rows.Add(dr);
                    }
                    else
                    {
                        if (!isAll)
                            PageTool.MsgBox(this, "�ʲ���Ϣ����Ϊ�գ�");
                    }
                }
                else if (row.ItemType == ListItemType.Item || row.ItemType == ListItemType.AlternatingItem)
                {
                    string EquID = ((HtmlInputHidden)row.FindControl("hidID")).Value; //��ƷID
                    string EquName = ((HtmlInputHidden)row.FindControl("hidEquName")).Value; //����
                    string Code = ((HtmlInputHidden)row.FindControl("hidCode")).Value; // ���
                    string TypeID = ((HtmlInputHidden)row.FindControl("hidListID")).Value; //���ID
                    string TypeName = ((HtmlInputHidden)row.FindControl("hidListName")).Value;// ((EquPicker)row.FindControl("epList")).EpuName;  //�������
                    string ChangeContent = ((CtrFlowFormText)row.FindControl("CtrChangeContent")).Value;//�������
                    string ChangeOld = ((CtrFlowFormText)row.FindControl("CtrOldValue")).Value; //���ԭֵ
                    string ChangeNew = ((CtrFlowFormText)row.FindControl("CtrNewValue")).Value;   //������ֵ
                    string Remark = ((CtrFlowFormText)row.FindControl("CtrNewValue")).Value;  //��ע


                    EquID = EquID == "" ? "0" : EquID;
                    TypeID = TypeID == "" ? "0" : TypeID;
                    if (row.ItemIndex == indexs)
                    {
                        strHidAddValue = EquID;
                    }
                    if (EquID != "0")
                    {
                        equDesk = equDesk.GetReCorded(long.Parse(EquID));
                        dr = dt.NewRow();
                        dr["DETAILSID"] = 0;
                        dr["EQUID"] = decimal.Parse(EquID);
                        dr["EQUNAME"] = EquName;
                        dr["LISTID"] = decimal.Parse(TypeID);
                        dr["LISTNAME"] = TypeName;
                        dr["EQUCODE"] = Code;
                        dr["CHANGECONTENT"] = ChangeContent;
                        dr["OLDVALUE"] = ChangeOld;
                        dr["NEWVALUE"] = ChangeNew;
                        dr["CHANGEDATE"] = "";
                        dr["CHANGEUSERID"] = 0;
                        dr["CHANGEUSERNAME"] = "";
                        dr["CHANGEDEPTID"] = 0;
                        dr["CHANGEDEPTNAME"] = "";
                        dr["CHANGESTATUS"] = 0;
                        dr["Remark"] = Remark;
                        dr["DEPT"] = equDesk.partBranchName;
                        dt.Rows.Add(dr);
                    }
                }
            }

            #endregion

            return dt;//��������
        }
        #endregion

        #region  �󶨱���ʲ���ϸ BindGrid
        /// <summary>
        /// �󶨱���ʲ���ϸ
        /// </summary>
        /// <param name="id"></param>
        private void BindGrid(long id)
        {
            #region ��ȡ����

            DataTable dtItem = ChangeDealDP.GetCLFareItem(id);

            dtItem = CheckUnSaveDataAndLoadIt(dtItem);

            gvBillItem.DataSource = dtItem;
            gvBillItem.DataBind();
            gvBillItem.Visible = true;

            #endregion
        }
        #endregion

        #region �����ʲ�Ϊֻ�� SetFareDetailReadOnly
        /// <summary>
        /// �����ʲ�Ϊֻ��
        /// </summary>
        private void SetFareDetailReadOnly()
        {
            foreach (DataGridItem row in gvBillItem.Items)
            {
                ((HtmlInputButton)row.FindControl("cmdEqu")).Visible = false;
                ((TextBox)row.FindControl("txtEquName")).Visible = false;
                ((TextBox)row.FindControl("txtEquName")).Attributes.Remove("onmousemove");
                ((Label)row.FindControl("lblEquName")).Visible = true;
                ((HtmlInputButton)row.FindControl("cmdListName")).Visible = false;
                ((TextBox)row.FindControl("txtListName")).Visible = false;
                ((Label)row.FindControl("lblListName")).Visible = true;
                ((CtrFlowFormText)row.FindControl("CtrChangeContent")).ContralState = eOA_FlowControlState.eReadOnly;
                ((CtrFlowFormText)row.FindControl("CtrOldValue")).ContralState = eOA_FlowControlState.eReadOnly;
                ((CtrFlowFormText)row.FindControl("CtrNewValue")).ContralState = eOA_FlowControlState.eReadOnly;

                gvBillItem.Columns[gvBillItem.Columns.Count - 2].Visible = false;
            }
            gvBillItem.ShowFooter = false;
        }

        private void SetFareDetailvisible(string text, bool flag)
        {
            foreach (DataGridItem row in gvBillItem.Items)
            {
                switch (text.ToLower())
                {
                    case "desktype":
                        ((HtmlInputButton)row.FindControl("cmdListName")).Visible = false;
                        ((TextBox)row.FindControl("txtListName")).Visible = false;
                        ((Label)row.FindControl("lblListName")).Visible = true;
                        if (flag == false)
                        {
                            ((Label)row.FindControl("lblListName")).Visible = false;
                        }
                        break;
                    case "equipmentname":

                        hidEquIsHid.Value = "1";            //�����ʲ�Ϊֻ��
                        SetFareDetailReadOnly();

                        if (flag == false)
                        {
                            hidEquIsHid.Value = "2";        //�����ʲ�Ϊ���ɼ�
                            //�����ʲ�datagrid������
                            gvBillItem.Visible = false;
                        }
                        break;
                    case "oldvalue":
                        ((CtrFlowFormText)row.FindControl("CtrOldValue")).ContralState = eOA_FlowControlState.eReadOnly;
                        if (flag == false)
                        {
                            ((CtrFlowFormText)row.FindControl("CtrOldValue")).ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    case "newvalue":
                        ((CtrFlowFormText)row.FindControl("CtrNewValue")).ContralState = eOA_FlowControlState.eReadOnly;
                        if (flag == false)
                        {
                            ((CtrFlowFormText)row.FindControl("CtrNewValue")).ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    case "impactanalysis":
                        gvBillItem.Columns[gvBillItem.Columns.Count - 3].Visible = false;
                        break;
                    case "adddetails":
                        gvBillItem.Columns[gvBillItem.Columns.Count - 2].Visible = false;
                        gvBillItem.ShowFooter = false;
                        break;
                    default:
                        break;
                }
            }

            if (text == "equchange")
            {
                gvBillItem.Columns[gvBillItem.Columns.Count - 1].Visible = false;
                hidEquEdit.Value = "0";
            }
        }
        #endregion

        #region ��ϸ������ɾ���¼� gvBillItem_ItemCommand

        /// <summary>
        /// ������ϸ������ɾ���¼�
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void gvBillItem_ItemCommand(object source, DataGridCommandEventArgs e)
        {

            DataTable dt = new DataTable();
            if (e.CommandName == "Delete")
            {
                string hidId = "";
                dt = GetDetailItem(true, e.Item.ItemIndex, ref hidId);
                Equ_DeskDP.DeleteEquChange(long.Parse(FlowID), long.Parse(hidId), long.Parse(ChangeID.ToString()));
                dt.Rows.RemoveAt(e.Item.ItemIndex);

                /*     
                 * Date: 2013-07-04 10:22
                 * summary: �Ự���� Session["frm_changebase_aspx_cs_gvBillItem_unsaved_data"] ��˵����
                 * CheckUnSaveDataAndLoadIt ����.
                 * 
                 * modified: sunshaozong@gmail.com     
                 */

                Session["frm_changebase_aspx_cs_gvBillItem_unsaved_data"] = dt;

                gvBillItem.DataSource = dt.DefaultView;
                gvBillItem.DataBind();

            }
            else if (e.CommandName == "Add")
            {
                string hidId = "";
                dt = GetDetailItem(false, e.Item.ItemIndex, ref hidId);

                Session["frm_changebase_aspx_cs_gvBillItem_unsaved_data"] = dt;

                gvBillItem.DataSource = dt.DefaultView;
                gvBillItem.DataBind();
            }

        }
        #endregion

        protected void gvBillItem_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer || e.Item.ItemType == ListItemType.Footer)
            {
                HiddenField hiddenfield = (HiddenField)e.Item.FindControl("hidClientId_ForOpenerPage");
                HtmlInputButton btn = (HtmlInputButton)e.Item.FindControl("cmdAddEqu");
                string url = "../EquipmentManager/frmEqu_DeskMain.aspx?IsSelect=true&EquipmentCatalogID=0&Opener_ClientId=" + hiddenfield.ClientID + "&TypeFrm=frm_ChangeBase";
                // e.Item.Attributes.Add("onclick", "window.open('" + url + "', '',  'resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=700,height=850px,left=150,top=50');");
                btn.Attributes.Add("onclick", "window.open('" + url + "', '',  'scrollbars=yes,resizable=yes,width=700px,height=600px,left=150px,top=50px');");

            }
        }
        #region GetUrl
        /// <summary>
        /// GetUrl
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        public string GetUrlEquId(decimal equID)
        {
            //��ʱû�����ҳ

            string sUrl = "";
            Random ro = new Random();
            string random = ro.Next().ToString();
            sUrl = "javascript:window.open('frmEqu_ImpactAnalysis.aspx?EquId=" + equID + "&ChangeBillFlowID=" + FlowID + "&randomid=" + random + " ','','scrollbars=yes,status=yes ,resizable=yes,width=800,height=600'); ";
            //  sUrl = "javascript:window.open('../Forms/frmIssueView.aspx?NewWin=true&FlowID=" + lngFlowID.ToString() + "','','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
            return sUrl;
        }
        #endregion


        #region ���δ������ʲ���Ϣ�����ص� gvBillItem ��. - 2013-07-04 @������
        /// <summary>
        /// ���δ������ʲ���Ϣ�����У��ͼ��ص� gvBillItem �С�
        /// </summary>
        /// <param name="dt">�ʲ���Ϣ��</param>
        /// <returns></returns>
        private DataTable CheckUnSaveDataAndLoadIt(DataTable dt)
        {
            /*     
             * Date: 2013-07-04 09:55
             * summary: �����δ������ʲ���Ϣ�����У��ͼ��ص� gvBillItem �С�
             * 
             * ����˵��,
             * from_equ_deskedit: ��ʾ�Ǵ� frmEqu_DeskEdit.aspx.cs �ļ��е� Master_Master_Button_GoHistory_Click ������ת�����ġ�
             * from_equ_deskedit �Ϸ�ֵ��yes
             * 
             * Session["frm_changebase_aspx_cs_gvBillItem_unsaved_data"]��δ��������ʲ���Ϣ
             * Session["frm_changebase_aspx_cs_gvBillItem_unsaved_data"] �Ϸ�ֵ��DataTable ����, ���ṹ�� gvBillItem �ؼ��а󶨵�����Դ��ṹһ�¡�
             * 
             * Ŀ��: �޸��ڱ���� > �ʲ���Ϣ �������ʲ�, Ȼ������������ť��ת���ʲ����ϱ༭ҳ�棬�ٵ㷵�ذ�ť��ȥʱ��ԭ����ѡ���ʲ�����
             * �˵����⡣
             * 
             * modified: sunshaozong@gmail.com     
             */

            String strFromEquDeskEdit = Request.QueryString["from_equ_deskedit"];
            if (strFromEquDeskEdit == null)
            {
                Session["frm_changebase_aspx_cs_gvBillItem_unsaved_data"] = null;
                return dt;
            }

            strFromEquDeskEdit = strFromEquDeskEdit.Trim().ToLower();

            if (!strFromEquDeskEdit.Equals("yes")) return dt;

            DataTable dtUnsavedEquData = Session["frm_changebase_aspx_cs_gvBillItem_unsaved_data"] as DataTable;
            if (dtUnsavedEquData == null || dtUnsavedEquData.Rows.Count <= 0) return dt;

            Boolean isHave = dtUnsavedEquData.Rows.Count > dt.Rows.Count;

            if (!isHave) return dt;

            Equ_DeskDP ee = new Equ_DeskDP();

            for (int intStartIdx = dt.Rows.Count; intStartIdx < dtUnsavedEquData.Rows.Count; intStartIdx++)
            {
                DataRow row = dtUnsavedEquData.Rows[intStartIdx];

                DataTable dtEqu = ee.GetDataTable(String.Format(" and id = {0} ", row["EQUID"]), "");
                if (dtEqu.Rows.Count <= 0)
                {
                    continue;    // �����ʲ������ݿ����ѱ�ɾ��, ������.
                }

                DataRow drUpdated = dtEqu.Rows[0];
                Object[] arrRow = null;

                if (dt.Columns.Contains("CHANGESTATUS"))
                {
                    arrRow = WrapChangeRow(row, drUpdated);
                }
                else
                {
                    arrRow = WrapProblemOrIssueRow(row, drUpdated);
                }


                dt.Rows.Add(arrRow);
            }

            return dt;
        }


        private Object[] WrapProblemOrIssueRow(DataRow row, DataRow rowUpdated)
        {
            List<Object> listRow = new List<object>();

            listRow.Add(row["LISTID"]);
            listRow.Add(row["LISTNAME"]);
            listRow.Add(row["EQUID"]);
            listRow.Add(rowUpdated["name"]);
            listRow.Add(rowUpdated["PARTBRANCHNAME"]);
            listRow.Add(rowUpdated["code"]);
            listRow.Add(row["CHANGECONTENT"]);
            listRow.Add(row["OLDVALUE"]);
            listRow.Add(row["NEWVALUE"]);
            listRow.Add(row["Remark"]);

            return listRow.ToArray();
        }

        private Object[] WrapChangeRow(DataRow row, DataRow rowUpdated)
        {
            List<Object> listRow = new List<object>();

            listRow.Add(row["DETAILSID"]);
            listRow.Add(row["EQUID"]);
            listRow.Add(rowUpdated["name"]);
            listRow.Add(rowUpdated["code"]);
            listRow.Add(row["CHANGECONTENT"]);
            listRow.Add(row["OLDVALUE"]);
            listRow.Add(row["NEWVALUE"]);
            listRow.Add(row["CHANGEDATE"]);
            listRow.Add(row["CHANGEUSERID"]);
            listRow.Add(row["CHANGEUSERNAME"]);

            listRow.Add(row["CHANGEDEPTID"]);
            listRow.Add(row["CHANGEDEPTNAME"]);
            listRow.Add(row["CHANGESTATUS"]);
            listRow.Add(-1);
            listRow.Add(row["Remark"]);
            listRow.Add(row["LISTID"]);
            listRow.Add(row["LISTNAME"]);


            listRow.Add(DateTime.Now);
            listRow.Add(DateTime.Now);
            listRow.Add(rowUpdated["PARTBRANCHNAME"]);

            return listRow.ToArray();
        }
        #endregion
        private void GetChangePlace()
        {
            DataTable dt = CatalogDP.GetNextCatalogs(1058);
            if (dt == null)
            {
                return;
            }
            this.chkChangePlace.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ListItem item = new ListItem();
                item.Text = dt.Rows[i]["CataLogName"].ToString();
                item.Value = dt.Rows[i]["catalogID"].ToString();
                this.chkChangePlace.Items.Add(item);
            }
        }
        /// <summary> 
        /// �ж����������Ƿ���ͬһ�� 
        /// </summary> 
        /// <param name="dtmS">��ʼ����</param> 
        /// <param name="dtmE">��������</param>
        /// <returns></returns> 
        public static bool IsInSameWeek(DateTime dtmS, DateTime dtmE)
        {
            //��ȡ����ʱ���
            TimeSpan ts = dtmE - dtmS;
            //��ȡ����ʱ����������
            double dbl = ts.TotalDays;
            //��ȡ����Ľ���ʱ������ںŲ�ת������һ����Ϊ1-6��0Ϊ������
            int intDow = Convert.ToInt32(dtmE.DayOfWeek);
            if (intDow == 0) intDow = 7;
            //���������������6�������������ڻ���ں����ʱ��ת�ɵ����־Ͳ�����ͬһ����
            if (dbl >= 7 || dbl >= intDow) return false;
            else return true;
        }
    }
}
