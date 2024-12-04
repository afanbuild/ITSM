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
    public partial class ctrDateSelectTime : System.Web.UI.UserControl
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
                if (HidBeginTime.Value.Trim() != "")
                {
                    try
                    {
                        return DateTime.Parse(HidBeginTime.Value).ToString();
                    }
                    catch(Exception ex)
                    {
                        Epower.DevBase.BaseTools.E8Logger.Error(ex);
                        Epower.DevBase.BaseTools.E8Logger.Error("HidBeginTime.Value=" + HidBeginTime.Value);
                        throw;
                    }      
                }
                else
                {
                    return HidBeginTime.Value;
                }
            }
            set {
                //HidBeginTime.Value = value == "" ? value : DateTime.Parse(value).ToShortDateString();
                HidBeginTime.Value = value == "" ? value : DateTime.Parse(value).ToString("yyyy-MM-dd");
                TxtTime.Text = HidBeginTime.Value;
            }
        }
        /// <summary>
        /// 获得结束时间
        /// </summary>
        public string EndTime
        {
            get{
                  if(HidBeginEnd.Value.Trim()!="")
                  {
                     return  DateTime.Parse(HidBeginEnd.Value).AddDays(1).AddMinutes(-1).ToString();

                  }else
                  {
                      return HidBeginEnd.Value;
                  }
            }
            set
            {
                //HidBeginEnd.Value = value == "" ? value : DateTime.Parse(value).ToShortDateString();
                HidBeginEnd.Value = value == "" ? value : DateTime.Parse(value).ToString("yyyy-MM-dd");
                TxtTime.Text += HidBeginEnd.Value == "" ? HidBeginEnd.Value : "~" + HidBeginEnd.Value;
            }
        }

        public string sApplicationUrl = Constant.ApplicationPath;     //虚拟路径
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TxtTime.Attributes.Add("onfocus", "ShowDivTimeChoice('../Controls/TimeSelect/SelectTime.aspx'," + DivTxtTime.ClientID + "," + HidBeginTime.ClientID + "," + HidBeginEnd.ClientID + "," + TxtTime.ClientID + ");");
            }            
            if (strOnChangeFun.Length > 0)
            {
                TxtTime.Attributes.Add("onchange", strOnChangeFun);
            }
            else
                TxtTime.Attributes.Add("onchange", "");
        }
    }
}