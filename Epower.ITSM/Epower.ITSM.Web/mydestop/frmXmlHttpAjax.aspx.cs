/****************************************************************************
 * 
 * description:异步处理类
 * 
 * 
 * 
 * Create by:yxq
 * Create Date:2011-08-15
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

namespace Epower.ITSM.Web.mydestop
{
    /// <summary>
    /// frmXmlHttp 的摘要说明。
    /// </summary>
    public partial class frmXmlHttpAjax : System.Web.UI.Page
    {
        #region 页面加载 Page_Load
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["Cust"] != null)    //检查客户资料
            {
                string sCust = Request["Cust"].ToString();
                CheckCust(sCust);
            }
            else if (Request["Equ"] != null)    //检查设备
            {
                string sEqu = Request["Equ"].ToString();
                CheckEqu(sEqu);
            }
            else if (Request["EquIDs"] != null)    //资产ID串
            {
                string strEqus = Request["EquIDs"].ToString();
                string strEquName = string.Empty;      //用来模糊查询的资产名称

                if (Request["EquName"] != null)
                {
                    strEquName = Request["EquName"].ToString();
                }
                CheckEqus(strEqus, strEquName);
            }
            
        }
        #endregion

        #region CheckEqus
        /// <summary>
        /// CheckEqus
        /// </summary>
        /// <param name="strEqus">不应该出现在选择列表中的资产ID串</param>
        /// /// <param name="strEquName">输入的用来模糊查询的资产名称</param>
        private void CheckEqus(string strEqus, string strEquName)
        {
            string strWhere = string.Empty;

            if (strEqus != "")
                strWhere = " and id not in (" + strEqus + ")";

            if (strEquName != "")
            {
                strWhere += " and Name like " + StringTool.SqlQ("%" + strEquName + "%");
            }

            DataTable dt = new DataTable();
            dt = new Equ_DeskDP().GetDataTable(strWhere, string.Empty);

            string sReturnValue = "-1";
            int icount = dt.Rows.Count;
            if (icount == 1)  //找到唯一
            {
                Json json = new Json(dt);
                sReturnValue = "{record:" + json.ToJson() + "}";

            }
            else if (icount > 1)  //找到多个
            {
                sReturnValue = "0";
            }
            Response.Clear();
            //返回是否有新增 0 无, 1 有
            Response.Write(sReturnValue);
            Response.Flush();
            Response.End();
        }
        #endregion

        #region 检查客户资料
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void CheckCust(string sCust)
        {
            string sSql = " and (ShortName like " + StringTool.SqlQ("%" + sCust + "%")
                       + " or FullName like " + StringTool.SqlQ("%" + sCust + "%")
                       + " or CustomCode like " + StringTool.SqlQ("%" + sCust + "%")
                       + " or LinkMan1 like " + StringTool.SqlQ("%" + sCust + "%")
                       + " or Email like " + StringTool.SqlQ("%" + sCust + "%")
                       + " or Tel1 like " + StringTool.SqlQ("%" + sCust + "%") + ")";
            Br_ECustomerDP mBr_ECustomerDP = new Br_ECustomerDP();
            DataTable dt = mBr_ECustomerDP.GetDataTableAjax(sSql, "");

            string sReturnValue = "-1";
            int icount = dt.Rows.Count;
            if (icount == 1)  //找到唯一
            {
                Json json = new Json(dt);
                sReturnValue = "{record:" + json.ToJson() + "}";

            }
            else if (icount > 1)  //找到多个
            {
                sReturnValue = "0";
            }
            Response.Clear();
            //返回是否有新增 0 无, 1 有
            Response.Write(sReturnValue);
            Response.Flush();
            Response.End();
        }
        #endregion

        #region 检查设备资料
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void CheckEqu(string sEqu)
        {
            string sSql = " and (Name like " + StringTool.SqlQ("%" + sEqu + "%")
                        +" or Code like"+StringTool.SqlQ("%" + sEqu + "%")+ ")";
            if (Request["Equ"] != null && Request["Equ"].ToString().Trim() != "")
                sSql += "And nvl(CostomName,'')  like " + StringTool.SqlQ("%" + Request["Equ"].ToString().Trim() + "%");

            if (Request["EquipmentCatalogID"] != null && decimal.Parse(Request["EquipmentCatalogID"].ToString() == "" ? "0" : Request["EquipmentCatalogID"].ToString()) > 0)
                sSql += " And listId=" + Request["EquipmentCatalogID"].ToString();
            //else
            //    sSql += " And 1=2 "; //没传资产分类ID 查询空

            Epower.ITSM.SqlDAL.Equ_DeskDP mEqu_DeskDP = new Epower.ITSM.SqlDAL.Equ_DeskDP();
            DataTable dt = mEqu_DeskDP.GetDataTableAjax(sSql, string.Empty);

            string sReturnValue = "-1";
            int icount = dt.Rows.Count;
            if (icount == 1)  //找到唯一
            {
                Json json = new Json(dt);
                sReturnValue = "{record:" + json.ToJson() + "}";
            }
            else if (icount > 1)  //找到多个
            {
                sReturnValue = "0";
            }
            Response.Clear();
            //返回是否有新增 0 无, 1 有
            Response.Write(sReturnValue);
            Response.Flush();
            Response.End();
        }
        #endregion
    }
}
