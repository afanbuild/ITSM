/*******************************************************************
 * 版权所有：非凡信息技术
 * Description：用户组批量添加人员成员
 * 
 * 
 * Create By  ：zhumc
 * Create Date：2010-12-22
 * *****************************************************************/
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.Base;

namespace Epower.ITSM.Web.DeptForms
{
    /// <summary>
    /// 
    /// </summary>
    public partial class frmMulAddUser : BasePage
    {
        /// <summary>
        /// 用户组编号
        /// </summary>
        protected string sActorID
        {
            get
            {
                if (Request["actorid"] != null)
                {
                    return Request["actorid"].ToString();
                }
                else
                {
                    return "0";
                }
            }
        }

        /// <summary>
        /// 用户或者部门
        /// </summary>
        protected string sType
        {
            get
            {
                if (Request["type"] != null)
                {
                    return Request["type"].ToString();
                }
                else
                {
                    return "0";
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (sType == "0") //批量添加用户
                {
                    truser.Visible = true;
                    trdept.Visible = false;
                }
                else
                {
                    truser.Visible = false;
                    trdept.Visible = true;
                }
            }
        }

        /// <summary>
        /// 批量保存用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            int iactortype = 20;
            string[] sUserID;
            string[] sUserName;

            if (sType == "0") //批量添加用户
            {
                iactortype = 20;
                if (UserPickerMult1.UserID.Trim() == string.Empty)
                {
                    PageTool.MsgBox(this, "请选择需要添加的用户！");
                    return;
                }
                sUserID = UserPickerMult1.UserID.Split(',');
                sUserName = UserPickerMult1.UserName.Split(',');
            }
            else
            {
                iactortype = 10;
                if (DeptPickerMult1.DeptID.Trim() == string.Empty)
                {
                    PageTool.MsgBox(this, "请选择需要添加的部门！");
                    return;
                }
                sUserID = DeptPickerMult1.DeptID.Split(',');
                sUserName = DeptPickerMult1.DeptName.Split(',');
            }

            
            if (sActorID == "0")
            {
                PageTool.MsgBox(this, "未指定相应用户组,保存失败！");
                return;
            }

            string sCatchName = string.Empty;
            string sRepeatName = string.Empty;
            

            try
            {
                for (int i = 0; i < sUserID.Length-1; i++)
                {
                    ActorMemberEntity ame = new ActorMemberEntity();
                    ame.ActorID = StringTool.String2Long(sActorID);
                    ame.ID = 0;
                    ame.ObjectID = long.Parse(sUserID[i]);
                    ame.ActorType = (eO_ActorObject)iactortype;
                    // 注意只有新增时才保存了rangeid
                    ame.RangeID = Session["RangeID"].ToString();

                    if (ame.ActorType == 0 || ame.ObjectID == 0)   //数据异常
                    {
                        sCatchName += sUserName[i] + ",";
                    }
                    if (ActorMemberEntity.Is_ActorMemeber_Exist(ame.ActorID.ToString(), ((int)ame.ActorType).ToString(), ame.ObjectID.ToString()))
                    {
                        sRepeatName += sUserName[i] + ",";
                    }
                    else
                    {
                        ame.Save();
                    }
                }

                if (sCatchName == string.Empty && sRepeatName == string.Empty)
                {
                    PageTool.MsgBox(this, "批量添加成功！");
                }
                else
                {
                    string sMsg = "批量添加成功！";
                    if (sCatchName != string.Empty)
                    {
                        sMsg += "异常数据有：" + sCatchName + "；";
                    }
                    if (sRepeatName != string.Empty)
                    {
                        sMsg += "已经存在用户组的成员有：" + sRepeatName + "。";
                    }
                    PageTool.MsgBox(this, sMsg);
                }
                PageTool.AddJavaScript(this, "window.opener.location.href=window.opener.location.href;window.close();");
            }
            catch (Exception ex)
            {
                PageTool.MsgBox(this, "保存用户组成员时出现错误,错误为:\n" + ex.Message.ToString());
            }
        }
    }
}
