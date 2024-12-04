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
using Epower.ITSM.Base;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// flow_Normal_form 的摘要说明。
	/// </summary>
	public partial class flow_Normal_form : System.Web.UI.Page
	{
        //2009年1月 将 ReadOnly FlowProcessType等参数改为在母版页上计算

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面  ****  根据消息ID  未做 ************


			MessageEntity msgObject = (MessageEntity)Session["MessageObject"];
			long lngMessageID = msgObject.MessageID;
			//long lngUserID = long.Parse(Session["UserID"].ToString());

			string strPage = epApp.GetStartWebFormByMessageP(lngMessageID);

            //eOA_FlowProcessType lngFPT = eOA_FlowProcessType.efptReadFinished;  //缺省值

            if (Session["g_FlowModelAutoPass"] != null)
            {
                strPage = strPage + "?FlowModelID=0&MessageID=" + lngMessageID.ToString() + "&autopass=" + Session["g_FlowModelAutoPass"].ToString();
                //Session 仅用一次
                Session.Remove("g_FlowModelAutoPass");
            }
            else
            {
                strPage = strPage + "?FlowModelID=0&MessageID=" + lngMessageID.ToString();
            }
			Response.Redirect(strPage);
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
