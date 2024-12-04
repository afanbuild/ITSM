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
using System.Xml;

using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.RecommendRule
{
    public partial class frmCst_RecommendRuleEdit : BasePage
    {

        #region 属性区

        /// <summary>
        /// 是否来源于选择
        /// </summary>
        protected bool IsSelect
        {
            get { if (Request["IsSelect"] != null) return true; else return false; }
        }

        /// <summary>
        /// ServiceLevelID
        /// </summary>
        protected string ServiceLevelID
        {
            get
            {
                string sRet = "0";
                if (this.Request.QueryString["id"] != null)
                {
                    sRet = this.Request.QueryString["id"];
                }
                return sRet;
            }
        }
        #endregion

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.ServiceStaff;
            this.Master.IsCheckRight = true;
            if (this.Request.QueryString["id"] != null)
            {
                this.Master.MainID = this.Request.QueryString["id"].ToString();
            }
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);
            this.Master.Master_Button_Save_Click += new Global_BtnClick(Master_Master_Button_Save_Click);
            this.Master.Master_Button_GoHistory_Click += new Global_BtnClick(Master_Master_Button_GoHistory_Click);
            if (string.IsNullOrEmpty(this.Master.MainID.ToString()))
                this.Master.ShowAddPageButton();
            else
                this.Master.ShowEditPageButton();


            if (IsSelect)   //如果详情状态只读
            {
                this.Master.ShowNewButton(false);
                this.Master.ShowDeleteButton(false);
                this.Master.ShowSaveButton(false);
                SetFormReadOnly();
            }
            if (Master.GetEditRight() == false && this.Request.QueryString["id"] != null)
            {
                this.Master.ShowNewButton(false);
                this.Master.ShowDeleteButton(false);
                this.Master.ShowSaveButton(false);
            }

        }
        #endregion


        private string mGuidList = "";

        #region Master_Master_Button_New_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("frmCst_RecommendRuleEdit.aspx");
        }
        #endregion

        #region Master_Master_Button_Delete_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            if (!String.IsNullOrEmpty(this.Master.MainID.ToString()))
            {
                Cst_RecommendRuleDP ee = new Cst_RecommendRuleDP();
                ee.DeleteRecorded(long.Parse(this.Master.MainID.Trim()));

                //强制相关缓存失效 
                //HttpRuntime.Cache.Insert("CommCacheValidCstServiceLevel", false);

                Master_Master_Button_GoHistory_Click();
            }
        }
        #endregion

        #region Master_Master_Button_GoHistory_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_GoHistory_Click()
        {
            if (IsSelect)
            {
                //来源于服务级别选择关闭窗口
                PageTool.SelfClose(this);
            }
            else
            {
                Response.Redirect("frmCst_RecommendRuleMain.aspx");
            }
        }
        #endregion

        #region Page_Load
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
                LoadData();

                //加载规则
                if (this.Master.MainID.Trim() != string.Empty)
                {
                    LoadData(long.Parse(this.Master.MainID));
                    Table2.Visible = true;
                    Table12.Visible = true;

                    LoadTimeData(long.Parse(this.Master.MainID));
                    Table3.Visible = true;
                    Table13.Visible = true;
                }
                else
                {
                    Table2.Visible = false;
                    Table12.Visible = false;

                    Table3.Visible = false;
                    Table13.Visible = false;
                }
            }
        }
        #endregion

        #region LoadData
        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            if (!string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                Cst_RecommendRuleDP ee = new Cst_RecommendRuleDP();
                ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                txtRuleName.Text = ee.RuleName.ToString();
                labRuleName.Text = txtRuleName.Text;
                txtDesc.Text = ee.Desc.ToString();
                labDesc.Text = txtDesc.Text;
                rdbtIsAvail.SelectedValue = ee.IsAvail.ToString();
            }
        }
        #endregion

        #region InitObject
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ee"></param>
        private void InitObject(Cst_RecommendRuleDP ee)
        {
            ///取得设置规则
            DataTable dt = GetDetailItem(false);
            string strXml = GetSchemaXml(dt);

            ee.RuleName = txtRuleName.Text.Trim();
            ee.Desc = txtDesc.Text.Trim();
            ee.Condition = strXml.Trim();
            ee.IsAvail = Int32.Parse(rdbtIsAvail.SelectedValue.Trim());
        }
        #endregion

        #region Master_Master_Button_Save_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Save_Click()
        {

            #region 相关工程师: 必选设置 - 2013-04-02 @孙绍棕

            DataTable dt = null;

            if (!string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                dt = GetRUDetailItem(true);
                if (dt.Rows.Count <= 0)
                {
                    PageTool.MsgBox(this.Page, "相关工程师必须选择！");
                    this.Master.IsSaveSuccess = false;
                    return;
                }
            }

            #endregion

            if (string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                Cst_RecommendRuleDP ee = new Cst_RecommendRuleDP();
                InitObject(ee);

                ee.Deleted = (int)eRecord_Status.eNormal;
                ee.updateUserID = long.Parse(Session["UserID"].ToString());
                ee.LastUpdate = DateTime.Now;
                ee.InsertRecorded(ee);
                this.Master.MainID = ee.ID.ToString();
            }
            else
            {
                Cst_RecommendRuleDP ee = new Cst_RecommendRuleDP();
                ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                InitObject(ee);

                #region 规则设置: 必须设置 - 2013-04-02 @孙绍棕

                if (String.IsNullOrEmpty(ee.Condition))
                {
                    PageTool.MsgBox(this.Page, "规则必须设置！");
                    this.Master.IsSaveSuccess = false;
                    return;
                }

                #endregion

                ee.UpdateRecorded(ee, dt);
                this.Master.MainID = ee.ID.ToString();
            }

            //强制相关缓存失效 
            HttpRuntime.Cache.Insert("CommCacheValidCstServiceLevel", false);

            //加载规则
            if (this.Master.MainID.Trim() != string.Empty)
            {
                LoadData(long.Parse(this.Master.MainID));
                Table2.Visible = true;
                Table12.Visible = true;
                LoadTimeData(long.Parse(this.Master.MainID));
                Table3.Visible = true;
                Table13.Visible = true;
            }
            else
            {
                Table2.Visible = false;
                Table12.Visible = false;

                Table3.Visible = false;
                Table13.Visible = false;
            }
        }
        #endregion

        #region 规则设置区


        /// <summary>
        /// 判断弹出是否可见
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected string GetPopCataVisible(string type)
        {
            string t = "Hidden";
            if (type.Split(",".ToCharArray())[1] == "CATA")
                t = "Visible";
            return t;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        private void LoadData(long lngID)
        {
            string sXml = "";
            Cst_RecommendRuleDP sldp = new Cst_RecommendRuleDP();
            sldp = sldp.GetReCorded(lngID);

            sXml = sldp.Condition;

            if (sXml == null)
                sXml = "";
            DataTable dt = CreateDataTable(sXml);


            dgCondition.DataSource = dt.DefaultView;
            dgCondition.DataBind();

            //Update_Grid_By_DataTable(dt);

        }

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

                object[] values = new object[7];

                XmlNodeList ns = xmldoc.DocumentElement.SelectNodes("Condition");

                foreach (XmlNode n in ns)
                {

                    values[0] = (object)n.Attributes["ID"].Value;
                    values[1] = (object)n.Attributes["Relation"].Value;
                    values[2] = (object)n.Attributes["CondItem"].Value;
                    values[3] = (object)n.Attributes["CondType"].Value;
                    values[4] = (object)n.Attributes["Operate"].Value;
                    values[5] = (object)n.Attributes["Expression"].Value;
                    values[6] = (object)n.Attributes["Tag"].Value;
                    tab.Rows.Add(values);

                }

            }

            return tab;
        }



        /// <summary>
        /// 创建 datatable结构
        /// </summary>
        /// <returns></returns>
        private DataTable CreateNullTable()
        {
            DataTable dt = new DataTable("ServiceLevelRule");
            dt.Columns.Add("id");
            dt.Columns.Add("Relation");
            dt.Columns.Add("CondItem");
            dt.Columns.Add("CondType");
            dt.Columns.Add("Operate");
            dt.Columns.Add("Expression");
            dt.Columns.Add("Tag");

            return dt;
        }

        /// <summary>
        /// 获取表单grid 的 datatabel
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
                        string sRelation = ((DropDownList)row.FindControl("cboRelation")).SelectedItem.Value;

                        string sCondItem = ((DropDownList)row.FindControl("cboItems")).SelectedItem.Text;
                        string sCondType = ((DropDownList)row.FindControl("cboItems")).SelectedItem.Value;

                        //string sOperate = ((DropDownList)row.FindControl("cboOperate")).SelectedItem.Value;
                        string sOperate = ((HtmlInputHidden)row.FindControl("hidOperate")).Value;

                        string sExpression = ((TextBox)row.FindControl("txtValue")).Text;
                        string sHidExpression = ((HtmlInputHidden)row.FindControl("hidValue")).Value;
                        string sTag = ((HtmlInputHidden)row.FindControl("hidTag")).Value;

                        sExpression = (sExpression.Trim() == "") ? sHidExpression.Trim() : sExpression.Trim();
                        dr = dt.NewRow();

                        if (isAll == true || sExpression.Length > 0)
                        {
                            dr["id"] = sID.Trim();
                            dr["Relation"] = sRelation;
                            dr["CondItem"] = sCondItem;
                            dr["CondType"] = sCondType;
                            dr["Operate"] = sOperate;
                            dr["Expression"] = sExpression;
                            dr["Tag"] = sTag;

                            dt.Rows.Add(dr);
                        }
                    }
                }
            }

            return dt;
        }


        private void BindCondItemsControl(DropDownList ddl)
        {
            DataTable dt = new DataTable("CondItems");
            dt.Columns.Add("ID");
            dt.Columns.Add("Text");

            DataRow dr;
            dr = dt.NewRow();

            dr["ID"] = "10,INTER";
            dr["Text"] = "[单位.服务次数]";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = "11,CHAR";//单位名称
            dr["Text"] = "[单位." + PageDeal.GetLanguageValue("Custom_MastCustName") + "]";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = "12,CATA";//事件类别
            dr["Text"] = "[事件." + PageDeal.GetLanguageValue("CST_ServiceType") + "]";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = "13,CATA";//服务级别
            dr["Text"] = "[事件." + PageDeal.GetLanguageValue("CST_ServiceLevel") + "]";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = "14,CHAR";//资产名称
            dr["Text"] = "[资产." + PageDeal.GetLanguageValue("Equ_DeskName") + "]";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = "15,INTER";
            dr["Text"] = "[用户.服务次数]";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = "16,CHAR";//用户名称
            dr["Text"] = "[用户." + PageDeal.GetLanguageValue("Custom_CustName") + "]";
            dt.Rows.Add(dr);


            ddl.DataTextField = "Text";
            ddl.DataValueField = "ID";
            ddl.DataSource = dt.DefaultView;
            ddl.DataBind();

        }

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

                dl.SelectedValue = e.Item.Cells[6].Text.Trim();

                TextBox txt = (TextBox)e.Item.FindControl("txtValue");
                HtmlInputButton bt = (HtmlInputButton)e.Item.FindControl("cmdPop");
                if (dl.SelectedValue.Split(",".ToCharArray())[1] == "CATA")
                {
                    bt.Style.Value = "visibility:visible";
                    txt.Attributes.Add("disabled", "true");
                }
                string sCondType = DataBinder.Eval(e.Item.DataItem, "CondType").ToString();
                DropDownList ddl = (DropDownList)e.Item.FindControl("cboOperate");
                string sOperate = DataBinder.Eval(e.Item.DataItem, "Operate").ToString();

                switch (sCondType.Split(",".ToCharArray())[1])
                {
                    case "CHAR":
                        ddl.Items.Clear();
                        ddl.Items.Add(new ListItem("等于", "0"));
                        ddl.Items.Add(new ListItem("不等于", "1"));
                        ddl.Items.Add(new ListItem("以..开头", "3"));
                        ddl.Items.Add(new ListItem("包含", "6"));
                        ddl.Items.Add(new ListItem("不包含", "7"));
                        break;
                    case "CATA":
                        ddl.Items.Clear();
                        ddl.Items.Add(new ListItem("等于", "0"));
                        ddl.Items.Add(new ListItem("不等于", "1"));
                        ddl.Items.Add(new ListItem("属于", "6"));
                        ddl.Items.Add(new ListItem("不属于", "7"));
                        break;
                    case "INTER":
                        ddl.Items.Clear();
                        ddl.Items.Add(new ListItem("等于", "0"));
                        ddl.Items.Add(new ListItem("不等于", "1"));
                        ddl.Items.Add(new ListItem("大于", "2"));
                        ddl.Items.Add(new ListItem("大于等于", "3"));
                        ddl.Items.Add(new ListItem("小于", "4"));
                        ddl.Items.Add(new ListItem("小于等于", "5"));
                        break;
                    default:
                        break;
                }
                ddl.SelectedValue = sOperate;
                ((HtmlInputHidden)e.Item.FindControl("hidOperate")).Value = sOperate;
                if (IsSelect)
                {
                    //只读状态


                    DropDownList dlRel = (DropDownList)e.Item.FindControl("cboRelation");
                    DropDownList dlOp = (DropDownList)e.Item.FindControl("cboOperate");

                    dl.Enabled = false;
                    txt.Enabled = false;
                    dlRel.Enabled = false;
                    dlOp.Enabled = false;

                }




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
                dt.Rows.RemoveAt(e.Item.ItemIndex);
                hasDeleted = true;
            }

            if (hasDeleted == true)
            {
                dgCondition.DataSource = dt.DefaultView;
                dgCondition.DataBind();
                //Update_Grid_By_DataTable(dt);
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
            dr["CondItem"] = "[单位.服务次数]";
            dr["CondType"] = "10,INTER";
            dr["Operate"] = 0;
            dr["Expression"] = "";
            dr["Tag"] = "";
            dt.Rows.Add(dr);
            dgCondition.DataSource = dt.DefaultView;
            dgCondition.DataBind();

            //Update_Grid_By_DataTable(dt);

        }

        /// <summary>
        /// 
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


        #region  SetFormReadOnly
        /// <summary>
        /// 
        /// </summary>
        private void SetFormReadOnly()
        {
            txtRuleName.Visible = false;
            RequiredFieldValidator1.Visible = false;
            labRuleName.Visible = true;
            lblMsg.Visible = false;

            rdbtIsAvail.Enabled = false;

            txtDesc.Visible = false;
            labDesc.Visible = true;

            //屏蔽GRID上的按钮列    只读状态在绑定时实现


            dgCondition.Columns[5].Visible = false;
            dgCst_ServiceStaff.Columns[4].Visible = false;

            //trItem.Visible = false;
        }
        #endregion


        #region 规则人员设置区


        /// <summary>
        /// 加载明细表


        /// </summary>
        /// <param name="lngID"></param>
        private void LoadTimeData(long lngID)
        {
            DataTable dt = Cst_RecommendRuleDP.getDetails(lngID);
            ViewState["ItemData"] = dt;
            dgCst_ServiceStaff.DataSource = dt.DefaultView;
            dgCst_ServiceStaff.DataBind();
        }

        /// <summary>
        /// 创建明细表结构


        /// </summary>
        /// <returns></returns>
        private DataTable CreateTimeLevelNullTable()
        {
            DataTable dt = new DataTable("RuDetails");
            dt.Columns.Add("RuleID");
            dt.Columns.Add("StaffID");
            dt.Columns.Add("Name");
            dt.Columns.Add("BlongDeptName");
            ViewState["ItemData"] = dt;
            return dt;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgSLTime_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string sIsSaved = e.Item.Cells[2].Text.Trim();

                DropDownList ddltGuidID = (DropDownList)e.Item.FindControl("ddltGuidID");
                DataTable dt = GetGuidDefintionData();
                DataView dv = dt.DefaultView;
                if (sIsSaved == "1")
                {
                    ddltGuidID.Enabled = false;

                }
                else
                {
                    if (mGuidList.Length > 0)
                    {
                        dv.RowFilter = "guidid not in (" + mGuidList + ")";
                    }
                }



                ddltGuidID.DataTextField = "GuidName";
                ddltGuidID.DataValueField = "GuidID";
                ddltGuidID.DataSource = dv;
                ddltGuidID.DataBind();

                ddltGuidID.SelectedValue = e.Item.Cells[1].Text.Trim();




                if (IsSelect)
                {
                    //只读状态


                    TextBox txtTime = (TextBox)e.Item.FindControl("txtTime");
                    DropDownList ddltUnit = (DropDownList)e.Item.FindControl("ddltUnit");
                    ddltGuidID.Enabled = false;
                    txtTime.Enabled = false;
                    ddltUnit.Enabled = false;

                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private DataTable GetGuidDefintionData()
        {
            DataTable dt = new DataTable();
            if (ViewState["GuidDefinition"] != null)
            {
                dt = (DataTable)ViewState["GuidDefinition"];
            }
            else
            {
                Cst_GuidDefinitionDP ee = new Cst_GuidDefinitionDP();
                dt = ee.GetDataTable(string.Empty, string.Empty);
                ViewState["GuidDefinition"] = dt;
            }
            return dt;
        }


        private bool CheckCanAdd(DataTable dt, string lst)
        {
            if (lst.Length == 0)
                return true;

            DataRow[] rows = dt.Select("guidid not in (" + lst + ")");

            if (rows.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        #endregion

        #region 明细操作


        #region 取得详细资料
        /// <summary>
        ///  GetDetailItem
        /// </summary>
        /// <returns></returns>
        private DataTable GetRUDetailItem()
        {
            return GetRUDetailItem(false);
        }

        /// <summary>
        /// 获取明细资料
        /// </summary>
        /// <param name="isAll"></param>
        /// <returns></returns>
        private DataTable GetRUDetailItem(bool isAll)
        {
            DataTable dt = (DataTable)ViewState["ItemData"];
            //zxl
            dt.Rows.Clear();
            int iCostID = 0;
            DataRow dr;

            foreach (DataGridItem row in dgCst_ServiceStaff.Controls[0].Controls)
            {
                iCostID++;
                if (row.ItemType == ListItemType.Footer)
                {
                    string sStaffID = ((HtmlInputHidden)row.FindControl("hidAddStaffID")).Value;
                    string sName = ((TextBox)row.FindControl("txtAddUserName")).Text;
                    string sBlongDeptName = ((HtmlInputHidden)row.FindControl("hidAddBlongDeptName")).Value;
                    string[] arrStaffID = sStaffID.Split(',');
                    string[] arrName = sName.Split(',');
                    string[] arrBlongDeptName = sBlongDeptName.Split(',');
                    for (int i = 0; i < arrStaffID.Length; i++)
                    {
                        if (arrStaffID[i] != "")
                        {
                            dr = dt.NewRow();
                            if (arrStaffID[i].Length > 0 && arrName[i].Length > 0 && arrBlongDeptName[i].Length > 0)
                            {
                                //zxl
                                if (dt.Columns.Count > 0)
                                {

                                    if (dt.Select("StaffID='" + arrStaffID[i] + "'").Length == 0)
                                    {
                                        dr["RuleID"] = Master.MainID.Trim();
                                        dr["StaffID"] = arrStaffID[i];
                                        dr["Name"] = arrName[i];
                                        dr["BlongDeptName"] = arrBlongDeptName[i];
                                        dt.Rows.Add(dr);
                                    }
                                    else
                                    {
                                        PageTool.MsgBox(this, "不能选择重复的工程师！");
                                    }
                                    // }
                                }
                            }
                            else
                            {
                                if (!isAll)
                                    PageTool.MsgBox(this, "相关人员不能为空！");
                            }
                        }
                    }
                }
                else if (row.ItemType == ListItemType.Item || row.ItemType == ListItemType.AlternatingItem)
                {
                    string sStaffID = ((HtmlInputHidden)row.FindControl("hidStaffID")).Value;
                    string sName = ((TextBox)row.FindControl("txtUserName")).Text;
                    string sBlongDeptName = ((HtmlInputHidden)row.FindControl("hidBlongDeptName")).Value;
                    string[] arrStaffID = sStaffID.Split(',');
                    string[] arrName = sName.Split(',');
                    string[] arrBlongDeptName = sBlongDeptName.Split(',');
                    for (int i = 0; i < arrStaffID.Length; i++)
                    {
                        if (arrStaffID[i] != "")
                        {
                            dr = dt.NewRow();
                            if (arrStaffID[i].Length > 0 && arrName[i].Length > 0 && arrBlongDeptName[i].Length > 0)
                            {
                                if (dt.Columns.Count > 0)
                                {
                                    if (dt.Select("StaffID='" + arrStaffID[i] + "'").Length == 0)
                                    {
                                        dr["RuleID"] = Master.MainID.Trim();
                                        dr["StaffID"] = arrStaffID[i];
                                        dr["Name"] = arrName[i];
                                        dr["BlongDeptName"] = arrBlongDeptName[i];
                                        dt.Rows.Add(dr);
                                    }
                                }

                            }
                            else
                            {
                                if (!isAll)
                                    PageTool.MsgBox(this, "相关人员不能为空！");
                            }
                        }
                    }
                }
            }
            ViewState["ItemData"] = dt;
            return dt;
        }
        #endregion

        #region 设置明细资料为只读 SetFareDetailReadOnly
        /// <summary>
        /// 设置费用明细资料为只读

        /// </summary>
        private void SetFareDetailReadOnly()
        {
            foreach (DataGridItem row in dgCst_ServiceStaff.Items)
            {
                ((TextBox)row.FindControl("txtUserName")).Visible = false;
                ((HtmlInputButton)row.FindControl("cmdUserName")).Visible = false;
                ((Label)row.FindControl("lblUserName")).Visible = true;

                dgCst_ServiceStaff.Columns[dgCst_ServiceStaff.Columns.Count - 1].Visible = false;
            }
            dgCst_ServiceStaff.ShowFooter = false;
        }
        #endregion

        #region 明细新增，删除事件 gvBillItem_ItemCommand
        /// <summary>
        /// 费用明细新增，删除事件

        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgCst_ServiceStaff_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            DataTable dt = new DataTable();
            if (e.CommandName == "Delete")
            {
                dt = GetRUDetailItem(true);
                dt.Rows.RemoveAt(e.Item.ItemIndex);
                dgCst_ServiceStaff.DataSource = dt.DefaultView;
                dgCst_ServiceStaff.DataBind();

                DataTable dt2 = GetDetailItem(true);
                dgCondition.DataSource = dt2.DefaultView;
                dgCondition.DataBind();
            }
            else if (e.CommandName == "Add")
            {
                dt = GetRUDetailItem(false);
                dgCst_ServiceStaff.DataSource = dt.DefaultView;
                dgCst_ServiceStaff.DataBind();

                DataTable dt2 = GetDetailItem(true);
                dgCondition.DataSource = dt2.DefaultView;
                dgCondition.DataBind();
            }
            ViewState["ItemData"] = dt;

        }
        #endregion

        protected void dgCst_ServiceStaff_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
            }
        }
        #endregion

    }
}
