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
using EpowerGlobal;
using System.Data.OracleClient;

namespace Epower.ITSM.SqlDAL.ES_TBLCS
{
    public class ES_TBLFileds
    {
        public ES_TBLFileds()
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

                isTrue = false;//判断是否
                try
                {
                    //插入数据库
                    strSQL = @" declare @maxID int 
                            SELECT @maxID=(nvl(max(id),0)+1) FROM ES_TBl
                            INSERT INTO ES_TBl(Id,tbl_Name) values(@maxID," + StringTool.SqlQ(name.Trim()) + ")";
                    CommonDP.ExcuteSql(strSQL);

                    //创建表
                    strSQL=" Create table "+name.Trim()+"( ID int primary key )";
                    CommonDP.ExcuteSql(strSQL);
                    return true;//保存成功
                }
                catch
                {
                    //保存失败
                    return false;
                }
            }
            else
            {
                isTrue = true;
                return false;
            }
        }
        #endregion

        #region 属性 
        private long intID;
        /// <summary>
        /// 主键id
        /// </summary>
        public long ID {
            set { intID=value;}
            get { return intID;}
        }

        private long intTBL_ID;
        /// <summary>
        /// 主配置表id
        /// </summary>
        public long TBL_ID
        {
            set { intTBL_ID = value; }
            get { return intTBL_ID; }
        }

        private string  strFilesName;
        /// <summary>
        /// 添加字段名称id
        /// </summary>
        public string FilesName
        {
            set { strFilesName = value; }
            get { return strFilesName; }
        }
        #endregion 


        /// <summary>
        /// 插入保存
        /// </summary>
        /// <param name="FiledTBL"></param>
        /// <returns></returns>
        public bool InsertSave(ref ES_TBLFileds FiledTBL)
        {
            string strSQL = " select * from  syscolumns where id =(select ID from sysobjects where name =(select tbl_Name from ES_TBl where ID=" + FiledTBL.TBL_ID + ")) and name =" + StringTool.SqlQ(FiledTBL.FilesName);
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            if (dt.Rows.Count == 0)
            {
                OracleConnection cn = ConfigTool.GetConnection();
                cn.Open();
                OracleTransaction trans = cn.BeginTransaction();                
                try
                {
                    FiledTBL.ID = EPGlobal.GetNextID("ES_TBLFiledsID");
                    //插入数据库
                    strSQL = @" INSERT INTO  ES_TBl_Files(Id,tbl_ID ,filesName) values(" + FiledTBL.ID + "," + FiledTBL.TBL_ID + "," + StringTool.SqlQ(FiledTBL.FilesName) + ")";
                    OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);

                    //根据id 获得表名称
                    string tabbleName = ES_TBL.RtnName(FiledTBL.TBL_ID);
                    if (tabbleName!="")
                    {
                        //创建表字段
                        strSQL = "alter table " + tabbleName + " add  " + FiledTBL.FilesName + " nvarchar(100) null";
                    }
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
                return false;
            }
 
        }


        /// <summary>
        /// 更新保存
        /// </summary>
        /// <param name="FiledTBL"></param>
        /// <returns></returns>
        public bool UpdateSave(ref ES_TBLFileds FiledTBL)
        {
            string strSQL = " select * from  syscolumns where id =(select ID from sysobjects where name =(select tbl_Name from ES_TBl where ID=" + FiledTBL.TBL_ID + ")) and name =" + StringTool.SqlQ(FiledTBL.FilesName);
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            if (dt.Rows.Count == 0)
            {

                //获得旧字段名称
                string strfeilname = getFeileName(FiledTBL.ID);

                OracleConnection cn = ConfigTool.GetConnection();
                cn.Open();
                OracleTransaction trans = cn.BeginTransaction();                
                try
                {   
                    //插入数据库
                    strSQL = @" UPDATE  ES_TBl_Files  set filesName= "+StringTool.SqlQ(FiledTBL.FilesName) +" WHERE ID =" + FiledTBL.ID + " and tbl_ID=" + FiledTBL.TBL_ID;
                    OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);
                    //根据id 获得表名称
                    string tabbleName = ES_TBL.RtnName(FiledTBL.TBL_ID);
                    if (tabbleName != "" && strfeilname != "")
                    {
                        //修改表字段
                        strSQL = "sp_rename   N'" + tabbleName + "." + strfeilname + "'," + StringTool.SqlQ(FiledTBL.FilesName) + ",'column' ";
                    }
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
                return false;
            }

        }


        #region 查询
        /// <summary>
        /// 查询出全部的
        /// </summary>
        /// <returns></returns>
        public static DataTable select()
        {
            string strSQL = @"
                      SELECT A.*,B.tbl_Name 
                        FROM ES_TBl_Files A
                   LEFT JOIN ES_TBl B
                          ON A.tbl_ID =B.Id";
            return CommonDP.ExcuteSqlTable(strSQL);
        }

        /// <summary>
        /// 根据id查询 单条数据
        /// </summary>
        /// <returns></returns>
        public static DataTable select(long id)
        {
            string strSQL = @"SELECT A.*,B.tbl_Name 
                        FROM ES_TBl_Files A
                   LEFT JOIN ES_TBl B
                          ON A.tbl_ID =B.Id WHERE A.ID=" + id.ToString();
            return CommonDP.ExcuteSqlTable(strSQL);
        }


        public string getFeileName(long id)
        {
            string strSQL = "select filesName from ES_TBl_Files WHERE ID ="+id.ToString();
            DataTable dt= CommonDP.ExcuteSqlTable(strSQL);
            string rtnValue=string.Empty;//返回值
            if (dt.Rows.Count > 0)
            {
                rtnValue = dt.Rows[0]["filesName"].ToString();
            }
            return rtnValue;
        }

        #endregion 


    }
}
