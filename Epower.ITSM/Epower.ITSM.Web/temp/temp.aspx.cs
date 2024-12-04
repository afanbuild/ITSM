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
using System.IO;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.BaseTools;
using System.Data.OracleClient;

namespace Epower.ITSM.Web.temp
{
    public partial class temp : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region 导入部门
        
        protected void btnImportDept_Click(object sender, EventArgs e)
        {
            string url = Server.MapPath("~/Files/") + fuDept.FileName;
            //上传文件并指定上传目录的路径   
            fuDept.PostedFile.SaveAs(url);

            StreamReader stream = new StreamReader(url, System.Text.Encoding.Default);
            string sline = "";
            ArrayList list = new ArrayList();
            while (sline != null)
            {
                sline = stream.ReadLine();
                if (sline != null)
                {
                    list.Add(sline);
                }
            }
            stream.Close();
            //未防止部门顺序排列错误，三次重复执行更新代码
            //第一次更新部门
            SaveDept(list, false);
            //第二次更新部门
            SaveDept(list, false);
            //第三次更新部门                 
            SaveDept(list, true);

            PageTool.MsgBox(this, "部门导入完成");
        }

        /// <summary>
        /// 保存部门
        /// </summary>
        /// <param name="list"></param>
        /// <param name="IsTrue"></param>
        public void SaveDept(ArrayList list, bool IsTrue)
        {
            createLog("有关hr部门更新逻辑代码开始执行", "sours");


            string manyDeptId = "";

            try
            {
                #region 循环取值
                foreach (string str in list)
                {
                    string[] strList = str.Split(',');
                    string parentDeptCode = strList[2].ToString();//上级部门编码
                    string DeptCode = strList[1].ToString();//部门编码
                    string deptName = strList[0].ToString();//部门名称    
                   
                    try
                    {
                        //判断上级部门是否存在
                        string[] ParentDept = GetParentDept(parentDeptCode);

                        if (ParentDept != null)
                        {
                            #region 导入部门
                            string parentId = ParentDept[0];//上级部门编号
                            manyDeptId += parentId + ",";
                            string ParentFull = ParentDept[1];//上级部门生成规则
                            string ParentName = ParentDept[2];//上级部门名称

                            string[] DeptTable = GetDeptTBL(DeptCode);
                            string strDeptId = "";//部门编号
                            string strDeptName = "";//部门名称
                            string oldParentDeptId = "";//就上级部门id
                            string oldParentDeptName = "";//旧上级部门name
                            if (DeptTable != null)
                            {
                                #region 更新部门
                                try
                                {
                                    strDeptId = DeptTable[0];
                                    manyDeptId += strDeptId + ",";
                                    strDeptName = DeptTable[2];
                                    oldParentDeptId = DeptTable[3];
                                    oldParentDeptName = DeptTable[4];
                                    string strFullID = ParentFull + strDeptId.ToString().PadLeft(6, char.Parse("0"));
                                    string Massage = string.Empty;
                                    bool isTrueDept = false;
                                    if (deptName != strDeptName.Trim())
                                    {
                                        isTrueDept = true;
                                        Massage += "部门名称由原 [" + strDeptName + "] 更新为[" + deptName + "]";
                                    }
                                    if (parentId != oldParentDeptId)
                                    {
                                        isTrueDept = true;
                                        Massage += "上级部门由原 [" + oldParentDeptName + "]变更为[" + ParentName + "]";
                                    }
                                    if (isTrueDept)
                                    {
                                        long DeptKind = 0;
                                        long ORGId = 0;

                                        #region 判断该部门下是否存在锁定用户 sql
                                        string sql = @"select * from ts_user where deleted=0 and  lockStatus =1 and userid in 
                                            (
                                            select userid from ts_userdept where deptid in 
                                            (select deptid from ts_dept where deleted=0 and 
                                             ( deptId=" + strDeptId + @" or fullID like (select Fullid from  ts_dept where deptid=" + strDeptId + @")||'%')) 
                                            and  deleted=0
                                            )";
                                        #endregion

                                        DataTable dtvalue = ExecuteTBL(sql);
                                        if (dtvalue.Rows.Count > 0)
                                        {
                                            ORGId = DeptDP.GetDirectOrg(long.Parse(strDeptId));
                                            ORGId = DeptKind.ToString() == "1" ? long.Parse(strDeptId.ToString()) : ORGId;
                                            //如果存在锁定用户，则部门名称发生变化时，更改部门名称，否则不做任何修改处理,对于上下级关系不做任何处理
                                            string strSQL = "update Ts_Dept set DeptName='" + deptName + "',ORGId=" + ORGId + ", deleted=0  where deptId=" + strDeptId;
                                            Execute(strSQL);
                                            createLog("部门名称由原 [" + strDeptName + "] 更新为[" + deptName + "]", "soure");
                                        }
                                        else
                                        {
                                            //不存在锁定用户时，部门名称以及上下级关系的调整的处理
                                            ORGId = DeptDP.GetDirectOrg(long.Parse(parentId));
                                            ORGId = DeptKind.ToString() == "1" ? long.Parse(strDeptId.ToString()) : ORGId;
                                            string strSQL = "update Ts_Dept set DeptName='" + deptName + "', parentId=" + parentId + ",ORGId=" + ORGId + ",fullid=" + StringTool.SqlQ(strFullID) + ",deptKind=" + DeptKind.ToString() + ",deleted=0  where deptId=" + strDeptId;
                                            Execute(strSQL);
                                            createLog(Massage, "soure");
                                        }
                                    }
                                    else
                                    {
                                        long DeptKind = 0;
                                        long ORGId = 0;

                                        ORGId = DeptDP.GetDirectOrg(long.Parse(strDeptId));
                                        ORGId = DeptKind.ToString() == "1" ? long.Parse(strDeptId.ToString()) : ORGId;
                                        string strSQL = "update Ts_Dept set deleted=0,FullId=" + StringTool.SqlQ(strFullID) + ",ORGId=" + ORGId + "  where deptId=" + strDeptId;
                                        Execute(strSQL);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    string Massage = "部门编号为[" + DeptCode + "] 在更新时出现类型转换错误  抛出错误：" + ex.Message;
                                    createLog(Massage, "error");
                                }
                                #endregion
                            }
                            else
                            {
                                #region 新建部门
                                try
                                {
                                    long DeptKind = 0;
                                    long ORGId = 0;

                                    long lngNextID = EpowerGlobal.EPGlobal.GetNextID("DEPT_ID");
                                    strDeptId = lngNextID.ToString();
                                    manyDeptId += strDeptId + ",";
                                    string strFullID = ParentFull + lngNextID.ToString().PadLeft(6, char.Parse("0"));
                                    ORGId = DeptDP.GetDirectOrg(long.Parse(parentId));
                                    ORGId = DeptKind.ToString() == "1" ? long.Parse(strDeptId.ToString()) : ORGId;
                                    string strSQL = "INSERT INTO ts_dept (deptid,fullid,orgid,deptkind,depttype,parentid,deptname,layer,iconid,sortid,istemp,managerid,leaderid,description,startdate,enddate,deleted,createid,createdate,PID,DeptCode,DOC_Center)" +
                                           " Values(" +
                                           lngNextID.ToString() + "," +
                                           StringTool.SqlQ(strFullID) + "," +
                                           ORGId.ToString() + "," +
                                           DeptKind.ToString() + "," +
                                           "0" + "," +
                                           parentId + "," +
                                           StringTool.SqlQ(deptName) + "," +
                                           "0" + "," +
                                           "0" + "," +
                                           "0" + "," +
                                           ((int)0).ToString() + "," +
                                           "1" + "," +
                                           "0" + "," +
                                           StringTool.SqlQ("") + "," +
                                           StringTool.EmptyToNullDate("") + "," +
                                           StringTool.EmptyToNullDate("") + "," +
                                           "0," +
                                           "0" + ",sysdate," +
                                           "999" + ", " +
                                           StringTool.SqlQ(DeptCode) + " , " +
                                           StringTool.SqlQ("") +
                                       ")";
                                    Execute(strSQL);
                                    string Massage = " 部门编号为 ：‘" + DeptCode + "’ 部门名称为[" + deptName + "]  上级部门编号为 [" + parentDeptCode + "]  部门创建成功！ ";
                                    createLog(Massage, "soure");

                                }
                                catch (Exception)
                                {
                                    string Massage = "异常 部门编号为[" + DeptCode + "]  在创建时转化类型出现错误";
                                    createLog(Massage, "error");
                                }
                                #endregion
                            }
                            #endregion
                        }
                        else
                        {
                            string errorMassage = " 因找不到上级部门编号[" + parentDeptCode + "],更新部门" + DeptCode + "失败";
                            createLog(errorMassage, "error");
                        }

                    }
                    catch (Exception ex)
                    {
                        string errorMassage = "部门编号[" + DeptCode + "]  部门名称[" + deptName + "]  上级部门编号[" + parentDeptCode + "]   因" + ex.Message.ToString() + "更新失败";
                        createLog(errorMassage, "error");
                    }

                }
                #endregion

            }
            catch (Exception ex)
            {
                createLog("导入HR部门数据异常:" + ex.Message, "error");
            }

            if (IsTrue == true)
            {
                #region 删除hr中没有的部门与表
                //删除在hr中没有的部门
                if (manyDeptId != "")
                {

                    string strSQLUser = "select deptid from ts_dept where deleted=0 and deptId<>1";
                    DataTable dt = ExecuteTBL(strSQLUser);
                    string[] DeptIdList = manyDeptId.Trim().Trim(',').Split(',');
                    string Depts = "";
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["deptid"].ToString() != "1")
                        {
                            int returnValue = Array.IndexOf(DeptIdList, dr["deptid"].ToString());
                            if (returnValue == -1)
                            {
                                Depts += dr["deptid"].ToString() + ",";
                            }
                        }
                    }
                    if (Depts != "")
                    {

                        string[] value = Depts.Trim().Trim(',').Split(',');
                        foreach (string deptIdValue in value)
                        {
                            string sql = @"select * from ts_user where deleted=0 and  lockStatus =1 and userid in 
                                        (
                                        select userid from ts_userdept where deptid in 
                                        (select deptid from ts_dept where deleted=0 and 
                                         ( deptId=" + deptIdValue + @" or fullID like (select Fullid from  ts_dept where deptid=" + deptIdValue + @")||'%')) 
                                        and  deleted=0
                                        )";
                            DataTable dtvalue = ExecuteTBL(sql);
                            if (dtvalue.Rows.Count == 0)
                            {
                                string strSQL = "update ts_dept set deleted=1 where deptId =" + deptIdValue + "  and deptId<>1 and deptKind<>1";
                                Execute(strSQL);
                            }
                            else
                            {

                                createLog(" 部门" + deptIdValue + " 存在锁定用户，不做任何更新处理", "sours");
                            }
                        }
                    }
                    else
                    {
                        createLog("本次hr更新未删除任何部门", "sours");
                    }
                }
                #endregion
            }

            createLog("有关hr部门更新逻辑代码结束执行", "sours");

        }

        #region 判断上级部门是否存在
        /// <summary>
        /// 判断部门编号是否存在
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public string[] GetParentDept(string deptCode)
        {
            string[] Parentdept = null;
            
            string strSQL = "SELECT  DeptId,FullID,DeptName from TS_DEPT where  deleted=0 and   DeptCode ='" + deptCode + "'";
            try
            {
                DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
                if (dt.Rows.Count > 0)
                {
                    Parentdept = new string[3];
                    Parentdept[0] = dt.Rows[0]["DeptId"].ToString();
                    Parentdept[1] = dt.Rows[0]["FullID"].ToString();
                    Parentdept[2] = dt.Rows[0]["DeptName"].ToString();
                }
            }
            catch (Exception ex)
            {
                string errorMassage = "判断部门编号是否存在异常：" + strSQL + ":" + ex.Message;
                createLog(errorMassage, "error");
            }           
            return Parentdept;
        }
        #endregion

        #region 判断部门是否存在
        /// <summary>
        /// 判断部门编号是否存在
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public string[] GetDeptTBL(string deptCode)
        {
            string[] deptTBL = null;            
            string strSQL = "SELECT  A.DeptId,A.FullID,A.DeptName,B.DeptId ParentDeptId,B.DeptName parentDeptName from TS_DEPT  A left join  TS_DEPT B on A.parentId=B.DeptId where A.deleted=0 and  A.DeptCode='" + deptCode + "'";
            try
            {
                DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
                if (dt.Rows.Count > 0)
                {
                    deptTBL = new string[5];
                    deptTBL[0] = dt.Rows[0]["DeptId"].ToString();
                    deptTBL[1] = dt.Rows[0]["FullID"].ToString();
                    deptTBL[2] = dt.Rows[0]["DeptName"].ToString();
                    deptTBL[3] = dt.Rows[0]["ParentDeptId"].ToString();
                    deptTBL[4] = dt.Rows[0]["parentDeptName"].ToString();
                }
            }
            catch (Exception ex)
            {
                string errorMassage = "判断部门编号是否存在异常：" + strSQL + ":" + ex.Message;
                createLog(errorMassage, "error");
            }
            return deptTBL;
        }
        #endregion

        #endregion


        #region 导入用户

        protected void btnImportUser_Click(object sender, EventArgs e)
        {

            string url = Server.MapPath("~/Files/") + fuUser.FileName;
            //上传文件并指定上传目录的路径   
            fuUser.PostedFile.SaveAs(url);

            StreamReader stream = new StreamReader(url, System.Text.Encoding.Default);
            string sline = "";
            ArrayList listUser = new ArrayList();
            while (sline != null)
            {
                sline = stream.ReadLine();
                if (sline != null)
                {
                    listUser.Add(sline);
                }
            }
            stream.Close();

            saveUser(listUser);

            PageTool.MsgBox(this, "用户导入完成");
        }

        public void saveUser(ArrayList list)
        {
            createLog("用户信息与hr同步的业务逻辑代码开始执行", "sours");

            string manyUserId = "";

            try
            {
                #region 循环取值
                foreach (string str in list)
                {
                    try
                    {
                        string[] strList = str.Split(',');
                        string loginName = strList[0].ToString();//工号
                        string UserName = strList[1].ToString();//姓名
                        string phone = strList[3].ToString();//电话
                        if (phone.Length > 14)
                        {
                            phone = phone.Substring(0, 14);
                        }
                        string mobile = strList[4].ToString();//手机
                        if (mobile.Length > 11)
                        {
                            mobile = mobile.Substring(0, 11).ToString();
                        }
                        string email = strList[5].ToString();//邮箱                           
                        string DeptCode = strList[6].ToString();//部门编码                           
                        string sex = strList[7].ToString();//性别                           
                        string job = strList[2].ToString();//职位名称
                        long lngJobID = 0;
                        if (job.Trim() != "")
                        {
                            lngJobID = ZhiW(job.Trim());
                        }

                        try
                        {
                            #region 实际部门和用户的业务操作
                            //判断上级部门是否存在
                            string[] DeptTBL = GetParentDept(DeptCode);
                            if (DeptTBL != null)
                            {
                                string strDeptId = DeptTBL[0];//部门编号
                                string strDeptName = DeptTBL[2];//部门名称                                                             

                                #region 导入用户
                                string[] userinfo = UserExists(loginName);
                                if (userinfo != null)//修改用户信息
                                {
                                    #region
                                    manyUserId += userinfo[1] + ",";
                                    try
                                    {
                                        bool flag = false;
                                        string message = "";
                                        if (UserName != userinfo[3])
                                        {
                                            flag = true;
                                            message = message + "原用户名称[" + userinfo[3] + "]改为[" + UserName + "]；";
                                        }
                                        if (strDeptName != userinfo[7])
                                        {
                                            flag = true;
                                            message = message + "原部门[" + userinfo[7] + "]调动到[" + strDeptName + "]。";
                                        }
                                        if (mobile != userinfo[4])
                                        {
                                            flag = true;
                                            message = message + "原手机号[" + userinfo[4] + "]改为[" + mobile + "]；";
                                        }
                                        if (email != userinfo[5])
                                        {
                                            flag = true;
                                            message = message + "原Email[" + userinfo[5] + "]改为[" + email + "]；";
                                        }
                                        if (phone != userinfo[9])
                                        {
                                            flag = true;
                                            message = message + "原电话号码[" + userinfo[9] + "]改为[" + phone + "]；";
                                        }
                                        if (userinfo[10] == "1")
                                        {
                                            string strSQL = "update ts_user set deleted=0 where userid = " + userinfo[1];
                                            Execute(strSQL);
                                            strSQL = " UPDATE Ts_UserDept SET UpdateID=1,UpdateDate=sysdate,deleted=0 Where UserId =" + userinfo[1];
                                            Execute(strSQL);
                                        }
                                        if (job != userinfo[11])
                                        {
                                            flag = true;
                                            message = message + "原职位[" + userinfo[11] + "]改为[" + job + "]；";
                                        }
                                        if (userinfo[8] != "1")//未锁定
                                        {
                                            if (flag)
                                            {
                                                #region 更新用户
                                                try
                                                {

                                                    string strSQL = "UPDATE ts_user SET " +
                                                      " Name = " + StringTool.SqlQ(UserName) + "," +
                                                      " TelNo = " + StringTool.SqlQ(phone) + "," +
                                                      " Mobile = " + StringTool.SqlQ(mobile) + "," +
                                                      " Email = " + StringTool.SqlQ(email) + "," +
                                                      " Job =" + StringTool.SqlQ(job) + "," +
                                                      " JobID=" + lngJobID + "," +
                                                      " UpdateDate=sysdate," +
                                                      " UpdateID=" + "1" + "," +
                                                      " Deleted=" + "0" +
                                                      " WHERE userid = " + userinfo[1];
                                                    Execute(strSQL);
                                                    strSQL = String.Format(" UPDATE Ts_UserDept SET UpdateID={0},DeptID={1},UpdateDate=sysdate,deleted=0 Where UserId ={2} ", "1", strDeptId, userinfo[1]);
                                                    Execute(strSQL);
                                                    createLog("工号为(" + loginName + ")的用户：" + message + "  更新成功", "soure");
                                                    deptEdit_Log(userinfo[1], "工号为(" + loginName + ")的用户：" + message + "  更新成功", "信息修改");


                                                }
                                                catch
                                                {
                                                    string Massage = "异常  工号为[" + loginName + "] 在更新时出现转化错误";
                                                    createLog(Massage, "error");
                                                }
                                                #endregion
                                            }

                                            try
                                            {
                                                #region  更新用户的时候，同时更新客户资料
                                                Br_ECustomerDP2 ee = new Br_ECustomerDP2();
                                                long DirectdeptId = DeptDP.GetDirectOrg(long.Parse(strDeptId));
                                                string DirectDeptname = DeptDP.GetDeptName(DirectdeptId);
                                                ee = ee.GetReCordedByUserID(long.Parse(userinfo[1]));
                                                ee.MastCustID = long.Parse(getMastCost(DirectDeptname, phone, UserName, long.Parse(userinfo[1])).ToString());
                                                ee.MastCustName = DirectDeptname; //服务单位名称
                                                ee.Address = "";
                                                ee.CustomerType = 0;
                                                ee.CustomerTypeName = "";
                                                ee.CustomCode = loginName;
                                                ee.FullName = UserName;
                                                ee.LinkMan1 = "";
                                                ee.ShortName = UserName;
                                                ee.Tel1 = phone;
                                                ee.Email = email;
                                                ee.Deleted = 0;
                                                ee.Rights = "";
                                                ee.Remark = "";
                                                ee.CustDeptName = DeptDP.GetDeptName(long.Parse(strDeptId));
                                                ee.UserID = long.Parse(userinfo[1]);
                                                ee.SchemaValue = "";
                                                ee.Moblie = mobile;
                                                ee.UpdateTime = DateTime.Now;
                                                if (ee.ID != 0)
                                                {
                                                    ee.UpdateRecorded(ee);
                                                }
                                                else
                                                {
                                                    ee.InsertRecorded(ee);
                                                }
                                                #endregion
                                            }
                                            catch
                                            {
                                                string kuMassage = "异常  工号为[" + loginName + "] 在更新客户时时出现错误";
                                                createLog(kuMassage, "error");
                                            }
                                        }
                                        else//已锁定
                                        {
                                            if (flag)
                                            {
                                                deptEdit_Log(userinfo[1], "工号为(" + loginName + ")的用户：" + message + "  已锁定，修改不成功", "信息修改");
                                                createLog("工号为(" + loginName + ")的用户 已被锁定 修改如下内容不成功" + message, "soure");
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        string Massage = "工号为[" + loginName + "]在更新时出现错误" + ex.Message;
                                        createLog(Massage, "error");
                                    }
                                    #endregion
                                }
                                else//新增用户信息
                                {
                                    #region
                                    try
                                    {
                                        long lngNextID = EpowerGlobal.EPGlobal.GetNextID("USER_ID");
                                        manyUserId += lngNextID.ToString() + ",";
                                        string strSQL = "INSERT INTO ts_user (UserID,LoginName,Password,Name,Sex,Job,JobID,TelNo,Mobile,Email,QQ,EduLevel,School,IsTemp,Deleted,CreateID,LockStatus,CreateDate)" +
                                             " Values(" +
                                             lngNextID.ToString() + "," +
                                             StringTool.SqlQ(loginName) + "," +
                                             StringTool.SqlQ("") + "," +
                                             StringTool.SqlQ(UserName) + "," +
                                             StringTool.SqlQ(sex) + "," +
                                             StringTool.SqlQ(job) + "," +
                                             lngJobID + "," +
                                             StringTool.SqlQ(phone) + "," +
                                             StringTool.SqlQ(mobile) + "," +
                                             StringTool.SqlQ(email) + "," +
                                             StringTool.SqlQ("") + "," +
                                             StringTool.SqlQ("") + "," +
                                             StringTool.SqlQ("") + "," +
                                             "0" + "," +
                                             "0" + "," +
                                             "1" + ",0,sysdate" +
                                             ")";

                                        Execute(strSQL);

                                        string strSQL_R = "INSERT INTO ts_userdept (userid,deptid,relation,sortid,deleted,CreateID,CreateDate,UpdateDate)" +
                                        "VALUES(" +
                                        lngNextID.ToString() + "," +
                                        strDeptId + "," +
                                        (int)0 + "," +
                                        "0" + "," +
                                        "0" + "," +
                                        "1" + "," +
                                        "sysdate,sysdate " +
                                        ")";
                                        Execute(strSQL_R);
                                        string Massage = "工号为[" + loginName + "] 姓名为[" + UserName + "] 新建成功";
                                        deptEdit_Log(lngNextID.ToString(), Massage, "增加用户");
                                        createLog(Massage, "soure");
                                        try
                                        {
                                            #region  插入用户的时候，同时插入客户资料
                                            Br_ECustomerDP2 ee = new Br_ECustomerDP2();
                                            long DirectdeptId = DeptDP.GetDirectOrg(long.Parse(strDeptId));
                                            string DirectDeptname = DeptDP.GetDeptName(DirectdeptId);
                                            ee.MastCustID = long.Parse(getMastCost(DirectDeptname, phone, UserName, lngNextID).ToString());
                                            ee.MastCustName = DirectDeptname; //服务单位名称
                                            ee.Address = "";
                                            ee.CustomerType = 0;
                                            ee.CustomerTypeName = "";
                                            ee.CustomCode = loginName;
                                            ee.FullName = UserName;
                                            ee.LinkMan1 = "";
                                            ee.ShortName = UserName;
                                            ee.Tel1 = phone;
                                            ee.Email = email;
                                            ee.Rights = "";
                                            ee.Remark = "";
                                            ee.CustDeptName = DeptDP.GetDeptName(long.Parse(strDeptId));
                                            ee.UserID = lngNextID;
                                            ee.SchemaValue = "";
                                            ee.Moblie = mobile;
                                            ee.UpdateTime = DateTime.Now;
                                            ee.InsertRecorded(ee);
                                            #endregion
                                        }
                                        catch
                                        {
                                            string kHMassage = "工号为[" + loginName + "]在新建客户时出现错误";
                                            createLog(kHMassage, "error");
                                        }
                                    }
                                    catch
                                    {
                                        string Massage = "工号为[" + loginName + "]在新建时出现错误";
                                        createLog(Massage, "error");
                                    }
                                    #endregion
                                }
                                #endregion

                            }
                            else
                            {
                                string errorMassage = "  姓名[" + UserName + "]  工号[" + loginName + "]  因未找到部门编号[" + DeptCode + "] 更新失败";
                                createLog(errorMassage, "error");
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            string errorMassage = "姓名[" + UserName + "] 工号[" + loginName + "]  部门编号[" + DeptCode + "]    因" + ex.Message.ToString() + "更新失败";
                            createLog(errorMassage, "error"); 
                        }

                    }
                    catch (Exception ex)
                    {
                        createLog("导入用户数据异常：" + ex.Message, "error");
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                createLog("导入用户数据异常：" + ex.Message, "error");
            }

            #region
            //删除在hr中没有的人员
            if (manyUserId != "")
            {

                string strSQLUser = "select userId from ts_user where deleted=0 and loginName<>'sa'  and nvl(lockstatus,0)=0";
                DataTable dt = ExecuteTBL(strSQLUser);
                string[] UserList = manyUserId.Trim().Trim(',').Split(',');
                string User = "";
                foreach (DataRow dr in dt.Rows)
                {
                    int returnValue = Array.IndexOf(UserList, dr["userId"].ToString());
                    if (returnValue == -1)
                    {
                        User += dr["userId"].ToString() + ",";
                    }
                }
                if (User.Trim() != "")
                {
                    string[] userValue = User.Trim().Trim(',').Trim().Split(',');
                    foreach (string vlaue in userValue)
                    {
                        string strSQL = "update ts_user set deleted=1 where userid =" + vlaue.Trim();
                        Execute(strSQL);
                        strSQL = "update ts_userdept set deleted=1 where userid =" + vlaue.Trim();
                        Execute(strSQL);
                        strSQL = "select * from ts_user where userid=" + vlaue.Trim();
                        DataTable dtdelete = ExecuteTBL(strSQL);
                        if (dtdelete.Rows.Count > 0)
                        {
                            deptEdit_Log(vlaue.ToString(), "用户工号[" + dtdelete.Rows[0]["loginName"].ToString() + "] 姓名[" + dtdelete.Rows[0]["name"].ToString() + "]", "离职用户");
                        }
                    }
                }
                else
                {
                    createLog("本次hr更新未删除任何用户", "sours");
                }

            }
            #endregion

            createLog("用户信息与hr同步的业务逻辑代码结束执行", "sours");
        }

        /// <summary>
        /// 得到直属机构部门ID
        /// </summary>
        /// <param name="deptName"></param>
        /// <returns></returns>
        public string getMastCost(string deptName, string TelNo, string UserName, long UserID)
        {
            string strMastid = string.Empty;
            string sqlSQL = "select * from Br_MastCustomer where Shortname=" + StringTool.SqlQ(deptName.ToString());
            OracleConnection scnn = ConfigTool.GetConnection("SQLConnString");
            DataTable dt = OracleDbHelper.ExecuteDataTable(scnn, CommandType.Text, sqlSQL);
            ConfigTool.CloseConnection(scnn);
            if (dt.Rows.Count > 0)
            {
                strMastid = dt.Rows[0]["ID"].ToString();
            }
            else
            {
                OracleConnection cn = ConfigTool.GetConnection("SQLConnString");
                string strSQL = string.Empty;
                string strID = "0";
                try
                {
                    strID = EpowerGlobal.EPGlobal.GetNextID("Br_MastCustomerID").ToString();
                    strSQL = @"INSERT INTO Br_MastCustomer(
									    ID,
									    ShortName,
									    FullName,
									    Address,
									    EnterpriseType,
									    EnterpriseTypeName,
									    CustomerType,
									    CustomerTypeName,
									    CustomCode,
									    Tel1,
									    LinkMan1,
									    Fax1,
									    WebSite,
									    ServiceProtocol,
									    Deleted,
									    RegUserID,
									    RegUserName,
									    RegTime
					    )
					    Values( " +
                                strID.ToString() + "," +
                                StringTool.SqlQ(deptName) + "," +
                                StringTool.SqlQ(deptName) + "," +
                                StringTool.SqlQ("") + "," +
                                0 + "," +
                                StringTool.SqlQ("") + "," +
                                0 + "," +
                                StringTool.SqlQ("") + "," +
                                StringTool.SqlQ("") + "," +
                                StringTool.SqlQ(TelNo) + "," +
                                StringTool.SqlQ(UserName) + "," +
                                StringTool.SqlQ("") + "," +
                                StringTool.SqlQ("") + "," +
                                StringTool.SqlQ("") + "," +
                                "0," +
                                UserID.ToString() + "," +
                                StringTool.SqlQ(UserName) + "," +
                                "sysdate" +
                        ")";
                    OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSQL);
                    strMastid = strID.ToString();

                }
                catch
                {
                    throw;
                }
                finally
                {
                    ConfigTool.CloseConnection(cn);
                }
            }
            return strMastid;
        }

        /// <summary>
        /// 用户信息是否存在
        /// </summary>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        private string[] UserExists(string usercode)
        {
            try
            {
                string[] userinfo = null;
                string strSQL = string.Format(@" SELECT A.DEPTNAME,A.DEPTCODE,A.DEPTID,C.USERID,C.LOGINNAME,C.NAME,C.MOBILE,C.EMAIL,c.TelNo,C.LockStatus,C.deleted,C.Job FROM  Ts_Dept A   left JOIN ts_userdept B   ON A.DEPTID=B.DEPTID   left JOIN ts_user C   ON C.USERID=B.USERID     WHERE C.LOGINNAME='{0}' ",
                                           usercode);
                DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
                
                if (dt.Rows.Count > 0)
                {
                    userinfo = new string[12];
                    userinfo[0] = dt.Rows[0]["DEPTID"].ToString();
                    userinfo[1] = dt.Rows[0]["USERID"].ToString();
                    userinfo[2] = dt.Rows[0]["LOGINNAME"].ToString();
                    userinfo[3] = dt.Rows[0]["NAME"].ToString();
                    userinfo[4] = dt.Rows[0]["MOBILE"].ToString();
                    userinfo[5] = dt.Rows[0]["EMAIL"].ToString();
                    userinfo[6] = dt.Rows[0]["DEPTCODE"].ToString();
                    userinfo[7] = dt.Rows[0]["DEPTNAME"].ToString();
                    userinfo[8] = dt.Rows[0]["LockStatus"].ToString();
                    userinfo[9] = dt.Rows[0]["TelNo"].ToString();
                    userinfo[10] = dt.Rows[0]["deleted"].ToString();
                    userinfo[11] = dt.Rows[0]["Job"].ToString();
                }
                return userinfo;
            }
            catch
            {
                throw new Exception();
            }
        }


        public long ZhiW(string catalogName)
        {
            long lngReturn = 0;

            string strSQL = "select * from es_catalog where parentid=1014 and catalogName=" + StringTool.SqlQ(catalogName);
            DataTable dt = ExecuteTBL(strSQL);
            if (dt.Rows.Count == 0)
            {
                long lngNextID = EpowerGlobal.EPGlobal.GetNextID("Catalog_ID");
                if (lngNextID != 0)
                {
                    string strFullID = "001014" + lngNextID.ToString().PadLeft(6, char.Parse("0"));
                    string FullID = strFullID;
                    strSQL = "INSERT INTO Es_Catalog (Catalogid,fullid,orgid,parentid,Catalogname,sortid,Remark,Deleted,UpdateTime,IsshowSchema)" +
                        " Values(" +
                        lngNextID.ToString() + "," +
                        StringTool.SqlQ(strFullID) + "," +
                        "10287" + "," +
                        "1014" + "," +
                        StringTool.SqlQ(catalogName) + "," +
                        0 + "," +
                        StringTool.SqlQ("由hr更新过来") + "," +
                        "0" + "," +
                        "sysdate,0" +
                        ")";
                    Execute(strSQL);

                    lngReturn = lngNextID;
                }
            }
            else
            {
                lngReturn = long.Parse(dt.Rows[0]["CatalogID"].ToString());
            }

            return lngReturn;
        }

        #region 锁定用户修改日志

        private void deptEdit_Log(string userid, string message, string typeName)
        {
            try
            {
                string strSQL = string.Format(@"INSERT INTO HR_Import_Log(id,logtype,logcontent,regtime)values({0},'{1}','{2}',sysdate)",
                                          userid, typeName, message);

                CommonDP.ExcuteSql(strSQL);
            }
            catch
            {
                throw new Exception();
            }
        }

        #endregion

        #endregion


        #region 执行sql语句
        public void Execute(string strSQL)
        {
           
            try
            {
                CommonDP.ExcuteSql(strSQL);
            }
            catch (Exception ex)
            {
                string errorMassage = "异常：" + strSQL + ":" + ex.Message;
                createLog(errorMassage, "error");
            }
        }
        #endregion


        #region 执行sql语句
        public DataTable ExecuteTBL(string strSQL)
        {
            DataTable dt = null;            
            try
            {
                dt = CommonDP.ExcuteSqlTable(strSQL);
            }
            catch (Exception ex)
            {
                string errorMassage = "异常：" + strSQL + ":" + ex.Message;
                createLog(errorMassage, "error");
            }           
            return dt;

        }
        #endregion

        #region 创建日志
        public void createLog(string errorMesssage, string Type)
        {
            string feiname = DateTime.Now.Date.ToString("yyyyMMdd");
            string logPath = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("TempCataLog", "logPath");
            string filePath = logPath + "/" + feiname + ".txt";
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
            StreamWriter strem = File.AppendText(filePath);
            strem.WriteLine(System.DateTime.Now.ToString() + "  " + Type + ":");
            strem.WriteLine(errorMesssage);
            strem.WriteLine("");
            strem.Close();
        }

        #endregion
    }
}
