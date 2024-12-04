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
using EpowerCom;
using EpowerGlobal;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// flow_Read_menu_Finished ��ժҪ˵����
	/// </summary>
	public partial class OA_Read_menu_Finished : System.Web.UI.Page
	{
        public string sApplicationUrl = Epower.ITSM.Base.Constant.ApplicationPath;     //����·��

		public long lngMessageID = 0;

		protected void Page_Load(object sender, System.EventArgs e)
		{
            // //�ڴ˴������û������Գ�ʼ��ҳ��
            MessageEntity msgObject = (MessageEntity)Session["MessageObject"];
            lngMessageID = msgObject.MessageID;		
			
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
		
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{    
			this.ID = "OA_Read_menu_Finished";

		}
		#endregion
	}
}
