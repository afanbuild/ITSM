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
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.AnalsysForms
{
    public partial class frmEquStatistics : BasePage
    {
        #region 设置父窗体按钮事件 SetParentButtonEvent
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_ExportExcel_Click +=new Global_BtnClick(Master_Master_Button_ExportExcel_Click);
            this.Master.ShowExportExcelButton(true);
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtonEvent();

            this.ctrEquCataDropList1.mySelectedIndexChanged+=new EventHandler(ctrEquCataDropList1_mySelectedIndexChanged);
            if (!IsPostBack)
            {
                //初始化服务单位下拉列表的值
                InitddlMastCust();

                LoadData();
            }

        }

        #region 资产类别改变时的事件 ctrEquCataDropList1_mySelectedIndexChanged
        protected void ctrEquCataDropList1_mySelectedIndexChanged(object sender, System.EventArgs e)
        {
            LoadData();
        }
        #endregion 

        #region 加载数据 LoadData
        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            ZHServiceDP ee = new ZHServiceDP();
            DataTable dt;
            String sWhere = " and 1=1 ";
            String sOrder = " order by MastCust Desc ";

            String sMastCust = ddlMastCust.SelectedItem.Text;
            String sCalalogID = ctrEquCataDropList1.CatelogID.ToString();

            if (sMastCust != "全部")
            {
                sWhere = " and MastCust = " + StringTool.SqlQ(sMastCust);
            }
            string strFullID = Equ_SubjectDP.GetSubjectFullID(long.Parse(sCalalogID));
            if ((sCalalogID == "1" || sCalalogID == "-1") && chkIncludeSub.Checked == true)
            {
            }
            else if (chkIncludeSub.Checked == true && strFullID.Length > 0)
            {
                sWhere += " And SUBSTR(FullID,1," + strFullID.Length.ToString() + ") = " + StringTool.SqlQ(strFullID);
            }
            else
            {
                sWhere += " And FullID = " + StringTool.SqlQ(strFullID);
            }
            dt = ee.GetEquStts(sWhere, sOrder);

            dgEquStatistics.DataSource = dt.DefaultView;
            dgEquStatistics.DataBind();
        }
        #endregion

        #region 导出excel  Master_Master_Button_ExportExcel_Click
        /// <summary>
        /// 
        /// </summary>
        protected void Master_Master_Button_ExportExcel_Click()
        {
            try
            {
                LoadData();
                ToExcel();
            }
            catch
            { 
                
            }
        }
        #endregion

        #region 导出到excel具体实现 ToExcel
        /// <summary>
        /// 
        /// </summary>
        private void ToExcel()
        {
            try
            {
                System.IO.StringWriter sw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new HtmlTextWriter(sw);

                hw.WriteFullBeginTag("html");
                hw.WriteLine();
                hw.WriteFullBeginTag("head");
                hw.WriteLine();
                hw.WriteLine("<meta http-equiv=Content-Type Content=text/html; charset=utf-8>");
                hw.WriteEndTag("head");
                hw.WriteLine();
                hw.WriteFullBeginTag("body");
                hw.WriteLine();

                hw.WriteLine("<table><tr><td align=\"center\"><font size=\"3\">资产统计表</font></td></tr></table>");
                this.dgEquStatistics.RenderControl(hw);

                hw.WriteEndTag("body");
                hw.WriteLine();
                hw.WriteEndTag("html");

                //输出到.xls文件
                Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("Content-Disposition","attachment; filename=服务事件趋势分析(事件类型).xls");  --会乱码
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("资产统计表", System.Text.Encoding.UTF8) + ".xls");

                Response.Charset = "utf-8";
                Response.Write(sw.ToString());
                Response.End();
            }
            catch 
            { 
                
            }
        }
        #endregion

        #region 初始化服务单位下拉列表的值
        /// <summary>
        /// 
        /// </summary>
        protected void InitddlMastCust()
        {
            Br_MastCustomerDP ee = new Br_MastCustomerDP();
            String sWhere = String.Empty;
            String sOrder = String.Empty;
            DataTable dt = ee.GetDataTable(sWhere, sOrder);
            ddlMastCust.DataSource = dt;
            ddlMastCust.DataTextField = "ShortName";
            ddlMastCust.DataValueField = "ID";
            ddlMastCust.DataBind();
            ddlMastCust.Items.Insert(0, new ListItem("全部", "0"));
        }
        #endregion

        #region 服务单位列表内容发生变化时 ddlMastCust_SelectedIndexChanged
        protected void ddlMastCust_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }
        #endregion

        protected void dgEquStatistics_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
            }
        }

        protected void dgEquStatistics_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            { 
                DataGrid dg = (DataGrid)sender;
                int j = 0;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i == 0 || i == 3)
                    {
                        j = i - 0;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",1);");
                    }
                    else
                    {
                        j = i - 0;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
    }
}
