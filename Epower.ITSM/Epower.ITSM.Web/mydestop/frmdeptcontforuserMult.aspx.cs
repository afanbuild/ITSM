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

namespace Epower.ITSM.Web.MyDestop
{
	/// <summary>
    /// frmdeptcontforuserMult 的摘要说明。
	/// </summary>
    public partial class frmdeptcontforuserMult : BasePage
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			if(!this.IsPostBack)
			{
				string DeptID="0";
                if (Request.QueryString["LimitCurr"] != null && Request.QueryString["LimitCurr"].ToLower() == "true")
                {
                    CtrDeptTree1.IsPower = long.Parse(Session["RootDeptID"].ToString());
                }
                else
                {
                    CtrDeptTree1.IsPower = 1;
                }
				if(Session["UserDeptID"] !=null)
				{
					DeptID=Session["UserDeptID"].ToString();
					if(Session["OldDeptID"] !=null)
					{
						DeptID=Session["OldDeptID"].ToString();
					}
                    Response.Write("<script>window.parent.document.all.DeptID="+DeptID+"</script>");
                   // Response.Write("<SCRIPT>window.parent.userinfo.location='frmUserQueryMult.aspx?DeptID=" + DeptID + "&LimitCurr=" + IsLimit.ToString() + "';</SCRIPT>");
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
