


/*******************************************************************
 *
 * Description
 * 
 * 
 * Create By  :yxq
 * Create Date:2012年8月22日
 * *****************************************************************/
using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// 
    /// </summary>
    public class GS_Schedules_BaseDP
    {
        /// <summary>
        /// 
        /// </summary>
        public GS_Schedules_BaseDP()
        { }

        #region Property
        #region
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

        #region 班次ID
        private String mFULLNAME = string.Empty;
        /// <summary>
        ///
        /// </summary>
        public String FULLNAME
        {
            get { return mFULLNAME; }
            set { mFULLNAME = value; }
        }
        #endregion

        #region 班次ID
        private String mSIMPLENAME = string.Empty;
        /// <summary>
        ///
        /// </summary>
        public String SIMPLENAME
        {
            get { return mSIMPLENAME; }
            set { mSIMPLENAME = value; }
        }
        #endregion

        #region 上班时间
        private String mSTARTDAYTIME = string.Empty;
        /// <summary>
        ///
        /// </summary>
        public String STARTDAYTIME
        {
            get { return mSTARTDAYTIME; }
            set { mSTARTDAYTIME = value; }
        }
        #endregion

        #region 中午下班时间
        private String mSTARTSEGMENTTIME = string.Empty;
        /// <summary>
        ///
        /// </summary>
        public String STARTSEGMENTTIME
        {
            get { return mSTARTSEGMENTTIME; }
            set { mSTARTSEGMENTTIME = value; }
        }
        #endregion

        #region 中午上班时间
        private String mENDSEGMENTTIME = string.Empty;
        /// <summary>
        ///
        /// </summary>
        public String ENDSEGMENTTIME
        {
            get { return mENDSEGMENTTIME; }
            set { mENDSEGMENTTIME = value; }
        }
        #endregion

        #region 下班时间
        private String mENDDAYTIME = string.Empty;
        /// <summary>
        ///
        /// </summary>
        public String ENDDAYTIME
        {
            get { return mENDDAYTIME; }
            set { mENDDAYTIME = value; }
        }
        #endregion

        #region
        private Decimal mABS_LENGTH1;
        /// <summary>
        ///
        /// </summary>
        public Decimal ABS_LENGTH1
        {
            get { return mABS_LENGTH1; }
            set { mABS_LENGTH1 = value; }
        }
        #endregion

        #region
        private Decimal mABS_LENGTH2;
        /// <summary>
        ///
        /// </summary>
        public Decimal ABS_LENGTH2
        {
            get { return mABS_LENGTH2; }
            set { mABS_LENGTH2 = value; }
        }
        #endregion

        #region
        private Decimal mABS_LENGTH3;
        /// <summary>
        ///
        /// </summary>
        public Decimal ABS_LENGTH3
        {
            get { return mABS_LENGTH3; }
            set { mABS_LENGTH3 = value; }
        }
        #endregion

        #region 是否跨天
        private Int64 mOVERDAYFLAG;
        /// <summary>
        ///
        /// </summary>
        public Int64 OVERDAYFLAG
        {
            get { return mOVERDAYFLAG; }
            set { mOVERDAYFLAG = value; }
        }
        #endregion

        #region 是否跨天1
        private Int64 mOVERDAYFLAG1 = 0;
        /// <summary>
        ///
        /// </summary>
        public Int64 OVERDAYFLAG1
        {
            get { return mOVERDAYFLAG1; }
            set { mOVERDAYFLAG1 = value; }
        }
        #endregion

        #region 是否跨天2
        private Int64 mOVERDAYFLAG2 = 0;
        /// <summary>
        ///
        /// </summary>
        public Int64 OVERDAYFLAG2
        {
            get { return mOVERDAYFLAG2; }
            set { mOVERDAYFLAG2 = value; }
        }
        #endregion

        #region 是否跨天3
        private Int64 mOVERDAYFLAG3 = 0;
        /// <summary>
        ///
        /// </summary>
        public Int64 OVERDAYFLAG3
        {
            get { return mOVERDAYFLAG3; }
            set { mOVERDAYFLAG3 = value; }
        }
        #endregion

        #region
        private Decimal mWORKHOUR;
        /// <summary>
        ///
        /// </summary>
        public Decimal WORKHOUR
        {
            get { return mWORKHOUR; }
            set { mWORKHOUR = value; }
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

        #region GetReCorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>GS_Schedules_BaseDP</returns>
        public GS_Schedules_BaseDP GetReCorded(long lngID)
        {
            GS_Schedules_BaseDP ee = new GS_Schedules_BaseDP();
            string strSQL = string.Empty;
            strSQL = "SELECT * FROM GS_SCHEDULES_BASE WHERE   SCHEDULESID = " + lngID.ToString();
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            foreach (DataRow dr in dt.Rows)
            {
                ee.SCHEDULESID = Int64.Parse(dr["SCHEDULESID"].ToString());
                ee.FULLNAME = dr["FULLNAME"].ToString();
                ee.SIMPLENAME = dr["SIMPLENAME"].ToString();
                ee.STARTDAYTIME = dr["STARTDAYTIME"].ToString();
                ee.STARTSEGMENTTIME = dr["STARTSEGMENTTIME"].ToString();
                ee.ENDSEGMENTTIME = dr["ENDSEGMENTTIME"].ToString();
                ee.ENDDAYTIME = dr["ENDDAYTIME"].ToString();
                ee.ABS_LENGTH1 = Decimal.Parse(dr["ABS_LENGTH1"].ToString());
                ee.ABS_LENGTH2 = Decimal.Parse(dr["ABS_LENGTH2"].ToString());
                ee.ABS_LENGTH3 = Decimal.Parse(dr["ABS_LENGTH3"].ToString());
                ee.OVERDAYFLAG = Int64.Parse(dr["OVERDAYFLAG"].ToString());
                ee.WORKHOUR = Int64.Parse(dr["WORKHOUR"].ToString());
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
            DataTable dt = CommonDP.ExcuteSqlTablePage("GS_SCHEDULES_BASE", "*", sWhere, sOrder, pagesize, pageindex, ref rowcount);
            return dt;
        }
        #endregion

        #region InsertRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pGS_Schedules_BaseDP></param>
        public void InsertRecorded(GS_Schedules_BaseDP mod)
        {
            string strSQL = string.Empty;
            try
            {
                string strID = EpowerGlobal.EPGlobal.GetNextID("GS_SCHEDULES_BASE_SEQ").ToString();

                strSQL = @"INSERT INTO GS_SCHEDULES_BASE(
									SCHEDULESID,
									FULLNAME,
									SIMPLENAME,
									STARTDAYTIME,
									STARTSEGMENTTIME,
									ENDSEGMENTTIME,
									ENDDAYTIME,
									ABS_LENGTH1,
									ABS_LENGTH2,
									ABS_LENGTH3,
									OVERDAYFLAG,
		                            OVERDAYFLAG1,		
                                    OVERDAYFLAG2,
		                            OVERDAYFLAG3,
									WORKHOUR,
									DELETED,
									CREBY,
									CRETIME,
									LATMDYBY,
									LSTMDYTIME
					)
					VALUES( " +
                            strID + "," +
                            StringTool.SqlQ(mod.FULLNAME.ToString()) + "," +
                            StringTool.SqlQ(mod.SIMPLENAME.ToString()) + "," +
                            StringTool.SqlQ(mod.STARTDAYTIME.ToString()) + "," +
                            StringTool.SqlQ(mod.STARTSEGMENTTIME.ToString()) + "," +
                            StringTool.SqlQ(mod.ENDSEGMENTTIME.ToString()) + "," +
                            StringTool.SqlQ(mod.ENDDAYTIME.ToString()) + "," +
                            mod.ABS_LENGTH1.ToString().ToString() + "," +
                            mod.ABS_LENGTH2.ToString().ToString() + "," +
                            mod.ABS_LENGTH3.ToString().ToString() + "," +
                            mod.OVERDAYFLAG.ToString().ToString() + "," +
                            mod.OVERDAYFLAG1.ToString().ToString() + "," +
                            mod.OVERDAYFLAG2.ToString().ToString() + "," +
                            mod.OVERDAYFLAG3.ToString().ToString() + "," +
                            mod.WORKHOUR.ToString().ToString() + "," +
                            mod.DELETED.ToString().ToString() + "," +
                            StringTool.SqlQ(mod.CREBY.ToString()) + "," +
                            (mod.CRETIME.ToString() == DateTime.MinValue.ToString() ? " null " : "to_date(" + StringTool.SqlQ(mod.CRETIME.ToString().ToString()) + ",'yyyy-MM-dd HH24:mi:ss')") + "," +
                            StringTool.SqlQ(mod.LATMDYBY.ToString()) + "," +
                            (mod.LSTMDYTIME.ToString() == DateTime.MinValue.ToString() ? " null " : "to_date(" + StringTool.SqlQ(mod.LSTMDYTIME.ToString().ToString()) + ",'yyyy-MM-dd HH24:mi:ss')") +
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
        /// <param name=pGS_Schedules_BaseDP></param>
        public void UpdateRecorded(GS_Schedules_BaseDP mod)
        {
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE GS_SCHEDULES_BASE Set " +
                                                        " FULLNAME = " + StringTool.SqlQ(mod.FULLNAME) + "," +
                            " SIMPLENAME = " + StringTool.SqlQ(mod.SIMPLENAME) + "," +
                            " STARTDAYTIME = " + StringTool.SqlQ(mod.STARTDAYTIME) + "," +
                            " STARTSEGMENTTIME = " + StringTool.SqlQ(mod.STARTSEGMENTTIME) + "," +
                            " ENDSEGMENTTIME = " + StringTool.SqlQ(mod.ENDSEGMENTTIME) + "," +
                            " ENDDAYTIME = " + StringTool.SqlQ(mod.ENDDAYTIME) + "," +
                            " ABS_LENGTH1 = " + mod.ABS_LENGTH1.ToString() + "," +
                            " ABS_LENGTH2 = " + mod.ABS_LENGTH2.ToString() + "," +
                            " ABS_LENGTH3 = " + mod.ABS_LENGTH3.ToString() + "," +
                            " OVERDAYFLAG = " + mod.OVERDAYFLAG.ToString() + "," +
                            " OVERDAYFLAG1 = " + mod.OVERDAYFLAG1.ToString() + "," +
                            " OVERDAYFLAG2 = " + mod.OVERDAYFLAG2.ToString() + "," +
                            " OVERDAYFLAG3 = " + mod.OVERDAYFLAG3.ToString() + "," +
                            " WORKHOUR = " + mod.WORKHOUR.ToString() + "," +
                            " CREBY = " + StringTool.SqlQ(mod.CREBY) +
                                " WHERE SCHEDULESID = " + mod.SCHEDULESID.ToString();
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
                string strSQL = "Update GS_SCHEDULES_BASE Set Deleted=1  WHERE SCHEDULESID =" + lngID.ToString();
                CommonDP.ExcuteSql(strSQL);
            }
            catch
            {
                throw;
            }
        }
        #endregion


        /// <summary>
        /// 得到所有排班信息，（休息为特殊类型，未带出）
        /// </summary>
        /// <returns></returns>
        public List<GS_Schedules_BaseDP> GetAllList()
        {
            List<GS_Schedules_BaseDP> list = new List<GS_Schedules_BaseDP>();
            string strSQL = string.Empty;
            strSQL = "SELECT * FROM GS_SCHEDULES_BASE WHERE deleted=0 AND SCHEDULESID>0  ORDER BY STARTDAYTIME ";
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            foreach (DataRow dr in dt.Rows)
            {
                GS_Schedules_BaseDP ee = new GS_Schedules_BaseDP();
                ee.SCHEDULESID = Int64.Parse(dr["SCHEDULESID"].ToString());
                ee.FULLNAME = dr["FULLNAME"].ToString();
                ee.SIMPLENAME = dr["SIMPLENAME"].ToString();
                ee.STARTDAYTIME = dr["STARTDAYTIME"].ToString();
                ee.STARTSEGMENTTIME = dr["STARTSEGMENTTIME"].ToString();
                ee.ENDSEGMENTTIME = dr["ENDSEGMENTTIME"].ToString();
                ee.ENDDAYTIME = dr["ENDDAYTIME"].ToString();
                ee.ABS_LENGTH1 = Decimal.Parse(dr["ABS_LENGTH1"].ToString());
                ee.ABS_LENGTH2 = Decimal.Parse(dr["ABS_LENGTH2"].ToString());
                ee.ABS_LENGTH3 = Decimal.Parse(dr["ABS_LENGTH3"].ToString());
                ee.OVERDAYFLAG = Int64.Parse(dr["OVERDAYFLAG"].ToString());
                ee.WORKHOUR = Int64.Parse(dr["WORKHOUR"].ToString());
                ee.DELETED = Int64.Parse(dr["DELETED"].ToString());
                ee.CREBY = dr["CREBY"].ToString();
                ee.CRETIME = dr["CRETIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["CRETIME"].ToString());
                ee.LATMDYBY = dr["LATMDYBY"].ToString();
                ee.LSTMDYTIME = dr["LSTMDYTIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["LSTMDYTIME"].ToString());
                list.Add(ee);
            }
            return list;
        }

             /// <summary>
        /// 获取班次表
        /// </summary>
        /// <returns></returns>
        public static DataTable GetFullSchedulesAreaBase()
        {
            StringBuilder sbStrSQL = new StringBuilder();
            sbStrSQL.Append("select schedulesid, fullname, simplename, startdaytime, startsegmenttime, endsegmenttime, enddaytime,");
            sbStrSQL.Append(" abs_length1, abs_length2, abs_length3, overdayflag, workhour, deleted, creby, cretime, latmdyby, lstmdytime ");
            sbStrSQL.Append(" from gs_schedules_base where  deleted=0  AND SCHEDULESID>0 ");
            return CommonDP.ExcuteSqlTable(sbStrSQL.ToString());
        }

        /// <summary>
        /// 获取班次表
        /// </summary>
        /// <returns></returns>
        public static GS_Schedules_BaseDP GetSchedulesAreaBase(long schedulesID)
        {
            DataTable dt = null;
            StringBuilder sbStrSQL = new StringBuilder();
            GS_Schedules_BaseDP gsSchedulesBase = new GS_Schedules_BaseDP();

            sbStrSQL.Append("select * from gs_schedules_base where schedulesid=" + schedulesID);

            dt = CommonDP.ExcuteSqlTable(sbStrSQL.ToString());

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    gsSchedulesBase.SCHEDULESID = Int64.Parse(dr["SCHEDULESID"].ToString());
                    gsSchedulesBase.FULLNAME = dr["FULLNAME"].ToString();
                    gsSchedulesBase.SIMPLENAME = dr["SIMPLENAME"].ToString();
                    gsSchedulesBase.STARTDAYTIME = dr["STARTDAYTIME"].ToString();
                    gsSchedulesBase.STARTSEGMENTTIME = dr["STARTSEGMENTTIME"].ToString();
                    gsSchedulesBase.ENDSEGMENTTIME = dr["ENDSEGMENTTIME"].ToString();
                    gsSchedulesBase.ENDDAYTIME = dr["ENDDAYTIME"].ToString();
                    gsSchedulesBase.ABS_LENGTH1 = Decimal.Parse(dr["ABS_LENGTH1"].ToString());
                    gsSchedulesBase.ABS_LENGTH2 = Decimal.Parse(dr["ABS_LENGTH2"].ToString());
                    gsSchedulesBase.ABS_LENGTH3 = Decimal.Parse(dr["ABS_LENGTH3"].ToString());
                    gsSchedulesBase.OVERDAYFLAG = Int64.Parse(dr["OVERDAYFLAG"].ToString());
                    gsSchedulesBase.OVERDAYFLAG1 = Int64.Parse(dr["OVERDAYFLAG1"].ToString());
                    gsSchedulesBase.OVERDAYFLAG2 = Int64.Parse(dr["OVERDAYFLAG2"].ToString());
                    gsSchedulesBase.OVERDAYFLAG3 = Int64.Parse(dr["OVERDAYFLAG3"].ToString());
                    gsSchedulesBase.WORKHOUR = Int64.Parse(dr["WORKHOUR"].ToString());
                    gsSchedulesBase.DELETED = Int64.Parse(dr["DELETED"].ToString());
                    gsSchedulesBase.CREBY = dr["CREBY"].ToString();
                    gsSchedulesBase.CRETIME = dr["CRETIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["CRETIME"].ToString());
                    gsSchedulesBase.LATMDYBY = dr["LATMDYBY"].ToString();
                    gsSchedulesBase.LSTMDYTIME = dr["LSTMDYTIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["LSTMDYTIME"].ToString());
                }
            }

            return gsSchedulesBase;
        }
	}
}

