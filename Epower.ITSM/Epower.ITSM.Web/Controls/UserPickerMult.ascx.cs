/*******************************************************************
 * 版权所有：
 * Description：多个人员选择
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

namespace Epower.ITSM.Web.Controls
{
    public partial class UserPickerMult : System.Web.UI.UserControl
    {
        public string sApplicationUrl = Epower.ITSM.Base.Constant.ApplicationPath;     //虚拟路径

        private eOA_FlowControlState cState = eOA_FlowControlState.eNormal;
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
        /// 控件状态
        /// </summary>
        public eOA_FlowControlState ContralState
        {
            get { return cState; }
            set { cState = value; }
        }
        /// <summary>
        /// 用户ID
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

        private string strMessage = "多用户选择";
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
            get { return strOnChangeFun; }
            set { strOnChangeFun = value; }
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
        #region 是否限制选择范围
        /// <summary>
        /// 
        /// </summary>
        private bool mIsLimit = false;
        /// <summary>
        /// 是否限制选择范围
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

                #region 在回发时, 也应检测是否必须输入, 若否则隐藏 * 号. - 2013-06-04 @孙绍棕

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
                    //添加公共校验基础资料
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