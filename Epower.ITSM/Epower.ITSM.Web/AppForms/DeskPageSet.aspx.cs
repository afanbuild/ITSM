using System;
using System.Collections;
using System.Configuration;
using System.Data;

using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.AppForms
{
    public partial class DeskPageSet : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.TableVisible = false;
            if (!IsPostBack)
            {
                PageName.Text = Request["PageName"].ToString();
                string strPageUrl = Request["PageUrl"].ToString();
                string strManager = Request["Manager"].ToString();
                string resourceKey = Request["ResourceKey"].ToString();
                string strVlues = Request["Power"].ToString();

                if (strVlues == "NO")
                {
                    //什么都没设置的情况
                    if (strManager == "false")
                    {
                        //非管理员的操作
                        btnPageAll.Visible = false;
                        btnConent.Visible = false;
                        //menu3[0]["设置为全局常用功能"].disabled = true;
                        //menu3[2]["取消常用功能设置"].disabled = true;
                    }
                    else
                    {
                        btnConent.Visible = false;
                        //menu3[2]["取消常用功能设置"].disabled = true;
                    }

                }
                else if (strVlues == "AllPage")
                {
                    //当已经设置了全局常用功能的情况
                    if (strManager == "true")
                    {
                        //是管理员的时候
                        btnPageAll.Visible = false;
                        btnOftenPage.Visible = false;
                        //menu3[0]["设置为全局常用功能"].disabled = true;
                        //menu3[1]["设置为常用功能"].disabled = true;
                    }
                    else
                    {
                        btnPageAll.Visible = false;
                        btnOftenPage.Visible = false;
                        btnConent.Visible = false;
                        //menu3[0]["设置为全局常用功能"].disabled = true;
                        //menu3[1]["设置为常用功能"].disabled = true;
                        //menu3[2]["取消常用功能设置"].disabled = true;
                    }
                }
                else if (strVlues == "OftenPage")
                {
                    //当已经设置了个人常用功能的情况
                    if (strManager == "true")
                    {
                        btnOftenPage.Visible = false;
                        //  menu3[1]["设置为常用功能"].disabled = true;
                    }
                    else
                    {
                        btnPageAll.Visible = false;
                        btnOftenPage.Visible = false;
                        //menu3[0]["设置为全局常用功能"].disabled = true;
                        //menu3[1]["设置为常用功能"].disabled = true;
                    }
                }
                btnPageAll.Attributes.Add("onclick", "return confirm('您确认设置全局常用功能')");
                btnOftenPage.Attributes.Add("onclick", "return confirm('您确认设置为常用功能')");
                btnConent.Attributes.Add("onclick", "return confirm('您确认取消常用功能设置')");
            }
     
        }

        protected void btnPageAll_Click(object sender, EventArgs e)
        {
            string PageName = Request["PageName"].ToString();
            string strPageUrl = Request["PageUrl"].ToString();
      
            string resourceKey = Request["ResourceKey"].ToString();
            string strApplication = Epower.ITSM.Base.Constant.ApplicationPath;

            strPageUrl = strPageUrl.Replace(strApplication,string.Empty);

            if (httpAjaxSC.insertPageAllType(PageName, strPageUrl, "1", Session["UserID"].ToString(), resourceKey))
            {
                Response.Write("<script type='text/javascript'>alert('设置成功');window.opener.location.reload();;window.close();</script>");
            }
            else
            {
                PageTool.MsgBox(this.Page, "设置失败");
            }
        }

        protected void btnOftenPage_Click(object sender, EventArgs e)
        {
            string PageName = Request["PageName"].ToString();
            string strPageUrl = Request["PageUrl"].ToString();
            string resourceKey = Request["ResourceKey"].ToString();

            string strApplication = Epower.ITSM.Base.Constant.ApplicationPath;
            strPageUrl = strPageUrl.Replace(strApplication, string.Empty);

            if (httpAjaxSC.insertPageAllType(PageName, strPageUrl, "0", Session["UserID"].ToString(), resourceKey))
            {
                Response.Write("<script type='text/javascript'>alert('设置成功');window.opener.location.reload();;window.close();</script>");
            }
            else
            {
                PageTool.MsgBox(this.Page, "设置失败");
            }
        }

        protected void btnConent_Click(object sender, EventArgs e)
        {
            string PageName = Request["PageName"].ToString();
            string strPageUrl = Request["PageUrl"].ToString();
            string strManager = Request["Manager"].ToString();            
            if (httpAjaxSC.deletePageAllType(PageName, strPageUrl, strManager, Session["UserID"].ToString()))
            {
                Response.Write("<script type='text/javascript'>alert('成功取消设置');window.opener.location.reload();;window.close();</script>");
            }
            else
            {
                PageTool.MsgBox(this.Page, "设置失败");
            }
        }
    }
}
