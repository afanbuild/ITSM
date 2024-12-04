/****************************************************************************
 * 
 * description:设备项目选择
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-10-05
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
using System.Xml;
using System.Drawing;
using System.Text;

using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using EpowerCom;
using EpowerGlobal;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frmEqu_SelectItem : BasePage
    {
        RightEntity reTrace = null;  //权限
        #region
        /// <summary>
        /// 获取传过来的路径
        /// </summary>
        public string Opener_ClientId
        {

            get
            {
                return (Request["Opener_ClientId"] == null) ? "" : Request["Opener_ClientId"].ToString().Replace("hidClientId_ForOpenerPage", "");
            }
        }
        #endregion

        #region 属性
        /// <summary>
        /// 设备分类ID
        /// </summary>
        protected string CatalogID
        {
            get
            {
                return CtrEquCataDropList1.CatelogID.ToString();
            }
        }

        /// <summary>
        /// 设备分类名称
        /// </summary>
        protected string CatalogName
        {
            get
            {
                return CtrEquCataDropList1.CatelogValue.Trim();
            }
        }

        /// <summary>
        /// 设备分类名称
        /// </summary>
        protected string FullID
        {
            get
            {
                return Equ_SubjectDP.GetSubjectFullID(long.Parse(CatalogID));
            }
        }
        #endregion 

        #region 自定义项
        /// <summary>
        /// 新增项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.Master.MainID.ToString().Trim()))
            {
                PageTool.MsgBox(this,"设备资料不能为空！");
                return;
            }
            Equ_DeskDefineItemDP ee = new Equ_DeskDefineItemDP();
            ee.ContractID = decimal.Parse(this.Master.MainID);
            ee.Deleted = (int)eRecord_Status.eNormal;
            ee.ItemName = txtItemName.Text.Trim();
            ee.Name = txtName.Text.Trim();

            if (!string.IsNullOrEmpty(this.hidItemNameID.Value.Trim()))
            {
                ee.ID = decimal.Parse(this.hidItemNameID.Value.Trim());
                ee.UpdateRecorded(ee);
            }
            else
                ee.InsertRecorded(ee);
            LoadItemData();
            this.txtItemName.Text = string.Empty;
        }
        /// <summary>
        /// 删除项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string sSQL = string.Empty;
            StringBuilder sb = new StringBuilder();
            sSQL = "update Equ_DeskDefineItem set Deleted=" + ((int)eRecord_Status.eDeleted).ToString() + " where ContractID=" + this.Master.MainID.Trim();
            if (dtlstProject.Items.Count > 0)
            {
                //批量删除
                for (int i = 0; i < dtlstProject.Items.Count; i++)
                {
                    Label lblID = (Label)dtlstProject.Items[i].FindControl("lblID");
                    CheckBox chkDel = (CheckBox)dtlstProject.Items[i].FindControl("chkDel");
                    if (chkDel != null && chkDel.Checked)
                    {
                        if(string.IsNullOrEmpty(sb.ToString()))
                            sb.Append(" ID=" + lblID.Text.Trim());
                        else
                            sb.Append(" or ID=" + lblID.Text.Trim());
                    }

                }
            }

            if (!string.IsNullOrEmpty(sb.ToString()))
            {
                sSQL += " and (" + sb.ToString() + ")";
                CommonDP.ExcuteSql(sSQL.ToString());

                LoadItemData();
                this.txtItemName.Text = string.Empty;
                this.hidItemNameID.Value = string.Empty;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected void LoadItemData()
        {
            if (string.IsNullOrEmpty(this.Master.MainID.ToString()))
            {
                TableImg2.Visible = false;
                Table2.Visible = false;
                return;
            }
            else 
            {
                TableImg2.Visible = true;
                Table2.Visible = true;
            }
            DataTable dt;
            Equ_DeskDefineItemDP ee = new Equ_DeskDefineItemDP();
            string sWhere = " and ContractID=" + this.Master.MainID.ToString();
            dt = ee.GetDataTable(sWhere,string.Empty);
            dtlstProject.DataSource = dt.DefaultView;
            dtlstProject.DataBind();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dtlstProject_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "select")
            {
                Label lblID = (Label)e.Item.FindControl("lblID");
                LinkButton lblItemName = (LinkButton)e.Item.FindControl("lblItemName");
                if (lblID != null)
                {
                    this.hidItemNameID.Value = lblID.Text.Trim();
                    this.txtItemName.Text = lblItemName.Text.Trim();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected void DeleteItem()
        {
            string sSQL = string.Empty;
            StringBuilder sb = new StringBuilder();
            sSQL = "update Equ_DeskDefineItem set Deleted=" + ((int)eRecord_Status.eDeleted).ToString() + " where ContractID=" + this.Master.MainID.Trim();
            CommonDP.ExcuteSql(sSQL.ToString());
        }

        /// <summary>
        /// 选择项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSelect_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dtlstProject.Items.Count; i++)
            {
                Label lblID = (Label)dtlstProject.Items[i].FindControl("lblID");
                string sID = lblID.Text.Trim();
                CheckBox chkdel = (CheckBox)dtlstProject.Items[i].FindControl("chkDel");
                if (chkdel.Checked)
                {
                    sb.Append(sID + ",");
                }
            }
            StringBuilder sbText = new StringBuilder();
            sbText.Append("<script>");
            sbText.Append("var arr = new Array();");
            // 项目ID
            sbText.Append("arr[0] ='" + sb.ToString() + "';");
            //设备ID
            sbText.Append("arr[1] ='" + this.Master.MainID.Trim() + "';");
            //====zxl==
            sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidItemArrID').value=arr[0];");

            sbText.Append(" var ss=window.opener.document.getElementById('" + Opener_ClientId + "hidItemArrID').value;");
            sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidbtnAddItem').click();");
            sbText.Append("window.close();");

            //========zxl==
            // 关闭窗口
            sbText.Append("top.close();");
            sbText.Append("</script>");
            // 向客户端发送
            Page.RegisterStartupScript(DateTime.Now.ToString(), sbText.ToString());
        }
        #endregion 

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            if (this.Request.QueryString["id"] != null)
            {
                this.Master.MainID = this.Request.QueryString["id"].ToString();
            }
            this.Master.TableVisible = false;
             SetFormReadOnly();
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
                reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.CustomerService];

                if (Request["subjectid"] != null)
                {
                    CtrEquCataDropList1.CatelogID = decimal.Parse(Request["subjectid"].ToString());
                }

                LoadData();

                //维护记录
                //BindData();

                //自定义项
                LoadItemData();

                //Session["FromUrl"] = "close";
            }

            if (this.txtName.Text.Trim() != this.hidEquName.Value.Trim())   //刷新丢失值
                this.txtName.Text = this.hidEquName.Value.Trim();
        }

        /// <summary>
        /// 根据设备加载数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLoadItem_Click(object sender, EventArgs e)
        {
            this.Master.MainID = this.hidEqu.Value.Trim();
            reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.CustomerService];

            LoadData();

            //维护记录
            //BindData();

            //自定义项
            LoadItemData();
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
                Equ_DeskDP ee = new Equ_DeskDP();
                ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                txtName.Text = ee.Name.ToString();
                txtCode.Text = ee.Code.ToString();

                txtMulu.Text = ee.ListName;
                lblMulu.Text = ee.ListName;
                txtOpionBank.Text = ee.partBankName;
                lblOpionBank.Text = ee.partBankName;

                txtDept.Text = ee.partBranchName;
                lbltDept.Text = ee.partBranchName;

                CtrEquCataDropList1.CatelogID = ee.CatalogID;

                lblName.Text = ee.Name.ToString();
                lblCode.Text = ee.Code.ToString();

            }
        }
        #endregion

        #region InitObject
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ee"></param>
        private void InitObject(Equ_DeskDP ee)
        {
            ee.Name = txtName.Text.Trim();
            ee.Code = txtCode.Text.Trim();

             ee.ListName=txtMulu.Text;

             ee.partBankName = txtOpionBank.Text;


             ee.partBranchName = txtDept.Text;


             ee.CatalogID = decimal.Parse(CatalogID);
             ee.CatalogName = CatalogName;
             ee.FullID = FullID;
        }
        #endregion

        #region  SetFormReadOnly
        /// <summary>
        /// 
        /// </summary>
        private void SetFormReadOnly()
        {
            //txtName.Visible = false;
            txtCode.Visible = false;
            txtMulu.Visible = false;
            txtOpionBank.Visible = false;
            txtDept.Visible = false;
            //cmdPop.Visible = false;
            CtrEquCataDropList1.ContralState = eOA_FlowControlState.eReadOnly;

            //lblName.Visible = true;
            lblCode.Visible = true;
            lblMulu.Visible = true;
            lblOpionBank.Visible = true;
            lbltDept.Visible = true;
        }
        #endregion 
    }
}