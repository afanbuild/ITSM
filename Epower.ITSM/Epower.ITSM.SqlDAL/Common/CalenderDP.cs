using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;
using System.Text;
using System.Collections;
using System.Collections.Generic;


namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// 
    /// </summary>
    public class CalenderDP
    {
        /// <summary>
        /// 
        /// </summary>
        public CalenderDP() { }
        #region 封装字段
        private decimal id;

        public decimal Id
        {
            get { return id; }
            set { id = value; }
        }
        private string caldate;

        public string Caldate
        {
            get { return caldate; }
            set { caldate = value; }
        }
        private int caltype;

        public int Caltype
        {
            get { return caltype; }
            set { caltype = value; }
        }
        private decimal compareid;

        public decimal Compareid
        {
            get { return compareid; }
            set { compareid = value; }
        }
        private decimal inputuser;

        public decimal Inputuser
        {
            get { return inputuser; }
            set { inputuser = value; }
        }
        private DateTime inputtime;

        public DateTime Inputtime
        {
            get { return inputtime; }
            set { inputtime = value; }
        }
        private decimal updateuser;

        public decimal Updateuser
        {
            get { return updateuser; }
            set { updateuser = value; }
        }
        private DateTime updatetime;

        public DateTime Updatetime
        {
            get { return updatetime; }
            set { updatetime = value; }
        }
        private int deleted;

        public int Deleted
        {
            get { return deleted; }
            set { deleted = value; }
        }
        #endregion

        private decimal deptid;

        public decimal Deptid
        {
            get { return deptid; }
            set { deptid = value; }
        }
        private int deptkind;

        public int Deptkind
        {
            get { return deptkind; }
            set { deptkind = value; }
        }
        private string deptname;

        public string Deptname
        {
            get { return deptname; }
            set { deptname = value; }
        }

        #region GetReCorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>Ea_DefineMainPageDP</returns>
        public CalenderDP GetReCorded(long lngID)
        {
            CalenderDP cd = new CalenderDP();
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM ts_calender WHERE ID = " + lngID.ToString();
            DataTable dt = null;
            try { dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL); }
            finally { ConfigTool.CloseConnection(cn); }            
            foreach (DataRow dr in dt.Rows)
            {
                cd.Id = Decimal.Parse(dr["ID"].ToString());
                cd.Caldate = dr["caldate"].ToString();
                cd.Caltype = int.Parse(dr["caltype"].ToString());
                cd.Compareid = decimal.Parse(dr["compareid"].ToString());
                cd.Inputuser = decimal.Parse(dr["inputuser"].ToString());
                cd.Inputtime = dr["inputtime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["inputtime"].ToString());
                cd.Updateuser = decimal.Parse(dr["updateuser"].ToString());
                cd.Updatetime = dr["updatetime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["updatetime"].ToString());
            }
            return cd;
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
            strSQL = @"select *,case a.CalType 
	                    when 1 then (SELECT DeptName||'('||TO_CHAR(DeptID)||')' FROM Ts_Dept WHERE ROWNUM<=1 AND DeptID=a.CompareID) 
	                    when 2 then (SELECT DeptName||'('||TO_CHAR(DeptID)||')' FROM Ts_Dept WHERE ROWNUM<=1 AND DeptID=a.CompareID) 
	                    when 3 then (SELECT Name||'('||TO_CHAR(UserID)||')' FROM Ts_User WHERE ROWNUM<=1 AND UserID=a.CompareID) 
	                    when 0 then '全局' end as ObjectName,
	                    case a.CalType 
	                    when 1 then '机构' 
	                    when 2 then '部门' 
	                    when 3 then '人员' 
	                    when 0 then '全局' end as ObjectType
	                    from Ts_Calender a Where 1=1 And a.Deleted=0";
            strSQL += sWhere;
            strSQL += sOrder;
            try
            {
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
        /// <param name=cd></param>
        public void InsertRecorded(CalenderDP cd)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            string strID = "0";
            try
            {
                strSQL = @"INSERT INTO ts_calender(caldate,caltype,compareid,inputuser,inputtime,updateuser,updatetime,deleted)
					VALUES( ";
                strSQL += StringTool.SqlQ(cd.Caldate) + ",";
                strSQL += cd.Caltype + ",";
                strSQL += cd.Compareid + ",";
                strSQL += cd.Inputuser + ",";
                strSQL += (cd.Inputtime == DateTime.MinValue ? " null " : StringTool.SqlQ(cd.Inputtime.ToString())) + ",";
                strSQL += cd.Updateuser + ",";
                strSQL += (cd.Updatetime == DateTime.MinValue ? " null " : StringTool.SqlQ(cd.Updatetime.ToString())) + ",0)";

                strSQL += " select @@IDENTITY";
                cd.Id = (decimal)OracleDbHelper.ExecuteScalar(cn, CommandType.Text, strSQL);
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

        #region GetAllKind
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<CalenderDP> GetAllKind()
        {
            string sql = "select deptid,deptname from ts_dept where deptkind=1";
            OracleConnection cn = ConfigTool.GetConnection();
            cn.Open();
            List<CalenderDP> list = new List<CalenderDP>();
            using (OracleCommand cmd = new OracleCommand(sql, cn))
            {
                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    CalenderDP cd = new CalenderDP();
                    cd.Deptid = decimal.Parse(dr["deptid"].ToString());
                    cd.Deptname = dr["deptname"].ToString();
                    list.Add(cd);
                }
                dr.Close();
            }
            ConfigTool.CloseConnection(cn);
            return list;
        }
        #endregion 

        #region UpdateRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=cd></param>
        public void UpdateRecorded(CalenderDP cd)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE ts_calender Set " +
                            " caldate = " + StringTool.SqlQ(cd.Caldate) + "," +
                            " caltype = " + cd.Caltype + "," +
                            " compareid = " + cd.Compareid + "," +
                            " updateuser = " + cd.Updateuser + "," +
                            " updatetime = '" + (cd.Updatetime == DateTime.MinValue ? " null " : cd.Updatetime.ToString()) + "'" +
                            " WHERE ID = " + cd.Id.ToString();

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
                string strSQL = "Update ts_calender Set Deleted=1  WHERE ID =" + lngID.ToString();
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


        #region 判断重复值 CheckIsRepeat
        /// <summary>
        /// 
        /// </summary>
        /// <param name="icaltype"></param>
        /// <param name="dCompareID"></param>
        /// <param name="dID"></param>
        /// <param name="sDate"></param>
        /// <returns></returns>
        public static bool CheckIsRepeat(int icaltype, decimal dCompareID, decimal dID, string sDate)
        {
            bool sReturn = false;
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                strSQL = "SELECT ID FROM ts_calender Where 1=1 And Deleted=0 And CalType=" + icaltype.ToString()
                + " And CompareID=" + dCompareID.ToString()
                + " And Caldate=" + StringTool.SqlQ(sDate);
                if (dID != 0)
                    strSQL += " and ID<>" + dID.ToString();
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                if (dt.Rows.Count > 0)
                    sReturn = true;
                
                return sReturn;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion 
    }
}

