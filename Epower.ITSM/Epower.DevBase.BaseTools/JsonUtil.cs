using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Epower.DevBase.BaseTools
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonUtil
    {
        /// <summary>
        /// 
        /// </summary>
        public JsonUtil()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static StringBuilder ChangeToJson(DataTable dt)
        {
            if (dt == null)
            {
                return new StringBuilder();
            }
            StringBuilder jsonBuilder = new StringBuilder();
            //jsonBuilder.Append("{\"");
            //jsonBuilder.Append(dt.TableName.ToString());
            jsonBuilder.Append("[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Rows[i][j].ToString() != "null")
                    {
                        jsonBuilder.Append("\"");
                        jsonBuilder.Append(dt.Columns[j].ColumnName.ToLower());
                        jsonBuilder.Append("\":\"");
                        jsonBuilder.Append(dt.Rows[i][j].ToString());
                        jsonBuilder.Append("\",");
                    }
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            //jsonBuilder.Append("}");
            return jsonBuilder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="drs"></param>
        /// <returns></returns>
        public static StringBuilder ChangeToJson(DataRow[] drs)
        {
            if (drs == null || drs.Length == 0)
            {
                return new StringBuilder();
            }
            DataTable dt = new DataTable();
            for (int i = 0; i < drs.Length; i++)
            {
                dt.ImportRow(drs[i]);
            }
            return ChangeToJson(dt); 
        }
    }
}
