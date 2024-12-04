/****************************************************************************
 * 
 * description：选择客户的数据处理层
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-08-18
 * *************************************************************************/
using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;
using EpowerGlobal;
using System.Xml;
using System.IO;


namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// 
    /// </summary>
    public class DRMUserDP
    {
        /// <summary>
        /// 
        /// 数据结构：client_id,client_name,client_address,client_contact,client_phone
        /// </summary>
        /// <param name="strXmlcond"></param>
        /// <param name="lngUserID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngOrgID"></param>
        /// <param name="re"></param>
        /// <param name="sServiceCustom"></param>
        /// <returns></returns>
        public DataTable GetDRMUser(string strXmlcond, long lngUserID, long lngDeptID, long lngOrgID, RightEntity re
            , string sServiceCustom, int pagesize, int pageindex, ref int rowcount)
        {
            DataTable dt;
            switch (sServiceCustom)
            {
                case "1":  //ITSM
                    dt = GetCustomsITSM(strXmlcond, lngUserID, lngDeptID, lngOrgID, re, pagesize, pageindex, ref rowcount);
                    break;
                default:  //ITSM
                    dt = GetCustomsITSM(strXmlcond, lngUserID, lngDeptID, lngOrgID, re, pagesize, pageindex, ref rowcount);
                    break;
            }
            return dt;
        }

        #region GetCustomsITSM
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strXmlcond"></param>
        /// <param name="lngUserID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngOrgID"></param>
        /// <param name="re"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="rowcount"></param>
        /// <returns></returns>
        private DataTable GetCustomsITSM(string strXmlcond, long lngUserID, long lngDeptID, long lngOrgID, RightEntity re
            , int pagesize, int pageindex, ref int rowcount)
        {
            string sSql = string.Empty;
            string strTmp = string.Empty;
            string sWhere = " 1=1 ";

            string strMastCustID = string.Empty;
            string strCustomerType = string.Empty;
            string strclient_name = string.Empty;
            string strclient_address = string.Empty;
            string strclient_contact = string.Empty;
            string strclient_phone = string.Empty;
            string strCustomCode = string.Empty;

            #region 获取查询参数的值
            XmlTextReader tr = new XmlTextReader(new StringReader(strXmlcond));
            while (tr.Read())
            {
                if (tr.Name == "Field" && tr.NodeType == XmlNodeType.Element)
                {
                    strTmp = tr.GetAttribute("Value").Trim();
                    switch (tr.GetAttribute("FieldName").ToLower())
                    {
                        case "client_name":
                            strclient_name = strTmp;
                            break;
                        case "client_address":
                            strclient_address = strTmp;
                            break;
                        case "client_contact":
                            strclient_contact = strTmp;
                            break;
                        case "client_phone":
                            strclient_phone = strTmp;
                            break;
                        case "customcode":
                            strCustomCode = strTmp;
                            break;
                        case "mastcustid":
                            strMastCustID = strTmp;
                            break;
                        case "customertype":
                            strCustomerType = strTmp;
                            break;
                        default:
                            break;
                    }
                }
            }
            tr.Close();
            #endregion

            if (strclient_name != string.Empty)
                sWhere += " and a.client_name like " + StringTool.SqlQ("%" + strclient_name.Replace("'", "''") + "%");

            if (strclient_address != string.Empty)
                sWhere += " and a.client_address like " + StringTool.SqlQ("%" + strclient_address.Replace("'", "''") + "%");

            if (strclient_contact != string.Empty)
                sWhere += " and a.client_contact like " + StringTool.SqlQ("%" + strclient_contact.Replace("'", "''") + "%");

            if (strclient_phone != string.Empty)
                sWhere += " and a.client_phone like " + StringTool.SqlQ("%" + strclient_phone.Replace("'", "''") + "%");
            if (strCustomCode != string.Empty)
                sWhere += " and a.CustomCode like " + StringTool.SqlQ("%" + strCustomCode.Replace("'", "''") + "%");
            if (strMastCustID != string.Empty)
                sWhere += " and a.MastCustID=" + strMastCustID;
            if (strCustomerType != string.Empty)
                sWhere += " and a.CustomerType=" + strCustomerType;

            if (CommonDP.GetConfigValue("Other", "DataLimit") != null && CommonDP.GetConfigValue("Other", "DataLimit") == "1")  //是否限制
            {
                if (System.Web.HttpContext.Current.Session["MastLimitList"] != null)
                {
                    sWhere = sWhere + " and a.MastCustID in (" + System.Web.HttpContext.Current.Session["MastLimitList"].ToString() + ")";
                }
            }

            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                //WriteState
                //writelog(sWhere + "-" + pagesize.ToString() + "-" + pageindex.ToString());

                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "V_Br_ECustomer a", "*", " order by client_id desc ", pagesize, pageindex, sWhere, ref rowcount);
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }
        #endregion

        #region 生成服务单号
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sNewBuildCode"></param>
        /// <param name="sServiceNo"></param>
        public static void GetInvoiceNo(OracleTransaction trans, string sNewBuildCode, ref string sServiceNo)
        {
            string sSql = string.Empty;
            sSql = @"select to_number(nvl(max(to_number(nvl(ServiceNo,0))),0))+1 as ServiceNo from cst_issues  where buildCode=" + StringTool.SqlQ(sNewBuildCode);
            object pobject = OracleDbHelper.ExecuteScalar(trans, CommandType.Text, sSql);
            if (pobject != null)
                sServiceNo = pobject.ToString();
        }
        #endregion
    }
}
