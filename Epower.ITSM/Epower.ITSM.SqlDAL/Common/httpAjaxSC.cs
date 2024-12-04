using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.SqlDAL
{
    public class httpAjaxSC
    {
        public httpAjaxSC()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// 取消常用功能
        /// </summary>
        /// <param name="pageName"></param>
        /// <param name="pageUrl"></param>
        /// <param name="strManager"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static bool deletePageAllType(string pageName, string pageUrl, string strManager, string UserId)
        {
            try
            {
                string strSQL = string.Empty;
                if (strManager == "true")
                {
                    //系统管理员的操作
                    strSQL = " delete Es_PageOften where pageName=" + StringTool.SqlQ(pageName) + @"  and UserId=" + StringTool.SqlQ(UserId);
                    CommonDP.ExcuteSql(strSQL);
                    strSQL = "delete Es_PageOften where pageName=" + StringTool.SqlQ(pageName) + @"  and pageType=1";
                    CommonDP.ExcuteSql(strSQL);
                }
                else
                {
                    //非系统管理员
                    strSQL = " delete Es_PageOften where pageName=" + StringTool.SqlQ(pageName) + @"  and UserId=" + StringTool.SqlQ(UserId);
                    CommonDP.ExcuteSql(strSQL);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }


        }

        /// <summary>
        /// 获得知识类别的名字（根据id）

        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static string InfCategoryName(string ID)
        {
            try
            {
                string strSQL = @"select * from Inf_Category where catalogid=" + StringTool.SqlQ(ID.ToString());
                DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["catalogname"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            catch
            {
                return "0";
            }
        }

        /// <summary>
        /// 设置常用功能
        /// </summary>
        /// <param name="pageName"></param>
        /// <param name="pageUrl"></param>
        /// <param name="PageType"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static bool insertPageAllType(string pageName, string pageUrl, string PageType, string UserId, string resourceKey)
        {
            try
            {
                if (resourceKey.Trim() == string.Empty)
                    resourceKey = "0";
                string strSQL = @"select pagename from Es_PageOften where pageName=" + StringTool.SqlQ(pageName) + " and UserId=" + StringTool.SqlQ(UserId);
                DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
                if (dt.Rows.Count > 0)
                {
                    strSQL = " update Es_PageOften set PageType=" + StringTool.SqlQ(PageType) + @",pageUrl=" + StringTool.SqlQ(pageUrl) + @",resourceKey=" + StringTool.SqlQ(resourceKey) + @" where pageName=" + StringTool.SqlQ(pageName) + @" and UserId=" + StringTool.SqlQ(UserId);
                }
                else
                {
                    strSQL = " insert into Es_PageOften(pageName,pageType,pageUrl,UserId,resourceKey) values (" + StringTool.SqlQ(pageName) + @"," + StringTool.SqlQ(PageType) + @"," + StringTool.SqlQ(pageUrl) + @"," + StringTool.SqlQ(UserId) + @"," + StringTool.SqlQ(resourceKey) + @")";
                }
                CommonDP.ExcuteSql(strSQL);
                return true;
            }
            catch
            {
                return false;
            }


        }


        public static DataTable getPageOftenPage(long lngUserid)
        {
            try
            {
                string strSQL = @"select * from Es_PageOften where pagetype=1
                            union all 
                            select * from Es_PageOften where UserId =" + lngUserid.ToString() + @" and pagetype<>1";
                return CommonDP.ExcuteSqlTable(strSQL);
            }
            catch
            {
                DataTable dt = new DataTable();
                return dt;
            }
        }

        /// <summary>
        /// 判断是否全局界面
        /// </summary>
        /// <param name="lngUserid"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static bool IsAllPage(long lngUserid, string title)
        {
            try
            {
                string strSQL = @"select * from Es_PageOften where pagetype=1 and pageName=" + StringTool.SqlQ(title); 
                DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 判断是否个人全局变量
        /// </summary>
        /// <param name="lngUserid"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static bool IsOftenPage(long lngUserid, string title)
        {
            try
            {
                string strSQL = @"select * from Es_PageOften where pagetype<>1 and pageName=" + StringTool.SqlQ(title) + " and UserId=" + lngUserid;
                DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
