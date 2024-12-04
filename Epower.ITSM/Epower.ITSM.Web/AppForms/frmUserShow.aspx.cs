/*******************************************************************
 * 版权所有：非凡信息技术
 * Description：用户详细信息展示
 * 
 * 
 * Create By  ：zhumc
 * Create Date：2010-12-24
 * *****************************************************************/
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

namespace Epower.ITSM.Web.AppForms
{
	/// <summary>
    /// frmUserShow 的摘要说明。
	/// </summary>
    public partial class frmUserShow : BasePage
	{
        /// <summary>
        /// 设置母版页面按钮
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.TableVisible = false;
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
            UserEntity ue = new UserEntity(StringTool.String2Long(Request["userid"].ToString()));
            this.CtrFlowCataDropListjob.CatelogValue = ue.Job;
            this.txtEmail.Text = ue.Email;
            this.txtQQ.Text = ue.QQ;
            this.txtName.Text = ue.Name;
            this.txtRole.Text = ue.School;
            this.txtTelNo.Text = ue.TelNo;
            this.txtMobile.Text = ue.Mobile;
            dropEdu.Text = ue.EduLevel.Trim();
            dropSex.Text = ue.Sex.Trim();
            CtrFlowCataDropListjob.ContralState = Epower.ITSM.Base.eOA_FlowControlState.eReadOnly;
        }
	}
}
