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
using Epower.DevBase.Organization;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// frmDeptQuery 的摘要说明。
	/// </summary>
	public partial class frmDeptQuery : System.Web.UI.Page
	{
	


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


		#region Custom Function

		protected string GetDeptName(string DeptID,string DeptName,string DeptKind,string IsTemp)
		{
			string strImage,strDeptName;

			if( int.Parse(DeptKind)== (int)eO_DeptKind.eOrg )
			{
				strImage="<IMG alt='机构' src='..\\Images\\root.gif'>";
			}
			else
			{
				if(IsTemp.Equals("0"))
				{
					strImage="<IMG alt='部门' src='..\\Images\\1.ico'>";
				}
				else
				{
					strImage="<IMG alt='临时部门' src='..\\Images\\2.ico'>";
				}
			}

			strDeptName=string.Format(" <A onclick=\"EditDept('{0}','{1}')\" href=\"#\">{1}</A>",DeptID,DeptName);

			return strImage + strDeptName;
		}


		private DataTable Exec_Query()
		{
			string[][] arrayQueryParam = new string[2][];
													  
			for (int i = 0; i < arrayQueryParam.Length; i++) 
			{
				arrayQueryParam[i] = new string[2];
			}


			//部门ID
			arrayQueryParam[0][0]="DeptID";
			if(txtDeptID.Text.Trim().Length>0)
			{
				arrayQueryParam[0][1]=txtDeptID.Text.Trim();
			}
			else
			{
				arrayQueryParam[0][1]="";
			}

			//部门名
			arrayQueryParam[1][0]="DeptName";
			if(txtDeptName.Text.Trim().Length>0)
			{
				arrayQueryParam[1][1]=txtDeptName.Text.Trim();
			}
			else
			{
				arrayQueryParam[1][1]="";
			}

			string strRangeID=Session["RangeID"].ToString();

			DataTable dt= DeptDP.QueryDept(arrayQueryParam,strRangeID);
			return dt;
			
		}


		private void LoadData()
		{
			DataTable dt = Exec_Query();
			Session["DeptInfo"] = dt;

			ControlPage1.Visible =true;
		}

		private void BindData()
		{
			DataTable dt=(DataTable)Session["DeptInfo"];
			this.dgDeptInfo.DataSource = dt.DefaultView;
			this.dgDeptInfo.DataBind();
		}
	


		#endregion


		protected void Page_Load(object sender, System.EventArgs e)
		{
			ControlPage1.On_PostBack+=new EventHandler(ControlPage1_On_PostBack);
			ControlPage1.DataGridToControl=dgDeptInfo;

		}

		protected void btnQuery_Click(object sender, System.EventArgs e)
		{
			LoadData();
			BindData();
		}

		private void ControlPage1_On_PostBack(object sender, EventArgs e)
		{
			//LoadData();
			BindData();
		}

	}
}
