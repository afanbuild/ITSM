
/*******************************************************************
 *
 * Description
 * 
 * 
 * Create By  :Administrator
 * Create Date:2009Äê6ÔÂ11ÈÕ
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
    public class Ea_MailTempleteDP
    {
        /// <summary>
        /// 
        /// </summary>
        public Ea_MailTempleteDP()
        { }

        #region Property
        #region id
        /// <summary>
        ///
        /// </summary>
        private Decimal mid;
        public Decimal id
        {
            get { return mid; }
            set { mid = value; }
        }
        #endregion

        #region MailTitle
        /// <summary>
        ///
        /// </summary>
        private String mMailTitle = string.Empty;
        public String MailTitle
        {
            get { return mMailTitle; }
            set { mMailTitle = value; }
        }
        #endregion

        #region MailBody
        /// <summary>
        ///
        /// </summary>
        private String mMailBody;
        public String MailBody
        {
            get { return mMailBody; }
            set { mMailBody = value; }
        }
        #endregion

        #region Deleted
        /// <summary>
        ///
        /// </summary>
        private int mDeleted;
        public int Deleted
        {
            get { return mDeleted; }
            set { mDeleted = value; }
        }
        #endregion

        #endregion

        #region GetReCorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>Ea_MailTempleteDP</returns>
        public Ea_MailTempleteDP GetReCorded(long lngID)
        {
            Ea_MailTempleteDP ee = new Ea_MailTempleteDP();
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT * FROM Ea_MailTemplete WHERE ID = " + lngID.ToString();
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                
                foreach (DataRow dr in dt.Rows)
                {
                    ee.id = Decimal.Parse(dr["id"].ToString());
                    ee.MailTitle = dr["MailTitle"].ToString();
                    ee.MailBody = dr["MailBody"].ToString();
                    ee.Deleted = Int32.Parse(dr["Deleted"].ToString());
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
                strSQL = "SELECT * FROM Ea_MailTemplete Where 1=1 And Deleted=0 ";
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
        /// <param name=pEa_MailTempleteDP></param>
        public void InsertRecorded(Ea_MailTempleteDP pEa_MailTempleteDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            string strID = "0";
            try
            {
                strID = EpowerGlobal.EPGlobal.GetNextID("Ea_MailTempleteID").ToString();
                pEa_MailTempleteDP.id = decimal.Parse(strID);
                strSQL = @"INSERT INTO Ea_MailTemplete(
									id,
									MailTitle,
									MailBody,
									Deleted
					)
					VALUES( " +
                            strID.ToString() + "," +
                            StringTool.SqlQ(pEa_MailTempleteDP.MailTitle) + "," +
                            StringTool.SqlQ(pEa_MailTempleteDP.MailBody) + "," +
                            pEa_MailTempleteDP.Deleted.ToString() +
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
        /// <param name=pEa_MailTempleteDP></param>
        public void UpdateRecorded(Ea_MailTempleteDP pEa_MailTempleteDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE Ea_MailTemplete Set " +
                                                        " MailTitle = " + StringTool.SqlQ(pEa_MailTempleteDP.MailTitle) + "," +
                            " MailBody = " + StringTool.SqlQ(pEa_MailTempleteDP.MailBody) + "," +
                            " Deleted = " + pEa_MailTempleteDP.Deleted.ToString() +
                                " WHERE id = " + pEa_MailTempleteDP.id.ToString();

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
                string strSQL = "Update Ea_MailTemplete Set Deleted=1  WHERE ID =" + lngID.ToString();
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

