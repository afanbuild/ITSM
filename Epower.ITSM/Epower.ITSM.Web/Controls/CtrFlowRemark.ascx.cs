/*******************************************************************
 * 版权所有：
 * Description：流程大文本输入控件
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
using Epower.ITSM.Base;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.Controls
{
    public partial class CtrFlowRemark : System.Web.UI.UserControl
    {
        private string sValue = "";

        private string strOnChangeFun = "";

        private string strOnBlurFun = "";

        private string strOnFocusFun = "";

        private bool blnMust = false;

        private string strMessage = "";



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

        /// <summary>
        /// 设置未输入时的提示信息
        /// </summary>
        public string TextToolTip
        {
            set { strMessage = value; }
        }

        /// <summary>
        /// 文本控件的客户端ID
        /// </summary>
        public string ClientID
        {
            get { return txtText.ClientID; }
        }

        /// <summary>
        /// 最大长度
        /// </summary>
        public int MaxLength
        {
            set
            {
                //txtText.Attributes.Add("onblur", "javascript:MaxLength(this," + value.ToString() + ",'输入内容超出限定长度：');");
                txtText.MaxLength = value;
            }

            get { return txtText.MaxLength; }
        }

        /// <summary>
        /// 更改时客户端脚本
        /// </summary>
        public string OnChangeScript
        {
            set { strOnChangeFun = value; }
        }


        /// <summary>
        /// 离开时客户端脚本
        /// </summary>
        public string OnBlurScript
        {
            set { strOnBlurFun = value; }
        }

        /// <summary>
        /// 获得焦点时客户端脚本
        /// </summary>
        public string OnFocusScript
        {
            set { strOnFocusFun = value; }
        }


        /// <summary>
        /// 控件状态
        /// </summary>
        public eOA_FlowControlState ContralState
        {
            get
            {
                if (ViewState["eOA_FlowControlState"] != null)
                    return (eOA_FlowControlState)ViewState["eOA_FlowControlState"];
                else
                    return eOA_FlowControlState.eNormal;
            }
            set { ViewState["eOA_FlowControlState"] = value; }
        }

        /// <summary>
        /// 控件的值
        /// </summary>
        public string Value
        {
            get { return GetMyValues(); }
            set { sValue = value; txtText.Text = sValue; }
        }

        /// <summary>
        /// 宽度
        /// </summary>
        public Unit Width
        {
            set
            {
                if (value != null)
                {
                    txtText.Width = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetMyValues()
        {

            return StringTool.ParseHtmlForString(txtText.Text.Trim());
        }

        /// <summary>
        /// 设备备注框的行数
        /// </summary>
        public int Rows
        {
            set
            {
                this.txtText.Rows = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                txtText.Text = sValue;
                labCaption.Text = StringTool.ParseForHtml(txtText.Text);

                if (ContralState != eOA_FlowControlState.eNormal)
                {
                    labCaption.Visible = true;
                    txtText.Visible = false;
                    rWarning.Visible = false;
                    if (ContralState == eOA_FlowControlState.eHidden)
                    {
                        labCaption.Text = "--";
                    }
                }

                #region 设置字符的默认可输入长度 和 替换生成 控制脚本 - 2013-04-24 @孙绍棕

                if (MaxLength <= 0)
                {
                    MaxLength = 500;    // 字符的默认可输入长度
                }

                literalCharCount.Text = literalCharCount.Text.Replace("#MaxLength#", MaxLength.ToString()).Replace("#ContainerID#", txtText.ClientID);

                if (ContralState != eOA_FlowControlState.eNormal)
                {
                    literalCharCount.Visible = false;
                }

                #endregion
            }

            if (blnMust == true)
            {
                //添加公共校验基础资料
                string strThisMsg = "";
                strThisMsg = txtText.ClientID + ">" + strMessage;
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), txtText.ClientID, "<script>if(typeof(sarrvalidlist) == 'undefined'){ var sarrvalidlist = new Array(1);}  sarrvalidlist[0] = sarrvalidlist[0] + '|' + '" + strThisMsg + "';</script>");
            }

            if (blnMust == false)
            {
                rWarning.Visible = false;
            }

            if (strOnChangeFun.Length > 0)
            {
                txtText.Attributes.Add("onchange", strOnChangeFun);
            }

            if (strOnBlurFun.Length > 0)
            {
                txtText.Attributes.Add("onblur", strOnBlurFun);
            }

            if (strOnFocusFun.Length > 0)
            {
                txtText.Attributes.Add("onfocus", strOnFocusFun);
            }

        }

    }
}
