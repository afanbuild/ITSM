/***********************
 *   创建人：oyangby
 * 创建时间：2011-08-19
 *     说明：资产配置变更临时表信息     
 * *********************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Epower.DevBase.BaseTools;
using EpowerGlobal;
using System.Data.OracleClient;
using Epower.ITSM.SqlDAL.ES_TBLCS;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// 资产配置历史结构表
    /// </summary>
    public class Equ_DeskChangeDeploy
    {
        #region 构造方法

        /// <summary>
        /// 资产配置历史表
        /// </summary>
        public Equ_DeskChangeDeploy()
        {
        }

        #endregion

        #region 属性

        private Decimal mFlowId;
        /// <summary>
        /// 流程ID
        /// </summary>
        public Decimal FlowId
        {
            set { mFlowId = value; }
            get { return mFlowId; }
        }


        private long mID;
        /// <summary>
        /// ID
        /// </summary>
        public long ID
        {
            set { mID = value; }
            get { return mID; }
        }


        private long mEquID;
        /// <summary>
        /// 资产id
        /// </summary>
        public long EquID
        {
            set { mEquID = value; }
            get { return mEquID; }
        }


        private long mFieldID;
        /// <summary>
        /// 配置值得id
        /// </summary>
        public long FieldID
        {
            set { mFieldID = value; }
            get { return mFieldID; }
        }

        private String mCHName;
        /// <summary>
        /// 配置名称
        /// </summary>
        public String CHName
        {
            set { mCHName = value; }
            get { return mCHName; }
        }


        private String mValue;
        /// <summary>
        /// 配置值
        /// </summary>
        public String Value
        {
            set { mValue = value; }
            get { return mValue; }
        }
        #endregion

        #region 保存数据

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="deploy"></param>
        public void InsertRecorded(OracleTransaction trans, Equ_DeskChangeDeploy deploy)
        {
            string strSQL = "";
            strSQL = @"INSERT INTO Equ_DeskChangeDeploy(ID,FlowId,EquID,FieldID,CHName,Value) VALUES(" + deploy.ID + "," + deploy.FlowId + "," + deploy.EquID + "," + deploy.FieldID + "," + StringTool.SqlQ(deploy.CHName) + "," + StringTool.SqlQ(deploy.Value) + ")";
            OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);
        }

        #endregion

        #region 删除数据

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="deploy"></param>
        public void DeleteRecorded(OracleTransaction trans, Equ_DeskChangeDeploy deploy)
        {
            string strSQL = "";
            strSQL = @"DELETE Equ_DeskChangeDeploy WHERE FlowId=" + deploy.FlowId;
            OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);
        }




        #endregion

        #region 获得一个list的 对象 数组


        /// <summary>
        /// 获得一个list的 对象 数组
        /// </summary>
        /// <param name="EquID"></param>
        /// <returns></returns>
        public static List<EQU_deploy> getEQU_ChangeDeployList(long FlowID, long EquID)
        {
            List<EQU_deploy> list = new List<EQU_deploy>();
            string strSQL = @"select * from Equ_DeskChangeDeploy where EquId=" + EquID.ToString() + " and FlowID =" + FlowID.ToString();
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
                    list.Add(EQU);
                }

            }
            return list;
        }

        #endregion

        #region trans 获得一个list的 对象 数组


        /// <summary>
        /// 获得一个list的 对象 数组
        /// </summary>
        /// <param name="EquID"></param>
        /// <returns></returns>
        public static List<EQU_deploy> getEQU_ChangeDeployList(OracleTransaction trans, long FlowID, long EquID)
        {
            List<EQU_deploy> list = new List<EQU_deploy>();
            string strSQL = @"select * from Equ_DeskChangeDeploy where EquId=" + EquID.ToString() + " and FlowID =" + FlowID.ToString();
            DataTable dt = OracleDbHelper.ExecuteDataset(trans, CommandType.Text, strSQL).Tables[0];
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

    }
}
