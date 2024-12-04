/************************************************************************************
 * 版权所有：
 * Description:
 * 
 * 
 * 
 * Create By:zhumingchun
 * Create Date:2007-05-23
 * **********************************************************************************/
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
using System.Drawing;
using Epower.ITSM.SqlDAL;
using EpowerCom;
using EpowerGlobal;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;

namespace Epower.ITSM.Web.Forms
{
    public partial class frmWaittingContent : BasePage
    {
        #region 变量申明
        private long lngUserID = 0;
        private long lngLastMessageID = 0;
        protected string sCheckEnable = "true";  //是否自动检测
        protected long lngCheckTime = 300000;    //检测时间间隔
        #endregion

        #region 取值类别
        /// <summary>
        /// 
        /// </summary>
        public string TypeContent
        {
            get
            {
                if (Request["TypeContent"] != null)
                  return Request["TypeContent"].ToString();
                else if (ViewState["TypeContent"]!=null)
                    return ViewState["TypeContent"].ToString();
                else
                    return string.Empty;
            }
        }
        #endregion 

        #region 脚本调用方法区
        /// <summary>
        /// 显示图片
        /// </summary>
        /// <param name="isRead"></param>
        /// <returns></returns>
        protected string GetStatusImage(int isRead)
        {
            string ret = @"~\Images\page\flow_status_newnormal.gif";
            if (isRead > 0)
                ret = @"~\Images\page\flow_status_normal.gif";
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        protected bool GetVisible(int i)
        {
            bool t = false;
            if (i > 0)
                t = true;
            return t;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
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
        protected string GetUrl(decimal lngMessageID)
        {
            //暂时没处理分页
            string sUrl = "javascript:window.open('../Forms/flow_Normal.aspx?MessageID=" + lngMessageID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
            if (Session["DeskAssistantFlag"] != null)
            {
                sUrl = "javascript:window.open('../Forms/flow_Normal.aspx?MessageID=" + lngMessageID.ToString() + "','newwindow','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
            }
            return sUrl;
        }
        protected string GetUrl2(decimal lngMessageID)
        {
            string sUrl = "javascript:window.open('../CustManager/frmBr_SellChanceEdit.aspx?id=" + lngMessageID.ToString() + "&ReturnValue1=true','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
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
            return "ShowDetailsInfo(this," + lngFlowID.ToString() + "," + lngAppID.ToString() + ");";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngNoticeID"></param>
        /// <returns></returns>
        protected string GetMyRegUrl(decimal lngFlowID)
        {
            //暂时没处理分页
            string sUrl = "";
            sUrl = "javascript:window.open('../Forms/frmIssueView.aspx?FlowID=" + lngFlowID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";

            return sUrl;


        }
        #endregion 

        #region 方法区
        /// <summary>
        /// 页面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            lngUserID = long.Parse(Session["UserID"].ToString());

            sCheckEnable = CommonDP.GetConfigValue("AutoCheckNewItems", "AutoCheckNewItemsEnable");
            lngCheckTime = long.Parse(CommonDP.GetConfigValue("AutoCheckNewItems", "AutoCheckNewItems"));
            if (lngCheckTime < 60000)
            {
                //最小间隔1分钟
                lngCheckTime = 60000;
            }
            if (!Page.IsPostBack)
            {
                if (Request["TypeContent"] != null)
                {
                    ViewState["TypeContent"] = Request["TypeContent"].ToString();

                    if (Request["TypeContent"].ToString() == "MyReg")
                    {
                        Session["FromUrl"] = Constant.ApplicationPath + "/Forms/frmWaittingContent.aspx?TypeContent=MyReg";
                    }
                    else
                    {
                        Session["FromUrl"] = Session["MainUrl"].ToString();
                    }
                }
                else
                {
                    Session["FromUrl"] = Session["MainUrl"].ToString();
                }
                DoDataBind(ViewState["TypeContent"].ToString());
                hidLastMessageID.Value = lngLastMessageID.ToString();
            }
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="TypeContent"></param>
        private void DoDataBind(string TypeContent)
        {
            DataSet ds;
            DataTable dt;
            DataView dv;
            int iSize = 8;
            try
            {
                switch (TypeContent)
                {
                    case "WarnMsg":
                        //待办事项
                        dt = MessageCollectionDP.GetWarnUndoMessage(lngUserID, iSize);
                        gridWarnMsg.Visible = true;
                        gridWarnMsg.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.gridMsg_ItemDataBound);
                        //gridWarnMsg.ItemCreated += new DataGridItemEventHandler(gridMsg_ItemCreated);
                        dv = dt.DefaultView;
                        gridWarnMsg.DataSource = dv;
                        gridWarnMsg.DataBind();
                        if (dv.Count == 0)
                        {
                            trWarnMsg2.Visible = false;
                        }
                        else
                        {
                            trWarnMsg2.Visible = true;
                        }
                        break;
                    case "UndoMsg":
                        //待办事项
                        dt = MessageCollectionDP.GetUndoMessage(lngUserID, iSize);
                        gridUndoMsg.Visible = true;
                        gridUndoMsg.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.gridMsg_ItemDataBound);
                        //gridUndoMsg.ItemCreated += new DataGridItemEventHandler(gridMsg_ItemCreated);
                        dv = dt.DefaultView;
                        gridUndoMsg.DataSource = dv;
                        gridUndoMsg.DataBind();
                        if (dv.Count == 0)
                        {
                            trUndoMsg2.Visible = false;
                        }
                        else
                        {
                            trUndoMsg2.Visible = true;
                        }
                        break;
                    case "DealMsg":
                        //处理过的事项
                        dt = MessageCollectionDP.GetHasDoneMessage(lngUserID, iSize);
                        dgDealMsg2.Visible = true;
                        dgDealMsg2.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.gridMsg_ItemDataBound);
                        //dgDealMsg2.ItemCreated += new DataGridItemEventHandler(gridMsg_ItemCreated);
                        dv = dt.DefaultView;
                        dgDealMsg2.DataSource = dv;
                        dgDealMsg2.DataBind();
                        if (dv.Count == 0)
                        {
                            trDealMsg2.Visible = false;
                        }
                        else
                        {
                            trDealMsg2.Visible = true;
                        }
                        break;
                    case "ReceiveMsg":
                        // 待接收事项
                        //ds = ReceiveList.GetReceiveMessageList(lngUserID, iSize);
                       ds= TestDP.GetReceiveMessageList(lngUserID, iSize);
                        gridReceiveMsg.Visible = true;
                        gridReceiveMsg.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.gridMsg_ItemDataBound);
                        //gridReceiveMsg.ItemCreated += new DataGridItemEventHandler(gridMsg_ItemCreated);
                        dv = ds.Tables[0].DefaultView;
                        gridReceiveMsg.DataSource = dv;
                        gridReceiveMsg.DataBind();
                        if (dv.Count == 0)
                        {
                            trReceiveMsg2.Visible = false;
                        }
                        else
                        {
                            trReceiveMsg2.Visible = true;
                        }
                        break;
                    case "ReadMsg":
                        //阅知事项
                        dt = MessageCollectionDP.GetUnReadMessage(lngUserID, iSize);
                        gridReadMsg.Visible = true;
                        gridReadMsg.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.gridMsg_ItemDataBound);
                        //gridReadMsg.ItemCreated += new DataGridItemEventHandler(gridMsg_ItemCreated);
                        dv = dt.DefaultView;
                        gridReadMsg.DataSource = dv;
                        gridReadMsg.DataBind();

                        if (dv.Count == 0)
                        {
                            trReadMsg2.Visible = false;
                        }
                        else
                        {
                            trReadMsg2.Visible = true;
                        }
                        break;
                    case "WaitingMsg":
                        //挂起的事项
                        dt = MessageCollectionDP.GetWaitingMessage(lngUserID, iSize);
                        gridWaitingMsg.Visible = true;
                        gridWaitingMsg.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.gridMsg_ItemDataBound);
                        //gridWaitingMsg.ItemCreated += new DataGridItemEventHandler(gridMsg_ItemCreated);
                        dv = dt.DefaultView;
                        gridWaitingMsg.DataSource = dv;
                        gridWaitingMsg.DataBind();
                        if (dv.Count == 0)
                        {
                            trWaiting2.Visible = false;
                        }
                        else
                        {
                            trWaiting2.Visible = true;
                        }
                        break;
                    case "Attention":
                        //关注事项
                        dt = AttentionDP.GetMyAttention((long)Session["UserID"], iSize);
                        gridAttention.Visible = true;
                        gridAttention.ItemDataBound += new DataGridItemEventHandler(this.gridAttention_ItemDataBound);
                        //gridAttention.ItemCommand += new DataGridCommandEventHandler(this.gridAttention_ItemCommand);
                        // 排序有BUG暂时屏蔽
                        //gridAttention.ItemCreated += new DataGridItemEventHandler(this.gridAttention_ItemCreated);
                        dv = dt.DefaultView;
                        gridAttention.DataSource = dv;
                        gridAttention.DataBind();
                        if (dv.Count == 0)
                        {
                            trAttention2.Visible = false;
                        }
                        else
                        {
                            trAttention2.Visible = true;
                        }
                        break;
                    case "MyReg":
                        //我登记事项
                        dt = MessageCollectionDP.GetMyRegMessageUnFinished((long)Session["UserID"],20);
                        // 排序有BUG暂时屏蔽
                        //gridMyReg.ItemCreated += new DataGridItemEventHandler(dgMyReg_ItemCreated);
                        gridMyReg.ItemDataBound += new DataGridItemEventHandler(this.gridMyReg_ItemDataBound);
                        dv = dt.DefaultView;
                        gridMyReg.DataSource = dv;
                        gridMyReg.DataBind();

                        trTitle.Visible = true;
                        if (dv.Count == 0)
                        {
                            trMyReg2.Visible = false;
                            trNoneData.Visible = true;
                        }
                        else
                        {
                            trMyReg2.Visible = true;
                            gridMyReg.Visible = true;
                            trNoneData.Visible = false;
                        }
                        break;                  
                    default:
                        break;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridMsg_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //记录最大的消息ID供自动检测
                long lngID = long.Parse(e.Item.Cells[7].Text.Trim());
                if (lngID > lngLastMessageID)
                    lngLastMessageID = lngID;

                switch ((e_ActorClass)(int.Parse(e.Item.Cells[11].Text.Trim())))
                {
                    case e_ActorClass.fmMasterActor:
                        e.Item.Cells[11].Text = "主办";
                        break;
                    case e_ActorClass.fmAssistActor:
                        e.Item.Cells[11].Text = "协办";
                        break;
                    case e_ActorClass.fmInfluxActor:
                        e.Item.Cells[11].Text = "会签";
                        break;
                    case e_ActorClass.fmReaderActor:
                        e.Item.Cells[11].Text = "阅知";
                        break;
                    default:
                        break;
                }

                //当超过预计处理时间未处理的，红低显示
                if (int.Parse(e.Item.Cells[8].Text.Trim()) < 0)
                {
                    for (int i = 0; i < e.Item.Cells.Count; i++)
                    {
                        e.Item.Cells[i].CssClass = "listItemOverTime";
                    }
                }

                if (long.Parse(e.Item.Cells[9].Text.Trim()) == (long)e_AppTypes.eatCommon)
                {
                    //通用流程特殊处理
                    if (e.Item.Cells[10].Text.Trim().StartsWith(e.Item.Cells[6].Text.Trim()))
                    {
                        e.Item.Cells[6].Text = e.Item.Cells[10].Text;
                    }
                    else
                    {
                        e.Item.Cells[6].Text = e.Item.Cells[6].Text.Trim() + "[" + e.Item.Cells[10].Text + "]";
                    }
                }
                string lngMessageID = DataBinder.Eval(e.Item.DataItem, "MessageID").ToString();
                e.Item.Attributes.Add("ondblclick", "window.open('../Forms/flow_Normal.aspx?MessageID=" + lngMessageID + "&randomid='+GetRandom(),'MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
                if (Session["DeskAssistantFlag"] != null)
                {
                    e.Item.Attributes.Add("ondblclick", "window.open('../Forms/flow_Normal.aspx?MessageID=" + lngMessageID + "&randomid='+GetRandom(),'newwindow','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
                }
                //e.Item.Attributes.Add("ItemStyle", "this.style.ForeColor='#A3C9E1'");
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");

                ((Label)e.Item.FindControl("lblSubject")).Attributes.Add("onmouseover", "ShowDetailsInfo(this," + DataBinder.Eval(e.Item.DataItem, "flowid").ToString() + "," + DataBinder.Eval(e.Item.DataItem, "appid").ToString() + ",400);");
            }
        }
        protected void gridMyReg_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string Flowid = DataBinder.Eval(e.Item.DataItem, "Flowid").ToString();
                e.Item.Attributes.Add("ondblclick", "window.open('../Forms/frmIssueView.aspx?FlowID=" + Flowid.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridMsg_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    // (DataView)e.Item.NamingContainer;
                    //if (i == 3 || i == 4 || i == 5 || i == 6)
                    if (i > 2 && i < e.Item.Cells.Count - 2)
                    {
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + i.ToString() + ",0);");
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridAttention_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    // (DataView)e.Item.NamingContainer;
                    if (i == 5 || i == 6 || i == 7)
                    {
                        int j = i - 5;  //前面有5个隐藏的列
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgMyReg_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    // (DataView)e.Item.NamingContainer;
                    if (i == 1 || i == 2)
                    {
                        int j = i -1;  //前面有1个隐藏的列
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void gridAttention_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName.ToLower() == "del")
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    string sID = e.Item.Cells[0].Text.Trim("&nbsp;".ToCharArray());
                    if (sID != "")
                    {
                        AttentionDP.DeleteAttention(long.Parse(sID));
                        DoDataBind(ViewState["TypeContent"].ToString());
                        Epower.DevBase.BaseTools.PageTool.MsgBox(this.Page, "关注事项取消成功！");
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridAttention_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string lngMessageID = DataBinder.Eval(e.Item.DataItem, "MessageID").ToString();
                e.Item.Attributes.Add("ondblclick", "window.open('../Forms/flow_Normal.aspx?MessageID=" + lngMessageID + "&randomid='+GetRandom(),'MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
                if (Session["DeskAssistantFlag"] != null)
                {
                    e.Item.Attributes.Add("ondblclick", "window.open('../Forms/flow_Normal.aspx?MessageID=" + lngMessageID + "&randomid='+GetRandom(),'newwindow','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
                }
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");

                ((Label)e.Item.FindControl("lblSubject")).Attributes.Add("onmouseover", "ShowDetailsInfo(this," + DataBinder.Eval(e.Item.DataItem, "flowid").ToString() + "," + DataBinder.Eval(e.Item.DataItem, "appid").ToString() + ",400);");
            }
        }
        #endregion 
   
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid_handOver_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                string lngMessageID = DataBinder.Eval(e.Item.DataItem, "ID").ToString();
                e.Item.Attributes.Add("ondblclick", "javascript:window.open('../CustManager/frmBr_SellChanceEdit.aspx?id=" + lngMessageID + "&ReturnValue1=true','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
            }
        }
    }
}
