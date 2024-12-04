using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Epower.ITSM.SqlDAL.ResourceMoniter
{
    /// <summary>
    /// 来自监控系统的标准数据
    /// </summary>
    public class EStandardData : StandardObject.StandardData
    {
        /// <summary>
        /// 判定项
        /// </summary>
        private OPPair _op_pair;
        /// <summary>
        /// 数值类型
        /// </summary>
        private static String _INTEGER_TYPE = "integer";
        /// <summary>
        /// 字串类型
        /// </summary>
        private static String _STRING_TYPE = "string";

        public EStandardData()
            : base()
        {
        }

        public EStandardData(StandardObject.StandardData data)
            : base()
        {
        }


        public EStandardData(OPPair opPair, StandardObject.StandardData data)
            : this(data)
        {
            this._op_pair = opPair;

            // 安装数据
            FillData(data);

            // 过滤非法值
            switch (this.Type)
            {
                case "integer":
                    try
                    {
                        this.Value = Int32.Parse(this.Value).ToString();
                        this._op_pair.Value = Int32.Parse(this._op_pair.Value).ToString();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(String.Format(@" 初始化 EStandardData 对象 | 
                                    预设值错误 | 当前值 {1} | 预设值 {2} | {0}", ex.Message, this.Value, this._op_pair.Value));
                    }
                    break;
                case "string":
                    this.Value = this.Value.Trim();
                    this._op_pair.Value = this._op_pair.Value.Trim();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 填入接收到的数据
        /// </summary>
        /// <param name="data"></param>
        private void FillData(StandardObject.StandardData data)
        {
            PropertyInfo[] p = typeof(StandardObject.StandardData).GetProperties();
            foreach (PropertyInfo item in p)
            {
                Object val = item.GetValue(data, null);
                this.GetType().InvokeMember(item.Name,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
                    null,
                    this,
                    new Object[] { val });
            }
        }

        /// <summary>
        /// 该监控项是否符合预设规则
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            /* 目前没有考虑到浮点型数值 */
            Boolean isMatching = false;

            switch (_op_pair.Operator)
            {
                case OP.Equal:
                    isMatching = isEquals();
                    break;
                case OP.NotEqual:
                    isMatching = isNotEquals();
                    break;
                case OP.GreaterThan:
                    isMatching = GreaterThan();
                    break;
                case OP.GreaterThanAndEqual:
                    isMatching = GreaterThanAndEqual();
                    break;
                case OP.SmallerThan:
                    isMatching = SmallerThan();
                    break;
                case OP.SmallerThanAndEqual:
                    isMatching = SmallerThanAndEqual();
                    break;
                case OP.In:
                    isMatching = In();
                    break;
                case OP.NotIn:
                    isMatching = NotIn();
                    break;
                default:
                    isMatching = false;
                    break;
            }

            if (isMatching)
            {

                // CPU负载 小于 22%, 当前值: 12%, 参考值: 24
                return String.Format("{0} {1} {2}{3}, 当前值: {4}{3}, 参考值: {5}",
                    this.Name,
                    this._op_pair.Operator.ToString(),
                    this._op_pair.Value,
                    this.Symbol,
                    this.Value,
                    this.NormalValue);

            }
            else
                return String.Empty;

        }

        #region verify rule function
        /// <summary>
        /// 相等性判断
        /// </summary>
        /// <returns></returns>
        private Boolean isEquals()
        {
            return this._op_pair.Value == this.Value;
        }
        /// <summary>
        /// 不相等性判断
        /// </summary>
        /// <returns></returns>
        private Boolean isNotEquals()
        {
            return this._op_pair.Value != this.Value;
        }
        /// <summary>
        /// 大于
        /// </summary>
        /// <returns></returns>
        private Boolean GreaterThan()
        {
            return int.Parse(this.Value) > int.Parse(_op_pair.Value);
        }
        /// <summary>
        /// 大于等于
        /// </summary>
        /// <returns></returns>
        private Boolean GreaterThanAndEqual()
        {
            return int.Parse(this.Value) >= int.Parse(_op_pair.Value);
        }
        /// <summary>
        /// 小于
        /// </summary>
        /// <returns></returns>
        private Boolean SmallerThan()
        {
            return int.Parse(this.Value) < int.Parse(_op_pair.Value);
        }
        /// <summary>
        /// 小于等于
        /// </summary>
        /// <returns></returns>
        private Boolean SmallerThanAndEqual()
        {
            return int.Parse(this.Value) <= int.Parse(_op_pair.Value);
        }
        /// <summary>
        /// 包含
        /// </summary>
        /// <returns></returns>
        private Boolean In()
        {
            if (this.Type.Equals(_INTEGER_TYPE))
            {
                String[] str_array_nums = this._op_pair.Value.Split(new String[] { "~" }, StringSplitOptions.RemoveEmptyEntries);
                if (str_array_nums.Length <= 1) return false;

                Int32 begin = Int32.Parse(str_array_nums[0]);
                Int32 end = Int32.Parse(str_array_nums[1]);

                Int32 val = Int32.Parse(this.Value);

                return val >= begin && val <= end;
            }
            else if (this.Type.Equals(_STRING_TYPE))
            {
                String[] str_array_items = this._op_pair.Value.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (str_array_items.Length <= 1) return false;

                Boolean isHave = false;
                foreach (String item in str_array_items)
                {
                    isHave = this.Value.Contains(item);

                    if (isHave) return isHave;
                }

                return isHave;
            }

            return false;
        }
        /// <summary>
        /// 不包含
        /// </summary>
        /// <returns></returns>
        private Boolean NotIn()
        {
            if (this.Type.Equals(_INTEGER_TYPE))
            {
                String[] str_array_nums = this._op_pair.Value.Split(new String[] { "~" }, StringSplitOptions.RemoveEmptyEntries);
                if (str_array_nums.Length <= 1) return false;

                Int32 begin = Int32.Parse(str_array_nums[0]);
                Int32 end = Int32.Parse(str_array_nums[1]);

                Int32 val = Int32.Parse(this.Value);

                return val < begin || val > end;
            }
            else if (this.Type.Equals(_STRING_TYPE))
            {
                String[] str_array_items = this._op_pair.Value.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (str_array_items.Length <= 1) return false;

                Boolean isHave = false;
                foreach (String item in str_array_items)
                {
                    isHave = this.Value.Contains(item);

                    if (isHave) return isHave;
                }

                return isHave;
            }

            return false;
        }
        #endregion


    }
}
