/*******************************************************************
•	 * 版权所有：深圳市非凡信息技术有限公司
•	 * 描述：设置进度条
•	
•	 * 
•	 * 
•	 * 创建人：余向前
•	 * 创建日期：2013-04-25
 * *****************************************************************/

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Epower.ITSM.Base;
using EpowerCom;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;
using System.IO;

namespace Epower.ITSM.Web.Forms
{
    public partial class frm_BR_ProgressBar_Set : BasePage
    {
        #region SetParentButtonEvent
        /// <summary>
        /// 设置父窗体按钮事件
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.ProgressBar;
            this.Master.IsCheckRight = true;

            if (this.Request.QueryString["id"] != null)
            {
                this.Master.MainID = this.Request.QueryString["id"].ToString();
            }

            this.Master.Master_Button_Save_Click += new Global_BtnClick(Master_Master_Button_Save_Click);
            this.Master.ShowSaveButton(true);
        }

        void Master_Master_Button_Save_Click()
        {
            long lngAppID = long.Parse(cboApp.SelectedValue.ToString() == "" ? "0" : cboApp.SelectedValue.ToString());
            long lngOFlowModelID = long.Parse(cboFlowModel.SelectedValue.ToString() == "" ? "0" : cboFlowModel.SelectedValue.ToString());

            if (lngOFlowModelID <= 0)
            {
                PageTool.MsgBox(Page, "请选择流程模型!");
                this.Master.IsSaveSuccess = false;
                return;
            }

            string str = "";

            DataTable dt = GetDetailItem(true, 0, ref str);

            //删除此流程模型下所有配置进度信息
            BR_ProgressBarDP.DeleteAll(lngAppID, lngOFlowModelID);

            //循环插入新的配置信息
            foreach (DataRow dr in dt.Rows)
            {
                BR_ProgressBarDP ee = new BR_ProgressBarDP();
                ee.AppID = lngAppID;
                ee.OFlowModelID = lngOFlowModelID;
                ee.NodeModelID = CTools.ToInt64(dr["NodeModelID"].ToString());
                ee.NodeName = dr["NodeName"].ToString();
                ee.ImgURL = dr["ImgURL"].ToString();
                
                int IsChangeImg = CTools.ToInt(dr["IsChangeImg"].ToString());
                //判断是否更改了图片
                if (IsChangeImg == 1)
                { 
                    //如果更改需要重新上传图片,并且更新图片地址

                    //从base64格式字符串中得到更新的图片二进制数组数据                    
                    byte[] UpFile = Convert.FromBase64String(dr["UpFile"].ToString());
                    string strFileName = dr["FileName"].ToString();
                    //上传图片返回 图片地址
                    ee.ImgURL = SaveImg(UpFile, strFileName, ee.AppID, ee.OFlowModelID, ee.NodeModelID);
                }

                ee.InsertRecorded(ee);
            }

            BindData();
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            //设置主页面
            SetParentButtonEvent();

            if (!IsPostBack)
            { 
                //绑定下拉框
                ddlAppBind();
            }
        }

        #region 绑定Datagrid
        /// <summary>
        /// 绑定Datagrid
        /// </summary>
        private void BindData()
        {
            DataTable dt = new DataTable();

            long lngAppID = long.Parse(cboApp.SelectedValue.ToString() == "" ? "0" : cboApp.SelectedValue.ToString());
            long lngOFlowModelID = long.Parse(cboFlowModel.SelectedValue.ToString() == "" ? "0" : cboFlowModel.SelectedValue.ToString());

            dt = BR_ProgressBarDP.GetDataTable(lngAppID, lngOFlowModelID);

            dgCondition.DataSource = dt.DefaultView;
            dgCondition.DataBind();
        }
        #endregion

        #region 绑定应用下拉框
        /// <summary>
        /// 绑定应用下拉框
        /// </summary>
        private void ddlAppBind()
        {
            cboApp.DataSource = epApp.GetAllApps().DefaultView;
            cboApp.DataTextField = "AppName";
            cboApp.DataValueField = "AppID";
            cboApp.DataBind();

            cboApp.Items.Remove(new ListItem("通用流程", "199"));
            cboApp.Items.Remove(new ListItem("进出操作间", "1027"));

            ListItem itm = new ListItem("", "-1");
            cboApp.Items.Insert(0, itm);
            cboApp.SelectedIndex = 0;

            ListItem itmModel = new ListItem("", "-1");
            cboFlowModel.Items.Insert(0, itmModel);
            cboFlowModel.SelectedIndex = 0;
        }
        #endregion

        #region  绑定流程模板
        /// <summary>
        /// 绑定流程模板
        /// </summary>
        private void BindOFlowModel()
        {
            string stWhere = cboApp.SelectedItem.Value == "-1" ? "" : " and AppID=" + cboApp.SelectedItem.Value;
            stWhere = stWhere + " and status=1 and deleted=0 ";
            cboFlowModel.DataSource = MailAndMessageRuleDP.getAllFlowModel(stWhere).DefaultView;
            cboFlowModel.DataTextField = "flowname";
            cboFlowModel.DataValueField = "oflowmodelid";
            cboFlowModel.DataBind();

            ListItem itmModel = new ListItem("", "-1");
            cboFlowModel.Items.Insert(0, itmModel);
            cboFlowModel.SelectedIndex = 0;
        }
        #endregion

        #region 应用名称下拉框改变时执行
        /// <summary>
        /// 应用名称下拉框改变时执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cboApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindOFlowModel();           
        }
        #endregion

        #region cboFlowModel 流程模型下拉框改变事件
        /// <summary>
        /// cboFlowModel 流程模型下拉框改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cboFlowModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }
        #endregion

        #region dgCondition_ItemDataBound
        /// <summary>
        /// 规则设置，绑定数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgCondition_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            MailAndMessageRuleDP mRulDP = new MailAndMessageRuleDP();

            if (e.Item.ItemType == ListItemType.Footer)
            {
                //取环节名称
                long lngAppID = long.Parse(cboApp.SelectedValue.ToString());
                long lngOFlowModeID = long.Parse(cboFlowModel.SelectedValue.ToString());
                DropDownList dlNodeName = (DropDownList)e.Item.FindControl("drpAddNodeName");

                DataTable dt = mRulDP.GetNodeName(lngAppID, lngOFlowModeID); //得到所对应流程的所有环节
                DataView dv = dt.DefaultView;

                dlNodeName.DataSource = dv;
                dlNodeName.DataTextField = "nodename";
                dlNodeName.DataValueField = "NodeModelID";
                dlNodeName.DataBind();
                dlNodeName.Items.Insert(0, new ListItem("", ""));//add
                //dlNodeName.Items.Insert(1, new ListItem("所有环节", "-1"));

                HtmlInputHidden hdNodeID = (HtmlInputHidden)e.Item.FindControl("HidAddNodeModelID");
                dlNodeName.SelectedIndex = dlNodeName.Items.IndexOf(dlNodeName.Items.FindByValue(hdNodeID.Value));
                
            }
            else if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //取环节名称
                long lngAppID = long.Parse(cboApp.SelectedValue.ToString());
                long lngOFlowModeID = long.Parse(cboFlowModel.SelectedValue.ToString());
                DropDownList dlNodeName = (DropDownList)e.Item.FindControl("drpNodeName");

                DataTable dt = mRulDP.GetNodeName(lngAppID, lngOFlowModeID); //得到所对应流程的所有环节
                DataView dv = dt.DefaultView;

                dlNodeName.DataSource = dv;
                dlNodeName.DataTextField = "nodename";
                dlNodeName.DataValueField = "NodeModelID";
                dlNodeName.DataBind();
                dlNodeName.Items.Insert(0, new ListItem("", ""));//add
                //dlNodeName.Items.Insert(1, new ListItem("所有环节", "-1"));

                HtmlInputHidden hdNodeID = (HtmlInputHidden)e.Item.FindControl("hidNodeModelID");
                dlNodeName.SelectedIndex = dlNodeName.Items.IndexOf(dlNodeName.Items.FindByValue(hdNodeID.Value));               
            }
        }
        #endregion

        #region dgCondition_ItemCommand
        /// <summary>
        /// 规则设置
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgCondition_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            DataTable dt = new DataTable();
            if (e.CommandName == "Delete")
            {                
                string hidId = "";

                dt = GetDetailItem(true, e.Item.ItemIndex, ref hidId);
                dt.Rows.RemoveAt(e.Item.ItemIndex);
                dgCondition.DataSource = dt.DefaultView;
                dgCondition.DataBind();
            }
            else if (e.CommandName == "Add")
            {                
                string hidid = "";

                dt = GetDetailItem(false, e.Item.ItemIndex, ref hidid);

                dgCondition.DataSource = dt.DefaultView;
                dgCondition.DataBind();

            }
        }
        #endregion

        #region 创建DataTable结构
        /// <summary>
        /// 创建 datatable结构
        /// </summary>
        /// <returns></returns>
        private DataTable CreateNullTable()
        {
            DataTable dt = new DataTable("ProgressSet");


            dt.Columns.Add("ID");
            dt.Columns.Add("AppID");
            dt.Columns.Add("OFlowModelID");
            dt.Columns.Add("NodeModelID");
            dt.Columns.Add("NodeName");
            dt.Columns.Add("ImgURL");
            dt.Columns.Add("UpFile");       //保存上传图片的byte[]数组
            dt.Columns.Add("FileName");     //图片名称
            dt.Columns.Add("IsChangeImg");  //是否更新图片
            return dt;
        }
        #endregion

        #region 得到DataGrid里设置的规则信息
        /// <summary>
        /// 得到DataGrid里设置的规则信息
        /// </summary>
        /// <param name="isAll"></param>
        /// <param name="indexs"></param>
        /// <param name="strHidAddValue"></param>
        /// <returns></returns>
        private DataTable GetDetailItem(bool isAll, int indexs, ref string strHidAddValue)
        {            
            DataTable dt = CreateNullTable();

            DataRow dr;  //数据行
            int id = 0;

            #region  构建DataTable

            foreach (DataGridItem row in dgCondition.Controls[0].Controls)
            {
                id++;
                if (row.ItemType == ListItemType.Footer)
                {

                    long lngAppID = long.Parse(cboApp.SelectedValue.ToString() == "" ? "0" : cboApp.SelectedValue.ToString());
                    long lngOFlowModelID = long.Parse(cboFlowModel.SelectedValue.ToString() == "" ? "0" : cboFlowModel.SelectedValue.ToString());

                    DropDownList drnode = (DropDownList)row.FindControl("drpAddNodeName");                    

                    long lngNodeModelID = long.Parse(drnode.SelectedItem.Value == "" ? "0" : drnode.SelectedItem.Value);
                    string strNodeName = drnode.SelectedItem.Text;

                    //图片地址
                    HttpPostedFile file = ((FileUpload)row.FindControl("fileLinedImageFoot")).PostedFile;
                    string strImgURL = "已更新图片";
                    int IsChangeImg = 0;
                    byte[] byFile = new byte[2000];
                    string UpFile = "";
                    string strFileName = "";

                    if (file != null && file.FileName != "")
                    {
                        IsChangeImg = 1;
                        strFileName = file.FileName;                        
                        byFile = CTools.ReadFileBuffer(file);  //将图片转成byte[]数组                      
                        UpFile = Convert.ToBase64String(byFile); //将byte[]数组转成base64格式字符串                        
                    }                    

                    if (row.ItemIndex == indexs)
                    {
                        strHidAddValue = lngNodeModelID.ToString();
                    }

                    if (lngOFlowModelID != 0 && lngNodeModelID != 0)
                    {
                        dr = dt.NewRow();
                        dr["ID"] = 0;
                        dr["AppID"] = lngAppID;
                        dr["OFlowModelID"] = lngOFlowModelID;
                        dr["NodeModelID"] = lngNodeModelID;
                        dr["NodeName"] = strNodeName;
                        dr["ImgURL"] = strImgURL;
                        dr["UpFile"] = UpFile;                        
                        dr["FileName"] = strFileName;
                        dr["IsChangeImg"] = IsChangeImg;
                        dt.Rows.Add(dr);

                    }
                    else
                    {
                        if (!isAll)
                        {                            
                            PageTool.MsgBox(this, "请选择环节信息!");
                        }
                    }
                }
                else if (row.ItemType == ListItemType.Item || row.ItemType == ListItemType.AlternatingItem)
                {

                    long lngAppID = long.Parse(cboApp.SelectedValue.ToString() == "" ? "0" : cboApp.SelectedValue.ToString());
                    long lngOFlowModelID = long.Parse(cboFlowModel.SelectedValue.ToString() == "" ? "0" : cboFlowModel.SelectedValue.ToString());

                    DropDownList drnode = (DropDownList)row.FindControl("drpNodeName");

                    long lngNodeModelID = long.Parse(drnode.SelectedItem.Value == "" ? "0" : drnode.SelectedItem.Value);
                    string strNodeName = drnode.SelectedItem.Text;

                    //图片地址
                    HtmlInputHidden lblImgURL = (HtmlInputHidden)row.FindControl("hidiamgeName");
                    string strImgURL = lblImgURL.Value;
                    HttpPostedFile file = ((FileUpload)row.FindControl("fileLinedImage")).PostedFile;

                    int IsChangeImg = CTools.ToInt(((HtmlInputHidden)row.FindControl("hidIsChangeImg")).Value);
                    string strFileName = ((HtmlInputHidden)row.FindControl("hidFileName")).Value;                    
                    string UpFile = ((HtmlInputHidden)row.FindControl("hidUpFile")).Value;
                    byte[] byFile = new byte[2000];

                    if (file != null && file.FileName != "")
                    {
                        strImgURL = "已更新图片";
                        IsChangeImg = 1;
                        strFileName = file.FileName;
                        byFile = CTools.ReadFileBuffer(file);  //将图片转成byte[]数组                      
                        UpFile = Convert.ToBase64String(byFile); //将byte[]数组转成base64格式字符串                        
                    }                      

                    if (row.ItemIndex == indexs)
                    {
                        strHidAddValue = lngNodeModelID.ToString();
                    }

                    if (lngOFlowModelID != 0 && lngNodeModelID != 0)
                    {
                        dr = dt.NewRow();
                        dr["ID"] = 0;
                        dr["AppID"] = lngAppID;
                        dr["OFlowModelID"] = lngOFlowModelID;
                        dr["NodeModelID"] = lngNodeModelID;
                        dr["NodeName"] = strNodeName;
                        dr["ImgURL"] = strImgURL;
                        dr["UpFile"] = UpFile;
                        dr["FileName"] = strFileName;
                        dr["IsChangeImg"] = IsChangeImg;
                        dt.Rows.Add(dr);
                    }
                }
            }

            #endregion


            return dt;
        }
        #endregion

        #region 保存图片信息
        /// <summary>
        /// 保存图片信息
        /// </summary>
        /// <param name="fs">图片二进制数组数据</param>
        /// <param name="strFileName">上传图片完整名称</param>
        /// <param name="lngAppID">应用ID</param>
        /// <param name="lngOFlowModelID">流程模型ID</param>
        /// <param name="lngNodeModelID">环节模型ID</param>
        /// <returns></returns>
        private string SaveImg(byte[] fs, string strFileName, long lngAppID, long lngOFlowModelID, long lngNodeModelID)
        {
            string path = System.Web.HttpContext.Current.Request.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath);
            string fileExt = strFileName.Substring(strFileName.LastIndexOf('.') + 1).ToLower();
            if (path.EndsWith(@"\") == false)
            {
                path = path + "\\upimg\\ProgressImg\\";
            }
            else
            {
                path = path + "upimg\\ProgressImg\\";
            }
            //判断目录是否存在
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string actpath = path + lngAppID + lngOFlowModelID + lngNodeModelID + "_act." + fileExt; //拼接图片保存地址
            //判断是否已经存在此文件
            if (File.Exists(actpath))
            {
                File.Delete(actpath);
            }
            //保存图片
            SaveImg(fs, actpath);

            string returnURL = "../upimg/ProgressImg/" + lngAppID + lngOFlowModelID + lngNodeModelID + "_act." + fileExt; //拼接返回地址
            //返回最终图片地址
            return returnURL;

        }

        /// <summary>
        /// 保存实际图
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="actpath"></param>
        /// <returns></returns>
        private bool SaveImg(byte[] fs, string actpath)
        {
            bool result = false;
            string errmsg = "";
            FileStream fileStream = new FileStream(actpath, FileMode.Create);
            try
            {
                fileStream.Write(fs, 0, fs.Length);
                result = true;
            }
            catch (Exception ex)
            {
                errmsg = "进度条保存图片失败." + ex.Message;
                E8Logger.Debug(errmsg);
                return false;
            }
            finally
            {
                fileStream.Close();
                fileStream.Dispose();
            }
            return result;
        }
        #endregion

        #region 得到展示信息
        /// <summary>
        /// 得到展示信息
        /// </summary>
        /// <param name="intIsChange">是否改变图片</param>
        /// <returns></returns>
        protected string GetShow(int intIsChangeImg)
        {
            string str = "查看图片";
            if (intIsChangeImg == 1)
                str = "已更新图片";
            return str;
        }
        #endregion
    }
}
