/****************************************************************************
 * 
 * description:���������ҳ��
 * 
 * 
 * 
 * Create by:zhumingchun
 * Create Date:2007-06-23
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
using Epower.ITSM.SqlDAL;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;
using Epower.DevBase.BaseTools;
using System.Xml;

namespace Epower.ITSM.Web.ProbleForms
{
    /// <summary>
    /// frmProblemMain
    /// </summary>
    public partial class frmProblemMain : BasePage
    {
        #region �������� - 2013-04-01 @������

        /// <summary>
        /// �߼���ѯ
        /// </summary>
        private const string __ADVANCED_SEARCH = "0";

        #endregion
        #region ��������

        RightEntity re = null;

        #region TypeID
        /// <summary>
        /// 
        /// </summary>
        protected string TypeID
        {
            get
            {
                if (ViewState["TypeID"] != null)
                    return ViewState["TypeID"].ToString();
                else
                    return "0";
            }
            set
            {
                ViewState["TypeID"] = value;
                hfTypeId.Value = value;
            }
        }
        #endregion

        #region FromBackUrl

        public string FromBackUrl
        {
            get
            {
                if (ViewState["FromBackUrl"] != null)
                    return ViewState["FromBackUrl"].ToString();
                else
                    return "";
            }
            set
            {
                ViewState["FromBackUrl"] = value;
            }
        }

        #endregion

        #region  SetParentButtonEvent
        /// <summary>
        /// ���ø����尴ť�¼�
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.Master_Button_ExportExcel_Click += new Global_BtnClick(Master_Master_Button_ExportExcel_Click);
            this.Master.ShowQueryButton(true);
            this.Master.ShowNewButton(false);
            this.Master.ShowExportExcelButton(true);
            this.Master.TxtKeyName.Visible = true;

            RightEntity re = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.QuestionTrace];
            if (!re.CanAdd)
            {
                this.Master.ShowNewButton(false);
            }

        }
        #endregion

        #region Excel����
        /// <summary>
        /// Excel����
        /// </summary>
        void Master_Master_Button_ExportExcel_Click()
        {
            string[] key = null;
            string[] value = null;
            string[] arrField = { "ProblemNo", "EquName", "Problem_TypeName", "Problem_LevelName", "EffectName", "InstancyName", "Problem_Title", "Problem_Subject", "StateName", "Remark", "RegUserName" };
            string[] fileName = { "���ⵥ��", "�ʲ�����", "�������", "���⼶��", "Ӱ���", "������", "ժҪ", "��������", "����״̬", "�������", "�ǵ���" };
            string s = "���ⵥ��";
            for (int i = 1; i < arrField.Length; i++)
            {
                s += "," + PageDeal.GetLanguageValue1(arrField[i], "���ⵥ");
            }
            fileName = s.Split(',');
            if (!string.IsNullOrEmpty(this.HiddenColumn.Value))
            {
                string[] columnValues = this.HiddenColumn.Value.Split(',');
                if (columnValues.Length > 1)
                {
                    string k = "ProblemNo";
                    for (int i = 0; i < columnValues.Length - 1; i++)
                    {
                        k += "," + columnValues[i];
                    }
                    key = k.Split(',');
                    string v = "���ⵥ��";
                    for (int i = 1; i < key.Length; i++)
                    {
                        v += "," + PageDeal.GetLanguageValue1(key[i], "���ⵥ");
                    }
                    value = v.Split(',');
                }
            }
            long lngUserID = (long)Session["UserID"];
            long lngDeptID = (long)Session["UserDeptID"];
            long lngOrgID = (long)Session["UserOrgID"];
            //���Ȩ��
            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.QuestionTrace];

            switch (TypeID)
            {
                case "0":
                    //�߼���ѯ
                    if (!string.IsNullOrEmpty(this.HiddenColumn.Value))
                    {
                        ProblemHighExcel(value, key);
                    }
                    else
                    {
                        ProblemHighExcel(fileName, arrField);
                    }
                    break;
                case "1":
                    //��������
                    ProblemFastExcel(fileName, arrField);
                    break;
                case "10":
                    //Ĭ�Ͻ����ʱ�򡣲�ѯ�����ڴ���ģ�����ʱ��Ϊ��һ���µ�Ȩ�޷�Χ�ڵļ�¼
                    ProblemdefaultExcel(fileName, arrField);
                    break;
                default:
                    ProblemdefaultExcel(fileName, arrField);
                    break;
            }
        }

        #endregion

        #region �߼���ѯ����excel
        /// <summary>
        /// �߼���ѯ����excel
        /// </summary>
        private void ProblemHighExcel()
        {
            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.QuestionTrace];
            int iRowCount = 0;
            //�߼���ѯ 
            XmlDocument xmlDoc = SqlWhereShow(hidSQLName.Value == "==ѡ���ղز�ѯ����==" ? "Temp1" : hidSQLName.Value);
            DataTable dt = ProblemDealDP.GetProblemsForCond(xmlDoc.InnerXml, long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()),
                    long.Parse(Session["UserOrgID"].ToString()), reTrace);
            Epower.ITSM.Web.Common.ExcelExport.ExportProblemList(this, dt, Session["UserID"].ToString());
        }
        /// <summary>
        /// �߼���ѯ����excel
        /// </summary>
        private void ProblemHighExcel(string[] fileName, string[] arrFlide)
        {
            string strWhere = " where 1=1 ";
            string strOrder = " ORDER BY SMSID DESC ";
            long lngUserID = (long)Session["UserID"];
            long lngDeptID = (long)Session["UserDeptID"];
            long lngOrgID = (long)Session["UserOrgID"];
            int intRowCount = -1;

            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.QuestionTrace];
            int iRowCount = 0;
            //�߼���ѯ 
            //XmlDocument xmlDoc = SqlWhereShow(hidSQLName.Value == "==ѡ���ղز�ѯ����==" ? "Temp1" : hidSQLName.Value);
            //DataTable dt = ProblemDealDP.GetProblemsForCond(xmlDoc.InnerXml, long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()),
            //        long.Parse(Session["UserOrgID"].ToString()), reTrace);

            //��ö�̬��ѯ����

            string strCondition = ctrCondition.strCondition;
            if (!string.IsNullOrEmpty(strCondition))
            {
                strWhere += " and " + strCondition;
            }

            //DataTable dt = DevRequestDP.GetDataTableExcel(strWhere, strOrder, lngUserID, lngDeptID, lngOrgID, reTrace);


            DataTable dt = ProblemDealDP.GetProblemsWithOutPage(strWhere,
                                                                    lngUserID,
                                                                    lngDeptID,
                                                                    lngOrgID,
                                                                    reTrace);


            Epower.ITSM.Web.Common.ExcelExport.ExportProblemList(this, dt, fileName, arrFlide, Session["UserID"].ToString());
        }
        #endregion

        #region ������������excel
        /// <summary>
        /// ������������excel
        /// </summary>
        private void ProblemFastExcel()
        {
            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.QuestionTrace];
            int iRowCount = 0;

            XmlDocument xmlDoc = GetXmlValue();
            DataTable dt = ProblemDealDP.GetProblemsForCond(xmlDoc.InnerXml, long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()),
                    long.Parse(Session["UserOrgID"].ToString()), reTrace);
            Epower.ITSM.Web.Common.ExcelExport.ExportProblemList(this, dt, Session["UserID"].ToString());
        }
        /// <summary>
        /// ������������excel
        /// </summary>
        private void ProblemFastExcel(string[] fileName, string[] arrFlide)
        {
            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.QuestionTrace];
            int iRowCount = 0;

            XmlDocument xmlDoc = GetXmlValue();
            DataTable dt = ProblemDealDP.GetProblemsForCond(xmlDoc.InnerXml, long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()),
                    long.Parse(Session["UserOrgID"].ToString()), reTrace);
            Epower.ITSM.Web.Common.ExcelExport.ExportProblemList(this, dt, fileName, arrFlide, Session["UserID"].ToString());
        }
        #endregion

        #region Ĭ�ϵ���excel
        /// <summary>
        /// Ĭ�ϵ���excel
        /// </summary>
        private void ProblemdefaultExcel()
        {
            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.QuestionTrace];
            int iRowCount = 0;

            DataTable dt = ProblemDealDP.GetProbsForCondNew_Init("", long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()), long.Parse(Session["UserOrgID"].ToString()), reTrace);
            Epower.ITSM.Web.Common.ExcelExport.ExportProblemList(this, dt, Session["UserID"].ToString());
        }

        /// <summary>
        /// Ĭ�ϵ���excel
        /// </summary>
        private void ProblemdefaultExcel(string[] fileName, string[] arrFlide)
        {
            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.QuestionTrace];
            int iRowCount = 0;

            DataTable dt = ProblemDealDP.GetProbsForCondNew_Init("", long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()), long.Parse(Session["UserOrgID"].ToString()), reTrace);
            Epower.ITSM.Web.Common.ExcelExport.ExportProblemList(this, dt, fileName, arrFlide, Session["UserID"].ToString());
        }
        #endregion

        #region ����Master_Master_Button_New_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("~/Forms/form_all_flowmodel.aspx?appid=210");
        }
        #endregion

        #region  ��ѯ�¼�
        /// <summary>
        /// ��ѯ�¼�
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            DropSQLwSave.SelectedIndex = 0;

            //�����ѯ��ťʱ����ȡ��ѯ���е����ݽ��в�ѯ
            TypeID = "1";

            LoadData();

            this.ctrCondition.SetDisplayMode = false;
        }
        #endregion

        #region Page_Load
        /// <summary>
        /// ҳ�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtonEvent();

            cpProblem.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(LoadData);

            dgProblem.Columns[dgProblem.Columns.Count - 2].Visible = CheckRight(Constant.admindeleteflow);

            if (!IsPostBack)
            {
                this.Master.KeyValue = "���������ⵥ��";
                hidUserID.Value = ((long)(Session["UserID"])).ToString();

                //���Ȩ��
                dgProblem.Columns[dgProblem.Columns.Count - 1].Visible = CheckRight(Constant.EquChangeQuery);
                InitDropSQLwSave();  //��ʼ����ѯ����

                re = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.QuestionTrace];


                #region װ�ظ߼���ѯ����

                if (hidSQLName.Value != "" && hidSQLName.Value != "==ѡ���ղز�ѯ����==")
                {
                    if (hidSQLName.Value != "Temp1")
                    {
                        DataTable dt = ProblemDealDP.getCST_ISSUE_Where("frmProblemMain", Session["UserName"].ToString(), hidSQLName.Value.ToString().Trim());

                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["SQLText"].ToString().Trim() == dt.Rows[1]["SQLText"].ToString().Trim())
                            {
                                #region ���Ϊԭ����
                                if (dt.Rows[0]["Name"].ToString().Trim() != "Temp1")
                                {
                                    //���·��ʴ���
                                    ProblemDealDP.updateCST_ISSUE_WhereNums(Session["UserName"].ToString(), dt.Rows[0]["Name"].ToString().Trim());

                                    hidSQLName.Value = dt.Rows[0]["Name"].ToString().Trim();
                                    TypeID = "0";
                                }

                                if (dt.Rows[1]["Name"].ToString().Trim() != "Temp1")
                                {
                                    //���·��ʴ���
                                    ProblemDealDP.updateCST_ISSUE_WhereNums(Session["UserName"].ToString(), dt.Rows[1]["Name"].ToString().Trim());

                                    hidSQLName.Value = dt.Rows[1]["Name"].ToString().Trim();
                                    TypeID = "0";
                                }
                            }
                                #endregion
                            else
                            {
                                #region �����Ϊԭ����
                                hidSQLName.Value = "Temp1";
                                TypeID = "0";
                                #endregion
                            }
                        }
                    }
                    else
                    {
                        TypeID = "0";
                    }
                }
                else
                {
                    DropSQLwSave.SelectedIndex = 0;
                    TypeID = "10";                       //����ǵ�һ�μ��أ���Ĭ�ϲ�ѯ���ڴ���ġ�����ʱ��Ϊ��һ���µ�Ȩ�޷�Χ�ڵļ�¼
                }

                #endregion

                LoadData();

                Session["FromUrl"] = "../ProbleForms/frmProblemMain.aspx";
                FromBackUrl = "../ProbleForms/frmProblemMain.aspx";
            }
            else
            {
                #region װ�ظ߼���ѯ����

                if (hidIsGaoji.Value == "1")
                {
                    //��ֵΪ1ʱ�������Ӹ߼��������洫�����ģ���ʱӦ��hidSQLName��ֵ����DropSQLwSave

                    InitDropSQLwSave(hidSQLName.Value);

                    #region
                    if (hidSQLName.Value != "" && hidSQLName.Value != "==ѡ���ղز�ѯ����==")
                    {
                        if (hidSQLName.Value != "Temp1")
                        {
                            //������ʱ�߼����������б�ѡ�е����Լ������һ���߼�����
                            DataTable dt = ProblemDealDP.getCST_ISSUE_Where("frmProblemMain", Session["UserName"].ToString(), hidSQLName.Value.ToString().Trim());

                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Rows[0]["SQLText"].ToString().Trim() == dt.Rows[1]["SQLText"].ToString().Trim())
                                {
                                    //�����ȫ��ȣ�������ʱ���õĲ�ѯ���������ǵ�ǰѡ�еĸ߼��������ƣ��������ڵ�ǰѡ�еĸ߼��������ƻ�����������N����ѯ��������ֻ����ʱ��ѯ
                                    #region ���Ϊԭ����
                                    if (dt.Rows[0]["Name"].ToString().Trim() != "Temp1")
                                    {
                                        //���·��ʴ���
                                        ProblemDealDP.updateCST_ISSUE_WhereNums(Session["UserName"].ToString(), dt.Rows[0]["Name"].ToString().Trim());

                                        hidSQLName.Value = dt.Rows[0]["Name"].ToString().Trim();
                                        TypeID = "0";
                                    }

                                    if (dt.Rows[1]["Name"].ToString().Trim() != "Temp1")
                                    {
                                        //���·��ʴ���
                                        ProblemDealDP.updateCST_ISSUE_WhereNums(Session["UserName"].ToString(), dt.Rows[1]["Name"].ToString().Trim());

                                        hidSQLName.Value = dt.Rows[1]["Name"].ToString().Trim();
                                        TypeID = "0";
                                    }
                                }
                                    #endregion
                                else
                                {
                                    #region �����Ϊԭ����������Ӹ߼����������������ѯ����Temp1����ʱxml����ѯ�����ļ�¼�����򣬲�ѯ�����б�ı���ֵ
                                    if (hidIsGaoji.Value == "1")
                                    {
                                        //�Ӹ߼�����������
                                        hidSQLName.Value = "Temp1";         //�����ԭ������ͬ�����ʱȡTemp1��xml��
                                        DropSQLwSave.SelectedIndex = 0;     //��ʱ����Ϊ�ǲ�ѯһ����ʱ�ģ����������б�Ӧ��Ϊ��Ŀ¼��
                                    }
                                    TypeID = "0";
                                    #endregion
                                }
                            }
                        }
                        else
                        {
                            TypeID = "0";
                        }
                    }
                    #endregion
                }
                else
                {
                    //������Ϊ0����˵���Ǹı������б�ĸ߼��������ƣ���ʱ�������б�ı���ֵ����hidSQLName
                    hidSQLName.Value = DropSQLwSave.SelectedItem.Text;
                }

                LoadData(hfTypeId.Value);
                #endregion
            }

            #region ��̬��ѯ: ���ö�̬��ѯ���� - 2013-04-01 @������

            this.ctrCondition.TableName = "Pro_Problemdeal";
            this.ctrCondition.mybtnSelectOnClick += new EventHandler(ctrCondition1_mybtnSelectOnClick);
            this.ctrCondition.mySelectedIndexChanged += new EventHandler(ctrCondition1_mySelectedIndexChanged);

            #endregion
        }

        #endregion

        #region ��̬��ѯ: ������ѡ��ͬ�Ĳ�ѯ�������ʱ���� - 2013-04-01 @������

        /// <summary>
        /// ������ѡ��ͬ�Ĳ�ѯ�������ʱ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ctrCondition1_mySelectedIndexChanged(object sender, EventArgs e)
        {
            this.TypeID = __ADVANCED_SEARCH;
            ctrCondition.ddlSelectChanged();

            LoadData(__ADVANCED_SEARCH);

            this.Master.TxtKeyName.Value = "���������ⵥ��";
        }

        #endregion

        #region ��̬��ѯ: �����̬��ѯ��ťʱ���� - 2013-04-01 @������

        /// <summary>
        /// �����̬��ѯ��ťʱ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ctrCondition1_mybtnSelectOnClick(object sender, EventArgs e)
        {
            ctrCondition.Bind();

            this.TypeID = __ADVANCED_SEARCH;
            LoadData(__ADVANCED_SEARCH);


            this.Master.TxtKeyName.Value = "���������ⵥ��";
        }

        #endregion

        #region  LoadData
        private void LoadData()
        {
            LoadData(TypeID);
        }
        /// <summary>
        /// ���ݼ���
        /// </summary>
        private void LoadData(String strTypeId)
        {
            int iRowCount = 0;
            if (Session["UserID"] == null || Session["UserDeptID"] == null || Session["UserOrgID"] == null)
            {
                return;
            }
            long lngUserID = (long)Session["UserID"];
            long lngDeptID = (long)Session["UserDeptID"];
            long lngOrgID = (long)Session["UserOrgID"];
            //���Ȩ��
            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.QuestionTrace];
            if (reTrace == null)
            {
                return;
            }
            DataTable dt = null;
            XmlDocument xmlDoc = null;
            switch (strTypeId)
            {
                case "0":
                    //�߼���ѯ
                    hidIsGaoji.Value = "0";         //��ֵ��ԭΪ0
                    //xmlDoc = SqlWhereShow(hidSQLName.Value);
                    //dt = ProblemDealDP.GetProblemsForCond(xmlDoc.InnerXml, lngUserID, lngDeptID, lngOrgID, reTrace, this.cpProblem.PageSize, this.cpProblem.CurrentPage, ref iRowCount);

                    Int32 intPageSize = this.cpProblem.PageSize;
                    Int32 intCurrentPage = this.cpProblem.CurrentPage;
                    Int32 intRowCount = iRowCount;
                    String strWhere = this.ctrCondition.strCondition;    // ��̬���ɵ�SQL���

                    dt = ProblemDealDP.GetProblemsWithMoreParams(strWhere,
                        lngUserID,
                        lngDeptID,
                        lngOrgID,
                        reTrace,
                        intPageSize,
                        intCurrentPage,
                        ref iRowCount);

                    break;
                case "1":
                    //��������
                    xmlDoc = GetXmlValue();
                    dt = ProblemDealDP.GetProblemsForCond(xmlDoc.InnerXml, lngUserID, lngDeptID, lngOrgID, reTrace, this.cpProblem.PageSize, this.cpProblem.CurrentPage, ref iRowCount);
                    break;
                case "10"://��ѯ�����ڴ���ģ�����ʱ��Ϊ��һ���µ�Ȩ�޷�Χ�ڵļ�¼
                    dt = ProblemDealDP.GetProbsForCondNew_Init("", long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()), long.Parse(Session["UserOrgID"].ToString()), reTrace, this.cpProblem.PageSize, this.cpProblem.CurrentPage, ref iRowCount);
                    break;
                default:
                    dt = new DataTable();
                    break;
            }


            dgProblem.DataSource = dt.DefaultView;
            dgProblem.DataBind();
            this.cpProblem.RecordCount = iRowCount;
            this.cpProblem.Bind();

        }

        #endregion ��ȡ����������ѯ����xmlֵ

        #region ��ȡ����������ѯ����xmlֵ
        /// <summary>
        /// ��ȡ����������ѯ����xmlֵ
        /// </summary>
        /// <returns></returns>
        private XmlDocument GetXmlValue()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlRoot = xmlDoc.CreateElement("Fields");
            XmlElement xmlEle;

            #region ���ⵥ��
            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "ProblemNo");
            xmlEle.SetAttribute("Value", this.Master.TxtKeyName.Value.Trim().ToString() == "���������ⵥ��" ? "" : this.Master.TxtKeyName.Value.Trim().ToString());
            xmlRoot.AppendChild(xmlEle);
            #endregion

            xmlDoc.AppendChild(xmlRoot);
            return xmlDoc;
        }
        #endregion

        #endregion

        #region ControlPage1_On_PostBack
        /// <summary>
        /// ControlPage1_On_PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ControlPage1_On_PostBack(object sender, EventArgs e)
        {
            LoadData();
        }
        #endregion

        #region dgProblem_ItemCreated
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgProblem_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    // (DataView)e.Item.NamingContainer;
                    if (i == 6 || i == 7 || i == 8 || i == 9 || i == 10)
                    {
                        int j = i - 5;  //ǰ����6�����ص���

                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");

                        //e.Item.Cells[i].Attributes.Add("onclick", String.Format("sortTable('{0}',{1},{1});", dg.ClientID, j, 0));
                    }
                }
            }
        }
        #endregion

        #region  dgProblem_ItemDataBound
        /// <summary>
        /// dgProblem_ItemDataBound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgProblem_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                decimal sScale = StringTool.String2Decimal(e.Item.Cells[10].Text);
                decimal sEffect = StringTool.String2Decimal(e.Item.Cells[11].Text);
                decimal sStress = StringTool.String2Decimal(e.Item.Cells[12].Text);

                if (sScale > 50)
                    e.Item.Cells[10].ForeColor = System.Drawing.Color.Red;
                if (sEffect > 50)
                    e.Item.Cells[11].ForeColor = System.Drawing.Color.Red;
                if (sStress > 50)
                    e.Item.Cells[12].ForeColor = System.Drawing.Color.Red;

                decimal d = decimal.Parse(e.Item.Cells[e.Item.Cells.Count - 5].Text);
                if (d < 0)
                {
                    //�������̹涨��ʱ����ɵ�
                    e.Item.Cells[6].ForeColor = System.Drawing.Color.Red;
                }

                if (DataBinder.Eval(e.Item.DataItem, "AssociateFlowID").ToString() == string.Empty || DataBinder.Eval(e.Item.DataItem, "AssociateFlowID").ToString() == "0")
                {
                    Button btnChange = (Button)e.Item.FindControl("btnAssociate");
                    Label lblChange = (Label)e.Item.FindControl("lblAssociate");
                    btnChange.Visible = true;
                    lblChange.Visible = false;
                }
                else
                {
                    Button btnChange = (Button)e.Item.FindControl("btnAssociate");
                    Label lblChange = (Label)e.Item.FindControl("lblAssociate");
                    btnChange.Visible = false;
                    lblChange.Visible = true;
                }

                //�淶��궯��--ly
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                String sID = DataBinder.Eval(e.Item.DataItem, "FlowID").ToString();
                e.Item.Attributes.Add("ondblclick", "SetUrl();window.open('../Forms/frmIssueView.aspx?FlowID=" + sID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes')");

                ((Label)e.Item.FindControl("Lb_ProblemNo")).Attributes.Add("onmouseover", "ShowDetailsInfo(this," + DataBinder.Eval(e.Item.DataItem, "Problem_ID").ToString() + ",400);");
            }

        }
        #endregion

        #region ���Ȩ�� CheckRight
        /// <summary>
        /// 
        /// </summary>
        /// <param name="OperatorID"></param>
        /// <returns></returns>
        private bool CheckRight(long OperatorID)
        {
            RightEntity re = (RightEntity)((Hashtable)Session["UserAllRights"])[OperatorID];
            if (re == null)
                return false;
            else
                return re.CanRead;
        }
        #endregion

        #region ɾ������dgProblem_DeleteCommand
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgProblem_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            LoadData();
        }
        #endregion

        #region dgProblem_ItemCommand
        /// <summary>
        /// dgProblem_ItemCommand
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgProblem_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "deal")  //����
            {
                Session["FromUrl"] = "../ProbleForms/frmProblemMain.aspx";

                string sUrl = "";
                long lngFlowID = long.Parse(e.Item.Cells[15].Text.Trim());
                sUrl = "../Forms/frmIssueView.aspx?FlowID=" + lngFlowID.ToString();
                Response.Redirect(sUrl);
            }
        }
        #endregion

        #region �߼���ѯ���

        #region DropSQLwSave_SelectedIndexChanged
        /// <summary>
        /// DropSQLwSave_SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DropSQLwSave_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Master.KeyValue = "���������ⵥ��";          //���¼�����ѯ�������
            string strTemp = string.Empty;              //��ʱ��Ÿı��������б�����
            String strTypeId = String.Empty;

            if (DropSQLwSave.SelectedItem.Text != "==ѡ���ղز�ѯ����==")
            {
                strTemp = DropSQLwSave.SelectedValue;           //��ѡ��ĸ߼��������ƴ洢����
                TypeID = "0";
                //LoadData();
                strTypeId = "0";
                //���·��ʴ���
                ProblemDealDP.updateCST_ISSUE_WhereNums(Session["UserName"].ToString(), DropSQLwSave.SelectedItem.Text.ToString());
                InitDropSQLwSave();             //���·��ʴ�����Ҫ���°������б����ݣ������ʴ�������

                DropSQLwSave.SelectedIndex = DropSQLwSave.Items.IndexOf(DropSQLwSave.Items.FindByValue(strTemp));
                hidSQLName.Value = DropSQLwSave.SelectedItem.Text.ToString();
            }
            else
            {
                hidSQLName.Value = "Temp1";
                strTypeId = "0";

            }

            LoadData(strTypeId);
        }
        #endregion

        #region ��ʼ���߼���ѯ����
        /// <summary>
        /// ��ʼ���߼���ѯ����
        /// </summary>
        private void InitDropSQLwSave()
        {
            DataTable dt = ProblemDealDP.getCST_ISSUE_LISTFASTQUERY("frmProblemMain", Session["UserName"].ToString(), string.Empty);
            DropSQLwSave.Items.Clear();
            DropSQLwSave.DataSource = dt.DefaultView;
            DropSQLwSave.DataTextField = "Name";
            DropSQLwSave.DataValueField = "ID";
            DropSQLwSave.DataBind();
            DropSQLwSave.Items.Insert(0, new ListItem("==ѡ���ղز�ѯ����==", "0"));
        }

        private void InitDropSQLwSave(string SQLName)
        {
            DataTable dt = ProblemDealDP.getCST_ISSUE_LISTFASTQUERY("frmProblemMain", Session["UserName"].ToString(), string.Empty);

            //���°󶨸߼���ѯ����
            DropSQLwSave.Items.Clear();
            DropSQLwSave.DataSource = dt.DefaultView;
            DropSQLwSave.DataTextField = "Name";
            DropSQLwSave.DataValueField = "ID";
            DropSQLwSave.DataBind();
            DropSQLwSave.Items.Insert(0, new ListItem("==ѡ���ղز�ѯ����==", "0"));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Name"].ToString().Trim() == SQLName.Trim())
                {
                    DropSQLwSave.SelectedIndex = DropSQLwSave.Items.IndexOf(DropSQLwSave.Items.FindByValue(dt.Rows[i]["ID"].ToString().Trim()));
                }
            }
        }
        #endregion

        #region ȷ���߼�����ʱִ��
        /// <summary>
        /// ȷ���߼�����ʱִ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void HidButton_Click(object sender, EventArgs e)
        {
            this.Master.KeyValue = "���������ⵥ��";

            LoadData();
        }
        #endregion

        #region ���ݸ߼��������Ƴ�ʼ����ѯ����
        /// <summary>
        /// ���ݸ߼��������Ƴ�ʼ����ѯ����
        /// </summary>
        /// <param name="SQLName"></param>
        protected XmlDocument SqlWhereShow(string SQLName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlRoot = xmlDoc.CreateElement("Fields");
            XmlElement xmlEle;

            DataTable dt = ProblemDealDP.getCST_ISSUE_LISTFASTQUERY("frmProblemMain", Session["UserName"].ToString(), SQLName);

            if (dt.Rows.Count > 0)
            {
                string[] SQLTextDetailList = dt.Rows[0]["SQLText"].ToString().Split(new string[] { "|@@?@$|" }, StringSplitOptions.None);
                this.HiddenColumn.Value = dt.Rows[0]["DISPLAYCOLUMN"].ToString();

                for (int i = 0; i < SQLTextDetailList.Length; i++)
                {
                    string[] SQLTextDetail = SQLTextDetailList[i].Trim().Split('=');

                    #region ���ⵥ��
                    if (SQLTextDetail[0].Trim() == "txtProblemNo.Text")
                    {
                        xmlEle = xmlDoc.CreateElement("Field");
                        xmlEle.SetAttribute("FieldName", "ProblemNo");
                        xmlEle.SetAttribute("Value", SQLTextDetail[1].Trim());
                        xmlRoot.AppendChild(xmlEle);
                    }
                    #endregion

                    #region �ʲ�����
                    if (SQLTextDetail[0].Trim() == "txtEquName.Text")
                    {
                        xmlEle = xmlDoc.CreateElement("Field");
                        xmlEle.SetAttribute("FieldName", "EquName");
                        xmlEle.SetAttribute("Value", SQLTextDetail[1].Trim());
                        xmlRoot.AppendChild(xmlEle);
                    }
                    #endregion

                    #region ����״̬
                    if (SQLTextDetail[0].Trim() == "cboStatus.SelectedValue")
                    {
                        xmlEle = xmlDoc.CreateElement("Field");
                        xmlEle.SetAttribute("FieldName", "FlowStatus");
                        xmlEle.SetAttribute("Value", SQLTextDetail[1].Trim());
                        xmlRoot.AppendChild(xmlEle);
                    }
                    #endregion

                    #region ����״̬
                    if (SQLTextDetail[0].Trim() == "ddlDealStatus.SelectedValue")
                    {
                        xmlEle = xmlDoc.CreateElement("Field");
                        xmlEle.SetAttribute("FieldName", "Status");
                        xmlEle.SetAttribute("Value", SQLTextDetail[1].Trim());
                        xmlRoot.AppendChild(xmlEle);
                    }
                    #endregion

                    #region �Ǽ�ʱ�俪ʼ
                    if (SQLTextDetail[0].Trim() == "ctrDateReSetTime.BeginTime")
                    {
                        xmlEle = xmlDoc.CreateElement("Field");
                        xmlEle.SetAttribute("FieldName", "MessageBegin");
                        xmlEle.SetAttribute("Value", SQLTextDetail[1].Trim());
                        xmlRoot.AppendChild(xmlEle);
                    }
                    #endregion

                    #region �Ǽ�ʱ�����
                    if (SQLTextDetail[0].Trim() == "ctrDateReSetTime.EndTime")
                    {
                        xmlEle = xmlDoc.CreateElement("Field");
                        xmlEle.SetAttribute("FieldName", "MessageEnd");
                        xmlEle.SetAttribute("Value", SQLTextDetail[1].Trim());
                        xmlRoot.AppendChild(xmlEle);
                    }
                    #endregion

                    #region ����
                    if (SQLTextDetail[0].Trim() == "txtSubject.Text")
                    {
                        xmlEle = xmlDoc.CreateElement("Field");
                        xmlEle.SetAttribute("FieldName", "Subject");
                        xmlEle.SetAttribute("Value", SQLTextDetail[1].Trim());
                        xmlRoot.AppendChild(xmlEle);
                    }
                    #endregion

                    #region �������
                    if (SQLTextDetail[0].Trim() == "CataProblemType.CatelogID")
                    {
                        xmlEle = xmlDoc.CreateElement("Field");
                        xmlEle.SetAttribute("FieldName", "CataProblemType");
                        xmlEle.SetAttribute("Value", SQLTextDetail[1].Trim());
                        xmlRoot.AppendChild(xmlEle);
                    }
                    #endregion

                    #region ���⼶��
                    if (SQLTextDetail[0].Trim() == "CataProblemLevel.CatelogID")
                    {
                        xmlEle = xmlDoc.CreateElement("Field");
                        xmlEle.SetAttribute("FieldName", "CataProblemLevel");
                        xmlEle.SetAttribute("Value", SQLTextDetail[1].Trim());
                        xmlRoot.AppendChild(xmlEle);
                    }
                    #endregion

                    #region Ӱ���
                    if (SQLTextDetail[0].Trim() == "CtrFCDEffect.CatelogID")
                    {
                        xmlEle = xmlDoc.CreateElement("Field");
                        xmlEle.SetAttribute("FieldName", "CtrFCDEffect");
                        xmlEle.SetAttribute("Value", SQLTextDetail[1].Trim());
                        xmlRoot.AppendChild(xmlEle);
                    }
                    #endregion

                    #region ������
                    if (SQLTextDetail[0].Trim() == "CtrFCDInstancy.CatelogID")
                    {
                        xmlEle = xmlDoc.CreateElement("Field");
                        xmlEle.SetAttribute("FieldName", "CtrFCDInstancy");
                        xmlEle.SetAttribute("Value", SQLTextDetail[1].Trim());
                        xmlRoot.AppendChild(xmlEle);
                    }
                    #endregion


                    #region �Ǽ���
                    if (SQLTextDetail[0].Trim() == "txtRegUser.Text")
                    {
                        xmlEle = xmlDoc.CreateElement("Field");
                        xmlEle.SetAttribute("FieldName", "txtRegUser");
                        xmlEle.SetAttribute("Value", SQLTextDetail[1].Trim());
                        xmlRoot.AppendChild(xmlEle);
                    }
                    #endregion
                }
            }

            xmlDoc.AppendChild(xmlRoot);
            return xmlDoc;
        }

        #endregion

        #endregion

      
        /// <summary>
        /// ɾ����¼��, ˢ������.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void hidd_btnDelete_Click(object sender, EventArgs e)
        {
            LoadData(hfTypeId.Value);
        }
    }
}
