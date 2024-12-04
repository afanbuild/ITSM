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
using Epower.DevBase.Organization.SqlDAL;
using System.Xml;
using Epower.ITSM.SqlDAL;
using EpowerCom;
using Epower.ITSM.Base;
using System.Drawing;


namespace Epower.ITSM.Web.AppForms
{
    public partial class frm_BR_MEETINGSCHEDULED_List : BasePage
    {
        #region 变量
        long lngUserID = 0;
        long lngDeptID = 0;
        long lngOrgID = 0;
        RightEntity reTrace = null;  //服务跟踪权限 
        #endregion

        #region 设置父窗体按钮事件SetParentButtonEvent
        /// <summary>
        /// 设置父窗体按钮事件
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Button_New_Click);

            //this.Master.ShowQueryPageButton();
            this.Master.ShowNewButton(true);

            this.Master.ShowDeleteButton(false);

        }
        #endregion
        public void Master_Button_New_Click()
        {
            Response.Redirect("~\\Forms\\form_all_flowmodel.aspx?appid=1029");
        }

        #region 页面加载事件 Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtonEvent();
            lngUserID = (long)Session["UserID"];
            lngDeptID = (long)Session["UserDeptID"];
            lngOrgID = (long)Session["UserOrgID"];

            //获得权限
            reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.MeetingScheduled];//这里要设置权限

            if (!Page.IsPostBack)
            {
                setTime();//设置日期

                this.datagrid_Meeting.Columns[this.datagrid_Meeting.Columns.Count - 1].Visible = CheckRight(Constant.MeetingScheduled);  //删除流程权限admindeleteflow/MeetingScheduled
                Session["FromUrl"] = "../AppForms/frm_BR_MEETINGSCHEDULED_List.aspx";
                BindToTable();
            }
        }
        #endregion

        #region 查询事件Master_Master_Button_Query_Click
        /// <summary>
        /// 查询事件
        /// </summary>
        void Master_Master_Button_Query_Click()
        {

            BindToTable();
        }
        #endregion

        #region
        /// <summary>
        /// 设置日期控件的值
        /// </summary>
        public void setTime()
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            string dayofstr = DateTime.Now.DayOfWeek.ToString();
            int dayof = 0;//获取星期几
            switch (dayofstr)
            {
                case "Monday":
                    dayof = 1;
                    break;
                case "Tuesday":
                    dayof = 2;
                    break;
                case "Wednesday":
                    dayof = 3;
                    break;
                case "Thursday":
                    dayof = 4;
                    break;
                case "Friday":
                    dayof = 5;
                    break;
                case "Saturday":
                    dayof = 6;
                    break;
                case "Sunday":
                    dayof = 7;
                    break;
            }
            int dayofs = dayof - 1;
            int dayofe = 7 - dayof;
            ctrmeetingseletime.BeginTime = DateTime.Now.AddDays(-dayofs).ToShortDateString();
            ctrmeetingseletime.EndTime = DateTime.Now.AddDays(dayofe).ToShortDateString();
        }
        #endregion



        #region 取得数据并绑定到BindToTable
        /// <summary>
        /// 绑定到控件
        /// </summary>
        private void BindToTable()
        {
            XmlDocument xmlDoc = GetXmlValue();

            DataTable dt = App_pub_BR_MEETINGSCHEDULED_DP.GetIssuesForCond(xmlDoc.InnerXml, lngUserID, lngDeptID, lngOrgID, reTrace);
            Session["Con_Dealing"] = dt;


            for (int j = 0; j < dt.Rows.Count; j++)
            {
                dt.Rows[j]["datetime"] = Convert.ToDateTime(dt.Rows[j]["datetime"].ToString()).ToString("yyyy-MM-dd");
            }

            datagrid_Meeting.DataSource = dt;
            datagrid_Meeting.DataBind();


            #region 这里拼会议显示
            labtitle.Text = Convert.ToDateTime(this.ctrmeetingseletime.BeginTime).Date.ToString("yyyy-MM-dd") + " 到 " + Convert.ToDateTime(this.ctrmeetingseletime.EndTime).Date.ToString("yyyy-MM-dd") + " 会议室安排表";

            #region 新建表
            //新建一个表，用于存储拼写后的会议信息
            DataTable newdt = new DataTable();
            newdt.Columns.Add(new DataColumn("MeetingRoom"));//会议室
            newdt.Columns.Add(new DataColumn("Monday"));//星期一
            newdt.Columns.Add(new DataColumn("Tuesday"));//星期二
            newdt.Columns.Add(new DataColumn("Wednesday"));//星期三
            newdt.Columns.Add(new DataColumn("Thursday"));//星期四
            newdt.Columns.Add(new DataColumn("Friday"));//星期五
            newdt.Columns.Add(new DataColumn("Saturday"));//星期六
            newdt.Columns.Add(new DataColumn("Sunday"));//星期日
            #endregion

            #region 声明保存拼写后的信息
            string meetingroom = "";
            string mondays = "";
            string tuesdays = "";
            string wednesdays = "";
            string thursdays = "";
            string fridays = "";
            string Saturday = "";
            string Sunday = "";
            #endregion

            #region 遍历表，拼写会议
            DataTable catalog_dt = App_pub_BR_MEETINGSCHEDULED_DP.Get_Es_Catalog();//得到所有会议室

            DateTime datime_Monday = Convert.ToDateTime(this.ctrmeetingseletime.BeginTime);//星期一
            DateTime datime_Tuesday = Convert.ToDateTime(this.ctrmeetingseletime.BeginTime).AddDays(1);//星期二
            DateTime datime_Wednesday = Convert.ToDateTime(this.ctrmeetingseletime.BeginTime).AddDays(2);//星期三
            DateTime datime_Thursday = Convert.ToDateTime(this.ctrmeetingseletime.BeginTime).AddDays(3);//星期四
            DateTime datime_Friday = Convert.ToDateTime(this.ctrmeetingseletime.BeginTime).AddDays(4);//星期五
            DateTime datime_Saturday = Convert.ToDateTime(this.ctrmeetingseletime.BeginTime).AddDays(5);//星期六
            DateTime datime_Sunday = Convert.ToDateTime(this.ctrmeetingseletime.BeginTime).AddDays(6);//星期日

            #region 添加头部
            meetingroom = "<br/>会议室<br/><br/>";
            mondays = "星期一<br/>" + datime_Monday.Date.ToString("yyyy-MM-dd");
            tuesdays = "星期二<br/>" + datime_Tuesday.Date.ToString("yyyy-MM-dd");
            wednesdays = "星期三<br/>" + datime_Wednesday.Date.ToString("yyyy-MM-dd");
            thursdays = "星期四<br/>" + datime_Thursday.Date.ToString("yyyy-MM-dd");
            fridays = "星期五<br/>" + datime_Friday.Date.ToString("yyyy-MM-dd");
            Saturday = "星期六<br/>" + datime_Saturday.Date.ToString("yyyy-MM-dd");
            Sunday = "星期日<br/>" + datime_Sunday.Date.ToString("yyyy-MM-dd");

            //新增一行
            DataRow newdrow = newdt.NewRow();
            newdrow["MeetingRoom"] = meetingroom;
            newdrow["Monday"] = mondays;
            newdrow["Tuesday"] = tuesdays;
            newdrow["Wednesday"] = wednesdays;
            newdrow["Thursday"] = thursdays;
            newdrow["Friday"] = fridays;
            newdrow["Saturday"] = Saturday;
            newdrow["Sunday"] = Sunday;
            newdt.Rows.Add(newdrow);

            //清空变量
            meetingroom = "";
            mondays = "";
            tuesdays = "";
            wednesdays = "";
            thursdays = "";
            fridays = "";
            Saturday = "";
            Sunday = "";
            #endregion

            #region 添加身体
            foreach (DataRow cata_row in catalog_dt.Rows)
            {
                string cata_code = cata_row["catalogid"].ToString();
                //得到这个会议室下 所有当前选择时间本周的每天的记录
                meetingroom = cata_row["catalogname"].ToString();
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["meetingid"].ToString() == cata_code)
                    {
                        //星期一
                        if (dr["datetime"].ToString() == datime_Monday.Date.ToString())
                        {
                            DateTime startime = Convert.ToDateTime(dr["starttime"].ToString());
                            DateTime endtime = Convert.ToDateTime(dr["endtime"].ToString());
                            string minute_s = "";
                            string minute_e = "";
                            if (startime.Minute > 0)
                            {
                                minute_s = startime.Minute.ToString();
                                mondays = mondays + startime.Hour + ":" + minute_s + "—";
                            }
                            else
                            {
                                mondays = mondays + startime.Hour + ":00—";
                            }

                            if (endtime.Minute > 0)
                            {
                                minute_e = endtime.Minute.ToString();
                                mondays = mondays + endtime.Hour + ":" + minute_e;
                            }
                            else
                            {
                                mondays = mondays + endtime.Hour + ":00";
                            }

                            mondays = mondays + "<br/>";

                            mondays = mondays + dr["meetingname"].ToString() + "<br/><br/>";
                        }//星期二
                        else if (dr["datetime"].ToString() == datime_Tuesday.Date.ToString())
                        {
                            DateTime startime = Convert.ToDateTime(dr["starttime"].ToString());
                            DateTime endtime = Convert.ToDateTime(dr["endtime"].ToString());
                            string minute_s = "";
                            string minute_e = "";
                            if (startime.Minute > 0)
                            {
                                minute_s = startime.Minute.ToString();
                                tuesdays = tuesdays + startime.Hour + ":" + minute_s + "—";
                            }
                            else
                            {
                                tuesdays = tuesdays + startime.Hour + ":00—";
                            }

                            if (endtime.Minute > 0)
                            {
                                minute_e = endtime.Minute.ToString();
                                tuesdays = tuesdays + endtime.Hour + ":" + minute_e;
                            }
                            else
                            {
                                tuesdays = tuesdays + endtime.Hour + ":00";
                            }

                            tuesdays = tuesdays + "<br/>";
                            tuesdays = tuesdays + dr["meetingname"].ToString() + "<br/><br/>";
                        }//星期三
                        else if (dr["datetime"].ToString() == datime_Wednesday.Date.ToString())
                        {
                            DateTime startime = Convert.ToDateTime(dr["starttime"].ToString());
                            DateTime endtime = Convert.ToDateTime(dr["endtime"].ToString());
                            string minute_s = "";
                            string minute_e = "";
                            if (startime.Minute > 0)
                            {
                                minute_s = startime.Minute.ToString();
                                wednesdays = wednesdays + startime.Hour + ":" + minute_s + "—";
                            }
                            else
                            {
                                wednesdays = wednesdays + startime.Hour + ":00—";
                            }

                            if (endtime.Minute > 0)
                            {
                                minute_e = endtime.Minute.ToString();
                                wednesdays = wednesdays + endtime.Hour + ":" + minute_e;
                            }
                            else
                            {
                                wednesdays = wednesdays + endtime.Hour + ":00";
                            }

                            wednesdays = wednesdays + "<br/>";
                            wednesdays = wednesdays + dr["meetingname"].ToString() + "<br/><br/>";
                        }//星期四
                        else if (dr["datetime"].ToString() == datime_Thursday.Date.ToString())
                        {
                            DateTime startime = Convert.ToDateTime(dr["starttime"].ToString());
                            DateTime endtime = Convert.ToDateTime(dr["endtime"].ToString());
                            string minute_s = "";
                            string minute_e = "";
                            if (startime.Minute > 0)
                            {
                                minute_s = startime.Minute.ToString();
                                thursdays = thursdays + startime.Hour + ":" + minute_s + "—";
                            }
                            else
                            {
                                thursdays = thursdays + startime.Hour + ":00—";
                            }

                            if (endtime.Minute > 0)
                            {
                                minute_e = endtime.Minute.ToString();
                                thursdays = thursdays + endtime.Hour + ":" + minute_e;
                            }
                            else
                            {
                                thursdays = thursdays + endtime.Hour + ":00";
                            }

                            thursdays = thursdays + "<br/>";
                            thursdays = thursdays + dr["meetingname"].ToString() + "<br/><br/>";
                        }//星期五
                        else if (dr["datetime"].ToString() == datime_Friday.Date.ToString())
                        {
                            DateTime startime = Convert.ToDateTime(dr["starttime"].ToString());
                            DateTime endtime = Convert.ToDateTime(dr["endtime"].ToString());
                            string minute_s = "";
                            string minute_e = "";
                            if (startime.Minute > 0)
                            {
                                minute_s = startime.Minute.ToString();
                                fridays = fridays + startime.Hour + ":" + minute_s + "—";
                            }
                            else
                            {
                                fridays = fridays + startime.Hour + ":00—";
                            }

                            if (endtime.Minute > 0)
                            {
                                minute_e = endtime.Minute.ToString();
                                fridays = fridays + endtime.Hour + ":" + minute_e;
                            }
                            else
                            {
                                fridays = fridays + endtime.Hour + ":00";
                            }

                            fridays = fridays + "<br/>";
                            fridays = fridays + dr["meetingname"].ToString() + "<br/><br/>";
                        }//星期六
                        else if (dr["datetime"].ToString() == datime_Saturday.Date.ToString())
                        {
                            DateTime startime = Convert.ToDateTime(dr["starttime"].ToString());
                            DateTime endtime = Convert.ToDateTime(dr["endtime"].ToString());
                            string minute_s = "";
                            string minute_e = "";
                            if (startime.Minute > 0)
                            {
                                minute_s = startime.Minute.ToString();
                                Saturday = Saturday + startime.Hour + ":" + minute_s + "—";
                            }
                            else
                            {
                                Saturday = Saturday + startime.Hour + ":00—";
                            }

                            if (endtime.Minute > 0)
                            {
                                minute_e = endtime.Minute.ToString();
                                Saturday = Saturday + endtime.Hour + ":" + minute_e;
                            }
                            else
                            {
                                Saturday = Saturday + endtime.Hour + ":00";
                            }

                            Saturday = Saturday + "<br/>";
                            Saturday = Saturday + dr["meetingname"].ToString() + "<br/><br/>";
                        }//星期日
                        else if (dr["datetime"].ToString() == datime_Sunday.Date.ToString())
                        {
                            DateTime startime = Convert.ToDateTime(dr["starttime"].ToString());
                            DateTime endtime = Convert.ToDateTime(dr["endtime"].ToString());
                            string minute_s = "";
                            string minute_e = "";
                            if (startime.Minute > 0)
                            {
                                minute_s = startime.Minute.ToString();
                                Sunday = Sunday + startime.Hour + ":" + minute_s + "—";
                            }
                            else
                            {
                                Sunday = Sunday + startime.Hour + ":00—";
                            }

                            if (endtime.Minute > 0)
                            {
                                minute_e = endtime.Minute.ToString();
                                Sunday = Sunday + endtime.Hour + ":" + minute_e;
                            }
                            else
                            {
                                Sunday = Sunday + endtime.Hour + ":00";
                            }

                            Sunday = Sunday + "<br/>";
                            Sunday = Sunday + dr["meetingname"].ToString() + "<br/><br/>";
                        }

                    }
                }
                //新增一行
                DataRow newdrow2 = newdt.NewRow();
                newdrow2["MeetingRoom"] = meetingroom;
                newdrow2["Monday"] = mondays;
                newdrow2["Tuesday"] = tuesdays;
                newdrow2["Wednesday"] = wednesdays;
                newdrow2["Thursday"] = thursdays;
                newdrow2["Friday"] = fridays;

                newdrow2["Saturday"] = Saturday;
                newdrow2["Sunday"] = Sunday;
                newdt.Rows.Add(newdrow2);

                meetingroom = "";
                mondays = "";
                tuesdays = "";
                wednesdays = "";
                thursdays = "";
                fridays = "";
                Saturday = "";
                Sunday = "";
            }
            #endregion

            #endregion

            datagrid_show.ShowHeader = false;//隐藏表头
            if (newdt.Rows.Count > 1)
            {
                datagrid_show.DataSource = newdt;
                datagrid_show.DataBind();
            }
            #endregion


        }
        #endregion

        #region  生成查询XML字符串 GetXmlValue
        /// <summary>
        /// 生成查询XML字符串
        /// </summary>
        /// <returns></returns>
        private XmlDocument GetXmlValue()
        {
            FieldValues fv = new FieldValues();

            #region 预定日期
            fv.Add("begin_time", ctrmeetingseletime.BeginTime);

            DateTime dtime = Convert.ToDateTime(ctrmeetingseletime.EndTime);
            string endtime = dtime.Year + "-" + dtime.Month + "-" + dtime.Day + " 23:59:59";

            fv.Add("end_time", endtime);
            #endregion

            XmlDocument xmlDoc = fv.GetXmlObject();
            return xmlDoc;
        }
        #endregion


        #region 删除流程事件 datagrid_Meeting_DeleteCommand
        /// <summary>
        /// 删除流程事件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void datagrid_Meeting_DeleteCommand(object source, DataGridCommandEventArgs e)
        {

            long lngFlowID = 0;
            if (!string.IsNullOrEmpty(e.Item.Cells[0].Text))
            {
                lngFlowID = long.Parse(e.Item.Cells[0].Text);
            }

            DataTable dt = (DataTable)Session["Con_Dealing"];
            foreach (DataRow r in dt.Rows)
            {
                if (r["flowid"].ToString() == lngFlowID.ToString())
                {
                    dt.Rows.Remove(r);
                    break;
                }
            }
            Session["Con_Dealing"] = dt;
            BindToTable();
        }




        #endregion

        #region 检查权限 CheckRight
        /// <summary>
        /// 检查删除权限
        /// </summary>
        /// <param name="OperatorID"></param>
        /// <returns></returns>
        private bool CheckRight(long OperatorID)
        {
            RightEntity re = (RightEntity)((Hashtable)Session["UserAllRights"])[OperatorID];
            if (re == null)
                return false;
            else
                return re.CanDelete;
        }
        #endregion

        #region 显示页面地址 GetUrl
        /// <summary>
        /// 显示页面地址
        /// </summary>
        /// <param name="lngNoticeID"></param>
        /// <returns></returns>
        protected string GetUrl(decimal lngFlowID)
        {
            string sUrl = "";
            sUrl = "javascript:window.open('../Forms/frmIssueView.aspx?FlowID=" + lngFlowID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
            return sUrl;
        }

        #endregion

        #region 翻页控制ControlPageIssues_On_PostBack
        /// <summary>
        /// 翻页控制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ControlPageIssues_On_PostBack(object sender, EventArgs e)
        {
            //BindToTable();
        }
        #endregion


        protected void datagrid_Meeting_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (!string.IsNullOrEmpty(e.Item.Cells[1].Text.ToString()))
                {
                    //当超过整个流程预计处理时限未处理的，红低显示
                    if (int.Parse(e.Item.Cells[1].Text.Trim()) < 0)
                    {
                        for (int i = 0; i < e.Item.Cells.Count; i++)
                        {
                            e.Item.Cells[i].ForeColor = Color.Red;
                        }
                    }
                }
            }
        }

        protected void datagrid_show_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

            }

            if (e.Item.DataSetIndex <= 0 || e.Item.ItemIndex <= 0)
            {
                e.Item.BackColor = Color.LightGray;
            }
        }

        /// <summary>
        /// 上周会议
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnupofday_Click(object sender, EventArgs e)
        {
            DateTime daybegintime = Convert.ToDateTime(this.ctrmeetingseletime.BeginTime);//得到本周开始时间
            this.ctrmeetingseletime.BeginTime = daybegintime.AddDays(-7).ToString();
            this.ctrmeetingseletime.EndTime = daybegintime.AddDays(-1).ToString();
            BindToTable();
        }

        /// <summary>
        /// 下周会议
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnnexofday_Click(object sender, EventArgs e)
        {
            DateTime dayendtime = Convert.ToDateTime(this.ctrmeetingseletime.EndTime);//得到本周最后时间
            this.ctrmeetingseletime.BeginTime = dayendtime.AddDays(1).ToString();
            this.ctrmeetingseletime.EndTime = dayendtime.AddDays(7).ToString();
            BindToTable();
        }
    }
}
