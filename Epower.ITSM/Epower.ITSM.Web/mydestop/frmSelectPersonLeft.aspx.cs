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

namespace Epower.ITSM.Web.mydestop
{
	/// <summary>
	/// frmSelectPersonLeft ��ժҪ˵����
	/// </summary>
	public partial class frmSelectPersonLeft : BasePage
	{
        /// <summary>
        /// ��Ա���ͣ�ѡ����Ա���߷�����Ա
        /// </summary>
        protected string Type
        {
            get
            {
                if (Request["Type"] != null)
                    return Request["Type"].ToString();
                else
                    return "-1";
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
