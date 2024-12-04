using System;
using System.DirectoryServices;
using System.Security.Principal;
using System.Runtime.InteropServices;
namespace Epower.DevBase.BaseTools
{
	/// <summary>
	/// �Ŀ¼�����ࡣ��װһϵ�лĿ¼������صķ�����
	/// </summary>
	public sealed class ADTool

	{



		/// <summary>
		/// ����
		/// </summary>
		private static string DomainName = System.Configuration.ConfigurationSettings.AppSettings["DomainName"];
		/// <summary>
		///  LDAP ��ַ
		/// </summary>
		private static string LDAPDomain = System.Configuration.ConfigurationSettings.AppSettings["LDAPDomain"];

		/// <summary>
		/// LDAP��·��
		/// </summary>
		private static string ADPath = System.Configuration.ConfigurationSettings.AppSettings["ADPath"];

		/// <summary>
		/// ��¼�ʺ�
		/// </summary>
 
		public static string ADUser = EncryTool.DeCrypt(System.Configuration.ConfigurationSettings.AppSettings["ADUser"]);
		/// <summary>
		/// ��¼����
		/// </summary>
		public static string ADPassword =EncryTool.DeCrypt(System.Configuration.ConfigurationSettings.AppSettings["ADPassword"]);

		/// <summary>
		/// ������ʵ��
		/// </summary>
		

		private static IdentityImpersonation impersonate = new IdentityImpersonation(ADUser, ADPassword, DomainName);


		/// <summary>
		/// �û���¼��֤���
		/// </summary>

		public enum LoginResult

		{

			/// <summary>
			/// ������¼
			/// </summary>

			LOGIN_USER_OK = 0,

			/// <summary>
			/// �û�������
			/// </summary>
 

			LOGIN_USER_DOESNT_EXIST,

			/// <summary>
			/// �û��ʺű�����
			/// </summary>

			LOGIN_USER_ACCOUNT_INACTIVE,

			/// <summary>
			/// �û����벻��ȷ
			/// </summary>

			LOGIN_USER_PASSWORD_INCORRECT

		}

 

		/// <summary>
		/// �û����Զ����־
		/// </summary>

		public enum ADS_USER_FLAG_ENUM

		{

			/// <summary>
			/// ��¼�ű���־�����ͨ�� ADSI LDAP ���ж���д����ʱ���ñ�־ʧЧ�����ͨ�� ADSI WINNT���ñ�־Ϊֻ����
			/// </summary>

			ADS_UF_SCRIPT = 0X0001,

			/// <summary>
			/// �û��ʺŽ��ñ�־
			/// </summary>

			ADS_UF_ACCOUNTDISABLE = 0X0002,

			/// <summary>
			/// ���ļ��б�־
			/// </summary>

			ADS_UF_HOMEDIR_REQUIRED = 0X0008,

			/// <summary>
			/// ���ڱ�־
			/// </summary>

			ADS_UF_LOCKOUT = 0X0010,

			/// <summary>
			/// �û����벻�Ǳ����
			/// </summary>

			ADS_UF_PASSWD_NOTREQD = 0X0020,

			/// <summary>
			/// ���벻�ܸ��ı�־
			/// </summary>

			ADS_UF_PASSWD_CANT_CHANGE = 0X0040,

			/// <summary>
			/// ʹ�ÿ���ļ��ܱ�������
			/// </summary>

			ADS_UF_ENCRYPTED_TEXT_PASSWORD_ALLOWED = 0X0080,

			/// <summary>
			/// �����ʺű�־
			/// </summary>

			ADS_UF_TEMP_DUPLICATE_ACCOUNT = 0X0100,

			/// <summary>
			/// ��ͨ�û���Ĭ���ʺ�����
			/// </summary>

			ADS_UF_NORMAL_ACCOUNT = 0X0200,

			/// <summary>
			/// ����������ʺű�־
			/// </summary>
 

			ADS_UF_INTERDOMAIN_TRUST_ACCOUNT = 0X0800,

			/// <summary>
			/// ����վ�����ʺű�־
			/// </summary>
 
			ADS_UF_WORKSTATION_TRUST_ACCOUNT = 0x1000,

			/// <summary>
			/// �����������ʺű�־
			/// </summary>

			ADS_UF_SERVER_TRUST_ACCOUNT = 0X2000,

			/// <summary>
			/// �����������ڱ�־
			/// </summary>

			ADS_UF_DONT_EXPIRE_PASSWD = 0X10000,

			/// <summary>
			/// MNS �ʺű�־
			/// </summary>

			ADS_UF_MNS_LOGON_ACCOUNT = 0X20000,

			/// <summary>
			/// ����ʽ��¼����ʹ�����ܿ�
			/// </summary>

			ADS_UF_SMARTCARD_REQUIRED = 0X40000,

			/// <summary>
			/// �����øñ�־ʱ�������ʺţ��û��������ʺţ���ͨ�� Kerberos ί������
			/// </summary>

			ADS_UF_TRUSTED_FOR_DELEGATION = 0X80000,

			/// <summary>
			/// �����øñ�־ʱ����ʹ�����ʺ���ͨ�� Kerberos ί�����εģ������ʺŲ��ܱ�ί��
			/// </summary>

			ADS_UF_NOT_DELEGATED = 0X100000,

			/// <summary>
			/// ���ʺ���Ҫ DES ��������
			/// </summary>

			ADS_UF_USE_DES_KEY_ONLY = 0X200000,

			/// <summary>
			/// ��Ҫ���� Kerberos Ԥ�����֤
			/// </summary>

			ADS_UF_DONT_REQUIRE_PREAUTH = 0X4000000,

			/// <summary>
			/// �û�������ڱ�־
			/// </summary>

			ADS_UF_PASSWORD_EXPIRED = 0X800000,

			/// <summary>
			/// �û��ʺſ�ί�б�־
			/// </summary>

			ADS_UF_TRUSTED_TO_AUTHENTICATE_FOR_DELEGATION = 0X1000000

		}

 

		public ADTool()

		{

			//

		}

 

		#region GetDirectoryObject

 

		/// <summary>
		/// ���DirectoryEntry����ʵ��,�Թ���Ա��½AD
		/// </summary>
		/// <returns></returns>

		private static DirectoryEntry GetDirectoryObject()

		{

			DirectoryEntry entry = new DirectoryEntry(ADPath, ADUser, ADPassword, AuthenticationTypes.Secure);

			return entry;

		}

 

		/// <summary>
		/// ����ָ���û�������������ӦDirectoryEntryʵ��
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		/// <returns></returns>

		private static DirectoryEntry GetDirectoryObject(string userName, string password)

		{

			DirectoryEntry entry = new DirectoryEntry(ADPath, userName, password, AuthenticationTypes.None);

			return entry;

		}

 

		/// <summary>
		/// i.e. /CN=Users,DC=creditsights, DC=cyberelves, DC=Com
		/// </summary>
		/// <param name="domainReference"></param>
		/// <returns></returns>

		private static DirectoryEntry GetDirectoryObject(string domainReference)

		{

			DirectoryEntry entry = new DirectoryEntry(ADPath + domainReference, ADUser, ADPassword, AuthenticationTypes.Secure);

			return entry;

		}

 

		/// <summary>
		/// �����UserName,Password������DirectoryEntry
		/// </summary>
		/// <param name="domainReference"></param>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		/// <returns></returns>

		private static DirectoryEntry GetDirectoryObject(string domainReference, string userName, string password)

		{

			DirectoryEntry entry = new DirectoryEntry(ADPath + domainReference, userName, password, AuthenticationTypes.Secure);

			return entry;

		}

 

		#endregion

 

		#region GetDirectoryEntry

 

		/// <summary>
		/// �����û���������ȡ���û��� ����
		/// �û���������
		/// ����ҵ����û����򷵻��û��� ���󣻷��򷵻� null
		/// </summary>
		/// <param name="commonName"></param>
		/// <returns></returns>

		public static DirectoryEntry GetDirectoryEntry(string commonName)
		{
			DirectoryEntry de = GetDirectoryObject();

			DirectorySearcher deSearch = new DirectorySearcher(de);

			deSearch.Filter = "(&(&(objectCategory=person)(objectClass=user))(cn=" + commonName + "))";

			deSearch.SearchScope = SearchScope.Subtree;

			//try
			//{
			SearchResult result = deSearch.FindOne();
			de = new DirectoryEntry(result.Path);
			return de;
			//}
			//catch(Exception ex)
			//{
			//	throw ex;
			//return null;
			//}
		}

 
		/// <summary>
		/// �����û���������ȡ���û��� ����
		/// �û���������
		/// ����ҵ����û����򷵻��û��� ���󣻷��򷵻� null
		/// </summary>
		/// <param name="commonName"></param>
		/// <returns></returns>

		public static DirectoryEntry GetDirectoryEntry(string commonName,string LoginName, string Password)
		{
			DirectoryEntry de = GetDirectoryObject();

			DirectorySearcher deSearch = new DirectorySearcher(de);

			deSearch.Filter = "(&(&(objectCategory=person)(objectClass=user))(cn=" + commonName + "))";

			deSearch.SearchScope = SearchScope.Subtree;

			//try
			//{
			SearchResult result = deSearch.FindOne();
			de = new DirectoryEntry(result.Path,LoginName,Password);
			return de;
			//}
			//catch(Exception ex)
			//{
			//	throw ex;
			//}
		}


		/// <summary>
		/// �����û��������ƺ�����ȡ���û��� ����
		/// �û���������
		/// �û�����
		/// ����ҵ����û����򷵻��û��� ���󣻷��򷵻� null
		/// </summary>
		/// <param name="commonName"></param>
		/// <param name="password"></param>
		/// <returns></returns>

		public static DirectoryEntry GetDirectoryEntry(string commonName, string password)

		{

			DirectoryEntry de = GetDirectoryObject(commonName, password);

			DirectorySearcher deSearch = new DirectorySearcher(de);

			deSearch.Filter = "(&(&(objectCategory=person)(objectClass=user))(cn=" + commonName + "))";

			deSearch.SearchScope = SearchScope.Subtree;

 

			//try
			//{

			SearchResult result = deSearch.FindOne();

			de = new DirectoryEntry(result.Path);

			return de;

			//}

			//catch(Exception ex)
			//{
			//	throw ex;				
			//return null;

			//}

		}

 

		/// <summary>
		/// �����û��ʺų�ȡ���û��� ����
		/// �û��ʺ���
		/// ����ҵ����û����򷵻��û��� ���󣻷��򷵻� null
		/// </summary>
		/// <param name="sAMAccountName"></param>
		/// <returns></returns>

		public static DirectoryEntry GetDirectoryEntryByAccount(string sAMAccountName)

		{

			DirectoryEntry de = GetDirectoryObject();

			DirectorySearcher deSearch = new DirectorySearcher(de);

			deSearch.Filter = "(&(&(objectCategory=person)(objectClass=user))(sAMAccountName=" + sAMAccountName + "))";

			deSearch.SearchScope = SearchScope.Subtree;

 

			try
			{

				SearchResult result = deSearch.FindOne();

				de = new DirectoryEntry(result.Path);

				return de;
			}

			catch(Exception ex)
			{
				throw ex;
				//return null;
			}

		}

 

		/// <summary>
		/// �����û��ʺź�����ȡ���û��� ����
		/// �û��ʺ���
		/// �û�����
		/// ����ҵ����û����򷵻��û��� ���󣻷��򷵻� null
		/// </summary>
		/// <param name="sAMAccountName"></param>
		/// <param name="password"></param>
		/// <returns></returns>

		public static DirectoryEntry GetDirectoryEntryByAccount(string sAMAccountName, string Password)

		{
			/*

			DirectoryEntry de = GetDirectoryEntryByAccount(sAMAccountName);

			if (de != null)

			{

				string commonName = de.Properties["cn"][0].ToString();

 

				if (GetDirectoryEntry(commonName, password) != null)

					return GetDirectoryEntry(commonName, password);

				else

					return null;

			}

			else

			{

				return null;

			}
			
			*/

			DirectoryEntry de = GetDirectoryObject();

			DirectorySearcher deSearch = new DirectorySearcher(de);

			deSearch.Filter = "(&(&(objectCategory=person)(objectClass=user))(sAMAccountName=" + sAMAccountName + "))";

			deSearch.SearchScope = SearchScope.Subtree;

 

			try
			{

				SearchResult result = deSearch.FindOne();

				de = new DirectoryEntry(result.Path,sAMAccountName,Password);

				return de;
			}

			catch(Exception ex)
			{
				throw ex;
				//return null;
			}

		}

 

		/// <summary>
		/// ��������ȡ���û���� ����
		/// 
		/// ����
		/// </summary>
		/// <param name="groupName"></param>
		/// <returns></returns>

		public static DirectoryEntry GetDirectoryEntryOfGroup(string groupName)

		{

			DirectoryEntry de = GetDirectoryObject();

			DirectorySearcher deSearch = new DirectorySearcher(de);

			deSearch.Filter = "(&(objectClass=group)(cn=" + groupName + "))";

			deSearch.SearchScope = SearchScope.Subtree;

 

			try

			{

				SearchResult result = deSearch.FindOne();

				de = new DirectoryEntry(result.Path);

				return de;

			}

			catch

			{

				return null;

			}

		}

 

		#endregion

 

		#region GetProperty

 

		/// <summary>
		/// ���ָ�� ָ����������Ӧ��ֵ
		/// ��������
		/// ����ֵ
		/// </summary>
		/// <param name="de"></param>
		/// <param name="propertyName"></param>
		/// <returns></returns>

		public static string GetProperty(DirectoryEntry de, string propertyName)

		{

			if(de.Properties.Contains(propertyName))

			{

				return de.Properties[propertyName][0].ToString() ;

			}

			else

			{

				return string.Empty;

			}

		}

 

		/// <summary>
		///  ���ָ��������� ��ָ����������Ӧ��ֵ
		/// 
		/// 
		/// ��������
		/// ����ֵ
		/// </summary>
		/// <param name="searchResult"></param>
		/// <param name="propertyName"></param>
		/// <returns></returns>

		public static string GetProperty(SearchResult searchResult, string propertyName)

		{

			if(searchResult.Properties.Contains(propertyName))

			{

				return searchResult.Properties[propertyName][0].ToString() ;

			}

			else

			{

				return string.Empty;

			}

		}

 

		#endregion

 

		/// <summary>
		/// ����ָ�� ������ֵ
		/// 
		/// 
		/// ��������
		/// ����ֵ
		/// </summary>
		/// <param name="de"></param>
		/// <param name="propertyName"></param>
		/// <param name="propertyValue"></param>

		public static void SetProperty(DirectoryEntry de, string propertyName, string propertyValue)

		{

			if(propertyValue != string.Empty || propertyValue != "" || propertyValue != null)

			{

				if(de.Properties.Contains(propertyName))

				{

					de.Properties[propertyName][0] = propertyValue; 

				}

				else

				{

					de.Properties[propertyName].Add(propertyValue);

				}

			}

		}

 

		/// <summary>
		/// �����µ��û�
		/// 
		/// DN λ�á����磺OU=����ƽ̨ �� CN=Users
		/// ��������
		/// �ʺ�
		/// ����
		/// </summary>
		/// <param name="ldapDN"></param>
		/// <param name="commonName"></param>
		/// <param name="sAMAccountName"></param>
		/// <param name="password"></param>
		/// <returns></returns>

		public static DirectoryEntry CreateNewUser(string ldapDN, string commonName, string sAMAccountName, string password)

		{

			DirectoryEntry entry = GetDirectoryObject();

			DirectoryEntry subEntry = entry.Children.Find(ldapDN);

			DirectoryEntry deUser = subEntry.Children.Add("CN=" + commonName, "user");

			deUser.Properties["sAMAccountName"].Value = sAMAccountName;

			deUser.CommitChanges();

			
			ADTool.SetPassword(commonName, password);

            ADTool.EnableUser(commonName);


			deUser.Close();

			return deUser;

		}

 

		/// <summary>
		/// �����µ��û���Ĭ�ϴ����� Users ��Ԫ�¡�
		/// 
		/// ��������
		/// �ʺ�
		/// ����
		/// </summary>
		/// <param name="commonName"></param>
		/// <param name="sAMAccountName"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public static DirectoryEntry CreateNewUser(string commonName, string sAMAccountName, string password)

		{

			return CreateNewUser("CN=Users", commonName, sAMAccountName, password);

		}

		/// <summary>
		/// �ж�ָ���������Ƶ��û��Ƿ����
		/// 
		/// �û���������
		/// ������ڣ����� true�����򷵻� false
		/// </summary>
		/// <param name="commonName"></param>
		/// <returns></returns>

		public static bool IsUserExists(string commonName)

		{

			DirectoryEntry de = GetDirectoryObject();

			DirectorySearcher deSearch = new DirectorySearcher(de);

			deSearch.Filter = "(&(&(objectCategory=person)(objectClass=user))(cn=" + commonName + "))";       // LDAP ��ѯ��

			SearchResultCollection results = deSearch.FindAll();

 

			if (results.Count == 0)

				return false;

			else

				return true;

		}

 

		/// <summary>
		/// �ж��û��ʺ��Ƿ񼤻�
		/// 
		/// �û��ʺ����Կ�����
		/// ����û��ʺ��Ѿ�������� true�����򷵻� false
		/// </summary>
		/// <param name="userAccountControl"></param>
		/// <returns></returns>

		public static bool IsAccountActive(int userAccountControl)

		{

			int userAccountControl_Disabled = Convert.ToInt32(ADS_USER_FLAG_ENUM.ADS_UF_ACCOUNTDISABLE);

			int flagExists = userAccountControl & userAccountControl_Disabled;

 

			if (flagExists > 0)

				return false;

			else

				return true;

		}

 

		/// <summary>
		/// �ж��û��������Ƿ��㹻�����������֤������¼

		/// 

		/// �û���������

		/// ����

		/// ���ܿ�������¼���򷵻� true�����򷵻� false
		/// </summary>
		/// <param name="commonName"></param>
		/// <param name="password"></param>
		/// <returns></returns>

		public static LoginResult Login(string commonName, string password)

		{

			DirectoryEntry de = GetDirectoryEntry(commonName);

 

			if (de != null)

			{

				// �������ж��û�������ȷǰ�����ʺż������Խ����жϣ����򽫳����쳣��

				int userAccountControl = Convert.ToInt32(de.Properties["userAccountControl"][0]);

				de.Close();

 

				if (!IsAccountActive(userAccountControl))

					return LoginResult.LOGIN_USER_ACCOUNT_INACTIVE;

 

				if (GetDirectoryEntry(commonName, password) != null)

					return LoginResult.LOGIN_USER_OK;

				else

					return LoginResult.LOGIN_USER_PASSWORD_INCORRECT;

			}

			else

			{

				return LoginResult.LOGIN_USER_DOESNT_EXIST; 

			}

		}

 

		/// <summary>
		/// �ж��û��ʺ��������Ƿ��㹻�����������֤������¼
		/// 
		/// �û��ʺ�
		/// ����
		/// ���ܿ�������¼���򷵻� true�����򷵻� false
		/// </summary>
		/// <param name="sAMAccountName"></param>
		/// <param name="password"></param>
		/// <returns></returns>

		public static LoginResult LoginByAccount(string sAMAccountName, string password)

		{

			DirectoryEntry de = GetDirectoryEntryByAccount(sAMAccountName);   
			if (de != null)
			{
				// �������ж��û�������ȷǰ�����ʺż������Խ����жϣ����򽫳����쳣��

				int userAccountControl = Convert.ToInt32(de.Properties["userAccountControl"][0]);

				de.Close();

 

				if (!IsAccountActive(userAccountControl))

					return LoginResult.LOGIN_USER_ACCOUNT_INACTIVE;

 

				if (GetDirectoryEntryByAccount(sAMAccountName, password) != null)

					return LoginResult.LOGIN_USER_OK;

				else

					return LoginResult.LOGIN_USER_PASSWORD_INCORRECT;

			}

			else

			{

				return LoginResult.LOGIN_USER_DOESNT_EXIST; 

			}

		}

 

		/// <summary>
		/// �����û����룬����Ա����ͨ�������޸�ָ���û������롣
		/// 
		/// �û���������
		/// �û�������
		/// </summary>
		/// <param name="commonName"></param>
		/// <param name="newPassword"></param>

		public static void SetPassword(string commonName, string newPassword)

		{

			DirectoryEntry de = GetDirectoryEntry(commonName);

              

			// ģ�ⳬ������Ա���Դﵽ��Ȩ���޸��û�����

			impersonate.BeginImpersonate();

			de.Invoke("SetPassword", new object[]{newPassword});

			impersonate.StopImpersonate();

 

			de.Close();

		}


		/// <summary>
		/// �����û�����
		/// </summary>
		/// <param name="LDAP">LADP</param>
		/// <param name="newPassword">������</param>
		public static void SetPassword_By_LDAP(string LDAP, string newPassword)

		{

			DirectoryEntry de = new DirectoryEntry(LDAP);;

              

			// ģ�ⳬ������Ա���Դﵽ��Ȩ���޸��û�����

			impersonate.BeginImpersonate();

			de.Invoke("SetPassword", new object[]{newPassword});

			impersonate.StopImpersonate();

 

			de.Close();

		}

 

		/// <summary>
		/// �����ʺ����룬����Ա����ͨ�������޸�ָ���ʺŵ����롣
		/// 
		/// �û��ʺ�
		/// �û�������
		/// </summary>
		/// <param name="sAMAccountName"></param>
		/// <param name="newPassword"></param>

		public static void SetPasswordByAccount(string sAMAccountName, string newPassword)

		{

			try
			{
				DirectoryEntry de = GetDirectoryEntryByAccount(sAMAccountName);

 

				// ģ�ⳬ������Ա���Դﵽ��Ȩ���޸��û�����

				IdentityImpersonation impersonate = new IdentityImpersonation(ADUser, ADPassword, DomainName);

				impersonate.BeginImpersonate();

				de.Invoke("SetPassword", new object[]{newPassword});

				impersonate.StopImpersonate(); 

				de.Close();
			}
			catch(Exception ex)
			{
				throw ex;
			}

		}

 

		/// <summary>
		/// �޸��û�����
		/// </summary>
		/// <param name="commonName">�û���������</param>
		/// <param name="LoginName">��½��</param>
		/// <param name="oldPassword">������</param>
		/// <param name="newPassword">������</param>
		public static void ChangeUserPassword (string commonName,string LoginName, string oldPassword, string newPassword)
		{

			//try
			//{
			DirectoryEntry oUser = GetDirectoryEntry(commonName,LoginName,oldPassword);
			oUser.Invoke("ChangePassword",oldPassword, newPassword);
			//}
			//catch(Exception ex)
			//{
			//	throw ex;
			//}

		}


		/// <summary>
		/// �޸��û�����
		/// </summary>
		/// <param name="LoginName">��½��</param>
		/// <param name="oldPassword">������</param>
		/// <param name="newPassword">������</param>
		public static void ChangeUserPasswordByAccount (string sAMAccountName, string oldPassword, string newPassword)
		{

			//try
			//{
			DirectoryEntry oUser = GetDirectoryEntryByAccount(sAMAccountName,oldPassword);
			oUser.Invoke("ChangePassword",oldPassword, newPassword);
			//}
			//catch(Exception ex)
			//{
			//	throw ex;
			//}

		}



		/// <summary>
		/// ����LDAP�޸��û�����
		/// </summary>
		/// <param name="LDAP"></param>
		/// <param name="LoginName"></param>
		/// <param name="oldPassword"></param>
		/// <param name="newPassword"></param>
		public static void ChangeUserPassword_By_LDAP(string LDAP,string LoginName, string oldPassword, string newPassword)
		{

			//try
			//{
			DirectoryEntry oUser = new DirectoryEntry(LDAP,LoginName,oldPassword);
			oUser.Invoke("ChangePassword",oldPassword, newPassword);
			//}
			//catch(Exception ex)
			//{
			//	throw ex;
			//}

		}


		/// <summary>
		/// ����ָ���������Ƶ��û�
		/// 
		/// �û���������
		/// </summary>
		/// <param name="commonName"></param>

		public static void EnableUser(string commonName)

		{

			EnableUser(GetDirectoryEntry(commonName));

		}

 

		/// <summary>
		/// ����ָ�� ���û�
		/// </summary>
		/// <param name="de"></param>

		public static void EnableUser(DirectoryEntry de)

		{

			impersonate.BeginImpersonate();

			de.Properties["userAccountControl"][0] = ADTool.ADS_USER_FLAG_ENUM.ADS_UF_NORMAL_ACCOUNT | ADTool.ADS_USER_FLAG_ENUM.ADS_UF_DONT_EXPIRE_PASSWD;

			de.CommitChanges();

			impersonate.StopImpersonate();

			de.Close();

		}

 

		/// <summary>
		/// ����ָ���������Ƶ��û�
		/// 
		/// �û���������
		/// </summary>
		/// <param name="commonName"></param>

		public static void DisableUser(string commonName)

		{

			DisableUser(GetDirectoryEntry(commonName));

		}

 

		/// <summary>
		/// ����ָ�� ���û�
		/// </summary>
		/// <param name="de"></param>
 

		public static void DisableUser(DirectoryEntry de)

		{

			impersonate.BeginImpersonate();

			de.Properties["userAccountControl"][0]=ADTool.ADS_USER_FLAG_ENUM.ADS_UF_NORMAL_ACCOUNT | ADTool.ADS_USER_FLAG_ENUM.ADS_UF_DONT_EXPIRE_PASSWD | ADTool.ADS_USER_FLAG_ENUM.ADS_UF_ACCOUNTDISABLE;

			de.CommitChanges();

			impersonate.StopImpersonate();

			de.Close();

		}

 

		/// <summary>
		/// ��ָ�����û���ӵ�ָ�������С�Ĭ��Ϊ Users �µ�����û���
		/// 
		/// �û���������
		/// ����
		/// </summary>
		/// <param name="userCommonName"></param>
		/// <param name="groupName"></param>

		public static void AddUserToGroup(string userCommonName, string groupName)

		{

			DirectoryEntry oGroup = GetDirectoryEntryOfGroup(groupName);

			DirectoryEntry oUser = GetDirectoryEntry(userCommonName);

              

			impersonate.BeginImpersonate();

			oGroup.Properties["member"].Add(oUser.Properties["distinguishedName"].Value);

			oGroup.CommitChanges();

			impersonate.StopImpersonate();

 

			oGroup.Close();

			oUser.Close();

		}

 

		/// <summary>
		/// ���û���ָ�������Ƴ���Ĭ��Ϊ Users �µ�����û���
		/// 
		/// �û���������
		/// ����
		/// </summary>
		/// <param name="userCommonName"></param>
		/// <param name="groupName"></param>
		public static void RemoveUserFromGroup(string userCommonName, string groupName)

		{

			DirectoryEntry oGroup = GetDirectoryEntryOfGroup(groupName);

			DirectoryEntry oUser = GetDirectoryEntry(userCommonName);

              

			impersonate.BeginImpersonate();

			oGroup.Properties["member"].Remove(oUser.Properties["distinguishedName"].Value);

			oGroup.CommitChanges();

			impersonate.StopImpersonate();

 

			oGroup.Close();

			oUser.Close();

		}

 

	}

 

	/// <summary>
	/// �û�ģ���ɫ�ࡣʵ���ڳ�����ڽ����û���ɫģ�⡣
	/// </summary>

	public class IdentityImpersonation

	{

		[DllImport("advapi32.dll", SetLastError=true)]

		public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

 

		[DllImport("advapi32.dll", CharSet=CharSet.Auto, SetLastError=true)]

		public extern static bool DuplicateToken(IntPtr ExistingTokenHandle, int SECURITY_IMPERSONATION_LEVEL, ref IntPtr DuplicateTokenHandle);

 

		[DllImport("kernel32.dll", CharSet=CharSet.Auto)]

		public extern static bool CloseHandle(IntPtr handle);

 

		// Ҫģ����û����û��������롢��(������)

		private String _sImperUsername;

		private String _sImperPassword;

		private String _sImperDomain;

		// ��¼ģ��������

		private WindowsImpersonationContext _imperContext;

		private IntPtr _adminToken;

		private IntPtr _dupeToken;

		// �Ƿ���ֹͣģ��

		private Boolean _bClosed;

 
		/// <summary>
		/// ���캯��
		/// 
		/// ��Ҫģ����û����û���
		/// ��Ҫģ����û�������
		/// ��Ҫģ����û����ڵ���
		/// </summary>
		/// <param name="impersonationUsername"></param>
		/// <param name="impersonationPassword"></param>
		/// <param name="impersonationDomain"></param>
		public IdentityImpersonation(String impersonationUsername, String impersonationPassword, String impersonationDomain) 

		{

			_sImperUsername = impersonationUsername;

			_sImperPassword = impersonationPassword;

			_sImperDomain = impersonationDomain;

 

			_adminToken = IntPtr.Zero;

			_dupeToken = IntPtr.Zero;

			_bClosed = true;

		}

 

		/// <summary>

		/// ��������

		/// </summary>

		~IdentityImpersonation() 

		{

			if(!_bClosed) 

			{

				StopImpersonate();

			}

		}

 

		/// <summary>
		/// ��ʼ��ݽ�ɫģ�⡣
		/// </summary>
		/// <returns></returns>
		public Boolean BeginImpersonate() 

		{

			Boolean bLogined = LogonUser(_sImperUsername, _sImperDomain, _sImperPassword, 2, 0, ref _adminToken);

                        

			if(!bLogined) 

			{

				return false;

			}

 

			Boolean bDuped = DuplicateToken(_adminToken, 2, ref _dupeToken);

 

			if(!bDuped) 

			{

				return false;

			}

 

			WindowsIdentity fakeId = new WindowsIdentity(_dupeToken);

			_imperContext = fakeId.Impersonate();

 

			_bClosed = false;

 

			return true;

		}

 

		/// <summary>
		///  ֹͣ��ֽ�ɫģ�⡣
		/// </summary>

		public void StopImpersonate() 

		{

			_imperContext.Undo();

			CloseHandle(_dupeToken);

			CloseHandle(_adminToken);

			_bClosed = true;

		}

	}

 

}



