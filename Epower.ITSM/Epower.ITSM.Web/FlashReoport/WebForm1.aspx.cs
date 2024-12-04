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
using Epower.ITSM.Log;
using Epower.ITSM.SqlDAL.Flash;

namespace Epower.ITSM.Web.FlashReoport
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            string strSQL = "select (case ServiceType when null then '其他' when '' then '其他' else ServiceType end  )ServiceType ,count(ServiceType) as number from Cst_Issues where ServiceType in('打印机故障','系统故障','网络故障','硬件故障')  group by  ServiceType";

            DataTable dt2 = CommonDP.ExcuteSqlTable(strSQL);



            string cc=@"<chart isSliced='1' slicingDistance='4' decimalPrecision='0'>
                      <set name='Selected' value='41' color='99CC00' alpha='85' /> 
                      <set name='Tested' value='84' color='333333' alpha='85' /> 
                      <set name='Interviewed' value='126' color='99CC00' alpha='85' /> 
                      <set name='Candidates Applied' value='180' color='333333' alpha='85' /> 
                      </chart>";

            Div1.InnerHtml = FlashCS.loudou();
                //.xxx(dt2, "ServiceType", "number", "愿饼图范例", "", "", "Flash/Line.swf", "100%", "248", true, 2);
        }
    }
}
