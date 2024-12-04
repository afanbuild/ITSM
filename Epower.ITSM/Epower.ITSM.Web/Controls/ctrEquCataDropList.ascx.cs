/****************************************************************************
 * 
 * description:�豸��������б�
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-09-24
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
using Epower.ITSM.Base;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.Controls
{
    public partial class ctrEquCataDropList : System.Web.UI.UserControl
    {
        #region ����������
        private string sValue = "";
        private eOA_FlowControlState cState = eOA_FlowControlState.eNormal;

        private decimal lngRootID = 0;
        private string strMessage = "";
        private decimal lngCatelogID = 0;
        #endregion


        #region ���Զ���

        #region ����ʱ�ͻ��˽ű�
        /// <summary>
        /// ����ʱ�ͻ��˽ű�
        /// </summary>
        public string OnChangeScript
        {
            set
            {
                ViewState["OnChangeScript"] = value;
            }
            get
            {
                return ViewState["OnChangeScript"] == null ? string.Empty : ViewState["OnChangeScript"].ToString();
            }

        }
        #endregion

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
        public decimal RootID
        {
            get { return lngRootID; }
            set { lngRootID = value; }
        }


        /// <summary>
        /// ����ֻ��״̬ʱ�ı�ǩ��ɫ
        /// </summary>
        public Color ForColor
        {
            set { labCate1.ForeColor = value; }
        }




        /// <summary>
        /// �ؼ�״̬
        /// </summary>
        public eOA_FlowControlState ContralState
        {
            get { return cState; }
            set { cState = value; }
        }

        public decimal CatelogID
        {
            get
            {
                if (ddlCate1.Items.Count > 0)
                    return decimal.Parse(ddlCate1.SelectedItem.Value);
                else
                    return -1;
            }
            set
            {
                lngCatelogID = value;
                ddlCate1.SelectedIndex = ddlCate1.Items.IndexOf(ddlCate1.Items.FindByValue(lngCatelogID.ToString()));
                //BindDrop();
            }
        }


        private string sSelectedIndex = string.Empty;
        public string SelectedIndex
        {
            get
            {
                if (sSelectedIndex != "" && sSelectedIndex != string.Empty)
                    return sSelectedIndex;
                return string.Empty;
            }
            set
            {
                sSelectedIndex = value;
            }
        }
        public string CatelogValue
        {
            get
            {
                if (ddlCate1.Items.Count > 0)
                    return ddlCate1.SelectedItem.Text.Trim();
                else
                    return string.Empty;
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
        #endregion

        public event EventHandler mySelectedIndexChanged;

        void DrpDictionary_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mySelectedIndexChanged != null)

                mySelectedIndexChanged(this, new EventArgs()); //����SelectIndexChanged�¼� 
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (mySelectedIndexChanged != null)
            {
                ddlCate1.AutoPostBack = true;
                ddlCate1.SelectedIndexChanged += new EventHandler(DrpDictionary_SelectedIndexChanged);
            }

            if (Page.IsPostBack == false)
            {
                if (mySelectedIndexChanged != null)
                {

                }


                if (lngRootID != 0)
                {
                    //û�����ø����� �����κ�����
                    SetDboControlValue(ref ddlCate1, lngRootID, lngCatelogID);
                    if (ddlCate1.Items.Count > 0)
                        labCate1.Text = ddlCate1.SelectedItem.Text.Replace(System.Web.HttpUtility.HtmlDecode("&nbsp;"), "");

                    if (cState != eOA_FlowControlState.eNormal)
                    {
                        labCate1.Visible = true;
                        ddlCate1.Visible = false;
                        if (cState == eOA_FlowControlState.eHidden)
                        {
                            labCate1.Text = "--";
                        }
                    }
                }

                if (sSelectedIndex != string.Empty)
                {
                    ddlCate1.SelectedItem.Value = sSelectedIndex;
                }
                //����ʱ�ͻ��˽ű�
                if (OnChangeScript.Length > 0)
                {
                    ddlCate1.Attributes.Add("onchange", "CateSelecteChanged(this);" + OnChangeScript);
                }
                else
                {
                    ddlCate1.Attributes.Add("onchange", "CateSelecteChanged(this);");
                }
            }

        }


        #region ���������б�ֵ SetDboControlValue
        /// <summary>
        /// ���������б�ֵ
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="lngRootID"></param>
        /// <param name="lngValue"></param>
        private void SetDboControlValue(ref DropDownList ddl, decimal lngRootID, decimal lngValue)
        {
            ArrayList arrValues = new ArrayList();
            ArrayList arrTexts = new ArrayList();

            SetArrListValue(lngRootID, ref arrTexts, ref arrValues);

            for (int i = 0; i < arrValues.Count; i++)
            {
                ddl.Items.Add(new ListItem(arrTexts[i].ToString(), arrValues[i].ToString()));
            }
            //���õ�ǰֵ
            for (int i = 0; i < ddl.Items.Count; i++)
            {
                if (ddl.Items[i].Value == lngValue.ToString())
                {
                    ddl.SelectedIndex = i;
                    break;
                }
            }
        }
        #endregion

        #region �����б� SetArrListValue
        /// <summary>
        /// �����б�
        /// </summary>
        /// <param name="lngRootID"></param>
        /// <param name="arrTexts"></param>
        /// <param name="arrValues"></param>
        private void SetArrListValue(decimal lngRootID, ref ArrayList arrTexts, ref ArrayList arrValues)
        {
            DataTable dt = Equ_SubjectDP.GetBelowCatas(lngRootID);
            bool blnHasRoot = false;
            if (dt != null && dt.Rows.Count > 0)
            {
                //�Ҹ��ڵ�
                foreach (DataRow row in dt.Rows)
                {
                    if (row["catalogid"].ToString() == lngRootID.ToString())
                    {
                        arrTexts.Add(row["CatalogName"].ToString());
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
        private void AddSubArrList(DataTable dt, decimal lngRootID, int Layer, ref ArrayList arrTexts, ref ArrayList arrValues)
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