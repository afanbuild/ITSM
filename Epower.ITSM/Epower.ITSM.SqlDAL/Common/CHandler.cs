/****************************************************************************
 * 
 * description:Ajax请求操作类 
 * 
 * 
 * 
 * Create by:yxq
 * Create Date:2011-09-07
 * *************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Epower.DevBase.Organization.SqlDAL;
using System.Data.OracleClient;
using System.Data;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.SqlDAL
{
    public class CHandler
    {
        /// <summary>
        /// 选择方法
        /// </summary>
        /// <param name="context"></param>
        public static void ChooseMethod(HttpContext context)
        {
            string act = MClass.getString(context, "act", "");
            switch (act)
            { 
                case "catalogimg":
                    GetCatalogImg(context);  //获取常用类别图标
                    break;
                case "easerviceimg":
                    GetEAServiceImg(context);  //获取服务项图标
                    break;
                case "custdetail":
                    GetCustDetail(context);
                    break;
                case "EquRel":  //获取是否能修改资产关联
                    CanUpateEquRel(context);
                    break;
                case "OANewTypeMsg":
                    GetOANewTypeMsg(context); //获取某个类别下的信息公告
                    break;
                case "ServiceLevel":
                    GetServiceLevel(context);
                    break;
                case "GetFlowModelByAppID":
                    GetFlowModelByAppID(context);
                    break;
            }
        }

        /// <summary>
        /// 获取某个类别下的信息公告
        /// </summary>
        /// <param name="context"></param>
        private static void GetOANewTypeMsg(HttpContext context)
        {
            int TypeID = MClass.getInt(context, "TypeID", 0);

            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT * FROM oa_news WHERE TypeID =" + TypeID.ToString();
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                

                string result = "false";
                if (dt != null && dt.Rows.Count > 0)
                    result = "true";

                string json = "{record:" + result + "}";
                HttpContext.Current.Response.Write(json);
            }
            finally { ConfigTool.CloseConnection(cn); }
        }

        /// <summary>
        /// 获取是否能修改资产关联
        /// </summary>
        /// <param name="context"></param>
        private static void CanUpateEquRel(HttpContext context)
        {
            long EquID = MClass.getLong(context, "EquID", 0);

            Equ_DeskDP ee = new Equ_DeskDP();
            ee = ee.GetReCorded(EquID);

            string result = "false";
            string sEquList = HttpContext.Current.Session["EquLimitList"].ToString() + ",";
            if (sEquList.Contains(ee.CatalogID.ToString() + ",") || DeptDP.IsExistUserByDept(HttpContext.Current.Session["UserID"].ToString(), ee.partBranchId.ToString() == "" ? "0" : ee.partBranchId.ToString()))
                result = "true";

            string json = "{record:" + result + "}";
            HttpContext.Current.Response.Write(json);
        }


        /// <summary>
        /// 获取常用类别图标
        /// </summary>
        /// <param name="context"></param>
        private static void GetCatalogImg(HttpContext context)
        {
            long catalogid = MClass.getLong(context, "catalogid", 0);

            string json = CatalogEntity.GetCatalogJson(catalogid);
            HttpContext.Current.Response.Write(json);
        }

        /// <summary>
        /// 获取服务项图标
        /// </summary>
        /// <param name="context"></param>
        private static void GetEAServiceImg(HttpContext context)
        {
            long lngid = MClass.getLong(context, "lngid", 0);

            string json = EA_ServicesTemplateDP.GetJson(lngid);
            HttpContext.Current.Response.Write(json);
        }
        /// <summary>
        /// 获取客户信息
        /// </summary>
        /// <param name="context"></param>
        private static void GetCustDetail(HttpContext context)
        {
            long CustID = MClass.getLong(context, "CustID", 0);
            string sWhere = " And ID=" + CustID;
            string sOrder = "";

            string json = Br_ECustomerDP.GetJson(sWhere, sOrder);
            HttpContext.Current.Response.Write(json);
        }

        /// 获取服务级别
        /// </summary>
        /// <param name="context"></param>
        private static void GetServiceLevel(HttpContext context)
        {
            long lngCustID = MClass.getLong(context, "CustID");
            long lngEquID = MClass.getLong(context, "EquID");
            long lngTypeID = MClass.getLong(context, "TypeID");
            long lngKindID = MClass.getLong(context, "KindID");
            long lngEffID = MClass.getLong(context, "EffID");
            long lngInsID = MClass.getLong(context, "InsID");


            string jsonstr = "";

            Cst_ServiceLevelDP ee = new Cst_ServiceLevelDP();
            DataTable dt = ee.GetDataTableForSelect(lngCustID, lngEquID, lngTypeID, lngKindID, lngEffID, lngInsID);

            if (dt != null)
            {
                if (dt.Rows.Count > 1)
                {
                    jsonstr = "0";
                }
                else if (dt.Rows.Count == 1)
                {

                    Cst_SLGuidDP ec = new Cst_SLGuidDP();
                    DataTable dt2 = ec.GetDataByLevelIDCache(long.Parse(dt.Rows[0]["id"].ToString()));

                    string sLimit = "";
                    foreach (DataRow row in dt2.Rows)
                    {
                        sLimit += row["guidname"].ToString().Trim() + ":" + row["TimeLimit"].ToString().Trim() + GetTimeUnit(row["TimeUnit"].ToString().Trim()) + ",";
                    }
                    if (sLimit.EndsWith(","))
                        sLimit = sLimit.Substring(0, sLimit.Length - 1);

                    dt.Columns.Add("limitstr");
                    dt.Rows[0]["limitstr"] = sLimit;

                    Json json = new Json(dt);
                    jsonstr = "{record:" + json.ToJson() + "}";
                }
            }

            HttpContext.Current.Response.Write(jsonstr);

        }

        private static string GetTimeUnit(string code)
        {
            string ret = "单位";
            if (code == "0")
                ret = "分钟";
            if (code == "1")
                ret = "小时";
            if (code == "2")
                ret = "天";
            if (code == "3")
                ret = "分钟";
            if (code == "4")
                ret = "小时";

            return ret;
        }

        #region 根据应用ID获取这个应用下所有的OFlowmodel  余向前 2013-03-20
        /// <summary>
        /// 根据应用ID获取这个应用下所有的OFlowmodel
        /// </summary>
        /// <param name="context"></param>
        private static void GetFlowModelByAppID(HttpContext context)
        {
            long AppID = MClass.getLong(context, "AppID", 0);
            string strSQL = "select * from es_flowmodel where status=1 and AppID=" + AppID.ToString();
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            Json json = new Json(dt);
            string jsonstr = "{record:" + json.ToJson() + "}";
            HttpContext.Current.Response.Write(jsonstr);
        }
        #endregion
    }
}
