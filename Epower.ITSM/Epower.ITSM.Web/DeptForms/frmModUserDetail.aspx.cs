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
using Epower.ITSM.Web.Controls;
namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// frmModUser 的摘要说明。
	/// </summary>
	public partial class frmModUserDetail : BasePage
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
			//隐藏登陆选项

			if(!IsPostBack)
			{
				hidUserID.Value=Convert.ToInt32(Request.Params.Get("userId")).ToString();
				//获取部门名
				hidDeptID.Value=Request.Params.Get("deptId").Trim();
				hidOldDeptID.Value=hidDeptID.Value;
				txtDeptName.Text=DeptDP.GetDeptName(StringTool.String2Long(hidDeptID.Value));

                #region 获取根节点
                //DeptPickerBranch dept = new DeptPickerBranch();
                hidprentDeptID.Value= getPrentDeptID(hidDeptID.Value);
                #endregion

                LoadData();
			}
			if(hidUserID.Value!="0")
			{
				string sLoginName=Session["UserName"].ToString().Trim().ToLower();
				this.txtLoginName.Enabled = (sLoginName.Equals("sa") || sLoginName.Equals("admin"));
				this.txtFistPwd.Enabled = false;
				this.txtLastPwd.Enabled = false;
				Response.Write("<script>document.title = '修改用户'</script>");

                trdept.Visible = true;
				this.lblDept.Visible =true;
				this.txtDeptName.Visible =true;
				this.cmdPopParentDept.Visible =true;	
			
			}else
			{
				this.txtFistPwd.TextMode=TextBoxMode.Password;
				this.txtLastPwd.TextMode=TextBoxMode.Password;
                trdept.Visible = false;
				this.lblDept.Visible =false;
				this.txtDeptName.Visible =false;
				this.cmdPopParentDept.Visible =false;
				Response.Write("<script>document.title = '添加用户'</script>");
			}
		}

        /// <summary>
        /// 获取根目录的部门
        /// </summary>
        /// <param name="deptID">当前节点的部门ID</param>
        /// <returns></returns>
        public string getPrentDeptID(string deptID)
        {
           string depid= DeptDP.GetDeptFullID(StringTool.String2Long(hidDeptID.Value));
           if (depid != "" && depid.Length > 5)
           {
               depid = depid.ToString();
           }
           return depid;
        }

		private void LoadData()
		{
			if(StringTool.String2Long(hidUserID.Value)!=0)
			{
				UserEntity ue=new UserEntity(StringTool.String2Long(hidUserID.Value),StringTool.String2Long(hidDeptID.Value));
				this.txtLoginName.Text = ue.LoginName.ToString();
				this.txtFistPwd.Text = "*****";
				this.txtLastPwd.Text = txtFistPwd.Text;
                this.CtrFlowCataDropListjob.CatelogID = long.Parse(ue.JobID.ToString()) ;
				this.txtEmail.Text = ue.Email;
				this.txtQQ.Text = ue.QQ;
				this.txtName.Text = ue.Name;
				this.txtRole.Text = ue.School;
				this.txtTelNo.Text = ue.TelNo;
                ddlLock.SelectedIndex = ddlLock.Items.IndexOf(ddlLock.Items.FindByValue(ue.LockStatus.ToString()));
				this.txtMobile.Text = ue.Mobile;
				if(dropEdu.Items.FindByValue(ue.EduLevel.Trim())!=null) dropEdu.SelectedValue=ue.EduLevel.Trim();
				//this.dropSex.SelectedItem.Text = ue.Sex;
				this.dropSex.SelectedValue  = ue.Sex.Trim()=="男"? "1":"2";
				this.hidPassWord.Value =ue.Password;
				this.txtSortID.Text =ue.SortID.ToString();
				this.ddlStatus.SelectedValue = ue.Deleted.ToString();


			}
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

			UserEntity user = new UserEntity();
			user.UserID  = StringTool.String2Long(hidUserID.Value);
			user.LoginName = this.txtLoginName.Text.Trim();
			user.Name = this.txtName.Text.Trim();
			user.Sex = this.dropSex.SelectedItem.Text.Trim();
			user.TelNo = this.txtTelNo.Text.Trim();
			user.Mobile = this.txtMobile.Text.Trim();
			user.School = this.txtRole.Text.Trim();
			user.Job = this.CtrFlowCataDropListjob.CatelogValue.Trim();
            user.JobID = this.CtrFlowCataDropListjob.CatelogID;
			user.Email = this.txtEmail.Text.Trim();
            user.LockStatus = int.Parse(ddlLock.SelectedValue.ToString());
			user.QQ  = this.txtQQ.Text.Trim();
			user.EduLevel = this.dropEdu.SelectedValue.Trim();
			user.SortID =StringTool.String2Int(txtSortID.Text);
			user.UpdateID =StringTool.String2Long(Session["UserID"].ToString());
			user.CreatorID=StringTool.String2Long(Session["UserID"].ToString());
			user.Deleted = StringTool.String2Int(this.ddlStatus.SelectedValue);
			if(user.UserID==0)
			{
				user.Password =this.txtFistPwd.Text.ToString();
			}else
			{
				user.Password =this.hidPassWord.Value.ToString();
			}
			
			//if(StringTool.String2Long(Page.Request.Form["hidDeptID"])!=0)
			user.OldDeptID=StringTool.String2Long(hidOldDeptID.Value);
			if(StringTool.String2Long(hidDeptID.Value)!=0)
			{
				user.DeptID=StringTool.String2Long(hidDeptID.Value);
			}
			else
			{
				PageTool.MsgBox(this,"请选择部门后再添加用户！");
				return;
			}

            if (!string.IsNullOrEmpty(hidprentDeptID.Value))
            {
                user.parentDeptID =hidprentDeptID.Value;
            }
            else
            {
                user.parentDeptID = "1";
            }

			try
			{
				UserControls.AddUser(user);
                HttpRuntime.Cache.Insert("EpCacheValidUser", false);
                HttpRuntime.Cache.Insert("EpCacheValidUserDept", false);
				hidUserID.Value=user.UserID.ToString();

                PageTool.AddJavaScript(this, "window.opener.location.href=window.opener.location.href;window.close();");
				//PageTool.MsgBox(this,"已经成功修改人员的信息");
			}
			catch(Exception ex)
			{
				if(ex.Message.CompareTo("登录名称已经存在") == 0)
				{
					//MsgBox(ex.Message);
					string sCheckUser=@"var features ='dialogWidth:350px;' +
												'dialogHeight:300px;' +
												'directories:no; localtion:no; menubar:no; status=no; toolbar=no;scrollbars:yes;resizable=yes';
												window.showModalDialog('frmCheckUser.aspx?LoginName="+txtLoginName.Text+"','',features);";
					PageTool.AddJavaScript(this,sCheckUser);
				}
				else
				{
					PageTool.MsgBox(this,"保存用户资料时出现错误<br>错误为:"+ex.Message.ToString());
				}
			}

		}

	}
}
