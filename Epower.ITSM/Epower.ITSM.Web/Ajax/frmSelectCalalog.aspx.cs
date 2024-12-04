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

namespace Epower.ITSM.Web.Ajax
{
	/// <summary>
	/// frmSelectDept 的摘要说明。
	/// </summary>
    public partial class frmSelectCalalog : BasePage
	{
        private long mlngIsPoint = 0;
        protected long IsPoint
        {
            get
            {
                if (Request.QueryString["IsPoint"] != null)
                {
                    mlngIsPoint = long.Parse(Request.QueryString["IsPoint"] == "" ? "0" : Request.QueryString["IsPoint"]);
                }
                return mlngIsPoint;
            }
        }

		protected void Page_Load(object sender, System.EventArgs e)
		{

            if (ctrcatalogtreeNew1 != null)
			{
                if (Request.QueryString["RootID"] != null)
				{
                    ctrcatalogtreeNew1.lngRootID = long.Parse(Request.QueryString["RootID"]);
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
