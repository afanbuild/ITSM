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
using Epower.ITSM.Base;

namespace Epower.ITSM.Web.Controls
{
    public partial class TrueOrFalseShowInpText2 : System.Web.UI.UserControl
    {
        private string _MustInput = "false";
        /// <summary>
        /// 是否必填
        /// </summary>
        public string MustInput
        {
            get { return _MustInput; }
            set { _MustInput = value; }
        }

        private string _IsShowText = "true";
        /// <summary>
        /// 是否显示文本框
        /// </summary>
        public string IsShowText
        {
            get { return _IsShowText; }
            set { _IsShowText = value; }
        }

        private string _SelToolTip = "信息项";
        /// <summary>
        /// 必选提示文本
        /// </summary>
        public string SelToolTip
        {
            get { return _SelToolTip; }
            set
            {
                _SelToolTip = value;
            }
        }

        private string _TextMustInput = "true";
        /// <summary>
        /// 文本是否必填
        /// </summary>
        public string TextMustInput
        {
            get { return _TextMustInput; }
            set { _TextMustInput = value; }
        }

        private string _ToolTipString = "信息项";
        /// <summary>
        /// 必填提示文本
        /// </summary>
        public string ToolTipString
        {
            get { return _ToolTipString; }
            set
            {
                _ToolTipString = value;
                hidText.Value = value;
            }
        }

        private string _TextControlWidth = "90%";
        /// <summary>
        /// 文本框控件宽度
        /// </summary>
        public string TextControlWidth
        {
            get { return _TextControlWidth; }
            set { _TextControlWidth = value; }
        }

        /// <summary>
        /// 文本框显示条件值
        /// </summary>
        public string SelectChangeValue
        {
            set
            {
                hidSelValue.Value = value;
                //if(value != "1")
                //SelValue = "1";
            }
            get
            {
                return hidSelValue.Value;
            }
        }

        /// <summary>
        /// 获取或设置下拉列表选中值
        /// </summary>
        public string SelValue
        {
            get { return selTrueOrFalse.Value; }
            set
            {
                if (selTrueOrFalse.Items.FindByValue(value) != null)
                {
                    selTrueOrFalse.Items.FindByValue(value).Selected = true;
                    lblTrueOrFalse.Text = selTrueOrFalse.Items[selTrueOrFalse.SelectedIndex].Text;
                }
            }
        }

        /// <summary>
        /// 获取或设置文本框值
        /// </summary>
        public string TextValue
        {
            get { return txtContent.Text == ToolTipString ? "" : txtContent.Text; }
            set
            {
                txtContent.Text = value;
                lblContent.Text = value;
            }
        }

        private eOA_FlowControlState cState = eOA_FlowControlState.eNormal;
        /// <summary>
        /// 控件状态
        /// </summary>
        public eOA_FlowControlState ContralState
        {
            get { return cState; }
            set { cState = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (MustInput.ToLower() == "true")
            {
                sp.Visible = true;
                string strThisMsg = "";
                strThisMsg = selTrueOrFalse.ClientID + ">" + SelToolTip;
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), selTrueOrFalse.ClientID, "<script type=\"text/javascript\" >if(typeof(sarrvalidlist) == 'undefined'){ var sarrvalidlist = new Array(1);}  sarrvalidlist[0] = sarrvalidlist[0] + '|' + '" + strThisMsg + "';</script>");
            }

            if (TextMustInput.ToLower() == "true")
            {
                lblStar.Visible = true;
            }

            if (string.IsNullOrEmpty(TextValue))
            {
                txtContent.Text = ToolTipString;
            }

            if (SelValue == hidSelValue.Value || SelValue == "1")
            {
                trContent.Attributes.Remove("style");
            }

            if (cState != eOA_FlowControlState.eNormal)
            {
                lblTrueOrFalse.Visible = true;
                selTrueOrFalse.Visible = false;
                lblContent.Visible = true;
                txtContent.Visible = false;
                lblStar.Visible = false;
                sp.Visible = false;
                if (cState == eOA_FlowControlState.eHidden)
                {
                    lblTrueOrFalse.Text = "--";
                    lblContent.Text = "--";
                }
            }
        }

    }
}