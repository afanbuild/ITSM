/****************************************************************************
 * 
 * description:进度条设置操作类
 * 
 * 
 * 
 * Create by: 余向前
 * Create Date:2013-04-25
 * *************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.SqlDAL
{
    public class BR_ProgressBarDP
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

        #region NodeModelID
        private long mNodeModelID;
        /// <summary>
        /// 环节模型ID
        /// </summary>
        public long NodeModelID
        {
            set
            {
                mNodeModelID = value;
            }
            get
            {
                return mNodeModelID;
            }
        }
        #endregion

        #region NodeName
        private string mNodeName;
        /// <summary>
        /// 环节名称
        /// </summary>
        public string NodeName
        {
            get { return mNodeName; }
            set { mNodeName = value; }
        }
        #endregion

        #region ImgURL
        private string mImgURL;
        /// <summary>
        /// 进度条图片地址
        /// </summary>
        public string ImgURL
        {
            get { return mImgURL; }
            set { mImgURL = value; }
        }
        #endregion

        #endregion

        public BR_ProgressBarDP()
        {

        }

        #region 查询某条记录
        /// <summary>
        /// 查询某条记录
        /// </summary>
        /// <param name="lngAppID">应用ID</param>
        /// <param name="lngOFlowModelID">流程模型ID</param>
        /// <param name="lngNodeModelID">环节模型ID</param>
        /// <returns></returns>
        public BR_ProgressBarDP GetRecorded(long lngAppID, long lngOFlowModelID, long lngNodeModelID)
        {
            BR_ProgressBarDP ee = new BR_ProgressBarDP();

            string strSQL = string.Empty;
            strSQL = "SELECT * FROM BR_ProgressBar WHERE rownum<=1 and AppID = " + lngAppID.ToString() + " and OFlowModelID = " + lngOFlowModelID.ToString() + " and NodeModelID=" + lngNodeModelID.ToString();
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            foreach (DataRow dr in dt.Rows)
            {
                ee.ID = CTools.ToInt64(dr["ID"].ToString());
                ee.AppID = CTools.ToInt64(dr["AppID"].ToString());
                ee.OFlowModelID =CTools.ToInt64(dr["OFlowModelID"].ToString());
                ee.NodeModelID = CTools.ToInt64(dr["NodeModelID"].ToString());
                ee.NodeName = dr["NodeName"].ToString();
                ee.ImgURL = dr["ImgURL"].ToString();                 
            }
            return ee;
        }
        #endregion

        #region 添加记录
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="ee"></param>
        public void InsertRecorded(BR_ProgressBarDP pBR_ProgressBarDP)
        {
            string strSQL = string.Empty;
            string strID = "0";
            try
            {
                strID = EpowerGlobal.EPGlobal.GetNextID("BR_ProgressBarID").ToString();
                pBR_ProgressBarDP.ID = long.Parse(strID);
                strSQL = @"INSERT INTO BR_ProgressBar(
									ID,
									AppID,
									OFlowModelID,
									NodeModelID,
									NodeName,
									ImgURL
                            )
					VALUES( " +
                            strID.ToString() + "," +
                            pBR_ProgressBarDP.AppID + "," +
                            pBR_ProgressBarDP.OFlowModelID + "," +
                            pBR_ProgressBarDP.NodeModelID + "," +
                            StringTool.SqlQ(pBR_ProgressBarDP.NodeName) + "," +
                            StringTool.SqlQ(pBR_ProgressBarDP.ImgURL) +
                    ")";

                CommonDP.ExcuteSql(strSQL);
            }
            catch(Exception ex)
            {
                E8Logger.Debug("保存进度配置数据出错" + ex.Message + ",数据库语句:" + strSQL);
            }
        }
        #endregion

        #region 根据应用ID和流程模型ID获取进度配置DataTable
        /// <summary>
        /// 根据应用ID和流程模型ID获取进度配置DataTable
        /// </summary>
        /// <param name="lngAppID">应用ID</param>
        /// <param name="lngOFlowModelID">流程模型ID</param>
        /// <returns></returns>
        public static DataTable GetDataTable(long lngAppID, long lngOFlowModelID)
        {
            string strSQL = "select a.*, 0 IsChangeImg,'' as FileName,'' as UpFile from BR_ProgressBar a where AppID = " + lngAppID.ToString() + " and OFlowModelID = " + lngOFlowModelID.ToString();
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
            string strSQL = "delete from BR_ProgressBar where AppID=" + lngAppID.ToString() + " and OFlowModelID=" + lngOFlowModelID.ToString();
            CommonDP.ExcuteSql(strSQL);
        }
        #endregion
    }
}
