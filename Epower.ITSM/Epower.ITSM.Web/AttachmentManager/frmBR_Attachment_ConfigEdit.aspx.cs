/*******************************************************************
 *
 * Description
 * 
 * 必填附件配置信息编辑页面
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
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.ITSM.SqlDAL;
using EpowerCom;
using Epower.ITSM.Web.Controls;

namespace Epower.ITSM.Web.AttachmentManager
{
    public partial class frmBR_Attachment_ConfigEdit : BasePage
    {
        public string sApplicationUrl = Constant.ApplicationPath;     //虚拟路径

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.CAttachmentConfig;
            this.Master.IsCheckRight = true;
            if (this.Request.QueryString["AppID"] != null && this.Request.QueryString["OFlowModelID"] != null)
            {
                this.Master.MainID = this.Request.QueryString["AppID"].ToString() + "," + this.Request.QueryString["OFlowModelID"];
            }

            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);
            this.Master.Master_Button_Save_Click += new Global_BtnClick(Master_Master_Button_Save_Click);
            this.Master.Master_Button_GoHistory_Click += new Global_BtnClick(Master_Master_Button_GoHistory_Click);
            if (string.IsNullOrEmpty(this.Master.MainID.ToString()))
                this.Master.ShowAddPageButton();
            else
                this.Master.ShowEditPageButton();
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

        #region Master_Master_Button_Delete_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            if (!String.IsNullOrEmpty(this.Master.MainID.ToString()))
            {
                string[] arr = this.Master.MainID.Split(',');
                string sAppID = arr[0];  //应用ID
                string sOFLOWMODELID = arr[1];  //流程模型ID

                BR_Attachment_ConfigDP ee = new BR_Attachment_ConfigDP();
                ee.DeleteAll(CTools.ToInt64(sAppID, 0), CTools.ToInt64(sOFLOWMODELID, 0));

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
            Response.Redirect("frmBR_Attachment_ConfigMain.aspx");            
        }
        #endregion

        #region Master_Master_Button_Save_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Save_Click()
        {
            //应用名称必填
            long lngAppID = -1;
            if (ddlApp.Items.Count > 0)
                lngAppID = CTools.ToInt64(ddlApp.SelectedItem.Value);
            if (lngAppID <= 0)
            {
                PageTool.MsgBox(this, "请选择应用名称!");
                this.Master.IsSaveSuccess = false;
                return;
            }
            //流程模型必填
            long lngOFlowModelID = -1;
            if (ddlFlowModel.Items.Count > 0)
                lngOFlowModelID = CTools.ToInt64(ddlFlowModel.SelectedItem.Value);
            if (lngOFlowModelID <= 0)
            {
                PageTool.MsgBox(this, "请选择流程名称!");
                this.Master.IsSaveSuccess = false;
                return;
            }

            //获取配置信息
            DataTable dt = GetDetailItem(true);

            //先删除此AppID和OFlowModelID相关的所有配置信息
            BR_Attachment_ConfigDP ee = new BR_Attachment_ConfigDP();
            ee.DeleteAll(lngAppID, lngOFlowModelID);
            //循环插入数据
            foreach (DataRow dr in dt.Rows)
            {
                ee.AppID = lngAppID;
                ee.OFlowModelID = lngOFlowModelID;
                ee.NodeModelID = CTools.ToInt64(dr["NodeModelID"].ToString(), 0);
                ee.NodeName = dr["NODENAME"].ToString();
                ee.Operators = CTools.ToInt(dr["Operators"].ToString(), 0);
                ee.AttachmentName = dr["AttachmentName"].ToString();
                ee.AttachmentType = dr["AttachmentType"].ToString();

                ee.InsertRecorded(ee);
            }


            Master_Master_Button_GoHistory_Click();
        }
        #endregion

        #region Page_Load
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
                ddlAppBind(); //绑定应用名称下拉框

                LoadData();   //初始化加载数据
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

        #region ddlFlowModelBind
        /// <summary>
        /// 绑定流程模型下拉框数据
        /// </summary>
        private void ddlFlowModelBind()
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

        #region LoadData
        /// <summary>
        /// 初始化数据
        /// </summary>
        private void LoadData()
        {
            if (!string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                string[] arr = this.Master.MainID.Split(',');
                string sAppID = arr[0];  //应用ID
                string sOFLOWMODELID = arr[1];  //流程模型ID

                BR_Attachment_ConfigDP ee = new BR_Attachment_ConfigDP();
                string strWhere = " and AppID = " + sAppID + " and OFlowModelID=" + sOFLOWMODELID;
                string strOrder = " order by NodeModelID asc ";

                DataTable dt = ee.GetDataTable(strWhere, strOrder);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ddlApp.SelectedIndex = ddlApp.Items.IndexOf(ddlApp.Items.FindByValue(dt.Rows[0]["AppID"].ToString()));
                    //绑定流程模型下拉框数据
                    ddlFlowModelBind();
                    ddlFlowModel.SelectedIndex = ddlFlowModel.Items.IndexOf(ddlFlowModel.Items.FindByValue(dt.Rows[0]["OFlowModelID"].ToString()));

                    ddlApp.Enabled = false;
                    ddlFlowModel.Enabled = false;
                }

                dgBR_Attachment_Config.DataSource = dt.DefaultView;
                dgBR_Attachment_Config.DataBind();
            }
            else
            {
                DataTable dt = CreateNullTable();
                dgBR_Attachment_Config.DataSource = dt.DefaultView;
                dgBR_Attachment_Config.DataBind();
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
            ddlFlowModelBind();
        }
        #endregion
      
        #region ddlFlowModel 流程模型下拉框改变事件
        /// <summary>
        /// ddlFlowModel 流程模型下拉框改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlFlowModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sAppID = "-1";
            if (ddlApp.Items.Count > 0)
                sAppID = ddlApp.SelectedItem.Value;
            string sOFLOWMODELID = "-1";
            if (ddlFlowModel.Items.Count > 0)
                sOFLOWMODELID = ddlFlowModel.SelectedItem.Value;

            BR_Attachment_ConfigDP ee = new BR_Attachment_ConfigDP();
            string strWhere = " and AppID = " + sAppID + " and OFlowModelID=" + sOFLOWMODELID;
            string strOrder = " order by NodeModelID asc ";

            DataTable dt = ee.GetDataTable(strWhere, strOrder);

            dgBR_Attachment_Config.DataSource = dt.DefaultView;
            dgBR_Attachment_Config.DataBind();

        }
        #endregion

        #region Grid相关事件
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgBR_Attachment_Config_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DropDownList ddlNode = (DropDownList)e.Item.FindControl("ddlNode");
                long lngAppID = 0;
                if (ddlApp.Items.Count > 0)
                    lngAppID = CTools.ToInt64(ddlApp.SelectedValue);
                long lngOFlowModelID = 0;
                if (ddlFlowModel.Items.Count > 0)
                    lngOFlowModelID = CTools.ToInt64(ddlFlowModel.SelectedValue);

                BindddlNodeControl(ddlNode, lngAppID, lngOFlowModelID); //绑定环节下拉框
                string strNodeModelID = DataBinder.Eval(e.Item.DataItem, "NodeModelID").ToString();
                ddlNode.SelectedIndex = ddlNode.Items.IndexOf(ddlNode.Items.FindByValue(strNodeModelID));

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgBR_Attachment_Config_ItemCommand(object source, DataGridCommandEventArgs e)
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
                dgBR_Attachment_Config.DataSource = dt.DefaultView;
                dgBR_Attachment_Config.DataBind();
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
            dr["NodeModelID"] = "-1";
            dr["NODENAME"] = "";
            dr["Operators"] = "0";
            dr["AttachmentName"] = "";
            dr["AttachmentType"] = "";
            dt.Rows.Add(dr);
            dgBR_Attachment_Config.DataSource = dt.DefaultView;
            dgBR_Attachment_Config.DataBind();
        }

        /// <summary>
        /// 绑定环节下拉框
        /// </summary>
        /// <param name="ddl">下拉框对象</param>
        /// <param name="lngAppID">应用ID</param>
        /// <param name="lngOFlowModelID">流程模型ID</param>
        private void BindddlNodeControl(DropDownList ddl,long lngAppID,long lngOFlowModelID)
        {
            DataTable dt = CommonDP.GetAllNodes(lngAppID, lngOFlowModelID);

            ddl.DataSource = dt.DefaultView;
            ddl.DataTextField = "NodeName";
            ddl.DataValueField = "NodeModelID";            
            ddl.DataBind();

            ListItem itm = new ListItem("", "-1");
            ddl.Items.Insert(0, itm);

        }

        /// <summary>
        /// 创建 datatable结构
        /// </summary>
        /// <returns></returns>
        private DataTable CreateNullTable()
        {
            DataTable dt = new DataTable("AttachmentConfig");
            dt.Columns.Add("ID");
            dt.Columns.Add("NodeModelID");
            dt.Columns.Add("NODENAME");
            dt.Columns.Add("Operators");
            dt.Columns.Add("AttachmentName");
            dt.Columns.Add("AttachmentType");           

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

            if (dgBR_Attachment_Config.Items.Count > 0)
            {
                foreach (DataGridItem row in dgBR_Attachment_Config.Controls[0].Controls)
                {
                    if (row.ItemType == ListItemType.Item || row.ItemType == ListItemType.AlternatingItem)
                    {
                        bool isOk = true;
                        string strmsg = "";

                        string sID = id.ToString();
                        id++;
                        //环节信息
                        string strNodeModelID = "-1";
                        string strNodeName = "";
                        DropDownList ddlNode = (DropDownList)row.FindControl("ddlNode");
                        if (ddlNode.Items.Count > 0)
                        {
                            strNodeModelID = ddlNode.SelectedItem.Value;
                            strNodeName = ddlNode.SelectedItem.Text;
                        }
                        //比较符
                        string strOperators = ((DropDownList)row.FindControl("ddlOperate")).SelectedItem.Value;
                        //必填附件名称
                        string strAttachmentName = ((CtrFlowFormText)row.FindControl("CtrFlowAttachmentName")).Value.ToString();
                        //必填附件类型
                        string strAttachmentType = ((DropDownList)row.FindControl("ddlAttachmentType")).SelectedItem.Value;

                        if (CTools.ToInt64(strNodeModelID) <= 0 || strAttachmentName.Trim() == string.Empty)
                        {
                            isOk = false;
                            strmsg = "信息不完整，有 * 的不能为空!";
                        }


                        if (isOk)
                        {

                            dr = dt.NewRow();

                            dr["ID"] = sID.Trim();
                            dr["NodeModelID"] = strNodeModelID;
                            dr["NODENAME"] = strNodeName;
                            dr["Operators"] = strOperators;
                            dr["AttachmentName"] = strAttachmentName;
                            dr["AttachmentType"] = strAttachmentType;

                            dt.Rows.Add(dr);

                        }
                        else
                        {
                            PageTool.MsgBox(this, strmsg);
                        }
                    }
                }
            }

            return dt;
        }
        #endregion

       
    }
}
