/*******************************************************************
 * 版权所有：深圳市非凡信息技术有限公司
 * 描述：知识转移规则设置页面

 * 
 * 
 * 创建人：余向前
 * 创建日期：2013-06-03 
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
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Epower.ITSM.SqlDAL;
using EpowerCom;
using Epower.ITSM.Web.Controls;
using System.Collections.Generic;
using Epower.ITSM.Base;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.InformationManager
{
    public partial class frmInf_Shift : BasePage
    {
        #region SetParentButtonEvent
        /// <summary>
        /// 设置父窗体按钮事件
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.InfTransfer;
            this.Master.IsCheckRight = true;

            if (this.Request.QueryString["id"] != null)
            {
                this.Master.MainID = this.Request.QueryString["id"].ToString();
            }

            this.Master.Master_Button_Save_Click += new Global_BtnClick(Master_Master_Button_Save_Click);
            this.Master.ShowSaveButton(true);
        }

        void Master_Master_Button_Save_Click()
        {
            long lngAppID = long.Parse(cboApp.SelectedValue.ToString() == "" ? "0" : cboApp.SelectedValue.ToString());
            long lngOFlowModelID = long.Parse(cboFlowModel.SelectedValue.ToString() == "" ? "0" : cboFlowModel.SelectedValue.ToString());

            if (lngAppID <= 0)
            {
                PageTool.MsgBox(Page, "请选择应用!");
                this.Master.IsSaveSuccess = false;
                return;
            }

            string str = "";

            DataTable dt = GetDetailItem(true, 0, ref str);

            //删除此流程模型下所有配置进度信息

            Inf_transfer_setDP.DeleteAll(lngAppID);

            //循环插入新的配置信息
            foreach (DataRow dr in dt.Rows)
            {
                Inf_transfer_setDP ee = new Inf_transfer_setDP();
                ee.AppID = lngAppID;
                ee.OFlowModelID = lngOFlowModelID;
                ee.FIELDDESCRIPTION = dr["FIELDDESCRIPTION"].ToString();
                ee.FLOWFIELD = dr["FLOWFIELD"].ToString();
                ee.INFOFIELD = dr["INFOFIELD"].ToString();                
               
                ee.InsertRecorded(ee);
            }

            BindData();
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            //设置主页面

            SetParentButtonEvent();

            if (!IsPostBack)
            {
                //绑定下拉框

                ddlAppBind();
            }
        }

        #region 绑定Datagrid
        /// <summary>
        /// 绑定Datagrid
        /// </summary>
        private void BindData()
        {
            DataTable dt = new DataTable();

            long lngAppID = long.Parse(cboApp.SelectedValue.ToString() == "" ? "0" : cboApp.SelectedValue.ToString());
            long lngOFlowModelID = long.Parse(cboFlowModel.SelectedValue.ToString() == "" ? "0" : cboFlowModel.SelectedValue.ToString());

            dt = Inf_transfer_setDP.GetDataTable(lngAppID, " order by ID ");

            dgCondition.DataSource = dt.DefaultView;
            dgCondition.DataBind();
        }
        #endregion

        #region 绑定应用下拉框

        /// <summary>
        /// 绑定应用下拉框
        /// </summary>
        private void ddlAppBind()
        {
            //cboApp.DataSource = epApp.GetAllApps().DefaultView;
            //cboApp.DataTextField = "AppName";
            //cboApp.DataValueField = "AppID";
            //cboApp.DataBind();

            //cboApp.Items.Remove(new ListItem("通用流程", "199"));
            //cboApp.Items.Remove(new ListItem("进出操作间", "1027"));

            cboApp.Items.Clear();
            cboApp.Items.Add(new ListItem("事件管理", "1026"));
            cboApp.Items.Add(new ListItem("问题管理", "210"));
            cboApp.Items.Add(new ListItem("变更管理", "420"));

            ListItem itm = new ListItem("", "-1");
            cboApp.Items.Insert(0, itm);
            cboApp.SelectedIndex = 0;

            ListItem itmModel = new ListItem("", "-1");
            cboFlowModel.Items.Insert(0, itmModel);
            cboFlowModel.SelectedIndex = 0;
        }
        #endregion

        #region  绑定流程模板
        /// <summary>
        /// 绑定流程模板
        /// </summary>
        private void BindOFlowModel()
        {
            string stWhere = cboApp.SelectedItem.Value == "-1" ? "" : " and AppID=" + cboApp.SelectedItem.Value;
            stWhere = stWhere + " and status=1 and deleted=0 ";
            cboFlowModel.DataSource = MailAndMessageRuleDP.getAllFlowModel(stWhere).DefaultView;
            cboFlowModel.DataTextField = "flowname";
            cboFlowModel.DataValueField = "oflowmodelid";
            cboFlowModel.DataBind();

            ListItem itmModel = new ListItem("", "-1");
            cboFlowModel.Items.Insert(0, itmModel);
            cboFlowModel.SelectedIndex = 0;
        }
        #endregion

        #region 应用名称下拉框改变时执行
        /// <summary>
        /// 应用名称下拉框改变时执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cboApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            //BindOFlowModel();
            BindData();
        }
        #endregion

        #region cboFlowModel 流程模型下拉框改变事件

        /// <summary>
        /// cboFlowModel 流程模型下拉框改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cboFlowModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }
        #endregion

        #region dgCondition_ItemDataBound
        /// <summary>
        /// 规则设置，绑定数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgCondition_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                //绑定知识字段下拉框
                DropDownList dlINFOFIELD = (DropDownList)e.Item.FindControl("drpAddINFOFIELD");
                dlINFOFIELD.Items.Clear();

                dlINFOFIELD.Items.Add(new ListItem("--请选择--", ""));
                dlINFOFIELD.Items.Add(new ListItem(PageDeal.GetLanguageValue("info_Title").Replace("&nbsp;", ""), "Title"));
                dlINFOFIELD.Items.Add(new ListItem(PageDeal.GetLanguageValue("info_PKey").Replace("&nbsp;", ""), "PKey"));
                dlINFOFIELD.Items.Add(new ListItem(PageDeal.GetLanguageValue("info_Tags").Replace("&nbsp;", ""), "Tags"));
                dlINFOFIELD.Items.Add(new ListItem(PageDeal.GetLanguageValue("info_Content").Replace("&nbsp;", ""), "Content"));



                HtmlInputHidden hdINFOFIELD = (HtmlInputHidden)e.Item.FindControl("HidAddINFOFIELD");
                dlINFOFIELD.SelectedIndex = dlINFOFIELD.Items.IndexOf(dlINFOFIELD.Items.FindByValue(hdINFOFIELD.Value));

            }
            else if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //绑定知识字段下拉框
                DropDownList dlINFOFIELD = (DropDownList)e.Item.FindControl("drpINFOFIELD");
                dlINFOFIELD.Items.Clear();

                dlINFOFIELD.Items.Add(new ListItem("--请选择--", ""));
                dlINFOFIELD.Items.Add(new ListItem(PageDeal.GetLanguageValue("info_Title").Replace("&nbsp;", ""), "Title"));
                dlINFOFIELD.Items.Add(new ListItem(PageDeal.GetLanguageValue("info_PKey").Replace("&nbsp;", ""), "PKey"));
                dlINFOFIELD.Items.Add(new ListItem(PageDeal.GetLanguageValue("info_Tags").Replace("&nbsp;", ""), "Tags"));
                dlINFOFIELD.Items.Add(new ListItem(PageDeal.GetLanguageValue("info_Content").Replace("&nbsp;", ""), "Content"));

                HtmlInputHidden hdINFOFIELD = (HtmlInputHidden)e.Item.FindControl("hidINFOFIELD");
                dlINFOFIELD.SelectedIndex = dlINFOFIELD.Items.IndexOf(dlINFOFIELD.Items.FindByValue(hdINFOFIELD.Value));
            }
        }
        #endregion

        #region dgCondition_ItemCommand
        /// <summary>
        /// 规则设置
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgCondition_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            DataTable dt = new DataTable();
            if (e.CommandName == "Delete")
            {
                string hidId = "";

                dt = GetDetailItem(true, e.Item.ItemIndex, ref hidId);
                dt.Rows.RemoveAt(e.Item.ItemIndex);
                dgCondition.DataSource = dt.DefaultView;
                dgCondition.DataBind();
            }
            else if (e.CommandName == "Add")
            {
                string hidid = "";

                dt = GetDetailItem(false, e.Item.ItemIndex, ref hidid);

                dgCondition.DataSource = dt.DefaultView;
                dgCondition.DataBind();

            }
        }
        #endregion

        #region 创建DataTable结构
        /// <summary>
        /// 创建 datatable结构
        /// </summary>
        /// <returns></returns>
        private DataTable CreateNullTable()
        {
            DataTable dt = new DataTable("InfTransferSet");

            dt.Columns.Add("ID");
            dt.Columns.Add("AppID");
            dt.Columns.Add("OFlowModelID");
            dt.Columns.Add("FIELDDESCRIPTION");
            dt.Columns.Add("FLOWFIELD");
            dt.Columns.Add("INFOFIELD");          
            return dt;
        }
        #endregion

        #region 得到DataGrid里设置的规则信息
        /// <summary>
        /// 得到DataGrid里设置的规则信息
        /// </summary>
        /// <param name="isAll"></param>
        /// <param name="indexs"></param>
        /// <param name="strHidAddValue"></param>
        /// <returns></returns>
        private DataTable GetDetailItem(bool isAll, int indexs, ref string strHidAddValue)
        {
            DataTable dt = CreateNullTable();

            DataRow dr;  //数据行

            int id = 0;

            #region  构建DataTable

            foreach (DataGridItem row in dgCondition.Controls[0].Controls)
            {
                id++;
                if (row.ItemType == ListItemType.Footer)
                {
                    //应用ID
                    long lngAppID = long.Parse(cboApp.SelectedValue.ToString() == "" ? "0" : cboApp.SelectedValue.ToString());
                    //流程模型ID
                    long lngOFlowModelID = long.Parse(cboFlowModel.SelectedValue.ToString() == "" ? "0" : cboFlowModel.SelectedValue.ToString());
                    //知识字段
                    DropDownList drINFOFIELD = (DropDownList)row.FindControl("drpAddINFOFIELD");
                    string strINFOFIELD = drINFOFIELD.SelectedItem.Value;
                    //转移字段
                    string strFLOWFIELD = ((CtrFlowFormText)row.FindControl("CtrFootFLOWFIELD")).Value;
                    //转移字段描述
                    string strFIELDDESCRIPTION = ((CtrFlowFormText)row.FindControl("CtrFootFIELDDESCRIPTION")).Value;                    

                    if (row.ItemIndex == indexs)
                    {
                        strHidAddValue = strINFOFIELD;
                    }

                    if (lngAppID != 0 && strINFOFIELD.Trim() != string.Empty && strFLOWFIELD.Trim() != string.Empty && strFIELDDESCRIPTION.Trim() != string.Empty)
                    {
                        dr = dt.NewRow();
                        dr["ID"] = 0;
                        dr["AppID"] = lngAppID;
                        dr["OFlowModelID"] = lngOFlowModelID;
                        dr["FIELDDESCRIPTION"] = strFIELDDESCRIPTION.Trim();
                        dr["FLOWFIELD"] = strFLOWFIELD.Trim();
                        dr["INFOFIELD"] = strINFOFIELD.Trim();                        
                        dt.Rows.Add(dr);

                    }
                    else
                    {
                        if (!isAll)
                        {
                            if (lngAppID <= 0)
                                PageTool.MsgBox(this, "请选择应用!");
                            else if(strFLOWFIELD.Trim() == string.Empty)
                                PageTool.MsgBox(this, "请输入字段名称!");
                            else if (strFIELDDESCRIPTION.Trim() == string.Empty)
                                PageTool.MsgBox(this, "请输入字段描述!");
                            else if (strINFOFIELD.Trim() == string.Empty)
                                PageTool.MsgBox(this, "请选择知识表字段!");
                            
                        }
                    }
                }
                else if (row.ItemType == ListItemType.Item || row.ItemType == ListItemType.AlternatingItem)
                {
                    //应用ID
                    long lngAppID = long.Parse(cboApp.SelectedValue.ToString() == "" ? "0" : cboApp.SelectedValue.ToString());
                    //流程模型ID
                    long lngOFlowModelID = long.Parse(cboFlowModel.SelectedValue.ToString() == "" ? "0" : cboFlowModel.SelectedValue.ToString());
                    //知识字段
                    DropDownList drINFOFIELD = (DropDownList)row.FindControl("drpINFOFIELD");
                    string strINFOFIELD = drINFOFIELD.SelectedItem.Value;
                    //转移字段
                    string strFLOWFIELD = ((CtrFlowFormText)row.FindControl("CtrFLOWFIELD")).Value;
                    //转移字段描述
                    string strFIELDDESCRIPTION = ((CtrFlowFormText)row.FindControl("CtrFIELDDESCRIPTION")).Value;

                    if (row.ItemIndex == indexs)
                    {
                        strHidAddValue = strINFOFIELD;
                    }

                    if (lngOFlowModelID != 0 && strINFOFIELD.Trim() != string.Empty && strFLOWFIELD.Trim() != string.Empty && strFIELDDESCRIPTION.Trim() != string.Empty)
                    {
                        dr = dt.NewRow();
                        dr["ID"] = 0;
                        dr["AppID"] = lngAppID;
                        dr["OFlowModelID"] = lngOFlowModelID;
                        dr["FIELDDESCRIPTION"] = strFIELDDESCRIPTION.Trim();
                        dr["FLOWFIELD"] = strFLOWFIELD.Trim();
                        dr["INFOFIELD"] = strINFOFIELD.Trim();
                        dt.Rows.Add(dr);

                    }
                }
            }

            #endregion


            return dt;
        }
        #endregion        

    }
}
