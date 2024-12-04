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
using Epower.ITSM.SqlDAL.SystemFileList;
using Epower.ITSM.SqlDAL.Common;
using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;
using System.Drawing;
using Epower.ITSM.SqlDAL.Flash;

namespace Epower.ITSM.Web.Common
{
    public partial class FormRefreshPageMain : BasePage
    {

        #region FromBackUrl
        public DataTable dt;
        private DataTable dt1;
        protected string sCheckEnable = "true";  //是否自动检测

        protected long lngCheckTime = 20000;// 300000;    //检测时间间隔

        protected int num = 0;
        public string FromBackUrl
        {
            get
            {
                if (ViewState["FromBackUrl"] != null)
                    return ViewState["FromBackUrl"].ToString();
                else
                    return "";
            }
            set
            {
                ViewState["FromBackUrl"] = value;
            }
        }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["num"]))
            {
                num = int.Parse(Request.QueryString["num"]);
            }
            SetParentButtonEvent();
            sCheckEnable = CommonDP.GetConfigValue("AutoCheckNewItems", "AutoCheckNewItemsEnable");
            lngCheckTime = long.Parse(CommonDP.GetConfigValue("AutoRefreshTime", "AutoRefreshTime"));
            if (lngCheckTime < 20000)//60000
            {
                //最小间隔0.5分钟
                lngCheckTime = 20000;
            }

            Session["FromUrl"] = "../Common/FormRefreshPageMain.aspx";
            FromBackUrl = Session["FromUrl"].ToString();


            Display(num);

        }

        void Display(int num)
        {
            if (num == 0)
            {
                tr1.Height = "450px";
                iframe1.Attributes["src"] = "Report.aspx";
                //Tr4.Height = "720px";
                //iframe4.Attributes["src"] = "ReportImage.aspx";
                //Tr3.Height = "600px";
                //iframe3.Attributes["src"] = "ReportDe.aspx";

            }
            else if (num == 1)
            {
                tr1.Height = "800px";
                iframe1.Attributes["src"] = "ReportImage.aspx";
                //Tr4.Height = "600px";
                //iframe4.Attributes["src"] = "ReportDe.aspx";
                //Tr3.Height = "700px";
                //iframe3.Attributes["src"] = "Report.aspx";

            }
            else if (num == 2)
            {
                tr1.Height = "600px";
                iframe1.Attributes["src"] = "ReportDe.aspx";
                //Tr4.Height = "700px";
                //iframe4.Attributes["src"] = "Report.aspx";
                //Tr3.Height = "720px";
                //iframe3.Attributes["src"] = "ReportImage.aspx";
                

            }
        }


        #region SetParentButtonEvent
        public void SetParentButtonEvent()
        {

            this.Master.MainID = "1";
        }
        #endregion



    }
}
