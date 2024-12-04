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

namespace Epower.ITSM.Web.Controls
{
    using System;
    using System.Data;
    using System.Drawing;
    using System.Web;
    using System.Xml;
    using System.Web.UI.WebControls;
    using System.Web.UI.HtmlControls;
    using EpowerGlobal;
    using EpowerCom;
    using Epower.DevBase.BaseTools;
    using Epower.ITSM.SqlDAL;

    public partial class ctrmessagequery_Hastodoitem : System.Web.UI.UserControl
    {
        #region 获取或设置所选的下拉应用
        public long AppID
        {
            get
            {
                if (cboApp == null || cboApp.SelectedItem == null)
                {
                    return -1;
                }
                else
                {
                    return long.Parse(cboApp.SelectedItem.Value.ToString());
                }
            }
            set
            {
                if (cboApp != null)
                {
                    cboApp.SelectedIndex = cboApp.Items.IndexOf(cboApp.Items.FindByValue(value.ToString()));
                }
            }
        }
        #endregion

        long lngUserDeptID = 0;
        long lngUserID = 0;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            lngUserDeptID = StringTool.String2Long(Session["UserDeptID"].ToString());
            lngUserID = StringTool.String2Long(Session["UserID"].ToString());

            imgBegin.Attributes.Add("onclick", "fPopUpDlg('../Controls/Calendar/calendar.htm',document.all." + txtProcessBegin.ClientID + ", 'winpop', 234, 261);return false");
            imgEnd.Attributes.Add("onclick", "fPopUpDlg('../Controls/Calendar/calendar.htm',document.all." + txtProcessEnd.ClientID + ", 'winpop', 234, 261);return false");

            imgSBegin.Attributes.Add("onclick", "fPopUpDlg('../Controls/Calendar/calendar.htm',document.all." + txtMsgDateBegin.ClientID + ", 'winpop', 234, 261);return false");
            imgEEnd.Attributes.Add("onclick", "fPopUpDlg('../Controls/Calendar/calendar.htm',document.all." + txtMsgDateEnd.ClientID + ", 'winpop', 234, 261);return false");

            if (!IsPostBack)
            {
                cboStatus.Items.Add(new ListItem("所有状态", "-1"));
                cboStatus.Items.Add(new ListItem("--已处理", ((int)e_MessageStatus.emsFinished).ToString()));
                cboStatus.Items.Add(new ListItem("--待处理", ((int)e_MessageStatus.emsHandle).ToString()));
                //cboStatus.Items.Add(new ListItem("--正常结束", ((int)e_MessageStatus.emsFinished).ToString()));
                cboStatus.Items.Add(new ListItem("--挂起", ((int)e_MessageStatus.emsWaiting).ToString()));
                cboStatus.Items.Add(new ListItem("--暂停", ((int)e_MessageStatus.emsStop).ToString()));
                cboStatus.Items[1].Selected = true;

                cboMsgRange.Items.Add(new ListItem("个人", "0"));
                cboMsgRange.Items.Add(new ListItem("部门", "1"));
                cboMsgRange.Items.Add(new ListItem("公司", "2"));
                cboMsgRange.Items[0].Selected = true;

                //设置起始日期
                string sQueryBeginDate = "0";
                if (CommonDP.GetConfigValue("Other", "QueryBeginDate") != null)
                    sQueryBeginDate = CommonDP.GetConfigValue("Other", "QueryBeginDate").ToString();
                if (sQueryBeginDate == "0")
                {
                    txtMsgDateBegin.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                }
                else
                {
                    txtMsgDateBegin.Text = DateTime.Parse(sQueryBeginDate).ToString("yyyy-MM-dd");
                }
                txtMsgDateEnd.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");


                cboApp.DataSource = epApp.GetAllApps().DefaultView;
                cboApp.DataTextField = "AppName";
                cboApp.DataValueField = "AppID";
                cboApp.DataBind();

                ListItem itm = new ListItem("[所有应用]", "-1");
                cboApp.Items.Insert(0, itm);
                cboApp.SelectedIndex = 0;


            }

        }

        #region GetAllValues

        public XmlDocument GetAllValues()
        {
            XmlDocument xmlDoc = new XmlDocument();

            XmlElement xmlRoot = xmlDoc.CreateElement("Fields");

            XmlElement xmlEle;

            #region Status
            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "Status");
            if (cboStatus == null || cboStatus.SelectedItem == null)
            {
                xmlEle.SetAttribute("Value", ((int)e_MessageStatus.emsFinished).ToString());
            }
            else
            {
                xmlEle.SetAttribute("Value", cboStatus.SelectedItem.Value.ToString());
            }
            xmlRoot.AppendChild(xmlEle);
            #endregion

            #region App
            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "AppID");
            if (cboApp == null || cboApp.SelectedItem == null)
            {
                xmlEle.SetAttribute("Value", "-1");
            }
            else
            {
                xmlEle.SetAttribute("Value", cboApp.SelectedItem.Value.ToString());
            }
            xmlRoot.AppendChild(xmlEle);
            #endregion

            #region Subject
            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "Subject");
            if (txtSubject == null)
            {
                xmlEle.SetAttribute("Value", "");
            }
            else
            {
                xmlEle.SetAttribute("Value", txtSubject.Text);
            }
            xmlRoot.AppendChild(xmlEle);
            #endregion

            #region ProcessDate
            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "ProcessBegin");
            xmlEle.SetAttribute("Value", txtProcessBegin.Text);

            xmlRoot.AppendChild(xmlEle);
            #endregion

            #region ProcessEnd
            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "ProcessEnd");
            xmlEle.SetAttribute("Value", txtProcessEnd.Text);

            xmlRoot.AppendChild(xmlEle);
            #endregion

            #region MessageBegin
            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "MessageBegin");
            if (txtMsgDateBegin == null || txtMsgDateBegin.Text.Equals(""))
            {
                string sQueryBeginDate = "0";
                if (CommonDP.GetConfigValue("Other", "QueryBeginDate") != null)
                    sQueryBeginDate = CommonDP.GetConfigValue("Other", "QueryBeginDate").ToString();
                if (sQueryBeginDate == "0")
                {
                    xmlEle.SetAttribute("Value", DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd"));
                }
                else
                {
                    xmlEle.SetAttribute("Value", sQueryBeginDate);
                }
            }
            else
            {
                xmlEle.SetAttribute("Value", txtMsgDateBegin.Text);
            }
            xmlRoot.AppendChild(xmlEle);
            #endregion

            #region MessageEnd
            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "MessageEnd");
            if (txtMsgDateEnd == null || txtMsgDateEnd.Text.Equals(""))
            {
                xmlEle.SetAttribute("Value", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
            }
            else
            {
                xmlEle.SetAttribute("Value", txtMsgDateEnd.Text);
            }

            xmlRoot.AppendChild(xmlEle);
            #endregion

            #region UserID
            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "UserID");
            xmlEle.SetAttribute("Value", Session["UserID"].ToString());
            xmlRoot.AppendChild(xmlEle);
            #endregion

            #region UserDeptID
            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "UserDeptID");
            xmlEle.SetAttribute("Value", Session["UserDeptID"].ToString());
            xmlRoot.AppendChild(xmlEle);
            #endregion

            xmlDoc.AppendChild(xmlRoot);
            return xmlDoc;
        }
        #endregion


        #region Web 窗体设计器生成的代码
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///		设计器支持所需的方法 - 不要使用代码编辑器
        ///		修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion
    }
}