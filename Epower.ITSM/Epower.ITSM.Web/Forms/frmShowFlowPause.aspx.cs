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

namespace Epower.ITSM.Web.Forms
{
    public partial class frmShowFlowPause : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            long id = long.Parse(Request.QueryString["id"]);
            if (Page.IsPostBack == false)
            {
                LoadHtmlControls(id);
                
            }
        }

        private void LoadHtmlControls(long id)
        {
            string sProcess = AddSubFlowProcess(id);

            if (sProcess != "")
            {
                litFlowPauseLog.Text = "<table width='100%'><tr><td class='listTitle' width='100%'>流程暂停恢复过程</td></tr><tr><td width='100%'>" + sProcess + "</td></tr></table>";
            }
        }

        private string AddSubFlowProcess(long lngFlowID)
        {
            string sProcess = "";
            int iRowCount = 0;


            if (!IsPostBack)
            {
            
                DataTable dt = MessageProcess.GetFlowPauseLog(lngFlowID);
                foreach (DataRow dr in dt.Rows)
                {
                    string pTime = dr["pausetime"].ToString();
                    string cTime = dr["continuetime"].ToString();
                    string sReason = dr["pausereason"].ToString();
                    string sDoUserID = dr["doUserID"].ToString();
                    string sContUserID = dr["ContUserID"].ToString();
                    string sTrPause = "<tr class='list'>" + AddTD(EPSystem.GetUserName(long.Parse(sDoUserID)) + "(" + sDoUserID + ") 于" + pTime + " <font color=blue>暂停流程</font>，理由是：" + sReason, "") + "</tr>";
                    string sTrContinue = "";
                    if (sContUserID != "" && sContUserID != "0")
                        sTrContinue = "<tr class='list'>" + AddTD(EPSystem.GetUserName(long.Parse(sContUserID)) + "(" + sContUserID + ") 于" + cTime + " <font color=green>恢复流程执行</font>", "") + "</tr>";
                    sProcess += sTrPause + sTrContinue;
                }

            }
            return sProcess;
        }

        


        private string AddTD(string sText)
        {
            string str = "<td>" + sText + "</td>";
            return str;
        }

        private string AddTD(string sText, string sAttrib)
        {
            string str = "<td " + sAttrib + " >" + sText + "</td>";
            return str;
        }

        private static string SP()
        {
            return "&nbsp;";
        }

        private static string SP(int n)
        {
            string str = "";
            for (int j = 0; j <= n; j++)
                str += "&nbsp;";

            return str;
        }


        protected override void Render(HtmlTextWriter writer)
        {
            //输出HTML
            System.IO.StringWriter html = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter tw = new System.Web.UI.HtmlTextWriter(html);
            litFlowPauseLog.RenderControl(tw);
            Response.Clear();
            //返回是否有新增 0 无, 1 有
            string sTmp = html.ToString();
            Response.Write(sTmp);
            Response.Flush();
            Response.End();
        }
    }
}
