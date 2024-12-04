using System;
using System.Collections.Generic;
using System.Text;

namespace Epower.ITSM.SqlDAL.ResourceMoniter
{
    /// <summary>
    /// 规则文件
    /// </summary>
    public class Rule
    {
        /// <summary>
        /// 规则编号
        /// </summary>
        public String RuleId { get; set; }
        /// <summary>
        /// 资源编号
        /// </summary>
        public String ResourceId { get; set; }
        /// <summary>
        /// 规则描述
        /// </summary>
        public String Description { get; set; }
        /// <summary>
        /// 逻辑判定规则
        /// </summary>
        public LogicWay Way { get; set; }
        /// <summary>
        /// 判定项集
        /// </summary>
        public List<OPPair> OPPairs { get; set; }
        /// <summary>
        /// 提示图片
        /// </summary>
        public String AlertImage { get; set; }
        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CTIME { get; set; }
    }
}
