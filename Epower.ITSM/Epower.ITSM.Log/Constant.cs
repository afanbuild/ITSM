using System;
using System.Text;
using System.Web;

namespace Epower.ITSM.Log
{
	/// <summary>
	/// �쳣����(��/��/��)
	/// </summary>
	public enum Level
	{
		//[Description("�߼�")]
		High_level = 1,
		//[Description("�м�")]
		Intermediate = 2,
		//[Description("�ͼ�")]
		Generally = 3
	}
	/// <summary>
	/// ����ϵͳ������
	/// </summary>
    public class Constant
    {
        public static long UserID
        {
            get
            {
				try
				{
					return long.Parse(HttpContext.Current.Session["UserID"] == null ? HttpContext.Current.User.Identity.Name.Split('@')[0] : HttpContext.Current.Session["UserID"].ToString());
				}
				catch{ return long.Parse("0");}
            }
        }

        public static string UserName
        {
            get
            {
                try
                {
                    return HttpContext.Current.Session["PersonName"] == null ? HttpContext.Current.User.Identity.Name.Split('@')[2] : HttpContext.Current.Session["PersonName"].ToString();
                }
                catch { return ""; }
            }
        }

        public static long DeptID
        {
            get
            {
                try
                {
                    return long.Parse(HttpContext.Current.Session["UserDeptID"] == null ? HttpContext.Current.User.Identity.Name.Split('@')[3] : HttpContext.Current.Session["UserDeptID"].ToString());
                }
                catch { return long.Parse("0"); }
            }
        }

        public static string DeptName
        {
            get
            {
                try
                {
                    return HttpContext.Current.Session["UserDeptName"] == null ? HttpContext.Current.User.Identity.Name.Split('@')[4] : HttpContext.Current.Session["UserDeptName"].ToString();
                }
                catch { return ""; }
            }
        }

		/// <summary>
		/// ��־������������ļ�����
		/// </summary>
		public static readonly string LogConfigNodeName = "LogConfig";

	
		

		
	}
}
