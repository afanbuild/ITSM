using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using StandardObject;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.SqlDAL.ResourceMoniter
{
    /// <summary>
    /// 资源监控状态查询
    /// </summary>
    public class ResourceState
    {
        /// <summary>
        /// Remoting Client-side
        /// </summary>
        private static readonly RemoteObject.IRemoteObject _proxy;
        /// <summary>
        /// 资源编号
        /// </summary>
        private String _resource_id;
        /// <summary>
        /// 初始化 Remote Object 对象
        /// </summary>
        static ResourceState()
        {
            /*
             * 创建 Remote Object
             * **/

            String str_resource_state_cacheLayer_objectUri = CommonDP
                .GetConfigValue("", "Resource-State-CacheLayer-Address");    // 取 Remote Object 地址

            if (String.IsNullOrEmpty(str_resource_state_cacheLayer_objectUri))
            { throw new Exception("请设置 Resource-State-CacheLayer-Address 配置项!"); }    // 配置项不存在或为空

            // using TCP protocol                
            TcpChannel chan = new TcpChannel();
            ChannelServices.RegisterChannel(chan);    // 注册 TCP 信道
            // Create an instance of the remote object
            _proxy = (RemoteObject.IRemoteObject)Activator.GetObject(typeof(RemoteObject.IRemoteObject),
                str_resource_state_cacheLayer_objectUri);    // 获取 Remote Object Proxy.
        }

        /// <summary>
        /// 使用资源编号, 实例化资源状态对象
        /// </summary>
        /// <param name="str_resource_id"></param>
        public ResourceState(String str_resource_id)
        {
            /*
             * 使用资源编号, 实例化资源状态对象
             * 
             * 若 str_resource_id 值为 null, 则赋予其空字串             
             * **/

            this._resource_id = Equ_DeskDP.GetEquCodeByID(str_resource_id);

            if (this._resource_id == null)
                this._resource_id = String.Empty;
        }

        /// <summary>
        /// 取资源支持的监控项集
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<String, String>> GetSupportKeyList()
        {
            /*
             * 使用 Remote Object 取资源支持的监控项集
             * 
             * 实现逻辑:
             *     1) 资源编号为空, 返回空集合
             *     2) 返回 JSON 字串翻译失败, 返回空集合
             *     3) 反序列化返回的 JSON 字串, 返回支持的监控项集
             * **/

            if (String.IsNullOrEmpty(this._resource_id))
            { return new List<KeyValuePair<string, string>>(); }

            String str_ret_json_text
                = ResourceState._proxy.GetSupportKeyListBy(_resource_id);    // 取该资源支持的监控项集            

            WrapperJSONResult result;
            Boolean isOkay;
            result = InterpertReuslt(str_ret_json_text, out isOkay);    // 翻译 JSON 字串

            if (!isOkay) return new List<KeyValuePair<string, string>>();    // 翻译不成功, 返回空集合

            List<KeyValuePair<String, String>> support_key_list
                    = Newtonsoft.Json.JsonConvert.DeserializeObject<List<KeyValuePair<String, String>>>(
                        result.Data.ToString());    // 反序列化 支持的监控项集 JSON 串.

            return support_key_list;
        }
        /// <summary>
        /// 取资源的监控项状态
        /// </summary>
        /// <returns></returns>
        public List<StandardObject.StandardData> GetResourceStateList()
        {
            if (String.IsNullOrEmpty(this._resource_id))
            { return new List<StandardObject.StandardData>(); }

            String str_ret_json_text
                = ResourceState._proxy.GetResourceStateBy(_resource_id);    // 取该资源的状态集            

            WrapperJSONResult result;
            Boolean isOkay;
            result = InterpertReuslt(str_ret_json_text, out isOkay);    // 翻译 JSON 字串

            if (!isOkay) return new List<StandardData>();    // 翻译不成功, 返回空集合

            List<StandardObject.StandardData> data
                    = Newtonsoft.Json.JsonConvert.DeserializeObject<List<StandardObject.StandardData>>(
                        result.Data.ToString());    // 反序列化 资产状态集 JSON 串

            return data;
        }

        /// <summary>
        /// 翻译返回结果
        /// </summary>
        /// <param name="str_json_text"></param>
        /// <param name="isOkay"></param>
        /// <returns></returns>
        private WrapperJSONResult InterpertReuslt(
            String str_json_text,
            out Boolean isOkay)
        {
            /*
             * 实现逻辑:             
             *     1) 调用返回状态不是 RetStatus.Okay, 返回空对象(null), isOkay = false;
             *     2) 若一切正常, 返回翻译后的 WrapperJSONResult 对象. isOkay = true.             
             * **/

            isOkay = false;

            WrapperJSONResult result;

            try
            {
                result = Newtonsoft.Json.JsonConvert
                .DeserializeObject<WrapperJSONResult>(str_json_text);
            }
            catch (Newtonsoft.Json.JsonException json_ex)
            {
                String str_error_msg = String.Format("JSON 反序列化错误: {0}\n JSON Text: {1}",
                    json_ex.Message, str_json_text);
                json_ex = new Newtonsoft.Json.JsonException(str_error_msg);

                E8Logger.Error(json_ex);

                throw json_ex;
            }

            if (((RetStatus)result.Status) != RetStatus.Okay)
            {
                String str_error_msg = String.Format("ResourceId:{0} [{1}]: {2}\nMethod-Name: GetSupportKeyList",
                    this._resource_id, (RetStatus)result.Status, result.Message);
                E8Logger.Error(str_error_msg);    // 调用返回状态不成功, 记录错误消息

                return null;
            }

            isOkay = true;

            return result;
        }
    }
}
