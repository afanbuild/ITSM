/*******************************************************************
 *
 * Description:服务级别处理
 * 
 * 
 * Create By  :
 * Create Date:2008年7月30日
 * *****************************************************************/
using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using EpowerGlobal;

namespace Epower.ITSM.SqlDAL
{
    public class FlowBusLimitDP
    {
        public FlowBusLimitDP()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}


        /// <summary>
        /// 流程恢复执行时依据服务级别设置,重新计算并保存相关业务时限
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngLevelID"></param>
        /// <param name="sBaseTime"></param>
        /// <param name="dtPauseTime">流程暂停的时间</param>
        /// <param name="dtContTime">流程恢复时间</param>
        public static void ReCalLevelLimitForPause(OracleTransaction trans, long lngFlowID, long lngLevelID, string sBaseTime,DateTime dtPauseTime,DateTime dtContTime)
        {
            string strSQL = "";

            if (sBaseTime == "")
            {
                //如果不存在发生时间，则退出
                return;
            }
            try
            {
                decimal dMinute = EpowerGlobal.EPGlobal.CalculateSpendWorkTime( dtPauseTime,dtContTime, -1, -1, -1, 3);  //计算暂停时间段的时间，分钟
                decimal dMinuteWork = EpowerGlobal.EPGlobal.CalculateSpendWorkTime(dtPauseTime,dtContTime, -1, -1, -1, 1);  //计算暂停时间段的时间，工分
                

                //重新计算
                OracleConnection cn = ConfigTool.GetConnection();
                strSQL = "SELECT timeunit,limittime,guidid from ea_flowbuslimit  WHERE limittime is not null AND flowid =  " + lngFlowID.ToString();
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                ConfigTool.CloseConnection(cn);
                foreach (DataRow row in dt.Rows)
                {
                    string sLimitTime = sBaseTime;
                    DateTime dtOldTime = (DateTime)row["limittime"];
                    int iTimeUnit = int.Parse(row["timeunit"].ToString());
                    decimal dGuid = decimal.Parse(row["guidid"].ToString());

                
                    switch (iTimeUnit)
                    {
                        case 0:
                        case 1:
                        case 2:
                            //天 //分钟 //小时
                            sLimitTime = " dateadd('minute'," + dMinute.ToString() + ",limittime) ";
                            break;
                        case 3:
                        case 4:
                            //工时 和 工时
                            sLimitTime = "to_date('" + EpowerCom.MessageDep.AddWorkTimeMinutes(dtOldTime, (int)dMinuteWork, -1, -1, -1) + "','yyyy-MM-dd HH24:mi:ss')";
                            break;
                        default:
                            break;

                    }
                    strSQL = "UPDATE Ea_FlowBusLimit SET limittime = " + sLimitTime + " WHERE flowid =" + lngFlowID.ToString() + " AND guidid = " + dGuid.ToString();
                    OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);

                }





            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
            finally
            {
            }

        }
		

		/// <summary>
        /// 依据服务级别设置,保存相关业务时限
		/// </summary>
		/// <param name="trans"></param>
		/// <param name="lngFlowID"></param>
		/// <param name="lngLevelID"></param>
		/// <param name="sBaseTime"></param>
		public static void SaveLevelLimit(OracleTransaction trans, long lngFlowID,long lngLevelID,string sBaseTime)
		{
            string strSQL = "";
            if (sBaseTime == "")
                sBaseTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm");
            try
            {
                strSQL = "DELETE ea_flowbuslimit WHERE flowid =" + lngFlowID.ToString();
                OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);
                //工时和工分 只计算了 timelimit
//                strSQL = @"INSERT into Ea_FlowBusLimit SELECT " + lngFlowID.ToString() + @" as flowid,guidid,case timeunit 
//                            when 1 then timelimit * 60 
//                            when 2 then timelimit * 1440
//                            else timelimit  end as Limit,timeunit,case timeunit 
//                            when 0 then dateadd('minute',timelimit,to_date(" + StringTool.SqlQ(sBaseTime) + @",'yyyy-MM-dd HH24:mi:ss')) 
//                            when 1 then dateadd('minute',timelimit * 60,to_date(" + StringTool.SqlQ(sBaseTime) + @",'yyyy-MM-dd HH24:mi:ss')) 
//                            when 2 then dateadd('minute',timelimit * 1440,to_date(" + StringTool.SqlQ(sBaseTime) + @",'yyyy-MM-dd HH24:mi:ss')) 
//                            else sysdate  end as LimitTime
//                                       FROM cst_slguid WHERE levelid =" + lngLevelID.ToString();
               
                strSQL = @"INSERT into Ea_FlowBusLimit SELECT " + lngFlowID.ToString() + @" as flowid,guidid,
                            timelimit as Limit,
                            timeunit,
                            case timeunit 
                            when 0 then dateadd('minute',timelimit,to_date(" + StringTool.SqlQ(sBaseTime) + @",'yyyy-MM-dd HH24:mi:ss')) 
                            when 1 then dateadd('minute',timelimit * 60,to_date(" + StringTool.SqlQ(sBaseTime) + @",'yyyy-MM-dd HH24:mi:ss')) 
                            when 2 then dateadd('minute',timelimit * 1440,to_date(" + StringTool.SqlQ(sBaseTime) + @",'yyyy-MM-dd HH24:mi:ss')) 
                            else sysdate  end as LimitTime
                                       FROM cst_slguid WHERE levelid =" + lngLevelID.ToString();

                OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);
                
                //单独计算工时 工分
                OracleConnection cn = ConfigTool.GetConnection();
                strSQL = "SELECT timelimit,timeunit,guidid FROM cst_slguid WHERE levelid = " + lngLevelID.ToString() + " AND timeunit >= 2";
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                ConfigTool.CloseConnection(cn);
                foreach(DataRow row in dt.Rows)
                {
                    string sLimitTime = sBaseTime;
                    int iTimeUnit = int.Parse(row[1].ToString());
                    int iLimit = int.Parse(row[0].ToString());
                    decimal dGuid = decimal.Parse(row[2].ToString());

                    if (iTimeUnit == 3)
                    {
                        //工分
                        sLimitTime = AddWorkTimeMinutes(sBaseTime, iLimit);
                    }
                    else if (iTimeUnit == 4)
                    {
                        //工时
                        sLimitTime = AddWorkTimeMinutes(sBaseTime, iLimit * 60);
                    }
                    else if (iTimeUnit == 2)
                    {
                        //天
                        sLimitTime = AddWorkTimeMinutes(sBaseTime, iLimit * 60 * GetWorkMinute());
                    }

                    //if (iTimeUnit == 3)
                    //{
                    //    //工分
                    //    sLimitTime = AddWorkTimeMinutes(sBaseTime, iLimit);
                    //}
                    //else
                    //{
                    //    //工时
                    //    sLimitTime = AddWorkTimeMinutes(sBaseTime, iLimit * 60);
                    //}
                   // strSQL = "UPDATE Ea_FlowBusLimit SET limittime = " + StringTool.SqlQ(sLimitTime) + " WHERE flowid =" + lngFlowID.ToString() + " AND guidid = " + dGuid.ToString();
                    strSQL = "UPDATE Ea_FlowBusLimit SET limittime = to_date(" + StringTool.SqlQ(sLimitTime) + ",'yyyy-MM-dd HH24:mi:ss') WHERE flowid =" + lngFlowID.ToString() + " AND guidid = " + dGuid.ToString();
                    OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);

                }


                
                

            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
            finally
            {
            }

		}

        /// <summary>
        /// 计算工作日的有效工作时间
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static int GetWorkMinute()
        {
            string strTmp = "";
            string[] arr;
            int onDutyHour1 = 0;
            int onDutyMinute1 = 0;

            int offDutyHour1 = 0;
            int offDutyMinute1 = 0;

            int onDutyHour2 = 0;
            int onDutyMinute2 = 0;

            int offDutyHour2 = 0;
            int offDutyMinute2 = 0;

            int dayMinute = 0;
            int amMinute = 0;    //上午时间
            int pmMinute = 0;     //下午时间
            bool isAMTime = false;  //是否为上午上班时间

            strTmp = CommonDP.GetConfigValue("WorkTimeType", "OnDutyTime1");
            arr = strTmp.Split(":".ToCharArray());
            onDutyHour1 = int.Parse(arr[0]);
            onDutyMinute1 = int.Parse(arr[1]);

            strTmp = CommonDP.GetConfigValue("WorkTimeType", "OffDutyTime1");
            arr = strTmp.Split(":".ToCharArray());
            offDutyHour1 = int.Parse(arr[0]);
            offDutyMinute1 = int.Parse(arr[1]);

            strTmp = CommonDP.GetConfigValue("WorkTimeType", "OnDutyTime2");
            arr = strTmp.Split(":".ToCharArray());
            onDutyHour2 = int.Parse(arr[0]);
            onDutyMinute2 = int.Parse(arr[1]);

            strTmp = CommonDP.GetConfigValue("WorkTimeType", "OffDutyTime2");
            arr = strTmp.Split(":".ToCharArray());
            offDutyHour2 = int.Parse(arr[0]);
            offDutyMinute2 = int.Parse(arr[1]);

            amMinute = offDutyHour1 * 60 + offDutyMinute1 - (onDutyHour1 * 60 + onDutyMinute1);
            pmMinute = offDutyHour2 * 60 + offDutyMinute2 - (onDutyHour2 * 60 + onDutyMinute2);
            dayMinute = amMinute + pmMinute;
            return dayMinute / 60;
        }

        /// <summary>
        /// 计算工作时间(分钟)
        /// </summary>
        /// <param name="sBaseTime"></param>
        /// <param name="rmMinute"></param>
        /// <returns></returns>
        public static string AddWorkTimeMinutes(string sBaseTime,long rmMinute)
        {
            DateTime bdate = DateTime.Parse(sBaseTime);
            DateTime edate;
            e_WorkTimeType wtype = (e_WorkTimeType)(int.Parse(CommonDP.GetConfigValue("WorkTimeType", "WorkTimeType")));
            string strTmp = "";
            string[] arr;
            int onDutyHour1 = 0;
            int onDutyMinute1 = 0;

            int offDutyHour1 = 0;
            int offDutyMinute1 = 0;

            int onDutyHour2 = 0;
            int onDutyMinute2 = 0;

            int offDutyHour2 = 0;
            int offDutyMinute2 = 0;



            int dayMinute = 0;

            int hasWorkMinute = 0;
            int rmDayMinute = 0;
            int amMinute = 0;    //上午时间
            int pmMinute = 0;     //下午时间
            bool isAMTime = false;  //是否为上午上班时间

            strTmp = CommonDP.GetConfigValue("WorkTimeType", "OnDutyTime1");
            arr = strTmp.Split(":".ToCharArray());
            onDutyHour1 = int.Parse(arr[0]);
            onDutyMinute1 = int.Parse(arr[1]);

            strTmp = CommonDP.GetConfigValue("WorkTimeType", "OffDutyTime1");
            arr = strTmp.Split(":".ToCharArray());
            offDutyHour1 = int.Parse(arr[0]);
            offDutyMinute1 = int.Parse(arr[1]);

            strTmp = CommonDP.GetConfigValue("WorkTimeType", "OnDutyTime2");
            arr = strTmp.Split(":".ToCharArray());
            onDutyHour2 = int.Parse(arr[0]);
            onDutyMinute2 = int.Parse(arr[1]);

            strTmp = CommonDP.GetConfigValue("WorkTimeType", "OffDutyTime2");
            arr = strTmp.Split(":".ToCharArray());
            offDutyHour2 = int.Parse(arr[0]);
            offDutyMinute2 = int.Parse(arr[1]);

            amMinute = offDutyHour1 * 60 + offDutyMinute1 - (onDutyHour1 * 60 + onDutyMinute1);
            pmMinute = offDutyHour2 * 60 + offDutyMinute2 - (onDutyHour2 * 60 + onDutyMinute2);
            if ((amMinute <= 0) || (pmMinute <= 0) ||
                (onDutyHour2 * 60 + onDutyMinute2 - (offDutyHour1 * 60 + offDutyMinute1) <= 0))
            {
                // 上午下班 小于 上午上班  || 下午下班小于下午上班  || 下午上班小于上午下班
                throw new Exception(@"工作时间设置不正确！请重新设置");
            }

            dayMinute = amMinute + pmMinute;

            if (dayMinute <= 0)
            {
                throw new Exception(@"工作时间设置不正确！请重新设置");
            }

            //如果当前时间为非工作时间，则从最近的工作时间开始计算
            if ((bdate.Hour * 60 + bdate.Minute < onDutyHour1 * 60 + onDutyMinute1))
            {
                //上午上班前
                edate = new DateTime(bdate.Year, bdate.Month, bdate.Day, onDutyHour1, onDutyMinute1, 0);
            }
            else if ((bdate.Hour * 60 + bdate.Minute > offDutyHour1 * 60 + offDutyMinute1) && ((bdate.Hour * 60 + bdate.Minute < onDutyHour2 * 60 + onDutyMinute2)))
            {
                //午休时间
                edate = new DateTime(bdate.Year, bdate.Month, bdate.Day, onDutyHour2, onDutyMinute2, 0);
            }
            else if ((bdate.Hour * 60 + bdate.Minute > offDutyHour2 * 60 + offDutyMinute2))
            {
                //下午下班之后  (下一个工作日的上班时间开始计算)
                edate = new DateTime(bdate.Year, bdate.Month, bdate.Day, onDutyHour1, onDutyMinute1, 0);
                edate = EPGlobal.CaluateNextWorkDay(wtype, edate, true, -1, -1, -1);
            }
            else
            {
                edate = bdate;
            }

            if (edate.Hour * 60 + edate.Minute <= offDutyHour1 * 60 + offDutyMinute1)
            {
                isAMTime = true;
            }


            //此时已经定位在正常的工作时段，判断是否为正常工作日
            if (((wtype == e_WorkTimeType.ewttFive && (edate.DayOfWeek == DayOfWeek.Saturday || edate.DayOfWeek == DayOfWeek.Sunday)) ||
                (wtype == e_WorkTimeType.ewttSix && edate.DayOfWeek == DayOfWeek.Sunday) ||
                (wtype == e_WorkTimeType.ewttFiveHalf && ((edate.DayOfWeek == DayOfWeek.Saturday && isAMTime == false) || edate.DayOfWeek == DayOfWeek.Sunday)))
                || EPGlobal.IsHoliday(edate, -1, -1, -1) == true)
            {
                // 5天工作日 时的星期六和星期天  6天时的星期天， 5天半时的星期六下午和星期天 ，取下一个工作日的最早上班时间
                edate = new DateTime(edate.Year, edate.Month, edate.Day, onDutyHour1, onDutyMinute1, 0);
                edate = EPGlobal.CaluateNextWorkDay(wtype, edate, isAMTime,-1,-1,-1);
            }

            while (rmMinute > 0)
            {
                if (rmMinute > dayMinute)
                {
                    //计算下一个工作日


                    if (edate.DayOfWeek == DayOfWeek.Friday && wtype == e_WorkTimeType.ewttFiveHalf && isAMTime == false)
                    {
                        //只 5天半工作日的时候出现星期五，并且是下午，改为从第二天上午开始
                        edate = edate.AddDays(1);
                        rmMinute = rmMinute - (offDutyHour2 * 60 + offDutyMinute2 - (edate.Hour * 60 + edate.Minute));
                        edate = new DateTime(edate.Year, edate.Month, edate.Day, onDutyHour1, onDutyMinute1, 0);
                        isAMTime = true;
                    }
                    else
                    {
                        if (edate.DayOfWeek == DayOfWeek.Saturday && wtype == e_WorkTimeType.ewttFiveHalf)
                        {
                            rmMinute -= amMinute;
                        }
                        else
                        {
                            rmMinute -= dayMinute;
                        }
                        edate = EPGlobal.CaluateNextWorkDay(wtype, edate, isAMTime, -1, -1, -1);
                    }

                }
                else
                {
                    //计算当前时间对应当天已经上班的分钟数(此时用来计算的时间肯定为正常的上班时间)
                    if (edate.Hour * 60 + edate.Minute >= onDutyHour2 * 60 + onDutyMinute2)
                    {
                        //下午上班时间
                        hasWorkMinute = offDutyHour1 * 60 + offDutyMinute1 - (onDutyHour1 * 60 + onDutyMinute1) + (edate.Hour * 60 + edate.Minute - (onDutyHour2 * 60 + onDutyMinute2));
                        if (dayMinute - hasWorkMinute < rmMinute)
                        {
                            //当前剩余时间小于总共剩余的时间，从第二天上午上班开始计算

                            if (edate.DayOfWeek == DayOfWeek.Friday && wtype == e_WorkTimeType.ewttFiveHalf && isAMTime == false)
                            {
                                //只 5天半工作日的时候出现星期五，并且是下午，改为从第二天上午开始
                                edate = edate.AddDays(1);
                                rmMinute = rmMinute - (offDutyHour2 * 60 + offDutyMinute2 - (edate.Hour * 60 + edate.Minute));
                                edate = new DateTime(edate.Year, edate.Month, edate.Day, onDutyHour1, onDutyMinute1, 0);
                                isAMTime = true;
                            }
                            else
                            {
                                edate = new DateTime(edate.Year, edate.Month, edate.Day, onDutyHour1, onDutyMinute1, 0);
                                edate = EPGlobal.CaluateNextWorkDay(wtype, edate, isAMTime, -1, -1, -1);
                                rmMinute = rmMinute - dayMinute + hasWorkMinute;
                            }
                        }
                        else
                        {
                            //直接加上剩余的分钟即可
                            edate = edate.AddMinutes(rmMinute);
                            rmMinute = 0;
                        }
                    }
                    else
                    {
                        //上午上班时间
                        hasWorkMinute = edate.Hour * 60 + edate.Minute - (onDutyHour1 * 60 + onDutyMinute1);
                        if (dayMinute - hasWorkMinute < rmMinute)
                        {
                            //当前剩余时间小于总共剩余的时间，从下一个工作日上午上班开始计算
                            if (edate.DayOfWeek == DayOfWeek.Friday && wtype == e_WorkTimeType.ewttFiveHalf && isAMTime == false)
                            {
                                //只 5天半工作日的时候出现星期五，并且是下午，改为从第二天上午开始
                                edate = edate.AddDays(1);
                                rmMinute = rmMinute - (offDutyHour2 * 60 + offDutyMinute2 - (edate.Hour * 60 + edate.Minute));
                                edate = new DateTime(edate.Year, edate.Month, edate.Day, onDutyHour1, onDutyMinute1, 0);
                                isAMTime = true;
                            }
                            else
                            {
                                edate = new DateTime(edate.Year, edate.Month, edate.Day, onDutyHour1, onDutyMinute1, 0);
                                edate = EPGlobal.CaluateNextWorkDay(wtype, edate, isAMTime, -1, -1, -1);
                                rmMinute = rmMinute - dayMinute + hasWorkMinute;
                            }
                        }
                        else
                        {
                            //计算上午剩余
                            rmDayMinute = offDutyHour1 * 60 + offDutyMinute1 - (edate.Hour * 60 + edate.Minute);
                            if (rmDayMinute < rmMinute)
                            {
                                if (edate.DayOfWeek == DayOfWeek.Saturday && wtype == e_WorkTimeType.ewttFiveHalf)
                                {
                                    //从下一个工作日的上午上班开始计算
                                    if (edate.DayOfWeek == DayOfWeek.Friday && wtype == e_WorkTimeType.ewttFiveHalf && isAMTime == false)
                                    {
                                        //只 5天半工作日的时候出现星期五，并且是下午，改为从第二天上午开始
                                        edate = edate.AddDays(1);
                                        rmMinute = rmMinute - (offDutyHour2 * 60 + offDutyMinute2 - (edate.Hour * 60 + edate.Minute));
                                        edate = new DateTime(edate.Year, edate.Month, edate.Day, onDutyHour1, onDutyMinute1, 0);
                                        isAMTime = true;
                                    }
                                    else
                                    {
                                        edate = EPGlobal.CaluateNextWorkDay(wtype, edate, isAMTime, -1, -1, -1);
                                        edate = new DateTime(edate.Year, edate.Month, edate.Day, onDutyHour1, onDutyMinute1, 0);
                                        isAMTime = true;
                                    }
                                }
                                else
                                {
                                    //从当前下午上班开始计算
                                    edate = new DateTime(edate.Year, edate.Month, edate.Day, onDutyHour2, onDutyMinute2, 0);

                                }
                                rmMinute = rmMinute - rmDayMinute;
                            }
                            else
                            {
                                //直接加上剩余的分钟即可
                                edate = edate.AddMinutes(rmMinute);
                                rmMinute = 0;
                            }
                        }
                    }



                }
            }

            return edate.ToShortDateString() + " " + edate.ToLongTimeString();


        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="lngSenderID"></param>
        /// <param name="lngRecID"></param>
        /// <param name="strContent"></param>
		private static void AddSMS(long lngSenderID,long lngRecID,string strContent)
		{
			string strSQL = "";
			OracleConnection cn = ConfigTool.GetConnection();
			try
			{
				strSQL = "INSERT INTO OA_SMS (SenderID,ReceiverID,Content,SendTime,Deleted)" +
					" Values(" + 
					lngSenderID.ToString() + "," +
					lngRecID.ToString() + "," +
					StringTool.SqlQ(strContent) + ",sysdate,0" +
					")";

				OracleDbHelper.ExecuteNonQuery(cn,CommandType.Text,strSQL);

			}
			catch(Exception err)
			{
				throw new Exception(err.Message );
			}
			finally
			{
				ConfigTool.CloseConnection(cn);
			}

		}

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="lngSMSID"></param>
        /// <param name="strContent"></param>
		private static void UpdateSMS(long lngSMSID,string strContent)
		{
			string strSQL = "";
			OracleConnection cn = ConfigTool.GetConnection();
			try
			{
				strSQL = "UPDATE OA_sms SET " +
					" content = " +	StringTool.SqlQ(strContent) + 
					" WHERE smsid = " + lngSMSID.ToString();

				OracleDbHelper.ExecuteNonQuery(cn,CommandType.Text,strSQL);
			}
			catch(Exception err)
			{
				throw new Exception(err.Message );
			}
			finally
			{
				ConfigTool.CloseConnection(cn);
			}
		}


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="lngSMSID"></param>
		public static void DeleteSMS(long lngSMSID)
		{
			string strSQL = "";
			OracleConnection cn = ConfigTool.GetConnection();
			try
			{
				strSQL = "Update OA_sms  set deleted=1" +
					" WHERE smsid = " + lngSMSID.ToString();

				OracleDbHelper.ExecuteNonQuery(cn,CommandType.Text,strSQL);
			}
			catch(Exception err)
			{
				throw new Exception(err.Message );
			}
			finally
			{
				ConfigTool.CloseConnection(cn);
			}
		}

        /// <summary>
        /// 已读
        /// </summary>
        /// <param name="lngSMSID"></param>
        public static void ReadSMS(long lngSMSID)
        {
            string strSQL = "";
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                strSQL = "UPDATE OA_sms SET " +
                    " ReadStatus = 1" +
                    " WHERE smsid = " + lngSMSID.ToString();

                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSQL);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }

    }
}
