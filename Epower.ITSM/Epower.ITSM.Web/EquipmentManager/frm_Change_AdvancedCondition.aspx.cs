/*******************************************************************
 * 版权所有：
 * Description：变更单高级查询页面
 * Create By  ：SuperMan
 * Create Date：2011-08-22
 * *****************************************************************/
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
using Epower.DevBase.BaseTools;
using System.Data.OracleClient;
using System.Text;
using Epower.ITSM.SqlDAL.Common;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frm_Change_AdvancedCondition : BasePage
    {
        static string staMsgDateBegion = "";
        #region 表名或视图名
        private string viewName;
        #endregion
        /// <summary>
        /// 取得流程FlowID
        /// </summary>
        /// <summary>
        /// 取得流程FlowID
        /// </summary>
        public string FlowID
        {
            get
            {
                if (ViewState["frm_Issue_Base_FlowID"] != null)
                {
                    return ViewState["frm_Issue_Base_FlowID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["frm_Issue_Base_FlowID"] = value;
            }
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                viewName = Request.QueryString["viewName"];

                //快速搜索
                if (Request["svalue"] != null)
                {
                    txtCustInfo.Text = Request["svalue"].ToString().Trim();
                }

                cboStatus.Items.Add(new ListItem("所有状态", "-1"));
                cboStatus.Items.Add(new ListItem("--正在处理", ((int)EpowerGlobal.e_FlowStatus.efsHandle).ToString()));
                cboStatus.Items.Add(new ListItem("--正常结束", ((int)EpowerGlobal.e_FlowStatus.efsEnd).ToString()));
                cboStatus.Items.Add(new ListItem("--流程暂停", ((int)EpowerGlobal.e_FlowStatus.efsStop).ToString()));
                cboStatus.Items.Add(new ListItem("--流程终止", ((int)EpowerGlobal.e_FlowStatus.efsAbort).ToString()));
                cboStatus.SelectedIndex = 1;

                string sQueryBeginDate = "0";
                if (CommonDP.GetConfigValue("Other", "QueryBeginDate") != null)
                    sQueryBeginDate = CommonDP.GetConfigValue("Other", "QueryBeginDate").ToString();
                if (sQueryBeginDate == "0")
                {
                    ctrDateSelectTime1.BeginTime = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                    staMsgDateBegion = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                }
                else
                {
                    ctrDateSelectTime1.BeginTime = DateTime.Parse(sQueryBeginDate).ToString("yyyy-MM-dd");
                    staMsgDateBegion = DateTime.Parse(sQueryBeginDate).ToString("yyyy-MM-dd");
                }
                ctrDateSelectTime1.EndTime = DateTime.Now.ToString("yyyy-MM-dd");

                if (Request["SQLName"] != null)
                {
                    InitDropSQLwSave1(Request["SQLName"].ToString().Trim());  //初始化查询条件
                    SqlWhereShow(Request["SQLName"].ToString().Trim());
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
                PageDeal.SetLanguage(this.Controls[0]);

                if (!string.IsNullOrEmpty(viewName))
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

        #region 保存查询条件名称

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (this.txtSQLName.Text.Trim() == string.Empty)
            {
                PageTool.MsgBox(this.Page, "查询条件名称不能为空！");
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
                cboStatus.SelectedIndex =0;
                CtrDealStatus.CatelogID = 0;
                CtrFCDlevel.CatelogID = 0;
                CtrFCDEffect.CatelogID = 0;
                CtrFCDEffect.CatelogID = 0;
                CtrFCDInstancy.CatelogID = 0;
                CtrChangeType.CatelogID = 0; //变更类别
                txtCustInfo.Text = "";
                txtEquipmentName.Text = "";
                txtEquipmentDir.Text = "";
                txtTitle.Text = "";
                ctrDateSelectTime1.BeginTime = "";      //发生时间开始时间
                ctrDateSelectTime1.EndTime = "";        //发生时间结束时间
            }

            ChangeDealDP.deleteCST_ISSUE_Where(Session["UserName"].ToString(), DropSQLwSave.SelectedItem.Text.ToString());
            this.txtSQLName.Text = string.Empty;
            InitDropSQLwSave();
        }

        #endregion

        #region 确定

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
            //=========zxl======
                
            //================
            //  sbText.Append("window.parent.returnValue = arr;");
            sbText.Append(
             " var arrValue=arr;" +
            " if(arrValue != null)" +
            "{" +
           " window.opener.document.getElementById('" + Opener_ClientId + "hidSQLName').value = arrValue[0];" +
            " window.opener.document.getElementById('" + Opener_ClientId + "HiddenColumn').value = arrValue[1]; " +
           " window.opener.document.getElementById('" + Opener_ClientId + "btnOk').click();" +
        "}");

            // 关闭窗口
            sbText.Append("top.close();");
            sbText.Append("</script>");
            // 向客户端发送
            Page.RegisterStartupScript(DateTime.Now.ToString(), sbText.ToString());
            Response.Write(sbText.ToString());
        }

        #endregion

        #region 收藏条件下拉框事件
        protected void DropSQLwSave_SelectedIndexChanged(object sender, EventArgs e)
        {
            viewName = Request.QueryString["viewName"];

            if (DropSQLwSave.SelectedItem.Text != "==选择收藏查询条件==")            
            {                
                SqlWhereShow(DropSQLwSave.SelectedItem.Text.ToString());
                this.txtSQLName.Text = DropSQLwSave.SelectedItem.Text.ToString();
            }
            else
            {
                SqlWhereShow("Temp1");
                this.txtSQLName.Text = "";
            }
        }

        #endregion

        #region 自定义方法

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

        #region 保存查询条件方法

        protected void SqlWhereSave()
        {
            if (txtSQLName.Text.Trim() != string.Empty)
            {
                StringBuilder SQLText = new StringBuilder();

                //用户信息
                SQLText.Append("txtCustInfo.Text=" + this.txtCustInfo.Text.Trim() + "|@@?@$|");

                //流程状态
                SQLText.Append("cboStatus.SelectedValue=" + cboStatus.SelectedValue.ToString().Trim() + "|@@?@$|");

                //变更状态


                SQLText.Append("CtrFlowChangeState.SelectedValue=" + CtrDealStatus.CatelogID.ToString().Trim() + "|@@?@$|");

                //标题
                SQLText.Append("txtTitle.Text=" + this.txtTitle.Text.Trim() + "|@@?@$|");

                //变更时间
                SQLText.Append("ctrDateSelectTime1.BeginTime=" + ctrDateSelectTime1.BeginTime.ToString().Trim() + "|@@?@$|");
                SQLText.Append("ctrDateSelectTime1.EndTime=" + ctrDateSelectTime1.EndTime.ToString().Trim() + "|@@?@$|");

                //资产目录
                SQLText.Append("txtEquipmentDir.Text=" + txtEquipmentDir.Text.Trim() + "|@@?@$|");

                //资产名称
                SQLText.Append("txtEquipmentName.Text=" + txtEquipmentName.Text.Trim() + "|@@?@$|");

                //变更级别
                SQLText.Append("CtrFCDlevel.CatelogID=" + CtrFCDlevel.CatelogID.ToString().Trim() + "|@@?@$|");

                //影响度
                SQLText.Append("CtrFCDEffect.CatelogID=" + CtrFCDEffect.CatelogID.ToString().Trim() + "|@@?@$|");

                //紧急度
                SQLText.Append("CtrFCDInstancy.CatelogID=" + CtrFCDInstancy.CatelogID.ToString().Trim() + "|@@?@$|");

                //变更类别
                SQLText.Append("CtrChangeType.CatelogID=" + CtrChangeType.CatelogID.ToString().Trim() + "|@@?@$|");

                //保存条件名
                SQLText.Append("txtSQLName.Text=" + txtSQLName.Text.ToString().Trim());

                string SQLstr = "";

                if (IsExistQuery(" and LOGINNAME=" + StringTool.SqlQ(Session["UserName"].ToString()) + "  and  nvl(SQLWhere,' ')!='Temp1' and FORMID='frm_ChangeQuery' and  Name=" + StringTool.SqlQ(txtSQLName.Text.Trim())))
                {
                    if (!String.IsNullOrEmpty(this.hidDeptID.Value))
                    {
                        SQLstr = "update CST_ISSUE_QUERYSave set SQLText=" + StringTool.SqlQ(SQLText.ToString().Trim()) 
                             +",DISPLAYCOLUMN=" + StringTool.SqlQ(this.hidDeptID.Value.ToString().Trim()) +
        "  where FORMID='frm_ChangeQuery' and  nvl(SQLWhere,' ')!='Temp1' and LOGINNAME=" + StringTool.SqlQ(Session["UserName"].ToString()) + " and Name=" + StringTool.SqlQ(this.txtSQLName.Text.Trim());
                    }
                    else {
                        SQLstr = "update CST_ISSUE_QUERYSave set SQLText=" + StringTool.SqlQ(SQLText.ToString().Trim()) +
        " where FORMID='frm_ChangeQuery' and  nvl(SQLWhere,' ')!='Temp1' and LOGINNAME=" + StringTool.SqlQ(Session["UserName"].ToString()) + " and Name=" + StringTool.SqlQ(this.txtSQLName.Text.Trim());
                    }
                }
                else
                {
                    string strID = EpowerGlobal.EPGlobal.GetNextID("CST_ISSUE_QUERYSaveID").ToString();

                    SQLstr = "insert into CST_ISSUE_QUERYSave(ID,Name,FORMID,SQLWhere,LOGINNAME,SQLText,DISPLAYCOLUMN)" +
                            " values(" + strID + "," + StringTool.SqlQ(this.txtSQLName.Text.Trim()) + ",'frm_ChangeQuery',''," +
                            StringTool.SqlQ(Session["UserName"].ToString()) + "," + StringTool.SqlQ(SQLText.ToString().Trim()) + ","
                            + StringTool.SqlQ(this.hidDeptID.Value.ToString().Trim()) + ")";
                }

                OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationSettings.AppSettings["SQLConnString"]);

                try
                {
                    int i = OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, SQLstr);
                }
                finally { ConfigTool.CloseConnection(cn); }
            }
        }

        protected void SqlWhereSaveTemp1(string SQLName)
        {
            StringBuilder SQLText = new StringBuilder();

            //用户信息
            SQLText.Append("txtCustInfo.Text=" + this.txtCustInfo.Text.Trim() + "|@@?@$|");

            //流程状态
            SQLText.Append("cboStatus.SelectedValue=" + cboStatus.SelectedValue.ToString().Trim() + "|@@?@$|");

            //变更状态


            SQLText.Append("CtrFlowChangeState.SelectedValue=" + CtrDealStatus.CatelogID.ToString().Trim() + "|@@?@$|");

            //标题
            SQLText.Append("txtTitle.Text=" + this.txtTitle.Text.Trim() + "|@@?@$|");

            //变更时间
            SQLText.Append("ctrDateSelectTime1.BeginTime=" + ctrDateSelectTime1.BeginTime.ToString().Trim() + "|@@?@$|");
            SQLText.Append("ctrDateSelectTime1.EndTime=" + ctrDateSelectTime1.EndTime.ToString().Trim() + "|@@?@$|");

            //资产目录
            SQLText.Append("txtEquipmentDir.Text=" + txtEquipmentDir.Text.Trim() + "|@@?@$|");

            //资产名称
            SQLText.Append("txtEquipmentName.Text=" + txtEquipmentName.Text.Trim() + "|@@?@$|");

            //变更级别
            SQLText.Append("CtrFCDlevel.CatelogID=" + CtrFCDlevel.CatelogID.ToString().Trim() + "|@@?@$|");

            //影响度
            SQLText.Append("CtrFCDEffect.CatelogID=" + CtrFCDEffect.CatelogID.ToString().Trim() + "|@@?@$|");

            //紧急度
            SQLText.Append("CtrFCDInstancy.CatelogID=" + CtrFCDInstancy.CatelogID.ToString().Trim() + "|@@?@$|");

            //变更类别
            SQLText.Append("CtrChangeType.CatelogID=" + CtrChangeType.CatelogID.ToString().Trim() + "|@@?@$|");

            //保存条件名
            SQLText.Append("txtSQLName.Text=" + SQLName.Trim());

            string SQLstr = "";

            if (IsExistQuery(" and Name='Temp1' and FORMID='frm_ChangeQuery' and SQLWhere='Temp1' and LOGINNAME=" + StringTool.SqlQ(Session["UserName"].ToString())))
            {
                if (String.IsNullOrEmpty(this.hidDeptID.Value))
                {
                    SQLstr = "update CST_ISSUE_QUERYSave set SQLText=" + StringTool.SqlQ(SQLText.ToString().Trim()) +
                            " where FORMID='frm_ChangeQuery' and  LOGINNAME=" + StringTool.SqlQ(Session["UserName"].ToString()) + " and Name='Temp1' and  SQLWhere='Temp1'";
                }
                else {
                    SQLstr = "update CST_ISSUE_QUERYSave set SQLText=" + StringTool.SqlQ(SQLText.ToString().Trim())
                        + ",DISPLAYCOLUMN=" + StringTool.SqlQ(this.hidDeptID.Value.ToString().Trim()) +
                            " where FORMID='frm_ChangeQuery' and  LOGINNAME=" + StringTool.SqlQ(Session["UserName"].ToString()) + " and Name='Temp1' and  SQLWhere='Temp1'";
                }
            }
            else
            {
                string strID = EpowerGlobal.EPGlobal.GetNextID("CST_ISSUE_QUERYSaveID").ToString();

                SQLstr = "insert into CST_ISSUE_QUERYSave(ID,Name,SQLWhere,FORMID,LOGINNAME,SQLText,DISPLAYCOLUMN)" +
                        " values(" + strID + ",'Temp1','Temp1','frm_ChangeQuery'," +
                        StringTool.SqlQ(Session["UserName"].ToString()) + "," + StringTool.SqlQ(SQLText.ToString().Trim()) + ","
                            + StringTool.SqlQ(this.hidDeptID.Value.ToString().Trim()) + ")";
            }

            OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationSettings.AppSettings["SQLConnString"]);
            try
            {
                int i = OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, SQLstr);
            }
            finally { ConfigTool.CloseConnection(cn); }
        }

        #endregion

        #region  收藏条件下拉框加载数据

        private void InitDropSQLwSave()
        {
            DataTable dt = ChangeDealDP.getCST_ISSUE_LISTFASTQUERY("frm_ChangeQuery", Session["UserName"].ToString(), string.Empty);
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
                        DropSQLwSave.SelectedValue = dt.Rows[i]["ID"].ToString().Trim();
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
            DataTable dt = ChangeDealDP.getCST_ISSUE_LISTFASTQUERY("frm_ChangeQuery", Session["UserName"].ToString(), string.Empty);
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
                        DropSQLwSave.SelectedValue = dt.Rows[i]["ID"].ToString().Trim();
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
            DataTable dt = ChangeDealDP.getCST_ISSUE_LISTFASTQUERY("frm_ChangeQuery", Session["UserName"].ToString(), SQLName);

            if (dt.Rows.Count > 0)
            {
                string[] SQLTextDetailList = dt.Rows[0]["SQLText"].ToString().Split(new string[] { "|@@?@$|" }, StringSplitOptions.None);

                for (int i = 0; i < SQLTextDetailList.Length; i++)
                {
                    string[] SQLTextDetail = SQLTextDetailList[i].Trim().Split('=');

                    //用户信息
                    if (SQLTextDetail[0].Trim() == "txtCustInfo.Text")
                    {
                        this.txtCustInfo.Text = SQLTextDetail[1].Trim();
                    }

                    //流程状态
                    if (SQLTextDetail[0].Trim() == "cboStatus.SelectedValue")
                    {
                        this.cboStatus.SelectedValue = SQLTextDetail[1].Trim();
                    }

                    //变更状态
                    if (SQLTextDetail[0].Trim() == "CtrFlowChangeState.SelectedValue")
                    {
                        this.CtrDealStatus.CatelogID = Convert.ToInt64(SQLTextDetail[1].Trim());
                    }

                    //标题
                    if (SQLTextDetail[0].Trim() == "txtTitle.Text")
                    {
                        this.txtTitle.Text = SQLTextDetail[1].Trim();
                    }

                    //变更时间
                    if (SQLTextDetail[0].Trim() == "ctrDateSelectTime1.BeginTime")
                    {
                        this.ctrDateSelectTime1.BeginTime = SQLTextDetail[1].Trim();
                    }
                    if (SQLTextDetail[0].Trim() == "ctrDateSelectTime1.EndTime")
                    {
                        this.ctrDateSelectTime1.EndTime = SQLTextDetail[1].Trim();
                    }

                    //资产目录
                    if (SQLTextDetail[0].Trim() == "txtEquipmentDir.Text")
                    {
                        this.txtEquipmentDir.Text = SQLTextDetail[1].Trim();
                    }

                    //资产名称
                    if (SQLTextDetail[0].Trim() == "txtEquipmentName.Text")
                    {
                        this.txtEquipmentName.Text = SQLTextDetail[1].Trim();
                    }

                    //变更级别
                    if (SQLTextDetail[0].Trim() == "CtrFCDlevel.CatelogID")
                    {
                        this.CtrFCDlevel.CatelogID = Convert.ToInt64(SQLTextDetail[1].Trim());
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
                    //变更类别
                    if (SQLTextDetail[0].Trim() == "CtrChangeType.CatelogID")
                    {
                        this.CtrChangeType.CatelogID = Convert.ToInt64(SQLTextDetail[1].Trim());
                    }
                }


                /*
                 * ####
                 * 加载显示字段
                 * ####
                 * **/
                
                // 清除上次选择.
                lsbDeptTo.Items.Clear();

                String strDisplayColumn = dt.Rows[0]["DISPLAYCOLUMN"].ToString();
                if (!String.IsNullOrEmpty(strDisplayColumn))
                {
                    //// Content,Email,
                    String[] _strValArray = strDisplayColumn.Split(new String[] { "," },
                        StringSplitOptions.RemoveEmptyEntries);

                    SeleteTableColumn st = new SeleteTableColumn();
                    dt = SeleteTableColumn.GetTableColumnName(viewName);



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

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Write("<script>window.close();</script>");
        }

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
