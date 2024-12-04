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

namespace Epower.ITSM.Web.Ajax
{
	/// <summary>
	/// frmSelectDept ��ժҪ˵����
	/// </summary>
    public partial class frmSelectCalalog : BasePage
	{
        private long mlngIsPoint = 0;
        protected long IsPoint
        {
            get
            {
                if (Request.QueryString["IsPoint"] != null)
                {
                    mlngIsPoint = long.Parse(Request.QueryString["IsPoint"] == "" ? "0" : Request.QueryString["IsPoint"]);
                }
                return mlngIsPoint;
            }
        }

		protected void Page_Load(object sender, System.EventArgs e)
		{

            if (ctrcatalogtreeNew1 != null)
			{
                if (Request.QueryString["RootID"] != null)
				{
                    ctrcatalogtreeNew1.lngRootID = long.Parse(Request.QueryString["RootID"]);
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
