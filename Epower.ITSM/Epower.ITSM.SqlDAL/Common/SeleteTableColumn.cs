using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.SqlDAL.Common
{
    public class SeleteTableColumn
    {

        #region 查询表字段
        /// <summary>
        /// 根据查询语句
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static DataTable GetTableColumnName(string groups)
        {

            OracleConnection cn = ConfigTool.GetConnection();
            string strSql = "SELECT * FROM EA_DefineLanguage Where 1=1 And IsValid=0 and  column_name is not null and groups='" + groups + "'";
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSql);
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
    }
}
