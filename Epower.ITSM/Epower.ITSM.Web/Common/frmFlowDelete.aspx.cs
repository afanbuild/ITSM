/*******************************************************************
 * ��Ȩ���У�
 * Description��ɾ������
 * 
 * 
 * Create By  ��
 * Create Date��2007-11-21
 * *****************************************************************/
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
using EpowerCom;
using System.Text;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.Common
{
    public partial class frmFlowDelete : BasePage
    {
        private long lngID=0;   //����ʵ����š�ע�� ��������ֹʱ��������ϢID,MessageID��

        private bool blnAbort = false;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["isAbort"] != null)
            {
                if (Request["isAbort"].ToString() == "true")
                {
                    blnAbort = true;
                    Page.Title = "��ֹ����";
                }
            }
        }
        #region ��ȡ���������ذ�ť��id
        public string Opener_hiddenDelete
        {
            get
            {

                if (Request.QueryString["Opener_hiddenDelete"] != null)
                {
                    return Request.QueryString["Opener_hiddenDelete"];
                }
                else
                {
                    return "";
                }
            }
        }
        /// <summary>
        /// �ж��ĸ�ҳ�洫������
        /// </summary>
        public string TypeFrm
        {
            get
            {

                if (Request.QueryString["TypeFrm"] != null)
                {
                    return Request.QueryString["TypeFrm"];
                }
                else
                {
                    return "";
                }
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            Message msg = new Message();
            ZHServiceDP dp = new ZHServiceDP();
            if (Request["FlowID"] != null)  //��ֹʱ������MessageID
                lngID = long.Parse(Request["FlowID"].ToString());

            string cstno = dp.GetCstIessNo(Request["FlowID"].ToString());//��ѯ��Ӧ���¼�
           

            if (blnAbort == true)
            {
                msg.AutoStopFlow((long)Session["UserID"],lngID, txtRemark.Text.Trim());
            }
            else
            {
                msg.AdminDeleteFlow(lngID, (long)Session["UserID"], txtRemark.Text.Trim());
            }

            if (cstno != "")
            {
                DeleteEmail(cstno);
            }

            //�رմ���
            StringBuilder sbText = new StringBuilder();
            sbText.Append("<script>");
            sbText.Append("var arr = new Array();");
            // ʧ��
            sbText.Append("arr[0] ='0';");
            // ����
            if (TypeFrm == "cst_Issue_list" || TypeFrm == "frmFlow_QuestHouseQuery" || TypeFrm == "frm_KBBaseQuery")
            {
                sbText.Append("window.opener.document.getElementById('" + Opener_hiddenDelete + "').click();");
            }
           // sbText.Append("window.returnValue = arr;");
            // �رմ���
            sbText.Append("top.close();");
            sbText.Append("</script>");
            // ��ͻ��˷���
            Page.RegisterStartupScript(DateTime.Now.ToString(), sbText.ToString());
           // Response.Write(sbText.ToString());
        }


        #region ɾ���ѷ��ʼ���¼
        public void DeleteEmail(string cstno)
        {
            ZHServiceDP dp = new ZHServiceDP();

            if (cstno != "")
            {
                dp.DeleteEmail(cstno);//ɾ���ѷ��ʼ�
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //�رմ���
            StringBuilder sbText = new StringBuilder();
            sbText.Append("<script>");
            sbText.Append("var arr = new Array();");
            // ʧ��
            sbText.Append("arr[0] ='1';");
            // ����
            sbText.Append("window.returnValue = arr;");
            // �رմ���
            sbText.Append("top.close();");
            sbText.Append("</script>");
            // ��ͻ��˷���
            Page.RegisterStartupScript(DateTime.Now.ToString(), sbText.ToString());
            Response.Write(sbText.ToString());
        }
    }
}
