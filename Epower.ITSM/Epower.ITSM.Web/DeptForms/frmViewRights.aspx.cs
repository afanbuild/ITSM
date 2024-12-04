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
	/// frmViewRights ��ժҪ˵����
	/// </summary>
	public partial class frmViewRights : BasePage
	{

		protected string GetRightName(string sright)
		{
			if(sright == "True")
			{
				return "��";
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
					strRet = "��������";
					break;
				case "eDeptDirect":
					strRet = "���ڲ���";
					break;
				case "eFull":
					strRet = "ȫ��";
					break;
				case "eOrg":
					strRet = "��������";
					break;
				case "eOrgDirect":
					strRet = "���ڻ���";
					break;
				case "ePersonal":
					strRet = "����";
					break;
			}
			if(strList != "")
			{
				string strNames = DeptDP.GetDeptListNames(strList);
				if(strNames != "")
				{
					strRet = strRet + "(��չ:" + strNames + ")";
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
	}
}
