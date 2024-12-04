using System;
using System.Collections.Generic;
using System.Text;

namespace Epower.ITSM.SqlDAL.ResourceMoniter
{
    /// <summary>
    /// 判定项
    /// </summary>
    public struct OPPair
    {
        /// <summary>
        /// 指标名
        /// </summary>
        public String Key;
        /// <summary>
        /// 判定值
        /// </summary>
        public String Value;
        /// <summary>
        /// 操作符
        /// </summary>
        public OP Operator;
    }

    /// <summary>
    /// 运算符
    /// </summary>
    public enum OP
    {
        /// <summary>
        /// 相等
        /// </summary>
        Equal,
        /// <summary>
        /// 不相等
        /// </summary>
        NotEqual,
        /// <summary>
        /// 大于
        /// </summary>
        GreaterThan,
        /// <summary>
        /// 大于等于
        /// </summary>
        GreaterThanAndEqual,
        /// <summary>
        /// 小于
        /// </summary>
        SmallerThan,
        /// <summary>
        /// 小于等于
        /// </summary>
        SmallerThanAndEqual,
        /// <summary>
        /// 在某个范围
        /// </summary>
        In,
        /// <summary>
        /// 不在某个范围
        /// </summary>
        NotIn
    }

    /// <summary>
    /// 逻辑关系
    /// </summary>
    public enum LogicWay
    {
        /// <summary>
        /// 并且
        /// </summary>
        And,
        /// <summary>
        /// 或者
        /// </summary>
        Or
    }
}
