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




            //防止用户通过IE后退按纽重复提交
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
                
                //接收后进入处理页面　
                if ((strNextMsgReceivers.Trim() + ",").IndexOf(lngUserID.ToString().Trim()) < 0)
                {
                    //下一环节没有自己要处理的事情,回到原来的页面
                    Response.Redirect(Session["AttemperBackUrl"].ToString());
                }
                else
                {
                    long lngNextMessageID = MessageDep.GetSelfFollowMessageID(lngMessageID, lngUserID);
                    if (lngNextMessageID != 0)
                    {
                        //调度后为自己处理的情况,直接进入处理页面
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
                    PageTool.MsgBox(this, "调度失败");
                    Response.Write("<script>window.history.back();</script>");
                    //Response.Redirect("frmContent.aspx");
                    return;
                }
            }
			

        }
    }
}
