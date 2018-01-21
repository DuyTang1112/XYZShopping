using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYZShoppingMVCApp_DuyTang.DataLayer
{
    public interface IDataAccess
    {
        // transaction capable methods, last three parametes of Dbconnection, DbTransaction, and bTransaction are optional
        object GetSingleRow(string sql, List<DbParameter> PList,
            SqlConnection connc = null, SqlTransaction sqtr = null, bool IsStoredProc = false);

        DataTable GetRowsCols(string sql, List<DbParameter> PList,
            SqlConnection connc = null, SqlTransaction sqtr = null, bool IsStoredProc = false);

        int InsertUpdateDelete(string sql, List<DbParameter> PList,
            SqlConnection connc = null, SqlTransaction sqtr = null, bool IsStoredProc = false);
    }
}
