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
	/// flow_Read_menu_Finished 的摘要说明。
	/// </summary>
	public partial class OA_Read_menu_Finished : System.Web.UI.Page
	{
        public string sApplicationUrl = Epower.ITSM.Base.Constant.ApplicationPath;     //虚拟路径

		public long lngMessageID = 0;

		protected void Page_Load(object sender, System.EventArgs e)
		{
            // //在此处放置用户代码以初始化页面
            MessageEntity msgObject = (MessageEntity)Session["MessageObject"];
            lngMessageID = msgObject.MessageID;		
			
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
			this.ID = "OA_Read_menu_Finished";

		}
		#endregion
	}
}
