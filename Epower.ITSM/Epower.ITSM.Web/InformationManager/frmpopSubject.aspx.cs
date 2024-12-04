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

namespace Epower.ITSM.Web.InformationManager
{
	/// <summary>
	/// frmPopCatalog ��ժҪ˵����
	/// </summary>
    public partial class frmpopSubject : BasePage
	{
		protected long lngCurrSubjectID = 1;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
            if (Request.QueryString["CurrSubjectID"] != null)
			{
                if (Request.QueryString["CurrSubjectID"].Length > 0)
                    lngCurrSubjectID = long.Parse(Request.QueryString["CurrSubjectID"]);
				else
                    lngCurrSubjectID = 1;
			}
		}
        /// <summary>
        /// ��ȡ·����id
        /// </summary>
        public string Opener_ClientId
        {
            get
            {
                return (Request["Opener_ClientId"] == null) ? "" : Request["Opener_ClientId"].ToString().Replace("hidClientId_ForOpenerPage", "");
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
