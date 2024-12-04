/*******************************************************************
 * ��Ȩ���У�
 * Description��������������ؼ�
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
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Epower.ITSM.Base;

namespace Epower.ITSM.Web.Controls
{
    public partial class CtrFlowNumeric : System.Web.UI.UserControl
    {
        private string sValue = "";

        private string strOnChangeFun = "";

        private string strOnBlurFun = "";

        private string strOnFocusFun = "";

        private bool blnMust = false;

        private string strMessage = "";
       
        //�������� 0��ʾ��ͨ��1��ʾRMB��2��ʾ��Ԫ,3��ʾӢ��
        private int cType = 0;

        /// <summary>
        /// �������� 0��ʾ��ͨ��1��ʾRMB��2��ʾ��Ԫ,3��ʾӢ��
        /// </summary>
        public int CurrencyType 
        {
            set 
            {
                cType = value;
            }
            get 
            {
                return cType;
            }
        }

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

        /// <summary>
        /// ����δ����ʱ����ʾ��Ϣ
        /// </summary>
        public string TextToolTip
        {
            set { strMessage = value; }
        }

        /// <summary>
        /// �ı��ؼ��Ŀͻ���ID
        /// </summary>
        public string ClientID
        {
            get { return txtText.ClientID; }
        }

        /// <summary>
        /// ��󳤶�
        /// </summary>
        public int MaxLength
        {
            set { txtText.MaxLength = value; }
        }

        /// <summary>
        /// ����ʱ�ͻ��˽ű�
        /// </summary>
        public string OnChangeScript
        {
            set { strOnChangeFun = value; }
        }

        /// <summary>
        /// ��ý���ʱ�ͻ��˽ű�
        /// </summary>
        public string OnFocusScript
        {
            set { strOnFocusFun = value; }
        }


        /// <summary>
        /// �뿪ʱ�ͻ��˽ű�
        /// </summary>
        public string OnBlurScript
        {
            set { strOnBlurFun = value; }
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

        /// <summary>
        /// �ؼ���ֵ
        /// </summary>
        public string Value
        {
            get { return GetMyValues(); }
            set 
            { 
                sValue = value;
                txtText.Text = sValue;

                #region ���ӻ�����ʾ���� ����ǰ 2014-01-22
                //��ͨ
                if (CurrencyType == 0)
                {
                    labCaption.Text = sValue;
                }
                else if (CurrencyType == 1) //RMB
                {
                    decimal d = decimal.Parse(sValue);

                    labCaption.Text = "��" + Math.Round(d, 2) + "��";
                }
                #endregion
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
                    txtText.Width = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetMyValues()
        {
            if (txtText.Text.Trim() != string.Empty)
                return txtText.Text.Trim();
            else
                return "0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {                
                txtText.Text = sValue;

                //labCaption.Text = txtText.Text;

                #region ���ӻ�����ʾ���� ����ǰ 2014-01-22
                //��ͨ
                if (CurrencyType == 0)
                {
                    labCaption.Text = sValue;
                }
                else if (CurrencyType == 1) //RMB
                {
                    decimal d = decimal.Parse(sValue);

                    labCaption.Text = "��" + Math.Round(d, 2) + "��";
                }
                #endregion

            }


            if (ContralState != eOA_FlowControlState.eNormal)
            {
                labCaption.Visible = true;
                txtText.Visible = false;
                rWarning.Visible = false;
                if (ContralState == eOA_FlowControlState.eHidden)
                {
                    labCaption.Text = "--";
                }
            }


            txtText.Attributes.Add("onkeydown", "NumberInput('1')");

            if (blnMust == true)
            {
                //��ӹ���У���������
                string strThisMsg = "";
                strThisMsg = txtText.ClientID + ">" + strMessage;
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), txtText.ClientID, "<script>if(typeof(sarrvalidlist) == 'undefined'){ var sarrvalidlist = new Array(1);}  sarrvalidlist[0] = sarrvalidlist[0] + '|' + '" + strThisMsg + "';</script>");
            }

            if (blnMust == false)
            {
                rWarning.Visible = false;
            }

            if (strOnChangeFun.Length > 0)
            {
                txtText.Attributes.Add("onchange", strOnChangeFun);
            }

            if (strOnBlurFun.Length > 0)
            {
                txtText.Attributes.Add("onblur", strOnBlurFun);
            }

            if (strOnFocusFun.Length > 0)
            {
                txtText.Attributes.Add("onfocus", strOnFocusFun);
            }

        }

    }
}