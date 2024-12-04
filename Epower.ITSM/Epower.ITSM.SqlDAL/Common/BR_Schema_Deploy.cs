/*******************************************************************
 *
 * Description:存储配置项值操作类
 * 
 * 
 * Create By  :余向前
 * Create Date:2013-04-15
 * *****************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Epower.DevBase.BaseTools;
using EpowerGlobal;
using System.Data.OracleClient;
using Epower.ITSM.Base;

namespace Epower.ITSM.SqlDAL
{
    [Serializable]
    public class BR_Schema_Deploy
    {
        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        public BR_Schema_Deploy()
        {

        }
        #endregion

        #region 属性

        private long mID;
        /// <summary>
        /// 标示ID
        /// </summary>
        public long ID
        {
            get { return mID; }
            set { mID = value; }
        }

        private long mRelateID;
        /// <summary>
        /// 关联ID
        /// </summary>
        public long RelateID
        {
            get { return mRelateID; }
            set { mRelateID = value; }
        }

        private int mRelateType;
        /// <summary>
        /// 关联类型对应的值由Epower.ITSM.Base下的FMEnum.cs里的eSchemaRelateType枚举提供
        /// </summary>
        public int RelateType
        {
            get { return mRelateType; }
            set { mRelateType = value; }
        }

        private long mFieldID;
        /// <summary>
        /// 配置项编号ID
        /// </summary>
        public long FieldID
        {
            get { return mFieldID; }
            set { mFieldID = value; }
        }        
        
        private string mCHName;
        /// <summary>
        /// 配置项名称
        /// </summary>
        public string CHName
        {
            get { return mCHName; }
            set { mCHName = value; }
        }

        private string mValue;
        /// <summary>
        /// 配置值
        /// </summary>
        public string Value
        {
            get { return mValue; }
            set { mValue = value; }
        }  

        #endregion

        #region 保存数据

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="deploy"></param>
        public void save(OracleTransaction trans, BR_Schema_Deploy deploy)
        {
            string strSQL = "";
            deploy.ID = EPGlobal.GetNextID("BR_Schema_DeployID");
            strSQL = @"INSERT INTO BR_Schema_Deploy(ID,RelateID,RelateType,FieldID,CHName,Value)
                                values(" + deploy.ID + "," + deploy.RelateID + "," + deploy.RelateType + "," + deploy.FieldID + "," + StringTool.SqlQ(deploy.CHName) + "," + StringTool.SqlQ(deploy.Value) + ")";
            OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="deploy"></param>
        public void save(BR_Schema_Deploy deploy)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                string strSQL = "";

                deploy.ID = EPGlobal.GetNextID("BR_Schema_DeployID");
                strSQL = @"INSERT INTO BR_Schema_Deploy(ID,RelateID,RelateType,FieldID,CHName,Value)
                                values(" + deploy.ID + "," + deploy.RelateID + "," + deploy.RelateType + "," + deploy.FieldID + "," + StringTool.SqlQ(deploy.CHName) + "," + StringTool.SqlQ(deploy.Value) + ")";
                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSQL);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);

            }

        }
        #endregion

        #region 查询数据

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <returns></returns>
        public static DataTable select()
        {
            string strSQL = @"SELECT * FROM BR_Schema_Deploy";
            return CommonDP.ExcuteSqlTable(strSQL);
        }
        #endregion

        #region 根据关联ID 和关联类型查询数据
        /// <summary>
        /// 根据关联ID 和关联类型查询数据
        /// </summary>
        /// <param name="lngRelateID">关联ID</param>
        /// <param name="intRelateType">关联类型</param>
        /// <returns></returns>
        public static DataTable select(long lngRelateID, int intRelateType)
        {
            string strSQL = @"select * from BR_Schema_Deploy where RelateID=" + lngRelateID.ToString() + " and RelateType=" + intRelateType.ToString();
            return CommonDP.ExcuteSqlTable(strSQL);
        }
        #endregion

        #region 获得一个list对象
        /// <summary>
        /// 获得一个list对象
        /// </summary>
        /// <param name="lngRelateID">关联ID</param>
        /// <param name="intRelateType">关联类型</param>
        /// <returns></returns>
        public static List<BR_Schema_Deploy> getDeployList(long lngRelateID, int intRelateType)
        {
            List<BR_Schema_Deploy> list = new List<BR_Schema_Deploy>();
            string strSQL = @"select * from BR_Schema_Deploy where RelateID=" + lngRelateID.ToString() + " and RelateType=" + intRelateType.ToString();
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            if (dt.Rows.Count > 0)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    BR_Schema_Deploy deploy = new BR_Schema_Deploy();
                    deploy.ID = CTools.ToInt64(dr["ID"].ToString());
                    deploy.RelateID = CTools.ToInt64(dr["RelateID"].ToString());
                    deploy.RelateType = CTools.ToInt(dr["RelateType"].ToString());
                    deploy.FieldID = CTools.ToInt64(dr["FieldID"].ToString());
                    deploy.CHName = dr["CHName"].ToString();
                    deploy.Value = dr["value"].ToString();
                    list.Add(deploy);
                }

            }
            return list;
        }

        /// <summary>
        /// 获得一个list对象
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngRelateID">关联ID</param>
        /// <param name="intRelateType">关联类型</param>
        /// <returns></returns>
        public static List<BR_Schema_Deploy> getDeployList(OracleTransaction trans, long lngRelateID, int intRelateType)
        {
            List<BR_Schema_Deploy> list = new List<BR_Schema_Deploy>();
            string strSQL = @"select * from BR_Schema_Deploy where RelateID=" + lngRelateID.ToString() + " and RelateType=" + intRelateType.ToString();
            DataTable dt = OracleDbHelper.ExecuteDataset(trans, CommandType.Text, strSQL).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    BR_Schema_Deploy deploy = new BR_Schema_Deploy();
                    deploy.ID = CTools.ToInt64(dr["ID"].ToString());
                    deploy.RelateID = CTools.ToInt64(dr["RelateID"].ToString());
                    deploy.RelateType = CTools.ToInt(dr["RelateType"].ToString());
                    deploy.FieldID = CTools.ToInt64(dr["FieldID"].ToString());
                    deploy.CHName = dr["CHName"].ToString();
                    deploy.Value = dr["value"].ToString();
                    list.Add(deploy);
                }

            }
            return list;
        }
        #endregion

        #region 删除数据
        /// <summary>
        /// 删除所有数据
        /// </summary>
        /// <param name="lngRelateID">关联ID</param>
        /// <param name="intRelateType">关联类型</param>
        public static void DeleteAll(long lngRelateID, int intRelateType)
        {
            string strSQL = "delete from BR_Schema_Deploy where RelateID=" + lngRelateID.ToString() + " and RelateType=" + intRelateType.ToString();
            CommonDP.ExcuteSql(strSQL);
        }
        /// <summary>
        /// 删除所有数据
        /// </summary>
        /// <param name="lngRelateID">关联ID</param>
        /// <param name="intRelateType">关联类型</param>
        public static void DeleteAll(OracleTransaction trans, long lngRelateID, int intRelateType)
        {
            string strSQL = "delete from BR_Schema_Deploy where RelateID=" + lngRelateID.ToString() + " and RelateType=" + intRelateType.ToString();
            OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);
        }

        #endregion
    }
}
