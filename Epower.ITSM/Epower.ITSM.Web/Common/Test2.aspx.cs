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

namespace Epower.ITSM.Web.Common
{
    public partial class Test2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CtrTextDropList1.FieldsSourceType = 1;
            CtrTextDropList1.FieldsSourceID = "PinYin_1001";
            CtrTextDropList1.OnTimeXmlHttp = true;

        }
    }
}
