using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.AnalsysForms
{
    public partial class frm_AssetsAvailability :BasePage
    {
        #region 设置父窗体按钮事件SetParentButtonEvent
        /// <summary>
        /// 设置父窗体按钮事件
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.ShowQueryButton(true);
            this.Master.MainID = "1";     //设置页面的ID编号，如果为查询页面，则设置为1     

            #region 设置日期控件 为当月的日期
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            string begindate = year + "-" + month + "-" + "1";

            CtrDateSelectTime1.BeginTime = begindate;
            CtrDateSelectTime1.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
            #endregion
        }
        #endregion

        ZHServiceDP zhdp = new ZHServiceDP();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetParentButtonEvent();
            }

            LoadData();
        }

        /// <summary>
        /// 查询
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            LoadData();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public DataTable getDate(string begindate,string enddate,string equipment)
        {
            //变量
            #region 变量
            DataTable dt_iss = new DataTable();   //有资产的事件

            DataTable dt_Desk = new DataTable();   //资产表
            DateTime today_date = new DateTime(); //当前时间的日期部分
            int todays = 0;                       //获取当前日期是当年中的第几天
            DateTime CustTime = new DateTime();   //发生时间
            DateTime FinishedTime = new DateTime();//完成时间
            int Assets_days = 0;                  //故障天数

            double Asslv = 0.0f;//可用率
            int year = 0;//当年
            int days = 0;//当年天数
            int N = 4;
            #endregion

            #region 可用率表格申请
            DataTable dt_Asty = new DataTable();//可用率
            dt_Asty.Columns.Add(new DataColumn("equipmentid"));
            dt_Asty.Columns.Add(new DataColumn("equipmentname"));
            dt_Asty.Columns.Add(new DataColumn("Asse"));

            #endregion

            dt_Desk = zhdp.Get_Desk();
            dt_iss = zhdp.Get_AssetsAvailability(begindate,enddate);

            today_date = DateTime.Now.Date;
            todays = today_date.DayOfYear;

            if (dt_iss != null && dt_iss.Rows.Count > 0)
            {
                foreach (DataRow dr_desk in dt_Desk.Rows)
                {
                    string desk_id = dr_desk["id"].ToString();
                    string desk_name = dr_desk["name"].ToString();
                    int day_sum = 0;
                    bool isok = false;
                    foreach (DataRow dr_iss in dt_iss.Rows)
                    {
                        string drissequid = dr_iss["equipmentid"].ToString();
                        if (desk_id == drissequid)
                        {
                            isok = true;
                            CustTime = Convert.ToDateTime(dr_iss["CustTime"].ToString() == "" ? DateTime.Now.Date.ToString() : dr_iss["CustTime"].ToString()).Date;//发生日期
                            FinishedTime = Convert.ToDateTime(dr_iss["FinishedTime"].ToString() == "" ? DateTime.Now.Date.ToString() : dr_iss["FinishedTime"].ToString()).Date;//完成日期
                            day_sum = day_sum + FinishedTime.DayOfYear - CustTime.DayOfYear;
                        }
                        else
                        { 
                            
                        }
                    }

                    if (isok)
                    {
                        Assets_days = todays - day_sum;//可用天数
                        year = DateTime.Now.Date.Year;
                        DateTime todtime = new DateTime(year, 12, 31);
                        days = todtime.DayOfYear;
                        Asslv = (Convert.ToDouble(Assets_days) / Convert.ToDouble(days)) * 100;
                    }
                    else
                    {
                        Asslv = 1 * 100;
                    }
                    if (Asslv > 0)
                    {
                        if (equipment != "")
                        {
                            if (equipment == desk_name)
                            {
                                DataRow dr_Asty = dt_Asty.NewRow();
                                dr_Asty["equipmentid"] = desk_id;
                                dr_Asty["equipmentname"] = dr_desk["name"].ToString();
                                dr_Asty["Asse"] = Asslv.ToString("N" + N.ToString()) + "%";
                                dt_Asty.Rows.Add(dr_Asty);
                                Asslv = 0;
                            }
                        }
                        else
                        {
                            DataRow dr_Asty = dt_Asty.NewRow();
                            dr_Asty["equipmentid"] = desk_id;
                            dr_Asty["equipmentname"] = dr_desk["name"].ToString();
                            dr_Asty["Asse"] = Asslv.ToString("N" + N.ToString()) + "%";
                            dt_Asty.Rows.Add(dr_Asty);
                            Asslv = 0;
                        }
                    }
                }
            }

            return dt_Asty;
        }

        /// <summary>
        /// 数据绑定
        /// </summary>
        private void LoadData()
        {
            string begindate =CtrDateSelectTime1.BeginTime.ToString() ; //ctrDateTime.BeginTime.ToString();
            string enddate = CtrDateSelectTime1.EndTime.ToString(); //ctrDateTime.EndTime.ToString();
            string equipmentid = HidAvaEqu.Value.ToString();
            string equipmentname = txtEqu.Text.ToString();

            DataTable dt = getDate(begindate, enddate, equipmentname);
            DataView dv = new DataView(dt);
            dv.Sort = "Asse";

            dgSchemeRatio.DataSource = dv;
            dgSchemeRatio.DataBind();
        }
    }
}