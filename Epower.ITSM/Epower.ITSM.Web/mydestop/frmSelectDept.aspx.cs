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
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.MyDestop
{
	/// <summary>
	/// frmSelectDept 的摘要说明。
	/// </summary>
    public partial class frmSelectDept : BasePage
	{
        
		protected void Page_Load(object sender, System.EventArgs e)
		{

			if(CtrDeptTree1 != null)
			{

				// 在此处放置用户代码以初始化页面
                if (Request.QueryString["LimitCurr"] != null && Request.QueryString["LimitCurr"].ToLower()=="true")
				{
					string strTmp = Request.QueryString["LimitCurr"];
					CtrDeptTree1.LimitCurr = bool.Parse(strTmp);
                    //CtrDeptTree1.IsPower = long.Parse(Session["RootDeptID"].ToString());
					//CtrDeptTree1.LimitCurr = true;
				}
				if(Request.QueryString["CurrDeptID"] != null)
				{
					CtrDeptTree1.CurrDeptID = long.Parse(Request.QueryString["CurrDeptID"]);

					// 记录当前部门
					//Session["OldDeptID"] = DeptDP.GetDeptParentID(CtrDeptTree1.CurrDeptID
                    Session["OldDeptID"] = CtrDeptTree1.CurrDeptID;
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
