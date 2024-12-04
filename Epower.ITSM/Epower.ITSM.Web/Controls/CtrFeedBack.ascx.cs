/*******************************************************************
 * 版权所有：
 * Description：回访控件
 * 
 * 
 * Create By  ：
 * Create Date：
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
using Epower.ITSM.Base;

namespace Epower.ITSM.Web.Controls
{
    public partial class CtrFeedBack : System.Web.UI.UserControl
    {
        private string _strCust = string.Empty;


        private eOA_FlowControlState cState = eOA_FlowControlState.eNormal;
        /// <summary>
        /// 设置控件状态
        /// </summary>
        public eOA_FlowControlState ContralState
        {
            get { return cState; }
            set { cState = value; }
        }

        /// <summary>
        /// 流程编号属性
        /// </summary>
        public long FlowID
        {
            get
            {
                return ViewState["v_FeedBackFlowID"] == null ? 0 : long.Parse(ViewState["v_FeedBackFlowID"].ToString());
            }
            set { ViewState["v_FeedBackFlowID"] = value; }
        }

        private string strFeedBack
        {
            get {
                return ViewState["strFeedBack"] == null ? "" : ViewState["strFeedBack"].ToString();
            }
            set { ViewState["strFeedBack"] = value; }
        }


        /// <summary>
        /// 默认被回访人属性
        /// </summary>
        public string  FeedBackCustomer
        {
           
            set { _strCust = value; }
        }



        /// <summary>
        /// 应用编号属性
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
        public bool TimeEnable
        {
            get { return ViewState["v_FeedBackVisible"] == null ? true : (bool)ViewState["v_FeedBackVisible"]; }
            set { ViewState["v_FeedBackVisible"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool DealVisible
        {
            get { return ViewState["v_DealVisible"] == null ? true : (bool)ViewState["v_DealVisible"]; }
            set { ViewState["v_DealVisible"] = value; }
        }

        //回访保存后的事件定义
        public delegate void DoActionAfterSave(long lngFlowID);

        /// <summary>
        /// 回访保存后的事件接口
        /// </summary>
        public event DoActionAfterSave myDoActionAfterSave;
 

        protected void Page_Load(object sender, EventArgs e)
        {
            if (cState == eOA_FlowControlState.eReadOnly) this.tbEdit.Visible = false;

            strFeedBack = txtFeedBack.Text;
            if (Page.IsPostBack == false)
            {
                InitFeedBack(this.FlowID);
                txtFeedPerson.Text = Session["PersonName"].ToString();
                if (_strCust != string.Empty)
                {
                    txtCustName.Text = _strCust;
                }
                if (!TimeEnable)
                {
                    CtrDTFBTime.Visible = false;
                }

                if (DealVisible)
                    this.cmdFeedBack.Visible = true;
                else
                    this.cmdFeedBack.Visible = false;
            }
            else
            {
                string strSender = Request.Form["ctl00$hidTarget"];
                string strPara = Request.Form["ctl00$hidPara"];
                if (strSender == this.ClientID)
                {
                    //判断是否是删除 督办意见
                    long lngFeedBackID = long.Parse(strPara);
                    this.DeleteFeedBack(lngFeedBackID);

                }
            }
        }


        #region 回访情况展示 InitFeedBack
        /// <summary>
        /// 回访情况展示
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
                            sFeedBack = "满意";
                            break;
                        case "2":
                            sFeedBack = "基本满意";
                            break;
                        case "3":
                            sFeedBack = "不满意";
                            break;
                        default:
                            break;
                    }

                    switch (sFeedType)
                    {
                        case "1":
                            sFeedType = "电话";
                            break;
                        case "2":
                            sFeedType = "上门";
                            break;
                        case "3":
                            sFeedType = "其它";
                            break;
                        case "4":
                            sFeedType = "邮件";
                            break;
                        default:
                            break;
                    }
                    string sTR = "";
                    if (iCount == 1)
                    {
                        //添加标题情况

                        sTR += AddTD("回访时间", "nowrap width=15%") +
                            AddTD("登记人", "nowrap width=8%") +
                            AddTD("回访人", "nowrap width=8%") +
                            AddTD("被回访者", "nowrap width=8%") +
                            AddTD("回访方式", "nowrap width=8%") +
                             AddTD("满意程度", "nowrap width=8%") +
                            AddTD("回访内容", "width=60%");

                        sTR += AddTD("处理", "nowrap");


                        sTR = "<tr class='listTitleNew_1' align='center'>" + sTR + "</tr>";
                        sProcess += sTR;

                    }
                    //回访情况
                    sOpinion = sOpinion.Trim() == "" ? "" : "&nbsp;<font color=blue>" + StringTool.ParseForHtml(sOpinion) + "</font>";

                    sTR = "";
                    sTR += AddTD(sFBTime, "nowrap") +
                        AddTD(sUserName, "nowrap") +
                        AddTD(sFeedPerson, "nowrap") +
                        AddTD(sCustName, "nowrap") +
                         AddTD(sFeedType, "nowrap") +
                        AddTD(sFeedBack, "nowrap") +
                        AddTD(sOpinion, "width=60%");
                    if (lngUserID == long.Parse(sUser))
                    {
                        sTR += AddTD(@"<INPUT type='button' class='btnClass1' id='btnDelf" + iCount.ToString() + @"' value='删除' onClick=""__doPostBackCustomize('" + this.ClientID + "','" + sMonID + @"');return false;"">", "nowrap");
                    }
                    else
                    {
                        sTR += AddTD("", "nowrap");
                    }

                    sTR = "<tr>" + sTR + "</tr>";
                    sProcess += sTR;
                }
                if (sProcess != "")
                {
                    sProcess = @"<TABLE cellSpacing='1' cellPadding='1' width='100%' border='0' class='gridTable'>
					<TR>
						<TD  class='listTitle' style='font-weight:bold'>&nbsp;&nbsp;回访情况</TD>
					</TR>
					<TR>
						<TD><table cellspacing='1' cellpadding='1' rules='all' border='0' class='gridTable'>" + sProcess;
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

            //执行保存后的事件
            if (myDoActionAfterSave != null)
                myDoActionAfterSave(this.FlowID);


            txtFeedBack.Text = "";
           // InitFeedBack(lngFlowID);
        }

        public void SaveFeedBack(object sender, EventArgs e)
        {
            E8Logger.Info("SaveFeedBack.");
         
            cmdFeedBack_Click(sender, e);
        }

        /// <summary>
        /// 删除回访情况
        /// </summary>
        /// <param name="?"></param>
        public void DeleteFeedBack(long lngFeedBackID)
        {
            RiseDP.DeleteFeedBack(lngFeedBackID);
            InitFeedBack(this.FlowID);
        }
    }
}