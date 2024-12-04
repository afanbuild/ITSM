/*******************************************************************
 * 版权所有：深圳市非凡信息技术有限公司
 * 描述：流程自定义扩展项配置-编辑页面
 * 
 * 
 * 创建人：孙绍棕
 * 创建日期：2013-12-02
 * *****************************************************************/
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.Web.Controls;
using Epower.ITSM.SqlDAL.Customer;
using Epower.ITSM.Business.Common.Configuration;
using Epower.DevBase.BaseTools;
using EpowerCom;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frm_ExtensionsEdit2 : BasePage
    {
        /// <summary>
        /// 设置父窗体按钮事件
        /// </summary>
        protected void SetParentButtonEvent()
        {
            base.SetupRight(Epower.ITSM.Base.Constant.FlowExtensionItem, true);    // 设置权限                         

            this.NormalMaster.ShowSaveButton(true);
            this.NormalMaster.ShowBackUrlButton(true);

            this.NormalMaster.Master_Button_Save_Click += new Global_BtnClick(masterPage_Master_Button_Save_Click);
            this.NormalMaster.Master_Button_GoHistory_Click += new Global_BtnClick(NormalMaster_Master_Button_GoHistory_Click);
        }

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtonEvent();

            if (!IsPostBack)
            {
                FirstPost();    // 首次提交
            }
            else
            {
                PostBack();    // 回发
            }
        }

        /// <summary>
        /// 第一次提交时调用
        /// </summary>
        private void FirstPost()
        {
            long lngFlowModelID = -1;
            String strFlowModelID = Request.QueryString["flowmodelid"];
            if (!String.IsNullOrEmpty(strFlowModelID))
            {
                lngFlowModelID = long.Parse(strFlowModelID);
            }

            LoadExtensionItemList(lngFlowModelID);

            ExtensionItemBS.LoadAppListToDropDownList(ddlApp);

            long lngAppId = -1;
            String strAppID = Request.QueryString["appid"];
            if (!String.IsNullOrEmpty(strAppID))
            {
                lngAppId = long.Parse(strAppID);
                ddlApp.SelectedValue = lngAppId.ToString();

                ExtensionItemBS.LoadFlowModelListToDropDownList(ddlFlowModel, lngAppId);
                ddlFlowModel.SelectedValue = lngFlowModelID.ToString();
            }
        }

        /// <summary>
        /// 回发时调用
        /// </summary>
        private void PostBack()
        {

        }

        /// <summary>
        /// 保存配置
        /// </summary>
        void masterPage_Master_Button_Save_Click()
        {
            if (!this.CanAdd() && !this.CanModify())
            {
                this.NormalMaster.IsSaveSuccess = false;

                PageTool.MsgBox(this, "权限不够！");
                return;
            }

            if (ddlApp.SelectedValue == "-1" ||
                ddlApp.SelectedValue == "" ||
                ddlFlowModel.SelectedValue.Trim() == "0" ||
                ddlFlowModel.SelectedValue.Trim() == "")
            {
                this.NormalMaster.IsSaveSuccess = false;

                PageTool.MsgBox(this, "请选择应用和流程模型！");

                return;
            }

            long lngAppID = long.Parse(ddlApp.SelectedValue);
            long lngFlowModelID = long.Parse(ddlFlowModel.SelectedValue);

            //lngFlowModelID = FlowModel.GetLastVersionFlowModelID(lngFlowModelID);

            DataTable dtExItemList = GetExItemListFromDataGrid();
            dtExItemList.Rows.RemoveAt(dtExItemList.Rows.Count - 1);    // 移除最后一行空白行

            try
            {
                ExtensionItemBS extensionItemBS = new ExtensionItemBS();
                extensionItemBS.SaveExItems(lngAppID, lngFlowModelID, dtExItemList);

                this.NormalMaster.IsSaveSuccess = true;



                String strURL = String.Format("~/EquipmentManager/frm_ExtensionsEdit2.aspx?appid={0}&flowmodelid={1}", ddlApp.SelectedValue, ddlFlowModel.SelectedValue);
                PageTool.AddJavaScript(this, strURL);
            }
            catch (Exception ex)
            {
                this.NormalMaster.IsSaveSuccess = false;

                E8Logger.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// 回到管理页面
        /// </summary>
        void NormalMaster_Master_Button_GoHistory_Click()
        {
            Response.Redirect("~/EquipmentManager/frm_Extensions2.aspx");
        }

        /// <summary>
        /// 切换应用, 并加载对应的流程模型列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(ddlApp.SelectedValue)
                || ddlApp.SelectedValue == "-1") { return; }

            long lngAppID = long.Parse(ddlApp.SelectedValue);

            ExtensionItemBS.LoadFlowModelListToDropDownList(ddlFlowModel, lngAppID);
        }


        /// <summary>
        /// 切换的流程模型, 并加载对应的扩展项列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlFlowModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(ddlFlowModel.SelectedValue)
                || ddlFlowModel.SelectedValue == "-1") { return; }

            long lngFlowModelID;
            long.TryParse(ddlFlowModel.SelectedValue, out lngFlowModelID);

            LoadExtensionItemList(lngFlowModelID);
        }

        /// <summary>
        /// 加载扩展项列表
        /// </summary>
        private void LoadExtensionItemList(long lngFlowModelID)
        {
            ExtensionItemBS extensionItemBS = new ExtensionItemBS();

            DataTable dtExItemList = extensionItemBS.GetExItemListByFlowModelID(lngFlowModelID);

            AddBlankRow(dtExItemList);

            dgExtensionItem.DataSource = dtExItemList;
            dgExtensionItem.DataBind();
        }


        /// <summary>
        /// datagrid 按钮命令事件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgExtensionItem_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "add":
                    AddRowForDataGrid(e);
                    break;
                case "delete":
                    DeleteRowForDataGrid(e);
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// datagrid 数据绑定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgExtensionItem_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.AlternatingItem && e.Item.ItemType != ListItemType.Item)
                return;

            SetupValToDataGridItem(e.Item);    // 将数据源中的值绑定到DataGridItem中.            
        }


        /// <summary>
        /// 取扩展项清单从DataGrid
        /// </summary>
        /// <returns></returns>
        private DataTable GetExItemListFromDataGrid()
        {
            DataTable dt = ExtensionItemBS.CreateNullTable();    // 初始化表

            foreach (DataGridItem item in dgExtensionItem.Items)
            {
                // 随便起名字的UI控件的对应关系清单

                // PanDefault -> 关联配置
                // PantxtDefault -> 基础信息
                // PantxtMDefault -> 备注
                // panDropDownList -> 下拉选择             
                // panDept -> 部门信息 
                // panUser -> 用户信息
                // PanTime -> 日期类型
                // PanNumber -> 数值类型
                //                        <asp:CheckBox ID="CheckIsTime" Checked=''
                //                            runat="server" />是否时间</asp:Panel>            

                TextBox txtID = item.FindControl("txtID") as TextBox;    // 编号  

                TextBox txtCHName = item.FindControl("txtCHName") as TextBox;    // 扩展项名
                DropDownList ddlExType = item.FindControl("ddlTypeName") as DropDownList;    // 扩展项类型

                CheckBox chkIsMust = item.FindControl("chkIsMust") as CheckBox;    // 必输?
                bool isChack = ((CheckBox)item.FindControl("CheckIsTime")).Checked;    // 是否时间            
                CheckBox chkIsSelect = item.FindControl("chkIsSelect") as CheckBox;    // 可查询?

                TextBox txtGroup = item.FindControl("txtGroup") as TextBox;    // 分组名
                TextBox TxtOrderBy = item.FindControl("TxtOrderBy") as TextBox;    // 排序号

                //Button lnkdelete = item.FindControl("lnkdelete") as Button;    // 删除
                //Button btnAddNew = item.FindControl("btnAddNew") as Button;    // 添加新行

                String strDefaultVal = String.Empty;    // 默认值
                String strExTypeName = ddlExType.SelectedValue;
                switch (strExTypeName)
                {
                    case "基础信息":
                        string strBaseInfo = ((TextBox)item.FindControl("txtDefault")).Text;
                        strDefaultVal = strBaseInfo;
                        break;
                    case "关联配置":
                        bool blnChecked = ((CheckBox)item.FindControl("chkDefault")).Checked;
                        strDefaultVal = blnChecked ? "1" : "0";
                        break;
                    case "备注信息":
                        string strRemarkVal = ((TextBox)item.FindControl("txtMDefault")).Text;
                        strDefaultVal = strRemarkVal;
                        break;
                    case "下拉选择":
                        string strVal = ((ctrFlowCataDropList)item.FindControl("ctrFlowCataDropDefault")).CatelogID.ToString();
                        strDefaultVal = strVal;
                        break;
                    case "部门信息":
                        bool blnDeptChecked = ((CheckBox)item.FindControl("CheckDept")).Checked;
                        strDefaultVal = blnDeptChecked ? "1" : "0";
                        break;
                    case "用户信息":
                        bool blnUserChecked = ((CheckBox)item.FindControl("CheckUser")).Checked;
                        strDefaultVal = blnUserChecked ? "1" : "0";
                        break;
                    case "日期类型":
                        bool blnTimeChecked = ((CheckBox)item.FindControl("CheckTime")).Checked;
                        strDefaultVal = blnTimeChecked ? "1" : "0";
                        break;
                    case "数值类型":
                        String strNumberVal = ((CtrFlowNumeric)item.FindControl("TextNumber")).Value;
                        strDefaultVal = strNumberVal == "" ? "0" : strNumberVal;
                        break;
                }

                DataRow dr = dt.NewRow();

                dr["TypeName"] = strExTypeName;
                dr["ID"] = txtID.Text;    // FieldID 默认为 0
                dr["CHName"] = txtCHName.Text.Trim();
                dr["IsMust"] = chkIsMust.Checked ? "1" : "0";
                dr["Group"] = txtGroup.Text.Trim();
                dr["Default"] = strDefaultVal;
                dr["OrderBy"] = TxtOrderBy.Text.Trim() == String.Empty ? "0" : TxtOrderBy.Text.Trim();
                dr["isChack"] = isChack ? "1" : "0";
                dr["IsSelect"] = chkIsSelect.Checked ? "1" : "0";
                dr["GroupID"] = 0;    // FlowModelID 默认为 0
                dr["GroupName"] = "";    // FlowModelName 默认为空                


                dt.Rows.Add(dr);
            }

            return dt;
        }

        /// <summary>
        /// 添加新行到 DataGrid
        /// </summary>
        /// <param name="e"></param>
        private void AddRowForDataGrid(DataGridCommandEventArgs e)
        {
            DataTable dtExItemList = GetExItemListFromDataGrid();
            dtExItemList.Rows[dtExItemList.Rows.Count - 1]["ID"] = 0;    // 已新增行

            AddBlankRow(dtExItemList);

            dgExtensionItem.DataSource = dtExItemList.DefaultView;
            dgExtensionItem.DataBind();
        }


        /// <summary>
        /// 设置值到DataGridItem
        /// </summary>
        /// <param name="item"></param>
        private void SetupValToDataGridItem(DataGridItem item)
        {
            // 随便起名字的UI控件的对应关系清单

            // PanDefault -> 关联配置
            // PantxtDefault -> 基础信息
            // PantxtMDefault -> 备注
            // panDropDownList -> 下拉选择             
            // panDept -> 部门信息 
            // panUser -> 用户信息
            // PanTime -> 日期类型
            // PanNumber -> 数值类型
            //                        <asp:CheckBox ID="CheckIsTime" Checked=''
            //                            runat="server" />是否时间</asp:Panel>  

            TextBox txtID = item.FindControl("txtID") as TextBox;    // 编号            

            DataRowView drv = item.DataItem as DataRowView;
            if (drv["ID"].ToString().Equals("-2"))    // -2 是待新增的行
            {
                txtID.Text = drv["ID"].ToString();
                return;
            }


            TextBox txtCHName = item.FindControl("txtCHName") as TextBox;    // 扩展项名
            DropDownList ddlExType = item.FindControl("ddlTypeName") as DropDownList;    // 扩展项类型

            CheckBox chkIsMust = item.FindControl("chkIsMust") as CheckBox;    // 必输?
            bool isChack = ((CheckBox)item.FindControl("CheckIsTime")).Checked;    // 是否时间            
            CheckBox chkIsSelect = item.FindControl("chkIsSelect") as CheckBox;    // 可查询?

            TextBox txtGroup = item.FindControl("txtGroup") as TextBox;    // 分组名
            TextBox TxtOrderBy = item.FindControl("TxtOrderBy") as TextBox;    // 排序号

            Button lnkdelete = item.FindControl("lnkdelete") as Button;    // 删除
            Button btnAddNew = item.FindControl("btnAddNew") as Button;    // 添加新行

            CheckBox chkIsTime = item.FindControl("CheckIsTime") as CheckBox;
            TextBox txtGroupID = item.FindControl("TxtGroupID") as TextBox;


            ddlExType.SelectedValue = drv["TypeName"].ToString();
            txtID.Text = drv["ID"].ToString();
            txtCHName.Text = drv["CHName"].ToString();
            chkIsMust.Checked = drv["IsMust"].ToString() == "1" ? true : false;
            txtGroup.Text = drv["Group"].ToString();

            TxtOrderBy.Text = drv["OrderBy"].ToString() == "0" ? String.Empty : drv["OrderBy"].ToString();
            chkIsTime.Checked = drv["isChack"].ToString() == "1" ? true : false;
            chkIsSelect.Checked = drv["isSelect"].ToString() == "1" ? true : false;
            txtGroupID.Text = drv["GroupID"].ToString();

            Panel panel = null;
            String strDefaultVal = drv["Default"].ToString();
            String strExTypeName = ddlExType.SelectedValue;
            switch (strExTypeName)
            {
                case "基础信息":
                    panel = item.FindControl("PantxtDefault") as Panel;

                    TextBox txtDefault = item.FindControl("txtDefault") as TextBox;
                    txtDefault.Text = strDefaultVal;
                    break;
                case "关联配置":
                    panel = item.FindControl("PanDefault") as Panel;

                    CheckBox chkDefault = item.FindControl("chkDefault") as CheckBox;
                    chkDefault.Checked = strDefaultVal == "1" ? true : false;
                    break;
                case "备注信息":
                    panel = item.FindControl("PantxtMDefault") as Panel;

                    TextBox txtMDefault = item.FindControl("txtMDefault") as TextBox;
                    txtMDefault.Text = strDefaultVal;
                    break;
                case "下拉选择":
                    panel = item.FindControl("panDropDownList") as Panel;

                    ctrFlowCataDropList ctrFlowCataDropDefault = item.FindControl("ctrFlowCataDropDefault") as ctrFlowCataDropList;
                    ctrFlowCataDropDefault.CatelogID = long.Parse(strDefaultVal);
                    break;
                case "部门信息":
                    panel = item.FindControl("panDept") as Panel;

                    CheckBox checkDept = item.FindControl("CheckDept") as CheckBox;
                    checkDept.Checked = strDefaultVal == "1" ? true : false;
                    break;
                case "用户信息":
                    panel = item.FindControl("panUser") as Panel;

                    CheckBox checkUser = item.FindControl("CheckUser") as CheckBox;
                    checkUser.Checked = strDefaultVal == "1" ? true : false;
                    break;
                case "日期类型":
                    panel = item.FindControl("PanTime") as Panel;

                    CheckBox checkTime = item.FindControl("CheckTime") as CheckBox;
                    checkTime.Checked = strDefaultVal == "1" ? true : false;
                    break;
                case "数值类型":
                    panel = item.FindControl("PanNumber") as Panel;

                    CtrFlowNumeric txtNumber = item.FindControl("TextNumber") as CtrFlowNumeric;
                    txtNumber.Value = strDefaultVal == "0" ? "" : strDefaultVal;
                    break;
            }


            ShowDeleteBtn(item);

        }

        /// <summary>
        /// 空白行, 用于添加新行
        /// </summary>
        /// <param name="dtExItemList"></param>
        private void AddBlankRow(DataTable dtExItemList)
        {
            DataRow dr2 = dtExItemList.NewRow();
            dr2["ID"] = -2;
            dtExItemList.Rows.Add(dr2);
        }

        /// <summary>
        /// 显示删除按钮, 隐藏新增按钮
        /// </summary>
        /// <param name="item"></param>
        private void ShowDeleteBtn(DataGridItem item)
        {
            Button lnkdelete = item.FindControl("lnkdelete") as Button;    // 删除
            Button btnAddNew = item.FindControl("btnAddNew") as Button;    // 添加新行

            lnkdelete.Visible = true;
            btnAddNew.Visible = false;
        }

        /// <summary>
        /// 删除选中行
        /// </summary>
        /// <param name="e"></param>
        private void DeleteRowForDataGrid(DataGridCommandEventArgs e)
        {
            DataTable dtExItemList = GetExItemListFromDataGrid();
            //AddBlankRow(dtExItemList);

            dtExItemList.Rows.RemoveAt(e.Item.ItemIndex);

            dgExtensionItem.DataSource = dtExItemList;
            dgExtensionItem.DataBind();
        }



    }
}
