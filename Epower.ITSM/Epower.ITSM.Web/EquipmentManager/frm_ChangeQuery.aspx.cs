/*******************************************************************
 * ��Ȩ���У�
 * Description��������߼���ѯҳ��
 * Create By  ��SuperMan
 * Create Date��2011-08-22
 * *****************************************************************/
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
using Epower.ITSM.SqlDAL;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;
using Epower.DevBase.BaseTools;
using System.Data.OracleClient;

namespace Epower.ITSM.Web.EquipmentManager
{
    /// <summary>
    /// frm_ChangeQuery
    /// </summary>
    public partial class frm_ChangeQuery : BasePage
    {
        #region �Զ������

        RightEntity re = null;
        static string staMsgDateBegion = "";
        static string staCustInfo = "";
        static int icurrent = 0;   //1.�߼���ѯ 2.��ѯ�¼� 3.ҳ���һ�μ���
        #endregion

        #region SetParentButtonEvent
        /// <summary>
        /// ���ø����尴ť�¼�
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.Master_Button_ExportExcel_Click += new Global_BtnClick(Master_Master_Button_ExportExcel_Click);
            this.Master.ShowQueryButton(true);
            this.Master.TxtKeyName.Visible = true;
            this.Master.ShowNewButton(false);
            this.Master.ShowExportExcelButton(true);

        }
        #endregion

        #region Master_Master_Button_Query_Click
        /// <summary>
        /// ��ѯ�¼�---
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            icurrent = 2;
            LoadData();
            DropSQLwSave.SelectedIndex = 0;

            this.ctrCondition.SetDisplayMode = false;
        }
        #endregion

        #region ����EXCEL�¼�Master_Master_Button_ExportExcel_Click
        /// <summary>
        /// ��ѯ�¼�
        /// </summary>
        void Master_Master_Button_ExportExcel_Click()
        {
            string[] key = null;
            string[] value = null;//, "InstancyName""������",
            DataTable dt = GetDataTable();
            string[] arrField = { "ChangeNo", "ChangeTypeName", "RegTime", "MastCustName", "CustName", "CTel", "CustAddress", "Subject", "Content", "EffectName", "LevelName", "DealStatus", "ChangeAnalyses", "ChangeAnalysesResult", "ChangeTime", "DealStatus", "TPersonList",
                                "isplanchange", "ChangeNeedPeople", "Isplan","CHANGE_PLACE_NAME", "PLAN_BEGIN_TIME", "PLAN_END_TIME", "isbuseffect", "BUS_EFFECT", "isdataeffect", "DATA_EFFECT", "CHANGE_WINDOW_NAME", "isstopserver", "STOP_SERVER_REMARK","real_begin_time","real_end_time"
                                };
            string[] fileName = { "�������", "������", "�Ǽ�ʱ��", "����λ", "�ͻ�����", "�칫�绰", "�ͻ���ַ", "ժҪ", "��������", "Ӱ���", "�������", "���״̬", "�������", "�������", "���ʱ��", "����״̬", "������",
                                "�Ƿ�ƻ��Ա��","���������","Ӧ�����˷���","�������","�ƻ���ʼʱ��","�ƻ����ʱ��","�Ƿ�ҵ��Ӱ��","ҵ��Ӱ��˵��","�Ƿ�����Ӱ��","����Ӱ��˵��","�������","�Ƿ�ͣ�÷���","ͣ�÷���˵��","ʵ�ʿ�ʼʱ��","ʵ�ʽ���ʱ��"
                                };

            //string s = "�������";
            //for (int i = 1; i < arrField.Length; i++)
            //{
            //    s += "," + PageDeal.GetLanguageValue1(arrField[i], "�����");
            //}
            //fileName = s.Split(',');
            //if (!string.IsNullOrEmpty(this.HiddenColumn.Value))
            //{
            //    string[] columnValues = this.HiddenColumn.Value.Split(',');
            //    if (columnValues.Length > 1)
            //    {
            //        string k = "ChangeNo";
            //        for (int i = 0; i < columnValues.Length - 1; i++)
            //        {
            //            k += "," + columnValues[i];
            //        }
            //        key = k.Split(',');
            //        string v = "�������";
            //        for (int i = 1; i < key.Length; i++)
            //        {
            //            v += "," + PageDeal.GetLanguageValue1(key[i], "�����");
            //        }
            //        value = v.Split(',');
            //    }
            //}

            //ȡ���������б�
            string strPersonList = string.Empty;
            DataColumn dtcl = new DataColumn("TPersonList");
            dt.Columns.Add(dtcl);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                strPersonList = ChangeDealDP.GetPersonList(long.Parse(dt.Rows[i]["FlowID"].ToString()));

                dt.Rows[i].SetModified();
                dt.Rows[i]["TPersonList"] = strPersonList;
            }
            if (!string.IsNullOrEmpty(this.HiddenColumn.Value))
            {
                Epower.ITSM.Web.Common.ExcelExport.ExportChangeList(this, dt, value, key, Session["UserID"].ToString());
            }
            else
            {
                Epower.ITSM.Web.Common.ExcelExport.ExportChangeList(this, dt, fileName, arrField, Session["UserID"].ToString());
            }

            //Epower.ITSM.Web.Common.ExcelExport.ExportChangeList(this, dt, Session["UserID"].ToString());
        }
        #endregion

        #region Master_Master_Button_New_Click����
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("~/Forms/form_all_flowmodel.aspx?appid=420");
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
            cpChange.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(LoadData);

            //Ӧ�ù���Աɾ��Ȩ��
            dgProblem.Columns[dgProblem.Columns.Count - 1].Visible = CheckRight(Constant.admindeleteflow);

            if (!IsPostBack)
            {

                icurrent = 3;
                PageLoadQuery();

                SetHeaderText();
            }

            #region ��̬��ѯ: ���ö�̬��ѯ���� - 2013-04-01 @������

            this.ctrCondition.TableName = "Equ_ChangeService";
            this.ctrCondition.mybtnSelectOnClick += new EventHandler(ctrCondition1_mybtnSelectOnClick);
            this.ctrCondition.mySelectedIndexChanged += new EventHandler(ctrCondition1_mySelectedIndexChanged);

            #endregion
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
            icurrent = 1;    // �߼���ѯ
            LoadData();

            this.Master.TxtKeyName.Value = "������������,�ͻ���Ϣ";
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
            //this.TypeID = __ADVANCED_SEARCH;
            ctrCondition.ddlSelectChanged();

            //LoadData(__ADVANCED_SEARCH);
            icurrent = 1;    // �߼���ѯ
            LoadData();

            this.Master.TxtKeyName.Value = "������������,�ͻ���Ϣ";
        }

        #endregion

        #region ����datagrid��ͷ��ʾ ����ǰ 2013-05-17
        /// <summary>
        /// ����datagrid��ͷ��ʾ
        /// </summary>
        private void SetHeaderText()
        {
            dgProblem.Columns[2].HeaderText = PageDeal.GetLanguageValue("Change_ChangeNo");
            dgProblem.Columns[3].HeaderText = PageDeal.GetLanguageValue("Change_CustName");
            dgProblem.Columns[4].HeaderText = PageDeal.GetLanguageValue("Change_CustContract");
            dgProblem.Columns[5].HeaderText = PageDeal.GetLanguageValue("Change_InstancyName");
            dgProblem.Columns[6].HeaderText = PageDeal.GetLanguageValue("Change_EffectName");
            dgProblem.Columns[7].HeaderText = PageDeal.GetLanguageValue("Change_ChangeTime");
            dgProblem.Columns[8].HeaderText = PageDeal.GetLanguageValue("Change_DealStatus");
        }
        #endregion

        #region LoadData

        /// <summary>
        /// ȡ���һ����δ��������̵�
        /// </summary>
        /// <param name="iRowCount">������</param>
        /// <returns></returns>
        private DataTable GetRecentlyMonthData(ref Int32 iRowCount)
        {
            long lngUserID = (long)Session["UserID"];
            long lngDeptID = (long)Session["UserDeptID"];
            long lngOrgID = (long)Session["UserOrgID"];
            //���Ȩ��
            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.EquChangeQuery];

            String sWhere = String.Empty;
            sWhere += " and ChangeTime>= to_date(" + StringTool.SqlQ(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd")) + ",'yyyy-MM-dd HH24:mi:ss')";
            sWhere += " and ChangeTime<= to_date(" + StringTool.SqlQ(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59") + ",'yyyy-MM-dd HH24:mi:ss')";
            sWhere += " and status=20 ";

            DataTable dt = ChangeDealDP.GetChangeDealDataForCustInfo(lngUserID, lngDeptID, lngOrgID, reTrace, sWhere, this.cpChange.PageSize, this.cpChange.CurrentPage, ref iRowCount);

            return dt;
        }

        /// <summary>
        /// [����Excel] ȡ���һ����δ��������̵�
        /// </summary>
        /// <param name="iRowCount">������</param>
        /// <returns></returns>
        private DataTable GetRecentlyMonthDataForExportExcel()
        {
            long lngUserID = (long)Session["UserID"];
            long lngDeptID = (long)Session["UserDeptID"];
            long lngOrgID = (long)Session["UserOrgID"];
            //���Ȩ��
            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.EquChangeQuery];

            String sWhere = String.Empty;
            sWhere += " and ChangeTime>= to_date(" + StringTool.SqlQ(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd")) + ",'yyyy-MM-dd HH24:mi:ss')";
            sWhere += " and ChangeTime<= to_date(" + StringTool.SqlQ(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59") + ",'yyyy-MM-dd HH24:mi:ss')";
            sWhere += " and status=20 ";

            DataTable dt = ChangeDealDP.GetChangeDealDataForCustInfo(lngUserID, lngDeptID, lngOrgID, reTrace, sWhere);

            return dt;
        }

        /// <summary>
        /// ȡ���ٲ�ѯ������̵�
        /// </summary>
        /// <param name="iRowCount">������</param>
        /// <returns></returns>
        private DataTable GetQuicklySearchData(ref Int32 iRowCount)
        {
            long lngUserID = (long)Session["UserID"];
            long lngDeptID = (long)Session["UserDeptID"];
            long lngOrgID = (long)Session["UserOrgID"];
            //���Ȩ��
            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.EquChangeQuery];

            String sWhere = "";
            String strCustInfo = this.Master.TxtKeyName.Value.Trim().ToString();

            if (strCustInfo != "������������,�ͻ���Ϣ")
            {
                string sSqlWhere1 = @"select ID from Br_ECustomer where 1=1";

                sSqlWhere1 += " And (";
                sSqlWhere1 += " CustName like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere1 += " OR Contact like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere1 += " OR customcode like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere1 += " OR CustAddress like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere1 += " OR mastCustname like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere1 += " OR Email like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere1 += " OR CTel like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere1 += " ) ";
                sWhere += " AND (custid IN (" + sSqlWhere1 + ")";

                sWhere += "  or  ChangeNo like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%") + ")";
            }

            DataTable dt = ChangeDealDP.GetChangeDealDataForCustInfo(lngUserID, lngDeptID, lngOrgID, reTrace, sWhere, this.cpChange.PageSize, this.cpChange.CurrentPage, ref iRowCount);

            return dt;
        }

        /// <summary>
        /// [����Excel] ȡ���ٲ�ѯ������̵�
        /// </summary>
        /// <param name="iRowCount">������</param>
        /// <returns></returns>
        private DataTable GetQuicklySearchDataForExportExcel()
        {
            long lngUserID = (long)Session["UserID"];
            long lngDeptID = (long)Session["UserDeptID"];
            long lngOrgID = (long)Session["UserOrgID"];
            //���Ȩ��
            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.EquChangeQuery];

            String sWhere = "";
            String strCustInfo = this.Master.TxtKeyName.Value.Trim().ToString();

            if (strCustInfo != "������������,�ͻ���Ϣ")
            {
                string sSqlWhere1 = @"select ID from Br_ECustomer where 1=1";

                sSqlWhere1 += " And (";
                sSqlWhere1 += " CustName like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere1 += " OR Contact like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere1 += " OR customcode like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere1 += " OR CustAddress like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere1 += " OR mastCustname like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere1 += " OR Email like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere1 += " OR CTel like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere1 += " ) ";
                sWhere += " AND (custid IN (" + sSqlWhere1 + ")";

                sWhere += "  or  ChangeNo like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%") + ")";
            }

            DataTable dt = ChangeDealDP.GetChangeDealDataForCustInfo(lngUserID, lngDeptID, lngOrgID, reTrace, sWhere);

            return dt;
        }

        /// <summary>
        /// ȡ�߼���ѯ������̵�
        /// </summary>
        /// <param name="iRowCount">������</param>
        /// <returns></returns>
        private DataTable GetAdvancedSearchData(ref Int32 iRowCount)
        {
            long lngUserID = (long)Session["UserID"];
            long lngDeptID = (long)Session["UserDeptID"];
            long lngOrgID = (long)Session["UserOrgID"];
            //���Ȩ��
            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.EquChangeQuery];

            String sWhere = "";
            if (!String.IsNullOrEmpty(this.ctrCondition.strCondition.Trim()))
            {
                sWhere = " AND " + this.ctrCondition.strCondition;
            }


            DataTable dt = ChangeDealDP.GetChangeDealDataForCustInfo(lngUserID, lngDeptID, lngOrgID, reTrace, sWhere, this.cpChange.PageSize, this.cpChange.CurrentPage, ref iRowCount);

            return dt;
        }

        /// <summary>
        /// [����Excel] ȡ�߼���ѯ������̵�
        /// </summary>
        /// <param name="iRowCount">������</param>
        /// <returns></returns>
        private DataTable GetAdvancedSearchDataForExportExcel()
        {
            long lngUserID = (long)Session["UserID"];
            long lngDeptID = (long)Session["UserDeptID"];
            long lngOrgID = (long)Session["UserOrgID"];
            //���Ȩ��
            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.EquChangeQuery];

            String sWhere = "";
            if (!String.IsNullOrEmpty(this.ctrCondition.strCondition.Trim()))
            {
                sWhere = " AND " + this.ctrCondition.strCondition;
            }


            DataTable dt = ChangeDealDP.GetChangeDealDataForCustInfo(lngUserID, lngDeptID, lngOrgID, reTrace, sWhere);

            return dt;
        }

        /// <summary>
        /// ������ʾ���̵��б�
        /// </summary>
        private void LoadData()
        {
            Int32 iRowCount = 0;

            DataTable dt = null;
            switch (icurrent)
            {
                case 1:    // �߼�����
                    dt = GetAdvancedSearchData(ref iRowCount);
                    break;

                case 2:    // ��������
                    dt = GetQuicklySearchData(ref iRowCount);
                    break;

                case 3:    // ���һ����δ��������̵�
                    dt = GetRecentlyMonthData(ref iRowCount);
                    break;
            }

            dgProblem.DataSource = dt.DefaultView;
            dgProblem.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");
            dgProblem.DataBind();
            this.cpChange.RecordCount = iRowCount;
            this.cpChange.Bind();
        }

        /// <summary>
        /// ���ݼ���
        /// </summary>
        private void LoadData2()
        {
            int iRowCount = 0;
            string sWhere = "";

            sWhere = LoadDataPre();

            //���μ���ʱֻ����һ���µ�����
            if (icurrent == 3)
            {
                sWhere = "";
                sWhere += " and ChangeTime>= to_date(" + StringTool.SqlQ(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd")) + ",'yyyy-MM-dd HH24:mi:ss')";
                sWhere += " and ChangeTime<= to_date(" + StringTool.SqlQ(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59") + ",'yyyy-MM-dd HH24:mi:ss')";
                sWhere += " and status=20 ";

                hidSQLName.Value = "Temp1";
                SqlWhereShow("Temp1");
                this.DropSQLwSave.SelectedIndex = DropSQLwSave.Items.IndexOf(DropSQLwSave.Items.FindByValue("0"));
            }

            long lngUserID = (long)Session["UserID"];
            long lngDeptID = (long)Session["UserDeptID"];
            long lngOrgID = (long)Session["UserOrgID"];
            //���Ȩ��
            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.EquChangeQuery];
            DataTable dt;
            dt = ChangeDealDP.GetChangeDealDataForCustInfo(lngUserID, lngDeptID, lngOrgID, reTrace, sWhere, this.cpChange.PageSize, this.cpChange.CurrentPage, ref iRowCount);

            //������ʾ�ֶ�
            if (!string.IsNullOrEmpty(this.HiddenColumn.Value))
            {
                SetColumnDisplay(this.HiddenColumn.Value);
            }

            dgProblem.DataSource = dt.DefaultView;
            dgProblem.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");
            dgProblem.DataBind();
            this.cpChange.RecordCount = iRowCount;
            this.cpChange.Bind();
        }

        /// <summary>
        /// ��ȡҳ������
        /// </summary>
        /// <returns></returns>
        private DataTable GetDataTable()
        {
            DataTable dt = null;
            switch (icurrent)
            {
                case 1:    // �߼�����
                    dt = GetAdvancedSearchDataForExportExcel();
                    break;

                case 2:    // ��������
                    dt = GetQuicklySearchDataForExportExcel();
                    break;

                case 3:    // ���һ����δ��������̵�
                    dt = GetRecentlyMonthDataForExportExcel();
                    break;
            }

            return dt;
        }


        #endregion

        #region ��ʾҳ���ַ
        /// <summary>
        /// ��ʾҳ���ַ
        /// </summary>
        /// <param name="lngNoticeID"></param>
        /// <returns></returns>
        protected string GetUrl(decimal lngFlowID)
        {
            string sUrl = "";
            sUrl = "javascript:window.open('../Forms/frmIssueView.aspx?FlowID=" + lngFlowID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
            return sUrl;
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
                for (int i = 0; i < e.Item.Cells.Count - 5; i++)
                {
                    int j = i - 2;  //ǰ����6�����ص���
                    e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                }
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

        #region �����¼�

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgProblem_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //�淶��궯��--ly
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                String sID = DataBinder.Eval(e.Item.DataItem, "FlowID").ToString();
                e.Item.Attributes.Add("ondblclick", "window.open('../Forms/frmIssueView.aspx?FlowID=" + sID.ToString() + "&randomid='+GetRandom(),'MainFrame','scrollbars=yes,resizable=yes')");

                ((Label)e.Item.FindControl("lblChangeNo")).Attributes.Add("onmouseover", "ShowDetailsInfo(this," + DataBinder.Eval(e.Item.DataItem, "ID").ToString() + ",400);");
            }
        }

        protected void DropSQLwSave_SelectedIndexChanged(object sender, EventArgs e)
        {
            icurrent = 1;
            string xx = DropSQLwSave.SelectedItem.Text;

            if (DropSQLwSave.SelectedItem.Text != "==ѡ���ղز�ѯ����==")
            {
                SqlWhereShow(DropSQLwSave.SelectedItem.Text.ToString());
                LoadData();
                hidSQLName.Value = DropSQLwSave.SelectedItem.Text.ToString();

                //���·��ʴ���
                ChangeDealDP.updateCST_ISSUE_WhereNums(Session["UserName"].ToString(), DropSQLwSave.SelectedItem.Text.ToString());

                hidSQLName.Value = DropSQLwSave.SelectedItem.Text.ToString();
            }
            else
            {
                SqlWhereShow("Temp1");
                hidSQLName.Value = "Temp1";
                LoadData();
            }
            this.Master.KeyValue = "������������,�ͻ���Ϣ";
        }

        protected void btn_addnew_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Forms/form_all_flowmodel.aspx?appid=420");
        }

        protected void btn_excel_Click(object sender, EventArgs e)
        {

            DataTable dt = GetDataTable();
            //ȡ���������б�
            string strPersonList = string.Empty;
            DataColumn dtcl = new DataColumn("TPersonList");
            dt.Columns.Add(dtcl);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                strPersonList = ChangeDealDP.GetPersonList(long.Parse(dt.Rows[i]["FlowID"].ToString()));

                dt.Rows[i].SetModified();
                dt.Rows[i]["TPersonList"] = strPersonList;
            }

            Epower.ITSM.Web.Common.ExcelExport.ExportChangeList(this, dt, Session["UserID"].ToString());
        }

        protected void btn_query_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        #endregion

        #region �Զ��巽��
        private void InitDropSQLwSave()
        {
            DataTable dt = ChangeDealDP.getCST_ISSUE_LISTFASTQUERY("frm_ChangeQuery", Session["UserName"].ToString(), string.Empty);
            DropSQLwSave.Items.Clear();
            DropSQLwSave.DataSource = dt.DefaultView;
            DropSQLwSave.DataTextField = "Name";
            DropSQLwSave.DataValueField = "ID";
            DropSQLwSave.DataBind();
            DropSQLwSave.Items.Insert(0, new ListItem("==ѡ���ղز�ѯ����==", "0"));
        }

        protected void SqlWhereShow(string SQLName)
        {
            DataTable dt = ChangeDealDP.getCST_ISSUE_LISTFASTQUERY("frm_ChangeQuery", Session["UserName"].ToString(), SQLName);

            if (dt.Rows.Count > 0)
            {
                string[] SQLTextDetailList = dt.Rows[0]["SQLText"].ToString().Split(new string[] { "|@@?@$|" }, StringSplitOptions.None);

                for (int i = 0; i < SQLTextDetailList.Length; i++)
                {
                    string[] SQLTextDetail = SQLTextDetailList[i].Trim().Split('=');

                    //�û���Ϣ
                    if (SQLTextDetail[0].Trim() == "txtCustInfo.Text")
                    {
                        this.txtCustInfo.Text = SQLTextDetail[1].Trim();
                    }

                    //����״̬
                    if (SQLTextDetail[0].Trim() == "cboStatus.SelectedValue")
                    {
                        this.cboStatus.SelectedValue = SQLTextDetail[1].Trim();
                    }

                    //���״̬
                    if (SQLTextDetail[0].Trim() == "CtrFlowChangeState.SelectedValue")
                    {
                        this.CtrDealState.CatelogID = Convert.ToInt64(SQLTextDetail[1].Trim());
                    }

                    //����
                    if (SQLTextDetail[0].Trim() == "txtTitle.Text")
                    {
                        this.txtTitle.Text = SQLTextDetail[1].Trim();
                    }

                    //���ʱ��
                    if (SQLTextDetail[0].Trim() == "ctrDateSelectTime1.BeginTime")
                    {
                        this.txtRegTime.Text = SQLTextDetail[1].Trim();
                    }
                    if (SQLTextDetail[0].Trim() == "ctrDateSelectTime1.EndTime")
                    {
                        this.txtEndTime.Text = SQLTextDetail[1].Trim();
                    }

                    //�ʲ�Ŀ¼
                    if (SQLTextDetail[0].Trim() == "txtEquipmentDir.Text")
                    {
                        this.txtEquipmentDir.Text = SQLTextDetail[1].Trim();
                    }

                    //�ʲ�����
                    if (SQLTextDetail[0].Trim() == "txtEquipmentName.Text")
                    {
                        this.txtEquipmentName.Text = SQLTextDetail[1].Trim();
                    }

                    //�������
                    if (SQLTextDetail[0].Trim() == "CtrFCDlevel.CatelogID")
                    {
                        this.CtrFCDlevel.CatelogID = Convert.ToInt64(SQLTextDetail[1].Trim());
                    }

                    //Ӱ���
                    if (SQLTextDetail[0].Trim() == "CtrFCDEffect.CatelogID")
                    {
                        this.CtrFCDEffect.CatelogID = Convert.ToInt64(SQLTextDetail[1].Trim());
                    }

                    //������
                    if (SQLTextDetail[0].Trim() == "CtrFCDInstancy.CatelogID")
                    {
                        this.CtrFCDInstancy.CatelogID = Convert.ToInt64(SQLTextDetail[1].Trim());
                    }
                    //������
                    if (SQLTextDetail[0].Trim() == "CtrChangeType.CatelogID")
                    {
                        this.CtrChangeType.CatelogID = Convert.ToInt64(SQLTextDetail[1].Trim());
                    }
                }

            }

        }

        private void InitDropSQLwSave1(string SQLName)
        {
            DataTable dt = ChangeDealDP.getCST_ISSUE_LISTFASTQUERY("frm_ChangeQuery", Session["UserName"].ToString(), string.Empty);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Name"].ToString().Trim() == SQLName.Trim())
                {
                    DropSQLwSave.SelectedValue = dt.Rows[i]["ID"].ToString().Trim();
                }
            }
        }

        private void setControlValue()
        {
            cboStatus.SelectedValue = "20";
            CtrDealState.CatelogID = 0;
            CtrFCDlevel.CatelogValue = string.Empty;
            CtrFCDEffect.CatelogValue = string.Empty;
            CtrFCDEffect.CatelogID = -1;
            CtrFCDInstancy.CatelogValue = string.Empty;
            CtrChangeType.CatelogValue = string.Empty;
            txtCustInfo.Text = staCustInfo;
            txtEquipmentName.Text = string.Empty;
            txtEquipmentDir.Text = string.Empty;
            txtTitle.Text = string.Empty;
            txtRegTime.Text = staMsgDateBegion;
            txtEndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// ��ѯ����
        /// </summary>
        /// <returns></returns>
        private string LoadDataPre()
        {
            string sWhere = "";
            int iRowCount = 0;
            string strCustInfo = "";

            if (cboStatus.SelectedItem.Value.Trim() != "-1")
                sWhere += " and status= " + cboStatus.SelectedItem.Value.ToString();
            if (!string.IsNullOrEmpty(CtrDealState.CatelogValue.Trim()))
                sWhere += " and DealStatusID= " + CtrDealState.CatelogID.ToString();

            if (!string.IsNullOrEmpty(CtrFCDlevel.CatelogValue.Trim()))
                sWhere += " and LevelID= " + CtrFCDlevel.CatelogID.ToString();
            if (!string.IsNullOrEmpty(CtrFCDEffect.CatelogValue.Trim()))
                sWhere += " and EffectID= " + CtrFCDEffect.CatelogID.ToString();
            if (!string.IsNullOrEmpty(CtrFCDInstancy.CatelogValue.Trim()))
                sWhere += " and InstancyID= " + CtrFCDInstancy.CatelogID.ToString();
            if (!string.IsNullOrEmpty(CtrChangeType.CatelogValue.Trim()))
                sWhere += "and ChangeTypeID=" + CtrChangeType.CatelogID.ToString();
            if (!string.IsNullOrEmpty(txtCustInfo.Text.Trim()))
            {
                strCustInfo = txtCustInfo.Text.Trim();

                string sSqlWhere = @"select ID from Br_ECustomer where 1=1";

                sSqlWhere += " And (";
                sSqlWhere += " CustName like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere += " OR Contact like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere += " OR customcode like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere += " OR CustAddress like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere += " OR mastCustname like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere += " OR Email like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere += " OR CTel like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere += " ) ";
                sWhere += " AND custid IN(" + sSqlWhere + ")";

            }

            #region ɸѡ�ʲ���Ϣ
            string sWhereEqu = "";
            if (!string.IsNullOrEmpty(txtEquipmentName.Text.Trim()))
            {
                sWhereEqu = "EquName like " + StringTool.SqlQ("%" + txtEquipmentName.Text.Trim() + "%");
            }

            if (!string.IsNullOrEmpty(txtEquipmentDir.Text.Trim()))
            {
                if (sWhereEqu == "")
                {
                    sWhereEqu = "a.ListName like " + StringTool.SqlQ("%" + txtEquipmentDir.Text.Trim() + "%");
                }
                else
                {
                    sWhereEqu += " and  a.ListName like " + StringTool.SqlQ("%" + txtEquipmentDir.Text.Trim() + "%");
                }
            }
            DataTable dt = ChangeDealDP.getEquipment(sWhereEqu);
            string changeids = "";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == (dt.Rows.Count - 1))
                    {
                        changeids += "'" + dt.Rows[i]["ChangeID"].ToString().Trim() + "'";
                    }
                    else
                    {
                        changeids += "'" + dt.Rows[i]["ChangeID"].ToString().Trim() + "',";
                    }
                }
            }
            else
            {
                changeids += "'0'";
            }

            if (changeids != "")
            {
                sWhere += " and ID in (" + changeids + ")";
            }

            #endregion

            if (!string.IsNullOrEmpty(txtTitle.Text.Trim()))
                sWhere += " and Subject like " + StringTool.SqlQ("%" + txtTitle.Text.Trim() + "%");
            if (!string.IsNullOrEmpty(txtRegTime.Text.Trim()))
                sWhere += " and ChangeTime>= to_date(" + StringTool.SqlQ(txtRegTime.Text.Trim()) + ",'yyyy-MM-dd HH24:mi:ss')";
            if (!string.IsNullOrEmpty(txtEndTime.Text.Trim()))
                sWhere += " and ChangeTime<= to_date(" + StringTool.SqlQ(txtEndTime.Text.Trim()) + ",'yyyy-MM-dd HH24:mi:ss')";

            if (icurrent == 2)
            {
                sWhere = "";
                strCustInfo = this.Master.TxtKeyName.Value.Trim().ToString();

                if (strCustInfo != "������������,�ͻ���Ϣ")
                {
                    string sSqlWhere1 = @"select ID from Br_ECustomer where 1=1";

                    sSqlWhere1 += " And (";
                    sSqlWhere1 += " CustName like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                    sSqlWhere1 += " OR Contact like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                    sSqlWhere1 += " OR customcode like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                    sSqlWhere1 += " OR CustAddress like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                    sSqlWhere1 += " OR mastCustname like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                    sSqlWhere1 += " OR Email like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                    sSqlWhere1 += " OR CTel like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                    sSqlWhere1 += " ) ";
                    sWhere += " AND (custid IN (" + sSqlWhere1 + ")";

                    sWhere += "  or  ChangeNo like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%") + ")";
                }
            }

            DataTable dt1 = ChangeDealDP.getCST_ISSUE_LISTFASTQUERY("frm_ChangeQuery", Session["UserName"].ToString(), DropSQLwSave.SelectedItem.Text.ToString());
            if (dt1.Rows.Count > 0)
                this.HiddenColumn.Value = dt1.Rows[0]["DISPLAYCOLUMN"].ToString();
            return sWhere;
        }

        private string LoadDataPre2()
        {
            string sWhere = "";
            int iRowCount = 0;
            string strCustInfo = "";

            if (cboStatus.SelectedItem.Value.Trim() != "-1")
                sWhere += " and status= " + cboStatus.SelectedItem.Value.ToString();
            if (!string.IsNullOrEmpty(CtrDealState.CatelogValue.Trim()))
                sWhere += " and DealStatusID= " + CtrDealState.CatelogID.ToString();

            if (!string.IsNullOrEmpty(CtrFCDlevel.CatelogValue.Trim()))
                sWhere += " and LevelID= " + CtrFCDlevel.CatelogID.ToString();
            if (!string.IsNullOrEmpty(CtrFCDEffect.CatelogValue.Trim()))
                sWhere += " and EffectID= " + CtrFCDEffect.CatelogID.ToString();
            if (!string.IsNullOrEmpty(CtrFCDInstancy.CatelogValue.Trim()))
                sWhere += " and InstancyID= " + CtrFCDInstancy.CatelogID.ToString();
            if (!string.IsNullOrEmpty(CtrChangeType.CatelogValue.Trim()))
                sWhere += "and ChangeTypeID=" + CtrChangeType.CatelogID.ToString();

            if (!string.IsNullOrEmpty(txtCustInfo.Text.Trim()))
            {
                strCustInfo = txtCustInfo.Text.Trim();

                string sSqlWhere = @"select ID from Br_ECustomer where 1=1";

                sSqlWhere += " And (";
                sSqlWhere += " CustName like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere += " OR Contact like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere += " OR customcode like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere += " OR CustAddress like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere += " OR mastCustname like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere += " OR Email like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere += " OR CTel like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                sSqlWhere += " ) ";
                sWhere += " AND custid IN(" + sSqlWhere + ")";

            }

            #region ɸѡ�ʲ���Ϣ
            string sWhereEqu = "";
            if (!string.IsNullOrEmpty(txtEquipmentName.Text.Trim()))
            {
                sWhereEqu = "EquName like " + StringTool.SqlQ("%" + txtEquipmentName.Text.Trim() + "%");
            }

            if (!string.IsNullOrEmpty(txtEquipmentDir.Text.Trim()))
            {
                if (sWhereEqu == "")
                {
                    sWhereEqu = "a.ListName like " + StringTool.SqlQ("%" + txtEquipmentDir.Text.Trim() + "%");
                }
                else
                {
                    sWhereEqu += " and  a.ListName like " + StringTool.SqlQ("%" + txtEquipmentDir.Text.Trim() + "%");
                }
            }
            DataTable dt = ChangeDealDP.getEquipment(sWhereEqu);
            string changeids = "";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == (dt.Rows.Count - 1))
                    {
                        changeids += "'" + dt.Rows[i]["ChangeID"].ToString().Trim() + "'";
                    }
                    else
                    {
                        changeids += "'" + dt.Rows[i]["ChangeID"].ToString().Trim() + "',";
                    }
                }
            }
            if (changeids != "")
            {
                sWhere += " and a.ID in (" + changeids + ")";
            }
            #endregion

            if (!string.IsNullOrEmpty(txtTitle.Text.Trim()))
                sWhere += " and Subject like " + StringTool.SqlQ("%" + txtTitle.Text.Trim() + "%");
            if (!string.IsNullOrEmpty(txtRegTime.Text.Trim()))
                sWhere += " and ChangeTime>= to_date(" + StringTool.SqlQ(txtRegTime.Text.Trim()) + ",'yyyy-MM-dd HH24:mi:ss')";
            if (!string.IsNullOrEmpty(txtEndTime.Text.Trim()))
                sWhere += " and ChangeTime<= to_date(" + StringTool.SqlQ(txtEndTime.Text.Trim() + " 23:59:59") + ",'yyyy-MM-dd HH24:mi:ss')";

            //��ѯ��ť
            if (icurrent == 2)
            {
                sWhere = "";
                strCustInfo = this.Master.TxtKeyName.Value.Trim().ToString();

                if (strCustInfo != "������������,�ͻ���Ϣ")
                {
                    string sSqlWhere1 = @"select ID from Br_ECustomer where 1=1";

                    sSqlWhere1 += " And (";
                    sSqlWhere1 += " CustName like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                    sSqlWhere1 += " OR Contact like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                    sSqlWhere1 += " OR customcode like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                    sSqlWhere1 += " OR CustAddress like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                    sSqlWhere1 += " OR mastCustname like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                    sSqlWhere1 += " OR Email like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                    sSqlWhere1 += " OR CTel like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                    sSqlWhere1 += " ) ";
                    sWhere += " AND (custid IN (" + sSqlWhere1 + ")";

                    sWhere += "  or  ChangeNo like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%") + ")";
                }
            }

            //���μ���ʱֻ����һ���µ�����
            if (icurrent == 3)
            {
                sWhere = "";
                sWhere += " and ChangeTime>= to_date(" + StringTool.SqlQ(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd")) + ",'yyyy-MM-dd HH24:mi:ss')";
                sWhere += " and ChangeTime<= to_date(" + StringTool.SqlQ(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59") + ",'yyyy-MM-dd HH24:mi:ss')";
                sWhere += " and status=20 ";
            }

            return sWhere;
        }

        #endregion

        protected void HidButton_Click(object sender, EventArgs e)
        {
            icurrent = 1;
            PageLoadQuery();
        }

        private void PageLoadQuery()
        {
            SetParentButtonEvent();
            cpChange.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(LoadData);

            this.Master.KeyValue = "������������,�ͻ���Ϣ";
            hidUserID.Value = ((long)(Session["UserID"])).ToString();

            this.txtFastQuery.Attributes.Add("onclick", "txtFastQueryClear()");

            //��������
            if (Request["svalue"] != null)
            {
                txtCustInfo.Text = Request["svalue"].ToString().Trim();
                staCustInfo = Request["svalue"].ToString().Trim();
            }

            re = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.EquChangeQuery];

            cboStatus.Items.Add(new ListItem("����״̬", "-1"));
            cboStatus.Items.Add(new ListItem("--���ڴ���", ((int)EpowerGlobal.e_FlowStatus.efsHandle).ToString()));
            cboStatus.Items.Add(new ListItem("--��������", ((int)EpowerGlobal.e_FlowStatus.efsEnd).ToString()));
            cboStatus.Items.Add(new ListItem("--������ͣ", ((int)EpowerGlobal.e_FlowStatus.efsStop).ToString()));
            cboStatus.Items.Add(new ListItem("--������ֹ", ((int)EpowerGlobal.e_FlowStatus.efsAbort).ToString()));
            cboStatus.SelectedIndex = 1;

            InitDropSQLwSave();  //��ʼ����ѯ����

            string sQueryBeginDate = "0";
            if (CommonDP.GetConfigValue("Other", "QueryBeginDate") != null)
                sQueryBeginDate = CommonDP.GetConfigValue("Other", "QueryBeginDate").ToString();
            if (sQueryBeginDate == "0")
            {
                txtRegTime.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                staMsgDateBegion = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            }
            else
            {
                txtRegTime.Text = DateTime.Parse(sQueryBeginDate).ToString("yyyy-MM-dd");
                staMsgDateBegion = DateTime.Parse(sQueryBeginDate).ToString("yyyy-MM-dd");
            }
            txtEndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            ////��ѯ������ֵ
            //Control[] arrControl = { Table12 };
            //PageDeal.SetPageQueryParam(arrControl, cpChange, "frm_ChangeQuery");

            #region װ�ظ߼���ѯ����
            if (hidSQLName.Value != "")
            {
                if (hidSQLName.Value != "Temp1")
                {
                    DataTable dt = ChangeDealDP.getCST_ISSUE_Where("frm_ChangeQuery", Session["UserName"].ToString(), hidSQLName.Value.ToString().Trim());

                    if (dt.Rows[0]["SQLText"].ToString().Trim() == dt.Rows[1]["SQLText"].ToString().Trim())
                    {
                        #region ���Ϊԭ����

                        if (dt.Rows[0]["Name"].ToString().Trim() != "Temp1")
                        {
                            //���·��ʴ���
                            ChangeDealDP.updateCST_ISSUE_WhereNums(Session["UserName"].ToString(), dt.Rows[0]["Name"].ToString().Trim());

                            hidSQLName.Value = dt.Rows[0]["Name"].ToString().Trim();
                            SqlWhereShow(dt.Rows[0]["Name"].ToString().Trim());
                            InitDropSQLwSave1(dt.Rows[0]["Name"].ToString().Trim());
                        }

                        if (dt.Rows[1]["Name"].ToString().Trim() != "Temp1")
                        {
                            //���·��ʴ���
                            ChangeDealDP.updateCST_ISSUE_WhereNums(Session["UserName"].ToString(), dt.Rows[1]["Name"].ToString().Trim());

                            hidSQLName.Value = dt.Rows[1]["Name"].ToString().Trim();
                            SqlWhereShow(dt.Rows[1]["Name"].ToString().Trim());
                            InitDropSQLwSave1(dt.Rows[1]["Name"].ToString().Trim());
                        }
                    }
                        #endregion
                    else
                    {
                        #region �����Ϊԭ����
                        hidSQLName.Value = "Temp1";
                        SqlWhereShow("Temp1");
                        this.DropSQLwSave.SelectedIndex = DropSQLwSave.Items.IndexOf(DropSQLwSave.Items.FindByValue("0"));
                        #endregion
                    }
                }
                else
                {
                    hidSQLName.Value = "Temp1";
                    SqlWhereShow("Temp1");
                    try
                    {
                        this.DropSQLwSave.SelectedIndex = DropSQLwSave.Items.IndexOf(DropSQLwSave.Items.FindByValue("0"));
                    }
                    catch
                    { }
                }
            }
            else
            {
                hidSQLName.Value = "Temp1";
            }

            #endregion

            LoadData();

            imgTBSJ.Attributes.Add("onclick", "fPopUpDlg('../Controls/Calendar/calendar.htm',document.all." + txtRegTime.ClientID + ", 'winpop', 234, 261);return false");
            imgEnd.Attributes.Add("onclick", "fPopUpDlg('../Controls/Calendar/calendar.htm',document.all." + txtEndTime.ClientID + ", 'winpop', 234, 261);return false");

            Session["FromUrl"] = "../EquipmentManager/frm_ChangeQuery.aspx";



            //�����ѯ����
            Control[] arrControl1 = { Table12 };
            PageDeal.GetPageQueryParam(arrControl1, cpChange, "frm_ChangeQuery");
        }

        #region  ������ʾ�ֶ�

        public string SetColumnDisplay(string arr)
        {
            if (!string.IsNullOrEmpty(arr))
            {
                for (int i = 3; i <= this.dgProblem.Columns.Count; i++)
                {

                    if (this.dgProblem.Columns.Count < 9)
                        break;
                    else
                        this.dgProblem.Columns.RemoveAt(3);
                }
                DataBing(arr);

            }

            return "";
        }
        #region DataGrid �ֶ�����
        public void DataBing(string columns)
        {

            string[] columnValues = columns.Split(',');

            for (int i = columnValues.Length - 1; i > -1; i--)
            {
                if (columnValues[i] != "")
                {
                    BoundColumn bf = new BoundColumn();
                    bf.HeaderText = PageDeal.GetLanguageValue1(columnValues[i], "�����");
                    bf.DataField = columnValues[i];
                    bf.SortExpression = columnValues[i];
                    bf.ItemStyle.Width = 150;
                    bf.HeaderStyle.Width = 150;
                    bf.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                    if (columnValues[i] == "RegSysDate")
                    {
                        bf.DataFormatString = "{0:g}";
                    }
                    this.dgProblem.Columns.AddAt(3, bf);
                }
            }
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
            LoadData();
        }
    }
}
