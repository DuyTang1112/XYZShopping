using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace XYZShoppingMVCApp_DuyTang.DataLayer
{
    public class DBHelper
    {
        // add a paramter to a list of sql parameters
        public static void AddSqlParam(List<DbParameter> PList, string paramName,
               SqlDbType paramType, object paramValue, int size = 0)
        {
            DbParameter sp = null;
            // for varchar datatype
            if (size == 0)
                sp = new SqlParameter(paramName, paramType);
            else
                sp = new SqlParameter(paramName, paramType, size);
            sp.Value = paramValue;
            PList.Add(sp);
        }
    }
}