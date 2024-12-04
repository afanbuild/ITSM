/*******************************************************************
 * 版权所有：深圳市非凡信息技术有限公司
 * 描述：流程自定义扩展项显示方式-编辑界面
 * 
 * 
 * 创建人：孙绍棕
 * 创建日期：2013-12-04
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
using Epower.ITSM.Business.Common.Configuration;
using System.Collections.Generic;
using Epower.DevBase.BaseTools;
using EpowerCom;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frm_ExtensionDisplayWayEdit : BasePage
    {

        /// <summary>
        /// 操作类型
        /// </summary>
        private String Action
        {
            get
            {
                return Request.QueryString["action"] ?? "";
            }
        }

        /// <summary>
        /// 设置父窗体按钮事件
        /// </summary>
        protected void SetParentButtonEvent()
        {
            base.SetupRight(Epower.ITSM.Base.Constant.FlowExtensionItemDisplayWay, true);    // 设置权限                         

            this.NormalMaster.ShowBackUrlButton(true);
            this.NormalMaster.Master_Button_GoHistory_Click += new Global_BtnClick(NormalMaster_Master_Button_GoHistory_Click);

            if (this.CanAdd() || this.CanModify())
            {
                this.NormalMaster.Master_Button_New_Click += new Global_BtnClick(NormalMaster_Master_Button_New_Click);

                this.NormalMaster.ShowSaveButton(true);
                this.NormalMaster.Master_Button_Save_Click += new Global_BtnClick(NormalMaster_Master_Button_Save_Click);
            }
        }


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
            ExtensionItemBS.LoadAppListToDropDownList(ddlApp);

            LoadExDisplayWayList();
        }

        /// <summary>
        /// 回发时调用
        /// </summary>
        private void PostBack()
        {

        }

        /// <summary>
        /// 加载扩展项显示方式列表
        /// </summary>
        private void LoadExDisplayWayList()
        {
            ExtensionDisplayWayBS extensionDisplayWayBS = new ExtensionDisplayWayBS();

            long lngAppId = -1;
            String strAppID = Request.QueryString["appid"];
            String strFlowModelID = Request.QueryString["flowmodelid"];
            String strNodeModelID = Request.QueryString["nodemodelid"];

            if (!String.IsNullOrEmpty(strAppID))
            {
                lngAppId = long.Parse(strAppID);
                ddlApp.SelectedValue = lngAppId.ToString();

                long lngFlowModelID = long.Parse(strFlowModelID);
                lngFlowModelID = FlowDP.GetOFlowModelID(lngFlowModelID);

                ExtensionItemBS.LoadFlowModelListToDropDownList(ddlFlowModel, lngAppId);
                ddlFlowModel.SelectedValue = lngFlowModelID.ToString();


                ExtensionDisplayWayBS.LoadNodeModelListToDropDownList(ddlNodeModel, lngAppId, lngFlowModelID);
                ddlNodeModel.SelectedValue = strNodeModelID;



                DataTable dtExDisplayWay = extensionDisplayWayBS.GetExDisplayWayList(lngFlowModelID, long.Parse(strNodeModelID));

                dgExtensionDisplayWay.DataSource = dtExDisplayWay;
                dgExtensionDisplayWay.DataBind();

                ddlApp.Enabled = false;
                ddlFlowModel.Enabled = false;
                ddlNodeModel.Enabled = false;


                string strNodeName = string.Empty;
                if (Action.Equals("init_all"))
                {
                    strNodeName = "全部环节";
                }
                else
                {
                    strNodeName = ddlNodeModel.SelectedItem.Text;
                }

                this.Page.Title = String.Format("环节模型:【{0}】- 可见可编辑", strNodeName);

            }
        }


        /// <summary>
        /// 保存扩展项的显示方式
        /// </summary>
        void NormalMaster_Master_Button_Save_Click()
        {
            ExtensionDisplayWayBS extensionDisplayWayBS = new ExtensionDisplayWayBS();

            try
            {
                if (this.CanAdd() == false && this.CanModify() == false)
                {
                    this.NormalMaster.IsSaveSuccess = false;

                    PageTool.MsgBox(this, "权限不够！");


                    return;
                }

                List<KeyValuePair<long, int>> listExDisplayWay = GetExDisplayWayValFromDataGrid();

                if (String.IsNullOrEmpty(ddlFlowModel.SelectedValue)
                    || String.IsNullOrEmpty(ddlNodeModel.SelectedValue))
                {
                    this.NormalMaster.IsSaveSuccess = false;

                    PageTool.MsgBox(this, "请选择流程模型和环节模型！");
                    return;
                }

                long lngFlowModelID = long.Parse(ddlFlowModel.SelectedValue);
                long lngNodeModelID = long.Parse(ddlNodeModel.SelectedValue);

                if (Action.Equals("init_all"))
                {
                    extensionDisplayWayBS.SaveAllNodeModelDisplayStatus(lngFlowModelID, listExDisplayWay);
                }
                else
                {
                    extensionDisplayWayBS.SaveExDisplayWayList(lngFlowModelID, lngNodeModelID, listExDisplayWay);
                }

                this.NormalMaster.IsSaveSuccess = false;    // 到这里就已经保存成功. 所以设置False, 是因这样就不会有"数据保存成功"的提示, 在这里是不需要这个提示的. - 2013-12-24 @孙绍棕                


                ddlApp.Enabled = false;
                ddlFlowModel.Enabled = false;
                ddlNodeModel.Enabled = false;

                if (this.CanAdd())
                { this.NormalMaster.ShowNewButton(false); }


                NormalMaster_Master_Button_GoHistory_Click();

            }
            catch (Exception ex)
            {
                E8Logger.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// 关闭可见可编辑窗体
        /// </summary>
        void NormalMaster_Master_Button_GoHistory_Click()
        {
            PageTool.AddJavaScript(this, "window.close();");
        }

        /// <summary>
        /// 新增扩展项显示方式
        /// </summary>
        void NormalMaster_Master_Button_New_Click()
        {
            Response.Redirect("~/EquipmentManager/frm_ExtensionDisplayWayEdit.aspx");
        }


        /// <summary>
        /// 应用切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(ddlApp.SelectedValue)
                || ddlApp.SelectedValue == "-1")
            {
                ddlFlowModel.Items.Clear();
                return;
            }

            long lngAppID = long.Parse(ddlApp.SelectedValue);

            ExtensionItemBS.LoadFlowModelListToDropDownList(ddlFlowModel, lngAppID);
        }

        /// <summary>
        /// 流程模型切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlFlowModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(ddlFlowModel.SelectedValue)
                || ddlFlowModel.SelectedValue == "-1")
            {
                ddlNodeModel.Items.Clear();
                return;
            }

            long lngAppID = long.Parse(ddlApp.SelectedValue);
            long lngFlowModelID = long.Parse(ddlFlowModel.SelectedValue);

            ExtensionDisplayWayBS.LoadNodeModelListToDropDownList(ddlNodeModel, lngAppID, lngFlowModelID);
        }


        /// <summary>
        /// 环节模型切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlNodeModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(ddlNodeModel.SelectedValue))
            {
                return;
            }

            long lngFlowModelID = long.Parse(ddlFlowModel.SelectedValue);

            ExtensionDisplayWayBS extensionDisplayWayBS = new ExtensionDisplayWayBS();
            DataTable dtExDisplayWay = extensionDisplayWayBS.GetBlankExDisplayWayList(lngFlowModelID);

            dgExtensionDisplayWay.DataSource = dtExDisplayWay;
            dgExtensionDisplayWay.DataBind();
        }

        /// <summary>
        /// datagrid 数据绑定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgExtensionDisplayWay_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.AlternatingItem && e.Item.ItemType != ListItemType.Item)
                return;

            SetupValToDataGridItem(e.Item);    // 将数据源中的值绑定到DataGridItem中.            
        }


        /// <summary>
        /// 设置值到DataGridItem
        /// </summary>
        /// <param name="item"></param>
        private void SetupValToDataGridItem(DataGridItem item)
        {
            DataRowView drv = item.DataItem as DataRowView;

            if (!drv.Row.Table.Columns.Contains("displaystatus"))
                return;

            CheckBox chkVisible = item.FindControl("chkVisible") as CheckBox;    // 可见            
            CheckBox chkEdit = item.FindControl("chkEdit") as CheckBox;    // 可编辑     

            int displayStatus;
            int.TryParse(drv["displaystatus"].ToString(), out displayStatus);

            if (displayStatus == 0)    // 可见，可编辑
            {
                chkVisible.Checked = true;
                chkEdit.Checked = true;
            }
            else if (displayStatus == 1)    // 可见，不可编辑
            {
                chkVisible.Checked = true;
                chkEdit.Checked = false;
            }
            else if (displayStatus == 2)    // 不可见，不可编辑
            {
                chkVisible.Checked = false;
                chkEdit.Checked = false;
            }
        }

        /// <summary>
        /// 取扩展项显示方式列表, 从DataGrid
        /// </summary>
        /// <returns></returns>
        private List<KeyValuePair<long, int>> GetExDisplayWayValFromDataGrid()
        {
            List<KeyValuePair<long, int>> listExDisplayWay = new List<KeyValuePair<long, int>>();

            foreach (DataGridItem item in dgExtensionDisplayWay.Items)
            {
                if (item.ItemType != ListItemType.AlternatingItem
                    && item.ItemType != ListItemType.Item)
                    continue;

                long fieldID = int.Parse(item.Cells[0].Text);    // 扩展项编号
                CheckBox chkVisible = item.FindControl("chkVisible") as CheckBox;    // 可见            
                CheckBox chkEdit = item.FindControl("chkEdit") as CheckBox;    // 可编辑                     

                int displayStatus = 0;
                if (chkVisible.Checked && chkEdit.Checked)    // 可见，可编辑
                    displayStatus = 0;
                else if (chkVisible.Checked && chkEdit.Checked == false)    // 可见，不可编辑
                    displayStatus = 1;
                else if ((chkVisible.Checked == false
                         && chkEdit.Checked == false) || (chkVisible.Checked == false && chkEdit.Checked))    // 不可见，不可编辑
                    displayStatus = 2;

                listExDisplayWay.Add(new KeyValuePair<long, int>(fieldID, displayStatus));
            }

            return listExDisplayWay;
        }

    }
}
