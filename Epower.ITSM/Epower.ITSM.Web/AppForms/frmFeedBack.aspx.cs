/*******************************************************************
 * ��Ȩ���У�
 * Description���ʼ��ط�
 * 
 * 
 * Create By  ��
 * Create Date��2009-09-29
 * *****************************************************************/
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

namespace Epower.ITSM.Web.AppForms
{
    public partial class frmFeedBack : BasePage
    {
        public string sApplicationUrl = Epower.ITSM.Base.Constant.ApplicationPath;     //����·��

        private string _strCust = string.Empty;  
        // <summary>
        /// ���̱������
        /// </summary>
        public long FlowID
        {
            get
            {
                return ViewState["v_FeedBackFlowID"] == null ? 0 : long.Parse(ViewState["v_FeedBackFlowID"].ToString());
            }
            set { ViewState["v_FeedBackFlowID"] = value; }
        }
        /// <summary>
        /// Ӧ�ñ������
        /// </summary>
        public long AppID
        {
            get
            {
                return ViewState["v_FeedBackAppID"] == null ? 0 : long.Parse(ViewState["v_FeedBackAppID"].ToString());
            }
            set { ViewState["v_FeedBackAppID"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //��ֹ�û�ͨ��IE���˰�Ŧ�ظ��ύ
			Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            long lngUserID = long.Parse(Request.QueryString["userid"]);
            string sGuid = Request.QueryString["guid"];

            string sIntention = "";
            long lngmnUserID = 0;
            sIntention = APTokenDP.GetTokenInfo20111104(lngUserID, sGuid, "", ref lngmnUserID);
            if (sIntention == "")
            {
                cmdFeedBack.Enabled = false;
            }

            if (Page.IsPostBack == false)
            {
                FlowID = long.Parse(Request["FlowID"].ToString());
                InitFeedBack(this.FlowID);
                txtFeedPerson.Text = Session["PersonName"].ToString();
                txtCustName.Text = Request["CustName"].ToString();
                CtrDTFBTime.dateTime = DateTime.Now;
                if (_strCust != string.Empty)
                {
                    txtCustName.Text = _strCust;
                }
            }
            else
            {
                string strSender = Request.Form["ctl00$hidTarget"];
                string strPara = Request.Form["ctl00$hidPara"];
                if (strSender == this.ClientID)
                {
                    //�ж��Ƿ���ɾ�� �������
                    long lngFeedBackID = long.Parse(strPara);
                    this.DeleteFeedBack(lngFeedBackID);

                }
            }
        }

        #region �ط����չʾ InitFeedBack
        /// <summary>
        /// �ط����չʾ
        /// </summary>
        /// <param name="lngFlowID"></param>
        private void InitFeedBack(long lngFlowID)
        {
            DataTable dt = RiseDP.GetAllFeedBack(lngFlowID);
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
                    string sDate = dr["processsysdate"].ToString();
                    string sOpinion = dr["suggest"].ToString();
                    string sUserName = dr["dousername"].ToString();
                    string sMonID = dr["feedbackid"].ToString();
                    string sUser = dr["douser"].ToString();
                    string sFeedBack = dr["feedback"].ToString();
                    string sFeedType = dr["feedtype"].ToString();
                    string sFeedPerson = dr["feedPerson"].ToString();
                    string sCustName = dr["CustName"].ToString();
                    string sFBTime = dr["fbtime"].ToString();

                    switch (sFeedBack)
                    {
                        case "1":
                            sFeedBack = "����";
                            break;
                        case "2":
                            sFeedBack = "��������";
                            break;
                        case "3":
                            sFeedBack = "������";
                            break;
                        default:
                            break;
                    }

                    switch (sFeedType)
                    {
                        case "1":
                            sFeedType = "�绰";
                            break;
                        case "2":
                            sFeedType = "����";
                            break;
                        case "3":
                            sFeedType = "����";
                            break;
                        case "4":
                            sFeedType = "�ʼ�";
                            break;
                        default:
                            break;
                    }
                    string sTR = "";
                    if (iCount == 1)
                    {
                        //��ӱ������

                        sTR += AddTD("�ط�ʱ��&nbsp;", "noWrap class='listTitle'") +
                            AddTD("�Ǽ���&nbsp;", "nowrap class='listTitle'") +
                            AddTD("�ط���&nbsp;", "nowrap class='listTitle'") +
                            AddTD("���ط���&nbsp;", "nowrap class='listTitle'") +
                            AddTD("�ط÷�ʽ&nbsp;", "nowrap class='listTitle'") +
                             AddTD("����̶�&nbsp;", "nowrap class='listTitle'") +
                            AddTD("�ط�����", "width=100% class='listTitle'");

                        //sTR += AddTD("", "nowrap");


                        sTR = "<tr class='listTitle'>" + sTR + "</tr>";
                        sProcess += sTR;

                    }
                    //�ط����
                    sOpinion = sOpinion.Trim() == "" ? "" : "&nbsp;<font color=blue>" + StringTool.ParseForHtml(sOpinion) + "</font>";

                    sTR = "";
                    sTR += AddTD(sFBTime + "&nbsp;", "noWrap") +
                        AddTD(sUserName + "&nbsp;", "nowrap") +
                        AddTD(sFeedPerson + "&nbsp;", "nowrap") +
                        AddTD(sCustName + "&nbsp;", "nowrap") +
                         AddTD(sFeedType + "&nbsp;", "nowrap") +
                        AddTD(sFeedBack + "&nbsp;", "nowrap") +
                        AddTD(sOpinion, "width=100%");

                    sTR = "<tr class='tablebody'>" + sTR + "</tr>";
                    sProcess += sTR;
                }
                if (sProcess != "")
                {
                    sProcess = @"<TABLE id='Table1' cellSpacing='0' cellPadding='0' width='100%' border='0'>
					<TR>
						<TD  class='list'><table cellspacing='2' cellpadding='1' rules='all' border='0' style='border-color:White;border-width:0px;width:100%;'  >" + sProcess;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdFeedBack_Click(object sender, EventArgs e)
        {

            RiseDP.AddFeedBack(this.FlowID, long.Parse(Session["UserID"].ToString()), txtFeedBack.Text.Trim(), Session["PersonName"].ToString(), int.Parse(this.rblFeedBack.SelectedValue), this.AppID, txtFeedPerson.Text.Trim(), txtCustName.Text.Trim(), int.Parse(rboFeedType.SelectedValue), CtrDTFBTime.dateTime.ToString("yyyy-MM-dd H:mm:ss"));
            InitFeedBack(this.FlowID);
            ZHServiceDP.UpdateEmailStateTwo(FlowID);

            //PageTool.MsgBox(this, "�ύ�ɹ���");

            string sGuid = Request.QueryString["guid"];
            APTokenDP.SetAPTokenSatus(sGuid,string.Empty);
            cmdFeedBack.Enabled = false;
            
        }

        /// <summary>
        /// ɾ���ط����
        /// </summary>
        /// <param name="?"></param>
        public void DeleteFeedBack(long lngFeedBackID)
        {
            RiseDP.DeleteFeedBack(lngFeedBackID);
            InitFeedBack(this.FlowID);

            cmdFeedBack.Enabled = true;
        }
    }
}
