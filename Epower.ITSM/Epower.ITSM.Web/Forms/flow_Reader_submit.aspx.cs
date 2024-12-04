using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using EpowerCom;
using EpowerGlobal;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Log;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.Business;

namespace Epower.ITSM.Web.Forms
{
    /// <summary>
    /// flow_Reader_submit ��ժҪ˵����
    /// </summary>
    public partial class flow_Reader_submit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            // �ڴ˴������û������Գ�ʼ��ҳ��
            long lngUserID;

            long lngMessageID = 0;
            string strOpinion = "";
            string strXmlValues = "";
            string sActorType = "1";   //��֪
            string sAttXml = "";      //����



            bool blnSuccess = true;



            string strMessage = "";

            lngUserID = (long)Session["UserID"];

            //��ֹ�û�ͨ��IE���˰�Ŧ�ظ��ύ
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);

            lngMessageID = long.Parse(Request.Form["MessageID"]);
            strOpinion = Request.Form["OpinionValue"];
            strXmlValues = Request.Form["FormXMLValue"];
            sAttXml = HttpUtility.UrlDecode(Request.Form["Attachment"]);
            sActorType = Request.Form["ActorType"];

            string strIsSaved = Request.QueryString["IsSaved"];

            try
            {
                if (strIsSaved == "no")
                {
                    if (sActorType == "2")
                    {
                        //2010.1.4 Э����Դ�����
                        Message.SetReadOver(lngUserID, lngMessageID, strOpinion, strXmlValues, sAttXml);
                    }
                    else
                    {
                        Message.SetReadOver(lngUserID, lngMessageID, strOpinion, strXmlValues);
                    }
                }
                else
                {
                    Message.SetReadTempSave(lngUserID, lngMessageID, strOpinion);
                }


                InfluxBS.SendMailForLastInfluxActor((long)Session["UserID"], lngMessageID, strXmlValues);
            
            }
            catch (Exception eSend)
            {
                strMessage = eSend.Message;
                blnSuccess = false;
            }



            if (strIsSaved == "no")
            {
                if (blnSuccess == true)
                {
                    //�Ķ���Э�����ͣ���ڴ�����ɵĽ���

                    Response.Redirect("flow_Finished.aspx?MessageID=" + lngMessageID.ToString());

                }
                else
                {
                    if (strMessage != "")
                    {
                        //���ַ����쳣�����
                        PageTool.MsgBox(this, strMessage.Replace("\r\n", ","));
                    }
                }


            }
            else
            {
                if (blnSuccess == true)
                {
                    PageTool.MsgBox(this, "�Ѿ����������Ϣ��");
                    Response.Write("<script>window.history.back();</script>");

                }
                else
                {
                    if (strMessage != "")
                    {
                        //���ַ����쳣�����
                        PageTool.MsgBox(this, strMessage.Replace("\r\n", ","));
                        //Response.Write("<script>window.history.back();</script>");
                        //return;
                    }
                }
            }

        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN���õ����� ASP.NET Web ���������������ġ�
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
        /// �˷��������ݡ�
        /// </summary>
        private void InitializeComponent()
        {
        }
        #endregion
    }
}
