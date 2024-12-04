using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Globalization;
using System.Security.Cryptography;

namespace Epower.DevBase.BaseTools
{
	/// <summary>
	/// ��װһЩ���ڱ���ķ�����
	/// </summary>
	public class EnCodeTool
	{
		public EnCodeTool()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
        }


        #region QP�������(Quoted-Printable)
        /// <summary>
        ///  QP����
        /// </summary>
        /// <param name="AText"></param>
        /// <returns></returns>
        public static string QuotedPrintableEncode(string AText) 
        {
            string Result = "";
            byte[] vBuffer = Encoding.Default.GetBytes(AText);
            foreach (byte vByte in vBuffer)
                // �ɼ��ַ�����"="(#61)
                if ((vByte >= 33 && vByte <= 60) || (vByte >= 62 && vByte <= 126))
                    Result += (char)vByte;
                else Result += "=" + vByte.ToString("X2");
            return Result;
        }

        /// <summary>
        /// QP ����
        /// </summary>
        /// <param name="ACode"></param>
        /// <returns></returns>
        public static string QuotedPrintableDecode(string ACode) 
        {
            ArrayList vBuffer = new ArrayList();

            for (int i = 0; i < ACode.Length; i++)
            {
                if (ACode[i] == '=')
                {
                    i++;
                    if (ACode[i] != '\r')
                    {
                        byte vByte;
                        if (byte.TryParse(ACode.Substring(i, 2),
                            NumberStyles.HexNumber, null, out vByte))
                            vBuffer.Add(vByte);
                    }
                    i++;
                }
                else if (ACode[i] != '\n') vBuffer.Add((byte)ACode[i]);
            }
            return Encoding.Default.GetString((byte[])vBuffer.ToArray(typeof(byte)));
        }


        #endregion


        /// <summary>
		/// �ַ�������
		/// </summary>
		/// <param name="strMessage"></param>
		/// <returns></returns>
		public static string EnCode(string strMessage)
		{
			byte[] b = Encoding.Default.GetBytes(StringTool.ReverseString(strMessage));

			
			return Convert.ToBase64String(b);
		}

		/// <summary>
		/// ���ļ�������ַ���
		/// ���ڽ������洢�����ݿ��еĳ���
		/// ע�⣺�ļ�����Ҫ��·����
		/// </summary>
		/// <param name="FileName"></param>
		/// <returns></returns>
		public static string EnCodeFile(string FileName)
		{
			FileStream inFile;
			byte[] b;
			try
			{
				inFile = new FileStream(FileName,FileMode.Open,FileAccess.Read);
				b=new byte[inFile.Length];
				long bytesRead = inFile.Read(b, 0,(int)inFile.Length);
				inFile.Close();
			}
			catch
			{
				
				throw;
			}
			return Convert.ToBase64String(b);
		}



		/// <summary>
		/// ���ļ��������һ���ļ�
		/// ���ڽ������洢���ļ��������еĳ������������ļ��������ϵ�ԭ������
		/// ע�⣺�ļ�����Ҫ��·����
		/// </summary>
		/// <param name="inFileName"></param>
		/// <param name="outFileName"></param>
		public static void EnCodeFileToFile(string inFileName,string outFileName)
		{
			FileStream inFile;
			FileStream outFile;
			byte[] b;
			byte[] bout;
			try
			{
				inFile = new FileStream(inFileName,FileMode.Open,FileAccess.Read);
				b=new byte[inFile.Length];
				long bytesRead = inFile.Read(b, 0,(int)inFile.Length);
				inFile.Close();
				bout = Encoding.Default.GetBytes(Convert.ToBase64String(b));
				outFile= new FileStream(outFileName,FileMode.Create,FileAccess.Write);
				outFile.Write(bout,0,bout.Length);
				outFile.Close();
			}
			catch
			{
				
				throw;
			}
		}


		/// <summary>
		/// �ַ���������
		/// </summary>
		/// <param name="strEncodeString"></param>
		/// <returns></returns>
		public static string DeCode(string strEncodeString)
		{
			try
			{
				byte[] b = Convert.FromBase64String(strEncodeString);
				return StringTool.ReverseString(Encoding.Default.GetString(b));
			}
			catch(Exception e)
			{
				throw e;
			}
		}

		/// <summary>
		/// ��������ַ���ת���ļ�
		/// ���ڽ������洢�����ݿ��У������������ļ��ĳ���
		/// ע�⣺�ļ�����Ҫ��·����
		/// </summary>
		/// <param name="FileName"></param>
		/// <param name="EncodedString"></param>
		public static void DeCodeFile(string FileName,string EncodedString)
		{
			byte[] b = Convert.FromBase64String(EncodedString);
			//			string DecodedString = Encoding.Default.GetString(b);
			FileStream outFile; 
			try
			{
				outFile= new FileStream(FileName,FileMode.Create,FileAccess.Write);
				outFile.Write(b,0,b.Length);
				outFile.Close();
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// ��һ���ѱ�����ļ���ԭ
		/// ע�⣺�ļ�����Ҫ��·����
		/// </summary>
		/// <param name="inFileName"></param>
		/// <param name="outFileName"></param>
		public static void DeCodeFileToFile(string inFileName,string outFileName)
		{
			FileStream inFile;
			FileStream outFile;
			byte[] b;
			byte[] bout;
			try
			{
				inFile = new FileStream(inFileName,FileMode.Open,FileAccess.Read);
				b=new byte[inFile.Length];
				long bytesRead = inFile.Read(b, 0,(int)inFile.Length);
				inFile.Close();
				bout = Convert.FromBase64String(Encoding.Default.GetString(b));
				outFile= new FileStream(outFileName,FileMode.Create,FileAccess.Write);
				outFile.Write(bout,0,bout.Length);
				outFile.Close();
			}
			catch
			{
				
				throw;
			}
		}

        /// <summary>
        /// MD5���ܣ������MD5�������εı���һ�µġ�
        /// </summary>
        /// <param name="toCryString"></param>
        /// <returns></returns>
        public static string MD5(string toCryString)
        {
            byte[] isoCode= Encoding.GetEncoding("iso-8859-1").GetBytes(toCryString);
            MD5 hashmd5 = new MD5CryptoServiceProvider();
            byte[] hashByte = hashmd5.ComputeHash(isoCode);
            return  BitConverter.ToString(hashByte).Replace("-","").ToLower();            
        } 

        public static string MD5ByUTF8(string toCryString)
        {
            byte[] isoCode= Encoding.UTF8.GetBytes(toCryString);
            MD5 hashmd5 = new MD5CryptoServiceProvider();
            byte[] hashByte = hashmd5.ComputeHash(isoCode);
            return  BitConverter.ToString(hashByte).Replace("-","").ToLower();            
        } 
        /*
        string randomchars = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"; 
        string password = MakePassword(randomchars, 10);
        */
        public static string MakePassword(string pwdchars, int pwdlen)
        { 
            string tmpstr = ""; 
            int iRandNum; 
            Random rnd = new Random();            
            for(int i=0;i<pwdlen;i++)
            { 
                iRandNum = rnd.Next(pwdchars.Length); 
                tmpstr += pwdchars[iRandNum]; 
            } 
            return tmpstr; 
        }

	}
}
