/****************************************************************************
 * 
 * description:�������Webҳ�洦��
 * 
 * 
 * 
 * Create by:zhumingchun
 * Create Date:2007-07-02
 * *************************************************************************/
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Drawing;
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
using Epower.ITSM.SqlDAL.ES_TBLCS;
using System.Collections.Generic;

namespace Epower.ITSM.Web.ProbleForms
{
    /// <summary>
    /// 
    /// </summary>
    public partial class frmProblemSolve : BasePage
    {
        /// <summary>
        /// 
        /// </summary>
        private FlowForms myFlowForms;

        decimal dScale = 0;
        decimal dEffect = 0;
        decimal dStress = 0;

        int iRefCount = 0;

        long lngFromFlowID = 0; //�鵵��Դ������ʵ��ID

        /// <summary>
        /// ȡ������FlowID
        /// </summary>
        /// <summary>
        /// ȡ������FlowID
        /// </summary>
        public string FlowID
        {
            get
            {
                if (ViewState["frm_Issue_Base_FlowID"] != null)
                {
                    return ViewState["frm_Issue_Base_FlowID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["frm_Issue_Base_FlowID"] = value;
            }
        }

        /// <summary>
        /// Ӧ��id
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
        /// <summary>
        /// ����ģ��id
        /// </summary>
        public string FlowModelID
        {
            get
            {
                if (ViewState["frm_Issue_Base_FlowModelID"] != null)
                {
                    return ViewState["frm_Issue_Base_FlowModelID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["frm_Issue_Base_FlowModelID"] = value;
            }
        }


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



        /// <summary>
        /// ��ر����ID
        /// </summary>
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
            set
            {
                ViewState["ChangeFlowID"] = value;
            }
        }
        /// <summary>
        /// ��غϲ����ⵥ
        /// </summary>
        public string ProblemSub
        {
            get
            {
                if (ViewState["ProblemSub"] != null)
                {
                    return ViewState["ProblemSub"].ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                ViewState["ProblemSub"] = value;
            }
        }
        /// <summary>
        /// �ϲ�������
        /// </summary>
        public string ProblemSubRel
        {
            get
            {
                if (ViewState["ProblemSubRel"] != null)
                {
                    return ViewState["ProblemSubRel"].ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                ViewState["ProblemSubRel"] = value;
            }
        }

        #region GRID URL
        /// <summary>
        /// 
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            myFlowForms = (FlowForms)this.Master;
            myFlowForms.mySetContentReadOnly += new FlowForms.DoContentActions(Master_mySetContentVisible);
            myFlowForms.myGetFormsValue += new FlowForms.GetFormsValue(Master_myGetFormsValue);
            myFlowForms.mySetFormsValue += new FlowForms.DoContentActions(Master_mySetFormsValue);
            myFlowForms.myPreClickCustomize += new FlowForms.DoContentSubmitValid(Master_myPreClickCustomize);
            myFlowForms.myPreSaveClickCustomize += new FlowForms.DoContentValid(Master_myPreSaveClickCustomize);
            myFlowForms.blnSMSNotify = true;   //���ζ���֪ͨ
            myFlowForms.blnEmail = true;       //�����ʼ�֪ͨ

            if (Page.IsPostBack == true)
            {
                lngFromFlowID = long.Parse(ViewState["frm_Problem_FromFlowID"].ToString());
            }
            else
            {
                PageDeal.SetLanguage(this.Controls[0].Controls[1]);
            }
            if (this.hidAppID.Value != "0")
            {
                //Extension_DayCtrList1.EquCategoryID = long.Parse(this.hidAppID.Value.ToString());
                EpowerCom.objFlow oFlow2 = new objFlow(long.Parse(Session["UserID"].ToString()), long.Parse(FlowModelID), long.Parse(MessageID));
                Extension_DayCtrList1.NodeModelID = oFlow2.NodeModelID;

                Extension_DayCtrList1.EquID = long.Parse(this.hidFlowID.Value.ToString());
                Extension_DayCtrList1.EquCategoryID = oFlow2.FlowModelID;

                
            }
        }

        #region �ύǰִ���¼� Master_myPreSaveClickCustomize
        /// <summary>
        /// �ύǰִ���¼�
        /// </summary>
        /// <returns></returns>
        bool Master_myPreSaveClickCustomize()
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


            //if (freeTextBox2.Text.Length > 1000)
            //{

            //    String strDealContentDisplayName = PageDeal.GetLanguageValue("PRO_DealContent");
            //    PageTool.MsgBox(this.Page, String.Format("��{0}�����볤�Ȳ��ܳ���1000���֣�", strDealContentDisplayName));
            //    breturn = false;
            //}

            return breturn;
        }
        #endregion

        #region �ݴ�ʱǰ��ִ���¼� Master_myPreClickCustomize
        /// <summary>
        /// �ݴ�ʱǰ��ִ���¼�
        /// </summary>
        /// <returns></returns>
        bool Master_myPreClickCustomize(long lngActionID, string strActionName)
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
            
            //if (freeTextBox2..Length > 1000)
            //{

            //    String strDealContentDisplayName = PageDeal.GetLanguageValue("PRO_DealContent");
            //    PageTool.MsgBox(this.Page, String.Format("��{0}�����볤�Ȳ��ܳ���1000���֣�", strDealContentDisplayName));
            //    breturn = false;
            //}

            return breturn;
        }
        #endregion
        //������ⵥ�����Ȩ��
        public string IssuesForProblem = string.Empty;


        private void BindRelItemData(long lngFlowID)
        {
            gridAttention.Columns[2].HeaderText = PageDeal.GetLanguageValue("CST_ServiceNO");
            gridAttention.Columns[3].HeaderText = PageDeal.GetLanguageValue("CST_ServiceType");

            DataTable dt = ProblemDealDP.GetIssuesForProblem(lngFlowID);
            if (dt.Rows.Count > 0)
            {
                //�������
                IssuesForProblem = "true";
            }
            gridAttention.DataSource = dt.DefaultView;
            gridAttention.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");

            gridAttention.DataBind();

            if (iRefCount > 0)
            {
                labScale.Text = "ƽ������Ȩ��:" + Math.Round(dScale / iRefCount, 2);
                labEffect.Text = "ƽ������Ӱ���:" + Math.Round(dEffect / iRefCount, 2);
                labStress.Text = "ƽ������������:" + Math.Round(dStress / iRefCount, 2);

            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Master_mySetFormsValue()
        {

            if (myFlowForms.oFlow.FlowStatus == e_FlowStatus.efsEnd)
            {
                myFlowForms.CtrButtons1.Button3Visible = true;
                myFlowForms.CtrButtons1.ButtonName3 = "֪ʶ�鵵";
                myFlowForms.CtrButtons1.Button3Function = "DoKmAdd(" + myFlowForms.oFlow.MessageID.ToString() + "," + myFlowForms.oFlow.AppID.ToString() + "," + myFlowForms.oFlow.FlowID.ToString() + ");";
            }

            myFlowForms.FormTitle = myFlowForms.oFlow.FlowName;
            #region Master_mySetFormsValue
            hidFlowID.Value = myFlowForms.oFlow.FlowID.ToString();
            this.MessageID = myFlowForms.oFlow.MessageID.ToString();
            this.hidAppID.Value = myFlowForms.oFlow.AppID.ToString();

            #region ��ӡʱ�������
            //��ӡʱ��Ҫ����Ĳ���
            this.FlowID = myFlowForms.oFlow.FlowID.ToString();
            this.FlowModelID = myFlowForms.oFlow.FlowModelID.ToString();
            this.AppID = myFlowForms.oFlow.AppID.ToString();
            #endregion

            ImplDataProcess dp = new ImplDataProcess(myFlowForms.oFlow.AppID);
            DataSet ds = dp.GetFieldsDataSet(myFlowForms.oFlow.FlowID, myFlowForms.oFlow.OpID);
            DataTable dt = ds.Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    CataProblemType.CatelogID = long.Parse(row["Problem_Type"].ToString());
                    CataProblemLevel.CatelogID = long.Parse(row["Problem_Level"].ToString());
                    CtrFCDEffect.CatelogID = long.Parse(row["EffectID"].ToString());
                    CtrFCDInstancy.CatelogID = long.Parse(row["InstancyID"].ToString());
                    txtProblem_Subject.Value = row["Problem_Subject"].ToString();
                    txtProblem_Title.Value = row["Problem_Title"].ToString();
                    labRemark.Text = row["Remark"].ToString();
                    LabDealContent.Text = row["DealContent"].ToString();
                    freeTextBox1.Text = row["Remark"].ToString();
                    freeTextBox2.Text = row["DealContent"].ToString();
                    labRegUserName.Text = row["RegUserName"].ToString();
                    labRegDeptName.Text = row["RegDeptName"].ToString();
                    labRegTime.Text = row["RegTime"].ToString();
                    //CtrFlowProblemState.CatelogID = long.Parse(row["State"].ToString());
                    if (row["State"].ToString() != string.Empty)
                        CtrDealState.CatelogID = long.Parse(row["State"].ToString());

                    //���ⵥ��
                    lblProblemNo.Text = row["ProblemNo"].ToString();
                    lblBuildCode.Text = row["BuildCode"].ToString();

                    #region �ʲ����
                    lblListName.Text = row["ListName"].ToString(); //�ʲ�Ŀ¼
                    txtListName.Text = row["ListName"].ToString();
                    hidListName.Value = row["ListName"].ToString();
                    hidListID.Value = row["ListID"].ToString() == "" ? "0" : row["ListID"].ToString();

                    lblEqu.Text = row["EquName"].ToString();  //�ʲ���Ϣ
                    txtEqu.Text = row["EquName"].ToString();
                    hidEquName.Value = row["EquName"].ToString();
                    hidEqu.Value = row["EquID"].ToString() == "" ? "0" : row["EquID"].ToString();


                    ChangeFlowID = row["AssociateFlowID"].ToString();

                    #endregion

                    #region �ϲ����ⵥ

                    this.dgProblemSub.Columns[1].HeaderText = PageDeal.GetLanguageValue("PRO_ProblemNo");
                    this.dgProblemSub.Columns[2].HeaderText = PageDeal.GetLanguageValue("PRO_ProblemTypeName");
                    this.dgProblemSub.Columns[3].HeaderText = PageDeal.GetLanguageValue("PRO_Title");
                    this.dgProblemSub.Columns[4].HeaderText = PageDeal.GetLanguageValue("PRO_StateName");

                    //�ϲ�����
                    DataTable dtPoblemSub = ProblemDealDP.GetProblemSub(myFlowForms.oFlow.FlowID);
                    dgProblemSub.DataSource = dtPoblemSub.DefaultView;
                    dgProblemSub.DataBind();
                    if (dtPoblemSub.Rows.Count > 0)
                        ProblemSub = "true";

                    this.dgProblemSub.Columns[1].HeaderText = PageDeal.GetLanguageValue("PRO_ProblemNo");
                    this.dgProblemSub.Columns[2].HeaderText = PageDeal.GetLanguageValue("PRO_ProblemTypeName");
                    this.dgProblemSub.Columns[3].HeaderText = PageDeal.GetLanguageValue("PRO_Title");
                    this.dgProblemSub.Columns[4].HeaderText = PageDeal.GetLanguageValue("PRO_StateName");

                    //�ϲ�������
                    DataTable dtPoblemSubRel = ProblemDealDP.GetProblemSubRel(myFlowForms.oFlow.FlowID);
                    dgProblemSubRel.DataSource = dtPoblemSubRel.DefaultView;
                    dgProblemSubRel.DataBind();
                    if (dtPoblemSubRel.Rows.Count > 0)
                        ProblemSubRel = "true";
                    #endregion

                }
                else
                {
                    labRegUserName.Text = Session["PersonName"].ToString();
                    labRegDeptName.Text = Session["UserDeptName"].ToString();
                    labRegTime.Text = DateTime.Now.ToString();

                    if (Session["ExtendParameter"] != null && Session["ExtendParameter"].ToString() != string.Empty && Session["ExtendParameter"].ToString() != "0")
                    {
                        string sFlowList = Session["ExtendParameter"].ToString();
                        if (!sFlowList.StartsWith("problemmerge"))   //�¼���������
                        {
                            //ͨ����ȡ
                            long lngFlowID = long.Parse(sFlowList);
                            //objFlow oldFlow = new objFlow((long)Session["UserID"], 0,lngOriMessageID);

                            //����Ƿ��Ѿ�ת��Ϊ���ⵥ
                            if (ProblemDealDP.CheckIsChangeProblem(lngFlowID))
                            {
                                Epower.DevBase.BaseTools.PageTool.MsgBox(this, "���¼�������Ϊ���⣬������������");
                                Epower.DevBase.BaseTools.PageTool.AddJavaScript(this, "window.close();");
                            }

                            ImplDataProcess problemdp = new ImplDataProcess(1026);
                            string strOldValues = problemdp.GetFieldValues(lngFlowID, 0);
                            if (strOldValues.Trim() != string.Empty)
                            {
                                FieldValues fv = new FieldValues(strOldValues);
                                txtProblem_Subject.Value = fv.GetFieldValue("ProblemContent").Value.Trim();
                                freeTextBox1.Text = fv.GetFieldValue("DealContent").Value.Trim();
                                freeTextBox2.Text = fv.GetFieldValue("DealContent").Value.Trim();
                                hidServiceTitle.Value = fv.GetFieldValue("ServiceTitle").Value.Trim();

                                //�ʲ�Ŀ¼
                                txtListName.Text = fv.GetFieldValue("EquipmentCatalogName").Value.Trim();
                                hidListName.Value = txtListName.Text;
                                hidListID.Value = fv.GetFieldValue("EquipmentCatalogID").Value.Trim();

                                //�ʲ�����
                                txtEqu.Text = fv.GetFieldValue("EquipmentName").Value.Trim();
                                hidEquName.Value = txtEqu.Text;
                                hidEqu.Value = fv.GetFieldValue("EquipmentID").Value.Trim();

                                // ժҪ������
                                txtProblem_Title.Value = fv.GetFieldValue("ServiceTitle").Value.Trim();
                                txtProblem_Subject.Value = fv.GetFieldValue("ProblemContent").Value.Trim();
                            }

                            lngFromFlowID = lngFlowID;

                            myFlowForms.TempFlowID = lngFlowID;
                        }
                        else   //�ϲ����ⵥ
                        {
                            string strFlowIDList = string.Empty;
                            if (sFlowList.Length > 12)
                            {
                                strFlowIDList = sFlowList.Substring("problemmerge".Length + 1, sFlowList.Length - "problemmerge".Length - 1);
                                DataTable dtPoblemSub = ProblemDealDP.GetProblemSubAdd(strFlowIDList);
                                dgProblemSub.DataSource = dtPoblemSub.DefaultView;
                                dgProblemSub.DataBind();
                                if (dtPoblemSub.Rows.Count > 0)
                                    ProblemSub = "true";

                                string strSubject = string.Empty;
                                foreach (DataRow dr in dtPoblemSub.Rows)
                                {
                                    strSubject += dr["Problem_Subject"] + ";";
                                }
                                txtProblem_Subject.Value = strSubject;
                            }
                        }

                    }
                }


                //�󶨹����¼�
                BindRelItemData(myFlowForms.oFlow.FlowID);

            }
            #endregion

            #region set visible
            int iShowDeal = 0;   //�Ƿ���ʾ������Ϣ
            setFieldCollection setFields = myFlowForms.oFlow.setFields;

            foreach (setField sf in setFields)
            {
                if (sf.Editable == true && sf.Visibled == true)
                {
                    continue;
                }
                switch (sf.Name.ToLower())
                {
                    case "problem_type":
                        CataProblemType.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CataProblemType.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    case "problem_level":
                        CataProblemLevel.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CataProblemLevel.ContralState = eOA_FlowControlState.eHidden;
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
                    case "problem_subject":
                        txtProblem_Subject.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            txtProblem_Subject.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    case "problem_title":
                        txtProblem_Title.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            txtProblem_Title.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    case "remark":
                        freeTextBox1.Visible = false;
                        if (sf.Visibled == true)
                        {
                            labRemark.Visible = true;
                        }
                        else
                        {
                            labRemark.Visible = false;
                            iShowDeal += 1;   //1
                        }
                        break;
                    case "dealcontent":
                        freeTextBox2.Visible = false;
                        if (sf.Visibled == true)
                        {
                            LabDealContent.Visible = true;
                            //LitCause.Visible = true;
                        }
                        else
                        {
                            LabDealContent.Visible = false;
                            //LitCause.Visible = false;
                            iShowDeal += 1;
                        }
                        break;
                    case "equid":
                        txtListName.Visible = false;
                        cmdListName.Visible = false;

                        txtEqu.Visible = false;
                        cmdEqu.Visible = false;

                        if (sf.Visibled == true)
                        {
                            lblListName.Visible = true;
                            lblEqu.Visible = true;
                        }

                        break;
                    case "state":
                        CtrDealState.ContralState = eOA_FlowControlState.eReadOnly;
                        if (!sf.Visibled)
                            CtrDealState.ContralState = eOA_FlowControlState.eHidden;
                        break;
                    case "pro_expr_property":    // ��չ���� - 2013-11-19 @������
                        Extension_DayCtrList1.ReadOnly = true;
                        if (!sf.Visibled)
                        {
                            Extension_DayCtrList1.Visible = false;
                        }
                        break;
                    default:
                        break;

                }
            }
            #endregion

            #region ��չ��

            //EquDeploy = ZHServiceDP.getEQU_deployList(long.Parse(this.hidFlowID.Value.ToString()));


            Extension_DayCtrList1.EquID = long.Parse(this.hidFlowID.Value.ToString());

            if (this.hidAppID.Value.ToString() != "0")
            {
                //Extension_DayCtrList1.EquCategoryID = long.Parse(this.hidAppID.Value.ToString());
                Extension_DayCtrList1.NodeModelID = this.FlowMaster.oFlow.NodeModelID;
                Extension_DayCtrList1.EquCategoryID = this.FlowMaster.oFlow.FlowModelID;


            }


            #endregion

            ViewState["frm_Problem_FromFlowID"] = lngFromFlowID;
        }

        /// <summary>
        /// 
        /// </summary>
        private void Master_mySetContentVisible()
        {
            #region Master_mySetContentVisible

            #region �������

            if (CataProblemType.ContralState != eOA_FlowControlState.eHidden)
                CataProblemType.ContralState = eOA_FlowControlState.eReadOnly;

            #endregion

            #region ���⼶��

            if (CataProblemLevel.ContralState != eOA_FlowControlState.eHidden)
                CataProblemLevel.ContralState = eOA_FlowControlState.eReadOnly;

            #endregion

            #region Ӱ���

            if (CtrFCDEffect.ContralState != eOA_FlowControlState.eHidden)
                CtrFCDEffect.ContralState = eOA_FlowControlState.eReadOnly;

            #endregion

            #region ������

            if (CtrFCDInstancy.ContralState != eOA_FlowControlState.eHidden)
                CtrFCDInstancy.ContralState = eOA_FlowControlState.eReadOnly;

            #endregion

            #region ��������

            if (txtProblem_Subject.ContralState != eOA_FlowControlState.eHidden)
                txtProblem_Subject.ContralState = eOA_FlowControlState.eReadOnly;

            #endregion

            #region ����

            if (txtProblem_Title.ContralState != eOA_FlowControlState.eHidden)
                txtProblem_Title.ContralState = eOA_FlowControlState.eReadOnly;

            #endregion

            #region �������

            if (freeTextBox1.Visible)
                labRemark.Visible = true;
            freeTextBox1.Visible = false;

            #endregion

            #region �ʲ�Ŀ¼

            txtListName.Visible = false;
            cmdListName.Visible = false;

            #endregion

            #region �ʲ���Ϣ

            //�ʲ���Ϣ
            txtEqu.Visible = false;
            cmdEqu.Visible = false;
            lblListName.Visible = true;
            lblEqu.Visible = true;

            #endregion

            #region ����״̬
            if (CtrDealState.ContralState != eOA_FlowControlState.eHidden)
                CtrDealState.ContralState = eOA_FlowControlState.eReadOnly;
            #endregion
            #region ԭ�����
            if (freeTextBox2.Visible)
            {
                LabDealContent.Visible = true;
            }
            freeTextBox2.Visible = false;

            #endregion
            #endregion

            Extension_DayCtrList1.ReadOnly = true;    // ��չ���� - 2013-11-19 @������

            if (this.hidAppID.Value != "0")
            {
                //Extension_DayCtrList1.EquCategoryID = long.Parse(this.hidAppID.Value.ToString());
                Extension_DayCtrList1.NodeModelID = this.FlowMaster.oFlow.NodeModelID;

                Extension_DayCtrList1.EquID = long.Parse(this.hidFlowID.Value.ToString());
                Extension_DayCtrList1.EquCategoryID = this.FlowMaster.oFlow.FlowModelID;

            }            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngActionID"></param>
        /// <param name="strActionName"></param>
        /// <returns></returns>
        private XmlDocument Master_myGetFormsValue(long lngActionID, string strActionName)
        {
            #region Master_myGetFormsValue
            FieldValues fv = new FieldValues();
            fv.Add("Problem_Type", CataProblemType.CatelogID.ToString());
            fv.Add("Problem_TypeName", CataProblemType.CatelogValue.Trim());
            fv.Add("Problem_Level", CataProblemLevel.CatelogID.ToString());
            fv.Add("Problem_LevelName", CataProblemLevel.CatelogValue.Trim());
            fv.Add("EffectID", CtrFCDEffect.CatelogID.ToString().Trim());
            fv.Add("EffectName", CtrFCDEffect.CatelogValue.Trim());
            fv.Add("InstancyID", CtrFCDInstancy.CatelogID.ToString().Trim());
            fv.Add("InstancyName", CtrFCDInstancy.CatelogValue.Trim());
            fv.Add("Problem_Subject", txtProblem_Subject.Value.Trim());
            fv.Add("State", CtrDealState.CatelogID.ToString());
            fv.Add("StateName", CtrDealState.CatelogValue.Trim());
            fv.Add("Problem_Title", txtProblem_Title.Value.Trim());
            fv.Add("Remark", freeTextBox1.Text.Trim());
            fv.Add("RegUserID", Session["UserID"].ToString());
            fv.Add("RegUserName", Session["PersonName"].ToString());
            fv.Add("RegDeptID", Session["UserDeptID"].ToString());
            fv.Add("RegDeptName", Session["UserDeptName"].ToString());
            fv.Add("RegOrgID", Session["UserOrgID"].ToString());
            fv.Add("RegTime", DateTime.Now.ToString());
            fv.Add("FromFlowID", lngFromFlowID.ToString());
            fv.Add("ServiceTitle", hidServiceTitle.Value.Trim());

            fv.Add("EmailNotify", myFlowForms.EmailValue.ToString());
            fv.Add("SMSNotify", myFlowForms.NotifyValue.ToString());
            fv.Add("DealContent", freeTextBox2.Text.Trim());
            #region ȡ�����ⵥǰ׺���������ⵥ��

            string sBuildCode = this.lblBuildCode.Text.Trim();
            string sProblemNo = this.lblProblemNo.Text.Trim();
            if (sProblemNo == string.Empty)
            {
                sProblemNo = RuleCodeDP.GetCodeBH2(10004, ref sBuildCode);
            }
            fv.Add("ProblemNo", sProblemNo);
            fv.Add("BuildCode", sBuildCode);
            #endregion


            #region �ʲ����
            fv.Add("EquID", hidEqu.Value.Trim() == "" ? "0" : hidEqu.Value.Trim());
            fv.Add("EquName", hidEquName.Value.Trim());
            fv.Add("ListID", hidListID.Value.Trim() == "" ? "0" : hidListID.Value.Trim());
            fv.Add("ListName", hidListName.Value.Trim());
            #endregion

            #region
            int iItemCount = 0;
            FieldValues fvitem = new FieldValues();
            foreach (DataGridItem item in dgProblemSub.Items)
            {
                if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
                {
                    CheckBox chkDel = (CheckBox)item.Cells[0].FindControl("chkDel");
                    string strChk = chkDel.Checked == true ? "1" : "0";
                    if (item.Cells[dgProblemSub.Columns.Count - 1].Text.Trim() == "2")
                    {
                        strChk = "2";
                    }
                    string sSubFlowID = item.Cells[dgProblemSub.Columns.Count - 3].Text;

                    fvitem.Add("strChk" + iItemCount, strChk);
                    fvitem.Add("sSubFlowID" + iItemCount, sSubFlowID);

                    iItemCount++;
                }
            }
            fv.Add("ItemCount", iItemCount.ToString());
            fv.Add("ItemXml", fvitem.GetXmlObject().InnerXml);
            #endregion

            string ExtensionDayList = SaveExtensionDayList();
            fv.Add("ExtensionDayList", ExtensionDayList);  //��չ��

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

        #region grid �ؼ�����¼�
        protected void gridAttention_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string sScale = e.Item.Cells[4].Text;
                string sEffect = e.Item.Cells[5].Text;
                string sStress = e.Item.Cells[6].Text;

                dScale += StringTool.String2Decimal(sScale);
                dEffect += StringTool.String2Decimal(sEffect);
                dStress += StringTool.String2Decimal(sStress);
                iRefCount++;

                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                string sFlowID = DataBinder.Eval(e.Item.DataItem, "FlowID").ToString();
                e.Item.Attributes.Add("ondblclick", "window.open('../Forms/frmIssueView.aspx?NewWin=true&FlowID=" + sFlowID.ToString() + "','','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");


            }
        }

        #endregion

        #region �����¼���
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridUndoMsg_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            e_FlowStatus fs;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //��������������Ԥ�ƴ���ʱ��δ����ģ������ʾ
                fs = (e_FlowStatus)int.Parse(e.Item.Cells[12].Text);

                if (int.Parse(e.Item.Cells[13].Text.Trim()) < 0 && fs != e_FlowStatus.efsEnd)
                {
                    for (int i = 0; i < e.Item.Cells.Count; i++)
                    {
                        e.Item.Cells[i].ForeColor = Color.Red;
                    }
                }

                if (e.Item.Cells[7].Text.Trim() == "20")
                {
                    e.Item.Cells[7].Text = "���ڴ���";
                }
                else if (e.Item.Cells[7].Text.Trim() == "30")
                {
                    e.Item.Cells[7].Text = "��������";
                }
                else if (e.Item.Cells[7].Text.Trim() == "40")
                {
                    e.Item.Cells[7].Text = "������ͣ";
                }

            }
        }
        #endregion

        #region ������ⵥ
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgProblem_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                e.Item.Attributes.Add("ondblclick", "window.open('../Forms/frmIssueView.aspx?NewWin=true&FlowID=" + DataBinder.Eval(e.Item.DataItem, "FlowID").ToString() + "','','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");

                string sFlowDealState = e.Item.Cells[dgProblemSub.Columns.Count - 1].Text.Trim();
                CheckBox chkDel = (CheckBox)e.Item.Cells[0].FindControl("chkDel");
                if (sFlowDealState == "0")  //������
                {
                    chkDel.Checked = false;
                    chkDel.Enabled = true;
                }
                else if (sFlowDealState == "1")
                {
                    chkDel.Checked = true;
                    chkDel.Enabled = true;
                }
                else
                {
                    chkDel.Checked = true;
                    chkDel.Enabled = false;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgProblemSubRel_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                e.Item.Attributes.Add("ondblclick", "window.open('../Forms/frmIssueView.aspx?NewWin=true&FlowID=" + DataBinder.Eval(e.Item.DataItem, "FlowID").ToString() + "','','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
            }
        }
        #endregion
    }
}
