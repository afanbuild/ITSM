/*******************************************************************
 * 版权所有：深圳市非凡信息技术有限公司
 * 描述：知识转移规则设置操作类

 * 
 * 
 * 创建人：余向前
 * 创建日期：2013-06-03 
 * 
 * 修改日志：
 * 修改时间：
 * 修改描述：
 * 
 * *****************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// 知识转移数据库操作及实体
    /// </summary>
    public class Inf_transfer_setDP
    {
        #region 属性

        #region ID
        private long mID;
        /// <summary>
        /// 标示ID
        /// </summary>
        public long ID
        {
            set
            {
                mID = value;
            }
            get
            {
                return mID;
            }
        }
        #endregion

        #region AppID
        private long mAppID;
        /// <summary>
        /// 应用ID
        /// </summary>
        public long AppID
        {
            set
            {
                mAppID = value;
            }
            get
            {
                return mAppID;
            }
        }
        #endregion

        #region OFlowModelID
        private long mOFlowModelID;
        /// <summary>
        /// 流程模型ID
        /// </summary>
        public long OFlowModelID
        {
            set
            {
                mOFlowModelID = value;
            }
            get
            {
                return mOFlowModelID;
            }
        }
        #endregion

        #region FIELDDESCRIPTION
        private string mFIELDDESCRIPTION;
        /// <summary>
        /// 转移字段描述
        /// </summary>
        public string FIELDDESCRIPTION
        {
            set
            {
                mFIELDDESCRIPTION = value;
            }
            get
            {
                return mFIELDDESCRIPTION;
            }
        }
        #endregion

        #region FLOWFIELD
        private string mFLOWFIELD;
        /// <summary>
        /// 需要转移表对应的字段名称
        /// </summary>
        public string FLOWFIELD
        {
            get { return mFLOWFIELD; }
            set { mFLOWFIELD = value; }
        }
        #endregion

        #region INFOFIELD
        private string mINFOFIELD;
        /// <summary>
        /// 转入对应的知识表字段名称
        /// </summary>
        public string INFOFIELD
        {
            get { return mINFOFIELD; }
            set { mINFOFIELD = value; }
        }
        #endregion

        #endregion       

        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        public Inf_transfer_setDP()
        {

        }
        #endregion

        #region 添加记录
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="ee"></param>
        public void InsertRecorded(Inf_transfer_setDP pInf_transfer_setDP)
        {
            string strSQL = string.Empty;
            string strID = "0";
            try
            {
                strID = EpowerGlobal.EPGlobal.GetNextID("Inf_transfer_setID").ToString();
                pInf_transfer_setDP.ID = long.Parse(strID);
                strSQL = @"INSERT INTO inf_transfer_set(
									ID,
									AppID,
									OFlowModelID,
									FIELDDESCRIPTION,
									FLOWFIELD,
									INFOFIELD
                            )
					VALUES( " +
                            strID.ToString() + "," +
                            pInf_transfer_setDP.AppID + "," +
                            pInf_transfer_setDP.OFlowModelID + "," +
                            StringTool.SqlQ(pInf_transfer_setDP.FIELDDESCRIPTION) + "," +
                            StringTool.SqlQ(pInf_transfer_setDP.FLOWFIELD) + "," +
                            StringTool.SqlQ(pInf_transfer_setDP.INFOFIELD) +
                    ")";

                CommonDP.ExcuteSql(strSQL);
            }
            catch (Exception ex)
            {
                E8Logger.Debug("保存知识转移配置数据出错" + ex.Message + ",数据库语句:" + strSQL);
            }
        }
        #endregion

        #region 根据应用ID和流程模型ID获取进度配置DataTable
        /// <summary>
        /// 根据应用ID和流程模型ID获取进度配置DataTable
        /// </summary>
        /// <param name="lngAppID">应用ID</param>
        /// <param name="lngOFlowModelID">流程模型ID</param>
        /// <param name="strOrderBy">排序方式 如 order by ID asc</param>
        /// <returns></returns>
        public static DataTable GetDataTable(long lngAppID, long lngOFlowModelID, string strOrderBy)
        {
            string strSQL = "select * from inf_transfer_set where AppID = " + lngAppID.ToString() + " and OFlowModelID = " + lngOFlowModelID.ToString() + " " + strOrderBy;
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            return dt;
        }
        /// <summary>
        /// 根据应用ID和流程模型ID获取进度配置DataTable
        /// </summary>
        /// <param name="lngAppID">应用ID</param>
        /// <param name="lngOFlowModelID">流程模型ID</param>
        /// <param name="strOrderBy">排序方式 如 order by ID asc</param>
        /// <returns></returns>
        public static DataTable GetDataTable(long lngAppID, string strOrderBy)
        {
            string strSQL = "select * from inf_transfer_set where AppID = " + lngAppID.ToString() + " " + strOrderBy;
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            return dt;
        }
        #endregion

        #region 根据应用ID和流程模型ID删除所有配置信息

        /// <summary>
        /// 根据应用ID和流程模型ID删除所有配置信息
        /// </summary>
        /// <param name="lngAppID">应用ID</param>
        /// <param name="lngOFlowModelID">流程模型ID</param>
        public static void DeleteAll(long lngAppID, long lngOFlowModelID)
        {
            string strSQL = "delete from inf_transfer_set where AppID=" + lngAppID.ToString() + " and OFlowModelID=" + lngOFlowModelID.ToString();
            CommonDP.ExcuteSql(strSQL);
        }
        /// <summary>
        /// 根据应用ID和流程模型ID删除所有配置信息
        /// </summary>
        /// <param name="lngAppID">应用ID</param>        
        public static void DeleteAll(long lngAppID)
        {
            string strSQL = "delete from inf_transfer_set where AppID=" + lngAppID.ToString();
            CommonDP.ExcuteSql(strSQL);
        }
        #endregion
    }
}
