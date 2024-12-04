/*******************************************************************
 *
 * Description:会员申请
 * 
 * 
 * Create By  :su
 * Create Date:2009年4月25日
 * *****************************************************************/
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
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.AppForms
{
    public partial class frmCstRequet : System.Web.UI.Page
    {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
           
         
        }

        

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdSubmit_Click(object sender, EventArgs e)
        {
          
                cst_RequestDP ee = new cst_RequestDP();
                InitObject(ee);
                ee.inType = 0;
                ee.DealLog = 0;
                ee.inDate = DateTime.Now;
                ee.InsertRecorded(ee);
                Response.Redirect("frmCstRequetFinished.aspx");
        }

        #region InitObject
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ee"></param>
        private void InitObject(cst_RequestDP ee)
        {
           // ee.subject = txtSubject.Text.Trim();
            ee.subject = "";
            ee.Contract = txtContract.Text.Trim();
            ee.CTel = txtCTel.Text.Trim();
            ee.Content = txtContent.Text.Trim();

          
 
        }
        #endregion


       
    }
}
