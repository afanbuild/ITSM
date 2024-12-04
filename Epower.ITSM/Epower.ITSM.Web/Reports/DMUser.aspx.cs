/********************************************************%
 * description:ģ���¼XMLHTTP����ҳ��
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
	/// DMUser ��ժҪ˵����
	/// </summary>
	public partial class DMUser : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			if(Session["UserID"]!=null)
			{
                string reportuser = CommonDP.GetConfigValue("PrintMode", "ReportUser"); 
                string reportpwd = CommonDP.GetConfigValue("PrintMode", "ReportPwd"); 
                string reporturl = CommonDP.GetConfigValue("PrintMode", "ReportServer");
				ReturnData(reportuser+";"+reportpwd+ ";" + reporturl);
			}
			

		}

		#region Web ������������ɵĴ���
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: �õ����� ASP.NET Web ���������������ġ�
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion

		/// <summary>
		/// ���ص�������
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
