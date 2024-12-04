using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.OracleClient;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.Common
{
    public partial class frmSendMailFeedBack : BasePage
    {
        #region ������
        private string msType = "0";
        /// <summary>
        /// �����ʼ����
        /// </summary>
        protected string sType
        {
            get
            {
                return msType;
            }
            set
            {
                msType = value;
            }
        }
        private long mFlowID = 0;
        /// <summary>
        ///���̱��
        /// </summary>
        protected long FlowID
        {
            get
            {
                return mFlowID;
            }
            set
            {
                mFlowID = value;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblSend.Text = CommonDP.GetConfigValue("SetMail", "smtpfrom");

                if (Request["Type"] != null)
                    sType = Request["Type"].ToString();
                if (Request["FlowID"] != null)
                    FlowID = long.Parse(Request["FlowID"].ToString());
                string sUrlRoot = CommonDP.GetConfigValue("SetMail", "smtpLinkRoot");
                string sEmail = string.Empty;
                string sCustName = string.Empty;
                string sSubject = string.Empty;
                string sBody = string.Empty;
                DataTable dt = new DataTable();
                switch (sType)
                {
                    case "1":            //���񵥻ط�
                        this.Title = "�ʼ��ط�";
                        sSubject = CommonDP.GetConfigValue("Other", "ServiceAutoFeedBackTitle");
                        dt = MailSendDeal.GetSendData(FlowID);
                        foreach (DataRow dr in dt.Rows)
                        {
                            sEmail = dr["Email"].ToString();
                            sCustName = dr["CustName"].ToString();
                            sSubject += dr["Subject"].ToString();
                            break;
                        }
                        //�����ʼ����� 
                        sBody = string.Empty;
                        sBody = MailBodyDeal.GetMailBody("FeedBack.htm", dt);

                        long lngmnUserID = 1;
                        if (CommonDP.GetConfigValue("Other", "ServiceAutoFeedBackUserID") != null && CommonDP.GetConfigValue("Other", "ServiceAutoFeedBackUserID") != string.Empty)
                            lngmnUserID = long.Parse(CommonDP.GetConfigValue("Other", "ServiceAutoFeedBackUserID"));
                        long lngUserID = lngmnUserID;

                        TokenIntention ti2 = new TokenIntention();
                        ti2.IntentionUrl = "../AppForms/frmFeedBack.aspx?FlowID=" + FlowID.ToString() + "&CustName=" + sCustName;
                        string sGuid2 = APTokenDP.SaveTokenInfo(ti2.ToXml(), lngUserID, lngmnUserID);
                        string sFeedBackUrl = string.Empty;
                        hidUrl.Value = sUrlRoot + "/Common/frmDoAction.aspx?userid=" + lngUserID.ToString() + "&guid=" + sGuid2;
                        sFeedBackUrl = @"<a href='####feedurl####' target=""_blank"">";
                        sFeedBackUrl += sUrlRoot + "/Common/frmDoAction.aspx?guid=" + sGuid2 + "&userid=" + lngUserID.ToString();
                        sFeedBackUrl += @"</a>";

                        sBody = sBody.Replace("#?feedbackurl?#", sFeedBackUrl);
                        txtContent.Text = sBody;
                        MsgTo.Text = sEmail;
                        txtSubject.Text = sSubject;
                        break;
                    case "2":       //�����ʼ�֪ͨ
                        this.Title = "�ʼ�֪ͨ";
                        sSubject = CommonDP.GetConfigValue("Other", "ServiceAutoFeedBackTitle");
                        dt = MailSendDeal.GetSendData(FlowID);
                        foreach (DataRow dr in dt.Rows)
                        {
                            sEmail = dr["Email"].ToString();
                            sCustName = dr["CustName"].ToString();
                            sSubject += dr["Subject"].ToString();
                            break;
                        }
                        //�����ʼ����� 
                        sBody = MailBodyDeal.GetMailBody("Event_Service.htm", dt);
                        string sUrl = string.Empty;
                        hidUrl.Value = sUrlRoot + "/default.aspx";
                        sUrl = @"<a href='####feedurl####'" + " target='_blank'>";
                        sUrl += sUrlRoot + "/default.aspx";
                        sUrl += @"</a>";
                        sBody = sBody.Replace("#?Detail?#", sUrl);
                        sBody = sBody.Replace("#?Title?#", sSubject);
                        txtContent.Text = sBody;
                        MsgTo.Text = sEmail;
                        txtSubject.Text = sSubject;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                string sstyle = @"<style> body,input,td,p,select,th,div,a,textarea { font-size: 11px; font-family: 'tahoma','verdana','sans-serif' }
	                                    .td_title { background-color:#DCF1FA; height:20px;valign:center; }
	                                    .td_detail { background-color:#FaFaFa;height:20px; }
                                    </style>";
                txtContent.Text = sstyle + txtContent.Text;
            }
        }

        /// <summary>
        /// �ʼ�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSend1_Click(object sender, EventArgs e)
        {
            string toMail = "";
            if (MsgTo.Text.Trim() != "")
            {
                toMail = MsgTo.Text;
            }
            if (toMail.Trim() != string.Empty)
            {
                string scontent = txtContent.Text.Trim();
                scontent = scontent.Replace("####feedurl####", hidUrl.Value);

                MailSendDeal.MailSend(toMail, MsgCc.Text.Trim(), MsgBcc.Text.Trim(), txtSubject.Text.Trim(), scontent);

                switch (sType)
                {
                    case "1":            //���񵥻ط�
                        ZHServiceDP.UpdateEmailState(FlowID);
                        break;
                    default:
                        break;
                }
                PageTool.MsgBox(this, "�ʼ����ͳɹ���");
                PageTool.AddJavaScript(this, "window.close();");
            }
            else
            {
                PageTool.MsgBox(this, "��ѡ���ʼ������ˣ�");
            }
        }
    }
}
