/*******************************************************************
 * ��Ȩ���У�
 * Description��ѡ��ಿ�ſؼ�
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
    public partial class DeptPickerMult : System.Web.UI.UserControl
    {
        public string sApplicationUrl = Constant.ApplicationPath;     //����·��

        private eOA_FlowControlState cState = eOA_FlowControlState.eNormal;
        private bool blnMust = false;
        #region ����
        /// <summary>
        /// �ؼ�״̬
        /// </summary>
        public eOA_FlowControlState ContralState
        {
            get { return cState; }
            set { cState = value; }
        }
        /// <summary>
        /// �Ƿ�һ��Ҫ����[���ڹ�����ĸ��ҳ���й�����֤,���������ط��������жϴ���]
        /// </summary>
        public bool MustInput
        {
            set
            {
                blnMust = value;
                rWarning.Visible = value;
            }
            get { return blnMust; }
        }

        public string DeptName
        {
            get
            {
                return txtDept.Text.Trim();
            }
            set
            {
                txtDept.Text = value;
                hidDeptName.Value = value;
                labDeptName.Text = value;
            }
        }

        public string DeptID
        {
            get
            {
               return hidDept.Value;
            }
            set
            {
                hidDept.Value = value.ToString();
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
                    txtDept.Width = value;
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
                txtDept.Text = hidDeptName.Value;
            }
            else
            {
                if (cState != eOA_FlowControlState.eNormal)
                {
                    labDeptName.Visible = true;
                    txtDept.Visible = false;
                    rWarning.Visible = false;
                    cmdPopParentDept.Visible = false;
                    if (cState == eOA_FlowControlState.eHidden)
                    {
                        labDeptName.Text = "--";
                    }
                }

                if (blnMust == true && txtDept.Visible == true)
                {
                    //��ӹ���У���������
                    string strThisMsg = "";
                    strThisMsg = txtDept.ClientID + ">ѡ��ಿ��";
                    //Response.Write("<script>if(typeof(window.parent.header.flowInfo.hidValidateList) != 'undefined')  {window.parent.header.flowInfo.hidValidateList.value = window.parent.header.flowInfo.hidValidateList.value + '|' + '" + strThisMsg + "';}</script>");
                    Response.Write("<script>if(typeof(sarrvalidlist) == 'undefined'){ var sarrvalidlist = new Array(1);}  sarrvalidlist[0] = sarrvalidlist[0] + '|' + '" + strThisMsg + "';</script>");
                    //Response.Write("<script>window.parent.header.flowInfo.hidValidateList.value = '" + txtText.ClientID + "';</script>");
                }
            }

            if (blnMust == false)
            {
                rWarning.Visible = false;
            }
        }
    }
}