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
using Epower.ITSM.SqlDAL;
using Epower.DevBase.Organization;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.mydestop
{
	/// <summary>
	/// frmSelectPersonRight ��ժҪ˵����
	/// </summary>
	public partial class frmSelectPersonRight : BasePage
	{
        /// <summary>
        /// ��Ա���ͣ�ѡ����Ա���߷�����Ա
        /// </summary>
        protected string Type
        {
            get
            {
                if (Request["Type"] != null)
                    return Request["Type"].ToString();
                else
                    return "-1";
            }
        }

        public string Opener_ClientId
        {
            set
            {
                ViewState["Opener_ClientId"] = value;
            }
            get
            {
                return (ViewState["Opener_ClientId"] == null) ? "" : ViewState["Opener_ClientId"].ToString();
            }
        }


		protected void Page_Load(object sender, System.EventArgs e)
		{

    
            if (Request["DeptID"] == null)
            {
                return;
            }
			if(!IsPostBack)
			{
                if (Request["Opener_ClientId"] != null)
                {
                    Opener_ClientId = Request["Opener_ClientId"].ToString().Replace("hidClientId_ForOpenerPage", "");
                }

				long deptId = Convert.ToInt32(Request.Params.Get("DeptID"));
				LoadData(deptId);
			}
		}

		private void LoadData(long lngDeptId)
		{
            DataTable dt = new DataTable();
            dt = DeptControl.GetDeptUserList(lngDeptId);

			lsbStaff.DataSource=dt.DefaultView;
			lsbStaff.DataTextField ="Name";
			lsbStaff.DataValueField ="userid";
			lsbStaff.DataBind();
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
