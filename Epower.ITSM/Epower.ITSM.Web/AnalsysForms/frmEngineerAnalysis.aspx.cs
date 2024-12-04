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
using System.Drawing;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.AnalsysForms
{
    public partial class frmEngineerAnalysis : BasePage
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                ///设置起始日期
                string sQueryBeginDate = string.Empty;
                //sQueryBeginDate = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-01";
                sQueryBeginDate = (DateTime.Now.Year - 1).ToString() + "-" + DateTime.Now.Month.ToString() + "-01";
                ctrDateTime.BeginTime = DateTime.Parse(sQueryBeginDate).ToString("yyyy-MM-dd");
                ctrDateTime.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                SetMastCustomDboValue();

                LoadData();
            }
            else
            {
                if (Request.Form["__EVENTTARGET"] == "datarange")
                {
                    LoadData();
                }
            }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dpdMastShortName_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        } 

        /// <summary>
        /// 
        /// </summary>
        private void SetMastCustomDboValue()
        {
            DataTable dt = ZHServiceDP.GetMastCustomer();
            DataView dv = new DataView(dt);
            dv.Sort = "ID";
            dpdMastShortName.DataSource = dv;
            dpdMastShortName.DataTextField = "ShortName";
            dpdMastShortName.DataValueField = "ID";
            dpdMastShortName.DataBind();

            dpdMastShortName.Items.Insert(0, new ListItem("全部", "0"));

            dt.Dispose();
        }
        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            string strBeginDate = ctrDateTime.BeginTime;
            string strEndDate = ctrDateTime.EndTime;
            long lngStatusID = 0;
            DataTable dt;
            dt = ZHServiceDP.EngineerAnalysis(strBeginDate, strEndDate, lngStatusID, long.Parse(dpdMastShortName.SelectedValue));
            dgMaterialStat.DataSource = dt.DefaultView;
            dgMaterialStat.DataBind();
        }
        protected void btnSelect_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
