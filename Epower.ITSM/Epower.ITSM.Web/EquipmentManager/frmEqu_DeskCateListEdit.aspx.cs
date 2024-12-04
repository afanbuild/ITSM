/*******************************************************************
 *
 * Description 资产目录编辑界面
 * 
 * 
 * Create By  :ly
 * Create Date:2011年8月15日
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

using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frmEqu_DeskCateListEdit : BasePage
    {
        #region 属性

        /// <summary>
        /// 设备分类ID
        /// </summary>
        protected string CatalogID
        {
            get
            {
                if (Request["subjectid"] != null && Request["subjectid"] != "")
                    return Request["subjectid"].ToString();
                else return "-1";
            }
        }
        #endregion

        #region SetParentButtonEvent
        /// <summary>
        /// 按钮事件
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.CEqu_CateLists;
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
        }
        #endregion

        #region Master_Master_Button_New_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("frmEqu_DeskCateListEdit.aspx?subjectid=" + CatalogID);
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
                Equ_CateListsDP ee = new Equ_CateListsDP();
                long lngID = long.Parse(this.Master.MainID.Trim());
                if (ee.CheckCateListChild(lngID, hidCataLogId.Value))
                {
                    ee.DeleteRecorded(lngID);
                    Master_Master_Button_GoHistory_Click();
                }
                else
                {
                    Master.IsSaveSuccess = false;
                    Response.Write("<script>alert('资产目录下有资产，不能删除！');</script>");
                }
            }
        }
        #endregion

        #region Master_Master_Button_GoHistory_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_GoHistory_Click()
        {
            Response.Redirect("frmEqu_DeskCateList.aspx?subjectid=" + CatalogID);
        }
        #endregion

        #region Master_Master_Button_Save_Click
        /// <summary>
        /// 保存
        /// </summary>
        void Master_Master_Button_Save_Click()
        {
            if (string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                int flag = 0;
                Equ_CateListsDP ee = new Equ_CateListsDP();
                InitObject(ee);
                ee.GetReCorded(ee.ListName, ee.CatalogID, ref flag);
                if (flag == 0)
                {
                    ee.InsertRecorded(ee);
                    this.Master.MainID = ee.ID.ToString();
                }
                else
                {
                    Master.IsSaveSuccess = false;
                    Response.Write("<script>alert('资产目录不能重复！');</script>");
                }
            }
            else
            {
                Equ_CateListsDP ee = new Equ_CateListsDP();
                ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                InitObject(ee);
                ee.UpdateRecorded(ee);
                this.Master.MainID = ee.ID.ToString();
            }
        }
        #endregion

        #region Page_Load
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            SetParentButtonEvent();
            if (!IsPostBack)
            {
                LoadData();
            }
        }
        #endregion

        #region LoadData
        /// <summary>
        /// 读取数据
        /// </summary>
        private void LoadData()
        {
            if (!string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                Equ_CateListsDP ee = new Equ_CateListsDP();
                if (CatalogID != "1" && CatalogID != "-1")
                {
                    ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()), CatalogID);
                }
                else
                {
                    ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                }

                CtrFlowListName.Value = ee.ListName.ToString();
                hidCataLogId.Value = ee.CatalogID.ToString();
            }
        }
        #endregion

        #region InitObject
        /// <summary>
        /// 数据初始化
        /// </summary>
        /// <param name="ee"></param>
        private void InitObject(Equ_CateListsDP ee)
        {
            ee.CatalogID = decimal.Parse(CatalogID);
            ee.CatalogName = Equ_SubjectDP.GetSubjectName(long.Parse(ee.CatalogID.ToString()));
            ee.ListName = CtrFlowListName.Value.Trim();
        }
        #endregion


    }
}
