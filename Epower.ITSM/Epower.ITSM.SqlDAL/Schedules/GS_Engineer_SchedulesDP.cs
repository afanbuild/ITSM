



/*******************************************************************
 *
 * Description
 * 
 * 
 * Create By  :yxq
 * Create Date:2012年8月20日
 * *****************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;
using System.Collections;
using System.Collections.Generic;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// 
    /// </summary>
    public class GS_Engineer_SchedulesDP
    {
        /// <summary>
        /// 
        /// </summary>
        public GS_Engineer_SchedulesDP()
        { }

        #region Property
        #region 工程师排班Id
        private Int64 mESID;
        /// <summary>
        ///
        /// </summary>
        public Int64 ESID
        {
            get { return mESID; }
            set { mESID = value; }
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

        #region 班次ID
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

        #region 工作天
        private DateTime mWORKDATE = DateTime.MinValue;
        /// <summary>
        ///
        /// </summary>
        public DateTime WORKDATE
        {
            get { return mWORKDATE; }
            set { mWORKDATE = value; }
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
        private Int64 mRESTFLAG;
        /// <summary>
        ///
        /// </summary>
        public Int64 RESTFLAG
        {
            get { return mRESTFLAG; }
            set { mRESTFLAG = value; }
        }
        #endregion

        #region 开始日期
        private DateTime mSTARTDAYTIME = DateTime.MinValue;
        /// <summary>
        ///
        /// </summary>
        public DateTime STARTDAYTIME
        {
            get { return mSTARTDAYTIME; }
            set { mSTARTDAYTIME = value; }
        }
        #endregion

        #region 中午开始上班时间
        private DateTime mSTARTSEGMENTTIME = DateTime.MinValue;
        /// <summary>
        ///
        /// </summary>
        public DateTime STARTSEGMENTTIME
        {
            get { return mSTARTSEGMENTTIME; }
            set { mSTARTSEGMENTTIME = value; }
        }
        #endregion

        #region 中午下班时间
        private DateTime mENDSEGMENTTIME = DateTime.MinValue;
        /// <summary>
        ///
        /// </summary>
        public DateTime ENDSEGMENTTIME
        {
            get { return mENDSEGMENTTIME; }
            set { mENDSEGMENTTIME = value; }
        }
        #endregion

        #region 下班时间
        private DateTime mENDDAYTIME = DateTime.MinValue;
        /// <summary>
        ///
        /// </summary>
        public DateTime ENDDAYTIME
        {
            get { return mENDDAYTIME; }
            set { mENDDAYTIME = value; }
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

        #endregion

        public string EngineerName
        { set; get; }

        public string ScheduleName
        { set; get; }

        public string DeptName
        { set; get; }

        #region GetReCorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>GS_Engineer_SchedulesDP</returns>
        public GS_Engineer_SchedulesDP GetReCorded(long lngID)
        {
            GS_Engineer_SchedulesDP ee = new GS_Engineer_SchedulesDP();
            string strSQL = string.Empty;
            strSQL = "SELECT * FROM GS_ENGINEER_SCHEDULES WHERE   ESID = " + lngID.ToString();
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            foreach (DataRow dr in dt.Rows)
            {
                ee.ESID = Int64.Parse(dr["ESID"].ToString());
                ee.ENGINEERID = Int64.Parse(dr["ENGINEERID"].ToString());
                ee.SCHEDULESID = Int64.Parse(dr["SCHEDULESID"].ToString());
                ee.WORKDATE = dr["WORKDATE"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["WORKDATE"].ToString());
                ee.CRESTATUS = Int64.Parse(dr["CRESTATUS"].ToString());
                ee.RESTFLAG = Int64.Parse(dr["RESTFLAG"].ToString());
                ee.STARTDAYTIME = dr["STARTDAYTIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["STARTDAYTIME"].ToString());
                ee.STARTSEGMENTTIME = dr["STARTSEGMENTTIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["STARTSEGMENTTIME"].ToString());
                ee.ENDSEGMENTTIME = dr["ENDSEGMENTTIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["ENDSEGMENTTIME"].ToString());
                ee.ENDDAYTIME = dr["ENDDAYTIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["ENDDAYTIME"].ToString());
                ee.DELETED = Int64.Parse(dr["DELETED"].ToString());
                ee.CREBY = dr["CREBY"].ToString();
                ee.CRETIME = dr["CRETIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["CRETIME"].ToString());
                ee.LATMDYBY = dr["LATMDYBY"].ToString();
                ee.LSTMDYTIME = dr["LSTMDYTIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["LSTMDYTIME"].ToString());
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
            DataTable dt = CommonDP.ExcuteSqlTablePage("GS_ENGINEER_SCHEDULES", "*", sWhere, sOrder, pagesize, pageindex, ref rowcount);
            return dt;
        }
        #endregion

        #region InsertRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pGS_Engineer_SchedulesDP></param>
        public void InsertRecorded(GS_Engineer_SchedulesDP mod)
        {
            string strSQL = string.Empty;
            try
            {
                string strID = EpowerGlobal.EPGlobal.GetNextID("GS_ENGINEER_SCHEDULES_SEQ").ToString();

                strSQL = @"INSERT INTO GS_ENGINEER_SCHEDULES(
									ESID,
									ENGINEERID,
									SCHEDULESID,
									WORKDATE,
									CRESTATUS,
									RESTFLAG,
									STARTDAYTIME,
									STARTSEGMENTTIME,
									ENDSEGMENTTIME,
									ENDDAYTIME,
									DELETED,
									CREBY,
									CRETIME,
									LATMDYBY,
									LSTMDYTIME
					)
					VALUES( " +
                            strID + "," +
                            mod.ENGINEERID.ToString().ToString() + "," +
                            mod.SCHEDULESID.ToString().ToString() + "," +
                            (mod.WORKDATE.ToString() == DateTime.MinValue.ToString() ? " null " : StringTool.SqlQ(mod.WORKDATE.ToString().ToString())) + "," +
                            mod.CRESTATUS.ToString().ToString() + "," +
                            mod.RESTFLAG.ToString().ToString() + "," +
                            (mod.STARTDAYTIME.ToString() == DateTime.MinValue.ToString() ? " null " : StringTool.SqlQ(mod.STARTDAYTIME.ToString().ToString())) + "," +
                            (mod.STARTSEGMENTTIME.ToString() == DateTime.MinValue.ToString() ? " null " : StringTool.SqlQ(mod.STARTSEGMENTTIME.ToString().ToString())) + "," +
                            (mod.ENDSEGMENTTIME.ToString() == DateTime.MinValue.ToString() ? " null " : StringTool.SqlQ(mod.ENDSEGMENTTIME.ToString().ToString())) + "," +
                            (mod.ENDDAYTIME.ToString() == DateTime.MinValue.ToString() ? " null " : StringTool.SqlQ(mod.ENDDAYTIME.ToString().ToString())) + "," +
                            mod.DELETED.ToString().ToString() + "," +
                            StringTool.SqlQ(mod.CREBY.ToString()) + "," +
                            (mod.CRETIME.ToString() == DateTime.MinValue.ToString() ? " null " : StringTool.SqlQ(mod.CRETIME.ToString().ToString())) + "," +
                            StringTool.SqlQ(mod.LATMDYBY.ToString()) + "," +
                            (mod.LSTMDYTIME.ToString() == DateTime.MinValue.ToString() ? " null " : StringTool.SqlQ(mod.LSTMDYTIME.ToString().ToString())) +
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
        /// <param name=pGS_Engineer_SchedulesDP></param>
        public void UpdateRecorded(GS_Engineer_SchedulesDP mod)
        {
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE GS_ENGINEER_SCHEDULES Set " +
                                                        " ENGINEERID = " + mod.ENGINEERID.ToString() + "," +
                            " SCHEDULESID = " + mod.SCHEDULESID.ToString() + "," +
                            //" WORKDATE = " + StringTool.SqlDate(mod.WORKDATE) + "," +
                            " WORKDATE = TO_Date('" + mod.WORKDATE.ToShortDateString() + "','yyyy-mm-dd')," +
                            " CRESTATUS = " + mod.CRESTATUS.ToString() + "," +
                            " RESTFLAG = " + mod.RESTFLAG.ToString() + "," +
                            " STARTDAYTIME = " + StringTool.SqlDate(mod.STARTDAYTIME) + "," +
                            " STARTSEGMENTTIME = " + StringTool.SqlDate(mod.STARTSEGMENTTIME) + "," +
                            " ENDSEGMENTTIME = " + StringTool.SqlDate(mod.ENDSEGMENTTIME) + "," +
                            " ENDDAYTIME = " + StringTool.SqlDate(mod.ENDDAYTIME) + "," +
                            " DELETED = " + mod.DELETED.ToString() + "," +
                            " CREBY = " + StringTool.SqlQ(mod.CREBY) + "," +
                            " CRETIME = " + StringTool.SqlDate(mod.CRETIME) + "," +
                            " LATMDYBY = " + StringTool.SqlQ(mod.LATMDYBY) + "," +
                            " LSTMDYTIME = " + StringTool.SqlDate(mod.LSTMDYTIME) +
                                " WHERE ESID = " + mod.ESID.ToString();
                CommonDP.ExcuteSql(strSQL);
            }
            catch
            {
                throw;
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
            try
            {
                string strSQL = "Update GS_ENGINEER_SCHEDULES Set Deleted=1  WHERE ESID =" + lngID.ToString();
                CommonDP.ExcuteSql(strSQL);
            }
            catch
            {
                throw;
            }
        }
        #endregion


        public bool IsHavedWorkIssues(long AreaId)
        {
            string strSql = string.Format(@"SELECT * FROM GS_ENGINEER_SCHEDULES 
                                                            WHERE 
                                                        WORKDATE>=(SELECT STARTDATE FROM GS_SCHEDULESAREA WHERE AREAID={0} AND DELETED =0) 
                                                            AND CRESTATUS =0 AND DELETED =0 ", AreaId);

            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            try
            {
                object obj = OracleDbHelper.ExecuteScalar(cn, CommandType.Text, strSql);
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
        /// 得到区间的排班记录
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<GS_Engineer_SchedulesDP> GetWorkIssues(DateTime startDate, DateTime endDate)
        {
            List<GS_Engineer_SchedulesDP> list = new List<GS_Engineer_SchedulesDP>();
            
            string strSQL = string.Empty;
            strSQL = string.Format(@"SELECT e.*,s.name,b.fullname as ScheduleName,s.RegDeptName FROM GS_ENGINEER_SCHEDULES e,cst_servicestaff s ,gs_schedules_base b 
                                where e.engineerid =s.id and e.schedulesid=b.schedulesid 
                                        and s.deleted=0 
                                        and e.deleted=0 
                                        and e.workDate >= to_date('{0}','yyyy-mm-dd')
                                        and e.workDate <= to_date('{1}','yyyy-mm-dd') order by e.schedulesid desc ",
                                                  startDate.ToShortDateString(),
                                                  endDate.ToShortDateString());
                            
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            foreach (DataRow dr in dt.Rows)
            {
                GS_Engineer_SchedulesDP ee = new GS_Engineer_SchedulesDP();

                ee.ESID = Int64.Parse(dr["ESID"].ToString());
                ee.ENGINEERID = Int64.Parse(dr["ENGINEERID"].ToString());
                ee.SCHEDULESID = Int64.Parse(dr["SCHEDULESID"].ToString());
                ee.WORKDATE = dr["WORKDATE"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["WORKDATE"].ToString());
                ee.CRESTATUS = Int64.Parse(dr["CRESTATUS"].ToString());
                ee.RESTFLAG = Int64.Parse(dr["RESTFLAG"].ToString());
                ee.STARTDAYTIME = dr["STARTDAYTIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["STARTDAYTIME"].ToString());
                ee.STARTSEGMENTTIME = dr["STARTSEGMENTTIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["STARTSEGMENTTIME"].ToString());
                ee.ENDSEGMENTTIME = dr["ENDSEGMENTTIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["ENDSEGMENTTIME"].ToString());
                ee.ENDDAYTIME = dr["ENDDAYTIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["ENDDAYTIME"].ToString());
                ee.DELETED = Int64.Parse(dr["DELETED"].ToString());
                ee.CREBY = dr["CREBY"].ToString();
                ee.CRETIME = dr["CRETIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["CRETIME"].ToString());
                ee.LATMDYBY = dr["LATMDYBY"].ToString();
                ee.LSTMDYTIME = dr["LSTMDYTIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["LSTMDYTIME"].ToString());

                ee.ScheduleName = dr["ScheduleName"].ToString();
                ee.EngineerName = dr["name"].ToString();
                ee.DeptName = dr["RegDeptName"].ToString();
                list.Add(ee);
            }
            return list;
        }



        /// <summary>
        /// 取得当天这个排班的所有人员，当IssuesId=-1即为当天所有的排班
        /// </summary>
        /// <param name="queryDate"></param>
        /// <param name="IssuesId"></param>
        /// <returns></returns>
        public List<GS_Engineer_SchedulesDP> GetIssuesOfDay(DateTime queryDate, long IssuesId)
        {
            List<GS_Engineer_SchedulesDP> list = new List<GS_Engineer_SchedulesDP>();

            string strSQL = string.Empty;
            strSQL = string.Format(@"SELECT e.*,s.name,b.fullname as ScheduleName ,s.RegDeptName FROM GS_ENGINEER_SCHEDULES e,cst_servicestaff s ,gs_schedules_base b 
                                where e.engineerid =s.id and e.schedulesid=b.schedulesid 
                                        and s.deleted=0 
                                        and e.deleted=0 
                                        and e.workDate = to_date('{0}','yyyy-mm-dd')  ",
                                                  queryDate.ToShortDateString());
            if (IssuesId != -1)
            {
                strSQL += " AND e.schedulesid=" + IssuesId;
            }
            strSQL += " order by e.schedulesid desc ";
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            foreach (DataRow dr in dt.Rows)
            {
                GS_Engineer_SchedulesDP ee = new GS_Engineer_SchedulesDP();

                ee.ESID = Int64.Parse(dr["ESID"].ToString());
                ee.ENGINEERID = Int64.Parse(dr["ENGINEERID"].ToString());
                ee.SCHEDULESID = Int64.Parse(dr["SCHEDULESID"].ToString());
                ee.WORKDATE = dr["WORKDATE"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["WORKDATE"].ToString());
                ee.CRESTATUS = Int64.Parse(dr["CRESTATUS"].ToString());
                ee.RESTFLAG = Int64.Parse(dr["RESTFLAG"].ToString());
                ee.STARTDAYTIME = dr["STARTDAYTIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["STARTDAYTIME"].ToString());
                ee.STARTSEGMENTTIME = dr["STARTSEGMENTTIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["STARTSEGMENTTIME"].ToString());
                ee.ENDSEGMENTTIME = dr["ENDSEGMENTTIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["ENDSEGMENTTIME"].ToString());
                ee.ENDDAYTIME = dr["ENDDAYTIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["ENDDAYTIME"].ToString());
                ee.DELETED = Int64.Parse(dr["DELETED"].ToString());
                ee.CREBY = dr["CREBY"].ToString();
                ee.CRETIME = dr["CRETIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["CRETIME"].ToString());
                ee.LATMDYBY = dr["LATMDYBY"].ToString();
                ee.LSTMDYTIME = dr["LSTMDYTIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["LSTMDYTIME"].ToString());

                ee.ScheduleName = dr["ScheduleName"].ToString();
                ee.EngineerName = dr["name"].ToString();
                ee.DeptName = dr["RegDeptName"].ToString();
                list.Add(ee);
            }
            return list;
        }

        public List<GS_Engineer_SchedulesDP> GetCurrentIssues()
        {
            DataTable dt = null;
            OracleParameter[] parms = {
                     
                      new OracleParameter("P_OUTTABLE",OracleType.Cursor)
                };
            parms[0].Direction = ParameterDirection.Output;

            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            try
            {
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_Schedule_GET_ISSUES", parms);
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
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            List<GS_Engineer_SchedulesDP> result = new List<GS_Engineer_SchedulesDP>();
            foreach (DataRow dr in dt.Rows)
            {
                GS_Engineer_SchedulesDP ee = new GS_Engineer_SchedulesDP();

                //ee.ESID = Int64.Parse(dr["ESID"].ToString());
                ee.ENGINEERID = Int64.Parse(dr["ENGINEERID"].ToString());
                //ee.SCHEDULESID = Int64.Parse(dr["SCHEDULESID"].ToString());
                //ee.WORKDATE = dr["WORKDATE"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["WORKDATE"].ToString());
                //ee.CRESTATUS = Int64.Parse(dr["CRESTATUS"].ToString());
                //ee.RESTFLAG = Int64.Parse(dr["RESTFLAG"].ToString());
                //ee.STARTDAYTIME = dr["STARTDAYTIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["STARTDAYTIME"].ToString());
                //ee.STARTSEGMENTTIME = dr["STARTSEGMENTTIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["STARTSEGMENTTIME"].ToString());
                //ee.ENDSEGMENTTIME = dr["ENDSEGMENTTIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["ENDSEGMENTTIME"].ToString());
                //ee.ENDDAYTIME = dr["ENDDAYTIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["ENDDAYTIME"].ToString());
                //ee.DELETED = Int64.Parse(dr["DELETED"].ToString());
                //ee.CREBY = dr["CREBY"].ToString();
                //ee.CRETIME = dr["CRETIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["CRETIME"].ToString());
                //ee.LATMDYBY = dr["LATMDYBY"].ToString();
                //ee.LSTMDYTIME = dr["LSTMDYTIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["LSTMDYTIME"].ToString());

                //ee.ScheduleName = dr["ScheduleName"].ToString();
                //ee.EngineerName = dr["name"].ToString();
                //ee.DeptName = dr["RegDeptName"].ToString();

   
                result.Add(ee);
            }
            return result;


        }


 
    }
}

