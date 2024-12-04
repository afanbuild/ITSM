using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Epower.ITSM.SqlDAL;
using EpowerCom;
using System.IO;

namespace Epower.ITSM.Web.Forms
{
    public partial class flow_attachment_preview : System.Web.UI.Page
    {
        /// <summary>
        /// 
        /// </summary>
        protected string FlashName
        {
            get
            {
                if (ViewState["FlashName"] != null)
                    return ViewState["FlashName"].ToString();
                else
                    return "a.swf";
            }
            set
            {

                ViewState["FlashName"] = value;

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            long lngFileID = Convert.ToInt64(Request.QueryString["Flash"]);
            string strFile = Epower.ITSM.SqlDAL.Inf_InformationDP.GetSWFFile(lngFileID.ToString());
            if (strFile != "" )
            {
                FlashName = strFile;
            }
            if (!IsPostBack)
            {
                //swfFile.Value = FlashName;
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>ShowFlash();</script>");
            }
        }
    }
}
