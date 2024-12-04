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
	/// frmCatalogContent ��ժҪ˵����
	/// </summary>
	public partial class frmCatalogContent : BasePage
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
            if (!this.IsPostBack)
            {
                string SubjectID = "1";
                if (Request.QueryString["CurrCatalogID"] != null)
                {
                    SubjectID = Request.QueryString["CurrCatalogID"].ToString();
                    Session["OldCatalogID"] = SubjectID;
                }
                if (Session["OldCatalogID"] != null)
                {
                    SubjectID = Session["OldCatalogID"].ToString();
                }
                Response.Write("<SCRIPT>window.parent.cataloginfo.location='frmcatalogedit.aspx?catalogid=" + SubjectID + "';</SCRIPT>");
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
