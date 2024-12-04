using System;
using System.Collections.Generic;
using System.Text;

namespace Epower.ITSM.SqlDAL.ResourceMoniter
{
    /// <summary>
    /// 生成转换解释器的工厂
    /// </summary>
    public class DataInterpreterFactory
    {
        /// <summary>
        /// 创建一个转换解释器
        /// </summary>
        /// <param name="strInterpreterName">解释器名</param>
        /// <returns></returns>
        public static IDataInterpreter CreateDataInterpreterAs(String strInterpreterName)
        {            
            switch (strInterpreterName)
            {
                case "HR_MONITER":    /*just a sample*/
                    IDataInterpreter interpreter = null;
                    return interpreter;
                    break;
                default:
                    throw new NotImplementedException("未实现的转换处理类");                    
            }

            return null;
        }
    }
}
