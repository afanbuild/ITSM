/****************************************************************************
 * 
 * description:动态查询操作类
 * 
 * 
 * 
 * Create by: yxq
 * Create Date:2012-12-26
 * *************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Epower.ITSM.SqlDAL
{
    public class Br_ConditionDP
    {
        #region 属性
        #region ID
        /// <summary>
        /// 标示ID
        /// </summary>
        private long mID;
        public long ID
        {
            get { return mID; }
            set { mID = value; }
        }
        #endregion

        #region ColumnName
        /// <summary>
        ///字段名
        /// </summary>
        private string mColumnName = string.Empty;
        public string ColumnName
        {
            get { return mColumnName; }
            set { mColumnName = value; }
        }
        #endregion

        #region ColRemark
        /// <summary>
        ///字段描述
        /// </summary>
        private string mColRemark = string.Empty;
        public string ColRemark
        {
            get { return mColRemark; }
            set { mColRemark = value; }
        }
        #endregion

        #region ColType
        /// <summary>
        ///字段类型 (根据字段类型不同,对应页面赋值位置展示的控件也不同：CHAR文本输入框,CATA常用类别,DEPT选择部门控件,DATE时间控件)
        /// </summary>
        private string mColType = string.Empty;
        public string ColType
        {
            get { return mColType; }
            set { mColType = value; }
        }
        #endregion

        #region CataRootID
        /// <summary>
        /// 分类RootID 如果字段类型为CATA 此字段必须设置对应的 常用类别RootID
        /// </summary>
        private long mCataRootID;
        public long CataRootID
        {
            get { return mCataRootID; }
            set { mCataRootID = value; }
        }
        #endregion

        #region TableName
        /// <summary>
        ///所属表名 (按表名进行分组)
        /// </summary>
        private string mTableName = string.Empty;
        public string TableName
        {
            get { return mTableName; }
            set { mTableName = value; }
        }
        #endregion

        #endregion

        public Br_ConditionDP()
        { }

        #region GetDataTable
        /// <summary>
        /// 根据条件查询所有数据
        /// </summary>
        /// <param name="strWhere">where 条件 如( where 1=1 and TableName='XXX' )</param>
        /// <param name="strOrder">排序字段 如( order by ID)</param>
        /// <returns></returns>
        public static DataTable GetDataTable(string strWhere, string strOrder)
        {
            string strSQL = "select * from Br_Condition ";
            strSQL = strSQL + strWhere + " " + strOrder;
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            return dt;
        }
        #endregion
    }
}
