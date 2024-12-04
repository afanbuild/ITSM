/*******************************************************************
 * 版权所有：
 * Description：事件单高级查询页面
 * Create By  ：SuperMan
 * Create Date：2011-08-22
 * *****************************************************************/
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using EpowerGlobal;
using System.Data.OracleClient;
using System.Text;
using System.Collections.Generic;


namespace Epower.ITSM.Web.Forms
{
    /// <summary>
    /// frmActorCondList 的摘要说明。
    /// </summary>
    public partial class CST_Issue_AdvancedCondition : BasePage
    {
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
        ///获得 路径
        /// </summary>
        public string Opener_ClientId
        {
           
            get
            {
                return (Request["Opener_ClientId"] == null) ? "" : Request["Opener_ClientId"].ToString().Replace("hidClientId_ForOpenerPage", "");
            }
        }

        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion
        #region 表名或试图名
        private string tableName;
        private string viewName;
        #endregion
        #region 窗体加载

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                cboStatus.Items.Add(new ListItem("所有状态", "-1"));
                cboStatus.Items.Add(new ListItem("--正在处理", ((int)e_FlowStatus.efsHandle).ToString()));
                cboStatus.Items.Add(new ListItem("--正常结束", ((int)e_FlowStatus.efsEnd).ToString()));
                cboStatus.Items.Add(new ListItem("--流程暂停", ((int)e_FlowStatus.efsStop).ToString()));
                cboStatus.Items.Add(new ListItem("--流程终止", ((int)e_FlowStatus.efsAbort).ToString()));
                cboStatus.SelectedIndex = 1;

                InitDropDown();  //初始化服务级别

                //设置起始日期
                string sQueryBeginDate = "0";
                if (CommonDP.GetConfigValue("Other", "QueryBeginDate") != null)
                    sQueryBeginDate = CommonDP.GetConfigValue("Other", "QueryBeginDate").ToString();

                if (sQueryBeginDate == "0")
                {
                    ctrDateSelectTime1.BeginTime = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                }
                else
                {
                    ctrDateSelectTime1.BeginTime = DateTime.Parse(sQueryBeginDate).ToString("yyyy-MM-dd");
                }
                ctrDateSelectTime1.EndTime = DateTime.Now.ToString("yyyy-MM-dd");


                if (Request["SQLName"] != null)
                {
                    InitDropSQLwSave1(Request["SQLName"].ToString().Trim());  //初始化查询条件
                    SqlWhereShow(Request["SQLName"].ToString().Trim() == "" ? "Temp1" : Request["SQLName"].ToString().Trim());
                }

                if (DropSQLwSave.SelectedItem.Value == "0")
                {
                    btn_delete.Text = "清空";
                }
                else
                {
                    btn_delete.Text = "删除";
                }

                //设置显示
                PageDeal.SetLanguage(this.Controls[1]);
                tableName = Request.QueryString["tableName"];
                viewName = Request.QueryString["viewName"];
                if (!string.IsNullOrEmpty(tableName) || !string.IsNullOrEmpty(viewName))
                    DataBing();
            }
            if (DropSQLwSave.SelectedItem.Value == "0")
            {
                btn_delete.Text = "清空";
            }
            else
            {
                btn_delete.Text = "删除";
            }
        }

        #endregion

        #region 窗体按钮事件

        #region 保存查询条件名称

        protected void chkSave_Click(object sender, EventArgs e)
        {
            if (this.txtSQLName.Text.Trim() == string.Empty)
            {
                PageTool.MsgBox(this.Page, "保存名称不能为空！");
                return;
            }

            if (this.txtSQLName.Text.Trim().ToLower() == "temp1")
            {
                PageTool.MsgBox(this.Page, "Temp1名称被该系统使用，请重新输入名称！");
                return;
            }

            SqlWhereSave();
            InitDropSQLwSave();
        }

        #endregion

        #region 删除查询条件名称
        /// <summary>
        /// 删除查询条件名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_delete_Click(object sender, EventArgs e)
        {
            if (this.txtSQLName.Text.Trim() == string.Empty && DropSQLwSave.SelectedItem.Text != "==选择收藏查询条件==")
            {
                PageTool.MsgBox(this.Page, "条件名称不能为空！");
                return;
            }
            else
            {
                //当条件名称不为空时，将查询条件的值全部清空
                txtCustName.Text = "";                  //客户名称
                txtIssueNo.Text = "";                   //事件单号
                ctrDateSelectTime1.BeginTime = "";      //发生时间开始时间
                ctrDateSelectTime1.EndTime = "";        //发生时间结束时间
                cboStatus.SelectedIndex = 0;            //流程状态
                CtrFCDEffect.CatelogID = 0;             //影响度
                ctrDealStatus.CatelogID = 0;         //事件状态
                CtrFCDInstancy.CatelogID = 0;           //紧急度
                ctrServiceType.CatelogID = 0;           //事件类别
                ctrServiceType.CatelogValue = "";
                ctrFCDWTType.CatelogID = 0;             //事件性质
                UserPSjzxr.UserID = "0";                //工程师
                UserPSjzxr.UserName = "";
                txtSubject.Text = "";                   //标题
                txtperson.Text = "";                    //登单人
                ddltServiceLevel.SelectedIndex = 0;     //服务级别
                txtEqu.Text = "";                       //资产信息
                lblEqu.Text = "";
                ddltEmailState.SelectedIndex = 0;       //邮件回访状态
            }

            ZHServiceDP.deleteCST_ISSUE_Where(Session["UserName"].ToString(), DropSQLwSave.SelectedItem.Text.ToString());
            this.txtSQLName.Text = string.Empty;
            InitDropSQLwSave();
        }

        #endregion

        #region 确定
        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOK_Click(object sender, EventArgs e)
        {
            string SQLName = "";
            if (DropSQLwSave.SelectedItem.Text != "==选择收藏查询条件==")
            {
                SQLName = DropSQLwSave.SelectedItem.Text.Trim();
            }
            else
            {
                SQLName = "Temp1";
            }

            //确定前先保存临时到数据库
            SqlWhereSaveTemp1(SQLName);

            //关闭窗体
            StringBuilder sbText = new StringBuilder();
            sbText.Append("<script>");
            sbText.Append("var arr = new Array();");
            // 成功
            sbText.Append("arr[0] ='" + SQLName + "';");
            sbText.Append("arr[1] ='" + this.hidValue.Value + "';");
            // 名称
            //==========zxl==
            sbText.Append(
             " var arrValue=arr;" +
            " if(arrValue != null)"+
	        "{" +
	        "window.opener.document.getElementById('"+Opener_ClientId+"hidIsGaoji').value = '1'; "+
	       " window.opener.document.getElementById('"+Opener_ClientId+"hidSQLName').value = arrValue[0];"+
	        " window.opener.document.getElementById('"+Opener_ClientId+"HiddenColumn').value = arrValue[1]; "+
	       " window.opener.document.getElementById('"+Opener_ClientId+"btnOk').click();" +
	    "}" );

            //=========zxl==
           // sbText.Append("window.parent.returnValue = arr;");
            // 关闭窗口
            sbText.Append("top.close();");
            sbText.Append("</script>");
            // 向客户端发送
            Page.RegisterStartupScript(DateTime.Now.ToString(), sbText.ToString());
            Response.Write(sbText.ToString());
        }

        #endregion

        #region 关闭

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Write("<script>window.close();</script>");
        }

        #endregion

        #region 收藏条件下拉框事件

        protected void DropSQLwSave_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strTemp = string.Empty;              //临时存放改变后的下拉列表内容

            if (DropSQLwSave.SelectedItem.Text != "==选择收藏查询条件==")
            {
                strTemp = DropSQLwSave.SelectedValue;           //将选择的高级条件名称存储起来

                SqlWhereShow(DropSQLwSave.SelectedItem.Text.ToString());

                //更新访问次数
                ZHServiceDP.updateCST_ISSUE_WhereNums(Session["UserName"].ToString(), DropSQLwSave.SelectedItem.Text.ToString());
                DataTable dt = ZHServiceDP.getCST_ISSUE_LISTFASTQUERY("CST_Issue_List", Session["UserName"].ToString(), string.Empty);
                DropSQLwSave.Items.Clear();
                DropSQLwSave.DataSource = dt.DefaultView;
                DropSQLwSave.DataTextField = "Name";
                DropSQLwSave.DataValueField = "ID";
                DropSQLwSave.DataBind();
                DropSQLwSave.Items.Insert(0, new ListItem("==选择收藏查询条件==", "0"));

                DropSQLwSave.SelectedIndex = DropSQLwSave.Items.IndexOf(DropSQLwSave.Items.FindByValue(strTemp));

                txtSQLName.Text = DropSQLwSave.SelectedItem.Text;
            }
            else
            {
                txtSQLName.Text = "";
            }
        }

        #endregion

        #endregion

        #region 自定义方法

        #region InitDropDown 初始化服务级别
        /// <summary>
        /// 
        /// </summary>
        private void InitDropDown()
        {
            Cst_ServiceLevelDP ee = new Cst_ServiceLevelDP();
            DataTable dt = ee.GetDataTable("", "");
            ddltServiceLevel.Items.Clear();
            ddltServiceLevel.DataSource = dt.DefaultView;
            ddltServiceLevel.DataTextField = "LevelName";
            ddltServiceLevel.DataValueField = "ID";
            ddltServiceLevel.DataBind();
            ddltServiceLevel.Items.Insert(0, new ListItem("", "0"));
        }

        #endregion

        protected void Button1_Click(object sender, EventArgs e)
        {
            HidIS.Value = "true";
            Bing();
        }
        void ShowDataBing(Dictionary<string, string> dic)
        {
            this._TableColumnCheckBoxList.DataSource = dic;
            this._TableColumnCheckBoxList.DataTextField = "value";
            this._TableColumnCheckBoxList.DataValueField = "key";
            this._TableColumnCheckBoxList.DataBind();
        }
        public void Bing()
        {

            //string value = @"申请人,申请人地址,办公电话,电子邮件,详细信息,服务级别,派出时间,上门时间,执行人名称,完成时间,措施及结果";//,合计金额,合计工时

            //string key = @"CustName,CustAddress,CTel,Email,Content,ServiceLevel,Outtime,ServiceTime,Sjwxr,FinishedTime,DealContent";//,TotalAmount,TotalHours


            string key2 = @"CustName,CustAddress,CTel,Email,Content,ServiceLevel,ReSouseName,DealStatus,Outtime,ServiceTime,Sjwxr,FinishedTime,DealContent";//IssueRootName,TotalAmount,TotalHours
            string value2 = @"客户名称,客户地址,办公电话,电子邮件,详细信息,服务级别,事件来源,变更状态,派出时间,上门时间,执行人名称,完成时间,措施及结果";//,总计金额,合计工时,事件根源

            string key = @"";
            string value= @"";
            if (this.DropDownList1.SelectedValue == "1")
            {

                this.ShowDataBing(Dic(value2, key2));
            }
            else
            {
                this.ShowDataBing(Dic(value, key));
            }
        }
        void DataBing()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("1", "事件");
            dic.Add("2", "服务请求");
            this.DropDownList1.DataSource = dic;
            this.DropDownList1.DataTextField = "value";
            this.DropDownList1.DataValueField = "key";
            this.DropDownList1.DataBind();
        }
        Dictionary<string, string> Dic(string value, string key)
        {
            string[] keys = key.Split(',');
            string[] values = value.Split(',');
            Dictionary<string, string> dic = new Dictionary<string, string>();
            for (int i = 0; i < keys.Length; i++)
            {
                dic.Add(keys[i], values[i]);
            }
            return dic;
        }
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bing();
        }
        #region 保存查询条件方法

        protected void SqlWhereSave()
        {
            if (txtSQLName.Text.Trim() != string.Empty)
            {
                StringBuilder SQLText = new StringBuilder();

                //客户名称
                SQLText.Append("txtCustName.Text=" + txtCustName.Text.ToString().Trim() + "|@@?@$|");

                //事件单号
                SQLText.Append("txtIssueNo.Text=" + txtIssueNo.Text.ToString().Trim() + "|@@?@$|");

                //事件状态
                SQLText.Append("ddlDealStatus.SelectedValue=" + ctrDealStatus.CatelogID.ToString().Trim() + "|@@?@$|");

                //流程状态
                SQLText.Append("cboStatus.SelectedValue=" + cboStatus.SelectedValue.ToString().Trim() + "|@@?@$|");

                //发生时间开始
                SQLText.Append("ctrDateSelectTime1.BeginTime=" + ctrDateSelectTime1.BeginTime.ToString().Trim() + "|@@?@$|");

                //发生时间结束
                SQLText.Append("ctrDateSelectTime1.EndTime=" + ctrDateSelectTime1.EndTime.ToString().Trim() + "|@@?@$|");

                //事件类别
                SQLText.Append("ctrServiceType.CatelogID=" + ctrServiceType.CatelogID.ToString().Trim() + "|@@?@$|");

                //影响度
                SQLText.Append("CtrFCDEffect.CatelogID=" + CtrFCDEffect.CatelogID.ToString().Trim() + "|@@?@$|");

                //事件性质
                SQLText.Append("ctrFCDWTType.CatelogID=" + ctrFCDWTType.CatelogID.ToString().Trim() + "|@@?@$|");

                //紧急度
                SQLText.Append("CtrFCDInstancy.CatelogID=" + CtrFCDInstancy.CatelogID.ToString().Trim() + "|@@?@$|");

                //标题
                SQLText.Append("txtSubject.Text=" + txtSubject.Text.ToString().Trim() + "|@@?@$|");

                //执行人
                SQLText.Append("UserPSjzxr.UserID=" + UserPSjzxr.UserID.ToString().Trim() + "|@@?@$|");

                //登单人
                SQLText.Append("txtperson.Text=" + txtperson.Text.ToString().Trim() + "|@@?@$|");

                //服务级别
                SQLText.Append("ddltServiceLevel.SelectedValue=" + ddltServiceLevel.SelectedValue.ToString().Trim() + "|@@?@$|");

                //资产信息
                SQLText.Append("txtEqu.Text=" + txtEqu.Text.ToString().Trim() + "|@@?@$|");

                //邮件回访状态
                SQLText.Append("ddltEmailState.SelectedValue=" + ddltEmailState.SelectedValue.ToString().Trim() + "|@@?@$|");

                //保存条件名
                SQLText.Append("txtSQLName.Text=" + txtSQLName.Text.ToString().Trim());

                string SQLstr = "";

                if (IsExistQuery(" and Name=" + StringTool.SqlQ(this.txtSQLName.Text.Trim()) + " and  nvl(SQLWhere,' ')!='Temp1'  and FORMID='CST_Issue_List'"))
                {
                    if (string.IsNullOrEmpty(this.hidValue.Value))
                    {
                        SQLstr = "update CST_ISSUE_QUERYSave set SQLText=" + StringTool.SqlQ(SQLText.ToString().Trim()) +
                                " where FORMID='CST_Issue_List' and  nvl(SQLWhere,' ')!='Temp1' and  LOGINNAME=" + StringTool.SqlQ(Session["UserName"].ToString()) + " and Name=" + StringTool.SqlQ(this.txtSQLName.Text.Trim());
                    }
                    else
                    {
                        SQLstr = "update CST_ISSUE_QUERYSave set SQLText=" + StringTool.SqlQ(SQLText.ToString().Trim()) + ",DISPLAYCOLUMN=" + StringTool.SqlQ(hidValue.Value.ToString().Trim()) +
                           " where FORMID='CST_Issue_List' and  nvl(SQLWhere,' ')!='Temp1' and  LOGINNAME=" + StringTool.SqlQ(Session["UserName"].ToString()) + " and Name=" + StringTool.SqlQ(this.txtSQLName.Text.Trim());
                    }
                }
                else
                {
                    string strID = EpowerGlobal.EPGlobal.GetNextID("CST_ISSUE_QUERYSaveID").ToString();

                    SQLstr = "insert into CST_ISSUE_QUERYSave(ID,Name,FORMID,SQLWhere,LOGINNAME,SQLText,DISPLAYCOLUMN)" +
                            " values(" + strID + "," + StringTool.SqlQ(this.txtSQLName.Text.Trim()) + ",'CST_Issue_List',''," +
                            StringTool.SqlQ(Session["UserName"].ToString()) + "," + StringTool.SqlQ(SQLText.ToString().Trim()) + "," + StringTool.SqlQ(this.hidValue.Value.ToString().Trim()) + ")";
                }
                OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationSettings.AppSettings["SQLConnString"]);
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, SQLstr);
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }

            }
        }

        protected void SqlWhereSaveTemp1(string SQLName)
        {
            StringBuilder SQLText = new StringBuilder();

            //客户名称
            SQLText.Append("txtCustName.Text=" + txtCustName.Text.ToString().Trim() + "|@@?@$|");

            //事件单号
            SQLText.Append("txtIssueNo.Text=" + txtIssueNo.Text.ToString().Trim() + "|@@?@$|");

            //事件状态
            SQLText.Append("ddlDealStatus.SelectedValue=" + ctrDealStatus.CatelogID.ToString().Trim() + "|@@?@$|");

            //流程状态
            SQLText.Append("cboStatus.SelectedValue=" + cboStatus.SelectedValue.ToString().Trim() + "|@@?@$|");

            //发生时间开始
            SQLText.Append("ctrDateSelectTime1.BeginTime=" + ctrDateSelectTime1.BeginTime.ToString().Trim() + "|@@?@$|");

            //发生时间结束
            SQLText.Append("ctrDateSelectTime1.EndTime=" + ctrDateSelectTime1.EndTime.ToString().Trim() + "|@@?@$|");

            //事件类别
            SQLText.Append("ctrServiceType.CatelogID=" + ctrServiceType.CatelogID.ToString().Trim() + "|@@?@$|");

            //影响度
            SQLText.Append("CtrFCDEffect.CatelogID=" + CtrFCDEffect.CatelogID.ToString().Trim() + "|@@?@$|");

            //事件性质
            SQLText.Append("ctrFCDWTType.CatelogID=" + ctrFCDWTType.CatelogID.ToString().Trim() + "|@@?@$|");

            //紧急度
            SQLText.Append("CtrFCDInstancy.CatelogID=" + CtrFCDInstancy.CatelogID.ToString().Trim() + "|@@?@$|");

            //标题
            SQLText.Append("txtSubject.Text=" + txtSubject.Text.ToString().Trim() + "|@@?@$|");

            //执行人
            SQLText.Append("UserPSjzxr.UserID=" + UserPSjzxr.UserID.ToString().Trim() + "|@@?@$|");

            //登单人
            SQLText.Append("txtperson.Text=" + txtperson.Text.ToString().Trim() + "|@@?@$|");

            //服务级别
            SQLText.Append("ddltServiceLevel.SelectedValue=" + ddltServiceLevel.SelectedValue.ToString().Trim() + "|@@?@$|");

            //资产信息
            SQLText.Append("txtEqu.Text=" + txtEqu.Text.ToString().Trim() + "|@@?@$|");

            //邮件回访状态
            SQLText.Append("ddltEmailState.SelectedValue=" + ddltEmailState.SelectedValue.ToString().Trim() + "|@@?@$|");

            //保存条件名
            SQLText.Append("txtSQLName.Text=" + SQLName.Trim());

            string SQLstr = "";

            if (IsExistQuery(" and Name='Temp1' and FORMID='CST_Issue_List' and  SQLWhere='Temp1' and LOGINNAME=" + StringTool.SqlQ(Session["UserName"].ToString())))
            {
                 if (!string.IsNullOrEmpty(this.hidValue.Value))
                {
                    SQLstr = "update CST_ISSUE_QUERYSave set SQLText=" + StringTool.SqlQ(SQLText.ToString().Trim()) + ",DISPLAYCOLUMN=" + StringTool.SqlQ(this.hidValue.Value.ToString().Trim()) +
                        " where FORMID='CST_Issue_List' and  LOGINNAME=" + StringTool.SqlQ(Session["UserName"].ToString()) + " and Name='Temp1' and  SQLWhere='Temp1'";
                }
                 else
                 {
                     SQLstr = "update CST_ISSUE_QUERYSave set SQLText=" + StringTool.SqlQ(SQLText.ToString().Trim()) +
                             " where FORMID='CST_Issue_List' and  LOGINNAME=" + StringTool.SqlQ(Session["UserName"].ToString()) + " and Name='Temp1' and  SQLWhere='Temp1'";
                 }
            }
            else
            {
                string strID = EpowerGlobal.EPGlobal.GetNextID("CST_ISSUE_QUERYSaveID").ToString();

                SQLstr = "insert into CST_ISSUE_QUERYSave(ID,Name,SQLWhere,FORMID,LOGINNAME,SQLText,DISPLAYCOLUMN)" +
                        " values(" + strID + ",'Temp1','Temp1','CST_Issue_List'," +
                        StringTool.SqlQ(Session["UserName"].ToString()) + "," + StringTool.SqlQ(SQLText.ToString().Trim()) + "," + StringTool.SqlQ(this.hidValue.Value.ToString().Trim()) + ")";
            }
            OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationSettings.AppSettings["SQLConnString"]);
            try
            {
                int i = OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, SQLstr);                
            }
            finally { ConfigTool.CloseConnection(cn); }
        }

        #endregion

        #region 判断查询条件是否存在
        /// <summary>
        /// 判断查询条件是否存在
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public static bool IsExistQuery(string strWhere)
        {
            bool result = false;
            string strSql = "select * from CST_ISSUE_QUERYSave where rownum<=1 " + strWhere;
            DataTable dt = CommonDP.ExcuteSqlTable(strSql);
            if (dt != null && dt.Rows.Count > 0)
                result = true;

            return result;
        }
        #endregion

        #region  收藏条件下拉框加载数据
        private void InitDropSQLwSave()
        {
            DataTable dt = ZHServiceDP.getCST_ISSUE_LISTFASTQUERY("CST_Issue_List", Session["UserName"].ToString(), string.Empty);
            DropSQLwSave.Items.Clear();
            DropSQLwSave.DataSource = dt.DefaultView;
            DropSQLwSave.DataTextField = "Name";
            DropSQLwSave.DataValueField = "ID";
            DropSQLwSave.DataBind();
            DropSQLwSave.Items.Insert(0, new ListItem("==选择收藏查询条件==", "0"));

            if (txtSQLName.Text.Trim() != string.Empty)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Name"].ToString().Trim() == txtSQLName.Text.Trim())
                    {
                        DropSQLwSave.SelectedIndex = DropSQLwSave.Items.IndexOf(DropSQLwSave.Items.FindByValue(dt.Rows[i]["ID"].ToString().Trim()));
                        txtSQLName.Text = DropSQLwSave.SelectedItem.Text == "==选择收藏查询条件==" ? "" : DropSQLwSave.SelectedItem.Text;
                    }
                }
            }

            if (DropSQLwSave.SelectedItem.Value == "0")
            {
                btn_delete.Text = "清空";
            }
            else
            {
                btn_delete.Text = "删除";
            }
        }

        private void InitDropSQLwSave1(string SQLName)
        {
            DataTable dt = ZHServiceDP.getCST_ISSUE_LISTFASTQUERY("CST_Issue_List", Session["UserName"].ToString(), string.Empty);
            DropSQLwSave.Items.Clear();
            DropSQLwSave.DataSource = dt.DefaultView;
            DropSQLwSave.DataTextField = "Name";
            DropSQLwSave.DataValueField = "ID";
            DropSQLwSave.DataBind();
            DropSQLwSave.Items.Insert(0, new ListItem("==选择收藏查询条件==", "0"));


            if (SQLName.Trim() != "Temp1")
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Name"].ToString().Trim() == SQLName.Trim())
                    {
                        DropSQLwSave.SelectedIndex = DropSQLwSave.Items.IndexOf(DropSQLwSave.Items.FindByValue(dt.Rows[i]["ID"].ToString().Trim()));
                        txtSQLName.Text = DropSQLwSave.SelectedItem.Text == "==选择收藏查询条件==" ? "" : DropSQLwSave.SelectedItem.Text;
                    }
                }
            }
            else
            {
                DropSQLwSave.SelectedValue = "0";
            }

        }
        #endregion

        #region 根据选择条件加载界面控件值

        protected void SqlWhereShow(string SQLName)
        {
            DataTable dt = ZHServiceDP.getCST_ISSUE_LISTFASTQUERY("CST_Issue_List", Session["UserName"].ToString(), SQLName);

            if (dt.Rows.Count > 0)
            {
                string[] SQLTextDetailList = dt.Rows[0]["SQLText"].ToString().Split(new string[] { "|@@?@$|" }, StringSplitOptions.None);

                for (int i = 0; i < SQLTextDetailList.Length; i++)
                {
                    string[] SQLTextDetail = SQLTextDetailList[i].Trim().Split('=');

                    //客户名称
                    if (SQLTextDetail[0].Trim() == "txtCustName.Text")
                    {
                        this.txtCustName.Text = SQLTextDetail[1].Trim();
                    }

                    //事件单号
                    if (SQLTextDetail[0].Trim() == "txtIssueNo.Text")
                    {
                        this.txtIssueNo.Text = SQLTextDetail[1].Trim();
                    }

                    //流程状态
                    if (SQLTextDetail[0].Trim() == "cboStatus.SelectedValue")
                    {
                        this.cboStatus.SelectedValue = SQLTextDetail[1].Trim();
                    }

                    //发生时间开始
                    if (SQLTextDetail[0].Trim() == "ctrDateSelectTime1.BeginTime")
                    {
                        this.ctrDateSelectTime1.BeginTime = SQLTextDetail[1].Trim();
                    }

                    //发生时间结束
                    if (SQLTextDetail[0].Trim() == "ctrDateSelectTime1.EndTime")
                    {
                        this.ctrDateSelectTime1.EndTime = SQLTextDetail[1].Trim();
                    }

                    //影响度
                    if (SQLTextDetail[0].Trim() == "CtrFCDEffect.CatelogID")
                    {
                        this.CtrFCDEffect.CatelogID = Convert.ToInt64(SQLTextDetail[1].Trim());
                    }

                    //事件状态
                    if (SQLTextDetail[0].Trim() == "ddlDealStatus.SelectedValue")
                    {
                        this.ctrDealStatus.CatelogID = Convert.ToInt64(SQLTextDetail[1].Trim());
                    }

                    //紧急度
                    if (SQLTextDetail[0].Trim() == "CtrFCDInstancy.CatelogID")
                    {
                        this.CtrFCDInstancy.CatelogID = Convert.ToInt64(SQLTextDetail[1].Trim());
                    }

                    //事件类别
                    if (SQLTextDetail[0].Trim() == "ctrServiceType.CatelogID")
                    {
                        this.ctrServiceType.CatelogID = Convert.ToInt64(SQLTextDetail[1].Trim());
                        this.hidServiceTypeID.Value = SQLTextDetail[1].Trim();
                    }

                    //事件性质
                    if (SQLTextDetail[0].Trim() == "ctrFCDWTType.CatelogID")
                    {
                        this.ctrFCDWTType.CatelogID = Convert.ToInt64(SQLTextDetail[1].Trim());
                    }

                    //执行人
                    if (SQLTextDetail[0].Trim() == "UserPSjzxr.UserID")
                    {
                        this.UserPSjzxr.UserID = SQLTextDetail[1].Trim();

                        UserPSjzxr.UserName = ZHServiceDP.GetStaffNamesByStaffIDs(UserPSjzxr.UserID == "" ? "0" : UserPSjzxr.UserID);
                    }

                    //标题
                    if (SQLTextDetail[0].Trim() == "txtSubject.Text")
                    {
                        this.txtSubject.Text = SQLTextDetail[1].Trim();
                    }

                    //登单人
                    if (SQLTextDetail[0].Trim() == "txtperson.Text")
                    {
                        this.txtperson.Text = SQLTextDetail[1].Trim();
                    }

                    //服务级别
                    if (SQLTextDetail[0].Trim() == "ddltServiceLevel.SelectedValue")
                    {
                        this.ddltServiceLevel.SelectedIndex = ddltServiceLevel.Items.IndexOf(ddltServiceLevel.Items.FindByValue(SQLTextDetail[1].Trim()));
                        this.hidServiceLevelID.Value = (ddltServiceLevel.Items.IndexOf(ddltServiceLevel.Items.FindByValue(SQLTextDetail[1].Trim()))).ToString();

                    }

                    //资产信息
                    if (SQLTextDetail[0].Trim() == "txtEqu.Text")
                    {
                        this.txtEqu.Text = SQLTextDetail[1].Trim();
                        this.lblEqu.Text = SQLTextDetail[1].Trim();
                    }

                    //邮件回访状态
                    if (SQLTextDetail[0].Trim() == "ddltEmailState.SelectedValue")
                    {
                        ddltEmailState.SelectedIndex = ddltEmailState.Items.IndexOf(ddltEmailState.Items.FindByValue(SQLTextDetail[1].Trim()));
                    }
                }

            }
        }

        #endregion

        #endregion

    }
}
