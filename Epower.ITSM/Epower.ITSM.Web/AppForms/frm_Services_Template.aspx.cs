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

using EpowerCom;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using System.IO;

namespace Epower.ITSM.Web.AppForms
{
    public partial class frm_Services_Template : BasePage
    {
        RightEntity re = null;

        /// <summary>
        /// 
        /// </summary>
        protected string TemplateID
        {
            get
            {
                return this.Master.MainID.ToString();
            }
        }

        /// <summary>
        /// 设置父窗体按钮事件
        /// </summary>
        protected void SetParentButtonEvent()
        {

            this.Master.OperatorID = Constant.serveDefine;
            this.Master.IsCheckRight = true;
            if (this.Request.QueryString["id"] != null)
            {
                this.Master.MainID = this.Request.QueryString["id"].ToString();
            }
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);
            this.Master.Master_Button_Save_Click += new Global_BtnClick(Master_Master_Button_Save_Click);
            Epower.DevBase.Organization.SqlDAL.RightEntity re = (Epower.DevBase.Organization.SqlDAL.RightEntity)((Hashtable)Session["UserAllRights"])[Constant.serveDefine];

            #region 增加、删除、修改权限
            if (re.CanDelete != true)
            {
                this.cmdDelete.Visible = false;
            }
            if (re.CanModify != true)
            {
                this.cmdSave.Visible = false;
            }
            if (!re.CanAdd)
            {
                this.cmdAdd.Visible = true;
            }
            if (re.CanAdd == true && re.CanModify != true && string.IsNullOrEmpty(this.Master.MainID.ToString()))
            {
                this.cmdSave.Visible = true;
            }
            #endregion
        }

        /// <summary>
        /// 删除
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {

            try
            {
                if (!String.IsNullOrEmpty(this.Master.MainID.ToString()))
                {
                    EA_ServicesTemplateDP ee = new EA_ServicesTemplateDP();
                    ee.DeleteRecorded(long.Parse(this.Master.MainID.Trim()));
                    //返回主页面
                    //  Master_Master_Button_GoHistory_Click();
                }

                //强制分类相关缓存失效 
                HttpRuntime.Cache.Insert("CommCacheValidEquCategory", false);

                //Session["OldEQSubectID"] = strSubjectID;

                //如过从查询分类界面打开，保存后关闭并刷新父窗体
                if (Request["CloseAction"] != null && Request["CloseAction"].ToString().Equals("1"))
                    PageTool.AddJavaScript(this, "window.parent.returnValue = 'refresh';self.close();");
                else
                    PageTool.AddJavaScript(this, "window.parent.contents.location='frmEqu_Content.aspx?type=0&CurrSubjectID=1';window.location='about:blank'");
            }
            catch (Exception ee)
            {
                PageTool.MsgBox(this, "删除分类时出现错误，错误为：" + ee.Message.ToString());
            }



        }

        /// <summary>
        /// 保存
        /// </summary>
        void Master_Master_Button_Save_Click()
        {
            string serversname = this.CtrFTTemplateName.Value.ToString();
            if (serversname == "")
            {
                PageTool.MsgBox(this, "服务名称不能为空,请输入!");
                this.Master.IsSaveSuccess = false;
                return;
            }

            if (ddlApp.SelectedIndex == 0
                || ddlFlowModel.SelectedIndex == 0)
            {
                PageTool.MsgBox(this, "必须选择所属应用和事件模板!");
                this.Master.IsSaveSuccess = false;
                return;
            }

            #region 获取图片logo信息 yxq 2011-09-06

            int MaxFileSize = 4;
            if (CommonDP.GetConfigValue("TempCataLog", "MaxFileSize") != null)
            {
                MaxFileSize = int.Parse(CommonDP.GetConfigValue("TempCataLog", "MaxFileSize"));
            }

            #endregion

            HttpPostedFile file = browsepic1.File.PostedFile;

            if (file != null && file.ContentLength > MaxFileSize * 1024 * 1024)
            {
                PageTool.MsgBox(this, "上传文件大于" + MaxFileSize.ToString() + "M，不能上传文件！");
                this.Master.IsSaveSuccess = false;
                return;
            }

            if (string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                EA_ServicesTemplateDP ee = new EA_ServicesTemplateDP();
                InitObject(ee);
                //   ee.ServiceLevelID = decimal.Parse(this.Request.QueryString["ServiceLevelID"]);
                ee.InsertRecorded(ee);
                this.Master.MainID = ee.TemplateID.ToString().Trim();
            }
            else
            {
                EA_ServicesTemplateDP ee = new EA_ServicesTemplateDP();
                ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                InitObject(ee);
                ee.UpdateRecorded(ee);

                #region 更新所有子服务目录的相关父类名称
                //  EA_ServicesTemplateDP.UpdateChrildServiceLevel(long.Parse(ee.TemplateID.ToString()));
                #endregion

                this.Master.MainID = ee.TemplateID.ToString();
            }
            //强制分类相关缓存失效 
            HttpRuntime.Cache.Insert("CommCacheValidEquCategory", false);

            //Session["OldEQSubectID"] = strSubjectID;

            //如过从查询分类界面打开，保存后关闭并刷新父窗体
            if (Request["CloseAction"] != null && Request["CloseAction"].ToString().Equals("1"))
                PageTool.AddJavaScript(this, "window.parent.returnValue = 'refresh';self.close();");
            else
                PageTool.AddJavaScript(this, "window.parent.contents.location='frmEqu_Content.aspx?type=0&CurrSubjectID=1';window.location='about:blank'");
        }

        /// <summary>
        /// 返回
        /// </summary>
        void Master_Master_Button_GoHistory_Click()
        {
            Response.Redirect("frm_Services_TemplateMain.aspx");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //设置主页面
            SetParentButtonEvent();
            if (!IsPostBack)
            {
                //LoadFlowModels();
                LoadDrList();
                LoadData();
            }

            if (txtPCatalogName.Text == "")
                txtPCatalogName.Text = hidPCatalogName.Value;
        }

        /// <summary>
        /// 绑定一级服务目录下拉框
        /// </summary>
        private void LoadDrList()
        {
            drParentList.Items.Clear();
            EA_ServicesTemplateDP ee = new EA_ServicesTemplateDP();
            string sWhere = " And IsParent=1 ";

            if (!string.IsNullOrEmpty(this.Master.MainID.Trim()))
                sWhere += " And TemplateID!=" + this.Master.MainID.Trim();

            DataTable dt = ee.GetDataTable(sWhere, "");
            drParentList.DataSource = dt.DefaultView;
            drParentList.DataTextField = "TemplateName";
            drParentList.DataValueField = "TemplateID";
            drParentList.DataBind();
            drParentList.Items.Insert(0, new ListItem("", "0"));

        }
        /// <summary>
        /// 获取事件模板
        /// </summary>
        private void LoadFlowModels()
        {
            long lngAppID = CTools.ToInt64(ddlApp.SelectedItem.Value);

            ddlFlowModel.Items.Clear();
            EA_ShortCutTemplateDP ee = new EA_ShortCutTemplateDP();
            string sWhere = " And AppID=" + lngAppID + " And owner=3 AND TemplateType = " + ((int)e_ITSMShortCutReqType.eitsmscrtIssue).ToString();
            DataTable dt = ee.GetDataTable(sWhere, " order by templatename asc");
            ddlFlowModel.DataSource = dt.DefaultView;
            ddlFlowModel.DataTextField = "TemplateName";
            ddlFlowModel.DataValueField = "TemplateID";
            ddlFlowModel.DataBind();
            ddlFlowModel.Items.Insert(0, new ListItem("", ""));//插入空白

        }

        private void LoadData()
        {
            ////if (!string.IsNullOrEmpty(this.Master.MainID.Trim()))
            ////{

            if (this.Request.QueryString["subjectid"] != "0" && this.Request.QueryString["ServiceLevelID"] == null)
            {
                this.Master.MainID = this.Request.QueryString["subjectid"].ToString();
                // 记录当前分类
                Session["OldTemplateID"] = long.Parse(this.Master.MainID);

                hidCatalogID1.Value = this.Master.MainID;

                EA_ServicesTemplateDP ee = new EA_ServicesTemplateDP();
                if (this.Master.MainID == "1")
                {
                    this.Master.MainID = "-1";
                }
                ee = ee.GetReCorded(long.Parse(this.Master.MainID));
                drParentList.SelectedIndex = drParentList.Items.IndexOf(drParentList.Items.FindByValue(ee.ServiceLevelID.ToString()));
                CtrFTTemplateName.Value = ee.TemplateName;
                ddlApp.SelectedIndex = ddlApp.Items.IndexOf(ddlApp.Items.FindByValue(ee.AppID.ToString()));
                //加载事件模板
                LoadFlowModels();
                ddlFlowModel.SelectedIndex = ddlFlowModel.Items.IndexOf(ddlFlowModel.Items.FindByValue(ee.IssTempID.ToString()));
                txtContent.Text = ee.Content;
                txtGuide.Text = ee.Guide;
                hfGuide.Value = ee.Guide;
                this.hidPCatalogID.Value = ee.ServiceLevelID.ToString();
                this.hidPCatalogID1.Value = ee.TemplateID.ToString();

                this.txtPCatalogName.Text = ee.ServiceLevel;
                //  radioParent.SelectedIndex = radioParent.Items.IndexOf(radioParent.Items.FindByValue(ee.IsParent.ToString()));

                //判断是否是一级目录 且是否有子类
                string sWhere = " And ServiceLevelID=" + ee.TemplateID;
                DataTable dt = ee.GetDataTable(sWhere, "");
                if (dt != null && dt.Rows.Count > 0)
                    hfRadio.Value = "1";

                #region 给附件控件赋值
                ctrattachment1.AttachmentType = eOA_AttachmentType.eZZ;
                ctrattachment1.FlowID = (long)ee.TemplateID;
                #endregion
                //}  
            }
            else
            {
                this.hidPCatalogID.Value = this.Request.QueryString["ServiceLevelID"].ToString();
                this.txtPCatalogName.Text = this.Request.QueryString["ServiceLevelName"].ToString();
            }
        }





        private void InitObject(EA_ServicesTemplateDP ee)
        {

            ee.TemplateType = (int)e_ITSMShortCutReqType.eitsmscrtIssue;   //这里固定是事件的快速请求
            ee.TemplateName = CtrFTTemplateName.Value.Trim();
            ee.ServiceKindID = 0;
            ee.ServiceKind = "";

            if (ddlApp.SelectedItem.Value != "")
                ee.AppID = CTools.ToInt64(ddlApp.SelectedItem.Value);
            else
                ee.AppID = 0;

            if (ddlFlowModel.SelectedItem.Text != "")
            {
                ee.IssTempID = decimal.Parse(ddlFlowModel.SelectedItem.Value);
                ee.IssTempName = ddlFlowModel.SelectedItem.Text;
                EA_ShortCutTemplateDP ect = new EA_ShortCutTemplateDP();
                ect = ect.GetReCorded(long.Parse(ddlFlowModel.SelectedItem.Value));
                ee.OFlowModelID = ect.OFlowModelID;
            }
            else
            {
                ee.IssTempID = 0;
                ee.IssTempName = "";
                ee.OFlowModelID = 0;
            }

            if (this.txtPCatalogName.Text != "" && this.hidPCatalogID.Value != "")
            {
                ee.ServiceLevelID = decimal.Parse(this.hidPCatalogID.Value);
                ee.ServiceLevel = this.txtPCatalogName.Text;
            }
            else
            {
                ee.ServiceLevelID = 0;
                ee.ServiceLevel = "";
            }

            ee.IsParent = 0;

            if (ee.IsParent == 1)
            {
                ee.ServiceLevelID = 0;
                ee.ServiceLevel = "";
            }
            ee.Content = txtContent.Text;
            ee.Guide = txtGuide.Text.Trim();

            int result = 0; //是否生成缩略图

            #region 保存图片
            HttpPostedFile file = browsepic1.File.PostedFile;
            if (file != null && file.FileName.Trim() != "")
            {
                string path = System.Web.HttpContext.Current.Request.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath);
                string fileExt = file.FileName.Substring(file.FileName.LastIndexOf('.') + 1).ToLower();
                if (path.EndsWith(@"\") == false)
                {
                    path = path + "\\upimg\\catalogimg\\EA_Services\\";
                }
                else
                {
                    path = path + "upimg\\catalogimg\\EA_Services\\";
                }
                //判断目录是否存在
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string time = DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + "" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second + "" + DateTime.Now.Millisecond + "";
                string actpath = path + time + "_act." + fileExt;
                string prepath = path + time + "_pre." + fileExt;
                ee.imglogo = "../upimg/catalogimg/EA_Services/" + time + "_pre." + fileExt;
                //判断文件是否存在
                if (File.Exists(actpath))
                    File.Delete(actpath);
                if (File.Exists(prepath))
                    File.Delete(prepath);

                //保存实际图片
                file.SaveAs(actpath);

                System.Threading.Thread.Sleep(500); //让线程暂停一会等待前面大图的保存

                //生成系统固定缩略图
                result = MClass.GetSmallImg(actpath, prepath, fileExt, 70, 70);

                if (result == 1)
                    ee.imglogo = "../upimg/catalogimg/EA_Services/" + time + "_act." + fileExt;

            }
            #endregion

            #region 保存附件XML
            ee.AttachXml = ctrattachment1.AttachXML;
            #endregion
        }

        protected void cmdSave_Click(object sender, System.EventArgs e)
        {
            Master_Master_Button_Save_Click();
        }

        protected void cmdAdd_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("frm_Services_Template.aspx?subjectid=0&ServiceLevelID=" + this.hidPCatalogID1.Value + "&ServiceLevelName=" + this.CtrFTTemplateName.Value);

        }

        protected void cmdDelete_Click(object sender, System.EventArgs e)
        {
            Master_Master_Button_Delete_Click();
        }

        #region 改变所属应用下拉框的值
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            //重新绑定模板下拉框
            LoadFlowModels();
        }
        #endregion

    }
}