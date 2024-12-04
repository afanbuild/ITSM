/*******************************************************************
 * 版权所有：
 * Description：批量接收
 * 
 * 
 * Create By  ：zhumc
 * Create Date：2010-12-01
 * *****************************************************************/
using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;
using EpowerCom;
using EpowerGlobal;
using System.Drawing;

namespace Epower.ITSM.Web.AppForms
{
    public partial class Cst_IssueBatchReceiver : BasePage
    {
        #region Page_Load
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
                Session["FromUrl"] = "../AppForms/Cst_IssueBatchReceiver.aspx";
            }
        }
        #endregion 

        #region 脚本调用方法区
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

        #region BindData
        private void BindData()
        {
            long lngUserID = long.Parse(Session["UserID"].ToString());
            DataTable dt = FlowReceiveList.GetReceiveMessageList(lngUserID);

            gridReceiveMsg.DataSource = dt;
            gridReceiveMsg.DataBind();
        }
        #endregion 

        #region DataGrid排序 gridUndoMsg_ItemCreated
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
                for (int i = 1; i < e.Item.Cells.Count - 10; i++)
                {
                    int j = i - 0;
                    e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                }
            }
        }
        #endregion

        #region btnConfirm_Click 批量接收 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReceiver_Click(object sender, EventArgs e)
        {
            FormDefineTool fdt = new FormDefineTool();
            fdt.Add(0, " ", e_ActorClass.fmMasterActor, e_DeptORUser.fmUser);
            string strDefineXmlValue = fdt.GetXmlObject().InnerXml;

            int iCount = 0;
            for (int i = 0; i < gridReceiveMsg.Items.Count; i++)
            {
                CheckBox chkSelect = (CheckBox)gridReceiveMsg.Items[i].Cells[0].FindControl("chkSelect");
                if (chkSelect.Checked)
                {
                    long lngFlowID = long.Parse(gridReceiveMsg.Items[i].Cells[13].Text.Trim());
                    long lngMessageID = long.Parse(gridReceiveMsg.Items[i].Cells[14].Text.Trim());
                    long lngNodeModelID = long.Parse(gridReceiveMsg.Items[i].Cells[15].Text.Trim());
                    long lngFlowModelID = long.Parse(gridReceiveMsg.Items[i].Cells[16].Text.Trim());
                    long lngAppID = long.Parse(gridReceiveMsg.Items[i].Cells[17].Text.Trim());
                    long lngUserID = long.Parse(Session["UserID"].ToString());

                    bool blnSuccess = false;
                    blnSuccess = ReceiveList.ReceiveMessage(lngMessageID, lngUserID, lngAppID);

                    iCount++;
                }
            }
            if (iCount != 0)
            {
                PageTool.MsgBox(this, "批量接收成功，接收了" + iCount.ToString() + "张单！");
                BindData();
            }
            else
            {
                Epower.DevBase.BaseTools.PageTool.MsgBox(this, "未选择批量接收记录！");
            }
        }
        #endregion 

        #region gridUndoMsg_ItemDataBound
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridUndoMsg_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            e_FlowStatus fs;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                //当超过整个流程预计处理时限未处理的，红低显示
                fs = (e_FlowStatus)int.Parse(e.Item.Cells[gridReceiveMsg.Columns.Count - 5].Text);

                if (int.Parse(e.Item.Cells[gridReceiveMsg.Columns.Count - 4].Text.Trim()) < 0 && fs != e_FlowStatus.efsEnd)
                {
                    for (int i = 0; i < e.Item.Cells.Count; i++)
                    {
                        e.Item.Cells[i].ForeColor = Color.Red;
                    }
                }
                ((Label)e.Item.FindControl("Lb_ServiceNo")).Attributes.Add("onmouseover", "ShowDetailsInfo(this," + DataBinder.Eval(e.Item.DataItem, "SMSID").ToString() + ",400);");
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                long lngMessageID = long.Parse(e.Item.Cells[14].Text.Trim());
                e.Item.Attributes.Add("ondblclick", "window.open('../Forms/flow_Normal.aspx?MessageID=" + lngMessageID.ToString() + "&randomid='+GetRandom(),'MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
            }
        }
        #endregion 
    }
}
