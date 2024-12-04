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

namespace  Epower.ITSM.Web.MyDestop
{
	/// <summary>
	/// frmPopDept ��ժҪ˵����
	/// </summary>
    public partial class frmPopDept : BasePage
	{
        #region �Ƿ�����ѡ��Χ
        /// <summary>
        /// 
        /// </summary>
        private bool mIsLimit = false;
        /// <summary>
        /// �Ƿ����Ʋ��ŷ�Χ
        /// </summary>
        public bool IsLimit
        {
            get
            {
                if (Request.QueryString["LimitCurr"] != null && Request.QueryString["LimitCurr"].ToLower() == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion 

        /// <summary>
        /// ��ȡ·�������
        /// </summary>

        public string TypeFrm
        {
            get{
               
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
        public string Opener_ClientId
        {
           
            get
            {
                return (Request["Opener_ClientId"] == null) ? "" : Request["Opener_ClientId"].ToString().Replace("hidClientId_ForOpenerPage", "");
            }
        }

        private long mlngCurrDeptID = 1;
        protected long lngCurrDeptID 
        {
            get
            {
                if (Request.QueryString["CurrDeptID"] != null)
                {
                    if (Request.QueryString["CurrDeptID"].Length > 0)
                        mlngCurrDeptID = long.Parse(Request.QueryString["CurrDeptID"]);
                    else
                        mlngCurrDeptID = long.Parse(Session["UserDeptID"].ToString());
                }
                else
                {
                    mlngCurrDeptID = long.Parse(Session["UserDeptID"].ToString());
                }
                return mlngCurrDeptID;
            }
        }
		protected void Page_Load(object sender, System.EventArgs e)
		{
            if (!IsPostBack) 
            {
                if (Request["TypeFrm"] != null)
                {
                   // TypeFrm = Request.QueryString["TypeFrm"].ToString();
                }
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
