using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Epower.DevBase.BaseTools
{
    public class GBToPY
    {
        private static int[] FIRST_TABLE = { 45217, 45253, 45761, 46318, 46826,
                47010, 47297, 47614, 47614, 48119, 49062, 49324, 49896, 50371,
                50614, 50622, 50906, 51387, 51446, 52218, 52218, 52218, 52698,
                52980, 53689, 54481, 55289 };

        private static string[] ALL_VALUE = { "zuo", "zun", "zui", "zuan", "zu",
                "zou", "zong", "zi", "zhuo", "zhun", "zhui", "zhuang", "zhuan",
                "zhuai", "zhua", "zhu", "zhou", "zhong", "zhi", "zheng",
                "zhen", "zhe", "zhao", "zhang", "zhan", "zhai", "zha", "zeng",
                "zen", "zei", "ze", "zao", "zang", "zan", "zai", "za", "yun",
                "yue", "yuan", "yu", "you", "yong", "yo", "ying", "yin", "yi",
                "ye", "yao", "yang", "yan", "ya", "xun", "xue", "xuan", "xu",
                "xiu", "xiong", "xing", "xin", "xie", "xiao", "xiang", "xian",
                "xia", "xi", "wu", "wo", "weng", "wen", "wei", "wang", "wan",
                "wai", "wa", "tuo", "tun", "tui", "tuan", "tu", "tou", "tong",
                "ting", "tie", "tiao", "tian", "ti", "teng", "te", "tao",
                "tang", "tan", "tai", "ta", "suo", "sun", "sui", "suan", "su",
                "sou", "song", "si", "shuo", "shun", "shui", "shuang", "shuan",
                "shuai", "shua", "shu", "shou", "shi", "sheng", "shen", "she",
                "shao", "shang", "shan", "shai", "sha", "seng", "sen", "se",
                "sao", "sang", "san", "sai", "sa", "ruo", "run", "rui", "ruan",
                "ru", "rou", "rong", "ri", "reng", "ren", "re", "rao", "rang",
                "ran", "qun", "que", "quan", "qu", "qiu", "qiong", "qing",
                "qin", "qie", "qiao", "qiang", "qian", "qia", "qi", "pu", "po",
                "ping", "pin", "pie", "piao", "pian", "pi", "peng", "pen",
                "pei", "pao", "pang", "pan", "pai", "pa", "ou", "o", "nuo",
                "nue", "nuan", "nv", "nu", "nong", "niu", "ning", "nin", "nie",
                "niao", "niang", "nian", "ni", "neng", "nen", "nei", "ne",
                "nao", "nang", "nan", "nai", "na", "mu", "mou", "mo", "miu",
                "ming", "min", "mie", "miao", "mian", "mi", "meng", "men",
                "mei", "me", "mao", "mang", "man", "mai", "ma", "luo", "lun",
                "lue", "luan", "lv", "lu", "lou", "long", "liu", "ling", "lin",
                "lie", "liao", "liang", "lian", "lia", "li", "leng", "lei",
                "le", "lao", "lang", "lan", "lai", "la", "kuo", "kun", "kui",
                "kuang", "kuan", "kuai", "kua", "ku", "kou", "kong", "keng",
                "ken", "ke", "kao", "kang", "kan", "kai", "ka", "jun", "jue",
                "juan", "ju", "jiu", "jiong", "jing", "jin", "jie", "jiao",
                "jiang", "jian", "jia", "ji", "huo", "hun", "hui", "huang",
                "huan", "huai", "hua", "hu", "hou", "hong", "heng", "hen",
                "hei", "he", "hao", "hang", "han", "hai", "ha", "guo", "gun",
                "gui", "guang", "guan", "guai", "gua", "gu", "gou", "gong",
                "geng", "gen", "gei", "ge", "gao", "gang", "gan", "gai", "ga",
                "fu", "fou", "fo", "feng", "fen", "fei", "fang", "fan", "fa",
                "er", "en", "e", "duo", "dun", "dui", "duan", "du", "dou",
                "dong", "diu", "ding", "die", "diao", "dian", "di", "deng",
                "de", "dao", "dang", "dan", "dai", "da", "cuo", "cun", "cui",
                "cuan", "cu", "cou", "cong", "ci", "chuo", "chun", "chui",
                "chuang", "chuan", "chuai", "chu", "chou", "chong", "chi",
                "cheng", "chen", "che", "chao", "chang", "chan", "chai", "cha",
                "ceng", "ce", "cao", "cang", "can", "cai", "ca", "bu", "bo",
                "bing", "bin", "bie", "biao", "bian", "bi", "beng", "ben",
                "bei", "bao", "bang", "ban", "bai", "ba", "ao", "ang", "an",
                "ai", "a" };

        private static int[] ALL_CODE = { -10254, -10256, -10260, -10262,
                -10270, -10274, -10281, -10296, -10307, -10309, -10315, -10322,
                -10328, -10329, -10331, -10519, -10533, -10544, -10587, -10764,
                -10780, -10790, -10800, -10815, -10832, -10838, -11014, -11018,
                -11019, -11020, -11024, -11038, -11041, -11045, -11052, -11055,
                -11067, -11077, -11097, -11303, -11324, -11339, -11340, -11358,
                -11536, -11589, -11604, -11781, -11798, -11831, -11847, -11861,
                -11867, -12039, -12058, -12067, -12074, -12089, -12099, -12120,
                -12300, -12320, -12346, -12359, -12556, -12585, -12594, -12597,
                -12607, -12802, -12812, -12829, -12831, -12838, -12849, -12852,
                -12858, -12860, -12871, -12875, -12888, -13060, -13063, -13068,
                -13076, -13091, -13095, -13096, -13107, -13120, -13138, -13147,
                -13318, -13326, -13329, -13340, -13343, -13356, -13359, -13367,
                -13383, -13387, -13391, -13395, -13398, -13400, -13404, -13406,
                -13601, -13611, -13658, -13831, -13847, -13859, -13870, -13878,
                -13894, -13896, -13905, -13906, -13907, -13910, -13914, -13917,
                -14083, -14087, -14090, -14092, -14094, -14097, -14099, -14109,
                -14112, -14122, -14123, -14125, -14135, -14137, -14140, -14145,
                -14149, -14151, -14159, -14170, -14345, -14353, -14355, -14368,
                -14379, -14384, -14399, -14407, -14429, -14594, -14630, -14645,
                -14654, -14663, -14668, -14670, -14674, -14678, -14857, -14871,
                -14873, -14882, -14889, -14894, -14902, -14908, -14914, -14921,
                -14922, -14926, -14928, -14929, -14930, -14933, -14937, -14941,
                -15109, -15110, -15117, -15119, -15121, -15128, -15139, -15140,
                -15141, -15143, -15144, -15149, -15150, -15153, -15158, -15165,
                -15180, -15183, -15362, -15363, -15369, -15375, -15377, -15385,
                -15394, -15408, -15416, -15419, -15435, -15436, -15448, -15454,
                -15625, -15631, -15640, -15652, -15659, -15661, -15667, -15681,
                -15701, -15707, -15878, -15889, -15903, -15915, -15920, -15933,
                -15944, -15958, -15959, -16155, -16158, -16169, -16171, -16180,
                -16187, -16202, -16205, -16212, -16216, -16220, -16393, -16401,
                -16403, -16407, -16412, -16419, -16423, -16427, -16429, -16433,
                -16448, -16452, -16459, -16465, -16470, -16474, -16647, -16657,
                -16664, -16689, -16706, -16708, -16733, -16915, -16942, -16970,
                -16983, -17185, -17202, -17417, -17427, -17433, -17454, -17468,
                -17482, -17487, -17496, -17676, -17683, -17692, -17697, -17701,
                -17703, -17721, -17730, -17733, -17752, -17759, -17922, -17928,
                -17931, -17947, -17950, -17961, -17964, -17970, -17988, -17997,
                -18012, -18181, -18183, -18184, -18201, -18211, -18220, -18231,
                -18237, -18239, -18446, -18447, -18448, -18463, -18478, -18490,
                -18501, -18518, -18526, -18696, -18697, -18710, -18722, -18731,
                -18735, -18741, -18756, -18763, -18773, -18774, -18783, -18952,
                -18961, -18977, -18996, -19003, -19006, -19018, -19023, -19038,
                -19212, -19218, -19224, -19227, -19235, -19238, -19242, -19243,
                -19249, -19261, -19263, -19270, -19275, -19281, -19288, -19289,
                -19467, -19479, -19484, -19500, -19515, -19525, -19531, -19540,
                -19715, -19725, -19728, -19739, -19741, -19746, -19751, -19756,
                -19763, -19774, -19775, -19784, -19805, -19976, -19982, -19986,
                -19990, -20002, -20026, -20032, -20036, -20051, -20230, -20242,
                -20257, -20265, -20283, -20292, -20295, -20304, -20317, -20319 };

        public static string getAllPY(string gb2312)
        {
            if (null == gb2312 || "".Equals(gb2312.Trim()))
            {
                return gb2312;
            }
            char[] chars = gb2312.ToCharArray();
            StringBuilder retuBuf = new StringBuilder();
            for (int i = 0, Len = chars.Length; i < Len; i++)
            {
                retuBuf.Append(getAllPY(chars[i]));
            } // end of for
            return retuBuf.ToString();
        }

        public static string getAllPY(char gb2312)
        {
            int ascii = getCnAscii(gb2312);
            if (ascii == 0)
            { // 取ascii时出错
                return new string(gb2312, 1);
            }
            else
            {
                string spell = getSpellByAscii(ascii);
                if (spell == null)
                {
                    return new string(gb2312, 1);
                }
                else
                {
                    return spell;
                } // end of if spell == null
            }
        }

        public static char getFirstPY(char ch)
        {
            if (ch >= 0 && ch <= 0x7F)
            {
                return ch;
            }
            int gb = 0;

            byte[] bytes = Encoding.GetEncoding("gb2312").GetBytes(new string(ch, 1));
            if (bytes.Length < 2)
            {
                gb = byte2Int(bytes[0]);
            }
            gb = (bytes[0] << 8 & 0xff00) + (bytes[1] & 0xff);
            if (gb < FIRST_TABLE[0])
                return ch;
            int i;
            for (i = 0; i < 26; ++i)
            {
                if (match(i, gb))
                    break;
            }
            if (i >= 26)
                return ch;
            else
                return (char)(65 + i);
        }

        public static string getFirstPY(string src)
        {
            StringBuilder sb = new StringBuilder();
            int len = src.Length;
            int i;
            for (i = 0; i < len; i++)
            {
                sb.Append(getFirstPY(src[i]));
            }
            return sb.ToString();
        }

        private static int getCnAscii(char cn)
        {
            byte[] bytes = null;
            bytes = Encoding.GetEncoding("gb2312").GetBytes(new string(cn, 1));
            if (bytes == null || bytes.Length > 2 || bytes.Length <= 0)
            {
                return 0;
            }
            if (bytes.Length == 1)
            {
                return bytes[0];
            }
            else
            {
                int hightByte = bytes[0];
                int lowByte = bytes[1];
                int ascii = (256 * hightByte + lowByte) - 256 * 256;
                return ascii;
            }
        }

        private static string getSpellByAscii(int ascii)
        {
            if (ascii > 0 && ascii < 160)
            { // 单字符
                return new string((char)ascii, 1);
            }
            if (ascii < -20319 || ascii > -10247)
            { // 不知道的字符
                return null;
            }
            int first = 0;
            int sLast = ALL_CODE.Length - 1;
            int last = ALL_CODE.Length - 1;
            int mid;
            int temp;
            while (true)
            {
                mid = (first + last) >> 1;
                if (ascii == ALL_CODE[mid])
                {
                    return ALL_VALUE[mid];
                }
                else if (ascii > ALL_CODE[mid])
                {
                    temp = mid - 1;
                    if (temp >= 0)
                    {
                        if (ascii < ALL_CODE[temp])
                        {
                            return ALL_VALUE[mid];
                        }
                        else
                        {
                            last = mid;
                        }
                    }
                    else
                    {
                        return ALL_VALUE[0];
                    }
                }
                else
                {
                    if (mid + 1 <= sLast)
                    {
                        first = mid + 1;
                    }
                    else
                    {
                        return ALL_VALUE[sLast];
                    }
                }
            }
        }

        private static bool match(int i, int gb)
        {
            if (gb < FIRST_TABLE[i])
            {
                return false;
            }
            int j = i + 1;
            // 字母Z使用了两个标签
            while (j < 26 && (FIRST_TABLE[j] == FIRST_TABLE[i]))
            {
                ++j;
            }
            if (j == 26)
                return gb <= FIRST_TABLE[j];
            else
                return gb < FIRST_TABLE[j];
        }

        private static int byte2Int(byte b)
        {
            if (b < 0)
            {
                return 256 + b;
            }
            else
            {
                return b;
            }
        }

        public static bool isSpliter(char c)
        {
            char[] spliter = { ',', '，', ';', '；' };
            foreach (char cc in spliter)
            {
                if (c == cc)
                {
                    return true;
                }
            }
            return false;
        }

        public static string[] split(string src)
        {
            string text = src.Trim();
            StringBuilder sb = new StringBuilder();
            ArrayList al = new ArrayList();
            int i = 0;
            //跳过之前的分隔符
            for (i = 0; i < text.Length; i++)
            {
                if (!isSpliter(text[i]))
                {
                    break;
                }
            }
            for (; i < text.Length; i++)
            {
                if (isSpliter(text[i]))
                {
                    if (sb.Length > 0)
                    {
                        al.Add(sb.ToString());
                    }
                    sb = new StringBuilder();
                }
                else
                {
                    sb.Append(text[i]);
                }
            }
            if (sb.Length > 0)
            {
                al.Add(sb.ToString());
            }
            if (al.Count > 0)
            {
                string[] ret = new string[al.Count];
                for (i = 0; i < al.Count; i++)
                {
                    ret[i] = (string)al[i];
                }
                return ret;
            }
            else
            {
                return null;
            }
        }
    }
}
