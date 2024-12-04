/*******************************************************************
 * 版权所有：
 * Description：工作流特殊控制，级别控制
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
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		CtrImportance 的摘要说明。
	/// </summary>
	public partial class CtrImportance : System.Web.UI.UserControl
	{


       #region 属性
		public int Importance
		{
			set{if(value <=2 && value >=0) rdoImportance.SelectedIndex = (2- value);}
			//get{return int.Parse(rdoImportance.Items[rdoImportance.SelectedIndex].Value);}
			get{return int.Parse(rdoImportance.SelectedValue);}
		}
		#endregion


		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
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
		///		设计器支持所需的方法 - 不要使用代码编辑器
		///		修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion
	}
}
