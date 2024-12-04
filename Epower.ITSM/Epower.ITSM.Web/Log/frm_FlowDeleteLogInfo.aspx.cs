/*******************************************************************
 *
 * Description:流程删除日志操作
 * 
 * 
 * Create By  :zhumc
 * Create Date:2011年2月14日
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
    public partial class frm_FlowDeleteLogInfo : BasePage
    {
        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {            
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.ShowQueryPageButton();
            Master.ShowNewButton(false);
            Master.ShowDeleteButton(false);
            this.Master.ShowExportExcelButton(false);
            Master.ShowQueryButton(Master.GetEditRight());
            this.Master.MainID = "1";
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

                //绑定应用
                es_flowdelelogDP ee = new es_flowdelelogDP();
                DataTable dt = ee.GetAllAppData();
                ddltApp.DataSource = dt;
                ddltApp.DataTextField = "AppName";
                ddltApp.DataValueField = "AppID";
                ddltApp.DataBind();

                ddltApp.Items.Insert(0, new ListItem("--全部应用--","-1"));

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
            if (ddltApp.SelectedItem.Value.Trim() != "-1")
            {
                sWhere += " And AppID=" + StringTool.SqlQ(ddltApp.SelectedItem.Value.Trim());
            }
            sWhere += " And DeletedTime >=to_date(" + StringTool.SqlQ(CtrdateBeginTime.dateTime.ToString("yyyy-MM-dd")) + ",'yyyy-mm-dd   hh24:mi:ss')";
            sWhere += " And DeletedTime <to_date(" + StringTool.SqlQ(CtrdateEndTime.dateTime.AddDays(1).ToString("yyyy-MM-dd")) + ",'yyyy-mm-dd   hh24:mi:ss')";

            sOrder = " order by DeletedTime Desc ";
            es_flowdelelogDP ee = new es_flowdelelogDP();
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
                    if (i > 0 && i < e.Item.Cells.Count )
                    {
                        j = i - 1;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
        #endregion
    }
}


