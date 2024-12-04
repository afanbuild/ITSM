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

namespace Epower.ITSM.Web.InformationManager
{
	/// <summary>
	/// frmSelectCatalog ��ժҪ˵����
	/// </summary>
    public partial class frmselectSubject : BasePage
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
            // �ڴ˴������û������Գ�ʼ��ҳ��
            if (Request.QueryString["LimitCurr"] != null)
            {
                CtrSubjecttree1.LimitCurr = bool.Parse(Request.QueryString["LimitCurr"]);
            }
			// �ڴ˴������û������Գ�ʼ��ҳ��
            if (Request.QueryString["CurrSubjectID"] != null)
			{
                CtrSubjecttree1.CurrSubjectID = long.Parse(Request.QueryString["CurrSubjectID"]);
				// ��¼��ǰ����
                Session["OldSubjectID"] = Inf_SubjectDP.GetSubjectByID(CtrSubjecttree1.CurrSubjectID).Rows[0]["ParentID"].ToString();
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
