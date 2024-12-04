/*******************************************************************
 * 版权所有：
 * Description：自定义桌面展示
 * 
 * 
 * Create By  ：
 * Create Date：2010-04-10
 * *****************************************************************/
using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using EpowerCom;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;


namespace Epower.ITSM.Web.NewOldMainPage
{
    /// <summary>
    /// NewmainDefine 的摘要说明。
    /// </summary>
    public partial class NewmainDefine : BasePage
    {
        #region 页面加载 Page_Load
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (!Page.IsPostBack)
            {
               
                string MainPageSet = UserDP.GetUserDeskDefineById((long)Session["UserID"]);
                Ea_DefineMainPageDP ee = new Ea_DefineMainPageDP();
                string sLeft = string.Empty;
                string sRight = string.Empty;
                Hashtable htAllRights = (Hashtable)Session["UserAllRights"];
                RightEntity re = (RightEntity)htAllRights[Constant.UserDeskTopDefined];

                string sExpand = "";
                string sCookiesID = Constant.UserDeskTopCookiesKey + Session["UserID"].ToString() + "_Expand";
                HttpCookie cookie = Request.Cookies[sCookiesID];
                if (cookie != null)
                {
                    sExpand = cookie.Value.ToString();
                }

                ee.CreateMainPage(MainPageSet, sExpand, ref sLeft, ref sRight, re); 
                ltlLeft.Text = sLeft ;
                ltlRight.Text = sRight;

                if (re != null)
                {
                    if (re.CanRead == true)
                    {
                        this.userdefined.Value = "true";
                    }
                }
            }
        }
        #endregion 

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

            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion
    }
}
