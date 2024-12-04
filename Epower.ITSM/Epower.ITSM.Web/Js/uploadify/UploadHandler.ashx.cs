using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Epower.ITSM.SqlDAL;
using EpowerCom;
using EpowerGlobal;
using System.Web.SessionState;

namespace Epower.ITSM.Web.Js.uploadify
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class UploadHandler : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Charset = "utf-8";

            HttpPostedFile file = context.Request.Files["Filedata"];
            
            if (file != null)
            {
                string strTmpCatalog = CommonDP.GetConfigValue("TempCataLog", "TempCataLog");
                string strFileCatalog = CommonDP.GetConfigValue("TempCataLog", "FileCataLog");
                string strTmpSubPath = "";
                string strTmpPath = "";

                if (context.Session["tmpsubpath"] == null || context.Session["tmpsubpath"].ToString() == "")
                {
                    Random rnd = new Random();
                    strTmpSubPath = rnd.Next(100000000).ToString();
                    context.Session["tmpsubpath"] = strTmpSubPath;
                }
                else
                {
                    strTmpSubPath = context.Session["tmpsubpath"].ToString();
                }

                if (strTmpCatalog.EndsWith(@"\") == false)
                {
                    strTmpPath = strTmpCatalog + @"\" + strTmpSubPath;
                }
                else
                {
                    strTmpPath = strTmpCatalog + strTmpSubPath;
                }
                MyFiles.AutoCreateDirectory(strTmpPath);

                long lngNextFileID = EPGlobal.GetNextID("FILE_ID");
                string filename = strTmpPath + @"\" + lngNextFileID.ToString();

                file.SaveAs(filename);
                //下面这句代码缺少的话，上传成功后上传队列的显示不会自动消失
                context.Response.Write(file.FileName + "|" + strTmpSubPath + "|" + lngNextFileID);
            }
            else
            {
                context.Response.Write("0");
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
