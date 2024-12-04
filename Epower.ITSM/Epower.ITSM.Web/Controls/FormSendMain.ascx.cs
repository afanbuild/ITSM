/*******************************************************************
 * 版权所有：
 * Description：工作流处理控件
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
	using EpowerCom;
	using EpowerGlobal;

	/// <summary>
	///		FormSendMain 的摘要说明。
	/// </summary>
	public partial  class FormSendMain : System.Web.UI.UserControl
	{

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

		protected override void Render(HtmlTextWriter writer)
		{
			//添加隐含字段信息
			//if (!Page.IsPostBack)
			UIGlobal.AddHiddenField(writer,"Subject");
			UIGlobal.AddHiddenField(writer,"FlowModelID","0");
			UIGlobal.AddHiddenField(writer,"MessageID","0");
            UIGlobal.AddHiddenField(writer, "FlowID", "0");
			UIGlobal.AddHiddenField(writer,"ActionID","-1");
			UIGlobal.AddHiddenField(writer,"FormXMLValue");
            UIGlobal.AddHiddenField(writer, "FormDefineValue");
			UIGlobal.AddHiddenField(writer,"OpinionValue");
			UIGlobal.AddHiddenField(writer,"Attachment");
			UIGlobal.AddHiddenField(writer,"Receivers");
			UIGlobal.AddHiddenField(writer,"Importance");
			UIGlobal.AddHiddenField(writer,"LinkNodeID");
			UIGlobal.AddHiddenField(writer,"LinkNodeType");
		    UIGlobal.AddHiddenField(writer,"strMessage");
			UIGlobal.AddHiddenField(writer,"SubmitType","Send");
			UIGlobal.AddHiddenField(writer,"HasSaved","false");
			UIGlobal.AddHiddenField(writer,"SpecRightType",((int)e_SpecRightType.esrtNormal).ToString());   //保存特殊处理的隐含字段
			UIGlobal.AddHiddenField(writer,"JumpToNodeModel","0");   //保存跳转到的环节模型ID的隐含字段

            UIGlobal.AddHiddenField(writer, "NodeID", "0");
		}
	}
}
