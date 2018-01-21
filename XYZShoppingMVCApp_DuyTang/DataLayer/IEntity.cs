using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYZShoppingMVCApp_DuyTang.DataLayer
{
    public interface IEntity
    {
        void SetFields(DataRow dr);
    }
}
