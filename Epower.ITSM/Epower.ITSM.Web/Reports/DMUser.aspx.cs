/********************************************************%
 * description:模拟登录XMLHTTP调用页面
 * 
 * 
 * create by :zhumingchun
 * create date:2006-12-04
 * ******************************************************/
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Xml;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using Epower.ITSM.SqlDAL;
namespace test
{
	/// <summary>
	/// DMUser 的摘要说明。
	/// </summary>
	public partial class DMUser : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			if(Session["UserID"]!=null)
			{
                string reportuser = CommonDP.GetConfigValue("PrintMode", "ReportUser"); 
                string reportpwd = CommonDP.GetConfigValue("PrintMode", "ReportPwd"); 
                string reporturl = CommonDP.GetConfigValue("PrintMode", "ReportServer");
				ReturnData(reportuser+";"+reportpwd+ ";" + reporturl);
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

		/// <summary>
		/// 返回调用数据
		/// </summary>
		/// <param name="strText"></param>
		private void ReturnData(string strText)
		{
			XmlTextWriter writer = new XmlTextWriter(Response.OutputStream, Response.ContentEncoding);
			writer.Formatting = Formatting.Indented;
			writer.Indentation = 4;
			writer.IndentChar = ' ';
			writer.WriteString(strText);
			writer.Flush();
			Response.End();
			writer.Close();
		}

	}
}
