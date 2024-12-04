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
using Epower.ITSM.SqlDAL.ES_TBLCS;
using System.Collections.Generic;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frmEqu_ImpactAnalysis : BasePage
    {
        string sHistorySchema = "";   //历史版本的schema 仅用于历史版本展示


        #region 属性


        /// <summary>
        /// 是否来源于图形展示详细

        /// </summary>
        protected bool ShowDetail
        {
            get { if (Request["ShowDetail"] != null) return true; else return false; }
        }


        /// <summary>
        /// 是否来源于选择
        /// </summary>
        protected bool IsSelect
        {
            get { if (Request["IsSelect"] != null) return true; else return false; }
        }

        /// <summary>
        /// 变更单上的FlowID
        /// </summary>
        protected long ChangeFlowID
        {
            get { if (Request["ChangeBillFlowID"] != null) return long.Parse(Request["ChangeBillFlowID"].ToString()); else return 0; }
        }

        /// <summary>
        /// 是否打开新窗口,新窗口返回时关闭
        /// </summary>
        protected bool IsNewWin
        {
            get { if (Request["newWin"] != null) return true; else return false; }
        }

        /// <summary>
        /// 是否来源于查看相同配置

        /// </summary>
        protected bool IsSameSchemaItem
        {
            get { if (Request["IsSameSchemaItem"] != null) return true; else return false; }
        }

        /// <summary>
        /// 资产ID
        /// </summary>
        protected string EquID
        {
            get
            {
                if (this.Master.MainID.Trim() != string.Empty)
                {
                    return this.Master.MainID.Trim();
                }
                else
                {
                    return "0";
                }
            }

        }

        /// <summary>
        /// 是否从变更单过来
        /// </summary>
        protected bool IsChange
        {
            get
            {
                if (Request["IsChange"] != null)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// 是否从变更单过来进入编辑状态

        /// </summary>
        protected bool IsChangeEdit
        {
            get
            {
                if (Request["IsChEdit"] != null)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// 是否从关联资产过来

        /// </summary>
        protected bool IsRel
        {
            get
            {
                if (Request["IsRel"] != null)
                    return true;
                else
                    return false;
            }
        }
        #endregion

        #region SetParentButtonEvent
        /// <summary>
        /// 按钮事件
        /// </summary>
        protected void SetParentButtonEvent()
        {
            if (this.Request.QueryString["EquId"] != null)
            {
                this.Master.MainID = this.Request.QueryString["EquId"].ToString();
            }
            this.Master.Master_Button_GoHistory_Click += new Global_BtnClick(Master_Master_Button_GoHistory_Click);
            this.Master.ShowBackUrlButton(true);
        }
        #endregion

        #region Master_Master_Button_GoHistory_Click
        /// <summary>
        /// 返回
        /// </summary>   
        void Master_Master_Button_GoHistory_Click()
        {
            Response.Write("<script>top.close();</script>");
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
                LoadData();//加载数据
            }
        }


        #endregion

        #region LoadData
        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadData()
        {
            if (!string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                long lngID = long.Parse(this.Master.MainID.Trim());
                Equ_DeskDP ee = new Equ_DeskDP();

                //当前版本情况下

                if (this.IsChange == true && this.ChangeFlowID != 0)
                {
                    ee = ee.GetReCordedForChange(lngID, this.ChangeFlowID);

                }
                else
                {
                    ee = ee.GetReCorded(lngID);
                }
                lblEquDeskName.Text = ee.Name;//资产名称
                lblListName.Text = ee.ListName;//资产目录
                lblEquDeskCode.Text = ee.Code;//资产编号
                lblCustName.Text = ee.CostomName;//客户名称
                lblPartBank.Text = ee.partBankName;//维护单位
                lblPartBranch.Text = ee.partBranchName;//维护单位
            }
        }
        #endregion
    }
}