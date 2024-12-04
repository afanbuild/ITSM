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
        /// txtText �ؼ���
        /// </summary>
        /// <remarks>
        /// �Զ����ɵ��ֶΡ�
        /// Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
        /// </remarks>
        protected global::System.Web.UI.WebControls.TextBox txtText;

        /// <summary>
        /// fieldsForDropdownLayer �ؼ���
        /// </summary>
        /// <remarks>
        /// �Զ����ɵ��ֶΡ�
        /// Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl fieldsForDropdownLayer;

        /// <summary>
        /// fieldsForDropdown �ؼ���
        /// </summary>
        /// <remarks>
        /// �Զ����ɵ��ֶΡ�
        /// Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlSelect fieldsForDropdown;


        #region ���Զ���

        bool _IsOnTime = false;
        /// <summary>
        /// ѡ����Դ��XMLHTTP�첽��ȡʱ���Ƿ����������ʵʱ��ȡ��ȱʡΪfalse
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
                //��̬���صĿؼ���ViewState���ܲ�����
                _IsOnTime = value;
            }
        }


        /// <summary>
        /// ѡ������Դ 0 ҳ�������ؼ���ֵ  1 ��Դ�� XMLHTTP�첽��ȡ
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
                //��̬���صĿؼ���ViewState���ܲ�����
                _FieldSourceType = value;
            }
        }

        /// <summary>
        /// ѡ����ID,�����������ؼ���ID,Ҳ������XMLHTTP�첽��ȡ����ڲ���
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
                //��̬���صĿؼ���ViewState���ܲ�����
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
        /// �ı��ؼ��Ŀͻ���ID
        /// </summary>
        public string ClientID
        {
            get { return txtText.ClientID; }
        }

        /// <summary>
        /// ��󳤶�
        /// </summary>
        public int MaxLength
        {
            set { txtText.MaxLength = value; }
        }

        /// <summary>
        /// �ؼ����
        /// </summary>
        public string Width
        {
            set { txtText.Width = new Unit(value); }
        }

     

        /// <summary>
        /// �뿪ʱ�ͻ��˽ű�
        /// </summary>
        public string OnBlurScript
        {
            set { strOnBlurFun = value; }
        }


      
        /// <summary>
        /// �ؼ���ֵ
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
        /// �Ƿ�һ��Ҫ����
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
        /// ����δ����ʱ����ʾ��Ϣ
        /// </summary>
        public string TextToolTip
        {
            set { strMessage = value; }
        }


        /// <summary>
        /// �ؼ�״̬
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
                    //��̬���ص������,ViewState ���ܲ�����ʱ,�жϱ���ֵ
                    if (_FieldSourceType == 0)
                    {
                        //ѡ������Դ�������ؼ�
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
                            //ʵʱ���ݵ�ǰֵ �Ӻ�̨ ��ȡ
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
                        //ѡ������Դ�������ؼ�
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
                //��ӹ���У���������
                string strThisMsg = "";
                strThisMsg = txtText.ClientID + ">" + strMessage;
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), txtText.ClientID, "<script type=\"text/javascript\" >if(typeof(sarrvalidlist) == 'undefined'){ var sarrvalidlist = new Array(1);}  sarrvalidlist[0] = sarrvalidlist[0] + '|' + '" + strThisMsg + "';</script>");
            }


            #region �����ı����� ����ǰ 2013-04-19
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