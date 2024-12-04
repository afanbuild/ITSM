using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.Forms
{
    public class Handler : IHttpHandler, System.Web.SessionState.IReadOnlySessionState 
    {

        public void ProcessRequest(HttpContext context)
        {
            CHandler.ChooseMethod(context);  // 选择Ajax方法
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
