




/*******************************************************************
 *
 * Description
 * 
 * 
 * Create By  :yxq
 * Create Date:2012年8月16日
 * *****************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;
using System.Data.OracleClient;
using System.Collections;
using System.Collections.Generic;


namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// 
    /// </summary>
    public class GS_CurSchedulesRuleDP
    {
        /// <summary>
        /// 
        /// </summary>
        public GS_CurSchedulesRuleDP()
        { }

        #region Property
        #region
        private Int64 mCURID;
        /// <summary>
        ///
        /// </summary>
        public Int64 CURID
        {
            get { return mCURID; }
            set { mCURID = value; }
        }
        #endregion

        #region 工程师Id
        private Int64 mENGINEERID;
        /// <summary>
        ///
        /// </summary>
        public Int64 ENGINEERID
        {
            get { return mENGINEERID; }
            set { mENGINEERID = value; }
        }
        #endregion

        #region 区间Id
        private Int64 mAREAID;
        /// <summary>
        ///
        /// </summary>
        public Int64 AREAID
        {
            get { return mAREAID; }
            set { mAREAID = value; }
        }
        #endregion

        #region 班次Id
        private Int64 mSCHEDULESID;
        /// <summary>
        ///
        /// </summary>
        public Int64 SCHEDULESID
        {
            get { return mSCHEDULESID; }
            set { mSCHEDULESID = value; }
        }
        #endregion

        #region 预期班次Id
        private Int64 mEXPECTSCHEDULESID;
        /// <summary>
        ///
        /// </summary>
        public Int64 EXPECTSCHEDULESID
        {
            get { return mEXPECTSCHEDULESID; }
            set { mEXPECTSCHEDULESID = value; }
        }
        #endregion

        #region 轮班Id
        private Int64 mTURNRULEID;
        /// <summary>
        ///
        /// </summary>
        public Int64 TURNRULEID
        {
            get { return mTURNRULEID; }
            set { mTURNRULEID = value; }
        }
        #endregion

        #region 上次排班的班次

        private String mPRESCHEDULESNAME = string.Empty;
        /// <summary>
        ///
        /// </summary>
        public String PRESCHEDULESNAME
        {
            get { return mPRESCHEDULESNAME; }
            set { mPRESCHEDULESNAME = value; }
        }
        #endregion

        #region 排班剩余的次数

        private Int64 mREMAINDERNUM;
        /// <summary>
        ///
        /// </summary>
        public Int64 REMAINDERNUM
        {
            get { return mREMAINDERNUM; }
            set { mREMAINDERNUM = value; }
        }
        #endregion

        #region 生成类型
        private Int64 mCRESTATUS;
        /// <summary>
        ///
        /// </summary>
        public Int64 CRESTATUS
        {
            get { return mCRESTATUS; }
            set { mCRESTATUS = value; }
        }
        #endregion

        #region
        private Int64 mDELETED;
        /// <summary>
        ///
        /// </summary>
        public Int64 DELETED
        {
            get { return mDELETED; }
            set { mDELETED = value; }
        }
        #endregion

        #region
        private String mCREBY = string.Empty;
        /// <summary>
        ///
        /// </summary>
        public String CREBY
        {
            get { return mCREBY; }
            set { mCREBY = value; }
        }
        #endregion

        #region
        private DateTime mCRETIME = DateTime.MinValue;
        /// <summary>
        ///
        /// </summary>
        public DateTime CRETIME
        {
            get { return mCRETIME; }
            set { mCRETIME = value; }
        }
        #endregion

        #region
        private String mLATMDYBY = string.Empty;
        /// <summary>
        ///
        /// </summary>
        public String LATMDYBY
        {
            get { return mLATMDYBY; }
            set { mLATMDYBY = value; }
        }
        #endregion

        #region
        private DateTime mLSTMDYTIME = DateTime.MinValue;
        /// <summary>
        ///
        /// </summary>
        public DateTime LSTMDYTIME
        {
            get { return mLSTMDYTIME; }
            set { mLSTMDYTIME = value; }
        }
        #endregion

        #region 上班类型 轮班/固定班次
        private Int64 mWORKCATEID;
        /// <summary>
        ///
        /// </summary>
        public Int64 WORKCATEID
        {
            get { return mWORKCATEID; }
            set { mWORKCATEID = value; }
        }
        #endregion

        #region 休息类型
        private string mRESTNAME;
        /// <summary>
        ///
        /// </summary>
        public string RESTNAME
        {
            get { return mRESTNAME; }
            set { mRESTNAME = value; }
        }
        #endregion

        #region 休息类型
        private string mRESTVALUE;
        /// <summary>
        ///
        /// </summary>
        public string RESTVALUE
        {
            get { return mRESTVALUE; }
            set { mRESTVALUE = value; }
        }
        #endregion

        #region 备注
        private String mREMARK = string.Empty;
        /// <summary>
        ///
        /// </summary>
        public String REMARK
        {
            get { return mREMARK; }
            set { mREMARK = value; }
        }
        #endregion

        #endregion

        #region GetReCorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>GS_CurSchedulesRuleDP</returns>
        public GS_CurSchedulesRuleDP GetReCorded(long lngID)
        {
            GS_CurSchedulesRuleDP ee = new GS_CurSchedulesRuleDP();
            string strSQL = string.Empty;
            strSQL = "SELECT * FROM GS_CURSCHEDULESRULE WHERE   CURID = " + lngID.ToString();
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            foreach (DataRow dr in dt.Rows)
            {
                ee.CURID = Int64.Parse(dr["CURID"].ToString());
                ee.ENGINEERID = Int64.Parse(dr["ENGINEERID"].ToString());
                ee.AREAID = Int64.Parse(dr["AREAID"].ToString());
                ee.SCHEDULESID = Int64.Parse(dr["SCHEDULESID"].ToString());
                ee.EXPECTSCHEDULESID = Int64.Parse(dr["EXPECTSCHEDULESID"].ToString());
                ee.TURNRULEID = Int64.Parse(dr["TURNRULEID"].ToString());
                ee.PRESCHEDULESNAME = dr["PRESCHEDULESNAME"].ToString();
                ee.REMAINDERNUM = Int64.Parse(dr["REMAINDERNUM"].ToString());
                ee.CRESTATUS = Int64.Parse(dr["CRESTATUS"].ToString());
                ee.DELETED = Int64.Parse(dr["DELETED"].ToString());
                ee.CREBY = dr["CREBY"].ToString();
                ee.CRETIME = dr["CRETIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["CRETIME"].ToString());
                ee.LATMDYBY = dr["LATMDYBY"].ToString();
                ee.LSTMDYTIME = dr["LSTMDYTIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["LSTMDYTIME"].ToString());
                ee.WORKCATEID = Int64.Parse(dr["WORKCATEID"].ToString());
                ee.RESTNAME = dr["RESTNAME"].ToString();
                ee.RESTVALUE = dr["RESTVALUE"].ToString();
                ee.REMARK = dr["REMARK"].ToString();
            }
            return ee;
        }
        #endregion

        #region GetDataTable
        /// <summary>
        /// GetDataTable
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="rowcount"></param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string sWhere, string sOrder, int pagesize, int pageindex, ref int rowcount)
        {
            DataTable dt = CommonDP.ExcuteSqlTablePage("GS_CURSCHEDULESRULE", "*", sWhere, sOrder, pagesize, pageindex, ref rowcount);
            return dt;
        }
        #endregion

        #region InsertRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pGS_CurSchedulesRuleDP></param>
        public void InsertRecorded(GS_CurSchedulesRuleDP mod)
        {
            string strSQL = string.Empty;
            try
            {
                strSQL = @"INSERT INTO GS_CURSCHEDULESRULE(
									CURID,
									ENGINEERID,
									AREAID,
									SCHEDULESID,
									EXPECTSCHEDULESID,
									TURNRULEID,
									PRESCHEDULESNAME,
									REMAINDERNUM,
									CRESTATUS,
									DELETED,
									CREBY,
									CRETIME,
									LATMDYBY,
									LSTMDYTIME,
									WORKCATEID,
									RESTNAME,
									RESTVALUE,
									REMARK
					)
					VALUES( " +
 " GS_CURSCHEDULESRULE_SEQ.nextval  ," +
                            mod.ENGINEERID.ToString().ToString() + "," +
                            mod.AREAID.ToString().ToString() + "," +
                            mod.SCHEDULESID.ToString().ToString() + "," +
                            mod.EXPECTSCHEDULESID.ToString().ToString() + "," +
                            mod.TURNRULEID.ToString().ToString() + "," +
                            StringTool.SqlQ(mod.PRESCHEDULESNAME.ToString()) + "," +
                            mod.REMAINDERNUM.ToString().ToString() + "," +
                            mod.CRESTATUS.ToString().ToString() + "," +
                            mod.DELETED.ToString().ToString() + "," +
                            StringTool.SqlQ(mod.CREBY.ToString()) + "," +
                            (mod.CRETIME.ToString() == DateTime.MinValue.ToString() ? " null " : StringTool.SqlQ(mod.CRETIME.ToString().ToString())) + "," +
                            StringTool.SqlQ(mod.LATMDYBY.ToString()) + "," +
                            (mod.LSTMDYTIME.ToString() == DateTime.MinValue.ToString() ? " null " : StringTool.SqlQ(mod.LSTMDYTIME.ToString().ToString())) + "," +
                            mod.WORKCATEID.ToString().ToString() + "," +
                            StringTool.SqlQ(mod.RESTNAME.ToString()) + "," +
                            StringTool.SqlQ(mod.RESTVALUE.ToString()) + "," +
                            StringTool.SqlQ(mod.REMARK.ToString()) +
                    ")";

                CommonDP.ExcuteSql(strSQL);
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region UpdateRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pGS_CurSchedulesRuleDP></param>
        public string UpdateRecorded(GS_CurSchedulesRuleDP mod)
        {
            string strSQL = string.Empty;

            strSQL = @"UPDATE GS_CURSCHEDULESRULE Set " +
                                                        " ENGINEERID = " + mod.ENGINEERID.ToString() + "," +
                            " AREAID = " + mod.AREAID.ToString() + "," +
                            " SCHEDULESID = " + mod.SCHEDULESID.ToString() + "," +
                            " EXPECTSCHEDULESID = " + mod.EXPECTSCHEDULESID.ToString() + "," +
                            " TURNRULEID = " + mod.TURNRULEID.ToString() + "," +
                            " PRESCHEDULESNAME = " + StringTool.SqlQ(mod.PRESCHEDULESNAME) + "," +
                            " REMAINDERNUM = " + mod.REMAINDERNUM.ToString() + "," +
                            " CRESTATUS = " + mod.CRESTATUS.ToString() + "," +
                            " DELETED = " + mod.DELETED.ToString() + "," +
                            " CREBY = " + StringTool.SqlQ(mod.CREBY) + "," +
                            " CRETIME = " + StringTool.SqlDate(mod.CRETIME) + "," +
                            " LATMDYBY = " + StringTool.SqlQ(mod.LATMDYBY) + "," +
                            " LSTMDYTIME = " + StringTool.SqlDate(mod.LSTMDYTIME) + "," +
                            " workcateid = " + mod.WORKCATEID.ToString() +
                            
                                " WHERE CURID = " + mod.CURID.ToString();
            return strSQL;

        }
        #endregion

        #region DeleteRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        public void DeleteRecorded(long lngID)
        {
            try
            {
                string strSQL = "Update GS_CURSCHEDULESRULE Set Deleted=1  WHERE CURID =" + lngID.ToString();
                CommonDP.ExcuteSql(strSQL);
            }
            catch
            {
                throw;
            }
        }
        #endregion

        /// <summary>
        /// 得到当期的排班表
        /// </summary>
        /// <param name="AreaId"></param>
        /// <returns></returns>
        public DataTable GetScheduleTableByAreaId(long AreaId)
        {
            DataTable dt = null;
            OracleParameter[] parms = {
                      new OracleParameter("P_AREAID",OracleType.Number ),     
                      new OracleParameter("P_OUTTABLE",OracleType.Cursor)
                };

            parms[0].Direction = ParameterDirection.Input;
            parms[1].Direction = ParameterDirection.Output;

            parms[0].Value = AreaId;

            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            try
            {
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_Schedule_DISP_BUDGET", parms);
            }
            catch (Exception ex)
            {
                E8Logger.Error(ex);
                throw;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }

            return dt;

        }

        public void DeleteRecordedByArea(long AreaId)
        {
            OracleParameter[] parms = {
                      new OracleParameter("P_AREAID",OracleType.Number )                      
                };

            parms[0].Direction = ParameterDirection.Input;
            parms[0].Value = AreaId;

            OracleDbHelper.ExecuteNonQuery(ConfigTool.GetConnectString(), CommandType.StoredProcedure, "PROC_Schedule_DELETE", parms);
            //string strSQL = "Update GS_CURSCHEDULESRULE Set Deleted=1  WHERE CRESTATUS=0 AND AreaId =" + AreaId.ToString();
            //CommonDP.ExcuteSql(strSQL);
        }

        /// <summary>
        /// 是否已经排班
        /// </summary>
        /// <param name="AreaId"></param>
        /// <returns></returns>
        public bool HavedCreateWorkIssues(long AreaId)
        {
            string strSQL = "SELECT nvl(count(*),0) FROM GS_CURSCHEDULESRULE where Deleted=0 and  CRESTATUS=0 AND AreaId =" + AreaId.ToString();
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            try
            {
                object obj = OracleDbHelper.ExecuteScalar(cn, CommandType.Text, strSQL);
                if (obj == DBNull.Value || obj == null)
                {
                    return false;
                }
                return int.Parse(obj.ToString()) > 0;
                //DataTable  dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                //return dt.Rows.Count > 0;
            }
            catch (Exception ex)
            {
                E8Logger.Error(ex);
                throw;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);

            }
        }

        /// <summary>
        /// 建立排班
        /// </summary>
        /// <param name="AreaId"></param>
        public void CreateWorkIssues(long AreaId)
        {

            OracleParameter[] parms = {
                      new OracleParameter("P_AREAID",OracleType.Number )                     
                };

            parms[0].Direction = ParameterDirection.Input;
            parms[0].Value = AreaId;

            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            try
            {
                OracleDbHelper.ExecuteNonQuery(cn, CommandType.StoredProcedure, "PROC_Schedule_CREATERULE", parms);
            }
            catch (Exception ex)
            {
                E8Logger.Error(ex);
                throw;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }

        }

        /// <summary>
        /// 给每个人排班
        /// </summary>
        /// <param name="AreaId"></param>
        public void CreateEveryOneWorkIssues(long AreaId)
        {

            OracleParameter[] parms = {
                      new OracleParameter("P_AREAID",OracleType.Number )                     
                };

            parms[0].Direction = ParameterDirection.Input;
            parms[0].Value = AreaId;

            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            try
            {
                OracleDbHelper.ExecuteNonQuery(cn, CommandType.StoredProcedure, "PROC_Schedule_EachEngineer", parms);
            }
            catch (Exception ex)
            {
                E8Logger.Error(ex);
                throw;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }

        }


        public GS_CurSchedulesRuleDP GetReCorded(long AreaId, long EngineerId)
        {
            GS_CurSchedulesRuleDP ee = new GS_CurSchedulesRuleDP();
            string strSQL = string.Empty;
            strSQL = "SELECT * FROM GS_CURSCHEDULESRULE WHERE deleted = 0 and AreaId = " + AreaId.ToString() + " and engineerId=" + EngineerId.ToString();
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            foreach (DataRow dr in dt.Rows)
            {
                ee.CURID = long.Parse(dr["CURID"].ToString());
                ee.ENGINEERID = long.Parse(dr["ENGINEERID"].ToString());
                ee.AREAID = long.Parse(dr["AREAID"].ToString());
                ee.SCHEDULESID = long.Parse(dr["SCHEDULESID"].ToString());
                ee.EXPECTSCHEDULESID = dr["EXPECTSCHEDULESID"].ToString() == string.Empty ? 0 : long.Parse(dr["EXPECTSCHEDULESID"].ToString());
                ee.TURNRULEID = long.Parse(dr["TURNRULEID"].ToString());
                ee.PRESCHEDULESNAME = dr["PRESCHEDULESNAME"].ToString();
                ee.REMAINDERNUM = long.Parse(dr["REMAINDERNUM"].ToString());
                ee.CRESTATUS = long.Parse(dr["CRESTATUS"].ToString());
                ee.DELETED = long.Parse(dr["DELETED"].ToString());
                ee.CREBY = dr["CREBY"].ToString();
                ee.CRETIME = dr["CRETIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["CRETIME"].ToString());
                ee.LATMDYBY = dr["LATMDYBY"].ToString();
                ee.LSTMDYTIME = dr["LSTMDYTIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["LSTMDYTIME"].ToString());
                ee.WORKCATEID = long.Parse(dr["WORKCATEID"].ToString());
            }
            return ee;
        }


    }
}

