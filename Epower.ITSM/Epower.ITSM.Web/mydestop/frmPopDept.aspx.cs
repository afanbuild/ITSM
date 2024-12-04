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

namespace  Epower.ITSM.Web.MyDestop
{
	/// <summary>
	/// frmPopDept 的摘要说明。
	/// </summary>
    public partial class frmPopDept : BasePage
	{
        #region 是否限制选择范围
        /// <summary>
        /// 
        /// </summary>
        private bool mIsLimit = false;
        /// <summary>
        /// 是否限制部门范围
        /// </summary>
        public bool IsLimit
        {
            get
            {
                if (Request.QueryString["LimitCurr"] != null && Request.QueryString["LimitCurr"].ToLower() == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion 

        /// <summary>
        /// 获取路径里参数
        /// </summary>

        public string TypeFrm
        {
            get{
               
                if (Request.QueryString["TypeFrm"] != null)
                {
                    return Request.QueryString["TypeFrm"];
                }
                else
                {
                    return "";
                }
            }
        }
        public string Opener_ClientId
        {
           
            get
            {
                return (Request["Opener_ClientId"] == null) ? "" : Request["Opener_ClientId"].ToString().Replace("hidClientId_ForOpenerPage", "");
            }
        }

        private long mlngCurrDeptID = 1;
        protected long lngCurrDeptID 
        {
            get
            {
                if (Request.QueryString["CurrDeptID"] != null)
                {
                    if (Request.QueryString["CurrDeptID"].Length > 0)
                        mlngCurrDeptID = long.Parse(Request.QueryString["CurrDeptID"]);
                    else
                        mlngCurrDeptID = long.Parse(Session["UserDeptID"].ToString());
                }
                else
                {
                    mlngCurrDeptID = long.Parse(Session["UserDeptID"].ToString());
                }
                return mlngCurrDeptID;
            }
        }
		protected void Page_Load(object sender, System.EventArgs e)
		{
            if (!IsPostBack) 
            {
                if (Request["TypeFrm"] != null)
                {
                   // TypeFrm = Request.QueryString["TypeFrm"].ToString();
                }
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
