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

namespace Epower.ITSM.Web.AnalsysForms
{
    public partial class frmfirstRatio : BasePage
    {
        int nLastYear = 0;
        //上次查询时选择的年份,优化用,避免不必要的查询

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (Request["Type"] != null)
                this.Master.TableVisible = false;
           
            SetParentButtonEvent();
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
                if (ViewState["LastYear"] != null)
                {
                    nLastYear = (int)ViewState["LastYear"];
                }
                if (Request.Form["__EVENTTARGET"] == "datarange")
                {
                    LoadData();
                }
            }
        }

        #region 设置父窗体按钮事件SetParentButtonEvent
        /// <summary>
        /// 设置父窗体按钮事件


        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);            
            this.Master.ShowQueryButton(true);
        }

        void Master_Master_Button_Query_Click()
        {   
            LoadData();
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

        private void LoadData()
        {
            if (string.IsNullOrEmpty(ctrDateTime.BeginTime) || string.IsNullOrEmpty(ctrDateTime.EndTime))
            {
                Epower.DevBase.BaseTools.PageTool.MsgBox(this.Page, "请输入查询的日期.");
                return ;
            } 
            DataTable dt = ReportDP.GetFirstSolve(DateTime.Parse(ctrDateTime.BeginTime),
                                        DateTime.Parse(ctrDateTime.EndTime).AddDays(1).AddMilliseconds(-1),
                                        int.Parse(dpdMastShortName.SelectedValue));
            //ZHServiceDP.GetFirstRatio(strBeginDate, strEndDate, long.Parse(dpdMastShortName.SelectedValue));
            ReportDiv.InnerHtml = FlashCS.PublicFlashUrl2D(dt, "首次解决", "数量", "首次解决分析", "", "", "../FlashReoport/Flash/Pie2D.swf", "100%", "248", true, 2);
        }

        /// <summary>
        /// 
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

                hw.WriteLine("<table><tr><td><font size=\"3\">首次解决分析</font></td></tr></table>");
                //this.charEfficiency.RenderControl(hw);
              //  this.tdResultChart.RenderControl(hw);


                hw.WriteEndTag("body");
                hw.WriteLine();
                hw.WriteEndTag("html");

                //输出到.xls文件
                Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("Content-Disposition","attachment; filename=服务事件趋势分析(事件类型).xls");  --会乱码
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("首次解决率分析", System.Text.Encoding.UTF8) + ".xls");
                Response.Charset = "utf-8";
                Response.Write(tw.ToString());
                Response.End();
            }
            catch
            {
            }
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
