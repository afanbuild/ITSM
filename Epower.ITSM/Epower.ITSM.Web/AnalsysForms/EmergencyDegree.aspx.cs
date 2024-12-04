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
using Epower.ITSM.Base;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;
using System.Collections.Generic;

namespace Epower.ITSM.Web.AnalsysForms
{
    public partial class EmergencyDegree : BasePage
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
            this.CtrFCDInstancy.mySelectedIndexChanged += new EventHandler(CtrFCDInstancy_mySelectedIndexChanged);
            if (!IsPostBack)
            {
                DeptPicker1.Right = this.Master.OperatorID;
                //设置起始日期
                string sQueryBeginDate = string.Empty;
                sQueryBeginDate = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-01";
                ctrDateTime.BeginTime = DateTime.Parse(sQueryBeginDate).ToString("yyyy-MM-dd");
                ctrDateTime.EndTime = DateTime.Now.ToString("yyyy-MM-dd");

            }

            LoadData();
        }

        private void LoadData()
        {

            DataTable dt = null;

            string pColum = CtrFCDInstancy.CatelogValue == "" ? "变更占比" : CtrFCDInstancy.CatelogValue;

            #region 基本查询条件
            int type = 0;
            if (DropDownList1.SelectedItem != null)
                type = int.Parse(DropDownList1.SelectedItem.Value);

            string strBeginDate = ctrDateTime.BeginTime;
            string strEndDate = ctrDateTime.EndTime;

            string sWhere = "";
            string sWhere2 = "";

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
                    sWhere2 += " AND RegDeptID IN (SELECT DeptID FROM Ts_Dept WHERE FullID LIKE '" + FullID + "%')";
                }
                else
                {
                    sWhere += " AND RegDeptID = " + DeptPicker1.DeptID.ToString();
                    sWhere2 += " AND RegDeptID = " + DeptPicker1.DeptID.ToString();
                }
            }

            if (CtrFCDInstancy.CatelogID > 0)
            {
                sWhere += " And InstancyID = " + CtrFCDInstancy.CatelogID;
            }

            if (strBeginDate.Trim() != "")
            {

                sWhere += " And RegTime>=to_date(" + StringTool.SqlQ(strBeginDate) + ",'yyyy-MM-dd HH24:mi:ss')";
                sWhere2 += " And RegTime>=to_date(" + StringTool.SqlQ(strBeginDate) + ",'yyyy-MM-dd HH24:mi:ss')";
            }
            if (strEndDate.Trim() != "")
            {
                sWhere += " And RegTime <=to_date(" + StringTool.SqlQ(DateTime.Parse(strEndDate).ToString("yyyy-MM-dd") + " 23:59:59") + ",'yyyy-MM-dd HH24:mi:ss')";
                sWhere2 += " And RegTime <=to_date(" + StringTool.SqlQ(DateTime.Parse(strEndDate).ToString("yyyy-MM-dd") + " 23:59:59") + ",'yyyy-MM-dd HH24:mi:ss')";
            }
            #endregion

            #region 判断权限

            string strList = "";

            if (this.Master.RightEntity.CanRead == false)
            {
                //查询出空结果
                sWhere += " AND RegUserID = -1 ";
                sWhere2 += " AND RegUserID = -1 ";
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
                        sWhere2 += "AND RegUserID = " + Session["UserID"].ToString();
                        break;
                    case eO_RightRange.eDeptDirect:
                        sWhere += "AND RegDeptID = " + Session["UserDeptID"].ToString();
                        sWhere2 += "AND RegDeptID = " + Session["UserDeptID"].ToString();
                        break;
                    case eO_RightRange.eOrgDirect:
                        sWhere += "AND RegOrgID  = " + Session["UserOrgID"].ToString();
                        sWhere2 += "AND RegOrgID = " + Session["RegOrgID"].ToString();
                        break;
                    case eO_RightRange.eDept:
                        strList = DeptDP.GetDeptFullID(long.Parse(Session["UserDeptID"].ToString()));
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到

                            sWhere += "AND RegDeptID in (select deptid from ts_dept where fullid like '" + strList + "%')";
                            sWhere2 += "AND RegDeptID in (select deptid from ts_dept where fullid like '" + strList + "%')";
                        }
                        break;
                    case eO_RightRange.eOrg:
                        strList = DeptDP.GetDeptFullID(long.Parse(Session["UserOrgID"].ToString()));
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到

                            sWhere += "AND RegOrgID in (select distinct deptid from ts_dept where deptkind = 1 and fullid like '" + strList + "%')";
                            sWhere2 += "AND RegOrgID in (select distinct deptid from ts_dept where deptkind = 1 and fullid like '" + strList + "%')";
                        }
                        break;
                    default:
                        sWhere += "";
                        break;
                }
                #endregion
            }
            #endregion

            dt = ChangeDealDP.GetEqu_ChangeService(type, StringTool.SqlQ(pColum), sWhere, sWhere2);

            #region 重新排序查询结果 - 2013-05-02 @孙绍棕

            List<Object[]> listRow = new List<object[]>();

            foreach (DataRow item in dt.Rows)
            {
                listRow.Add(item.ItemArray);
            }

            /*
             *  ##备注##
             *  
             *  查询出的 dt 中的数据是无序的, 没有按照区间的先后顺序排序.
             *  所以在此: 我首先将 dt 拆分成列表, 然后对列表进行排序. 最后再组装成 dt. 
             * 
             * 之所以要拆分成列表排序, 是因为 dt 中的区间列的数据是类似如下的值:
             * 
             *      2013.4.8~2013.4.14
             * 
             * 在 dt 中实在无法将之排序. 故在列表的排序方法( PROC_Equ_ChangeServiceSorter )中, 
             * 首先将截取该值得前部分,  然后转换成日期类型, 最后进行比较.
             * 
             * 当然, 我们的比较是按值的前半部分. 即 2013.4.8~2013.4.14 中的 2013.4.8.
             * 
             */

            listRow.Sort(new PROC_Equ_ChangeServiceSorter());

            dt = dt.Clone();
            dt.Clear();

            foreach (object[] item in listRow)
            {
                dt.Rows.Add(item);
            }

            #endregion

            ReportDiv.InnerHtml = FlashCS.MSPublicFlashUrl3DRate(dt, "nLevel", "CountName", "rate", "变更紧急度占比", "百分比", "区间", "../FlashReoport/Flash/MSLine.swf", "100%", "248", true, 2);

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

                hw.WriteLine("<table><tr><td><font size=\"3\">变更紧急度占比</font></td></tr></table>");
                this.dgTypesCount.RenderControl(hw);
                hw.WriteEndTag("body");
                hw.WriteLine();
                hw.WriteEndTag("html");

                //输出到.xls文件
                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("变更紧急度占比", System.Text.Encoding.UTF8) + ".xls");
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

        void CtrFCDInstancy_mySelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }
    }

    #region 对查询进行再排序 - 2013-05-02 @孙绍棕

    /// <summary>
    /// 对查询进行再排序
    /// </summary>
    public class PROC_Equ_ChangeServiceSorter : IComparer<Object[]>
    {
        /*
         * 详细说明, 见 > 重新排序查询结果 - 2013-05-02 @孙绍棕 < 中的注释说明.
         */
        #region IComparer<object[]> 成员

        public int Compare(object[] x, object[] y)
        {
            /*
             * 要使用该排序方法, 其 X[1] 和 y[1] 中的值必须是如:
             *      2013.4.8~2013.4.14
             * 这样的值.
             * **/

            String strFirstTimeRange = x[1].ToString();
            String strSecondTimeRange = y[1].ToString();

            String strFirstTimeHead = strFirstTimeRange.Substring(0, strFirstTimeRange.IndexOf("~"));
            String strSecondTimeHead = strSecondTimeRange.Substring(0, strSecondTimeRange.IndexOf("~"));


            return DateTime.Parse(strFirstTimeHead).CompareTo(DateTime.Parse(strSecondTimeHead));
        }

        #endregion
    }

    #endregion
}
