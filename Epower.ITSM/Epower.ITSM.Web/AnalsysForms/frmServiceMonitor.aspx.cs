using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.SqlDAL.Flash;

namespace Epower.ITSM.Web.AnalsysForms
{
    public partial class frmServiceMonitor : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtons();
            
            if (!IsPostBack)
            { 
                InitDrops();

                //加载报表
                LoadData();
            }
            
        }

        #region 加载模板内容
        /// <summary>
        /// 加载模板内容
        /// </summary>
        private void SetParentButtons()
        {
            
            this.Master.Master_Button_Query_Click +=new Global_BtnClick(Master_Master_Button_Query_Click);
            //this.Master.ShowQueryButton(true);
        }
        #endregion

        #region 绑定服务单位内容
        /// <summary>
        /// 绑定服务单位内容
        /// </summary>
        private void InitDrops()
        {
            Br_MastCustomerDP ee = new Br_MastCustomerDP();
            string sWhere = string.Empty;
            string sOrder = string.Empty;
            DataTable dt = ee.GetDataTable(sWhere, sOrder);
            ddltMastCustID.DataSource = dt;
            ddltMastCustID.DataTextField = "ShortName";
            ddltMastCustID.DataValueField = "ID";
            ddltMastCustID.DataBind();
            ddltMastCustID.Items.Insert(0, new ListItem("", ""));
        }
        #endregion

        #region 查询Master_Master_Button_Query_Click
        /// <summary>
        /// Master_Master_Button_Query_Click
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            LoadData();
        }
        #endregion

        #region 加载事件单图表
        /// <summary>
        /// 加载事件单图表
        /// </summary>
        private void LoadData()
        {
            string strWhere = string.Empty;

            if (ddltMastCustID.SelectedValue != "")
            {
                strWhere = ddltMastCustID.SelectedItem.Value;
            }

            #region 事件单
            DataTable dt = ZHServiceDP.GetSM(strWhere);
            IssueDiv.InnerHtml = FlashCS.PublicFlashUrl2D(dt, "Title", "counts", "事件单监控", "", "数量", "../FlashReoport/Flash/Column3D.swf", "80%", "248", true, 2, strWhere);
            #endregion

            #region 变更单
            dt = ChangeDealDP.GetSM(strWhere);
            ChangeDiv.InnerHtml = FlashCS.PublicFlashUrl2D(dt, "Title", "counts", "变更单监控", "", "数量", "../FlashReoport/Flash/Column3D.swf", "80%", "248", true, 2, strWhere);
            #endregion

            #region 问题单
            dt = ProblemDealDP.GetSM(strWhere);
            ProblemDiv.InnerHtml = FlashCS.PublicFlashUrl2D(dt, "Title", "counts", "问题单监控", "", "数量", "../FlashReoport/Flash/Column3D.swf", "80%", "248", true, 2, strWhere);
            #endregion

            #region 资产
            dt = Equ_DeskDP.GetSM(strWhere);
            EquDiv.InnerHtml = FlashCS.PublicFlashUrl2D(dt, "Title", "counts", "资产监控", "", "数量", "../FlashReoport/Flash/Column3D.swf", "80%", "248", true, 2, strWhere);
            #endregion

            #region 知识
            dt = Inf_InformationDP.GetSM(strWhere);
            InfDiv.InnerHtml = FlashCS.PublicFlashUrl2D(dt, "Title", "counts", "知识监控", "", "数量", "../FlashReoport/Flash/Column3D.swf", "80%", "248", true, 2, strWhere);
            #endregion

            #region 满意度
            dt = RiseDP.GetSM();
            FeedBackDiv.InnerHtml = FlashCS.PublicFlashUrl2D(dt, "Title", "counts", "本年度满意度", "", "数量", "../FlashReoport/Flash/Column3D.swf", "80%", "248", true, 2, strWhere);
            #endregion
        }
        #endregion

        #region ddltMastCustID_SelectedIndexChanged
        /// <summary>
        /// ddltMastCustID_SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddltMastCustID_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }
        #endregion
    }
}
