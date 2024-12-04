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
	/// flow_Normal 的摘要说明。
	/// </summary>
	public partial class flow_Normal : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			long lngMessageID = long.Parse(Request.QueryString["MessageID"]);
            string strAutoPass = "false";

            //2007-09-15 记录 调度后 的页面
            Session["AttemperBackUrl"] = "flow_Normal.aspx?MessageID=" + lngMessageID.ToString();

			string strUrl = "";
			
			if(Request.QueryString["OUrl"] != null)
			{
				strUrl = HttpUtility.UrlDecode(Request.QueryString["OUrl"]);
			}

            if (Request.QueryString["autopass"] != null)
            {
                strAutoPass = Request.QueryString["autopass"];
            }
            Session["g_FlowModelAutoPass"] = strAutoPass;

            if (Request.QueryString["OUrl"] != null)
            {
                strUrl = HttpUtility.UrlDecode(Request.QueryString["OUrl"]);
            }

			// 特殊处理需求　：　从待办事项进入的处理返回　待办事项
			if(strUrl.IndexOf("FrmContent.aspx") == 0)
			{
				Session["BackToUndoList"] = true;
				
			}
			else
			{
				Session["BackToUndoList"] = false;
			}

			Session["BackToUrl"] = (Session["FromUrl"]==null)?strUrl:Session["FromUrl"];

			MessageEntity msgObject = new MessageEntity(lngMessageID);


			Session["MessageObject"] = msgObject;

			

			
			Message.SetMessageReadStatus(lngMessageID);



			if(msgObject.MessageID == 0)
			{
				UIGlobal.MsgBox(this,"待办事项不存在！");

			}
			


			//if(lngActorType == e_ActorClass.fmMasterActor)
			if(msgObject.ActorType == e_ActorClass.fmMasterActor)
			{
				if(Session["NormalPortal"] == null)
				{
					Session["NormalPortal"] = true;
				}
				if((bool)Session["NormalPortal"] == true)
				{
					Session["NormalPortal"] = false;
					Response.Redirect("OA_Normal.htm");
					
				}
				else
				{
					Session["NormalPortal"] = true;
					Response.Redirect("OA_Normal11.htm");
					
				}
			}
			else
			{
				//阅知，协办　会签全部从这里进入
				Response.Redirect("OA_Reader.htm");  
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
