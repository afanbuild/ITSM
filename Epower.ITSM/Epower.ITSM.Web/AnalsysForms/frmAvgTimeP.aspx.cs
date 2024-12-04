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
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.AnalsysForms
{
    /// <summary>
    /// frmAvgTimeP 的摘要说明。
    /// </summary>
    public partial class frmAvgTimeP : BasePage
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

        #region Page_Load
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            SetParentButtonEvent();
            if (!Page.IsPostBack)
            {
                ///设置起始日期
                string sQueryBeginDate = string.Empty;
                //sQueryBeginDate = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-01";
                sQueryBeginDate = (DateTime.Now.Year - 1).ToString() + "-" + DateTime.Now.Month.ToString() + "-01";
                ctrDateTime.BeginTime = DateTime.Parse(sQueryBeginDate).ToString("yyyy-MM-dd");
                ctrDateTime.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                DoBind();
                if (dpoFlows.Items.Count > 0)
                {
                    QueryResult();
                }
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

        #region DoBind
        /// <summary>
        /// 
        /// </summary>
        private void DoBind()
        {
            DataSet ds;
            if (System.Configuration.ConfigurationSettings.AppSettings["SystemModel"] == "1")
            {
                long lngOrgID = (long)Session["UserOrgID"];
                ds = FlowDP.GetFlowListForSelect(lngOrgID);
            }
            else
            {
                ds = FlowModel.GetFlowListForSelect();
            }
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                dpoFlows.Items.Add(new ListItem(ds.Tables[0].Rows[i]["FlowName"].ToString(), ds.Tables[0].Rows[i]["OFlowModelID"].ToString()));

            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                dpoFlows.SelectedIndex = 0;

            }
        }
        #endregion

        #region QueryResult
        /// <summary>
        /// 
        /// </summary>
        private void QueryResult()
        {
            long lngOID = 0;
            string strStart = "";
            string strEnd = "";
            strStart = ctrDateTime.BeginTime;
            strEnd = ctrDateTime.EndTime;
            lngOID = long.Parse(dpoFlows.SelectedValue);
            dgResult.DataSource = flowAnalysis.AnalysisAvgTimeForPerson(lngOID, strStart, strEnd);
            dgResult.DataBind();
        }
        #endregion

        #region dgResult_ItemDataBound
        protected void dgResult_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            int iTmp = 0;
            decimal dlTmp = 0;
            string sUnit = "工时";
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                switch (e.Item.Cells[4].Text.Trim())
                {
                    case "0":
                        sUnit = "工时";
                        break;
                    case "1":
                        sUnit = "工分";
                        break;
                    case "2":
                        sUnit = "工作日";
                        break;
                    case "3":
                        sUnit = "分钟";
                        break;
                    case "4":
                        sUnit = "小时";
                        break;
                    default:
                        break;
                }
                if (e.Item.Cells[0].Text.Trim() == "-1")
                {
                    e.Item.Cells[0].Text = "";
                }
                else
                {
                    if (e.Item.Cells[0].Text.Trim() == "1")
                    {
                        e.Item.Cells[0].Text = "当前版本";
                    }
                    else
                    {
                        iTmp = int.Parse(e.Item.Cells[0].Text.Trim()) - 1;
                        e.Item.Cells[0].Text = "前" + iTmp.ToString() + "版本";
                    }
                }
                dlTmp = Math.Round(decimal.Parse(e.Item.Cells[3].Text), 2);
                e.Item.Cells[3].Text = dlTmp.ToString() + sUnit;
            }
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

                hw.WriteLine("<table><tr><td><font size=\"3\">参与人员平均办理周期分析(工作时间) </font></td></tr></table>");
                this.dgResult.RenderControl(hw);


                hw.WriteEndTag("body");
                hw.WriteLine();
                hw.WriteEndTag("html");

                //输出到.xls文件
                Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("Content-Disposition","attachment; filename=服务事件趋势分析(事件类型).xls");  --会乱码
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("参与人员平均办理周期分析(工作时间)", System.Text.Encoding.UTF8) + ".xls");
                Response.Charset = "utf-8";
                Response.Write(tw.ToString());
                Response.End();
            }
            catch
            {
            }
        }
        #endregion

        #region dpoFlows_SelectedIndexChanged
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dpoFlows_SelectedIndexChanged(object sender, EventArgs e)
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
