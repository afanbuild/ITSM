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
    public partial class frmEqu_PlanRunLog : BasePage
    {
        #region PlanID
        /// <summary>
        /// 
        /// </summary>
        public string PlanID
        {
            get
            {
                if (Request["ID"] != null)
                    return Request["ID"].ToString();
                else
                    return "0";
            }
        }
        #endregion 

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

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            //this.Master.ShowQueryPageButton();
            this.Master.Master_Button_GoHistory_Click += new Global_BtnClick(Master_Master_Button_GoHistory_Click);
            this.Master.ShowBackUrlButton(true);
            this.Master.MainID = "1";
        }
        #endregion

        #region Master_Master_Button_GoHistory_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_GoHistory_Click()
        {
            Response.Redirect("frmEqu_PlanDetailMain.aspx?PlanType=" + PlanType);
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
            ControlPage1.On_PostBack += new EventHandler(ControlPage1_On_PostBack);
            ControlPage1.DataGridToControl = dgEA_PlanDetail;
            if (!IsPostBack)
            {
                DataTable dt = LoadData();
                dgEA_PlanDetail.DataSource = dt.DefaultView;
                dgEA_PlanDetail.DataBind();
            }
        }
        #endregion

        #region LoadData
        /// <summary>
        /// 
        /// </summary>
        private DataTable LoadData()
        {
            DataTable dt;
            dt = EA_PlanDetailDP.GetPlanRunLog(decimal.Parse(PlanID)); ;
            Session["EA_PlanDetailLog"] = dt;
            return dt;
        }
        #endregion

        #region  Bind
        /// <summary>
        /// 
        /// </summary>
        private void Bind()
        {
            DataTable dt;
            if (Session["EA_PlanDetailLog"] == null)
            {
                dt = LoadData();
            }
            else
            {
                dt = (DataTable)Session["EA_PlanDetailLog"];
            }
            dgEA_PlanDetail.DataSource = dt.DefaultView;
            dgEA_PlanDetail.DataBind();
        }
        #endregion

        #region ControlPage1_On_PostBack
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ControlPage1_On_PostBack(object sender, EventArgs e)
        {
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
                    if (i >= 3 && i < e.Item.Cells.Count)
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
            }
        }
        #endregion 
    }
}
