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
    public partial class frmpopdeptBranch : BasePage
	{
		protected long lngCurrDeptID = 0;
		protected void Page_Load(object sender, System.EventArgs e)
		{

            try
            {
                if (Request.QueryString["SubBankID"] != null)
                {
                    if (Request.QueryString["SubBankID"].Length > 0)
                    {
                        lngCurrDeptID = long.Parse(Request.QueryString["SubBankID"]);
                        if (lngCurrDeptID == 0)
                        {
                            lngCurrDeptID = 1;
                        }
                    }
                    else
                    {
                        lngCurrDeptID = 0;
                    }
                }
            }
            catch
            { }

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
