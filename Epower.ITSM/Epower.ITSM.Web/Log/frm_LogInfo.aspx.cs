/*******************************************************************
 *
 * Description:日志操作
 * 
 * 
 * Create By  :wangxiuwei
 * Create Date:2009年12月21日
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

using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;
using Epower.ITSM.Log;

namespace Epower.ITSM.Web.Log
{
    public partial class frm_LogInfo : BasePage
    {
        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_ExportExcel_Click += new Global_BtnClick(Master_Master_Button_ExportExcel_Click);
            this.Master.ShowQueryPageButton();
            Master.ShowNewButton(false);
            Master.ShowDeleteButton(false);
            this.Master.ShowExportExcelButton(false);
            Master.ShowQueryButton(Master.GetEditRight());
            this.Master.MainID = "1";
        }
        #endregion

        #region Master_Master_Button_ExportExcel_Click 导出数据
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_ExportExcel_Click()
        {
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

        #region Page_Load
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            SetParentButtonEvent();
            cpHol.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(Bind);
            if (!IsPostBack)
            {
                CtrdateBeginTime.dateTime = DateTime.Now.AddMonths(-1);
                CtrdateEndTime.dateTime = DateTime.Now;
                Bind();
            }
        }
        #endregion

        #region  Bind
        /// <summary>
        /// 
        /// </summary>
        private void Bind()
        {
            int iRowCount = 0;

            string sWhere = " 1=1 ";
            string sOrder = string.Empty;
            if (ddlType.SelectedItem.Value.Trim() != "-1")
            {
                sWhere += " And SourceTable=" + StringTool.SqlQ(ddlType.SelectedItem.Value.Trim());
            }
            if (ddlStatus.SelectedItem.Value.Trim() != "-1")
            {
                sWhere += " And Action=" + ddlStatus.SelectedItem.Value.Trim();
            }
            sWhere += " And to_date(OPEndTime,'yyyy-MM-dd HH24:mi:ss') >= to_date('" + CtrdateBeginTime.dateTime.ToString("yyyy-MM-dd") + " 00:00:00 " + "','yyyy-MM-dd HH24:mi:ss')";
            sWhere += " And to_date(OPEndTime,'yyyy-MM-dd HH24:mi:ss') <to_date('" + CtrdateEndTime.dateTime.AddDays(1).ToString("yyyy-MM-dd") + " 23:59:59" + "','yyyy-MM-dd HH24:mi:ss')";

            sOrder = " order by OPEndTime Desc ";
            EpowerOperateLogInfo ee = new EpowerOperateLogInfo();
            DataTable dt = ee.GetDataTable(sWhere, sOrder, this.cpHol.PageSize, this.cpHol.CurrentPage, ref iRowCount);
            dgPrj_OperLogsInfo.DataSource = dt.DefaultView;
            dgPrj_OperLogsInfo.DataBind();
            this.cpHol.RecordCount = iRowCount;
            this.cpHol.Bind();
        }
        #endregion

        #region dgPrj_OperLogsInfo_ItemCreated
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgPrj_OperLogsInfo_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                int j = 0;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i > 0 && i < e.Item.Cells.Count)
                    {
                        j = i - 1;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBackUp_Click(object sender, EventArgs e)
        {
            string sWhere = " 1=1 ";
            string sOrder = string.Empty;
            if (ddlType.SelectedItem.Value.Trim() != "-1")
            {
                sWhere += " And SourceTable=" + StringTool.SqlQ(ddlType.SelectedItem.Value.Trim());
            }
            if (ddlStatus.SelectedItem.Value.Trim() != "-1")
            {
                sWhere += " And Action=" + ddlStatus.SelectedItem.Value.Trim();
            }
            sWhere += " And to_date(OPEndTime,'yyyy-MM-dd HH24:mi:ss') >= to_date('" + CtrdateBeginTime.dateTime.ToString("yyyy-MM-dd") + " 00:00:00 " + "','yyyy-MM-dd HH24:mi:ss')";
            sWhere += " And to_date(OPEndTime,'yyyy-MM-dd HH24:mi:ss') <to_date('" + CtrdateEndTime.dateTime.AddDays(1).ToString("yyyy-MM-dd") + " 23:59:59" + "','yyyy-MM-dd HH24:mi:ss')";

            sOrder = " order by OPEndTime Desc ";
            EpowerOperateLogInfo ee = new EpowerOperateLogInfo();

            E8Logger.Error(sWhere);

            DataTable dt = ee.GetDataTable(sWhere, sOrder);

            Epower.ITSM.Web.Common.ExcelExport.ExportLogList(this, dt, Session["UserID"].ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClear_Click(object sender, EventArgs e)
        {
            string sWhere = " 1=1 ";
            if (ddlType.SelectedItem.Value.Trim() != "-1")
            {
                sWhere += " And SourceTable=" + StringTool.SqlQ(ddlType.SelectedItem.Value.Trim());
            }
            if (ddlStatus.SelectedItem.Value.Trim() != "-1")
            {
                sWhere += " And Action=" + ddlStatus.SelectedItem.Value.Trim();
            }
            sWhere += " And to_date(OPEndTime,'yyyy-MM-dd HH24:mi:ss') >= to_date('" + CtrdateBeginTime.dateTime.ToString("yyyy-MM-dd") + " 00:00:00 " + "','yyyy-MM-dd HH24:mi:ss')";
            sWhere += " And to_date(OPEndTime,'yyyy-MM-dd HH24:mi:ss') <to_date('" + CtrdateEndTime.dateTime.AddDays(1).ToString("yyyy-MM-dd") + " 23:59:59" + "','yyyy-MM-dd HH24:mi:ss')";

            EpowerOperateLogInfo ee = new EpowerOperateLogInfo();
            ee.ClearLogData(sWhere);

            Bind(); //重新加载数据
        }
    }
}


