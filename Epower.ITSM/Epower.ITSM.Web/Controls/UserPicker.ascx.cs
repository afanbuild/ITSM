/*******************************************************************
 * 版权所有：
 * Description：用户人员选择
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
    public partial class UserPicker : System.Web.UI.UserControl
    {
        public string sApplicationUrl = Epower.ITSM.Base.Constant.ApplicationPath;     //虚拟路径

        private string strOnChangeFun = string.Empty;

        #region 属性
        /// <summary>
        /// 用户名称
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
        /// 显示输入文本框
        /// </summary>
        public bool VisibleText
        {
            get { return txtUser.Visible; }
            set
            {
                txtUser.Visible = value;
                cmdPopUser.Visible = value;
            }
        }

        /// <summary>
        /// 显示只读Lable
        /// </summary>
        public bool VisibleLabel
        {
            set
            {
                labUser.Visible = value;
            }
        }

        /// <summary>
        /// 部门ID
        /// </summary>
        public long DeptID {
            set {
                HidDept.Value = value.ToString() ;
            }
        }

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
        /// 用户ID
        /// </summary>
        public long UserID
        {
            get
            {
                if (hidUser.Value.Trim() == string.Empty)
                    return 0;
                else
                    return long.Parse(hidUser.Value);
            }
            set
            {
                hidUser.Value = value.ToString();

                UserName = UserDP.GetUserName(value);
            }
        }

        private bool blnMust = false;
        /// <summary>
        /// 是否一定要输入
        /// </summary>
        public bool MustInput
        {
            set
            {
                blnMust = value;

            }
            get { return blnMust; }
        }

        private string strMessage = "用户选择";
        /// <summary>
        /// 设置未输入时的提示信息
        /// </summary>
        public string TextToolTip
        {
            set { strMessage = value; }
        }

        /// <summary>
        /// 更改时客户端脚本
        /// </summary>
        public string OnChangeScript
        {
            set
            {
                ViewState["OnChangeScriptUserPicker"] = value;
            }
            get { return ViewState["OnChangeScriptUserPicker"] == null ? string.Empty : ViewState["OnChangeScriptUserPicker"].ToString(); }
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
                    txtUser.Width = value;
                }
            }
        }
        #endregion

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                txtUser.Text = hidUserName.Value;
            }
            else
            {               

                if (blnMust == true && ContralState == eOA_FlowControlState.eNormal)
                {
                    //添加公共校验基础资料
                    string strThisMsg = "";
                    strThisMsg = txtUser.ClientID + ">" + strMessage;
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), txtUser.ClientID, "<script>if(typeof(sarrvalidlist) == 'undefined'){ var sarrvalidlist = new Array(1);}  sarrvalidlist[0] = sarrvalidlist[0] + '|' + '" + strThisMsg + "';</script>");
                }
                else
                {
                    rWarning.Visible = false;
                }
              
            }

            if (OnChangeScript.Length > 0)
            {
                txtUser.Attributes.Add("onchange", OnChangeScript);
            }

            if (ContralState != eOA_FlowControlState.eNormal)
            {
                labUser.Visible = true;
                txtUser.Visible = false;
                rWarning.Visible = false;
                cmdPopUser.Visible = false;
                if (ContralState == eOA_FlowControlState.eHidden)
                {
                    labUser.Text = "--";
                }
            }

        }
    }
}