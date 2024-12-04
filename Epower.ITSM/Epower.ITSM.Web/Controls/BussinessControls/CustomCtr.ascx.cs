/****************************************************************************
 * 
 * description:客户资料选择
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-10-24
 * *************************************************************************/
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

namespace Epower.ITSM.Web.Controls.BussinessControls
{
    public partial class CustomCtr : System.Web.UI.UserControl
    {
        /// <summary>
        /// 更改时客户端脚本
        /// </summary>
        public string OnChangeScript
        {
            set
            {
                ViewState["OnChangeScript"] = value;
            }
            get { return ViewState["OnChangeScript"] == null ? string.Empty : ViewState["OnChangeScript"].ToString(); }
        }

        /// <summary>
        /// 更改时客户端脚本
        /// </summary>
        public string OnChangeEndScript
        {
            set
            {
                ViewState["OnChangeEndScript"] = value;
            }
            get { return ViewState["OnChangeEndScript"] == null ? string.Empty : ViewState["OnChangeEndScript"].ToString(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                txtCustom.Text = hidCustomName.Value;
            }
        }

        public string CustomName
        {
            get
            {
                return txtCustom.Text.Trim();
            }
            set
            {
                txtCustom.Text = value;
                this.hidCustomName.Value = value;
                lblCustom.Text = value;
            }
        }

        public bool VisibleText
        {
            set
            {
                txtCustom.Visible = value;
                cmdPopCustom.Visible = value;
            }
        }


        public bool VisibleLabel
        {
            set
            {
                lblCustom.Visible = value;
            }
        }

        public bool MustInput
        {
            set 
            {
                labCustom.Visible = value;
            }
        }

        public Decimal CustomID
        {
            get
            {
                if (hidCustom.Value.Trim() == string.Empty)
                    return 0;
                else
                    return Decimal.Parse(hidCustom.Value);
            }
            set
            {
                hidCustom.Value = value.ToString();
            }
        }
    }
}