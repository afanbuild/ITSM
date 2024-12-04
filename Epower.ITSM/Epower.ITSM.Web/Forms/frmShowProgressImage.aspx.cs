/*******************************************************************
•	 * 版权所有：深圳市非凡信息技术有限公司
•	 * 描述：查看进度条图片
•	
•	 * 
•	 * 
•	 * 创建人：余向前
•	 * 创建日期：2013-04-26
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
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.Forms
{
    public partial class frmShowProgressImage : BasePage
    {
        #region SetParentButtonEvent
        /// <summary>
        /// 设置父窗体按钮事件
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.TableVisible = false;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                long lngAppID = CTools.ToInt64(Request["AppID"].ToString());
                long lngOFlowModelID = CTools.ToInt64(Request["OFlowModelID"].ToString());
                long lngNodeModelID = CTools.ToInt64(Request["NodeModelID"].ToString());

                BR_ProgressBarDP ee = new BR_ProgressBarDP();
                ee = ee.GetRecorded(lngAppID, lngOFlowModelID, lngNodeModelID);
                showImg.Src = ee.ImgURL;
            }
        }
    }
}
