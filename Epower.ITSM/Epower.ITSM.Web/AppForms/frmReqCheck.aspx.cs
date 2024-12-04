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

namespace Epower.ITSM.Web.AppForms
{
    public partial class frmReqCheck : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sLastMessageID = "0";  //���µĴ�����ϢID�������յ���ϢID������ϢID
            if (Request.Params["LastID"] != null)
                sLastMessageID = Request.QueryString["LastID"];

            //��ȡ���µ���Ϣ���
            string str = cst_RequestDP.HasNewRequest(long.Parse(sLastMessageID));
            //string strXml = GetNoticeMessageXml(lngUserID, 0);
            Response.Clear();
            //�����Ƿ������� 0 ��, 1 ��
            Response.Write(str);
            Response.Flush();
            Response.End();
        }
    }
}
