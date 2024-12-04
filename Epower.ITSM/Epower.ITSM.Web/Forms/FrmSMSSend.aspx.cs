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
using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// FrmSMSSend 的摘要说明。
	/// </summary>
	public partial class FrmSMSSend : BasePage
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			CtrTitle1.Title="发送短消息";
            if (!IsPostBack)
            {
                if (Request["UserID"] != null)
                {
                    UserPicker1.UserID = long.Parse(Request["UserID"].ToString());
                    UserPicker1.UserName = UserDP.GetUserName(long.Parse(UserPicker1.UserID.ToString().Trim()));
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

		protected void cmdSend_Click(object sender, System.EventArgs e)
		{
            if (UserPicker1.UserID.ToString() == "" || UserPicker1.UserID.ToString() == "0")
			{
                PageTool.MsgBox(Page, "请选择接收人!");
				return;
			}

            if (txtContent.Text.Trim() == string.Empty)
            {
                PageTool.MsgBox(Page, "请输入消息内容!");
                return;
            }

            SMSDp.SaveSMS(0, (long)Session["UserID"], UserPicker1.UserID, txtContent.Text.Trim());

			PageTool.SelfClose(this);
			
			
		}
	}
}
