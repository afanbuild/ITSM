using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Epower.ITSM.SqlDAL.Flash;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.KPI
{
    public partial class CstDownRateKPI : BasePage
    {
        #region 设置父窗体按钮事件SetParentButtonEvent
        /// <summary>
        /// 设置父窗体按钮事件

        /// </summary>
        protected void SetParentButtonEvent()
        {

        }

        void Master_Master_Button_Query_Click()
        {
            LoadData();
        }


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtonEvent();
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        #region LoadData
        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            DataTable dt = ZHServiceDP.GetCstDownRateKPI("", "");

            ReportDiv.InnerHtml = FlashCS.MSPublicFlashUrl3DRate(dt, "nYear", "nMonth", "rate", "事件降低率KPI", "数量", "年份", "../FlashReoport/Flash/MSLine.swf", "100%", "248", true, 0);
        }
        #endregion 
    }
}
