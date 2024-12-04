



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
using System.Collections.Generic;
using System.Collections;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// 
    /// </summary>
    public class GS_PreSchedulesDP
    {
        /// <summary>
        /// 
        /// </summary>
        public GS_PreSchedulesDP()
        { }

        #region Property
        #region
        private Int64 mPREID;
        /// <summary>
        ///
        /// </summary>
        public Int64 PREID
        {
            get { return mPREID; }
            set { mPREID = value; }
        }
        #endregion

        #region 排班区间Id
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

        #region 工程师ID
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

        #region 排班Id
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

        #region 剩余次数
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

        #region 工程师ID
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

        #region 上次标记
        private Int64 mPREFLAG;
        /// <summary>
        ///
        /// </summary>
        public Int64 PREFLAG
        {
            get { return mPREFLAG; }
            set { mPREFLAG = value; }
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
        /// <returns>GS_PreSchedulesDP</returns>
        public GS_PreSchedulesDP GetReCorded(long lngID)
        {
            GS_PreSchedulesDP ee = new GS_PreSchedulesDP();
            string strSQL = string.Empty;
            strSQL = "SELECT * FROM GS_PRESCHEDULES WHERE   PREID = " + lngID.ToString();
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            foreach (DataRow dr in dt.Rows)
            {
                ee.PREID = Int64.Parse(dr["PREID"].ToString());
                ee.AREAID = Int64.Parse(dr["AREAID"].ToString());
                ee.ENGINEERID = Int64.Parse(dr["ENGINEERID"].ToString());
                ee.SCHEDULESID = Int64.Parse(dr["SCHEDULESID"].ToString());
                ee.REMAINDERNUM = Int64.Parse(dr["REMAINDERNUM"].ToString());
                ee.CRESTATUS = Int64.Parse(dr["CRESTATUS"].ToString());
                ee.PREFLAG = Int64.Parse(dr["PREFLAG"].ToString());
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
            DataTable dt = CommonDP.ExcuteSqlTablePage("GS_PRESCHEDULES", "*", sWhere, sOrder, pagesize, pageindex, ref rowcount);
            return dt;
        }
        #endregion

        #region InsertRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pGS_PreSchedulesDP></param>
        public void InsertRecorded(GS_PreSchedulesDP mod)
        {
            string strSQL = string.Empty;
            try
            {
                string strID = EpowerGlobal.EPGlobal.GetNextID("GS_PRESCHEDULES_SEQ").ToString();

                strSQL = @"INSERT INTO GS_PRESCHEDULES(
									PREID,
									AREAID,
									ENGINEERID,
									SCHEDULESID,
									REMAINDERNUM,
									CRESTATUS,
									PREFLAG,
									DELETED,
									CREBY,
									CRETIME,
									LATMDYBY,
									LSTMDYTIME
					)
					VALUES( " +
                            strID + "," +
                            mod.AREAID.ToString().ToString() + "," +
                            mod.ENGINEERID.ToString().ToString() + "," +
                            mod.SCHEDULESID.ToString().ToString() + "," +
                            mod.REMAINDERNUM.ToString().ToString() + "," +
                            mod.CRESTATUS.ToString().ToString() + "," +
                            mod.PREFLAG.ToString().ToString() + "," +
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
        /// <param name=pGS_PreSchedulesDP></param>
        public void UpdateRecorded(GS_PreSchedulesDP mod)
        {
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE GS_PRESCHEDULES Set " +
                                                        " AREAID = " + mod.AREAID.ToString() + "," +
                            " ENGINEERID = " + mod.ENGINEERID.ToString() + "," +
                            " SCHEDULESID = " + mod.SCHEDULESID.ToString() + "," +
                            " REMAINDERNUM = " + mod.REMAINDERNUM.ToString() + "," +
                            " CRESTATUS = " + mod.CRESTATUS.ToString() + "," +
                            " PREFLAG = " + mod.PREFLAG.ToString() + "," +
                            " DELETED = " + mod.DELETED.ToString() + "," +
                            " CREBY = " + StringTool.SqlQ(mod.CREBY) + "," +
                            " CRETIME = " + StringTool.SqlDate(mod.CRETIME) + "," +
                            " LATMDYBY = " + StringTool.SqlQ(mod.LATMDYBY) + "," +
                            " LSTMDYTIME = " + StringTool.SqlDate(mod.LSTMDYTIME) +
                                " WHERE PREID = " + mod.PREID.ToString();
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
                string strSQL = "Update GS_PRESCHEDULES Set Deleted=1  WHERE PREID =" + lngID.ToString();
                CommonDP.ExcuteSql(strSQL);
            }
            catch
            {
                throw;
            }
        }
        #endregion

        public GS_PreSchedulesDP GetReCorded(long AreaId,long EngineerId)
        {
            GS_PreSchedulesDP ee = new GS_PreSchedulesDP();
            string strSQL = string.Empty;
            strSQL = "SELECT * FROM GS_PRESCHEDULES WHERE  deleted =0 and  AreaId = " + AreaId.ToString() + " And EngineerId=" + EngineerId.ToString();
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            foreach (DataRow dr in dt.Rows)
            {
                ee.PREID = Int64.Parse(dr["PREID"].ToString());
                ee.AREAID = Int64.Parse(dr["AREAID"].ToString());
                ee.ENGINEERID = Int64.Parse(dr["ENGINEERID"].ToString());
                ee.SCHEDULESID = Int64.Parse(dr["SCHEDULESID"].ToString());
                ee.REMAINDERNUM = Int64.Parse(dr["REMAINDERNUM"].ToString());
                ee.CRESTATUS = Int64.Parse(dr["CRESTATUS"].ToString());
                ee.PREFLAG = Int64.Parse(dr["PREFLAG"].ToString());
                ee.DELETED = Int64.Parse(dr["DELETED"].ToString());
                ee.CREBY = dr["CREBY"].ToString();
                ee.CRETIME = dr["CRETIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["CRETIME"].ToString());
                ee.LATMDYBY = dr["LATMDYBY"].ToString();
                ee.LSTMDYTIME = dr["LSTMDYTIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["LSTMDYTIME"].ToString());
            }
            return ee;
        }

        public List<GS_PreSchedulesDP> GetAllByAreaId(long AreaId)
        {
            List<GS_PreSchedulesDP> list = new List<GS_PreSchedulesDP>();
            string strSQL = string.Empty;
            strSQL = "SELECT * FROM GS_PRESCHEDULES WHERE  deleted =0 and  AreaId = " + AreaId.ToString() ;
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            foreach (DataRow dr in dt.Rows)
            {
                GS_PreSchedulesDP ee = new GS_PreSchedulesDP();
               

                ee.PREID = Int64.Parse(dr["PREID"].ToString());
                ee.AREAID = Int64.Parse(dr["AREAID"].ToString());
                ee.ENGINEERID = Int64.Parse(dr["ENGINEERID"].ToString());
                ee.SCHEDULESID = Int64.Parse(dr["SCHEDULESID"].ToString());
                ee.REMAINDERNUM = Int64.Parse(dr["REMAINDERNUM"].ToString());
                ee.CRESTATUS = Int64.Parse(dr["CRESTATUS"].ToString());
                ee.PREFLAG = Int64.Parse(dr["PREFLAG"].ToString());
                ee.DELETED = Int64.Parse(dr["DELETED"].ToString());
                ee.CREBY = dr["CREBY"].ToString();
                ee.CRETIME = dr["CRETIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["CRETIME"].ToString());
                ee.LATMDYBY = dr["LATMDYBY"].ToString();
                ee.LSTMDYTIME = dr["LSTMDYTIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["LSTMDYTIME"].ToString());
                list.Add(ee);
            }
            return list;
        }

       
    }


    
}

