using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace Service.WebApi
{
    class SQLHelper
    {
        private SqlConnection con;

        /// <summary>
        /// 构造函数，调用方法：SQLHelper helper = new SQLHelper("DatabaseConnectionString");
        /// </summary>
        /// <param name="_db">传入数据库信息</param>
        public SQLHelper(string _db)
        {
            string datebase = _db;
            string connectionString = ConfigurationManager.AppSettings[datebase].ToString();
            //ConfigurationManager.ConnectionStrings[datebase].ConnectionString;
            ///创建连接
            con = new SqlConnection(connectionString);
            try
            {   ///打开连接
                con.Open();
            }
            catch
            {

            }

        }

        /// <summary>
        /// 查询SQL语句，返回DataSet类型数据，使用方法：DataSet ds = helper.GetDataSet("Select * from EDB_TEST1");
        /// </summary>
        /// <param name="sql">传入SQL语句</param>
        /// <returns></returns>
        public DataSet GetDataSet(string sql)
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            DataSet ds = new DataSet();
            try
            {
                da.Fill(ds, "DataTable");
            }
            catch
            {

            }
            finally
            {   ///关闭连接
                con.Close();
            }
            return ds;
        }

        /// <summary>
        /// 查询SQL语句，返回DataTable类型数据，使用方法：DataTable ds = helper.GetDataTable("Select * from EDB_TEST1");
        /// </summary>
        /// <param name="sql">传入SQL语句</param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql)
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(da);
            DataTable dt = new DataTable("DefaultTable");
            try
            {
                da.Fill(dt);
            }
            catch
            {

            }
            finally
            {   ///关闭连接
                con.Close();
            }
            return dt;
        }

        /// <summary>
        /// 查询存储过程，返回DataTable类型数据，使用方法：DataTable taskData = helper.ExcuStoredProcedure("CMHI_MKT_003", parameter);
        /// </summary>
        /// <param name="storedProcedure">存储过程名称</param>
        /// <param name="parameter">传递的参数</param>
        /// <returns></returns>
        public DataTable ExcuStoredProcedure(string storedProcedure, SqlParameter[] parameter)
        {
            SqlCommand cmd = new SqlCommand(storedProcedure, con);
            cmd.CommandType = CommandType.StoredProcedure;//存储过程
            cmd.CommandTimeout = 1000 * 60 * 3;
            for (int i = 0; i < parameter.Length; i++)
            {
                cmd.Parameters.Add(parameter[i]);
            }
            DataTable dt = new DataTable("DefaultTable");
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            try
            {
                da.Fill(dt);
            }
            catch
            {

            }
            finally
            {   ///关闭连接
                con.Close();
            }
            return dt;
        }

        /// <summary>
        /// 查询存储过程，返回DataTable类型数据，使用方法：DataTable taskData = helper.ExcuStoredProcedure("CMHI_PBLC_020");
        /// </summary>
        /// <param name="storedProcedure">存储过程名称</param>
        /// <returns></returns>
        public DataTable ExcuStoredProcedure(string storedProcedure)
        {
            SqlCommand cmd = new SqlCommand(storedProcedure, con);
            cmd.CommandType = CommandType.StoredProcedure;//存储过程
            cmd.CommandTimeout = 1000 * 60 * 3;
            DataTable dt = new DataTable("DefaultTable");
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            try
            {
                da.Fill(dt);
            }
            catch
            {

            }
            finally
            {   ///关闭连接
                con.Close();
            }
            return dt;
        }

        /// <summary>
        /// 执行SQL语句，返回int类型数值表示成功条数
        /// </summary>
        /// <param name="sql">传递SQL语句</param>
        /// <param name="parameter">传递的参数</param>
        /// <returns></returns>
        public int Execute(string sql, SqlParameter[] parameter)
        {
            SqlCommand cmd = new SqlCommand(sql, con);
            ///创建参数并赋值
            for (int i = 0; i < parameter.Length; i++)
            {
                cmd.Parameters.Add(parameter[i]);
            }
            int result = -1;
            try
            {
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {   ///关闭连接
                con.Close();
            }
            return result;
        }

        public int DeleteInfo(int messageID)
        {	///执行SQL删除语句，返回int类型数值表示成功条数，★请修改其中内容后再进行调用★
            string cmdText = "DELETE tel WHERE ID = @ID";
            ///创建SqlCommand
            SqlCommand cmd = new SqlCommand(cmdText, con);
            ///创建参数并赋值
            cmd.Parameters.Add("@ID", SqlDbType.Int, 4);
            cmd.Parameters[0].Value = messageID;

            int result = -1;
            try
            {
                result = cmd.ExecuteNonQuery();
            }
            catch
            {   ///抛出异常

            }
            finally
            {   ///关闭连接
                con.Close();
            }
            return result;
        }
    }
}
