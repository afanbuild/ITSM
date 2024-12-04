/*******************************************************************
 *
 * Description:系统常用分类参数配置
 * 
 * 
 * Create By  :
 * Create Date:2008年7月30日
 * *****************************************************************/
using System;
using System.Xml;
using System.Data;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.Base;
using Epower.ITSM.Base;

namespace Epower.ITSM.SqlDAL
{
	/// <summary>
	/// CatalogControl 的摘要说明。
	/// </summary>
	public class CatalogControl
	{
		public CatalogControl()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

		/// <summary>
		/// 获取所有根分类
		/// </summary>
		/// <returns></returns>
		public static OCatalogCollection GetAllRootCatalogCollection()
		{
			DataTable dt = CatalogDP.GetRootCatalogs();

			return AddCatalogCollection(dt);
		}

		/// <summary>
		/// 获取当前根节点的所有根分类
		/// </summary>
		/// <returns></returns>
		public static OCatalogCollection GetAllRootCatalogCollection(long orgid)
		{
			DataTable dt = CatalogDP.GetRootCatalogs(orgid);

			return AddCatalogCollection(dt);
		}

		/// <summary>
		/// 获取当前机构的所有根分类
		/// </summary>
		/// <returns></returns>
		public static OCatalogCollection GetAllCatalogCollection(long orgid)
		{
			DataTable dt = CatalogDP.GetCatalogs(orgid);

			return AddCatalogCollection(dt);
		}

		/// <summary>
		/// 获取所有分类
		/// </summary>
		/// <returns></returns>
		public static OCatalogCollection GetAllCatalogCollection()
		{
			DataTable dt = CatalogDP.GetCatalogs();

			return AddCatalogCollection(dt);
		}

        /// <summary>
        /// 获取某个RootID下的所有分类
        /// </summary>
        /// <returns></returns>
        public static OCatalogCollection GetAllCatalogCollectionbyRooID(long RootID)
        {
            DataTable dt = CatalogDP.GetBelowCatas(RootID);

            return AddCatalogCollection(dt);
        }

		/// <summary>
		/// 获取所有下属分类的集合
		/// </summary>
		/// <param name="lngID"></param>
		/// <returns></returns>
		public static OCatalogCollection GetBelowCatalogCollection(long lngID)
		{
			DataTable dt = CatalogDP.GetBelowCatalogs(lngID);

			return AddCatalogCollection(dt);
		}

		private static OCatalogCollection AddCatalogCollection(DataTable dt)
		{
			OCatalogCollection odc = new OCatalogCollection();
			
			if(dt != null)
			{
				for(int i=0;i<dt.Rows.Count;i++)
				{
					odc.Add(long.Parse(dt.Rows[i]["CatalogID"].ToString()),dt.Rows[i]["CatalogName"].ToString(),long.Parse(dt.Rows[i]["parentid"].ToString()));
				}

			}
			return odc;
		}
	}
}
