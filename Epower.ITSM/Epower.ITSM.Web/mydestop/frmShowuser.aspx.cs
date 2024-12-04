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
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization;
using Epower.DevBase.Organization.SqlDAL;
namespace Epower.ITSM.Web.Forms
{
	/// <summary>
    /// frmShowuser ��ժҪ˵����
	/// </summary>
    public partial class frmShowuser : BasePage
	{
		

		//long deptid=0;

		protected string GetVisible()
		{
			string sResult="VISIBILITY:visible";
			if((!Session["UserName"].ToString().Trim().ToLower().Equals("sa"))&&(StringTool.String2Long(hidUserID.Value)>0))
				sResult="VISIBILITY:hidden";

			return sResult;
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			//���ص�½ѡ��

			if(!IsPostBack)
			{
				hidUserID.Value=Convert.ToInt32(Request.Params.Get("userId")).ToString();
				LoadData();
			}
		}

		private void LoadData()
		{
			if(StringTool.String2Long(hidUserID.Value)!=0)
			{
				UserEntity ue=new UserEntity(StringTool.String2Long(hidUserID.Value));
				this.lblJob.Text = ue.Job;
				this.lblEmail.Text = ue.Email;
				this.lblQQ.Text = ue.QQ;
				this.lblName.Text = ue.Name;
				this.lblRole.Text = ue.School;
				this.lblTelNo.Text = ue.TelNo;
				this.lblMobile.Text = ue.Mobile;
                if (dropEdu.Items.FindByValue(ue.EduLevel.Trim()) != null) dropEdu.SelectedValue = ue.EduLevel.Trim();
                if (dropEdu.SelectedValue != "")
                {
                    this.lblEdu.Text = dropEdu.SelectedItem.Text;
                }
                else
                {
                    this.lblEdu.Text = string.Empty;
                }
				this.lblSortID.Text =ue.SortID.ToString();
                this.lblDeptName.Text = DeptDP.GetDeptName(ue.DeptID);
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
