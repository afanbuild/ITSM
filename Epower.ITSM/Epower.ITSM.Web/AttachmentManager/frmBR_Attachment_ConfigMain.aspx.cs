/*******************************************************************
 *
 * Description
 * 
 * 必填附件配置信息查询页面
 * Create By  :余向前
 * Create Date:2013年3月20日
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
using Epower.ITSM.Base;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;
using EpowerCom;

namespace Epower.ITSM.Web.AttachmentManager
{
    public partial class frmBR_Attachment_ConfigMain : BasePage
    {
        #region 属性
        public string sApplicationUrl = Epower.ITSM.Base.Constant.ApplicationPath;     //虚拟路径
        #endregion

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.CAttachmentConfig;
            this.Master.IsCheckRight = true;
            this.dgBR_Attachment_Config.Columns[this.dgBR_Attachment_Config.Columns.Count - 1].Visible = this.Master.GetEditRight();
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);            
            this.Master.ShowQueryPageButton();         
            this.Master.MainID = "1";
        }
        #endregion

        #region Master_Master_Button_New_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("frmBR_Attachment_ConfigEdit.aspx");
        }
        #endregion

        #region Master_Master_Button_Query_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            BindData();
        }
        #endregion

        #region Master_Master_Button_Delete_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            BR_Attachment_ConfigDP ee = new BR_Attachment_ConfigDP();
            foreach (DataGridItem itm in dgBR_Attachment_Config.Items)
            {
                if (itm.ItemType == ListItemType.AlternatingItem ||
                    itm.ItemType == ListItemType.Item)
                {
                    string sAppID = itm.Cells[3].Text;  //应用ID
                    string sOFLOWMODELID = itm.Cells[4].Text;//流程模型ID
                    CheckBox chkdel = (CheckBox)itm.Cells[0].FindControl("chkDel");
                    if (chkdel.Checked)
                    {
                        ee.DeleteAll(CTools.ToInt64(sAppID, 0), CTools.ToInt64(sOFLOWMODELID, 0));
                    }
                }
            }
            BindData();
        }
        #endregion

        #region Page_Load
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtonEvent();
            cpBR_Attachment_Config.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(BindData);
            if (!IsPostBack)
            {
                ddlAppBind();

                BindData();
            }
        }
        #endregion

        #region ddlAppBind
        /// <summary>
        /// 绑定应用名称下拉框
        /// </summary>
        private void ddlAppBind()
        {
            ddlApp.DataSource = epApp.GetAllApps().DefaultView;
            ddlApp.DataTextField = "AppName";
            ddlApp.DataValueField = "AppID";
            ddlApp.DataBind();

            //ddlApp.Items.Remove(new ListItem("通用流程", "199"));
            //ddlApp.Items.Remove(new ListItem("进出操作间", "1027"));

            ListItem itm = new ListItem("", "-1");
            ddlApp.Items.Insert(0, itm);
            ddlApp.SelectedIndex = 0;
        }
        #endregion

        #region BindData
        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindData()
        {
            DataTable dt;
            string sWhere = " 1=1 ";
            string sOrder = "";
            //应用名称
            if (ddlApp.Items.Count > 0 && CTools.ToInt64(ddlApp.SelectedItem.Value) > 0)
            {
                sWhere += " And AppID= " + ddlApp.SelectedItem.Value;
            }
            //流程模型
            if (ddlFlowModel.Items.Count > 0 && CTools.ToInt64(ddlFlowModel.SelectedItem.Value) > 0)
            {
                sWhere += " And oflowmodelid= " + ddlFlowModel.SelectedItem.Value;
            }

            BR_Attachment_ConfigDP ee = new BR_Attachment_ConfigDP();
            int iRowCount = 0;
            dt = ee.GetDataTableView(sWhere, sOrder, this.cpBR_Attachment_Config.PageSize, this.cpBR_Attachment_Config.CurrentPage, ref iRowCount);
            dgBR_Attachment_Config.DataSource = dt.DefaultView;
            dgBR_Attachment_Config.DataBind();
            this.cpBR_Attachment_Config.RecordCount = iRowCount;
            this.cpBR_Attachment_Config.Bind();
        }
        #endregion

        #region  dgBR_Attachment_Config_ItemCommand
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgBR_Attachment_Config_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                Response.Redirect("frmBR_Attachment_ConfigEdit.aspx?AppID=" + e.Item.Cells[3].Text.ToString() + "&OFlowModelID=" + e.Item.Cells[4].Text.ToString());
            }
        }
        #endregion

        #region dgBR_Attachment_Config_ItemCreated
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgBR_Attachment_Config_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                int j = 0;
                for (int i = 0; i < e.Item.Cells.Count - 3; i++)
                {
                    if (i >= 1)
                    {
                        j = i;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
        #endregion

        #region dgBR_Attachment_Config_ItemDataBound
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgBR_Attachment_Config_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                string sAppID = DataBinder.Eval(e.Item.DataItem, "AppID").ToString();
                string sOFlowModelID = DataBinder.Eval(e.Item.DataItem, "OFlowModelID").ToString();
                e.Item.Attributes.Add("ondblclick", "window.open('frmBR_Attachment_ConfigEdit.aspx?AppID=" + sAppID + "&OFlowModelID=" + sOFlowModelID + "&randomid='+GetRandom(),'MainFrame','scrollbars=yes,resizable=yes')");
            }
        }
        #endregion

        #region 应用名称下拉框改变事件
        /// <summary>
        /// 应用名称下拉框改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            long lngAppID = CTools.ToInt64(ddlApp.SelectedItem.Value, 0);
            ddlFlowModel.DataSource = CommonDP.GetAllFlowModelByAppID(lngAppID).DefaultView;
            ddlFlowModel.DataTextField = "FlowName";
            ddlFlowModel.DataValueField = "OFlowModelID";
            ddlFlowModel.DataBind();

            ListItem item = new ListItem("", "-1");
            ddlFlowModel.Items.Insert(0, item);
        }
        #endregion        
    }
}
