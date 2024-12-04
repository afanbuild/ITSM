using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Epower.ITSM.SqlDAL.Mobile;
using EpowerGlobal;
using Newtonsoft.Json;

namespace E8ITSM_Phone.WebServiceITSM
{
    /// <summary>
    /// 取已办事项列表
    /// </summary>
    [WebService(Namespace = "http://feifanE8.cn/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]   
    public class HasProcessedService : System.Web.Services.WebService
    {
        /// <summary>
        /// 获取已办事项单列表.         
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="messageStatus">消息状态</param>
        /// <param name="isDeleted">是否删除</param>
        /// <returns>JSON字串</returns>
        [WebMethod]
        public string HasProcessed(int userId,
            int messageStatus, int isDeleted)
        {
            /* ##目前仅包括 事件单和变更单## */

            HasProcessedDP hasProcessedDP = new HasProcessedDP(userId,
                e_MessageStatus.emsFinished, e_Deleted.eNormal);

            DataTable dt = hasProcessedDP.GetMsgList();            

            return " { has_processed:" + JsonConvert.SerializeObject(dt) + " } ";            
        }
    }
}
