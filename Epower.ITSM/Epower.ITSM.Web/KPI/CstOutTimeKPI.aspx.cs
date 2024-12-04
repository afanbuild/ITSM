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
    public partial class CstOutTimeKPI : BasePage
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
            int type = 0;
            if (drType.SelectedItem != null)
                type = int.Parse(drType.SelectedItem.Value.ToString());

            DataTable dt = ZHServiceDP.GetCstOutTimeKPI(type, "");

            if (type == 0)
                ReportDiv.InnerHtml = FlashCS.MSPublicFlashUrl3D(dt, "nMonth", "Monthday", "rate", "事件平均超时时长KPI  (单位:小时)", "数量", "月份", "../FlashReoport/Flash/MSLine.swf", "100%", "248", true, 2);
            else
                ReportDiv.InnerHtml = FlashCS.MSPublicFlashUrl3D(dt, "nMonth", "Monthday", "rate", "事件平均超时时长KPI  (单位:小时)", "数量", "年份", "../FlashReoport/Flash/MSLine.swf", "100%", "248", true, 2);
        }
        #endregion 

        protected void drType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
