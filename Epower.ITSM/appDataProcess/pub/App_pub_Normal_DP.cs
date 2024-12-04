using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.IO;
using MyComponent;
using System.Collections;
using IappDataProcess;
using EpowerGlobal;
using appDataProcess;
using Epower.ITSM.SqlDAL;
using EpowerCom;
using Epower.DevBase.BaseTools;
using System.Data.OracleClient;



namespace appDataProcess.pub
{
    /// <summary>
    /// App_pub_Normal_DP 的摘要说明。
    /// </summary>
    public class App_pub_Normal_DP : IDataProcess
    {
        public App_pub_Normal_DP()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 返回信息项结果表
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <param name="lngOpID"></param>
        /// <returns></returns>
        public override DataTable GetFieldsDataTable(long lngFlowID, long lngOpID)
        {
            string strSQL = "";

            OracleConnection cn = ConfigToolApp.GetConnection();

            strSQL = "SELECT * FROM App_pub_Normal_Head WHERE FlowID =" + lngFlowID.ToString();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigToolApp.CloseConnection(cn);
            return dt;
        }

        /// <summary>
        /// 沟通时保存业务数据
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngID"></param>
        /// <param name="lngNodeModelID"></param>
        /// <param name="lngFlowModelID"></param>
        /// <param name="lngOpID"></param>
        /// <param name="strXMlFieldValue"></param>
        /// <param name="lngMessageID"></param>
        public override void SaveFieldValuesForCommunic(OracleTransaction trans, long lngID, long lngNodeModelID, long lngFlowModelID, long lngOpID, string strXMlFieldValue, long lngMessageID)
        {
        }

        /// <summary>
        ///  获取字段的值的字符串（XML）
        /// </summary>
        /// <param name="lngID"></param>
        /// <param name="lngAppID"></param>
        /// <param name="lngOpID"></param>
        /// <returns></returns>
        public override string GetFieldValues(long lngID, long lngOpID)
        {
            return string.Empty;
        }

        /// <summary>
        /// 返回信息项结果集
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <param name="lngOpID"></param>
        /// <returns></returns>
        public override DataSet GetFieldsDataSet(long lngFlowID, long lngOpID)
        {
            string strSQL = "";

            OracleConnection cn = ConfigToolApp.GetConnection();

            strSQL = "SELECT * FROM App_pub_Normal_Head WHERE FlowID =" + lngFlowID.ToString();
            DataSet ds = OracleDbHelper.ExecuteDataset(cn, CommandType.Text, strSQL);
            ConfigToolApp.CloseConnection(cn);
            return ds;
        }

        /// <summary>
        /// 阅知状态下保存应用中的信息值
        /// </summary>
        /// <param name="lngID"></param>
        /// <param name="lngAppID"></param>
        /// <param name="lngOpID"></param>
        /// <param name="strXMlFieldValue"></param>
        public override void SaveFieldValuesForRead(OracleTransaction trans, long lngFlowID, long lngNodeModelID, long lngFlowModelID, long lngOpID, string strValues, long lngMessageID)
        {
        }

        /// <summary>
        /// 添加和发送流程后用户代码
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngNodeModelID"></param>
        /// <param name="lngFlowModelID"></param>
        /// <param name="lngOpID"></param>
        /// <param name="lngMessageID"></param>
        public override void SendFlowFinish(OracleTransaction trans, long lngFlowID, long lngNodeModelID, long lngFlowModelID, long lngOpID, long lngMessageID)
        {
        }


        /// <summary>
        /// 回收时应用执行的具体实现的接口
        /// 由于回收操作需要删除一些内容，所以用户自定义操作会在前段执行
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngMessageID"></param>
        public override void TakeBackUserProcess(OracleTransaction trans, long lngFlowID, long lngMessageID)
        {

        }

        /// <summary>
        /// 流程发送处理通知接口(在流程发送/新增处理的提交前执行)
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngID"></param>
        /// <param name="lngNodeModelID"></param>
        /// <param name="lngFlowModelID"></param>
        /// <param name="lngOpID"></param>
        /// <param name="strXMlFieldValue"></param>
        /// <param name="strReceivers">接收者和消息值列字符串 格式: 接收者ID|消息ID,接收者ID|消息ID,... (仅主办的消息)</param>
        /// <param name="lngMessageID"></param>
        public override void NotifyMessage(OracleTransaction trans, long lngID, long lngNodeModelID, long lngFlowModelID, long lngOpID, string strXMlFieldValue, string strReceivers, long lngMessageID)
        {

            FieldValues fv = new FieldValues(strXMlFieldValue);

            #region 机构环节时获取接收人员列表


            if (strReceivers == "")
            {
                string strSQL = "SELECT RECEIVEID,MESSAGEID FROM ES_RECEIVELIST WHERE MessageID IN (SELECT TMessageID FROM ES_MESSAGEFROMTO WHERE FMESSAGEID=" + lngMessageID.ToString() + ")";
                DataTable dt = OracleDbHelper.ExecuteDataset(trans, CommandType.Text, strSQL).Tables[0];
                foreach (DataRow row in dt.Rows)
                {
                    strReceivers = strReceivers + row["RECEIVEID"].ToString() + "|" + row["MESSAGEID"].ToString() + ",";
                }
                if (strReceivers != "")
                    strReceivers = strReceivers.Substring(0, strReceivers.Length - 1);
            }
            #endregion

            #region 发送邮件


            if (fv.GetFieldValue("EmailNotify") != null)
            {
                //通知处理人的情况下


                if (fv.GetFieldValue("EmailNotify").Value.Trim().ToLower() == "true")
                {
                    MailSendDeal.SendEmailPublicV2(trans, lngID, strReceivers, fv, lngFlowModelID, 0, lngMessageID);
                }
            }
            #endregion

            #region 发送短信


            //短信发送


            //if (fv.GetFieldValue("SMSNotify") != null && fv.GetFieldValue("SMSNotify").Value.Trim().ToLower() == "true")
            //{
            //    SMSSendDeal.SendMessagePublic(trans, lngID, strReceivers, fv, lngFlowModelID);
            //}
            #endregion
        }


        /// <summary>
        /// 保存应用中的信息值
        /// </summary>
        /// <param name="lngID"></param>
        /// <param name="lngAppID"></param>
        /// <param name="lngOpID"></param>
        /// <param name="strXMlFieldValue"></param>
        public override void SaveFieldValues(OracleTransaction trans, long lngFlowID, long lngNodeModelID, long lngFlowModelID, long lngActionID, long lngOpID, string strValues, long lngMessageID)
        {
            string strSQL = "";
            OracleDataReader dr;


            //long lngBusNodeID = MessageDep.GetBusinessNodeID(lngFlowModelID, lngNodeModelID);
            //long lngBusActionID = MessageDep.GetBusinessActionID(lngFlowModelID, lngNodeModelID, lngActionID);

            long lngNextID = 0;
            long lngApplyID = 0;
            string strApplyName = "";
            long lngDeptID = 0;
            string strDeptName = "";
            string strFlowName = "";
            string strStartDate = "";
            string strEndDate = "";

            string strDate1 = "";
            string strDate2 = "";
            string strDate3 = "";
            string strDate4 = "";
            string strDate5 = "";
            string strDate6 = "";
            string strDate7 = "";
            string strDate8 = "";

            string strString1 = "";
            string strString2 = "";
            string strString3 = "";
            string strString4 = "";
            string strString5 = "";
            string strString6 = "";
            string strString7 = "";
            string strString8 = "";

            string strNumber1 = "0";
            string strNumber2 = "0";
            string strNumber3 = "0";
            string strNumber4 = "0";
            string strNumber5 = "0";

            string strCateValue1 = "";
            string strCateValue2 = "";
            string strCateValue3 = "";
            string strCateValue4 = "";
            string strCateValue5 = "";

            string strCate1 = "0";
            string strCate2 = "0";
            string strCate3 = "0";
            string strCate4 = "0";
            string strCate5 = "0";

            string strRemark1 = "";
            string strRemark2 = "";
            string strRemark3 = "";
            string strRemark4 = "";

            string strDesc = string.Empty;

            string strBool1 = "0";
            string strBool2 = "0";
            string strBool3 = "0";
            string strBool4 = "0";

            string strTmp = "";

            long lngCateRoot1 = 1;
            long lngCateRoot2 = 1;
            long lngCateRoot3 = 1;
            long lngCateRoot4 = 1;
            long lngCateRoot5 = 1;


            DataTable dt1 = FlowModel.GetCommAppFlowModelFields(lngFlowModelID);

            DataRow dr1 = null;
            if (dt1.Rows.Count > 0)
                dr1 = dt1.Rows[0];

            if (dr1 != null)
            {
                lngCateRoot1 = long.Parse(dr1["Cate1RootID"].ToString());
                lngCateRoot2 = long.Parse(dr1["Cate2RootID"].ToString());
                lngCateRoot3 = long.Parse(dr1["Cate3RootID"].ToString());
                lngCateRoot4 = long.Parse(dr1["Cate4RootID"].ToString());
                lngCateRoot5 = long.Parse(dr1["Cate5RootID"].ToString());
            }





            XmlTextReader tr = new XmlTextReader(new StringReader(strValues));
            while (tr.Read())
            {
                if (tr.Name == "Field" && tr.NodeType == XmlNodeType.Element)
                {
                    strTmp = tr.GetAttribute("Value").Trim();
                    switch (tr.GetAttribute("FieldName"))
                    {

                        case "applyid":
                            if (strTmp != "")
                                lngApplyID = long.Parse(strTmp);
                            break;
                        case "applyname":
                            strApplyName = strTmp;
                            break;
                        case "deptid":
                            if (strTmp != "")
                                lngDeptID = long.Parse(strTmp);
                            break;
                        case "deptname":
                            strDeptName = strTmp;
                            break;
                        case "flowname":
                            strFlowName = strTmp;
                            break;
                        case "startdate":
                            strStartDate = strTmp;
                            break;
                        case "enddate":
                            strEndDate = strTmp;
                            break;
                        case "date1":
                            strDate1 = strTmp;
                            break;
                        case "date2":
                            strDate2 = strTmp;
                            break;
                        case "date3":
                            strDate3 = strTmp;
                            break;
                        case "date4":
                            strDate4 = strTmp;
                            break;
                        case "date5":
                            strDate5 = strTmp;
                            break;
                        case "date6":
                            strDate6 = strTmp;
                            break;
                        case "date7":
                            strDate7 = strTmp;
                            break;
                        case "date8":
                            strDate8 = strTmp;
                            break;
                        case "string1":
                            strString1 = strTmp;
                            break;
                        case "string2":
                            strString2 = strTmp;
                            break;
                        case "string3":
                            strString3 = strTmp;
                            break;
                        case "string4":
                            strString4 = strTmp;

                            break;
                        case "string5":
                            strString5 = strTmp;
                            break;
                        case "string6":
                            strString6 = strTmp;
                            break;
                        case "string7":
                            strString7 = strTmp;
                            break;
                        case "string8":
                            strString8 = strTmp;

                            break;
                        case "number1":
                            strNumber1 = strTmp;
                            if (strNumber1 == "")
                            {
                                strNumber1 = "0";
                            }
                            break;
                        case "number2":
                            strNumber2 = strTmp;
                            if (strNumber2 == "")
                            {
                                strNumber2 = "0";
                            }
                            break;
                        case "number3":
                            strNumber3 = strTmp;
                            if (strNumber3 == "")
                            {
                                strNumber3 = "0";
                            }
                            break;
                        case "number4":
                            strNumber4 = strTmp;
                            if (strNumber4 == "")
                            {
                                strNumber4 = "0";
                            }
                            break;
                        case "number5":
                            strNumber5 = strTmp;
                            if (strNumber5 == "")
                            {
                                strNumber5 = "0";
                            }
                            break;
                        case "bool1":
                            strBool1 = strTmp;
                            if (strBool1 == "")
                            {
                                strBool1 = "0";
                            }
                            break;
                        case "bool2":
                            strBool2 = strTmp;
                            if (strBool2 == "")
                            {
                                strBool2 = "0";
                            }
                            break;
                        case "bool3":
                            strBool3 = strTmp;
                            if (strBool3 == "")
                            {
                                strBool3 = "0";
                            }
                            break;
                        case "bool4":
                            strBool4 = strTmp;
                            if (strBool4 == "")
                            {
                                strBool4 = "0";
                            }
                            break;
                        case "cate1":
                            strCate1 = strTmp;
                            if (strCate1 == "")
                            {
                                strCate1 = "0";
                            }
                            if (strCate1 != "0")
                            {
                                strCateValue1 = CatalogDP.GetFullCatalogName(long.Parse(strCate1), lngCateRoot1);
                            }
                            break;
                        case "cate2":
                            strCate2 = strTmp;
                            if (strCate2 == "")
                            {
                                strCate2 = "0";
                            }
                            if (strCate2 != "0")
                            {
                                strCateValue2 = CatalogDP.GetFullCatalogName(long.Parse(strCate2), lngCateRoot2);
                            }
                            break;
                        case "cate3":
                            strCate3 = strTmp;
                            if (strCate3 == "")
                            {
                                strCate3 = "0";
                            }
                            if (strCate3 != "0")
                            {
                                strCateValue3 = CatalogDP.GetFullCatalogName(long.Parse(strCate3), lngCateRoot3);
                            }
                            break;
                        case "cate4":
                            strCate4 = strTmp;
                            if (strCate4 == "")
                            {
                                strCate4 = "0";
                            }
                            if (strCate4 != "0")
                            {
                                strCateValue4 = CatalogDP.GetFullCatalogName(long.Parse(strCate4), lngCateRoot4);
                            }
                            break;
                        case "cate5":
                            strCate5 = strTmp;
                            if (strCate5 == "")
                            {
                                strCate5 = "0";
                            }
                            if (strCate5 != "0")
                            {
                                strCateValue5 = CatalogDP.GetFullCatalogName(long.Parse(strCate5), lngCateRoot5);
                            }
                            break;

                        case "remark1":
                            strRemark1 = strTmp;
                            break;
                        case "remark2":
                            strRemark2 = strTmp;
                            break;
                        case "remark3":
                            strRemark3 = strTmp;
                            break;
                        case "remark4":
                            strRemark4 = strTmp;
                            break;
                        case "description":
                            strDesc = strTmp;
                            break;
                        default:
                            break;
                    }
                }
            }

            tr.Close();

            strSQL = "SELECT id FROM App_pub_Normal_Head WHERE FlowID=" + lngFlowID.ToString();
            dr = OracleDbHelper.ExecuteReader(trans, CommandType.Text, strSQL);
            while (dr.Read())
            {
                lngNextID = (long)dr.GetDecimal(0);
                break;
            }
            dr.Close();
            try
            {

                if (lngNextID == 0)
                {
                    //新增
                    lngNextID = EPGlobal.GetNextID("APP_pub_Normal_ID");
                    strSQL = "INSERT INTO App_pub_Normal_Head (ID,FlowID,FlowStatus,FlowModelID,ApplyID,ApplyName,DeptID,DeptName,FlowName,StartDate,EndDate,date1,date2,date3,date4,date5,date6,date7,date8," +
                           "string1,string2,string3,string4,string5,string6,string7,string8,number1,number2,number3,number4,number5,bool1,bool2,bool3,bool4,remark1,remark2,remark3,remark4," +
                           "cate1,cate2,cate3,cate4,cate5,catevalue1,catevalue2,catevalue3,catevalue4,catevalue5)" +
                        " VALUES( " +
                        lngNextID.ToString() + "," +
                        lngFlowID.ToString() + "," +
                        lngNodeModelID.ToString() + "," +
                        lngFlowModelID.ToString() + "," +
                        lngApplyID.ToString() + "," +
                        MyGlobalString.SqlQ(strApplyName) + "," +
                        lngDeptID.ToString() + "," +
                        MyGlobalString.SqlQ(strDeptName) + "," +
                        MyGlobalString.SqlQ(strFlowName) + "," +
                        "to_date(" + MyGlobalString.EmptyToNullDate(strStartDate) + ",'yyyy-MM-dd HH24:mi:ss')," +
                        "to_date(" + MyGlobalString.EmptyToNullDate(strEndDate) + ",'yyyy-MM-dd HH24:mi:ss')," +
                        "to_date(" + MyGlobalString.EmptyToNullDate(strDate1) + ",'yyyy-MM-dd HH24:mi:ss')," +
                        "to_date(" + MyGlobalString.EmptyToNullDate(strDate2) + ",'yyyy-MM-dd HH24:mi:ss')," +
                        "to_date(" + MyGlobalString.EmptyToNullDate(strDate3) + ",'yyyy-MM-dd HH24:mi:ss')," +
                        "to_date(" + MyGlobalString.EmptyToNullDate(strDate4) + ",'yyyy-MM-dd HH24:mi:ss')," +
                        "to_date(" + MyGlobalString.EmptyToNullDate(strDate5) + ",'yyyy-MM-dd HH24:mi:ss')," +
                        "to_date(" + MyGlobalString.EmptyToNullDate(strDate6) + ",'yyyy-MM-dd HH24:mi:ss')," +
                        "to_date(" + MyGlobalString.EmptyToNullDate(strDate7) + ",'yyyy-MM-dd HH24:mi:ss')," +
                        "to_date(" + MyGlobalString.EmptyToNullDate(strDate8) + ",'yyyy-MM-dd HH24:mi:ss')," +
                        MyGlobalString.SqlQ(strString1) + "," +
                        MyGlobalString.SqlQ(strString2) + "," +
                        MyGlobalString.SqlQ(strString3) + "," +
                        MyGlobalString.SqlQ(strString4) + "," +
                        MyGlobalString.SqlQ(strString5) + "," +
                        MyGlobalString.SqlQ(strString6) + "," +
                        MyGlobalString.SqlQ(strString7) + "," +
                        MyGlobalString.SqlQ(strString8) + "," +
                        strNumber1 + "," +
                        strNumber2 + "," +
                        strNumber3 + "," +
                        strNumber4 + "," +
                        strNumber5 + "," +
                        strBool1 + "," +
                        strBool2 + "," +
                        strBool3 + "," +
                        strBool4 + "," +
                        MyGlobalString.SqlQ(strRemark1) + "," +
                        MyGlobalString.SqlQ(strRemark2) + "," +
                        MyGlobalString.SqlQ(strRemark3) + "," +
                        MyGlobalString.SqlQ(strRemark4) + "," +
                        strCate1 + "," +
                        strCate2 + "," +
                        strCate3 + "," +
                        strCate4 + "," +
                        strCate5 + "," +
                        MyGlobalString.SqlQ(strCateValue1) + "," +
                        MyGlobalString.SqlQ(strCateValue2) + "," +
                        MyGlobalString.SqlQ(strCateValue3) + "," +
                        MyGlobalString.SqlQ(strCateValue4) + "," +
                        MyGlobalString.SqlQ(strCateValue5) +                         
                        ")";
                }
                else
                {
                    //更新
                    strSQL = "UPDATE App_pub_Normal_Head SET " +
                        " FlowStatus=" + lngNodeModelID.ToString() + "," +
                        " FlowModelID=" + lngFlowModelID.ToString() + "," +
                        //" ApplyID=" + lngApplyID.ToString() + "," +
                        //" ApplyName=" + MyGlobalString.SqlQ(strApplyName) + "," +
                        //" DeptID=" + lngDeptID.ToString() + "," +
                        //" DeptName=" +  MyGlobalString.SqlQ(strDeptName) + "," +
                        " FlowName=" + MyGlobalString.SqlQ(strFlowName) + "," +
                        " StartDate=to_date(" + MyGlobalString.EmptyToNullDate(strStartDate) + ",'yyyy-MM-dd HH24:mi:ss')," +
                        " EndDate=to_date(" + MyGlobalString.EmptyToNullDate(strEndDate) + ",'yyyy-MM-dd HH24:mi:ss')," +
                        " Date1=to_date(" + MyGlobalString.EmptyToNullDate(strDate1) + ",'yyyy-MM-dd HH24:mi:ss')," +
                        " Date2=to_date(" + MyGlobalString.EmptyToNullDate(strDate2) + ",'yyyy-MM-dd HH24:mi:ss')," +
                        " Date3=to_date(" + MyGlobalString.EmptyToNullDate(strDate3) + ",'yyyy-MM-dd HH24:mi:ss')," +
                        " Date4=to_date(" + MyGlobalString.EmptyToNullDate(strDate4) + ",'yyyy-MM-dd HH24:mi:ss')," +
                        " Date5=to_date(" + MyGlobalString.EmptyToNullDate(strDate5) + ",'yyyy-MM-dd HH24:mi:ss')," +
                        " Date6=to_date(" + MyGlobalString.EmptyToNullDate(strDate6) + ",'yyyy-MM-dd HH24:mi:ss')," +
                        " Date7=to_date(" + MyGlobalString.EmptyToNullDate(strDate7) + ",'yyyy-MM-dd HH24:mi:ss')," +
                        " Date8=to_date(" + MyGlobalString.EmptyToNullDate(strDate8) + ",'yyyy-MM-dd HH24:mi:ss')," +
                        " String1=" + MyGlobalString.SqlQ(strString1) + "," +
                        " String2=" + MyGlobalString.SqlQ(strString2) + "," +
                        " String3=" + MyGlobalString.SqlQ(strString3) + "," +
                        " String4=" + MyGlobalString.SqlQ(strString4) + "," +
                        " String5=" + MyGlobalString.SqlQ(strString5) + "," +
                        " String6=" + MyGlobalString.SqlQ(strString6) + "," +
                        " String7=" + MyGlobalString.SqlQ(strString7) + "," +
                        " String8=" + MyGlobalString.SqlQ(strString8) + "," +
                        " Number1=" + strNumber1 + "," +
                        " Number2=" + strNumber2 + "," +
                        " Number3=" + strNumber3 + "," +
                        " Number4=" + strNumber4 + "," +
                        " Number5=" + strNumber5 + "," +
                        " Bool1=" + strBool1 + "," +
                        " Bool2=" + strBool2 + "," +
                        " Bool3=" + strBool3 + "," +
                        " Bool4=" + strBool4 + "," +
                        " Cate1=" + strCate1 + "," +
                        " Cate2=" + strCate2 + "," +
                        " Cate3=" + strCate3 + "," +
                        " Cate4=" + strCate4 + "," +
                        " Cate5=" + strCate5 + "," +
                        " CateValue1=" + MyGlobalString.SqlQ(strCateValue1) + "," +
                        " CateValue2=" + MyGlobalString.SqlQ(strCateValue2) + "," +
                        " CateValue3=" + MyGlobalString.SqlQ(strCateValue3) + "," +
                        " CateValue4=" + MyGlobalString.SqlQ(strCateValue4) + "," +
                        " CateValue5=" + MyGlobalString.SqlQ(strCateValue5) + "," +
                        " Remark1=" + MyGlobalString.SqlQ(strRemark1) + "," +
                        " Remark2=" + MyGlobalString.SqlQ(strRemark2) + "," +
                        " Remark3=" + MyGlobalString.SqlQ(strRemark3) + "," +
                        " Remark4=" + MyGlobalString.SqlQ(strRemark4) +                         
                        "	WHERE ID = " + lngNextID.ToString();
                }
                OracleDbHelper.ExecuteNonQuery(trans, strSQL);

                #region 解决字符超长出错 yxq 2014-05-19
                strSQL = "update App_pub_Normal_Head set description=:a where FlowID = " + lngFlowID;
                OracleCommand cmdCST = new OracleCommand(strSQL, trans.Connection, trans);
                cmdCST.Parameters.Add("a", OracleType.Clob).Value = strDesc;
                cmdCST.ExecuteNonQuery();
                #endregion

            }
            catch
            {
                throw;
            }


        }


        /// <summary>
        /// 退回时应用执行的具体实现的接口
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngMessageID"></param>
        public override void SendBackUserProcess(OracleTransaction trans, long lngFlowID, long lngMessageID)
        {

        }


        /// <summary>
        /// 删除应用相关的信息。（当前应用仅一个OP）
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngOpID"></param>
        public override void DeleteFieldValues(OracleTransaction trans, long lngFlowID, long lngOpID)
        {
            string strSQL = "";
            try
            {
                strSQL = "DELETE App_pub_Normal_Head WHERE FlowID = " + lngFlowID.ToString();
                OracleDbHelper.ExecuteNonQuery(trans, strSQL);
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// 统一的归档处理接口
        /// </summary>
        /// <param name="lngID">流程ID</param>
        public override void DoFlowEnd(OracleTransaction trans, long lngID)
        {
            string strSQL = "";
            try
            {
                strSQL = "UPDATE App_pub_Normal_Head SET EndDate = sysdate WHERE FlowID = " + lngID.ToString();
                OracleDbHelper.ExecuteNonQuery(trans, strSQL);
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// 统一的归档处理接口
        /// </summary>
        /// <param name="lngID">流程ID</param>
        public override void DoFlowAbort(OracleTransaction trans, long lngID)
        {
            string strSQL = "";
            try
            {
                strSQL = "UPDATE App_pub_Normal_Head SET EndDate = sysdate WHERE FlowID = " + lngID.ToString();
                OracleDbHelper.ExecuteNonQuery(trans, strSQL);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 新增和发送时产生MESSAGE时,用户自定义处理接口
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngMessageID"></param>
        public override void AfterMessageAddedForAddOrSend(OracleTransaction trans, long lngFlowID, long lngNodeID, long lngMessageID, int intActorType, string sFActor)
        {

        }

        /// <summary>
        /// 删除消息（协办/阅知/转发/传阅/沟通【非主办】）用户接口
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngMessageID"></param>
        public override void DeleteMessage(OracleTransaction trans, long lngMessageID)
        {
        }

        /// <summary>
        ///  实现用户自定义二次开发 接收人员 结果
        ///           如:  自动分配 (根据工作量)
        ///                从表单上提取  等
        /// </summary>
        /// <param name="lngAppID"></param>
        /// <param name="lngOpID"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngNodeID"></param>
        /// <param name="lngMessageID"></param>
        /// <param name="lngFlowModelID"></param>
        /// <param name="lngNodeModelID"></param>
        /// <param name="lngUserID"></param>
        /// <param name="strFormXMLValue"></param>
        /// <param name="xmlDoc"></param>
        public override void UserInterfaceReceivers(long lngAppID, long lngOpID, long lngFlowID, long lngNodeID, long lngMessageID, long lngFlowModelID, long lngNodeModelID, long lngUserID, string strFormXMLValue, ref XmlDocument xmlDoc)
        {

        }


        /// <summary>
        /// 用户接收事项后自定义处理
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngMessageID"></param>
        public override void AfterReceiveMessage(OracleTransaction trans, long lngMessageID, long lngUserID)
        {

        }

        /// <summary>
        /// 判断会签环节是否可以结束
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngMessageID"></param>
        /// <returns></returns>
        public override bool IsInfluxMessageFinished(OracleTransaction trans, long lngMessageID)
        {
            return false;
        }


    }
}
