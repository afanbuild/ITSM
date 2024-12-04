/*******************************************************************
 * 版权所有：
 * Description：时间日期控件
 * 
 * 
 * Create By  ：于向前 
 * 
 * 
 * Modified: 孙绍棕
 * Summary: 重构代码，移除了无效的历史代码
 * 
 * Modified Time: 2013-07-09 15:12
 * 
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
    public partial class CtrDateAndTimeV2 : System.Web.UI.UserControl
    {
        #region 属性定义

        /// <summary>
        /// 可见可编辑状态, 默认可编辑 ( cState = eOA_FlowControlState.eNormal )
        /// </summary>
        private eOA_FlowControlState cState = eOA_FlowControlState.eNormal;

        /// <summary>
        /// 可见可编辑状态
        /// </summary>
        public eOA_FlowControlState ContralState
        {
            get
            {
                return cState;
            }
            set
            {
                cState = value;
            }
        }

        /// <summary>
        /// 是否必须输入, 默认 False ( blnMust = false )
        /// </summary>
        private bool blnMust = false;

        /// <summary>
        /// 是否必须输入
        /// </summary>
        public bool MustInput
        {
            set
            {
                blnMust = value;
            }
            get
            {
                return blnMust;
            }
        }

        /// <summary>
        /// 是否显示时间, 默认 True ( blnShowTime = true )
        /// </summary>
        private bool blnShowTime = true;

        /// <summary>
        /// 是否显示时间
        /// </summary>
        public bool ShowTime
        {
            set { blnShowTime = value; }
            get { return blnShowTime; }
        }

        private string strMessage = "";
        /// <summary>
        /// 设置未输入时的提示信息

        /// </summary>
        public string TextToolTip
        {
            set { strMessage = value; }
        }

        /// <summary>
        /// 取 DateTime 类型的设置值
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
                    return DateTime.Now;    // 默认返回当前时间
                }
            }
            set
            {
                //dTime = value;
                //txtDate.Text = dTime.ToString("yyyy-MM-dd HH:mm:ss");

                if (blnShowTime)
                {
                    if (blnShowMinute)
                    {
                        txtDate.Text = value.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    else
                    {
                        txtDate.Text = value.ToString("yyyy-MM-dd HH");
                    }
                }
                else
                {
                    txtDate.Text = value.ToString("yyyy-MM-dd");
                }
            }
        }

        /// <summary>
        /// 取值的字符串形式
        /// </summary>
        public string dateTimeString
        {
            get
            {
                String strOrgiDateTime = txtDate.Text;

                if (String.IsNullOrEmpty(strOrgiDateTime))
                {
                    return String.Empty;    // 没有取到值
                }

                if (blnShowTime == true)
                {
                    if (blnShowMinute)
                    {
                        return DateTime.Parse(strOrgiDateTime).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    else
                    {
                        return DateTime.Parse(strOrgiDateTime + ":00:00").ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }
                else
                {
                    return DateTime.Parse(strOrgiDateTime).ToString("yyyy-MM-dd");
                }
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    txtDate.Text = String.Empty;
                }
                else
                {
                    if (blnShowTime)
                    {
                        if (blnShowMinute)
                        {
                            txtDate.Text = DateTime.Parse(value).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else
                        {
                            txtDate.Text = DateTime.Parse(value).ToString("yyyy-MM-dd HH");
                        }
                    }
                    else
                    {
                        txtDate.Text = DateTime.Parse(value).ToString("yyyy-MM-dd");
                    }
                }
            }
        }

        /// <summary>
        /// 是否显示分钟, 默认 True ( blnShowMinute = true )
        /// </summary>
        private bool blnShowMinute = true;

        /// <summary>
        /// 是否显示分钟
        /// </summary>
        public bool ShowMinute
        {
            set
            {
                blnShowMinute = value;
            }
            get
            {
                return blnShowMinute;
            }
        }

        /// <summary>
        /// 是否容许显示空日期, 默认 False ( blnAllowNull = false )
        /// </summary>
        private bool blnAllowNull = false;

        /// <summary>
        /// 是否容许显示空日期[如果容许显示空日期,但实际未赋值,dateTime属性需要特殊处理]
        /// </summary>
        public bool AllowNull
        {
            set { blnAllowNull = value; }
            get { return blnAllowNull; }
        }

        /// <summary>
        /// 是否只读, 默认 False ( mReadOnly = false )
        /// </summary>
        private bool mReadOnly = false;

        /// <summary>
        /// 是否只读
        /// </summary>
        public bool ReadOnly
        {
            get
            {
                return mReadOnly;
            }
            set
            {
                mReadOnly = value;
            }
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

        /// <summary>
        /// [已废弃]
        /// </summary>
        public String OnChangeScript
        { get; set; }


        #endregion

        #region 方法区

        /// <summary>
        /// 页面加载时调用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (blnShowTime)
            {
                if (blnShowMinute)
                {
                    txtDate.Attributes.Add("onClick", "WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})");
                }
                else
                {
                    txtDate.Attributes.Add("onClick", "WdatePicker({dateFmt:'yyyy-MM-dd HH'})");
                }
            }
            else
            {
                txtDate.Attributes.Add("onClick", "WdatePicker({dateFmt:'yyyy-MM-dd'})");
            }

            //txtDate.Attributes.Add("onfocus", "WdatePicker({skin:'blue'});");
            if (!Page.IsPostBack)
            {
                if (ReadOnly)
                {
                    txtDate.ReadOnly = true;
                }
            }

            if (txtDate.Text.Trim() == string.Empty)
            {
                labDate.Text = String.Empty;
            }
            else
            {
                labDate.Text = txtDate.Text;
            }

            if (cState != eOA_FlowControlState.eNormal)
            {
                labDate.Visible = true;
                txtDate.Visible = false;

                if (cState == eOA_FlowControlState.eHidden)
                {
                    labDate.Text = "--";
                }
            }

            if (blnMust == false || ContralState == eOA_FlowControlState.eReadOnly || ContralState == eOA_FlowControlState.eHidden)
            {
                rWarning.Visible = false;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);

            if (blnMust == true)
            {
                //添加公共校验基础资料
                string strThisMsg = "";
                strThisMsg = txtDate.ClientID + ">" + strMessage + ">";
                Response.Write("<script>if(typeof(sarrvalidlist) == 'undefined'){ var sarrvalidlist = new Array(1);}  sarrvalidlist[0] = sarrvalidlist[0] + '|' + '" + strThisMsg + "';</script>");

            }
        }

        #endregion
    }
}