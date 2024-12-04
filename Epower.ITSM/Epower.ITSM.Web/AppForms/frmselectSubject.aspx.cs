using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace Epower.ITSM.Web.AppForms
{
    public partial class frmselectSubject :BasePage
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            // 在此处放置用户代码以初始化页面
            if (Request.QueryString["LimitCurr"] != null)
            {
                CtrSubjecttree1.LimitCurr = bool.Parse(Request.QueryString["LimitCurr"]);
            }
            // 在此处放置用户代码以初始化页面
            if (Request.QueryString["CurrSubjectID"] != null)
            {
                CtrSubjecttree1.CurrSubjectID = long.Parse(Request.QueryString["CurrSubjectID"]);
                // 记录当前部门
                //  Session["OldTemplateID"] = Equ_ServerDP.GetServerByID(CtrSubjecttree1.CurrSubjectID).Rows[0]["ServiceLevelID"].ToString();
            }
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
