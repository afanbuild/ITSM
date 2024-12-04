/****************************************************************************
 * 
 * description:Drm系统用户选择
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-08-18
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
using System.Text;
using System.Xml;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.Common
{
    /// <summary>
    /// 
    /// </summary>
    public partial class frmDRMUserSelectajax : BasePage
    {
        RightEntity reTrace = null;  //权限
        private StringBuilder sbSetOpenerText = new StringBuilder();

        public string Opener_ClientId
        {
            set
            {
                ViewState["Opener_ClientId"] = value;
            }
            get
            {
                return (ViewState["Opener_ClientId"] == null) ? "" : ViewState["Opener_ClientId"].ToString();
            }
        }


        #region 是否查询
        /// <summary>
        /// 
        /// </summary>
        protected bool IsSelect
        {
            get
            {
                if (Request["IsSelect"] != null && Request["IsSelect"] == "true")
                    return true;
                else
                    return false;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        protected string FlowID
        {
            get { if (Request["FlowID"] != null) return Request["FlowID"].ToString(); else return "0"; }
        }
        /// <summary>
        /// 客户名称
        /// </summary>
        protected string CustName
        {
            get
            {
                if (ViewState["CustName"] != null)
                {
                    return ViewState["CustName"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState["CustName"] = value;
            }
        }
        /// <summary>
        /// 地址
        /// </summary>
        protected string CustAddress
        {
            get
            {
                if (ViewState["CustAddress"] != null)
                {
                    return ViewState["CustAddress"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState["CustAddress"] = value;
            }
        }
        public string TypeFrm 
        {
            get 
            {
                if (Request["TypeFrom"] != null)
                {
                    return Request["TypeFrom"].ToString();
                }
                else 
                {
                    return string.Empty;
                }
            
            }
        }
        /// <summary>
        /// 联系人
        /// </summary>
        protected string CustLinkMan
        {
            get
            {
                if (ViewState["CustLinkMan"] != null)
                {
                    return ViewState["CustLinkMan"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState["CustLinkMan"] = value;
            }
        }
        /// <summary>
        /// 联系电话
        /// </summary>
        protected string CustTel
        {
            get
            {
                if (ViewState["CustTel"] != null)
                {
                    return ViewState["CustTel"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState["CustTel"] = value;
            }
        }

        public string RequestPageType
        {
            set { ViewState["RequestPageType"] = value; }
            get { return ViewState["RequestPageType"].ToString(); }
        }

        #region 设置父窗体按钮事件SetParentButtonEvent
        /// <summary>
        /// 设置父窗体按钮事件
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.ShowQueryButton(true);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtonEvent();
            cpfECustomerInfo.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(Bind);
            if (!IsPostBack)
            {
                if (Request["Opener_ClientId"] != null)
                {
                    Opener_ClientId = Request["Opener_ClientId"].ToString().Replace("hidClientId_ForOpenerPage", "");
                }
                RequestPageType = (Request["PageType"] == null) ? "1" : Request["PageType"].ToString();
                //设置显示
                PageDeal.SetLanguage(this.Controls[0]);
                SetHeaderText();
                
                if (Request["CustName"] != null)
                {
                    txtName.Text = Request["CustName"].ToString();
                    CustName = Request["CustName"].ToString();
                }
                if (Request["CustAddress"] != null)
                {
                    CustAddress = Request["CustAddress"].ToString();
                }
                if (Request["CustLinkMan"] != null)
                {
                    CustLinkMan = Request["CustLinkMan"].ToString();
                }
                if (Request["CustTel"] != null)
                {
                    CustTel = Request["CustTel"].ToString();
                }

                if (Request["MastCust"] != null)
                {
                    hidMastCustID.Value = Request["MastCust"].ToString();
                }
                InitDropDownList();
                Bind();
            }
        }

        /// <summary>
        /// 设置列头名称 余向前 2013-05-24
        /// </summary>
        void SetHeaderText()
        {
            dgdrmuser.Columns[1].HeaderText = PageDeal.GetLanguageValue("Custom_MastCustName");
            dgdrmuser.Columns[2].HeaderText = PageDeal.GetLanguageValue("Custom_CustomerType");
            dgdrmuser.Columns[3].HeaderText = PageDeal.GetLanguageValue("Custom_CustName");
            dgdrmuser.Columns[4].HeaderText = PageDeal.GetLanguageValue("Custom_Contact");
            dgdrmuser.Columns[5].HeaderText = PageDeal.GetLanguageValue("Custom_CTel");
            dgdrmuser.Columns[6].HeaderText = PageDeal.GetLanguageValue("Custom_CustEmail");
            dgdrmuser.Columns[7].HeaderText = PageDeal.GetLanguageValue("Custom_CustomCode");
            dgdrmuser.Columns[8].HeaderText = PageDeal.GetLanguageValue("Custom_CustAddress");
        }


        /// <summary>
        /// 绑定服务单位
        /// </summary>
        private void InitDropDownList()
        {
            Br_MastCustomerDP ee = new Br_MastCustomerDP();
            string sWhere = string.Empty;
            string sOrder = string.Empty;
            DataTable dt = ee.GetDataTable(sWhere, sOrder);
            ddltMastCustID.DataSource = dt;
            ddltMastCustID.DataTextField = "ShortName";
            ddltMastCustID.DataValueField = "ID";
            ddltMastCustID.DataBind();
            ddltMastCustID.Items.Insert(0, new ListItem("", ""));

            ddltMastCustID.SelectedIndex = ddltMastCustID.Items.IndexOf(ddltMastCustID.Items.FindByValue(hidMastCustID.Value));
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Master_Master_Button_Query_Click()
        {
            Bind();
        }

        #region  生成查询XML字符串 GetXmlValue
        /// <summary>
        /// 生成查询XML字符串

        /// </summary>
        /// <returns></returns>
        private XmlDocument GetXmlValue()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlRoot = xmlDoc.CreateElement("Fields");
            XmlElement xmlEle;
            #region MastCustID
            if (ddltMastCustID.SelectedItem.Text.Trim() != string.Empty)
            {
                xmlEle = xmlDoc.CreateElement("Field");
                xmlEle.SetAttribute("FieldName", "MastCustID");
                xmlEle.SetAttribute("Value", ddltMastCustID.SelectedValue.Trim());
                xmlRoot.AppendChild(xmlEle);
            }
            #endregion
            #region CustomerType
            if (ctrFCDServiceType.CatelogValue.ToString() != string.Empty)
            {
                xmlEle = xmlDoc.CreateElement("Field");
                xmlEle.SetAttribute("FieldName", "CustomerType");
                xmlEle.SetAttribute("Value", ctrFCDServiceType.CatelogID.ToString().Trim());
                xmlRoot.AppendChild(xmlEle);
            }
            #endregion
            #region client_name
            if (txtName.Text != "")
            {
                xmlEle = xmlDoc.CreateElement("Field");
                xmlEle.SetAttribute("FieldName", "client_name");
                xmlEle.SetAttribute("Value", txtName.Text.Replace("'", "''"));
                xmlRoot.AppendChild(xmlEle);
            }
            #endregion
            #region client_address
            if (txtaddress.Text != "")
            {
                xmlEle = xmlDoc.CreateElement("Field");
                xmlEle.SetAttribute("FieldName", "client_address");
                xmlEle.SetAttribute("Value", txtaddress.Text.Replace("'", "''"));
                xmlRoot.AppendChild(xmlEle);
            }
            #endregion
            #region client_contact
            if (txtcontract.Text != "")
            {
                xmlEle = xmlDoc.CreateElement("Field");
                xmlEle.SetAttribute("FieldName", "client_contact");
                xmlEle.SetAttribute("Value", txtcontract.Text.Replace("'", "''"));
                xmlRoot.AppendChild(xmlEle);
            }
            #endregion
            #region client_phone
            if (txttel.Text != "")
            {
                xmlEle = xmlDoc.CreateElement("Field");
                xmlEle.SetAttribute("FieldName", "client_phone");
                xmlEle.SetAttribute("Value", txttel.Text.Replace("'", "''"));
                xmlRoot.AppendChild(xmlEle);
            }
            #endregion
            #region txtCustomCode
            if (txtCustomCode.Text != "")
            {
                xmlEle = xmlDoc.CreateElement("Field");
                xmlEle.SetAttribute("FieldName", "customcode");
                xmlEle.SetAttribute("Value", txtCustomCode.Text.Replace("'", "''"));
                xmlRoot.AppendChild(xmlEle);
            }
            #endregion
            xmlDoc.AppendChild(xmlRoot);

            return xmlDoc;
        }
        #endregion

        #region 数据加载 Bind
        /// <summary>
        /// 数据加载
        /// </summary>
        private void Bind()
        {
            int iRowCount = 0;
            string sServiceCustom = "1";
            DataTable dt;
            XmlDocument xmlDoc = GetXmlValue();

            //writelog(DateTime.Now.ToString());

            DRMUserDP ee = new DRMUserDP();
            dt = ee.GetDRMUser(xmlDoc.InnerXml, long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString())
           , long.Parse(Session["UserOrgID"].ToString()), reTrace, sServiceCustom
           , this.cpfECustomerInfo.PageSize, this.cpfECustomerInfo.CurrentPage, ref iRowCount);

            dgdrmuser.DataSource = dt;
            dgdrmuser.DataBind();

            this.cpfECustomerInfo.RecordCount = iRowCount;
            this.cpfECustomerInfo.Bind();
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgProduct_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                string client_id = e.Item.Cells[0].Text.Replace("&nbsp;", "");
                string sWhere = " And id = " + client_id;
                Br_ECustomerDP ec = new Br_ECustomerDP();
                DataTable dt = ec.GetDataTableAjax(sWhere, "");
                
                sbSetOpenerText.Remove(0, sbSetOpenerText.Length);
                sbSetOpenerText.Append("<script>");
                appendJsForParentPage(dt);  
                sbSetOpenerText.Append("window.close();");

                sbSetOpenerText.Append("</script>");

                //Json json = new Json(dt);

                //string jsonstr = "{record:" + json.ToJson() + "}";
                //sbSetOpenerText.Append("<script>");
                //sbSetOpenerText.Append("var arr = " + jsonstr + ";");
                //sbSetOpenerText.Append("window.parent.returnValue = arr;");
                //// 关闭窗口
                //sbSetOpenerText.Append("top.close();");
                //sbSetOpenerText.Append("</script>");
                // 向客户端发送
                Page.RegisterStartupScript(DateTime.Now.Ticks.ToString(), sbSetOpenerText.ToString());
                //Response.Write(sbSetOpenerText.ToString());
            }
            else if (e.CommandName == "look")
            {
                Response.Redirect("frmcusShow.aspx?id=" + e.Item.Cells[0].Text.Trim() + "&FlowID=" + FlowID);
            }
            else if (e.CommandName == "edit")
            {
                Response.Redirect("../AppForms/frmBr_ECustomerEdit.aspx?QuickNew=1&id=" + e.Item.Cells[0].Text.ToString());
            }
        }

        private void appendJsForParentPage(DataTable dt)
        {
            switch (RequestPageType)
            {
                case "1":
                    //request from issue_base page
                    appendJsForIssue_base(dt);
                    break;
                case "2":
                    //request from advance search.
                    appendJsForAdvanceSearch(dt);
                    break;
                case "3":
                    appendJsForChange_base(dt);
                    break;
                case "4": //需求管理相关页面获取客户信息
                    appendJsForDemandBase(dt);
                    break;
            }
        }

        private void appendJsForAdvanceSearch(DataTable dt)
        {
            sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtCustName", dt.Rows[0]["shortname"].ToString());
            //sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidCust", dt.Rows[0]["shortname"].ToString());     //客户
        }

        private void appendJsForChange_base(DataTable dt)
        {
            sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtCustAddr", dt.Rows[0]["shortname"].ToString());     //客户
            sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidCust", dt.Rows[0]["shortname"].ToString());     //客户

            sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtAddr", dt.Rows[0]["address"].ToString());     //地址
            sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidAddr", dt.Rows[0]["address"].ToString());     //地址

            sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtContact", dt.Rows[0]["linkman1"].ToString());     //联系人
            sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidContact", dt.Rows[0]["linkman1"].ToString());     //联系人

            sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtCTel", dt.Rows[0]["tel1"].ToString());     //联系人电话
            sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidTel", dt.Rows[0]["tel1"].ToString());     //

            sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidCustID", dt.Rows[0]["id"].ToString());     //客户ID号

            sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.innerText='{1}';", Opener_ClientId + "lblCustDeptName", dt.Rows[0]["custdeptname"].ToString());     //所属部门

            sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.innerText='{1}';", Opener_ClientId + "lblEmail", dt.Rows[0]["email"].ToString());     //
            sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.innerText='{1}';", Opener_ClientId + "lblMastCust", dt.Rows[0]["mastcustname"].ToString());     //
            sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidCustDeptName", dt.Rows[0]["custdeptname"].ToString());     //
            sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidCustEmail", dt.Rows[0]["email"].ToString());     //

            sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidMastCust", dt.Rows[0]["mastcustname"].ToString());     //服务单位
            sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.innerText='{1}';", Opener_ClientId + "lbljob", dt.Rows[0]["job"].ToString());     //职位
            sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidjob", dt.Rows[0]["job"].ToString());     //职位
        }
        /// <summary>
        /// 设置给父页面CST_Issue_Base_self赋值。
        /// </summary>
        /// <param name="dt"></param>
        private void CST_Issue_Base_self(DataTable dt) 
        {
            if (dt != null)
            {
                //==zxl===
                //客户
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtCustAddr", dt.Rows[0]["shortname"].ToString());
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidCust", dt.Rows[0]["shortname"].ToString());   //客户 
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtAddr", dt.Rows[0]["address"].ToString());
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidAddr", dt.Rows[0]["address"].ToString());
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtContact", dt.Rows[0]["linkman1"].ToString());//联系人

                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidContact", dt.Rows[0]["linkman1"].ToString());
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtCTel", dt.Rows[0]["tel1"].ToString());//联系电话

                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidTel", dt.Rows[0]["tel1"].ToString());

                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidCustID", dt.Rows[0]["id"].ToString());//客户id号

                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.innerHTML='{1}';", Opener_ClientId + "lblCustDeptName", dt.Rows[0]["custdeptname"].ToString());//所属部门
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.innerHTML='{1}';", Opener_ClientId + "lblEmail", dt.Rows[0]["email"].ToString());//电子邮件

                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.innerHTML='{1}';", Opener_ClientId + "lblMastCust", dt.Rows[0]["mastcustname"].ToString());//服务单位
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidCustDeptName", dt.Rows[0]["custdeptname"].ToString());//所属部门

                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidCustEmail", dt.Rows[0]["email"].ToString());//电子邮件

                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidMastCust", dt.Rows[0]["mastcustname"].ToString());//服务单位
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.innerHTML='{1}';", Opener_ClientId + "lbljob", dt.Rows[0]["job"].ToString());//职位
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidjob", dt.Rows[0]["job"].ToString());//职位
                sbSetOpenerText.Append("if (typeof(window.opener.document.all." + Opener_ClientId + "Table3)!='undefined') ");
                sbSetOpenerText.Append("{");

                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtEqu", dt.Rows[0]["equname"].ToString()); //资产名称
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidEquName", dt.Rows[0]["equname"].ToString()); //资产名称

                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidEqu", dt.Rows[0]["equid"].ToString()); //资产IDontact")).id);   

                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtListName", dt.Rows[0]["listname"].ToString()); //资产目录
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidListName", dt.Rows[0]["listname"].ToString()); //资产目录
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidListID", dt.Rows[0]["listid"].ToString()); //资产目录ID


                sbSetOpenerText.Append("}");

                //================

                //        var json = value;
                //var record=json.record;


                //================
            }
            else 
            {
                //为空的情况
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtCustAddr", "''");
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidCust", "''");   //客户 
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtAddr", "''");
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidAddr", "''");
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtContact", "''");//联系人

                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidContact", "''");
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtCTel", "''");//联系电话

                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidTel", "''");

                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.innerHTML='{1}';", Opener_ClientId + "hidCustID", "''");//客户id号

                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.innerHTML='{1}';", Opener_ClientId + "lblCustDeptName", "''");//所属部门
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.innerHTML='{1}';", Opener_ClientId + "lblEmail", "''");//电子邮件

                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.innerHTML='{1}';", Opener_ClientId + "lblMastCust", "''");//服务单位
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.innerHTML='{1}';", Opener_ClientId + "hidCustDeptName", "''");//所属部门

                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidCustEmail", "''");//电子邮件

                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidMastCust", "''");//服务单位
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.innerHTML='{1}';", Opener_ClientId + "lbljob", "''");//职位
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidjob", "''");//职位
                
            }
        }
        /// <summary>
        /// 设置父页面为Cst_Issue_Serive 的参数
        /// </summary>
        private void Cst_Issue_Serive(DataTable dt)
        {

            //============
            if (dt != null)
            {
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtCustAddr", dt.Rows[0]["shortname"].ToString());   //客户 
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}'; ", Opener_ClientId + "hidCust", dt.Rows[0]["shortname"].ToString());
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtAddr", dt.Rows[0]["address"].ToString()); //地址
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidAddr", dt.Rows[0]["address"].ToString()); //地址
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtCTel", dt.Rows[0]["tel1"].ToString()); //地址
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidTel", dt.Rows[0]["tel1"].ToString());
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidCustID", dt.Rows[0]["id"].ToString());
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.innerHTML='{1}';", Opener_ClientId + "lblCustDeptName", dt.Rows[0]["custdeptname"].ToString());//所属部门
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.innerHTML='{1}';", Opener_ClientId + "lblEmail", dt.Rows[0]["email"].ToString()); //电子邮件
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.innerHTML='{1}';", Opener_ClientId + "lblMastCust", dt.Rows[0]["mastcustname"].ToString()); //服务单位

                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidCustDeptName", dt.Rows[0]["custdeptname"].ToString()); //所属部门



                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidCustEmail", dt.Rows[0]["email"].ToString()); //电子邮件



                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidMastCust", dt.Rows[0]["mastcustname"].ToString());

                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtCustMobile", dt.Rows[0]["mobile"].ToString());
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidCustMobile", dt.Rows[0]["mobile"].ToString());

                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidCustAreaID", dt.Rows[0]["custareaid"].ToString());
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidCustArea", dt.Rows[0]["custarea"].ToString()); //客户片区
                sbSetOpenerText.Append("if (typeof(window.opener.document.all." + Opener_ClientId + "Table3)!='undefined') ");
                sbSetOpenerText.Append("{");
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtEqu", dt.Rows[0]["equname"].ToString()); //资产名称
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidEquName", dt.Rows[0]["equname"].ToString()); //资产名称

                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidEqu", dt.Rows[0]["equid"].ToString()); //资产IDontact")).id);   

                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtListName", dt.Rows[0]["listname"].ToString()); //资产目录
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidListName", dt.Rows[0]["listname"].ToString()); //资产目录
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidListID", dt.Rows[0]["listid"].ToString()); //资产目录ID


                sbSetOpenerText.Append("}");

                //===================
            }
            else
            {

                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtCustAddr", "''");   //客户 
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}'; ", Opener_ClientId + "hidCust", "''");
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtAddr", "''"); //地址
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidAddr", "''"); //地址
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtCTel", "''"); //地址
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidTel", "''");
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidCustID", "''");
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.innerHTML='{1}';", Opener_ClientId + "lblCustDeptName", "''");//所属部门
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.innerHTML='{1}';", Opener_ClientId + "lblEmail", "''"); //电子邮件
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.innerHTML='{1}';", Opener_ClientId + "lblMastCust", "''"); //服务单位

                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidCustDeptName", "''"); //所属部门



                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidCustEmail", "''"); //电子邮件



                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidMastCust", "''");

                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtCustMobile", "''");
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidCustMobile", "''");

                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidCustAreaID", "''");
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidCustArea", "''"); //客户片区
            }
        
        }

        private void appendJsForIssue_base(DataTable dt)
        {
            if (Request["TypeFrom"] != null)
            {
                if (Request["TypeFrom"] == "CST_Issue_Base") 
                {
                    sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtCustAddr", dt.Rows[0]["shortname"].ToString());     //客户
                    sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidCust", dt.Rows[0]["shortname"].ToString());     //客户

                    sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtAddr", dt.Rows[0]["address"].ToString());     //地址
                    sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidAddr", dt.Rows[0]["address"].ToString());     //地址

                    sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtContact", dt.Rows[0]["linkman1"].ToString());     //联系人
                    sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidContact", dt.Rows[0]["linkman1"].ToString());     //联系人

                    sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtCTel", dt.Rows[0]["tel1"].ToString());     //联系人电话
                    sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidTel", dt.Rows[0]["tel1"].ToString());     //

                    sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidCustID", dt.Rows[0]["id"].ToString());     //客户ID号


                    sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.innerText='{1}';", Opener_ClientId + "lblCustDeptName", dt.Rows[0]["custdeptname"].ToString());     //所属部门

                    sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.innerHTML='{1}';", Opener_ClientId + "lblEmail", dt.Rows[0]["email"].ToString());     //
                    
                    sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.innerText='{1}';", Opener_ClientId + "lblMastCust", dt.Rows[0]["mastcustname"].ToString());     //
                    sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidCustDeptName", dt.Rows[0]["custdeptname"].ToString());     //
                    sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidCustEmail", dt.Rows[0]["email"].ToString());     //

                    sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidMastCust", dt.Rows[0]["mastcustname"].ToString());     //服务单位
                    sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.innerText='{1}';", Opener_ClientId + "lbljob", dt.Rows[0]["job"].ToString());     //职位
                    sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidjob", dt.Rows[0]["job"].ToString());     //职位
              
                    
                }
                else if (Request["TypeFrom"] == "CST_Issue_Service")
                {
                    //zxl==2012.8.7
                    Cst_Issue_Serive(dt);
                }
                else if (Request["TypeFrom"] == "CST_Issue_Base_self")
                {
                    CST_Issue_Base_self(dt);


                }


            }
            else 
            {
           


            //zxl修改========
            sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", "ctl00_ContentPlaceHolder1_CustomCtr1_txtCustom", dt.Rows[0]["shortname"].ToString());
            sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", "ctl00_ContentPlaceHolder1_CustomCtr1_hidCustomName", dt.Rows[0]["shortname"].ToString());
            sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", "ctl00_ContentPlaceHolder1_CustomCtr1_hidCustom", dt.Rows[0]["id"].ToString());
            }

            //================              
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddltBuild_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind();
        }

        protected void dgdrmuser_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                ((Label)e.Item.FindControl("client_name")).Attributes.Add("onmouseover", "ShowDetailsInfo(this," + DataBinder.Eval(e.Item.DataItem, "client_id").ToString() + ",400);");

                string sID = e.Item.Cells[1].Text.Trim();
                //string id = DataBinder.Eval(e.Item.DataItem, "ID").ToString();  sID 和 id指的是同一个id，获取方式不同

                string name = DataBinder.Eval(e.Item.DataItem, "client_name").ToString();
                Button lnkselect = (Button)e.Item.FindControl("lnkselect");
                if (!IsSelect)
                {
                    e.Item.Attributes.Add("ondblclick", "window.open('../AppForms/frmBr_ECustomerEdit.aspx?QuickNew=1&id=" + e.Item.Cells[0].Text.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
                }
                else
                {
                    string client_id = e.Item.Cells[0].Text.Replace("&nbsp;", "");
                    string sWhere = " And id = " + client_id;
                    Br_ECustomerDP ec = new Br_ECustomerDP();
                    DataTable dt = ec.GetDataTableAjax(sWhere, "");
                    Json json = new Json(dt);

                    string jsonstr = "{record:" + json.ToJson() + "}";

                    // 向客户端发送
                    //e.Item.Attributes.Add("ondblclick", "ServerOndblclick(" + jsonstr + ");");
                    e.Item.Attributes.Add("ondblclick", "OnClientClick('" + lnkselect.ClientID  + "');");
                    
                }
            }
        }

        protected void dgdrmuser_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header) {
                DataGrid dg = (DataGrid)sender;
                int j = 0;
                for (int i = 0; i < e.Item.Cells.Count; i++) {
                    if (i > 0 && i < 9) {
                        j = i - 1;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }

        #region 需求管理页面获取客户信息 余向前 2013-04-27
        /// <summary>
        /// 需求管理页面获取客户信息
        /// </summary>
        /// <param name="dt"></param>
        private void appendJsForDemandBase(DataTable dt)
        {
            if (Request["TypeFrom"] == "DemandBase")
            {
                //客户
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtCustAddr", dt.Rows[0]["shortname"].ToString());
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidCust", dt.Rows[0]["shortname"].ToString());   //客户 
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtAddr", dt.Rows[0]["address"].ToString());
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidAddr", dt.Rows[0]["address"].ToString());
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtContact", dt.Rows[0]["linkman1"].ToString());//联系人
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidContact", dt.Rows[0]["linkman1"].ToString());
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtCTel", dt.Rows[0]["tel1"].ToString());//联系电话
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidTel", dt.Rows[0]["tel1"].ToString());
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidCustID", dt.Rows[0]["id"].ToString());//客户id号
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.innerHTML='{1}';", Opener_ClientId + "lblCustDeptName", dt.Rows[0]["custdeptname"].ToString());//所属部门
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.innerHTML='{1}';", Opener_ClientId + "lblEmail", dt.Rows[0]["email"].ToString());//电子邮件
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.innerHTML='{1}';", Opener_ClientId + "lblMastCust", dt.Rows[0]["mastcustname"].ToString());//服务单位
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidCustDeptName", dt.Rows[0]["custdeptname"].ToString());//所属部门
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidCustEmail", dt.Rows[0]["email"].ToString());//电子邮件
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidMastCust", dt.Rows[0]["mastcustname"].ToString());//服务单位
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.innerHTML='{1}';", Opener_ClientId + "lbljob", dt.Rows[0]["job"].ToString());//职位
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidjob", dt.Rows[0]["job"].ToString());//职位

                //资产
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtEqu", dt.Rows[0]["equname"].ToString()); //资产名称
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidEquName", dt.Rows[0]["equname"].ToString()); //资产名称
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidEqu", dt.Rows[0]["equid"].ToString()); //资产IDontact")).id);                  
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidListName", dt.Rows[0]["listname"].ToString()); //资产目录
                sbSetOpenerText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidListID", dt.Rows[0]["listid"].ToString()); //资产目录ID

            }
        }
        #endregion
    }
}