using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Epower.ITSM.SqlDAL;
using EpowerCom;
using EpowerGlobal;

namespace Epower.ITSM.Web.Forms
{
    public partial class frmMessageCC : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strRecs = "";
            long lngMessageID = 0;
            long lngUserID = (long)Session["UserID"];
            string sReturnValue = "0";

            lngMessageID = long.Parse(Request.QueryString["MessageID"]);
            strRecs = Request.QueryString["Receivers"];
            string sType = "0";
            if (Request.QueryString["type"] != "")
            {
                sType = Request.QueryString["type"];
            }

            Message msg = new Message();
            try
            {
                if (sType == "0")
                {
                    msg.MessageCCByActor(lngUserID, lngMessageID, strRecs, e_ActorClass.fmReaderActor);
                }
                else
                {
                    msg.MessageCCByActor(lngUserID, lngMessageID, strRecs,e_ActorClass.fmAssistActor);
                }
            }
            catch
            {
                sReturnValue = "œ˚œ¢≥≠ÀÕ ß∞‹";
            }
            Response.Clear();
            Response.Write(sReturnValue);
            Response.Flush();
            Response.End();
        }
    }
}
