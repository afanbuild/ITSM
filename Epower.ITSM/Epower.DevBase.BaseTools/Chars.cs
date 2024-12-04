using System;

namespace  Epower.DevBase.BaseTools
{
	/// <summary>
	/// chars 的摘要说明。
	/// </summary>
	public class Chars
	{
		public Chars()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

        public static int GetLength(String aOrgStr)
        {   
            int intLen=aOrgStr.Length; 
            int i;    
            char[] chars = aOrgStr.ToCharArray() ;
            for(i=0;i<chars.Length;i++)
            {          
                if(System.Convert.ToInt32( chars[i] )>255)
                { 
                    intLen++; 
                } 
            } 
            return intLen; 
        }


        public static String MutiSubString(String aOrgStr ,  int aLength, ref String aAfterStr)
        {
            int intLen = aOrgStr.Length ;
            int start = 0 ;
            int end = intLen ;
            int single = 0;
            char[] chars = aOrgStr.ToCharArray();
            for (int i=0; i<chars.Length ;i++)
            {
                if (System.Convert.ToInt32(chars[i])>255)
                {
                    start += 2;
                }
                else
                {
                    start += 1;
                    single ++ ;
                }
                if (start >= aLength)
                {
      
                    if ( end % 2 == 0)
                    {
                        if ( single % 2 == 0 )
                        {
                            end = i+1 ;
                        }
                        else
                        {
                            end = i ;
                        }
                    }
                    else
                    {
                        end = i+1 ;
                    }
                    break ;
                }
            }
            string temp = aOrgStr.Substring(0, end);
            string temp2 = aOrgStr.Remove(0,end);
            aAfterStr = temp2 ;
            return temp;            
        }

  

	}
}
