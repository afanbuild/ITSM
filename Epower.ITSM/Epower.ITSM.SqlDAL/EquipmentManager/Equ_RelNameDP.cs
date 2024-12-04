using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Epower.DevBase.BaseTools;
using System.Data.OracleClient;
//## 关联名称表的 数据访问类
namespace Epower.ITSM.SqlDAL.EquipmentManager
{
    /// <summary>
    /// 关联名称
    /// </summary>
    public class Equ_RelNameDP
    {
        public Equ_RelNameDP() { }
        /// <summary>
        /// 检索所有关联名称
        /// </summary>
        /// <returns></returns>
        public DataTable GetAll()
        {
            String strSQLQuery = "SELECT ID, RelKey FROM EQU_RELNAME ORDER BY ID ASC";

            OracleConnection cn = null;
            try
            {
                cn = ConfigTool.GetConnection();
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQLQuery);

                return dt;
            }
            finally
            {
                if (cn.State != ConnectionState.Closed)
                    ConfigTool.CloseConnection(cn);
            }
        }
        /// <summary>
        /// 添加关联名称.
        /// </summary>
        /// <param name="strRelName">关联名称</param>
        public long AddRelName(String strRelName)
        {
            long lngId = EpowerGlobal.EPGlobal.GetNextID("Equ_RelNameID");
            String strSQLInsert = String.Format("INSERT INTO EQU_RELNAME(ID, RELKEY) VALUES({0}, {1})",
                lngId, StringTool.SqlQ(strRelName));

            OracleConnection cn = null;
            try
            {
                cn = ConfigTool.GetConnection();
                Int32 intAffected = OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSQLInsert);

                return intAffected > 0 ? lngId : -1;
            }
            finally
            {
                if (cn.State != ConnectionState.Closed)
                    ConfigTool.CloseConnection(cn);
            }
        }
        /// <summary>
        /// 修改关联名称.
        /// </summary>        
        public Boolean UpdateRelName(Int32 intRelKeyId, String strRelKeyName)
        {
            String strSQLInsert = String.Format("UPDATE EQU_RELNAME SET RELKEY = {0} WHERE ID = {1}",
                StringTool.SqlQ(strRelKeyName), intRelKeyId);

            DataTable dt = CommonDP.ExcuteSqlTable(String.Format("SELECT RELKEY FROM EQU_RELNAME WHERE ID = {0}", intRelKeyId));

            String strSQL2 = String.Format("UPDATE EQU_REL SET RELKEY = {0} WHERE RELKEY = {1}",
                StringTool.SqlQ(strRelKeyName), StringTool.SqlQ(dt.Rows[0][0].ToString()));

            OracleConnection cn = null;
            cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
                cn.Open();

            OracleTransaction trans = cn.BeginTransaction();
            try
            {
                Int32 intAffected = OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL2);
                intAffected = OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQLInsert);

                trans.Commit();
                return intAffected > 0;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                if (cn.State != ConnectionState.Closed)
                    ConfigTool.CloseConnection(cn);
            }
        }
        /// <summary>
        /// 删除关联名称
        /// </summary>
        /// <param name="lngId">关联名称编号</param>
        /// <param name="strRelKey">关联名称</param>
        public Boolean DeleteById(long lngId, String strRelKey)
        {
            String strSQLDelete = String.Format("DELETE FROM EQU_RELNAME WHERE ID = {0}",
                lngId);
            // 移除关联名称时, 也移除与之相关的资产连接.
            String strSQLDeleteWithEQURel = String.Format("DELETE FROM EQU_REL WHERE RELKEY = {0}",
                StringTool.SqlQ(strRelKey));

            OracleConnection cn = null;
            OracleTransaction trans = null;
            try
            {
                cn = ConfigTool.GetConnection();
                if (cn.State != ConnectionState.Open)
                    cn.Open();

                trans = cn.BeginTransaction();

                OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQLDelete);
                OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQLDeleteWithEQURel);

                trans.Commit();

                return true;
            }
            catch (Exception ex)
            {
                trans.Rollback();

                throw ex;
            }
            finally
            {
                if (cn.State != ConnectionState.Closed)
                    ConfigTool.CloseConnection(cn);
            }
        }
    }
}
