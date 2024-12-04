using System;
using System.Collections.Generic;
using System.Text;

namespace Epower.ITSM.SqlDAL.ResourceMoniter
{
    /// <summary>
    /// 按规则优先级排序
    /// </summary>
    internal class PriorityOfRuleComparer : IComparer<JSONOfAlertMessage>
    {
        #region IComparer<JSONOfAlertMessage> 成员

        public int Compare(JSONOfAlertMessage x, JSONOfAlertMessage y)
        {
            return x.Priority.CompareTo(y.Priority);
        }

        #endregion
    }
}
