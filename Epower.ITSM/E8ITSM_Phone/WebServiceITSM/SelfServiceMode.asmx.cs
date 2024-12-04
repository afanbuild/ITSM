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
using E8ITSM_Phone.Toos;
using System.Xml;
using E8ITSM_Phone.Proxy;
using EpowerCom;

namespace E8ITSM_Phone.WebServiceITSM
{
    /// <summary>
    /// SelfServiceMode 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://feifanE8.cn/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class SelfServiceMode : System.Web.Services.WebService
    {
        /// <summary>
        /// 服务目录根编号
        /// </summary>
        private static int SERVER_LIST_ROOT = -1;

        /// <summary>
        /// 空消息编号, 在流程起草阶段使用
        /// </summary>
        private static int EMPTY_MESSAGE_ID = 0;

        /// <summary>
        /// 取指定用户的服务目录层级关系
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string GetServiceList(long lngUserId)
        {
            Proxy.frm_service_list_wrapper service_list_wrapper
                = new E8ITSM_Phone.Proxy.frm_service_list_wrapper();

            DataTable dt = service_list_wrapper.GetServiceListStruct();
            service_list_wrapper.GetServiceListByUserId(lngUserId, SERVER_LIST_ROOT, dt);

            String jsonText = JsonConvert.SerializeObject(dt);
            return jsonText;
        }

        /// <summary>
        /// 取流程模型的业务动作
        /// </summary>
        /// <param name="lngUserId">用户编号</param>
        /// <param name="lngFlowModelId">流程模型编号</param>
        /// <returns></returns>
        [WebMethod]
        public string GetActionList(long lngUserId,
            long lngFlowModelId)
        {
            lngFlowModelId = FlowModel.GetLastVersionFlowModelID(lngFlowModelId);//获取最新FlowModelID;

            EpowerCom.objFlow oFlow = new EpowerCom.objFlow(lngUserId, lngFlowModelId, EMPTY_MESSAGE_ID);
            long lngNextNodeModelId = oFlow.NodeModelID;    // 下一环节模型编号

            FlowDP f = new FlowDP();

            return f.LoadActions(lngFlowModelId, lngNextNodeModelId, false /* 是否阅知, 在起草阶段不用 */);
        }

        /// <summary>
        /// 取起草环节的下一环节接收人
        /// </summary>
        /// <param name="lngUserId">用户编号</param>
        /// <param name="lngMessageId">消息编号</param>
        /// <param name="lngFlowModelId">流程模型编号</param>
        /// <param name="lngActionId">业务动作编号</param>        
        /// <returns>接收人列表</returns>
        [WebMethod]
        public string GetNextReceiver(long lngUserId,
            long lngMessageId,
            long lngFlowModelId,
            long lngActionId)
        {
            Proxy.message_object_wrapper msgObjWrapper = new E8ITSM_Phone.Proxy.message_object_wrapper();
            lngFlowModelId = FlowModel.GetLastVersionFlowModelID(lngFlowModelId);//获取最新FlowModelID;

            DataTable dtReceiver = msgObjWrapper.GetNextReceiver(lngUserId, lngMessageId,
                lngFlowModelId, lngActionId);

            return JsonConvert.SerializeObject(dtReceiver);
        }

        /// <summary>
        /// 创建事件流程
        /// </summary>
        /// <param name="lngUserId">用户编号</param>
        /// <param name="lngMessageId">消息编号</param>
        /// <param name="lngFlowModelId">流程模型编号</param>
        /// <param name="lngActionId">业务动作编号</param>
        /// <param name="lngServiceListId">服务目录编号</param>
        /// <param name="strRemark">详细描述</param>
        /// <param name="strNextReceiver">下一接收人</param>
        /// <returns></returns>
        [WebMethod]
        public string AddFlow(long lngUserId,
            long lngMessageId,
            long lngFlowModelId,
            long lngActionId,
            long lngServiceListId,
            string strRemark,
            string strNextReceiver)
        {
            lngFlowModelId = FlowModel.GetLastVersionFlowModelID(lngFlowModelId);//获取最新FlowModelID;

            // 构造表单内容串
            FormValueGenerator fvGen = new FormValueGenerator(lngUserId);

            String strSubject = fvGen.AddOtherInfo(lngServiceListId);
            fvGen.AddCustomerInfo();
            fvGen.AddEquipmentInfo();
            fvGen.AddIssueTemplateInfo(lngServiceListId);
            fvGen.AddFieldValue("Content", strRemark);            

            try
            {
                // strNextReceiver = strNextReceiver.Remove(0, 1);
                // strNextReceiver = strNextReceiver.Remove(strNextReceiver.Length - 1, 1);
                
                // 构造接收人员串
                System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, string>> listReceiver
                    = JsonConvert.DeserializeObject<System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, string>>>(strNextReceiver);

                XmlDocument xmlReceiver = CommonTool.CreateXMLWithRecevier(listReceiver);

                long lngLinkNodeId = long.Parse(listReceiver[0]["lngLinkNodeID"]);
                long lngLinkNodeType = long.Parse(listReceiver[0]["lngLinkNodeType"]);                

                // 提交流程
                message_object_wrapper msg_obj_wrapper = new message_object_wrapper();
                msg_obj_wrapper.AddFlow(lngUserId,
                    lngFlowModelId,
                    ref lngMessageId,
                    lngActionId, lngLinkNodeId, lngLinkNodeType,
                    fvGen.ToString(),
                    xmlReceiver.InnerXml,
                    strSubject);
            }
            catch (Exception)
            {
                throw;
            }

            return "{state: success}";
        }
    }
}
