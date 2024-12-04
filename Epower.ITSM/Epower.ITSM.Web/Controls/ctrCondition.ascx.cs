
/*******************************************************************
 * 版权所有：深圳市非凡信息技术有限公司
 * 描述：动态查询用户控件

 * 
 * 
 * 创建人：余向前
 * 创建日期：2013-05-20
 * 
 * 修改日志：
 * 修改时间：
 * 修改描述：
 * 
 * *****************************************************************/
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
using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using EpowerGlobal;
using Epower.DevBase.Organization.SqlDAL;
using System.Xml;
using System.Text;

namespace Epower.ITSM.Web.Controls
{
    public partial class ctrCondition : System.Web.UI.UserControl
    {

        public string sApplicationUrl = Constant.ApplicationPath;     //虚拟路径

        #region 激活查询按钮的OnClick事件
        public event EventHandler mybtnSelectOnClick;

        void btnSelect_OnClick(object sender, EventArgs e)
        {
            if (chkHiddenConditionPanel.Checked)
            {
                trConditionList.Visible = true;
                trConditionButton.Visible = true;
            }

            btnSave_Click(null, null);

            if (mybtnSelectOnClick != null)
                mybtnSelectOnClick(this, new EventArgs()); //激活查询按钮的OnClick事件 

            if (ddlCondition.SelectedIndex == 0)
            {
                literalConditionFriendlyContent.Text = "{ 默认查询全部 }";
            }
            else
                literalConditionFriendlyContent.Text = this.GetFriendlyContent();
        }
        #endregion

        #region 激活下拉框改变事件
        public event EventHandler mySelectedIndexChanged;

        void ddlCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCondition.SelectedIndex == 0)
                literalConditionFriendlyContent.Text = "{ 默认查询全部 }";
            else
                literalConditionFriendlyContent.Text = this.GetFriendlyContent();

            if (mySelectedIndexChanged != null)
                mySelectedIndexChanged(this, new EventArgs()); //激活SelectIndexChanged事件 
        }
        #endregion

        #region 属性

        /// <summary>
        /// 所属表名(注意大小写)
        /// </summary>
        public string TableName
        {
            get
            {
                return ViewState[this.ID + "TableName"] == null ? string.Empty : ViewState[this.ID + "TableName"].ToString();
            }
            set
            {
                ViewState[this.ID + "TableName"] = value;
                hidTableName.Value = value;
            }
        }
        /// <summary>
        /// 得到动态条件字符串 如 ( Name like'%...%' and  Sex=1) or (Name='...')
        /// </summary>
        public string strCondition
        {
            get
            {
                return GetCondition();
            }
        }

        /// <summary>
        /// 高级查询面板是否打开?
        /// </summary>
        public Boolean IsOpen
        {
            get
            {
                return hidIsShowAdvancedSearch.Value == "1";
            }
        }

        /// <summary>
        /// 查询条件的文本显示名
        /// </summary>
        /// <returns></returns>
        public String GetFriendlyContent()
        {
            //{[主题] 包含 "计算机" 并且 [内容] 等于 "计算机"} 并且 {[主题] 包含 "计算机" 并且 [内容] 等于 "计算机"}

            StringBuilder sbText = new StringBuilder();

            long lngConditionId = long.Parse(ddlCondition.SelectedValue);
            String strConditionValue = Br_ConditionSaveDP.GetConditionContent(lngConditionId);

            DataTable dtCondition = CreateDataTable(strConditionValue);

            if (dtCondition.Rows.Count <= 0) return String.Empty;

            sbText.Append("{ ");
            for (int index = 0; index < dtCondition.Rows.Count; index++)
            {
                DataRow dr = dtCondition.Rows[index];

                String strCondItem = dr["CondItem"].ToString().Trim();
                if (String.IsNullOrEmpty(strCondItem)) continue;

                Int32 intGroupValue = Int32.Parse(dr["GroupValue"].ToString());    // 分组名
                String strLogicWay = dr["Relation"].ToString().Equals("0") ? "并且" : "或者";    // 逻辑关系

                Int32 intOperate = Int32.Parse(dr["Operate"].ToString());    // 比较关系
                String strOperateChineseName = TranslateOperate(intOperate);    // 比较关系的可读名

                if (intOperate == 2)    // 2 ==> 以...开头
                {
                    sbText.AppendFormat(" [{0}] 以 \"{1}\" 开头 ", strCondItem, dr["Expression"]);
                }
                else
                {
                    sbText.AppendFormat(" [{0}] {1} \"{2}\" ", strCondItem, strOperateChineseName, dr["Expression"]);
                }

                Int32 intNextIdx = index + 1;
                if (dtCondition.Rows.Count - 1 < intNextIdx)
                {
                    sbText.Append(" } ");
                    break;
                }
                else
                {
                    DataRow drNext = dtCondition.Rows[intNextIdx];
                    Int32 intNextGroupValue = Int32.Parse(drNext["GroupValue"].ToString());    // 下一分组名

                    if (intGroupValue == intNextGroupValue)
                    {
                        sbText.AppendFormat(" " + strLogicWay + " ");
                    }
                    else
                    {
                        sbText.Append(" } " + strLogicWay + " { ");
                    }
                }
            }

            return sbText.ToString();
        }

        /// <summary>
        /// 设置是否显示高级查询面板
        /// </summary>
        public Boolean SetDisplayMode
        {
            set
            {
                if (value)
                {
                    hidIsShowAdvancedSearch.Value = "1";
                }
                else
                {
                    hidIsShowAdvancedSearch.Value = "0";
                }

            }
        }

        #endregion

        /// <summary>
        /// 翻译比较关系值为可读名
        /// </summary>
        /// <param name="intOperate">比较关系值</param>
        /// <returns>比较关系的可读名</returns>
        private String TranslateOperate(Int32 intOperate)
        {
            String strOperateChineseName = String.Empty;
            switch (intOperate)
            {
                case 0:
                    strOperateChineseName = "等于";
                    break;
                case 1:
                    strOperateChineseName = "不等于";
                    break;
                case 2:
                    strOperateChineseName = "以...开头";
                    break;
                case 3:
                    strOperateChineseName = "包含";
                    break;
                case 4:
                    strOperateChineseName = "不包含";
                    break;
                case 5:
                    strOperateChineseName = "属于";
                    break;
                case 6:
                    strOperateChineseName = "不属于";
                    break;
                case 7:
                    strOperateChineseName = "大于";
                    break;
                case 8:
                    strOperateChineseName = "大于等于";
                    break;
                case 9:
                    strOperateChineseName = "小于";
                    break;
                case 10:
                    strOperateChineseName = "小于等于";
                    break;
            }

            return strOperateChineseName;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            //给查询按钮设置Click事件
            if (mybtnSelectOnClick != null)
            {
                btnSelect.Click += new EventHandler(btnSelect_OnClick);
            }
            //设置下拉框的改变事件
            if (mySelectedIndexChanged != null)
            {
                ddlCondition.AutoPostBack = true;
                ddlCondition.SelectedIndexChanged += new EventHandler(ddlCondition_SelectedIndexChanged);
            }

            if (!IsPostBack)
            {
                BindDrop(); //绑定快速查询下拉框

                LoadData(); //初始化绑定数据

                if (ddlCondition.SelectedIndex == 0)
                {
                    literalConditionFriendlyContent.Text = "{ 默认查询全部 }";
                }
            }
        }


        #region 绑定快速查询下拉框
        /// <summary>
        /// 绑定数据到[切换组合]下拉框
        /// </summary>
        private void BindDrop()
        {
            string strWhere = " where UserID = " + HttpContext.Current.Session["UserID"].ToString() + " and TableName=" + StringTool.SqlQ(TableName);
            DataTable dt = Br_ConditionSaveDP.GetNames(strWhere, "");

            ddlCondition.DataSource = dt.DefaultView;
            ddlCondition.DataTextField = "ConditionName";
            ddlCondition.DataValueField = "ID";
            ddlCondition.DataBind();

            ddlCondition.Items.Insert(0, new ListItem("查询全部", "-1"));
        }
        #endregion

        private void LoadData()
        {
            DataTable dt = CreateNullTable();
            dgCondition.DataSource = dt.DefaultView;
            dgCondition.DataBind();
        }

        /// <summary>
        /// 调用页面点击查询后需要调用此方法重新绑定列表，防止数据丢失

        /// </summary>
        public void Bind()
        {
            DataTable dt = GetDetailItem(true);
            dgCondition.DataSource = dt.DefaultView;
            dgCondition.DataBind();

            if (ddlSelect.SelectedItem != null)
            {
                if (ddlSelect.SelectedIndex != 0)
                    txtConditionName.Value = ddlSelect.SelectedItem.Text;
            }
        }

        #region Grid相关事件
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgCondition_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DropDownList dl = (DropDownList)e.Item.FindControl("cboItems");

                BindCondItemsControl(dl);

                dl.SelectedValue = e.Item.Cells[7].Text.Trim();


                TextBox txt = (TextBox)e.Item.FindControl("txtValue");
                HtmlInputButton bt = (HtmlInputButton)e.Item.FindControl("cmdPop");

                string sCondType = DataBinder.Eval(e.Item.DataItem, "CondType").ToString();
                DropDownList ddl = (DropDownList)e.Item.FindControl("cboOperate");
                string sOperate = DataBinder.Eval(e.Item.DataItem, "Operate").ToString();

                switch (sCondType.Split(",".ToCharArray())[1])
                {
                    case "CHAR":
                        ddl.Items.Clear();
                        ddl.Items.Add(new ListItem("等于", "0"));
                        ddl.Items.Add(new ListItem("不等于", "1"));
                        ddl.Items.Add(new ListItem("以..开头", "2"));
                        ddl.Items.Add(new ListItem("包含", "3"));
                        ddl.Items.Add(new ListItem("不包含", "4"));

                        //控制显示
                        bt.Style.Value = "visibility:Hidden";
                        if (!IsPostBack)
                        {
                            txt.Attributes.Add("disabled", "false");
                        }
                        //txt.Attributes.Add("disabled", "false");
                        break;
                    case "CLOB":
                        ddl.Items.Clear();
                        ddl.Items.Add(new ListItem("以..开头", "2"));
                        ddl.Items.Add(new ListItem("包含", "3"));
                        ddl.Items.Add(new ListItem("不包含", "4"));

                        //控制显示
                        bt.Style.Value = "visibility:Hidden";
                        //txt.Attributes.Add("disabled", "false");
                        break;
                    case "CATA":
                        ddl.Items.Clear();
                        ddl.Items.Add(new ListItem("等于", "0"));
                        ddl.Items.Add(new ListItem("不等于", "1"));
                        ddl.Items.Add(new ListItem("属于", "5"));
                        ddl.Items.Add(new ListItem("不属于", "6"));

                        //控制显示
                        bt.Style.Value = "visibility:visible";
                        txt.Attributes.Add("disabled", "true");
                        break;
                    case "USER":
                        ddl.Items.Clear();
                        ddl.Items.Add(new ListItem("等于", "0"));
                        ddl.Items.Add(new ListItem("不等于", "1"));
                        //控制显示
                        bt.Style.Value = "visibility:visible";
                        txt.Attributes.Add("disabled", "true");
                        break;
                    case "DEPT":
                        ddl.Items.Clear();
                        ddl.Items.Add(new ListItem("等于", "0"));
                        ddl.Items.Add(new ListItem("不等于", "1"));
                        ddl.Items.Add(new ListItem("属于", "5"));
                        ddl.Items.Add(new ListItem("不属于", "6"));
                        //控制显示
                        bt.Style.Value = "visibility:visible";
                        txt.Attributes.Add("disabled", "true");
                        break;
                    case "DATE":
                        ddl.Items.Clear();
                        ddl.Items.Add(new ListItem("等于", "0"));
                        ddl.Items.Add(new ListItem("大于", "7"));
                        ddl.Items.Add(new ListItem("大于等于", "8"));
                        ddl.Items.Add(new ListItem("小于", "9"));
                        ddl.Items.Add(new ListItem("小于等于", "10"));


                        //控制显示
                        bt.Style.Value = "visibility:visible";
                        txt.Attributes.Add("disabled", "true");
                        break;
                    default:
                        txt.Attributes.Add("disabled", "true");
                        break;
                }
                ddl.SelectedValue = sOperate;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgCondition_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            DataTable dt = GetDetailItem(true);
            bool hasDeleted = false;
            if (e.CommandName == "Delete")
            {
                if (dt.Rows.Count > 0 && e.Item.ItemIndex < dt.Rows.Count)
                    dt.Rows.RemoveAt(e.Item.ItemIndex);

                hasDeleted = true;

                if (ddlCondition.SelectedIndex <= 0)
                {
                    if (dt.Rows.Count <= 0)
                    {
                        txtConditionName.Value = String.Empty;
                    }
                }
            }

            if (hasDeleted == true)
            {
                dgCondition.DataSource = dt.DefaultView;
                dgCondition.DataBind();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdOK_Click(object sender, EventArgs e)
        {
            DataTable dt = GetDetailItem(false);
            DataRow dr = dt.NewRow();
            //设定默认值

            dr["ID"] = (dt.Rows.Count + 1).ToString();
            dr["Relation"] = 0;
            dr["GroupValue"] = 0;
            dr["CondItem"] = "";
            dr["CondType"] = ",CHAR";
            dr["Operate"] = 0;
            dr["Expression"] = "";
            dr["Tag"] = "";
            dt.Rows.Add(dr);
            dgCondition.DataSource = dt.DefaultView;
            dgCondition.DataBind();

            if (ddlCondition.SelectedIndex == 0)
            {
                txtConditionName.Value = "最近一次的查询记录";
            }
        }

        private void BindCondItemsControl(DropDownList ddl)
        {
            DataTable dt = new DataTable("CondItems");
            dt.Columns.Add("ID");
            dt.Columns.Add("Text");

            DataRow drFirst = dt.NewRow();
            drFirst["ID"] = ",CHAR";
            drFirst["Text"] = "";
            dt.Rows.Add(drFirst);


            #region 获取对应TableName所设置的动态查询字段列表

            string strWhere = " where TableName=" + StringTool.SqlQ(TableName);
            string strOrder = " order by ID ";
            DataTable dtCon = Br_ConditionDP.GetDataTable(strWhere, strOrder);
            if (dtCon != null && dtCon.Rows.Count > 0)
            {
                for (int i = 0; i < dtCon.Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();

                    dr["ID"] = dtCon.Rows[i]["ColumnName"].ToString() + "," + dtCon.Rows[i]["ColType"].ToString();
                    dr["Text"] = dtCon.Rows[i]["ColRemark"].ToString();
                    dt.Rows.Add(dr);
                }
            }
            #endregion

            ddl.DataTextField = "Text";
            ddl.DataValueField = "ID";
            ddl.DataSource = dt.DefaultView;
            ddl.DataBind();

        }

        /// <summary>
        /// 创建 datatable结构
        /// </summary>
        /// <returns></returns>
        private DataTable CreateNullTable()
        {
            DataTable dt = new DataTable("ConditionRule");
            dt.Columns.Add("id");
            dt.Columns.Add("Relation");
            dt.Columns.Add("GroupValue");
            dt.Columns.Add("CondItem");
            dt.Columns.Add("CondType");
            dt.Columns.Add("Operate");
            dt.Columns.Add("Expression");
            dt.Columns.Add("Tag");

            #region lsj 2013.02.27 修改(默认显示一条查询条件)
            if (!this.IsPostBack)
            {
                DataRow dr = dt.NewRow();
                //设定默认值


                dr["ID"] = (dt.Rows.Count + 1).ToString();
                dr["Relation"] = 0;
                dr["GroupValue"] = 0;
                dr["CondItem"] = "";
                dr["CondType"] = ",CHAR";
                dr["Operate"] = 0;
                dr["Expression"] = "";
                dr["Tag"] = "";
                dt.Rows.Add(dr);
            }
            #endregion

            return dt;
        }
        #endregion

        #region 获取表单grid 的 datatable
        /// <summary>
        /// 获取表单grid 的 datatable
        /// </summary>
        /// <param name="isAll"></param>
        /// <returns></returns>
        private DataTable GetDetailItem(bool isAll)
        {
            DataTable dt = CreateNullTable();
            DataRow dr;

            int id = 1;

            if (dgCondition.Items.Count > 0)
            {
                foreach (DataGridItem row in dgCondition.Controls[0].Controls)
                {
                    if (row.ItemType == ListItemType.Item || row.ItemType == ListItemType.AlternatingItem)
                    {
                        string sID = id.ToString();
                        id++;
                        //并且 or 或者

                        string sRelation = ((DropDownList)row.FindControl("cboRelation")).SelectedItem.Value;
                        //组

                        string sGroupValue = ((CtrFlowNumeric)row.FindControl("CtrFlowGroupValue")).Value.ToString();
                        //字段描述
                        string sCondItem = ((DropDownList)row.FindControl("cboItems")).SelectedItem.Text;

                        sCondItem = sCondItem.Trim();
                        if (String.IsNullOrEmpty(sCondItem)) continue;

                        //字段及字段类型

                        string sCondType = ((DropDownList)row.FindControl("cboItems")).SelectedItem.Value;
                        //比较类型
                        string sOperate = ((DropDownList)row.FindControl("cboOperate")).SelectedItem.Value;
                        //条件值

                        string sExpression = ((TextBox)row.FindControl("txtValue")).Text;
                        string sHidExpression = ((HtmlInputHidden)row.FindControl("hidValue")).Value;

                        string sTag = ((HtmlInputHidden)row.FindControl("hidTag")).Value;

                        sExpression = (sExpression.Trim() == "") ? sHidExpression.Trim() : sExpression.Trim();

                        string sValue = "";

                        switch (sCondType.Split(",".ToCharArray())[1])
                        {
                            case "CHAR":
                                sValue = sExpression;
                                break;
                            case "CLOB":
                                sValue = sExpression;
                                break;
                            case "CATA":
                                sValue = sTag; //取得所选分类ID的值
                                break;
                            case "USER":
                                sValue = sTag;
                                break;
                            case "DEPT":
                                sValue = sTag;
                                break;
                            case "DATE":
                                sValue = sTag;
                                break;
                            default:
                                break;
                        }

                        dr = dt.NewRow();

                        if (isAll == true || sValue.Length > 0)
                        {
                            dr["id"] = sID.Trim();
                            dr["Relation"] = sRelation;
                            dr["GroupValue"] = sGroupValue;
                            dr["CondItem"] = sCondItem;
                            dr["CondType"] = sCondType;
                            dr["Operate"] = sOperate;
                            dr["Expression"] = sExpression;
                            dr["Tag"] = sValue;

                            dt.Rows.Add(dr);
                        }
                    }
                }
            }

            return dt;
        }
        #endregion

        #region 得到动态条件字符串 如 ( and 1=1 )
        /// <summary>
        /// 得到动态条件字符串 如 ( Name like'%...%' and  Sex=1) or (Name='...')
        /// </summary>
        /// <returns></returns>
        private string GetCondition()
        {
            string strWhere = "";

            DataTable dt = GetDetailItem(false);//得到当前设置的条件列表

            DataRow[] drarry = dt.Select("", " GroupValue asc"); //按分组重新排序得到的数据

            int GroupValue = 0; //记录当前分组ID
            int j = 0;  //记录是否是当前分组下的第一条记录
            int beforeRelation = -1;//前一行逻辑关系

            #region 循环dt 拼接字符串

            if (drarry != null && drarry.Length > 0)
            {
                for (int i = 0; i < drarry.Length; i++)
                {
                    //并且 or 或者
                    string sRelation = drarry[i]["Relation"].ToString();
                    //组
                    string sGroupValue = drarry[i]["GroupValue"].ToString();
                    //字段描述
                    string sCondItem = drarry[i]["CondItem"].ToString();
                    //字段及字段类型
                    string sCondType = drarry[i]["CondType"].ToString();
                    //比较类型
                    string sOperate = drarry[i]["Operate"].ToString();
                    //条件值                    
                    string sValue = drarry[i]["Tag"].ToString();

                    string[] arr = sCondType.Split(',');

                    //初始化当前分组ID
                    if (i == 0)
                        GroupValue = int.Parse(sGroupValue == "" ? "0" : sGroupValue);


                    #region 根据分组合并条件

                    //如果字段和比较值不为空
                    if (arr[0] != string.Empty && sValue != string.Empty)
                    {
                        if (i == 0) strWhere += " ( ";

                        //判断是否在同一个组内
                        if (GroupValue == int.Parse(sGroupValue == "" ? "0" : sGroupValue))
                        {
                            #region 在同一个组的情况 廖世进 2013-03-22 修改
                            if (beforeRelation >= 0)
                            {
                                //添加和上一个条件的逻辑关系
                                if ((e_fm_RELATION_TYPE)beforeRelation == e_fm_RELATION_TYPE.fmConditionAnd)
                                    strWhere += " and ";
                                else
                                    strWhere += " or ";
                            }
                            #endregion

                            #region
                            ////判断是否为此分组下的第一条记录
                            //if (j == 0)
                            //{
                            //    if ((e_fm_RELATION_TYPE)(int.Parse(sRelation)) == e_fm_RELATION_TYPE.fmConditionAnd)
                            //        strWhere += " and (";
                            //    else
                            //        strWhere += " or (";
                            //}
                            //else
                            //{
                            //    if ((e_fm_RELATION_TYPE)(int.Parse(sRelation)) == e_fm_RELATION_TYPE.fmConditionAnd)
                            //        strWhere += " and ";
                            //    else
                            //        strWhere += " or ";
                            //}
                            //j++;
                            #endregion
                        }
                        else
                        {
                            #region 不在同一个组的情况 廖世进 2013-03-22 修改
                            GroupValue = int.Parse(sGroupValue == "" ? "0" : sGroupValue);
                            strWhere += " ) ";

                            //添加和上一个条件的逻辑关系
                            if ((e_fm_RELATION_TYPE)beforeRelation == e_fm_RELATION_TYPE.fmConditionAnd)
                                strWhere += " and ";
                            else
                                strWhere += " or ";

                            strWhere += " ( ";
                            #endregion

                            #region
                            //j = 0;
                            //strWhere += " ) ";

                            //if ((e_fm_RELATION_TYPE)(int.Parse(sRelation)) == e_fm_RELATION_TYPE.fmConditionAnd)
                            //    strWhere += " and (";
                            //else
                            //    strWhere += " or (";

                            //j++;
                            #endregion
                        }
                        //得到条件字符串
                        strWhere += GetsOperate(int.Parse(sOperate), arr[1], arr[0], sValue);
                        beforeRelation = int.Parse(sRelation);
                    }
                    #endregion

                }
            }
            #endregion

            //最后需要补上结尾的括号
            if (strWhere.Trim() != "")
                strWhere += " )";

            return strWhere;

        }
        #endregion

        #region 根据比较符，返回对应的条件字符
        /// <summary>
        /// 根据比较符，返回对应的条件字符
        /// </summary>
        /// <param name="sOperate"></param>
        /// <param name="CondType"></param>
        /// <param name="Column"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetsOperate(int Operate, string CondType, string Column, string value)
        {
            string str = "";
            string FullID = "";

            switch (Operate)
            {
                case 0:
                    switch (CondType)
                    {
                        case "CHAR": //等于
                            str = Column + " = " + StringTool.SqlQ(value);
                            break;
                        case "CATA"://等于
                            str = Column + " = " + value;
                            break;
                        case "USER":
                            str = Column + " = " + value;
                            break;
                        case "DEPT"://等于
                            str = Column + " = " + value;
                            break;
                        case "DATE"://等于
                            str = " nvl(" + Column + ",sysdate) >= to_date(" + StringTool.SqlQ(value + " 00:00:00") + ",'yyyy-MM-dd HH24:mi:ss') ";
                            str = str + " and nvl(" + Column + ",sysdate) <= to_date(" + StringTool.SqlQ(value + " 23:59:59") + ",'yyyy-MM-dd HH24:mi:ss')";
                            break;
                    }
                    break;
                case 1:
                    switch (CondType)
                    {
                        case "CHAR"://不等于

                            str = " (" + Column + " != " + StringTool.SqlQ(value) + " or " + Column + " is null) ";
                            break;
                        case "CATA"://不等于

                            str = " (" + Column + " != " + value + " or " + Column + " is null) ";
                            break;
                        case "USER"://不等于

                            str = " (" + Column + " != " + value + " or " + Column + " is null) ";
                            break;
                        case "DEPT"://不等于

                            str = " (" + Column + " != " + value + " or " + Column + " is null) ";
                            break;
                    }
                    break;
                case 2:
                    switch (CondType)
                    {
                        case "CHAR"://以...开头

                            str = Column + " like " + StringTool.SqlQ(value + "%");
                            break;
                        case "CLOB"://以...开头

                            str = Column + " like " + StringTool.SqlQ(value + "%");
                            break;
                    }
                    break;
                case 3:
                    switch (CondType)
                    {
                        case "CHAR"://包含
                            str = Column + " like " + StringTool.SqlQ("%" + value + "%");
                            break;
                        case "CLOB"://包含
                            str = Column + " like " + StringTool.SqlQ("%" + value + "%");
                            break;
                    }
                    break;
                case 4:
                    switch (CondType)
                    {
                        case "CHAR"://不包含

                            str = Column + " not like " + StringTool.SqlQ("%" + value + "%");
                            break;
                        case "CLOB"://不包含

                            str = Column + " not like " + StringTool.SqlQ("%" + value + "%");
                            break;
                    }
                    break;
                case 5:
                    switch (CondType)
                    {
                        case "CATA"://属于
                            FullID = CatalogDP.GetCatalogFullID(long.Parse(value));
                            str = "nvl(" + Column + ",0) in ( select CatalogID from es_catalog where FullID like " + StringTool.SqlQ(FullID + "%") + ")";
                            break;
                        case "DEPT"://属于
                            FullID = DeptDP.GetDeptFullID(long.Parse(value));
                            str = "nvl(" + Column + ",0) in ( select deptid from ts_dept where FullID like " + StringTool.SqlQ(FullID + "%") + ")";
                            break;

                    }
                    break;
                case 6:
                    switch (CondType)
                    {
                        case "CATA"://不属于

                            FullID = CatalogDP.GetCatalogFullID(long.Parse(value));
                            str = "nvl(" + Column + ",0) not in ( select CatalogID from es_catalog where FullID like " + StringTool.SqlQ(FullID + "%") + ")";
                            break;
                        case "DEPT"://不属于

                            FullID = DeptDP.GetDeptFullID(long.Parse(value));
                            str = "nvl(" + Column + ",0) not in ( select deptid from ts_dept where FullID like " + StringTool.SqlQ(FullID + "%") + ")";
                            break;
                    }
                    break;
                case 7:
                    switch (CondType)
                    {
                        case "DATE"://大于
                            str = " nvl(" + Column + ",sysdate) > to_date(" + StringTool.SqlQ(value + " 00:00:00") + ",'yyyy-MM-dd HH24:mi:ss')";
                            break;
                    }
                    break;
                case 8:
                    switch (CondType)
                    {
                        case "DATE"://大于等于
                            str = " nvl(" + Column + ",sysdate) >= to_date(" + StringTool.SqlQ(value + " 00:00:00") + ",'yyyy-MM-dd HH24:mi:ss') ";
                            break;
                    }
                    break;
                case 9:
                    switch (CondType)
                    {
                        case "DATE"://小于
                            str = "nvl(" + Column + ",sysdate) < to_date(" + StringTool.SqlQ(value + " 23:59:59") + ",'yyyy-MM-dd HH24:mi:ss')";
                            break;
                    }
                    break;
                case 10:
                    switch (CondType)
                    {
                        case "DATE"://小于等于
                            str = "nvl(" + Column + ",sysdate) <= to_date(" + StringTool.SqlQ(value + " 23:59:59") + ",'yyyy-MM-dd HH24:mi:ss')";
                            break;
                    }
                    break;
                default:
                    break;
            }

            return str;
        }
        #endregion

        #region 将dt转成XML字符串
        /// <summary>
        /// 将dt转成XML字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string GetSchemaXml(DataTable dt)
        {
            if (dt.Rows.Count == 0)
                return "";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(@"<Conditions></Conditions>");
            int i = 0;
            foreach (DataRow row in dt.Rows)
            {

                XmlElement xmlEle = xmlDoc.CreateElement("Condition");
                xmlEle.SetAttribute("ID", row["ID"].ToString().Trim());
                xmlEle.SetAttribute("Relation", row["Relation"].ToString().Trim());
                xmlEle.SetAttribute("GroupValue", row["GroupValue"].ToString().Trim());
                xmlEle.SetAttribute("CondItem", row["CondItem"].ToString().Trim());
                xmlEle.SetAttribute("CondType", row["CondType"].ToString().Trim());
                xmlEle.SetAttribute("Operate", row["Operate"].ToString().Trim());
                xmlEle.SetAttribute("Expression", row["Expression"].ToString().Trim());
                xmlEle.SetAttribute("Tag", row["Tag"].ToString().Trim());
                xmlDoc.DocumentElement.AppendChild(xmlEle);
            }
            return xmlDoc.InnerXml;

        }
        #endregion

        #region 将xml字符串转成dt
        /// <summary>
        /// 创建DATATABLE
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private DataTable CreateDataTable(string s)
        {
            DataTable tab = CreateNullTable();

            if (s != "")
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(s);

                object[] values = new object[8];

                XmlNodeList ns = xmldoc.DocumentElement.SelectNodes("Condition");

                foreach (XmlNode n in ns)
                {
                    values[0] = (object)n.Attributes["ID"] == null ? "" : n.Attributes["ID"].Value;
                    values[1] = (object)n.Attributes["Relation"] == null ? "" : n.Attributes["Relation"].Value;
                    values[2] = (object)n.Attributes["GroupValue"] == null ? "" : n.Attributes["GroupValue"].Value;
                    values[3] = (object)n.Attributes["CondItem"] == null ? "" : n.Attributes["CondItem"].Value;
                    values[4] = (object)n.Attributes["CondType"] == null ? "" : n.Attributes["CondType"].Value;
                    values[5] = (object)n.Attributes["Operate"] == null ? "" : n.Attributes["Operate"].Value;
                    values[6] = (object)n.Attributes["Expression"] == null ? "" : n.Attributes["Expression"].Value;
                    values[7] = (object)n.Attributes["Tag"] == null ? "" : n.Attributes["Tag"].Value;
                    tab.Rows.Add(values);

                }
            }

            return tab;
        }
        #endregion

        #region 改变下拉框的值时触发
        /// <summary>
        /// 改变下拉框的值时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ddlSelectChanged()
        {
            string ConditionName = ddlCondition.SelectedItem.Text; //条件名

            long lngConditionId = long.Parse(ddlCondition.SelectedItem.Value);
            string Condition = Br_ConditionSaveDP.GetConditionContent(lngConditionId);    // 条件XML串


            //判断条件XML串是否为空

            if (string.IsNullOrEmpty(Condition))
            {
                //btnDelete.Visible = false;
                if (ddlCondition.SelectedIndex > 0)
                    txtConditionName.Value = ConditionName;
                else
                    txtConditionName.Value = String.Empty;
                LoadData();
                return;
            }

            //btnDelete.Visible = true;

            txtConditionName.Value = ConditionName;

            //根据得到的条件XML串 重新绑定DataGrid
            DataTable dt = CreateDataTable(Condition);
            dgCondition.DataSource = dt.DefaultView;
            dgCondition.DataBind();

        }
        #endregion

        #region 重置查询条件
        /// <summary>
        /// 重置查询条件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReset_Click(object sender, EventArgs e)
        {
            //btnDelete.Visible = false;

            LoadData();
            //txtConditionName.Value = "";
            if (ddlCondition.SelectedIndex == 0)
                txtConditionName.Value = String.Empty;
            else { literalConditionFriendlyContent.Text = String.Empty; }
            //ddlCondition.SelectedIndex = 0;
        }
        #endregion

        #region 保存查询条件
        /// <summary>
        /// 保存查询条件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {

            long UserID = long.Parse(HttpContext.Current.Session["UserID"].ToString());

            //获取条件名称
            string ConditionName = txtConditionName.Value.Trim();
            //如果条件名称为空，直接返回

            if (string.IsNullOrEmpty(ConditionName))
            {
                //重新绑定防止text控件值丢失

                Bind();

                if (ddlCondition.SelectedIndex > 0)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "",
                    "<script>alert('查询条件名称不能为空');</script>");
                }

                return;
            }

            //获取条件XML串

            DataTable dt = GetDetailItem(false);

            //if (dt.Rows.Count <= 0)
            //{
            //    //重新绑定防止text控件值丢失

            //    Bind();
            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "",
            //        "<script>alert('内容项不能为空');</script>");
            //    return;
            //}


            string Condition = GetSchemaXml(dt);

            Br_ConditionSaveDP ee = new Br_ConditionSaveDP();
            //判断是否已经存在此条件名称

            ee = ee.GetReCorded(UserID, ConditionName, TableName);
            ee.UserID = UserID;
            ee.ConditionName = ConditionName;
            ee.TableName = TableName;
            ee.Condition = Condition;

            if (ee != null && ee.ID > 0)
            {
                ee.UpdateRecorded(ee);
            }
            else
            {
                ee.InsertRecorded(ee);
            }

            //重新绑定下拉框的值

            BindDrop();
            //设置下拉框选取的值

            ddlCondition.SelectedIndex = ddlCondition.Items.IndexOf(ddlCondition.Items.FindByText(ConditionName));
            //btnDelete.Visible = true;
            //重新绑定防止text控件值丢失

            Bind();

        }
        #endregion

        #region 删除保存的动态条件

        /// <summary>
        /// 删除保存的动态条件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (ddlCondition.SelectedIndex == 0)
            {
                PageTool.MsgBox(this.Page, "该查询不能删除");
                return;
            }

            //删除保存的动态条件

            long lngConditionId = long.Parse(ddlCondition.SelectedItem.Value);
            //long UserID = long.Parse(HttpContext.Current.Session["UserID"].ToString());
            Br_ConditionSaveDP ee = new Br_ConditionSaveDP();
            //ee.DeleteRecorded(UserID, ConditionName, TableName);
            ee.DeleteRecorded(lngConditionId);

            //重置数据
            LoadData();
            BindDrop();
            txtConditionName.Value = "";
            ddlCondition.SelectedIndex = 0;

            ddlCondition_SelectedIndexChanged(null, null);
        }
        #endregion




        /// <summary>
        /// 该方法解决在页面上动态添加数据到下拉框后提交出错的问题
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            /*
             * 该方法解决在页面上动态添加数据到下拉框后提交出错的问题
             * 
             * 添加人: 孙绍棕
             */
            //String[] arrVal = new String[] { "等于", "不等于", "以..开头", "包含", "不包含", "属于", "不属于", "大于", "大于等于", "小于", "小于等于" };
            String[] arrVal = new String[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };

            foreach (DataGridItem item in this.dgCondition.Items)
            {
                DropDownList ddl = item.FindControl("cboOperate") as DropDownList;

                if (ddl != null)
                {
                    foreach (String val in arrVal)
                    {
                        Page.ClientScript.RegisterForEventValidation(ddl.UniqueID, val);
                    }
                }
            }

            base.Render(writer);
        }
    }
}