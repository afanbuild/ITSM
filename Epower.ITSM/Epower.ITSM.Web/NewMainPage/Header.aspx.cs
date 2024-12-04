/*******************************************************************
 * 版权所有：
 * Description：主框架头部
 * 
 * 
 * Create By  ：
 * Create Date：2010-04-10
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
	/// Header 的摘要说明。
	/// </summary>
    public partial class Header : BasePage
	{
        /// <summary>
        /// 内容区显示页面
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
        /// 是否启用呼叫中心
        /// </summary>
        protected string sCallTel = "";
        protected string sBlockRoom = "";

        protected string strExchangeWeb = "";
        protected string sExitUrl = "";
        //SysSession sysSession=new SysSession ();
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)   //注册验证
            {
                //2008-07-11  校验许可
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
                        //提示 继续租用
                        Session["WarningPay"] = "true";
                    }
                }
            }

            if (Session["WarningPay"] != null)
            {
                //期限到了 提示警告
                lblWarning.Visible = true;
                lklLicense.Visible = true;
                Label1.Visible = false;
            }

            // 在此处放置用户代码以初始化页面
            if (CommonDP.GetConfigValue("Other", "E8Online") != null && CommonDP.GetConfigValue("Other", "E8Online") == "1")
            {
                sExitUrl = CommonDP.GetConfigValue("Other", "E8OnlineLogin");
            }
            else
            {
                sExitUrl = "../Default.aspx?Logout=1";
            }

            #region 语音系统参数 yxq 2011-08-16
            sCallTel = CommonDP.GetConfigValue("Other", "CallTel");
            GetDesk();//呼叫中心
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

                Label1.Text = " 欢迎您 ," + Session["PersonName"] + "[" + Session["UserDeptName"] + "]";

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
        /// 获取当前当前用户是否是服务台
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
                strtmp += "(图文)";
            }
            return strtmp;
        }

		#region Web 窗体设计器生成的代码
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);

		}    
		#endregion

        protected void btnRelegate_Click(object sender, ImageClickEventArgs e)
        {
            //刷新 去除 submenu的相关CACH
            Session["RemoveSubMenuCach"] = true;
            Response.Write("<Script>window.top.location.href='Index.aspx';</Script>");
        }
	}
}
