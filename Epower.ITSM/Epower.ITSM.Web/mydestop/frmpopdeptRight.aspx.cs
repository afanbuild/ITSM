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
    public partial class frmpopdeptRight : BasePage
	{
		protected long lngCurrDeptID = 1;
        public long Right = 0;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(Request.QueryString["CurrDeptID"] != null)
			{
				if (Request.QueryString["CurrDeptID"].Length >0)
					lngCurrDeptID = long.Parse(Request.QueryString["CurrDeptID"]);
				else
					lngCurrDeptID=1;
			}

            if (Request.QueryString["Right"] != null)
            {
                Right = long.Parse(Request.QueryString["Right"].ToString());
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
