/*******************************************************************
 * ��Ȩ���У�
 * Description��������������ƣ��������
 * 
 * 
 * Create By  ��
 * Create Date��
 * *****************************************************************/
namespace Epower.ITSM.Web.Controls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		CtrImportance ��ժҪ˵����
	/// </summary>
	public partial class CtrImportance : System.Web.UI.UserControl
	{


       #region ����
		public int Importance
		{
			set{if(value <=2 && value >=0) rdoImportance.SelectedIndex = (2- value);}
			//get{return int.Parse(rdoImportance.Items[rdoImportance.SelectedIndex].Value);}
			get{return int.Parse(rdoImportance.SelectedValue);}
		}
		#endregion


		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
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
		///		�����֧������ķ��� - ��Ҫʹ�ô���༭��
		///		�޸Ĵ˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion
	}
}
