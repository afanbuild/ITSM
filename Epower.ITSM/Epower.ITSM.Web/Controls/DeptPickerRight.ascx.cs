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
using Epower.DevBase.Organization.Base;

namespace Epower.ITSM.Web.Controls
{
    public partial class DeptPickerRight : System.Web.UI.UserControl
    {
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

        /// <summary>
        /// 是否设置默认值 (设置默认值时不能选空)
        /// </summary>
        public bool isDefault
        {
            set
            {
                if (value == true)
                {
                    Hiddefualt.Value = "1";
                }
                else
                {
                    Hiddefualt.Value = "0";
                }
            }
        }

        /// <summary>
        /// 设置权限
        /// </summary>
        public long Right
        {
            get
            {
                return long.Parse(hidRight.Value);
            }
            set
            {
                hidRight.Value = value.ToString();
                RightEntity nRightEntity = (RightEntity)((System.Collections.Hashtable)Session["UserAllRights"])[value];
                if (nRightEntity.RightRange == eO_RightRange.eDeptDirect || nRightEntity.RightRange == eO_RightRange.ePersonal)
                {
                    ContralState = eOA_FlowControlState.eReadOnly;
                    DeptID = long.Parse(Session["UserDeptID"].ToString());
                    DeptName = Session["UserDeptName"].ToString();
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
        /// //给控件赋默认值
        /// </summary>
        private void isMoren()
        {
            if (Hiddefualt.Value == "1")
            {
                RightEntity nRightEntity = (RightEntity)((System.Collections.Hashtable)Session["UserAllRights"])[Right];
                if (nRightEntity.RightRange == eO_RightRange.eDept || nRightEntity.RightRange == eO_RightRange.eDeptDirect || nRightEntity.RightRange == eO_RightRange.ePersonal)
                {
                    HidMoRenValue.Value = Session["UserDeptID"].ToString();
                    HidMoRenName.Value = Session["UserDeptName"].ToString();
                }
                else if (nRightEntity.RightRange == eO_RightRange.eOrgDirect || nRightEntity.RightRange == eO_RightRange.eOrg)//所在机构
                {
                    HidMoRenValue.Value = Session["UserOrgID"].ToString();
                    HidMoRenName.Value = DeptDP.GetDeptName(long.Parse(Session["UserOrgID"].ToString()));
                }

                DeptID = long.Parse(Session["UserDeptID"].ToString());
                DeptName = Session["UserDeptName"].ToString();
            }
        }
        /// <summary>
        /// 设置未输入时的提示信息
        /// </summary>
        public string TextToolTip
        {
            set { strMessage = value; }
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
                isMoren();
            }
            if (blnMust == false)
            {
                rWarning.Visible = false;
            }
        }
    }
}