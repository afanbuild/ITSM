using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using EpowerCom;
using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.SqlDAL.Flash;

namespace Epower.ITSM.Web.AnalsysForms
{
    /// <summary>
    /// frmAnalsisWorkLoad 的摘要说明。
    /// </summary>
    public partial class frmAnalsisWorkLoad : BasePage
    {
        #region 设置父窗体按钮事件SetParentButtonEvent
        /// <summary>
        /// 设置父窗体按钮事件

        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_ExportExcel_Click += new Global_BtnClick(btnToExcel_Click);
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.ShowQueryButton(false);
            this.Master.ShowExportExcelButton(true);
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
            //txtProcessBegin.Attributes["onblur"] = "if(isDate(document.all." + txtProcessBegin.ClientID + ".value) == false){document.all." + txtProcessBegin.ClientID + ".value = '" + txtProcessBegin.Text + "';return false }else{if(document.all." + txtProcessBegin.ClientID + ".value != '" + txtProcessBegin.Text + "')__doPostBack('datarange', '');};";
            //txtProcessEnd.Attributes["onblur"] = "if(isDate(document.all." + txtProcessEnd.ClientID + ".value) == false){document.all." + txtProcessEnd.ClientID + ".value = '" + txtProcessEnd.Text + "';return false }else{if(document.all." + txtProcessEnd.ClientID + ".value != '" + txtProcessEnd.Text + "')__doPostBack('datarange', '');};";          

            if (!Page.IsPostBack)
            {
                ///设置起始日期
                string sQueryBeginDate = string.Empty;
                //sQueryBeginDate = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-01";
                sQueryBeginDate = (DateTime.Now.Year - 1).ToString() + "-" + DateTime.Now.Month.ToString() + "-01";
                ctrDateTime.BeginTime = DateTime.Parse(sQueryBeginDate).ToString("yyyy-MM-dd");
                ctrDateTime.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                UserPicker1.UserID = long.Parse(Session["UserID"].ToString());
                UserPicker1.UserName = Session["PersonName"].ToString();
                QueryResult();
            }
            else
            {
                if (Request.Form["__EVENTTARGET"] == "datarange")
                {
                    QueryResult();
                }
            }
        }
        #endregion

        #region QueryResult
        /// <summary>
        /// 
        /// </summary>
        private void QueryResult()
        {
            long lngID = 0;
            DateTime dtStart = DateTime.MinValue;
            DateTime dtEnd = DateTime.MinValue;

            if (ctrDateTime.BeginTime != "")
                dtStart = DateTime.Parse(ctrDateTime.BeginTime);
            if (ctrDateTime.EndTime != "")
                dtEnd = DateTime.Parse(ctrDateTime.EndTime);

            if (UserPicker1.UserName.Trim() != string.Empty)
            {
                lngID = UserPicker1.UserID;
            }
            else
            {
                Epower.DevBase.BaseTools.PageTool.MsgBox(this, "请选择用户！");
                return;
            }
            DataTable table = flowAnalysis.AnalysisWorkLoad(lngID, dtStart, dtEnd).Tables[0];
            dgResult.DataSource = table.DataSet;
            dgResult.DataBind();



            if (table.Rows.Count > 0)
            {
                object totalCount = table.Compute("Sum(TotalCount)", "");
                object fCount = table.Compute("Sum(FCount)", "");
                object cCount = table.Compute("Sum(CCount)", "");
                object uCount = table.Compute("Sum(UCount)", "");
                object overTimeCount = table.Compute("Sum(OverTime)", "");

                DataTable tableSum = new DataTable("Sum");

                DataColumn Col1 = new DataColumn("任务", typeof(string));
                DataColumn Col2 = new DataColumn("数量", typeof(int));

                tableSum.Columns.Add(Col1);
                tableSum.Columns.Add(Col2);

                tableSum.Rows.Add(new Object[] { "总任务", int.Parse(totalCount.ToString()) });
                tableSum.Rows.Add(new Object[] { "按时完成", int.Parse(fCount.ToString()) });
                tableSum.Rows.Add(new Object[] { "超时完成", int.Parse(cCount.ToString()) });
                tableSum.Rows.Add(new Object[] { "未完成", int.Parse(uCount.ToString()) });
                tableSum.Rows.Add(new Object[] { "平均超时", int.Parse(overTimeCount.ToString()) });

                ReportDiv.InnerHtml = FlashCS.PublicFlashUrl2D(tableSum, "任务", "数量", "工作量及状态分析", "", "", "../FlashReoport/Flash/Column3D.swf", "100%", "248", true, 2);

            }
        }
        #endregion

        #region dgResult_ItemDataBound
        protected void dgResult_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            decimal dlTmp = 0;
            int iOverLimit = 10;
            iOverLimit = int.Parse(CommonDP.GetConfigValue("WorkTimeType", "OverTimeLimit"));
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                dlTmp = Math.Round(decimal.Parse(e.Item.Cells[5].Text) / 60, 2);
                if (dlTmp >= iOverLimit)
                {
                    e.Item.Cells[5].ForeColor = Color.Red;
                }
                e.Item.Cells[5].Text = dlTmp.ToString() + "工时";
            }
        }
        #endregion

        #region btnToExcel_Click
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnToExcel_Click()
        {
            try
            {
                this.QueryResult();
                this.ToExcel();
            }
            catch { }
        }
        #endregion

        #region ToExcel
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

                hw.WriteLine("<table><tr><td><font size=\"3\">工作量及状态分析</font></td></tr></table>");
                this.dgResult.RenderControl(hw);
                //this.UltraChart1.RenderControl(hw);

                hw.WriteEndTag("body");
                hw.WriteLine();
                hw.WriteEndTag("html");

                //输出到.xls文件
                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("工作量及状态分析", System.Text.Encoding.UTF8) + ".xls");
                Response.Charset = "utf-8";
                Response.Write(tw.ToString());
                Response.End();
            }
            catch
            {
            }
        }
        #endregion

        #region 查询事件Master_Master_Button_Query_Click
        /// <summary>
        /// 查询事件
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            QueryResult();
        }
        #endregion

        #region
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOk_Click(object sender, EventArgs e)
        {
            QueryResult();
        }
        #endregion

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            QueryResult();
        }
    }
}
