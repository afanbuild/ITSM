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
    public partial class frmpopdeptBranch : BasePage
	{
		protected long lngCurrDeptID = 0;
		protected void Page_Load(object sender, System.EventArgs e)
		{

            try
            {
                if (Request.QueryString["SubBankID"] != null)
                {
                    if (Request.QueryString["SubBankID"].Length > 0)
                    {
                        lngCurrDeptID = long.Parse(Request.QueryString["SubBankID"]);
                        if (lngCurrDeptID == 0)
                        {
                            lngCurrDeptID = 1;
                        }
                    }
                    else
                    {
                        lngCurrDeptID = 0;
                    }
                }
            }
            catch
            { }

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
