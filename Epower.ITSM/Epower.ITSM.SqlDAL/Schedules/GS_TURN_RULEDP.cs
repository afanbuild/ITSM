



/*******************************************************************
 *
 * Description
 * 
 * 
 * Create By  :yxq
 * Create Date:2012年9月5日
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

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// 
    /// </summary>
    public class GS_TURN_RULEDP
    {
        /// <summary>
        /// 
        /// </summary>
        public GS_TURN_RULEDP()
        { }

        #region Property
        #region 轮班ID
        private Int64 mTRID;
        /// <summary>
        ///
        /// </summary>
        public Int64 TRID
        {
            get { return mTRID; }
            set { mTRID = value; }
        }
        #endregion

        #region 名称
        private String mTURNNAME = string.Empty;
        /// <summary>
        ///
        /// </summary>
        public String TURNNAME
        {
            get { return mTURNNAME; }
            set { mTURNNAME = value; }
        }
        #endregion

        #region 频率
        private Int64 mTURNRATE;
        /// <summary>
        ///
        /// </summary>
        public Int64 TURNRATE
        {
            get { return mTURNRATE; }
            set { mTURNRATE = value; }
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

        #region 状态
        private Int64 mSTATUS;
        /// <summary>
        ///
        /// </summary>
        public Int64 STATUS
        {
            get { return mSTATUS; }
            set { mSTATUS = value; }
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
        /// <returns>GS_TURN_RULEDP</returns>
        public GS_TURN_RULEDP GetReCorded(long lngID)
        {
            GS_TURN_RULEDP ee = new GS_TURN_RULEDP();
            string strSQL = string.Empty;
            strSQL = "SELECT * FROM GS_TURN_RULE WHERE   TRID = " + lngID.ToString();
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            foreach (DataRow dr in dt.Rows)
            {
                ee.TRID = Int64.Parse(dr["TRID"].ToString());
                ee.TURNNAME = dr["TURNNAME"].ToString();
                ee.TURNRATE = Int64.Parse(dr["TURNRATE"].ToString());
                ee.REMARK = dr["REMARK"].ToString();
                ee.STATUS = Int64.Parse(dr["STATUS"].ToString());
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
            DataTable dt = CommonDP.ExcuteSqlTablePage("GS_TURN_RULE", "*", sWhere, sOrder, pagesize, pageindex, ref rowcount);
            return dt;
        }
        #endregion

        #region InsertRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pGS_TURN_RULEDP></param>
        public void InsertRecorded(GS_TURN_RULEDP mod)
        {
            string strSQL = string.Empty;
            try
            {
                strSQL = @"INSERT INTO GS_TURN_RULE(
									TRID,
									TURNNAME,
									TURNRATE,
									REMARK,
									STATUS,
									DELETED,
									CREBY,
									CRETIME,
									LATMDYBY,
									LSTMDYTIME
					)
					VALUES( " +
 " GS_TURN_RULE_SEQ.nextval  ," +
                            StringTool.SqlQ(mod.TURNNAME.ToString()) + "," +
                            mod.TURNRATE.ToString().ToString() + "," +
                            StringTool.SqlQ(mod.REMARK.ToString()) + "," +
                            mod.STATUS.ToString().ToString() + "," +
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
        /// <param name=pGS_TURN_RULEDP></param>
        public void UpdateRecorded(GS_TURN_RULEDP mod)
        {
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE GS_TURN_RULE Set " +
                                                        " TURNNAME = " + StringTool.SqlQ(mod.TURNNAME) + "," +
                            " TURNRATE = " + mod.TURNRATE.ToString() + "," +
                            " REMARK = " + StringTool.SqlQ(mod.REMARK) + "," +
                            " STATUS = " + mod.STATUS.ToString() + "," +
                            " DELETED = " + mod.DELETED.ToString() + "," +
                            " CREBY = " + StringTool.SqlQ(mod.CREBY) + "," +
                            " CRETIME = " + StringTool.SqlDate(mod.CRETIME) + "," +
                            " LATMDYBY = " + StringTool.SqlQ(mod.LATMDYBY) + "," +
                            " LSTMDYTIME = " + StringTool.SqlDate(mod.LSTMDYTIME) +
                                " WHERE TRID = " + mod.TRID.ToString();
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
                string strSQL = "Update GS_TURN_RULE Set Deleted=1  WHERE TRID =" + lngID.ToString();
                CommonDP.ExcuteSql(strSQL);
            }
            catch
            {
                throw;
            }
        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<GS_TURN_RULEDP> GetAll( )
        {
            List<GS_TURN_RULEDP> list = new List<GS_TURN_RULEDP>();
            
            string strSQL = string.Empty;
            strSQL = "SELECT * FROM GS_TURN_RULE WHERE Deleted =0 " ;
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            foreach (DataRow dr in dt.Rows)
            {
                GS_TURN_RULEDP ee = new GS_TURN_RULEDP();
                ee.TRID = Int64.Parse(dr["TRID"].ToString());
                ee.TURNNAME = dr["TURNNAME"].ToString();
                ee.TURNRATE = Int64.Parse(dr["TURNRATE"].ToString());
                ee.REMARK = dr["REMARK"].ToString();
                ee.STATUS = Int64.Parse(dr["STATUS"].ToString());
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

