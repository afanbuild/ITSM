using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;
using Epower.ITSM.SqlDAL;
using System.Configuration;


namespace Epower.ITSM.Web
{
    /// <summary>
    /// _Default ��ժҪ˵����
    /// </summary>
    public partial class default1 : System.Web.UI.Page
    {
        /// <summary>
        /// ����
        /// </summary>
        protected string Title
        {
            get
            {
                return CommonDP.GetConfigValue("SystemName", "SystemName");
            }
        }

        /// <summary>
        /// ���¼��
        /// </summary>
        protected string LoginID { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        protected string DomainName { get; set; }

        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            string sGuid = Request.QueryString["Token"];
            if (sGuid != null)
            {
                string slogName = string.Empty;
                string sE8Theme = string.Empty;

                LogSession objSession = new LogSession();
                objSession.GetE8OnlineLogin(sGuid, ref slogName, ref sE8Theme);
                if (slogName != string.Empty)
                {
                    loginAuto(slogName, sE8Theme, objSession);
                }
                else
                {
                    Response.Redirect(CommonDP.GetConfigValue("Other", "E8OnlineLogin"));
                }
            }
            else
            {
                if (!this.IsPostBack)
                {
                    //wangxiaohuo,������¼����ֵ�û���
                    string[] strUserInfo = User.Identity.Name.Split('\\');
                    LoginID = strUserInfo[strUserInfo.Length - 1];
                    DomainName = User.Identity.Name.Replace("\\" + LoginID, "");

                    if (!string.IsNullOrEmpty(LoginID))
                    {
                        loginDomain();
                    }

                    string sUserID = "E8HelpDeskThemes";
                    HttpCookie cookie = Request.Cookies[sUserID];
                    if (cookie != null)
                    {
                        string svalue = cookie.Value.ToString();
                    }

                    #region ��ȡCookie�����ֵ yxq 2011-09-06
                    string UserName = MClass.GetCookie("userid"); //�û���
                    string pwd = MClass.GetCookie("pwd");//����
                    string identity = MClass.GetCookie("identity"); //���
                    drIdentity.SelectedIndex = drIdentity.Items.IndexOf(drIdentity.Items.FindByValue(identity == "" ? "0" : identity));
                    #endregion
                }
            }
        }
        
        /// <summary>
        /// sso�Զ���¼
        /// </summary>
        /// <param name="sName"></param>
        /// <param name="themes"></param>
        /// <param name="objSession"></param>
        private void loginAuto(string sName, string themes, LogSession objSession)
        {
            try
            {
                int Result = objSession.Login(sName);
                switch (Result)
                {
                    case 0:
                        //��·��
                        if (HttpRuntime.Cache["E8ApplicationPath"] == null)
                        {
                            HttpRuntime.Cache["E8ApplicationPath"] = Request.ApplicationPath;
                        }

                        Session["SystemID"] = long.Parse(System.Configuration.ConfigurationSettings.AppSettings["DefaultSystem"]);
                        //�����ʱĿ¼��û��ɾ����ϵ�
                        string strTmpCatalog = CommonDP.GetConfigValue("TempCataLog", "TempCataLog");
                        EpowerCom.MyFiles.ClearHistoryTempDir(strTmpCatalog);
                        EpowerCom.UserGroupDP.ClearHistoryTempUser();

                        Session["UserName"] = objSession.UserName;
                        Session["PersonName"] = objSession.Name;
                        Session["UserID"] = objSession.UserID;
                        Session["AgentStatus"] = objSession.AgentStatus;
                        Session["RootDeptID"] = objSession.RootDeptID;
                        Session["RangeID"] = objSession.RangeID;


                        UserDeptCollection ud = UserControls.GetUserIncludedDept(objSession.UserID);
                        if (ud.Count > 0)
                        {
                            Session["UserDeptID"] = ((oUserDept)ud[0]).DeptID;
                            Session["OldDeptID"] = ((oUserDept)ud[0]).DeptID;//Ĭ�ϵ�ǰ���ţ����ڲ�����
                            Session["UserDeptName"] = DeptDP.GetDeptName(((oUserDept)ud[0]).DeptID);
                            //����ID
                            Session["UserOrgID"] = DeptDP.GetDirectOrg(((oUserDept)ud[0]).DeptID);
                        }
                        else
                        {
                            Session["UserDeptID"] = 1;
                            Session["OldDeptID"] = 1;
                            Session["UserDeptName"] = "";
                            //����ID
                            Session["UserOrgID"] = 1;
                        }

                        //����ϵͳʱ����
                        Session["RootDeptName"] = DeptDP.GetDeptName(objSession.RootDeptID);

                        Session["UserAllRights"] = RightDP.getUserRightTable(objSession.UserID);

                        //�ڲ�ģʽ����ȱʡ�� ȱʡ�ͻ�����
                        Br_ECustomerDP ec = new Br_ECustomerDP();
                        string[] sCust = ec.GetRefUserIDAndName(long.Parse(Session["UserID"].ToString()));

                        Session["UserDefaultCustomerName"] = sCust[1];
                        Session["UserDefaultCustomerID"] = sCust[0];
                       
                        //�ͻ����ϣ��ʲ����Ϸ�Χ����
                        if (CommonDP.GetConfigValue("Other", "DataLimit") != null && CommonDP.GetConfigValue("Other", "DataLimit") == "1")
                        {
                            string sMastLimitList = EA_ExtendRightsDP.getUserOtherRight(10, long.Parse(Session["UserID"].ToString()));
                            Session["MastLimitList"] = sMastLimitList;
                        }
                        //֪ʶ�ּ�����
                        if (CommonDP.GetConfigValue("Other", "InformationLimit") != null && CommonDP.GetConfigValue("Other", "InformationLimit") == "1")
                        {
                            string sMastLimitList = EA_ExtendRightsDP.getUserOtherRight(20, long.Parse(Session["UserID"].ToString()));
                            Session["InformationLimitList"] = sMastLimitList;
                        }

                        //�ʲ��ּ�����
                        string sEquLimitList = EA_ExtendRightsDP.getUserOtherRight(30, long.Parse(Session["UserID"].ToString()));
                        Session["EquLimitList"] = sEquLimitList;

                        FormsAuthentication.SignOut();
                        FormsAuthentication.SetAuthCookie(txtUserName.Text.Trim(), false);
                        Response.Cookies["CyanineOAUserName"].Value = Server.UrlEncode(txtUserName.Text.Trim());

                        Session["Themes"] = "NewOldMainPage";
                        Session["MainUrl"] = "NewOldMainPage/Index.aspx";
                        Response.Redirect(Session["MainUrl"].ToString());

                        break;
                    case -1:
                        Response.Write(@"<script language='javascript'>alert('�û������ڣ�����������'); </script> ");
                        Response.Redirect(CommonDP.GetConfigValue("Other", "E8OnlineLogin"));

                        break;
                    case -2:
                        Response.Write(@"<script language='javascript'>alert('�û������������������'); </script> ");
                        Response.Redirect(CommonDP.GetConfigValue("Other", "E8OnlineLogin"));
                        break;
                }

            }
            catch (Exception e1)
            {
                if (e1.Message == "���ݿ����ӳ���")
                {
                    Response.Redirect("SetConnInfo.aspx");
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// ����֤�Զ���¼
        /// </summary>
        private void loginDomain()
        {
            int Result = -3;
            LogSession objSession = new LogSession();

            //wangxiaohuo,���¼,����֤�û��Ƿ��ڱ�ϵͳ�д���
            string domainServerName = System.Configuration.ConfigurationSettings.AppSettings["DomainServerName"];
            string domainName = System.Configuration.ConfigurationSettings.AppSettings["DomainName"];
            if (!string.IsNullOrEmpty(domainServerName)
                && !string.IsNullOrEmpty(domainName)
                && txtUserName.Text.Trim().ToLower() != "sa")
            {
                try
                {
                    txtUserName.Text = LoginID;
                    String adPath = string.Format("LDAP://{0}.{1}", domainServerName, domainName); //Fully-qualified Domain Name
                    LdapAuthentication adAuth = new LdapAuthentication(adPath);

                    Configuration conn = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("");
                    System.Web.Configuration.AuthenticationSection section = (System.Web.Configuration.AuthenticationSection)conn.SectionGroups.Get("system.web").Sections.Get("authentication");

                    if (section.Mode == System.Web.Configuration.AuthenticationMode.Windows)
                    {
                        //����֤��ֱ�ӵ�¼
                        if (domainName.ToUpper().IndexOf(DomainName.ToUpper()) >= 0)
                        {
                            Result = objSession.Login(txtUserName.Text.Trim());
                        }
                    }
                    else
                    {
                        if (true == adAuth.IsAuthenticated(domainName, txtUserName.Text.Trim(), txtPassword.Text.Trim()))
                        {
                            Result = objSession.Login(txtUserName.Text.Trim());
                        }
                    }

                    loginResult(Result, objSession);
                }
                catch (Exception epp)
                {
                    hidFlag.Value = "-2";
                    Response.Write(@"<script language='javascript'>alert('" + epp.Message + "'); </script> ");
                }
            }
        }

        /// <summary>
        /// ����֤���ڵ�¼
        /// </summary>
        private void loginDomainForm()
        {
            int Result = -3;
            LogSession objSession = new LogSession();

            //wangxiaohuo,���¼
            string domainServerName = System.Configuration.ConfigurationSettings.AppSettings["DomainServerName"];
            string domainName = System.Configuration.ConfigurationSettings.AppSettings["DomainName"];
            if (!string.IsNullOrEmpty(domainServerName)
                && !string.IsNullOrEmpty(domainName)
                && txtUserName.Text.Trim().ToLower() != "sa")
            {
                try
                {
                    UserLoginForDomain CheckUserLogin = new UserLoginForDomain();
                    if (CheckUserLogin.impersonateValidUser(txtUserName.Text.Trim(), domainName, txtPassword.Text))
                    {
                        Result = objSession.Login(txtUserName.Text.Trim());
                        loginResult(Result, objSession);
                    }                   
                }
                catch (Exception epp)
                {
                    hidFlag.Value = "-2";
                    Response.Write(@"<script language='javascript'>alert('" + epp.Message + "'); </script> ");
                }
            }
        }


        /// <summary>
        /// ��¼����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loginResult(int Result, LogSession objSession)
        {
            switch (Result)
            {
                case 0:

                    #region ����Cookies��Ϣ yxq 2011-09-06
                    MClass.SaveCookie(txtUserName.Text.Trim(), "", drIdentity.SelectedItem.Value); //������ʱ������
                    #endregion

                    //������
                    if (Application["FlowCacheDate"] != null)
                    {
                        DateTime cachedate = DateTime.Parse(Application["FlowCacheDate"].ToString());
                        if (cachedate.AddMinutes(5) <= DateTime.Now)
                        {
                            //��ȡOracle���ݿ⣬�ж��Ƿ��и���
                            string sreturn = EpowerGlobal.EpSqlCacheHelper.GetSQLCache();
                            string[] sarr = sreturn.Split(',');
                            for (int n = 0; n < sarr.Length - 1; n++)
                            {
                                switch (sarr[n].ToString().ToLower())
                                {
                                    case "flow":
                                        HttpRuntime.Cache["EpCacheValidFlowModel"] = false;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            Application["FlowCacheDate"] = DateTime.Now.ToString();
                        }
                    }
                    else
                    {
                        Application["FlowCacheDate"] = DateTime.Now.ToString();
                        HttpRuntime.Cache["EpCacheValidFlowModel"] = false;
                        Application["FlowCacheDate"] = DateTime.Now.ToString();
                    }

                    Session["SystemID"] = long.Parse(System.Configuration.ConfigurationSettings.AppSettings["DefaultSystem"]);
                    //�����ʱĿ¼��û��ɾ����ϵ�
                    string strTmpCatalog = CommonDP.GetConfigValue("TempCataLog", "TempCataLog");
                    EpowerCom.MyFiles.ClearHistoryTempDir(strTmpCatalog);
                    EpowerCom.UserGroupDP.ClearHistoryTempUser();

                    Session["UserName"] = objSession.UserName;
                    Session["PersonName"] = objSession.Name;
                    Session["UserID"] = objSession.UserID;
                    Session["AgentStatus"] = objSession.AgentStatus;
                    Session["RootDeptID"] = objSession.RootDeptID;
                    Session["RangeID"] = objSession.RangeID;

                    UserDeptCollection ud = UserControls.GetUserIncludedDept(objSession.UserID);
                    if (ud.Count > 0)
                    {
                        Session["UserDeptID"] = ((oUserDept)ud[0]).DeptID;
                        Session["OldDeptID"] = ((oUserDept)ud[0]).DeptID;//Ĭ�ϵ�ǰ���ţ����ڲ�����
                        Session["UserDeptName"] = DeptDP.GetDeptName(((oUserDept)ud[0]).DeptID);
                        //����ID
                        Session["UserOrgID"] = DeptDP.GetDirectOrg(((oUserDept)ud[0]).DeptID);
                    }
                    else
                    {
                        Session["UserDeptID"] = 1;
                        Session["OldDeptID"] = 1;
                        Session["UserDeptName"] = "";
                        //����ID
                        Session["UserOrgID"] = 1;
                    }

                    //����ϵͳʱ����
                    Session["RootDeptName"] = DeptDP.GetDeptName(objSession.RootDeptID);

                    Session["UserAllRights"] = RightDP.getUserRightTable(objSession.UserID);


                    //�ڲ�ģʽ����ȱʡ�� ȱʡ�ͻ�����
                    Br_ECustomerDP ec = new Br_ECustomerDP();
                    string[] sCust = ec.GetRefUserIDAndName(long.Parse(Session["UserID"].ToString()));

                    Session["UserDefaultCustomerName"] = sCust[1];
                    Session["UserDefaultCustomerID"] = sCust[0];
                    //}
                    //�ͻ����ϣ��ʲ����Ϸ�Χ����
                    if (CommonDP.GetConfigValue("Other", "DataLimit") != null && CommonDP.GetConfigValue("Other", "DataLimit") == "1")
                    {
                        string sMastLimitList = EA_ExtendRightsDP.getUserOtherRight(10, long.Parse(Session["UserID"].ToString()));
                        Session["MastLimitList"] = sMastLimitList;
                    }
                    //֪ʶ�ּ�����
                    if (CommonDP.GetConfigValue("Other", "InformationLimit") != null && CommonDP.GetConfigValue("Other", "InformationLimit") == "1")
                    {
                        string sMastLimitList = EA_ExtendRightsDP.getUserOtherRight(20, long.Parse(Session["UserID"].ToString()));
                        Session["InformationLimitList"] = sMastLimitList;
                    }

                    //�ʲ��ּ�����
                    string sEquLimitList = EA_ExtendRightsDP.getUserOtherRight(30, long.Parse(Session["UserID"].ToString()));
                    Session["EquLimitList"] = sEquLimitList;

                    FormsAuthentication.SignOut();

                    FormsAuthentication.SetAuthCookie(txtUserName.Text.Trim(), false);

                    Response.Cookies["CyanineOAUserName"].Value = Server.UrlEncode(txtUserName.Text.Trim());

                    if (drIdentity.SelectedItem.Value == "0")
                    {
                        Session["Themes"] = "NewOldMainPage";
                        Session["MainUrl"] = "NewOldMainPage/Index.aspx";
                    }
                    else
                    {
                        Session["Themes"] = "NewOldMainPage";
                        Session["MainUrl"] = "NewMainPage/Index.aspx";
                    }

                    if (txtUserName.Text.ToLower() == "sa")   //��������Ա
                    {
                        Session["Themes"] = "NewOldMainPage";
                        Session["MainUrl"] = "NewOldMainPage/Index.aspx";
                    }


                    //��¼��־
                    Epower.ITSM.Log.OperateLog.PostOperateLog(System.DateTime.Now.ToString(), ((int)Epower.ITSM.Base.eOperLog_ActionType.eLoginLog).ToString(), Session["UserDeptName"].ToString() + "[" + Session["PersonName"].ToString() + "]��¼ϵͳ", "0", "Default");

                    Response.Redirect(Session["MainUrl"].ToString());
                    break;
                case -1:
                    hidFlag.Value = "-1";
                    Response.Write(@"<script language='javascript'>alert('�û������ڣ�����������'); </script> ");

                    break;
                case -2:
                    hidFlag.Value = "-2";
                    Response.Write(@"<script language='javascript'>alert('�û������������������'); </script> ");
                    break;
            }
        }

        /// <summary>
        /// ��¼��ť
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdOK_Click(object sender, EventArgs e)
        {
            hidFlag.Value = string.Empty;
            int Result = -3;

            if (txtUserName.Text.Trim() == "")
            {
                PageTool.MsgBox(this, "�����������û�����");
                return;
            }
            try
            {
                loginDomainForm();

                LogSession objSession = new LogSession();
                Result = objSession.Login(txtUserName.Text.Trim(), txtPassword.Text.Trim());
                loginResult(Result, objSession);

            }
            catch (Exception e1)
            {
                if (e1.Message == "���ݿ����ӳ���")
                {
                    Response.Redirect("SetConnInfo.aspx");
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void WriteCookie(string sParams)
        {
            string sUserID = "E8HelpDeskThemes";
            Response.Cookies[sUserID].Value = sParams;
            Response.Cookies[sUserID].Path = "/";           
            Response.Cookies[sUserID].Expires = DateTime.Now.AddYears(1);
        }
    }
}
