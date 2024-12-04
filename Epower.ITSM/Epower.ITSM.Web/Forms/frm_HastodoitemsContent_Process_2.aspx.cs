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
    public partial class frm_HastodoitemsContent_Process_2 : BasePage
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

        #region FromBackUrl

        public string FromBackUrl
        {
            get
            {
                if (ViewState["FromBackUrl"] != null)
                    return ViewState["FromBackUrl"].ToString();
                else
                    return "";
            }
            set
            {
                ViewState["FromBackUrl"] = value;
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

        #region 接收的ＵＲＬ
        /// <summary>
        /// 接收的ＵＲＬ
        /// </summary>
        /// <param name="lngNoticeID"></param>
        /// <returns></returns>
        protected string GetHrefUrl(decimal lngMessageID)
        {
            //暂时没处理分页

            string sUrl = "flow_Normal.aspx?MessageID=" + lngMessageID.ToString();
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
            //Session["FromUrl"] = Constant.ApplicationPath + "/Forms/frm_Hastodoitems.aspx";
            //FromBackUrl = Session["FromUrl"].ToString();

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
                Session["FromUrl"] = Constant.ApplicationPath + "/Forms/frm_Hastodoitems.aspx";
                DoDataBind(ViewState["TypeContent"].ToString());
                hidLastMessageID.Value = lngLastMessageID.ToString();
            }
        }
        #endregion

        /// <summary>
        /// 事件
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataTable Data_Issue(DataTable dt)
        {
            //声明一个新表
            DataTable newdt = new DataTable();
            newdt.Columns.Add(new DataColumn("NumberNo"));//
            newdt.Columns.Add(new DataColumn("ServiceKind"));//
            newdt.Columns.Add(new DataColumn("ServiceType"));//
            newdt.Columns.Add(new DataColumn("regusername"));//
            newdt.Columns.Add(new DataColumn("subject"));//
            newdt.Columns.Add(new DataColumn("FlowID"));//
            newdt.Columns.Add(new DataColumn("FActors"));//
            newdt.Columns.Add(new DataColumn("ServiceLevel"));//
            newdt.Columns.Add(new DataColumn("MessageID"));//
            newdt.Columns.Add(new DataColumn("ReceiveTime"));//
            newdt.Columns.Add(new DataColumn("senderusername"));//
            newdt.Columns.Add(new DataColumn("sendernodename"));//
            newdt.Columns.Add(new DataColumn("IsRead"));//
            newdt.Columns.Add(new DataColumn("Important"));//
            newdt.Columns.Add(new DataColumn("Attachment"));//
            newdt.Columns.Add(new DataColumn("appid"));//
            newdt.Columns.Add(new DataColumn("AppName"));//
            newdt.Columns.Add(new DataColumn("DiffMinute"));//
            newdt.Columns.Add(new DataColumn("FlowName"));//
            newdt.Columns.Add(new DataColumn("actortype"));//
            newdt.Columns.Add(new DataColumn("FinishedTime"));//
            newdt.Columns.Add(new DataColumn("Content"));//

            //遍历表
            foreach (DataRow dr in dt.Rows)
            {
                //先判断是否存在新表
                int n = 0;
                if (newdt != null && newdt.Rows.Count > 0)
                {
                    foreach (DataRow dr2 in newdt.Rows)
                    {
                        string numdo = dr["NumberNo"].ToString();
                        string numdo2 = dr2["NumberNo"].ToString();
                        if (numdo == numdo2)
                            n++;
                    }
                }

                //新增记录到新表
                if (n == 0 && newdt.Rows.Count <= 50)
                {
                    DataRow newdr = newdt.NewRow();
                    newdr["NumberNo"] = dr["NumberNo"].ToString();
                    newdr["ServiceKind"] = dr["ServiceKind"].ToString();
                    newdr["ServiceType"] = dr["ServiceType"].ToString();
                    newdr["regusername"] = dr["regusername"].ToString();
                    newdr["subject"] = dr["subject"].ToString();
                    newdr["FlowID"] = Convert.ToDecimal(dr["FlowID"].ToString());
                    newdr["FActors"] = dr["FActors"].ToString();
                    newdr["ServiceLevel"] = dr["ServiceLevel"].ToString();
                    newdr["MessageID"] = Convert.ToDecimal(dr["MessageID"].ToString());
                    newdr["ReceiveTime"] = dr["ReceiveTime"].ToString();
                    newdr["senderusername"] = dr["senderusername"].ToString();
                    newdr["sendernodename"] = dr["sendernodename"].ToString();
                    newdr["IsRead"] = Convert.ToInt32(dr["IsRead"].ToString());
                    newdr["Important"] = dr["Important"].ToString();
                    newdr["Attachment"] = dr["Attachment"].ToString();
                    newdr["appid"] = Convert.ToDecimal(dr["appid"].ToString());
                    newdr["AppName"] = dr["AppName"].ToString();
                    newdr["DiffMinute"] = dr["DiffMinute"].ToString();
                    newdr["FlowName"] = dr["FlowName"].ToString();
                    newdr["actortype"] = dr["actortype"].ToString();
                    newdr["FinishedTime"] = dr["FinishedTime"].ToString();
                    newdr["Content"] = dr["Content"].ToString();
                    newdt.Rows.Add(newdr);
                }
            }

            return newdt;
        }


        /// <summary>
        /// 变更
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataTable Data_Change(DataTable dt)
        {
            DataTable newdt = new DataTable();
            newdt.Columns.Add(new DataColumn("NumberNo"));//
            newdt.Columns.Add(new DataColumn("subject"));//
            newdt.Columns.Add(new DataColumn("FlowID"));//
            newdt.Columns.Add(new DataColumn("FActors"));//
            newdt.Columns.Add(new DataColumn("LevelName"));//
            newdt.Columns.Add(new DataColumn("MessageID"));//
            newdt.Columns.Add(new DataColumn("ReceiveTime"));//
            newdt.Columns.Add(new DataColumn("senderusername"));//
            newdt.Columns.Add(new DataColumn("sendernodename"));//
            newdt.Columns.Add(new DataColumn("IsRead"));//
            newdt.Columns.Add(new DataColumn("Important"));//
            newdt.Columns.Add(new DataColumn("Attachment"));//
            newdt.Columns.Add(new DataColumn("appid"));//
            newdt.Columns.Add(new DataColumn("AppName"));//
            newdt.Columns.Add(new DataColumn("DiffMinute"));//
            newdt.Columns.Add(new DataColumn("FlowName"));//
            newdt.Columns.Add(new DataColumn("actortype"));//


            //遍历表
            foreach (DataRow dr in dt.Rows)
            {
                //先判断是否存在新表
                int n = 0;
                if (newdt != null && newdt.Rows.Count > 0)
                {
                    foreach (DataRow dr2 in newdt.Rows)
                    {
                        string numdo = dr["NumberNo"].ToString();
                        string numdo2 = dr2["NumberNo"].ToString();
                        if (numdo == numdo2)
                            n++;
                    }
                }

                //新增记录到新表
                if (n == 0 && newdt.Rows.Count <=50)
                {
                    DataRow newdr = newdt.NewRow();
                    newdr["NumberNo"] = dr["NumberNo"].ToString();
                    newdr["subject"] = dr["subject"].ToString();
                    newdr["FlowID"] = Convert.ToDecimal(dr["FlowID"].ToString());
                    newdr["FActors"] = dr["FActors"].ToString();
                    newdr["LevelName"] = dr["LevelName"].ToString();
                    newdr["MessageID"] = Convert.ToDecimal(dr["MessageID"].ToString());
                    newdr["ReceiveTime"] = dr["ReceiveTime"].ToString();
                    newdr["senderusername"] = dr["senderusername"].ToString();
                    newdr["sendernodename"] = dr["sendernodename"].ToString();
                    newdr["IsRead"] = Convert.ToInt32(dr["IsRead"].ToString());
                    newdr["Important"] = dr["Important"].ToString();
                    newdr["Attachment"] = dr["Attachment"].ToString();
                    newdr["appid"] = Convert.ToDecimal(dr["appid"].ToString());
                    newdr["AppName"] = dr["AppName"].ToString();
                    newdr["DiffMinute"] = dr["DiffMinute"].ToString();
                    newdr["FlowName"] = dr["FlowName"].ToString();
                    newdr["actortype"] = dr["actortype"].ToString();
                    newdt.Rows.Add(newdr);
                }
            }
            return newdt;
        }

        /// <summary>
        /// 问题
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataTable Data_Byts(DataTable dt)
        {
            DataTable newdt = new DataTable();
            newdt.Columns.Add(new DataColumn("NumberNo"));//
            newdt.Columns.Add(new DataColumn("IsRead"));//
            newdt.Columns.Add(new DataColumn("subject"));//
            newdt.Columns.Add(new DataColumn("MessageID"));//
            newdt.Columns.Add(new DataColumn("flowid"));//
            newdt.Columns.Add(new DataColumn("appid"));//
            newdt.Columns.Add(new DataColumn("DiffMinute"));//
            newdt.Columns.Add(new DataColumn("ReceiveTime"));//
            newdt.Columns.Add(new DataColumn("Problem_LevelName"));//
            //遍历表
            foreach (DataRow dr in dt.Rows)
            {
                //先判断是否存在新表
                int n = 0;
                if (newdt != null && newdt.Rows.Count > 0)
                {
                    foreach (DataRow dr2 in newdt.Rows)
                    {
                        string numdo = dr["NumberNo"].ToString();
                        string numdo2 = dr2["NumberNo"].ToString();
                        if (numdo == numdo2)
                            n++;
                    }
                }

                //新增记录到新表
                if (n == 0 && newdt.Rows.Count <= 50)
                {
                    DataRow newdr = newdt.NewRow();
                    newdr["NumberNo"] = dr["NumberNo"].ToString();
                    newdr["subject"] = dr["subject"].ToString();
                    newdr["FlowID"] = Convert.ToDecimal(dr["FlowID"].ToString());
                    newdr["MessageID"] = Convert.ToDecimal(dr["MessageID"].ToString());
                    newdr["IsRead"] = Convert.ToInt32(dr["IsRead"].ToString());
                    newdr["appid"] = Convert.ToDecimal(dr["appid"].ToString());
                    newdr["DiffMinute"] = dr["DiffMinute"].ToString();
                    newdr["ReceiveTime"] = dr["ReceiveTime"].ToString();
                    newdr["Problem_LevelName"] = dr["Problem_LevelName"].ToString();
                    newdt.Rows.Add(newdr);
                }
            }

            return newdt;
        }

        /// <summary>
        /// 知识
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataTable Data_Kb(DataTable dt)
        {
            DataTable newdt = new DataTable();
            newdt.Columns.Add(new DataColumn("Title"));//
            newdt.Columns.Add(new DataColumn("PKey"));//
            newdt.Columns.Add(new DataColumn("TypeName"));//
            newdt.Columns.Add(new DataColumn("RegTime"));//
            newdt.Columns.Add(new DataColumn("IsRead"));//
            newdt.Columns.Add(new DataColumn("subject"));//
            newdt.Columns.Add(new DataColumn("flowid"));//
            newdt.Columns.Add(new DataColumn("MessageID"));//
            newdt.Columns.Add(new DataColumn("appid"));//
            newdt.Columns.Add(new DataColumn("DiffMinute"));//


            //遍历表
            foreach (DataRow dr in dt.Rows)
            {
                //先判断是否存在新表
                int n = 0;
                if (newdt != null && newdt.Rows.Count > 0)
                {
                    foreach (DataRow dr2 in newdt.Rows)
                    {
                        string numdo = dr["FlowID"].ToString();
                        string numdo2 = dr2["FlowID"].ToString();
                        if (numdo == numdo2)
                            n++;
                    }
                }

                //新增记录到新表
                if (n == 0 && newdt.Rows.Count <= 50)
                {
                    DataRow newdr = newdt.NewRow();
                    newdr["Title"] = dr["Title"].ToString();
                    newdr["PKey"] = dr["PKey"].ToString();
                    newdr["TypeName"] = dr["TypeName"].ToString();
                    newdr["RegTime"] = dr["RegTime"].ToString();
                    newdr["IsRead"] = Convert.ToInt32(dr["IsRead"].ToString());
                    newdr["subject"] = dr["subject"].ToString();
                    newdr["flowid"] = Convert.ToDecimal(dr["FlowID"].ToString());
                    newdr["MessageID"] = Convert.ToDecimal(dr["MessageID"].ToString());
                    newdr["appid"] = Convert.ToDecimal(dr["appid"].ToString());
                    newdr["DiffMinute"] = dr["DiffMinute"].ToString();
                    newdt.Rows.Add(newdr);
                }
            }

            return newdt;
        }

        /// <summary>
        /// 其他
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataTable Data_Other(DataTable dt)
        {
            DataTable newdt = new DataTable();
           
            newdt.Columns.Add(new DataColumn("IsRead"));//
            newdt.Columns.Add(new DataColumn("subject"));//
            newdt.Columns.Add(new DataColumn("MessageID"));//
            newdt.Columns.Add(new DataColumn("flowid"));//
            newdt.Columns.Add(new DataColumn("appid"));//
            newdt.Columns.Add(new DataColumn("name"));//
            newdt.Columns.Add(new DataColumn("ReceiveTime"));//
            newdt.Columns.Add(new DataColumn("DiffMinute"));

            //遍历表
            foreach (DataRow dr in dt.Rows)
            {
                //先判断是否存在新表
                int n = 0;
                if (newdt != null && newdt.Rows.Count > 0)
                {
                    foreach (DataRow dr2 in newdt.Rows)
                    {
                        string numdo = dr["FlowID"].ToString();
                        string numdo2 = dr2["FlowID"].ToString();
                        if (numdo == numdo2)
                            n++;
                    }
                }

                //新增记录到新表
                if (n == 0 && newdt.Rows.Count <= 50)
                {
                    DataRow newdr = newdt.NewRow();
                    newdr["subject"] = dr["subject"].ToString();
                    newdr["FlowID"] = Convert.ToDecimal(dr["FlowID"].ToString());
                    newdr["MessageID"] = Convert.ToDecimal(dr["MessageID"].ToString());
                    newdr["IsRead"] = Convert.ToInt32(dr["IsRead"].ToString());
                    newdr["appid"] = Convert.ToDecimal(dr["appid"].ToString());
                    newdr["name"] = dr["name"].ToString();
                    newdr["ReceiveTime"] = dr["ReceiveTime"].ToString();
                    newdr["DiffMinute"] = dr["DiffMinute"].ToString();
                    newdt.Rows.Add(newdr);
                }
            }
            return newdt;
        }


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
                        dt = ZHServiceDP.getHastodoitem_FlowWorkNow((long)Session["UserID"], 1026, 1);
                        dt = Data_Issue(dt);
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
                        dt = ZHServiceDP.getHastodoitem_FlowWorkNow((long)Session["UserID"], 420, 1);
                        dt = Data_Change(dt);
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
                        dt = ZHServiceDP.getHastodoitem_FlowWorkNow((long)Session["UserID"], 210, 1);
                        dt = Data_Byts(dt);
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
                        dt = ZHServiceDP.getHastodoitem_FlowWorkNow((long)Session["UserID"], 1028, 1);
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
                    case "Kb"://知识                      
                        dt = ZHServiceDP.getHastodoitem_FlowWorkNow((long)Session["UserID"], 400, 1);
                        dt = Data_Kb(dt);
                        DataGrid_Kb.Visible = true;
                        DataGrid_Kb.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.gridMsg_ItemDataBound);
                        DataGrid_Kb.ItemCreated += new DataGridItemEventHandler(gridByts_ItemCreated);
                        dv = dt.DefaultView;
                        DataGrid_Kb.DataSource = dv;
                        DataGrid_Kb.DataBind();

                        if (dv.Count == 0)
                        {
                            DataGrid_Kb.Visible = false;
                        }
                        else
                        {
                            DataGrid_Kb.Visible = true;
                        }
                        break;
                    case "Other"://其它                        
                        dt = ZHServiceDP.getHastodoitem_FlowWorkNow((long)Session["UserID"], 0, 1);
                        dt = Data_Other(dt);
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
                }

                string sFlowID = DataBinder.Eval(e.Item.DataItem, "FlowID").ToString();

                #region 新增2个显示字段 时间差

                if (TypeContent == "Issue")
                {
                    DataTable dt = ZHServiceDP.GetFlowBusLimitByFlowID(decimal.Parse(sFlowID == "" ? "0" : sFlowID));
                    //DateTime outtime = DataBinder.Eval(e.Item.DataItem, "Outtime").ToString() == "" ? DateTime.Now : DateTime.Parse(DataBinder.Eval(e.Item.DataItem, "Outtime").ToString());
                    DateTime FinishedTime = DataBinder.Eval(e.Item.DataItem, "FinishedTime").ToString() == "" ? DateTime.Now : DateTime.Parse(DataBinder.Eval(e.Item.DataItem, "FinishedTime").ToString());

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DateTime LimitTime = dt.Rows[i]["LimitTime"].ToString() == "" ? DateTime.Now : DateTime.Parse(dt.Rows[i]["LimitTime"].ToString());
                            TimeSpan ts1 = LimitTime.Subtract(FinishedTime);
                            //TimeSpan ts2 = LimitTime.Subtract(outtime);
                            if (dt.Rows[i]["GuidID"].ToString() == "10001")
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
                                    e.Item.Cells[5].Text = "超" + str1;
                                    e.Item.Cells[5].ForeColor = Color.Red;
                                }
                                else
                                {
                                    e.Item.Cells[5].Text = "还剩" + str1;
                                }
                            }
                        }
                    }
                    else
                    {
                        e.Item.Cells[5].Text = "";
                    }
                    e.Item.Cells[5].Font.Bold = true;
                }
                #endregion
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
                    if (i > 0 && i <= 4)
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
