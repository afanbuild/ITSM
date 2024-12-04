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
using System.Drawing;

namespace Epower.ITSM.Web.Controls
{


    public partial class DeptPickerBranch : System.Web.UI.UserControl
    {
        public event EventHandler BranchTexChange;//

        public string sApplicationUrl = Constant.ApplicationPath;     //虚拟路径
        private string strMessage = "";
        private bool blnMust = false;
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
                else
                {
                    txtDept.Width = Unit.Parse("70%");
                }
            }
        }

        /// <summary>
        /// 设置只读状态时的标签颜色
        /// </summary>
        public Color ForColor
        {
            set
            {
                labDeptName.ForeColor = value;
                txtDept.ForeColor = value;
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

        public long PrentDeptID
        {
            get
            {
                if (hidParentId.Value.Trim() == string.Empty)
                    return 0;
                else
                    return long.Parse(hidParentId.Value);
            }

            set {
                hidParentId.Value = value.ToString();
            }
        }


        //public string ScriptValue;
        //public string ScriptValues
        //{
        //    set {
        //        ScriptValue = value;
        //    }
        //}
        /// <summary>
        /// 设置未输入时的提示信息
        /// </summary>
        public string TextToolTip
        {
            set { strMessage = value; }
        }

        #region 只读
        /// <summary>
        /// 只读
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
                
        public string ParentID
        {
            get
            {
                if (hidParentId.Value.Trim() == string.Empty)
                    return "0";
                else
                    return hidParentId.Value;
            }
            set
            {
                hidParentId.Value = value;
            }
        }        

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

        protected void txtDept_TextChanged(object sender, EventArgs e)
        {
            if (BranchTexChange != null)
                BranchTexChange(this, new EventArgs());
        }
    }
}