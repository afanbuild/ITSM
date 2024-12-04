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
using EpowerCom;
using Epower.ITSM.Base;
using EpowerGlobal;
using System.Text;
using Epower.DevBase.BaseTools;
using appDataProcess;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Epower.ITSM.SqlDAL;
using System.Web.UI.MobileControls;
using System.Xml;
using System.Collections.Generic;
using Epower.ITSM.SqlDAL.Mobile;
using Epower.DevBase.Organization.SqlDAL;
using E8ITSM_Phone.Toos;

namespace E8ITSM_Phone.WebServiceITSM
{
    /// <summary>
    /// FlowDP 流程处理接口
    /// </summary>
    [WebService(Namespace = "http://feifanE8.cn/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。

    // [System.Web.Script.Services.ScriptService]
    public class FlowDP : System.Web.Services.WebService
    {
        #region 获取跳转路径
        /// <summary>
        /// 获取跳转路径
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="messageID"></param>
        /// <param name="flowModelID"></param>
        /// <returns>json</returns>
        [WebMethod]
        public string Flow_Normal(long userID, long messageID, long flowModelID)
        {
            long lngUserID = userID;
            long lngMessageID = messageID;// MobileloginDP.GetNewMessageID(messageID);
            long lngFlowModelID = flowModelID;
            string strUrl = string.Empty;
            string jsonText = string.Empty;
            string strReadOnly = "false";
            eOA_FlowProcessType iFPType = eOA_FlowProcessType.efptReadFinished;  //缺省值

            try
            {
                MessageEntity msgObject = new MessageEntity(lngMessageID);
                Message.SetMessageReadStatus(lngMessageID);
                long lngActorType = (long)msgObject.ActorType;
                objFlow oFlow = new objFlow(lngUserID, lngFlowModelID, lngMessageID);
                string strPage = epApp.GetStartWebFormByMessageP(lngMessageID);

                //生成跳转页面名称
                strPage = strPage.Substring(0, strPage.Length - 5);

                if (oFlow.MessageID != 0)
                {
                    //设置iFPType值
                    SetFlowProcessType(lngUserID, ref strReadOnly, ref iFPType, oFlow);
                }
                else
                {
                    jsonText = "noMessage";
                    return jsonText;
                }

                bool blnBackV = false; //退回
                bool blnTakeBackV = false;//回收
                bool blnReceiveV = false;//接收
                bool TakeOver = false;//交办
                bool CanTransmit = false;//沟通
                bool CanCommunic = false;//加签

                //判断退回，回收，接收，交办，沟通，加签按钮是否可见
                SetPagePermissions(lngMessageID, lngFlowModelID, iFPType, oFlow, ref blnBackV,
                    ref blnTakeBackV, ref blnReceiveV, ref TakeOver, ref CanTransmit, ref CanCommunic);

                string[] strP = strPage.Split("/".ToCharArray());
                strPage = "[{'name':'" + strP[strP.Length - 1].ToString() + "','flowModelID':" + lngFlowModelID.ToString() + ",'flowID':"
                    + oFlow.FlowID.ToString() + ",'messageID':" + lngMessageID.ToString() + ",'appID':" + oFlow.AppID.ToString() 
                    + ",'readOnly':" + strReadOnly + ",'blnBackV':" + blnBackV + ",'blnTakeBackV':" + blnTakeBackV 
                    + ",'blnReceiveV':" + blnReceiveV + ",'canTransmit':" + CanTransmit + ",'canCommunic':" 
                    + CanCommunic + ",'takeOver':" + TakeOver + ",'flowProcessType':" + ((int)iFPType).ToString() 
                    + ",'actorType':" + lngActorType.ToString() + "}]";

                jsonText = "{nameList:" + strPage + "}";
            }
            catch
            {
                jsonText = "errorNET";
            }
            return jsonText;
        }

        /// <summary>
        /// 获取跳转路径2
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="messageID"></param>
        /// <param name="flowModelID"></param>
        /// <returns>json</returns>
        [WebMethod]
        public string Flow_Normal2(long userID, long flowID, long flowModelID)
        {
            long lngUserID = userID;
            long lngMessageID = MobileDP.GetMessageID(flowID, userID);
            long lngFlowModelID = flowModelID;
            string strUrl = string.Empty;
            string jsonText = string.Empty;
            eOA_FlowProcessType iFPType = eOA_FlowProcessType.efptReadFinished;  //缺省值
            string strReadOnly = "false";

            try
            {
                MessageEntity msgObject = new MessageEntity(lngMessageID);
                Message.SetMessageReadStatus(lngMessageID);
                objFlow oFlow = new objFlow(lngUserID, lngFlowModelID, lngMessageID);
                long lngActorType = (long)msgObject.ActorType;
                string strPage = epApp.GetStartWebFormByMessageP(lngMessageID);

                //生成跳转页面名称
                strPage = strPage.Substring(0, strPage.Length - 5);

                if (oFlow.MessageID != 0)
                {
                    //设置iFPType值
                    SetFlowProcessType(lngUserID,  ref strReadOnly, ref iFPType,oFlow);
                }
                else
                {
                    jsonText = "noMessage";
                    return jsonText;
                }

                bool blnBackV = false; //退回
                bool blnTakeBackV = false;//回收
                bool blnReceiveV = false;//接收
                bool TakeOver = false;//交办
                bool CanTransmit = false;//沟通
                bool CanCommunic = false;//加签

                //判断退回和回收按钮是否可见
                SetPagePermissions(lngMessageID, lngFlowModelID, iFPType, oFlow, ref blnBackV,
                    ref blnTakeBackV, ref blnReceiveV, ref TakeOver, ref CanTransmit, ref CanCommunic);

                string[] strP = strPage.Split("/".ToCharArray());
                strPage = "[{'name':'" + strP[strP.Length - 1].ToString() + "','flowModelID':" + lngFlowModelID.ToString() + ",'flowID':"
                    + oFlow.FlowID.ToString() + ",'messageID':" + lngMessageID.ToString() + ",'appID':" + oFlow.AppID.ToString()
                    + ",'readOnly':" + strReadOnly + ",'blnBackV':" + blnBackV + ",'blnTakeBackV':" + blnTakeBackV
                    + ",'blnReceiveV':" + blnReceiveV + ",'canTransmit':" + CanTransmit + ",'canCommunic':"
                    + CanCommunic + ",'takeOver':" + TakeOver + ",'flowProcessType':" + ((int)iFPType).ToString()
                    + ",'actorType':" + lngActorType.ToString() + "}]";

                jsonText = "{nameList:" + strPage + "}";
            }
            catch
            {
                jsonText = "errorNET";
            }
            return jsonText;
        }

        /// <summary>
        /// 判断退回，回收，接收，交办，沟通，加签按钮是否可见
        /// </summary>
        /// <param name="lngMessageID"></param>
        /// <param name="lngFlowModelID"></param>
        /// <param name="iFPType"></param>
        /// <param name="oFlow"></param>
        /// <param name="blnBackV"></param>
        /// <param name="blnTakeBackV"></param>
        /// <param name="blnReceiveV"></param>
        /// <param name="TakeOver"></param>
        /// <param name="CanTransmit"></param>
        /// <param name="CanCommunic"></param>
        private static void SetPagePermissions(long lngMessageID, long lngFlowModelID, eOA_FlowProcessType iFPType, objFlow oFlow, ref bool blnBackV, ref bool blnTakeBackV, ref bool blnReceiveV, ref bool TakeOver, ref bool CanTransmit, ref bool CanCommunic)
        {
            switch (iFPType)
            {

                case eOA_FlowProcessType.efptReadFinished:
                case eOA_FlowProcessType.efptNormalFinished:
                    if (oFlow.TakeBackRight == e_IsTrue.fmTrue)
                    {
                        blnTakeBackV = MessageDep.CanTakeBackFlow(lngMessageID);
                    }
                    break;

                case eOA_FlowProcessType.efptNew:

                    break;
                case eOA_FlowProcessType.efptNormal:


                    if (oFlow.BackRight == e_IsTrue.fmTrue)
                    {
                        blnBackV = MessageDep.CanSendBackFlow(lngMessageID);
                    }

                    #region 添加展示传阅按钮
                    Hashtable ht = FlowModel.GetNodeSpecRights20090610(lngFlowModelID, oFlow.NodeModelID);//获取所有特殊权限 

                    if (ht["TakeOver"] != null && ht["TakeOver"].ToString() == "1")
                    {
                        TakeOver = true;

                    }

                    if (lngMessageID != 0)
                    {

                        if (ht["CanTransmit"] != null && ht["CanTransmit"].ToString() == "1")
                        {
                            CanTransmit = true;
                        }
                        if (ht["CanCommunic"] != null && ht["CanCommunic"].ToString() == "1")
                        {
                            CanCommunic = true;
                        }
                    }
                    #endregion

                    break;
                case eOA_FlowProcessType.eftpReceiveMsg:
                    blnReceiveV = true;
                    break;
                case eOA_FlowProcessType.efptReader:
                case eOA_FlowProcessType.eftpLookOtherMsg:
                case eOA_FlowProcessType.eftpWaitingMsg:
                case eOA_FlowProcessType.eftpStopMsg:
                    break;
                default:
                    break;

            }
        }

        /// <summary>
        /// 设置iFPType值
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="strReadOnly"></param>
        /// <param name="iFPType"></param>
        /// <param name="oFlow"></param>
        private static void SetFlowProcessType(long lngUserID, ref string strReadOnly, ref eOA_FlowProcessType iFPType, objFlow oFlow)
        {
            if (oFlow.ReceiverID != lngUserID && oFlow.ReceiverID != 0)
            {
                iFPType = eOA_FlowProcessType.eftpLookOtherMsg;
                strReadOnly = "true";
            }
            else
            {
                if (oFlow.MessageStatus == e_MessageStatus.emsHandle)
                {
                    if (oFlow.ReceiverID == 0)
                    {
                        iFPType = eOA_FlowProcessType.eftpReceiveMsg;
                        strReadOnly = "true";
                    }
                    else
                    {
                        if (oFlow.ActorClass == e_ActorClass.fmMasterActor || oFlow.ActorClass == e_ActorClass.fmInfluxActor)
                        {
                            iFPType = eOA_FlowProcessType.efptNormal;
                        }
                        else
                        {
                            iFPType = eOA_FlowProcessType.efptReader;
                        }
                    }
                }
                else if (oFlow.MessageStatus == e_MessageStatus.emsFinished || oFlow.MessageStatus == e_MessageStatus.emsWaiting || oFlow.MessageStatus == e_MessageStatus.emsStop)
                {
                    strReadOnly = "true";
                    if (oFlow.MessageStatus == e_MessageStatus.emsWaiting)
                    {
                        iFPType = eOA_FlowProcessType.eftpWaitingMsg;
                    }
                    if (oFlow.MessageStatus == e_MessageStatus.emsFinished)
                    {
                        if (oFlow.ActorClass == e_ActorClass.fmMasterActor || oFlow.ActorClass == e_ActorClass.fmInfluxActor)
                        {
                            iFPType = eOA_FlowProcessType.efptNormalFinished;
                        }
                        else
                        {
                            iFPType = eOA_FlowProcessType.efptReadFinished;
                        }
                    }
                    if (oFlow.MessageStatus == e_MessageStatus.emsStop)
                    {
                        iFPType = eOA_FlowProcessType.eftpStopMsg;
                    }
                }
            }
        }

        #endregion

        #region 事件提交

        /// <summary>
        /// 事件单条件 - 可选人的版本
        /// </summary>
        /// <param name="userID">用户编号</param>
        /// <param name="messageID">消息编号</param>
        /// <param name="flowModelID">流程模型编号</param>
        /// <param name="strActionID"></param>
        /// <param name="strOpinionValue">处理意见</param>
        /// <param name="strMemberSelectValue">下一主办人员( JSON 字串)</param>
        /// <returns></returns>
        [WebMethod]
        public string Flow_SenderClick(long userID, 
            long messageID, 
            long flowModelID, 
            string strActionID, 
            string strOpinionValue, 
            string strMemberSelectValue)
        {
            string jsonText = string.Empty;
            string strFormXMLValue = "";
            string strAttXml = "";
            string strReceiverName = "";

            bool blnSuccess = false;

            long lngLinkNodeID = 0;
            long lngLinkNodeType = 0;
            long lngCurrNodeID = 0;
            long lngReceiverID = 0;

            int iCount = 0;

            e_FMNodeType lngCurrNodeType = e_FMNodeType.fmNormal;   //当前环节的类别
            e_ReceiveActorType lngCurrReceiveType = e_ReceiveActorType.eratNone; //当前接收者类别
            //Message objMsg = new Message();

            //objMsg.AddFlow(lngUserID, 
            //    lngFlowModelID, 
            //    lngMessageID, 
            //    strSubject, 
            //    lngActionID, 
            //    lngLinkNodeID, 
            //    lngLinkNodeType, 
            //    lngImportance, 
            //    strOpinionValue, 
            //    strFormXMLValue, 
            //    strReceivers, 
            //    strAttachment, 
            //    lngPreMessageID, 
            //    (e_FlowJoinType)iFlowJoinType, 
            //    lngExpectedLimit, 
            //    lngWarningLimit, 
            //    ref lngMessageID);

            try
            {
                objFlow oFlow = new objFlow(userID, flowModelID, messageID);

                strFormXMLValue = Master_GetFormValues(oFlow.AppID, oFlow.FlowID, oFlow.OpID, userID);
                strAttXml = Message.GetAttachmentXml(oFlow.FlowID);

                XmlDocument xmlRec = new XmlDocument();
                xmlRec.LoadXml("<Receivers></Receivers>");
                Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(strMemberSelectValue);

                if (values.Count != 0)
                {
                    lngCurrNodeType = (e_FMNodeType)(int.Parse(values["iNodeType"].ToString()));
                    lngCurrReceiveType = (e_ReceiveActorType)(int.Parse(values["iReceiveType"].ToString()));
                    lngReceiverID = long.Parse(values["strReceiveID"].ToString());
                    strReceiverName = values["strReceiveName"].ToString();
                    lngLinkNodeID = long.Parse(values["lngLinkNodeID"].ToString());
                    lngLinkNodeType = long.Parse(values["lngLinkNodeType"].ToString());

                    XmlElement xmlEle = xmlRec.CreateElement("Receiver");
                    iCount++;
                    xmlEle.SetAttribute("ID", iCount.ToString());
                    xmlEle.SetAttribute("NodeID", values["strNodeID"].ToString());
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

                blnSuccess = DoFlowSubmit(userID, messageID, long.Parse(strActionID), lngLinkNodeID, lngLinkNodeType, strOpinionValue, strFormXMLValue, xmlRec.InnerXml, strAttXml);
                if (blnSuccess)
                {
                    jsonText = "Success";
                }
                else
                {
                    jsonText = "errorNET";
                }
            }
            catch
            {
                jsonText = "errorNET";
            }
            return jsonText;
        }

        /// <summary>
        /// 事件提交
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="messageID"></param>
        /// <param name="flowModelID"></param>
        /// <param name="strActionID"></param>
        /// <param name="strOpinionValue"></param>
        /// <param name="strMemberSelectValue"></param>
        /// <param name="intSpecRightType"></param>
        /// <returns></returns>
        [WebMethod]
        public string Flow_SenderClick2(long userID, long messageID, long flowModelID, string strActionID, string strOpinionValue, string strMemberSelectValue, int intSpecRightType)
        {
            string jsonText = string.Empty;
            string strFormXMLValue = "";
            string strAttXml = "";
            string strReceiverName = "";

            bool blnSuccess = false;

            long lngLinkNodeID = 0;
            long lngLinkNodeType = 0;
            long lngCurrNodeID = 0;
            long lngReceiverID = 0;

            e_FMNodeType lngCurrNodeType = e_FMNodeType.fmNormal;   //当前环节的类别
            e_ReceiveActorType lngCurrReceiveType = e_ReceiveActorType.eratNone; //当前接收者类别

            int iCount = 0;

            try
            {
                objFlow oFlow = new objFlow(userID, flowModelID, messageID);

                strFormXMLValue = Master_GetFormValues(oFlow.AppID, oFlow.FlowID, oFlow.OpID, userID);
                strAttXml = Message.GetAttachmentXml(oFlow.FlowID);

                XmlDocument xmlRec = new XmlDocument();
                xmlRec.LoadXml("<Receivers></Receivers>");
                Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(strMemberSelectValue);

                if (values.Count != 0)
                {
                    lngCurrNodeType = (e_FMNodeType)(int.Parse(values["iNodeType"].ToString()));
                    lngCurrReceiveType = (e_ReceiveActorType)(int.Parse(values["iReceiveType"].ToString()));
                    lngReceiverID = long.Parse(values["strReceiveID"].ToString());
                    strReceiverName = values["strReceiveName"].ToString();
                    lngLinkNodeID = long.Parse(values["lngLinkNodeID"].ToString());
                    lngLinkNodeType = long.Parse(values["lngLinkNodeType"].ToString());

                    XmlElement xmlEle = xmlRec.CreateElement("Receiver");
                    iCount++;
                    xmlEle.SetAttribute("ID", iCount.ToString());
                    xmlEle.SetAttribute("NodeID", values["strNodeID"].ToString());
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

                blnSuccess = DoFlowSubmit(userID, messageID, long.Parse(strActionID), lngLinkNodeID, lngLinkNodeType, strOpinionValue, strFormXMLValue, xmlRec.InnerXml, strAttXml, intSpecRightType);
                if (blnSuccess)
                {
                    jsonText = "Success";
                }
                else
                {
                    jsonText = "errorNET";
                }
            }
            catch
            {
                jsonText = "errorNET";
            }
            return jsonText;
        }

        /// <summary>
        /// 传阅、沟通、交接 选人方法 返回对应JSON字符串
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="messageID">消息ID</param>
        /// <param name="flowModelID">流程模型ID</param>
        /// <param name="strActionID">动作ID</param>
        /// <param name="strOpinionValue">办理意见</param>
        /// <param name="strFormDefineXml">表单自定人员XML串</param>
        /// <param name="intSpecRightType">调用类型: 60表示传阅,80表示沟通，25表示交接</param>
        /// <returns></returns>
        [WebMethod]
        public string SpecRight_Sender(long userID, long messageID, long flowModelID, string strActionID, string strOpinionValue, string strFormDefineXml, int intSpecRightType)
        {
            objFlow oFlow;
            long lngUserID = userID;
            long lngFlowID = 0;
            long lngMessageID = 0;
            long lngFlowModelID = 0;
            string strFormXMLValue = "";
            string strAttXml = "";
            string strOpinion = "";
            string jsonText = string.Empty;
            e_FMNodeType lngCurrNodeType = e_FMNodeType.fmNormal;   //当前环节的类别
            e_ReceiveActorType lngCurrReceiveType = e_ReceiveActorType.eratNone; //当前接收者类别

            //定义环节接收人员表
            DataTable dtNextReceivers = null;
            dtNextReceivers = setTableColumns();

            try
            {
                oFlow = new objFlow(lngUserID, flowModelID, messageID);
                lngFlowID = oFlow.FlowID;
                lngMessageID = messageID;
                lngFlowModelID = flowModelID;
                strAttXml = Message.GetAttachmentXml(lngFlowID);
                strOpinion = strOpinionValue;

                //不是所有的都需要选人的..
                strFormXMLValue = Master_GetFormValues(oFlow.AppID, lngFlowID, oFlow.OpID, lngUserID);
                Message objMsg = new Message();
                string strRet = objMsg.GetNextReceivers(lngUserID, lngMessageID, lngFlowModelID, int.Parse(strActionID), (e_SpecRightType)intSpecRightType, 0, strFormXMLValue, 1, strFormDefineXml);

                #region 解析人员的XML串

                long lngReceiverID = 0;
                string strNodeID = "";
                string strNodeName = "";
                string strReceiverName = "";

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(strRet);

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
                        lngReceiverID = long.Parse(wk.Attributes["ID"].Value);
                        lngCurrReceiveType = (e_ReceiveActorType)(int.Parse(wk.Attributes["ReceiveType"].Value));
                        strReceiverName = wk.Attributes["Name"].Value;

                        //过滤已经选过的阅知人员

                        if (intSpecRightType == 60)
                        {
                            if (Epower.ITSM.SqlDAL.FlowDP.IsExistesrtTransmit(lngFlowID, lngReceiverID, oFlow.NodeID))
                                continue;
                        }

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
                }

                #endregion
            }
            catch
            {
                jsonText = "errorNET";
            }
            jsonText = "{nameList:" + JsonConvert.SerializeObject(dtNextReceivers) + "}";

            return jsonText;
        }

        /// <summary>
        /// 定义处理人员表
        /// </summary>
        /// <returns></returns>
        private static DataTable setTableColumns()
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

        /// <summary>
        /// 事件单提交 - 不选人版本
        /// </summary>
        /// <param name="userID">用户编号</param>
        /// <param name="messageID">消息编号</param>
        /// <param name="flowModelID">流程模型编号</param>
        /// <param name="strActionID">?</param>
        /// <param name="strOpinionValue">处理意见</param>
        /// <returns></returns>
        [WebMethod]
        public string Flow_Sender(long userID, long messageID, long flowModelID, string strActionID, string strOpinionValue)
        {
            string jsonText = string.Empty;
            objFlow oFlow;
            long lngUserID = userID;
            long lngFlowID = 0;
            long lngMessageID = 0;
            long lngFlowModelID = 0;

            string strFormXMLValue = "";
            string strAttXml = "";
            string strOpinion = "";

            oFlow = new objFlow(lngUserID, flowModelID, messageID);
            lngFlowID = oFlow.FlowID;
            lngMessageID = messageID;
            lngFlowModelID = flowModelID;

            strFormXMLValue = Master_GetFormValues(oFlow.AppID, lngFlowID, oFlow.OpID, lngUserID);
            strAttXml = Message.GetAttachmentXml(lngFlowID);
            strOpinion = strOpinionValue;
            MessageEntity msgObject = new MessageEntity(lngMessageID);

            if (msgObject.ActorType != e_ActorClass.fmMasterActor)
            {
                jsonText = SetReadOver(lngUserID, lngMessageID, strOpinion, strFormXMLValue);
            }
            else
            {
                //不是所有的都需要选人的..
                Message objMsg = new Message();
                string strRet = objMsg.GetNextReceivers(lngUserID, lngMessageID, lngFlowModelID, int.Parse(strActionID), e_SpecRightType.esrtNormal, 0, strFormXMLValue, 1);

                //定义环节接收人员表
                DataTable dtNextReceivers = null;
                dtNextReceivers = setTableColumns();

                int iCount = GenerateMemberSelect(strRet, ref dtNextReceivers, lngUserID, lngMessageID, long.Parse(strActionID), strOpinion, strFormXMLValue, strAttXml);
                if (iCount == 1)
                {
                    jsonText = "Success";
                }
                else if (iCount == -1)
                {
                    jsonText = "errorNET";
                }
                else
                {
                    jsonText = "{nameList:" + JsonConvert.SerializeObject(dtNextReceivers) + "}";
                }
            }
            return jsonText;
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
        public int GenerateMemberSelect(string strRet, ref DataTable dtNextReceivers, long lngUserID, long lngMessageID, long lngActionID, string strOpinion, string strValues, string strAttXml)
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
            int isSuccess = 0;

            //判断流程是否直接提交
            isSuccess=IsTheDirectSubmit(lngUserID, lngMessageID, lngActionID, strOpinion, strValues, strAttXml, ref blnSuccess, lngCurrNodeType, lngCurrReceiveType, lngReceiverID, strNodeID, strReceiverName, iCount, lngLinkNodeID, lngLinkNodeType);
      
            return isSuccess;
        }

        /// <summary>
        /// 判断流程是否直接提交
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="lngMessageID"></param>
        /// <param name="lngActionID"></param>
        /// <param name="strOpinion"></param>
        /// <param name="strValues"></param>
        /// <param name="strAttXml"></param>
        /// <param name="blnSuccess"></param>
        /// <param name="lngCurrNodeType"></param>
        /// <param name="lngCurrReceiveType"></param>
        /// <param name="lngReceiverID"></param>
        /// <param name="strNodeID"></param>
        /// <param name="strReceiverName"></param>
        /// <param name="iCount"></param>
        /// <param name="lngLinkNodeID"></param>
        /// <param name="lngLinkNodeType"></param>
        /// <returns></returns>
        private int IsTheDirectSubmit(long lngUserID, long lngMessageID, long lngActionID, string strOpinion, string strValues, string strAttXml, ref bool blnSuccess, e_FMNodeType lngCurrNodeType, e_ReceiveActorType lngCurrReceiveType, long lngReceiverID, string strNodeID, string strReceiverName, int iCount, long lngLinkNodeID, long lngLinkNodeType)
        {
            int isSuccess = 0;
            if (iCount == 1)
            {
                //直接提交
                XmlDocument xmlRec = new XmlDocument();
                xmlRec.LoadXml("<Receivers></Receivers>");
                XmlElement xmlEle = xmlRec.CreateElement("Receiver");
                xmlEle.SetAttribute("ID", "1");
                xmlEle.SetAttribute("NodeID", strNodeID.ToString());
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
                blnSuccess = DoFlowSubmit(lngUserID, lngMessageID, lngActionID, lngLinkNodeID, lngLinkNodeType, strOpinion, strValues, xmlRec.InnerXml, strAttXml);
                if (blnSuccess)
                {
                    isSuccess = 1;
                }
                else
                {
                    isSuccess = -1;
                }
            }

            if (iCount == 0)
            {
                //直接提交
                XmlDocument xmlRec = new XmlDocument();
                xmlRec.LoadXml("<Receivers></Receivers>");
                blnSuccess = DoFlowSubmit(lngUserID, lngMessageID, lngActionID, lngLinkNodeID, lngLinkNodeType, strOpinion, strValues, xmlRec.InnerXml, strAttXml);
                if (blnSuccess)
                {
                    isSuccess = 1;
                }
                else
                {
                    isSuccess = -1;
                }
            }
            return isSuccess;
        }

        /// <summary>
        /// 流程发送处理
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="lngMessageID"></param>
        /// <param name="lngActionID"></param>
        /// <param name="lngLinkNodeID"></param>
        /// <param name="lngLinkNodeType"></param>
        /// <param name="strOpinion"></param>
        /// <param name="strValues"></param>
        /// <param name="strReceivers"></param>
        /// <param name="strAttXml"></param>
        /// <returns></returns>
        public bool DoFlowSubmit(long lngUserID, long lngMessageID, long lngActionID, long lngLinkNodeID, long lngLinkNodeType, string strOpinion, string strValues, string strReceivers, string strAttXml)
        {
            bool blnSuccess = false;
            try
            {
                blnSuccess = true;
                Message objMsg = new Message();
                objMsg.SendFlow(lngUserID, lngMessageID, lngActionID, e_SpecRightType.esrtNormal, lngLinkNodeID, lngLinkNodeType, 0, strOpinion, strValues, strReceivers, strAttXml);
               
            }
            catch (Exception err)
            {
                blnSuccess = false;
            }
            return blnSuccess;
        }

        /// <summary>
        /// 流程发送处理
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="lngMessageID"></param>
        /// <param name="lngActionID"></param>
        /// <param name="lngLinkNodeID"></param>
        /// <param name="lngLinkNodeType"></param>
        /// <param name="strOpinion"></param>
        /// <param name="strValues"></param>
        /// <param name="strReceivers"></param>
        /// <param name="strAttXml"></param>
        /// <param name="intSpecRightType"></param>
        /// <returns></returns>
        public bool DoFlowSubmit(long lngUserID, long lngMessageID, long lngActionID, long lngLinkNodeID, long lngLinkNodeType, string strOpinion, string strValues, string strReceivers, string strAttXml, int intSpecRightType)
        {
            bool blnSuccess = false;
            try
            {
                blnSuccess = true;
                Message objMsg = new Message();
                objMsg.SendFlow(lngUserID, lngMessageID, lngActionID, (e_SpecRightType)intSpecRightType, lngLinkNodeID, lngLinkNodeType, 0, strOpinion, strValues, strReceivers, strAttXml);
            }
            catch (Exception err)
            {
                blnSuccess = false;
            }
            return blnSuccess;
        }

        /// <summary>
        /// 获取事项XML值2
        /// </summary>
        /// <param name="lngAppID"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngOpID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        private string Master_GetFormValues(long lngAppID, long lngFlowID, long lngOpID, long userID)
        {
            ImplDataProcess dp = new ImplDataProcess(lngAppID);

            DataSet ds = dp.GetFieldsDataSet(lngFlowID, lngOpID);
            DataTable dt = ds.Tables[0];

            FieldValues fv = new FieldValues();

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    if (lngAppID == 1026)
                    {
                        #region 事件
                        fv.Add("Flag", "true");
                        fv.Add("Subject", row["Subject"].ToString());
                        fv.Add("Content", row["Content"].ToString());
                        fv.Add("RegUserID", row["RegUserID"].ToString());
                        fv.Add("RegUserName", row["RegUserName"].ToString());

                        fv.Add("ServiceLevelID", row["ServiceLevelID"].ToString());
                        fv.Add("ServiceLevel", row["ServiceLevel"].ToString());



                        fv.Add("ServiceTypeID", row["ServiceTypeID"].ToString());
                        fv.Add("ServiceType", row["ServiceType"].ToString());
                        fv.Add("ServiceKindID", row["ServiceKindID"].ToString());
                        fv.Add("ServiceKind", row["ServiceKind"].ToString());

                        fv.Add("EffectID", row["EffectID"].ToString());
                        fv.Add("EffectName", row["EffectName"].ToString());
                        fv.Add("InstancyID", row["InstancyID"].ToString());
                        fv.Add("InstancyName", row["InstancyName"].ToString());
                        fv.Add("DealStatusID", row["DealStatusID"].ToString());   //事件状态ID
                        fv.Add("DealStatus", row["DealStatus"].ToString());           //事件状态

                        fv.Add("CustTime", row["CustTime"].ToString()); //发生时间
                        fv.Add("ReportingTime", row["ReportingTime"].ToString()); //报告时间

                        fv.Add("CustID", row["CustID"].ToString());
                        fv.Add("CustName", row["CustName"].ToString());
                        fv.Add("CustAddress", row["CustAddress"].ToString());

                        fv.Add("Contact", row["Contact"].ToString());
                        fv.Add("CTel", row["CTel"].ToString());
                        fv.Add("CustDeptName", row["CustDeptName"].ToString());

                        fv.Add("Job", row["Job"].ToString());
                        fv.Add("Email", row["Email"].ToString());
                        fv.Add("MastCust", row["MastCust"].ToString());
                        fv.Add("CustMobile", row["CustMobile"].ToString());
                        fv.Add("CustAreaID", row["CustAreaID"].ToString());
                        fv.Add("CustArea", row["CustArea"].ToString());

                        #region 资产信息

                        fv.Add("EquipmentID", row["EquipmentID"].ToString());
                        fv.Add("EquipmentName", row["EquipmentName"].ToString());
                        fv.Add("EquipmentCatalogName", row["EquipmentCatalogName"].ToString()); //资产目录
                        fv.Add("EquipmentCatalogID", row["EquipmentCatalogID"].ToString());

                        fv.Add("EquPositions", row["EquPositions"].ToString());
                        fv.Add("EquCode", row["EquCode"].ToString());
                        fv.Add("EquSN", row["EquSN"].ToString());
                        fv.Add("EquModel", row["EquModel"].ToString());
                        fv.Add("EquBreed", row["EquBreed"].ToString());

                        #endregion

                        fv.Add("Outtime", row["Outtime"].ToString());
                        fv.Add("ServiceTime", row["ServiceTime"].ToString());
                        fv.Add("FinishedTime", row["FinishedTime"].ToString());
                        fv.Add("SjwxrID", row["SjwxrID"].ToString());
                        fv.Add("Sjwxr", row["Sjwxr"].ToString());

                        fv.Add("TotalAmount", row["TotalAmount"].ToString());
                        fv.Add("OrgID", row["OrgID"].ToString());

                        fv.Add("RegSysDate", row["RegSysDate"].ToString());
                        fv.Add("RegSysUserID", row["RegSysUserID"].ToString());
                        fv.Add("RegSysUser", row["RegSysUser"].ToString());


                        fv.Add("RegDeptID", row["RegDeptID"].ToString());
                        fv.Add("RegDeptName", row["RegDeptName"].ToString());


                        fv.Add("ApplicationTime", row["ApplicationTime"].ToString());//申请时间
                        fv.Add("Reason", row["Reason"].ToString());//申请原因
                        fv.Add("ExpectedTime", row["ExpectedTime"].ToString());//期间时间


                        //取得服务单前缀，生成服务单号



                        fv.Add("serviceno", row["serviceno"].ToString());
                        fv.Add("buildCode", row["buildCode"].ToString());





                        #region yxq 新增关闭理由和事件来源 2011-08-02
                        fv.Add("CloseReasonID", row["CloseReasonID"].ToString());  //关闭理由ID
                        fv.Add("CloseReasonName", row["CloseReasonName"].ToString());        //关闭理由名称
                        fv.Add("ReSouseID", row["ReSouseID"].ToString());          //事件来源ID
                        fv.Add("ReSouseName", row["ReSouseName"].ToString());                //事件来源名称
                        #endregion



                        //事件模板相关
                        fv.Add("IssTempID", row["IssTempID"].ToString());
                        fv.Add("IsUseIssTempID", row["IsUseIssTempID"].ToString());
                        //事件根源
                        fv.Add("IssueRootId", row["IssueRootId"].ToString());
                        //fv.Add("IssueRootName", row["IssueRootName"].ToString());

                        //fv.Add("RealityTime", row["RealityTime"].ToString());
                        //fv.Add("InfluenceUserTime", row["InfluenceUserTime"].ToString());
                        //fv.Add("EndTime", row["EndTime"].ToString());
                        //fv.Add("IsChang", row["IsChang"].ToString());
                        //fv.Add("IsHandle", row["IsHandle"].ToString());
                        //fv.Add("IsProject", row["IsProject"].ToString());
                        //fv.Add("MeasuresContent", row["MeasuresContent"].ToString());

                        //fv.Add("HandleName", row["HandleName"].ToString());//处理预案名称
                        //fv.Add("HandleId", row["HandleId"].ToString());//处理预案Id

                        //原因分类
                        //fv.Add("InitiationId", row["InitiationId"].ToString());
                        //fv.Add("InitiationName", row["InitiationName"].ToString());
                        //fv.Add("BridleId", row["BridleId"].ToString());
                        //fv.Add("BridleName", row["BridleName"].ToString());
                        //fv.Add("HitchId", row["HitchId"].ToString());
                        //fv.Add("HitchName", row["HitchName"].ToString());
                        //流程ID-重大事件页面使用
                        fv.Add("FromFlowID", "0");
                        //机构影响系数
                        //fv.Add("MachineryId", row["MachineryId"].ToString());
                        //fv.Add("MachineryNum", row["MachineryNum"].ToString());
                        //fv.Add("CatelogId", row["CatelogId"].ToString());
                        //fv.Add("TemplateId", row["TemplateId"].ToString());


                        fv.Add("DealContent", row["DealContent"].ToString());

                        fv.Add("ServiceLevelChange", "false");

                        //fv.Add("FaultEffect", row["FaultEffect"].ToString());//故障影响业务时间
                        //fv.Add("InfluenceDegree", row["InfluenceDegree"].ToString());//影响程度及范围
                        //fv.Add("Inprogress", row["Inprogress"].ToString());//进展
                        //fv.Add("recovery", row["recovery"].ToString());//进展
                        //fv.Add("restorationTime", row["restorationTime"].ToString());//进展

                        /* you know: 下面这三个是关于合并事件单 和 扩展项的*/
                        fv.Add("ItemXml", "");
                        fv.Add("ItemCount", "");                        
                        fv.Add("ExtensionDayList", "");
                        #endregion
                    }
                    else if (lngAppID == 420)
                    {
                        #region 变更
                        fv.Add("Flag", "true");
                        fv.Add("EffectID", row["EffectID"].ToString());
                        fv.Add("EffectName", row["EffectName"].ToString());
                        fv.Add("InstancyID", row["InstancyID"].ToString());
                        fv.Add("InstancyName", row["InstancyName"].ToString());
                        fv.Add("LevelID", row["LevelID"].ToString());
                        fv.Add("LevelName", row["LevelName"].ToString());
                        fv.Add("ChangeTypeID", row["ChangeTypeID"].ToString());
                        fv.Add("ChangeTypeName", row["ChangeTypeName"].ToString());
                        fv.Add("ChangeNo", row["ChangeNo"].ToString());
                        fv.Add("Subject", row["Subject"].ToString());
                        fv.Add("Content", row["Content"].ToString());
                        //fv.Add("EffectContent", row["EffectContent"].ToString());
                        //fv.Add("Edition", row["Edition"].ToString());
                        fv.Add("Reason", String.Empty);
                        fv.Add("CustID", row["CustID"].ToString());
                        fv.Add("CustName", row["CustName"].ToString());
                        fv.Add("CustAddress", row["CustAddress"].ToString());
                        fv.Add("Contact", row["Contact"].ToString());
                        fv.Add("CTel", row["CTel"].ToString());
                        fv.Add("EquipmentID", "0");
                        fv.Add("EquipmentName", "");
                        fv.Add("ChangeAnalyses", row["ChangeAnalyses"].ToString());
                        fv.Add("ChangeAnalysesResult", row["ChangeAnalysesResult"].ToString());
                        fv.Add("Remark", string.Empty);
                        fv.Add("DealStatusID", row["DealStatusID"].ToString());
                        fv.Add("DealStatus", row["DealStatus"].ToString());
                        fv.Add("RegUserID", row["RegUserID"].ToString());
                        fv.Add("RegUserName", row["RegUserName"].ToString());
                        fv.Add("RegDeptID", row["RegDeptID"].ToString());
                        fv.Add("RegDeptName", row["RegDeptName"].ToString());
                        fv.Add("RegOrgID", row["RegOrgID"].ToString());
                        fv.Add("FromFlowID", row["IssuesFlowID"].ToString());                         
                        fv.Add("ExtensionDayList", "");
                        fv.Add("SMSNotify", "");


                        fv.Add("FromProblemFlowID", row["ProblemFlowID"].ToString());            //问题单传过来的问题单FlowID

                        //fv.Add("PlanBeginDt", row["PlanBeginDt"].ToString());//计划开始时间

                        //fv.Add("PlanEndDt", row["PlanEndDt"].ToString());//计划完成时间
                        fv.Add("ChangeTime", row["ChangeTime"].ToString());//实际开始时间

                        //fv.Add("ChangeEndDt", row["ChangeEndDt"].ToString());//实际完成时间
                        //fv.Add("ChangeQBeingDt", row["ChangeQBeingDt"].ToString());//希望实施时间
                        fv.Add("RegTime", row["RegTime"].ToString());//变更创建时间
                        fv.Add("Changesource", row["Changesource"].ToString());//变更来源
                        fv.Add("Businesapprover", row["Businesapprover"].ToString());//业务审批人

                        #region 保存资产数据
                        string deskChangeUrl = CommonDP.GetConfigValue("TempCataLog", "TempCataLog") + "DeskChange" + userID.ToString() + CTools.GetRandom() + ".xml";
                        long changeID = long.Parse(row["id"].ToString());
                        SaveDetailChange(changeID, deskChangeUrl);
                        fv.Add("DeskChange", deskChangeUrl);
                        #endregion

                        #endregion
                    }
                    else
                    {
                        fv.Add("Flag", "true");
                    }
                }
            }
            return fv.GetXmlObject().InnerXml;

        }

        /// <summary>
        /// 获取事项XML值
        /// </summary>
        /// <param name="lngAppID"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngOpID"></param>
        /// <returns></returns>
        private string Master_GetFormValues(long lngAppID, long lngFlowID, long lngOpID)
        {
            ImplDataProcess dp = new ImplDataProcess(lngAppID);

            DataSet ds = dp.GetFieldsDataSet(lngFlowID, lngOpID);
            DataTable dt = ds.Tables[0];

            FieldValues fv = new FieldValues();

            DataRow row = dt.Rows[0];
            fv.Add("Flag", "true");
            return fv.GetXmlObject().InnerXml;

        }

        /// <summary>
        ///  保存资产数据
        /// </summary>
        /// <param name="changeServiceID"></param>
        /// <param name="deskChangeUrl"></param>
        private void SaveDetailChange(long changeServiceID, string deskChangeUrl)
        {
            DataTable dtChange = ChangeDealDP.GetCLFareItem(changeServiceID);

            ChangeDealDP.SaveCLFareDetailItem(dtChange, deskChangeUrl);
        }


        #endregion

        #region 事件详情

        [WebMethod]
        public DataTable Get_Issue_Detailed2(long userID, long messageID, long flowModelID)
        {
            objFlow oFlow;
            DataTable dt = null;
            oFlow = new objFlow(userID, flowModelID, messageID);
            ImplDataProcess dp = new ImplDataProcess(oFlow.AppID);
            DataSet ds = dp.GetFieldsDataSet(oFlow.FlowID, oFlow.OpID);
            dt = ds.Tables[0];
            return dt;
        }

        /// <summary>
        /// 事件详情
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="messageID"></param>
        /// <param name="flowModelID"></param>
        /// <param name="ReadOnly"></param>
        /// <param name="FlowProcessType"></param>
        /// <param name="ActorType"></param>
        /// <returns></returns>
        [WebMethod]
        public string Get_Issue_Detailed(long userID, long messageID, long flowModelID, long FlowProcessType, long ActorType)
        {
            objFlow oFlow;
            string jsonText = string.Empty;
            string str = string.Empty;
            long lngUserID = userID;
            long lngMessageID = messageID;
            long lngFlowModelID = flowModelID;
            long lngActorType = ActorType;
            eOA_FlowProcessType iFPType = eOA_FlowProcessType.efptReadFinished;  //缺省值
            bool m_bIsReader = false;
            long lngFlowID = 0;
            long lngAppID = 0;
            long lngOpID = 0;
            DataTable dtMatterOf = null;
            DataTable dtProcess = null;
            try
            {
                iFPType = (eOA_FlowProcessType)FlowProcessType;
                oFlow = new objFlow(lngUserID, lngFlowModelID, lngMessageID);
                //当前消息为阅知，并且未完，可以编辑意见的
                if (oFlow.ActorClass == e_ActorClass.fmReaderActor || oFlow.ActorClass == e_ActorClass.fmAssistActor || oFlow.ActorClass == e_ActorClass.fmCommunicActor)
                {
                    m_bIsReader = true;  //表示阅知情况
                }
                lngFlowID = oFlow.FlowID;
                lngAppID = oFlow.AppID;
                lngOpID = oFlow.OpID;
                StringBuilder sb = new StringBuilder("");
                if (lngMessageID != 0)
                {
                    ImplDataProcess dp = new ImplDataProcess(oFlow.AppID);
                    DataSet ds = dp.GetFieldsDataSet(oFlow.FlowID, oFlow.OpID);
                    dtMatterOf = ds.Tables[0];
                }
                else
                {
                    sb.Append("标题:" + oFlow.FlowName);
                }

                if (dtMatterOf != null)
                {
                    if (dtMatterOf.Rows.Count > 0)
                    {
                        JsonSerializer js = JsonToos.GetJsonSerializer();
                        //转换为为Json Array
                        jsonText = JArray.FromObject(dtMatterOf, js).ToString();

                        string jsonProcess = GetProcess(lngUserID, lngFlowID, lngFlowModelID, iFPType);
                        string jsonActions = string.Empty;
                        jsonActions = LoadActions(lngFlowModelID, oFlow.NodeModelID, m_bIsReader);

                        //变更加上关联资产
                        if (lngAppID == 420)
                        {
                            DataRow row = dtMatterOf.Rows[0];
                            long changeID = long.Parse(row["id"].ToString());
                            string strChange = JArray.FromObject(BindChange(changeID), js).ToString();

                            str = "{nameList:" + jsonText + ",process:" + jsonProcess + ",actions:" + jsonActions + ",change:" + strChange + "}";
                        }
                        else
                        {
                            str = "{nameList:" + jsonText + ",process:" + jsonProcess + ",actions:" + jsonActions + "}";
                        }
                    }
                }
            }
            catch
            {
                str = "errorNET";
            }
            return str;

        }



        /// <summary>
        /// 变更关联资产
        /// </summary>
        /// <param name="changeID"></param>
        /// <returns></returns>
        private DataTable BindChange(long changeID)
        {
            DataTable dtItem = ChangeDealDP.GetCLFareItem(changeID);
            return dtItem;
        }

        /// <summary>
        /// 页面显示流程提交按钮
        /// </summary>
        /// <param name="lngFlowModelID"></param>
        /// <param name="lngNodeModelID"></param>
        /// <param name="bIsReader"></param>
        /// <returns></returns>
        public string LoadActions(long lngFlowModelID, long lngNodeModelID, bool bIsReader)
        {
            string jsonText = string.Empty;
            try
            {
                DataSet ds1 = FlowModel.GetNodeActions(lngFlowModelID, lngNodeModelID);
                string strActionName = "";
                string strActionID = "0";

                DataTable dTable = new DataTable();
                //定义表结构
                dTable.Columns.Add("text", typeof(System.String));
                dTable.Columns.Add("id", typeof(System.String));

                if (ds1.Tables[0].Rows.Count == 0 || bIsReader == true)
                {
                    jsonText = "[{'text':'确定','id':'0'}]";
                }
                else
                {
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        DataRow drNew = dTable.NewRow();
                        strActionName = ds1.Tables[0].Rows[i]["ActionName"].ToString();
                        strActionID = ds1.Tables[0].Rows[i]["ActionID"].ToString();
                        drNew[0] = strActionName;
                        drNew[1] = strActionID;
                        dTable.Rows.Add(drNew);
                    }
                    jsonText = JsonConvert.SerializeObject(dTable);
                }
            }
            catch
            {
                return "[{'text':'errorNET','id':'errorNET'}]";
            }
            return jsonText;
        }

        /// <summary>
        /// 处理过程
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="FlowID"></param>
        /// <param name="FlowModelID"></param>
        /// <param name="iFPType"></param>
        /// <returns></returns>
        private string GetProcess(long lngUserID, long FlowID, long FlowModelID, eOA_FlowProcessType iFPType)
        {
            string jsonText = string.Empty;
            long lngMessageID = 0;
            long lngReceiverID = 0;
            long lngSenderID = 0;
            int iIsRead = 0;
            long lngCurrUserID = lngUserID;
            bool blnCanAttemper = MessageDep.CanDoHelpDeskAttemper(lngUserID, FlowID);
            bool blnOnlyRedo = false;
            bool blnHasPause = MessageProcess.HasFlowPauseLog(FlowID);
            string sProcess = "";
            string sMsgIDs = "";
            string sReader = "";
            string sTransmit = "";
            e_FlowStatus fs = Flow.GetFlowStatus(FlowID);

            DataTable dtProcessStatus = MessageProcess.GetProcessStatus(FlowModelID, FlowID);
            DataTable dProcess = new DataTable();

            //定义表结构
            dProcess.Columns.Add("dotime", typeof(System.String));
            dProcess.Columns.Add("tactors", typeof(System.String));
            dProcess.Columns.Add("sActorType", typeof(System.String));
            dProcess.Columns.Add("scol", typeof(System.String));
            DataTable dReader = new DataTable();
            //定义表结构
            dReader.Columns.Add("dotime", typeof(System.String));
            dReader.Columns.Add("tactors", typeof(System.String));
            dReader.Columns.Add("sActorType", typeof(System.String));
            dReader.Columns.Add("scol", typeof(System.String));
            DataTable dTransmit = new DataTable();
            //定义表结构
            dTransmit.Columns.Add("dotime", typeof(System.String));
            dTransmit.Columns.Add("tactors", typeof(System.String));
            dTransmit.Columns.Add("sActorType", typeof(System.String));
            dTransmit.Columns.Add("scol", typeof(System.String));

            DataView dv = new DataView(dtProcessStatus);
            DataView dvTO = new DataView(dtProcessStatus);
            foreach (DataRow dr in dtProcessStatus.Rows)
            {
                DataRow drProcess = dProcess.NewRow();
                DataRow drReader = dReader.NewRow();
                DataRow drTransmit = dTransmit.NewRow();
                lngMessageID = long.Parse(dr["MessageID"].ToString());
                lngReceiverID = long.Parse(dr["receiverid"].ToString());
                lngSenderID = long.Parse(dr["SenderID"].ToString());
                iIsRead = int.Parse(dr["IsRead"].ToString());

                if ((blnCanAttemper == true && iFPType != eOA_FlowProcessType.eftpReceiveMsg) || lngReceiverID > 0)
                {
                    //没有调度权限 并且 未接收的消息不显示出来
                    if (sMsgIDs.IndexOf(lngMessageID.ToString() + ",") < 0)//判断ID不重复
                    {
                        string sDate = dr["dotime"].ToString();
                        string sOpinion = dr["opinion"].ToString();
                        string sAddOpinion = dr["mpcontent"].ToString();   // 2007-08-30 补充业务处理过程描述[来自业务接口]
                        string sToMsgID = dr["tmessageid"].ToString();
                        string sTo = "";
                        string sTR = "";
                        e_MessageStatus ms = (e_MessageStatus)StringTool.String2Long(dr["status"].ToString());

                        //-------------------接收消息-------------------//
                        dv.RowFilter = "MessageID=" + lngMessageID.ToString();
                        string sReceiveType = "";
                        //TO消息
                        for (int n = 0; n < dv.Count; n++)
                        {
                            #region 内循环代码
                            //--接收类型
                            dvTO.RowFilter = "MessageID=" + dv[n]["TMessageID"].ToString() + " AND receiverid <> 0";
                            if (dvTO.Count > 0)
                            {
                                e_MsgReceiveType mrt = (e_MsgReceiveType)StringTool.String2Long(dvTO[0]["receivetype"].ToString());
                                e_MsgSelfType mst = (e_MsgSelfType)StringTool.String2Long(dvTO[0]["messagetype"].ToString());
                                blnOnlyRedo = false;
                                //仅重审的驳回 不能调度
                                if (mrt == e_MsgReceiveType.emrtBackHasDoneRedo)
                                {
                                    blnOnlyRedo = true;
                                }
                                switch (mrt)
                                {
                                    case e_MsgReceiveType.emrtaNormal:
                                        sReceiveType = "提交";
                                        break;
                                    case e_MsgReceiveType.emrtInfuxSumTo:
                                        sReceiveType = "汇总给";
                                        break;

                                    case e_MsgReceiveType.emrtBack:
                                        if (dvTO[0]["receiverid"].ToString() == dvTO[0]["senderid"].ToString())
                                        {
                                            sReceiveType = "返回";
                                        }
                                        else
                                        {
                                            sReceiveType = "退回";
                                        }
                                        break;
                                    case e_MsgReceiveType.emrtSkip:
                                        sReceiveType = "跳转至";
                                        break;
                                    case e_MsgReceiveType.emrtBackHasDone:
                                    case e_MsgReceiveType.emrtBackHasDoneRedo:
                                    case e_MsgReceiveType.emrtBackHasDoneFlow:
                                        sReceiveType = "驳回到";
                                        break;
                                    case e_MsgReceiveType.emrtTakeOver:
                                        sReceiveType = "交办给";
                                        break;
                                    case e_MsgReceiveType.emrtAttemper:
                                        sReceiveType = "给";
                                        break;
                                    case e_MsgReceiveType.emrtReDoBack:
                                        sReceiveType = "提交重审给";
                                        break;
                                }
                                if (mst != e_MsgSelfType.emstCommunic && mst != e_MsgSelfType.emstAssist && mst != e_MsgSelfType.emstTransmit)
                                {
                                    // 协作 转发  沟通时不在处理过程中展示
                                    sTo += dvTO[0]["tactors"].ToString() + ",";//+
                                }
                            }
                            else
                            {
                                //没有查询到ＴＭＥＳＳＡＧＥＩＤ详细资料的情况,
                                //理论上的可能性只有当前ReceiverID为0 或者 没有处理，分流等待环节　是没有后续消息 的时候
                                if (ms == e_MessageStatus.emsFinished && fs != e_FlowStatus.efsEnd
                                    && dr["rectactors"].ToString().Trim() != "")
                                {
                                    sTo = "  等待 " +  dr["rectactors"].ToString() + " 处理";
                                }
                            }
                            #endregion
                        }

                        if (sTo != "" && dvTO.Count > 0)
                        {
                            sTo = sReceiveType + SP() + sTo.TrimEnd(',') + SP() + "进行" + SP() + dvTO[0]["nodename"].ToString();
                            if ((e_MsgReceiveType)StringTool.String2Long(dr["receivetype"].ToString()) == e_MsgReceiveType.emrtInfuxSumTo)
                            {
                                sTo = sTo + "[会签汇总]";
                            }
                        }

                        //处理意见
                        sOpinion = sOpinion.Trim() == "" ? "" : " 处理意见:" + sOpinion ;
                        sAddOpinion = sAddOpinion.Trim() == "" ? "" : " " + sAddOpinion;

                        //----------处理意见----------------//
                        string sStatusName = "";
                        switch (ms)
                        {
                            case e_MessageStatus.emsFinished:
                                sStatusName = "";
                                break;
                            case e_MessageStatus.emsHandle:
                                sStatusName = "处理中";
                                break;
                            case e_MessageStatus.emsStop:
                                sStatusName = "暂停";
                                break;
                            case e_MessageStatus.emsWaiting:
                                sStatusName = "挂起";
                                break;
                        }

                        //-----------------角色类型--------------------//
                        string sActorType = "";
                        e_ActorClass at = (e_ActorClass)StringTool.String2Long(dr["actortype"].ToString());
                        e_FMNodeType fmnodetype = (e_FMNodeType)StringTool.String2Long(dr["nmtype"].ToString());

                        switch (at)
                        {
                            case e_ActorClass.fmAssistActor:
                                sActorType = "协办";
                                break;
                            case e_ActorClass.fmMasterActor:
                                sActorType = "主办";
                                //if (lngUserID != lngReceiverID && lngReceiverID != 0 && ms == e_MessageStatus.emsHandle && blnCanAttemper == true)
                                if ((lngUserID != lngReceiverID || lngReceiverID == 0) && fmnodetype != e_FMNodeType.fmDraft && ms == e_MessageStatus.emsHandle && blnOnlyRedo == false && CheckRight(lngUserID,Constant.DoAttemper))
                                {
                                    //主办的情况  并且待办 起草环节不调度
                                    sActorType += "调度";
                                }
                                break;
                            case e_ActorClass.fmReaderActor:
                                sActorType = "传阅";
                                break;
                            case e_ActorClass.fmCommunicActor:
                                sActorType = "沟通";
                                break;
                            case e_ActorClass.fmInfluxActor:
                                sActorType = "会签";
                                break;
                        }
                        //接收时间     接收者    (环节名称      状态  意见,意见和后续消息)
                        if (at == e_ActorClass.fmReaderActor)
                        {
                            string ST = "";
                            if (ms == e_MessageStatus.emsFinished && dr["isread"].ToString() == "1")
                            {
                                ST = "<已沟通>";
                            }
                            else if (dr["isread"].ToString() == "1")
                            {
                                ST = "<已读>";
                            }
                            else
                            {
                                ST = "<未沟通>";
                            }

                            if (dr["messagetype"].ToString() == ((int)e_MsgSelfType.emstTransmit).ToString())
                            {
                                sTransmit = "";
                                sTransmit += dr["tactors"].ToString() +
                                "  (" + dr["factors"].ToString() + "" + dr["receivetime"].ToString() + "转" + "：" + ST + ")";

                                if (sOpinion.Length != 0)
                                {
                                    sTransmit += "［" + sOpinion + "］";
                                }
                                sTransmit += SP();
                                drTransmit[3] = sTransmit;
                                dTransmit.Rows.Add(drTransmit);
                            }
                            else
                            {
                                sReader = "";
                                sReader += dr["tactors"].ToString() + SP() +
                                "(" + dr["nodename"].ToString() + "：" + ST + ")";
                                if (sOpinion.Length != 0)
                                {
                                    sReader += "［" + sOpinion + "］";
                                }
                                drReader[3] = sReader;
                                dReader.Rows.Add(drReader);
                            }

                        }
                        else
                        {
                            string scol = dr["nodename"].ToString();
                            if ((e_MsgReceiveType)StringTool.String2Long(dr["receivetype"].ToString()) == e_MsgReceiveType.emrtInfuxSumTo)
                            {
                                scol = scol + "[会签汇总]";
                            }
                            if ((e_MsgReceiveType)StringTool.String2Long(dr["receivetype"].ToString()) == e_MsgReceiveType.emrtReDoBack)
                            {
                                scol = scol + "[重审]";
                            }
                            scol += " " + sStatusName;
                            if (sOpinion + sTo != "")//意见和后续消息
                                scol += "," + sAddOpinion + sOpinion + " "+ sTo;
                            drProcess[0] = sDate;
                            drProcess[1] = sActorType;
                            drProcess[2] = dr["tactors"].ToString();
                            drProcess[3] = scol;
                            dProcess.Rows.Add(drProcess);
                        }
                        sMsgIDs += dr["MessageID"].ToString() + ",";
                    }
                }
            }

            if (dReader.Rows.Count>0)
            {
                dProcess.Merge(dReader);
            }

            if (dTransmit.Rows.Count>0)
            {
                dProcess.Merge(dTransmit);
            }

            if (fs != e_FlowStatus.efsHandle)
            {
                string sTrFlowEnd = "";
                DataRow drFlowEnd = dProcess.NewRow();
                string sFstatus = (fs == e_FlowStatus.efsEnd) ? "结束" : (fs == e_FlowStatus.efsStop ? "暂停" : "终止");
                sTrFlowEnd = "--流程已经" + sFstatus + "--";
                drFlowEnd[3] = sTrFlowEnd;
                dProcess.Rows.Add(drFlowEnd);
            }
            if (dProcess.Rows.Count > 0)
            {
                jsonText = JsonConvert.SerializeObject(dProcess);
            }
            return jsonText;
        }

        /// <summary>
        /// 检查权限
        /// </summary>
        /// <param name="OperatorID"></param>
        /// <returns></returns>
        private bool CheckRight(long UserID, long OperatorID)
        {
            RightEntity re = (RightEntity)(RightDP.getUserRightTable(UserID))[OperatorID];
            if (re == null)
            {
                return false;
            }
            else
            {
                return re.CanRead;
            }
        }

        /// <summary>
        ///  加空格
        /// </summary>
        /// <returns></returns>
        private static string SP()
        {
            return "  ";
        }

        #endregion

        #region 事件接收 事件回收  事件退回 事件退回
        /// <summary>
        /// 事件接收
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="messageID"></param>
        /// <returns>json</returns>
        [WebMethod]
        public string ReceiveClick(long lngMessageID, long lngUserID, long lngAppID)
        {
            string strText = string.Empty;
            bool blnSuccess = false;
            try
            {
                blnSuccess = ReceiveList.ReceiveMessage(lngMessageID, lngUserID, lngAppID);
            }
            catch (Exception eSend)
            {
                blnSuccess = false;
                strText = "errorNET";
            }
            if (blnSuccess == true)
            {
                strText = "Success";
            }
            return strText;
        }
       
        /// <summary>
        /// 事件回收
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="messageID"></param>
        /// <returns>json</returns>
        [WebMethod]
        public string TakeBackClick(long lngUserID, long lngMessageID)
        {
            bool blnSuccess = false;
            string strText = string.Empty;
            try
            {
                blnSuccess = true;
                Message objMsg = new Message();
                objMsg.TakeBackFlow(lngUserID, lngMessageID);
            }
            catch (Exception eTakeBack)
            {
                blnSuccess = false;
                strText = "errorNET";
            }
            if (blnSuccess == true)
            {
                strText = "Success";
            }
            return strText;
        }

    

        /// <summary>
        /// 事件退回

        /// </summary>
        /// <param name="userID"></param>
        /// <param name="messageID"></param>
        /// <returns>json</returns>
        [WebMethod]
        public string SendBackClick(long lngUserID, long lngMessageID, long lngFlowID, string strOpinion)
        {
            bool blnSuccess = false;
            string strText = string.Empty;
            string strAttXml = "";
            strAttXml = Message.GetAttachmentXml(lngFlowID);
            try
            {
                blnSuccess = true;
                Message objMsg = new Message();
                objMsg.SendBackFlow(lngUserID, lngMessageID, 0, strOpinion, strAttXml);
            }
            catch (Exception eTakeBack)
            {
                blnSuccess = false;
                strText = "errorNET";
            }
            if (blnSuccess == true)
            {
                strText = "Success";
            }
            return strText;
        }


        /// <summary>
        /// 阅读
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="lngMessageID"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="appID"></param>
        /// <param name="lngOpID"></param>
        /// <param name="strOpinion"></param>
        /// <returns></returns>
        public string SetReadOver(long lngUserID, long lngMessageID, string strOpinion, string strXmlValues)
        {
            bool blnSuccess = false;
            string strText = string.Empty;
            try
            {
                blnSuccess = true;
                Message.SetReadOver(lngUserID, lngMessageID, strOpinion, strXmlValues);
            }
            catch (Exception eTakeBack)
            {
                blnSuccess = false;
                strText = "errorNET";
            }
            if (blnSuccess == true)
            {
                strText = "Success";
            }
            return strText;
        }
        #endregion
    }
}
