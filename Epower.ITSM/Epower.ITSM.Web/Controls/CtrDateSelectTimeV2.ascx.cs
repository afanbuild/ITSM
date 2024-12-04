/************************************************
 * 创建人：yanghw
 * 创建时间：2011-08-26  
 * 时间选择控件
 * 
**********************************************/
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Epower.ITSM.Base;


namespace Epower.ITSM.Web.Controls
{
    public partial class ctrDateSelectTimeV2 : System.Web.UI.UserControl
    {

        private string strOnChangeFun = "";
        /// <summary>
        /// 日期更改时客户端脚本
        /// </summary>
        public string OnChangeScript
        {
            set { strOnChangeFun = value; }
        }

        /// <summary>
        /// 获得开始时间


        /// </summary>
        public string BeginTime
        {
            get
            {
                if (!string.IsNullOrEmpty(txtBeginDate.Text))
                {
                    try
                    {
                        return DateTime.Parse(txtBeginDate.Text).ToString();
                    }
                    catch (Exception ex)
                    {
                        Epower.DevBase.BaseTools.E8Logger.Error(ex);
                        Epower.DevBase.BaseTools.E8Logger.Error("txtBeginDate.Text=" + txtBeginDate.Text);
                        throw;
                    }
                }
                else
                {
                    return txtBeginDate.Text;
                }
            }
            set
            {
                //HidBeginTime.Value = value == "" ? value : DateTime.Parse(value).ToShortDateString();
                txtBeginDate.Text = value == "" ? value : DateTime.Parse(value).ToString("yyyy-MM-dd");

            }
        }
        /// <summary>
        /// 获得结束时间
        /// </summary>
        public string EndTime
        {
            get
            {
                if (!string.IsNullOrEmpty(txtEndDate.Text))
                {
                    return DateTime.Parse(txtEndDate.Text).AddDays(1).AddMinutes(-1).ToString();

                }
                else
                {
                    return txtEndDate.Text;
                }
            }
            set
            {
                //HidBeginEnd.Value = value == "" ? value : DateTime.Parse(value).ToShortDateString();
                txtEndDate.Text = value == "" ? value : DateTime.Parse(value).ToString("yyyy-MM-dd");

            }
        }

        public string sApplicationUrl = Constant.ApplicationPath;     //虚拟路径
        protected void Page_Load(object sender, EventArgs e)
        {
            /*     
             * Date: 2013-07-09 10:21
             * summary: 为日期范围控件注册 my97 日期函数. 当选择了开始日期
             * 后, 则结束日期不能选择开始日期以前的. 
             * modified: sunshaozong@gmail.com     
             */

            String strBeginDateFunc = "var d5222=$dp.$('" + txtEndDate.ClientID + "');WdatePicker({onpicked:function(){d5222.focus();},maxDate:'#F{$dp.$D(\\'" + txtEndDate.ClientID + "\\')}'})";
            String strEndDateFunc = "WdatePicker({minDate:'#F{$dp.$D(\\'" + txtBeginDate.ClientID + "\\')}'})";

            txtBeginDate.Attributes.Add("onfocus", strBeginDateFunc);
            txtEndDate.Attributes.Add("onfocus", strEndDateFunc);
        }
    }
}