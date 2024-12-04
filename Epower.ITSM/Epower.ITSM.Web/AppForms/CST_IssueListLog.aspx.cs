using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using EpowerGlobal;
using Epower.DevBase.Organization.SqlDAL;


namespace Epower.ITSM.Web.AppForms
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CST_IssueListLog : BasePage
    {
        #region 设置父窗体按钮事件SetParentButtonEvent
        /// <summary>
        /// 设置父窗体按钮事件
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            
            this.Master.ShowQueryButton(true);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtonEvent();
            cpCST_Issue.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(GridBind);
            if (!Page.IsPostBack)
            {
                SetHeaderText();

                #region 绑定流程状态下拉列表
                cboStatus.Items.Add(new ListItem("所有状态", "-1"));
                cboStatus.Items.Add(new ListItem("--正在处理", ((int)e_FlowStatus.efsHandle).ToString()));
                cboStatus.Items.Add(new ListItem("--正常结束", ((int)e_FlowStatus.efsEnd).ToString()));
                cboStatus.Items.Add(new ListItem("--流程暂停", ((int)e_FlowStatus.efsStop).ToString()));
                cboStatus.Items.Add(new ListItem("--流程终止", ((int)e_FlowStatus.efsAbort).ToString()));

                cboStatus.SelectedIndex = 1;
                #endregion 

                #region 设置起始日期
                //设置起始日期
                string sQueryBeginDate = "0";
                //if (CommonDP.GetConfigValue("Other", "QueryBeginDate") != null)
                //    sQueryBeginDate = CommonDP.GetConfigValue("Other", "QueryBeginDate").ToString();
                if (sQueryBeginDate == "0")
                {
                    txtMsgDateBegin.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                }
                else
                {
                    txtMsgDateBegin.Text = DateTime.Parse(sQueryBeginDate).ToString("yyyy-MM-dd");
                }
                txtMsgDateEnd.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                this.imgSBegin.Attributes.Add("onclick", "fPopUpDlg('../Controls/Calendar/calendar.htm',document.all." + this.txtMsgDateBegin.ClientID + ", 'winpop', 234, 261);return false");
                this.imgEEnd.Attributes.Add("onclick", "fPopUpDlg('../Controls/Calendar/calendar.htm',document.all." + this.txtMsgDateEnd.ClientID + ", 'winpop', 234, 261);return false");
                #endregion 

                string strParam = string.Empty;
                if (Request.QueryString["param"] != null)
                {
                    strParam = Request.QueryString["param"].ToString();
                    if (Request.QueryString["svalue"] != null)
                    {
                        txtCustInfo.Text = Request.QueryString["svalue"].ToString();
                    }
                    GridBind();
                }
                Session["FromUrl"] = "../AppForms/CST_IssueListLog.aspx?param=" + strParam;
             
            }
        }

        /// <summary>
        /// 设置列头名称 廖世进 2013-05-16
        /// </summary>
        void SetHeaderText()
        {
            gridUndoMsg.Columns[1].HeaderText = PageDeal.GetLanguageValue("CST_Header_ServiceNO");
            gridUndoMsg.Columns[2].HeaderText = PageDeal.GetLanguageValue("CST_Header_CustName");
            gridUndoMsg.Columns[3].HeaderText = PageDeal.GetLanguageValue("CST_Header_CustPhone");
            gridUndoMsg.Columns[4].HeaderText = PageDeal.GetLanguageValue("CST_Header_ServiceLevel");
            gridUndoMsg.Columns[5].HeaderText = PageDeal.GetLanguageValue("CST_Header_CustTime");
            gridUndoMsg.Columns[6].HeaderText = PageDeal.GetLanguageValue("CST_Header_EquName");
            gridUndoMsg.Columns[7].HeaderText = PageDeal.GetLanguageValue("CST_Header_Subject");
            gridUndoMsg.Columns[8].HeaderText = PageDeal.GetLanguageValue("CST_Header_Content");

        }

        #region 查询事件Master_Master_Button_Query_Click
        /// <summary>
        /// 查询事件
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            GridBind();
        }
        #endregion 

        //绑定grid
        /// <summary>
        /// 
        /// </summary>
        public void GridBind()
        {
            string strParam = string.Empty;
            if (Request.QueryString["param"] != null)
            {
                strParam = Request.QueryString["param"].ToString();
            }
           DataTable  dt =new DataTable();
           int iRowCount = 0;
           RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.CustomerService];
           switch (strParam)
           {

               case "isMeCheck":
                   //由我登记的事件

                   dt = ZHServiceDP.getProccess(eOA_TocdProccess.isMeCheck, long.Parse(Session["UserID"].ToString()), txtCustInfo.Text.Trim(), cboStatus.SelectedValue.ToString(), CtrFCDDealStatus.CatelogID.ToString(), txtMsgDateBegin.Text.ToString(), txtMsgDateEnd.Text.ToString(), long.Parse(Session["UserDeptID"].ToString())
                   , long.Parse(Session["UserOrgID"].ToString()), reTrace, this.cpCST_Issue.PageSize, this.cpCST_Issue.CurrentPage, ref iRowCount);
                   break;
               case "isMePartakeCheck":
                   //我参与处理的
                   dt = ZHServiceDP.getProccess(eOA_TocdProccess.isMePartakeCheck, long.Parse(Session["UserID"].ToString()), txtCustInfo.Text.Trim(), cboStatus.SelectedValue.ToString(), CtrFCDDealStatus.CatelogID.ToString(), txtMsgDateBegin.Text.ToString(), txtMsgDateEnd.Text.ToString(), long.Parse(Session["UserDeptID"].ToString())
                   , long.Parse(Session["UserOrgID"].ToString()), reTrace, this.cpCST_Issue.PageSize, this.cpCST_Issue.CurrentPage, ref iRowCount);
                   break;
               case "overtimeEvent":
                   //超时的


                   dt = ZHServiceDP.getProccess(eOA_TocdProccess.overtimeEvent, long.Parse(Session["UserID"].ToString()), txtCustInfo.Text.Trim(), cboStatus.SelectedValue.ToString(), CtrFCDDealStatus.CatelogID.ToString(), txtMsgDateBegin.Text.ToString(), txtMsgDateEnd.Text.ToString(), long.Parse(Session["UserDeptID"].ToString())
                   , long.Parse(Session["UserOrgID"].ToString()), reTrace, this.cpCST_Issue.PageSize, this.cpCST_Issue.CurrentPage, ref iRowCount);
                   break;
               case "overtime48Event":
                   //超时48小时的


                   dt = ZHServiceDP.getProccess(eOA_TocdProccess.overtime48Event, long.Parse(Session["UserID"].ToString()), txtCustInfo.Text.Trim(), cboStatus.SelectedValue.ToString(), CtrFCDDealStatus.CatelogID.ToString(), txtMsgDateBegin.Text.ToString(), txtMsgDateEnd.Text.ToString(), long.Parse(Session["UserDeptID"].ToString())
                   , long.Parse(Session["UserOrgID"].ToString()), reTrace, this.cpCST_Issue.PageSize, this.cpCST_Issue.CurrentPage, ref iRowCount);
                   break;
               case "notReturnVisit":   //未回访事件

                   dt = ZHServiceDP.getProccess(eOA_TocdProccess.notReturnVisit, long.Parse(Session["UserID"].ToString()), txtCustInfo.Text.Trim(), cboStatus.SelectedValue.ToString(), CtrFCDDealStatus.CatelogID.ToString(), txtMsgDateBegin.Text.ToString(), txtMsgDateEnd.Text.ToString(), long.Parse(Session["UserDeptID"].ToString())
                   , long.Parse(Session["UserOrgID"].ToString()), reTrace, this.cpCST_Issue.PageSize, this.cpCST_Issue.CurrentPage, ref iRowCount);
                   //获取评估(回访)权限
                   bool blnDisplayFeedBack = CheckRight(Constant.feedbackright);
                   if (blnDisplayFeedBack == true && CommonDP.GetConfigValue("Other", "IsEmailFeedBack") != null && CommonDP.GetConfigValue("Other", "IsEmailFeedBack") == "0")
                   {
                       tblEmail.Visible = true;
                       gridUndoMsg.Columns[0].Visible = true;
                   }
                   break;
               case "overtimeEventfulfill":
                   //超时完成
                   dt = ZHServiceDP.getProccess(eOA_TocdProccess.overtimeEventfulfill, long.Parse(Session["UserID"].ToString()), txtCustInfo.Text.Trim(), cboStatus.SelectedValue.ToString(), CtrFCDDealStatus.CatelogID.ToString(), txtMsgDateBegin.Text.ToString(), txtMsgDateEnd.Text.ToString(), long.Parse(Session["UserDeptID"].ToString())
                   , long.Parse(Session["UserOrgID"].ToString()), reTrace, this.cpCST_Issue.PageSize, this.cpCST_Issue.CurrentPage, ref iRowCount);
                   break;
               case "overtimeEventNOfulfill":
                   //超时未完成

                   dt = ZHServiceDP.getProccess(eOA_TocdProccess.overtimeEventNOfulfill, long.Parse(Session["UserID"].ToString()), txtCustInfo.Text.Trim(), cboStatus.SelectedValue.ToString(), CtrFCDDealStatus.CatelogID.ToString(), txtMsgDateBegin.Text.ToString(), txtMsgDateEnd.Text.ToString(), long.Parse(Session["UserDeptID"].ToString())
                   , long.Parse(Session["UserOrgID"].ToString()), reTrace, this.cpCST_Issue.PageSize, this.cpCST_Issue.CurrentPage, ref iRowCount);
                   break;
               default:
                   //由我登记的事件

                   dt = ZHServiceDP.getProccess(eOA_TocdProccess.isMeCheck, long.Parse(Session["UserID"].ToString()), txtCustInfo.Text.Trim(), cboStatus.SelectedValue.ToString(), CtrFCDDealStatus.CatelogID.ToString(), txtMsgDateBegin.Text.ToString(), txtMsgDateEnd.Text.ToString(), long.Parse(Session["UserDeptID"].ToString())
                   , long.Parse(Session["UserOrgID"].ToString()), reTrace, this.cpCST_Issue.PageSize, this.cpCST_Issue.CurrentPage, ref iRowCount);
                   break;
           }

           gridUndoMsg.DataSource = dt;
           gridUndoMsg.DataBind();
         //  int iRowCount = 0;
           this.cpCST_Issue.RecordCount = iRowCount;
           this.cpCST_Issue.Bind();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridUndoMsg_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ((Label)e.Item.FindControl("Lb_ServiceNo")).Attributes.Add("onmouseover", "ShowDetailsInfo(this," + DataBinder.Eval(e.Item.DataItem, "SMSID").ToString() + ",400);");
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                string sFlowID = DataBinder.Eval(e.Item.DataItem, "FlowID").ToString();
                e.Item.Attributes.Add("ondblclick", "window.open('../Forms/frmIssueView.aspx?FlowID=" + sFlowID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridUndoMsg_ItemCreated(object sender, DataGridItemEventArgs e)
        {
        
                if (e.Item.ItemType == ListItemType.Header)
                {
                    DataGrid dg = (DataGrid)sender;
                    for (int i = 0; i < e.Item.Cells.Count; i++)
                    {
                        if (Request.QueryString["param"].ToString() == "notReturnVisit")
                        {
                            bool blnDisplayFeedBack = CheckRight(Constant.feedbackright);
                            if (blnDisplayFeedBack == true && CommonDP.GetConfigValue("Other", "IsEmailFeedBack") != null && CommonDP.GetConfigValue("Other", "IsEmailFeedBack") == "0")
                            {
                                if (i > 0 && i < 9)
                                {
                                    e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + i.ToString() + ",0);");
                                }
                            }
                            else
                            {
                                if (i < 9)
                                {
                                    e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + (i - 1).ToString() + ",0);");
                                }
                            }
                        }
                        else  
                        {
                            if (i < 9)
                            {
                                e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + (i - 1).ToString() + ",0);");
                            }
                        }
                    }
                }
        
        }

        /// <summary>
        /// 批量邮件回访
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSendMail_Click1(object sender, EventArgs e)
        {
            foreach (DataGridItem itm in gridUndoMsg.Items)
            {
                if (itm.ItemType == ListItemType.AlternatingItem ||
                    itm.ItemType == ListItemType.Item)
                {
                    long lngFlowID = long.Parse(itm.Cells[gridUndoMsg.Columns.Count - 5].Text);
                    int status = int.Parse(itm.Cells[gridUndoMsg.Columns.Count - 1].Text);
                    CheckBox chkdel = (CheckBox)itm.Cells[0].FindControl("chkDel");
                    if (chkdel.Checked && status == (int)e_FlowStatus.efsEnd)
                    {
                        //发送邮件
                        string sEmail = string.Empty;
                        string sCustName = string.Empty;
                        string sSubject = CommonDP.GetConfigValue("Other", "ServiceAutoFeedBackTitle");
                        string strSQL = "SELECT a.Subject,b.Email,b.ShortName FROM Cst_Issues a,Br_ECustomer b WHERE a.CustID=b.ID And a.FlowID=" + lngFlowID.ToString();
                        DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
                        foreach (DataRow dr in dt.Rows)
                        {
                            sEmail = dr["Email"].ToString();
                            sCustName = dr["ShortName"].ToString();
                            sSubject += dr["Subject"].ToString();
                            break;
                        }
                        MailSendDeal.EmailFeedBack(lngFlowID, sEmail, sCustName, sSubject);

                        if (sEmail != string.Empty)
                        {
                            ZHServiceDP.UpdateEmailState(lngFlowID);
                        }
                    }
                }
            }
            Epower.DevBase.BaseTools.PageTool.MsgBox(this, "批量邮件回访成功！");
        }

        protected string GetUrl(decimal lngFlowID)
        {
            //暂时没处理分页
            string sUrl = "";

            sUrl = "javascript:window.open('../Forms/frmIssueView.aspx?FlowID=" + lngFlowID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";

            return sUrl;


        }

        protected string GetButtonValue(int status)
        {
            string strRet = "详情";

            if (status == (int)e_FlowStatus.efsEnd && CheckRight(Constant.feedbackright)==true)
            {
                strRet = "详情/回访";

            }
            return strRet;

        }


        #region 检查权限 CheckRight
        /// <summary>
        /// 
        /// </summary>
        /// <param name="OperatorID"></param>
        /// <returns></returns>
        private bool CheckRight(long OperatorID)
        {
            RightEntity re = (RightEntity)((Hashtable)Session["UserAllRights"])[OperatorID];
            if (re == null)
                return false;
            else
                return re.CanRead;
        }
        #endregion 
    }
}
