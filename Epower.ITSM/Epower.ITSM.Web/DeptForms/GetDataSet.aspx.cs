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
using Epower.DevBase.Organization.SqlDAL;



namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// GetXML 的摘要说明。
	/// </summary>
	public partial class GetDataSet : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(this.Request["Param"]!=null)
			{				
				ReturnData(Get_DataSet(Request["Param"].ToString()));
				
			}

		}


		private DataSet Get_DataSet(string Param)
		{
			string[] arrayParam = Param.Split(new char[] {'\n'});
			DataSet ds=new DataSet();

			switch(arrayParam[0])
			{
				case "Get_Dept_Manager_And_Leader":	//获取部门成员
					DataTable dt=DeptDP.Get_Dept_Manager_And_Leader(arrayParam[1]);
					if (dt!=null)
						ds.Tables.Add(dt);
					break;			
			}

			return ds;
		}


		private void ReturnData(DataSet ds)
		{
			XmlTextWriter writer = new XmlTextWriter(Response.OutputStream, Response.ContentEncoding);
			writer.Formatting = Formatting.Indented;
			writer.Indentation = 4;
			writer.IndentChar = ' ';
			ds.WriteXml(writer);
			writer.Flush();
			Response.End();
			writer.Close();
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
