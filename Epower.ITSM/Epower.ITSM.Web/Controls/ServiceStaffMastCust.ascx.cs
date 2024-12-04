/*******************************************************************
 * 版权所有：
 * Description：服务人员选择
 * 
 * 
 * Create By  ：
 * Create Date：
 * *****************************************************************/
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.Controls
{
    public partial class ServiceStaffMastCust : System.Web.UI.UserControl
    {
        private string strOnChangeFun = string.Empty;

        private string stronkeydownFun = string.Empty;
        /// <summary>
        /// 更改时客户端脚本
        /// </summary>
        public string OnChangeScript
        {
            get { return strOnChangeFun; }
            set { strOnChangeFun = value; }
        }

        public string RequestType
        {
            set;
            get;
        }

        /// <summary>
        /// 点击选择客户端脚本
        /// </summary>
        public string OnKeyDownScript
        {
            get { return stronkeydownFun; }
            set { stronkeydownFun = value; }
        }

        public string EngineerJsonString
        { set; get; }

        

        protected void Page_Load(object sender, EventArgs e)
        {
            EngineerJsonString = JsonUtil.ChangeToJson(new EngineerSourceServer().GetEngineerTable()).ToString();
            if (Page.IsPostBack)
            {
                txtUser.Text = hidUserName.Value;
                
            }
            else
            {
               
                if (strOnChangeFun.Length > 0)
                {
                    txtUser.Attributes.Add("onchange", strOnChangeFun);
                }
                if (stronkeydownFun.Length > 0)
                {
                    cmdPopUser.Attributes.Add("onmousedown", stronkeydownFun);
                }
            }
        }

        public string GetRequestType()
        {            
            return (string.IsNullOrEmpty(RequestType))?"":RequestType.ToString();
        }


        public string UserName
        {
            get
            {
                return txtUser.Text.Trim();
            }
            set
            {
                txtUser.Text = value;
                this.hidUserName.Value = value;
                labUser.Text = value;
            }
        }

        public string FlowID
        {
            get
            {
                return hidFlowID.Value.Trim();
            }
            set
            {
                hidFlowID.Value = value;
            }
        }

        public bool VisibleText
        {
            get { return txtUser.Visible; }
            set
            {
                txtUser.Visible = value;
                cmdPopUser.Visible = value;
            }
        }


        public bool VisibleLabel
        {
            get { return labUser.Visible; }
            set
            {
                labUser.Visible = value;
            }
        }

        public string UserID
        {
            get
            {
                return hidUser.Value;
            }
            set
            {
                hidUser.Value = value.ToString();
            }
        }
    }
}