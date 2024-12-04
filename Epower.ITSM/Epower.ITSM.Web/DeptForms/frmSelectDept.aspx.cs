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
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// frmSelectDept ��ժҪ˵����
	/// </summary>
	public partial class frmSelectDept : System.Web.UI.Page
	{
        /// <summary>
        /// ��ô������Ĳ����ж��ĸ���ҳ�洫�����ġ�
        /// </summary>
        public string TypeFrm
        {
            get
            {

                if (Request.QueryString["TypeFrm"] != null)
                {
                    return Request.QueryString["TypeFrm"];
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// ��ô������Ĳ��������жϸ�ҳ���id����
        /// </summary>
        public string Opener_ClientId
        {

            get
            {
                return (Request["Opener_ClientId"] == null) ? "" : Request["Opener_ClientId"].ToString().Replace("hidClientId_ForOpenerPage", "");
            }
        }


		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			if(Request.QueryString["LimitCurr"] != null)
			{
				CtrDeptTree1.LimitCurr = bool.Parse(Request.QueryString["LimitCurr"]);
			}
			if(Request.QueryString["CurrDeptID"] != null)
			{
				CtrDeptTree1.CurrDeptID = long.Parse(Request.QueryString["CurrDeptID"]);
				// ��¼��ǰ����
				//Session["OldDeptID"] = DeptDP.GetDeptParentID(CtrDeptTree1.CurrDeptID);
                Session["OldDeptID"] = CtrDeptTree1.CurrDeptID;
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
