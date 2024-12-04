using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// 
    /// </summary>
    public class ECustomerDP
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public static DataTable GetECustomers(string sWhere)
        {
            string sSql = "Select * From Br_ECustomer Where 1=1 and nvl(Deleted,0)=" + ((int)eRecord_Status.eNormal).ToString() + sWhere;
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sSql);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sLoginName"></param>
        /// <param name="sPassword"></param>
        /// <param name="sCustName"></param>
        /// <returns></returns>
        public static string GetUserName(string sLoginName,string sPassword,ref string sCustName)
        {
            string sSql = "Select b.LoginName,a.ShortName From Br_ECustomer a,ts_user b Where nvl(a.UserID,0)=b.UserID  and nvl(a.Deleted,0)=" + ((int)eRecord_Status.eNormal).ToString() +
                        " and nvl(b.Deleted,0)=" + ((int)eRecord_Status.eNormal).ToString();
            sSql += " And a.LoginName=" + StringTool.SqlQ(sLoginName) + " And nvl(a.Password,'')=" + StringTool.SqlQ(sPassword);
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                string LoginName = string.Empty;
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sSql);
                if (dt.Rows.Count > 0)
                {
                    LoginName = dt.Rows[0]["LoginName"].ToString();
                    sCustName = dt.Rows[0]["ShortName"].ToString();
                }
                return LoginName;
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
    }
}
