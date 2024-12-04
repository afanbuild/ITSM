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
using Epower.ITSM.SqlDAL;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization;
using Epower.DevBase.Organization.Base;


namespace Epower.ITSM.Web.Common
{
    public partial class frmDoAction : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            long lngUserID = long.Parse(Request.QueryString["userid"]);
            string sGuid = Request.QueryString["guid"];

            string sIntention = "";
            long lngmnUserID = 0;
            sIntention = APTokenDP.GetTokenInfo20111104(lngUserID, sGuid, "", ref lngmnUserID);

            if (lngmnUserID == 0)
                lngmnUserID = lngUserID;

            if (sIntention != "")
            {
                //��ʾ���ҵ�������
                TokenIntention ti = new TokenIntention(sIntention);

                //ģ���¼
                UserEntity user = new UserEntity(lngmnUserID);

                DoLoginAuto(user.LoginName, ti);



            }
            else
            {
                Response.Redirect(Epower.ITSM.Base.Constant.ApplicationPath + "/default.aspx");
            }

        }

        /// <summary>
        /// �Զ���¼��������Ӧ��ҳ��
        /// </summary>
        /// <param name="sLoginName"></param>
        /// <param name="ti"></param>
        private void DoLoginAuto(string sLoginName, TokenIntention ti)
        {
            int Result = -3;
            try
            {
                LogSession objSession = new LogSession();
                Result = objSession.Login(sLoginName.Trim());
                switch (Result)
                {
                    case 0:
                        Session["SystemID"] = long.Parse(System.Configuration.ConfigurationSettings.AppSettings["DefaultSystem"]);
                        //�����ʱĿ¼��û��ɾ����ϵ�
                        string strTmpCatalog = CommonDP.GetConfigValue("TempCataLog", "TempCataLog");
                        EpowerCom.MyFiles.ClearHistoryTempDir(strTmpCatalog);
                        EpowerCom.UserGroupDP.ClearHistoryTempUser();

                        //ɾ��Excel������ʱ�ļ���
                        string sFolder = Server.MapPath("./ExcelTemplate") + "\\" + objSession.UserID.ToString() + "\\";
                        System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(sFolder);
                        if (di.Exists)
                            System.IO.Directory.Delete(sFolder, true);

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

                        //�������
                        Session["Themes"] = "NewOldMainPage";

                        FormsAuthentication.SignOut();

                        FormsAuthentication.SetAuthCookie(sLoginName.Trim(), false);

                        Response.Cookies["CyanineOAUserName"].Value = Server.UrlEncode(sLoginName.Trim());


                        FormsAuthentication.SetAuthCookie(sLoginName.Trim(), false);

                        string sGuid = Request.QueryString["guid"];
                        long lngUserID = long.Parse(Request.QueryString["userid"]);
                        Server.Transfer(ti.IntentionUrl + "&guid=" + sGuid + "&userid=" + lngUserID.ToString());
                        //Response.Write("<script>window.open('" + ti.IntentionUrl + "','','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');window.opener=null;window.close();</script");
                       

                        break;

                    case -1:
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


    }
}
