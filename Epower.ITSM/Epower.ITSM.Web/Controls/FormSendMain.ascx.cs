/*******************************************************************
 * ��Ȩ���У�
 * Description������������ؼ�
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
	using EpowerCom;
	using EpowerGlobal;

	/// <summary>
	///		FormSendMain ��ժҪ˵����
	/// </summary>
	public partial  class FormSendMain : System.Web.UI.UserControl
	{

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

		protected override void Render(HtmlTextWriter writer)
		{
			//��������ֶ���Ϣ
			//if (!Page.IsPostBack)
			UIGlobal.AddHiddenField(writer,"Subject");
			UIGlobal.AddHiddenField(writer,"FlowModelID","0");
			UIGlobal.AddHiddenField(writer,"MessageID","0");
            UIGlobal.AddHiddenField(writer, "FlowID", "0");
			UIGlobal.AddHiddenField(writer,"ActionID","-1");
			UIGlobal.AddHiddenField(writer,"FormXMLValue");
            UIGlobal.AddHiddenField(writer, "FormDefineValue");
			UIGlobal.AddHiddenField(writer,"OpinionValue");
			UIGlobal.AddHiddenField(writer,"Attachment");
			UIGlobal.AddHiddenField(writer,"Receivers");
			UIGlobal.AddHiddenField(writer,"Importance");
			UIGlobal.AddHiddenField(writer,"LinkNodeID");
			UIGlobal.AddHiddenField(writer,"LinkNodeType");
		    UIGlobal.AddHiddenField(writer,"strMessage");
			UIGlobal.AddHiddenField(writer,"SubmitType","Send");
			UIGlobal.AddHiddenField(writer,"HasSaved","false");
			UIGlobal.AddHiddenField(writer,"SpecRightType",((int)e_SpecRightType.esrtNormal).ToString());   //�������⴦��������ֶ�
			UIGlobal.AddHiddenField(writer,"JumpToNodeModel","0");   //������ת���Ļ���ģ��ID�������ֶ�

            UIGlobal.AddHiddenField(writer, "NodeID", "0");
		}
	}
}
