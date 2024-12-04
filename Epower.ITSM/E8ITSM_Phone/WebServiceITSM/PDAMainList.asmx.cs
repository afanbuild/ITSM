/*******************************************************************
 *
 * Description:手机客户端
 * 
 * 
 * Create By  :谭雨
 * Create Date:2012年8月7日
 * *****************************************************************/
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using EpowerCom;
using System.IO;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using Epower.ITSM.SqlDAL.Mobile;
using Epower.ITSM.Base;
using E8ITSM_Phone.Toos;

namespace E8ITSM_Phone.WebServiceITSM
{
    /// <summary>
    /// PDAMainList 的摘要说明

    /// </summary>
    [WebService(Namespace = "http://feifanE8.cn/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。

    // [System.Web.Script.Services.ScriptService]
    public class PDAMainList : System.Web.Services.WebService
    {
        /// <summary>
        /// 首页（待办事项，待接收事项，我登记事项的数量）

        /// </summary>
        /// <param name="userId">登陆用户ID</param>
        /// <returns>Json字符串</returns>
        [WebMethod]
        public string GetMainListMessageCount(long userID, int tp)
        {
            string strOut = string.Empty;
            string jsonText = string.Empty;
            try
            {
                String strLatestTime = GetLatestDateTimeOfDB();

                //待办事项
                int countUndo = MobileDP.GetUndoMessageCount(userID, (eOA_TracePeriod)tp);
                jsonText += "[{'NAME': 'Type1','COUNT':" + countUndo + ", LAST_DATETIME: '" + strLatestTime + "'},";

                //待接收事项

                int countReceive = MobileDP.GetReceiveMessageListCount(userID, (eOA_TracePeriod)tp);
                jsonText += "{'NAME': 'Type2','COUNT':" + countReceive + ", LAST_DATETIME: '" + strLatestTime + "'},";

                //我登记事项

                int countMyReg = MobileDP.GetMyRegMessageUnFinishedCount(userID, (eOA_TracePeriod)tp);
                jsonText += "{'NAME': 'Type3','COUNT':" + countMyReg + ", LAST_DATETIME: '" + strLatestTime + "'},";

                //阅读事项
                int countUnRead = MobileDP.GetUnReadMessageCount(userID, (eOA_TracePeriod)tp);
                jsonText += "{'NAME': 'Type4','COUNT':" + countUnRead + ", LAST_DATETIME: '" + strLatestTime + "'},";

                //参与事项
                int countMyParticipate = MobileDP.GetMyParticipateMattersCount(userID, (eOA_TracePeriod)tp);
                jsonText += "{'NAME': 'Type5','COUNT':" + countMyParticipate + ", LAST_DATETIME: '" + strLatestTime + "'},";

                //已办事项
                HasProcessedDP hasProcessDP = new HasProcessedDP(int.Parse(userID.ToString()),
                    EpowerGlobal.e_MessageStatus.emsFinished,
                    EpowerGlobal.e_Deleted.eNormal);

                int countHasProcessed = hasProcessDP.GetMsgList().Rows.Count;
                jsonText += "{'NAME': 'Type6','COUNT':" + countHasProcessed + ", LAST_DATETIME: '" + strLatestTime + "'},";

                // 事件登单                
                jsonText += "{'NAME': 'Type7','COUNT':0, LAST_DATETIME: '" + strLatestTime + "'}]";

                strOut = "{nameList:" + jsonText + "}";
            }
            catch
            {
                strOut = "errorNET";
            }
            return strOut;
        }


        /// <summary>
        /// 获取用户未处理的事件
        /// </summary>
        /// <param name="userId">登陆用户ID</param>
        /// <returns>Json字符串</returns>
        [WebMethod]
        public string GetUndoMessageList(long userID, int tp)
        {
            string strOut = string.Empty;
            string jsonText = string.Empty;

            DataTable dt = MobileDP.GetUndoMessageList(userID, (eOA_TracePeriod)tp);

            //str_json = JsonConvert.SerializeObject(dt, Formatting.Indented);

            JsonSerializer js = JsonToos.GetJsonSerializer();

            //转换为为Json Array
            jsonText = JArray.FromObject(dt, js).ToString();

            String strLatestTime = GetLatestDateTimeOfDB();

            strOut = "{nameList:" + jsonText + ", LatestTime: '" + strLatestTime + "' }";
            return strOut;
        }


        /// <summary>
        /// 获取用户未接收列表
        /// </summary>
        /// <param name="userId">登陆用户ID</param>
        /// <returns>Json字符串</returns>
        [WebMethod]
        public string GetReceiveMessageList(long userID, int tp)
        {
            string strOut = string.Empty;
            string jsonText = string.Empty;

            DataTable dt = MobileDP.GetReceiveMessageList(userID, (eOA_TracePeriod)tp);

            JsonSerializer js = JsonToos.GetJsonSerializer();
            //转换为为Json Array
            jsonText = JArray.FromObject(dt, js).ToString();

            String strLatestTime = GetLatestDateTimeOfDB();

            strOut = "{nameList:" + jsonText + ", LatestTime: '" + strLatestTime + "' }";
            return strOut;
        }


        /// <summary>
        /// 获取用户未完成

        /// </summary>
        /// <param name="userId">登陆用户ID</param>
        /// <returns>Json字符串</returns>
        [WebMethod]
        public string GetMyRegMessageUnFinishedList(long userID, int tp)
        {
            string strOut = string.Empty;
            string jsonText = string.Empty;

            DataTable dt = MobileDP.GetMyRegMessageUnFinished(userID, (eOA_TracePeriod)tp);

            JsonSerializer js = JsonToos.GetJsonSerializer();
            //转换为为Json Array
            jsonText = JArray.FromObject(dt, js).ToString();

            strOut = "{nameList:" + jsonText + "}";
            return strOut;
        }

        /// <summary>
        /// 获取阅读事项
        /// </summary>
        /// <param name="userId">登陆用户ID</param>
        /// <returns>Json字符串</returns>
        [WebMethod]
        public string GetUnReadMessage(long userID, int tp)
        {
            string strOut = string.Empty;
            string jsonText = string.Empty;

            DataTable dt = MobileDP.GetUnReadMessage(userID, (eOA_TracePeriod)tp);

            JsonSerializer js = JsonToos.GetJsonSerializer();
            //转换为为Json Array
            jsonText = JArray.FromObject(dt, js).ToString();

            strOut = "{nameList:" + jsonText + "}";
            return strOut;
        }

        /// <summary>
        /// 获取参与事项
        /// </summary>
        /// <param name="userId">登陆用户ID</param>
        /// <returns>Json字符串</returns>
        [WebMethod]
        public string GetMyParticipateMatters(long userID, int tp)
        {
            string strOut = string.Empty;
            string jsonText = string.Empty;

            DataTable dt = MobileDP.GetMyParticipateMatters(userID, (eOA_TracePeriod)tp);

            JsonSerializer js = JsonToos.GetJsonSerializer();
            //转换为为Json Array
            jsonText = JArray.FromObject(dt, js).ToString();

            strOut = "{nameList:" + jsonText + "}";
            return strOut;
        }

        #region  sunshaozong@gmail.com
        /// <summary>
        /// 取自上次更新后的新待办事项
        /// </summary>
        /// <param name="lngUserId">用户编号</param>
        /// <param name="lastUpdateTime">上次更新时间</param>
        /// <param name="tp">数据量</param>
        /// <returns></returns>
        [WebMethod]
        public string GetUndoMessageListByDatetime(long lngUserId,
            string strLastUpdateTime,
            int tp)
        {
            string strOut = string.Empty;
            string jsonText = string.Empty;

            DataTable dt = MobileDP.GetUndoMessageList(lngUserId,
                DateTime.Parse(strLastUpdateTime), (eOA_TracePeriod)tp);

            JsonSerializer js = JsonToos.GetJsonSerializer();

            //转换为为Json Array
            jsonText = JArray.FromObject(dt, js).ToString();

            return "{nameList:" + jsonText + "}";
        }

        /// <summary>
        /// 取自上次更新后的用户未接收列表
        /// </summary>
        /// <param name="lngUserID">用户编号</param>
        /// <param name="strLastUpdateTime">上次更新时间</param>
        /// <param name="tp">数据量</param>
        /// <returns></returns>
        [WebMethod]
        public string GetReceiveMessageListByDatetime(long lngUserID,
            string strLastUpdateTime,
            int tp)
        {
            string strOut = string.Empty;
            string jsonText = string.Empty;

            DataTable dt = MobileDP.GetReceiveMessageList(lngUserID,
                DateTime.Parse(strLastUpdateTime), (eOA_TracePeriod)tp);

            JsonSerializer js = JsonToos.GetJsonSerializer();
            //转换为为Json Array
            jsonText = JArray.FromObject(dt, js).ToString();

            return "{nameList:" + jsonText + "}";
        }

        /// <summary>
        /// 获取数据库服务器的最新时间, 用于在手机端检测是否有新到
        /// 待办/待接收事项.  [应用服务器和数据库服务器的时间不同步]
        /// </summary>
        /// <returns></returns>
        private String GetLatestDateTimeOfDB()
        {
            DataTable dt = Epower.ITSM.SqlDAL.CommonDP.ExcuteSqlTable("SELECT SYSDATE FROM dual");
            return DateTime.Parse(dt.Rows[0][0].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
        }
        #endregion
    }
}
