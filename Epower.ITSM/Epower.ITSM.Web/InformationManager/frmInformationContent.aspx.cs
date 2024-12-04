/****************************************************************************
 * 
 * description:��Ŀ�����ҳ��
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-08-22
 * *************************************************************************/
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
using Microsoft.Web.UI.WebControls;
using Epower.DevBase.Organization;
using Epower.DevBase.Organization.Base ;

namespace Epower.ITSM.Web.InformationManager
{
    /// <summary>
    /// 
    /// </summary>
    public partial class frmInformationContent : BasePage
	{
        /// <summary>
        /// ȡ��ҳ��չʾ�����
        /// </summary>
        protected string sType
        {
            get
            {
                if (Request["Type"] != null)
                    return Request["Type"].ToString();
                else
                    return "0";
            }
        }

        /// <summary>
        /// չʾ��ַ
        /// </summary>
        protected string sUrl
        {
            get
            {
                string sReturn = string.Empty;
                if (sType == "0")          //֪ʶ������ά��
                {
                    sReturn = "frmSubjectedit.aspx?1=1";
                }
                else if(sType =="1")  //֪ʶ��ά��
                {
                    sReturn = "frmInf_InformationMain.aspx?1=1";
                }
                else if (sType == "2")  //֪ʶ��鿴
                {
                    sReturn = "frmInf_InformationMain.aspx?IsGaoji=true";
                }
                else   //֪ʶ��鿴
                {
                    sReturn = "frmInf_InformationMain.aspx?IsSelect=1&IsGaoji=true";
                }
                return sReturn;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Page_Load(object sender, System.EventArgs e)
		{

            if (!this.IsPostBack)
            {
                if (sType == "0" || sType == "1")   //�����Ϊ֪ʶ���ά����֪ʶά��
                    CtrSubjecttree1.InformationLimit = false;
                else
                    CtrSubjecttree1.InformationLimit = true;
                string SubjectID = "1";
                if (Request.QueryString["CurrSubjectID"] != null)
                {
                    SubjectID = Request.QueryString["CurrSubjectID"].ToString();
                }
                if (Session["OldSubjectID"] != null)
                {
                    SubjectID = Session["OldSubjectID"].ToString();
                }
                Response.Write("<SCRIPT>window.parent.subjectinfo.location='" + sUrl + "&subjectid=" + SubjectID + "';</SCRIPT>");
            }
		}
		

		#region Web ������������ɵĴ���
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: �õ����� ASP.NET Web ���������������ġ�
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
