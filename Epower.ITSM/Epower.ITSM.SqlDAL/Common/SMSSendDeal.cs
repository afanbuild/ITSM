/****************************************************************************
 * 
 * description:短信发送
 * 
 * 
 * 
 * Create by:苏康胜
 * Create Date:2009-03-22
 * *************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Epower.DevBase.BaseTools;
using System.Threading;
using Epower.DevBase.Organization.SqlDAL;
using System.Data;
using System.Data.OracleClient;
using EpowerCom;
using EpowerGlobal;
using System.Net;
using System.Net.Sockets;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// 
    /// </summary>
    public class SMSSendDeal
    {
        #region 事件管理
        /// <summary>
        /// 事件单发送短信通知
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngFlowModelID"></param>
        /// <param name="lngReceiverID"></param>
        /// <param name="lngMessageID"></param>
        /// <param name="fv"></param>
        /// <returns></returns>
        public static bool SmsNotifyReceiver(OracleTransaction trans, long lngFlowID, long lngFlowModelID, long lngReceiverID, long lngMessageID, FieldValues fv)
        {
            string[] stModelContent = MailAndMessageRuleDP.GetTemContent(trans,lngFlowID, 2, 1);
            string sMobile = "";
            Epower.DevBase.Organization.SqlDAL.UserEntity user = new UserEntity(lngReceiverID);
            sMobile = user.Mobile;

            if (sMobile.Trim() != "")
            {
                string sSubject = fv.GetFieldValue("Subject").Value.Trim();

                string sContent = fv.GetFieldValue("Content").Value.Trim();
                string sContact = fv.GetFieldValue("Contact").Value.Trim();
                string sCTel = fv.GetFieldValue("CTel").Value.Trim();

                //生成短信主体 
                string sBody = string.Empty;
                sBody = stModelContent[6].ToLower();

                #region 获取动作相关帮助信息

                long lngNodeModelID = 0;
                string strSQL = "SELECT b.nodemodelid FROM es_message a,es_node b Where a.nodeid = b.nodeid AND a.messageid=" + lngMessageID.ToString();
                OracleDataReader dr = OracleDbHelper.ExecuteReader(trans, CommandType.Text, strSQL);
                while (dr.Read())
                {
                    lngNodeModelID = (long)dr.GetDecimal(0);
                    break;
                }
                dr.Close();

                OracleConnection cn = ConfigTool.GetConnection();

                bool blnHasAction = false;
                string sDealDesc = "处理类别：";
                try
                {  //判断动作是否存在，并提示动作帮助
                    strSQL = "SELECT ActionID,ActionName " +
                    " FROM Es_N_M_Action " +
                    " WHERE flowModelID = " + lngFlowModelID.ToString() +
                    "		AND NodeModelID = " + lngNodeModelID.ToString();
                    DataTable dtActions = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                    

                    foreach (DataRow r in dtActions.Rows)
                    {
                        blnHasAction = true;
                        sDealDesc += r["actionid"].ToString() + r["actionname"].ToString().Trim();

                    }
                    if (blnHasAction == true)
                    {
                        sDealDesc = sDealDesc + " 200退回201补充意见202回收,处理格式#[标识号]#[处理类别]#[处理内容]";
                    }
                    else
                    {
                        sDealDesc = sDealDesc + "0确定200退回201补充意见202回收,处理格式#[标识号]#[处理类别]#[处理内容]";
                    }
                    
                }
                finally { ConfigTool.CloseConnection(cn); }

                #endregion


                long lngBusID = EpowerGlobal.EPGlobal.GetNextID("Ea_SmsSendID");

                sBody += " 处理标识:" + lngBusID + "可回复处理，" + sDealDesc;

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>问题单发送短信通知
        /// 
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngFlowModelID"></param>
        /// <param name="lngReceiverID"></param>
        /// <param name="lngMessageID"></param>
        /// <param name="fv"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static bool SmsNotifyReceiver(OracleTransaction trans, long lngFlowID, long lngFlowModelID, long lngReceiverID, long lngMessageID, FieldValues fv, int flag)
        {
            string[] stModelContent = MailAndMessageRuleDP.GetTemContent(trans,lngFlowID, 2, 1);
            string sMobile = "";
            Epower.DevBase.Organization.SqlDAL.UserEntity user = new UserEntity(lngReceiverID);
            sMobile = user.Mobile;

            if (sMobile.Trim() != "")
            {
                string sSubject = fv.GetFieldValue("Problem_Title").Value.Trim();

                string sContent = fv.GetFieldValue("Remark").Value.Trim();
                string sContact = fv.GetFieldValue("RegUserName").Value.Trim();

                //生成短信主体 
                string sBody = string.Empty;
                sBody = stModelContent[6].ToLower();

                #region 获取动作相关帮助信息

                long lngNodeModelID = 0;
                string strSQL = "SELECT b.nodemodelid FROM es_message a,es_node b Where a.nodeid = b.nodeid AND a.messageid=" + lngMessageID.ToString();
                OracleDataReader dr = OracleDbHelper.ExecuteReader(trans, CommandType.Text, strSQL);
                while (dr.Read())
                {
                    lngNodeModelID = (long)dr.GetDecimal(0);
                    break;
                }
                dr.Close();

                OracleConnection cn = ConfigTool.GetConnection();

                try
                { //判断动作是否存在，并提示动作帮助
                    strSQL = "SELECT ActionID,ActionName " +
                    " FROM Es_N_M_Action " +
                    " WHERE flowModelID = " + lngFlowModelID.ToString() +
                    "		AND NodeModelID = " + lngNodeModelID.ToString();
                    DataTable dtActions = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                    bool blnHasAction = false;
                    string sDealDesc = "处理类别：";

                    foreach (DataRow r in dtActions.Rows)
                    {
                        blnHasAction = true;
                        sDealDesc += r["actionid"].ToString() + r["actionname"].ToString().Trim();

                    }
                    if (blnHasAction == true)
                    {
                        sDealDesc = sDealDesc + " 200退回201补充意见202回收,处理格式#[标识号]#[处理类别]#[处理内容]";
                    }
                    else
                    {
                        sDealDesc = sDealDesc + "0确定200退回201补充意见202回收,处理格式#[标识号]#[处理类别]#[处理内容]";
                    }
                    
                }
                finally { ConfigTool.CloseConnection(cn); }

                #endregion


                long lngBusID = EpowerGlobal.EPGlobal.GetNextID("Ea_SmsSendID");

                return true;
            }
            else
            {
                return false;
            }
        }


        #endregion

        #region 事件单满意度调查
        /// <summary>
        /// 短信方式实现事件单满意度调查
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        public static bool SmsNotifyIssuesSurvey(OracleTransaction trans, long lngFlowID)
        {

            string sSubject = "";
            string sContact = "";
            string sCTel = "";

            string strSQL = "SELECT subject,contact,ctel FROM cst_issues WHERE FlowID=" + lngFlowID.ToString();
            OracleDataReader dr = OracleDbHelper.ExecuteReader(trans, CommandType.Text, strSQL);
            while (dr.Read())
            {
                sSubject = dr.GetString(0);
                sContact = dr.GetString(1);
                sCTel = dr.GetString(2);
                break;
            }
            dr.Close();

            if (sCTel.Trim() != "")
            {
                //生成短信主体 
                string sBody = string.Empty;
                //暂时不用模板方式
                sBody = sContact + "您好,事件[" + sSubject + "]已经完成，回复";

                long lngBusID = EpowerGlobal.EPGlobal.GetNextID("Ea_SmsSendID");
                sBody += ":#" + lngBusID.ToString() + "#[1基本满意2满意3不满意]#[意见] 进行满意度评选";
                //SendMessage(sCTel, sBody, "Surv", lngBusID, lngFlowID, 0);

                return true;
            }
            else
            {
                return false;
            }
        }


        #endregion 

        #region 短信模板配置共有的发送方式
        /// <summary>
        /// 短信模板配置共有的发送方式
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="receiverArray"></param>
        /// <param name="fv"></param>
        public static void SendMessagePublic(OracleTransaction trans, long lngFlowID, string receiverArray, FieldValues fv, long lngFlowModelId)
        {
            //获取收件人Email
            string getMessageByName = "";//获取收件人的Email 数组形式A
            string getMessageCopyByName = "";//获取抄送人的Email 数组形式
            string getTitle = "";//获取邮件标题
            string getMessageContent = "";//邮件的内容

            string getAppName = "";//应用名称
            long getAppId = 0;//应用id
            string flowname = ""; //流程名称
            long getMessageAppModelId = MailAndMessageRuleDP.GetOFlowModelId(lngFlowModelId, ref getAppId, ref flowname);//获得流程模型的模型id
            getAppName = MailAndMessageRuleDP.GetAppName(getAppId);

            //过滤掉是否第一个环节
            if (getAppId != 0)
            {
                #region 根据模板配置获取收件人Email
                if (fv.GetFieldValue("CTEL1") != null)
                {
                    getMessageByName = getMessagebyNamefunction(getAppId, getMessageAppModelId, fv.GetFieldValue("CTEL1").Value.Trim(), receiverArray);
                }
                else
                {
                    getMessageByName = getMessagebyNamefunction(getAppId, getMessageAppModelId, "", receiverArray);
                }
                #endregion

                //根据邮件配置，获取抄送人的Email
                getMessageCopyByName = SendCopyByName(getAppId, getMessageAppModelId);

                //获得配置内容
                getMessageContent = getModeTitle_content(getAppId, getMessageAppModelId);

                //替换各表单的不同的值
                getMessageContent = getContent(getAppId, getMessageContent, fv);
                //发送邮件的内容
                string sBody = getMessageContent;
                try
                {
                    //收件人不等于空
                    if (getMessageByName != "" && sBody.Trim() != "")
                    {
                        //短信接收人
                        string[] messageName = getMessageByName.Split(';');
                        foreach (string mobile in messageName)
                        {
                            if (mobile.Trim() != string.Empty)
                                SendMessage(mobile, sBody.Trim());
                        }
                    }

                    if (getMessageCopyByName != "" && sBody.Trim() != "")
                    {
                        //短信配置其它接收人
                        string[] copyMobile = getMessageCopyByName.Split(';');
                        foreach (string mobile in copyMobile)
                        {
                            if (mobile.Trim() != string.Empty)
                                SendMessage(mobile, sBody.Trim());
                        }
                    }

                }
                catch (Exception e)
                {

                }
            }
        }
        #endregion

        #region 获得短信模板的内容
        /// <summary>
        /// 获得模板中的标题 和 模板内容
        /// </summary>
        /// <param name="appid">应用id</param>
        /// <param name="flowmodelid">流程模型id</param>
        /// <returns>返回标题和模板内容</returns>
        public static string getModeTitle_content(long appid, long flowmodelid)
        {
            string strSQL = " SELECT R.ID,R.TEMPLATENAME,R.mailtitle as title,T.mailcontent,T.modelcontent  FROM mailandmessagetemplate T,mailandmessagerule R " +
                      " WHERE ROWNUM<=1 AND T.ID=R.TEMPLATEID AND T.STATUS=1 AND R.DELETED=0 AND R.STATUS=1 AND " +
                      "  R.SYSTEMID= " + appid + " AND R.MODELID= " + flowmodelid + "  and r.sendertypeid=1";
            string values = "";
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            if (dt.Rows.Count > 0)
            {
                values = dt.Rows[0]["modelcontent"].ToString();
            }
            return values;
        }
        #endregion

        #region 获取各表单不同的信息项内容
        /// <summary>
        /// 获取各表单不同的信息项内容
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="content"></param>
        /// <param name="fv"></param>
        /// <returns></returns>
        public static string getContent(long appId, string content, FieldValues fv)
        {
            switch (appId)
            {
                case 1028://发布管理
                    content = content.Replace(",#?版本名称?#", fv.GetFieldValue("VERSIONNAME").Value).Replace(",#?版本号?#", fv.GetFieldValue("VERSIONCODE").Value).Replace(",#?发布范围?#", fv.GetFieldValue("RELEASESCOPENAME").Value).Replace(",#?联系人?#", fv.GetFieldValue("RELEASEPERSONNAME").Value).Replace(",#?联系电话?#", fv.GetFieldValue("RELEASEPHONE").Value);
                    content = content.Replace(",#?版本性质?#", fv.GetFieldValue("VERSIONKINDNAME").Value).Replace(",#?版本类型?#", fv.GetFieldValue("VERSIONTYPENAME").Value).Replace(",#?版本发布内容简介?#", fv.GetFieldValue("RELEASECONTENT").Value);
                    break;
                case 201://自定义表单流程
                    break;
                case 400://知识管理                  
                    content = content.Replace("#?主题?#", fv.GetFieldValue("Title").Value).Replace("#?关键字?#", fv.GetFieldValue("Pkey").Value);
                    content = content.Replace("#?摘要?#", fv.GetFieldValue("tags").Value).Replace("#?知识类别?#", fv.GetFieldValue("Type").Value).Replace("#?知识内容?#", fv.GetFieldValue("Content").Value);
                    break;
                case 199://通用流程       
                    break;
                case 210://问题管理                    
                    content = content.Replace("#?登记人?#", fv.GetFieldValue("RegUserName").Value);
                    content = content.Replace("#?登记人部门?#", fv.GetFieldValue("RegDeptName").Value);
                    content = content.Replace("#?登记时间?#", fv.GetFieldValue("RegTime").Value);
                    content = content.Replace("#?影响度?#", fv.GetFieldValue("EffectName").Value);
                    content = content.Replace("#?紧急度?#", fv.GetFieldValue("InstancyName").Value);
                    content = content.Replace("#?标题?#", fv.GetFieldValue("Problem_Title").Value);
                    content = content.Replace("#?问题描述?#", fv.GetFieldValue("Problem_Subject").Value);
                    content = content.Replace("#?问题状态?#", fv.GetFieldValue("StateName").Value);
                    content = content.Replace("#?解决方案?#", fv.GetFieldValue("Remark").Value);
                    break;
                case 410://资产巡检 
                    content = content.Replace("#?标题?#", fv.GetFieldValue("Title").Value);
                    content = content.Replace("#?登记人?#", fv.GetFieldValue("RegUserName").Value);
                    content = content.Replace("#?登记部门?#", fv.GetFieldValue("RegDeptName").Value);
                    content = content.Replace("#?登记时间?#", fv.GetFieldValue("RegTime").Value);
                    content = content.Replace("#?备注?#", fv.GetFieldValue("Remark").Value);
                    break;
                case 420://变更管理                   
                    content = content.Replace("#?客户名称?#", fv.GetFieldValue("CustName").Value);
                    content = content.Replace("#?客户电话?#", fv.GetFieldValue("CTel").Value);
                    content = content.Replace("#?变更级别?#", fv.GetFieldValue("LevelName").Value);
                    content = content.Replace("#?变更状态?#", fv.GetFieldValue("DealStatus").Value);
                    content = content.Replace("#?影响度?#", fv.GetFieldValue("EffectName").Value);
                    content = content.Replace("#?摘要?#", fv.GetFieldValue("Subject").Value);
                    content = content.Replace("#?请求内容?#", fv.GetFieldValue("Content").Value);
                    content = content.Replace("#?紧急度?#", fv.GetFieldValue("InstancyName").Value);
                    break;
                case 1026://事件管理                    
                    content = content.Replace("#?客户名称?#", fv.GetFieldValue("CustName").Value);
                    content = content.Replace("#?客户手机?#", fv.GetFieldValue("CTel").Value);
                    content = content.Replace("#?客户邮件?#", fv.GetFieldValue("Email").Value);
                    content = content.Replace("#?登记时间?#", fv.GetFieldValue("RegSysDate").Value);
                    content = content.Replace("#?发生时间?#", fv.GetFieldValue("CustTime").Value == null ? "" : fv.GetFieldValue("CustTime").Value);
                    content = content.Replace("#?报告时间?#", fv.GetFieldValue("ReportingTime").Value == null ? "" : fv.GetFieldValue("ReportingTime").Value);
                    content = content.Replace("#?事件类别?#", fv.GetFieldValue("ServiceType").Value);
                    content = content.Replace("#?服务级别?#", fv.GetFieldValue("ServiceLevel").Value);
                    content = content.Replace("#?影响范围?#", fv.GetFieldValue("EffectName").Value);
                    content = content.Replace("#?紧急度?#", fv.GetFieldValue("InstancyName").Value);
                    content = content.Replace("#?摘要?#", fv.GetFieldValue("Subject").Value);
                    content = content.Replace("#?详细描述?#", fv.GetFieldValue("Content").Value);
                    break;
                default:
                    break;
            }

            return content;
        }
        #endregion

        #region 获取短信收取人的电话号码集合
        /// <summary>
        /// 获取收件人信息
        /// </summary>
        /// <param name="appid">应用id</param>
        /// <param name="modelid">流程模型id</param>
        /// <param name="clientEmail">客户电话</param>
        /// <param name="receiversIds">提交处理人以逗号隔开的形式的用户id字符串</param>
        /// <returns></returns>
        public static string getMessagebyNamefunction(long appid, long modelid, string clientMobile, string receiversIds)
        {

            //sendertypeid=1为提交时发送短信
            //提交时的接收人情况
            string strSQL = "select receiverstypeid from MailAndMessageRule t where ROWNUM<=1 AND t.systemid=" + appid + " and t.modelid =" + modelid + "  and t.status=1 and sendertypeid=1";
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            string MobileAdd = "";
            if (dt.Rows.Count > 0)
            {
                #region 获取收件人的 Email
                if (dt.Rows[0]["receiverstypeid"].ToString() == "1")
                {
                    //客户
                    MobileAdd = clientMobile;
                }
                else if (dt.Rows[0]["receiverstypeid"].ToString() == "2")
                {
                    #region 配置时，配置了处理人的情况
                    string sMobile = "";
                    string[] people = receiversIds.Trim().Split(",".ToCharArray());
                    foreach (string ple in people)
                    {
                        //处理人
                        string[] sRecMsgs = ple.Trim().Split("|".ToCharArray());
                        if (sRecMsgs[0] != "")
                        {
                            Epower.DevBase.Organization.SqlDAL.UserEntity user = new UserEntity(long.Parse(sRecMsgs[0]));
                            sMobile += user.Mobile.ToString() + ";";
                        }
                    }
                    MobileAdd = sMobile.Trim(';');
                    #endregion
                }
                else if (dt.Rows[0]["receiverstypeid"].ToString() == "3")
                {
                    #region 配置时，配置了客户和处理人的情况
                    //客户和处理人
                    string sMobile = "";
                    string[] people = receiversIds.Trim().Split(",".ToCharArray());
                    foreach (string ple in people)
                    {
                        //处理人
                        string[] sRecMsgs = ple.Trim().Split("|".ToCharArray());
                        if (sRecMsgs[0] != "")
                        {
                            Epower.DevBase.Organization.SqlDAL.UserEntity user = new UserEntity(long.Parse(sRecMsgs[0]));
                            sMobile += user.Mobile.ToString() + ";";
                        }
                    }
                    if (sMobile != "" && clientMobile != "")
                    {
                        //客户与处理都非空的情况
                        MobileAdd = clientMobile + ";" + sMobile.Trim(';');
                    }
                    else
                    {
                        if (sMobile != "")
                        {
                            //获取处理人非空，客户为空的情况
                            MobileAdd = sMobile.Trim(';');
                        }
                        else
                        {
                            //客户非空，处理人空的情况
                            MobileAdd = clientMobile;
                        }
                    }
                    #endregion
                }
                #endregion
            }
            return MobileAdd;

        }
        #endregion


        #region 获取配置短信抄送人员电话集合
        /// <summary>
        /// 获取抄送人员的手机号码的集合
        /// </summary>
        /// <param name="AppId">应用id </param>
        /// <param name="modelid">流程模型id</param>
        /// <returns>返回抄送人员Email的集合</returns>
        public static string SendCopyByName(long AppId, long modelid)
        {
            string strSQL = "select ruseridlist from MailAndMessageRule t where ROWNUM<=1 AND t.systemid=" + AppId + " and t.modelid =" + modelid + "  and t.status=1   and t.sendertypeid=1";
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            string MobileAdd = "";
            if (dt.Rows.Count > 0)
            {
                string recevide = dt.Rows[0]["ruseridlist"].ToString();
                if (recevide != "")
                {
                    string[] arrRecevide = recevide.Trim(',').Split(',');
                    string sMobile = "";
                    foreach (string recevid in arrRecevide)
                    {
                        Epower.DevBase.Organization.SqlDAL.UserEntity user = new UserEntity(long.Parse(recevid));
                        sMobile += user.Mobile + ";";
                    }
                    MobileAdd = sMobile.Trim(';');
                }
            }
            return MobileAdd;
        }
        #endregion



        #region 发送短消息
        /// <summary>
        /// 发送短消息
        /// </summary>
        /// <param name="ToNo"></param>
        /// <param name="sBody"></param>
        /// <param name="strChannel"></param>
        /// <param name="lngBusID"></param>
        /// <param name="lngEpowerID"></param>
        /// <param name="iBusType"></param>
        public static void SendMessage(string ToNo, string sBody)
        {
            setcount strValue = new setcount();
            strValue.mobileNo = ToNo;
            strValue.content = sBody;
            Thread thread1 = new Thread(SendMobileMessage);
            thread1.Start((object)strValue);
        }

        #region 同步发送短信使用线程方式发送

        #region 设置传入的短信类容的值
        /// <summary>
        /// 设置传入的短信类容的值
        /// </summary>
        public struct setcount
        {
            /// <summary>
            /// 手机号码
            /// </summary>
            public string mobileNo;
            /// <summary>
            /// 发送的内容
            /// </summary>
            public string content;
        }
        #endregion

        #region 接口短信
        /// <summary>
        /// 接口短信
        /// </summary>
        /// <param name="strValueOjb"></param>
        public static void SendMobileMessage(object strValueOjb)
        {
            setcount value = (setcount)strValueOjb;
            string mobileNo = value.mobileNo;
            string content = value.content;
            try
            {
                int port = int.Parse(Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "sendPort"));//端口号
                string host = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "sendIpAddress");//服务器IP
                //*******************报文头*************************************************//
                //string stAllLenth = "";//总报文长（不包含本字段的长度）6
                string stSerialNumber = System.DateTime.Now.ToString("yyyyMMdd") + "00";//数据包序列号(供发送方进行报文配对检查。保证每天唯一)10
                string stType = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Title_MessageType");//报文类型(1-联机交易 2-文件传输)
                string stSource = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Title_MessageSource");//报文来源 2
                string stCode = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Title_TransactionCode");//交易代码
                string stLevel = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Title_Level");//优先级(1-9 数字越大优先级越高)
                string stDate = System.DateTime.Now.ToString("yyMMdd");//交易日期
                string stTime = System.DateTime.Now.ToString(System.DateTime.Now.ToLongTimeString().Replace(":", "") + System.DateTime.Now.Millisecond.ToString()); ;//交易时间
                string stReqFlag = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Title_ReqFlag");//请求/应答标识(R-请求 A-应答)
                string stEncFlag = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Title_EncFlag");//加密标志(Y-加密 其它-不加密)
                string stPakNumber = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Title_PakNumber"); //包的记录数(单包时，填1。多包时，填总包数)
                string stPakSerialNum = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Title_PakSerialNum");//当前包序号(单包时，填1。多包时，填实际的包号。)
                string stFIELDS = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Title_FIELDS");//数据部份域段数(填报文体中的字段数)
                string stSuccess = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Title_Success"); //成功处理标志(应答时，必填。‘1’-成功；其它-失败。失败原因将在返回包中描述)
                string stReserve = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Title_Reserve");//保留
                //*******************报文体*************************************************//
                string stZDK = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Body_Sub-port");//子端口号
                string stYWDM = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Body_BusinessCode");//业务代码
                string stFSJG = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Body_SendDeptID");//发送部门(机构)ID
                string stYYSID = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Body_OperatorsID");//运营商ID
                string stSJH = mobileNo;//消息传递(请求)对象
                string stDXBM = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Body_MessageCode");// 短信编码
                string stContent = content;//发送内容
                string stXX = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Body_MessageTT");//信息码
                string stYXJ = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Body_Level");//优先级
                string stFSCS = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Body_SendNumber");//发送次数
                string stTHH = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "TeFuhaoFlag");// 特服号标识
                string stJFBS = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "BillingFlag");//计费标识
                //报文头
                string stSendHead = stSerialNumber + stType + stSource;
                stSendHead = stSendHead + stCode + stLevel + stDate + stTime + stReqFlag;
                stSendHead = stSendHead + stEncFlag + stPakNumber + stPakSerialNum + stFIELDS + stSuccess + stReserve;
                //报文体
                string stSendBody = "01" + getStringByteLenth(stZDK, 4) + stZDK + "02" + getStringByteLenth(stYWDM, 4) + stYWDM + "03" + getStringByteLenth(stFSJG, 4) + stFSJG + "04" + getStringByteLenth(stYYSID, 4) + stYYSID + "05" + getStringByteLenth(stSJH, 4) + stSJH + "06" + getStringByteLenth(stDXBM, 4) + stDXBM;
                stSendBody = stSendBody + "07" + getStringByteLenth(stXX, 4) + stXX + "08" + getStringByteLenth(stContent, 4) + stContent + "09" + getStringByteLenth(stYXJ, 4) + stYXJ + "10" + getStringByteLenth(stFSCS, 4) + stFSCS + "11" + getStringByteLenth(stTHH, 4) + stTHH + "12" + getStringByteLenth(stJFBS, 4) + stJFBS;
                //组成的报文
                string stSend = getStringByteLenth(stSendHead + stSendBody, 6) + stSendHead + stSendBody;
                if (Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "sendIsOpen").Trim() == "1")
                {
                    try
                    {
                        IPAddress ip = IPAddress.Parse(host);
                        IPEndPoint ipe = new IPEndPoint(ip, port);//把ip和端口转化为IPEndPoint实例
                        Socket c = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建一个Socket
                        c.Connect(ipe);//连接到服务器
                        byte[] bs = Encoding.GetEncoding("GBK").GetBytes(stSend.Trim());
                        c.Send(bs, bs.Length, 0);//发送测试信息
                        string recvStr = "";
                        byte[] recvBytes = new byte[1024];
                        int bytes;
                        bytes = c.Receive(recvBytes, recvBytes.Length, 0);//从服务器端接受返回信息
                        recvStr += Encoding.GetEncoding("GB2312").GetString(recvBytes, 0, bytes);
                        c.Close();
                        if (recvStr != "")
                        {
                            if (recvStr.Substring(70, 4).ToString() == "0000")
                            {
                                createSmsLog(mobileNo, content, "短信发送成功");
                            }
                            else
                            {
                                createSmsLog(mobileNo, content, "短信发送失败，接收返回内容包：" + recvStr + ", 发送内容包：" + stSend);
                            }
                        }
                        else
                        {
                            createSmsLog(mobileNo, content, "短信发送失败，未收到数据反馈包，发送内容包：" + stSend);
                        }
                    }
                    catch (Exception ex)
                    {
                        createSmsLog(mobileNo, content, "短信发送失败，失败原因如下" + ex.Message);
                    }
                }
                else
                {
                    createSmsLog(mobileNo, content, " 短信发送失败，未开启短信功能：" + stSend);
                }
            }
            catch (ArgumentNullException e1)
            {
                createSmsLog(mobileNo, content, "短信发送失败， 异常：" + e1.Message);
            }
            catch (SocketException e2)
            {
                createSmsLog(mobileNo, content, "短信发送失败， 异常：" + e2.Message);
            }
        }
        #endregion

        #region 短信日志
        /// <summary>
        ///短信日志
        /// </summary>
        /// <param name="mobileNo"></param>
        /// <param name="content"></param>
        /// <param name="message"></param>
        public static void createSmsLog(string mobileNo, string content, string message)
        {
            string strSQl = "  insert into  sen_message_log(mobile,conter,sendIsTrueMessage,operateTime) values (" + StringTool.SqlQ(mobileNo) + "," + StringTool.SqlQ(content) + "," + StringTool.SqlQ(message) + ",sysdate)";
            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            OracleTransaction trans = cn.BeginTransaction();
            try
            {
                OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQl);
                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback();
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }
        }
        #endregion

        #region 内容的字节数计算
        /// <summary>
        /// 内容的字节数计算
        /// </summary>
        /// <param name="stContent"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        private static string getStringByteLenth(string stContent, int len)
        {
            string stLen = Convert.ToString(System.Text.Encoding.GetEncoding("GB2312").GetByteCount(stContent));
            if (len == 4)
            {
                switch (stLen.Length)
                {
                    case 0:
                        stLen = "0000";
                        break;
                    case 1:
                        stLen = "000" + stLen;
                        break;
                    case 2:
                        stLen = "00" + stLen;
                        break;
                    case 3:
                        stLen = "0" + stLen;
                        break;
                    default:
                        break;
                }
            }
            else if (len == 6)
            {
                switch (stLen.Length)
                {
                    case 0:
                        stLen = "000000";
                        break;
                    case 1:
                        stLen = "00000" + stLen;
                        break;
                    case 2:
                        stLen = "0000" + stLen;
                        break;
                    case 3:
                        stLen = "000" + stLen;
                        break;
                    case 4:
                        stLen = "00" + stLen;
                        break;
                    case 5:
                        stLen = "0" + stLen;
                        break;
                    default:
                        break;
                }
            }
            return stLen;
        }
        #endregion

        #endregion


        #region 发送短信
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="ToNo"></param>
        /// <param name="sBody"></param>
        /// <param name="strChannel"></param>
        /// <param name="lngBusID"></param>
        /// <param name="lngEpowerID"></param>
        /// <param name="iBusType"></param>
        private static void Send(string ToNo, string sBody, string strChannel, long lngBusID, long lngEpowerID, int iBusType)
        {
            OracleConnection cn = new OracleConnection();
            string sToNo = ToNo;
            long lngBusSerialID = lngBusID;
            long lngNextID = EPGlobal.GetNextID("Ea_SmsSendID");
            if (lngBusID == 0)
            {
                //如果业务系列号 为0 则是方法内产生一个系列号
                lngBusSerialID = lngNextID;
            }

            try
            {
                cn = ConfigTool.GetConnection();
                string sSql = "Insert Into ea_smssend(id,schannel,tonumber,content,busid,bustype,epowerid,sendtime,status,respstatus)values(" +
                    lngNextID.ToString() + "," +
                StringTool.SqlQ(strChannel) + "," +
                StringTool.SqlQ(sToNo) + "," +
                StringTool.SqlQ(sBody) + "," +
                lngBusSerialID.ToString() + "," +
                iBusType.ToString() + "," +
                lngEpowerID.ToString() + "," +
                "sysdate" + "," + "0,0" + ")";
                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, sSql);
            }
            catch (Exception e)
            {
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    ConfigTool.CloseConnection(cn);
                }
            }

        }
        #endregion

        #endregion
    }
}

