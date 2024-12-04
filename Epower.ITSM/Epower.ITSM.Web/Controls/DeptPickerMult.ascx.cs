/*******************************************************************
 * 版权所有：
 * Description：选择多部门控件
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
    public partial class DeptPickerMult : System.Web.UI.UserControl
    {
        public string sApplicationUrl = Constant.ApplicationPath;     //虚拟路径

        private eOA_FlowControlState cState = eOA_FlowControlState.eNormal;
        private bool blnMust = false;
        #region 属性
        /// <summary>
        /// 控件状态
        /// </summary>
        public eOA_FlowControlState ContralState
        {
            get { return cState; }
            set { cState = value; }
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
                    //添加公共校验基础资料
                    string strThisMsg = "";
                    strThisMsg = txtDept.ClientID + ">选择多部门";
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