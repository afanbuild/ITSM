/****************************************************************************
 * 
 * description:服务人员维护
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-11-12
 * *************************************************************************/
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;

namespace Epower.ITSM.Web.AppForms
{
    public partial class frmCst_ServiceStaffShow : BasePage
    {

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            if (this.Request.QueryString["id"] != null)
            {
                this.Master.MainID = this.Request.QueryString["id"].ToString();
            }
            this.Master.TableVisible = false;
        }
        #endregion

        #region Page_Load
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            SetParentButtonEvent();
            if (!IsPostBack)
            {
                LoadData();
            }
        }
        #endregion

        #region LoadData
        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            if (!string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                Cst_ServiceStaffDP ee = new Cst_ServiceStaffDP();
                ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                lblName.Text = ee.Name.ToString();
                lblBlongDeptName.Text = ee.BlongDeptName;
                lblRemark.Text = ee.Remark.ToString();
                lblJoinDate.Text = ee.JoinDate.ToShortDateString();
                lblFaculty.Text = ee.Faculty.ToString();
                lblUserName.Text = ee.UserName.ToString();
            }
        }
        #endregion
    }
}
