using System;
using System.Text;
using System.Web;
using System.IO;

namespace Epower.ITSM.Log
{
	/// <summary>
	/// ��־�����HttpModule��
	/// </summary>
    public class EpowerHttpModule : IHttpModule
    {
		private string VisitScript = string.Empty; //������־�ύҳ��

        public String ModuleName
        {
            get { return "EpowerLogModule"; }
        }

        public void Init(HttpApplication application)
        {
			//�쳣��־��¼
            if (CommonDP.GetConfigValue(Constant.LogConfigNodeName, "ExceptionLog").Equals("1"))
				application.Error += (new EventHandler(this.Application_Error));

			//������־��¼
            if (CommonDP.GetConfigValue(Constant.LogConfigNodeName, "VisitLog").Equals("1"))
			{
				application.EndRequest += new EventHandler(OnEndRequest);
				application.AcquireRequestState += new EventHandler(OnAcquireRequestState);
			}
		}

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception objErr = HttpContext.Current.Server.GetLastError().GetBaseException();
            ExceptionLog.PostException(objErr, string.Empty,((int)Level.Intermediate).ToString());
        }

		private void OnEndRequest(object sender, EventArgs e)
		{
			HttpContext.Current.Response.Write(this.VisitScript);
		}

		private void OnAcquireRequestState(object sender, EventArgs e)
		{
			//����¼��в���ȡ��Session
			//this.VisitScript = VisitLog.GetVisitScript();
		}

        public void Dispose()
        {
        }
    }
}
