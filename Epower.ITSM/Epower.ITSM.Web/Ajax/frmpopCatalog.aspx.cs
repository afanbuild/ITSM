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

namespace Epower.ITSM.Web.Ajax
{
	/// <summary>
	/// frmPopDept ��ժҪ˵����
	/// </summary>
    public partial class frmpopCatalog : BasePage
	{

        private long mlngRootID = 0;
        protected long lngRootID 
        {
            get
            {
                if (Request.QueryString["RootID"] != null)
                {
                    mlngRootID = long.Parse(Request.QueryString["RootID"]);
                }
                else
                {
                    mlngRootID = 0;
                }
                return mlngRootID;
            }
        }

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
