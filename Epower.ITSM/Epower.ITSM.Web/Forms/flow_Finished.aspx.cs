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
using EpowerCom;
using EpowerGlobal;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// flow_Finished 的摘要说明。
	/// </summary>
	public partial class flow_Finished : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			long lngMessageID = long.Parse(Request.QueryString["MessageID"]);
			//string strUrl = HttpUtility.UrlDecode(Request.QueryString["OUrl"]);

            //2007-09-15 记录 调度后 的页面
            Session["AttemperBackUrl"] = "flow_Finished.aspx?MessageID=" + lngMessageID.ToString();



			MessageEntity msgObject = new MessageEntity(lngMessageID);

			Session["MessageObject"] = msgObject;

			if(msgObject.MessageID == 0)
			{
				UIGlobal.MsgBox(this,"待办事项不存在！");

			}
			
			if(msgObject.ActorType == e_ActorClass.fmMasterActor)
			{
				Response.Redirect("OA_Finished.htm");
			}
			else
			{
				Response.Redirect("OA_Finished_Read.htm");
			}

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
