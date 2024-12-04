using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web
{
    public class EngineerSourceServer
    {
        private const string EngineerKeyName = "ItEngineerSource";
        private const string EquKeyName = "EquEngineerSource";


        public DataTable GetEngineerTable()
        {
            checkCache();
            return HttpRuntime.Cache[EngineerKeyName] as DataTable;            
        }

        private void checkCache()
        {

            if (HttpRuntime.Cache[EngineerKeyName] == null)
            {
                HttpRuntime.Cache.Add(
                        EngineerKeyName,
                        getItEngineerTable(),
                        null,
                        DateTime.Now.AddMinutes(15),
                        System.Web.Caching.Cache.NoSlidingExpiration,
                        System.Web.Caching.CacheItemPriority.Default,
                        null);
            }
        }

        private DataTable getItEngineerTable()
        {
            string strSql = "select \"NAME\",F_PINYIN(\"NAME\") AS FIRSTWORD,\"ID\" from Cst_ServiceStaff where deleted=0";
            return CommonDP.ExcuteSqlTable(strSql);
        }

        #region 资产设备
        public DataTable GetEqu_DeskTable()
        {
            checkCacheEqu();
            return HttpRuntime.Cache[EquKeyName] as DataTable;
        }
        private void checkCacheEqu()
        {

            if (HttpRuntime.Cache[EquKeyName] == null)
            {
                HttpRuntime.Cache.Add(
                        EquKeyName,
                        getItEquTable(),
                        null,
                        DateTime.Now.AddMinutes(15),
                        System.Web.Caching.Cache.NoSlidingExpiration,
                        System.Web.Caching.CacheItemPriority.Default,
                        null);
            }
        }

        private DataTable getItEquTable()
        {
            string strSql = "select \"NAME\",F_PINYIN(\"NAME\") AS FIRSTWORD,\"ID\" from V_Equ_Desk";
            return CommonDP.ExcuteSqlTable(strSql);
        }
        #endregion

    }
}
