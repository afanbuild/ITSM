/****************************************************************************
 * 
 * description:�ʼ�����
 * 
 * 
 * 
 * Create by:������
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

        #region ֪ʶ����
        /// <summary>
        /// ֪ʶ����ʱ�����ʼ�
        /// </summary>
        /// <param name="lngKBID"></param>
        /// <param name="lngCommentUserID"></param>
        /// <param name="CommentUserName"></param>
        /// <returns></returns>
        public static bool InfCommentSend(long lngKBID, long lngCommentUserID, string CommentUserName)
        {
            DataTable dt;
            string sSql = @"select a.ID,Score,Title,Pkey,Tags,TypeName,CreationName,UpdateTime,RegUserID,RegUserName,Content,
                                Case IsShow when 0 then '��'  else '��' end IsShow
                                from Inf_Information a,Inf_Score b 
                                where a.ID=b.KBID and b.UserID=" + lngCommentUserID.ToString() + " And b.KBID=" + lngKBID.ToString();
            dt = CommonDP.ExcuteSqlTable(sSql);

            UserEntity pUserEntity = new UserEntity(long.Parse(dt.Rows[0]["RegUserID"].ToString()));
            string sEmail = pUserEntity.Email.Trim();
            if (sEmail == string.Empty)  //����ռ��˲�����,�򲻷��� 
                return false;


            //�ʼ�����
            string sSubject = CommentUserName + ":������֪ʶ����!";
            //�����ʼ����� 
            string sBody = string.Empty;
            sBody = MailBodyDeal.GetMailBody("Inf_Comment.htm", dt);
            try
            {
                //���̷߳��͵����ʼ�
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
        /// ֪ʶ�Ƽ���������ʱ�����ʼ�
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        public static bool InfReCommendSend(OracleTransaction trans, long lngFlowID)
        {
            DataTable dt;
            string sSql = @"select KBID,Title,Pkey,Tags,TypeName,CreationName,RegTime,RegUserName,Content,
                                nvl(ReCommendUserID,0) ReCommendUserID,ReCommendUserName,
	                            Case IsInKB when 1 then '������ͨ������⣡'  when 0 then 'δ����ͨ����' end Pass,
                                Case IsShow when 0 then '��'  else '��' end IsShow
                                from Inf_KMBase
                                where FlowID=" + lngFlowID.ToString();
            dt = CommonDP.ExcuteSqlTable(trans, sSql);

            UserEntity pUserEntity = new UserEntity(long.Parse(dt.Rows[0]["ReCommendUserID"].ToString()));
            string sEmail = pUserEntity.Email.Trim();
            if (sEmail == string.Empty)  //����ռ��˲�����,�򲻷��� 
                return false;


            //�ʼ�����
            string sSubject = "���Ƽ���֪ʶ��" + dt.Rows[0]["Pass"].ToString();
            //�����ʼ����� 
            string sBody = string.Empty;
            sBody = MailBodyDeal.GetMailBody("Inf_ReCommend.htm", dt);
            try
            {
                //���̷߳��͵����ʼ�
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

        #region �¼�����
        /// <summary>
        ///  ȡ������
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
        ///  ȡ������
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

        #region �ʼ�ģ�干�еķ��ͷ�ʽ       


        /// <summary>
        /// �ƺ�ֻ�����¼�������˵��wxh
        /// �����������ʼ����߼����򣬽��˹��ܸĳ�ͨ��ģ��,�����ʼ�������ڷ��� yxq 2014-07-15
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
            string getAppName = "";//Ӧ������
            long lngAppID = 0;//Ӧ��id
            string flowname = ""; //��������
            long getEmailAppModelId = MailAndMessageRuleDP.GetOFlowModelId(lngFlowModelId, ref lngAppID, ref flowname);//�������ģ�͵�ģ��id


            if (getEmailAppModelId > 0)
            {

                getAppName = MailAndMessageRuleDP.GetAppName(lngAppID);

                long nodemodelid = GetNodeModeID(trans, lngFlowModelId, lngMessageId); //��û���ID

                #region ���ݻ������ù����ʼ�

                //�ж��Ƿ����������л�������
                string strSQL1 = @" select * from Br_Messagerulinstall where oflowmodelid=" + getEmailAppModelId + " and nodeid=-1 and trigger_type= '����'";
                DataTable dtallNode = OracleDbHelper.ExecuteDataset(trans, CommandType.Text, strSQL1).Tables[0];

                //�������˷��ʼ�
                if (dtallNode.Rows.Count == 1)
                {
                    SendEmailByNodeModelV2(fv, receiverArray, nodemodelid, getEmailAppModelId, lngAppID, flowname, lngFlag, getAppName, dtallNode);
                }
                else  //���ݻ��ڷ��ʼ�
                {
                    //���ݻ���id������ģ��id�ڹ�������ҵ���Ӧ�ļ�¼
                    string strSQL3 = @" select * from Br_Messagerulinstall where rownum=1 and
                 oflowmodelid=" + getEmailAppModelId + " and nodeid=" + nodemodelid + " and trigger_type = '����'";

                    //���ݲ��ҵļ�¼��ȡ�������ͣ��������ʼ�ģ��
                    DataTable dtEmail = OracleDbHelper.ExecuteDataset(trans, CommandType.Text, strSQL3).Tables[0];

                    if (dtEmail.Rows.Count > 0)
                    {
                        SendEmailByNodeModelV2(fv, receiverArray, nodemodelid, getEmailAppModelId, lngAppID, flowname, lngFlag, getAppName, dtEmail);

                    }

                }

                #endregion
            }

        }

        #region ��ȡ����ģ��ID
        /// <summary>
        /// ��ȡ����ģ��ID
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowModelId"></param>
        /// <param name="lngMessageId"></param>
        /// <returns>nodemodelid</returns>
        public static long GetNodeModeID(OracleTransaction trans, long lngFlowModelId, long lngMessageId)
        {
            //����lngMessageID�ҵ���Ӧ�Ļ���ID
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

            //��es_node�и���node�ҵ�����id
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

        #region ��ȡ������Email
        /// <summary>
        /// ��ȡ������Email
        /// </summary>
        /// <param name="userids">�û�ID</param>
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
     

        #region ���ݻ��ڷ��ʼ�,�ֿ�����
        /// <summary>
        /// ���ݻ��ڷ��ʼ�,�ֿ�����
        /// </summary>
        /// <param name="fv">ҳ�������XML��</param>
        /// <param name="receiverArray">�������б�</param>
        /// <param name="nodemodelid">��ǰ����ģ��ID</param>
        /// <param name="getEmailAppModelId">����OFlowModelID</param>
        /// <param name="lngAppID">Ӧ��ID</param>
        /// <param name="flowname">��������</param>
        /// <param name="lngFlag">�Ƿ��ͳ�����Ա 0�� 1��</param>
        /// <param name="getAppName">Ӧ������</param>
        /// <param name="dt"></param>
        public static void SendEmailByNodeModelV2(FieldValues fv, string receiverArray, long nodemodelid, long getEmailAppModelId, long lngAppID, string flowname, long lngFlag, string getAppName, DataTable dt)
        {
            //ͨ�������滻����ʱ��Ҫʹ��,��ʱ�洢
            fv.Add("oflowmodelid", getEmailAppModelId.ToString());

            string getEmailByName = "";//��ȡ�ռ��˵�Email ������ʽA
            string getEmailCopyByName = "";//��ȡ�����˵�Email ������ʽ
            string getTitle = "";//��ȡ�ʼ�����           

            #region ѭ�������ʼ������е�����

            foreach (DataRow dr in dt.Rows)
            {
                string recervicesType = "";  //��������
                string flowModelName = "";   //���ڹ���ģ��
                string flownameid = "";//���ڹ���ģ��ID
                string nodeContent = "";    //����ģ�����
                string userids = "";        //������ID
                string triggertype = "";//��������
                string nodename = "";//����

                recervicesType = dr["ReceiversTypeName"].ToString();
                flowModelName = dr["FlowName"].ToString();
                flownameid = dr["FlowNameID"].ToString();
                nodeContent = dr["NodeContent"].ToString();
                nodemodelid = long.Parse(dr["NodeID"].ToString());
                userids = dr["RECIPIENT_USERID"].ToString();
                triggertype = dr["trigger_type"].ToString();
                nodename = dr["nodename"].ToString();

                
                DataTable dt_emailmodel = GetoneEmailModel(flownameid);//��ȡ�ʼ�ģ��

                #region ���������б��ʼ�
                //���������б��ʼ�
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

                #region ��ȡ��Ҫ�����˵����估��Ӧ���õ��ʼ�����

                List<UserEntity> extList = new List<UserEntity>();
                List<UserEntity> extListCC = new List<UserEntity>();

                string temp1=string.Empty;
                string temp2=string.Empty;
                string strClientEmail = string.Empty;

                //�������û�ȡ��Ҫ�����ʼ�����Ա�����б�
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
                    getTitle = nodeContent;  //�ʼ�����

                    //if (lngFlag > 0)
                    //{
                    //    //��ȡ������Ա�б� ���Ͷ��ŵĵط��õ����ʼ����ڻ�ȡ����Ա�б�
                    //    extListCC = EmailCopyByNameV2(lngAppID, getEmailAppModelId);
                    //}                    

                    //�滻�����Ĳ�ͬ��ֵ                
                    string sBody = "";

                    try
                    {
                        //���ϱ��˷��ʼ�
                        if (recervicesType.Contains("�ͻ�") == true && strClientEmail!=string.Empty)
                        {
                            if (MailTool.IsEmail(strClientEmail))
                            {
                                sBody = temp1;
                                //�滻�����Ĳ�ͬ��ֵ
                                sBody = getContent(lngAppID, sBody, fv);                                

                                Thread(strClientEmail, "", getTitle, sFrom, sBody, lngAppID, getAppName, getEmailAppModelId, flowname, triggertype, nodename);
                            }
                        }

                        //�������˷��ʼ�
                        foreach (UserEntity user in extList)
                        {
                            if (MailTool.IsEmail(user.Email))
                            {
                                sBody = temp2;
                                //�滻�����Ĳ�ͬ��ֵ
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

        #region �õ��¼�����Ϣ
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

        #region ƴдEmail
        public static string PEmail(string sbody, string cstno, DataRow dr_Cst)
        {
            sbody = sbody.Replace("#?�¼�����?#", cstno);
            sbody = sbody.Replace("#?�ͻ�����?#", dr_Cst["CustName"].ToString());
            sbody = sbody.Replace("#?�ͻ��ֻ�?#", dr_Cst["CTel"].ToString());
            sbody = sbody.Replace("#?�ͻ��ʼ�?#", dr_Cst["Email"].ToString());
            sbody = sbody.Replace("#?�Ǽ�ʱ��?#", dr_Cst["RegSysDate"].ToString());
            sbody = sbody.Replace("#?����ʱ��?#", dr_Cst["CustTime"].ToString());
            sbody = sbody.Replace("#?����ʱ��?#", dr_Cst["reportingtime"].ToString());
            sbody = sbody.Replace("#?�¼����?#", dr_Cst["servicetype"].ToString());
            sbody = sbody.Replace("#?���񼶱�?#", dr_Cst["servicelevel"].ToString());
            sbody = sbody.Replace("#?Ӱ�췶Χ?#", dr_Cst["effectname"].ToString());
            sbody = sbody.Replace("#?������?#", dr_Cst["InstancyName"].ToString());
            sbody = sbody.Replace("#?ժҪ?#", dr_Cst["subject"].ToString());
            sbody = sbody.Replace("#?��ϸ����?#", dr_Cst["content"].ToString());

            return sbody;
        }
        #endregion

        #region �õ��ʼ�ģ��
        /// <summary>
        /// ��ȡ�ʼ�ģ��
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

        #region �õ��ʼ�ģ��
        /// <summary>
        /// ��ȡ�ʼ�ģ��
        /// </summary>
        /// <returns></returns>
        public static DataTable GetoneEmailModel(string flownameid)
        {
            return MailSendDeal.GetoneEmailModel(flownameid, 0);
        }
        #endregion

        #region  ����FlowID��ѯ����Ӧ�ĵ�ǰ������
        /// <summary>
        /// ����flowid��ѯ��ǰ�����˵�email
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
            
            MailSendLog(getEmailByName + "," + getEmailCopyByName, sFrom, sBody, "1", lngAppID, getAppName, getEmailAppModelId, flowname);//������־
            using (MailToolAgent ma = new MailToolAgent(getEmailByName, getEmailCopyByName, getTitle, sBody, sSmtpServer, sPsd, sFrom, sSSL, sPort, sUserName, smtpdisplayName, true))
            {
                Thread d = new Thread(new ThreadStart(ma.DoAction));
                d.Start();               
            }

        }
        #endregion        

        /// <summary>
        /// ��ȡ�ʼ���������Ϣ upt by wxh
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
            if (recerviceType == "������")
            {
                #region ����ʱ�������˴����˵����               
                string[] people = receiversIds.Trim().Split(",".ToCharArray());
                foreach (string ple in people)
                {
                    //������
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
            else if (recerviceType == "�ͻ�")
            { 
                temp1 = arrTitle_content[1];
            }
            else if (recerviceType == "�ͻ��ʹ�����")
            {      

                #region ����ʱ�������˿ͻ��ʹ����˵����
                string[] people = receiversIds.Trim().Split(",".ToCharArray());
                foreach (string ple in people)
                {
                    //������
                    string[] sRecMsgs = ple.Trim().Split("|".ToCharArray());
                    if (sRecMsgs[0] != "")
                    {
                        Epower.DevBase.Organization.SqlDAL.UserEntity user = new UserEntity(long.Parse(sRecMsgs[0]));   
                        list.Add(user);
                    }
                }                

                temp1 = arrTitle_content[1].ToString(); //�ϱ����ʼ�ģ��
                temp2 = arrTitle_content[3].ToString(); //�������ʼ�ģ��

                #endregion
            }
            return list;
        }

        
        #region ����ʼ�ģ��ı��������
        /// <summary>
        /// ���ģ���еı��� �� ģ������
        /// </summary>
        /// <param name="appid">Ӧ��id</param>
        /// <param name="flowmodelid">����ģ��id</param>
        /// <returns>���ر����ģ������</returns>
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
                values[1] = dt.Rows[0]["mailcontent"].ToString(); //�ϱ���ģ��
                values[2] = dt.Rows[0]["remark"].ToString();
                values[3] = dt.Rows[0]["Dealmailcontent"].ToString();  //������ģ��
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
                values[1] = dt.Rows[0]["mailcontent"].ToString(); //�ϱ���ģ��
                values[2] = dt.Rows[0]["remark"].ToString();
                values[3] = dt.Rows[0]["Dealmailcontent"].ToString();  //������ģ��
            }
            return values;
        }

        //���ر�����ϱ�������


        #endregion

        #region ��ȡ�ʼ������˵� Email
        /// <summary>
        /// ��ȡ�ռ�����Ϣ
        /// </summary>
        /// <param name="appid">Ӧ��id</param>
        /// <param name="modelid">����ģ��id</param>
        /// <param name="clientEmail">�ͻ�Email</param>
        /// <param name="receiversIds">�ύ�������Զ��Ÿ�������ʽ���û�id�ַ���</param>
        /// <returns></returns>
        public static string getEmaibyNamefunction(long appid, long modelid, string clientEmail, string receiversIds)
        {

            //sendertypeid=1Ϊ�ύʱ���Ͷ���
            //�ύʱ�Ľ��������
            string strSQL = "select receiverstypeid from MailAndMessageRule t where rownum<=1 and t.systemid=" + appid + " and t.modelid =" + modelid + "  and t.status=1 and sendertypeid=1";
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            string EmialAdd = "";
            if (dt.Rows.Count > 0)
            {
                #region ��ȡ�ռ��˵� Email
                if (dt.Rows[0]["receiverstypeid"].ToString() == "1")
                {
                    //�ͻ�
                    EmialAdd = clientEmail;
                }
                else if (dt.Rows[0]["receiverstypeid"].ToString() == "2")
                {
                    #region ����ʱ�������˴����˵����
                    string sEmail = "";
                    string[] people = receiversIds.Trim().Split(",".ToCharArray());
                    foreach (string ple in people)
                    {
                        //������
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
                    #region ����ʱ�������˿ͻ��ʹ����˵����
                    //�ͻ��ʹ�����
                    string sEmail = "";
                    string[] people = receiversIds.Trim().Split(",".ToCharArray());
                    foreach (string ple in people)
                    {
                        //������
                        string[] sRecMsgs = ple.Trim().Split("|".ToCharArray());
                        if (sRecMsgs[0] != "")
                        {
                            Epower.DevBase.Organization.SqlDAL.UserEntity user = new UserEntity(long.Parse(sRecMsgs[0]));
                            sEmail += user.Email.ToString() + ";";
                        }
                    }
                    if (sEmail != "" && clientEmail != "")
                    {
                        //�ͻ��봦���ǿյ����
                        EmialAdd = clientEmail + ";" + sEmail.Trim(';');
                    }
                    else
                    {
                        if (sEmail != "")
                        {
                            //��ȡ�����˷ǿգ��ͻ�Ϊ�յ����
                            EmialAdd = sEmail.Trim(';');
                        }
                        else
                        {
                            //�ͻ��ǿգ������˿յ����
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
        /// ��ȡ�ռ�����Ϣ
        /// </summary>
        /// <param name="appid">Ӧ��id</param>
        /// <param name="modelid">����ģ��id</param>
        /// <param name="clientEmail">�ͻ�Email</param>
        /// <param name="receiversIds">�ύ�������Զ��Ÿ�������ʽ���û�id�ַ���</param>
        /// <returns></returns>
        public static List<UserEntity> getEmaibyNamefunctionV2(long appid, long modelid, string clientEmail, string receiversIds,ref bool isOnlyClient)
        {
            List<UserEntity> list = new List<UserEntity>();
            
            //�ύʱ�Ľ��������
            string strSQL = "select receiverstypeid from MailAndMessageRule t where rownum<=1 and t.systemid=" + appid + " and t.modelid =" + modelid + "  and t.status=1 and sendertypeid=1";
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            
            if (dt.Rows.Count > 0)
            {
                #region ��ȡ�ռ��˵� Email
                if (dt.Rows[0]["receiverstypeid"].ToString() == "1")
                {
                    //�ͻ�
                    isOnlyClient=true;
                }
                else if (dt.Rows[0]["receiverstypeid"].ToString() == "2")
                {
                    #region ����ʱ�������˴����˵����                   
                    string[] people = receiversIds.Trim().Split(",".ToCharArray());
                    foreach (string ple in people)
                    {
                        //������
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
                    #region ����ʱ�������˿ͻ��ʹ����˵����
                    //�ͻ��ʹ�����                    
                    string[] people = receiversIds.Trim().Split(",".ToCharArray());
                    foreach (string ple in people)
                    {
                        //������
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

        #region ��ȡ�ʼ�������Ա
        /// <summary>
        /// ��ȡ������Ա��Email��ַ����
        /// </summary>
        /// <param name="AppId">Ӧ��id </param>
        /// <param name="modelid">����ģ��id</param>
        /// <returns>���س�����ԱEmail�ļ���</returns>
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
        /// ��ȡ������Ա��Email��ַ����
        /// </summary>
        /// <param name="AppId">Ӧ��id </param>
        /// <param name="modelid">����ģ��id</param>
        /// <returns>���س�����ԱEmail�ļ���</returns>
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

        #region ��ñ�źͱ���
        /// <summary>
        /// ��ȡ��źͱ���
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
                case 1028://��������
                    strSql2 = "select '' servicno,f.subject from OA_RELEASEMANAGEMENT a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 201://�Զ��������                   
                    strSql2 = "select '' servicno,f.subject from App_DefineData a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 400://֪ʶ����                  
                    strSql2 = "select '' servicno,f.subject from Inf_KMBase a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 199://ͨ������                   
                    strSql2 = "select '' servicno,f.subject from App_pub_Normal_Head a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 210://�������                    
                    strSql2 = "select '' servicno,f.subject from Pro_ProblemDeal a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 410://�ʲ�Ѳ��                    
                    strSql2 = "select '' servicno,f.subject from Equ_PatrolService a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 420://�������                   
                    strSql2 = "select Changeno as servicno,f.subject from Equ_ChangeService a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 1026://�¼�����                    
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
       

        #region �¼��������ʼ�
        /// <summary>
        /// �¼��������ʼ�
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

                //�����ʼ����� 
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
                        //���̷߳��͵����ʼ�
                        MailSendLog(sEmail, sFrom, sBody, "1", long.Parse(stModelContent[0]), stModelContent[1], long.Parse(stModelContent[2]), stModelContent[3]);//������־
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

        #region ��¼�ʼ�������־
        /// <summary>
        /// ��¼�ʼ�������־
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

                //�����ʼ����� 
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
                        //���̷߳��͵����ʼ�
                        MailSendLog(sEmail, sFrom, sBody, "1", long.Parse(stModelContent[0]), stModelContent[1], long.Parse(stModelContent[2]), stModelContent[3]);//������־
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

        #region �ʼ��Զ��ط�
        /// <summary>
        /// �ʼ��Զ��ط�
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

                //�����ʼ����� 
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
                    //���̷߳��͵����ʼ�
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

        #region �ʼ��طã���������
        /// <summary>
        /// �ʼ��طã���������
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

                //�����ʼ����� 
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
                    //���̷߳��͵����ʼ�
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

        #region ȡ�÷��񵥻طõ�ַ
        /// <summary>
        /// ȡ�÷��񵥻طõ�ַ
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

        #region �ʼ���ʱ����
        /// <summary>
        /// �ףţ�ҳ���Ϸ��͵����ʼ�
        /// </summary>
        /// <param name="sRecs"></param>
        /// <param name="sTitle"></param>
        /// <param name="sbody"></param>
        /// <returns></returns>
        public static void WebMailSend(string sRecs, string sTitle, string sBody)
        {

            //�ʼ�����
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

                            //���̷߳��͵����ʼ�
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
        /// ���͵����ʼ�
        /// </summary>
        /// <param name="sRecs"></param>
        /// <param name="sTitle"></param>
        /// <param name="sbody"></param>
        /// <returns></returns>
        public static void MailSend(string sRecs, string sTitle, string sBody)
        {
            //�ʼ�����
            string[] mails = sRecs.Split(";".ToCharArray());
            for (int i = 0; i < mails.Length; i++)
            {
                string sEmail = mails[i].Trim();
                if (sEmail != "")
                {
                    try
                    {
                        //���̷߳��͵����ʼ�
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
            //�ʼ�����
            string[] mails = sRecs.Split(";".ToCharArray());
            int count = 0;
            for (int i = 0; i < mails.Length; i++)
            {
                string sEmail = mails[i].Trim();
                if (sEmail != "")
                {
                    try
                    {
                        //���̷߳��͵����ʼ�
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

        #region ��ȡ������ͬ����Ϣ������
        /// <summary>
        /// ��ȡ������ͬ����Ϣ������
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="content"></param>
        /// <param name="fv"></param>
        /// <returns></returns>
        public static string getContent(long appId, string content, FieldValues fv)
        {
            switch (appId)
            {
                case 1028://���淢������
                    content = content.Replace("#?��Ϣ����?#", fv.GetFieldValue("TITLE").Value);
                    content = content.Replace("#?��Ϣ���?#", fv.GetFieldValue("TypeName").Value);
                    content = content.Replace("#?������?#", fv.GetFieldValue("WRITER").Value);
                    content = content.Replace("#?����ʱ��?#", fv.GetFieldValue("PUBDATE").Value == null ? "" : fv.GetFieldValue("PUBDATE").Value);
                    content = content.Replace("#?��ֹʱ��?#", fv.GetFieldValue("OUTDATE").Value == null ? "" : fv.GetFieldValue("OUTDATE").Value);
                    content = content.Replace("#?��������?#", fv.GetFieldValue("CONTENT").Value);
                    break;
                case 201://�Զ��������
                    break;
                case 400://֪ʶ����                  
                    content = content.Replace("#?����?#", fv.GetFieldValue("Title").Value).Replace("#?�ؼ���?#", fv.GetFieldValue("Pkey").Value);
                    content = content.Replace("#?ժҪ?#", fv.GetFieldValue("tags").Value).Replace("#?֪ʶ���?#", fv.GetFieldValue("Type").Value).Replace("#?֪ʶ����?#", fv.GetFieldValue("Content").Value);
                    break;
                case 199://ͨ������                   
                    //content += "ͨ�ñ��̶��ֶΣ�[#?������?#], [#?�Ǽ���?#], [#?�Ǽ�����?#], [#?�Ǽǲ���?#] �Զ����ֶε��÷���[#?�ֶ���?#]�����ֶ����滻Ϊ�Զ�����ֶ������ɡ�";
                    content = content.Replace("#?������?#", fv.GetFieldValue("flowname").Value);
                    content = content.Replace("#?�Ǽ���?#", fv.GetFieldValue("applyname").Value);
                    content = content.Replace("#?�Ǽ�����?#", fv.GetFieldValue("startdate").Value);
                    content = content.Replace("#?�Ǽǲ���?#", fv.GetFieldValue("deptname").Value);

                    long lngOFlowModelID = long.Parse(fv.GetFieldValue("oflowmodelid").Value);
                    List<KeyValuePair<String, String>> listField = AppFieldConfigDP.GetFieldNameAndDisplayName(lngOFlowModelID);

                    foreach (KeyValuePair<String, String> field in listField)
                    {
                        content = content.Replace(field.Value, field.Key)
                                         .Replace(String.Format("#?{0}?#", field.Key), fv.GetFieldValue(field.Key).Value);
                    }

                    break;
                case 210://�������
                    content = content.Replace("#?���ⵥ��?#", fv.GetFieldValue("BuildCode").Value + fv.GetFieldValue("ProblemNo").Value);
                    content = content.Replace("#?�Ǽ���?#", fv.GetFieldValue("RegUserName").Value);
                    content = content.Replace("#?�Ǽ��˲���?#", fv.GetFieldValue("RegDeptName").Value);
                    content = content.Replace("#?�Ǽ�ʱ��?#", fv.GetFieldValue("RegTime").Value);
                    content = content.Replace("#?Ӱ���?#", fv.GetFieldValue("EffectName").Value);
                    content = content.Replace("#?������?#", fv.GetFieldValue("InstancyName").Value);
                    content = content.Replace("#?����?#", fv.GetFieldValue("Problem_Title").Value);
                    content = content.Replace("#?��������?#", fv.GetFieldValue("Problem_Subject").Value);
                    content = content.Replace("#?����״̬?#", fv.GetFieldValue("StateName").Value);
                    content = content.Replace("#?�������?#", fv.GetFieldValue("Remark").Value);
                    break;
                case 410://�ʲ�Ѳ�� 
                    content = content.Replace("#?����?#", fv.GetFieldValue("Title").Value);
                    content = content.Replace("#?�Ǽ���?#", fv.GetFieldValue("RegUserName").Value);
                    content = content.Replace("#?�Ǽǲ���?#", fv.GetFieldValue("RegDeptName").Value);
                    content = content.Replace("#?�Ǽ�ʱ��?#", fv.GetFieldValue("RegTime").Value);
                    content = content.Replace("#?��ע?#", fv.GetFieldValue("Remark").Value);
                    break;
                case 420://�������
                    content = content.Replace("#?�������?#", fv.GetFieldValue("ChangeNo").Value);
                    content = content.Replace("#?�ͻ�����?#", fv.GetFieldValue("CustName").Value);
                    content = content.Replace("#?�ͻ��绰?#", fv.GetFieldValue("CTel").Value);
                    content = content.Replace("#?�������?#", fv.GetFieldValue("LevelName").Value);
                    content = content.Replace("#?���״̬?#", fv.GetFieldValue("DealStatus").Value);
                    content = content.Replace("#?Ӱ���?#", fv.GetFieldValue("EffectName").Value);
                    content = content.Replace("#?ժҪ?#", fv.GetFieldValue("Subject").Value);
                    content = content.Replace("#?��������?#", fv.GetFieldValue("Content").Value);
                    content = content.Replace("#?������?#", fv.GetFieldValue("InstancyName").Value);
                    break;
                case 1026://�¼�����
                    content = content.Replace("#?�¼�����?#", fv.GetFieldValue("BuildCode").Value + fv.GetFieldValue("ServiceNo").Value);
                    content = content.Replace("#?�ͻ�����?#", fv.GetFieldValue("CustName").Value);
                    content = content.Replace("#?�ͻ��ֻ�?#", fv.GetFieldValue("CTel").Value);
                    content = content.Replace("#?�ͻ��ʼ�?#", fv.GetFieldValue("Email").Value);
                    content = content.Replace("#?�Ǽ�ʱ��?#", fv.GetFieldValue("RegSysDate").Value);
                    content = content.Replace("#?����ʱ��?#", fv.GetFieldValue("CustTime").Value == null ? "" : fv.GetFieldValue("CustTime").Value);
                    content = content.Replace("#?����ʱ��?#", fv.GetFieldValue("ReportingTime").Value == null ? "" : fv.GetFieldValue("ReportingTime").Value);
                    content = content.Replace("#?�¼����?#", fv.GetFieldValue("ServiceType").Value);
                    content = content.Replace("#?���񼶱�?#", fv.GetFieldValue("ServiceLevel").Value);
                    content = content.Replace("#?Ӱ�췶Χ?#", fv.GetFieldValue("EffectName").Value);
                    content = content.Replace("#?������?#", fv.GetFieldValue("InstancyName").Value);
                    content = content.Replace("#?ժҪ?#", fv.GetFieldValue("Subject").Value);
                    content = content.Replace("#?��ϸ����?#", fv.GetFieldValue("Content").Value);
                    break;
                default:
                    break;
            }

            //�������ӵ�ַ�滻
            string sUrlRoot = CommonDP.GetConfigValue("SetMail", "smtpLinkRoot");
            string sUrl = "<a href=\"" + sUrlRoot + "\" target=\"_blank\">";
            sUrl += sUrlRoot;
            sUrl += "</a>";
            content = content.Replace("[���ӵ�ַ]", sUrl);

            return content;
        }
        #endregion

        #region ����FlowID����Ա�Ľ������ͣ���������,ģ�����͵õ�ģ��
        /// <summary>
        /// ����FlowID����Ա�Ľ������ͣ���������,ģ�����͵õ�ģ��
        /// </summary>
        /// <param name="lngFlowID">FlowID</param>
        /// <param name="lngSystemID">AppID</param>
        /// <param name="RTypeID">��������()</param>
        /// <param name="STypeID">��������()</param>
        /// <param name="iTemType">ģ�����ͣ�1Ϊ���ʣ�2Ϊ���ţ�</param>
        /// <returns></returns>
        public static string[] GetTemContent(long lngFlowID, long RTypeID, long STypeID)
        {

            string[] strTempContent = { "0", "", "0", "", "", "", "" };//Ӧ��ID��ڽ�����ƣ�����ID���������ƣ��ʸ�TITLE���ʼ�ģ�壬����ģ��
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
                case 1028://��������
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from OA_RELEASEMANAGEMENT where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from OA_RELEASEMANAGEMENT a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 201://�Զ��������

                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from App_DefineData where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from App_DefineData a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 400://֪ʶ����
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from Inf_KMBase where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from Inf_KMBase a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 199://ͨ������
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from App_pub_Normal_Head where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from App_pub_Normal_Head a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 210://�������
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from Pro_ProblemDeal where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from Pro_ProblemDeal a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 320://Ͷ�߹���
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from CST_BYTS where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from CST_BYTS a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 410://�ʲ�Ѳ��
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from Equ_PatrolService where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from Equ_PatrolService a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 420://�������
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from Equ_ChangeService where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select Changeno as servicno,f.subject from Equ_ChangeService a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 1026://�¼�����
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from Cst_Issues where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select nvl(a.buildCode,'') || nvl(a.ServiceNo,'') as servicno,f.subject from Cst_Issues a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 1027://����������

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

            string[] strTempContent = { "0", "", "0", "", "", "", "" };//Ӧ��ID��ڽ�����ƣ�����ID���������ƣ��ʸ�TITLE���ʼ�ģ�壬����ģ��
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
                case 1028://��������
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from OA_RELEASEMANAGEMENT where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from OA_RELEASEMANAGEMENT a,es_flow f where f.flowid=a.flowid";
                    break;
                case 201://�Զ��������

                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from App_DefineData where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from App_DefineData a,es_flow f where f.flowid=a.flowid";
                    break;
                case 400://֪ʶ����
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from Inf_KMBase where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from Inf_KMBase a,es_flow f where f.flowid=a.flowid";
                    break;
                case 199://ͨ������
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from App_pub_Normal_Head where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from App_pub_Normal_Head a,es_flow f where f.flowid=a.flowid";
                    break;
                case 210://�������
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from Pro_ProblemDeal where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from Pro_ProblemDeal a,es_flow f where f.flowid=a.flowid";
                    break;
                case 410://�ʲ�Ѳ��
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from Equ_PatrolService where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from Equ_PatrolService a,es_flow f where f.flowid=a.flowid";
                    break;
                case 420://�������
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from Equ_ChangeService where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select Changeno as servicno,f.subject from Equ_ChangeService a,es_flow f where f.flowid=a.flowid";
                    break;
                case 1026://�¼�����
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
