using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Epower.DevBase.Organization;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;
using System.Text;

using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.MyDestop
{
    /// <summary>
    /// frmQuickLocateCust 的摘要说明。
    /// </summary>
    public partial class frmQuickLocateCustAjax : BasePage
    {
        #region 是否查询IsSelect
        protected bool IsSelect
        {
            get
            {
                if (Request["IsSelect"] != null && Request["IsSelect"] == "true")
                    return true;
                else return false;
            }
        }
        #endregion

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.TableVisible = false;
        }
        #endregion

        #region 判断是哪个页面传递过来的 余向前 2013-04-24
        /// <summary>
        /// 判断是哪个页面传递过来的 1默认事件单 2事件单高级查询 3需求管理
        /// </summary>
        public string RequestPageType
        {
            set { ViewState["RequestPageType"] = value; }
            get { return ViewState["RequestPageType"].ToString(); }
        }
        #endregion

        #region 获得传过来的参数用来判断父窗体的控件id的名称的相同部分
        /// <summary>
        /// 获得传过来的参数用来判断父窗体的控件id的名称的相同部分
        /// </summary>
        public string Opener_ClientId
        {

            get
            {
                return (Request["Opener_ClientId"] == null) ? "" : Request["Opener_ClientId"].ToString().Replace("hidClientId_ForOpenerPage", "");
            }
        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            SetParentButtonEvent();
            if (!IsPostBack)
            {
                SetHeaderText();

                RequestPageType = (Request["PageType"] == null) ? "1" : Request["PageType"].ToString();

                if (Request["Name"] != null)
                    BindData(Request["Name"].ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindData(string sCust)
        {
            string sSql = " and (E.ShortName like " + StringTool.SqlQ("%" + sCust + "%")
                       + " or E.FullName like " + StringTool.SqlQ("%" + sCust + "%")
                       + " or E.CustomCode like " + StringTool.SqlQ("%" + sCust + "%")
                       + " or E.LinkMan1 like " + StringTool.SqlQ("%" + sCust + "%")
                       + " or Email like " + StringTool.SqlQ("%" + sCust + "%")
                       + " or E.Tel1 like " + StringTool.SqlQ("%" + sCust + "%") + ")";
            Br_ECustomerDP ee = new Br_ECustomerDP();
            DataTable dt = ee.GetCustomerServic(sSql, string.Empty);          

            this.dgUserInfo.DataSource = dt.DefaultView;
            this.dgUserInfo.DataBind();
        }

        /// <summary>
        /// 设置列头名称 余向前 2013-05-24
        /// </summary>
        void SetHeaderText()
        {
            dgUserInfo.Columns[1].HeaderText = PageDeal.GetLanguageValue("Custom_CustName");
            dgUserInfo.Columns[2].HeaderText = PageDeal.GetLanguageValue("Custom_CustAddress");
            dgUserInfo.Columns[3].HeaderText = PageDeal.GetLanguageValue("Custom_Contact");
            dgUserInfo.Columns[4].HeaderText = PageDeal.GetLanguageValue("Custom_CTel");
            dgUserInfo.Columns[5].HeaderText = PageDeal.GetLanguageValue("Custom_CustomCode");
            dgUserInfo.Columns[6].HeaderText = PageDeal.GetLanguageValue("Custom_CustEmail");
            dgUserInfo.Columns[7].HeaderText = PageDeal.GetLanguageValue("Custom_MastCustName");
        }

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
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.dgUserInfo.ItemCreated += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgUserInfo_ItemCreated);

        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgUserInfo_ItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    // (DataView)e.Item.NamingContainer;
                    if (i > 0 && i < 8)
                    {
                        int j = i - 1;   //注意,因为前面有一个不可见的列
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgUserInfo_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {

                string client_id = e.Item.Cells[0].Text.Replace("&nbsp;", "");
                string sWhere = " And id = " + client_id;
                Br_ECustomerDP ec = new Br_ECustomerDP();
                DataTable dt = ec.GetDataTableAjax(sWhere, "");
                Json json = new Json(dt);


                if (RequestPageType == "2")
                {
                    StringBuilder strText = new StringBuilder();
                    strText.Append("<script>");
                    strText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtCustName", dt.Rows[0]["shortname"].ToString());
                    strText.Append("window.close();");
                    strText.Append("</script>");
                    Page.RegisterStartupScript(DateTime.Now.Ticks.ToString(), strText.ToString());
                    return;
                }

                //需求管理页面获取客户信息
                if (RequestPageType == "3")
                {
                    string strjson = "{record:" + json.ToJson() + "}";

                    StringBuilder strText = new StringBuilder();
                    strText.Append("<script>");
                    strText.Append("var arr = " + strjson + ";");
                    strText.Append("if(arr != '')");
                    strText.Append(" {");
                    strText.Append(" var json = arr;");
                    strText.Append(" var record=json.record;");

                    strText.Append(" for(var i=0; i < record.length; i++)");
                    strText.Append("  {");
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "txtCustAddr').value = record[i].shortname; ");  //客户
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidCust').value = record[i].shortname;");
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "txtAddr').value = record[i].address;");
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidAddr').value = record[i].address;");
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "txtContact').value = record[i].linkman1;"); //联系人
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidContact').value = record[i].linkman1;"); //联系人
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "txtCTel').value = record[i].tel1;"); //联系人电话
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidTel').value = record[i].tel1;"); //联系人电话
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidCustID').value = record[i].id;"); //客户ID号
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "lblCustDeptName').innerHTML = record[i].custdeptname;"); //所属部门
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "lblEmail').innerHTML = record[i].email;"); //电子邮件
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "lblMastCust').innerHTML = record[i].mastcustname;"); //服务单位
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidCustDeptName').value = record[i].custdeptname;"); //所属部门
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidCustEmail').value = record[i].email;"); //电子邮件
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidMastCust').value = record[i].mastcustname;"); //服务单位
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "lbljob').innerHTML = record[i].job;"); //职位
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidjob').value = record[i].job;"); //职位

                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "txtEqu').value = record[i].equname;"); //资产名称
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidEquName').value = record[i].equname;"); //资产名称
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidEqu').value = record[i].equid;"); //资产id                    
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidListName').value = record[i].listname;"); //资产目录
                    strText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidListID').value = record[i].listid;"); //资产目录
                    strText.Append(" }");
                    strText.Append(" }");
                    strText.Append("window.close();");
                    strText.Append("</script>");
                    Page.RegisterStartupScript(DateTime.Now.Ticks.ToString(), strText.ToString());
                    return;
                }


                string jsonstr = "{record:" + json.ToJson() + "}";

                StringBuilder sbText = new StringBuilder();
                sbText.Append("<script>");
                sbText.Append("var arr = " + jsonstr + ";");
                //==========zxl========
               // sbText.Append("window.parent.returnValue = arr;");
                //===============================
               sbText.Append("if(arr != '')");
               sbText.Append(" {");
               sbText.Append(" var json = arr;");
               sbText.Append(" var record=json.record;");

                sbText.Append(" for(var i=0; i < record.length; i++)");
                sbText.Append("  {");
                sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"txtCustAddr').value = record[i].shortname; ");  //客户

                 sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidCust').value = record[i].shortname;");

                 sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"txtAddr').value = record[i].address;");
                 sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidAddr').value = record[i].address;");
                 sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"txtContact').value = record[i].linkman1;"); //联系人
                 sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidContact').value = record[i].linkman1;"); //联系人

                 sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"txtCTel').value = record[i].tel1;"); //联系人电话

                 sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidTel').value = record[i].tel1;"); //联系人电话

                 sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidCustID').value = record[i].id;"); //客户ID号
                

                        
                
                sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"lblCustDeptName').innerHTML = record[i].custdeptname;"); //所属部门
                 sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"lblEmail').innerHTML = record[i].email;"); //电子邮件
                sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"lblMastCust').innerHTML = record[i].mastcustname;"); //服务单位
                sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidCustDeptName').value = record[i].custdeptname;"); //所属部门
                

               // sbText.Append("");
                 sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidCustEmail').value = record[i].email;"); //电子邮件
                 sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidMastCust').value = record[i].mastcustname;"); //服务单位
                 sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"lbljob').innerHTML = record[i].job;"); //职位
                sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidjob').value = record[i].job;"); //职位
                 
                sbText.Append(" if (typeof(window.opener.document.getElementById('"+Opener_ClientId+"Table3'))!='undefined' )");
                sbText.Append("{");

                    sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"txtEqu').value = record[i].equname;"); //资产名称

                    sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidEquName').value = record[i].equname;"); //资产名称
                    sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidEqu').value = record[i].equid;"); //资产id
                    sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"txtListName').value = record[i].listname;"); //资产目录
                    sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidListName').value = record[i].listname;"); //资产目录
                    sbText.Append("window.opener.document.getElementById('"+Opener_ClientId+"hidListID').value = record[i].listid;"); //资产目录
              
                                                 

                     sbText.Append("}");               
                    sbText.Append("}");
                sbText.Append("}");
                sbText.Append("else");
                sbText.Append("{");

                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidCustID').value ='';"); //客户ID号	

                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "txtCustAddr').value ='';"); //客户名称

                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidCust').value ='';"); //客户名称	

                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "txtAddr').value ='';"); //地址	

                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidAddr').value ='';"); //地址	

                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "txtContact').value ='';"); //联系人
                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidContact').value ='';");

                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "txtCTel').value ='';"); //联系人电话
                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidTel').value ='';");

                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "lblCustDeptName').innerHTML ='';"); //所属部门
                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "lblEmail').innerHTML ='';"); //电子邮件
                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "lblMastCust').innerHTML ='';"); //服务单位


                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidCustDeptName').value ='';"); //所属部门
                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidCustEmail').value ='';"); //电子邮件
                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidMastCust').value ='';"); //服务单位

                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "lbljob').innerHTML ='';"); //职位
                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidjob').value ='';"); //职位
             
                sbText.Append(" }");           
      

                // 关闭窗口
                sbText.Append("top.close();");
                sbText.Append("</script>");
                // 向客户端发送
                Page.RegisterStartupScript(DateTime.Now.ToString(), sbText.ToString());
               // Response.Write(sbText.ToString());
            }
        }

        protected void dgUserInfo_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");

                if (IsSelect)
                {
                    string client_id = e.Item.Cells[0].Text.Replace("&nbsp;", "");
                    string sWhere = " And id = " + client_id;
                    Br_ECustomerDP ec = new Br_ECustomerDP();
                    DataTable dt = ec.GetDataTableAjax(sWhere, "");
                    Json json = new Json(dt);

                    string jsonstr = "{record:" + json.ToJson() + "}";

                    // 向客户端发送
                    //e.Item.Attributes.Add("ondblclick", "ServerOndblclick(" + jsonstr + ");");
                    Button lnkselect = (Button)e.Item.FindControl("lnkselect");
                    e.Item.Attributes.Add("ondblclick", "OnClientClick('" + lnkselect.ClientID + "');");
                }
            }
        }
    }
}
