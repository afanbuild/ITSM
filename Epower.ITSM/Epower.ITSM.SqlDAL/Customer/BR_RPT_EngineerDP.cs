using System;
using System.Data;
using System.Data.SqlClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;
using Epower.ITSM.SqlDAL.CommonUtil;

namespace Epower.ITSM.SqlDAL
{
    public class BR_RPT_EngineerDP
    {
        #region Property

        #region ID
        private Decimal mID;
        /// <summary>
        ///
        /// </summary>
        public Decimal ID
        {
            get { return mID; }
            set { mID = value; }
        }
         #endregion        

        #region userID
        private Decimal userid;
        /// <summary>
        ///
        /// </summary>
        public Decimal UserID
        {
            get { return userid; }
            set { userid = value; }
        }
         #endregion

        #region systype
        private string systype;
        /// <summary>
        ///
        /// </summary>
        public string SysType
        {
            get { return systype; }
            set { systype = value; }
        }
         #endregion

        #region engineerids
        private string engineerids;
        /// <summary>
        ///
        /// </summary>
        public string EngineerIds
        {
            get { return engineerids; }
            set { engineerids = value; }
        }
         #endregion

        #region engineerNames
        private string engineerNames;
        /// <summary>
        ///
        /// </summary>
        public string EngineerNames
        {
            get { return engineerNames; }
            set { engineerNames = value; }
        }
         #endregion

        #region Deleted
        private Int32 mDeleted;
        /// <summary>
        ///
        /// </summary>
        public Int32 Deleted
        {
            get { return mDeleted; }
            set { mDeleted = value; }
        }
        #endregion

        #region CreBy
        private String mCreBy;
        /// <summary>
        ///
        /// </summary>
        public String CreBy
        {
            get { return mCreBy; }
            set { mCreBy = value; }
        }
        #endregion

        #region CreTime
        /// <summary>
        ///
        /// </summary>
        private DateTime mCreTime;
        public DateTime CreTime
        {
            get { return mCreTime; }
            set { mCreTime = value; }
        }
        #endregion

        #region LastMdyBy
        private String mLastMdfBy;
        /// <summary>
        ///
        /// </summary>
        public String LastMdfBy
        {
            get { return mLastMdfBy; }
            set { mLastMdfBy = value; }
        }
        #endregion

        #region LastMdyTime
        /// <summary>
        ///
        /// </summary>
        private DateTime mLastMdfTime;
        public DateTime LastMdfTime
        {
            get { return mLastMdfTime; }
            set { mLastMdfTime = value; }
        }
        #endregion

        #endregion

        public BR_RPT_EngineerDP GetByType(decimal userId, string sysType)
        {
            string strSQL = string.Format("SELECT * FROM (SELECT * FROM BR_RPT_ENGINEER WHERE USERID='{0}' AND SYSTYPE='{1}' ORDER BY LASTMDFTIME DESC ) WHERE ROWNUM=1  ", userId, sysType);
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            BR_RPT_EngineerDP engineer = new BR_RPT_EngineerDP();
            engineer.ID = decimal.Parse(dt.Rows[0]["ID"].ToString());
            engineer.UserID = decimal.Parse(dt.Rows[0]["UserID"].ToString());

            engineer.SysType = dt.Rows[0]["SysType"].ToString();
            engineer.EngineerIds = dt.Rows[0]["EngineerIds"].ToString();
            engineer.EngineerNames = dt.Rows[0]["EngineerNames"].ToString();
            engineer.CreBy = dt.Rows[0]["CreBy"].ToString();
            engineer.LastMdfBy = dt.Rows[0]["LASTMDFBY"].ToString();


            engineer.Deleted = int.Parse(dt.Rows[0]["Deleted"].ToString());

            engineer.CreTime = DateTime.Parse(dt.Rows[0]["CreTime"].ToString());
            engineer.LastMdfTime = DateTime.Parse(dt.Rows[0]["LASTMDFTIME"].ToString());

            return engineer;
        }

        public void Insert(BR_RPT_EngineerDP engineer)
        {
            string strSQL = string.Empty;

            try
            {
                //int id = SerialNumber.GetNextval("BR_RPT_ENGINEER_SEQ");
                string strID = EpowerGlobal.EPGlobal.GetNextID("BR_RPT_ENGINEER_SEQ").ToString();
                engineer.ID = decimal.Parse(strID.ToString());
                strSQL = @"INSERT INTO BR_RPT_ENGINEER(
									ID,
									userid,
									systype,
									engineerids,
									engineernames,
									Deleted,
                                    CreBy,
                                    CreTime,
                                    LASTMDFBY,
                                    LASTMDFTIME
					)
					VALUES(" +
                            strID.ToString() + "," +
                            engineer.UserID.ToString() + "," +
                            StringTool.SqlQ(engineer.SysType) + "," +                      
                            StringTool.SqlQ(engineer.EngineerIds) + "," +
                            StringTool.SqlQ(engineer.EngineerNames) + "," +                           
                            engineer.Deleted.ToString() + "," +
                            StringTool.SqlQ(engineer.CreBy) + "," +
                            StringTool.SqlDate(engineer.CreTime) + "," +
                            StringTool.SqlQ(engineer.LastMdfBy) + "," +
                            StringTool.SqlDate(engineer.LastMdfTime) +
                    ")";

                CommonDP.ExcuteSql(strSQL);
            }
            catch
            {
                throw;
            }
            finally
            { 
            
            }
        }
    }
}
