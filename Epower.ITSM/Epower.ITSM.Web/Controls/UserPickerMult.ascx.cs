/*******************************************************************
 * ��Ȩ���У�
 * Description�������Աѡ��
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
    public partial class UserPickerMult : System.Web.UI.UserControl
    {
        public string sApplicationUrl = Epower.ITSM.Base.Constant.ApplicationPath;     //����·��

        private eOA_FlowControlState cState = eOA_FlowControlState.eNormal;
        private string strOnChangeFun = string.Empty;
        #region ����
        /// <summary>
        /// �û�����
        /// </summary>
        public string UserName
        {
            get
            {
                return txtUser.Text.Trim();
            }
            set
            {
                txtUser.Text = value;
                this.hidUserName.Value = value;
                labUser.Text = value;
            }
        }

        /// <summary>
        /// �ؼ�״̬
        /// </summary>
        public eOA_FlowControlState ContralState
        {
            get { return cState; }
            set { cState = value; }
        }
        /// <summary>
        /// �û�ID
        /// </summary>
        public string UserID
        {
            get
            {
                if (hidUser.Value.Trim() == string.Empty)
                    return "";
                else
                    return hidUser.Value.Trim();
            }
            set
            {
                hidUser.Value = value.ToString();
            }
        }

        private bool blnMust = false;
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

        private string strMessage = "���û�ѡ��";
        /// <summary>
        /// ����δ����ʱ����ʾ��Ϣ
        /// </summary>
        public string TextToolTip
        {
            set { strMessage = value; }
        }

        /// <summary>
        /// ����ʱ�ͻ��˽ű�
        /// </summary>
        public string OnChangeScript
        {
            get { return strOnChangeFun; }
            set { strOnChangeFun = value; }
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
                    txtUser.Width = value;
                }
            }
        }
        #region �Ƿ�����ѡ��Χ
        /// <summary>
        /// 
        /// </summary>
        private bool mIsLimit = false;
        /// <summary>
        /// �Ƿ�����ѡ��Χ
        /// </summary>
        public bool IsLimit
        {
            set
            {
                mIsLimit = value;
            }
            get
            {
                return mIsLimit;
            }
        }
        #endregion
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                txtUser.Text = hidUserName.Value;

                #region �ڻط�ʱ, ҲӦ����Ƿ��������, ���������� * ��. - 2013-06-04 @������

                if (!MustInput)
                    rWarning.Visible = false;

                #endregion
            }
            else
            {
                if (cState != eOA_FlowControlState.eNormal)
                {
                    labUser.Visible = true;
                    txtUser.Visible = false;
                    rWarning.Visible = false;
                    cmdPopUser.Visible = false;
                    if (cState == eOA_FlowControlState.eHidden)
                    {
                        labUser.Text = "--";
                    }
                }

                if (blnMust == true && txtUser.Visible == true)
                {
                    //��ӹ���У���������
                    string strThisMsg = "";
                    strThisMsg = txtUser.ClientID + ">" + strMessage;

                    Response.Write("<script>if(typeof(sarrvalidlist) == 'undefined'){ var sarrvalidlist = new Array(1);}  sarrvalidlist[0] = sarrvalidlist[0] + '|' + '" + strThisMsg + "';</script>");
                    //Response.Write("<script>window.parent.header.flowInfo.hidValidateList.value = '" + txtText.ClientID + "';</script>");
                }
                else
                {
                    rWarning.Visible = false;
                }
                if (strOnChangeFun.Length > 0)
                {
                    txtUser.Attributes.Add("onchange", strOnChangeFun);
                }
            }
        }
    }
}