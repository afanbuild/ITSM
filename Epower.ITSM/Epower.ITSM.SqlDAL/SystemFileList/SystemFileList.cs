using System;
using System.Xml;
using System.Data;
using System.Collections;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;
using Epower.ITSM.SqlDAL.ES_TBLCS;
using System.Collections.Generic;
using System.Text;
using EpowerGlobal;
using System.IO;
using EpowerCom;

namespace Epower.ITSM.SqlDAL.SystemFileList
{
    public class SystemFile
    {

        #region 系统档案清单
        #region 查询系统档案清单
        /// <summary>
        /// 查询系统档案清单
        /// <summary>
        /// </summary>
        /// <param name="strWhere"></param>
        /// <param name="strOrder"></param>
        /// <returns></returns>
        public static DataTable GetSystemFileList(string strWhere, string strOrder)
        {
            string strSQL = string.Empty;
            strSQL = "SELECT * FROM Br_Systemfilelist Where 1=1 ";
            strSQL += strWhere;
            strSQL += strOrder;
            OracleConnection cn = ConfigTool.GetConnection();
            return OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
        }
        #endregion
        #region 分页查询系统档案清单
        /// <summary>
        /// 分页查询系统档案清单
        /// <summary>
        /// </summary>
        /// <param name="strWhere"></param>
        /// <param name="strOrder"></param>
        /// <returns></returns>
        public static DataTable GetSystemFileList(string strWhere, int pagesize, int pageindex, ref int rowcount)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "Br_Systemfilelist", "*", " ORDER BY ID DESC", pagesize, pageindex, strWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion
        #region 查询自动增长列

        /// <summary>
        /// 查询系统档案清单
        /// <summary>
        /// </summary>
        /// <param name="strWhere"></param>
        /// <param name="strOrder"></param>
        /// <returns></returns>
        public static DataTable GetSystemFileList()
        {
            string strSQL = string.Empty;
            strSQL = "select BR_SYSTEMFILELIST1.Nextval from dual";
            OracleConnection cn = ConfigTool.GetConnection();
            return OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
        }
        #endregion
        #region 添加系统档案清单
        /// <summary>
        /// 添加系统档案清单
        /// </summary>
        /// <param name="entity">系统档案清单</param>
        /// <param name="entity0">硬件</param>
        /// <param name="sentity">软件</param>
        /// <param name="entity1">系统灾备信息</param>
        /// <param name="entity2">系统模块分布信息</param>
        /// <param name="entity3">系统交易码清单</param>
        /// <param name="entity4">系统数据类信息</param>
        /// <param name="entity5">系统档案清单</param>
        public static string InsertSystemFileList(SystemFileEntity entity, List<HardwareEntity> entity0, List<SoftwareEntity> sentity, List<DisasterInformationEntity> entity1, List<ModuleInformationEntity> entity2, List<TransactionCodeEntity> entity3, List<DataInformationEntity> entity4, List<InterfaceListEntity> entity5)
        {
            DataTable dt = GetSystemFileList();
            if (dt.Rows.Count > 0)
            {
                entity.ID = dt.Rows[0][0].ToString();
            }
            string strSQL = string.Empty;
            strSQL = @"insert into Br_Systemfilelist(
                                   ID ,
                                   SystemName ,
                                   EGName ,
                                   SystemID,
                                   HomeName,
                                   Introduction,
                                   depetName,
                                   ManagerName ,
                                   BA ,
                                   ITManager ,
                                   SA ,
                                   DevelopmentMode ,
                                   Manufacturer ,
                                   State ,
                                   ProductionTime ,
                                   MaintenanceMode ,
                                   Outsourcing ,
                                   TradingVolume,
                                   PeakVolume ,
                                   HandleTime ,
                                   SystemAccount,
                                   CustomerNumber ,
                                   PhysicsUrl,
                                   ApplicationUrl
                                   )values("
                                   + StringTool.SqlQ(entity.ID) + ","
                                   + StringTool.SqlQ(entity.SystemName) + ","
                                   + StringTool.SqlQ(entity.EGName) + ","
                                   + StringTool.SqlQ(entity.SystemID) + ","
                                   + StringTool.SqlQ(entity.HomeName) + ","
                                   + StringTool.SqlQ(entity.Introduction) + ","
                                   + StringTool.SqlQ(entity.DepetName) + ","
                                   + StringTool.SqlQ(entity.ManagerName) + ","
                                   + StringTool.SqlQ(entity.BA) + ","
                                   + StringTool.SqlQ(entity.ITManager) + ","
                                   + StringTool.SqlQ(entity.SA) + ","
                                   + StringTool.SqlQ(entity.DevelopmentMode) + ","
                                   + StringTool.SqlQ(entity.Manufacturer) + ","
                                   + StringTool.SqlQ(entity.State) + ","
                                   + StringTool.SqlQ(entity.ProductionTime == "0001-1-1 0:00:00" ? "" : entity.ProductionTime) + ","
                                   + StringTool.SqlQ(entity.MaintenanceMode) + ","
                                   + StringTool.SqlQ(entity.Outsourcing) + ","

                                   //+ StringTool.SqlQ(entity.Middleware) + ","
                //+ StringTool.SqlQ(entity.MiddlewareEdition) + ","
                //+ StringTool.SqlQ(entity.Database) + ","
                //+ StringTool.SqlQ(entity.DatabaseEdition) + ","
                //+ StringTool.SqlQ(entity.Tools) + ","
                //+ StringTool.SqlQ(entity.ToolsEdition) + ","
                                   + entity.TradingVolume + ","
                                   + entity.PeakVolume + ","
                                   + entity.HandleTime + ","
                                   + entity.SystemAccount + ","
                                   + entity.CustomerNumber + ","
                                   + StringTool.SqlQ(entity.PhysicsUrl) + ","
                                   + StringTool.SqlQ(entity.ApplicationUrl) +
                                   ")";
            OracleConnection cn = ConfigTool.GetConnection();
            cn.Open();
            OracleTransaction trans = cn.BeginTransaction();
            try
            {
                OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);
                foreach (HardwareEntity hentity in entity0)
                {
                    hentity.SystemId = entity.ID;
                    InsertHardware(hentity, trans);
                }
                foreach (SoftwareEntity sentity1 in sentity)
                {
                    sentity1.SystemId = entity.ID;
                    InsertSoftwareEnvironment(sentity1, trans);
                }
                foreach (DisasterInformationEntity dentity in entity1)
                {
                    dentity.SystemId = entity.ID;
                    InsertDisasterInformation(dentity, trans);
                }
                foreach (ModuleInformationEntity mentity in entity2)
                {
                    mentity.SystemId = entity.ID;
                    InsertModuleInformation(mentity, trans);
                }
                foreach (TransactionCodeEntity tentity in entity3)
                {
                    tentity.SystemId = entity.ID;
                    InsertTransactionCode(tentity, trans);
                }
                foreach (DataInformationEntity dientity in entity4)
                {
                    dientity.SystemId = entity.ID;
                    InsertDataInformation(dientity, trans);
                }
                foreach (InterfaceListEntity fentity in entity5)
                {
                    fentity.SystemId = entity.ID;
                    InsertInterfaceList(fentity, trans);
                }
                //保存附件
                SaveAttachments(trans, long.Parse(entity.ID), entity.PhysicsUrl);
                trans.Commit();
            }
            catch (Exception err)
            {
                trans.Rollback();
                throw new Exception(err.Message);
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
            return entity.ID;
        }
        #endregion
        #region 更新系统档案清单
        /// <summary>
        /// 更新系统档案清单
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="strWhere"></param>
        public static void UpdateSystemFileList(SystemFileEntity entity, List<HardwareEntity> entity0, List<SoftwareEntity> sentity, List<DisasterInformationEntity> entity1, List<ModuleInformationEntity> entity2, List<TransactionCodeEntity> entity3, List<DataInformationEntity> entity4, List<InterfaceListEntity> entity5, string strWhere)
        {
            string strSQL = "update Br_Systemfilelist set "
                            + " SystemName=" + StringTool.SqlQ(entity.SystemName) + ","
                            + " EGName=" + StringTool.SqlQ(entity.EGName) + ","
                            + " SystemID=" + StringTool.SqlQ(entity.SystemID) + ","
                            + " HomeName=" + StringTool.SqlQ(entity.HomeName) + ","
                            + " Introduction=" + StringTool.SqlQ(entity.Introduction) + ","
                            + " depetName=" + StringTool.SqlQ(entity.DepetName) + ","
                            + " ManagerName=" + StringTool.SqlQ(entity.ManagerName) + ","
                            + " BA=" + StringTool.SqlQ(entity.BA) + ","
                            + " ITManager=" + StringTool.SqlQ(entity.ITManager) + ","
                            + " SA=" + StringTool.SqlQ(entity.SA) + ","
                            + " DevelopmentMode=" + StringTool.SqlQ(entity.DevelopmentMode) + ","
                            + " Manufacturer=" + StringTool.SqlQ(entity.Manufacturer) + ","
                            + " State=" + StringTool.SqlQ(entity.State) + ","
                            + " ProductionTime=" + StringTool.SqlQ(entity.ProductionTime) + ","
                            + " MaintenanceMode=" + StringTool.SqlQ(entity.MaintenanceMode) + ","
                            + " Outsourcing=" + StringTool.SqlQ(entity.Outsourcing) + ","
                //+ " Operation=" + StringTool.SqlQ(entity.Operation) + ","
                //+ " OperationEdition=" + StringTool.SqlQ(entity.OperationEdition) + ","
                //+ " Middleware=" + StringTool.SqlQ(entity.Middleware) + ","
                //+ " MiddlewareEdition=" + StringTool.SqlQ(entity.MiddlewareEdition) + ","
                //+ " Database=" + StringTool.SqlQ(entity.Database) + ","
                //+ " DatabaseEdition=" + StringTool.SqlQ(entity.DatabaseEdition) + ","
                //+ " Tools=" + StringTool.SqlQ(entity.Tools) + ","
                //+ " ToolsEdition=" + StringTool.SqlQ(entity.ToolsEdition) + ","
                            + " TradingVolume=" + entity.TradingVolume + ","
                            + " PeakVolume=" + entity.PeakVolume + ","
                            + " HandleTime=" + entity.HandleTime + ","
                            + " SystemAccount=" + entity.SystemAccount + ","
                            + " CustomerNumber=" + entity.CustomerNumber + ","
                            + "  PhysicsUrl =" + StringTool.SqlQ(entity.PhysicsUrl) + " ,"
                            + "  ApplicationUrl =" + StringTool.SqlQ(entity.ApplicationUrl)
                            + " where 1=1";
            strSQL += strWhere;
            OracleConnection cn = ConfigTool.GetConnection();
            cn.Open();
            OracleTransaction tran = cn.BeginTransaction();
            try
            {
                OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);
                DeleteHardware(entity.ID, tran);
                foreach (HardwareEntity hentity in entity0)
                {
                    hentity.SystemId = entity.ID;

                    UpdateHardware(hentity, tran);
                }
                DeleteSoftwareEnvironment(entity.ID, tran);
                foreach (SoftwareEntity sentity1 in sentity)
                {
                    sentity1.SystemId = entity.ID;
                    UpdateSoftwareEnvironment(sentity1, tran);
                }
                DeleteDisasterInformation(entity.ID, tran);
                foreach (DisasterInformationEntity dentity in entity1)
                {
                    dentity.SystemId = entity.ID;
                    UpdateDisasterInformation(dentity, tran);
                }
                DeleteModuleInformation(entity.ID, tran);
                foreach (ModuleInformationEntity mentity in entity2)
                {
                    mentity.SystemId = entity.ID;
                    UpdateModuleInformation(mentity, tran);
                }
                DeleteTransactionCode(entity.ID, tran);
                foreach (TransactionCodeEntity tentity in entity3)
                {
                    tentity.SystemId = entity.ID;
                    UpdateTransactionCode(tentity, tran);
                }
                DeleteDataInformation(entity.ID, tran);
                foreach (DataInformationEntity dientity in entity4)
                {
                    dientity.SystemId = entity.ID;
                    UpdateDataInformation(dientity, tran);
                }
                DeleteInterfaceList(entity.ID, tran);
                foreach (InterfaceListEntity fentity in entity5)
                {
                    fentity.SystemId = entity.ID;
                    UpdateInterfaceList(fentity, tran);
                }

                //保存附件
                SaveAttachments(tran, long.Parse(entity.ID), entity.PhysicsUrl);
                tran.Commit();

            }
            catch (Exception err)
            {
                tran.Rollback();
                throw new Exception(err.Message);
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }
        #endregion
        #region 删除系统档案清单
        /// <summary>
        /// 删除系统档案清单
        /// </summary>
        /// <param name="ID"></param>
        public static void DeleteSystemFileList(string ID)
        {
            string strSQL = "delete from Br_Systemfilelist where id=" + ID;
            OracleConnection cn = ConfigTool.GetConnection();
            cn.Open();
            OracleTransaction tran = cn.BeginTransaction();

            try
            {

                OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);
                DeleteDataInformation(ID, tran);
                DeleteDisasterInformation(ID, tran);
                DeleteHardware(ID, tran);
                DeleteInterfaceList(ID, tran);
                DeleteModuleInformation(ID, tran);
                DeleteSoftwareEnvironment(ID, tran);
                DeleteTransactionCode(ID, tran);
                tran.Commit();

            }
            catch (Exception err)
            {
                tran.Rollback();
                throw new Exception(err.Message);
            }
            finally
            {
                ConfigTool.CloseConnection(cn);

            }
        }
        #endregion
        #endregion
        #region 硬件
        #region 查询硬件
        /// <summary>
        /// 查询硬件
        /// </summary>
        /// <returns></returns>
        public static DataTable GetHardware(string strWhere)
        {
            string strSQL = "select * from BR_Hardware where 1=1 ";
            strSQL += strWhere;
            OracleConnection cn = ConfigTool.GetConnection();
            return OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
        }
        #endregion
        #region 添加硬件
        /// <summary>
        /// 添加硬件
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="tran"></param>
        public static void InsertHardware(HardwareEntity entity, OracleTransaction tran)
        {
            long ID = 0;
            string strSQL = "insert into BR_Hardware(id,SystemId,HardwarePlatform,HardwareModel,HardwareFunction) values(BR_HARDWARESEQUENCES.Nextval,"
                            + StringTool.SqlQ(entity.SystemId) + "," + StringTool.SqlQ(entity.HardwarePlatform) + ","
                            + StringTool.SqlQ(entity.HardwareModel) + "," + StringTool.SqlQ(entity.HardwareFunction) + ")";
            //OracleConnection cn = ConfigTool.GetConnection();
            //cn.Open();
            //tran = cn.BeginTransaction();
            try
            {
                OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);

            }
            catch (Exception err)
            {

                throw new Exception(err.Message);
            }
            finally
            {

            }
        }
        #endregion
        #region 删除硬件
        /// <summary>
        /// 删除硬件
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteHardware(string id)
        {
            string strSQL = "delete from BR_Hardware where id=" + id;
            OracleConnection cn = ConfigTool.GetConnection();
            cn.Open();
            try
            {
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
        #endregion
        #region 删除硬件(带事务)
        /// <summary>
        /// 删除硬件(带事务)
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteHardware(string id, OracleTransaction tran)
        {
            string strSQL = "delete from BR_Hardware where  SystemId=" + StringTool.SqlQ(id);
            try
            {
                OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);
            }
            catch (Exception err)
            {

                throw new Exception(err.Message);
            }
            finally
            {
                //ConfigTool.CloseConnection(cn);
            }
        }
        #endregion
        #region 更新硬件
        public static void UpdateHardware(HardwareEntity entity, OracleTransaction tran)
        {

            InsertHardware(entity, tran);
        }
        #endregion
        #endregion
        #region 软件
        #region 查询软件
        /// <summary>
        /// 查询软件
        /// </summary>
        /// <returns></returns>
        public static DataTable GetSoftwareEnvironment(string strWhere)
        {
            string strSQL = "select * from Br_SoftwareEnvironment where 1=1 ";
            strSQL += strWhere;
            OracleConnection cn = ConfigTool.GetConnection();
            return OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
        }
        #endregion
        #region 添加软件
        /// <summary>
        /// 添加软件
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="tran"></param>
        public static void InsertSoftwareEnvironment(SoftwareEntity entity, OracleTransaction tran)
        {
            long ID = 0;
            string strSQL = "insert into Br_SoftwareEnvironment(id,SystemId,OperatingSystem,SystemVersion,Middleware,MiddlewareVersion,Database,DatabaseVersion,Tool,ToolVersion) values(BR_SOFTWAREENVIRONMENTS.Nextval,"
                            + StringTool.SqlQ(entity.SystemId) + "," + StringTool.SqlQ(entity.OperatingSystem) + ","
                            + StringTool.SqlQ(entity.SystemVersion) + "," + StringTool.SqlQ(entity.Middleware) + ","
                            + StringTool.SqlQ(entity.MiddlewareVersion) + "," + StringTool.SqlQ(entity.Database) + ","
                            + StringTool.SqlQ(entity.DatabaseVersion) + "," + StringTool.SqlQ(entity.Tool) + "," + StringTool.SqlQ(entity.ToolVersion) + ")";
            //OracleConnection cn = ConfigTool.GetConnection();
            //cn.Open();
            //tran = cn.BeginTransaction();
            try
            {
                OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);

            }
            catch (Exception err)
            {

                throw new Exception(err.Message);
            }
            finally
            {
                // ConfigTool.CloseConnection(cn);
            }
        }
        #endregion
        #region 删除软件
        /// <summary>
        /// 删除软件
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteSoftwareEnvironment(string id)
        {
            string strSQL = "delete from Br_SoftwareEnvironment where id=" + id;
            OracleConnection cn = ConfigTool.GetConnection();
            cn.Open();
            try
            {
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
        #endregion
        #region 删除软件(带事务)
        /// <summary>
        /// 删除软件(带事务)
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteSoftwareEnvironment(string id, OracleTransaction tran)
        {
            string strSQL = "delete from Br_SoftwareEnvironment where  SystemId=" + StringTool.SqlQ(id);
            try
            {
                OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);
            }
            catch (Exception err)
            {

                throw new Exception(err.Message);
            }
            finally
            {
                // ConfigTool.CloseConnection(cn);
            }
        }
        #endregion
        #region 更新软件
        public static void UpdateSoftwareEnvironment(SoftwareEntity entity, OracleTransaction tran)
        {

            InsertSoftwareEnvironment(entity, tran);

        }
        #endregion
        #endregion
        #region 系统灾备信息
        #region 查询系统灾备信息
        /// <summary>
        /// 查询系统灾备信息
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDisasterInformation(string strWhere)
        {
            string strSQL = "select * from BR_DisasterInformation where 1=1 ";
            strSQL += strWhere;
            OracleConnection cn = ConfigTool.GetConnection();
            return OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
        }
        #endregion
        #region 添加系统灾备信息
        /// <summary>
        /// 添加系统灾备信息
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="tran"></param>
        public static void InsertDisasterInformation(DisasterInformationEntity entity, OracleTransaction tran)
        {
            long ID = 0;
            string strSQL = "insert into BR_DisasterInformation(id,SystemId,IsHaveDisaster,BackupType,BackupServer,SchemeManufacturers) values(BR_DISASTERINFORMATIONS.Nextval,"
                            + StringTool.SqlQ(entity.SystemId) + "," + StringTool.SqlQ(entity.IsHaveDisaster) + ","
                            + StringTool.SqlQ(entity.BackupType) + "," + StringTool.SqlQ(entity.IsHaveDisaster) + "," + StringTool.SqlQ(entity.BackupServer) + ")";
            //OracleConnection cn = ConfigTool.GetConnection();
            //cn.Open();
            //tran = cn.BeginTransaction();
            try
            {
                OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);

            }
            catch (Exception err)
            {

                throw new Exception(err.Message);
            }
            finally
            {
                // ConfigTool.CloseConnection(cn);
            }
        }
        #endregion
        #region 删除系统灾备信息
        /// <summary>
        /// 删除系统灾备信息
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteDisasterInformation(string strWhere)
        {
            string strSQL = "delete from BR_DisasterInformation where 1=1";
            strSQL += strWhere;
            OracleConnection cn = ConfigTool.GetConnection();
            cn.Open();
            try
            {
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
        #endregion
        #region 删除系统灾备信息(带事务)
        /// <summary>
        /// 删除系统灾备信息(带事务)
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteDisasterInformation(string id, OracleTransaction tran)
        {
            string strSQL = "delete from BR_DisasterInformation where  SystemId=" + StringTool.SqlQ(id);
            try
            {
                OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);
            }
            catch (Exception err)
            {

                throw new Exception(err.Message);
            }
            finally
            {
                //  ConfigTool.CloseConnection(cn);
            }
        }
        #endregion
        #region 更新系统灾备信息
        public static void UpdateDisasterInformation(DisasterInformationEntity entity, OracleTransaction tran)
        {

            InsertDisasterInformation(entity, tran);
        }
        #endregion
        #endregion
        #region 系统模块分布信息
        #region 查询系统模块分布信息
        /// <summary>
        /// 查询系统模块分布信息
        /// </summary>
        /// <returns></returns>
        public static DataTable GetModuleInformation(string strWhere)
        {
            string strSQL = "select * from BR_ModuleInformation where 1=1 ";
            strSQL += strWhere;
            OracleConnection cn = ConfigTool.GetConnection();
            return OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
        }
        #endregion
        #region 添加系统模块分布信息
        /// <summary>
        /// 添加系统模块分布信息
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="tran"></param>
        public static void InsertModuleInformation(ModuleInformationEntity entity, OracleTransaction tran)
        {
            long ID = 0;
            string strSQL = "insert into BR_ModuleInformation(id,SystemId,ModuleName,ModuleDescription) values(BR_MODULEINFORMATIONS.Nextval,"
                            + StringTool.SqlQ(entity.SystemId) + "," + StringTool.SqlQ(entity.ModuleName) + ","
                            + StringTool.SqlQ(entity.ModuleDescription) + ")";
            //OracleConnection cn = ConfigTool.GetConnection();
            //cn.Open();
            //tran = cn.BeginTransaction();
            try
            {
                OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);

            }
            catch (Exception err)
            {

                throw new Exception(err.Message);
            }
            finally
            {

            }
        }
        #endregion
        #region 删除系统模块分布信息
        /// <summary>
        /// 删除系统模块分布信息
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteModuleInformation(string strWhere)
        {
            string strSQL = "delete from BR_ModuleInformation where 1=1";
            strSQL += strWhere;
            OracleConnection cn = ConfigTool.GetConnection();
            cn.Open();
            try
            {
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
        #endregion
        #region 删除系统模块分布信息(带事务)
        /// <summary>
        /// 删除系统模块分布信息(带事务)
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteModuleInformation(string id, OracleTransaction tran)
        {
            string strSQL = "delete from BR_ModuleInformation where  SystemId=" + StringTool.SqlQ(id);
            try
            {
                OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);
            }
            catch (Exception err)
            {

                throw new Exception(err.Message);
            }
            finally
            {
                //ConfigTool.CloseConnection(cn);
            }
        }
        #endregion
        #region 更新系统模块分布信息
        public static void UpdateModuleInformation(ModuleInformationEntity entity, OracleTransaction tran)
        {

            InsertModuleInformation(entity, tran);

        }
        #endregion
        #endregion
        #region 系统交易码清单

        #region 查询系统交易码清单


        /// <summary>
        /// 查询系统模块分布信息
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTransactionCode(string strWhere)
        {
            string strSQL = "select * from BR_TransactionCode where 1=1 ";
            strSQL += strWhere;
            OracleConnection cn = ConfigTool.GetConnection();
            return OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
        }
        #endregion
        #region 添加系统交易码清单

        /// <summary>
        /// 添加系统交易码清单

        /// </summary>
        /// <param name="entity"></param>
        /// <param name="tran"></param>
        public static void InsertTransactionCode(TransactionCodeEntity entity, OracleTransaction tran)
        {
            long ID = 0;
            string strSQL = "insert into BR_TransactionCode(id,SystemId,TransactionCode,FunctionSketch) values(BR_TRANSACTIONCODES.Nextval,"
                            + StringTool.SqlQ(entity.SystemId) + "," + StringTool.SqlQ(entity.TransactionCode) + ","
                            + StringTool.SqlQ(entity.FunctionSketch) + ")";
            //OracleConnection cn = ConfigTool.GetConnection();
            //cn.Open();
            //tran = cn.BeginTransaction();
            try
            {
                OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);

            }
            catch (Exception err)
            {

                throw new Exception(err.Message);
            }
            finally
            {

            }
        }
        #endregion
        #region 删除系统交易码清单

        /// <summary>
        /// 删除系统交易码清单

        /// </summary>
        /// <param name="id"></param>
        public static void DeleteTransactionCode(string strWhere)
        {
            string strSQL = "delete from BR_TransactionCode where 1=1";
            strSQL += strWhere;
            OracleConnection cn = ConfigTool.GetConnection();
            cn.Open();
            try
            {
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
        #endregion
        #region 删除系统交易码清单带事务)
        /// <summary>
        /// 删除系统交易码清单(带事务)
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteTransactionCode(string id, OracleTransaction tran)
        {
            string strSQL = "delete from BR_TransactionCode where  SystemId=" + StringTool.SqlQ(id);

            try
            {
                OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);
            }
            catch (Exception err)
            {

                throw new Exception(err.Message);
            }
            finally
            {
                // ConfigTool.CloseConnection(cn);
            }
        }
        #endregion
        #region 更新系统交易码清单

        public static void UpdateTransactionCode(TransactionCodeEntity entity, OracleTransaction tran)
        {
            InsertTransactionCode(entity, tran);
        }
        #endregion
        #endregion
        #region 系统数据类信息


        #region 查询系统数据类信息


        /// <summary>
        /// 查询系统数据类信息

        /// </summary>
        /// <returns></returns>
        public static DataTable GetDataInformation(string strWhere)
        {
            string strSQL = "select * from BR_DataInformation where 1=1 ";
            strSQL += strWhere;
            OracleConnection cn = ConfigTool.GetConnection();
            return OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
        }
        #endregion
        #region 添加系统数据类信息

        /// <summary>
        /// 添加系统数据类信息

        /// </summary>
        /// <param name="entity"></param>
        /// <param name="tran"></param>
        public static void InsertDataInformation(DataInformationEntity entity, OracleTransaction tran)
        {
            long ID = 0;
            string strSQL = "insert into BR_DataInformation(id,SystemId,TableName,DatabaseDescription,Count,Capacity) values(BR_DATAINFORMATIONS.Nextval,"
                            + StringTool.SqlQ(entity.SystemId) + "," + StringTool.SqlQ(entity.TableName) + ","
                            + StringTool.SqlQ(entity.DatabaseDescription) + "," + entity.Count + ","
                            + entity.Capacity + ")";
            //OracleConnection cn = ConfigTool.GetConnection();
            //cn.Open();
            //tran = cn.BeginTransaction();
            try
            {
                OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);

            }
            catch (Exception err)
            {

                throw new Exception(err.Message);
            }
            finally
            {

            }
        }
        #endregion
        #region 删除系统数据类信息

        /// <summary>
        /// 删除系统数据类信息

        /// </summary>
        /// <param name="id"></param>
        public static void DeleteDataInformation(string strWhere)
        {
            string strSQL = "delete from BR_DataInformation where 1=1";
            strSQL += strWhere;
            OracleConnection cn = ConfigTool.GetConnection();
            cn.Open();
            try
            {
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
        #endregion
        #region 删除系统数据类信息(带事务)
        /// <summary>
        /// 删除系统数据类信息(带事务)
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteDataInformation(string id, OracleTransaction tran)
        {
            string strSQL = "delete from BR_DataInformation where  SystemId=" + StringTool.SqlQ(id);
            try
            {
                OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);
            }
            catch (Exception err)
            {

                throw new Exception(err.Message);
            }
            finally
            {
                // ConfigTool.CloseConnection(cn);
            }
        }
        #endregion
        #region 更新系统数据类信息

        /// <summary>
        /// 更新系统数据类信息

        /// </summary>
        /// <param name="entity"></param>
        /// <param name="strWhere"></param>
        /// <param name="tran"></param>
        public static void UpdateDataInformation(DataInformationEntity entity, OracleTransaction tran)
        {
            InsertDataInformation(entity, tran);
        }
        #endregion
        #endregion
        #region 系统档案清单

        #region 查询系统档案清单

        /// <summary>
        /// 查询系统档案清单
        /// </summary>
        /// <returns></returns>
        public static DataTable GetInterfaceList(string strWhere)
        {
            string strSQL = "select * from BR_InterfaceList where 1=1 ";
            strSQL += strWhere;
            OracleConnection cn = ConfigTool.GetConnection();
            return OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
        }
        #endregion
        #region 添加系统档案清单
        /// <summary>
        /// 添加系统档案清单
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="tran"></param>
        public static void InsertInterfaceList(InterfaceListEntity entity, OracleTransaction tran)
        {
            long ID = 0;
            string strSQL = @"insert into BR_InterfaceList(id,SystemId,SystemName,EGName,FunctionTypes,FunctionName,
                              TransactionCode,SequenceNumber,ExternalName,InterfaceCode,InterfaceMode) values(BR_INTERFACELISTS.Nextval,"
                            + StringTool.SqlQ(entity.SystemId) + "," + StringTool.SqlQ(entity.SystemName) + ","
                            + StringTool.SqlQ(entity.EGName) + ","
                            + StringTool.SqlQ(entity.FunctionTypes) + ","
                            + StringTool.SqlQ(entity.FunctionName) + ","
                            + StringTool.SqlQ(entity.TransactionCode) + ","
                            + entity.SequenceNumber + ","
                            + StringTool.SqlQ(entity.ExternalName) + ","
                            + StringTool.SqlQ(entity.InterfaceCode) + ","
                            + StringTool.SqlQ(entity.InterfaceMode) + ")";
            //OracleConnection cn = ConfigTool.GetConnection();
            //cn.Open();
            //tran = cn.BeginTransaction();
            try
            {
                OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);

            }
            catch (Exception err)
            {

                throw new Exception(err.Message);
            }
            finally
            {

            }
        }
        #endregion
        #region 删除系统档案清单
        /// <summary>
        /// 删除系统档案清单
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteInterfaceList(string strWhere)
        {
            string strSQL = "delete from BR_InterfaceList where 1=1";
            strSQL += strWhere;
            OracleConnection cn = ConfigTool.GetConnection();
            cn.Open();
            try
            {
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
        #endregion
        #region 删除系统档案清单(带事务)
        /// <summary>
        /// 删除系统档案清单(带事务)
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteInterfaceList(string id, OracleTransaction tran)
        {
            string strSQL = "delete from BR_InterfaceList where  SystemId=" + StringTool.SqlQ(id);
            try
            {
                OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);
            }
            catch (Exception err)
            {

                throw new Exception(err.Message);
            }
            finally
            {
                //ConfigTool.CloseConnection(cn);
            }
        }
        #endregion
        #region 更新系统档案清单
        /// <summary>
        /// 更新系统档案清单
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="strWhere"></param>
        /// <param name="tran"></param>
        public static void UpdateInterfaceList(InterfaceListEntity entity, OracleTransaction tran)
        {
            InsertInterfaceList(entity, tran);
        }
        #endregion
        #endregion

        #region 获得资产附件的信息，以XML串表示

        /// <summary>
        /// 获得资产附件的信息，以XML串表示

        /// </summary>
        /// <param name="lngKBID"></param>
        /// <returns></returns>
        public static string GetAttachmentXml(decimal lngKBID)
        {
            string strSQL = "";
            OracleDataReader dr;

            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlElTmp;
            XmlElement xmlElSub;

            xmlElTmp = xmlDoc.CreateElement("Attachments");


            //添加附件信息  
            strSQL = "SELECT a.FileID,a.FileName,a.SufName,a.Status,a.upTime,a.upUserID,b.Name as upUserName ,nvl(a.requstFileId,'') requstFileId " +
                 " FROM SystemFile_ATTACHMENT a,Ts_User b " +
                 " WHERE a.upUserID = b.UserID AND a.Status <>" + (int)e_FileStatus.efsDeleted +
                 "		AND a.KBID =" + lngKBID.ToString() + " AND nvl(a.deleted,0)=" + (int)e_Deleted.eNormal + " ORDER BY a.FileID";

            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");
            try
            {
                dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                while (dr.Read())
                {
                    xmlElSub = xmlDoc.CreateElement("Attachment");
                    xmlElSub.SetAttribute("FileID", ((long)dr.GetDecimal(0)).ToString());
                    xmlElSub.SetAttribute("FileName", dr.GetString(1));
                    xmlElSub.SetAttribute("SufName", dr.GetString(2));
                    xmlElSub.SetAttribute("Status", dr.GetInt32(3).ToString());
                    xmlElSub.SetAttribute("upTime", dr.GetDateTime(4).ToLongDateString());
                    xmlElSub.SetAttribute("upUserID", ((long)dr.GetDecimal(5)).ToString());
                    xmlElSub.SetAttribute("upUserName", dr.GetString(6));
                    xmlElSub.SetAttribute("replace", dr["requstFileId"].ToString());
                    xmlElTmp.AppendChild(xmlElSub);

                }
                dr.Close();                
            }
            finally { ConfigTool.CloseConnection(cn); }


            xmlDoc.AppendChild(xmlElTmp);
            return xmlDoc.InnerXml;
        }
        #endregion

        #region 取附件名称

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngFileID"></param>
        /// <returns></returns>
        public static string GetAttachmentName(long lngFileID)
        {
            string strFileName = string.Empty;
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");
            try
            {
                strSQL = "SELECT FileName FROM SystemFile_ATTACHMENT WHERE FileID=" + lngFileID.ToString();
                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                while (dr.Read())
                {
                    strFileName = dr.GetString(0);
                }
                dr.Close();                
            }
            finally { ConfigTool.CloseConnection(cn); }

            return strFileName;
        }
        #endregion

        #region 保存附件
        /// <summary>
        /// 保存附件信息及存储附件

        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngKBID"></param>
        /// <param name="strAttachment"></param>
        private static void SaveAttachments(OracleTransaction trans, decimal lngKBID, string strAttachment)
        {
            string strSQL = "";
            OracleDataReader dr;
            e_FileStatus lngFileStatus = 0;
            long lngFileID = 0;
            string strFileName = "";
            string strSufName = "";
            long lngupUserID = 0;

            bool blnNew = true;

            string strTmpCatalog = "";
            string strFileCatalog = "";
            string strTmpSubPath = "";
            string strTmpPath = "";

            string strOldFilePath = "";

            string strTmpFileN = "";
            string strFileN = "";
            string reqestId = "";
            int count = 0;

            strTmpCatalog = CommonDP.GetConfigValue("TempCataLog", "TempCataLog");
            strFileCatalog = CommonDP.GetConfigValue("TempCataLog", "FileCataLog");

            XmlTextReader tr = new XmlTextReader(new StringReader(strAttachment));
            while (tr.Read())
            {
                if (tr.NodeType == XmlNodeType.Element && tr.Name == "Attachments")
                {
                    //获取临时子路径

                    if (tr.GetAttribute("TempSubPath") != null)
                        strTmpSubPath = tr.GetAttribute("TempSubPath");

                }
                if (tr.NodeType == XmlNodeType.Element && tr.Name == "Attachment")
                {
                    lngFileStatus = (e_FileStatus)(int.Parse(tr.GetAttribute("Status")));
                    lngFileID = long.Parse(tr.GetAttribute("FileID"));
                    strFileName = tr.GetAttribute("FileName");
                    strSufName = tr.GetAttribute("SufName");
                    lngupUserID = long.Parse(tr.GetAttribute("upUserID"));
                    reqestId = tr.GetAttribute("replace");

                    if (strTmpCatalog.EndsWith(@"\") == false)
                    {
                        if (strTmpSubPath == "")
                        {
                            strTmpPath = strTmpCatalog;
                        }
                        else
                        {
                            strTmpPath = strTmpCatalog + @"\" + strTmpSubPath;
                        }
                    }
                    else
                    {
                        if (strTmpSubPath == "")
                        {
                            strTmpPath = strTmpCatalog.Substring(0, strTmpCatalog.Length - 1);
                        }
                        else
                        {
                            strTmpPath = strTmpCatalog + strTmpSubPath;
                        }

                    }

                    strTmpFileN = strTmpPath + @"\" + lngFileID.ToString();

                    string smonthfilepath = DateTime.Now.ToString("yyyyMM");
                    if (strFileCatalog.EndsWith(@"\") == false)
                    {
                        strFileN = strFileCatalog + @"\" + smonthfilepath;
                    }
                    else
                    {
                        strFileN = strFileCatalog + smonthfilepath;
                    }
                    MyFiles.AutoCreateDirectory(strFileN);
                    strFileN += @"\" + lngFileID.ToString();

                    blnNew = true;
                    switch (lngFileStatus)
                    {
                        case e_FileStatus.efsUpdate:
                        case e_FileStatus.efsNew:
                            //新增处理 ：1、添加记录、更新的情况判断是否存在记录（可能操作的同时别人在进行删除）  2、将临时目录中对应的文件编码并移到文件存储目录下
                            count++;
                            strSQL = "SELECT FileID,nvl(filepath,'') FROM SystemFile_ATTACHMENT WHERE FileID=" + lngFileID.ToString();
                            dr = OracleDbHelper.ExecuteReader(trans, CommandType.Text, strSQL);
                            while (dr.Read())
                            {
                                blnNew = false;
                                strOldFilePath = dr.GetString(1);
                            }
                            dr.Close();

                            if (blnNew)
                            {
                                strSQL = "INSERT INTO SystemFile_ATTACHMENT (FileID,KBID,FileName,SufName,filepath,Status,upTime,upUserID,MonthPath,deleted,deleteTime,requstFileId) " +
                                    " VALUES(" +
                                    lngFileID.ToString() + "," +
                                    lngKBID.ToString() + "," +
                                    StringTool.SqlQ(strFileName) + "," +
                                    StringTool.SqlQ(strSufName) + "," +
                                    StringTool.SqlQ(strFileCatalog) + "," +
                                    (int)e_FileStatus.efsNormal + "," +
                                    " sysdate " + "," +
                                    StringTool.SqlQ(lngupUserID.ToString()) + "," +
                                    StringTool.SqlQ(smonthfilepath) + "," +
                                     "0," +
                                     "null," + StringTool.SqlQ(reqestId.ToString()) +
                                    ")";
                            }
                            else
                            {
                                strSQL = "UPDATE SystemFile_ATTACHMENT SET upTime = sysdate,upUserID =" + lngupUserID.ToString() + "," +
                                             " filepath = " + StringTool.SqlQ(strFileCatalog) +
                                            " WHERE FileID=" + lngFileID.ToString();
                            }
                            OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);

                            //2、将临时目录中对应的文件编码并移到文件存储目录下,并删除

                            if (PreProcessForAttachment(strTmpCatalog, strFileCatalog, strTmpFileN))
                            {
                                //由于没有比较好的办法处理，下载时的临时文件，所以，取消编码  2003-06-26 ***
                                //								MyComponent.MyTechLib.MyEnCoder.EnCodeFileToFile(strTmpFileN,strFileN);
                                if (File.Exists(strFileN))
                                    File.Delete(strFileN);
                                File.Move(strTmpFileN, strFileN);
                            }

                            if (strOldFilePath != "" && strOldFilePath.Trim().ToLower() != strFileCatalog.Trim().ToLower())
                            {
                                string strOldFileN = "";
                                if (strFileCatalog.EndsWith(@"\") == false)
                                {
                                    strOldFileN = strOldFilePath + @"\" + lngFileID.ToString();
                                }
                                else
                                {
                                    strOldFileN = strOldFilePath + lngFileID.ToString();
                                }
                                //删除附件
                                if (File.Exists(strOldFileN))
                                    File.Delete(strOldFileN);
                            }


                            break;

                        case e_FileStatus.efsDeleted:
                            strSQL = "SELECT nvl(filepath,'') FROM SystemFile_ATTACHMENT WHERE FileID=" + lngFileID.ToString();
                            dr = OracleDbHelper.ExecuteReader(trans, CommandType.Text, strSQL);
                            while (dr.Read())
                            {
                                strOldFilePath = dr.GetString(0);
                            }
                            dr.Close();
                            //删除记录
                            strSQL = "update SystemFile_ATTACHMENT set deleted=1,deletetime=sysdate,requstFileId=" + StringTool.SqlQ(reqestId.ToString()) + " WHERE FileID =" + lngFileID.ToString();
                            OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);
                            break;
                        default:
                            count++;
                            break;
                    }

                    //无论如何删除临时文件  ***  保险语句  ****
                    if (File.Exists(strTmpFileN))
                        File.Delete(strTmpFileN);

                }
            }
            tr.Close();

            if (strTmpSubPath != "")
            {
                if (Directory.Exists(strTmpPath))
                    Directory.Delete(strTmpPath);
            }

            //更新流程附件状态

            if (count == 0)
            {
                strSQL = "UPDATE Equ_desk SET Attachment =" + (int)e_IsTrue.fmFalse + " WHERE ID=" + lngKBID.ToString();
            }
            else
            {
                strSQL = "UPDATE Equ_desk SET Attachment =" + (int)e_IsTrue.fmTrue + " WHERE ID=" + lngKBID.ToString();
            }
            OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);

        }

        private static bool PreProcessForAttachment(string strTmpCatalog, string strFileCatalog, string strTmpFileN)
        {
            MyFiles.AutoCreateDirectory(strTmpCatalog);
            MyFiles.AutoCreateDirectory(strFileCatalog);


            FileInfo fi = new FileInfo(strTmpFileN);
            return fi.Exists;

        }

        #endregion

        #region 根据表ID查看附件是否存在
        /// <summary>
        /// 根据表ID查看附件是否存在
        /// </summary>
        /// <param name="FileId"></param>
        /// <returns></returns>
        public static DataTable getFileIsTrue(long FileId)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            try
            {
                string strSQL = "SELECT * FROM SystemFile_ATTACHMENT where FileID=" + FileId.ToString();
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                ConfigTool.CloseConnection(cn);
                return dt;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }
        }
        #endregion

        #region 取得已删除的流程附件
        /// <summary>
        /// 取得已删除的流程附件
        /// </summary>
        /// <param name="strKBID"></param>
        /// <returns></returns>
        public static DataTable getDeleteAttchmentTBL(string strKBID)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            try
            {
                string strSQL = @"select OA.KBID as FlowId,OA.*,U.name as UpuserName,OldOA.FileName as oldFiledName from SystemFile_ATTACHMENT OA
                left join ts_user  U on OA.upUserID=U.userid 
                left join SystemFile_ATTACHMENT OldOA  on OA.requstFileId=to_char(OldOA.FileId)
                where nvl(OA.deleted,0)=1 and (OA.requstFileid='' or OA.requstFileid is null) and OA.KBID=" + strKBID.ToString();
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                return dt;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }

        }
        #endregion

        #region 获取更新过的附件
        /// <summary>
        /// 获取更新过的附件
        /// </summary>
        /// <param name="lngKBID"></param>
        /// <param name="requstFileid"></param>
        /// <param name="IsDelete"></param>
        /// <returns></returns>
        public static DataTable getUpdateAttchmentTBL(string lngKBID, long requstFileid, bool IsDelete)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            try
            {

                string strSQL = string.Empty;
                if (IsDelete == false)
                {
                    strSQL = "SELECT a.deleteTime,a.KBID as FlowId,a.FileID,a.FileName,a.SufName,a.Status,a.upTime,a.upUserID,b.Name as upUserName ,nvl(a.requstFileId,'') requstFileId " +
                   " FROM SystemFile_ATTACHMENT a,Ts_User b " +
                   " WHERE a.upUserID = b.UserID " +
                   "		AND a.KBID =" + lngKBID.ToString() + " AND a.requstFileId =" + requstFileid.ToString() + "  ORDER BY a.FileID";
                }
                else
                {
                    strSQL = "SELECT a.deleteTime,a.KBID as FlowId,a.FileID,a.FileName,a.SufName,a.Status,a.upTime,a.upUserID,b.Name as upUserName ,nvl(a.requstFileId,'') requstFileId " +
                     " FROM SystemFile_ATTACHMENT a,Ts_User b " +
                     " WHERE a.upUserID = b.UserID " +
                     "		AND a.KBID =" + lngKBID.ToString() + " AND a.FileID =" + requstFileid.ToString() + "  ORDER BY a.FileID";
                }
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                ConfigTool.CloseConnection(cn);
                return dt;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }
        }
        #endregion


        #region 取附件名称

        /// <summary>
        /// 取附件名称

        /// </summary>
        /// <param name="lngFileID"></param>
        /// <param name="strMonthPath"></param>
        /// <returns></returns>
        public static string GetAttachmentName(long lngFileID, ref string strMonthPath)
        {
            string strFileName = string.Empty;
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT FileName,nvl(MonthPath,'')  MonthPath FROM SystemFile_ATTACHMENT WHERE FileID=" + lngFileID.ToString();
                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                while (dr.Read())
                {
                    strFileName = dr.GetString(0);
                    strMonthPath = dr.GetString(1);
                }
                dr.Close();
                
                return strFileName;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion

    }

    #region 系统档案清单
    public class SystemFileEntity
    {
        private string _ID;
        /// <summary>
        /// ID
        /// </summary>
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private string _SystemName;
        /// <summary>
        /// 系统的名称

        /// </summary>
        public string SystemName
        {
            get { return _SystemName; }
            set { _SystemName = value; }
        }
        private string _EGName;
        /// <summary>
        /// 系统英文名

        /// </summary>
        public string EGName
        {
            get { return _EGName; }
            set { _EGName = value; }
        }

        private string _SystemID;
        /// <summary>
        /// 系统ID
        /// </summary>
        public string SystemID
        {
            get { return _SystemID; }
            set { _SystemID = value; }
        }
        private string _HomeName;
        /// <summary>
        /// 归属系统群名称

        /// </summary>
        public string HomeName
        {
            get { return _HomeName; }
            set { _HomeName = value; }
        }
        private string _Introduction;
        /// <summary>
        /// 系统功能简介

        /// </summary>
        public string Introduction
        {
            get { return _Introduction; }
            set { _Introduction = value; }
        }
        private string _DepetName;
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepetName
        {
            get { return _DepetName; }
            set { _DepetName = value; }
        }
        private string _ManagerName;
        /// <summary>
        /// 业务经理
        /// </summary>
        public string ManagerName
        {
            get { return _ManagerName; }
            set { _ManagerName = value; }
        }
        private string _BA = string.Empty;
        /// <summary>
        /// BA
        /// </summary>
        public string BA
        {
            get { return _BA; }
            set { _BA = value; }
        }
        private string _ITManager = string.Empty;
        /// <summary>
        /// IT经理
        /// </summary>
        public string ITManager
        {
            get { return _ITManager; }
            set { _ITManager = value; }
        }
        private string _SA = string.Empty;
        /// <summary>
        /// 系统英SA文名
        /// </summary>
        public string SA
        {
            get { return _SA; }
            set { _SA = value; }
        }
        private string _DevelopmentMode = string.Empty;
        /// <summary>
        /// 系统开发方式

        /// </summary>
        public string DevelopmentMode
        {
            get { return _DevelopmentMode; }
            set { _DevelopmentMode = value; }
        }
        private string _Manufacturer = string.Empty;
        /// <summary>
        /// 产品厂商
        /// </summary>
        public string Manufacturer
        {
            get { return _Manufacturer; }
            set { _Manufacturer = value; }
        }
        private string _State;
        /// <summary>
        /// 系统目前状态

        /// </summary>
        public string State
        {
            get { return _State; }
            set { _State = value; }
        }
        private string _ProductionTime = string.Empty;
        /// <summary>
        /// 系统投产时间
        /// </summary>
        public string ProductionTime
        {
            get { return _ProductionTime; }
            set { _ProductionTime = value; }
        }
        private string _MaintenanceMode = string.Empty;
        /// <summary>
        /// 系统运维方式
        /// </summary>
        public string MaintenanceMode
        {
            get { return _MaintenanceMode; }
            set { _MaintenanceMode = value; }
        }
        private string _Outsourcing = string.Empty;
        /// <summary>
        /// 运维厂商
        /// </summary>
        public string Outsourcing
        {
            get { return _Outsourcing; }
            set { _Outsourcing = value; }
        }
        private string _Operation = string.Empty;
        /// <summary>
        /// 操作系统
        /// </summary>
        public string Operation
        {
            get { return _Operation; }
            set { _Operation = value; }
        }

        private string _OperationEdition;
        /// <summary>
        /// 操作系统版本
        /// </summary>
        public string OperationEdition
        {
            get { return _OperationEdition; }
            set { _OperationEdition = value; }
        }
        private string _PhysicsUrl = "<Attachments />";
        /// <summary>
        /// 物理部署图

        /// </summary>
        public string PhysicsUrl
        {
            get { return _PhysicsUrl; }
            set { _PhysicsUrl = value; }
        }
        private string _ApplicationUrl = "<Attachments />";
        /// <summary>
        /// 应用架构图

        /// </summary>
        public string ApplicationUrl
        {
            get { return _ApplicationUrl; }
            set { _ApplicationUrl = value; }
        }
        //private string _Database;
        ///// <summary>
        ///// 数据库

        ///// </summary>
        //public string Database
        //{
        //    get { return _Database; }
        //    set { _Database = value; }
        //}
        //private string _DatabaseEdition;
        ///// <summary>
        ///// 数据库版本

        ///// </summary>
        //public string DatabaseEdition
        //{
        //    get { return _DatabaseEdition; }
        //    set { _DatabaseEdition = value; }
        //}
        //private string _Tools;
        ///// <summary>
        ///// 开发工具

        ///// </summary>
        //public string Tools
        //{
        //    get { return _Tools; }
        //    set { _Tools = value; }
        //}
        //private string _ToolsEdition;
        ///// <summary>
        ///// 开发工具版本

        ///// </summary>
        //public string ToolsEdition
        //{
        //    get { return _ToolsEdition; }
        //    set { _ToolsEdition = value; }
        //}
        private string _TradingVolume = "0";
        /// <summary>
        /// 日均交易量

        /// </summary>
        public string TradingVolume
        {
            get { return _TradingVolume; }
            set { _TradingVolume = value; }
        }
        private string _PeakVolume = "0";
        /// <summary>
        /// 峰值交易量
        /// </summary>
        public string PeakVolume
        {
            get { return _PeakVolume; }
            set { _PeakVolume = value; }
        }
        private string _HandleTime = "0";
        /// <summary>
        /// 系统批处理持续时间

        /// </summary>
        public string HandleTime
        {
            get { return _HandleTime; }
            set { _HandleTime = value; }
        }
        private string _SystemAccount = "0";
        /// <summary>
        /// 系统账户数

        /// </summary>
        public string SystemAccount
        {
            get { return _SystemAccount; }
            set { _SystemAccount = value; }
        }

        private string _CustomerNumber = "0";
        /// <summary>
        /// 系统账户数

        /// </summary>
        public string CustomerNumber
        {
            get { return _CustomerNumber; }
            set { _CustomerNumber = value; }
        }
    }
    #endregion

    #region 硬件
    /// <summary>
    /// 硬件
    /// </summary>
    public class HardwareEntity
    {

        private string _ID;
        /// <summary>
        /// ID
        /// </summary>
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private string _SystemId;
        /// <summary>
        /// 系统ID
        /// </summary>
        public string SystemId
        {
            get { return _SystemId; }
            set { _SystemId = value; }
        }
        private string _HardwarePlatform;
        /// <summary>
        /// 硬件平台
        /// </summary>
        public string HardwarePlatform
        {
            get { return _HardwarePlatform; }
            set { _HardwarePlatform = value; }
        }
        private string _HardwareModel;
        /// <summary>
        /// 硬件型号
        /// </summary>
        public string HardwareModel
        {
            get { return _HardwareModel; }
            set { _HardwareModel = value; }
        }
        private string _HardwareFunction;
        /// <summary>
        /// 硬件主要功能
        /// </summary>
        public string HardwareFunction
        {
            get { return _HardwareFunction; }
            set { _HardwareFunction = value; }
        }
    }
    #endregion
    #region 软件
    /// <summary>
    /// 软件
    /// </summary>
    public class SoftwareEntity
    {
        private string _ID;
        /// <summary>
        /// ID
        /// </summary>
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private string _SystemId;
        /// <summary>
        /// 系统ID
        /// </summary>
        public string SystemId
        {
            get { return _SystemId; }
            set { _SystemId = value; }
        }
        private string _OperatingSystem;
        /// <summary>
        /// 操作系统
        /// </summary>
        public string OperatingSystem
        {
            get { return _OperatingSystem; }
            set { _OperatingSystem = value; }
        }
        private string _SystemVersion;
        /// <summary>
        /// 操作系统版本
        /// </summary>
        public string SystemVersion
        {
            get { return _SystemVersion; }
            set { _SystemVersion = value; }
        }
        private string _Middleware;
        /// <summary>
        /// 中间件

        /// </summary>
        public string Middleware
        {
            get { return _Middleware; }
            set { _Middleware = value; }
        }


        private string _MiddlewareVersion;
        /// <summary>
        /// 中间件版本

        /// </summary>
        public string MiddlewareVersion
        {
            get { return _MiddlewareVersion; }
            set { _MiddlewareVersion = value; }
        }
        private string _Database;
        /// <summary>
        /// 数据库

        /// </summary>
        public string Database
        {
            get { return _Database; }
            set { _Database = value; }
        }
        private string _DatabaseVersion;
        /// <summary>
        /// 数据库版本

        /// </summary>
        public string DatabaseVersion
        {
            get { return _DatabaseVersion; }
            set { _DatabaseVersion = value; }
        }

        private string _Tool;
        /// <summary>
        /// 开发工具

        /// </summary>
        public string Tool
        {
            get { return _Tool; }
            set { _Tool = value; }
        }
        private string _ToolVersion;
        /// <summary>
        /// 开发工具版本

        /// </summary>
        public string ToolVersion
        {
            get { return _ToolVersion; }
            set { _ToolVersion = value; }
        }
    }
    #endregion

    #region 系统灾备信息
    /// <summary>
    /// 系统灾备信息
    /// </summary>
    public class DisasterInformationEntity
    {
        private string _ID;
        /// <summary>
        /// ID
        /// </summary>
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private string _SystemId;
        /// <summary>
        /// 系统ID
        /// </summary>
        public string SystemId
        {
            get { return _SystemId; }
            set { _SystemId = value; }
        }
        private string _IsHaveDisaster;
        /// <summary>
        /// 有无灾备
        /// </summary>
        public string IsHaveDisaster
        {
            get { return _IsHaveDisaster; }
            set { _IsHaveDisaster = value; }
        }
        private string _BackupType;
        /// <summary>
        /// 灾备类型
        /// </summary>
        public string BackupType
        {
            get { return _BackupType; }
            set { _BackupType = value; }
        }
        private string _BackupServer;
        /// <summary>
        /// 灾备服务器型号


        /// </summary>
        public string BackupServer
        {
            get { return _BackupServer; }
            set { _BackupServer = value; }
        }

        private string _SchemeManufacturers;
        /// <summary>
        /// 灾备方案及厂商


        /// </summary>
        public string SchemeManufacturers
        {
            get { return _SchemeManufacturers; }
            set { _SchemeManufacturers = value; }
        }
    }
    #endregion

    #region 系统模块分布信息
    /// <summary>
    /// 系统模块分布信息
    /// </summary>
    public class ModuleInformationEntity
    {
        private string _ID;
        /// <summary>
        /// ID
        /// </summary>
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private string _SystemId;
        /// <summary>
        /// 系统ID
        /// </summary>
        public string SystemId
        {
            get { return _SystemId; }
            set { _SystemId = value; }
        }
        private string _ModuleName;
        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName
        {
            get { return _ModuleName; }
            set { _ModuleName = value; }
        }
        private string _ModuleDescription;
        /// <summary>
        /// 模块简述


        /// </summary>
        public string ModuleDescription
        {
            get { return _ModuleDescription; }
            set { _ModuleDescription = value; }
        }

    }
    #endregion

    #region 系统交易码清单


    /// <summary>
    /// 系统交易码清单

    /// </summary>
    public class TransactionCodeEntity
    {
        private string _ID;
        /// <summary>
        /// ID
        /// </summary>
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private string _SystemId;
        /// <summary>
        /// 系统ID
        /// </summary>
        public string SystemId
        {
            get { return _SystemId; }
            set { _SystemId = value; }
        }
        private string _TransactionCode;
        /// <summary>
        /// 交易代码
        /// </summary>
        public string TransactionCode
        {
            get { return _TransactionCode; }
            set { _TransactionCode = value; }
        }
        private string _FunctionSketch;
        /// <summary>
        /// 交易功能简述

        /// </summary>
        public string FunctionSketch
        {
            get { return _FunctionSketch; }
            set { _FunctionSketch = value; }
        }

    }
    #endregion

    #region 系统数据类信息


    /// <summary>
    /// 系统数据类信息

    /// </summary>
    public class DataInformationEntity
    {
        private string _ID;
        /// <summary>
        /// ID
        /// </summary>
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private string _SystemId;
        /// <summary>
        /// 系统ID
        /// </summary>
        public string SystemId
        {
            get { return _SystemId; }
            set { _SystemId = value; }
        }
        private string _TableName;
        /// <summary>
        /// 库表名称
        /// </summary>
        public string TableName
        {
            get { return _TableName; }
            set { _TableName = value; }
        }
        private string _DatabaseDescription;
        /// <summary>
        /// 交易功能简述


        /// </summary>
        public string DatabaseDescription
        {
            get { return _DatabaseDescription; }
            set { _DatabaseDescription = value; }
        }

        private long _Count;
        /// <summary>
        /// 记录数


        /// </summary>
        public long Count
        {
            get { return _Count; }
            set { _Count = value; }
        }

        private long _Capacity;
        /// <summary>
        /// 库表容量
        /// </summary>
        public long Capacity
        {
            get { return _Capacity; }
            set { _Capacity = value; }
        }

    }
    #endregion

    #region 系统档案清单
    /// <summary>
    /// 系统数据类信息

    /// </summary>
    public class InterfaceListEntity
    {
        private string _ID;
        /// <summary>
        /// ID
        /// </summary>
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private string _SystemId;
        /// <summary>
        /// 系统ID
        /// </summary>
        public string SystemId
        {
            get { return _SystemId; }
            set { _SystemId = value; }
        }

        private string _SystemName;
        /// <summary>
        /// 系统名称
        /// </summary>
        public string SystemName
        {
            get { return _SystemName; }
            set { _SystemName = value; }
        }
        private string _EGName;
        /// <summary>
        /// 系统英文简称


        /// </summary>
        public string EGName
        {
            get { return _EGName; }
            set { _EGName = value; }
        }
        private string _FunctionTypes;
        /// <summary>
        /// 功能类型
        /// </summary>
        public string FunctionTypes
        {
            get { return _FunctionTypes; }
            set { _FunctionTypes = value; }
        }

        private string _FunctionName;
        /// <summary>
        /// 记录功能名称数


        /// </summary>
        public string FunctionName
        {
            get { return _FunctionName; }
            set { _FunctionName = value; }
        }

        private string _TransactionCode;
        /// <summary>
        /// 接口交易码


        /// </summary>
        public string TransactionCode
        {
            get { return _TransactionCode; }
            set { _TransactionCode = value; }
        }

        private long _SequenceNumber;
        /// <summary>
        /// 调用顺序号


        /// </summary>
        public long SequenceNumber
        {
            get { return _SequenceNumber; }
            set { _SequenceNumber = value; }
        }
        private string _ExternalName;
        /// <summary>
        /// 连接的外部系统名称


        /// </summary>
        public string ExternalName
        {
            get { return _ExternalName; }
            set { _ExternalName = value; }
        }
        private string _InterfaceCode;
        /// <summary>
        /// 接口功能名称及接口代码


        /// </summary>
        public string InterfaceCode
        {
            get { return _InterfaceCode; }
            set { _InterfaceCode = value; }
        }

        private string _InterfaceMode;
        /// <summary>
        /// 接口方式
        /// </summary>
        public string InterfaceMode
        {
            get { return _InterfaceMode; }
            set { _InterfaceMode = value; }
        }
    }
    #endregion
}
