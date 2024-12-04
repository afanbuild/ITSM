/***********************
 *   创建人：yanghw
 * 创建时间：2011-08-01
 *     说明：自定义配置表 操作类     
 * *********************/


using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Epower.DevBase.BaseTools;
using System.Data.OracleClient;
using EpowerCom;

namespace Epower.ITSM.SqlDAL.ES_TBLCS
{
    public class ES_TBL
    {
        public ES_TBL()
        {
        }

        #region 保存
        /// <summary>
        /// 保存配置表

        /// </summary>
        /// <param name="name"></param>
        /// <param name="isTrue"></param>
        /// <returns></returns>
        public static bool Save(string name,ref bool isTrue)
        {

            string strSQL = "select * from ES_TBl where tbl_Name=" + StringTool.SqlQ(name.Trim());
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            if (dt.Rows.Count == 0)
            {
                OracleConnection cn = ConfigTool.GetConnection();
                cn.Open();
                OracleTransaction trans = cn.BeginTransaction();
                isTrue = false;//判断是否
                try
                {
                    //插入数据库

                    strSQL = @" declare @maxID int 
                            SELECT @maxID=(nvl(max(id),0)+1) FROM ES_TBl
                            INSERT INTO ES_TBl(Id,tbl_Name) values(@maxID," + StringTool.SqlQ(name.Trim()) + ")";
                    OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);

                    //CommonDP.ExcuteSql(strSQL);

                    //创建表

                    strSQL = " Create table " + name.Trim() + "( ID int primary key )";
                    OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);
                    trans.Commit();
                    return true;//保存成功
                }
                catch
                {
                    trans.Rollback();
                    //保存失败
                    return false;
                }
                finally
                {
                    cn.Close();
                }
            }
            else
            {
                isTrue = true;
                return false;
            }
        }
        #endregion


        #region 查询
        /// <summary>
        /// 查询出全部的
        /// </summary>
        /// <returns></returns>
        public static DataTable select()
        {
            string strSQL = "SELECT * FROM ES_TBl";
            return CommonDP.ExcuteSqlTable(strSQL);
        }

        /// <summary>
        /// 根据id查询 单条数据
        /// </summary>
        /// <returns></returns>
        public static DataTable select(long id)
        {
            string strSQL = "SELECT * FROM ES_TBl WHERE ID=" + id.ToString();
            return CommonDP.ExcuteSqlTable(strSQL);
        }


        /// <summary>
        /// 根据id查询 查询表名
        /// </summary>
        /// <returns></returns>
        public static string  RtnName(long id)
        {
            string strSQL = "SELECT tbl_Name FROM ES_TBl WHERE ID=" + id.ToString();
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            string tblName = string.Empty;//返回的table的名称

            if (dt.Rows.Count > 0)
            {
                tblName = dt.Rows[0]["tbl_Name"].ToString();
            }
            return tblName;

        }
        #endregion 

        #region 查询自定义表单的记录
        /// <summary>
        /// 查询自定义表单的记录
        /// </summary>
        /// <param name="strXml"></param>
        /// <param name="strWhere"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="rowcount"></param>
        /// <returns></returns>
        public static DataTable GetDefineLanguageLists(string strXml, string strWhere, int pagesize, int pageindex, ref int rowcount)
        {
            string pWhere = " 1=1 ";
            FieldValues fv = new FieldValues(strXml);

            #region 查询参数    
            if (fv.GetFieldValue("Group").Value != string.Empty && fv.GetFieldValue("Group").Value != "0")
            {
                pWhere += " and Groups = " + StringTool.SqlQ(fv.GetFieldValue("Group").Value);
            }
            if (fv.GetFieldValue("KeyValue").Value != string.Empty)
            {
                pWhere += " and KeyValue like " + StringTool.SqlQ("%" + fv.GetFieldValue("KeyValue").Value + "%");
            }
            #endregion

            pWhere += strWhere;

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "EA_DefineLanguage", "*", " ORDER BY Groups Desc", pagesize, pageindex, pWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region 获取自定义表单中所有的分组名称
        /// <summary>
        /// 获取自定义表单中所有的分组名称
        /// </summary>
        /// <returns></returns>
        public static DataTable GetGroups()
        {
            string strSql = "select Groups from EA_DefineLanguage group by Groups";

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSql);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region 根据ID更新信息项名称

        /// <summary>
        /// 根据ID更新信息项名称

        /// </summary>
        /// <param name="strID"></param>
        /// <param name="strKeyValue"></param>
        public static void UptKeyValueByID(string strID,string strKeyValue)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {

                string strRet = string.Empty;
                string strSql = "update EA_DefineLanguage set KeyValue = " + StringTool.SqlQ(strKeyValue) + " where ID = " + strID;

                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSql);
                
            }
            finally { ConfigTool.CloseConnection(cn); }
           
        }
        #endregion
    }
}
