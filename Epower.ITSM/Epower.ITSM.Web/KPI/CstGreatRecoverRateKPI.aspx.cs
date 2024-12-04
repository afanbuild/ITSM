﻿using System;
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
    public partial class CstGreatRecoverRateKPI : BasePage
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
            this.CtrFCDEffect.mySelectedIndexChanged += new EventHandler(CtrFCDEffect_mySelectedIndexChanged);

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

            string sWhere = "";
            string sWhere2 = "";

            if (drTime.SelectedItem != null)
                sWhere += " And datediff('minute',ReportingTime,FinishedTime)<=" + drTime.SelectedItem.Value;

            if (CtrFCDEffect.CatelogID > 0 && CtrFCDEffect.CatelogID != 1023)
            {
                string FullID = "";
                DataTable FullIDDT = CommonDP.ExcuteSqlTable("SELECT FullID from Es_Catalog where CatalogID = " + CtrFCDEffect.CatelogID);
                if (FullIDDT.Rows.Count == 1)
                {
                    FullID = FullIDDT.Rows[0][0].ToString();
                }
                sWhere += " AND EffectID IN (SELECT CatalogID FROM Es_Catalog WHERE FullID LIKE '" + FullID + "%')";
                sWhere2 += " AND EffectID IN (SELECT CatalogID FROM Es_Catalog WHERE FullID LIKE '" + FullID + "%')";
            }

            DataTable dt = ZHServiceDP.GetCstGreatRecoverAvgTimeKPI(type, sWhere);

            if (type == 0)
                ReportDiv.InnerHtml = FlashCS.MSPublicFlashUrl3DRate(dt, "nMonth", "MonthDay", "rate", "重大事件恢复率KPI", "数量", "月份", "../FlashReoport/Flash/MSLine.swf", "100%", "248", true, 2);
            else
                ReportDiv.InnerHtml = FlashCS.MSPublicFlashUrl3DRate(dt, "nMonth", "MonthDay", "rate", "重大事件恢复率KPI", "数量", "年份", "../FlashReoport/Flash/MSLine.swf", "100%", "248", true, 2);
        }
        #endregion 

        protected void drType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void drTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void CtrFCDEffect_mySelectedIndexChanged(object sender, System.EventArgs e)
        {
            LoadData();
        }
    }
}
