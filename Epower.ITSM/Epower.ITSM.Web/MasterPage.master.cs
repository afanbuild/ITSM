using System;
using System.Data;
using System.Configuration;
using System.Collections.Specialized;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;


public delegate void Global_BtnClick();

public partial class EpowerMasterPage : System.Web.UI.MasterPage
{
    public event Global_BtnClick Master_Button_New_Click; /*新增事件*/
    public event Global_BtnClick Master_Button_Edit_Click;  /*修改事件*/
    public event Global_BtnClick Master_Button_Save_Click;　/*保存事件*/
    public event Global_BtnClick Master_Button_Delete_Click;   /*删除事件*/
    public event Global_BtnClick Master_Button_Print_Click;   /*打印事件*/
    public event Global_BtnClick Master_Button_Query_Click;   /*查询事件*/
    public event Global_BtnClick Master_Button_Stat_Click;   /*统计事件*/
    public event Global_BtnClick Master_Button_ExportWord_Click;  /*导出Word事件*/
    public event Global_BtnClick Master_Button_ExportExcel_Click;  /*导出Excel事件*/
    public event Global_BtnClick Master_Button_ExportPdf_Click;  /*导出Pdf事件*/
    public event Global_BtnClick Master_Button_GoHistory_Click;   /*返回事件*/


    public event Global_BtnClick Master_Button_NewFinish_Click;　/*新增后事件*/
    public event Global_BtnClick Master_Button_EditFinish_Click;  /*修改后事件*/
    public event Global_BtnClick Master_Button_SaveFinish_Click;　/*保存后事件*/
    public event Global_BtnClick Master_Button_DeleteFinish_Click;   /*删除后事件*/
    public event Global_BtnClick Master_Button_QueryFinish_Click;   /*查询后事件*/

    public string sApplicationUrl = Epower.ITSM.Base.Constant.ApplicationPath;     //虚拟路径

    #region 属性区
    /// <summary>
    /// 记录ID
    /// </summary>
    public string MainID
    {
        get { return this.hidID.Value.Trim(); }
        set { this.hidID.Value = value; }
    }

    /// <summary>
    /// 返回查询输入框
    /// </summary>
    public HtmlInputText TxtKeyName
    {
        get { return this.txtKeyName; }
    }

    /// <summary>
    /// 
    /// </summary>
    public string KeyValue
    {
        get { if (ViewState["FastKeyValue"] != null) return ViewState["FastKeyValue"].ToString(); else return string.Empty; }
        set { ViewState["FastKeyValue"] = value;
        this.txtKeyName.Value = value;
        }
    }

    /// <summary>
    /// 设备按钮表格是否可见
    /// </summary>
    public bool TableVisible
    {
        set
        {
            btlbutton.Visible = value;
            tbtop.Visible = value;
        }
    }

    private bool mIsSaveSuccess = true;
    /// <summary>
    /// 保存是否成功
    /// </summary>
    public bool IsSaveSuccess
    {
        get
        {
            return mIsSaveSuccess;
        }
        set
        {
            mIsSaveSuccess = value;
        }
    }

    private long mOperatorID = 0;
    /// <summary>
    /// 操作项ID
    /// </summary>
    public long OperatorID
    {
        get { return mOperatorID; }
        set { mOperatorID = value; }
    }
    public bool mIsCheck = false;  //默认为不检查权限

    /// <summary>
    /// 
    /// </summary>
    public bool IsCheckRight
    {
        get { return mIsCheck; }
        set { mIsCheck = value; }
    }

    /// <summary>
    /// 默认按钮
    /// </summary>
    public string DefaultButton
    {
        get { return ViewState["DefaultButton"] != null ? ViewState["DefaultButton"].ToString() : "btn_query"; }
        set { ViewState["DefaultButton"] = value; }
    }
    Epower.DevBase.Organization.SqlDAL.RightEntity mRightEntity = null;
    /// <summary>
    /// 母版页上主权限对象
    /// </summary>
    public Epower.DevBase.Organization.SqlDAL.RightEntity RightEntity
    {
        get
        {
            if (OperatorID != 0 && mRightEntity == null)
            {
                mRightEntity = (Epower.DevBase.Organization.SqlDAL.RightEntity)((Hashtable)Session["UserAllRights"])[OperatorID];
            }
            return mRightEntity;
        }
    }

    #region 设置按钮
    /// <summary>
    /// 查询
    /// </summary>
    public Button Btn_query
    {
        get { return this.btn_query; }
    }
    /// <summary>
    /// 新增
    /// </summary>
    public Button Btn_new
    {
        get { return this.btn_new; }
    }
    /// <summary>
    /// 修改
    /// </summary>
    public Button Btn_edit
    {
        get { return this.btn_edit; }
    }
    /// <summary>
    /// 保存
    /// </summary>
    public Button Btn_save
    {
        get { return this.btn_save; }
    }
    /// <summary>
    /// 删除
    /// </summary>
    public Button Btn_delete
    {
        get { return this.btn_delete; }
    }
    /// <summary>
    /// 打印
    /// </summary>
    public Button Btn_print
    {
        get { return this.btn_print; }
    }
    /// <summary>
    /// 导出Excel
    /// </summary>
    public Button Btn_exportExcel
    {
        get { return this.btn_exportExcel; }
    }
    /// <summary>
    /// 导出Excel
    /// </summary>
    public Button Btn_back
    {
        get { return this.btn_back; }
    }
    /// <summary>
    /// 返回主页
    /// </summary>
    public Button Btn_back_home
    {
        get { return this.Btn_back_home; }
    }
    #endregion 
    #endregion

    #region 页面初始化 OnInit
    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
    }
    #endregion

    #region 页面加载 Page_Load
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null) { Response.Redirect("~/default.aspx"); }

        if (!IsPostBack)
        {
            this.txtKeyName.Attributes.Add("onclick", "txtKeyNameClear()");
            this.form1.DefaultButton = DefaultButton;
        }
    }
    #endregion    

    #region 检查母版页的权限 CheckPageRight
    /// <summary>
    /// 0表示查询，1表示新增，2表示修改，3表示删除
    /// </summary>
    /// <param name="iType"></param>
    /// <returns></returns>
    protected bool CheckPageRight(int iType)
    {
        if (OperatorID == 0)   //如果没有设置操作项，默认为有权限
            return true;
        if (!IsCheckRight)    //如果设置了不检查权限，默认为有权限
            return true;
        if (OperatorID != 0 && mRightEntity == null)
        {
            mRightEntity = (Epower.DevBase.Organization.SqlDAL.RightEntity)((Hashtable)Session["UserAllRights"])[OperatorID];
        }
        bool bReturn = false;
        if (OperatorID != 0)
        {
            if (mRightEntity != null)
            {
                switch (iType)
                {
                    case 0:
                        if (mRightEntity.CanRead)
                            bReturn = true;
                        break;
                    case 1:
                        if (mRightEntity.CanAdd)
                            bReturn = true;
                        break;
                    case 2:
                        if (mRightEntity.CanModify)
                            bReturn = true;
                        break;
                    case 3:
                        if (mRightEntity.CanDelete)
                            bReturn = true;
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            bReturn = true;
        }
        return bReturn;
    }
    #endregion

    #region  返回页面的修改权限 GetEditRight
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool GetEditRight()
    {
        return CheckPageRight(2);
    }
    #endregion

    #region 显示或隐藏母体上的服务端按钮功能
    /// <summary>
    ///新增页面显示按钮处理 
    /// </summary>
    public void ShowAddPageButton()
    {
        ShowNewButton(false);
        ShowSaveButton(true);
        ShowDeleteButton(false);
        ShowPrintButton(false);    /*打印按钮*/
        ShowQueryButton(false);    /*查询按钮*/
        ShowBackUrlButton(true);/*返回按钮*/
    }

    /// <summary>
    ///修改页面显示按钮处理 
    /// </summary>
    public void ShowEditPageButton()
    {
        ShowNewButton(true);
        ShowSaveButton(true);
        ShowDeleteButton(true);
        ShowPrintButton(false);    /*打印按钮*/
        ShowQueryButton(false);    /*查询按钮*/
        ShowBackUrlButton(true);/*返回按钮*/
    }


    #region
    /// <summary>
    /// 设置权限显示按钮！
    /// </summary>
    /// <param name="lngOperatorID"></param>
    public void setButtonRigth(long lngOperatorID,bool isEdit)
    {
        Epower.DevBase.Organization.SqlDAL.RightEntity xRightEntity = (Epower.DevBase.Organization.SqlDAL.RightEntity)((Hashtable)Session["UserAllRights"])[lngOperatorID];

        if (xRightEntity != null)
        {
            if (xRightEntity.CanAdd)            
                ShowNewButton(true);            
            else            
                ShowNewButton(false);

            if (xRightEntity.CanDelete)
                ShowDeleteButton(true);
            else
                ShowDeleteButton(false);

            if (isEdit)//当是编辑页的情况
            {
                if (xRightEntity.CanModify)
                    ShowSaveButton(true);
                else
                    ShowSaveButton(false);

                ShowBackUrlButton(true);/*返回按钮*/

                if (MainID.Trim() == "" || MainID.Trim() == "0")
                {
                    ShowNewButton(false);
                    ShowDeleteButton(false);
                }
                
            }
            else
            {
                ShowSaveButton(false);          
            }
        }

    }
    #endregion 


    /// <summary>
    ///管理页面显示按钮处理 
    /// </summary>
    public void ShowQueryPageButton()
    {
        ShowNewButton(true);    /*新增按钮*/
        ShowSaveButton(false);    /*保存按钮*/
        ShowDeleteButton(true);    /*删除按钮*/
        ShowPrintButton(false);    /*打印按钮*/
        ShowQueryButton(true);    /*查询按钮*/
        ShowBackUrlButton(false);/*返回按钮*/
    }


    /// <summary>
    /// 显示或隐藏删除按钮

    /// </summary>
    /// <param name="bShow">true=显示/ false=隐藏</param>
    public void ShowDeleteButton(bool bShow)
    {
        if (!CheckPageRight(3)) bShow = false;
        this.btn_delete.Visible = bShow;
    }

    /// <summary>
    /// 删除前提示
    /// </summary>
    /// <param name="script"></param>
    public void deleteButtonScript(string script)
    {
        if (script != "")
        {
            this.btn_delete.Attributes.Add("onclick", "return " + script);
        }
    }

    /// <summary>
    /// 显示或隐藏修改按钮

    /// </summary>
    /// <param name="bShow">true=显示/ false=隐藏</param>
    public void ShowEditButton(bool bShow)
    {
        if (!CheckPageRight(2)) bShow = false;
        this.btn_edit.Visible = bShow;
    }


    /// <summary>
    /// 显示或隐藏保存按钮

    /// </summary>
    /// <param name="bShow">true=显示/ false=隐藏</param>
    public void ShowSaveButton(bool bShow)
    {
        bool bTemp = false;
        if (this.MainID != string.Empty && CheckPageRight(2))
        {
            bTemp = true;
        }
        else if (this.MainID == string.Empty && CheckPageRight(1))   //新增
        {
            bTemp = true;
        }
        if (bShow == true)
        {
            this.btn_save.Visible = bTemp;
        }
        else
        {
            this.btn_save.Visible = false;
        }
    }

    /// <summary>
    /// 显示或隐藏打印按钮
    /// </summary>
    /// <param name="bShow">true=显示/ false=隐藏</param>
    public void ShowPrintButton(bool bShow)
    {
        //if (!PrintRight()) bShow = false;
        this.btn_print.Visible = bShow; // bShow;
    }

    /// <summary>
    /// 显示或隐藏导出按钮
    /// </summary>
    /// <param name="bShow">true=显示/ false=隐藏</param>
    public void ShowExportExcelButton(bool bShow)
    {
        //if (!PrintRight()) bShow = false;
        this.btn_exportExcel.Visible = bShow;
    }

    /// <summary>
    /// 显示或隐藏查询按钮
    /// </summary>
    /// <param name="bShow">true=显示/ false=隐藏</param>
    public void ShowQueryButton(bool bShow)
    {
        if (!CheckPageRight(0)) bShow = false;
        this.btn_query.Visible = bShow;
    }

    /// <summary>
    /// 显示或隐藏主页按钮
    /// </summary>
    /// <param name="bShow">true=显示/ false=隐藏</param>
    public void ShowHomeButton(bool bShow)
    {
        this.btn_back_home.Visible = bShow;
    }

    /// <summary>
    /// 显示或隐藏返回按钮
    /// </summary>
    /// <param name="bShow">true=显示/ false=隐藏</param>
    public void ShowBackUrlButton(bool bShow)
    {
        this.btn_back.Visible = bShow;
    }

    /// <summary>
    /// 显示或隐藏新建按钮
    /// </summary>
    /// <param name="bShow">true=显示/ false=隐藏</param>
    public void ShowNewButton(bool bShow)
    {
        if (!CheckPageRight(1)) bShow = false;
        this.btn_new.Visible = bShow;
    }

    /// <summary>
    /// 设置站点导航是否显示
    /// </summary>
    /// <param name="bShow"></param>
    public void ShowSiteMap(bool bShow)
    {
        this.SiteMapPath1.Visible = bShow;
    }
    #endregion

    #region 按钮事件
    /// <summary>
    /// 新增按钮单击事件
    /// </summary>
    /// <param name="sender">新增按钮对象</param>
    /// <param name="e">单击事件参数</param>
    protected void Button_New_Click(object sender, EventArgs e)
    {
        if (Master_Button_New_Click != null)
        {
            Master_Button_New_Click();  //新增事件
            ShowAddPageButton();

            if (Master_Button_NewFinish_Click != null)
                Master_Button_NewFinish_Click();  //新增后事件

        }
    }

    /// <summary>
    /// 修改按钮单击事件
    /// </summary>
    /// <param name="sender">修改按钮对象</param>
    /// <param name="e">单击事件参数</param>
    protected void Button_Edit_Click(object sender, EventArgs e)
    {
        if (Master_Button_Edit_Click != null)
        {
            Master_Button_Edit_Click();   //修改事件

            if (Master_Button_EditFinish_Click != null)
                Master_Button_EditFinish_Click();  //修改后事件

        }
    }


    /// <summary>
    /// 保存按钮单击事件
    /// </summary>
    /// <param name="sender">保存按钮对象</param>
    /// <param name="e">单击事件参数</param>
    protected void Button_Save_Click(object sender, EventArgs e)
    {
        if (Master_Button_Save_Click != null)
        {
            Master_Button_Save_Click();   //保存事件
            if (this.MainID.Trim() != string.Empty)
                ShowEditPageButton();
            if (IsSaveSuccess)
                Epower.DevBase.BaseTools.PageTool.MsgBox(this.Page, "数据保存成功！");

            if (Master_Button_SaveFinish_Click != null)
                Master_Button_SaveFinish_Click();   //保存后事件

        }
    }

    /// <summary>
    /// 删除按钮单击事件
    /// </summary>
    /// <param name="sender">删除按钮对象</param>
    /// <param name="e">单击事件参数</param>
    protected void Button_Delete_Click(object sender, EventArgs e)
    {
        if (Master_Button_Delete_Click != null)
        {
            Master_Button_Delete_Click();   //删除事件

            if (Master_Button_DeleteFinish_Click != null)
                Master_Button_DeleteFinish_Click();  //删除后事件

        }
    }

    /// <summary>
    /// 返回事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Btn_Back(object sender, EventArgs e)
    {
        if (Master_Button_GoHistory_Click != null)
            Master_Button_GoHistory_Click();
        else
            Response.Write("<script language=javascript> history.back(); </script>");
    }


    /// <summary>
    /// 打印按钮单击事件
    /// </summary>
    /// <param name="sender">打印按钮对象</param>
    /// <param name="e">单击事件参数</param>
    protected void Button_Print_Click(object sender, EventArgs e)
    {
        if (Master_Button_Print_Click != null)
            Master_Button_Print_Click();
    }


    /// <summary>
    /// 导出WORD按钮单击事件
    /// </summary>
    /// <param name="sender">导出WORD按钮对象</param>
    /// <param name="e">单击事件参数</param>
    protected void Button_ExportWord_Click(object sender, EventArgs e)
    {
        if (this.Master_Button_ExportWord_Click != null)
            this.Master_Button_ExportWord_Click();
    }

    /// <summary>
    /// 导出EXCEL按钮单击事件
    /// </summary>
    /// <param name="sender">导出EXCEL按钮对象</param>
    /// <param name="e">单击事件参数</param>
    protected void Button_ExportExcel_Click(object sender, EventArgs e)
    {
        if (this.Master_Button_ExportExcel_Click != null)
            this.Master_Button_ExportExcel_Click();
    }

    /// <summary>
    /// 导出PDF按钮单击事件
    /// </summary>
    /// <param name="sender">导出PDF按钮对象</param>
    /// <param name="e">单击事件参数</param>
    protected void Button_ExportPdf_Click(object sender, EventArgs e)
    {
        if (this.Master_Button_ExportPdf_Click != null)
            this.Master_Button_ExportPdf_Click();
    }

    /// <summary>
    /// 查询按钮单击事件
    /// </summary>
    /// <param name="sender">查询按钮对象</param>
    /// <param name="e">单击事件参数</param>
    protected void Button_Query_Click(object sender, EventArgs e)
    {
        if (Master_Button_Query_Click != null)
        {
            Master_Button_Query_Click();   //查询事件

            if (Master_Button_QueryFinish_Click != null)
                Master_Button_QueryFinish_Click();  //查询后事件

        }
    }

    protected void Button_Stat_Click(object sender, EventArgs e)
    {
        if (Master_Button_Stat_Click != null)
            Master_Button_Stat_Click();
    }
    #endregion

}

