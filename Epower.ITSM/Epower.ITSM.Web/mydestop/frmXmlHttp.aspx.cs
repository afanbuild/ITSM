/****************************************************************************
 * 
 * description:异步处理类
 * 
 * 
 * 
 * Create by:zhumingchun
 * Create Date:2007-11-26
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
    public partial class frmXmlHttp : System.Web.UI.Page
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

            if (Request["MYTABLE"] != null)   //自定义桌面
            {
                string sParams = Request["MYTABLE"].ToString();
                WriteDeskDefineToProfile(sParams);
            }
            if (Request["MYTABLEEXPAND"] != null)   //自定义桌面折叠状态
            {
                string sParams = Request["MYTABLEEXPAND"].ToString();
                WriteDeskDefineExpandToProfile(sParams);
            }
            if (Request["Tabs"] != null)   //jquery的Tabs属性页
            {
                string sUserID = "TabsFlowFormsTab";
                if (Request["Tabs"] != "-1")
                {
                    Response.Cookies[sUserID].Value = Request["Tabs"].ToString();
                    Response.Cookies[sUserID].Path = "/";
                    Response.Cookies[sUserID].Expires = DateTime.Now.AddMonths(1);
                }
                else
                {
                    HttpCookie cookie = Request.Cookies[sUserID];
                    string svalue = "0";
                    if (cookie != null)
                    {
                        svalue = cookie.Value.ToString();
                    }
                    Response.Clear();
                    Response.Write(svalue);
                    Response.Flush();
                    Response.End();
                }
            }

            if (Request["PersonOp"] != null)   //jquery的自定义快速处理意见
            {
                string svalue = string.Empty;
                Response.Clear();
                Response.Write(svalue);
                Response.Flush();
                Response.End();
            }

            if (Request["EquId"] != null)//资产锁定
            {
                string equId = Request["EquId"].ToString();
                string changeId = Request["ChangeId"].ToString();
                AssetLock(equId, changeId);
            }
            if (Request["AssetLockId"] != null)//资产是否锁定
            {
                string equId = Request["AssetLockId"].ToString();
                string changeId = Request["ChangeId"].ToString();
                AssetIsLock(equId, changeId);
            }

            if (Request["Owner"] != null && Request["AppID"] != null)//模板性质
            {
                GetFlowModelList(CTools.ToInt64(Request["AppID"].ToString()), Request["Owner"].ToString());
            }
        }
        #endregion

        #region 自定义桌面 WriteCookie
        /// <summary>
        /// 
        /// </summary>
        private void WriteDeskDefineToProfile(string sParams)
        {
            UserDP.UpdateUserDeskDefine((long)Session["UserID"], sParams);
        }
        #endregion

        #region 自定义桌面折叠状态 WriteCookie
        /// <summary>
        /// 
        /// </summary>
        private void WriteDeskDefineExpandToProfile(string sParams)
        {
            string sUserID = Constant.UserDeskTopCookiesKey + Session["UserID"].ToString() + "_Expand";
            Response.Cookies[sUserID].Value = sParams;
            Response.Cookies[sUserID].Path = "/";
            //Response.Cookies["FeiMainPage"].Domain = "oa.cpm.gd.cn";
            Response.Cookies[sUserID].Expires = DateTime.Now.AddYears(1);
            //UserDP.UpdateUserDeskDefine((long)Session["UserID"], sParams);
        }
        #endregion

        #region 检查客户资料
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void CheckCust(string sCust)
        {
            string sSql = " and (E.ShortName like " + StringTool.SqlQ("%" + sCust + "%")
                       + " or E.FullName like " + StringTool.SqlQ("%" + sCust + "%")
                       + " or E.CustomCode like " + StringTool.SqlQ("%" + sCust + "%")
                       + " or E.LinkMan1 like " + StringTool.SqlQ("%" + sCust + "%")
                       + " or E.Tel1 like " + StringTool.SqlQ("%" + sCust + "%") + ")";
            Br_ECustomerDP mBr_ECustomerDP = new Br_ECustomerDP();
            DataTable dt = mBr_ECustomerDP.GetCustomerServic(sSql, string.Empty);

            string sReturnValue = "-1";
            int icount = dt.Rows.Count;
            if (icount == 1)  //找到唯一
            {
                sReturnValue = dt.Rows[0]["Id"].ToString() + "," + dt.Rows[0]["ShortName"].ToString() + ","
                    + dt.Rows[0]["Address"].ToString() + "," + dt.Rows[0]["LinkMan1"].ToString() + ","
                    + dt.Rows[0]["Tel1"].ToString();

                sReturnValue += "," + dt.Rows[0]["CustomCode"].ToString() + "," + dt.Rows[0]["Email"].ToString() + "," + dt.Rows[0]["MastCust"].ToString();

                //根据客户ID取得设备信息
                Epower.ITSM.SqlDAL.Equ_DeskDP ee = new Epower.ITSM.SqlDAL.Equ_DeskDP();
                ee = ee.GetEquByCustID(long.Parse(dt.Rows[0]["Id"].ToString()));
                sReturnValue += "," + ee.ID.ToString() + "," + ee.Name.ToString() + ","
                    + ee.Positions + "," + ee.Code.ToString() + ","
                    + ee.SerialNumber.ToString() + "," + ee.Breed.Trim() + "," + ee.Model.Trim() + "," + ee.ListID.ToString().Trim() + "," + ee.ListName.Trim();

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
                        + " or Code like " + StringTool.SqlQ("%" + sEqu + "%") + ")";
            if (Request["EquCust"] != null)
                sSql += "And nvl(CostomName,'')  like " + StringTool.SqlQ("%" + Request["EquCust"].ToString().Trim() + "%");
            Epower.ITSM.SqlDAL.Equ_DeskDP mEqu_DeskDP = new Epower.ITSM.SqlDAL.Equ_DeskDP();
            DataTable dt = mEqu_DeskDP.GetDataTable(sSql, string.Empty);

            string sReturnValue = "-1";
            int icount = dt.Rows.Count;
            if (icount == 1)  //找到唯一
            {
                sReturnValue = dt.Rows[0]["Id"].ToString() + "," + dt.Rows[0]["Name"].ToString() + "," + dt.Rows[0]["Positions"].ToString() +
                    "," + dt.Rows[0]["Code"].ToString() + "," + dt.Rows[0]["SerialNumber"].ToString() + "," + dt.Rows[0]["Breed"].ToString() + "," + dt.Rows[0]["Model"].ToString();
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

        #region 资产锁定

        /// <summary>
        /// 资产锁定
        /// </summary>
        /// <param name="strEquId"></param>
        private void AssetLock(string strEquId, string strChangeId)
        {
            #region 定义变量

            long equId = (strEquId == string.Empty ? 0 : long.Parse(strEquId));//资产ID
            long changeId = (strChangeId == string.Empty ? 0 : long.Parse(strChangeId));//变更ID
            string sReturnValue = string.Empty;//返回值
            Equ_DeskDP ed = new Equ_DeskDP();//资产

            #endregion

            #region 进行锁定

            sReturnValue = ed.AssetLock(equId, changeId);//进行锁定

            #endregion

            #region 输出

            Response.Clear();
            //返回是否有新增 0 无, 1 有
            Response.Write(sReturnValue);
            Response.Flush();
            Response.End();

            #endregion
        }

        #endregion

        #region 资产是否锁定

        /// <summary>
        /// 资产是否锁定
        /// </summary>
        /// <param name="strEquId"></param>
        /// <param name="strChangeId"></param>
        private void AssetIsLock(string strEquId, string strChangeId)
        {
            #region 定义变量

            long equId = (strEquId == string.Empty ? 0 : long.Parse(strEquId));//资产ID
            long changeId = (strChangeId == string.Empty ? 0 : long.Parse(strChangeId));//变更ID
            string sReturnValue = string.Empty;//返回值
            Equ_DeskDP ed = new Equ_DeskDP();//资产

            #endregion

            #region 进行锁定

            sReturnValue = ed.AssetIsLock(equId, changeId);//进行锁定

            #endregion

            #region 输出

            Response.Clear();
            //返回是否有新增 0 无, 1 有
            Response.Write(sReturnValue);
            Response.Flush();
            Response.End();

            #endregion
        }

        #endregion

        #region 获取流程模型

        /// <summary>
        /// 获取流程模型
        /// </summary>
        /// <param name="strOwner"></param>
        private void GetFlowModelList(long lngAppID, string strOwner)
        {
            long lngOwner = long.Parse(strOwner);
            DataTable dt = AppFieldConfigDP.GetFlowModelList(lngAppID, (long)Session["UserID"], lngOwner);
            string sReturnValue = string.Empty;
            Json json = new Json(dt);
            sReturnValue = "{record:" + json.ToJson() + "}";
            Response.Clear();
            //返回是否有新增 0 无, 1 有
            Response.Write(sReturnValue);
            Response.Flush();
            Response.End();
        }

        #endregion
    }
}
