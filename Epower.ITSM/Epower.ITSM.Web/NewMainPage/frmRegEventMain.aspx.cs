/*******************************************************************
 *
 * Description
 * 
 * 我登记事项查询
 * Create By  :余向前
 * Create Date:2013-04-19

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
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.NewMainPage
{
    public partial class frmRegEventMain : BasePage
    {        
        protected long lngUserID
        {
            get
            {
                return CTools.ToInt64(HttpContext.Current.Session["UserID"].ToString());
            }
        }

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.TableVisible = false;
            this.Master.MainID = "1";
        }       
        #endregion

        #region Page_Load
        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtonEvent();
            cpRegEvent.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(BindData);            

            if (!IsPostBack)
            {
                BindData();
            }

            Session["FromUrl"] = "../NewMainPage/frmRegEventMain.aspx";
        }
        #endregion

        #region 绑定数据
        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindData()
        {
            int iRowCount = 0;
            string strWhere = " AppID in (1026,1062) and ReceiverID = " + lngUserID;
            string strOrder = " order by FlowID desc ";
            DataTable dt = CommonDP.GetRegEventData(strWhere, strOrder, this.cpRegEvent.PageSize, this.cpRegEvent.CurrentPage, ref iRowCount);

            dgRegEvent.DataSource = dt.DefaultView;
            dgRegEvent.DataBind();
            this.cpRegEvent.RecordCount = iRowCount;
            this.cpRegEvent.Bind();

        }
        #endregion

        #region 获取跳转链接
        /// <summary>
        /// 获取跳转链接
        /// </summary>
        /// <param name="lngNoticeID"></param>
        /// <returns></returns>
        protected string GetUrl(long lngFlowID, long lngAppID)
        {           
            //获取最新MessageID
            long lngMessageID = CommonDP.GetMessageIDForSelf(lngFlowID, lngUserID);
            
            //根据应用ID不同跳转到不同页面                        
            string sUrl = "";
            switch (lngAppID)
            { 
                case 1026:
                    sUrl = "javascript:window.open('../AppForms/CST_Issue_Base_Self.aspx?FlowModelID=0&IsSelfMode=true&MessageID=" + lngMessageID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
                    break;
                case 1062:
                    sUrl = "javascript:window.open('../Demand/frm_REQ_DEMAND_Self.aspx?FlowModelID=0&IsSelfMode=true&MessageID=" + lngMessageID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
                    break;
                default:
                    sUrl = "javascript:window.open('../Forms/frmIssueView.aspx?FlowID=" + lngFlowID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
                    break;
            }            

            return sUrl;
        }
        #endregion

        #region dgRegEvent_ItemDataBound
        /// <summary>
        /// dgRegEvent_ItemDataBound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgRegEvent_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                long lngFlowID = CTools.ToInt64(DataBinder.Eval(e.Item.DataItem, "Flowid").ToString());
                long lngAppID = CTools.ToInt64(DataBinder.Eval(e.Item.DataItem, "AppID").ToString());

                //获取最新MessageID
                long lngMessageID = CommonDP.GetMessageIDForSelf(lngFlowID, lngUserID);

                //根据应用ID不同跳转到不同页面                        
                string sUrl = "";
                switch (lngAppID)
                {
                    case 1026:
                        sUrl = "window.open('../AppForms/CST_Issue_Base_Self.aspx?FlowModelID=0&IsSelfMode=true&MessageID=" + lngMessageID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
                        break;
                    case 1062:
                        sUrl = "window.open('../Demand/frm_REQ_DEMAND_Self.aspx?FlowModelID=0&IsSelfMode=true&MessageID=" + lngMessageID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
                        break;
                    default:
                        sUrl = "window.open('../Forms/frmIssueView.aspx?FlowID=" + lngFlowID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
                        break;
                }

                e.Item.Attributes.Add("ondblclick", sUrl);
            }
        }
        #endregion
    }
}
