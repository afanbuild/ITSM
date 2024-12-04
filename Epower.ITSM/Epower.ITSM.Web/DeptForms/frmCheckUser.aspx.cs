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

using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization;
using Epower.DevBase.Organization.SqlDAL;


namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// frmCheckUser 的摘要说明。
	/// </summary>
	public partial class frmCheckUser : BasePage
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!this.IsPostBack)
			{
				if(this.Request.QueryString["LoginName"]!=null)
				{
					
					string sLoginName=this.Request.QueryString["LoginName"].ToString();
					UserEntity ue=new UserEntity();
					if(ue.CheckUser(sLoginName))
					{

						tbTips.Rows[0].Cells[0].Align="Left";
						lblTips.Text ="<font color=#ff0066>登陆帐号已被其他人占用！</font><br><br>" +
							"帐号："+sLoginName+"<br>"+
							"姓名："+ ue.Name +"<br>"+
							"性别："+ ue.Sex +"<br>"+
							"职位："+ ue.Job +"<br>"+
							"电话："+ ue.TelNo +"<br>"+
							//"手机："+ue.Mobile +"<br>"+
							"Email："+ ue.Email +"<br>"+
							"学历："+ ue.EduLevel +"<br>"+
							"部门："+ ue.FullDeptName +"<br>"+
							"状态："+ ((ue.Deleted==0)? "启用":"禁用");
							
					}
					else
					{
						tbTips.Rows[0].Cells[0].Align="Center";
						lblTips.Text="<font color=green>登陆帐号可用。</font>";
					}				
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
