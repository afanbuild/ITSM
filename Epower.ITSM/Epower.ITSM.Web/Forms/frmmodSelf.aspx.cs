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
//using Epower.DevBase.BaseTools;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// frmModUser 的摘要说明。
	/// </summary>
    public partial class frmmodSelf : BasePage
	{
        /// <summary>
        /// 设置母版页面按钮
        /// </summary>
        protected void SetParentButtonEvent()
        {
            //this.Master.TableVisible = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
		{
            SetParentButtonEvent();
			if(!IsPostBack)
			{
				LoadData();

			}
		}

        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            UserEntity ue = new UserEntity(StringTool.String2Long(Session["UserID"].ToString()));
            this.txtLoginName.Text = ue.LoginName.ToString();
            this.CtrFlowCataDropListjob.CatelogID = long.Parse(ue.JobID.ToString());
            this.txtEmail.Text = ue.Email;
            this.txtQQ.Text = ue.QQ;
            this.txtName.Text = ue.Name;
            this.txtRole.Text = ue.School;
            this.txtTelNo.Text = ue.TelNo;
            this.txtMobile.Text = ue.Mobile;
            if (dropEdu.Items.FindByValue(ue.EduLevel.Trim()) != null) dropEdu.SelectedValue = ue.EduLevel.Trim();
            this.dropSex.SelectedValue = ue.Sex.Trim() == "男" ? "1" : "2";
            this.ddlStatus.SelectedValue = ue.Deleted.ToString();
        }

		#region Web 窗体设计器生成的代码
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

		protected void cmdSave_Click(object sender, System.EventArgs e)
		{
            UserEntity user = new UserEntity(StringTool.String2Long(Session["UserID"].ToString()));
            user.UserID = StringTool.String2Long(Session["UserID"].ToString());
            user.LoginName = this.txtLoginName.Text.Trim();
            user.Name = this.txtName.Text.Trim();
            user.Sex = this.dropSex.SelectedItem.Text.Trim();
            user.TelNo = this.txtTelNo.Text.Trim();
            user.Mobile = this.txtMobile.Text.Trim();
            user.School = this.txtRole.Text.Trim();
            user.Job = this.CtrFlowCataDropListjob.CatelogValue.Trim();
            user.JobID = this.CtrFlowCataDropListjob.CatelogID;
            user.Email = this.txtEmail.Text.Trim();
            user.QQ = this.txtQQ.Text.Trim();
            user.EduLevel = this.dropEdu.SelectedValue.Trim();
            user.UpdateID = StringTool.String2Long(Session["UserID"].ToString());
            user.CreatorID = StringTool.String2Long(Session["UserID"].ToString());
            user.Deleted = StringTool.String2Int(this.ddlStatus.SelectedValue);
            UserControls.AddUser(user);
            PageTool.MsgBox(this, "修改成功！");
		}
	}
}
