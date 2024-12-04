using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;

using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.Ashx
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ItEngineerHandler : IHttpHandler
    {

        private const string sourceKeyName = "ItEngineerSource";

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
        

            context.Response.Write(JsonUtil.ChangeToJson(new EngineerSourceServer().GetEngineerTable()));
  
            
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
