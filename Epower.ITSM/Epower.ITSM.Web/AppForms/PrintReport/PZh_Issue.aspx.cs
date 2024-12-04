/****************************************************************************
 * 
 * description:客户打印表单
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-07-31
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

namespace Epower.ITSM.Web.AppForms.PrintReport
{
    public partial class PZh_Issue : System.Web.UI.Page
    {
        /// <summary>
        /// 初始化页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string strFlowID = "0";
            if (Request["FlowID"] != null)
                strFlowID = Request["FlowID"].ToString();
            DataTable dt = LoadData(strFlowID);
            if (dt.Rows.Count > 0)
            {
                lblID.Text = dt.Rows[0]["buildCode"].ToString() + dt.Rows[0]["ServiceNo"].ToString();
                lblTime.Text = dt.Rows[0]["RegSysDate"].ToString();
                lblPerson.Text = dt.Rows[0]["RegUserName"].ToString();
                lblCustomName.Text = dt.Rows[0]["CustName"].ToString();
                lblLxr.Text = dt.Rows[0]["Contact"].ToString();
                lblContent.Text = dt.Rows[0]["Content"].ToString();
            }
        }

        /// <summary>
        /// 根据流程ID，取得数据
        /// </summary>
        /// <param name="strFlowID"></param>
        /// <returns></returns>
        private DataTable LoadData(string strFlowID)
        {
            DataTable dt = new DataTable();
            dt = ZHServiceDP.GetDataByFlowID(decimal.Parse(strFlowID));
            return dt;
        }
    }
}
