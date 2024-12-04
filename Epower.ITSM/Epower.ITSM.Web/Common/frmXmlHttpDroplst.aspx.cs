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

using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.Common
{
    public partial class frmXmlHttpDroplst : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["id"] != null)    
            {
                string sID = Request["id"].ToString();
                string sCurr = "";
                if (Request["curr"] != null)
                    sCurr = Request["curr"].ToString();
                CheckFields(sID,sCurr);
            }
        }


        private void CheckFields(string id,string sCurr)
        {
            //string sReturnValue = "<AllFields><Field Text='日nsdfabc' /><Field Text='你1238abc' /><Field Text='法国456abc' /><Field Text='家了副123abc' /><Field Text='yrtyabc' /><Field Text='asdfdabc' /><Field Text='gsabc' /><Field Text='eeabc' /><Field Text='abc' /><Field Text='abcdef' /><Field Text='ghte' /><Field Text='aber' /><Field Text='beaber' /><Field Text='中' /><Field Text='中国aber' /><Field Text='日aber' /><Field Text='aber' /></AllFields>";
            string sReturnValue = "";

            if (id.StartsWith("SchemaItem_") == true)
            {
                //配置项快速录入选择
                sReturnValue = IdiomDP.GetIdiom((long)Session["UserID"], 100, id);
            }
            if (id.StartsWith("EquRel_") == true)
            {
                //配置项快速录入选择
                sReturnValue = IdiomDP.GetIdiom((long)Session["UserID"], 100, id);
            }


            //知识高级搜索
            if (id == "infSeach_001")
                sReturnValue = IdiomDP.GetInfoTagsIdiom(100);

            if (id.StartsWith("PinYin_") == true)
            {
                //配置项快速录入选择，暂时只用在了知识库高级搜索（２００９－０２－０６）
                sReturnValue = IdiomDP.GetPinYinIdiom(100, id,sCurr);
            }

           // sReturnValue = "<AllFields><Field Text='日nsdfabc' /><Field Text='你1238abc' /><Field Text='法国456abc' /><Field Text='家了副123abc' /><Field Text='yrtyabc' /><Field Text='asdfdabc' /><Field Text='gsabc' /><Field Text='eeabc' /><Field Text='abc' /><Field Text='abcdef' /><Field Text='ghte' /><Field Text='aber' /><Field Text='beaber' /><Field Text='中' /><Field Text='中国aber' /><Field Text='日aber' /><Field Text='aber' /></AllFields>";

            Response.Clear();
            //返回是否有新增 0 无, 1 有
            Response.Write(sReturnValue);
            Response.Flush();
            Response.End();
        }
    }
}
