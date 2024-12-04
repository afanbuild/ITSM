/****************************************************************************
 * 
 * description:服务台
 * 
 * 
 * 
 * Create by:
 * Create Date:2008-03-11
 * *************************************************************************/
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;

using Epower.ITSM.SqlDAL;
using EpowerCom;
using EpowerGlobal;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;

namespace Epower.ITSM.Web
{

    public partial class frm_mineDesktop : BasePage
    {
        #region 变量申明
        private long lngUserID = 0;
        #endregion

        #region 属性
        /// <summary>
        /// 服务权限
        /// </summary>
        protected string ServiceRights
        {
            get {
                if (CheckRight(Constant.CustomerService))
                {
                    return "true";
                }
                else
                {
                    return "false";
                }
            }
        }
        /// <summary>
        /// 投诉单登记
        /// </summary>
        protected string BYTSApplyRights
        {
            get
            {
                if (CheckRight(Constant.BytsApply))
                {
                    return "true";
                }
                else
                {
                    return "false";
                }
            }
        }
        /// <summary>
        /// 服务跟踪
        /// </summary>
        protected string CustomerServiceRights
        {
            get
            {
                if (CheckRight(Constant.CustomerService))
                {
                    return "true";
                }
                else
                {
                    return "false";
                }
            }
        }
        #endregion 

        #region 页面加载 Page_Load
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            lngUserID = long.Parse(Session["UserID"].ToString());
            if (!Page.IsPostBack)
            {
                DoDataBind();
                //ShowNewsInfo();
            }
        }
        #endregion 

        #region 我的事项 DoDataBind
        /// <summary>
        /// 绑定数据
        /// </summary>
        private void DoDataBind()
        {
            try
            {
                // 待接收事项
                int iRec = ReceiveList.GetReceiveMessageListCount(lngUserID);
                if (iRec > 0)
                {
                    Lab_a.Text = "<font color=red>[" + iRec.ToString() + "]" + "条待接收事项</font>";
                }

                //待办事项
                int iMsgUndo = MessageCollectionDP.GetUndoMessageCount(lngUserID);
                if (iMsgUndo > 0)
                {
 
                    Lab_b.Text = "<font color=red>[" + iMsgUndo.ToString() + "]" + "条待办事项</font>";
                }
            }
            catch
            {
                //统一错误展示页面
                throw;
                //Response.Redirect("ErrMessage.aspx?Souce="+e.Source + "&Desc=" + e.ToString());
            }
        }
        #endregion 

        #region 检查权限 CheckRight
        /// <summary>
        /// 检查权限
        /// </summary>
        /// <param name="OperatorID"></param>
        /// <returns></returns>
        private bool CheckRight(long OperatorID)
        {
            Epower.DevBase.Organization.SqlDAL.RightEntity re = (Epower.DevBase.Organization.SqlDAL.RightEntity)((Hashtable)Session["UserAllRights"])[OperatorID];
            if (re == null)
                return false;
            else
                return re.CanRead;
        }
        #endregion 
    }
}
