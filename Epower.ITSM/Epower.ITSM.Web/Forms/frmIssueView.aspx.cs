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

namespace Epower.ITSM.Web.Forms
{
    /// <summary>
    /// frmIssueView ��ժҪ˵����
    /// </summary>
    public partial class frmIssueView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
		{
			long lngFlowID = 0;
            long lngMessageID=0;
			long lngUserID = (long)Session["UserID"];
			if(Request.QueryString["FlowID"]!= null)
			{
				lngFlowID = long.Parse(Request.QueryString["FlowID"]);
			}
            if(Request.QueryString["MessageID"]!= null)
			{
				lngMessageID = long.Parse(Request.QueryString["MessageID"]);
			}

            //����ǵ����´���,���������ϵ��˳�,ͳһִ�йرմ���
            if (Request.QueryString["NewWin"] != null)
            {
                if(Request.QueryString["NewWin"].ToLower() =="true")
                     Session["FromUrl"] = "close";
            }
            if(lngMessageID>0)
            {
                Response.Redirect(Epower.ITSM.SqlDAL.FlowDP.GetProcessORViewUrlV2(lngMessageID, lngUserID));
            }
            else
             {
			    Response.Redirect(Epower.ITSM.SqlDAL.FlowDP.GetProcessORViewUrl(lngFlowID,lngUserID));
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
