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
using Epower.ITSM.Base;
using System.Text;


namespace Epower.ITSM.Web.AppForms
{
    public partial class frmServiceLevelCata : BasePage
    {
        public string ObjID
        {
            get { return String.IsNullOrEmpty(Request["objID"]) ? "" : Request["objID"].ToString(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int id = int.Parse(Request.QueryString["id"]);

                switch ((e_ITSMSeviceLevelItem)id)
                {
                    case e_ITSMSeviceLevelItem.eitsmSLIMcCustomerType: // 单位类型
                        NormalCata.Visible = true;
                        NormalCata.RootID = 1015;
                        break;
                    case e_ITSMSeviceLevelItem.eitsmSLIMcEnterpriseType: // 单位性质
                        NormalCata.Visible = true;
                        NormalCata.RootID = 1016;
                        break;
                    case e_ITSMSeviceLevelItem.eitsmSLIEcCustomerType: // 客户类型
                        NormalCata.Visible = true;
                        NormalCata.RootID = 1019;
                        break;
                    case e_ITSMSeviceLevelItem.eitsmSLIEqCatalogID: // 资产类别
                        EquCata.Visible = true;
                        EquCata.RootID = 1;
                        break;
                    case e_ITSMSeviceLevelItem.eitsmSLIServiceTypeID: // 事件类别
                        NormalCata.Visible = true;
                        NormalCata.RootID = 1001;
                        break;

                    case e_ITSMSeviceLevelItem.eitsmSLTServiceKindID: // 事件性质
                        NormalCata.Visible = true;
                        NormalCata.RootID = 1002;
                        break;
                    case e_ITSMSeviceLevelItem.eitsmSLTEffectID: // 影响度
                        NormalCata.Visible = true;
                        NormalCata.RootID = 1023;
                        break;
                    case e_ITSMSeviceLevelItem.eitsmSLTInstancyID: // 紧急度
                        NormalCata.Visible = true;
                        NormalCata.RootID = 1024;
                        break;

                }
            }

        }

        protected void cmdOK_Click(object sender, EventArgs e)
        {
            string strValues = "";
            string arr1 = "";
            string arr2 = "";
            if (EquCata.Visible == true)
            {
                strValues = EquCata.CatelogID.ToString() + "@" + EquCata.CatelogValue.Trim();
                arr1 = EquCata.CatelogID.ToString();
                arr2 = EquCata.CatelogValue.Trim();

            }
            else
            {
                strValues = NormalCata.CatelogID.ToString() + "@" + NormalCata.CatelogValue.Trim();
                arr1 = NormalCata.CatelogID.ToString();
                arr2 = NormalCata.CatelogValue.Trim();
            }
            StringBuilder sbText = new StringBuilder();

            sbText.Append("<script language='javascript'>");
            sbText.Append("var value='"+strValues+"';");
            sbText.Append("if(value !=''){ var arr = value.split('@');");
            //sbText.Append("alert('" + ObjID.Replace("cmdPop", "hidTag") + "'); ");


            sbText.Append("window.opener.document.getElementById('" + ObjID.Replace("cmdPop", "HidTag") + "').value='" + arr1 + "';");         
            sbText.Append("window.opener.document.getElementById('" + ObjID.Replace("cmdPop", "txtValue")+"').value='"+arr2+"';");
            sbText.Append("window.opener.document.getElementById('" + ObjID.Replace("cmdPop", "hidValue")+"').value='"+arr2+"';");
            sbText.Append(" }");
          
            sbText.Append("top.close();");

            sbText.Append("</script>");
            Page.RegisterStartupScript(DateTime.Now.Ticks.ToString(), sbText.ToString());           
        }
    }
}
