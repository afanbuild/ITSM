/****************************************************************************
 * 
 * description:邮件发送
 * 
 * 
 * 
 * Create by:朱明春
 * Create Date:2008-07-28
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
using System.Text.RegularExpressions;
using EpowerGlobal;
using Epower.DevBase.Organization;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// 
    /// </summary>
    public class MailSendDeal
    {
        private static string sSmtpServer = CommonDP.GetConfigValue("SetMail", "smtpserver");
        private static string sPsd = CommonDP.GetConfigValue("SetMail", "smtppsd");
        private static string sFrom = CommonDP.GetConfigValue("SetMail", "smtpfrom");
        private static string sSSL = CommonDP.GetConfigValue("SetMail", "smtpSSL");
        private static string sPort = CommonDP.GetConfigValue("SetMail", "smtpPort");
        private static string sUserName = CommonDP.GetConfigValue("SetMail", "smtpUserName");
        private static string smtpdisplayName = CommonDP.GetConfigValue("SetMail", "smtpDisplayName");

        #region 知识管理
        /// <summary>
        /// 知识评分时发送邮件
        /// </summary>
        /// <param name="lngKBID"></param>
        /// <param name="lngCommentUserID"></param>
        /// <param name="CommentUserName"></param>
        /// <returns></returns>
        public static bool InfCommentSend(long lngKBID, long lngCommentUserID, string CommentUserName)
        {
            DataTable dt;
            string sSql = @"select a.ID,Score,Title,Pkey,Tags,TypeName,CreationName,UpdateTime,RegUserID,RegUserName,Content,
                                Case IsShow when 0 then '否'  else '是' end IsShow
                                from Inf_Information a,Inf_Score b 
                                where a.ID=b.KBID and b.UserID=" + lngCommentUserID.ToString() + " And b.KBID=" + lngKBID.ToString();
            dt = CommonDP.ExcuteSqlTable(sSql);

            UserEntity pUserEntity = new UserEntity(long.Parse(dt.Rows[0]["RegUserID"].ToString()));
            string sEmail = pUserEntity.Email.Trim();
            if (sEmail == string.Empty)  //如果收件人不存在,则不发送 
                return false;


            //邮件标题
            string sSubject = CommentUserName + ":进行了知识评分!";
            //生成邮件主体 
            string sBody = string.Empty;
            sBody = MailBodyDeal.GetMailBody("Inf_Comment.htm", dt);
            try
            {
                //多线程发送电子邮件
                using (MailToolAgent ma = new MailToolAgent(sEmail, "", sSubject, sBody, sSmtpServer, sPsd, sFrom, sSSL, sPort, sUserName))
                {
                    Thread d = new Thread(new ThreadStart(ma.DoAction));
                    d.Start();
                }
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// 知识推荐审批结束时发送邮件
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        public static bool InfReCommendSend(OracleTransaction trans, long lngFlowID)
        {
            DataTable dt;
            string sSql = @"select KBID,Title,Pkey,Tags,TypeName,CreationName,RegTime,RegUserName,Content,
                                nvl(ReCommendUserID,0) ReCommendUserID,ReCommendUserName,
	                            Case IsInKB when 1 then '已审批通过并入库！'  when 0 then '未审批通过！' end Pass,
                                Case IsShow when 0 then '否'  else '是' end IsShow
                                from Inf_KMBase
                                where FlowID=" + lngFlowID.ToString();
            dt = CommonDP.ExcuteSqlTable(trans, sSql);

            UserEntity pUserEntity = new UserEntity(long.Parse(dt.Rows[0]["ReCommendUserID"].ToString()));
            string sEmail = pUserEntity.Email.Trim();
            if (sEmail == string.Empty)  //如果收件人不存在,则不发送 
                return false;


            //邮件标题
            string sSubject = "您推荐的知识：" + dt.Rows[0]["Pass"].ToString();
            //生成邮件主体 
            string sBody = string.Empty;
            sBody = MailBodyDeal.GetMailBody("Inf_ReCommend.htm", dt);
            try
            {
                //多线程发送电子邮件
                using (MailToolAgent ma = new MailToolAgent(sEmail, "", sSubject, sBody, sSmtpServer, sPsd, sFrom, sSSL, sPort, sUserName))
                {
                    Thread d = new Thread(new ThreadStart(ma.DoAction));
                    d.Start();
                }
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }
        #endregion

        #region 事件管理
        /// <summary>
        ///  取得数据
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        private static DataTable GetSendData(OracleTransaction trans, long lngFlowID)
        {
            string sSql = @"select a.*,nvl(a.buildCode,'')||nvl(a.ServiceNo,'') ServiceNum,b.Code,b.Positions,b.SerialNumber,b.Model,b.Breed from Cst_Issues a left outer join Equ_Desk b ON b.ID=a.EquipmentID
                                where FlowID=" + lngFlowID.ToString();
            DataTable dt = CommonDP.ExcuteSqlTable(trans, sSql);
            return dt;
        }
        /// <summary>
        ///  取得数据
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        public static DataTable GetSendData(long lngFlowID)
        {
            string sSql = @"select a.DealContent,a.*,nvl(a.buildCode,'')||nvl(a.ServiceNo,'') ServiceNum,b.Code,b.Positions,b.SerialNumber,b.Model,b.Breed,nvl(c.Email,'') Email,nvl(c.ShortName,'') ShortName,a.CustName from Cst_Issues a left outer join Equ_Desk b ON b.ID=a.EquipmentID
                                Left outer join Br_ECustomer c On a.CustID=c.ID
                                where a.FlowID=" + lngFlowID.ToString();
            DataTable dt = CommonDP.ExcuteSqlTable(sSql);
            return dt;
        }

        #region 邮件模板共有的发送方式       


        /// <summary>
        /// 似乎只用在事件，补充说明wxh
        /// 重新梳理发送邮件的逻辑规则，将此功能改成通用模块,发送邮件的主入口方法 yxq 2014-07-15
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="receiverArray"></param>
        /// <param name="fv"></param>
        /// <param name="lngFlowModelId"></param>
        /// <param name="lngFlag"></param>
        /// <param name="lngMessageId"></param>
        public static void SendEmailPublicV2(OracleTransaction trans, long lngFlowID, string receiverArray, FieldValues fv, long lngFlowModelId, long lngFlag, long lngMessageId)
        {
            string getAppName = "";//应用名称
            long lngAppID = 0;//应用id
            string flowname = ""; //流程名称
            long getEmailAppModelId = MailAndMessageRuleDP.GetOFlowModelId(lngFlowModelId, ref lngAppID, ref flowname);//获得流程模型的模型id


            if (getEmailAppModelId > 0)
            {

                getAppName = MailAndMessageRuleDP.GetAppName(lngAppID);

                long nodemodelid = GetNodeModeID(trans, lngFlowModelId, lngMessageId); //获得环节ID

                #region 根据环节配置规则发邮件

                //判断是否配置有所有环节设置
                string strSQL1 = @" select * from Br_Messagerulinstall where oflowmodelid=" + getEmailAppModelId + " and nodeid=-1 and trigger_type= '环节'";
                DataTable dtallNode = OracleDbHelper.ExecuteDataset(trans, CommandType.Text, strSQL1).Tables[0];

                //给所有人发邮件
                if (dtallNode.Rows.Count == 1)
                {
                    SendEmailByNodeModelV2(fv, receiverArray, nodemodelid, getEmailAppModelId, lngAppID, flowname, lngFlag, getAppName, dtallNode);
                }
                else  //根据环节发邮件
                {
                    //根据环节id和流程模型id在规则表中找到对应的记录
                    string strSQL3 = @" select * from Br_Messagerulinstall where rownum=1 and
                 oflowmodelid=" + getEmailAppModelId + " and nodeid=" + nodemodelid + " and trigger_type = '环节'";

                    //根据查找的记录，取接收类型，关联的邮件模板
                    DataTable dtEmail = OracleDbHelper.ExecuteDataset(trans, CommandType.Text, strSQL3).Tables[0];

                    if (dtEmail.Rows.Count > 0)
                    {
                        SendEmailByNodeModelV2(fv, receiverArray, nodemodelid, getEmailAppModelId, lngAppID, flowname, lngFlag, getAppName, dtEmail);

                    }

                }

                #endregion
            }

        }

        #region 获取环节模型ID
        /// <summary>
        /// 获取环节模型ID
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowModelId"></param>
        /// <param name="lngMessageId"></param>
        /// <returns>nodemodelid</returns>
        public static long GetNodeModeID(OracleTransaction trans, long lngFlowModelId, long lngMessageId)
        {
            //根据lngMessageID找到对应的环节ID
            string strSQL = string.Empty;
            strSQL = @"select a.messageid,a.nodeid,b.flowmodelid 
                        from es_message a left join es_node b on a.nodeid=b.nodeid 
                        where flowmodelid=" + lngFlowModelId
                        + " and messageid=" + lngMessageId;
            DataTable dtNode = OracleDbHelper.ExecuteDataset(trans, CommandType.Text, strSQL).Tables[0];
            long nodeid = 0;
            if (dtNode.Rows.Count == 1)
            {
                nodeid = long.Parse(dtNode.Rows[0]["NODEID"].ToString() == "" ? "0" : dtNode.Rows[0]["NODEID"].ToString());
            }

            //在es_node中根据node找到环节id
            string strSQL2 = @"select nodemodelid from es_node where nodeid=" + nodeid;
            DataTable dtnodemodelid = OracleDbHelper.ExecuteDataset(trans, CommandType.Text, strSQL2).Tables[0];
            long nodemodelid = 0;
            if (dtnodemodelid.Rows.Count == 1)
            {
                nodemodelid = long.Parse(dtnodemodelid.Rows[0]["NodeModelid"].ToString() == "" ? "0" : dtnodemodelid.Rows[0]["NodeModelid"].ToString());
            }
            return nodemodelid;
        }
        #endregion

        #region 获取接收人Email
        /// <summary>
        /// 获取接收人Email
        /// </summary>
        /// <param name="userids">用户ID</param>
        /// <returns></returns>
        public static DataTable GetRecipient_User_Email(string userids)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt_user_Emails = new DataTable();

            try
            {
                if (userids != "")
                {
                    if (userids.LastIndexOf(',') == userids.Length - 1)
                        userids = userids.Substring(0, userids.Length - 1);

                    string sSql = string.Format("select email,loginname from ts_user where userid in({0})", userids);
                    dt_user_Emails = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sSql);
                }
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
            return dt_user_Emails;
        }
        #endregion
     

        #region 根据环节发邮件,分开发送
        /// <summary>
        /// 根据环节发邮件,分开发送
        /// </summary>
        /// <param name="fv">页面表单数据XML串</param>
        /// <param name="receiverArray">处理人列表</param>
        /// <param name="nodemodelid">当前环节模型ID</param>
        /// <param name="getEmailAppModelId">流程OFlowModelID</param>
        /// <param name="lngAppID">应用ID</param>
        /// <param name="flowname">流程名称</param>
        /// <param name="lngFlag">是否发送抄送人员 0否 1是</param>
        /// <param name="getAppName">应用名称</param>
        /// <param name="dt"></param>
        public static void SendEmailByNodeModelV2(FieldValues fv, string receiverArray, long nodemodelid, long getEmailAppModelId, long lngAppID, string flowname, long lngFlag, string getAppName, DataTable dt)
        {
            //通用流程替换内容时需要使用,临时存储
            fv.Add("oflowmodelid", getEmailAppModelId.ToString());

            string getEmailByName = "";//获取收件人的Email 数组形式A
            string getEmailCopyByName = "";//获取抄送人的Email 数组形式
            string getTitle = "";//获取邮件标题           

            #region 循环遍历邮件规则中的数据

            foreach (DataRow dr in dt.Rows)
            {
                string recervicesType = "";  //接收类型
                string flowModelName = "";   //环节关联模板
                string flownameid = "";//环节关联模版ID
                string nodeContent = "";    //环节模板标题
                string userids = "";        //接收人ID
                string triggertype = "";//触发类型
                string nodename = "";//环节

                recervicesType = dr["ReceiversTypeName"].ToString();
                flowModelName = dr["FlowName"].ToString();
                flownameid = dr["FlowNameID"].ToString();
                nodeContent = dr["NodeContent"].ToString();
                nodemodelid = long.Parse(dr["NodeID"].ToString());
                userids = dr["RECIPIENT_USERID"].ToString();
                triggertype = dr["trigger_type"].ToString();
                nodename = dr["nodename"].ToString();

                
                DataTable dt_emailmodel = GetoneEmailModel(flownameid);//获取邮件模版

                #region 给接收人列表发邮件
                //给接收人列表发邮件
                if (!string.IsNullOrEmpty(userids))
                {
                    string sBody = "";
                    if (dt_emailmodel != null && dt_emailmodel.Rows.Count > 0)
                    {
                        sBody = dt_emailmodel.Rows[0]["leadercontent"].ToString();
                        sBody = getContent(lngAppID, sBody, fv);
                    }
                    DataTable dt_user_Emails = GetRecipient_User_Email(userids);
                    if (dt_user_Emails != null && dt_user_Emails.Rows.Count > 0)
                    {
                        foreach (DataRow ddr in dt_user_Emails.Rows)
                        {                         

                            Thread(ddr["email"].ToString(), getEmailCopyByName, nodeContent, sFrom, sBody, lngAppID, getAppName, getEmailAppModelId, flowname, triggertype, nodename);
                        }
                    
                    }
                }
                #endregion

                #region 获取需要发送人的邮箱及对应配置的邮件内容

                List<UserEntity> extList = new List<UserEntity>();
                List<UserEntity> extListCC = new List<UserEntity>();

                string temp1=string.Empty;
                string temp2=string.Empty;
                string strClientEmail = string.Empty;

                //根据配置获取需要发送邮件的人员邮箱列表
                if (fv.GetFieldValue("Email") != null)
                {
                    strClientEmail = fv.GetFieldValue("Email").Value.Trim();
                    extList = getEmaibyNamefunctionV2(lngAppID, getEmailAppModelId, fv.GetFieldValue("Email").Value.Trim(), receiverArray, recervicesType, getEmailAppModelId, nodemodelid, ref temp1,ref temp2);
                }
                else
                {
                    extList = getEmaibyNamefunctionV2(lngAppID, getEmailAppModelId, "", receiverArray, recervicesType, getEmailAppModelId, nodemodelid, ref temp1, ref temp2);
                }
                               

                if (lngAppID != 0)
                {
                    getTitle = nodeContent;  //邮件标题

                    //if (lngFlag > 0)
                    //{
                    //    //获取抄送人员列表 发送短信的地方用到，邮件不在获取此人员列表
                    //    extListCC = EmailCopyByNameV2(lngAppID, getEmailAppModelId);
                    //}                    

                    //替换各表单的不同的值                
                    string sBody = "";

                    try
                    {
                        //给上报人发邮件
                        if (recervicesType.Contains("客户") == true && strClientEmail!=string.Empty)
                        {
                            if (MailTool.IsEmail(strClientEmail))
                            {
                                sBody = temp1;
                                //替换各表单的不同的值
                                sBody = getContent(lngAppID, sBody, fv);                                

                                Thread(strClientEmail, "", getTitle, sFrom, sBody, lngAppID, getAppName, getEmailAppModelId, flowname, triggertype, nodename);
                            }
                        }

                        //给处理人发邮件
                        foreach (UserEntity user in extList)
                        {
                            if (MailTool.IsEmail(user.Email))
                            {
                                sBody = temp2;
                                //替换各表单的不同的值
                                sBody = getContent(lngAppID, sBody, fv);                                

                                Thread(user.Email, "", getTitle, sFrom, sBody, lngAppID, getAppName, getEmailAppModelId, flowname, triggertype, nodename);
                            }

                        }                    
                    }
                    catch (Exception e)
                    {
                        E8Logger.Error(e);
                    }
                }
                #endregion
            }
            #endregion
        }
        #endregion

        #region 得到事件单信息
        public static DataTable GetCstIss(string cstno)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt_Cst = new DataTable();
            string serviceno = "";
            string buildcode = "";
            if (cstno != "")
            {
                try
                {
                    int lastindex = cstno.LastIndexOf('-');
                    buildcode = cstno.Substring(0, lastindex + 1);
                    serviceno = cstno.Substring(lastindex + 1, cstno.Length - (lastindex + 1));

                    string secst = string.Format("select * from cst_issues where serviceno='{0}' and buildcode='{1}'", serviceno, buildcode);
                    dt_Cst = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, secst);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ConfigTool.CloseConnection(cn);
                }
            }
            return dt_Cst;
        }
        #endregion

        #region 拼写Email
        public static string PEmail(string sbody, string cstno, DataRow dr_Cst)
        {
            sbody = sbody.Replace("#?事件单号?#", cstno);
            sbody = sbody.Replace("#?客户名称?#", dr_Cst["CustName"].ToString());
            sbody = sbody.Replace("#?客户手机?#", dr_Cst["CTel"].ToString());
            sbody = sbody.Replace("#?客户邮件?#", dr_Cst["Email"].ToString());
            sbody = sbody.Replace("#?登记时间?#", dr_Cst["RegSysDate"].ToString());
            sbody = sbody.Replace("#?发生时间?#", dr_Cst["CustTime"].ToString());
            sbody = sbody.Replace("#?报告时间?#", dr_Cst["reportingtime"].ToString());
            sbody = sbody.Replace("#?事件类别?#", dr_Cst["servicetype"].ToString());
            sbody = sbody.Replace("#?服务级别?#", dr_Cst["servicelevel"].ToString());
            sbody = sbody.Replace("#?影响范围?#", dr_Cst["effectname"].ToString());
            sbody = sbody.Replace("#?紧急度?#", dr_Cst["InstancyName"].ToString());
            sbody = sbody.Replace("#?摘要?#", dr_Cst["subject"].ToString());
            sbody = sbody.Replace("#?详细描述?#", dr_Cst["content"].ToString());

            return sbody;
        }
        #endregion

        #region 得到邮件模版
        /// <summary>
        /// 获取邮件模版
        /// </summary>
        /// <returns></returns>
        public static DataTable GetoneEmailModel(string flownameid, long lngFlowModelID)
        {
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                StringBuilder sbSQL = new StringBuilder();
                sbSQL.AppendFormat(@"SELECT mailcontent,dealmailcontent,leadercontent 
                                                    FROM MailAndMessageTemplate  where id ={0}", flownameid);

                if (lngFlowModelID > 0)
                {
                    sbSQL.AppendFormat(" and flowmodelid = {0} ", lngFlowModelID);
                }

                DataTable dt_Email = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sbSQL.ToString());
                return dt_Email;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }
        #endregion

        #region 得到邮件模版
        /// <summary>
        /// 获取邮件模版
        /// </summary>
        /// <returns></returns>
        public static DataTable GetoneEmailModel(string flownameid)
        {
            return MailSendDeal.GetoneEmailModel(flownameid, 0);
        }
        #endregion

        #region  根据FlowID查询出对应的当前处理人
        /// <summary>
        /// 根据flowid查询当前处理人的email
        /// </summary>
        /// <param name="strFlowID"></param>
        /// <returns></returns>
        public static string GetUserEmail(string cstno)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string email = "";
            string serviceno = "";
            string buildcode = "";
            string strFlowID = "0";
            if (cstno != "")
            {
                try
                {
                    int lastindex = cstno.LastIndexOf('-');
                    buildcode = cstno.Substring(0, lastindex + 1);
                    serviceno = cstno.Substring(lastindex + 1, cstno.Length - (lastindex + 1));

                    string secst = string.Format("select FlowID from cst_issues where serviceno='{0}' and buildcode='{1}'", serviceno, buildcode);
                    DataTable dt_cst = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, secst);
                    if (dt_cst != null && dt_cst.Rows.Count > 0)
                    {
                        strFlowID = dt_cst.Rows[0]["FlowID"].ToString();
                    }

                    string sSql = string.Format("SELECT c.email FROM ES_MESSAGE a LEFT JOIN TS_USER c ON a.ReceiverID=c.UserID WHERE a.MessageID = "
                        + "( " + "SELECT max(b.MessageID)MessageID FROM CST_ISSUES a LEFT JOIN ES_MESSAGE b ON a.FlowID=b.FlowID WHERE a.FlowID=" + strFlowID + ")");
                    DataTable dt_user = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sSql);
                    if (dt_user != null && dt_user.Rows.Count > 0)
                        email = dt_user.Rows[0]["email"].ToString();
                }
                finally
                {
                    ConfigTool.CloseConnection(cn);
                }
            }
            return email;
        }
        #endregion


        public static void Thread(string getEmailByName, string getEmailCopyByName, string getTitle, string sFrom, string sBody,
            long lngAppID, string getAppName, long getEmailAppModelId, string flowname, string triggertype, string nodename)
        {
            
            MailSendLog(getEmailByName + "," + getEmailCopyByName, sFrom, sBody, "1", lngAppID, getAppName, getEmailAppModelId, flowname);//发送日志
            using (MailToolAgent ma = new MailToolAgent(getEmailByName, getEmailCopyByName, getTitle, sBody, sSmtpServer, sPsd, sFrom, sSSL, sPort, sUserName, smtpdisplayName, true))
            {
                Thread d = new Thread(new ThreadStart(ma.DoAction));
                d.Start();               
            }

        }
        #endregion        

        /// <summary>
        /// 获取邮件接收人信息 upt by wxh
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="medelid"></param>
        /// <param name="clientEmail"></param>
        /// <param name="receiversIds"></param>
        /// <param name="recerviceType"></param>
        /// <param name="getEmailAppModelId"></param>
        /// <param name="nodemodelid"></param>
        /// <returns></returns>
        public static List<UserEntity> getEmaibyNamefunctionV2(long appid, long medelid, string clientEmail, string receiversIds, string recerviceType, long getEmailAppModelId, long nodemodelid,ref string temp1,ref string temp2)
        {
            List<UserEntity> list = new List<UserEntity>();
            string[] arrTitle_content = getModeTitle_content(appid, getEmailAppModelId);
            if (recerviceType == "处理人")
            {
                #region 配置时，配置了处理人的情况               
                string[] people = receiversIds.Trim().Split(",".ToCharArray());
                foreach (string ple in people)
                {
                    //处理人
                    string[] sRecMsgs = ple.Trim().Split("|".ToCharArray());
                    if (sRecMsgs[0] != "")
                    {
                        Epower.DevBase.Organization.SqlDAL.UserEntity user = new UserEntity(long.Parse(sRecMsgs[0]));   
                        list.Add(user);
                    }
                }              
                temp1 = (arrTitle_content == null || arrTitle_content.Length < 3) ? "" : arrTitle_content[3];
                #endregion

            }
            else if (recerviceType == "客户")
            { 
                temp1 = arrTitle_content[1];
            }
            else if (recerviceType == "客户和处理人")
            {      

                #region 配置时，配置了客户和处理人的情况
                string[] people = receiversIds.Trim().Split(",".ToCharArray());
                foreach (string ple in people)
                {
                    //处理人
                    string[] sRecMsgs = ple.Trim().Split("|".ToCharArray());
                    if (sRecMsgs[0] != "")
                    {
                        Epower.DevBase.Organization.SqlDAL.UserEntity user = new UserEntity(long.Parse(sRecMsgs[0]));   
                        list.Add(user);
                    }
                }                

                temp1 = arrTitle_content[1].ToString(); //上报人邮件模板
                temp2 = arrTitle_content[3].ToString(); //处理人邮件模板

                #endregion
            }
            return list;
        }

        
        #region 获得邮件模板的标题和内容
        /// <summary>
        /// 获得模板中的标题 和 模板内容
        /// </summary>
        /// <param name="appid">应用id</param>
        /// <param name="flowmodelid">流程模型id</param>
        /// <returns>返回标题和模板内容</returns>
        public static string[] getModeTitle_content2(long flowmodelid, long nodemodelid)
        {
            string strSQL = " select * from mailandmessagetemplate b" +
                        " inner join (select * from Br_Messagerulinstall where oflowmodelid=" +
                        flowmodelid + " and NodeID=" + nodemodelid + " ) a  on a.flownameid = b.id";
            string[] values = null;
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            if (dt.Rows.Count > 0)
            {
                values = new string[4];
                values[0] = dt.Rows[0]["ModelContent"].ToString();
                values[1] = dt.Rows[0]["mailcontent"].ToString(); //上报人模板
                values[2] = dt.Rows[0]["remark"].ToString();
                values[3] = dt.Rows[0]["Dealmailcontent"].ToString();  //处理人模板
            }
            return values;
        }

        public static string[] getModeTitle_content(long appid, long flowmodelid)
        {
            string strSQL = " SELECT  R.ID,R.TEMPLATENAME,R.mailtitle as title,T.mailcontent,T.Dealmailcontent,T.remark  FROM mailandmessagetemplate T,mailandmessagerule R " +
                      " WHERE rownum<=1 and T.ID=R.TEMPLATEID AND T.STATUS=1 AND R.DELETED=0 AND R.STATUS=1 AND " +
                      "  R.SYSTEMID= " + appid + " AND R.MODELID= " + flowmodelid + " and R.sendertypeid=1";
            string[] values = null;
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            if (dt.Rows.Count > 0)
            {
                values = new string[4];
                values[0] = dt.Rows[0]["title"].ToString();
                values[1] = dt.Rows[0]["mailcontent"].ToString(); //上报人模板
                values[2] = dt.Rows[0]["remark"].ToString();
                values[3] = dt.Rows[0]["Dealmailcontent"].ToString();  //处理人模板
            }
            return values;
        }

        //返回标题和上报人内容


        #endregion

        #region 获取邮件接收人的 Email
        /// <summary>
        /// 获取收件人信息
        /// </summary>
        /// <param name="appid">应用id</param>
        /// <param name="modelid">流程模型id</param>
        /// <param name="clientEmail">客户Email</param>
        /// <param name="receiversIds">提交处理人以逗号隔开的形式的用户id字符串</param>
        /// <returns></returns>
        public static string getEmaibyNamefunction(long appid, long modelid, string clientEmail, string receiversIds)
        {

            //sendertypeid=1为提交时发送短信
            //提交时的接收人情况
            string strSQL = "select receiverstypeid from MailAndMessageRule t where rownum<=1 and t.systemid=" + appid + " and t.modelid =" + modelid + "  and t.status=1 and sendertypeid=1";
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            string EmialAdd = "";
            if (dt.Rows.Count > 0)
            {
                #region 获取收件人的 Email
                if (dt.Rows[0]["receiverstypeid"].ToString() == "1")
                {
                    //客户
                    EmialAdd = clientEmail;
                }
                else if (dt.Rows[0]["receiverstypeid"].ToString() == "2")
                {
                    #region 配置时，配置了处理人的情况
                    string sEmail = "";
                    string[] people = receiversIds.Trim().Split(",".ToCharArray());
                    foreach (string ple in people)
                    {
                        //处理人
                        string[] sRecMsgs = ple.Trim().Split("|".ToCharArray());
                        if (sRecMsgs[0] != "")
                        {
                            Epower.DevBase.Organization.SqlDAL.UserEntity user = new UserEntity(long.Parse(sRecMsgs[0]));
                            sEmail += user.Email.ToString() + ";";
                        }
                    }
                    EmialAdd = sEmail.Trim(';');
                    #endregion
                }
                else if (dt.Rows[0]["receiverstypeid"].ToString() == "3")
                {
                    #region 配置时，配置了客户和处理人的情况
                    //客户和处理人
                    string sEmail = "";
                    string[] people = receiversIds.Trim().Split(",".ToCharArray());
                    foreach (string ple in people)
                    {
                        //处理人
                        string[] sRecMsgs = ple.Trim().Split("|".ToCharArray());
                        if (sRecMsgs[0] != "")
                        {
                            Epower.DevBase.Organization.SqlDAL.UserEntity user = new UserEntity(long.Parse(sRecMsgs[0]));
                            sEmail += user.Email.ToString() + ";";
                        }
                    }
                    if (sEmail != "" && clientEmail != "")
                    {
                        //客户与处理都非空的情况
                        EmialAdd = clientEmail + ";" + sEmail.Trim(';');
                    }
                    else
                    {
                        if (sEmail != "")
                        {
                            //获取处理人非空，客户为空的情况
                            EmialAdd = sEmail.Trim(';');
                        }
                        else
                        {
                            //客户非空，处理人空的情况
                            EmialAdd = clientEmail;
                        }
                    }
                    #endregion
                }
                #endregion
            }

            return EmialAdd;

        }

        /// <summary>
        /// 获取收件人信息
        /// </summary>
        /// <param name="appid">应用id</param>
        /// <param name="modelid">流程模型id</param>
        /// <param name="clientEmail">客户Email</param>
        /// <param name="receiversIds">提交处理人以逗号隔开的形式的用户id字符串</param>
        /// <returns></returns>
        public static List<UserEntity> getEmaibyNamefunctionV2(long appid, long modelid, string clientEmail, string receiversIds,ref bool isOnlyClient)
        {
            List<UserEntity> list = new List<UserEntity>();
            
            //提交时的接收人情况
            string strSQL = "select receiverstypeid from MailAndMessageRule t where rownum<=1 and t.systemid=" + appid + " and t.modelid =" + modelid + "  and t.status=1 and sendertypeid=1";
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            
            if (dt.Rows.Count > 0)
            {
                #region 获取收件人的 Email
                if (dt.Rows[0]["receiverstypeid"].ToString() == "1")
                {
                    //客户
                    isOnlyClient=true;
                }
                else if (dt.Rows[0]["receiverstypeid"].ToString() == "2")
                {
                    #region 配置时，配置了处理人的情况                   
                    string[] people = receiversIds.Trim().Split(",".ToCharArray());
                    foreach (string ple in people)
                    {
                        //处理人
                        string[] sRecMsgs = ple.Trim().Split("|".ToCharArray());
                        if (sRecMsgs[0] != "")
                        {
                            Epower.DevBase.Organization.SqlDAL.UserEntity user = new UserEntity(long.Parse(sRecMsgs[0]));
                            list.Add(user);
                        }
                    }                    
                    #endregion
                }
                else if (dt.Rows[0]["receiverstypeid"].ToString() == "3")
                {
                    #region 配置时，配置了客户和处理人的情况
                    //客户和处理人                    
                    string[] people = receiversIds.Trim().Split(",".ToCharArray());
                    foreach (string ple in people)
                    {
                        //处理人
                        string[] sRecMsgs = ple.Trim().Split("|".ToCharArray());
                        if (sRecMsgs[0] != "")
                        {
                            Epower.DevBase.Organization.SqlDAL.UserEntity user = new UserEntity(long.Parse(sRecMsgs[0]));
                            list.Add(user);
                        }
                    }
                    isOnlyClient = true;
                    
                    #endregion
                }
                #endregion
            }

            return list;

        }

        #endregion

        #region 获取邮件抄送人员
        /// <summary>
        /// 获取抄送人员的Email地址集合
        /// </summary>
        /// <param name="AppId">应用id </param>
        /// <param name="modelid">流程模型id</param>
        /// <returns>返回抄送人员Email的集合</returns>
        public static string EmailCopyByName(long AppId, long modelid)
        {
            string strSQL = "select ruseridlist from MailAndMessageRule t where rownum<=1 and  t.systemid=" + AppId + " and t.modelid =" + modelid + "  and t.status=1 and sendertypeid=1";
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            string EmialAdd = "";
            if (dt.Rows.Count > 0)
            {
                string recevide = dt.Rows[0]["ruseridlist"].ToString();
                if (recevide != "")
                {
                    string[] arrRecevide = recevide.Trim(',').Split(',');
                    string sEmail = "";
                    foreach (string recevid in arrRecevide)
                    {
                        Epower.DevBase.Organization.SqlDAL.UserEntity user = new UserEntity(long.Parse(recevid));
                        sEmail += user.Email + ";";
                    }
                    EmialAdd = sEmail.Trim(';');
                }
            }
            return EmialAdd;
        }


        /// <summary>
        /// 获取抄送人员的Email地址集合
        /// </summary>
        /// <param name="AppId">应用id </param>
        /// <param name="modelid">流程模型id</param>
        /// <returns>返回抄送人员Email的集合</returns>
        public static List<UserEntity> EmailCopyByNameV2(long AppId, long modelid)
        {
            List<UserEntity> list = new List<UserEntity>();
            string strSQL = "select ruseridlist from MailAndMessageRule t where rownum<=1 and  t.systemid=" + AppId + " and t.modelid =" + modelid + "  and t.status=1 and sendertypeid=1";
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
      
            if (dt.Rows.Count > 0)
            {
                string recevide = dt.Rows[0]["ruseridlist"].ToString();
                if (recevide != "")
                {
                    string[] arrRecevide = recevide.Trim(',').Split(',');              
                    foreach (string recevid in arrRecevide)
                    {
                        Epower.DevBase.Organization.SqlDAL.UserEntity user = new UserEntity(long.Parse(recevid));
                        list.Add(user); 
                    }                  
                }
            }
            return list;
        }

        #endregion

        #region 获得编号和标题
        /// <summary>
        /// 获取编号和标题
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        public static string[] serverNo(long appid, long lngFlowID)
        {

            string[] serverNOs = null;
            string strSql2 = "";
            switch (appid)
            {
                case 1028://发布管理
                    strSql2 = "select '' servicno,f.subject from OA_RELEASEMANAGEMENT a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 201://自定义表单流程                   
                    strSql2 = "select '' servicno,f.subject from App_DefineData a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 400://知识管理                  
                    strSql2 = "select '' servicno,f.subject from Inf_KMBase a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 199://通用流程                   
                    strSql2 = "select '' servicno,f.subject from App_pub_Normal_Head a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 210://问题管理                    
                    strSql2 = "select '' servicno,f.subject from Pro_ProblemDeal a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 410://资产巡检                    
                    strSql2 = "select '' servicno,f.subject from Equ_PatrolService a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 420://变更管理                   
                    strSql2 = "select Changeno as servicno,f.subject from Equ_ChangeService a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 1026://事件管理                    
                    strSql2 = "select nvl(a.buildCode,'') || nvl(a.ServiceNo,'') as servicno,f.subject from Cst_Issues a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                default:
                    break;
            }
            DataTable dt = CommonDP.ExcuteSqlTable(strSql2);
            if (dt.Rows.Count > 0)
            {
                serverNOs = new string[2];
                serverNOs[0] = dt.Rows[0]["servicno"].ToString();
                serverNOs[1] = dt.Rows[0]["subject"].ToString();
            }
            return serverNOs;
        }
        #endregion
       

        #region 事件单发送邮件
        /// <summary>
        /// 事件单发送邮件
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngReceiverID"></param>
        /// <param name="fv"></param>
        /// <returns></returns>
        public static bool EmailNotifyReceiver(OracleTransaction trans, long lngFlowID, long lngReceiverID, FieldValues fv)
        {
            string sEmail = "";
            Epower.DevBase.Organization.SqlDAL.UserEntity user = new UserEntity(lngReceiverID);
            sEmail = user.Email;

            if (sEmail.Trim() != "")
            {
                string sUrlRoot = CommonDP.GetConfigValue("SetMail", "smtpLinkRoot");
                long lngUserID = lngReceiverID;
                TokenIntention ti = new TokenIntention();
                ti.IntentionUrl = "../Forms/frmIssueView.aspx?NewWin=true&FlowID=" + lngFlowID.ToString();
                string sGuid = APTokenDP.SaveTokenInfo(ti.ToXml(), lngReceiverID, lngReceiverID);

                //生成邮件主体 
                string sBody = string.Empty;
                string sUrl = string.Empty;
                string[] stModelContent = MailAndMessageRuleDP.GetTemContent(trans, lngFlowID, 2, 1);
                sBody = stModelContent[5];
                if (sBody != "")
                {
                    sUrl = @"<a href=""" + sUrlRoot + "/Common/frmDoAction.aspx?userid=" + lngUserID.ToString() +
                                  "&guid=" + sGuid + @""" target=""_blank"">";
                    sUrl += sUrlRoot + "/Common/frmDoAction.aspx?guid=" + sGuid + "&userid=" + lngUserID.ToString();
                    sUrl += @"</a>";

                    sBody = sBody.Replace("#?detail?#", sUrl);
                    try
                    {
                        //多线程发送电子邮件
                        MailSendLog(sEmail, sFrom, sBody, "1", long.Parse(stModelContent[0]), stModelContent[1], long.Parse(stModelContent[2]), stModelContent[3]);//发送日志
                        using (MailToolAgent ma = new MailToolAgent(sEmail, "", stModelContent[4], sBody, sSmtpServer, sPsd, sFrom, sSSL, sPort, sUserName))
                        {
                            Thread d = new Thread(new ThreadStart(ma.DoAction));
                            d.Start();
                        }
                        return true;

                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 记录邮件发送日志
        /// <summary>
        /// 记录邮件发送日志
        /// </summary>
        /// <param name="ToMail"></param>
        /// <param name="FromMail"></param>
        /// <param name="Content"></param>
        /// <param name="Status"></param>
        /// <param name="lngSystemID"></param>
        /// <param name="SystemName"></param>
        /// <param name="lngModelID"></param>
        /// <param name="ModelName"></param>
        private static void MailSendLog(string ToMail, string FromMail, string Content, string Status, long lngSystemID, string SystemName, long lngModelID, string ModelName)
        {
            OracleConnection cn = new OracleConnection();
            long lngNextID = EPGlobal.GetNextID("EA_MailSENDID");
            try
            {
                cn = ConfigTool.GetConnection();
                string sSql = "Insert Into EA_MailSEND(ID,ToMail,FromMail,Content,SendTime,Status,SystemID,SystemName,ModelID,ModelName)values(" +
                lngNextID.ToString() + "," +
                StringTool.SqlQ(ToMail) + "," +
                StringTool.SqlQ(FromMail) + "," +
                StringTool.SqlQ(Content) + "," +
                "sysdate" + "," +
                Status.ToString() + "," +
                lngSystemID.ToString() + "," +
                StringTool.SqlQ(SystemName) + "," +
                lngModelID.ToString() + "," +
                StringTool.SqlQ(ModelName) + ")";
                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, sSql);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);                
            }

        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="fv"></param>
        /// <returns></returns>
        public static bool EmailNotifyCust(OracleTransaction trans, long lngFlowID, FieldValues fv)
        {
            string sCustID = fv.GetFieldValue("CustID").Value.Trim();
            if (sCustID == "" || sCustID == "0")
            {
                return false;
            }
            string sEmail = "";
            Br_ECustomerDP ec = new Br_ECustomerDP();
            ec = ec.GetReCorded(long.Parse(sCustID));
            sEmail = ec.Email;

            if (sEmail.Trim() != "")
            {
                string sUrlRoot = CommonDP.GetConfigValue("SetMail", "smtpLinkRoot");
                long lngmnUserID = 1;
                string sSubject = fv.GetFieldValue("Subject").Value.Trim();
                long lngUserID = long.Parse(fv.GetFieldValue("RegSysUserID").Value);
                TokenIntention ti = new TokenIntention();
                ti.IntentionUrl = "../Forms/frmIssueView.aspx?NewWin=true&FlowID=" + lngFlowID.ToString();
                string sGuid = APTokenDP.SaveTokenInfo(ti.ToXml(), lngUserID, lngmnUserID);

                //DataTable dt = GetSendData(trans, lngFlowID);

                //生成邮件主体 
                string sBody = string.Empty;
                string sUrl = string.Empty;
                string[] stModelContent = MailAndMessageRuleDP.GetTemContent(trans, lngFlowID, 1, 1);
                //sBody = MailBodyDeal.GetMailBody("Event_Service.htm", dt);
                sBody = stModelContent[5];
                if (sBody != "")
                {
                    sUrl = @"<a href=""" + sUrlRoot + "/Common/frmDoAction.aspx?userid=" + lngUserID.ToString() +
                                  "&guid=" + sGuid + @""" target=""_blank"">";
                    sUrl += sUrlRoot + "/Common/frmDoAction.aspx?guid=" + sGuid + "&userid=" + lngUserID.ToString();
                    sUrl += @"</a>";

                    sBody = sBody.Replace("#?detail?#", sUrl);
                    //sBody = sBody.Replace("#?Title?#", sSubject);
                    try
                    {
                        //多线程发送电子邮件
                        MailSendLog(sEmail, sFrom, sBody, "1", long.Parse(stModelContent[0]), stModelContent[1], long.Parse(stModelContent[2]), stModelContent[3]);//发送日志
                        using (MailToolAgent ma = new MailToolAgent(sEmail, "", stModelContent[4], sBody, sSmtpServer, sPsd, sFrom, sSSL, sPort, sUserName))
                        {
                            Thread d = new Thread(new ThreadStart(ma.DoAction));
                            d.Start();
                        }
                        return true;

                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        #region 邮件自动回访
        /// <summary>
        /// 邮件自动回访
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="sEmail"></param>
        /// <param name="sCustName"></param>
        /// <param name="strSubject"></param>
        /// <returns></returns>
        public static bool EmailFeedBack(OracleTransaction trans, long lngFlowID, string sEmail, string sCustName, string strSubject)
        {
            if (sEmail.Trim() != "")
            {
                string sUrlRoot = CommonDP.GetConfigValue("SetMail", "smtpLinkRoot");
                long lngmnUserID = 1;
                if (CommonDP.GetConfigValue("Other", "ServiceAutoFeedBackUserID") != null && CommonDP.GetConfigValue("Other", "ServiceAutoFeedBackUserID") != string.Empty)
                    lngmnUserID = long.Parse(CommonDP.GetConfigValue("Other", "ServiceAutoFeedBackUserID"));

                long lngUserID = lngmnUserID;

                DataTable dt = GetSendData(trans, lngFlowID);

                //生成邮件主体 
                string sBody = string.Empty;
                sBody = MailBodyDeal.GetMailBody("FeedBack.htm", dt);

                TokenIntention ti2 = new TokenIntention();
                ti2.IntentionUrl = "../AppForms/frmFeedBack.aspx?FlowID=" + lngFlowID.ToString() + "&CustName=" + sCustName;
                string sGuid2 = APTokenDP.SaveTokenInfo(ti2.ToXml(), lngUserID, lngmnUserID);
                string sFeedBackUrl = string.Empty;
                sFeedBackUrl = @"<a href=""" + sUrlRoot + "/Common/frmDoAction.aspx?userid=" + lngUserID.ToString() +
                              "&guid=" + sGuid2 + @""" target=""_blank"">";
                sFeedBackUrl += sUrlRoot + "/Common/frmDoAction.aspx?guid=" + sGuid2 + "&userid=" + lngUserID.ToString();
                sFeedBackUrl += @"</a>";

                sBody = sBody.Replace("#?feedbackurl?#", sFeedBackUrl);
                try
                {
                    //多线程发送电子邮件
                    using (MailToolAgent ma = new MailToolAgent(sEmail, "", strSubject, sBody, sSmtpServer, sPsd, sFrom, sSSL, sPort, sUserName))
                    {
                        Thread d = new Thread(new ThreadStart(ma.DoAction));
                        d.Start();
                    }
                    return true;

                }
                catch (Exception e)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 邮件回访，不用事务
        /// <summary>
        /// 邮件回访，不用事务
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <param name="sEmail"></param>
        /// <param name="sCustName"></param>
        /// <param name="strSubject"></param>
        /// <returns></returns>
        public static bool EmailFeedBack(long lngFlowID, string sEmail, string sCustName, string strSubject)
        {
            if (sEmail.Trim() != "")
            {
                string sUrlRoot = CommonDP.GetConfigValue("SetMail", "smtpLinkRoot");
                long lngmnUserID = 1;
                if (CommonDP.GetConfigValue("Other", "ServiceAutoFeedBackUserID") != null && CommonDP.GetConfigValue("Other", "ServiceAutoFeedBackUserID") != string.Empty)
                    lngmnUserID = long.Parse(CommonDP.GetConfigValue("Other", "ServiceAutoFeedBackUserID"));

                long lngUserID = lngmnUserID;

                DataTable dt = GetSendData(lngFlowID);

                //生成邮件主体 
                string sBody = string.Empty;
                sBody = MailBodyDeal.GetMailBody("FeedBack.htm", dt);

                TokenIntention ti2 = new TokenIntention();
                ti2.IntentionUrl = "../AppForms/frmFeedBack.aspx?FlowID=" + lngFlowID.ToString() + "&CustName=" + sCustName;
                string sGuid2 = APTokenDP.SaveTokenInfo(ti2.ToXml(), lngUserID, lngmnUserID);
                string sFeedBackUrl = string.Empty;
                sFeedBackUrl = @"<a href=""" + sUrlRoot + "/Common/frmDoAction.aspx?userid=" + lngUserID.ToString() +
                              "&guid=" + sGuid2 + @""" target=""_blank"">";
                sFeedBackUrl += sUrlRoot + "/Common/frmDoAction.aspx?guid=" + sGuid2 + "&userid=" + lngUserID.ToString();
                sFeedBackUrl += @"</a>";

                sBody = sBody.Replace("#?feedbackurl?#", sFeedBackUrl);
                try
                {
                    //多线程发送电子邮件
                    using (MailToolAgent ma = new MailToolAgent(sEmail, "", strSubject, sBody, sSmtpServer, sPsd, sFrom, sSSL, sPort, sUserName))
                    {
                        Thread d = new Thread(new ThreadStart(ma.DoAction));
                        d.Start();
                    }
                    return true;

                }
                catch (Exception e)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 取得服务单回访地址
        /// <summary>
        /// 取得服务单回访地址
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <param name="sCustName"></param>
        /// <returns></returns>
        public static string GetServiceFeedBackUrl(long lngFlowID, string sCustName)
        {
            string sUrlRoot = CommonDP.GetConfigValue("SetMail", "smtpLinkRoot");
            long lngmnUserID = 1;
            if (CommonDP.GetConfigValue("Other", "ServiceAutoFeedBackUserID") != null && CommonDP.GetConfigValue("Other", "ServiceAutoFeedBackUserID") != string.Empty)
                lngmnUserID = long.Parse(CommonDP.GetConfigValue("Other", "ServiceAutoFeedBackUserID"));
            long lngUserID = lngmnUserID;
            TokenIntention ti2 = new TokenIntention();
            ti2.IntentionUrl = "../AppForms/frmFeedBack.aspx?FlowID=" + lngFlowID.ToString() + "&CustName=" + sCustName;
            string sGuid2 = APTokenDP.SaveTokenInfo(ti2.ToXml(), lngUserID, lngmnUserID);
            string sFeedBackUrl = string.Empty;
            sFeedBackUrl = "<a href=\"" + sUrlRoot + "\" target=\"_blank\">";
            sFeedBackUrl += sUrlRoot;
            sFeedBackUrl += @"</a>";
            return sFeedBackUrl;
        }
        #endregion

        #endregion

        #region 邮件及时发送
        /// <summary>
        /// ＷＥＢ页面上发送电子邮件
        /// </summary>
        /// <param name="sRecs"></param>
        /// <param name="sTitle"></param>
        /// <param name="sbody"></param>
        /// <returns></returns>
        public static void WebMailSend(string sRecs, string sTitle, string sBody)
        {

            //邮件标题
            string[] mails = sRecs.Split(";".ToCharArray());
            Regex r = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            for (int i = 0; i < mails.Length; i++)
            {
                string sEmail = mails[i].Trim();

                if (sEmail != "")
                {

                    if (r.Match(sRecs).Success == false)
                    {
                        try
                        {

                            //多线程发送电子邮件
                            using (MailToolAgent ma = new MailToolAgent(sEmail, "", sTitle, sBody, sSmtpServer, sPsd, sFrom, sSSL, sPort, sUserName))
                            {
                                Thread d = new Thread(new ThreadStart(ma.DoAction));
                                d.Start();
                            }

                        }
                        catch (Exception e)
                        {

                        }
                    }
                }
            }
        }

        /// <summary>
        /// 发送电子邮件
        /// </summary>
        /// <param name="sRecs"></param>
        /// <param name="sTitle"></param>
        /// <param name="sbody"></param>
        /// <returns></returns>
        public static void MailSend(string sRecs, string sTitle, string sBody)
        {
            //邮件标题
            string[] mails = sRecs.Split(";".ToCharArray());
            for (int i = 0; i < mails.Length; i++)
            {
                string sEmail = mails[i].Trim();
                if (sEmail != "")
                {
                    try
                    {
                        //多线程发送电子邮件
                        using (MailToolAgent ma = new MailToolAgent(sEmail, "", sTitle, sBody, sSmtpServer, sPsd, sFrom, sSSL, sPort, sUserName))
                        {
                            Thread d = new Thread(new ThreadStart(ma.DoAction));
                            d.Start();
                        }
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sRecs"></param>
        /// <param name="sCC"></param>
        /// <param name="sBcc"></param>
        /// <param name="sTitle"></param>
        /// <param name="sBody"></param>
        public static void MailSend(string sRecs, string sCC, string sBcc, string sTitle, string sBody)
        {
            //邮件标题
            string[] mails = sRecs.Split(";".ToCharArray());
            int count = 0;
            for (int i = 0; i < mails.Length; i++)
            {
                string sEmail = mails[i].Trim();
                if (sEmail != "")
                {
                    try
                    {
                        //多线程发送电子邮件
                        if (count == 0)
                        {
                            using (MailToolAgent ma = new MailToolAgent(sEmail, sCC, sBcc, sTitle, sBody, sSmtpServer, sPsd, sFrom, sSSL, sPort, sUserName))
                            {
                                Thread d = new Thread(new ThreadStart(ma.DoAction));
                                d.Start();
                            }
                        }
                        else
                        {
                            using (MailToolAgent ma = new MailToolAgent(sEmail, "", sTitle, sBody, sSmtpServer, sPsd, sFrom, sSSL, sPort, sUserName))
                            {
                                Thread d = new Thread(new ThreadStart(ma.DoAction));
                                d.Start();
                            }
                        }
                        count++;
                    }
                    catch //(Exception e)
                    {

                    }
                }
            }
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
                case 1028://公告发布管理
                    content = content.Replace("#?信息标题?#", fv.GetFieldValue("TITLE").Value);
                    content = content.Replace("#?信息类别?#", fv.GetFieldValue("TypeName").Value);
                    content = content.Replace("#?发布人?#", fv.GetFieldValue("WRITER").Value);
                    content = content.Replace("#?发布时间?#", fv.GetFieldValue("PUBDATE").Value == null ? "" : fv.GetFieldValue("PUBDATE").Value);
                    content = content.Replace("#?截止时间?#", fv.GetFieldValue("OUTDATE").Value == null ? "" : fv.GetFieldValue("OUTDATE").Value);
                    content = content.Replace("#?具体内容?#", fv.GetFieldValue("CONTENT").Value);
                    break;
                case 201://自定义表单流程
                    break;
                case 400://知识管理                  
                    content = content.Replace("#?主题?#", fv.GetFieldValue("Title").Value).Replace("#?关键字?#", fv.GetFieldValue("Pkey").Value);
                    content = content.Replace("#?摘要?#", fv.GetFieldValue("tags").Value).Replace("#?知识类别?#", fv.GetFieldValue("Type").Value).Replace("#?知识内容?#", fv.GetFieldValue("Content").Value);
                    break;
                case 199://通用流程                   
                    //content += "通用表单固定字段：[#?流程名?#], [#?登记人?#], [#?登记日期?#], [#?登记部门?#] 自定义字段的用法：[#?字段名?#]，将字段名替换为自定义的字段名即可。";
                    content = content.Replace("#?流程名?#", fv.GetFieldValue("flowname").Value);
                    content = content.Replace("#?登记人?#", fv.GetFieldValue("applyname").Value);
                    content = content.Replace("#?登记日期?#", fv.GetFieldValue("startdate").Value);
                    content = content.Replace("#?登记部门?#", fv.GetFieldValue("deptname").Value);

                    long lngOFlowModelID = long.Parse(fv.GetFieldValue("oflowmodelid").Value);
                    List<KeyValuePair<String, String>> listField = AppFieldConfigDP.GetFieldNameAndDisplayName(lngOFlowModelID);

                    foreach (KeyValuePair<String, String> field in listField)
                    {
                        content = content.Replace(field.Value, field.Key)
                                         .Replace(String.Format("#?{0}?#", field.Key), fv.GetFieldValue(field.Key).Value);
                    }

                    break;
                case 210://问题管理
                    content = content.Replace("#?问题单号?#", fv.GetFieldValue("BuildCode").Value + fv.GetFieldValue("ProblemNo").Value);
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
                    content = content.Replace("#?变更单号?#", fv.GetFieldValue("ChangeNo").Value);
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
                    content = content.Replace("#?事件单号?#", fv.GetFieldValue("BuildCode").Value + fv.GetFieldValue("ServiceNo").Value);
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

            //增加链接地址替换
            string sUrlRoot = CommonDP.GetConfigValue("SetMail", "smtpLinkRoot");
            string sUrl = "<a href=\"" + sUrlRoot + "\" target=\"_blank\">";
            sUrl += sUrlRoot;
            sUrl += "</a>";
            content = content.Replace("[连接地址]", sUrl);

            return content;
        }
        #endregion

        #region 根据FlowID，人员的接收类型，发送类型,模板类型得到模板
        /// <summary>
        /// 根据FlowID，人员的接收类型，发送类型,模板类型得到模板
        /// </summary>
        /// <param name="lngFlowID">FlowID</param>
        /// <param name="lngSystemID">AppID</param>
        /// <param name="RTypeID">接收类型()</param>
        /// <param name="STypeID">发送类型()</param>
        /// <param name="iTemType">模板类型（1为邮邮，2为短信）</param>
        /// <returns></returns>
        public static string[] GetTemContent(long lngFlowID, long RTypeID, long STypeID)
        {

            string[] strTempContent = { "0", "", "0", "", "", "", "" };//应用ID，诮用名称，规则ID，规则名称，邮个TITLE，邮件模板，短信模板
            string stFlowModelID = "0";
            OracleConnection cn;
            DataTable dt;
            DataTable dtInfo;
            string strSql2;
            string strSql = "";
            strSql = "select a.appid,a.appname from es_flow f,es_app a where f.appid=a.appid and f.flowid=" + lngFlowID.ToString();
            if (strSql != string.Empty)
            {
                cn = ConfigTool.GetConnection("SQLConnString");
                try { dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSql); }
                finally { ConfigTool.CloseConnection(cn); }

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        strTempContent[0] = dt.Rows[0]["appid"].ToString();
                        strTempContent[1] = dt.Rows[0]["appname"].ToString();
                    }
                }
            }
            strSql = "";
            strSql2 = "";
            switch (long.Parse(strTempContent[0]))
            {
                case 1028://发布管理
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from OA_RELEASEMANAGEMENT where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from OA_RELEASEMANAGEMENT a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 201://自定义表单流程

                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from App_DefineData where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from App_DefineData a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 400://知识管理
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from Inf_KMBase where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from Inf_KMBase a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 199://通用流程
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from App_pub_Normal_Head where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from App_pub_Normal_Head a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 210://问题管理
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from Pro_ProblemDeal where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from Pro_ProblemDeal a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 320://投诉管理
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from CST_BYTS where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from CST_BYTS a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 410://资产巡检
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from Equ_PatrolService where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from Equ_PatrolService a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 420://变更管理
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from Equ_ChangeService where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select Changeno as servicno,f.subject from Equ_ChangeService a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 1026://事件管理
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from Cst_Issues where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select nvl(a.buildCode,'') || nvl(a.ServiceNo,'') as servicno,f.subject from Cst_Issues a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 1027://进出操作间

                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from Flow_QuestHouse where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select ITILNO as servicno,f.subject from Flow_QuestHouse a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                default:
                    break;
            }

            if (strSql != string.Empty)
            {
                cn = ConfigTool.GetConnection("SQLConnString");
                try { dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSql); }
                finally { ConfigTool.CloseConnection(cn); }

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        stFlowModelID = dt.Rows[0]["Oflowmodelid"].ToString();
                    }
                }
            }
            dtInfo = null;
            if (strSql2 != string.Empty)
            {
                cn = ConfigTool.GetConnection("SQLConnString");
                try { dtInfo = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSql2); }
                finally { ConfigTool.CloseConnection(cn); }
            }
            strSql = "SELECT R.ID,R.TEMPLATENAME,R.mailtitle as title,T.mailcontent,T.modelcontent  FROM mailandmessagetemplate T,mailandmessagerule R " +
                     "WHERE T.ID=R.TEMPLATEID AND T.STATUS=1 AND R.DELETED=0 AND R.STATUS=1 AND " +
                     "R.SYSTEMID= " + strTempContent[0] + " AND R.MODELID= " + stFlowModelID + " AND " +
                     "R.receiverstypeid=" + RTypeID.ToString() + " AND sendertypeid=" + STypeID.ToString();

            cn = ConfigTool.GetConnection("SQLConnString");
            try { dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSql); }
            finally { ConfigTool.CloseConnection(cn); }

            if (dt != null && dtInfo != null)
            {
                if (dt.Rows.Count > 0 && dtInfo.Rows.Count > 0)
                {
                    strTempContent[2] = dt.Rows[0]["ID"].ToString();
                    strTempContent[3] = dt.Rows[0]["TEMPLATENAME"].ToString();
                    strTempContent[4] = dt.Rows[0]["title"].ToString();
                    strTempContent[5] = MailBodyDeal.GetMailBody_ByString(dt.Rows[0]["mailcontent"].ToString(), dtInfo);
                    strTempContent[6] = MailBodyDeal.GetMailBody_ByString(dt.Rows[0]["modelcontent"].ToString(), dtInfo);
                }
            }

            return strTempContent;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <param name="rid"></param>
        /// <returns></returns>
        public static string[] GetTemContentbyID(long lngFlowID, long rid)
        {

            string[] strTempContent = { "0", "", "0", "", "", "", "" };//应用ID，诮用名称，规则ID，规则名称，邮个TITLE，邮件模板，短信模板
            string stFlowModelID = "0";
            OracleConnection cn;
            DataTable dt;
            DataTable dtInfo;
            string strSql2;
            string strSql = "";
            strSql = "select a.appid,a.appname from es_flow f,es_app a where f.appid=a.appid and f.flowid=" + lngFlowID.ToString();
            if (strSql != string.Empty)
            {
                cn = ConfigTool.GetConnection("SQLConnString");
                try { dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSql); }
                finally { ConfigTool.CloseConnection(cn); }

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        strTempContent[0] = dt.Rows[0]["appid"].ToString();
                        strTempContent[1] = dt.Rows[0]["appname"].ToString();
                    }
                }
            }
            strSql = "";
            strSql2 = "";
            switch (long.Parse(strTempContent[0]))
            {
                case 1028://发布管理
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from OA_RELEASEMANAGEMENT where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from OA_RELEASEMANAGEMENT a,es_flow f where f.flowid=a.flowid";
                    break;
                case 201://自定义表单流程

                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from App_DefineData where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from App_DefineData a,es_flow f where f.flowid=a.flowid";
                    break;
                case 400://知识管理
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from Inf_KMBase where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from Inf_KMBase a,es_flow f where f.flowid=a.flowid";
                    break;
                case 199://通用流程
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from App_pub_Normal_Head where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from App_pub_Normal_Head a,es_flow f where f.flowid=a.flowid";
                    break;
                case 210://问题管理
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from Pro_ProblemDeal where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from Pro_ProblemDeal a,es_flow f where f.flowid=a.flowid";
                    break;
                case 410://资产巡检
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from Equ_PatrolService where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from Equ_PatrolService a,es_flow f where f.flowid=a.flowid";
                    break;
                case 420://变更管理
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from Equ_ChangeService where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select Changeno as servicno,f.subject from Equ_ChangeService a,es_flow f where f.flowid=a.flowid";
                    break;
                case 1026://事件管理
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from Cst_Issues where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select nvl(a.buildCode,'') || nvl(a.ServiceNo,'') as servicno,f.subject from Cst_Issues a,es_flow f where f.flowid=a.flowid";
                    break;
                default:
                    break;
            }

            if (strSql != string.Empty)
            {
                cn = ConfigTool.GetConnection("SQLConnString");
                try { dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSql); }
                finally { ConfigTool.CloseConnection(cn); }

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        stFlowModelID = dt.Rows[0]["Oflowmodelid"].ToString();
                    }
                }
            }
            dtInfo = null;
            if (strSql2 != string.Empty)
            {
                cn = ConfigTool.GetConnection("SQLConnString");
                try { dtInfo = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSql2); }
                finally { ConfigTool.CloseConnection(cn); }
            }
            strSql = "SELECT R.ID,R.TEMPLATENAME,R.mailtitle as title,T.mailcontent,T.modelcontent  FROM mailandmessagetemplate T,mailandmessagerule R " +
                     "WHERE T.ID=R.TEMPLATEID AND T.STATUS=0 AND R.DELETED=0 AND R.STATUS=0 AND R.ID=" + rid.ToString();

            cn = ConfigTool.GetConnection("SQLConnString");
            try { dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSql); }
            finally { ConfigTool.CloseConnection(cn); }

            if (dt != null && dtInfo != null)
            {
                if (dt.Rows.Count > 0 && dtInfo.Rows.Count > 0)
                {
                    strTempContent[2] = dt.Rows[0]["ID"].ToString();
                    strTempContent[3] = dt.Rows[0]["TEMPLATENAME"].ToString();
                    strTempContent[4] = dt.Rows[0]["title"].ToString();
                    strTempContent[5] = MailBodyDeal.GetMailBody_ByString(dt.Rows[0]["mailcontent"].ToString(), dtInfo);
                    strTempContent[6] = MailBodyDeal.GetMailBody_ByString(dt.Rows[0]["modelcontent"].ToString(), dtInfo);
                }
            }

            return strTempContent;
        }
    }
}
