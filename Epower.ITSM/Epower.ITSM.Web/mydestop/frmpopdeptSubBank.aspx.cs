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

namespace  Epower.ITSM.Web.MyDestop
{
	/// <summary>
	/// frmPopDept ��ժҪ˵����
	/// </summary>
    public partial class frmpopdeptSubBank : BasePage
	{
		protected long lngCurrDeptID = 1;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(Request.QueryString["CurrDeptID"] != null)
			{
				if (Request.QueryString["CurrDeptID"].Length >0)
					lngCurrDeptID = long.Parse(Request.QueryString["CurrDeptID"]);
				else
					lngCurrDeptID=1;
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
