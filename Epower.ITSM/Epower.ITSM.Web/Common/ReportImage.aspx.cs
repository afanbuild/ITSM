using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Epower.ITSM.SqlDAL.Common;
using Epower.ITSM.SqlDAL.Flash;

namespace Epower.ITSM.Web.Common
{
    public partial class ReportImage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dt = RefreshPage.GetRefreshPageDate();

                DataTable dt1 = RefreshPage.GetRefreshPageDate1();

                string YFeildName = "displayName,处理中数量,未解决数量,已解决数量,当月新增数量,今日新增数量";//处理中数量


                string FiledValue = "Processing,NoResponse,resolved,MonthNum,DateNum";//
                //ReportDiv.InnerHtml = FlashCS.MSPublicFlashUrl3D1(
                //                                    dt,
                //                                    YFeildName, "Miscellaneous", FiledValue,
                //                                    "本月事件监控", "数量", "本月事件监控",
                //                                    "../FlashReoport/Flash/MSColumn3D.swf", "100%", "300", true, 1, "1");


                ReportDiv.InnerHtml = FlashCS.MSPublicFlashUrl3D2(
                                                    dt,
                                                    YFeildName, "Miscellaneous", FiledValue,
                                                    "本月事件监控", "数量", 
                                                    "../FlashReoport/Flash/MSColumn3D.swf", "100%", "300", true, 1, "1");



                string YFeildName1 = "displayName,全年事件量,全年时效,当季事件量,当季时效,当月事件量,当月时效";//处理中数量


                string FiledValue1 = "yearNum,yearAchievements,quarter,quarterAchievements,monthNum,monthAchievements";//
                _YearDiv.InnerHtml = FlashCS.MSPublicFlashUrl3D2(
                                                    dt1,
                                                    YFeildName1, "Miscellaneous", FiledValue1,
                                                    "本年度事件监控", "数量",
                                                    "../FlashReoport/Flash/MSColumn3D.swf", "100%", "400", true, 1, "2");

            }
        }
    }
}
