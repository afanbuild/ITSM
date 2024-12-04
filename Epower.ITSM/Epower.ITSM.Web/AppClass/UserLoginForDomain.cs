using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Runtime.InteropServices;   //必要引用
using System.Security.Principal;    //必要引用
/**//// <summary>
/// UserLoginForDomain 的摘要说明
/// 适用ASP.NET 2.0 
/// Windows XP 调试成功
/// 调用”advapi32.dll“win32 API
/// </summary>

namespace Epower.ITSM.Web
{
    public class UserLoginForDomain
    {
        public UserLoginForDomain()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        #region 【用户登录域】方法【用户登录域】方法

        public const int LOGON32_LOGON_INTERACTIVE = 2;
        public const int LOGON32_PROVIDER_DEFAULT = 0;

        WindowsImpersonationContext impersonationContext;

        [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
        public static extern int LogonUser(String lpszUserName,
                                          String lpszDomain,
                                          String lpszPassword,
                                          int dwLogonType,
                                          int dwLogonProvider,
                                          ref IntPtr phToken);
        [DllImport("advapi32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public extern static int DuplicateToken(IntPtr hToken,
                                          int impersonationLevel,
                                          ref IntPtr hNewToken);
        /**/
        /// <summary>
        /// 输入用户名、密码、登录域判断是否成功
        /// </summary>
        /// <example>
        /// if (impersonateValidUser(UserName, Domain, Password)){}
        /// </example>
        /// <param name="userName">账户名称，如：string UserName = UserNameTextBox.Text;</param>
        /// <param name="domain">要登录的域，如：string Domain   = DomainTextBox.Text;</param>
        /// <param name="password">账户密码, 如：string Password = PasswordTextBox.Text;</param>
        /// <returns>成功返回true,否则返回false</returns>
        public bool impersonateValidUser(String userName, String domain, String password)
        {
            WindowsIdentity tempWindowsIdentity;
            IntPtr token = IntPtr.Zero;
            IntPtr tokenDuplicate = IntPtr.Zero;

            if (LogonUser(userName, domain, password, LOGON32_LOGON_INTERACTIVE,
            LOGON32_PROVIDER_DEFAULT, ref token) != 0)
            {
                if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                {
                    tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                    impersonationContext = tempWindowsIdentity.Impersonate();
                    if (impersonationContext != null)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            else
                return false;
        }


        #endregion
    }
}