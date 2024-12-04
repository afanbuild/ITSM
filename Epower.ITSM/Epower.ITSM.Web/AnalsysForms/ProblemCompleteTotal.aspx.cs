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
using Epower.DevBase.BaseTools;
using System.Drawing;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.AnalsysForms
{
    public partial class ProblemCompleteTotal : BasePage
    {
        #region 设置父窗体按钮事件SetParentButtonEvent
        /// <summary>
        /// 设置父窗体按钮事件
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.ProblemReport;
            this.Master.IsCheckRight = true;
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_ExportExcel_Click += new Global_BtnClick(btnToExcel_Click);
            this.Master.ShowQueryButton(true);
            this.Master.ShowExportExcelButton(true);
            this.Master.MainID = "1";     //设置页面的ID编号，如果为查询页面，则设置为1
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtonEvent();
            this.CataProblemType.mySelectedIndexChanged += new EventHandler(CataProblemType_mySelectedIndexChanged);
            this.CtrDealState.mySelectedIndexChanged += new EventHandler(CtrDealState_mySelectedIndexChanged);

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

        void Master_Master_Button_Query_Click()
        {
            LoadData();
        }

        private void LoadData()
        {
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
                    sWhere += " AND a.RegDeptID IN (SELECT DeptID FROM Ts_Dept WHERE FullID LIKE '" + FullID + "%')";
                }
                else
                {
                    sWhere += " AND a.RegDeptID = " + DeptPicker1.DeptID.ToString();
                }
            }

            if (strBeginDate.Trim() != "")
                sWhere += " And RegTime>=to_date(" + StringTool.SqlQ(strBeginDate) + ",'yyyy-MM-dd HH24:mi:ss')";
            if (strEndDate.Trim() != "")
                sWhere += " And RegTime <=to_date(" + StringTool.SqlQ(DateTime.Parse(strEndDate).ToString("yyyy-MM-dd") + " 23:59:59") + ",'yyyy-MM-dd HH24:mi:ss')";

            if (CataProblemType.CatelogValue.Trim()!="")
            {
                sWhere += "and Problem_Type=" + CataProblemType.CatelogID;
            }

            if (CtrDealState.CatelogID > 0)
            {
                sWhere += "and State=" + CtrDealState.CatelogID;
            }

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
                        sWhere += "AND a.RegUserID = " + Session["UserID"].ToString();
                        break;
                    case eO_RightRange.eDeptDirect:
                        sWhere += "AND a.RegDeptID = " + Session["UserDeptID"].ToString();
                        break;
                    case eO_RightRange.eOrgDirect:
                        sWhere += "AND a.RegOrgID  = " + Session["UserOrgID"].ToString();
                        break;
                    case eO_RightRange.eDept:
                        strList = DeptDP.GetDeptFullID(long.Parse(Session["UserDeptID"].ToString()));
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            sWhere += "AND a.RegDeptID in (select deptid from ts_dept where fullid like '" + strList + "%')";
                        }
                        break;
                    case eO_RightRange.eOrg:
                        strList = DeptDP.GetDeptFullID(long.Parse(Session["UserOrgID"].ToString()));
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            sWhere += "AND a.RegOrgID in (select distinct deptid from ts_dept where deptkind = 1 and fullid like '" + strList + "%')";
                        }
                        break;
                    default:
                        sWhere += "";
                        break;
                }
                #endregion
            }
            #endregion

            DataTable dt;


            dt = ProblemDealDP.GetProblemCompleteTotal(sWhere, "");

            ReportDiv.InnerHtml = FlashCS.PublicFlashUrl2D(dt, "StateName", "CountNumb", "问题按完成总数统计", "", "", "../FlashReoport/Flash/Pie2D.swf", "100%", "248", true, 2);

            #region 给页面展示增加合计 2013-06-04 余向前
            int intCount = 0;

            foreach (DataRow drin in dt.Rows)
            {
                intCount += Convert.ToInt32(drin["CountNumb"].ToString());
            }

            DataRow dr = dt.NewRow();
            dr["StateName"] = "合计";
            dr["CountNumb"] = intCount;

            dt.Rows.Add(dr);
            #endregion

            dgTypesCount.DataSource = dt.DefaultView;
            dgTypesCount.DataBind();
        }

        /// <summary>
        /// 
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

                hw.WriteLine("<table><tr><td><font size=\"3\">问题按完成总数统计</font></td></tr></table>");
                this.dgTypesCount.RenderControl(hw);
                hw.WriteEndTag("body");
                hw.WriteLine();
                hw.WriteEndTag("html");

                //输出到.xls文件
                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("问题按完成总数统计", System.Text.Encoding.UTF8) + ".xls");
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

        protected void CtrDealState_mySelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        void CataProblemType_mySelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
