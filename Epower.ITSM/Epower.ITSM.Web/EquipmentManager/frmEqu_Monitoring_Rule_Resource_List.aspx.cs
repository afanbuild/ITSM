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

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frmEqu_Monitoring_Rule_Resource_List : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) { BindData(); }
        }

        /// <summary>
        /// 加载资源列表
        /// </summary>
        private void BindData()
        {
            String str_subject_id = Request.QueryString["subjectid"];
            if (String.IsNullOrEmpty(str_subject_id)) return;

            Equ_DeskDP ee = new Equ_DeskDP();
            DataTable dt = ee.GetDataTable(String.Format(" AND CATALOGID = {0} ", str_subject_id), " ORDER BY ID ASC ");

            rptResourceList.DataSource = dt;
            rptResourceList.DataBind();
        }
    }
}
