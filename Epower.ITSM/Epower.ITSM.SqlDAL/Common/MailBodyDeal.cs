/*******************************************************************
 *
 * Description:�ʼ�ģ�崦��
 * 
 * 
 * Create By  :zhumc
 * Create Date:2008��7��28��
 * *****************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// �ʼ����崦����
    /// </summary>
    public class MailBodyDeal
    {
        private static string sPath = System.Web.HttpContext.Current.Server.MapPath("..\\MailTemplate") + "\\";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MailFileName">�ʼ�ģ���ļ�����</param>
        /// <param name="dt">��֮ƥ������ݼ�</param>
        /// <returns>�ʼ�����</returns>
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
                            //�ʲ���ŵ��ж�                            
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
