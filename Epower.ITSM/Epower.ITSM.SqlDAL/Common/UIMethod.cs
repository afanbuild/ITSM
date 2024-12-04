/****************************************************************************
 * 
 * description:生成菜单
 * 
 * 
 * 
 * Create by:
 * Create Date:
 * *************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// 
    /// </summary>
    public class UIMethod
    {
        private string _strUserName = string.Empty;
        /// <summary>
        /// 获取管理员下级菜单的数组的脚本
        /// </summary>
        /// <param name="strUserName"></param>
        /// <param name="lngUserID"></param>
        /// <returns></returns>
        public string GetAdminSubMenuArrayScript(string strUserName, long lngUserID)
        {
            _strUserName = strUserName;
            bool isManager = UserDP.IsManager(lngUserID);

            string strStartIndex = SiteMap.RootNode["startIndex"];
            string strAdminIndex = SiteMap.RootNode["AdminIndex"];

            string strNodeIndex = "";
            if (strAdminIndex == strStartIndex)
            {
                //如果相同则取第一个   实际设置中几乎不太可能设置成一个
                strStartIndex = "1";
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("<script language=\"javascript\">\n");
            sb.Append("var arrOperateMenu = new Array();\n"); //MenuCode, SupMenuCode, MenuName, MenuUrl, Level, HasChild


            SiteMapNodeCollection childCollection = SiteMap.RootNode.ChildNodes;
            Hashtable htAllRights = (Hashtable)System.Web.HttpContext.Current.Session["UserAllRights"];

            int num = 0;
            int i = 1;
            foreach (SiteMapNode node in childCollection)
            {
                strNodeIndex = node["menuIndex"];
                if (strNodeIndex == strAdminIndex)
                {
                    foreach (SiteMapNode n in node.ChildNodes)
                    {
                        if (CheckRight(n))
                        {
                            sb.Append(GetChildMenusArrary(n, strNodeIndex + i.ToString().PadLeft(2, char.Parse("0")), ref num, 1, htAllRights, string.Empty));
                            i++;
                        }
                    }
                }
            }

            sb.Append("E8MenuConfigItems = arrOperateMenu;</script>\n");

            return sb.ToString();
        }



        /// <summary>
        /// 获取管理员菜单HTML, ITSM风格
        /// </summary>
        /// <param name="strUserName"></param>
        /// <param name="lngUserID"></param>
        /// <param name="strMens"></param>
        /// <param name="sWidth"></param>
        public void GetAdminMenuHtmlForITSM(string strUserName, long lngUserID, ref string strMens, string sHeight)
        {
            _strUserName = strUserName;
            bool isManager = UserDP.IsManager(lngUserID);

            string strStartIndex = SiteMap.RootNode["startIndex"];
            string strAdminIndex = SiteMap.RootNode["AdminIndex"];

            string strNodeIndex = "";
            if (strAdminIndex == strStartIndex)
            {
                //如果相同则取第一个   实际设置中几乎不太可能设置成一个
                strStartIndex = "1";
            }

            SiteMapNodeCollection childCollection = SiteMap.RootNode.ChildNodes;

            StringBuilder sb = new StringBuilder();
            sb.Append(@"<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0""><tr>");
            if (sHeight != "28")
            {
                sb.Append(@" <td width=""7""><img src=""images/lmt-1-a.gif"" width=""7"" height=""" + sHeight + @"""></td>");
            }
            //sb.Append(@" <td nowrap background=""images/lmt-1-b.gif"" width=""30""></td>");
            if (sHeight == "28")
            {
                sb.Append(@" <td nowrap width=""2""><img src=""images/lmt-1-xian.gif"" width=""2"" height=""" + sHeight + @"""></td>");
            }

            int i = 1;
            string imgbackurl = "images/lmt-1-b.gif";
            foreach (SiteMapNode node in childCollection)
            {
                strNodeIndex = node["menuIndex"];
                if (strNodeIndex == strAdminIndex)
                {
                    foreach (SiteMapNode n in node.ChildNodes)
                    {
                        if (CheckRight(n))
                        {
                            if (CheckRight(node))
                            {
                                if (i == 1)
                                    imgbackurl = "images/lmt-2.gif";
                                else
                                    imgbackurl = "images/lmt-1-b.gif";
                            }
                            if (n.HasChildNodes)
                            {
                                //有子菜单要弹出下一级菜单
                                sb.Append(@"<td nowrap id=""Td" + i.ToString() + @""" onMouseDown=""myMouseDown(this,'lmt-2.gif')"" onMouseOut=""chang(this,'lmt-1-b.gif')"" onClick=""E8MenuConfigGoToUrl('" + n.Url + @"')""  onMouseOver=""E8MenuConfigShowSubMenu('" + strNodeIndex + i.ToString().PadLeft(2, char.Parse("0"))
                                         + @"',this,'lmt-3.gif',1,0);"" width=""" + n.Title.Trim().Length * 17 + @"""  height=""" + sHeight + @""" align=""center"" background=""" + imgbackurl + @""" class=""td_0""><a href=""#"" class=""td_0"">" + n.Title +
                                          @"</a></td><td nowrap width=""2""><img src=""images/lmt-1-xian.gif"" width=""2"" height=""24""></td>");
                            }
                            else
                            {
                                sb.Append(@"<td nowrap id=""Td" + i.ToString() + @""" onMouseDown=""myMouseDown(this,'lmt-2.gif')"" onMouseOut=""chang(this,'lmt-1-b.gif')"" onClick=""E8MenuConfigGoToUrl('" + n.Url + @"')"" width=""" + n.Title.Trim().Length * 17 + @"""  height=""24"" align=""center"" background=""" + imgbackurl + @""" class=""td_0""><a href=""#"" class=""td_0"">" + n.Title +
                                          @"</a></td><td nowrap width=""2""><img src=""images/lmt-1-xian.gif"" width=""2"" height=""" + sHeight + @"""></td>");
                            }
                            i++;
                        }
                    }
                }

            }

            sb.Append(@"<td nowrap background=""images/lmt-1-b.gif"" width=""20""></td>
                                  </tr>
                                </table>");

            strMens = sb.ToString();

        }


        /// <summary>
        /// 获取下级菜单的数组的脚本
        /// </summary>
        /// <param name="strUserName"></param>
        /// <param name="lngUserID"></param>
        /// <returns></returns>
        public string GetSubMenuArrayScript(string strUserName, long lngUserID)
        {

            bool isManager = UserDP.IsManager(lngUserID);

            string strStartIndex = SiteMap.RootNode["startIndex"];
            string strAdminIndex = SiteMap.RootNode["AdminIndex"];

            string strNodeIndex = "";
            string strLeftUrl = string.Empty;
            if (strAdminIndex == strStartIndex)
            {
                //如果相同则取第一个   实际设置中几乎不太可能设置成一个
                strStartIndex = "1";
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("<script language=\"javascript\">\n");
            sb.Append("var arrOperateMenu = new Array();\n"); //MenuCode, SupMenuCode, MenuName, MenuUrl, Level, HasChild


            SiteMapNodeCollection childCollection = SiteMap.RootNode.ChildNodes;
            Hashtable htAllRights = (Hashtable)System.Web.HttpContext.Current.Session["UserAllRights"];

            int num = 0;
            foreach (SiteMapNode node in childCollection)
            {
                strNodeIndex = node["menuIndex"];
                if (node["LeftUrl"] != null)     //左边菜单
                    strLeftUrl = node["LeftUrl"].ToString();
                else
                    strLeftUrl = string.Empty;
                sb.Append(GetChildMenusArrary(node, strNodeIndex, ref num, 1, htAllRights, strLeftUrl));
            }

            sb.Append("E8MenuConfigItems = arrOperateMenu;</script>\n");

            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oNode"></param>
        /// <param name="SupMenuCode"></param>
        /// <param name="num"></param>
        /// <param name="Level"></param>
        /// <param name="htAllRights"></param>
        /// <param name="strLeftUrl"></param>
        /// <returns></returns>
        private string GetChildMenusArrary(SiteMapNode oNode, string SupMenuCode, ref int num, int Level, Hashtable htAllRights, string strLeftUrl)
        {
            StringBuilder strResult = new StringBuilder();

            //获取根节点的所有子节点列表 
            SiteMapNodeCollection oList = oNode.ChildNodes;

            int i = 1;
            foreach (SiteMapNode node in oList)
            {
                bool blnRight = false;
                CheckNodeRight(node, ref blnRight, htAllRights);
                if (blnRight == true)
                {
                    String MenuCode;
                    String MenuName = node.Title;
                    String MenuUrl = node.Url;
                    String sSubLeftUrl = string.Empty;

                    if (node["LeftUrl"] != null && node["LeftUrl"].ToString() != string.Empty)
                    {
                        sSubLeftUrl = node["LeftUrl"].ToString();
                    }
                    else
                    {
                        sSubLeftUrl = strLeftUrl;
                    }

                    MenuCode = SupMenuCode + (i + 1).ToString().PadLeft(2, '0');

                    int oldnum = num;

                    if (node.HasChildNodes)
                        strResult.Append(GetChildMenusArrary(node, MenuCode, ref num, Level + 1, htAllRights, sSubLeftUrl));

                    if (num > oldnum) //有子菜单
                        strResult.Append("arrOperateMenu[" + num + "] = new Array(\"" + MenuCode + "\", \"" + SupMenuCode + "\", \"" + MenuName + "\", \"" + MenuUrl + "\", " + Level.ToString() + ", true,\"" + sSubLeftUrl + "\");\n");
                    else
                        strResult.Append("arrOperateMenu[" + num + "] = new Array(\"" + MenuCode + "\", \"" + SupMenuCode + "\", \"" + MenuName + "\", \"" + MenuUrl + "\", " + Level.ToString() + ", false,\"" + sSubLeftUrl + "\");\n");

                    num++;

                }

                i++;

            }


            return strResult.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strUserName"></param>
        /// <param name="lngUserID"></param>
        /// <param name="strMens"></param>
        /// <param name="sWidth"></param>
        public void GetMenuHtmlForITSM(string strUserName, long lngUserID, ref string strMens, string sHeight)
        {
            bool isManager = UserDP.IsManager(lngUserID);

            string strStartIndex = SiteMap.RootNode["startIndex"];
            string strAdminIndex = SiteMap.RootNode["AdminIndex"];

            string strNodeIndex = "";
            if (strAdminIndex == strStartIndex)
            {
                //如果相同则取第一个   实际设置中几乎不太可能设置成一个
                strStartIndex = "1";
            }

            SiteMapNodeCollection childCollection = SiteMap.RootNode.ChildNodes;

            StringBuilder sb = new StringBuilder();
            sb.Append(@"<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0""><tr>");
            if (sHeight != "28")
            {
                sb.Append(@" <td width=""7""><img src=""images/lmt-1-a.gif"" width=""7"" height=""" + sHeight + @"""></td>");
            }
            //sb.Append(@" <td nowrap background=""images/lmt-1-b.gif"" width=""10""></td>");
            if (sHeight == "28")
            {
                sb.Append(@" <td nowrap width=""2""><img src=""images/lmt-1-xian.gif"" width=""2"" height=""" + sHeight + @"""></td>");
            }

            int i = 1;
            string imgbackurl = "images/lmt-1-b.gif";
            string DefaultUrl = string.Empty;
            string LeftUrl = string.Empty;
            foreach (SiteMapNode node in childCollection)
            {
                strNodeIndex = node["menuIndex"];
                if (isManager == true || strNodeIndex != strAdminIndex)
                {
                    if (CheckRight(node))
                    {
                        DefaultUrl = string.Empty;
                        if (node["DefaultUrl"] != null)
                        {
                            if (node.ResourceKey != null)
                            {
                                if (CheckRight(long.Parse(node.ResourceKey.Trim())))
                                    DefaultUrl = node["DefaultUrl"].ToString();
                            }
                            else
                                DefaultUrl = node["DefaultUrl"].ToString();
                        }
                        LeftUrl = string.Empty;
                        if (node["LeftUrl"] != null)
                        {
                            LeftUrl = node["LeftUrl"].ToString();
                        }
                        if (i == 1)
                            imgbackurl = "images/lmt-2.gif";
                        else
                            imgbackurl = "images/lmt-1-b.gif";
                        sb.Append(@"<td nowrap id=""Td" + i.ToString() + @""" onMouseDown=""myMouseDown(this,'lmt-2.gif')"" onMouseOut=""chang(this,'lmt-1-b.gif')"" onClick=""E8MenuConfigGoToUrl('" + DefaultUrl + @"');E8MenuLeftGoToUrl('" + LeftUrl + @"');"" onMouseOver=""E8MenuConfigShowSubMenu('" + strNodeIndex
                                 + @"',this,'lmt-3.gif',1,0);"" width=""" + node.Title.Trim().Length * 17 + @"""  height=""" + sHeight + @""" align=""center"" background=""" + imgbackurl + @""" class=""td_0""><a href=""#"" class=""td_0"">" + node.Title +
                                  @"</a></td><td nowrap width=""2""><img src=""images/lmt-1-xian.gif"" width=""2"" height=""" + sHeight + @"""></td>");
                        //                        sb.Append(@"<A onclick=""navTitleDiv.innerHTML='" + node.Title + @"';document.all.frameColumnNav.src='submenu.aspx?username=" + strUserName + "&mid=" + strNodeIndex + @"';setNavMenuBG(" + i.ToString() + @")""
                        //																href=""#""><IMG title=""" + node.Title + @""" src=""" + node["img"] + @""" width=""16"" align=""absMiddle""
                        //																	border=""0""></A> ");
                        i++;
                    }
                }

            }

            sb.Append(@"<td nowrap background=""images/lmt-1-b.gif"" width=""20""></td>
                                  </tr>
                                </table>");

            strMens = sb.ToString();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strUserName"></param>
        /// <param name="lngUserID"></param>
        /// <param name="strMens"></param>
        /// <param name="sWidth"></param>
        public void GetMenuHtmlNew(string strUserName, long lngUserID, ref string strMens, string sHeight)
        {
            bool isManager = UserDP.IsManager(lngUserID);

            string strStartIndex = SiteMap.RootNode["startIndex"];
            string strAdminIndex = SiteMap.RootNode["AdminIndex"];

            string strNodeIndex = "";
            if (strAdminIndex == strStartIndex)
            {
                //如果相同则取第一个   实际设置中几乎不太可能设置成一个
                strStartIndex = "1";
            }

            SiteMapNodeCollection childCollection = SiteMap.RootNode.ChildNodes;

            StringBuilder sb = new StringBuilder();
            sb.Append(@"<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0""><tr>");
            if (sHeight != "28")
            {
                sb.Append(@" <td width=""7""><img src=""images/lmt-1-a.gif"" width=""7"" height=""" + sHeight + @"""></td>");
            }
            //sb.Append(@" <td nowrap background=""images/lmt-1-b.gif"" width=""10""></td>");
            if (sHeight == "28")
            {
                sb.Append(@" <td nowrap width=""2""><img src=""images/lmt-1-xian.gif"" width=""2"" height=""" + sHeight + @"""></td>");
            }

            int i = 1;
            string imgbackurl = "images/lmt-1-b.gif";
            string DefaultUrl = string.Empty;
            string LeftUrl = string.Empty;
            foreach (SiteMapNode node in childCollection)
            {
                strNodeIndex = node["menuIndex"];
                if (isManager == true || strNodeIndex != strAdminIndex)
                {
                    if (CheckRight(node))
                    {
                        DefaultUrl = string.Empty;
                        if (node["DefaultUrl"] != null)
                        {
                            if (node.ResourceKey != null)
                            {
                                if (CheckRight(long.Parse(node.ResourceKey.Trim())))
                                    DefaultUrl = node["DefaultUrl"].ToString();
                            }
                            else
                                DefaultUrl = node["DefaultUrl"].ToString();
                        }
                        LeftUrl = string.Empty;
                        if (node["LeftUrl"] != null)
                        {
                            LeftUrl = node["LeftUrl"].ToString();
                        }
                        if (i == 1)
                            imgbackurl = "images/lmt-2.gif";
                        else
                            imgbackurl = "images/lmt-1-b.gif";
                        sb.Append(@"<td nowrap id=""Td" + i.ToString() + @""" onMouseDown=""myMouseDown(this,'lmt-2.gif')"" onMouseOut=""chang(this,'lmt-1-b.gif')"" onClick=""E8MenuConfigGoToUrl('" + DefaultUrl + @"');E8MenuLeftGoToUrl('" + LeftUrl + @"');"" onMouseOver=""E8MenuConfigShowSubMenu('" + strNodeIndex
                                 + @"',this,'lmt-3.gif',1,0);"" width=""" + node.Title.Trim().Length * 17 + @"""  height=""" + sHeight + @""" align=""center"" background=""" + imgbackurl + @""" class=""td_0""><a href=""#"" class=""td_0"">" + node.Title +
                                  @"</a></td><td nowrap width=""2""><img src=""images/lmt-1-xian.gif"" width=""2"" height=""" + sHeight + @"""></td>");
                        //                        sb.Append(@"<A onclick=""navTitleDiv.innerHTML='" + node.Title + @"';document.all.frameColumnNav.src='submenu.aspx?username=" + strUserName + "&mid=" + strNodeIndex + @"';setNavMenuBG(" + i.ToString() + @")""
                        //																href=""#""><IMG title=""" + node.Title + @""" src=""" + node["img"] + @""" width=""16"" align=""absMiddle""
                        //																	border=""0""></A> ");
                        i++;
                    }
                }

            }

            sb.Append(@"<td nowrap background=""images/lmt-1-b.gif"" width=""20""></td>
                                  </tr>
                                </table>");

            strMens = sb.ToString();
        }

        /// <summary>
        /// 获取普通用户菜单HTML
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="strHeader"></param>
        /// <param name="strMens"></param>
        public void GetMenuHtml(string strUserName, long lngUserID, ref string strHeader, ref string strMens)
        {
            StringBuilder sb = new StringBuilder("");

            StringBuilder sbTitle = new StringBuilder("");

            bool isManager = UserDP.IsManager(lngUserID);

            string strStartIndex = SiteMap.RootNode["startIndex"];
            string strAdminIndex = SiteMap.RootNode["AdminIndex"];

            string strNodeIndex = "";
            if (strAdminIndex == strStartIndex)
            {
                //如果相同则取第一个   实际设置中几乎不太可能设置成一个
                strStartIndex = "1";
            }

            SiteMapNodeCollection childCollection = SiteMap.RootNode.ChildNodes;

            sb.Append(@"<table class=""navContent"" height=""100%"" cellSpacing=""0"" cellPadding=""0"" width=""96%"" align=""center""
									border=""0"">
									<tr>
										<td vAlign=""top"" bgColor=""#ffffff"" height=""100%""><iframe id=""frameColumnNav"" style=""WIDTH: 100%; HEIGHT: 100%"" onfocus=""this.blur();"" border=""0""
												name=""frameColumnNav"" marginWidth=""0"" marginHeight=""0"" src=""submenu.aspx?username=" + strUserName + "&mid=" + strStartIndex + @""" frameBorder=""0"" scrolling=""auto""></iframe>
										</td>
									</tr>
									<tr>
										<td style=""CURSOR: hand"" onclick=""setNavMenu();"" vAlign=""bottom"" align=""center"" background=""skins/2004/images/bg_middle1_main.gif""
											height=""5""><IMG height=""5"" src=""skins/2004/images/bg_middle1_button.gif""></td>
									</tr>
									<tr id=""navMenu"" name=""navMenu"">
										<td vAlign=""bottom"" bgColor=""#ffffff"" height=""0px"">
											<table id=""tableNAV"" cellSpacing=""0"" cellPadding=""0"" width=""100%"" align=""center"" border=""0""
												name=""tableNAV"">");




            int i = 1;
            foreach (SiteMapNode node in childCollection)
            {
                //以下菜单都没有权限，则不可见
                if (CheckRight(node))
                {
                    strNodeIndex = node["menuIndex"];
                    string strLeftUrl = string.Empty;

                    if (node["LeftUrl"] != null)     //左边菜单
                    {
                        strLeftUrl = node["LeftUrl"].ToString();
                    }

                    if (isManager == true || strNodeIndex != strAdminIndex)
                    {
                        sb.Append(@"<tr><td align=""left"" onmouseover=""setNavMenuBGMove(" + i.ToString() + @")"" 
                               onmouseout=""setNavMenuBGOut(" + i.ToString() + @")""
                             class=""" + (strStartIndex == strNodeIndex ? "navColumnLight" : "navColumnDark") + @""" id=""navMenu" + i.ToString() + @""" style=""height: 25px""
                             onclick=""setNavUrl('" + strLeftUrl + @"');document.all.frameColumnNav.src='submenu.aspx?username=" + strUserName + "&mid=" + strNodeIndex + @"';
                                                     setNavMenuBG(" + i.ToString() + @")""
			                     ><IMG src=""" + node["img"] + @""" width=""16"" align=""absMiddle"">
														<A href=""#"">" + node.Title + @"</A>
													</td>
												</tr>");
                        if (strStartIndex == strNodeIndex)
                        {
                            //因为 开始ID 不可能与 管理ID相同,因此 此句一定能执行得到
                            sbTitle.Append(strLeftUrl);
                        }
                        i++;
                    }
                }
            }

            sb.Append(@"</table>
											<table id=""tableNavIndex"" cellSpacing=""0"" cellPadding=""0"" width=""100%"" border=""0"" name=""tableNavIndex"">
												<tr>
													<td class=""navColumnDark"" id=""navMenuIndex"" style=""DISPLAY: none"" height=""25"">
														<div align=""center"" width=""100%""> ");
            i = 1;
            foreach (SiteMapNode node in childCollection)
            {
                if (CheckRight(node))
                {
                    strNodeIndex = node["menuIndex"];
                    if (isManager == true || strNodeIndex != strAdminIndex)
                    {
                        string strLeftUrl = string.Empty;
                        string strLeftUrlShow = string.Empty;
                        strLeftUrlShow = @"<table width='170' height='25' border='0' cellpadding='0' cellspacing='0'>
                                                  <tr>
                                                    <td style='CURSOR: hand' onClick=""changeSrc('" + string.Empty + @"')"" width='52' height='25' align='center' valign='middle' background='images/lm-b.gif' class='STYLE2' id='home' onMouseDown=""myMouseDown(this,'lm-b.gif',1)"" onMouseOver=""chang(this,'lm-c.gif')"" onMouseOut=""chang(this,'lm-a.gif')""><span id='a1' class='STYLE1'>菜单</span></td>
                                                    <td width='2'></td>";
                        if (node["LeftUrl"] != null)     //左边菜单
                        {
                            strLeftUrl = node["LeftUrl"].ToString();
                            strLeftUrlShow += @"<td style='CURSOR: hand' onClick=""changeSrc('" + strLeftUrl + @"')"" width='52' height='25' align='center' valign='middle' background='images/lm-a.gif' class='td_01' onMouseDown=""myMouseDown(this,'lm-b.gif',2)"" onMouseOver=""chang(this,'lm-c.gif')"" onMouseOut=""chang(this,'lm-a.gif')""><span id='a2' class='STYLE2'>导航</span></td>";
                        }
                        else
                        {
                            strLeftUrlShow += @"<td width='52'></td>";
                        }
                        strLeftUrlShow += @"<td width='2'></td>
                                                    <td  width='52' height='25' ></td>
                                                    
                                                    <td width='10'></td>
                                                  </tr>
                                                </table>";

                        sb.Append(@"<A onclick=""setNavUrl('" + strLeftUrl + @"');document.all.frameColumnNav.src='submenu.aspx?username=" + strUserName + "&mid=" + strNodeIndex + @"';setNavMenuBG(" + i.ToString() + @")""
																href=""#""><IMG title=""" + node.Title + @""" src=""" + node["img"] + @""" width=""16"" align=""absMiddle""
																	border=""0""></A> ");
                        i++;
                    }
                }
            }

            sb.Append(@"</div>
													</td>
												</tr>
											</table>
										</td>
									</tr>
								</table>");

            strHeader = sbTitle.ToString();
            strMens = sb.ToString();
        }

        /// <summary>
        /// 获取普通用户菜单HTML
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="strHeader"></param>
        /// <param name="strMens"></param>
        public void GetMenuHtml2(string strUserName, long lngUserID, ref string strMens)
        {
            StringBuilder sb = new StringBuilder("");

            bool isManager = UserDP.IsManager(lngUserID);

            string strStartIndex = SiteMap.RootNode["startIndex"];
            string strAdminIndex = SiteMap.RootNode["AdminIndex"];

            string strNodeIndex = "";
            if (strAdminIndex == strStartIndex)
            {
                //如果相同则取第一个   实际设置中几乎不太可能设置成一个
                strStartIndex = "1";
            }

            SiteMapNodeCollection childCollection = SiteMap.RootNode.ChildNodes;

            int i = 1;
            foreach (SiteMapNode node in childCollection)
            {                
                //以下菜单都没有权限，则不可见
                if (CheckRight2(node))
                {
                    strNodeIndex = node["menuIndex"];

                    if (isManager == true || strNodeIndex != strAdminIndex)
                    {
                        sb.Append(@"<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" class=""b"">
                                                <tr>
                                                    <td height=""2"">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td onclick=""javascript:onSelect(" + i + @")"" width=""160"" height=""29"" align=""left"" valign=""middle""
                                                        background=""images/lanmu-a.gif"" onmousedown=""chang(this,'lanmu-c.gif'," + i + @")"" onmouseover=""chang(this,'lanmu-b.gif')""
                                                        onmouseout=""chang(this,'lanmu-a.gif')"">");
                        if(i==1)
                            sb.Append(@"&nbsp;&nbsp;<img src=""images/left_yj1.gif"" width=""16"" height=""16"" align=""absmiddle"" id=""imgID_" + i + @"""
                                                            border=""0"">");
                        else
                            sb.Append(@"&nbsp;&nbsp;<img src=""images/left_yj.gif"" width=""16"" height=""16"" align=""absmiddle"" id=""imgID_" + i + @"""
                                                            border=""0"">");

                        sb.Append(@"<a href=""#"" class=""td_3"">" + node.Title + @"</a>
                                                    </td>
                                                </tr>");
                        #region 循环node得到所有子项

                        if(i==1)
                            sb.Append(@"<tr id=""menuID_" + i + @""">");
                        else
                            sb.Append(@"<tr id=""menuID_" + i + @""" style=""display: none"">");

                        sb.Append(@"<td align=""right""><div class=""xlk"">
                                                        <table width=""95%"" border=""0"" cellpadding=""3"" cellspacing=""0"" bgcolor=""#FFFFFF"" class=""b"">");
                        //递归菜单
                        for (int j = 0; j < node.ChildNodes.Count; j++)
                        {
                            SiteMapNode node2 = node.ChildNodes[j];
                            
                            if (CheckRight2(node2))
                            {
                                string url = node2["url"];
                                sb.Append(@"<tr onmouseover=""mOvr(this,'#FFFBE8')"" onmouseout=""mOut(this,'');"">
                                                                <td align=""right"">
                                                                    <img src=""images/left_data.gif"" width=""16"" height=""16"">
                                                                </td>
                                                                <td>
                                                                    <a href=""#"" onclick=""GoUrl('" + node2.Url + @"');"">" + node2.Title + @" </a>
                                                                </td>
                                                            </tr>");
                            }
                            


                        }

                        sb.Append(@"</table></div>
                                    </td>
                                    </tr>
                                    </table>");
                        #endregion
                        i++;
                    }
                }
            }           

            strMens = sb.ToString();
        }

        /// <summary>
        /// 获取超级管理员菜单HTML内容
        /// </summary>
        /// <param name="strHeader"></param>
        /// <param name="strMens"></param>
        public void GetAdminOnlyMenuHtml(string strUserName, ref string strHeader, ref string strMens)
        {
            StringBuilder sb = new StringBuilder("");

            StringBuilder sbTitle = new StringBuilder("");

            string strAdminIndex = SiteMap.RootNode["AdminIndex"];
            string strNodeIndex = "";

            SiteMapNodeCollection childCollection = SiteMap.RootNode.ChildNodes;

            sb.Append(@"<table class=""navContent"" height=""100%"" cellSpacing=""0"" cellPadding=""0"" width=""96%"" align=""center""
									border=""0"">
									<tr>
										<td vAlign=""top"" bgColor=""#ffffff"" height=""100%""><iframe id=""frameColumnNav"" style=""WIDTH: 100%; HEIGHT: 100%"" onfocus=""this.blur();"" border=""0""
												name=""frameColumnNav"" marginWidth=""0"" marginHeight=""0"" src=""submenu.aspx?username=" + strUserName + "&mid=" + strAdminIndex + @""" frameBorder=""0"" scrolling=""auto""></iframe>
										</td>
									</tr>
									<tr>
										<td style=""CURSOR: hand"" onclick=""setNavMenu();"" vAlign=""bottom"" align=""center"" background=""skins/2004/images/bg_middle1_main.gif""
											height=""5""><IMG height=""5"" src=""skins/2004/images/bg_middle1_button.gif""></td>
									</tr>
									<tr id=""navMenu"" name=""navMenu"">
										<td vAlign=""bottom"" bgColor=""#ffffff"" height=""0px"">
											<table id=""tableNAV"" cellSpacing=""0"" cellPadding=""0"" width=""100%"" align=""center"" border=""0""
												name=""tableNAV"">");




            int i = 1;
            foreach (SiteMapNode node in childCollection)
            {
                strNodeIndex = node["menuIndex"];
                if (strAdminIndex == strNodeIndex)
                {

                    sb.Append(@"<tr><td align=""left"" onmouseover=""setNavMenuBGMove(" + node["menuIndex"] + @")"" 
                               onmouseout=""setNavMenuBGOut(" + strNodeIndex + @")""
                             class=""" + (strAdminIndex == strNodeIndex ? "navColumnLight" : "navColumnDark") + @""" id=""navMenu" + i.ToString() + @""" style=""height: 25px""
                             onclick=""document.all.frameColumnNav.src='submenu.aspx?username=" + strUserName + "&mid=" + strNodeIndex + @"';
                                                     setNavMenuBG(" + strNodeIndex + @")""
			                     ><IMG src=""" + node["img"] + @""" width=""16"" align=""absMiddle"">
														<A href=""#"">" + node.Title + @"</A>
													</td>
												</tr>");

                    sbTitle.Append(@"<div class=""navTitle"" id=""navTitleDiv"" name=""navTitleDiv"">" + node.Title + @"</div>");
                }
                i++;
            }

            sb.Append(@"</table>
											<table id=""tableNavIndex"" cellSpacing=""0"" cellPadding=""0"" width=""100%"" border=""0"" name=""tableNavIndex"">
												<tr>
													<td class=""navColumnDark"" id=""navMenuIndex"" style=""DISPLAY: none"" height=""25"">
														<div align=""center"" width=""100%""> ");

            foreach (SiteMapNode node in childCollection)
            {
                strNodeIndex = node["menuIndex"];
                if (strAdminIndex == strNodeIndex)
                {
                    sb.Append(@"<A onclick=""navTitleDiv.innerHTML='" + node.Title + @"';document.all.frameColumnNav.src='submenu.aspx?username=" + strUserName + "&mid=" + strNodeIndex + @"';setNavMenuBG(" + strNodeIndex + @")""
																href=""#""><IMG title=""" + node.Title + @""" src=""" + node["img"] + @""" width=""16"" align=""absMiddle""
																	border=""0""></A> ");
                }
            }

            sb.Append(@"</div>
													</td>
												</tr>
											</table>
										</td>
									</tr>
								</table>");

            strHeader = sbTitle.ToString();
            strMens = sb.ToString();
        }


        /// <summary>
        /// 如果为false,表示设置为没有权限的菜单移除，OUTLOOK风格时检查权限
        /// </summary>
        /// <param name="strID"></param>
        /// <param name="item"></param>
        /// <param name="htAllRights"></param>
        /// <param name="TreeView1"></param>
        /// <param name="node"></param>
        /// <param name="strName"></param>
        public void CheckNodeRight(string strID, TreeNode item, Hashtable htAllRights, TreeView TreeView1, SiteMapNode node, string strName)
        {
            _strUserName = strName;
            long OperatorID = 0;
            try
            {
                if (strID != string.Empty)
                {
                    OperatorID = long.Parse(strID);
                }
            }
            catch { }

            //修改 朱明春，为了去掉多了的菜单项，当非SA超级管理员时，
            string strvisble = "true";
            if (node["visible"] != null)
            {
                strvisble = node["visible"].ToString();
            }
            if (_strUserName.ToLower() != "sa" && strvisble.ToLower() == "false")
            {
                //移除菜单项
                TreeNode pitem = item.Parent;
                if (pitem != null)
                {
                    pitem.ChildNodes.Remove(item);
                }
                else
                {
                    TreeView1.Nodes.Remove(item);
                }
                return;
            }

            if (OperatorID == 0)
                return;

            RightEntity re = (RightEntity)htAllRights[OperatorID];
            if (re == null)
                return;
            else
            {
                if (re.CanRead == false)
                {
                    //移除菜单项
                    TreeNode pitem = item.Parent;
                    if (pitem != null)
                    {
                        pitem.ChildNodes.Remove(item);
                    }
                    else
                    {
                        TreeView1.Nodes.Remove(item);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool CheckRight(SiteMapNode node)
        {
            bool breturn = false;
            Hashtable htAllRights = (Hashtable)System.Web.HttpContext.Current.Session["UserAllRights"];
            CheckNodeRight(node, ref breturn, htAllRights);
            return breturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool CheckRight2(SiteMapNode node)
        {
            bool breturn = false;
            Hashtable htAllRights = (Hashtable)System.Web.HttpContext.Current.Session["UserAllRights"];
            CheckNodeRight2(node, ref breturn, htAllRights);
            return breturn;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="bRight"></param>
        /// <param name="htAllRights"></param>
        public void CheckNodeRight2(SiteMapNode node, ref bool bRight, Hashtable htAllRights)
        {
            if (bRight)   //如果发现有一个有权限，就跳出
                return;

            //修改 朱明春，为了去掉多了的菜单项
            string strvisble = "true";
            if (node["visible"] != null)
            {
                strvisble = node["visible"].ToString();
            }
            if (_strUserName.ToLower() != "sa" && strvisble.ToLower() == "false")
            {
                bRight = false;
                return;
            }
            if (!string.IsNullOrEmpty(node.ResourceKey))
            {
                if (!CheckRight(long.Parse(node.ResourceKey)))
                {
                    bRight = false;
                    return;
                }
            }

            SiteMapNode tempnode = node;
            //递归菜单
            for (int i = 0; i < tempnode.ChildNodes.Count; i++)
            {
                CheckNodeRight(tempnode.ChildNodes[i], ref bRight, htAllRights);
            }
            //如果没有子菜单，则移除父菜单
            if (tempnode.ChildNodes.Count == 0)
            {
                #region 朱明春20100130增加，如果设置为不可见时菜单就不可见，默认可见
                //朱明春20100130增加
                if (node["visible"] != null)
                    strvisble = node["visible"];
                if (_strUserName.ToLower() != "sa" && strvisble.ToLower() == "false")
                {
                    bRight = false;
                    return;
                }
                #endregion

                if (string.IsNullOrEmpty(tempnode.ResourceKey))
                {
                    bRight = true;
                    return;
                }
                RightEntity re = (RightEntity)htAllRights[long.Parse(tempnode.ResourceKey)];
                if (re == null)
                {
                    bRight = true;
                }
                else
                {
                    if (re.CanRead == false)
                    {
                        bRight = false;
                    }
                    else
                    {
                        bRight = true;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ResourceKey"></param>
        /// <returns></returns>
        public bool CheckRight(long ResourceKey)
        {
            bool breturn = false;
            Hashtable htAllRights = (Hashtable)System.Web.HttpContext.Current.Session["UserAllRights"];
            RightEntity re = (RightEntity)htAllRights[ResourceKey];
            if (re == null)
            {
                breturn = true;
            }
            else
            {
                if (re.CanRead == false)
                {
                    breturn = false;
                }
                else
                {
                    breturn = true;
                }
            }
            return breturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="bRight"></param>
        /// <param name="htAllRights"></param>
        public void CheckNodeRight(SiteMapNode node, ref bool bRight, Hashtable htAllRights)
        {
            if (bRight)   //如果发现有一个有权限，就跳出
                return;

            //修改 朱明春，为了去掉多了的菜单项
            string strvisble = "true";
            if (node["visible"] != null)
            {
                strvisble = node["visible"].ToString();
            }
            if (_strUserName.ToLower() != "sa" && strvisble.ToLower() == "false")
            {
                bRight = false;
                return;
            }

            SiteMapNode tempnode = node;
            //递归菜单
            for (int i = 0; i < tempnode.ChildNodes.Count; i++)
            {
                CheckNodeRight(tempnode.ChildNodes[i], ref bRight, htAllRights);
            }
            //如果没有子菜单，则移除父菜单
            if (tempnode.ChildNodes.Count == 0)
            {
                #region 朱明春20100130增加，如果设置为不可见时菜单就不可见，默认可见
                //朱明春20100130增加
                if (node["visible"] != null)
                    strvisble = node["visible"];
                if (_strUserName.ToLower() != "sa" && strvisble.ToLower() == "false")
                {
                    bRight = false;
                    return;
                }
                #endregion

                if (string.IsNullOrEmpty(tempnode.ResourceKey))
                {
                    bRight = true;
                    return;
                }
                RightEntity re = (RightEntity)htAllRights[long.Parse(tempnode.ResourceKey)];
                if (re == null)
                {
                    bRight = true;
                }
                else
                {
                    if (re.CanRead == false)
                    {
                        bRight = false;
                    }
                    else
                    {
                        bRight = true;
                    }
                }
            }
        }

        #region 所有功能、常用功能配置及显示
        /// <summary>
        /// 获得桌面配置项
        /// </summary>
        /// <param name="strUserName"></param>
        /// <param name="lngUserID"></param>
        /// <returns></returns>
        public string getMenuHtmlDeskPage(string strUserName, long lngUserID, int intVaueRows)
        {
            bool isManager = UserDP.IsManager(lngUserID);

            string strStartIndex = SiteMap.RootNode["startIndex"];
            string strAdminIndex = SiteMap.RootNode["AdminIndex"];

            string strNodeIndex = "";
            if (strAdminIndex == strStartIndex)
            {
                //如果相同则取第一个   实际设置中几乎不太可能设置成一个
                strStartIndex = "1";
            }

            SiteMapNodeCollection childCollection = SiteMap.RootNode.ChildNodes;
            StringBuilder strTB = new StringBuilder();

            strTB.Append("<table border=\"0\" cellspacing=\"2\" cellpadding=\"2\">");
            string DefaultUrl = string.Empty;
            string resourceKey = string.Empty;//当前权限resourceKey

            strTB.Append("<tr>");
            int RtnRowsUL = 0;
            int RtnRows = 0;
            foreach (SiteMapNode node in childCollection)
            {

                strNodeIndex = node["menuIndex"];
                if (isManager == true || strNodeIndex != strAdminIndex)
                {
                    if (CheckRight(node))
                    {
                        resourceKey = string.Empty;
                        DefaultUrl = string.Empty;
                        if (node["DefaultUrl"] != null)
                        {
                            if (node.ResourceKey != null)
                            {
                                resourceKey = long.Parse(node.ResourceKey.Trim()).ToString();
                                if (CheckRight(long.Parse(node.ResourceKey.Trim())))
                                    DefaultUrl = node["DefaultUrl"].ToString();
                            }
                            else
                                DefaultUrl = node["DefaultUrl"].ToString();
                        }
                        string Img = "";
                        if (node["img"] != null)
                        {
                            Img = node["img"].ToString();
                        }
                        if (RtnRows == 0)
                        {
                            strTB.Append("<td style=\"padding-top:auto;vertical-align:top;width:200px;\" nowrap>");
                            strTB.Append("<ul>");
                            RtnRowsUL++;
                        }
                        RtnRows++;
                        tableMuneLinePage(node.Title.Trim(), DefaultUrl, node, lngUserID, ref RtnRows, ref RtnRowsUL, ref strTB, intVaueRows, resourceKey);

                        if (RtnRows == intVaueRows)
                        {
                            for (int row = 0; row < RtnRowsUL; row++)
                            {
                                strTB.Append("</ul>");
                            }
                            strTB.Append("</td>");
                            RtnRows = 0;
                        }
                    }
                }


            }
            if (RtnRows != 0)
            {
                for (int row = 0; row < RtnRowsUL; row++)
                {
                    strTB.Append("</ul>");
                }
                strTB.Append("</td>");
            }



            strTB.Append("</tr>");
            strTB.Append("</table>");
            return strTB.ToString();
        }


        /// <summary>
        /// 全部功能明细菜单
        /// </summary>
        /// <param name="titile"></param>
        /// <param name="DefaultUrl"></param>
        /// <param name="node"></param>
        /// <param name="RtnRows"></param>
        /// <param name="RtnRowsUL"></param>
        /// <param name="strDatable"></param>
        /// <returns></returns>
        public string tableMuneLinePage(string titile, string DefaultUrl, SiteMapNode node, long lngUserID, ref int RtnRows, ref int RtnRowsUL, ref StringBuilder strDatable, int intVaueRows, string resourceKey)
        {
            Hashtable htAllRights = (Hashtable)System.Web.HttpContext.Current.Session["UserAllRights"];
            bool blnRight = false;
            CheckNodeRight(node, ref blnRight, htAllRights);
            if (blnRight == true)
            {
                string StrTitle = string.Empty;
                string strVlues = "NO";
                if (httpAjaxSC.IsAllPage(lngUserID, titile) == true)
                {
                    //全局权限常用功能
                    strVlues = "AllPage";
                    StrTitle = "<font color='red'>☆</font>";
                }
                else
                {
                    if (httpAjaxSC.IsOftenPage(lngUserID, titile) == true)
                    {
                        //个人权限常用功能
                        strVlues = "OftenPage";
                        StrTitle = "<font color='bule'>☆</font>";
                    }
                }

                strDatable.Append("<li title=\"" + titile.ToString() + "\"><font color='blue' size='3'><a herf=\"#\"  style=\"cursor:pointer\" onclick=\"E8MenuConfigGoToUrl('" + DefaultUrl + "')\" onmousemove=\"onmuseover(this,'" + titile + "','" + DefaultUrl + "','" + strVlues + "','" + resourceKey + "')\" onmouseout=\"onmuseoverOut(this)\"><B>" + StrTitle + titile + "</B></a></font></li>");
                if (RtnRows == intVaueRows)
                {
                    for (int xx = 0; xx < RtnRowsUL; xx++)
                    {
                        strDatable.Append("</ul>");
                    }
                    strDatable.Append("</td>");
                    RtnRows = 0;
                }

                tablmenus(node, lngUserID, ref strDatable, ref RtnRows, ref RtnRowsUL, intVaueRows);
            }


            return strDatable.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oNode"></param>
        /// <param name="strDatable"></param>
        /// <param name="RtnRows"></param>
        /// <param name="RtnRowsUL"></param>
        /// <param name="intVaueRows"></param>
        public void tablmenus(SiteMapNode oNode, long lngUserID, ref StringBuilder strDatable, ref int RtnRows, ref int RtnRowsUL, int intVaueRows)
        {
            SiteMapNodeCollection oList = oNode.ChildNodes;
            if (oList.Count > 0)
            {
                if (RtnRows != 0)
                {
                    strDatable.Append("<ul>");
                }
                RtnRowsUL++;
                foreach (SiteMapNode node in oList)
                {
                    Hashtable htAllRights = (Hashtable)System.Web.HttpContext.Current.Session["UserAllRights"];
                    bool blnRight = false;
                    CheckNodeRight(node, ref blnRight, htAllRights);
                    if (blnRight == true)
                    {
                        string titile = node.Title;
                        string DefaultUrl = node.Url;
                        string resourceKey = node.ResourceKey;
                        if (RtnRows == 0)
                        {
                            strDatable.Append("<td style=\"padding-top:auto;vertical-align:top;width:200px;\"  nowrap>");
                            for (int xx = 0; xx < RtnRowsUL; xx++)
                            {
                                strDatable.Append("<ul>");
                            }
                        }
                        RtnRows++;

                        //如果是全局常用功能 
                        string StrTitle2 = string.Empty;

                        string strVlues = "NO";
                        if (httpAjaxSC.IsAllPage(lngUserID, titile) == true)
                        {
                            //全局权限常用功能
                            strVlues = "AllPage";
                            StrTitle2 = "<font color='red'>☆</font>";
                        }
                        else
                        {
                            if (httpAjaxSC.IsOftenPage(lngUserID, titile) == true)
                            {
                                //个人权限常用功能
                                strVlues = "OftenPage";
                                StrTitle2 = "<font color='bule'>☆</font>";
                            }
                        }
                        strDatable.Append("<li title=\"" + titile.ToString() + "\"><a class=\"allFunctions\" herf=\"#\"  style=\"cursor:pointer\" onclick=\"E8MenuConfigGoToUrl('" + DefaultUrl + "')\" onmousemove=\"onmuseover(this,'" + titile + "','" + DefaultUrl + "','" + strVlues + "','" + resourceKey + "')\" onmouseout=\"onmuseoverOut(this)\">" + StrTitle2 + titile + "</a></li>");

                        if (RtnRows == intVaueRows)
                        {
                            for (int xx = 0; xx < RtnRowsUL; xx++)
                            {
                                strDatable.Append("</ul>");
                            }
                            strDatable.Append("</td>");
                            RtnRows = 0;

                        }
                        tablmenus(node, lngUserID, ref strDatable, ref  RtnRows, ref  RtnRowsUL, intVaueRows);

                    }
                }
                if (RtnRows != 0)
                {
                    strDatable.Append("</ul>");
                    RtnRowsUL--;
                }


            }
        }

        /// <summary>
        /// 常用功能☆
        /// </summary>
        /// <returns></returns>
        public string getMenuOften(long lngUserID, int rows)
        {
            StringBuilder strTB = new StringBuilder();
            strTB.Append("<table width='100%' border='0' cellpadding='0'cellspacing='0'>");
            strTB.Append("<tr style='height:100%'>");

            DataTable dt = httpAjaxSC.getPageOftenPage(lngUserID);
            string strApplication = Epower.ITSM.Base.Constant.ApplicationPath;

            if (dt.Rows.Count > 0)
            {
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    if (CheckRight(long.Parse(dr["resourceKey"].ToString())))
                    {
                        if (i == 0)
                        {
                            strTB.Append("<td align=\"left\">");
                        }
                        i++;
                        //strTB.Append("<li><input type=\"button\" id=\"" + i.ToString() + dr["pageName"].ToString() + "\" onclick=\"E8MenuConfigGoToUrl('" + dr["pageUrl"] + "')\" value=\"" + dr["pageName"].ToString().Replace("&nbsp;","") + "\" /></li>");
                        strTB.Append("<li><a  href=\"#\" id=\"" + i.ToString() + dr["pageName"].ToString() + "\" class=\"td_3\" onclick=\"E8MenuConfigGoToUrl('" + strApplication + dr["pageUrl"] + "')\">" + dr["pageName"].ToString().Replace("&nbsp;", "") + "</a></li>");
                        if (i == rows)
                        {
                            i = 0;
                            strTB.Append("</td>");
                        }
                    }
                }
                if (i > 0 && i < rows)
                {
                    strTB.Append("</td>");
                }
            }
            strTB.Append("</tr>");
            strTB.Append("</table>");
            return strTB.ToString();

        }
        #endregion
    }
}
