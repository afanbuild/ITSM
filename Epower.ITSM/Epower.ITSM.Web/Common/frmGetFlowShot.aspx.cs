using System;
using System.Data;
using System.Text;
using System.Xml;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using appDataProcess;
using EpowerGlobal;

namespace Epower.ITSM.Web.Common
{
    public partial class frmGetFlowShot : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected override void Render(HtmlTextWriter writer)
        {
            string sOutput = string.Empty;

            if (Request.QueryString["appid"] != null && Request.QueryString["flowid"] != null)
            {
            long lngAppID = long.Parse(Request.QueryString["appid"]);
            long lngFlowID = long.Parse(Request.QueryString["flowid"]);
            ImplDataProcess dp = new ImplDataProcess(lngAppID);
            string sXml = dp.GetBussinessShotValues(lngFlowID);
            sOutput = GetOutputForShot(sXml);
            }
            else if (Request.QueryString["userid"] != null)
            {
                sOutput = GetUserInfo(Request.QueryString["userid"].ToString());
            }

            Response.Clear();
            Response.Write(sOutput);
            Response.Flush();
            Response.End();
        }



        private string GetOutputForShot(string sXml)
        {
            //return sXml;
           // #region 注释 --yanghw

            if (sXml == "")
                return "";


            StringBuilder sb = new StringBuilder();

            bool blnHasHeader = false;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(sXml);

            XmlNodeList nodes = xmlDoc.DocumentElement.ChildNodes;
            string sName = "";
            string sValue = "";
            foreach (XmlNode n in nodes)
            {
                sName = n.Attributes["FieldName"].Value;
                sValue = n.Attributes["Value"].Value.Trim();
                if (sValue.Length > 0)
                {
                    if (blnHasHeader == false)
                    {
                        sb.Append("<table width='220px'  class='listContent'>");
                        blnHasHeader = true;
                    }
                    sb.Append("<tr>");
                    sb.Append("<td class='listTitleNew_s' width='30%'>");
                    sb.Append(sName);
                    sb.Append("</td>");

                    sb.Append("<td class='listNew_s' width='70%' style='word-break:break-all;'>");
                    sb.Append(sValue);
                    sb.Append("</td>");

                    sb.Append("</tr>");


                }
            }

            if (blnHasHeader == true)
            {
                sb.Append("</table>");
            }
            return sb.ToString();
           // #endregion 
        }

        #region GetUserInfo 用户展示
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sUserID"></param>
        /// <returns></returns>
        private string GetUserInfo(string sUserID)
        {
            StringBuilder sb = new StringBuilder();
            Epower.DevBase.Organization.SqlDAL.UserEntity userentity = new Epower.DevBase.Organization.SqlDAL.UserEntity(long.Parse(sUserID));
            sb.Append("<table width='220px'  class='listContent'>");

            sb.Append("<tr>");
            sb.Append("<td width='30%' class='listTitle'>");
            sb.Append("职位：");
            sb.Append("</td>");
            sb.Append("<td  width='70%' style='word-break:break-all;'  class='list'>");
            sb.Append(userentity.Job);
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td width='30%' class='listTitle'>");
            sb.Append("手机：");
            sb.Append("</td>");
            sb.Append("<td  width='70%' style='word-break:break-all;'  class='list'>");
            sb.Append(userentity.Mobile);
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td width='30%' class='listTitle'>");
            sb.Append("电话：");
            sb.Append("</td>");
            sb.Append("<td  width='70%' style='word-break:break-all;'  class='list'>");
            sb.Append(userentity.TelNo);
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td width='30%' class='listTitle'>");
            sb.Append("电子邮件：");
            sb.Append("</td>");
            sb.Append("<td  width='70%' style='word-break:break-all;'  class='list'>");
            sb.Append(userentity.Email);
            sb.Append("</td>");
            sb.Append("</tr>");

            sb.Append("</table>");
            return sb.ToString();
        }
        #endregion 
    }
}
