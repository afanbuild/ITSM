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
using Epower.ITSM.Log;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.Business;

namespace Epower.ITSM.Web.Forms
{
    /// <summary>
    /// flow_Reader_submit 的摘要说明。
    /// </summary>
    public partial class flow_Reader_submit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            // 在此处放置用户代码以初始化页面
            long lngUserID;

            long lngMessageID = 0;
            string strOpinion = "";
            string strXmlValues = "";
            string sActorType = "1";   //阅知
            string sAttXml = "";      //附件



            bool blnSuccess = true;



            string strMessage = "";

            lngUserID = (long)Session["UserID"];

            //防止用户通过IE后退按纽重复提交
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);

            lngMessageID = long.Parse(Request.Form["MessageID"]);
            strOpinion = Request.Form["OpinionValue"];
            strXmlValues = Request.Form["FormXMLValue"];
            sAttXml = HttpUtility.UrlDecode(Request.Form["Attachment"]);
            sActorType = Request.Form["ActorType"];

            string strIsSaved = Request.QueryString["IsSaved"];

            try
            {
                if (strIsSaved == "no")
                {
                    if (sActorType == "2")
                    {
                        //2010.1.4 协办可以处理附件
                        Message.SetReadOver(lngUserID, lngMessageID, strOpinion, strXmlValues, sAttXml);
                    }
                    else
                    {
                        Message.SetReadOver(lngUserID, lngMessageID, strOpinion, strXmlValues);
                    }
                }
                else
                {
                    Message.SetReadTempSave(lngUserID, lngMessageID, strOpinion);
                }


                InfluxBS.SendMailForLastInfluxActor((long)Session["UserID"], lngMessageID, strXmlValues);
            
            }
            catch (Exception eSend)
            {
                strMessage = eSend.Message;
                blnSuccess = false;
            }



            if (strIsSaved == "no")
            {
                if (blnSuccess == true)
                {
                    //阅读／协办完成停留在处理完成的界面

                    Response.Redirect("flow_Finished.aspx?MessageID=" + lngMessageID.ToString());

                }
                else
                {
                    if (strMessage != "")
                    {
                        //出现返回异常的情况
                        PageTool.MsgBox(this, strMessage.Replace("\r\n", ","));
                    }
                }


            }
            else
            {
                if (blnSuccess == true)
                {
                    PageTool.MsgBox(this, "已经保存意见信息！");
                    Response.Write("<script>window.history.back();</script>");

                }
                else
                {
                    if (strMessage != "")
                    {
                        //出现返回异常的情况
                        PageTool.MsgBox(this, strMessage.Replace("\r\n", ","));
                        //Response.Write("<script>window.history.back();</script>");
                        //return;
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
