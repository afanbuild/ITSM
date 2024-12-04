/****************************************************************************
 * 
 * description:客户历史服务记录
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
    public partial class frmIssueList : BasePage
    {
        RightEntity reTrace = null;

        /// <summary>
        /// 流程ID 
        /// </summary>
        protected string FlowID
        {
            get { if (Request["FlowID"] != null) return Request["FlowID"].ToString(); else return "0"; }
        }

        /// <summary>
        /// 服务类别
        /// </summary>
        protected string ServiceType
        {
            get { if (Request["ServiceType"] != null) return Request["ServiceType"].ToString(); else return "0"; }
        }

        /// <summary>
        /// 资产ID
        /// </summary>
        protected string EquID
        {
            get { if (Request["EquID"] != null && Request["EquID"] != "") return Request["EquID"].ToString(); else return "0"; }
        }

        #region 是否查看历史参照
        /// <summary>
        /// 是否查看历史参照
        /// </summary>
        protected string IsHistory
        {
            get { if (Request["IsHistory"] != null && Request["IsHistory"] == "true") return "true"; else return "false"; }
        }
        #endregion


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

        #region Master_Master_Button_GoHistory_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_GoHistory_Click()
        {
            Response.Write("<script>top.close();</script>");
        }
        #endregion

        #region Page_Load
        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //设置主页面
            SetParentButtonEvent();
            ControlPageIssues.On_PostBack += new EventHandler(ControlPageIssues_On_PostBack);
            ControlPageIssues.DataGridToControl = gridUndoMsg;
            if (!IsPostBack)
            {
                reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.CustomerService];

                gridUndoMsg.Columns[0].HeaderText = PageDeal.GetLanguageValue("CST_Subject");
                gridUndoMsg.Columns[1].HeaderText = PageDeal.GetLanguageValue("CST_RegUserName");
                gridUndoMsg.Columns[2].HeaderText = PageDeal.GetLanguageValue("CST_CustTime");
                gridUndoMsg.Columns[3].HeaderText = PageDeal.GetLanguageValue("CST_CustName");
                gridUndoMsg.Columns[4].HeaderText = PageDeal.GetLanguageValue("CST_CustContract");
                gridUndoMsg.Columns[5].HeaderText = PageDeal.GetLanguageValue("CST_DealStatus");  

                BindData();
            }
        }
        #endregion 

        #region 服务记录
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngNoticeID"></param>
        /// <returns></returns>
        protected string GetUrl(decimal lngFlowID)
        {
            //暂时没处理分页
            string sUrl = "";
            sUrl = "javascript:window.open('../Forms/frmIssueView.aspx?NewWin=true&FlowID=" + lngFlowID.ToString() + "','','scrollbars=yes,resizable=yes,width=800,height=600');";

            return sUrl;


        }

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
            #region 流程ID
            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "FlowID");
            xmlEle.SetAttribute("Value", FlowID);
            xmlRoot.AppendChild(xmlEle);
            #endregion
            #region 服务类别
            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "servicetypeid");
            xmlEle.SetAttribute("Value", ServiceType);
            xmlRoot.AppendChild(xmlEle);
            #endregion
            #region 资产ID
            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "EquID");
            xmlEle.SetAttribute("Value", EquID);
            xmlRoot.AppendChild(xmlEle);
            #endregion

            #region 查看历史参照
            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "IsHistory");
            xmlEle.SetAttribute("Value", IsHistory);
            xmlRoot.AppendChild(xmlEle);
            #endregion

            xmlDoc.AppendChild(xmlRoot);

            return xmlDoc;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        private void BindData()
        {
            XmlDocument xmlDoc = GetXmlValue();
            DataTable dt = ZHServiceDP.GetIssuesForCondForClientManage(xmlDoc.InnerXml, long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString())
           , long.Parse(Session["UserOrgID"].ToString()), reTrace);
            Session["Cust_Data_Issues"] = dt;
            ControlPageIssues.DataGridToControl.CurrentPageIndex = 0;
            BindToTable();
        }

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
                fs = (e_FlowStatus)int.Parse(e.Item.Cells[8].Text);

                if (int.Parse(e.Item.Cells[9].Text.Trim()) < 0 && fs != e_FlowStatus.efsEnd)
                {
                    for (int i = 0; i < e.Item.Cells.Count; i++)
                    {
                        e.Item.Cells[i].ForeColor = Color.Red;
                    }
                }
                if (e.Item.Cells[6].Text.Trim() == "20")
                {
                    e.Item.Cells[6].Text = "正在处理";
                }
                else if (e.Item.Cells[6].Text.Trim() == "30")
                {
                    e.Item.Cells[6].Text = "正常结束";
                }
                else if (e.Item.Cells[6].Text.Trim() == "40")
                {
                    e.Item.Cells[6].Text = "流程暂停";
                }
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                string sFlowID = DataBinder.Eval(e.Item.DataItem, "FlowID").ToString();
                e.Item.Attributes.Add("ondblclick", "window.open('../Forms/frmIssueView.aspx?FlowID=" + sFlowID.ToString() + "','','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");

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
                    // (DataView)e.Item.NamingContainer;
                    if (i == 0 || i == 1 || i == 2 || i == 3 || i == 4 || i == 5 || i == 6)
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
        private void ControlPageIssues_On_PostBack(object sender, EventArgs e)
        {
            BindToTable();
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindToTable()
        {
            DataTable dt = (DataTable)Session["Cust_Data_Issues"];

            //gridUndoMsg.DataSource = dw;
            gridUndoMsg.DataSource = dt.DefaultView;
            gridUndoMsg.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");

            gridUndoMsg.DataBind();
        }
        #endregion 
    }
}
