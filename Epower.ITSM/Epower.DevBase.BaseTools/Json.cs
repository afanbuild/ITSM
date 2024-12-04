using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Epower.DevBase.BaseTools
{
    /// <summary>
    /// json结构:"{'userid':'33320'}
    /// </summary>
    public class Json
    {
        /// <summary>
        /// json结构:"{'userid':'33320'}"
        /// </summary>
        public Json()
        { }

        public Json(DataTable dt)
        {
            this.dt = dt;
        }

        public Json(DataTable dt, bool IsUBB)
        {
            this.dt = dt;
            this.IsUBB = IsUBB;
        }

        public Json(DataTable dt, string DateFormat)
        {
            this.dt = dt;
            this.DateFormat = DateFormat;
        }

        public Json(DataTable dt, bool IsUBB, string DateFormat)
        {
            this.dt = dt;
            this.IsUBB = IsUBB;
            this.DateFormat = DateFormat;
        }

        /// <summary>
        /// 要转换成json的DataTable
        /// </summary>
        public DataTable dt;
        /// <summary>
        /// 日期格式,默认是"yyyy-MM-dd"格式,
        /// 需要显示完整格式只需将该属性置为""
        /// </summary>
        public string DateFormat = "yyyy-MM-dd";
        /// <summary>
        /// 是否包含UBB语法
        /// </summary>
        public bool IsUBB = false;

        /// <summary>
        /// 转换成json
        /// 此处转换的结果是一个json数组包括了"[]"
        /// 转换后需要加一个键
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            if (dt == null)
                return "[]";

            string colname = "";
            string jsonstr = "";
            string value = "";
            jsonstr = "[";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonstr += "{";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    colname = dt.Columns[j].ColumnName.ToLower();
                    value = dt.Rows[i][j].ToString().Replace("'", "&apos;").Replace("\"", "&quot;").Replace("\n", "").Replace("\r", "").Replace("\0", "").Replace("\\","/");
                    if (dt.Columns[j].DataType.Name.ToLower() == "datetime")
                        value = CTools.ToDateTime(value).ToString(DateFormat);
                    //if (IsUBB)
                    //    value = CRegex.SetCode(value);

                    if (dt.Columns[j].DataType.Name.ToLower() == "string")
                        value = HttpUtility.HtmlEncode(HttpUtility.HtmlDecode(value));
                    jsonstr += colname + ":'" + value + "'";
                    if (j < dt.Columns.Count - 1)
                        jsonstr += ",";
                }
                jsonstr += "}";
                if (i < dt.Rows.Count - 1)
                    jsonstr += ",";
            }
            jsonstr += "]";
            return jsonstr;
        }
    }
}