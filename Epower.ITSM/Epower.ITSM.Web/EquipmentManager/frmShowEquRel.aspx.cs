/****************************************************************************
 * 
 * description:资产关联展示
 * 
 * 
 * 
 * Create by:
 * Create Date:2010-06-28
 * *************************************************************************/
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using System.Drawing;

using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using EpowerGlobal;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL.EquipmentManager;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frmShowEquRel : BasePage
    {
        /// <summary>
        /// 资产ID
        /// </summary>
        protected string EquID
        {
            get
            {
                if (ViewState["EquID"] != null)
                {
                    return ViewState["EquID"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState["EquID"] = value;
            }

        }

        /// <summary>
        /// 关联类别
        /// </summary>
        protected string RelType
        {
            get
            {
                if (ViewState["RelType"] != null)
                {
                    return ViewState["RelType"].ToString();
                }
                else
                {
                    return "1";
                }
            }
            set
            {
                ViewState["RelType"] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                EquID = Request["EquID"].ToString();
                RelType = Request["Type"].ToString();
                switch (RelType)
                {
                    case "1":   //资产事件记录
                        Table1.Visible = true;
                        BindData();
                        break;
                    case "2":   //巡检记录
                        //巡检记录
                        BindPatrolData();
                        Table2.Visible = true;
                        break;
                    case "3":  //变更记录
                        //变更记录
                        BindChangeData();
                        Table3.Visible = true;
                        break;
                    case "4":  //关联资产
                        //关系资产
                        BindEquRel();
                        Table4.Visible = true;

                        Equ_RelNameDP equRelNameDP = new Equ_RelNameDP();
                        DataTable dt = equRelNameDP.GetAll();

                        DataRow dr = dt.NewRow();
                        dr[0] = -1;
                        dr[1] = "请选择";
                        dt.Rows.InsertAt(dr, 0);


                        ddlYourRelName.DataSource = dt;
                        ddlYourRelName.DataTextField = "RelKey";
                        ddlYourRelName.DataValueField = "ID";
                        ddlYourRelName.DataBind();

                        break;
                    case "5":  //资产关联于
                        BindEquRelIn();
                        Table5.Visible = true;
                        break;
                    case "6":  //相关问题单
                        DataTable dtProblem = ProblemDealDP.GetProblemData(long.Parse(EquID));
                        dgProblem.DataSource = dtProblem;
                        dgProblem.DataBind();
                        Table6.Visible = true;
                        break;
                    case "7":  //相关变更单
                        DataTable dtChange = ChangeDealDP.GetChangeData(long.Parse(EquID));
                        dgChangeEvent.DataSource = dtChange;
                        dgChangeEvent.DataBind();
                        Table7.Visible = true;
                        break;
                }
            }
        }


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
            sUrl = "javascript:window.open('../Forms/frmIssueView.aspx?NewWin=true&FlowID=" + lngFlowID.ToString() + "','','scrollbars=yes,resizable=yes,top=0,left=0,width=1277,height=657');";
            //   sUrl = "javascript:window.open('../Forms/frmIssueView.aspx?NewWin=true&FlowID=" + lngFlowID.ToString() + "','','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";

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
            #region 设备
            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "Equipmentid");
            xmlEle.SetAttribute("Value", EquID == string.Empty ? "-1" : EquID);
            xmlRoot.AppendChild(xmlEle);
            #endregion
            #region
            if (Request["FlowID"] != null)
            {
                xmlEle = xmlDoc.CreateElement("Field");
                xmlEle.SetAttribute("FieldName", "FlowID");
                xmlEle.SetAttribute("Value", Request["FlowID"].ToString());
                xmlRoot.AppendChild(xmlEle);
            }
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
            RightEntity reTrace = null;  //权限
            reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.EquManager];
            XmlDocument xmlDoc = GetXmlValue();
            DataTable dt = ZHServiceDP.GetIssuesForCondForClientManage(xmlDoc.InnerXml, long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString())
           , long.Parse(Session["UserOrgID"].ToString()), reTrace);
            BindToTable(dt);
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
                fs = (e_FlowStatus)int.Parse(e.Item.Cells[0].Text);

                if (int.Parse(e.Item.Cells[8].Text.Trim()) < 0 && fs != e_FlowStatus.efsEnd)
                {
                    for (int i = 0; i < e.Item.Cells.Count; i++)
                    {
                        e.Item.Cells[i].ForeColor = Color.Red;
                    }
                }

                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                e.Item.Attributes.Add("ondblclick", "window.open('../Forms/frmIssueView.aspx?NewWin=true&FlowID=" + e.Item.Cells[7].Text.Trim() + "','','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
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
                int j = 0;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i > 0 && i < 6)
                    {
                        j = i - 1;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        private void BindToTable(DataTable dt)
        {
            gridUndoMsg.DataSource = dt.DefaultView;
            gridUndoMsg.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");
            gridUndoMsg.DataBind();
        }
        #endregion

        #region 巡检记录
        #region 绑定数据 Bind
        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindPatrolData()
        {
            #region BindPatrolData
            string sWhere = "";
            if (EquID != string.Empty)  //设备/产品名称	
            {
                sWhere += " And a.ID in (select PatrolID from Equ_PatrolItemData where 1=1 and EquID=" + EquID + ")";
            }
            else
            {
                sWhere += " And 1<>1";
            }
            DataTable dt = Equ_PatrolDataDP.GetFieldsTable(long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()), long.Parse(Session["UserOrgID"].ToString()),
                            (Epower.DevBase.Organization.SqlDAL.RightEntity)((Hashtable)Session["UserAllRights"])[Epower.ITSM.Base.Constant.EquPatrolQuery], sWhere);
            BindToTablePatrol(dt);
            #endregion
        }
        #endregion

        #region 翻页控制
        /// <summary>
        /// 
        /// </summary>
        private void BindToTablePatrol(DataTable dt)
        {
            grd.DataSource = dt.DefaultView;
            grd.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");
            grd.DataBind();
        }
        #endregion

        #region 排序 grd_ItemCreated
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                int j = 0;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i < e.Item.Cells.Count - 1)
                    {
                        j = i - 0;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
        #endregion
        #endregion

        #region 变更记录
        #region 绑定数据 BindChangeData
        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindChangeData()
        {
            #region BindChangeData
            string sWhere = "";
            if (EquID != string.Empty)  //设备/产品名称	
            {
                sWhere += " And a.ID in (SELECT changeid FROM EQU_CHANGESERVICEDETAILS where 1=1 AND equid=" + EquID + ")";
            }
            else
            {
                sWhere += " And 1<>1";
            }
            DataTable dt = ChangeDealDP.GetChangeDealData(long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()), long.Parse(Session["UserOrgID"].ToString()),
                            (Epower.DevBase.Organization.SqlDAL.RightEntity)((Hashtable)Session["UserAllRights"])[Epower.ITSM.Base.Constant.EquChangeQuery], sWhere);
            BindToTableChange(dt);
            #endregion
        }
        #endregion

        #region 翻页控制
        /// <summary>
        /// 
        /// </summary>
        private void BindToTableChange(DataTable dt)
        {
            dgChange.DataSource = dt.DefaultView;
            dgChange.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");

            dgChange.DataBind();
        }
        #endregion

        #region 排序 dgChange_ItemCreated
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgChange_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                int j = 0;
                for (int i = 0; i < e.Item.Cells.Count - 1; i++)
                {
                    if (i > 0)
                    {
                        j = i - 1;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
        #endregion
        #endregion

        #region 关联资产
        /// <summary>
        /// 加载数据
        /// </summary>
        private void BindEquRel()
        {
            Equ_RelDP ee = new Equ_RelDP();
            string sID = EquID == string.Empty ? "-1" : EquID;
            string sWhere = " and Equ_ID=" + sID;
            DataTable dt = ee.GetDataTable(sWhere, string.Empty);
            dgPro_ProblemAnalyse.DataSource = dt.DefaultView;
            dgPro_ProblemAnalyse.DataBind();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgPro_ProblemAnalyse_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i > 2 && i < 6)
                    {
                        int j = i - 3;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
        #endregion

        #region 关联资产于


        /// <summary>
        /// 加载数据
        /// </summary>
        private void BindEquRelIn()
        {
            Equ_RelDP ee = new Equ_RelDP();
            string sID = EquID == string.Empty ? "-1" : EquID;
            string sWhere = " and RelID=" + sID;
            DataTable dt = ee.GetDataTable(sWhere, string.Empty);
            dg_EquRelIn.DataSource = dt.DefaultView;
            dg_EquRelIn.DataBind();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dg_EquRelIn_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i > 2 && i < 6)
                    {
                        int j = i - 3;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
        #endregion

        #region 相关问题单


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgProblem_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                decimal sScale = StringTool.String2Decimal(e.Item.Cells[10].Text);
                decimal sEffect = StringTool.String2Decimal(e.Item.Cells[11].Text);
                decimal sStress = StringTool.String2Decimal(e.Item.Cells[12].Text);

                if (sScale > 50)
                    e.Item.Cells[10].ForeColor = System.Drawing.Color.Red;
                if (sEffect > 50)
                    e.Item.Cells[11].ForeColor = System.Drawing.Color.Red;
                if (sStress > 50)
                    e.Item.Cells[12].ForeColor = System.Drawing.Color.Red;

                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                e.Item.Attributes.Add("ondblclick", "window.open('../Forms/frmIssueView.aspx?NewWin=true&FlowID=" + DataBinder.Eval(e.Item.DataItem, "FlowID").ToString() + "','','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
            }
        }
        #endregion

        #region 相关变更单
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgChangeEvent_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                e.Item.Attributes.Add("ondblclick", "window.open('../Forms/frmIssueView.aspx?NewWin=true&FlowID=" + DataBinder.Eval(e.Item.DataItem, "FlowID").ToString() + "','','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
            }
        }

        #endregion

        protected void grd_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                e.Item.Attributes.Add("ondblclick", "window.open('../Forms/frmIssueView.aspx?NewWin=true&FlowID=" + DataBinder.Eval(e.Item.DataItem, "FlowID").ToString() + "','','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
            }
        }

        protected void dgChange_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                e.Item.Attributes.Add("ondblclick", "window.open('../Forms/frmIssueView.aspx?NewWin=true&FlowID=" + DataBinder.Eval(e.Item.DataItem, "FlowID").ToString() + "','','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
            }
        }

        protected void dgPro_ProblemAnalyse_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
            }
        }

        protected void dg_EquRelIn_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");

                //资产编号

                ((Label)e.Item.FindControl("lblCode")).Text = Equ_DeskDP.GetEquCodeByID(e.Item.Cells[0].Text);
                ((HyperLink)e.Item.FindControl("hypBtnRelEqu")).Text = Equ_DeskDP.GetEquNameByID(e.Item.Cells[0].Text);     //所影响的资产名称                

                ((HyperLink)e.Item.FindControl("hypBtnRelEqu")).Target = "_blank";
                ((HyperLink)e.Item.FindControl("hypBtnRelEqu")).NavigateUrl = "frmEqu_DeskEdit.aspx?IsSelect=1&FlowID=0&IsRel=0&ID=" + e.Item.Cells[0].Text;
            }
        }
    }
}
