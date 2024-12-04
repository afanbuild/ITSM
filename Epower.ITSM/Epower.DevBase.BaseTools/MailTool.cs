using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Mail;


namespace Epower.DevBase.BaseTools
{
    /// <summary>
    /// 邮件发送代理类,用于多线程处理
    /// </summary>
    public class MailToolAgent : IDisposable
    {
        string sTo = "";
        string sCC = "";//抄送
        string sBCC = "";//密送
        string sTitle = "";
        string sBody = "";
        string sSmtpServer = string.Empty;
        string sPsd=string.Empty;
        string sFrom=string.Empty;
        string sSSL = string.Empty;
        string sPort=string.Empty;
        string sUserName= string.Empty;
        string smtpdisplayName = string.Empty;//显示名称

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strTo"></param>
        /// <param name="strCC"></param>
        /// <param name="strTitle"></param>
        /// <param name="strBody"></param>
        /// <param name="strSmtpServer"></param>
        /// <param name="strPsd"></param>
        /// <param name="strFrom"></param>
        /// <param name="strSSL"></param>
        /// <param name="strPort"></param>
        /// <param name="strUserName"></param>
        /// <param name="pdisplayName"></param>
        /// <param name="isxianshi"></param>
        public MailToolAgent(string strTo, string strCC, string strTitle, string strBody, string strSmtpServer, string strPsd,
            string strFrom, string strSSL, string strPort, string strUserName, string pdisplayName, bool isxianshi)
        {
            sTo = strTo;
            sCC = strCC;
            sBCC = "";
            sTitle = strTitle;
            sBody = strBody;
            sSmtpServer = strSmtpServer;
            sPsd = strPsd;
            sFrom = strFrom;
            strSSL = sSSL;
            sPort = strPort;
            sUserName = strUserName;
            smtpdisplayName = pdisplayName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strTo"></param>
        /// <param name="strCC"></param>
        /// <param name="strTitle"></param>
        /// <param name="strBody"></param>
        /// <param name="strSmtpServer"></param>
        /// <param name="strPsd"></param>
        /// <param name="strFrom"></param>
        /// <param name="strSSL"></param>
        /// <param name="strPort"></param>
        /// <param name="strUserName"></param>
        public MailToolAgent(string strTo, string strCC, string strTitle, string strBody, string strSmtpServer, string strPsd,
            string strFrom,string strSSL, string strPort, string strUserName)
        {
            sTo = strTo;
            sCC = strCC;
            sTitle = strTitle;
            sBody = strBody;
            sSmtpServer = strSmtpServer;
            sPsd = strPsd;
            sFrom = strFrom;
            strSSL = sSSL;
            sPort = strPort;
            sUserName = strUserName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strTo"></param>
        /// <param name="strCC"></param>
        /// <param name="strBCC"></param>
        /// <param name="strTitle"></param>
        /// <param name="strBody"></param>
        /// <param name="strSmtpServer"></param>
        /// <param name="strPsd"></param>
        /// <param name="strFrom"></param>
        /// <param name="strSSL"></param>
        /// <param name="strPort"></param>
        /// <param name="strUserName"></param>
        public MailToolAgent(string strTo, string strCC, string strBCC,string strTitle, string strBody, string strSmtpServer, string strPsd,
           string strFrom, string strSSL, string strPort, string strUserName)
        {
            sTo = strTo;
            sCC = strCC;
            sBCC = strBCC;
            sTitle = strTitle;
            sBody = strBody;
            sSmtpServer = strSmtpServer;
            sPsd = strPsd;
            sFrom = strFrom;
            strSSL = sSSL;
            sPort = strPort;
            sUserName = strUserName;
        }

        /// <summary>
        /// 
        /// </summary>
        public void DoAction()
        {
            MailTool mailTool = new MailTool();
            bool blnSuccess = mailTool.SendEmail(sTo, sCC, sBCC, sTitle, sBody, sSmtpServer, sPsd, sFrom, sSSL, sPort, sUserName, smtpdisplayName);
        }

        #region IDisposable 成员

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            // TODO:  添加 HandleAgent.Dispose 实现
            GC.SuppressFinalize(this);
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class MailTool
    {
        /// <summary>
        /// 
        /// </summary>
        public MailTool()
        {

        }

        //判断邮件格式是否正确
        public static bool IsEmail(string Str)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

            Regex re = new Regex(strRegex);
            if (re.IsMatch(Str))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        
            //mail.CC.Add(new MailAddress(""));
            //mail.Bcc.Add(new MailAddress(""));

        /// <summary>
        /// 发送电子邮件的方法
        /// </summary>
        /// <param name="strTo"></param>
        /// <param name="strCC"></param>
        /// <param name="strTitle"></param>
        /// <param name="strBody"></param>
        /// <param name="sSmtpServer"></param>
        /// <param name="sPsd"></param>
        /// <param name="sFrom"></param>
        /// <param name="sSSL"></param>
        /// <param name="sPort"></param>
        /// <param name="sUserName"></param>
        /// <returns></returns>
        public bool SendEmail(string strTo, string strCC, string strBcc, string strTitle, string strBody, string sSmtpServer, string sPsd,
            string sFrom, string sSSL, string sPort, string sUserName, string smtpDisplayName)
        {

            MailMessage mail = new MailMessage();

            mail.BodyEncoding = Encoding.GetEncoding("gb2312");       //邮件主题编码
            mail.SubjectEncoding = Encoding.GetEncoding("gb2312");    //标题编码

            #region
            //获取收件人
            if (strTo != "")
            {
                string[] toMail = strTo.Trim(',').Trim(';').Split(',');
                foreach (string value in toMail)
                {
                    if (value != "")
                    {
                        string[] mailValue = value.Split(';');
                        foreach (string valuesMail in mailValue)
                        {
                            if (valuesMail != "" && IsEmail(valuesMail))
                            {                                
                                mail.To.Add(new MailAddress(valuesMail));
                            }
                        }
                    }
                }
            }

            if (strCC != "")
            {
                string[] toMail = strCC.Trim(',').Trim(';').Split(',');
                foreach (string value in toMail)
                {
                    if (value != "")
                    {
                        string[] mailValue = value.Split(';');
                        foreach (string valuesMail in mailValue)
                        {
                            if (valuesMail != "" && IsEmail(valuesMail))
                            {
                                mail.CC.Add(new MailAddress(valuesMail));
                            }
                        }
                    }
                }
            }
            if (strBcc != "")
            {
                string[] mailsB = strBcc.Split(";".ToCharArray());
                for (int i = 0; i < mailsB.Length; i++)
                {
                    string sEmail = mailsB[i].Trim();
                    if (sEmail != "" && IsEmail(sEmail))
                    {
                        mail.Bcc.Add(new MailAddress(sEmail));
                    }
                }
            }
            #endregion

            if (smtpDisplayName.Length > 0)
            {
                mail.From = new MailAddress(sFrom, smtpDisplayName, System.Text.Encoding.GetEncoding("gb2312"));
            }
            else
            {
                mail.From = new MailAddress(sFrom);
            }

            mail.Subject = strTitle;
            mail.Body = strBody;


            mail.IsBodyHtml = true;                                   //获取或设置一个值，该值指示电子邮件正文是否为 HTML
            mail.Priority = MailPriority.Normal;    //邮件级别


            SmtpClient smtpClient = new SmtpClient();
            if (sSSL.ToLower() == "true")
            {
                smtpClient.EnableSsl = true;
            }
            else
            {
                smtpClient.EnableSsl = false;
            }
            smtpClient.Host = sSmtpServer;
            if (sPort != "0")
            {
                smtpClient.Port = int.Parse(sPort);
            }
            else
            {
                smtpClient.Port = 25;
            }
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            smtpClient.Credentials = new NetworkCredential(sUserName, sPsd);
            try
            {
                smtpClient.Send(mail);
                return true;
            }
            catch (Exception e)
            {
                EventTool.EventLog("邮件发送失败" + e.Message);
                return false;
            }
        }

        public bool SendEmail(string strTo, string strCC, string strTitle, string strBody, string sSmtpServer, string sPsd,
           string sFrom, string sSSL, string sPort, string sUserName)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(new MailAddress(strTo));
            if (strCC != "")
            {
                mail.CC.Add(new MailAddress(strCC));
            }
            mail.From = new MailAddress(sFrom);
            mail.Subject = strTitle;
            mail.Body = strBody;
            mail.BodyEncoding = Encoding.GetEncoding("gb2312");       //邮件主题编码
            mail.SubjectEncoding = Encoding.GetEncoding("gb2312");    //标题编码
            mail.IsBodyHtml = true;                                   //获取或设置一个值，该值指示电子邮件正文是否为 HTML
            mail.Priority = MailPriority.Normal;    //邮件级别


            SmtpClient smtpClient = new SmtpClient();
            if (sSSL.ToLower() == "true")
            {
                smtpClient.EnableSsl = true;
            }
            else
            {
                smtpClient.EnableSsl = false;
            }
            smtpClient.Host = sSmtpServer;
            if (sPort != "0")
            {
                smtpClient.Port = int.Parse(sPort);
            }
            else
            {
                smtpClient.Port = 25;
            }
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            //smtpClient.UseDefaultCredentials = false;
            //smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.PickupDirectoryFromIis;

            smtpClient.Credentials = new NetworkCredential(sUserName, sPsd);
            try
            {
                //smtpClient.SendAsync(mail,mail);
                smtpClient.Send(mail);
                return true;
            }
            catch (Exception Ex)
            {
                EventTool.EventLog("邮件发送失败" + Ex.Message);
                return false;
            }



        }
    }
}
