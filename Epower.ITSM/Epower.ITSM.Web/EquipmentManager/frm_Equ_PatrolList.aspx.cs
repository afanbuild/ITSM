/*******************************************************************
 * 版权所有：
 * Description：设备/产品巡检维保（记录）
 * 
 * 
 * Create By  ：zhumingchun
 * Create Date：2007-10-20
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

using Epower.ITSM.Base;
using EpowerGlobal;
using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frm_Equ_PatrolList : BasePage
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        protected string EquID
        {
            get { if (Request["EquID"] != null) return Request["EquID"].ToString(); else return "0"; }
        }

        #region 设置父窗体按钮事件SetParentButtonEvent
        /// <summary>
        /// 设置父窗体按钮事件
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_GoHistory_Click += new Global_BtnClick(Master_Master_Button_GoHistory_Click);
            this.Master.ShowBackUrlButton(true);
        }
        #endregion 

        #region Master_Master_Button_GoHistory_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_GoHistory_Click()
        {
            Response.Write("<script>top.close();</script>");
        }
        #endregion

        #region 页面初始化 Page_Load
        /// <summary>
        /// 页面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtonEvent();

            cpPatrol.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(Bind);
            if (!IsPostBack)
            {
                Bind();
            }
        }
        #endregion 
         
        #region 绑定数据 Bind
        /// <summary>
        /// 绑定数据
        /// </summary>
        private void Bind()
        {
            #region bind
            int iRowCount = 0;
            string sWhere = "";
            if (EquID != "0")
            {
                sWhere += " And ID in (select PatrolID from Equ_PatrolItemData where 1=1 and EquID=" + EquID + ")";
            }
            DataTable dt = Equ_PatrolDataDP.GetFieldsTable(long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()), long.Parse(Session["UserOrgID"].ToString()),
                            (Epower.DevBase.Organization.SqlDAL.RightEntity)((Hashtable)Session["UserAllRights"])[Epower.ITSM.Base.Constant.EquPatrolQuery], sWhere, this.cpPatrol.PageSize, this.cpPatrol.CurrentPage, ref iRowCount);
            grd.DataSource = dt.DefaultView;
            grd.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");

            grd.DataBind();

            this.cpPatrol.RecordCount = iRowCount;
            this.cpPatrol.Bind();
            #endregion
        }
        #endregion 

        #region 显示页面地址 GetUrl
        /// <summary>
        /// 显示页面地址
        /// </summary>
        /// <param name="lngNoticeID"></param>
        /// <returns></returns>
        protected string GetUrl(decimal lngFlowID)
        {
            string sUrl = "";
            sUrl = "javascript:window.open('../Forms/frmIssueView.aspx?NewWin=true&FlowID=" + lngFlowID.ToString() + "','','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
            return sUrl;
        }
        #endregion 

       

        #region 排序 grd_ItemCreated
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                int j = 0;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i < e.Item.Cells.Count-1)
                    {
                        j = i - 0;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
        #endregion 
    }
}
