/*******************************************************************
 *
 * Description:手机客户端
 * 
 * 
 * Create By  :谭雨
 * Create Date:2012年8月7日
 * *****************************************************************/
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Epower.DevBase.Organization;
using Epower.ITSM.SqlDAL.Mobile;

namespace E8ITSM_Phone.WebServiceITSM
{
    /// <summary>
    /// LoginInterface android客户端登陆类
    /// </summary>
    [WebService(Namespace = "http://feifanE8.cn/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。

    // [System.Web.Script.Services.ScriptService]
    public class LoginInterface : System.Web.Services.WebService
    {

        private string CONST_ENCRYKEY = "WangyqLijSuks_GainALotOfMoney_AndBuyCarAndBuildHouse";

        /// <summary>
        /// 登陆接口 
        /// { 
        ///   1.F失败标志（F#ERROR）
        ///   2.S成功标志（S#ROLE#UserID#TOKENID）
        ///   3.R拒绝标志（R#INFO）
        ///   4.返回errorNET，表示方法异常
        /// }
        /// </summary>
        /// <param name="loginName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <returns>string</returns>
        [WebMethod]
        public string LoginWithMobile(string loginName, string passWord)
        {
            string strOut = string.Empty;
            try
            {
                //生成tokenID
                string guid = System.Guid.NewGuid().ToString("N");

                //判断用户是否存在，密码是否正确
                LogSession objSession = new LogSession();
                int Result = objSession.Login(loginName.Trim(), passWord.Trim());
                switch (Result)
                {
                    //用户名和密码验证成功
                    case 0:
                        //添加登陆信息TS_MobileloginHistory表
                        MobileDP.AddMobileloginHistory(objSession.UserID);

                        //验证是否30分钟内连续错误登录系统5次
                        DateTime dt = DateTime.Now;
                        DateTime dtEnd = dt.AddMinutes(-30);
                        if (MobileDP.IsUserLoginFrequency(objSession.UserID, dt.ToString(), dtEnd.ToString()))
                        {
                            //添加登陆Token信息
                            MobileDP.AddToken(guid, objSession.UserID, "M", "1", "Mobile");

                            string tokenID = MobileDP.GetTokenMaxID(objSession.UserID);
                            strOut = "S#Normal#" + objSession.UserID + "#" + tokenID.ToString();

                            //修改登陆类型为1，TS_MobileloginHistory表
                            long mlID = MobileDP.GetMobileloginHistoryMaxID(objSession.UserID);
                            MobileDP.UpdateMobileloginHistory(mlID, objSession.UserID, "1", "");
                        }
                        else
                        {
                            strOut = "R#30分钟内不可以连续错误登录系统5次";
                            //修改登陆类型为2，TS_MobileloginHistory表
                            long mlID = MobileDP.GetMobileloginHistoryMaxID(objSession.UserID);
                            MobileDP.UpdateMobileloginHistory(mlID, objSession.UserID, "2", "");
                        }
                        break;
                    case -1:
                        strOut = "F#登录用户不存在";
                        break;
                    case -2:
                        //添加登陆信息TS_MobileloginHistory表
                        MobileDP.AddMobileloginHistory(objSession.UserID);

                        //修改登陆类型为1，TS_MobileloginHistory表
                        long mlID2 = MobileDP.GetMobileloginHistoryMaxID(objSession.UserID);
                        MobileDP.UpdateMobileloginHistory(mlID2, objSession.UserID, "0", "");

                        strOut = "F#登录用户密码错误";
                        break;
                }
            }
            catch
            {
                strOut = "errorNET";
            }
            return strOut;
        }

        /// <summary>
        /// 解密密码
        /// </summary>
        /// <param name="passWord">密码</param>
        /// <returns></returns>
        public string DeCryptPwd(string passWord)
        {
            string strOut = string.Empty;
            try
            {
                strOut = EpowerGlobal.MessageGlobal.DeCryptData(passWord, CONST_ENCRYKEY).Trim();
            }
            catch
            {
                strOut = "errorNET";
            }
            return strOut;
        }

        /// <summary>
        /// 加密密码
        /// </summary>
        /// <param name="passWord">密码</param>
        /// <returns></returns>
        [WebMethod]
        public string EnCryptPwd(string passWord)
        {
            string strOut = string.Empty;
            try
            {
                strOut = EpowerGlobal.MessageGlobal.EnCryptData(passWord, CONST_ENCRYKEY).Trim();
            }
            catch
            {
                strOut = "errorNET";
            }
            return strOut;
        }

        /// <summary>
        /// 从web.config获取一个Url路径
        /// </summary>
        /// <returns>string</returns>
        [WebMethod]
        public string GetRequestUrl()
        {
            string strOut = string.Empty;
            try
            {
                if (System.Configuration.ConfigurationSettings.AppSettings["RequestUrl"] != null)
                {
                    strOut = System.Configuration.ConfigurationSettings.AppSettings["RequestUrl"].ToString();
                }
            }
            catch
            {
                strOut = "errorNET";
            }
            return strOut;
        }

    }
}
