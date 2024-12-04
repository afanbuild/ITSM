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
	/// flow_Receive_submit 的摘要说明。
	/// </summary>
	public partial class flow_Receive_submit : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			long lngUserID;

			long lngMessageID =0;
			

			bool blnSuccess = true;



			string strMessage = "";

			lngUserID = (long)Session["UserID"];

			lngMessageID = long.Parse(Request.Form["MessageID"]);

            long lngAppID = long.Parse(Request.Form["AppID"]);
			

			//防止用户通过IE后退按纽重复提交
			Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);

			try
			{
                blnSuccess = ReceiveList.ReceiveMessage(lngMessageID, lngUserID, lngAppID);
			}
			catch(Exception eSend)
			{
				strMessage =eSend.Message;
				blnSuccess = false;
			}

			
			
			if(blnSuccess == true)
			{
				//接收后进入处理页面　
				Response.Redirect("flow_Normal.aspx?MessageID="  + lngMessageID.ToString());
			
			}
			else
			{
				if(strMessage != "")
				{
					PageTool.MsgBox(this,strMessage);
				}
				else
				{
					PageTool.MsgBox(this,"此事项已经被其它用户接收");
					Response.Write("<script>window.history.back();</script>");
					//Response.Redirect("frmContent.aspx");
					return;
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
