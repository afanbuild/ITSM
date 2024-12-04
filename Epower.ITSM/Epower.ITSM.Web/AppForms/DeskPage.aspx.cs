using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Epower.DevBase.Organization.SqlDAL;
using System.Collections;

namespace Epower.ITSM.Web.AppForms
{
    public partial class DeskPage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.TableVisible = false;

            UserId.Value = Session["UserID"].ToString();
            bool isManager = CheckRight(Epower.ITSM.Base.Constant.AllFunctionSet);
            if (isManager == true)
            {
                Manager.Value = "true";
            }
            else
            {
                Manager.Value = "false";
            }

            if (!IsPostBack)
            {
                Epower.ITSM.SqlDAL.UIMethod ui = new Epower.ITSM.SqlDAL.UIMethod();

                int rows = int.Parse(Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("PageRows", "PageRow").ToString());
                DeskPageId.InnerHtml = ui.getMenuHtmlDeskPage(Session["UserName"].ToString(), (long)Session["UserID"], rows);

            }
        }

        /// <summary>
        /// 检查权限
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
    }
}
