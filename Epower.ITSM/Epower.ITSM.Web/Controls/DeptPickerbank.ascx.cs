/*******************************************************************
 * ��Ȩ���У�
 * Description��ѡ���ſؼ�
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
    public partial class DeptPickerbank : System.Web.UI.UserControl
    {
        public string sApplicationUrl = Constant.ApplicationPath;     //����·��
        private string strMessage = "";
        private bool blnMust = false;
        #region ����
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
            set 
            { 
                ViewState["eOA_FlowControlState"] = value;
                if (value == eOA_FlowControlState.eReadOnly)
                {
                     txtDept.Visible = false;
                     labDeptName.Visible = true;
                     rWarning.Visible = false;
                     cmdPopParentDept.Visible = false;
                }
                else if (value == eOA_FlowControlState.eHidden)
                {
                    txtDept.Visible = false;
                    labDeptName.Visible = true;
                    labDeptName.Text = "--";
                    rWarning.Visible = false;
                    cmdPopParentDept.Visible = false;
                }
                else
                {
                    txtDept.Visible = true;
                    labDeptName.Visible = false;
                    rWarning.Visible = true;
                    cmdPopParentDept.Visible = true;
                }
            }
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
                else
                {
                    txtDept.Width = Unit.Parse("70%");
                }
            }
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

        public bool VisibleText
        {
            get
            {
                return txtDept.Visible;
            }
            set
            {
                txtDept.Visible = value;
                rWarning.Visible = value;
                cmdPopParentDept.Visible = value;
            }
        }


        public bool VisibleLabel
        {
            set
            {
                labDeptName.Visible = value;
                if (labDeptName.Visible)
                    rWarning.Visible = false;
            }
        }

        public long DeptID
        {
            get
            {
                if (hidDept.Value.Trim() == string.Empty)
                    return 0;
                else
                    return long.Parse(hidDept.Value);
            }
            set
            {
                hidDept.Value = value.ToString();
            }
        }

        /// <summary>
        /// ����ֻ��״̬ʱ�ı�ǩ��ɫ
        /// </summary>
        public Color ForColor
        {
            set {
                labDeptName.ForeColor = value;
                txtDept.ForeColor = value;
            }
        }
        /// <summary>
        /// ����δ����ʱ����ʾ��Ϣ
        /// </summary>
        public string TextToolTip
        {
            set { strMessage = value; }
        }

        #region ֻ��
        /// <summary>
        /// ֻ��
        /// </summary>
        public bool ReadOnly
        {
            set 
            {
                labDeptName.Visible = true;
                txtDept.Visible = false;
                cmdPopParentDept.Visible = false;
                rWarning.Visible = false;
            }
        }
        #endregion
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            if (Page.IsPostBack == false)
            {
                if (blnMust == true && txtDept.Visible == true)
                {
                    //��ӹ���У���������
                    string strThisMsg = "";
                    strThisMsg = txtDept.ClientID + ">" + strMessage;
                    //Response.Write("<script>if(typeof(window.parent.header.flowInfo.hidValidateList) != 'undefined')  {window.parent.header.flowInfo.hidValidateList.value = window.parent.header.flowInfo.hidValidateList.value + '|' + '" + strThisMsg + "';}</script>");
                    Response.Write("<script>if(typeof(sarrvalidlist) == 'undefined'){ var sarrvalidlist = new Array(1);}  sarrvalidlist[0] = sarrvalidlist[0] + '|' + '" + strThisMsg + "';</script>");
                    //Response.Write("<script>window.parent.header.flowInfo.hidValidateList.value = '" + txtText.ClientID + "';</script>");
                }
            }
        }

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
                if (ContralState != eOA_FlowControlState.eNormal)
                {
                    labDeptName.Visible = true;
                    txtDept.Visible = false;
                    rWarning.Visible = false;
                    cmdPopParentDept.Visible = false;
                    if (ContralState == eOA_FlowControlState.eHidden)
                    {
                        labDeptName.Text = "--";
                    }
                }
            }

            if (blnMust == false)
            {
                rWarning.Visible = false;
            }
        }

        public event EventHandler bankTexChange;//

        protected void txtDept_TextChanged(object sender, EventArgs e)
        {
            if (bankTexChange != null)
            {
                bankTexChange(this, new EventArgs());
            }
        }
    }
}