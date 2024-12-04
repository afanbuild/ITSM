/*******************************************************************
 * ��Ȩ���У�
 * Description��ҳ����࣬���е�ҳ��̳д�ҳ��
 * 
 * 
 * Create By  ��
 * Create Date��2008-08-20
 * *****************************************************************/
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web;
using System.Text;

using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization;
using Epower.DevBase.Organization.SqlDAL;
using System.Collections;

namespace Epower.ITSM.Web
{
    /// <summary>
    /// BasePage
    /// </summary>
    public class BasePage : System.Web.UI.Page
    {
        #region ������
        // �Ƿ���ʾ������
        bool mShowProgressBar = true;

        /// <summary>
        /// Ȩ�޶���
        /// </summary>
        private RightEntity _re;

        #endregion ����������

        #region ������
        #region #BaseRoot:string Ӧ�ó����·��������·����
        /// <summary>
        /// Ӧ�ó����·��������·����
        /// </summary>
        protected string BaseRoot
        {
            get { return Request.ApplicationPath; }
        }
        #endregion
        #endregion ����������

        #region BasePage
        /// <summary>
        /// constructor
        /// </summary>
        public BasePage()
        {

        }



        #endregion

        #region Page_PreInit
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Session["UserID"] == null) { Response.Redirect("~/default.aspx"); }

            if (Session["Themes"] == null)
            {
                string js = "<script type=\"javascript\">top.location.href = \"../default.aspx\";</script>";
                Page.RegisterStartupScript("redirectLogin", js);
                return;
            }
            this.Page.Theme = Session["Themes"].ToString();
        }

        protected override void OnError(EventArgs e)
        {
            HttpContext ctx = HttpContext.Current;

            Exception exception = ctx.Server.GetLastError();

            StringBuilder sbErrInfo = new StringBuilder();
            sbErrInfo.Append(Environment.NewLine + "=====================================" + System.DateTime.Now.ToString() + "=============================================");
            sbErrInfo.Append(Environment.NewLine + "��ǰ�û���: " + Session["UserName"] == null ? "empty" : Session["UserName"].ToString());
            sbErrInfo.Append(Environment.NewLine + "Offending URL: " + ctx.Request.Url.ToString());
            sbErrInfo.Append(Environment.NewLine + "Source: " + exception.Source);
            sbErrInfo.Append(Environment.NewLine + "Message: " + exception.Message);
            sbErrInfo.Append(Environment.NewLine + "Stack trace: " + exception.StackTrace);


            E8Logger.Error(sbErrInfo.ToString());
            sbErrInfo.Remove(0, sbErrInfo.ToString().Length);
            // --------------------------------------------------
            // To let the page finish running we clear the error
            // --------------------------------------------------
            ctx.Server.ClearError();
            HttpContext.Current.Response.Redirect(HttpContext.Current.Request.ApplicationPath + "/Error.aspx", true);
            base.OnError(e);
        }

        #endregion


        #region ĸ��ҳ

        /// <summary>
        /// ������ĸ��ҳ����
        /// </summary>
        protected EpowerMasterPage NormalMaster
        {
            get
            {
                return this.Master as EpowerMasterPage;
            }
        }

        /// <summary>
        /// ����ĸ��ҳ����
        /// </summary>
        protected FlowForms FlowMaster
        {
            get
            {
                return this.Master as FlowForms;
            }
        }

        #endregion

        #region ����Ȩ��

        /// <summary>
        /// ����Ȩ��
        /// </summary>
        /// <param name="lngOperateID">��������</param>
        protected void SetupRight(long lngOperateID)
        {
            _re = GetRight(lngOperateID);
        }

        /// <summary>
        /// ����Ȩ��
        /// </summary>
        /// <param name="lngOperateID">��������</param>
        /// <param name="isVerifyComeinRight">��֤����ҳ��Ȩ��</param>
        protected void SetupRight(long lngOperateID, bool isVerifyComeinRight)
        {
            _re = GetRight(lngOperateID);

            if (isVerifyComeinRight)
            {
                VerifyComeInRight();
            }
        }

        /// <summary>
        /// ȡȨ�޶���
        /// </summary>
        /// <param name="lngOperateID">��������</param>
        /// <returns></returns>
        protected RightEntity GetRight(long lngOperateID)
        {
            return (RightEntity)((Hashtable)Session["UserAllRights"])[lngOperateID];
        }

        /// <summary>
        /// ��֤����ҳ���Ȩ��
        /// </summary>
        /// <returns></returns>
        protected void VerifyComeInRight()
        {
            if (_re != null)
            {
                if (!this.CanRead())
                    throw new Exception(String.Format("Ȩ�޲�����url: {0}", Request.Url.ToString()));
            }
        }

        /// <summary>
        /// �Ƿ�ɼ�
        /// </summary>
        /// <returns></returns>
        protected bool CanAdd()
        {
            return _re.CanAdd;
        }

        /// <summary>
        /// �Ƿ�ɶ�
        /// </summary>
        /// <returns></returns>
        protected bool CanRead()
        {
            return _re.CanRead;
        }

        /// <summary>
        /// �Ƿ�ɱ༭
        /// </summary>
        /// <returns></returns>
        protected bool CanModify()
        {
            return _re.CanModify;
        }

        /// <summary>
        /// �Ƿ��ɾ��
        /// </summary>
        /// <returns></returns>
        protected bool CanDelete()
        {
            return _re.CanDelete;
        }

        #endregion
    }
}
