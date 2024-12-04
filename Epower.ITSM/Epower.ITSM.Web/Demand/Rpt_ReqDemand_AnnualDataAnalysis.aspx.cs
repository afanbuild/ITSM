/*******************************************************************
 * 
 * 版权所有：深圳市非凡信息技术有限公司
 * 描述："需求管理" - 年度数据分析报表
 * 
 * 
 * 创建人：孙绍棕
 * 创建日期：2013-05-03
 * 
 * 修改日志：
 * 修改时间：2013-05-03 修改人：孙绍棕
 * 修改描述：
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
using System.Drawing;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL.Flash;
using Epower.ITSM.SqlDAL.Demand;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;

namespace Epower.ITSM.Web.Demand
{
    public partial class Rpt_ReqDemand_AnnualDataAnalysis : BasePage
    {
        int nLastYear = 0;
        //上次查询时选择的年份,优化用,避免不必要的查询

        #region 设置父窗体按钮事件SetParentButtonEvent
        /// <summary>
        /// 设置父窗体按钮事件

        /// </summary>
        protected void SetParentButtonEvent()
        {
            RightEntity reReqDemand = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.ReqDemandRptAnnualDataAnalysis];
            if (!reReqDemand.CanRead)
            {
                throw new Exception("没有权限.");
            }

            this.Master.Master_Button_ExportExcel_Click += new Global_BtnClick(btnToExcel_Click);
            this.Master.ShowExportExcelButton(true);
            this.Master.MainID = "1";     //设置页面的ID编号，如果为查询页面，则设置为1
        }


        #endregion

        protected void Page_Load(object sender, System.EventArgs e)
        {
            SetParentButtonEvent();
            this.ctrFCDServiceType.mySelectedIndexChanged += new EventHandler(ctrFCDServiceType_mySelectedIndexChanged);
            if (!IsPostBack)
            {
                //获取dbo控件的缺省值


                SetYearSelectDboValue();

                SetManageOfficeDboValue();

                SetMastCustomDboValue();

                LoadData();
            }
            else
            {
                if (ViewState["LastYear"] != null)
                {
                    nLastYear = (int)ViewState["LastYear"];
                }
            }
        }

        void ctrFCDServiceType_mySelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void dpdYear_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            LoadData();
        }



        protected void dpdManageOffice_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void dpdMastCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void SetYearSelectDboValue()
        {
            ReqDemandDP reqDemandDP = new ReqDemandDP();
            DataTable dt = reqDemandDP.GetReqDemandYears();
            DataView dv = new DataView(dt);
            // dv.Sort = "years";

            dpdYear.DataSource = dv;
            dpdYear.DataTextField = "years";
            dpdYear.DataValueField = "years";
            dpdYear.DataBind();

            dt.Dispose();
        }


        private void SetManageOfficeDboValue()
        {
            ReqDemandDP reqDemandDP = new ReqDemandDP();
            DataTable dt = reqDemandDP.GetManageOffices();
            DataView dv = new DataView(dt);
            dv.Sort = "deptid";

            dpdManageOffice.DataSource = dv;
            dpdManageOffice.DataTextField = "deptname";
            dpdManageOffice.DataValueField = "deptid";
            dpdManageOffice.DataBind();

            dpdManageOffice.Items.Insert(0, new ListItem("全部", "0"));

            dt.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetMastCustomDboValue()
        {
            DataTable dt = ZHServiceDP.GetMastCustomer();
            DataView dv = new DataView(dt);
            dv.Sort = "ID";
            dpdMastCustomer.DataSource = dv;
            dpdMastCustomer.DataTextField = "ShortName";
            dpdMastCustomer.DataValueField = "ID";
            dpdMastCustomer.DataBind();

            dpdMastCustomer.Items.Insert(0, new ListItem("全部", "0"));

            dt.Dispose();
        }

        private void LoadData()
        {

            int nYear = Epower.DevBase.BaseTools.StringTool.String2Int(dpdYear.SelectedValue);
            long lngServiceTypeID = (ctrFCDServiceType.CatelogID == ctrFCDServiceType.RootID) ? 0 : ctrFCDServiceType.CatelogID;

            long lngDeptID = long.Parse(dpdManageOffice.SelectedValue);

            string strManageOffic = dpdManageOffice.SelectedItem.Text;

            Font f = new Font("宋体", 9);

            DataTable dt;
            if (nLastYear != nYear)
            {
                nLastYear = nYear;
                ViewState["LastYear"] = nYear;
            }

            ReqDemandDP reqDemandDP = new ReqDemandDP();

            dt = reqDemandDP.GetAnalysisDirection(nYear, lngServiceTypeID, lngDeptID, long.Parse(dpdMastCustomer.SelectedValue));

            ReportDiv.InnerHtml = FlashCS.MSPublicFlashUrl3D(dt, "月份", "年度数据分析报表", "数量", "月份", "../FlashReoport/Flash/MSLine.swf", "100%", "248", true, 0);

            dgTypesCount.DataSource = dt.DefaultView;
            dgTypesCount.DataBind();


        }



        protected void btnToExcel_Click()
        {
            try
            {
                LoadData(); ;
                ToExcel();
            }
            catch { }
        }


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

                hw.WriteLine("<table><tr><td><font size=\"3\">年度数据分析报表</font></td></tr></table>");
                this.dgTypesCount.RenderControl(hw);
                // this.tdResultChart.RenderControl(hw);


                hw.WriteEndTag("body");
                hw.WriteLine();
                hw.WriteEndTag("html");

                //输出到.xls文件
                Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("Content-Disposition","attachment; filename=服务事件趋势分析(事件类型).xls");  --会乱码
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("年度数据分析报表", System.Text.Encoding.UTF8) + ".xls");
                Response.Charset = "utf-8";
                Response.Write(tw.ToString());
                Response.End();
            }
            catch
            {
            }
        }

    }
}
