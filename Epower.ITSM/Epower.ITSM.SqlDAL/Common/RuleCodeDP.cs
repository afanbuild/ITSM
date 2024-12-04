/*******************************************************************
 *
 * Description:编号规则操作
 * 
 * 
 * Create By  :
 * Create Date:2010年03月06日
 * *****************************************************************/
using System;
using System.Data;
using System.Data.OracleClient;

using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// 
    /// </summary>
    public class RuleCodeDP
    {
        /// <summary>
        /// 根据设置规则，生成编号
        /// </summary>
        /// <param name="KeyID"></param>
        /// <returns></returns>
        public static string GetCodeBH(int KeyID)
        {
            string sReturn = string.Empty;
            string strSql = string.Empty;

            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            OracleTransaction tran = cn.BeginTransaction();
            try
            {
                strSql = @"SELECT KeyID,Prefix,Suffix,LinkChar,RuleTypeID,Step
                            FROM EA_CodeRule WHERE KeyID=" + KeyID.ToString();
                DataTable dt = OracleDbHelper.ExecuteDataset(tran, CommandType.Text, strSql).Tables[0];

                int iKeyID = 0;
                int iStep = 1;
                int iRuleTypeID = 0;
                string strLinkChar = string.Empty;  //分割符号

                string strPrefix = string.Empty;  //前缀
                string strSuffix = string.Empty;  //后缀
                string strKeyValue = string.Empty;  //除后缀的全部
                decimal dCurrBH = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    iKeyID = int.Parse(dr["KeyID"].ToString());
                    iStep = int.Parse(dr["Step"].ToString());
                    iRuleTypeID = int.Parse(dr["RuleTypeID"].ToString());
                    strLinkChar = dr["LinkChar"].ToString();

                    if (dr["Prefix"].ToString().Trim() != string.Empty)  //前缀
                        strKeyValue = dr["Prefix"].ToString().Trim() + strLinkChar;
                    if (iRuleTypeID == 0)  //当前日期
                    {
                        strKeyValue += DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    else if (iRuleTypeID == 1) //当前月份
                    {
                        strKeyValue += DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString();
                    }
                    else if (iRuleTypeID == 2) //当前年份
                    {
                        strKeyValue += DateTime.Now.Year.ToString();
                    }

                    strPrefix = strKeyValue;    //前面一部分，不包括后缀

                    if (dr["Suffix"].ToString().Trim() != string.Empty)   //后缀
                    {
                        strSuffix = strLinkChar + dr["Suffix"].ToString().Trim();
                        strKeyValue += strSuffix;
                    }
                }

                strSql = "SELECT CurrBH FROM EA_Code WHERE KeyID=" + iKeyID + " AND KeyValue=" + StringTool.SqlQ(strKeyValue);
                OracleDataReader datar = OracleDbHelper.ExecuteReader(tran, CommandType.Text, strSql);
                while (datar.Read())
                {
                    dCurrBH = datar.GetDecimal(0);
                    break;
                }
                datar.Close();

                if (dCurrBH != 0)
                {
                    strSql = " UPDATE EA_Code SET CurrBH=CurrBH+" + iStep + " WHERE KeyID=" + iKeyID + " AND KeyValue=" + StringTool.SqlQ(strKeyValue);
                    //dCurrBH += iStep; 
                }
                else
                {
                    strSql = " INSERT INTO EA_Code (KeyID,KeyValue,CurrBH) VALUES(" + iKeyID + "," + StringTool.SqlQ(strKeyValue) + ",1)";
                    //dCurrBH = 1;
                }
                OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSql);


                strSql = "SELECT CurrBH FROM EA_Code WHERE KeyID=" + iKeyID + " AND KeyValue=" + StringTool.SqlQ(strKeyValue);
                OracleDataReader dataread = OracleDbHelper.ExecuteReader(tran, CommandType.Text, strSql);
                while (dataread.Read())
                {
                    dCurrBH = dataread.GetDecimal(0);
                    break;
                }
                dataread.Close();

                sReturn = strPrefix + strLinkChar + dCurrBH.ToString() + strSuffix;   //前缀部分与规则部分 + 分割符号 + 编号 + 后缀部分

                tran.Commit();
                return sReturn;
            }
            catch (Exception e)
            {
                tran.Rollback();
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }


        /// <summary>
        /// 根据设置规则，生成编号
        /// </summary>
        /// <param name="KeyID"></param>
        /// <param name="sKeyValue"></param>
        /// <returns></returns>
        public static string GetCodeBH2(int KeyID, ref string sKeyValue)
        {
            string sReturn = string.Empty;
            string strSql = string.Empty;

            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            OracleTransaction tran = cn.BeginTransaction();
            try
            {
                strSql = @"SELECT KeyID,Prefix,Suffix,LinkChar,RuleTypeID,Step
                            FROM EA_CodeRule WHERE KeyID=" + KeyID.ToString();
                DataTable dt = OracleDbHelper.ExecuteDataset(tran, CommandType.Text, strSql).Tables[0];

                int iKeyID = 0;
                int iStep = 1;
                int iRuleTypeID = 0;
                string strLinkChar = string.Empty;  //分割符号

                string strPrefix = string.Empty;  //前缀
                string strSuffix = string.Empty;  //后缀
                string strKeyValue = string.Empty;  //除后缀的全部
                decimal dCurrBH = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    iKeyID = int.Parse(dr["KeyID"].ToString());
                    iStep = int.Parse(dr["Step"].ToString());
                    iRuleTypeID = int.Parse(dr["RuleTypeID"].ToString());
                    strLinkChar = dr["LinkChar"].ToString();

                    if (dr["Prefix"].ToString().Trim() != string.Empty)  //前缀
                        strKeyValue = dr["Prefix"].ToString().Trim() + strLinkChar;
                    if (iRuleTypeID == 0)  //当前日期
                    {
                        strKeyValue += DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    else if (iRuleTypeID == 1) //当前月份
                    {
                        string sYear = DateTime.Now.Year.ToString();
                        string sMonth = DateTime.Now.Month.ToString();
                        if (sYear.Length == 1)
                            sYear = "0" + sYear;
                        if (sMonth.Length == 1)
                            sMonth = "0" + sMonth;
                        strKeyValue += sYear + sMonth;
                    }
                    else if (iRuleTypeID == 2) //当前年份
                    {
                        strKeyValue += DateTime.Now.Year.ToString();
                    }

                    strPrefix = strKeyValue;    //前面一部分，不包括后缀

                    if (dr["Suffix"].ToString().Trim() != string.Empty)   //后缀
                    {
                        strSuffix = strLinkChar + dr["Suffix"].ToString().Trim();
                        strKeyValue += strSuffix;
                    }
                }

                strSql = "SELECT CurrBH FROM EA_Code WHERE KeyID=" + iKeyID + " AND KeyValue=" + StringTool.SqlQ(strKeyValue);
                OracleDataReader datar = OracleDbHelper.ExecuteReader(tran, CommandType.Text, strSql);
                while (datar.Read())
                {
                    dCurrBH = datar.GetDecimal(0);
                    break;
                }
                datar.Close();

                if (dCurrBH != 0)
                {
                    strSql = " UPDATE EA_Code SET CurrBH=CurrBH+" + iStep + " WHERE KeyID=" + iKeyID + " AND KeyValue=" + StringTool.SqlQ(strKeyValue);
                    //dCurrBH += iStep; 
                }
                else
                {
                    strSql = " INSERT INTO EA_Code (KeyID,KeyValue,CurrBH) VALUES(" + iKeyID + "," + StringTool.SqlQ(strKeyValue) + ",1)";
                    //dCurrBH = 1;
                }
                OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSql);


                strSql = "SELECT CurrBH FROM EA_Code WHERE KeyID=" + iKeyID + " AND KeyValue=" + StringTool.SqlQ(strKeyValue);
                OracleDataReader dataread = OracleDbHelper.ExecuteReader(tran, CommandType.Text, strSql);
                while (dataread.Read())
                {
                    dCurrBH = dataread.GetDecimal(0);
                    break;
                }
                dataread.Close();

                sReturn = dCurrBH.ToString();   //前缀部分与规则部分 + 分割符号 + 编号 + 后缀部分
                sKeyValue = strKeyValue + strLinkChar;

                tran.Commit();
                return sReturn;
            }
            catch (Exception e)
            {
                tran.Rollback();
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }
    }
}
