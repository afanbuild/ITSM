/****************************************************************************
 * 
 * description:问题管理数据处理类
 * 
 * 
 * 
 * Create by:zhumingchun
 * Create Date:2007-06-23
 * *************************************************************************/
using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;
using EpowerGlobal;
using System.Xml;
using System.IO;
using System.Text;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// 
    /// </summary>
    public class ProblemDealEntity
    {
        public ProblemDealEntity()
		{ }
        #region model
        private decimal _problem_id;
		private decimal _flowid;
		private decimal _nodemodelid;
		private decimal _flowmodelid;
		private decimal _problem_type;
		private decimal _problem_level;
		private string _problem_subject;
		private decimal _state;
		private string _problem_title;
		private string _remark;
        private decimal _deleted;
		private decimal _reguserid;
		private string _regusername;
		private decimal _regdeptid;
		private string _regdeptname;
		private DateTime _regtime;
		/// <summary>
		/// 
		/// </summary>
		public decimal Problem_ID
		{
			set{ _problem_id=value;}
			get{return _problem_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal FlowID
		{
			set{ _flowid=value;}
			get{return _flowid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal NodeModelID
		{
			set{ _nodemodelid=value;}
			get{return _nodemodelid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal FlowModelID
		{
			set{ _flowmodelid=value;}
			get{return _flowmodelid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal Problem_Type
		{
			set{ _problem_type=value;}
			get{return _problem_type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal Problem_Level
		{
			set{ _problem_level=value;}
			get{return _problem_level;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Problem_Subject
		{
			set{ _problem_subject=value;}
			get{return _problem_subject;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal State
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Problem_Title
		{
			set{ _problem_title=value;}
			get{return _problem_title;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
        /// <summary>
        /// 
        /// </summary>
        public decimal Deleted
        {
            set { _deleted = value; }
            get { return Deleted; }
        }
		/// <summary>
		/// 
		/// </summary>
		public decimal RegUserID
		{
			set{ _reguserid=value;}
			get{return _reguserid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RegUserName
		{
			set{ _regusername=value;}
			get{return _regusername;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal RegDeptID
		{
			set{ _regdeptid=value;}
			get{return _regdeptid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RegDeptName
		{
			set{ _regdeptname=value;}
			get{return _regdeptname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime RegTime
		{
			set{ _regtime=value;}
			get{return _regtime;}
        }
        #endregion 

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(decimal Problem_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Pro_ProblemDeal where Problem_ID=" + Problem_ID + "");
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                object obj = OracleDbHelper.ExecuteScalar(cn,CommandType.Text,strSql.ToString());
                int cmdresult;
                if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                {
                    cmdresult = 0;
                }
                else
                {
                    cmdresult = int.Parse(obj.ToString());
                }
                if (cmdresult == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public decimal Add(ProblemDealEntity model)
        {
            model.Problem_ID = EPGlobal.GetNextID("ProblemDealID");
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Pro_ProblemDeal(");
            strSql.Append("Problem_ID,FlowID,NodeModelID,FlowModelID,Problem_Type,Problem_Level,Problem_Subject,State,Problem_Title,Remark,Deleted,RegUserID,RegUserName,RegDeptID,RegDeptName,RegTime");
            strSql.Append(")");
            strSql.Append(" values (");
            strSql.Append("" + model.Problem_ID + ",");
            strSql.Append("" + model.FlowID + ",");
            strSql.Append("" + model.NodeModelID + ",");
            strSql.Append("" + model.FlowModelID + ",");
            strSql.Append("" + model.Problem_Type + ",");
            strSql.Append("" + model.Problem_Level + ",");
            strSql.Append("" + StringTool.SqlQ(model.Problem_Subject) + ",");
            strSql.Append("" + model.State + ",");
            strSql.Append("'" + model.Problem_Title + "',");
            strSql.Append("'" + model.Remark + "',");
            strSql.Append("" + ((int)eRecord_Status.eNormal).ToString() + ",");
            strSql.Append("" + model.RegUserID + ",");
            strSql.Append("" + StringTool.SqlQ(model.RegUserName) + ",");
            strSql.Append("" + model.RegDeptID + ",");
            strSql.Append("'" + model.RegDeptName + "',");
            strSql.Append("'" + model.RegTime + "'");
            strSql.Append(")");
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSql.ToString());
                return model.Problem_ID;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ProblemDealEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Pro_ProblemDeal set ");
            strSql.Append("FlowID=" + model.FlowID + ",");
            strSql.Append("NodeModelID=" + model.NodeModelID + ",");
            strSql.Append("FlowModelID=" + model.FlowModelID + ",");
            strSql.Append("Problem_Type=" + model.Problem_Type + ",");
            strSql.Append("Problem_Level=" + model.Problem_Level + ",");
            strSql.Append("Problem_Subject=" + StringTool.SqlQ(model.Problem_Subject) + ",");
            strSql.Append("State=" + model.State + ",");
            strSql.Append("Problem_Title='" + model.Problem_Title + "',");
            strSql.Append("Remark=" +StringTool.SqlQ(model.Remark));
            strSql.Append("Deleted=" +model.Deleted);
            strSql.Append(" where Problem_ID=" + model.Problem_ID + "");
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSql.ToString());
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(ProblemDealEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Pro_ProblemDeal set ");
            strSql.Append(" Deleted=" + model.Deleted);
            strSql.Append(" where Problem_ID=" + model.Problem_ID);
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSql.ToString());
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ProblemDealEntity(decimal Problem_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Pro_ProblemDeal ");
            strSql.Append(" where Problem_ID=" + Problem_ID);

            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                DataSet ds = OracleDbHelper.ExecuteDataset(cn, CommandType.Text, strSql.ToString());
                this.Problem_ID = Problem_ID;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["FlowID"].ToString() != "")
                    {
                        this.FlowID = decimal.Parse(ds.Tables[0].Rows[0]["FlowID"].ToString());
                    }
                    if (ds.Tables[0].Rows[0]["NodeModelID"].ToString() != "")
                    {
                        this.NodeModelID = decimal.Parse(ds.Tables[0].Rows[0]["NodeModelID"].ToString());
                    }
                    if (ds.Tables[0].Rows[0]["FlowModelID"].ToString() != "")
                    {
                        this.FlowModelID = decimal.Parse(ds.Tables[0].Rows[0]["FlowModelID"].ToString());
                    }
                    if (ds.Tables[0].Rows[0]["Problem_Type"].ToString() != "")
                    {
                        this.Problem_Type = decimal.Parse(ds.Tables[0].Rows[0]["Problem_Type"].ToString());
                    }
                    if (ds.Tables[0].Rows[0]["Problem_Level"].ToString() != "")
                    {
                        this.Problem_Level = decimal.Parse(ds.Tables[0].Rows[0]["Problem_Level"].ToString());
                    }
                    this.Problem_Subject = ds.Tables[0].Rows[0]["Problem_Subject"].ToString();
                    if (ds.Tables[0].Rows[0]["State"].ToString() != "")
                    {
                        this.State = decimal.Parse(ds.Tables[0].Rows[0]["State"].ToString());
                    }
                    this.Problem_Title = ds.Tables[0].Rows[0]["Problem_Title"].ToString();
                    this.Remark = ds.Tables[0].Rows[0]["Remark"].ToString();
                    if (ds.Tables[0].Rows[0]["RegUserID"].ToString() != "")
                    {
                        this.RegUserID = decimal.Parse(ds.Tables[0].Rows[0]["RegUserID"].ToString());
                    }
                    this.RegUserName = ds.Tables[0].Rows[0]["RegUserName"].ToString();
                    if (ds.Tables[0].Rows[0]["RegDeptID"].ToString() != "")
                    {
                        this.RegDeptID = decimal.Parse(ds.Tables[0].Rows[0]["RegDeptID"].ToString());
                    }
                    this.RegDeptName = ds.Tables[0].Rows[0]["RegDeptName"].ToString();
                    if (ds.Tables[0].Rows[0]["RegTime"].ToString() != "")
                    {
                        this.RegTime = DateTime.Parse(ds.Tables[0].Rows[0]["RegTime"].ToString());
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }
    }
}
