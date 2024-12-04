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
using Epower.ITSM.SqlDAL.Flash;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.AnalsysForms
{
    public partial class ChangeAcceptedNumCounter : BasePage
    {
        #region 设置父窗体按钮事件SetParentButtonEvent
        /// <summary>
        /// 设置父窗体按钮事件
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.EquChangeReport;
            this.Master.IsCheckRight = true;
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_ExportExcel_Click += new Global_BtnClick(btnToExcel_Click);
            this.Master.ShowQueryButton(true);
            this.Master.ShowExportExcelButton(true);
            this.Master.MainID = "1";     //设置页面的ID编号，如果为查询页面，则设置为1
        }

        #endregion

        void Master_Master_Button_Query_Click()
        {
            LoadData();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtonEvent();
            if (!IsPostBack)
            {
                DeptPicker1.Right = this.Master.OperatorID;
                //设置起始日期
                string sQueryBeginDate = string.Empty;
                //sQueryBeginDate = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-01";
                sQueryBeginDate = (DateTime.Now.Year - 1).ToString() + "-" + DateTime.Now.Month.ToString() + "-01";
                ctrDateTime.BeginTime = DateTime.Parse(sQueryBeginDate).ToString("yyyy-MM-dd");
                ctrDateTime.EndTime = DateTime.Now.ToString("yyyy-MM-dd");

            }

            LoadData();
        }

        private void LoadData()
        {

            DataTable dt;

            #region 基本查询条件
            int type = 0;
            if (DropDownList1.SelectedItem != null)
                type = int.Parse(DropDownList1.SelectedItem.Value);

            int GroupType = 0;
            if (chkGroupType.Checked)
                GroupType = 1;

            string strBeginDate = ctrDateTime.BeginTime;
            string strEndDate = ctrDateTime.EndTime;

            string sWhere = "";

            if (DeptPicker1.DeptID > 0)
            {
                if (chkIncludeSub.Checked)
                {
                    string FullID = "";
                    DataTable FullIDDT = CommonDP.ExcuteSqlTable("SELECT FullID from Ts_Dept where DeptID = " + DeptPicker1.DeptID);
                    if (FullIDDT.Rows.Count == 1)
                    {
                        FullID = FullIDDT.Rows[0][0].ToString();
                    }
                    sWhere += " AND RegDeptID IN (SELECT DeptID FROM Ts_Dept WHERE FullID LIKE '" + FullID + "%')";
                }
                else
                {
                    sWhere += " AND RegDeptID = " + DeptPicker1.DeptID.ToString();
                }
            }

            if (strBeginDate.Trim() != "")
            {

                sWhere += " And RegTime>=to_date(" + StringTool.SqlQ(strBeginDate) + ",'yyyy-MM-dd HH24:mi:ss')";
            }
            if (strEndDate.Trim() != "")
            {
                sWhere += " And RegTime <=to_date(" + StringTool.SqlQ(DateTime.Parse(strEndDate).ToString("yyyy-MM-dd") + " 23:59:59") + ",'yyyy-MM-dd HH24:mi:ss')";
            }
            #endregion

            #region 判断权限

            string strList = "";

            if (this.Master.RightEntity.CanRead == false)
            {
                //查询出空结果
                sWhere += " AND a.RegUserID = -1 ";
            }
            else
            {
                #region 范围条件
                switch (this.Master.RightEntity.RightRange)
                {
                    case eO_RightRange.eFull:
                        sWhere += "";
                        break;
                    case eO_RightRange.ePersonal:
                        sWhere += "AND RegUserID = " + Session["UserID"].ToString();
                        break;
                    case eO_RightRange.eDeptDirect:
                        sWhere += "AND RegDeptID = " + Session["UserDeptID"].ToString();
                        break;
                    case eO_RightRange.eOrgDirect:
                        sWhere += "AND RegOrgID  = " + Session["UserOrgID"].ToString();
                        break;
                    case eO_RightRange.eDept:
                        strList = DeptDP.GetDeptFullID(long.Parse(Session["UserDeptID"].ToString()));
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到

                            sWhere += "AND RegDeptID in (select deptid from ts_dept where fullid like '" + strList + "%')";
                        }
                        break;
                    case eO_RightRange.eOrg:
                        strList = DeptDP.GetDeptFullID(long.Parse(Session["UserOrgID"].ToString()));
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到

                            sWhere += "AND RegOrgID in (select distinct deptid from ts_dept where deptkind = 1 and fullid like '" + strList + "%')";
                        }
                        break;
                    default:
                        sWhere += "";
                        break;
                }
                #endregion
            }
            #endregion

            dt = ChangeDealDP.GetCHANGEACCPTED(GroupType, type, sWhere);

            if (!chkGroupType.Checked)
            {
                ReportDiv.InnerHtml = FlashCS.PublicFlashUrl2D(dt, "CountName", "CountNum", "变更受理数量统计表", "", "", "../FlashReoport/Flash/Column3D.swf", "100%", "248", true, 2);

                dgTypesCount2.Visible = false;
                dgTypesCount.Visible = true;
                dgTypesCount.DataSource = dt.DefaultView;
                dgTypesCount.DataBind();                
            }
            else
            {
                ReportDiv.InnerHtml = FlashCS.MSPublicFlashUrl3D(dt, "changetypename", "CountName", "CountNum", "变更受理数量统计表", "数量", "区间", "../FlashReoport/Flash/MSColumn3D.swf", "100%", "248", true, 2);

                dgTypesCount.Visible = false;
                dgTypesCount2.Visible = true;
                dgTypesCount2.DataSource = dt.DefaultView;
                dgTypesCount2.DataBind();
            }
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

                hw.WriteLine("<table><tr><td><font size=\"3\">变更受理数量统计</font></td></tr></table>");
                if (this.dgTypesCount.Visible == true)
                    this.dgTypesCount.RenderControl(hw);
                if (this.dgTypesCount2.Visible == true)
                    this.dgTypesCount2.RenderControl(hw);
                hw.WriteEndTag("body");
                hw.WriteLine();
                hw.WriteEndTag("html");

                //输出到.xls文件
                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("变更受理数量统计", System.Text.Encoding.UTF8) + ".xls");
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

        protected void dpdMastCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
