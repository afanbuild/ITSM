/****************************************************************************
 * 
 * description:�첽������
 * 
 * 
 * 
 * Create by:zhumingchun
 * Create Date:2011-05-18
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
using Epower.DevBase.BaseTools;

using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
    /// frmXmlHttp ��ժҪ˵����
	/// </summary>
    public partial class frmXmlHttp : System.Web.UI.Page
    {
        #region ҳ����� Page_Load
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["Type"] != null)   //jquery��Tabs����ҳ
            {
                long lngMessageID = long.Parse(Request["MessageID"].ToString());
                long lngUserID = long.Parse(Request["UserID"].ToString());
                long lngFlowID = long.Parse(Request["FlowID"].ToString());
                long lngNodeID = long.Parse(Request["NodeID"].ToString());
                string strReturn = string.Empty;

                if (Request["Type"].ToString() == "esrtTransmit") //��֪
                {
                    if (Epower.ITSM.SqlDAL.FlowDP.IsExistesrtTransmit(lngFlowID, lngUserID,lngNodeID))  //����
                    {
                        strReturn = "true";
                    }
                    else
                    {
                        strReturn = "false";
                    }
                }
                if (Request["Type"].ToString() == "esrtAssist") //Э��
                {
                    strReturn = Epower.ITSM.SqlDAL.FlowDP.IsExistesrtAssist(lngFlowID, lngUserID, lngNodeID).ToString();
                }
                Response.Clear();
                Response.Write(strReturn);
                Response.Flush();
                Response.End();
            }
        }
        #endregion
    }
}
