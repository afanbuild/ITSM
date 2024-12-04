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
	/// flow_Normal_submit 的摘要说明。
	/// </summary>
	public partial class flow_Normal_submit : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面

			//提交流程的代码。。。。

			long lngReturn =0;
			long lngUserID;

			long lngActionID=0;
			long lngImportance=1;
			long lngMessageID =0;
            long lngFlowID = 0;   //2010-01-12 增加，用于过滤子流程产生的后续消息

			bool blnSuccess = true;



			string strFormXMLValue="";
			string strOpinionValue="";
			string strReceivers="";
			string strAttachment="";
			string strSubmitType="";

			string strMessage = "";

			long lngLinkNodeID=0;    //下一环节模型ID
			long lngLinkNodeType=0;  //下一环节模型类型

			int lngExpectedLimit = 0;
			int lngWarningLimit = 0;

			e_SpecRightType intSpecRightType = e_SpecRightType.esrtNormal;   //特殊处理类别　：交接　终止　跳转等

			lngUserID = (long)Session["UserID"];

			//防止用户通过IE后退按纽重复提交
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

			string strNextMsgReceivers = "";   //保存下面产生的消息列表,以便于判断继续处理

			long lngNextMessageID = 0;  //处理完成后页面继续处理的消息ＩＤ
			bool blnProcessCont = false; //是否继续处理：如果没有要自己处理的消息则不继续处理，以完成的界面去查看

	　　　　bool blnBackToUnDoList = (Session["BackToUndoList"] == null)?false:(bool)Session["BackToUndoList"];
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
				//下一环节没有自己要处理的事情,停留在当前消息的下一个处理状态上
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
                        //沟通 协作 传阅  重新打开当前消息
                        Response.Redirect("flow_Normal.aspx?MessageID=" + lngMessageID.ToString());

                    }
                    else
                    {

                        if (blnProcessCont == true)
                        {
                            //2010-05-03 增加自动通过功能处理，只要下一环节是自己，设置了自动通过，则自动转到下一环节【退回等情况下不需要自动处理】
                            Hashtable ht = FlowModel.GetNodeSpecRights20090610(lngNextMessageID);
                            if (ht["canAutoPass"] != null && ht["canAutoPass"].ToString() == "1")
                            {
                                //如果设置了自动通过权限 则增加一个参数，自动处理
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
                            //2010-05-03 增加自动通过功能处理，只要下一环节是自己，设置了自动通过，则自动转到下一环节【退回等情况下不需要自动处理】
                            Hashtable ht = FlowModel.GetNodeSpecRights20090610(lngNextMessageID);
                            if (ht["canAutoPass"] != null && ht["canAutoPass"].ToString() == "1")
                            {
                                //如果设置了自动通过权限 则增加一个参数，自动处理
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
						//出现返回异常的情况
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
						//出现返回异常的情况
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
					//界面上有可能显示＂返回＂,暂时屏蔽
					//PageTool.MsgBox(this,"已经退回！");
					
				}
				else
				{
					if(strMessage != "")
					{
						//出现返回异常的情况
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
			// CODEGEN：该调用是 ASP.NET Web 窗体设计器所必需的。
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion
	}
}
