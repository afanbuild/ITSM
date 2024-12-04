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
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.MyDestop
{
	/// <summary>
	/// frmSelectDept ��ժҪ˵����
	/// </summary>
    public partial class frmSelectDept : BasePage
	{
        
		protected void Page_Load(object sender, System.EventArgs e)
		{

			if(CtrDeptTree1 != null)
			{

				// �ڴ˴������û������Գ�ʼ��ҳ��
                if (Request.QueryString["LimitCurr"] != null && Request.QueryString["LimitCurr"].ToLower()=="true")
				{
					string strTmp = Request.QueryString["LimitCurr"];
					CtrDeptTree1.LimitCurr = bool.Parse(strTmp);
                    //CtrDeptTree1.IsPower = long.Parse(Session["RootDeptID"].ToString());
					//CtrDeptTree1.LimitCurr = true;
				}
				if(Request.QueryString["CurrDeptID"] != null)
				{
					CtrDeptTree1.CurrDeptID = long.Parse(Request.QueryString["CurrDeptID"]);

					// ��¼��ǰ����
					//Session["OldDeptID"] = DeptDP.GetDeptParentID(CtrDeptTree1.CurrDeptID
                    Session["OldDeptID"] = CtrDeptTree1.CurrDeptID;
				}
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
