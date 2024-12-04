using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace Epower.ITSM.Web.AppSchedules
{
    public partial class CrtTime : System.Web.UI.UserControl
    {
        private DateTime dTime = DateTime.Now;
        private int hour = 0;
        private int minute = 0;

        /// <summary>
        /// 返回控件的值
        /// </summary>
        public string dateTime
        {
            get
            {
                string hours = ddlHours.SelectedValue.PadLeft(2, char.Parse("0"));
                string minutes = ddlMinutes.SelectedValue.PadLeft(2, char.Parse("0"));
                return hours + ":" + minutes;
            }
            set
            {
                string time = value;
                string[] times=time.Split(":".ToCharArray());
                ddlHours.SelectedIndex = ddlHours.Items.IndexOf(ddlHours.Items.FindByText(times[0]));
                ddlMinutes.SelectedIndex = ddlMinutes.Items.IndexOf(ddlMinutes.Items.FindByText(times[1]));
            }
        }

        /// <summary>
        /// 小时
        /// </summary>
        public int Hour
        {
            get
            {
                return int.Parse(ddlHours.SelectedValue);
            }
            set
            {
                hour = value;
            }
        }

        /// <summary>
        /// 分钟
        /// </summary>
        public int Minute
        {
            get
            {
                return int.Parse(ddlMinutes.SelectedValue);
            }
            set
            {
                minute = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack == false)
            {
                InitPage();
            }
        }

        private void InitPage()
        {

        }

        #region Web 窗体设计器生成的代码
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
            //
            InitializeComponent();
            base.OnInit(e);


            for (int i = 0; i <= 23; i++)
            {
                string m = i.ToString().PadLeft(2, char.Parse("0"));
                ddlHours.Items.Add(new ListItem(m, i.ToString()));
            }
            ddlHours.SelectedValue = hour.ToString().PadLeft(2, char.Parse("0"));

            for (int j = 0; j <= 59; j++)
            {
                string m = j.ToString().PadLeft(2, char.Parse("0"));
                ddlMinutes.Items.Add(new ListItem(m, j.ToString()));
            }
            ddlMinutes.SelectedValue = minute.ToString().PadLeft(2, char.Parse("0"));
        }

        /// <summary>
        ///		设计器支持所需的方法 - 不要使用代码编辑器
        ///		修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion
    }
}