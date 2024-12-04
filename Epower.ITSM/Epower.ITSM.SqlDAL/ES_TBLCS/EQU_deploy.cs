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
    [Serializable]
    public class EQU_deploy
    {
        #region 构造方法
        
        /// <summary>
        /// 构造方法
        /// </summary>
        public EQU_deploy()
        {
        }

        #endregion

        #region 属性
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

        #region save
        
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="deploy"></param>
        public void save(OracleTransaction trans, EQU_deploy deploy)
        {
            string strSQL="";
            if (deploy.ID == 0)
            {

                deploy.ID = EPGlobal.GetNextID("EQU_deployID");
                strSQL = @"INSERT INTO EQU_deploy(ID,EquID,FieldID,CHName,Value)
                                values(" + deploy.ID + "," + deploy.EquID + "," + deploy.FieldID + "," + StringTool.SqlQ(deploy.CHName) + "," + StringTool.SqlQ(deploy.Value) + ")";
            }
            else
            {
                strSQL = @"UPDATE  EQU_deploy set EquID=" + deploy.EquID + ",FieldID=" + deploy.FieldID + ",CHName=" + StringTool.SqlQ(deploy.CHName) + ",Value=" + StringTool.SqlQ(deploy.Value)
                      +" where  ID =" + deploy.ID ;
            }
            OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);
        }

        //流程保存数据
        public void save(EQU_deploy deploy)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                string strSQL = "";
                if (deploy.ID == 0)
                {

                    deploy.ID = EPGlobal.GetNextID("EQU_deployID");
                    strSQL = @"INSERT INTO EQU_deploy(ID,EquID,FieldID,CHName,Value)
                                values(" + deploy.ID + "," + deploy.EquID + "," + deploy.FieldID + "," + StringTool.SqlQ(deploy.CHName) + "," + StringTool.SqlQ(deploy.Value) + ")";
                }
                else
                {
                    strSQL = @"UPDATE  EQU_deploy set EquID=" + deploy.EquID + ",FieldID=" + deploy.FieldID + ",CHName=" + StringTool.SqlQ(deploy.CHName) + ",Value=" + StringTool.SqlQ(deploy.Value)
                          + " where  ID =" + deploy.ID;
                }
                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSQL);
            }
            catch (Exception)
            {

                
                throw;
            }
            finally {
                ConfigTool.CloseConnection(cn);
                
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
            string strSQL = @"SELECT * FROM EQU_deploy";
            return CommonDP.ExcuteSqlTable(strSQL);
        }

        /// <summary>
        /// 根据资产id查询 单个资产的数据
        /// </summary>
        /// <returns></returns>
        public static DataTable select(long EquID)
        {
            string strSQL = @"select * from EQU_deploy where EquId="+EquID.ToString();
            return CommonDP.ExcuteSqlTable(strSQL);
        }


        /// <summary>
        /// 获得一个list的 对象 数组
        /// </summary>
        /// <param name="EquID"></param>
        /// <returns></returns>
        public static List<EQU_deploy> getEQU_deployList(long EquID)
        {
            List<EQU_deploy> list =new List<EQU_deploy>();
            string strSQL = @"select * from EQU_deploy where EquId=" + EquID.ToString();            
            DataTable dt= CommonDP.ExcuteSqlTable(strSQL);
            if(dt.Rows.Count>0)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    EQU_deploy EQU = new EQU_deploy();
                    EQU.ID = long.Parse(dr["ID"].ToString());
                    EQU.EquID = long.Parse(dr["EquID"].ToString());
                    EQU.FieldID = long.Parse(dr["FieldID"].ToString());
                    EQU.CHName = dr["CHName"].ToString();
                    EQU.Value = dr["value"].ToString();
                    list.Add(EQU);
                }

            }
            return list;
        }    

        #region
        /// <summary>
        /// 获得一个list的 对象 数组
        /// </summary>
        /// <param name="EquID"></param>
        /// <returns></returns>
        public static List<EQU_deploy> getEQU_deployList(OracleTransaction trans, long EquID)
        {
            List<EQU_deploy> list = new List<EQU_deploy>();
            string strSQL = @"select * from EQU_deploy where EquId=" + EquID.ToString();
            DataTable dt = OracleDbHelper.ExecuteDataset(trans,CommandType.Text,strSQL).Tables[0];
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
                    list.Add(EQU);
                }

            }
            return list;
        }    
        #endregion

        #endregion
    }
}
