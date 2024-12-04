using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.SqlDAL.Service;
using System.Threading;

namespace Epower.ITSM.Web.Ajax
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明

    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class HttpAjax : IHttpHandler
    {


        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.ContentType = "text/plain";
            //不让浏览器缓存 
            context.Response.Buffer = true;
            context.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            context.Response.AddHeader("pragma", "no-cache");
            context.Response.AddHeader("cache-control", "");
            context.Response.CacheControl = "no-cache";

            string Type = context.Request.Params["Type"];
            switch (Type)
            {
                case "pageAllType"://全局常用功能
                    string PageName = context.Request.Params["PageName"];
                    string pageUrl = context.Request.Params["pageUrl"];
                    string pageType = context.Request.Params["pageType"];
                    string UserId = context.Request.Params["UserId"];
                    context.Response.Write(pageAllType(PageName, pageUrl, pageType, UserId));
                    break;
                case "deleted"://删除常用功能
                    string dltPageName = context.Request.Params["PageName"];
                    string dltpageUrl = context.Request.Params["pageUrl"];
                    string strManager = context.Request.Params["strManager"];
                    string dltUserId = context.Request.Params["UserId"];
                    context.Response.Write(deleteType(dltPageName, dltpageUrl, strManager, dltUserId));
                    break;
                case "Equ":// 根据资产ID组合删除资产时的提示
                    string strEquIDs = context.Request.Params["strEquIDs"];
                    context.Response.Write(deleteEqusByIDs(strEquIDs));
                    break;
                case "EmailIssue":// 邮件自动报障碍                    
                    context.Response.Write(EmailIssue());
                    break;
                case "DelEquRel":   //在资产关联图中删除此资产与前一资产的关联

                    string strRelID = context.Request.Params["RelID"];
                    context.Response.Write(DelEquRel(strRelID));
                    break;
                case "normal_app_get_orderbyfieldandmenu":    // 通用表单, 取已排序的字段和分组菜单信息

                    long lngFlowModelID = long.Parse(context.Request.Params["flowmodelid"]);

                    lngFlowModelID = FlowDP.GetOFlowModelID(lngFlowModelID);
                    DataTable dtField = AppFieldConfigDP.GetOrderbyFieldAndMenu(lngFlowModelID);

                    OutputForNormalAppOrderbyField(context, dtField);
                    break;
                default:
                    break;
            }
        }

        #region 在资产关联图中删除此资产与前一资产的关联

        /// <summary>
        /// 在资产关联图中删除此资产与前一资产的关联

        /// </summary>
        /// <param name="strEquRelID"></param>
        /// <returns></returns>
        public string DelEquRel(string strEquRelID)
        {
            if (Equ_RelDP.DelEquRelByRelID(strEquRelID))
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }
        #endregion


        /// <summary>
        /// 邮件自动报账  --yanghw 2011-08-18
        /// </summary>
        /// <returns></returns>
        public string EmailIssue()
        {

            ///使用线程收取邮件
            //Thread d = new Thread(new ThreadStart(EmialIssue.getEmailIssue));
            EmialIssue.getEmailIssue();
            //d.Start();


            return "1";
        }

        /// <summary>
        /// 设置为全局常用类别
        /// </summary>
        /// <param name="pageName"></param>
        /// <param name="pageUrl"></param>
        /// <param name="PageType"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        private string pageAllType(string pageName, string pageUrl, string PageType, string UserId)
        {

            //if (httpAjaxSC.insertPageAllType(pageName, pageUrl, PageType, UserId))
            //{
            //    //操作成功
            //    return "0";
            //}
            //else
            //{
            //    //操作失败
            //    return "1";
            //}
            return "1";
        }
        /// <summary>
        /// 删除常用功能
        /// </summary>
        /// <param name="pageName"></param>
        /// <param name="pageUrl"></param>
        /// <param name="strManager"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        private string deleteType(string pageName, string pageUrl, string strManager, string UserId)
        {
            //bool bReturn = httpAjaxSC.IsPageExists(long.Parse(UserId), pageUrl);  //判断PageUrl是否存在
            //if (bReturn)
            //{
            //    bool bReturn2 = httpAjaxSC.deletePageAllType(pageName, pageUrl, strManager, UserId);  //取消
            //    if (bReturn2)  //更新成功
            //    {
            //        return "0";
            //    }
            //    else
            //    {
            //        //操作失败
            //        return "1";
            //    }
            //}
            //else
            //{
            //    return "2";
            //}
            return "1";

        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #region 根据资产ID组合删除资产时的提示
        /// <summary>
        /// 根据资产ID组合删除资产
        /// </summary>
        /// <param name="strEquIDs"></param>
        private string deleteEqusByIDs(string strIDs)
        {
            string strTip = string.Empty;
            //获取各个资产对应的提示信息

            strIDs = strIDs.Substring(0, strIDs.Length - 1);

            string[] strEquIDs = strIDs.Split(',');

            Equ_DeskDP ee = new Equ_DeskDP();

            for (int i = 0; i < strEquIDs.Length; i++)
            {
                string strID = strEquIDs[i].ToString();         //资产ID
                ee = ee.GetReCorded(long.Parse(strID));
                string strName = ee.Name;                       //资产名称

                string strEqus = Equ_DeskDP.GetEquNamesByEquID(strID);        //根据资产ID得到其所影响的资产名称组合

                if (strEqus.Length > 0) strEqus = strEqus.Substring(0, strEqus.Length - 1);         //将最后一个逗号去掉

                if (strEqus != "")
                {
                    //如果有影响的资产，则给出提示
                    strTip += strName + "的删除会影响到:" + strEqus + ";";
                }
            }
            return strTip;
        }
        #endregion



        /// <summary>
        ///  通用表单 已排序字段
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dtField"></param>
        private void OutputForNormalAppOrderbyField(HttpContext context, DataTable dtField)
        {
            String strJSONText = Newtonsoft.Json.JsonConvert.SerializeObject(dtField, Newtonsoft.Json.Formatting.Indented);
            context.Response.Write(strJSONText);
        }
    }
}
