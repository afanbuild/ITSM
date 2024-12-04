/*******************************************************************
 * ��Ȩ���У�
 * Description�����̷��������б�ؼ�
 * 
 * 
 * Create By  ��
 * Create Date��
 * *****************************************************************/
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
using Epower.ITSM.Base;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class ctrFlowEquCategory : System.Web.UI.UserControl
    {

        #region ����������
        private string sValue = "";

        private bool blnMust = false;
        private long lngRootID = 0;
        private string strMessage = "";
        private long lngCatelogID = 0;
        private int iIndex = 0;
        private bool blnRemark = false;
        #endregion


        #region ���Զ���

        /// <summary>
        /// �Ƿ�һ��Ҫ����
        /// </summary>
        public bool MustInput
        {
            set
            {
                blnMust = value;

            }
            get { return blnMust; }
        }

        public bool ShowRemark
        {
            set
            {
                blnRemark = value;
            }
        }

        /// <summary>
        /// ���
        /// </summary>
        public Unit Width
        {
            set
            {
                if (value != null)
                {
                    ddlCate1.Width = value;
                }
                else
                {
                    ddlCate1.Width = Unit.Parse("70%");
                }
            }
        }


        /// <summary>
        /// ����δ����ʱ����ʾ��Ϣ
        /// </summary>
        public string TextToolTip
        {
            set { strMessage = value; }
        }

        /// <summary>
        /// �����ؼ��Ŀͻ���ID
        /// </summary>
        public string ClientID
        {
            get { return ddlCate1.ClientID; }
        }

        /// <summary>
        /// ������ڵ�
        /// </summary>
        public long RootID
        {
            get { return lngRootID; }
            set
            {
                lngRootID = value;

                SetDboControlValue(ref ddlCate1, lngRootID, lngCatelogID);
                //��ʱdropDownList �ؼ�ʱ�Ĵ���
                if (ddlCate1.Items.Count > 0)
                    labCate1.Text = ddlCate1.SelectedItem.Text.Replace(System.Web.HttpUtility.HtmlDecode("&nbsp;"), "");


                if (ContralState != eOA_FlowControlState.eNormal)
                {
                    labCate1.Visible = true;
                    ddlCate1.Visible = false;
                    rWarning.Visible = false;
                    if (ContralState == eOA_FlowControlState.eHidden)
                    {
                        labCate1.Text = "--";
                        lblMessage.Visible = false;
                    }
                }
            }
        }

        /// <summary>
        /// ��ǰѡ��ڵ�Index
        /// </summary>
        public int CurIndex
        {
            get { return iIndex; }
            set { iIndex = value; }
        }

        /// <summary>
        /// ����ֻ��״̬ʱ�ı�ǩ��ɫ
        /// </summary>
        public Color ForColor
        {
            set
            {
                labCate1.ForeColor = value;
                ddlCate1.ForeColor = value;
            }
        }


        /// <summary>
        /// �ؼ�״̬
        /// </summary>
        public eOA_FlowControlState ContralState
        {
            get
            {
                if (ViewState["eOA_FlowControlState"] != null)
                    return (eOA_FlowControlState)ViewState["eOA_FlowControlState"];
                else
                    return eOA_FlowControlState.eNormal;
            }
            set { ViewState["eOA_FlowControlState"] = value; }
        }

        #region ��� �ؼ��� value ID ֵ
        /// <summary>
        /// ��� �ؼ��� value ID ֵ
        /// </summary>
        public long CatelogID
        {
            get
            {
                #region dropDownlist �ؼ�
                //dropDownlist �ؼ�
                if (ddlCate1.Items.Count > 0)
                    return long.Parse(ddlCate1.SelectedItem.Value);
                else
                    return -1;
                #endregion

            }
            set
            {
                lngCatelogID = value;
                hidCateID.Value = Convert.ToString(value);

                #region dropDownList�ؼ�
                //dropDownList�ؼ�
                if (ddlCate1.Items.Count == 0)
                {
                    SetDboControlValue(ref ddlCate1, lngRootID, lngCatelogID);
                }
                ddlCate1.SelectedIndex = ddlCate1.Items.IndexOf(ddlCate1.Items.FindByValue(value.ToString()));
                #endregion

                lblMessage.Text = "";
            }
        }

        #endregion 

        #region ��ÿؼ���  ValueText ֵ
        /// <summary>
        /// ��ÿؼ���  ValueText ֵ
        /// </summary>
        public string CatelogValue
        {
            get
            {
                if (ddlCate1.Items.Count > 0)
                    return ddlCate1.SelectedItem.Text.Trim().Replace(System.Web.HttpUtility.HtmlDecode("&#160;"), "").Replace(System.Web.HttpUtility.HtmlDecode("&nbsp;"), "");
                else
                    return string.Empty;

            }
            set
            {
                sValue = value;
                ddlCate1.SelectedIndex = ddlCate1.Items.IndexOf(ddlCate1.Items.FindByText(value));
            }
        }
        #endregion 

        /// <summary>
        /// ����ʱ�ͻ��˽ű�
        /// </summary>
        public string OnChangeScript
        {
            set { ViewState["OnChangeScript"] = value; }
            get { return ViewState["OnChangeScript"] == null ? string.Empty : ViewState["OnChangeScript"].ToString(); }
        }

        #endregion

        public event EventHandler mySelectedIndexChanged;

        void DrpDictionary_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mySelectedIndexChanged != null)

                mySelectedIndexChanged(this, new EventArgs()); //����SelectIndexChanged�¼� 
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //ddlCate1.Attributes.Add("onchange", "SelecteChanged()"); 
            if (mySelectedIndexChanged != null)
            {
                ddlCate1.AutoPostBack = true;
                ddlCate1.SelectedIndexChanged += new EventHandler(DrpDictionary_SelectedIndexChanged);
            }
            if (blnRemark)
            {
                lblMessage.Visible = true;
            }

            if (Page.IsPostBack == false)
            {
                if (lngRootID != 0)
                {
                    //û�����ø����� �����κ�����
                    #region
                    //������ DropDownList �ǵĴ���
                    if (ddlCate1.Items.Count > 0)
                        labCate1.Text = ddlCate1.SelectedItem.Text.Replace(System.Web.HttpUtility.HtmlDecode("&nbsp;"), "");

                    if (blnMust == true)
                    {
                        //��ӹ���У���������

                        string strThisMsg = "";
                        strThisMsg = ddlCate1.ClientID + ">" + strMessage + ">" + lngRootID.ToString();
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), ddlCate1.ClientID, "<script>if(typeof(sarrvalidlist) == 'undefined'){ var sarrvalidlist = new Array(1);}  sarrvalidlist[0] = sarrvalidlist[0] + '|' + '" + strThisMsg + "';</script>");
                    }
                    #endregion

                    if (ContralState != eOA_FlowControlState.eNormal)
                    {
                        labCate1.Visible = true;
                        ddlCate1.Visible = false;
                        rWarning.Visible = false;
                        if (ContralState == eOA_FlowControlState.eHidden)
                        {
                            labCate1.Text = "--";
                            lblMessage.Visible = false;
                        }
                    }

                }
              
                if (OnChangeScript.Length > 0)
                {
                    ddlCate1.Attributes.Add("onchange", "CateSelecteChanged(this);" + OnChangeScript);
                }
                else
                {
                    ddlCate1.Attributes.Add("onchange", "CateSelecteChanged(this);");
                }
            }
            if (blnMust == false)
            {
                rWarning.Visible = false;
            }
        }


        #region ���������б�ֵ SetDboControlValue
        /// <summary>
        /// ���������б�ֵ
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="lngRootID"></param>
        /// <param name="lngValue"></param>
        private void SetDboControlValue(ref DropDownList ddl, long lngRootID, long lngValue)
        {
            ArrayList arrValues = new ArrayList();
            ArrayList arrTexts = new ArrayList();

            SetArrListValue(lngRootID, ref arrTexts, ref arrValues);
            ddl.Items.Clear();

            for (int i = 0; i < arrValues.Count; i++)
            {
                ddl.Items.Add(new ListItem(arrTexts[i].ToString(), arrValues[i].ToString()));
            }
            //���õ�ǰֵ
            for (int i = 0; i < ddl.Items.Count; i++)
            {
                if (CurIndex != 0)
                {
                    ddl.SelectedIndex = CurIndex;
                }
                if (ddl.Items[i].Value == lngValue.ToString())
                {
                    ddl.SelectedIndex = i;
                    break;
                }
                if (sValue != string.Empty && ddl.Items[i].Text.Trim() == sValue)
                {
                    ddl.SelectedIndex = i;
                    break;
                }
            }

            lblMessage.Text = "";
        }
        #endregion

        #region �����б� SetArrListValue
        /// <summary>
        /// �����б�
        /// </summary>
        /// <param name="lngRootID"></param>
        /// <param name="arrTexts"></param>
        /// <param name="arrValues"></param>
        private void SetArrListValue(long lngRootID, ref ArrayList arrTexts, ref ArrayList arrValues)
        {
            DataTable dt = Equ_SubjectDP.GetSubjects();
            bool blnHasRoot = false;
            if (dt == null)
                return;
            if (dt.Rows.Count > 0)
            {
                //�Ҹ��ڵ�
                foreach (DataRow row in dt.Rows)
                {
                    if (row["catalogid"].ToString() == lngRootID.ToString())
                    {
                        //arrTexts.Add(row["CatalogName"].ToString());
                        arrTexts.Add("");
                        arrValues.Add(lngRootID.ToString());
                        blnHasRoot = true;
                        break;
                    }
                }
            }
            if (blnHasRoot == true)
            {
                //�ݹ�����¼�
                AddSubArrList(dt, lngRootID, 1, ref arrTexts, ref arrValues);
            }
        }
        #endregion

        #region �������б� AddSubArrList
        /// <summary>
        /// �������б� 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="lngRootID"></param>
        /// <param name="Layer"></param>
        /// <param name="arrTexts"></param>
        /// <param name="arrValues"></param>
        private void AddSubArrList(DataTable dt, long lngRootID, int Layer, ref ArrayList arrTexts, ref ArrayList arrValues)
        {
            foreach (DataRow row in dt.Rows)
            {
                if (row["parentid"].ToString() == lngRootID.ToString())
                {
                    arrTexts.Add(PriLongEmptyString(Layer * 4) + row["CataLogName"].ToString());
                    arrValues.Add(row["catalogid"].ToString());
                    AddSubArrList(dt, long.Parse(row["catalogID"].ToString()), Layer + 1, ref arrTexts, ref arrValues);
                }
            }
        }
        #endregion

        #region �ӿմ� PriLongEmptyString
        /// <summary>
        /// �ӿմ�
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        private string PriLongEmptyString(int len)
        {
            string str = "";
            for (int i = 0; i < len; i++)
            {
                str += System.Web.HttpUtility.HtmlDecode("&nbsp;");
            }
            return str;
        }
        #endregion


    }
}