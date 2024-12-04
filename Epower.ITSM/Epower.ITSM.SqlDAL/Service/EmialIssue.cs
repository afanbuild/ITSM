/***********************************
 *创建人：yanghw
 *创建时间：2011-08-18
 *
 * 说明：收取自动报账邮件
 * 
 * 
 * *****************************/


using System;
using System.Collections.Generic;
using System.Text;
using Epower.ITSM.Net.POP3.Client;
using Epower.ITSM.Net.Mime;
using Epower.DevBase.BaseTools;
using EpowerGlobal;
using System.Data;
using System.Data.OracleClient;
using Epower.ITSM.Net;


namespace Epower.ITSM.SqlDAL.Service
{
    /// <summary>
    /// 邮件自动报账
    /// </summary>
    public class EmialIssue
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public EmialIssue()
        {
        }

        #region 邮件收取过程
        /// <summary>
        /// 邮件收取过程
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="PassWord"></param>
        /// <param name="PopServer"></param>
        /// <param name="Port"></param>
        /// <param name="MaxDate"></param>
        public static void GetNewMailIntoDataBase(string UserName, string PassWord, string PopServer, int Port, DateTime MaxDate)
        {
            //初始化一个POP3 Client
            using (POP3_Client c = new POP3_Client())
            {
                //连接POP3服务器

                c.Connect(PopServer, WellKnownPorts.POP3);
                //验证用户身份
                c.Authenticate(UserName, PassWord, false);
                if (c.Messages.Count > 0)
                {
                    foreach (POP3_ClientMessage mail in c.Messages)
                    {
                        //判断是否跟当前最大的时间作比较，大于当前时间就处理

                        DateTime CurrentEmailDate = DateTime.Now;
                        Mime m =Mime.Parse(mail.MessageToByte());
                        try
                        {
                            CurrentEmailDate = DateTime.Parse(m.MainEntity.Date.ToString());
                        }
                        catch (Exception ex)
                        {
                            string s = ex.Message;
                        }
                        try
                        {
                            if (CurrentEmailDate.CompareTo(MaxDate) > 0 && m.BodyText != null)
                            {
                                //插入报障数据库
                                saveEmailIssue(m.MainEntity.From.ToAddressListString(), m.MainEntity.Subject, m.BodyText);


                                #region 暂时注释
                                //string strFileCatalog = CommonDP.GetConfigValue("TempCataLog", "FileCataLog");
                                //Email_NetDP MyModel = new Email_NetDP();
                                //MyModel.EmailContent = m.BodyText;
                                //MyModel.EmailState = 1;
                                //MyModel.EmailTitle = m.MainEntity.Subject;
                                //MyModel.FromUser = DecodeMailto(m.MainEntity.From.ToAddressListString());
                                //MyModel.strAttachment = "";
                                //MyModel.ToUser = touser;
                                //MyModel.RegUserName = MyModel.FromUser;
                                //MyModel.RegTime = DateTime.Parse(DateTime.Now.ToString());
                                //MyModel.InsertRecorded(MyModel);
                                ////获取附件
                                //foreach (MimeEntity entry in m.Attachments)
                                //{
                                //    long lngNextFileID = EPGlobal.GetNextID("FILE_ID");
                                //    string FileName = entry.ContentDisposition_FileName; //获取文件名称
                                //    Email_Net_AttachmentDP ee = new Email_Net_AttachmentDP();
                                //    ee.FileID = lngNextFileID;
                                //    ee.FileName = FileName;
                                //    ee.filepath = strFileCatalog;
                                //    ee.MailID = MyModel.ID;
                                //    ee.Status = 10;
                                //    string[] arrFileName = FileName.Split('.');
                                //    ee.SufName = arrFileName[arrFileName.Length - 1].ToString();
                                //    string path = strFileCatalog + lngNextFileID.ToString();
                                //    byte[] data = entry.Data;
                                //    FileStream pFileStream = null;
                                //    pFileStream = new FileStream(path, FileMode.Create);
                                //    pFileStream.Write(data, 0, data.Length);
                                //    pFileStream.Close();
                                //    ee.upTime = DateTime.Parse(DateTime.Now.ToString());
                                //    ee.FileID = lngNextFileID;
                                //    ee.InsertRecorded(ee);
                                //}
                                #endregion 
                            }
                        }
                        catch (Exception ee)
                        {
                            System.Web.HttpContext.Current.Response.Write("<script>alert('" + ee.Message.ToString() + "');</script>");
                        }
                    }
                }
            }

        }

        #endregion 

        #region 收取保障邮件
        /// <summary>
        /// 收取保障邮件
        /// </summary>
        public static void getEmailIssue()
        {
            string userName = CommonDP.GetConfigValue("SetMail", "smtpUserName");
            string PassWord = CommonDP.GetConfigValue("SetMail", "smtppsd");
            string PopServer = CommonDP.GetConfigValue("SetMail", "smtpserver");
            string Port = CommonDP.GetConfigValue("SetMail", "smtpPort");
            DateTime maxDate = getEmailIssueTime() ;


           GetNewMailIntoDataBase(userName, PassWord, PopServer, int.Parse(Port), maxDate);

        }

        #endregion 


        #region 保存保障内容
        /// <summary>
        /// 保存保障内容
        /// </summary>
        /// <param name="FromEmail"></param>
        /// <param name="EmailTitle"></param>
        /// <param name="EmailContant"></param>
        public static void saveEmailIssue(string FromEmail,string EmailTitle,string EmailContant)
        {
            string ID = EPGlobal.GetNextID("EmailIssueID").ToString();
            string strSQL = @" insert  into EmailIssue (ID,FromEmail,EmailTitle,EmailContant,Datatime,Statue)   
                values(" +ID+","+ StringTool.SqlQ(FromEmail)+","+ StringTool.SqlQ(EmailTitle)+","+ StringTool.SqlQ(EmailContant)+",sysdate,0)";

            CommonDP.ExcuteSql(strSQL);
            
            #region 
            //插入未处理请求
            cst_RequestDP requst = new cst_RequestDP();
            requst.Content = StringTool.SqlQ(EmailContant);
            requst.Contract = "[邮件]" + EmailTitle;
            requst.DealLog = 0;
            requst.inType = 0;
            requst.inDate = System.DateTime.Now;
            requst.subject = EmailTitle;
            requst.CTel = "";
            requst.InsertRecorded(requst);
            #endregion 
        }
        #endregion 


        #region 获得收取邮件的最大时间
        /// <summary>
        /// 获得收取邮件的最大时间
        /// </summary>
        /// <returns></returns>
        public static DateTime getEmailIssueTime()
        {
            string strSQL = " select nvl(max(datatime),sysdate-1) datatimes from EmailIssue ";
            DataTable dt= CommonDP.ExcuteSqlTable(strSQL);
            if (dt.Rows.Count > 0)
            {
                return DateTime.Parse(dt.Rows[0]["datatimes"].ToString());
            }
            else
            {
                return System.DateTime.Now;
            }

        }
        #endregion 

        #region 返回数据邮件的数据集
        /// <summary>
        /// 返回数据邮件的数据集
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public static DataTable getEmailIssueTable(string strWhere)
        {
            string strSQL = " select * from EmailIssue where 1=1  " + strWhere +"  order by datatime  desc";
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            return dt;

        }

        /// <summary>
        /// 返回数据邮件的数据集
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public static DataTable getEmailIssueTable(long ID)
        {
            string strSQL = " select * from EmailIssue where 1=1  and Id=" + ID.ToString();
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            return dt;

        }

        public static void updateStatue(long ID)
        {
            string strSQL = " update  EmailIssue set Statue=1 where Id=" + ID.ToString();
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
        }


        #endregion 

        #region 存储过程翻页
        /// <summary>
        /// 存储过程翻页
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="rowcount"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sWhere, string sOrder, int pagesize, int pageindex, ref int rowcount)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "EmailIssue", "*", sOrder, pagesize, pageindex, sWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion 


        #region 删除邮件保障 
        /// <summary>
        /// 删除邮件保障
        /// </summary>
        /// <param name="Id"></param>
        public static void DeleteEmailIssue(long Id)
        {
            string strSQL = " delete EmailIssue where Id=" + Id.ToString();
            CommonDP.ExcuteSql(strSQL);
        }
        #endregion
    }

}
