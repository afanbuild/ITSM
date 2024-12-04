/*******************************************************************
 * 版权所有：
 * Description：选择部门控件
 * 
 * 
 * Create By  ：
 * Create Date：
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
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.Controls
{
    public partial class DeptPicker : System.Web.UI.UserControl
    {
        public string sApplicationUrl = Constant.ApplicationPath;     //虚拟路径
        private string strMessage = "";
        private bool blnMust = false;
        private string strOnChangeFun = string.Empty;
        #region 属性
        /// <summary>
        /// 控件状态
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
        /// 是否一定要输入[用在工作流母版页中有公共验证,用于其它地方需新增判断代码]
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
                DeptName = DeptDP.GetDeptName(value);
            }
        }
        /// <summary>
        /// 设置未输入时的提示信息
        /// </summary>
        public string TextToolTip
        {
            set { strMessage = value; }
        }

        /// <summary>
        /// 宽度
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

        /// <summary>
        /// 更改时客户端脚本
        /// </summary>
        public string OnChangeScript
        {
            get { return strOnChangeFun; }
            set { strOnChangeFun = value; }
        }

        #region 是否限制选择范围
        /// <summary>
        /// 
        /// </summary>
        private bool mIsLimit = false;
        /// <summary>
        /// 是否限制部门范围
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
                if (blnMust == true && txtDept.Visible == true)
                {
                    //添加公共校验基础资料
                    string strThisMsg = "";
                    strThisMsg = txtDept.ClientID + ">" + strMessage;
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), txtDept.ClientID, "<script>if(typeof(sarrvalidlist) == 'undefined'){ var sarrvalidlist = new Array(1);}  sarrvalidlist[0] = sarrvalidlist[0] + '|' + '" + strThisMsg + "';</script>");
                }               

                if (strOnChangeFun.Length > 0)
                {
                    txtDept.Attributes.Add("onchange", strOnChangeFun);
                }
            }

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

            if (blnMust == false)
            {
                rWarning.Visible = false;
            }
        }

        public event EventHandler deptTexChange;//

        protected void txtChange_TextChanged(object sender, EventArgs e)
        {
            if (deptTexChange != null)
                deptTexChange(this, new EventArgs());
        }
    }
}