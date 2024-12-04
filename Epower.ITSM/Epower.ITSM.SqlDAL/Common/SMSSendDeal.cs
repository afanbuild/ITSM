/****************************************************************************
 * 
 * description:���ŷ���
 * 
 * 
 * 
 * Create by:�տ�ʤ
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
        #region �¼�����
        /// <summary>
        /// �¼������Ͷ���֪ͨ
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

                //���ɶ������� 
                string sBody = string.Empty;
                sBody = stModelContent[6].ToLower();

                #region ��ȡ������ذ�����Ϣ

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
                string sDealDesc = "�������";
                try
                {  //�ж϶����Ƿ���ڣ�����ʾ��������
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
                        sDealDesc = sDealDesc + " 200�˻�201�������202����,�����ʽ#[��ʶ��]#[�������]#[��������]";
                    }
                    else
                    {
                        sDealDesc = sDealDesc + "0ȷ��200�˻�201�������202����,�����ʽ#[��ʶ��]#[�������]#[��������]";
                    }
                    
                }
                finally { ConfigTool.CloseConnection(cn); }

                #endregion


                long lngBusID = EpowerGlobal.EPGlobal.GetNextID("Ea_SmsSendID");

                sBody += " �����ʶ:" + lngBusID + "�ɻظ�����" + sDealDesc;

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>���ⵥ���Ͷ���֪ͨ
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

                //���ɶ������� 
                string sBody = string.Empty;
                sBody = stModelContent[6].ToLower();

                #region ��ȡ������ذ�����Ϣ

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
                { //�ж϶����Ƿ���ڣ�����ʾ��������
                    strSQL = "SELECT ActionID,ActionName " +
                    " FROM Es_N_M_Action " +
                    " WHERE flowModelID = " + lngFlowModelID.ToString() +
                    "		AND NodeModelID = " + lngNodeModelID.ToString();
                    DataTable dtActions = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                    bool blnHasAction = false;
                    string sDealDesc = "�������";

                    foreach (DataRow r in dtActions.Rows)
                    {
                        blnHasAction = true;
                        sDealDesc += r["actionid"].ToString() + r["actionname"].ToString().Trim();

                    }
                    if (blnHasAction == true)
                    {
                        sDealDesc = sDealDesc + " 200�˻�201�������202����,�����ʽ#[��ʶ��]#[�������]#[��������]";
                    }
                    else
                    {
                        sDealDesc = sDealDesc + "0ȷ��200�˻�201�������202����,�����ʽ#[��ʶ��]#[�������]#[��������]";
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

        #region �¼�������ȵ���
        /// <summary>
        /// ���ŷ�ʽʵ���¼�������ȵ���
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
                //���ɶ������� 
                string sBody = string.Empty;
                //��ʱ����ģ�巽ʽ
                sBody = sContact + "����,�¼�[" + sSubject + "]�Ѿ���ɣ��ظ�";

                long lngBusID = EpowerGlobal.EPGlobal.GetNextID("Ea_SmsSendID");
                sBody += ":#" + lngBusID.ToString() + "#[1��������2����3������]#[���] �����������ѡ";
                //SendMessage(sCTel, sBody, "Surv", lngBusID, lngFlowID, 0);

                return true;
            }
            else
            {
                return false;
            }
        }


        #endregion 

        #region ����ģ�����ù��еķ��ͷ�ʽ
        /// <summary>
        /// ����ģ�����ù��еķ��ͷ�ʽ
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="receiverArray"></param>
        /// <param name="fv"></param>
        public static void SendMessagePublic(OracleTransaction trans, long lngFlowID, string receiverArray, FieldValues fv, long lngFlowModelId)
        {
            //��ȡ�ռ���Email
            string getMessageByName = "";//��ȡ�ռ��˵�Email ������ʽA
            string getMessageCopyByName = "";//��ȡ�����˵�Email ������ʽ
            string getTitle = "";//��ȡ�ʼ�����
            string getMessageContent = "";//�ʼ�������

            string getAppName = "";//Ӧ������
            long getAppId = 0;//Ӧ��id
            string flowname = ""; //��������
            long getMessageAppModelId = MailAndMessageRuleDP.GetOFlowModelId(lngFlowModelId, ref getAppId, ref flowname);//�������ģ�͵�ģ��id
            getAppName = MailAndMessageRuleDP.GetAppName(getAppId);

            //���˵��Ƿ��һ������
            if (getAppId != 0)
            {
                #region ����ģ�����û�ȡ�ռ���Email
                if (fv.GetFieldValue("CTEL1") != null)
                {
                    getMessageByName = getMessagebyNamefunction(getAppId, getMessageAppModelId, fv.GetFieldValue("CTEL1").Value.Trim(), receiverArray);
                }
                else
                {
                    getMessageByName = getMessagebyNamefunction(getAppId, getMessageAppModelId, "", receiverArray);
                }
                #endregion

                //�����ʼ����ã���ȡ�����˵�Email
                getMessageCopyByName = SendCopyByName(getAppId, getMessageAppModelId);

                //�����������
                getMessageContent = getModeTitle_content(getAppId, getMessageAppModelId);

                //�滻�����Ĳ�ͬ��ֵ
                getMessageContent = getContent(getAppId, getMessageContent, fv);
                //�����ʼ�������
                string sBody = getMessageContent;
                try
                {
                    //�ռ��˲����ڿ�
                    if (getMessageByName != "" && sBody.Trim() != "")
                    {
                        //���Ž�����
                        string[] messageName = getMessageByName.Split(';');
                        foreach (string mobile in messageName)
                        {
                            if (mobile.Trim() != string.Empty)
                                SendMessage(mobile, sBody.Trim());
                        }
                    }

                    if (getMessageCopyByName != "" && sBody.Trim() != "")
                    {
                        //������������������
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

        #region ��ö���ģ�������
        /// <summary>
        /// ���ģ���еı��� �� ģ������
        /// </summary>
        /// <param name="appid">Ӧ��id</param>
        /// <param name="flowmodelid">����ģ��id</param>
        /// <returns>���ر����ģ������</returns>
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
                case 1028://��������
                    content = content.Replace(",#?�汾����?#", fv.GetFieldValue("VERSIONNAME").Value).Replace(",#?�汾��?#", fv.GetFieldValue("VERSIONCODE").Value).Replace(",#?������Χ?#", fv.GetFieldValue("RELEASESCOPENAME").Value).Replace(",#?��ϵ��?#", fv.GetFieldValue("RELEASEPERSONNAME").Value).Replace(",#?��ϵ�绰?#", fv.GetFieldValue("RELEASEPHONE").Value);
                    content = content.Replace(",#?�汾����?#", fv.GetFieldValue("VERSIONKINDNAME").Value).Replace(",#?�汾����?#", fv.GetFieldValue("VERSIONTYPENAME").Value).Replace(",#?�汾�������ݼ��?#", fv.GetFieldValue("RELEASECONTENT").Value);
                    break;
                case 201://�Զ��������
                    break;
                case 400://֪ʶ����                  
                    content = content.Replace("#?����?#", fv.GetFieldValue("Title").Value).Replace("#?�ؼ���?#", fv.GetFieldValue("Pkey").Value);
                    content = content.Replace("#?ժҪ?#", fv.GetFieldValue("tags").Value).Replace("#?֪ʶ���?#", fv.GetFieldValue("Type").Value).Replace("#?֪ʶ����?#", fv.GetFieldValue("Content").Value);
                    break;
                case 199://ͨ������       
                    break;
                case 210://�������                    
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

            return content;
        }
        #endregion

        #region ��ȡ������ȡ�˵ĵ绰���뼯��
        /// <summary>
        /// ��ȡ�ռ�����Ϣ
        /// </summary>
        /// <param name="appid">Ӧ��id</param>
        /// <param name="modelid">����ģ��id</param>
        /// <param name="clientEmail">�ͻ��绰</param>
        /// <param name="receiversIds">�ύ�������Զ��Ÿ�������ʽ���û�id�ַ���</param>
        /// <returns></returns>
        public static string getMessagebyNamefunction(long appid, long modelid, string clientMobile, string receiversIds)
        {

            //sendertypeid=1Ϊ�ύʱ���Ͷ���
            //�ύʱ�Ľ��������
            string strSQL = "select receiverstypeid from MailAndMessageRule t where ROWNUM<=1 AND t.systemid=" + appid + " and t.modelid =" + modelid + "  and t.status=1 and sendertypeid=1";
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            string MobileAdd = "";
            if (dt.Rows.Count > 0)
            {
                #region ��ȡ�ռ��˵� Email
                if (dt.Rows[0]["receiverstypeid"].ToString() == "1")
                {
                    //�ͻ�
                    MobileAdd = clientMobile;
                }
                else if (dt.Rows[0]["receiverstypeid"].ToString() == "2")
                {
                    #region ����ʱ�������˴����˵����
                    string sMobile = "";
                    string[] people = receiversIds.Trim().Split(",".ToCharArray());
                    foreach (string ple in people)
                    {
                        //������
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
                    #region ����ʱ�������˿ͻ��ʹ����˵����
                    //�ͻ��ʹ�����
                    string sMobile = "";
                    string[] people = receiversIds.Trim().Split(",".ToCharArray());
                    foreach (string ple in people)
                    {
                        //������
                        string[] sRecMsgs = ple.Trim().Split("|".ToCharArray());
                        if (sRecMsgs[0] != "")
                        {
                            Epower.DevBase.Organization.SqlDAL.UserEntity user = new UserEntity(long.Parse(sRecMsgs[0]));
                            sMobile += user.Mobile.ToString() + ";";
                        }
                    }
                    if (sMobile != "" && clientMobile != "")
                    {
                        //�ͻ��봦���ǿյ����
                        MobileAdd = clientMobile + ";" + sMobile.Trim(';');
                    }
                    else
                    {
                        if (sMobile != "")
                        {
                            //��ȡ�����˷ǿգ��ͻ�Ϊ�յ����
                            MobileAdd = sMobile.Trim(';');
                        }
                        else
                        {
                            //�ͻ��ǿգ������˿յ����
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


        #region ��ȡ���ö��ų�����Ա�绰����
        /// <summary>
        /// ��ȡ������Ա���ֻ�����ļ���
        /// </summary>
        /// <param name="AppId">Ӧ��id </param>
        /// <param name="modelid">����ģ��id</param>
        /// <returns>���س�����ԱEmail�ļ���</returns>
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



        #region ���Ͷ���Ϣ
        /// <summary>
        /// ���Ͷ���Ϣ
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

        #region ͬ�����Ͷ���ʹ���̷߳�ʽ����

        #region ���ô���Ķ������ݵ�ֵ
        /// <summary>
        /// ���ô���Ķ������ݵ�ֵ
        /// </summary>
        public struct setcount
        {
            /// <summary>
            /// �ֻ�����
            /// </summary>
            public string mobileNo;
            /// <summary>
            /// ���͵�����
            /// </summary>
            public string content;
        }
        #endregion

        #region �ӿڶ���
        /// <summary>
        /// �ӿڶ���
        /// </summary>
        /// <param name="strValueOjb"></param>
        public static void SendMobileMessage(object strValueOjb)
        {
            setcount value = (setcount)strValueOjb;
            string mobileNo = value.mobileNo;
            string content = value.content;
            try
            {
                int port = int.Parse(Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "sendPort"));//�˿ں�
                string host = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "sendIpAddress");//������IP
                //*******************����ͷ*************************************************//
                //string stAllLenth = "";//�ܱ��ĳ������������ֶεĳ��ȣ�6
                string stSerialNumber = System.DateTime.Now.ToString("yyyyMMdd") + "00";//���ݰ����к�(�����ͷ����б�����Լ�顣��֤ÿ��Ψһ)10
                string stType = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Title_MessageType");//��������(1-�������� 2-�ļ�����)
                string stSource = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Title_MessageSource");//������Դ 2
                string stCode = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Title_TransactionCode");//���״���
                string stLevel = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Title_Level");//���ȼ�(1-9 ����Խ�����ȼ�Խ��)
                string stDate = System.DateTime.Now.ToString("yyMMdd");//��������
                string stTime = System.DateTime.Now.ToString(System.DateTime.Now.ToLongTimeString().Replace(":", "") + System.DateTime.Now.Millisecond.ToString()); ;//����ʱ��
                string stReqFlag = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Title_ReqFlag");//����/Ӧ���ʶ(R-���� A-Ӧ��)
                string stEncFlag = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Title_EncFlag");//���ܱ�־(Y-���� ����-������)
                string stPakNumber = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Title_PakNumber"); //���ļ�¼��(����ʱ����1�����ʱ�����ܰ���)
                string stPakSerialNum = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Title_PakSerialNum");//��ǰ�����(����ʱ����1�����ʱ����ʵ�ʵİ��š�)
                string stFIELDS = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Title_FIELDS");//���ݲ��������(������е��ֶ���)
                string stSuccess = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Title_Success"); //�ɹ������־(Ӧ��ʱ�������1��-�ɹ�������-ʧ�ܡ�ʧ��ԭ���ڷ��ذ�������)
                string stReserve = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Title_Reserve");//����
                //*******************������*************************************************//
                string stZDK = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Body_Sub-port");//�Ӷ˿ں�
                string stYWDM = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Body_BusinessCode");//ҵ�����
                string stFSJG = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Body_SendDeptID");//���Ͳ���(����)ID
                string stYYSID = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Body_OperatorsID");//��Ӫ��ID
                string stSJH = mobileNo;//��Ϣ����(����)����
                string stDXBM = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Body_MessageCode");// ���ű���
                string stContent = content;//��������
                string stXX = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Body_MessageTT");//��Ϣ��
                string stYXJ = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Body_Level");//���ȼ�
                string stFSCS = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "Body_SendNumber");//���ʹ���
                string stTHH = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "TeFuhaoFlag");// �ط��ű�ʶ
                string stJFBS = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "BillingFlag");//�Ʒѱ�ʶ
                //����ͷ
                string stSendHead = stSerialNumber + stType + stSource;
                stSendHead = stSendHead + stCode + stLevel + stDate + stTime + stReqFlag;
                stSendHead = stSendHead + stEncFlag + stPakNumber + stPakSerialNum + stFIELDS + stSuccess + stReserve;
                //������
                string stSendBody = "01" + getStringByteLenth(stZDK, 4) + stZDK + "02" + getStringByteLenth(stYWDM, 4) + stYWDM + "03" + getStringByteLenth(stFSJG, 4) + stFSJG + "04" + getStringByteLenth(stYYSID, 4) + stYYSID + "05" + getStringByteLenth(stSJH, 4) + stSJH + "06" + getStringByteLenth(stDXBM, 4) + stDXBM;
                stSendBody = stSendBody + "07" + getStringByteLenth(stXX, 4) + stXX + "08" + getStringByteLenth(stContent, 4) + stContent + "09" + getStringByteLenth(stYXJ, 4) + stYXJ + "10" + getStringByteLenth(stFSCS, 4) + stFSCS + "11" + getStringByteLenth(stTHH, 4) + stTHH + "12" + getStringByteLenth(stJFBS, 4) + stJFBS;
                //��ɵı���
                string stSend = getStringByteLenth(stSendHead + stSendBody, 6) + stSendHead + stSendBody;
                if (Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("MobileInterface", "sendIsOpen").Trim() == "1")
                {
                    try
                    {
                        IPAddress ip = IPAddress.Parse(host);
                        IPEndPoint ipe = new IPEndPoint(ip, port);//��ip�Ͷ˿�ת��ΪIPEndPointʵ��
                        Socket c = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//����һ��Socket
                        c.Connect(ipe);//���ӵ�������
                        byte[] bs = Encoding.GetEncoding("GBK").GetBytes(stSend.Trim());
                        c.Send(bs, bs.Length, 0);//���Ͳ�����Ϣ
                        string recvStr = "";
                        byte[] recvBytes = new byte[1024];
                        int bytes;
                        bytes = c.Receive(recvBytes, recvBytes.Length, 0);//�ӷ������˽��ܷ�����Ϣ
                        recvStr += Encoding.GetEncoding("GB2312").GetString(recvBytes, 0, bytes);
                        c.Close();
                        if (recvStr != "")
                        {
                            if (recvStr.Substring(70, 4).ToString() == "0000")
                            {
                                createSmsLog(mobileNo, content, "���ŷ��ͳɹ�");
                            }
                            else
                            {
                                createSmsLog(mobileNo, content, "���ŷ���ʧ�ܣ����շ������ݰ���" + recvStr + ", �������ݰ���" + stSend);
                            }
                        }
                        else
                        {
                            createSmsLog(mobileNo, content, "���ŷ���ʧ�ܣ�δ�յ����ݷ��������������ݰ���" + stSend);
                        }
                    }
                    catch (Exception ex)
                    {
                        createSmsLog(mobileNo, content, "���ŷ���ʧ�ܣ�ʧ��ԭ������" + ex.Message);
                    }
                }
                else
                {
                    createSmsLog(mobileNo, content, " ���ŷ���ʧ�ܣ�δ�������Ź��ܣ�" + stSend);
                }
            }
            catch (ArgumentNullException e1)
            {
                createSmsLog(mobileNo, content, "���ŷ���ʧ�ܣ� �쳣��" + e1.Message);
            }
            catch (SocketException e2)
            {
                createSmsLog(mobileNo, content, "���ŷ���ʧ�ܣ� �쳣��" + e2.Message);
            }
        }
        #endregion

        #region ������־
        /// <summary>
        ///������־
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

        #region ���ݵ��ֽ�������
        /// <summary>
        /// ���ݵ��ֽ�������
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


        #region ���Ͷ���
        /// <summary>
        /// ���Ͷ���
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
                //���ҵ��ϵ�к� Ϊ0 ���Ƿ����ڲ���һ��ϵ�к�
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

