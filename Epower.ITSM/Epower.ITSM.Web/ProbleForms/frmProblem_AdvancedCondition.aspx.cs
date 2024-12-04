using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Epower.ITSM.SqlDAL;
using System.Text;
using EpowerGlobal;
using Epower.DevBase.BaseTools;
using System.Data.OracleClient;
using Epower.ITSM.SqlDAL.Common;

namespace Epower.ITSM.Web.ProbleForms
{
    public partial class frmProblem_AdvancedCondition : BasePage
    {
        #region 表名或视图名
        private string viewName;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                viewName = Request.QueryString["viewName"];

                cboStatus.Items.Add(new ListItem("所有状态", "-1"));
                cboStatus.Items.Add(new ListItem("--正在处理", ((int)e_FlowStatus.efsHandle).ToString()));
                cboStatus.Items.Add(new ListItem("--正常结束", ((int)e_FlowStatus.efsEnd).ToString()));
                cboStatus.Items.Add(new ListItem("--流程暂停", ((int)e_FlowStatus.efsStop).ToString()));
                cboStatus.Items.Add(new ListItem("--流程终止", ((int)e_FlowStatus.efsAbort).ToString()));
                cboStatus.SelectedIndex = 0;

                if (Request["SQLName"] != null)
                {
                    InitDropSQLwSave(Request["SQLName"].ToString().Trim());  //初始化查询条件

                    SqlWhereShow(Request["SQLName"].ToString().Trim() == "" ? "Temp1" : Request["SQLName"].ToString().Trim());
                }

                if (DropSQLwSave.SelectedValue != "")
                {
                    if (DropSQLwSave.SelectedItem.Value == "0")
                    {
                        btn_delete.Text = "清空";
                    }
                    else
                    {
                        btn_delete.Text = "删除";
                    }
                }


                //设置显示
                PageDeal.SetLanguage(this.Controls[0]);                
                if (!string.IsNullOrEmpty(viewName))
                    DataBing();
            }
            if (DropSQLwSave.SelectedValue != "")
            {
                if (DropSQLwSave.SelectedItem.Value == "0")
                {
                    btn_delete.Text = "清空";
                }
                else
                {
                    btn_delete.Text = "删除";
                }
            }
        }
        public string Opener_ClientId
        {

            get
            {
                return (Request["Opener_ClientId"] == null) ? "" : Request["Opener_ClientId"].ToString().Replace("hidClientId_ForOpenerPage", "");
            }
        }

        #region 保存查询条件名称
        /// <summary>
        /// 保存查询条件名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        #region InitDropSQLwSave不带参

        /// <summary>
        /// InitDropSQLwSave
        /// </summary>
        private void InitDropSQLwSave()
        {
            DataTable dt = ProblemDealDP.getCST_ISSUE_LISTFASTQUERY("frmProblemMain", Session["UserName"].ToString(), string.Empty);
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
        #endregion

        #region InitDropSQLwSave带参
        /// <summary>
        /// InitDropSQLwSave
        /// </summary>
        /// <param name="SQLName"></param>
        private void InitDropSQLwSave(string SQLName)
        {
            DataTable dt = ProblemDealDP.getCST_ISSUE_LISTFASTQUERY("frmProblemMain", Session["UserName"].ToString(), string.Empty);
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

        /// <summary>
        /// 根据选择条件加载界面控件值

        /// </summary>
        /// <param name="SQLName"></param>
        protected void SqlWhereShow(string SQLName)
        {
            DataTable dt = ProblemDealDP.getCST_ISSUE_LISTFASTQUERY("frmProblemMain", Session["UserName"].ToString(), SQLName);

            if (dt.Rows.Count > 0)
            {
                string[] SQLTextDetailList = dt.Rows[0]["SQLText"].ToString().Split(new string[] { "|@@?@$|" }, StringSplitOptions.None);

                for (int i = 0; i < SQLTextDetailList.Length; i++)
                {
                    string[] SQLTextDetail = SQLTextDetailList[i].Trim().Split('=');

                    //问题单号
                    if (SQLTextDetail[0].Trim() == "txtProblemNo.Text")
                    {
                        this.txtProblemNo.Text = SQLTextDetail[1].Trim();
                    }

                    //资产名称
                    if (SQLTextDetail[0].Trim() == "txtEquName.Text")
                    {
                        this.txtEquName.Text = SQLTextDetail[1].Trim();
                    }

                    //流程状态

                    if (SQLTextDetail[0].Trim() == "cboStatus.SelectedValue")
                    {
                        this.cboStatus.SelectedValue = SQLTextDetail[1].Trim();
                    }

                    //问题状态

                    if (SQLTextDetail[0].Trim() == "ddlDealStatus.SelectedValue")
                    {
                        this.CtrDealStatus.CatelogID = Convert.ToInt64(SQLTextDetail[1].Trim());
                    }

                    //登记时间开始

                    if (SQLTextDetail[0].Trim() == "ctrDateReSetTime.BeginTime")
                    {
                        this.ctrDateReSetTime.BeginTime = SQLTextDetail[1].Trim();
                    }

                    //登记时间结束
                    if (SQLTextDetail[0].Trim() == "ctrDateReSetTime.EndTime")
                    {
                        this.ctrDateReSetTime.EndTime = SQLTextDetail[1].Trim();
                    }

                    //标题
                    if (SQLTextDetail[0].Trim() == "txtSubject.Text")
                    {
                        this.txtSubject.Text = SQLTextDetail[1].Trim();
                    }

                    //问题类别
                    if (SQLTextDetail[0].Trim() == "CataProblemType.CatelogID")
                    {
                        this.CataProblemType.CatelogID = Convert.ToInt64(SQLTextDetail[1].Trim());
                    }

                    //问题级别
                    if (SQLTextDetail[0].Trim() == "CataProblemLevel.CatelogID")
                    {
                        this.CataProblemLevel.CatelogID = Convert.ToInt64(SQLTextDetail[1].Trim());
                    }

                    //影响度

                    if (SQLTextDetail[0].Trim() == "CtrFCDEffect.CatelogID")
                    {
                        this.CtrFCDEffect.CatelogID = Convert.ToInt64(SQLTextDetail[1].Trim());
                    }

                    //紧急度
                    if (SQLTextDetail[0].Trim() == "CtrFCDInstancy.CatelogID")
                    {
                        this.CtrFCDInstancy.CatelogID = Convert.ToInt64(SQLTextDetail[1].Trim());
                    }

                    //登记人

                    if (SQLTextDetail[0].Trim() == "txtRegUser.Text")
                    {
                        this.txtRegUser.Text = SQLTextDetail[1].Trim();
                    }
                }

                /*
                 * ####
                 * 加载显示字段
                 * ####
                 * **/
                String strDisplayColumn = dt.Rows[0]["DISPLAYCOLUMN"].ToString();
                if (!String.IsNullOrEmpty(strDisplayColumn))
                {
                    //// Content,Email,
                    String[] _strValArray = strDisplayColumn.Split(new String[] { "," },
                        StringSplitOptions.RemoveEmptyEntries);

                    SeleteTableColumn st = new SeleteTableColumn();
                    dt = SeleteTableColumn.GetTableColumnName(viewName);

                    // 清除上次选择.
                    lsbDeptTo.Items.Clear();

                    for (int i = 0; i < _strValArray.Length; i++)
                    {
                        String strVal = _strValArray[i].Trim();
                        String strKey = String.Empty;
                        foreach (DataRow item in dt.Rows)
                        {
                            String _val = item["column_name"].ToString().Trim();
                            if (_val.Trim().Equals(strVal))
                            {
                                strKey = item["keyValue"].ToString().Trim();
                                break;
                            }
                        }

                        lsbDeptTo.Items.Add(new ListItem(strKey,
                            strVal));
                    }
                }

            }
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

        #region 保存查询条件
        /// <summary>
        /// 保存查询条件
        /// </summary>
        protected void SqlWhereSave()
        {
            if (txtSQLName.Text.Trim() != string.Empty)
            {
                StringBuilder SQLText = new StringBuilder();

                SQLText = GetQuerys(SQLText);

                //保存条件名

                SQLText.Append("txtSQLName.Text=" + txtSQLName.Text.ToString().Trim());

             
                string SQLstr = "";
           
                if (IsExistQuery(" and Name=" + StringTool.SqlQ(this.txtSQLName.Text.Trim()) + " and  nvl(SQLWhere,' ')!='Temp1'  and FORMID='frmProblemMain'"))
                {
                    if (!String.IsNullOrEmpty(this.hidDeptID.Value))
                    {
                        SQLstr = "update CST_ISSUE_QUERYSave set SQLText=" + StringTool.SqlQ(SQLText.ToString().Trim())
                        + ",DISPLAYCOLUMN=" + StringTool.SqlQ(this.hidDeptID.Value.ToString().Trim()) +
                        " where FORMID='frmProblemMain' and  nvl(SQLWhere,' ')!='Temp1' and  LOGINNAME=" + StringTool.SqlQ(Session["UserName"].ToString()) + " and Name=" + StringTool.SqlQ(this.txtSQLName.Text.Trim());
                    }
                    else
                    {
                        SQLstr = "update CST_ISSUE_QUERYSave set SQLText=" + StringTool.SqlQ(SQLText.ToString().Trim()) +
                        " where FORMID='frmProblemMain' and  nvl(SQLWhere,' ')!='Temp1' and  LOGINNAME=" + StringTool.SqlQ(Session["UserName"].ToString()) + " and Name=" + StringTool.SqlQ(this.txtSQLName.Text.Trim());
                    }
                }
                else
                {

                    string strID = EpowerGlobal.EPGlobal.GetNextID("CST_ISSUE_QUERYSaveID").ToString();

                    SQLstr = "insert into CST_ISSUE_QUERYSave(ID,Name,FORMID,SQLWhere,LOGINNAME,SQLText,DISPLAYCOLUMN)" +
                            " values(" + strID + "," + StringTool.SqlQ(this.txtSQLName.Text.Trim()) + ",'frmProblemMain',''," +
                            StringTool.SqlQ(Session["UserName"].ToString()) + "," + StringTool.SqlQ(SQLText.ToString().Trim()) + ","
                            + StringTool.SqlQ(this.hidDeptID.Value.ToString().Trim()) + ")";
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

            SQLText = GetQuerys(SQLText);

            //保存条件名

            SQLText.Append("txtSQLName.Text=" + SQLName.Trim());

      
            string SQLstr = "";

            if (IsExistQuery(" and Name='Temp1' and FORMID='frmProblemMain' and  SQLWhere='Temp1' and LOGINNAME=" + StringTool.SqlQ(Session["UserName"].ToString())))
            {
                if (!String.IsNullOrEmpty(this.hidDeptID.Value))
                {
                    SQLstr = "update CST_ISSUE_QUERYSave set SQLText=" + StringTool.SqlQ(SQLText.ToString().Trim())
                        + ",DISPLAYCOLUMN=" + StringTool.SqlQ(this.hidDeptID.Value.ToString().Trim()) +
                " where FORMID='frmProblemMain' and  LOGINNAME=" + StringTool.SqlQ(Session["UserName"].ToString()) + " and Name='Temp1' and  SQLWhere='Temp1'";
                }
                else
                {
                    SQLstr = "update CST_ISSUE_QUERYSave set SQLText=" + StringTool.SqlQ(SQLText.ToString().Trim()) +
                " where FORMID='frmProblemMain' and  LOGINNAME=" + StringTool.SqlQ(Session["UserName"].ToString()) + " and Name='Temp1' and  SQLWhere='Temp1'";
                }
            }
            else
            {

                string strID = EpowerGlobal.EPGlobal.GetNextID("CST_ISSUE_QUERYSaveID").ToString();

                SQLstr = "insert into CST_ISSUE_QUERYSave(ID,Name,SQLWhere,FORMID,LOGINNAME,SQLText,DISPLAYCOLUMN)" +
                        " values(" + strID + ",'Temp1','Temp1','frmProblemMain'," +
                        StringTool.SqlQ(Session["UserName"].ToString()) + "," + StringTool.SqlQ(SQLText.ToString().Trim()) + ","
                            + StringTool.SqlQ(this.hidDeptID.Value.ToString().Trim()) + ")";
            }

            OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationSettings.AppSettings["SQLConnString"]);
            try { int i = OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, SQLstr); }
            finally { ConfigTool.CloseConnection(cn); }
            
        }
        #endregion

        #region GetQuerys
        /// <summary>
        /// GetQuerys
        /// </summary>
        /// <param name="SQLText"></param>
        /// <returns></returns>
        private StringBuilder GetQuerys(StringBuilder SQLText)
        {
            //问题单号
            SQLText.Append("txtProblemNo.Text=" + txtProblemNo.Text.ToString().Trim() + "|@@?@$|");

            //资产名称
            SQLText.Append("txtEquName.Text=" + txtEquName.Text.ToString().Trim() + "|@@?@$|");

            //流程状态

            SQLText.Append("cboStatus.SelectedValue=" + cboStatus.SelectedValue.ToString().Trim() + "|@@?@$|");

            //问题状态


            SQLText.Append("ddlDealStatus.SelectedValue=" + CtrDealStatus.CatelogID.ToString().Trim() + "|@@?@$|");

            //登记时间开始

            SQLText.Append("ctrDateReSetTime.BeginTime=" + ctrDateReSetTime.BeginTime.ToString().Trim() + "|@@?@$|");

            //登记时间结束
            SQLText.Append("ctrDateReSetTime.EndTime=" + ctrDateReSetTime.EndTime.ToString().Trim() + "|@@?@$|");

            //标题
            SQLText.Append("txtSubject.Text=" + txtSubject.Text.ToString().Trim() + "|@@?@$|");

            //问题类别
            SQLText.Append("CataProblemType.CatelogID=" + CataProblemType.CatelogID.ToString().Trim() + "|@@?@$|");

            //问题级别
            SQLText.Append("CataProblemLevel.CatelogID=" + CataProblemLevel.CatelogID.ToString().Trim() + "|@@?@$|");

            //影响度

            SQLText.Append("CtrFCDEffect.CatelogID=" + CtrFCDEffect.CatelogID.ToString().Trim() + "|@@?@$|");

            //紧急度
            SQLText.Append("CtrFCDInstancy.CatelogID=" + CtrFCDInstancy.CatelogID.ToString().Trim() + "|@@?@$|");

            //登记人

            SQLText.Append("txtRegUser.Text=" + txtRegUser.Text.ToString().Trim() + "|@@?@$|");

            return SQLText;
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
            sbText.Append("arr[1] ='" + this.hidDeptID.Value + "';");
            // 名称
            sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidIsGaoji", "1");
            sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidSQLName", SQLName);
            sbText.AppendFormat("window.opener.document.all.{0}.value={1};", Opener_ClientId + "HiddenColumn", "arr[1]");
            // sbText.Append("window.opener.location.reload();");
            sbText.AppendFormat("window.opener.document.all.{0}.click();", Opener_ClientId + "btnOk");

            // 关闭窗口
            sbText.Append("top.close();");
            sbText.Append("</script>");
            // 向客户端发送

            Page.RegisterStartupScript(DateTime.Now.ToString(), sbText.ToString());
            Response.Write(sbText.ToString());
        }
        #endregion

        #region 关闭
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Write("<script>window.close();</script>");
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

                txtProblemNo.Text = string.Empty;                   //问题单号
                txtEquName.Text = string.Empty;                     //资产名称
                cboStatus.SelectedIndex = 0;                        //流程状态


                CtrDealStatus.CatelogID = 0;                    //问题状态

                ctrDateReSetTime.BeginTime = string.Empty;          //登记时间开始时间

                ctrDateReSetTime.EndTime = string.Empty;            //登记时间结束时间
                txtSubject.Text = string.Empty;                     //标题
                CataProblemType.CatelogID = 0;                      //问题类别
                CataProblemLevel.CatelogID = 0;                     //问题级别
                CtrFCDEffect.CatelogID = 0;                         //影响度

                CtrFCDInstancy.CatelogID = 0;                       //紧急度
                txtRegUser.Text = string.Empty;                     //登单人

            }

            ProblemDealDP.deleteCST_ISSUE_Where(Session["UserName"].ToString(), DropSQLwSave.SelectedItem.Text.ToString());
            this.txtSQLName.Text = string.Empty;
            InitDropSQLwSave();
        }

        #region 收藏条件下拉框事件 DropSQLwSave_SelectedIndexChanged
        /// <summary>
        /// 收藏条件下拉框事件 DropSQLwSave_SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DropSQLwSave_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strTemp = string.Empty;              //临时存放改变后的下拉列表内容

            //if (DropSQLwSave.SelectedItem.Text != "==选择收藏查询条件==")
            if (1 == 1)
            {
                strTemp = DropSQLwSave.SelectedValue;           //将选择的高级条件名称存储起来

                viewName = Request.QueryString["viewName"];

                SqlWhereShow(DropSQLwSave.SelectedItem.Text.ToString());

                //更新访问次数
                ProblemDealDP.updateCST_ISSUE_WhereNums(Session["UserName"].ToString(), DropSQLwSave.SelectedItem.Text.ToString());
                DataTable dt = ProblemDealDP.getCST_ISSUE_LISTFASTQUERY("frmProblemMain", Session["UserName"].ToString(), string.Empty);
                DropSQLwSave.Items.Clear();
                DropSQLwSave.DataSource = dt.DefaultView;
                DropSQLwSave.DataTextField = "Name";
                DropSQLwSave.DataValueField = "ID";
                DropSQLwSave.DataBind();
                DropSQLwSave.Items.Insert(0, new ListItem("==选择收藏查询条件==", "0"));

                DropSQLwSave.SelectedIndex = DropSQLwSave.Items.IndexOf(DropSQLwSave.Items.FindByValue(strTemp));

                if (DropSQLwSave.SelectedItem.Text != "==选择收藏查询条件==")
                    txtSQLName.Text = DropSQLwSave.SelectedItem.Text;
                else txtSQLName.Text = String.Empty;
            }
            else
            {
                txtSQLName.Text = "";
            }
        }

        #endregion

        #endregion

        #region 获得表的字段
        void DataBing()
        {
            SeleteTableColumn st = new SeleteTableColumn();
            DataTable dt = SeleteTableColumn.GetTableColumnName(viewName);
            this._TableColumnCheckBoxList.DataSource = dt;
            this._TableColumnCheckBoxList.DataTextField = "keyValue";
            this._TableColumnCheckBoxList.DataValueField = "column_name";
            this._TableColumnCheckBoxList.DataBind();

        }

        #endregion
    }
}
