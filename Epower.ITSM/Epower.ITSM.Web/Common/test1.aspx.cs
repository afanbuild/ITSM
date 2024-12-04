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
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.Common
{
    public partial class test1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           // CtrTextDropList1.Value = "a";
            //CtrTextDropList1.OnBlurScript = "alert('aa')";
             hidXml.Value = "<AllFields><Field Text='nsdfabc' /><Field Text='1238abc' /><Field Text='456abc' /><Field Text='123abc' /><Field Text='yrtyabc' /><Field Text='asdfdabc' /><Field Text='gsabc' /><Field Text='eeabc' /><Field Text='abc' /><Field Text='abcdef' /><Field Text='ghte' /><Field Text='aber' /><Field Text='beaber' /><Field Text='中' /><Field Text='中国aber' /><Field Text='日aber' /><Field Text='aber' /></AllFields>";
            CtrTextDropList1.FieldsSourceType = 1;
            //CtrTextDropList1.FieldsSourceID = "PinYin_1001";
            CtrTextDropList1.FieldsSourceID = "infSeach_001";
            //CtrTextDropList1.FieldsSourceID = hidXml.ClientID;

        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            TextBox2.Text = GBToPY.getAllPY(TextBox1.Text)  + "  " + GBToPY.getFirstPY(TextBox1.Text);
        }
    }
}
