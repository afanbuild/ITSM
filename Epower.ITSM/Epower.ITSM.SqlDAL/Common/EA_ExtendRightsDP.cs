/*******************************************************************
 *
 * Description:业务扩展权限数据处理
 * 
 * 
 * Create By  :zmc
 * Create Date:2008年11月25日
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
    /// <summary>
    /// 
    /// </summary>
    public class EA_ExtendRightsDP
    {
        /// <summary>
        /// 
        /// </summary>
        public EA_ExtendRightsDP()
        { }

        #region Property
        #region RightID
        /// <summary>
        ///
        /// </summary>
        private Decimal mRightID;
        public Decimal RightID
        {
            get { return mRightID; }
            set { mRightID = value; }
        }
        #endregion

        #region OperateType
        /// <summary>
        ///
        /// </summary>
        private Int32 mOperateType;
        public Int32 OperateType
        {
            get { return mOperateType; }
            set { mOperateType = value; }
        }
        #endregion

        #region OperateID
        /// <summary>
        ///
        /// </summary>
        private Int32 mOperateID;
        public Int32 OperateID
        {
            get { return mOperateID; }
            set { mOperateID = value; }
        }
        #endregion

        #region ObjectID
        /// <summary>
        ///
        /// </summary>
        private Decimal mObjectID;
        public Decimal ObjectID
        {
            get { return mObjectID; }
            set { mObjectID = value; }
        }
        #endregion

        #region ObjectType
        /// <summary>
        ///
        /// </summary>
        private Int32 mObjectType;
        public Int32 ObjectType
        {
            get { return mObjectType; }
            set { mObjectType = value; }
        }
        #endregion

        #region RightValue
        /// <summary>
        ///
        /// </summary>
        private Int32 mRightValue;
        public Int32 RightValue
        {
            get { return mRightValue; }
            set { mRightValue = value; }
        }
        #endregion

        #region RightValue
        /// <summary>
        ///
        /// </summary>
        private string mObjectName;
        public string ObjectName
        {
            get { return mObjectName; }
            set { mObjectName = value; }
        }
        #endregion

        #endregion

        #region GetReCorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>EA_ExtendRightsDP</returns>
        public EA_ExtendRightsDP GetReCorded(long lngID)
        {
            EA_ExtendRightsDP ee = new EA_ExtendRightsDP();
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = @"SELECT EA_ExtendRights.*," +
                        "case EA_ExtendRights.ObjectType  " +
                        "  when 10 then (SELECT DeptName FROM Ts_Dept WHERE ROWNUM<=1 AND DeptID=ObjectId) " +
                        " when 20 then (SELECT Name FROM Ts_User WHERE ROWNUM<=1 AND UserID=ObjectId) " +
                        " when 30 then (SELECT ActorName FROM Ts_Actors WHERE ROWNUM<=1 AND ActorID=ObjectId) end as ObjectName " +
                        " FROM EA_ExtendRights WHERE RightID = " + lngID.ToString();
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                
                foreach (DataRow dr in dt.Rows)
                {
                    ee.RightID = Decimal.Parse(dr["RightID"].ToString());
                    ee.OperateType = Int32.Parse(dr["OperateType"].ToString());
                    ee.OperateID = Int32.Parse(dr["OperateID"].ToString());
                    ee.ObjectID = Decimal.Parse(dr["ObjectID"].ToString());
                    ee.ObjectType = Int32.Parse(dr["ObjectType"].ToString());
                    ee.RightValue = Int32.Parse(dr["RightValue"].ToString());
                    ee.ObjectName = dr["ObjectName"].ToString();
                }
                return ee;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion

        #region GetDataTable
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string sWhere, string sOrder)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT * FROM EA_ExtendRights Where 1=1 ";
                strSQL += sWhere;
                strSQL += sOrder;
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            
                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion

        #region InsertRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pEA_ExtendRightsDP></param>
        public void InsertRecorded(EA_ExtendRightsDP pEA_ExtendRightsDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            try
            {
                string strID = EpowerGlobal.EPGlobal.GetNextID("EA_ExtendRightsID").ToString();
                pEA_ExtendRightsDP.RightID = decimal.Parse(strID);

                strSQL = @"INSERT INTO EA_ExtendRights(
                                    RightID,
									OperateType,
									OperateID,
									ObjectID,
									ObjectType,
									RightValue
					)
					VALUES( " + strID +","+
                            pEA_ExtendRightsDP.OperateType.ToString() + "," +
                            pEA_ExtendRightsDP.OperateID.ToString() + "," +
                            pEA_ExtendRightsDP.ObjectID.ToString() + "," +
                            pEA_ExtendRightsDP.ObjectType.ToString() + "," +
                            pEA_ExtendRightsDP.RightValue.ToString() +
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

        #region UpdateRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pEA_ExtendRightsDP></param>
        public void UpdateRecorded(EA_ExtendRightsDP pEA_ExtendRightsDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE EA_ExtendRights Set " +
                                                        " OperateType = " + pEA_ExtendRightsDP.OperateType.ToString() + "," +
                            " OperateID = " + pEA_ExtendRightsDP.OperateID.ToString() + "," +
                            " ObjectID = " + pEA_ExtendRightsDP.ObjectID.ToString() + "," +
                            " ObjectType = " + pEA_ExtendRightsDP.ObjectType.ToString() + "," +
                            " RightValue = " + pEA_ExtendRightsDP.RightValue.ToString() +
                                " WHERE RightID = " + pEA_ExtendRightsDP.RightID.ToString();

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
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        public void DeleteRecorded(long lngID)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                string strSQL = "Delete EA_ExtendRights WHERE RightID =" + lngID.ToString();
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

        #region getUserOtherRight 取得业务权限列表
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngUserId"></param>
        /// <returns></returns>
        public static string getUserOtherRight(int OperateType,long lngUserId)
        {
            string sReturn = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            OracleTransaction trans = cn.BeginTransaction();

            OracleDataReader dr = null;
            try
            {
                #region 获取用户权限

                OracleCommand cmd1 = new OracleCommand("GetUserOtherRight", cn, trans);
                cmd1.CommandType = CommandType.StoredProcedure;

                cmd1.Parameters.Add("p_UserID", OracleType.Number);
                cmd1.Parameters.Add("p_TableName", OracleType.NVarChar, 100);
                cmd1.Parameters.Add("p_OperateType", OracleType.Number);
                cmd1.Parameters.Add("P_GetData", OracleType.Cursor);

                cmd1.Parameters["p_UserID"].Direction = ParameterDirection.Input;
                cmd1.Parameters["p_TableName"].Direction = ParameterDirection.Output;
                cmd1.Parameters["p_OperateType"].Direction = ParameterDirection.Input;
                cmd1.Parameters["P_GetData"].Direction = ParameterDirection.Output;
                cmd1.Parameters["p_UserID"].Value = lngUserId;
                cmd1.Parameters["p_OperateType"].Value = OperateType;

                dr = cmd1.ExecuteReader();

                while (dr.Read())
                {
                    sReturn += StringTool.SqlQ(dr["OperateID"].ToString()) + ",";
                }
                if (sReturn == string.Empty)
                {
                    sReturn = "''";
                }
                else
                {
                    sReturn = sReturn.Substring(0, sReturn.Length - 1);
                }

                #endregion

                string tablename = cmd1.Parameters["p_TableName"].Value.ToString();//临时表名

                #region 删除临时表

                OracleCommand cmd2 = new OracleCommand("PROC_DELUSERRIGHTVALUETABLE", cn, trans);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.Add("p_TableName", OracleType.NVarChar, 50);

                cmd2.Parameters["p_TableName"].Direction = ParameterDirection.Input;
                cmd2.Parameters["p_TableName"].Value = tablename;
                cmd2.ExecuteNonQuery();

                #endregion

                trans.Commit();
            }
            finally
            {
                if (dr != null) dr.Close();
            }
            return sReturn;
        }
        #endregion 

        #region [GetRightInfo(string sWhere)]
        /// <summary>
        /// 通用获取操作权限列表的方法
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetRightInfo(string sWhere)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string sSql = @"SELECT a.RightID as RightID,ObjectID,
              case a.ObjectType 
                when 10 then (SELECT DeptName||'('||to_char(DeptID)||')' FROM Ts_Dept WHERE DeptID=a.ObjectId AND ROWNUM<=1) 
                when 20 then (SELECT Name||'('||to_char(UserID)||')' FROM Ts_User WHERE UserID=a.ObjectId AND ROWNUM<=1) 
                when 30 then (SELECT ActorName||'('||to_char(ActorID)||')' FROM Ts_Actors WHERE ActorID=a.ObjectId AND ROWNUM<=1) end as ObjectName,
              a.RightValue as RightValue ,
              case a.ObjectType when 10 then '部门' when 20 then '人员' when 30 then '用户组' end as ObjectType
            FROM EA_ExtendRights a
            WHERE 1=1 " + sWhere +
                        "";
          

            DataTable dt;
            try
            {
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sSql);
            }
            catch (OracleException sqle)
            {
                throw new EpowerException("获取权限时出现错误", "EA_ExtendRightsDP.cs/[public DataTable GetRightInfo]", sqle.Message.ToString());
            }
            catch (Exception ex)
            {
                throw new EpowerException("获取权限时出现错误", "EA_ExtendRightsDP.cs/[public DataTable GetRightInfo]", ex.Message.ToString());
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
            return dt;
        }
        #endregion

        #region Get_RightID
        /// <summary>
        /// 
        /// </summary>
        /// <param name="OperateType"></param>
        /// <param name="OperateID"></param>
        /// <param name="ObjectID"></param>
        /// <param name="ObjectType"></param>
        /// <returns></returns>
        public string Get_RightID(int OperateType, string OperateID, string ObjectID, string ObjectType)
        {
            string strSQL = "SELECT RightID FROM EA_ExtendRights WHERE ROWNUM<=1 AND OperateType={0} and OperateID={1} and ObjectID={2} and ObjectType={3}";

            strSQL = String.Format(strSQL,OperateType, OperateID, ObjectID, ObjectType);

            OracleConnection cn = ConfigTool.GetConnection();


            try
            {
                object obj = OracleDbHelper.ExecuteScalar(cn, CommandType.Text, strSQL);
                
                if (obj != null)
                    return obj.ToString();
                else
                    return "0";
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion 
    }
}

