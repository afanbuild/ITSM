/*******************************************************************
 * 版权所有：
 * Description：工作流一些特殊控制,如交接，处理时限等
 * 
 * 
 * Create By  ：
 * Create Date：
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
	///		CtrFlowSpecRight 的摘要说明。
	/// </summary>
	public partial class CtrFlowSpecRight : System.Web.UI.UserControl
	{

		private long lngMessageID = 0;
        long lngFlowID = 0;
		private long lngFlowModelID = 0;
        private long lngNodeModelID = 0;
		private bool blnReadOnly = false;
        bool blnIsReceiving = false;   //是否处于接收状态,接收状态有些功能不能操作,如转发.

		#region 属性
		/// <summary>
		/// 消息编号
		/// </summary>
		public long MessageID
		{
			set{lngMessageID = value;}
		}

        /// <summary>
        /// 流程编号
        /// </summary>
        public long FlowID
        {
            set { lngFlowID = value; }
        }

		/// <summary>
		/// 流程模型编号
		/// </summary>
		public long FlowModelID
		{
			set{lngFlowModelID = value;}
		}

        /// <summary>
        /// 当前模型编号
        /// </summary>
        public long NodeModelID
        {
            set { lngNodeModelID = value; }
        }

	

		/// <summary>
		/// 是否只读(只读不显示任何动作)
		/// </summary>
		public bool ReadOnly
		{
			set{blnReadOnly = value;}
		}

        /// <summary>
        /// 是否处于接收状态
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
						//只有主办 和 非仅重审的驳回才可以有这些特殊权限
						LoadActionsHtml();
					}
				}
			}
		}


		private void LoadActionsHtml()
		{
            //string strSpecRight = FlowModel.GetNodeSpecRights(lngMessageID);
            //e_IsTrue blnCustLimit = FlowModel.GetCanCustLimitRight(lngMessageID,lngFlowModelID);

            //2009-06-10修改为特殊权限为统一的方式，优化的方式
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

				//开头标记
				sb.Append("<TABLE id=\"ctrtabactions\" cellSpacing=\"1\" cellPadding=\"1\" width=\"100%\" border=\"0\">");
                if (ht["CanCustLimit"] != null && ht["CanCustLimit"].ToString() == "1")
                {
                    sb.Append("<TR><TD align=left nowrap colspan='" + iCount2.ToString() + "'>处理时限:<input name=\"ctrtxtExpected\" type=\"text\" maxlength=\"4\" " +
                        " id=\"ctrtxtExpected\" style='ime-mode:Disabled' onkeydown=\"NumberInput('');\" onchange=\"DoTransfLimitValue(this,1);\" />工时&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; " +
                        "预警时限:<input name=\"ctrtxtWarning\" width=\"10px\" type=\"text\" maxlength=\"4\" " +
                        " id=\"ctrtxtWarning\"  width=\"10px\" style='ime-mode:Disabled' onkeydown=\"NumberInput('');\" onchange=\"DoTransfLimitValue(this,2);\" />工时</TD></TR>");
                }

                if (iCount2 > 0)
                {
                    sb.Append("<TR>");
                    if (ht["TakeOver"] != null && ht["TakeOver"].ToString() == "1")
                    {
                        sb.Append("<TD align=left ><INPUT id=\"ctrcmdTakeOver" +
                            "\"  onclick=\"DoFreeTakeOverCtr();" +
                            "\" type=\"button\" class='btnClass' value=\" 交接 " +
                            "\" name=\"ctrcmdTakeOver" + "\" Height=\"24\"></TD>"
                            );
                    }                    

                    if (lngMessageID != 0 && blnIsReceiving == false)
                    {
                        if (ht["CanJump"] != null && ht["CanJump"].ToString() == "1")
                        {
                            //cboSpecRight.Items.Add(new ListItem("跳转",((int)e_SpecRightType.esrtCanJump).ToString()));
                            sb.Append("<TD align=left ><INPUT id=\"ctrcmdJump" +
                                "\" onclick=\"DoJumpCtr();" +
                                "\" type=\"button\" class='btnClass' value=\" 跳转 " +
                                "\" name=\"ctrcmdJump" + "\" Height=\"24\"></TD>"
                                );
                        }

                        if (ht["CanTransmit"] != null && ht["CanTransmit"].ToString() == "1")
                        {
                            sb.Append("<TD align=left ><INPUT class=\"btnClass\" id=\"ctrTransmit" +
                                "\" onclick=\"DoFreeTransmitCtr();\" type=\"button\" value=\"传阅" +
                                "\" name=\"ctrTransmit\" Height=\"24\"></TD>");
                        }

                        if (ht["CanAssist"] != null && ht["CanAssist"].ToString() == "1")
                        {
                            sb.Append("<TD align=left ><INPUT class=\"btnClass\" id=\"ctrTransmit" +
                                "\" onclick=\"DoFreeAssistCtr();\" type=\"button\" value=\"协作" +
                                "\" name=\"ctrAssist\" Height=\"24\"></TD>");
                        }
                        if (ht["CanCommunic"] != null && ht["CanCommunic"].ToString() == "1")
                        {
                            sb.Append("<TD align=left ><INPUT class=\"btnClass\" id=\"ctrTransmit" +
                                "\" onclick=\"DoFreeCommunicCtr();\" type=\"button\" value=\"沟通" +
                                "\" name=\"ctrCommunic\" Height=\"24\"></TD>");
                        }


                        if (ht["CanBackHasDone"] != null && ht["CanBackHasDone"].ToString() == "1")
                        {
                            sb.Append("<TD align=left ><INPUT class=\"btnClass\" id=\"ctrTransmit" +
                                "\" onclick=\"DoTakeBackHasDoneCtr();\" type=\"button\" value=\"驳回" +
                                "\" name=\"ctrBackHasDone\" Height=\"24\"></TD>");
                        }

                        sb.Append("<TD align=left width='100%'>&nbsp;</TD>");
                    }

                    sb.Append("</TR>");
                    
                }

				
				//结束标记
				sb.Append("</TABLE>");

				ltlFlowSpecRight.Text = sb.ToString();
			}
		}

		




		#region Web 窗体设计器生成的代码
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		设计器支持所需的方法 - 不要使用代码编辑器
		///		修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion
	}
}
