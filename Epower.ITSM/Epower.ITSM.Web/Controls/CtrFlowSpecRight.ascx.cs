/*******************************************************************
 * ��Ȩ���У�
 * Description��������һЩ�������,�罻�ӣ�����ʱ�޵�
 * 
 * 
 * Create By  ��
 * Create Date��
 * *****************************************************************/
namespace Epower.ITSM.Web.Controls
{
	using System;
	using System.Data;
	using System.Text;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using EpowerCom;
	using EpowerGlobal;
    using System.Collections;

	/// <summary>
	///		CtrFlowSpecRight ��ժҪ˵����
	/// </summary>
	public partial class CtrFlowSpecRight : System.Web.UI.UserControl
	{

		private long lngMessageID = 0;
        long lngFlowID = 0;
		private long lngFlowModelID = 0;
        private long lngNodeModelID = 0;
		private bool blnReadOnly = false;
        bool blnIsReceiving = false;   //�Ƿ��ڽ���״̬,����״̬��Щ���ܲ��ܲ���,��ת��.

		#region ����
		/// <summary>
		/// ��Ϣ���
		/// </summary>
		public long MessageID
		{
			set{lngMessageID = value;}
		}

        /// <summary>
        /// ���̱��
        /// </summary>
        public long FlowID
        {
            set { lngFlowID = value; }
        }

		/// <summary>
		/// ����ģ�ͱ��
		/// </summary>
		public long FlowModelID
		{
			set{lngFlowModelID = value;}
		}

        /// <summary>
        /// ��ǰģ�ͱ��
        /// </summary>
        public long NodeModelID
        {
            set { lngNodeModelID = value; }
        }

	

		/// <summary>
		/// �Ƿ�ֻ��(ֻ������ʾ�κζ���)
		/// </summary>
		public bool ReadOnly
		{
			set{blnReadOnly = value;}
		}

        /// <summary>
        /// �Ƿ��ڽ���״̬
        /// </summary>
        public bool IsReceiving
        {
            set { blnIsReceiving = value; }
        }


		
#endregion

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				
				if(blnReadOnly == true)  // 
				{
					ltlFlowSpecRight.Text = "";
				}
				else 
				{
					MessageEntity msgObject = new MessageEntity(lngMessageID);
					if(msgObject.ActorType == e_ActorClass.fmMasterActor && msgObject.ReceiveType != e_MsgReceiveType.emrtBackHasDoneRedo)
					{
						//ֻ������ �� �ǽ�����Ĳ��زſ�������Щ����Ȩ��
						LoadActionsHtml();
					}
				}
			}
		}


		private void LoadActionsHtml()
		{
            //string strSpecRight = FlowModel.GetNodeSpecRights(lngMessageID);
            //e_IsTrue blnCustLimit = FlowModel.GetCanCustLimitRight(lngMessageID,lngFlowModelID);

            //2009-06-10�޸�Ϊ����Ȩ��Ϊͳһ�ķ�ʽ���Ż��ķ�ʽ
            Hashtable ht = FlowModel.GetNodeSpecRights20090610(lngFlowModelID, lngNodeModelID);
            
            int iCount = 0;
            int iCount2 = 0;

            if (ht["CanCustLimit"] != null && ht["CanCustLimit"].ToString() == "1")
                iCount++;
            if (ht["TakeOver"] != null && ht["TakeOver"].ToString() == "1")
            {
                iCount++;
                iCount2++;
            }
            if (ht["CanJump"] != null && ht["CanJump"].ToString() == "1")
            {
                iCount++;
                iCount2++;
            }

            if (lngMessageID != 0 && blnIsReceiving == false)
            {

                if (ht["CanTransmit"] != null && ht["CanTransmit"].ToString() == "1")
                {
                    iCount++;
                    iCount2++;
                }

                if (ht["CanAssist"] != null && ht["CanAssist"].ToString() == "1")
                {
                    iCount++;
                    iCount2++;
                }

                if (ht["CanCommunic"] != null && ht["CanCommunic"].ToString() == "1")
                {
                    iCount++;
                    iCount2++;
                }

                if (ht["CanBackHasDone"] != null && ht["CanBackHasDone"].ToString() == "1")
                {
                    iCount++;
                    iCount2++;
                }
            }
			if(iCount == 0)
			{
				ltlFlowSpecRight.Text = "";
			}
			else
			{

				StringBuilder sb = new StringBuilder("");

				//��ͷ���
				sb.Append("<TABLE id=\"ctrtabactions\" cellSpacing=\"1\" cellPadding=\"1\" width=\"100%\" border=\"0\">");
                if (ht["CanCustLimit"] != null && ht["CanCustLimit"].ToString() == "1")
                {
                    sb.Append("<TR><TD align=left nowrap colspan='" + iCount2.ToString() + "'>����ʱ��:<input name=\"ctrtxtExpected\" type=\"text\" maxlength=\"4\" " +
                        " id=\"ctrtxtExpected\" style='ime-mode:Disabled' onkeydown=\"NumberInput('');\" onchange=\"DoTransfLimitValue(this,1);\" />��ʱ&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; " +
                        "Ԥ��ʱ��:<input name=\"ctrtxtWarning\" width=\"10px\" type=\"text\" maxlength=\"4\" " +
                        " id=\"ctrtxtWarning\"  width=\"10px\" style='ime-mode:Disabled' onkeydown=\"NumberInput('');\" onchange=\"DoTransfLimitValue(this,2);\" />��ʱ</TD></TR>");
                }

                if (iCount2 > 0)
                {
                    sb.Append("<TR>");
                    if (ht["TakeOver"] != null && ht["TakeOver"].ToString() == "1")
                    {
                        sb.Append("<TD align=left ><INPUT id=\"ctrcmdTakeOver" +
                            "\"  onclick=\"DoFreeTakeOverCtr();" +
                            "\" type=\"button\" class='btnClass' value=\" ���� " +
                            "\" name=\"ctrcmdTakeOver" + "\" Height=\"24\"></TD>"
                            );
                    }                    

                    if (lngMessageID != 0 && blnIsReceiving == false)
                    {
                        if (ht["CanJump"] != null && ht["CanJump"].ToString() == "1")
                        {
                            //cboSpecRight.Items.Add(new ListItem("��ת",((int)e_SpecRightType.esrtCanJump).ToString()));
                            sb.Append("<TD align=left ><INPUT id=\"ctrcmdJump" +
                                "\" onclick=\"DoJumpCtr();" +
                                "\" type=\"button\" class='btnClass' value=\" ��ת " +
                                "\" name=\"ctrcmdJump" + "\" Height=\"24\"></TD>"
                                );
                        }

                        if (ht["CanTransmit"] != null && ht["CanTransmit"].ToString() == "1")
                        {
                            sb.Append("<TD align=left ><INPUT class=\"btnClass\" id=\"ctrTransmit" +
                                "\" onclick=\"DoFreeTransmitCtr();\" type=\"button\" value=\"����" +
                                "\" name=\"ctrTransmit\" Height=\"24\"></TD>");
                        }

                        if (ht["CanAssist"] != null && ht["CanAssist"].ToString() == "1")
                        {
                            sb.Append("<TD align=left ><INPUT class=\"btnClass\" id=\"ctrTransmit" +
                                "\" onclick=\"DoFreeAssistCtr();\" type=\"button\" value=\"Э��" +
                                "\" name=\"ctrAssist\" Height=\"24\"></TD>");
                        }
                        if (ht["CanCommunic"] != null && ht["CanCommunic"].ToString() == "1")
                        {
                            sb.Append("<TD align=left ><INPUT class=\"btnClass\" id=\"ctrTransmit" +
                                "\" onclick=\"DoFreeCommunicCtr();\" type=\"button\" value=\"��ͨ" +
                                "\" name=\"ctrCommunic\" Height=\"24\"></TD>");
                        }


                        if (ht["CanBackHasDone"] != null && ht["CanBackHasDone"].ToString() == "1")
                        {
                            sb.Append("<TD align=left ><INPUT class=\"btnClass\" id=\"ctrTransmit" +
                                "\" onclick=\"DoTakeBackHasDoneCtr();\" type=\"button\" value=\"����" +
                                "\" name=\"ctrBackHasDone\" Height=\"24\"></TD>");
                        }

                        sb.Append("<TD align=left width='100%'>&nbsp;</TD>");
                    }

                    sb.Append("</TR>");
                    
                }

				
				//�������
				sb.Append("</TABLE>");

				ltlFlowSpecRight.Text = sb.ToString();
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
		///		�����֧������ķ��� - ��Ҫʹ�ô���༭��
		///		�޸Ĵ˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion
	}
}
