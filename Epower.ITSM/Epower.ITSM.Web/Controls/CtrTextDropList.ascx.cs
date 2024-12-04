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

namespace Epower.ITSM.Web.Controls
{
    public partial class CtrTextDropList : System.Web.UI.UserControl
    {


        /// <summary>
        /// txtText 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.WebControls.TextBox txtText;

        /// <summary>
        /// fieldsForDropdownLayer 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl fieldsForDropdownLayer;

        /// <summary>
        /// fieldsForDropdown 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlSelect fieldsForDropdown;


        #region 属性定义

        bool _IsOnTime = false;
        /// <summary>
        /// 选择来源于XMLHTTP异步获取时，是否根据输入项实时获取，缺省为false
        /// </summary>
        public bool OnTimeXmlHttp
        {
            get
            {
                if (ViewState[this.ID + "OnTimeXmlHttp"] == null)
                    return false;
                else
                    return bool.Parse(ViewState[this.ID + "OnTimeXmlHttp"].ToString());
            }
            set
            {
                ViewState[this.ID + "OnTimeXmlHttp"] = value;
                //动态加载的控件中ViewState可能不可用
                _IsOnTime = value;
            }
        }


        /// <summary>
        /// 选择项来源 0 页面隐含控件的值  1 来源于 XMLHTTP异步获取
        /// </summary>

        int _FieldSourceType = -1;
        public int FieldsSourceType
        {
            get
            {
                if (ViewState[this.ID + "FieldsSourceType"] == null)
                    return -1;
                else
                    return int.Parse(ViewState[this.ID + "FieldsSourceType"].ToString());
            }
            set {
                ViewState[this.ID + "FieldsSourceType"] = value;
                //动态加载的控件中ViewState可能不可用
                _FieldSourceType = value;
            }
        }

        /// <summary>
        /// 选择项ID,可能是隐含控件的ID,也可能是XMLHTTP异步获取的入口参数
        /// </summary>
        string _FieldSourceID = "";
        public string FieldsSourceID
        {
            get
            {
                if (ViewState[this.ID + "FieldsSourceID"] == null)
                    return "";
                else
                    return ViewState[this.ID + "FieldsSourceID"].ToString();
            }
            set { 
                ViewState[this.ID + "FieldsSourceID"] = value;
                //动态加载的控件中ViewState可能不可用
                _FieldSourceID = value;
            }
        }

        private string sValue = "";
        private string strOnBlurFun = "";
        private TextBoxMode sTextMode = TextBoxMode.SingleLine;

        /// <summary>
        /// 
        /// </summary>
        public TextBoxMode TextMode
        {
            get { return sTextMode; }
            set
            {
                sTextMode = value;
            }
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
            set { txtText.MaxLength = value; }
        }

        /// <summary>
        /// 控件宽度
        /// </summary>
        public string Width
        {
            set { txtText.Width = new Unit(value); }
        }

     

        /// <summary>
        /// 离开时客户端脚本
        /// </summary>
        public string OnBlurScript
        {
            set { strOnBlurFun = value; }
        }


      
        /// <summary>
        /// 控件的值
        /// </summary>
        public string Value
        {
            get { return GetMyValues(); }
            set { sValue = value;
            txtText.Text = sValue;
            labCaption.Text = value;
            }
        }

        private string GetMyValues()
        {

            return txtText.Text.Trim();
        }

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

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {

                txtText.Text = sValue;
                //txtText.TextMode = sTextMode;
                txtText.Attributes.Add("autocomplete", "off");              

                if (strOnBlurFun.Length > 0)
                {
                    txtText.Attributes.Add("onblur", strOnBlurFun);
                }

                
                
                if (this.FieldsSourceType == -1 )
                {
                    //动态加载的情况下,ViewState 可能不可用时,判断变量值
                    if (_FieldSourceType == 0)
                    {
                        //选择项来源于隐含控件
                        txtText.Attributes.Add("onfocus", "setInputID('" + txtText.ClientID + "');");
                        txtText.Attributes.Add("onblur", "setInputIDNone();");
                        txtText.Attributes.Add("onkeydown", "focusToDropDown('" + fieldsForDropdown.ClientID + "');");
                        txtText.Attributes.Add("onkeyup", "getItemsForDropdown('" + fieldsForDropdownLayer.ClientID + "','"
                                        + fieldsForDropdown.ClientID + "','" + txtText.ClientID
                                        + "','" + _FieldSourceID.Trim() + "',this);");
                        txtText.Attributes.Add("ondblclick", "getItemsForDropdownXmlHttp('" + fieldsForDropdownLayer.ClientID + "','"
                                            + fieldsForDropdown.ClientID + "','" + txtText.ClientID
                                            + "','" + _FieldSourceID.Trim() + "',this);");
                    }
                    else
                    {
                        if (this.OnTimeXmlHttp == false || _IsOnTime == false)
                        {
                            txtText.Attributes.Add("onfocus", "setInputID('" + txtText.ClientID + "');");
                            txtText.Attributes.Add("onblur", "setInputIDNone();");
                            txtText.Attributes.Add("onkeydown", "focusToDropDown('" + fieldsForDropdown.ClientID + "');");
                            txtText.Attributes.Add("onkeyup", "getItemsForDropdownXmlHttp('" + fieldsForDropdownLayer.ClientID + "','"
                                            + fieldsForDropdown.ClientID + "','" + txtText.ClientID
                                            + "','" + _FieldSourceID.Trim() + "',this);");
                            txtText.Attributes.Add("ondblclick", "getItemsForDropdownXmlHttp('" + fieldsForDropdownLayer.ClientID + "','"
                                            + fieldsForDropdown.ClientID + "','" + txtText.ClientID
                                            + "','" + _FieldSourceID.Trim() + "',this);");
                        }
                        else
                        {
                            //实时根据当前值 从后台 获取
                            txtText.Attributes.Add("onfocus", "setInputID('" + txtText.ClientID + "');");
                            txtText.Attributes.Add("onblur", "setInputIDNone();");
                            txtText.Attributes.Add("onkeydown", "focusToDropDown('" + fieldsForDropdown.ClientID + "');");
                            txtText.Attributes.Add("onkeyup", "getItemsForDropdownXmlHttpOnTime('" + fieldsForDropdownLayer.ClientID + "','"
                                            + fieldsForDropdown.ClientID + "','" + txtText.ClientID
                                            + "','" + _FieldSourceID.Trim() + "',this);");
                            txtText.Attributes.Add("ondblclick", "getItemsForDropdownXmlHttp('" + fieldsForDropdownLayer.ClientID + "','"
                                            + fieldsForDropdown.ClientID + "','" + txtText.ClientID
                                            + "','" + _FieldSourceID.Trim() + "',this);");
                        }
                    }
                    
                    
 
                }
                else
                {
                    if (this.FieldsSourceType == 0)
                    {
                        //选择项来源于隐含控件
                        txtText.Attributes.Add("onfocus", "setInputID('" + txtText.ClientID + "');");
                        txtText.Attributes.Add("onblur", "setInputIDNone();");
                        txtText.Attributes.Add("onkeydown", "focusToDropDown('" + fieldsForDropdown.ClientID + "');");
                        txtText.Attributes.Add("onkeyup", "getItemsForDropdown('" + fieldsForDropdownLayer.ClientID + "','"
                                        + fieldsForDropdown.ClientID + "','" + txtText.ClientID
                                        + "','" + this.FieldsSourceID + "',this);");
                    }
                    else
                    {
                        if (this.OnTimeXmlHttp == false || _IsOnTime == false)
                        {
                            txtText.Attributes.Add("onfocus", "setInputID('" + txtText.ClientID + "');");
                            txtText.Attributes.Add("onblur", "setInputIDNone();");
                            txtText.Attributes.Add("onkeydown", "focusToDropDown('" + fieldsForDropdown.ClientID + "');");
                            txtText.Attributes.Add("onkeyup", "getItemsForDropdownXmlHttp('" + fieldsForDropdownLayer.ClientID + "','"
                                        + fieldsForDropdown.ClientID + "','" + txtText.ClientID
                                        + "','" + this.FieldsSourceID + "',this);");
                            txtText.Attributes.Add("ondblclick", "getItemsForDropdownXmlHttp('" + fieldsForDropdownLayer.ClientID + "','"
                                            + fieldsForDropdown.ClientID + "','" + txtText.ClientID
                                            + "','" + _FieldSourceID.Trim() + "',this);");
                        }
                        else
                        {
                            txtText.Attributes.Add("onfocus", "setInputID('" + txtText.ClientID + "');SetInputCheckValid('" + fieldsForDropdownLayer.ClientID + "','"
                                        + fieldsForDropdown.ClientID + "','" + txtText.ClientID
                                        + "','" + this.FieldsSourceID + "',this);");
                            txtText.Attributes.Add("onblur", "setInputIDNone();SetInputCheckDisable();");
                            txtText.Attributes.Add("onkeydown", "focusToDropDown('" + fieldsForDropdown.ClientID + "');");
                            //txtText.Attributes.Add("onkeyup", "getItemsForDropdownXmlHttpOnTime('" + fieldsForDropdownLayer.ClientID + "','"
                            //            + fieldsForDropdown.ClientID + "','" + txtText.ClientID
                            //            + "','" + this.FieldsSourceID + "',this);");
                           
                        }
                        
                    }
                }
                fieldsForDropdownLayer.Attributes.Add("onmouseout", "hideMe('" + fieldsForDropdownLayer.ClientID + "','none');");

                fieldsForDropdownLayer.Attributes.Add("onmouseover", "MoverToDropDownLayer('" + fieldsForDropdown.ClientID + "');");

                

                fieldsForDropdown.Attributes.Add("onkeydown", "selectOnReturn('" + fieldsForDropdownLayer.ClientID + "','"
                                        + txtText.ClientID + "',this);");
                fieldsForDropdown.Attributes.Add("onclick", "getSelectedLabel('" + fieldsForDropdownLayer.ClientID + "','"
                                        + txtText.ClientID + "',this);");
              
            }

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


            if (blnMust == true)
            {
                //添加公共校验基础资料
                string strThisMsg = "";
                strThisMsg = txtText.ClientID + ">" + strMessage;
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), txtText.ClientID, "<script type=\"text/javascript\" >if(typeof(sarrvalidlist) == 'undefined'){ var sarrvalidlist = new Array(1);}  sarrvalidlist[0] = sarrvalidlist[0] + '|' + '" + strThisMsg + "';</script>");
            }


            #region 设置文本类型 余向前 2013-04-19
            txtText.TextMode = sTextMode;
            #endregion

            if (blnMust == true)
            {
                rWarning.Visible = true;
            }
            else
            {
                rWarning.Visible = false;
            }


           


            
        }
    }
}