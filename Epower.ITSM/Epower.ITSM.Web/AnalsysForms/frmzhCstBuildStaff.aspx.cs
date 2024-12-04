/****************************************************************************
 * 
 * description:人员工时统计
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-11-13
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
    public partial class frmzhCstBuildStaff : BasePage
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
            txtBeginDate.Attributes["onblur"] = "if(isDate(document.all." + txtBeginDate.ClientID + ".value) == false){document.all." + txtBeginDate.ClientID + ".value = '" + txtBeginDate.Text + "';return false }else{if(document.all." + txtBeginDate.ClientID + ".value != '" + txtBeginDate.Text + "')__doPostBack('datarange', '');};";
            txtEndDate.Attributes["onblur"] = "if(isDate(document.all." + txtEndDate.ClientID + ".value) == false){document.all." + txtEndDate.ClientID + ".value = '" + txtEndDate.Text + "';return false }else{if(document.all." + txtEndDate.ClientID + ".value != '" + txtEndDate.Text + "')__doPostBack('datarange', '');};";
            if (!IsPostBack)
            {
                //获取dbo控件的缺省值
                //设置起始日期
                string sQueryBeginDate = "0";
                if (CommonDP.GetConfigValue("Other", "QueryBeginDate") != null)
                    sQueryBeginDate = CommonDP.GetConfigValue("Other", "QueryBeginDate").ToString();
                if (sQueryBeginDate == "0")
                {
                    txtBeginDate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                }
                else
                {
                    txtBeginDate.Text = DateTime.Parse(sQueryBeginDate).ToString("yyyy-MM-dd");
                }
                txtEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                imgBeginDate.Attributes.Add("onclick", "fPopUpDlg('../Controls/Calendar/calendar.htm',document.all." + txtBeginDate.ClientID + ", 'winpop', 234, 261);__doPostBack('datarange', '')");
                imgEndDate.Attributes.Add("onclick", "fPopUpDlg('../Controls/Calendar/calendar.htm',document.all." + txtEndDate.ClientID + ", 'winpop', 234, 261);__doPostBack('datarange', '');");
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

        #region 选择楼栋时发生事件 dpdManageOffice_SelectedIndexChanged
        /// <summary>
        /// 选择楼栋时发生事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dpdManageOffice_SelectedIndexChanged(object sender, EventArgs e)
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
            string strBeginDate = txtBeginDate.Text.Trim();
            string strEndDate = txtEndDate.Text.Trim();
            long lngDeptID = DeptPicker1.DeptID;
            DataTable dt;
            dt = ZHServiceDP.GetCstBuildStaffStat(strBeginDate, strEndDate, lngDeptID);
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

                hw.WriteLine("<table><tr><td><font size=\"3\">材料费用统计分析</font></td></tr></table>");
                this.dgMaterialStat.RenderControl(hw);


                hw.WriteEndTag("body");
                hw.WriteLine();
                hw.WriteEndTag("html");

                //输出到.xls文件
                Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("Content-Disposition","attachment; filename=服务事件趋势分析(事件类型).xls");  --会乱码
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("材料费用统计分析", System.Text.Encoding.UTF8) + ".xls");
                Response.Charset = "utf-8";
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
