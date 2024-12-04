/*******************************************************************
 *
 * Description:用户认证数据处理
 * 
 * 
 * Create By  :
 * Create Date:2010年3月24日
 * *****************************************************************/
using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.Base;
using EpowerGlobal;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// 用户认证
    /// </summary>
    public class Ts_User_FlowDP
    {
        static string CONST_ENCRYKEY = "WangyqLijSuks_GainALotOfMoney_AndBuyCarAndBuildHouse";

        /// <summary>
        /// 
        /// </summary>
        public Ts_User_FlowDP()
        { }

        #region Property
        #region UserID
        /// <summary>
        ///
        /// </summary>
        private Decimal mUserID;
        public Decimal UserID
        {
            get { return mUserID; }
            set { mUserID = value; }
        }
        #endregion

        #region FlowId
        /// <summary>
        ///
        /// </summary>
        private Decimal mFlowId;
        public Decimal FlowId
        {
            get { return mFlowId; }
            set { mFlowId = value; }
        }
        #endregion

        #region NodeModelID
        /// <summary>
        ///
        /// </summary>
        private Decimal mNodeModelID;
        public Decimal NodeModelID
        {
            get { return mNodeModelID; }
            set { mNodeModelID = value; }
        }
        #endregion

        #region FlowModelID
        /// <summary>
        ///
        /// </summary>
        private Decimal mFlowModelID;
        public Decimal FlowModelID
        {
            get { return mFlowModelID; }
            set { mFlowModelID = value; }
        }
        #endregion

        #region LoginName
        /// <summary>
        ///
        /// </summary>
        private string mLoginName;
        public string LoginName
        {
            get { return mLoginName; }
            set { mLoginName = value; }
        }
        #endregion

        #region Password
        /// <summary>
        ///
        /// </summary>
        private string mPassword;
        public string Password
        {
            get { return mPassword; }
            set { mPassword = value; }
        }
        #endregion

        #region Name
        /// <summary>
        ///
        /// </summary>
        private string mName;
        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }
        #endregion

        #region Sex
        /// <summary>
        ///
        /// </summary>
        private string mSex;
        public string Sex
        {
            get { return mSex; }
            set { mSex = value; }
        }
        #endregion

        #region Job
        /// <summary>
        ///
        /// </summary>
        private string mJob;
        public string Job
        {
            get { return mJob; }
            set { mJob = value; }
        }
        #endregion

        #region TelNo
        /// <summary>
        ///
        /// </summary>
        private string mTelNo;
        public string TelNo
        {
            get { return mTelNo; }
            set { mTelNo = value; }
        }
        #endregion

        #region Mobile
        /// <summary>
        ///
        /// </summary>
        private string mMobile;
        public string Mobile
        {
            get { return mMobile; }
            set { mMobile = value; }
        }
        #endregion

        #region Email
        /// <summary>
        ///
        /// </summary>
        private string mEmail;
        public string Email
        {
            get { return mEmail; }
            set { mEmail = value; }
        }
        #endregion

        #region QQ
        /// <summary>
        ///
        /// </summary>
        private string mQQ;
        public string QQ
        {
            get { return mQQ; }
            set { mQQ = value; }
        }
        #endregion

        #region EduLevel
        /// <summary>
        ///
        /// </summary>
        private string mEduLevel;
        public string EduLevel
        {
            get { return mEduLevel; }
            set { mEduLevel = value; }
        }
        #endregion

        #region School
        /// <summary>
        ///
        /// </summary>
        private string mSchool;
        public string School
        {
            get { return mSchool; }
            set { mSchool = value; }
        }
        #endregion

        #region LoginSystems
        /// <summary>
        ///
        /// </summary>
        private string mLoginSystems;
        public string LoginSystems
        {
            get { return mLoginSystems; }
            set { mLoginSystems = value; }
        }
        #endregion

        #region CreateID
        /// <summary>
        ///
        /// </summary>
        private Decimal mCreateID;
        public Decimal CreateID
        {
            get { return mCreateID; }
            set { mCreateID = value; }
        }
        #endregion

        #region CreateDate
        /// <summary>
        ///
        /// </summary>
        private DateTime mCreateDate = DateTime.MinValue;
        public DateTime CreateDate
        {
            get { return mCreateDate; }
            set { mCreateDate = value; }
        }
        #endregion

        #endregion

        #region GetReCorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>Ts_User_FlowDP</returns>
        public Ts_User_FlowDP GetReCorded(long lngID)
        {
            Ts_User_FlowDP ee = new Ts_User_FlowDP();
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = null;
            try
            {
                strSQL = "SELECT * FROM Ts_User_Flow WHERE ID = " + lngID.ToString();
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);                
            }
            finally { ConfigTool.CloseConnection(cn); }

            foreach (DataRow dr in dt.Rows)
            {
                ee.UserID = Decimal.Parse(dr["UserID"].ToString());
                ee.FlowId = Decimal.Parse(dr["FlowId"].ToString());
                ee.NodeModelID = Decimal.Parse(dr["NodeModelID"].ToString());
                ee.FlowModelID = Decimal.Parse(dr["FlowModelID"].ToString());
                ee.LoginName = dr["LoginName"].ToString();
                //if (System.Configuration.ConfigurationSettings.AppSettings["Is64Machine"] != null
                //                && System.Configuration.ConfigurationSettings.AppSettings["Is64Machine"].ToLower() == "true")
                if ( WebConfigTool.GetValue("Is64Machine","true").Equals("true"))
                {
                    ee.Password = EpowerGlobal.MessageGlobal.DeCryptData(dr["Password"].ToString(), CONST_ENCRYKEY).Trim();
                }
                else
                {
                    ee.Password = EncryTool.DeCrypt(dr["Password"].ToString(), CONST_ENCRYKEY).Trim();
                }
                ee.Name = dr["Name"].ToString();
                ee.Sex = dr["Sex"].ToString();
                ee.Job = dr["Job"].ToString();
                ee.TelNo = dr["TelNo"].ToString();
                ee.Mobile = dr["Mobile"].ToString();
                ee.Email = dr["Email"].ToString();
                ee.QQ = dr["QQ"].ToString();
                ee.EduLevel = dr["EduLevel"].ToString();
                ee.School = dr["School"].ToString();
                ee.LoginSystems = dr["LoginSystems"].ToString();
                ee.CreateID = Decimal.Parse(dr["CreateID"].ToString());
                ee.CreateDate = dr["CreateDate"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["CreateDate"].ToString());
            }
            return ee;
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
                strSQL = "SELECT * FROM Ts_User_Flow Where 1=1 And Deleted=0 ";
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
        /// <param name=pTs_User_FlowDP></param>
        public void InsertRecorded(Ts_User_FlowDP pTs_User_FlowDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            string strID = "0";
            try
            {
                strID = EpowerGlobal.EPGlobal.GetNextID("USER_ID").ToString();
                pTs_User_FlowDP.UserID = decimal.Parse(strID);
                strSQL = @"INSERT INTO Ts_User_Flow(
									UserID,
									FlowId,
									NodeModelID,
									FlowModelID,
									LoginName,
									Password,
									Name,
									Sex,
									Job,
									TelNo,
									Mobile,
									Email,
									QQ,
									EduLevel,
									School,
									LoginSystems,
									CreateID,
									CreateDate
					)
					VALUES( " +
                            strID.ToString() + "," +
                            pTs_User_FlowDP.FlowId.ToString() + "," +
                            pTs_User_FlowDP.NodeModelID.ToString() + "," +
                            pTs_User_FlowDP.FlowModelID.ToString() + "," +
                            StringTool.SqlQ(pTs_User_FlowDP.LoginName) + "," +
                            StringTool.SqlQ(pTs_User_FlowDP.Password) + "," +
                            StringTool.SqlQ(pTs_User_FlowDP.Name) + "," +
                            pTs_User_FlowDP.Sex.ToString() + "," +
                            StringTool.SqlQ(pTs_User_FlowDP.Job) + "," +
                            StringTool.SqlQ(pTs_User_FlowDP.TelNo) + "," +
                            StringTool.SqlQ(pTs_User_FlowDP.Mobile) + "," +
                            StringTool.SqlQ(pTs_User_FlowDP.Email) + "," +
                            StringTool.SqlQ(pTs_User_FlowDP.QQ) + "," +
                            pTs_User_FlowDP.EduLevel.ToString() + "," +
                            StringTool.SqlQ(pTs_User_FlowDP.School) + "," +
                            StringTool.SqlQ(pTs_User_FlowDP.LoginSystems) + "," +
                            pTs_User_FlowDP.CreateID.ToString() + "," +
                            (pTs_User_FlowDP.CreateDate == DateTime.MinValue ? " null " : StringTool.SqlQ(pTs_User_FlowDP.CreateDate.ToString())) +
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
        /// <param name=pTs_User_FlowDP></param>
        public void UpdateRecorded(Ts_User_FlowDP pTs_User_FlowDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE Ts_User_Flow Set " +
                                                        " FlowId = " + pTs_User_FlowDP.FlowId.ToString() + "," +
                            " NodeModelID = " + pTs_User_FlowDP.NodeModelID.ToString() + "," +
                            " FlowModelID = " + pTs_User_FlowDP.FlowModelID.ToString() + "," +
                            " LoginName = " + StringTool.SqlQ(pTs_User_FlowDP.LoginName) + "," +
                            " Password = " + StringTool.SqlQ(pTs_User_FlowDP.Password) + "," +
                            " Name = " + StringTool.SqlQ(pTs_User_FlowDP.Name) + "," +
                            " Sex = " + pTs_User_FlowDP.Sex.ToString() + "," +
                            " Job = " + StringTool.SqlQ(pTs_User_FlowDP.Job) + "," +
                            " TelNo = " + StringTool.SqlQ(pTs_User_FlowDP.TelNo) + "," +
                            " Mobile = " + StringTool.SqlQ(pTs_User_FlowDP.Mobile) + "," +
                            " Email = " + StringTool.SqlQ(pTs_User_FlowDP.Email) + "," +
                            " QQ = " + StringTool.SqlQ(pTs_User_FlowDP.QQ) + "," +
                            " EduLevel = " + pTs_User_FlowDP.EduLevel.ToString() + "," +
                            " School = " + StringTool.SqlQ(pTs_User_FlowDP.School) + "," +
                            " LoginSystems = " + StringTool.SqlQ(pTs_User_FlowDP.LoginSystems) + "," +
                            " CreateID = " + pTs_User_FlowDP.CreateID.ToString() + "," +
                            " CreateDate = " + (pTs_User_FlowDP.CreateDate == DateTime.MinValue ? " null " : StringTool.SqlQ(pTs_User_FlowDP.CreateDate.ToString())) +
                                " WHERE UserID = " + pTs_User_FlowDP.UserID.ToString();

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
                string strSQL = "Delete Ts_User_Flow WHERE ID =" + lngID.ToString();
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

        /// <summary>
        /// 检查用户是否存在
        /// </summary>
        /// <param name="LoginName"></param>
        /// <param name="lngUserID"></param>
        /// <returns></returns>
        public static DataTable CheckUser(string LoginName,long lngUserID)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = @"SELECT LoginName,Name,Sex,Ts_Dept.DeptID,Ts_User.Deleted,
									GetFullDept(Ts_Dept.FullID,'-->') FullDeptName,Job,TelNo,Mobile,Email,
									Edulevel
							 FROM Ts_User
							 LEFT OUTER JOIN Ts_UserDept ON Ts_User.UserId=  Ts_UserDept.UserId
							 LEFT OUTER JOIN Ts_Dept ON Ts_UserDept.DeptID=  Ts_Dept.DeptID
							 WHERE ROWNUM<=1 AND LoginName =" + StringTool.SqlQ(LoginName);
            if (lngUserID != 0)
            {
                strSQL += " And Ts_User.UserID<>" + lngUserID.ToString();
            }
            strSQL += @" UNION ALL SELECT LoginName,Name,Sex,Ts_Dept.DeptID,2,
									GetFullDept(Ts_Dept.FullID,'-->') FullDeptName,Job,TelNo,Mobile,Email,
									Edulevel
							 FROM Ts_User_Flow
							 LEFT OUTER JOIN Ts_UserDept_Flow ON Ts_User_Flow.UserId=  Ts_UserDept_Flow.UserId
							 LEFT OUTER JOIN Ts_Dept ON Ts_UserDept_Flow.DeptID=  Ts_Dept.DeptID
							 WHERE ROWNUM<=1 AND LoginName =" + StringTool.SqlQ(LoginName);
            if (lngUserID != 0)
            {
                strSQL += " And Ts_User_Flow.UserID<>" + lngUserID.ToString();
            }

            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);                

                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
       
    }
}
