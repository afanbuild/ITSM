/*******************************************************************
 * ��Ȩ���У�
 * Description�������ͷ��
 * 
 * 
 * Create By  ��
 * Create Date��2010-04-10
 * *****************************************************************/
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
using System.Web.Security;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using Epower.DevBase.Organization;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.BaseTools;


namespace Epower.ITSM.Web.NewMainPage
{ 
	/// <summary>
	/// Header ��ժҪ˵����
	/// </summary>
    public partial class Header : BasePage
	{
        /// <summary>
        /// ��������ʾҳ��
        /// </summary>
        protected string StartPage
        {
            get
            {
                string strUserName = Session["UserName"].ToString().Trim();

                string sReturn = "../AppForms/frm_Services_List.aspx";
                Session["MainUrl"] = Constant.ApplicationPath + "/AppForms/frm_Services_List.aspx";
                if (strUserName.ToLower() == "sa")
                {
                    sReturn = "../DeptForms/frmMain.htm";
                }
                return sReturn;
            }
        }

        /// <summary>
        /// �Ƿ����ú�������
        /// </summary>
        protected string sCallTel = "";
        protected string sBlockRoom = "";

        protected string strExchangeWeb = "";
        protected string sExitUrl = "";
        //SysSession sysSession=new SysSession ();
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)   //ע����֤
            {
                //2008-07-11  У�����
                string sCustCode = "";
                string sProdCode = "";
                string sLicense = "-1";
                sLicense = SysRegEncry.WebLicenseDP.GetLicenseInfo(ref sCustCode, ref sProdCode, Session["UserName"].ToString());
                if (sLicense == "-1")
                {
                    Response.Write("<Script>window.top.location.href='../Common/frmSetLicense.aspx?cust=" + sCustCode.Trim() + "&prod=" + sProdCode.Trim() + "';</Script>");
                }
                else
                {
                    if (sLicense == "1")
                    {
                        //��ʾ ��������
                        Session["WarningPay"] = "true";
                    }
                }
            }

            if (Session["WarningPay"] != null)
            {
                //���޵��� ��ʾ����
                lblWarning.Visible = true;
                lklLicense.Visible = true;
                Label1.Visible = false;
            }

            // �ڴ˴������û������Գ�ʼ��ҳ��
            if (CommonDP.GetConfigValue("Other", "E8Online") != null && CommonDP.GetConfigValue("Other", "E8Online") == "1")
            {
                sExitUrl = CommonDP.GetConfigValue("Other", "E8OnlineLogin");
            }
            else
            {
                sExitUrl = "../Default.aspx?Logout=1";
            }

            #region ����ϵͳ���� yxq 2011-08-16
            sCallTel = CommonDP.GetConfigValue("Other", "CallTel");
            GetDesk();//��������
            #endregion

            if (!this.Page.IsPostBack)
            {
                string strTitle = CommonDP.GetConfigValue("SystemName", "SystemName");
                if (System.Configuration.ConfigurationManager.AppSettings["SystemModel"] == "1")
                {
                    Response.Write("<script>window.top.document.title='" + Session["RootDeptName"].ToString() +
                        strTitle + "'</script>");
                }
                else
                {
                    Response.Write("<script>window.top.document.title='" +
                        strTitle + "'</script>");
                }

                Label1.Text = " ��ӭ�� ," + Session["PersonName"] + "[" + Session["UserDeptName"] + "]";

                BindData();
            }
        }


        private void BindData()
        {
            //DataList RptNewsShow = (DataList)WPAfficle.FindControl("RptNewsShow");
            DataTable dt = NewsDp.GetBulletin(StringTool.String2Long(Session["UserOrgID"].ToString()), StringTool.String2Long(Session["UserDeptID"].ToString()));
            RptNewsShow.DataSource = dt.DefaultView;

            RptNewsShow.DataBind();

            if (dt.Rows.Count > 0)
            {
                //WPAfficle.Visible = true;
                RptNewsShow.Visible = true;
            }
        }

        /// <summary>
        /// ��ȡ��ǰ��ǰ�û��Ƿ��Ƿ���̨
        /// </summary>
        private void GetDesk()
        {
            Ea_SetdeskDP ee = new Ea_SetdeskDP();
            string sWhere = " AND UserID=" + Session["UserID"].ToString() + " AND UserName=" + StringTool.SqlQ(Session["PersonName"].ToString());
            string sOrder = "";

            DataTable dt = ee.GetDataTable(sWhere, sOrder);
            foreach (DataRow row in dt.Rows)
            {
                sBlockRoom = row["BlockRoom"].ToString();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngNewsID"></param>
        /// <returns></returns>
        protected string GetUrl(decimal lngNewsID)
        {

            return "~/Forms/ShowNews.aspx?&NewsID=" + lngNewsID.ToString().Trim();

        }
        protected string GetShowTitle(string strTitle, string strphoto)
        {
            string strtmp;
            strtmp = strTitle.ToString().Trim();
            if (strphoto.ToString().Trim() != "")
            {
                strtmp += "(ͼ��)";
            }
            return strtmp;
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
			this.Load += new System.EventHandler(this.Page_Load);

		}    
		#endregion

        protected void btnRelegate_Click(object sender, ImageClickEventArgs e)
        {
            //ˢ�� ȥ�� submenu�����CACH
            Session["RemoveSubMenuCach"] = true;
            Response.Write("<Script>window.top.location.href='Index.aspx';</Script>");
        }
	}
}
