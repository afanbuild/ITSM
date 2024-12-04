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
using Epower.DevBase.BaseTools;
using System.Xml;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// flow_Normal_submit ��ժҪ˵����
	/// </summary>
	public partial class flow_Normal_submit : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��

			//�ύ���̵Ĵ��롣������

			long lngReturn =0;
			long lngUserID;

			long lngActionID=0;
			long lngImportance=1;
			long lngMessageID =0;
            long lngFlowID = 0;   //2010-01-12 ���ӣ����ڹ��������̲����ĺ�����Ϣ

			bool blnSuccess = true;



			string strFormXMLValue="";
			string strOpinionValue="";
			string strReceivers="";
			string strAttachment="";
			string strSubmitType="";

			string strMessage = "";

			long lngLinkNodeID=0;    //��һ����ģ��ID
			long lngLinkNodeType=0;  //��һ����ģ������

			int lngExpectedLimit = 0;
			int lngWarningLimit = 0;

			e_SpecRightType intSpecRightType = e_SpecRightType.esrtNormal;   //���⴦����𡡣����ӡ���ֹ����ת��

			lngUserID = (long)Session["UserID"];

			//��ֹ�û�ͨ��IE���˰�Ŧ�ظ��ύ
			Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);

			lngMessageID = long.Parse(Request.Form["MessageID"]);
            lngFlowID = long.Parse(Request.Form["FlowID"]);
			lngActionID = long.Parse(Request.Form["ActionID"]);
			lngImportance = long.Parse(Request.Form["Importance"]);
			strFormXMLValue = Request.Form["FormXMLValue"];
			strOpinionValue = Request.Form["OpinionValue"];
			strReceivers = Request.Form["Receivers"];
			strAttachment = HttpUtility.UrlDecode(Request.Form["Attachment"]);
			strMessage = Request.Form["strMessage"];
			strSubmitType = Request.Form["SubmitType"];
			intSpecRightType = (e_SpecRightType)int.Parse(Request.Form["SpecRightType"]);

            decimal dTemp = decimal.Parse(Request.Form["ExpectedLimit"]) * 60;
            lngExpectedLimit =(int) Math.Round(dTemp, 0);
            dTemp = decimal.Parse(Request.Form["WarningLimit"]) * 60;
            lngWarningLimit = (int) Math.Round(dTemp, 0);

			string strNextMsgReceivers = "";   //���������������Ϣ�б�,�Ա����жϼ�������

			long lngNextMessageID = 0;  //������ɺ�ҳ������������Ϣ�ɣ�
			bool blnProcessCont = false; //�Ƿ�����������û��Ҫ�Լ��������Ϣ�򲻼�����������ɵĽ���ȥ�鿴

	��������bool blnBackToUnDoList = (Session["BackToUndoList"] == null)?false:(bool)Session["BackToUndoList"];
			string strUrl = Session["BackToUrl"].ToString();
			


			try
			{
				
				Message objMsg= new Message();

				if (strSubmitType == "Send")
				{

					lngLinkNodeID = int.Parse(EnCodeTool.DeCode(Request.Form["LinkNodeID"]));
					lngLinkNodeType = int.Parse(EnCodeTool.DeCode(Request.Form["LinkNodeType"]));
			

					objMsg.SendFlow(lngUserID,lngMessageID,lngActionID,intSpecRightType,lngLinkNodeID,lngLinkNodeType,lngImportance,strOpinionValue,strFormXMLValue,strReceivers,strAttachment,lngExpectedLimit,lngWarningLimit);
					
					strNextMsgReceivers = MessageDep.GetFollowReceiverIDs(lngMessageID);
					
					
				}
				else if(strSubmitType == "Save")
				{
					lngReturn = objMsg.SaveFlow(lngUserID,lngMessageID,0,"",lngActionID,lngImportance,strOpinionValue,strFormXMLValue,strAttachment,false);


				}
				else if(strSubmitType == "SendBack")
				{
					objMsg.SendBackFlow(lngUserID,lngMessageID,lngImportance,strOpinionValue,strAttachment,lngExpectedLimit,lngWarningLimit);
					strNextMsgReceivers = MessageDep.GetFollowReceiverIDs(lngMessageID);
				}
			}
			catch(Exception eSend)
			{
				strMessage =eSend.Message;
				blnSuccess = false;
			}

			if((strNextMsgReceivers.Trim() + ",").IndexOf(lngUserID.ToString().Trim()) < 0)
			{
				//��һ����û���Լ�Ҫ���������,ͣ���ڵ�ǰ��Ϣ����һ������״̬��
				lngNextMessageID = lngMessageID;
			}
			else
			{
				lngNextMessageID = MessageDep.GetSelfFollowMessageID(lngMessageID,lngUserID,lngFlowID);
				if(lngNextMessageID == 0)
				{
					lngNextMessageID = lngMessageID;
				}
				else
				{
					blnProcessCont = true;
				}
			}


			if (strSubmitType == "Send")
			{
				if (strMessage == "")
				{
                    if (intSpecRightType == e_SpecRightType.esrtCommunic || intSpecRightType == e_SpecRightType.esrtTransmit || intSpecRightType == e_SpecRightType.esrtAssist)
                    {
                        //��ͨ Э�� ����  ���´򿪵�ǰ��Ϣ
                        Response.Redirect("flow_Normal.aspx?MessageID=" + lngMessageID.ToString());

                    }
                    else
                    {

                        if (blnProcessCont == true)
                        {
                            //2010-05-03 �����Զ�ͨ�����ܴ���ֻҪ��һ�������Լ����������Զ�ͨ�������Զ�ת����һ���ڡ��˻ص�����²���Ҫ�Զ�����
                            Hashtable ht = FlowModel.GetNodeSpecRights20090610(lngNextMessageID);
                            if (ht["canAutoPass"] != null && ht["canAutoPass"].ToString() == "1")
                            {
                                //����������Զ�ͨ��Ȩ�� ������һ���������Զ�����
                                Response.Redirect("flow_Normal.aspx?MessageID=" + lngNextMessageID.ToString() + "&autopass=true");
                            }
                            else
                            {
                                Response.Redirect("flow_Normal.aspx?MessageID=" + lngNextMessageID.ToString());
                            }


                        }
                        else
                        {

                            if (blnBackToUnDoList == false)
                            {
                                Response.Redirect("flow_Finished.aspx?MessageID=" + lngNextMessageID.ToString());
                            }
                            else
                            {
                                Response.Redirect(strUrl.Trim());
                                return;
                            }
                        }

                    }
					

					
				}
				else
				{
					
					
					if(blnSuccess == true)
					{
						if(blnProcessCont == true)
						{
                            //2010-05-03 �����Զ�ͨ�����ܴ���ֻҪ��һ�������Լ����������Զ�ͨ�������Զ�ת����һ���ڡ��˻ص�����²���Ҫ�Զ�����
                            Hashtable ht = FlowModel.GetNodeSpecRights20090610(lngNextMessageID);
                            if (ht["canAutoPass"] != null && ht["canAutoPass"].ToString() == "1")
                            {
                                //����������Զ�ͨ��Ȩ�� ������һ���������Զ�����
                                Response.Redirect("flow_Normal.aspx?MessageID=" + lngNextMessageID.ToString() + "&autopass=true");
                            }
                            else
                            {
                                Response.Redirect("flow_Normal.aspx?MessageID=" + lngNextMessageID.ToString());
                            }
						}
						else
						{
						
							if(blnBackToUnDoList == false)
							{
								Response.Redirect("flow_Finished.aspx?MessageID="  + lngNextMessageID.ToString());
							}
							else
							{
								Response.Redirect(strUrl.Trim());
							}
						}
						return;
						

					}
					else
					{
						//���ַ����쳣�����
                        strMessage = strMessage.Replace("\r\n", ",");
                        if (strMessage.EndsWith(Environment.NewLine))
                        {
                            strMessage = strMessage.Substring(0, strMessage.Length - 1);
                        }
                        PageTool.MsgBox(this, strMessage);
						//Response.Write("<script>window.history.back();</script>");
						return;
					}

					
					

				}



		
			
				
				
			}
			else if(strSubmitType == "Save")
			{
				if(blnSuccess == true)
				{
					Response.Redirect("flow_Normal.aspx?MessageID=" + lngMessageID.ToString());
				}
				else
				{
					if(strMessage != "")
					{
						//���ַ����쳣�����
						PageTool.MsgBox(this,strMessage.Replace("\r\n",","));
						Response.Write("<script>window.history.back();</script>");
						return;
					}
				}

			}
			else if(strSubmitType == "SendBack")
			{
				if(blnSuccess == true)
				{
					//�������п�����ʾ�����أ�,��ʱ����
					//PageTool.MsgBox(this,"�Ѿ��˻أ�");
					
				}
				else
				{
					if(strMessage != "")
					{
						//���ַ����쳣�����
						PageTool.MsgBox(this,strMessage.Replace("\r\n",","));
						Response.Write("<script>window.history.back();</script>");
						return;
					}
					
				}

				
				if(blnProcessCont == true)
				{
					Response.Redirect("flow_Normal.aspx?MessageID=" + lngNextMessageID.ToString());
				}
				else
				{
					if(blnBackToUnDoList == false)
					{
						Response.Redirect("flow_Finished.aspx?MessageID=" + lngNextMessageID.ToString());
					}
					else
					{
						Response.Redirect(strUrl.Trim());
					}
				}
				
				




			}
			

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
		}
		#endregion
	}
}
