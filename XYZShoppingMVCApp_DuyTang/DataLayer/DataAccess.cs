using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYZShoppingMVCApp_DuyTang.DataLayer
{
    public class DataAccess: IDataAccess
    {
        /*Properties*/
        string ConnectionString = ConfigurationManager.ConnectionStrings["XYZEVEDSN"].ConnectionString;
        /*Constructor*/
        public DataAccess() { }

        public DataAccess(string connstr) // to be able to change the connectionstring
        {
            this.ConnectionString = connstr;
        }
        /*Methods*/
        public  DataTable GetRowsCols(string sql, List<DbParameter> PList,
            SqlConnection connc = null, SqlTransaction sqtr = null, bool IsStoredProc = false)
        {
            SqlConnection conn;
            //creating connection
            if (sqtr == null)
            {
                conn = new SqlConnection(ConnectionString);
            }
            else
            {
                conn = connc;
            }
            DataTable dt = new DataTable();
            try
            {
                if (sqtr == null)
                {
                    conn.Open();
                }
                SqlCommand cmd = new SqlCommand(sql, conn);
                // bound to a transaction object
                if (sqtr != null)
                {
                    cmd.Transaction = sqtr;
                }
                if (IsStoredProc == true)
                    cmd.CommandType = CommandType.StoredProcedure;
                //adding parameters to the command
                if (PList != null)
                {
                    foreach (DbParameter sp in PList)
                    {
                        cmd.Parameters.Add(sp);
                    }
                }
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(dt);

            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                if (sqtr == null)
                {
                    conn.Close();
                }
            }
            return dt;
        }

        public  object GetSingleRow(string sql, List<DbParameter> PList,
            SqlConnection connc = null, SqlTransaction sqtr = null, bool IsStoredProc = false)
        {
            SqlConnection conn;
            //creating connection
            if (sqtr == null)
            {
                conn = new SqlConnection(ConnectionString);
            }
            else
            {
                conn = connc;
            }
            object obj = null;
            try
            {
                if (sqtr == null)
                {
                    conn.Open();
                }
                SqlCommand cmd = new SqlCommand(sql, conn);
                // bound to a transaction object
                if (sqtr != null)
                {
                    cmd.Transaction = sqtr;
                }
                if (IsStoredProc == true)
                    cmd.CommandType = CommandType.StoredProcedure;
                //adding parameters to the command
                if (PList != null)
                {
                    foreach (DbParameter sp in PList)
                    {
                        cmd.Parameters.Add(sp);
                    }
                }
                obj = cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                if (sqtr == null)
                {
                    conn.Close();
                }
            }
            return obj;
        }
        public  int InsertUpdateDelete(string sql, List<DbParameter> PList,
            SqlConnection connc = null, SqlTransaction sqtr = null, bool IsStoredProc = false)
        {
            SqlConnection conn;
            //creating connection
            if (sqtr == null)
            {
                conn = new SqlConnection(ConnectionString);
            }
            else
            {
                conn = connc;
            }
            int rows = 0;
            try
            {
                if (sqtr == null)
                {
                    conn.Open();
                }
                SqlCommand cmd = new SqlCommand(sql, conn);
                if (sqtr != null)
                {
                    cmd.Transaction = sqtr;
                }
                if (IsStoredProc == true)
                    cmd.CommandType = CommandType.StoredProcedure;
                //adding parameters to the command
                if (PList != null)
                {
                    foreach (DbParameter sp in PList)
                    {
                        cmd.Parameters.Add(sp);
                    }
                }
                rows = cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                if (sqtr == null)
                {
                    conn.Close();
                }
            }
            return rows;
        }
    }
}
