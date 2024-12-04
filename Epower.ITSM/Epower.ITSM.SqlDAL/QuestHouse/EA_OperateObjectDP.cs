/*******************************************************************
 * Description:操作对象
 * Create By  :oyangby
 * Create Date:2011年7月25日
 * *****************************************************************/
using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;

namespace Epower.ITSM.SqlDAL
{
    public class EA_OperateObjectDP
    {
        #region EA_OperateObjectDP

        /// <summary>
        /// 构造函数
        /// </summary>
        public EA_OperateObjectDP()
        { }

        #endregion

        #region Property

        #region ID
        /// <summary>
        /// 对象ID
        /// </summary>
        private Decimal mID;
        public Decimal ID
        {
            get { return mID; }
            set { mID = value; }
        }
        #endregion

        #region ObjName
        /// <summary>
        /// 对象名称
        /// </summary>
        private String mObjName;
        public String ObjName
        {
            get { return mObjName; }
            set { mObjName = value; }
        }
        #endregion

        #region RegUserId
        /// <summary>
        ///对应用户ID
        /// </summary>
        private Decimal mRegUserId;
        public Decimal RegUserId
        {
            get { return mRegUserId; }
            set { mRegUserId = value; }
        }
        #endregion

        #region RegUserName
        /// <summary>
        /// 对应用户
        /// </summary>
        private String mRegUserName;
        public String RegUserName
        {
            get { return mRegUserName; }
            set { mRegUserName = value; }
        }
        #endregion

        #region Deleted
        /// <summary>
        /// 是否删除
        /// </summary>
        private eRecord_Status mDeleted;
        public eRecord_Status Deleted
        {
            get { return mDeleted; }
            set { mDeleted = value; }
        }
        #endregion

        #endregion

        #region GetDataTable
        /// <summary>
        /// 获取所有操作对象
        /// </summary>  
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string sWhere, string sOrder)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");
            strSQL = "SELECT * FROM EA_OperateObject Where 1=1 And Deleted=" + ((int)Deleted).ToString();
            strSQL += sWhere;
            strSQL += sOrder;
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region InsertRecorded

        /// <summary>
        /// 添加操作对象
        /// </summary>
        /// <param name=pEA_OperateObjectDP>添加对象</param>
        public void InsertRecorded(EA_OperateObjectDP pEA_OperateObjectDP)
        {
            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");
            string strSQL = string.Empty;
            string strID = "0";
            try
            {
                strID = EpowerGlobal.EPGlobal.GetNextID("EA_OperateObjectID").ToString();
                pEA_OperateObjectDP.ID = decimal.Parse(strID);
                strSQL = @"INSERT INTO EA_OperateObject(
									ID,
									ObjName,
									RegUserId,
									RegUserName,
									Deleted
					)
					VALUES( " +
                            strID.ToString() + "," +
                            StringTool.SqlQ(pEA_OperateObjectDP.ObjName) + "," +
                            pEA_OperateObjectDP.RegUserId.ToString() + "," +
                            StringTool.SqlQ(pEA_OperateObjectDP.RegUserName) + "," +
                           ((int)pEA_OperateObjectDP.Deleted).ToString() +
                    ")";

                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSQL);
            }
            catch
            {
                throw;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }
        #endregion

        #region DeleteRecorded
        /// <summary>
        /// 删除操作对象
        /// </summary>
        /// <param name="lngID">删除ID</param>
        public void DeleteRecorded(long lngID)
        {
            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");
            try
            {
                string strSQL = "Update EA_OperateObject Set Deleted=1  WHERE ID =" + lngID.ToString();
                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSQL);
            }
            catch
            {
                throw;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }
        #endregion
    }
}

