/****************************************************************************
 * 
 * description:巡检计划
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-10-06
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

using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;


namespace Epower.ITSM.Web.Common
{
    public partial class frmEqu_PlanDetailMain : BasePage
    {
        #region 计划类别PlanType
        /// <summary>
        /// 
        /// </summary>
        protected string PlanType
        {
            get
            {
                if (Request["PlanType"] != null)
                    return Request["PlanType"].ToString();
                else
                    return "0";    //默认为设备巡检计划
            }
        }
        #endregion

        #region 修改地址
        protected string EditUrl
        {
            get
            {
                string sUrl = string.Empty;
                switch (PlanType)
                {
                    case "0":
                        sUrl = "frmEqu_PlanDetailEdit.aspx?PlanType=" + PlanType;
                        break;
                    case "1":
                        sUrl = "frmCommom_PlanDetailEdit.aspx?PlanType=" + PlanType;
                        break;
                    default:
                        sUrl = "frmEqu_PlanDetailEdit.aspx?PlanType=" + PlanType;
                        break;
                }
                return sUrl;
            }
        }
        #endregion

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.IsCheckRight = true;
            if (PlanType == "0")   //设备巡检
            {
                this.Master.OperatorID = Constant.Equ_PatrolPlan;
            }
            else   //通用流程巡检
            {
                this.Master.OperatorID = Constant.Equ_CommonPlan;
            }
            dgEA_PlanDetail.Columns[10].Visible = this.Master.GetEditRight();
            dgEA_PlanDetail.Columns[9].Visible = this.Master.GetEditRight();
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);
            this.Master.ShowQueryPageButton();
            this.Master.MainID = "1";
        }
        #endregion

        #region Master_Master_Button_New_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect(EditUrl);
        }
        #endregion

        #region Master_Master_Button_Query_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            Bind();
        }
        #endregion

        #region Master_Master_Button_Delete_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            EA_PlanDetailDP ee = new EA_PlanDetailDP();
            foreach (DataGridItem itm in dgEA_PlanDetail.Items)
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
            Bind();
        }
        #endregion

        #region Page_Load
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            SetParentButtonEvent();
            cpfPlanDetail.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(Bind);
            if (!IsPostBack)
            {
                Bind();
            }
        }
        #endregion

        #region Bind
        /// <summary>
        /// 
        /// </summary>
        private void Bind()
        {
            int iRowCount = 0;
            DataTable dt;
            string sWhere = string.Empty;
            string sOrder = string.Empty;
            if (txtPlanName.Text.Trim() != string.Empty)
            {
                sWhere += " And PlanName like " + StringTool.SqlQ("%" + txtPlanName.Text.Trim() + "%");
            }
            if (ddltRunState.SelectedValue.Trim() != "-1" && ddltRunState.SelectedValue.Trim() != "")
            {
                sWhere += " And RunStatus=" + ddltRunState.SelectedValue.Trim();
            }
            if (ddltPlanState.SelectedValue.Trim() != "-1" && ddltPlanState.SelectedValue.Trim() != "")
            {
                sWhere += " And PlanState=" + ddltPlanState.SelectedValue.Trim();
            }
            if (txtPlanDutyUserName.Text.Trim() != string.Empty)
            {
                sWhere += " And PlanDutyUserName like " + StringTool.SqlQ("%" + txtPlanDutyUserName.Text.Trim() + "%");
            }
            sWhere += " And RefType=" + PlanType;
            EA_PlanDetailDP ee = new EA_PlanDetailDP();
            dt = ee.GetDataTable(sWhere, sOrder, this.cpfPlanDetail.PageSize, this.cpfPlanDetail.CurrentPage, ref iRowCount);
            dgEA_PlanDetail.DataSource = dt.DefaultView;
            dgEA_PlanDetail.DataBind();

            this.cpfPlanDetail.RecordCount = iRowCount;
            this.cpfPlanDetail.Bind();
        }
        #endregion

        #region  dgEA_PlanDetail_ItemCommand
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgEA_PlanDetail_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                Response.Redirect(EditUrl + "&id=" + e.Item.Cells[1].Text.ToString());
            }
            else if (e.CommandName == "effect")
            {
                //计算下次运行时间
                CheckBox chkEffect = (CheckBox)e.Item.Cells[9].FindControl("chkEffect");
                EA_PlanDetailDP ee = new EA_PlanDetailDP();
                ee = ee.GetReCorded(long.Parse(e.Item.Cells[1].Text.ToString()));
                if (chkEffect.Checked)
                {
                    ee.PlanState = (int)ePlan_State.Effect;
                    ee.CalcNextTime(false);  //计算下次执行时间
                    ee.UpdateRecorded(ee);   //更新为有效


                    Label lblNextTime = (Label)e.Item.Cells[6].FindControl("lblNextTime");
                    lblNextTime.Text = ee.NextTime.ToString();
                }
                else
                {
                    ee.PlanState = (int)ePlan_State.Inefficacy;
                    ee.CancelNextTime(false);
                    ee.UpdateRecorded(ee);   //更新为有效
                    Label lblNextTime = (Label)e.Item.Cells[6].FindControl("lblNextTime");
                    lblNextTime.Text = "";
                }
            }
            else if (e.CommandName == "show")
            {
                Response.Redirect("frmEqu_PlanRunLog.aspx?id=" + e.Item.Cells[1].Text.ToString() + "&PlanType=" + PlanType);
            }
            Bind();
        }
        #endregion

        #region dgEA_PlanDetail_ItemCreated
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgEA_PlanDetail_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                int j = 0;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i > 3 && i < e.Item.Cells.Count - 2)
                    {
                        j = i - 3;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
        #endregion

        #region dgEA_PlanDetail_ItemDataBound
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgEA_PlanDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.Item.Cells[2].Text.Trim() == "0")
                    e.Item.Cells[7].Text = "成功";
                else if (e.Item.Cells[2].Text.Trim() == "1")
                    e.Item.Cells[7].Text = "失败";
                else
                    e.Item.Cells[7].Text = "未执行";

                CheckBox chkEffect = (CheckBox)e.Item.Cells[9].FindControl("chkEffect");
                if (e.Item.Cells[3].Text.Trim() == "0")
                    chkEffect.Checked = true;
                else
                    chkEffect.Checked = false;

                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");

                string sUrl;
                switch (PlanType)
                {
                    case "0":
                        sUrl = "frmEqu_PlanDetailEdit.aspx?PlanType=" + PlanType + "&id=" + e.Item.Cells[1].Text.ToString();
                        break;
                    case "1":
                        sUrl = "frmCommom_PlanDetailEdit.aspx?PlanType=" + PlanType + "&id=" + e.Item.Cells[1].Text.ToString();
                        break;
                    default:
                        sUrl = "frmEqu_PlanDetailEdit.aspx?PlanType=" + PlanType + "&id=" + e.Item.Cells[1].Text.ToString();
                        break;
                }

                e.Item.Attributes.Add("ondblclick", "window.open('" + sUrl + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
            }
        }
        #endregion
    }
}
