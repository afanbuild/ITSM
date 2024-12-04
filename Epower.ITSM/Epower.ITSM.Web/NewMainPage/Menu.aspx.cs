/*******************************************************************
 * ��Ȩ���У�
 * Description���˵�
 * 
 * 
 * Create By  ��
 * Create Date��2010-04-10
 * *****************************************************************/
using System;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;


using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.NewMainPage
{
    /// <summary>
    /// Menu ��ժҪ˵����
    /// </summary>
    public partial class Menu : BasePage
    {
        /// <summary>
        /// 
        /// </summary>
        protected string FirstLeftUrl
        {
            get { return ViewState["firstlefturl"].ToString(); }
            set { ViewState["firstlefturl"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            // �ڴ˴������û������Գ�ʼ��ҳ��
            if (!Page.IsPostBack)
            {
                if (Session["UserName"].ToString().Trim() == "sa")
                {
                    Response.Redirect("../NewOldMainPage/index.aspx");
                }
            }


            #region ��ʾ��Ҫ�����Ĺ���

            if (ConfigurationManager.AppSettings["PopupAlertWindowWhenLogin"] == null || ConfigurationManager.AppSettings["PopupAlertWindowWhenLogin"].Equals("0"))
            {
                return;
            }

            //��ʾ��Ҫ�����Ĺ���
            DataTable dt = NewsDp.GetIsAlterNews(StringTool.String2Long(Session["UserOrgID"].ToString()), StringTool.String2Long(Session["UserDeptID"].ToString()));
            if (dt != null && dt.Rows.Count > 0)
            {
                literalPopupWindow.Visible = true;
            }
            #endregion
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
