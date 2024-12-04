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
    public partial class frmCheck : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string LoginID = "";
            string sPassword = "";
            LoginID = Request.QueryString["username"];
            sPassword = Request.QueryString["password"];

           bool logined = Check_DomainUser(LoginID,sPassword);
           if (logined == true)
           {
               hidLogined.Value = "true";
               hidUser.Value = LoginID;
               hidPsw.Value = sPassword;
           }
           else
           {

           }
          // Response.Redirect("Forms/frmContentCheck.aspx?LastMessageID=100&CheckType=1");
        }


        /// <summary>
        /// 域登录
        /// </summary>
        /// <param name="LoginID"></param>
        private bool Check_DomainUser(string LoginID,string sPass)
        {
            int Result = -3;
            bool ret = false;
            try
            {
                LogSession objSession = new LogSession();
                Result = objSession.Login(LoginID.Trim(),sPass);
                switch (Result)
                {
                    case 0:
                        Session["SystemID"] = long.Parse(System.Configuration.ConfigurationSettings.AppSettings["DefaultSystem"]);
                        //清除临时目录中没有删除完毕的
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
                            Session["OldDeptID"] = ((oUserDept)ud[0]).DeptID;//默认当前部门，用于部门树
                            Session["UserDeptName"] = DeptDP.GetDeptName(((oUserDept)ud[0]).DeptID);
                            //机构ID
                            Session["UserOrgID"] = DeptDP.GetDirectOrg(((oUserDept)ud[0]).DeptID);
                        }
                        else
                        {
                            Session["UserDeptID"] = 1;
                            Session["OldDeptID"] = 1;
                            Session["UserDeptName"] = "";
                            //机构ID
                            Session["UserOrgID"] = 1;
                        }

                        //租用系统时有用
                        Session["RootDeptName"] = DeptDP.GetDeptName(objSession.RootDeptID);

                        Session["UserAllRights"] = RightDP.getUserRightTable(objSession.UserID);


                        if (CommonDP.GetConfigValue("Other", "ItsmCustomerMode") == "0")
                        {
                            //内部模式设置缺省的 缺省客户资料
                            Br_ECustomerDP ec = new Br_ECustomerDP();
                            string[] sCust = ec.GetRefUserIDAndName(long.Parse(Session["UserID"].ToString()));

                            Session["UserDefaultCustomerName"] = sCust[1];
                            Session["UserDefaultCustomerID"] = sCust[0];
                        }

                        FormsAuthentication.SignOut();

                        FormsAuthentication.SetAuthCookie(LoginID, false);

                        Response.Cookies["CyanineOAUserName"].Value = Server.UrlEncode(LoginID);

                        Session["Themes"] = "StandardThemes";
                        Session["MainUrl"] = "NewMainPage/Index.aspx";
                        ret = true;
                        break;
                    case -1:
                        //PageTool.MsgBox(this,"登陆的域用户在OA系统中不存在，请登录!");						
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
