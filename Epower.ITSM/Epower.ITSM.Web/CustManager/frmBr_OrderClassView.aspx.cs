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
using Epower.ITSM.Base;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;
using System.Data.OracleClient;
using Epower.ITSM.SqlDAL.Flash;
using System.Text;

namespace Epower.ITSM.Web.CustManager
{
    public partial class frmBr_OrderClassView : BasePage
    {

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.CBr_OrderClass;
            this.Master.IsCheckRight = true;
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_Edit_Click += new Global_BtnClick(Master_Master_Button_Edit_Click);
            this.Master.Master_Button_Save_Click += new Global_BtnClick(Master_Master_Button_Save_Click);
            this.Master.ShowQueryButton(true);
            this.Master.Btn_edit.Text = "排班表管理";
            this.Master.ShowEditButton(true);
            this.Master.Btn_save.Text = "班次管理";
            this.Master.ShowSaveButton(true);
            this.Master.MainID = "1";
            
        }
        #endregion

        void Master_Master_Button_Save_Click()
        {
            Response.Redirect("frmBr_OrderClassTypeMain.aspx");
        }

        void Master_Master_Button_Edit_Click()
        {
            Response.Redirect("frmBr_OrderClassMain.aspx");
        }

        #region Master_Master_Button_Query_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            BindData();
        }
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtonEvent();
            cpBr_OrderClassType.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(BindDataClassType);

            if (!IsPostBack)
            {
                BindDataClassType();
                BindData();
            }
        }
        #endregion

        #region 班次表相关

        private void BindDataClassType()
        {
            DataTable dt;
            string sWhere = " 1=1 And Deleted=0 ";
            string sOrder = " order by id";

            Br_OrderClassTypeDP ee = new Br_OrderClassTypeDP();
            int iRowCount = 0;
            dt = ee.GetDataTable(sWhere, sOrder, this.cpBr_OrderClassType.PageSize, this.cpBr_OrderClassType.CurrentPage, ref iRowCount);
            dgBr_OrderClassType.DataSource = dt.DefaultView;
            dgBr_OrderClassType.DataBind();
            this.cpBr_OrderClassType.RecordCount = iRowCount;
            this.cpBr_OrderClassType.Bind();
        }

        #region dgBr_OrderClassType_ItemCreated
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgBr_OrderClassType_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                int j = 0;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    j = i;
                    e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");

                }
            }
        }
        #endregion

        #region dgBr_OrderClassType_ItemDataBound
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgBr_OrderClassType_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
            }
        }
        #endregion

        #endregion

        #region BindData
        /// <summary>
        /// 
        /// </summary>
        private void BindData()
        {
            DateTime begintime = DateTime.Now;
            DateTime endtime = DateTime.Now;

            DataTable dt;
            string sWhere = "";
            string sOrder = " order by id";

            if (UserPickerMult1.UserID.Trim() != "")
            {
                sWhere += " And StaffID in (" + UserPickerMult1.UserID.Trim(',') + ")";
            }

            if (ctrDateSelectTime1.BeginTime.Trim() != "" && ctrDateSelectTime1.EndTime.Trim() != "")
            {
                sWhere += " And to_char(DutyTime,'yyyy-MM-dd') >= " + StringTool.SqlQ(DateTime.Parse(ctrDateSelectTime1.BeginTime.Trim()).ToString("yyyy-MM-dd")) + " And to_char(DutyTime,'yyyy-MM-dd')<=" + StringTool.SqlQ(DateTime.Parse(ctrDateSelectTime1.EndTime.Trim()).ToString("yyyy-MM-dd"));

                begintime = DateTime.Parse(ctrDateSelectTime1.BeginTime);
                endtime = DateTime.Parse(ctrDateSelectTime1.EndTime);

            }
            else
            {
                int weekdays = (int)DateTime.Now.DayOfWeek + 1;
                int mindays = weekdays - 1;
                int maxdays = 7 - weekdays;
                begintime = DateTime.Now.AddDays(-mindays);
                endtime = DateTime.Now.AddDays(maxdays);

                sWhere += " And to_char(DutyTime,'yyyy-MM-dd') >= " + StringTool.SqlQ(DateTime.Now.AddDays(-mindays).ToString("yyyy-MM-dd")) + " And to_char(DutyTime,'yyyy-MM-dd')<=" + StringTool.SqlQ(DateTime.Now.AddDays(maxdays).ToString("yyyy-MM-dd"));
            }
            Br_OrderClassDP ee = new Br_OrderClassDP();
            dt = ee.GetDataTable(sWhere, sOrder);          

            ShowDT(dt, begintime, endtime);
        }
        #endregion

        #region 展示内容到页面
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ht"></param>
        private void ShowDT(DataTable dt,DateTime begintime,DateTime endtime)
        {
            DateTime dtime = begintime;
            DateTime dtime2 = begintime;

            string str = "";

            str += "<div>" +
                "<table class='listContent' width='100%'>" +
                    "<tr>" +
                        "<td class='listTitle' align='center'>部门</td>" +
                        "<td class='listTitle' align='center'>姓名</td>";

            while (dtime <= endtime)
            {
                str += "<td class='listTitle' align='center'>" + dtime.ToString("MM月dd日") + "</td>";
                dtime = dtime.AddDays(1);
            }
            str += "</tr>";

            string[] y = new string[2];
            y[0] = "StaffName";
            y[1] = "DeptName";
            DataTable dt2 = FlashCS.rtnTBL(dt, y, "DutyTime", "ClassTypeName");


            foreach (DataRow dr in dt2.Rows)
            {
                str += "<tr>" +
                        "<td class='list' align='left'>" +
                            dr["DeptName"].ToString() +
                        "</td>" +
                        "<td class='list' align='left'>" +
                            dr["StaffName"].ToString() +
                        "</td>";

                dtime = begintime;

                while (dtime <= endtime)
                {

                    if (dr.Table.Columns.Contains(dtime.ToUniversalTime().Date.ToString()))
                    {
                        if (dr[dtime.ToUniversalTime().Date.ToString()].ToString() != string.Empty)
                            str += "<td class='list' align='center'>" + dr[dtime.ToUniversalTime().Date.ToString()].ToString() + "</td>";
                        else
                            str += "<td class='list' align='center'><font color='red'>休</font></td>";
                    }
                    else
                        str += "<td class='list' align='center'><font color='red'>休</font></td>";

                    dtime = dtime.AddDays(1);
                }

                str += "</tr>";

            }

            str += "</table></div>";


            mainDiv.InnerHtml = str;

        }
        #endregion
    }
}
