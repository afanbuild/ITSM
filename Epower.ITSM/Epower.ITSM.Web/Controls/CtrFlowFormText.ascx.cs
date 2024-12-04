/*******************************************************************
 * ��Ȩ���У�
 * Description�������ַ�����ؼ�
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
using System.Drawing;

namespace Epower.ITSM.Web.Controls
{
    public partial class CtrFlowFormText : System.Web.UI.UserControl
    {
        private string sValue = "";

        private string strOnChangeFun = "";

        private string strOnFocusFun = "";

        private string strOnBlurFun = "";

        private bool blnMust = false;

        private string strMessage = "";



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
        /// ���
        /// </summary>
        public Unit Width
        {
            set {
                if (value != null)
                {
                    txtText.Width = value;
                }
                else
                {
                    txtText.Width = Unit.Parse("70%");
                }
            }
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
        /// ������ɫ
        /// </summary>
        public Color foreColor
        {
            set { labCaption.ForeColor = value; }
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
            set { sValue = value; txtText.Text = sValue; }
        }

        /// <summary>
        /// �ı���ģʽ
        /// </summary>
        public TextBoxMode TextMode
        {
            set { txtText.TextMode = value; }
        }

        /// <summary>
        /// �ı�����
        /// </summary>
        public int Row
        {
            set { txtText.Rows = value; }
        }

        /// <summary>
        /// �ı�����
        /// </summary>
        public int Column
        {
            set { txtText.Columns = value; }
        }

        private string GetMyValues()
        {
           
            return txtText.Text.Trim();
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
                labCaption.Text = txtText.Text;

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
            }

            if (blnMust == true)
            {
                //��ӹ���У���������
                string strThisMsg = "";
                strThisMsg = txtText.ClientID + ">" + strMessage;
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), txtText.ClientID, "<script type=\"text/javascript\" >if(typeof(sarrvalidlist) == 'undefined'){ var sarrvalidlist = new Array(1);}  sarrvalidlist[0] = sarrvalidlist[0] + '|' + '" + strThisMsg + "';</script>");
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