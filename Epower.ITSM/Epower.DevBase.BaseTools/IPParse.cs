/*
 *              IP��ַ������
 * 
 * �ṩ��������:����Ϊ��̬������
 * 
 *	1����֤IP��ַ����Ч��
 *  2���ж�ĳIP��ַ�Ƿ��ڸ�����IP��Χ��
 * 
 * ����ʱ�䣺2005-04-05
 * �����ˣ�duanqs 
 * 
 * */
using System;
namespace Epower.DevBase.BaseTools
{

	/// <summary>
	/// IP������һЩ��ط�����
	/// </summary>
	public class IPParse
	{
		public IPParse()
		{
			
		}

		/// <summary>
		/// ��֤IP��ַ����Ч��
		/// </summary>
		/// <param name="strIP">Ҫ��֤��IP��ַ(�ַ�)</param>
		/// <returns>������֤���(Bool����)</returns>
		public static bool IPValidate(string strIP)
		{
			
			//�մ�
			if(strIP.Length==0)
				return false;
			
			//��.
			if(strIP.IndexOf(".")==0)
				return false;

			//���ܺ��ַ�.����
			string sTmp=strIP.Replace(".","");
			for(int n=0;n<sTmp.Length;n++)
			{
				if(!char.IsNumber(sTmp,n))
					return false;
			}

			string [] arrIPElements= strIP.Split('.');

			//�ָ�Ϊ4���Ӵ�
			if(arrIPElements.Length!=4)
				return false;
			
			//��֤0-255
			foreach(string strIPElement in arrIPElements)
			{
				if(strIPElement=="")
					return false;

				if(int.Parse(strIPElement)<0 || int.Parse(strIPElement)>255)
					return false;

				if(strIPElement.Length!=1 && strIPElement.Substring(0,1)=="0")
					return false;
			}
			return true;
		}



		/// <summary>
		/// ��֤IP��ַ�Ƿ���ָ���ķ�Χ��
		/// </summary>
		/// <param name="strIP">��Ҫ��֤��IP��ַ(�ַ�)</param>
		/// <param name="strBeginIP">ָ��IP��Χ�Ŀ�ʼIP��ַ</param>
		/// <param name="strEndIP">ָ��IP��Χ�Ľ���IP��ַ</param>
		/// <returns>������֤���(BOOL����)</returns>
		public static bool IsIncluded(string strIP,string strBeginIP,string strEndIP)
		{
			if(!IPValidate(strIP) || !IPValidate(strBeginIP) || !IPValidate(strEndIP))
			{
				throw new Exception("IP��ַ��ʽ����");
			}
			
			strIP=FormatIPElement(strIP);
			strBeginIP=FormatIPElement(strBeginIP);
			strEndIP=FormatIPElement(strEndIP);

			if(string.Compare(strIP,strBeginIP)>=0  && string.Compare(strIP,strEndIP)<=0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// ��ʽ��IP��ַ��3.4.45.56 ��> 003.004.045.056���ڱȽ�
		/// </summary>
		/// <param name="strIP">Ҫ��ʽ����IP��ַ</param>
		/// <returns>��ʽ�����IP��ַ</returns>
		private static string FormatIPElement(string strIP)
		{
			string [] arrIPElements= strIP.Split('.');
			string strTmp="";
			for(int n=0; n<arrIPElements.Length ;n++)
			{	
				strTmp=strTmp+arrIPElements[n].PadLeft(3,'0')+".";
			}
			strTmp=strTmp.TrimEnd('.');
			return strTmp;
		}

	}
}
