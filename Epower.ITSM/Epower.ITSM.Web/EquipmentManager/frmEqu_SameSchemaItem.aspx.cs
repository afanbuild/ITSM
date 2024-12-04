
/****************************************************************************
 * 
 * description:查看某类配置相同的设备情况
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-09-19
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

using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;
using System.Xml;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frmEqu_SameSchemaItem:BasePage
    {
        string strItemID = "";
        string strItemValue = "";
        string strItemType = "0";

        #region 属性
        /// <summary>
        /// 是否来源于配置项管理
        /// </summary>
        protected bool isQuery
        {
            get { if (Request["isQuery"] != null) return true; else return false; }
        }

        #endregion 

      
        /// <summary>
        /// 返回控件值
        /// </summary>
        /// <param name="strvalue"></param>
        /// <returns></returns>
        protected bool GetDefaulCheckValue(string strvalue)
        {
            if (strvalue == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 下拉 用户 部门 3种类型时的取值
        /// </summary>
        /// <param name="strvalue"></param>
        /// <returns></returns>
        protected string GetDefaultValue(string strType , string strvalue)
        {
            if (strType == "3")
                return CatalogDP.GetCatalogName(long.Parse(strvalue == "" ? "0" : strvalue));
            else if (strType == "4")
                return DeptDP.GetDeptName(long.Parse(strvalue == "" ? "0" : strvalue));
            else
                return UserDP.GetUserName(long.Parse(strvalue == "" ? "0" : strvalue));            
        }

        /// <summary>
        /// 返回服务器状态
        /// </summary>
        /// <param name="strType"></param>
        /// <returns></returns>
        protected string GetControlDisplayStatus(string strType)
        {
            if (strItemType == strType)
                return "";
            else
                return "display:none";
        }

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
           this.Master.Master_Button_GoHistory_Click += new Global_BtnClick(Master_Master_Button_GoHistory_Click);
           // this.Master.ShowBackUrlButton(true);
            if (isQuery)
            {
                this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
                this.Master.Master_Button_GoHistory_Click += new Global_BtnClick(Master_Master_Button_GoHistory_Click);
                this.Master.ShowBackUrlButton(true);
                this.Master.ShowQueryButton(true);
            }
            else
            {
                this.Master.ShowBackUrlButton(true);
                btnBatchUpdate.Visible = false;
                dgEqu_Desk.Columns[0].Visible = false;
                dgEqu_Desk.Columns[8].Visible = true;
                dgEqu_Desk.Columns[9].Visible = false;
                dgEqu_Desk.Columns[10].Visible = false;
                dgEqu_Desk.Columns[11].Visible = false;
                //dgEqu_Desk.Columns[12].Visible = false;                

            }
        }

        #endregion


        #region Master_Master_Button_Query_Click
        /// <summary>
        /// Master_Master_Button_Query_Click
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            if (strItemType == "3")
                strItemValue = ctrFlowCataDropDefault.CatelogID.ToString();
            else if (strItemType == "4")
                strItemValue = DeptPicker1.DeptID.ToString();
            else if (strItemType == "5")
                strItemValue = UserPicker1.UserID.ToString();
            else if (strItemType == "1")
                strItemValue = (chkItemValue.Checked == true ? "1" : "0");
            else
                strItemValue = txtItemValue.Text.Trim();

            DataTable dt;

            Equ_SchemaItemsDP es = new Equ_SchemaItemsDP();
            es = es.GetReCorded(strItemID);

            dt = LoadData(strItemID, strItemValue);

            dgEqu_Desk.Columns[8].HeaderText = es.CHName;
            dgEqu_Desk.Columns[9].HeaderText = es.CHName;

            dgEqu_Desk.DataSource = dt.DefaultView;
            dgEqu_Desk.DataBind();

            txtItemValue.Attributes.Add("onchange", "BatchUpdateCanDo();");
            chkItemValue.Attributes.Add("onclick", "BatchUpdateCanDo();");            
        }
        #endregion

        #region Master_Master_Button_GoHistory_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_GoHistory_Click()
        {

            if (isQuery)
            {
                Response.Redirect("frmEqu_SchemaItemsMain.aspx");
            }
            else
            {
                Response.Write("<script>top.close();</script>");
            }
          //  Response.Write("<script>window.close();<script>");
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
			ControlPage1.On_PostBack+=new EventHandler(ControlPage1_On_PostBack);
			ControlPage1.DataGridToControl=dgEqu_Desk;

            //检查批量更新的权限
            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.SchemaItemsMain];

            if (reTrace.CanRead == true)
            {
                //有修改权限
               
                    txtItemValue.Attributes.Add("onchange", "BatchUpdateCanDo();");
                    chkItemValue.Attributes.Add("onclick", "BatchUpdateCanDo();");
                
                btnBatchUpdate.Enabled = false;  //只有更改过取值才可以
            }
            else
            {
                btnBatchUpdate.Visible = false;
            }


            if (!IsPostBack)
            {

                btnImplyEquProp.Attributes.Add("onclick", "return SellChanceFirm();"); //给批量关联按钮增加JS 单击事件

                DataTable dt;

                strItemID = Request["ItemFieldID"].ToString().Trim();
                HidItemFieldID.Value = strItemID;
                Equ_SchemaItemsDP es = new Equ_SchemaItemsDP();
                es = es.GetReCorded(strItemID);

                txtFieldID.Text = es.CHName;


                strItemValue = Server.HtmlDecode(Request["ItemFieldValue"].ToString()).Trim();
                HidItemFieldValue.Value = strItemValue;

                #region 根据扩展项类型判断
                txtItemValue.Text = strItemValue;
                if (es.itemType == 3)
                {
                    ctrFlowCataDropDefault.RootID = long.Parse(es.CatalogID.ToString() == "" ? "1" : es.CatalogID.ToString());
                    ctrFlowCataDropDefault.CatelogID = long.Parse(strItemValue == "" ? "0" : strItemValue);
                }
                else if (es.itemType == 4)
                {
                    DeptPicker1.DeptID = long.Parse(strItemValue == "" ? "0" : strItemValue);
                    DeptPicker1.DeptName = DeptDP.GetDeptName(long.Parse(strItemValue == "" ? "0" : strItemValue));
                }
                else if (es.itemType == 5)
                {
                    UserPicker1.UserID = long.Parse(strItemValue == "" ? "0" : strItemValue);
                    UserPicker1.UserName = UserDP.GetUserName(long.Parse(strItemValue == "" ? "0" : strItemValue));
                }

                #endregion

                chkItemValue.Text = es.CHName;
                if (strItemValue == "1")
                    chkItemValue.Checked = true;
                else
                    chkItemValue.Checked = false;

                strItemType = es.itemType.ToString();    //暂时未从数据库取

                if (strItemType == "3")
                {
                    ctrFlowCataDropDefault.Visible = true;
                    DeptPicker1.Visible = false;
                    UserPicker1.Visible = false;
                    txtItemValue.Visible = false;
                    chkItemValue.Visible = false;
                }
                else if (strItemType == "4")
                {
                    ctrFlowCataDropDefault.Visible = false;
                    DeptPicker1.Visible = true;
                    UserPicker1.Visible = false;
                    txtItemValue.Visible = false;
                    chkItemValue.Visible = false;
                }
                else if (strItemType == "5")
                {
                    ctrFlowCataDropDefault.Visible = false;
                    DeptPicker1.Visible = false;
                    UserPicker1.Visible = true;
                    txtItemValue.Visible = false;
                    chkItemValue.Visible = false;
                }
                else if (strItemType == "1")
                {
                    ctrFlowCataDropDefault.Visible = false;
                    DeptPicker1.Visible = false;
                    UserPicker1.Visible = false;
                    txtItemValue.Visible = false;
                    chkItemValue.Visible = true;
                }
                else
                {
                    ctrFlowCataDropDefault.Visible = false;
                    DeptPicker1.Visible = false;
                    UserPicker1.Visible = false;
                    txtItemValue.Visible = true;
                    chkItemValue.Visible = false;
                }



                ViewState["SameSchemaItemID"] = strItemID;
                ViewState["SameSchemaItemType"] = strItemType;


                dt = LoadData(strItemID, strItemValue);

                dgEqu_Desk.Columns[8].HeaderText = es.CHName;
                dgEqu_Desk.Columns[9].HeaderText = es.CHName;

                dgEqu_Desk.DataSource = dt.DefaultView;
                dgEqu_Desk.DataBind();


                //初始化下拉列表内容
                InitDropdownList();
            }
            else
            {
                strItemID = ViewState["SameSchemaItemID"].ToString();
                HidItemFieldID.Value = strItemID;

                strItemType = ViewState["SameSchemaItemType"].ToString();

                if (strItemType == "3")
                    strItemValue = ctrFlowCataDropDefault.CatelogID.ToString();
                if (strItemType == "4")
                    strItemValue = DeptPicker1.DeptID.ToString();
                else if (strItemType == "5")
                    strItemValue = UserPicker1.UserID.ToString();
                else if (strItemType == "1")
                    strItemValue = (chkItemValue.Checked == true ? "1" : "0");
                else
                    strItemValue = txtItemValue.Text.Trim();
            }

            Session["SameSchemaDetailReturnUrl"] = "frmEqu_SameSchemaItem.aspx?isQuery=1&ItemFieldID=" + strItemID + "&ItemFieldValue=" + strItemValue;

		}
		#endregion         

        #region LoadData
        /// <summary>
        /// 
        /// </summary>
        private DataTable LoadData(string sID,string sValue)
        {
            DataTable dt;
            Equ_DeskDP ee = new Equ_DeskDP();
            //改为无论什么时候精确查询,因为 GRID上需要显示值,而值是直接来自查询值的
            //dt = ee.GetSameSchemaItems(sID, sValue, isQuery==false?true:false); ;
            dt = ee.GetSameSchemasEqus(sID, sValue); ;
            Session["Equ_SameSchemaItem"] = dt;
            return dt;
        }
		#endregion 
		
		#region  Bind
        /// <summary>
        /// 
        /// </summary>
        private void Bind()
        {
            DataTable dt;
            if (Session["Equ_SameSchemaItem"] == null)
            {
                dt = LoadData(strItemID, strItemValue);
            }
            else
            {
                dt = (DataTable)Session["Equ_SameSchemaItem"];
            }
            dgEqu_Desk.DataSource = dt.DefaultView;
            dgEqu_Desk.DataBind();
        }
        #endregion 
		
		#region ControlPage1_On_PostBack
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void ControlPage1_On_PostBack(object sender, EventArgs e)
		{
            Bind();
        }
        #endregion 
		
		#region  dgEqu_Desk_ItemCommand
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgEqu_Desk_ItemCommand(object source, DataGridCommandEventArgs e)
        {
           
            if (e.CommandName == "look")
            {
                Response.Redirect("frmEqu_DeskEdit.aspx?IsSameSchemaItem=1&id=" + e.Item.Cells[1].Text.ToString() + "&FlowID=-1");
            }
        }
        #endregion 

        #region dgEqu_Desk_ItemDataBound
        /// <summary>
        /// dgEqu_Desk_ItemDataBound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgEqu_Desk_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");

                string strIDs = string.Empty;           //资产ID字符串

                string strvalue = DataBinder.Eval(e.Item.DataItem, "Default").ToString();

                Label lblDefalutValue3 = (Label)e.Item.FindControl("lblDefalutValue3");
                Label lblDefalutValue4 = (Label)e.Item.FindControl("lblDefalutValue4");
                Label lblDefalutValue5 = (Label)e.Item.FindControl("lblDefalutValue5");
                if (strItemType == "3")
                    lblDefalutValue3.Text = CatalogDP.GetCatalogName(long.Parse(strvalue == "" ? "0" : strvalue));
                else if (strItemType == "4")
                    lblDefalutValue4.Text = DeptDP.GetDeptName(long.Parse(strvalue == "" ? "0" : strvalue));
                else if (strItemType == "5")
                    lblDefalutValue5.Text = UserDP.GetUserName(long.Parse(strvalue == "" ? "0" : strvalue));

                if (strvalue.Length > 25)
                {
                    Label Label1 = (Label)e.Item.FindControl("Label1");
                    Label1.Text = strvalue.Substring(0, 10) + "...";
                }


                foreach (DataGridItem item in dgEqu_Desk.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        strIDs += item.Cells[1].Text + ",";
                    }
                }
                if (strIDs.Length > 0)
                    HidEqusID.Value = strIDs.Substring(0, strIDs.Length - 1);
            }
        }
        #endregion 

        #region dgEqu_Desk_ItemCreated
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgEqu_Desk_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                int j = 0;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (isQuery)
                    {
                        if (i > 1 && i < 8)
                        {
                            j = i - 1;
                            if (i == 7)
                            {
                                e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",1);");
                            }
                            else
                            {
                                e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                            }
                        }
                    }
                    else
                    {
                        if (i > 1 && i < 8)
                        { 
                             e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + (i-2).ToString() + ",0);");
                          
                        }
                    }
                }
            }
        }
        #endregion 

        #region 事件记录
        /// <summary>
        /// 事件记录
        /// </summary>
        /// <param name="lngNoticeID"></param>
        /// <returns></returns>
        protected string GetServiceUrl(decimal lngID)
        {
            //暂时没处理分页
            string sUrl = "";
            sUrl = "javascript:window.open('../AppForms/frmIssueList.aspx?NewWin=true&ID=0&EquID=" + lngID.ToString() + "','newwindow','scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');";
            return sUrl;
        }
        #endregion

        #region 巡检历史
        /// <summary>
        /// 巡检历史
        /// </summary>
        /// <param name="lngNoticeID"></param>
        /// <returns></returns>
        protected string GetUrl(decimal lngID)
        {
            //暂时没处理分页
            string sUrl = "";
            sUrl = "javascript:window.open('frm_Equ_PatrolList.aspx?NewWin=true&EquID=" + lngID.ToString() + "','newwindow','scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');";
            return sUrl;
        }
        #endregion 

        #region 批量更新资产
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBatchUpdate_Click(object sender, EventArgs e)
        {
            Equ_DeskDP ee = new Equ_DeskDP();
            
            foreach (DataGridItem itm in dgEqu_Desk.Items)
            {
                if (itm.ItemType == ListItemType.AlternatingItem ||
                    itm.ItemType == ListItemType.Item)
                {
                    string sID = itm.Cells[1].Text;
                    CheckBox chkSelect = (CheckBox)itm.Cells[0].FindControl("chkSelect");
                    if (chkSelect.Checked)
                    {
                        ee.BatchUpdateSchemaItem(long.Parse(sID), strItemID, strItemValue,(long)Session["UserID"]);
                    }
                }
            }           

            DataTable dt = LoadData(strItemID,strItemValue);
            dgEqu_Desk.DataSource = dt.DefaultView;
            dgEqu_Desk.DataBind();            
        }
        #endregion

        #region 批量关联【快捷方式】

        #region InitDropdownList
        /// <summary>
        /// InitDropdownList
        /// </summary>
        private void InitDropdownList()
        {           
            //初始化资产列表
            DataTable dt = Equ_DeskDP.GetInitEqus();            

            Equ_RelDP ee = new Equ_RelDP();
            string strRelPropID = string.Empty;                         //关联属性ID
            string strWhere = string.Empty;

            string[] strArrEquID = HidEqusID.Value.Split(',');
            if (strArrEquID.Length > 1)
            {
                for (int j = 0; j < strArrEquID.Length; j++)
                {
                    strWhere += "  and RelID in (select RelID from Equ_Rel where Equ_ID = " + strArrEquID[j] + ") ";
                }                
            }            

            dt = ee.GetPropsByID(strWhere);               //根据资产ID串得到批量关联的资产

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    hidEqu.Value = row["RelID"].ToString();
                    hidEquName.Value = Equ_DeskDP.GetEquNameByID(hidEqu.Value);
                    txtEqu.Text = hidEquName.Value;

                    #region 判断是否有资产关联权限 与资产修改相同的判断
                    Equ_DeskDP equ = new Equ_DeskDP();
                    equ = equ.GetReCorded(long.Parse(hidEqu.Value.ToString() == "" ? "0" : hidEqu.Value.ToString()));
                    string sEquList = Session["EquLimitList"].ToString() + ",";
                    if (sEquList.Contains(equ.CatalogID.ToString() + ",") || DeptDP.IsExistUserByDept(Session["UserID"].ToString(), equ.partBranchId.ToString() == "" ? "0" : equ.partBranchId.ToString()))
                        btnImplyEquProp.Enabled = true;
                    else
                        btnImplyEquProp.Enabled = false;
                    #endregion

                    strRelPropID = row["RelPropID"].ToString();
                    txt_RelDescription.CatelogID = long.Parse(row["RelDescriptionID"].ToString() == "" ? "-1" : row["RelDescriptionID"].ToString());
                    txt_RelDescription.CatelogValue = row["RelDescription"].ToString();
                }
            }

            //初始化属性列表
            if (hidEqu.Value == "0")
                GetPropsByDeskID("-1", ddlDeskProp);                                   //初始，属性下拉内容默认为空
            else
            {
                GetPropsByDeskID(hidEqu.Value, ddlDeskProp);
                ddlDeskProp.SelectedIndex = ddlDeskProp.Items.IndexOf(ddlDeskProp.Items.FindByValue(strRelPropID));
            }
        }   
        #endregion

        #region 根据资产ID获取其对应的资产属性列表
        /// <summary>
        /// 根据资产ID获取其对应的资产属性列表
        /// </summary>
        /// <param name="strEquID"></param>
        private void GetPropsByDeskID(string strEquID, DropDownList ddlDeskProp)
        {
            string strSchemaXml = Equ_DeskDP.GetSchemaXmlByEquID(strEquID);         //资产对应其类别的配置项ID

            DataTable dt = new DataTable();

            dt.Columns.Add("ID");
            dt.Columns.Add("CHName");

            //解析xml，并存放到dt中
            if (strSchemaXml != "")
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(strSchemaXml);

                //添加基本配置
                XmlNodeList xmlList = xmlDoc.SelectNodes("EquScheme/BaseItem/AttributeItem");
                foreach (XmlNode node in xmlList)
                {
                    DataRow dr = dt.NewRow();
                    dr["ID"] = node.Attributes["ID"].Value;
                    dr["CHName"] = node.Attributes["CHName"].Value;
                    dt.Rows.Add(dr);
                }

                //添加关联配置
                xmlList = xmlDoc.SelectNodes("EquScheme/RelationConfig/AttributeItem");
                foreach (XmlNode node in xmlList)
                {
                    DataRow dr = dt.NewRow();
                    dr["ID"] = node.Attributes["ID"].Value;
                    dr["CHName"] = node.Attributes["CHName"].Value;
                    dt.Rows.Add(dr);
                }

                //添加备注配置
                xmlList = xmlDoc.SelectNodes("EquScheme/Remark/AttributeItem");
                foreach (XmlNode node in xmlList)
                {
                    DataRow dr = dt.NewRow();
                    dr["ID"] = node.Attributes["ID"].Value;
                    dr["CHName"] = node.Attributes["CHName"].Value;
                    dt.Rows.Add(dr);
                }
            }

            //绑定到关联属性下拉列表           

            ddlDeskProp.DataSource = dt.DefaultView;
            ddlDeskProp.DataValueField = "ID";
            ddlDeskProp.DataTextField = "CHName";
            ddlDeskProp.DataBind();
            ddlDeskProp.Items.Insert(0, new ListItem("", "0"));
        }
        #endregion


        #region 保存批量关联
        /// <summary>
        /// 保存批量关联
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveRel_Click(object sender, EventArgs e)
        {
            //定义配置项相同值的资产ID串
            //string strEquIDs = HidEqusID.Value;
            string strEquIDs = "";

            int iindex = 0;
            foreach (DataGridItem item in dgEqu_Desk.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    if (((CheckBox)item.Cells[0].FindControl("chkSelect")).Checked)
                    {
                        strEquIDs += item.Cells[1].Text + ",";
                        iindex++;
                    }
                }
            }

            if (iindex <= 0)
            {
                Response.Write("<script>alert('请确认选择的资产列表不为空');</script>");
                this.Master.IsSaveSuccess = false;
                return;
            }

            strEquIDs = strEquIDs.Trim(',');

            if (hidEqu.Value == "0")
            {
                Equ_RelDP.SaveItem(strEquIDs, hidEqu.Value, ddlDeskProp.SelectedValue, ddlDeskProp.SelectedItem.Text, txt_RelDescription.CatelogID.ToString(), txt_RelDescription.CatelogValue);
            }
            else
                Equ_RelDP.SaveItem(strEquIDs, long.Parse(Session["UserID"].ToString()), hidEqu.Value, ddlDeskProp.SelectedValue, ddlDeskProp.SelectedItem.Text, txt_RelDescription.CatelogID.ToString(), txt_RelDescription.CatelogValue);

            PageTool.MsgBox(this, "批量关联成功!");
        }
        #endregion 

        
        #region 资产改变时，更改其对应的属性列表
        /// <summary>
        /// btnDeskPropChange_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDeskPropChange_Click(object sender, EventArgs e)
        {
            GetPropsByDeskID(hidEqu.Value.ToString(), ddlDeskProp);
            txt_RelDescription.CatelogID = 0;

            #region 判断是否有资产关联权限 与资产修改相同的判断
            Equ_DeskDP ee = new Equ_DeskDP();
            ee = ee.GetReCorded(long.Parse(hidEqu.Value.ToString() == "" ? "0" : hidEqu.Value.ToString()));
            string sEquList = Session["EquLimitList"].ToString() + ",";
            if (sEquList.Contains(ee.CatalogID.ToString() + ",") || DeptDP.IsExistUserByDept(Session["UserID"].ToString(), ee.partBranchId.ToString() == "" ? "0" : ee.partBranchId.ToString()))
                btnImplyEquProp.Enabled = true;
            else
                btnImplyEquProp.Enabled = false;
            #endregion

        }
        #endregion

        #endregion
    }
}

