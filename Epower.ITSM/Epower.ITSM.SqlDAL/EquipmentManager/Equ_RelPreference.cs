using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Epower.DevBase.BaseTools;
using System.Data.OracleClient;
/*
 * #####
 * 资产关联图视角的偏好设置
 * #####
 * **/
namespace Epower.ITSM.SqlDAL.EquipmentManager
{
    /// <summary>
    /// 资产关联图视角的偏好设置
    /// </summary>
    public class Equ_RelPreference
    {
        #region 对象属性
        /// <summary>
        /// 用户编号
        /// </summary>        
        private Int32 _intUserId;
        /// <summary>
        /// 偏好编号
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public Int32 UserId { get; set; }
        /// <summary>
        /// 视角编号
        /// </summary>
        public Int32 RelKeyId { get; set; }
        /// <summary>
        /// 视角名称
        /// </summary>
        public String RelKey { get; set; }
        #endregion

        /// <summary>
        /// 默认无参
        /// </summary>
        public Equ_RelPreference() { }

        #region 用户个人偏好设置
        /// <summary>
        /// 加载用户的视角偏好.
        /// <code>
        /// List<Equ_RelPreference> listPrefer = new Equ_RelPreference(10012).Load();
        /// </code>
        /// </summary>
        /// <returns>视角偏好集合</returns>
        public static List<Equ_RelPreference> Load(Int32 intUserId)
        {
            String strSQLQuery = @"SELECT PREFER.ID, PREFER.UserId, PREFER.RelKeyId, RelName.Relkey 
                                   FROM EQU_RELPREFERENCE PREFER 
                                   LEFT JOIN EQU_REL RelName ON PREFER.RELKEYID = RELNAME.ID
                                   WHERE PREFER.USERID = {0} ";
            strSQLQuery = String.Format(strSQLQuery, intUserId);

            OracleConnection cn = null;
            try
            {
                cn = ConfigTool.GetConnection();
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQLQuery);

                List<Equ_RelPreference> listPreference = new List<Equ_RelPreference>();
                foreach (DataRow item in dt.Rows)
                {
                    Equ_RelPreference prefer = new Equ_RelPreference();
                    prefer.Id = Int32.Parse(item["ID"].ToString());
                    prefer.UserId = Int32.Parse(item["UserId"].ToString());
                    prefer.RelKeyId = Int32.Parse(item["RelKeyId"].ToString());
                    prefer.RelKey = item["RelKey"].ToString();
                    listPreference.Add(prefer);
                }

                return listPreference;
            }
            finally
            {
                if (cn.State != ConnectionState.Closed)
                    ConfigTool.CloseConnection(cn);
            }
        }
        /// <summary>
        /// 新增视角
        /// <code>
        /// EQU_RELPREFERENCE prefer = new EQU_RELPREFERENCE()
        /// prefer.UserId = 1001;
        /// prefer.RelKeyId = 102;
        /// prefer.Save();
        /// 
        /// String strRelKey = prefer.RelKey;
        /// </code>
        /// </summary>
        /// <returns>偏好编号</returns>
        public long Save()
        {
            long lngId = EpowerGlobal.EPGlobal.GetNextID("Equ_RelPreferID");
            String strSQLQuery = String.Format(@"INSERT INTO 
                EQU_RELPREFERENCE(ID, USERID, RELKEYID) VALUES({0},{1},{2})", lngId, UserId, RelKeyId);

            OracleConnection cn = null;
            try
            {
                cn = ConfigTool.GetConnection();

                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSQLQuery);

                this.Id = lngId;

                strSQLQuery = String.Format(@"SELECT RELKEY FROM EQU_RELNAME WHERE ID = {0}",
                    this.RelKeyId);
                Object o = OracleDbHelper.ExecuteScalar(cn, CommandType.Text, strSQLQuery);
                if (o != null) this.RelKey = o.ToString();
                else throw new Exception("在视角表中没有与之对应的视角!");

                return this.Id;
            }
            finally
            {
                if (cn.State != ConnectionState.Closed)
                    ConfigTool.CloseConnection(cn);
            }
        }
        /// <summary>
        /// 删除某个视角
        /// </summary>
        public void Delete()
        {
            String strSQLQuery = String.Format(@"DELETE FROM EQU_RELPREFERENCE WHERE ID = {0}",
                this.Id);

            OracleConnection cn = null;
            try
            {
                cn = ConfigTool.GetConnection();

                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSQLQuery);
            }
            finally
            {
                if (cn.State != ConnectionState.Closed)
                    ConfigTool.CloseConnection(cn);
            }
        }
        /// <summary>
        /// 加载用户视角
        /// </summary>
        /// <param name="intUserId">用户编号</param>
        /// <returns></returns>
        public static DataTable LoadDataByUid(Int32 intUserId)
        {
            String strSQLQuery = String.Format(@"SELECT * FROM EQU_RELNAME REL LEFT JOIN EQU_RELPREFERENCE PREFER
                        ON REL.ID = PREFER.RELKEYID WHERE USERID = {0} ORDER BY REL.ID ASC", intUserId);

            OracleConnection cn = null;
            try
            {
                cn = ConfigTool.GetConnection();

                return OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQLQuery);
            }
            finally
            {
                if (cn.State != ConnectionState.Closed)
                    ConfigTool.CloseConnection(cn);
            }
        }
        #endregion

        #region 临时导入数据方法
        /// <summary>
        /// 临时方法: 用户第一次进入偏好设置时, 自动为其导入所有视角
        /// </summary>
        public static void ImportData(Int32 intUserId)
        {
            Equ_RelNameDP equRelNameDP = new Equ_RelNameDP();
            DataTable dt = equRelNameDP.GetAll();

            String strSQLQuery = String.Empty;

            OracleConnection cn = null;
            cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
                cn.Open();

            OracleTransaction trans = cn.BeginTransaction();
            try
            {
                foreach (DataRow item in dt.Rows)
                {
                    long lngId = EpowerGlobal.EPGlobal.GetNextID("Equ_RelPreferID");
                    strSQLQuery = String.Format(@"INSERT INTO 
                EQU_RELPREFERENCE(ID, USERID, RELKEYID) VALUES({0},{1},{2})",
                                  lngId, intUserId, item["ID"]);

                    OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQLQuery);
                }

                trans.Commit();
            }
            catch (Exception ex) { trans.Rollback(); throw ex; }
            finally
            {
                if (cn.State != ConnectionState.Closed)
                    ConfigTool.CloseConnection(cn);
            }
        }
        /// <summary>
        /// 临时方法:检测该用户是否已导入偏好设置
        /// </summary>
        /// <param name="intUserId">用户编号</param>
        /// <returns>T, 是;F, 否</returns>
        public static Boolean CheckHave(Int32 intUserId)
        {
            String strSQLQuery = String.Format(@"SELECT ID FROM EQU_RELPREFERENCE WHERE USERID = {0}",
            intUserId);

            OracleConnection cn = null;
            try
            {
                cn = ConfigTool.GetConnection();

                Object o = OracleDbHelper.ExecuteScalar(cn, CommandType.Text, strSQLQuery);
                return o != null && Int32.Parse(o.ToString()) > 0;
            }
            finally
            {
                if (cn.State != ConnectionState.Closed)
                    ConfigTool.CloseConnection(cn);
            }
        }
        #endregion
    }
}
