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
using Epower.ITSM.SqlDAL;
using System.Xml;

namespace Epower.ITSM.Web.Controls.EquNature
{
    public partial class selectEquNature : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if (Request["EquId"] != null)
                    BandChackList(Request["EquId"].ToString());
            }
        }
        /// <summary>
        /// 绑定chacklist
        /// </summary>
        /// <param name="EquId"></param>
        private void BandChackList(string  EquId)
        {
            string strSchemaXml = Equ_DeskDP.GetSchemaXmlByEquID(EquId);         //资产对应其类别的配置项ID

            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("CHName");

            //解析xml，并存放到dt中
            if (strSchemaXml != "")
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(strSchemaXml);

                //添加基本配置
                XmlNodeList xmlList = xmlDoc.SelectNodes("EquScheme/BaseItem/AttributeItem");
                foreach (XmlNode node in xmlList)
                {
                    DataRow dr = dt.NewRow();
                    dr["ID"] = node.Attributes["ID"].Value;
                    dr["CHName"] = node.Attributes["CHName"].Value;
                    dt.Rows.Add(dr);
                }

                //添加关联配置
                xmlList = xmlDoc.SelectNodes("EquScheme/RelationConfig/AttributeItem");
                foreach (XmlNode node in xmlList)
                {
                    DataRow dr = dt.NewRow();
                    dr["ID"] = node.Attributes["ID"].Value;
                    dr["CHName"] = node.Attributes["CHName"].Value;
                    dt.Rows.Add(dr);
                }

                //添加备注配置
                xmlList = xmlDoc.SelectNodes("EquScheme/Remark/AttributeItem");
                foreach (XmlNode node in xmlList)
                {
                    DataRow dr = dt.NewRow();
                    dr["ID"] = node.Attributes["ID"].Value;
                    dr["CHName"] = node.Attributes["CHName"].Value;
                    dt.Rows.Add(dr);
                }
            }

            //绑定到关联属性下拉列表           

            int i=0;
            foreach (DataRow dr in dt.Rows)
            {
                ListItem item = new ListItem(dr["CHName"].ToString(),dr["ID"].ToString());
                checkEquNature.Items.Add(item);
                item.Attributes.Add("selectValue",dr["ID"].ToString());
                item.Attributes.Add("selectText",dr["CHName"].ToString());
            }
            if (dt.Rows.Count == 0)
            {
                checkEquNature.DataTextField = "CHName";
                checkEquNature.DataValueField  = "ID";
                checkEquNature.DataBind();
            }
      
        }


    }
}
