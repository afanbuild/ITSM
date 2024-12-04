using System;
using System.Collections.Generic;
using System.Text;

namespace Epower.ITSM.SqlDAL.ResourceMoniter
{
    public class WrapperJSONResult
    {
        /// <summary>
        /// 操作状态
        /// </summary>
        public Int32 Status { get; set; }
        /// <summary>
        /// 提示文本
        /// </summary>
        public String Message { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public Object Data { get; set; }
    }
}
