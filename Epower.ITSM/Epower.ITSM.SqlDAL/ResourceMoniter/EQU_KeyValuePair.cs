using System;
using System.Collections.Generic;
using System.Text;

namespace Epower.ITSM.SqlDAL.ResourceMoniter
{
    /// <summary>
    /// 自定义键值对结构
    /// </summary>
    public class EQU_KeyValuePair<T, T1>
    {
        public EQU_KeyValuePair() { }
        public EQU_KeyValuePair(T key, T1 value)
        {
            this.Key = key;
            this.Value = value;
        }
        /// <summary>
        /// 键
        /// </summary>
        public T Key { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public T1 Value { get; set; }
    }
}
