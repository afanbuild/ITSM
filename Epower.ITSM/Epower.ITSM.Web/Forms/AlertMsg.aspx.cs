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
using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.Forms
{
    public partial class AlterMsg : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 显示需要弹出的公告
            //显示需要弹出的公告
            DataTable dt = NewsDp.GetIsAlterNews(StringTool.String2Long(Session["UserOrgID"].ToString()), StringTool.String2Long(Session["UserDeptID"].ToString()));
            if (dt != null && dt.Rows.Count > 0)
            {

                rptAlertMsg.DataSource = dt;
                rptAlertMsg.DataBind();

            }
            #endregion
        }
    }
}
