using System;
using System.Text;
namespace Epower.DevBase.BaseTools
{
	/// <summary>
	/// �ַ����������ط�����
	/// </summary>
	public class StringTool
	{
		public StringTool()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}


		/// <summary>
		/// ���ַ��� \n ת���ɻ��з�
		/// </summary>
		/// <param name="strOriginal"></param>
		/// <param name="intMargin"></param>
		/// <returns></returns>
		public static string ReplaceCrlf(string strOriginal,int intMargin)
		{
			//return strOriginal.Replace(@"\n","\n" + "".PadLeft(intMargin,' '));
			return strOriginal.Replace(@"\n","\n" + new string(' ',intMargin));
		}

		public static string ParseForResponse(string strOriginal)
		{
			return strOriginal.Replace(@"\n","\\\\n").Replace(@"\t","\\\\t");
		}


		public static string ParseForNoraml(string strOriginal)
		{
			return strOriginal.Replace("'",@"""");
		}

		public static string ParseForHtml(string strOriginal)
		{
			return ParseForHtml(strOriginal,false);
		}

		public static string ParseForHtml(string strOriginal,bool isNullReplaceDoubleLine)
		{
			strOriginal=strOriginal.Replace("<","&lt;");//�滻<
			strOriginal=strOriginal.Replace(">","&gt;");//�滻>
			strOriginal=strOriginal.Replace("\r\n","<br/>");//�滻�س�
			strOriginal=strOriginal.Replace(" ","&nbsp;");//�滻�ո�
			if(strOriginal=="" && isNullReplaceDoubleLine)
				strOriginal+="--";
			return strOriginal;
		}


        /// <summary>
        /// ��htmlת����string 
        /// </summary>
        /// <param name="strOriginal"></param>
        /// <returns></returns>
        public static string ParseHtmlForString(string strOriginal)
        {
            return ParseHtmlForString(strOriginal, false);
        }
        /// <summary>
        /// ��htmlת����string 
        /// </summary>
        /// <param name="strOriginal"></param>
        /// <param name="isNullReplaceDoubleLine"></param>
        /// <returns></returns>
        public static string ParseHtmlForString(string strOriginal, bool isNullReplaceDoubleLine)
        {
            strOriginal = strOriginal.Replace("&lt;", "<");//�滻<
            strOriginal = strOriginal.Replace("&gt;", ">");//�滻>
            strOriginal = strOriginal.Replace("<br/>", "\r\n");//�滻�س�
            strOriginal = strOriginal.Replace("&nbsp;", " ");//�滻�ո�
            if (strOriginal == "" && isNullReplaceDoubleLine)
                strOriginal += "--";
            return strOriginal;
        }

		/// <summary>
		/// ���ַ��� \n ת���ɻ��з�
		/// </summary>
		/// <param name="strOriginal"></param>
		/// <returns></returns>
		public static string ReplaceCrlf(string strOriginal)
		{
			//return strOriginal.Replace(@"\n","\n" + "".PadLeft(intMargin,' '));
			return strOriginal.Replace(@"\n","\n");
		}


		/// <summary>
		/// ���ַ�������
		/// </summary>
		/// <param name="OriginString"></param>
		/// <returns></returns>
		public static string ReverseString(string OriginString)
		{
			string strRet="";
			for(int i=OriginString.Trim().Length -1;i>=0;i--)
			{
				strRet=strRet+OriginString.Substring(i,1);
			}
			return strRet;
		}
		
		/// <summary>
		/// �ڸ������ַ����б��й��˵��ظ�����
		/// �磺 "aaaa,1234,78,aaaa,56,78"
		///     ----> "aaaa,1234,78,56"
		///     
		///     ע�⣺�������ַ����в��ܺ��зָ��
		/// </summary>
		/// <param name="strX"></param>
		/// <returns></returns>
		/// 
		public static string FilterDoubleInStrList(string strX)
		{	
			//����","�ָ���Ľ��
			return FilterDoubleInStrList(strX,",");
			
		}
		public static string FilterDoubleInStrList(string strX,string strSplit)
		{
			string strTemp="",strRet="";
			//string strSplit = ",";
			int intPos;
			if(strX=="")
				return "";

			while(strX!="")
			{
				intPos = strX.IndexOf(strSplit);
				if(intPos==-1)
				{
					strRet=strRet+strSplit+strX;
					strX="";
				}
				else
				{
					strTemp = strX.Substring(0,intPos);
					strX=strX.Substring(intPos)+strSplit;
					while(strX.IndexOf(strSplit+strTemp+strSplit)!=-1)
					{
						strX=strX.Replace(strSplit+strTemp+strSplit,strSplit);
					}
					strX=strX.Substring(0,strX.Length-1);
					strRet=strRet+strSplit+strTemp;

				}
				if(strX.IndexOf(strSplit)==0)
				{
					strX=strX.Substring(1);
				}
			}
			if(strRet.IndexOf(strSplit)==0)
			{
				strRet=strRet.Substring(1);
			}


			return strRet;
			
		}

	

		/// <summary>
		/// ����SQL����еĵ����ţ� ��һ�������� ' ��� '' �����ַ�����ǰ����ϵ�����
		/// ������װSQL���
		/// ���磺 SqlQ("aaaa'bbb") --> 'aaaa''bbb' 
		/// </summary>
		/// <param name="strTmp"></param>
		/// <returns></returns>
		public static string SqlQ(string strTmp)
		{
			string strT = strTmp;
			strT = strT.Replace("'","''");
			strT = "'" + strT + "'";
			return strT;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string SqlDate(DateTime dt)
        {
            return string.Format("to_date('{0}','yyyy-mm-dd hh24:mi:ss')", dt.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string SqlDate(DateTime? dt)
        {
            if (!dt.HasValue)
            {
                return "null";
            }
            return SqlDate(dt.Value);
        }


        /// <summary>
        /// �ж��Ƿ�������ģ�2009-02-06��
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool isIncludeChina(string str)
        {
            bool blnRet = false;
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("[\u4e00-\u9fa5]");
            for (int i = 0; i < str.Length; i++)
            {
                if (regex.IsMatch(str.Substring(i,1)))
                {
                    blnRet = true;
                    break;
                }
            }
            return blnRet;

        }


		/// <summary>
		/// ͳһ�Ľ�����ת��Ϊ�̶����ַ�����ʽ
		/// ͳһ��ʽ�� 2004��05��01��
		/// </summary>
		/// <param name="d"></param>
		/// <returns></returns>
		public static string ToMyDateFormat(DateTime d)
		{
			string str ="";
			str = d.Year.ToString() + "��" + (((int)(100 + d.Month)).ToString()).Substring(1) + "��" + (((int)(100 + d.Day)).ToString()).Substring(1) + "��";
			return str;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="strTmp"></param>
		/// <returns></returns>
		public static string EmptyToNullDate(string strTmp)
		{
			if (strTmp.Trim() == "")
				return " null ";
			return "'" + strTmp.Replace("��","-").Replace("��","-").Replace("��","") + "'";
		}


		public static string JavaScriptQ(string strTmp)
		{
			string strT = strTmp;
			//strT.Replace(@"/",@"//");
			strT = "'" + strT.Replace(@"\",@"\\").Replace("'",@"\'") + "'";
			return strT;
		}

		public static string HidenFieldQ(string strTmp)
		{
			string strT = strTmp;
			strT = strT.Replace(@"\",@"\\").Replace("'",@"\'");
			return strT;
		}


		/// <summary>
		///		��ȡ���Ĵ�д��������            ���أ�������������
		//		mvarOrDollar ���ֽ���С, mstrLanguage �ַ������� P���������� C����������  
		/// </summary>
		/// <param name="mvarOrDollar"></param>
		/// <param name="mstrLanguage"></param>
		/// <returns></returns>
		public static string GetDollorStr(double mvarOrDollar)
		{
			//���ؼ������ĵ���������
			return GetDollorStr(mvarOrDollar,"P");
		}
		public static string GetDollorStr(double mvarOrDollar,string mstrLanguage)
		{
			string t_word;
			string WLAMT;
			//			double tt;
			t_word = "";
			//			If mstrLanguage = "E" Or mstrLanguage = "e" Then
			//				t_word = t_word + noinword(Int(mvarOrDollar))
			//				If mvarOrDollar <> Int(mvarOrDollar) Then
			//					tt = Int((mvarOrDollar - Int(mvarOrDollar)) * 100)
			//					t_word = t_word & "And " & " Cents " & noinword(tt)
			//				End If
			//		         
			//			Else
			//			WLAMT = mvarOrDollar.ToString();
			WLAMT = StrFormat(mvarOrDollar, 12, 2);
			
			for(int i = 0;i<12;i++)
			{					   
				if (i != 9)
					t_word = t_word + SHRCHG(WLAMT, WLAMT.Substring(i, 1), i, mstrLanguage);
			     
			}
			string spacestr = "";
			t_word = t_word + spacestr.PadLeft(40 - t_word.Length,' ');
		        
			//			End If
			return t_word.Trim();
		}

		private static string SHRCHG(string WLAMT, string WLCD,int WLLOC,string mstrLanguage)
		{
			string WLNAME;
			string WLDD;
			if (mstrLanguage == "C") 
				WLDD = "���a�լB�U�a�լB��  ����";
			else
				//WLDD = "��ǧ��ʮ��ǧ��ʮԪ �Ƿ�";
                WLDD = "��Ǫ��ʰ��Ǫ��ʰԪ �Ƿ�";
 
			WLNAME = "    ";
			switch (WLCD)
			{
				case " ":
					WLNAME = "   ";
					break;
				case "1":
					//WLNAME = IIf(mstrLanguage = "C", "��", "Ҽ") + Mid(WLDD, (WLLOC - 1) * 2 + 1, 2)
					if(mstrLanguage.Equals("C"))
						WLNAME = "��"  + WLDD.Substring(WLLOC, 1);
					else
						WLNAME = "Ҽ" + WLDD.Substring(WLLOC, 1);
					break;
				case "2":
					//'WLNAME = IIf(mstrLanguage = "C", "�L", "��") + Mid(WLDD, (WLLOC - 1) * 2 + 1, 2)
					if(mstrLanguage.Equals("C"))
						WLNAME = "�L"  + WLDD.Substring(WLLOC, 1);
					else
						WLNAME = "��" + WLDD.Substring(WLLOC, 1);
										
					break;
				case "3":
					//'WLNAME = IIf(mstrLanguage = "C", "��", "��") + Mid(WLDD, (WLLOC - 1) * 2 + 1, 2)
					if(mstrLanguage.Equals("C"))
						WLNAME = "��"  + WLDD.Substring(WLLOC, 1);
					else
						WLNAME = "��" + WLDD.Substring(WLLOC, 1);
					
					break;
				case "4":
					//'WLNAME = IIf(mstrLanguage = "C", "�v", "��") + Mid(WLDD, (WLLOC - 1) * 2 + 1, 2)
					if(mstrLanguage.Equals("C"))
						WLNAME = "�v"  + WLDD.Substring(WLLOC, 1);
					else
						WLNAME = "��" + WLDD.Substring(WLLOC, 1);
					
					break;
				case "5":
					//'WLNAME = IIf(mstrLanguage = "C", "��", "��") + Mid(WLDD, (WLLOC - 1) * 2 + 1, 2)
					if(mstrLanguage.Equals("C"))
						WLNAME = "��"  + WLDD.Substring(WLLOC, 1);
					else
						WLNAME = "��" + WLDD.Substring(WLLOC, 1);
					
					break;
				case "6":
					//'WLNAME = IIf(mstrLanguage = "C", "��", "½") + Mid(WLDD, (WLLOC - 1) * 2 + 1, 2)
					if(mstrLanguage.Equals("C"))
						WLNAME = "��"  + WLDD.Substring(WLLOC, 1);
					else
						WLNAME = "½" + WLDD.Substring(WLLOC, 1);
					
					break;
				case "7":
					//'WLNAME = IIf(mstrLanguage = "C", "�m", "��") + Mid(WLDD, (WLLOC - 1) * 2 + 1, 2)
					if(mstrLanguage.Equals("C"))
						WLNAME = "�m"  + WLDD.Substring(WLLOC, 1);
					else
						WLNAME = "��" + WLDD.Substring(WLLOC, 1);
					
					break;
				case "8":
					//'WLNAME = IIf(mstrLanguage = "C", "��", "��") + Mid(WLDD, (WLLOC - 1) * 2 + 1, 2)
					if(mstrLanguage.Equals("C"))
						WLNAME = "��"  + WLDD.Substring(WLLOC, 1);
					else
						WLNAME = "��" + WLDD.Substring(WLLOC, 1);
					break;
				case "9":
					//'WLNAME = IIf(mstrLanguage = "C", "�h", "��") + Mid(WLDD, (WLLOC - 1) * 2 + 1, 2)
					if(mstrLanguage.Equals("C"))
						WLNAME = "�h"  + WLDD.Substring(WLLOC, 1);
					else
						WLNAME = "��" + WLDD.Substring(WLLOC, 1);
					break;
				case "0":
					string locList = "123567";
					if (locList.IndexOf(WLLOC.ToString().Trim()) > 0 && WLAMT.Substring(WLLOC + 1, 1) != "0")
						if(mstrLanguage.Equals("C"))
							WLNAME = "�s";
						else
							WLNAME = "��";
					else
						WLNAME = "";
					if (WLAMT.Substring(WLLOC,1) == ".")
						WLNAME = WLDD.Substring(WLLOC, 1);
					
					if (WLLOC == 4 && (WLAMT.Substring(1, 1) != "0" || WLAMT.Substring(2, 1) != "0" || WLAMT.Substring(3, 1) != "0"))
						if(mstrLanguage.Equals("C"))
							WLNAME = "�U";
						else
							WLNAME = "��";
					break;
																
			}																														 																														 
			return WLNAME.Trim();
		}

		private static string StrFormat(double Tlong,int Along,int Adec)
		{
			string tstr;
   
   
			tstr = Tlong.ToString().Trim();
			if (tstr.IndexOf(".") == -1)
			{
				tstr += ".00";
			}
			else
			{
				if (tstr.IndexOf(".") == 0)
				{
					tstr = "0" + tstr;
				}
				if (tstr.IndexOf(".") == tstr.Length - 1)  //0.  case
				{
					tstr = tstr + "0";
				}
				if (tstr.Substring(tstr.IndexOf(".") + 1).Length == 1)
				{
					tstr = tstr + "0";
				}
				else
				{
					tstr = tstr.Substring(0, tstr.IndexOf(".") + 3);
				}
				
			}	
			if (tstr.Length < 12) 
				tstr = tstr.PadLeft(12,' ');
			return tstr;

		}


		/// <summary>
		/// �����LABEL�ı�ǩ�ϱ�����ʾ�����ַ�&
		/// ���ô˷���ת��
		/// </summary>
		/// <param name="strCaption"></param>
		/// <returns></returns>
		public static string sfLableCaption(string strCaption)
		{
			return strCaption.Replace("&","&&");
		}

		/// <summary>
		/// ��ǩ�ı�̫��ʱ�Ĵ��������ض�һ�����ȣ��������....����
		/// ȱʡ����Ϊ20
		/// </summary>
		/// <param name="strCaption"></param>		
		/// <returns></returns>
		public static string sfLongLableCaption(string strCaption)
		{
			return sfLongLableCaption(strCaption,20);
		}
		public static string sfLongLableCaption(string strCaption,int length)
		{
			string strRet = strCaption;
			if(strCaption.Length > length)
			{
				strRet = strCaption.Substring(0,length) + "......";
			}
			return strRet;
		}

		/// <summary>
		/// ���滻Windows�е��ļ����в�֧�ֵķ���(\ / : * ? " < > |)
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static string sfReplaceFileNameSymol(string fileName)
		{
			string strRet = fileName;
			strRet =  strRet.Replace(@"\"," ");
			strRet =  strRet.Replace("/"," ");
			strRet =  strRet.Replace(":","��");
			strRet =  strRet.Replace("*","*");
			strRet =  strRet.Replace("?","��");
			strRet =  strRet.Replace("<","��");
			strRet =  strRet.Replace(">","��");
			strRet =  strRet.Replace("|","��");
			strRet =  strRet.Replace("&","");
			strRet =  strRet.Replace("'","��");
			

			return strRet;
		}


		/// <summary>
		/// ���ļ�������ת��
		/// �磺���л���Ա���͹��������ƵĿ��ȼ�.doc  ==> �л���Ա���͹��������Ƶ�~.doc
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static string sfLongFileName(string fileName,int length)
		{
			int lng = 0;
						
			string strName = fileName;
			string strExtName = "";


			lng = fileName.LastIndexOf(".");

			try
			{
			
				if(lng> 0)
				{
					strName = fileName.Substring(0,lng);
					strExtName = fileName.Substring(lng+1);
					

				}
				else if(lng == 0)
				{
					if(fileName.Length > 1)
					{
						strName = fileName;
					}
					else
					{
						strName = "";
					}
				}
				if(strName.Length > length)
				{
					strName = strName.Substring(0,length - 1) + "~";
				}
				return strName + "." + strExtName;
			}
			catch
			{
				return fileName;
			}
			
		}

		#region ����ת�� 
		// duanqs [4/11/2005]
		/// <summary>
		/// �ַ���ת��ΪLong
		/// </summary>
		/// <param name="strValue"></param>
		/// <returns></returns>
		public static long String2Long(string strValue)
		{
			if (strValue == null)
				strValue = "0";
			strValue=strValue.Trim()==""?"0":strValue;
			long lngRet = 0;
			try
			{
				lngRet = long.Parse(strValue);

			}
			catch
			{
				lngRet = 0;
			}
			return lngRet;
		}

        /// <summary>
        /// �ַ���ת��ΪDecimal
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static decimal String2Decimal(string strValue)
        {
            if (strValue == null)
                strValue = "0";
            strValue = strValue.Trim() == "" ? "0" : strValue;
            decimal lngRet = 0;
            try
            {
                lngRet = decimal.Parse(strValue);

            }
            catch
            {
                lngRet = 0;
            }
            return lngRet;
        }

		/// <summary>
		/// �ַ���ת��ΪINT
		/// </summary>
		/// <param name="strValue"></param>
		/// <returns></returns>
		public static int String2Int(string strValue)
		{
			if (strValue == null)
				strValue = "0";
			strValue=strValue.Trim()==""?"0":strValue;
            int iRet = 0;
            try
            {
                iRet = int.Parse(strValue);

            }
            catch
            {
                iRet = 0;
            }
			return iRet;
		}

		/// <summary>
		/// ���ַ���ת��ΪAscii��,���ֵ�ASCII����3λ��߲���
		/// </summary>
		/// <param name="strValue"></param>
		/// <returns></returns>
		public static string  String2Ascii(string strValue)
		{
			string sAsc="";
			ASCIIEncoding asci=new ASCIIEncoding();
			byte[] bts=asci.GetBytes(strValue);
			foreach(byte t in bts)
			{
				sAsc+=t.ToString().PadLeft(3,'0');
			}
			return sAsc;
		}

		/// <summary>
		/// ASCII�ַ���ת��Ϊ�ַ���
		/// </summary>
		/// <param name="strAsc">����Ϊ3��������</param>
		/// <returns></returns>
		public static string Ascii2String(string strAsc)
		{
			byte[] bts=new byte[strAsc.Length/3];
			string schar="";int j=0;
			ASCIIEncoding asci=new ASCIIEncoding();
			for(int i=0;i<strAsc.Length;i=i+3)
			{
				schar=strAsc.Substring(i,3);
				bts[j]=byte.Parse(schar);
				j++;
			}
			string str= new String(asci.GetChars(bts));
			return str;
		}
		#endregion
		

	}
}
