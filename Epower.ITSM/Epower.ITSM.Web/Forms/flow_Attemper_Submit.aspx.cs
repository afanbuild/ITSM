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

namespace Epower.ITSM.Web.Forms
{
    public partial class flow_Attemper_Submit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            long lngUserID;

            long lngMessageID = 0;


            bool blnSuccess = true;

            int lngLinkNodeID = 0;
            int lngLinkNodeType = 0;

            string strReceivers ="";

            string strMessage = "";
            string strNextMsgReceivers = "";

            lngUserID = (long)Session["UserID"];

            lngMessageID = long.Parse(Request.QueryString["MessageID"]);
            

            lngLinkNodeID = int.Parse(EnCodeTool.DeCode(Request.Form["LinkNodeID"]));
            lngLinkNodeType = int.Parse(EnCodeTool.DeCode(Request.Form["LinkNodeType"]));

            strReceivers = Request.Form["Receivers"];




            //��ֹ�û�ͨ��IE���˰�Ŧ�ظ��ύ
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);

            try
            {
                Message objMsg = new Message();
                objMsg.AttemperFlow(lngUserID, lngMessageID, lngLinkNodeID, lngLinkNodeType, strReceivers);
                strNextMsgReceivers = MessageDep.GetFollowReceiverIDs(lngMessageID);
            }
            catch (Exception eSend)
            {
                strMessage = eSend.Message;
                blnSuccess = false;
            }



            if (blnSuccess == true)
            {
                
                //���պ���봦��ҳ�桡
                if ((strNextMsgReceivers.Trim() + ",").IndexOf(lngUserID.ToString().Trim()) < 0)
                {
                    //��һ����û���Լ�Ҫ���������,�ص�ԭ����ҳ��
                    Response.Redirect(Session["AttemperBackUrl"].ToString());
                }
                else
                {
                    long lngNextMessageID = MessageDep.GetSelfFollowMessageID(lngMessageID, lngUserID);
                    if (lngNextMessageID != 0)
                    {
                        //���Ⱥ�Ϊ�Լ���������,ֱ�ӽ��봦��ҳ��
                        Response.Redirect("flow_Normal.aspx?MessageID=" + lngNextMessageID.ToString());
                    }
                    else
                    {
                        Response.Redirect(Session["AttemperBackUrl"].ToString());
                    }
                }

               

            }
            else
            {
                if (strMessage != "")
                {
                    PageTool.MsgBox(this, strMessage);
                }
                else
                {
                    PageTool.MsgBox(this, "����ʧ��");
                    Response.Write("<script>window.history.back();</script>");
                    //Response.Redirect("frmContent.aspx");
                    return;
                }
            }
			

        }
    }
}
