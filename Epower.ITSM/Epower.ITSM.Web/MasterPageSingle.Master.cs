using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace Epower.ITSM.Web
{   

    public partial class MasterPageSingle : System.Web.UI.MasterPage
    {
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
        /// 设备按钮表格是否可见
        /// </summary>
        public bool TableVisible
        {
            set
            {                
                tbtop.Visible = value;
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


      

    }

}

