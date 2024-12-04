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

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// frmSelectCatalog ��ժҪ˵����
	/// </summary>
	public partial class frmSelectCatalog : BasePage
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			if(Request.QueryString["LimitCurr"] != null)
			{
				CtrCatalogTree1.LimitCurr = bool.Parse(Request.QueryString["LimitCurr"]);
			}
			if(Request.QueryString["CurrCatalogID"] != null)
			{
				CtrCatalogTree1.CurrCatalogID = long.Parse(Request.QueryString["CurrCatalogID"]);
				// ��¼��ǰ����
				Session["OldCatalogID"] = CatalogDP.GetCatalogParentID(CtrCatalogTree1.CurrCatalogID);
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
