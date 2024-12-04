/*******************************************************************
 * 版权所有：
 * Description：页面基类，所有的页面继承此页面
 * 
 * 
 * Create By  ：
 * Create Date：2008-08-20
 * *****************************************************************/
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web;
using System.Text;

using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization;
using Epower.DevBase.Organization.SqlDAL;
using System.Collections;

namespace Epower.ITSM.Web
{
    /// <summary>
    /// BasePage
    /// </summary>
    public class BasePage : System.Web.UI.Page
    {
        #region 变量区
        // 是否显示进度条
        bool mShowProgressBar = true;

        /// <summary>
        /// 权限对象
        /// </summary>
        private RightEntity _re;

        #endregion 变量区结束

        #region 属性区
        #region #BaseRoot:string 应用程序根路径（虚拟路径）
        /// <summary>
        /// 应用程序根路径（虚拟路径）
        /// </summary>
        protected string BaseRoot
        {
            get { return Request.ApplicationPath; }
        }
        #endregion
        #endregion 属性区结束

        #region BasePage
        /// <summary>
        /// constructor
        /// </summary>
        public BasePage()
        {

        }



        #endregion

        #region Page_PreInit
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Session["UserID"] == null) { Response.Redirect("~/default.aspx"); }

            if (Session["Themes"] == null)
            {
                string js = "<script type=\"javascript\">top.location.href = \"../default.aspx\";</script>";
                Page.RegisterStartupScript("redirectLogin", js);
                return;
            }
            this.Page.Theme = Session["Themes"].ToString();
        }

        protected override void OnError(EventArgs e)
        {
            HttpContext ctx = HttpContext.Current;

            Exception exception = ctx.Server.GetLastError();

            StringBuilder sbErrInfo = new StringBuilder();
            sbErrInfo.Append(Environment.NewLine + "=====================================" + System.DateTime.Now.ToString() + "=============================================");
            sbErrInfo.Append(Environment.NewLine + "当前用户名: " + Session["UserName"] == null ? "empty" : Session["UserName"].ToString());
            sbErrInfo.Append(Environment.NewLine + "Offending URL: " + ctx.Request.Url.ToString());
            sbErrInfo.Append(Environment.NewLine + "Source: " + exception.Source);
            sbErrInfo.Append(Environment.NewLine + "Message: " + exception.Message);
            sbErrInfo.Append(Environment.NewLine + "Stack trace: " + exception.StackTrace);


            E8Logger.Error(sbErrInfo.ToString());
            sbErrInfo.Remove(0, sbErrInfo.ToString().Length);
            // --------------------------------------------------
            // To let the page finish running we clear the error
            // --------------------------------------------------
            ctx.Server.ClearError();
            HttpContext.Current.Response.Redirect(HttpContext.Current.Request.ApplicationPath + "/Error.aspx", true);
            base.OnError(e);
        }

        #endregion


        #region 母版页

        /// <summary>
        /// 非流程母版页对象
        /// </summary>
        protected EpowerMasterPage NormalMaster
        {
            get
            {
                return this.Master as EpowerMasterPage;
            }
        }

        /// <summary>
        /// 流程母版页对象
        /// </summary>
        protected FlowForms FlowMaster
        {
            get
            {
                return this.Master as FlowForms;
            }
        }

        #endregion

        #region 操作权限

        /// <summary>
        /// 设置权限
        /// </summary>
        /// <param name="lngOperateID">操作项编号</param>
        protected void SetupRight(long lngOperateID)
        {
            _re = GetRight(lngOperateID);
        }

        /// <summary>
        /// 设置权限
        /// </summary>
        /// <param name="lngOperateID">操作项编号</param>
        /// <param name="isVerifyComeinRight">验证进入页面权限</param>
        protected void SetupRight(long lngOperateID, bool isVerifyComeinRight)
        {
            _re = GetRight(lngOperateID);

            if (isVerifyComeinRight)
            {
                VerifyComeInRight();
            }
        }

        /// <summary>
        /// 取权限对象
        /// </summary>
        /// <param name="lngOperateID">操作项编号</param>
        /// <returns></returns>
        protected RightEntity GetRight(long lngOperateID)
        {
            return (RightEntity)((Hashtable)Session["UserAllRights"])[lngOperateID];
        }

        /// <summary>
        /// 验证进入页面的权限
        /// </summary>
        /// <returns></returns>
        protected void VerifyComeInRight()
        {
            if (_re != null)
            {
                if (!this.CanRead())
                    throw new Exception(String.Format("权限不够。url: {0}", Request.Url.ToString()));
            }
        }

        /// <summary>
        /// 是否可见
        /// </summary>
        /// <returns></returns>
        protected bool CanAdd()
        {
            return _re.CanAdd;
        }

        /// <summary>
        /// 是否可读
        /// </summary>
        /// <returns></returns>
        protected bool CanRead()
        {
            return _re.CanRead;
        }

        /// <summary>
        /// 是否可编辑
        /// </summary>
        /// <returns></returns>
        protected bool CanModify()
        {
            return _re.CanModify;
        }

        /// <summary>
        /// 是否可删除
        /// </summary>
        /// <returns></returns>
        protected bool CanDelete()
        {
            return _re.CanDelete;
        }

        #endregion
    }
}
