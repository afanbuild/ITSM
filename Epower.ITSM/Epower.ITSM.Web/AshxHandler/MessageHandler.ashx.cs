using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using EpowerCom;
using Epower.DevBase.BaseTools;


namespace Epower.ITSM.Web.Forms
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明


    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class MessageHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string userId = context.Request["userId"] != null ? context.Request["userId"] : "0";
            context.Response.Write(GetCount(userId));
        }


        private static string GetCount(string userId)
        {
            try
            {
                //return System.DateTime.Now.Millisecond.ToString();

                //int count = 0;
                DataTable dtUndoMessage = MessageCollectionDP.GetUndoMessage(long.Parse(userId), int.MaxValue);//待办事项
                //DataTable dtReceiveMessageList = ReceiveList.GetReceiveMessageList(long.Parse(userId)).Tables[0];//待接收事项

                return (dtUndoMessage == null) ? "0" : dtUndoMessage.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                E8Logger.Error(ex);
                return "0";
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
