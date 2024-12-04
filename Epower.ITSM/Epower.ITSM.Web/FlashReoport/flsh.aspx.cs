using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Epower.ITSM.SqlDAL.Flash;
using Epower.ITSM.SqlDAL;
using System.Text;
using System.Drawing;
using System.IO;
using Epower.ITSM.Web.Common;



namespace Epower.ITSM.Web.FlashReoport
{
    public partial class flsh : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string SQL = @"select ServiceTypes,months,count(1) as countNumber from (
                        select nvl(ServiceType,'其他')
                         ServiceTypes, to_char(DATEPART('yyyy', regSysDate))||'-'||to_char(DATEPART('mm', regSysDate)) months    from Cst_Issues  where ServiceType in ('打印机故障','系统故障','网络故障','硬件故障')
                        ) A group by A.ServiceTypes,months  ";
            DataTable dt = CommonDP.ExcuteSqlTable(SQL);






            //3维数据结构 线状图
            
            Div3.InnerHtml = FlashCS.MSPublicFlashUrl3D(dt, "ServiceTypes", "months", "countNumber","flash控件曲线范例图", "数量", "月份", "Flash/MSLine.swf","100%","248",false,2);

            //3维数据结构 柱状图  2D效果            
            Div4.InnerHtml = FlashCS.MSPublicFlashUrl3D(dt, "ServiceTypes", "months", "countNumber", "flash控件曲线范例图", "数量", "月份", "Flash/MSColumn2D.swf", "100%", "248", true, 2);

            //3维数据结构 柱状图  3D效果            
            Div5.InnerHtml = FlashCS.MSPublicFlashUrl3D(dt, "ServiceTypes", "months", "countNumber", "flash控件曲线范例图", "数量", "月份", "Flash/MSColumn3D.swf", "100%", "248", true, 2);


            string strSQL = "select nvl(ServiceType,'其他') ServiceType ,count(ServiceType) as numbers from Cst_Issues where ServiceType in('打印机故障','系统故障','网络故障','硬件故障')  group by  ServiceType";

            DataTable dt2 = CommonDP.ExcuteSqlTable(strSQL);

            //二维数据结构 柱状图 2D效果
            Div6.InnerHtml = FlashCS.PublicFlashUrl2D(dt2, "ServiceType", "numbers", "二维数据结构柱状图范例", "类别", "数量", "Flash/Column2D.swf", "100%", "248", true, 2);
            //二维数据结构 柱状图 3D效果
            Div7.InnerHtml = FlashCS.PublicFlashUrl2D(dt2, "ServiceType", "numbers", "二维数据结构柱状图范例", "类别", "数量", "Flash/Column3D.swf", "100%", "248", true, 2);
            //二维数据结构 线状图 
            Div8.InnerHtml = FlashCS.PublicFlashUrl2D(dt2, "ServiceType", "numbers", "二维数据结构柱状图范例", "类别", "数量", "Flash/Line.swf", "100%", "248", true, 2);

            // 二维数据结构 圆饼图 
            Div9.InnerHtml = FlashCS.PublicFlashUrl2D(dt2, "ServiceType", "numbers", "愿饼图范例", "", "", "Flash/Pie2D.swf", "100%", "248", true, 2);

            Div10.InnerHtml = FlashCS.PublicFlashUrl2D(dt2, "ServiceType", "numbers", "愿饼图范例", "", "", "Flash/Pie3D.swf", "100%", "248", true, 2);

            Div1.InnerHtml = FlashCS.PublicFlashUrl2D(dt2, "ServiceType", "numbers", "愿饼图范例", "", "", "Flash/FCF_Funnel.swf", "100%", "248", true, 2);


            Div11.InnerHtml = FlashCS.MSPublicFlashUrl3D(dt, "ServiceTypes", "months", "countNumber", "flash控件曲线范例图", "数量", "月份", "Flash/MSColumn3DLineDY.swf", "100%", "248", true, 2);

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {



            }
            catch
            {
            }
        }
    
    }
}
