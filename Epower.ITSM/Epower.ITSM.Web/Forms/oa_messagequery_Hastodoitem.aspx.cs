using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Xml;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using EpowerCom;
using EpowerGlobal;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;

namespace Epower.ITSM.Web.Forms
{
    /// <summary>
    /// oa_messagequery_Hastodoitem 的摘要说明。
    /// </summary>
    public partial class oa_messagequery_Hastodoitem : BasePage
    {
        /// <summary>
        /// 当前选择的标签
        /// </summary>
        protected eOA_ListBookMark eCurr
        {
            get
            {
                if (ViewState["eCurr"] == null)
                    return (eOA_ListBookMark)ViewState["eCurr"];
                else
                    return eOA_ListBookMark.eEmpty;
            }
            set
            {
                ViewState["eCurr"] = value;
            }
        }

        #region 获取点击更多 进入的type类型
        /// <summary>
        /// 获取点击更多 进入的type类型
        /// </summary>
        protected long AppType
        {
            get
            {
                if (Request["type"] != null && Request["type"] != "")
                {
                    return long.Parse(Request["type"].ToString());
                }
                else
                    return -1;
            }
        }
        #endregion

        #region 控件


        #endregion

        /// <summary>
        /// 设置母版页面按钮
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.TableVisible = false;
        }

        protected string GetStatusImage(int isRead)
        {
            string ret = @"..\Images\page\flow_status_newnormal.gif";
            if (isRead > 0)
                ret = @"..\Images\page\flow_status_normal.gif";
            return ret;
        }

        protected bool GetVisible(int i)
        {
            bool t = false;
            if (i > 0)
                t = true;
            return t;
        }



        protected bool GetImportanceVisible(int i)
        {
            bool t = false;
            if (i == 2)
                t = true;
            return t;
        }




        /// <summary>
        /// 接收的ＵＲＬ
        /// </summary>
        /// <param name="lngNoticeID"></param>
        /// <returns></returns>
        protected string GetUrl(decimal lngMessageID, int iStatus)
        {
            //暂时没处理分页
            string sUrl = "";
            if (iStatus == (int)e_MessageStatus.emsFinished)
            {
                sUrl = "javascript:window.open('flow_Finished.aspx?MessageID=" + lngMessageID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
            }
            else
            {
                sUrl = "javascript:window.open('flow_Normal.aspx?MessageID=" + lngMessageID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
            }
            return sUrl;


        }

        /// <summary>
        /// 获取快照的脚本
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <param name="lngAppID"></param>
        /// <returns></returns>
        protected string GetFlowShotInfo(decimal lngFlowID, decimal lngAppID)
        {
            return "GetFlowShotInfo(this," + lngFlowID.ToString() + "," + lngAppID.ToString() + ");";
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            SetParentButtonEvent();
            CtrTitle1.Title = "所有事项查询";
            cpFlow.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(BindData);
            if (!Page.IsPostBack)
            {
                //string type = Request["type"];

                Session["PersonAttention"] = AttentionDP.GetMyAttention(long.Parse(Session["UserID"].ToString()));
                BindDataBookMark(eOA_ListBookMark.eEmpty);
                Session["FromUrl"] = Constant.ApplicationPath + "/Forms/OA_MessageQuery.aspx";

                //设置主页点击更多 默认应用
                ctrmessagequery_Hastodoitem1.AppID = AppType;
            }

        }

        private void BindData()
        {
            BindDataBookMark(eCurr);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iBMK"></param>
        private void BindDataBookMark(eOA_ListBookMark iBMK)
        {
            eCurr = iBMK;
            DataTable dt = new DataTable();
            int iRowCount = 0;
            string sWhere = string.Empty;
            string sOrder = string.Empty;
            sOrder = "  ORDER BY MessageID DESC";
            if (iBMK == eOA_ListBookMark.eEmpty)
            {
                XmlDocument xmlDoc = ctrmessagequery_Hastodoitem1.GetAllValues();
                dt = MessageBookMarkDP.GetMessagesForCond(xmlDoc.InnerXml, sOrder, this.cpFlow.PageSize, this.cpFlow.CurrentPage, ref iRowCount);
            }
            else
            {
                dt = MessageBookMarkDP.GetMessageBookMarkList((long)Session["UserID"], iBMK, sOrder, this.cpFlow.PageSize, this.cpFlow.CurrentPage, ref iRowCount);
            }

            gridUndoMsg.DataSource = dt.DefaultView;
            gridUndoMsg.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");
            gridUndoMsg.DataBind();
            this.cpFlow.RecordCount = iRowCount;
            this.cpFlow.Bind();
        }

        #region Web 窗体设计器生成的代码
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.gridUndoMsg.ItemCreated += new System.Web.UI.WebControls.DataGridItemEventHandler(this.gridUndoMsg_ItemCreated);
            this.gridUndoMsg.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.gridUndoMsg_ItemDataBound);

        }
        #endregion

        protected void cmdQuery_Click(object sender, System.EventArgs e)
        {
            eCurr = eOA_ListBookMark.eEmpty;
            BindData();
        }

        private void gridUndoMsg_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                switch ((e_ActorClass)(int.Parse(e.Item.Cells[13].Text.Trim())))
                {
                    case e_ActorClass.fmMasterActor:
                        e.Item.Cells[13].Text = "主办";
                        break;
                    case e_ActorClass.fmAssistActor:
                        e.Item.Cells[13].Text = "协办";
                        break;
                    case e_ActorClass.fmInfluxActor:
                        e.Item.Cells[13].Text = "会签";
                        break;
                    case e_ActorClass.fmReaderActor:
                        e.Item.Cells[13].Text = "阅知";
                        break;
                    case e_ActorClass.fmCommunicActor:
                        e.Item.Cells[13].Text = "沟通";
                        break;
                    default:
                        break;
                }


                //当超过预计处理时间未处理的，红低显示
                if (int.Parse(e.Item.Cells[9].Text.Trim()) < 0)
                {
                    for (int i = 0; i < e.Item.Cells.Count; i++)
                    {
                        e.Item.Cells[i].BackColor = Color.DarkOrange;
                    }
                }

                if (long.Parse(e.Item.Cells[10].Text.Trim()) == (long)e_AppTypes.eatCommon)
                {
                    //通用流程特殊处理
                    if (e.Item.Cells[11].Text.Trim().StartsWith(e.Item.Cells[6].Text.Trim()))
                    {
                        e.Item.Cells[6].Text = e.Item.Cells[10].Text;
                    }
                    else
                    {
                        e.Item.Cells[6].Text = e.Item.Cells[6].Text.Trim() + "[" + e.Item.Cells[11].Text + "]";
                    }
                }

                //判断是否已经加入了关注
                string sMessageID = e.Item.Cells[8].Text;
                sMessageID = sMessageID == "" ? "0" : sMessageID;
                DataTable dt = (DataTable)Session["PersonAttention"];
                DataRow[] dtrows = dt.Select("messageid=" + sMessageID);
                if (dtrows.Length > 0)
                {
                    CheckBox chkAttention = (CheckBox)e.Item.Cells[e.Item.Cells.Count - 1].FindControl("chkAttention");
                    chkAttention.Visible = false;
                }

                //增加双击效果
                int status = Convert.ToInt32(e.Item.Cells[12].Text.ToString());
                e.Item.CssClass = "hand";
                if (status == (int)e_MessageStatus.emsFinished)
                {
                    e.Item.Attributes.Add("onclick", "window.open('flow_Finished.aspx?MessageID=" + sMessageID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
                }
                else
                {
                    e.Item.Attributes.Add("onclick", "window.open('flow_Normal.aspx?MessageID=" + sMessageID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
                }

                #region 增加 处理时效的显示
                if (eCurr == eOA_ListBookMark.eEmpty)
                {
                    //判断是否是事件 如果是展示处理时效
                    if (AppType == 1026 || ctrmessagequery_Hastodoitem1.AppID == 1026)
                    {
                        string sFlowID = DataBinder.Eval(e.Item.DataItem, "FlowID").ToString();

                        DataTable dt3 = ZHServiceDP.GetFlowBusLimitByFlowID(decimal.Parse(sFlowID == "" ? "0" : sFlowID));
                        
                        DataTable dt2 = ZHServiceDP.GetEventData(long.Parse(sFlowID == "" ? "0" : sFlowID));

                        if (dt2 != null && dt2.Rows.Count > 0)
                        {
                            DateTime FinishedTime = dt2.Rows[0]["FinishedTime"].ToString() == "" ? DateTime.Now : DateTime.Parse(dt2.Rows[0]["FinishedTime"].ToString());

                            if (dt3 != null && dt3.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt3.Rows.Count; i++)
                                {
                                    DateTime LimitTime = dt3.Rows[i]["LimitTime"].ToString() == "" ? DateTime.Now : DateTime.Parse(dt3.Rows[i]["LimitTime"].ToString());
                                    TimeSpan ts1 = LimitTime.Subtract(FinishedTime);
                                    //TimeSpan ts2 = LimitTime.Subtract(outtime);
                                    if (dt3.Rows[i]["GuidID"].ToString() == "10001")
                                    {
                                        string str1 = "";
                                        if (ts1.Days != 0)
                                            str1 += (ts1.Days > 0 ? ts1.Days : -ts1.Days) + "天";
                                        if (ts1.Hours != 0)
                                            str1 += (ts1.Hours > 0 ? ts1.Hours : -ts1.Hours) + "小时";
                                        if (ts1.Minutes != 0)
                                            str1 += (ts1.Minutes > 0 ? ts1.Minutes : -ts1.Minutes) + "分钟";
                                        if (ts1.Seconds != 0)
                                            str1 += (ts1.Seconds > 0 ? ts1.Seconds : -ts1.Seconds) + "秒";

                                        if (ts1.Days < 0 || ts1.Hours < 0 || ts1.Minutes < 0 || ts1.Seconds < 0)
                                        {
                                            e.Item.Cells[7].Text = "超" + str1;
                                            e.Item.Cells[7].ForeColor = Color.Red;
                                        }
                                        else
                                        {
                                            e.Item.Cells[7].Text = "还剩" + str1;
                                        }
                                    }
                                }
                            }

                            e.Item.Cells[7].Font.Bold = true;
                        }
                    }
                }
                #endregion

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdAttention_Click(object sender, System.EventArgs e)
        {
            int i = 0;
            foreach (DataGridItem itm in gridUndoMsg.Items)
            {
                if (itm.ItemType == ListItemType.Item || itm.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox chk = (CheckBox)itm.Cells[itm.Cells.Count - 1].FindControl("chkAttention");
                    string sMessageID = itm.Cells[8].Text;
                    sMessageID = sMessageID == "" ? "0" : sMessageID;
                    if (chk.Checked)
                    {
                        long nFlowID = FlowDP.GetFlowIDByMessageId(long.Parse(sMessageID));
                        AttentionDP.AddAttention(nFlowID, long.Parse(sMessageID), (long)Session["UserID"]);
                        i++;
                    }
                }
            }
            if (i > 0)
            {
                Session["PersonAttention"] = AttentionDP.GetMyAttention(long.Parse(Session["UserID"].ToString()));
                Epower.DevBase.BaseTools.PageTool.MsgBox(this.Page, "数据保存成功！");
            }
            BindData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbOverTime_Click(object sender, System.EventArgs e)
        {
            BindDataBookMark(eOA_ListBookMark.elbmOverTime);
        }

        protected void lkbInTime_Click(object sender, System.EventArgs e)
        {
            BindDataBookMark(eOA_ListBookMark.elbmInTime);
        }

        protected void lkbMaster_Click(object sender, System.EventArgs e)
        {
            BindDataBookMark(eOA_ListBookMark.elbmMaster);
        }

        protected void lkbAssist_Click(object sender, System.EventArgs e)
        {
            BindDataBookMark(eOA_ListBookMark.elbmAssist);
        }

        protected void lkbReader_Click(object sender, System.EventArgs e)
        {
            BindDataBookMark(eOA_ListBookMark.elbmReader);
        }

        protected void lkbInFlux_Click(object sender, System.EventArgs e)
        {
            BindDataBookMark(eOA_ListBookMark.elbmInflux);
        }

        private void gridUndoMsg_ItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    // (DataView)e.Item.NamingContainer;
                    if (i == 3 || i == 4 || i == 5 || i == 6 || i == 7)
                    {
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + i.ToString() + ",0);");
                    }
                }
            }
        }
    }
}
