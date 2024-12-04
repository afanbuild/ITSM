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

namespace Epower.ITSM.Web.Controls
{
    public partial class ServiceStaff : System.Web.UI.UserControl
    {
        private string strOnChangeFun = string.Empty;
        /// <summary>
        /// 更改时客户端脚本
        /// </summary>
        public string OnChangeScript
        {
            get { return strOnChangeFun; }
            set { strOnChangeFun = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
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
            }
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

        public long UserID
        {
            get
            {
                if (hidUser.Value.Trim() == string.Empty)
                    return 0;
                else
                    return long.Parse(hidUser.Value);
            }
            set
            {
                hidUser.Value = value.ToString();
            }
        }
    }
}