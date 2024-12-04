/*******************************************************************
 * 版权所有：深圳市非凡信息技术有限公司
 * 描述：流程自定义扩展项配置-管理界面
 * 
 * 
 * 创建人：孙绍棕
 * 创建日期：2013-12-03
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
using Epower.DevBase.BaseTools;
using System.Collections.Generic;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frm_Extensions2 : BasePage
    {
        /// <summary>
        /// 设置父窗体按钮事件
        /// </summary>
        protected void SetParentButtonEvent()
        {
            base.SetupRight(Epower.ITSM.Base.Constant.FlowExtensionItem, true);    // 设置权限                         

            this.NormalMaster.ShowQueryButton(true);
            this.NormalMaster.Master_Button_Query_Click += new Global_BtnClick(NormalMaster_Master_Button_Query_Click);

            //this.NormalMaster.ShowBackUrlButton(true);
            //this.NormalMaster.Master_Button_GoHistory_Click += new Global_BtnClick(NormalMaster_Master_Button_GoHistory_Click);

            if (this.CanAdd())
            {
                this.NormalMaster.ShowNewButton(true);
                this.NormalMaster.Master_Button_New_Click += new Global_BtnClick(masterPage_Master_Button_New_Click);
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
            LoadExFlowModelList();

            ExtensionItemBS.LoadAppListToDropDownList(ddlApp);
        }

        /// <summary>
        /// 回发时调用
        /// </summary>
        private void PostBack()
        {

        }

        /// <summary>
        /// 加载拥有扩展项的流程模型列表
        /// </summary>
        /// <param name="lngAppID">应用编号</param>
        /// <param name="lngFlowModelID">流程模型编号</param>
        /// <param name="strSearchKey">查询字符串</param>
        private void LoadExFlowModelList()
        {
            long lngAppID = 0;
            long lngFlowModelID = 0;
            String strSearchKey = String.Empty;

            String strAppID = ddlApp.SelectedValue;
            String strFlowModelID = ddlFlowModel.SelectedValue;

            if (!String.IsNullOrEmpty(strAppID))
                lngAppID = long.Parse(strAppID);

            if (!String.IsNullOrEmpty(strFlowModelID) && strFlowModelID.Trim() != String.Empty)
            {
                lngFlowModelID = long.Parse(strFlowModelID);
            }

            if (!String.IsNullOrEmpty(txtSearchKey.Text))
                strSearchKey = txtSearchKey.Text;


            ExtensionItemBS extensionItemBS = new ExtensionItemBS();
            DataTable dtExFlowModelList = extensionItemBS.GetExtFlowModelList(lngAppID, lngFlowModelID, strSearchKey);

            dgExFlowModelList.DataSource = dtExFlowModelList;
            dgExFlowModelList.DataBind();

            if (this.CanModify())
                dgExFlowModelList.Columns[dgExFlowModelList.Columns.Count - 2].Visible = true;

            if (this.CanDelete())
                dgExFlowModelList.Columns[dgExFlowModelList.Columns.Count - 1].Visible = true;
        }

        /// <summary>
        /// 新增配置项
        /// </summary>
        void masterPage_Master_Button_New_Click()
        {
            Response.Redirect("~/EquipmentManager/frm_ExtensionsEdit2.aspx");
        }

        /// <summary>
        /// 查询拥有扩展项的流程模型
        /// </summary>
        void NormalMaster_Master_Button_Query_Click()
        {
            LoadExFlowModelList();
        }


        ///// <summary>
        ///// 跳转到扩展项规则管理界面
        ///// </summary>
        //void NormalMaster_Master_Button_GoHistory_Click()
        //{
        //    Response.Redirect("~/EquipmentManager/frm_ExtensionDisplayWay.aspx");
        //}

        /// <summary>
        /// DataGrid的按钮命令事件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgExFlowModelList_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "view":
                case "modify":    // 编辑和查看的区别在于：查看模式是无法保存的！
                    GotoDetailPage(e);
                    break;
                case "delete":
                    DeleteExFlowModel(e);
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

            Response.Redirect(String.Format("~/EquipmentManager/frm_ExtensionsEdit2.aspx?appid={0}&flowmodelid={1}", arrVal[0], arrVal[1]));
        }


        /// <summary>
        /// 删除拥有扩展项的流程模型
        /// </summary>
        private void DeleteExFlowModel(DataGridCommandEventArgs e)
        {
            try
            {
                long lngFlowModelID = long.Parse(e.CommandArgument.ToString());


                ExtensionItemBS extensionItemBS = new ExtensionItemBS();

                List<long> listFlowModelID = new List<long>();
                listFlowModelID.Add(lngFlowModelID);

                extensionItemBS.DeleteExItemFlowModelList(listFlowModelID);
                PageTool.MsgBox(this, "已删除！");

                LoadExFlowModelList();
            }
            catch (Exception ex)
            {
                E8Logger.Error(ex);
                throw ex;
            }

        }

        /// <summary>
        /// 切换应用, 并加载对应的流程模型列表
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

            LoadExFlowModelList();
        }


    }
}
