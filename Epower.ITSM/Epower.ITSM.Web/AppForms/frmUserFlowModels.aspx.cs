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
using EpowerCom;

namespace Epower.ITSM.Web.AppForms
{
    public partial class frmUserFlowModels : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            long lngUserID = (long)Session["UserID"];
            DataSet ds = FlowModel.GetUserFlowModels(lngUserID);

            grdFlowModel.DataSource = ds.Tables[0].DefaultView;
            grdFlowModel.DataBind();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdFlowModel_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string FlowModelID = DataBinder.Eval(e.Item.DataItem, "FlowModelID").ToString();
                e.Item.Attributes.Add("ondblclick", "window.open('../Forms/OA_AddNew.aspx?flowmodelid=" + FlowModelID + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");               
            }
        }
    }
}
