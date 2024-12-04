/****************************************************************************
 * 
 * description:�豸�����ҳ��
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
using Epower.DevBase.Organization.Base;

namespace Epower.ITSM.Web.EquipmentManager
{
    /// <summary>
    /// 
    /// </summary>
    public partial class frmEqu_Content : BasePage
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
                if (sType == "0")          //�豸����ά��
                {
                    sReturn = "frmSubjectedit.aspx";
                }
                else if (sType == "1")     //�豸ά��
                {
                    sReturn = "frmEqu_DeskMain.aspx";
                }
                else if (sType == "2")     //�ʲ�Ŀ¼����
                {
                    sReturn = "frmEqu_DeskCateList.aspx";
                }
                else if (sType == "3")     // �ʲ���ع������
                {
                    sReturn = "frmEqu_Monitoring_Rule_Resource_List.aspx";
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
                JumpToOtherPage();
            }
        }

        private void JumpToOtherPage()
        {
            string SubjectID = "1";
            if (Request.QueryString["CurrSubjectID"] != null)
            {
                SubjectID = Request.QueryString["CurrSubjectID"].ToString();
                Session["OldEQSubectID"] = SubjectID;
            }
            if (Session["OldEQSubectID"] != null)
            {
                SubjectID = Session["OldEQSubectID"].ToString();
            }

            if (sType == "3")
            {
                Response.Write("<SCRIPT>window.parent.list.location='" + sUrl + "?subjectid=" + SubjectID + "';</SCRIPT>");
            }
            else
                Response.Write("<SCRIPT>window.parent.subjectinfo.location='" + sUrl + "?subjectid=" + SubjectID + "';</SCRIPT>");
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
