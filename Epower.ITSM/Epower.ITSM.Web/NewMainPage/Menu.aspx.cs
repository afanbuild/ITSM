/*******************************************************************
 * 版权所有：
 * Description：菜单
 * 
 * 
 * Create By  ：
 * Create Date：2010-04-10
 * *****************************************************************/
using System;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;


using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.NewMainPage
{
    /// <summary>
    /// Menu 的摘要说明。
    /// </summary>
    public partial class Menu : BasePage
    {
        /// <summary>
        /// 
        /// </summary>
        protected string FirstLeftUrl
        {
            get { return ViewState["firstlefturl"].ToString(); }
            set { ViewState["firstlefturl"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            // 在此处放置用户代码以初始化页面
            if (!Page.IsPostBack)
            {
                if (Session["UserName"].ToString().Trim() == "sa")
                {
                    Response.Redirect("../NewOldMainPage/index.aspx");
                }
            }


            #region 显示需要弹出的公告

            if (ConfigurationManager.AppSettings["PopupAlertWindowWhenLogin"] == null || ConfigurationManager.AppSettings["PopupAlertWindowWhenLogin"].Equals("0"))
            {
                return;
            }

            //显示需要弹出的公告
            DataTable dt = NewsDp.GetIsAlterNews(StringTool.String2Long(Session["UserOrgID"].ToString()), StringTool.String2Long(Session["UserDeptID"].ToString()));
            if (dt != null && dt.Rows.Count > 0)
            {
                literalPopupWindow.Visible = true;
            }
            #endregion
        }

        #region Web 窗体设计器生成的代码
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
        }
        #endregion
    }
}
