/*******************************************************************
 *
 * Description:ϵͳ���÷����������
 * 
 * 
 * Create By  :
 * Create Date:2008��7��30��
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
	/// CatalogControl ��ժҪ˵����
	/// </summary>
	public class CatalogControl
	{
		public CatalogControl()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}

		/// <summary>
		/// ��ȡ���и�����
		/// </summary>
		/// <returns></returns>
		public static OCatalogCollection GetAllRootCatalogCollection()
		{
			DataTable dt = CatalogDP.GetRootCatalogs();

			return AddCatalogCollection(dt);
		}

		/// <summary>
		/// ��ȡ��ǰ���ڵ�����и�����
		/// </summary>
		/// <returns></returns>
		public static OCatalogCollection GetAllRootCatalogCollection(long orgid)
		{
			DataTable dt = CatalogDP.GetRootCatalogs(orgid);

			return AddCatalogCollection(dt);
		}

		/// <summary>
		/// ��ȡ��ǰ���������и�����
		/// </summary>
		/// <returns></returns>
		public static OCatalogCollection GetAllCatalogCollection(long orgid)
		{
			DataTable dt = CatalogDP.GetCatalogs(orgid);

			return AddCatalogCollection(dt);
		}

		/// <summary>
		/// ��ȡ���з���
		/// </summary>
		/// <returns></returns>
		public static OCatalogCollection GetAllCatalogCollection()
		{
			DataTable dt = CatalogDP.GetCatalogs();

			return AddCatalogCollection(dt);
		}

        /// <summary>
        /// ��ȡĳ��RootID�µ����з���
        /// </summary>
        /// <returns></returns>
        public static OCatalogCollection GetAllCatalogCollectionbyRooID(long RootID)
        {
            DataTable dt = CatalogDP.GetBelowCatas(RootID);

            return AddCatalogCollection(dt);
        }

		/// <summary>
		/// ��ȡ������������ļ���
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
