using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Net;
using System.Security.Principal;
using Microsoft.Reporting.WebForms;

namespace Epower.ITSM.Web.Reports
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable] 
    public class MyReportViewerCredential : IReportServerCredentials
    {
        private string _username;
        private string _password;
        private string _domain;
        public Uri ReportServerUrl;

        public MyReportViewerCredential(string username, string password, string domain)
        {
            _username = username;
            _password = password;
            _domain = domain;
        }

        public WindowsIdentity ImpersonationUser
        {
            get
            {
                return null;
            }
        }

        public System.Net.ICredentials NetworkCredentials
        {
            get
            {
                return new NetworkCredential(_username, _password, _domain);
            }
        }

        public bool GetFormsCredentials(out Cookie authCookie, out string user, out string password, out string authority)
        {
            authCookie = null;
            user = _username;
            password = _password;
            authority = _domain;
            return false;

        }

    }
}

