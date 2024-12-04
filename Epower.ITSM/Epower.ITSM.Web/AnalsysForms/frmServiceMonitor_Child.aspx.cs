using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Epower.ITSM.SqlDAL;
using EpowerCom;

namespace Epower.ITSM.Web.AnalsysForms
{
    public partial class frmServiceMonitor_Child : BasePage
    {
        private int sType = 0; //请求类型

        protected void Page_Load(object sender, EventArgs e)
        {
            cpCST_Issue.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(BindData);
            cpCST_Change.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(BindData);
            cpCST_Problem.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(BindData);
            cpCST_Equ.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(BindData);
            cpCST_Inf.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(BindData);

            this.Master.TableVisible = false;

            if (!IsPostBack)
            {
                if (Request.QueryString["strArr"] != null)
                {
                    hidStrArr.Value = Request.QueryString["strArr"].ToString();
                }

                BindData();
            }           
            
        }

        #region BindData
        /// <summary>
        /// 
        /// </summary>
        private void BindData()
        {
            DataTable dtChild = new DataTable();
            int iRowCount = 0;

            //第一次出现","的位置
            int iFir = hidStrArr.Value.IndexOf(',');

            string strType = string.Empty;          //类型
            if (iFir > 0)
            {
                strType = hidStrArr.Value.Substring(0, iFir);                       //得到查询的类型[事件A、B、C、D等]
            }
            string strWhere = string.Empty;         //sql语句的查询条件

            if (hidStrArr.Value.Length > 0 && hidStrArr.Value.Length >= iFir + 1)
            {
                strWhere = hidStrArr.Value.Substring(iFir + 1, hidStrArr.Value.Length - iFir - 1);          //得到sql语句的where条件
            }

            #region 事件
            if (strType == "IssueA" || strType == "IssueB" || strType == "IssueC" || strType == "IssueD" || strType == "IssueW")
            {
                sType = 1;

                TabIssue.Visible = true;
                TabChange.Visible = false;
                tabProblem.Visible = false;
                tabEqu.Visible = false;
                tabInf.Visible = false;

                dtChild = ZHServiceDP.GetSMDetail(strWhere, strType, cpCST_Issue.PageSize, cpCST_Issue.CurrentPage, ref iRowCount);
                gridIssues.DataSource = dtChild;
                gridIssues.DataBind();
                cpCST_Issue.RecordCount = iRowCount;
                cpCST_Issue.Bind();
            }
            #endregion
            #region 变更
            if (strType == "ChangeA" || strType == "ChangeB" || strType == "ChangeC" || strType == "ChangeD" || strType == "ChangeW")
            {
                sType = 2;

                TabIssue.Visible = false;
                TabChange.Visible = true;
                tabProblem.Visible = false;
                tabEqu.Visible = false;
                tabInf.Visible = false;

                dtChild = ChangeDealDP.GetSMDetail(strWhere, strType, cpCST_Change.PageSize, cpCST_Change.CurrentPage, ref iRowCount);
                dgChange.DataSource = dtChild;
                dgChange.DataBind();
                cpCST_Change.RecordCount = iRowCount;
                cpCST_Change.Bind();
            }
            #endregion
            #region 问题
            if (strType == "ProblemA" || strType == "ProblemB" || strType == "ProblemC" || strType == "ProblemW")
            {
                sType = 3;

                TabIssue.Visible = false;
                TabChange.Visible = false;
                tabProblem.Visible = true;
                tabEqu.Visible = false;
                tabInf.Visible = false;

                dtChild = ProblemDealDP.GetSMDetail(strWhere, strType, cpCST_Problem.PageSize, cpCST_Problem.CurrentPage, ref iRowCount);
                dgProblem.DataSource = dtChild;
                dgProblem.DataBind();
                cpCST_Problem.RecordCount = iRowCount;
                cpCST_Problem.Bind();
            }
            #endregion
            #region 资产
            if (strType == "EquA" || strType == "EquB" || strType == "EquC" || strType == "EquD" || strType == "EquE")
            {
                sType = 4;

                TabIssue.Visible = false;
                TabChange.Visible = false;
                tabProblem.Visible = false;
                tabEqu.Visible = true;
                tabInf.Visible = false;

                dtChild = Equ_DeskDP.GetSMDetail(strWhere, strType, cpCST_Equ.PageSize, cpCST_Equ.CurrentPage, ref iRowCount);
                dgEqu.DataSource = dtChild;
                dgEqu.DataBind();
                cpCST_Equ.RecordCount = iRowCount;
                cpCST_Equ.Bind();
            }
            #endregion
            #region 知识
            if (strType == "InfA" || strType == "InfB" || strType == "InfC" || strType == "InfD")
            {
                sType = 5;

                TabIssue.Visible = false;
                TabChange.Visible = false;
                tabProblem.Visible = false;
                tabEqu.Visible = false;
                tabInf.Visible = true;

                dtChild = Inf_InformationDP.GetSMDetail(strWhere, strType, cpCST_Inf.PageSize, cpCST_Inf.CurrentPage, ref iRowCount);
                dgInf.DataSource = dtChild;
                dgInf.DataBind();
                cpCST_Inf.RecordCount = iRowCount;
                cpCST_Inf.Bind();
            }
            #endregion
            #region 满意度
            if (strType == "FeedBackA" || strType == "FeedBackB" || strType == "FeedBackC")
            {
                sType = 1;

                TabIssue.Visible = true;
                TabChange.Visible = false;
                tabProblem.Visible = false;
                tabEqu.Visible = false;
                tabInf.Visible = false;

                dtChild = RiseDP.GetSMDetail(strWhere, strType, cpCST_Issue.PageSize, cpCST_Issue.CurrentPage, ref iRowCount);
                gridIssues.DataSource = dtChild;
                gridIssues.DataBind();
                cpCST_Issue.RecordCount = iRowCount;
                cpCST_Issue.Bind();
            }
            #endregion
        }
        #endregion

        #region GetUrl
        /// <summary>
        /// GetUrl
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        protected string GetUrl(decimal lngFlowID)
        {
            //暂时没处理分页
            string sUrl = "";
            sUrl = "javascript:window.open('../Forms/frmIssueView.aspx?NewWin=true&FlowID=" + lngFlowID.ToString() + "','','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
            return sUrl;
        }
        #endregion

        #region GetEqu
        /// <summary>
        /// GetEqu
        /// </summary>
        /// <param name="lngEquID"></param>
        /// <returns></returns>
        protected string GetEqu(decimal lngEquID)
        {
            //暂时没处理分页
            string sUrl = "";
            sUrl = "javascript:window.open('../EquipmentManager/frmEqu_DeskEdit.aspx?IsTanChu=true&FlowID=true&id=" + lngEquID.ToString() + "','','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
            return sUrl;
        }
        #endregion

        #region GetInf
        /// <summary>
        /// GetInf
        /// </summary>
        /// <param name="lngEquID"></param>
        /// <returns></returns>
        protected string GetInf(decimal lngEquID)
        {
            //暂时没处理分页
            string sUrl = "";
            sUrl = "javascript:window.open('../InformationManager/frmKBShow.aspx?KBID=" + lngEquID.ToString() + "','','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
            return sUrl;
        }
        #endregion        

        #region dg_ItemDataBound
        /// <summary>
        /// dg_ItemDataBound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dg_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");

                string sID = "";

                if (sType < 4)
                {
                    sID = DataBinder.Eval(e.Item.DataItem, "FlowID").ToString();
                    e.Item.Attributes.Add("ondblclick", "window.open('../Forms/frmIssueView.aspx?NewWin=true&FlowID=" + sID.ToString() + "','','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
                }
                else
                {
                    sID = DataBinder.Eval(e.Item.DataItem, "ID").ToString();

                    if (sType == 4)
                        e.Item.Attributes.Add("ondblclick", "window.open('../EquipmentManager/frmEqu_DeskEdit.aspx?IsTanChu=true&FlowID=true&id=" + sID.ToString() + "','','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
                    else
                        e.Item.Attributes.Add("ondblclick", "window.open('../InformationManager/frmKBShow.aspx?KBID=" + sID.ToString() + "','','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");

                }
            }
        }
        #endregion

        #region dg_ItemCreated
        /// <summary>
        /// dg_ItemCreated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dg_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count - 1; i++)
                {
                    int j = i;
                    e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                }
            }
        }
        #endregion
    }
}
