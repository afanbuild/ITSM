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
using System.Configuration ;

using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.NewOldMainPage
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

        protected string BackUrl
        {
            get { return (string)Session["BackUrl"] ?? string.Empty; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Page_Load(object sender, System.EventArgs e)
		{
            if (string.IsNullOrEmpty(BackUrl))
            {
                // 在此处放置用户代码以初始化页面
                if (!Page.IsPostBack)
                {
                    LoadMenusHtml();
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
		}

        private void LoadMenusHtml()
        {

            string strTitle = "";
            string strMenus = "";
            string strUserName = Session["UserName"].ToString().Trim();
            //Epower.ITSM.SqlDAL.UIMethod ui = new Epower.ITSM.SqlDAL.UIMethod();

            if (strUserName.ToLower() == "sa")
            {
                //ui.GetAdminOnlyMenuHtml(Session["UserName"].ToString(), ref strTitle, ref strMenus);
                new MenuService().GetAdminOnlyMenuHtml(Session["UserName"].ToString(), ref strTitle, ref strMenus);
                strTitle = "";
            }
            else
            {
                new MenuService().GetMenuHtml2(Session["UserName"].ToString(), (long)Session["UserID"], ref strMenus);  
            }
            //FirstLeftUrl = strTitle;
            elSixParent.InnerHtml = strMenus;
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
