using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Xml;
using System.Collections.Generic;
using Newtonsoft.Json;
using EpowerGlobal;
using EpowerCom;

namespace E8ITSM_Phone.Toos
{
    public class CommonTool
    {
        /// <summary>
        /// 创建接收人员的XML对象
        /// </summary>
        /// <param name="dictReceiver">接收人员</param>
        /// <returns></returns>
        public static XmlDocument CreateXMLWithRecevier(List<Dictionary<string, string>> listReceiver)
        {
            XmlDocument xmlRec = new XmlDocument();
            xmlRec.LoadXml("<Receivers></Receivers>");


            /*"[{\"name\":\"技术部\",\"nodeID\":\"8\",\"nodeType\":33,\"
             * receiveType\":20,\"receiveID\":\"12076\",\"receiveName\":\"技术部\",\"
             * lngLinkNodeID\":\"8\",\"lngLinkNodeType\":\"33\"}]"
             * */
            foreach (System.Collections.Generic.Dictionary<String, String>
                dictReceiver in listReceiver)
            {
                e_FMNodeType lngCurrNodeType = (e_FMNodeType)(int.Parse(dictReceiver["nodeType"].ToString()));
                e_ReceiveActorType lngCurrReceiveType = (e_ReceiveActorType)(int.Parse(dictReceiver["receiveType"].ToString()));
                long lngReceiverID = long.Parse(dictReceiver["receiveID"].ToString());
                string strReceiverName = dictReceiver["receiveName"].ToString();
                long lngLinkNodeID = long.Parse(dictReceiver["lngLinkNodeID"].ToString());
                long lngLinkNodeType = long.Parse(dictReceiver["lngLinkNodeType"].ToString());

                XmlElement xmlEle = xmlRec.CreateElement("Receiver");
                xmlEle.SetAttribute("ID", "1");
                xmlEle.SetAttribute("NodeID", dictReceiver["nodeID"].ToString());
                xmlEle.SetAttribute("NodeType", ((int)lngCurrNodeType).ToString());
                xmlEle.SetAttribute("UserID", lngReceiverID.ToString());
                xmlEle.SetAttribute("Name", strReceiverName);
                xmlEle.SetAttribute("ActorType", "Worker_");

                if (lngCurrNodeType == e_FMNodeType.fmOrgNode || lngCurrNodeType == e_FMNodeType.fmDisperse)
                {
                    //机构环节或发散环节只选择组
                    xmlEle.SetAttribute("ReceiveType", ((int)lngCurrReceiveType).ToString());
                }
                else
                {
                    //普通环节设置接收角色为空
                    xmlEle.SetAttribute("ReceiveType", "");
                }

                xmlRec.DocumentElement.AppendChild(xmlEle);
            }

            return xmlRec;
        }
    }
}
