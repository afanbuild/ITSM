/*******************************************************************
 *
 * Description:服务目录
 * Create By  :xjm(SuperMan)
 * Create Date:2011年8月26日
 * *****************************************************************/

using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;

namespace Epower.ITSM.SqlDAL
{
    public class Services_List
    {
        public Services_List()
        { }

        public static DataTable getServicesListCataLog()
        {
            string SQLstr = "";
            SQLstr = @"SELECT * FROM Es_Catalog  where ParentID in (select CatalogID from Es_Catalog where CatalogName='服务目录') and Deleted=0 order by SortID asc";

            OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationSettings.AppSettings["SQLConnString"]);
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, SQLstr);
            if (cn.State == ConnectionState.Open)
            {
                cn.Close();
            }
            return dt;
        }

        public static DataTable getServicesListDtl(string strCatalogID)
        {
            string SQLstr = "";
            SQLstr = @"select * from EA_ServicesTemplate where ServiceLevelID=" + strCatalogID;

            OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationSettings.AppSettings["SQLConnString"]);
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, SQLstr);
            if (cn.State == ConnectionState.Open)
            {
                cn.Close();
            }
            return dt;
        }
    }
}
