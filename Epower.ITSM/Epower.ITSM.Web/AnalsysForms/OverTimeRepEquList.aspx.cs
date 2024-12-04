/****************************************************************************
 * 
 * description:资产明细表
 * 
 * 
 * 
 * Create by:
 * Create Date:2009-02-11
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
using System.Xml;

namespace Epower.ITSM.Web.AnalsysForms
{
    public partial class OverTimeRepEquList : BasePage
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
            cpCST_Issue.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(LoadData);
            if (!IsPostBack)
            {
                //获取dbo控件的缺省值
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
            int iRowCount = 0;
            string sWhere = string.Empty;
            if (CtrFCDDealStatus.CatelogID != CtrFCDDealStatus.RootID && CtrFCDDealStatus.CatelogID > 0)
            {
                sWhere += " And EquStatusID=" + CtrFCDDealStatus.CatelogID.ToString();
            }
            if (dpdMastShortName.SelectedValue.Trim() != "0")
            {
                sWhere += " And nvl(Costom,0) In (select ID from Br_ECustomer where MastCustID=" + dpdMastShortName.SelectedValue.Trim() + ")";
            }
            string strEquPreWar = CommonDP.GetConfigValue("Other", "EquPreWar");
            if (strEquPreWar == string.Empty)
                strEquPreWar = "0";
            lblDays.Text = strEquPreWar;
            //sWhere += " And DATEDIFF('day', ServiceEndTime, sysdate)>-" + strEquPreWar;
            sWhere += " AND ServiceEndTime < to_date(to_char(sysdate,'yyyy/mm/dd') || ' 23:59:59','yyyy/mm/dd HH24:MI:SS' ) + " + strEquPreWar;

            Equ_DeskDP ee = new Equ_DeskDP();
            DataTable dt = ee.GetEquOverTime(sWhere, "order by Br_ECustomer.ID Desc", int.Parse(strEquPreWar), this.cpCST_Issue.PageSize, this.cpCST_Issue.CurrentPage, ref iRowCount);
            dgMaterialStat.DataSource = dt.DefaultView;
            dgMaterialStat.DataBind();
            this.cpCST_Issue.RecordCount = iRowCount;
            this.cpCST_Issue.Bind();
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

                hw.WriteLine("<table><tr><td><font size=\"3\">资产过期预警列表</font></td></tr></table>");
                //this.dgMaterialStat.Columns[this.dgMaterialStat.Columns.Count - 1].Visible = false;
                //this.dgMaterialStat.RenderControl(hw);
                //this.dgMaterialStat.Columns[this.dgMaterialStat.Columns.Count - 1].Visible = true ;


                int iRowCount = 0;
                string sWhere = string.Empty;
                if (!(CtrFCDDealStatus.CatelogID == CtrFCDDealStatus.RootID || CtrFCDDealStatus.CatelogID == 0))
                {
                    sWhere += " And EquStatusID=" + CtrFCDDealStatus.CatelogID.ToString();
                }
                if (dpdMastShortName.SelectedValue.Trim() != "0")
                {
                    sWhere += " And nvl(Costom,0) In (select ID from Br_ECustomer where MastCustID=" + dpdMastShortName.SelectedValue.Trim() + ")";
                }
                string strEquPreWar = CommonDP.GetConfigValue("Other", "EquPreWar");
                if (strEquPreWar == string.Empty)
                    strEquPreWar = "0";
                lblDays.Text = strEquPreWar;
                sWhere += " And DATEDIFF('day', ServiceEndTime, sysdate)>-" + strEquPreWar;
                Equ_DeskDP ee = new Equ_DeskDP();
                DataTable dt = ee.GetEquOverTime(sWhere, "order by Br_ECustomer.ID Desc", int.Parse(strEquPreWar), 100000000, 1, ref iRowCount);

                hw.WriteLine("<table cellspacing=\"1\" cellpadding=\"1\" rules=\"cols\" rules=\"all\" border=\"0\" class=\"listContent\" border=\"1\"  style=\"width:100%;\"><tr class=\"listTitle\" align=\"center\" onmouseover=\"currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'\"  style=\"background-color:#EEF5FB;\"><td>服务单位</td><td>所属客户</td><td>资产类别</td><td>资产名称</td><td>SN/PN</td><td>超过预警日期（天）</td><td>保修起始时间</td><td>保修截止时间</td></tr>");
                foreach (DataRow dr in dt.Rows)
                {

                    hw.WriteLine("<tr class=\"listTitle\" align=\"center\" onmouseover=\"currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'\"  style=\"background-color:#EEF5FB;\"><td>" + dr["CustMastName"].ToString() + "</td><td>" + dr["CostomName"].ToString() + "</td><td>" + dr["CatalogName"].ToString() + "</td><td>" + dr["Name"].ToString() + "</td><td>" + dr["SerialNumber"].ToString() + "</td><td>" + dr["OverDays"].ToString() + "</td><td>" + DateTime.Parse(dr["ServiceBeginTime"].ToString()).ToShortDateString() + "</td><td>" + DateTime.Parse(dr["ServiceEndTime"].ToString()).ToShortDateString() + "</td></tr>");
                }
                hw.WriteLine("</table>");


                hw.WriteEndTag("body");
                hw.WriteLine();
                hw.WriteEndTag("html");

                //输出到.xls文件
                Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("Content-Disposition","attachment; filename=服务事件趋势分析(事件类型).xls");  --会乱码
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("资产过期预警列表", System.Text.Encoding.UTF8) + ".xls");
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dpdMastShortName_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        #region dgMaterialStat_ItemCreated
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgMaterialStat_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                int j = 0;
                for (int i = 0; i < e.Item.Cells.Count-1; i++)
                {
                    if (i >= 1)
                    {
                        j = i - 1;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
        #endregion 

        protected void dgMaterialStat_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                e.Item.Attributes.Add("ondblclick", "window.open('../EquipmentManager/frmEqu_DeskEdit.aspx?IsSelect=1&id=" + e.Item.Cells[0].Text.ToString() + "&FlowID=0&Soure=0&newWin=0', '', 'scrollbars=yes,status=yes ,resizable=yes,width=800,height=600');");
            }
        }
    }
}
