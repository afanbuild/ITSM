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
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// frmViewRights 的摘要说明。
	/// </summary>
	public partial class frmViewRights : BasePage
	{

		protected string GetRightName(string sright)
		{
			if(sright == "True")
			{
				return "■";
			}
			else
			{
				return "";
			}
		}

		protected string GetRangeName(string srange,string strList)
		{
			string strRet = "";
			//eO_RightRange range = (eO_RightRange)int.Parse(srange);
			switch(srange)
			{
				case "eDept":
					strRet = "所属部门";
					break;
				case "eDeptDirect":
					strRet = "所在部门";
					break;
				case "eFull":
					strRet = "全局";
					break;
				case "eOrg":
					strRet = "所属机构";
					break;
				case "eOrgDirect":
					strRet = "所在机构";
					break;
				case "ePersonal":
					strRet = "个人";
					break;
			}
			if(strList != "")
			{
				string strNames = DeptDP.GetDeptListNames(strList);
				if(strNames != "")
				{
					strRet = strRet + "(扩展:" + strNames + ")";
				}
			}

			return strRet;
		}
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			long lngUserID = StringTool.String2Long(Request.QueryString["UserID"]);
			if(!Page.IsPostBack)
			{
				lblName.Text = UserDP.GetUserName(lngUserID);
				Hashtable ht=RightDP.getUserRightTable(lngUserID);
				
				rptRights.DataSource=ht;
				rptRights.DataBind();
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
