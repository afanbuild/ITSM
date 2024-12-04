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
using EpowerCom;
using E8ITSM_Phone.WebServiceITSM;
using EpowerGlobal;
using System.Xml;

namespace E8ITSM_Phone.Proxy
{
    public class message_object_wrapper
    {
        /// <summary>
        /// 取下一环节接收人员
        /// </summary>
        /// <param name="lngUserId">用户编号</param>
        /// <param name="lngMessageId">消息编号</param>
        /// <param name="lngFlowModelId">流程模型编号</param>
        /// <param name="lngActionId">操作编号[即点击的是[确定]按钮? 还是[提交]按钮?]</param>        
        public DataTable GetNextReceiver(long lngUserId,
            long lngMessageId,
            long lngFlowModelId,
            long lngActionId)
        {
            Message objMsg = new Message();
            string strRetValue = objMsg.GetNextReceivers(lngUserId, lngMessageId, lngFlowModelId,
                lngActionId, EpowerGlobal.e_SpecRightType.esrtNormal, 0, String.Empty,
                404 /* !! 这个参数没有任何意义 !! */);

            DataTable dtReceiver = SetTableStruct();
            GenerateMemberSelect(strRetValue, ref dtReceiver,
                lngUserId, lngMessageId, lngActionId,
                String.Empty, String.Empty, String.Empty);

            return dtReceiver;
        }

        /// <summary>
        /// 新增流程 - 事件单
        /// </summary>
        /// <param name="lngUserId">用户编号</param>
        /// <param name="lngFlowModelId">流程模型编号</param>
        /// <param name="lngMessageId">消息编号</param>        
        /// <param name="lngActionId">业务动作编号</param>
        /// <param name="lngLinkNodeId">下一环节编号</param>
        /// <param name="lngLinkNodeType">下一环节类型</param>                
        /// <param name="strFormXMLValue">表单内容</param>
        /// <param name="strReceivers">下一处理人员</param>
        public void AddFlow(long lngUserId,
            long lngFlowModelId,
            ref long lngMessageId,
            long lngActionId,
            long lngLinkNodeId,
            long lngLinkNodeType,
            string strFormXMLValue,
            string strReceivers,
            string strSubject)
        {            
            AddFlow(lngUserId, lngFlowModelId, ref lngMessageId,
                strSubject, lngActionId, lngLinkNodeId, lngLinkNodeType,
                0, String.Empty /* 处理意见 */, strFormXMLValue, strReceivers,
                "<attachments></attachments>" /* 附件 */, 0, e_FlowJoinType.efjtaNormal, 0, 0);
        }

        /// <summary>
        /// 新增流程
        /// </summary>
        /// <param name="lngUserId">用户编号</param>
        /// <param name="lngFlowModelId">流程模型编号</param>
        /// <param name="lngMessageId">消息编号</param>
        /// <param name="strSubject">摘要</param>
        /// <param name="lngActionId">业务动作编号</param>
        /// <param name="lngLinkNodeId">下一环节编号</param>
        /// <param name="lngLinkNodeType">下一环节类型</param>
        /// <param name="lngImportance">重要级别</param>
        /// <param name="strOpinionValue">处理意见</param>
        /// <param name="strFormXMLValue">表单内容</param>
        /// <param name="strReceivers">下一处理人员</param>
        /// <param name="strAttachment">附件字串</param>
        /// <param name="lngPreMessageId">?</param>
        /// <param name="flowJoinType">流程接入类别</param>
        /// <param name="lngExpectedLimit">期望时间</param>
        /// <param name="lngWarningLimit">警告时间</param>
        private void AddFlow(long lngUserId,
            long lngFlowModelId,
            ref long lngMessageId,
            string strSubject,
            long lngActionId,
            long lngLinkNodeId,
            long lngLinkNodeType,
            long lngImportance,
            string strOpinionValue,
            string strFormXMLValue,
            string strReceivers,
            string strAttachment,
            long lngPreMessageId,
            e_FlowJoinType flowJoinType,
            int lngExpectedLimit,
            int lngWarningLimit)
        {
            Message objMsg = new Message();

            objMsg.AddFlow(lngUserId,
                lngFlowModelId,
                lngMessageId,
                strSubject,
                lngActionId,
                lngLinkNodeId, lngLinkNodeType, lngImportance, strOpinionValue,
                strFormXMLValue, strReceivers, strAttachment, lngPreMessageId,
                flowJoinType, lngExpectedLimit, lngWarningLimit, ref lngMessageId);
        }

        /// <summary>
        /// 流程处理人员（1.多个人员值设置到dtNextReceivers，2.否则直接提交流程[1表示成功，2表示失败]）
        /// </summary>
        /// <param name="strRet"></param>
        /// <param name="dtNextReceivers"></param>
        /// <param name="lngUserID"></param>
        /// <param name="lngMessageID"></param>
        /// <param name="lngActionID"></param>
        /// <param name="strOpinion"></param>
        /// <param name="strValues"></param>
        /// <param name="strAttXml"></param>
        /// <returns></returns>
        private int GenerateMemberSelect(string strRet, ref DataTable dtNextReceivers, long lngUserID, long lngMessageID, long lngActionID, string strOpinion, string strValues, string strAttXml)
        {
            bool blnSuccess = false;
            e_FMNodeType lngCurrNodeType = e_FMNodeType.fmNormal;   //当前环节的类别

            e_ReceiveActorType lngCurrReceiveType = e_ReceiveActorType.eratNone; //当前接收者类别

            long lngReceiverID = 0;
            string strNodeID = "";
            string strNodeName = "";
            string strReceiverName = "";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(strRet);
            int iCount = 0;
            long lngLinkNodeID = long.Parse(xmlDoc.DocumentElement.Attributes["LinkNodeID"].Value);
            long lngLinkNodeType = long.Parse(xmlDoc.DocumentElement.Attributes["LinkNodeType"].Value);
            XmlNodeList nodes = xmlDoc.SelectNodes("NextReceivers/Nodes/Node");

            foreach (XmlNode node in nodes)
            {
                strNodeID = node.Attributes["ID"].Value;
                strNodeName = node.Attributes["Name"].Value;
                lngCurrNodeType = (e_FMNodeType)(int.Parse(node.Attributes["NodeType"].Value));
                XmlNodeList workers = node.SelectNodes("Workers/Worker");
                foreach (XmlNode wk in workers)
                {
                    string strWk = wk.Attributes["DeptPathXml"].Value;
                    string strDeptName = string.Empty;

                    if (!strWk.Equals("") || strWk != "")
                    {
                        XmlDocument xmlRec = new XmlDocument();
                        xmlRec.LoadXml(strWk);
                        XmlNodeList strWkL = xmlRec.DocumentElement.SelectNodes("Dept");
                        foreach (XmlNode xmlDeptNode in strWkL)
                        {
                            strDeptName = ((XmlElement)xmlDeptNode).GetAttribute("DeptName");
                        }
                    }

                    iCount++;
                    lngReceiverID = long.Parse(wk.Attributes["ID"].Value);
                    lngCurrReceiveType = (e_ReceiveActorType)(int.Parse(wk.Attributes["ReceiveType"].Value));
                    strReceiverName = wk.Attributes["Name"].Value;
                    DataRow dr = dtNextReceivers.NewRow();
                    dr["name"] = strDeptName;
                    dr["nodeID"] = strNodeID;
                    dr["nodeType"] = (int)lngCurrNodeType;
                    dr["receiveName"] = strReceiverName;
                    dr["receiveID"] = lngReceiverID;
                    dr["receiveType"] = (int)lngCurrReceiveType;
                    dr["lngLinkNodeID"] = lngLinkNodeID.ToString();
                    dr["lngLinkNodeType"] = lngLinkNodeType.ToString();
                    dtNextReceivers.Rows.Add(dr);
                }
                if (workers.Count == 0)
                {
                    iCount = 0;
                }
            }

            return 0;
        }

        /// <summary>
        /// 定义环节接收人员表
        /// </summary>
        /// <returns></returns>
        private static DataTable SetTableStruct()
        {
            DataTable dTable = new DataTable();
            //定义表结构

            dTable.Columns.Add("name", typeof(System.String));
            dTable.Columns.Add("nodeID", typeof(System.String));
            dTable.Columns.Add("nodeType", typeof(System.Int32));
            dTable.Columns.Add("receiveType", typeof(System.Int32));
            dTable.Columns.Add("receiveID", typeof(System.String));
            dTable.Columns.Add("receiveName", typeof(System.String));
            dTable.Columns.Add("lngLinkNodeID", typeof(System.String));
            dTable.Columns.Add("lngLinkNodeType", typeof(System.String));
            return dTable;
        }
    }
}
