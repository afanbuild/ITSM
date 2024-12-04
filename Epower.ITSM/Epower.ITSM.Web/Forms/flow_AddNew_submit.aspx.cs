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
	/// flow_AddNew_submit ��ժҪ˵����
	/// </summary>
	public partial class flow_AddNew_submit : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
            try
            {
                // �ڴ˴������û������Գ�ʼ��ҳ��

                //�ύ���̵Ĵ��롣������

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

                long lngPreMessageID = 0;    //ǰ����Ϣ�ɣ�
                int iFlowJoinType = 10;

                string strHasSaved = "";

                bool blnSuccess = true;

                long lngLinkNodeID = 0;    //��һ����ģ��ID
                long lngLinkNodeType = 0;  //��һ����ģ������

                int lngExpectedLimit = 0;
                int lngWarningLimit = 0;

                lngUserID = (long)Session["UserID"];

                //��ֹ�û�ͨ��IE���˰�Ŧ�ظ��ύ
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                //ȥ�� HTML��һЩ�������
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

                string strNextMsgReceivers = "";   //���������������Ϣ�б�,�Ա����жϼ�������

                long lngNextMessageID = 0;  //������ɺ�ҳ������������Ϣ�ɣ�
                bool blnProcessCont = false; //�Ƿ�����������û��Ҫ�Լ��������Ϣ�򲻼�����������ɵĽ���ȥ�鿴

                if (strHasSaved == "true")
                    lngMessageID = long.Parse(Request.Form["MessageID"]);




                try
                {
                    Message objMsg = new Message();
                    if (strSubmitType == "Send")
                    {
                        lngLinkNodeID = long.Parse(EnCodeTool.DeCode(Request.Form["LinkNodeID"]));
                        lngLinkNodeType = long.Parse(EnCodeTool.DeCode(Request.Form["LinkNodeType"]));
                        //���û���ݴ��Ҫ��֤lngMessageID == 0

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
                    //��һ����û���Լ�Ҫ���������,ͣ���ڵ�ǰ��Ϣ����һ������״̬��
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

                            //2010-05-03 �����Զ�ͨ�����ܴ���ֻҪ��һ�������Լ����������Զ�ͨ�������Զ�ת����һ����
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

                            //Response.Write("<script>window.opener.parent.location='flow_Finished.aspx?MessageID="  + lngNextMessageID.ToString() +"';</script>");
                            Response.Redirect("flow_Finished.aspx?MessageID=" + lngNextMessageID.ToString());
                        }

                        //Response.Write("<script>window.opener.parent.close();</script>");
                    }
                    else
                    {
                        E8Logger.Error(strMessage);
                        //���ַ����쳣�����
                        PageTool.MsgBox(this, strMessage.Replace("\r\n", ","));
                        //Response.Write("<script>window.history.back();</script>");
                        //Response.Write("<script>window.history.go(-1);</script>");
                        return;
                    }



                }
                else
                {
                    //����
                    if (blnSuccess == true)
                    {

                        //���ϴ洢�²�������ϢID,����װ��һ��

                        Response.Redirect("flow_Normal.aspx?MessageID=" + lngReturn.ToString());

                    }
                    else
                    {
                        //Response.Write("<script>window.opener.parent.close();</script>");
                        if (strMessage != "")
                        {
                            //���ַ����쳣�����
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
