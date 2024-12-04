using System;
using System.Collections.Generic;
using System.Text;
using StandardObject;

namespace Epower.ITSM.SqlDAL.ResourceMoniter
{
    /// <summary>
    /// 数据处理转换类
    /// </summary>
    public interface IDataInterpreter
    {
        /// <summary>
        /// 支持的资源文件集
        /// </summary>
        /// <returns></returns>
        List<String> GetAvailableResources();
        /// <summary>
        /// 取该资源文件的标准数据
        /// </summary>
        /// <param name="strResourceID">资源文件ID</param>
        /// <returns></returns>
        StandardData GetFormattedData(String strResourceID);
    }
}
