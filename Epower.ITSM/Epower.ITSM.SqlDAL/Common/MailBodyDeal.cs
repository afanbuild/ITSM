/*******************************************************************
 *
 * Description:邮件模板处理
 * 
 * 
 * Create By  :zhumc
 * Create Date:2008年7月28日
 * *****************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// 邮件主体处理类
    /// </summary>
    public class MailBodyDeal
    {
        private static string sPath = System.Web.HttpContext.Current.Server.MapPath("..\\MailTemplate") + "\\";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MailFileName">邮件模板文件名称</param>
        /// <param name="dt">与之匹配的数据集</param>
        /// <returns>邮件正文</returns>
        public static string GetMailBody(string MailFileName,DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            using (StreamReader reader = new StreamReader(sPath + MailFileName, System.Text.Encoding.Default))
            {
                sb.Append(reader.ReadToEnd());
                reader.Close();
                string sColumnsName = string.Empty;
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            //资产编号的判断                            
                            if (dt.Columns[i].ColumnName.ToLower() == "equipmentid")
                            {
                                sColumnsName = "#?" + dt.Columns[i].ColumnName.ToLower() + "?#";
                                sb.Replace(sColumnsName, Equ_DeskDP.GetEquCodeByID(dr[dt.Columns[i].ColumnName].ToString()));
                            }
                            else
                            {
                                sColumnsName = "#?" + dt.Columns[i].ColumnName.ToLower() + "?#";
                                sb.Replace(sColumnsName, dr[dt.Columns[i].ColumnName].ToString());
                            }
                        }
                    }
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MailFileName"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetMailBody_ByString(string MailFileName, DataTable dt)
        {
            MailFileName = MailFileName.ToLower();
            string sColumnsName = string.Empty;
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        sColumnsName = "#?" + dt.Columns[i].ColumnName.ToLower() + "?#";
                        MailFileName.Replace(sColumnsName, dr[dt.Columns[i].ColumnName].ToString());
                    }
                }
            }
            return MailFileName;
        }
    }
}
