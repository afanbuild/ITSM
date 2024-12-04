/****************************************************************************
 * 
 * description:材料统计分析
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-08-03
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
using System.Drawing;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.AnalsysForms
{
    public partial class frmzhCstWork : BasePage
    {
        #region 设置父窗体按钮事件SetParentButtonEvent
        /// <summary>
        /// 设置父窗体按钮事件

        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_ExportExcel_Click += new Global_BtnClick(btnToExcel_Click);
            this.Master.ShowExportExcelButton(true);
        }


        #endregion

        #region 页面初始化 Page_Load
        /// <summary>
        /// 页面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            SetParentButtonEvent();
            this.CtrFCDDealStatus.mySelectedIndexChanged += new EventHandler(CtrFCDDealStatus_mySelectedIndexChanged);

            if (!IsPostBack)
            {
                //设置起始日期
                string sQueryBeginDate = string.Empty;
                //sQueryBeginDate = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-01";
                sQueryBeginDate = (DateTime.Now.Year - 1).ToString() + "-" + DateTime.Now.Month.ToString() + "-01";
                ctrDateTime.BeginTime = DateTime.Parse(sQueryBeginDate).ToString("yyyy-MM-dd");
                ctrDateTime.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                SetMastCustomDboValue();
                LoadData();
            }
            else
            {
                if (Request.Form["__EVENTTARGET"] == "datarange")
                {
                    LoadData();
                }
            }
        }

        #endregion

        #region 选择服务状态时发生事件 CtrFCDDealStatus_mySelectedIndexChanged
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CtrFCDDealStatus_mySelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }
        #endregion 

        #region 加载数据 LoadData
        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadData()
        {
            string strBeginDate = ctrDateTime.BeginTime;
            string strEndDate = ctrDateTime.EndTime;
            long lngStatusID = (CtrFCDDealStatus.CatelogID == CtrFCDDealStatus.RootID) ? 0 : CtrFCDDealStatus.CatelogID;
            DataTable dt;
            dt = ZHServiceDP.StatStaffWorks(strBeginDate, strEndDate, lngStatusID, long.Parse(dpdMastShortName.SelectedValue));
            dgMaterialStat.DataSource = dt.DefaultView;
            dgMaterialStat.DataBind();
        }
        #endregion

        #region 导出Excel btnToExcel_Click
        /// <summary>
        /// 导出Excel
        /// </summary>
        protected void btnToExcel_Click()
        {
            try
            {
                LoadData(); ;
                ToExcel();
            }
            catch { }
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void ToExcel()
        {
            try
            {
                System.IO.StringWriter tw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);

                hw.WriteFullBeginTag("html");
                hw.WriteLine();
                hw.WriteFullBeginTag("head");
                hw.WriteLine();
                hw.WriteLine("<meta http-equiv=Content-Type Content=text/html; charset=utf-8>");
                hw.WriteEndTag("head");
                hw.WriteLine();
                hw.WriteFullBeginTag("body");
                hw.WriteLine();

                hw.WriteLine("<table><tr><td><font size=\"3\">执行人工作量统计</font></td></tr></table>");
                this.dgMaterialStat.RenderControl(hw);


                hw.WriteEndTag("body");
                hw.WriteLine();
                hw.WriteEndTag("html");

                //输出到.xls文件
                Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("Content-Disposition","attachment; filename=服务事件趋势分析(事件类型).xls");  --会乱码
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("执行人工作量统计", System.Text.Encoding.UTF8) + ".xls");
                Response.Charset = "utf-8";
                //this.EnableViewState = false;
                Response.Write(tw.ToString());
                Response.End();
            }
            catch
            {
            }
        }
        #endregion   

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dpdMastShortName_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }
        /// <summary>
        /// 
        /// </summary>
        private void SetMastCustomDboValue()
        {
            DataTable dt = ZHServiceDP.GetMastCustomer();
            DataView dv = new DataView(dt);
            dv.Sort = "ID";
            dpdMastShortName.DataSource = dv;
            dpdMastShortName.DataTextField = "ShortName";
            dpdMastShortName.DataValueField = "ID";
            dpdMastShortName.DataBind();

            dpdMastShortName.Items.Insert(0, new ListItem("全部", "0"));

            dt.Dispose();
        }

        protected void dgMaterialStat_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
            }
        }

        protected void dgMaterialStat_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i == 2 || i == 3 || i == 4)
                    {
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + i.ToString() + ",1);");
                    }
                    else
                    {
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + i.ToString() + ",0);");
                    }
                }
            }
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
