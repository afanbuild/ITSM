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
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web
{
    public partial class LoginAuto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string LoginID = "";
            string sPassword = "";

            //�Զ���¼�Ĵ��� һ�㲻���õ�. ����������õ�
            if (Request.QueryString["username"] != null)
            {
                LoginID = Request.QueryString["username"];
                sPassword = Request.QueryString["password"];

                bool logined = Check_DomainUser(LoginID, sPassword);
            }
        }


        /// <summary>
        /// ���¼
        /// </summary>
        /// <param name="LoginID"></param>
        private bool Check_DomainUser(string LoginID, string sPass)
        {
            int Result = -3;
            bool ret = false;
            try
            {
                LogSession objSession = new LogSession();
                Result = objSession.Login(LoginID.Trim(), sPass);
                switch (Result)
                {
                    case 0:
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


                        if (CommonDP.GetConfigValue("Other", "ItsmCustomerMode") == "0")
                        {
                            //�ڲ�ģʽ����ȱʡ�� ȱʡ�ͻ�����
                            Br_ECustomerDP ec = new Br_ECustomerDP();
                            string[] sCust = ec.GetRefUserIDAndName(long.Parse(Session["UserID"].ToString()));

                            Session["UserDefaultCustomerName"] = sCust[1];
                            Session["UserDefaultCustomerID"] = sCust[0];
                        }
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

                        FormsAuthentication.SetAuthCookie(LoginID, false);

                        Response.Cookies["CyanineOAUserName"].Value = Server.UrlEncode(LoginID);

                        Session["Themes"] = "NewOldMainPage";
                        Session["MainUrl"] = "NewOldMainPage/Index.aspx";

                        string sParam = string.Empty;
                        if (Request["Param"] != null)
                        {
                            sParam = Request["Param"].ToString();
                        }
                        switch(sParam)
                        {
                            case "":
                                //��¼��־
                                //Epower.ITSM.Log.OperateLog.PostOperateLog(System.DateTime.Now.ToString(), ((int)Epower.ITSM.Base.eOperLog_ActionType.eLoginLog).ToString(), Session["UserDeptName"].ToString() + "[" + Session["PersonName"].ToString() + "]�Զ���¼ϵͳ", "0", "LoginAuto");

                                Response.Redirect("NewOldMainPage/Index.aspx");
                                break;
                            case "1":
                                Session["DeskAssistantFlag"] = "true";
                                Session["FromUrl"] = "close";
                                break;
                            default:
                                break;
                        }
                        ret = true;
                        break;
                    case -1:
                        //PageTool.MsgBox(this,"��½�����û���OAϵͳ�в����ڣ����¼!");						
                        ret = false;
                        break;
                }



            }
            catch (Exception e1)
            {
                ret = false;
            }

            return ret;
        }
    }
}
