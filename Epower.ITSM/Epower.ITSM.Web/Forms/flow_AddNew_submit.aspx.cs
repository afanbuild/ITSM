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
using System.Xml;
using Epower.DevBase.BaseTools;
using EpowerGlobal;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// flow_AddNew_submit 的摘要说明。
	/// </summary>
	public partial class flow_AddNew_submit : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
            try
            {
                // 在此处放置用户代码以初始化页面

                //提交流程的代码。。。。

                long lngReturn = 0;
                long lngUserID;

                string strSubject = "";
                long lngFlowModelID = 0;
                long lngMessageID = 0;
                long lngActionID = 0;
                long lngImportance = 1;
                string strFormXMLValue = "";
                string strOpinionValue = "";
                string strReceivers = "";
                string strAttachment = "";
                string strMessage = "";
                string strSubmitType = "";

                long lngPreMessageID = 0;    //前置消息ＩＤ
                int iFlowJoinType = 10;

                string strHasSaved = "";

                bool blnSuccess = true;

                long lngLinkNodeID = 0;    //下一环节模型ID
                long lngLinkNodeType = 0;  //下一环节模型类型

                int lngExpectedLimit = 0;
                int lngWarningLimit = 0;

                lngUserID = (long)Session["UserID"];

                //防止用户通过IE后退按纽重复提交
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                //去除 HTML的一些特殊编码
                strSubject = Request.Form["Subject"].Trim().Replace(System.Web.HttpUtility.HtmlDecode("&#160;"), "").Replace(System.Web.HttpUtility.HtmlDecode("&nbsp;"), "");
                lngFlowModelID = long.Parse(Request.Form["FlowModelID"]);
                lngActionID = long.Parse(Request.Form["ActionID"]);
                lngImportance = long.Parse(Request.Form["Importance"]);
                strFormXMLValue = Request.Form["FormXMLValue"];
                strOpinionValue = Request.Form["OpinionValue"];
                strReceivers = Request.Form["Receivers"];
                strAttachment = HttpUtility.UrlDecode(Request.Form["Attachment"]);
                strMessage = Request.Form["strMessage"];
                strSubmitType = Request.Form["SubmitType"];
                strHasSaved = Request.Form["HasSaved"];

                lngPreMessageID = long.Parse(Request.Form["PreMessageID"]);
                iFlowJoinType = int.Parse(Request.Form["FlowJoinType"]);

                decimal dTemp = decimal.Parse(Request.Form["ExpectedLimit"]) * 60;
                lngExpectedLimit = (int)Math.Round(dTemp, 0);
                dTemp = decimal.Parse(Request.Form["WarningLimit"]) * 60;
                lngWarningLimit = (int)Math.Round(dTemp, 0);

                string strNextMsgReceivers = "";   //保存下面产生的消息列表,以便于判断继续处理

                long lngNextMessageID = 0;  //处理完成后页面继续处理的消息ＩＤ
                bool blnProcessCont = false; //是否继续处理：如果没有要自己处理的消息则不继续处理，以完成的界面去查看

                if (strHasSaved == "true")
                    lngMessageID = long.Parse(Request.Form["MessageID"]);




                try
                {
                    Message objMsg = new Message();
                    if (strSubmitType == "Send")
                    {
                        lngLinkNodeID = long.Parse(EnCodeTool.DeCode(Request.Form["LinkNodeID"]));
                        lngLinkNodeType = long.Parse(EnCodeTool.DeCode(Request.Form["LinkNodeType"]));
                        //如果没有暂存过要保证lngMessageID == 0

                        //objMsg.AddFlow(lngUserID,lngFlowModelID,lngMessageID,strSubject,lngActionID,lngLinkNodeID,lngLinkNodeType,lngImportance,strOpinionValue,strFormXMLValue,strReceivers,strAttachment,ref lngMessageID);
                        objMsg.AddFlow(lngUserID, lngFlowModelID, lngMessageID, strSubject, lngActionID, lngLinkNodeID, lngLinkNodeType, lngImportance, strOpinionValue, strFormXMLValue, strReceivers, strAttachment, lngPreMessageID, (e_FlowJoinType)iFlowJoinType, lngExpectedLimit, lngWarningLimit, ref lngMessageID);

                        strNextMsgReceivers = MessageDep.GetFollowReceiverIDs(lngMessageID);
                    }
                    else
                    {

                        if (strHasSaved == "true")
                        {
                            lngReturn = objMsg.SaveFlow(lngUserID, lngMessageID, lngFlowModelID, strSubject, lngActionID, lngImportance, strOpinionValue, strFormXMLValue, strAttachment, true, lngPreMessageID, (e_FlowJoinType)iFlowJoinType);
                        }
                        else
                        {
                            lngReturn = objMsg.SaveFlow(lngUserID, lngMessageID, lngFlowModelID, strSubject, lngActionID, lngImportance, strOpinionValue, strFormXMLValue, strAttachment, false, lngPreMessageID, (e_FlowJoinType)iFlowJoinType);

                        }

                    }
                }
                catch (Exception eSend)
                {
                    blnSuccess = false;
                    strMessage = eSend.Message;
                    Epower.DevBase.BaseTools.E8Logger.Error(eSend);
                }


                if ((strNextMsgReceivers.Trim() + ",").IndexOf(lngUserID.ToString().Trim()) < 0)
                {
                    //下一环节没有自己要处理的事情,停留在当前消息的下一个处理状态上
                    lngNextMessageID = lngMessageID;
                }
                else
                {
                    lngNextMessageID = MessageDep.GetSelfFollowMessageID(lngMessageID, lngUserID);
                    if (lngNextMessageID == 0)
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
                    if (blnSuccess == true)
                    {


                        if (blnProcessCont == true)
                        {
                            //Response.Write("<script>window.opener.parent.location='flow_Normal.aspx?MessageID=" + lngNextMessageID.ToString() +"';</script>");

                            //2010-05-03 增加自动通过功能处理，只要下一环节是自己，设置了自动通过，则自动转到下一环节
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

                            //Response.Write("<script>window.opener.parent.location='flow_Finished.aspx?MessageID="  + lngNextMessageID.ToString() +"';</script>");
                            Response.Redirect("flow_Finished.aspx?MessageID=" + lngNextMessageID.ToString());
                        }

                        //Response.Write("<script>window.opener.parent.close();</script>");
                    }
                    else
                    {
                        E8Logger.Error(strMessage);
                        //出现返回异常的情况
                        PageTool.MsgBox(this, strMessage.Replace("\r\n", ","));
                        //Response.Write("<script>window.history.back();</script>");
                        //Response.Write("<script>window.history.go(-1);</script>");
                        return;
                    }



                }
                else
                {
                    //保存
                    if (blnSuccess == true)
                    {

                        //表单上存储新产生的消息ID,重新装载一次

                        Response.Redirect("flow_Normal.aspx?MessageID=" + lngReturn.ToString());

                    }
                    else
                    {
                        //Response.Write("<script>window.opener.parent.close();</script>");
                        if (strMessage != "")
                        {
                            //出现返回异常的情况
                            PageTool.MsgBox(this, strMessage.Replace("\r\n", ","));
                            Response.Write("<script>window.history.back();</script>");
                            return;
                        }
                    }
                }
            }
            catch (System.Threading.ThreadAbortException)
            { }
            catch (Exception ex)
            {
                E8Logger.Error(ex);
                throw;
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
