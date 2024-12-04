/***********************
 *   创建人：yanghw
 * 创建时间：2011-08-01
 *     说明：资产配置历史结构表 操作类     
 * *********************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Epower.DevBase.BaseTools;
using EpowerGlobal;
using System.Data.OracleClient;

namespace Epower.ITSM.SqlDAL.ES_TBLCS
{
    /// <summary>
    /// 资产配置历史结构表
    /// </summary>
    [Serializable]
    public class EQU_deployHistory
    {
        #region 构造方法


        /// <summary>
        /// 资产配置历史表
        /// </summary>
        public EQU_deployHistory()
        {
        }

        #endregion

        #region 属性


        private long intversion;
        /// <summary>
        /// 资产历史版本号

        /// </summary>
        public long version
        {
            set { intversion = value; }
            get { return intversion; }
        }


        private DateTime DataversionTime;
        /// <summary>
        /// 资产历史版产生时间

        /// </summary>
        public DateTime versionTime
        {
            set { DataversionTime = value; }
            get { return DataversionTime; }
        }




        private long intID;
        /// <summary>
        /// 资产id
        /// </summary>
        public long ID
        {
            set { intID = value; }
            get { return intID; }
        }

        private long intEquID;
        /// <summary>
        /// 资产id
        /// </summary>
        public long EquID
        {
            set { intEquID = value; }
            get { return intEquID; }
        }

        private long intFieldID;
        /// <summary>
        /// 配置值得id
        /// </summary>
        public long FieldID
        {
            set { intFieldID = value; }
            get { return intFieldID; }
        }

        private string strCHName;
        /// <summary>
        /// 配置名称
        /// </summary>
        public string CHName
        {
            set { strCHName = value; }
            get { return strCHName; }
        }


        private string strValue;
        /// <summary>
        /// 配置值

        /// </summary>
        public string Value
        {
            set { strValue = value; }
            get { return strValue; }
        }
        #endregion

        #region 保存数据

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="deploy"></param>
        public void saveInsert(OracleTransaction trans, EQU_deployHistory deploy)
        {
            string strSQL = "";
            strSQL = @"INSERT INTO EQU_deployHistory(ID,EquID,FieldID,CHName,Value,version,versionTime)
                                values(" + deploy.ID + "," + deploy.EquID + "," + deploy.FieldID + "," + StringTool.SqlQ(deploy.CHName) + "," + StringTool.SqlQ(deploy.Value) + "," + deploy.version + ",sysdate)";
            OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);
        }

        #endregion

        #region saveUpdate

        /// <summary>
        /// 当是同一个人更新同一个资产时，版本号不发生改变，历史版本 不重新新增，只做修改
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="deploy"></param>
        public void saveUpdate(OracleTransaction trans, EQU_deployHistory deploy)
        {
            string strSQL = "";
            strSQL = @"update EQU_deployHistory set versionTime=sysdate " +
                                                    " where ID=" + deploy.ID +
                                                      " and EquID=" + deploy.EquID +
                                                      " and version=" + deploy.version;
            OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);
        }

        #endregion

        #region 删除

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="EquId"></param>
        /// <param name="Version"></param>
        public static void Delete(OracleTransaction trans, long EquId, long Version)
        {
            string strSQL = "DELETE EQU_deployHistory WHERE EquID=" + EquId.ToString() + " AND version=" + Version.ToString();
            OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);
        }

        #endregion
        #region 查询
        /// <summary>
        /// 查询出全部的
        /// </summary>
        /// <returns></returns>
        public static DataTable select()
        {
            string strSQL = @"SELECT * FROM EQU_deploy";
            return CommonDP.ExcuteSqlTable(strSQL);
        }

        /// <summary>
        /// 根据资产id查询 单个资产的数据
        /// </summary>
        /// <returns></returns>
        public static DataTable select(long EquID)
        {
            string strSQL = @"select * from EQU_deploy where EquId=" + EquID.ToString();
            return CommonDP.ExcuteSqlTable(strSQL);
        }


        /// <summary>
        /// 获得一个list的 对象 数组
        /// </summary>
        /// <param name="EquID"></param>
        /// <returns></returns>
        public static List<EQU_deployHistory> getEQU_deployList(long EquID, long version)
        {
            List<EQU_deployHistory> list = new List<EQU_deployHistory>();
            string strSQL = @"select * from EQU_deployHistory where EquId=" + EquID.ToString() + " and version=" + version.ToString();
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            if (dt.Rows.Count > 0)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    EQU_deployHistory EQU = new EQU_deployHistory();
                    EQU.ID = long.Parse(dr["ID"].ToString());
                    EQU.EquID = long.Parse(dr["EquID"].ToString());
                    EQU.FieldID = long.Parse(dr["FieldID"].ToString());
                    EQU.CHName = dr["CHName"].ToString();
                    EQU.Value = dr["value"].ToString();
                    EQU.version = int.Parse(dr["version"].ToString());
                    EQU.versionTime = DateTime.Parse(dr["versionTime"].ToString());
                    list.Add(EQU);
                }

            }
            return list;
        }

        /// <summary>
        /// 获得一个list的 对象 数组  从list表中取出历史变更记录显示在历史表中

        /// </summary>
        /// <param name="EquID"></param>
        /// <returns></returns>
        public static List<EQU_deploy> getEQU_deployVersionList(long EquID, long lngVersion)
        {
            List<EQU_deploy> list = new List<EQU_deploy>();
            string strSQL = @"select * from EQU_deployHistory where EquId=" + EquID.ToString() + " and version=" + lngVersion.ToString();
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            if (dt.Rows.Count > 0)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    EQU_deploy EQU = new EQU_deploy();
                    EQU.ID = long.Parse(dr["ID"].ToString());
                    EQU.EquID = long.Parse(dr["EquID"].ToString());
                    EQU.FieldID = long.Parse(dr["FieldID"].ToString());
                    EQU.CHName = dr["CHName"].ToString();
                    EQU.Value = dr["value"].ToString();
                    //EQU.version = int.Parse(dr["version"].ToString());
                    //EQU.versionTime = DateTime.Parse(dr["versionTime"].ToString());
                    list.Add(EQU);
                }

            }
            return list;
        }



        /// <summary>
        /// 获得一个list的 对象 数组  从list表中取出历史变更记录显示在历史表中

        /// </summary>
        /// <param name="EquID"></param>
        /// <returns></returns>
        public static List<EQU_deploy> getEQU_deployHostoryList(long EquID, long FlowID)
        {
            List<EQU_deploy> list = new List<EQU_deploy>();
            string strSQL = @"select * from EQU_deployHistory where EquId=" + EquID.ToString() + " and version=(select version  from Equ_DeskChange where flowid =" + FlowID.ToString() + " and id=" + EquID.ToString() + " )";
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            if (dt.Rows.Count > 0)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    EQU_deploy EQU = new EQU_deploy();
                    EQU.ID = long.Parse(dr["ID"].ToString());
                    EQU.EquID = long.Parse(dr["EquID"].ToString());
                    EQU.FieldID = long.Parse(dr["FieldID"].ToString());
                    EQU.CHName = dr["CHName"].ToString();
                    EQU.Value = dr["value"].ToString();
                    //EQU.version = int.Parse(dr["version"].ToString());
                    //EQU.versionTime = DateTime.Parse(dr["versionTime"].ToString());
                    list.Add(EQU);
                }

            }
            return list;
        }

        #endregion

    }
}
