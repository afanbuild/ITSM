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
    public partial class ctrFlowCataDropList : System.Web.UI.UserControl
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
            get
            {
                if (DDlorRedio == "DDL")
                {
                    return ddlCate1.ClientID;
                }
                else
                {
                    return RadioCate1.ClientID;
                }
            }
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

                SetDboControlValue(ref ddlCate1, ref RadioCate1, lngRootID, lngCatelogID);
                if (DDlorRedio == "DDL")
                {
                    //��ʱdropDownList �ؼ�ʱ�Ĵ���
                    if (ddlCate1.Items.Count > 0)
                        labCate1.Text = ddlCate1.SelectedItem.Text.Replace(System.Web.HttpUtility.HtmlDecode("&nbsp;"), "");
                }
                else
                {
                    //��ʱradioButtonList �ؼ�ʱ�Ĵ���
                    if (RadioCate1.Items.Count > 0)
                        if (RadioCate1.SelectedIndex != -1)
                            labCate1.Text = RadioCate1.SelectedItem.Text.Replace(System.Web.HttpUtility.HtmlDecode("&nbsp;"), "");
                        else
                            labCate1.Text = "";


                }

                if (ContralState != eOA_FlowControlState.eNormal)
                {
                    labCate1.Visible = true;
                    ddlCate1.Visible = false;
                    rWarning.Visible = false;
                    RadioCate1.Visible = false;
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
                if (DDlorRedio == "DDL")
                {
                    #region dropDownlist �ؼ�
                    //dropDownlist �ؼ�
                    if (ddlCate1.Items.Count > 0)
                        return long.Parse(ddlCate1.SelectedItem.Value);
                    else
                        return -1;
                    #endregion
                }
                else
                {
                    #region RadioDownList �ؼ�
                    //RadioDownList �ؼ��Ĵ���
                    if (RadioCate1.Items.Count > 0)
                        if (RadioCate1.SelectedIndex != -1)//�ж��Ƿ���ѡ��
                            return long.Parse(RadioCate1.SelectedItem.Value);
                        else
                            return -1;
                    else
                        return -1;
                    #endregion
                }
            }
            set
            {
                lngCatelogID = value;
                hidCateID.Value = Convert.ToString(value);

                if (DDlorRedio == "DDL")
                {
                    #region dropDownList�ؼ�
                    //dropDownList�ؼ�
                    if (ddlCate1.Items.Count == 0)
                    {
                        SetDboControlValue(ref ddlCate1, ref RadioCate1, lngRootID, lngCatelogID);
                    }
                    ddlCate1.SelectedIndex = ddlCate1.Items.IndexOf(ddlCate1.Items.FindByValue(value.ToString()));
                    if (ddlCate1.Items.Count > 0)
                        labCate1.Text = ddlCate1.SelectedItem.Text.Replace(System.Web.HttpUtility.HtmlDecode("&nbsp;"), "");
                    #endregion
                }
                else
                {
                    #region radioButtonlist �ؼ�
                    //radioButtonlist �ؼ�
                    if (RadioCate1.Items.Count == 0)
                    {
                        SetDboControlValue(ref ddlCate1, ref RadioCate1, lngRootID, lngCatelogID);
                    }
                    RadioCate1.SelectedIndex = RadioCate1.Items.IndexOf(RadioCate1.Items.FindByValue(value.ToString()));

                    if (RadioCate1.Items.Count > 0)
                        if (RadioCate1.SelectedIndex != -1)
                            labCate1.Text = RadioCate1.SelectedItem.Text.Replace(System.Web.HttpUtility.HtmlDecode("&nbsp;"), "");
                        else
                            labCate1.Text = "";

                    #endregion
                }
                lblMessage.Text = CatalogDP.GetCatalogRemark(CatelogID);
                //BindDrop();
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

                if (DDlorRedio == "DDL")
                {
                    if (ddlCate1.Items.Count > 0)
                        return ddlCate1.SelectedItem.Text.Trim().Replace(System.Web.HttpUtility.HtmlDecode("&#160;"), "").Replace(System.Web.HttpUtility.HtmlDecode("&nbsp;"), "");
                    else
                        return string.Empty;
                }
                else
                {
                    if (RadioCate1.Items.Count > 0)
                        if (RadioCate1.SelectedIndex != -1)
                            return RadioCate1.SelectedItem.Text.Trim().Replace(System.Web.HttpUtility.HtmlDecode("&#160;"), "").Replace(System.Web.HttpUtility.HtmlDecode("&nbsp;"), "");
                        else
                            return string.Empty;
                    else
                        return string.Empty;
                }
            }
            set
            {
                sValue = value;
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

        #region ���� �ؼ� ���ֵ��� dropDownList ���� radioButtonList
        /// <summary>
        /// ���� �ؼ� ���ֵ��� dropDownList ���� radioButtonList 
        /// DDL ΪdropDownList�ؼ�
        /// REDIO Ϊ RadioButtonList �ؼ� 
        /// </summary>
        private string DDlorRedio
        {
            get
            {
                if (ViewState["DDlorRedio"] != null)
                    return ViewState["DDlorRedio"].ToString();
                else
                    return "DDL";
            }
            set
            {
                ViewState["DDlorRedio"] = value;
            }
        }
        #endregion


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
                    //  SetDboControlValue(ref ddlCate1, ref RadioCate1, lngRootID, lngCatelogID);
                    if (DDlorRedio == "DDL") //������ DropDownList �ǵĴ���
                    {
                        #region //������ DropDownList �ǵĴ���
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
                    }
                    else if (DDlorRedio == "REDIO")
                    {
                        #region ��ʱredio �ؼ�ʱ�Ĵ���
                        //��ʱredio �ؼ�ʱ�Ĵ���
                        if (RadioCate1.Items.Count > 0)
                            if (RadioCate1.SelectedIndex != -1)
                                labCate1.Text = RadioCate1.SelectedItem.Text.Replace(System.Web.HttpUtility.HtmlDecode("&nbsp;"), "");
                            else
                                labCate1.Text = "";

                        if (blnMust == true)
                        {
                            //��ӹ���У���������
                            string strThisMsg = "";
                            strThisMsg = RadioCate1.ClientID + ">" + strMessage + ">" + lngRootID.ToString();
                            this.Page.ClientScript.RegisterStartupScript(this.GetType(), RadioCate1.ClientID, "<script>if(typeof(sarrvalidlist) == 'undefined'){ var sarrvalidlist = new Array(1);}  sarrvalidlist[0] = sarrvalidlist[0] + '|' + '" + strThisMsg + "';</script>");
                        }

                        rWarning.Visible = false;
                        #endregion
                    }                   

                }

                if (OnChangeScript.Length > 0)
                {
                    ddlCate1.Attributes.Add("onchange", "CateSelecteChanged(this);" + OnChangeScript);
                    RadioCate1.Attributes.Add("onchange", "CateSelecteChanged(this);" + OnChangeScript);
                }
                else
                {
                    ddlCate1.Attributes.Add("onchange", "CateSelecteChanged(this);");
                    RadioCate1.Attributes.Add("onchange", "CateSelecteChanged(this);");
                }
            }

            if (ContralState != eOA_FlowControlState.eNormal)
            {
                labCate1.Visible = true;
                ddlCate1.Visible = false;
                RadioCate1.Visible = false;
                rWarning.Visible = false;
                if (ContralState == eOA_FlowControlState.eHidden)
                {
                    labCate1.Text = "--";
                    lblMessage.Visible = false;
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
        private void SetDboControlValue(ref DropDownList ddl, ref RadioButtonList Redio, long lngRootID, long lngValue)
        {
            ArrayList arrValues = new ArrayList();
            ArrayList arrTexts = new ArrayList();

            SetArrListValue(lngRootID, ref arrTexts, ref arrValues);
            ddl.Items.Clear();
            Redio.Items.Clear();

            int intCount = int.Parse(CommonDP.GetConfigValue("Other", "RadioOrDDL") == "" ? "0" : CommonDP.GetConfigValue("Other", "RadioOrDDL"));
            if (arrValues.Count > intCount)
            {
                DDlorRedio = "DDL";
                Redio.Visible = false;
                ddl.Visible = true;
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
            }
            else
            {
                DDlorRedio = "REDIO";
                ddl.Visible = false;
                Redio.Visible = true;
                for (int i = 0; i < arrValues.Count; i++)
                {
                    if (arrTexts[i].ToString().Trim() != "")
                        Redio.Items.Add(new ListItem(arrTexts[i].ToString().Replace(System.Web.HttpUtility.HtmlDecode("&#160;"), "").Replace(System.Web.HttpUtility.HtmlDecode("&nbsp;"), "").Trim(), arrValues[i].ToString()));
                }
                //���õ�ǰֵ
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