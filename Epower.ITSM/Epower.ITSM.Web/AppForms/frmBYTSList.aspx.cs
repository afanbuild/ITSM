/****************************************************************************
 * 
 * description:客户投诉记录
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-10-19
 * *************************************************************************/
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
using System.Xml;
using System.Drawing;

using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using EpowerCom;
using EpowerGlobal;

namespace Epower.ITSM.Web.AppForms
{
    public partial class frmBYTSList : BasePage
    {
        /// <summary>
        /// 
        /// </summary>
        protected string FlowID
        {
            get { if (Request["FlowID"] != null) return Request["FlowID"].ToString(); else return "0"; }
        }


        #region 变量
        long lngUserID = 0;
        long lngDeptID = 0;
        long lngOrgID = 0;
        RightEntity reTrace = null;  //服务跟踪权限 
        #endregion

        #region 方法区

        #region 设置父窗体按钮事件SetParentButtonEvent
        /// <summary>
        /// 设置父窗体按钮事件

        /// </summary>
        protected void SetParentButtonEvent()
        {
            if (this.Request.QueryString["id"] != null)
            {
                this.Master.MainID = this.Request.QueryString["id"].ToString();
            }
            this.Master.Master_Button_GoHistory_Click += new Global_BtnClick(Master_Master_Button_GoHistory_Click);
            this.Master.ShowBackUrlButton(true);
        }
        #endregion

        #region Master_Master_Button_GoHistory_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_GoHistory_Click()
        {
            Response.Write("<script>top.close();</script>");
        }
         #endregion

        #region 页面加载事件
        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtonEvent();
            lngUserID = (long)Session["UserID"];
            lngDeptID = (long)Session["UserDeptID"];
            lngOrgID = (long)Session["UserOrgID"];

            ControlPageIssues.On_PostBack += new EventHandler(ControlPageIssues_On_PostBack);
            ControlPageIssues.DataGridToControl = gridUndoMsg;

            //获得投诉权限
            if (!Page.IsPostBack)
            {
                reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.BYTSQuery];
                BindData();
            }
        }
        #endregion 

        #region 显示页面地址
        /// <summary>
        /// 显示页面地址
        /// </summary>
        /// <param name="lngNoticeID"></param>
        /// <returns></returns>
        protected string GetUrl(decimal lngFlowID)
        {
            string sUrl = "";
            sUrl = "javascript:window.open('../Forms/frmIssueView.aspx?NewWin=true&FlowID=" + lngFlowID.ToString() + "','','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
            return sUrl;
        }

        #endregion

        #region 数据绑定
        /// <summary>
        /// 数据绑定
        /// </summary>
        private void BindData()
        {
            XmlDocument xmlDoc = GetXmlValue();
            DataTable dt = BYTSDP.GetIssuesForCond(xmlDoc.InnerXml, lngUserID, lngDeptID, lngOrgID, reTrace);
            Session["grid_Data_Issues"] = dt;
            ControlPageIssues.DataGridToControl.CurrentPageIndex = 0;
            BindToTable();
        }
        #endregion

        #region  生成查询XML字符串 GetXmlValue
        /// <summary>
        /// 生成查询XML字符串

        /// </summary>
        /// <returns></returns>
        private XmlDocument GetXmlValue()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlRoot = xmlDoc.CreateElement("Fields");
            XmlElement xmlEle;
            #region 客户
            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "CustID");
            xmlEle.SetAttribute("Value", this.Master.MainID == string.Empty ? "-1" : this.Master.MainID);
            xmlRoot.AppendChild(xmlEle);
            #endregion
            #region
            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "FlowID");
            xmlEle.SetAttribute("Value", FlowID);
            xmlRoot.AppendChild(xmlEle);
            #endregion
            xmlDoc.AppendChild(xmlRoot);

            return xmlDoc;
        }
        #endregion

        #region 数据绑定
        /// <summary>
        /// 数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridUndoMsg_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //当超过整个流程预计处理时限未处理的，红低显示
                if (int.Parse(e.Item.Cells[10].Text.Trim()) < 0)
                {
                    for (int i = 0; i < e.Item.Cells.Count; i++)
                    {
                        e.Item.Cells[i].ForeColor = Color.Red;
                    }
                }
                if (e.Item.Cells[7].Text.Trim() == "20")
                {
                    e.Item.Cells[7].Text = "正在处理";
                }
                else if (e.Item.Cells[7].Text.Trim() == "30")
                {
                    e.Item.Cells[7].Text = "正常结束";
                }
                else if (e.Item.Cells[7].Text.Trim() == "40")
                {
                    e.Item.Cells[7].Text = "流程暂停";
                }

                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                string sFlowID = DataBinder.Eval(e.Item.DataItem, "FlowID").ToString();
                e.Item.Attributes.Add("ondblclick", "window.open('../Forms/frmIssueView.aspx?FlowID=" + sFlowID.ToString() + "','','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
            }
        }
        #endregion

        #region 翻页控制
        /// <summary>
        /// 翻页控制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ControlPageIssues_On_PostBack(object sender, EventArgs e)
        {
            BindToTable();
        }
        #endregion

        #region 取得数据并绑定到GRID
        /// <summary>
        /// 
        /// </summary>
        private void BindToTable()
        {
            DataTable dt = (DataTable)Session["grid_Data_Issues"];
            gridUndoMsg.DataSource = dt.DefaultView;
            gridUndoMsg.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");
            gridUndoMsg.DataBind();
        }
        #endregion

        protected void gridUndoMsg_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i < 8)
                    {
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + i.ToString() + ",0);");
                    }
                }
            }
        }
        #endregion
    }
}
