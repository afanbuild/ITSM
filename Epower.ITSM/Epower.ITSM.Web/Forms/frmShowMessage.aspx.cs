/*******************************************************************
 * 版权所有：
 * Description：短信息
 * 
 * 
 * Create By  ：zhumingchun
 * Create Date：2007-10-31
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
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web
{
    /// <summary>
    /// 
    /// </summary>
    public partial class frmPaneList : BasePage
    {
        #region 设置父窗体按钮事件SetParentButtonEvent
        /// <summary>
        /// 设置父窗体按钮事件
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.TableVisible = false;
        }
        #endregion 

        #region 页面加载 Page_Load
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtonEvent();
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
            long lngUserID = long.Parse(Session["userid"].ToString());
            int intNum = 3;
            string sWhere = string.Empty;
            DataTable dt = SMSDp.GetSMS(lngUserID, intNum, sWhere,0,string.Empty,string.Empty);
            DataList1.DataSource = dt.DefaultView;
            DataList1.DataBind();


            this.lblcount.Text = SMSDp.GetSMSReadCount(lngUserID).ToString(); ;
        }
        #endregion 

        #region 删除消息，已读消息
        /// <summary>
        /// 删除消息,已读消息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
        {
            //删除消息
            if (e.CommandName == "delete")
            {
                Label lblID = (Label)e.Item.FindControl("lblID");
                long lngID = long.Parse(lblID.Text.Trim());
                SMSDp.DeleteSMS(lngID);
                Bind();
            }
            else if (e.CommandName == "read")
            {
                Label lblID = (Label)e.Item.FindControl("lblID");
                long lngID = long.Parse(lblID.Text.Trim());
                SMSDp.ReadSMS(lngID);
                Bind();
            }
            
        }
        #endregion 
    }
}
