/*******************************************************************
 * ��Ȩ���У�
 * Description�����������ؼ�
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
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		flowReader ��ժҪ˵����
	/// </summary>
	public partial  class flowReader : System.Web.UI.UserControl
	{

		protected string _SendType = "Handle";
		public string SendType
		{
			get{return _SendType;}
			set{_SendType = value;}
		}

		protected bool GetVisible()
		{
			return  (_SendType == "Handle")?true:false;
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��

		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN���õ����� ASP.NET Web ���������������ġ�
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		///		�����֧������ķ��� - ��Ҫʹ��
		///		����༭���޸Ĵ˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion
	}
}
