/*******************************************************************
 * ��Ȩ���У�
 * Description��ʱ�����ڿؼ�
 * 
 * 
 * Create By  ��
 * Create Date��
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
    /// <summary>
    ///		CtrDateAndTime ��ժҪ˵����
    /// </summary>
    public partial class CtrDateAndTime : System.Web.UI.UserControl
    {

        #region ����������
        private DateTime dTime = DateTime.Now;

        private string strOnChangeFun = "";

        private bool blnShowTime = true;

        private bool blnAllowNull = false;
        private eOA_FlowControlState cState = eOA_FlowControlState.eNormal;
        private bool blnShowMinute = true;

        private bool blnShowYear = true;
        #endregion

        #region ���Զ���
        public eOA_FlowControlState ContralState
        {
            get { return cState; }
            set { cState = value; }
        }
        /// <summary>
        /// ���ؿؼ���ֵ
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
                    return DateTime.Now;
                }
            }
            set
            {
                dTime = value;                
                txtDate.Text = dTime.ToString("yyyy-MM-dd");
                ddlHours.SelectedIndex = ddlHours.Items.IndexOf(ddlHours.Items.FindByValue(dTime.Hour.ToString()));
                ddlMinutes.SelectedIndex = ddlMinutes.Items.IndexOf(ddlMinutes.Items.FindByValue(dTime.Minute.ToString()));
             
            }
        }
        /// <summary>
        /// ���ؿؼ�ֵ���ַ�����ʽ
        /// </summary>
        public string dateTimeString
        {
            get { return GetMyDateTime(); }
            set
            {
                string svalue = value;
              
                DateTime datetime = DateTime.Now;
                if (svalue == string.Empty)
                {
                    txtDate.Text = string.Empty;
                }
                else
                {
                    datetime = DateTime.Parse(svalue);
                    txtDate.Text = datetime.ToString("yyyy-MM-dd");
                }
            }
        }
        /// <summary>
        /// ���ڸ���ʱ�ͻ��˽ű�
        /// </summary>
        public string OnChangeScript
        {
            set { strOnChangeFun = value; }
        }
        /// <summary>
        /// �Ƿ���ʾʱ�䲿��
        /// </summary>
        public bool ShowTime
        {
            set { blnShowTime = value; }
            get { return blnShowTime; }
        }
        /// <summary>
        /// �Ƿ���ʾ����
        /// </summary>
        public bool ShowMinute
        {
            set { blnShowMinute = value; }
            get { return blnShowMinute; }
        }

        /// <summary>
        /// �Ƿ�������ʾ������[���������ʾ������,��ʵ��δ��ֵ,dateTime������Ҫ���⴦��]
        /// </summary>
        public bool AllowNull
        {
            set { blnAllowNull = value; }
            get { return blnAllowNull; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetMyDateTime()
        {
            string strDateTime;

            try
            {
                DateTime.Parse(txtDate.Text.Trim());
            }
            catch
            {
                txtDate.Text = "";
            }

            if (blnShowTime == true)
            {
                strDateTime = txtDate.Text + " " + ddlHours.SelectedValue.PadLeft(2, char.Parse("0")) + ":" + ddlMinutes.SelectedValue.PadLeft(2, char.Parse("0"));
            }
            else
            {
                strDateTime = txtDate.Text;
            }
            if (txtDate.Text.Trim() == string.Empty)
            {
                strDateTime = string.Empty;
            }
            return strDateTime;
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
        private bool blnMust = false;
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
        private string strMessage = "";
        /// <summary>
        /// ����δ����ʱ����ʾ��Ϣ
        /// </summary>
        public string TextToolTip
        {
            set { strMessage = value; }
        }
        #endregion

        #region ������
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            if (Page.IsPostBack == false)
            {
                if (blnMust == true)
                {
                    //��ӹ���У���������
                    string strThisMsg = "";
                    strThisMsg = txtDate.ClientID + ">" + strMessage + ">";                    
                    Response.Write("<script>if(typeof(sarrvalidlist) == 'undefined'){ var sarrvalidlist = new Array(1);}  sarrvalidlist[0] = sarrvalidlist[0] + '|' + '" + strThisMsg + "';</script>");
           
                }
             
               
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            txtDate.Attributes.Add("onfocus", "WdatePicker({skin:'blue'});");
            if (Page.IsPostBack == false)
            {
                InitPage();
            }
            else     //�������
            {
                if (Disparity)
                {
                    InitPage();
                }
            }
            txtDate.Attributes.Add("onblur", "if(isDate(document.all." + txtDate.ClientID + ".value) == false){document.all." + txtDate.ClientID + ".value = '" + txtDate.Text + "' };");
            if (strOnChangeFun.Length > 0)
            {
                txtDate.Attributes.Add("onchange", strOnChangeFun);
                ddlHours.Attributes.Add("onchange", strOnChangeFun);
                ddlMinutes.Attributes.Add("onchange", strOnChangeFun);
            }
            if (blnMust == false || ContralState == eOA_FlowControlState.eReadOnly || ContralState == eOA_FlowControlState.eHidden)
            {
                rWarning.Visible = false;
            }     
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitPage()
        {
            if (blnAllowNull == false)
            {
                if (txtDate.Text.Trim() != string.Empty)
                {
                    dateTime = Convert.ToDateTime(GetMyDateTime());
                    txtDate.Text = dTime.ToString("yyyy-MM-dd");
                    labDate.Text = txtDate.Text;
                }
                else
                {
                    txtDate.Text = "";
                    labDate.Text = "";
                }
            }
            else
            {
                txtDate.Text = "";
                labDate.Text = "";
            }

            if (blnShowTime == true)
            {
                for (int i = 0; i < 24; i++)
                {
                    if (i == dTime.Hour)
                    {
                        ddlHours.SelectedIndex = i;
                    }
                }
                for (int i = 0; i < 60; i++)
                {
                    if (i == dTime.Minute)
                    {
                        ddlMinutes.SelectedIndex = i;
                    }
                }

                if (blnShowMinute == true)
                {
                    labDate.Text = txtDate.Text + ((txtDate.Text.Equals(string.Empty)) ? "" : " " + ddlHours.SelectedValue.PadLeft(2, char.Parse("0")) + ":" + ddlMinutes.SelectedValue.PadLeft(2, char.Parse("0")));
                }
                else
                {
                    ddlMinutes.Visible = false;
                    ddlMinutes.SelectedIndex = 0;
                    labDate.Text = txtDate.Text + ((txtDate.Text.Equals(string.Empty)) ? "" : " " + ddlHours.SelectedValue.PadLeft(2, char.Parse("0")));
                }
            }
            else
            {
                ddlHours.Visible = false;
                ddlMinutes.Visible = false;
            }

            if (cState != eOA_FlowControlState.eNormal)
            {
                labDate.Visible = true;
                txtDate.Visible = false;
               // imgDate.Visible = false;
                ddlHours.Visible = false;
                ddlMinutes.Visible = false;
                if (cState == eOA_FlowControlState.eHidden)
                {
                    labDate.Text = "--";
                }
            }

        }
        #endregion

        #region Web ������������ɵĴ���
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: �õ����� ASP.NET Web ���������������ġ�
            //
            InitializeComponent();
            base.OnInit(e);

            for (int i = 0; i < 24; i++)
            {
                ddlHours.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            for (int i = 0; i < 60; i++)
            {
                ddlMinutes.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
        }

        /// <summary>
        ///		�����֧������ķ��� - ��Ҫʹ�ô���༭��
        ///		�޸Ĵ˷��������ݡ�
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion
    }
}
