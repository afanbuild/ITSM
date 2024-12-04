/*******************************************************************
 *
 * Description
 * 
 * 
 * Create By  :zhumc
 * Create Date:2011年8月16日
 * *****************************************************************/
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

using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.AppForms
{
    /// <summary>
    /// 
    /// </summary>
    public partial class frmFormList : BasePage
    {
        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.UserDefineP;
            this.Master.IsCheckRight = true;
            this.dgFC_BILLZL.Columns[this.dgFC_BILLZL.Columns.Count - 1].Visible = this.Master.GetEditRight();
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);
            this.Master.ShowQueryPageButton();
            this.Master.MainID = "1";
            this.Master.ShowNewButton(false);
        }
        #endregion 

        #region Page_Load 初始化页面
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtonEvent();
            cpFC_BILLZL.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(BindData);
            if (!IsPostBack)
            {
                FC_BILLZLDP ee = new FC_BILLZLDP();
                ee.SyncFlowData();   //同步流程模型

                InitPage();
                BindData();
            }
        }

        /// <summary>
        /// 初始化页面
        /// </summary>
        private void InitPage()
        {
            DataTable dtUserType = CommonDP.ExcuteSqlTable("select distinct userType from fc_billzl");   //表单分类
            ddltdjlx.DataSource = dtUserType;
            ddltdjlx.DataTextField = "userType";
            ddltdjlx.DataValueField = "userType";
            ddltdjlx.DataBind();

            //页面初始化时同步流程模型与表单表

        }
        #endregion 

        #region Master_Master_Button_Query_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            BindData();
        }
        #endregion 

        #region Master_Master_Button_Delete_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            FC_BILLZLDP ee = new FC_BILLZLDP();
            foreach (DataGridItem itm in dgFC_BILLZL.Items)
            {
                if (itm.ItemType == ListItemType.AlternatingItem ||
                    itm.ItemType == ListItemType.Item)
                {
                    string sID = itm.Cells[1].Text;
                    CheckBox chkdel = (CheckBox)itm.Cells[0].FindControl("chkDel");
                    if (chkdel.Checked)
                    {
                        ee.DeleteRecorded(long.Parse(sID));
                    }
                }
            }
            BindData();
        }
        #endregion 

        #region BindData
        /// <summary>
        /// 
        /// </summary>
        private void BindData()
        {
            DataTable dt;
            string sWhere = " 1=1 AND oFlowModelID<>0";
            //string sWhere = " 1=1 ";
            string sOrder = " order by djid";
            if (txtdj_name.Text.Trim() != string.Empty)
            {
                sWhere += " And dj_name like " + StringTool.SqlQ("%" + txtdj_name.Text.Trim() + "%");
            }
            if (ddltdjlx.SelectedValue.Trim() != string.Empty)
            {
                sWhere += " And djlx=" + ddltdjlx.SelectedValue.Trim();
            }
            if (txtFormSN.Text.Trim() != string.Empty)
            {
                sWhere += " And djsn like " + StringTool.SqlQ("%" + txtFormSN.Text.Trim() + "%");
            }
            if (txtFlowName.Text.Trim() != string.Empty)
            {
                sWhere += " And FlowName like " + StringTool.SqlQ("%" + txtFlowName.Text.Trim() + "%");
            }
            FC_BILLZLDP ee = new FC_BILLZLDP();
            int iRowCount = 0;
            dt = ee.GetDataTable(sWhere, sOrder, this.cpFC_BILLZL.PageSize, this.cpFC_BILLZL.CurrentPage, ref iRowCount);
            dgFC_BILLZL.DataSource = dt.DefaultView;
            dgFC_BILLZL.DataBind();
            this.cpFC_BILLZL.RecordCount = iRowCount;
            this.cpFC_BILLZL.Bind();
        }
        #endregion

        #region  dgFC_BILLZL_ItemCommand
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgFC_BILLZL_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "edit")  //修改表单
            {
                string stype = e.Item.Cells[3].Text.Trim();
                string sn = e.Item.Cells[4].Text.Trim();
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "Key1", "<script language='javascript'>window.open('../ebsys/fceform/design/design.htm?isfile=no&djtype=" + stype + "&djsn=" + sn + "','')</script>");
                //Response.Redirect("~/ebsys/fceform/design/design.htm?isfile=no&djtype=" + stype + "&djsn=" + sn);
            }
            else if(e.CommandName == "run")  //运行表单
            {
                long lngoFlowModelID = long.Parse(e.Item.Cells[7].Text.Trim());
                long lngFlowModelID = EpowerCom.FlowModel.GetLastVersionFlowModelID(lngoFlowModelID);
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "Key1", "<script language='javascript'>window.open('../Forms/OA_AddNew.aspx?flowmodelid=" + lngFlowModelID.ToString() + "&NewWin=true','')</script>");
            }
        }
        #endregion

        #region dgFC_BILLZL_ItemCreated
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgFC_BILLZL_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                int j = 0;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i > 1 && i < e.Item.Cells.Count - 2)
                    {
                        j = i - 1;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
        #endregion

        #region dgFC_BILLZL_ItemDataBound
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgFC_BILLZL_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                string stype = e.Item.Cells[3].Text.Trim();
                string sn = e.Item.Cells[4].Text.Trim();
                e.Item.Attributes.Add("ondblclick", "window.open('../ebsys/fceform/design/design.htm?isfile=no&djtype=" + stype + "&djsn=" + sn + "','')");
            }
        }
        #endregion 

        #region 同步流程模型 btnSync_Click
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSync_Click(object sender, EventArgs e)
        {
            FC_BILLZLDP ee = new FC_BILLZLDP();
            ee.SyncFlowData();   //同步

            BindData();   //刷新

            PageTool.MsgBox(this,"同步流程模型数据成功！");
        }
        #endregion 
    }
}
