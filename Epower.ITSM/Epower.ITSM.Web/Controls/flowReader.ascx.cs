/*******************************************************************
 * 版权所有：
 * Description：工作流读控件
 * 
 * 
 * Create By  ：
 * Create Date：
 * *****************************************************************/
namespace Epower.ITSM.Web.Controls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		flowReader 的摘要说明。
	/// </summary>
	public partial  class flowReader : System.Web.UI.UserControl
	{

		protected string _SendType = "Handle";
		public string SendType
		{
			get{return _SendType;}
			set{_SendType = value;}
		}

		protected bool GetVisible()
		{
			return  (_SendType == "Handle")?true:false;
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面

		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN：该调用是 ASP.NET Web 窗体设计器所必需的。
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		///		设计器支持所需的方法 - 不要使用
		///		代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion
	}
}
