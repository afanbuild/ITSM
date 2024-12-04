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

namespace Epower.ITSM.Web.Controls
{
    public partial class ctrFlowCataDropListNew : System.Web.UI.UserControl
    {

        #region ����������
        public string sApplicationUrl = Constant.ApplicationPath; 
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

        private bool blnPoint = false;
        /// <summary>
        /// �Ƿ�ֻ��ѡҶ�ڵ�
        /// </summary>
        public bool IsPoint
        {
            set
            {
                blnPoint = value;
                if (value)
                    HidIsPoint.Value = "1";
                else
                    HidIsPoint.Value = "0";

            }
            get
            {
                return blnPoint;
            }
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
                HidRootID.Value = value.ToString();

                SetDboControlValue(ref ddlCate1, ref RadioCate1, lngRootID, lngCatelogID);
                if (ddlCate1.Items.Count > 0)
                {
                    labCata1.Text = ddlCate1.SelectedItem.Text.Replace(System.Web.HttpUtility.HtmlDecode("&nbsp;"), "");
                    txtCatalog.Text = labCata1.Text;
                    hidCatalogName.Value = labCata1.Text;
                }
                if (RadioCate1.Items.Count > 0)
                    if (RadioCate1.SelectedIndex != -1)
                        labCata1.Text = RadioCate1.SelectedItem.Text.Replace(System.Web.HttpUtility.HtmlDecode("&nbsp;"), "");
                    else
                        labCata1.Text = "";

                if (ContralState != eOA_FlowControlState.eNormal)
                {
                    labCata1.Visible = true;
                    ddlCate1.Visible = false;
                    RadioCate1.Visible = false;
                    txtCatalog.Visible = false;
                    cmdPopParentCataLog.Visible = false;
                    rWarning.Visible = false;
                    if (ContralState == eOA_FlowControlState.eHidden)
                    {
                        labCata1.Text = "--";
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

        private int mShowType = 0;
        /// <summary>
        /// �������ʹ�����ַ�ʽ��ʾ 1��ʾ��ѡ��ť 2 ��ʾ������ 3 ��ʾ���οؼ�
        /// </summary>
        public int ShowType
        {
            get { return mShowType; }
            set { mShowType = value; }
        }

        /// <summary>
        /// ����ֻ��״̬ʱ�ı�ǩ��ɫ
        /// </summary>
        public Color ForColor
        {
            set
            {
                labCata1.ForeColor = value;
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

        public long CatelogID
        {
            get
            {
                if (hidCataID.Value.Trim() == string.Empty)
                    return 0;
                else
                    return long.Parse(hidCataID.Value);
            }
            set
            {
                lngCatelogID = value;
                hidCataID.Value = Convert.ToString(value);
                if (ddlCate1.Items.Count == 0)
                {
                    SetDboControlValue(ref ddlCate1, ref RadioCate1, lngRootID, lngCatelogID);
                }
                if (RadioCate1.Items.Count == 0)
                {
                    SetDboControlValue(ref ddlCate1, ref RadioCate1, lngRootID, lngCatelogID);
                }
                RadioCate1.SelectedIndex = RadioCate1.Items.IndexOf(RadioCate1.Items.FindByValue(value.ToString()));
                ddlCate1.SelectedIndex = ddlCate1.Items.IndexOf(ddlCate1.Items.FindByValue(value.ToString()));
                lblMessage.Text = CatalogDP.GetCatalogRemark(CatelogID);
                //BindDrop();
            }
        }

        public string CatelogValue
        {
            get
            {
                return hidCatalogName.Value.Replace(" ", "").Replace("&nbsp;", "").Trim();
            }
            set
            {
                sValue = value;
                labCata1.Text = value;
                txtCatalog.Text = value;
                hidCatalogName.Value = value;
                //ddlCate1.SelectedIndex = ddlCate1.Items.IndexOf(ddlCate1.Items.FindByText(value));
                //RadioCate1.SelectedIndex = RadioCate1.Items.IndexOf(RadioCate1.Items.FindByText(value));
            }
        }

        /// <summary>
        /// ����ʱ�ͻ��˽ű�
        /// </summary>
        public string OnChangeScript
        {
            set { ViewState["OnChangeScript"] = value;
            hidscript.Value = value;
            }
            get { return ViewState["OnChangeScript"] == null ? string.Empty : ViewState["OnChangeScript"].ToString(); }
        }

        #endregion

        public event EventHandler mySelectedIndexChanged;

        void DrpDictionary_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mySelectedIndexChanged != null)

                mySelectedIndexChanged(this, new EventArgs()); //����SelectIndexChanged�¼� 
        }

        public event EventHandler myRadioSelectedIndexChanged;

        void RadioDictionary_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (myRadioSelectedIndexChanged != null)

                myRadioSelectedIndexChanged(this, new EventArgs()); //����SelectIndexChanged�¼� 
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

            if (myRadioSelectedIndexChanged != null)
            {
                RadioCate1.AutoPostBack = true;
                RadioCate1.SelectedIndexChanged += new EventHandler(RadioDictionary_SelectedIndexChanged);
            }

            if (blnRemark)
            {
                lblMessage.Visible = true;
            }

            if (Page.IsPostBack == false)
            {

                #region �ж�ʹ�����ַ�ʽ��ʾ
                if (blnMust == true && !IsPoint)
                {
                    txtCatalog.Visible = false;
                    cmdPopParentCataLog.Visible = false;
                    RadioCate1.Visible = false;
                    ddlCate1.Visible = true;
                }
                else if (ShowType <= 0 || IsPoint) //��ʹ��������ʾ
                {
                    int listnum = int.Parse(CommonDP.GetConfigValue("Other", "ListNum") == "" ? "0" : CommonDP.GetConfigValue("Other", "ListNum"));
                    int radionum = int.Parse(CommonDP.GetConfigValue("Other", "RadioNum") == "" ? "0" : CommonDP.GetConfigValue("Other", "RadioNum"));

                    DataTable dt = CatalogDP.GetBelowCatas(lngRootID);
                    if (ContralState == eOA_FlowControlState.eNormal)
                    {
                        if ((dt != null && dt.Rows.Count - 1 > listnum) || IsPoint)
                        {
                            ddlCate1.Visible = false;
                            RadioCate1.Visible = false;
                            txtCatalog.Visible = true;
                            cmdPopParentCataLog.Visible = true;
                        }
                        else if (dt != null && dt.Rows.Count - 1 <= radionum)
                        {
                            txtCatalog.Visible = false;
                            cmdPopParentCataLog.Visible = false;
                            RadioCate1.Visible = true;
                            ddlCate1.Visible = false;
                        }
                        else
                        {
                            txtCatalog.Visible = false;
                            cmdPopParentCataLog.Visible = false;
                            RadioCate1.Visible = false;
                            ddlCate1.Visible = true;
                        }
                    }
                }
                else
                {
                    switch (ShowType)
                    {
                        case 1:
                            txtCatalog.Visible = false;
                            cmdPopParentCataLog.Visible = false;
                            RadioCate1.Visible = true;
                            ddlCate1.Visible = false;
                            break;
                        case 2:
                            txtCatalog.Visible = false;
                            cmdPopParentCataLog.Visible = false;
                            RadioCate1.Visible = false;
                            ddlCate1.Visible = true;
                            break;
                        case 3:
                            ddlCate1.Visible = false;
                            RadioCate1.Visible = false;
                            txtCatalog.Visible = true;
                            cmdPopParentCataLog.Visible = true;
                            break;
                        default:
                            txtCatalog.Visible = false;
                            cmdPopParentCataLog.Visible = false;
                            RadioCate1.Visible = false;
                            ddlCate1.Visible = true;
                            break;
                    }
                }

                #endregion

                if (lngRootID != 0)
                {
                    //û�����ø����� �����κ�����
                    //SetDboControlValue(ref ddlCate1, lngRootID, lngCatelogID);
                    if (ddlCate1.Items.Count > 0)
                    {
                        labCata1.Text = ddlCate1.SelectedItem.Text.Replace(System.Web.HttpUtility.HtmlDecode("&nbsp;"), "");
                        hidCatalogName.Value = labCata1.Text;
                        txtCatalog.Text = labCata1.Text;
                    }

                    if (ContralState != eOA_FlowControlState.eNormal)
                    {
                        labCata1.Visible = true;
                        ddlCate1.Visible = false;
                        RadioCate1.Visible = false;
                        txtCatalog.Visible = false;
                        cmdPopParentCataLog.Visible = false;
                        rWarning.Visible = false;
                        if (ContralState == eOA_FlowControlState.eHidden)
                        {
                            labCata1.Text = "--";
                            lblMessage.Visible = false;
                        }
                    }

                }                                            
            }
            else
            {
                txtCatalog.Text = hidCatalogName.Value;
            }

            #region ���ñ�����ʾ yxq
            if (blnMust == true)
            {
                if (ddlCate1.Visible == true)
                {
                    //��ӹ���У���������
                    string strThisMsg = "";
                    strThisMsg = ddlCate1.ClientID + ">" + strMessage + ">" + "0";
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), ddlCate1.ClientID, "<script>if(typeof(sarrvalidlist) == 'undefined'){ var sarrvalidlist = new Array(1);}  sarrvalidlist[0] = sarrvalidlist[0] + '|' + '" + strThisMsg + "';</script>");
                }
                else if (txtCatalog.Visible == true)
                {
                    //��ӹ���У���������
                    string strThisMsg = "";

                    strThisMsg = txtCatalog.ClientID + ">" + strMessage;
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), txtCatalog.ClientID, "<script type=\"text/javascript\" >if(typeof(sarrvalidlist) == 'undefined'){ var sarrvalidlist = new Array(1);}  sarrvalidlist[0] = sarrvalidlist[0] + '|' + '" + strThisMsg + "';</script>");
                }
                else if (RadioCate1.Visible == true)
                {
                    //��ӹ���У���������
                    string strThisMsg = "";
                    strThisMsg = RadioCate1.ClientID + ">" + strMessage + ">" + "0" + ">" + RadioCate1.Items.Count;
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), RadioCate1.ClientID, "<script>if(typeof(sarrvalidlist) == 'undefined'){ var sarrvalidlist = new Array(1);}  sarrvalidlist[0] = sarrvalidlist[0] + '|' + '" + strThisMsg + "';</script>");
                }
            }
            #endregion

            if (blnMust == false)
            {
                rWarning.Visible = false;
            }

            if (OnChangeScript.Length > 0)
            {
                ddlCate1.Attributes.Add("onchange", "CataSelecteChanged(this);" + OnChangeScript);
                RadioCate1.Attributes.Add("onclick", "RadioSelecteChanged(this);" + OnChangeScript);
                txtCatalog.Attributes.Add("onchange", OnChangeScript);
            }
            else
            {
                ddlCate1.Attributes.Add("onchange", "CataSelecteChanged(this);");
                RadioCate1.Attributes.Add("onclick", "RadioSelecteChanged(this);");
            }
        }


        #region ���������б�ֵ SetDboControlValue
        /// <summary>
        /// ���������б�ֵ
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="lngRootID"></param>
        /// <param name="lngValue"></param>
        private void SetDboControlValue(ref DropDownList ddl, ref RadioButtonList Redio, long lngRootID, long lngValue)
        {
            ArrayList arrValues = new ArrayList();
            ArrayList arrTexts = new ArrayList();

            SetArrListValue(lngRootID, ref arrTexts, ref arrValues);
            ddl.Items.Clear();
            Redio.Items.Clear();

            for (int i = 0; i < arrValues.Count; i++)
            {
                ddl.Items.Add(new ListItem(arrTexts[i].ToString(), arrValues[i].ToString()));
                if (arrTexts[i].ToString().Trim() != "")
                    Redio.Items.Add(new ListItem(arrTexts[i].ToString().Replace(System.Web.HttpUtility.HtmlDecode("&#160;"), "").Replace(System.Web.HttpUtility.HtmlDecode("&nbsp;"), "").Trim(), arrValues[i].ToString()));
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

            for (int i = 0; i < Redio.Items.Count; i++)
            {
                if (CurIndex != 0)
                {
                    Redio.SelectedIndex = CurIndex;
                }
                if (Redio.Items[i].Value == lngValue.ToString())
                {
                    Redio.SelectedIndex = i;
                    break;
                }
                if (sValue != string.Empty && Redio.Items[i].Text.Trim() == sValue)
                {
                    Redio.SelectedIndex = i;
                    break;
                }
            }

            lblMessage.Text = CatalogDP.GetCatalogRemark(CatelogID);
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
            DataTable dt = CatalogDP.GetBelowCatas(lngRootID);
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
                        //arrValues.Add(lngRootID.ToString());
                        arrValues.Add("0");
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