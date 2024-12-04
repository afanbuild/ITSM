/*******************************************************************
 * 版权所有：深圳市非凡信息技术有限公司
 * 描述：流程自定义扩展项显示方式-管理界面
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
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Business.Common.Configuration;
using System.Collections.Generic;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frm_ExtensionDisplayWay : BasePage
    {
        /// <summary>
        /// 设置父窗体按钮事件
        /// </summary>
        protected void SetParentButtonEvent()
        {
            base.SetupRight(Epower.ITSM.Base.Constant.FlowExtensionItemDisplayWay, true);    // 设置权限                         

            this.NormalMaster.ShowQueryButton(true);
            this.NormalMaster.Master_Button_Query_Click += new Global_BtnClick(NormalMaster_Master_Button_Query_Click);

            if (this.CanAdd())
            {
                this.NormalMaster.ShowNewButton(true);
                this.NormalMaster.Master_Button_New_Click += new Global_BtnClick(masterPage_Master_Button_New_Click);
            }

            if (this.CanDelete())
            {
                this.NormalMaster.MainID = "1";    // 在这里设置的原因：母版页中删除按钮有一个js函数，它要求MainID 必须有值。详情见相关代码。
                this.NormalMaster.ShowDeleteButton(true);
                this.NormalMaster.Master_Button_Delete_Click += new Global_BtnClick(NormalMaster_Master_Button_Delete_Click);
            }

            RightEntity re = this.GetRight(Epower.ITSM.Base.Constant.FlowExtensionItem);    // 扩展项管理权限

            if (re != null && re.CanRead)
            {
                this.NormalMaster.ShowBackUrlButton(true);
                this.NormalMaster.Btn_back.Text = "扩展项配置";
                this.NormalMaster.Master_Button_GoHistory_Click += new Global_BtnClick(NormalMaster_Master_Button_GoHistory_Click);
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
            LoadExNodeModelList();
        }

        /// <summary>
        /// 回发时调用
        /// </summary>
        private void PostBack()
        {

        }

        /// <summary>
        /// 加载拥有扩展项显示方式的环节模型列表
        /// </summary>
        private void LoadExNodeModelList()
        {
            long lngAppID = 0;
            long lngFlowModelID = 0;
            long lngNodeModelID = 0;
            String strSearchKey = String.Empty;

            String strAppID = ddlApp.SelectedValue;
            String strFlowModelID = ddlFlowModel.SelectedValue;
            String strNodeModelID = ddlNodeModel.SelectedValue;

            if (!String.IsNullOrEmpty(strAppID))
                lngAppID = long.Parse(strAppID);

            if (!String.IsNullOrEmpty(strFlowModelID) && strFlowModelID.Trim() != String.Empty)
            {
                lngFlowModelID = long.Parse(strFlowModelID);
            }

            if (!String.IsNullOrEmpty(strNodeModelID) && strNodeModelID.Trim() != String.Empty)
            {
                lngNodeModelID = long.Parse(strNodeModelID);
            }

            if (!String.IsNullOrEmpty(txtSearchKey.Text))
                strSearchKey = txtSearchKey.Text;


            ExtensionDisplayWayBS extensionDisplayWayBS = new ExtensionDisplayWayBS();
            DataTable dtExNodeModelList = extensionDisplayWayBS.GetExNodeModelList(lngAppID, lngFlowModelID, lngNodeModelID, strSearchKey);

            dgExNodeModelList.DataSource = dtExNodeModelList;
            dgExNodeModelList.DataBind();

            if (this.CanModify())
                dgExNodeModelList.Columns[dgExNodeModelList.Columns.Count - 2].Visible = true;

            if (this.CanDelete())
            {
                dgExNodeModelList.Columns[0].Visible = true;
                dgExNodeModelList.Columns[dgExNodeModelList.Columns.Count - 1].Visible = true;
            }
        }

        /// <summary>
        /// 新增显示方式
        /// </summary>
        void masterPage_Master_Button_New_Click()
        {
            Response.Redirect("~/EquipmentManager/frm_ExtensionDisplayWayEdit.aspx");
        }


        /// <summary>
        /// 删除选中的拥有扩展项显示方式的环节模型列表
        /// </summary>
        void NormalMaster_Master_Button_Delete_Click()
        {
            List<KeyValuePair<long, long>> listNodeModel = GetSelectedExNodeModelList();
            DeleteExNodeModel(listNodeModel);
        }

        /// <summary>
        ///  查询
        /// </summary>
        void NormalMaster_Master_Button_Query_Click()
        {
            LoadExNodeModelList();
        }

        /// <summary>
        /// 跳转到扩展项配置管理页面
        /// </summary>
        void NormalMaster_Master_Button_GoHistory_Click()
        {
            Response.Redirect("~/EquipmentManager/frm_Extensions2.aspx");
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

            LoadExNodeModelList();
        }

        /// <summary>
        /// 流程模型切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlFlowModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(ddlFlowModel.SelectedValue)
                || String.IsNullOrEmpty(ddlFlowModel.SelectedValue.Trim()))
            {
                ddlNodeModel.Items.Clear();
                return;
            }

            long lngAppID = long.Parse(ddlApp.SelectedValue);
            long lngFlowModelID = long.Parse(ddlFlowModel.SelectedValue);

            ExtensionDisplayWayBS.LoadNodeModelListToDropDownList(ddlNodeModel, lngAppID, lngFlowModelID);

            LoadExNodeModelList();
        }


        /// <summary>
        /// DataGrid的按钮命令事件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgExNodeModelList_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "view":
                case "modify":    // 编辑和查看的区别在于：查看模式是无法保存的！
                    GotoDetailPage(e);
                    break;
                case "delete":
                    String[] arrVal = e.CommandArgument.ToString().Split(new String[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

                    List<KeyValuePair<long, long>> listNodeModelID = new List<KeyValuePair<long, long>>();
                    long lngFlowModelID = long.Parse(arrVal[1]);
                    long lngNodeModelID = long.Parse(arrVal[2]);

                    listNodeModelID.Add(new KeyValuePair<long, long>(lngFlowModelID, lngNodeModelID));

                    DeleteExNodeModel(listNodeModelID);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 跳转到编辑页面
        /// </summary>
        /// <param name="item"></param>
        private void GotoDetailPage(DataGridCommandEventArgs e)
        {
            String[] arrVal = e.CommandArgument.ToString().Split(new String[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

            Response.Redirect(String.Format("~/EquipmentManager/frm_ExtensionDisplayWayEdit.aspx?appid={0}&flowmodelid={1}&nodemodelid={2}", arrVal[0], arrVal[1], arrVal[2]));
        }


        /// <summary>
        /// 删除拥有扩展项的流程模型
        /// </summary>
        private void DeleteExNodeModel(List<KeyValuePair<long, long>> listNodeModelID)
        {
            try
            {

                ExtensionDisplayWayBS extensionDisplayWayBS = new ExtensionDisplayWayBS();

                extensionDisplayWayBS.DeleteExNodeModelList(listNodeModelID);
                PageTool.MsgBox(this, "已删除！");

                LoadExNodeModelList();
            }
            catch (Exception ex)
            {
                E8Logger.Error(ex);
                throw ex;
            }

        }


        /// <summary>
        /// 取DataGrid中选中的环节模型列表
        /// </summary>
        /// <returns></returns>
        private List<KeyValuePair<long, long>> GetSelectedExNodeModelList()
        {
            List<KeyValuePair<long, long>> listNodeModel = new List<KeyValuePair<long, long>>();

            foreach (DataGridItem itm in dgExNodeModelList.Items)
            {
                if (itm.ItemType == ListItemType.AlternatingItem ||
                    itm.ItemType == ListItemType.Item)
                {
                    long lngFlowModelID = long.Parse(itm.Cells[1].Text);
                    long lngNodeModelID = long.Parse(itm.Cells[2].Text);

                    CheckBox chkdel = (CheckBox)itm.Cells[0].FindControl("chkDel");
                    if (chkdel.Checked)
                    {
                        listNodeModel.Add(new KeyValuePair<long, long>(lngFlowModelID, lngNodeModelID));
                    }
                }
            }

            return listNodeModel;
        }

    }
}
