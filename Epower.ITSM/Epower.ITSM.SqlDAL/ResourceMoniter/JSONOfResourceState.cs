using System;
using System.Collections.Generic;
using System.Text;

namespace Epower.ITSM.SqlDAL.ResourceMoniter
{
    public class JSONOfResourceState
    {
        /// <summary>
        /// 编号
        /// </summary>
        public String ResourceId { get; set; }
        /// <summary>
        /// 查看地址
        /// </summary>
        public String URLAddress { get; set; }
        /// <summary>
        /// 提示文本
        /// </summary>
        public List<JSONOfAlertMessage> MessageList { get; set; }

        public JSONOfResourceState() { }
    }

    public class JSONOfAlertMessage
    {
        /// <summary>
        /// 编号
        /// </summary>
        public String RuleId { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public String Description { get; set; }
        /// <summary>
        /// 提示图标
        /// </summary>
        public String AlertImage { get; set; }
        /// <summary>
        /// 优先级
        /// </summary>
        public String Priority { get; set; }
        /// <summary>
        /// 消息文本
        /// </summary>
        public List<JSONOfOPPairMessage> Messages { get; set; }


        public JSONOfAlertMessage() { }
    }

    public class JSONOfOPPairMessage
    {
        /// <summary>
        /// 指标名
        /// </summary>
        public String Key { get; set; }
        /// <summary>
        /// 操作符
        /// </summary>
        public String Operator { get; set; }
        /// <summary>
        /// 值符号
        /// </summary>
        public String Symbol { get; set; }
        /// <summary>
        /// 预设值
        /// </summary>
        public String Preset { get; set; }
        /// <summary>
        /// 当前值
        /// </summary>
        public String Value { get; set; }
        /// <summary>
        /// 通常值
        /// </summary>
        public String NormalValue { get; set; }

        public JSONOfOPPairMessage() { }
    }
}
