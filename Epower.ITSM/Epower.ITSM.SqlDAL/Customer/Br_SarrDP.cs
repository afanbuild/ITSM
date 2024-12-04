using System;
using System.Collections.Generic;
using System.Text;
using Epower.DevBase.BaseTools;
using System.Data.OracleClient;
using System.Data;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// 
    /// </summary>
    public class Br_SarrDP
    {
        /// <summary>
        /// 
        /// </summary>
        public void Br_ContactDP()
        { }

        #region Property 属性设置

        #region ID 主键
        /// <summary>
        ///
        /// </summary>
        private Decimal mID;
        public Decimal ID
        {
            get { return mID; }
            set { mID = value; }
        }
        #endregion

        #region SARR_ID 选择人ID
        private Decimal sARR_ID;
        public Decimal SARR_ID
        {
            get { return sARR_ID; }
            set { sARR_ID = value; }
        }
        #endregion

        #region SARR_NAME 选择人姓名

        /// <summary>
        /// 
        /// </summary>
        private String sARR_NAME = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public String SARR_NAME
        {
            get { return sARR_NAME; }
            set { sARR_NAME = value; }
        }
        #endregion

        #region SARR_DEPTNAME 选择人部门

        /// <summary>
        /// 
        /// </summary>
        private String sARR_DEPTNAME;
        /// <summary>
        /// 
        /// </summary>
        public String SARR_DEPTNAME
        {
            get { return sARR_DEPTNAME; }
            set { sARR_DEPTNAME = value; }
        }
        #endregion

        #region SARR_INDATETIME 选择时间
        /// <summary>
        /// 
        /// </summary>
        private DateTime sARR_INDATETIME;
        /// <summary>
        /// 
        /// </summary>
        public DateTime SARR_INDATETIME
        {
            get { return sARR_INDATETIME; }
            set { sARR_INDATETIME = value; }
        }
        #endregion

        #endregion

        #region 操作方法封装

        /// <summary>
        /// 添加到数据库
        /// </summary>
        /// <param name="sarrlist"></param>
        /// <returns></returns>
        public int InsertSarr(List<Br_SarrDP> sarrlist)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            cn.Open();
            string strSQL = string.Empty;
            int sum = 0;
            try
            {
                foreach (Br_SarrDP sarr in sarrlist)
                {
                    strSQL = @"INSERT INTO br_sarr(id,SARR_ID,SARR_NAME,SARR_DEPTNAME,SARR_INDATETIME)
                					VALUES( BR_SARR_SEQ.nextval,";
                    strSQL += Convert.ToDecimal(sarr.SARR_ID.ToString()) + ",";//选择人编号

                    strSQL += StringTool.SqlQ(sarr.SARR_NAME.ToString()) + ",";//选择人姓名

                    strSQL += StringTool.SqlQ(sarr.SARR_DEPTNAME.ToString()) + ",";//选择人部门

                    strSQL += (sarr.SARR_INDATETIME == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(sarr.SARR_INDATETIME.ToString()) + ",'yyyy-MM-dd HH24:mi:ss')") + ")";

                    object obj = OracleDbHelper.ExecuteScalar(cn, CommandType.Text, strSQL);
                    if (obj != null)
                    {
                        strSQL = "";
                        sum++;
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
            return sum;
        }

        /// <summary>
        /// 查询选择人信息

        /// </summary>
        /// <param name="sarr"></param>
        /// <returns></returns>
        public List<Br_SarrDP> SeachSarr()
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            cn.Open();
            strSQL = string.Format("select * from br_sarr");

            List<Br_SarrDP> list = new List<Br_SarrDP>();
            using (OracleCommand cmd = new OracleCommand(strSQL, cn))
            {
                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Br_SarrDP cd = new Br_SarrDP();
                    cd.ID = Convert.ToDecimal(dr["ID"].ToString());
                    cd.SARR_ID = Convert.ToDecimal(dr["SARR_ID"].ToString());
                    cd.SARR_NAME = dr["SARR_NAME"].ToString();
                    cd.SARR_DEPTNAME = dr["SARR_DEPTNAME"].ToString();
                    cd.SARR_INDATETIME = Convert.ToDateTime(dr["SARR_INDATETIME"].ToString());
                    list.Add(cd);
                }
            }
            ConfigTool.CloseConnection(cn);
            return list;
        }


        /// <summary>
        /// 删除选择人信息

        /// </summary>
        /// <param name="sarrlist"></param>
        /// <returns></returns>
        public int DeletdSarr()
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            int sumupnum = 0;
            try
            {
                strSQL = "delete  from br_sarr";
                sumupnum = OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSQL);
            }
            catch
            {
                throw;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
            return sumupnum;
        }
        #endregion
    }
}
