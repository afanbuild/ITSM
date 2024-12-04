/*******************************************************************
 *
 * Description:服务级别管理
 * 
 * 
 * Create By  :zhumc
 * Create Date:2008年4月23日
 * *****************************************************************/
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
using Epower.ITSM.Web.Controls;

namespace Epower.ITSM.Web.AppForms
{
    public partial class frmCst_ServiceLevelEdit : BasePage
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
            this.Master.OperatorID = Constant.ServerLevel;
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
            Response.Redirect("frmCst_ServiceLevelEdit.aspx");
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
                Cst_ServiceLevelDP ee = new Cst_ServiceLevelDP();
                ee.DeleteRecorded(long.Parse(this.Master.MainID.Trim()));

                //强制相关缓存失效 
                HttpRuntime.Cache.Insert("CommCacheValidCstServiceLevel", false);

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
                Response.Redirect("frmCst_ServiceLevelMain.aspx");
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
                Cst_ServiceLevelDP ee = new Cst_ServiceLevelDP();
                ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                CtrFTLevelName.Value = ee.LevelName.ToString();
                txtDefinition.Text = ee.Definition.ToString();
                labDefinition.Text = txtDefinition.Text;
                txtBaseLevel.Text = ee.BaseLevel.ToString();
                labBaseLevel.Text = txtBaseLevel.Text;
                txtNotInclude.Text = ee.NotInclude.ToString();
                labNotInclude.Text = txtNotInclude.Text;
                txtAvailability.Text = ee.Availability.ToString();
                labAvailability.Text = txtAvailability.Text;
                txtCharge.Text = ee.Charge.ToString();
                labCharge.Text = txtCharge.Text;
                //txtCondition.Value = ee.Condition.ToString();
                rdbtIsAvail.SelectedValue = ee.IsAvail.ToString();
            }
        }
        #endregion

        #region InitObject
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ee"></param>
        private void InitObject(Cst_ServiceLevelDP ee)
        {
            ///取得设置规则
            DataTable dt = GetDetailItem(false);
            string strXml = GetSchemaXml(dt);

            ee.LevelName = CtrFTLevelName.Value.Trim();
            ee.Definition = txtDefinition.Text.Trim();
            ee.BaseLevel = txtBaseLevel.Text.Trim();
            ee.NotInclude = txtNotInclude.Text.Trim();
            ee.Availability = txtAvailability.Text.Trim();
            ee.Charge = txtCharge.Text.Trim();
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
            if (string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                Cst_ServiceLevelDP ee = new Cst_ServiceLevelDP();
                InitObject(ee);
                ee.Deleted = (int)eRecord_Status.eNormal;
                ee.updateUserID = long.Parse(Session["UserID"].ToString());
                ee.LastUpdate = DateTime.Now;
                ee.InsertRecorded(ee);
                this.Master.MainID = ee.ID.ToString();
            }
            else
            {
                Cst_ServiceLevelDP ee = new Cst_ServiceLevelDP();
                ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                InitObject(ee);
                ee.UpdateRecorded(ee);
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

                SaveSLTime();  //保存数据
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
            Cst_ServiceLevelDP sldp = new Cst_ServiceLevelDP();
            sldp = sldp.GetReCorded(lngID);

            sXml = sldp.Condition;

            if (sXml == null)
                sXml = "";
            DataTable dt = CreateDataTable(sXml);

            dgCondition.DataSource = dt.DefaultView;
            dgCondition.DataBind();

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



        /// <summary>
        /// 创建 datatable结构
        /// </summary>
        /// <returns></returns>
        private DataTable CreateNullTable()
        {
            DataTable dt = new DataTable("ServiceLevelRule");
            dt.Columns.Add("id");
            dt.Columns.Add("Relation");
            dt.Columns.Add("GroupValue");
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
                        //组
                        string sGroupValue = ((CtrFlowNumeric)row.FindControl("CtrFlowGroupValue")).Value.ToString();
                        string sCondItem = ((DropDownList)row.FindControl("cboItems")).SelectedItem.Text;
                        string sCondType = ((DropDownList)row.FindControl("cboItems")).SelectedItem.Value;

                        string sOperate = ((DropDownList)row.FindControl("cboOperate")).SelectedItem.Value;

                        string sExpression = ((TextBox)row.FindControl("txtValue")).Text;
                        string sHidExpression = ((HtmlInputHidden)row.FindControl("hidValue")).Value;
                        string sTag = ((HtmlInputHidden)row.FindControl("hidTag")).Value;

                        sExpression = (sExpression.Trim() == "") ? sHidExpression.Trim() : sExpression.Trim();
                        dr = dt.NewRow();

                        if (isAll == true || sExpression.Length > 0)
                        {
                            dr["id"] = sID.Trim();
                            dr["Relation"] = sRelation;
                            dr["GroupValue"] = sGroupValue;
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

            dr["ID"] = "10,CHAR";
            dr["Text"] = "[单位." + PageDeal.GetLanguageValue("Custom_MastCustName") + "]";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = "11,CATA";
            dr["Text"] = "[单位.类型]";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = "12,CATA";
            dr["Text"] = "[单位.性质]";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = "13,CHAR";
            dr["Text"] = "[单位.地址]";
            dt.Rows.Add(dr);


            dr = dt.NewRow();
            dr["ID"] = "20,CHAR";
            dr["Text"] = "[用户." + PageDeal.GetLanguageValue("Custom_CustName") + "]";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = "21,CHAR";
            dr["Text"] = "[用户." + PageDeal.GetLanguageValue("Custom_CustomCode") + "]";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = "22,CATA";
            dr["Text"] = "[用户." + PageDeal.GetLanguageValue("Custom_CustomerType") + "]";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = "23,CHAR";
            dr["Text"] = "[用户." + PageDeal.GetLanguageValue("Custom_CustDeptName") + "]";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = "24,CHAR";
            dr["Text"] = "[用户." + PageDeal.GetLanguageValue("Custom_Rights") + "]";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = "25,CHAR";
            dr["Text"] = "[用户." + PageDeal.GetLanguageValue("Custom_CustAddress") + "]";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = "26,CATA";
            dr["Text"] = "[用户.所属区域]";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = "30,CHAR";
            dr["Text"] = "[资产." + PageDeal.GetLanguageValue("Equ_DeskName") + "]";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = "31,CHAR";
            dr["Text"] = "[资产." + PageDeal.GetLanguageValue("Equ_Code") + "]";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = "40,CATA";
            dr["Text"] = "[事件." + PageDeal.GetLanguageValue("CST_ServiceType") + "]";
            dt.Rows.Add(dr);

            //dr = dt.NewRow();
            //dr["ID"] = "42,CATA";
            //dr["Text"] = "[事件." + PageDeal.GetLanguageValue("LitServiceKind") + "]";
            //dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = "43,CATA";
            dr["Text"] = "[事件." + PageDeal.GetLanguageValue("CST_EffectName") + "]";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = "44,CATA";
            dr["Text"] = "[事件." + PageDeal.GetLanguageValue("CST_InstancyName") + "]";
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

                dl.SelectedValue = e.Item.Cells[7].Text.Trim();

                TextBox txt = (TextBox)e.Item.FindControl("txtValue");
               // ==zxl
                HtmlInputButton bt = (HtmlInputButton)e.Item.FindControl("cmdPop");
               // Button bt = (Button)e.Item.FindControl("cmdPop");
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
                    default:
                        break;
                }
                ddl.SelectedValue = sOperate;

                if (IsSelect)
                {
                    //只读状态
                    DropDownList dlRel = (DropDownList)e.Item.FindControl("cboRelation");
                    DropDownList dlOp = (DropDownList)e.Item.FindControl("cboOperate");
                    CtrFlowNumeric ctrf = (CtrFlowNumeric)e.Item.FindControl("CtrFlowGroupValue");
                    //=======zxl
                   // Button btncmdPop = (Button)e.Item.FindControl("cmdPop");
                    HtmlInputButton btncmdPop = (HtmlInputButton)e.Item.FindControl("cmdPop");

                    btncmdPop.Visible = false;
                    ctrf.ContralState = eOA_FlowControlState.eReadOnly;
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
        private void OpenSonWindow(DataGridCommandEventArgs e) 
        {
            
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
            dr["CondItem"] = "[单位.简称]";
            dr["CondType"] = "10,CHAR";
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


        #region  SetFormReadOnly
        /// <summary>
        /// 
        /// </summary>
        private void SetFormReadOnly()
        {
            CtrFTLevelName.ContralState = eOA_FlowControlState.eReadOnly;

            rdbtIsAvail.Enabled = false;

            txtDefinition.Visible = false;
            labDefinition.Visible = true;

            txtBaseLevel.Visible = false;
            labBaseLevel.Visible = true;

            txtNotInclude.Visible = false;
            labNotInclude.Visible = true;

            txtAvailability.Visible = false;
            labAvailability.Visible = true;

            txtCharge.Visible = false;
            labCharge.Visible = true;

            //屏蔽GRID上的按钮列    只读状态在绑定时实现
            dgCondition.Columns[6].Visible = false;
            dgSLTime.Columns[8].Visible = false;

            //trItem.Visible = false;
        }
        #endregion


        #region 时限设置区
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        private void LoadTimeData(long lngID)
        {
            Cst_SLGuidDP ee = new Cst_SLGuidDP();
            DataTable dt = ee.GetDataByLevelID(lngID);

            dgSLTime.DataSource = dt.DefaultView;
            dgSLTime.DataBind();
        }
        /// <summary>
        /// 
        /// </summary>
        private void SaveSLTime()
        {
            DataTable dt = new DataTable();
            string sList = "";
            dt = GetTimeDetailItem(ref sList);
            Cst_SLGuidDP ee = new Cst_SLGuidDP();
            ee.SaveDetailItem(dt, long.Parse(this.Master.MainID.Trim()));
        }
        /// <summary>
        /// 创建 datatable结构
        /// </summary>
        /// <returns></returns>
        private DataTable CreateTimeLevelNullTable()
        {
            DataTable dt = new DataTable("ServiceLevelTime");
            dt.Columns.Add("ID");
            dt.Columns.Add("LevelID");
            dt.Columns.Add("GuidID");
            dt.Columns.Add("TimeLimit");
            dt.Columns.Add("TimeUnit");
            dt.Columns.Add("Saved");
            dt.Columns.Add("Target");
            dt.Columns.Add("Remark");
            return dt;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private DataTable GetTimeDetailItem(ref string sList)
        {
            DataTable dt = CreateTimeLevelNullTable();
            DataRow dr;

            int id = 1;

            if (dgSLTime.Items.Count > 0)
            {
                foreach (DataGridItem row in dgSLTime.Controls[0].Controls)
                {
                    if (row.ItemType == ListItemType.Item || row.ItemType == ListItemType.AlternatingItem)
                    {
                        string sID = id.ToString();
                        id++;
                        string sGuidID = ((DropDownList)row.FindControl("ddltGuidID")).SelectedItem.Value;
                        string sTime = ((TextBox)row.FindControl("txtTime")).Text;
                        string sTimeUnit = ((DropDownList)row.FindControl("ddltUnit")).SelectedItem.Value;
                        string sTarget = ((TextBox)row.FindControl("txtTarget")).Text;
                        string sRemark = ((TextBox)row.FindControl("txtRemark")).Text;
                        dr = dt.NewRow();
                        if (sGuidID.Length > 0 && sTime.Length > 0 && sTimeUnit.Length > 0)
                        {
                            dr["ID"] = sID.Trim();

                            dr["LevelID"] = this.Master.MainID.Trim();
                            dr["GuidID"] = sGuidID;
                            sList += sGuidID.Trim() + ",";
                            dr["TimeLimit"] = sTime;
                            dr["TimeUnit"] = sTimeUnit;
                            dr["Saved"] = "1";      //只要回发过了就不能再编辑指标选择项
                            dr["Target"] = sTarget;
                            dr["Remark"] = sRemark;
                            dt.Rows.Add(dr);
                        }
                    }
                }
            }
            if (sList.EndsWith(","))
                sList = sList.Substring(0, sList.Length - 1);

            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgSLTime_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            string sList = "";
            DataTable dt = GetTimeDetailItem(ref sList);
            if (e.CommandName == "Delete")
            {
                dt.Rows.RemoveAt(e.Item.ItemIndex);

                dgSLTime.DataSource = dt.DefaultView;
                dgSLTime.DataBind();

                DataTable dt2 = GetDetailItem(true);
                dgCondition.DataSource = dt2.DefaultView;
                dgCondition.DataBind();

            }
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
                    TextBox txtTarget = (TextBox)e.Item.FindControl("txtTarget");
                    TextBox txtRemark = (TextBox)e.Item.FindControl("txtRemark");
                    DropDownList ddltUnit = (DropDownList)e.Item.FindControl("ddltUnit");
                    ddltGuidID.Enabled = false;
                    txtTime.Enabled = false;
                    ddltUnit.Enabled = false;

                    txtTarget.Enabled = false;
                    txtRemark.Enabled = false;

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdTimeAdd_Click(object sender, EventArgs e)
        {
            DataTable dt = GetTimeDetailItem(ref mGuidList);

            DataTable dtGuid = GetGuidDefintionData();

            if (CheckCanAdd(dtGuid, mGuidList) == true)
            {

                DataRow dr = dt.NewRow();
                //设定默认值
                dr["ID"] = (dt.Rows.Count + 1).ToString();
                dr["LevelID"] = 0;
                dr["GuidID"] = 0;
                dr["TimeLimit"] = 0;
                dr["TimeUnit"] = 0;
                dr["Saved"] = "0";
                dr["Target"] = string.Empty;
                dr["Remark"] = string.Empty;
                dt.Rows.Add(dr);
                dgSLTime.DataSource = dt.DefaultView;
                dgSLTime.DataBind();
            }
            else
            {
                PageTool.MsgBox(this, "相关指标已经全部添加");
            }

            DataTable dt2 = GetDetailItem(true);
            dgCondition.DataSource = dt2.DefaultView;
            dgCondition.DataBind();
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


    }
}
