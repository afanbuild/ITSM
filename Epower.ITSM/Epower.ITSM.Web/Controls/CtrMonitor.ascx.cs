/*****************************************************************************
 * 
 * Description:����ؼ�
 * 
 * 
 * 
 * Create by : mczhu
 * Create Date: 2007-08-28
 * ******************************************************************/
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
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CtrMonitor : System.Web.UI.UserControl
    {
        #region ������
        /// <summary>
        /// ���̱������
        /// </summary>
        public long FlowID
        {
            get
            {
                return ViewState["v_MonitorFlowID"] == null ? 0 : long.Parse(ViewState["v_MonitorFlowID"].ToString());
            }
            set { ViewState["v_MonitorFlowID"] = value; }
        }

        /// <summary>
        /// Ӧ�ñ������
        /// </summary>
        public long AppID
        {
            get
            {
                return ViewState["v_MonitorAppID"] == null ? 0 : long.Parse(ViewState["v_MonitorAppID"].ToString());
            }
            set { ViewState["v_MonitorAppID"] = value; }
        }

        /// <summary>
        /// �Ƿ�ɲ���
        /// </summary>
        public bool DealVisible
        {
            get { return ViewState["v_DealMonitoVisible"] == null ? true : (bool)ViewState["v_DealMonitoVisible"]; }
            set { ViewState["v_DealMonitoVisible"] = value; }
        }
        #endregion 

        //���챣�����¼�����
        public delegate void DoActionAfterSave(long lngFlowID);

        /// <summary>
        /// ���챣�����¼��ӿ�
        /// </summary>
        public event DoActionAfterSave myDoActionAfterSave;


        #region ҳ����� Page_Load
        /// <summary>
        /// ҳ�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                InitMonitor(this.FlowID);
                txtMonitor.Visible = DealVisible;
                cmdMonitor.Visible = DealVisible;
            }
            else
            {
                string strSender = Request.Form["ctl00$hidTarget"];
                string strPara = Request.Form["ctl00$hidPara"];
                if (strSender == this.ClientID)
                {
                    //�ж��Ƿ���ɾ�� �������
                    long lngMonitorID = long.Parse(strPara);
                    this.DeleteMonitor(lngMonitorID);
                }
            }
        }
        #endregion 

        #region �������չʾ InitMonitor
        /// <summary>
        /// �������չʾ
        /// </summary>
        /// <param name="lngFlowID"></param>
        private void InitMonitor(long lngFlowID)
        {
            DataTable dt = MonitorDP.GetMonitor(lngFlowID);
            int iCount = 0;
            long lngUserID = long.Parse(Session["UserID"].ToString());
            string sProcess = "";
            if (dt.Rows.Count == 0)
            {
                ltlHFList.Text = "";
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    iCount++;
                    string sMonID = dr["MonitorID"].ToString();
                    string sUser = dr["RegUserID"].ToString();
                    string sUserName = dr["RegUserName"].ToString();
                    string sDate = dr["RegTime"].ToString();
                    string sOpinion = dr["Suggest"].ToString();
                    
                    string sTR = "";
                    if (iCount == 1)
                    {
                        //��ӱ������
                        sTR += AddTD("����ʱ��&nbsp;", "noWrap class='list'") +
                            AddTD("������&nbsp;", "nowrap class='list'") +
                            AddTD("��������", "width=100% class='list'");

                        sTR += AddTD("", "nowrap class='list'");


                        sTR = "<tr class='listTitle'>" + sTR + "</tr>";
                        sProcess += sTR;

                    }
                    //�������
                    sOpinion = sOpinion.Trim() == "" ? "" : "&nbsp;<font color=blue>" + StringTool.ParseForHtml(sOpinion) + "</font>";

                    sTR = "";
                    sTR += AddTD(sDate + "&nbsp;", "noWrap class='list'") +
                        AddTD(sUserName + "&nbsp;", "nowrap class='list'") +
                        AddTD(sOpinion, "width=100% class='list'");
                    if (lngUserID == long.Parse(sUser) && DealVisible)
                    {
                        sTR += AddTD(@"<INPUT class='btnClass' type='button' id='btnMonitor" + iCount.ToString() + @"' value='ɾ��' onClick=""__doPostBackCustomize('" + this.ClientID + "','" + sMonID + @"');return false;"">", "nowrap");
                    }
                    else
                    {
                        sTR += AddTD("", "nowrap class='list'");
                    }

                    sTR = "<tr class='listContent'>" + sTR + "</tr>";
                    sProcess += sTR;
                }
                if (sProcess != "")
                {
                    sProcess = @"<TABLE id='Table1' width='100%' class='listContent'>
					<TR vAlign='top' class='listTitle'>
						<TD  class='listTitle'>&nbsp;&nbsp;�������</TD>
					</TR>
					<TR>
						<TD  class='list'><table style='width:100%;' class='listContent'>" + sProcess;
                    ltlHFList.Text = sProcess + "</table></td></tr></table>";
                }
                else
                    ltlHFList.Text = SpecTransText(sProcess);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sText"></param>
        /// <param name="sAttrib"></param>
        /// <returns></returns>
        private string AddTD(string sText, string sAttrib)
        {
            string str = "<td " + sAttrib + " >" + sText + "</td>";
            return str;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string SpecTransText(string str)
        {
            string strR = str;
            if (str == "")
            {
                strR = "--";
            }
            if (str == "--")
            {
                strR = "";
            }
            return strR;
        }
        #endregion 

        #region ��Ӷ��� cmdMonitor_Click
        /// <summary>
        /// ��Ӷ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdMonitor_Click(object sender, EventArgs e)
        {

            MonitorDP.AdMonitor(this.FlowID, long.Parse(Session["UserID"].ToString()), txtMonitor.Text.Trim(), Session["PersonName"].ToString(), this.AppID);
            txtMonitor.Text = string.Empty;
            InitMonitor(this.FlowID);
            //ִ�б������¼�
            if (myDoActionAfterSave != null)
                myDoActionAfterSave(this.FlowID);


            txtMonitor.Text = "";
        }
        #endregion 

        #region ɾ������ DeleteMonitor
        /// <summary>
        /// ɾ���������
        /// </summary>
        /// <param name="?"></param>
        public void DeleteMonitor(long lngMonitorID)
        {
            MonitorDP.DeleteMonitor(lngMonitorID);
            InitMonitor(this.FlowID);
        }
        #endregion 
    }
}