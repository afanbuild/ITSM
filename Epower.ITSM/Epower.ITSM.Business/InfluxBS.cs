/*******************************************************************
 * 版权所有：深圳市非凡信息技术有限公司
 * 描述：会签环节相关的业务逻辑代码
 * 
 * 
 * 创建人：孙绍棕
 * 创建日期：2013-11-28 
 * *****************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using EpowerCom;
using EpowerGlobal;
using Epower.ITSM.SqlDAL;
using System.Data;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Business
{
    /// <summary>
    /// 会签环节的业务逻辑
    /// </summary>
    public class InfluxBS
    {
        #region 会签环节时，仅在最后一个会签人员处理时, 才默认选中邮件发送的复选框 - 2013-11-27 @孙绍棕

        /// <summary>
        /// 会签环节时，仅在最后一个会签人员处理时, 才默认选中邮件发送的复选框
        /// </summary>
        public static void SendMailForLastInfluxActor(long lngUserID, long lngMessageID, String strXmlValues)
        {
            objFlow oFlow2 = new objFlow(lngUserID, 0, lngMessageID);

            if (oFlow2.NodeType == e_FMNodeType.fmInflux
                && oFlow2.ReceiveType != e_MsgReceiveType.emrtInfuxSumTo)
            {
                FieldValues fvList = new FieldValues(strXmlValues);
                FieldValue fv = fvList.GetFieldValue("EmailNotify");

                if (fv != null)
                {
                    bool isEmailNotify = bool.Parse(fv.Value);

                    if (!isEmailNotify)
                    {
                        return;  
                    }
                }

                bool isInfuxSumTo = CommonDP.CheckInfuxSumTo(oFlow2.FlowID, oFlow2.NodeID);


                if (isInfuxSumTo)
                {
                    DataTable dtInfluxNodeMaster = UserDP.GetInfluxNodeMaster(oFlow2.FlowID);
                    String strReceiverMail = dtInfluxNodeMaster.Rows[0]["email"].ToString();

                    if (!String.IsNullOrEmpty(strReceiverMail))
                    {

                        String strAlerrTitle = String.Format("{0} - 会签环节已结束", oFlow2.FlowName);
                        String strAlertMsg = String.Format("流程[{0} - {2}]的会签环节[{1}]已结束。", oFlow2.FlowName, oFlow2.NodeName, oFlow2.Subject);
                        String strLog = String.Format("邮件发送成功: {0} - {2} - {1}会签环节已结束.", oFlow2.FlowName, oFlow2.NodeName, oFlow2.Subject);


                        switch (oFlow2.AppID)
                        {
                            case 1026:    // 事件
                                String strIssueNO = ZHServiceDP.GetIssueNO(oFlow2.FlowID);
                                strAlerrTitle = String.Format("事件单 {0} - 会签环节已结束", oFlow2.Subject);
                                strAlertMsg = String.Format("事件单[{0} - {2}]的会签环节[{1}]已结束。", strIssueNO, oFlow2.NodeName, oFlow2.Subject);
                                strLog = String.Format("邮件发送成功: {0}", strAlertMsg);

                                break;
                            case 210:    // 问题

                                String strProblemNO = ProblemDealDP.GetProblemNO(oFlow2.FlowID);
                                strAlerrTitle = String.Format("问题单 {0} - 会签环节已结束", oFlow2.Subject);
                                strAlertMsg = String.Format("问题单[{0} - {2}]的会签环节[{1}]已结束。", strProblemNO, oFlow2.NodeName, oFlow2.Subject);
                                strLog = String.Format("邮件发送成功: {0}", strAlertMsg);

                                break;
                            case 420:    // 变更

                                String strEQUChangeNO = Equ_ServerDP.GetEQUChangeNO(oFlow2.FlowID);
                                strAlerrTitle = String.Format("变更单 {0} - 会签环节已结束", oFlow2.Subject);
                                strAlertMsg = String.Format("变更单[{0} - {2}]的会签环节[{1}]已结束。", strEQUChangeNO, oFlow2.NodeName, oFlow2.Subject);
                                strLog = String.Format("邮件发送成功: {0}", strAlertMsg);

                                break;
                            default:

                                strAlerrTitle = String.Format(" {1} - {0} - 会签环节已结束", oFlow2.Subject, oFlow2.FlowName);
                                strAlertMsg = String.Format("[{0} - {2}]的会签环节[{1}]已结束。", oFlow2.FlowName, oFlow2.NodeName, oFlow2.Subject);
                                strLog = String.Format("邮件发送成功: {0}", strAlertMsg);

                                break;
                        }


                        //t2.userid, t2.name, t2.mobile, t2.email
                        MailSendDeal.MailSend(strReceiverMail, strAlerrTitle, strAlertMsg);
                        E8Logger.Info(strLog);
                    }
                }
            }

        }

        #endregion


        /// <summary>
        /// 是否会签环节
        /// </summary>
        public static bool IsInfluxNode(objFlow oFlow)
        {
            return oFlow.NodeType == e_FMNodeType.fmInflux
                && oFlow.ReceiveType != e_MsgReceiveType.emrtInfuxSumTo;
        }

        /// <summary>
        /// 是否会签环节中, 最后一个会签处理人员.
        /// </summary>
        public static bool IsLastInfluxActor(objFlow oFlow)
        {
            int unInfluxActorCount = CommonDP.GetUnInfuxActorCount(oFlow.FlowID, oFlow.NodeID);

            if (unInfluxActorCount <= 1)
                return true;
            else
                return false;
        }

    }
}
