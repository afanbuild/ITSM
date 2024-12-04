using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.SqlDAL.Excel
{
    public class ExcelSQl
    {
        #region 查询系统档案清单
        /// <summary>
        /// 查询系统档案清单
        /// <summary>
        /// </summary>
        /// <param name="strWhere"></param>
        /// <param name="strOrder"></param>
        /// <returns></returns>
        public static DataTable GetExcel(string strSQL)
        {

            OracleConnection cn = ConfigTool.GetConnection();
            return OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);

        }
        #endregion
    }
}
