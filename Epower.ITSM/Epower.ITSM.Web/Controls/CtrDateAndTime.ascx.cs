/*******************************************************************
 * 版权所有：
 * Description：时间日期控件
 * 
 * 
 * Create By  ：
 * Create Date：
 * *****************************************************************/
using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Epower.ITSM.Base;

namespace Epower.ITSM.Web.Controls
{
    /// <summary>
    ///		CtrDateAndTime 的摘要说明。
    /// </summary>
    public partial class CtrDateAndTime : System.Web.UI.UserControl
    {

        #region 变量定义区
        private DateTime dTime = DateTime.Now;

        private string strOnChangeFun = "";

        private bool blnShowTime = true;

        private bool blnAllowNull = false;
        private eOA_FlowControlState cState = eOA_FlowControlState.eNormal;
        private bool blnShowMinute = true;

        private bool blnShowYear = true;
        #endregion

        #region 属性定义
        public eOA_FlowControlState ContralState
        {
            get { return cState; }
            set { cState = value; }
        }
        /// <summary>
        /// 返回控件的值
        /// </summary>
        public DateTime dateTime
        {
            get
            {

                if (this.dateTimeString.Trim().Length > 5)
                {
                    return DateTime.Parse(this.dateTimeString);
                }
                else
                {
                    return DateTime.Now;
                }
            }
            set
            {
                dTime = value;                
                txtDate.Text = dTime.ToString("yyyy-MM-dd");
                ddlHours.SelectedIndex = ddlHours.Items.IndexOf(ddlHours.Items.FindByValue(dTime.Hour.ToString()));
                ddlMinutes.SelectedIndex = ddlMinutes.Items.IndexOf(ddlMinutes.Items.FindByValue(dTime.Minute.ToString()));
             
            }
        }
        /// <summary>
        /// 返回控件值的字符串形式
        /// </summary>
        public string dateTimeString
        {
            get { return GetMyDateTime(); }
            set
            {
                string svalue = value;
              
                DateTime datetime = DateTime.Now;
                if (svalue == string.Empty)
                {
                    txtDate.Text = string.Empty;
                }
                else
                {
                    datetime = DateTime.Parse(svalue);
                    txtDate.Text = datetime.ToString("yyyy-MM-dd");
                }
            }
        }
        /// <summary>
        /// 日期更改时客户端脚本
        /// </summary>
        public string OnChangeScript
        {
            set { strOnChangeFun = value; }
        }
        /// <summary>
        /// 是否显示时间部分
        /// </summary>
        public bool ShowTime
        {
            set { blnShowTime = value; }
            get { return blnShowTime; }
        }
        /// <summary>
        /// 是否显示分钟
        /// </summary>
        public bool ShowMinute
        {
            set { blnShowMinute = value; }
            get { return blnShowMinute; }
        }

        /// <summary>
        /// 是否容许显示空日期[如果容许显示空日期,但实际未赋值,dateTime属性需要特殊处理]
        /// </summary>
        public bool AllowNull
        {
            set { blnAllowNull = value; }
            get { return blnAllowNull; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetMyDateTime()
        {
            string strDateTime;

            try
            {
                DateTime.Parse(txtDate.Text.Trim());
            }
            catch
            {
                txtDate.Text = "";
            }

            if (blnShowTime == true)
            {
                strDateTime = txtDate.Text + " " + ddlHours.SelectedValue.PadLeft(2, char.Parse("0")) + ":" + ddlMinutes.SelectedValue.PadLeft(2, char.Parse("0"));
            }
            else
            {
                strDateTime = txtDate.Text;
            }
            if (txtDate.Text.Trim() == string.Empty)
            {
                strDateTime = string.Empty;
            }
            return strDateTime;
        }
        private bool mDisparity = false;
        /// <summary>
        /// 
        /// </summary>
        public bool Disparity
        {
            get
            {
                return mDisparity;
            }
            set
            {
                mDisparity = value;
            }
        }
        private bool blnMust = false;
        /// <summary>
        /// 是否一定要输入
        /// </summary>
        public bool MustInput
        {
            set
            {
                blnMust = value;

            }
            get { return blnMust; }
        }
        private string strMessage = "";
        /// <summary>
        /// 设置未输入时的提示信息
        /// </summary>
        public string TextToolTip
        {
            set { strMessage = value; }
        }
        #endregion

        #region 方法区
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            if (Page.IsPostBack == false)
            {
                if (blnMust == true)
                {
                    //添加公共校验基础资料
                    string strThisMsg = "";
                    strThisMsg = txtDate.ClientID + ">" + strMessage + ">";                    
                    Response.Write("<script>if(typeof(sarrvalidlist) == 'undefined'){ var sarrvalidlist = new Array(1);}  sarrvalidlist[0] = sarrvalidlist[0] + '|' + '" + strThisMsg + "';</script>");
           
                }
             
               
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            txtDate.Attributes.Add("onfocus", "WdatePicker({skin:'blue'});");
            if (Page.IsPostBack == false)
            {
                InitPage();
            }
            else     //特殊情况
            {
                if (Disparity)
                {
                    InitPage();
                }
            }
            txtDate.Attributes.Add("onblur", "if(isDate(document.all." + txtDate.ClientID + ".value) == false){document.all." + txtDate.ClientID + ".value = '" + txtDate.Text + "' };");
            if (strOnChangeFun.Length > 0)
            {
                txtDate.Attributes.Add("onchange", strOnChangeFun);
                ddlHours.Attributes.Add("onchange", strOnChangeFun);
                ddlMinutes.Attributes.Add("onchange", strOnChangeFun);
            }
            if (blnMust == false || ContralState == eOA_FlowControlState.eReadOnly || ContralState == eOA_FlowControlState.eHidden)
            {
                rWarning.Visible = false;
            }     
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitPage()
        {
            if (blnAllowNull == false)
            {
                if (txtDate.Text.Trim() != string.Empty)
                {
                    dateTime = Convert.ToDateTime(GetMyDateTime());
                    txtDate.Text = dTime.ToString("yyyy-MM-dd");
                    labDate.Text = txtDate.Text;
                }
                else
                {
                    txtDate.Text = "";
                    labDate.Text = "";
                }
            }
            else
            {
                txtDate.Text = "";
                labDate.Text = "";
            }

            if (blnShowTime == true)
            {
                for (int i = 0; i < 24; i++)
                {
                    if (i == dTime.Hour)
                    {
                        ddlHours.SelectedIndex = i;
                    }
                }
                for (int i = 0; i < 60; i++)
                {
                    if (i == dTime.Minute)
                    {
                        ddlMinutes.SelectedIndex = i;
                    }
                }

                if (blnShowMinute == true)
                {
                    labDate.Text = txtDate.Text + ((txtDate.Text.Equals(string.Empty)) ? "" : " " + ddlHours.SelectedValue.PadLeft(2, char.Parse("0")) + ":" + ddlMinutes.SelectedValue.PadLeft(2, char.Parse("0")));
                }
                else
                {
                    ddlMinutes.Visible = false;
                    ddlMinutes.SelectedIndex = 0;
                    labDate.Text = txtDate.Text + ((txtDate.Text.Equals(string.Empty)) ? "" : " " + ddlHours.SelectedValue.PadLeft(2, char.Parse("0")));
                }
            }
            else
            {
                ddlHours.Visible = false;
                ddlMinutes.Visible = false;
            }

            if (cState != eOA_FlowControlState.eNormal)
            {
                labDate.Visible = true;
                txtDate.Visible = false;
               // imgDate.Visible = false;
                ddlHours.Visible = false;
                ddlMinutes.Visible = false;
                if (cState == eOA_FlowControlState.eHidden)
                {
                    labDate.Text = "--";
                }
            }

        }
        #endregion

        #region Web 窗体设计器生成的代码
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
            //
            InitializeComponent();
            base.OnInit(e);

            for (int i = 0; i < 24; i++)
            {
                ddlHours.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            for (int i = 0; i < 60; i++)
            {
                ddlMinutes.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
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
