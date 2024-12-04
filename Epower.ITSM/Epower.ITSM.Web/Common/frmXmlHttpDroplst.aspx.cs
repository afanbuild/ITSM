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
            //string sReturnValue = "<AllFields><Field Text='��nsdfabc' /><Field Text='��1238abc' /><Field Text='����456abc' /><Field Text='���˸�123abc' /><Field Text='yrtyabc' /><Field Text='asdfdabc' /><Field Text='gsabc' /><Field Text='eeabc' /><Field Text='abc' /><Field Text='abcdef' /><Field Text='ghte' /><Field Text='aber' /><Field Text='beaber' /><Field Text='��' /><Field Text='�й�aber' /><Field Text='��aber' /><Field Text='aber' /></AllFields>";
            string sReturnValue = "";

            if (id.StartsWith("SchemaItem_") == true)
            {
                //���������¼��ѡ��
                sReturnValue = IdiomDP.GetIdiom((long)Session["UserID"], 100, id);
            }
            if (id.StartsWith("EquRel_") == true)
            {
                //���������¼��ѡ��
                sReturnValue = IdiomDP.GetIdiom((long)Session["UserID"], 100, id);
            }


            //֪ʶ�߼�����
            if (id == "infSeach_001")
                sReturnValue = IdiomDP.GetInfoTagsIdiom(100);

            if (id.StartsWith("PinYin_") == true)
            {
                //���������¼��ѡ����ʱֻ������֪ʶ��߼�����������������������������
                sReturnValue = IdiomDP.GetPinYinIdiom(100, id,sCurr);
            }

           // sReturnValue = "<AllFields><Field Text='��nsdfabc' /><Field Text='��1238abc' /><Field Text='����456abc' /><Field Text='���˸�123abc' /><Field Text='yrtyabc' /><Field Text='asdfdabc' /><Field Text='gsabc' /><Field Text='eeabc' /><Field Text='abc' /><Field Text='abcdef' /><Field Text='ghte' /><Field Text='aber' /><Field Text='beaber' /><Field Text='��' /><Field Text='�й�aber' /><Field Text='��aber' /><Field Text='aber' /></AllFields>";

            Response.Clear();
            //�����Ƿ������� 0 ��, 1 ��
            Response.Write(sReturnValue);
            Response.Flush();
            Response.End();
        }
    }
}
