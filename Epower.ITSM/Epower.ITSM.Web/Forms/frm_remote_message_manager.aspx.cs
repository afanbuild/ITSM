using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;

namespace Epower.ITSM.Web.Forms
{
    public partial class frm_remote_message_manager : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String strMsgFilePath = Server.MapPath("~/remote_content.txt");

            String strMessageContent = ReadContentFromFile(strMsgFilePath);

            //dgRemoteMessageList                        
            System.Collections.Generic.List<
                System.Collections.Generic.Dictionary<String, String>>
                dictMessages = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<
                System.Collections.Generic.Dictionary<String, String>>>(strMessageContent);

            dgRemoteMessageList.DataSource = dictMessages;
            dgRemoteMessageList.DataBind();
        }

        private String ReadContentFromFile(String strFilePath)
        {
            try
            {
                using (StreamReader sr = new StreamReader(strFilePath))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void dgRelKey_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            return;
        }
    }
}
