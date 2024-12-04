/*******************************************************************
 * ��Ȩ���У�
 * Description����������ť
 * 
 * 
 * Create By  ��
 * Create Date��
 * *****************************************************************/

using System;
using System.Data;
using System.Text;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using EpowerGlobal;
using EpowerCom;

namespace Epower.ITSM.Web.Controls
{
	/// <summary>
	///		CtrActions ��ժҪ˵����
	/// </summary>
	public partial class CtrActions : System.Web.UI.UserControl
	{

		private long lngFlowModelID = 0;
		private long lngNodeModelID = 0;
		private bool blnReadOnly = false;
		private bool blnIsReader = false;

		#region ����
		/// <summary>
		/// ����ģ�ͱ��
		/// </summary>
		public long FlowModelID
		{
			set{lngFlowModelID = value;}
		}

		/// <summary>
		/// ����ģ�ͱ��
		/// </summary>
		public long NodeModelID
		{
			set{lngNodeModelID = value;}
		}

		/// <summary>
		/// �Ƿ�ֻ��(ֻ������ʾ�κζ���)
		/// </summary>
		public bool ReadOnly
		{
			set{blnReadOnly = value;}
		}

		/// <summary>
		/// �Ƿ�����֪��Э�� ��ͨ ��ǩ ״̬(ֻ��ʾȷ����ť)
		/// </summary>
		public bool IsReader
		{
			set{blnIsReader = value;}
		}

#endregion



		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				
				if(blnReadOnly == true )  // 
				{
					ltlActions.Text = "";
				}
				else 
				{
					//�Ķ���Э��ʱֻ��ʾȷ����ť
					LoadActionsHtml();
				}
			}
		}


		private void LoadActionsHtml()
		{
			//DataSet ds1 = FlowModel.GetNodeActions(lngFlowModelID,lngNodeModelID);
            DataRow[] drs = FlowModel.GetNodeActionNew(lngFlowModelID, lngNodeModelID);

			StringBuilder sb = new StringBuilder("");
			string strActionName = "";
			string strActionID = "0";
			int iPecent = 0;
			int iButtLen = 40;   // 8 + �ַ�������16

            //��ͷ���
            sb.Append("<TABLE id=\"ctrtabactions\" cellSpacing=\"1\" cellPadding=\"1\" width=\"100%\" border=\"0\"><TR>");

            if (drs == null || drs.Length == 0 || blnIsReader == true)
            {
                //Ĭ��һ���ɣ�Ϊ���Ķ���
                sb.Append("<TD align=center width=\"100%\"><INPUT class=\"btnClass3\" id=\"ctrcmdAction0" +
                    "\" onclick=\"DoActionsCtr(0,'ȷ��'" +
                    ");\" type=\"button\" value=\" ȷ�� " +
                    "\" name=\"ctrcmdAction0" + "\" Height=\"24\"></TD>"
                    );
            }
            else
            {
                iPecent = 100 / (drs.Length);
                foreach (DataRow r in drs)
                {

                    strActionName = r["ActionName"].ToString();
                    strActionID = r["ActionID"].ToString();

                    sb.Append("<TD align=center width=\"" + iPecent.ToString() + "%\"><INPUT class=\"btnClass3\" id=\"ctrcmdAction" + strActionID.Trim() +
                            "\"  onclick=\"DoActionsCtr(" + strActionID.Trim() + ",'" + strActionName + "'" +
                            ");\" type=\"button\" value=\"" + strActionName.Trim() +
                            "\" name=\"ctrcmdAction" + strActionID.Trim() + "\" ></TD>"
                            );
                    //������ȱʡ������Ӱ����
                }
            }

			//�������
			sb.Append("</TR></TABLE>");

			ltlActions.Text = sb.ToString();
		}

		private int GetButtonLength(string strName)
		{
			
			int len = strName.Length;

			return (len==0)?40:(8 + 16 * len);

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
