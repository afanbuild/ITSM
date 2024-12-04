using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Epower.DevBase.BaseTools;
using System.Data.OracleClient;

namespace Epower.ITSM.SqlDAL.Common
{
    /// <summary>
    /// 自动刷新页面
    /// </summary>
    public class RefreshPage
    {

        #region 查询事件
        /// <summary>
        /// 查询事件
        /// <summary>
        /// </summary>
        /// 
        public static DataTable GetRefreshPageDate()
        {
            string strSQL = string.Empty;
            strSQL = @"select * from v_refreshPage";
            OracleConnection cn = ConfigTool.GetConnection();
            try { return OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL); }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion

        #region 查询事件
        /// <summary>
        /// 查询事件
        /// <summary>
        /// </summary>
        /// 
        public static DataTable GetRefreshPageDate1()
        {
            string strSQL = string.Empty;
            strSQL = @"select * from v_YearRefreshpage";
            OracleConnection cn = ConfigTool.GetConnection();
            try { return OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL); }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion

        #region 查询事件详细信息
        /// <summary>
        /// 查询事件详细信息
        /// <summary>
        /// </summary>
        public static DataTable GetRefreshPage(int pagesize, int pageindex, ref int rowcount)
        {

            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "V_ISSUEREFRESH", "*", "order by regsysdate desc", pagesize, pageindex, " 1=1 and status=20", ref rowcount);
                
                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }

        }
        #endregion
    }
}
