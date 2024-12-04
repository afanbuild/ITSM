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
using System.Globalization;
using System.Threading;

namespace Epower.ITSM.Web.AnalsysForms
{
    public partial class SLAMark : BasePage
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
                //获取dbo控件的缺省值
                SetMastCustomDboValue();
                //设置起始日期
                string sQueryBeginDate = "0";
                if (CommonDP.GetConfigValue("Other", "ReportBeginDate") != null)
                    sQueryBeginDate = CommonDP.GetConfigValue("Other", "ReportBeginDate").ToString();

                #region 由当前年的第一个月的第一天，改成当前年的当前月的第一天
                int year = DateTime.Now.Year;
                int month = DateTime.Now.Month;
                string begindate = year + "-" + month + "-" + "1";
                ctrDateTime.BeginTime = begindate;
                #endregion

                ctrDateTime.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
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
            long lngMastCustID = long.Parse(dpdMastShortName.SelectedValue.Trim());

            DataTable dt;
            dt = ZHServiceDP.SLAMark(strBeginDate, strEndDate, lngStatusID, lngMastCustID, "proc_compliancerate");
            dt = getDataRow(dt, "SERVICELEVEL");
            dgMaterialStat.DataSource = dt.DefaultView;
            dgMaterialStat.DataBind();

            DataTable dt1;
            dt1 = ZHServiceDP.SLAMark(strBeginDate, strEndDate, lngStatusID, lngMastCustID, "proc_compliancerate2");
            dt1 = getDataRow(dt1, "SERVICELEVEL");
            dgMaterialStat2.DataSource = dt1.DefaultView;
            dgMaterialStat2.DataBind();
            StatFootSum();
        }

        /// <summary>
        /// DataTable中某一中文列排序
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="strColumnName">DataTable中要排序的列名</param>
        public DataTable getDataRow(DataTable dt, string strColumnName)
        {
            string[] arr = new string[dt.Rows.Count];
            CultureInfo StrokCi = new CultureInfo(133124);
            Thread.CurrentThread.CurrentCulture = StrokCi;
            for (int d = 0; d < dt.Rows.Count; d++)
            {
                arr[d] = dt.Rows[d][strColumnName].ToString();
            }
            Array.Sort(arr);

            DataTable dtNew = dt.Clone();

            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    if (dt.Rows[j][strColumnName].ToString() == arr.GetValue(i).ToString())
                    {
                        dtNew.Rows.Add(dt.Rows[j].ItemArray);
                        break;
                    }
                }

            }
            return dtNew;

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
                LoadData();
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

                hw.WriteLine("<table><tr><td><font size=\"3\">SLA达标率</font></td></tr></table>");
                this.dgMaterialStat.RenderControl(hw);


                hw.WriteEndTag("body");
                hw.WriteLine();
                hw.WriteEndTag("html");

                //输出到.xls文件
                Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("Content-Disposition","attachment; filename=服务事件趋势分析(事件类型).xls");  --会乱码
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("SLA达标率", System.Text.Encoding.UTF8) + ".xls");
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

        /// <summary>
        /// 
        /// </summary>
        private void StatFootSum()
        {
            double icount = 0;
            double iRespond = 0;
            foreach (DataGridItem row in dgMaterialStat.Controls[0].Controls)
            {
                if (row.ItemType == ListItemType.Footer)  //如果为合计的话
                {
                    ((Label)row.FindControl("lblicountFoot")).Text = icount.ToString();
                    ((Label)row.FindControl("lbliRespondFoot")).Text = iRespond.ToString();
                    double dResult = (iRespond / icount) * 100;
                    ((Label)row.FindControl("lblirateFoot")).Text = dResult.ToString("0.0000");//ToString("n");
                }
                else if (row.ItemType == ListItemType.Item || row.ItemType == ListItemType.AlternatingItem)
                {
                    icount += double.Parse(((Label)row.FindControl("lblicount")).Text);
                    iRespond += double.Parse(((Label)row.FindControl("lbliRespond")).Text);
                }
            }

            double icount1 = 0;
            double iRespond1 = 0;
            foreach (DataGridItem row in dgMaterialStat2.Controls[0].Controls)
            {
                if (row.ItemType == ListItemType.Footer)  //如果为合计的话
                {
                    ((Label)row.FindControl("lblicountFoot")).Text = icount1.ToString();
                    ((Label)row.FindControl("lbliRespondFoot")).Text = iRespond1.ToString();
                    double dResult = (iRespond1 / icount1) * 100;
                    ((Label)row.FindControl("lblirateFoot")).Text = dResult.ToString("0.0000");//ToString("n");
                }
                else if (row.ItemType == ListItemType.Item || row.ItemType == ListItemType.AlternatingItem)
                {
                    icount1 += double.Parse(((Label)row.FindControl("lblicount")).Text);
                    iRespond1 += double.Parse(((Label)row.FindControl("lbliRespond")).Text);
                }
            }
        }
        protected void btnSelect_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
