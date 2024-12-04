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
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// flow_Finished_submit 的摘要说明。
	/// </summary>
	public partial class flow_Finished_submit : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面

			long lngUserID;

			long lngMessageID =0;

			bool blnSuccess = true;



			string strMessage = "";

			lngUserID = (long)Session["UserID"];

			lngMessageID = long.Parse(Request.Form["MessageID"]);
		

			//防止用户通过IE后退按纽重复提交
			Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);

			try
			{
				
				Message objMsg= new Message();

				objMsg.TakeBackFlow(lngUserID,lngMessageID);
			
			}
			catch(Exception eSend)
			{
				//strMessage = MyGlobalString.ParseForResponse(eSend.Message.Replace("\n",@"\n"));
				strMessage =eSend.Message;
				blnSuccess = false;
			}

			
			
			
			if(blnSuccess == true)
			{
				//UIGlobal.MsgBox(this,"流程已经回收！");
				//回收后进入当前消息的处理状态
				//Response.Write("<script>window.opener.parent.location='flow_Normal.aspx?MessageID=" + lngMessageID.ToString() +"';</script>");
				Response.Redirect("flow_Normal.aspx?MessageID=" + lngMessageID.ToString());
				
			}
			else
			{
				if(strMessage != "")
				{
					//出现返回异常的情况
					PageTool.MsgBox(this,strMessage.Replace("\r\n",","));
					Response.Write("<script>window.history.back();</script>");
				}
			}
			//Response.Write("<script>window.opener.parent.opener.location.reload();</script>");
			//Response.Write("<script>window.opener.parent.close();</script>");

			

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
