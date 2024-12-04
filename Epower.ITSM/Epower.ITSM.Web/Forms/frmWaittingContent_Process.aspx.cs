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
    public partial class frmWaittingContent_Process : BasePage
    {
        #region 变量申明
        private long lngUserID = 0;              //用户ID
        private long lngLastMessageID = 0;
        protected string sCheckEnable = "true";  //是否自动检测
        protected long lngCheckTime = 300000;    //检测时间间隔
        #endregion

        #region 取值类别
        /// <summary>
        /// 取值类别
        /// </summary>
        public string TypeContent
        {
            get
            {
                if (Request["TypeContent"] != null)
                    return Request["TypeContent"].ToString();
                else if (ViewState["TypeContent"] != null)
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

        #region 接收的ＵＲＬ
        /// <summary>
        /// 接收的ＵＲＬ
        /// </summary>
        /// <param name="lngNoticeID"></param>
        /// <returns></returns>
        protected string GetUrl(decimal lngMessageID)
        {
            //暂时没处理分页

            string sUrl = "javascript:window.open('flow_Normal.aspx?MessageID=" + lngMessageID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
            if (Session["DeskAssistantFlag"] != null)
            {
                sUrl = "javascript:window.open('flow_Normal.aspx?MessageID=" + lngMessageID.ToString() + "','newwindow','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
            }
            return sUrl;
        }
        #endregion

        #region 获取快照的脚本
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
        #endregion

        #endregion

        #region 方法区

        #region 页面初始化
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
        #endregion

        #region 绑定数据
        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="TypeContent"></param>
        private void DoDataBind(string TypeContent)
        {
            DataTable dt;
            DataView dv;
            try
            {
                switch (TypeContent)
                {
                    case "Issue"://事件
                        gridIssue.Visible = true;
                        dt = ZHServiceDP.getFlowWorkNow((long)Session["UserID"], 1026, 1);
                        gridIssue.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.gridMsg_ItemDataBound);
                        gridIssue.ItemCreated += new DataGridItemEventHandler(gridMsg_ItemCreated);
                        dv = dt.DefaultView;
                        gridIssue.DataSource = dv;
                        gridIssue.DataBind();
                        if (dv.Count == 0)
                        {
                            trIssue.Visible = false;
                        }
                        else
                        {
                            trIssue.Visible = true;
                        }
                        break;                    
                    case "Change"://变更                        
                        dt = ZHServiceDP.getFlowWorkNow((long)Session["UserID"], 420, 1);
                        gridChange.Visible = true;
                        gridChange.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.gridMsg_ItemDataBound);
                        gridChange.ItemCreated += new DataGridItemEventHandler(gridChange_ItemCreated);
                        dv = dt.DefaultView;
                        gridChange.DataSource = dv;
                        gridChange.DataBind();
                        if (dv.Count == 0)
                        {
                            trChange.Visible = false;
                        }
                        else
                        {
                            trChange.Visible = true;
                        }
                        break;
                    case "Byts": //问题                       
                        dt = ZHServiceDP.getFlowWorkNow((long)Session["UserID"], 210, 1);
                        gridByts.Visible = true;
                        gridByts.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.gridMsg_ItemDataBound);
                        gridByts.ItemCreated += new DataGridItemEventHandler(gridByts_ItemCreated);
                        dv = dt.DefaultView;
                        gridByts.DataSource = dv;
                        gridByts.DataBind();
                        if (dv.Count == 0)
                        {
                            trByts.Visible = false;
                        }
                        else
                        {
                            trByts.Visible = true;
                        }
                        break;
                    case "Release":// 发布                        
                        dt = ZHServiceDP.getFlowWorkNow((long)Session["UserID"], 1028, 1);
                        gridRelease.Visible = true;
                        gridRelease.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.gridMsg_ItemDataBound);
                        gridRelease.ItemCreated += new DataGridItemEventHandler(gridByts_ItemCreated);
                        dv = dt.DefaultView;
                        gridRelease.DataSource = dv;
                        gridRelease.DataBind();
                        if (dv.Count == 0)
                        {
                            trRelease.Visible = false;
                        }
                        else
                        {
                            trRelease.Visible = true;
                        }
                        break;
                    case "Other"://其它                        
                        dt = ZHServiceDP.getFlowWorkNow((long)Session["UserID"], 0, 1);
                        gridOther.Visible = true;
                        gridOther.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.gridMsg_ItemDataBound);
                        gridOther.ItemCreated += new DataGridItemEventHandler(gridByts_ItemCreated);
                        dv = dt.DefaultView;
                        gridOther.DataSource = dv;
                        gridOther.DataBind();

                        if (dv.Count == 0)
                        {
                            trOther.Visible = false;
                        }
                        else
                        {
                            trOther.Visible = true;
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
        #endregion

        #region gridMsg_ItemDataBound
        /// <summary>
        /// gridMsg_ItemDataBound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridMsg_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //记录最大的消息ID供自动检测

                long lngID = long.Parse(e.Item.Cells[e.Item.Cells.Count - 3].Text.Trim());
                if (lngID > lngLastMessageID)
                    lngLastMessageID = lngID;

                //当超过预计处理时间未处理的，红低显示
                if (int.Parse(e.Item.Cells[e.Item.Cells.Count - 2].Text.Trim()) < 0)
                {
                    for (int i = 0; i < e.Item.Cells.Count; i++)
                    {
                        e.Item.Cells[i].BackColor = Color.DarkOrange;
                    }
                }
                string typecontent = ViewState["TypeContent"].ToString();
                ((Label)e.Item.FindControl("lblSubject")).Attributes.Add("onmouseover", "ShowDetailsInfo(this," + DataBinder.Eval(e.Item.DataItem, "flowid").ToString() + "," + DataBinder.Eval(e.Item.DataItem, "appid").ToString() + ",400);");

                string lngMessageID = DataBinder.Eval(e.Item.DataItem, "MessageID").ToString();
                e.Item.Attributes.Add("onclick", "window.open('../Forms/flow_Normal.aspx?MessageID=" + lngMessageID + "&randomid='+GetRandom(),'MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
                e.Item.CssClass = "hand";
                if (Session["DeskAssistantFlag"] != null)
                {
                    e.Item.Attributes.Add("onclick", "window.open('../Forms/flow_Normal.aspx?MessageID=" + lngMessageID + "&randomid='+GetRandom(),'newwindow','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
                    e.Item.CssClass = "hand";
                }
            }
        }
        #endregion

        #region gridMsg_ItemCreated
        /// <summary>
        /// gridMsg_ItemCreated
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
                    if (i > 0 && i <= 5)
                    {
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + i.ToString() + ",0);");
                    }
                }
            }
        }
        #endregion        

        #region gridChange_ItemCreated
        /// <summary>
        /// gridChange_ItemCreated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridChange_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i >0 && i <= 4)
                    {
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + i.ToString() + ",0);");
                    }
                }
            }
        }
        #endregion

        #region gridByts_ItemCreated
        /// <summary>
        /// gridByts_ItemCreated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridByts_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i > 0 && i <= 4)
                    {
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + i.ToString() + ",0);");
                    }
                }
            }
        }
        #endregion

        #endregion
    }
}
