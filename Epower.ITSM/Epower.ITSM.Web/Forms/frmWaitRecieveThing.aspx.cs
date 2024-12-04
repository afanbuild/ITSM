using System;
using System.Collections;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using EpowerCom;

namespace Epower.ITSM.Web.Forms
{
    public partial class frmWaitRecieveThing : BasePage
    {
        protected string sDate = "";
        protected string sUserID = "0";
        protected string Issue = "0";//事件        
        protected string Change = "0";//变更
        protected string Byts = "0";//问题
        protected string Release = "0";//发布
        protected string Other = "0"; //其它       

        #region 界面转换时，用于传递的随机数
        /// <summary>
        /// 界面转换时，用于传递的随机数
        /// </summary>
        protected string sparams
        {
            get { return new Random().Next().ToString(); }
        }
        #endregion

        #region 页面加载
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                sUserID = Session["UserID"].ToString();
            }
            Issue = ZHServiceDP.getFlowWorkNowCount(long.Parse(sUserID), 1026, 0).ToString();//事件            
            Change = ZHServiceDP.getFlowWorkNowCount(long.Parse(sUserID), 420, 0).ToString();//变更
            Byts = ZHServiceDP.getFlowWorkNowCount(long.Parse(sUserID), 210, 0).ToString();//问题
            Release = ZHServiceDP.getFlowWorkNowCount(long.Parse(sUserID), 1028, 0).ToString();//问题
            Other = ZHServiceDP.getFlowWorkNowCount(long.Parse(sUserID), 0, 0).ToString();//问题
        }
        #endregion
    }
}
