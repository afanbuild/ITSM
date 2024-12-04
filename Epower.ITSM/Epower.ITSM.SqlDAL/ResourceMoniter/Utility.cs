using System;
using System.Collections.Generic;
using System.Text;

namespace Epower.ITSM.SqlDAL.ResourceMoniter
{
    /// <summary>
    /// 资产监控公共方法集
    /// </summary>
    public class Utility
    {
        /// <summary>
        /// 操作符转成可视符号
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        public static String Operator2Symbol(OP op)
        {
            /*
             * OP.Eqaul      ==
             * OP.NotEqual   !=
             * **/
            String str_symbol = String.Empty;

            switch (op)
            {
                case OP.Equal:
                    str_symbol = "==";
                    break;
                case OP.NotEqual:
                    str_symbol = "<>";
                    break;
                case OP.GreaterThan:
                    str_symbol = ">";
                    break;
                case OP.GreaterThanAndEqual:
                    str_symbol = ">=";
                    break;
                case OP.SmallerThan:
                    str_symbol = "<";
                    break;
                case OP.SmallerThanAndEqual:
                    str_symbol = "<=";
                    break;
                case OP.In:
                    str_symbol = "In";
                    break;
                case OP.NotIn:
                    str_symbol = "Not In";
                    break;
                default:
                    break;
            }

            return str_symbol;
        }

        /// <summary>
        /// 资源监控状态刷新间隔
        /// </summary>
        /// <returns></returns>
        public static String GetInterval()
        {
            /*
             * 间隔时间单位: 毫秒.
             * **/

            //String str_resource_state_interval = System.Configuration.ConfigurationSettings.AppSettings["Get-Resource-State-Interval"];
            String str_resource_state_interval = CommonDP
                .GetConfigValue("", "Get-Resource-State-Interval");
            if (String.IsNullOrEmpty(str_resource_state_interval)) return "1000";    // 若没有设置, 默认 1000 毫秒间隔.

            return str_resource_state_interval;
        }
    }
}
