
/*******************************************************************
 * ��Ȩ���У������зǷ���Ϣ�������޹�˾
 * ��������̬��ѯ�û��ؼ�

 * 
 * 
 * �����ˣ�����ǰ
 * �������ڣ�2013-05-20
 * 
 * �޸���־��
 * �޸�ʱ�䣺
 * �޸�������
 * 
 * *****************************************************************/
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using EpowerGlobal;
using Epower.DevBase.Organization.SqlDAL;
using System.Xml;
using System.Text;

namespace Epower.ITSM.Web.Controls
{
    public partial class ctrCondition : System.Web.UI.UserControl
    {

        public string sApplicationUrl = Constant.ApplicationPath;     //����·��

        #region �����ѯ��ť��OnClick�¼�
        public event EventHandler mybtnSelectOnClick;

        void btnSelect_OnClick(object sender, EventArgs e)
        {
            if (chkHiddenConditionPanel.Checked)
            {
                trConditionList.Visible = true;
                trConditionButton.Visible = true;
            }

            btnSave_Click(null, null);

            if (mybtnSelectOnClick != null)
                mybtnSelectOnClick(this, new EventArgs()); //�����ѯ��ť��OnClick�¼� 

            if (ddlCondition.SelectedIndex == 0)
            {
                literalConditionFriendlyContent.Text = "{ Ĭ�ϲ�ѯȫ�� }";
            }
            else
                literalConditionFriendlyContent.Text = this.GetFriendlyContent();
        }
        #endregion

        #region ����������ı��¼�
        public event EventHandler mySelectedIndexChanged;

        void ddlCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCondition.SelectedIndex == 0)
                literalConditionFriendlyContent.Text = "{ Ĭ�ϲ�ѯȫ�� }";
            else
                literalConditionFriendlyContent.Text = this.GetFriendlyContent();

            if (mySelectedIndexChanged != null)
                mySelectedIndexChanged(this, new EventArgs()); //����SelectIndexChanged�¼� 
        }
        #endregion

        #region ����

        /// <summary>
        /// ��������(ע���Сд)
        /// </summary>
        public string TableName
        {
            get
            {
                return ViewState[this.ID + "TableName"] == null ? string.Empty : ViewState[this.ID + "TableName"].ToString();
            }
            set
            {
                ViewState[this.ID + "TableName"] = value;
                hidTableName.Value = value;
            }
        }
        /// <summary>
        /// �õ���̬�����ַ��� �� ( Name like'%...%' and  Sex=1) or (Name='...')
        /// </summary>
        public string strCondition
        {
            get
            {
                return GetCondition();
            }
        }

        /// <summary>
        /// �߼���ѯ����Ƿ��?
        /// </summary>
        public Boolean IsOpen
        {
            get
            {
                return hidIsShowAdvancedSearch.Value == "1";
            }
        }

        /// <summary>
        /// ��ѯ�������ı���ʾ��
        /// </summary>
        /// <returns></returns>
        public String GetFriendlyContent()
        {
            //{[����] ���� "�����" ���� [����] ���� "�����"} ���� {[����] ���� "�����" ���� [����] ���� "�����"}

            StringBuilder sbText = new StringBuilder();

            long lngConditionId = long.Parse(ddlCondition.SelectedValue);
            String strConditionValue = Br_ConditionSaveDP.GetConditionContent(lngConditionId);

            DataTable dtCondition = CreateDataTable(strConditionValue);

            if (dtCondition.Rows.Count <= 0) return String.Empty;

            sbText.Append("{ ");
            for (int index = 0; index < dtCondition.Rows.Count; index++)
            {
                DataRow dr = dtCondition.Rows[index];

                String strCondItem = dr["CondItem"].ToString().Trim();
                if (String.IsNullOrEmpty(strCondItem)) continue;

                Int32 intGroupValue = Int32.Parse(dr["GroupValue"].ToString());    // ������
                String strLogicWay = dr["Relation"].ToString().Equals("0") ? "����" : "����";    // �߼���ϵ

                Int32 intOperate = Int32.Parse(dr["Operate"].ToString());    // �ȽϹ�ϵ
                String strOperateChineseName = TranslateOperate(intOperate);    // �ȽϹ�ϵ�Ŀɶ���

                if (intOperate == 2)    // 2 ==> ��...��ͷ
                {
                    sbText.AppendFormat(" [{0}] �� \"{1}\" ��ͷ ", strCondItem, dr["Expression"]);
                }
                else
                {
                    sbText.AppendFormat(" [{0}] {1} \"{2}\" ", strCondItem, strOperateChineseName, dr["Expression"]);
                }

                Int32 intNextIdx = index + 1;
                if (dtCondition.Rows.Count - 1 < intNextIdx)
                {
                    sbText.Append(" } ");
                    break;
                }
                else
                {
                    DataRow drNext = dtCondition.Rows[intNextIdx];
                    Int32 intNextGroupValue = Int32.Parse(drNext["GroupValue"].ToString());    // ��һ������

                    if (intGroupValue == intNextGroupValue)
                    {
                        sbText.AppendFormat(" " + strLogicWay + " ");
                    }
                    else
                    {
                        sbText.Append(" } " + strLogicWay + " { ");
                    }
                }
            }

            return sbText.ToString();
        }

        /// <summary>
        /// �����Ƿ���ʾ�߼���ѯ���
        /// </summary>
        public Boolean SetDisplayMode
        {
            set
            {
                if (value)
                {
                    hidIsShowAdvancedSearch.Value = "1";
                }
                else
                {
                    hidIsShowAdvancedSearch.Value = "0";
                }

            }
        }

        #endregion

        /// <summary>
        /// ����ȽϹ�ϵֵΪ�ɶ���
        /// </summary>
        /// <param name="intOperate">�ȽϹ�ϵֵ</param>
        /// <returns>�ȽϹ�ϵ�Ŀɶ���</returns>
        private String TranslateOperate(Int32 intOperate)
        {
            String strOperateChineseName = String.Empty;
            switch (intOperate)
            {
                case 0:
                    strOperateChineseName = "����";
                    break;
                case 1:
                    strOperateChineseName = "������";
                    break;
                case 2:
                    strOperateChineseName = "��...��ͷ";
                    break;
                case 3:
                    strOperateChineseName = "����";
                    break;
                case 4:
                    strOperateChineseName = "������";
                    break;
                case 5:
                    strOperateChineseName = "����";
                    break;
                case 6:
                    strOperateChineseName = "������";
                    break;
                case 7:
                    strOperateChineseName = "����";
                    break;
                case 8:
                    strOperateChineseName = "���ڵ���";
                    break;
                case 9:
                    strOperateChineseName = "С��";
                    break;
                case 10:
                    strOperateChineseName = "С�ڵ���";
                    break;
            }

            return strOperateChineseName;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            //����ѯ��ť����Click�¼�
            if (mybtnSelectOnClick != null)
            {
                btnSelect.Click += new EventHandler(btnSelect_OnClick);
            }
            //����������ĸı��¼�
            if (mySelectedIndexChanged != null)
            {
                ddlCondition.AutoPostBack = true;
                ddlCondition.SelectedIndexChanged += new EventHandler(ddlCondition_SelectedIndexChanged);
            }

            if (!IsPostBack)
            {
                BindDrop(); //�󶨿��ٲ�ѯ������

                LoadData(); //��ʼ��������

                if (ddlCondition.SelectedIndex == 0)
                {
                    literalConditionFriendlyContent.Text = "{ Ĭ�ϲ�ѯȫ�� }";
                }
            }
        }


        #region �󶨿��ٲ�ѯ������
        /// <summary>
        /// �����ݵ�[�л����]������
        /// </summary>
        private void BindDrop()
        {
            string strWhere = " where UserID = " + HttpContext.Current.Session["UserID"].ToString() + " and TableName=" + StringTool.SqlQ(TableName);
            DataTable dt = Br_ConditionSaveDP.GetNames(strWhere, "");

            ddlCondition.DataSource = dt.DefaultView;
            ddlCondition.DataTextField = "ConditionName";
            ddlCondition.DataValueField = "ID";
            ddlCondition.DataBind();

            ddlCondition.Items.Insert(0, new ListItem("��ѯȫ��", "-1"));
        }
        #endregion

        private void LoadData()
        {
            DataTable dt = CreateNullTable();
            dgCondition.DataSource = dt.DefaultView;
            dgCondition.DataBind();
        }

        /// <summary>
        /// ����ҳ������ѯ����Ҫ���ô˷������°��б���ֹ���ݶ�ʧ

        /// </summary>
        public void Bind()
        {
            DataTable dt = GetDetailItem(true);
            dgCondition.DataSource = dt.DefaultView;
            dgCondition.DataBind();

            if (ddlSelect.SelectedItem != null)
            {
                if (ddlSelect.SelectedIndex != 0)
                    txtConditionName.Value = ddlSelect.SelectedItem.Text;
            }
        }

        #region Grid����¼�
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgCondition_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DropDownList dl = (DropDownList)e.Item.FindControl("cboItems");

                BindCondItemsControl(dl);

                dl.SelectedValue = e.Item.Cells[7].Text.Trim();


                TextBox txt = (TextBox)e.Item.FindControl("txtValue");
                HtmlInputButton bt = (HtmlInputButton)e.Item.FindControl("cmdPop");

                string sCondType = DataBinder.Eval(e.Item.DataItem, "CondType").ToString();
                DropDownList ddl = (DropDownList)e.Item.FindControl("cboOperate");
                string sOperate = DataBinder.Eval(e.Item.DataItem, "Operate").ToString();

                switch (sCondType.Split(",".ToCharArray())[1])
                {
                    case "CHAR":
                        ddl.Items.Clear();
                        ddl.Items.Add(new ListItem("����", "0"));
                        ddl.Items.Add(new ListItem("������", "1"));
                        ddl.Items.Add(new ListItem("��..��ͷ", "2"));
                        ddl.Items.Add(new ListItem("����", "3"));
                        ddl.Items.Add(new ListItem("������", "4"));

                        //������ʾ
                        bt.Style.Value = "visibility:Hidden";
                        if (!IsPostBack)
                        {
                            txt.Attributes.Add("disabled", "false");
                        }
                        //txt.Attributes.Add("disabled", "false");
                        break;
                    case "CLOB":
                        ddl.Items.Clear();
                        ddl.Items.Add(new ListItem("��..��ͷ", "2"));
                        ddl.Items.Add(new ListItem("����", "3"));
                        ddl.Items.Add(new ListItem("������", "4"));

                        //������ʾ
                        bt.Style.Value = "visibility:Hidden";
                        //txt.Attributes.Add("disabled", "false");
                        break;
                    case "CATA":
                        ddl.Items.Clear();
                        ddl.Items.Add(new ListItem("����", "0"));
                        ddl.Items.Add(new ListItem("������", "1"));
                        ddl.Items.Add(new ListItem("����", "5"));
                        ddl.Items.Add(new ListItem("������", "6"));

                        //������ʾ
                        bt.Style.Value = "visibility:visible";
                        txt.Attributes.Add("disabled", "true");
                        break;
                    case "USER":
                        ddl.Items.Clear();
                        ddl.Items.Add(new ListItem("����", "0"));
                        ddl.Items.Add(new ListItem("������", "1"));
                        //������ʾ
                        bt.Style.Value = "visibility:visible";
                        txt.Attributes.Add("disabled", "true");
                        break;
                    case "DEPT":
                        ddl.Items.Clear();
                        ddl.Items.Add(new ListItem("����", "0"));
                        ddl.Items.Add(new ListItem("������", "1"));
                        ddl.Items.Add(new ListItem("����", "5"));
                        ddl.Items.Add(new ListItem("������", "6"));
                        //������ʾ
                        bt.Style.Value = "visibility:visible";
                        txt.Attributes.Add("disabled", "true");
                        break;
                    case "DATE":
                        ddl.Items.Clear();
                        ddl.Items.Add(new ListItem("����", "0"));
                        ddl.Items.Add(new ListItem("����", "7"));
                        ddl.Items.Add(new ListItem("���ڵ���", "8"));
                        ddl.Items.Add(new ListItem("С��", "9"));
                        ddl.Items.Add(new ListItem("С�ڵ���", "10"));


                        //������ʾ
                        bt.Style.Value = "visibility:visible";
                        txt.Attributes.Add("disabled", "true");
                        break;
                    default:
                        txt.Attributes.Add("disabled", "true");
                        break;
                }
                ddl.SelectedValue = sOperate;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgCondition_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            DataTable dt = GetDetailItem(true);
            bool hasDeleted = false;
            if (e.CommandName == "Delete")
            {
                if (dt.Rows.Count > 0 && e.Item.ItemIndex < dt.Rows.Count)
                    dt.Rows.RemoveAt(e.Item.ItemIndex);

                hasDeleted = true;

                if (ddlCondition.SelectedIndex <= 0)
                {
                    if (dt.Rows.Count <= 0)
                    {
                        txtConditionName.Value = String.Empty;
                    }
                }
            }

            if (hasDeleted == true)
            {
                dgCondition.DataSource = dt.DefaultView;
                dgCondition.DataBind();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdOK_Click(object sender, EventArgs e)
        {
            DataTable dt = GetDetailItem(false);
            DataRow dr = dt.NewRow();
            //�趨Ĭ��ֵ

            dr["ID"] = (dt.Rows.Count + 1).ToString();
            dr["Relation"] = 0;
            dr["GroupValue"] = 0;
            dr["CondItem"] = "";
            dr["CondType"] = ",CHAR";
            dr["Operate"] = 0;
            dr["Expression"] = "";
            dr["Tag"] = "";
            dt.Rows.Add(dr);
            dgCondition.DataSource = dt.DefaultView;
            dgCondition.DataBind();

            if (ddlCondition.SelectedIndex == 0)
            {
                txtConditionName.Value = "���һ�εĲ�ѯ��¼";
            }
        }

        private void BindCondItemsControl(DropDownList ddl)
        {
            DataTable dt = new DataTable("CondItems");
            dt.Columns.Add("ID");
            dt.Columns.Add("Text");

            DataRow drFirst = dt.NewRow();
            drFirst["ID"] = ",CHAR";
            drFirst["Text"] = "";
            dt.Rows.Add(drFirst);


            #region ��ȡ��ӦTableName�����õĶ�̬��ѯ�ֶ��б�

            string strWhere = " where TableName=" + StringTool.SqlQ(TableName);
            string strOrder = " order by ID ";
            DataTable dtCon = Br_ConditionDP.GetDataTable(strWhere, strOrder);
            if (dtCon != null && dtCon.Rows.Count > 0)
            {
                for (int i = 0; i < dtCon.Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();

                    dr["ID"] = dtCon.Rows[i]["ColumnName"].ToString() + "," + dtCon.Rows[i]["ColType"].ToString();
                    dr["Text"] = dtCon.Rows[i]["ColRemark"].ToString();
                    dt.Rows.Add(dr);
                }
            }
            #endregion

            ddl.DataTextField = "Text";
            ddl.DataValueField = "ID";
            ddl.DataSource = dt.DefaultView;
            ddl.DataBind();

        }

        /// <summary>
        /// ���� datatable�ṹ
        /// </summary>
        /// <returns></returns>
        private DataTable CreateNullTable()
        {
            DataTable dt = new DataTable("ConditionRule");
            dt.Columns.Add("id");
            dt.Columns.Add("Relation");
            dt.Columns.Add("GroupValue");
            dt.Columns.Add("CondItem");
            dt.Columns.Add("CondType");
            dt.Columns.Add("Operate");
            dt.Columns.Add("Expression");
            dt.Columns.Add("Tag");

            #region lsj 2013.02.27 �޸�(Ĭ����ʾһ����ѯ����)
            if (!this.IsPostBack)
            {
                DataRow dr = dt.NewRow();
                //�趨Ĭ��ֵ


                dr["ID"] = (dt.Rows.Count + 1).ToString();
                dr["Relation"] = 0;
                dr["GroupValue"] = 0;
                dr["CondItem"] = "";
                dr["CondType"] = ",CHAR";
                dr["Operate"] = 0;
                dr["Expression"] = "";
                dr["Tag"] = "";
                dt.Rows.Add(dr);
            }
            #endregion

            return dt;
        }
        #endregion

        #region ��ȡ��grid �� datatable
        /// <summary>
        /// ��ȡ��grid �� datatable
        /// </summary>
        /// <param name="isAll"></param>
        /// <returns></returns>
        private DataTable GetDetailItem(bool isAll)
        {
            DataTable dt = CreateNullTable();
            DataRow dr;

            int id = 1;

            if (dgCondition.Items.Count > 0)
            {
                foreach (DataGridItem row in dgCondition.Controls[0].Controls)
                {
                    if (row.ItemType == ListItemType.Item || row.ItemType == ListItemType.AlternatingItem)
                    {
                        string sID = id.ToString();
                        id++;
                        //���� or ����

                        string sRelation = ((DropDownList)row.FindControl("cboRelation")).SelectedItem.Value;
                        //��

                        string sGroupValue = ((CtrFlowNumeric)row.FindControl("CtrFlowGroupValue")).Value.ToString();
                        //�ֶ�����
                        string sCondItem = ((DropDownList)row.FindControl("cboItems")).SelectedItem.Text;

                        sCondItem = sCondItem.Trim();
                        if (String.IsNullOrEmpty(sCondItem)) continue;

                        //�ֶμ��ֶ�����

                        string sCondType = ((DropDownList)row.FindControl("cboItems")).SelectedItem.Value;
                        //�Ƚ�����
                        string sOperate = ((DropDownList)row.FindControl("cboOperate")).SelectedItem.Value;
                        //����ֵ

                        string sExpression = ((TextBox)row.FindControl("txtValue")).Text;
                        string sHidExpression = ((HtmlInputHidden)row.FindControl("hidValue")).Value;

                        string sTag = ((HtmlInputHidden)row.FindControl("hidTag")).Value;

                        sExpression = (sExpression.Trim() == "") ? sHidExpression.Trim() : sExpression.Trim();

                        string sValue = "";

                        switch (sCondType.Split(",".ToCharArray())[1])
                        {
                            case "CHAR":
                                sValue = sExpression;
                                break;
                            case "CLOB":
                                sValue = sExpression;
                                break;
                            case "CATA":
                                sValue = sTag; //ȡ����ѡ����ID��ֵ
                                break;
                            case "USER":
                                sValue = sTag;
                                break;
                            case "DEPT":
                                sValue = sTag;
                                break;
                            case "DATE":
                                sValue = sTag;
                                break;
                            default:
                                break;
                        }

                        dr = dt.NewRow();

                        if (isAll == true || sValue.Length > 0)
                        {
                            dr["id"] = sID.Trim();
                            dr["Relation"] = sRelation;
                            dr["GroupValue"] = sGroupValue;
                            dr["CondItem"] = sCondItem;
                            dr["CondType"] = sCondType;
                            dr["Operate"] = sOperate;
                            dr["Expression"] = sExpression;
                            dr["Tag"] = sValue;

                            dt.Rows.Add(dr);
                        }
                    }
                }
            }

            return dt;
        }
        #endregion

        #region �õ���̬�����ַ��� �� ( and 1=1 )
        /// <summary>
        /// �õ���̬�����ַ��� �� ( Name like'%...%' and  Sex=1) or (Name='...')
        /// </summary>
        /// <returns></returns>
        private string GetCondition()
        {
            string strWhere = "";

            DataTable dt = GetDetailItem(false);//�õ���ǰ���õ������б�

            DataRow[] drarry = dt.Select("", " GroupValue asc"); //��������������õ�������

            int GroupValue = 0; //��¼��ǰ����ID
            int j = 0;  //��¼�Ƿ��ǵ�ǰ�����µĵ�һ����¼
            int beforeRelation = -1;//ǰһ���߼���ϵ

            #region ѭ��dt ƴ���ַ���

            if (drarry != null && drarry.Length > 0)
            {
                for (int i = 0; i < drarry.Length; i++)
                {
                    //���� or ����
                    string sRelation = drarry[i]["Relation"].ToString();
                    //��
                    string sGroupValue = drarry[i]["GroupValue"].ToString();
                    //�ֶ�����
                    string sCondItem = drarry[i]["CondItem"].ToString();
                    //�ֶμ��ֶ�����
                    string sCondType = drarry[i]["CondType"].ToString();
                    //�Ƚ�����
                    string sOperate = drarry[i]["Operate"].ToString();
                    //����ֵ                    
                    string sValue = drarry[i]["Tag"].ToString();

                    string[] arr = sCondType.Split(',');

                    //��ʼ����ǰ����ID
                    if (i == 0)
                        GroupValue = int.Parse(sGroupValue == "" ? "0" : sGroupValue);


                    #region ���ݷ���ϲ�����

                    //����ֶκͱȽ�ֵ��Ϊ��
                    if (arr[0] != string.Empty && sValue != string.Empty)
                    {
                        if (i == 0) strWhere += " ( ";

                        //�ж��Ƿ���ͬһ������
                        if (GroupValue == int.Parse(sGroupValue == "" ? "0" : sGroupValue))
                        {
                            #region ��ͬһ�������� ������ 2013-03-22 �޸�
                            if (beforeRelation >= 0)
                            {
                                //��Ӻ���һ���������߼���ϵ
                                if ((e_fm_RELATION_TYPE)beforeRelation == e_fm_RELATION_TYPE.fmConditionAnd)
                                    strWhere += " and ";
                                else
                                    strWhere += " or ";
                            }
                            #endregion

                            #region
                            ////�ж��Ƿ�Ϊ�˷����µĵ�һ����¼
                            //if (j == 0)
                            //{
                            //    if ((e_fm_RELATION_TYPE)(int.Parse(sRelation)) == e_fm_RELATION_TYPE.fmConditionAnd)
                            //        strWhere += " and (";
                            //    else
                            //        strWhere += " or (";
                            //}
                            //else
                            //{
                            //    if ((e_fm_RELATION_TYPE)(int.Parse(sRelation)) == e_fm_RELATION_TYPE.fmConditionAnd)
                            //        strWhere += " and ";
                            //    else
                            //        strWhere += " or ";
                            //}
                            //j++;
                            #endregion
                        }
                        else
                        {
                            #region ����ͬһ�������� ������ 2013-03-22 �޸�
                            GroupValue = int.Parse(sGroupValue == "" ? "0" : sGroupValue);
                            strWhere += " ) ";

                            //��Ӻ���һ���������߼���ϵ
                            if ((e_fm_RELATION_TYPE)beforeRelation == e_fm_RELATION_TYPE.fmConditionAnd)
                                strWhere += " and ";
                            else
                                strWhere += " or ";

                            strWhere += " ( ";
                            #endregion

                            #region
                            //j = 0;
                            //strWhere += " ) ";

                            //if ((e_fm_RELATION_TYPE)(int.Parse(sRelation)) == e_fm_RELATION_TYPE.fmConditionAnd)
                            //    strWhere += " and (";
                            //else
                            //    strWhere += " or (";

                            //j++;
                            #endregion
                        }
                        //�õ������ַ���
                        strWhere += GetsOperate(int.Parse(sOperate), arr[1], arr[0], sValue);
                        beforeRelation = int.Parse(sRelation);
                    }
                    #endregion

                }
            }
            #endregion

            //�����Ҫ���Ͻ�β������
            if (strWhere.Trim() != "")
                strWhere += " )";

            return strWhere;

        }
        #endregion

        #region ���ݱȽϷ������ض�Ӧ�������ַ�
        /// <summary>
        /// ���ݱȽϷ������ض�Ӧ�������ַ�
        /// </summary>
        /// <param name="sOperate"></param>
        /// <param name="CondType"></param>
        /// <param name="Column"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetsOperate(int Operate, string CondType, string Column, string value)
        {
            string str = "";
            string FullID = "";

            switch (Operate)
            {
                case 0:
                    switch (CondType)
                    {
                        case "CHAR": //����
                            str = Column + " = " + StringTool.SqlQ(value);
                            break;
                        case "CATA"://����
                            str = Column + " = " + value;
                            break;
                        case "USER":
                            str = Column + " = " + value;
                            break;
                        case "DEPT"://����
                            str = Column + " = " + value;
                            break;
                        case "DATE"://����
                            str = " nvl(" + Column + ",sysdate) >= to_date(" + StringTool.SqlQ(value + " 00:00:00") + ",'yyyy-MM-dd HH24:mi:ss') ";
                            str = str + " and nvl(" + Column + ",sysdate) <= to_date(" + StringTool.SqlQ(value + " 23:59:59") + ",'yyyy-MM-dd HH24:mi:ss')";
                            break;
                    }
                    break;
                case 1:
                    switch (CondType)
                    {
                        case "CHAR"://������

                            str = " (" + Column + " != " + StringTool.SqlQ(value) + " or " + Column + " is null) ";
                            break;
                        case "CATA"://������

                            str = " (" + Column + " != " + value + " or " + Column + " is null) ";
                            break;
                        case "USER"://������

                            str = " (" + Column + " != " + value + " or " + Column + " is null) ";
                            break;
                        case "DEPT"://������

                            str = " (" + Column + " != " + value + " or " + Column + " is null) ";
                            break;
                    }
                    break;
                case 2:
                    switch (CondType)
                    {
                        case "CHAR"://��...��ͷ

                            str = Column + " like " + StringTool.SqlQ(value + "%");
                            break;
                        case "CLOB"://��...��ͷ

                            str = Column + " like " + StringTool.SqlQ(value + "%");
                            break;
                    }
                    break;
                case 3:
                    switch (CondType)
                    {
                        case "CHAR"://����
                            str = Column + " like " + StringTool.SqlQ("%" + value + "%");
                            break;
                        case "CLOB"://����
                            str = Column + " like " + StringTool.SqlQ("%" + value + "%");
                            break;
                    }
                    break;
                case 4:
                    switch (CondType)
                    {
                        case "CHAR"://������

                            str = Column + " not like " + StringTool.SqlQ("%" + value + "%");
                            break;
                        case "CLOB"://������

                            str = Column + " not like " + StringTool.SqlQ("%" + value + "%");
                            break;
                    }
                    break;
                case 5:
                    switch (CondType)
                    {
                        case "CATA"://����
                            FullID = CatalogDP.GetCatalogFullID(long.Parse(value));
                            str = "nvl(" + Column + ",0) in ( select CatalogID from es_catalog where FullID like " + StringTool.SqlQ(FullID + "%") + ")";
                            break;
                        case "DEPT"://����
                            FullID = DeptDP.GetDeptFullID(long.Parse(value));
                            str = "nvl(" + Column + ",0) in ( select deptid from ts_dept where FullID like " + StringTool.SqlQ(FullID + "%") + ")";
                            break;

                    }
                    break;
                case 6:
                    switch (CondType)
                    {
                        case "CATA"://������

                            FullID = CatalogDP.GetCatalogFullID(long.Parse(value));
                            str = "nvl(" + Column + ",0) not in ( select CatalogID from es_catalog where FullID like " + StringTool.SqlQ(FullID + "%") + ")";
                            break;
                        case "DEPT"://������

                            FullID = DeptDP.GetDeptFullID(long.Parse(value));
                            str = "nvl(" + Column + ",0) not in ( select deptid from ts_dept where FullID like " + StringTool.SqlQ(FullID + "%") + ")";
                            break;
                    }
                    break;
                case 7:
                    switch (CondType)
                    {
                        case "DATE"://����
                            str = " nvl(" + Column + ",sysdate) > to_date(" + StringTool.SqlQ(value + " 00:00:00") + ",'yyyy-MM-dd HH24:mi:ss')";
                            break;
                    }
                    break;
                case 8:
                    switch (CondType)
                    {
                        case "DATE"://���ڵ���
                            str = " nvl(" + Column + ",sysdate) >= to_date(" + StringTool.SqlQ(value + " 00:00:00") + ",'yyyy-MM-dd HH24:mi:ss') ";
                            break;
                    }
                    break;
                case 9:
                    switch (CondType)
                    {
                        case "DATE"://С��
                            str = "nvl(" + Column + ",sysdate) < to_date(" + StringTool.SqlQ(value + " 23:59:59") + ",'yyyy-MM-dd HH24:mi:ss')";
                            break;
                    }
                    break;
                case 10:
                    switch (CondType)
                    {
                        case "DATE"://С�ڵ���
                            str = "nvl(" + Column + ",sysdate) <= to_date(" + StringTool.SqlQ(value + " 23:59:59") + ",'yyyy-MM-dd HH24:mi:ss')";
                            break;
                    }
                    break;
                default:
                    break;
            }

            return str;
        }
        #endregion

        #region ��dtת��XML�ַ���
        /// <summary>
        /// ��dtת��XML�ַ���
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string GetSchemaXml(DataTable dt)
        {
            if (dt.Rows.Count == 0)
                return "";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(@"<Conditions></Conditions>");
            int i = 0;
            foreach (DataRow row in dt.Rows)
            {

                XmlElement xmlEle = xmlDoc.CreateElement("Condition");
                xmlEle.SetAttribute("ID", row["ID"].ToString().Trim());
                xmlEle.SetAttribute("Relation", row["Relation"].ToString().Trim());
                xmlEle.SetAttribute("GroupValue", row["GroupValue"].ToString().Trim());
                xmlEle.SetAttribute("CondItem", row["CondItem"].ToString().Trim());
                xmlEle.SetAttribute("CondType", row["CondType"].ToString().Trim());
                xmlEle.SetAttribute("Operate", row["Operate"].ToString().Trim());
                xmlEle.SetAttribute("Expression", row["Expression"].ToString().Trim());
                xmlEle.SetAttribute("Tag", row["Tag"].ToString().Trim());
                xmlDoc.DocumentElement.AppendChild(xmlEle);
            }
            return xmlDoc.InnerXml;

        }
        #endregion

        #region ��xml�ַ���ת��dt
        /// <summary>
        /// ����DATATABLE
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private DataTable CreateDataTable(string s)
        {
            DataTable tab = CreateNullTable();

            if (s != "")
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(s);

                object[] values = new object[8];

                XmlNodeList ns = xmldoc.DocumentElement.SelectNodes("Condition");

                foreach (XmlNode n in ns)
                {
                    values[0] = (object)n.Attributes["ID"] == null ? "" : n.Attributes["ID"].Value;
                    values[1] = (object)n.Attributes["Relation"] == null ? "" : n.Attributes["Relation"].Value;
                    values[2] = (object)n.Attributes["GroupValue"] == null ? "" : n.Attributes["GroupValue"].Value;
                    values[3] = (object)n.Attributes["CondItem"] == null ? "" : n.Attributes["CondItem"].Value;
                    values[4] = (object)n.Attributes["CondType"] == null ? "" : n.Attributes["CondType"].Value;
                    values[5] = (object)n.Attributes["Operate"] == null ? "" : n.Attributes["Operate"].Value;
                    values[6] = (object)n.Attributes["Expression"] == null ? "" : n.Attributes["Expression"].Value;
                    values[7] = (object)n.Attributes["Tag"] == null ? "" : n.Attributes["Tag"].Value;
                    tab.Rows.Add(values);

                }
            }

            return tab;
        }
        #endregion

        #region �ı��������ֵʱ����
        /// <summary>
        /// �ı��������ֵʱ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ddlSelectChanged()
        {
            string ConditionName = ddlCondition.SelectedItem.Text; //������

            long lngConditionId = long.Parse(ddlCondition.SelectedItem.Value);
            string Condition = Br_ConditionSaveDP.GetConditionContent(lngConditionId);    // ����XML��


            //�ж�����XML���Ƿ�Ϊ��

            if (string.IsNullOrEmpty(Condition))
            {
                //btnDelete.Visible = false;
                if (ddlCondition.SelectedIndex > 0)
                    txtConditionName.Value = ConditionName;
                else
                    txtConditionName.Value = String.Empty;
                LoadData();
                return;
            }

            //btnDelete.Visible = true;

            txtConditionName.Value = ConditionName;

            //���ݵõ�������XML�� ���°�DataGrid
            DataTable dt = CreateDataTable(Condition);
            dgCondition.DataSource = dt.DefaultView;
            dgCondition.DataBind();

        }
        #endregion

        #region ���ò�ѯ����
        /// <summary>
        /// ���ò�ѯ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReset_Click(object sender, EventArgs e)
        {
            //btnDelete.Visible = false;

            LoadData();
            //txtConditionName.Value = "";
            if (ddlCondition.SelectedIndex == 0)
                txtConditionName.Value = String.Empty;
            else { literalConditionFriendlyContent.Text = String.Empty; }
            //ddlCondition.SelectedIndex = 0;
        }
        #endregion

        #region �����ѯ����
        /// <summary>
        /// �����ѯ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {

            long UserID = long.Parse(HttpContext.Current.Session["UserID"].ToString());

            //��ȡ��������
            string ConditionName = txtConditionName.Value.Trim();
            //�����������Ϊ�գ�ֱ�ӷ���

            if (string.IsNullOrEmpty(ConditionName))
            {
                //���°󶨷�ֹtext�ؼ�ֵ��ʧ

                Bind();

                if (ddlCondition.SelectedIndex > 0)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "",
                    "<script>alert('��ѯ�������Ʋ���Ϊ��');</script>");
                }

                return;
            }

            //��ȡ����XML��

            DataTable dt = GetDetailItem(false);

            //if (dt.Rows.Count <= 0)
            //{
            //    //���°󶨷�ֹtext�ؼ�ֵ��ʧ

            //    Bind();
            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "",
            //        "<script>alert('�������Ϊ��');</script>");
            //    return;
            //}


            string Condition = GetSchemaXml(dt);

            Br_ConditionSaveDP ee = new Br_ConditionSaveDP();
            //�ж��Ƿ��Ѿ����ڴ���������

            ee = ee.GetReCorded(UserID, ConditionName, TableName);
            ee.UserID = UserID;
            ee.ConditionName = ConditionName;
            ee.TableName = TableName;
            ee.Condition = Condition;

            if (ee != null && ee.ID > 0)
            {
                ee.UpdateRecorded(ee);
            }
            else
            {
                ee.InsertRecorded(ee);
            }

            //���°��������ֵ

            BindDrop();
            //����������ѡȡ��ֵ

            ddlCondition.SelectedIndex = ddlCondition.Items.IndexOf(ddlCondition.Items.FindByText(ConditionName));
            //btnDelete.Visible = true;
            //���°󶨷�ֹtext�ؼ�ֵ��ʧ

            Bind();

        }
        #endregion

        #region ɾ������Ķ�̬����

        /// <summary>
        /// ɾ������Ķ�̬����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (ddlCondition.SelectedIndex == 0)
            {
                PageTool.MsgBox(this.Page, "�ò�ѯ����ɾ��");
                return;
            }

            //ɾ������Ķ�̬����

            long lngConditionId = long.Parse(ddlCondition.SelectedItem.Value);
            //long UserID = long.Parse(HttpContext.Current.Session["UserID"].ToString());
            Br_ConditionSaveDP ee = new Br_ConditionSaveDP();
            //ee.DeleteRecorded(UserID, ConditionName, TableName);
            ee.DeleteRecorded(lngConditionId);

            //��������
            LoadData();
            BindDrop();
            txtConditionName.Value = "";
            ddlCondition.SelectedIndex = 0;

            ddlCondition_SelectedIndexChanged(null, null);
        }
        #endregion




        /// <summary>
        /// �÷��������ҳ���϶�̬������ݵ���������ύ���������
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            /*
             * �÷��������ҳ���϶�̬������ݵ���������ύ���������
             * 
             * �����: ������
             */
            //String[] arrVal = new String[] { "����", "������", "��..��ͷ", "����", "������", "����", "������", "����", "���ڵ���", "С��", "С�ڵ���" };
            String[] arrVal = new String[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };

            foreach (DataGridItem item in this.dgCondition.Items)
            {
                DropDownList ddl = item.FindControl("cboOperate") as DropDownList;

                if (ddl != null)
                {
                    foreach (String val in arrVal)
                    {
                        Page.ClientScript.RegisterForEventValidation(ddl.UniqueID, val);
                    }
                }
            }

            base.Render(writer);
        }
    }
}